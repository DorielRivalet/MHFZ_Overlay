using Squirrel;
using System.Windows;
using NLog;
using MHFZ_Overlay.Core.Class.Log;
using MHFZ_Overlay.Core.Class.IO;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

// TODO: all of this needs testing
namespace MHFZ_Overlay
{
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
        public const string CurrentProgramVersion = "v0.22.0";

        protected override void OnStartup(StartupEventArgs e)
        {
            logger.Info("Started WPF application");
            logger.Trace("Call stack: {0}", new StackTrace().ToString());
            logger.Debug("OS: {0}, is64BitOS: {1}, is64BitProcess: {2}, CLR version: {3}", Environment.OSVersion, Environment.Is64BitOperatingSystem, Environment.Is64BitProcess, Environment.Version); 

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

Happy Hunting!", "MHF-Z Overlay Installation", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void OnAppUpdate(SemanticVersion version, IAppTools tools)
        {
            logger.Info($"Clowd.Squirrel update process called. {nameof(SemanticVersion)}: {version}");
        }

        private string programVersion = "";

        public static async Task UpdateMyApp()
        {
            isClowdSquirrelUpdating = true;
            try
            {
                using var mgr = new GithubUpdateManager(@"https://github.com/DorielRivalet/mhfz-overlay");
                var newVersion = await mgr.UpdateApp();
                BackupSettings();

                // optionally restart the app automatically, or ask the user if/when they want to restart
                if (newVersion != null)
                {
                    logger.Info("Overlay has been updated, restarting application");
                    MessageBox.Show("【MHF-Z】Overlay has been updated, restarting application.", "MHF-Z Overlay Update", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateManager.RestartApp();
                }
                else
                {
                    logger.Error("No updates available.");
                    isClowdSquirrelUpdating = false;
                    MessageBox.Show("No updates available. If you want to check for updates manually, visit the GitHub repository at https://github.com/DorielRivalet/mhfz-overlay.", "MHF-Z Overlay Update", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                isClowdSquirrelUpdating = false;
                MessageBox.Show("An error has occurred with the update process, see logs.log for more information", LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
