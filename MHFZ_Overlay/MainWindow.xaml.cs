using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MHFZ_Overlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataLoader DataLoader { get; set; } = new();

        private bool isVisible = true;

        private void ToggleVisibility()
        {
            if (isVisible)
            {
                OpenConfigButton.Visibility = Visibility.Hidden;
                ReloadButton.Visibility = Visibility.Hidden;
                CloseButton.Visibility = Visibility.Hidden;
            } else {
                OpenConfigButton.Visibility = Visibility.Visible;
                ReloadButton.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Visible;
            }
            isVisible = !isVisible;
        }

        public MainWindow()
        {
            InitializeComponent();
            Left = 0;
            Top = 0;
            Topmost = true;
            DispatcherTimer timer = new();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 30);
            timer.Tick += Timer_Tick;
            timer.Start();
            DataContext = DataLoader.model;
            GlobalHotKey.RegisterHotKey("Shift + Insert", () => ToggleVisibility()); 

        }


        int counter = 0;
        int prevTime = 0;

        public void Timer_Tick(object? obj, EventArgs e)
        {
            HideMonsterInfoWhenNotInQuest();
            DataLoader.model.ReloadData();
            Monster1HPBar.ReloadData();
            Monster2HPBar.ReloadData();
            Monster3HPBar.ReloadData();
            Monster4HPBar.ReloadData();
            MonsterPoisonBar.ReloadData();
            MonsterSleepBar.ReloadData();
            MonsterParaBar.ReloadData();
            MonsterBlastBar.ReloadData();
            MonsterStunBar.ReloadData();

            CreateDamageNumber();

            // Debug.WriteLine("Monster1 SEL=" + DataLoader.model.RoadSelectedMonster() + " ID=" + DataLoader.model.LargeMonster1ID() + " HP=" + DataLoader.model.Monster1HPInt() + "/" + DataLoader.model.SavedMonster1MaxHP);
            //Debug.WriteLine("Monster2 SEL=" + DataLoader.model.RoadSelectedMonster() + " ID=" + DataLoader.model.LargeMonster2ID() + " HP=" + DataLoader.model.Monster2HPInt() + "/" + DataLoader.model.SavedMonster2MaxHP);
        }

        int curNum = 0;
        int prevNum = 0;
        bool isFirstAttack = false;
        public bool IsDragConfigure { get; set; } = false;

        private void CreateDamageNumber()
        {
            int damage = 0;
            if (DataLoader.model.HitCountInt() == 0)
            {
                curNum = 0;
                prevNum = 0;
                isFirstAttack = true;
            }
            else
            {
                damage = DataLoader.model.DamageDealt();
            }

            if (prevNum != damage)
            {
                curNum = damage - prevNum;
                if (isFirstAttack)
                {
                    isFirstAttack = false;
                    CreateDamageNumberLabel(damage);
                }
                else if (curNum < 0)
                {
                    curNum = 1000 + curNum;
                    CreateDamageNumberLabel(curNum);
                }
                else
                {
                    if (curNum != damage)
                    {
                        CreateDamageNumberLabel(curNum);
                    }
                }
            }
            prevNum = damage;
        }

        private void CreateDamageNumberLabel(int damage)
        {
            System.Random random = new();
            double x = random.Next(450);
            double y = random.Next(254);
            Point newPoint = DamageNumbers.TranslatePoint(new Point(x, y), DamageNumbers);
            Label tb = new();
            tb.Content = damage.ToString();
            tb.Foreground = Brushes.Orange;
            tb.FontSize = 30;
            tb.SetValue(Canvas.TopProperty, newPoint.Y);
            tb.SetValue(Canvas.LeftProperty, newPoint.X);
            DamageNumbers.Children.Add(tb);
            RemoveDamageNumberLabel(tb);

        }

        private void RemoveDamageNumberLabel(Label tb)
        {
            DispatcherTimer timer = new();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += (o, e) => { DamageNumbers.Children.Remove(tb); };
            timer.Start();
        }

        private void HideMonsterInfoWhenNotInQuest()
        {
            int time = DataLoader.model.TimeInt();
            if (prevTime == time)
                counter++;
            else
                counter = 0;
            prevTime = time;
            Settings s = (Settings)Application.Current.FindResource("Settings");
            bool v = s.AlwaysShowMonsterInfo || DataLoader.model.Configuring || counter < 60;
            DataLoader.model.ShowMonsterInfos = v && s.MonsterStatusInfoShown;
            DataLoader.model.ShowMonsterHPBars = v && s.HealthBarsShown;
        }



        #region DragAndDrop

        private double? XOffset;

        private double? YOffset;

        private FrameworkElement? MovingObject;

        private void MainGrid_DragOver(object sender, DragEventArgs e)
        {
            if (MovingObject == null)
                return;
            Point pos = e.GetPosition(this);
            if (XOffset == null || YOffset == null)
                return;
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            switch (MovingObject.Name)
            {
                case "PlayerInfo":
                    s.PlayerInfoX = (double)(pos.X - XOffset);
                    s.PlayerInfoY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterHpBars":
                    s.HealthBarsX = (double)(pos.X - XOffset);
                    s.HealthBarsY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterStatusInfo":
                    s.MonsterStatusInfoX = (double)(pos.X - XOffset);
                    s.MonsterStatusInfoY = (double)(pos.Y - YOffset);
                    break;
            }

        }

        private void DoDragDrop(FrameworkElement? item)
        {
            if (item == null)
                return;
            DragDrop.DoDragDrop(item, new DataObject(DataFormats.Xaml, item), DragDropEffects.Move);
        }

        private void MainGrid_Drop(object sender, DragEventArgs e)
        {
            if (MovingObject != null)
                MovingObject.IsHitTestVisible = true;
            MovingObject = null;
        }


        private void ElementMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsDragConfigure)
                return;
            MovingObject = (FrameworkElement)sender;
            Point pos = e.GetPosition(this);
            XOffset = pos.X - Canvas.GetLeft(MovingObject);
            YOffset = pos.Y - Canvas.GetTop(MovingObject);
            MovingObject.IsHitTestVisible = false;
        }
        #endregion DragAndDrop

        #region clickbuttons
        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            System.Windows.Forms.Application.Restart();
        }

        ConfigWindow? configWindow;

        private void OpenConfigButton_Click(object sender, RoutedEventArgs e)
        {
            if (configWindow == null || !configWindow.IsLoaded)
                configWindow = new(this);
            configWindow.Show();
            DataLoader.model.Configuring = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            DoDragDrop(MovingObject);
        }

        public void EnableDragAndDrop()
        {
            IsDragConfigure = true;
            ExitDragAndDrop.Visibility = Visibility.Visible;
            MainGrid.Background = (Brush?)new BrushConverter().ConvertFrom("#01000000");
            if (configWindow != null)
                configWindow.Visibility = Visibility.Hidden;
        }

        public void DisableDragAndDrop()
        {
            IsDragConfigure = false;
            ExitDragAndDrop.Visibility = Visibility.Hidden;
            MainGrid.Background = (Brush?)new BrushConverter().ConvertFrom("#00FFFFFF");
            if (configWindow != null)
                configWindow.Visibility = Visibility.Visible;
        }

        private void ExitDragAndDrop_Click(object sender, RoutedEventArgs e)
        {
            DisableDragAndDrop();
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DisableDragAndDrop();
        }
        #endregion clickbuttons
    }
}
/// <TODO>
/// ## Look at hp bar to make better
/// ## damage numbers
/// ## look at other popular overlays and steal their design
/// fix road stuff
/// ## implement for monsters 1-4
/// select monster 
/// ## configuration
/// move data translation into dataloder out of the abstract address model
/// ## remove unnecesary fields in DataLoader
/// figure out way to make it work for all monsters with the same functions (use list u dunmbass)
/// ## figure out a way to make templates
/// bodyparts
/// ## status panel
/// ## maybe more ....
/// Tooltips for Configuration
/// Configuration for Damage numbers
/// </TODO>
