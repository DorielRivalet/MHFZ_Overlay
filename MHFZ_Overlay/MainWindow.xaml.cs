// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using EZlion.Mapper;
using Gma.System.MouseKeyHook;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Memory;
using MHFZ_Overlay.Core.Class;
using MHFZ_Overlay.Core.Class.Achievements;
using MHFZ_Overlay.Core.Class.Application;
using MHFZ_Overlay.Core.Class.DataAccessLayer;
using MHFZ_Overlay.Core.Class.Dictionary;
using MHFZ_Overlay.Core.Class.Discord;
using MHFZ_Overlay.Core.Class.Log;
using MHFZ_Overlay.Core.Constant;
using MHFZ_Overlay.UI.Class;
using MHFZ_Overlay.UI.Class.Mapper;
using Microsoft.Extensions.DependencyModel;
using Octokit;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;
using Wpf.Ui.Services;
using XInputium;
using XInputium.XInput;
using Application = System.Windows.Application;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using Direction = MHFZ_Overlay.Core.Enum.Direction;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using Image = System.Windows.Controls.Image;
using Label = System.Windows.Controls.Label;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using NotifyIcon = Wpf.Ui.Controls.NotifyIcon;
using Point = System.Windows.Point;
using Window = System.Windows.Window;

namespace MHFZ_Overlay;

/// <summary>
/// Interaction logic for MainWindow.xaml. The main window of the application. It has a DataLoader object, which is used to load the data into the window. It also has several controls, including a custom progress bar (CustomProgressBar), which is bound to the properties of the AddressModel object. The MainWindow also initializes several global hotkeys and registers the Tick event of a DispatcherTimer.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// DataLoader
    /// </summary>
    public DataLoader dataLoader { get; set; }

    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();
    private static readonly AchievementManager achievementManager = AchievementManager.GetInstance();

    private static readonly DiscordManager discordManager = DiscordManager.GetInstance();

    private readonly Mem m = new();

    #region system tray

    public static NotifyIcon? _mainWindowNotifyIcon { get; set; }

    private void CreateSystemTrayIcon()
    {
        _mainWindowNotifyIcon = MainWindowNotifyIcon;
    }

    private void _notifyIcon_Click(object sender, RoutedEventArgs e)
    {
        OpenConfigButton_Key();
    }

    private void OptionSettings_Click(object sender, RoutedEventArgs e)
    {
        OpenConfigButton_Key();
    }

    private void OptionHelp_Click(object sender, RoutedEventArgs e)
    {
        OpenLink("https://github.com/DorielRivalet/mhfz-overlay/blob/main/FAQ.md");
    }

    private void OptionDocumentation_Click(object sender, RoutedEventArgs e)
    {
        OpenLink("https://github.com/DorielRivalet/mhfz-overlay/tree/main/docs");
    }

    private void OptionReportBug_Click(object sender, RoutedEventArgs e)
    {
        OpenLink("https://github.com/DorielRivalet/mhfz-overlay/issues/new?assignees=DorielRivalet&labels=bug&projects=&template=BUG-REPORT.yml&title=%5BBUG%5D+-+title");
    }

    private void OptionRequestFeature_Click(object sender, RoutedEventArgs e)
    {
        OpenLink("https://github.com/DorielRivalet/mhfz-overlay/issues/new?assignees=DorielRivalet&labels=question%2Cenhancement&projects=&template=FEATURE-REQUEST.yml&title=%5BREQUEST%5D+-+title");
    }

    private void OptionSendFeedback_Click(object sender, RoutedEventArgs e)
    {
        OpenLink("https://forms.gle/hrAVWMcYS5HEo1v7A");
    }

    private void OptionSettingsFolder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string settingsFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            var settingsFileDirectoryName = Path.GetDirectoryName(settingsFile);
            if (!Directory.Exists(settingsFileDirectoryName))
            {
                logger.Error("Could not open settings folder");
                MainWindowSnackBar.ShowAsync(Messages.ERROR_TITLE, "Could not open settings folder", new SymbolIcon(SymbolRegular.ErrorCircle24), ControlAppearance.Danger);
                return;
            }
            string settingsFolder = settingsFileDirectoryName;
            // Open file manager at the specified folder
            Process.Start(ApplicationPaths.EXPLORER_PATH, settingsFolder);
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            MainWindowSnackBar.ShowAsync(Messages.ERROR_TITLE, "Could not open settings folder", new SymbolIcon(SymbolRegular.ErrorCircle24), ControlAppearance.Danger);
        }
    }

    private void OptionLogsFolder_Click(object sender, RoutedEventArgs e)
    {
        var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (directoryName == null) return;
        var logFilePath = Path.Combine(directoryName, "logs", "logs.log");

        if (!File.Exists(logFilePath))
        {
            logger.Error("Could not find the log file: {0}", logFilePath);
            System.Windows.MessageBox.Show(string.Format("Could not find the log file: {0}", logFilePath), Messages.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Open the log file using the default application
        try
        {
            var logFilePathDirectory = Path.GetDirectoryName(logFilePath);
            if (logFilePathDirectory == null) return;
            Process.Start(ApplicationPaths.EXPLORER_PATH, logFilePathDirectory);
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }
    }

    private void OptionDatabaseFolder_Click(object sender, RoutedEventArgs e)
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        var directoryName = Path.GetDirectoryName(s.DatabaseFilePath);
        if (directoryName == null) return;

        if (!File.Exists(s.DatabaseFilePath))
        {
            logger.Error("Could not find the database file: {0}", s.DatabaseFilePath);
            System.Windows.MessageBox.Show(string.Format("Could not find the database file: {0}", s.DatabaseFilePath), Messages.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Open the log file using the default application
        try
        {
            Process.Start(ApplicationPaths.EXPLORER_PATH, directoryName);
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }
    }

    private void OptionRestart_Click(object sender, RoutedEventArgs e)
    {
        ReloadButton_Key();
    }

    private void OptionExit_Click(object sender, RoutedEventArgs e)
    {
        CloseButton_Key();
    }

    private void OptionAbout_Click(object sender, RoutedEventArgs e)
    {
        OpenLink("https://github.com/DorielRivalet/mhfz-overlay");
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

    [DllImport("user32.dll")]
    public static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    #endregion

    #region main

    public DateTime ProgramStart;
    public DateTime ProgramEnd;

    // Declare a dictionary to map keys to images
    private readonly Dictionary<Keys, Image> _keyImages = new();

    private readonly Dictionary<MouseButtons, Image> _mouseImages = new();

    // TODO
    private readonly XGamepad gamepad;
    private readonly Dictionary<XInputButton, Image> _gamepadImages = new();
    private readonly Dictionary<XInputium.Trigger, Image> _gamepadTriggersImages = new();
    private readonly Dictionary<Joystick, Image> _gamepadJoystickImages = new();

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
        // Create a Stopwatch instance
        Stopwatch stopwatch = new Stopwatch();
        // Start the stopwatch
        stopwatch.Start();

        var splashScreen = new SplashScreen("UI/Icons/png/loading.png");

        splashScreen.Show(false);
        dataLoader = new DataLoader();
        InitializeComponent();

        logger.Info($"MainWindow initialized");
        logger.Trace(new StackTrace().ToString());

        Left = 0;
        Top = 0;
        Topmost = true;
        DispatcherTimer timer = new();
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        timer.Interval = new TimeSpan(0, 0, 0, 0, 1_000 / s.RefreshRate);
        //memory leak?
        timer.Tick += Timer_Tick;
        timer.Start();
        DataContext = dataLoader.model;
        GlobalHotKey.RegisterHotKey("Shift + F1", () => OpenConfigButton_Key());
        GlobalHotKey.RegisterHotKey("Shift + F5", () => ReloadButton_Key());
        GlobalHotKey.RegisterHotKey("Shift + F6", () => CloseButton_Key());

        DiscordManager.InitializeDiscordRPC();
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
        ProgramStart = DateTime.UtcNow;

        // Calculate the total time spent and update the TotalTimeSpent property
        dataLoader.model.TotalTimeSpent = databaseManager.CalculateTotalTimeSpent();

        MapPlayerInputImages();
        Subscribe();
        // TODO unsubscribe

        // TODO gamepad
        gamepad = new();
        gamepad.ButtonPressed += Gamepad_ButtonPressed;
        gamepad.LeftJoystickMove += Gamepad_LeftJoystickMove;
        gamepad.RightJoystickMove += Gamepad_RightJoystickMove;
        gamepad.LeftTrigger.ToDigitalButton(triggerActivationThreshold).Pressed += Gamepad_LeftTriggerPressed;
        gamepad.RightTrigger.ToDigitalButton(triggerActivationThreshold).Pressed += Gamepad_RightTriggerPressed;
        gamepad.ButtonReleased += Gamepad_ButtonReleased;
        gamepad.LeftTrigger.ToDigitalButton(triggerActivationThreshold).Released += Gamepad_LeftTriggerReleased;
        gamepad.RightTrigger.ToDigitalButton(triggerActivationThreshold).Released += Gamepad_RightTriggerReleased;

        DispatcherTimer timer1Frame = new();
        timer1Frame.Interval = new TimeSpan(0, 0, 0, 0, 1_000 / Numbers.FRAMES_PER_SECOND);
        timer1Frame.Tick += Timer1Frame_Tick;
        timer1Frame.Start();

        SetGraphSeries();
        GetDependencies();

        // The rendering tier corresponds to the high-order word of the Tier property.
        int renderingTier = (RenderCapability.Tier >> 16);

        logger.Info("Found rendering tier {0}", renderingTier);

        CreateSystemTrayIcon();

        DispatcherTimer timer1Second = new();
        timer1Second.Interval = new TimeSpan(0, 0, 1);
        timer1Second.Tick += Timer1Second_Tick;
        timer1Second.Start();

        // we run the 10 seconds timer tick once in the constructor
        try
        {
            HideMonsterInfoWhenNotInQuest();
            HidePlayerInfoWhenNotInQuest();
            dataLoader.CheckForExternalProcesses();
            dataLoader.CheckForIllegalModifications();
        }
        catch (Exception ex)
        {
            LoggingManager.WriteCrashLog(ex);
        }

        DispatcherTimer timer10Seconds = new();
        timer10Seconds.Interval = new TimeSpan(0, 0, 10);
        timer10Seconds.Tick += Timer10Seconds_Tick;
        timer10Seconds.Start();

        dataLoader.model.ShowSaveIcon = false;

        logger.Info("Loaded MHF-Z Overlay {0}", App.CurrentProgramVersion);

        // In your initialization or setup code
        ISnackbarService snackbarService = new SnackbarService();
        // Replace 'snackbarControl' with your actual snackbar control instance
        snackbarService.SetSnackbarControl(MainWindowSnackBar);

        splashScreen.Close(TimeSpan.FromSeconds(0.1));
        // Stop the stopwatch
        stopwatch.Stop();
        // Get the elapsed time in milliseconds
        double elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        // Print the elapsed time
        logger.Debug($"MainWindow ctor Elapsed Time: {elapsedTimeMs} ms");
    }

    private void GetDependencies()
    {
        // Get the dependency context for the current application
        var context = DependencyContext.Default;
        if (context == null) return;
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

        logger.Trace("Loading dependency data\n{0}", dependenciesInfo);
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

        dataLoader.model.attackBuffSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = dataLoader.model.attackBuffCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerAttackGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerAttackGraphColor, "7f")), new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerAttackGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
        });

        dataLoader.model.damagePerSecondSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = dataLoader.model.damagePerSecondCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerDPSGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerDPSGraphColor, "7f")), new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerDPSGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
        });

        dataLoader.model.actionsPerMinuteSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = dataLoader.model.actionsPerMinuteCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerAPMGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerAPMGraphColor, "7f")), new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerAPMGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
        });

        dataLoader.model.hitsPerSecondSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = dataLoader.model.hitsPerSecondCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
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
        if (latestRelease != App.CurrentProgramVersion)
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableUpdateNotifier)
            {
                logger.Info("Detected different overlay version");
                System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(String.Format(
@"Detected different version ({0}) from latest ({1}). Do you want to update the overlay?

The process may take some time, as the program attempts to download from GitHub Releases. You will get a notification once the process is complete.", App.CurrentProgramVersion, latest.TagName), "【MHF-Z】Overlay Update Available", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Asterisk, MessageBoxResult.No);

                if (messageBoxResult.ToString() == "Yes")
                {
                    await App.UpdateMyApp();
                }
            }
        }
    }

    // TODO: refactor to somewhere else
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
        // TODO what is this?
        AutomationElement element = AutomationElement.RootElement.FindFirst(
            TreeScope.Children, condition);

        //get the classname
        if (element != null)
        {
            var className = element.Current.ClassName;

            if (className == "MHFLAUNCH")
            {
                logger.Error("Detected game launcher");
                System.Windows.MessageBox.Show("Detected launcher, please start the overlay when fully loading into Mezeporta. Closing overlay.", Messages.ERROR_TITLE, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                dataLoader.model.isInLauncherBool = true;
                ApplicationManager.HandleShutdown();
            }
            else
            {
                dataLoader.model.isInLauncherBool = false;
            }

            //https://stackoverflow.com/questions/51148/how-do-i-find-out-if-a-process-is-already-running-using-c
            //https://stackoverflow.com/questions/12273825/c-sharp-process-start-how-do-i-know-if-the-process-ended
            Process mhfProcess = Process.GetProcessById(pidToSearch);

            mhfProcess.EnableRaisingEvents = true;
            mhfProcess.Exited += (sender, e) =>
            {

                dataLoader.model.closedGame = true;
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                logger.Info("Detected closed game");
                System.Windows.MessageBox.Show("Detected closed game, closing overlay. Please start the overlay when fully loading into Mezeporta.", Messages.INFO_TITLE, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                //https://stackoverflow.com/a/9050477/18859245
                ApplicationManager.HandleShutdown();
            };
        }
    }

    private bool showedNullError = false;
    private bool showedGameFolderWarning = false;

    // TODO: optimization
    private async void Timer_Tick(object? obj, EventArgs e)
    {
        try
        {
            dataLoader.model.ReloadData();
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
            await CheckQuestStateForDatabaseLogging();

            // TODO should this be here or somewhere else?
            // this is also for database logging
            CheckMezFesScore();

            if (dataLoader.model.isInLauncher() == "NULL" && !showedNullError)
            {
                showedNullError = true;
            }

            if (!showedGameFolderWarning)
            {
                dataLoader.model.ValidateGameFolder();
                showedGameFolderWarning = true;
            }

        }
        catch (Exception ex)
        {
            LoggingManager.WriteCrashLog(ex);
            // the flushing is done automatically according to the docs
        }
    }

    private void Timer1Second_Tick(object? obj, EventArgs e)
    {
        try
        {
            HideMonsterInfoWhenNotInQuest();
            HidePlayerInfoWhenNotInQuest();
            discordManager.UpdateDiscordRPC(dataLoader);
            CheckIfLocationChanged();
            CheckIfQuestChanged();
        }
        catch (Exception ex)
        {
            LoggingManager.WriteCrashLog(ex);
        }
    }

    private void Timer10Seconds_Tick(object? obj, EventArgs e)
    {
        try
        {
            dataLoader.CheckForExternalProcesses();
            dataLoader.CheckForIllegalModifications();
        }
        catch (Exception ex)
        {
            LoggingManager.WriteCrashLog(ex);
        }
    }

    /// <summary>
    /// 1 frame timer tick. Should contain very few calculations.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void Timer1Frame_Tick(object? obj, EventArgs e)
    {
        try
        {
            gamepad.Update();
            if (!gamepad.IsConnected)
            {
                gamepad.Device = XInputDevice.GetFirstConnectedDevice();
                if (_gamepadImages.Count > 0)
                    _gamepadImages.Clear();
                if (_gamepadTriggersImages.Count > 0)
                    _gamepadTriggersImages.Clear();
                if (_gamepadJoystickImages.Count > 0)
                    _gamepadJoystickImages.Clear();
                if (gamepad.IsConnected)
                {
                    logger.Debug("Gamepad reconnected");
                    AddGamepadImages();
                }
            }
        }
        catch (Exception ex)
        {
            LoggingManager.WriteCrashLog(ex);
        }
    }

    private void CheckIfQuestChanged()
    {
        if (dataLoader.model.previousQuestID != dataLoader.model.QuestID() && dataLoader.model.QuestID() != 0)
        {
            dataLoader.model.previousQuestID = dataLoader.model.QuestID();
            ShowQuestName();
        }
        else if (dataLoader.model.QuestID() == 0 && dataLoader.model.previousQuestID != 0)
        {
            dataLoader.model.previousQuestID = dataLoader.model.QuestID();
        }
    }

    private void ShowQuestName()
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");

        if (s == null || !s.QuestNameShown)
            return;

        EZlion.Mapper.Quest.IDName.TryGetValue(dataLoader.model.previousQuestID, out string? previousQuestID);
        if (previousQuestID == null) return;
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
        if (dataLoader.model.previousGlobalAreaID != dataLoader.model.AreaID() && dataLoader.model.AreaID() != 0)
        {
            dataLoader.model.previousGlobalAreaID = dataLoader.model.AreaID();
            ShowLocationName();
        }
    }

    private void ShowLocationName()
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");

        if (s == null || !s.LocationTextShown) return;

        Location.IDName.TryGetValue(dataLoader.model.previousGlobalAreaID, out string? previousGlobalAreaID);
        if (previousGlobalAreaID == null) return;
        locationTextBlock.Text = previousGlobalAreaID;
        Brush blackBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        Brush blueBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x89, 0xB4, 0xFA));
        AnimateOutlinedTextBlock(locationTextBlock, blackBrush, blueBrush);
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
        if (dataLoader.model.QuestID() == 0) return;

        int damage = 0;
        if (dataLoader.model.HitCountInt() == 0)
        {
            curNum = 0;
            prevNum = 0;
            isFirstAttack = true;
        }
        else
        {
            damage = dataLoader.model.DamageDealt();
        }

        if (prevNum != damage)
        {
            curNum = damage - prevNum;
            if (isFirstAttack)
            {
                isFirstAttack = false;
                CreateDamageNumberLabel(damage);
                if (!dataLoader.model.damageDealtDictionary.ContainsKey(dataLoader.model.TimeInt()))
                {
                    try
                    {
                        dataLoader.model.damageDealtDictionary.Add(dataLoader.model.TimeInt(), damage);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex, "Could not insert into damageDealtDictionary");
                    }
                }
            }
            else if (curNum < 0)
            {
                // TODO
                curNum += 1_000;
                CreateDamageNumberLabel(curNum);
                if (!dataLoader.model.damageDealtDictionary.ContainsKey(dataLoader.model.TimeInt()))
                {
                    try
                    {
                        dataLoader.model.damageDealtDictionary.Add(dataLoader.model.TimeInt(), curNum);

                    }
                    catch (Exception ex)
                    {
                        logger.Warn(ex, "Could not insert into damageDealtDictionary");
                    }
                }
            }
            else
            {
                if (curNum != damage)
                {
                    CreateDamageNumberLabel(curNum);
                    if (!dataLoader.model.damageDealtDictionary.ContainsKey(dataLoader.model.TimeInt()))
                    {
                        try
                        {
                            dataLoader.model.damageDealtDictionary.Add(dataLoader.model.TimeInt(), curNum);
                        }
                        catch
                        (Exception ex)
                        {
                            logger.Warn(ex, "Could not insert into damageDealtDictionary");
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
        damageOutlinedTextBlock.FontFamily = new System.Windows.Media.FontFamily(s.DamageNumbersFontFamily);
        damageOutlinedTextBlock.FontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(s.DamageNumbersFontWeight);
        damageOutlinedTextBlock.FontSize = 21;
        damageOutlinedTextBlock.StrokeThickness = 4;
        damageOutlinedTextBlock.Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));

        //does not alter actual number displayed, only the text style
        double damageModifier = damage / (dataLoader.model.CurrentWeaponMultiplier / 2);
        string exclamations = string.Empty;

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
                exclamations += "!";
                break;
            case < 500.0:
                damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xEB, 0xA0, 0xAC));
                damageOutlinedTextBlock.FontSize = 26;
                exclamations += "!!";
                break;
            default:
                damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xF3, 0x8B, 0xA8));
                damageOutlinedTextBlock.FontSize = 30;
                exclamations += "!!!";
                break;
        }

        var defenseMultiplier = Double.Parse(dataLoader.model.DefMult);
        if (defenseMultiplier <= 0)
            defenseMultiplier = 1;
        var effectiveDamage = damage / defenseMultiplier;
        // If the defense rate is so high the effective damage is essentially 0, show the true damage instead.
        if (effectiveDamage == 0) effectiveDamage = damage;

        switch (s.DamageNumbersMode)
        {
            case "Automatic":
                if (!s.EnableEHPNumbers)
                {
                    damageOutlinedTextBlock.Text = damage.ToString();
                    damageOutlinedTextBlock.Text += exclamations;
                    break;
                }
                damageOutlinedTextBlock.Text = effectiveDamage.ToString("F0");
                damageOutlinedTextBlock.Text += exclamations;
                break;
            case "Effective Damage":
                damageOutlinedTextBlock.Text = effectiveDamage.ToString("F0");
                damageOutlinedTextBlock.Text += exclamations;
                break;
            default: // or True Damage
                damageOutlinedTextBlock.Text = damage.ToString();
                damageOutlinedTextBlock.Text += exclamations;
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
        timer.Interval = new TimeSpan(0, 0, 0, 0, 1_000);
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
        Settings s = (Settings)Application.Current.FindResource("Settings");
        bool v = s.AlwaysShowMonsterInfo || dataLoader.model.Configuring || dataLoader.model.QuestID() != 0;
        SetMonsterStatsVisibility(v, s);
    }

    private void SetMonsterStatsVisibility(bool v, Settings s)
    {
        dataLoader.model.ShowMonsterAtkMult = v && s.MonsterAtkMultShown;
        dataLoader.model.ShowMonsterDefrate = v && s.MonsterDefrateShown;
        dataLoader.model.ShowMonsterSize = v && s.MonsterSizeShown;
        dataLoader.model.ShowMonsterPoison = v && s.MonsterPoisonShown;
        dataLoader.model.ShowMonsterSleep = v && s.MonsterSleepShown;
        dataLoader.model.ShowMonsterPara = v && s.MonsterParaShown;
        dataLoader.model.ShowMonsterBlast = v && s.MonsterBlastShown;
        dataLoader.model.ShowMonsterStun = v && s.MonsterStunShown;

        dataLoader.model.ShowMonster1HPBar = v && s.Monster1HealthBarShown;
        dataLoader.model.ShowMonster2HPBar = v && s.Monster2HealthBarShown;
        dataLoader.model.ShowMonster3HPBar = v && s.Monster3HealthBarShown;
        dataLoader.model.ShowMonster4HPBar = v && s.Monster4HealthBarShown;

        dataLoader.model.ShowMonsterPartHP = v && s.PartThresholdShown;
        dataLoader.model.ShowMonster1Icon = v && s.Monster1IconShown;
    }

    /// <summary>
    /// Hides the player information when not in quest.
    /// </summary>
    private void HidePlayerInfoWhenNotInQuest()
    {
        Settings s = (Settings)Application.Current.FindResource("Settings");
        bool v = s.AlwaysShowPlayerInfo || dataLoader.model.Configuring || dataLoader.model.QuestID() != 0;
        SetPlayerStatsVisibility(v, s);
    }

    private void SetPlayerStatsVisibility(bool v, Settings s)
    {
        //DL.m.?.Visibility = v && s.?.IsChecked
        dataLoader.model.ShowTimerInfo = v && s.TimerInfoShown;
        dataLoader.model.ShowHitCountInfo = v && s.HitCountShown;
        dataLoader.model.ShowPlayerAtkInfo = v && s.PlayerAtkShown;
        dataLoader.model.ShowPlayerHitsTakenBlockedInfo = v && s.TotalHitsTakenBlockedShown;
        dataLoader.model.ShowSharpness = v && s.EnableSharpness;
        dataLoader.model.ShowSessionTimeInfo = v && s.SessionTimeShown;

        dataLoader.model.ShowMap = v && s.EnableMap;
        dataLoader.model.ShowFrameCounter = v && s.FrameCounterShown;
        dataLoader.model.ShowPlayerAttackGraph = v && s.PlayerAttackGraphShown;
        dataLoader.model.ShowPlayerDPSGraph = v && s.PlayerDPSGraphShown;
        dataLoader.model.ShowPlayerAPMGraph = v && s.PlayerAPMGraphShown;
        dataLoader.model.ShowPlayerHitsPerSecondGraph = v && s.PlayerHitsPerSecondGraphShown;

        dataLoader.model.ShowDamagePerSecond = v && s.DamagePerSecondShown;

        dataLoader.model.ShowKBMLayout = v && s.KBMLayoutShown;
        dataLoader.model.ShowGamepadLayout = v && s.GamepadShown;
        dataLoader.model.ShowAPM = v && s.ActionsPerMinuteShown;
        dataLoader.model.ShowOverlayModeWatermark = v && s.OverlayModeWatermarkShown;
        dataLoader.model.ShowQuestID = v && s.QuestIDShown;

        dataLoader.model.ShowPersonalBestInfo = v && s.PersonalBestShown;
        dataLoader.model.ShowQuestAttemptsInfo = v && s.QuestAttemptsShown;
        dataLoader.model.ShowPersonalBestTimePercentInfo = v && s.PersonalBestTimePercentShown;
        dataLoader.model.ShowPersonalBestAttemptsInfo = v && s.PersonalBestAttemptsShown;
    }

    #endregion

    #region get info



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
            case "GamepadGrid":
                s.GamepadX = (double)(pos.X - XOffset);
                s.GamepadY = (double)(pos.Y - XOffset);
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
            case "PersonalBestAttemptsInfo":
                s.PersonalBestAttemptsX = (double)(pos.X - XOffset);
                s.PersonalBestAttemptsY = (double)(pos.Y - YOffset);
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
        ApplicationManager.HandleRestart();
    }

    ConfigWindow? configWindow;

    private void OpenConfigButton_Click(object sender, RoutedEventArgs e)
    {
        if (configWindow == null || !configWindow.IsLoaded)
            configWindow = new(this);
        configWindow.Show();
        dataLoader.model.Configuring = true;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        ApplicationManager.HandleShutdown();
    }

    //https://stackoverflow.com/questions/4773632/how-do-i-restart-a-wpf-application
    private void ReloadButton_Key()
    {
        ApplicationManager.HandleRestart();
    }

    private void OpenConfigButton_Key()
    {
        if (IsDragConfigure) return;

        if (dataLoader.model.isInLauncherBool)
        {
            System.Windows.MessageBox.Show("Using the configuration menu outside of the game might cause slow performance", "Warning", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            logger.Info("Detected game launcher while using configuration menu");
        }

        if (configWindow == null || !configWindow.IsLoaded)
            configWindow = new(this);
        try
        {
            configWindow.Show();// TODO: memory error?
            dataLoader.model.Configuring = true;
        }
        catch (Exception ex)
        {
            logger.Error("Could not show configuration window", ex);
        }

        try
        {
            dataLoader.CheckForExternalProcesses();
            dataLoader.CheckForIllegalModifications();
        }
        catch (Exception ex)
        {
            LoggingManager.WriteCrashLog(ex);
        }

    }

    private void CloseButton_Key()
    {
        ApplicationManager.HandleShutdown();
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
        PersonalBestAttemptsInfoBorder.BorderThickness = thickness;
        QuestNameInfoBorder.BorderThickness = thickness;
        SessionTimeInfoBorder.BorderThickness = thickness;
        SharpnessInfoBorder.BorderThickness = thickness;
        TimerInfoBorder.BorderThickness = thickness;
        GamepadGridBorder.BorderThickness = thickness;
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
        TitleBarBorder.BorderThickness = thickness;
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

    private async Task UpdateQuestAttempts()
    {
        string category = OverlayModeWatermarkTextBlock.Text;
        int weaponType = dataLoader.model.WeaponType();
        long questID = dataLoader.model.QuestID();

        int attempts = await databaseManager.UpsertQuestAttemptsAsync(questID, weaponType, category);
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        string completions = "";
        if (s.EnableQuestCompletionsCounter)
            completions = await databaseManager.GetQuestCompletionsAsync(questID, category, weaponType) + "/";

        questAttemptsTextBlock.Text = $"{completions}{attempts}";
    }

    private async Task UpdatePersonalBestAttempts()
    {
        string category = OverlayModeWatermarkTextBlock.Text;
        int weaponType = dataLoader.model.WeaponType();
        long questID = dataLoader.model.QuestID();

        int attempts = await databaseManager.UpsertPersonalBestAttemptsAsync(questID, weaponType, category);
        personalBestAttemptsTextBlock.Text = attempts.ToString();
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
                score = dataLoader.model.UrukiPachinkoScore() + dataLoader.model.UrukiPachinkoBonusScore();
                break;
            case 467: // Nyanrendo
                score = dataLoader.model.NyanrendoScore();
                break;
            case 469: // Dokkan Battle Cats
                score = dataLoader.model.DokkanBattleCatsScore();
                break;
            case 466: // Guuku Scoop
                score = dataLoader.model.GuukuScoopScore();
                break;
            case 468: // Panic Honey
                score = dataLoader.model.PanicHoneyScore();
                break;
        }
        return score;
    }

    /// <summary>
    /// Checks the mezeporta festival score. 
    /// At minigame end, the score is the max obtained, with the area id as the minigame id, 
    /// then shortly goes to 0 score with the same area id, 
    /// then switches area id to the lobby id shortly afterwards. 
    /// </summary>
    private void CheckMezFesScore()
    {
        if (dataLoader.model.QuestID() != 0 || !(dataLoader.model.AreaID() == 462 || MezFesMinigame.ID.ContainsKey(dataLoader.model.AreaID())))
            return;

        int areaID = dataLoader.model.AreaID();

        // Check if player is in a minigame area
        if (MezFesMinigame.ID.ContainsKey(areaID))
        {
            // Check if the player has entered a new minigame area
            if (areaID != dataLoader.model.previousMezFesArea)
            {
                dataLoader.model.previousMezFesArea = areaID;
                dataLoader.model.previousMezFesScore = 0;
            }

            // Read player score from corresponding memory address based on current area ID
            int score = GetMezFesMinigameScore(areaID);

            // Update current score with new score if it's greater and doesn't surpass the UI limit
            if (score > dataLoader.model.previousMezFesScore && score <= 999999)
            {
                dataLoader.model.previousMezFesScore = score;
            }
        }
        // Check if the player has exited a minigame area and the score is 0
        else if (dataLoader.model.previousMezFesArea != -1 && areaID == 462)
        {
            // Save current score and minigame area ID to database
            databaseManager.InsertMezFesMinigameScore(dataLoader, dataLoader.model.previousMezFesArea, dataLoader.model.previousMezFesScore);

            // Reset previousMezFesArea and previousMezFesScore
            dataLoader.model.previousMezFesArea = -1;
            dataLoader.model.previousMezFesScore = 0;
        }
    }

    //TODO: optimization
    private async Task CheckQuestStateForDatabaseLogging()
    {
        Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

        // Check if in quest and timer is NOT frozen
        if (dataLoader.model.QuestID() != 0 && dataLoader.model.TimeInt() != dataLoader.model.TimeDefInt() && dataLoader.model.QuestState() == 0 && dataLoader.model.previousTimeInt != dataLoader.model.TimeInt())
        {
            dataLoader.model.previousTimeInt = dataLoader.model.TimeInt();
            dataLoader.model.TotalHitsTakenBlockedPerSecond = dataLoader.model.CalculateTotalHitsTakenBlockedPerSecond();
            dataLoader.model.HitsPerSecond = dataLoader.model.CalculateHitsPerSecond();
            dataLoader.model.DPS = dataLoader.model.CalculateDPS();
            dataLoader.model.APM = dataLoader.model.CalculateAPM();
            dataLoader.model.InsertQuestInfoIntoDictionaries();

            //TODO: test on dure/etc
            if (!calculatedPersonalBest && dataLoader.model.TimeDefInt() > dataLoader.model.TimeInt() && int.Parse(dataLoader.model.ATK) > 0)
            {
                calculatedPersonalBest = true;
                personalBestTextBlock.Text = await databaseManager.GetPersonalBestAsync(dataLoader.model.QuestID(), dataLoader.model.WeaponType(), OverlayModeWatermarkTextBlock.Text, dataLoader.model.QuestTimeMode, dataLoader);
                dataLoader.model.PersonalBestLoaded = personalBestTextBlock.Text;
            }

            if (!calculatedQuestAttempts
                && dataLoader.model.TimeDefInt() > dataLoader.model.TimeInt()
                && int.Parse(dataLoader.model.ATK) > 0
                && dataLoader.model.TimeDefInt() - dataLoader.model.TimeInt() >= 30)
            {
                calculatedQuestAttempts = true;
                await UpdateQuestAttempts();
                await UpdatePersonalBestAttempts();
            }
        }

        if ((dataLoader.model.QuestState() == 0 && dataLoader.model.QuestID() == 0))
        {
            dataLoader.model.questCleared = false;
            dataLoader.model.questRewardsGiven = false;
            dataLoader.model.clearQuestInfoDictionaries();
            dataLoader.model.clearGraphCollections();
            dataLoader.model.resetQuestInfoVariables();
            dataLoader.model.previousRoadFloor = 0;
            personalBestTextBlock.Text = Messages.TIMER_NOT_LOADED;
            calculatedPersonalBest = false;
            calculatedQuestAttempts = false;
            return;
        }
        else if (!dataLoader.model.loadedItemsAtQuestStart && dataLoader.model.QuestState() == 0 && dataLoader.model.QuestID() != 0)
        {
            dataLoader.model.loadedItemsAtQuestStart = true;
            dataLoader.model.PouchItem1IDAtQuestStart = dataLoader.model.PouchItem1ID();
            dataLoader.model.PouchItem2IDAtQuestStart = dataLoader.model.PouchItem2ID();
            dataLoader.model.PouchItem3IDAtQuestStart = dataLoader.model.PouchItem3ID();
            dataLoader.model.PouchItem4IDAtQuestStart = dataLoader.model.PouchItem4ID();
            dataLoader.model.PouchItem5IDAtQuestStart = dataLoader.model.PouchItem5ID();
            dataLoader.model.PouchItem6IDAtQuestStart = dataLoader.model.PouchItem6ID();
            dataLoader.model.PouchItem7IDAtQuestStart = dataLoader.model.PouchItem7ID();
            dataLoader.model.PouchItem8IDAtQuestStart = dataLoader.model.PouchItem8ID();
            dataLoader.model.PouchItem9IDAtQuestStart = dataLoader.model.PouchItem9ID();
            dataLoader.model.PouchItem10IDAtQuestStart = dataLoader.model.PouchItem10ID();
            dataLoader.model.PouchItem11IDAtQuestStart = dataLoader.model.PouchItem11ID();
            dataLoader.model.PouchItem12IDAtQuestStart = dataLoader.model.PouchItem12ID();
            dataLoader.model.PouchItem13IDAtQuestStart = dataLoader.model.PouchItem13ID();
            dataLoader.model.PouchItem14IDAtQuestStart = dataLoader.model.PouchItem14ID();
            dataLoader.model.PouchItem15IDAtQuestStart = dataLoader.model.PouchItem15ID();
            dataLoader.model.PouchItem16IDAtQuestStart = dataLoader.model.PouchItem16ID();
            dataLoader.model.PouchItem17IDAtQuestStart = dataLoader.model.PouchItem17ID();
            dataLoader.model.PouchItem18IDAtQuestStart = dataLoader.model.PouchItem18ID();
            dataLoader.model.PouchItem19IDAtQuestStart = dataLoader.model.PouchItem19ID();
            dataLoader.model.PouchItem20IDAtQuestStart = dataLoader.model.PouchItem20ID();
            dataLoader.model.PouchItem1QuantityAtQuestStart = dataLoader.model.PouchItem1Qty();
            dataLoader.model.PouchItem2QuantityAtQuestStart = dataLoader.model.PouchItem2Qty();
            dataLoader.model.PouchItem3QuantityAtQuestStart = dataLoader.model.PouchItem3Qty();
            dataLoader.model.PouchItem4QuantityAtQuestStart = dataLoader.model.PouchItem4Qty();
            dataLoader.model.PouchItem5QuantityAtQuestStart = dataLoader.model.PouchItem5Qty();
            dataLoader.model.PouchItem6QuantityAtQuestStart = dataLoader.model.PouchItem6Qty();
            dataLoader.model.PouchItem7QuantityAtQuestStart = dataLoader.model.PouchItem7Qty();
            dataLoader.model.PouchItem8QuantityAtQuestStart = dataLoader.model.PouchItem8Qty();
            dataLoader.model.PouchItem9QuantityAtQuestStart = dataLoader.model.PouchItem9Qty();
            dataLoader.model.PouchItem10QuantityAtQuestStart = dataLoader.model.PouchItem10Qty();
            dataLoader.model.PouchItem11QuantityAtQuestStart = dataLoader.model.PouchItem11Qty();
            dataLoader.model.PouchItem12QuantityAtQuestStart = dataLoader.model.PouchItem12Qty();
            dataLoader.model.PouchItem13QuantityAtQuestStart = dataLoader.model.PouchItem13Qty();
            dataLoader.model.PouchItem14QuantityAtQuestStart = dataLoader.model.PouchItem14Qty();
            dataLoader.model.PouchItem15QuantityAtQuestStart = dataLoader.model.PouchItem15Qty();
            dataLoader.model.PouchItem16QuantityAtQuestStart = dataLoader.model.PouchItem16Qty();
            dataLoader.model.PouchItem17QuantityAtQuestStart = dataLoader.model.PouchItem17Qty();
            dataLoader.model.PouchItem18QuantityAtQuestStart = dataLoader.model.PouchItem18Qty();
            dataLoader.model.PouchItem19QuantityAtQuestStart = dataLoader.model.PouchItem19Qty();
            dataLoader.model.PouchItem20QuantityAtQuestStart = dataLoader.model.PouchItem20Qty();

            dataLoader.model.AmmoPouchItem1IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem2IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem3IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem4IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem5IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem6IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem7IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem8IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem9IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem10IDAtQuestStart = dataLoader.model.AmmoPouchItem1ID();
            dataLoader.model.AmmoPouchItem1QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();
            dataLoader.model.AmmoPouchItem2QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();
            dataLoader.model.AmmoPouchItem3QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();
            dataLoader.model.AmmoPouchItem4QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();
            dataLoader.model.AmmoPouchItem5QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();
            dataLoader.model.AmmoPouchItem6QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();
            dataLoader.model.AmmoPouchItem7QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();
            dataLoader.model.AmmoPouchItem8QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();
            dataLoader.model.AmmoPouchItem9QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();
            dataLoader.model.AmmoPouchItem10QuantityAtQuestStart = dataLoader.model.AmmoPouchItem1Qty();

            dataLoader.model.PartnyaBagItem1IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem2IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem3IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem4IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem5IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem6IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem7IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem8IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem9IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem10IDAtQuestStart = dataLoader.model.PartnyaBagItem1ID();
            dataLoader.model.PartnyaBagItem1QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();
            dataLoader.model.PartnyaBagItem2QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();
            dataLoader.model.PartnyaBagItem3QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();
            dataLoader.model.PartnyaBagItem4QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();
            dataLoader.model.PartnyaBagItem5QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();
            dataLoader.model.PartnyaBagItem6QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();
            dataLoader.model.PartnyaBagItem7QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();
            dataLoader.model.PartnyaBagItem8QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();
            dataLoader.model.PartnyaBagItem9QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();
            dataLoader.model.PartnyaBagItem10QuantityAtQuestStart = dataLoader.model.PartnyaBagItem1Qty();

        }

        if (dataLoader.model.QuestState() == 0)
            return;

        // check if quest clear 
        if (dataLoader.model.QuestState() == 1 && !dataLoader.model.questCleared)
        {
            // TODO test on dure/road/etc
            // If this code is ever reached, it is not known if the cause is from the overlay interacting with the server,
            // the server itself, or just the overlay itself.
            // The overlay does NOT write to memory addresses.
            if (dataLoader.model.TimeDefInt() - dataLoader.model.TimeInt() == 0)
            {
                logger.Fatal("Illegal quest completion time [ID {0}]", dataLoader.model.QuestID());
                ApplicationManager.HandleGameShutdown();
                LoggingManager.WriteCrashLog(new Exception($"Illegal quest completion time [ID {dataLoader.model.QuestID()}]"));
            }

            dataLoader.model.questCleared = true;
            dataLoader.model.loadedItemsAtQuestStart = false;
            if (s.EnableQuestLogging)
                databaseManager.InsertQuestData(dataLoader, (int)databaseManager.GetQuestAttempts((long)dataLoader.model.QuestID(), dataLoader.model.WeaponType(), OverlayModeWatermarkTextBlock.Text));
        }

        // check if rewards given
        if (dataLoader.model.QuestState() == 129 && !dataLoader.model.questRewardsGiven)
        {
            dataLoader.model.questRewardsGiven = true;
            // TODO: add logging check requirement in case the user needs the hash sets.
            // We await since we are dealing with database
            await achievementManager.CheckForAchievementsAsync(MainWindowSnackBar, dataLoader, databaseManager, s);
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

            if (s.EnableInputLogging && !dataLoader.model.mouseInputDictionary.ContainsKey(dataLoader.model.TimeInt()) && dataLoader.model.QuestID() != 0 && dataLoader.model.TimeInt() != dataLoader.model.TimeDefInt() && dataLoader.model.QuestState() == 0 && dataLoader.model.previousTimeInt != dataLoader.model.TimeInt() && _mouseImages[e.Button].Opacity == unpressedInputOpacity)
            {
                try
                {
                    dataLoader.model.mouseInputDictionary.Add(dataLoader.model.TimeInt(), e.Button.ToString());
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Could not insert into mouseInputDictionary");
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _mouseImages[e.Button].Opacity = pressedInputOpacity;
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
                _mouseImages[e.Button].Opacity = unpressedInputOpacity;
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

            if (s.EnableInputLogging && !dataLoader.model.keystrokesDictionary.ContainsKey(dataLoader.model.TimeInt()) && dataLoader.model.QuestID() != 0 && dataLoader.model.TimeInt() != dataLoader.model.TimeDefInt() && dataLoader.model.QuestState() == 0 && dataLoader.model.previousTimeInt != dataLoader.model.TimeInt() && _keyImages[e.KeyCode].Opacity == unpressedInputOpacity)
            {
                try
                {
                    dataLoader.model.keystrokesDictionary.Add(dataLoader.model.TimeInt(), e.KeyCode.ToString());
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Could not insert into keystrokesDictionary");
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _keyImages[e.KeyCode].Opacity = pressedInputOpacity;
            }));
        }
    }

    private void GlobalHookKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        if (_keyImages.ContainsKey(e.KeyCode))
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _keyImages[e.KeyCode].Opacity = unpressedInputOpacity;
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

        // TODO
        if (gamepad == null)
        {
            logger.Debug("Gamepad not found");
            return;
        }

        AddGamepadImages();
    }

    private void AddGamepadImages()
    {
        _gamepadImages.Add(gamepad.Buttons.A, ButtonA);
        _gamepadImages.Add(gamepad.Buttons.B, ButtonB);
        _gamepadImages.Add(gamepad.Buttons.X, ButtonX);
        _gamepadImages.Add(gamepad.Buttons.Y, ButtonY);
        _gamepadImages.Add(gamepad.Buttons.Start, ButtonStart);
        _gamepadImages.Add(gamepad.Buttons.Back, ButtonSelect);
        _gamepadImages.Add(gamepad.Buttons.LS, LJoystick);
        _gamepadImages.Add(gamepad.Buttons.RS, RJoystick);
        _gamepadImages.Add(gamepad.Buttons.LB, ButtonL1);
        _gamepadImages.Add(gamepad.Buttons.RB, ButtonR1);
        _gamepadTriggersImages.Add(gamepad.LeftTrigger, ButtonL2);
        _gamepadTriggersImages.Add(gamepad.RightTrigger, ButtonR2);
        _gamepadJoystickImages.Add(gamepad.LeftJoystick, LJoystickMovement);
        _gamepadJoystickImages.Add(gamepad.RightJoystick, RJoystickMovement);
    }

    double pressedInputOpacity = 0.5;
    double unpressedInputOpacity = 0.2;
    float triggerActivationThreshold = 0.5f;
    float joystickThreshold = 0.5f;

    private void Gamepad_RightTriggerReleased(object? sender, EventArgs e)
    {
        if (_gamepadTriggersImages.ContainsKey(gamepad.RightTrigger))
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _gamepadTriggersImages[gamepad.RightTrigger].Opacity = unpressedInputOpacity;
            }));
        }
    }

    private void Gamepad_LeftTriggerReleased(object? sender, EventArgs e)
    {
        if (_gamepadTriggersImages.ContainsKey(gamepad.LeftTrigger))
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _gamepadTriggersImages[gamepad.LeftTrigger].Opacity = unpressedInputOpacity;
            }));
        }
    }

    private void Gamepad_ButtonReleased(object? sender, DigitalButtonEventArgs<XInputButton> e)
    {
        if (e.Button == gamepad.Buttons.DPadLeft || e.Button == gamepad.Buttons.DPadUp || e.Button == gamepad.Buttons.DPadRight || e.Button == gamepad.Buttons.DPadDown)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateDpadImage(unpressedInputOpacity);
            }));
        }
        else if (e.Button == gamepad.Buttons.LS)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateLeftStickImage(unpressedInputOpacity);
            }));
        }
        else if (e.Button == gamepad.Buttons.RS)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateRightStickImage(unpressedInputOpacity);
            }));
        }
        else if (_gamepadImages.ContainsKey(e.Button))
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _gamepadImages[e.Button].Opacity = unpressedInputOpacity;
            }));
        }
    }

    private void Gamepad_RightTriggerPressed(object? sender, EventArgs e)
    {
        if (_gamepadTriggersImages.ContainsKey(gamepad.RightTrigger))
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !dataLoader.model.gamepadInputDictionary.ContainsKey(dataLoader.model.TimeInt()) && dataLoader.model.QuestID() != 0 && dataLoader.model.TimeInt() != dataLoader.model.TimeDefInt() && dataLoader.model.QuestState() == 0 && dataLoader.model.previousTimeInt != dataLoader.model.TimeInt() && _gamepadTriggersImages[gamepad.RightTrigger].Opacity == unpressedInputOpacity)
            {
                try
                {
                    dataLoader.model.gamepadInputDictionary.Add(dataLoader.model.TimeInt(), gamepad.RightTrigger.ToString());
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _gamepadTriggersImages[gamepad.RightTrigger].Opacity = pressedInputOpacity;
            }));
        }
    }

    private void Gamepad_LeftTriggerPressed(object? sender, EventArgs e)
    {
        if (_gamepadTriggersImages.ContainsKey(gamepad.LeftTrigger))
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !dataLoader.model.gamepadInputDictionary.ContainsKey(dataLoader.model.TimeInt()) && dataLoader.model.QuestID() != 0 && dataLoader.model.TimeInt() != dataLoader.model.TimeDefInt() && dataLoader.model.QuestState() == 0 && dataLoader.model.previousTimeInt != dataLoader.model.TimeInt() && _gamepadTriggersImages[gamepad.LeftTrigger].Opacity == unpressedInputOpacity)
            {
                try
                {
                    dataLoader.model.gamepadInputDictionary.Add(dataLoader.model.TimeInt(), gamepad.LeftTrigger.ToString());
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _gamepadTriggersImages[gamepad.LeftTrigger].Opacity = pressedInputOpacity;
            }));
        }
    }

    private void Gamepad_LeftJoystickMove(object? sender, EventArgs e)
    {
        UpdateLeftJoystickImage();
    }

    private void UpdateLeftJoystickImage()
    {
        // Get the joystick's X and Y positions
        float x = gamepad.LeftJoystick.X;
        float y = gamepad.LeftJoystick.Y;
        double opacity = pressedInputOpacity;

        // Calculate the joystick direction based on X and Y values
        Direction direction;
        if (Math.Abs(x) <= joystickThreshold && Math.Abs(y) <= joystickThreshold)
        {
            direction = Direction.None;
            opacity = unpressedInputOpacity;
        }
        else if (Math.Abs(x) <= joystickThreshold && y > joystickThreshold)
        {
            direction = Direction.Up;
        }
        else if (x > joystickThreshold && y > joystickThreshold)
        {
            direction = Direction.UpRight;
        }
        else if (x > joystickThreshold && Math.Abs(y) <= joystickThreshold)
        {
            direction = Direction.Right;
        }
        else if (x > joystickThreshold && y < -joystickThreshold)
        {
            direction = Direction.DownRight;
        }
        else if (Math.Abs(x) <= joystickThreshold && y < -joystickThreshold)
        {
            direction = Direction.Down;
        }
        else if (x < -joystickThreshold && y < -joystickThreshold)
        {
            direction = Direction.DownLeft;
        }
        else if (x < -joystickThreshold && Math.Abs(y) <= joystickThreshold)
        {
            direction = Direction.Left;
        }
        else if (x < -joystickThreshold && y > joystickThreshold)
        {
            direction = Direction.UpLeft;
        }
        else
        {
            direction = Direction.None;
            opacity = unpressedInputOpacity;
        }

        // Get the image path based on the direction
        string imagePath = JoystickImageMapper.GetImage(direction);

        // Get the current image source of the left joystick
        var currentImageSource = LJoystickMovement.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the left joystick
            LJoystickMovement.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            LJoystickMovement.Opacity = opacity;
        }
    }

    private void Gamepad_RightJoystickMove(object? sender, EventArgs e)
    {
        UpdateRightJoystickImage();
    }

    private void UpdateRightJoystickImage()
    {
        // Get the joystick's X and Y positions
        float x = gamepad.RightJoystick.X;
        float y = gamepad.RightJoystick.Y;
        double opacity = pressedInputOpacity;

        // Calculate the joystick direction based on X and Y values
        Direction direction;
        if (Math.Abs(x) <= joystickThreshold && Math.Abs(y) <= joystickThreshold)
        {
            direction = Direction.None;
            opacity = unpressedInputOpacity;
        }
        else if (Math.Abs(x) <= joystickThreshold && y > joystickThreshold)
        {
            direction = Direction.Up;
        }
        else if (x > joystickThreshold && y > joystickThreshold)
        {
            direction = Direction.UpRight;
        }
        else if (x > joystickThreshold && Math.Abs(y) <= joystickThreshold)
        {
            direction = Direction.Right;
        }
        else if (x > joystickThreshold && y < -joystickThreshold)
        {
            direction = Direction.DownRight;
        }
        else if (Math.Abs(x) <= joystickThreshold && y < -joystickThreshold)
        {
            direction = Direction.Down;
        }
        else if (x < -joystickThreshold && y < -joystickThreshold)
        {
            direction = Direction.DownLeft;
        }
        else if (x < -joystickThreshold && Math.Abs(y) <= joystickThreshold)
        {
            direction = Direction.Left;
        }
        else if (x < -joystickThreshold && y > joystickThreshold)
        {
            direction = Direction.UpLeft;
        }
        else
        {
            direction = Direction.None;
            opacity = unpressedInputOpacity;
        }

        // Get the image path based on the direction
        string imagePath = JoystickImageMapper.GetImage(direction);

        // Get the current image source of the left joystick
        var currentImageSource = RJoystickMovement.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the left joystick
            RJoystickMovement.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            RJoystickMovement.Opacity = opacity;
        }
    }

    private void Gamepad_DPadPressed(XInputButton button)
    {
        Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

        if (s.EnableInputLogging && !dataLoader.model.gamepadInputDictionary.ContainsKey(dataLoader.model.TimeInt()) && dataLoader.model.QuestID() != 0 && dataLoader.model.TimeInt() != dataLoader.model.TimeDefInt() && dataLoader.model.QuestState() == 0 && dataLoader.model.previousTimeInt != dataLoader.model.TimeInt() && DPad.Opacity == unpressedInputOpacity)
        {
            try
            {
                dataLoader.model.gamepadInputDictionary.Add(dataLoader.model.TimeInt(), button.ToString());
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Could not insert into gamepadInputDictionary (Gamepad_DPadPressed)");
            }
        }

        Dispatcher.BeginInvoke(new Action(() =>
        {
            UpdateDpadImage(pressedInputOpacity);
        }));
    }

    private void Gamepad_ButtonPressed(object? sender, DigitalButtonEventArgs<XInputButton> e)
    {
        if (e.Button == gamepad.Buttons.DPadLeft || e.Button == gamepad.Buttons.DPadUp || e.Button == gamepad.Buttons.DPadRight || e.Button == gamepad.Buttons.DPadDown)
        {
            Gamepad_DPadPressed(e.Button);
            return;
        }

        if (_gamepadImages.ContainsKey(e.Button))
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !dataLoader.model.gamepadInputDictionary.ContainsKey(dataLoader.model.TimeInt()) && dataLoader.model.QuestID() != 0 && dataLoader.model.TimeInt() != dataLoader.model.TimeDefInt() && dataLoader.model.QuestState() == 0 && dataLoader.model.previousTimeInt != dataLoader.model.TimeInt() && _gamepadImages[e.Button].Opacity == unpressedInputOpacity)
            {
                try
                {
                    dataLoader.model.gamepadInputDictionary.Add(dataLoader.model.TimeInt(), e.Button.ToString());
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }

            if (e.Button == gamepad.Buttons.LS)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    UpdateLeftStickImage(pressedInputOpacity);
                }));
            }
            else if (e.Button == gamepad.Buttons.RS)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    UpdateRightStickImage(pressedInputOpacity);
                }));
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _gamepadImages[e.Button].Opacity = pressedInputOpacity;
                }));
            }
        }
    }

    private void UpdateRightStickImage(double opacity)
    {
        // Get the image path based on the direction
        string imagePath = JoystickImageMapper.GetImage(Direction.None);

        // Get the current image source of the D-pad
        var currentImageSource = RJoystick.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the D-pad
            RJoystick.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        RJoystick.Opacity = opacity;
    }

    private void UpdateLeftStickImage(double opacity)
    {
        // Get the image path based on the direction
        string imagePath = JoystickImageMapper.GetImage(Direction.None);

        // Get the current image source of the D-pad
        var currentImageSource = LJoystick.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the D-pad
            LJoystick.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        LJoystick.Opacity = opacity;
    }

    private void UpdateDpadImage(double opacity)
    {
        // Determine the D-pad direction based on the button states
        Direction direction;
        if (gamepad.Buttons.DPadUp.IsPressed)
        {
            direction = Direction.Up;
        }
        else if (gamepad.Buttons.DPadDown.IsPressed)
        {
            direction = Direction.Down;
        }
        else if (gamepad.Buttons.DPadLeft.IsPressed)
        {
            direction = Direction.Left;
        }
        else if (gamepad.Buttons.DPadRight.IsPressed)
        {
            direction = Direction.Right;
        }
        else
        {
            direction = Direction.None;
        }

        // Get the image path based on the direction
        string imagePath = DPadImageMapper.GetImage(direction);

        // Get the current image source of the D-pad
        var currentImageSource = DPad.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the D-pad
            DPad.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        DPad.Opacity = opacity;
    }

    #endregion

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (dataLoader.loadedOutsideMezeporta)
        {
            MainWindowSnackBar.ShowAsync(Messages.WARNING_TITLE, "It is not recommended to load the overlay outside of Mezeporta", new SymbolIcon(SymbolRegular.Warning28), ControlAppearance.Caution);
        }
        databaseManager.LoadDatabaseDataIntoHashSets(SaveIconGrid, dataLoader);
        achievementManager.LoadPlayerAchievements();
    }

    private void victoryMediaElement_MediaEnded(object sender, RoutedEventArgs e)
    {
        victoryMediaElement.Stop();
    }

    public static MediaElement? victoryMediaSound { get; private set; }

    private void victoryMediaElement_Loaded(object sender, RoutedEventArgs e)
    {
        MediaElement mediaElement = (MediaElement)sender;
        if (mediaElement != null)
            victoryMediaSound = mediaElement;
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
/// </TODO>
