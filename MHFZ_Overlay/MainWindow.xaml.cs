using Dictionary;
using DiscordRPC;
using Gma.System.MouseKeyHook;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Memory;
using MHFZ_Overlay.UI.Class;
using Microsoft.Extensions.DependencyModel;
using NLog;
using Octokit;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using XInput.Wrapper;
using Application = System.Windows.Application;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using Image = System.Windows.Controls.Image;
using Label = System.Windows.Controls.Label;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace MHFZ_Overlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataLoader DataLoader { get; set; }

        #region system tray

        private NotifyIcon _notifyIcon;

        private void CreateSystemTrayIcon()
        {
            _notifyIcon = new NotifyIcon();
            var iconPath = "UI\\Icons\\ico\\mhfzoverlayicon256.ico";
            var iconStream = Application.GetResourceStream(new Uri(iconPath, UriKind.Relative)).Stream;
            var icon = new System.Drawing.Icon(iconStream);
            _notifyIcon.Icon = icon;
            _notifyIcon.Text = "MHF-Z Overlay";
            _notifyIcon.Visible = true;

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Open Configuration", null, Option1_Click);
            contextMenu.Items.Add("Restart Application", null, Option2_Click);
            contextMenu.Items.Add("Close Application", null, Option3_Click);

            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void Option1_Click(object sender, EventArgs e)
        {
            OpenConfigButton_Key();
        }

        private void Option2_Click(object sender, EventArgs e)
        {
            ReloadButton_Key();
        }

        private void Option3_Click(object sender, EventArgs e)
        {
            CloseButton_Key();
        }

        #endregion

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

        /// <summary>
        /// The current program version
        /// </summary>
        public const string CurrentProgramVersion = "v0.22.0";

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
        private DiscordRpcClient Client;

        //Called when your application first starts.
        //For example, just before your main loop, on OnEnable for unity.
        void SetupDiscordRPC()
        {
            /*
            Create a Discord client
            NOTE: 	If you are using Unity3D, you must use the full constructor and define
                     the pipe connection.
            */
            Client = new DiscordRpcClient(GetDiscordClientID);

            //Set the logger

            //Subscribe to events

            //Connect to the RPC
            Client.Initialize();

            //Set the rich presence
            //Call this as many times as you want and anywhere in your code.

        }

        /// <summary>
        /// Gets a value indicating whether [show discord RPC].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show discord RPC]; otherwise, <c>false</c>.
        /// </value>
        public static bool ShowDiscordRPC
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                if (s.EnableRichPresence)
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
        public static string GetDiscordClientID
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
        public static string GetDiscordServerInvite
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
        public static string GetHunterName
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
        public static string GetGuildName
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

        public static string GetServerName
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                bool serverNameFound = Dictionary.DiscordServersList.DiscordServerID.TryGetValue(s.DiscordServerID, out string? value);
                if (serverNameFound)
                    return value;
                else
                    return "Unknown Server";
            }
        }

        //Dispose client        
        /// <summary>
        /// Cleanups the Discord RPC instance.
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
        private RichPresence presenceTemplate = new RichPresence()
        {
            Details = "【MHF-Z】Overlay " + CurrentProgramVersion,
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
                    new DiscordRPC.Button() {Label = "【MHF-Z】Overlay "+CurrentProgramVersion, Url = "https://github.com/DorielRivalet/mhfz-overlay"},
                    new DiscordRPC.Button() { Label = "Discord RPC C# Dev Site", Url = "https://lachee.dev/" }
                }
        };

        /// <summary>
        /// Is the main loop currently running?
        /// </summary>
        private bool isDiscordRPCRunning = false;

        /// <summary>
        /// Initializes the discord RPC.
        /// </summary>
        private void InitializeDiscordRPC()
        {
            if (isDiscordRPCRunning)
                return;

            if (ShowDiscordRPC && GetDiscordClientID != "")
            {
                SetupDiscordRPC();

                //Set Presence
                presenceTemplate.Timestamps = Timestamps.Now;

                if (GetHunterName != "" && GetGuildName != "" && GetServerName != "")
                {
                    presenceTemplate.Assets = new Assets()
                    {
                        LargeImageKey = "cattleya",
                        LargeImageText = "Please Wait",
                        SmallImageKey = "https://i.imgur.com/9OkLYAz.png",
                        SmallImageText = GetHunterName + " | " + GetGuildName + " | " + GetServerName
                    };
                }

                //should work fine
                presenceTemplate.Buttons = new DiscordRPC.Button[] { };
                presenceTemplate.Buttons = new DiscordRPC.Button[]
                {
                    new DiscordRPC.Button() {Label = "【MHF-Z】Overlay "+CurrentProgramVersion, Url = "https://github.com/DorielRivalet/mhfz-overlay"},
                    new DiscordRPC.Button() { Label = "Discord RPC C# Dev Site", Url = "https://lachee.dev/" }
                };

                if (GetDiscordServerInvite != "")
                {
                    presenceTemplate.Buttons = new DiscordRPC.Button[] { };
                    presenceTemplate.Buttons = new DiscordRPC.Button[]
                    {
                    new DiscordRPC.Button() {Label = "【MHF-Z】Overlay "+CurrentProgramVersion, Url = "https://github.com/DorielRivalet/mhfz-overlay"},
                    new DiscordRPC.Button() { Label = "Join Discord Server", Url = String.Format("https://discord.com/invite/{0}",GetDiscordServerInvite)}
                    };
                }

                Client.SetPresence(presenceTemplate);
                isDiscordRPCRunning = true;
            }
        }

        #endregion

        public DateTime ProgramStart;
        public static DateTime ProgramStartStatic;
        public DateTime ProgramEnd;


        #region main

        // Declare a dictionary to map keys to images
        private readonly Dictionary<Keys, Image> _keyImages = new Dictionary<Keys, Image>();

        private readonly Dictionary<MouseButtons, Image> _mouseImages = new Dictionary<MouseButtons, Image>();

        private readonly Dictionary<X.Gamepad.GamepadButtons, Image> _controllerImages = new Dictionary<X.Gamepad.GamepadButtons, Image>();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //Main entry point?        
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable property 'Client' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable field 'latestRelease' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'releaseInfo' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        public MainWindow()
#pragma warning restore CS8618 // Non-nullable field 'releaseInfo' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning restore CS8618 // Non-nullable field 'latestRelease' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning restore CS8618 // Non-nullable property 'Client' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        {
            var splashScreen = new SplashScreen("UI/Icons/png/loading.png");

            splashScreen.Show(false);
            DataLoader = new DataLoader();
            InitializeComponent();

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "logs.log" };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            logger.Info($"PROGRAM OPERATION: MainWindow initialized");

            Left = 0;
            Top = 0;
            Topmost = true;
            DispatcherTimer timer = new();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 30);
            //memory leak?
            timer.Tick += Timer_Tick;
            timer.Start();
            DataContext = DataLoader.model;
            GlobalHotKey.RegisterHotKey("Shift + F1", () => OpenConfigButton_Key());
            GlobalHotKey.RegisterHotKey("Shift + F5", () => ReloadButton_Key());
            GlobalHotKey.RegisterHotKey("Shift + F6", () => CloseButton_Key());
            OpenConfigButton.Visibility = Visibility.Hidden;
            ReloadButton.Visibility = Visibility.Hidden;
            CloseButton.Visibility = Visibility.Hidden;

            InitializeDiscordRPC();
            CheckGameState();
            _ = LoadOctoKit();

            LiveCharts.Configure(config =>
            config
                // registers SkiaSharp as the library backend
                // REQUIRED unless you build your own
                .AddSkiaSharp()

                // adds the default supported types
                // OPTIONAL but highly recommend
                .AddDefaultMappers()

                // select a theme, default is Light
                // OPTIONAL
                .AddLightTheme());


            // When the program starts
            ProgramStart = DateTime.Now;
            ProgramStartStatic = DateTime.Now;

            // Calculate the total time spent and update the TotalTimeSpent property
            DataLoader.model.TotalTimeSpent = databaseManager.CalculateTotalTimeSpent();

            MapPlayerInputImages();

            // TODO controller
            Subscribe();

            SetGraphSeries();

            // Get the dependency context for the current application
            var context = DependencyContext.Default;

            // Build a string with information about all the dependencies
            var sb = new StringBuilder();
            var runtimeTarget = RuntimeInformation.FrameworkDescription;
            sb.AppendLine($"Target framework: {runtimeTarget}");
            foreach (var lib in context.RuntimeLibraries)
            {
                sb.AppendLine($"Library: {lib.Name} {lib.Version}");
                sb.AppendLine($"  Type: {lib.Type}");
                sb.AppendLine($"  Hash: {lib.Hash}");
                sb.AppendLine($"  Dependencies:");
                foreach (var dep in lib.Dependencies)
                {
                    sb.AppendLine($"    {dep.Name} {dep.Version}");
                }
            }

            string dependenciesInfo = sb.ToString();

            logger.Info("PROGRAM OPERATION: Loading dependency data\n{0}", dependenciesInfo);

            // The rendering tier corresponds to the high-order word of the Tier property.
            int renderingTier = (RenderCapability.Tier >> 16);

            logger.Info("PROGRAM OPERATION: Found rendering tier {0}", renderingTier);

            DataLoader.model.ShowSaveIcon = false;

            CreateSystemTrayIcon();

            splashScreen.Close(TimeSpan.FromSeconds(0.1));
        }

        /// <summary>
        /// Sets the graph series for player stats.
        /// </summary>
        private void SetGraphSeries()
        {
            //TODO graphs
            //https://stackoverflow.com/questions/74719777/livecharts2-binding-continuously-changing-data-to-graph
            //inspired by HunterPie
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            DataLoader.model.attackBuffSeries.Add(new LineSeries<ObservablePoint>
            {
                Values = DataLoader.model.attackBuffCollection,
                LineSmoothness = .5,
                GeometrySize = 0,
                Stroke = new SolidColorPaint(new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerAttackGraphColor))) { StrokeThickness = 2 },
                Fill = new LinearGradientPaint(new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerAttackGraphColor, "7f")), new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerAttackGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
            });

            DataLoader.model.damagePerSecondSeries.Add(new LineSeries<ObservablePoint>
            {
                Values = DataLoader.model.damagePerSecondCollection,
                LineSmoothness = .5,
                GeometrySize = 0,
                Stroke = new SolidColorPaint(new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerDPSGraphColor))) { StrokeThickness = 2 },
                Fill = new LinearGradientPaint(new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerDPSGraphColor, "7f")), new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerDPSGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
            });

            DataLoader.model.actionsPerMinuteSeries.Add(new LineSeries<ObservablePoint>
            {
                Values = DataLoader.model.actionsPerMinuteCollection,
                LineSmoothness = .5,
                GeometrySize = 0,
                Stroke = new SolidColorPaint(new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerAPMGraphColor))) { StrokeThickness = 2 },
                Fill = new LinearGradientPaint(new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerAPMGraphColor, "7f")), new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerAPMGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
            });

            DataLoader.model.hitsPerSecondSeries.Add(new LineSeries<ObservablePoint>
            {
                Values = DataLoader.model.hitsPerSecondCollection,
                LineSmoothness = .5,
                GeometrySize = 0,
                Stroke = new SolidColorPaint(new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
                Fill = new LinearGradientPaint(new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(DataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
            });
        }

        GitHubClient ghClient = new GitHubClient(new ProductHeaderValue("MHFZ_Overlay"));

        /// <summary>
        /// Loads the github api integration.
        /// </summary>
        private async Task LoadOctoKit()
        {
            var releases = await ghClient.Repository.Release.GetAll("DorielRivalet", "MHFZ_Overlay");
            var latest = releases[0];
            var latestRelease = latest.TagName;
            if (latestRelease != MainWindow.CurrentProgramVersion)
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                if (s.EnableUpdateNotifier)
                {
                    System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(String.Format("Detected different version ({0}) from latest ({1}). Do you want to download the file?", CurrentProgramVersion, latest.TagName), "【MHF-Z】Overlay Update Available", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Asterisk, MessageBoxResult.No);
                    logger.Info("PROGRAM OPERATION: Detected different overlay version");

                    if (messageBoxResult.ToString() == "Yes")
                    {
                        OpenLink("https://github.com/DorielRivalet/mhfz-overlay/releases/latest/download/Releases.7z");
                    }
                }
            }
        }

        /// <summary>
        /// Opens the link. https://stackoverflow.com/a/60221582/18859245
        /// </summary>
        /// <param name="destinationurl">The destinationurl.</param>
        private static void OpenLink(string destinationurl)
        {
            var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }

        readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();

        /// <summary>
        /// Checks the state of the game.
        /// </summary>
        public void CheckGameState()
        {
            int PID = m.GetProcIdFromName("mhf");

            //https://stackoverflow.com/questions/12372534/how-to-get-a-process-window-class-name-from-c
            int pidToSearch = PID;
            //Init a condition indicating that you want to search by process id.
            var condition = new PropertyCondition(AutomationElementIdentifiers.ProcessIdProperty,
                pidToSearch);
            //Find the automation element matching the criteria
            AutomationElement element = AutomationElement.RootElement.FindFirst(
                TreeScope.Children, condition);

            //get the classname
            if (element != null)
            {
                var className = element.Current.ClassName;

                if (className == "MHFLAUNCH")
                {
                    System.Windows.MessageBox.Show("Detected launcher, please restart overlay when fully loading into Mezeporta.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    logger.Info("PROGRAM OPERATION: Detected game launcher");

                    DataLoader.model.isInLauncherBool = true;
                }
                else
                {
                    DataLoader.model.isInLauncherBool = false;
                }

                //https://stackoverflow.com/questions/51148/how-do-i-find-out-if-a-process-is-already-running-using-c
                //https://stackoverflow.com/questions/12273825/c-sharp-process-start-how-do-i-know-if-the-process-ended
                Process mhfProcess = Process.GetProcessById(pidToSearch);

                mhfProcess.EnableRaisingEvents = true;
                mhfProcess.Exited += (sender, e) =>
                {

                    DataLoader.model.closedGame = true;
                    Settings s = (Settings)Application.Current.TryFindResource("Settings");

                    if (s.EnableAutoClose)
                    {
                        System.Windows.MessageBox.Show("Detected closed game, closing overlay. Please restart overlay when fully loading into Mezeporta.", "Warning", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        logger.Info("PROGRAM OPERATION: Detected closed game");

                        //https://stackoverflow.com/a/9050477/18859245
                        Cleanup();
                        databaseManager.StoreSessionTime(this);
                        Environment.Exit(0);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Detected closed game, please restart overlay when fully loading into Mezeporta.", "Warning", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        logger.Info("PROGRAM OPERATION: Detected closed game");

                    }
                };
            }
        }

        int counter = 0;
        int prevTime = 0;

        private bool showedNullError = false;
        private bool showedGameFolderWarning = false;

        // TODO: optimization
        public void Timer_Tick(object? obj, EventArgs e)
        {
            try
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

                CheckQuestStateForDatabaseLogging();

                // TODO should this be here or somewhere else?
                // this is also for database logging
                CheckMezFesScore();

                if (DataLoader.model.isInLauncher() == "NULL" && !showedNullError)
                {
                    showedNullError = true;
                }

                if (!showedGameFolderWarning)
                {
                    DataLoader.model.ValidateGameFolder();
                    showedGameFolderWarning = true;
                }

                DataLoader.CheckForExternalProcesses();

                CheckIfLocationChanged();
                CheckIfQuestChanged();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"PROGRAM OPERATION: An error has occurred in the Timer_Tick function");
                WriteCrashLog(ex);
                // the flushing is done automatically according to the docs
            }
        }
        private void CheckIfQuestChanged()
        {
            if (DataLoader.model.previousQuestID != DataLoader.model.QuestID() && DataLoader.model.QuestID() != 0)
            {
                DataLoader.model.previousQuestID = DataLoader.model.QuestID();
                ShowQuestName();
            }
            else if (DataLoader.model.QuestID() == 0 && DataLoader.model.previousQuestID != 0)
            {
                DataLoader.model.previousQuestID = DataLoader.model.QuestID();
            }
        }

        private void ShowQuestName()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            if (s == null || !s.QuestNameShown)
                return;

            Dictionary.Quests.QuestIDs.TryGetValue(DataLoader.model.previousQuestID, out string? previousQuestID);
            questNameTextBlock.Text = previousQuestID;
            Brush blackBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
            Brush peachBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xB3, 0x87));
            AnimateOutlinedTextBlock(questNameTextBlock, blackBrush, peachBrush);
        }

        /// <summary>
        /// Animates the outlined text block.
        /// </summary>
        /// <param name="outlinedTextBlock">The outlined text block.</param>
        private void AnimateOutlinedTextBlock(OutlinedTextBlock outlinedTextBlock, Brush startBrush, Brush endBrush)
        {
            // Define the animation durations and colors
            var fadeInDuration = TimeSpan.FromSeconds(1);
            var fadeOutDuration = TimeSpan.FromSeconds(1);

            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, fadeInDuration);
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, fadeOutDuration);
            BrushAnimation colorInAnimation = new BrushAnimation
            {
                From = startBrush,
                To = endBrush,
                Duration = fadeInDuration,
            };
            BrushAnimation colorOutAnimation = new BrushAnimation
            {
                From = endBrush,
                To = startBrush,
                Duration = fadeOutDuration,
            };

            Storyboard fadeInStoryboard = new Storyboard();
            Storyboard.SetTarget(fadeIn, outlinedTextBlock);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(TextBlock.OpacityProperty));
            Storyboard.SetTarget(colorInAnimation, outlinedTextBlock);
            Storyboard.SetTargetProperty(colorInAnimation, new PropertyPath(OutlinedTextBlock.FillProperty));
            fadeInStoryboard.Children.Add(fadeIn);
            fadeInStoryboard.Children.Add(colorInAnimation);

            Storyboard fadeOutStoryboard = new Storyboard();
            Storyboard.SetTarget(fadeOut, outlinedTextBlock);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath(TextBlock.OpacityProperty));
            Storyboard.SetTarget(colorOutAnimation, outlinedTextBlock);
            Storyboard.SetTargetProperty(colorOutAnimation, new PropertyPath(OutlinedTextBlock.FillProperty));
            fadeOutStoryboard.Children.Add(fadeOut);
            fadeOutStoryboard.Children.Add(colorOutAnimation);

            fadeInStoryboard.Completed += (sender, e) =>
            {
                // Wait for 2 seconds before starting fade-out animation
                fadeOutStoryboard.BeginTime = TimeSpan.FromSeconds(2);
                fadeOutStoryboard.Begin();
            };

            // Start the fade-in storyboard
            fadeInStoryboard.Begin();
        }

        private void CheckIfLocationChanged()
        {
            if (DataLoader.model.previousGlobalAreaID != DataLoader.model.AreaID() && DataLoader.model.AreaID() != 0)
            {
                DataLoader.model.previousGlobalAreaID = DataLoader.model.AreaID();
                ShowLocationName();
            }
        }

        private void ShowLocationName()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            if (s == null || !s.LocationTextShown)
                return;

            Dictionary.MapAreaList.MapAreaID.TryGetValue(DataLoader.model.previousGlobalAreaID, out string? previousGlobalAreaID);
            locationTextBlock.Text = previousGlobalAreaID;
            Brush blackBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
            Brush blueBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x89, 0xB4, 0xFA));
            AnimateOutlinedTextBlock(locationTextBlock, blackBrush, blueBrush);
        }

        private void WriteCrashLog(Exception ex)
        {
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"MHFZ_Overlay\\MHFZ_Overlay-CrashLog-{dateTime}.txt");

            // Log the exception to a file
            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                sw.WriteLine("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine("Message: " + ex.Message);
                sw.WriteLine("Stack Trace: " + ex.StackTrace);
            }

            System.Windows.MessageBox.Show("Fatal error, closing overlay. See the crash log in the overlay folder for more information.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            logger.Fatal(ex, "PROGRAM OPERATION: Program crashed");

            //https://stackoverflow.com/a/9050477/18859245
            Cleanup();
            databaseManager.StoreSessionTime(this);
            Environment.Exit(0);
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
                    if (!DataLoader.model.damageDealtDictionary.ContainsKey(DataLoader.model.TimeInt()))
                    {
                        try
                        {
                            DataLoader.model.damageDealtDictionary.Add(DataLoader.model.TimeInt(), damage);
                        }
                        catch (Exception ex)
                        {
                            logger.Warn(ex, "PROGRAM OPERATION: Could not insert into damageDealtDictionary");
                        }
                    }
                }
                else if (curNum < 0)
                {
                    // TODO
                    curNum += 1000;
                    CreateDamageNumberLabel(curNum);
                    if (!DataLoader.model.damageDealtDictionary.ContainsKey(DataLoader.model.TimeInt()))
                    {
                        try
                        {
                            DataLoader.model.damageDealtDictionary.Add(DataLoader.model.TimeInt(), curNum);

                        }
                        catch (Exception ex)
                        {
                            logger.Warn(ex, "PROGRAM OPERATION: Could not insert into damageDealtDictionary");
                        }
                    }
                }
                else
                {
                    if (curNum != damage)
                    {
                        CreateDamageNumberLabel(curNum);
                        if (!DataLoader.model.damageDealtDictionary.ContainsKey(DataLoader.model.TimeInt()))
                        {
                            try
                            {
                                DataLoader.model.damageDealtDictionary.Add(DataLoader.model.TimeInt(), curNum);
                            }
                            catch
                            (Exception ex)
                            {
                                logger.Warn(ex, "PROGRAM OPERATION: Could not insert into damageDealtDictionary");
                            }
                        }
                    }
                }
            }
            prevNum = damage;
        }

        /// <summary>
        /// Shows multicolor damage numbers?
        /// </summary>
        /// <returns></returns>
        public static bool ShowDamageNumbersMulticolor()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableDamageNumbersMulticolor)
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
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            Random random = new();
            double x = random.Next(450);
            double y = random.Next(254);
            Point newPoint = DamageNumbers.TranslatePoint(new Point(x, y), DamageNumbers);

            // Create a new instance of the OutlinedTextBlock class.
            OutlinedTextBlock damageOutlinedTextBlock = new OutlinedTextBlock();

            // Set the properties of the OutlinedTextBlock instance.
            damageOutlinedTextBlock.Text = damage.ToString();
            damageOutlinedTextBlock.FontFamily = new System.Windows.Media.FontFamily(s.DamageNumbersFontFamily);
            damageOutlinedTextBlock.FontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(s.DamageNumbersFontWeight);
            damageOutlinedTextBlock.FontSize = 21;
            damageOutlinedTextBlock.StrokeThickness = 4;
            damageOutlinedTextBlock.Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));

            //does not alter actual number displayed, only the text style
            double damageModifier = damage / (DataLoader.model.CurrentWeaponMultiplier / 2);

            switch (damageModifier)
            {
                case < 15.0:
                    damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xB4, 0xBE, 0xFE));
                    damageOutlinedTextBlock.FontSize = 22;
                    break;
                case < 35.0:
                    damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x89, 0xB4, 0xFA));
                    damageOutlinedTextBlock.FontSize = 22;
                    break;
                case < 75.0:
                    damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x74, 0xC7, 0xEC));
                    damageOutlinedTextBlock.FontSize = 22;
                    break;
                case < 200.0:
                    damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x89, 0xDC, 0xEB));
                    damageOutlinedTextBlock.FontSize = 22;
                    break;
                case < 250.0:
                    damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x94, 0xE2, 0xD5));
                    damageOutlinedTextBlock.FontSize = 24;
                    break;
                case < 300.0:
                    damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xF9, 0xE2, 0xAF));
                    damageOutlinedTextBlock.FontSize = 24;
                    break;
                case < 350.0:
                    damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xB3, 0x97));
                    damageOutlinedTextBlock.FontSize = 24;
                    damageOutlinedTextBlock.Text += "!";
                    break;
                case < 500.0:
                    damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xEB, 0xA0, 0xAC));
                    damageOutlinedTextBlock.FontSize = 26;
                    damageOutlinedTextBlock.Text += "!!";
                    break;
                default:
                    damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xF3, 0x8B, 0xA8));
                    damageOutlinedTextBlock.FontSize = 30;
                    damageOutlinedTextBlock.Text += "!!!";
                    break;
            }

            // TODO add check for effects
            if (!ShowDamageNumbersMulticolor())
            {
                //https://stackoverflow.com/questions/14601759/convert-color-to-byte-value
                System.Drawing.Color color = ColorTranslator.FromHtml(s.DamageNumbersColor);
                damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }

            damageOutlinedTextBlock.SetValue(Canvas.TopProperty, newPoint.Y);
            damageOutlinedTextBlock.SetValue(Canvas.LeftProperty, newPoint.X);

            DamageNumbers.Children.Add(damageOutlinedTextBlock);

            if (!s.EnableDamageNumbersFlash && !s.EnableDamageNumbersSize)
            {
                RemoveDamageNumberLabel(damageOutlinedTextBlock);

            }
            else
            {
                Brush blackBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
                Brush whiteBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xCD, 0xD6, 0xF4));
                Brush redBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xF3, 0x8B, 0xA8));
                Brush originalFillBrush = damageOutlinedTextBlock.Fill;

                // TODO: Animation, inspired by rise

                // Create a Storyboard to animate the label's size, color and opacity
                Storyboard fadeInIncreaseSizeFlashColorStoryboard = new Storyboard();

                // Create a DoubleAnimation to animate the label's width
                DoubleAnimation sizeIncreaseAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = damageOutlinedTextBlock.FontSize * 1.5,
                    Duration = TimeSpan.FromSeconds(.3),
                };
                Storyboard.SetTarget(sizeIncreaseAnimation, damageOutlinedTextBlock);
                Storyboard.SetTargetProperty(sizeIncreaseAnimation, new PropertyPath(OutlinedTextBlock.FontSizeProperty));

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(.3),
                };
                Storyboard.SetTarget(fadeInAnimation, damageOutlinedTextBlock);
                Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(OutlinedTextBlock.OpacityProperty));

                BrushAnimation flashWhiteStrokeAnimation = new BrushAnimation
                {
                    From = whiteBrush,
                    To = redBrush,
                    Duration = TimeSpan.FromSeconds(0.05),
                    //AutoReverse = true,
                    //RepeatBehavior = new RepeatBehavior(2),
                };
                Storyboard.SetTarget(flashWhiteStrokeAnimation, damageOutlinedTextBlock);
                Storyboard.SetTargetProperty(flashWhiteStrokeAnimation, new PropertyPath(OutlinedTextBlock.StrokeProperty));

                BrushAnimation flashWhiteFillAnimation = new BrushAnimation
                {
                    From = whiteBrush,
                    To = redBrush,
                    Duration = TimeSpan.FromSeconds(0.05),
                    //AutoReverse = true,
                    //RepeatBehavior = new RepeatBehavior(2),
                };
                Storyboard.SetTarget(flashWhiteFillAnimation, damageOutlinedTextBlock);
                Storyboard.SetTargetProperty(flashWhiteFillAnimation, new PropertyPath(OutlinedTextBlock.FillProperty));

                fadeInIncreaseSizeFlashColorStoryboard.Children.Add(fadeInAnimation);

                if (s.EnableDamageNumbersSize)
                    fadeInIncreaseSizeFlashColorStoryboard.Children.Add(sizeIncreaseAnimation);

                if (s.EnableDamageNumbersFlash)
                {
                    fadeInIncreaseSizeFlashColorStoryboard.Children.Add(flashWhiteStrokeAnimation);
                    fadeInIncreaseSizeFlashColorStoryboard.Children.Add(flashWhiteFillAnimation);
                }


                Storyboard decreaseSizeShowColorStoryboard = new Storyboard();

                // Create a DoubleAnimation to animate the label's width
                DoubleAnimation sizeDecreaseAnimation = new DoubleAnimation
                {
                    From = damageOutlinedTextBlock.FontSize * 1.5,
                    To = damageOutlinedTextBlock.FontSize,
                    Duration = TimeSpan.FromSeconds(.2),
                };
                Storyboard.SetTarget(sizeDecreaseAnimation, damageOutlinedTextBlock);
                Storyboard.SetTargetProperty(sizeDecreaseAnimation, new PropertyPath(OutlinedTextBlock.FontSizeProperty));

                BrushAnimation showColorStrokeAnimation = new BrushAnimation
                {
                    From = redBrush,
                    //To = (Color)converter.ConvertFromString(damageOutlinedTextBlock.Stroke.ToString()),
                    To = blackBrush,
                    Duration = TimeSpan.FromSeconds(0.05),
                    //AutoReverse = true,
                    //RepeatBehavior = new RepeatBehavior(2),
                };
                Storyboard.SetTarget(showColorStrokeAnimation, damageOutlinedTextBlock);
                Storyboard.SetTargetProperty(showColorStrokeAnimation, new PropertyPath(OutlinedTextBlock.StrokeProperty));

                BrushAnimation showColorFillAnimation = new BrushAnimation
                {
                    From = redBrush,
                    To = originalFillBrush,
                    Duration = TimeSpan.FromSeconds(0.05),
                    //AutoReverse = true,
                    //RepeatBehavior = new RepeatBehavior(2),
                };
                Storyboard.SetTarget(showColorFillAnimation, damageOutlinedTextBlock);
                Storyboard.SetTargetProperty(showColorFillAnimation, new PropertyPath(OutlinedTextBlock.FillProperty));

                if (s.EnableDamageNumbersSize)
                    decreaseSizeShowColorStoryboard.Children.Add(sizeDecreaseAnimation);

                if (s.EnableDamageNumbersFlash)
                {
                    decreaseSizeShowColorStoryboard.Children.Add(showColorStrokeAnimation);
                    decreaseSizeShowColorStoryboard.Children.Add(showColorFillAnimation);
                }


                Storyboard fadeOutStoryboard = new Storyboard();

                DoubleAnimation fadeOutAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(.4),
                };

                fadeOutAnimation.BeginTime = TimeSpan.FromSeconds(0.75);

                Storyboard.SetTarget(fadeOutAnimation, damageOutlinedTextBlock);
                Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(Label.OpacityProperty));

                DoubleAnimation translateUpwardsAnimation = new DoubleAnimation
                {
                    From = y,
                    To = y - 20,
                    Duration = TimeSpan.FromSeconds(.4),
                };

                translateUpwardsAnimation.BeginTime = TimeSpan.FromSeconds(0.75);

                Storyboard.SetTarget(translateUpwardsAnimation, damageOutlinedTextBlock);
                Storyboard.SetTargetProperty(translateUpwardsAnimation, new PropertyPath(Canvas.TopProperty));

                fadeOutStoryboard.Children.Add(fadeOutAnimation);
                fadeOutStoryboard.Children.Add(translateUpwardsAnimation);


                // Set up event handlers to start the next animation in the sequence
                fadeInIncreaseSizeFlashColorStoryboard.Completed += (s, e) => decreaseSizeShowColorStoryboard.Begin();
                decreaseSizeShowColorStoryboard.Completed += (s, e) => fadeOutStoryboard.Begin();
                fadeOutAnimation.Completed += (s, e) => DamageNumbers.Children.Remove(damageOutlinedTextBlock);

                // Start the first animation
                fadeInIncreaseSizeFlashColorStoryboard.Begin();
            }
        }

        /// <summary>
        /// Removes the damage number label.
        /// </summary>
        /// <param name="tb">The tb.</param>
        private void RemoveDamageNumberLabel(OutlinedTextBlock tb)
        {
            DispatcherTimer timer = new();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            //memory leak?
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
            SetMonsterVisibility(v, s);
        }

        private void SetMonsterVisibility(bool v, Settings s)
        {
            DataLoader.model.ShowMonsterAtkMult = v && s.MonsterAtkMultShown;
            DataLoader.model.ShowMonsterDefrate = v && s.MonsterDefrateShown;
            DataLoader.model.ShowMonsterSize = v && s.MonsterSizeShown;
            DataLoader.model.ShowMonsterPoison = v && s.MonsterPoisonShown;
            DataLoader.model.ShowMonsterSleep = v && s.MonsterSleepShown;
            DataLoader.model.ShowMonsterPara = v && s.MonsterParaShown;
            DataLoader.model.ShowMonsterBlast = v && s.MonsterBlastShown;
            DataLoader.model.ShowMonsterStun = v && s.MonsterStunShown;

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
            DataLoader.model.ShowPlayerHitsTakenBlockedInfo = v && s.TotalHitsTakenBlockedShown;
            DataLoader.model.ShowSharpness = v && s.EnableSharpness;
            DataLoader.model.ShowSessionTimeInfo = v && s.SessionTimeShown;

            DataLoader.model.ShowMap = v && s.EnableMap;
            DataLoader.model.ShowFrameCounter = v && s.FrameCounterShown;
            DataLoader.model.ShowPlayerAttackGraph = v && s.PlayerAttackGraphShown;
            DataLoader.model.ShowPlayerDPSGraph = v && s.PlayerDPSGraphShown;
            DataLoader.model.ShowPlayerAPMGraph = v && s.PlayerAPMGraphShown;
            DataLoader.model.ShowPlayerHitsPerSecondGraph = v && s.PlayerHitsPerSecondGraphShown;

            DataLoader.model.ShowDamagePerSecond = v && s.DamagePerSecondShown;

            DataLoader.model.ShowKBMLayout = v && s.KBMLayoutShown;
            DataLoader.model.ShowControllerLayout = v && s.ControllerLayoutShown;
            DataLoader.model.ShowAPM = v && s.ActionsPerMinuteShown;
            DataLoader.model.ShowOverlayModeWatermark = v && s.OverlayModeWatermarkShown;
            DataLoader.model.ShowQuestID = v && s.QuestIDShown;

            DataLoader.model.ShowPersonalBestInfo = v && s.PersonalBestShown;
            DataLoader.model.ShowQuestAttemptsInfo = v && s.QuestAttemptsShown;
            DataLoader.model.ShowPersonalBestTimePercentInfo = v && s.PersonalBestTimePercentShown;
        }

        #endregion

        #region get info

        /// <summary>
        /// Gets the caravan score.
        /// </summary>
        /// <returns></returns>
        public string GetCaravanScore()
        {
            if (ShowCaravanScore())
                return string.Format("Caravan Score: {0} | ", DataLoader.model.CaravanScore());
            else
                return "";
        }

        public static bool ShowCaravanScore()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableCaravanScore)
                return true;
            else
                return false;
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
        public static string GetDivaSkillNameFromID(int id)
        {
            Dictionary.DivaSkillList.DivaSkillID.TryGetValue(id, out string? divaskillaname);
            return divaskillaname + "";
        }

        /// <summary>
        /// Gets the discord timer mode.
        /// </summary>
        /// <returns></returns>
        public static string GetDiscordTimerMode()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.DiscordTimerMode == "Time Left")
                return "Time Left";
            else if (s.DiscordTimerMode == "Time Elapsed")
                return "Time Elapsed";
            else return "Time Left";
        }

        /// <summary>
        /// Gets the weapon style from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string GetWeaponStyleFromID(int id)
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
        public static string getArmorSkill(int id)
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
        public static string GetItemName(int id)
        {
            string itemValue1;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Dictionary.Items.ItemIDs.TryGetValue(id, out itemValue1);  //returns true
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return itemValue1 + "";
        }

        /// <summary>
        /// Gets the weapon name from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string getWeaponNameFromID(int id)
        {
            Dictionary.WeaponTypes.WeaponTypeID.TryGetValue(id, out string? weaponname);
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
        /// Gets the area icon from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string GetAreaIconFromID(int id) //TODO: are highlands, tidal island or painted falls icons correct?
        {
            if (id >= 470 && id < 0)
                return "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/icon/cattleya.png";
            else
                return FindAreaIcon(id);
        }

        private static string FindAreaIcon(int id)
        {
            List<int> AreaGroup = new List<int> { 0 };

            foreach (KeyValuePair<List<int>, string> kvp in AreaIconDictionary.AreaIconID)
            {
                List<int> areaIDs = kvp.Key;

                if (areaIDs.Contains(id))
                {
                    AreaGroup = kvp.Key;
                    break;
                }
            }
            return DetermineAreaIcon(AreaGroup);
        }

        private static string DetermineAreaIcon(List<int> key)
        {
            bool areaIcon = AreaIconDictionary.AreaIconID.ContainsKey(key);
            if (!areaIcon)
                return "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/icon/cattleya.png";
            else
                return AreaIconDictionary.AreaIconID[key];
        }

        /// <summary>
        /// Gets the game mode.
        /// </summary>
        /// <param name="isHighGradeEdition">if set to <c>true</c> [is high grade edition].</param>
        /// <returns></returns>
        public static string GetGameMode(bool isHighGradeEdition)
        {
            if (isHighGradeEdition)
                return " [High-Grade Edition]";
            else
                return "";
        }

        readonly Mem m = new();

        /// <summary>
        /// Shows the current hp percentage.
        /// </summary>
        /// <returns></returns>
        public static bool ShowCurrentHPPercentage()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableCurrentHPPercentage)
                return true;
            else
                return false;
        }

        private int currentMonster1MaxHP = 0;

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
            return DataLoader.model.DisplayMonsterEHP(DataLoader.model.Monster1DefMult(), DataLoader.model.Monster1HPInt(), DataLoader.model.Monster1DefMult());
        }

        /// <summary>
        /// Gets the monster1 maximum ehp.
        /// </summary>
        /// <returns></returns>
        public int GetMonster1MaxEHP()
        {
            return currentMonster1MaxHP;
        }

        /// <summary>
        /// Gets the poogie clothes.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string GetPoogieClothes(int id)
        {
            string? clothesValue1;
            _ = Dictionary.PoogieCostumeList.PoogieCostumeID.TryGetValue(id, out clothesValue1);  //returns true
            return clothesValue1 + "";
        }

        /// <summary>
        /// Gets the objective1 current quantity.
        /// </summary>
        /// <returns></returns>
        public string GetObjective1CurrentQuantity(bool isLargeImageText = false)
        {
            if (DataLoader.model.ShowDiscordQuestNames() && !(isLargeImageText)) return "";
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
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            switch (s.MaxFaintsOverride)
            {
                default:
                    return DataLoader.model.MaxFaints().ToString();
                case "Normal Quests":
                    return DataLoader.model.MaxFaints().ToString();
                case "Shiten/Conquest/Pioneer/Daily/Caravan/Interception Quests":
                    return DataLoader.model.AlternativeMaxFaints().ToString();
                case "Automatic":
                    if (DataLoader.model.roadOverride() != null && DataLoader.model.roadOverride() == false)
                        return DataLoader.model.MaxFaints().ToString();

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
                        return DataLoader.model.AlternativeMaxFaints().ToString();
                    }
                    else
                    {
                        return DataLoader.model.MaxFaints().ToString();
                    }
            }
        }

        #endregion

        #region discord info

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

        private bool StartedRoadElapsedTime = false;

        private bool inDuremudiraArena = false;

        private bool inDuremudiraDoorway = false;

        /// <summary>
        /// Gets the quest information.
        /// </summary>
        /// <returns></returns>
        public string GetQuestInformation()
        {
            if (DataLoader.model.ShowDiscordQuestNames())
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
                        return "Slay 1st District Duremudira | ";
                    case 21746://2nd district dure
                    case 21750://sky corridor version
                        return "Slay 2nd District Duremudira | ";
                    default:
                        if ((DataLoader.model.ObjectiveType() == 0x0 || DataLoader.model.ObjectiveType() == 0x02 || DataLoader.model.ObjectiveType() == 0x1002 || DataLoader.model.ObjectiveType() == 0x10) && (DataLoader.model.QuestID() != 23527 && DataLoader.model.QuestID() != 23628 && DataLoader.model.QuestID() != 21731 && DataLoader.model.QuestID() != 21749 && DataLoader.model.QuestID() != 21746 && DataLoader.model.QuestID() != 21750))
                            return string.Format("{0}{1}{2}{3}{4}{5} | ", DataLoader.model.GetObjectiveNameFromID(DataLoader.model.ObjectiveType(), true), GetObjective1CurrentQuantity(true), DataLoader.model.GetObjective1Quantity(true), DataLoader.model.GetRankNameFromID(DataLoader.model.RankBand(), true), DataLoader.model.GetStarGrade(true), DataLoader.model.GetObjective1Name(DataLoader.model.Objective1ID(), true));
                        else
                            return string.Format("{0}{1}{2}{3}{4}{5} | ", DataLoader.model.GetObjectiveNameFromID(DataLoader.model.ObjectiveType(), true), "", DataLoader.model.GetObjective1Quantity(true), DataLoader.model.GetRankNameFromID(DataLoader.model.RankBand(), true), DataLoader.model.GetStarGrade(true), DataLoader.model.GetRealMonsterName(DataLoader.model.CurrentMonster1Icon, true));
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
            Dictionary.RavienteTriggerEvents.RavienteTriggerEventIDs.TryGetValue(id, out string? EventValue1);
            Dictionary.ViolentRavienteTriggerEvents.ViolentRavienteTriggerEventIDs.TryGetValue(id, out string? EventValue2);
            Dictionary.BerserkRavienteTriggerEvents.BerserkRavienteTriggerEventIDs.TryGetValue(id, out string? EventValue3);
            Dictionary.BerserkRavientePracticeTriggerEvents.BerserkRavientePracticeTriggerEventIDs.TryGetValue(id, out string? EventValue4);

            switch (DataLoader.model.getRaviName())
            {
                default:
                    return "";
                case "Raviente":
                    return EventValue1 + "";
                case "Violent Raviente":
                    return EventValue2 + "";
                case "Berserk Raviente Practice":
                    return EventValue4 + "";
                case "Berserk Raviente":
                    return EventValue3 + "";
                case "Extreme Raviente":
                    return EventValue3 + "";
            }
        }

        private string GetRoadTimerResetMode()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.DiscordRoadTimerReset == "Never")
                return "Never";
            else if (s.DiscordRoadTimerReset == "Always")
                return "Always";
            else return "Never";
        }


        /// <summary>
        /// Get quest state
        /// </summary>
        /// <returns></returns>
        public string GetQuestState()
        {
            if (DataLoader.model.isInLauncherBool) //works?
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
                    return String.Format("Quest Clear! | {0} | ", DataLoader.model.Time);
            }
        }

        /// <summary>
        /// Gets the size of the party.
        /// </summary>
        /// <returns></returns>
        public string GetPartySize()
        {
            if (DataLoader.model.QuestID() == 0 || DataLoader.model.PartySize() == 0 || DataLoader.model.isInLauncher() == "NULL" || DataLoader.model.isInLauncher() == "Yes")
            {
                return "";
            }
            else
            {
                return string.Format("Party: {0}/{1} | ", DataLoader.model.PartySize(), GetPartySizeMax());
            }
        }

        public int GetPartySizeMax()
        {
            if (DataLoader.model.PartySize() >= DataLoader.model.PartySizeMax())
                return DataLoader.model.PartySize();
            else
                return DataLoader.model.PartySizeMax();
        }

        private bool IsInHubAreaID()
        {
            switch (DataLoader.model.AreaID())
            {
                default:
                    return false;
                case 200://Mezeporta
                case 210://Private Bar
                case 260://Pallone Caravan
                case 282://Cities Map
                case 202://Guild Halls
                case 203:
                case 204:
                    return true;
            }
        }

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

        const int MAX_DISCORD_RPC_STRING_LENGTH = 127; // or any other maximum length specified by Discord

        /// <summary>
        /// Updates the discord RPC.
        /// </summary>
        private void UpdateDiscordRPC()
        {
            if (!(isDiscordRPCRunning))
            {
                return;
            }

            // TODO also need to handle the other fields lengths
            if (string.Format("{0}{1}{2}{3}{4}{5}", GetPartySize(), GetQuestState(), GetCaravanScore(), DataLoader.model.GetOverlayModeForRPC(), DataLoader.model.GetAreaName(DataLoader.model.AreaID()), GetGameMode(DataLoader.isHighGradeEdition)).Length >= 95)
                presenceTemplate.Details = string.Format("{0}{1}{2}", GetQuestState(), DataLoader.model.GetOverlayModeForRPC(), DataLoader.model.GetAreaName(DataLoader.model.AreaID()));
            else
                presenceTemplate.Details = string.Format("{0}{1}{2}{3}{4}{5}", GetPartySize(), GetQuestState(), GetCaravanScore(), DataLoader.model.GetOverlayModeForRPC(), DataLoader.model.GetAreaName(DataLoader.model.AreaID()), GetGameMode(DataLoader.isHighGradeEdition));

            // TODO should this be outside UpdateDiscordRPC?
            if (IsInHubAreaID() && DataLoader.model.QuestID() == 0)
                DataLoader.model.PreviousHubAreaID = DataLoader.model.AreaID();

            string stateString = "";
            string largeImageTextString = "";
            string smallImageTextString = "";

            //Info
            if ((DataLoader.model.QuestID() != 0 && DataLoader.model.TimeDefInt() != DataLoader.model.TimeInt() && int.Parse(DataLoader.model.ATK) > 0) || ((DataLoader.model.QuestID() == 21731 || DataLoader.model.QuestID() == 21746 || DataLoader.model.QuestID() == 21749 || DataLoader.model.QuestID() == 21750 || DataLoader.model.QuestID() == 23648 || DataLoader.model.QuestID() == 23649 || DataLoader.model.QuestID() == 21748 || DataLoader.model.QuestID() == 21747 || DataLoader.model.QuestID() == 21734) && int.Parse(DataLoader.model.ATK) > 0))
            {
                switch (DataLoader.model.QuestID())
                {
                    case 23527:// Hunter's Road Multiplayer
                        stateString = String.Format("Multiplayer Floor: {0} ({1}/{2} Max/Total) | RP: {3} | White Fatalis: {4}/{5} (Slain/Encounters)", DataLoader.model.RoadFloor() + 1, DataLoader.model.RoadMaxStagesMultiplayer(), DataLoader.model.RoadTotalStagesMultiplayer(), DataLoader.model.RoadPoints(), DataLoader.model.RoadFatalisSlain(), DataLoader.model.RoadFatalisEncounters());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 23628://solo road
                        stateString = String.Format("Solo Floor: {0} ({1}/{2} Max/Total) | RP: {3} | White Fatalis: {4}/{5} (Slain/Encounters)", DataLoader.model.RoadFloor() + 1, DataLoader.model.RoadMaxStagesSolo(), DataLoader.model.RoadTotalStagesSolo(), DataLoader.model.RoadPoints(), DataLoader.model.RoadFatalisSlain(), DataLoader.model.RoadFatalisEncounters());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 21731://1st district dure
                    case 21749://sky corridor version
                        stateString = String.Format("{0}{1}{2}{3}{4}{5} | Slain: {6} | Encounters: {7}", DataLoader.model.GetQuestNameFromID(DataLoader.model.QuestID()), DataLoader.model.GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), "", DataLoader.model.GetObjective1Quantity(), DataLoader.model.GetRankNameFromID(DataLoader.model.RankBand()), DataLoader.model.GetRealMonsterName(DataLoader.model.CurrentMonster1Icon), DataLoader.model.FirstDistrictDuremudiraSlays(), DataLoader.model.FirstDistrictDuremudiraEncounters());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 21746://2nd district dure
                    case 21750://sky corridor version
                        stateString = String.Format("{0}{1}{2}{3}{4}{5} | Slain: {6} | Encounters: {7}", DataLoader.model.GetQuestNameFromID(DataLoader.model.QuestID()), DataLoader.model.GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), "", DataLoader.model.GetObjective1Quantity(), DataLoader.model.GetRankNameFromID(DataLoader.model.RankBand()), DataLoader.model.GetRealMonsterName(DataLoader.model.CurrentMonster1Icon), DataLoader.model.SecondDistrictDuremudiraSlays(), DataLoader.model.SecondDistrictDuremudiraEncounters());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
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
                        stateString = String.Format("{0}{1}{2}{3}{4}{5}{6} | True Raw: {7} (Max {8}) | Hits: {9}", DataLoader.model.GetQuestNameFromID(DataLoader.model.QuestID()), DataLoader.model.GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), "", DataLoader.model.GetObjective1Quantity(), DataLoader.model.GetRankNameFromID(DataLoader.model.RankBand()), DataLoader.model.GetStarGrade(), DataLoader.model.GetRealMonsterName(DataLoader.model.CurrentMonster1Icon), DataLoader.model.ATK, DataLoader.model.HighestAtk, DataLoader.model.HitCountInt());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";

                        break;
                    default:
                        if ((DataLoader.model.ObjectiveType() == 0x0 || DataLoader.model.ObjectiveType() == 0x02 || DataLoader.model.ObjectiveType() == 0x1002 || DataLoader.model.ObjectiveType() == 0x10) && (DataLoader.model.QuestID() != 23527 && DataLoader.model.QuestID() != 23628 && DataLoader.model.QuestID() != 21731 && DataLoader.model.QuestID() != 21749 && DataLoader.model.QuestID() != 21746 && DataLoader.model.QuestID() != 21750))
                        {
                            stateString = String.Format("{0}{1}{2}{3}{4}{5}{6} | True Raw: {7} (Max {8}) | Hits: {9}", DataLoader.model.GetQuestNameFromID(DataLoader.model.QuestID()), DataLoader.model.GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), GetObjective1CurrentQuantity(), DataLoader.model.GetObjective1Quantity(), DataLoader.model.GetRankNameFromID(DataLoader.model.RankBand()), DataLoader.model.GetStarGrade(), DataLoader.model.GetObjective1Name(DataLoader.model.Objective1ID()), DataLoader.model.ATK, DataLoader.model.HighestAtk, DataLoader.model.HitCountInt());
                            presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        }
                        else
                        {
                            stateString = String.Format("{0}{1}{2}{3}{4}{5}{6} | True Raw: {7} (Max {8}) | Hits: {9}", DataLoader.model.GetQuestNameFromID(DataLoader.model.QuestID()), DataLoader.model.GetObjectiveNameFromID(DataLoader.model.ObjectiveType()), "", DataLoader.model.GetObjective1Quantity(), DataLoader.model.GetRankNameFromID(DataLoader.model.RankBand()), DataLoader.model.GetStarGrade(), DataLoader.model.GetRealMonsterName(DataLoader.model.CurrentMonster1Icon), DataLoader.model.ATK, DataLoader.model.HighestAtk, DataLoader.model.HitCountInt());
                            presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        }
                        break;
                }

                //Gathering/etc
                if ((DataLoader.model.ObjectiveType() == 0x0 || DataLoader.model.ObjectiveType() == 0x02 || DataLoader.model.ObjectiveType() == 0x1002) && (DataLoader.model.QuestID() != 23527 && DataLoader.model.QuestID() != 23628 && DataLoader.model.QuestID() != 21731 && DataLoader.model.QuestID() != 21749 && DataLoader.model.QuestID() != 21746 && DataLoader.model.QuestID() != 21750))
                {
                    largeImageTextString = string.Format("{0}{1}", GetQuestInformation(), DataLoader.model.GetAreaName(DataLoader.model.AreaID()));
                    presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                    presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? largeImageTextString : largeImageTextString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                }
                //Tenrou Sky Corridor areas
                else if (DataLoader.model.AreaID() == 391 || DataLoader.model.AreaID() == 392 || DataLoader.model.AreaID() == 394 || DataLoader.model.AreaID() == 415 || DataLoader.model.AreaID() == 416)
                {
                    largeImageTextString = string.Format("{0}{1}", GetQuestInformation(), DataLoader.model.GetAreaName(DataLoader.model.AreaID()));
                    presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                    presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? largeImageTextString : largeImageTextString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                }
                //Duremudira Doors
                else if (DataLoader.model.AreaID() == 399 || DataLoader.model.AreaID() == 414)
                {
                    presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                    largeImageTextString = string.Format("{0}{1}", GetQuestInformation(), DataLoader.model.GetAreaName(DataLoader.model.AreaID()));
                    presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? largeImageTextString : largeImageTextString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                }
                //Duremudira Arena
                else if (DataLoader.model.AreaID() == 398)
                {
                    presenceTemplate.Assets.LargeImageKey = DataLoader.model.getMonsterIcon(DataLoader.model.LargeMonster1ID());
                    largeImageTextString = string.Format("{0}{1}/{2}{3}", GetQuestInformation(), GetMonster1EHP(), GetMonster1MaxEHP(), GetMonster1EHPPercent());
                    presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? largeImageTextString : largeImageTextString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                }
                //Hunter's Road Base Camp
                else if (DataLoader.model.AreaID() == 459)
                {
                    presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                    largeImageTextString = string.Format("{0}{1} | Faints: {2}/{3}", GetQuestInformation(), DataLoader.model.GetAreaName(DataLoader.model.AreaID()), DataLoader.model.CurrentFaints(), GetMaxFaints());
                    presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? largeImageTextString : largeImageTextString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                }
                //Raviente
                else if (DataLoader.model.AreaID() == 309 || (DataLoader.model.AreaID() >= 311 && DataLoader.model.AreaID() <= 321) || (DataLoader.model.AreaID() >= 417 && DataLoader.model.AreaID() <= 422) || DataLoader.model.AreaID() == 437 || (DataLoader.model.AreaID() >= 440 && DataLoader.model.AreaID() <= 444))
                {
                    presenceTemplate.Assets.LargeImageKey = DataLoader.model.getMonsterIcon(DataLoader.model.LargeMonster1ID());
                    largeImageTextString = string.Format("{0}{1}/{2}{3} | Faints: {4}/{5} | Points: {6} | {7}", GetQuestInformation(), GetMonster1EHP(), GetMonster1MaxEHP(), GetMonster1EHPPercent(), DataLoader.model.CurrentFaints(), GetMaxFaints(), DataLoader.model.GreatSlayingPoints(), GetRavienteEvent(DataLoader.model.RavienteTriggeredEvent()));
                    presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? largeImageTextString : largeImageTextString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                }
                else
                {
                    presenceTemplate.Assets.LargeImageKey = DataLoader.model.getMonsterIcon(DataLoader.model.LargeMonster1ID());
                    largeImageTextString = string.Format("{0}{1}/{2}{3} | Faints: {4}/{5}", GetQuestInformation(), GetMonster1EHP(), GetMonster1MaxEHP(), GetMonster1EHPPercent(), DataLoader.model.CurrentFaints(), GetMaxFaints());
                    presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? largeImageTextString : largeImageTextString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                }
            }
            else if (DataLoader.model.QuestID() == 0)
            {

                switch (DataLoader.model.AreaID())
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
                        stateString = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Diva Skill: {3} ({4} Left) | Poogie Item: {5}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetDivaSkillNameFromID(DataLoader.model.DivaSkill()), DataLoader.model.DivaSkillUsesLeft(), GetItemName(DataLoader.model.PoogieItemUseID()));
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 173:// My House (original)
                    case 175://My House (MAX)
                        stateString = string.Format("GR: {0} | Partner Lv: {1} | Armor Color: {2} | GCP: {3}", DataLoader.model.GRankNumber(), DataLoader.model.PartnerLevel(), getArmorColor(), DataLoader.model.GCP());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 202://Guild Halls
                    case 203:
                    case 204:
                        stateString = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 205://Pugi Farm
                        stateString = string.Format("GR: {0} | Poogie Points: {1} | Poogie Clothes: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.PoogiePoints(), GetPoogieClothes(DataLoader.model.PoogieCostume()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 256://Caravan Areas
                    case 260:
                    case 261:
                    case 262:
                    case 263:
                        stateString = string.Format("CP: {0} | Gg: {1} | g: {2} | Gem Lv: {3} | Great Slaying Points: {4}", DataLoader.model.CaravanPoints(), DataLoader.model.RaviGg(), DataLoader.model.Ravig(), DataLoader.model.CaravenGemLevel() + 1, DataLoader.model.GreatSlayingPointsSaved());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 257://Blacksmith
                        stateString = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | GZenny: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), DataLoader.model.GZenny());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 264://Gallery
                        stateString = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Score: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), DataLoader.model.GalleryEvaluationScore());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 265://Guuku Farm
                        stateString = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 283://Halk Area TODO partnya lv
                        stateString = string.Format("GR: {0} | GCP: {1} | PNRP: {2} | Halk Fullness: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), DataLoader.model.PartnyaRankPoints(), DataLoader.model.HalkFullness());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 286://PvP Room
                        stateString = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 379://Diva Hall
                    case 445:
                        stateString = string.Format("GR: {0} | Diva Skill: {1} ({2} Left) | Diva Bond: {3} | Items Given: {4}", DataLoader.model.GRankNumber(), GetDivaSkillNameFromID(DataLoader.model.DivaSkill()), DataLoader.model.DivaSkillUsesLeft(), DataLoader.model.DivaBond(), DataLoader.model.DivaItemsGiven());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 462://MezFez Entrance
                    case 463: //Volpkun Together
                    case 465://MezFez Minigame
                        stateString = string.Format("GR: {0} | MezFes Points: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.MezeportaFestivalPoints(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 464://Uruki Pachinko
                        stateString = string.Format("Score: {0} | Chain: {1} | Fish: {2} | Mushroom: {3} | Seed: {4} | Meat: {5}", DataLoader.model.UrukiPachinkoScore() + DataLoader.model.UrukiPachinkoBonusScore(), DataLoader.model.UrukiPachinkoChain(), DataLoader.model.UrukiPachinkoFish(), DataLoader.model.UrukiPachinkoMushroom(), DataLoader.model.UrukiPachinkoSeed(), DataLoader.model.UrukiPachinkoMeat());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 466://Guuku Scoop
                        stateString = string.Format("Score: {0} | Small Guuku: {1} | Medium Guuku: {2} | Large Guuku: {3} | Golden Guuku: {4}", DataLoader.model.GuukuScoopScore(), DataLoader.model.GuukuScoopSmall(), DataLoader.model.GuukuScoopMedium(), DataLoader.model.GuukuScoopLarge(), DataLoader.model.GuukuScoopGolden());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 467://Nyanrendo
                        stateString = string.Format("Score: {0}", DataLoader.model.NyanrendoScore());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 468://Panic Honey
                        stateString = string.Format("Honey: {0}", DataLoader.model.PanicHoneyScore());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    case 469://Dokkan Battle Cats
                        stateString = string.Format("Score: {0} | Scale: {1} | Shell: {2} | Camp: {3}", DataLoader.model.DokkanBattleCatsScore(), DataLoader.model.DokkanBattleCatsScale(), DataLoader.model.DokkanBattleCatsShell(), DataLoader.model.DokkanBattleCatsCamp());
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                    default: //same as Mezeporta
                        stateString = string.Format("GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", DataLoader.model.GRankNumber(), DataLoader.model.GCP(), getArmorSkill(DataLoader.model.GuildFoodSkill()), GetItemName(DataLoader.model.PoogieItemUseID()));
                        presenceTemplate.State = stateString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? stateString : stateString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
                        break;
                }

                presenceTemplate.Assets.LargeImageKey = GetAreaIconFromID(DataLoader.model.AreaID());
                largeImageTextString = DataLoader.model.GetAreaName(DataLoader.model.AreaID());
                presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? largeImageTextString : largeImageTextString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
            }

            //Timer
            if ((DataLoader.model.QuestID() != 0 && !DataLoader.model.inQuest && DataLoader.model.TimeDefInt() > DataLoader.model.TimeInt() && int.Parse(DataLoader.model.ATK) > 0) || (DataLoader.model.IsRoad() || DataLoader.model.IsDure()))
            {
                DataLoader.model.inQuest = true;

                if (!(DataLoader.model.IsRoad() || DataLoader.model.IsDure()))
                {
                    presenceTemplate.Timestamps = GetDiscordTimerMode() switch
                    {
                        "Time Left" => Timestamps.FromTimeSpan((double)DataLoader.model.TimeDefInt() / 30.0),
                        "Time Elapsed" => Timestamps.Now,
                        //dure doorway too
                        _ => Timestamps.FromTimeSpan((double)DataLoader.model.TimeDefInt() / 30.0),
                    };
                }

                if (DataLoader.model.IsRoad())
                {
                    switch (GetRoadTimerResetMode())
                    {
                        case "Always":
                            if (DataLoader.model.AreaID() == 458)//Hunter's Road Area 1
                            {
                                break;
                            }

                            else if (DataLoader.model.AreaID() == 459)//Hunter's Road Base Camp
                            {
                                if (DataLoader.model.RoadFloor() + 1 > DataLoader.model.previousRoadFloor)
                                {
                                    // reset values
                                    DataLoader.model.inQuest = false;
                                    currentMonster1MaxHP = 0;
                                    //DataLoader.model.previousRoadFloor = DataLoader.model.RoadFloor() + 1;
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
                                break;
                            }

                            else if (DataLoader.model.AreaID() == 459)//Hunter's Road Base Camp
                            {
                                if (DataLoader.model.RoadFloor() + 1 > DataLoader.model.previousRoadFloor)
                                {
                                    // reset values
                                    DataLoader.model.inQuest = false;
                                    currentMonster1MaxHP = 0;
                                    //DataLoader.model.previousRoadFloor = DataLoader.model.RoadFloor() + 1;

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
                                break;
                            }

                            else if (DataLoader.model.AreaID() == 459)//Hunter's Road Base Camp
                            {
                                if (DataLoader.model.RoadFloor() + 1 > DataLoader.model.previousRoadFloor)
                                {
                                    // reset values
                                    DataLoader.model.inQuest = false;
                                    currentMonster1MaxHP = 0;
                                    //DataLoader.model.previousRoadFloor = DataLoader.model.RoadFloor() + 1;

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

                if (DataLoader.model.IsDure())
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
            // going back to Mezeporta or w/e
            else if (DataLoader.model.QuestState() != 1 && DataLoader.model.QuestID() == 0 && DataLoader.model.inQuest && int.Parse(DataLoader.model.ATK) == 0)
            {
                //reset values
                DataLoader.model.inQuest = false;
                currentMonster1MaxHP = 0;
                //DataLoader.model.previousRoadFloor = 0;
                StartedRoadElapsedTime = false;
                inDuremudiraArena = false;
                inDuremudiraDoorway = false;

                presenceTemplate.Timestamps = Timestamps.Now;
            }

            //SmallInfo
            presenceTemplate.Assets.SmallImageKey = getWeaponIconFromID(DataLoader.model.WeaponType());

            if (GetHunterName != "" && GetGuildName != "" && GetServerName != "")
            {
                smallImageTextString = String.Format("{0} | {1} | {2} | GSR: {3} | {4} Style | Caravan Skills: {5}", GetHunterName, GetGuildName, GetServerName, DataLoader.model.GSR(), GetWeaponStyleFromID(DataLoader.model.WeaponStyle()), GetCaravanSkills());
                presenceTemplate.Assets.SmallImageText = smallImageTextString.Length <= MAX_DISCORD_RPC_STRING_LENGTH ? smallImageTextString : smallImageTextString.Substring(0, MAX_DISCORD_RPC_STRING_LENGTH - 3) + "...";
            }

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
                case "TimerInfo":
                    s.TimerX = (double)(pos.X - XOffset);
                    s.TimerY = (double)(pos.Y - YOffset);
                    break;
                case "PersonalBestInfo":
                    s.PersonalBestX = (double)(pos.X - XOffset);
                    s.PersonalBestY = (double)(pos.Y - YOffset);
                    break;
                case "QuestAttemptsInfo":
                    s.QuestAttemptsX = (double)(pos.X - XOffset);
                    s.QuestAttemptsY = (double)(pos.Y - YOffset);
                    break;
                case "HitCountInfo":
                    s.HitCountX = (double)(pos.X - XOffset);
                    s.HitCountY = (double)(pos.Y - YOffset);
                    break;
                case "PlayerAtkInfo":
                    s.PlayerAtkX = (double)(pos.X - XOffset);
                    s.PlayerAtkY = (double)(pos.Y - YOffset);
                    break;
                case "PlayerHitsTakenBlockedInfo":
                    s.TotalHitsTakenBlockedX = (double)(pos.X - XOffset);
                    s.TotalHitsTakenBlockedY = (double)(pos.Y - YOffset);
                    break;

                // TODO graphs
                case "PlayerAttackGraphGrid":
                    s.PlayerAttackGraphX = (double)(pos.X - XOffset);
                    s.PlayerAttackGraphY = (double)(pos.Y - YOffset);
                    break;
                case "PlayerDPSGraphGrid":
                    s.PlayerDPSGraphX = (double)(pos.X - XOffset);
                    s.PlayerDPSGraphY = (double)(pos.Y - YOffset);
                    break;
                case "PlayerAPMGraphGrid":
                    s.PlayerAPMGraphX = (double)(pos.X - XOffset);
                    s.PlayerAPMGraphY = (double)(pos.Y - YOffset);
                    break;
                case "PlayerHitsPerSecondGraphGrid":
                    s.PlayerHitsPerSecondGraphX = (double)(pos.X - XOffset);
                    s.PlayerHitsPerSecondGraphY = (double)(pos.Y - YOffset);
                    break;

                case "DamagePerSecondInfo":
                    s.PlayerDPSX = (double)(pos.X - XOffset);
                    s.PlayerDPSY = (double)(pos.Y - XOffset);
                    break;
                case "KBMLayoutGrid":
                    s.KBMLayoutX = (double)(pos.X - XOffset);
                    s.KBMLayoutY = (double)(pos.Y - XOffset);
                    break;
                case "ControllerLayoutGrid":
                    s.ControllerLayoutX = (double)(pos.X - XOffset);
                    s.ControllerLayoutY = (double)(pos.Y - XOffset);
                    break;
                case "ActionsPerMinuteInfo":
                    s.ActionsPerMinuteX = (double)(pos.X - XOffset);
                    s.ActionsPerMinuteY = (double)(pos.Y - XOffset);
                    break;
                case "MapImage":
                    s.MapX = (double)(pos.X - XOffset);
                    s.MapY = (double)(pos.Y - YOffset);
                    break;
                //case "OverlayModeWatermark":
                //    s.OverlayModeWatermarkX = (double)(pos.X - XOffset);
                //    s.OverlayModeWatermarkY = (double)(pos.Y - YOffset);
                //    break;
                case "QuestIDGrid":
                    s.QuestIDX = (double)(pos.X - XOffset);
                    s.QuestIDY = (double)(pos.Y - YOffset);
                    break;
                case "SessionTimeInfo":
                    s.SessionTimeX = (double)(pos.X - XOffset);
                    s.SessionTimeY = (double)(pos.Y - XOffset);
                    break;
                case "LocationTextInfo":
                    s.LocationTextX = (double)(pos.X - XOffset);
                    s.LocationTextY = (double)(pos.Y - YOffset);
                    break;
                case "QuestNameInfo":
                    s.QuestNameX = (double)(pos.X - XOffset);
                    s.QuestNameY = (double)(pos.Y - YOffset);
                    break;
                case "PersonalBestTimePercentInfo":
                    s.PersonalBestTimePercentX = (double)(pos.X - XOffset);
                    s.PersonalBestTimePercentY = (double)(pos.Y - YOffset);
                    break;

                // Monster
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

        /// <summary>
        /// Does the drag drop.
        /// </summary>
        /// <param name="item">The item.</param>
        private static void DoDragDrop(FrameworkElement? item)
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
            databaseManager.StoreSessionTime(this);
            Environment.Exit(0);
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
            databaseManager.StoreSessionTime(this);
            Environment.Exit(0);
        }

        //https://stackoverflow.com/questions/4773632/how-do-i-restart-a-wpf-application
        private void ReloadButton_Key()
        {
            Cleanup();
            databaseManager.StoreSessionTime(this);
            _notifyIcon.Dispose();
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenConfigButton_Key()
        {
            if (IsDragConfigure) return;

            if (DataLoader.model.isInLauncherBool)
            {
                System.Windows.MessageBox.Show("Using the configuration menu outside of the game might cause slow performance", "Warning", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                logger.Info("PROGRAM OPERATION: Detected game launcher while using configuration menu");
            }

            if (configWindow == null || !configWindow.IsLoaded)
                configWindow = new(this);
            try
            {
                configWindow.Show();// TODO: memory error?
                DataLoader.model.Configuring = true;
            }
            catch (Exception ex)
            {
                logger.Error("PROGRAM OPERATION: could not show configuration window", ex);
            }

        }

        private void CloseButton_Key()
        {
            Cleanup();
            databaseManager.StoreSessionTime(this);
            _notifyIcon.Dispose();
            Environment.Exit(0);
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
            ToggleOverlayBorders();
        }

        // TODO: use a dictionary for looping instead
        private void ToggleOverlayBorders()
        {
            var thickness = new System.Windows.Thickness(0);

            if (IsDragConfigure)
                thickness = new System.Windows.Thickness(2);

            ActionsPerMinuteInfoBorder.BorderThickness = thickness;
            DamagePerSecondInfoBorder.BorderThickness = thickness;
            HitCountInfoBorder.BorderThickness = thickness;
            LocationTextInfoBorder.BorderThickness = thickness;
            MonsterAtkMultInfoBorder.BorderThickness = thickness;
            MonsterBlastInfoBorder.BorderThickness = thickness;
            MonsterDefrateInfoBorder.BorderThickness = thickness;
            MonsterParaInfoBorder.BorderThickness = thickness;
            MonsterPoisonInfoBorder.BorderThickness = thickness;
            MonsterSizeInfoBorder.BorderThickness = thickness;
            MonsterSleepInfoBorder.BorderThickness = thickness;
            MonsterStunInfoBorder.BorderThickness = thickness;
            PersonalBestInfoBorder.BorderThickness = thickness;
            PersonalBestTimePercentInfoBorder.BorderThickness = thickness;
            PlayerAtkInfoBorder.BorderThickness = thickness;
            PlayerHitsTakenBlockedInfoBorder.BorderThickness = thickness;
            QuestAttemptsInfoBorder.BorderThickness = thickness;
            QuestNameInfoBorder.BorderThickness = thickness;
            SessionTimeInfoBorder.BorderThickness = thickness;
            SharpnessInfoBorder.BorderThickness = thickness;
            TimerInfoBorder.BorderThickness = thickness;
            ControllerLayoutGridBorder.BorderThickness = thickness;
            KBMLayoutGridBorder.BorderThickness = thickness;
            QuestIDGridBorder.BorderThickness = thickness;
            OverlayModeWatermarkBorder.BorderThickness = thickness;
            Monster1HpBarBorder.BorderThickness = thickness;
            Monster2HpBarBorder.BorderThickness = thickness;
            Monster3HpBarBorder.BorderThickness = thickness;
            Monster4HpBarBorder.BorderThickness = thickness;
            MonsterPartThresholdBorder.BorderThickness = thickness;
            DamageNumbersBorder.BorderThickness = thickness;
            PlayerAPMGraphGridBorder.BorderThickness = thickness;
            PlayerAttackGraphGridBorder.BorderThickness = thickness;
            PlayerDPSGraphGridBorder.BorderThickness = thickness;
            PlayerHitsPerSecondGraphGridBorder.BorderThickness = thickness;
        }

        private bool ClickThrough = true;

        private void ToggleClickThrough()
        {
            if (!ClickThrough)
            {
                IsHitTestVisible = false;
                Focusable = false;
                // Get this window's handle         
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                // Change the extended window style to include WS_EX_TRANSPARENT         
                int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);

            }
            else
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
            ToggleOverlayBorders();
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

        #region database

        private bool calculatedPersonalBest = false;
        private bool calculatedQuestAttempts = false;

        private static object lockObj = new object();

        private async void UpdateQuestAttempts()
        {
            string category = OverlayModeWatermarkTextBlock.Text;
            int weaponType = DataLoader.model.WeaponType();
            long questID = DataLoader.model.QuestID();

            int attempts = await Task.Run(() => databaseManager.UpsertQuestAttempts(questID, weaponType, category));

            await Dispatcher.BeginInvoke(new Action(() =>
            {
                questAttemptsTextBlock.Text = attempts.ToString();
            }));
        }

        /// <summary>
        /// Gets the mezeporta festival minigame score depending on area id
        /// </summary>
        /// <param name="areaID"></param>
        /// <returns></returns>
        private int GetMezFesMinigameScore(int areaID)
        {
            // Read player score from corresponding memory address based on current area ID
            int score = 0;
            switch (areaID)
            {
                case 464: // Uruki Pachinko
                    score = DataLoader.model.UrukiPachinkoScore() + DataLoader.model.UrukiPachinkoBonusScore();
                    break;
                case 467: // Nyanrendo
                    score = DataLoader.model.NyanrendoScore();
                    break;
                case 469: // Dokkan Battle Cats
                    score = DataLoader.model.DokkanBattleCatsScore();
                    break;
                case 466: // Guuku Scoop
                    score = DataLoader.model.GuukuScoopScore();
                    break;
                case 468: // Panic Honey
                    score = DataLoader.model.PanicHoneyScore();
                    break;
            }
            return score;
        }

        /// <summary>
        /// Checks the mezeporta festival score. At minigame end, the score is the max obtained, with the area id as the minigame id, then shortly goes to 0 score with the same area id, then switches area id to the lobby id shortly afterwards. 
        /// </summary>
        private void CheckMezFesScore()
        {
            if (DataLoader.model.QuestID() != 0 || !(DataLoader.model.AreaID() == 462 || MezFesMinigame.ID.ContainsKey(DataLoader.model.AreaID())))
                return;

            int areaID = DataLoader.model.AreaID();

            // Check if player is in a minigame area
            if (MezFesMinigame.ID.ContainsKey(areaID))
            {
                // Check if the player has entered a new minigame area
                if (areaID != DataLoader.model.previousMezFesArea)
                {
                    DataLoader.model.previousMezFesArea = areaID;
                    DataLoader.model.previousMezFesScore = 0;
                }

                // Read player score from corresponding memory address based on current area ID
                int score = GetMezFesMinigameScore(areaID);

                // Update current score with new score if it's greater
                if (score > DataLoader.model.previousMezFesScore)
                {
                    DataLoader.model.previousMezFesScore = score;
                }
            }
            // Check if the player has exited a minigame area and the score is 0
            else if (DataLoader.model.previousMezFesArea != -1 && areaID == 462)
            {
                // Save current score and minigame area ID to database
                databaseManager.InsertMezFesMinigameScore(DataLoader, DataLoader.model.previousMezFesArea, DataLoader.model.previousMezFesScore);

                // Reset previousMezFesArea and previousMezFesScore
                DataLoader.model.previousMezFesArea = -1;
                DataLoader.model.previousMezFesScore = 0;
            }
        }

        //TODO: optimization
        private void CheckQuestStateForDatabaseLogging()
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            // Check if in quest and timer is NOT frozen
            if (DataLoader.model.QuestID() != 0 && DataLoader.model.TimeInt() != DataLoader.model.TimeDefInt() && DataLoader.model.QuestState() == 0 && DataLoader.model.previousTimeInt != DataLoader.model.TimeInt())
            {
                DataLoader.model.previousTimeInt = DataLoader.model.TimeInt();
                DataLoader.model.TotalHitsTakenBlockedPerSecond = DataLoader.model.CalculateTotalHitsTakenBlockedPerSecond();
                DataLoader.model.HitsPerSecond = DataLoader.model.CalculateHitsPerSecond();
                DataLoader.model.DPS = DataLoader.model.CalculateDPS();
                DataLoader.model.APM = DataLoader.model.CalculateAPM();
                DataLoader.model.InsertQuestInfoIntoDictionaries();

                //TODO: test on dure/etc
                if (!calculatedPersonalBest && DataLoader.model.TimeDefInt() > DataLoader.model.TimeInt() && int.Parse(DataLoader.model.ATK) > 0)
                {
                    calculatedPersonalBest = true;
                    personalBestTextBlock.Text = databaseManager.GetPersonalBest(DataLoader.model.QuestID(), DataLoader.model.WeaponType(), OverlayModeWatermarkTextBlock.Text, DataLoader.model.QuestTimeMode, DataLoader);
                    DataLoader.model.PersonalBestLoaded = personalBestTextBlock.Text;
                }

                if (!calculatedQuestAttempts && DataLoader.model.TimeDefInt() > DataLoader.model.TimeInt() && int.Parse(DataLoader.model.ATK) > 0)
                {
                    calculatedQuestAttempts = true;
                    UpdateQuestAttempts();
                }
            }

            if ((DataLoader.model.QuestState() == 0 && DataLoader.model.QuestID() == 0))
            {
                DataLoader.model.questCleared = false;
                DataLoader.model.clearQuestInfoDictionaries();
                DataLoader.model.clearGraphCollections();
                DataLoader.model.resetQuestInfoVariables();
                DataLoader.model.previousRoadFloor = 0;
                personalBestTextBlock.Text = "--:--.--";
                calculatedPersonalBest = false;
                calculatedQuestAttempts = false;
                return;
            }
            else if (!DataLoader.model.loadedItemsAtQuestStart && DataLoader.model.QuestState() == 0 && DataLoader.model.QuestID() != 0)
            {
                DataLoader.model.loadedItemsAtQuestStart = true;
                DataLoader.model.PouchItem1IDAtQuestStart = DataLoader.model.PouchItem1ID();
                DataLoader.model.PouchItem2IDAtQuestStart = DataLoader.model.PouchItem2ID();
                DataLoader.model.PouchItem3IDAtQuestStart = DataLoader.model.PouchItem3ID();
                DataLoader.model.PouchItem4IDAtQuestStart = DataLoader.model.PouchItem4ID();
                DataLoader.model.PouchItem5IDAtQuestStart = DataLoader.model.PouchItem5ID();
                DataLoader.model.PouchItem6IDAtQuestStart = DataLoader.model.PouchItem6ID();
                DataLoader.model.PouchItem7IDAtQuestStart = DataLoader.model.PouchItem7ID();
                DataLoader.model.PouchItem8IDAtQuestStart = DataLoader.model.PouchItem8ID();
                DataLoader.model.PouchItem9IDAtQuestStart = DataLoader.model.PouchItem9ID();
                DataLoader.model.PouchItem10IDAtQuestStart = DataLoader.model.PouchItem10ID();
                DataLoader.model.PouchItem11IDAtQuestStart = DataLoader.model.PouchItem11ID();
                DataLoader.model.PouchItem12IDAtQuestStart = DataLoader.model.PouchItem12ID();
                DataLoader.model.PouchItem13IDAtQuestStart = DataLoader.model.PouchItem13ID();
                DataLoader.model.PouchItem14IDAtQuestStart = DataLoader.model.PouchItem14ID();
                DataLoader.model.PouchItem15IDAtQuestStart = DataLoader.model.PouchItem15ID();
                DataLoader.model.PouchItem16IDAtQuestStart = DataLoader.model.PouchItem16ID();
                DataLoader.model.PouchItem17IDAtQuestStart = DataLoader.model.PouchItem17ID();
                DataLoader.model.PouchItem18IDAtQuestStart = DataLoader.model.PouchItem18ID();
                DataLoader.model.PouchItem19IDAtQuestStart = DataLoader.model.PouchItem19ID();
                DataLoader.model.PouchItem20IDAtQuestStart = DataLoader.model.PouchItem20ID();
                DataLoader.model.PouchItem1QuantityAtQuestStart = DataLoader.model.PouchItem1Qty();
                DataLoader.model.PouchItem2QuantityAtQuestStart = DataLoader.model.PouchItem2Qty();
                DataLoader.model.PouchItem3QuantityAtQuestStart = DataLoader.model.PouchItem3Qty();
                DataLoader.model.PouchItem4QuantityAtQuestStart = DataLoader.model.PouchItem4Qty();
                DataLoader.model.PouchItem5QuantityAtQuestStart = DataLoader.model.PouchItem5Qty();
                DataLoader.model.PouchItem6QuantityAtQuestStart = DataLoader.model.PouchItem6Qty();
                DataLoader.model.PouchItem7QuantityAtQuestStart = DataLoader.model.PouchItem7Qty();
                DataLoader.model.PouchItem8QuantityAtQuestStart = DataLoader.model.PouchItem8Qty();
                DataLoader.model.PouchItem9QuantityAtQuestStart = DataLoader.model.PouchItem9Qty();
                DataLoader.model.PouchItem10QuantityAtQuestStart = DataLoader.model.PouchItem10Qty();
                DataLoader.model.PouchItem11QuantityAtQuestStart = DataLoader.model.PouchItem11Qty();
                DataLoader.model.PouchItem12QuantityAtQuestStart = DataLoader.model.PouchItem12Qty();
                DataLoader.model.PouchItem13QuantityAtQuestStart = DataLoader.model.PouchItem13Qty();
                DataLoader.model.PouchItem14QuantityAtQuestStart = DataLoader.model.PouchItem14Qty();
                DataLoader.model.PouchItem15QuantityAtQuestStart = DataLoader.model.PouchItem15Qty();
                DataLoader.model.PouchItem16QuantityAtQuestStart = DataLoader.model.PouchItem16Qty();
                DataLoader.model.PouchItem17QuantityAtQuestStart = DataLoader.model.PouchItem17Qty();
                DataLoader.model.PouchItem18QuantityAtQuestStart = DataLoader.model.PouchItem18Qty();
                DataLoader.model.PouchItem19QuantityAtQuestStart = DataLoader.model.PouchItem19Qty();
                DataLoader.model.PouchItem20QuantityAtQuestStart = DataLoader.model.PouchItem20Qty();

                DataLoader.model.AmmoPouchItem1IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem2IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem3IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem4IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem5IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem6IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem7IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem8IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem9IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem10IDAtQuestStart = DataLoader.model.AmmoPouchItem1ID();
                DataLoader.model.AmmoPouchItem1QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();
                DataLoader.model.AmmoPouchItem2QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();
                DataLoader.model.AmmoPouchItem3QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();
                DataLoader.model.AmmoPouchItem4QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();
                DataLoader.model.AmmoPouchItem5QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();
                DataLoader.model.AmmoPouchItem6QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();
                DataLoader.model.AmmoPouchItem7QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();
                DataLoader.model.AmmoPouchItem8QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();
                DataLoader.model.AmmoPouchItem9QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();
                DataLoader.model.AmmoPouchItem10QuantityAtQuestStart = DataLoader.model.AmmoPouchItem1Qty();

                DataLoader.model.PartnyaBagItem1IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem2IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem3IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem4IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem5IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem6IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem7IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem8IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem9IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem10IDAtQuestStart = DataLoader.model.PartnyaBagItem1ID();
                DataLoader.model.PartnyaBagItem1QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();
                DataLoader.model.PartnyaBagItem2QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();
                DataLoader.model.PartnyaBagItem3QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();
                DataLoader.model.PartnyaBagItem4QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();
                DataLoader.model.PartnyaBagItem5QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();
                DataLoader.model.PartnyaBagItem6QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();
                DataLoader.model.PartnyaBagItem7QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();
                DataLoader.model.PartnyaBagItem8QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();
                DataLoader.model.PartnyaBagItem9QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();
                DataLoader.model.PartnyaBagItem10QuantityAtQuestStart = DataLoader.model.PartnyaBagItem1Qty();

            }

            if (DataLoader.model.QuestState() == 0)
                return;

            // check if quest clear 
            if (DataLoader.model.QuestState() == 1 && !DataLoader.model.questCleared)
            {
                DataLoader.model.questCleared = true;
                DataLoader.model.loadedItemsAtQuestStart = false;
                if (s.EnableQuestLogging)
                    databaseManager.InsertQuestData(DataLoader, int.Parse(questAttemptsTextBlock.Text));
            }
        }

        #endregion

        #region input

        //TODO fix alt tab issues?
        private IKeyboardMouseEvents m_GlobalHook;

        /// <summary>
        /// Subscribes this instance for player input.
        /// </summary>
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            m_GlobalHook.MouseUpExt += GlobalHookMouseUpExt;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
            m_GlobalHook.KeyDown += GlobalHookKeyDown;
            m_GlobalHook.KeyUp += GlobalHookKeyUp;

            // Register the event handler for button presses
            // TODO: do i really need this kind of interface?
            //m_GlobalHook.KeyDown += HandleHorizontalInput;

        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            // goodbye world
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (_mouseImages.ContainsKey(e.Button))
            {
                Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

                if (s.EnableKeyLogging && !DataLoader.model.mouseInputDictionary.ContainsKey(DataLoader.model.TimeInt()) && DataLoader.model.QuestID() != 0 && DataLoader.model.TimeInt() != DataLoader.model.TimeDefInt() && DataLoader.model.QuestState() == 0 && DataLoader.model.previousTimeInt != DataLoader.model.TimeInt() && _mouseImages[e.Button].Opacity == unpressedKeyOpacity)
                {
                    try
                    {
                        DataLoader.model.mouseInputDictionary.Add(DataLoader.model.TimeInt(), e.Button.ToString());
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex, "PROGRAM OPERATION: Could not insert into mouseInputDictionary");
                    }
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _mouseImages[e.Button].Opacity = pressedKeyOpacity;
                }));
            }
            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
        }

        private void GlobalHookMouseUpExt(object sender, MouseEventExtArgs e)
        {
            if (_mouseImages.ContainsKey(e.Button))
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _mouseImages[e.Button].Opacity = unpressedKeyOpacity;
                }));
            }
        }

        public void Unsubscribe()
        {
            m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            m_GlobalHook.MouseUpExt -= GlobalHookMouseUpExt;
            m_GlobalHook.KeyDown -= GlobalHookKeyDown;
            m_GlobalHook.KeyUp -= GlobalHookKeyUp;

            //m_GlobalHook.KeyDown -= HandleHorizontalInput;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }

        private void GlobalHookKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (_keyImages.ContainsKey(e.KeyCode))
            {
                Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

                if (s.EnableKeyLogging && !DataLoader.model.keystrokesDictionary.ContainsKey(DataLoader.model.TimeInt()) && DataLoader.model.QuestID() != 0 && DataLoader.model.TimeInt() != DataLoader.model.TimeDefInt() && DataLoader.model.QuestState() == 0 && DataLoader.model.previousTimeInt != DataLoader.model.TimeInt() && _keyImages[e.KeyCode].Opacity == unpressedKeyOpacity)
                {
                    try
                    {
                        DataLoader.model.keystrokesDictionary.Add(DataLoader.model.TimeInt(), e.KeyCode.ToString());
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex, "PROGRAM OPERATION: Could not insert into keystrokesDictionary");
                    }
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _keyImages[e.KeyCode].Opacity = pressedKeyOpacity;
                }));
            }
        }

        private void GlobalHookKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (_keyImages.ContainsKey(e.KeyCode))
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _keyImages[e.KeyCode].Opacity = unpressedKeyOpacity;
                }));
            }
        }

        /// <summary>
        /// Maps the player input to images.
        /// </summary>
        private void MapPlayerInputImages()
        {
            // Add the key-image pairs to the dictionary
            _keyImages.Add(Keys.D1, Key1);
            _keyImages.Add(Keys.D2, Key2);
            _keyImages.Add(Keys.D3, Key3);
            _keyImages.Add(Keys.D4, Key4);
            _keyImages.Add(Keys.D5, Key5);
            _keyImages.Add(Keys.Q, KeyQ);
            _keyImages.Add(Keys.W, KeyW);
            _keyImages.Add(Keys.E, KeyE);
            _keyImages.Add(Keys.R, KeyR);
            _keyImages.Add(Keys.T, KeyT);
            _keyImages.Add(Keys.A, KeyA);
            _keyImages.Add(Keys.S, KeyS);
            _keyImages.Add(Keys.D, KeyD);
            _keyImages.Add(Keys.F, KeyF);
            _keyImages.Add(Keys.G, KeyG);
            _keyImages.Add(Keys.LShiftKey, KeyShift);
            _keyImages.Add(Keys.Z, KeyZ);
            _keyImages.Add(Keys.X, KeyX);
            _keyImages.Add(Keys.C, KeyC);
            _keyImages.Add(Keys.V, KeyV);
            _keyImages.Add(Keys.LControlKey, KeyCtrl);
            _keyImages.Add(Keys.Space, KeySpace);

            _mouseImages.Add(MouseButtons.Left, MouseLeftClick);
            _mouseImages.Add(MouseButtons.Middle, MouseMiddleClick);
            _mouseImages.Add(MouseButtons.Right, MouseRightClick);

            _controllerImages.Add(X.Gamepad.GamepadButtons.A, ButtonA);
            _controllerImages.Add(X.Gamepad.GamepadButtons.B, ButtonB);
            _controllerImages.Add(X.Gamepad.GamepadButtons.X, ButtonX);
            _controllerImages.Add(X.Gamepad.GamepadButtons.Y, ButtonY);
            _controllerImages.Add(X.Gamepad.GamepadButtons.Dpad_Up, DPad);
            _controllerImages.Add(X.Gamepad.GamepadButtons.Dpad_Left, DPad);
            _controllerImages.Add(X.Gamepad.GamepadButtons.Dpad_Down, DPad);
            _controllerImages.Add(X.Gamepad.GamepadButtons.Dpad_Right, DPad);
            _controllerImages.Add(X.Gamepad.GamepadButtons.Start, ButtonStart);
            _controllerImages.Add(X.Gamepad.GamepadButtons.LeftStick, LJoystick);
            _controllerImages.Add(X.Gamepad.GamepadButtons.RightStick, RJoystick);
            _controllerImages.Add(X.Gamepad.GamepadButtons.LBumper, ButtonL1);
            _controllerImages.Add(X.Gamepad.GamepadButtons.RBumper, ButtonR1);
        }

        double pressedKeyOpacity = 0.5;
        double unpressedKeyOpacity = 0.2;

        #endregion

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
