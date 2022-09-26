using DiscordRPC;
using MHFZ_Overlay.addresses;
using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
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

        public DiscordRpcClient Client { get; private set; }

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

        void Setup()
        {
            Client = new DiscordRpcClient(GetDiscordClientID);  //Creates the client
            Client.Initialize();                            //Connects the client
        }

        //Dispose client
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
            Details = "Overlay 0.2.0 by Imulion",
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
                    new DiscordRPC.Button() {Label = "Overlay Repository", Url = "https://github.com/Imulion/MHFZ_Overlay"},
                    new DiscordRPC.Button() { Label = "Discord RPC C# Dev Site", Url = "https://lachee.dev/" }
                }
        };

        /// <summary>
        /// Is the main loop currently running?
        /// </summary>
        public static bool isDiscordRPCRunning = false;

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
                    new DiscordRPC.Button() {Label = "Overlay Repository", Url = "https://github.com/Imulion/MHFZ_Overlay"},
                    new DiscordRPC.Button() { Label = "Discord RPC C# Dev Site", Url = "https://lachee.dev/" }
                    };
                //}

                if (GetDiscordServerInvite != "")
                {
                    presenceTemplate.Buttons = new DiscordRPC.Button[] { }; ;
                    presenceTemplate.Buttons = new DiscordRPC.Button[]
                    {
                    new DiscordRPC.Button() {Label = "Overlay Repository", Url = "https://github.com/Imulion/MHFZ_Overlay"},
                    new DiscordRPC.Button() { Label = "Join Discord Server", Url = String.Format("https://discord.com/invite/{0}",GetDiscordServerInvite)}
                    };
                }

                Client.SetPresence(presenceTemplate);
                isDiscordRPCRunning = true;
            }
        }

        //Main entry point?
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
            InitializeDiscordRPC();
        }


        int counter = 0;
        int prevTime = 0;

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

        //does this sometimes bug?
        //the UI flashes at least once when loading into quest
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
            DataLoader.model.ShowMonsterPartHP = v && s.PartThresholdShown;
            DataLoader.model.ShowMonster1Icon = v && s.Monster1IconShown;
        }

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
        }

        public string getAreaName(int id)
        {
            if (id == 0)
                return "Loading...";
            Dictionary.MapAreaList.MapAreaID.TryGetValue(id, out string? areaname);
            return areaname + "";
        }

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

        public string GetWeaponStyleFromID(int id)
        {
            switch (id)
            {
                case 0: return "Earth";
                case 1: return "Heaven";
                case 2: return "Storm";
                case 3: return "Extreme";
                default: return "None";
            } 
        }

        public string getArmorSkill(int id)
        {
            Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(id, out string? skillname);
            if (skillname == "")
                return "None";
            else
                return skillname + "";
        }

        public string getItemName(int id)
        {
            string itemValue1;
            bool isItemExists1 = Dictionary.Items.ItemIDs.TryGetValue(id, out itemValue1);  //returns true
            //Console.WriteLine(itemValue1); //Print "First"
            //Dictionary.Items.ItemIDs.TryGetValue(1, out itemname);
            return itemValue1 + "";
        }

        public bool itemsLoaded = false;

        public string getWeaponNameFromID(int id)
        {
            Dictionary.WeaponList.WeaponID.TryGetValue(id, out string? weaponname);
            return weaponname + "";
        }

        public string getArmorColor()
        {
            Dictionary.ArmorColorList.ArmorColorID.TryGetValue(DataLoader.model.ArmorColor(), out string? colorname);
            return colorname+"";
        }

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

            switch (weaponIconName)
            {
                default: return "https://i.imgur.com/9OkLYAz.png"; //transcend
                case "sns": return "https://i.imgur.com/hVCDfnA.png";
                case "sns_green": return "https://i.imgur.com/U61zPJa.png";
                case "sns_red": return "https://i.imgur.com/ZGCd1lH.png";
                case "sns_pink": return "https://i.imgur.com/O4qyBfI.png";
                case "sns_blue": return "https://i.imgur.com/dQvflcw.png";
                case "sns_navy": return "https://i.imgur.com/vdeSnZh.png";
                case "sns_cyan": return "https://i.imgur.com/37gfP8P.png";
                case "sns_purple": return "https://i.imgur.com/x7dFt4G.png";
                case "sns_orange": return "https://i.imgur.com/bz22IiF.png";
                case "sns_yellow": return "https://i.imgur.com/wKItSbP.png";
                case "sns_grey": return "https://i.imgur.com/U25Xxfj.png";
                case "sns_rainbow": return "https://i.imgur.com/3a6OI1V.gif";
                case "ds": return "https://i.imgur.com/JIFNgz9.png";
                case "ds_green": return "https://i.imgur.com/MEWrHcC.png";
                case "ds_red": return "https://i.imgur.com/dzIoOF2.png";
                case "ds_pink": return "https://i.imgur.com/OfUVJy6.png";
                case "ds_blue": return "https://i.imgur.com/fUvIuCl.png";
                case "ds_navy": return "https://i.imgur.com/oPz7WAA.png";
                case "ds_cyan": return "https://i.imgur.com/Lkf6v4A.png";
                case "ds_purple": return "https://i.imgur.com/b5Ly09E.png";
                case "ds_orange": return "https://i.imgur.com/LdHWvui.png";
                case "ds_yellow": return "https://i.imgur.com/2A8UXdT.png";
                case "ds_grey": return "https://i.imgur.com/snw6dPs.png";
                case "ds_rainbow": return "https://i.imgur.com/eWTRTJl.gif";
                case "gs": return "https://i.imgur.com/vLxcWM8.png";
                case "gs_green": return "https://i.imgur.com/9puI44e.png";
                case "gs_red": return "https://i.imgur.com/Xhs5yJj.png";
                case "gs_pink": return "https://i.imgur.com/DXI9FHs.png";
                case "gs_blue": return "https://i.imgur.com/GxWofdH.png";
                case "gs_navy": return "https://i.imgur.com/ZM1Isqt.png";
                case "gs_cyan": return "https://i.imgur.com/tO2TrkB.png";
                case "gs_purple": return "https://i.imgur.com/ijgJ69Y.png";
                case "gs_orange": return "https://i.imgur.com/CWHEhAi.png";
                case "gs_yellow": return "https://i.imgur.com/ZpGOD3z.png";
                case "gs_grey": return "https://i.imgur.com/82HhSFD.png";
                case "gs_rainbow": return "https://i.imgur.com/WnRuqll.gif";
                case "ls": return "https://i.imgur.com/qdA0x3k.png";
                case "ls_green": return "https://i.imgur.com/9LvQVQ7.png";
                case "ls_red": return "https://i.imgur.com/oc0ExLi.png";
                case "ls_pink": return "https://i.imgur.com/jjBGbGu.png";
                case "ls_blue": return "https://i.imgur.com/AZ606vb.png";
                case "ls_navy": return "https://i.imgur.com/M6mmOpO.png";
                case "ls_cyan": return "https://i.imgur.com/qHFbpoJ.png";
                case "ls_purple": return "https://i.imgur.com/ICgTu6S.png";
                case "ls_orange": return "https://i.imgur.com/XPYDego.png";
                case "ls_yellow": return "https://i.imgur.com/H4vJFd1.png";
                case "ls_grey": return "https://i.imgur.com/1v7T5Hm.png";
                case "ls_rainbow": return "https://i.imgur.com/BUYVOih.gif";
                case "hammer": return "https://i.imgur.com/hnY1HC0.png";
                case "hammer_green": return "https://i.imgur.com/iOGBcmQ.png";
                case "hammer_red": return "https://i.imgur.com/Z5QGsTO.png";
                case "hammer_pink": return "https://i.imgur.com/WHkXOoC.png";
                case "hammer_blue": return "https://i.imgur.com/fb7bxlw.png";
                case "hammer_navy": return "https://i.imgur.com/oaLfSIP.png";
                case "hammer_cyan": return "https://i.imgur.com/N2N0Uib.png";
                case "hammer_purple": return "https://i.imgur.com/CqNUgtg.png";
                case "hammer_orange": return "https://i.imgur.com/PzYNYZh.png";
                case "hammer_yellow": return "https://i.imgur.com/Ujpj7WL.png";
                case "hammer_grey": return "https://i.imgur.com/R0xCYk5.png";
                case "hammer_rainbow": return "https://i.imgur.com/GIAbKkO.gif";
                case "hh": return "https://i.imgur.com/EmjAq37.png";
                case "hh_green": return "https://i.imgur.com/LWCOXI4.png";
                case "hh_red": return "https://i.imgur.com/lwtBV09.png";
                case "hh_pink": return "https://i.imgur.com/tZBuDi2.png";
                case "hh_blue": return "https://i.imgur.com/7qncIzQ.png";
                case "hh_navy": return "https://i.imgur.com/yaFS4N0.png";
                case "hh_cyan": return "https://i.imgur.com/GvHKg1u.png";
                case "hh_purple": return "https://i.imgur.com/33FpZMA.png";
                case "hh_orange": return "https://i.imgur.com/5ZHbR8K.png";
                case "hh_yellow": return "https://i.imgur.com/2YdtoVI.png";
                case "hh_grey": return "https://i.imgur.com/pyPzmJI.png";
                case "hh_rainbow": return "https://i.imgur.com/VuRLWWG.gif";
                case "lance": return "https://i.imgur.com/M8fmT4f.png";
                case "lance_green": return "https://i.imgur.com/zSyyIZY.png";
                case "lance_red": return "https://i.imgur.com/ZFeN3aA.png";
                case "lance_pink": return "https://i.imgur.com/X1EncHA.png";
                case "lance_blue": return "https://i.imgur.com/qMM2gqG.png";
                case "lance_navy": return "https://i.imgur.com/F7vp82x.png";
                case "lance_cyan": return "https://i.imgur.com/9q1rqfF.png";
                case "lance_purple": return "https://i.imgur.com/qF9JMEE.png";
                case "lance_orange": return "https://i.imgur.com/s1Agqri.png";
                case "lance_yellow": return "https://i.imgur.com/EcOCe50.png";
                case "lance_grey": return "https://i.imgur.com/jKPcLtN.png";
                case "lance_rainbow": return "https://i.imgur.com/BXgEuDy.gif";
                case "gl": return "https://i.imgur.com/9wq3LQe.png";
                case "gl_green": return "https://i.imgur.com/bQdGiiB.png";
                case "gl_red": return "https://i.imgur.com/QorzUm5.png";
                case "gl_pink": return "https://i.imgur.com/OqkXeZy.png";
                case "gl_blue": return "https://i.imgur.com/lsFZSnT.png";
                case "gl_navy": return "https://i.imgur.com/2fkkHVd.png";
                case "gl_cyan": return "https://i.imgur.com/MzqTO9c.png";
                case "gl_purple": return "https://i.imgur.com/QqDN0jm.png";
                case "gl_orange": return "https://i.imgur.com/GowSPSA.png";
                case "gl_yellow": return "https://i.imgur.com/az9QfWH.png";
                case "gl_grey": return "https://i.imgur.com/Q5lK9Nw.png";
                case "gl_rainbow": return "https://i.imgur.com/47VsWHj.gif";
                case "saf": return "https://i.imgur.com/fVbaN34.png";
                case "saf_green": return "https://i.imgur.com/V3x8aaf.png";
                case "saf_red": return "https://i.imgur.com/3l8TO9T.png";
                case "saf_pink": return "https://i.imgur.com/DTXXEb9.png";
                case "saf_blue": return "https://i.imgur.com/Dgr9oQg.png";
                case "saf_navy": return "https://i.imgur.com/Tv40lQg.png";
                case "saf_cyan": return "https://i.imgur.com/uKxiYhr.png";
                case "saf_purple": return "https://i.imgur.com/x3RC716.png";
                case "saf_orange": return "https://i.imgur.com/GU2eOdb.png";
                case "saf_yellow": return "https://i.imgur.com/f0jrcYq.png";
                case "saf_grey": return "https://i.imgur.com/jIRe9fA.png";
                case "saf_rainbow": return "https://i.imgur.com/icBF5lS.gif";
                case "tonfa": return "https://i.imgur.com/8YpLQ5G.png";
                case "tonfa_green": return "https://i.imgur.com/0VflTRd.png";
                case "tonfa_red": return "https://i.imgur.com/f5mIJgU.png";
                case "tonfa_pink": return "https://i.imgur.com/M6ANARX.png";
                case "tonfa_blue": return "https://i.imgur.com/BrCnJbs.png";
                case "tonfa_navy": return "https://i.imgur.com/b2lbCN1.png";
                case "tonfa_cyan": return "https://i.imgur.com/7bm8xyW.png";
                case "tonfa_purple": return "https://i.imgur.com/BOcCFhU.png";
                case "tonfa_orange": return "https://i.imgur.com/vi8qGs5.png";
                case "tonfa_yellow": return "https://i.imgur.com/qDR1aJZ.png";
                case "tonfa_grey": return "https://i.imgur.com/GxFrQm6.png";
                case "tonfa_rainbow": return "https://i.imgur.com/2StcKCZ.gif";
                case "ms": return "https://i.imgur.com/s3OaNkP.png";
                case "ms_green": return "https://i.imgur.com/7c8pPow.png";
                case "ms_red": return "https://i.imgur.com/zA4wMON.png";
                case "ms_pink": return "https://i.imgur.com/dOc22Dm.png";
                case "ms_blue": return "https://i.imgur.com/rz4anE4.png";
                case "ms_navy": return "https://i.imgur.com/dvghN1a.png";
                case "ms_cyan": return "https://i.imgur.com/gCWBOWm.png";
                case "ms_purple": return "https://i.imgur.com/UI3KO1c.png";
                case "ms_orange": return "https://i.imgur.com/9Bg0QzE.png";
                case "ms_yellow": return "https://i.imgur.com/rAKEtTa.png";
                case "ms_grey": return "https://i.imgur.com/dNkcRIR.png";
                case "ms_rainbow": return "https://i.imgur.com/TyZFrvK.gif";
                case "lbg": return "https://i.imgur.com/txp2GsM.png";
                case "lbg_green": return "https://i.imgur.com/CMf9U6x.png";
                case "lbg_red": return "https://i.imgur.com/aKLv0na.png";
                case "lbg_pink": return "https://i.imgur.com/theTGWy.png";
                case "lbg_blue": return "https://i.imgur.com/IgXj7vl.png";
                case "lbg_navy": return "https://i.imgur.com/N8vleIL.png";
                case "lbg_cyan": return "https://i.imgur.com/4iF0Kex.png";
                case "lbg_purple": return "https://i.imgur.com/V36MwUh.png";
                case "lbg_orange": return "https://i.imgur.com/FZ3sAAr.png";
                case "lbg_yellow": return "https://i.imgur.com/l0Fga4q.png";
                case "lbg_grey": return "https://i.imgur.com/WE1oZuG.png";
                case "lbg_rainbow": return "https://i.imgur.com/Q0Firpd.gif";
                case "hbg": return "https://i.imgur.com/8WD2bI7.png";
                case "hbg_green": return "https://i.imgur.com/j6qe1uh.png";
                case "hbg_red": return "https://i.imgur.com/hd8cwCa.png";
                case "hbg_pink": return "https://i.imgur.com/PDfABOO.png";
                case "hbg_blue": return "https://i.imgur.com/qURblCM.png";
                case "hbg_navy": return "https://i.imgur.com/FxInecI.png";
                case "hbg_cyan": return "https://i.imgur.com/UclnhBS.png";
                case "hbg_purple": return "https://i.imgur.com/IHifTBB.png";
                case "hbg_orange": return "https://i.imgur.com/7JRHNzp.png";
                case "hbg_yellow": return "https://i.imgur.com/rihlgaB.png";
                case "hbg_grey": return "https://i.imgur.com/mKpJc0p.png";
                case "hbg_rainbow": return "https://i.imgur.com/TgPORx6.gif";
                case "bow": return "https://i.imgur.com/haCsXQr.png";
                case "bow_green": return "https://i.imgur.com/vykrGg9.png";
                case "bow_red": return "https://i.imgur.com/01nEtNy.png";
                case "bow_pink": return "https://i.imgur.com/DLIYT8G.png";
                case "bow_blue": return "https://i.imgur.com/THX3O3X.png";
                case "bow_navy": return "https://i.imgur.com/DGHifcq.png";
                case "bow_cyan": return "https://i.imgur.com/sXnzQrG.png";
                case "bow_purple": return "https://i.imgur.com/D6NYg8r.png";
                case "bow_orange": return "https://i.imgur.com/fy47m6l.png";
                case "bow_yellow": return "https://i.imgur.com/ExGTxvl.png";
                case "bow_grey": return "https://i.imgur.com/Y5vOofE.png";
                case "bow_rainbow": return "https://i.imgur.com/rsEycVk.gif";
            }
        }

        public string GetRealMonsterName(string iconName)
        {
            string RealMonsterName = iconName.Replace("https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/main/img/monster/","");
            RealMonsterName = RealMonsterName.Replace(".gif", "");
            RealMonsterName = RealMonsterName.Replace(".png", "");
            RealMonsterName = RealMonsterName.Replace("zenith_", "");
            RealMonsterName = RealMonsterName.Replace("_", " ");
            
            //https://stackoverflow.com/questions/4315564/capitalizing-words-in-a-string-using-c-sharp
            RealMonsterName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(RealMonsterName);
            
            return RealMonsterName;
        }

        public string GetRankNameFromID(int id)
        {
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
                    return "Low Rank";
                case 11:
                    return "Low/High Rank";
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    return "High Rank";
                case 26:
                case 31:
                case 42:
                    return "HR5";
                case 32:
                case 46:
                    if (GetRealMonsterName(DataLoader.model.CurrentMonster1Icon).Contains("Supremacy"))
                    {
                        return "";
                    } else
                    {
                        return "Supremacy";
                    }
                case 53://TODO: conquest levels via quest id
                    return "G Rank";
                case 54://TODO
                    return ""; //20m lower shiten/musou repel/musou lesser slay
                case 55:
                    return ""; //10m upper shiten/musou true slay
                case 64:
                    return "Z1";
                case 65:
                    return "Z2";
                case 66:
                    return "Z3";
                case 67:
                    return "Z4";
                case 71:
                case 72:
                case 73:
                    return "Interception";
                default:
                    return "";
            }
        }

        public string GetObjectiveNameFromID(int id)
        {
            switch (id)
            {
                case 0:
                    return "Nothing";
                case 1:
                    return "Hunt";
                case 257:
                    return "Capture";
                case 513:
                    return "Slay";
                case 32772:
                    return "Repel";
                case 98308:
                    return "Slay or Repel";
                case 262144:
                    return "Slay All";
                case 131072:
                    return "Slay Total";
                case 2:
                    return "Deliver";
                case 16388:
                    return "Break Part";
                case 4098:
                    return "Deliver Flag";
                case 16:
                    return "Esoteric Action";
                default:
                    return "Nothing";
            }
        }

        public string getAreaIconFromID(int id)
        {
            switch (id) 
            {
                case 0://Loading
                    return "https://i.imgur.com/Dwdfpen.png";
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

        public string getGameMode(bool isHighGradeEdition)
        {
            if (isHighGradeEdition)
                return " [High-Grade Edition]";
            else
                return "";
        }

        public string getOverlayMode()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableDamageNumbers || s.HealthBarsShown || s.EnableSharpness || s.PartThresholdShown || s.HitCountShown || s.PlayerAtkShown || s.MonsterAtkMultShown || s.MonsterDefrateShown || s.MonsterSizeShown || s.MonsterPoisonShown || s.MonsterParaShown || s.MonsterSleepShown || s.MonsterBlastShown || s.MonsterStunShown)
                return "";
            else if (s.TimerInfoShown)
                return "(Speedrun) ";
            else
                return "(Zen) ";
        }

        private void UpdateDiscordRPC()
        {
            if (!(isDiscordRPCRunning))
            {
                return;
            }

            presenceTemplate.Details = string.Format("{0}{1}{2}",getOverlayMode(),getAreaName(DataLoader.model.AreaID()),getGameMode(DataLoader.isHighGradeEdition));

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

            //DataLoader.model.DisplayMonsterEHP(float.Parse(Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat)

            //Info
            if (DataLoader.model.QuestID() != 0 && DataLoader.model.TimeDefInt() > DataLoader.model.TimeInt() && int.Parse(DataLoader.model.ATK) > 0)
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
                        presenceTemplate.State = String.Format("Floor: {0} ({1}/{2} Max/Total) | RP: {3} | White Fatalis: {4}/{5} (Slain/Encounters)",DataLoader.model.RoadFloor()+1,DataLoader.model.RoadMaxStagesMultiplayer(),DataLoader.model.RoadTotalStagesMultiplayer(),DataLoader.model.RoadPoints(),DataLoader.model.RoadFatalisSlain(),DataLoader.model.RoadFatalisEncounters());
                        break;
                    case 23628://solo road
                        presenceTemplate.State = String.Format("Floor: {0} ({1}/{2} Max/Total) | RP: {3} | White Fatalis: {4}/{5} (Slain/Encounters)", DataLoader.model.RoadFloor()+1, DataLoader.model.RoadMaxStagesSolo(), DataLoader.model.RoadTotalStagesSolo(),DataLoader.model.RoadPoints(),DataLoader.model.RoadFatalisSlain(), DataLoader.model.RoadFatalisEncounters());
                        break;
                    case 21731://1st district dure
                    case 21749://sky corridor version
                        presenceTemplate.State = String.Format("Slay 1st District Duremudira | Slain: {0} | Encounters: {1}",DataLoader.model.FirstDistrictDuremudiraSlays(),DataLoader.model.FirstDistrictDuremudiraEncounters());
                        break;
                    case 21746://2nd district dure
                    case 21750://sky corridor version
                        presenceTemplate.State = String.Format("Slay 1st District Duremudira | Slain: {0} | Encounters: {1}", DataLoader.model.SecondDistrictDuremudiraSlays(), DataLoader.model.SecondDistrictDuremudiraEncounters());
                        break;
                    default:
                        presenceTemplate.State = String.Format("{0} {1} {2} | True Raw: {3} (Max {4}) | Hits: {5}",GetObjectiveNameFromID(DataLoader.model.ObjectiveType()),GetRankNameFromID(DataLoader.model.RankBand()), GetRealMonsterName(DataLoader.model.CurrentMonster1Icon),DataLoader.model.ATK,DataLoader.model.HighestAtk,DataLoader.model.HitCount);
                        break;
                }

                //presenceTemplate.Assets.LargeImageKey = monsterNameKey;

            }
            else if (DataLoader.model.QuestID() == 0)
            {
                if (!(itemsLoaded))
                { 
                    //load item list
                    Dictionary.Items.initiate();
                    itemsLoaded = true;
                }
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
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), getItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 173:// My House (original)
                    case 175://My House (MAX)
                        presenceTemplate.State = string.Format("GR: {0} | Partner Lv: {1} | Armor Color: {2} | GCP: {3}", DataLoader.model.GRankNumber(), DataLoader.model.PartnerLevel(), getArmorColor(), DataLoader.model.GCP());
                        break;

                    case 202://Guild Halls
                    case 203:
                    case 204:
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), getItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 205://Pugi Farm
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), getItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 256://Caravan Areas
                    case 260:
                    case 261:
                    case 262:
                    case 263:
                        presenceTemplate.State = string.Format("CP: {0} | Gg: {1} | g: {2} | Gem Lv: {3}", DataLoader.model.CaravanPoints(), DataLoader.model.RaviGg(), DataLoader.model.Ravig(), DataLoader.model.CaravenGemLevel()+1);
                        break;

                    case 257://Blacksmith
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | GZenny: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), DataLoader.model.GZenny());
                        break;

                    case 264://Gallery
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Score: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), DataLoader.model.GalleryEvaluationScore());
                        break;

                    case 265://Guuku Farm
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), getItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 283://Halk Area TODO partnya lv
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | PNRP: {2} | Halk Fullness: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), DataLoader.model.PartnyaRankPoints(), DataLoader.model.HalkFullness());
                        break;

                    case 286://PvP Room
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), getItemName(DataLoader.model.PoogieItemUseID()));
                        break;

                    case 379://Diva Hall
                    case 445:
                        presenceTemplate.State = string.Format("GR: {0} | Diva Bond: {1} | Items Given: {2} | Diva Skills Left: {3}", DataLoader.model.GRankNumber(), DataLoader.model.DivaBond(), DataLoader.model.DivaItemsGiven(), DataLoader.model.DivaSkillUsesLeft());
                        break;

                    // case 458:// "Hunter's Road 1 Area 1" },
                    //case 459:// "Hunter's Road 2 Base Camp" },

                    case 462://MezFez Entrance
                    case 463: //Volpkun Together
                    case 465://MezFez Minigame
                        presenceTemplate.State = string.Format("GR: {0} | MezFes Points: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.MezeportaFestivalPoints(), getArmorSkill(DataLoader.model.GuildFoodSkill()), getItemName(DataLoader.model.PoogieItemUseID()));
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
                        presenceTemplate.State = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), getItemName(DataLoader.model.PoogieItemUseID()));
                        break;
                }

                presenceTemplate.Assets.LargeImageKey = getAreaIconFromID(DataLoader.model.AreaID());
                presenceTemplate.Assets.LargeImageText = getAreaName(DataLoader.model.AreaID());
            }

            //Timer
            if (DataLoader.model.QuestID() != 0 && !inQuest && DataLoader.model.TimeDefInt() > DataLoader.model.TimeInt() && int.Parse(DataLoader.model.ATK) > 0)
            {
                inQuest = true; 


                switch (GetDiscordTimerMode())
                {
                    case "Time Left":
                        presenceTemplate.Timestamps = Timestamps.FromTimeSpan((double)DataLoader.model.TimeDefInt() / 30.0);
                        break;
                    case "Time Elapsed":
                        presenceTemplate.Timestamps = Timestamps.Now;
                        break;
                    default:
                        presenceTemplate.Timestamps = Timestamps.FromTimeSpan((double)DataLoader.model.TimeDefInt() / 30.0);
                        break;
                }
            } 
            else if (DataLoader.model.QuestID() == 0 && inQuest && DataLoader.model.TimeDefInt() > DataLoader.model.TimeInt() && int.Parse(DataLoader.model.ATK) == 0)
            { 
                inQuest = false;
                presenceTemplate.Timestamps = Timestamps.Now; 
            }

            //SmallInfo
            presenceTemplate.Assets.SmallImageKey = getWeaponIconFromID(DataLoader.model.WeaponType());

            if (GetHunterName != "" && GetGuildName != "")
            {
                presenceTemplate.Assets.SmallImageText = String.Format("{0} | {1} | GSR: {2} | {3} Style", GetHunterName, GetGuildName, DataLoader.model.GSR(), GetWeaponStyleFromID(DataLoader.model.WeaponStyle()));
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
                case "MonsterHpBars":
                    s.HealthBarsX = (double)(pos.X - XOffset);
                    s.HealthBarsY = (double)(pos.Y - YOffset);
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
/// ## Look at hp bar to make better
/// ## damage numbers
/// ## look at other popular overlays and steal their design
/// fix road stuff
/// ## implement for monsters 1-4
/// select monster 
/// ## configuration
/// move data translation into dataloader out of the abstract address model
/// ## remove unnecessary fields in DataLoader
/// figure out way to make it work for all monsters with the same functions (use list u dunmbass)
/// ## figure out a way to make templates
/// body parts
/// ## status panel
/// ## maybe more ....
/// Tooltips for Configuration
/// Configuration for Damage numbers
/// </TODO>
