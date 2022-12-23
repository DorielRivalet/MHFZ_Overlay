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

namespace MHFZ_Overlay
{
    // Singleton
    internal class DatabaseManager
    {
        private readonly string _connectionString;

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

            using (var connection = new SQLiteConnection(_connectionString))
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

        public void SetupLocalDatabase(DataLoader dataLoader)
        {

            if (!File.Exists(_connectionString))
            {
                SQLiteConnection.CreateFile(_connectionString);
            }

            using (var conn = new SQLiteConnection("Data Source=" + _connectionString + ""))
            {
                conn.Open();

                // Do something with the connection
                CreateDatabaseTables(conn, dataLoader);
                CreateDatabaseIndexes(conn);
                CreateDatabaseTriggers(conn);
            }
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

        private void InsertQuestData(string connectionString, DataLoader dataLoader)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var model = dataLoader.model;
                Settings s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
                string sql;

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {

                        // Insert data into the Quests table
                        sql = @"INSERT INTO Quests (
                        QuestID, AreaID, FinalTimeValue, FinalTimeDisplay, ObjectiveImage, ObjectiveTypeID, ObjectiveQuantity, StarGrade, RankNameID, ObjectiveName, Date
                        ) VALUES (@QuestID, @AreaID, @FinalTimeValue, @FinalTimeDisplay, @ObjectiveImage, @ObjectiveTypeID, @ObjectiveQuantity, @StarGrade, @RankNameID, @ObjectiveName, @Date)";

                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {


                            int questID = model.QuestID();
                            int areaID = model.AreaID();
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
                            int objectiveQuantity = int.Parse(model.GetObjective1Quantity());
                            int starGrade = model.StarGrades();

                            if ((model.ObjectiveType() == 0x0 || model.ObjectiveType() == 0x02 || model.ObjectiveType() == 0x1002 || model.ObjectiveType() == 0x10) && (model.QuestID() != 23527 && model.QuestID() != 23628 && model.QuestID() != 21731 && model.QuestID() != 21749 && model.QuestID() != 21746 && model.QuestID() != 21750))
                                objectiveName = model.GetObjective1Name(model.Objective1ID(), true);
                            else
                                objectiveName = model.GetRealMonsterName(model.CurrentMonster1Icon, true);

                            DateTime date = DateTime.Now;

                            //                    --Insert data into the ZenithSkills table
                            //INSERT INTO ZenithSkills(ZenithSkill1, ZenithSkill2, ZenithSkill3, ZenithSkill4, ZenithSkill5, ZenithSkill6)
                            //VALUES(zenithSkillsID, zenithSkillsID, zenithSkillsID, zenithSkillsID, zenithSkillsID, zenithSkillsID);

                            //                    --Get the ZenithSkillsID that was generated
                            //                    SELECT LAST_INSERT_ROWID() as ZenithSkillsID;

                            cmd.Parameters.AddWithValue("@QuestID", questID);
                            cmd.Parameters.AddWithValue("@AreaID", areaID);
                            cmd.Parameters.AddWithValue("@FinalTimeValue", finalTimeValue);
                            cmd.Parameters.AddWithValue("@FinalTimeDisplay", finalTimeDisplay);
                            cmd.Parameters.AddWithValue("@ObjectiveImage", objectiveImage);
                            cmd.Parameters.AddWithValue("@ObjectiveTypeID", objectiveTypeID);
                            cmd.Parameters.AddWithValue("@ObjectiveQuantity", objectiveQuantity);
                            cmd.Parameters.AddWithValue("@StarGrade", starGrade);
                            cmd.Parameters.AddWithValue("@RankNameID", rankName);
                            cmd.Parameters.AddWithValue("@ObjectiveName", objectiveName);
                            cmd.Parameters.AddWithValue("@Date", date);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int runID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            runID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Insert data into the Players table
                        sql = "INSERT INTO Players (PlayerName, GuildName, Gender) VALUES (@PlayerName, @GuildName, @Gender)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            string playerName = s.HunterName;
                            string guildName = s.GuildName;
                            string gender = s.GenderExport;

                            cmd.Parameters.AddWithValue("@PlayerName", playerName);
                            cmd.Parameters.AddWithValue("@GuildName", guildName);
                            cmd.Parameters.AddWithValue("@Gender", gender);
                            cmd.ExecuteNonQuery();
                        }

                        // Get the ID of the last inserted row in the Players table
                        sql = "SELECT LAST_INSERT_ROWID()";
                        int playerID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            playerID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        //    FOREIGN KEY(RunID) REFERENCES Quests(RunID),
                        //FOREIGN KEY(PlayerID) REFERENCES Players(PlayerID),
                        //FOREIGN KEY(WeaponTypeID) REFERENCES WeaponType(WeaponTypeID),
                        //FOREIGN KEY(WeaponID) REFERENCES Gear(PieceID),
                        //FOREIGN KEY(HeadID) REFERENCES Gear(PieceID),
                        //FOREIGN KEY(ChestID) REFERENCES Gear(PieceID),
                        //FOREIGN KEY(ArmsID) REFERENCES Gear(PieceID),
                        //FOREIGN KEY(WaistID) REFERENCES Gear(PieceID),
                        //FOREIGN KEY(LegsID) REFERENCES Gear(PieceID),
                        //FOREIGN KEY(Cuff1ID) REFERENCES Item(ItemID),
                        //FOREIGN KEY(Cuff2ID) REFERENCES Item(ItemID),
                        //FOREIGN KEY(ZenithSkillsID) REFERENCES ZenithSkills(ZenithSkillsID),
                        //FOREIGN KEY(AutomaticSkillsID) REFERENCES AutomaticSkills(AutomaticSkillsID),
                        //FOREIGN KEY(ActiveSkillsID) REFERENCES ActiveSkills(ActiveSkillsID),
                        //FOREIGN KEY(CaravanSkillsID) REFERENCES CaravanSkills(CaravanSkillsID),
                        //FOREIGN KEY(StyleRankSkillsID) REFERENCES StyleRankSkills(StyleRankSkillsID),
                        //FOREIGN KEY(PlayerInventoryID) REFERENCES PlayerInventory(PlayerInventoryID),
                        //FOREIGN KEY(AmmoPouchID) REFERENCES AmmoPouch(AmmoPouchID),
                        //FOREIGN KEY(RoadDureSkillsID) REFERENCES RoadDureSkills(RoadDureSkillsID)

                        // Insert data into the ZenithSkills table
                        sql = "INSERT INTO ZenithSkills (RunID, ZenithSkill1ID, ZenithSkill2ID, ZenithSkill3ID, ZenithSkill4ID, ZenithSkill5ID, ZenithSkill6ID, ZenithSkill7ID) VALUES (@RunID, @ZenithSkill1ID, @ZenithSkill2ID, @ZenithSkill3ID, @ZenithSkill4ID, @ZenithSkill5ID, @ZenithSkill6ID, @ZenithSkill7ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int zenithSkill1ID = model.ZenithSkill1();
                            int zenithSkill2ID = model.ZenithSkill2();
                            int zenithSkill3ID = model.ZenithSkill3();
                            int zenithSkill4ID = model.ZenithSkill4();
                            int zenithSkill5ID = model.ZenithSkill5();
                            int zenithSkill6ID = model.ZenithSkill6();
                            int zenithSkill7ID = model.ZenithSkill7();

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

                        sql = "INSERT INTO AutomaticSkills (RunID, AutomaticSkill1ID, AutomaticSkill2ID, AutomaticSkill3ID, AutomaticSkill4ID, AutomaticSkill5ID, AutomaticSkill6ID) VALUES (@RunID, @AutomaticSkill1ID, @AutomaticSkill2ID, @AutomaticSkill3ID, @AutomaticSkill4ID, @AutomaticSkill5ID, @AutomaticSkill6ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int automaticSkill1ID = model.AutomaticSkillWeapon();
                            int automaticSkill2ID = model.AutomaticSkillHead();
                            int automaticSkill3ID = model.AutomaticSkillChest();
                            int automaticSkill4ID = model.AutomaticSkillArms();
                            int automaticSkill5ID = model.AutomaticSkillWaist();
                            int automaticSkill6ID = model.AutomaticSkillLegs();

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

                        sql = "INSERT INTO ActiveSkills (RunID, ActiveSkill1ID, ActiveSkill2ID, ActiveSkill3ID, ActiveSkill4ID, ActiveSkill5ID, ActiveSkill6ID, ActiveSkill7ID, ActiveSkill8ID, ActiveSkill9ID, ActiveSkill10ID, ActiveSkill11ID, ActiveSkill12ID,ActiveSkill13ID,ActiveSkill14ID,ActiveSkill15ID,ActiveSkill16ID,ActiveSkill17ID,ActiveSkill18ID,ActiveSkill19ID) VALUES (@RunID, @ActiveSkill1ID, @ActiveSkill2ID, @ActiveSkill3ID, @ActiveSkill4ID, @ActiveSkill5ID, @ActiveSkill6ID, @ActiveSkill7ID, @ActiveSkill8ID, @ActiveSkill9ID, @ActiveSkill10ID, @ActiveSkill11ID, @ActiveSkill12ID, @ActiveSkill13ID, @ActiveSkill14ID, @ActiveSkill15ID, @ActiveSkill16ID, @ActiveSkill17ID, @ActiveSkill18ID, @ActiveSkill19ID)";
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

                        sql = "INSERT INTO CaravanSkills (RunID, CaravanSkill1ID, CaravanSkill2ID, CaravanSkill3ID) VALUES (@RunID, @CaravanSkill1ID, @CaravanSkill2ID, @CaravanSkill3ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int caravanSkill1ID = model.CaravanSkill1();
                            int caravanSkill2ID = model.CaravanSkill2();
                            int caravanSkill3ID = model.CaravanSkill3();

                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@ZenithSkill1ID", caravanSkill1ID);
                            cmd.Parameters.AddWithValue("@ZenithSkill2ID", caravanSkill2ID);
                            cmd.Parameters.AddWithValue("@ZenithSkill3ID", caravanSkill3ID);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int caravanSkillsID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            caravanSkillsID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = "INSERT INTO StyleRankSkills (RunID, StyleRankSkill1ID, StyleRankSkill2ID) VALUES (@RunID, @StyleRankSkill1ID, @StyleRankSkill2ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int styleRankSkill1ID = model.StyleRank1();
                            int styleRankSkill2ID = model.StyleRank2();

                            cmd.Parameters.AddWithValue("@RunID", runID);
                            cmd.Parameters.AddWithValue("@ZenithSkill1ID", styleRankSkill1ID);
                            cmd.Parameters.AddWithValue("@ZenithSkill2ID", styleRankSkill2ID);
                            cmd.ExecuteNonQuery();
                        }

                        sql = "SELECT LAST_INSERT_ROWID()";
                        int styleRankSkillsID;
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            styleRankSkillsID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        sql = @"INSERT INTO PlayerInventory (
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

                        sql = "INSERT INTO RoadDureSkills (RunID, ZenithSkill1ID, ZenithSkill2ID, ZenithSkill3ID, ZenithSkill4ID, ZenithSkill5ID, ZenithSkill6ID, ZenithSkill7ID) VALUES (@RunID, @ZenithSkill1ID, @ZenithSkill2ID, @ZenithSkill3ID, @ZenithSkill4ID, @ZenithSkill5ID, @ZenithSkill6ID, @ZenithSkill7ID)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            int zenithSkill1ID = model.ZenithSkill1();
                            int zenithSkill2ID = model.ZenithSkill2();
                            int zenithSkill3ID = model.ZenithSkill3();
                            int zenithSkill4ID = model.ZenithSkill4();
                            int zenithSkill5ID = model.ZenithSkill5();
                            int zenithSkill6ID = model.ZenithSkill6();
                            int zenithSkill7ID = model.ZenithSkill7();

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





                        string gearName = s.GearDescriptionExport;
                        string weaponClass = model.GetWeaponClass();
                        int weaponTypeID = model.WeaponType();
                        int weaponID = model.MeleeWeaponID();//ranged and melee are the same afaik
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

                        // Add the parameter values as SQLiteParameter objects
                        //cmd.Parameters.Add(new SQLiteParameter("@questID", questID));
                        //cmd.Parameters.Add(new SQLiteParameter("@areaID", areaID));
                        //cmd.Parameters.Add(new SQLiteParameter("@finalTimeValue", finalTimeValue));
                        //cmd.Parameters.Add(new SQLiteParameter("@finalTimeDisplay", finalTimeDisplay));
                        //cmd.Parameters.Add(new SQLiteParameter("@objectiveImage", finalTimeDisplay));
                        //cmd.Parameters.Add(new SQLiteParameter("@objectiveTypeID", finalTimeDisplay));
                        //cmd.Parameters.Add(new SQLiteParameter("@objectiveQuantity", finalTimeDisplay));
                        //cmd.Parameters.Add(new SQLiteParameter("@starGrade", finalTimeDisplay));
                        //cmd.Parameters.Add(new SQLiteParameter("@rankNameID", finalTimeDisplay));
                        //cmd.Parameters.Add(new SQLiteParameter("@objectiveName", finalTimeDisplay));
                        //cmd.Parameters.Add(new SQLiteParameter("@date", finalTimeDisplay));
                        //// Add the remaining parameters here

                        //// Execute the stored procedure
                        //cmd.ExecuteNonQuery();

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

        private void CreateDatabaseTriggers(SQLiteConnection conn)
        {
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    // goodbye world
                    // Commit the transaction
                    //transaction.Commit();
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
                "CREATE UNIQUE INDEX IF NOT EXISTS isx_allzenithskills_zenithskillid ON AllZenithSkills(ZenithSkillID)",
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
                "CREATE UNIQUE INDEX IF NOT EXISTS idx_zenithskills_runid ON ZenithSkills(RunID)"
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

        private void HandleError(SQLiteTransaction transaction, Exception ex)
        {
            // Handle the exception and show an error message to the user
            MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            // Roll back the transaction
            transaction.Rollback();
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
                string connectionString = "Data Source=" + dbFilePath + "";
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
                MessageBox.Show("An error occurred while accessing the database: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                // Handle an I/O exception
                MessageBox.Show("An error occurred while accessing a file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Handle any other exception
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion

        private readonly List<string> _validTableNames = new List<string> {"RankName", "ObjectiveType", "QuestName", "WeaponType", "Item", "Area", "AllZenithSkills", "AllArmorSkills", "AllCaravanSkills", "AllStyleRankSkills", "AllRoadDureSkills" };

        private void InsertIntoTable(IReadOnlyDictionary<int, string> dictionary, string tableName, string idColumn, string valueColumn, SQLiteConnection conn)
        {
            // Start a transaction
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    // Validate the input table name
                    if (!_validTableNames.Contains(tableName))
                    {
                        throw new ArgumentException("Invalid table name");
                    }

                    // Validate the input parameters
                    if (dictionary == null || dictionary.Count == 0)
                    {
                        throw new ArgumentException("Invalid dictionary");
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

                    //Create the Quests table
                    sql = @"CREATE TABLE IF NOT EXISTS Quests 
                    (RunID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    QuestID INTEGER NOT NULL, 
                    AreaID INTEGER NOT NULL, 
                    FinalTimeValue INTEGER NOT NULL,
                    FinalTimeDisplay TEXT NOT NULL, 
                    ObjectiveImage TEXT NOT NULL,
                    ObjectiveTypeID INTEGER NOT NULL, 
                    ObjectiveQuantity INTEGER NOT NULL, 
                    StarGrade INTEGER NOT NULL, 
                    RankNameID INTEGER NOT NULL, 
                    ObjectiveName TEXT NOT NULL, 
                    Date DATETIME NOT NULL,
                    FOREIGN KEY(QuestID) REFERENCES QuestName(QuestNameID),
                    FOREIGN KEY(AreaID) REFERENCES Area(AreaID),
                    FOREIGN KEY(ObjectiveTypeID) REFERENCES ObjectiveType(ObjectiveTypeID),
                    FOREIGN KEY(RankNameID) REFERENCES RankName(RankNameID)
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

                    InsertIntoTable(Dictionary.RanksBandsList.RankBandsID, "RankName", "RankNameID", "RankNameName", conn);

                    //Create the ObjectiveTypes table
                    sql = @"CREATE TABLE IF NOT EXISTS ObjectiveType
                    (ObjectiveTypeID INTEGER PRIMARY KEY, 
                    ObjectiveTypeName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertIntoTable(Dictionary.ObjectiveTypeList.ObjectiveTypeID, "ObjectiveType", "ObjectiveTypeID", "ObjectiveTypeName", conn);

                    //Create the QuestNames table
                    sql = @"CREATE TABLE IF NOT EXISTS QuestName
                    (QuestNameID INTEGER PRIMARY KEY, 
                    QuestNameName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertIntoTable(Quests.QuestIDs, "QuestName", "QuestNameID", "QuestNameName", conn);

                    // Create the Players table
                    //do an UPDATE when inserting quests. since its just local player?
                    sql = @"
                    CREATE TABLE IF NOT EXISTS Players (
                    PlayerID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    PlayerName TEXT NOT NULL,
                    GuildName TEXT NOT NULL,
                    Gender TEXT NOT NULL)";
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

                    InsertIntoTable(WeaponList.WeaponID, "WeaponType", "WeaponTypeID", "WeaponTypeName", conn);

                    // Create the Item table
                    sql = @"CREATE TABLE IF NOT EXISTS Item (
                    ItemID INTEGER PRIMARY KEY, 
                    ItemName TEXT NOT NULL)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    InsertIntoTable(Items.ItemIDs, "Item", "ItemID", "ItemName", conn);

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
                    sql = "INSERT OR REPLACE INTO Area (AreaID, AreaIcon, AreaName) VALUES (@AreaID, @AreaIcon, @AreaName)";
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
                    RunID INTEGER PRIMARY KEY AUTOINCREMENT, 
                    PlayerID INTEGER NOT NULL,
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
                    FOREIGN KEY(PlayerID) REFERENCES Players(PlayerID),
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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

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

                    InsertIntoTable(Dictionary.RoadDureSkills.RoadDureSkillIDs, "AllRoadDureSkills", "RoadDureSkillID", "RoadDureSkillName", conn);

                    sql = @"
                    CREATE TABLE IF NOT EXISTS Gear (
                        PieceID INTEGER NOT NULL,
                        PieceName TEXT NOT NULL,
                        PieceType TEXT NOT NULL,
                        PRIMARY KEY (PieceID, PieceType),
                        UNIQUE (PieceID, PieceName, PieceType)
                    )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

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

                    // Create a command to execute the insert or replace statement
                    using (SQLiteCommand updateCmd = new SQLiteCommand(conn))
                    {
                        // Create the parameter objects
                        SQLiteParameter pieceIdParam = new SQLiteParameter("@PieceID", DbType.Int32);
                        SQLiteParameter pieceNameParam = new SQLiteParameter("@PieceName", DbType.String);
                        SQLiteParameter pieceTypeParam = new SQLiteParameter("@PieceType", DbType.String);

                        // Add the parameters to the command
                        updateCmd.Parameters.Add(pieceIdParam);
                        updateCmd.Parameters.Add(pieceNameParam);
                        updateCmd.Parameters.Add(pieceTypeParam);

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
                                pieceIdParam.Value = pieceID;
                                pieceNameParam.Value = pieceName;
                                pieceTypeParam.Value = pieceType;

                                // Set the command text for the insert or replace statement
                                updateCmd.CommandText = @"
                                INSERT OR REPLACE INTO Gear (PieceID, PieceName, PieceType)
                                VALUES (@PieceID, @PieceName, @PieceType);
                                ";
                                // Execute the statement
                                updateCmd.ExecuteNonQuery();
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
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + _connectionString + "");
            
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
