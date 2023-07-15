// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Views.Windows;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using EZlion.Mapper;
using Gma.System.MouseKeyHook;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Memory;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.Services.Converter;
using MHFZ_Overlay.Services.Hotkey;
using MHFZ_Overlay.Views.CustomControls;
using Microsoft.Extensions.DependencyModel;
using Octokit;
using SkiaSharp;
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
using Direction = MHFZ_Overlay.Models.Structures.Direction;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using Image = System.Windows.Controls.Image;
using Label = System.Windows.Controls.Label;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using NotifyIcon = Wpf.Ui.Controls.NotifyIcon;
using Point = System.Windows.Point;
using Window = System.Windows.Window;

/// <summary>
/// Interaction logic for MainWindow.xaml. The main window of the application. It has a DataLoader object, which is used to load the data into the window. It also has several controls, including a custom progress bar (CustomProgressBar), which is bound to the properties of the AddressModel object. The MainWindow also initializes several global hotkeys and registers the Tick event of a DispatcherTimer.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// DataLoader
    /// </summary>
    public DataLoader dataLoader { get; set; }

    private static readonly NLog.Logger LoggerInstance = NLog.LogManager.GetCurrentClassLogger();

    private static readonly DatabaseService DatabaseManagerInstance = DatabaseService.GetInstance();

    private static readonly AchievementService AchievementManagerInstance = AchievementService.GetInstance();

    private static readonly OverlaySettingsService OverlaySettingsManagerInstance = OverlaySettingsService.GetInstance();

    private static readonly DiscordService DiscordManagerInstance = DiscordService.GetInstance();

    private readonly Mem m = new();

    public static NotifyIcon? _mainWindowNotifyIcon { get; set; }

    private void CreateSystemTrayIcon()
    {
        _mainWindowNotifyIcon = this.MainWindowNotifyIcon;
    }

    private void _notifyIcon_Click(object sender, RoutedEventArgs e)
    {
        this.OpenConfigButton_Key();
    }

    private void OptionSettings_Click(object sender, RoutedEventArgs e)
    {
        this.OpenConfigButton_Key();
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

    private void OptionChangelog_Click(object sender, RoutedEventArgs e)
    {
        OpenLink("https://github.com/DorielRivalet/mhfz-overlay/blob/main/CHANGELOG.md");
    }

    private void OptionSettingsFolder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string settingsFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            var settingsFileDirectoryName = Path.GetDirectoryName(settingsFile);
            if (!Directory.Exists(settingsFileDirectoryName))
            {
                LoggerInstance.Error(CultureInfo.InvariantCulture, "Could not open settings folder");
                this.MainWindowSnackBar.ShowAsync(Messages.ErrorTitle, "Could not open settings folder", new SymbolIcon(SymbolRegular.ErrorCircle24), ControlAppearance.Danger);
                return;
            }

            string settingsFolder = settingsFileDirectoryName;

            // Open file manager at the specified folder
            Process.Start(ApplicationPaths.ExplorerPath, settingsFolder);
        }
        catch (Exception ex)
        {
            LoggerInstance.Error(ex);
            this.MainWindowSnackBar.ShowAsync(Messages.ErrorTitle, "Could not open settings folder", new SymbolIcon(SymbolRegular.ErrorCircle24), ControlAppearance.Danger);
        }
    }

    private void OptionLogsFolder_Click(object sender, RoutedEventArgs e)
    {
        var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (directoryName == null)
        {
            return;
        }

        var logFilePath = Path.Combine(directoryName, "logs", "logs.log");

        if (!File.Exists(logFilePath))
        {
            LoggerInstance.Error(CultureInfo.InvariantCulture, "Could not find the log file: {0}", logFilePath);
            System.Windows.MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Could not find the log file: {0}", logFilePath), Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Open the log file using the default application
        try
        {
            var logFilePathDirectory = Path.GetDirectoryName(logFilePath);
            if (logFilePathDirectory == null)
            {
                return;
            }

            Process.Start(ApplicationPaths.ExplorerPath, logFilePathDirectory);
        }
        catch (Exception ex)
        {
            LoggerInstance.Error(ex);
        }
    }

    private void OptionDatabaseFolder_Click(object sender, RoutedEventArgs e)
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        var directoryName = Path.GetDirectoryName(s.DatabaseFilePath);
        if (directoryName == null)
        {
            return;
        }

        if (!File.Exists(s.DatabaseFilePath))
        {
            LoggerInstance.Error(CultureInfo.InvariantCulture, "Could not find the database file: {0}", s.DatabaseFilePath);
            System.Windows.MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Could not find the database file: {0}", s.DatabaseFilePath), Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Open the log file using the default application
        try
        {
            Process.Start(ApplicationPaths.ExplorerPath, directoryName);
        }
        catch (Exception ex)
        {
            LoggerInstance.Error(ex);
        }
    }

    private void OptionRestart_Click(object sender, RoutedEventArgs e)
    {
        this.ReloadButton_Key();
    }

    private void OptionExit_Click(object sender, RoutedEventArgs e)
    {
        this.CloseButton_Key();
    }

    private void OptionAbout_Click(object sender, RoutedEventArgs e)
    {
        OpenLink("https://github.com/DorielRivalet/mhfz-overlay");
    }

    private int originalStyle;

    // https://stackoverflow.com/questions/2798245/click-through-in-c-sharp-form
    // https://stackoverflow.com/questions/686132/opening-a-form-in-c-sharp-without-focus/10727337#10727337
    // https://social.msdn.microsoft.com/Forums/en-us/a5e3cbbb-fd07-4343-9b60-6903cdfeca76/click-through-window-with-image-wpf-issues-httransparent-isnt-working?forum=csharplanguage        
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

        if (this.originalStyle == 0)
        {
            this.originalStyle = extendedStyle;
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

    private DateTime ProgramStart;

    private DateTime ProgramEnd;

    // Declare a dictionary to map keys to images
    private readonly Dictionary<Keys, Image> _keyImages = new();

    private readonly Dictionary<MouseButtons, Image> _mouseImages = new();

    // TODO
    private readonly XGamepad gamepad;

    private readonly Dictionary<XInputButton, Image> _gamepadImages = new();

    private readonly Dictionary<XInputium.Trigger, Image> _gamepadTriggersImages = new();

    private readonly Dictionary<Joystick, Image> _gamepadJoystickImages = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        // Create a Stopwatch instance
        Stopwatch stopwatch = new Stopwatch();

        // Start the stopwatch
        stopwatch.Start();

        var splashScreen = new SplashScreen("Assets/Icons/png/loading.png");

        splashScreen.Show(false);
        this.dataLoader = new DataLoader();
        this.InitializeComponent();

        LoggerInstance.Info(CultureInfo.InvariantCulture, $"MainWindow initialized");
        LoggerInstance.Trace(new StackTrace().ToString());

        this.Left = 0;
        this.Top = 0;
        this.Topmost = true;
        DispatcherTimer timer = new();
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        timer.Interval = new TimeSpan(0, 0, 0, 0, 1_000 / s.RefreshRate);

        // memory leak?
        timer.Tick += this.Timer_Tick;
        timer.Start();

        this.dataLoader.model.ValidateGameFolder();

        DataContext = this.dataLoader.model;
        GlobalHotKey.RegisterHotKey("Shift + F1", () => this.OpenConfigButton_Key());
        GlobalHotKey.RegisterHotKey("Shift + F5", () => this.ReloadButton_Key());
        GlobalHotKey.RegisterHotKey("Shift + F6", () => this.CloseButton_Key());

        DiscordService.InitializeDiscordRPC();
        this.CheckGameState();
        _ = this.LoadOctoKit();

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
        this.ProgramStart = DateTime.UtcNow;

        // Calculate the total time spent and update the TotalTimeSpent property
        this.dataLoader.model.TotalTimeSpent = DatabaseManagerInstance.CalculateTotalTimeSpent();

        this.MapPlayerInputImages();
        this.Subscribe();

        // TODO unsubscribe
        // TODO gamepad
        this.gamepad = new();
        this.gamepad.ButtonPressed += this.Gamepad_ButtonPressed;
        this.gamepad.LeftJoystickMove += this.Gamepad_LeftJoystickMove;
        this.gamepad.RightJoystickMove += this.Gamepad_RightJoystickMove;
        this.gamepad.LeftTrigger.ToDigitalButton(this.triggerActivationThreshold).Pressed += this.Gamepad_LeftTriggerPressed;
        this.gamepad.RightTrigger.ToDigitalButton(this.triggerActivationThreshold).Pressed += this.Gamepad_RightTriggerPressed;
        this.gamepad.ButtonReleased += this.Gamepad_ButtonReleased;
        this.gamepad.LeftTrigger.ToDigitalButton(this.triggerActivationThreshold).Released += this.Gamepad_LeftTriggerReleased;
        this.gamepad.RightTrigger.ToDigitalButton(this.triggerActivationThreshold).Released += this.Gamepad_RightTriggerReleased;

        DispatcherTimer timer1Frame = new();
        timer1Frame.Interval = new TimeSpan(0, 0, 0, 0, 1_000 / Numbers.FramesPerSecond);
        timer1Frame.Tick += this.Timer1Frame_Tick;
        timer1Frame.Start();

        this.SetGraphSeries();
        this.GetDependencies();

        // The rendering tier corresponds to the high-order word of the Tier property.
        int renderingTier = (RenderCapability.Tier >> 16);

        LoggerInstance.Info(CultureInfo.InvariantCulture, "Found rendering tier {0}", renderingTier);

        this.CreateSystemTrayIcon();

        DispatcherTimer timer1Second = new();
        timer1Second.Interval = new TimeSpan(0, 0, 1);
        timer1Second.Tick += this.Timer1Second_Tick;

        // we run the 1 second timer tick once in the constructor
        try
        {
            this.HideMonsterInfoWhenNotInQuest();
            this.HidePlayerInfoWhenNotInQuest();
            this.dataLoader.CheckForExternalProcesses();
            this.dataLoader.CheckForIllegalModifications();
        }
        catch (Exception ex)
        {
            LoggingService.WriteCrashLog(ex);
        }

        timer1Second.Start();

        DispatcherTimer timer10Seconds = new();
        timer10Seconds.Interval = new TimeSpan(0, 0, 10);
        timer10Seconds.Tick += this.Timer10Seconds_Tick;
        timer10Seconds.Start();

        this.dataLoader.model.ShowSaveIcon = false;

        LoggerInstance.Info(CultureInfo.InvariantCulture, "Loaded MHF-Z Overlay {0}", App.CurrentProgramVersion);

        // In your initialization or setup code
        ISnackbarService snackbarService = new SnackbarService();

        // Replace 'snackbarControl' with your actual snackbar control instance
        snackbarService.SetSnackbarControl(this.MainWindowSnackBar);

        splashScreen.Close(TimeSpan.FromSeconds(0.1));

        // Stop the stopwatch
        stopwatch.Stop();

        // Get the elapsed time in milliseconds
        double elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        // Print the elapsed time
        LoggerInstance.Debug($"MainWindow ctor Elapsed Time: {elapsedTimeMs} ms");
    }

    private void GetDependencies()
    {
        // Get the dependency context for the current application
        var context = DependencyContext.Default;
        if (context == null)
        {
            return;
        }

        // Build a string with information about all the dependencies
        var sb = new StringBuilder();
        var runtimeTarget = RuntimeInformation.FrameworkDescription;
        sb.AppendLine(CultureInfo.InvariantCulture, $"Target framework: {runtimeTarget}");
        foreach (var lib in context.RuntimeLibraries)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"Library: {lib.Name} {lib.Version}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"  Type: {lib.Type}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"  Hash: {lib.Hash}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"  Dependencies:");
            foreach (var dep in lib.Dependencies)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"    {dep.Name} {dep.Version}");
            }
        }

        string dependenciesInfo = sb.ToString();

        LoggerInstance.Trace(CultureInfo.InvariantCulture, "Loading dependency data\n{0}", dependenciesInfo);
    }

    /// <summary>
    /// Sets the graph series for player stats.
    /// </summary>
    private void SetGraphSeries()
    {
        // TODO graphs
        // https://stackoverflow.com/questions/74719777/livecharts2-binding-continuously-changing-data-to-graph
        // inspired by HunterPie
        Settings s = (Settings)Application.Current.TryFindResource("Settings");

        this.dataLoader.model.attackBuffSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = this.dataLoader.model.attackBuffCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerAttackGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerAttackGraphColor, "7f")), new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerAttackGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.dataLoader.model.damagePerSecondSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = this.dataLoader.model.damagePerSecondCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerDPSGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerDPSGraphColor, "7f")), new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerDPSGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.dataLoader.model.actionsPerMinuteSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = this.dataLoader.model.actionsPerMinuteCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerAPMGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerAPMGraphColor, "7f")), new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerAPMGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.dataLoader.model.hitsPerSecondSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = this.dataLoader.model.hitsPerSecondCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(this.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
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
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Detected different overlay version");
                System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(String.Format(CultureInfo.InvariantCulture,
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
        int processID = m.GetProcIdFromName("mhf");

        // https://stackoverflow.com/questions/12372534/how-to-get-a-process-window-class-name-from-c
        int pidToSearch = processID;

        // Init a condition indicating that you want to search by process id.
        var condition = new PropertyCondition(AutomationElementIdentifiers.ProcessIdProperty,
            pidToSearch);

        // Find the automation element matching the criteria
        // TODO what is this?
        AutomationElement element = AutomationElement.RootElement.FindFirst(
            TreeScope.Children, condition);

        // get the classname
        if (element != null)
        {
            var className = element.Current.ClassName;

            if (className == "MHFLAUNCH")
            {
                LoggerInstance.Error(CultureInfo.InvariantCulture, "Detected game launcher");
                System.Windows.MessageBox.Show("Detected launcher, please start the overlay when fully loading into Mezeporta. Closing overlay.", Messages.ErrorTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                this.dataLoader.model.isInLauncherBool = true;
                ApplicationService.HandleShutdown();
            }
            else
            {
                this.dataLoader.model.isInLauncherBool = false;
            }

            // https://stackoverflow.com/questions/51148/how-do-i-find-out-if-a-process-is-already-running-using-c
            // https://stackoverflow.com/questions/12273825/c-sharp-process-start-how-do-i-know-if-the-process-ended
            Process mhfProcess = Process.GetProcessById(pidToSearch);

            mhfProcess.EnableRaisingEvents = true;
            mhfProcess.Exited += (sender, e) =>
            {
                this.dataLoader.model.closedGame = true;
                Settings s = (Settings)Application.Current.TryFindResource("Settings");
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Detected closed game");
                System.Windows.MessageBox.Show("Detected closed game, closing overlay. Please start the overlay when fully loading into Mezeporta.", Messages.InfoTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                // https://stackoverflow.com/a/9050477/18859245
                ApplicationService.HandleShutdown();
            };
        }
    }

    // TODO: optimization
    private async void Timer_Tick(object? obj, EventArgs e)
    {
        try
        {
            this.dataLoader.model.ReloadData();
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
        }
        catch (Exception ex)
        {
            LoggingService.WriteCrashLog(ex);

            // the flushing is done automatically according to the docs
        }
    }

    private async void Timer1Second_Tick(object? obj, EventArgs e)
    {
        try
        {
            HideMonsterInfoWhenNotInQuest();
            HidePlayerInfoWhenNotInQuest();
            await Task.Run(() => DiscordManagerInstance.UpdateDiscordRPC(dataLoader));
            CheckIfLocationChanged();
            CheckIfQuestChanged();
        }
        catch (Exception ex)
        {
            LoggingService.WriteCrashLog(ex);
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
            LoggingService.WriteCrashLog(ex);
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
                {
                    _gamepadImages.Clear();
                }

                if (_gamepadTriggersImages.Count > 0)
                {
                    _gamepadTriggersImages.Clear();
                }

                if (_gamepadJoystickImages.Count > 0)
                {
                    _gamepadJoystickImages.Clear();
                }

                if (gamepad.IsConnected)
                {
                    LoggerInstance.Debug("Gamepad reconnected");
                    AddGamepadImages();
                }
            }
        }
        catch (Exception ex)
        {
            LoggingService.WriteCrashLog(ex);
        }
    }

    private void CheckIfQuestChanged()
    {
        if (this.dataLoader.model.previousQuestID != this.dataLoader.model.QuestID() && this.dataLoader.model.QuestID() != 0)
        {
            this.dataLoader.model.previousQuestID = this.dataLoader.model.QuestID();
            ShowQuestName();
        }
        else if (this.dataLoader.model.QuestID() == 0 && this.dataLoader.model.previousQuestID != 0)
        {
            this.dataLoader.model.previousQuestID = this.dataLoader.model.QuestID();
        }
    }

    private void ShowQuestName()
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");

        if (s == null || !s.QuestNameShown)
        {
            return;
        }

        EZlion.Mapper.Quest.IDName.TryGetValue(this.dataLoader.model.previousQuestID, out string? previousQuestID);
        if (previousQuestID == null)
        {
            return;
        }

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

    private bool IsInHubAreaID(DataLoader dataLoader)
    {
        switch (this.dataLoader.model.AreaID())
        {
            default:
                return false;
            case 200:// Mezeporta
            case 210:// Private Bar
            case 260:// Pallone Caravan
            case 282:// Cities Map
            case 202:// Guild Halls
            case 203:
            case 204:
                return true;
        }
    }

    private void CheckIfLocationChanged()
    {
        if (IsInHubAreaID(dataLoader) && this.dataLoader.model.QuestID() == 0)
        {
            this.dataLoader.model.PreviousHubAreaID = this.dataLoader.model.AreaID();
        }

        if (this.dataLoader.model.previousGlobalAreaID != this.dataLoader.model.AreaID() && this.dataLoader.model.AreaID() != 0)
        {
            this.dataLoader.model.previousGlobalAreaID = this.dataLoader.model.AreaID();
            ShowLocationName();
        }
    }

    private void ShowLocationName()
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");

        if (s == null || !s.LocationTextShown)
        {
            return;
        }

        Location.IDName.TryGetValue(this.dataLoader.model.previousGlobalAreaID, out string? previousGlobalAreaID);
        if (previousGlobalAreaID == null)
        {
            return;
        }

        locationTextBlock.Text = previousGlobalAreaID;
        Brush blackBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        Brush blueBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x89, 0xB4, 0xFA));
        AnimateOutlinedTextBlock(locationTextBlock, blackBrush, blueBrush);
    }

    int curNum { get; set; }

    int prevNum { get; set; }

    bool isFirstAttack { get; set; }

    public bool IsDragConfigure { get; set; }

    /// <summary>
    /// Creates the damage number.
    /// </summary>
    private void CreateDamageNumber()
    {
        if (this.dataLoader.model.QuestID() == 0)
        {
            return;
        }

        int damage = 0;
        if (this.dataLoader.model.HitCountInt() == 0)
        {
            curNum = 0;
            prevNum = 0;
            isFirstAttack = true;
        }
        else
        {
            damage = this.dataLoader.model.DamageDealt();
        }

        if (prevNum != damage)
        {
            curNum = damage - prevNum;
            if (isFirstAttack)
            {
                isFirstAttack = false;
                CreateDamageNumberLabel(damage);
                if (!this.dataLoader.model.damageDealtDictionary.ContainsKey(this.dataLoader.model.TimeInt()))
                {
                    try
                    {
                        this.dataLoader.model.damageDealtDictionary.Add(this.dataLoader.model.TimeInt(), damage);
                    }
                    catch (Exception ex)
                    {
                        LoggerInstance.Warn(ex, "Could not insert into damageDealtDictionary");
                    }
                }
            }
            else if (curNum < 0)
            {
                // TODO
                curNum += 1_000;
                CreateDamageNumberLabel(curNum);
                if (!this.dataLoader.model.damageDealtDictionary.ContainsKey(this.dataLoader.model.TimeInt()))
                {
                    try
                    {
                        this.dataLoader.model.damageDealtDictionary.Add(this.dataLoader.model.TimeInt(), curNum);
                    }
                    catch (Exception ex)
                    {
                        LoggerInstance.Warn(ex, "Could not insert into damageDealtDictionary");
                    }
                }
            }
            else
            {
                if (curNum != damage)
                {
                    CreateDamageNumberLabel(curNum);
                    if (!this.dataLoader.model.damageDealtDictionary.ContainsKey(this.dataLoader.model.TimeInt()))
                    {
                        try
                        {
                            this.dataLoader.model.damageDealtDictionary.Add(this.dataLoader.model.TimeInt(), curNum);
                        }
                        catch
                        (Exception ex)
                        {
                            LoggerInstance.Warn(ex, "Could not insert into damageDealtDictionary");
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
        return s.EnableDamageNumbersMulticolor;
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

        // does not alter actual number displayed, only the text style
        double damageModifier = damage / (this.dataLoader.model.CurrentWeaponMultiplier / 2);
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

        var defenseMultiplier = Double.Parse(this.dataLoader.model.DefMult, CultureInfo.InvariantCulture);
        if (defenseMultiplier <= 0)
        {
            defenseMultiplier = 1;
        }

        var effectiveDamage = damage / defenseMultiplier;

        // If the defense rate is so high that the effective damage is essentially 0, show the true damage instead.
        if (effectiveDamage == 0)
        {
            effectiveDamage = damage;
        }

        switch (s.DamageNumbersMode)
        {
            case "Automatic":
                if (!s.EnableEHPNumbers)
                {
                    damageOutlinedTextBlock.Text = damage.ToString(CultureInfo.InvariantCulture);
                    damageOutlinedTextBlock.Text += exclamations;
                    break;
                }

                damageOutlinedTextBlock.Text = effectiveDamage.ToString("F0", CultureInfo.InvariantCulture);
                damageOutlinedTextBlock.Text += exclamations;
                break;
            case "Effective Damage":
                damageOutlinedTextBlock.Text = effectiveDamage.ToString("F0", CultureInfo.InvariantCulture);
                damageOutlinedTextBlock.Text += exclamations;
                break;
            default: // or True Damage
                damageOutlinedTextBlock.Text = damage.ToString(CultureInfo.InvariantCulture);
                damageOutlinedTextBlock.Text += exclamations;
                break;
        }

        // TODO add check for effects
        if (!ShowDamageNumbersMulticolor())
        {
            // https://stackoverflow.com/questions/14601759/convert-color-to-byte-value
            System.Drawing.Color color = ColorTranslator.FromHtml(s.DamageNumbersColor);
            damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        damageOutlinedTextBlock.SetValue(Canvas.TopProperty, newPoint.Y);
        damageOutlinedTextBlock.SetValue(Canvas.LeftProperty, newPoint.X);

        this.DamageNumbers.Children.Add(damageOutlinedTextBlock);

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
            };
            Storyboard.SetTarget(flashWhiteStrokeAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(flashWhiteStrokeAnimation, new PropertyPath(OutlinedTextBlock.StrokeProperty));

            BrushAnimation flashWhiteFillAnimation = new BrushAnimation
            {
                From = whiteBrush,
                To = redBrush,
                Duration = TimeSpan.FromSeconds(0.05),
            };
            Storyboard.SetTarget(flashWhiteFillAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(flashWhiteFillAnimation, new PropertyPath(OutlinedTextBlock.FillProperty));

            fadeInIncreaseSizeFlashColorStoryboard.Children.Add(fadeInAnimation);

            if (s.EnableDamageNumbersSize)
            {
                fadeInIncreaseSizeFlashColorStoryboard.Children.Add(sizeIncreaseAnimation);
            }

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
                To = blackBrush,
                Duration = TimeSpan.FromSeconds(0.05),
            };
            Storyboard.SetTarget(showColorStrokeAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(showColorStrokeAnimation, new PropertyPath(OutlinedTextBlock.StrokeProperty));

            BrushAnimation showColorFillAnimation = new BrushAnimation
            {
                From = redBrush,
                To = originalFillBrush,
                Duration = TimeSpan.FromSeconds(0.05),
            };
            Storyboard.SetTarget(showColorFillAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(showColorFillAnimation, new PropertyPath(OutlinedTextBlock.FillProperty));

            if (s.EnableDamageNumbersSize)
            {
                decreaseSizeShowColorStoryboard.Children.Add(sizeDecreaseAnimation);
            }

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

        // memory leak?
        timer.Tick += (o, e) => { DamageNumbers.Children.Remove(tb); };
        timer.Start();
    }

    // does this sometimes bug?
    // the UI flashes at least once when loading into quest        
    /// <summary>
    /// Hides the monster information when not in quest.
    /// </summary>
    private void HideMonsterInfoWhenNotInQuest()
    {
        Settings s = (Settings)Application.Current.FindResource("Settings");
        bool v = IsGameFocused(s) &&
            (s.AlwaysShowMonsterInfo || this.dataLoader.model.Configuring || this.dataLoader.model.QuestID() != 0);
        SetMonsterStatsVisibility(v, s);
    }

    // Import the necessary Win32 functions
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    private static Process currentProcess = Process.GetCurrentProcess();

    /// <summary>
    /// Checks if the game or overlay is focused.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is game focused; otherwise, <c>false</c>.
    /// </value>
    private bool IsGameFocused(Settings s)
    {
        if (!s.HideOverlayWhenUnfocusedGame || this.dataLoader.model.Configuring)
        {
            return true;
        }

        // Get the active window handle
        IntPtr activeWindowHandle = GetForegroundWindow();

        // Check if the active window belongs to the current process or any process spawned from it
        if (activeWindowHandle == currentProcess.MainWindowHandle)
        {
            return true;
        }
        else
        {
            // Check if the active window belongs to the game process
            bool isGameProcessActive = (mhfProcess != null && activeWindowHandle == mhfProcess.MainWindowHandle);
            return isGameProcessActive;
        }
    }

    private void SetMonsterStatsVisibility(bool v, Settings s)
    {
        this.dataLoader.model.ShowMonsterAtkMult = v && s.MonsterAtkMultShown;
        this.dataLoader.model.ShowMonsterDefrate = v && s.MonsterDefrateShown;
        this.dataLoader.model.ShowMonsterSize = v && s.MonsterSizeShown;
        this.dataLoader.model.ShowMonsterPoison = v && s.MonsterPoisonShown;
        this.dataLoader.model.ShowMonsterSleep = v && s.MonsterSleepShown;
        this.dataLoader.model.ShowMonsterPara = v && s.MonsterParaShown;
        this.dataLoader.model.ShowMonsterBlast = v && s.MonsterBlastShown;
        this.dataLoader.model.ShowMonsterStun = v && s.MonsterStunShown;

        this.dataLoader.model.ShowMonster1HPBar = v && s.Monster1HealthBarShown;
        this.dataLoader.model.ShowMonster2HPBar = v && s.Monster2HealthBarShown;
        this.dataLoader.model.ShowMonster3HPBar = v && s.Monster3HealthBarShown;
        this.dataLoader.model.ShowMonster4HPBar = v && s.Monster4HealthBarShown;

        this.dataLoader.model.ShowMonsterPartHP = v && s.PartThresholdShown;
        this.dataLoader.model.ShowMonster1Icon = v && s.Monster1IconShown;
    }

    /// <summary>
    /// Hides the player information when not in quest.
    /// </summary>
    private void HidePlayerInfoWhenNotInQuest()
    {
        Settings s = (Settings)Application.Current.FindResource("Settings");
        bool v = IsGameFocused(s) &&
            (s.AlwaysShowPlayerInfo || this.dataLoader.model.Configuring || this.dataLoader.model.QuestID() != 0);
        SetPlayerStatsVisibility(v, s);
    }

    private void SetPlayerStatsVisibility(bool v, Settings s)
    {
        // DL.m.?.Visibility = v && s.?.IsChecked
        this.dataLoader.model.ShowTimerInfo = v && s.TimerInfoShown;
        this.dataLoader.model.ShowHitCountInfo = v && s.HitCountShown;
        this.dataLoader.model.ShowPlayerAtkInfo = v && s.PlayerAtkShown;
        this.dataLoader.model.ShowPlayerHitsTakenBlockedInfo = v && s.TotalHitsTakenBlockedShown;
        this.dataLoader.model.ShowSharpness = v && s.EnableSharpness;
        this.dataLoader.model.ShowSessionTimeInfo = v && s.SessionTimeShown;

        this.dataLoader.model.ShowMap = v && s.EnableMap;
        this.dataLoader.model.ShowFrameCounter = v && s.FrameCounterShown;
        this.dataLoader.model.ShowPlayerAttackGraph = v && s.PlayerAttackGraphShown;
        this.dataLoader.model.ShowPlayerDPSGraph = v && s.PlayerDPSGraphShown;
        this.dataLoader.model.ShowPlayerAPMGraph = v && s.PlayerAPMGraphShown;
        this.dataLoader.model.ShowPlayerHitsPerSecondGraph = v && s.PlayerHitsPerSecondGraphShown;

        this.dataLoader.model.ShowDamagePerSecond = v && s.DamagePerSecondShown;

        this.dataLoader.model.ShowKBMLayout = v && s.KBMLayoutShown;
        this.dataLoader.model.ShowGamepadLayout = v && s.GamepadShown;
        this.dataLoader.model.ShowAPM = v && s.ActionsPerMinuteShown;
        this.dataLoader.model.ShowOverlayModeWatermark = v && s.OverlayModeWatermarkShown;
        this.dataLoader.model.ShowQuestID = v && s.QuestIDShown;

        this.dataLoader.model.ShowPersonalBestInfo = v && s.PersonalBestShown;
        this.dataLoader.model.ShowQuestAttemptsInfo = v && s.QuestAttemptsShown;
        this.dataLoader.model.ShowPersonalBestTimePercentInfo = v && s.PersonalBestTimePercentShown;
        this.dataLoader.model.ShowPersonalBestAttemptsInfo = v && s.PersonalBestAttemptsShown;
    }

    private double? XOffset;

    private double? YOffset;

    private FrameworkElement? MovingObject;

    private void MainGrid_DragOver(object sender, DragEventArgs e)
    {
        if (MovingObject == null)
        {
            return;
        }

        Point pos = e.GetPosition(this);
        if (XOffset == null || YOffset == null)
        {
            return;
        }

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

            // case "OverlayModeWatermark":
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
        {
            return;
        }

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
        {
            MovingObject.IsHitTestVisible = true;
        }

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
        {
            return;
        }

        MovingObject = (FrameworkElement)sender;
        Point pos = e.GetPosition(this);
        XOffset = pos.X - Canvas.GetLeft(MovingObject);
        YOffset = pos.Y - Canvas.GetTop(MovingObject);
        MovingObject.IsHitTestVisible = false;
    }

    private void ReloadButton_Click(object sender, RoutedEventArgs e)
    {
        ApplicationService.HandleRestart();
    }

    ConfigWindow? configWindow { get; set; }

    private void OpenConfigButton_Click(object sender, RoutedEventArgs e)
    {
        if (configWindow == null || !configWindow.IsLoaded)
        {
            configWindow = new(this);
        }

        configWindow.Show();
        this.dataLoader.model.Configuring = true;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        ApplicationService.HandleShutdown();
    }

    // https://stackoverflow.com/questions/4773632/how-do-i-restart-a-wpf-application
    private void ReloadButton_Key()
    {
        ApplicationService.HandleRestart();
    }

    private void OpenConfigButton_Key()
    {
        if (IsDragConfigure)
        {
            return;
        }

        if (this.dataLoader.model.isInLauncherBool)
        {
            System.Windows.MessageBox.Show("Using the configuration menu outside of the game might cause slow performance", Messages.WarningTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            LoggerInstance.Info(CultureInfo.InvariantCulture, "Detected game launcher while using configuration menu");
        }

        if (configWindow == null || !configWindow.IsLoaded)
        {
            configWindow = new(this);
        }

        try
        {
            configWindow.Show();// TODO: memory error?
            this.dataLoader.model.Configuring = true;
        }
        catch (Exception ex)
        {
            LoggerInstance.Error(CultureInfo.InvariantCulture, "Could not show configuration window", ex);
        }

        try
        {
            dataLoader.CheckForExternalProcesses();
            dataLoader.CheckForIllegalModifications();
        }
        catch (Exception ex)
        {
            LoggingService.WriteCrashLog(ex);
        }
    }

    private void CloseButton_Key()
    {
        ApplicationService.HandleShutdown();
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
        {
            configWindow.Visibility = Visibility.Hidden;
        }

        ToggleClickThrough();
        ToggleOverlayBorders();
    }

    // TODO: use a dictionary for looping instead
    private void ToggleOverlayBorders()
    {
        var thickness = new System.Windows.Thickness(0);

        if (IsDragConfigure)
        {
            thickness = new System.Windows.Thickness(2);
        }

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
        {
            configWindow.Visibility = Visibility.Visible;
        }

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

    private bool calculatedPersonalBest;

    private bool calculatedQuestAttempts;

    private async Task UpdateQuestAttempts()
    {
        string category = OverlayModeWatermarkTextBlock.Text;
        int weaponType = this.dataLoader.model.WeaponType();
        long questID = this.dataLoader.model.QuestID();

        int attempts = await DatabaseManagerInstance.UpsertQuestAttemptsAsync(questID, weaponType, category);
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        string completions = string.Empty;
        if (s.EnableQuestCompletionsCounter)
        {
            completions = await DatabaseManagerInstance.GetQuestCompletionsAsync(questID, category, weaponType) + "/";
        }

        questAttemptsTextBlock.Text = $"{completions}{attempts}";
    }

    private async Task UpdatePersonalBestAttempts()
    {
        string category = OverlayModeWatermarkTextBlock.Text;
        int weaponType = this.dataLoader.model.WeaponType();
        long questID = this.dataLoader.model.QuestID();

        int attempts = await DatabaseManagerInstance.UpsertPersonalBestAttemptsAsync(questID, weaponType, category);
        personalBestAttemptsTextBlock.Text = attempts.ToString(CultureInfo.InvariantCulture);
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
                score = this.dataLoader.model.UrukiPachinkoScore() + this.dataLoader.model.UrukiPachinkoBonusScore();
                break;
            case 467: // Nyanrendo
                score = this.dataLoader.model.NyanrendoScore();
                break;
            case 469: // Dokkan Battle Cats
                score = this.dataLoader.model.DokkanBattleCatsScore();
                break;
            case 466: // Guuku Scoop
                score = this.dataLoader.model.GuukuScoopScore();
                break;
            case 468: // Panic Honey
                score = this.dataLoader.model.PanicHoneyScore();
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
        if (this.dataLoader.model.QuestID() != 0 || !(this.dataLoader.model.AreaID() == 462 || MezFesMinigameCollection.ID.ContainsKey(this.dataLoader.model.AreaID())))
        {
            return;
        }

        int areaID = this.dataLoader.model.AreaID();

        // Check if player is in a minigame area
        if (MezFesMinigameCollection.ID.ContainsKey(areaID))
        {
            // Check if the player has entered a new minigame area
            if (areaID != this.dataLoader.model.previousMezFesArea)
            {
                this.dataLoader.model.previousMezFesArea = areaID;
                this.dataLoader.model.previousMezFesScore = 0;
            }

            // Read player score from corresponding memory address based on current area ID
            int score = GetMezFesMinigameScore(areaID);

            // Update current score with new score if it's greater and doesn't surpass the UI limit
            if (score > this.dataLoader.model.previousMezFesScore && score <= 999999)
            {
                this.dataLoader.model.previousMezFesScore = score;
            }
        }

        // Check if the player has exited a minigame area and the score is 0
        else if (this.dataLoader.model.previousMezFesArea != -1 && areaID == 462)
        {
            // Save current score and minigame area ID to database
            DatabaseManagerInstance.InsertMezFesMinigameScore(dataLoader, this.dataLoader.model.previousMezFesArea, this.dataLoader.model.previousMezFesScore);

            // Reset previousMezFesArea and previousMezFesScore
            this.dataLoader.model.previousMezFesArea = -1;
            this.dataLoader.model.previousMezFesScore = 0;
        }
    }

    // TODO: optimization
    private async Task CheckQuestStateForDatabaseLogging()
    {
        Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

        // Check if in quest and timer is NOT frozen
        if (this.dataLoader.model.QuestID() != 0 && this.dataLoader.model.TimeInt() != this.dataLoader.model.TimeDefInt() && this.dataLoader.model.QuestState() == 0 && this.dataLoader.model.previousTimeInt != this.dataLoader.model.TimeInt())
        {
            this.dataLoader.model.previousTimeInt = this.dataLoader.model.TimeInt();
            this.dataLoader.model.TotalHitsTakenBlockedPerSecond = this.dataLoader.model.CalculateTotalHitsTakenBlockedPerSecond();
            this.dataLoader.model.HitsPerSecond = this.dataLoader.model.CalculateHitsPerSecond();
            this.dataLoader.model.DPS = this.dataLoader.model.CalculateDPS();
            this.dataLoader.model.APM = this.dataLoader.model.CalculateAPM();
            this.dataLoader.model.InsertQuestInfoIntoDictionaries();

            // TODO: test on dure/etc
            if (!calculatedPersonalBest && this.dataLoader.model.TimeDefInt() > this.dataLoader.model.TimeInt() && int.Parse(this.dataLoader.model.ATK, CultureInfo.InvariantCulture) > 0)
            {
                calculatedPersonalBest = true;
                personalBestTextBlock.Text = await DatabaseManagerInstance.GetPersonalBestAsync(this.dataLoader.model.QuestID(), this.dataLoader.model.WeaponType(), OverlayModeWatermarkTextBlock.Text, this.dataLoader.model.QuestTimeMode, dataLoader);
                this.dataLoader.model.PersonalBestLoaded = personalBestTextBlock.Text;
            }

            if (!calculatedQuestAttempts
                && this.dataLoader.model.TimeDefInt() > this.dataLoader.model.TimeInt()
                && int.Parse(this.dataLoader.model.ATK, CultureInfo.InvariantCulture) > 0
                && this.dataLoader.model.TimeDefInt() - this.dataLoader.model.TimeInt() >= 30)
            {
                calculatedQuestAttempts = true;
                await UpdateQuestAttempts();
                await UpdatePersonalBestAttempts();
            }
        }

        if ((this.dataLoader.model.QuestState() == 0 && this.dataLoader.model.QuestID() == 0))
        {
            this.dataLoader.model.questCleared = false;
            this.dataLoader.model.questRewardsGiven = false;
            this.dataLoader.model.clearQuestInfoDictionaries();
            this.dataLoader.model.clearGraphCollections();
            this.dataLoader.model.resetQuestInfoVariables();
            this.dataLoader.model.previousRoadFloor = 0;
            personalBestTextBlock.Text = Messages.TimerNotLoaded;
            calculatedPersonalBest = false;
            calculatedQuestAttempts = false;
            return;
        }
        else if (!this.dataLoader.model.loadedItemsAtQuestStart && this.dataLoader.model.QuestState() == 0 && this.dataLoader.model.QuestID() != 0)
        {
            this.dataLoader.model.loadedItemsAtQuestStart = true;
            this.dataLoader.model.PouchItem1IDAtQuestStart = this.dataLoader.model.PouchItem1ID();
            this.dataLoader.model.PouchItem2IDAtQuestStart = this.dataLoader.model.PouchItem2ID();
            this.dataLoader.model.PouchItem3IDAtQuestStart = this.dataLoader.model.PouchItem3ID();
            this.dataLoader.model.PouchItem4IDAtQuestStart = this.dataLoader.model.PouchItem4ID();
            this.dataLoader.model.PouchItem5IDAtQuestStart = this.dataLoader.model.PouchItem5ID();
            this.dataLoader.model.PouchItem6IDAtQuestStart = this.dataLoader.model.PouchItem6ID();
            this.dataLoader.model.PouchItem7IDAtQuestStart = this.dataLoader.model.PouchItem7ID();
            this.dataLoader.model.PouchItem8IDAtQuestStart = this.dataLoader.model.PouchItem8ID();
            this.dataLoader.model.PouchItem9IDAtQuestStart = this.dataLoader.model.PouchItem9ID();
            this.dataLoader.model.PouchItem10IDAtQuestStart = this.dataLoader.model.PouchItem10ID();
            this.dataLoader.model.PouchItem11IDAtQuestStart = this.dataLoader.model.PouchItem11ID();
            this.dataLoader.model.PouchItem12IDAtQuestStart = this.dataLoader.model.PouchItem12ID();
            this.dataLoader.model.PouchItem13IDAtQuestStart = this.dataLoader.model.PouchItem13ID();
            this.dataLoader.model.PouchItem14IDAtQuestStart = this.dataLoader.model.PouchItem14ID();
            this.dataLoader.model.PouchItem15IDAtQuestStart = this.dataLoader.model.PouchItem15ID();
            this.dataLoader.model.PouchItem16IDAtQuestStart = this.dataLoader.model.PouchItem16ID();
            this.dataLoader.model.PouchItem17IDAtQuestStart = this.dataLoader.model.PouchItem17ID();
            this.dataLoader.model.PouchItem18IDAtQuestStart = this.dataLoader.model.PouchItem18ID();
            this.dataLoader.model.PouchItem19IDAtQuestStart = this.dataLoader.model.PouchItem19ID();
            this.dataLoader.model.PouchItem20IDAtQuestStart = this.dataLoader.model.PouchItem20ID();
            this.dataLoader.model.PouchItem1QuantityAtQuestStart = this.dataLoader.model.PouchItem1Qty();
            this.dataLoader.model.PouchItem2QuantityAtQuestStart = this.dataLoader.model.PouchItem2Qty();
            this.dataLoader.model.PouchItem3QuantityAtQuestStart = this.dataLoader.model.PouchItem3Qty();
            this.dataLoader.model.PouchItem4QuantityAtQuestStart = this.dataLoader.model.PouchItem4Qty();
            this.dataLoader.model.PouchItem5QuantityAtQuestStart = this.dataLoader.model.PouchItem5Qty();
            this.dataLoader.model.PouchItem6QuantityAtQuestStart = this.dataLoader.model.PouchItem6Qty();
            this.dataLoader.model.PouchItem7QuantityAtQuestStart = this.dataLoader.model.PouchItem7Qty();
            this.dataLoader.model.PouchItem8QuantityAtQuestStart = this.dataLoader.model.PouchItem8Qty();
            this.dataLoader.model.PouchItem9QuantityAtQuestStart = this.dataLoader.model.PouchItem9Qty();
            this.dataLoader.model.PouchItem10QuantityAtQuestStart = this.dataLoader.model.PouchItem10Qty();
            this.dataLoader.model.PouchItem11QuantityAtQuestStart = this.dataLoader.model.PouchItem11Qty();
            this.dataLoader.model.PouchItem12QuantityAtQuestStart = this.dataLoader.model.PouchItem12Qty();
            this.dataLoader.model.PouchItem13QuantityAtQuestStart = this.dataLoader.model.PouchItem13Qty();
            this.dataLoader.model.PouchItem14QuantityAtQuestStart = this.dataLoader.model.PouchItem14Qty();
            this.dataLoader.model.PouchItem15QuantityAtQuestStart = this.dataLoader.model.PouchItem15Qty();
            this.dataLoader.model.PouchItem16QuantityAtQuestStart = this.dataLoader.model.PouchItem16Qty();
            this.dataLoader.model.PouchItem17QuantityAtQuestStart = this.dataLoader.model.PouchItem17Qty();
            this.dataLoader.model.PouchItem18QuantityAtQuestStart = this.dataLoader.model.PouchItem18Qty();
            this.dataLoader.model.PouchItem19QuantityAtQuestStart = this.dataLoader.model.PouchItem19Qty();
            this.dataLoader.model.PouchItem20QuantityAtQuestStart = this.dataLoader.model.PouchItem20Qty();

            this.dataLoader.model.AmmoPouchItem1IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem2IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem3IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem4IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem5IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem6IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem7IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem8IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem9IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem10IDAtQuestStart = this.dataLoader.model.AmmoPouchItem1ID();
            this.dataLoader.model.AmmoPouchItem1QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();
            this.dataLoader.model.AmmoPouchItem2QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();
            this.dataLoader.model.AmmoPouchItem3QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();
            this.dataLoader.model.AmmoPouchItem4QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();
            this.dataLoader.model.AmmoPouchItem5QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();
            this.dataLoader.model.AmmoPouchItem6QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();
            this.dataLoader.model.AmmoPouchItem7QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();
            this.dataLoader.model.AmmoPouchItem8QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();
            this.dataLoader.model.AmmoPouchItem9QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();
            this.dataLoader.model.AmmoPouchItem10QuantityAtQuestStart = this.dataLoader.model.AmmoPouchItem1Qty();

            this.dataLoader.model.PartnyaBagItem1IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem2IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem3IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem4IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem5IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem6IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem7IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem8IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem9IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem10IDAtQuestStart = this.dataLoader.model.PartnyaBagItem1ID();
            this.dataLoader.model.PartnyaBagItem1QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();
            this.dataLoader.model.PartnyaBagItem2QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();
            this.dataLoader.model.PartnyaBagItem3QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();
            this.dataLoader.model.PartnyaBagItem4QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();
            this.dataLoader.model.PartnyaBagItem5QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();
            this.dataLoader.model.PartnyaBagItem6QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();
            this.dataLoader.model.PartnyaBagItem7QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();
            this.dataLoader.model.PartnyaBagItem8QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();
            this.dataLoader.model.PartnyaBagItem9QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();
            this.dataLoader.model.PartnyaBagItem10QuantityAtQuestStart = this.dataLoader.model.PartnyaBagItem1Qty();

        }

        if (this.dataLoader.model.QuestState() == 0)
        {
            return;
        }

        // check if quest clear 
        if (this.dataLoader.model.QuestState() == 1 && !this.dataLoader.model.questCleared)
        {
            // TODO test on dure/road/etc
            // If this code is ever reached, it is not known if the cause is from the overlay interacting with the server,
            // the server itself, or just the overlay itself.
            // The overlay does NOT write to memory addresses.
            if (this.dataLoader.model.TimeDefInt() - this.dataLoader.model.TimeInt() == 0)
            {
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Illegal quest completion time [ID {0}]", this.dataLoader.model.QuestID());
                ApplicationService.HandleGameShutdown();
                LoggingService.WriteCrashLog(new Exception($"Illegal quest completion time [ID {this.dataLoader.model.QuestID()}]"));
            }

            this.dataLoader.model.questCleared = true;
            this.dataLoader.model.loadedItemsAtQuestStart = false;
            if (s.EnableQuestLogging)
            {
                DatabaseManagerInstance.InsertQuestData(dataLoader, (int)DatabaseManagerInstance.GetQuestAttempts((long)this.dataLoader.model.QuestID(), this.dataLoader.model.WeaponType(), OverlayModeWatermarkTextBlock.Text));
            }
        }

        // check if rewards given
        if (this.dataLoader.model.QuestState() == 129 && !this.dataLoader.model.questRewardsGiven)
        {
            this.dataLoader.model.questRewardsGiven = true;

            // TODO: add logging check requirement in case the user needs the hash sets.
            // We await since we are dealing with database
            await AchievementManagerInstance.CheckForAchievementsAsync(MainWindowSnackBar, dataLoader, DatabaseManagerInstance, s);
        }
    }

    // TODO fix alt tab issues?
    private IKeyboardMouseEvents m_GlobalHook;

    /// <summary>
    /// Subscribes this instance for player input.
    /// </summary>
    public void Subscribe()
    {
        // Note: for the application hook, use the Hook.AppEvents() instead
        m_GlobalHook = Hook.GlobalEvents();

        m_GlobalHook.MouseDownExt += this.GlobalHookMouseDownExt;
        m_GlobalHook.MouseUpExt += this.GlobalHookMouseUpExt;
        m_GlobalHook.KeyPress += this.GlobalHookKeyPress;
        m_GlobalHook.KeyDown += this.GlobalHookKeyDown;
        m_GlobalHook.KeyUp += this.GlobalHookKeyUp;

        // Register the event handler for button presses
        // TODO: do i really need this kind of interface?
        // m_GlobalHook.KeyDown += HandleHorizontalInput;
    }

    private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
    {
        // goodbye world
    }

    private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
    {
        if (_mouseImages.TryGetValue(e.Button, out Image? image))
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.dataLoader.model.mouseInputDictionary.ContainsKey(this.dataLoader.model.TimeInt()) && this.dataLoader.model.QuestID() != 0 && this.dataLoader.model.TimeInt() != this.dataLoader.model.TimeDefInt() && this.dataLoader.model.QuestState() == 0 && this.dataLoader.model.previousTimeInt != this.dataLoader.model.TimeInt() && image.Opacity == unpressedInputOpacity)
            {
                try
                {
                    this.dataLoader.model.mouseInputDictionary.Add(this.dataLoader.model.TimeInt(), e.Button.ToString());
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into mouseInputDictionary");
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                image.Opacity = pressedInputOpacity;
            }));
        }

        // uncommenting the following line will suppress the middle mouse button click
        // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
    }

    private void GlobalHookMouseUpExt(object sender, MouseEventExtArgs e)
    {
        if (_mouseImages.TryGetValue(e.Button, out Image? image))
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                image.Opacity = unpressedInputOpacity;
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

        // m_GlobalHook.KeyDown -= HandleHorizontalInput;

        // It is recommened to dispose it
        m_GlobalHook.Dispose();
    }

    private void GlobalHookKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        if (_keyImages.TryGetValue(e.KeyCode, out Image? image))
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.dataLoader.model.keystrokesDictionary.ContainsKey(this.dataLoader.model.TimeInt()) && this.dataLoader.model.QuestID() != 0 && this.dataLoader.model.TimeInt() != this.dataLoader.model.TimeDefInt() && this.dataLoader.model.QuestState() == 0 && this.dataLoader.model.previousTimeInt != this.dataLoader.model.TimeInt() && image.Opacity == unpressedInputOpacity)
            {
                try
                {
                    this.dataLoader.model.keystrokesDictionary.Add(this.dataLoader.model.TimeInt(), e.KeyCode.ToString());
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into keystrokesDictionary");
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                image.Opacity = pressedInputOpacity;
            }));
        }
    }

    private void GlobalHookKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        if (_keyImages.TryGetValue(e.KeyCode, out Image? image))
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                image.Opacity = unpressedInputOpacity;
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
            LoggerInstance.Debug("Gamepad not found");
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

    double pressedInputOpacity { get; set; } = 0.5;

    double unpressedInputOpacity { get; set; } = 0.2;

    float triggerActivationThreshold { get; set; } = 0.5f;

    float joystickThreshold { get; set; } = 0.5f;

    private void Gamepad_RightTriggerReleased(object? sender, EventArgs e)
    {
        if (_gamepadTriggersImages.TryGetValue(gamepad.RightTrigger, out Image? image))
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                image.Opacity = unpressedInputOpacity;
            }));
        }
    }

    private void Gamepad_LeftTriggerReleased(object? sender, EventArgs e)
    {
        if (_gamepadTriggersImages.TryGetValue(gamepad.LeftTrigger, out Image? image))
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                image.Opacity = unpressedInputOpacity;
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
        else if (_gamepadImages.TryGetValue(e.Button, out Image? image))
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                image.Opacity = unpressedInputOpacity;
            }));
        }
    }

    private void Gamepad_RightTriggerPressed(object? sender, EventArgs e)
    {
        if (_gamepadTriggersImages.TryGetValue(gamepad.RightTrigger, out Image? image))
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.dataLoader.model.gamepadInputDictionary.ContainsKey(this.dataLoader.model.TimeInt()) && this.dataLoader.model.QuestID() != 0 && this.dataLoader.model.TimeInt() != this.dataLoader.model.TimeDefInt() && this.dataLoader.model.QuestState() == 0 && this.dataLoader.model.previousTimeInt != this.dataLoader.model.TimeInt() && image.Opacity == unpressedInputOpacity)
            {
                try
                {
                    this.dataLoader.model.gamepadInputDictionary.Add(this.dataLoader.model.TimeInt(), gamepad.RightTrigger.ToString());
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                image.Opacity = pressedInputOpacity;
            }));
        }
    }

    private void Gamepad_LeftTriggerPressed(object? sender, EventArgs e)
    {
        if (_gamepadTriggersImages.TryGetValue(gamepad.LeftTrigger, out Image? image))
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.dataLoader.model.gamepadInputDictionary.ContainsKey(this.dataLoader.model.TimeInt()) && this.dataLoader.model.QuestID() != 0 && this.dataLoader.model.TimeInt() != this.dataLoader.model.TimeDefInt() && this.dataLoader.model.QuestState() == 0 && this.dataLoader.model.previousTimeInt != this.dataLoader.model.TimeInt() && image.Opacity == unpressedInputOpacity)
            {
                try
                {
                    this.dataLoader.model.gamepadInputDictionary.Add(this.dataLoader.model.TimeInt(), gamepad.LeftTrigger.ToString());
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                image.Opacity = pressedInputOpacity;
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
        string imagePath = JoystickImageCollection.GetImage(direction);

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
        string imagePath = JoystickImageCollection.GetImage(direction);

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

        if (s.EnableInputLogging && !this.dataLoader.model.gamepadInputDictionary.ContainsKey(this.dataLoader.model.TimeInt()) && this.dataLoader.model.QuestID() != 0 && this.dataLoader.model.TimeInt() != this.dataLoader.model.TimeDefInt() && this.dataLoader.model.QuestState() == 0 && this.dataLoader.model.previousTimeInt != this.dataLoader.model.TimeInt() && DPad.Opacity == unpressedInputOpacity)
        {
            try
            {
                this.dataLoader.model.gamepadInputDictionary.Add(this.dataLoader.model.TimeInt(), button.ToString());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary (Gamepad_DPadPressed)");
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

        if (_gamepadImages.TryGetValue(e.Button, out Image? image))
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.dataLoader.model.gamepadInputDictionary.ContainsKey(this.dataLoader.model.TimeInt()) && this.dataLoader.model.QuestID() != 0 && this.dataLoader.model.TimeInt() != this.dataLoader.model.TimeDefInt() && this.dataLoader.model.QuestState() == 0 && this.dataLoader.model.previousTimeInt != this.dataLoader.model.TimeInt() && image.Opacity == unpressedInputOpacity)
            {
                try
                {
                    this.dataLoader.model.gamepadInputDictionary.Add(this.dataLoader.model.TimeInt(), e.Button.ToString());
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary");
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
                    image.Opacity = pressedInputOpacity;
                }));
            }
        }
    }

    private void UpdateRightStickImage(double opacity)
    {
        // Get the image path based on the direction
        string imagePath = JoystickImageCollection.GetImage(Direction.None);

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
        string imagePath = JoystickImageCollection.GetImage(Direction.None);

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
        string imagePath = DPadImageCollection.GetImage(direction);

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

    private Process? mhfProcess { get; set; }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (App.isFirstRun)
        {
            OnboardEndUser();
        }

        if (dataLoader.loadedOutsideMezeporta)
        {
            MainWindowSnackBar.ShowAsync(Messages.WarningTitle, "It is not recommended to load the overlay outside of Mezeporta", new SymbolIcon(SymbolRegular.Warning28), ControlAppearance.Caution);
        }

        DatabaseManagerInstance.LoadDatabaseDataIntoHashSets(SaveIconGrid, dataLoader);
        AchievementManagerInstance.LoadPlayerAchievements();

        mhfProcess = System.Diagnostics.Process.GetProcessesByName("mhf").First();

        if (mhfProcess == null)
        {
            LoggingService.WriteCrashLog(new Exception("Target process not found"));
        }
    }

    private void OnboardEndUser()
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        var result = System.Windows.MessageBox.Show("Would you like to quickly set the settings?", Messages.InfoTitle, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Information);

        if (result == System.Windows.MessageBoxResult.Yes)
        {
            var settingsForm = new SettingsForm();
            bool? settingsFormResult = settingsForm.ShowDialog();
            if (settingsFormResult == null)
            {
                return;
            }

            var resultSelected = string.Empty;
            if (settingsFormResult == true)
            {
                // User clicked the "Apply" button and made some selections
                // Retrieve the selected settings and perform the necessary actions
                if (settingsForm.IsDefaultSettingsSelected)
                {
                    resultSelected = "Default";
                    s.Reset();
                }
                else if (settingsForm.IsMonsterHpOnlySelected)
                {
                    resultSelected = "HP Only";
                    OverlaySettingsManagerInstance.SetConfigurationPreset(s, ConfigurationPresetConverter.Convert("hp only"));
                    switch (settingsForm.MonsterHPModeSelected)
                    {
                        default:
                            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Invalid Monster HP Mode option");
                            break;
                        case "Automatic":
                            s.EnableEHPNumbers = true;
                            s.EnableMonsterEHPDisplayCorrector = true;
                            s.MonsterEHPDisplayCorrectorDefrateMaximumThreshold = 1;
                            s.MonsterEHPDisplayCorrectorDefrateMinimumThreshold = 0.001M;
                            break;
                        case "Effective HP":
                            s.EnableEHPNumbers = true;
                            s.EnableMonsterEHPDisplayCorrector = false;
                            break;
                        case "True HP":
                            s.EnableEHPNumbers = false;
                            break;
                    }
                }
                else if (settingsForm.IsSpeedrunSelected)
                {
                    resultSelected = "Speedrun";
                    OverlaySettingsManagerInstance.SetConfigurationPreset(s, ConfigurationPresetConverter.Convert("speedrun"));
                }
                else if (settingsForm.IsEverythingSelected)
                {
                    resultSelected = "All";
                    OverlaySettingsManagerInstance.SetConfigurationPreset(s, ConfigurationPresetConverter.Convert("all"));
                }
                else
                {
                    return;
                }

                s.Save();
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Onboarded end-user. Result selected: {0}", resultSelected);
                System.Windows.MessageBox.Show("Settings set. Happy hunting!", Messages.InfoTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else if (settingsFormResult == false)
            {
                // User closed the window without making any selections
                // Handle this scenario as needed (e.g., use default settings, display a message, etc.)
                return;
            }
        }
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
        {
            victoryMediaSound = mediaElement;
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
/// </TODO>
