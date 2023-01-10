using Dictionary;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Globalization;
using System.Windows.Documents;
using System.Windows.Markup;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Transactions;
using System.Collections;
using Octokit;
using System.Windows.Controls;
using System.Diagnostics;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

// TODO: PascalCase for functions, camelCase for private fields, ALL_CAPS for constants
namespace MHFZ_Overlay
{
    // Singleton
    internal class DatabaseManager
    {
        private readonly string _connectionString;

        public readonly string dataSource = "Data Source=" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\MHFZ_Overlay.sqlite");

        private static DatabaseManager instance;

        private DatabaseManager()
        {
            // Private constructor to prevent external instantiation
            _connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\MHFZ_Overlay.sqlite");
        }

        public static DatabaseManager GetInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseManager();
            }

            return instance;
        }

        #region program time

        // Calculate the total time spent using the program
        public TimeSpan CalculateTotalTimeSpent()
        {
            TimeSpan totalTimeSpent = TimeSpan.Zero;

            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT SUM(SessionDuration) FROM Session";
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalTimeSpent = TimeSpan.FromSeconds(Convert.ToInt32(result));
                    }
                }
            }

            return totalTimeSpent;
        }

        #endregion

        #region database

        private bool isDatabaseSetup = false;

        public bool SetupLocalDatabase(DataLoader dataLoader)
        {
            if (!isDatabaseSetup)
            {
                isDatabaseSetup = true;
                if (!File.Exists(_connectionString))
                {
                    SQLiteConnection.CreateFile(_connectionString);
                }

                try
                {
                    using (var conn = new SQLiteConnection(dataSource))
                    {
                        conn.Open();

                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            // file is a valid database file
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(String.Format("Invalid database file. Delete both MHFZ_Overlay.sqlite and reference_schema.json if present, and rerun the program.\n\n{0}", ex),"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    Environment.Exit(0);
                }

                using (var conn = new SQLiteConnection(dataSource))
                {
                    conn.Open();

                    CreateDatabaseTables(conn, dataLoader);
                    CreateDatabaseIndexes(conn);
                    CreateDatabaseTriggers(conn);
                }

                using (var conn = new SQLiteConnection(dataSource))
                {
                    conn.Open();

                    // Check if the reference schema file exists
                    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\reference_schema.json")))
                    {
                        CreateReferenceSchemaJSONFromLocalDatabaseFile(conn);
                    }
                    else
                    {
                        // Load the reference schema file
                        var referenceSchemaJson = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\reference_schema.json"));
                        var referenceSchema = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(referenceSchemaJson);

                        // Create a dictionary to store the current schema
                        var currentSchema = CreateReferenceSchemaJSONFromLocalDatabaseFile(conn, false);
                        CompareDatabaseSchemas(referenceSchema, currentSchema);
                    }
                }

                if (schemaChanged)
                {
                    MessageBox.Show("Your quest runs will not be accepted into the central database unless you update the schemas.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            return schemaChanged;
        }

        // Calculate the finalTimeDisplay value in the "mm:ss.mm" format
        //string finalTimeDisplay = TimeSpan.FromSeconds(timeLeft / 30.0).ToString();

        //// Insert the TimeLeft value into the FinalTimeValue field and the finalTimeDisplay value into the FinalTimeString field of the Quests table
        //string sql = "INSERT INTO Quests (QuestID, FinalTimeValue, FinalTimeString) VALUES (@QuestID, @FinalTimeValue, @FinalTimeString)";
        //using (SQLiteCommand cmd1 = new SQLiteCommand(sql, conn))
        //{
        //    cmd1.Parameters.AddWithValue("@QuestID", 1);
        //    cmd1.Parameters.AddWithValue("@FinalTimeValue", timeLeft);
        //    cmd1.Parameters.AddWithValue("@FinalTimeString", finalTimeDisplay.ToString("mm\\:ss\\.ff"));
        //    cmd1.ExecuteNonQuery();
        //}

        //sql = "SELECT FinalTimeValue, FinalTimeString FROM Quests WHERE QuestID = @QuestID ORDER BY FinalTimeValue ASC";
        //using (SQLiteCommand cmd1 = new SQLiteCommand(sql, conn))
        //{
        //    cmd1.Parameters.AddWithValue("@QuestID", 1);
        //    using (SQLiteDataReader reader = cmd1.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            int finalTimeValue = reader.GetInt32(0);
        //            string finalTimeString = reader.GetString(1);
        //            // Do something with the finalTimeValue and finalTimeString values
        //        }
        //    }
        //}

        public void InsertAllExternalPlayerQuestsData()
        {
            // TODO: hygogg, etc.
        }

        public string CalculateStringHash(string input)
        {
            // Calculate the hash value for the data in the row
            // concatenate the relevant data from the row
            byte[] dataBytes = Encoding.UTF8.GetBytes(input); // convert the data to a byte array
            byte[] hashBytes = SHA256.Create().ComputeHash(dataBytes); // compute the hash value
            string hash = Convert.ToBase64String(hashBytes); // convert the hash value to a string
            return hash;
        }

        public string CalculateFileHash(string folderPath, string fileName)
        {
            // Calculate the SHA256 hash of a file
            string filePath = folderPath + fileName;
            byte[] hash;
            using (var stream = new BufferedStream(File.OpenRead(filePath), 1200000))
            {
                hash = SHA256.Create().ComputeHash(stream);
            }

            // Convert the hash to a hexadecimal string
            string hashString = BitConverter.ToString(hash).Replace("-", string.Empty);

            // Print the hash to the console
            return hashString;
        }

        public void InsertQuestData(string connectionString, DataLoader dataLoader)
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (!dataLoader.model.ValidateGameFolder())
                return;

            if (!s.EnableQuestLogging)
                return;

            if (!dataLoader.model.questCleared)
                return;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var model = dataLoader.model;
                string sql;
                DateTime createdAt = DateTime.Now;
                string createdBy = dataLoader.model.GetFullCurrentProgramVersion();
                // TODO: tomotaka is 2, hygogg is 3, etc. prob make a dictionary that holds these.
                int playerID = 1;

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int runID;

                        using (var cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = @"SELECT MAX(RunID) FROM Quests;";
                            var result = cmd.ExecuteScalar();
                            if (result != null && result.ToString() != "")
                                runID = Convert.ToInt32(result);
                            else
                                runID = 1;// TODO test
                        }

                        // Insert data into the Quests table
                        sql = @"INSERT INTO Quests (
                        QuestHash,
                        CreatedAt,
                        CreatedBy,
                        QuestID, 
                        FinalTimeValue, 
                        FinalTimeDisplay, 
                        ObjectiveImage, 
                        ObjectiveTypeID,
                        ObjectiveQuantity,
                        StarGrade,
                        RankName,
                        ObjectiveName, 
                        Date,
                        YouTubeID,
                        AttackBuffDictionary,
                        HitCountDictionary,
                        HitsPerSecondDictionary,
                        DamageDealtDictionary,
                        DamagePerSecondDictionary,
                        AreaChangesDictionary,
                        CartsDictionary,
                        Monster1HPDictionary,
                        Monster2HPDictionary,
                        Monster3HPDictionary,
                        Monster4HPDictionary,
                        HitsTakenBlockedDictionary,
                        HitsTakenBlockedPerSecondDictionary,
                        PlayerHPDictionary,
                        PlayerStaminaDictionary,
                        KeystrokesDictionary,
                        MouseInputDictionary,
                        GamepadInputDictionary,
                        ActionsPerMinuteDictionary,
                        PartySize,
                        OverlayMode
                        ) VALUES (
                        @QuestHash,
                        @CreatedAt,
                        @CreatedBy,
                        @QuestID, 
                        @FinalTimeValue,
                        @FinalTimeDisplay, 
                        @ObjectiveImage,
                        @ObjectiveTypeID, 
                        @ObjectiveQuantity, 
                        @StarGrade,
                        @RankName,
                        @ObjectiveName, 
                        @Date,
                        @YouTubeID,
                        @AttackBuffDictionary,
                        @HitCountDictionary,
                        @HitsPerSecondDictionary,
                        @DamageDealtDictionary,
                        @DamagePerSecondDictionary,
                        @AreaChangesDictionary,
                        @CartsDictionary,
                        @Monster1HPDictionary,
                        @Monster2HPDictionary,
                        @Monster3HPDictionary,
                        @Monster4HPDictionary,
                        @HitsTakenBlockedDictionary,
                        @HitsTakenBlockedPerSecondDictionary,
                        @PlayerHPDictionary,
                        @PlayerStaminaDictionary,
                        @KeystrokesDictionary,
                        @MouseInputDictionary,
                        @GamepadInputDictionary,
                        @ActionsPerMinuteDictionary,
                        @PartySize,
                        @OverlayMode
                        )";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int questID = model.QuestID();
                            int timeLeft = model.TimeInt(); // Example value of the TimeLeft variable
                            int finalTimeValue = timeLeft;
                            // Calculate the elapsed time of the quest
                            string finalTimeDisplay = dataLoader.GetQuestTimeCompletion();
                            // Convert the elapsed time to a DateTime object
                            DateTime endTime = DateTime.ParseExact(finalTimeDisplay, @"mm\:ss\.ff", CultureInfo.InvariantCulture);
                            string objectiveImage;
                            //Gathering/etc
                            if ((dataLoader.model.ObjectiveType() == 0x0 || dataLoader.model.ObjectiveType() == 0x02 || dataLoader.model.ObjectiveType() == 0x1002) && (dataLoader.model.QuestID() != 23527 && dataLoader.model.QuestID() != 23628 && dataLoader.model.QuestID() != 21731 && dataLoader.model.QuestID() != 21749 && dataLoader.model.QuestID() != 21746 && dataLoader.model.QuestID() != 21750))
                            {
                                objectiveImage = MainWindow.GetAreaIconFromID(dataLoader.model.AreaID());
                            }
                            //Tenrou Sky Corridor areas
                            else if (dataLoader.model.AreaID() == 391 || dataLoader.model.AreaID() == 392 || dataLoader.model.AreaID() == 394 || dataLoader.model.AreaID() == 415 || dataLoader.model.AreaID() == 416)
                            {
                                objectiveImage = MainWindow.GetAreaIconFromID(dataLoader.model.AreaID());

                            }
                            //Duremudira Doors
                            else if (dataLoader.model.AreaID() == 399 || dataLoader.model.AreaID() == 414)
                            {
                                objectiveImage = MainWindow.GetAreaIconFromID(dataLoader.model.AreaID());
                            }
                            //Duremudira Arena
                            else if (dataLoader.model.AreaID() == 398)
                            {
                                objectiveImage = dataLoader.model.getMonsterIcon(dataLoader.model.LargeMonster1ID());
                            }
                            //Hunter's Road Base Camp
                            else if (dataLoader.model.AreaID() == 459)
                            {
                                objectiveImage = MainWindow.GetAreaIconFromID(dataLoader.model.AreaID());
                            }
                            //Raviente
                            else if (dataLoader.model.AreaID() == 309 || (dataLoader.model.AreaID() >= 311 && dataLoader.model.AreaID() <= 321) || (dataLoader.model.AreaID() >= 417 && dataLoader.model.AreaID() <= 422) || dataLoader.model.AreaID() == 437 || (dataLoader.model.AreaID() >= 440 && dataLoader.model.AreaID() <= 444))
                            {
                                objectiveImage = dataLoader.model.getMonsterIcon(dataLoader.model.LargeMonster1ID());
                            }
                            else
                            {
                                objectiveImage = dataLoader.model.getMonsterIcon(dataLoader.model.LargeMonster1ID());
                            }

                            int objectiveTypeID = model.ObjectiveType();

                            string objectiveName;
                            if ((model.ObjectiveType() == 0x0 || model.ObjectiveType() == 0x02 || model.ObjectiveType() == 0x1002 || model.ObjectiveType() == 0x10) && (model.QuestID() != 23527 && model.QuestID() != 23628 && model.QuestID() != 21731 && model.QuestID() != 21749 && model.QuestID() != 21746 && model.QuestID() != 21750))
                                objectiveName = model.GetObjective1Name(model.Objective1ID(), true);
                            else
                                objectiveName = model.GetRealMonsterName(model.CurrentMonster1Icon, true);

                            string rankName = model.GetRankNameFromID(model.RankBand(), true);
                            int objectiveQuantity = model.Objective1Quantity();
                            int starGrade = model.StarGrades();

                            if ((model.ObjectiveType() == 0x0 || model.ObjectiveType() == 0x02 || model.ObjectiveType() == 0x1002 || model.ObjectiveType() == 0x10) && (model.QuestID() != 23527 && model.QuestID() != 23628 && model.QuestID() != 21731 && model.QuestID() != 21749 && model.QuestID() != 21746 && model.QuestID() != 21750))
                                objectiveName = model.GetObjective1Name(model.Objective1ID(), true);
                            else
                                objectiveName = model.GetRealMonsterName(model.CurrentMonster1Icon, true);

                            DateTime date = DateTime.Now;

                            // TODO
                            //rick roll
                            string youtubeID = "dQw4w9WgXcQ";
                            Dictionary<int, int> attackBuffDictionary = dataLoader.model.attackBuffDictionary;
                            Dictionary<int, int> hitCountDictionary = dataLoader.model.hitCountDictionary;
                            Dictionary<int, double> hitsPerSecondDictionary = dataLoader.model.hitsPerSecondDictionary;
                            Dictionary<int, int> damageDealtDictionary = dataLoader.model.damageDealtDictionary;
                            Dictionary<int, double> damagePerSecondDictionary = dataLoader.model.damagePerSecondDictionary;
                            Dictionary<int, int> areaChangesDictionary = dataLoader.model.areaChangesDictionary;
                            Dictionary<int, int> cartsDictionary = dataLoader.model.cartsDictionary;
                            // time <monsterid, monsterhp>
                            Dictionary<int, Dictionary<int, int>> monster1HPDictionary = dataLoader.model.monster1HPDictionary;
                            Dictionary<int, Dictionary<int, int>> monster2HPDictionary = dataLoader.model.monster2HPDictionary;
                            Dictionary<int, Dictionary<int, int>> monster3HPDictionary = dataLoader.model.monster3HPDictionary;
                            Dictionary<int, Dictionary<int, int>> monster4HPDictionary = dataLoader.model.monster4HPDictionary;
                            Dictionary<int, Dictionary<int, int>> hitsTakenBlockedDictionary = dataLoader.model.hitsTakenBlockedDictionary;
                            Dictionary<int, double> hitsTakenBlockedPerSecondDictionary = dataLoader.model.hitsTakenBlockedPerSecondDictionary;
                            Dictionary<int, int> playerHPDictionary = dataLoader.model.playerHPDictionary;
                            Dictionary<int, int> playerStaminaDictionary = dataLoader.model.playerStaminaDictionary;
                            Dictionary<int, string> keystrokesDictionary = dataLoader.model.keystrokesDictionary;
                            Dictionary<int, string> mouseInputDictionary = dataLoader.model.mouseInputDictionary;
                            Dictionary<int, string> gamepadInputDictionary = dataLoader.model.gamepadInputDictionary;
                            Dictionary<int, double> actionsPerMinuteDictionary = dataLoader.model.actionsPerMinuteDictionary;
                            int partySize = dataLoader.model.PartySize();
                            string overlayMode = dataLoader.model.GetOverlayMode();
                            overlayMode = overlayMode.Replace("(", "");
                            overlayMode = overlayMode.Replace(")", "");
                            overlayMode = overlayMode.Trim();
                            if (overlayMode == null || overlayMode == "")
                                overlayMode = "Standard";
                            //                    --Insert data into the ZenithSkills table
                            //INSERT INTO ZenithSkills(ZenithSkill1, ZenithSkill2, ZenithSkill3, ZenithSkill4, ZenithSkill5, ZenithSkill6)
                            //VALUES(zenithSkillsID, zenithSkillsID, zenithSkillsID, zenithSkillsID, zenithSkillsID, zenithSkillsID);

                            //                    --Get the ZenithSkillsID that was generated
                            //                    SELECT LAST_INSERT_ROWID() as ZenithSkillsID;

                            string questData = string.Format(
                                "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}{33}",
                                runID, createdAt, createdBy, questID, finalTimeValue, 
                                finalTimeDisplay, objectiveImage, objectiveTypeID, objectiveQuantity, starGrade, 
                                rankName, objectiveName, date, attackBuffDictionary, hitCountDictionary,
                                hitsPerSecondDictionary, damageDealtDictionary, damagePerSecondDictionary, areaChangesDictionary, cartsDictionary, 
                                monster1HPDictionary, monster2HPDictionary, monster3HPDictionary, monster4HPDictionary, hitsTakenBlockedDictionary,
                                hitsTakenBlockedPerSecondDictionary, playerHPDictionary, playerStaminaDictionary, keystrokesDictionary, mouseInputDictionary,
                                gamepadInputDictionary, actionsPerMinuteDictionary, partySize, overlayMode
                                );

                            // Calculate the hash value for the data in the row
                            string questHash = CalculateStringHash(questData); // concatenate the relevant data from the row

                            cmd.Parameters.AddWithValue("@QuestHash", questHash);
                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@QuestID", questID);
                            cmd.Parameters.AddWithValue("@FinalTimeValue", finalTimeValue);
                            cmd.Parameters.AddWithValue("@FinalTimeDisplay", finalTimeDisplay);
                            cmd.Parameters.AddWithValue("@ObjectiveImage", objectiveImage);
                            cmd.Parameters.AddWithValue("@ObjectiveTypeID", objectiveTypeID);
                            cmd.Parameters.AddWithValue("@ObjectiveQuantity", objectiveQuantity);
                            cmd.Parameters.AddWithValue("@StarGrade", starGrade);
                            cmd.Parameters.AddWithValue("@RankName", rankName);
                            cmd.Parameters.AddWithValue("@ObjectiveName", objectiveName);
                            cmd.Parameters.AddWithValue("@Date", date);
                            cmd.Parameters.AddWithValue("@YouTubeID", youtubeID);
                            cmd.Parameters.AddWithValue("@AttackBuffDictionary", JsonConvert.SerializeObject(attackBuffDictionary));
                            cmd.Parameters.AddWithValue("@HitCountDictionary", JsonConvert.SerializeObject(hitCountDictionary));
                            cmd.Parameters.AddWithValue("@HitsPerSecondDictionary", JsonConvert.SerializeObject(hitsPerSecondDictionary));
                            cmd.Parameters.AddWithValue("@DamageDealtDictionary", JsonConvert.SerializeObject(damageDealtDictionary));
                            cmd.Parameters.AddWithValue("@DamagePerSecondDictionary", JsonConvert.SerializeObject(damagePerSecondDictionary));
                            cmd.Parameters.AddWithValue("@AreaChangesDictionary", JsonConvert.SerializeObject(areaChangesDictionary));
                            cmd.Parameters.AddWithValue("@CartsDictionary", JsonConvert.SerializeObject(cartsDictionary));
                            cmd.Parameters.AddWithValue("@Monster1HPDictionary", JsonConvert.SerializeObject(monster1HPDictionary));
                            cmd.Parameters.AddWithValue("@Monster2HPDictionary", JsonConvert.SerializeObject(monster2HPDictionary));
                            cmd.Parameters.AddWithValue("@Monster3HPDictionary", JsonConvert.SerializeObject(monster3HPDictionary));
                            cmd.Parameters.AddWithValue("@Monster4HPDictionary", JsonConvert.SerializeObject(monster4HPDictionary));
                            cmd.Parameters.AddWithValue("@HitsTakenBlockedDictionary", JsonConvert.SerializeObject(hitsTakenBlockedDictionary));
                            cmd.Parameters.AddWithValue("@HitsTakenBlockedPerSecondDictionary", JsonConvert.SerializeObject(hitsTakenBlockedPerSecondDictionary));
                            cmd.Parameters.AddWithValue("@PlayerHPDictionary", JsonConvert.SerializeObject(playerHPDictionary));
                            cmd.Parameters.AddWithValue("@PlayerStaminaDictionary", JsonConvert.SerializeObject(playerStaminaDictionary));
                            cmd.Parameters.AddWithValue("@KeystrokesDictionary", JsonConvert.SerializeObject(keystrokesDictionary));
                            cmd.Parameters.AddWithValue("@MouseInputDictionary", JsonConvert.SerializeObject(mouseInputDictionary));
                            cmd.Parameters.AddWithValue("@GamepadInputDictionary", JsonConvert.SerializeObject(gamepadInputDictionary));
                            cmd.Parameters.AddWithValue("@ActionsPerMinuteDictionary", JsonConvert.SerializeObject(actionsPerMinuteDictionary));
                            cmd.Parameters.AddWithValue("@PartySize", partySize);
                            cmd.Parameters.AddWithValue("@OverlayMode", overlayMode);

                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            runID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO GameFolder (
                        GameFolderHash,
                        CreatedAt,
                        CreatedBy,
                        RunID, 
                        GameFolderPath, 
                        mhfdatHash, 
                        mhfemdHash,
                        mhfinfHash, 
                        mhfsqdHash,
                        mhfodllHash,
                        mhfohddllHash
                        ) VALUES (
                        @GameFolderHash,
                        @CreatedAt,
                        @CreatedBy,
                        @RunID, 
                        @GameFolderPath,
                        @mhfdatHash,
                        @mhfemdHash,
                        @mhfinfHash,
                        @mhfsqdHash,
                        @mhfodllHash,
                        @mhfohddllHash)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            string gameFolderPath = s.GameFolderPath;
                            string mhfdatHash = CalculateFileHash(gameFolderPath, @"\dat\mhfdat.bin");
                            string mhfemdHash = CalculateFileHash(gameFolderPath, @"\dat\mhfemd.bin");
                            string mhfinfHash = CalculateFileHash(gameFolderPath, @"\dat\mhfinf.bin");
                            string mhfsqdHash = CalculateFileHash(gameFolderPath, @"\dat\mhfsqd.bin");
                            string mhfodllHash = CalculateFileHash(gameFolderPath, @"\mhfo.dll");
                            string mhfohddllHash = CalculateFileHash(gameFolderPath, @"\mhfo-hd.dll");
                            string datFolderData = string.Format(
                                "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                                createdAt, createdBy, runID,
                                gameFolderPath, mhfdatHash, mhfemdHash,
                                mhfinfHash, mhfsqdHash, mhfodllHash, mhfohddllHash);
                            string datFolderHash = CalculateStringHash(datFolderData);

                            cmd.Parameters.AddWithValue("@GameFolderHash", datFolderHash);
                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@GameFolderPath", gameFolderPath);
                            cmd.Parameters.AddWithValue("@mhfdatHash", mhfdatHash);
                            cmd.Parameters.AddWithValue("@mhfemdHash", mhfemdHash);
                            cmd.Parameters.AddWithValue("@mhfinfHash", mhfinfHash);
                            cmd.Parameters.AddWithValue("@mhfsqdHash", mhfsqdHash);
                            cmd.Parameters.AddWithValue("@mhfodllHash", mhfodllHash);
                            cmd.Parameters.AddWithValue("@mhfohddllHash", mhfohddllHash);
                            cmd.ExecuteNonQuery();
                        }

                        InsertPlayerDictionaryDataIntoTable(conn);

                        // Get the ID of the last inserted row in the Players table
                        //sql = "SELECT LAST_INSERT_ROWID()";
                        //int playerID;
                        //using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        //{
                        //    playerID = Convert.ToInt32(cmd.ExecuteScalar());
                        //}

                        // Insert data into the ZenithSkills table
                        sql = @"INSERT INTO ZenithSkills (
                        CreatedAt,
                        CreatedBy,
                        RunID, 
                        ZenithSkill1ID, 
                        ZenithSkill2ID,
                        ZenithSkill3ID, 
                        ZenithSkill4ID, 
                        ZenithSkill5ID, 
                        ZenithSkill6ID,
                        ZenithSkill7ID) 
                        VALUES (
                        @CreatedAt,
                        @CreatedBy,
                        @RunID,
                        @ZenithSkill1ID,
                        @ZenithSkill2ID, 
                        @ZenithSkill3ID,
                        @ZenithSkill4ID, 
                        @ZenithSkill5ID, 
                        @ZenithSkill6ID,
                        @ZenithSkill7ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int zenithSkill1ID = model.ZenithSkill1();
                            int zenithSkill2ID = model.ZenithSkill2();
                            int zenithSkill3ID = model.ZenithSkill3();
                            int zenithSkill4ID = model.ZenithSkill4();
                            int zenithSkill5ID = model.ZenithSkill5();
                            int zenithSkill6ID = model.ZenithSkill6();
                            int zenithSkill7ID = model.ZenithSkill7();

                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@ZenithSkill1ID", zenithSkill1ID);
                            cmd.Parameters.AddWithValue("@ZenithSkill2ID", zenithSkill2ID);
                            cmd.Parameters.AddWithValue("@ZenithSkill3ID", zenithSkill3ID);
                            cmd.Parameters.AddWithValue("@ZenithSkill4ID", zenithSkill4ID);
                            cmd.Parameters.AddWithValue("@ZenithSkill5ID", zenithSkill5ID);
                            cmd.Parameters.AddWithValue("@ZenithSkill6ID", zenithSkill6ID);
                            cmd.Parameters.AddWithValue("@ZenithSkill7ID", zenithSkill7ID);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int zenithSkillsID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            zenithSkillsID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO AutomaticSkills (
                        CreatedAt,
                        CreatedBy,
                        RunID,
                        AutomaticSkill1ID,
                        AutomaticSkill2ID,
                        AutomaticSkill3ID,
                        AutomaticSkill4ID, 
                        AutomaticSkill5ID,
                        AutomaticSkill6ID
                        ) VALUES (
                        @CreatedAt,
                        @CreatedBy,
                        @RunID,
                        @AutomaticSkill1ID,
                        @AutomaticSkill2ID, 
                        @AutomaticSkill3ID, 
                        @AutomaticSkill4ID, 
                        @AutomaticSkill5ID,
                        @AutomaticSkill6ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int automaticSkill1ID = model.AutomaticSkillWeapon();
                            int automaticSkill2ID = model.AutomaticSkillHead();
                            int automaticSkill3ID = model.AutomaticSkillChest();
                            int automaticSkill4ID = model.AutomaticSkillArms();
                            int automaticSkill5ID = model.AutomaticSkillWaist();
                            int automaticSkill6ID = model.AutomaticSkillLegs();

                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@AutomaticSkill1ID", automaticSkill1ID);
                            cmd.Parameters.AddWithValue("@AutomaticSkill2ID", automaticSkill2ID);
                            cmd.Parameters.AddWithValue("@AutomaticSkill3ID", automaticSkill3ID);
                            cmd.Parameters.AddWithValue("@AutomaticSkill4ID", automaticSkill4ID);
                            cmd.Parameters.AddWithValue("@AutomaticSkill5ID", automaticSkill5ID);
                            cmd.Parameters.AddWithValue("@AutomaticSkill6ID", automaticSkill6ID);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int automaticSkillsID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            automaticSkillsID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO ActiveSkills (
                        CreatedAt,
                        CreatedBy,
                        RunID, 
                        ActiveSkill1ID,
                        ActiveSkill2ID,
                        ActiveSkill3ID,
                        ActiveSkill4ID,
                        ActiveSkill5ID,
                        ActiveSkill6ID,
                        ActiveSkill7ID,
                        ActiveSkill8ID,
                        ActiveSkill9ID,
                        ActiveSkill10ID,
                        ActiveSkill11ID, 
                        ActiveSkill12ID,
                        ActiveSkill13ID,
                        ActiveSkill14ID,
                        ActiveSkill15ID,
                        ActiveSkill16ID,
                        ActiveSkill17ID,
                        ActiveSkill18ID,
                        ActiveSkill19ID
                        ) VALUES (
                        @CreatedAt,
                        @CreatedBy,
                        @RunID, 
                        @ActiveSkill1ID,
                        @ActiveSkill2ID,
                        @ActiveSkill3ID,
                        @ActiveSkill4ID,
                        @ActiveSkill5ID,
                        @ActiveSkill6ID,
                        @ActiveSkill7ID, 
                        @ActiveSkill8ID, 
                        @ActiveSkill9ID, 
                        @ActiveSkill10ID,
                        @ActiveSkill11ID,
                        @ActiveSkill12ID,
                        @ActiveSkill13ID, 
                        @ActiveSkill14ID, 
                        @ActiveSkill15ID, 
                        @ActiveSkill16ID, 
                        @ActiveSkill17ID,
                        @ActiveSkill18ID, 
                        @ActiveSkill19ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int activeSkill1ID = model.ArmorSkill1();
                            int activeSkill2ID = model.ArmorSkill2();
                            int activeSkill3ID = model.ArmorSkill3();
                            int activeSkill4ID = model.ArmorSkill4();
                            int activeSkill5ID = model.ArmorSkill5();
                            int activeSkill6ID = model.ArmorSkill6();
                            int activeSkill7ID = model.ArmorSkill7();
                            int activeSkill8ID = model.ArmorSkill8();
                            int activeSkill9ID = model.ArmorSkill9();
                            int activeSkill10ID = model.ArmorSkill10();
                            int activeSkill11ID = model.ArmorSkill11();
                            int activeSkill12ID = model.ArmorSkill12();
                            int activeSkill13ID = model.ArmorSkill13();
                            int activeSkill14ID = model.ArmorSkill14();
                            int activeSkill15ID = model.ArmorSkill15();
                            int activeSkill16ID = model.ArmorSkill16();
                            int activeSkill17ID = model.ArmorSkill17();
                            int activeSkill18ID = model.ArmorSkill18();
                            int activeSkill19ID = model.ArmorSkill19();

                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@ActiveSkill1ID", activeSkill1ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill2ID", activeSkill2ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill3ID", activeSkill3ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill4ID", activeSkill4ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill5ID", activeSkill5ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill6ID", activeSkill6ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill7ID", activeSkill7ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill8ID", activeSkill8ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill9ID", activeSkill9ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill10ID", activeSkill10ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill11ID", activeSkill11ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill12ID", activeSkill12ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill13ID", activeSkill13ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill14ID", activeSkill14ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill15ID", activeSkill15ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill16ID", activeSkill16ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill17ID", activeSkill17ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill18ID", activeSkill18ID);
                            cmd.Parameters.AddWithValue("@ActiveSkill19ID", activeSkill19ID);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int activeSkillsID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            activeSkillsID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO CaravanSkills (
                        CreatedAt,
                        CreatedBy,
                        RunID, 
                        CaravanSkill1ID,
                        CaravanSkill2ID,
                        CaravanSkill3ID
                        ) VALUES (
                        @CreatedAt,
                        @CreatedBy,
                        @RunID, 
                        @CaravanSkill1ID,
                        @CaravanSkill2ID,
                        @CaravanSkill3ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int caravanSkill1ID = model.CaravanSkill1();
                            int caravanSkill2ID = model.CaravanSkill2();
                            int caravanSkill3ID = model.CaravanSkill3();

                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@CaravanSkill1ID", caravanSkill1ID);
                            cmd.Parameters.AddWithValue("@CaravanSkill2ID", caravanSkill2ID);
                            cmd.Parameters.AddWithValue("@CaravanSkill3ID", caravanSkill3ID);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int caravanSkillsID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            caravanSkillsID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO StyleRankSkills (
                        CreatedAt,
                        CreatedBy,
                        RunID, 
                        StyleRankSkill1ID,
                        StyleRankSkill2ID) 
                        VALUES (
                        @CreatedAt,
                        @CreatedBy,
                        @RunID,
                        @StyleRankSkill1ID,
                        @StyleRankSkill2ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int styleRankSkill1ID = model.StyleRank1();
                            int styleRankSkill2ID = model.StyleRank2();

                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@StyleRankSkill1ID", styleRankSkill1ID);
                            cmd.Parameters.AddWithValue("@StyleRankSkill2ID", styleRankSkill2ID);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int styleRankSkillsID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            styleRankSkillsID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO PlayerInventory (
                            CreatedAt,
                            CreatedBy,
                            RunID,
                            Item1ID , 
                            Item1Quantity ,
                            Item2ID , 
                            Item2Quantity ,
                            Item3ID , 
                            Item3Quantity ,
                            Item4ID , 
                            Item4Quantity ,
                            Item5ID , 
                            Item5Quantity ,
                            Item6ID , 
                            Item6Quantity ,
                            Item7ID , 
                            Item7Quantity ,
                            Item8ID , 
                            Item8Quantity ,
                            Item9ID , 
                            Item9Quantity ,
                            Item10ID , 
                            Item10Quantity ,
                            Item11ID , 
                            Item11Quantity ,
                            Item12ID , 
                            Item12Quantity ,
                            Item13ID , 
                            Item13Quantity ,
                            Item14ID , 
                            Item14Quantity ,
                            Item15ID , 
                            Item15Quantity ,
                            Item16ID , 
                            Item16Quantity ,
                            Item17ID , 
                            Item17Quantity ,
                            Item18ID , 
                            Item18Quantity ,
                            Item19ID , 
                            Item19Quantity ,
                            Item20ID , 
                            Item20Quantity )
                            VALUES (
                            @CreatedAt,
                            @CreatedBy,
                            @RunID,
                            @Item1ID , 
                            @Item1Quantity ,
                            @Item2ID , 
                            @Item2Quantity ,
                            @Item3ID , 
                            @Item3Quantity ,
                            @Item4ID , 
                            @Item4Quantity ,
                            @Item5ID , 
                            @Item5Quantity ,
                            @Item6ID , 
                            @Item6Quantity ,
                            @Item7ID , 
                            @Item7Quantity ,
                            @Item8ID , 
                            @Item8Quantity ,
                            @Item9ID , 
                            @Item9Quantity ,
                            @Item10ID , 
                            @Item10Quantity ,
                            @Item11ID , 
                            @Item11Quantity ,
                            @Item12ID , 
                            @Item12Quantity ,
                            @Item13ID , 
                            @Item13Quantity ,
                            @Item14ID , 
                            @Item14Quantity ,
                            @Item15ID , 
                            @Item15Quantity ,
                            @Item16ID , 
                            @Item16Quantity ,
                            @Item17ID , 
                            @Item17Quantity ,
                            @Item18ID , 
                            @Item18Quantity ,
                            @Item19ID , 
                            @Item19Quantity ,
                            @Item20ID , 
                            @Item20Quantity )";

                        // TODO: test
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int item1ID = model.PouchItem1IDAtQuestStart;
                            int item1Quantity = model.PouchItem1QuantityAtQuestStart;
                            int item2ID = model.PouchItem2IDAtQuestStart;
                            int item2Quantity = model.PouchItem2QuantityAtQuestStart;
                            int item3ID = model.PouchItem3IDAtQuestStart;
                            int item3Quantity = model.PouchItem3QuantityAtQuestStart;
                            int item4ID = model.PouchItem4IDAtQuestStart;
                            int item4Quantity = model.PouchItem4QuantityAtQuestStart;
                            int item5ID = model.PouchItem5IDAtQuestStart;
                            int item5Quantity = model.PouchItem5QuantityAtQuestStart;
                            int item6ID = model.PouchItem6IDAtQuestStart;
                            int item6Quantity = model.PouchItem6QuantityAtQuestStart;
                            int item7ID = model.PouchItem7IDAtQuestStart;
                            int item7Quantity = model.PouchItem7QuantityAtQuestStart;
                            int item8ID = model.PouchItem8IDAtQuestStart;
                            int item8Quantity = model.PouchItem8QuantityAtQuestStart;
                            int item9ID = model.PouchItem9IDAtQuestStart;
                            int item9Quantity = model.PouchItem9QuantityAtQuestStart;
                            int item10ID = model.PouchItem10IDAtQuestStart;
                            int item10Quantity = model.PouchItem10QuantityAtQuestStart;
                            int item11ID = model.PouchItem11IDAtQuestStart;
                            int item11Quantity = model.PouchItem11QuantityAtQuestStart;
                            int item12ID = model.PouchItem12IDAtQuestStart;
                            int item12Quantity = model.PouchItem12QuantityAtQuestStart;
                            int item13ID = model.PouchItem13IDAtQuestStart;
                            int item13Quantity = model.PouchItem13QuantityAtQuestStart;
                            int item14ID = model.PouchItem14IDAtQuestStart;
                            int item14Quantity = model.PouchItem14QuantityAtQuestStart;
                            int item15ID = model.PouchItem15IDAtQuestStart;
                            int item15Quantity = model.PouchItem15QuantityAtQuestStart;
                            int item16ID = model.PouchItem16IDAtQuestStart;
                            int item16Quantity = model.PouchItem16QuantityAtQuestStart;
                            int item17ID = model.PouchItem17IDAtQuestStart;
                            int item17Quantity = model.PouchItem17QuantityAtQuestStart;
                            int item18ID = model.PouchItem18IDAtQuestStart;
                            int item18Quantity = model.PouchItem18QuantityAtQuestStart;
                            int item19ID = model.PouchItem19IDAtQuestStart;
                            int item19Quantity = model.PouchItem19QuantityAtQuestStart;
                            int item20ID = model.PouchItem20IDAtQuestStart;
                            int item20Quantity = model.PouchItem20QuantityAtQuestStart;

                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@Item1ID", item1ID);
                            cmd.Parameters.AddWithValue("@Item1Quantity", item1Quantity);
                            cmd.Parameters.AddWithValue("@Item2ID", item2ID);
                            cmd.Parameters.AddWithValue("@Item2Quantity", item2Quantity);
                            cmd.Parameters.AddWithValue("@Item3ID", item3ID);
                            cmd.Parameters.AddWithValue("@Item3Quantity", item3Quantity);
                            cmd.Parameters.AddWithValue("@Item4ID", item4ID);
                            cmd.Parameters.AddWithValue("@Item4Quantity", item4Quantity);
                            cmd.Parameters.AddWithValue("@Item5ID", item5ID);
                            cmd.Parameters.AddWithValue("@Item5Quantity", item5Quantity);
                            cmd.Parameters.AddWithValue("@Item6ID", item6ID);
                            cmd.Parameters.AddWithValue("@Item6Quantity", item6Quantity);
                            cmd.Parameters.AddWithValue("@Item7ID", item7ID);
                            cmd.Parameters.AddWithValue("@Item7Quantity", item7Quantity);
                            cmd.Parameters.AddWithValue("@Item8ID", item8ID);
                            cmd.Parameters.AddWithValue("@Item8Quantity", item8Quantity);
                            cmd.Parameters.AddWithValue("@Item9ID", item9ID);
                            cmd.Parameters.AddWithValue("@Item9Quantity", item9Quantity);
                            cmd.Parameters.AddWithValue("@Item10ID", item10ID);
                            cmd.Parameters.AddWithValue("@Item10Quantity", item10Quantity);
                            cmd.Parameters.AddWithValue("@Item11ID", item11ID);
                            cmd.Parameters.AddWithValue("@Item11Quantity", item11Quantity);
                            cmd.Parameters.AddWithValue("@Item12ID", item12ID);
                            cmd.Parameters.AddWithValue("@Item12Quantity", item12Quantity);
                            cmd.Parameters.AddWithValue("@Item13ID", item13ID);
                            cmd.Parameters.AddWithValue("@Item13Quantity", item13Quantity);
                            cmd.Parameters.AddWithValue("@Item14ID", item14ID);
                            cmd.Parameters.AddWithValue("@Item14Quantity", item14Quantity);
                            cmd.Parameters.AddWithValue("@Item15ID", item15ID);
                            cmd.Parameters.AddWithValue("@Item15Quantity", item15Quantity);
                            cmd.Parameters.AddWithValue("@Item16ID", item16ID);
                            cmd.Parameters.AddWithValue("@Item16Quantity", item16Quantity);
                            cmd.Parameters.AddWithValue("@Item17ID", item17ID);
                            cmd.Parameters.AddWithValue("@Item17Quantity", item17Quantity);
                            cmd.Parameters.AddWithValue("@Item18ID", item18ID);
                            cmd.Parameters.AddWithValue("@Item18Quantity", item18Quantity);
                            cmd.Parameters.AddWithValue("@Item19ID", item19ID);
                            cmd.Parameters.AddWithValue("@Item19Quantity", item19Quantity);
                            cmd.Parameters.AddWithValue("@Item20ID", item20ID);
                            cmd.Parameters.AddWithValue("@Item20Quantity", item20Quantity);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int playerInventoryID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            playerInventoryID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO AmmoPouch (
                            CreatedAt,
                            CreatedBy,
                            RunID,
                            Item1ID , 
                            Item1Quantity ,
                            Item2ID , 
                            Item2Quantity ,
                            Item3ID , 
                            Item3Quantity ,
                            Item4ID , 
                            Item4Quantity ,
                            Item5ID , 
                            Item5Quantity ,
                            Item6ID , 
                            Item6Quantity ,
                            Item7ID , 
                            Item7Quantity ,
                            Item8ID , 
                            Item8Quantity ,
                            Item9ID , 
                            Item9Quantity ,
                            Item10ID , 
                            Item10Quantity
                            )
                            VALUES (
                            @CreatedAt,
                            @CreatedBy,
                            @RunID,
                            @Item1ID , 
                            @Item1Quantity ,
                            @Item2ID , 
                            @Item2Quantity ,
                            @Item3ID , 
                            @Item3Quantity ,
                            @Item4ID , 
                            @Item4Quantity ,
                            @Item5ID , 
                            @Item5Quantity ,
                            @Item6ID , 
                            @Item6Quantity ,
                            @Item7ID , 
                            @Item7Quantity ,
                            @Item8ID , 
                            @Item8Quantity ,
                            @Item9ID , 
                            @Item9Quantity ,
                            @Item10ID , 
                            @Item10Quantity)";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int item1ID = model.AmmoPouchItem1IDAtQuestStart;
                            int item1Quantity = model.AmmoPouchItem1QuantityAtQuestStart;
                            int item2ID = model.AmmoPouchItem2IDAtQuestStart;
                            int item2Quantity = model.AmmoPouchItem2QuantityAtQuestStart;
                            int item3ID = model.AmmoPouchItem3IDAtQuestStart;
                            int item3Quantity = model.AmmoPouchItem3QuantityAtQuestStart;
                            int item4ID = model.AmmoPouchItem4IDAtQuestStart;
                            int item4Quantity = model.AmmoPouchItem4QuantityAtQuestStart;
                            int item5ID = model.AmmoPouchItem5IDAtQuestStart;
                            int item5Quantity = model.AmmoPouchItem5QuantityAtQuestStart;
                            int item6ID = model.AmmoPouchItem6IDAtQuestStart;
                            int item6Quantity = model.AmmoPouchItem6QuantityAtQuestStart;
                            int item7ID = model.AmmoPouchItem7IDAtQuestStart;
                            int item7Quantity = model.AmmoPouchItem7QuantityAtQuestStart;
                            int item8ID = model.AmmoPouchItem8IDAtQuestStart;
                            int item8Quantity = model.AmmoPouchItem8QuantityAtQuestStart;
                            int item9ID = model.AmmoPouchItem9IDAtQuestStart;
                            int item9Quantity = model.AmmoPouchItem9QuantityAtQuestStart;
                            int item10ID = model.AmmoPouchItem10IDAtQuestStart;
                            int item10Quantity = model.AmmoPouchItem10QuantityAtQuestStart;

                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@Item1ID", item1ID);
                            cmd.Parameters.AddWithValue("@Item1Quantity", item1Quantity);
                            cmd.Parameters.AddWithValue("@Item2ID", item2ID);
                            cmd.Parameters.AddWithValue("@Item2Quantity", item2Quantity);
                            cmd.Parameters.AddWithValue("@Item3ID", item3ID);
                            cmd.Parameters.AddWithValue("@Item3Quantity", item3Quantity);
                            cmd.Parameters.AddWithValue("@Item4ID", item4ID);
                            cmd.Parameters.AddWithValue("@Item4Quantity", item4Quantity);
                            cmd.Parameters.AddWithValue("@Item5ID", item5ID);
                            cmd.Parameters.AddWithValue("@Item5Quantity", item5Quantity);
                            cmd.Parameters.AddWithValue("@Item6ID", item6ID);
                            cmd.Parameters.AddWithValue("@Item6Quantity", item6Quantity);
                            cmd.Parameters.AddWithValue("@Item7ID", item7ID);
                            cmd.Parameters.AddWithValue("@Item7Quantity", item7Quantity);
                            cmd.Parameters.AddWithValue("@Item8ID", item8ID);
                            cmd.Parameters.AddWithValue("@Item8Quantity", item8Quantity);
                            cmd.Parameters.AddWithValue("@Item9ID", item9ID);
                            cmd.Parameters.AddWithValue("@Item9Quantity", item9Quantity);
                            cmd.Parameters.AddWithValue("@Item10ID", item10ID);
                            cmd.Parameters.AddWithValue("@Item10Quantity", item10Quantity);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int ammoPouchID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            ammoPouchID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO RoadDureSkills (
                        CreatedAt,
                        CreatedBy,
                        RunID, 
                        RoadDureSkill1ID,
                        RoadDureSkill1Level,
                        RoadDureSkill2ID,
                        RoadDureSkill2Level,
                        RoadDureSkill3ID,
                        RoadDureSkill3Level,
                        RoadDureSkill4ID,
                        RoadDureSkill4Level,
                        RoadDureSkill5ID,
                        RoadDureSkill5Level,
                        RoadDureSkill6ID,
                        RoadDureSkill6Level,
                        RoadDureSkill7ID,
                        RoadDureSkill7Level,
                        RoadDureSkill8ID,
                        RoadDureSkill8Level,
                        RoadDureSkill9ID,
                        RoadDureSkill9Level,
                        RoadDureSkill10ID,
                        RoadDureSkill10Level,
                        RoadDureSkill11ID,
                        RoadDureSkill11Level,
                        RoadDureSkill12ID,
                        RoadDureSkill12Level,
                        RoadDureSkill13ID,
                        RoadDureSkill13Level,
                        RoadDureSkill14ID,
                        RoadDureSkill14Level,
                        RoadDureSkill15ID,
                        RoadDureSkill15Level,
                        RoadDureSkill16ID,
                        RoadDureSkill16Level
                        ) VALUES (
                        @CreatedAt,
                        @CreatedBy,
                        @RunID, 
                        @RoadDureSkill1ID,
                        @RoadDureSkill1Level,
                        @RoadDureSkill2ID,
                        @RoadDureSkill2Level,
                        @RoadDureSkill3ID,
                        @RoadDureSkill3Level,
                        @RoadDureSkill4ID,
                        @RoadDureSkill4Level,
                        @RoadDureSkill5ID,
                        @RoadDureSkill5Level,
                        @RoadDureSkill6ID,
                        @RoadDureSkill6Level,
                        @RoadDureSkill7ID,
                        @RoadDureSkill7Level,
                        @RoadDureSkill8ID,
                        @RoadDureSkill8Level,
                        @RoadDureSkill9ID,
                        @RoadDureSkill9Level,
                        @RoadDureSkill10ID,
                        @RoadDureSkill10Level,
                        @RoadDureSkill11ID,
                        @RoadDureSkill11Level,
                        @RoadDureSkill12ID,
                        @RoadDureSkill12Level,
                        @RoadDureSkill13ID,
                        @RoadDureSkill13Level,
                        @RoadDureSkill14ID,
                        @RoadDureSkill14Level,
                        @RoadDureSkill15ID,
                        @RoadDureSkill15Level,
                        @RoadDureSkill16ID,
                        @RoadDureSkill16Level
                        )";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int roadDureSkill1ID = model.RoadDureSkill1Name();
                            int roadDureSkill2ID = model.RoadDureSkill2Name();
                            int roadDureSkill3ID = model.RoadDureSkill3Name();
                            int roadDureSkill4ID = model.RoadDureSkill4Name();
                            int roadDureSkill5ID = model.RoadDureSkill5Name();
                            int roadDureSkill6ID = model.RoadDureSkill6Name();
                            int roadDureSkill7ID = model.RoadDureSkill7Name();
                            int roadDureSkill8ID = model.RoadDureSkill8Name();
                            int roadDureSkill9ID = model.RoadDureSkill9Name();
                            int roadDureSkill10ID = model.RoadDureSkill10Name();
                            int roadDureSkill11ID = model.RoadDureSkill11Name();
                            int roadDureSkill12ID = model.RoadDureSkill12Name();
                            int roadDureSkill13ID = model.RoadDureSkill13Name();
                            int roadDureSkill14ID = model.RoadDureSkill14Name();
                            int roadDureSkill15ID = model.RoadDureSkill15Name();
                            int roadDureSkill16ID = model.RoadDureSkill16Name();

                            int roadDureSkill1Level = model.RoadDureSkill1Level();
                            int roadDureSkill2Level = model.RoadDureSkill2Level();
                            int roadDureSkill3Level = model.RoadDureSkill3Level();
                            int roadDureSkill4Level = model.RoadDureSkill4Level();
                            int roadDureSkill5Level = model.RoadDureSkill5Level();
                            int roadDureSkill6Level = model.RoadDureSkill6Level();
                            int roadDureSkill7Level = model.RoadDureSkill7Level();
                            int roadDureSkill8Level = model.RoadDureSkill8Level();
                            int roadDureSkill9Level = model.RoadDureSkill9Level();
                            int roadDureSkill10Level = model.RoadDureSkill10Level();
                            int roadDureSkill11Level = model.RoadDureSkill11Level();
                            int roadDureSkill12Level = model.RoadDureSkill12Level();
                            int roadDureSkill13Level = model.RoadDureSkill13Level();
                            int roadDureSkill14Level = model.RoadDureSkill14Level();
                            int roadDureSkill15Level = model.RoadDureSkill15Level();
                            int roadDureSkill16Level = model.RoadDureSkill16Level();

                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill1ID", roadDureSkill1ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill2ID", roadDureSkill2ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill3ID", roadDureSkill3ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill4ID", roadDureSkill4ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill5ID", roadDureSkill5ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill6ID", roadDureSkill6ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill7ID", roadDureSkill7ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill8ID", roadDureSkill8ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill9ID", roadDureSkill9ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill10ID", roadDureSkill10ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill11ID", roadDureSkill11ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill12ID", roadDureSkill12ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill13ID", roadDureSkill13ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill14ID", roadDureSkill14ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill15ID", roadDureSkill15ID);
                            cmd.Parameters.AddWithValue("@RoadDureSkill16ID", roadDureSkill16ID);

                            cmd.Parameters.AddWithValue("@RoadDureSkill1Level", roadDureSkill1Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill2Level", roadDureSkill2Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill3Level", roadDureSkill3Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill4Level", roadDureSkill4Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill5Level", roadDureSkill5Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill6Level", roadDureSkill6Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill7Level", roadDureSkill7Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill8Level", roadDureSkill8Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill9Level", roadDureSkill9Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill10Level", roadDureSkill10Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill11Level", roadDureSkill11Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill12Level", roadDureSkill12Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill13Level", roadDureSkill13Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill14Level", roadDureSkill14Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill15Level", roadDureSkill15Level);
                            cmd.Parameters.AddWithValue("@RoadDureSkill16Level", roadDureSkill16Level);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int roadDureSkillsID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            roadDureSkillsID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string gearName = s.GearDescriptionExport;
                        if (gearName == "" || gearName == null)
                            gearName = "Unnamed";

                        int weaponTypeID = model.WeaponType();
                        int weaponClassID = weaponTypeID;
                        int weaponID = model.BlademasterWeaponID();//ranged and melee are the same afaik
                        string weaponSlot1 = model.GetDecoName(model.WeaponDeco1ID(), 1);// no sigils in database ig
                        string weaponSlot2 = model.GetDecoName(model.WeaponDeco2ID(), 2);
                        string weaponSlot3 = model.GetDecoName(model.WeaponDeco3ID(), 3);
                        int headID = model.ArmorHeadID();
                        int headSlot1 = model.ArmorHeadDeco1ID();
                        int headSlot2 = model.ArmorHeadDeco2ID();
                        int headSlot3 = model.ArmorHeadDeco3ID();
                        int chestID = model.ArmorChestID();
                        int chestSlot1 = model.ArmorChestDeco1ID();
                        int chestSlot2 = model.ArmorChestDeco2ID();
                        int chestSlot3 = model.ArmorChestDeco3ID();
                        int armsID = model.ArmorArmsID();
                        int armsSlot1 = model.ArmorArmsDeco1ID();
                        int armsSlot2 = model.ArmorArmsDeco2ID();
                        int armsSlot3 = model.ArmorArmsDeco3ID();
                        int waistID = model.ArmorWaistID();
                        int waistSlot1 = model.ArmorWaistDeco1ID();
                        int waistSlot2 = model.ArmorWaistDeco2ID();
                        int waistSlot3 = model.ArmorWaistDeco3ID();
                        int legsID = model.ArmorLegsID();
                        int legsSlot1 = model.ArmorLegsDeco1ID();
                        int legsSlot2 = model.ArmorLegsDeco2ID();
                        int legsSlot3 = model.ArmorLegsDeco3ID();
                        int cuffSlot1 = model.Cuff1ID();
                        int cuffSlot2 = model.Cuff2ID();
                        string questName = model.GetQuestNameFromID(model.QuestID());
                        int styleID = model.WeaponStyle();
                        int weaponIconID = weaponTypeID;
                        int divaSkillID = model.DivaSkill();
                        int guildFoodID = model.GuildFoodSkill();
                        int poogieItemID = model.PoogieItemUseID();

                        int? blademasterWeaponID = null;
                        int? gunnerWeaponID = null;

                        //Check the WeaponTypeID and insert the corresponding weapon ID
                        switch (weaponTypeID)
                        {
                            case 0:
                            case 2:
                            case 3:
                            case 4:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 11:
                            case 12:
                            case 13:
                                blademasterWeaponID = model.BlademasterWeaponID();
                                break;
                            case 1:
                            case 5:
                            case 10:
                                gunnerWeaponID = model.GunnerWeaponID();
                                break;
                        }

                        Dictionary<int, Dictionary<List<int>,List<int>>> playerInventoryDictionary = dataLoader.model.playerInventoryDictionary;
                        Dictionary<int, Dictionary<List<int>, List<int>>> playerAmmoPouchDictionary = dataLoader.model.playerAmmoPouchDictionary;
                        Dictionary<int, Dictionary<List<int>, List<int>>> partnyaBagDictionary = dataLoader.model.partnyaBagDictionary;

                        string insertSql = @"INSERT INTO PlayerGear (
                        PlayerGearHash,
                        CreatedAt,
                        CreatedBy,
                        --PlayerGearID INTEGER PRIMARY KEY AUTOINCREMENT,
                        RunID, --INTEGER NOT NULL, 
                        PlayerID, --INTEGER NOT NULL,
                        GearName, --TEXT NOT NULL,
                        StyleID, --INTEGER NOT NULL CHECK (StyleID >= 0),
                        WeaponIconID, --INTEGER NOT NULL,
                        WeaponClassID, --INTEGER NOT NULL,
                        WeaponTypeID, --INTEGER NOT NULL CHECK (WeaponTypeID >= 0),
                        BlademasterWeaponID, --INTEGER,
                        GunnerWeaponID, --INTEGER,
                        WeaponSlot1, --TEXT NOT NULL,
                        WeaponSlot2, --TEXT NOT NULL,
                        WeaponSlot3, --TEXT NOT NULL,
                        HeadID, --INTEGER NOT NULL CHECK (HeadID >= 0), 
                        HeadSlot1ID, --INTEGER NOT NULL CHECK (HeadSlot1ID >= 0),
                        HeadSlot2ID, --INTEGER NOT NULL CHECK (HeadSlot2ID >= 0),
                        HeadSlot3ID, --INTEGER NOT NULL CHECK (HeadSlot3ID >= 0),
                        ChestID, --INTEGER NOT NULL CHECK (ChestID >= 0),
                        ChestSlot1ID, --INTEGER NOT NULL CHECK (ChestSlot1ID >= 0),
                        ChestSlot2ID,-- INTEGER NOT NULL CHECK (ChestSlot2ID >= 0),
                        ChestSlot3ID,-- INTEGER NOT NULL CHECK (ChestSlot3ID >= 0),
                        ArmsID,-- INTEGER NOT NULL CHECK (ArmsID >= 0),
                        ArmsSlot1ID,-- INTEGER NOT NULL CHECK (ArmsSlot1ID >= 0),
                        ArmsSlot2ID,-- INTEGER NOT NULL CHECK (ArmsSlot2ID >= 0),
                        ArmsSlot3ID,-- INTEGER NOT NULL CHECK (ArmsSlot3ID >= 0),
                        WaistID,-- INTEGER NOT NULL CHECK (WaistID >= 0),
                        WaistSlot1ID,-- INTEGER NOT NULL CHECK (WaistSlot1ID >= 0),
                        WaistSlot2ID,-- INTEGER NOT NULL CHECK (WaistSlot2ID >= 0),
                        WaistSlot3ID,-- INTEGER NOT NULL CHECK (WaistSlot3ID >= 0),
                        LegsID,-- INTEGER NOT NULL CHECK (LegsID >= 0),
                        LegsSlot1ID,-- INTEGER NOT NULL CHECK (LegsSlot1ID >= 0),
                        LegsSlot2ID,-- INTEGER NOT NULL CHECK (LegsSlot2ID >= 0),
                        LegsSlot3ID,-- INTEGER NOT NULL CHECK (LegsSlot3ID >= 0),
                        Cuff1ID,-- INTEGER NOT NULL CHECK (Cuff1ID >= 0),
                        Cuff2ID,-- INTEGER NOT NULL CHECK (Cuff2ID >= 0),
                        ZenithSkillsID,-- INTEGER NOT NULL,
                        AutomaticSkillsID,-- INTEGER NOT NULL,
                        ActiveSkillsID,-- INTEGER NOT NULL,
                        CaravanSkillsID,-- INTEGER NOT NULL,
                        DivaSkillID,-- INTEGER NOT NULL,
                        GuildFoodID,-- INTEGER NOT NULL,
                        StyleRankSkillsID,-- INTEGER NOT NULL,
                        PlayerInventoryID,-- INTEGER NOT NULL,
                        AmmoPouchID,-- INTEGER NOT NULL,
                        PoogieItemID,-- INTEGER NOT NULL,
                        RoadDureSkillsID,-- INTEGER NOT NULL,
                        PlayerInventoryDictionary,-- TEXT NOT NULL,
                        PlayerAmmoPouchDictionary,-- TEXT NOT NULL,
                        PartnyaBagDictionary-- TEXT NOT NULL,
                        ) VALUES (
                        @PlayerGearHash,
                        @CreatedAt,
                        @CreatedBy,
                        --PlayerGearID INTEGER PRIMARY KEY AUTOINCREMENT,
                        @RunID, --INTEGER NOT NULL, 
                        @PlayerID, --INTEGER NOT NULL,
                        @GearName, --TEXT NOT NULL,
                        @StyleID, --INTEGER NOT NULL CHECK (StyleID >= 0),
                        @WeaponIconID, --INTEGER NOT NULL,
                        @WeaponClassID, --INTEGER NOT NULL,
                        @WeaponTypeID, --INTEGER NOT NULL CHECK (WeaponTypeID >= 0),
                        @BlademasterWeaponID, --INTEGER,
                        @GunnerWeaponID, --INTEGER,
                        @WeaponSlot1, --TEXT NOT NULL,
                        @WeaponSlot2, --TEXT NOT NULL,
                        @WeaponSlot3, --TEXT NOT NULL,
                        @HeadID, --INTEGER NOT NULL CHECK (HeadID >= 0), 
                        @HeadSlot1ID, --INTEGER NOT NULL CHECK (HeadSlot1ID >= 0),
                        @HeadSlot2ID, --INTEGER NOT NULL CHECK (HeadSlot2ID >= 0),
                        @HeadSlot3ID, --INTEGER NOT NULL CHECK (HeadSlot3ID >= 0),
                        @ChestID, --INTEGER NOT NULL CHECK (ChestID >= 0),
                        @ChestSlot1ID, --INTEGER NOT NULL CHECK (ChestSlot1ID >= 0),
                        @ChestSlot2ID,-- INTEGER NOT NULL CHECK (ChestSlot2ID >= 0),
                        @ChestSlot3ID,-- INTEGER NOT NULL CHECK (ChestSlot3ID >= 0),
                        @ArmsID,-- INTEGER NOT NULL CHECK (ArmsID >= 0),
                        @ArmsSlot1ID,-- INTEGER NOT NULL CHECK (ArmsSlot1ID >= 0),
                        @ArmsSlot2ID,-- INTEGER NOT NULL CHECK (ArmsSlot2ID >= 0),
                        @ArmsSlot3ID,-- INTEGER NOT NULL CHECK (ArmsSlot3ID >= 0),
                        @WaistID,-- INTEGER NOT NULL CHECK (WaistID >= 0),
                        @WaistSlot1ID,-- INTEGER NOT NULL CHECK (WaistSlot1ID >= 0),
                        @WaistSlot2ID,-- INTEGER NOT NULL CHECK (WaistSlot2ID >= 0),
                        @WaistSlot3ID,-- INTEGER NOT NULL CHECK (WaistSlot3ID >= 0),
                        @LegsID,-- INTEGER NOT NULL CHECK (LegsID >= 0),
                        @LegsSlot1ID,-- INTEGER NOT NULL CHECK (LegsSlot1ID >= 0),
                        @LegsSlot2ID,-- INTEGER NOT NULL CHECK (LegsSlot2ID >= 0),
                        @LegsSlot3ID,-- INTEGER NOT NULL CHECK (LegsSlot3ID >= 0),
                        @Cuff1ID,-- INTEGER NOT NULL CHECK (Cuff1ID >= 0),
                        @Cuff2ID,-- INTEGER NOT NULL CHECK (Cuff2ID >= 0),
                        @ZenithSkillsID,-- INTEGER NOT NULL,
                        @AutomaticSkillsID,-- INTEGER NOT NULL,
                        @ActiveSkillsID,-- INTEGER NOT NULL,
                        @CaravanSkillsID,-- INTEGER NOT NULL,
                        @DivaSkillID,-- INTEGER NOT NULL,
                        @GuildFoodID,-- INTEGER NOT NULL,
                        @StyleRankSkillsID,-- INTEGER NOT NULL,
                        @PlayerInventoryID,-- INTEGER NOT NULL,
                        @AmmoPouchID,-- INTEGER NOT NULL,
                        @PoogieItemID,-- INTEGER NOT NULL,
                        @RoadDureSkillsID,-- INTEGER NOT NULL,
                        @PlayerInventoryDictionary,-- TEXT NOT NULL,
                        @PlayerAmmoPouchDictionary,-- TEXT NOT NULL,
                        @PartnyaBagDictionary-- TEXT NOT NULL,
                        )";

                        string playerGearData = string.Format(
                            "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}{33}{34}{35}{36}{37}{38}{39}{40}{41}{42}{43}{44}{45}{46}{47}{48}{49}",
                            createdAt, createdBy, runID, playerID, 
                            gearName, styleID, weaponIconID, weaponClassID,
                            weaponTypeID, blademasterWeaponID, gunnerWeaponID, weaponSlot1,
                            weaponSlot2, weaponSlot3, headID, headSlot1, 
                            headSlot2, headSlot3, chestID, chestSlot1, 
                            chestSlot2, chestSlot3, armsID, armsSlot1, 
                            armsSlot2, armsSlot3, waistID, waistSlot1, 
                            waistSlot2, waistSlot3, legsID, legsSlot1, 
                            legsSlot2, legsSlot3, cuffSlot1, cuffSlot2, 
                            zenithSkillsID, automaticSkillsID, activeSkillsID, caravanSkillsID, 
                            divaSkillID, guildFoodID, styleRankSkillsID, playerInventoryID, 
                            ammoPouchID, poogieItemID, roadDureSkillsID, playerInventoryDictionary,
                            playerAmmoPouchDictionary, partnyaBagDictionary
                            );
                        string playerGearHash = CalculateStringHash(playerGearData);

                        using (SQLiteCommand cmd = new SQLiteCommand(insertSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@PlayerGearHash", playerGearHash);
                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@PlayerID", playerID);
                            cmd.Parameters.AddWithValue("@GearName", gearName);
                            cmd.Parameters.AddWithValue("@StyleID", styleID);
                            cmd.Parameters.AddWithValue("@WeaponIconID", weaponIconID);
                            cmd.Parameters.AddWithValue("@WeaponClassID", weaponClassID);
                            cmd.Parameters.AddWithValue("@WeaponTypeID", weaponTypeID);
                            if (blademasterWeaponID == null)
                            {
                                cmd.Parameters.AddWithValue("@BlademasterWeaponID", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@BlademasterWeaponID", blademasterWeaponID);
                            }
                            if (gunnerWeaponID == null)
                            {
                                cmd.Parameters.AddWithValue("@GunnerWeaponID", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@GunnerWeaponID", gunnerWeaponID);
                            }
                            cmd.Parameters.AddWithValue("@WeaponSlot1", weaponSlot1);
                            cmd.Parameters.AddWithValue("@WeaponSlot2", weaponSlot2);
                            cmd.Parameters.AddWithValue("@WeaponSlot3", weaponSlot3);
                            cmd.Parameters.AddWithValue("@HeadID", headID);
                            cmd.Parameters.AddWithValue("@HeadSlot1ID", headSlot1);
                            cmd.Parameters.AddWithValue("@HeadSlot2ID", headSlot2);
                            cmd.Parameters.AddWithValue("@HeadSlot3ID", headSlot3);
                            cmd.Parameters.AddWithValue("@ChestID", chestID);
                            cmd.Parameters.AddWithValue("@ChestSlot1ID", chestSlot1);
                            cmd.Parameters.AddWithValue("@ChestSlot2ID", chestSlot2);
                            cmd.Parameters.AddWithValue("@ChestSlot3ID", chestSlot3);
                            cmd.Parameters.AddWithValue("@ArmsID", armsID);
                            cmd.Parameters.AddWithValue("@ArmsSlot1ID", armsSlot1);
                            cmd.Parameters.AddWithValue("@ArmsSlot2ID", armsSlot2);
                            cmd.Parameters.AddWithValue("@ArmsSlot3ID", armsSlot3);
                            cmd.Parameters.AddWithValue("@WaistID", waistID);
                            cmd.Parameters.AddWithValue("@WaistSlot1ID", waistSlot1);
                            cmd.Parameters.AddWithValue("@WaistSlot2ID", waistSlot2);
                            cmd.Parameters.AddWithValue("@WaistSlot3ID", waistSlot3);
                            cmd.Parameters.AddWithValue("@LegsID", legsID);
                            cmd.Parameters.AddWithValue("@LegsSlot1ID", legsSlot1);
                            cmd.Parameters.AddWithValue("@LegsSlot2ID", legsSlot2);
                            cmd.Parameters.AddWithValue("@LegsSlot3ID", legsSlot3);
                            cmd.Parameters.AddWithValue("@Cuff1ID", cuffSlot1);
                            cmd.Parameters.AddWithValue("@Cuff2ID", cuffSlot2);
                            cmd.Parameters.AddWithValue("@ZenithSkillsID", zenithSkillsID);
                            cmd.Parameters.AddWithValue("@AutomaticSkillsID", automaticSkillsID);
                            cmd.Parameters.AddWithValue("@ActiveSkillsID", activeSkillsID);
                            cmd.Parameters.AddWithValue("@CaravanSkillsID", caravanSkillsID);
                            cmd.Parameters.AddWithValue("@DivaSkillID", divaSkillID);
                            cmd.Parameters.AddWithValue("@GuildFoodID", guildFoodID);
                            cmd.Parameters.AddWithValue("@StyleRankSkillsID", styleRankSkillsID);
                            cmd.Parameters.AddWithValue("@PlayerInventoryID", playerInventoryID);
                            cmd.Parameters.AddWithValue("@AmmoPouchID", ammoPouchID);
                            cmd.Parameters.AddWithValue("@PoogieItemID", poogieItemID);
                            cmd.Parameters.AddWithValue("@RoadDureSkillsID", roadDureSkillsID);
                            cmd.Parameters.AddWithValue("@PlayerInventoryDictionary", JsonConvert.SerializeObject(playerInventoryDictionary));
                            cmd.Parameters.AddWithValue("@PlayerAmmoPouchDictionary", JsonConvert.SerializeObject(playerAmmoPouchDictionary));
                            cmd.Parameters.AddWithValue("@PartnyaBagDictionary", JsonConvert.SerializeObject(partnyaBagDictionary));

                            // Execute the stored procedure
                            cmd.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (SQLiteException ex)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                        // Handle a SQL exception
                        MessageBox.Show("An error occurred while accessing the database: " + ex.SqlState + "\n\n" + ex.HelpLink + "\n\n" + ex.ResultCode + "\n\n" + ex.ErrorCode + "\n\n" + ex.Source + "\n\n" + ex.StackTrace + "\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (IOException ex)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                        // Handle an I/O exception
                        MessageBox.Show("An error occurred while accessing a file: " + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (ArgumentException ex)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                        MessageBox.Show("ArgumentException " + ex.ParamName + "\n\n" + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        HandleError(transaction, ex);
                    }
                }
            }
        }

        private void CreateDatabaseTriggers(SQLiteConnection conn)
        {
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // quests
                    // datfolder
                    // zenithskills
                    // automaticskills
                    // activeskills
                    // caravanskills
                    // stylerankskills
                    // playerinventory
                    // ammopouch
                    // roaddureskills
                    // playergear 
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_audit_deletion
                        AFTER DELETE ON Audit
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_audit_updates
                        AFTER UPDATE ON Audit
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_game_folder_updates
                        AFTER UPDATE ON GameFolder
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_game_folder_deletion
                        AFTER DELETE ON GameFolder
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_zenith_skills_deletion
                        AFTER DELETE ON ZenithSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_zenith_skills_updates
                        AFTER UPDATE ON ZenithSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_automatic_skills_deletion
                        AFTER DELETE ON AutomaticSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_automatic_skills_updates
                        AFTER UPDATE ON AutomaticSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_active_skills_deletion
                        AFTER DELETE ON ActiveSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_active_skills_updates
                        AFTER UPDATE ON ActiveSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_caravan_skills_deletion
                        AFTER DELETE ON CaravanSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_caravan_skills_updates
                        AFTER UPDATE ON CaravanSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_style_rank_skills_deletion
                        AFTER DELETE ON StyleRankSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_style_rank_skills_updates
                        AFTER UPDATE ON StyleRankSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_player_inventory_deletion
                        AFTER DELETE ON PlayerInventory
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_player_inventory_updates
                        AFTER UPDATE ON PlayerInventory
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_ammo_pouch_deletion
                        AFTER DELETE ON AmmoPouch
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_ammo_pouch_updates
                        AFTER UPDATE ON AmmoPouch
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_road_dure_skills_deletion
                        AFTER DELETE ON RoadDureSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_road_dure_skills_updates
                        AFTER UPDATE ON RoadDureSkills
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_player_gear_updates
                        AFTER UPDATE ON PlayerGear
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_player_gear_deletion
                        AFTER DELETE ON PlayerGear
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        //TODO: add playergear, datfolder, zenithskills, automaticskills, activeskills, playerinventory, roaddureskills, etc

                        // Create the trigger
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS trigger_insert_quests_insert_audit 
                        AFTER INSERT ON Quests
                        BEGIN
                            INSERT INTO Audit (
                                CreatedAt, 
                                CreatedBy,
                                ChangeType,
                                HashValue
                            ) VALUES (
                                new.CreatedAt, 
                                new.CreatedBy,
                                'INSERT',
                                new.QuestHash
                            );
                        END;";

                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        // Create the trigger
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS trigger_update_quests_insert_audit 
                        AFTER UPDATE ON Quests
                        BEGIN
                            INSERT INTO Audit (
                                CreatedAt, 
                                CreatedBy,
                                ChangeType,
                                HashValue
                            ) VALUES (
                                new.CreatedAt, 
                                new.CreatedBy,
                                'UPDATE',
                                new.QuestHash
                            );
                        END;";

                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        // Create the trigger
                        // TODO this doesnt work
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_quest_deletion
                        BEFORE DELETE ON Quests
                        FOR EACH ROW
                        BEGIN
                            INSERT INTO Audit (
                                CreatedAt, 
                                CreatedBy,
                                ChangeType,
                                HashValue
                            ) VALUES (
                                datetime('now'), 
                                OLD.CreatedBy,
                                'DELETE',
                                OLD.QuestHash
                            );
                            SELECT RAISE(ROLLBACK, 'Deleting rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;
                        ";

                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS trigger_update_quests_check_integrity
                        BEFORE UPDATE ON Quests
                        BEGIN
                            SELECT
                                CASE
                                    WHEN (SELECT QuestHash FROM Quests WHERE RunID = NEW.RunID) != NEW.QuestHash THEN
                                        RAISE(ABORT, 'Data integrity check failed')
                                END;
                        END;";

                        cmd.ExecuteNonQuery();
                    }

                    // Commit the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    HandleError(transaction, ex);
                }
            }
        }

        private void CreateDatabaseIndexes(SQLiteConnection conn)
        {
            List<string> createIndexSqlStatements = new List<string>
            {
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_activeskills_runid ON ActiveSkills(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allarmorskills_armorskillid ON AllArmorSkills(ArmorSkillID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allcaravanskills_caravanskillid ON AllCaravanSkills(CaravanSkillID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allroaddureskills_roaddureskillid ON AllRoadDureSkills(RoadDureSkillID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allstylerankskills_stylerankskillid ON AllStyleRankSkills(StyleRankSkillID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allzenithskills_zenithskillid ON AllZenithSkills(ZenithSkillID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_ammopouch_runid ON AmmoPouch(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_area_areaid ON Area(AreaID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_automaticskills_runid ON AutomaticSkills(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_caravanskills_runid ON CaravanSkills(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_item_itemid ON Item(ItemID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_objectivetype_objectivetypeid ON ObjectiveType(ObjectiveTypeID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_playergear_runid ON PlayerGear(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_playerinventory_runid ON PlayerInventory(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_players_playerid ON Players(PlayerID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_questname_questnameid ON QuestName(QuestNameID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_quests_runid ON Quests(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_rankname_ranknameid ON RankName(RankNameID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_roaddureskills_runid ON RoadDureSkills(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_stylerankskills_runid ON StyleRankSkills(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_weapontype_weapontypeid ON WeaponType(WeaponTypeID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_zenithskills_runid ON ZenithSkills(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allblademasterweapons_blademasterweaponid ON AllBlademasterWeapons(BlademasterWeaponID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allgunnerweapons_gunnerweaponid ON AllGunnerWeapons(GunnerWeaponID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allheadpieces_headpieceid ON AllHeadPieces(HeadPieceID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allchestpieces_chestpieceid ON AllChestPieces(ChestPieceID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allarmspieces_armspieceid ON AllArmsPieces(ArmsPieceID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_allwaistpieces_waistpieceid ON AllWaistPieces(WaistPieceID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_alllegspieces_legspieceid ON AllLegsPieces(LegsPieceID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_questevents_runid ON QuestEvents(RunID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_weaponclass_weaponclassid ON WeaponClass(WeaponClassID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_weaponicon_weaponiconid ON WeaponIcon(WeaponIconID)",
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_weaponstyles_styleid ON WeaponStyles(StyleID)"

            };

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    foreach (string createIndexSql in createIndexSqlStatements)
                    {
                        using (var cmd = new SQLiteCommand(createIndexSql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    HandleError(transaction, ex);
                }
            }
        }

        private void HandleError(SQLiteTransaction? transaction, Exception ex)
        {
            // Roll back the transaction
            if (transaction != null)
                transaction.Rollback();

            // Handle the exception and show an error message to the user
            MessageBox.Show("An error occurred: " + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void MakeDeserealizedQuestInfoDictionariesFromRunID(SQLiteConnection conn, DataLoader dataLoader, int runID)
        {
            //To retrieve the dictionaries from the database, you can use the following code:
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = @"SELECT
                           AttackBuffDictionary,
                           HitCountDictionary,
                           DamageDealtDictionary,
                           DamagePerSecondDictionary,
                           AreaChangesDictionary,
                           CartsDictionary,
                           Monster1HPDictionary,
                           Monster2HPDictionary,
                           Monster3HPDictionary,
                           Monster4HPDictionary
                           FROM Quests WHERE RunID = @RunID";

                cmd.Parameters.AddWithValue("@RunID", runID);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string attackBuffDictionaryJson = reader.GetString(0);
                        string hitCountDictionaryJson = reader.GetString(1);
                        string damageDealtDictionaryJson = reader.GetString(2);
                        string damagePerSecondDictionaryJson = reader.GetString(3);
                        string areaChangesDictionaryJson = reader.GetString(4);
                        string cartsDictionaryJson = reader.GetString(5);
                        string monster1HPDictionaryJson = reader.GetString(6);
                        string monster2HPDictionaryJson = reader.GetString(7);
                        string monster3HPDictionaryJson = reader.GetString(8);
                        string monster4HPDictionaryJson = reader.GetString(9);

                        dataLoader.model.attackBuffDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, int>>(attackBuffDictionaryJson);
                        dataLoader.model.hitCountDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, int>>(hitCountDictionaryJson);
                        dataLoader.model.damageDealtDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, int>>(damageDealtDictionaryJson);
                        dataLoader.model.damagePerSecondDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, double>>(damagePerSecondDictionaryJson);
                        dataLoader.model.areaChangesDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, int>>(areaChangesDictionaryJson);
                        dataLoader.model.cartsDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartsDictionaryJson);
                        dataLoader.model.monster1HPDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(monster1HPDictionaryJson);
                        dataLoader.model.monster2HPDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(monster2HPDictionaryJson);
                        dataLoader.model.monster3HPDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(monster3HPDictionaryJson);
                        dataLoader.model.monster4HPDictionaryDeserealized = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(monster4HPDictionaryJson);
                    }
                }
            }
        }

        #region session time

        public void StoreSessionTime(MainWindow window)
        {
            try
            {
                var model = window.DataLoader.model;
                DateTime ProgramEnd = DateTime.Now;
                DateTime ProgramStart = window.ProgramStart;
                TimeSpan duration = ProgramEnd - ProgramStart;
                int sessionDuration = (int)duration.TotalSeconds;

                // Connect to the database
                string dbFilePath = _connectionString;
                string connectionString = "Data Source=" + dbFilePath;
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Begin a transaction
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Create the INSERT command
                            string insertSql = "INSERT INTO Session (StartTime, EndTime, SessionDuration) VALUES (@startTime, @endTime, @sessionDuration)";
                            using (SQLiteCommand insertCommand = new SQLiteCommand(insertSql, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@startTime", ProgramStart);
                                insertCommand.Parameters.AddWithValue("@endTime", ProgramEnd);
                                insertCommand.Parameters.AddWithValue("@sessionDuration", sessionDuration);
                                // Execute the INSERT statement
                                insertCommand.ExecuteNonQuery();
                            }
                            // Commit the transaction
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            HandleError(transaction, ex);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                // Handle a SQL exception
                MessageBox.Show("An error occurred while accessing the database: " + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                // Handle an I/O exception
                MessageBox.Show("An error occurred while accessing a file: " + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Handle any other exception
                MessageBox.Show("An error occurred: " + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion

        private readonly List<string> _validTableNames = new List<string> {
            "RankName",
            "ObjectiveType",
            "QuestName",
            "WeaponType",
            "Item",
            "Area",
            "AllZenithSkills",
            "AllArmorSkills",
            "AllCaravanSkills",
            "AllStyleRankSkills",
            "AllRoadDureSkills",
            "AllBlademasterWeapons",
            "AllGunnerWeapons",
            "AllHeadPieces",
            "AllChestPieces",
            "AllArmsPieces",
            "AllWaistPieces",
            "AllLegsPieces",
            "WeaponClass",
            "WeaponIcon",
            "WeaponStyles",
            "AllDivaSkills"};

        private Dictionary<string, Dictionary<string, object>> CreateReferenceSchemaJSONFromLocalDatabaseFile(SQLiteConnection conn, bool writeFile = true)
        {
            // Create a dictionary to store the reference schema
            var schema = new Dictionary<string, Dictionary<string, object>>();

            // Start a transaction
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        // Query the database to get information about the tables, triggers, and indexes
                        cmd.CommandText = "SELECT name, type, tbl_name, sql FROM sqlite_master WHERE type IN ('table', 'trigger', 'index')";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Get the name and type of the object
                                var objectName = reader["name"].ToString();
                                var objectType = reader["type"].ToString();

                                // Check the type of the object
                                if (objectType == "table")
                                {
                                    // Set the table name
                                    var tableName = objectName;

                                    // Query the database to get information about the columns in the table
                                    using (var command2 = conn.CreateCommand())
                                    {
                                        command2.CommandText = $"PRAGMA table_info({objectName})";

                                        using (var reader2 = command2.ExecuteReader())
                                        {
                                            // Create a list to store the column information
                                            var columns = new List<object>();

                                            // Iterate through the columns in the table
                                            while (reader2.Read())
                                            {
                                                // Get the name and data type of the column
                                                var columnName = reader2["name"].ToString();
                                                var columnType = reader2["type"].ToString();

                                                // Add the column information to the list
                                                columns.Add(new { name = columnName, type = columnType });
                                            }

                                            // Initialize the schema entry for the table if it doesn't exist
                                            if (!schema.ContainsKey(tableName))
                                            {
                                                schema[tableName] = new Dictionary<string, object>();
                                            }
                                            // Add the list of columns to the schema dictionary
                                            schema[tableName]["columns"] = columns;
                                        }
                                    }
                                }
                                else if (objectType == "index")
                                {
                                    // Set the table name
                                    using (var cmd2 = conn.CreateCommand())
                                    {
                                        cmd2.CommandText = $"SELECT tbl_name FROM sqlite_master WHERE name='{objectName}'";
                                        var tableName = cmd2.ExecuteScalar().ToString();

                                        // Initialize the schema entry for the table if it doesn't exist
                                        if (!schema.ContainsKey(tableName))
                                        {
                                            schema[tableName] = new Dictionary<string, object>();
                                        }

                                        // Add the index information to the schema dictionary
                                        schema[tableName]["indexes"] = schema[tableName].ContainsKey("indexes")
                                            ? schema[tableName]["indexes"]
                                        : new List<object>();
                                        ((List<object>)schema[tableName]["indexes"]).Add(new { name = objectName, sql = reader["sql"].ToString() });
                                    }
                                }
                                else if (objectType == "trigger")
                                {
                                    // Set the table name
                                    using (var cmd3 = conn.CreateCommand())
                                    {
                                        cmd3.CommandText = $"SELECT tbl_name FROM sqlite_master WHERE name='{objectName}'";
                                        var tableName = cmd3.ExecuteScalar().ToString();

                                        // Initialize the schema entry for the table if it doesn't exist
                                        if (!schema.ContainsKey(tableName))
                                        {
                                            schema[tableName] = new Dictionary<string, object>();
                                        }

                                        // Add the trigger information to the schema dictionary
                                        schema[tableName]["triggers"] = schema[tableName].ContainsKey("triggers")
                                            ? schema[tableName]["triggers"] as List<object>
                                            : new List<object>();

                                        (schema[tableName]["triggers"] as List<object>).Add(new { name = objectName, sql = reader["sql"].ToString() });
                                    }
                                }
                            }
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    HandleError(transaction, ex);
                }
            }

            // Check if the user requested to write the file
            if (writeFile)
            {
                // Serialize the schema dictionary to a JSON string
                var json = JsonConvert.SerializeObject(schema, Formatting.Indented);

                // Write the JSON string to the reference schema file
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\reference_schema.json"), json);
            }

            return schema;
        }

        public bool schemaChanged = false;

        public bool CompareDictionaries(Dictionary<string, Dictionary<string, object>> dict1, Dictionary<string, Dictionary<string, object>> dict2)
        {
            // Check if the number of tables is different
            if (dict1.Count != dict2.Count)
            {
                return false;
            }

            // Iterate through the tables in the first dictionary
            foreach (var table1 in dict1)
            {
                // Get the name of the table
                var tableName = table1.Key;

                // Check if the table exists in the second dictionary
                if (!dict2.ContainsKey(tableName))
                {
                    return false;
                }

                var tableData1 = table1.Value;
                var tableData2 = dict2[tableName];

                // Check if the number of key-value pairs in the table is different
                if (tableData1.Count != tableData2.Count)
                {
                    return false;
                }

                // Iterate through the key-value pairs in the table
                foreach (var data1 in tableData1)
                {
                    var dataName = data1.Key;
                    var dataValue1 = data1.Value;

                    // Check if the key exists in the second dictionary
                    if (!tableData2.ContainsKey(dataName))
                    {
                        return false;
                    }

                    var dataValue2 = tableData2[dataName];

                    // Get the JSON representations of the values
                    var json1 = JsonConvert.SerializeObject(dataValue1);
                    var json2 = JsonConvert.SerializeObject(dataValue2);

                    // Check if the JSON representations are equal
                    if (json1 != json2)
                    {
                        return false;
                    }
                }
            }

            // If all checks pass, the dictionaries are equal
            return true;
        }

        //TODO: Test
        private bool CompareDatabaseSchemas(Dictionary<string, Dictionary<string, object>> referenceSchema, Dictionary<string, Dictionary<string, object>> currentSchema)
        {
    
            if (!CompareDictionaries(referenceSchema, currentSchema))
            { 
                schemaChanged = true;
            }

            // Check if the schema has changed
            if (schemaChanged)
            {
                Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
                MessageBox.Show(@"ERROR: The database schema got updated in the latest version. Please make sure that both MHFZ_Overlay.sqlite and reference_schema.json don't exist in the current overlay directory, so that the program can make new ones",
                "Monster Hunter Frontier Z Overlay", MessageBoxButton.OK, MessageBoxImage.Error);
                s.EnableQuestLogging = false;
            }

            return schemaChanged;
        }

        private void InsertPlayerDictionaryDataIntoTable(SQLiteConnection conn)
        {
            // Start a transaction
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    DateTime startTime = DateTime.UnixEpoch;

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"SELECT StartTime FROM Session WHERE SessionID = 1";
                        var result = cmd.ExecuteScalar();
                        if (result != null && result.ToString() != "")
                            startTime = Convert.ToDateTime(result);
                        else
                            startTime = DateTime.MinValue;
                    }

                    // Create a command that will be used to insert multiple rows in a batch
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        // Set the command text to insert a single row
                        cmd.CommandText = @"INSERT OR REPLACE INTO Players (
                        PlayerID, 
                        PlayerAvatar, 
                        CreationDate,
                        PlayerName,
                        GuildName,
                        ServerName,
                        Gender,
                        Nationality
                        ) VALUES (
                        @PlayerID, 
                        @PlayerAvatar,
                        @CreationDate,
                        @PlayerName,
                        @GuildName,
                        @ServerName,
                        @Gender,
                        @Nationality)";

                        // Add the parameter placeholders
                        cmd.Parameters.Add("@PlayerID", DbType.Int32);
                        cmd.Parameters.Add("@PlayerAvatar", DbType.String);
                        cmd.Parameters.Add("@CreationDate", DbType.String);
                        cmd.Parameters.Add("@PlayerName", DbType.String);
                        cmd.Parameters.Add("@GuildName", DbType.String);
                        cmd.Parameters.Add("@ServerName", DbType.String);
                        cmd.Parameters.Add("@Gender", DbType.String);
                        cmd.Parameters.Add("@Nationality", DbType.String);

                        // Iterate through the list of players
                        foreach (KeyValuePair<int, List<string>> kvp in PlayersList.PlayerIDs)
                        {
                            int playerID = kvp.Key;
                            List<string> playerInfo = kvp.Value;
                            string playerAvatar = playerInfo[0];
                            string creationDate = playerInfo[1];
                            string playerName = playerInfo[2];
                            string guildName = playerInfo[3];
                            string serverName = playerInfo[4];
                            string gender = playerInfo[5];
                            string nationality = playerInfo[6];

                            if (playerID == 1 && (startTime == DateTime.UnixEpoch || startTime == DateTime.MinValue))
                                creationDate = DateTime.Now.Date.ToString();
                            else
                                creationDate = startTime.Date.ToString();

                            if (playerID == 1)
                            {
                                Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
                                playerAvatar = s.PlayerAvatarLink;
                                playerName = s.HunterName;
                                guildName = s.GuildName;
                                serverName = s.ServerName;
                                gender = s.GenderExport;
                                nationality = s.PlayerNationality;
                            }

                            // Set the parameter values
                            cmd.Parameters["@PlayerID"].Value = playerID;
                            cmd.Parameters["@PlayerAvatar"].Value = playerAvatar;
                            cmd.Parameters["@CreationDate"].Value = creationDate;
                            cmd.Parameters["@PlayerName"].Value = playerName;
                            cmd.Parameters["@GuildName"].Value = guildName;
                            cmd.Parameters["@ServerName"].Value = serverName;
                            cmd.Parameters["@Gender"].Value = gender;
                            cmd.Parameters["@Nationality"].Value = nationality;

                            // Execute the SQL statement
                            cmd.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    HandleError(transaction, ex);
                }
            }
        }

        private void InsertDictionaryDataIntoTable(IReadOnlyDictionary<int, string> dictionary, string tableName, string idColumn, string valueColumn, SQLiteConnection conn)
        {
            // Start a transaction
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    // Validate the input table name
                    if (!_validTableNames.Contains(tableName))
                    {
                        throw new ArgumentException($"Invalid table name: {tableName}");
                    }

                    // Validate the input parameters
                    if (dictionary == null || dictionary.Count == 0)
                    {
                        throw new ArgumentException($"Invalid dictionary: {dictionary}");
                    }

                    if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(idColumn) || string.IsNullOrEmpty(valueColumn))
                    {
                        throw new ArgumentException("Invalid table name, id column, or value column");
                    }
                    if (conn == null)
                    {
                        throw new ArgumentException("Invalid connection");
                    }
                    if (conn.State != ConnectionState.Open)
                    {
                        throw new InvalidOperationException("Connection is not open");
                    }

                    // Create a command that will be used to insert multiple rows in a batch
                    using (var cmd = new SQLiteCommand(conn))
                    {
                        // Set the command text to insert a single row
                        cmd.CommandText = $"INSERT OR REPLACE INTO {tableName} ({idColumn}, {valueColumn}) VALUES (@id, @value)";

                        // Create a parameter for the value to be inserted
                        var valueParam = cmd.CreateParameter();
                        valueParam.ParameterName = "@value";
                        cmd.Parameters.Add(valueParam);

                        // Create a parameter for the ID to be inserted
                        var idParam = cmd.CreateParameter();
                        idParam.ParameterName = "@id";
                        cmd.Parameters.Add(idParam);

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
                catch (Exception ex)
                {
                    HandleError(transaction, ex); 
                }
            }
        }

        public void UpdateYoutubeLink(string youtubeId, int runId, SQLiteConnection conn)
        {
            try
            {
                // Start a transaction
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Execute the query
                        string updateSql = "UPDATE Quests SET YouTubeID = @youtubeId WHERE RunID = @runId";
                        using (SQLiteCommand cmd = new SQLiteCommand(updateSql, conn))
                        {
                            // Add the parameters for the placeholders in the SQL query
                            cmd.Parameters.AddWithValue("@youtubeId", youtubeId);
                            cmd.Parameters.AddWithValue("@runId", runId);

                            // Execute the update statement
                            int rowsAffected = cmd.ExecuteNonQuery();

                            // Check if the update was successful
                            if (rowsAffected > 0)
                            {
                                // The update was successful
                                MessageBox.Show("YouTubeID updated successfully");
                            }
                            else
                            {
                                // The update was not successful
                                MessageBox.Show("Error updating YouTubeID");
                            }
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Handle the error
                        HandleError(transaction, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur when starting the transaction
                HandleError(null, ex);
            }
        }

        private void CreateDatabaseTables(SQLiteConnection conn, DataLoader dataLoader)
        {
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    var model = dataLoader.model;

                    // Create table to store program usage time
                    string sql = @"CREATE TABLE IF NOT EXISTS Session (
                    SessionID INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime DATETIME NOT NULL,
                    EndTime DATETIME NOT NULL,
                    SessionDuration INTEGER NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS Audit(
                    AuditID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    ChangeType TEXT NOT NULL,
                    HashValue TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    //Create the Quests table
                    sql = @"CREATE TABLE IF NOT EXISTS Quests 
                    (
                    QuestHash TEXT NOT NULL,
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    RunID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    QuestID INTEGER NOT NULL CHECK (QuestID >= 0), 
                    FinalTimeValue INTEGER NOT NULL,
                    FinalTimeDisplay TEXT NOT NULL, 
                    ObjectiveImage TEXT NOT NULL,
                    ObjectiveTypeID INTEGER NOT NULL CHECK (ObjectiveTypeID >= 0), 
                    ObjectiveQuantity INTEGER NOT NULL, 
                    StarGrade INTEGER NOT NULL, 
                    RankName TEXT NOT NULL, 
                    ObjectiveName TEXT NOT NULL, 
                    Date DATETIME NOT NULL,
                    YouTubeID TEXT DEFAULT 'dQw4w9WgXcQ', -- default value for YouTubeID is a Rick Roll video
                    -- DpsData TEXT NOT NULL,
                    AttackBuffDictionary TEXT NOT NULL,
                    HitCountDictionary TEXT NOT NULL,
                    HitsPerSecondDictionary TEXT NOT NULL,
                    DamageDealtDictionary TEXT NOT NULL,
                    DamagePerSecondDictionary TEXT NOT NULL,
                    AreaChangesDictionary TEXT NOT NULL,
                    CartsDictionary TEXT NOT NULL,
                    Monster1HPDictionary TEXT NOT NULL,
                    Monster2HPDictionary TEXT NOT NULL,
                    Monster3HPDictionary TEXT NOT NULL,
                    Monster4HPDictionary TEXT NOT NULL,
                    HitsTakenBlockedDictionary TEXT NOT NULL,
                    HitsTakenBlockedPerSecondDictionary TEXT NOT NULL,
                    PlayerHPDictionary TEXT NOT NULL,
                    PlayerStaminaDictionary TEXT NOT NULL,
                    KeystrokesDictionary TEXT NOT NULL,
                    MouseInputDictionary TEXT NOT NULL,
                    GamepadInputDictionary TEXT NOT NULL,
                    ActionsPerMinuteDictionary TEXT NOT NULL,
                    PartySize INTEGER NOT NULL,
                    OverlayMode TEXT NOT NULL,
                    FOREIGN KEY(QuestID) REFERENCES QuestName(QuestNameID),
                    FOREIGN KEY(ObjectiveTypeID) REFERENCES ObjectiveType(ObjectiveTypeID)
                    -- FOREIGN KEY(RankNameID) REFERENCES RankName(RankNameID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    /*
                     * Here's an example of how you could store the dps data as a JSON string in a single column in the "Quests" table:

                        1. Add a new column to the "Quests" table to store the dps data as a JSON string. You could use a data type like "TEXT" or "BLOB" for this column.

                        2. When a quest finishes, generate an array or list of dps values recorded during the quest run.

                        3. Use a JSON serialization library (such as Newtonsoft.Json in C#) to convert the array or list of dps values into a JSON string.

                        4. Insert the JSON string into the new column in the "Quests" table, along with the other quest run data (such as the QuestID, AreaID, etc.).

                        To extract the dps data and plot it in the chart later, you would do the following:

                        1. Retrieve the JSON string from the "Quests" table for the specific quest run you want to display.

                        2. Use a JSON parsing library (such as Newtonsoft.Json in C#) to deserialize the JSON string into an array or list of dps values.

                        3. Use the dps values and a charting library (such as OxyPlot or LiveCharts) to plot the dps data in a chart.

                        Note that this approach assumes that the dps data is recorded and stored consistently for each quest run. You may need to do additional processing or validation on the data to ensure that it is in a suitable format for charting.
                     */

                    /*
                     Quest Events log example:

                        00:00.00 Start at zone X
                        00:10.00 Changed to zone X
                        00:15.00 First hit towards monster
                        00:20.00 Maximum attack buff obtained is now 2850
                        00:25.00 Reached 67 Hits towards monster
                        00:27.00 Maximum attack buff obtained is now 3060
                        00:30.00 Hit by monster
                        00:40.00 Changed to zone X
                        00:50.00 Carted at zone X
                        01:10.33 Changed to zone X (Basecamp ig)
                        02:00.30 Monster is now at 90% HP
                        ...
                        35:34.27 Monster is now at 10% HP
                        40:00.00 Completed Quest
                     */

                    // TODO
                    sql = @"CREATE TABLE IF NOT EXISTS QuestEvents(
                      EventID INTEGER PRIMARY KEY AUTOINCREMENT,
                      RunID INTEGER NOT NULL, -- foreign key to the Quests table
                      EventType TEXT NOT NULL, -- type of event, e.g. Start, Hit, Cart, etc.
                      TimeValue INTEGER NOT NULL,
                      TimeDisplay TEXT NOT NULL, 
                      EventDetails TEXT NOT NULL, -- data for the event, e.g. zone X, 67 Hits. 
                      FOREIGN KEY(RunID) REFERENCES Quests(RunID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    //Create the RankNames table
                    sql = @"CREATE TABLE IF NOT EXISTS RankName
                    (RankNameID INTEGER PRIMARY KEY, 
                    RankNameName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.RanksBandsList.RankBandsID, "RankName", "RankNameID", "RankNameName", conn);

                    //Create the ObjectiveTypes table
                    sql = @"CREATE TABLE IF NOT EXISTS ObjectiveType
                    (ObjectiveTypeID INTEGER PRIMARY KEY, 
                    ObjectiveTypeName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.ObjectiveTypeList.ObjectiveTypeID, "ObjectiveType", "ObjectiveTypeID", "ObjectiveTypeName", conn);

                    //Create the QuestNames table
                    sql = @"CREATE TABLE IF NOT EXISTS QuestName
                    (QuestNameID INTEGER PRIMARY KEY, 
                    QuestNameName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Quests.QuestIDs, "QuestName", "QuestNameID", "QuestNameName", conn);

                    // Create the Players table
                    //do an UPDATE when inserting quests. since its just local player?
                    sql = @"
                    CREATE TABLE IF NOT EXISTS Players (
                    PlayerID INTEGER PRIMARY KEY, 
                    PlayerAvatar TEXT NOT NULL DEFAULT 'https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/icon/transcend.png',
                    CreationDate DATE NOT NULL,
                    PlayerName TEXT NOT NULL,
                    GuildName TEXT NOT NULL,
                    ServerName TEXT NOT NULL,
                    Gender TEXT NOT NULL,
                    Nationality TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertPlayerDictionaryDataIntoTable(conn);

                    /*
                     * mhfdat.bin
                        All shops' core inventory (exceptions are held in files mentioned below)
                        All equipment stats, names, model data, flags etc.
                        All item stats, names, icon data, colours, flags etc.
                        Carve tables
                        Caravan gem unlock levels
                        Caravan gem colour requirements
                        Raviente upgrade points
                    mhfemd.bin
                        All enemy related information, stats, movesets, size ranges etc.
                    mhfgao.bin
                        Cat NPC companion equipment, decorations, etc.
                    mhfinf.bin
                        Client side quest data for permanent quests such as subs required, descriptions, target icons etc.
                        Does not actually contain quest data at all.
                    mhfjmp.bin
                        Town and facility quick travel functionality
                        mhfmec.bin
                        Unknown.
                    mhfmfd.bin
                        Mezefest carnival game data, item store inventory and costs, etc.
                    mhfmsx.bin
                        Unknown, contains strings related to since removed Tower content.
                    mhfnav.bin
                        Hunter Navigation rewards, requirements, etc.
                    mhfpac.bin
                        Most menu strings, NPC interaction prompts etc.
                    mhfrcc.bin
                        Currently held event information, blurbs below icon for travelling to the NPCs.
                    mhfsch.bin
                        Unknown.
                    mhfsdt.bin
                        Unknown, has HC repeated internally a lot.
                    mhfsqd.bin
                        Diva defense related strings such as activating buffs and skills in second week of event.
                     */
                    sql = @"
                    CREATE TABLE IF NOT EXISTS GameFolder (
                    GameFolderHash TEXT NOT NULL,
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    GameFolderID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    GameFolderPath TEXT NOT NULL,
                    mhfdatHash TEXT NOT NULL,
                    mhfemdHash TEXT NOT NULL,
                    mhfinfHash TEXT NOT NULL,
                    mhfsqdHash TEXT NOT NULL,
                    mhfodllHash TEXT NOT NULL,
                    mhfohddllHash TEXT NOT NULL,
                    FOREIGN KEY(RunID) REFERENCES Quests(RunID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Create the WeaponTypes table
                    sql = @"CREATE TABLE IF NOT EXISTS WeaponType (
                    WeaponTypeID INTEGER PRIMARY KEY, 
                    WeaponTypeName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(WeaponTypes.WeaponTypeID, "WeaponType", "WeaponTypeID", "WeaponTypeName", conn);

                    // Create the Item table
                    sql = @"CREATE TABLE IF NOT EXISTS Item (
                    ItemID INTEGER PRIMARY KEY, 
                    ItemName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Items.ItemIDs, "Item", "ItemID", "ItemName", conn);

                    // Create the Area table
                    sql = @"CREATE TABLE IF NOT EXISTS Area (
                    AreaID INTEGER PRIMARY KEY,
                    AreaName TEXT NOT NULL,
                    AreaIcon TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Prepare the SQL statement
                    sql = @"INSERT OR REPLACE INTO Area (
                    AreaID, 
                    AreaIcon, 
                    AreaName
                    ) VALUES (
                    @AreaID,
                    @AreaIcon,
                    @AreaName)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        // Add the parameter placeholders
                        cmd.Parameters.Add("@AreaID", DbType.Int32);
                        cmd.Parameters.Add("@AreaIcon", DbType.String);
                        cmd.Parameters.Add("@AreaName", DbType.String);

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
                    }

                    // Create the PlayerGear table
                    sql = @"CREATE TABLE IF NOT EXISTS PlayerGear (
                    PlayerGearHash TEXT NOT NULL,
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    PlayerGearID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL, 
                    PlayerID INTEGER NOT NULL,
                    GearName TEXT NOT NULL,
                    StyleID INTEGER NOT NULL CHECK (StyleID >= 0),
                    WeaponIconID INTEGER NOT NULL,
                    WeaponClassID INTEGER NOT NULL,
                    WeaponTypeID INTEGER NOT NULL CHECK (WeaponTypeID >= 0),
                    BlademasterWeaponID INTEGER,
                    GunnerWeaponID INTEGER,
                    WeaponSlot1 TEXT NOT NULL,
                    WeaponSlot2 TEXT NOT NULL,
                    WeaponSlot3 TEXT NOT NULL,
                    HeadID INTEGER NOT NULL CHECK (HeadID >= 0), 
                    HeadSlot1ID INTEGER NOT NULL CHECK (HeadSlot1ID >= 0),
                    HeadSlot2ID INTEGER NOT NULL CHECK (HeadSlot2ID >= 0),
                    HeadSlot3ID INTEGER NOT NULL CHECK (HeadSlot3ID >= 0),
                    ChestID INTEGER NOT NULL CHECK (ChestID >= 0),
                    ChestSlot1ID INTEGER NOT NULL CHECK (ChestSlot1ID >= 0),
                    ChestSlot2ID INTEGER NOT NULL CHECK (ChestSlot2ID >= 0),
                    ChestSlot3ID INTEGER NOT NULL CHECK (ChestSlot3ID >= 0),
                    ArmsID INTEGER NOT NULL CHECK (ArmsID >= 0),
                    ArmsSlot1ID INTEGER NOT NULL CHECK (ArmsSlot1ID >= 0),
                    ArmsSlot2ID INTEGER NOT NULL CHECK (ArmsSlot2ID >= 0),
                    ArmsSlot3ID INTEGER NOT NULL CHECK (ArmsSlot3ID >= 0),
                    WaistID INTEGER NOT NULL CHECK (WaistID >= 0),
                    WaistSlot1ID INTEGER NOT NULL CHECK (WaistSlot1ID >= 0),
                    WaistSlot2ID INTEGER NOT NULL CHECK (WaistSlot2ID >= 0),
                    WaistSlot3ID INTEGER NOT NULL CHECK (WaistSlot3ID >= 0),
                    LegsID INTEGER NOT NULL CHECK (LegsID >= 0),
                    LegsSlot1ID INTEGER NOT NULL CHECK (LegsSlot1ID >= 0),
                    LegsSlot2ID INTEGER NOT NULL CHECK (LegsSlot2ID >= 0),
                    LegsSlot3ID INTEGER NOT NULL CHECK (LegsSlot3ID >= 0),
                    Cuff1ID INTEGER NOT NULL CHECK (Cuff1ID >= 0),
                    Cuff2ID INTEGER NOT NULL CHECK (Cuff2ID >= 0),
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
                    PlayerInventoryDictionary TEXT NOT NULL,
                    PlayerAmmoPouchDictionary TEXT NOT NULL,
                    PartnyaBagDictionary TEXT NOT NULL,
                    FOREIGN KEY(RunID) REFERENCES Quests(RunID),
                    FOREIGN KEY(PlayerID) REFERENCES Players(PlayerID),
                    FOREIGN KEY(StyleID) REFERENCES WeaponStyles(StyleID),
                    FOREIGN KEY(WeaponIconID) REFERENCES WeaponIcon(WeaponIconID),
                    FOREIGN KEY(WeaponClassID) REFERENCES WeaponClass(WeaponClassID),
                    FOREIGN KEY(WeaponTypeID) REFERENCES WeaponType(WeaponTypeID),
                    FOREIGN KEY(BlademasterWeaponID) REFERENCES AllBlademasterWeapons(BlademasterWeaponID),
                    FOREIGN KEY(GunnerWeaponID) REFERENCES AllGunnerWeapons(GunnerWeaponID),
                    CHECK 
                    (
                        (BlademasterWeaponID IS NOT NULL AND GunnerWeaponID IS NULL) OR 
                        (BlademasterWeaponID IS NULL AND GunnerWeaponID IS NOT NULL)
                    )
                    FOREIGN KEY(HeadID) REFERENCES AllHeadPieces(HeadPieceID),
                    FOREIGN KEY(ChestID) REFERENCES AllChestPieces(ChestPieceID),
                    FOREIGN KEY(ArmsID) REFERENCES AllArmsPieces(ArmsPieceID),
                    FOREIGN KEY(WaistID) REFERENCES AllWaistPieces(WaistPieceID),
                    FOREIGN KEY(LegsID) REFERENCES AllLegsPieces(LegsPieceID),
                    FOREIGN KEY(Cuff1ID) REFERENCES Item(ItemID),
                    FOREIGN KEY(Cuff2ID) REFERENCES Item(ItemID),
                    FOREIGN KEY(ZenithSkillsID) REFERENCES ZenithSkills(ZenithSkillsID),
                    FOREIGN KEY(AutomaticSkillsID) REFERENCES AutomaticSkills(AutomaticSkillsID),
                    FOREIGN KEY(ActiveSkillsID) REFERENCES ActiveSkills(ActiveSkillsID),
                    FOREIGN KEY(CaravanSkillsID) REFERENCES CaravanSkills(CaravanSkillsID),
                    FOREIGN KEY(DivaSkillID) REFERENCES AllDivaSkills(DivaSkillID),
                    FOREIGN KEY(GuildFoodID) REFERENCES AllArmorSkills(ArmorSkillID),
                    FOREIGN KEY(StyleRankSkillsID) REFERENCES StyleRankSkills(StyleRankSkillsID),
                    FOREIGN KEY(PlayerInventoryID) REFERENCES PlayerInventory(PlayerInventoryID),
                    FOREIGN KEY(AmmoPouchID) REFERENCES AmmoPouch(AmmoPouchID),
                    FOREIGN KEY(PoogieItemID) REFERENCES Item(ItemID),
                    FOREIGN KEY(RoadDureSkillsID) REFERENCES RoadDureSkills(RoadDureSkillsID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS AllDivaSkills (
                      DivaSkillID INTEGER PRIMARY KEY,
                      DivaSkillName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.DivaSkillList.DivaSkillID, "AllDivaSkills", "DivaSkillID", "DivaSkillName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS WeaponStyles (
                      StyleID INTEGER PRIMARY KEY,
                      StyleName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.WeaponStyles.WeaponStyleID, "WeaponStyles", "StyleID", "StyleName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS WeaponIcon (
                      WeaponIconID INTEGER PRIMARY KEY,
                      WeaponIconLink TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.WeaponIconsDictionary.WeaponIconID, "WeaponIcon", "WeaponIconID", "WeaponIconLink", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS WeaponClass (
                      WeaponClassID INTEGER PRIMARY KEY,
                      WeaponClassName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.WeaponClass.WeaponClassID, "WeaponClass", "WeaponClassID", "WeaponClassName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllBlademasterWeapons (
                      BlademasterWeaponID INTEGER PRIMARY KEY,
                      BlademasterWeaponName TEXT NOT NULL
                    )"; 
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.BlademasterWeapons.BlademasterWeaponIDs, "AllBlademasterWeapons", "BlademasterWeaponID", "BlademasterWeaponName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllGunnerWeapons (
                      GunnerWeaponID INTEGER PRIMARY KEY,
                      GunnerWeaponName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.GunnerWeapons.GunnerWeaponIDs, "AllGunnerWeapons", "GunnerWeaponID", "GunnerWeaponName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllHeadPieces (
                      HeadPieceID INTEGER PRIMARY KEY,
                      HeadPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.ArmorHeads.ArmorHeadIDs, "AllHeadPieces", "HeadPieceID", "HeadPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllChestPieces (
                      ChestPieceID INTEGER PRIMARY KEY,
                      ChestPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.ArmorChests.ArmorChestIDs, "AllChestPieces", "ChestPieceID", "ChestPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllArmsPieces (
                      ArmsPieceID INTEGER PRIMARY KEY,
                      ArmsPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.ArmorArms.ArmorArmIDs, "AllArmsPieces", "ArmsPieceID", "ArmsPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllWaistPieces (
                      WaistPieceID INTEGER PRIMARY KEY,
                      WaistPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.ArmorWaists.ArmorWaistIDs, "AllWaistPieces", "WaistPieceID", "WaistPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllLegsPieces (
                      LegsPieceID INTEGER PRIMARY KEY,
                      LegsPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.ArmorLegs.ArmorLegIDs, "AllLegsPieces", "LegsPieceID", "LegsPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS ZenithSkills(
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    ZenithSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    ZenithSkill1ID INTEGER NOT NULL CHECK (ZenithSkill1ID >= 0),
                    ZenithSkill2ID INTEGER NOT NULL CHECK (ZenithSkill2ID >= 0),
                    ZenithSkill3ID INTEGER NOT NULL CHECK (ZenithSkill3ID >= 0),
                    ZenithSkill4ID INTEGER NOT NULL CHECK (ZenithSkill4ID >= 0),
                    ZenithSkill5ID INTEGER NOT NULL CHECK (ZenithSkill5ID >= 0),
                    ZenithSkill6ID INTEGER NOT NULL CHECK (ZenithSkill6ID >= 0),
                    ZenithSkill7ID INTEGER NOT NULL CHECK (ZenithSkill7ID >= 0),
                    FOREIGN KEY(RunID) REFERENCES Quests(RunID)
                    FOREIGN KEY(ZenithSkill1ID) REFERENCES AllZenithSkills(ZenithSkillID),
                    FOREIGN KEY(ZenithSkill2ID) REFERENCES AllZenithSkills(ZenithSkillID),
                    FOREIGN KEY(ZenithSkill3ID) REFERENCES AllZenithSkills(ZenithSkillID),
                    FOREIGN KEY(ZenithSkill4ID) REFERENCES AllZenithSkills(ZenithSkillID),
                    FOREIGN KEY(ZenithSkill5ID) REFERENCES AllZenithSkills(ZenithSkillID),
                    FOREIGN KEY(ZenithSkill6ID) REFERENCES AllZenithSkills(ZenithSkillID),
                    FOREIGN KEY(ZenithSkill7ID) REFERENCES AllZenithSkills(ZenithSkillID))";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS AllZenithSkills(
                    ZenithSkillID INTEGER PRIMARY KEY,
                    ZenithSkillName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.ZenithSkillList.ZenithSkillID, "AllZenithSkills", "ZenithSkillID", "ZenithSkillName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AutomaticSkills(
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    AutomaticSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    AutomaticSkill1ID INTEGER NOT NULL CHECK (AutomaticSkill1ID >= 0),
                    AutomaticSkill2ID INTEGER NOT NULL CHECK (AutomaticSkill2ID >= 0),
                    AutomaticSkill3ID INTEGER NOT NULL CHECK (AutomaticSkill3ID >= 0),
                    AutomaticSkill4ID INTEGER NOT NULL CHECK (AutomaticSkill4ID >= 0),
                    AutomaticSkill5ID INTEGER NOT NULL CHECK (AutomaticSkill5ID >= 0),
                    AutomaticSkill6ID INTEGER NOT NULL CHECK (AutomaticSkill6ID >= 0),
                    FOREIGN KEY(RunID) REFERENCES Quests(RunID),
                    FOREIGN KEY(AutomaticSkill1ID) REFERENCES AllArmorSkills(ArmorSkillID),
                    FOREIGN KEY(AutomaticSkill2ID) REFERENCES AllArmorSkills(ArmorSkillID),
                    FOREIGN KEY(AutomaticSkill3ID) REFERENCES AllArmorSkills(ArmorSkillID),
                    FOREIGN KEY(AutomaticSkill4ID) REFERENCES AllArmorSkills(ArmorSkillID),
                    FOREIGN KEY(AutomaticSkill5ID) REFERENCES AllArmorSkills(ArmorSkillID),
                    FOREIGN KEY(AutomaticSkill6ID) REFERENCES AllArmorSkills(ArmorSkillID))";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS AllArmorSkills(
                    ArmorSkillID INTEGER PRIMARY KEY,
                    ArmorSkillName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.ArmorSkillList.ArmorSkillID, "AllArmorSkills", "ArmorSkillID", "ArmorSkillName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS ActiveSkills(
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    ActiveSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    ActiveSkill1ID INTEGER NOT NULL CHECK (ActiveSkill1ID >= 0),
                    ActiveSkill2ID INTEGER NOT NULL CHECK (ActiveSkill2ID >= 0),
                    ActiveSkill3ID INTEGER NOT NULL CHECK (ActiveSkill3ID >= 0),
                    ActiveSkill4ID INTEGER NOT NULL CHECK (ActiveSkill4ID >= 0),
                    ActiveSkill5ID INTEGER NOT NULL CHECK (ActiveSkill5ID >= 0),
                    ActiveSkill6ID INTEGER NOT NULL CHECK (ActiveSkill6ID >= 0),
                    ActiveSkill7ID INTEGER NOT NULL CHECK (ActiveSkill7ID >= 0),
                    ActiveSkill8ID INTEGER NOT NULL CHECK (ActiveSkill8ID >= 0),
                    ActiveSkill9ID INTEGER NOT NULL CHECK (ActiveSkill9ID >= 0),
                    ActiveSkill10ID INTEGER NOT NULL CHECK (ActiveSkill10ID >= 0),
                    ActiveSkill11ID INTEGER NOT NULL CHECK (ActiveSkill11ID >= 0),
                    ActiveSkill12ID INTEGER NOT NULL CHECK (ActiveSkill12ID >= 0),
                    ActiveSkill13ID INTEGER NOT NULL CHECK (ActiveSkill13ID >= 0),
                    ActiveSkill14ID INTEGER NOT NULL CHECK (ActiveSkill14ID >= 0),
                    ActiveSkill15ID INTEGER NOT NULL CHECK (ActiveSkill15ID >= 0),
                    ActiveSkill16ID INTEGER NOT NULL CHECK (ActiveSkill16ID >= 0),
                    ActiveSkill17ID INTEGER NOT NULL CHECK (ActiveSkill17ID >= 0),
                    ActiveSkill18ID INTEGER NOT NULL CHECK (ActiveSkill18ID >= 0),
                    ActiveSkill19ID INTEGER NOT NULL CHECK (ActiveSkill19ID >= 0),
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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS CaravanSkills(
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    CaravanSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    CaravanSkill1ID INTEGER NOT NULL CHECK (CaravanSkill1ID >= 0),
                    CaravanSkill2ID INTEGER NOT NULL CHECK (CaravanSkill2ID >= 0),
                    CaravanSkill3ID INTEGER NOT NULL CHECK (CaravanSkill3ID >= 0),
                    FOREIGN KEY(RunID) REFERENCES Quests(RunID)
                    FOREIGN KEY(CaravanSkill1ID) REFERENCES AllCaravanSkills(CaravanSkillID),
                    FOREIGN KEY(CaravanSkill2ID) REFERENCES AllCaravanSkills(CaravanSkillID),
                    FOREIGN KEY(CaravanSkill3ID) REFERENCES AllCaravanSkills(CaravanSkillID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS AllCaravanSkills(
                    CaravanSkillID INTEGER PRIMARY KEY,
                    CaravanSkillName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.CaravanSkillList.CaravanSkillID, "AllCaravanSkills", "CaravanSkillID", "CaravanSkillName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS StyleRankSkills(
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    StyleRankSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    StyleRankSkill1ID INTEGER NOT NULL CHECK (StyleRankSkill1ID >= 0),
                    StyleRankSkill2ID INTEGER NOT NULL CHECK (StyleRankSkill2ID >= 0),
                    FOREIGN KEY(RunID) REFERENCES Quests(RunID),
                    FOREIGN KEY(StyleRankSkill1ID) REFERENCES AllStyleRankSkills(StyleRankSkillID),
                    FOREIGN KEY(StyleRankSkill2ID) REFERENCES AllStyleRankSkills(StyleRankSkillID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS AllStyleRankSkills(
                    StyleRankSkillID INTEGER PRIMARY KEY,
                    StyleRankSkillName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.StyleRankSkillList.StyleRankSkillID, "AllStyleRankSkills", "StyleRankSkillID", "StyleRankSkillName", conn);

                    // Create the PlayerInventory table
                    sql = @"CREATE TABLE IF NOT EXISTS PlayerInventory (
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    PlayerInventoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    Item1ID INTEGER NOT NULL CHECK (Item1ID >= 0), 
                    Item1Quantity INTEGER NOT NULL,
                    Item2ID INTEGER NOT NULL CHECK (Item2ID >= 0), 
                    Item2Quantity INTEGER NOT NULL,
                    Item3ID INTEGER NOT NULL CHECK (Item3ID >= 0), 
                    Item3Quantity INTEGER NOT NULL,
                    Item4ID INTEGER NOT NULL CHECK (Item4ID >= 0), 
                    Item4Quantity INTEGER NOT NULL,
                    Item5ID INTEGER NOT NULL CHECK (Item5ID >= 0), 
                    Item5Quantity INTEGER NOT NULL,
                    Item6ID INTEGER NOT NULL CHECK (Item6ID >= 0), 
                    Item6Quantity INTEGER NOT NULL,
                    Item7ID INTEGER NOT NULL CHECK (Item7ID >= 0), 
                    Item7Quantity INTEGER NOT NULL,
                    Item8ID INTEGER NOT NULL CHECK (Item8ID >= 0), 
                    Item8Quantity INTEGER NOT NULL,
                    Item9ID INTEGER NOT NULL CHECK (Item9ID >= 0), 
                    Item9Quantity INTEGER NOT NULL,
                    Item10ID INTEGER NOT NULL CHECK (Item10ID >= 0), 
                    Item10Quantity INTEGER NOT NULL,
                    Item11ID INTEGER NOT NULL CHECK (Item11ID >= 0), 
                    Item11Quantity INTEGER NOT NULL,
                    Item12ID INTEGER NOT NULL CHECK (Item12ID >= 0), 
                    Item12Quantity INTEGER NOT NULL,
                    Item13ID INTEGER NOT NULL CHECK (Item13ID >= 0), 
                    Item13Quantity INTEGER NOT NULL,
                    Item14ID INTEGER NOT NULL CHECK (Item14ID >= 0), 
                    Item14Quantity INTEGER NOT NULL,
                    Item15ID INTEGER NOT NULL CHECK (Item15ID >= 0), 
                    Item15Quantity INTEGER NOT NULL,
                    Item16ID INTEGER NOT NULL CHECK (Item16ID >= 0), 
                    Item16Quantity INTEGER NOT NULL,
                    Item17ID INTEGER NOT NULL CHECK (Item17ID >= 0), 
                    Item17Quantity INTEGER NOT NULL,
                    Item18ID INTEGER NOT NULL CHECK (Item18ID >= 0), 
                    Item18Quantity INTEGER NOT NULL,
                    Item19ID INTEGER NOT NULL CHECK (Item19ID >= 0), 
                    Item19Quantity INTEGER NOT NULL,
                    Item20ID INTEGER NOT NULL CHECK (Item20ID >= 0), 
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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS AmmoPouch (
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    AmmoPouchID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    Item1ID INTEGER NOT NULL CHECK (Item1ID >= 0), 
                    Item1Quantity INTEGER NOT NULL,
                    Item2ID INTEGER NOT NULL CHECK (Item2ID >= 0), 
                    Item2Quantity INTEGER NOT NULL,
                    Item3ID INTEGER NOT NULL CHECK (Item3ID >= 0), 
                    Item3Quantity INTEGER NOT NULL,
                    Item4ID INTEGER NOT NULL CHECK (Item4ID >= 0), 
                    Item4Quantity INTEGER NOT NULL,
                    Item5ID INTEGER NOT NULL CHECK (Item5ID >= 0), 
                    Item5Quantity INTEGER NOT NULL,
                    Item6ID INTEGER NOT NULL CHECK (Item6ID >= 0), 
                    Item6Quantity INTEGER NOT NULL,
                    Item7ID INTEGER NOT NULL CHECK (Item7ID >= 0), 
                    Item7Quantity INTEGER NOT NULL,
                    Item8ID INTEGER NOT NULL CHECK (Item8ID >= 0), 
                    Item8Quantity INTEGER NOT NULL,
                    Item9ID INTEGER NOT NULL CHECK (Item9ID >= 0), 
                    Item9Quantity INTEGER NOT NULL,
                    Item10ID INTEGER NOT NULL CHECK (Item10ID >= 0), 
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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS RoadDureSkills (
                    CreatedAt DATETIME NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    RoadDureSkillsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    RoadDureSkill1ID INTEGER NOT NULL CHECK (RoadDureSkill1ID >= 0), 
                    RoadDureSkill1Level INTEGER NOT NULL,
                    RoadDureSkill2ID INTEGER NOT NULL CHECK (RoadDureSkill2ID >= 0), 
                    RoadDureSkill2Level INTEGER NOT NULL,
                    RoadDureSkill3ID INTEGER NOT NULL CHECK (RoadDureSkill3ID >= 0), 
                    RoadDureSkill3Level INTEGER NOT NULL,
                    RoadDureSkill4ID INTEGER NOT NULL CHECK (RoadDureSkill4ID >= 0), 
                    RoadDureSkill4Level INTEGER NOT NULL,
                    RoadDureSkill5ID INTEGER NOT NULL CHECK (RoadDureSkill5ID >= 0), 
                    RoadDureSkill5Level INTEGER NOT NULL,
                    RoadDureSkill6ID INTEGER NOT NULL CHECK (RoadDureSkill6ID >= 0), 
                    RoadDureSkill6Level INTEGER NOT NULL,
                    RoadDureSkill7ID INTEGER NOT NULL CHECK (RoadDureSkill7ID >= 0), 
                    RoadDureSkill7Level INTEGER NOT NULL,
                    RoadDureSkill8ID INTEGER NOT NULL CHECK (RoadDureSkill8ID >= 0), 
                    RoadDureSkill8Level INTEGER NOT NULL,
                    RoadDureSkill9ID INTEGER NOT NULL CHECK (RoadDureSkill9ID >= 0), 
                    RoadDureSkill9Level INTEGER NOT NULL,
                    RoadDureSkill10ID INTEGER NOT NULL CHECK (RoadDureSkill10ID >= 0), 
                    RoadDureSkill10Level INTEGER NOT NULL,
                    RoadDureSkill11ID INTEGER NOT NULL CHECK (RoadDureSkill11ID >= 0), 
                    RoadDureSkill11Level INTEGER NOT NULL,
                    RoadDureSkill12ID INTEGER NOT NULL CHECK (RoadDureSkill12ID >= 0), 
                    RoadDureSkill12Level INTEGER NOT NULL,
                    RoadDureSkill13ID INTEGER NOT NULL CHECK (RoadDureSkill13ID >= 0), 
                    RoadDureSkill13Level INTEGER NOT NULL,
                    RoadDureSkill14ID INTEGER NOT NULL CHECK (RoadDureSkill14ID >= 0), 
                    RoadDureSkill14Level INTEGER NOT NULL,
                    RoadDureSkill15ID INTEGER NOT NULL CHECK (RoadDureSkill15ID >= 0), 
                    RoadDureSkill15Level INTEGER NOT NULL,
                    RoadDureSkill16ID INTEGER NOT NULL CHECK (RoadDureSkill16ID >= 0), 
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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS AllRoadDureSkills(
                    RoadDureSkillID INTEGER PRIMARY KEY,
                    RoadDureSkillName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.RoadDureSkills.RoadDureSkillIDs, "AllRoadDureSkills", "RoadDureSkillID", "RoadDureSkillName", conn);

                    #region gacha
                    // a mh game but like a MUD. hunt in-game to get many kinds of points for this game. hunt and tame monsters. challenge other CPU players/monsters.

                    sql = @"CREATE TABLE IF NOT EXISTS GachaMaterial(
                    GachaMaterialID INTEGER PRIMARY KEY,
                    GachaMaterialName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaPlayerCurrency (
                    GachaPlayerID INTEGER PRIMARY KEY,
                    TrialGachaCoins INTEGER NOT NULL DEFAULT 0,
                    PremiumGachaCoins INTEGER NOT NULL DEFAULT 0,
                    PrismaticGachaCoins INTEGER NOT NULL DEFAULT 0,
                    FrontierPoints INTEGER NOT NULL DEFAULT 0,
                    LegendaryTickets INTEGER NOT NULL DEFAULT 0,
                    NetCafePoints INTEGER NOT NULL DEFAULT 0,
                    MonsterCoins INTEGER NOT NULL DEFAULT 0,
                    GZenny INTEGER NOT NULL DEFAULT 0,
                    Zenny INTEGER NOT NULL DEFAULT 0,
                    GCP INTEGER NOT NULL DEFAULT 0,
                    CP INTEGER NOT NULL DEFAULT 0,
                    Gg INTEGER NOT NULL DEFAULT 0,
                    g INTEGER NOT NULL DEFAULT 0,
                    GreatSlayingPoints INTEGER NOT NULL DEFAULT 0,
                    MezFesCoins INTEGER NOT NULL DEFAULT 0,
                    RdP INTEGER NOT NULL DEFAULT 0, -- Road
                    Gm INTEGER NOT NULL DEFAULT 0,
                    TowerMedals INTEGER NOT NULL DEFAULT 0,
                    TowerPoints INTEGER NOT NULL DEFAULT 0,
                    Souls INTEGER NOT NULL DEFAULT 0,
                    FestivalPoints INTEGER NOT NULL DEFAULT 0,
                    FestivalTickets INTEGER NOT NULL DEFAULT 0,
                    FestivalGems INTEGER NOT NULL DEFAULT 0,
                    FestivalMarks INTEGER NOT NULL DEFAULT 0,
                    GuildTickets INTEGER NOT NULL DEFAULT 0,
                    GuildMedals INTEGER NOT NULL DEFAULT 0,
                    HuntingMedals INTEGER NOT NULL DEFAULT 0,
                    InterceptionPoints INTEGER NOT NULL DEFAULT 0,
                    DivaNotes INTEGER NOT NULL DEFAULT 0,
                    PoogiePoints INTEGER NOT NULL DEFAULT 0,
                    PartnerPoints INTEGER NOT NULL DEFAULT 0,
                    PartnyaaPoints INTEGER NOT NULL DEFAULT 0,
                    GalleryPoints INTEGER NOT NULL DEFAULT 0,
                    TranscendPoints INTEGER NOT NULL DEFAULT 0,
                    ZZenny INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY(GachaPlayerID) REFERENCES GachaPlayer(GachaPlayerID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaPlayerGem (
                    GachaPlayerID INTEGER PRIMARY KEY,
                    GemColor TEXT NOT NULL,
                    GemLv INTEGER NOT NULL DEFAULT 1,
                    PeachPoints INTEGER NOT NULL DEFAULT 0,
                    BrownPoints INTEGER NOT NULL DEFAULT 0,
                    YellowPoints INTEGER NOT NULL DEFAULT 0,
                    GreenPoints INTEGER NOT NULL DEFAULT 0,
                    WhitePoints INTEGER NOT NULL DEFAULT 0,
                    PurplePoints INTEGER NOT NULL DEFAULT 0,
                    CyanPoints INTEGER NOT NULL DEFAULT 0,
                    RainbowPoints INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY(GachaPlayerID) REFERENCES GachaPlayer(GachaPlayerID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaPlayer (
                    GachaPlayerID INTEGER PRIMARY KEY,
                    GachaPlayerAvatar TEXT NOT NULL DEFAULT 'https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/icon/transcend.png',
                    Level INTEGER NOT NULL, -- the character level
                    Experience INTEGER NOT NULL, -- xp for the char level
                    Health INTEGER NOT NULL, -- The player's maximum hit points and current hit points
                    Stamina INTEGER NOT NULL, -- the player stamina for actions
                    Strength INTEGER NOT NULL,-- Affects the player's physical damage and carrying capacity
                    Dexterity INTEGER NOT NULL, -- Affects the player's accuracy and critical hit chance with weapons, as well as their dodge chance
                    Intelligence INTEGER NOT NULL, --Affects the player's XP gain and the number of moves they can learn
                    Charisma INTEGER NOT NULL, --Affects the player's ability to persuade or intimidate NPC characters, and may affect the prices of items they buy or sell. also increases chance of NPC asking for monsterVSmonster battles.
                    Endurance INTEGER NOT NULL, -- Affects the player's resistance to physical damage and ability to withstand harsh environments
                    Stealth INTEGER NOT NULL, -- Affects the ability to be spotted by monsters
                    Agility INTEGER NOT NULL, -- Affects the player's movement speed and overall agility in combat
                    Accuracy INTEGER NOT NULL, -- Affects the player's ability to hit targets with their attacks
                    Synergy INTEGER NOT NULL, -- the player coordination ability with NPC teammates. high points increases the chance of more hunt rewards and success. low points increase the chance of lower hunt rewards and success, and also NPC asking for PVP duels after a successful hunt.
                    Artisanry INTEGER NOT NULL, -- This name implies a level of creativity and artistic ability in the creation of gear.
                    Taming INTEGER NOT NULL, -- for monster pets. chance of taming and monster pet stats increases.
                    Dueling INTEGER NOT NULL, -- for pvp. chance of winning and/or losing but avoiding injury increases.
                    -- idk what else
                    BlademasterXP INTEGER NOT NULL, -- xp for the weapon class
                    GunnerXP INTEGER NOT NULL,
                    ShortSwordXP INTEGER NOT NULL, -- xp for the weapon subclasses, SNS+DS
                    LargeSwordXP INTEGER NOT NULL, -- GS+LS
                    LancesXP INTEGER NOT NULL, -- LA+GL
                    BluntWeaponsXP INTEGER NOT NULL, -- HA+HH
                    BowgunXP INTEGER NOT NULL, --LBG+HBG
                    -- TO, SAF, MS and Bow have bonus xp gain because they don't form part of a subclass for the extra XP.
                    SwordAndShieldLevel INTEGER NOT NULL, -- the mastery of the weapons
                    DualSwordsLevel INTEGER NOT NULL,
                    GreatSwordLevel INTEGER NOT NULL,
                    LongSwordLevel INTEGER NOT NULL,
                    LanceLevel INTEGER NOT NULL,
                    GunlanceLevel INTEGER NOT NULL,
                    HammerLevel INTEGER NOT NULL,
                    HuntingHornLevel INTEGER NOT NULL,
                    TonfaLevel INTEGER NOT NULL,
                    SwitchAxeFLevel INTEGER NOT NULL,
                    MagnetSpikeLevel INTEGER NOT NULL,
                    LightBowgunLevel INTEGER NOT NULL,
                    HeavyBowgunLevel INTEGER NOT NULL,
                    BowLevel INTEGER NOT NULL,
                    GachaWeaponID INTEGER NOT NULL,
                    WeaponLevel INTEGER NOT NULL,
                    WeaponXP INTEGER NOT NULL, --used to level weapon along with monster materials
                    FOREIGN KEY(GachaWeaponID) REFERENCES GachaWeapon(GachaWeaponID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaWeapon(
                    GachaWeaponID INTEGER PRIMARY KEY,
                    -- flavor text option? 
                    GachaWeaponType TEXT NOT NULL,
                    GachaWeaponName TEXT NOT NULL,
                    GachaWeaponDamage INTEGER NOT NULL,
                    GachaWeaponBonusDefense INTEGER NOT NULL,
                    GachaWeaponAffinity INTEGER NOT NULL,
                    GachaWeaponElementTypeID INTEGER NOT NULL,
                    GachaWeaponElementValue INTEGER NOT NULL,
                    GachaWeaponStatusTypeID INTEGER NOT NULL,
                    GachaWeaponStatusValue INTEGER NOT NULL,
                    GachaWeaponSlot1Type TEXT NOT NULL,
                    GachaWeaponSlot1ItemID INTEGER NOT NULL,
                    GachaWeaponSlot2Type TEXT NOT NULL,
                    GachaWeaponSlot2ItemID INTEGER NOT NULL,
                    GachaWeaponSlot3Type TEXT NOT NULL,
                    GachaWeaponSlot3ItemID INTEGER NOT NULL,
                    FOREIGN KEY(GachaWeaponSlot1ItemID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY(GachaWeaponSlot2ItemID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY(GachaWeaponSlot3ItemID) REFERENCES GachaMaterial(GachaMaterialID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaCraftWeapon(
                    GachaCraftID INTEGER PRIMARY KEY,
                    GachaWeaponID INTEGER NOT NULL,
                    Material1ID INTEGER,
                    Material1Quantity INTEGER,
                    Material2ID INTEGER,
                    Material2Quantity INTEGER,
                    Material3ID INTEGER,
                    Material3Quantity INTEGER,
                    Material4ID INTEGER,
                    Material4Quantity INTEGER,
                    Material5ID INTEGER,
                    Material5Quantity INTEGER,
                    Material6ID INTEGER,
                    Material6Quantity INTEGER,
                    FOREIGN KEY (GachaWeaponID) REFERENCES GachaWeapon(GachaWeaponID),
                    FOREIGN KEY (Material1ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material2ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material3ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material4ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material5ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material6ID) REFERENCES GachaMaterial(GachaMaterialID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaWeaponShop(
                    GachaWeaponID INTEGER PRIMARY KEY,
                    FrontierPoints INTEGER NOT NULL DEFAULT 0,
                    NetCafePoints INTEGER NOT NULL DEFAULT 0,
                    GZenny INTEGER NOT NULL DEFAULT 0,
                    Zenny INTEGER NOT NULL DEFAULT 0,
                    GCP INTEGER NOT NULL DEFAULT 0,
                    CP INTEGER NOT NULL DEFAULT 0,
                    Gg INTEGER NOT NULL DEFAULT 0,
                    g INTEGER NOT NULL DEFAULT 0,
                    RdP INTEGER NOT NULL DEFAULT 0, -- Road
                    TowerMedals INTEGER NOT NULL DEFAULT 0,
                    TowerPoints INTEGER NOT NULL DEFAULT 0,
                    FestivalTickets INTEGER NOT NULL DEFAULT 0,
                    FestivalGems INTEGER NOT NULL DEFAULT 0,
                    FestivalMarks INTEGER NOT NULL DEFAULT 0,
                    GuildMedals INTEGER NOT NULL DEFAULT 0,
                    HuntingMedals INTEGER NOT NULL DEFAULT 0,
                    InterceptionPoints INTEGER NOT NULL DEFAULT 0,
                    DivaNotes INTEGER NOT NULL DEFAULT 0,
                    ZZenny INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY(GachaWeaponID) REFERENCES GachaWeapon(GachaWeaponID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaArmor(
                    GachaArmorID INTEGER PRIMARY KEY,
                    -- flavor text option? 
                    GachaArmorType TEXT NOT NULL,
                    GachaArmorName TEXT NOT NULL,
                    GachaArmorDefense INTEGER NOT NULL,
                    GachaArmorFireRes INTEGER NOT NULL,
                    GachaArmorWaterRes INTEGER NOT NULL,
                    GachaArmorThunderRes INTEGER NOT NULL,
                    GachaArmorIceRes INTEGER NOT NULL,
                    GachaArmorDragonRes INTEGER NOT NULL,
                    GachaArmorSkill1ID INTEGER NOT NULL,
                    GachaArmorSkill1Points INTEGER NOT NULL,
                    GachaArmorSkill2ID INTEGER NOT NULL,
                    GachaArmorSkill2Points INTEGER NOT NULL,
                    GachaArmorSkill3ID INTEGER NOT NULL,
                    GachaArmorSkill3Points INTEGER NOT NULL,
                    GachaArmorSkill4ID INTEGER NOT NULL,
                    GachaArmorSkill4Points INTEGER NOT NULL,
                    GachaArmorSlot1Type TEXT NOT NULL,
                    GachaArmorSlot1ItemID INTEGER NOT NULL,
                    GachaArmorSlot2Type TEXT NOT NULL,
                    GachaArmorSlot2ItemID INTEGER NOT NULL,
                    GachaArmorSlot3Type TEXT NOT NULL,
                    GachaArmorSlot3ItemID INTEGER NOT NULL,
                    FOREIGN KEY(GachaArmorSkill1ID) REFERENCES GachaArmorSkill(GachaArmorSkillID),
                    FOREIGN KEY(GachaArmorSkill2ID) REFERENCES GachaArmorSkill(GachaArmorSkillID),
                    FOREIGN KEY(GachaArmorSkill3ID) REFERENCES GachaArmorSkill(GachaArmorSkillID),
                    FOREIGN KEY(GachaArmorSkill4ID) REFERENCES GachaArmorSkill(GachaArmorSkillID),
                    FOREIGN KEY(GachaArmorSlot1ItemID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY(GachaArmorSlot2ItemID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY(GachaArmorSlot3ItemID) REFERENCES GachaMaterial(GachaMaterialID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaCraftArmor(
                    GachaCraftID INTEGER PRIMARY KEY,
                    GachaArmorID INTEGER NOT NULL,
                    Material1ID INTEGER,
                    Material1Quantity INTEGER,
                    Material2ID INTEGER,
                    Material2Quantity INTEGER,
                    Material3ID INTEGER,
                    Material3Quantity INTEGER,
                    Material4ID INTEGER,
                    Material4Quantity INTEGER,
                    Material5ID INTEGER,
                    Material5Quantity INTEGER,
                    Material6ID INTEGER,
                    Material6Quantity INTEGER,
                    FOREIGN KEY (GachaArmorID) REFERENCES GachaArmor(GachaArmorID),
                    FOREIGN KEY (Material1ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material2ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material3ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material4ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material5ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material6ID) REFERENCES GachaMaterial(GachaMaterialID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaArmorShop(
                    GachaArmorID INTEGER PRIMARY KEY,
                    FrontierPoints INTEGER NOT NULL DEFAULT 0,
                    NetCafePoints INTEGER NOT NULL DEFAULT 0,
                    GZenny INTEGER NOT NULL DEFAULT 0,
                    Zenny INTEGER NOT NULL DEFAULT 0,
                    GCP INTEGER NOT NULL DEFAULT 0,
                    CP INTEGER NOT NULL DEFAULT 0,
                    Gg INTEGER NOT NULL DEFAULT 0,
                    g INTEGER NOT NULL DEFAULT 0,
                    RdP INTEGER NOT NULL DEFAULT 0, -- Road
                    TowerMedals INTEGER NOT NULL DEFAULT 0,
                    TowerPoints INTEGER NOT NULL DEFAULT 0,
                    FestivalTickets INTEGER NOT NULL DEFAULT 0,
                    FestivalGems INTEGER NOT NULL DEFAULT 0,
                    FestivalMarks INTEGER NOT NULL DEFAULT 0,
                    GuildMedals INTEGER NOT NULL DEFAULT 0,
                    HuntingMedals INTEGER NOT NULL DEFAULT 0,
                    InterceptionPoints INTEGER NOT NULL DEFAULT 0,
                    DivaNotes INTEGER NOT NULL DEFAULT 0,
                    ZZenny INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY(GachaArmorID) REFERENCES GachaArmor(GachaArmorID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaCraftItem(
                    GachaCraftID INTEGER PRIMARY KEY,
                    GachaItemID INTEGER NOT NULL,
                    Material1ID INTEGER,
                    Material1Quantity INTEGER,
                    Material2ID INTEGER,
                    Material2Quantity INTEGER,
                    Material3ID INTEGER,
                    Material3Quantity INTEGER,
                    Material4ID INTEGER,
                    Material4Quantity INTEGER,
                    Material5ID INTEGER,
                    Material5Quantity INTEGER,
                    Material6ID INTEGER,
                    Material6Quantity INTEGER,
                    FOREIGN KEY (GachaItemID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material1ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material2ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material3ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material4ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material5ID) REFERENCES GachaMaterial(GachaMaterialID),
                    FOREIGN KEY (Material6ID) REFERENCES GachaMaterial(GachaMaterialID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaItemShop(
                    GachaItemID INTEGER PRIMARY KEY,
                    FrontierPoints INTEGER NOT NULL DEFAULT 0,
                    NetCafePoints INTEGER NOT NULL DEFAULT 0,
                    GZenny INTEGER NOT NULL DEFAULT 0,
                    Zenny INTEGER NOT NULL DEFAULT 0,
                    GCP INTEGER NOT NULL DEFAULT 0,
                    CP INTEGER NOT NULL DEFAULT 0,
                    Gg INTEGER NOT NULL DEFAULT 0,
                    g INTEGER NOT NULL DEFAULT 0,
                    RdP INTEGER NOT NULL DEFAULT 0, -- Road
                    TowerMedals INTEGER NOT NULL DEFAULT 0,
                    TowerPoints INTEGER NOT NULL DEFAULT 0,
                    FestivalTickets INTEGER NOT NULL DEFAULT 0,
                    FestivalGems INTEGER NOT NULL DEFAULT 0,
                    FestivalMarks INTEGER NOT NULL DEFAULT 0,
                    GuildMedals INTEGER NOT NULL DEFAULT 0,
                    HuntingMedals INTEGER NOT NULL DEFAULT 0,
                    InterceptionPoints INTEGER NOT NULL DEFAULT 0,
                    DivaNotes INTEGER NOT NULL DEFAULT 0,
                    ZZenny INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY(GachaItemID) REFERENCES GachaMaterial(GachaMaterialID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaMonster(
                    GachaMonsterID INTEGER PRIMARY KEY,
                    Level INTEGER NOT NULL,
                    Experience INTEGER NOT NULL,
                    Health INTEGER NOT NULL,
                    Stamina INTEGER NOT NULL,
                    Defense INTEGER NOT NULL,
                    Attack INTEGER NOT NULL,
                    Speed INTEGER NOT NULL,
                    SpecialAttack INTEGER NOT NULL, -- increases the chance and damage of the special attack
                    Intelligence INTEGER NOT NULL, -- increases chance to counter-attack
                    Morale INTEGER NOT NULL, -- high morale decreases chance to flee if opponent stats are higher                
                    FireRes INTEGER NOT NULL, 
                    WaterRes INTEGER NOT NULL, 
                    ThunderRes INTEGER NOT NULL, 
                    IceRes INTEGER NOT NULL, 
                    DragonRes INTEGER NOT NULL, 
                    FireAttack INTEGER NOT NULL,
                    WaterAttack INTEGER NOT NULL,
                    ThunderAttack INTEGER NOT NULL,
                    IceAttack INTEGER NOT NULL,
                    DragonAttack INTEGER NOT NULL,
                    Stun INTEGER NOT NULL,
                    StunRes INTEGER NOT NULL,
                    Poison INTEGER NOT NULL,
                    PoisonRes INTEGER NOT NULL,
                    Paralysis INTEGER NOT NULL,
                    ParalysisRes INTEGER NOT NULL,
                    Sleep INTEGER NOT NULL,
                    SleepRes INTEGER NOT NULL,
                    Blast INTEGER,
                    BlastRes INTEGER NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    #endregion

                    // Commit the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    HandleError(transaction, ex);
                }
            }
        }

        //i would first insert into the quest table,
        //then the tables referencing
        //playergear, then the playergear table
        // TODO
        void InsertQuestIntoDatabase(SQLiteConnection conn, DataLoader dataLoader)
        {
            var model = dataLoader.model;
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

        // TODO
        void RetreiveQuestsFromDatabase()
        {
            SQLiteConnection conn = new SQLiteConnection(dataSource);
            
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
    }
}
/* TODO
 * add checker for triggers and indexes changes
 * 
 * USE BLOB for attack buff list and hit count list.
 * data structure: list<int (timeint), int (hit count / attack buff)>
 * 
 * 
 * 
 You can use LINQ to perform various operations on lists, such as filtering, sorting, and aggregating data. Here's an example of how you can use LINQ to calculate the average attack buff of a particular quest run with a particular weapon type:

Copy code
int weaponType = 3;
int questID = 123;

List<int> attackBuffList = GetAttackBuffList(weaponType, questID);

double averageAttackBuff = attackBuffDictionary.Values.Average();
To calculate the maximum attack buff of a particular quest run with a particular weapon type, you can use the Max() method:

Copy code
int maxAttackBuff = attackBuffDictionary.Values.Max();
To calculate the maximum attack buff of all quest runs of a particular quest with a particular weapon type, you can use LINQ to group the attack buff values by quest and weapon type, and then use the Max() method to find the maximum attack buff for each group:

Copy code
var attackBuffGroups = attackBuffList
    .GroupBy(x => new { QuestID = questID, WeaponType = weaponType })
    .Select(g => new {
        QuestID = g.Key.QuestID,
        WeaponType = g.Key.WeaponType,
        MaxAttackBuff = g.Max()
    });
This will return a list of groups, each containing the quest ID, weapon type, and maximum attack buff for a particular quest and weapon type. You can then iterate over this list to find the maximum attack buff for all quest runs. 
 
 Include info from more spreadsheets (speedrun calculation etc)

LINQ (Language Integrated Query) is a set of features in C# that allows you to write queries to filter, transform, and aggregate data in your code. It works with various data sources, including arrays, lists, and dictionaries.

For example, if you want to calculate the average attack buff for a particular quest run with a particular weapon type, you could use LINQ's Average method like this:

Copy code
double averageAttackBuff = attackBuffDictionary.Values.Average();
To calculate the maximum attack buff for a particular quest run with a particular weapon type, you could use LINQ's Max method like this:

Copy code
int maxAttackBuff = attackBuffDictionary.Values.Max();
To calculate the maximum attack buff of all quest runs of a particular quest with a particular weapon type, you would need to store the attack buff dictionaries for each quest run in a list or collection and then use LINQ's Max method like this:

Copy code
int maxAttackBuffAllRuns = attackBuffDictionaries.Select(d => d.Values.Max()).Max();
This would select the maximum attack buff value for each dictionary in the list, and then find the overall maximum value from those.

You can read more about LINQ and its various methods and features in the C# documentation: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
 */
