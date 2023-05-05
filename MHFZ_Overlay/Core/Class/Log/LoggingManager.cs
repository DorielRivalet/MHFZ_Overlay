using DiscordRPC;
using MHFZ_Overlay.Core.Class.Application;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHFZ_Overlay.Core.Class.Log
{
    internal class LoggingManager
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();

        public static void WriteCrashLog(Exception ex, DiscordRpcClient client, NotifyIcon _notifyIcon, string logMessage = "Program crashed")
        {
            System.Windows.MessageBox.Show(@"Fatal error, closing overlay. See the crash log in the overlay\logs folder for more information.", "MHF-Z Overlay Fatal Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            logger.Fatal(ex, logMessage);

            ApplicationManager.DiscordRPCCleanup(client);
            ApplicationManager.HandleShutdown(_notifyIcon);
        }
    }
}
