// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using MHFZ_Overlay.Models.Constant;

/// <summary>
/// Handles logging functionality. Uses NLog.
/// </summary>
public sealed class LoggingService
{
    private static readonly NLog.Logger LoggerInstance = NLog.LogManager.GetCurrentClassLogger();

    public static void WriteCrashLog(Exception ex, string logMessage = "Program crashed")
    {
        LoggerInstance.Fatal(ex, logMessage);
        MessageBox.Show(@"Fatal error, closing overlay. See the crash log in the overlay\logs folder for more information.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        PromptForOpeningLogs();
        ApplicationService.HandleShutdown();
    }

    private static void PromptForOpeningLogs()
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
            MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Could not find the log file: {0}", logFilePath), Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Show an error message to the user and ask if they want to open the log file
        var result = MessageBox.Show("An error occurred. Do you want to open the log file?", Messages.ErrorTitle, MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No);
        if (result == MessageBoxResult.Yes)
        {
            // Open the log file using the default application
            try
            {
                Process.Start(ApplicationPaths.NotepadPath, logFilePath);
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Opened with Notepad the file {0}", logFilePath);
            }
            catch (Exception ex)
            {
                LoggerInstance.Error(ex, "Could not open the log file: {0}", logFilePath);
                MessageBox.Show("Could not open the log file: " + ex.Message, Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public static NLog.LogLevel GetLogLevel(string logLevel)
    {
        switch (logLevel)
        {
            case "Trace":
                return NLog.LogLevel.Trace;
            case "Debug":
                return NLog.LogLevel.Debug;
            case "Info":
                return NLog.LogLevel.Info;
            case "Warn":
                return NLog.LogLevel.Warn;
            case "Error":
                return NLog.LogLevel.Error;
            case "Fatal":
                return NLog.LogLevel.Fatal;
            default:
                return NLog.LogLevel.Debug;
        }
    }
}
