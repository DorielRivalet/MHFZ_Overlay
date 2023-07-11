// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

// TODO separation of concerns
/*
 
 
 To make the DataLoader class clearly fit into a single category in the MVVM pattern,
you can refactor it by separating its responsibilities into distinct components.
Here's a suggestion on how you can achieve this:

Service/Model: Move the data loading and database management responsibilities to
a dedicated service class, such as DataLoadService.
This service will be responsible for initializing the DatabaseManager,
checking external processes and illegal modifications, and interacting with memory addresses.
It can expose methods or properties to retrieve game data or perform data-related operations.
This will help separate data-related concerns from the other responsibilities of DataLoader.

ViewModel: Extract the logic related to game state checks and warnings into a separate ViewModel class,
such as GameStatusViewModel. This ViewModel can have properties and commands
that represent the game state and provide warnings or error messages to the View based on that state.
It can utilize the DataLoadService to fetch relevant game data.

View: The MainWindow.xaml and MainWindow.xaml.cs files will remain as the View components,
responsible for displaying the UI and interacting with the ViewModel.

By following this approach, you achieve a clearer separation of concerns:

The DataLoadService handles the data loading, database management,
and memory address interactions (service/model).
The GameStatusViewModel contains the logic for game state checks and warnings (viewmodel).
The MainWindow acts as the user interface (view).
This separation allows for better maintainability, testability, and adherence to the MVVM pattern.
Each component has a well-defined responsibility,
making it easier to understand and modify the codebase.
 
 */

namespace MHFZ_Overlay;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Memory;
using MHFZ_Overlay.Models.Addresses;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Services.DataAccessLayer;
using MHFZ_Overlay.Services.Manager;
using MHFZ_Overlay.ViewModels;

/// <summary>
/// Responsible for loading data into the application. It has a DatabaseManager object that is used to access and manipulate the database. It also has instances of AddressModelNotHGE and AddressModelHGE classes, which inherit from the AddressModel abstract class. Depending on the state of the game, one of these instances is used to get the hit count value (etc.) from the memory.
/// </summary>
public class DataLoader
{
    private static readonly NLog.Logger LoggerInstance = NLog.LogManager.GetCurrentClassLogger();

    // TODO: would like to make this a singleton but its complicated
    // this loads first before MainWindow constructor is called. meaning this runs twice.
    public DataLoader()
    {
        // Create a Stopwatch instance
        var stopwatch = new Stopwatch();

        // Start the stopwatch
        stopwatch.Start();
        LoggerInstance.Trace(CultureInfo.InvariantCulture, "DataLoader constructor called. Call stack: {0}", new StackTrace().ToString());
        LoggerInstance.Info(CultureInfo.InvariantCulture, $"DataLoader initialized");

        var PID = this.m.GetProcIdFromName("mhf");
        if (PID > 0)
        {
            this.m.OpenProcess(PID);
            try
            {
                this.CreateCodeCave(PID);
            }
            catch (Exception ex)
            {
                // TODO: does this warrant the program closing?
                // ReShade or similar programs might trigger this warning. Does these affect overlay functionality?
                // Imulion's version does not have anything in the catch block.
                // I'm marking this as error since overlay might interfere with custom shaders.
                LoggerInstance.Error(ex, "Could not create code cave");
                System.Windows.MessageBox.Show("Could not create code cave. ReShade or similar programs might trigger this error. Also make sure you are not loading the overlay when on game launcher.", Messages.ErrorTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            if (!this.isHighGradeEdition)
            {
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Running game in Non-HGE");
                this.model = new AddressModelNotHGE(this.m);
            }
            else
            {
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Running game in HGE");
                this.model = new AddressModelHGE(this.m);
            }

            // first we check if there are duplicate mhf.exe
            this.CheckForExternalProcesses();
            this.CheckForIllegalModifications();

            // if there aren't then this runs and sets the game folder and also the database folder if needed
            this.GetMHFFolderLocation();

            // and thus set the data to database then, after doing it to the settings
            this.databaseChanged = databaseManager.SetupLocalDatabase(this);
            string overlayHash = databaseManager.StoreOverlayHash();
            this.CheckIfLoadedInMezeporta();
            this.CheckIfLoadedOutsideQuest();
        }
        else
        {
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Launch game first");
            System.Windows.MessageBox.Show("Please launch game first", Messages.ErrorTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            ApplicationManager.HandleShutdown();
        }

        // Stop the stopwatch
        stopwatch.Stop();

        // Get the elapsed time in milliseconds
        double elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        // Print the elapsed time
        LoggerInstance.Debug($"DataLoader ctor Elapsed Time: {elapsedTimeMs} ms");
    }

    public bool loadedOutsideMezeporta { get; set; }

    private void CheckIfLoadedInMezeporta()
    {
        if (this.model.AreaID() != 200)
        {
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Loaded overlay outside Mezeporta");

            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            if (s.EnableOutsideMezeportaLoadingWarning)
                this.loadedOutsideMezeporta = true;
        }
    }

    private void CheckIfLoadedOutsideQuest()
    {
        if (this.model.QuestID() != 0)
        {
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Loaded overlay inside quest {0}", this.model.QuestID());
            System.Windows.MessageBox.Show("Loaded overlay inside quest. Please load the overlay outside quests.", Messages.FatalTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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
            var module = processes[0].MainModule;
            if (module == null)
            {
                // The "mhf.exe" process was not found
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                MessageBox.Show("The 'mhf.exe' process was not found.", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationManager.HandleShutdown();
                return;
            }

            // Get the location of the first "mhf.exe" process
            string? mhfPath = module.FileName;

            // Get the directory that contains "mhf.exe"
            string? mhfDirectory = Path.GetDirectoryName(mhfPath);

            if (mhfDirectory == null)
            {
                // The "mhf.exe" process was not found
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                MessageBox.Show("The 'mhf.exe' process was not found.", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationManager.HandleShutdown();
                return;
            }

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
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
            MessageBox.Show("The 'mhf.exe' process was not found.", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
        {
            return;
        }

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
            if (process.ProcessName == "GameOverlayUI" && !this.steamOverlayWarningShown)
            {
                LoggerInstance.Warn(CultureInfo.InvariantCulture, "Found Steam overlay: {0}", process.ProcessName);
                var result = MessageBox.Show($"Having Steam Overlay open while MHF-Z Overlay is running may decrease performance. ({process.ProcessName} found)", Messages.WarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    this.steamOverlayWarningShown = true;
                }

                continue;
            }

            if (this.allowedProcesses.Any(s => process.ProcessName.Contains(s)) && process.ProcessName != "MHFZ_Overlay")
            {
                continue;
            }

            if (this.bannedProcesses.Any(s => process.ProcessName.Contains(s)) && process.ProcessName != "MHFZ_Overlay")
            {

                // processName is a substring of one of the banned process strings
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Found banned process {0}", process.ProcessName);
                MessageBox.Show($"Close other external programs before opening the overlay ({process.ProcessName} found)", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);

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
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Found duplicate overlay");
            MessageBox.Show("Close other instances of the overlay before opening a new one", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);

            // Close the overlay program
            ApplicationManager.HandleShutdown();
        }

        if (gameCount > 1)
        {
            // More than one game process is running
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Found duplicate game");
            MessageBox.Show("Close other instances of the game before opening a new one", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);

            // Close the overlay program
            ApplicationManager.HandleShutdown();
        }
    }

    // This checks for illegal folders or files in the game folder
    // TODO: test
    public void CheckForIllegalModifications()
    {
        if (App.isClowdSquirrelUpdating)
        {
            return;
        }

        try
        {
            // Get the process that is running "mhf.exe"
            Process[] processes = Process.GetProcessesByName("mhf");

            if (processes.Length > 0)
            {
                var module = processes[0].MainModule;

                if (module == null)
                {
                    // The "mhf.exe" process was not found
                    LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                    MessageBox.Show("The 'mhf.exe' process was not found. You may have closed the game. Closing overlay.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    ApplicationManager.HandleShutdown();
                    return;
                }

                // Get the location of the first "mhf.exe" process
                string? mhfPath = module.FileName;

                // Get the directory that contains "mhf.exe"
                string? mhfDirectory = Path.GetDirectoryName(mhfPath);

                if (string.IsNullOrEmpty(mhfDirectory))
                {
                    // The "mhf.exe" process was not found
                    LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                    MessageBox.Show("The 'mhf.exe' process was not found. You may have closed the game. Closing overlay.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    ApplicationManager.HandleShutdown();
                    return;
                }

                // Get a list of all files and folders in the game folder
                string[] files = Directory.GetFiles(mhfDirectory, "*", SearchOption.AllDirectories);
                string[] folders = Directory.GetDirectories(mhfDirectory, "*", SearchOption.AllDirectories);
                var isFatal = true;
                FileManager.CheckIfFileExtensionFolderExists(files, folders, this.bannedFiles, this.bannedFileExtensions, this.bannedFolders, isFatal);
            }
            else
            {
                // The "mhf.exe" process was not found
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                MessageBox.Show("The 'mhf.exe' process was not found. You may have closed the game. Closing overlay.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationManager.HandleShutdown();
            }
        }
        catch (Exception ex)
        {
            LoggingManager.WriteCrashLog(ex);
        }
    }

    public bool databaseChanged { get; set; }

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
    public AddressModel model { get; } // TODO: fix null warning

    private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();

    /// <summary>
    /// Creates the code cave.
    /// </summary>
    /// <param name="PID">The pid.</param>
    private void CreateCodeCave(int PID)
    {
        // TODO why is this needed?
        Process? proc = this.LoadMHFODLL(PID);
        if (proc == null)
        {
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Launch game first");
            System.Windows.MessageBox.Show("Please launch game first", Messages.ErrorTitle, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            ApplicationManager.HandleShutdown();
            return;
        }

        long searchAddress = this.m.AoBScan("89 04 8D 00 C6 43 00 61 E9").Result.FirstOrDefault();
        if (searchAddress.ToString("X8", CultureInfo.InvariantCulture) == "00000000")
        {
            LoggerInstance.Info(CultureInfo.InvariantCulture, "Creating code cave");

            // Create code cave and get its address
            long baseScanAddress = this.m.AoBScan("0F B7 8a 24 06 00 00 0f b7 ?? ?? ?? c1 c1 e1 0b").Result.FirstOrDefault();
            UIntPtr codecaveAddress = this.m.CreateCodeCave(baseScanAddress.ToString("X8", CultureInfo.InvariantCulture), new byte[] { 0x0F, 0xB7, 0x8A, 0x24, 0x06, 0x00, 0x00, 0x0F, 0xB7, 0x52, 0x0C, 0x88, 0x15, 0x21, 0x00, 0x0F, 0x15, 0x8B, 0xC1, 0xC1, 0xE1, 0x0B, 0x0F, 0xBF, 0xC9, 0xC1, 0xE8, 0x05, 0x09, 0xC8, 0x01, 0xD2, 0xB9, 0x8E, 0x76, 0x21, 0x25, 0x29, 0xD1, 0x66, 0x8B, 0x11, 0x66, 0xF7, 0xD2, 0x0F, 0xBF, 0xCA, 0x0F, 0xBF, 0x15, 0xC4, 0x22, 0xEA, 0x17, 0x31, 0xC8, 0x31, 0xD0, 0xB9, 0xC0, 0x5E, 0x73, 0x16, 0x0F, 0xBF, 0xD1, 0x31, 0xD0, 0x60, 0x8B, 0x0D, 0x21, 0x00, 0x0F, 0x15, 0x89, 0x04, 0x8D, 0x00, 0xC6, 0x43, 0x00, 0x61 }, 63, 0x100);

            // Change addresses
            UIntPtr storeValueAddress = codecaveAddress + 125;                  //address where store some value?
            string storeValueAddressString = storeValueAddress.ToString("X8", CultureInfo.InvariantCulture);
            byte[] storeValueAddressByte = Enumerable.Range(0, storeValueAddressString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(storeValueAddressString.Substring(x, 2), 16)).ToArray();
            Array.Reverse(storeValueAddressByte, 0, storeValueAddressByte.Length);
            byte[] by15 = { 136, 21 };
            this.m.WriteBytes(codecaveAddress + 11, by15);
            this.m.WriteBytes(codecaveAddress + 13, storeValueAddressByte);  //1
            this.m.WriteBytes(codecaveAddress + 72, storeValueAddressByte);  //2

            this.WriteByteFromAddress(codecaveAddress, proc, this.isHighGradeEdition ? 249263758 : 102223598, 33);
            this.WriteByteFromAddress(codecaveAddress, proc, this.isHighGradeEdition ? 27534020 : 27601756, 51);
            this.WriteByteFromAddress(codecaveAddress, proc, this.isHighGradeEdition ? 2973376 : 2865056, 60);

        }
        else
        {
            this.LoadMHFODLL(PID);
        }
    }

    public string GetQuestTimeCompletion()
    {
        double totalQuestDuration = (double)this.model.TimeDefInt() / Numbers.FramesPerSecond; // Total duration of the quest in seconds
        double timeRemainingInQuest = (double)this.model.TimeInt() / Numbers.FramesPerSecond; // Time left in the quest in seconds

        // Calculate the elapsed time by subtracting the time left from the total duration
        double elapsedTime = totalQuestDuration - timeRemainingInQuest;

        // Convert the elapsed time from seconds to milliseconds
        elapsedTime *= 1_000;

        // Convert the elapsed time to a TimeSpan object
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(elapsedTime);

        // Format the TimeSpan object as a string
        string formattedTime = timeSpan.ToString(TimeFormats.MinutesSecondsMilliseconds, CultureInfo.InvariantCulture);

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
        long address = proc.Modules[this.index].BaseAddress.ToInt32() + offset1;
        string addressString = address.ToString("X8", CultureInfo.InvariantCulture);
        byte[] addressByte = Enumerable.Range(0, addressString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(addressString.Substring(x, 2), 16)).ToArray();
        Array.Reverse(addressByte, 0, addressByte.Length);
        this.m.WriteBytes(codecaveAddress + offset2, addressByte);
    }

    /// <summary>
    /// Loads the mhfo.dll.
    /// </summary>
    /// <param name="PID">The pid.</param>
    /// <returns></returns>
    Process? LoadMHFODLL(int PID)
    {
        LoggerInstance.Info(CultureInfo.InvariantCulture, "Loading MHFODLL");

        //Search and get mhfo-hd.dll module base address
        Process proccess = Process.GetProcessById(PID);
        if (proccess == null)
        {
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Process not found");
            return null;
        }

        var ModuleList = new List<string>();
        foreach (ProcessModule md in proccess.Modules)
        {
            string? moduleName = md.ModuleName;
            if (moduleName != null)
            {
                ModuleList.Add(moduleName);
            }
        }

        this.index = ModuleList.IndexOf("mhfo-hd.dll");
        if (this.index > 0)
        {
            this.isHighGradeEdition = true;
        }
        else
        {
            this.index = ModuleList.IndexOf("mhfo.dll");
            if (this.index > 0)
            {
                this.isHighGradeEdition = false;
            }
            else
            {
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Could not find game dll");
                MessageBox.Show("Could not find game dll. Make sure you start the overlay inside Mezeporta.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationManager.HandleShutdown();
            }
        }

        return proccess;
    }
}
