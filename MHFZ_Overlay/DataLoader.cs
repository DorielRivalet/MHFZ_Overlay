// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using Memory;
using MHFZ_Overlay.Addresses;
using MHFZ_Overlay.Core.Class.Application;
using MHFZ_Overlay.Core.Class.DataAccessLayer;
using MHFZ_Overlay.Core.Class.IO;
using MHFZ_Overlay.Core.Class.Log;
using MHFZ_Overlay.Core.Constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Xunit.Sdk;

namespace MHFZ_Overlay;

/// <summary>
/// Responsible for loading data into the application. It has a DatabaseManager object that is used to access and manipulate the database. It also has instances of AddressModelNotHGE and AddressModelHGE classes, which inherit from the AddressModel abstract class. Depending on the state of the game, one of these instances is used to get the hit count value (etc.) from the memory.
/// </summary>
public class DataLoader
{
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    // TODO: would like to make this a singleton but its complicated
    // this loads first before MainWindow constructor is called. meaning this runs twice.
    public DataLoader()
    {
        // Create a Stopwatch instance
        Stopwatch stopwatch = new Stopwatch();
        // Start the stopwatch
        stopwatch.Start();
        logger.Trace("DataLoader constructor called. Call stack: {0}", new StackTrace().ToString());

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
                // I'm marking this as error since overlay might interfere with custom shaders.
                logger.Error(ex, "Could not create code cave");
                System.Windows.MessageBox.Show("Could not create code cave. ReShade or similar programs might trigger this error. Also make sure you are not loading the overlay when on game launcher.", Messages.ERROR_TITLE, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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
            CheckIfLoadedOutsideQuest();
        }
        else
        {
            logger.Fatal("Launch game first");
            System.Windows.MessageBox.Show("Please launch game first", Messages.ERROR_TITLE, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            ApplicationManager.HandleShutdown();
        }
        // Stop the stopwatch
        stopwatch.Stop();
        // Get the elapsed time in milliseconds
        double elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        // Print the elapsed time
        logger.Debug($"DataLoader ctor Elapsed Time: {elapsedTimeMs} ms");
    }

    public static bool loadedOutsideMezeporta = false;

    private void CheckIfLoadedInMezeporta()
    {
        if (model.AreaID() != 200)
        {
            logger.Warn("Loaded overlay outside Mezeporta");

            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            if (s.EnableOutsideMezeportaLoadingWarning)
                loadedOutsideMezeporta = true;
        }
    }

    private void CheckIfLoadedOutsideQuest()
    {
        if (model.QuestID() != 0)
        {
            logger.Fatal("Loaded overlay inside quest {0}", model.QuestID());
            System.Windows.MessageBox.Show("Loaded overlay inside quest. Please load the overlay outside quests.", Messages.FATAL_TITLE, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            LoggingManager.WriteCrashLog(new Exception("Loaded overlay inside quest"));
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
            logger.Fatal("mhf.exe not found");
            MessageBox.Show("The 'mhf.exe' process was not found.", Messages.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
            ApplicationManager.HandleShutdown();
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

    private readonly List<string> allowedProcesses = new List<string>()
    {
        "LogiOverlay" // Logitech Bluetooth for mouse
    };

    private bool steamOverlayWarningShown = false;

    // TODO: savvy users know the bypass. Short of making the overlay too involved in the user's machine, 
    // there are currently no workarounds.
    public void CheckForExternalProcesses()
    {
        if (App.isClowdSquirrelUpdating)
            return;

        var processList = System.Diagnostics.Process.GetProcesses();
        int overlayCount = 0;
        int gameCount = 0;
        // first we check for steam overlay, if found then skip to next iteration
        // then we apply the whitelist, if found then skip to next iteration
        // then we apply the blacklist
        // then we check if the process is the overlay
        // then we check if the process is the game
        // then we check for disallowed duplicates
        foreach (var process in processList)
        {
            if (process.ProcessName == "GameOverlayUI" && !steamOverlayWarningShown)
            {
                logger.Warn("Found Steam overlay: {0}", process.ProcessName);
                var result = MessageBox.Show($"Having Steam Overlay open while MHF-Z Overlay is running may decrease performance. ({process.ProcessName} found)", Messages.WARNING_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    steamOverlayWarningShown = true;
                }
                continue;
            }
            if (allowedProcesses.Any(s => process.ProcessName.Contains(s)) && process.ProcessName != "MHFZ_Overlay")
            {
                continue;
            }
            if (bannedProcesses.Any(s => process.ProcessName.Contains(s)) && process.ProcessName != "MHFZ_Overlay")
            {

                // processName is a substring of one of the banned process strings
                logger.Fatal("Found banned process {0}", process.ProcessName);
                MessageBox.Show($"Close other external programs before opening the overlay ({process.ProcessName} found)", Messages.FATAL_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);

                // Close the overlay program
                ApplicationManager.HandleShutdown();
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
            logger.Fatal("Found duplicate overlay");
            MessageBox.Show("Close other instances of the overlay before opening a new one", Messages.FATAL_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);

            // Close the overlay program
            ApplicationManager.HandleShutdown();
        }
        if (gameCount > 1)
        {
            // More than one game process is running
            logger.Fatal("Found duplicate game");
            MessageBox.Show("Close other instances of the game before opening a new one", Messages.FATAL_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);

            // Close the overlay program
            ApplicationManager.HandleShutdown();
        }
    }

    // This checks for illegal folders or files in the game folder
    // TODO: test
    public void CheckForIllegalModifications()
    {
        if (App.isClowdSquirrelUpdating)
            return;

        try 
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
                logger.Fatal("mhf.exe not found");
                MessageBox.Show("The 'mhf.exe' process was not found. You may have closed the game. Closing overlay.", Messages.FATAL_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationManager.HandleShutdown();
            }
        } 
        catch (Exception ex)
        {
            LoggingManager.WriteCrashLog(ex);
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

    private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();

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
            logger.Fatal("Launch game first");
            System.Windows.MessageBox.Show("Please launch game first", Messages.ERROR_TITLE, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            ApplicationManager.HandleShutdown();
            return;
        }
        long searchAddress = m.AoBScan("89 04 8D 00 C6 43 00 61 E9").Result.FirstOrDefault();
        if (searchAddress.ToString("X8") == "00000000")
        {
            logger.Info("Creating code cave");

            // Create code cave and get its address
            long baseScanAddress = m.AoBScan("0F B7 8a 24 06 00 00 0f b7 ?? ?? ?? c1 c1 e1 0b").Result.FirstOrDefault();
            UIntPtr codecaveAddress = m.CreateCodeCave(baseScanAddress.ToString("X8"), new byte[] { 0x0F, 0xB7, 0x8A, 0x24, 0x06, 0x00, 0x00, 0x0F, 0xB7, 0x52, 0x0C, 0x88, 0x15, 0x21, 0x00, 0x0F, 0x15, 0x8B, 0xC1, 0xC1, 0xE1, 0x0B, 0x0F, 0xBF, 0xC9, 0xC1, 0xE8, 0x05, 0x09, 0xC8, 0x01, 0xD2, 0xB9, 0x8E, 0x76, 0x21, 0x25, 0x29, 0xD1, 0x66, 0x8B, 0x11, 0x66, 0xF7, 0xD2, 0x0F, 0xBF, 0xCA, 0x0F, 0xBF, 0x15, 0xC4, 0x22, 0xEA, 0x17, 0x31, 0xC8, 0x31, 0xD0, 0xB9, 0xC0, 0x5E, 0x73, 0x16, 0x0F, 0xBF, 0xD1, 0x31, 0xD0, 0x60, 0x8B, 0x0D, 0x21, 0x00, 0x0F, 0x15, 0x89, 0x04, 0x8D, 0x00, 0xC6, 0x43, 0x00, 0x61 }, 63, 0x100);

            // Change addresses
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
        double totalQuestDuration = (double)model.TimeDefInt() / Numbers.FRAMES_PER_SECOND; // Total duration of the quest in seconds
        double timeRemainingInQuest = (double)model.TimeInt() / Numbers.FRAMES_PER_SECOND; // Time left in the quest in seconds

        // Calculate the elapsed time by subtracting the time left from the total duration
        double elapsedTime = totalQuestDuration - timeRemainingInQuest;

        // Convert the elapsed time from seconds to milliseconds
        elapsedTime *= 1_000;

        // Convert the elapsed time to a TimeSpan object
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(elapsedTime);

        // Format the TimeSpan object as a string
        string formattedTime = timeSpan.ToString(TimeFormats.MINUTES_SECONDS_MILLISECONDS);

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
        logger.Info("Loading MHFODLL");
        //Search and get mhfo-hd.dll module base address
        Process proccess = Process.GetProcessById(PID);
        if (proccess == null)
        {
            logger.Warn("Process not found");
            return null;
        }
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
            if (index > 0)
            {
                isHighGradeEdition = false;
            }
            else
            {
                logger.Fatal("Could not find game dll");
                MessageBox.Show("Could not find game dll. Make sure you start the overlay inside Mezeporta.", Messages.FATAL_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationManager.HandleShutdown();
            }
        }
        return proccess;
    }
}
