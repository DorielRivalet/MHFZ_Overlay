using DiscordRPC;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHFZ_Overlay.Core.Class.Application
{
    internal class ApplicationManager
    {
        private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void HandleShutdown()
        {
            //https://stackoverflow.com/a/9050477/18859245
            databaseManager.StoreSessionTime(databaseManager.DatabaseStartTime);
            ApplicationManager.DiscordRPCCleanup();
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
            DisposeNotifyIcon();
            logger.Info("Restarting overlay");
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        //Dispose client        
        /// <summary>
        /// Cleanups the Discord RPC instance.
        /// </summary>
        public static void DiscordRPCCleanup()
        {
            if (MainWindow.discordRPCClient != null)//&& ShowDiscordRPC)
            {
                MainWindow.discordRPCClient.Dispose();
                logger.Info("Disposed Discord RPC");
            }
        }
    }
}
