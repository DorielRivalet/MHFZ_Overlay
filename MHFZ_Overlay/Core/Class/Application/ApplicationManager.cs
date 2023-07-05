// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MHFZ_Overlay.Core.Class.DataAccessLayer;
using MHFZ_Overlay.Core.Class.Discord;
using MHFZ_Overlay.Core.Constant;
using System;
using System.Diagnostics;
using System.Windows;

namespace MHFZ_Overlay.Core.Class.Application;

/// <summary>
/// Handles the application's state changes (shutdown, restart, etc.)
/// </summary>
internal class ApplicationManager
{
    private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Handles the shutdown.
    /// </summary>
    public static void HandleShutdown()
    {
        //https://stackoverflow.com/a/9050477/18859245
        databaseManager.StoreSessionTime(databaseManager.DatabaseStartTime);
        DiscordManager.DiscordRPCCleanup();
        DisposeNotifyIcon();
        logger.Info("Closing overlay");
        Environment.Exit(0);
    }

    /// <summary>
    /// Disposes the notify icon.
    /// </summary>
    public static void DisposeNotifyIcon()
    {
        if (MainWindow._mainWindowNotifyIcon == null) return;
        MainWindow._mainWindowNotifyIcon.Dispatcher.Invoke(() =>
        {
            MainWindow._mainWindowNotifyIcon.Visibility = Visibility.Collapsed;
            MainWindow._mainWindowNotifyIcon.Icon = null;
            MainWindow._mainWindowNotifyIcon.Dispose();
            logger.Info("Disposed Notify Icon");
        });
    }

    /// <summary>
    /// Handles the restart.
    /// </summary>
    public static void HandleRestart()
    {
        databaseManager.StoreSessionTime(databaseManager.DatabaseStartTime);
        DiscordManager.DiscordRPCCleanup();
        DisposeNotifyIcon();
        logger.Info("Restarting overlay");
        System.Windows.Forms.Application.Restart();
        System.Windows.Application.Current.Shutdown();
    }

    /// <summary>
    /// This should never run if possible, only used as last resort
    /// </summary>
    public static void HandleGameShutdown()
    {
        logger.Info("Closing game");
        KillProcess("mhf");
        MessageBox.Show("The game was closed due to a fatal overlay error, please report this to the developer", Messages.FATAL_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <summary>
    /// Kills the process.
    /// </summary>
    /// <param name="processName">Name of the process.</param>
    public static void KillProcess(string processName)
    {
        try
        {
            logger.Info("Killing process {0}", processName);

            // Get all running processes with the specified name
            Process[] processes = Process.GetProcessesByName(processName);

            // Check if there is only a single process with the specified name
            if (processes.Length == 1)
            {
                Process process = processes[0];

                // Forcefully close the process
                process.Kill();

                logger.Info("Process {0} has been killed.", processName);
            }
            else
            {
                // Handle the case when multiple processes with the specified name are found
                if (processes.Length == 0)
                {
                    logger.Warn("No process with the specified name found.");
                }
                else
                {
                    logger.Warn("Multiple processes with the specified name found. Unable to kill the process automatically.");
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex);
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
            logger.Info("Closing process {0}", processName);

            // Get all running processes with the specified name
            Process[] processes = Process.GetProcessesByName(processName);

            // Check if there is only a single process with the specified name
            if (processes.Length == 1)
            {
                Process process = processes[0];

                // Close the process
                var successful = process.CloseMainWindow();
                logger.Info("Closed main window, successful: {0}.", successful);
                process.WaitForExit();

                // Check if the process is still running after closing its main window
                if (!process.HasExited)
                {
                    // Forcefully close the process if it's still running
                    process.Kill();
                }

                logger.Info("Process {0} has been closed.", processName);
            }
            else
            {
                // Handle the case when multiple processes with the specified name are found
                if (processes.Length == 0)
                {
                    logger.Warn("No process with the specified name found.");
                }
                else
                {
                    logger.Warn("Multiple processes with the specified name found. Unable to close the process automatically.");
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }
    }
}
