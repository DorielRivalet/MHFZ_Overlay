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
using SharpCompress.Common;
using MHFZ_Overlay.controls;
using System.Data;
using System.Data.Common;

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

        private string GetQuestTimeCompletion()
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

        public static List<Dictionary<int, string>> GetGearDictionariesList()
        {
            return new List<Dictionary<int, string>>
            {
                (Dictionary<int, string>)ArmorHeads.ArmorHeadIDs,
                (Dictionary<int, string>)ArmorChests.ArmorChestIDs,
                (Dictionary<int, string>)ArmorArms.ArmorArmIDs,
                (Dictionary<int, string>)ArmorWaists.ArmorWaistIDs,
                (Dictionary<int, string>)ArmorLegs.ArmorLegIDs,
                (Dictionary<int, string>)MeleeWeapons.MeleeWeaponIDs,
                (Dictionary<int, string>)RangedWeapons.RangedWeaponIDs
            };
        }

        #region database

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

        private void InsertIntoTable(IReadOnlyDictionary<int, string> dictionary, string tableName, string idColumn, string valueColumn, SQLiteConnection conn)
        {
            // Start a transaction
            using (var transaction = conn.BeginTransaction())
            {
                // Create a command that will be used to insert multiple rows in a batch
                using (var cmd = new SQLiteCommand(conn))
                {
                    // Create a parameter for the value to be inserted
                    var valueParam = cmd.CreateParameter();
                    valueParam.ParameterName = "@value";

                    // Create a parameter for the ID to be inserted
                    var idParam = cmd.CreateParameter();
                    idParam.ParameterName = "@id";

                    // Set the command text to insert a single row
                    cmd.CommandText = $"INSERT INTO {tableName} ({idColumn}, {valueColumn}) VALUES (@id, @value)";
                    cmd.Parameters.Add(idParam);
                    cmd.Parameters.Add(valueParam);

                    // Insert each row in the dictionary
                    foreach (var pair in dictionary)
                    {
                        // Set the values of the parameters
                        idParam.Value = pair.Key;
                        valueParam.Value = pair.Value;

                        // Execute the command to insert the row
                        cmd.ExecuteNonQuery();
                    }

                    // Commit the transaction
                    transaction.Commit();
                }
            }
        }


        private void CreateDatabaseTables(SQLiteConnection conn)
        {
            // Create table to store program usage time
            string sql = @"CREATE TABLE IF NOT EXISTS Session (
            SessionID INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime DATETIME NOT NULL,
            EndTime DATETIME NOT NULL,
            SessionDuration INTEGER NOT NULL)";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            //Create the Quests table
            sql = @"CREATE TABLE IF NOT EXISTS Quests 
            (RunID INTEGER PRIMARY KEY AUTOINCREMENT, 
            QuestID INTEGER NOT NULL, 
            AreaID INTEGER NOT NULL, 
            FinalTimeValue INTEGER NOT NULL,
            FinalTimeDisplay TEXT NOT NULL, 
            ObjectiveImage TEXT NOT NULL,
            ObjectiveType TEXT NOT NULL, 
            ObjectiveQuantity INTEGER NOT NULL, 
            StarGrade INTEGER NOT NULL, 
            RankName TEXT NOT NULL, 
            ObjectiveName TEXT NOT NULL, 
            Date DATETIME NOT NULL,
            FOREIGN KEY(QuestID) REFERENCES QuestName(QuestNameID),
            FOREIGN KEY(AreaID) REFERENCES Area(AreaID),
            FOREIGN KEY(ObjectiveType) REFERENCES ObjectiveType(ObjectiveTypeID),
            FOREIGN KEY(RankName) REFERENCES RankName(RankNameID)
            )";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            //int timeLeft = 514; // Example value of the TimeLeft variable

            //// Calculate the finalTimeDisplay value in the "mm:ss.mm" format
            //TimeSpan finalTimeDisplay = TimeSpan.FromSeconds(timeLeft / 30.0);

            //// Insert the TimeLeft value into the FinalTimeValue field and the finalTimeDisplay value into the FinalTimeString field of the Quests table
            //string sql = "INSERT INTO Quests (QuestID, FinalTimeValue, FinalTimeString) VALUES (@QuestID, @FinalTimeValue, @FinalTimeString)";
            //using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            //{
            //    cmd.Parameters.AddWithValue("@QuestID", 1);
            //    cmd.Parameters.AddWithValue("@FinalTimeValue", timeLeft);
            //    cmd.Parameters.AddWithValue("@FinalTimeString", finalTimeDisplay.ToString("mm\\:ss\\.ff"));
            //    cmd.ExecuteNonQuery();
            //}

            //string sql = "SELECT FinalTimeValue, FinalTimeString FROM Quests WHERE QuestID = @QuestID ORDER BY FinalTimeValue ASC";
            //using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            //{
            //    cmd.Parameters.AddWithValue("@QuestID", 1);
            //    using (SQLiteDataReader reader = cmd.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            int finalTimeValue = reader.GetInt32(0);
            //            string finalTimeString = reader.GetString(1);
            //            // Do something with the finalTimeValue and finalTimeString values
            //        }
            //    }
            //}

            //Create the RankNames table
            sql = @"CREATE TABLE IF NOT EXISTS RankName
            (RankNameID INTEGER PRIMARY KEY, 
            RankNameName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(Dictionary.RanksBandsList.RankBandsID, "RankName", "RankNameID","RankNameName",conn);

            //Create the ObjectiveTypes table
            sql = @"CREATE TABLE IF NOT EXISTS ObjectiveType
            (ObjectiveTypeID INTEGER PRIMARY KEY, 
            ObjectiveTypeName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(Dictionary.ObjectiveTypeList.ObjectiveTypeID, "ObjectiveType", "ObjectiveTypeID", "ObjectiveTypeName", conn);

            //Create the QuestNames table
            sql = @"CREATE TABLE IF NOT EXISTS QuestName
            (QuestNameID INTEGER PRIMARY KEY, 
            QuestNameName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(Quests.QuestIDs, "QuestName", "QuestNameID", "QuestNameName", conn);

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

            // Create the Players table
            //do an UPDATE when inserting quests. since its just local player
            sql = @"
            CREATE TABLE IF NOT EXISTS Players (
            PlayerID INTEGER PRIMARY KEY AUTOINCREMENT, 
            PlayerName TEXT NOT NULL,
            GuildName TEXT NOT NULL,
            Gender TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

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

            // Create the WeaponTypes table
            sql = @"CREATE TABLE IF NOT EXISTS WeaponType (
            WeaponTypeID INTEGER PRIMARY KEY, 
            WeaponTypeName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(WeaponList.WeaponID, "WeaponType", "WeaponTypeID", "WeaponTypeName", conn);

            // Create the Item table
            sql = @"CREATE TABLE IF NOT EXISTS Item (
            ItemID INTEGER PRIMARY KEY, 
            ItemName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(Items.ItemIDs, "Item", "ItemID", "ItemName", conn);

            // Create the Area table
            sql = @"CREATE TABLE IF NOT EXISTS Area (
            AreaID INTEGER PRIMARY KEY,
            AreaName TEXT NOT NULL,
            AreaIcon TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            // Start a transaction
            using (DbTransaction transaction = conn.BeginTransaction())
            {
                // Prepare the SQL statement
                sql = "INSERT OR IGNORE INTO Area (AreaID, AreaIcon, AreaName) VALUES (@AreaID, @AreaIcon, @AreaName)";
                using (cmd = new SQLiteCommand(sql, conn))
                {
                    // Add the parameter placeholders
                    cmd.Parameters.Add("@AreaID", DbType.Int32);
                    cmd.Parameters.Add("@AreaIcon", DbType.String);
                    cmd.Parameters.Add("@AreaName", DbType.String);

                    try
                    {
                        // Iterate through the list of areas
                        foreach (KeyValuePair<List<int>, string> kvp in AreaIconDictionary.AreaIconID)
                        {
                            List<int> areaIDs = kvp.Key;

                            foreach (int areaID in areaIDs)
                            {
                                string areaIcon = kvp.Value;
                                string areaName = model.GetAreaName(areaID);

                                // Set the parameter values
                                cmd.Parameters["@AreaID"].Value = areaID;
                                cmd.Parameters["@AreaIcon"].Value = areaIcon;
                                cmd.Parameters["@AreaName"].Value = areaName;

                                // Execute the SQL statement
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        // Roll back the transaction
                        transaction.Rollback();
                    }
                }
            }

            // Create the PlayerGear table
            sql = @"CREATE TABLE IF NOT EXISTS PlayerGear (
            RunID INTEGER PRIMARY KEY AUTOINCREMENT, 
            GearName TEXT NOT NULL,
            StyleID INTEGER NOT NULL,
            WeaponIcon TEXT NOT NULL,
            WeaponClass TEXT NOT NULL,
            WeaponTypeID INTEGER NOT NULL,
            WeaponID INTEGER NOT NULL,
            WeaponSlot1 TEXT NOT NULL,
            WeaponSlot2 TEXT NOT NULL,
            WeaponSlot3 TEXT NOT NULL,
            HeadID INTEGER NOT NULL, 
            HeadSlot1ID INTEGER NOT NULL,
            HeadSlot2ID INTEGER NOT NULL,
            HeadSlot3ID INTEGER NOT NULL,
            ChestID INTEGER NOT NULL,
            ChestSlot1ID INTEGER NOT NULL,
            ChestSlot2ID INTEGER NOT NULL,
            ChestSlot3ID INTEGER NOT NULL,
            ArmsID INTEGER NOT NULL,
            ArmsSlot1ID INTEGER NOT NULL,
            ArmsSlot2ID INTEGER NOT NULL,
            ArmsSlot3ID INTEGER NOT NULL,
            WaistID INTEGER NOT NULL,
            WaistSlot1ID INTEGER NOT NULL,
            WaistSlot2ID INTEGER NOT NULL,
            WaistSlot3ID INTEGER NOT NULL,
            LegsID INTEGER NOT NULL,
            LegsSlot1ID INTEGER NOT NULL,
            LegsSlot2ID INTEGER NOT NULL,
            LegsSlot3ID INTEGER NOT NULL,
            Cuff1ID INTEGER NOT NULL,
            Cuff2ID INTEGER NOT NULL,
            ZenithSkillsID INTEGER NOT NULL,
            AutomaticSkillsID INTEGER NOT NULL,
            ActiveSkillsID INTEGER NOT NULL,
            CaravanSkillsID INTEGER NOT NULL,
            DivaSkillID INTEGER NOT NULL,
            GuildFoodID INTEGER NOT NULL,
            StyleRankSkillsID INTEGER NOT NULL,
            PlayerInventoryID INTEGER NOT NULL,
            AmmoPouchID INTEGER NOT NULL,
            PoogieItemID INTEGER NOT NULL,
            RoadDureSkillsID INTEGER NOT NULL,
            FOREIGN KEY(RunID) REFERENCES Quests(RunID),
            FOREIGN KEY(WeaponTypeID) REFERENCES WeaponType(WeaponTypeID),
            FOREIGN KEY(WeaponID) REFERENCES Gear(PieceID),
            FOREIGN KEY(HeadID) REFERENCES Gear(PieceID),
            FOREIGN KEY(ChestID) REFERENCES Gear(PieceID),
            FOREIGN KEY(ArmsID) REFERENCES Gear(PieceID),
            FOREIGN KEY(WaistID) REFERENCES Gear(PieceID),
            FOREIGN KEY(LegsID) REFERENCES Gear(PieceID),
            FOREIGN KEY(Cuff1ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Cuff2ID) REFERENCES Item(ItemID),
            FOREIGN KEY(ZenithSkillsID) REFERENCES ZenithSkills(ZenithSkillsID),
            FOREIGN KEY(AutomaticSkillsID) REFERENCES AutomaticSkills(AutomaticSkillsID),
            FOREIGN KEY(ActiveSkillsID) REFERENCES ActiveSkills(ActiveSkillsID),
            FOREIGN KEY(CaravanSkillsID) REFERENCES CaravanSkills(CaravanSkillsID),
            FOREIGN KEY(StyleRankSkillsID) REFERENCES StyleRankSkills(StyleRankSkillsID),
            FOREIGN KEY(PlayerInventoryID) REFERENCES PlayerInventory(PlayerInventoryID),
            FOREIGN KEY(AmmoPouchID) REFERENCES AmmoPouch(AmmoPouchID),
            FOREIGN KEY(RoadDureSkillsID) REFERENCES RoadDureSkills(RoadDureSkillsID)
            )";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = @"CREATE TABLE IF NOT EXISTS ZenithSkills(
            ZenithSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
            RunID INTEGER NOT NULL,
            ZenithSkill1ID INTEGER NOT NULL,
            ZenithSkill2ID INTEGER NOT NULL,
            ZenithSkill3ID INTEGER NOT NULL,
            ZenithSkill4ID INTEGER NOT NULL,
            ZenithSkill5ID INTEGER NOT NULL,
            ZenithSkill6ID INTEGER NOT NULL,
            ZenithSkill7ID INTEGER NOT NULL,
            FOREIGN KEY(RunID) REFERENCES Quests(RunID)
            FOREIGN KEY(ZenithSkill1ID) REFERENCES AllZenithSkills(ZenithSkillID),
            FOREIGN KEY(ZenithSkill2ID) REFERENCES AllZenithSkills(ZenithSkillID),
            FOREIGN KEY(ZenithSkill3ID) REFERENCES AllZenithSkills(ZenithSkillID),
            FOREIGN KEY(ZenithSkill4ID) REFERENCES AllZenithSkills(ZenithSkillID),
            FOREIGN KEY(ZenithSkill5ID) REFERENCES AllZenithSkills(ZenithSkillID),
            FOREIGN KEY(ZenithSkill6ID) REFERENCES AllZenithSkills(ZenithSkillID),
            FOREIGN KEY(ZenithSkill7ID) REFERENCES AllZenithSkills(ZenithSkillID))";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = @"CREATE TABLE IF NOT EXISTS AllZenithSkills(
            ZenithSkillID INTEGER PRIMARY KEY,
            ZenithSkillName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(Dictionary.ZenithSkillList.ZenithSkillID, "AllZenithSkills", "ZenithSkillID", "ZenithSkillName", conn);

            sql = @"CREATE TABLE IF NOT EXISTS AutomaticSkills(
            AutomaticSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
            RunID INTEGER NOT NULL,
            AutomaticSkill1ID INTEGER NOT NULL,
            AutomaticSkill2ID INTEGER NOT NULL,
            AutomaticSkill3ID INTEGER NOT NULL,
            AutomaticSkill4ID INTEGER NOT NULL,
            AutomaticSkill5ID INTEGER NOT NULL,
            AutomaticSkill6ID INTEGER NOT NULL,
            FOREIGN KEY(RunID) REFERENCES Quests(RunID),
            FOREIGN KEY(AutomaticSkill1ID) REFERENCES AllArmorSkills(ArmorSkillID),
            FOREIGN KEY(AutomaticSkill2ID) REFERENCES AllArmorSkills(ArmorSkillID),
            FOREIGN KEY(AutomaticSkill3ID) REFERENCES AllArmorSkills(ArmorSkillID),
            FOREIGN KEY(AutomaticSkill4ID) REFERENCES AllArmorSkills(ArmorSkillID),
            FOREIGN KEY(AutomaticSkill5ID) REFERENCES AllArmorSkills(ArmorSkillID),
            FOREIGN KEY(AutomaticSkill6ID) REFERENCES AllArmorSkills(ArmorSkillID))";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = @"CREATE TABLE IF NOT EXISTS AllArmorSkills(
            ArmorSkillID INTEGER PRIMARY KEY,
            ArmorSkillName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(Dictionary.ArmorSkillList.ArmorSkillID, "AllArmorSkills", "ArmorSkillID", "ArmorSkillName", conn);

            sql = @"CREATE TABLE IF NOT EXISTS ActiveSkills(
                ActiveSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
                RunID INTEGER NOT NULL,
                ActiveSkill1ID INTEGER NOT NULL,
                ActiveSkill2ID INTEGER NOT NULL,
                ActiveSkill3ID INTEGER NOT NULL,
                ActiveSkill4ID INTEGER NOT NULL,
                ActiveSkill5ID INTEGER NOT NULL,
                ActiveSkill6ID INTEGER NOT NULL,
                ActiveSkill7ID INTEGER NOT NULL,
                ActiveSkill8ID INTEGER NOT NULL,
                ActiveSkill9ID INTEGER NOT NULL,
                ActiveSkill10ID INTEGER NOT NULL,
                ActiveSkill11ID INTEGER NOT NULL,
                ActiveSkill12ID INTEGER NOT NULL,
                ActiveSkill13ID INTEGER NOT NULL,
                ActiveSkill14ID INTEGER NOT NULL,
                ActiveSkill15ID INTEGER NOT NULL,
                ActiveSkill16ID INTEGER NOT NULL,
                ActiveSkill17ID INTEGER NOT NULL,
                ActiveSkill18ID INTEGER NOT NULL,
                ActiveSkill19ID INTEGER NOT NULL,
                FOREIGN KEY(RunID) REFERENCES Quests(RunID)
                FOREIGN KEY(ActiveSkill1ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill2ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill3ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill4ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill5ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill6ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill7ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill8ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill9ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill10ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill11ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill12ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill13ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill14ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill15ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill16ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill17ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill18ID) REFERENCES AllArmorSkills(ArmorSkillID),
                FOREIGN KEY(ActiveSkill19ID) REFERENCES AllArmorSkills(ArmorSkillID)
                )";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = @"CREATE TABLE IF NOT EXISTS CaravanSkills(
                CaravanSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
                RunID INTEGER NOT NULL,
                CaravanSkill1ID INTEGER NOT NULL,
                CaravanSkill2ID INTEGER NOT NULL,
                CaravanSkill3ID INTEGER NOT NULL,
                FOREIGN KEY(RunID) REFERENCES Quests(RunID)
                FOREIGN KEY(CaravanSkill1ID) REFERENCES AllCaravanSkills(CaravanSkillID),
                FOREIGN KEY(CaravanSkill2ID) REFERENCES AllCaravanSkills(CaravanSkillID),
                FOREIGN KEY(CaravanSkill3ID) REFERENCES AllCaravanSkills(CaravanSkillID)
                )";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = @"CREATE TABLE IF NOT EXISTS AllCaravanSkills(
            CaravanSkillID INTEGER PRIMARY KEY,
            CaravanSkillName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(Dictionary.CaravanSkillList.CaravanSkillID, "AllCaravanSkills", "CaravanSkillID", "CaravanSkillName", conn);

            sql = @"CREATE TABLE IF NOT EXISTS StyleRankSkills(
                StyleRankSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
                RunID INTEGER NOT NULL,
                StyleRankSkill1ID INTEGER NOT NULL,
                StyleRankSkill2ID INTEGER NOT NULL,
                FOREIGN KEY(RunID) REFERENCES Quests(RunID),
                FOREIGN KEY(StyleRankSkill1ID) REFERENCES AllStyleRankSkills(StyleRankSkillID),
                FOREIGN KEY(StyleRankSkill2ID) REFERENCES AllStyleRankSkills(StyleRankSkillID)
                )";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = @"CREATE TABLE IF NOT EXISTS AllStyleRankSkills(
            StyleRankSkillID INTEGER PRIMARY KEY,
            StyleRankSkillName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(Dictionary.StyleRankSkillList.StyleRankSkillID, "AllStyleRankSkills", "StyleRankSkillID", "StyleRankSkillName", conn);

            // Create the PlayerInventory table
            sql = @"CREATE TABLE IF NOT EXISTS PlayerInventory (
            PlayerInventoryID INTEGER PRIMARY KEY AUTOINCREMENT,
            RunID INTEGER NOT NULL,
            Item1ID INTEGER NOT NULL, 
            Item1Quantity INTEGER NOT NULL,
            Item2ID INTEGER NOT NULL, 
            Item2Quantity INTEGER NOT NULL,
            Item3ID INTEGER NOT NULL, 
            Item3Quantity INTEGER NOT NULL,
            Item4ID INTEGER NOT NULL, 
            Item4Quantity INTEGER NOT NULL,
            Item5ID INTEGER NOT NULL, 
            Item5Quantity INTEGER NOT NULL,
            Item6ID INTEGER NOT NULL, 
            Item6Quantity INTEGER NOT NULL,
            Item7ID INTEGER NOT NULL, 
            Item7Quantity INTEGER NOT NULL,
            Item8ID INTEGER NOT NULL, 
            Item8Quantity INTEGER NOT NULL,
            Item9ID INTEGER NOT NULL, 
            Item9Quantity INTEGER NOT NULL,
            Item10ID INTEGER NOT NULL, 
            Item10Quantity INTEGER NOT NULL,
            Item11ID INTEGER NOT NULL, 
            Item11Quantity INTEGER NOT NULL,
            Item12ID INTEGER NOT NULL, 
            Item12Quantity INTEGER NOT NULL,
            Item13ID INTEGER NOT NULL, 
            Item13Quantity INTEGER NOT NULL,
            Item14ID INTEGER NOT NULL, 
            Item14Quantity INTEGER NOT NULL,
            Item15ID INTEGER NOT NULL, 
            Item15Quantity INTEGER NOT NULL,
            Item16ID INTEGER NOT NULL, 
            Item16Quantity INTEGER NOT NULL,
            Item17ID INTEGER NOT NULL, 
            Item17Quantity INTEGER NOT NULL,
            Item18ID INTEGER NOT NULL, 
            Item18Quantity INTEGER NOT NULL,
            Item19ID INTEGER NOT NULL, 
            Item19Quantity INTEGER NOT NULL,
            Item20ID INTEGER NOT NULL, 
            Item20Quantity INTEGER NOT NULL,
            FOREIGN KEY(RunID) REFERENCES Quests(RunID),
            FOREIGN KEY(Item1ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item2ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item3ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item4ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item5ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item6ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item7ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item8ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item9ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item10ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item11ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item12ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item13ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item14ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item15ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item16ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item17ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item18ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item19ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item20ID) REFERENCES Item(ItemID)
            )";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = @"CREATE TABLE IF NOT EXISTS AmmoPouch (
            AmmoPouchID INTEGER PRIMARY KEY AUTOINCREMENT,
            RunID INTEGER NOT NULL,
            Item1ID INTEGER NOT NULL, 
            Item1Quantity INTEGER NOT NULL,
            Item2ID INTEGER NOT NULL, 
            Item2Quantity INTEGER NOT NULL,
            Item3ID INTEGER NOT NULL, 
            Item3Quantity INTEGER NOT NULL,
            Item4ID INTEGER NOT NULL, 
            Item4Quantity INTEGER NOT NULL,
            Item5ID INTEGER NOT NULL, 
            Item5Quantity INTEGER NOT NULL,
            Item6ID INTEGER NOT NULL, 
            Item6Quantity INTEGER NOT NULL,
            Item7ID INTEGER NOT NULL, 
            Item7Quantity INTEGER NOT NULL,
            Item8ID INTEGER NOT NULL, 
            Item8Quantity INTEGER NOT NULL,
            Item9ID INTEGER NOT NULL, 
            Item9Quantity INTEGER NOT NULL,
            Item10ID INTEGER NOT NULL, 
            Item10Quantity INTEGER NOT NULL,
            FOREIGN KEY(RunID) REFERENCES Quests(RunID),
            FOREIGN KEY(Item1ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item2ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item3ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item4ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item5ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item6ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item7ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item8ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item9ID) REFERENCES Item(ItemID),
            FOREIGN KEY(Item10ID) REFERENCES Item(ItemID)
            )";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = @"CREATE TABLE IF NOT EXISTS RoadDureSkills (
            RoadDureSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
            RunID INTEGER NOT NULL,
            RoadDureSkill1ID INTEGER NOT NULL, 
            RoadDureSkill1Level INTEGER NOT NULL,
            RoadDureSkill2ID INTEGER NOT NULL, 
            RoadDureSkill2Level INTEGER NOT NULL,
            RoadDureSkill3ID INTEGER NOT NULL, 
            RoadDureSkill3Level INTEGER NOT NULL,
            RoadDureSkill4ID INTEGER NOT NULL, 
            RoadDureSkill4Level INTEGER NOT NULL,
            RoadDureSkill5ID INTEGER NOT NULL, 
            RoadDureSkill5Level INTEGER NOT NULL,
            RoadDureSkill6ID INTEGER NOT NULL, 
            RoadDureSkill6Level INTEGER NOT NULL,
            RoadDureSkill7ID INTEGER NOT NULL, 
            RoadDureSkill7Level INTEGER NOT NULL,
            RoadDureSkill8ID INTEGER NOT NULL, 
            RoadDureSkill8Level INTEGER NOT NULL,
            RoadDureSkill9ID INTEGER NOT NULL, 
            RoadDureSkill9Level INTEGER NOT NULL,
            RoadDureSkill10ID INTEGER NOT NULL, 
            RoadDureSkill10Level INTEGER NOT NULL,
            RoadDureSkill11ID INTEGER NOT NULL, 
            RoadDureSkill11Level INTEGER NOT NULL,
            RoadDureSkill12ID INTEGER NOT NULL, 
            RoadDureSkill12Level INTEGER NOT NULL,
            RoadDureSkill13ID INTEGER NOT NULL, 
            RoadDureSkill13Level INTEGER NOT NULL,
            RoadDureSkill14ID INTEGER NOT NULL, 
            RoadDureSkill14Level INTEGER NOT NULL,
            RoadDureSkill15ID INTEGER NOT NULL, 
            RoadDureSkill15Level INTEGER NOT NULL,
            RoadDureSkill16ID INTEGER NOT NULL, 
            RoadDureSkill16Level INTEGER NOT NULL,
            FOREIGN KEY(RunID) REFERENCES Quests(RunID)
            FOREIGN KEY(RoadDureSkill1ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill2ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill3ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill4ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill5ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill6ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill7ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill8ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill9ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill10ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill11ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill12ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill13ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill14ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill15ID) REFERENCES AllRoadDureSkills(RoadDureSkillID),
            FOREIGN KEY(RoadDureSkill16ID) REFERENCES AllRoadDureSkills(RoadDureSkillID)
            )";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = @"CREATE TABLE IF NOT EXISTS AllRoadDureSkills(
            RoadDureSkillID INTEGER PRIMARY KEY,
            RoadDureSkillName TEXT NOT NULL)";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            InsertIntoTable(Dictionary.RoadDureSkills.RoadDureSkillIDs, "AllRoadDureSkills", "RoadDureSkillID", "RoadDureSkillName", conn);

            sql = @"
            CREATE TABLE IF NOT EXISTS Gear (
              GearPieceID INTEGER PRIMARY KEY AUTOINCREMENT,
              PieceID INTEGER NOT NULL,
              PieceName TEXT NOT NULL,
              PieceType TEXT NOT NULL
            )";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            // Start a transaction
            using (var transaction = conn.BeginTransaction())
            {
                // Prepare the SQL statement
                sql = "INSERT OR IGNORE INTO Gear (PieceID, PieceName, PieceType) VALUES (@PieceID, @PieceName, @PieceType)";
                using (cmd = new SQLiteCommand(sql, conn))
                {
                    // Add the parameter placeholders
                    cmd.Parameters.Add("@PieceID", DbType.Int32);
                    cmd.Parameters.Add("@PieceName", DbType.String);
                    cmd.Parameters.Add("@PieceType", DbType.String);

                    // Get a list of dictionaries containing the armor piece IDs and names
                    List<Dictionary<int, string>> gearDictionaries = GetGearDictionariesList();

                    // Create a list of the gear piece types
                    List<string> gearTypes = new List<string>
                    {
                        "Head",
                        "Chest",
                        "Arms",
                        "Waist",
                        "Legs",
                        "Melee",
                        "Ranged",
                    };

                    // Iterate over the dictionaries and piece types
                    for (int i = 0; i < gearDictionaries.Count; i++)
                    {
                        Dictionary<int, string> dictionary = gearDictionaries[i];
                        string pieceType = gearTypes[i];

                        // Loop through the entries in the dictionary
                        foreach (KeyValuePair<int, string> entry in dictionary)
                        {
                            int pieceID = entry.Key;
                            string pieceName = entry.Value;

                            // Set the parameter values
                            cmd.Parameters["@PieceID"].Value = pieceID;
                            cmd.Parameters["@PieceName"].Value = pieceName;
                            cmd.Parameters["@PieceType"].Value = pieceType;

                            // Execute the statement
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // Commit the transaction
                transaction.Commit();
            }
        }

        //i would first insert into the quest table,
        //then the tables referencing
        //playergear, then the playergear table
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
