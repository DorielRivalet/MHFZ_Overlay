// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

// TODO: all of this needs testing
namespace MHFZ_Overlay;

using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Services;
using Squirrel;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App : Application
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    public static bool IsClowdSquirrelUpdating { get; set; }

    /// <summary>
    /// Gets the current program version. TODO: put in env var.
    /// </summary>
    public static string? CurrentProgramVersion { get; private set; }

    public static bool IsFirstRun { get; set; }

    private static string GetAssemblyVersion
    {
        get
        {
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var versionString = assemblyVersion != null
                ? $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}"
                : "0.0.0";

            return versionString;
        }
    }

    public static async Task UpdateMyApp()
    {
        IsClowdSquirrelUpdating = true;
        var splashScreen = new SplashScreen("./Assets/Icons/png/loading.png");
        splashScreen.Show(false);
        try
        {
            using var mgr = new GithubUpdateManager(@"https://github.com/DorielRivalet/mhfz-overlay");
            var newVersion = await mgr.UpdateApp();
            BackupSettings();

            // optionally restart the app automatically, or ask the user if/when they want to restart
            if (newVersion != null)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Overlay has been updated, restarting application.");
                splashScreen.Close(TimeSpan.FromSeconds(0.1));
                MessageBox.Show(
@"【MHF-Z】Overlay has been updated, restarting application.

If after overlay startup your settings did not transfer over, try restarting the overlay again without saving or changing any settings. Alternatively, find the old settings file by going into the parent folder when clicking the settings folder option.", "MHF-Z Overlay Update", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateManager.RestartApp();
            }
            else
            {
                Logger.Error(CultureInfo.InvariantCulture, "No updates available.");
                IsClowdSquirrelUpdating = false;
                splashScreen.Close(TimeSpan.FromSeconds(0.1));
                MessageBox.Show("No updates available. If you want to check for updates manually, visit the GitHub repository at https://github.com/DorielRivalet/mhfz-overlay.", "MHF-Z Overlay Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            IsClowdSquirrelUpdating = false;
            splashScreen.Close(TimeSpan.FromSeconds(0.1));
            MessageBox.Show("An error has occurred with the update process, see logs.log for more information", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <inheritdoc/>
    protected override void OnStartup(StartupEventArgs e)
    {
        // Create a Stopwatch instance
        var stopwatch = new Stopwatch();

        // Start the stopwatch
        stopwatch.Start();

        // https://stackoverflow.com/questions/12729922/how-to-set-cultureinfo-invariantculture-default
        CultureInfo culture = CultureInfo.InvariantCulture;

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name)));

        var loggingRules = NLog.LogManager.Configuration.LoggingRules;
        var s = (Settings)Current.TryFindResource("Settings");
        loggingRules[0].SetLoggingLevels(LoggingService.GetLogLevel(s.LogLevel), NLog.LogLevel.Fatal);
        Logger.Info(CultureInfo.InvariantCulture, "Started WPF application");
        Logger.Trace(CultureInfo.InvariantCulture, "Call stack: {0}", new StackTrace().ToString());
        Logger.Debug(CultureInfo.InvariantCulture, "OS: {0}, is64BitOS: {1}, is64BitProcess: {2}, CLR version: {3}", Environment.OSVersion, Environment.Is64BitOperatingSystem, Environment.Is64BitProcess, Environment.Version);

        // TODO: test if this doesnt conflict with squirrel update
        CurrentProgramVersion = $"v{GetAssemblyVersion}";
        if (CurrentProgramVersion == "v0.0.0")
        {
            Logger.Fatal(CultureInfo.InvariantCulture, "Program version not found");
            MessageBox.Show("Program version not found", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            LoggingService.WriteCrashLog(new Exception("Program version not found"));
        }
        else
        {
            Logger.Info(CultureInfo.InvariantCulture, "Found program version {0}", CurrentProgramVersion);
        }

        // run Squirrel first, as the app may exit after these run
        SquirrelAwareApp.HandleEvents(
            onInitialInstall: OnAppInstall,
            onAppUninstall: OnAppUninstall,
            onEveryRun: OnAppRun,
            onAppUpdate: this.OnAppUpdate);

        // ... other app init code after ...
        RestoreSettings();
        Settings.Default.Reload();
        Logger.Info(CultureInfo.InvariantCulture, "Reloaded default settings");

        this.DispatcherUnhandledException += App_DispatcherUnhandledException;

        // Stop the stopwatch
        stopwatch.Stop();

        // Get the elapsed time in milliseconds
        var elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        base.OnStartup(e);
        SetRenderingMode(s.RenderingMode);

        // Print the elapsed time
        Logger.Debug($"App ctor Elapsed Time: {elapsedTimeMs} ms");
    }

    private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) =>

        // Log/inspect the inspection here
        Logger.Error(CultureInfo.InvariantCulture, "Unhandled exception\n\nMessage: {0}\n\nStack Trace: {1}\n\nHelp Link: {2}\n\nHResult: {3}\n\nSource: {4}\n\nTarget Site: {5}", e.Exception.Message, e.Exception.StackTrace, e.Exception.HelpLink, e.Exception.HResult, e.Exception.Source, e.Exception.TargetSite);

    private static void SetRenderingMode(string renderingMode)
    {
        RenderOptions.ProcessRenderMode = renderingMode == "Hardware"
            ? RenderMode.Default
            : RenderMode.SoftwareOnly;
        Logger.Info(CultureInfo.InvariantCulture, $"Rendering mode: {renderingMode}");
    }

    // https://github.com/Squirrel/Squirrel.Windows/issues/198#issuecomment-299262613
    // Indeed you can use the methods below to backup your settings,
    // typically just after your update has completed,
    // so just after your call to await mgr . UpdateApp();
    // You want to restore them at the very beginning of your program,
    // like just after Squirrel's event handler registration.
    // Don't try doing a restore from the onAppUpdate it won't work.
    // By the look of it onAppUpdate is executing from the older app process
    // as it would not get access to the newer app data folder.

    /// <summary>
    /// Make a backup of our settings.
    /// Used to persist settings across updates.
    /// </summary>
    private static void BackupSettings()
    {
        var settingsFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
        var destination = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\last.config";
        FileService.CopyFileToDestination(settingsFile, destination, true, "Backed up settings", true);
    }

    /// <summary>
    /// Restore our settings backup if any.
    /// Used to persist settings across updates.
    /// </summary>
    private static void RestoreSettings()
    {
        // Restore settings after application update
        var destFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
        var sourceFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\last.config";
        var restorationMessage = "Restored settings";
        Logger.Info(CultureInfo.InvariantCulture, "Restore our settings backup if any. Destination: {0}. Source: {1}", destFile, sourceFile);
        FileService.RestoreFileFromSourceToDestination(destFile, sourceFile, restorationMessage);
    }

    /// <summary>
    /// Called when [application install].
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="tools">The tools.</param>
    private static void OnAppInstall(SemanticVersion version, IAppTools tools)
    {
        Logger.Info(CultureInfo.InvariantCulture, "Created overlay shortcut");
        MessageBox.Show("【MHF-Z】Overlay is now installed. Creating a shortcut.", "MHF-Z Overlay Installation", MessageBoxButton.OK, MessageBoxImage.Information);
        tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
    }

    /// <summary>
    /// Called when [application uninstall].
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="tools">The tools.</param>
    private static void OnAppUninstall(SemanticVersion version, IAppTools tools)
    {
        Logger.Info(CultureInfo.InvariantCulture, "Removed overlay shortcut");
        MessageBox.Show("【MHF-Z】Overlay has been uninstalled. Removing shortcut.", "MHF-Z Overlay Installation", MessageBoxButton.OK, MessageBoxImage.Information);
        tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
    }

    /// <summary>
    /// Called when [application run].
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="tools">The tools.</param>
    /// <param name="firstRun">if set to <c>true</c> [first run].</param>
    private static void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
    {
        tools.SetProcessAppUserModelId();

        // show a welcome message when the app is first installed
        if (firstRun)
        {
            Logger.Info(CultureInfo.InvariantCulture, "Running overlay for first time");
            MessageBox.Show(
@"【MHF-Z】Overlay is now running! Thanks for installing【MHF-Z】Overlay.

Hotkeys: Shift+F1 (Configuration) | Shift+F5 (Restart) | Shift+F6 (Close)

As an alternative to hotkeys, you can check your system tray options by right-clicking the icon.

Press Alt+Enter if your game resolution changed.

The overlay might take some time to start due to databases. The next time you run the program, you may be asked to update the database. 

It's also recommended to change the resolution of the overlay if you are using a resolution other than the default set.

Happy Hunting!", "MHF-Z Overlay Installation",
MessageBoxButton.OK,
MessageBoxImage.Information);
            IsFirstRun = true;
        }
    }

    private void OnAppUpdate(SemanticVersion version, IAppTools tools) => Logger.Info($"Clowd.Squirrel update process called. {nameof(SemanticVersion)}: {version}");
}
