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
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.ViewModels.Windows;

/// <summary>
/// Responsible for loading data into the application. It has a DatabaseManager object that is used to access and manipulate the database. It also has instances of AddressModelNotHGE and AddressModelHGE classes, which inherit from the AddressModel abstract class. Depending on the state of the game, one of these instances is used to get the hit count value (etc.) from the memory.
/// </summary>
public sealed class DataLoader
{
    private static readonly NLog.Logger LoggerInstance = NLog.LogManager.GetCurrentClassLogger();

    private static readonly DatabaseService DatabaseManager = DatabaseService.GetInstance();

    private readonly List<string> bannedProcesses = new ()
    {
        "displayer", "Displayer", "cheat", "Cheat", "overlay", "Overlay", "Wireshark", "FreeCam",
    };

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

        var pID = this.m.GetProcIdFromName("mhf");
        if (pID > 0)
        {
            this.m.OpenProcess(pID);
            try
            {
                this.CreateCodeCave(pID);
            }
            catch (Exception ex)
            {
                // TODO: does this warrant the program closing?
                // ReShade or similar programs might trigger this warning. Does these affect overlay functionality?
                // Imulion's version does not have anything in the catch block.
                // I'm marking this as error since overlay might interfere with custom shaders.
                LoggerInstance.Error(ex, "Could not create code cave");
                MessageBox.Show("Could not create code cave. ReShade or similar programs might trigger this error. Also make sure you are not loading the overlay when on game launcher.", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!this.IsHighGradeEdition)
            {
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Running game in Non-HGE");
                this.Model = new AddressModelNotHGE(this.m);
            }
            else
            {
                LoggerInstance.Info(CultureInfo.InvariantCulture, "Running game in HGE");
                this.Model = new AddressModelHGE(this.m);
            }

            // first we check if there are duplicate mhf.exe
            this.CheckForExternalProcesses();
            this.CheckForIllegalModifications();

            // if there aren't then this runs and sets the game folder and also the database folder if needed
            GetMHFFolderLocation();

            // and thus set the data to database then, after doing it to the settings
            this.DatabaseChanged = DatabaseManager.SetupLocalDatabase(this);
            var overlayHash = DatabaseManager.StoreOverlayHash();
            this.CheckIfLoadedInMezeporta();
            this.CheckIfLoadedOutsideQuest();
        }
        else
        {
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Launch game first");
            MessageBox.Show("Please launch game first", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            ApplicationService.HandleShutdown();
        }

        // Stop the stopwatch
        stopwatch.Stop();

        // Get the elapsed time in milliseconds
        var elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        // Print the elapsed time
        LoggerInstance.Debug($"DataLoader ctor Elapsed Time: {elapsedTimeMs} ms");
    }

    public bool LoadedOutsideMezeporta { get; set; }

    private void CheckIfLoadedInMezeporta()
    {
        if (this.Model.AreaID() != 200)
        {
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Loaded overlay outside Mezeporta");

            var s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableOutsideMezeportaLoadingWarning)
            {
                this.LoadedOutsideMezeporta = true;
            }
        }
    }

    private void CheckIfLoadedOutsideQuest()
    {
        if (this.Model.QuestID() != 0)
        {
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Loaded overlay inside quest {0}", this.Model.QuestID());
            MessageBox.Show("Loaded overlay inside quest. Please load the overlay outside quests.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            LoggingService.WriteCrashLog(new Exception("Loaded overlay inside quest"));
        }
    }

    private static void GetMHFFolderLocation()
    {
        // Get the process that is running "mhf.exe"
        var processes = Process.GetProcessesByName("mhf");

        var s = (Settings)Application.Current.TryFindResource("Settings");

        if (processes.Length > 0)
        {
            var module = processes[0].MainModule;
            if (module == null)
            {
                // The "mhf.exe" process was not found
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                MessageBox.Show("The 'mhf.exe' process was not found.", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationService.HandleShutdown();
                return;
            }

            // Get the location of the first "mhf.exe" process
            var mhfPath = module.FileName;

            // Get the directory that contains "mhf.exe"
            var mhfDirectory = Path.GetDirectoryName(mhfPath);

            if (mhfDirectory == null)
            {
                // The "mhf.exe" process was not found
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                MessageBox.Show("The 'mhf.exe' process was not found.", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationService.HandleShutdown();
                return;
            }

            // Save the directory to the program's settings
            // (Assuming you have a "settings" object that can store strings)
            s.GameFolderPath = mhfDirectory;

            // Check if the "database" folder exists in the "mhf" folder
            var databasePath = Path.Combine(mhfDirectory, "database");
            if (!Directory.Exists(databasePath))
            {
                // Create the "database" folder if it doesn't exist
                Directory.CreateDirectory(databasePath);
            }

            // Check if the "MHFZ_Overlay.sqlite" file exists in the "database" folder
            var sqlitePath = Path.Combine(databasePath, "MHFZ_Overlay.sqlite");

            s.DatabaseFilePath = sqlitePath;

            // Check if the version file exists in the database folder
            var previousVersionPath = Path.Combine(databasePath, "previous-version.txt");
            FileService.CreateFileIfNotExists(previousVersionPath, "Creating version file at ");

            s.PreviousVersionFilePath = previousVersionPath;

            s.Save();
        }
        else
        {
            // The "mhf.exe" process was not found
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
            MessageBox.Show("The 'mhf.exe' process was not found.", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            ApplicationService.HandleShutdown();
        }
    }

    private readonly List<string> bannedFiles = new ()
    {
        "d3d8", "d3d9", "d3d10", "d3d11", "d3d12", "ddraw", "dinput", "dinput8", "dsound",
        "msacm32", "msvfw32", "version", "wininet", "winmm", "xlive", "bink2w64", "bink2w64Hooked",
        "vorbisFile", "vorbisHooked", "binkw32", "binkw32Hooked",
    };

    private readonly List<string> bannedFileExtensions = new ()
    {
        ".asi",
    };

    private readonly List<string> bannedFolders = new ()
    {
        "scripts", "plugins", "script", "plugin", "localize-dat",
    };

    private readonly List<string> bannedFoldersInSpeedruns = new()
    {
        "scripts", "plugins", "script", "plugin", "localize-dat", "mods"
    };

    private readonly List<string> allowedProcesses = new ()
    {
        "LogiOverlay", // Logitech Bluetooth for mouse
        "GameOverlayUI", // Steam
    };

    // needed for getting data
    private readonly Mem m = new ();

    /// <summary>
    /// Index for the dll.
    /// </summary>
    private int index { get; set; }

    public bool DatabaseChanged { get; set; }

    // TODO: savvy users know the bypass. Short of making the overlay too involved in the user's machine,
    // there are currently no workarounds.
    public void CheckForExternalProcesses()
    {
        if (Program.IsVelopackUpdating || DatabaseManager.SchemaChanged)
        {
            return;
        }

        var processList = Process.GetProcesses();
        var overlayCount = 0;
        var gameCount = 0;

        // first we check for steam overlay, if found then skip to next iteration
        // then we apply the whitelist, if found then skip to next iteration
        // then we apply the blacklist
        // then we check if the process is the overlay
        // then we check if the process is the game
        // then we check for disallowed duplicates
        foreach (var process in processList)
        {
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
                ApplicationService.HandleShutdown();
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
            ApplicationService.HandleShutdown();
        }

        if (gameCount > 1)
        {
            // More than one game process is running
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Found duplicate game");
            MessageBox.Show("Close other instances of the game before opening a new one", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);

            // Close the overlay program
            ApplicationService.HandleShutdown();
        }
    }

    // This checks for illegal folders or files in the game folder
    // TODO: test
    public void CheckForIllegalModifications(DataLoader? dataLoader = null)
    {
        if (Program.IsVelopackUpdating)
        {
            return;
        }

        try
        {
            // Get the process that is running "mhf.exe"
            var processes = Process.GetProcessesByName("mhf");

            if (processes.Length > 0)
            {
                var module = processes[0].MainModule;

                if (module == null)
                {
                    // The "mhf.exe" process was not found
                    LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                    MessageBox.Show("The 'mhf.exe' process was not found. You may have closed the game. Closing overlay.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    ApplicationService.HandleShutdown();
                    return;
                }

                // Get the location of the first "mhf.exe" process
                var mhfPath = module.FileName;

                // Get the directory that contains "mhf.exe"
                var mhfDirectory = Path.GetDirectoryName(mhfPath);

                if (string.IsNullOrEmpty(mhfDirectory))
                {
                    // The "mhf.exe" process was not found
                    LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                    MessageBox.Show("The 'mhf.exe' process was not found. You may have closed the game. Closing overlay.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    ApplicationService.HandleShutdown();
                    return;
                }

                // Get a list of all files and folders in the game folder
                var files = Directory.GetFiles(mhfDirectory, "*", SearchOption.AllDirectories);
                var folders = Directory.GetDirectories(mhfDirectory, "*", SearchOption.AllDirectories);
                var isFatal = true;
                if (dataLoader != null && dataLoader.Model.GetOverlayMode() is OverlayMode.Speedrun)
                {
                    FileService.CheckIfFileExtensionFolderExists(files, folders, this.bannedFiles, this.bannedFileExtensions, this.bannedFoldersInSpeedruns, isFatal);
                }
                else
                {
                    FileService.CheckIfFileExtensionFolderExists(files, folders, this.bannedFiles, this.bannedFileExtensions, this.bannedFolders, isFatal);
                }
            }
            else
            {
                // The "mhf.exe" process was not found
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "mhf.exe not found");
                MessageBox.Show("The 'mhf.exe' process was not found. You may have closed the game. Closing overlay.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationService.HandleShutdown();
            }
        }
        catch (Exception ex)
        {
            LoggingService.WriteCrashLog(ex);
        }
    }

    public bool IsHighGradeEdition { get; set; }

    /// <summary>
    /// Gets the model.
    /// </summary>
    /// <value>
    /// The model.
    /// </value>
    public AddressModel Model { get; } // TODO: fix null warning

    /// <summary>
    /// Creates the code cave.
    /// </summary>
    /// <param name="pID">The pid.</param>
    private void CreateCodeCave(int pID)
    {
        // TODO why is this needed?
        var proc = this.LoadMHFODLL(pID);
        if (proc == null)
        {
            LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Launch game first");
            MessageBox.Show("Please launch game first", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            ApplicationService.HandleShutdown();
            return;
        }

        var searchAddress = this.m.AoBScan("89 04 8D 00 C6 43 00 61 E9").Result.FirstOrDefault();
        if (searchAddress.ToString("X8", CultureInfo.InvariantCulture) == "00000000")
        {
            LoggerInstance.Info(CultureInfo.InvariantCulture, "Creating code cave");

            // Create code cave and get its address
            var baseScanAddress = this.m.AoBScan("0F B7 8a 24 06 00 00 0f b7 ?? ?? ?? c1 c1 e1 0b").Result.FirstOrDefault();
            var codecaveAddress = this.m.CreateCodeCave(baseScanAddress.ToString("X8", CultureInfo.InvariantCulture), new byte[] { 0x0F, 0xB7, 0x8A, 0x24, 0x06, 0x00, 0x00, 0x0F, 0xB7, 0x52, 0x0C, 0x88, 0x15, 0x21, 0x00, 0x0F, 0x15, 0x8B, 0xC1, 0xC1, 0xE1, 0x0B, 0x0F, 0xBF, 0xC9, 0xC1, 0xE8, 0x05, 0x09, 0xC8, 0x01, 0xD2, 0xB9, 0x8E, 0x76, 0x21, 0x25, 0x29, 0xD1, 0x66, 0x8B, 0x11, 0x66, 0xF7, 0xD2, 0x0F, 0xBF, 0xCA, 0x0F, 0xBF, 0x15, 0xC4, 0x22, 0xEA, 0x17, 0x31, 0xC8, 0x31, 0xD0, 0xB9, 0xC0, 0x5E, 0x73, 0x16, 0x0F, 0xBF, 0xD1, 0x31, 0xD0, 0x60, 0x8B, 0x0D, 0x21, 0x00, 0x0F, 0x15, 0x89, 0x04, 0x8D, 0x00, 0xC6, 0x43, 0x00, 0x61 }, 63, 0x100);

            // Change addresses
            var storeValueAddress = codecaveAddress + 125;                  // address where store some value?
            var storeValueAddressString = storeValueAddress.ToString("X8", CultureInfo.InvariantCulture);
            var storeValueAddressByte = Enumerable.Range(0, storeValueAddressString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(storeValueAddressString.Substring(x, 2), 16)).ToArray();
            Array.Reverse(storeValueAddressByte, 0, storeValueAddressByte.Length);
            byte[] by15 = { 136, 21 };
            this.m.WriteBytes(codecaveAddress + 11, by15);
            this.m.WriteBytes(codecaveAddress + 13, storeValueAddressByte);  // 1
            this.m.WriteBytes(codecaveAddress + 72, storeValueAddressByte);  // 2

            this.WriteByteFromAddress(codecaveAddress, proc, this.IsHighGradeEdition ? 249263758 : 102223598, 33);
            this.WriteByteFromAddress(codecaveAddress, proc, this.IsHighGradeEdition ? 27534020 : 27601756, 51);
            this.WriteByteFromAddress(codecaveAddress, proc, this.IsHighGradeEdition ? 2973376 : 2865056, 60);
        }
        else
        {
            this.LoadMHFODLL(pID);
        }
    }

    /// <summary>
    /// Writes the byte from address.
    /// </summary>
    /// <param name="codecaveAddress">The codecave address.</param>
    /// <param name="proc">The proc.</param>
    /// <param name="offset1">The offset1.</param>
    /// <param name="offset2">The offset2.</param>
    private void WriteByteFromAddress(UIntPtr codecaveAddress, Process proc, long offset1, int offset2)
    {
        var address = proc.Modules[this.index].BaseAddress.ToInt32() + offset1;
        var addressString = address.ToString("X8", CultureInfo.InvariantCulture);
        var addressByte = Enumerable.Range(0, addressString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(addressString.Substring(x, 2), 16)).ToArray();
        Array.Reverse(addressByte, 0, addressByte.Length);
        this.m.WriteBytes(codecaveAddress + offset2, addressByte);
    }

    /// <summary>
    /// Loads the mhfo.dll.
    /// </summary>
    /// <param name="pID">The pid.</param>
    /// <returns></returns>
    private Process? LoadMHFODLL(int pID)
    {
        LoggerInstance.Info(CultureInfo.InvariantCulture, "Loading MHFODLL");

        // Search and get mhfo-hd.dll module base address
        var proccess = Process.GetProcessById(pID);
        if (proccess == null)
        {
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Process not found");
            return null;
        }

        var moduleList = new List<string>();
        foreach (ProcessModule md in proccess.Modules)
        {
            var moduleName = md.ModuleName;
            if (moduleName != null)
            {
                moduleList.Add(moduleName);
            }
        }

        this.index = moduleList.IndexOf("mhfo-hd.dll");
        if (this.index > 0)
        {
            this.IsHighGradeEdition = true;
        }
        else
        {
            this.index = moduleList.IndexOf("mhfo.dll");
            if (this.index > 0)
            {
                this.IsHighGradeEdition = false;
            }
            else
            {
                LoggerInstance.Fatal(CultureInfo.InvariantCulture, "Could not find game dll");
                MessageBox.Show("Could not find game dll. Make sure you start the overlay inside Mezeporta.", Messages.FatalTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationService.HandleShutdown();
            }
        }

        return proccess;
    }
}
