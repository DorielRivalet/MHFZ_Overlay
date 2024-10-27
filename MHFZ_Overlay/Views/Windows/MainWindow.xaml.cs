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
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.Services.Converter;
using MHFZ_Overlay.Services.Hotkey;
using MHFZ_Overlay.ViewModels.Windows;
using MHFZ_Overlay.Views.CustomControls;
using Microsoft.Extensions.DependencyModel;
using NLog;
using Octokit;
using SkiaSharp;
using Wpf.Ui;
using Wpf.Ui.Controls;
using XInputium;
using XInputium.XInput;
using Application = System.Windows.Application;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using Direction = Models.Structures.Direction;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using Image = System.Windows.Controls.Image;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxResult = System.Windows.MessageBoxResult;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using NotifyIcon = Wpf.Ui.Tray.Controls.NotifyIcon;
using Point = System.Windows.Point;
using Window = System.Windows.Window;

/// <summary>
/// Interaction logic for MainWindow.xaml. The main window of the application. It has a DataLoader object, which is used to load the data into the window. It also has several controls, including a custom progress bar (CustomProgressBar), which is bound to the properties of the AddressModel object. The MainWindow also initializes several global hotkeys and registers the Tick event of a DispatcherTimer.
/// </summary>
public partial class MainWindow : Window
{
    public const int WSEXTRANSPARENT = 0x00000020;

    private static readonly Logger LoggerInstance = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Gets or sets dataLoader.
    /// </summary>
    public DataLoader DataLoader { get; set; }

    private static readonly DatabaseService DatabaseManagerInstance = DatabaseService.GetInstance();

    private static readonly AchievementService AchievementServiceInstance = AchievementService.GetInstance();

    private static readonly OverlaySettingsService OverlaySettingsManagerInstance = OverlaySettingsService.GetInstance();

    private static readonly DiscordService DiscordManagerInstance = DiscordService.GetInstance();

    private readonly Mem m = new ();

    private int originalStyle;

    public TimeSpan MainWindowSnackbarTimeOut { get; set; } = TimeSpan.FromSeconds(5);

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
        var hwnd = new WindowInteropHelper(this).Handle;

        // Change the extended window style to include WS_EX_TRANSPARENT
        var extendedStyle = GetWindowLong(hwnd, GWLEXSTYLE);

        if (this.originalStyle == 0)
        {
            this.originalStyle = extendedStyle;
        }

        SetWindowLong(hwnd, GWLEXSTYLE, extendedStyle | WSEXTRANSPARENT);
        base.OnSourceInitialized(e);
    }

    public static NotifyIcon? _mainWindowNotifyIcon { get; set; }

    private void CreateSystemTrayIcon()
    {
        _mainWindowNotifyIcon = this.MainWindowNotifyIcon;
    }

    // Add method to open hotkey settings
    private void OpenHotkeySettings()
    {
        var settingsWindow = new HotkeySettingsWindow(_hotkeyManager);
        settingsWindow.Owner = this; // Make it a child window of MainWindow
        settingsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        settingsWindow.ShowDialog();
    }

    // Ensure proper cleanup. TODO test
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _hotkeyManager?.Dispose();
    }

    private void NotifyIcon_Click(object sender, RoutedEventArgs e) => this.OpenConfigButton_Key();

    private void OptionSettings_Click(object sender, RoutedEventArgs e) => this.OpenConfigButton_Key();

    private void OptionHotkeys_Click(object sender, RoutedEventArgs e) => this.OpenHotkeySettings();

    private void OptionHelp_Click(object sender, RoutedEventArgs e) => OpenLink("https://github.com/DorielRivalet/mhfz-overlay/blob/main/FAQ.md");

    private void OptionDocumentation_Click(object sender, RoutedEventArgs e) => OpenLink("https://github.com/DorielRivalet/mhfz-overlay/tree/main/docs");

    private void OptionReportBug_Click(object sender, RoutedEventArgs e) => OpenLink("https://github.com/DorielRivalet/mhfz-overlay/issues/new?assignees=DorielRivalet&labels=bug&projects=&template=BUG-REPORT.yml&title=%5BBUG%5D+-+title");

    private void OptionRequestFeature_Click(object sender, RoutedEventArgs e) => OpenLink("https://github.com/DorielRivalet/mhfz-overlay/issues/new?assignees=DorielRivalet&labels=question%2Cenhancement&projects=&template=FEATURE-REQUEST.yml&title=%5BREQUEST%5D+-+title");

    private void OptionSendFeedback_Click(object sender, RoutedEventArgs e) => OpenLink("https://forms.gle/hrAVWMcYS5HEo1v7A");

    private void OptionChangelog_Click(object sender, RoutedEventArgs e) => OpenLink("https://github.com/DorielRivalet/mhfz-overlay/blob/main/CHANGELOG.md");

    private void OptionOverlayFolder_Click(object sender, RoutedEventArgs e) => FileService.OpenApplicationFolder(this.MainWindowSnackBarPresenter, (Style)this.FindResource("CatppuccinMochaSnackBar"), this.MainWindowSnackbarTimeOut);

    private void OptionSettingsFolder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var settingsFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            var settingsFileDirectoryName = Path.GetDirectoryName(settingsFile);
            if (!Directory.Exists(settingsFileDirectoryName))
            {
                LoggerInstance.Error(CultureInfo.InvariantCulture, "Could not open settings folder");
                var snackbar = new Snackbar(this.MainWindowSnackBarPresenter)
                {
                    Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
                    Title = Messages.ErrorTitle,
                    Content = "Could not open settings folder",
                    Icon = new SymbolIcon(SymbolRegular.ErrorCircle24),
                    Appearance = ControlAppearance.Danger,
                    Timeout = this.MainWindowSnackbarTimeOut,
                };
                snackbar.Show();
                return;
            }

            var settingsFolder = settingsFileDirectoryName;

            // Open file manager at the specified folder
            Process.Start(ApplicationPaths.ExplorerPath, settingsFolder);
        }
        catch (Exception ex)
        {
            LoggerInstance.Error(ex);
            var snackbar = new Snackbar(this.MainWindowSnackBarPresenter)
            {
                Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
                Title = Messages.ErrorTitle,
                Content = "Could not open settings folder",
                Icon = new SymbolIcon(SymbolRegular.ErrorCircle24),
                Appearance = ControlAppearance.Danger,
                Timeout = this.MainWindowSnackbarTimeOut,
            };
            snackbar.Show();
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
        try
        {
            Process.Start(ApplicationPaths.ExplorerPath, "\"" + DatabaseManagerInstance.GetDatabaseFolderPath() + "\"");
        }
        catch (Exception ex)
        {
            LoggerInstance.Error(ex);
        }
    }

    private void OptionRestart_Click(object sender, RoutedEventArgs e) => ReloadButton_Key();

    private void OptionExit_Click(object sender, RoutedEventArgs e) => CloseButton_Key();

    private void OptionAbout_Click(object sender, RoutedEventArgs e) => OpenLink("https://github.com/DorielRivalet/mhfz-overlay");

    public const int GWLEXSTYLE = -20;

    private static readonly Process CurrentProcess = Process.GetCurrentProcess();

    private readonly DateTime programStart;

    [DllImport("user32.dll")]
    public static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    // Declare a dictionary to map keys to images
    private readonly Dictionary<Keys, Image> keyImages = new ();

    private readonly Dictionary<MouseButtons, Image> mouseImages = new ();

    // TODO
    private readonly XGamepad gamepad;

    private readonly Dictionary<XInputButton, Image> gamepadImages = new ();

    private readonly Dictionary<XInputium.Trigger, Image> gamepadTriggersImages = new ();

    private readonly Dictionary<Joystick, Image> gamepadJoystickImages = new ();

    private readonly GitHubClient ghClient = new (new ProductHeaderValue("MHFZ_Overlay"));

    private double? xOffset { get; set; }

    private readonly HotkeyManager _hotkeyManager;


    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        // Create a Stopwatch instance
        var stopwatch = new Stopwatch();

        // Start the stopwatch
        stopwatch.Start();

        _hotkeyManager = new HotkeyManager();

        var splashScreen = new SplashScreen("../../Assets/Icons/png/loading.png");

        splashScreen.Show(false);
        this.DataLoader = new DataLoader();
        this.InitializeComponent();

        LoggerInstance.Info(CultureInfo.InvariantCulture, $"MainWindow initialized");
        LoggerInstance.Trace(new StackTrace().ToString());

        this.Left = 0;
        this.Top = 0;
        this.Topmost = true;
        DispatcherTimer timer = new ();
        var s = (Settings)Application.Current.TryFindResource("Settings");
        timer.Interval = new TimeSpan(0, 0, 0, 0, 1_000 / s.RefreshRate);

        // memory leak?
        timer.Tick += this.Timer_Tick;
        timer.Start();

        ViewModels.Windows.AddressModel.ValidateGameFolder();

        this.DataContext = this.DataLoader.Model;
        // Replace your existing hotkey registration with this:
        _hotkeyManager.RegisterHotkeys(
            () => this.OpenConfigButton_Key(),
            () => this.ReloadButton_Key(),
            () => this.CloseButton_Key()
        );

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
        this.programStart = DateTime.UtcNow;

        // Calculate the total time spent and update the TotalTimeSpent property
        this.DataLoader.Model.TotalTimeSpent = DatabaseManagerInstance.CalculateTotalTimeSpent();

        // TODO unsubscribe
        // TODO gamepad
        this.gamepad = new ();
        this.MapPlayerInputImages();
        this.Subscribe();
        this.gamepad.ButtonPressed += this.Gamepad_ButtonPressed;
        this.gamepad.LeftJoystickMove += this.Gamepad_LeftJoystickMove;
        this.gamepad.RightJoystickMove += this.Gamepad_RightJoystickMove;
        this.gamepad.LeftTrigger.ToDigitalButton(this.TriggerActivationThreshold).Pressed += this.Gamepad_LeftTriggerPressed;
        this.gamepad.RightTrigger.ToDigitalButton(this.TriggerActivationThreshold).Pressed += this.Gamepad_RightTriggerPressed;
        this.gamepad.ButtonReleased += this.Gamepad_ButtonReleased;
        this.gamepad.LeftTrigger.ToDigitalButton(this.TriggerActivationThreshold).Released += this.Gamepad_LeftTriggerReleased;
        this.gamepad.RightTrigger.ToDigitalButton(this.TriggerActivationThreshold).Released += this.Gamepad_RightTriggerReleased;

        DispatcherTimer timer1Frame = new ()
        {
            Interval = new TimeSpan(0, 0, 0, 0, 1_000 / (int)Numbers.FramesPerSecond),
        };
        timer1Frame.Tick += this.Timer1Frame_Tick;
        timer1Frame.Start();

        this.SetGraphSeries();
        GetDependencies();

        // The rendering tier corresponds to the high-order word of the Tier property.
        var renderingTier = RenderCapability.Tier >> 16;

        LoggerInstance.Info(CultureInfo.InvariantCulture, "Found rendering tier {0}", renderingTier);

        this.CreateSystemTrayIcon();

        DispatcherTimer timer1Second = new ()
        {
            Interval = new TimeSpan(0, 0, 1),
        };
        timer1Second.Tick += this.Timer1Second_Tick;

        // we run the 1 second timer tick once in the constructor
        try
        {
            this.HideMonsterInfoWhenNotInQuest();
            this.HidePlayerInfoWhenNotInQuest();
            this.DataLoader.CheckForExternalProcesses();
            this.DataLoader.CheckForIllegalModifications(this.DataLoader);
        }
        catch (Exception ex)
        {
            LoggingService.WriteCrashLog(ex);
        }

        timer1Second.Start();

        DispatcherTimer timer10Seconds = new ()
        {
            Interval = new TimeSpan(0, 0, 10),
        };
        timer10Seconds.Tick += this.Timer10Seconds_Tick;
        timer10Seconds.Start();

        this.DataLoader.Model.ShowSaveIcon = false;

        LoggerInstance.Info(CultureInfo.InvariantCulture, "Loaded MHF-Z Overlay {0}", Program.CurrentProgramVersion);

        // In your initialization or setup code
        ISnackbarService snackbarService = new SnackbarService();

        // Replace 'snackbarControl' with your actual snackbar control instance
        snackbarService.SetSnackbarPresenter(this.MainWindowSnackBarPresenter);

        splashScreen.Close(TimeSpan.FromSeconds(0.1));

        // Stop the stopwatch
        stopwatch.Stop();

        // Get the elapsed time in milliseconds
        var elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        // Print the elapsed time
        LoggerInstance.Debug($"MainWindow ctor Elapsed Time: {elapsedTimeMs} ms");
    }

    public bool IsDragConfigure { get; set; }

    private int CurNum { get; set; }

    /// <summary>
    /// Checks the state of the game.
    /// </summary>
    public void CheckGameState()
    {
        var processID = this.m.GetProcIdFromName("mhf");

        // https://stackoverflow.com/questions/12372534/how-to-get-a-process-window-class-name-from-c
        var pidToSearch = processID;

        // Init a condition indicating that you want to search by process id.
        var condition = new PropertyCondition(
            AutomationElementIdentifiers.ProcessIdProperty,
            pidToSearch);

        // Find the automation element matching the criteria
        // TODO what is this?
        var element = AutomationElement.RootElement.FindFirst(
            TreeScope.Children, condition);

        // get the classname
        if (element != null)
        {
            var className = element.Current.ClassName;

            if (className == "MHFLAUNCH")
            {
                LoggerInstance.Error(CultureInfo.InvariantCulture, "Detected game launcher");
                System.Windows.MessageBox.Show("Detected launcher, please start the overlay when fully loading into Mezeporta. Closing overlay.", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                this.DataLoader.Model.IsInLauncherBool = true;
                ApplicationService.HandleShutdown();
            }
            else
            {
                this.DataLoader.Model.IsInLauncherBool = false;
            }

            // https://stackoverflow.com/questions/51148/how-do-i-find-out-if-a-process-is-already-running-using-c
            // https://stackoverflow.com/questions/12273825/c-sharp-process-start-how-do-i-know-if-the-process-ended
            var mhfProcess = Process.GetProcessById(pidToSearch);

            mhfProcess.EnableRaisingEvents = true;
            mhfProcess.Exited += (sender, e) =>
            {
                this.DataLoader.Model.ClosedGame = true;
                var s = (Settings)Application.Current.TryFindResource("Settings");
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Detected closed game");
                System.Windows.MessageBox.Show("Detected closed game, closing overlay. Please start the overlay when fully loading into Mezeporta.", Messages.InfoTitle, MessageBoxButton.OK, MessageBoxImage.Information);

                // https://stackoverflow.com/a/9050477/18859245
                ApplicationService.HandleShutdown();
            };
        }
    }

    // TODO: refactor to somewhere else

    /// <summary>
    /// Opens the link. https://stackoverflow.com/a/60221582/18859245.
    /// </summary>
    /// <param name="destinationurl">The destinationurl.</param>
    private static void OpenLink(string destinationurl)
    {
        var sInfo = new ProcessStartInfo(destinationurl)
        {
            UseShellExecute = true,
        };
        Process.Start(sInfo);
    }

    private static void GetDependencies()
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

        var dependenciesInfo = sb.ToString();

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
        var s = (Settings)Application.Current.TryFindResource("Settings");

        this.DataLoader.Model.AttackBuffSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = this.DataLoader.Model.AttackBuffCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerAttackGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerAttackGraphColor, "7f")), new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerAttackGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.DataLoader.Model.DamagePerSecondSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = this.DataLoader.Model.DamagePerSecondCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerDPSGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerDPSGraphColor, "7f")), new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerDPSGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.DataLoader.Model.ActionsPerMinuteSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = this.DataLoader.Model.ActionsPerMinuteCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerAPMGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerAPMGraphColor, "7f")), new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerAPMGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.DataLoader.Model.HitsPerSecondSeries.Add(new LineSeries<ObservablePoint>
        {
            Values = this.DataLoader.Model.HitsPerSecondCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(this.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });
    }

    /// <summary>
    /// Loads the github api integration.
    /// </summary>
    private async Task LoadOctoKit()
    {
        var releases = await this.ghClient.Repository.Release.GetAll("DorielRivalet", "MHFZ_Overlay");
        var latest = releases[0];
        var latestRelease = latest.TagName;
        if (latestRelease != Program.CurrentProgramVersion)
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableUpdateNotifier)
            {
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Detected different overlay version");
                var messageBoxResult = System.Windows.MessageBox.Show(
                    string.Format(
                    CultureInfo.InvariantCulture,
                    @"Detected different version ({0}) from latest ({1}). Do you want to update the overlay?

The process may take some time, as the program attempts to download from GitHub Releases. You will get a notification once the process is complete.",
                    Program.CurrentProgramVersion, latest.TagName),
                    "【MHF-Z】Overlay Update Available",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Asterisk,
                    MessageBoxResult.No);

                if (messageBoxResult.ToString() == "Yes")
                {
                    await Program.UpdateMyApp();
                }
            }
        }
    }

    /// <summary>
    /// TODO: optimization. The main loop of the program, affected by Refresh Rate.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    private async void Timer_Tick(object? obj, EventArgs e)
    {
        try
        {
            this.DataLoader.Model.ReloadData();
            this.Monster1HPBar.ReloadData();
            this.Monster2HPBar.ReloadData();
            this.Monster3HPBar.ReloadData();
            this.Monster4HPBar.ReloadData();
            this.MonsterPoisonBar.ReloadData();
            this.MonsterSleepBar.ReloadData();
            this.MonsterParaBar.ReloadData();
            this.MonsterBlastBar.ReloadData();
            this.MonsterStunBar.ReloadData();

            this.CreateDamageNumber();
            await this.CheckQuestStateForDatabaseLogging();

            // TODO should this be here or somewhere else?
            // this is also for database logging
            this.CheckMezFesScore();
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
            this.HideMonsterInfoWhenNotInQuest();
            this.HidePlayerInfoWhenNotInQuest();
            await Task.Run(() => DiscordManagerInstance.UpdateDiscordRPC(this.DataLoader));
            this.CheckIfLocationChanged();
            this.CheckIfQuestChanged();
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
            this.DataLoader.CheckForExternalProcesses();
            this.DataLoader.CheckForIllegalModifications(this.DataLoader);
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
            this.gamepad.Update();
            if (!this.gamepad.IsConnected)
            {
                this.gamepad.Device = XInputDevice.GetFirstConnectedDevice();
                if (this.gamepadImages.Count > 0)
                {
                    this.gamepadImages.Clear();
                }

                if (this.gamepadTriggersImages.Count > 0)
                {
                    this.gamepadTriggersImages.Clear();
                }

                if (this.gamepadJoystickImages.Count > 0)
                {
                    this.gamepadJoystickImages.Clear();
                    LoggerInstance.Debug("Gamepad disconnected");
                }
            }

            if (this.gamepad.IsConnected && this.gamepadImages.Count == 0 && this.gamepadTriggersImages.Count == 0 && this.gamepadJoystickImages.Count == 0)
            {
                this.AddGamepadImages();
                LoggerInstance.Debug("Gamepad connected");
            }
        }
        catch (Exception ex)
        {
            LoggingService.WriteCrashLog(ex);
        }
    }

    private void CheckIfQuestChanged()
    {
        if (this.DataLoader.Model.PreviousQuestID != this.DataLoader.Model.QuestID() && this.DataLoader.Model.QuestID() != 0)
        {
            this.DataLoader.Model.PreviousQuestID = this.DataLoader.Model.QuestID();
            LoggerInstance.Info(CultureInfo.InvariantCulture, $"In quest: ID {this.DataLoader.Model.PreviousQuestID} | {AddressModel.GetQuestName(this.DataLoader.Model.PreviousQuestID)}");
            this.ShowQuestName();
        }
        else if (this.DataLoader.Model.QuestID() == 0 && this.DataLoader.Model.PreviousQuestID != 0)
        {
            this.DataLoader.Model.PreviousQuestID = this.DataLoader.Model.QuestID();
        }
    }

    private void ShowQuestName()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        if (s == null || !s.QuestNameShown)
        {
            return;
        }

        EZlion.Mapper.Quest.IDName.TryGetValue(this.DataLoader.Model.PreviousQuestID, out var previousQuestID);
        if (string.IsNullOrEmpty(previousQuestID))
        {
            previousQuestID = string.Format("{0} {1}", Messages.CustomQuestName, this.DataLoader.Model.QuestID());
        }

        this.questNameTextBlock.Text = previousQuestID;
        Brush blackBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        Brush peachBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xB3, 0x87));
        AnimateOutlinedTextBlock(this.questNameTextBlock, blackBrush, peachBrush);
    }

    /// <summary>
    /// Animates the outlined text block.
    /// </summary>
    /// <param name="outlinedTextBlock">The outlined text block.</param>
    private static void AnimateOutlinedTextBlock(OutlinedTextBlock outlinedTextBlock, Brush startBrush, Brush endBrush)
    {
        // Define the animation durations and colors
        var fadeInDuration = TimeSpan.FromSeconds(1);
        var fadeOutDuration = TimeSpan.FromSeconds(1);

        var fadeIn = new DoubleAnimation(0, 1, fadeInDuration);
        var fadeOut = new DoubleAnimation(1, 0, fadeOutDuration);
        var colorInAnimation = new BrushAnimation
        {
            From = startBrush,
            To = endBrush,
            Duration = fadeInDuration,
        };
        var colorOutAnimation = new BrushAnimation
        {
            From = endBrush,
            To = startBrush,
            Duration = fadeOutDuration,
        };

        var fadeInStoryboard = new Storyboard();
        Storyboard.SetTarget(fadeIn, outlinedTextBlock);
        Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));
        Storyboard.SetTarget(colorInAnimation, outlinedTextBlock);
        Storyboard.SetTargetProperty(colorInAnimation, new PropertyPath(OutlinedTextBlock.FillProperty));
        fadeInStoryboard.Children.Add(fadeIn);
        fadeInStoryboard.Children.Add(colorInAnimation);

        var fadeOutStoryboard = new Storyboard();
        Storyboard.SetTarget(fadeOut, outlinedTextBlock);
        Storyboard.SetTargetProperty(fadeOut, new PropertyPath(OpacityProperty));
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

    private bool IsInHubAreaID() => this.DataLoader.Model.AreaID() switch
    {
        // Mezeporta
        200 or 210 or 260 or 282 or 202 or 203 or 204 => true,
        _ => false,
    };

    private void CheckIfLocationChanged()
    {
        if (this.IsInHubAreaID() && this.DataLoader.Model.QuestID() == 0)
        {
            this.DataLoader.Model.PreviousHubAreaID = this.DataLoader.Model.AreaID();
        }

        if (this.DataLoader.Model.PreviousGlobalAreaID != this.DataLoader.Model.AreaID() && this.DataLoader.Model.AreaID() != 0)
        {
            this.DataLoader.Model.PreviousGlobalAreaID = this.DataLoader.Model.AreaID();
            this.ShowLocationName();
        }
    }

    private void ShowLocationName()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        if (s == null || !s.LocationTextShown)
        {
            return;
        }

        Location.IDName.TryGetValue(this.DataLoader.Model.PreviousGlobalAreaID, out var previousGlobalAreaID);
        if (previousGlobalAreaID == null)
        {
            return;
        }

        this.locationTextBlock.Text = previousGlobalAreaID;
        Brush blackBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        Brush blueBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x89, 0xB4, 0xFA));
        AnimateOutlinedTextBlock(this.locationTextBlock, blackBrush, blueBrush);
    }

    private int PrevNum { get; set; }

    private bool IsFirstAttack { get; set; }

    /// <summary>
    /// Shows multicolor damage numbers?.
    /// </summary>
    /// <returns></returns>
    public static bool ShowDamageNumbersMulticolor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableDamageNumbersMulticolor;
    }

    // Import the necessary Win32 functions
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    /// <summary>
    /// Creates the damage number.
    /// </summary>
    private void CreateDamageNumber()
    {
        if (this.DataLoader.Model.QuestID() == 0)
        {
            return;
        }

        var damage = 0;
        if (this.DataLoader.Model.HitCountInt() == 0)
        {
            this.CurNum = 0;
            this.PrevNum = 0;
            this.IsFirstAttack = true;
        }
        else
        {
            damage = this.DataLoader.Model.DamageDealt();
        }

        if (this.PrevNum != damage)
        {
            this.CurNum = damage - this.PrevNum;
            if (this.IsFirstAttack)
            {
                this.IsFirstAttack = false;
                this.CreateDamageNumberLabel(damage);
                if (!this.DataLoader.Model.DamageDealtDictionary.ContainsKey(this.DataLoader.Model.TimeInt()))
                {
                    try
                    {
                        this.DataLoader.Model.DamageDealtDictionary.Add(this.DataLoader.Model.TimeInt(), damage);
                    }
                    catch (Exception ex)
                    {
                        LoggerInstance.Warn(ex, "Could not insert into damageDealtDictionary");
                    }
                }
            }
            else if (this.CurNum < 0)
            {
                // TODO
                this.CurNum += 1_000;
                this.CreateDamageNumberLabel(this.CurNum);
                if (!this.DataLoader.Model.DamageDealtDictionary.ContainsKey(this.DataLoader.Model.TimeInt()))
                {
                    try
                    {
                        this.DataLoader.Model.DamageDealtDictionary.Add(this.DataLoader.Model.TimeInt(), this.CurNum);
                    }
                    catch (Exception ex)
                    {
                        LoggerInstance.Warn(ex, "Could not insert into damageDealtDictionary");
                    }
                }
            }
            else
            {
                if (this.CurNum != damage)
                {
                    this.CreateDamageNumberLabel(this.CurNum);
                    if (!this.DataLoader.Model.DamageDealtDictionary.ContainsKey(this.DataLoader.Model.TimeInt()))
                    {
                        try
                        {
                            this.DataLoader.Model.DamageDealtDictionary.Add(this.DataLoader.Model.TimeInt(), this.CurNum);
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

        this.PrevNum = damage;
    }

    /// <summary>
    /// Creates the damage number label.
    /// </summary>
    /// <param name="damage">The damage.</param>
    private void CreateDamageNumberLabel(int damage)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        Random random = new ();
        double x = random.Next(450);
        double y = random.Next(254);
        var newPoint = this.DamageNumbers.TranslatePoint(new Point(x, y), this.DamageNumbers);

        // Create a new instance of the OutlinedTextBlock class.
        var damageOutlinedTextBlock = new OutlinedTextBlock
        {
            // Set the properties of the OutlinedTextBlock instance.
            FontFamily = new System.Windows.Media.FontFamily(s.DamageNumbersFontFamily),
            FontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(s.DamageNumbersFontWeight),
            FontSize = 21,
            StrokeThickness = 4,
            Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E)),
        };

        // does not alter actual number displayed, only the text style
        double damageModifier = damage / (this.DataLoader.Model.CurrentWeaponMultiplier / 2);
        var exclamations = string.Empty;

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

        if (!double.TryParse(this.DataLoader.Model.DefMult, NumberStyles.Any, CultureInfo.InvariantCulture, out var defenseMultiplier))
        {
            LoggerInstance.Warn("Could not parse monster defense multiplier: {0}", this.DataLoader.Model.DefMult);
            defenseMultiplier = 1; // Default value if parse fails
        }

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

        // TODO sometimes i still show 0 as damage. specially in road.
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
            var color = ColorTranslator.FromHtml(s.DamageNumbersColor);
            damageOutlinedTextBlock.Fill = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        damageOutlinedTextBlock.SetValue(Canvas.TopProperty, newPoint.Y);
        damageOutlinedTextBlock.SetValue(Canvas.LeftProperty, newPoint.X);

        this.DamageNumbers.Children.Add(damageOutlinedTextBlock);

        if (!s.EnableDamageNumbersFlash && !s.EnableDamageNumbersSize)
        {
            this.RemoveDamageNumberLabel(damageOutlinedTextBlock);
        }
        else
        {
            Brush blackBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
            Brush whiteBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xCD, 0xD6, 0xF4));
            Brush redBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xF3, 0x8B, 0xA8));
            var originalFillBrush = damageOutlinedTextBlock.Fill;

            // TODO: Animation, inspired by rise

            // Create a Storyboard to animate the label's size, color and opacity
            var fadeInIncreaseSizeFlashColorStoryboard = new Storyboard();

            // Create a DoubleAnimation to animate the label's width
            var sizeIncreaseAnimation = new DoubleAnimation
            {
                From = 0,
                To = damageOutlinedTextBlock.FontSize * 1.5,
                Duration = TimeSpan.FromSeconds(.3),
            };
            Storyboard.SetTarget(sizeIncreaseAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(sizeIncreaseAnimation, new PropertyPath(OutlinedTextBlock.FontSizeProperty));

            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(.3),
            };
            Storyboard.SetTarget(fadeInAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(OpacityProperty));

            var flashWhiteStrokeAnimation = new BrushAnimation
            {
                From = whiteBrush,
                To = redBrush,
                Duration = TimeSpan.FromSeconds(0.05),
            };
            Storyboard.SetTarget(flashWhiteStrokeAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(flashWhiteStrokeAnimation, new PropertyPath(OutlinedTextBlock.StrokeProperty));

            var flashWhiteFillAnimation = new BrushAnimation
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

            var decreaseSizeShowColorStoryboard = new Storyboard();

            // Create a DoubleAnimation to animate the label's width
            var sizeDecreaseAnimation = new DoubleAnimation
            {
                From = damageOutlinedTextBlock.FontSize * 1.5,
                To = damageOutlinedTextBlock.FontSize,
                Duration = TimeSpan.FromSeconds(.2),
            };
            Storyboard.SetTarget(sizeDecreaseAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(sizeDecreaseAnimation, new PropertyPath(OutlinedTextBlock.FontSizeProperty));

            var showColorStrokeAnimation = new BrushAnimation
            {
                From = redBrush,
                To = blackBrush,
                Duration = TimeSpan.FromSeconds(0.05),
            };
            Storyboard.SetTarget(showColorStrokeAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(showColorStrokeAnimation, new PropertyPath(OutlinedTextBlock.StrokeProperty));

            var showColorFillAnimation = new BrushAnimation
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

            var fadeOutStoryboard = new Storyboard();

            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(.4),
                BeginTime = TimeSpan.FromSeconds(0.75),
            };

            Storyboard.SetTarget(fadeOutAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(OpacityProperty));

            var translateUpwardsAnimation = new DoubleAnimation
            {
                From = y,
                To = y - 20,
                Duration = TimeSpan.FromSeconds(.4),
                BeginTime = TimeSpan.FromSeconds(0.75),
            };

            Storyboard.SetTarget(translateUpwardsAnimation, damageOutlinedTextBlock);
            Storyboard.SetTargetProperty(translateUpwardsAnimation, new PropertyPath(Canvas.TopProperty));

            fadeOutStoryboard.Children.Add(fadeOutAnimation);
            fadeOutStoryboard.Children.Add(translateUpwardsAnimation);

            // Set up event handlers to start the next animation in the sequence
            fadeInIncreaseSizeFlashColorStoryboard.Completed += (s, e) => decreaseSizeShowColorStoryboard.Begin();
            decreaseSizeShowColorStoryboard.Completed += (s, e) => fadeOutStoryboard.Begin();
            fadeOutAnimation.Completed += (s, e) => this.DamageNumbers.Children.Remove(damageOutlinedTextBlock);

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
        DispatcherTimer timer = new ()
        {
            Interval = new TimeSpan(0, 0, 0, 0, 1_000),
        };

        // memory leak?
        timer.Tick += (o, e) => this.DamageNumbers.Children.Remove(tb);
        timer.Start();
    }

    // does this sometimes bug?
    // the UI flashes at least once when loading into quest

    /// <summary>
    /// Hides the monster information when not in quest.
    /// </summary>
    private void HideMonsterInfoWhenNotInQuest()
    {
        var s = (Settings)Application.Current.FindResource("Settings");
        var v = this.IsGameFocused(s) &&
            (s.AlwaysShowMonsterInfo || this.DataLoader.Model.Configuring || this.DataLoader.Model.QuestID() != 0);
        this.SetMonsterStatsVisibility(v, s);
    }

    /// <summary>
    /// Checks if the game or overlay is focused.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is game focused; otherwise, <c>false</c>.
    /// </value>
    private bool IsGameFocused(Settings s)
    {
        if (!s.HideOverlayWhenUnfocusedGame || this.DataLoader.Model.Configuring)
        {
            return true;
        }

        // Get the active window handle
        var activeWindowHandle = GetForegroundWindow();

        // Check if the active window belongs to the current process or any process spawned from it
        if (activeWindowHandle == CurrentProcess.MainWindowHandle)
        {
            return true;
        }
        else
        {
            // Check if the active window belongs to the game process
            var isGameProcessActive = this.MhfProcess != null && activeWindowHandle == this.MhfProcess.MainWindowHandle;
            return isGameProcessActive;
        }
    }

    private void SetMonsterStatsVisibility(bool v, Settings s)
    {
        this.DataLoader.Model.ShowMonsterAtkMult = v && s.MonsterAtkMultShown;
        this.DataLoader.Model.ShowMonsterDefrate = v && s.MonsterDefrateShown;
        this.DataLoader.Model.ShowMonsterSize = v && s.MonsterSizeShown;
        this.DataLoader.Model.ShowMonsterPoison = v && s.MonsterPoisonShown;
        this.DataLoader.Model.ShowMonsterSleep = v && s.MonsterSleepShown;
        this.DataLoader.Model.ShowMonsterPara = v && s.MonsterParaShown;
        this.DataLoader.Model.ShowMonsterBlast = v && s.MonsterBlastShown;
        this.DataLoader.Model.ShowMonsterStun = v && s.MonsterStunShown;

        this.DataLoader.Model.ShowMonster1HPBar = v && s.Monster1HealthBarShown;
        this.DataLoader.Model.ShowMonster2HPBar = v && s.Monster2HealthBarShown;
        this.DataLoader.Model.ShowMonster3HPBar = v && s.Monster3HealthBarShown;
        this.DataLoader.Model.ShowMonster4HPBar = v && s.Monster4HealthBarShown;

        this.DataLoader.Model.ShowMonsterPartHP = v && s.PartThresholdShown;
        this.DataLoader.Model.ShowMonster1Icon = v && s.Monster1IconShown;
    }

    /// <summary>
    /// Hides the player information when not in quest.
    /// </summary>
    private void HidePlayerInfoWhenNotInQuest()
    {
        var s = (Settings)Application.Current.FindResource("Settings");
        var v = this.IsGameFocused(s) &&
            (s.AlwaysShowPlayerInfo || this.DataLoader.Model.Configuring || this.DataLoader.Model.QuestID() != 0);
        this.SetPlayerStatsVisibility(v, s);
    }

    private void SetPlayerStatsVisibility(bool v, Settings s)
    {
        // DL.m.?.Visibility = v && s.?.IsChecked
        this.DataLoader.Model.ShowTimerInfo = v && s.TimerInfoShown;
        this.DataLoader.Model.ShowHitCountInfo = v && s.HitCountShown;
        this.DataLoader.Model.ShowPlayerAtkInfo = v && s.PlayerAtkShown;
        this.DataLoader.Model.ShowPlayerHitsTakenBlockedInfo = v && s.TotalHitsTakenBlockedShown;
        this.DataLoader.Model.ShowSharpness = v && s.EnableSharpness;
        this.DataLoader.Model.ShowSessionTimeInfo = v && s.SessionTimeShown;
        this.DataLoader.Model.ShowPlayerPositionInfo = v && s.PlayerPositionShown;
        this.DataLoader.Model.ShowDivaSongTimer = v && s.DivaSongTimerShown && this.DataLoader.Model.DivaSongActive;
        this.DataLoader.Model.ShowGuildFoodTimer = v && s.GuildFoodTimerShown && this.DataLoader.Model.GuildFoodActive;

        this.DataLoader.Model.ShowMap = v && s.EnableMap;
        this.DataLoader.Model.ShowFrameCounter = v && s.FrameCounterShown;
        this.DataLoader.Model.ShowPlayerAttackGraph = v && s.PlayerAttackGraphShown;
        this.DataLoader.Model.ShowPlayerDPSGraph = v && s.PlayerDPSGraphShown;
        this.DataLoader.Model.ShowPlayerAPMGraph = v && s.PlayerAPMGraphShown;
        this.DataLoader.Model.ShowPlayerHitsPerSecondGraph = v && s.PlayerHitsPerSecondGraphShown;

        this.DataLoader.Model.ShowDamagePerSecond = v && s.DamagePerSecondShown;

        this.DataLoader.Model.ShowKBMLayout = v && s.KBMLayoutShown;
        this.DataLoader.Model.ShowGamepadLayout = v && s.GamepadShown;
        this.DataLoader.Model.ShowAPM = v && s.ActionsPerMinuteShown;
        this.DataLoader.Model.ShowOverlayModeWatermark = v && s.OverlayModeWatermarkShown;
        this.DataLoader.Model.ShowQuestID = v && s.QuestIDShown;

        this.DataLoader.Model.ShowPersonalBestInfo = v && s.PersonalBestShown;
        this.DataLoader.Model.ShowQuestAttemptsInfo = v && s.QuestAttemptsShown;
        this.DataLoader.Model.ShowPersonalBestTimePercentInfo = v && s.PersonalBestTimePercentShown;
        this.DataLoader.Model.ShowPersonalBestAttemptsInfo = v && s.PersonalBestAttemptsShown;

        this.DataLoader.Model.ShowDualSwordsSharpens = v && s.DualSwordsSharpensShown && DataLoader.Model.CurrentWeaponName == "Dual Swords";

    }

    private double? yOffset { get; set; }

    private FrameworkElement? movingObject { get; set; }

    private bool clickThrough { get; set; } = true;

    private ConfigWindow? ConfigWindow { get; set; }

    public void EnableDragAndDrop()
    {
        this.IsDragConfigure = true;
        this.ExitDragAndDrop.Visibility = Visibility.Visible;
        this.MainGrid.Background = (Brush?)new BrushConverter().ConvertFrom("#01000000");
        if (this.ConfigWindow != null)
        {
            this.ConfigWindow.Visibility = Visibility.Hidden;
        }

        this.ToggleClickThrough();
        this.ToggleOverlayBorders();
    }

    public void DisableDragAndDrop()
    {
        this.IsDragConfigure = false;
        this.ExitDragAndDrop.Visibility = Visibility.Hidden;
        this.MainGrid.Background = (Brush?)new BrushConverter().ConvertFrom("#00FFFFFF");
        if (this.ConfigWindow != null)
        {
            this.ConfigWindow.Visibility = Visibility.Visible;
        }

        this.ToggleClickThrough();
        this.ToggleOverlayBorders();
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

    private void MainGrid_DragOver(object sender, DragEventArgs e)
    {
        if (this.movingObject == null)
        {
            return;
        }

        var pos = e.GetPosition(this);
        if (this.xOffset == null || this.yOffset == null)
        {
            return;
        }

        var s = (Settings)Application.Current.TryFindResource("Settings");
        switch (this.movingObject.Name)
        {
            case "TimerInfo":
                s.TimerX = (double)(pos.X - this.xOffset);
                s.TimerY = (double)(pos.Y - this.yOffset);
                break;
            case "PersonalBestInfo":
                s.PersonalBestX = (double)(pos.X - this.xOffset);
                s.PersonalBestY = (double)(pos.Y - this.yOffset);
                break;
            case "QuestAttemptsInfo":
                s.QuestAttemptsX = (double)(pos.X - this.xOffset);
                s.QuestAttemptsY = (double)(pos.Y - this.yOffset);
                break;
            case "HitCountInfo":
                s.HitCountX = (double)(pos.X - this.xOffset);
                s.HitCountY = (double)(pos.Y - this.yOffset);
                break;
            case "PlayerAtkInfo":
                s.PlayerAtkX = (double)(pos.X - this.xOffset);
                s.PlayerAtkY = (double)(pos.Y - this.yOffset);
                break;
            case "PlayerHitsTakenBlockedInfo":
                s.TotalHitsTakenBlockedX = (double)(pos.X - this.xOffset);
                s.TotalHitsTakenBlockedY = (double)(pos.Y - this.yOffset);
                break;

            // TODO graphs
            case "PlayerAttackGraphGrid":
                s.PlayerAttackGraphX = (double)(pos.X - this.xOffset);
                s.PlayerAttackGraphY = (double)(pos.Y - this.yOffset);
                break;
            case "PlayerDPSGraphGrid":
                s.PlayerDPSGraphX = (double)(pos.X - this.xOffset);
                s.PlayerDPSGraphY = (double)(pos.Y - this.yOffset);
                break;
            case "PlayerAPMGraphGrid":
                s.PlayerAPMGraphX = (double)(pos.X - this.xOffset);
                s.PlayerAPMGraphY = (double)(pos.Y - this.yOffset);
                break;
            case "PlayerHitsPerSecondGraphGrid":
                s.PlayerHitsPerSecondGraphX = (double)(pos.X - this.xOffset);
                s.PlayerHitsPerSecondGraphY = (double)(pos.Y - this.yOffset);
                break;

            case "DamagePerSecondInfo":
                s.PlayerDPSX = (double)(pos.X - this.xOffset);
                s.PlayerDPSY = (double)(pos.Y - this.yOffset);
                break;
            case "KBMLayoutGrid":
                s.KBMLayoutX = (double)(pos.X - this.xOffset);
                s.KBMLayoutY = (double)(pos.Y - this.yOffset);
                break;
            case "GamepadGrid":
                s.GamepadX = (double)(pos.X - this.xOffset);
                s.GamepadY = (double)(pos.Y - this.yOffset);
                break;
            case "ActionsPerMinuteInfo":
                s.ActionsPerMinuteX = (double)(pos.X - this.xOffset);
                s.ActionsPerMinuteY = (double)(pos.Y - this.yOffset);
                break;
            case "MapImage":
                s.MapX = (double)(pos.X - this.xOffset);
                s.MapY = (double)(pos.Y - this.yOffset);
                break;
            case "DualSwordsSharpensInfo":
                s.DualSwordsSharpensX = (double)(pos.X - this.xOffset);
                s.DualSwordsSharpensY = (double)(pos.Y - this.yOffset);
                break;

            // case "OverlayModeWatermark":
            //    s.OverlayModeWatermarkX = (double)(pos.X - this.XOffset);
            //    s.OverlayModeWatermarkY = (double)(pos.Y - this.YOffset);
            //    break;
            case "QuestIDGrid":
                s.QuestIDX = (double)(pos.X - this.xOffset);
                s.QuestIDY = (double)(pos.Y - this.yOffset);
                break;
            case "SessionTimeInfo":
                s.SessionTimeX = (double)(pos.X - this.xOffset);
                s.SessionTimeY = (double)(pos.Y - this.yOffset);
                break;
            case "PlayerPositionInfo":
                s.PlayerPositionX = (double)(pos.X - this.xOffset);
                s.PlayerPositionY = (double)(pos.Y - this.yOffset);
                break;
            case "DivaSongTimer":
                s.DivaSongTimerX = (double)(pos.X - this.xOffset);
                s.DivaSongTimerY = (double)(pos.Y - this.yOffset);
                break;
            case "GuildFoodTimer":
                s.GuildFoodTimerX = (double)(pos.X - this.xOffset);
                s.GuildFoodTimerY = (double)(pos.Y - this.yOffset);
                break;
            case "LocationTextInfo":
                s.LocationTextX = (double)(pos.X - this.xOffset);
                s.LocationTextY = (double)(pos.Y - this.yOffset);
                break;
            case "QuestNameInfo":
                s.QuestNameX = (double)(pos.X - this.xOffset);
                s.QuestNameY = (double)(pos.Y - this.yOffset);
                break;
            case "PersonalBestTimePercentInfo":
                s.PersonalBestTimePercentX = (double)(pos.X - this.xOffset);
                s.PersonalBestTimePercentY = (double)(pos.Y - this.yOffset);
                break;
            case "PersonalBestAttemptsInfo":
                s.PersonalBestAttemptsX = (double)(pos.X - this.xOffset);
                s.PersonalBestAttemptsY = (double)(pos.Y - this.yOffset);
                break;

            // Monster
            case "Monster1HpBar":
                s.Monster1HealthBarX = (double)(pos.X - this.xOffset);
                s.Monster1HealthBarY = (double)(pos.Y - this.yOffset);
                break;
            case "Monster2HpBar":
                s.Monster2HealthBarX = (double)(pos.X - this.xOffset);
                s.Monster2HealthBarY = (double)(pos.Y - this.yOffset);
                break;
            case "Monster3HpBar":
                s.Monster3HealthBarX = (double)(pos.X - this.xOffset);
                s.Monster3HealthBarY = (double)(pos.Y - this.yOffset);
                break;
            case "Monster4HpBar":
                s.Monster4HealthBarX = (double)(pos.X - this.xOffset);
                s.Monster4HealthBarY = (double)(pos.Y - this.yOffset);
                break;

            case "MonsterAtkMultInfo":
                s.MonsterAtkMultX = (double)(pos.X - this.xOffset);
                s.MonsterAtkMultY = (double)(pos.Y - this.yOffset);
                break;
            case "MonsterDefrateInfo":
                s.MonsterDefrateX = (double)(pos.X - this.xOffset);
                s.MonsterDefrateY = (double)(pos.Y - this.yOffset);
                break;
            case "MonsterSizeInfo":
                s.MonsterSizeX = (double)(pos.X - this.xOffset);
                s.MonsterSizeY = (double)(pos.Y - this.yOffset);
                break;
            case "MonsterPoisonInfo":
                s.MonsterPoisonX = (double)(pos.X - this.xOffset);
                s.MonsterPoisonY = (double)(pos.Y - this.yOffset);
                break;
            case "MonsterSleepInfo":
                s.MonsterSleepX = (double)(pos.X - this.xOffset);
                s.MonsterSleepY = (double)(pos.Y - this.yOffset);
                break;
            case "MonsterParaInfo":
                s.MonsterParaX = (double)(pos.X - this.xOffset);
                s.MonsterParaY = (double)(pos.Y - this.yOffset);
                break;
            case "MonsterBlastInfo":
                s.MonsterBlastX = (double)(pos.X - this.xOffset);
                s.MonsterBlastY = (double)(pos.Y - this.yOffset);
                break;
            case "MonsterStunInfo":
                s.MonsterStunX = (double)(pos.X - this.xOffset);
                s.MonsterStunY = (double)(pos.Y - this.yOffset);
                break;
            case "SharpnessInfo":
                s.SharpnessInfoX = (double)(pos.X - this.xOffset);
                s.SharpnessInfoY = (double)(pos.Y - this.yOffset);
                break;
            case "MonsterPartThreshold":
                s.Monster1PartX = (double)(pos.X - this.xOffset);
                s.Monster1PartY = (double)(pos.Y - this.yOffset);
                break;
            case "Monster1Icon":
                s.Monster1IconX = (double)(pos.X - this.xOffset);
                s.Monster1IconY = (double)(pos.Y - this.yOffset);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Handles the Drop event of the MainGrid control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
    private void MainGrid_Drop(object sender, DragEventArgs e)
    {
        if (this.movingObject != null)
        {
            this.movingObject.IsHitTestVisible = true;
        }

        this.movingObject = null;
    }

    /// <summary>
    /// Elements the mouse left button down.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
    private void ElementMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!this.IsDragConfigure)
        {
            return;
        }

        this.movingObject = (FrameworkElement)sender;
        var pos = e.GetPosition(this);
        this.xOffset = pos.X - Canvas.GetLeft(this.movingObject);
        this.yOffset = pos.Y - Canvas.GetTop(this.movingObject);
        this.movingObject.IsHitTestVisible = false;
    }

    private void ReloadButton_Click(object sender, RoutedEventArgs e) => ApplicationService.HandleRestart();

    private void OpenConfigButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.ConfigWindow == null || !this.ConfigWindow.IsLoaded)
        {
            this.ConfigWindow = new (this);
        }

        this.ConfigWindow.Show();
        this.DataLoader.Model.Configuring = true;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => ApplicationService.HandleShutdown();

    // https://stackoverflow.com/questions/4773632/how-do-i-restart-a-wpf-application
    private void ReloadButton_Key() => ApplicationService.HandleRestart();

    private void OpenConfigButton_Key()
    {
        if (this.IsDragConfigure)
        {
            return;
        }

        if (this.DataLoader.Model.IsInLauncherBool)
        {
            System.Windows.MessageBox.Show("Using the configuration menu outside of the game might cause slow performance", Messages.WarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
            LoggerInstance.Info(CultureInfo.InvariantCulture, "Detected game launcher while using configuration menu");
        }

        if (this.ConfigWindow == null || !this.ConfigWindow.IsLoaded)
        {
            this.ConfigWindow = new (this);
        }

        try
        {
            this.ConfigWindow.Show(); // TODO: memory error?
            this.DataLoader.Model.Configuring = true;
        }
        catch (Exception ex)
        {
            LoggerInstance.Error(CultureInfo.InvariantCulture, "Could not show configuration window", ex);
        }

        try
        {
            this.DataLoader.CheckForExternalProcesses();
            this.DataLoader.CheckForIllegalModifications(this.DataLoader);
        }
        catch (Exception ex)
        {
            LoggingService.WriteCrashLog(ex);
        }
    }

    private void CloseButton_Key() => ApplicationService.HandleShutdown();

    private void MainGrid_MouseMove(object sender, MouseEventArgs e) => DoDragDrop(this.movingObject);

    // TODO: use a dictionary for looping instead
    private void ToggleOverlayBorders()
    {
        var thickness = new Thickness(0);

        if (this.IsDragConfigure)
        {
            thickness = new Thickness(2);
        }

        this.ActionsPerMinuteInfoBorder.BorderThickness = thickness;
        this.DamagePerSecondInfoBorder.BorderThickness = thickness;
        this.HitCountInfoBorder.BorderThickness = thickness;
        this.LocationTextInfoBorder.BorderThickness = thickness;
        this.MonsterAtkMultInfoBorder.BorderThickness = thickness;
        this.MonsterBlastInfoBorder.BorderThickness = thickness;
        this.MonsterDefrateInfoBorder.BorderThickness = thickness;
        this.MonsterParaInfoBorder.BorderThickness = thickness;
        this.MonsterPoisonInfoBorder.BorderThickness = thickness;
        this.MonsterSizeInfoBorder.BorderThickness = thickness;
        this.MonsterSleepInfoBorder.BorderThickness = thickness;
        this.MonsterStunInfoBorder.BorderThickness = thickness;
        this.PersonalBestInfoBorder.BorderThickness = thickness;
        this.PersonalBestTimePercentInfoBorder.BorderThickness = thickness;
        this.PlayerAtkInfoBorder.BorderThickness = thickness;
        this.PlayerHitsTakenBlockedInfoBorder.BorderThickness = thickness;
        this.QuestAttemptsInfoBorder.BorderThickness = thickness;
        this.PersonalBestAttemptsInfoBorder.BorderThickness = thickness;
        this.QuestNameInfoBorder.BorderThickness = thickness;
        this.SessionTimeInfoBorder.BorderThickness = thickness;
        this.PlayerPositionInfoBorder.BorderThickness = thickness;
        this.DivaSongTimerBorder.BorderThickness = thickness;
        this.GuildFoodTimerBorder.BorderThickness = thickness;
        this.SharpnessInfoBorder.BorderThickness = thickness;
        this.TimerInfoBorder.BorderThickness = thickness;
        this.GamepadGridBorder.BorderThickness = thickness;
        this.KBMLayoutGridBorder.BorderThickness = thickness;
        this.QuestIDGridBorder.BorderThickness = thickness;
        this.OverlayModeWatermarkBorder.BorderThickness = thickness;
        this.Monster1HpBarBorder.BorderThickness = thickness;
        this.Monster2HpBarBorder.BorderThickness = thickness;
        this.Monster3HpBarBorder.BorderThickness = thickness;
        this.Monster4HpBarBorder.BorderThickness = thickness;
        this.MonsterPartThresholdBorder.BorderThickness = thickness;
        this.DamageNumbersBorder.BorderThickness = thickness;
        this.PlayerAPMGraphGridBorder.BorderThickness = thickness;
        this.PlayerAttackGraphGridBorder.BorderThickness = thickness;
        this.PlayerDPSGraphGridBorder.BorderThickness = thickness;
        this.PlayerHitsPerSecondGraphGridBorder.BorderThickness = thickness;
        this.TitleBarBorder.BorderThickness = thickness;
        this.DualSwordsSharpensBorder.BorderThickness = thickness;
    }

    private void ToggleClickThrough()
    {
        if (!this.clickThrough)
        {
            this.IsHitTestVisible = false;
            this.Focusable = false;

            // Get this window's handle
            var hwnd = new WindowInteropHelper(this).Handle;

            // Change the extended window style to include WS_EX_TRANSPARENT
            var extendedStyle = GetWindowLong(hwnd, GWLEXSTYLE);
            SetWindowLong(hwnd, GWLEXSTYLE, extendedStyle | WSEXTRANSPARENT);
        }
        else
        {
            this.IsHitTestVisible = true;
            this.Focusable = true;

            // Get this window's handle
            var hwnd = new WindowInteropHelper(this).Handle;

            // Change the extended window style to include WS_EX_TRANSPARENT
            SetWindowLong(hwnd, GWLEXSTYLE, this.originalStyle);
        }

        this.clickThrough = !this.clickThrough;
    }

    private void ExitDragAndDrop_Click(object sender, RoutedEventArgs e) => this.DisableDragAndDrop();

    private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) => this.DisableDragAndDrop();

    // TODO fix alt tab issues?
    private IKeyboardMouseEvents GlobalHook { get; set; }

    public static MediaPlayer? MainWindowMediaPlayer { get; private set; }

    /// <summary>
    /// Subscribes this instance for player input.
    /// </summary>
    public void Subscribe()
    {
        // Note: for the application hook, use the Hook.AppEvents() instead
        this.GlobalHook = Hook.GlobalEvents();

        this.GlobalHook.MouseDownExt += this.GlobalHookMouseDownExt;
        this.GlobalHook.MouseUpExt += this.GlobalHookMouseUpExt;
        this.GlobalHook.KeyPress += this.GlobalHookKeyPress;
        this.GlobalHook.KeyDown += this.GlobalHookKeyDown;
        this.GlobalHook.KeyUp += this.GlobalHookKeyUp;

        // Register the event handler for button presses
        // TODO: do i really need this kind of interface?
        // m_GlobalHook.KeyDown += HandleHorizontalInput;
    }

    public void Unsubscribe()
    {
        this.GlobalHook.MouseDownExt -= this.GlobalHookMouseDownExt;
        this.GlobalHook.KeyPress -= this.GlobalHookKeyPress;

        this.GlobalHook.MouseUpExt -= this.GlobalHookMouseUpExt;
        this.GlobalHook.KeyDown -= this.GlobalHookKeyDown;
        this.GlobalHook.KeyUp -= this.GlobalHookKeyUp;

        // m_GlobalHook.KeyDown -= HandleHorizontalInput;

        // It is recommened to dispose it
        this.GlobalHook.Dispose();
    }

    private void UpdateQuestDataForDisplay()
    {
        var category = this.DataLoader.Model.GetOverlayModeForStorage();
        var weaponType = this.DataLoader.Model.WeaponType();
        long questID = this.DataLoader.Model.QuestID();
        long partySize = this.DataLoader.Model.PartySize();
        long runBuffs = (long)this.DataLoader.Model.GetRunBuffs();

        var s = (Settings)Application.Current.TryFindResource("Settings");
        var completions = string.Empty;
        var attemptsPerPersonalBest = 0.0;

        var pbAttempts = DatabaseManagerInstance.UpsertPersonalBestAttempts(questID, weaponType, category, partySize, runBuffs);
        var questAttempts = DatabaseManagerInstance.UpsertQuestAttempts(questID, weaponType, category, partySize, runBuffs);

        if (s.EnableQuestCompletionsCounter)
        {
            completions = DatabaseManagerInstance.GetQuestCompletions(questID, category, weaponType, partySize, runBuffs) + "/";
        }

        if (s.EnableAttemptsPerPersonalBest)
        {
            attemptsPerPersonalBest = DatabaseManagerInstance.GetQuestAttemptsPerPersonalBest(questID, weaponType, category, questAttempts.ToString(CultureInfo.InvariantCulture), partySize, runBuffs);
        }

        // TODO putting this first before the others triggers "database is locked" error
        this.DataLoader.Model.PersonalBestLoaded = DatabaseManagerInstance.GetPersonalBest(questID, weaponType, category, ViewModels.Windows.AddressModel.QuestTimeMode, this.DataLoader, partySize, runBuffs);

        var _ = Dispatcher.BeginInvoke((Action)(() =>
        {
            this.questAttemptsTextBlock.Text = $"{completions}{questAttempts}";
            this.personalBestTextBlock.Text = this.DataLoader.Model.PersonalBestLoaded;
            this.personalBestAttemptsTextBlock.Text = s.EnableAttemptsPerPersonalBest ? string.Format(CultureInfo.InvariantCulture, "{0} ({1}/PB)", pbAttempts, Math.Truncate(attemptsPerPersonalBest)) : string.Format(CultureInfo.InvariantCulture, "{0}", pbAttempts);
        }));
    }

    /// <summary>
    /// Gets the mezeporta festival minigame score depending on area id.
    /// </summary>
    /// <param name="areaID"></param>
    /// <returns></returns>
    private int GetMezFesMinigameScore(int areaID)
    {
        // Read player score from corresponding memory address based on current area ID
        var score = 0;
        switch (areaID)
        {
            case 464: // Uruki Pachinko
                score = this.DataLoader.Model.UrukiPachinkoScore() + this.DataLoader.Model.UrukiPachinkoBonusScore();
                break;
            case 467: // Nyanrendo
                score = this.DataLoader.Model.NyanrendoScore();
                break;
            case 469: // Dokkan Battle Cats
                score = this.DataLoader.Model.DokkanBattleCatsScore();
                break;
            case 466: // Guuku Scoop
                score = this.DataLoader.Model.GuukuScoopScore();
                break;
            case 468: // Panic Honey
                score = this.DataLoader.Model.PanicHoneyScore();
                break;
            default:
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
        if (this.DataLoader.Model.QuestID() != 0 || !(this.DataLoader.Model.AreaID() == 462 || MezFesMinigames.ID.ContainsKey(this.DataLoader.Model.AreaID())))
        {
            return;
        }

        var areaID = this.DataLoader.Model.AreaID();

        // Check if player is in a minigame area
        if (MezFesMinigames.ID.ContainsKey(areaID))
        {
            // Check if the player has entered a new minigame area
            if (areaID != this.DataLoader.Model.PreviousMezFesArea)
            {
                this.DataLoader.Model.PreviousMezFesArea = areaID;
                this.DataLoader.Model.PreviousMezFesScore = 0;
            }

            // Read player score from corresponding memory address based on current area ID
            var score = this.GetMezFesMinigameScore(areaID);

            // Update current score with new score if it's greater and doesn't surpass the UI limit
            if (score > this.DataLoader.Model.PreviousMezFesScore && score <= 999999)
            {
                this.DataLoader.Model.PreviousMezFesScore = score;
            }
        }

        // Check if the player has exited a minigame area and the score is 0
        else if (this.DataLoader.Model.PreviousMezFesArea != -1 && areaID == 462)
        {
            // Save current score and minigame area ID to database
            DatabaseManagerInstance.InsertMezFesMinigameScore(this.DataLoader, this.DataLoader.Model.PreviousMezFesArea, this.DataLoader.Model.PreviousMezFesScore);

            // Reset previousMezFesArea and previousMezFesScore
            this.DataLoader.Model.PreviousMezFesArea = -1;
            this.DataLoader.Model.PreviousMezFesScore = 0;
        }
    }

    private bool CalculatedQuestDataForDisplay { get; set; }

    private bool DivaSongActive { get; set; }

    private bool GuildFoodActive { get; set; }


    // TODO: optimization
    private Task CheckQuestStateForDatabaseLogging()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        var playerAtk = 0;
        var success = int.TryParse(this.DataLoader.Model.ATK, NumberStyles.Integer, CultureInfo.InvariantCulture, out var playerTrueRaw);
        if (success)
        {
            playerAtk = playerTrueRaw;
        }
        else
        {
            LoggerInstance.Warn("Could not parse player true raw: {0}", this.DataLoader.Model.ATK);
        }

        // Check if in quest and timer is NOT frozen
        if (this.DataLoader.Model.QuestID() != 0 && this.DataLoader.Model.TimeInt() != this.DataLoader.Model.TimeDefInt() && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.PreviousTimeInt != this.DataLoader.Model.TimeInt())
        {
            this.DataLoader.Model.PreviousTimeInt = this.DataLoader.Model.TimeInt();
            this.DataLoader.Model.TotalHitsTakenBlockedPerSecond = this.DataLoader.Model.CalculateTotalHitsTakenBlockedPerSecond();
            this.DataLoader.Model.HitsPerSecond = this.DataLoader.Model.CalculateHitsPerSecond();
            this.DataLoader.Model.DPS = this.DataLoader.Model.CalculateDPS();
            this.DataLoader.Model.APM = this.DataLoader.Model.CalculateAPM();
            this.DataLoader.Model.InsertQuestInfoIntoDictionaries();

            // TODO: test on dure/etc
            if (!this.CalculatedQuestDataForDisplay
                && this.DataLoader.Model.TimeDefInt() > this.DataLoader.Model.TimeInt()
                && playerAtk > 0
                && this.DataLoader.Model.TimeDefInt() - this.DataLoader.Model.TimeInt() >= 30)
            {
                this.CalculatedQuestDataForDisplay = true;
                this.UpdateQuestDataForDisplay();
                this.DataLoader.Model.DivaSongActive = this.DataLoader.Model.DivaSongEnded ? false : true;
                this.DataLoader.Model.GuildFoodActive = this.DataLoader.Model.GuildFoodEnded ? false : true;
            }
        }

        if (this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.QuestID() == 0)
        {
            this.DataLoader.Model.QuestCleared = false;
            this.DataLoader.Model.QuestRewardsGiven = false;
            this.DataLoader.Model.ClearQuestInfoDictionaries();
            this.DataLoader.Model.ClearGraphCollections();
            this.DataLoader.Model.ResetQuestInfoVariables();
            this.DataLoader.Model.PreviousRoadFloor = 0;
            this.personalBestTextBlock.Text = Messages.TimerNotLoaded;
            this.CalculatedQuestDataForDisplay = false;
            // TODO test
            this.DataLoader.Model.DivaSongActive = this.DataLoader.Model.DivaSongEnded ? false : true;
            this.DataLoader.Model.GuildFoodActive = this.DataLoader.Model.GuildFoodEnded ? false : true;
            return Task.CompletedTask;
        }
        else if (!this.DataLoader.Model.LoadedItemsAtQuestStart && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.QuestID() != 0)
        {
            this.DataLoader.Model.LoadedItemsAtQuestStart = true;
            LoadInventoriesAtQuestStart();
        }

        if (this.DataLoader.Model.QuestState() == 0)
        {
            return Task.CompletedTask;
        }

        // check if quest clear
        if (this.DataLoader.Model.QuestState() == 1 && !this.DataLoader.Model.QuestCleared)
        {
            // TODO test on dure/road/etc
            // If this code is ever reached, it is not known if the cause is from the overlay interacting with the server,
            // the server itself, or just the overlay itself.
            // The overlay does NOT write to memory addresses.
            if (this.DataLoader.Model.TimeDefInt() - this.DataLoader.Model.TimeInt() == 0)
            {
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Illegal quest completion time [ID {0}]", this.DataLoader.Model.QuestID());
                ApplicationService.HandleGameShutdown();
                LoggingService.WriteCrashLog(new Exception($"Illegal quest completion time [ID {this.DataLoader.Model.QuestID()}]"));
            }

            this.DataLoader.Model.QuestCleared = true;
            this.DataLoader.Model.LoadedItemsAtQuestStart = false;
            if (s.EnableQuestLogging)
            {
                DatabaseManagerInstance.InsertQuestData(this.DataLoader, (int)DatabaseManagerInstance.GetQuestAttempts(this.DataLoader.Model.QuestID(), this.DataLoader.Model.WeaponType(), this.DataLoader.Model.GetOverlayModeForStorage(), (uint)this.DataLoader.Model.GetRunBuffs()));
            }
        }

        // check if rewards given
        if (this.DataLoader.Model.QuestState() == 129 && !this.DataLoader.Model.QuestRewardsGiven)
        {
            this.DataLoader.Model.QuestRewardsGiven = true;

            // TODO: add logging check requirement in case the user needs the hash sets.
            // We await since we are dealing with database?
            AchievementServiceInstance.CheckForAchievements(this.MainWindowSnackBarPresenter, this.DataLoader, DatabaseManagerInstance, s, (Style)this.FindResource("CatppuccinMochaSnackBar"));
        }

        return Task.CompletedTask;
    }

    private void LoadInventoriesAtQuestStart()
    {
        this.DataLoader.Model.PouchItem1IDAtQuestStart = this.DataLoader.Model.PouchItem1ID();
        this.DataLoader.Model.PouchItem2IDAtQuestStart = this.DataLoader.Model.PouchItem2ID();
        this.DataLoader.Model.PouchItem3IDAtQuestStart = this.DataLoader.Model.PouchItem3ID();
        this.DataLoader.Model.PouchItem4IDAtQuestStart = this.DataLoader.Model.PouchItem4ID();
        this.DataLoader.Model.PouchItem5IDAtQuestStart = this.DataLoader.Model.PouchItem5ID();
        this.DataLoader.Model.PouchItem6IDAtQuestStart = this.DataLoader.Model.PouchItem6ID();
        this.DataLoader.Model.PouchItem7IDAtQuestStart = this.DataLoader.Model.PouchItem7ID();
        this.DataLoader.Model.PouchItem8IDAtQuestStart = this.DataLoader.Model.PouchItem8ID();
        this.DataLoader.Model.PouchItem9IDAtQuestStart = this.DataLoader.Model.PouchItem9ID();
        this.DataLoader.Model.PouchItem10IDAtQuestStart = this.DataLoader.Model.PouchItem10ID();
        this.DataLoader.Model.PouchItem11IDAtQuestStart = this.DataLoader.Model.PouchItem11ID();
        this.DataLoader.Model.PouchItem12IDAtQuestStart = this.DataLoader.Model.PouchItem12ID();
        this.DataLoader.Model.PouchItem13IDAtQuestStart = this.DataLoader.Model.PouchItem13ID();
        this.DataLoader.Model.PouchItem14IDAtQuestStart = this.DataLoader.Model.PouchItem14ID();
        this.DataLoader.Model.PouchItem15IDAtQuestStart = this.DataLoader.Model.PouchItem15ID();
        this.DataLoader.Model.PouchItem16IDAtQuestStart = this.DataLoader.Model.PouchItem16ID();
        this.DataLoader.Model.PouchItem17IDAtQuestStart = this.DataLoader.Model.PouchItem17ID();
        this.DataLoader.Model.PouchItem18IDAtQuestStart = this.DataLoader.Model.PouchItem18ID();
        this.DataLoader.Model.PouchItem19IDAtQuestStart = this.DataLoader.Model.PouchItem19ID();
        this.DataLoader.Model.PouchItem20IDAtQuestStart = this.DataLoader.Model.PouchItem20ID();
        this.DataLoader.Model.PouchItem1QuantityAtQuestStart = this.DataLoader.Model.PouchItem1Qty();
        this.DataLoader.Model.PouchItem2QuantityAtQuestStart = this.DataLoader.Model.PouchItem2Qty();
        this.DataLoader.Model.PouchItem3QuantityAtQuestStart = this.DataLoader.Model.PouchItem3Qty();
        this.DataLoader.Model.PouchItem4QuantityAtQuestStart = this.DataLoader.Model.PouchItem4Qty();
        this.DataLoader.Model.PouchItem5QuantityAtQuestStart = this.DataLoader.Model.PouchItem5Qty();
        this.DataLoader.Model.PouchItem6QuantityAtQuestStart = this.DataLoader.Model.PouchItem6Qty();
        this.DataLoader.Model.PouchItem7QuantityAtQuestStart = this.DataLoader.Model.PouchItem7Qty();
        this.DataLoader.Model.PouchItem8QuantityAtQuestStart = this.DataLoader.Model.PouchItem8Qty();
        this.DataLoader.Model.PouchItem9QuantityAtQuestStart = this.DataLoader.Model.PouchItem9Qty();
        this.DataLoader.Model.PouchItem10QuantityAtQuestStart = this.DataLoader.Model.PouchItem10Qty();
        this.DataLoader.Model.PouchItem11QuantityAtQuestStart = this.DataLoader.Model.PouchItem11Qty();
        this.DataLoader.Model.PouchItem12QuantityAtQuestStart = this.DataLoader.Model.PouchItem12Qty();
        this.DataLoader.Model.PouchItem13QuantityAtQuestStart = this.DataLoader.Model.PouchItem13Qty();
        this.DataLoader.Model.PouchItem14QuantityAtQuestStart = this.DataLoader.Model.PouchItem14Qty();
        this.DataLoader.Model.PouchItem15QuantityAtQuestStart = this.DataLoader.Model.PouchItem15Qty();
        this.DataLoader.Model.PouchItem16QuantityAtQuestStart = this.DataLoader.Model.PouchItem16Qty();
        this.DataLoader.Model.PouchItem17QuantityAtQuestStart = this.DataLoader.Model.PouchItem17Qty();
        this.DataLoader.Model.PouchItem18QuantityAtQuestStart = this.DataLoader.Model.PouchItem18Qty();
        this.DataLoader.Model.PouchItem19QuantityAtQuestStart = this.DataLoader.Model.PouchItem19Qty();
        this.DataLoader.Model.PouchItem20QuantityAtQuestStart = this.DataLoader.Model.PouchItem20Qty();

        this.DataLoader.Model.AmmoPouchItem1IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem2IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem3IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem4IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem5IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem6IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem7IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem8IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem9IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem10IDAtQuestStart = this.DataLoader.Model.AmmoPouchItem1ID();
        this.DataLoader.Model.AmmoPouchItem1QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();
        this.DataLoader.Model.AmmoPouchItem2QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();
        this.DataLoader.Model.AmmoPouchItem3QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();
        this.DataLoader.Model.AmmoPouchItem4QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();
        this.DataLoader.Model.AmmoPouchItem5QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();
        this.DataLoader.Model.AmmoPouchItem6QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();
        this.DataLoader.Model.AmmoPouchItem7QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();
        this.DataLoader.Model.AmmoPouchItem8QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();
        this.DataLoader.Model.AmmoPouchItem9QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();
        this.DataLoader.Model.AmmoPouchItem10QuantityAtQuestStart = this.DataLoader.Model.AmmoPouchItem1Qty();

        this.DataLoader.Model.PartnyaBagItem1IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem2IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem3IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem4IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem5IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem6IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem7IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem8IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem9IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem10IDAtQuestStart = this.DataLoader.Model.PartnyaBagItem1ID();
        this.DataLoader.Model.PartnyaBagItem1QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
        this.DataLoader.Model.PartnyaBagItem2QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
        this.DataLoader.Model.PartnyaBagItem3QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
        this.DataLoader.Model.PartnyaBagItem4QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
        this.DataLoader.Model.PartnyaBagItem5QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
        this.DataLoader.Model.PartnyaBagItem6QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
        this.DataLoader.Model.PartnyaBagItem7QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
        this.DataLoader.Model.PartnyaBagItem8QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
        this.DataLoader.Model.PartnyaBagItem9QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
        this.DataLoader.Model.PartnyaBagItem10QuantityAtQuestStart = this.DataLoader.Model.PartnyaBagItem1Qty();
    }

    private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
    {
        // goodbye world
    }

    private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
    {
        if (this.mouseImages.TryGetValue(e.Button, out var image))
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.DataLoader.Model.MouseInputDictionary.ContainsKey(this.DataLoader.Model.TimeInt()) && this.DataLoader.Model.QuestID() != 0 && this.DataLoader.Model.TimeInt() != this.DataLoader.Model.TimeDefInt() && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.PreviousTimeInt != this.DataLoader.Model.TimeInt() && image.Opacity == s.PlayerInputUnpressedOpacity)
            {
                try
                {
                    this.DataLoader.Model.MouseInputDictionary.Add(this.DataLoader.Model.TimeInt(), e.Button.ToString());
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into mouseInputDictionary");
                }
            }

            this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputPressedOpacity));
        }

        // uncommenting the following line will suppress the middle mouse button click
        // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
    }

    private void GlobalHookMouseUpExt(object sender, MouseEventExtArgs e)
    {
        if (this.mouseImages.TryGetValue(e.Button, out var image))
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputUnpressedOpacity));
        }
    }

    // TODO: its finicky
    private void GlobalHookKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        if (this.keyImages.TryGetValue(e.KeyCode, out var image))
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.DataLoader.Model.KeystrokesDictionary.ContainsKey(this.DataLoader.Model.TimeInt()) && this.DataLoader.Model.QuestID() != 0 && this.DataLoader.Model.TimeInt() != this.DataLoader.Model.TimeDefInt() && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.PreviousTimeInt != this.DataLoader.Model.TimeInt() && image.Opacity == s.PlayerInputUnpressedOpacity)
            {
                try
                {
                    this.DataLoader.Model.KeystrokesDictionary.Add(this.DataLoader.Model.TimeInt(), e.KeyCode.ToString());
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into keystrokesDictionary");
                }
            }

            this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputPressedOpacity));
        }
    }

    private void GlobalHookKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        if (this.keyImages.TryGetValue(e.KeyCode, out var image))
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputUnpressedOpacity));
        }
    }

    /// <summary>
    /// Maps the player input to images.
    /// </summary>
    private void MapPlayerInputImages()
    {
        // Add the key-image pairs to the dictionary
        this.keyImages.Add(Keys.D1, this.Key1);
        this.keyImages.Add(Keys.D2, this.Key2);
        this.keyImages.Add(Keys.D3, this.Key3);
        this.keyImages.Add(Keys.D4, this.Key4);
        this.keyImages.Add(Keys.D5, this.Key5);
        this.keyImages.Add(Keys.D6, this.Key6);
        this.keyImages.Add(Keys.Q, this.KeyQ);
        this.keyImages.Add(Keys.W, this.KeyW);
        this.keyImages.Add(Keys.E, this.KeyE);
        this.keyImages.Add(Keys.R, this.KeyR);
        this.keyImages.Add(Keys.T, this.KeyT);
        this.keyImages.Add(Keys.Y, this.KeyY);

        this.keyImages.Add(Keys.A, this.KeyA);
        this.keyImages.Add(Keys.S, this.KeyS);
        this.keyImages.Add(Keys.D, this.KeyD);
        this.keyImages.Add(Keys.F, this.KeyF);
        this.keyImages.Add(Keys.G, this.KeyG);
        this.keyImages.Add(Keys.H, this.KeyH);

        this.keyImages.Add(Keys.LShiftKey, this.KeyShift);
        this.keyImages.Add(Keys.Z, this.KeyZ);
        this.keyImages.Add(Keys.X, this.KeyX);
        this.keyImages.Add(Keys.C, this.KeyC);
        this.keyImages.Add(Keys.V, this.KeyV);
        this.keyImages.Add(Keys.B, this.KeyB);

        this.keyImages.Add(Keys.LControlKey, this.KeyCtrl);
        this.keyImages.Add(Keys.Alt, this.KeyAlt);
        this.keyImages.Add(Keys.Space, this.KeySpace);

        this.mouseImages.Add(MouseButtons.Left, this.MouseLeftClick);
        this.mouseImages.Add(MouseButtons.Middle, this.MouseMiddleClick);
        this.mouseImages.Add(MouseButtons.Right, this.MouseRightClick);

        var s = (Settings)Application.Current.TryFindResource("Settings");
        Key1.Opacity = s.PlayerInputUnpressedOpacity;
        Key2.Opacity = s.PlayerInputUnpressedOpacity;
        Key3.Opacity = s.PlayerInputUnpressedOpacity;
        Key4.Opacity = s.PlayerInputUnpressedOpacity;
        Key5.Opacity = s.PlayerInputUnpressedOpacity;
        Key6.Opacity = s.PlayerInputUnpressedOpacity;
        KeyQ.Opacity = s.PlayerInputUnpressedOpacity;
        KeyW.Opacity = s.PlayerInputUnpressedOpacity;
        KeyE.Opacity = s.PlayerInputUnpressedOpacity;
        KeyR.Opacity = s.PlayerInputUnpressedOpacity;
        KeyT.Opacity = s.PlayerInputUnpressedOpacity;
        KeyY.Opacity = s.PlayerInputUnpressedOpacity;
        KeyA.Opacity = s.PlayerInputUnpressedOpacity;
        KeyS.Opacity = s.PlayerInputUnpressedOpacity;
        KeyD.Opacity = s.PlayerInputUnpressedOpacity;
        KeyF.Opacity = s.PlayerInputUnpressedOpacity;
        KeyG.Opacity = s.PlayerInputUnpressedOpacity;
        KeyH.Opacity = s.PlayerInputUnpressedOpacity;
        KeyShift.Opacity = s.PlayerInputUnpressedOpacity;
        KeyZ.Opacity = s.PlayerInputUnpressedOpacity;
        KeyX.Opacity = s.PlayerInputUnpressedOpacity;
        KeyC.Opacity = s.PlayerInputUnpressedOpacity;
        KeyV.Opacity = s.PlayerInputUnpressedOpacity;
        KeyB.Opacity = s.PlayerInputUnpressedOpacity;
        KeyCtrl.Opacity = s.PlayerInputUnpressedOpacity;
        KeyAlt.Opacity = s.PlayerInputUnpressedOpacity;
        KeySpace.Opacity = s.PlayerInputUnpressedOpacity;
        MouseMiddleClick.Opacity = s.PlayerInputUnpressedOpacity;
        MouseLeftClick.Opacity = s.PlayerInputUnpressedOpacity;
        MouseRightClick.Opacity = s.PlayerInputUnpressedOpacity;
    }

    private void AddGamepadImages()
    {
        LoggerInstance.Debug("Adding images. images count: {0}, triggers count: {1}, joystick count: {2}", this.gamepadImages.Count, this.gamepadTriggersImages.Count, this.gamepadJoystickImages.Count);
        this.gamepadImages.Add(this.gamepad.Buttons.A, this.ButtonA);
        this.gamepadImages.Add(this.gamepad.Buttons.B, this.ButtonB);
        this.gamepadImages.Add(this.gamepad.Buttons.X, this.ButtonX);
        this.gamepadImages.Add(this.gamepad.Buttons.Y, this.ButtonY);
        this.gamepadImages.Add(this.gamepad.Buttons.Start, this.ButtonStart);
        this.gamepadImages.Add(this.gamepad.Buttons.Back, this.ButtonSelect);
        this.gamepadImages.Add(this.gamepad.Buttons.LS, this.LJoystick);
        this.gamepadImages.Add(this.gamepad.Buttons.RS, this.RJoystick);
        this.gamepadImages.Add(this.gamepad.Buttons.LB, this.ButtonL1);
        this.gamepadImages.Add(this.gamepad.Buttons.RB, this.ButtonR1);
        this.gamepadTriggersImages.Add(this.gamepad.LeftTrigger, this.ButtonL2);
        this.gamepadTriggersImages.Add(this.gamepad.RightTrigger, this.ButtonR2);
        this.gamepadJoystickImages.Add(this.gamepad.LeftJoystick, this.LJoystickMovement);
        this.gamepadJoystickImages.Add(this.gamepad.RightJoystick, this.RJoystickMovement);
        var s = (Settings)Application.Current.TryFindResource("Settings");

        this.ButtonA.Opacity = s.PlayerInputUnpressedOpacity;
        this.ButtonB.Opacity = s.PlayerInputUnpressedOpacity;
        this.ButtonX.Opacity = s.PlayerInputUnpressedOpacity;
        this.ButtonY.Opacity = s.PlayerInputUnpressedOpacity;
        this.ButtonStart.Opacity = s.PlayerInputUnpressedOpacity;
        this.ButtonSelect.Opacity = s.PlayerInputUnpressedOpacity;
        this.LJoystick.Opacity = s.PlayerInputUnpressedOpacity;
        this.RJoystick.Opacity = s.PlayerInputUnpressedOpacity;
        this.ButtonL1.Opacity = s.PlayerInputUnpressedOpacity;
        this.ButtonR1.Opacity = s.PlayerInputUnpressedOpacity;
        this.ButtonL2.Opacity = s.PlayerInputUnpressedOpacity;
        this.ButtonR2.Opacity = s.PlayerInputUnpressedOpacity;
        this.LJoystickMovement.Opacity = s.PlayerInputUnpressedOpacity;
        this.RJoystickMovement.Opacity = s.PlayerInputUnpressedOpacity;

        LoggerInstance.Debug(CultureInfo.InvariantCulture, "Added images. images count: {0}, triggers count: {1}, joystick count: {2}", this.gamepadImages.Count, this.gamepadTriggersImages.Count, this.gamepadJoystickImages.Count);
    }

    private float TriggerActivationThreshold { get; set; } = 0.5f;

    private float JoystickThreshold { get; set; } = 0.5f;

    private Process? MhfProcess { get; set; }

    private void Gamepad_RightTriggerReleased(object? sender, EventArgs e)
    {
        if (this.gamepadTriggersImages.TryGetValue(this.gamepad.RightTrigger, out var image))
        {var s = (Settings)Application.Current.TryFindResource("Settings");
            this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputUnpressedOpacity));
        }
    }

    private void Gamepad_LeftTriggerReleased(object? sender, EventArgs e)
    {
        if (this.gamepadTriggersImages.TryGetValue(this.gamepad.LeftTrigger, out var image))
        {var s = (Settings)Application.Current.TryFindResource("Settings");
            this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputUnpressedOpacity));
        }
    }

    private void Gamepad_ButtonReleased(object? sender, DigitalButtonEventArgs<XInputButton> e)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        if (e.Button == this.gamepad.Buttons.DPadLeft || e.Button == this.gamepad.Buttons.DPadUp || e.Button == this.gamepad.Buttons.DPadRight || e.Button == this.gamepad.Buttons.DPadDown)
        {

            this.Dispatcher.BeginInvoke(new Action(() => this.UpdateDpadImage(s.PlayerInputUnpressedOpacity)));
        }
        else if (e.Button == this.gamepad.Buttons.LS)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.UpdateLeftStickImage(s.PlayerInputUnpressedOpacity)));
        }
        else if (e.Button == this.gamepad.Buttons.RS)
        {
            this.Dispatcher.BeginInvoke(new Action(() => this.UpdateRightStickImage(s.PlayerInputUnpressedOpacity)));
        }
        else if (this.gamepadImages.TryGetValue(e.Button, out var image))
        {
            this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputUnpressedOpacity));
        }
    }

    private void Gamepad_RightTriggerPressed(object? sender, EventArgs e)
    {
        if (this.gamepadTriggersImages.TryGetValue(this.gamepad.RightTrigger, out var image))
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.DataLoader.Model.GamepadInputDictionary.ContainsKey(this.DataLoader.Model.TimeInt()) && this.DataLoader.Model.QuestID() != 0 && this.DataLoader.Model.TimeInt() != this.DataLoader.Model.TimeDefInt() && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.PreviousTimeInt != this.DataLoader.Model.TimeInt() && image.Opacity == s.PlayerInputUnpressedOpacity)
            {
                try
                {
                    this.DataLoader.Model.GamepadInputDictionary.Add(this.DataLoader.Model.TimeInt(), $"R2 {this.gamepad.RightTrigger}");
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }

            this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputPressedOpacity));
        }
    }

    private void Gamepad_LeftTriggerPressed(object? sender, EventArgs e)
    {
        if (this.gamepadTriggersImages.TryGetValue(this.gamepad.LeftTrigger, out var image))
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.DataLoader.Model.GamepadInputDictionary.ContainsKey(this.DataLoader.Model.TimeInt()) && this.DataLoader.Model.QuestID() != 0 && this.DataLoader.Model.TimeInt() != this.DataLoader.Model.TimeDefInt() && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.PreviousTimeInt != this.DataLoader.Model.TimeInt() && image.Opacity == s.PlayerInputUnpressedOpacity)
            {
                try
                {
                    this.DataLoader.Model.GamepadInputDictionary.Add(this.DataLoader.Model.TimeInt(), $"L2 {this.gamepad.LeftTrigger}");
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }

            this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputPressedOpacity));
        }
    }

    private void Gamepad_LeftJoystickMove(object? sender, EventArgs e)
    {
        var direction = this.UpdateLeftJoystickImage();
        if (direction != Direction.None)
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.DataLoader.Model.GamepadInputDictionary.ContainsKey(this.DataLoader.Model.TimeInt()) && this.DataLoader.Model.QuestID() != 0 && this.DataLoader.Model.TimeInt() != this.DataLoader.Model.TimeDefInt() && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.PreviousTimeInt != this.DataLoader.Model.TimeInt())
            {
                try
                {
                    this.DataLoader.Model.GamepadInputDictionary.Add(this.DataLoader.Model.TimeInt(), $"LS {direction}");
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }
        }
    }

    /// <summary>
    /// Updates the left joystick image.
    /// </summary>
    /// <returns>The direction of the joystick.</returns>
    private Direction UpdateLeftJoystickImage()
    {
        // Get the joystick's X and Y positions
        var x = this.gamepad.LeftJoystick.X;
        var y = this.gamepad.LeftJoystick.Y;
        var s = (Settings)Application.Current.TryFindResource("Settings");

        var opacity = s.PlayerInputPressedOpacity;


        // Calculate the joystick direction based on X and Y values
        Direction direction;
        if (Math.Abs(x) <= this.JoystickThreshold && Math.Abs(y) <= this.JoystickThreshold)
        {
            direction = Direction.None;
            opacity = s.PlayerInputUnpressedOpacity;
        }
        else if (Math.Abs(x) <= this.JoystickThreshold && y > this.JoystickThreshold)
        {
            direction = Direction.Up;
        }
        else if (x > this.JoystickThreshold && y > this.JoystickThreshold)
        {
            direction = Direction.UpRight;
        }
        else if (x > this.JoystickThreshold && Math.Abs(y) <= this.JoystickThreshold)
        {
            direction = Direction.Right;
        }
        else if (x > this.JoystickThreshold && y < -this.JoystickThreshold)
        {
            direction = Direction.DownRight;
        }
        else if (Math.Abs(x) <= this.JoystickThreshold && y < -this.JoystickThreshold)
        {
            direction = Direction.Down;
        }
        else if (x < -this.JoystickThreshold && y < -this.JoystickThreshold)
        {
            direction = Direction.DownLeft;
        }
        else if (x < -this.JoystickThreshold && Math.Abs(y) <= this.JoystickThreshold)
        {
            direction = Direction.Left;
        }
        else if (x < -this.JoystickThreshold && y > this.JoystickThreshold)
        {
            direction = Direction.UpLeft;
        }
        else
        {
            direction = Direction.None;
            opacity = s.PlayerInputUnpressedOpacity;
        }

        // Get the image path based on the direction
        var imagePath = JoystickImages.GetImage(direction);

        // Get the current image source of the left joystick
        var currentImageSource = this.LJoystickMovement.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the left joystick
            this.LJoystickMovement.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            this.LJoystickMovement.Opacity = opacity;
        }

        return direction;
    }

    private void Gamepad_RightJoystickMove(object? sender, EventArgs e)
    {
        var direction = this.UpdateRightJoystickImage();
        if (direction != Direction.None)
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.DataLoader.Model.GamepadInputDictionary.ContainsKey(this.DataLoader.Model.TimeInt()) && this.DataLoader.Model.QuestID() != 0 && this.DataLoader.Model.TimeInt() != this.DataLoader.Model.TimeDefInt() && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.PreviousTimeInt != this.DataLoader.Model.TimeInt())
            {
                try
                {
                    this.DataLoader.Model.GamepadInputDictionary.Add(this.DataLoader.Model.TimeInt(), $"RS {direction}");
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }
        }
    }

    /// <summary>
    /// Updates the right joystick image.
    /// </summary>
    /// <returns>The direction of the joystick.</returns>
    private Direction UpdateRightJoystickImage()
    {
        // Get the joystick's X and Y positions
        var x = this.gamepad.RightJoystick.X;
        var y = this.gamepad.RightJoystick.Y;
        var s = (Settings)Application.Current.TryFindResource("Settings");

        var opacity = s.PlayerInputPressedOpacity;

        // Calculate the joystick direction based on X and Y values
        Direction direction;
        if (Math.Abs(x) <= this.JoystickThreshold && Math.Abs(y) <= this.JoystickThreshold)
        {
            direction = Direction.None;
            opacity = s.PlayerInputUnpressedOpacity;
        }
        else if (Math.Abs(x) <= this.JoystickThreshold && y > this.JoystickThreshold)
        {
            direction = Direction.Up;
        }
        else if (x > this.JoystickThreshold && y > this.JoystickThreshold)
        {
            direction = Direction.UpRight;
        }
        else if (x > this.JoystickThreshold && Math.Abs(y) <= this.JoystickThreshold)
        {
            direction = Direction.Right;
        }
        else if (x > this.JoystickThreshold && y < -this.JoystickThreshold)
        {
            direction = Direction.DownRight;
        }
        else if (Math.Abs(x) <= this.JoystickThreshold && y < -this.JoystickThreshold)
        {
            direction = Direction.Down;
        }
        else if (x < -this.JoystickThreshold && y < -this.JoystickThreshold)
        {
            direction = Direction.DownLeft;
        }
        else if (x < -this.JoystickThreshold && Math.Abs(y) <= this.JoystickThreshold)
        {
            direction = Direction.Left;
        }
        else if (x < -this.JoystickThreshold && y > this.JoystickThreshold)
        {
            direction = Direction.UpLeft;
        }
        else
        {
            direction = Direction.None;
            opacity = s.PlayerInputUnpressedOpacity;
        }

        // Get the image path based on the direction
        var imagePath = JoystickImages.GetImage(direction);

        // Get the current image source of the left joystick
        var currentImageSource = this.RJoystickMovement.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the left joystick
            this.RJoystickMovement.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            this.RJoystickMovement.Opacity = opacity;
        }

        return direction;
    }

    private void Gamepad_DPadPressed(XInputButton button)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        if (s.EnableInputLogging && !this.DataLoader.Model.GamepadInputDictionary.ContainsKey(this.DataLoader.Model.TimeInt()) && this.DataLoader.Model.QuestID() != 0 && this.DataLoader.Model.TimeInt() != this.DataLoader.Model.TimeDefInt() && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.PreviousTimeInt != this.DataLoader.Model.TimeInt() && this.DPad.Opacity == s.PlayerInputUnpressedOpacity)
        {
            try
            {
                this.DataLoader.Model.GamepadInputDictionary.Add(this.DataLoader.Model.TimeInt(), button.ToString());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary (Gamepad_DPadPressed)");
            }
        }

        this.Dispatcher.BeginInvoke(new Action(() => this.UpdateDpadImage(s.PlayerInputPressedOpacity)));
    }

    private void Gamepad_ButtonPressed(object? sender, DigitalButtonEventArgs<XInputButton> e)
    {
        if (e.Button == this.gamepad.Buttons.DPadLeft || e.Button == this.gamepad.Buttons.DPadUp || e.Button == this.gamepad.Buttons.DPadRight || e.Button == this.gamepad.Buttons.DPadDown)
        {
            this.Gamepad_DPadPressed(e.Button);
            return;
        }

        if (this.gamepadImages.TryGetValue(e.Button, out var image))
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.EnableInputLogging && !this.DataLoader.Model.GamepadInputDictionary.ContainsKey(this.DataLoader.Model.TimeInt()) && this.DataLoader.Model.QuestID() != 0 && this.DataLoader.Model.TimeInt() != this.DataLoader.Model.TimeDefInt() && this.DataLoader.Model.QuestState() == 0 && this.DataLoader.Model.PreviousTimeInt != this.DataLoader.Model.TimeInt() && image.Opacity == s.PlayerInputUnpressedOpacity)
            {
                try
                {
                    this.DataLoader.Model.GamepadInputDictionary.Add(this.DataLoader.Model.TimeInt(), e.Button.ToString());
                }
                catch (Exception ex)
                {
                    LoggerInstance.Warn(ex, "Could not insert into gamepadInputDictionary");
                }
            }

            if (e.Button == this.gamepad.Buttons.LS)
            {
                this.Dispatcher.BeginInvoke(new Action(() => this.UpdateLeftStickImage(s.PlayerInputPressedOpacity)));
            }
            else if (e.Button == this.gamepad.Buttons.RS)
            {
                this.Dispatcher.BeginInvoke(new Action(() => this.UpdateRightStickImage(s.PlayerInputPressedOpacity)));
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() => image.Opacity = s.PlayerInputPressedOpacity));
            }
        }
    }

    private void UpdateRightStickImage(double opacity)
    {
        // Get the image path based on the direction
        var imagePath = JoystickImages.GetImage(Direction.None);

        // Get the current image source of the D-pad
        var currentImageSource = this.RJoystick.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the D-pad
            this.RJoystick.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        this.RJoystick.Opacity = opacity;
    }

    private void UpdateLeftStickImage(double opacity)
    {
        // Get the image path based on the direction
        var imagePath = JoystickImages.GetImage(Direction.None);

        // Get the current image source of the D-pad
        var currentImageSource = this.LJoystick.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the D-pad
            this.LJoystick.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        this.LJoystick.Opacity = opacity;
    }

    private void UpdateDpadImage(double opacity)
    {
        // Determine the D-pad direction based on the button states
        Direction direction;
        if (this.gamepad.Buttons.DPadUp.IsPressed)
        {
            direction = Direction.Up;
        }
        else if (this.gamepad.Buttons.DPadDown.IsPressed)
        {
            direction = Direction.Down;
        }
        else if (this.gamepad.Buttons.DPadLeft.IsPressed)
        {
            direction = Direction.Left;
        }
        else if (this.gamepad.Buttons.DPadRight.IsPressed)
        {
            direction = Direction.Right;
        }
        else
        {
            direction = Direction.None;
        }

        // Get the image path based on the direction
        var imagePath = DPadImages.GetImage(direction);

        // Get the current image source of the D-pad
        var currentImageSource = this.DPad.Source as BitmapImage;

        // Compare the current image path with the new image path
        if (currentImageSource?.UriSource?.OriginalString != imagePath)
        {
            // Set the new image source for the D-pad
            this.DPad.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        this.DPad.Opacity = opacity;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (Program.WasFirstRun)
        {
            OnboardEndUser();
        }

        if (this.DataLoader.LoadedOutsideMezeporta)
        {
            var snackbar = new Snackbar(this.MainWindowSnackBarPresenter)
            {
                Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
                Title = Messages.WarningTitle,
                Content = "It is not recommended to load the overlay outside of Mezeporta",
                Icon = new SymbolIcon(SymbolRegular.Warning28),
                Appearance = ControlAppearance.Caution,
                Timeout = this.MainWindowSnackbarTimeOut,
            };
            snackbar.Show();
        }

        DatabaseManagerInstance.LoadDatabaseDataIntoHashSets(this.SaveIconGrid, this.DataLoader);
        AchievementServiceInstance.LoadPlayerAchievements();

        this.MhfProcess = Process.GetProcessesByName("mhf").First();

        if (this.MhfProcess == null)
        {
            LoggingService.WriteCrashLog(new Exception("Target process not found"));
        }

        MainWindowMediaPlayer = new MediaPlayer();
    }

    private static void OnboardEndUser()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        var result = System.Windows.MessageBox.Show("Would you like to quickly set the settings?", Messages.InfoTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);

        if (result == MessageBoxResult.Yes)
        {
            var settingsForm = new SettingsForm();
            var settingsFormResult = settingsForm.ShowDialog();
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
                    OverlaySettingsService.SetConfigurationPreset(s, ConfigurationPresetConverter.Convert("hp only"));
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
                    OverlaySettingsService.SetConfigurationPreset(s, ConfigurationPresetConverter.Convert("speedrun"));
                }
                else if (settingsForm.IsEverythingSelected)
                {
                    resultSelected = "All";
                    OverlaySettingsService.SetConfigurationPreset(s, ConfigurationPresetConverter.Convert("all"));
                }
                else
                {
                    return;
                }

                s.Save();
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Onboarded end-user. Result selected: {0}", resultSelected);
                System.Windows.MessageBox.Show("Settings set. Happy hunting!", Messages.InfoTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (settingsFormResult == false)
            {
                // User closed the window without making any selections
                // Handle this scenario as needed (e.g., use default settings, display a message, etc.)
                return;
            }
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
