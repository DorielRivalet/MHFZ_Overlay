namespace MHFZ_Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Velopack;
using System.Windows;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Services;
using NuGet.Versioning;
using RESTCountries.NET.Models;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using Velopack.Sources;
using Velopack.Windows;
using System.IO;

public class Program
{
    public static bool WasFirstRun { get; private set; }

    public static bool WasJustUpdated { get; private set; }

    public static string UpdateUrl { get; private set; } = @"https://github.com/DorielRivalet/mhfz-overlay";

    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    public static bool IsVelopackUpdating { get; set; }

    /// <summary>
    /// Gets the current program version. TODO: put in env var.
    /// </summary>
    public static string? CurrentProgramVersion { get; private set; }

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

    private static void VelopackFirstRun(SemanticVersion v)
    {
        WasFirstRun = true;
        Logger.Info(CultureInfo.InvariantCulture, $"Running overlay v{v.Major}.{v.Minor}.{v.Patch} for first time");
        MessageBox.Show(
$@"【MHF-Z】Overlay is now running! Thanks for installing【MHF-Z】Overlay v{v.Major}.{v.Minor}.{v.Patch}.

Hotkeys: Shift+F1 (Configuration) | Shift+F5 (Restart) | Shift+F6 (Close)

As an alternative to hotkeys, you can check your system tray options by right-clicking the icon.

Press Alt+Enter if your game resolution changed.

The overlay might take some time to start due to databases. The next time you run the program, you may be asked to update the database. 

It's also recommended to change the resolution of the overlay if you are using a resolution other than the default set.

Happy Hunting!", $"MHF-Z Overlay v{v.Major}.{v.Minor}.{v.Patch} Installation",
MessageBoxButton.OK,
MessageBoxImage.Information);
    }

    private static void VelopackUpdatedAndRestarted(SemanticVersion v)
    {
        WasJustUpdated = true;
        Logger.Info($"Velopack update and restart process called. {nameof(SemanticVersion)}: {v}");
    }

    // Since WPF has an "automatic" Program.Main, we need to create our own.
    // In order for this to work, you must also add the following to your .csproj:
    // <StartupObject>VeloWpfSample.Program</StartupObject>
    [STAThread]
    public static void Main(string[] args)
    {
        try
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
            // Logging is essential for debugging! Ideally you should write it to a file.
            // Log = new MemoryLogger();
            var loggerFactory = new NLog.Extensions.Logging.NLogLoggerFactory();
            var velopackLogger = loggerFactory.CreateLogger("VelopackLogger");

            // It's important to Run() the VelopackApp as early as possible in app startup.
            VelopackApp.Build()
                .WithRestarted((v) => VelopackUpdatedAndRestarted(v))
                .WithFirstRun((v) => VelopackFirstRun(v))
                .Run(velopackLogger);

            // ... other app init code after ...
            RestoreSettings();
            Settings.Default.Reload();
            Logger.Info(CultureInfo.InvariantCulture, "Reloaded default settings");

            // This is purely for demonstration purposes, we get the update URL from a
            // property defined by MSBuild, so we can locate the local releases directory.
            // In your production app, this should point to your update server.
            //UpdateUrl = Assembly.GetEntryAssembly()
            //    .GetCustomAttributes<AssemblyMetadataAttribute>()
            //    .Where(x => x.Key == "WpfSampleReleaseDir")
            //    .Single().Value;

            stopwatch.Stop();

            // Get the elapsed time in milliseconds
            var elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;
            // Print the elapsed time
            Logger.Debug($"Program initialization Elapsed Time: {elapsedTimeMs} ms");

            // We can now launch the WPF application as normal.
            var app = new App();
            app.InitializeComponent();
            app.Run();

        }
        catch (Exception ex)
        {
            MessageBox.Show("Unhandled exception: " + ex.ToString());
        }
    }

    public static async Task UpdateMyApp()
    {
        IsVelopackUpdating = true;
        var splashScreen = new SplashScreen("./Assets/Icons/png/loading.png");
        splashScreen.Show(false);
        try
        {
            var mgr = new UpdateManager(new GithubSource(UpdateUrl, "", false));

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                Logger.Info($"No updates available.");
                MessageBox.Show(
$@"No updates available.", $"MHF-Z Overlay Installation",
MessageBoxButton.OK,
MessageBoxImage.Information);
                return; // no update available
            }

            Logger.Info($"Updates found, downloading new version {newVersion.TargetFullRelease.Version}.");

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            Logger.Info($"Downloaded new version {newVersion.TargetFullRelease.Version}.");
            BackupSettings();

            // optionally restart the app automatically, or ask the user if/when they want to restart
            if (mgr.IsUpdatePendingRestart)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Overlay has been updated, restarting application.");
                splashScreen.Close(TimeSpan.FromSeconds(0.1));
                MessageBox.Show(
@"【MHF-Z】Overlay has been updated, restarting application.

If after overlay startup your settings did not transfer over, try restarting the overlay again without saving or changing any settings. Alternatively, find the old settings file by going into the parent folder when clicking the settings folder option.", "MHF-Z Overlay Update", MessageBoxButton.OK, MessageBoxImage.Information);
                // install new version and restart app
                mgr.ApplyUpdatesAndRestart(newVersion);
            }
            else
            {
                Logger.Error(CultureInfo.InvariantCulture, "No updates available.");
                IsVelopackUpdating = false;
                splashScreen.Close(TimeSpan.FromSeconds(0.1));
                MessageBox.Show("No updates available. If you want to check for updates manually, visit the GitHub repository at https://github.com/DorielRivalet/mhfz-overlay.", "MHF-Z Overlay Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            IsVelopackUpdating = false;
            splashScreen.Close(TimeSpan.FromSeconds(0.1));
            MessageBox.Show("An error has occurred with the update process, see logs.log for more information", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
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
}

