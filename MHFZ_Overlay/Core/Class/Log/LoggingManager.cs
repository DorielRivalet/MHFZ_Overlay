// Copyright 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using DiscordRPC;
using MHFZ_Overlay.Core.Class.Application;
using MHFZ_Overlay.Core.Class.DataAccessLayer;
using NLog;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MHFZ_Overlay.Core.Class.Log
{
    /// <summary>
    /// Handles logging functionality. Uses NLog.
    /// </summary>
    internal class LoggingManager
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();
        public const string FATAL_TITLE = "MHF-Z Overlay Fatal Error";
        public const string ERROR_TITLE = "MHF-Z Overlay Error";
        public const string WARNING_TITLE = "MHF-Z Overlay Warning";
        public const string INFO_TITLE = "MHF-Z Overlay Info";
        public const string DEBUG_TITLE = "MHF-Z Overlay Debug";

        public static void WriteCrashLog(Exception ex, string logMessage = "Program crashed")
        {
            logger.Fatal(ex, logMessage);
            System.Windows.MessageBox.Show(@"Fatal error, closing overlay. See the crash log in the overlay\logs folder for more information.", FATAL_TITLE, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            PromptForOpeningLogs();
            ApplicationManager.HandleShutdown();
        }

        private static void PromptForOpeningLogs()
        {
            var logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "logs", "logs.log");

            if (!File.Exists(logFilePath))
            {
                logger.Error("Could not find the log file: {0}", logFilePath);
                System.Windows.MessageBox.Show(string.Format("Could not find the log file: {0}", logFilePath), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Show an error message to the user and ask if they want to open the log file
            MessageBoxResult result = System.Windows.MessageBox.Show("An error occurred. Do you want to open the log file?", LoggingManager.ERROR_TITLE, MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No);
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
                    System.Windows.MessageBox.Show("Could not open the log file: " + ex.Message, LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
