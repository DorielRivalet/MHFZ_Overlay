using MHFZ_Overlay.addresses;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Application = System.Windows.Application;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using Label = System.Windows.Controls.Label;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace MHFZ_Overlay
{
    /// <summary>
    /// Create a Value Converter to disable the Up & Down Arrow buttons of the scrollbar
    /// when the Thumb reaches the minimum & maximum position on the scroll track.
    /// </summary>

    public class ScrollLimitConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double && values[1] is double)
            {
                return (double)values[0] == (double)values[1];
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataLoader DataLoader { get; set; } = new();

        private int originalStyle = 0;

        //https://stackoverflow.com/questions/2798245/click-through-in-c-sharp-form
        //https://stackoverflow.com/questions/686132/opening-a-form-in-c-sharp-without-focus/10727337#10727337
        //https://social.msdn.microsoft.com/Forums/en-us/a5e3cbbb-fd07-4343-9b60-6903cdfeca76/click-through-window-with-image-wpf-issues-httransparent-isnt-working?forum=csharplanguage
        protected override void OnSourceInitialized(EventArgs e)
        {
            // Get this window's handle         
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            // Change the extended window style to include WS_EX_TRANSPARENT         
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            if (originalStyle == 0)
            {
                originalStyle = extendedStyle;
            }
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            base.OnSourceInitialized(e);
        }

        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

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
                //GlobalHotKey.RegisterHotKey("Shift + Insert", () => ToggleVisibility());
                GlobalHotKey.RegisterHotKey("Shift + F1", () => OpenConfigButton_Key());
                GlobalHotKey.RegisterHotKey("Shift + F5", () => ReloadButton_Key());
                GlobalHotKey.RegisterHotKey("Shift + F6", () => CloseButton_Key());
                OpenConfigButton.Visibility = Visibility.Hidden;
                ReloadButton.Visibility = Visibility.Hidden;
                CloseButton.Visibility = Visibility.Hidden;
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

        public bool ShowDamageNumbersMulticolor()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableDamageNumbersMulticolor == true)
                return true;
            else
                return false;
        }

        private void CreateDamageNumberLabel(int damage)
        {
            Random random = new();
            double x = random.Next(450);
            double y = random.Next(254);
            Point newPoint = DamageNumbers.TranslatePoint(new Point(x, y), DamageNumbers);
            Label damageLabel = new()
            {
                Content = damage.ToString(),
                FontFamily = new System.Windows.Media.FontFamily("MS Gothic Bold")
                //BorderBrush = System.Windows.Media.Brushes.Black,
                //BorderThickness = new Thickness(2.0)
                
                //BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };
            //damageLabel.
            //        void CreateLabel(int damage)
            //{
            //    Label namelabel = new Label();
            //    Random rnd = new Random();
            //    int x = rnd.Next(centerx, centerx + width);
            //    int y = rnd.Next(centery, centery + height);
            //    namelabel.Location = new Point(x, y);
            //    namelabel.BringToFront();
            //    namelabel.Text = damage.ToString();
            //    namelabel.Font = new Font(textfont, damageTextSize);
            //    namelabel.ForeColor = Color.FromName(textcolor);
            //    namelabel.BackColor = Color.Transparent;
            //    namelabel.AutoSize = true;
            //    this.Controls.Add(namelabel);
            //    DeleteLabel(namelabel);
            //}

            //does not alter actual number displayed, only the text style
            double damageModifier = damage / (DataLoader.model.CurrentWeaponMultiplier / 2);

            switch (damageModifier)
            {
                case < 15.0:
                    damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xB4, 0xBE, 0xFE));
                    damageLabel.FontSize = 22;
                    break;
                case < 35.0:
                    damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x89, 0xB4, 0xFA));
                    damageLabel.FontSize = 22;
                    break;
                case < 75.0:
                    damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0xC7, 0xEC));
                    damageLabel.FontSize = 22;
                    break;
                case < 200.0:
                    damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x89, 0xDC, 0xEB));
                    damageLabel.FontSize = 22;
                    break;
                case < 250.0:
                    damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x94, 0xE2, 0xD5));
                    damageLabel.FontSize = 24;
                    break;
                case < 300.0:
                    damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xF9, 0xE2, 0xAF));
                    damageLabel.FontSize = 24;
                    break;
                case < 350.0:
                    damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xB3, 0x97));
                    damageLabel.FontSize = 24;
                    damageLabel.Content += "!";
                    break;
                case < 500.0:
                    damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xEB, 0xA0, 0xAC));
                    damageLabel.FontSize = 26;
                    damageLabel.Content += "!!";
                    break;
                default:
                    damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xF3, 0x8B, 0xA8));
                    damageLabel.FontSize = 30;
                    damageLabel.Content += "!!!";
                    break;
            }

            if (!ShowDamageNumbersMulticolor()) 
            {
                //damageLabel.Foreground = Brushes.Orange;
                //https://stackoverflow.com/questions/14601759/convert-color-to-byte-value
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                System.Drawing.Color color = ColorTranslator.FromHtml(s.DamageNumbersColor);
                damageLabel.Foreground = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }

            damageLabel.SetValue(Canvas.TopProperty, newPoint.Y);
            damageLabel.SetValue(Canvas.LeftProperty, newPoint.X);

            //Point newPoint2 = DamageNumbers.TranslatePoint(new Point(x, newPoint.Y), DamageNumbers);
            //damageLabelBorder1.SetValue(Canvas.TopProperty, newPoint.Y - 1);
            //damageLabelBorder1.SetValue(Canvas.LeftProperty, newPoint.X - 1);

            DamageNumbers.Children.Add(damageLabel);

            RemoveDamageNumberLabel(damageLabel);
            

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

        private void ReloadButton_Key()
        {
            Application.Current.Shutdown();
            System.Windows.Forms.Application.Restart();
        }

        private void OpenConfigButton_Key()
        {
            if (IsDragConfigure) return;
            if (configWindow == null || !configWindow.IsLoaded)
                configWindow = new(this);
            configWindow.Show();
            DataLoader.model.Configuring = true;
        }

        private void CloseButton_Key()
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
            ToggleClickThrough();

        }

        private bool ClickThrough = true;

        private void ToggleClickThrough()
        {
            if(ClickThrough == false) 
            {
                IsHitTestVisible = false;
                Focusable = false;
                // Get this window's handle         
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                // Change the extended window style to include WS_EX_TRANSPARENT         
                int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);

            } else
            {
                IsHitTestVisible = true;
                Focusable = true;
                // Get this window's handle         
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                // Change the extended window style to include WS_EX_TRANSPARENT         
                SetWindowLong(hwnd, GWL_EXSTYLE, originalStyle);

            }
            ClickThrough = !ClickThrough;
        }

        public void DisableDragAndDrop()
        {
            IsDragConfigure = false;
            ExitDragAndDrop.Visibility = Visibility.Hidden;
            MainGrid.Background = (Brush?)new BrushConverter().ConvertFrom("#00FFFFFF");
            if (configWindow != null)
                configWindow.Visibility = Visibility.Visible;
            ToggleClickThrough();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
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
