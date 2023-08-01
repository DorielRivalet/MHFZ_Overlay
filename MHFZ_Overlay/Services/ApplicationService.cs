// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Views.Windows;

/// <summary>
/// Handles the application's state changes (shutdown, restart, etc.)
/// </summary>
public sealed class ApplicationService
{
    private static readonly DatabaseService DatabaseManager = DatabaseService.GetInstance();

    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Handles the shutdown.
    /// </summary>
    public static void HandleShutdown()
    {
        // https://stackoverflow.com/a/9050477/18859245
        DatabaseManager.StoreSessionTime(DatabaseManager.DatabaseStartTime);
        DiscordService.DiscordRPCCleanup();
        DisposeNotifyIcon();
        Logger.Info(CultureInfo.InvariantCulture, "Closing overlay");
        Environment.Exit(0);
    }

    /// <summary>
    /// Disposes the notify icon.
    /// </summary>
    public static void DisposeNotifyIcon()
    {
        if (MainWindow._mainWindowNotifyIcon == null)
        {
            return;
        }

        MainWindow._mainWindowNotifyIcon.Dispatcher.Invoke(() =>
        {
            MainWindow._mainWindowNotifyIcon.Visibility = Visibility.Collapsed;
            MainWindow._mainWindowNotifyIcon.Icon = null;
            MainWindow._mainWindowNotifyIcon.Dispose();
            Logger.Info(CultureInfo.InvariantCulture, "Disposed Notify Icon");
        });
    }

    /// <summary>
    /// Handles the restart.
    /// </summary>
    public static void HandleRestart()
    {
        DatabaseManager.StoreSessionTime(DatabaseManager.DatabaseStartTime);
        DiscordService.DiscordRPCCleanup();
        DisposeNotifyIcon();
        Logger.Info(CultureInfo.InvariantCulture, "Restarting overlay");
        System.Windows.Forms.Application.Restart();
        Application.Current.Shutdown();
    }

    /// <summary>
    /// This should never run if possible, only used as last resort.
    /// </summary>
    public static void HandleGameShutdown()
    {
        Logger.Info(CultureInfo.InvariantCulture, "Closing game");
        KillProcess("mhf");
        MessageBox.Show("The game was closed due to a fatal overlay error, please report this to the developer", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <summary>
    /// Kills the process.
    /// </summary>
    /// <param name="processName">Name of the process.</param>
    public static void KillProcess(string processName)
    {
        try
        {
            Logger.Info(CultureInfo.InvariantCulture, "Killing process {0}", processName);

            // Get all running processes with the specified name
            var processes = Process.GetProcessesByName(processName);

            // Check if there is only a single process with the specified name
            if (processes.Length == 1)
            {
                var process = processes[0];

                // Forcefully close the process
                process.Kill();

                Logger.Info(CultureInfo.InvariantCulture, "Process {0} has been killed.", processName);
            }
            else
            {
                // Handle the case when multiple processes with the specified name are found
                if (processes.Length == 0)
                {
                    Logger.Warn(CultureInfo.InvariantCulture, "No process with the specified name found.");
                }
                else
                {
                    Logger.Warn(CultureInfo.InvariantCulture, "Multiple processes with the specified name found. Unable to kill the process automatically.");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }

    /// <summary>
    /// Closes the process.
    /// </summary>
    /// <param name="processName">Name of the process.</param>
    public static void CloseProcess(string processName)
    {
        try
        {
            Logger.Info(CultureInfo.InvariantCulture, "Closing process {0}", processName);

            // Get all running processes with the specified name
            var processes = Process.GetProcessesByName(processName);

            // Check if there is only a single process with the specified name
            if (processes.Length == 1)
            {
                var process = processes[0];

                // Close the process
                var successful = process.CloseMainWindow();
                Logger.Info(CultureInfo.InvariantCulture, "Closed main window, successful: {0}.", successful);
                process.WaitForExit();

                // Check if the process is still running after closing its main window
                if (!process.HasExited)
                {
                    // Forcefully close the process if it's still running
                    process.Kill();
                }

                Logger.Info(CultureInfo.InvariantCulture, "Process {0} has been closed.", processName);
            }
            else
            {
                // Handle the case when multiple processes with the specified name are found
                if (processes.Length == 0)
                {
                    Logger.Warn(CultureInfo.InvariantCulture, "No process with the specified name found.");
                }
                else
                {
                    Logger.Warn(CultureInfo.InvariantCulture, "Multiple processes with the specified name found. Unable to close the process automatically.");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}
