using DiscordRPC;
using MHFZ_Overlay.Core.Class.Discord;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHFZ_Overlay.Core.Class.Application
{
    /// <summary>
    /// Handles the application's state changes (shutdown, restart, etc.)
    /// </summary>
    internal class ApplicationManager
    {
        private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void HandleShutdown()
        {
            //https://stackoverflow.com/a/9050477/18859245
            databaseManager.StoreSessionTime(databaseManager.DatabaseStartTime);
            DiscordManager.DiscordRPCCleanup();
            DisposeNotifyIcon();
            logger.Info("Closing overlay");
            Environment.Exit(0);
        }

        public static void DisposeNotifyIcon()
        {
            MainWindow._notifyIcon.Visible = false;
            MainWindow._notifyIcon.Icon = null;
            MainWindow._notifyIcon.Dispose();
            logger.Info("Disposed Notify Icon");
        }

        public static void HandleRestart()
        {
            databaseManager.StoreSessionTime(databaseManager.DatabaseStartTime);
            DiscordManager.DiscordRPCCleanup();
            DisposeNotifyIcon();
            logger.Info("Restarting overlay");
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
