using Dictionary;
using EZlion.Mapper;
using MHFZ_Overlay.Core.Class.Application;
using MHFZ_Overlay.Core.Class.IO;
using MHFZ_Overlay.Core.Class.Log;
using MHFZ_Overlay.UI.Class;
using MHFZ_Overlay.UI.Class.Mapper;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Targets;
using Octokit;
using SharpCompress.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Formatting = Newtonsoft.Json.Formatting;
using Quest = MHFZ_Overlay.UI.Class.Quest;

// TODO: PascalCase for functions, camelCase for private fields, ALL_CAPS for constants
namespace MHFZ_Overlay
{
    /// <summary>
    /// Handles the SQLite database, MHFZ_Overlay.sqlite. A singleton.
    /// </summary>
    internal class DatabaseManager
    {
        private string _connectionString;

        private static DatabaseManager instance;

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private const string BackupFolderName = "backups";

        private DatabaseManager()
        {
            logger.Info($"DatabaseManager initialized");
        }

        private string _customDatabasePath;
        private string dataSource;

        public void CheckIfSchemaChanged(DataLoader mainWindowDataLoader)
        {
            if (mainWindowDataLoader.databaseChanged)
            {
                Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
                logger.Warn("Database structure needs update");
                MessageBox.Show("Please update the database structure", LoggingManager.WARNING_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                s.EnableQuestLogging = false;
            }
        }

        //TODO test
        public void CheckIfUserSetDatabasePath()
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (string.IsNullOrEmpty(s.DatabaseFilePath) || !Directory.Exists(Path.GetDirectoryName(s.DatabaseFilePath)))
            {
                _connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\MHFZ_Overlay.sqlite");

                // Show warning to user that they should set a custom database path to prevent data loss on update
                logger.Warn("The database file is being saved to the overlay default location");
                MessageBox.Show("Warning: The database is currently stored in the default location and will be deleted on update. Please select a custom database location to prevent data loss.", "MHFZ-Overlay Data Loss Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                // Use default database path
                _customDatabasePath = _connectionString;
            }
            else
            {
                _customDatabasePath = s.DatabaseFilePath;
            }

            if (!File.Exists(_customDatabasePath))
            {
                logger.Info("{0} not found, creating file", _customDatabasePath);
                SQLiteConnection.CreateFile(_customDatabasePath);
            }

            dataSource = "Data Source=" + _customDatabasePath;
        }

        public static DatabaseManager GetInstance()
        {
            if (instance == null)
            {
                logger.Info("Singleton not found, creating instance.");
                instance = new DatabaseManager();
            }
            logger.Info("Singleton found, returning instance.");
            logger.Trace(new StackTrace().ToString());
            return instance;
        }

        #region program time

        // Calculate the total time spent using the program
        // TODO: add transaction. check if others also need transaction.
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
        private static string previousVersion;

        public bool SetupLocalDatabase(DataLoader dataLoader)
        {
            dataLoader.model.ShowSaveIcon = true;

            if (!isDatabaseSetup)
            {
                isDatabaseSetup = true;

                CheckIfUserSetDatabasePath();
                // TODO: test and add semantic versioning regex
                WritePreviousVersionToFile();

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
                    logger.Error(ex, "Invalid database file");
                    MessageBox.Show(String.Format("Invalid database file. Delete the MHFZ_Overlay.sqlite, previousVersion.txt and reference_schema.json if present, and rerun the program.\n\n{0}", ex), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                    ApplicationManager.HandleShutdown();
                }

                using (var conn = new SQLiteConnection(dataSource))
                {
                    conn.Open();

                    // Toggle comment this for testing the error handling
                    //ThrowException(conn);

                    CreateDatabaseTables(conn, dataLoader);
                    CreateDatabaseIndexes(conn);
                    CreateDatabaseTriggers(conn);
                    UpdateDatabaseSchema(conn);
                }

                using (var conn = new SQLiteConnection(dataSource))
                {
                    conn.Open();

                    var referenceSchemaFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\reference_schema.json");

                    var doesReferenceSchemaFileExist = FileManager.CheckIfFileExists(referenceSchemaFilePath, "Checking reference schema");
                    // Check if the reference schema file exists
                    if (!doesReferenceSchemaFileExist)
                    {
                        logger.Info("Creating reference schema");
                        CreateReferenceSchemaJSONFromLocalDatabaseFile(conn);
                    }
                    else
                    {
                        // Load the reference schema file
                        var referenceSchemaJson = File.ReadAllText(referenceSchemaFilePath);
                        var referenceSchema = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(referenceSchemaJson);

                        // Create a dictionary to store the current schema
                        var currentSchema = CreateReferenceSchemaJSONFromLocalDatabaseFile(conn, false);
                        logger.Info("Found existing reference schema, comparing schemas", referenceSchemaFilePath);
                        CompareDatabaseSchemas(referenceSchema, currentSchema);
                    }
                }

                if (schemaChanged)
                {
                    logger.Fatal("Outdated database schema");
                    MessageBox.Show("Your quest runs will not be accepted into the central database unless you update the schemas.", LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                    ApplicationManager.HandleShutdown();
                }
            }

            dataLoader.model.ShowSaveIcon = false;

            return schemaChanged;
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

        /// <summary>
        /// Insert personal best
        /// </summary>
        /// <param name="dataLoader"></param>
        /// <param name="currentPersonalBest"></param>
        /// <param name="attempts"></param>
        /// <param name="runID"></param>
        public void InsertPersonalBest(DataLoader dataLoader, long currentPersonalBest, long attempts, int runID)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                var model = dataLoader.model;
                string sql;

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        sql = @"INSERT INTO PersonalBests(
                        RunID,
                        Attempts
                        ) VALUES (
                        @RunID,
                        @Attempts)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int finalTimeValue = model.TimeDefInt() - model.TimeInt();
                            if (finalTimeValue < currentPersonalBest || currentPersonalBest == 0)
                            {
                                cmd.Parameters.AddWithValue("@RunID", runID);
                                cmd.Parameters.AddWithValue("@Attempts", attempts);
                                // Execute the stored procedure
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
        }

        /// <summary>
        /// Insert quest data
        /// </summary>
        /// <param name="dataLoader"></param>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public int InsertQuestData(DataLoader dataLoader, int attempts)
        {
            logger.Info("Inserting quest data");

            int runID = 0;
            string actualOverlayMode = "";
            dataLoader.model.ShowSaveIcon = true;

            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (!dataLoader.model.ValidateGameFolder())
                return runID;

            if (!s.EnableQuestLogging)
                return runID;

            if (!dataLoader.model.questCleared)
                return runID;

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                var model = dataLoader.model;
                string sql;
                DateTime createdAt = DateTime.Now;
                string createdBy = dataLoader.model.GetFullCurrentProgramVersion();
                int playerID = 1;

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
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
                        TimeLeft,
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
                        HitsTakenBlockedDictionary,
                        HitsTakenBlockedPerSecondDictionary,
                        PlayerHPDictionary,
                        PlayerStaminaDictionary,
                        KeystrokesDictionary,
                        MouseInputDictionary,
                        GamepadInputDictionary,
                        ActionsPerMinuteDictionary,
                        OverlayModeDictionary,
                        ActualOverlayMode,
                        PartySize,
                        Monster1HPDictionary,
                        Monster2HPDictionary,
                        Monster3HPDictionary,
                        Monster4HPDictionary,
                        Monster1AttackMultiplierDictionary,
                        Monster1DefenseRateDictionary,
                        Monster1SizeMultiplierDictionary,
                        Monster1PoisonThresholdDictionary,
                        Monster1SleepThresholdDictionary,
                        Monster1ParalysisThresholdDictionary,
                        Monster1BlastThresholdDictionary,
                        Monster1StunThresholdDictionary,
                        IsHighGradeEdition,
                        RefreshRate
                        ) VALUES (
                        @QuestHash,
                        @CreatedAt,
                        @CreatedBy,
                        @QuestID, 
                        @TimeLeft,
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
                        @HitsTakenBlockedDictionary,
                        @HitsTakenBlockedPerSecondDictionary,
                        @PlayerHPDictionary,
                        @PlayerStaminaDictionary,
                        @KeystrokesDictionary,
                        @MouseInputDictionary,
                        @GamepadInputDictionary,
                        @ActionsPerMinuteDictionary,
                        @OverlayModeDictionary,
                        @ActualOverlayMode,
                        @PartySize,
                        @Monster1HPDictionary,
                        @Monster2HPDictionary,
                        @Monster3HPDictionary,
                        @Monster4HPDictionary,
                        @Monster1AttackMultiplierDictionary,
                        @Monster1DefenseRateDictionary,
                        @Monster1SizeMultiplierDictionary,
                        @Monster1PoisonThresholdDictionary,
                        @Monster1SleepThresholdDictionary,
                        @Monster1ParalysisThresholdDictionary,
                        @Monster1BlastThresholdDictionary,
                        @Monster1StunThresholdDictionary,
                        @IsHighGradeEdition,
                        @RefreshRate
                        )";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int questID = model.QuestID();
                            int timeLeft = model.TimeInt(); // Example value of the TimeLeft variable
                            int finalTimeValue = model.TimeDefInt() - model.TimeInt();
                            // Calculate the elapsed time of the quest
                            string finalTimeDisplay = dataLoader.GetQuestTimeCompletion();
                            // Convert the elapsed time to a DateTime object
                            string objectiveImage;
                            //Gathering/etc
                            if ((dataLoader.model.ObjectiveType() == 0x0 || dataLoader.model.ObjectiveType() == 0x02 || dataLoader.model.ObjectiveType() == 0x1002) && (dataLoader.model.QuestID() != 23527 && dataLoader.model.QuestID() != 23628 && dataLoader.model.QuestID() != 21731 && dataLoader.model.QuestID() != 21749 && dataLoader.model.QuestID() != 21746 && dataLoader.model.QuestID() != 21750))
                            {
                                objectiveImage = dataLoader.model.GetAreaIconFromID(dataLoader.model.AreaID());
                            }
                            //Tenrou Sky Corridor areas
                            else if (dataLoader.model.AreaID() == 391 || dataLoader.model.AreaID() == 392 || dataLoader.model.AreaID() == 394 || dataLoader.model.AreaID() == 415 || dataLoader.model.AreaID() == 416)
                            {
                                objectiveImage = dataLoader.model.GetAreaIconFromID(dataLoader.model.AreaID());

                            }
                            //Duremudira Doors
                            else if (dataLoader.model.AreaID() == 399 || dataLoader.model.AreaID() == 414)
                            {
                                objectiveImage = dataLoader.model.GetAreaIconFromID(dataLoader.model.AreaID());
                            }
                            //Duremudira Arena
                            else if (dataLoader.model.AreaID() == 398)
                            {
                                objectiveImage = dataLoader.model.getMonsterIcon(dataLoader.model.LargeMonster1ID());
                            }
                            //Hunter's Road Base Camp
                            else if (dataLoader.model.AreaID() == 459)
                            {
                                objectiveImage = dataLoader.model.GetAreaIconFromID(dataLoader.model.AreaID());
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
                            Dictionary<int, Dictionary<int, int>> hitsTakenBlockedDictionary = dataLoader.model.hitsTakenBlockedDictionary;
                            Dictionary<int, double> hitsTakenBlockedPerSecondDictionary = dataLoader.model.hitsTakenBlockedPerSecondDictionary;
                            Dictionary<int, int> playerHPDictionary = dataLoader.model.playerHPDictionary;
                            Dictionary<int, int> playerStaminaDictionary = dataLoader.model.playerStaminaDictionary;
                            Dictionary<int, string> keystrokesDictionary = dataLoader.model.keystrokesDictionary;
                            Dictionary<int, string> mouseInputDictionary = dataLoader.model.mouseInputDictionary;
                            Dictionary<int, string> gamepadInputDictionary = dataLoader.model.gamepadInputDictionary;
                            Dictionary<int, double> actionsPerMinuteDictionary = dataLoader.model.actionsPerMinuteDictionary;
                            Dictionary<int, string> overlayModeDictionary = dataLoader.model.overlayModeDictionary;
                            //check if its grabbing a TimeDefInt from a previous quest
                            //TODO is this enough?
                            if ((overlayModeDictionary.Count == 2 && overlayModeDictionary.Last().Value == "") ||
                                (overlayModeDictionary.Count == 1 && overlayModeDictionary.First().Value == "") ||
                                overlayModeDictionary.Count > 2)
                                actualOverlayMode = "Standard";
                            else
                            {
                                //TODO: test
                                if (overlayModeDictionary.Count == 2 && overlayModeDictionary.First().Value == "")
                                    actualOverlayMode = overlayModeDictionary.Last().Value;
                                else
                                    actualOverlayMode = overlayModeDictionary.First().Value;

                                actualOverlayMode = actualOverlayMode.Replace(")", "");
                                actualOverlayMode = actualOverlayMode.Replace("(", "");
                                actualOverlayMode = actualOverlayMode.Trim();
                            }

                            int partySize = dataLoader.model.PartySize();

                            Dictionary<int, Dictionary<int, int>> monster1HPDictionary = dataLoader.model.monster1HPDictionary;
                            Dictionary<int, Dictionary<int, int>> monster2HPDictionary = dataLoader.model.monster2HPDictionary;
                            Dictionary<int, Dictionary<int, int>> monster3HPDictionary = dataLoader.model.monster3HPDictionary;
                            Dictionary<int, Dictionary<int, int>> monster4HPDictionary = dataLoader.model.monster4HPDictionary;

                            Dictionary<int, Dictionary<int, double>> monster1AttackMultiplierDictionary = dataLoader.model.monster1AttackMultiplierDictionary;
                            Dictionary<int, Dictionary<int, double>> monster1DefenseRateDictionary = dataLoader.model.monster1DefenseRateDictionary;
                            Dictionary<int, Dictionary<int, double>> monster1SizeMultiplierDictionary = dataLoader.model.monster1SizeMultiplierDictionary;
                            Dictionary<int, Dictionary<int, int>> monster1PoisonThresholdDictionary = dataLoader.model.monster1PoisonThresholdDictionary;
                            Dictionary<int, Dictionary<int, int>> monster1SleepThresholdDictionary = dataLoader.model.monster1SleepThresholdDictionary;
                            Dictionary<int, Dictionary<int, int>> monster1ParalysisThresholdDictionary = dataLoader.model.monster1ParalysisThresholdDictionary;
                            Dictionary<int, Dictionary<int, int>> monster1BlastThresholdDictionary = dataLoader.model.monster1BlastThresholdDictionary;
                            Dictionary<int, Dictionary<int, int>> monster1StunThresholdDictionary = dataLoader.model.monster1StunThresholdDictionary;

                            int isHighGradeEdition = dataLoader.isHighGradeEdition ? 1 : 0;
                            int refreshRate = s.RefreshRate;

                            string questData = string.Format(
                                "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}{33}{34}{35}{36}{37}{38}{39}{40}{41}{42}{43}{44}{45}",
                                runID, createdAt, createdBy, questID, timeLeft,
                                finalTimeValue, finalTimeDisplay, objectiveImage, objectiveTypeID, objectiveQuantity,
                                starGrade, rankName, objectiveName, date, attackBuffDictionary,
                                hitCountDictionary, hitsPerSecondDictionary, damageDealtDictionary, damagePerSecondDictionary, areaChangesDictionary,
                                cartsDictionary, hitsTakenBlockedDictionary, hitsTakenBlockedPerSecondDictionary, playerHPDictionary, playerStaminaDictionary,
                                keystrokesDictionary, mouseInputDictionary, gamepadInputDictionary, actionsPerMinuteDictionary, overlayModeDictionary,
                                actualOverlayMode, partySize, monster1HPDictionary, monster2HPDictionary, monster3HPDictionary,
                                monster4HPDictionary, monster1AttackMultiplierDictionary, monster1DefenseRateDictionary, monster1SizeMultiplierDictionary, monster1PoisonThresholdDictionary,
                                monster1SleepThresholdDictionary, monster1ParalysisThresholdDictionary, monster1BlastThresholdDictionary, monster1StunThresholdDictionary, isHighGradeEdition,
                                refreshRate
                                );

                            // Calculate the hash value for the data in the row
                            string questHash = CalculateStringHash(questData); // concatenate the relevant data from the row

                            cmd.Parameters.AddWithValue("@QuestHash", questHash);
                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@QuestID", questID);
                            cmd.Parameters.AddWithValue("@TimeLeft", timeLeft);
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
                            cmd.Parameters.AddWithValue("@HitsTakenBlockedDictionary", JsonConvert.SerializeObject(hitsTakenBlockedDictionary));
                            cmd.Parameters.AddWithValue("@HitsTakenBlockedPerSecondDictionary", JsonConvert.SerializeObject(hitsTakenBlockedPerSecondDictionary));
                            cmd.Parameters.AddWithValue("@PlayerHPDictionary", JsonConvert.SerializeObject(playerHPDictionary));
                            cmd.Parameters.AddWithValue("@PlayerStaminaDictionary", JsonConvert.SerializeObject(playerStaminaDictionary));
                            cmd.Parameters.AddWithValue("@KeystrokesDictionary", JsonConvert.SerializeObject(keystrokesDictionary));
                            cmd.Parameters.AddWithValue("@MouseInputDictionary", JsonConvert.SerializeObject(mouseInputDictionary));
                            cmd.Parameters.AddWithValue("@GamepadInputDictionary", JsonConvert.SerializeObject(gamepadInputDictionary));
                            cmd.Parameters.AddWithValue("@ActionsPerMinuteDictionary", JsonConvert.SerializeObject(actionsPerMinuteDictionary));
                            cmd.Parameters.AddWithValue("@OverlayModeDictionary", JsonConvert.SerializeObject(overlayModeDictionary));
                            cmd.Parameters.AddWithValue("@ActualOverlayMode", actualOverlayMode);
                            cmd.Parameters.AddWithValue("@PartySize", partySize);
                            cmd.Parameters.AddWithValue("@Monster1HPDictionary", JsonConvert.SerializeObject(monster1HPDictionary));
                            cmd.Parameters.AddWithValue("@Monster2HPDictionary", JsonConvert.SerializeObject(monster2HPDictionary));
                            cmd.Parameters.AddWithValue("@Monster3HPDictionary", JsonConvert.SerializeObject(monster3HPDictionary));
                            cmd.Parameters.AddWithValue("@Monster4HPDictionary", JsonConvert.SerializeObject(monster4HPDictionary));
                            cmd.Parameters.AddWithValue("@Monster1AttackMultiplierDictionary", JsonConvert.SerializeObject(monster1AttackMultiplierDictionary));
                            cmd.Parameters.AddWithValue("@Monster1DefenseRateDictionary", JsonConvert.SerializeObject(monster1DefenseRateDictionary));
                            cmd.Parameters.AddWithValue("@Monster1SizeMultiplierDictionary", JsonConvert.SerializeObject(monster1SizeMultiplierDictionary));
                            cmd.Parameters.AddWithValue("@Monster1PoisonThresholdDictionary", JsonConvert.SerializeObject(monster1PoisonThresholdDictionary));
                            cmd.Parameters.AddWithValue("@Monster1SleepThresholdDictionary", JsonConvert.SerializeObject(monster1SleepThresholdDictionary));
                            cmd.Parameters.AddWithValue("@Monster1ParalysisThresholdDictionary", JsonConvert.SerializeObject(monster1ParalysisThresholdDictionary));
                            cmd.Parameters.AddWithValue("@Monster1BlastThresholdDictionary", JsonConvert.SerializeObject(monster1BlastThresholdDictionary));
                            cmd.Parameters.AddWithValue("@Monster1StunThresholdDictionary", JsonConvert.SerializeObject(monster1StunThresholdDictionary));
                            cmd.Parameters.AddWithValue("@IsHighGradeEdition", isHighGradeEdition);
                            cmd.Parameters.AddWithValue("@RefreshRate", refreshRate);

                            cmd.ExecuteNonQuery();
                        }

                        logger.Debug("Inserted into Quests table");

                        sql = "SELECT LAST_INSERT_ROWID()";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            runID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        if (dataLoader.model.PartySize() == 1)
                        {
                            long personalBest = 0;
                            int questID = dataLoader.model.QuestID();
                            int weaponType = dataLoader.model.WeaponType();
                            bool improvedPersonalBest = false;

                            using (SQLiteCommand cmd = new SQLiteCommand(
                            @"SELECT 
                                TimeLeft, 
                                FinalTimeValue,
                                FinalTimeDisplay,
                                ActualOverlayMode,
                                pg.WeaponTypeID
                            FROM 
                                Quests q
                            JOIN
                                PlayerGear pg ON q.RunID = pg.RunID
                            WHERE 
                                QuestID = @questID
                                AND pg.WeaponTypeID = @weaponTypeID
                                AND ActualOverlayMode = @category
                                AND PartySize = 1
                            ORDER BY 
                                FinalTimeValue ASC
                            LIMIT 1", conn))
                            {
                                cmd.Parameters.AddWithValue("@questID", questID);
                                cmd.Parameters.AddWithValue("@weaponTypeID", weaponType);
                                cmd.Parameters.AddWithValue("@category", actualOverlayMode);

                                var reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    long time = 0;
                                    time = reader.GetInt64(reader.GetOrdinal("FinalTimeValue"));
                                    personalBest = time;
                                }
                            }

                            sql = @"INSERT INTO PersonalBests(
                            RunID,
                            Attempts
                            ) VALUES (
                            @RunID,
                            @Attempts)";
                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                int finalTimeValue = model.TimeDefInt() - model.TimeInt();
                                if (finalTimeValue < personalBest || personalBest == 0)
                                {
                                    improvedPersonalBest = true;
                                    cmd.Parameters.AddWithValue("@RunID", runID);
                                    cmd.Parameters.AddWithValue("@Attempts", attempts);
                                    // Execute the stored procedure
                                    cmd.ExecuteNonQuery();
                                    logger.Debug("Inserted into PersonalBests table");
                                }
                            }

                            if (improvedPersonalBest)
                            {
                                sql = @"UPDATE PersonalBestAttempts 
                                    SET 
                                        Attempts = 0 
                                    WHERE 
                                        (QuestID, WeaponTypeID, ActualOverlayMode) = (@QuestID, @WeaponTypeID, @ActualOverlayMode)";
                                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                                {
                                    cmd.Parameters.AddWithValue("@QuestID", questID);
                                    cmd.Parameters.AddWithValue("@WeaponTypeID", weaponType);
                                    cmd.Parameters.AddWithValue("@ActualOverlayMode", actualOverlayMode);
                                    // Execute the stored procedure
                                    cmd.ExecuteNonQuery();
                                    logger.Debug("Updated PersonalBestAttempts table");
                                }
                            }
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
                        mhfohddllHash,
                        mhfexeHash
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
                        @mhfohddllHash,
                        @mhfexeHash)";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            string gameFolderPath = s.GameFolderPath;
                            string mhfdatHash = CalculateFileHash(gameFolderPath, @"\dat\mhfdat.bin");
                            string mhfemdHash = CalculateFileHash(gameFolderPath, @"\dat\mhfemd.bin");
                            string mhfinfHash = CalculateFileHash(gameFolderPath, @"\dat\mhfinf.bin");
                            string mhfsqdHash = CalculateFileHash(gameFolderPath, @"\dat\mhfsqd.bin");
                            string mhfodllHash = CalculateFileHash(gameFolderPath, @"\mhfo.dll");
                            string mhfohddllHash = CalculateFileHash(gameFolderPath, @"\mhfo-hd.dll");
                            string mhfexeHash = CalculateFileHash(gameFolderPath, @"\mhf.exe");

                            string gameFolderData = string.Format(
                                "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                createdAt, createdBy, runID,
                                gameFolderPath, mhfdatHash, mhfemdHash,
                                mhfinfHash, mhfsqdHash, mhfodllHash,
                                mhfohddllHash, mhfexeHash);
                            string gameFolderHash = CalculateStringHash(gameFolderData);

                            cmd.Parameters.AddWithValue("@GameFolderHash", gameFolderHash);
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
                            cmd.Parameters.AddWithValue("@mhfexeHash", mhfexeHash);

                            cmd.ExecuteNonQuery();
                        }

                        logger.Debug("Inserted into GameFolder table");

                        InsertPlayerDictionaryDataIntoTable(conn, dataLoader);

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

                        logger.Debug("Inserted into ZenithSkills table");

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

                        logger.Debug("Inserted into AutomaticSkills table");

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

                        logger.Debug("Inserted into ActiveSkills table");

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

                        logger.Debug("Inserted into CaravanSkills table");

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

                        logger.Debug("Inserted into StyleRankSkills table");

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

                        logger.Debug("Inserted into PlayerInventory table");

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

                        logger.Debug("Inserted into AmmoPouch table");

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int ammoPouchID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            ammoPouchID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO PartnyaBag (
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
                            int item1ID = model.PartnyaBagItem1IDAtQuestStart;
                            int item1Quantity = model.PartnyaBagItem1QuantityAtQuestStart;
                            int item2ID = model.PartnyaBagItem2IDAtQuestStart;
                            int item2Quantity = model.PartnyaBagItem2QuantityAtQuestStart;
                            int item3ID = model.PartnyaBagItem3IDAtQuestStart;
                            int item3Quantity = model.PartnyaBagItem3QuantityAtQuestStart;
                            int item4ID = model.PartnyaBagItem4IDAtQuestStart;
                            int item4Quantity = model.PartnyaBagItem4QuantityAtQuestStart;
                            int item5ID = model.PartnyaBagItem5IDAtQuestStart;
                            int item5Quantity = model.PartnyaBagItem5QuantityAtQuestStart;
                            int item6ID = model.PartnyaBagItem6IDAtQuestStart;
                            int item6Quantity = model.PartnyaBagItem6QuantityAtQuestStart;
                            int item7ID = model.PartnyaBagItem7IDAtQuestStart;
                            int item7Quantity = model.PartnyaBagItem7QuantityAtQuestStart;
                            int item8ID = model.PartnyaBagItem8IDAtQuestStart;
                            int item8Quantity = model.PartnyaBagItem8QuantityAtQuestStart;
                            int item9ID = model.PartnyaBagItem9IDAtQuestStart;
                            int item9Quantity = model.PartnyaBagItem9QuantityAtQuestStart;
                            int item10ID = model.PartnyaBagItem10IDAtQuestStart;
                            int item10Quantity = model.PartnyaBagItem10QuantityAtQuestStart;

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

                        logger.Debug("Inserted into PartnyaBag table");

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int partnyaBagID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            partnyaBagID = Convert.ToInt32(cmd.ExecuteScalar());
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

                        logger.Debug("Inserted into RoadDureSkills table");

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

                        Dictionary<int, List<Dictionary<int, int>>> playerInventoryDictionary = dataLoader.model.playerInventoryDictionary;
                        Dictionary<int, List<Dictionary<int, int>>> playerAmmoPouchDictionary = dataLoader.model.playerAmmoPouchDictionary;
                        Dictionary<int, List<Dictionary<int, int>>> partnyaBagDictionary = dataLoader.model.partnyaBagDictionary;

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
                        PartnyaBagID,
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
                        @PartnyaBagID,
                        @PoogieItemID,-- INTEGER NOT NULL,
                        @RoadDureSkillsID,-- INTEGER NOT NULL,
                        @PlayerInventoryDictionary,-- TEXT NOT NULL,
                        @PlayerAmmoPouchDictionary,-- TEXT NOT NULL,
                        @PartnyaBagDictionary-- TEXT NOT NULL,
                        )";

                        string playerGearData = string.Format(
                            "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}{33}{34}{35}{36}{37}{38}{39}{40}{41}{42}{43}{44}{45}{46}{47}{48}{49}{50}",
                            createdAt, createdBy, runID, playerID, gearName,
                            styleID, weaponIconID, weaponClassID, weaponTypeID, blademasterWeaponID,
                            gunnerWeaponID, weaponSlot1, weaponSlot2, weaponSlot3, headID,
                            headSlot1, headSlot2, headSlot3, chestID, chestSlot1,
                            chestSlot2, chestSlot3, armsID, armsSlot1, armsSlot2,
                            armsSlot3, waistID, waistSlot1, waistSlot2, waistSlot3,
                            legsID, legsSlot1, legsSlot2, legsSlot3, cuffSlot1,
                            cuffSlot2, zenithSkillsID, automaticSkillsID, activeSkillsID, caravanSkillsID,
                            divaSkillID, guildFoodID, styleRankSkillsID, playerInventoryID, ammoPouchID,
                            partnyaBagID, poogieItemID, roadDureSkillsID, playerInventoryDictionary, playerAmmoPouchDictionary,
                            partnyaBagDictionary
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
                            cmd.Parameters.AddWithValue("@PartnyaBagID", partnyaBagID);
                            cmd.Parameters.AddWithValue("@PoogieItemID", poogieItemID);
                            cmd.Parameters.AddWithValue("@RoadDureSkillsID", roadDureSkillsID);
                            cmd.Parameters.AddWithValue("@PlayerInventoryDictionary", JsonConvert.SerializeObject(playerInventoryDictionary));
                            cmd.Parameters.AddWithValue("@PlayerAmmoPouchDictionary", JsonConvert.SerializeObject(playerAmmoPouchDictionary));
                            cmd.Parameters.AddWithValue("@PartnyaBagDictionary", JsonConvert.SerializeObject(partnyaBagDictionary));

                            // Execute the stored procedure
                            cmd.ExecuteNonQuery();

                            logger.Debug("Inserted into PlayerGear table");
                        }
                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (SQLiteException ex)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                        // Handle a SQL exception
                        logger.Error(ex, "An error occurred while accessing the database");
                        MessageBox.Show("An error occurred while accessing the database: " + ex.SqlState + "\n\n" + ex.HelpLink + "\n\n" + ex.ResultCode + "\n\n" + ex.ErrorCode + "\n\n" + ex.Source + "\n\n" + ex.StackTrace + "\n\n" + ex.Message, LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (IOException ex)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                        // Handle an I/O exception
                        logger.Error(ex, "An error occurred while accessing a file");
                        MessageBox.Show("An error occurred while accessing a file: " + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (ArgumentException ex)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                        logger.Error(ex, "ArgumentException");
                        MessageBox.Show("ArgumentException " + ex.ParamName + "\n\n" + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        HandleError(transaction, ex);
                    }
                }
            }

            dataLoader.model.ShowSaveIcon = false;
            return runID;
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
                    // overlay
                    // bingo
                    // mezfesminigames
                    // mezfes
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
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_overlay_updates
                        AFTER UPDATE ON Overlay
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_overlay_deletion
                        AFTER DELETE ON Overlay
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_bingo_updates
                        AFTER UPDATE ON Bingo
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_bingo_deletion
                        AFTER DELETE ON Bingo
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_mezfesminigames_updates
                        AFTER UPDATE ON MezFesMinigames
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_mezfesminigames_deletion
                        AFTER DELETE ON MezFesMinigames
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_mezfes_updates
                        AFTER UPDATE ON MezFes
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_mezfes_deletion
                        AFTER DELETE ON MezFes
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

                    using (var cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_quest_updates
                        AFTER UPDATE ON Quests
                        FOR EACH ROW
                        WHEN NEW.YoutubeID = OLD.YoutubeID
                        BEGIN
                            SELECT RAISE(ABORT, 'Cannot update quest fields');
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

        // TODO optimization
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

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="ex">The ex.</param>
        private void HandleError(SQLiteTransaction? transaction, Exception ex)
        {
            var serverVersion = "";

            // Roll back the transaction
            if (transaction != null)
            {
                serverVersion = transaction.Connection.ServerVersion;
                transaction.Rollback();
            }

            // Handle the exception and show an error message to the user
            LoggingManager.WriteCrashLog(ex, $"SQLite error (version: {serverVersion})");
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

        /// <summary>
        /// Stores the overlay hash.
        /// </summary>
        /// <returns></returns>
        public string StoreOverlayHash()
        {
            string overlayHash = "";

            // Find the path of the first found process with the name "MHFZ_Overlay.exe"
            string exeName = "MHFZ_Overlay.exe";
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(exeName));
            string exePath = "";
            if (processes.Length > 0)
            {
                exePath = processes[0].MainModule.FileName;
            }
            else
            {
                exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exeName);
            }

            // Calculate the SHA256 hash of the executable
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(exePath))
                {
                    byte[] hash = sha256.ComputeHash(stream);
                    string hashString = BitConverter.ToString(hash).Replace("-", "");
                    overlayHash = hashString;

                    // Store the hash in the "Overlay" table of the SQLite database
                    using (SQLiteConnection conn = new SQLiteConnection(dataSource))
                    {
                        conn.Open();
                        using (SQLiteTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                string sql = "INSERT INTO Overlay (Hash) VALUES (@hash)";
                                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                                {
                                    cmd.Parameters.AddWithValue("@hash", hashString);
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
                }
            }

            logger.Info("Stored overlay hash {0}", overlayHash);
            return overlayHash;
        }

        #region session time

        public DateTime DatabaseStartTime = DateTime.Now;

        /// <summary>
        /// Stores the session time.
        /// </summary>
        /// <param name="window">The window.</param>
        public void StoreSessionTime(DateTime ProgramStart)
        {
            try
            {
                DateTime ProgramEnd = DateTime.Now;
                TimeSpan duration = ProgramEnd - ProgramStart;
                int sessionDuration = (int)duration.TotalSeconds;

                using (SQLiteConnection connection = new SQLiteConnection(dataSource))
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

                            logger.Info("Stored session time. Duration: {0}", TimeSpan.FromSeconds(sessionDuration).ToString(@"hh\:mm\:ss\.ff"));
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
                logger.Error(ex, "An error occurred while accessing the database");
                MessageBox.Show("An error occurred while accessing the database: " + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                // Handle an I/O exception
                logger.Error(ex, "An error occurred while accessing a file");
                MessageBox.Show("An error occurred while accessing a file: " + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Handle any other exception
                logger.Error(ex, "An error occurred");
                MessageBox.Show("An error occurred: " + ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Source + "\n\n" + ex.Data.ToString(), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
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
            "AllDivaSkills",
            "MezFesMinigames"};

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
                                        cmd2.CommandText = "SELECT tbl_name FROM sqlite_master WHERE name=@name";
                                        cmd2.Parameters.AddWithValue("@name", objectName);

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
                                        cmd3.CommandText = "SELECT tbl_name FROM sqlite_master WHERE name=@name";
                                        cmd3.Parameters.AddWithValue("@name", objectName);

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
                var referenceSchemaFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\reference_schema.json");
                // Write the JSON string to the reference schema file
                FileManager.WriteToFile(referenceSchemaFilePath, json);
            }

            return schema;
        }

        public bool schemaChanged = false;

        /// <summary>
        /// Compares the dictionaries.
        /// </summary>
        /// <param name="dict1">The dict1.</param>
        /// <param name="dict2">The dict2.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Compares the database schemas.
        /// </summary>
        /// <param name="referenceSchema">The reference schema.</param>
        /// <param name="currentSchema">The current schema.</param>
        /// <returns></returns>
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
                logger.Error("Invalid database schema");
                MessageBox.Show(
@"The database schema got updated in the latest version. 

Please make sure that both MHFZ_Overlay.sqlite (in the game\database directory) and reference_schema.json (in the current overlay directory) don't exist, so that the program can make new ones. 

Disabling Quest Logging.",
                LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                s.EnableQuestLogging = false;
            }

            return schemaChanged;
        }

        /// <summary>
        /// Inserts the player dictionary data into table.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="dataLoader">The data loader.</param>
        private void InsertPlayerDictionaryDataIntoTable(SQLiteConnection conn, DataLoader dataLoader)
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
                        CreationDate,
                        PlayerName,
                        GuildName,
                        DiscordServerID,
                        Gender,
                        Nationality
                        ) VALUES (
                        @PlayerID, 
                        @CreationDate,
                        @PlayerName,
                        @GuildName,
                        @DiscordServerID,
                        @Gender,
                        @Nationality)";

                        // Add the parameter placeholders
                        cmd.Parameters.Add("@PlayerID", DbType.Int32);
                        cmd.Parameters.Add("@CreationDate", DbType.String);
                        cmd.Parameters.Add("@PlayerName", DbType.String);
                        cmd.Parameters.Add("@GuildName", DbType.String);
                        cmd.Parameters.Add("@DiscordServerID", DbType.Int64);
                        cmd.Parameters.Add("@Gender", DbType.String);
                        cmd.Parameters.Add("@Nationality", DbType.String);

                        // Iterate through the list of players
                        foreach (KeyValuePair<int, List<string>> kvp in PlayersList.PlayerIDs)
                        {
                            int playerID = kvp.Key;
                            List<string> playerInfo = kvp.Value;
                            string creationDate = playerInfo[0];
                            string playerName = playerInfo[1];
                            string guildName = playerInfo[2];
                            long discordServerID = int.Parse(playerInfo[3]);
                            string gender = playerInfo[4];
                            string nationality = playerInfo[5];

                            if (playerID == 1 && (startTime == DateTime.UnixEpoch || startTime == DateTime.MinValue))
                                creationDate = DateTime.Now.Date.ToString();
                            else
                                creationDate = startTime.Date.ToString();

                            if (playerID == 1)
                            {
                                Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
                                playerName = s.HunterName;
                                guildName = s.GuildName;
                                discordServerID = s.DiscordServerID;
                                gender = s.GenderExport;
                                //TODO test
                                nationality = s.EnableNationality ? dataLoader.model.Countries.ToList()[s.PlayerNationalityIndex].Name.Common : "World";
                            }

                            // Set the parameter values
                            cmd.Parameters["@PlayerID"].Value = playerID;
                            cmd.Parameters["@CreationDate"].Value = creationDate;
                            cmd.Parameters["@PlayerName"].Value = playerName;
                            cmd.Parameters["@GuildName"].Value = guildName;
                            cmd.Parameters["@DiscordServerID"].Value = discordServerID;
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
            logger.Debug("Inserted into Players table");
        }

        /// <summary>
        /// Inserts the dictionary data into table. https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/bulk-insert
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="idColumn">The identifier column.</param>
        /// <param name="valueColumn">The value column.</param>
        /// <param name="conn">The connection.</param>
        /// <exception cref="ArgumentException">
        /// Invalid table name: {tableName}
        /// or
        /// Invalid dictionary: {dictionary}
        /// or
        /// Invalid table name, id column, or value column
        /// or
        /// Invalid connection
        /// </exception>
        /// <exception cref="InvalidOperationException">Connection is not open</exception>
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
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL,
                    SessionDuration INTEGER NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS Audit(
                    AuditID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CreatedAt TEXT NOT NULL,
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
                    QuestHash TEXT NOT NULL DEFAULT '',
                    CreatedAt TEXT NOT NULL DEFAULT '',
                    CreatedBy TEXT NOT NULL DEFAULT '',
                    RunID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    QuestID INTEGER NOT NULL CHECK (QuestID >= 0) DEFAULT 0, 
                    TimeLeft INTEGER NOT NULL DEFAULT 0,
                    FinalTimeValue INTEGER NOT NULL DEFAULT 0,
                    FinalTimeDisplay TEXT NOT NULL DEFAULT '', 
                    ObjectiveImage TEXT NOT NULL DEFAULT '',
                    ObjectiveTypeID INTEGER NOT NULL CHECK (ObjectiveTypeID >= 0) DEFAULT 0, 
                    ObjectiveQuantity INTEGER NOT NULL DEFAULT 0, 
                    StarGrade INTEGER NOT NULL DEFAULT 0, 
                    RankName TEXT NOT NULL DEFAULT '', 
                    ObjectiveName TEXT NOT NULL DEFAULT '', 
                    Date TEXT NOT NULL DEFAULT '',
                    YouTubeID TEXT DEFAULT 'dQw4w9WgXcQ', -- default value for YouTubeID is a Rick Roll video
                    -- DpsData TEXT NOT NULL DEFAULT '',
                    AttackBuffDictionary TEXT NOT NULL DEFAULT '{}',
                    HitCountDictionary TEXT NOT NULL DEFAULT '{}',
                    HitsPerSecondDictionary TEXT NOT NULL DEFAULT '{}',
                    DamageDealtDictionary TEXT NOT NULL DEFAULT '{}',
                    DamagePerSecondDictionary TEXT NOT NULL DEFAULT '{}',
                    AreaChangesDictionary TEXT NOT NULL DEFAULT '{}',
                    CartsDictionary TEXT NOT NULL DEFAULT '{}',
                    HitsTakenBlockedDictionary TEXT NOT NULL DEFAULT '{}',
                    HitsTakenBlockedPerSecondDictionary TEXT NOT NULL DEFAULT '{}',
                    PlayerHPDictionary TEXT NOT NULL DEFAULT '{}',
                    PlayerStaminaDictionary TEXT NOT NULL DEFAULT '{}',
                    KeystrokesDictionary TEXT NOT NULL DEFAULT '{}',
                    MouseInputDictionary TEXT NOT NULL DEFAULT '{}',
                    GamepadInputDictionary TEXT NOT NULL DEFAULT '{}',
                    ActionsPerMinuteDictionary TEXT NOT NULL DEFAULT '{}',
                    OverlayModeDictionary TEXT NOT NULL DEFAULT '{}',
                    ActualOverlayMode TEXT NOT NULL DEFAULT 'Standard',
                    PartySize INTEGER NOT NULL DEFAULT 0,
                    Monster1HPDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster2HPDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster3HPDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster4HPDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1AttackMultiplierDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1DefenseRateDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1SizeMultiplierDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1PoisonThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1SleepThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1ParalysisThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1BlastThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1StunThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    IsHighGradeEdition INTEGER NOT NULL CHECK (IsHighGradeEdition IN (0, 1)) DEFAULT 0,
                    RefreshRate INTEGER NOT NULL CHECK (RefreshRate IN (1,30)) DEFAULT 30,
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

                    InsertDictionaryDataIntoTable(RankBand.IDName, "RankName", "RankNameID", "RankNameName", conn);

                    //Create the ObjectiveTypes table
                    sql = @"CREATE TABLE IF NOT EXISTS ObjectiveType
                    (ObjectiveTypeID INTEGER PRIMARY KEY, 
                    ObjectiveTypeName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(ObjectiveType.IDName, "ObjectiveType", "ObjectiveTypeID", "ObjectiveTypeName", conn);

                    //Create the QuestNames table
                    sql = @"CREATE TABLE IF NOT EXISTS QuestName
                    (QuestNameID INTEGER PRIMARY KEY, 
                    QuestNameName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(EZlion.Mapper.Quest.IDName, "QuestName", "QuestNameID", "QuestNameName", conn);

                    // Create the Players table
                    //do an UPDATE when inserting quests. since its just local player?
                    sql = @"
                    CREATE TABLE IF NOT EXISTS Players (
                    PlayerID INTEGER PRIMARY KEY, 
                    CreationDate DATE NOT NULL,
                    PlayerName TEXT NOT NULL,
                    GuildName TEXT NOT NULL,
                    DiscordServerID INTEGER NOT NULL,
                    Gender TEXT NOT NULL,
                    Nationality TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertPlayerDictionaryDataIntoTable(conn, dataLoader);

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
                    CreatedAt TEXT NOT NULL,
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
                    mhfexeHash TEXT NOT NULL,
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

                    InsertDictionaryDataIntoTable(WeaponType.IDName, "WeaponType", "WeaponTypeID", "WeaponTypeName", conn);

                    // Create the Item table
                    sql = @"CREATE TABLE IF NOT EXISTS Item (
                    ItemID INTEGER PRIMARY KEY, 
                    ItemName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Item.IDName, "Item", "ItemID", "ItemName", conn);

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
                    CreatedAt TEXT NOT NULL,
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
                    PartnyaBagID INTEGER NOT NULL,
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
                    FOREIGN KEY(PartnyaBagID) REFERENCES PartnyaBag(PartnyaBagID),
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

                    InsertDictionaryDataIntoTable(SkillDiva.IDName, "AllDivaSkills", "DivaSkillID", "DivaSkillName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS WeaponStyles (
                      StyleID INTEGER PRIMARY KEY,
                      StyleName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(WeaponStyle.IDName, "WeaponStyles", "StyleID", "StyleName", conn);

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

                    InsertDictionaryDataIntoTable(WeaponClass.IDName, "WeaponClass", "WeaponClassID", "WeaponClassName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllBlademasterWeapons (
                      BlademasterWeaponID INTEGER PRIMARY KEY,
                      BlademasterWeaponName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(WeaponBlademaster.IDName, "AllBlademasterWeapons", "BlademasterWeaponID", "BlademasterWeaponName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllGunnerWeapons (
                      GunnerWeaponID INTEGER PRIMARY KEY,
                      GunnerWeaponName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(WeaponGunner.IDName, "AllGunnerWeapons", "GunnerWeaponID", "GunnerWeaponName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllHeadPieces (
                      HeadPieceID INTEGER PRIMARY KEY,
                      HeadPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(ArmorHead.IDName, "AllHeadPieces", "HeadPieceID", "HeadPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllChestPieces (
                      ChestPieceID INTEGER PRIMARY KEY,
                      ChestPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(ArmorChest.IDName, "AllChestPieces", "ChestPieceID", "ChestPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllArmsPieces (
                      ArmsPieceID INTEGER PRIMARY KEY,
                      ArmsPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(ArmorArms.IDName, "AllArmsPieces", "ArmsPieceID", "ArmsPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllWaistPieces (
                      WaistPieceID INTEGER PRIMARY KEY,
                      WaistPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(ArmorWaist.IDName, "AllWaistPieces", "WaistPieceID", "WaistPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AllLegsPieces (
                      LegsPieceID INTEGER PRIMARY KEY,
                      LegsPieceName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(ArmorLegs.IDName, "AllLegsPieces", "LegsPieceID", "LegsPieceName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS ZenithSkills(
                    CreatedAt TEXT NOT NULL,
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

                    InsertDictionaryDataIntoTable(SkillZenith.IDName, "AllZenithSkills", "ZenithSkillID", "ZenithSkillName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS AutomaticSkills(
                    CreatedAt TEXT NOT NULL,
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

                    InsertDictionaryDataIntoTable(SkillArmor.IDName, "AllArmorSkills", "ArmorSkillID", "ArmorSkillName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS ActiveSkills(
                    CreatedAt TEXT NOT NULL,
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
                    CreatedAt TEXT NOT NULL,
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

                    InsertDictionaryDataIntoTable(SkillCaravan.IDName, "AllCaravanSkills", "CaravanSkillID", "CaravanSkillName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS StyleRankSkills(
                    CreatedAt TEXT NOT NULL,
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

                    InsertDictionaryDataIntoTable(SkillStyleRank.IDName, "AllStyleRankSkills", "StyleRankSkillID", "StyleRankSkillName", conn);

                    // Create the PlayerInventory table
                    sql = @"CREATE TABLE IF NOT EXISTS PlayerInventory (
                    CreatedAt TEXT NOT NULL,
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
                    CreatedAt TEXT NOT NULL,
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

                    sql = @"CREATE TABLE IF NOT EXISTS PartnyaBag (
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    PartnyaBagID INTEGER PRIMARY KEY AUTOINCREMENT,
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
                    CreatedAt TEXT NOT NULL,
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

                    InsertDictionaryDataIntoTable(SkillRoadTower.IDName, "AllRoadDureSkills", "RoadDureSkillID", "RoadDureSkillName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS QuestAttempts(
                    QuestAttemptsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    QuestID INTEGER NOT NULL,
                    WeaponTypeID INTEGER NOT NULL,
                    ActualOverlayMode TEXT NOT NULL,
                    Attempts INTEGER NOT NULL,
                    FOREIGN KEY(QuestID) REFERENCES QuestName(QuestNameID),
                    FOREIGN KEY(WeaponTypeID) REFERENCES WeaponType(WeaponTypeID),
                    UNIQUE (QuestID, WeaponTypeID, ActualOverlayMode)
                    )
                    ";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS PersonalBests(
                    PersonalBestsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    Attempts INTEGER NOT NULL,
                    FOREIGN KEY(RunID) REFERENCES Quests(RunID))";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS Overlay(
                    OverlayID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Hash TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS Bingo(
                    BingoID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    Difficulty TEXT NOT NULL,
                    MonsterList TEXT NOT NULL,
                    ElapsedTime TEXT NOT NULL,
                    Score INTEGER NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS MezFesMinigames(
                    MezFesMinigameID INTEGER PRIMARY KEY AUTOINCREMENT,
                    MezFesMinigameName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertDictionaryDataIntoTable(Dictionary.MezFesMinigame.ID, "MezFesMinigames", "MezFesMinigameID", "MezFesMinigameName", conn);

                    sql = @"CREATE TABLE IF NOT EXISTS MezFes(
                    MezFesID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    MezFesMinigameID INTEGER NOT NULL,
                    Score INTEGER NOT NULL,
                    FOREIGN KEY(MezFesMinigameID) REFERENCES MezFesMinigames(MezFesMinigameID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS PersonalBestAttempts(
                    PersonalBestAttemptsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    QuestID INTEGER NOT NULL,
                    WeaponTypeID INTEGER NOT NULL,
                    ActualOverlayMode TEXT NOT NULL,
                    Attempts INTEGER NOT NULL,
                    FOREIGN KEY(QuestID) REFERENCES QuestName(QuestNameID),
                    FOREIGN KEY(WeaponTypeID) REFERENCES WeaponType(WeaponTypeID),
                    UNIQUE (QuestID, WeaponTypeID, ActualOverlayMode)
                    )
                    ";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS ZenithGauntlets(
                        ZenithGauntletID INTEGER PRIMARY KEY AUTOINCREMENT,
                        WeaponType TEXT NOT NULL,
                        Category TEXT NOT NULL,
                        TotalFramesElapsed INTEGER NOT NULL,
                        TotalTimeElapsed TEXT NOT NULL,
                        Run1ID INTEGER NOT NULL,
                        Run2ID INTEGER NOT NULL,
                        Run3ID INTEGER NOT NULL,
                        Run4ID INTEGER NOT NULL,
                        Run5ID INTEGER NOT NULL,
                        Run6ID INTEGER NOT NULL,
                        Run7ID INTEGER NOT NULL,
                        Run8ID INTEGER NOT NULL,
                        Run9ID INTEGER NOT NULL,
                        Run10ID INTEGER NOT NULL,
                        Run11ID INTEGER NOT NULL,
                        Run12ID INTEGER NOT NULL,
                        Run13ID INTEGER NOT NULL,
                        Run14ID INTEGER NOT NULL,
                        Run15ID INTEGER NOT NULL,
                        Run16ID INTEGER NOT NULL,
                        Run17ID INTEGER NOT NULL,
                        Run18ID INTEGER NOT NULL,
                        Run19ID INTEGER NOT NULL,
                        Run20ID INTEGER NOT NULL,
                        Run21ID INTEGER NOT NULL,
                        Run22ID INTEGER NOT NULL,
                        Run23ID INTEGER NOT NULL,
                        FOREIGN KEY(Run1ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run2ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run3ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run4ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run5ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run6ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run7ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run8ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run9ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run10ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run11ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run12ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run13ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run14ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run15ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run16ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run17ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run18ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run19ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run20ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run21ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run22ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run23ID) REFERENCES Quests(RunID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

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

                    sql = @"CREATE TABLE IF NOT EXISTS GachaCard(
                    GachaCardID INTEGER PRIMARY KEY AUTOINCREMENT,
                    GachaCardTypeID INTEGER NOT NULL,
                    GachaCardRarityID INTEGER NOT NULL,
                    GachaCardName INTEGER NOT NULL,
                    GachaCardFrontImage TEXT NOT NULL,
                    GachCardBackImage TEXT NOT NULL,
                    UNIQUE(GachaCardTypeID, GachaCardRarityID, GachaCardName),
                    FOREIGN KEY(GachaCardTypeID) REFERENCES GachaCardType(GachaCardTypeID),
                    FOREIGN KEY(GachaCardRarityID) REFERENCES GachaCardRarity(GachaCardRarityID)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaCardType(
                    GachaCardTypeID INTEGER PRIMARY KEY,
                    GachaCardTypeName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaCardRarity(
                    GachaCardRarityID INTEGER PRIMARY KEY,
                    GachaCardRarityName TEXT NOT NULL
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sql = @"CREATE TABLE IF NOT EXISTS GachaCardInventory(
                    GachaCardInventoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                    GachaCardID INTEGER NOT NULL,
                    FOREIGN KEY(GachaCardID) REFERENCES GachaCard(GachaCardID)
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

        //TODO put somewhere else
        public string FormatTime(int framesElapsed)
        {
            int minutes = framesElapsed / (30 * 60);
            int seconds = (framesElapsed % (30 * 60)) / 30;
            double milliseconds = ((framesElapsed % (30 * 60)) % 30) / 30.0;
            return $"{minutes:D2}:{seconds:D2}.{(int)(milliseconds * 100):D2}";
        }

        public string GetYoutubeLinkForRunID(long runID)
        {
            string youtubeLink = "";
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT YoutubeID FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    youtubeLink = (string)reader["YoutubeID"];
                                }
                                else
                                {
                                    youtubeLink = "";
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
            }

            return "https://youtube.com/watch?v=" + youtubeLink;
        }

        public bool UpdateYoutubeLink(object sender, RoutedEventArgs e, long runID, string youtubeLink)
        {
            bool success = false;
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            long count = (long)cmd.ExecuteScalar();
                            if (count > 0)
                            {
                                using (SQLiteCommand cmd2 = new SQLiteCommand("UPDATE Quests SET YoutubeID = @youtubeLink WHERE RunID = @runID", conn))
                                {
                                    cmd2.Parameters.AddWithValue("@youtubeLink", youtubeLink);
                                    cmd2.Parameters.AddWithValue("@runID", runID);

                                    int rowsAffected = cmd2.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        success = true;
                                    }
                                }
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
            return success;
        }

        public long GetPersonalBestElapsedTimeValue(long questID, int weaponTypeID, string category)
        {
            long personalBest = 0;

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(
                            @"SELECT 
                                TimeLeft, 
                                FinalTimeValue,
                                FinalTimeDisplay,
                                ActualOverlayMode,
                                pg.WeaponTypeID
                            FROM 
                                Quests q
                            JOIN
                                PlayerGear pg ON q.RunID = pg.RunID
                            WHERE 
                                QuestID = @questID
                                AND pg.WeaponTypeID = @weaponTypeID
                                AND ActualOverlayMode = @category
                                AND PartySize = 1
                            ORDER BY 
                                FinalTimeValue ASC
                            LIMIT 1", conn))
                        {
                            cmd.Parameters.AddWithValue("@questID", questID);
                            cmd.Parameters.AddWithValue("@weaponTypeID", weaponTypeID);
                            cmd.Parameters.AddWithValue("@category", category);

                            var reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                long time = 0;
                                time = reader.GetInt64(reader.GetOrdinal("FinalTimeValue"));
                                personalBest = time;
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
            return personalBest;
        }

        public string GetPersonalBest(long questID, int weaponTypeID, string category, string timerMode, DataLoader dataLoader)
        {
            string personalBest = "--:--.--";

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(
                            @"SELECT 
                                TimeLeft, 
                                FinalTimeValue,
                                FinalTimeDisplay,
                                ActualOverlayMode,
                                pg.WeaponTypeID
                            FROM 
                                Quests q
                            JOIN
                                PlayerGear pg ON q.RunID = pg.RunID
                            WHERE 
                                QuestID = @questID
                                AND pg.WeaponTypeID = @weaponTypeID
                                AND ActualOverlayMode = @category
                                AND PartySize = 1
                            ORDER BY 
                                FinalTimeValue ASC
                            LIMIT 1", conn))
                        {
                            cmd.Parameters.AddWithValue("@questID", questID);
                            cmd.Parameters.AddWithValue("@weaponTypeID", weaponTypeID);
                            cmd.Parameters.AddWithValue("@category", category);

                            var reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                long time = 0;
                                if (timerMode == "Elapsed")
                                {
                                    time = reader.GetInt64(reader.GetOrdinal("FinalTimeValue"));
                                }
                                else
                                {
                                    time = reader.GetInt64(reader.GetOrdinal("TimeLeft"));
                                }
                                personalBest = dataLoader.model.GetMinutesSecondsMillisecondsFromFrames(time);
                            }
                            else
                            {
                                personalBest = "--:--.--";
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

            return personalBest;
        }

        public Dictionary<DateTime, long> GetPersonalBestsByDate(long questID, int weaponTypeID, string category)
        {
            Dictionary<DateTime, long> personalBests = new();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(
                            @"SELECT 
                        q.FinalTimeValue, 
                        q.ActualOverlayMode,
                        pg.WeaponTypeID,
                        q.CreatedAt,
                        q.RunID
                    FROM 
                        Quests q
                    JOIN
                        PlayerGear pg ON q.RunID = pg.RunID
                    WHERE 
                        q.QuestID = @questID
                        AND pg.WeaponTypeID = @weaponTypeID
                        AND q.ActualOverlayMode = @category
                        AND q.PartySize = 1
                    ORDER BY 
                        q.CreatedAt ASC"
                        , conn))
                        {
                            cmd.Parameters.AddWithValue("@questID", questID);
                            cmd.Parameters.AddWithValue("@weaponTypeID", weaponTypeID);
                            cmd.Parameters.AddWithValue("@category", category);

                            var reader = cmd.ExecuteReader();
                            Dictionary<DateTime, long> personalBestTimes = new Dictionary<DateTime, long>();

                            while (reader.Read())
                            {
                                long runID = reader.GetInt64(reader.GetOrdinal("RunID"));
                                long time = reader.GetInt64(reader.GetOrdinal("FinalTimeValue"));
                                DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")).Date;

                                if (personalBestTimes.ContainsKey(createdAt))
                                {
                                    long personalBest = personalBestTimes[createdAt];
                                    // Check if current time is faster than previous time for this day
                                    if (time < personalBest)
                                    {
                                        personalBestTimes[createdAt] = time;
                                    }
                                }
                                else
                                {
                                    personalBestTimes[createdAt] = time;
                                }
                            }

                            if (!personalBestTimes.Any())
                                return personalBests;

                            // Populate personalBests dictionary with personal best times by date
                            DateTime currentDate = personalBestTimes.Keys.Min();
                            long currentBest = personalBestTimes[currentDate];
                            personalBests[currentDate] = currentBest;
                            currentDate = currentDate.AddDays(1);

                            while (currentDate <= DateTime.Today)
                            {
                                if (personalBestTimes.TryGetValue(currentDate, out long newBest))
                                {
                                    currentBest = Math.Min(currentBest, newBest);
                                }
                                personalBests[currentDate] = currentBest;
                                currentDate = currentDate.AddDays(1);
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

            return personalBests;
        }

        // Get personal best times by attempts
        public Dictionary<long, long> GetPersonalBestsByAttempts(long questID, int weaponTypeID, string category)
        {
            Dictionary<long, long> personalBests = new();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(
                            @"SELECT 
                                pb.Attempts,
                                q.FinalTimeValue
                            FROM 
                                PersonalBests pb
                            JOIN
                                Quests q ON pb.RunID = q.RunID
                            JOIN
                                PlayerGear pg ON q.RunID = pg.RunID
                            WHERE 
                                q.QuestID = @questID
                                AND pg.WeaponTypeID = @weaponTypeID
                                AND q.ActualOverlayMode = @category
                                AND q.PartySize = 1
                            ORDER BY 
                                pb.Attempts ASC"
                                , conn))
                        {
                            cmd.Parameters.AddWithValue("@questID", questID);
                            cmd.Parameters.AddWithValue("@weaponTypeID", weaponTypeID);
                            cmd.Parameters.AddWithValue("@category", category);

                            var reader = cmd.ExecuteReader();

                            // Store personal best times by attempts
                            while (reader.Read())
                            {
                                long attempts = reader.GetInt64(reader.GetOrdinal("Attempts"));
                                long time = reader.GetInt64(reader.GetOrdinal("FinalTimeValue"));

                                if (personalBests.ContainsKey(attempts))
                                {
                                    long personalBest = personalBests[attempts];
                                    if (time < personalBest)
                                    {
                                        personalBests[attempts] = time;
                                        // Update personal best for all future attempts
                                        for (long i = attempts + 1; i <= personalBests.Keys.Max(); i++)
                                        {
                                            if (personalBests[i] > time)
                                            {
                                                personalBests[i] = time;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    personalBests.Add(attempts, time);
                                }
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

            return personalBests;
        }

        private static object lockObj = new object();

        public int UpsertQuestAttempts(long questID, int weaponTypeID, string category)
        {
            int attempts = 0;

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    int numRetries = 0;
                    bool success = false;
                    while (!success && numRetries < 3)
                    {
                        try
                        {
                            using (SQLiteCommand command = new SQLiteCommand(conn))
                            {
                                command.CommandText =
                                    @"INSERT INTO 
                            QuestAttempts (QuestID, WeaponTypeID, ActualOverlayMode, Attempts)
                        VALUES 
                            (@QuestID, @WeaponTypeID, @ActualOverlayMode, 1)
                        ON CONFLICT 
                            (QuestID, WeaponTypeID, ActualOverlayMode) 
                        DO UPDATE
                        SET 
                            Attempts = Attempts + 1
                        RETURNING 
                            Attempts;";

                                command.Parameters.AddWithValue("@QuestID", questID);
                                command.Parameters.AddWithValue("@WeaponTypeID", weaponTypeID);
                                command.Parameters.AddWithValue("@ActualOverlayMode", category);

                                attempts = Convert.ToInt32(command.ExecuteScalar());
                            }
                            transaction.Commit();
                            success = true;
                        }
                        catch (SQLiteException ex)
                        {
                            if (ex.ResultCode == SQLiteErrorCode.Locked || ex.ResultCode == SQLiteErrorCode.Busy)
                            {
                                // Database is locked, retry after a short delay
                                numRetries++;
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                // Some other error occurred, abort the transaction
                                HandleError(transaction, ex);
                                break;
                            }
                        }
                    }
                }
            }
            return attempts;
        }

        public int UpsertPersonalBestAttempts(long questID, int weaponTypeID, string category)
        {
            int attempts = 0;

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    int numRetries = 0;
                    bool success = false;
                    while (!success && numRetries < 3)
                    {
                        try
                        {
                            using (SQLiteCommand command = new SQLiteCommand(conn))
                            {
                                command.CommandText =
                                    @"INSERT INTO 
                            PersonalBestAttempts (QuestID, WeaponTypeID, ActualOverlayMode, Attempts)
                        VALUES 
                            (@QuestID, @WeaponTypeID, @ActualOverlayMode, 1)
                        ON CONFLICT 
                            (QuestID, WeaponTypeID, ActualOverlayMode) 
                        DO UPDATE
                        SET 
                            Attempts = Attempts + 1
                        RETURNING 
                            Attempts;";

                                command.Parameters.AddWithValue("@QuestID", questID);
                                command.Parameters.AddWithValue("@WeaponTypeID", weaponTypeID);
                                command.Parameters.AddWithValue("@ActualOverlayMode", category);

                                attempts = Convert.ToInt32(command.ExecuteScalar());
                            }
                            transaction.Commit();
                            success = true;
                        }
                        catch (SQLiteException ex)
                        {
                            if (ex.ResultCode == SQLiteErrorCode.Locked || ex.ResultCode == SQLiteErrorCode.Busy)
                            {
                                // Database is locked, retry after a short delay
                                numRetries++;
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                // Some other error occurred, abort the transaction
                                HandleError(transaction, ex);
                                break;
                            }
                        }
                    }
                }
            }
            return attempts;
        }

        public long GetQuestAttempts(long questID, int weaponTypeID, string category)
        {
            long attempts = 0;

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // I'm not taking into account party size, should be fine
                        using (SQLiteCommand cmd = new SQLiteCommand(
                            @"SELECT
                            Attempts
                        FROM 
                            QuestAttempts
                        WHERE 
                            QuestID = @questID
                            AND WeaponTypeID = @weaponTypeID
                            AND ActualOverlayMode = @category", conn))
                        {
                            cmd.Parameters.Add("@questID", DbType.Int64).Value = questID;
                            cmd.Parameters.Add("@weaponTypeID", DbType.Int32).Value = weaponTypeID;
                            cmd.Parameters.Add("@category", DbType.String).Value = category;

                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                attempts = Convert.ToInt64(result);
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

            return attempts;
        }

        public AmmoPouch GetAmmoPouch(long runID)
        {
            AmmoPouch ammoPouch = new AmmoPouch();

            // Use a SQL query to retrieve the AmmoPouch data for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT AmmoPouchID, RunID, Item1ID, Item1Quantity, Item2ID, Item2Quantity, Item3ID, Item3Quantity, Item4ID, Item4Quantity, Item5ID, Item5Quantity, Item6ID, Item6Quantity, Item7ID, Item7Quantity, Item8ID, Item8Quantity, Item9ID, Item9Quantity, Item10ID, Item10Quantity, CreatedAt, CreatedBy FROM AmmoPouch WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    ammoPouch.AmmoPouchID = long.Parse(reader["AmmoPouchID"].ToString());
                                    ammoPouch.RunID = runID;
                                    for (int i = 1; i <= 10; i++)
                                    {
                                        ammoPouch.GetType().GetProperty($"Item{i}ID").SetValue(ammoPouch, long.Parse(reader[$"Item{i}ID"].ToString()));
                                        ammoPouch.GetType().GetProperty($"Item{i}Quantity").SetValue(ammoPouch, long.Parse(reader[$"Item{i}Quantity"].ToString()));
                                    }
                                    ammoPouch.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    ammoPouch.CreatedBy = reader["CreatedBy"].ToString();
                                }
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

            return ammoPouch;
        }

        public PartnyaBag GetPartnyaBag(long runID)
        {
            PartnyaBag partnyaBag = new PartnyaBag();

            // Use a SQL query to retrieve the PartnyaBag data for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT PartnyaBagID, RunID, Item1ID, Item1Quantity, Item2ID, Item2Quantity, Item3ID, Item3Quantity, Item4ID, Item4Quantity, Item5ID, Item5Quantity, Item6ID, Item6Quantity, Item7ID, Item7Quantity, Item8ID, Item8Quantity, Item9ID, Item9Quantity, Item10ID, Item10Quantity, CreatedAt, CreatedBy FROM PartnyaBag WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    partnyaBag.PartnyaBagID = long.Parse(reader["PartnyaBagID"].ToString());
                                    partnyaBag.RunID = runID;
                                    for (int i = 1; i <= 10; i++)
                                    {
                                        partnyaBag.GetType().GetProperty("Item" + i + "ID").SetValue(partnyaBag, long.Parse(reader["Item" + i + "ID"].ToString()));
                                        partnyaBag.GetType().GetProperty("Item" + i + "Quantity").SetValue(partnyaBag, long.Parse(reader["Item" + i + "Quantity"].ToString()));
                                    }
                                    partnyaBag.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    partnyaBag.CreatedBy = reader["CreatedBy"].ToString();
                                }
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
            return partnyaBag;
        }

        public PlayerInventory GetPlayerInventory(long runID)
        {
            PlayerInventory playerInventory = new PlayerInventory();

            // Use a SQL query to retrieve the PlayerInventory for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT PlayerInventoryID, RunID, Item1ID, Item1Quantity, Item2ID, Item2Quantity, Item3ID, Item3Quantity, Item4ID, Item4Quantity, Item5ID, Item5Quantity, Item6ID, Item6Quantity, Item7ID, Item7Quantity, Item8ID, Item8Quantity, Item9ID, Item9Quantity, Item10ID, Item10Quantity, Item11ID, Item11Quantity, Item12ID, Item12Quantity, Item13ID, Item13Quantity, Item14ID, Item14Quantity, Item15ID, Item15Quantity, Item16ID, Item16Quantity, Item17ID, Item17Quantity, Item18ID, Item18Quantity, Item19ID, Item19Quantity, Item20ID, Item20Quantity, CreatedAt, CreatedBy FROM PlayerInventory WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    playerInventory.PlayerInventoryID = long.Parse(reader["PlayerInventoryID"].ToString());
                                    playerInventory.RunID = runID;
                                    for (int i = 1; i <= 20; i++)
                                    {
                                        playerInventory.GetType().GetProperty("Item" + i + "ID").SetValue(playerInventory, long.Parse(reader["Item" + i + "ID"].ToString()));
                                        playerInventory.GetType().GetProperty("Item" + i + "Quantity").SetValue(playerInventory, long.Parse(reader["Item" + i + "Quantity"].ToString()));
                                    }
                                    playerInventory.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    playerInventory.CreatedBy = reader["CreatedBy"].ToString();
                                }
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
            return playerInventory;
        }

        public Quest GetQuest(long runID)
        {
            Quest quest = new Quest();
            // Use a SQL query to retrieve the Quest for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT QuestHash, CreatedAt, CreatedBy, RunID, QuestID, TimeLeft, FinalTimeValue, FinalTimeDisplay, ObjectiveImage, ObjectiveTypeID, ObjectiveQuantity, StarGrade, RankName, ObjectiveName, Date, YouTubeID, AttackBuffDictionary, HitCountDictionary, HitsPerSecondDictionary, DamageDealtDictionary, DamagePerSecondDictionary, AreaChangesDictionary, CartsDictionary, Monster1HPDictionary, Monster2HPDictionary, Monster3HPDictionary, Monster4HPDictionary, HitsTakenBlockedDictionary, HitsTakenBlockedPerSecondDictionary, PlayerHPDictionary, PlayerStaminaDictionary, KeyStrokesDictionary, MouseInputDictionary, GamepadInputDictionary, ActionsPerMinuteDictionary, OverlayModeDictionary, ActualOverlayMode, PartySize FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    quest.QuestHash = reader["QuestHash"].ToString();
                                    quest.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    quest.CreatedBy = reader["CreatedBy"].ToString();
                                    quest.RunID = runID;
                                    quest.QuestID = long.Parse(reader["QuestID"].ToString());
                                    quest.TimeLeft = long.Parse(reader["TimeLeft"].ToString());
                                    quest.FinalTimeValue = long.Parse(reader["FinalTimeValue"].ToString());
                                    quest.FinalTimeDisplay = reader["FinalTimeDisplay"].ToString();
                                    quest.ObjectiveImage = reader["ObjectiveImage"].ToString();
                                    quest.ObjectiveTypeID = long.Parse(reader["ObjectiveTypeID"].ToString());
                                    quest.ObjectiveQuantity = long.Parse(reader["ObjectiveQuantity"].ToString());
                                    quest.StarGrade = long.Parse(reader["StarGrade"].ToString());
                                    quest.RankName = reader["RankName"].ToString();
                                    quest.ObjectiveName = reader["ObjectiveName"].ToString();
                                    quest.Date = DateTime.Parse(reader["Date"].ToString());
                                    quest.YouTubeID = reader["YouTubeID"].ToString();
                                    quest.AttackBuffDictionary = reader["AttackBuffDictionary"].ToString();
                                    quest.HitCountDictionary = reader["HitCountDictionary"].ToString();
                                    quest.HitsPerSecondDictionary = reader["HitsPerSecondDictionary"].ToString();
                                    quest.DamageDealtDictionary = reader["DamageDealtDictionary"].ToString();
                                    quest.DamagePerSecondDictionary = reader["DamagePerSecondDictionary"].ToString();
                                    quest.AreaChangesDictionary = reader["AreaChangesDictionary"].ToString();
                                    quest.CartsDictionary = reader["CartsDictionary"].ToString();

                                    quest.Monster1HPDictionary = reader["Monster1HPDictionary"].ToString();
                                    quest.Monster2HPDictionary = reader["Monster2HPDictionary"].ToString();
                                    quest.Monster3HPDictionary = reader["Monster3HPDictionary"].ToString();
                                    quest.Monster4HPDictionary = reader["Monster4HPDictionary"].ToString();

                                    quest.HitsTakenBlockedDictionary = reader["HitsTakenBlockedDictionary"].ToString();
                                    quest.HitsTakenBlockedPerSecondDictionary = reader["HitsTakenBlockedPerSecondDictionary"].ToString();
                                    quest.PlayerHPDictionary = reader["PlayerHPDictionary"].ToString();
                                    quest.PlayerStaminaDictionary = reader["PlayerStaminaDictionary"].ToString();
                                    quest.KeyStrokesDictionary = reader["KeyStrokesDictionary"].ToString();
                                    quest.MouseInputDictionary = reader["MouseInputDictionary"].ToString();
                                    quest.GamepadInputDictionary = reader["GamepadInputDictionary"].ToString();

                                    quest.ActionsPerMinuteDictionary = reader["ActionsPerMinuteDictionary"].ToString();
                                    quest.OverlayModeDictionary = reader["OverlayModeDictionary"].ToString();
                                    quest.ActualOverlayMode = reader["ActualOverlayMode"].ToString();
                                    quest.PartySize = long.Parse(reader["PartySize"].ToString());
                                }
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
            return quest;
        }

        public UI.Class.RoadDureSkills GetRoadDureSkills(long runID)
        {
            UI.Class.RoadDureSkills roadDureSkills = new UI.Class.RoadDureSkills();

            // Use a SQL query to retrieve the RoadDureSkills data for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT RoadDureSkillsID, RunID, RoadDureSkill1ID, RoadDureSkill1Level, RoadDureSkill2ID, RoadDureSkill2Level, RoadDureSkill3ID, RoadDureSkill3Level, RoadDureSkill4ID, RoadDureSkill4Level, RoadDureSkill5ID, RoadDureSkill5Level, RoadDureSkill6ID, RoadDureSkill6Level, RoadDureSkill7ID, RoadDureSkill7Level, RoadDureSkill8ID, RoadDureSkill8Level, RoadDureSkill9ID, RoadDureSkill9Level, RoadDureSkill10ID, RoadDureSkill10Level, RoadDureSkill11ID, RoadDureSkill11Level, RoadDureSkill12ID, RoadDureSkill12Level, RoadDureSkill13ID, RoadDureSkill13Level, RoadDureSkill14ID, RoadDureSkill14Level, RoadDureSkill15ID, RoadDureSkill15Level, RoadDureSkill16ID, RoadDureSkill16Level, CreatedAt, CreatedBy FROM RoadDureSkills WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    roadDureSkills.RoadDureSkillsID = long.Parse(reader["RoadDureSkillsID"].ToString());
                                    roadDureSkills.RunID = runID;
                                    for (int i = 1; i <= 16; i++)
                                    {
                                        roadDureSkills.GetType().GetProperty("RoadDureSkill" + i + "ID").SetValue(roadDureSkills, long.Parse(reader["RoadDureSkill" + i + "ID"].ToString()));
                                        roadDureSkills.GetType().GetProperty("RoadDureSkill" + i + "Level").SetValue(roadDureSkills, long.Parse(reader["RoadDureSkill" + i + "Level"].ToString()));
                                    }
                                    roadDureSkills.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    roadDureSkills.CreatedBy = reader["CreatedBy"].ToString();
                                }
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
            return roadDureSkills;
        }

        public StyleRankSkills GetStyleRankSkills(long runID)
        {
            StyleRankSkills styleRankSkills = new StyleRankSkills();

            // Use a SQL query to retrieve the StyleRankSkills data for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT StyleRankSkillsID, RunID, StyleRankSkill1ID, StyleRankSkill2ID, CreatedAt, CreatedBy FROM StyleRankSkills WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    styleRankSkills.StyleRankSkillsID = long.Parse(reader["StyleRankSkillsID"].ToString());
                                    styleRankSkills.RunID = runID;
                                    styleRankSkills.StyleRankSkill1ID = long.Parse(reader["StyleRankSkill1ID"].ToString());
                                    styleRankSkills.StyleRankSkill2ID = long.Parse(reader["StyleRankSkill2ID"].ToString());
                                    styleRankSkills.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    styleRankSkills.CreatedBy = reader["CreatedBy"].ToString();
                                }
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
            return styleRankSkills;
        }


        public AutomaticSkills GetAutomaticSkills(long runID)
        {
            AutomaticSkills automaticSkills = new AutomaticSkills();

            // Use a SQL query to retrieve the AutomaticSkills data for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT AutomaticSkillsID, RunID, AutomaticSkill1ID, AutomaticSkill2ID, AutomaticSkill3ID, AutomaticSkill4ID, AutomaticSkill5ID, AutomaticSkill6ID, CreatedAt, CreatedBy FROM AutomaticSkills WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    automaticSkills.AutomaticSkillsID = long.Parse(reader["AutomaticSkillsID"].ToString());
                                    automaticSkills.RunID = runID;
                                    for (int i = 1; i <= 6; i++)
                                    {
                                        automaticSkills.GetType().GetProperty("AutomaticSkill" + i + "ID").SetValue(automaticSkills, long.Parse(reader["AutomaticSkill" + i + "ID"].ToString()));
                                    }
                                    automaticSkills.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    automaticSkills.CreatedBy = reader["CreatedBy"].ToString();
                                }
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
            return automaticSkills;
        }

        public ZenithSkills GetZenithSkills(long runID)
        {
            ZenithSkills zenithSkills = new ZenithSkills();

            // Use a SQL query to retrieve the ZenithSkills data for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT ZenithSkillsID, RunID, ZenithSkill1ID, ZenithSkill2ID, ZenithSkill3ID, ZenithSkill4ID, ZenithSkill5ID, ZenithSkill6ID, ZenithSkill7ID, CreatedAt, CreatedBy FROM ZenithSkills WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    zenithSkills.ZenithSkillsID = long.Parse(reader["ZenithSkillsID"].ToString());
                                    zenithSkills.RunID = runID;
                                    for (int i = 1; i <= 7; i++)
                                    {
                                        zenithSkills.GetType().GetProperty("ZenithSkill" + i + "ID").SetValue(zenithSkills, long.Parse(reader["ZenithSkill" + i + "ID"].ToString()));
                                    }
                                    zenithSkills.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    zenithSkills.CreatedBy = reader["CreatedBy"].ToString();
                                }
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
            return zenithSkills;
        }

        public CaravanSkills GetCaravanSkills(long runID)
        {
            CaravanSkills caravanSkills = new CaravanSkills();

            // Use a SQL query to retrieve the CaravanSkills data for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT CaravanSkillsID, RunID, CaravanSkill1ID, CaravanSkill2ID, CaravanSkill3ID, CreatedAt, CreatedBy FROM CaravanSkills WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    caravanSkills.CaravanSkillsID = long.Parse(reader["CaravanSkillsID"].ToString());
                                    caravanSkills.RunID = runID;
                                    for (int i = 1; i <= 3; i++)
                                    {
                                        caravanSkills.GetType().GetProperty("CaravanSkill" + i + "ID").SetValue(caravanSkills, long.Parse(reader["CaravanSkill" + i + "ID"].ToString()));
                                    }
                                    caravanSkills.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    caravanSkills.CreatedBy = reader["CreatedBy"].ToString();
                                }
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
            return caravanSkills;
        }

        public Dictionary<int, int> GetAttackBuffDictionary(long runID)
        {
            Dictionary<int, int> attackBuffDictionary = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT AttackBuffDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                attackBuffDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>((string)result);
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
            return attackBuffDictionary;
        }

        public Dictionary<int, int> GetHitCountDictionary(long runID)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT HitCountDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, double> GetHitsPerSecondDictionary(long runID)
        {
            Dictionary<int, double> dictionary = new Dictionary<int, double>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT HitsPerSecondDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, double>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, int> GetDamageDealtDictionary(long runID)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT DamageDealtDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, double> GetDamagePerSecondDictionary(long runID)
        {
            Dictionary<int, double> dictionary = new Dictionary<int, double>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT DamagePerSecondDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, double>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, int> GetAreaChangesDictionary(long runID)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT AreaChangesDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, int> GetCartsDictionary(long runID)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT CartsDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetMonster1HPDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster1HPDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetMonster2HPDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster2HPDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetMonster3HPDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster3HPDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetMonster4HPDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster4HPDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, double>> GetMonster1AttackMultiplierDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, double>> dictionary = new Dictionary<int, Dictionary<int, double>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster1AttackMultiplierDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, double>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, double>> GetMonster1DefenseRateDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, double>> dictionary = new Dictionary<int, Dictionary<int, double>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster1DefenseRateDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, double>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetMonster1PoisonThresholdDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster1PoisonThresholdDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetMonster1SleepThresholdDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster1SleepThresholdDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetMonster1ParalysisThresholdDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster1ParalysisThresholdDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetMonster1BlastThresholdDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster1BlastThresholdDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetMonster1StunThresholdDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT Monster1StunThresholdDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, List<Dictionary<int, int>>> GetPlayerInventoryDictionary(long runID)
        {
            Dictionary<int, List<Dictionary<int, int>>> dictionary = new Dictionary<int, List<Dictionary<int, int>>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT PlayerInventoryDictionary FROM PlayerGear WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, List<Dictionary<int, int>>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, List<Dictionary<int, int>>> GetAmmoDictionary(long runID)
        {
            Dictionary<int, List<Dictionary<int, int>>> dictionary = new Dictionary<int, List<Dictionary<int, int>>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT PlayerAmmoPouchDictionary FROM PlayerGear WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, List<Dictionary<int, int>>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, List<Dictionary<int, int>>> GetPartnyaBagDictionary(long runID)
        {
            Dictionary<int, List<Dictionary<int, int>>> dictionary = new Dictionary<int, List<Dictionary<int, int>>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT PartnyaBagDictionary FROM PlayerGear WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, List<Dictionary<int, int>>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, Dictionary<int, int>> GetHitsTakenBlockedDictionary(long runID)
        {
            Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT HitsTakenBlockedDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, double> GetHitsTakenBlockedPerSecondDictionary(long runID)
        {
            Dictionary<int, double> dictionary = new Dictionary<int, double>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT HitsTakenBlockedPerSecondDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, double>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, int> GetPlayerHPDictionary(long runID)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT PlayerHPDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, int> GetPlayerStaminaDictionary(long runID)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT PlayerStaminaDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, string> GetKeystrokesDictionary(long runID)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT KeystrokesDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, string>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, string> GetMouseInputDictionary(long runID)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT MouseInputDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, string>>((string)result);
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
            return dictionary;
        }

        public Dictionary<int, double> GetActionsPerMinuteDictionary(long runID)
        {
            Dictionary<int, double> dictionary = new Dictionary<int, double>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT ActionsPerMinuteDictionary FROM Quests WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                dictionary = JsonConvert.DeserializeObject<Dictionary<int, double>>((string)result);
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
            return dictionary;
        }

        public Dictionary<string, int> GetMostCommonCategory()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        ActualOverlayMode, 
                        COUNT(*) as count
                    FROM 
                        Quests
                    GROUP BY 
                        ActualOverlayMode 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string field = reader.GetString(0);
                                    int count = reader.GetInt32(1);
                                    fieldCounts.Add(field, count);
                                }
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
            return fieldCounts;
        }

        public ActiveSkills GetActiveSkills(long runID)
        {
            ActiveSkills activeSkills = new ActiveSkills();

            // Use a SQL query to retrieve the ActiveSkills for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("SELECT ActiveSkill1ID, ActiveSkill2ID, ActiveSkill3ID, ActiveSkill4ID, ActiveSkill5ID, ActiveSkill6ID, ActiveSkill7ID, ActiveSkill8ID, ActiveSkill9ID, ActiveSkill10ID, ActiveSkill11ID, ActiveSkill12ID, ActiveSkill13ID, ActiveSkill14ID, ActiveSkill15ID, ActiveSkill16ID, ActiveSkill17ID, ActiveSkill18ID, ActiveSkill19ID, CreatedAt, CreatedBy FROM ActiveSkills WHERE RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    activeSkills.RunID = runID;
                                    for (int i = 1; i <= 19; i++)
                                    {
                                        activeSkills.GetType().GetProperty("ActiveSkill" + i + "ID").SetValue(activeSkills, long.Parse(reader["ActiveSkill" + i + "ID"].ToString()));
                                    }
                                    activeSkills.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                                    activeSkills.CreatedBy = reader["CreatedBy"].ToString();
                                }
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
            return activeSkills;
        }

        public PlayerGear GetPlayerGear(long runID)
        {
            PlayerGear gearUsed = new PlayerGear();

            // Use a SQL query to retrieve the gear used for the specific RunID from the database
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(
                            @"SELECT 
                                * 
                            FROM 
                                PlayerGear 
                            WHERE 
                                RunID = @runID", conn))
                        {
                            cmd.Parameters.AddWithValue("@runID", runID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    gearUsed = new PlayerGear()
                                    {
                                        PlayerGearHash = reader["PlayerGearHash"].ToString(),
                                        CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                                        CreatedBy = reader["CreatedBy"].ToString(),
                                        PlayerGearID = long.Parse(reader["PlayerGearID"].ToString()),
                                        RunID = long.Parse(reader["RunID"].ToString()),
                                        PlayerID = long.Parse(reader["PlayerID"].ToString()),
                                        GearName = reader["GearName"].ToString(),
                                        StyleID = long.Parse(reader["StyleID"].ToString()),
                                        WeaponIconID = long.Parse(reader["WeaponIconID"].ToString()),
                                        WeaponClassID = long.Parse(reader["WeaponClassID"].ToString()),
                                        WeaponTypeID = long.Parse(reader["WeaponTypeID"].ToString()),
                                        BlademasterWeaponID = reader["BlademasterWeaponID"] == DBNull.Value ? null : (long?)long.Parse(reader["BlademasterWeaponID"].ToString()),
                                        GunnerWeaponID = reader["GunnerWeaponID"] == DBNull.Value ? null : (long?)long.Parse(reader["GunnerWeaponID"].ToString()),
                                        WeaponSlot1 = reader["WeaponSlot1"].ToString(),
                                        WeaponSlot2 = reader["WeaponSlot2"].ToString(),
                                        WeaponSlot3 = reader["WeaponSlot3"].ToString(),

                                        HeadID = long.Parse(reader["HeadID"].ToString()),
                                        HeadSlot1ID = long.Parse(reader["HeadSlot1ID"].ToString()),
                                        HeadSlot2ID = long.Parse(reader["HeadSlot2ID"].ToString()),
                                        HeadSlot3ID = long.Parse(reader["HeadSlot3ID"].ToString()),

                                        ChestID = long.Parse(reader["ChestID"].ToString()),
                                        ChestSlot1ID = long.Parse(reader["ChestSlot1ID"].ToString()),
                                        ChestSlot2ID = long.Parse(reader["ChestSlot2ID"].ToString()),
                                        ChestSlot3ID = long.Parse(reader["ChestSlot3ID"].ToString()),

                                        ArmsID = long.Parse(reader["ArmsID"].ToString()),
                                        ArmsSlot1ID = long.Parse(reader["ArmsSlot1ID"].ToString()),
                                        ArmsSlot2ID = long.Parse(reader["ArmsSlot2ID"].ToString()),
                                        ArmsSlot3ID = long.Parse(reader["ArmsSlot3ID"].ToString()),

                                        WaistID = long.Parse(reader["WaistID"].ToString()),
                                        WaistSlot1ID = long.Parse(reader["WaistSlot1ID"].ToString()),
                                        WaistSlot2ID = long.Parse(reader["WaistSlot2ID"].ToString()),
                                        WaistSlot3ID = long.Parse(reader["WaistSlot3ID"].ToString()),

                                        LegsID = long.Parse(reader["LegsID"].ToString()),
                                        LegsSlot1ID = long.Parse(reader["LegsSlot1ID"].ToString()),
                                        LegsSlot2ID = long.Parse(reader["LegsSlot2ID"].ToString()),
                                        LegsSlot3ID = long.Parse(reader["LegsSlot3ID"].ToString()),

                                        Cuff1ID = long.Parse(reader["Cuff1ID"].ToString()),
                                        Cuff2ID = long.Parse(reader["Cuff2ID"].ToString()),
                                        ZenithSkillsID = long.Parse(reader["ZenithSkillsID"].ToString()),
                                        AutomaticSkillsID = long.Parse(reader["AutomaticSkillsID"].ToString()),
                                        ActiveSkillsID = long.Parse(reader["ActiveSkillsID"].ToString()),
                                        CaravanSkillsID = long.Parse(reader["CaravanSkillsID"].ToString()),
                                        DivaSkillID = long.Parse(reader["DivaSkillID"].ToString()),
                                        GuildFoodID = long.Parse(reader["GuildFoodID"].ToString()),
                                        StyleRankSkillsID = long.Parse(reader["StyleRankSkillsID"].ToString()),
                                        PlayerInventoryID = long.Parse(reader["PlayerInventoryID"].ToString()),
                                        AmmoPouchID = long.Parse(reader["AmmoPouchID"].ToString()),
                                        PartnyaBagID = long.Parse(reader["PartnyaBagID"].ToString()),
                                        PoogieItemID = long.Parse(reader["PoogieItemID"].ToString()),
                                        RoadDureSkillsID = long.Parse(reader["RoadDureSkillsID"].ToString()),
                                        PlayerInventoryDictionary = reader["PlayerInventoryDictionary"].ToString(),
                                        PlayerAmmoPouchDictionary = reader["PlayerAmmoPouchDictionary"].ToString(),
                                        PartnyaBagDictionary = reader["PartnyaBagDictionary"].ToString()
                                    };
                                }
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
            return gearUsed;
        }

        //TODO add check if there are runs for the quest id?
        public List<FastestRun> GetFastestRuns(ConfigWindow configWindow, string weaponName = "All Weapons")
        {
            List<FastestRun> fastestRuns = new List<FastestRun>();
            if (long.TryParse(configWindow.QuestIDTextBox.Text.Trim(), out long questID))
            {
                using (SQLiteConnection conn = new SQLiteConnection(dataSource))
                {
                    conn.Open();
                    using (SQLiteTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string sql = "";
                            int weaponTypeID = 0;

                            if (weaponName == "All Weapons")
                            {
                                sql = @"SELECT 
                                            ObjectiveImage, 
                                            qn.QuestNameName, 
                                            q.RunID, 
                                            QuestID, 
                                            YoutubeID, 
                                            FinalTimeDisplay, 
                                            Date, 
                                            ActualOverlayMode, 
                                            PartySize
                                        FROM 
                                            Quests q
                                        JOIN 
                                            QuestName qn ON q.QuestID = qn.QuestNameID
                                        WHERE 
                                            q.QuestID = @questID 
                                            AND q.PartySize = 1
                                            AND q.ActualOverlayMode = @SelectedOverlayMode
                                        ORDER BY 
                                            FinalTimeValue ASC
                                        LIMIT 20";
                            }
                            else
                            {
                                sql = @"SELECT 
                                            ObjectiveImage, 
                                            qn.QuestNameName, 
                                            q.RunID, 
                                            QuestID, 
                                            YoutubeID, 
                                            FinalTimeDisplay, 
                                            Date, 
                                            ActualOverlayMode, 
                                            PartySize,
                                            pg.WeaponTypeID
                                        FROM 
                                            Quests q
                                        JOIN 
                                            QuestName qn ON q.QuestID = qn.QuestNameID
                                        JOIN
                                            PlayerGear pg ON q.RunID =  pg.RunID
                                        WHERE 
                                            q.QuestID = @questID 
                                            AND q.PartySize = 1
                                            AND q.ActualOverlayMode = @SelectedOverlayMode
                                            AND pg.WeaponTypeID = @SelectedWeaponTypeID
                                        ORDER BY 
                                            FinalTimeValue ASC
                                        LIMIT 20";
                                weaponTypeID = WeaponType.IDName.FirstOrDefault(x => x.Value == weaponName).Key;
                            }

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                string selectedOverlayMode = ((ComboBoxItem)configWindow.OverlayModeComboBox.SelectedItem).Content.ToString();
                                // idk if this is needed
                                if (selectedOverlayMode == "" || selectedOverlayMode == null)
                                    selectedOverlayMode = "Standard";

                                cmd.Parameters.AddWithValue("@questID", questID);
                                cmd.Parameters.AddWithValue("@SelectedOverlayMode", selectedOverlayMode);

                                if (weaponName != "All Weapons")
                                    cmd.Parameters.AddWithValue("@SelectedWeaponTypeID", weaponTypeID);

                                using (SQLiteDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader == null || !reader.HasRows)
                                        return fastestRuns;
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            fastestRuns.Add(new FastestRun
                                            {
                                                ObjectiveImage = (string)reader["ObjectiveImage"],
                                                QuestName = (string)reader["QuestNameName"],
                                                RunID = (long)reader["RunID"],
                                                QuestID = (long)reader["QuestID"],
                                                YoutubeID = (string)reader["YoutubeID"],
                                                FinalTimeDisplay = (string)reader["FinalTimeDisplay"],
                                                Date = DateTime.Parse((string)reader["Date"])
                                            });
                                        }
                                    }

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
            }
            return fastestRuns;
        }


        public List<RecentRuns> GetRecentRuns()
        {
            List<RecentRuns> recentRuns = new List<RecentRuns>();
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(
                        @"SELECT 
                            ObjectiveImage, 
                            qn.QuestNameName, 
                            RunID, 
                            QuestID, 
                            YoutubeID, 
                            FinalTimeDisplay, 
                            Date, 
                            ActualOverlayMode, 
                            PartySize
                        FROM 
                            Quests q
                        JOIN
                            QuestName qn ON q.QuestID = qn.QuestNameID
                        ORDER BY 
                            Date DESC
                        LIMIT 10", conn
                        ))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader == null || !reader.HasRows)
                                    return recentRuns;
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        RecentRuns recentRun = new RecentRuns
                                        {
                                            ObjectiveImage = (string)reader["ObjectiveImage"],
                                            QuestName = (string)reader["QuestNameName"],
                                            RunID = (long)reader["RunID"],
                                            QuestID = (long)reader["QuestID"],
                                            YoutubeID = (string)reader["YoutubeID"],
                                            FinalTimeDisplay = (string)reader["FinalTimeDisplay"],
                                            Date = DateTime.Parse((string)reader["Date"]),
                                            ActualOverlayMode = (string)reader["ActualOverlayMode"],
                                            PartySize = (long)reader["PartySize"]
                                        };
                                        recentRuns.Add(recentRun);
                                    }
                                }
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
            return recentRuns;
        }




        public List<WeaponUsageMapper> CalculateTotalWeaponUsage(ConfigWindow configWindow, DataLoader dataLoader, bool isByQuestID = false)
        {
            List<WeaponUsageMapper> weaponUsageData = new List<WeaponUsageMapper>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = "";
                        long questID = 0;

                        if (isByQuestID)
                        {
                            questID = long.Parse(configWindow.QuestIDTextBox.Text.Trim());
                            sql = @"SELECT 
                            WeaponTypeID, 
                            StyleID, 
                            COUNT(*) AS RunCount 
                        FROM 
                            PlayerGear 
                            JOIN Quests ON PlayerGear.RunID = Quests.RunID
                        WHERE 
                            Quests.QuestID = @QuestID
                        GROUP BY 
                            WeaponTypeID, StyleID";
                        }
                        else
                        {
                            sql = @"SELECT 
                            WeaponTypeID, 
                            StyleID, 
                            COUNT(*) AS RunCount 
                        FROM 
                            PlayerGear 
                        GROUP BY 
                            WeaponTypeID, StyleID";
                        }

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            if (isByQuestID)
                                cmd.Parameters.AddWithValue("@QuestID", questID);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    //MessageBox.Show(String.Format("Runs not found. Please use the Quest ID option in Settings and go into a quest in order to view the ID needed to search. You may also not have completed any runs for the selected Quest ID or for the selected category.\n\nQuest ID: {0}\nOverlay Mode: {1}\n{2}", questID, selectedOverlayMode, reader.ToString()), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return weaponUsageData;
                                }
                                else
                                {
                                    while (reader.Read())
                                    {
                                        long weaponTypeID = (long)reader["WeaponTypeID"];
                                        long styleID = (long)reader["StyleID"];
                                        long runCount = (long)reader["RunCount"];

                                        string weaponType = "";
                                        string style = "";

                                        lock (dataLoader.model.weaponUsageSync)
                                        {
                                            // Use the weaponTypeID, styleID, and runCount values to populate your
                                            // LiveChart graph
                                            // use a switch statement or a lookup table to convert the
                                            // weaponTypeID and styleID to their corresponding string names

                                            weaponType = WeaponType.IDName[int.Parse(weaponTypeID.ToString())];
                                            style = WeaponStyle.IDName[int.Parse(styleID.ToString())];
                                            weaponUsageData.Add(new WeaponUsageMapper(weaponType, style, (int)runCount));
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
            }
            return weaponUsageData;
        }


        public void QuestIDButton_Click(object sender, RoutedEventArgs e, ConfigWindow configWindow)
        {
            // Execute query with only Quest ID
            int questID = int.Parse(configWindow.QuestIDTextBox.Text);
            string selectedOverlayMode = ((ComboBoxItem)configWindow.OverlayModeComboBox.SelectedItem).Content.ToString();
            // idk if this is needed
            if (selectedOverlayMode == "" || selectedOverlayMode == null)
                selectedOverlayMode = "Standard";

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(
                        @"SELECT 
                            q.ObjectiveImage,
                            q.ObjectiveTypeID,
                            q.ObjectiveQuantity,
                            q.ObjectiveName,
                            qn.QuestNameName
                        FROM 
                            Quests q 
                        INNER JOIN 
                            QuestName qn ON q.QuestID = qn.QuestNameID
                        WHERE 
                            q.QuestID = @QuestID
                            AND q.PartySize = 1
                            AND q.ActualOverlayMode = @SelectedOverlayMode", conn
                        ))
                        {

                            cmd.Parameters.AddWithValue("@QuestID", questID);
                            cmd.Parameters.AddWithValue("@SelectedOverlayMode", selectedOverlayMode);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    MessageBox.Show(String.Format("Quest ID not found. Please use the Quest ID option in Settings and go into a quest in order to view the ID needed to search. You may also not have completed any runs for the selected Quest ID or for the selected category.\n\nQuest ID: {0}\nOverlay Mode: {1}\n{2}", questID, selectedOverlayMode, reader.ToString()), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                else
                                {
                                    reader.Read();
                                    configWindow.SelectedQuestObjectiveImage.Source = new BitmapImage(new Uri(reader["ObjectiveImage"].ToString()));
                                    configWindow.SelectedQuestNameTextBlock.Text = reader["QuestNameName"].ToString();
                                    configWindow.SelectedQuestObjectiveTextBlock.Text = string.Format("{0} {1} {2}", ObjectiveType.IDName[int.Parse(reader["ObjectiveTypeID"].ToString())], reader["ObjectiveQuantity"], reader["ObjectiveName"]);
                                    configWindow.CurrentTimeTextBlock.Text = DateTime.Now.ToString();

                                }
                            }
                        }


                        using (SQLiteCommand cmd = new SQLiteCommand(
                        @"SELECT 
                                pg.WeaponTypeID, 
                                MIN(q.FinalTimeValue) as BestTime, 
                                q.RunID
                            FROM 
                                Quests q 
                            JOIN 
                                PlayerGear pg ON q.RunID = pg.RunID 
                            WHERE 
                                q.QuestID = @QuestID 
                                AND q.PartySize = 1 
                                AND q.ActualOverlayMode = @SelectedOverlayMode
                            GROUP BY 
                                pg.WeaponTypeID
                            ", conn
                        )
                        )
                        {

                            cmd.Parameters.AddWithValue("@QuestID", questID);
                            cmd.Parameters.AddWithValue("@SelectedOverlayMode", selectedOverlayMode);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    MessageBox.Show(String.Format("Quest ID not found. Please use the Quest ID option in Settings and go into a quest in order to view the ID needed to search. You may also not have completed any runs for the selected Quest ID or for the selected category.\n\nQuest ID: {0}\nOverlay Mode: {1}\n{2}", questID, selectedOverlayMode, reader.ToString()), LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                while (reader.Read())
                                {
                                    WeaponType.IDName.TryGetValue(Convert.ToInt32(reader["WeaponTypeID"]), out string? weaponType);

                                    int bestTime;
                                    if (reader["BestTime"] == DBNull.Value)
                                        bestTime = -1;
                                    else
                                        bestTime = int.Parse(reader["BestTime"].ToString());

                                    int runID;
                                    if (reader["RunID"] == DBNull.Value)
                                        runID = -1;
                                    else
                                        runID = int.Parse(reader["RunID"].ToString());

                                    switch (weaponType)
                                    {
                                        case "Sword and Shield":
                                            configWindow.SwordAndShieldBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.SwordAndShieldRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Great Sword":
                                            configWindow.GreatSwordBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.GreatSwordRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Dual Swords":
                                            configWindow.DualSwordsBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.DualSwordsRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Long Sword":
                                            configWindow.LongSwordBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.LongSwordRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Lance":
                                            configWindow.LanceBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.LanceRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Gunlance":
                                            configWindow.GunlanceBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.GunlanceRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Hammer":
                                            configWindow.HammerBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.HammerRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Hunting Horn":
                                            configWindow.HuntingHornBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.HuntingHornRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Tonfa":
                                            configWindow.TonfaBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.TonfaRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Switch Axe F":
                                            configWindow.SwitchAxeFBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.SwitchAxeFRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Magnet Spike":
                                            configWindow.MagnetSpikeBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.MagnetSpikeRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Light Bowgun":
                                            configWindow.LightBowgunBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.LightBowgunRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Heavy Bowgun":
                                            configWindow.HeavyBowgunBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.HeavyBowgunRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                        case "Bow":
                                            configWindow.BowBestTimeTextBlock.Text = FormatTime(bestTime);
                                            configWindow.BowRunIDTextBlock.Text = string.Format("Run ID: {0}", runID);
                                            break;
                                            // Add more cases for other weapon types
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
            }
        }

        public Dictionary<int, int> GetMostQuestCompletions()
        {
            Dictionary<int, int> questCompletions = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                                QuestID, 
                                COUNT(*) as completions 
                            FROM 
                                Quests
                            GROUP BY 
                                QuestID 
                            ORDER BY completions DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int questID = reader.GetInt32(0);
                                    int completions = reader.GetInt32(1);
                                    questCompletions.Add(questID, completions);
                                }
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
            return questCompletions;
        }

        public Dictionary<string, int> GetMostCommonObjectiveTypes()
        {
            Dictionary<string, int> objectiveCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        ObjectiveTypeID, 
                        COUNT(*) as count
                    FROM 
                        Quests
                    GROUP BY 
                        ObjectiveTypeID 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int objectiveTypeID = reader.GetInt32(0);
                                    string objectiveTypeName = ObjectiveType.IDName[objectiveTypeID];
                                    int count = reader.GetInt32(1);
                                    objectiveCounts.Add(objectiveTypeName, count);
                                }
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
            return objectiveCounts;
        }

        public Dictionary<int, int> GetMostCommonStarGrades()
        {
            Dictionary<int, int> fieldCounts = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        StarGrade, 
                        COUNT(*) as count
                    FROM 
                        Quests
                    GROUP BY 
                        StarGrade 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int field = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    fieldCounts.Add(field, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<string, int> GetMostCommonHeadPieces()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        HeadID, 
                        COUNT(*) as count
                    FROM 
                        PlayerGear
                    GROUP BY 
                        HeadID 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int field = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    string pieceName = ArmorHead.IDName[field];
                                    fieldCounts.Add(pieceName, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<string, int> GetMostCommonChestPieces()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        ChestID, 
                        COUNT(*) as count
                    FROM 
                        PlayerGear
                    GROUP BY 
                        ChestID 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int field = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    string pieceName = ArmorChest.IDName[field];
                                    fieldCounts.Add(pieceName, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<string, int> GetMostCommonArmsPieces()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        ArmsID, 
                        COUNT(*) as count
                    FROM 
                        PlayerGear
                    GROUP BY 
                        ArmsID 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int field = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    string pieceName = ArmorArms.IDName[field];
                                    fieldCounts.Add(pieceName, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<string, int> GetMostCommonWaistPieces()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        WaistID, 
                        COUNT(*) as count
                    FROM 
                        PlayerGear
                    GROUP BY 
                        WaistID 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int field = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    string pieceName = ArmorWaist.IDName[field];
                                    fieldCounts.Add(pieceName, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<string, int> GetMostCommonLegsPieces()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        LegsID, 
                        COUNT(*) as count
                    FROM 
                        PlayerGear
                    GROUP BY 
                        LegsID 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int field = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    string pieceName = ArmorLegs.IDName[field];
                                    fieldCounts.Add(pieceName, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<string, int> GetMostCommonDivaSkill()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        DivaSkillID, 
                        COUNT(*) as count
                    FROM 
                        PlayerGear
                    GROUP BY 
                        DivaSkillID 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int field = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    string name = SkillDiva.IDName[field];
                                    fieldCounts.Add(name, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<string, int> GetMostCommonGuildFood()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        GuildFoodID, 
                        COUNT(*) as count
                    FROM 
                        PlayerGear
                    GROUP BY 
                        GuildFoodID 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int field = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    string name = SkillArmor.IDName[field];
                                    fieldCounts.Add(name, count);
                                }
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
            return fieldCounts;
        }


        public Dictionary<string, int> GetMostCommonRankBands()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        RankName, 
                        COUNT(*) as count
                    FROM 
                        Quests
                    GROUP BY 
                        RankName 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string field = reader.GetString(0);
                                    int count = reader.GetInt32(1);
                                    fieldCounts.Add(field, count);
                                }
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
            return fieldCounts;
        }


        public Dictionary<string, int> GetMostCommonObjectives()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        ObjectiveName, 
                        COUNT(*) as count
                    FROM 
                        Quests
                    GROUP BY 
                        ObjectiveName 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string field = reader.GetString(0);
                                    int count = reader.GetInt32(1);
                                    fieldCounts.Add(field, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<string, int> GetMostCommonSetNames()
        {
            Dictionary<string, int> fieldCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        GearName, 
                        COUNT(*) as count
                    FROM 
                        PlayerGear
                    GROUP BY 
                        GearName 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string field = reader.GetString(0);
                                    int count = reader.GetInt32(1);
                                    fieldCounts.Add(field, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<DateTime, int> GetQuestsCompletedByDate()
        {
            Dictionary<DateTime, int> questsCompletedByDate = new Dictionary<DateTime, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        DATE(CreatedAt) as DateOnly, 
                        COUNT(*) as completions 
                    FROM 
                        Quests
                    GROUP BY 
                        DateOnly 
                    ORDER BY DateOnly ASC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DateTime date = reader.GetDateTime(0);
                                    int completions = reader.GetInt32(1);
                                    questsCompletedByDate.Add(date, completions);
                                }
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
            return questsCompletedByDate;
        }

        public Dictionary<string, int> GetMostCommonWeaponNames()
        {
            Dictionary<string, int> weaponCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                                WeaponClassID,
                                COALESCE(BlademasterWeaponID, GunnerWeaponID) as WeaponID,
                                COUNT(*) as Frequency
                            FROM 
                                PlayerGear
                            GROUP BY 
                                WeaponID
                            ORDER BY Frequency DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int weaponClassID = reader.GetInt32(0);
                                    int weaponID = reader.GetInt32(1);
                                    int frequency = reader.GetInt32(2);
                                    string weaponName = "";
                                    if (WeaponBlademaster.IDName.ContainsKey(weaponID) && WeaponClass.IDName[weaponClassID] == "Blademaster")
                                    {
                                        weaponName = WeaponBlademaster.IDName[weaponID];
                                    }
                                    else if (WeaponGunner.IDName.ContainsKey(weaponID) && WeaponClass.IDName[weaponClassID] == "Gunner")
                                    {
                                        weaponName = WeaponGunner.IDName[weaponID];
                                    }
                                    if (!string.IsNullOrEmpty(weaponName))
                                    {
                                        weaponCounts[weaponName] = frequency;
                                    }
                                }
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
            return weaponCounts;
        }

        public Dictionary<string, int> GetMostCommonStyleRankSkills()
        {
            Dictionary<string, int> skillCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                    StyleRankSkill1ID, 
                    COUNT(*) as count
                FROM 
                    StyleRankSkills
                GROUP BY 
                    StyleRankSkill1ID 
                UNION
                SELECT 
                    StyleRankSkill2ID, 
                    COUNT(*) as count
                FROM 
                    StyleRankSkills
                GROUP BY 
                    StyleRankSkill2ID 
                ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int skillID = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    string skillName = SkillStyleRank.IDName[skillID];
                                    if (!skillCounts.ContainsKey(skillName))
                                    {
                                        skillCounts.Add(skillName, count);
                                    }
                                    else
                                    {
                                        skillCounts[skillName] += count;
                                    }
                                }
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
            return skillCounts;
        }

        public Dictionary<string, int> GetMostCommonCaravanSkills()
        {
            Dictionary<string, int> skillCounts = new Dictionary<string, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                    CaravanSkill1ID, 
                    COUNT(*) as count
                FROM 
                    CaravanSkills
                GROUP BY 
                    CaravanSkill1ID 
                UNION
                SELECT 
                    CaravanSkill2ID, 
                    COUNT(*) as count
                FROM 
                    CaravanSkills
                GROUP BY 
                    CaravanSkill2ID 
                UNION
                SELECT 
                    CaravanSkill3ID, 
                    COUNT(*) as count
                FROM 
                    CaravanSkills
                GROUP BY 
                    CaravanSkill3ID 
                ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int skillID = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    string skillName = SkillCaravan.IDName[skillID];
                                    if (!skillCounts.ContainsKey(skillName))
                                    {
                                        skillCounts.Add(skillName, count);
                                    }
                                    else
                                    {
                                        skillCounts[skillName] += count;
                                    }
                                }
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
            return skillCounts;
        }

        public Dictionary<int, int> GetMostCommonPartySize()
        {
            Dictionary<int, int> fieldCounts = new Dictionary<int, int>();

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                        PartySize, 
                        COUNT(*) as count
                    FROM 
                        Quests
                    GROUP BY 
                        PartySize 
                    ORDER BY count DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int field = reader.GetInt32(0);
                                    int count = reader.GetInt32(1);
                                    fieldCounts.Add(field, count);
                                }
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
            return fieldCounts;
        }

        public Dictionary<int, int> GetTotalTimeSpentInQuests()
        {
            Dictionary<int, int> questTimeSpent = new Dictionary<int, int>();
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql =
                            @"SELECT 
                                QuestID, 
                                SUM(FinalTimeValue) as timeSpent 
                            FROM 
                                Quests
                            GROUP BY 
                                QuestID 
                            ORDER BY timeSpent DESC";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int questID = reader.GetInt32(0);
                                    int timeSpent = reader.GetInt32(1);
                                    questTimeSpent.Add(questID, timeSpent);
                                }
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
            return questTimeSpent;
        }

        public void InsertMezFesMinigameScore(DataLoader dataLoader, int previousMezFesArea, int previousMezFesScore)
        {
            if (!MezFesMinigame.ID.ContainsKey(previousMezFesArea) || previousMezFesScore <= 0)
            {
                logger.Error("wrong mezfes area or empty score, area id: {0}, score: {1}", previousMezFesArea, previousMezFesScore);
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = @"INSERT INTO MezFes (
                            CreatedAt,
                            CreatedBy,
                            MezFesMinigameID,
                            Score
                            ) VALUES (
                            @CreatedAt,
                            @CreatedBy,
                            @MezFesMinigameID,
                            @Score)";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            DateTime createdAt = DateTime.Now;
                            string createdBy = dataLoader.model.GetFullCurrentProgramVersion();

                            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);
                            cmd.Parameters.AddWithValue("@MezFesMinigameID", previousMezFesArea);
                            cmd.Parameters.AddWithValue("@Score", previousMezFesScore);

                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        HandleError(transaction, ex);
                    }
                }
            }
        }

        // TODO: i still need to reorganize all regions. ideally i put in separate classes/files. maybe a DatabaseHelper class?
        #region database functions

        /// <summary>
        /// Helper function to calculate the median of a list of integers
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static double CalculateMedianOfList(List<int> values)
        {
            int count = values.Count;
            if (count % 2 == 0)
            {
                // Even number of values, average the middle two
                int middleIndex = count / 2;
                return (values[middleIndex - 1] + values[middleIndex]) / 2.0;
            }
            else
            {
                // Odd number of values, return the middle value
                int middleIndex = (count - 1) / 2;
                return values[middleIndex];
            }
        }

        /// <summary>
        /// Gets the average of field.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private double GetAverageOfDictionaryField(string tableName, string fieldName, SQLiteConnection conn)
        {
            var query = $"SELECT {fieldName} FROM {tableName}";

            double sumOfValues = 0;
            int numberOfEntries = 0;

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string valueDictionaryString = (string)reader[fieldName];

                        if (string.IsNullOrEmpty(valueDictionaryString) || valueDictionaryString == "{}")
                            continue;

                        var valueDictionary = JObject.Parse(valueDictionaryString)
                            .ToObject<Dictionary<double, double>>();

                        double averageOfValueValues = valueDictionary.Values.Average();
                        sumOfValues += averageOfValueValues;
                        numberOfEntries++;
                    }
                }
            }

            double averageValue = sumOfValues / numberOfEntries;

            return averageValue;
        }

        /// <summary>
        /// Gets the median of field.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private double GetMedianOfDictionaryField(string tableName, string fieldName, SQLiteConnection conn)
        {
            var query = $"SELECT {fieldName} FROM {tableName}";
            var valueList = new List<double>();

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string valueDictionaryString = (string)reader[fieldName];
                        if (string.IsNullOrEmpty(valueDictionaryString) || valueDictionaryString == "{}")
                            continue;
                        var valueDictionary = JObject.Parse(valueDictionaryString).ToObject<Dictionary<double, double>>();
                        valueList.AddRange(valueDictionary.Values);
                    }
                }
            }

            valueList.Sort();
            double medianValue = valueList.Count % 2 == 0
                ? (valueList[valueList.Count / 2] + valueList[valueList.Count / 2 - 1]) / 2.0
                : valueList[valueList.Count / 2];

            return medianValue;
        }

        /// <summary>
        /// Gets the mode of field.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private double GetModeOfDictionaryField(string tableName, string fieldName, SQLiteConnection conn)
        {
            var query = $"SELECT {fieldName} FROM {tableName}";
            var valueDictionary = new Dictionary<double, int>();

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string valueDictionaryString = (string)reader[fieldName];

                        if (string.IsNullOrEmpty(valueDictionaryString) || valueDictionaryString == "{}")
                            continue;

                        var dict = JObject.Parse(valueDictionaryString).ToObject<Dictionary<double, double>>();

                        foreach (var value in dict.Values)
                        {
                            if (valueDictionary.ContainsKey(value))
                                valueDictionary[value]++;
                            else
                                valueDictionary.Add(value, 1);
                        }
                    }
                }
            }

            double modeValue = 0;
            int maxCount = 0;

            foreach (var kvp in valueDictionary)
            {
                if (kvp.Value > maxCount)
                {
                    modeValue = kvp.Key;
                    maxCount = kvp.Value;
                }
            }

            return modeValue;
        }

        /// <summary>
        /// Gets the row with highest value.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private (double, long) GetRowWithHighestDictionaryValue(string tableName, string fieldName, SQLiteConnection conn)
        {
            var query = $"SELECT {fieldName}, RunID FROM {tableName}";

            double highestValue = 0;
            long highestValueRunID = 0;

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long runID = (long)reader["RunID"];
                        string valueDictionaryString = (string)reader[fieldName];

                        if (string.IsNullOrEmpty(valueDictionaryString) || valueDictionaryString == "{}")
                            continue;

                        var valueDictionary = JObject.Parse(valueDictionaryString)
                            .ToObject<Dictionary<double, double>>();

                        double maxValueInField = valueDictionary.Values.Max();

                        if (maxValueInField > highestValue)
                        {
                            highestValue = maxValueInField;
                            highestValueRunID = runID;
                        }
                    }
                }
            }

            return (highestValue, highestValueRunID);
        }

        /// <summary>
        /// Gets the quest with highest hits taken blocked.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private (double, long) GetQuestWithHighestHitsTakenBlocked(SQLiteConnection conn)
        {
            var query = "SELECT RunID, HitsTakenBlockedDictionary FROM Quests";
            long highestHitsTakenBlockedRunID = 0;
            int highestHitsTakenBlockedCount = 0;

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long runID = (long)reader["RunID"];
                        string hitsTakenBlockedDictionaryString = (string)reader["HitsTakenBlockedDictionary"];

                        if (string.IsNullOrEmpty(hitsTakenBlockedDictionaryString) || hitsTakenBlockedDictionaryString == "{}")
                            continue;

                        var hitsTakenBlocked = JObject.Parse(hitsTakenBlockedDictionaryString)
    .ToObject<Dictionary<double, Dictionary<double, double>>>();

                        int hitsTakenBlockedCount = hitsTakenBlocked.Count;
                        if (hitsTakenBlockedCount > highestHitsTakenBlockedCount)
                        {
                            highestHitsTakenBlockedCount = hitsTakenBlockedCount;
                            highestHitsTakenBlockedRunID = runID;
                        }
                    }
                }
            }

            return ((double)highestHitsTakenBlockedCount, highestHitsTakenBlockedRunID);
        }

        /// <summary>
        /// Gets the average hits taken blocked count.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private double GetAverageHitsTakenBlockedCount(SQLiteConnection conn)
        {
            var totalQuestRunsQuery = "SELECT COUNT(*) as TotalQuestRuns FROM Quests";
            var hitsTakenBlockedCountQuery = "SELECT HitsTakenBlockedDictionary FROM Quests";
            double sumOfHitsTakenBlockedCount = 0;
            int totalQuestRuns = 0;

            using (SQLiteCommand cmd = new SQLiteCommand(totalQuestRunsQuery, conn))
            {
                totalQuestRuns = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (SQLiteCommand cmd = new SQLiteCommand(hitsTakenBlockedCountQuery, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string hitsTakenBlockedDictionaryString = (string)reader["HitsTakenBlockedDictionary"];

                        if (string.IsNullOrEmpty(hitsTakenBlockedDictionaryString) || hitsTakenBlockedDictionaryString == "{}")
                            continue;

                        var hitsTakenBlocked = JObject.Parse(hitsTakenBlockedDictionaryString)
                            .ToObject<Dictionary<double, Dictionary<double, double>>>();

                        int hitsTakenBlockedCount = hitsTakenBlocked.Count;
                        sumOfHitsTakenBlockedCount += hitsTakenBlockedCount;
                    }
                }
            }

            double averageHitsTakenBlockedCount = sumOfHitsTakenBlockedCount / totalQuestRuns;
            return averageHitsTakenBlockedCount;
        }

        /// <summary>
        /// Gets the median hits taken blocked count.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private double GetMedianHitsTakenBlockedCount(SQLiteConnection conn)
        {
            var hitsTakenBlockedCountQuery = "SELECT HitsTakenBlockedDictionary FROM Quests";
            var hitsTakenBlockedCountList = new List<int>();

            using (SQLiteCommand cmd = new SQLiteCommand(hitsTakenBlockedCountQuery, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string hitsTakenBlockedDictionaryString = (string)reader["HitsTakenBlockedDictionary"];

                        if (string.IsNullOrEmpty(hitsTakenBlockedDictionaryString) || hitsTakenBlockedDictionaryString == "{}")
                            continue;

                        var hitsTakenBlocked = JObject.Parse(hitsTakenBlockedDictionaryString)
                            .ToObject<Dictionary<double, Dictionary<double, double>>>();

                        int hitsTakenBlockedCount = hitsTakenBlocked.Count;
                        hitsTakenBlockedCountList.Add(hitsTakenBlockedCount);
                    }
                }
            }

            hitsTakenBlockedCountList.Sort();

            double medianHitsTakenBlockedCount;
            int count = hitsTakenBlockedCountList.Count;

            if (count % 2 == 0)
            {
                int middle = count / 2;
                medianHitsTakenBlockedCount = (hitsTakenBlockedCountList[middle - 1] + hitsTakenBlockedCountList[middle]) / 2.0;
            }
            else
            {
                int middle = count / 2;
                medianHitsTakenBlockedCount = hitsTakenBlockedCountList[middle];
            }

            return medianHitsTakenBlockedCount;
        }

        /// <summary>
        /// Gets the total count of value in dictionary field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="table">The table.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private long GetTotalCountOfValueInDictionaryField(string field, string table, SQLiteConnection conn)
        {
            var query = $"SELECT {field} FROM {table}";
            long totalCount = 0;

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string valueDictionaryString = (string)reader[field];

                        if (string.IsNullOrEmpty(valueDictionaryString) || valueDictionaryString == "{}")
                            continue;

                        if (field == "HitsTakenBlockedDictionary" && table == "Quests")
                        {
                            var dictionary = JObject.Parse(valueDictionaryString)
                            .ToObject<Dictionary<double, Dictionary<double, double>>>();
                            totalCount += dictionary.Count;
                        }
                        else if (table == "Quests" && (field == "KeystrokesDictionary" || field == "MouseInputDictionary" || field == "GamepadInputDictionary"))
                        {
                            var dictionary = JObject.Parse(valueDictionaryString)
                            .ToObject<Dictionary<double, string>>();
                            totalCount += dictionary.Count;
                        }
                        else
                        {
                            var dictionary = JObject.Parse(valueDictionaryString)
                            .ToObject<Dictionary<double, double>>();
                            totalCount += dictionary.Count;
                        }
                    }
                }
            }

            return totalCount;
        }

        /// <summary>
        /// Gets the table row count.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private long GetTableRowCount(string fieldName, string tableName, SQLiteConnection conn)
        {
            string sql = $"SELECT COUNT({fieldName}) FROM {tableName}";
            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                return Convert.ToInt64(command.ExecuteScalar());
            }
        }

        /// <summary>
        /// Gets the count of int value.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="value">The value.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private static long GetCountOfIntValue(string fieldName, string tableName, int value, SQLiteConnection conn)
        {
            long count = 0;
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {fieldName} = @value";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@value", value);
                count = Convert.ToInt64(cmd.ExecuteScalar());
            }
            return count;
        }

        /// <summary>
        /// Gets the count of string value.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="value">The value.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private static long GetCountOfStringValue(string fieldName, string tableName, string value, SQLiteConnection conn)
        {
            long count = 0;
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {fieldName} = @value";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@value", value);
                count = Convert.ToInt64(cmd.ExecuteScalar());
            }
            return count;
        }

        /// <summary>
        /// Get the maximum value of a given field in a given table
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static long GetMaxValue(string field, string table, SQLiteConnection conn)
        {
            string query = $"SELECT MAX({field}) FROM {table} WHERE {field} IS NOT NULL";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                long maxValue = (long)cmd.ExecuteScalar();
                return maxValue;
            }
        }

        /// <summary>
        /// Get the minimum value of a given field in a given table
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static long GetMinValue(string field, string table, SQLiteConnection conn)
        {
            string query = $"SELECT MIN({field}) FROM {table} WHERE {field} IS NOT NULL";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                long minValue = (long)cmd.ExecuteScalar();
                return minValue;
            }
        }

        /// <summary>
        /// Get the average value of a given field in a given table
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static double GetAvgValue(string field, string table, SQLiteConnection conn)
        {
            string query = $"SELECT AVG({field}) FROM {table} WHERE {field} IS NOT NULL";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                double avgValue = (double)cmd.ExecuteScalar();
                return avgValue;
            }
        }

        /// <summary>
        /// Get the median value of a given field in a given table
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static double GetMedianValue(string field, string table, SQLiteConnection conn)
        {
            string query = $"SELECT {field} FROM {table} WHERE {field} IS NOT NULL ORDER BY {field}";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        return 0.0;
                    }
                    List<long> values = new List<long>();
                    while (reader.Read())
                    {
                        values.Add(reader.GetInt64(0));
                    }
                    long[] valuesArray = values.ToArray();
                    Array.Sort(valuesArray);
                    int count = valuesArray.Length;
                    double medianValue = (count % 2 == 0) ? ((double)valuesArray[count / 2] + (double)valuesArray[(count / 2) - 1]) / 2 : (double)valuesArray[count / 2];
                    return medianValue;
                }
            }
        }

        /// <summary>
        /// Get the mode value of a given field in a given table
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static long GetModeValue(string field, string table, SQLiteConnection conn)
        {
            string query = $"SELECT {field}, COUNT(*) as count FROM {table} WHERE {field} IS NOT NULL GROUP BY {field} ORDER BY count DESC LIMIT 1";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt64(0);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }


        /// <summary>
        /// Gets the total unique armor pieces.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private long GetTotalUniqueArmorPieces(SQLiteConnection conn)
        {
            long totalUniqueArmorPieces = 0;

            var query = @"
                            SELECT 
                                COUNT(DISTINCT HeadID) + 
                                COUNT(DISTINCT ChestID) + 
                                COUNT(DISTINCT ArmsID) + 
                                COUNT(DISTINCT WaistID) + 
                                COUNT(DISTINCT LegsID) AS TotalUniqueArmorPieces
                            FROM PlayerGear
                        ";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        totalUniqueArmorPieces += (long)reader["TotalUniqueArmorPieces"];
                    }
                }
            }

            return totalUniqueArmorPieces;
        }

        /// <summary>
        /// Gets the total unique weapon i ds.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private long GetTotalUniqueWeaponIDs(SQLiteConnection conn)
        {
            long totalUniqueWeaponIDs = 0;

            var query = @"
                            SELECT 
                                COUNT(DISTINCT BlademasterWeaponID) + 
                                COUNT(DISTINCT GunnerWeaponID) AS TotalUniqueWeaponIDs
                            FROM PlayerGear
                        ";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        totalUniqueWeaponIDs += (long)reader["TotalUniqueWeaponIDs"];
                    }
                }
            }

            return totalUniqueWeaponIDs;
        }

        /// <summary>
        /// Gets the total unique decorations. Does not count weapon slots, as those are meant to use sigils
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private long GetTotalUniqueDecorations(SQLiteConnection conn)
        {
            long totalUniqueDecorations = 0;

            var query = @"
                        SELECT 
                            COUNT(DISTINCT HeadSlot1ID) +
                            COUNT(DISTINCT HeadSlot2ID) +
                            COUNT(DISTINCT HeadSlot3ID) +
                            COUNT(DISTINCT ChestSlot1ID) +
                            COUNT(DISTINCT ChestSlot2ID) +
                            COUNT(DISTINCT ChestSlot3ID) +
                            COUNT(DISTINCT ArmsSlot1ID) +
                            COUNT(DISTINCT ArmsSlot2ID) +
                            COUNT(DISTINCT ArmsSlot3ID) +
                            COUNT(DISTINCT WaistSlot1ID) +
                            COUNT(DISTINCT WaistSlot2ID) +
                            COUNT(DISTINCT WaistSlot3ID) +
                            COUNT(DISTINCT LegsSlot1ID) +
                            COUNT(DISTINCT LegsSlot2ID) +
                            COUNT(DISTINCT LegsSlot3ID) AS TotalUniqueDecorations
                        FROM PlayerGear
                        ";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        totalUniqueDecorations += (long)reader["TotalUniqueDecorations"];
                    }
                }
            }

            return totalUniqueDecorations;
        }

        /// <summary>
        /// Gets the most common decoration identifier.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private long GetMostCommonDecorationID(SQLiteConnection conn)
        {
            Dictionary<long, long> decorationCounts = new Dictionary<long, long>();

            var query = @"
                        SELECT HeadSlot1ID, HeadSlot2ID, HeadSlot3ID, 
                               ChestSlot1ID, ChestSlot2ID, ChestSlot3ID, 
                               ArmsSlot1ID, ArmsSlot2ID, ArmsSlot3ID, 
                               WaistSlot1ID, WaistSlot2ID, WaistSlot3ID, 
                               LegsSlot1ID, LegsSlot2ID, LegsSlot3ID 
                        FROM PlayerGear
                        ";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long?[] decorationIDs = {
                                        reader.IsDBNull(0) ? null : (long?)reader.GetInt64(0),
                                        reader.IsDBNull(1) ? null : (long?)reader.GetInt64(1),
                                        reader.IsDBNull(2) ? null : (long?)reader.GetInt64(2),
                                        reader.IsDBNull(3) ? null : (long?)reader.GetInt64(3),
                                        reader.IsDBNull(4) ? null : (long?)reader.GetInt64(4),
                                        reader.IsDBNull(5) ? null : (long?)reader.GetInt64(5),
                                        reader.IsDBNull(6) ? null : (long?)reader.GetInt64(6),
                                        reader.IsDBNull(7) ? null : (long?)reader.GetInt64(7),
                                        reader.IsDBNull(8) ? null : (long?)reader.GetInt64(8),
                                        reader.IsDBNull(9) ? null : (long?)reader.GetInt64(9),
                                        reader.IsDBNull(10) ? null : (long?)reader.GetInt64(10),
                                        reader.IsDBNull(11) ? null : (long?)reader.GetInt64(11),
                                        reader.IsDBNull(12) ? null : (long?)reader.GetInt64(12),
                                        reader.IsDBNull(13) ? null : (long?)reader.GetInt64(13),
                                    };

                        foreach (long? decorationID in decorationIDs)
                        {
                            if (decorationID.HasValue && decorationID.Value != 0)
                            {
                                if (decorationCounts.ContainsKey(decorationID.Value))
                                {
                                    decorationCounts[decorationID.Value]++;
                                }
                                else
                                {
                                    decorationCounts[decorationID.Value] = 1;
                                }
                            }
                        }
                    }
                }
            }

            long? mostCommonDecorationID = decorationCounts.OrderByDescending(x => x.Value).Select(x => (long?)x.Key).FirstOrDefault();

            return (long)mostCommonDecorationID;        
        }

        private long GetLeastUsedArmorSkillID(SQLiteConnection conn)
        {
            long leastUsedArmorSkillID = 0;

            var query = @"
                        SELECT ActiveSkill1ID, ActiveSkill2ID, ActiveSkill3ID, ActiveSkill4ID, ActiveSkill5ID,
                               ActiveSkill6ID, ActiveSkill7ID, ActiveSkill8ID, ActiveSkill9ID, ActiveSkill10ID,
                               ActiveSkill11ID, ActiveSkill12ID, ActiveSkill13ID, ActiveSkill14ID, ActiveSkill15ID,
                               ActiveSkill16ID, ActiveSkill17ID, ActiveSkill18ID, ActiveSkill19ID
                        FROM ActiveSkills
                        ";

            Dictionary<long, long> skillCounts = new Dictionary<long, long>();

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            long skillID = reader.GetInt64(i);

                            if (skillID != 0)
                            {
                                if (skillCounts.ContainsKey(skillID))
                                {
                                    skillCounts[skillID]++;
                                }
                                else
                                {
                                    skillCounts[skillID] = 1;
                                }
                            }
                        }
                    }
                }
            }

            leastUsedArmorSkillID = skillCounts.OrderBy(x => x.Value).Select(x => x.Key).FirstOrDefault();

            return (long)leastUsedArmorSkillID;
        }

        /// <summary>
        /// Gets the maximum value with WHERE clause.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="table">The table.</param>
        /// <param name="conn">The connection.</param>
        /// <param name="whereField">The where field.</param>
        /// <param name="whereValue">The where value.</param>
        /// <returns></returns>
        private static long GetMaxValueWithWhere(string field, string table, SQLiteConnection conn, string whereField, long whereValue)
        {
            string query = $"SELECT MAX({field}) FROM {table} WHERE {whereField} = @whereValue";
            using (var command = new SQLiteCommand(query, conn))
            {
                command.Parameters.AddWithValue("@whereValue", whereValue);
                return (long)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Gets the minimum value with WHERE clause.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="table">The table.</param>
        /// <param name="conn">The connection.</param>
        /// <param name="whereField">The where field.</param>
        /// <param name="whereValue">The where value.</param>
        /// <returns></returns>
        private static long GetMinValueWithWhere(string field, string table, SQLiteConnection conn, string whereField, long whereValue)
        {
            string query = $"SELECT MIN({field}) FROM {table} WHERE {whereField} = @whereValue";
            using (var command = new SQLiteCommand(query, conn))
            {
                command.Parameters.AddWithValue("@whereValue", whereValue);
                return (long)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Gets the average value with WHERE clause.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="table">The table.</param>
        /// <param name="conn">The connection.</param>
        /// <param name="whereField">The where field.</param>
        /// <param name="whereValue">The where value.</param>
        /// <returns></returns>
        private static double GetAverageValueWithWhere(string field, string table, SQLiteConnection conn, string whereField, long whereValue)
        {
            string query = $"SELECT AVG({field}) FROM {table} WHERE {whereField} = @whereValue";
            using (var command = new SQLiteCommand(query, conn))
            {
                command.Parameters.AddWithValue("@whereValue", whereValue);
                return (double)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Gets the median value with WHERE clause.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="table">The table.</param>
        /// <param name="conn">The connection.</param>
        /// <param name="whereField">The where field.</param>
        /// <param name="whereValue">The where value.</param>
        /// <returns></returns>
        private static double GetMedianValueWithWhere(string field, string table, SQLiteConnection conn, string whereField, long whereValue)
        {
            // TODO: not sure if correct
            string query = $"SELECT AVG({field}) FROM (SELECT {field}, ROW_NUMBER() OVER (ORDER BY {field}) AS RowNum, COUNT(*) OVER() AS TotalRows FROM {table} WHERE {whereField} = @whereValue) temp WHERE RowNum BETWEEN (TotalRows/2) + 1 AND (TotalRows/2) + 2;";
            using (var command = new SQLiteCommand(query, conn))
            {
                command.Parameters.AddWithValue("@whereValue", whereValue);
                return (double)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Gets the record with highest value in field.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private (double, long) GetRecordWithHighestValueInField(SQLiteConnection conn, string tableName, string fieldName)
        {
            var query = $"SELECT RunID, {fieldName} FROM {tableName}";
            long highestValueRunID = 0;
            double highestValue = 0;

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long runID = (long)reader["RunID"];
                        string fieldValueString = (string)reader[fieldName];

                        if (string.IsNullOrEmpty(fieldValueString) || fieldValueString == "{}")
                            continue;

                        var fieldValue = JObject.Parse(fieldValueString)
                                                .ToObject<Dictionary<double, Dictionary<double, double>>>();

                        double fieldValueMax = fieldValue.Max(kv1 => kv1.Value.Max(kv2 => kv2.Value));
                        if (fieldValueMax > highestValue)
                        {
                            highestValue = fieldValueMax;
                            highestValueRunID = runID;
                        }
                    }
                }
            }

            return (highestValue, highestValueRunID);
        }

        /// <summary>
        /// Gets the record with lowest value in field.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private (double, long) GetRecordWithLowestValueInField(SQLiteConnection conn, string tableName, string fieldName)
        {
            var query = $"SELECT RunID, {fieldName} FROM {tableName}";
            long lowestValueRunID = 0;
            double lowestValue = double.MaxValue;

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long runID = (long)reader["RunID"];
                        string fieldValueString = (string)reader[fieldName];

                        if (string.IsNullOrEmpty(fieldValueString) || fieldValueString == "{}")
                            continue;

                        var fieldValue = JObject.Parse(fieldValueString)
                                                .ToObject<Dictionary<double, Dictionary<double, double>>>();

                        double value = fieldValue.Min(kv1 => kv1.Value.Min(kv2 => kv2.Value));
                        if (value < lowestValue)
                        {
                            lowestValue = value;
                            lowestValueRunID = runID;
                        }
                    }
                }
            }

            return (lowestValue, lowestValueRunID);
        }

        /// <summary>
        /// Gets the most completed quest run.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private (long, long) GetMostCompletedQuestRun(SQLiteConnection conn)
        {
            long timesCompleted = 0;
            long questId = 0;

            var query = @"
                        SELECT QuestID, COUNT(*) as TimesCompleted
                        FROM Quests
                        GROUP BY QuestID
                        ORDER BY TimesCompleted DESC
                        LIMIT 1";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // The most completed quest ID
                        questId = (long)reader["QuestID"];

                        // The number of times the quest was completed
                        timesCompleted = (long)reader["TimesCompleted"];

                    }
                }
            }

            return (timesCompleted, questId);
        }

        /// <summary>
        /// Gets the most completed quest runs attempted.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private long GetMostCompletedQuestRunsAttempted(SQLiteConnection conn, long MostCompletedQuestRunsQuestID)
        {
            long attempts = 0;
            var query = @"
                SELECT SUM(Attempts) AS TotalAttempts
                FROM QuestAttempts
                WHERE QuestID = @questId";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@questId", MostCompletedQuestRunsQuestID);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        attempts = (long)reader["TotalAttempts"];
                    }
                }
            }

            return attempts;
        }

        /// <summary>
        /// Gets the most attempted quest run.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private (long, long) GetMostAttemptedQuestRun(SQLiteConnection conn)
        {
            long questID = 0;
            long attempts = 0;
            var query = @"
            SELECT q.QuestID, SUM(q.Attempts) AS Attempts
            FROM QuestAttempts q
            JOIN (
                SELECT QuestID, COUNT(*) AS TotalAttempts
                FROM QuestAttempts
                GROUP BY QuestID
                ORDER BY TotalAttempts DESC
                LIMIT 1
            ) m ON q.QuestID = m.QuestID
            GROUP BY q.QuestID";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        questID = (long)reader["QuestID"];
                        attempts = (long)reader["Attempts"];
                    }
                }
            }
            return (attempts, questID);
        }

        /// <summary>
        /// Gets the most attempted quest runs completed.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="MostAttemptedQuestRunsQuestID">The most attempted quest runs quest identifier.</param>
        /// <returns></returns>
        private long GetMostAttemptedQuestRunsCompleted(SQLiteConnection conn, long MostAttemptedQuestRunsQuestID)
        {
            long timesCompleted = 0;

            var query = @"
                        SELECT COUNT(*) AS TimesCompleted
                        FROM Quests
                        WHERE QuestID = @QuestID";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@QuestID", MostAttemptedQuestRunsQuestID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // The number of times the most attempted quest was completed
                        timesCompleted = (long)reader["TimesCompleted"];

                    }
                }
            }
            return timesCompleted;
        }

        /// <summary>
        /// Get the sum of values in a given field in a given table, excluding null values
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static long GetSumValue(string field, string table, SQLiteConnection conn)
        {
            string query = $"SELECT SUM({field}) FROM {table} WHERE {field} IS NOT NULL";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                long sum = Convert.ToInt64(cmd.ExecuteScalar());
                return sum;
            }
        }

        /// <summary>
        /// Takes the field parameter and returns the percentage of non-zero values in that field
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="table">The table.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private static double GetNonZeroPercentageOfField(string field, string table, SQLiteConnection conn)
        {
            // Initialize variables to hold the total number of entries and the number of entries with non-zero field values
            long totalEntries = 0;
            long nonZeroEntries = 0;

            // Query to get the total number of entries and the number of entries with non-zero field values
            string query = $"SELECT COUNT(*) AS TotalEntries, COUNT(CASE WHEN {field} != 0 THEN 1 ELSE NULL END) AS NonZeroEntries FROM {table}";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        totalEntries = (long)reader["TotalEntries"];
                        nonZeroEntries = (long)reader["NonZeroEntries"];
                    }
                }
            }

            // Calculate the percentage of non-zero values
            double percentage = nonZeroEntries * 100.0 / totalEntries;

            return percentage;
        }

        /// <summary>
        /// Gets the solo quests percentage.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private double GetSoloQuestsPercentage(SQLiteConnection conn)
        {
            // Initialize variables to hold the total number of quests and the number of solo quests
            long totalQuests = 0;
            long soloQuests = 0;

            // Query to get the total number of quests and the number of solo quests
            var query = @"
                        SELECT COUNT(*) AS TotalQuests,
                               COUNT(CASE WHEN PartySize = 1 THEN 1 ELSE NULL END) AS SoloQuests
                        FROM Quests
                    ";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        totalQuests = (long)reader["TotalQuests"];
                        soloQuests = (long)reader["SoloQuests"];
                    }
                }
            }

            // Calculate the percentage of solo quests
            return soloQuests * 100.0 / totalQuests;
        }

        #endregion

        #region compendium

        public QuestCompendium GetQuestCompendium()
        {
            QuestCompendium questCompendium = new QuestCompendium();
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // TODO i guess this works?
                        (questCompendium.MostCompletedQuestRuns, questCompendium.MostCompletedQuestRunsQuestID) = GetMostCompletedQuestRun(conn);
                        questCompendium.MostCompletedQuestRunsAttempted = GetMostCompletedQuestRunsAttempted(conn, questCompendium.MostCompletedQuestRunsQuestID);
                        (questCompendium.MostAttemptedQuestRuns, questCompendium.MostAttemptedQuestRunsQuestID) = GetMostAttemptedQuestRun(conn);
                        questCompendium.MostAttemptedQuestRunsCompleted = GetMostAttemptedQuestRunsCompleted(conn, questCompendium.MostAttemptedQuestRunsQuestID);
                        questCompendium.TotalQuestsCompleted = GetTableRowCount("RunID", "Quests", conn);
                        questCompendium.TotalQuestsAttempted = GetSumValue("Attempts", "QuestAttempts", conn);
                        questCompendium.QuestCompletionTimeElapsedAverage = GetAvgValue("FinalTimeValue", "Quests", conn);
                        questCompendium.QuestCompletionTimeElapsedMedian = GetMedianValue("FinalTimeValue", "Quests", conn);

                        var query = @"SELECT SUM(FinalTimeValue) AS TotalTimeElapsed
                        FROM Quests
                        ";

                        using (var cmd = new SQLiteCommand(query, conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    long value = (long)reader["TotalTimeElapsed"];

                                    questCompendium.TotalTimeElapsedQuests = value;

                                }
                            }
                        }

                        // Initialize a list to hold the final cart values
                        List<int> finalCartValues = new List<int>();

                        // Query to get the last cart value from each quest entry
                        query = @"SELECT CartsDictionary FROM Quests WHERE CartsDictionary IS NOT NULL";

                        using (var cmd = new SQLiteCommand(query, conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string cartsDictionaryJson = reader.GetString(0);
                                    if (!string.IsNullOrEmpty(cartsDictionaryJson) && cartsDictionaryJson != "{}")
                                    {
                                        // Deserialize the carts dictionary JSON string
                                        Dictionary<int, int> cartsDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartsDictionaryJson);

                                        // Get the last cart value from the dictionary
                                        int finalCartValue = cartsDictionary.Values.LastOrDefault();

                                        // Add the final cart value to the list
                                        finalCartValues.Add(finalCartValue);
                                    }
                                }
                            }
                        }

                        // Calculate the average and median final cart values
                        if (finalCartValues.Count > 0)
                        {
                            questCompendium.TotalCartsInQuestAverage = finalCartValues.Average();
                            questCompendium.TotalCartsInQuestMedian = CalculateMedianOfList(finalCartValues);
                        }
                        else
                        {
                            // No quest entries with non-empty carts dictionaries found
                            questCompendium.TotalCartsInQuestAverage = 0;
                            questCompendium.TotalCartsInQuestMedian = 0;
                        }

                        questCompendium.TotalCartsInQuest = finalCartValues.Sum();

                        // Initialize dictionary to hold the total carts for each quest ID
                        Dictionary<int, int> questTotalCarts = new Dictionary<int, int>();

                        // Query to get carts dictionary for all quests with non-empty carts dictionary
                        query = @"SELECT QuestId, CartsDictionary FROM Quests WHERE CartsDictionary IS NOT NULL AND CartsDictionary != '{}'";

                        using (var cmd = new SQLiteCommand(query, conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int questId = Convert.ToInt32(reader["QuestId"]);
                                    string cartsDictionaryJson = reader.GetString(1);

                                    // Deserialize carts dictionary JSON string
                                    Dictionary<int, int> cartsDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartsDictionaryJson);

                                    // Add total carts for this quest to the questTotalCarts dictionary
                                    int totalCarts = cartsDictionary.Values.Sum();
                                    questTotalCarts[questId] = totalCarts;
                                }
                            }
                        }

                        // Get quest ID with the most total carts
                        if (questTotalCarts.Count > 0)
                        {
                            int mostCompletedQuestId = questTotalCarts.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                            int totalCartsInMostCompletedQuest = questTotalCarts[mostCompletedQuestId];

                            questCompendium.MostCompletedQuestWithCartsQuestID = mostCompletedQuestId;
                            questCompendium.MostCompletedQuestWithCarts = totalCartsInMostCompletedQuest;
                        }
                        else
                        {
                            // No quest entries with non-empty carts dictionaries found
                            questCompendium.MostCompletedQuestWithCartsQuestID = 0;
                            questCompendium.MostCompletedQuestWithCarts = 0;
                        }

                        questCompendium.PercentOfSoloQuests = GetSoloQuestsPercentage(conn);
                        questCompendium.QuestPartySizeAverage = GetAvgValue("PartySize", "Quests", conn);
                        questCompendium.QuestPartySizeMedian = GetMedianValue("PartySize", "Quests", conn);
                        questCompendium.QuestPartySizeMode = GetModeValue("PartySize", "Quests", conn);
                        questCompendium.PercentOfGuildFood = GetNonZeroPercentageOfField("GuildFoodID", "PlayerGear", conn);
                        questCompendium.MostCommonGuildFood = GetModeValue("GuildFoodID", "PlayerGear", conn);
                        questCompendium.MostCommonDivaSkill = GetModeValue("DivaSkillID", "PlayerGear", conn);
                        questCompendium.PercentOfDivaSkill = GetNonZeroPercentageOfField("DivaSkillID", "PlayerGear", conn);

                        // Query to get the item IDs for each player gear row
                        query = @"
                        SELECT 
                            PlayerInventoryID, 
                            Item1ID, 
                            Item2ID, 
                            Item3ID, 
                            Item4ID, 
                            Item5ID,
                            Item6ID,
                            Item7ID,
                            Item8ID,
                            Item9ID,
                            Item10ID,
                            Item11ID,
                            Item12ID,
                            Item13ID,
                            Item14ID,
                            Item15ID,
                            Item16ID,
                            Item17ID,
                            Item18ID,
                            Item19ID,
                            Item20ID
                        FROM PlayerInventory";

                        // Create a list to hold the PlayerInventoryItemIds objects
                        List<PlayerInventoryItemIds> playerInventoryItemIdsList = new List<PlayerInventoryItemIds>();

                        // Loop through the query results and create a PlayerInventoryItemIds object for each row
                        using (var cmd = new SQLiteCommand(query, conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    long playerInventoryID = (long)reader["PlayerInventoryID"];
                                    List<long> itemIds = new List<long>();

                                    for (int i = 1; i <= 20; i++)
                                    {
                                        long itemId = (long)reader[$"Item{i}ID"];
                                        itemIds.Add(itemId);
                                    }

                                    var playerInventoryItemIds = new PlayerInventoryItemIds
                                    {
                                        PlayerInventoryID = playerInventoryID,
                                        ItemIds = itemIds
                                    };

                                    playerInventoryItemIdsList.Add(playerInventoryItemIds);
                                }
                            }
                        }

                        // Loop through the PlayerInventoryItemIds objects and check if any of them have a skill fruit item ID
                        int skillFruitUsageCount = 0;
                        foreach (var playerInventoryItemIds in playerInventoryItemIdsList)
                        {
                            foreach (var itemId in playerInventoryItemIds.ItemIds)
                            {
                                if (Dictionary.SkillFruits.ItemID.ContainsKey(itemId))
                                {
                                    skillFruitUsageCount++;
                                    break;
                                }
                            }
                        }

                        // Calculate the percentage of skill fruit usage
                        double skillFruitUsagePercentage = skillFruitUsageCount * 100.0 / GetTableRowCount("RunID", "Quests", conn);

                        questCompendium.PercentOfSkillFruit = skillFruitUsagePercentage;

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        HandleError(transaction, ex);
                    }
                }
            }
            return questCompendium;
        }

        public GearCompendium GetGearCompendium()
        {
            GearCompendium gearCompendium = new GearCompendium();
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        gearCompendium.MostUsedWeaponType = GetModeValue("WeaponTypeID", "PlayerGear", conn);

                        gearCompendium.TotalUniqueArmorPiecesUsed = GetTotalUniqueArmorPieces(conn);

                        gearCompendium.TotalUniqueWeaponsUsed = GetTotalUniqueWeaponIDs(conn);

                        gearCompendium.TotalUniqueDecorationsUsed = GetTotalUniqueDecorations(conn);

                        gearCompendium.MostCommonDecorationID = GetMostCommonDecorationID(conn);

                        gearCompendium.LeastUsedArmorSkill = GetLeastUsedArmorSkillID(conn);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        HandleError(transaction, ex);
                    }
                }
            }

            return gearCompendium;
        }

        public PerformanceCompendium GetPerformanceCompendium()
        {
            PerformanceCompendium performanceCompendium = new PerformanceCompendium();
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        (performanceCompendium.HighestTrueRaw, performanceCompendium.HighestTrueRawRunID) = GetRowWithHighestDictionaryValue("Quests", "AttackBuffDictionary", conn);
                        performanceCompendium.TrueRawAverage = GetAverageOfDictionaryField("Quests", "AttackBuffDictionary", conn);
                        performanceCompendium.TrueRawMedian = GetMedianOfDictionaryField("Quests", "AttackBuffDictionary", conn);

                        (performanceCompendium.HighestActionsPerMinute, performanceCompendium.HighestActionsPerMinuteRunID) = GetRowWithHighestDictionaryValue("Quests", "ActionsPerMinuteDictionary", conn);
                        performanceCompendium.ActionsPerMinuteAverage = GetAverageOfDictionaryField("Quests", "ActionsPerMinuteDictionary", conn);
                        performanceCompendium.ActionsPerMinuteMedian = GetMedianOfDictionaryField("Quests", "ActionsPerMinuteDictionary", conn);

                        (performanceCompendium.HighestDPS, performanceCompendium.HighestDPSRunID) = GetRowWithHighestDictionaryValue("Quests", "DamagePerSecondDictionary", conn);
                        performanceCompendium.DPSAverage = GetAverageOfDictionaryField("Quests", "DamagePerSecondDictionary", conn);
                        performanceCompendium.DPSMedian = GetMedianOfDictionaryField("Quests", "DamagePerSecondDictionary", conn);

                        (performanceCompendium.HighestHitCount, performanceCompendium.HighestHitCountRunID) = GetRowWithHighestDictionaryValue("Quests", "HitCountDictionary", conn);
                        performanceCompendium.HitCountAverage = GetAverageOfDictionaryField("Quests", "HitCountDictionary", conn);
                        performanceCompendium.HitCountMedian = GetMedianOfDictionaryField("Quests", "HitCountDictionary", conn);

                        (performanceCompendium.HighestHitsPerSecond, performanceCompendium.HighestHitsPerSecondRunID) = GetRowWithHighestDictionaryValue("Quests", "HitsPerSecondDictionary", conn);
                        performanceCompendium.HitsPerSecondAverage = GetAverageOfDictionaryField("Quests", "HitsPerSecondDictionary", conn);
                        performanceCompendium.HitsPerSecondMedian = GetMedianOfDictionaryField("Quests", "HitsPerSecondDictionary", conn);

                        (performanceCompendium.HighestHitsTakenBlockedPerSecond, performanceCompendium.HighestHitsTakenBlockedPerSecondRunID) = GetRowWithHighestDictionaryValue("Quests", "HitsTakenBlockedPerSecondDictionary", conn);
                        performanceCompendium.HitsTakenBlockedPerSecondAverage = GetAverageOfDictionaryField("Quests", "HitsTakenBlockedPerSecondDictionary", conn);
                        performanceCompendium.HitsTakenBlockedPerSecondMedian = GetMedianOfDictionaryField("Quests", "HitsTakenBlockedPerSecondDictionary", conn); ;

                        (performanceCompendium.HighestSingleHitDamage, performanceCompendium.HighestSingleHitDamageRunID) = GetRowWithHighestDictionaryValue("Quests", "DamageDealtDictionary", conn);
                        performanceCompendium.SingleHitDamageAverage = GetAverageOfDictionaryField("Quests", "DamageDealtDictionary", conn);
                        performanceCompendium.SingleHitDamageMedian = GetMedianOfDictionaryField("Quests", "DamageDealtDictionary", conn);

                        (performanceCompendium.HighestHitsTakenBlocked, performanceCompendium.HighestHitsTakenBlockedRunID) = GetQuestWithHighestHitsTakenBlocked(conn);
                        performanceCompendium.HitsTakenBlockedAverage = GetAverageHitsTakenBlockedCount(conn);
                        performanceCompendium.HitsTakenBlockedMedian = GetMedianHitsTakenBlockedCount(conn);

                        performanceCompendium.TotalActions = GetTotalCountOfValueInDictionaryField("KeystrokesDictionary", "Quests", conn) + GetTotalCountOfValueInDictionaryField("MouseInputDictionary", "Quests", conn) + GetTotalCountOfValueInDictionaryField("GamepadInputDictionary", "Quests", conn);
                        performanceCompendium.TotalHitsCount = GetTotalCountOfValueInDictionaryField("HitCountDictionary", "Quests", conn);
                        performanceCompendium.TotalHitsTakenBlocked = GetTotalCountOfValueInDictionaryField("HitsTakenBlockedDictionary", "Quests", conn);

                        performanceCompendium.HealthAverage = GetAverageOfDictionaryField("Quests", "PlayerHPDictionary", conn);
                        performanceCompendium.HealthMedian = GetMedianOfDictionaryField("Quests", "PlayerHPDictionary", conn);
                        performanceCompendium.HealthMode = GetModeOfDictionaryField("Quests", "PlayerHPDictionary", conn);

                        performanceCompendium.StaminaAverage = GetAverageOfDictionaryField("Quests", "PlayerStaminaDictionary", conn);
                        performanceCompendium.StaminaMedian = GetMedianOfDictionaryField("Quests", "PlayerStaminaDictionary", conn);
                        performanceCompendium.StaminaMode = GetModeOfDictionaryField("Quests", "PlayerStaminaDictionary", conn);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        HandleError(transaction, ex);
                    }
                }
            }
            return performanceCompendium;
        }

        public MezFesCompendium GetMezFesCompendium()
        {
            MezFesCompendium mezFesCompendium = new MezFesCompendium();
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        mezFesCompendium.MinigamesPlayed = GetTableRowCount("MezFesID", "MezFes", conn);
                        mezFesCompendium.UrukiPachinkoTimesPlayed = GetCountOfIntValue("MezFesMinigameID", "MezFes", 464, conn);
                        mezFesCompendium.UrukiPachinkoHighscore = GetMaxValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 464);
                        mezFesCompendium.UrukiPachinkoAverageScore = GetAverageValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 464);
                        mezFesCompendium.UrukiPachinkoMedianScore = GetMedianValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 464);

                        mezFesCompendium.GuukuScoopTimesPlayed = GetCountOfIntValue("MezFesMinigameID", "MezFes", 466, conn);
                        mezFesCompendium.GuukuScoopHighscore = GetMaxValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 466);
                        mezFesCompendium.GuukuScoopAverageScore = GetAverageValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 466);
                        mezFesCompendium.GuukuScoopMedianScore = GetMedianValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 466);

                        mezFesCompendium.NyanrendoTimesPlayed = GetCountOfIntValue("MezFesMinigameID", "MezFes", 467, conn);
                        mezFesCompendium.NyanrendoHighscore = GetMaxValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 467);
                        mezFesCompendium.NyanrendoAverageScore = GetAverageValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 467);
                        mezFesCompendium.NyanrendoMedianScore = GetMedianValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 467);

                        mezFesCompendium.PanicHoneyTimesPlayed = GetCountOfIntValue("MezFesMinigameID", "MezFes", 468, conn);
                        mezFesCompendium.PanicHoneyHighscore = GetMaxValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 468);
                        mezFesCompendium.PanicHoneyAverageScore = GetAverageValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 468);
                        mezFesCompendium.PanicHoneyMedianScore = GetMedianValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 468);

                        mezFesCompendium.DokkanBattleCatsTimesPlayed = GetCountOfIntValue("MezFesMinigameID", "MezFes", 469, conn);
                        mezFesCompendium.DokkanBattleCatsHighscore = GetMaxValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 469);
                        mezFesCompendium.DokkanBattleCatsAverageScore = GetAverageValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 469);
                        mezFesCompendium.DokkanBattleCatsMedianScore = GetMedianValueWithWhere("Score", "MezFes", conn, "MezFesMinigameID", 469);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        HandleError(transaction, ex);
                    }
                }
            }
            return mezFesCompendium;
        }

        public MonsterCompendium GetMonsterCompendium()
        {
            MonsterCompendium monsterCompendium = new MonsterCompendium();
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        (monsterCompendium.HighestMonsterAttackMultiplier, monsterCompendium.HighestMonsterAttackMultiplierRunID) = GetRecordWithHighestValueInField(conn, "Quests", "Monster1AttackMultiplierDictionary");
                        (monsterCompendium.LowestMonsterAttackMultiplier, monsterCompendium.LowestMonsterAttackMultiplierRunID) = GetRecordWithLowestValueInField(conn, "Quests", "Monster1AttackMultiplierDictionary");

                        (monsterCompendium.HighestMonsterDefenseRate, monsterCompendium.HighestMonsterDefenseRateRunID) = GetRecordWithHighestValueInField(conn, "Quests", "Monster1DefenseRateDictionary");
                        (monsterCompendium.LowestMonsterDefenseRate, monsterCompendium.LowestMonsterDefenseRateRunID) = GetRecordWithLowestValueInField(conn, "Quests", "Monster1DefenseRateDictionary");

                        (monsterCompendium.HighestMonsterSizeMultiplier, monsterCompendium.HighestMonsterSizeMultiplierRunID) = GetRecordWithHighestValueInField(conn, "Quests", "Monster1SizeMultiplierDictionary");
                        (monsterCompendium.LowestMonsterSizeMultiplier, monsterCompendium.LowestMonsterSizeMultiplierRunID) = GetRecordWithLowestValueInField(conn, "Quests", "Monster1SizeMultiplierDictionary");

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        HandleError(transaction, ex);
                    }
                }
            }
            return monsterCompendium;
        }

        public MiscellaneousCompendium GetMiscellaneousCompendium()
        {
            MiscellaneousCompendium miscellaneousCompendium = new MiscellaneousCompendium();
            using (SQLiteConnection conn = new SQLiteConnection(dataSource))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        miscellaneousCompendium.TotalOverlaySessions = GetTableRowCount("SessionID", "Session", conn);
                        miscellaneousCompendium.HighestSessionDuration = GetMaxValue("SessionDuration", "Session", conn);
                        miscellaneousCompendium.LowestSessionDuration = GetMinValue("SessionDuration", "Session", conn);
                        miscellaneousCompendium.AverageSessionDuration = GetAvgValue("SessionDuration", "Session", conn);
                        miscellaneousCompendium.MedianSessionDuration = GetMedianValue("SessionDuration", "Session", conn);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        HandleError(transaction, ex);
                    }
                }
            }
            return miscellaneousCompendium;
        }

        #endregion

        #region migrations

        private int GetUserVersion(SQLiteConnection connection)
        {
            int version = 0;
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    string sql = @"PRAGMA user_version";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            version = Convert.ToInt32(result);
                            logger.Info("Current user_version is {0}", version);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    HandleError(transaction, ex);
                }
            }
            return version;
        }

        private void SetUserVersion(SQLiteConnection connection, int version)
        {
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    string sql = $"PRAGMA user_version = {version}";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    logger.Info("Set user_version to {0}", version);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    HandleError(transaction, ex);
                }
            }
        }

        // TODO: i dont like using goto, but it seems to make code more succinct in this case.
        // https://stackoverflow.com/questions/550662/database-schema-updates
        // https://stackoverflow.com/questions/989558/best-practices-for-in-app-database-migration-for-sqlite
        /*
        The only schema altering commands directly supported by SQLite are the "rename table", "rename column", 
        "add column", "drop column" commands shown above. However, applications can make other arbitrary changes to 
        the format of a table using a simple sequence of operations. The steps to make arbitrary 
        changes to the schema design of some table X are as follows:

        1. If foreign key constraints are enabled, disable them using PRAGMA foreign_keys=OFF.

        2. Start a transaction.

        3. Remember the format of all indexes, triggers, and views associated with table X. This information will be needed in step 8 below. One way to do this is to run a query like the following: SELECT type, sql FROM sqlite_schema WHERE tbl_name='X'.

        4. Use CREATE TABLE to construct a new table "new_X" that is in the desired revised format of table X. Make sure that the name "new_X" does not collide with any existing table name, of course.

        5. Transfer content from X into new_X using a statement like: INSERT INTO new_X SELECT ... FROM X.

        6. Drop the old table X: DROP TABLE X.

        7. Change the name of new_X to X using: ALTER TABLE new_X RENAME TO X.

        8. Use CREATE INDEX, CREATE TRIGGER, and CREATE VIEW to reconstruct indexes, triggers, and views associated with table X. Perhaps use the old format of the triggers, indexes, and views saved from step 3 above as a guide, making changes as appropriate for the alteration.

        9. If any views refer to table X in a way that is affected by the schema change, then drop those views using DROP VIEW and recreate them with whatever changes are necessary to accommodate the schema change using CREATE VIEW.

        10. If foreign key constraints were originally enabled then run PRAGMA foreign_key_check to verify that the schema change did not break any foreign key constraints.

        11. Commit the transaction started in step 2.

        12. If foreign keys constraints were originally enabled, re-enable them now.
         */
        private void MigrateToSchemaFromVersion(SQLiteConnection conn, int fromVersion) 
        {
            // 1. If foreign key constraints are enabled, disable them using PRAGMA foreign_keys=OFF.
            DisableForeignKeyConstraints(conn);

            // 2. Start a transaction.
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    int newVersion = fromVersion;
                    // allow migrations to fall through switch cases to do a complete run
                    // start with current version + 1
                    //[self beginTransaction];
                    switch (fromVersion)
                    {
                        default:
                            logger.Info("No new schema updates found. Schema version: {0}", fromVersion);
                            MessageBox.Show(string.Format(
@"No new schema updates found! Schema version: {0}", fromVersion), 
                                string.Format("MHF-Z Overlay Database Update ({0} to {1})", 
                                previousVersion, 
                                App.CurrentProgramVersion), 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Information);
                            break;
                        case 0://v0.22.0 or older (TODO: does this work with older or just v0.22.0?)
                            // change pin type to mode 'pin' for keyboard handling changes
                            // removing types from previous schema
                            //sqlite3_exec(db, "DELETE FROM types;", NULL, NULL, NULL);
                            //NSLog(@"installing current types");
                            //[self loadInitialData];
                            {
                                PerformUpdateToVersion_0_23_0(conn);
                                newVersion++;
                                logger.Info("Updated schema to version v0.23.0. user_version {0}", newVersion);
                                break;
                                //goto case 1;
                            }
                        //case 1://v0.23.0
                            //adds support for recent view tracking
                            //sqlite3_exec(db, "ALTER TABLE entries ADD COLUMN touched_at TEXT;", NULL, NULL, NULL);
                            //{
                                //PerformUpdateToVersion_0_24_0(conn);
                                //newVersion++;
                                //goto case 2;
                            //}
                        //case 2://v0.24.0
                            //sqlite3_exec(db, "ALTER TABLE categories ADD COLUMN image TEXT;", NULL, NULL, NULL);
                            //sqlite3_exec(db, "ALTER TABLE categories ADD COLUMN entry_count INTEGER;", NULL, NULL, NULL);
                            //sqlite3_exec(db, "CREATE INDEX IF NOT EXISTS categories_id_idx ON categories(id);", NULL, NULL, NULL);
                            //sqlite3_exec(db, "CREATE INDEX IF NOT EXISTS categories_name_id ON categories(name);", NULL, NULL, NULL);
                            //sqlite3_exec(db, "CREATE INDEX IF NOT EXISTS entries_id_idx ON entries(id);", NULL, NULL, NULL);

                            // etc...
                            //{
                                //PerformUpdateToVersion_0_25_0(conn);
                                //newVersion++;
                                //break;
                            //}
                    }

                    //[self setSchemaVersion];
                    SetUserVersion(conn, newVersion);

                    // 11. Commit the transaction started in step 2.
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    HandleError(transaction, ex);
                }
            }

            // 12. If foreign keys constraints were originally enabled, re-enable them now.
            EnableForeignKeyConstraints(conn);
            //[self endTransaction];
        }

        private void UpdateDatabaseSchema(SQLiteConnection connection)
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    int currentUserVersion = GetUserVersion(connection);

                    if (App.isClowdSquirrelUpdating == false && (App.CurrentProgramVersion.Trim() != previousVersion.Trim() || currentUserVersion == 0))
                    {
                        logger.Info("Found different program version or userVersion 0. Current: {0}, Previous: {1}, userVersion: {2}", App.CurrentProgramVersion, previousVersion, currentUserVersion);

                        MessageBoxResult result = MessageBox.Show(
@"A new version of the program has been installed.

Do you want to perform the necessary database updates? A backup of your current MHFZ_Overlay.sqlite file will be done if you accept.

Updating the database structure may take some time, it will transport all of your current data straight to the latest database structure, regardless of the previous database version.",
                                                         string.Format("MHF-Z Overlay Database Update ({0} to {1})", previousVersion, App.CurrentProgramVersion), MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                        if (result == MessageBoxResult.Yes)
                        {
                            // Make a backup of the current SQLite file before updating the schema

                            // Get the process that is running "mhf.exe"
                            Process[] processes = Process.GetProcessesByName("mhf");

                            if (processes.Length > 0)
                            {
                                FileManager.CreateDatabaseBackup(connection, BackupFolderName);
                            }
                            else
                            {
                                // The "mhf.exe" process was not found
                                logger.Fatal("mhf.exe not found");
                                MessageBox.Show("The 'mhf.exe' process was not found.", LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                                ApplicationManager.HandleShutdown();
                            }

                            MigrateToSchemaFromVersion(connection, currentUserVersion);
                            var referenceSchemaFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MHFZ_Overlay\\reference_schema.json");
                            FileManager.DeleteFile(referenceSchemaFilePath);
                            // later on it creates it
                            // see this comment: Check if the reference schema file exists
                            //MessageBox.Show("The current version and the previous version aren't the same, however no update was found", LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                            //logger.Fatal("The current version and the previous version aren't the same, however no update was found");
                            //ApplicationManager.HandleShutdown(MainWindow._notifyIcon);
                        }
                        else
                        {
                            logger.Fatal("Outdated database schema");
                            MessageBox.Show("Cannot use the overlay with an outdated database schema", LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                            ApplicationManager.HandleShutdown();
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

        // TODO: this is repeating code. also not sure if the data types handling is correct
        // UPDATE: so it turns out, data types are suggestions, not rules.
        private void PerformUpdateToVersion_0_23_0(SQLiteConnection connection)
        {
            // Perform database updates for version 0.23.0
            string sql = @"CREATE TABLE IF NOT EXISTS QuestAttempts(
            QuestAttemptsID INTEGER PRIMARY KEY AUTOINCREMENT,
            QuestID INTEGER NOT NULL,
            WeaponTypeID INTEGER NOT NULL,
            ActualOverlayMode TEXT NOT NULL,
            Attempts INTEGER NOT NULL,
            FOREIGN KEY(QuestID) REFERENCES QuestName(QuestNameID),
            FOREIGN KEY(WeaponTypeID) REFERENCES WeaponType(WeaponTypeID),
            UNIQUE (QuestID, WeaponTypeID, ActualOverlayMode)
            )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS PersonalBestAttempts(
                    PersonalBestAttemptsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    QuestID INTEGER NOT NULL,
                    WeaponTypeID INTEGER NOT NULL,
                    ActualOverlayMode TEXT NOT NULL,
                    Attempts INTEGER NOT NULL,
                    FOREIGN KEY(QuestID) REFERENCES QuestName(QuestNameID),
                    FOREIGN KEY(WeaponTypeID) REFERENCES WeaponType(WeaponTypeID),
                    UNIQUE (QuestID, WeaponTypeID, ActualOverlayMode)
                    )
                    ";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS ZenithGauntlets(
                        ZenithGauntletID INTEGER PRIMARY KEY AUTOINCREMENT,
                        WeaponType TEXT NOT NULL,
                        Category TEXT NOT NULL,
                        TotalFramesElapsed INTEGER NOT NULL,
                        TotalTimeElapsed TEXT NOT NULL,
                        Run1ID INTEGER NOT NULL,
                        Run2ID INTEGER NOT NULL,
                        Run3ID INTEGER NOT NULL,
                        Run4ID INTEGER NOT NULL,
                        Run5ID INTEGER NOT NULL,
                        Run6ID INTEGER NOT NULL,
                        Run7ID INTEGER NOT NULL,
                        Run8ID INTEGER NOT NULL,
                        Run9ID INTEGER NOT NULL,
                        Run10ID INTEGER NOT NULL,
                        Run11ID INTEGER NOT NULL,
                        Run12ID INTEGER NOT NULL,
                        Run13ID INTEGER NOT NULL,
                        Run14ID INTEGER NOT NULL,
                        Run15ID INTEGER NOT NULL,
                        Run16ID INTEGER NOT NULL,
                        Run17ID INTEGER NOT NULL,
                        Run18ID INTEGER NOT NULL,
                        Run19ID INTEGER NOT NULL,
                        Run20ID INTEGER NOT NULL,
                        Run21ID INTEGER NOT NULL,
                        Run22ID INTEGER NOT NULL,
                        Run23ID INTEGER NOT NULL,
                        FOREIGN KEY(Run1ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run2ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run3ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run4ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run5ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run6ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run7ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run8ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run9ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run10ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run11ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run12ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run13ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run14ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run15ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run16ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run17ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run18ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run19ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run20ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run21ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run22ID) REFERENCES Quests(RunID),
                        FOREIGN KEY(Run23ID) REFERENCES Quests(RunID)
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS PersonalBests(
                    PersonalBestsID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunID INTEGER NOT NULL,
                    Attempts INTEGER NOT NULL,
                    FOREIGN KEY(RunID) REFERENCES Quests(RunID))";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS Overlay(
                    OverlayID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Hash TEXT NOT NULL)";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_overlay_updates
                        AFTER UPDATE ON Overlay
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_overlay_deletion
                        AFTER DELETE ON Overlay
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS Bingo(
                    BingoID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    Difficulty TEXT NOT NULL,
                    MonsterList TEXT NOT NULL,
                    ElapsedTime TEXT NOT NULL,
                    Score INTEGER NOT NULL
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS MezFesMinigames(
                    MezFesMinigameID INTEGER PRIMARY KEY AUTOINCREMENT,
                    MezFesMinigameName INTEGER NOT NULL
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS MezFes(
                    MezFesID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    MinigameID INTEGER NOT NULL,
                    Score TEXT NOT NULL,
                    FOREIGN KEY(MinigameID) REFERENCES MezFesMinigames(MezFesMinigameID)
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_bingo_updates
                        AFTER UPDATE ON Bingo
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_bingo_deletion
                        AFTER DELETE ON Bingo
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_mezfesminigames_updates
                        AFTER UPDATE ON MezFesMinigames
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_mezfesminigames_deletion
                        AFTER DELETE ON MezFesMinigames
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_mezfes_updates
                        AFTER UPDATE ON MezFes
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                cmd.ExecuteNonQuery();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = @"CREATE TRIGGER IF NOT EXISTS prevent_mezfes_deletion
                        AFTER DELETE ON MezFes
                        BEGIN
                          SELECT RAISE(ROLLBACK, 'Updating rows is not allowed. Keep in mind that all attempted modifications are logged into the central database.');
                        END;";
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS GachaCard(
                    GachaCardID INTEGER PRIMARY KEY AUTOINCREMENT,
                    GachaCardTypeID INTEGER NOT NULL,
                    GachaCardRarityID INTEGER NOT NULL,
                    GachaCardName INTEGER NOT NULL,
                    GachaCardFrontImage TEXT NOT NULL,
                    GachCardBackImage TEXT NOT NULL,
                    UNIQUE(GachaCardTypeID, GachaCardRarityID, GachaCardName),
                    FOREIGN KEY(GachaCardTypeID) REFERENCES GachaCardType(GachaCardTypeID),
                    FOREIGN KEY(GachaCardRarityID) REFERENCES GachaCardRarity(GachaCardRarityID)
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS GachaCardType(
                    GachaCardTypeID INTEGER PRIMARY KEY,
                    GachaCardTypeName TEXT NOT NULL
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS GachaCardRarity(
                    GachaCardRarityID INTEGER PRIMARY KEY,
                    GachaCardRarityName TEXT NOT NULL
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            sql = @"CREATE TABLE IF NOT EXISTS GachaCardInventory(
                    GachaCardInventoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                    GachaCardID INTEGER NOT NULL,
                    FOREIGN KEY(GachaCardID) REFERENCES GachaCard(GachaCardID)
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }

            //            sql = @"ALTER TABLE Quests
            //                    MODIFY COLUMN Monster1HPDictionary TEXT NOT NULL AFTER PartySize,
            //                    MODIFY COLUMN Monster2HPDictionary TEXT NOT NULL AFTER Monster1HPDictionary,
            //                    MODIFY COLUMN Monster3HPDictionary TEXT NOT NULL AFTER Monster2HPDictionary,
            //                    MODIFY COLUMN Monster4HPDictionary TEXT NOT NULL AFTER Monster3HPDictionary,
            //                    MODIFY COLUMN Monster1AttackMultiplierDictionary TEXT NOT NULL AFTER Monster4HPDictionary,
            //                    MODIFY COLUMN Monster1DefenseRateDictionary TEXT NOT NULL AFTER Monster1AttackMultiplierDictionary,
            //                    MODIFY COLUMN Monster1SizeMultiplierDictionary TEXT NOT NULL AFTER Monster1DefenseRateDictionary,
            //                    MODIFY COLUMN Monster1PoisonThresholdDictionary TEXT NOT NULL AFTER Monster1SizeMultiplierDictionary,
            //                    MODIFY COLUMN Monster1SleepThresholdDictionary TEXT NOT NULL AFTER Monster1PoisonThresholdDictionary,
            //                    MODIFY COLUMN Monster1ParalysisThresholdDictionary TEXT NOT NULL AFTER Monster1SleepThresholdDictionary,
            //                    MODIFY COLUMN Monster1BlastThresholdDictionary TEXT NOT NULL AFTER Monster1ParalysisThresholdDictionary,
            //                    MODIFY COLUMN Monster1StunThresholdDictionary TEXT NOT NULL AFTER Monster1BlastThresholdDictionary;
            //                    MODIFY COLUMN IsHighGradeEdition INTEGER NOT NULL CHECK (IsHighGradeEdition IN (0, 1)) AFTER Monster1StunThresholdDictionary;
            //                    MODIFY COLUMN RefreshRate INTEGER NOT NULL CHECK (RefreshRate IN (1, 30)) AFTER IsHighGradeEdition;                    
            //";
            //            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            //            {
            //                cmd.ExecuteNonQuery();
            //            }

            sql = @"CREATE TABLE IF NOT EXISTS new_Quests
                    (
                    QuestHash TEXT NOT NULL DEFAULT '',
                    CreatedAt TEXT NOT NULL DEFAULT '',
                    CreatedBy TEXT NOT NULL DEFAULT '',
                    RunID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    QuestID INTEGER NOT NULL CHECK (QuestID >= 0) DEFAULT 0, 
                    TimeLeft INTEGER NOT NULL DEFAULT 0,
                    FinalTimeValue INTEGER NOT NULL DEFAULT 0,
                    FinalTimeDisplay TEXT NOT NULL DEFAULT '', 
                    ObjectiveImage TEXT NOT NULL DEFAULT '',
                    ObjectiveTypeID INTEGER NOT NULL CHECK (ObjectiveTypeID >= 0) DEFAULT 0, 
                    ObjectiveQuantity INTEGER NOT NULL DEFAULT 0, 
                    StarGrade INTEGER NOT NULL DEFAULT 0, 
                    RankName TEXT NOT NULL DEFAULT '', 
                    ObjectiveName TEXT NOT NULL DEFAULT '', 
                    Date TEXT NOT NULL DEFAULT '',
                    YouTubeID TEXT DEFAULT 'dQw4w9WgXcQ', -- default value for YouTubeID is a Rick Roll video
                    -- DpsData TEXT NOT NULL DEFAULT '',
                    AttackBuffDictionary TEXT NOT NULL DEFAULT '{}',
                    HitCountDictionary TEXT NOT NULL DEFAULT '{}',
                    HitsPerSecondDictionary TEXT NOT NULL DEFAULT '{}',
                    DamageDealtDictionary TEXT NOT NULL DEFAULT '{}',
                    DamagePerSecondDictionary TEXT NOT NULL DEFAULT '{}',
                    AreaChangesDictionary TEXT NOT NULL DEFAULT '{}',
                    CartsDictionary TEXT NOT NULL DEFAULT '{}',
                    HitsTakenBlockedDictionary TEXT NOT NULL DEFAULT '{}',
                    HitsTakenBlockedPerSecondDictionary TEXT NOT NULL DEFAULT '{}',
                    PlayerHPDictionary TEXT NOT NULL DEFAULT '{}',
                    PlayerStaminaDictionary TEXT NOT NULL DEFAULT '{}',
                    KeystrokesDictionary TEXT NOT NULL DEFAULT '{}',
                    MouseInputDictionary TEXT NOT NULL DEFAULT '{}',
                    GamepadInputDictionary TEXT NOT NULL DEFAULT '{}',
                    ActionsPerMinuteDictionary TEXT NOT NULL DEFAULT '{}',
                    OverlayModeDictionary TEXT NOT NULL DEFAULT '{}',
                    ActualOverlayMode TEXT NOT NULL DEFAULT 'Standard',
                    PartySize INTEGER NOT NULL DEFAULT 0,
                    Monster1HPDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster2HPDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster3HPDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster4HPDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1AttackMultiplierDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1DefenseRateDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1SizeMultiplierDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1PoisonThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1SleepThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1ParalysisThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1BlastThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1StunThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    IsHighGradeEdition INTEGER NOT NULL CHECK (IsHighGradeEdition IN (0, 1)) DEFAULT 0,
                    RefreshRate INTEGER NOT NULL CHECK (RefreshRate IN (1,30)) DEFAULT 30,
                    FOREIGN KEY(QuestID) REFERENCES QuestName(QuestNameID),
                    FOREIGN KEY(ObjectiveTypeID) REFERENCES ObjectiveType(ObjectiveTypeID)
                    -- FOREIGN KEY(RankNameID) REFERENCES RankName(RankNameID)
                    )";

            // Transfer content from X into new_X using a statement like: INSERT INTO new_X SELECT ... FROM X.
            AlterTableQuests(connection, sql);

            // https://www.sqlite.org/lang_altertable.html#otheralter must read
            // Repeat the same pattern for other version updates
            // By using ALTER TABLE, you can make changes to the structure of a table
            // without having to recreate the table and manually transfer all the data.
        }

        // Define a function that takes a connection string, the name of the table to alter (X), and the new schema for the table (as a SQL string)
        private void AlterTableQuests(SQLiteConnection connection, string newSchema)
        {
            logger.Info("Altering Quests table");

            var tableName = "Quests";

            try
            {
                // 3. Remember the format of all indexes, triggers, and views associated with table X. This information will be needed in step 8 below. One way to do this is to run a query like the following: SELECT type, sql FROM sqlite_schema WHERE tbl_name='X'.
                // Remember the format of all indexes, triggers, and views associated with table X
                SQLiteCommand rememberFormat = new SQLiteCommand("SELECT type, sql FROM sqlite_schema WHERE tbl_name=@tableName;", connection);
                rememberFormat.Parameters.AddWithValue("@tableName", tableName);
                SQLiteDataReader reader = rememberFormat.ExecuteReader();
                List<string> indexSqls = new List<string>();
                List<string> triggerSqls = new List<string>();
                List<string> viewSqls = new List<string>();
                while (reader.Read())
                {
                    string type = reader.GetString(0);
                    string sql = reader.GetString(1);
                    if (type == "index")
                    {
                        indexSqls.Add(sql);
                    }
                    else if (type == "trigger")
                    {
                        triggerSqls.Add(sql);
                    }
                    else if (type == "view")
                    {
                        viewSqls.Add(sql);
                    }
                }
                reader.Close();

                // 4. Use CREATE TABLE to construct a new table "new_X" that is in the desired revised format of table X. Make sure that the name "new_X" does not collide with any existing table name, of course.
                // Use CREATE TABLE to construct a new table "new_X" that is in the desired revised format of table X
                using (SQLiteCommand createTable = new SQLiteCommand(newSchema, connection))
                {
                    createTable.ExecuteNonQuery();
                }

                logger.Debug("Created table if not exists new_{0}", tableName);

                // 5. Transfer content from X into new_X using a statement like: INSERT INTO new_X SELECT ... FROM X.
                // Transfer content from X into new_X using a statement like: INSERT INTO new_X SELECT ... FROM X

                // Get the number of rows in the Quests table
                string countQuery = "SELECT COUNT(*) FROM Quests";
                using (var command = new SQLiteCommand(countQuery, connection))
                {
                    int rowCount = Convert.ToInt32(command.ExecuteScalar());

                    logger.Debug("Inserting default values into new_Quests");

                    // Insert rows with default values into new_Quests
                    string insertQuery = $"INSERT INTO new_Quests DEFAULT VALUES";
                    for (int i = 0; i < rowCount; i++)
                    {
                        using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                        {
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    logger.Debug("Inserted default values into new_Quests");

                    // Update values from Quests to new_Quests
                    string updateQuery = @"
        UPDATE new_Quests
        SET QuestHash = (SELECT QuestHash FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            CreatedAt = (SELECT CreatedAt FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            CreatedBy = (SELECT CreatedBy FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            QuestID = (SELECT QuestID FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            TimeLeft = (SELECT TimeLeft FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            FinalTimeValue = (SELECT FinalTimeValue FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            FinalTimeDisplay = (SELECT FinalTimeDisplay FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            ObjectiveImage = (SELECT ObjectiveImage FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            ObjectiveTypeID = (SELECT ObjectiveTypeID FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            ObjectiveQuantity = (SELECT ObjectiveQuantity FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            StarGrade = (SELECT StarGrade FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            RankName = (SELECT RankName FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            ObjectiveName = (SELECT ObjectiveName FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            Date = (SELECT Date FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            YouTubeID = (SELECT YouTubeID FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            AttackBuffDictionary = (SELECT AttackBuffDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            HitCountDictionary = (SELECT HitCountDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            HitsPerSecondDictionary = (SELECT HitsPerSecondDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            DamageDealtDictionary = (SELECT DamageDealtDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            DamagePerSecondDictionary = (SELECT DamagePerSecondDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            AreaChangesDictionary = (SELECT AreaChangesDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            CartsDictionary = (SELECT CartsDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            HitsTakenBlockedDictionary = (SELECT HitsTakenBlockedDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            HitsTakenBlockedPerSecondDictionary = (SELECT HitsTakenBlockedPerSecondDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            PlayerHPDictionary = (SELECT PlayerHPDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            PlayerStaminaDictionary = (SELECT PlayerStaminaDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            KeystrokesDictionary = (SELECT KeystrokesDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            MouseInputDictionary = (SELECT MouseInputDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            GamepadInputDictionary = (SELECT GamepadInputDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            ActionsPerMinuteDictionary = (SELECT ActionsPerMinuteDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            OverlayModeDictionary = (SELECT OverlayModeDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            ActualOverlayMode = (SELECT ActualOverlayMode FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            PartySize = (SELECT PartySize FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            Monster1HPDictionary = (SELECT Monster1HPDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            Monster2HPDictionary = (SELECT Monster2HPDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            Monster3HPDictionary = (SELECT Monster3HPDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID),
            Monster4HPDictionary = (SELECT Monster4HPDictionary FROM Quests WHERE Quests.RunID = new_Quests.RunID)
            WHERE EXISTS (SELECT 1 FROM Quests WHERE Quests.RunID = new_Quests.RunID)"
                    ;
                    /*
                    Monster1AttackMultiplierDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1DefenseRateDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1SizeMultiplierDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1PoisonThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1SleepThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1ParalysisThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1BlastThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    Monster1StunThresholdDictionary TEXT NOT NULL DEFAULT '{}',
                    IsHighGradeEdition INTEGER NOT NULL CHECK (IsHighGradeEdition IN (0, 1)) DEFAULT 0,
                    RefreshRate INTEGER NOT NULL CHECK (RefreshRate IN (1,30)) DEFAULT 30,
                    */

                    using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                    {
                        updateCommand.ExecuteNonQuery();
                    }
                }

                logger.Debug("Transferred data from {0} to new_{1}", tableName, tableName);

                // 6. Drop the old table X: DROP TABLE X.
                // Drop the old table X
                using (SQLiteCommand dropTable = new SQLiteCommand("DROP TABLE " + tableName + ";", connection))
                {
                    dropTable.ExecuteNonQuery();
                }

                logger.Debug("Deleted table {0}", tableName);

                // 7. Change the name of new_X to X using: ALTER TABLE new_X RENAME TO X.
                // Change the name of new_X to X using: ALTER TABLE new_X RENAME TO X
                using (SQLiteCommand renameTable = new SQLiteCommand("ALTER TABLE new_" + tableName + " RENAME TO " + tableName + ";", connection))
                {
                    renameTable.ExecuteNonQuery();
                }

                logger.Debug("Renamed new_{0} to {1}", tableName, tableName);

                // 8. Use CREATE INDEX, CREATE TRIGGER, and CREATE VIEW to reconstruct indexes, triggers, and views associated with table X. Perhaps use the old format of the triggers, indexes, and views saved from step 3 above as a guide, making changes as appropriate for the alteration.
                // Use CREATE INDEX, CREATE TRIGGER, and CREATE VIEW to reconstruct indexes, triggers, and views associated with table X
                foreach (string indexSql in indexSqls)
                {
                    using (SQLiteCommand createIndex = new SQLiteCommand(indexSql, connection))
                    {
                        createIndex.ExecuteNonQuery();
                    }
                }
                foreach (string triggerSql in triggerSqls)
                {
                    using (SQLiteCommand createTrigger = new SQLiteCommand(triggerSql, connection))
                    {
                        createTrigger.ExecuteNonQuery();
                    }
                }
                foreach (string viewSql in viewSqls)
                {
                    using (SQLiteCommand createView = new SQLiteCommand(viewSql, connection))
                    {
                        createView.ExecuteNonQuery();
                    }
                }

                logger.Debug("Indexes: {0}, Triggers: {1}, Views: {2}", indexSqls.Count, triggerSqls.Count, viewSqls.Count);

                // TODO: since im not using any views this still needs testing in case i make views someday.
                // 9. If any views refer to table X in a way that is affected by the schema change, then drop those views using DROP VIEW and recreate them with whatever changes are necessary to accommodate the schema change using CREATE VIEW.
                //using (SQLite
                // Check if any views refer to table X in a way that is affected by the schema change
                SQLiteCommand findViews = new SQLiteCommand("SELECT name, sql FROM sqlite_master WHERE type='view' AND sql LIKE '% " + tableName + " %';", connection);
                SQLiteDataReader viewReader = findViews.ExecuteReader();
                List<string> viewNames = new List<string>();
                List<string> viewSqlsModified = new List<string>();
                while (viewReader.Read())
                {
                    viewNames.Add(viewReader.GetString(0));
                    string viewSql = viewReader.GetString(1);
                    viewSql = viewSql.Replace(tableName, "new_" + tableName);
                    viewSqlsModified.Add(viewSql);
                }
                viewReader.Close();

                // Drop those views using DROP VIEW
                foreach (string viewName in viewNames)
                {
                    using (SQLiteCommand dropView = new SQLiteCommand("DROP VIEW " + viewName + ";", connection))
                    {
                        dropView.ExecuteNonQuery();
                    }
                }

                // TODO: test
                // Recreate views with whatever changes are necessary to accommodate the schema change using CREATE VIEW
                for (int i = 0; i < viewNames.Count; i++)
                {
                    using (SQLiteCommand createView = new SQLiteCommand(viewSqlsModified[i], connection))
                    {
                        createView.ExecuteNonQuery();
                    }
                }

                logger.Debug("Views affected: {0}", viewSqlsModified.Count);

                string foreignKeysViolations = CheckForeignKeys(connection);

                // 10. If foreign key constraints were originally enabled then run PRAGMA foreign_key_check to verify that the schema change did not break any foreign key constraints.
                if (foreignKeysViolations != "")
                {
                    logger.Fatal("Foreign keys violations detected, closing program. Violations: {0}", foreignKeysViolations);
                    MessageBox.Show("Foreign keys violations detected, closing program.", LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                    ApplicationManager.HandleShutdown();
                }
                else
                {
                    logger.Debug("No foreign keys violations found");
                }

                logger.Info("Altered Quests table successfully");
            }
            catch (Exception ex)
            {
                // Roll back the transaction if any errors occur
                logger.Error(ex, "Could not alter table {0}", tableName);
            }
        }

        /// <summary>
        /// Both tables must have the same column count. Define a function that takes a connection string, the name of the table to alter (X), and the new schema for the table (as a SQL string)
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="newSchema"></param>
        private void AlterTableSchema(SQLiteConnection connection, string tableName, string newSchema)
        {
            logger.Info("Altering table {0}", tableName);

            try
            {
                // 3. Remember the format of all indexes, triggers, and views associated with table X. This information will be needed in step 8 below. One way to do this is to run a query like the following: SELECT type, sql FROM sqlite_schema WHERE tbl_name='X'.
                // Remember the format of all indexes, triggers, and views associated with table X
                SQLiteCommand rememberFormat = new SQLiteCommand("SELECT type, sql FROM sqlite_schema WHERE tbl_name=@tableName;", connection);
                rememberFormat.Parameters.AddWithValue("@tableName", tableName);
                SQLiteDataReader reader = rememberFormat.ExecuteReader();
                List<string> indexSqls = new List<string>();
                List<string> triggerSqls = new List<string>();
                List<string> viewSqls = new List<string>();
                while (reader.Read())
                {
                    string type = reader.GetString(0);
                    string sql = reader.GetString(1);
                    if (type == "index")
                    {
                        indexSqls.Add(sql);
                    }
                    else if (type == "trigger")
                    {
                        triggerSqls.Add(sql);
                    }
                    else if (type == "view")
                    {
                        viewSqls.Add(sql);
                    }
                }
                reader.Close();

                // 4. Use CREATE TABLE to construct a new table "new_X" that is in the desired revised format of table X. Make sure that the name "new_X" does not collide with any existing table name, of course.
                // Use CREATE TABLE to construct a new table "new_X" that is in the desired revised format of table X
                using (SQLiteCommand createTable = new SQLiteCommand(newSchema, connection))
                {
                    createTable.ExecuteNonQuery();
                }

                logger.Debug("Created table new_{0}", tableName);

                // 5. Transfer content from X into new_X using a statement like: INSERT INTO new_X SELECT ... FROM X.
                // Transfer content from X into new_X using a statement like: INSERT INTO new_X SELECT ... FROM X
                string insertSql = "INSERT INTO new_" + tableName + " SELECT * FROM " + tableName + ";";
                using (SQLiteCommand insertData = new SQLiteCommand(insertSql, connection))
                {
                    insertData.ExecuteNonQuery();
                }

                logger.Debug("Transferred data from {0} to new_{1}", tableName, tableName);

                // 6. Drop the old table X: DROP TABLE X.
                // Drop the old table X
                using (SQLiteCommand dropTable = new SQLiteCommand("DROP TABLE " + tableName + ";", connection))
                {
                    dropTable.ExecuteNonQuery();
                }

                logger.Debug("Deleted table {0}", tableName);

                // 7. Change the name of new_X to X using: ALTER TABLE new_X RENAME TO X.
                // Change the name of new_X to X using: ALTER TABLE new_X RENAME TO X
                using (SQLiteCommand renameTable = new SQLiteCommand("ALTER TABLE new_" + tableName + " RENAME TO " + tableName + ";", connection))
                {
                    renameTable.ExecuteNonQuery();
                }

                logger.Debug("Renamed new_{0} to {1}", tableName, tableName);

                // 8. Use CREATE INDEX, CREATE TRIGGER, and CREATE VIEW to reconstruct indexes, triggers, and views associated with table X. Perhaps use the old format of the triggers, indexes, and views saved from step 3 above as a guide, making changes as appropriate for the alteration.
                // Use CREATE INDEX, CREATE TRIGGER, and CREATE VIEW to reconstruct indexes, triggers, and views associated with table X
                foreach (string indexSql in indexSqls)
                {
                    using (SQLiteCommand createIndex = new SQLiteCommand(indexSql, connection))
                    {
                        createIndex.ExecuteNonQuery();
                    }
                }
                foreach (string triggerSql in triggerSqls)
                {
                    using (SQLiteCommand createTrigger = new SQLiteCommand(triggerSql, connection))
                    {
                        createTrigger.ExecuteNonQuery();
                    }
                }
                foreach (string viewSql in viewSqls)
                {
                    using (SQLiteCommand createView = new SQLiteCommand(viewSql, connection))
                    {
                        createView.ExecuteNonQuery();
                    }
                }

                logger.Debug("Indexes: {0}, Triggers: {1}, Views: {2}", indexSqls.Count, triggerSqls.Count, viewSqls.Count);

                // TODO: since im not using any views this still needs testing in case i make views someday.
                // 9. If any views refer to table X in a way that is affected by the schema change, then drop those views using DROP VIEW and recreate them with whatever changes are necessary to accommodate the schema change using CREATE VIEW.
                //using (SQLite
                // Check if any views refer to table X in a way that is affected by the schema change
                SQLiteCommand findViews = new SQLiteCommand("SELECT name, sql FROM sqlite_master WHERE type='view' AND sql LIKE '% " + tableName + " %';", connection);
                SQLiteDataReader viewReader = findViews.ExecuteReader();
                List<string> viewNames = new List<string>();
                List<string> viewSqlsModified = new List<string>();
                while (viewReader.Read())
                {
                    viewNames.Add(viewReader.GetString(0));
                    string viewSql = viewReader.GetString(1);
                    viewSql = viewSql.Replace(tableName, "new_" + tableName);
                    viewSqlsModified.Add(viewSql);
                }
                viewReader.Close();

                // Drop those views using DROP VIEW
                foreach (string viewName in viewNames)
                {
                    using (SQLiteCommand dropView = new SQLiteCommand("DROP VIEW " + viewName + ";", connection))
                    {
                        dropView.ExecuteNonQuery();
                    }
                }

                // TODO: test
                // Recreate views with whatever changes are necessary to accommodate the schema change using CREATE VIEW
                for (int i = 0; i < viewNames.Count; i++)
                {
                    using (SQLiteCommand createView = new SQLiteCommand(viewSqlsModified[i], connection))
                    {
                        createView.ExecuteNonQuery();
                    }
                }

                logger.Debug("Views affected: {0}", viewSqlsModified.Count);

                string foreignKeysViolations = CheckForeignKeys(connection);

                // 10. If foreign key constraints were originally enabled then run PRAGMA foreign_key_check to verify that the schema change did not break any foreign key constraints.
                if (foreignKeysViolations != "") 
                {
                    logger.Fatal("Foreign keys violations detected, closing program. Violations: {0}", foreignKeysViolations);
                    MessageBox.Show("Foreign keys violations detected, closing program.", "MHF-Z Overlay Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ApplicationManager.HandleShutdown();
                }
                else
                {
                    logger.Debug("No foreign keys violations found");
                }

                logger.Info("Altered table {0} successfully", tableName);
            }
            catch (Exception ex)
            {
                // Roll back the transaction if any errors occur
                logger.Error(ex, "Could not alter table {0}", tableName);
            }
        }

        // Define a function that takes a connection string and the name of the table to check for foreign key violations
        public string CheckForeignKeys(SQLiteConnection connection, string tableName = "")
        {
            logger.Debug("Checking foreign keys");

            string query = "PRAGMA foreign_key_check";
            if (!string.IsNullOrEmpty(tableName))
            {
                query += "('" + tableName + "')";
            }

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // There are foreign key violations
                        var violations = new List<Dictionary<string, object>>();

                        while (reader.Read())
                        {
                            var violation = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                violation[reader.GetName(i)] = reader.GetValue(i);
                            }
                            violations.Add(violation);
                        }
                        logger.Error(violations);
                        return JsonConvert.SerializeObject(violations);
                    }
                    else
                    {
                        // No foreign key violations
                        logger.Debug("No foreign key violations found.");
                        return "";
                    }
                }
            }
        }

        private void EnableForeignKeyConstraints(SQLiteConnection connection)
        {
            try
            {
                string sql = @"PRAGMA foreign_keys = ON";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                logger.Info("Enabled foreign key constraints");
            } 
            catch (Exception ex)
            {
                logger.Fatal("Could not toggle foreign key constraints", ex);
                MessageBox.Show("Could not toggle foreign key constraints", LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationManager.HandleShutdown();
            }
        }

        private void DisableForeignKeyConstraints(SQLiteConnection connection)
        {
            try
            {
                string sql = @"PRAGMA foreign_keys = OFF";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                logger.Info("Disabled foreign key constraints");
            }
            catch (Exception ex)
            {
                logger.Fatal("Could not toggle foreign key constraints", ex);
                MessageBox.Show("Could not toggle foreign key constraints", LoggingManager.ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                ApplicationManager.HandleShutdown();
            }
        }

        private static void PerformUpdateToVersion_0_24_0(SQLiteConnection connection)
        {
            // Perform database updates for version 0.24.0
            // Add your update logic here
        }

        private void WritePreviousVersionToFile()
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            var previousVersionFilePath = s.PreviousVersionFilePath.Trim();
            var logMessage = "Error with version file";

            // TODO why does this error? also find a way to put this in FileManager
            try
            {
                if (File.ReadAllText(previousVersionFilePath) == "")
                    previousVersion = App.CurrentProgramVersion.Trim();
                else
                    previousVersion = File.ReadAllText(previousVersionFilePath);

                File.WriteAllText(previousVersionFilePath, previousVersion);
                logger.Info("Writing previous version {0} to file {1}", previousVersion, previousVersionFilePath);
            }
            catch (Exception ex)
            {
                logger.Error(ex, logMessage);
            }
        }

        private string ReadPreviousVersionFromFile()
        {
            Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            string version = "";
            if (File.Exists(s.PreviousVersionFilePath))
            {
                using (StreamReader reader = new StreamReader(s.PreviousVersionFilePath))
                {
                    version = reader.ReadLine();
                }
            }
            return version;
        }

        #endregion

        /// <summary>
        /// Throws an exception.
        /// </summary>
        /// <param name="conn">The connection.</param>
        private void ThrowException(SQLiteConnection conn)
        {
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    // Execute an invalid SQL statement to trigger an exception
                    using (var command = new SQLiteCommand("SELECT * FROM non_existent_table", conn))
                    {
                        command.ExecuteNonQuery();
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

        #endregion
    }
}
/// <TODO>
/// * add checker for triggers and indexes changes
/// * USE BLOB for attack buff list and hit count list.
/// * data structure: list < int(timeint), int(hit count / attack buff) >
/// You can use LINQ to perform various operations on lists, such as filtering, sorting, and aggregating data. Here's an example of how you can use LINQ to calculate the average attack buff of a particular quest run with a particular weapon type:
///
/// To calculate the maximum attack buff of a particular quest run with a particular weapon type, you can use the Max() method:
///
/// To calculate the maximum attack buff of all quest runs of a particular quest with a particular weapon type, you can use LINQ to group the attack buff values by quest and weapon type, and then use the Max() method to find the maximum attack buff for each group:
///
/// This will return a list of groups, each containing the quest ID, weapon type, and maximum attack buff for a particular quest and weapon type. You can then iterate over this list to find the maximum attack buff for all quest runs. 
///
/// Include info from more spreadsheets (speedrun calculation etc)
///
/// LINQ(Language Integrated Query) is a set of features in C# that allows you to write queries to filter, transform, and aggregate data in your code. It works with various data sources, including arrays, lists, and dictionaries.
///
/// For example, if you want to calculate the average attack buff for a particular quest run with a particular weapon type, you could use LINQ's Average method like this:
///
/// To calculate the maximum attack buff for a particular quest run with a particular weapon type, you could use LINQ's Max method like this:
///
/// To calculate the maximum attack buff of all quest runs of a particular quest with a particular weapon type, you would need to store the attack buff dictionaries for each quest run in a list or collection and then use LINQ's Max method like this:
///
/// This would select the maximum attack buff value for each dictionary in the list, and then find the overall maximum value from those.
///
/// You can read more about LINQ and its various methods and features in the C# documentation: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
/// 
/// </TODO>
