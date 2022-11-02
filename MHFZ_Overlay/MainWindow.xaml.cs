using DiscordRPC;
using Memory;
using MHFZ_Overlay.addresses;
using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Application = System.Windows.Application;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using Label = System.Windows.Controls.Label;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;
//using static System.Globalization.CultureInfo;


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

        #region click through

        private int originalStyle = 0;

        //https://stackoverflow.com/questions/2798245/click-through-in-c-sharp-form
        //https://stackoverflow.com/questions/686132/opening-a-form-in-c-sharp-without-focus/10727337#10727337
        //https://social.msdn.microsoft.com/Forums/en-us/a5e3cbbb-fd07-4343-9b60-6903cdfeca76/click-through-window-with-image-wpf-issues-httransparent-isnt-working?forum=csharplanguage        
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.SourceInitialized" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
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
        //set version here
        public const string CurrentProgramVersion = "v0.4.0";

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        #endregion

        #region discord rpc

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public DiscordRpcClient Client { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [show discord RPC].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show discord RPC]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowDiscordRPC
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                if (s.EnableRichPresence == true)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets the discord client identifier.
        /// </summary>
        /// <value>
        /// The discord client identifier.
        /// </value>
        public string GetDiscordClientID
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                if (s.DiscordClientID.Length == 19)
                    return s.DiscordClientID;
                else
                    return "";
            }
        }

        /// <summary>
        /// Gets the discord server invite.
        /// </summary>
        /// <value>
        /// The discord server invite.
        /// </value>
        public string GetDiscordServerInvite
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                if (s.DiscordServerInvite.Length >= 8)
                    return s.DiscordServerInvite;
                else
                    return "";
            }
        }

        /// <summary>
        /// Gets the name of the hunter.
        /// </summary>
        /// <value>
        /// The name of the hunter.
        /// </value>
        public string GetHunterName
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                if (s.HunterName.Length >= 1)
                    return s.HunterName;
                else
                    return "Hunter Name";
            }
        }

        /// <summary>
        /// Gets the name of the guild.
        /// </summary>
        /// <value>
        /// The name of the guild.
        /// </value>
        public string GetGuildName
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                if (s.GuildName.Length >= 1)
                    return s.GuildName;
                else
                    return "Guild Name";
            }
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        void Setup()
        {
            Client = new DiscordRpcClient(GetDiscordClientID);  //Creates the client
            Client.Initialize();                            //Connects the client
        }

        //Dispose client        
        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        void Cleanup()
        {
            if (Client != null)//&& ShowDiscordRPC)
            {
                Client.Dispose();
            }

        }

        /// <summary>
        /// The current presence to send to discord.
        /// </summary>
        public static RichPresence presenceTemplate = new RichPresence()
        {
            Details = "【MHF-Z】Overlay "+CurrentProgramVersion,
            State = "Loading...",
            //check img folder
            Assets = new Assets()
            {
                LargeImageKey = "cattleya",
                LargeImageText = "Please Wait",
                SmallImageKey = "https://i.imgur.com/9OkLYAz.png",
                SmallImageText = "Hunter Name | Guild Name"
            },
            Buttons = new DiscordRPC.Button[]
                {
                    new DiscordRPC.Button() {Label = "【MHF-Z】Overlay "+CurrentProgramVersion, Url = "https://github.com/DorielRivalet/MHFZ_Overlay"},
                    new DiscordRPC.Button() { Label = "Discord RPC C# Dev Site", Url = "https://lachee.dev/" }
                }
        };

        /// <summary>
        /// Is the main loop currently running?
        /// </summary>
        public static bool isDiscordRPCRunning = false;

        /// <summary>
        /// Initializes the discord RPC.
        /// </summary>
        private void InitializeDiscordRPC()
        {
            if (isDiscordRPCRunning)
                return;

            if (ShowDiscordRPC && GetDiscordClientID != "")
            {
                Setup();

                //Set Presence
                presenceTemplate.Timestamps = Timestamps.Now;

                if (GetHunterName != "" && GetGuildName != "")
                {
                    presenceTemplate.Assets = new Assets()
                    {
                        LargeImageKey = "cattleya",
                        LargeImageText = "Please Wait",
                        SmallImageKey = "https://i.imgur.com/9OkLYAz.png",
                        SmallImageText = GetHunterName + " | " + GetGuildName
                    };
                }

                //if (GetDiscordServerInvite != "")
                //{
                //should work fine
                presenceTemplate.Buttons = new DiscordRPC.Button[] { }; ;
                presenceTemplate.Buttons = new DiscordRPC.Button[]
                {
                    new DiscordRPC.Button() {Label = "【MHF-Z】Overlay "+CurrentProgramVersion, Url = "https://github.com/DorielRivalet/MHFZ_Overlay"},
                    new DiscordRPC.Button() { Label = "Discord RPC C# Dev Site", Url = "https://lachee.dev/" }
                };
                //}

                if (GetDiscordServerInvite != "")
                {
                    presenceTemplate.Buttons = new DiscordRPC.Button[] { }; ;
                    presenceTemplate.Buttons = new DiscordRPC.Button[]
                    {
                    new DiscordRPC.Button() {Label = "【MHF-Z】Overlay "+CurrentProgramVersion, Url = "https://github.com/DorielRivalet/MHFZ_Overlay"},
                    new DiscordRPC.Button() { Label = "Join Discord Server", Url = String.Format("https://discord.com/invite/{0}",GetDiscordServerInvite)}
                    };
                }

                Client.SetPresence(presenceTemplate);
                isDiscordRPCRunning = true;
            }
        }

        #endregion

        #region main

        public bool itemsLoaded = false;
        public bool questsLoaded = false;
        public bool gearLoaded = false;

        /// <summary>
        /// Loads the dictionaries
        /// </summary>
        private void LoadDictionaries()
        {
            if (!(questsLoaded && ShowDiscordQuestNames))
            {
                questsLoaded = true;
                Dictionary.Quests.Initiate();
            }

            if (!(itemsLoaded))
            {
                itemsLoaded = true;
                //load item list
                Dictionary.Items.initiate();
            }

            if (!(gearLoaded))
            {
                gearLoaded = true;
                //load all gear lists
                Dictionary.MeleeWeapons.Initiate();
                Dictionary.RangedWeapons.Initiate();
                Dictionary.ArmorHeads.Initiate();
                Dictionary.ArmorChests.Initiate();
                Dictionary.ArmorArms.Initiate();
                Dictionary.ArmorWaists.Initiate();
                Dictionary.ArmorLegs.Initiate();
            }
        }



        //Main entry point?        
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
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



            ////Main Loop
            //Setting a random details to test the update rate of the presence
            //Program.startRichPresence("");
            //Issue104();
            //IssueMultipleSets();
            //IssueJoinLogic();

            //Console.WriteLine("Press any key to terminate");
            //onsole.ReadKey();
            LoadDictionaries();
            InitializeDiscordRPC();
            //DataLoader.model.GenerateGearStats();
        }


        int counter = 0;
        int prevTime = 0;

        private bool showedNullError = false;

        //
        public void Timer_Tick(object? obj, EventArgs e)
        {
            HideMonsterInfoWhenNotInQuest();
            HidePlayerInfoWhenNotInQuest();

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

            UpdateDiscordRPC();

            // Debug.WriteLine("Monster1 SEL=" + DataLoader.model.RoadSelectedMonster() + " ID=" + DataLoader.model.LargeMonster1ID() + " HP=" + DataLoader.model.Monster1HPInt() + "/" + DataLoader.model.SavedMonster1MaxHP);
            //Debug.WriteLine("Monster2 SEL=" + DataLoader.model.RoadSelectedMonster() + " ID=" + DataLoader.model.LargeMonster2ID() + " HP=" + DataLoader.model.Monster2HPInt() + "/" + DataLoader.model.SavedMonster2MaxHP);
            if (isInLauncher() == "NULL" && !showedNullError)
            {
                showedNullError = true;
                //System.Windows.MessageBox.Show("Incorrect values detected, please restart overlay when fully loading into Mezeporta.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        int curNum = 0;
        int prevNum = 0;
        bool isFirstAttack = false;
        public bool IsDragConfigure { get; set; } = false;

        #endregion

        #region damage

        /// <summary>
        /// Creates the damage number.
        /// </summary>
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
                    curNum = 1000 + curNum; //TODO
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

        /// <summary>
        /// Shows multicolor damage numbers?
        /// </summary>
        /// <returns></returns>
        public bool ShowDamageNumbersMulticolor()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableDamageNumbersMulticolor == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Creates the damage number label.
        /// </summary>
        /// <param name="damage">The damage.</param>
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

        /// <summary>
        /// Removes the damage number label.
        /// </summary>
        /// <param name="tb">The tb.</param>
        private void RemoveDamageNumberLabel(Label tb)
        {
            DispatcherTimer timer = new();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += (o, e) => { DamageNumbers.Children.Remove(tb); };
            timer.Start();
        }

        #endregion

        #region UI

        //does this sometimes bug?
        //the UI flashes at least once when loading into quest        
        /// <summary>
        /// Hides the monster information when not in quest.
        /// </summary>
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
            //DL.m.?.Visibility = v && s.?.IsChecked
            //DataLoader.model.ShowMonsterInfos = v && s.MonsterStatusInfoShown;
            DataLoader.model.ShowMonsterAtkMult = v && s.MonsterAtkMultShown;
            DataLoader.model.ShowMonsterDefrate = v && s.MonsterDefrateShown;
            DataLoader.model.ShowMonsterSize = v && s.MonsterSizeShown;
            DataLoader.model.ShowMonsterPoison = v && s.MonsterPoisonShown;
            DataLoader.model.ShowMonsterSleep = v && s.MonsterSleepShown;
            DataLoader.model.ShowMonsterPara = v && s.MonsterParaShown;
            DataLoader.model.ShowMonsterBlast = v && s.MonsterBlastShown;
            DataLoader.model.ShowMonsterStun = v && s.MonsterStunShown;

            DataLoader.model.ShowMonsterHPBars = v && s.HealthBarsShown;
            DataLoader.model.ShowMonster1HPBar = v && s.Monster1HealthBarShown;
            DataLoader.model.ShowMonster2HPBar = v && s.Monster2HealthBarShown;
            DataLoader.model.ShowMonster3HPBar = v && s.Monster3HealthBarShown;
            DataLoader.model.ShowMonster4HPBar = v && s.Monster4HealthBarShown;

            DataLoader.model.ShowMonsterPartHP = v && s.PartThresholdShown;
            DataLoader.model.ShowMonster1Icon = v && s.Monster1IconShown;
        }

        /// <summary>
        /// Hides the player information when not in quest.
        /// </summary>
        private void HidePlayerInfoWhenNotInQuest()
        {
            int time = DataLoader.model.TimeInt();
            if (prevTime == time)
                counter++;
            else
                counter = 0;
            prevTime = time;
            Settings s = (Settings)Application.Current.FindResource("Settings");
            //what is the counter for?
            bool v = s.AlwaysShowPlayerInfo || DataLoader.model.Configuring || counter < 60;
            //DL.m.?.Visibility = v && s.?.IsChecked
            DataLoader.model.ShowTimerInfo = v && s.TimerInfoShown;
            DataLoader.model.ShowHitCountInfo = v && s.HitCountShown;
            DataLoader.model.ShowPlayerAtkInfo = v && s.PlayerAtkShown;
            DataLoader.model.ShowSharpness = v && s.EnableSharpness;

            DataLoader.model.ShowMap = v && s.EnableMap;
        }

        #endregion

        #region get info

        /// <summary>
        /// Gets the name of the area.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetAreaName(int id)
        {
            if (id == 0)
                return "Loading...";
            Dictionary.MapAreaList.MapAreaID.TryGetValue(id, out string? areaname);
            Dictionary.MapAreaList.MapAreaID.TryGetValue(DataLoader.model.RavienteAreaID(), out string? raviareaname);

            switch (DataLoader.model.getRaviName())
            {
                default://or None
                    return areaname + "";
                case "Raviente":
                case "Violent Raviente":
                    return raviareaname + "";
                case "Berserk Raviente Practice":
                case "Berserk Raviente":
                case "Extreme Raviente":
                    if (DataLoader.model.QuestID() != 55796 || DataLoader.model.QuestID() != 55807 || DataLoader.model.QuestID() != 54751 || DataLoader.model.QuestID() != 54761 || DataLoader.model.QuestID() != 55596 || DataLoader.model.QuestID() != 55607)
                        return areaname + "";
                    else
                        return raviareaname + "";
            }
        }

        /// <summary>
        /// Gets the caravan score.
        /// </summary>
        /// <returns></returns>
        public string GetCaravanScore()
        {
            if (DataLoader.model.ShowCaravanScore())
                return string.Format("Caravan Score: {0} | ", DataLoader.model.CaravanScore());
            else
                return "";
        }

        /// <summary>
        /// Gets the caravan skills.
        /// </summary>
        /// <returns></returns>
        public string GetCaravanSkills()
        {
            int id1 = DataLoader.model.CaravanSkill1();
            int id2 = DataLoader.model.CaravanSkill2();
            int id3 = DataLoader.model.CaravanSkill3();

            Dictionary.CaravanSkillList.CaravanSkillID.TryGetValue(id1, out string? caravanSkillName1);
            Dictionary.CaravanSkillList.CaravanSkillID.TryGetValue(id2, out string? caravanSkillName2);
            Dictionary.CaravanSkillList.CaravanSkillID.TryGetValue(id3, out string? caravanSkillName3);

            if (caravanSkillName1 == "" || caravanSkillName1 == "None")
                return "None";
            else if (caravanSkillName2 == "" || caravanSkillName2 == "None")
                return caravanSkillName1 + "";
            else if (caravanSkillName3 == "" || caravanSkillName3 == "None")
                return caravanSkillName1 + ", " + caravanSkillName2;
            else
                return caravanSkillName1 + ", " + caravanSkillName2 + ", " + caravanSkillName3;
        }

        /// <summary>
        /// Gets the diva skill name from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetDivaSkillNameFromID(int id)
        {
            Dictionary.DivaSkillList.DivaSkillID.TryGetValue(id, out string? divaskillaname);
            return divaskillaname + "";
        }

        /// <summary>
        /// Gets the discord timer mode.
        /// </summary>
        /// <returns></returns>
        public string GetDiscordTimerMode()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.DiscordTimerMode == "Time Left")
                return "Time Left";
            else if (s.DiscordTimerMode == "Time Elapsed")
                return "Time Elapsed";
            else return "Time Left";
        }

        private bool inQuest = false;

        /// <summary>
        /// Gets the weapon style from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetWeaponStyleFromID(int id)
        {
            return id switch
            {
                0 => "Earth",
                1 => "Heaven",
                2 => "Storm",
                3 => "Extreme",
                _ => "None",
            };
        }

        /// <summary>
        /// Gets the armor skill.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string getArmorSkill(int id)
        {
            Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(id, out string? skillname);
            if (skillname == "")
                return "None";
            else
                return skillname + "";
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetItemName(int id)
        {
            string itemValue1;
            bool isItemExists1 = Dictionary.Items.ItemIDs.TryGetValue(id, out itemValue1);  //returns true
            //Console.WriteLine(itemValue1); //Print "First"
            //Dictionary.Items.ItemIDs.TryGetValue(1, out itemname);
            return itemValue1 + "";
        }

        /// <summary>
        /// Gets the name of the quest.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetQuestNameFromID(int id)
        {
            if (!(ShowDiscordQuestNames)) return "";
            string QuestValue1;
            bool isQuestExists1 = Dictionary.Quests.QuestIDs.TryGetValue(id, out QuestValue1);  //returns true
            //Console.WriteLine(itemValue1); //Print "First"
            //Dictionary.Items.ItemIDs.TryGetValue(1, out itemname);
            return QuestValue1 + "";
        }

        /// <summary>
        /// Gets the weapon name from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string getWeaponNameFromID(int id)
        {
            Dictionary.WeaponList.WeaponID.TryGetValue(id, out string? weaponname);
            return weaponname + "";
        }

        /// <summary>
        /// Gets the color of the armor.
        /// </summary>
        /// <returns></returns>
        public string getArmorColor()
        {
            Dictionary.ArmorColorList.ArmorColorID.TryGetValue(DataLoader.model.ArmorColor(), out string? colorname);
            return colorname + "";
        }

        /// <summary>
        /// Gets the weapon icon from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string getWeaponIconFromID(int id)
        {
            string weaponName = getWeaponNameFromID(id);
            string colorName = getArmorColor();

            string weaponIconName = "";

            switch (weaponName)
            {
                case "Sword and Shield":
                    weaponIconName += "sns";
                    break;
                case "Dual Swords":
                    weaponIconName += "ds";
                    break;
                case "Great Sword":
                    weaponIconName += "gs";
                    break;
                case "Long Sword":
                    weaponIconName += "ls";
                    break;
                case "Hammer":
                    weaponIconName += "hammer";
                    break;
                case "Hunting Horn":
                    weaponIconName += "hh";
                    break;
                case "Lance":
                    weaponIconName += "lance";
                    break;
                case "Gunlance":
                    weaponIconName += "gl";
                    break;
                case "Switch Axe F":
                    weaponIconName += "saf";
                    break;
                case "Tonfa":
                    weaponIconName += "tonfa";
                    break;
                case "Magnet Spike":
                    weaponIconName += "ms";
                    break;
                case "Light Bowgun":
                    weaponIconName += "lbg";
                    break;
                case "Heavy Bowgun":
                    weaponIconName += "hbg";
                    break;
                case "Bow":
                    weaponIconName += "bow";
                    break;
                default:
                    weaponIconName = "https://i.imgur.com/9OkLYAz.png";//transcend
                    break;
            }

            if (weaponIconName != "https://i.imgur.com/9OkLYAz.png" && !(colorName.Contains("White")))
                weaponIconName += "_";

            if (colorName.Contains("Green"))
            {
                weaponIconName += "green";
            }
            else if (colorName.Contains("Red"))
            {
                weaponIconName += "red";
            }
            //else if (colorName.Contains("White"))
            //{
            //    weaponIconName += weaponName;
            //}
            else if (colorName.Contains("Pink"))
            {
                weaponIconName += "pink";
            }
            else if (colorName.Contains("Blue"))
            {
                weaponIconName += "blue";
            }
            else if (colorName.Contains("Navy"))
            {
                weaponIconName += "navy";
            }
            else if (colorName.Contains("Cyan"))
            {
                weaponIconName += "cyan";
            }
            else if (colorName.Contains("Purple"))
            {
                weaponIconName += "purple";
            }
            else if (colorName.Contains("Orange"))
            {
                weaponIconName += "orange";
            }
            else if (colorName.Contains("Yellow"))
            {
                weaponIconName += "yellow";
            }
            else if (colorName.Contains("Grey"))
            {
                weaponIconName += "grey";
            }
            else if (colorName.Contains("Rainbow"))
            {
                weaponIconName += "rainbow";
            }

            return weaponIconName switch
            {
                "sns" => "https://i.imgur.com/hVCDfnA.png",
                "sns_green" => "https://i.imgur.com/U61zPJa.png",
                "sns_red" => "https://i.imgur.com/ZGCd1lH.png",
                "sns_pink" => "https://i.imgur.com/O4qyBfI.png",
                "sns_blue" => "https://i.imgur.com/dQvflcw.png",
                "sns_navy" => "https://i.imgur.com/vdeSnZh.png",
                "sns_cyan" => "https://i.imgur.com/37gfP8P.png",
                "sns_purple" => "https://i.imgur.com/x7dFt4G.png",
                "sns_orange" => "https://i.imgur.com/bz22IiF.png",
                "sns_yellow" => "https://i.imgur.com/wKItSbP.png",
                "sns_grey" => "https://i.imgur.com/U25Xxfj.png",
                "sns_rainbow" => "https://i.imgur.com/3a6OI1V.gif",
                "ds" => "https://i.imgur.com/JIFNgz9.png",
                "ds_green" => "https://i.imgur.com/MEWrHcC.png",
                "ds_red" => "https://i.imgur.com/dzIoOF2.png",
                "ds_pink" => "https://i.imgur.com/OfUVJy6.png",
                "ds_blue" => "https://i.imgur.com/fUvIuCl.png",
                "ds_navy" => "https://i.imgur.com/oPz7WAA.png",
                "ds_cyan" => "https://i.imgur.com/Lkf6v4A.png",
                "ds_purple" => "https://i.imgur.com/b5Ly09E.png",
                "ds_orange" => "https://i.imgur.com/LdHWvui.png",
                "ds_yellow" => "https://i.imgur.com/2A8UXdT.png",
                "ds_grey" => "https://i.imgur.com/snw6dPs.png",
                "ds_rainbow" => "https://i.imgur.com/eWTRTJl.gif",
                "gs" => "https://i.imgur.com/vLxcWM8.png",
                "gs_green" => "https://i.imgur.com/9puI44e.png",
                "gs_red" => "https://i.imgur.com/Xhs5yJj.png",
                "gs_pink" => "https://i.imgur.com/DXI9FHs.png",
                "gs_blue" => "https://i.imgur.com/GxWofdH.png",
                "gs_navy" => "https://i.imgur.com/ZM1Isqt.png",
                "gs_cyan" => "https://i.imgur.com/tO2TrkB.png",
                "gs_purple" => "https://i.imgur.com/ijgJ69Y.png",
                "gs_orange" => "https://i.imgur.com/CWHEhAi.png",
                "gs_yellow" => "https://i.imgur.com/ZpGOD3z.png",
                "gs_grey" => "https://i.imgur.com/82HhSFD.png",
                "gs_rainbow" => "https://i.imgur.com/WnRuqll.gif",
                "ls" => "https://i.imgur.com/qdA0x3k.png",
                "ls_green" => "https://i.imgur.com/9LvQVQ7.png",
                "ls_red" => "https://i.imgur.com/oc0ExLi.png",
                "ls_pink" => "https://i.imgur.com/jjBGbGu.png",
                "ls_blue" => "https://i.imgur.com/AZ606vb.png",
                "ls_navy" => "https://i.imgur.com/M6mmOpO.png",
                "ls_cyan" => "https://i.imgur.com/qHFbpoJ.png",
                "ls_purple" => "https://i.imgur.com/ICgTu6S.png",
                "ls_orange" => "https://i.imgur.com/XPYDego.png",
                "ls_yellow" => "https://i.imgur.com/H4vJFd1.png",
                "ls_grey" => "https://i.imgur.com/1v7T5Hm.png",
                "ls_rainbow" => "https://i.imgur.com/BUYVOih.gif",
                "hammer" => "https://i.imgur.com/hnY1HC0.png",
                "hammer_green" => "https://i.imgur.com/iOGBcmQ.png",
                "hammer_red" => "https://i.imgur.com/Z5QGsTO.png",
                "hammer_pink" => "https://i.imgur.com/WHkXOoC.png",
                "hammer_blue" => "https://i.imgur.com/fb7bxlw.png",
                "hammer_navy" => "https://i.imgur.com/oaLfSIP.png",
                "hammer_cyan" => "https://i.imgur.com/N2N0Uib.png",
                "hammer_purple" => "https://i.imgur.com/CqNUgtg.png",
                "hammer_orange" => "https://i.imgur.com/PzYNYZh.png",
                "hammer_yellow" => "https://i.imgur.com/Ujpj7WL.png",
                "hammer_grey" => "https://i.imgur.com/R0xCYk5.png",
                "hammer_rainbow" => "https://i.imgur.com/GIAbKkO.gif",
                "hh" => "https://i.imgur.com/EmjAq37.png",
                "hh_green" => "https://i.imgur.com/LWCOXI4.png",
                "hh_red" => "https://i.imgur.com/lwtBV09.png",
                "hh_pink" => "https://i.imgur.com/tZBuDi2.png",
                "hh_blue" => "https://i.imgur.com/7qncIzQ.png",
                "hh_navy" => "https://i.imgur.com/yaFS4N0.png",
                "hh_cyan" => "https://i.imgur.com/GvHKg1u.png",
                "hh_purple" => "https://i.imgur.com/33FpZMA.png",
                "hh_orange" => "https://i.imgur.com/5ZHbR8K.png",
                "hh_yellow" => "https://i.imgur.com/2YdtoVI.png",
                "hh_grey" => "https://i.imgur.com/pyPzmJI.png",
                "hh_rainbow" => "https://i.imgur.com/VuRLWWG.gif",
                "lance" => "https://i.imgur.com/M8fmT4f.png",
                "lance_green" => "https://i.imgur.com/zSyyIZY.png",
                "lance_red" => "https://i.imgur.com/ZFeN3aA.png",
                "lance_pink" => "https://i.imgur.com/X1EncHA.png",
                "lance_blue" => "https://i.imgur.com/qMM2gqG.png",
                "lance_navy" => "https://i.imgur.com/F7vp82x.png",
                "lance_cyan" => "https://i.imgur.com/9q1rqfF.png",
                "lance_purple" => "https://i.imgur.com/qF9JMEE.png",
                "lance_orange" => "https://i.imgur.com/s1Agqri.png",
                "lance_yellow" => "https://i.imgur.com/EcOCe50.png",
                "lance_grey" => "https://i.imgur.com/jKPcLtN.png",
                "lance_rainbow" => "https://i.imgur.com/BXgEuDy.gif",
                "gl" => "https://i.imgur.com/9wq3LQe.png",
                "gl_green" => "https://i.imgur.com/bQdGiiB.png",
                "gl_red" => "https://i.imgur.com/QorzUm5.png",
                "gl_pink" => "https://i.imgur.com/OqkXeZy.png",
                "gl_blue" => "https://i.imgur.com/lsFZSnT.png",
                "gl_navy" => "https://i.imgur.com/2fkkHVd.png",
                "gl_cyan" => "https://i.imgur.com/MzqTO9c.png",
                "gl_purple" => "https://i.imgur.com/QqDN0jm.png",
                "gl_orange" => "https://i.imgur.com/GowSPSA.png",
                "gl_yellow" => "https://i.imgur.com/az9QfWH.png",
                "gl_grey" => "https://i.imgur.com/Q5lK9Nw.png",
                "gl_rainbow" => "https://i.imgur.com/47VsWHj.gif",
                "saf" => "https://i.imgur.com/fVbaN34.png",
                "saf_green" => "https://i.imgur.com/V3x8aaf.png",
                "saf_red" => "https://i.imgur.com/3l8TO9T.png",
                "saf_pink" => "https://i.imgur.com/DTXXEb9.png",
                "saf_blue" => "https://i.imgur.com/Dgr9oQg.png",
                "saf_navy" => "https://i.imgur.com/Tv40lQg.png",
                "saf_cyan" => "https://i.imgur.com/uKxiYhr.png",
                "saf_purple" => "https://i.imgur.com/x3RC716.png",
                "saf_orange" => "https://i.imgur.com/GU2eOdb.png",
                "saf_yellow" => "https://i.imgur.com/f0jrcYq.png",
                "saf_grey" => "https://i.imgur.com/jIRe9fA.png",
                "saf_rainbow" => "https://i.imgur.com/icBF5lS.gif",
                "tonfa" => "https://i.imgur.com/8YpLQ5G.png",
                "tonfa_green" => "https://i.imgur.com/0VflTRd.png",
                "tonfa_red" => "https://i.imgur.com/f5mIJgU.png",
                "tonfa_pink" => "https://i.imgur.com/M6ANARX.png",
                "tonfa_blue" => "https://i.imgur.com/BrCnJbs.png",
                "tonfa_navy" => "https://i.imgur.com/b2lbCN1.png",
                "tonfa_cyan" => "https://i.imgur.com/7bm8xyW.png",
                "tonfa_purple" => "https://i.imgur.com/BOcCFhU.png",
                "tonfa_orange" => "https://i.imgur.com/vi8qGs5.png",
                "tonfa_yellow" => "https://i.imgur.com/qDR1aJZ.png",
                "tonfa_grey" => "https://i.imgur.com/GxFrQm6.png",
                "tonfa_rainbow" => "https://i.imgur.com/2StcKCZ.gif",
                "ms" => "https://i.imgur.com/s3OaNkP.png",
                "ms_green" => "https://i.imgur.com/7c8pPow.png",
                "ms_red" => "https://i.imgur.com/zA4wMON.png",
                "ms_pink" => "https://i.imgur.com/dOc22Dm.png",
                "ms_blue" => "https://i.imgur.com/rz4anE4.png",
                "ms_navy" => "https://i.imgur.com/dvghN1a.png",
                "ms_cyan" => "https://i.imgur.com/gCWBOWm.png",
                "ms_purple" => "https://i.imgur.com/UI3KO1c.png",
                "ms_orange" => "https://i.imgur.com/9Bg0QzE.png",
                "ms_yellow" => "https://i.imgur.com/rAKEtTa.png",
                "ms_grey" => "https://i.imgur.com/dNkcRIR.png",
                "ms_rainbow" => "https://i.imgur.com/TyZFrvK.gif",
                "lbg" => "https://i.imgur.com/txp2GsM.png",
                "lbg_green" => "https://i.imgur.com/CMf9U6x.png",
                "lbg_red" => "https://i.imgur.com/aKLv0na.png",
                "lbg_pink" => "https://i.imgur.com/theTGWy.png",
                "lbg_blue" => "https://i.imgur.com/IgXj7vl.png",
                "lbg_navy" => "https://i.imgur.com/N8vleIL.png",
                "lbg_cyan" => "https://i.imgur.com/4iF0Kex.png",
                "lbg_purple" => "https://i.imgur.com/V36MwUh.png",
                "lbg_orange" => "https://i.imgur.com/FZ3sAAr.png",
                "lbg_yellow" => "https://i.imgur.com/l0Fga4q.png",
                "lbg_grey" => "https://i.imgur.com/WE1oZuG.png",
                "lbg_rainbow" => "https://i.imgur.com/Q0Firpd.gif",
                "hbg" => "https://i.imgur.com/8WD2bI7.png",
                "hbg_green" => "https://i.imgur.com/j6qe1uh.png",
                "hbg_red" => "https://i.imgur.com/hd8cwCa.png",
                "hbg_pink" => "https://i.imgur.com/PDfABOO.png",
                "hbg_blue" => "https://i.imgur.com/qURblCM.png",
                "hbg_navy" => "https://i.imgur.com/FxInecI.png",
                "hbg_cyan" => "https://i.imgur.com/UclnhBS.png",
                "hbg_purple" => "https://i.imgur.com/IHifTBB.png",
                "hbg_orange" => "https://i.imgur.com/7JRHNzp.png",
                "hbg_yellow" => "https://i.imgur.com/rihlgaB.png",
                "hbg_grey" => "https://i.imgur.com/mKpJc0p.png",
                "hbg_rainbow" => "https://i.imgur.com/TgPORx6.gif",
                "bow" => "https://i.imgur.com/haCsXQr.png",
                "bow_green" => "https://i.imgur.com/vykrGg9.png",
                "bow_red" => "https://i.imgur.com/01nEtNy.png",
                "bow_pink" => "https://i.imgur.com/DLIYT8G.png",
                "bow_blue" => "https://i.imgur.com/THX3O3X.png",
                "bow_navy" => "https://i.imgur.com/DGHifcq.png",
                "bow_cyan" => "https://i.imgur.com/sXnzQrG.png",
                "bow_purple" => "https://i.imgur.com/D6NYg8r.png",
                "bow_orange" => "https://i.imgur.com/fy47m6l.png",
                "bow_yellow" => "https://i.imgur.com/ExGTxvl.png",
                "bow_grey" => "https://i.imgur.com/Y5vOofE.png",
                "bow_rainbow" => "https://i.imgur.com/rsEycVk.gif",
                _ => "https://i.imgur.com/9OkLYAz.png",//transcend
            };
        }

        /// <summary>
        /// Gets the real name of the monster.
        /// </summary>
        /// <param name="iconName">Name of the icon.</param>
        /// <returns></returns>
        public string GetRealMonsterName(string iconName, bool isLargeImageText = false)
        {
            if (ShowDiscordQuestNames && !(isLargeImageText)) return "";
            //quest ids:
            //mp road: 23527
            //solo road: 23628
            //1st district dure: 21731
            //2nd district dure: 21746
            //1st district dure sky corridor: 21749
            //2nd district dure sky corridor: 21750
            //arrogant dure repel: 23648
            //arrogant dure slay: 23649
            //urgent tower: 21751
            //4th district dure: 21748
            //3rd district dure: 21747
            //3rd district dure 2: 21734
            //UNUSED sky corridor: 21730
            //sky corridor prologue: 21729
            //string RealMonsterName = iconName.Replace("https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/main/img/monster/", "");
            //RealMonsterName = RealMonsterName.Replace(".gif", "");
            //RealMonsterName = RealMonsterName.Replace(".png", "");
            //RealMonsterName = RealMonsterName.Replace("zenith_", "");
            //RealMonsterName = RealMonsterName.Replace("_", " ");

            ////https://stackoverflow.com/questions/4315564/capitalizing-words-in-a-string-using-c-sharp
            //RealMonsterName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(RealMonsterName);

            int id;

            if (DataLoader.model.roadOverride() == false)
                id = DataLoader.model.RoadSelectedMonster() == 0 ? DataLoader.model.LargeMonster1ID() : DataLoader.model.LargeMonster2ID();
            else if (DataLoader.model.CaravanOverride())
                id = DataLoader.model.CaravanMonster1ID();
            else
                id = DataLoader.model.LargeMonster1ID();

            //dure
            if (DataLoader.model.QuestID() == 21731 || DataLoader.model.QuestID() == 21746 || DataLoader.model.QuestID() == 21749 || DataLoader.model.QuestID() == 21750)
                return "Duremudira";
            else if (DataLoader.model.QuestID() == 23648 || DataLoader.model.QuestID() == 23649)
                return "Arrogant Duremudira";

            //ravi
            if (DataLoader.model.getRaviName() != "None")
            {
                switch (DataLoader.model.getRaviName())
                {
                    case "Raviente":
                        return "Raviente";
                    case "Violent Raviente":
                        return "Violent Raviente";
                    case "Berserk Raviente Practice":
                        return "Berserk Raviente (Practice)";
                    case "Berserk Raviente":
                        return "Berserk Raviente";
                    case "Extreme Raviente":
                        return "Extreme Raviente";
                    default:
                        return "Raviente";
                }
            }


            switch (id)
            {
                case 0: //none
                    return "None";
                case 1:
                    return "Rathian";
                case 2:
                    if (DataLoader.model.RankBand() == 53)
                        return "Fatalis";
                    else
                        return "Fatalis";
                case 3:
                    return "Kelbi";
                case 4:
                    return "Mosswine";
                case 5:
                    return "Bullfango";
                case 6:
                    return "Yian Kut-Ku";
                case 7:
                    return "Lao-Shan Lung";
                case 8:
                    return "Cephadrome";
                case 9:
                    return "Felyne";
                case 10: //veggie elder
                    return "Veggie Elder";
                case 11:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Rathalos";
                    else
                        return "Rathalos";
                case 12:
                    return "Aptonoth";
                case 13:
                    return "Genprey";
                case 14:
                    return "Diablos";
                case 15:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Khezu";
                    else
                        return "Khezu";
                case 16:
                    return "Velociprey";
                case 17:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Gravios";
                    else
                        return "Gravios";
                case 18:
                    return "Felyne";
                case 19:
                    return "Vespoid";
                case 20:
                    return "Gypceros";
                case 21:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Plesioth";
                    else
                        return "Plesioth";
                case 22:
                    return "Basarios";
                case 23:
                    return "Melynx";
                case 24:
                    return "Hornetaur";
                case 25:
                    return "Apceros";
                case 26:
                    return "Monoblos";
                case 27:
                    return "Velocidrome";
                case 28:
                    return "Gendrome";
                case 29://rocks
                    return "Rocks";
                case 30:
                    return "Ioprey";
                case 31:
                    return "Iodrome";
                case 32://pugis
                    return "Poogie";
                case 33:
                    return "Kirin";
                case 34:
                    return "Cephalos";
                case 35:
                    return "Giaprey";
                case 36:
                    if (DataLoader.model.RankBand() == 53)
                        return "Crimson Fatalis";
                    else
                        return "Crimson Fatalis";
                case 37:
                    return "Pink Rathian";
                case 38:
                    return "Blue Yian Kut-Ku";
                case 39:
                    return "Purple Gypceros";
                case 40:
                    return "Yian Garuga";
                case 41:
                    return "Silver Rathalos";
                case 42:
                    return "Gold Rathian";
                case 43:
                    return "Black Diablos";
                case 44:
                    return "White Monoblos";
                case 45:
                    return "Red Khezu";
                case 46:
                    return "Green Plesioth";
                case 47:
                    return "Black Gravios";
                case 48:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Daimyo Hermitaur";
                    else
                        return "Daimyo Hermitaur";
                case 49:
                    return "Azure Rathalos";
                case 50:
                    return "Ashen Lao-Shan Lung";
                case 51:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Blangonga";
                    else
                        return "Blangonga";
                case 52:
                    return "Congalala";
                case 53:
                    if (DataLoader.model.RankBand() == 56 || DataLoader.model.RankBand() == 57)
                        return "Rajang";
                    else
                        return "Rajang";
                case 54:
                    return "Kushala Daora";
                case 55:
                    return "Shen Gaoren";
                case 56:
                    return "Great Thunderbug";
                case 57:
                    return "Shakalaka";
                case 58:
                    return "Yama Tsukami";
                case 59:
                    return "Chameleos";
                case 60:
                    return "Rusted Kushala Daora";
                case 61:
                    return "Blango";
                case 62:
                    return "Conga";
                case 63:
                    return "Remobra";
                case 64:
                    return "Lunastra";
                case 65:
                    if (DataLoader.model.RankBand() == 32)
                        return "Supremacy Teostra";
                    else
                        return "Teostra";
                case 66:
                    return "Hermitaur";
                case 67:
                    return "Shogun Ceanataur";
                case 68:
                    return "Bulldrome";
                case 69:
                    return "Anteka";
                case 70:
                    return "Popo";
                case 71:
                    if (DataLoader.model.RankBand() == 53)
                        return "White Fatalis";
                    else
                        return "White Fatalis";
                case 72:
                    return "Yama Tsukami";
                case 73:
                    return "Ceanataur";
                case 74:
                    return "Hypnoc";
                case 75:
                    return "Lavasioth";
                case 76:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Zenith Tigrex";
                    else
                        return "Tigrex";
                case 77:
                    return "Akantor";
                case 78:
                    return "Bright Hypnoc";
                case 79:
                    return "Red Lavasioth";
                case 80:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Espinas";
                    else
                        return "Espinas";
                case 81:
                    return "Orange Espinas";
                case 82:
                    return "Silver Hypnoc";
                case 83:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Akura Vashimu";
                    else
                        return "Akura Vashimu";
                case 84:
                    return "Akura Jebia";
                case 85:
                    return "Berukyurosu";
                case 86://cactus
                    return "Cactus";
                case 87://gorge objects
                    return "Gorge Object";
                case 88://gorge rocks
                    return "Gorge Rock";
                case 89:
                    if (DataLoader.model.RankBand() == 32 || DataLoader.model.RankBand() == 54)
                        return "Thirsty Pariapuria";
                    else
                        return "Pariapuria";
                case 90:
                    return "White Espinas";
                case 91:
                    return "Kamu Orugaron";
                case 92:
                    return "Nono Orugaron";
                case 93:
                    return "Raviente";
                case 94:
                    return "Dyuragaua";
                case 95:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Doragyurosu";
                    else if (DataLoader.model.RankBand() == 32)
                        return "Supremacy Doragyurosu";
                    else
                        return "Doragyurosu";
                case 96:
                    return "Gurenzeburu";
                case 97:
                    return "Burukku";
                case 98:
                    return "Erupe";
                case 99:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Rukodiora";
                    else
                        return "Rukodiora";
                case 100:
                    if (DataLoader.model.RankBand() == 70 || DataLoader.model.RankBand() == 54)
                        return "Unknown";
                    else
                        return "Unknown";
                case 101:
                    return "Gogomoa";
                case 102://kokomoa
                    return "Kokomoa";
                case 103:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Taikun Zamuza";
                    else
                        return "Taikun Zamuza";
                case 104:
                    return "Abiorugu";
                case 105:
                    return "Kuarusepusu";
                case 106:
                    if (DataLoader.model.RankBand() == 32)
                        return "Supremacy Odibatorasu";
                    else
                        return "Odibatorasu";
                case 107:
                    if (DataLoader.model.RankBand() == 54 || DataLoader.model.RankBand() == 55)
                        return "Disufiroa";
                    else
                        return "Disufiroa";
                case 108:
                    return "Rebidiora";
                case 109:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Anorupatisu";
                    else
                        return "Anorupatisu";
                case 110:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Hyujikiki";
                    else
                        return "Hyujikiki";
                case 111:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Midogaron";
                    else
                        return "Midogaron";
                case 112:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Giaorugu";
                    else
                        return "Giaorugu";
                case 113:
                    if (DataLoader.model.RankBand() == 55)
                        return "Shifting Mi Ru";
                    else
                        return "Mi Ru";
                case 114:
                    return "Farunokku";
                case 115:
                    return "Pokaradon";
                case 116:
                    if (DataLoader.model.RankBand() == 53)
                        return "Shantien";
                    else
                        return "Shantien";
                case 117:
                    return "Pokara";
                case 118://dummy
                    return "Dummy";
                case 119:
                    return "Goruganosu";
                case 120:
                    return "Aruganosu";
                case 121:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Baruragaru";
                    else
                        return "Baruragaru";
                case 122:
                    return "Zerureusu";
                case 123:
                    return "Gougarf";
                case 124:
                    return "Uruki";
                case 125:
                    return "Forokururu";
                case 126:
                    return "Meraginasu";
                case 127:
                    return "Diorex";
                case 128:
                    return "Garuba Daora";
                case 129:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Inagami";
                    else
                        return "Inagami";
                case 130:
                    return "Varusaburosu";
                case 131:
                    return "Poborubarumu";
                case 132:
                    return "Duremudira";
                case 133://UNK
                    return "UNK";
                case 134:
                    return "Felyne";
                case 135://blue npc
                    return "Blue NPC";
                case 136://UNK
                    return "UNK";
                case 137://cactus
                    return "Cactus";
                case 138://veggie elders
                    return "Veggie Elder";
                case 139:
                    return "Gureadomosu";
                case 140:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Harudomerugu";
                    else
                        return "Harudomerugu";
                case 141:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Toridcless";
                    else
                        return "Toridcless";
                case 142:
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "Gasurabazura";
                    else
                        return "Gasurabazura";
                case 143:
                    return "Kusubami";
                case 144:
                    return "Yama Kurai";
                case 145://3rd phase duremudira
                    return "Duremudira";
                case 146:
                    if (DataLoader.model.RankBand() >= 54 && DataLoader.model.RankBand() <= 55)
                        return "Howling Zinogre";
                    else
                        return "Zinogre";
                case 147:
                    return "Deviljho";
                case 148:
                    return "Brachydios";
                case 149:
                    return "Berserk Raviente";
                case 150:
                    return "Toa Tesukatora";
                case 151:
                    return "Barioth";
                case 152:
                    return "Uragaan";
                case 153:
                    return "Stygian Zinogre";
                case 154:
                    if (DataLoader.model.RankBand() >= 54 && DataLoader.model.RankBand() <= 55)
                        return "Ruling Guanzorumu";
                    else
                        return "Guanzorumu";
                case 155:
                    if (DataLoader.model.RankBand() == 55)
                        return "Golden Deviljho";
                    else
                        return "Starving Deviljho";
                case 156://UNK
                    return "UNK";
                case 157://egyurasu
                    return "Egyurasu";
                case 158:
                    return "Voljang";
                case 159:
                    return "Nargacuga";
                case 160:
                    return "Keoaruboru";
                case 161:
                    return "Zenaserisu";
                case 162:
                    return "Gore Magala";
                case 163:
                    return "Blinking Nargacuga";
                case 164:
                    return "Shagaru Magala";
                case 165:
                    return "Amatsu";
                case 166:
                    if (DataLoader.model.RankBand() >= 54 && DataLoader.model.RankBand() <= 55)
                        return "Burning Freezing Elzelion";
                    else
                        return "Elzelion";
                case 167:
                    return "Arrogant Duremudira";
                case 168://rocks
                    return "Rock";
                case 169:
                    return "Seregios";
                case 170:
                    return "Bogabadorumu";
                case 171://unknown blue barrel
                    return "Blue Barrel";
                case 172:
                    return "Blitzkrieg Bogabadorumu";
                case 173://costumed uruki
                    return "Uruki";
                case 174:
                    return "Sparkling Zerureusu";
                case 175://PSO2 Rappy
                    return "PSO2 Rappy";
                case 176:
                    return "King Shakalaka";
                default:
                    return "Loading...";
            }
        }

        /// <summary>
        /// Gets the rank name from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetRankNameFromID(int id, bool isLargeImageText = false)
        {
            if (ShowDiscordQuestNames && !(isLargeImageText)) return "";
            switch (id)
            {
                case 0:
                    return "";
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    return "Low Rank ";
                case 11:
                    return "Low/High Rank ";
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    return "High Rank ";
                case 26:
                case 31:
                case 42:
                    return "HR5 ";
                case 32:
                case 46://supremacies
                    //if (GetRealMonsterName(DataLoader.model.CurrentMonster1Icon).Contains("Supremacy"))
                    //{
                        return "";
                    //} else
                    //{
                     //   return "";
                    //}
                case 53://: conquest levels via quest id
                    //shantien
                    //lv1 23585
                    //lv200 23586
                    //lv1000 23587
                    //lv9999 23588
                    //disufiroa
                    //lv1 23589
                    //lv200 23590
                    //lv1000 23591
                    //lv9999 23592
                    //fatalis
                    //lv1 23593
                    //lv200 23594
                    //lv1000 23595
                    //lv9999 23596
                    //crimson fatalis
                    //lv1 23597
                    //lv200 23598
                    //lv1000 23599
                    //lv9999 23601
                    //upper shiten unknown 23605
                    //lower shiten unknown 23604
                    //upper shiten disufiroa 23603
                    //lower shiten disu 23602
                    //thirsty 55532
                    //shifting 55920
                    //starving 55916
                    //golden 55917
                    switch (DataLoader.model.QuestID())
                    {
                        default:
                            return "G Rank ";
                        case 23585:
                        case 23589:
                        case 23593:
                        case 23597:
                            return "Lv1 ";
                        case 23586:
                        case 23590:
                        case 23594:
                        case 23598:
                            return "Lv200 ";
                        case 23587:
                        case 23591:
                        case 23595:
                        case 23599:
                            return "Lv1000 ";
                        case 23588:
                        case 23592:
                        case 23596:
                        case 23601:
                            return "Lv9999 ";
                    }

                case 54:
                    switch (DataLoader.model.QuestID())
                    {
                        default:
                            return "";
                        case 23604:
                        case 23602:
                            return "Lower Shiten ";
                    }
                //return ""; //20m lower shiten/musou repel/musou lesser slay
                case 55:
                    switch (DataLoader.model.QuestID())
                    {
                        default:
                            return "";
                        case 23603:
                            return "Upper Shiten ";
                    }
                //10m upper shiten/musou true slay

                
                case 56://twinhead rajang / voljang and rajang
                case 57://twinhead mi ru / white and brown espi / unknown and zeru / rajang and dorag
                    return "Twinhead ";
                case 64:
                    return "Zenith★1 ";
                case 65:
                    return "Zenith★2 ";
                case 66:
                    return "Zenith★3 ";
                case 67:
                    return "Zenith★4 ";
                case 70://unknown
                    return "Upper Shiten ";
                case 71:
                case 72:
                case 73:
                    return "Interception ";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Gets the objective name from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetObjectiveNameFromID(int id, bool isLargeImageText = false)
        {
            if (ShowDiscordQuestNames && !(isLargeImageText)) return "";
            return id switch
            {
                0 => "Nothing ",
                1 => "Hunt ",
                257 => "Capture ",
                513 => "Slay ",
                32772 => "Repel ",
                98308 => "Slay or Repel ",
                262144 => "Slay All ",
                131072 => "Slay Total ",
                2 => "Deliver ",
                16388 => "Break Part ",
                4098 => "Deliver Flag ",
                16 => "Esoteric Action ",
                _ => "Nothing ",
            };
        }

        /// <summary>
        /// Gets the area icon from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetAreaIconFromID(int id) //TODO: are highlands, tidal island or painted falls icons correct?
        {
            switch (id)
            {
                case 0://Loading
                    return "https://i.imgur.com/Dwdfpen.png";
                case 1://Jungle areas
                case 2:
                case 3:
                case 4:
                case 5:
                case 18:
                case 19:
                case 22:
                case 23:
                case 26:
                case 110:
                case 111:
                case 112:
                case 113:
                case 114:
                case 115:
                case 116:
                case 117:
                case 118:
                case 119:
                case 120:
                case 212:
                case 213:
                    return "https://i.imgur.com/IZ8Zrv4.png";
                case 6: //Snowy mountain areas
                case 15:
                case 92:
                case 93:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                case 218:
                case 219:
                    return "https://i.imgur.com/LLr0RoN.png";
                case 7: //Desert areas
                case 24:
                case 45:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 140:
                case 141:
                case 142:
                case 143:
                case 144:
                case 145:
                case 146:
                case 147:
                case 148:
                case 149:
                case 150:
                case 214:
                case 215:
                    return "https://i.imgur.com/qmLxGj3.png";
                case 8://Volcano areas
                case 27:
                case 58:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 74:
                case 161:
                case 162:
                case 163:
                case 164:
                case 165:
                case 166:
                case 167:
                case 168:
                case 169:
                case 216:
                case 217:
                case 220:
                case 221:
                case 222:
                case 223:
                    return "https://i.imgur.com/m18pzvD.png";
                case 9://Swamp areas
                case 16:
                case 29:
                case 44:
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                case 73:
                case 75:
                case 151:
                case 152:
                case 153:
                case 154:
                case 155:
                case 156:
                case 157:
                case 158:
                case 159:
                case 160:
                    return "https://i.imgur.com/i9oRjSE.png";
                case 21://Forest and Hills areas
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 184:
                case 185:
                case 186:
                case 187:
                case 188:
                case 189:
                case 190:
                case 191:
                case 192:
                case 193:
                case 194:
                case 195:
                case 196:
                    return "https://i.imgur.com/mSCOW0P.png";
                case 224://Great Forest areas
                case 225:
                case 226:
                case 227:
                case 228:
                case 229:
                case 230:
                case 231:
                case 232:
                case 233:
                case 234:
                case 235:
                case 236:
                case 237:
                case 238:
                case 239:
                case 240:
                case 241:
                    return "https://i.imgur.com/prSGAFU.png";
                case 247://Highlands areas
                case 248:
                case 249:
                case 250:
                case 251:
                case 252:
                case 253:
                case 254:
                case 255:
                case 302:
                case 303:
                case 304:
                case 305:
                case 306:
                case 307:
                case 308:
                    return "https://i.imgur.com/2HkWBN6.png";
                case 322://Tidal Island areas
                case 323:
                case 324:
                case 325:
                case 326:
                case 327:
                case 328:
                case 329:
                case 330:
                case 331:
                case 332:
                case 333:
                case 334:
                case 335:
                case 336:
                case 337:
                case 338:
                case 339:
                    return "https://i.imgur.com/31ounTS.png";
                case 345://Polar Sea areas
                case 346:
                case 347:
                case 348:
                case 349:
                case 350:
                case 351:
                case 352:
                case 353:
                case 354:
                case 355:
                case 356:
                case 357:
                case 358:
                    return "https://i.imgur.com/XpIow4u.png";
                case 361://Flower Field areas
                case 362:
                case 363:
                case 364:
                case 365:
                case 366:
                case 367:
                case 368:
                case 369:
                case 370:
                case 371:
                case 372:
                    return "https://i.imgur.com/ybX6w9G.png";
                case 390://TODO test
                case 391://Tower / Tenrou (Sky Corridor) areas
                case 392:
                case 393:
                case 394:
                case 415:
                case 416:
                    return "https://i.imgur.com/Kq8qx0P.png";
                case 399://dure doorway
                case 414://dure door
                    return "https://i.imgur.com/ylzLXo8.gif";
                case 400://White Lake areas
                case 401:
                case 402:
                case 403:
                case 404:
                case 405:
                case 406:
                case 407:
                case 408:
                case 409:
                case 410:
                case 411:
                case 412:
                case 413:
                    return "https://i.imgur.com/qz6XfEq.png";
                case 423://Painted Falls areas
                case 424:
                case 425:
                case 426:
                case 427:
                case 428:
                case 429:
                case 430:
                case 431:
                case 432:
                case 433:
                case 434:
                case 435:
                case 436:
                    return "https://i.imgur.com/36TTe1a.png";
                case 459://Hunter's Road Base Camp
                    return "https://i.imgur.com/MEPAa5x.png";//TODO test
                case 288://Gorge areas
                case 289:
                case 290:
                case 291:
                case 292:
                case 293:
                case 294:
                case 295:
                case 296:
                case 297:
                case 298:
                case 299:
                case 300:
                case 301:
                    return "https://i.imgur.com/EKfebRg.png";




                case 200://Mezeporta
                case 397://Mezeporta dupe non-HD
                    return "https://i.imgur.com/c4iPpu8.png"; //cattleya
                case 173://My Houses
                case 175:
                    return "https://i.imgur.com/MwGHMuD.png";
                case 201://Hairdresser
                    return "https://i.imgur.com/ADsy9ml.png";
                case 202: //Guild Halls
                case 203:
                case 204:
                    return "https://i.imgur.com/3wSfRmu.png'";
                case 205://Pugi Farm
                    return "https://i.imgur.com/Bi5xwQo.png";
                case 210://Private Bar
                case 211://Rasta Bar
                    return "https://i.imgur.com/c08AXk3.png";
                case 256://Caravan Areas
                case 260:
                case 261:
                case 262:
                case 263:
                    return "https://i.imgur.com/YgPzhKw.png";
                case 257://Blacksmith
                    return "https://i.imgur.com/nZ1E0KR.png";
                case 264://Gallery
                    return "https://i.imgur.com/7f4bwjx.png";
                case 265://Guuku Farm
                    return "https://i.imgur.com/4930Pa9.png";
                case 283://Halk Area
                    return "https://i.imgur.com/N0NyS0U.png";
                case 286://PvP Room
                    return "https://i.imgur.com/QxkNRac.png";
                case 340: //SR Rooms
                case 341:
                    return "https://i.imgur.com/C7te61a.png";
                case 379://Diva Halls
                case 445:
                    return "https://i.imgur.com/76eoGbY.png";
                //case road?
                case 462: //MezFes areas
                case 463:
                case 464:
                case 465:
                case 466:
                case 467:
                case 468:
                case 469:
                    return "https://i.imgur.com/EcQLTmm.png";
                default:
                    return "https://i.imgur.com/c4iPpu8.png";//cattleya
            }
        }

        /// <summary>
        /// Gets the game mode.
        /// </summary>
        /// <param name="isHighGradeEdition">if set to <c>true</c> [is high grade edition].</param>
        /// <returns></returns>
        public string GetGameMode(bool isHighGradeEdition)
        {
            if (isHighGradeEdition)
                return " [High-Grade Edition]";
            else
                return "";
        }

        readonly Mem m = new();

        public string isInLauncher()
        {
            int pidToSearch = m.GetProcIdFromName("mhf");
            //Init a condition indicating that you want to search by process id.
            var condition = new PropertyCondition(AutomationElementIdentifiers.ProcessIdProperty,
                pidToSearch);
            //Find the automation element matching the criteria
            AutomationElement element = AutomationElement.RootElement.FindFirst(
                TreeScope.Children, condition);

            if (element == null || pidToSearch == 0)
                return "NULL";

            //get the classname
            var className = element.Current.ClassName;

            if (className == "MHFLAUNCH")
                return "Yes";
            else
                return "No";
        }

        /// <summary>
        /// Gets the overlay mode.
        /// </summary>
        /// <returns></returns>
        public string GetOverlayMode()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            if (DataLoader.model.Configuring)
                return "(Configuring) ";
            else if (DataLoader.closedGame)
                return "(Closed Game) ";
            else if (DataLoader.isInLauncher || isInLauncher() == "Yes") //works?
                return "(Launcher) ";
            else if (isInLauncher() == "NULL")
                return "(No game detected) ";
            else if (DataLoader.model.QuestID() == 0 && DataLoader.model.AreaID() == 0 && DataLoader.model.MeleeWeaponID() == 0 && DataLoader.model.RangedWeaponID() == 0)
                return "(Main Menu) ";
            else if (DataLoader.model.QuestID() == 0 && DataLoader.model.AreaID() == 200 && DataLoader.model.MeleeWeaponID() == 0 && DataLoader.model.RangedWeaponID() == 0)
                return "(World Select) ";
            else if (!(inQuest) || s.EnableDamageNumbers || s.HealthBarsShown || s.EnableSharpness || s.PartThresholdShown || s.HitCountShown || s.PlayerAtkShown || s.MonsterAtkMultShown || s.MonsterDefrateShown || s.MonsterSizeShown || s.MonsterPoisonShown || s.MonsterParaShown || s.MonsterSleepShown || s.MonsterBlastShown || s.MonsterStunShown)
                return "";
            else if (s.TimerInfoShown)
                return "(Speedrun) ";
            else
                return "(Zen) ";
        }

        /// <summary>
        /// Gets the monster icon.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string getMonsterIcon(int id)
        {
            //quest ids:
            //mp road: 23527
            //solo road: 23628
            //1st district dure: 21731
            //2nd district dure: 21746
            //1st district dure sky corridor: 21749
            //2nd district dure sky corridor: 21750
            //arrogant dure repel: 23648
            //arrogant dure slay: 23649
            //urgent tower: 21751
            //4th district dure: 21748
            //3rd district dure: 21747
            //3rd district dure 2: 21734
            //UNUSED sky corridor: 21730
            //sky corridor prologue: 21729
            if (DataLoader.model.roadOverride() == false)
                id = DataLoader.model.RoadSelectedMonster() == 0 ? DataLoader.model.LargeMonster1ID() : DataLoader.model.LargeMonster2ID();
            else if (DataLoader.model.CaravanOverride())
                id = DataLoader.model.CaravanMonster1ID();
            //Duremudira Arena
            if (DataLoader.model.AreaID() == 398 && (DataLoader.model.QuestID() == 21731 || DataLoader.model.QuestID() == 21746 || DataLoader.model.QuestID() == 21749 || DataLoader.model.QuestID() == 21750)) 
                id = 132;//duremudira
            else if (DataLoader.model.AreaID() == 398 && (DataLoader.model.QuestID() == 23648 || DataLoader.model.QuestID() == 23649))
                id = 167;//arrogant duremudira

            switch (id)
            {
                case 0: //none (fatalis/random)
                    return "https://i.imgur.com/3pQEtzw.png";
                case 1://rathian
                    return "https://i.imgur.com/uW1PHeW.png";
                case 2://fatalis
                    if (DataLoader.model.RankBand() == 53)
                        return "https://i.imgur.com/VJNLFWf.png";
                    else
                        return "https://i.imgur.com/Fht5iyz.png";
                case 3://kelbi
                    return "https://i.imgur.com/Ad5xCF6.png";
                case 4://mosswine
                    return "https://i.imgur.com/qWSVddC.png";
                case 5://bullfango
                    return "https://i.imgur.com/qz7CynW.png";
                case 6://kutku
                    return "https://i.imgur.com/TtJ7KPw.png";
                case 7://lao
                    return "https://i.imgur.com/ZW43PS5.png";
                case 8://cephadrome
                    return "https://i.imgur.com/RwkhTLJ.png";
                case 9://felyne
                    return "https://i.imgur.com/Ry2zu5r.png";
                case 10: //veggie elder
                    return "https://i.imgur.com/3pQEtzw.png";
                case 11://rathalos
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/o8bJZUN.gif";
                    else
                        return "https://i.imgur.com/suPp2tU.png";
                case 12://aptonoth
                    return "https://i.imgur.com/15KEndF.png";
                case 13://genprey
                    return "https://i.imgur.com/ChQomJ4.png";
                case 14://diablos
                    return "https://i.imgur.com/XZaYYFL.png";
                case 15://khezu
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/t58j2Zm.gif";
                    else
                        return "https://i.imgur.com/f8rLuGe.png";
                case 16://velociprey
                    return "https://i.imgur.com/WGrl1DY.png";
                case 17://gravios
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/PGkduy9.gif";
                    else
                        return "https://i.imgur.com/Sj4SsYR.png";
                case 18://felyne
                    return "https://i.imgur.com/Ry2zu5r.png";
                case 19://vespoid
                    return "https://i.imgur.com/dhiIvMc.png";
                case 20://gypceros
                    return "https://i.imgur.com/vovKgVw.png";
                case 21://plesioth
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/bsHESxp.gif";
                    else
                        return "https://i.imgur.com/BQUAjEf.png";
                case 22://basarios
                    return "https://i.imgur.com/y86jUp6.png";
                case 23://melynx
                    return "https://i.imgur.com/LoqmciT.png";
                case 24://hornetaur
                    return "https://i.imgur.com/uXam7c6.png";
                case 25://apceros
                    return "https://i.imgur.com/Y2ovscJ.png";
                case 26://monoblos
                    return "https://i.imgur.com/bJt02Qe.png";
                case 27://velocidrome
                    return "https://i.imgur.com/6HMWaGt.png";
                case 28://gendrome
                    return "https://i.imgur.com/XBWX8Wm.png";
                case 29://rocks
                    return "https://i.imgur.com/3pQEtzw.png";
                case 30://ioprey
                    return "https://i.imgur.com/KDxPzPs.png";
                case 31://iodrome
                    return "https://i.imgur.com/QsyXEmc.png";
                case 32://pugis/poogies
                    return "https://i.imgur.com/3pQEtzw.png";
                case 33://kirin
                    return "https://i.imgur.com/5s4ToRS.png";
                case 34://cephalos
                    return "https://i.imgur.com/3n7NQG9.png";
                case 35://giaprey
                    return "https://i.imgur.com/QPl1AEF.png";
                case 36://crimson fatalis
                    if (DataLoader.model.RankBand() == 53)
                        return "https://i.imgur.com/PRFnN10.png";
                    else
                        return "https://i.imgur.com/T36zMrZ.png";
                case 37://pink rathian
                    return "https://i.imgur.com/yn3uMc2.png";
                case 38://blue kutku
                    return "https://i.imgur.com/NmRxU2H.png";
                case 39://purple gypceros
                    return "https://i.imgur.com/eDiBAxX.png";
                case 40://garuga
                    return "https://i.imgur.com/ApZmoUv.png";
                case 41://silverlos
                    return "https://i.imgur.com/mYY8y19.png";
                case 42://goldian
                    return "https://i.imgur.com/xe8nLNM.png";
                case 43://black diablos
                    return "https://i.imgur.com/IVXcxRD.png";
                case 44://white monoblos
                    return "https://i.imgur.com/BQ9FJBB.png";
                case 45://red khezu
                    return "https://i.imgur.com/Cmb6AYd.png";
                case 46://green plesioth
                    return "https://i.imgur.com/LQnA7d6.png";
                case 47://black gravios
                    return "https://i.imgur.com/Fmw5Etb.png";
                case 48://zenith daimyo
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/AO0Olia.gif";
                    else
                        return "https://i.imgur.com/WDe9OJl.png";
                case 49://azurelos
                    return "https://i.imgur.com/gSCligX.png";
                case 50://ashen lao
                    return "https://i.imgur.com/fHPx16u.png";
                case 51://blangonga
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/YiyBzoU.gif";
                    else
                        return "https://i.imgur.com/Di3LdOq.png";
                case 52://congalala
                    return "https://i.imgur.com/qxaMV1h.png";
                case 53://rajang
                    if (DataLoader.model.RankBand() == 56 || DataLoader.model.RankBand() == 57)
                        return "https://i.imgur.com/HRMmhW1.png";
                    else
                        return "https://i.imgur.com/R1dDHaQ.png";
                case 54://kushala
                    return "https://i.imgur.com/uAyaJ9z.png";
                case 55://shen
                    return "https://i.imgur.com/pgKJuGj.png";
                case 56://great thunderbug
                    return "https://i.imgur.com/u0M5jXA.png";
                case 57://shakalaka
                    return "https://i.imgur.com/UaZkOgY.png";
                case 58://yama tsukami
                    return "https://i.imgur.com/suHOj84.png";
                case 59://chammy
                    return "https://i.imgur.com/TXmNN2X.png";
                case 60://rusted kushala
                    return "https://i.imgur.com/4hNQwSU.png";
                case 61://blango
                    return "https://i.imgur.com/3hBdp8b.png";
                case 62://conga
                    return "https://i.imgur.com/zxQcbQD.png";
                case 63://remobra
                    return "https://i.imgur.com/kewyYlK.png";
                case 64://lunastra
                    return "https://i.imgur.com/8OvYfy6.png";
                case 65://teostra
                    if (DataLoader.model.RankBand() == 32)
                        return "https://i.imgur.com/H3zUhEw.png";
                    else
                        return "https://i.imgur.com/dgq8E90.png";
                case 66://hermitaur
                    return "https://i.imgur.com/l2SOZee.png";
                case 67://shogun
                    return "https://i.imgur.com/lEcEWZ6.png";
                case 68://bulldrome
                    return "https://i.imgur.com/AxBWXBC.png";
                case 69://anteka
                    return "https://i.imgur.com/QxGg3Np.png";
                case 70://popo
                    return "https://i.imgur.com/jTFVi1A.png";
                case 71://white fatalis
                    if (DataLoader.model.RankBand() == 53)
                        return "https://i.imgur.com/OAbx9JC.png";
                    else
                        return "https://i.imgur.com/z2QtMnG.png";
                case 72://yama tsukami
                    return "https://i.imgur.com/suHOj84.png";
                case 73://ceanataur
                    return "https://i.imgur.com/2PbL0oE.png";
                case 74://hypnoc
                    return "https://i.imgur.com/tkYXFBc.png";
                case 75://lavasioth
                    return "https://i.imgur.com/ZSgmzGi.png";
                case 76://tigrex
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/K7qTAoa.gif";
                    else
                        return "https://i.imgur.com/QKw3HSE.png";
                case 77://akantor
                    return "https://i.imgur.com/CKY7zkV.png";
                case 78://bright hypnoc
                    return "https://i.imgur.com/fhF6yZY.png";
                case 79://red lavasioth
                    return "https://i.imgur.com/AzfTTSq.png";
                case 80://espinas
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/sMr1oCg.gif";
                    else
                        return "https://i.imgur.com/cx07Z7B.png";
                case 81://orange espi
                    return "https://i.imgur.com/m8DhwiJ.png";
                case 82://silver hypnoc
                    return "https://i.imgur.com/WZkQYDL.png";
                case 83://akura vashimu
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/jdy96PH.gif";
                    else
                        return "https://i.imgur.com/QiRd5dc.png";
                case 84://akura jebia
                    return "https://i.imgur.com/hOfRrph.png";
                case 85://beru
                    return "https://i.imgur.com/KBCnVhH.png";
                case 86://cactus
                    return "https://i.imgur.com/3pQEtzw.png";
                case 87://gorge objects
                    return "https://i.imgur.com/3pQEtzw.png";
                case 88://gorge rocks
                    return "https://i.imgur.com/3pQEtzw.png";
                case 89://pariapuria
                    if (DataLoader.model.RankBand() == 32 || DataLoader.model.RankBand() == 54)
                        return "https://i.imgur.com/rskDsju.png";
                    else
                        return "https://i.imgur.com/eXaT0PD.png";
                case 90://white espi
                    return "https://i.imgur.com/Uc5mTQi.png";
                case 91://kamu orugaron
                    return "https://i.imgur.com/L9naUDO.png";
                case 92://nono orugaron
                    return "https://i.imgur.com/klyXxuc.png";
                case 93://raviente
                    return "https://i.imgur.com/blsy8Rx.png";
                case 94://dyuragaua
                    return "https://i.imgur.com/dxEtSjL.png";
                case 95://dorag
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/qirZmdn.gif";
                    else if (DataLoader.model.RankBand() == 32)
                        return "https://i.imgur.com/orniFm3.png";
                    else
                        return "https://i.imgur.com/HmNSD8G.png";
                case 96://gurenzeburu
                    return "https://i.imgur.com/gLA5gdi.png";
                case 97://burukku
                    return "https://i.imgur.com/6RIoFpM.png";
                case 98://erupe
                    return "https://i.imgur.com/R3xnyMd.png";
                case 99://ruko
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/BhbCOWn.gif";
                    else
                        return "https://i.imgur.com/84ZBXjW.png";
                case 100://unknown
                    if (DataLoader.model.RankBand() == 70 || DataLoader.model.RankBand() == 54)
                        return "https://i.imgur.com/fplk67z.png";
                    else
                        return "https://i.imgur.com/ssuzTlK.png";
                case 101://gogomoa
                    return "https://i.imgur.com/HBYZoa0.png";
                case 102://kokomoa
                    return "https://i.imgur.com/HBYZoa0.png";
                case 103://taikun
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/FbfYG4P.gif";
                    else
                        return "https://i.imgur.com/CACFYGy.png";
                case 104://abio
                    return "https://i.imgur.com/dxkeQcm.png";
                case 105://kuaru
                    return "https://i.imgur.com/PqTQLGE.png";
                case 106://odiba
                    if (DataLoader.model.RankBand() == 32)
                        return "https://i.imgur.com/zqvxw7m.png";
                    else
                        return "https://i.imgur.com/KvC12wl.png";
                case 107://disu
                    if (DataLoader.model.RankBand() == 54 || DataLoader.model.RankBand() == 55)
                        return "https://i.imgur.com/S8kiGS3.png";
                    else
                        return "https://i.imgur.com/eQnTB2u.png";
                case 108://rebi
                    return "https://i.imgur.com/fdFZFKe.png";
                case 109://anoru
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/LUXuyEi.gif";
                    else
                        return "https://i.imgur.com/XKot29j.png";
                case 110://hyuji
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/jKcdja3.gif";
                    else
                        return "https://i.imgur.com/YqZLy2J.png";
                case 111://mido
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/YJWD5xy.gif";
                    else
                        return "https://i.imgur.com/WvHY8Lf.png";
                case 112://giao
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/oTBfALR.gif";
                    else
                        return "https://i.imgur.com/ZFsUNTL.png";
                case 113://mi ru
                    if (DataLoader.model.RankBand() == 55)
                        return "https://i.imgur.com/FWPACuf.png";
                    else
                        return "https://i.imgur.com/e3l7mhh.png";
                case 114://faru
                    return "https://i.imgur.com/cBHzq5t.png";
                case 115://pokaradon
                    return "https://i.imgur.com/OpYh7mb.png";
                case 116://shantien
                    if (DataLoader.model.RankBand() == 53)
                        return "https://i.imgur.com/y0b0y7G.png";
                    else
                        return "https://i.imgur.com/Ib4dmgd.png";
                case 117://pokara
                    return "https://i.imgur.com/jaKE3QM.png";
                case 118://dummy
                    return "https://i.imgur.com/3pQEtzw.png";
                case 119://goruganosu
                    return "https://i.imgur.com/jwR2xoG.png";
                case 120://aruganosu
                    return "https://i.imgur.com/d9K9HlH.png";
                case 121://baru
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/2WKXvPn.gif";
                    else
                        return "https://i.imgur.com/AvO02Ri.png";
                case 122://zeru
                    return "https://i.imgur.com/XOEGJRu.png";
                case 123://gougarf
                    return "https://i.imgur.com/0TeOdn4.png";
                case 124://uruki
                    return "https://i.imgur.com/fQPpwGE.png";
                case 125://foro
                    return "https://i.imgur.com/p7LWhIe.png";
                case 126://mera
                    return "https://i.imgur.com/iQMDmCN.png";
                case 127://diorex
                    return "https://i.imgur.com/4zi1Kva.png";
                case 128://garuba
                    return "https://i.imgur.com/NHyezpo.png";
                case 129://inagami
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/yDF4h6I.gif";
                    else
                        return "https://i.imgur.com/bioZ1Rx.png";
                case 130://varusaburosu
                    return "https://i.imgur.com/KXLFD8f.png";
                case 131://pobo
                    return "https://i.imgur.com/56tHHHc.png";
                case 132://dure
                    return "https://i.imgur.com/fKVoJ3m.png";
                case 133://UNK
                    return "https://i.imgur.com/3pQEtzw.png";
                case 134://felyne
                    return "https://i.imgur.com/Ry2zu5r.png";
                case 135://blue npc
                    return "https://i.imgur.com/3pQEtzw.png";
                case 136://UNK
                    return "https://i.imgur.com/3pQEtzw.png";
                case 137://cactus
                    return "https://i.imgur.com/3pQEtzw.png";
                case 138://veggie elders
                    return "https://i.imgur.com/3pQEtzw.png";
                case 139:
                    return "https://i.imgur.com/JntsUFx.png";
                case 140://harudo
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/5NPChxD.gif";
                    else
                        return "https://i.imgur.com/daI89CT.png";
                case 141://torid
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/Z5BuMEZ.gif";
                    else
                        return "https://i.imgur.com/1Ru7AMQ.png";
                case 142://gasura
                    if (DataLoader.model.RankBand() >= 64 && DataLoader.model.RankBand() <= 67)
                        return "https://i.imgur.com/vwkiFLs.gif";
                    else
                        return "https://i.imgur.com/OtU0yAB.png";
                case 143://kusubami
                    return "https://i.imgur.com/EMWA1p7.png";
                case 144://yama kurai
                    return "https://i.imgur.com/qy7yTjz.png";
                case 145://3rd phase duremudira
                    return "https://i.imgur.com/fKVoJ3m.png";
                case 146://zinogre
                    if (DataLoader.model.RankBand() >= 54 && DataLoader.model.RankBand() <= 55)
                        return "https://i.imgur.com/bcx6HGf.png";
                    else
                        return "https://i.imgur.com/hU4lRx3.png";
                case 147://jho
                    return "https://i.imgur.com/eGsb66E.png";
                case 148://brachy
                    return "https://i.imgur.com/XrYCP6k.png";
                case 149://berserk
                    return "https://i.imgur.com/HDkGUQL.png";
                case 150://toa
                    return "https://i.imgur.com/muRi2Yz.png";
                case 151://barioth
                    return "https://i.imgur.com/OE88eb9.png";
                case 152://uragaan
                    return "https://i.imgur.com/K3vqmPA.png";
                case 153://styggy
                    return "https://i.imgur.com/oz11SGA.png";
                case 154://guanzorumu
                    if (DataLoader.model.RankBand() >= 54 && DataLoader.model.RankBand() <= 55)
                        return "https://i.imgur.com/kBMkdfr.png";
                    else
                        return "https://i.imgur.com/ZaCyD6O.png";
                case 155://starving jho
                    if (DataLoader.model.RankBand() == 55)
                        return "https://i.imgur.com/HZc9wjS.png";
                    else
                        return "https://i.imgur.com/OYIaUWR.png";
                case 156://UNK
                    return "https://i.imgur.com/3pQEtzw.png";
                case 157://egyurasu
                    return "https://i.imgur.com/3pQEtzw.png";
                case 158://voljang
                    return "https://i.imgur.com/WgzdVtV.png";
                case 159://narga
                    return "https://i.imgur.com/Xi9lXsZ.png";
                case 160://keo
                    return "https://i.imgur.com/tHCL5U3.png";
                case 161://zena
                    return "https://i.imgur.com/cLOEjlO.png";
                case 162://gore
                    return "https://i.imgur.com/Lq6kNtM.png";
                case 163://blinking nargacuga
                    return "https://i.imgur.com/ossvc1M.png";
                case 164://shaggy
                    return "https://i.imgur.com/15V9po1.png";
                case 165://amatsu
                    return "https://i.imgur.com/z5E7rSP.png";
                case 166://elzelion
                    if (DataLoader.model.RankBand() >= 54 && DataLoader.model.RankBand() <= 55)
                        return "https://i.imgur.com/HX8b8EB.png";
                    else
                        return "https://i.imgur.com/CxoXy9E.png";
                case 167://arrogant dure
                    return "https://i.imgur.com/HrSImCm.png";
                case 168://rocks
                    return "https://i.imgur.com/3pQEtzw.png";
                case 169://steve
                    return "https://i.imgur.com/BEeL8xd.png";
                case 170://boggy
                    return "https://i.imgur.com/tbV6QPE.gif";
                case 171://unknown blue barrel
                    return "https://i.imgur.com/3pQEtzw.png";
                case 172://blitzkrieg boga
                    return "https://i.imgur.com/vd6Y06a.png";
                case 173://costumed uruki
                    return "https://i.imgur.com/fQPpwGE.png";
                case 174://sparkling zeru
                    return "https://i.imgur.com/3c8kwQD.png";
                case 175://PSO2 Rappy
                    return "https://i.imgur.com/3pQEtzw.png";
                case 176://king shakalaka
                    return "https://i.imgur.com/UXi0TEu.png";
                default:// "?" icon
                    return "https://i.imgur.com/3pQEtzw.png"; //fatalis
            }
        }

        /// <summary>
        /// Shows the current hp percentage.
        /// </summary>
        /// <returns></returns>
        public bool ShowCurrentHPPercentage()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableCurrentHPPercentage == true)
                return true;
            else
                return false;
        }

        public int currentMonster1MaxHP = 0;
        public string currentMonster1HPPercent = "";

        /// <summary>
        /// Gets the monster1 ehp percent.
        /// </summary>
        /// <returns></returns>
        public string GetMonster1EHPPercent()
        {
            if (currentMonster1MaxHP < int.Parse(DataLoader.model.Monster1HP))
                currentMonster1MaxHP = int.Parse(DataLoader.model.Monster1HP);

            if (currentMonster1MaxHP == 0 || GetMonster1EHP() == 0) //should be OK
                currentMonster1MaxHP = 1;

            if (!(ShowCurrentHPPercentage()))
                return "";

            return string.Format(" ({0:0}%)", (float)int.Parse(DataLoader.model.Monster1HP) / currentMonster1MaxHP * 100.0);
        }

        /// <summary>
        /// Gets the monster1 ehp.
        /// </summary>
        /// <returns></returns>
        public int GetMonster1EHP()
        {
            return DataLoader.model.DisplayMonsterEHP(float.Parse(DataLoader.model.Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat), DataLoader.model.Monster1HPInt(), DataLoader.model.Monster1DefMult());
        }

        /// <summary>
        /// Gets the monster1 maximum ehp.
        /// </summary>
        /// <returns></returns>
        public int GetMonster1MaxEHP()
        {
            return currentMonster1MaxHP;
        }

        //TODO cactus quest shows fatalis        
        /// <summary>
        /// Gets the name of the objective1.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetObjective1Name(int id, bool isLargeImageText = false)
        {
            if (ShowDiscordQuestNames && !(isLargeImageText)) return "";
            string? objValue1;
            bool isNameExists1 = Dictionary.Items.ItemIDs.TryGetValue(id, out objValue1);  //returns true
            //Console.WriteLine(itemValue1); //Print "First"
            //Dictionary.Items.ItemIDs.TryGetValue(1, out itemname);
            return objValue1 + "";
        }

        /// <summary>
        /// Gets the poogie clothes.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetPoogieClothes(int id)
        {
            string? clothesValue1;
            _ = Dictionary.PoogieCostumeList.PoogieCostumeID.TryGetValue(id, out clothesValue1);  //returns true
            //Console.WriteLine(itemValue1); //Print "First"
            //Dictionary.Items.ItemIDs.TryGetValue(1, out itemname);
            return clothesValue1 + "";
        }

        /// <summary>
        /// Gets the objective1 quantity.
        /// </summary>
        /// <returns></returns>
        public string GetObjective1Quantity(bool isLargeImageText = false)
        {
            if (ShowDiscordQuestNames && !(isLargeImageText)) return "";
            if (DataLoader.model.Objective1Quantity() <= 1)
                return "";
            // hunt / capture / slay
            else if (DataLoader.model.ObjectiveType() == 0x1 || DataLoader.model.ObjectiveType() == 0x101 || DataLoader.model.ObjectiveType() == 0x201)
                return DataLoader.model.Objective1Quantity().ToString() + " ";
            else if (DataLoader.model.ObjectiveType() == 0x8004 || DataLoader.model.ObjectiveType() == 0x18004)
                return string.Format("({0} True HP) ",DataLoader.model.Objective1Quantity()*100);
            else
                return DataLoader.model.Objective1Quantity().ToString() + " ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetRepelDamage()
        {
            //1 qty is 100 true hp
            return "";
        }

        /// <summary>
        /// Gets the objective1 current quantity.
        /// </summary>
        /// <returns></returns>
        public string GetObjective1CurrentQuantity(bool isLargeImageText = false)
        {
            if (ShowDiscordQuestNames && !(isLargeImageText)) return "";
            if (DataLoader.model.ObjectiveType() == 0x0 || DataLoader.model.ObjectiveType() == 0x02 || DataLoader.model.ObjectiveType() == 0x1002)
            {
                if (DataLoader.model.Objective1Quantity() <= 1)
                    return "";
                else
                    return DataLoader.model.Objective1CurrentQuantityItem().ToString() + "/";
            }
            else
            {
                if (DataLoader.model.Objective1Quantity() <= 1)
                    return "";
                else
                    //increases when u hit a dead large monster
                    return DataLoader.model.Objective1CurrentQuantityMonster().ToString() + "/";
            }  
        }

        /// <summary>
        /// Gets the max faints
        /// </summary>
        /// <returns></returns>
        public string GetMaxFaints()
        {
            if (
                (
                    DataLoader.model.CaravanOverride() && !
                    (
                        DataLoader.model.QuestID() == 23603 ||
                        DataLoader.model.RankBand() == 70 ||
                        DataLoader.model.QuestID() == 23602 ||
                        DataLoader.model.QuestID() == 23604 ||
                        DataLoader.model.QuestID() == 23588 ||
                        DataLoader.model.QuestID() == 23592 ||
                        DataLoader.model.QuestID() == 23596 ||
                        DataLoader.model.QuestID() == 23601 ||
                        DataLoader.model.QuestID() == 23599 ||
                        DataLoader.model.QuestID() == 23595 ||
                        DataLoader.model.QuestID() == 23591 ||
                        DataLoader.model.QuestID() == 23587 ||
                        DataLoader.model.QuestID() == 23598 ||
                        DataLoader.model.QuestID() == 23594 ||
                        DataLoader.model.QuestID() == 23590 ||
                        DataLoader.model.QuestID() == 23586 ||
                        DataLoader.model.QuestID() == 23597 ||
                        DataLoader.model.QuestID() == 23593 ||
                        DataLoader.model.QuestID() == 23589 ||
                        DataLoader.model.QuestID() == 23585
                    )
                )
                
                ||

                DataLoader.model.QuestID() == 23603 ||
                DataLoader.model.RankBand() == 70 ||
                DataLoader.model.QuestID() == 23602 ||
                DataLoader.model.QuestID() == 23604 ||
                DataLoader.model.QuestID() == 23588 ||
                DataLoader.model.QuestID() == 23592 ||
                DataLoader.model.QuestID() == 23596 ||
                DataLoader.model.QuestID() == 23601 ||
                DataLoader.model.QuestID() == 23599 ||
                DataLoader.model.QuestID() == 23595 ||
                DataLoader.model.QuestID() == 23591 ||
                DataLoader.model.QuestID() == 23587 ||
                DataLoader.model.QuestID() == 23598 ||
                DataLoader.model.QuestID() == 23594 ||
                DataLoader.model.QuestID() == 23590 ||
                DataLoader.model.QuestID() == 23586 ||
                DataLoader.model.QuestID() == 23597 ||
                DataLoader.model.QuestID() == 23593 ||
                DataLoader.model.QuestID() == 23589 ||
                DataLoader.model.QuestID() == 23585
                )
            {
                return DataLoader.model.AlternativeMaxFaints().ToString(); ;
            }
            else
            {
                return DataLoader.model.MaxFaints().ToString();
            }
        }

        #endregion

        #region discord info

        /// <summary>
        /// Determines whether this instance is road.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is road; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRoad()
        {
            if (DataLoader.model.roadOverride() != null && DataLoader.model.roadOverride() == false)
                return true;
            else if (DataLoader.model.roadOverride() != null && DataLoader.model.roadOverride() == true)
                return false;
            else 
                return false;
        }

        /// <summary>
        /// Determines whether this instance is dure quest.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is dure; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDure()
        {
            if (DataLoader.model.getDureName() != "None")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determines whether [is toggeable difficulty].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is toggeable difficulty]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsToggeableDifficulty()
        {
            if (!(IsDure()) && !(IsRavi()) && !(IsRoad()) && DataLoader.model.QuestID() != 0)
            {
                switch (DataLoader.model.RankBand())
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 26:
                    case 31:
                    case 42:
                    case 53:
                        return true;
                    default:
                        return false;
                }
            }
            else
            { 
                return false; 
            }
        }

        /// <summary>
        /// Determines whether this instance is ravi.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is ravi; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRavi()
        {
            if (DataLoader.model.getRaviName() != "None")
                return true;
            else
                return false;
        }

        //dure and road        
        /// <summary>
        /// In the arena?
        /// </summary>
        /// <returns></returns>
        public bool InArena()
        {
            if (DataLoader.model.AreaID() == 398 || DataLoader.model.AreaID() == 458)
                return true;
            else
                return false;
        }

        private int previousRoadFloor = 0;

        private bool StartedRoadElapsedTime = false;

        private bool inDuremudiraArena = false;

        private bool inDuremudiraDoorway = false;

        /// <summary>
        /// Gets a value indicating whether [show discord quest names].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show discord quest names]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowDiscordQuestNames
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                if (s.DiscordQuestNameShown == true)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets the star grade.
        /// </summary>
        /// <param name="isLargeImageText">if set to <c>true</c> [is large image text].</param>
        /// <returns></returns>
        public string GetStarGrade(bool isLargeImageText = false)
        {
            if ((ShowDiscordQuestNames && !(isLargeImageText)) || DataLoader.model.CaravanOverride()) 
                return "";

            if (IsToggeableDifficulty())
                return string.Format("★{0} ", DataLoader.model.StarGrades().ToString());
            else
                return "";
        }

        /// <summary>
        /// Gets the quest information.
        /// </summary>
        /// <returns></returns>
        public string GetQuestInformation()
        {
            if (ShowDiscordQuestNames)
            {
                switch (DataLoader.model.QuestID())
                {
                    case 23648://arrogant repel
                        return "Repel Arrogant Duremudira | ";
                    case 23649://arrogant slay
                        return "Slay Arrogant Duremudira | ";
                    case 23527:// Hunter's Road Multiplayer
                        return "";                    
                   case 23628://solo road
                        return "";
                    case 21731://1st district dure
                    case 21749://sky corridor version
                        //return string.Format("{0}{1} | ", DataLoader.model.FirstDistrictDuremudiraSlays(), DataLoader.model.FirstDistrictDuremudiraEncounters());
                        return "Slay 1st District Duremudira | "; ;
                    case 21746://2nd district dure
                    case 21750://sky corridor version
                        return "Slay 2nd District Duremudira | ";
                        //return string.Format("{0}{1} | ", DataLoader.model.SecondDistrictDuremudiraSlays(), DataLoader.model.SecondDistrictDuremudiraEncounters());
                    default:
                        if ((DataLoader.model.ObjectiveType() == 0x0 || DataLoader.model.ObjectiveType() == 0x02 || DataLoader.model.ObjectiveType() == 0x1002 || DataLoader.model.ObjectiveType() == 0x10) && (DataLoader.model.QuestID() != 23527 && DataLoader.model.QuestID() != 23628 && DataLoader.model.QuestID() != 21731 && DataLoader.model.QuestID() != 21749 && DataLoader.model.QuestID() != 21746 && DataLoader.model.QuestID() != 21750))
                            return string.Format("{0}{1}{2}{3}{4}{5} | ", GetObjectiveNameFromID(DataLoader.model.ObjectiveType(),true), GetObjective1CurrentQuantity(true), GetObjective1Quantity(true), GetRankNameFromID(DataLoader.model.RankBand(),true),GetStarGrade(true), GetObjective1Name(DataLoader.model.Objective1ID(),true));
                        else
                            return string.Format("{0}{1}{2}{3}{4}{5} | ", GetObjectiveNameFromID(DataLoader.model.ObjectiveType(),true), "", GetObjective1Quantity(true), GetRankNameFromID(DataLoader.model.RankBand(),true), GetStarGrade(true), GetRealMonsterName(DataLoader.model.CurrentMonster1Icon,true));
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the raviente event.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetRavienteEvent(int id)
        {
            //if (!(ShowDiscordQuestNames)) return "";
            //string EventValue1;
            Dictionary.RavienteTriggerEvents.RavienteTriggerEventIDs.TryGetValue(id, out string? EventValue1);
            Dictionary.ViolentRavienteTriggerEvents.ViolentRavienteTriggerEventIDs.TryGetValue(id, out string? EventValue2);
            Dictionary.BerserkRavienteTriggerEvents.BerserkRavienteTriggerEventIDs.TryGetValue(id, out string? EventValue3);
            Dictionary.BerserkRavientePracticeTriggerEvents.BerserkRavientePracticeTriggerEventIDs.TryGetValue(id, out string? EventValue4);
            //Console.WriteLine(itemValue1); //Print "First"
            //Dictionary.Items.ItemIDs.TryGetValue(1, out itemname);
            //return EventValue1 + "";

            switch (DataLoader.model.getRaviName())
            {
                default:
                    return "";
                case "Raviente":
                    return EventValue1+"";
                case "Violent Raviente":
                    return EventValue2+"";
                case "Berserk Raviente Practice":
                    return EventValue4+"";
                case "Berserk Raviente":
                    return EventValue3+"";
                case "Extreme Raviente":
                    return EventValue3+"";
            }
        }


        /// <summary>
        /// Get quest state
        /// </summary>
        /// <returns></returns>
        public string GetQuestState()
        {
            if (DataLoader.isInLauncher) //works?
                return "";

            switch (DataLoader.model.QuestState())
            {
                default:
                    return "";
                case 0:
                    return "";
                case 1:
                    return String.Format("Achieved Main Objective | {0} | ", DataLoader.model.Time);
                case 129:
                    return String.Format("Quest Clear! | {0} | ",DataLoader.model.Time);
            }
        }

        /// <summary>
        /// Gets the size of the party.
        /// </summary>
        /// <returns></returns>
        public string GetPartySize()
        {
            if (DataLoader.model.QuestID() == 0 || DataLoader.model.PartySize() == 0 || isInLauncher() == "NULL" || isInLauncher() == "Yes")
            {
                return "";
            }
            else
            {
                return string.Format("Party: {0}/{1} | ", DataLoader.model.PartySize(), DataLoader.model.PartySizeMax());
            }
        }
       

        /// <summary>
        /// Updates the discord RPC.
        /// </summary>
        private void UpdateDiscordRPC()
        {
            if (!(isDiscordRPCRunning))
            {
                return;
            }

            presenceTemplate.Details = string.Format("{0}{1}{2}{3}{4}{5}", GetPartySize(), GetQuestState(),GetCaravanScore(), GetOverlayMode(), GetAreaName(DataLoader.model.AreaID()), GetGameMode(DataLoader.isHighGradeEdition));

            //quest ids:
            //mp road: 23527
            //solo road: 23628
            //1st district dure: 21731
            //2nd district dure: 21746
            //1st district dure sky corridor: 21749
            //2nd district dure sky corridor: 21750
            //arrogant dure repel: 23648
            //arrogant dure slay: 23649
            //urgent tower: 21751
            //4th district dure: 21748
            //3rd district dure: 21747
            //3rd district dure 2: 21734
            //UNUSED sky corridor: 21730
            //sky corridor prologue: 21729
            //raviente 62105
            //raviente carve 62108
            ///violent raviente 62101
            ///violent carve 62104
            //berserk slay practice 55796
            //berserk support practice 1 55802
            //berserk support practice 2 55803
            //berserk support practice 3 55804
            //berserk support practice 4 55805
            //berserk support practice 5 55806
            //berserk practice carve 55807
            //berserk slay  54751
            //berserk support 1 54756
            //berserk support 2 54757
            //berserk support 3 54758
            //berserk support 4 54759
            //berserk support 5 54760
            //berserk carve 54761
            //extreme slay (musou table 54) 55596 
            //extreme support 1 55602
            //extreme support 2 55603
            //extreme support 3 55604
            //extreme support 4 55605
            //extreme support 5 55606
            //extreme carve 55607

            //DataLoader.model.DisplayMonsterEHP(float.Parse(Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat)

            //Info
            if ((DataLoader.model.QuestID() != 0 && DataLoader.model.TimeDefInt() != DataLoader.model.TimeInt() && int.Parse(DataLoader.model.ATK) > 0) || ((DataLoader.model.QuestID() == 21731 || DataLoader.model.QuestID() == 21746 || DataLoader.model.QuestID() == 21749 || DataLoader.model.QuestID() == 21750 || DataLoader.model.QuestID() == 23648 || DataLoader.model.QuestID() == 23649 || DataLoader.model.QuestID() == 21748 || DataLoader.model.QuestID() == 21747 || DataLoader.model.QuestID() == 21734) && int.Parse(DataLoader.model.ATK) > 0))
            {
                //inQuest = true;

                //switch (GetDiscordTimerMode())
                //{
                //    case "Time Left":
                //        presenceTemplate.Timestamps = Timestamps.FromTimeSpan((double)DataLoader.model.TimeDefInt() / 30.0);
                //        break;
                //    case "Time Elapsed":
                //        presenceTemplate.Timestamps = Timestamps.Now;
                //        break;
                //    default:
                //        presenceTemplate.Timestamps = Timestamps.FromTimeSpan((double)DataLoader.model.TimeDefInt() / 30.0);
                //        break;
                //}

                switch (DataLoader.model.QuestID())
                {
                    case 23527:// Hunter's Road Multiplayer
                        presenceTemplate.State = String.Format("Multiplayer Floor: {0} ({1}/{2} Max/Total) | RP: {3} | White Fatalis: {4}/{5} (Slain/Encounters)",DataLoader.model.RoadFloor()+1,DataLoader.model.RoadMaxStagesMultiplayer(),DataLoader.model.RoadTotalStagesMultiplayer(),DataLoader.model.RoadPoints(),DataLoader.model.RoadFatalisSlain(),DataLoader.model.RoadFatalisEncounters());
                        break;
                    case 23628://solo road
                        presenceTemplate.State = String.Format("Solo Floor: {0} ({1}/{2} Max/Total) | RP: {3} | White Fatalis: {4}/{5} (Slain/Encounters)", DataLoader.model.RoadFloor()+1, DataLoader.model.RoadMaxStagesSolo(), DataLoader.model.RoadTotalStagesSolo(),DataLoader.model.RoadPoints(),DataLoader.model.RoadFatalisSlain(), DataLoader.model.RoadFatalisEncounters());
                        break;
                    case 21731://1st district dure
                    case 21749://sky corridor version
                        presenceTemplate.State = String.Format("{0}{1}{2}{3}{4}{5} | Slain: {6} | Encounters: {7}", GetQuestNameFromID(DataLoader.model.QuestID()), GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), "", GetObjective1Quantity(), GetRankNameFromID(DataLoader.model.RankBand()), GetRealMonsterName(DataLoader.model.CurrentMonster1Icon),DataLoader.model.FirstDistrictDuremudiraSlays(),DataLoader.model.FirstDistrictDuremudiraEncounters());
                        break;
                    case 21746://2nd district dure
                    case 21750://sky corridor version
                        presenceTemplate.State = String.Format("{0}{1}{2}{3}{4}{5} | Slain: {6} | Encounters: {7}", GetQuestNameFromID(DataLoader.model.QuestID()), GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), "", GetObjective1Quantity(), GetRankNameFromID(DataLoader.model.RankBand()), GetRealMonsterName(DataLoader.model.CurrentMonster1Icon),DataLoader.model.SecondDistrictDuremudiraSlays(), DataLoader.model.SecondDistrictDuremudiraEncounters());
                        break;
                    case 62105://raviente quests
                    case 62108:
                    case 62101:
                    case 62104:
                    case 55796:
                    case 55802:
                    case 55803:
                    case 55804:
                    case 55805:
                    case 55806:
                    case 55807:
                    case 54751:
                    case 54756:
                    case 54757:
                    case 54758:
                    case 54759:
                    case 54760:
                    case 54761:
                    case 55596://extreme
                    case 55602:
                    case 55603:
                    case 55604:
                    case 55605:
                    case 55606:
                    case 55607:
                        presenceTemplate.State = String.Format("{0}{1}{2}{3}{4}{5}{6} | True Raw: {7} (Max {8}) | Hits: {9}", GetQuestNameFromID(DataLoader.model.QuestID()), GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), "", GetObjective1Quantity(), GetRankNameFromID(DataLoader.model.RankBand()), GetStarGrade(), GetRealMonsterName(DataLoader.model.CurrentMonster1Icon), DataLoader.model.ATK, DataLoader.model.HighestAtk, DataLoader.model.HitCount);
                        break;
                    default:
                        if ((DataLoader.model.ObjectiveType() == 0x0 || DataLoader.model.ObjectiveType() == 0x02 || DataLoader.model.ObjectiveType() == 0x1002 || DataLoader.model.ObjectiveType() == 0x10) && (DataLoader.model.QuestID() != 23527 && DataLoader.model.QuestID() != 23628 && DataLoader.model.QuestID() != 21731 && DataLoader.model.QuestID() != 21749 && DataLoader.model.QuestID() != 21746 && DataLoader.model.QuestID() != 21750))
                            presenceTemplate.State = String.Format("{0}{1}{2}{3}{4}{5}{6} | True Raw: {7} (Max {8}) | Hits: {9}",GetQuestNameFromID(DataLoader.model.QuestID()),GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), GetObjective1CurrentQuantity(), GetObjective1Quantity(), GetRankNameFromID(DataLoader.model.RankBand()), GetStarGrade(), GetObjective1Name(DataLoader.model.Objective1ID()), DataLoader.model.ATK, DataLoader.model.HighestAtk, DataLoader.model.HitCount);
                        else
                            presenceTemplate.State = String.Format("{0}{1}{2}{3}{4}{5}{6} | True Raw: {7} (Max {8}) | Hits: {9}",GetQuestNameFromID(DataLoader.model.QuestID()),GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), "", GetObjective1Quantity(),GetRankNameFromID(DataLoader.model.RankBand()), GetStarGrade(),GetRealMonsterName(DataLoader.model.CurrentMonster1Icon),DataLoader.model.ATK,DataLoader.model.HighestAtk,DataLoader.model.HitCount);
                        break;
                }

                //Gathering/etc
                if ((DataLoader.model.ObjectiveType() == 0x0 || DataLoader.model.ObjectiveType() == 0x02 || DataLoader.model.ObjectiveType() == 0x1002) && (DataLoader.model.QuestID() != 23527 && DataLoader.model.QuestID() != 23628 && DataLoader.model.QuestID() != 21731 && DataLoader.model.QuestID() != 21749 && DataLoader.model.QuestID() != 21746 && DataLoader.model.QuestID() != 21750))
                {
                    presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                    presenceTemplate.Assets.LargeImageText = string.Format("{0}{1}",GetQuestInformation(),GetAreaName(DataLoader.model.AreaID()));
                }
                //Tenrou Sky Corridor areas
                else if (DataLoader.model.AreaID() == 391 || DataLoader.model.AreaID() == 392 || DataLoader.model.AreaID() == 394 || DataLoader.model.AreaID() == 415 || DataLoader.model.AreaID() == 416) 
                {
                    presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                    presenceTemplate.Assets.LargeImageText = string.Format("{0}{1}", GetQuestInformation(), GetAreaName(DataLoader.model.AreaID()));
                }
                //Duremudira Doors
                else if (DataLoader.model.AreaID() == 399 || DataLoader.model.AreaID() == 414) 
                {
                    presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                    presenceTemplate.Assets.LargeImageText = string.Format("{0}{1}", GetQuestInformation(), GetAreaName(DataLoader.model.AreaID()));
                }
                //Duremudira Arena
                else if (DataLoader.model.AreaID() == 398) 
                {
                    presenceTemplate.Assets.LargeImageKey = getMonsterIcon(DataLoader.model.LargeMonster1ID());
                    presenceTemplate.Assets.LargeImageText = string.Format("{0}{1}/{2}{3}", GetQuestInformation(), GetMonster1EHP(), GetMonster1MaxEHP(), GetMonster1EHPPercent());
                }
                //Hunter's Road Base Camp
                else if (DataLoader.model.AreaID() == 459) 
                {
                    presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                    presenceTemplate.Assets.LargeImageText = string.Format("{0}{1} | Faints: {2}/{3}", GetQuestInformation(), GetAreaName(DataLoader.model.AreaID()),DataLoader.model.CurrentFaints(),GetMaxFaints());
                }
                //Raviente
                else if (DataLoader.model.AreaID() == 309 || (DataLoader.model.AreaID() >= 311 && DataLoader.model.AreaID() <= 321) || (DataLoader.model.AreaID() >= 417 && DataLoader.model.AreaID() <= 422) || DataLoader.model.AreaID() == 437 || (DataLoader.model.AreaID() >= 440 && DataLoader.model.AreaID() <= 444))
                {
                    presenceTemplate.Assets.LargeImageKey = getMonsterIcon(DataLoader.model.LargeMonster1ID());
                    presenceTemplate.Assets.LargeImageText = string.Format("{0}{1}/{2}{3} | Faints: {4}/{5} | Points: {6} | {7}", GetQuestInformation(), GetMonster1EHP(), GetMonster1MaxEHP(), GetMonster1EHPPercent(), DataLoader.model.CurrentFaints(), GetMaxFaints(), DataLoader.model.GreatSlayingPoints(), GetRavienteEvent(DataLoader.model.RavienteTriggeredEvent()));
                }
                else
                {
                    presenceTemplate.Assets.LargeImageKey = getMonsterIcon(DataLoader.model.LargeMonster1ID());
                    presenceTemplate.Assets.LargeImageText = string.Format("{0}{1}/{2}{3} | Faints: {4}/{5}", GetQuestInformation(), GetMonster1EHP(), GetMonster1MaxEHP(), GetMonster1EHPPercent(), DataLoader.model.CurrentFaints(), GetMaxFaints());
                }
            }
            else if (DataLoader.model.QuestID() == 0)
            {
                //inQuest = false;

                switch(DataLoader.model.AreaID())
                {
                    case 0: //Loading
                        presenceTemplate.State = "Loading...";
                        break;
                    case 87: //Kokoto Village
                    case 131: //Dundorma areas
                    case 132:
                    case 133:
                    case 134:
                    case 135:
                    case 136:
                    case 200: //Mezeporta
                    case 201://Hairdresser
                    case 206://Old Town Areas
                    case 207:
                    case 210://Private Bar
                    case 211://Rasta Bar
                    case 244://Code Claiming Room
                    case 282://Cities Map
                    case 340://SR Rooms
                    case 341:
                    case 397://Mezeporta Dupe(non-HD)
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Diva Skill: {3} ({4} Left) | Poogie Item: {5}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetDivaSkillNameFromID(DataLoader.model.DivaSkill()), DataLoader.model.DivaSkillUsesLeft(), GetItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 173:// My House (original)
                    case 175://My House (MAX)
                        presenceTemplate.State = string.Format("GR: {0} | Partner Lv: {1} | Armor Color: {2} | GCP: {3}", DataLoader.model.GRankNumber(), DataLoader.model.PartnerLevel(), getArmorColor(), DataLoader.model.GCP());
                        break;

                    case 202://Guild Halls
                    case 203:
                    case 204:
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 205://Pugi Farm
                        presenceTemplate.State = string.Format("GR: {0} | Poogie Points: {1} | Poogie Clothes: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.PoogiePoints(), GetPoogieClothes(DataLoader.model.PoogieCostume()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 256://Caravan Areas
                    case 260:
                    case 261:
                    case 262:
                    case 263:
                        presenceTemplate.State = string.Format("CP: {0} | Gg: {1} | g: {2} | Gem Lv: {3} | Great Slaying Points: {4}", DataLoader.model.CaravanPoints(), DataLoader.model.RaviGg(), DataLoader.model.Ravig(), DataLoader.model.CaravenGemLevel()+1, DataLoader.model.GreatSlayingPointsSaved());
                        break;

                    case 257://Blacksmith
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | GZenny: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), DataLoader.model.GZenny());
                        break;

                    case 264://Gallery
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Score: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), DataLoader.model.GalleryEvaluationScore());
                        break;

                    case 265://Guuku Farm
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 283://Halk Area TODO partnya lv
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | PNRP: {2} | Halk Fullness: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), DataLoader.model.PartnyaRankPoints(), DataLoader.model.HalkFullness());
                        break;

                    case 286://PvP Room
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 379://Diva Hall
                    case 445:
                        presenceTemplate.State = string.Format("GR: {0} | Diva Skill: {1} ({2} Left) | Diva Bond: {3} | Items Given: {4}", DataLoader.model.GRankNumber(), GetDivaSkillNameFromID(DataLoader.model.DivaSkill()),DataLoader.model.DivaSkillUsesLeft(),DataLoader.model.DivaBond(), DataLoader.model.DivaItemsGiven());
                        break;

                    // case 458:// "Hunter's Road 1 Area 1" },
                    //case 459:// "Hunter's Road 2 Base Camp" },

                    case 462://MezFez Entrance
                    case 463: //Volpkun Together
                    case 465://MezFez Minigame
                        presenceTemplate.State = string.Format("GR: {0} | MezFes Points: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.MezeportaFestivalPoints(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 464://Uruki Pachinko
                        presenceTemplate.State = string.Format("Score: {0} | Chain: {1} | Fish: {2} | Mushroom: {3} | Seed: {4} | Meat: {5}", DataLoader.model.UrukiPachinkoScore(), DataLoader.model.UrukiPachinkoChain(), DataLoader.model.UrukiPachinkoFish(), DataLoader.model.UrukiPachinkoMushroom(), DataLoader.model.UrukiPachinkoSeed(), DataLoader.model.UrukiPachinkoMeat());
                        break;

                    case 466://Guuku Scoop
                        presenceTemplate.State = string.Format("Score: {0} | Small Guuku: {1} | Medium Guuku: {2} | Large Guuku: {3} | Golden Guuku: {4}", DataLoader.model.GuukuScoopScore(), DataLoader.model.GuukuScoopSmall(), DataLoader.model.GuukuScoopMedium(), DataLoader.model.GuukuScoopLarge(),DataLoader.model.GuukuScoopGolden());
                        break;

                    case 467://Nyanrendo
                        presenceTemplate.State = string.Format("Score: {0}", DataLoader.model.NyanrendoScore());
                        break;

                    case 468://Panic Honey
                        presenceTemplate.State = string.Format("Honey: {0}", DataLoader.model.PanicHoneyScore());
                        break;

                    case 469://Dokkan Battle Cats
                        presenceTemplate.State = string.Format("Score: {0} | Scale: {1} | Shell: {2} | Camp: {3}", DataLoader.model.DokkanBattleCatsScore(), DataLoader.model.DokkanBattleCatsScale(), DataLoader.model.DokkanBattleCatsShell(), DataLoader.model.DokkanBattleCatsCamp());
                        break;

                    default: //same as Mezeporta
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        break;
                }

                presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                presenceTemplate.Assets.LargeImageText = GetAreaName(DataLoader.model.AreaID());
            }

            //Timer
            if ((DataLoader.model.QuestID() != 0 && !inQuest && DataLoader.model.TimeDefInt() > DataLoader.model.TimeInt() && int.Parse(DataLoader.model.ATK) > 0) || (IsRoad() || IsDure()))
            {
                inQuest = true;

                if (!(IsRoad() || IsDure()))
                {
                    presenceTemplate.Timestamps = GetDiscordTimerMode() switch
                    {
                        "Time Left" => Timestamps.FromTimeSpan((double)DataLoader.model.TimeDefInt() / 30.0),
                        "Time Elapsed" => Timestamps.Now,
                        //dure doorway too
                        _ => Timestamps.FromTimeSpan((double)DataLoader.model.TimeDefInt() / 30.0),
                    };
                }


                if (IsRoad()) 
                {
                    switch (DataLoader.model.GetRoadTimerResetMode())
                    {
                    case "Always":
                        if (DataLoader.model.AreaID() == 458)//Hunter's Road Area 1
                        {
                            //previousRoadFloor = DataLoader.model.RoadFloor() + 1;
                            break;
                        }

                        else if (DataLoader.model.AreaID() == 459)//Hunter's Road Base Camp
                        {
                            if (DataLoader.model.RoadFloor() + 1 > previousRoadFloor)
                            {
                                inQuest = false;
                                currentMonster1MaxHP = 0;//reset values
                                previousRoadFloor = DataLoader.model.RoadFloor() + 1;
                                presenceTemplate.Timestamps = GetDiscordTimerMode() switch
                                {
                                    "Time Left" => Timestamps.FromTimeSpan((double)DataLoader.model.TimeInt() / 30.0),
                                    "Time Elapsed" => Timestamps.Now,
                                    _ => Timestamps.FromTimeSpan((double)DataLoader.model.TimeInt() / 30.0),
                                };
                            }
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "Never":
                        if (DataLoader.model.AreaID() == 458)//Hunter's Road Area 1
                        {
                            //previousRoadFloor = DataLoader.model.RoadFloor() + 1;
                            break;
                        }

                        else if (DataLoader.model.AreaID() == 459)//Hunter's Road Base Camp
                        {
                            if (DataLoader.model.RoadFloor() + 1 > previousRoadFloor)
                            {
                                inQuest = false;
                                currentMonster1MaxHP = 0;//reset values
                                previousRoadFloor = DataLoader.model.RoadFloor() + 1;

                                if (!(StartedRoadElapsedTime))
                                {
                                    StartedRoadElapsedTime = true;
                                    presenceTemplate.Timestamps = Timestamps.Now;
                                }
                            }
                            break;
                        }
                        else
                        {
                            break;
                        }
                    default:
                        if (DataLoader.model.AreaID() == 458)//Hunter's Road Area 1
                        {
                            //previousRoadFloor = DataLoader.model.RoadFloor() + 1;
                            break;
                        }

                        else if (DataLoader.model.AreaID() == 459)//Hunter's Road Base Camp
                        {
                            if (DataLoader.model.RoadFloor() + 1 > previousRoadFloor)
                            {
                                inQuest = false;
                                currentMonster1MaxHP = 0;//reset values
                                previousRoadFloor = DataLoader.model.RoadFloor() + 1;

                                if (!(StartedRoadElapsedTime))
                                {
                                    StartedRoadElapsedTime = true;
                                    presenceTemplate.Timestamps = Timestamps.Now;
                                }
                            }
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (IsDure())
                {

                    switch (DataLoader.model.AreaID())
                    {
                    case 398://Duremudira Arena

                        if (!(inDuremudiraArena))
                        {
                            inDuremudiraArena = true;

                            if (DataLoader.model.QuestID() == 23649)//Arrogant Dure Slay
                            {
                                presenceTemplate.Timestamps = GetDiscordTimerMode() switch
                                {
                                    "Time Left" => Timestamps.FromTimeSpan(600),
                                    "Time Elapsed" => Timestamps.Now,
                                    _ => Timestamps.FromTimeSpan(600),
                                };
                                
                            }
                            else
                            {
                                presenceTemplate.Timestamps = GetDiscordTimerMode() switch
                                {
                                    "Time Left" => Timestamps.FromTimeSpan(1200),
                                    "Time Elapsed" => Timestamps.Now,
                                    _ => Timestamps.FromTimeSpan(1200),
                                };
                            }
                        }
                        break;

                    default:
                        if (!(inDuremudiraDoorway))
                        {
                            inDuremudiraDoorway = true;
                            presenceTemplate.Timestamps = Timestamps.Now;
                        }
                        break;
                    }
                }
            }
            else if (DataLoader.model.QuestID() == 0 && inQuest && int.Parse(DataLoader.model.ATK) == 0)
            { 
                inQuest = false;

                //reset values
                currentMonster1MaxHP = 0;
                previousRoadFloor = 0;
                StartedRoadElapsedTime = false;
                inDuremudiraArena = false;
                inDuremudiraDoorway = false;

                presenceTemplate.Timestamps = Timestamps.Now; 
            }

            //SmallInfo
            presenceTemplate.Assets.SmallImageKey = getWeaponIconFromID(DataLoader.model.WeaponType());

            if (GetHunterName != "" && GetGuildName != "")
            {
                presenceTemplate.Assets.SmallImageText = String.Format("{0} | {1} | GSR: {2} | {3} Style | Caravan Skills: {4}", GetHunterName, GetGuildName, DataLoader.model.GSR(), GetWeaponStyleFromID(DataLoader.model.WeaponStyle()),GetCaravanSkills());
            }
            //string currentState = presenceTemplate.State;
            //DiscordRPC.Timestamps currentTimestamps = presenceTemplate.Timestamps;
            //string currentLargeImageKey = presenceTemplate.Assets.LargeImageKey;
            //string currentLargeImageText = presenceTemplate.Assets.LargeImageText;
            //string currentSmallImageKey = presenceTemplate.Assets.SmallImageKey;
            //string currentSmallImageText = presenceTemplate.Assets.SmallImageText;
            //DiscordRPC.Button[] currentButtons = presenceTemplate.Buttons;

            Client.SetPresence(presenceTemplate);
        }

        #endregion

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
                //case "PlayerInfo":
                //    s.PlayerInfoX = (double)(pos.X - XOffset);
                //    s.PlayerInfoY = (double)(pos.Y - YOffset);
                //    break;
                case "TimerInfo":
                    s.TimerX = (double)(pos.X - XOffset);
                    s.TimerY = (double)(pos.Y - YOffset);
                    break;
                case "HitCountInfo":
                    s.HitCountX = (double)(pos.X - XOffset);
                    s.HitCountY = (double)(pos.Y - YOffset);
                    break;
                case "PlayerAtkInfo":
                    s.PlayerAtkX = (double)(pos.X - XOffset);
                    s.PlayerAtkY = (double)(pos.Y - YOffset);
                    break;
                //case "MonsterHpBars":
                //    s.HealthBarsX = (double)(pos.X - XOffset);
                //    s.HealthBarsY = (double)(pos.Y - YOffset);
                //    break;
                case "Monster1HpBar":
                    s.Monster1HealthBarX = (double)(pos.X - XOffset);
                    s.Monster1HealthBarY = (double)(pos.Y - YOffset);
                    break;
                case "Monster2HpBar":
                    s.Monster2HealthBarX = (double)(pos.X - XOffset);
                    s.Monster2HealthBarY = (double)(pos.Y - YOffset);
                    break;
                case "Monster3HpBar":
                    s.Monster3HealthBarX = (double)(pos.X - XOffset);
                    s.Monster3HealthBarY = (double)(pos.Y - YOffset);
                    break;
                case "Monster4HpBar":
                    s.Monster4HealthBarX = (double)(pos.X - XOffset);
                    s.Monster4HealthBarY = (double)(pos.Y - YOffset);
                    break;
                //case "MonsterStatusInfo":
                //    s.MonsterStatusInfoX = (double)(pos.X - XOffset);
                //    s.MonsterStatusInfoY = (double)(pos.Y - YOffset);
                //    break;
                case "MonsterAtkMultInfo":
                    s.MonsterAtkMultX = (double)(pos.X - XOffset);
                    s.MonsterAtkMultY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterDefrateInfo":
                    s.MonsterDefrateX = (double)(pos.X - XOffset);
                    s.MonsterDefrateY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterSizeInfo":
                    s.MonsterSizeX = (double)(pos.X - XOffset);
                    s.MonsterSizeY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterPoisonInfo":
                    s.MonsterPoisonX = (double)(pos.X - XOffset);
                    s.MonsterPoisonY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterSleepInfo":
                    s.MonsterSleepX = (double)(pos.X - XOffset);
                    s.MonsterSleepY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterParaInfo":
                    s.MonsterParaX = (double)(pos.X - XOffset);
                    s.MonsterParaY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterBlastInfo":
                    s.MonsterBlastX = (double)(pos.X - XOffset);
                    s.MonsterBlastY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterStunInfo":
                    s.MonsterStunX = (double)(pos.X - XOffset);
                    s.MonsterStunY = (double)(pos.Y - YOffset);
                    break;
                case "SharpnessInfo":
                    s.SharpnessInfoX = (double)(pos.X - XOffset);
                    s.SharpnessInfoY = (double)(pos.Y - YOffset);
                    break;
                case "MonsterPartThreshold":
                    s.Monster1PartX = (double)(pos.X - XOffset);
                    s.Monster1PartY = (double)(pos.Y - YOffset);
                    break;
                case "Monster1Icon":
                    s.Monster1IconX = (double)(pos.X - XOffset);
                    s.Monster1IconY = (double)(pos.Y - YOffset);
                    break;
                case "MapImage":
                    s.MapX = (double)(pos.X - XOffset);
                    s.MapY = (double)(pos.Y - YOffset);
                    break;
            }

        }

        /// <summary>
        /// Does the drag drop.
        /// </summary>
        /// <param name="item">The item.</param>
        private void DoDragDrop(FrameworkElement? item)
        {
            if (item == null)
                return;
            DragDrop.DoDragDrop(item, new DataObject(DataFormats.Xaml, item), DragDropEffects.Move);
        }

        /// <summary>
        /// Handles the Drop event of the MainGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void MainGrid_Drop(object sender, DragEventArgs e)
        {
            if (MovingObject != null)
                MovingObject.IsHitTestVisible = true;
            MovingObject = null;
        }

        /// <summary>
        /// Elements the mouse left button down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
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
            Cleanup();
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
            Cleanup();
            Application.Current.Shutdown();
        }

        private void ReloadButton_Key()
        {
            Cleanup();
            Application.Current.Shutdown();
            System.Windows.Forms.Application.Restart();
        }

        private void OpenConfigButton_Key()
        {
            if (IsDragConfigure) return;

            if (DataLoader.isInLauncher)
                System.Windows.MessageBox.Show("Using the configuration menu outside of the game might cause slow performance", "Warning", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

            if (configWindow == null || !configWindow.IsLoaded)
                configWindow = new(this);
            configWindow.Show();
            DataLoader.model.Configuring = true;
        }

        private void CloseButton_Key()
        {
            Cleanup();
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
/// [] Not Done
/// [X] Done
/// [O] WIP
/// 
/// ## Look at hp bar to make better
/// [O] damage numbers
/// ## look at other popular overlays and steal their design
/// fix road stuff
/// ## implement for monsters 1-4
/// select monster 
/// ## configuration
/// move data translation into dataloader out of the abstract address model
/// ## remove unnecessary fields in DataLoader
/// figure out way to make it work for all monsters with the same functions (use list u dunmbass)
/// ## figure out a way to make templates
/// [O] body parts
/// [] status panel
/// [X] Tooltips for Configuration
/// [X]Configuration for Damage numbers
/// </TODO>
