﻿using Memory;
using MHFZ_Overlay.addresses;
using MHFZ_Overlay.Core.Class.Application;
using MHFZ_Overlay.Core.Class.IO;
using NLog;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MHFZ_Overlay
{
    /// <summary>
    /// Responsible for loading data into the application. It has a DatabaseManager object that is used to access and manipulate the database. It also has instances of AddressModelNotHGE and AddressModelHGE classes, which inherit from the AddressModel abstract class. Depending on the state of the game, one of these instances is used to get the hit count value (etc.) from the memory.
    /// </summary>
    public class DataLoader
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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
        public void BackupSettings()
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

            FileManager.RestoreFileFromSourceToDestination(destFile, sourceFile, restorationMessage);
        }

        // TODO: would like to make this a singleton but its complicated
        // this loads first before MainWindow constructor is called. meaning this runs twice.
        public DataLoader()
        {
            Debug.WriteLine("DataLoader constructor called. Call stack:");
            Debug.WriteLine(new StackTrace().ToString());

            // run Squirrel first, as the app may exit after these run
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: OnAppInstall,
                onAppUninstall: OnAppUninstall,
                onEveryRun: OnAppRun);

            // ... other app init code after ...
            RestoreSettings();

            logger.Info($"DataLoader initialized");

            int PID = m.GetProcIdFromName("mhf");
            if (PID > 0)
            {
                m.OpenProcess(PID);
                try
                {
                    CreateCodeCave(PID);
                }
                catch (Exception ex)
                {
                    // TODO: does this warrant the program closing?
                    // ReShade or similar programs might trigger this warning. Does these affect overlay functionality?
                    // Imulion's version does not have anything in the catch block.
                    //System.Windows.MessageBox.Show($"Warning: could not create code cave. ReShade or similar programs might trigger this warning. You can ignore this warning. \n\n{ex}", "Warning - MHFZ Overlay", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    logger.Warn(ex, "Could not create code cave");
                }

                if (!isHighGradeEdition)
                {
                    logger.Info("Running game in Non-HGE");
                    model = new AddressModelNotHGE(m);
                }
                else
                {
                    logger.Info("Running game in HGE");
                    model = new AddressModelHGE(m);
                }

                // first we check if there are duplicate mhf.exe
                CheckForExternalProcesses();
                CheckForIllegalModifications();
                // if there aren't then this runs and sets the game folder and also the database folder if needed
                GetMHFFolderLocation();
                // and thus set the data to database then, after doing it to the settings
                databaseChanged = databaseManager.SetupLocalDatabase(this);
                string overlayHash = databaseManager.StoreOverlayHash();
                CheckIfLoadedInMezeporta();
            }
            else
            {
                System.Windows.MessageBox.Show("Please launch game first", "Error - MHFZ Overlay", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                logger.Fatal("Launch game first");
                ApplicationManager.HandleShutdown(MainWindow._notifyIcon);
            }
        }

        private void CheckIfLoadedInMezeporta()
        {
            if (model.AreaID() != 200)
            {
                logger.Warn("Loaded overlay outside Mezeporta");

                Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
                if (s.EnableOutsideMezeportaLoadingWarning)
                    System.Windows.MessageBox.Show("It is not recommended to load the overlay outside of Mezeporta", "Warning - MHFZ Overlay", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        private void GetMHFFolderLocation()
        {
            // Get the process that is running "mhf.exe"
            Process[] processes = Process.GetProcessesByName("mhf");

            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (processes.Length > 0)
            {
                // Get the location of the first "mhf.exe" process
                string mhfPath = processes[0].MainModule.FileName;
                // Get the directory that contains "mhf.exe"
                string mhfDirectory = Path.GetDirectoryName(mhfPath);
                // Save the directory to the program's settings
                // (Assuming you have a "settings" object that can store strings)
                s.GameFolderPath = mhfDirectory;

                // Check if the "database" folder exists in the "mhf" folder
                string databasePath = Path.Combine(mhfDirectory, "database");
                if (!Directory.Exists(databasePath))
                {
                    // Create the "database" folder if it doesn't exist
                    Directory.CreateDirectory(databasePath);
                }

                // Check if the "MHFZ_Overlay.sqlite" file exists in the "database" folder
                string sqlitePath = Path.Combine(databasePath, "MHFZ_Overlay.sqlite");

                s.DatabaseFilePath = sqlitePath;

                // Check if the version file exists in the database folder
                string previousVersionPath = Path.Combine(databasePath, "previous-version.txt");
                FileManager.CreateFileIfNotExists(previousVersionPath, "Creating version file at ");

                s.PreviousVersionFilePath = previousVersionPath;

                s.Save();
            }
            else
            {
                // The "mhf.exe" process was not found
                MessageBox.Show("The 'mhf.exe' process was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Fatal("mhf.exe not found");
                ApplicationManager.HandleShutdown(MainWindow._notifyIcon);
            }
        }

        private readonly List<string> bannedProcesses = new List<string>()
        {
            "displayer","Displayer","cheat","Cheat","overlay","Overlay","Wireshark", "FreeCam"
        };

        private readonly List<string> bannedFiles = new List<string>()
        {
            "d3d8","d3d9","d3d10","d3d11","d3d12","ddraw","dinput","dinput8","dsound",
            "msacm32","msvfw32","version","wininet","winmm","xlive","bink2w64","bink2w64Hooked",
            "vorbisFile","vorbisHooked","binkw32","binkw32Hooked"
        };

        private readonly List<string> bannedFileExtensions = new List<string>()
        {
            ".asi"
        };

        private readonly List<string> bannedFolders = new List<string>()
        {
            "scripts","plugins","script","plugin","localize-dat"
        };

        public void CheckForExternalProcesses()
        {
            var processList = System.Diagnostics.Process.GetProcesses();
            int overlayCount = 0;
            int gameCount = 0;
            foreach (var process in processList)
            {
                if (process.ProcessName == "GameOverlayUI")
                {
                    MessageBox.Show($"Having Steam Overlay open while MHF-Z Overlay is running may decrease performance. ({process.ProcessName} found)", "Warning");
                    logger.Warn("Found Steam overlay: {0}", process.ProcessName);
                    continue;
                }
                if (bannedProcesses.Any(s => process.ProcessName.Contains(s)) && process.ProcessName != "MHFZ_Overlay")
                {

                    // processName is a substring of one of the banned process strings
                    MessageBox.Show($"Close other external programs before opening the overlay ({process.ProcessName} found)", "Error");
                    logger.Fatal("Found banned process {0}", process.ProcessName);

                    // Close the overlay program
                    ApplicationManager.HandleShutdown(MainWindow._notifyIcon);
                }
                if (process.ProcessName == "MHFZ_Overlay")
                {
                    overlayCount++;
                }
                if (process.ProcessName == "mhf")
                {
                    gameCount++;
                }
            }
            if (overlayCount > 1)
            {
                // More than one "MHFZ_Overlay" process is running
                MessageBox.Show("Close other instances of the overlay before opening a new one", "Error");
                logger.Fatal("Found duplicate overlay");

                // Close the overlay program
                ApplicationManager.HandleShutdown(MainWindow._notifyIcon);
            }
            if (gameCount > 1)
            {
                // More than one game process is running
                MessageBox.Show("Close other instances of the game before opening a new one", "Error");
                logger.Fatal("Found duplicate game");

                // Close the overlay program
                ApplicationManager.HandleShutdown(MainWindow._notifyIcon);
            }
        }

        // This checks for illegal folders or files in the game folder
        // TODO: test
        public void CheckForIllegalModifications()
        {
            // Get the process that is running "mhf.exe"
            Process[] processes = Process.GetProcessesByName("mhf");

            if (processes.Length > 0)
            {
                // Get the location of the first "mhf.exe" process
                string mhfPath = processes[0].MainModule.FileName;
                // Get the directory that contains "mhf.exe"
                string mhfDirectory = Path.GetDirectoryName(mhfPath);

                // Get a list of all files and folders in the game folder
                string[] files = Directory.GetFiles(mhfDirectory, "*", SearchOption.AllDirectories);
                string[] folders = Directory.GetDirectories(mhfDirectory, "*", SearchOption.AllDirectories);
                var isFatal = true;
                FileManager.CheckIfFileExtensionFolderExists(files, folders, bannedFiles, bannedFileExtensions, bannedFolders, isFatal);
            }
            else
            {
                // The "mhf.exe" process was not found
                MessageBox.Show("The 'mhf.exe' process was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Fatal("mhf.exe not found");
                ApplicationManager.HandleShutdown(MainWindow._notifyIcon);
            }
        }

        public bool databaseChanged = false;


        #region DataLoaderVariables
        //needed for getting data
        readonly Mem m = new();

        public bool isHighGradeEdition { get; set; }

        int index;
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public AddressModel model { get; }

        #endregion

        /// <summary>
        /// Called when [application install].
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="tools">The tools.</param>
        private static void OnAppInstall(SemanticVersion version, IAppTools tools)
        {
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
            if (firstRun) MessageBox.Show(
@"【MHF-Z】Overlay is now running! Thanks for installing【MHF-Z】Overlay.

Hotkeys: Shift+F1 (Configuration) | Shift+F5 (Restart) | Shift+F6 (Close)

As an alternative to hotkeys, you can check your system tray options by right-clicking the icon.

Press Alt+Enter if your game resolution changed.

The overlay might take some time to start due to databases.

Happy Hunting!", "MHF-Z Overlay Installation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();

        /// <summary>
        /// Creates the code cave.
        /// </summary>
        /// <param name="PID">The pid.</param>
        private void CreateCodeCave(int PID)
        {
            //TODO why is this needed?
            Process? proc = LoadMHFODLL(PID);
            if (proc == null)
            {
                System.Windows.MessageBox.Show("Please launch game first", "Error - MHFZ Overlay", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                logger.Fatal("Launch game first");
                ApplicationManager.HandleShutdown(MainWindow._notifyIcon);
                return;
            }
            long searchAddress = m.AoBScan("89 04 8D 00 C6 43 00 61 E9").Result.FirstOrDefault();
            if (searchAddress.ToString("X8") == "00000000")
            {
                //Create codecave and get its address
                long baseScanAddress = m.AoBScan("0F B7 8a 24 06 00 00 0f b7 ?? ?? ?? c1 c1 e1 0b").Result.FirstOrDefault();
                UIntPtr codecaveAddress = m.CreateCodeCave(baseScanAddress.ToString("X8"), new byte[] { 0x0F, 0xB7, 0x8A, 0x24, 0x06, 0x00, 0x00, 0x0F, 0xB7, 0x52, 0x0C, 0x88, 0x15, 0x21, 0x00, 0x0F, 0x15, 0x8B, 0xC1, 0xC1, 0xE1, 0x0B, 0x0F, 0xBF, 0xC9, 0xC1, 0xE8, 0x05, 0x09, 0xC8, 0x01, 0xD2, 0xB9, 0x8E, 0x76, 0x21, 0x25, 0x29, 0xD1, 0x66, 0x8B, 0x11, 0x66, 0xF7, 0xD2, 0x0F, 0xBF, 0xCA, 0x0F, 0xBF, 0x15, 0xC4, 0x22, 0xEA, 0x17, 0x31, 0xC8, 0x31, 0xD0, 0xB9, 0xC0, 0x5E, 0x73, 0x16, 0x0F, 0xBF, 0xD1, 0x31, 0xD0, 0x60, 0x8B, 0x0D, 0x21, 0x00, 0x0F, 0x15, 0x89, 0x04, 0x8D, 0x00, 0xC6, 0x43, 0x00, 0x61 }, 63, 0x100);

                //Change addresses
                UIntPtr storeValueAddress = codecaveAddress + 125;                  //address where store some value?
                string storeValueAddressString = storeValueAddress.ToString("X8");
                byte[] storeValueAddressByte = Enumerable.Range(0, storeValueAddressString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(storeValueAddressString.Substring(x, 2), 16)).ToArray();
                Array.Reverse(storeValueAddressByte, 0, storeValueAddressByte.Length);
                byte[] by15 = { 136, 21 };
                m.WriteBytes(codecaveAddress + 11, by15);
                m.WriteBytes(codecaveAddress + 13, storeValueAddressByte);  //1
                m.WriteBytes(codecaveAddress + 72, storeValueAddressByte);  //2

                WriteByteFromAddress(codecaveAddress, proc, isHighGradeEdition ? 249263758 : 102223598, 33);
                WriteByteFromAddress(codecaveAddress, proc, isHighGradeEdition ? 27534020 : 27601756, 51);
                WriteByteFromAddress(codecaveAddress, proc, isHighGradeEdition ? 2973376 : 2865056, 60);

            }
            else
            {
                LoadMHFODLL(PID);
            }
        }

        public string GetQuestTimeCompletion()
        {
            double totalQuestDuration = (double)model.TimeDefInt() / 30; // Total duration of the quest in seconds
            double timeRemainingInQuest = (double)model.TimeInt() / 30; // Time left in the quest in seconds

            // Calculate the elapsed time by subtracting the time left from the total duration
            double elapsedTime = totalQuestDuration - timeRemainingInQuest;

            // Convert the elapsed time from seconds to milliseconds
            elapsedTime *= 1000;

            // Convert the elapsed time to a TimeSpan object
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(elapsedTime);

            // Format the TimeSpan object as a string
            string formattedTime = timeSpan.ToString(@"mm\:ss\.ff");

            return formattedTime;
        }

        /// <summary>
        /// Writes the byte from address.
        /// </summary>
        /// <param name="codecaveAddress">The codecave address.</param>
        /// <param name="proc">The proc.</param>
        /// <param name="offset1">The offset1.</param>
        /// <param name="offset2">The offset2.</param>
        void WriteByteFromAddress(UIntPtr codecaveAddress, Process proc, long offset1, int offset2)
        {
            long address = proc.Modules[index].BaseAddress.ToInt32() + offset1;
            string addressString = address.ToString("X8");
            byte[] addressByte = Enumerable.Range(0, addressString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(addressString.Substring(x, 2), 16)).ToArray();
            Array.Reverse(addressByte, 0, addressByte.Length);
            m.WriteBytes(codecaveAddress + offset2, addressByte);
        }

        /// <summary>
        /// Loads the mhfo.dll.
        /// </summary>
        /// <param name="PID">The pid.</param>
        /// <returns></returns>
        Process? LoadMHFODLL(int PID)
        {
            //Search and get mhfo-hd.dll module base address
            Process proccess = Process.GetProcessById(PID);
            if (proccess == null)
                return null;
            var ModuleList = new List<string>();
            foreach (ProcessModule md in proccess.Modules)
            {
                string? moduleName = md.ModuleName;
                if (moduleName != null)
                    ModuleList.Add(moduleName);
            }
            index = ModuleList.IndexOf("mhfo-hd.dll");
            if (index > 0)
            {
                isHighGradeEdition = true;
            }
            else
            {
                index = ModuleList.IndexOf("mhfo.dll");
                isHighGradeEdition = false;
            }
            return proccess;
        }
    }
}
