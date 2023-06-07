// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MHFZ_Overlay.Core.Class.Application;
using MHFZ_Overlay.Core.Class.IO;
using MHFZ_Overlay.Core.Class.Log;
using Squirrel;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

// TODO: all of this needs testing
namespace MHFZ_Overlay;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    public static bool isClowdSquirrelUpdating = false;

    /// <summary>
    /// The current program version. TODO: put in env var
    /// </summary>
    public static string? CurrentProgramVersion { get; private set; }

    //private string GetAssemblyVersion()
    //{
    //    return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
    //}
    private static string GetAssemblyVersion
    {
        get
        {
            var assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var versionString = assemblyVersion != null
                ? $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}"
                : "0.0.0";

            return versionString;
        }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        // Create a Stopwatch instance
        Stopwatch stopwatch = new Stopwatch();
        // Start the stopwatch
        stopwatch.Start();
        logger.Info("Started WPF application");
        logger.Trace("Call stack: {0}", new StackTrace().ToString());
        logger.Debug("OS: {0}, is64BitOS: {1}, is64BitProcess: {2}, CLR version: {3}", Environment.OSVersion, Environment.Is64BitOperatingSystem, Environment.Is64BitProcess, Environment.Version);

        // TODO: test if this doesnt conflict with squirrel update
        CurrentProgramVersion = $"v{GetAssemblyVersion}";
        if (CurrentProgramVersion == "v0.0.0")
        {
            logger.Fatal("Program version not found");
            MessageBox.Show("Program version not found", LoggingManager.FATAL_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
            LoggingManager.WriteCrashLog(new Exception("Program version not found"));
        }
        else
        {
            logger.Info("Found program version {0}", CurrentProgramVersion);
        }

        // run Squirrel first, as the app may exit after these run
        SquirrelAwareApp.HandleEvents(
            onInitialInstall: OnAppInstall,
            onAppUninstall: OnAppUninstall,
            onEveryRun: OnAppRun,
            onAppUpdate: OnAppUpdate);

        // ... other app init code after ...
        RestoreSettings();
        Settings.Default.Reload();
        logger.Info("Reloaded default settings");
        // Stop the stopwatch
        stopwatch.Stop();
        // Get the elapsed time in milliseconds
        double elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        // Print the elapsed time
        logger.Debug($"App ctor Elapsed Time: {elapsedTimeMs} ms");
        base.OnStartup(e);
    }

    // https://github.com/Squirrel/Squirrel.Windows/issues/198#issuecomment-299262613
    // Indeed you can use the methods below to backup your settings,
    // typically just after your update has completed,
    // so just after your call to await mgr.UpdateApp();
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
        string settingsFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
        string destination = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\last.config";
        FileManager.CopyFileToDestination(settingsFile, destination, true, "Backed up settings", true);
    }

    /// <summary>
    /// Restore our settings backup if any.
    /// Used to persist settings across updates.
    /// </summary>
    private static void RestoreSettings()
    {
        //Restore settings after application update            
        string destFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
        string sourceFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\last.config";
        var restorationMessage = "Restored settings";
        logger.Info("Restore our settings backup if any. Destination: {0}. Source: {1}", destFile, sourceFile);
        FileManager.RestoreFileFromSourceToDestination(destFile, sourceFile, restorationMessage);
    }

    /// <summary>
    /// Called when [application install].
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="tools">The tools.</param>
    private static void OnAppInstall(SemanticVersion version, IAppTools tools)
    {
        logger.Info("Created overlay shortcut");
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
        logger.Info("Removed overlay shortcut");
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
            logger.Info("Running overlay for first time");
            MessageBox.Show(
@"【MHF-Z】Overlay is now running! Thanks for installing【MHF-Z】Overlay.

Hotkeys: Shift+F1 (Configuration) | Shift+F5 (Restart) | Shift+F6 (Close)

As an alternative to hotkeys, you can check your system tray options by right-clicking the icon.

Press Alt+Enter if your game resolution changed.

The overlay might take some time to start due to databases. The next time you run the program, you may be asked to update the database. 

It's also recommended to change the resolution of the overlay if you are using a resolution other than 4K.

Happy Hunting!", "MHF-Z Overlay Installation", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }

    private void OnAppUpdate(SemanticVersion version, IAppTools tools)
    {
        logger.Info($"Clowd.Squirrel update process called. {nameof(SemanticVersion)}: {version}");
    }

    public static async Task UpdateMyApp()
    {
        isClowdSquirrelUpdating = true;
        var splashScreen = new SplashScreen("UI/Icons/png/loading.png");
        splashScreen.Show(false);
        try
        {
            using var mgr = new GithubUpdateManager(@"https://github.com/DorielRivalet/mhfz-overlay");
            var newVersion = await mgr.UpdateApp();
            BackupSettings();

            // optionally restart the app automatically, or ask the user if/when they want to restart
            if (newVersion != null)
            {
                logger.Info("Overlay has been updated, restarting application");
                splashScreen.Close(TimeSpan.FromSeconds(0.1));
                MessageBox.Show("【MHF-Z】Overlay has been updated, restarting application.", "MHF-Z Overlay Update", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateManager.RestartApp();
            }
            else
            {
                logger.Error("No updates available.");
                isClowdSquirrelUpdating = false;
                splashScreen.Close(TimeSpan.FromSeconds(0.1));
                MessageBox.Show("No updates available. If you want to check for updates manually, visit the GitHub repository at https://github.com/DorielRivalet/mhfz-overlay.", "MHF-Z Overlay Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            isClowdSquirrelUpdating = false;
            splashScreen.Close(TimeSpan.FromSeconds(0.1));
            MessageBox.Show("An error has occurred with the update process, see logs.log for more information", LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
