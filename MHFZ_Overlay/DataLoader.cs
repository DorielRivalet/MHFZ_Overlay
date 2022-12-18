using Dictionary;
using Memory;
using MHFZ_Overlay.addresses;
using SQLitePCL;
using Octokit;
using Squirrel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Data.SQLite;

namespace MHFZ_Overlay
{
    /// <summary>
    /// DataLoader
    /// </summary>
    public class DataLoader
    {
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
            if (firstRun) MessageBox.Show("【MHF-Z】Overlay is now running! Thanks for installing【MHF-Z】Overlay.\n\nHotkeys: Shift+F1 (Configuration) | Shift+F5 (Restart) | Shift+F6 (Close)\n\nPress Alt+Enter if your game resolution changed.", "MHF-Z Overlay Installation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoader"/> class.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable property 'model' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public DataLoader()
#pragma warning restore CS8618 // Non-nullable property 'model' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        {
            // run Squirrel first, as the app may exit after these run
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: OnAppInstall,
                onAppUninstall: OnAppUninstall,
                onEveryRun: OnAppRun);

            // ... other app init code after ...

            int PID = m.GetProcIdFromName("mhf");
            if (PID > 0)
            {
                m.OpenProcess(PID);
                try
                {
                    CreateCodeCave(PID);
                }
                catch (Exception)
                {
                    // hi
                }

                if (!isHighGradeEdition)
                    model = new AddressModelNotHGE(m);
                else
                    model = new AddressModelHGE(m);

                SetupLocalDatabase();
            }
            else
            {
                System.Windows.MessageBox.Show("Please launch game first", "Error - MHFZ Overlay", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                App.Current.Shutdown();
            }
        }

        /// <summary>
        /// Creates the code cave.
        /// </summary>
        /// <param name="PID">The pid.</param>
        private void CreateCodeCave(int PID)
        {
            Process? proc = LoadMHFODLL(PID);
            if (proc == null)
            {
                System.Windows.MessageBox.Show("Please launch game first", "Error - MHFZ Overlay", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                App.Current.Shutdown();
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

        string GetQuestTimeCompletion()
        {
            double totalQuestDuration = model.TimeDefInt() / 30; // Total duration of the quest in seconds
            double timeRemainingInQuest = model.TimeInt() / 30; // Time left in the quest in seconds

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

        public static List<Dictionary<int, string>> GetArmorDictionariesList()
        {
            return new List<Dictionary<int, string>>
            {
                (Dictionary<int, string>)ArmorHeads.ArmorHeadIDs,
                (Dictionary<int, string>)ArmorChests.ArmorChestIDs,
                (Dictionary<int, string>)ArmorArms.ArmorArmIDs,
                (Dictionary<int, string>)ArmorWaists.ArmorWaistIDs,
                (Dictionary<int, string>)ArmorLegs.ArmorLegIDs
            };
        }

        #region database

        void CreateDatabaseTables(SQLiteConnection conn)
        {   
            // Create table to store program usage time
            string sql = @"CREATE TABLE IF NOT EXISTS Session (
            SessionID INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime DATETIME NOT NULL,
            EndTime DATETIME NOT NULL,
            SessionDuration INTEGER NOT NULL)";
            SQLiteCommand createTableCommand = new SQLiteCommand(sql, conn);
            createTableCommand.ExecuteNonQuery();

            // Create the Quests table
            //sql = @"CREATE TABLE IF NOT EXISTS Quests 
            //(RunID INTEGER PRIMARY KEY AUTOINCREMENT, 
            //QuestID INTEGER, 
            //AreaID INTEGER,
            //QuestName TEXT, 
            //EndTime DATETIME, 
            //ObjectiveImage BLOB,
            //ObjectiveType TEXT, 
            //ObjectiveQuantity INTEGER, 
            //StarGrade INTEGER, 
            //RankName TEXT, 
            //ObjectiveName TEXT, 
            //Date DATETIME)";
            //SqliteCommand cmd = new SqliteCommand(sql, conn);
            //cmd.ExecuteNonQuery();

            //// Calculate the elapsed time of the quest
            //string questTime = GetQuestTimeCompletion();

            //// Convert the elapsed time to a DateTime object
            //DateTime endTime = DateTime.ParseExact(questTime, @"mm\:ss\.ff", CultureInfo.InvariantCulture);

            //string objectiveType = model.GetObjectiveNameFromID(model.ObjectiveType());
            //string objectiveName = "Not Loaded";
            //string rankName = model.GetRankNameFromID(model.RankBand());
            //int objectiveQuantity = int.Parse(model.GetObjective1Quantity());
            //int starGrade = model.StarGrades();

            //if ((model.ObjectiveType() == 0x0 || model.ObjectiveType() == 0x02 || model.ObjectiveType() == 0x1002 || model.ObjectiveType() == 0x10) && (model.QuestID() != 23527 && model.QuestID() != 23628 && model.QuestID() != 21731 && model.QuestID() != 21749 && model.QuestID() != 21746 && model.QuestID() != 21750))
            //    objectiveName = model.GetObjective1Name(model.Objective1ID());
            //else
            //    objectiveName = model.GetRealMonsterName(model.CurrentMonster1Icon);

            //// Create the Players table
            //sql = @"
            //CREATE TABLE IF NOT EXISTS Players (
            //    PlayerID INTEGER PRIMARY KEY AUTOINCREMENT, 
            //    PlayerName TEXT,
            //    GuildName TEXT,
            //    Gender TEXT"; 

            //cmd = new SqliteCommand(sql, conn);
            //cmd.ExecuteNonQuery();

            //Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            //string playerName = s.HunterName;
            //string guildName = s.GuildName;
            //string gender = s.GenderExport;
            //string gearName = s.GearDescriptionExport;
            //string weaponClass = model.GetWeaponClass();
            //int weaponTypeID = model.WeaponType();
            //int weaponID = model.MeleeWeaponID();//ranged and melee are the same afaik
            //string weaponSlot1 = model.GetDecoName(model.WeaponDeco1ID(), 1);// no sigils in database ig
            //string weaponSlot2 = model.GetDecoName(model.WeaponDeco2ID(), 2);
            //string weaponSlot3 = model.GetDecoName(model.WeaponDeco3ID(), 3);
            //int headID = model.ArmorHeadID();
            //int headSlot1 = model.ArmorHeadDeco1ID();
            //int headSlot2 = model.ArmorHeadDeco2ID();
            //int headSlot3 = model.ArmorHeadDeco3ID();
            //int chestID = model.ArmorChestID();
            //int chestSlot1 = model.ArmorChestDeco1ID();
            //int chestSlot2 = model.ArmorChestDeco2ID();
            //int chestSlot3 = model.ArmorChestDeco3ID();
            //int armsID = model.ArmorArmsID();
            //int armsSlot1 = model.ArmorArmsDeco1ID();
            //int armsSlot2 = model.ArmorArmsDeco2ID();
            //int armsSlot3 = model.ArmorArmsDeco3ID();
            //int waistID = model.ArmorWaistID();
            //int waistSlot1 = model.ArmorWaistDeco1ID();
            //int waistSlot2 = model.ArmorWaistDeco2ID();
            //int waistSlot3 = model.ArmorWaistDeco3ID();
            //int legsID = model.ArmorLegsID();
            //int legsSlot1 = model.ArmorLegsDeco1ID();
            //int legsSlot2 = model.ArmorLegsDeco2ID();
            //int legsSlot3 = model.ArmorLegsDeco3ID();
            //int cuffSlot1 = model.Cuff1ID();
            //int cuffSlot2 = model.Cuff2ID();

            //// Create the WeaponTypes table
            //sql = @"CREATE TABLE IF NOT EXISTS WeaponTypes (
            //WeaponTypeID INTEGER PRIMARY KEY, 
            //WeaponTypeName TEXT)";
            //cmd = new SqliteCommand(sql, conn);
            //cmd.ExecuteNonQuery();

            //// Create the Item table
            //sql = @"CREATE TABLE IF NOT EXISTS Item (
            //ItemID INTEGER PRIMARY KEY, 
            //ItemName TEXT)";
            //cmd = new SqliteCommand(sql, conn);
            //cmd.ExecuteNonQuery();

            //// Loop through the entries in the dictionary
            //foreach (KeyValuePair<int, string> entry in Items.ItemIDs)
            //{
            //    int itemID = entry.Key;
            //    string itemName = entry.Value;

            //    sql = "INSERT OR IGNORE INTO Item (ItemID, ItemName) VALUES (@ItemID, @ItemName)";
            //    cmd = new SqliteCommand(sql, conn);
            //    cmd.Parameters.AddWithValue("@ItemID", itemID);
            //    cmd.Parameters.AddWithValue("@ItemName", itemName);
            //    cmd.ExecuteNonQuery();
            //}
            
            //// Create the PlayerGear table
            //sql = @"CREATE TABLE IF NOT EXISTS PlayerGear (
            //RunID INTEGER PRIMARY KEY AUTOINCREMENT, 
            //FOREIGN KEY(RunID) REFERENCES Quests(RunID), 
            //GearName TEXT,
            //StyleID INTEGER,
            //WeaponClass TEXT,
            //WeaponTypeID INTEGER,
            //WeaponID INTEGER,
            //WeaponSlot1 TEXT,
            //WeaponSlot2 TEXT,
            //WeaponSlot3 TEXT,
            //HeadID INTEGER, 
            //HeadSlot1ID INTEGER,
            //HeadSlot2ID INTEGER,
            //HeadSlot3ID INTEGER,
            //ChestID INTEGER, 
            //ChestSlot1ID INTEGER,
            //ChestSlot2ID INTEGER,
            //ChestSlot3ID INTEGER,
            //ArmsID INTEGER, 
            //ArmsSlot1ID INTEGER,
            //ArmsSlot2ID INTEGER,
            //ArmsSlot3ID INTEGER,
            //WaistID INTEGER, 
            //WaistSlot1ID INTEGER,
            //WaistSlot2ID INTEGER,
            //WaistSlot3ID INTEGER,
            //LegsID INTEGER,
            //LegsSlot1ID INTEGER,
            //LegsSlot2ID INTEGER,
            //LegsSlot3ID INTEGER,
            //Cuff1ID INTEGER,
            //Cuff2ID INTEGER,
            //FOREIGN KEY(WeaponTypeID) REFERENCES WeaponTypes(WeaponTypeID),
            //FOREIGN KEY(WeaponID) REFERENCES Gear(ItemID),
            //FOREIGN KEY(HeadID) REFERENCES Gear(ItemID),
            //FOREIGN KEY(ChestID) REFERENCES Gear(ItemID),
            //FOREIGN KEY(ArmsID) REFERENCES Gear(ItemID),
            //FOREIGN KEY(WaistID) REFERENCES Gear(ItemID),
            //FOREIGN KEY(LegsID) REFERENCES Gear(ItemID)
            //)";
            //cmd = new SqliteCommand(sql, conn);
            //cmd.ExecuteNonQuery();

            //// Create the PlayerInventory table
            //sql = "CREATE TABLE IF NOT EXISTS PlayerInventory (RunID INTEGER PRIMARY KEY AUTOINCREMENT, FOREIGN KEY(RunID) REFERENCES Quests(RunID), ItemID INTEGER, ItemQuantity INTEGER)";
            //cmd = new SqliteCommand(sql, conn);
            //cmd.ExecuteNonQuery();

            //sql = @"
            //CREATE TABLE IF NOT EXISTS Gear (
            //  PieceID INTEGER PRIMARY KEY,
            //  PieceName TEXT,
            //  PieceType TEXT
            //)";

            //// Get a list of dictionaries containing the armor piece IDs and names
            //List<Dictionary<int, string>> armorDictionaries = GetArmorDictionariesList();

            //// Create a list of the armor piece types
            //List<string> armorTypes = new List<string>
            //{
            //    "Head",
            //    "Chest",
            //    "Arms",
            //    "Waist",
            //    "Legs"
            //};

            //// Iterate over the dictionaries and piece types
            //for (int i = 0; i < armorDictionaries.Count; i++)
            //{
            //    Dictionary<int, string> dictionary = armorDictionaries[i];
            //    string pieceType = armorTypes[i];

            //    // Loop through the entries in the dictionary
            //    foreach (KeyValuePair<int, string> entry in dictionary)
            //    {
            //        int pieceID = entry.Key;
            //        string pieceName = entry.Value;

            //        sql = "INSERT OR IGNORE INTO Gear (PieceID, PieceName, PieceType) VALUES (@PieceID, @PieceName, @PieceType)";
            //        cmd = new SqliteCommand(sql, conn);
            //        cmd.Parameters.AddWithValue("@PieceID", pieceID);
            //        cmd.Parameters.AddWithValue("@PieceName", pieceName);
            //        cmd.Parameters.AddWithValue("@PieceType", pieceType);
            //        cmd.ExecuteNonQuery();
            //    }
            //}
        }

        void InsertQuestIntoDatabase(SQLiteConnection conn)
        {
            // Insert a new quest into the Quests table
            string sql = @"INSERT INTO Quests (
            QuestID, 
            QuestName, 
            EndTime, 
            ObjectiveQuantity, 
            ObjectiveName, 
            Gear, 
            Weapon, 
            Date) 
            VALUES (
            @questID, 
            @questName, 
            @endTime, 
            @objectiveType, 
            @objectiveQuantity, 
            @starGrade, 
            @rankName, 
            @objectiveName, 
            @date)";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            
            cmd.Parameters.AddWithValue("@questName", model.GetQuestNameFromID(model.QuestID()));
            cmd.Parameters.AddWithValue("@endTime", "");
            cmd.Parameters.AddWithValue("@objectiveType", "");
            cmd.Parameters.AddWithValue("@objectiveQuantity", "");
            cmd.Parameters.AddWithValue("@starGrade", "");
            cmd.Parameters.AddWithValue("@rankName", "");
            cmd.Parameters.AddWithValue("@objectiveName", model.GetObjectiveNameFromID(model.Objective1ID()));
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.ExecuteNonQuery();

            // Check if the player has already been inserted into the Players table
            sql = "SELECT PlayerID FROM Players WHERE PlayerName = @playerName";
            cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@playerName", "Doriel");
            object result = cmd.ExecuteScalar();

            int playerID;

            // If the player has not been inserted, insert the player into the Players table
            if (result == null)
            {
                sql = "INSERT INTO Players (PlayerName) VALUES (@playerName)";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@playerName", "Doriel");
                cmd.ExecuteNonQuery();

                // Get the PlayerID of the inserted player
                sql = "SELECT PlayerID FROM Players WHERE PlayerName = @playerName";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@playerName", "Doriel");
                playerID = (int)cmd.ExecuteScalar();

            }
            else
            {
                // Get the PlayerID of the player that was retrieved from the database
                playerID = (int)result;
            }

            // Check if the helmet, chestplate, and weapon have already been inserted into the Gear table
            sql = "SELECT GearID FROM Gear WHERE GearType = @gearType AND Rarity = @rarity AND OtherInfo = @otherInfo";
            cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@gearType", "Helmet");
            cmd.Parameters.AddWithValue("@rarity", 3);
            cmd.Parameters.AddWithValue("@otherInfo", "Alisys ZP Head");
            result = cmd.ExecuteScalar();

            // If the gear has not been inserted, insert it into the Gear table
            if (result == null)
            {
                sql = "INSERT INTO Gear (GearType, Rarity, OtherInfo) VALUES (@gearType, @rarity, @otherInfo)";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@gearType", "Helmet");
                cmd.Parameters.AddWithValue("@rarity", 3);
                cmd.Parameters.AddWithValue("@otherInfo", "Alisys ZP Head");
                cmd.ExecuteNonQuery();

                // Retrieve the ID of the newly inserted gear
                //int gearID = (int)cmd.LastInsertedId;

                // Insert data into the PlayerGear table
                sql = "INSERT INTO PlayerGear (PlayerID, RunID, GearID) VALUES (@playerID, @runID, @gearID)";
                cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@playerID", playerID);
                cmd.Parameters.AddWithValue("@runID", "");
                cmd.Parameters.AddWithValue("@gearID", "");
                cmd.ExecuteNonQuery();

                // Close the database connection
                conn.Close();

                return;
            }

            // Close the database connection
            conn.Close();
        }

        void RetreiveQuestsFromDatabase()
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + dbFilePath + "");
            SQLitePCL.Batteries.Init();
            conn.Open();

            // Create the Quests table
            string sql = "CREATE TABLE IF NOT EXISTS Quests (RunID INTEGER PRIMARY KEY AUTOINCREMENT, QuestID INTEGER, QuestName TEXT, EndTime DATETIME, ObjectiveType TEXT, ObjectiveQuantity INTEGER, StarGrade INTEGER, RankName TEXT, ObjectiveName TEXT, Date DATETIME)";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            // Retrieve all quests from the Quests table
            sql = "SELECT * FROM Quests";
            cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // Print the quest data to the console
                Console.WriteLine("Quest: " + reader["QuestName"].ToString());
                Console.WriteLine("End Time: " + reader["EndTime"].ToString());
                Console.WriteLine("Monster: " + reader["Monster"].ToString());
                Console.WriteLine("Gear: " + reader["Gear"].ToString());
                Console.WriteLine();
            }

            // Close the database connection
            conn.Close();
        }

        public static string dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\MHFZ_Overlay.sqlite");

        void SetupLocalDatabase()
        {

            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }

            using (var conn = new SQLiteConnection("Data Source=" + dbFilePath + ""))
            {
                conn.Open();

                // Do something with the connection
                CreateDatabaseTables(conn);
            }
            //            SQLitePCL.Batteries.Init();
        }

        #endregion

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
