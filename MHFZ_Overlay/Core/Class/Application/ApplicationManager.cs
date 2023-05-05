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

        public static void HandleShutdown(NotifyIcon _notifyIcon)
        {
            //https://stackoverflow.com/a/9050477/18859245
            databaseManager.StoreSessionTime(databaseManager.DatabaseStartTime);
            DisposeNotifyIcon(_notifyIcon);
            logger.Info("Stored session time, closing overlay");
            Environment.Exit(0);
        }

        public static void DisposeNotifyIcon(NotifyIcon _notifyIcon)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Icon = null;
            _notifyIcon.Dispose();
        }

        public static void HandleRestart(NotifyIcon _notifyIcon)
        {
            databaseManager.StoreSessionTime(databaseManager.DatabaseStartTime);
            DisposeNotifyIcon(_notifyIcon);
            logger.Info("Stored session time, restarting overlay");
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        //Dispose client        
        /// <summary>
        /// Cleanups the Discord RPC instance.
        /// </summary>
        public static void DiscordRPCCleanup(DiscordRpcClient Client)
        {
            if (Client != null)//&& ShowDiscordRPC)
            {
                Client.Dispose();
                logger.Info("Disposed Discord RPC");
            }
        }
    }
}
