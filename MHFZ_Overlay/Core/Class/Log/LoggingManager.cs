// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MHFZ_Overlay.Core.Class.Application;
using MHFZ_Overlay.Core.Class.DataAccessLayer;
using MHFZ_Overlay.Core.Constant;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MHFZ_Overlay.Core.Class.Log;

/// <summary>
/// Handles logging functionality. Uses NLog.
/// </summary>
internal class LoggingManager
{
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();

    public static void WriteCrashLog(Exception ex, string logMessage = "Program crashed")
    {
        logger.Fatal(ex, logMessage);
        System.Windows.MessageBox.Show(@"Fatal error, closing overlay. See the crash log in the overlay\logs folder for more information.", Messages.FATAL_TITLE, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        PromptForOpeningLogs();
        ApplicationManager.HandleShutdown();
    }

    private static void PromptForOpeningLogs()
    {
        var logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "logs", "logs.log");

        if (!File.Exists(logFilePath))
        {
            logger.Error("Could not find the log file: {0}", logFilePath);
            System.Windows.MessageBox.Show(string.Format("Could not find the log file: {0}", logFilePath), Messages.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Show an error message to the user and ask if they want to open the log file
        MessageBoxResult result = System.Windows.MessageBox.Show("An error occurred. Do you want to open the log file?", Messages.ERROR_TITLE, MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No);
        if (result == MessageBoxResult.Yes)
        {
            // Open the log file using the default application
            try
            {
                Process.Start("notepad.exe", logFilePath);
                logger.Info("Opened with notepad the file {0}", logFilePath);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Could not open the log file: {0}", logFilePath);
                System.Windows.MessageBox.Show("Could not open the log file: " + ex.Message, Messages.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
