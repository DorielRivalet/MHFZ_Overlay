// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Manager;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MHFZ_Overlay;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Mappers;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services.DataAccessLayer;
using Newtonsoft.Json;
using Wpf.Ui.Controls;

public class AchievementManager
{

    public static AchievementManager GetInstance()
    {
        if (instance == null)
        {
            LoggerInstance.Debug("Singleton not found, creating instance.");
            instance = new AchievementManager();
        }

        LoggerInstance.Debug("Singleton found, returning instance.");
        LoggerInstance.Trace(new StackTrace().ToString());
        return instance;
    }

    public void LoadPlayerAchievements()
    {
        var playerAchievements = DatabaseManagerInstance.GetPlayerAchievementIDList();

        foreach (var achievementID in playerAchievements)
        {
            obtainedAchievements.Add(achievementID);
        }
    }

    public async Task CheckForAchievementsAsync(Snackbar snackbar, DataLoader dataLoader, DatabaseManager DatabaseManagerInstance, Settings s)
    {
        var newAchievements = GetNewlyObtainedAchievements(dataLoader, DatabaseManagerInstance, s);

        if (newAchievements.Count > 0)
        {
            UpdatePlayerAchievements(newAchievements);
            await Achievement.ShowMany(snackbar, newAchievements);
            LoggerInstance.Info(CultureInfo.InvariantCulture, "Awarded achievements: {0}", JsonConvert.SerializeObject(newAchievements));
        }
        else
        {
            LoggerInstance.Info(CultureInfo.InvariantCulture, "No new achievements found");
        }
    }

    public async Task RewardAchievement(int achievementID, Snackbar snackbar)
    {
        AchievementsMapper.IDAchievement.TryGetValue(achievementID, out var achievement);
        if (achievement == null)
        {
            return;
        }

        if (!obtainedAchievements.Contains(achievementID))
        {
            obtainedAchievements.Add(achievementID);
            // Store the achievement in the SQLite PlayerAchievements table
            DatabaseManagerInstance.StoreAchievement(achievementID);
            await achievement.Show(snackbar);
            LoggerInstance.Info(CultureInfo.InvariantCulture, "Awarded achievement ID {0}", achievementID);
        }
        else
        {
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Achievement ID {0} already found", achievementID);
        }
    }

    private static readonly DatabaseManager DatabaseManagerInstance = DatabaseManager.GetInstance();
    private static readonly NLog.Logger LoggerInstance = NLog.LogManager.GetCurrentClassLogger();
    private static AchievementManager? instance;
    private HashSet<int> obtainedAchievements = new();

    private AchievementManager()
    {
        LoggerInstance.Info($"AchievementManager initialized");
    }

    private List<int> GetNewlyObtainedAchievements(DataLoader dataLoader, DatabaseManager DatabaseManagerInstance, Settings s)
    {
        var newAchievements = new List<int>();

        foreach (var kvp in AchievementsMapper.IDAchievement)
        {
            var achievementID = kvp.Key;
            var achievement = kvp.Value;

            // Check the specific conditions for obtaining the achievement
            if (!obtainedAchievements.Contains(achievementID) && CheckConditionsForAchievement(achievementID, dataLoader, DatabaseManagerInstance, s))
            {
                newAchievements.Add(achievementID);
            }
        }

        return newAchievements;
    }

    private bool CheckConditionsForAchievement(int achievementID, DataLoader dataLoader, DatabaseManager DatabaseManagerInstance, Settings s)
    {
        // Implement your logic here to check the conditions for obtaining the achievement
        // Return true if the conditions are met, false otherwise
        // You can access the properties of the achievement object to perform the checks

        switch (achievementID)
        {
            default:
                return false;
            case 0:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_AKURA_VASHIMU);
            case 1:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_AKURA_VASHIMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 2:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_AKURA_VASHIMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 3:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_AKURA_VASHIMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 4:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_AKURA_VASHIMU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 5:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ANORUPATISU);
            case 6:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ANORUPATISU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 7:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ANORUPATISU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 8:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ANORUPATISU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 9:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ANORUPATISU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 10:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BLANGONGA);
            case 11:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BLANGONGA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 12:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BLANGONGA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 13:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BLANGONGA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 14:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BLANGONGA && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 15:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DAIMYO_HERMITAUR);
            case 16:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DAIMYO_HERMITAUR) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 17:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DAIMYO_HERMITAUR) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 18:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DAIMYO_HERMITAUR) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 19:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DAIMYO_HERMITAUR && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 20:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DORAGYUROSU);
            case 21:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DORAGYUROSU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 22:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DORAGYUROSU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 23:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DORAGYUROSU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 24:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DORAGYUROSU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 25:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ESPINAS);
            case 26:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ESPINAS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 27:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ESPINAS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 28:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ESPINAS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 29:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ESPINAS && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 30:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GASURABAZURA);
            case 31:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GASURABAZURA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 32:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GASURABAZURA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 33:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GASURABAZURA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 34:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GASURABAZURA && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 35:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GIAORUGU);
            case 36:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GIAORUGU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 37:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GIAORUGU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 38:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GIAORUGU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 39:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GIAORUGU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 40:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYPNOCATRICE);
            case 41:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYPNOCATRICE) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 42:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYPNOCATRICE) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 43:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYPNOCATRICE) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 44:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYPNOCATRICE && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 45:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYUJIKIKI);
            case 46:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYUJIKIKI) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 47:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYUJIKIKI) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 48:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYUJIKIKI) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 49:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYUJIKIKI && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 50:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_INAGAMI);
            case 51:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_INAGAMI) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 52:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_INAGAMI) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 53:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_INAGAMI) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 54:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_INAGAMI && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 55:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_KHEZU);
            case 56:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_KHEZU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 57:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_KHEZU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 58:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_KHEZU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 59:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_KHEZU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 60:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_MIDOGARON);
            case 61:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_MIDOGARON) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 62:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_MIDOGARON) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 63:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_MIDOGARON) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 64:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_MIDOGARON && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 65:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_HUGE_PLESIOTH);
            case 66:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_HUGE_PLESIOTH) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 67:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_HUGE_PLESIOTH) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 68:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_HUGE_PLESIOTH) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 69:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_HUGE_PLESIOTH && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 70:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RATHALOS);
            case 71:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RATHALOS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 72:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RATHALOS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 73:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RATHALOS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 74:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RATHALOS && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 75:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RUKODIORA);
            case 76:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RUKODIORA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 77:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RUKODIORA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 78:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RUKODIORA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 79:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RUKODIORA && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 80:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TIGREX);
            case 81:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TIGREX) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 82:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TIGREX) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 83:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TIGREX) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 84:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TIGREX && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 85:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TORIDCLESS);
            case 86:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TORIDCLESS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 87:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TORIDCLESS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 88:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TORIDCLESS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 89:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TORIDCLESS && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 90:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BARURAGARU);
            case 91:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BARURAGARU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 92:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BARURAGARU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 93:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BARURAGARU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 94:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BARURAGARU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 95:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BOGABADORUMU);
            case 96:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BOGABADORUMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 97:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BOGABADORUMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 98:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BOGABADORUMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 99:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BOGABADORUMU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 100:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GRAVIOS);
            case 101:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GRAVIOS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 102:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GRAVIOS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 103:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GRAVIOS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 104:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GRAVIOS && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 105:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HARUDOMERUGU);
            case 106:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HARUDOMERUGU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 107:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HARUDOMERUGU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 108:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HARUDOMERUGU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 109:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HARUDOMERUGU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 110:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TAIKUN_ZAMUZA);
            case 111:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TAIKUN_ZAMUZA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 112:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TAIKUN_ZAMUZA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 113:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TAIKUN_ZAMUZA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 114:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TAIKUN_ZAMUZA && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 115:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_FATALIS);
            case 116:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_FATALIS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 117:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_FATALIS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 118:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_FATALIS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 119:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_FATALIS && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 120:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_CRIMSON_FATALIS);
            case 121:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_CRIMSON_FATALIS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 122:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_CRIMSON_FATALIS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 123:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_CRIMSON_FATALIS) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 124:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_CRIMSON_FATALIS && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 125:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_SHANTIEN);
            case 126:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_SHANTIEN) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 127:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_SHANTIEN) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 128:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_SHANTIEN) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 129:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_SHANTIEN && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 130:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_DISUFIROA);
            case 131:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_DISUFIROA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 132:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_DISUFIROA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 133:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_DISUFIROA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 134:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_DISUFIROA && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 135:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_UNKNOWN);
            case 136:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_UNKNOWN) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 137:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_UNKNOWN) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 138:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_UNKNOWN) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 139:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_UNKNOWN && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 140:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_DISUFIROA);
            case 141:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_DISUFIROA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 142:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_DISUFIROA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 143:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_DISUFIROA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 144:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_DISUFIROA && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 145:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_THIRSTY_PARIAPURIA);
            case 146:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_THIRSTY_PARIAPURIA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 147:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_THIRSTY_PARIAPURIA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 148:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_THIRSTY_PARIAPURIA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 149:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_THIRSTY_PARIAPURIA && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 150:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_RULING_GUANZORUMU);
            case 151:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_RULING_GUANZORUMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 152:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_RULING_GUANZORUMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 153:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_RULING_GUANZORUMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 154:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_RULING_GUANZORUMU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 155:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_SHIFTING_MI_RU);
            case 156:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_SHIFTING_MI_RU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 157:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_SHIFTING_MI_RU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 158:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_SHIFTING_MI_RU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 159:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_SHIFTING_MI_RU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 160:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_FOREST || quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_HISTORIC);
            case 161:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_FOREST || quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 162:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_FOREST || quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 163:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_FOREST || quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 164:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_FOREST || quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_HISTORIC && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 165:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_FOREST || quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_HISTORIC);
            case 166:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_FOREST || quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 167:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_FOREST || quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 168:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_FOREST || quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 169:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_FOREST || quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_HISTORIC && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 170:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_ARENA || quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_HISTORIC);
            case 171:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_ARENA || quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 172:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_ARENA || quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 173:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_ARENA || quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 174:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_ARENA || quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_HISTORIC && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 175:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU || quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU_EVENT);
            case 176:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU || quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU_EVENT) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 177:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU || quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU_EVENT) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 178:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU || quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU_EVENT) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 179:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU || quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU_EVENT && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 180:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_ARROGANT_DUREMUDIRA);
            case 181:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_ARROGANT_DUREMUDIRA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 182:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_ARROGANT_DUREMUDIRA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 183:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_ARROGANT_DUREMUDIRA) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 184:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_ARROGANT_DUREMUDIRA && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 185:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_BLITZKRIEG_BOGABADORUMU);
            case 186:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_BLITZKRIEG_BOGABADORUMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 187:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_BLITZKRIEG_BOGABADORUMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 188:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_BLITZKRIEG_BOGABADORUMU) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 189:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_BLITZKRIEG_BOGABADORUMU && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 190:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_TOWER || quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_HISTORIC);
            case 191:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_TOWER || quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_SLAYER)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 192:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_TOWER || quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 193:
                if (DatabaseManagerInstance.allQuests.Count(quest => quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_TOWER || quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_HISTORIC) >= Numbers.REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 194://
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_TOWER || quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_HISTORIC && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 195:
                // Join quests and player inventories based on RunID
                var completedQuests = from quest in DatabaseManagerInstance.allQuests
                                      join playerInventory in DatabaseManagerInstance.allPlayerInventories on quest.RunID equals playerInventory.RunID
                                      where quest.QuestID == Numbers.QUEST_ID_THIRSTY_PARIAPURIA &&
                                            (playerInventory.Item1ID == 4943 ||
                                             playerInventory.Item2ID == 4943 ||
                                             playerInventory.Item3ID == 4943 ||
                                             playerInventory.Item4ID == 4943 ||
                                             playerInventory.Item5ID == 4943 ||
                                             playerInventory.Item6ID == 4943 ||
                                             playerInventory.Item7ID == 4943 ||
                                             playerInventory.Item8ID == 4943 ||
                                             playerInventory.Item9ID == 4943 ||
                                             playerInventory.Item10ID == 4943 ||
                                             playerInventory.Item11ID == 4943 ||
                                             playerInventory.Item12ID == 4943 ||
                                             playerInventory.Item13ID == 4943 ||
                                             playerInventory.Item14ID == 4943 ||
                                             playerInventory.Item15ID == 4943 ||
                                             playerInventory.Item16ID == 4943 ||
                                             playerInventory.Item17ID == 4943 ||
                                             playerInventory.Item18ID == 4943 ||
                                             playerInventory.Item19ID == 4943 ||
                                             playerInventory.Item20ID == 4943)
                                      select quest;
                if (completedQuests != null && completedQuests.Any())
                    return true;
                else
                    return false;
            case 196:
                completedQuests = from quest in DatabaseManagerInstance.allQuests
                                  join playerGear in DatabaseManagerInstance.allPlayerGear on quest.RunID equals playerGear.RunID
                                  where quest.QuestID == Numbers.QUEST_ID_RULING_GUANZORUMU && playerGear.StyleID != 3
                                  select quest;
                if (completedQuests != null && completedQuests.Any())
                    return true;
                else
                    return false;
            case 197:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_SHIFTING_MI_RU && quest.KeyStrokesDictionary != null && JsonConvert.DeserializeObject<Dictionary<int, string>>(quest.KeyStrokesDictionary).Values.First() == "LShiftKey");
            case 198:
                return DatabaseManagerInstance.allQuests.Any(quest => (quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_FOREST || quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_HISTORIC) && quest.HitsTakenBlockedDictionary != null && JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(quest.HitsTakenBlockedDictionary).Count == 0);
            case 199:
                return DatabaseManagerInstance.allQuests.Any(quest => (quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_FOREST || quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_HISTORIC) && quest.PartySize == 1 && quest.PlayerStaminaDictionary != null && JsonConvert.DeserializeObject<Dictionary<int, int>>(quest.PlayerStaminaDictionary).Values.First() <= 75);
            case 200:
                completedQuests = from quest in DatabaseManagerInstance.allQuests
                                  join activeSkills in DatabaseManagerInstance.allActiveSkills on quest.RunID equals activeSkills.RunID
                                  where (quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_HISTORIC || quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_TOWER) &&
                                  (activeSkills.ActiveSkill1ID == 473 || activeSkills.ActiveSkill1ID == 504 ||
                                  activeSkills.ActiveSkill2ID == 473 || activeSkills.ActiveSkill2ID == 504 ||
                                  activeSkills.ActiveSkill3ID == 473 || activeSkills.ActiveSkill3ID == 504 ||
                                  activeSkills.ActiveSkill4ID == 473 || activeSkills.ActiveSkill4ID == 504 ||
                                  activeSkills.ActiveSkill5ID == 473 || activeSkills.ActiveSkill5ID == 504 ||
                                  activeSkills.ActiveSkill6ID == 473 || activeSkills.ActiveSkill6ID == 504 ||
                                  activeSkills.ActiveSkill7ID == 473 || activeSkills.ActiveSkill7ID == 504 ||
                                  activeSkills.ActiveSkill8ID == 473 || activeSkills.ActiveSkill8ID == 504 ||
                                  activeSkills.ActiveSkill9ID == 473 || activeSkills.ActiveSkill9ID == 504 ||
                                  activeSkills.ActiveSkill10ID == 473 || activeSkills.ActiveSkill10ID == 504 ||
                                  activeSkills.ActiveSkill11ID == 473 || activeSkills.ActiveSkill11ID == 504 ||
                                  activeSkills.ActiveSkill12ID == 473 || activeSkills.ActiveSkill12ID == 504 ||
                                  activeSkills.ActiveSkill13ID == 473 || activeSkills.ActiveSkill13ID == 504 ||
                                  activeSkills.ActiveSkill14ID == 473 || activeSkills.ActiveSkill14ID == 504 ||
                                  activeSkills.ActiveSkill15ID == 473 || activeSkills.ActiveSkill15ID == 504 ||
                                  activeSkills.ActiveSkill16ID == 473 || activeSkills.ActiveSkill16ID == 504 ||
                                  activeSkills.ActiveSkill17ID == 473 || activeSkills.ActiveSkill17ID == 504 ||
                                  activeSkills.ActiveSkill18ID == 473 || activeSkills.ActiveSkill18ID == 504 ||
                                  activeSkills.ActiveSkill19ID == 473 || activeSkills.ActiveSkill19ID == 504)
                                  select quest;
                if (completedQuests != null && completedQuests.Any())
                    return true;
                else
                    return false;
            case 201:
                completedQuests = from quest in DatabaseManagerInstance.allQuests
                                  join playerInventory in DatabaseManagerInstance.allPlayerInventories on quest.RunID equals playerInventory.RunID
                                  where quest.QuestID == Numbers.QUEST_ID_BLITZKRIEG_BOGABADORUMU &&
                                        (playerInventory.Item1ID == 93 ||
                                         playerInventory.Item2ID == 93 ||
                                         playerInventory.Item3ID == 93 ||
                                         playerInventory.Item4ID == 93 ||
                                         playerInventory.Item5ID == 93 ||
                                         playerInventory.Item6ID == 93 ||
                                         playerInventory.Item7ID == 93 ||
                                         playerInventory.Item8ID == 93 ||
                                         playerInventory.Item9ID == 93 ||
                                         playerInventory.Item10ID == 93 ||
                                         playerInventory.Item11ID == 93 ||
                                         playerInventory.Item12ID == 93 ||
                                         playerInventory.Item13ID == 93 ||
                                         playerInventory.Item14ID == 93 ||
                                         playerInventory.Item15ID == 93 ||
                                         playerInventory.Item16ID == 93 ||
                                         playerInventory.Item17ID == 93 ||
                                         playerInventory.Item18ID == 93 ||
                                         playerInventory.Item19ID == 93 ||
                                         playerInventory.Item20ID == 93)
                                  select quest;
                if (completedQuests != null && completedQuests.Any())
                    return true;
                else
                    return false;
            case 202:
                completedQuests = from quest in DatabaseManagerInstance.allQuests
                                  join styleRankSkills in DatabaseManagerInstance.allStyleRankSkills on quest.RunID equals styleRankSkills.RunID
                                  where quest.QuestID == Numbers.QUEST_ID_ARROGANT_DUREMUDIRA &&
                                        (styleRankSkills.StyleRankSkill1ID == 14 || styleRankSkills.StyleRankSkill2ID == 14)
                                  select quest;
                if (completedQuests != null && completedQuests.Any())
                    return true;
                else
                    return false;
            case 203:
                // Set the target date to February 14th of the current year
                var targetDate = new DateTime(DateTime.UtcNow.Year, 2, 14);

                if (DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_VEGGIE_ELDER_LOVE && quest.CreatedAt?.Date == targetDate.Date))
                    return true;
                else
                    return false;
            case 204:
                completedQuests = from quest in DatabaseManagerInstance.allQuests
                                  join playerGear in DatabaseManagerInstance.allPlayerGear on quest.RunID equals playerGear.RunID
                                  where (quest.QuestID == Numbers.QUEST_ID_PRODUCER_GOGOMOA_HR || quest.QuestID == Numbers.QUEST_ID_PRODUCER_GOGOMOA_LR) &&
                                  playerGear.WeaponTypeID == 9
                                  select quest;
                if (completedQuests != null && completedQuests.Any())
                    return true;
                else
                    return false;
            case 205:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_FOUR_HEAVENLY_KING_MALE_1 || quest.QuestID == Numbers.QUEST_ID_FOUR_HEAVENLY_KING_MALE_2 || quest.QuestID == Numbers.QUEST_ID_FOUR_HEAVENLY_KING_FEMALE_1 || quest.QuestID == Numbers.QUEST_ID_FOUR_HEAVENLY_KING_FEMALE_2);
            case 206:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_HATSUNE_MIKU);
            case 207:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_PSO2);
            case 208:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_MEGAMAN);
            case 209:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_HIGANJIMA);
            case 210:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_HUGE_PLESIOTH);
            case 211:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_SUNGLASSES_KUTKU);
            case 212:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_MHFQ);
            case 213:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_CONGALALA_CURE);
            case 214:
                if (dataLoader.model.GZenny() >= 9_999_999)
                    return true;
                else
                    return false;
            case 215: // TODO test
                if (dataLoader.model.DivaBond() >= 999)
                    return true;
                else
                    return false;
            case 216:// TODO Obtain S Rank in all single-player MezFes minigames
                return false;
            case 217:
                if (dataLoader.model.CaravanPoints() >= 9_999_999)
                    return true;
                else
                    return false;
            case 218:
                if (dataLoader.model.RoadMaxStagesMultiplayer() >= 50)
                    return true;
                else
                    return false;
            case 219:
                if (dataLoader.model.RoadMaxStagesMultiplayer() >= 100)
                    return true;
                else
                    return false;
            case 220:
                if (dataLoader.model.PartnerLevel() >= 999)
                    return true;
                else
                    return false;
            case 221:
                return DatabaseManagerInstance.allQuestAttempts.Any(questAttempts => questAttempts.Attempts >= 1_000);
            case 222:
                return DatabaseManagerInstance.allPersonalBestAttempts.Any(pbAttempts => pbAttempts.Attempts >= 100);
            case 223:
                if (dataLoader.model.SecondDistrictDuremudiraSlays() >= 100)
                    return true;
                else
                    return false;
            case 224:
                if (dataLoader.model.RoadFatalisSlain() >= 100)
                    return true;
                else
                    return false;
            case 225:// fumo
                return false;
            case 226:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_TWINHEAD_RAJANGS_HISTORIC);
            case 227:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_AKURA_VASHIMU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 228:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ANORUPATISU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 229:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BLANGONGA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 230:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DAIMYO_HERMITAUR && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 231:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DORAGYUROSU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 232:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ESPINAS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 233:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GASURABAZURA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 234:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GIAORUGU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 235:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYPNOCATRICE && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 236:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYUJIKIKI && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 237:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_INAGAMI && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 238:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_KHEZU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 239:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_MIDOGARON && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 240:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_PLESIOTH && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 241:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RATHALOS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 242:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RUKODIORA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 243:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TIGREX && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 244:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TORIDCLESS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 245:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BARURAGARU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 246:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BOGABADORUMU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 247:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GRAVIOS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 248:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HARUDOMERUGU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 249:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TAIKUN_ZAMUZA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 250:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_FATALIS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 251:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_CRIMSON_FATALIS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 252:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_SHANTIEN && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 253:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_DISUFIROA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 10);
            case 254:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_AKURA_VASHIMU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 255:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ANORUPATISU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 256:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BLANGONGA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 257:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DAIMYO_HERMITAUR && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 258:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DORAGYUROSU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 259:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ESPINAS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 260:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GASURABAZURA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 261:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GIAORUGU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 262:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYPNOCATRICE && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 263:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYUJIKIKI && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 264:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_INAGAMI && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 265:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_KHEZU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 266:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_MIDOGARON && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 267:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_PLESIOTH && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 268:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RATHALOS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 269:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RUKODIORA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 270:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TIGREX && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 271:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TORIDCLESS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 272:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BARURAGARU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 273:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BOGABADORUMU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 274:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GRAVIOS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 275:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HARUDOMERUGU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 276:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TAIKUN_ZAMUZA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 277:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_FATALIS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 278:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_CRIMSON_FATALIS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 279:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_SHANTIEN && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 280:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_DISUFIROA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 8);
            case 281:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_AKURA_VASHIMU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 282:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ANORUPATISU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 283:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BLANGONGA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 284:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DAIMYO_HERMITAUR && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 285:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_DORAGYUROSU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 286:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_ESPINAS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 287:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GASURABAZURA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 288:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GIAORUGU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 289:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYPNOCATRICE && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 290:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HYUJIKIKI && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 291:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_INAGAMI && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 292:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_KHEZU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 293:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_MIDOGARON && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 294:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_PLESIOTH && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 295:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RATHALOS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 296:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_RUKODIORA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 297:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TIGREX && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 298:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TORIDCLESS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 299:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BARURAGARU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 300:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_BOGABADORUMU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 301:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_GRAVIOS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 302:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_HARUDOMERUGU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 303:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_Z4_TAIKUN_ZAMUZA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 304:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_FATALIS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 305:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_CRIMSON_FATALIS && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 306:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_SHANTIEN && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 307:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_LV9999_DISUFIROA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 308:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_THIRSTY_PARIAPURIA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 3);
            case 309:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_SHIFTING_MI_RU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 310:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_RULING_GUANZORUMU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 5);
            case 311:
                return DatabaseManagerInstance.allQuests.Any(quest => (quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_FOREST || quest.QuestID == Numbers.QUEST_ID_BLINKING_NARGACUGA_HISTORIC) && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 7);
            case 312:
                return DatabaseManagerInstance.allQuests.Any(quest => (quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_FOREST || quest.QuestID == Numbers.QUEST_ID_HOWLING_ZINOGRE_HISTORIC) && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 7);
            case 313:
                return DatabaseManagerInstance.allQuests.Any(quest => (quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU || quest.QuestID == Numbers.QUEST_ID_SPARKLING_ZERUREUSU_EVENT) && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 9);
            case 314:
                return DatabaseManagerInstance.allQuests.Any(quest => (quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_ARENA || quest.QuestID == Numbers.QUEST_ID_STARVING_DEVILJHO_HISTORIC) && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 9);
            case 315:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_ARROGANT_DUREMUDIRA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 9);
            case 316:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_BLITZKRIEG_BOGABADORUMU && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 9);
            case 317:
                return DatabaseManagerInstance.allQuests.Any(quest => (quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_HISTORIC || quest.QuestID == Numbers.QUEST_ID_BURNING_FREEZING_ELZELION_TOWER) && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 9);
            case 318:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_UNKNOWN && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 9);
            case 319:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_UPPER_SHITEN_DISUFIROA && quest.FinalTimeValue < Numbers.FRAMES_1_MINUTE * 9);
            case 320:
                return DatabaseManagerInstance.allBingo.Any(bingo => bingo.Difficulty == Difficulty.Easy);
            case 321:
                return DatabaseManagerInstance.allBingo.Any(bingo => bingo.Difficulty == Difficulty.Medium);
            case 322:
                return DatabaseManagerInstance.allBingo.Any(bingo => bingo.Difficulty == Difficulty.Hard);
            case 323:
                return DatabaseManagerInstance.allBingo.Any(bingo => bingo.Difficulty == Difficulty.Extreme);
            case 324:
                if (DatabaseManagerInstance.allGachaCards.Count >= 1)
                    return true;
                else
                    return false;
            case 325:
                if (DatabaseManagerInstance.allGachaCards.Count >= 100)
                    return true;
                else
                    return false;
            case 326:
                if (DatabaseManagerInstance.allGachaCards.Count >= 1000)
                    return true;
                else
                    return false;
            case 327: // TODO obtain all gacha cards
                return false;
            case 328:
                if (DatabaseManagerInstance.allZenithGauntlets.Count >= 1)
                    return true;
                else
                    return false;
            case 329:
                if (DatabaseManagerInstance.allZenithGauntlets.Count >= 10)
                    return true;
                else
                    return false;
            case 330:
                if (DatabaseManagerInstance.allZenithGauntlets.Count >= 25)
                    return true;
                else
                    return false;
            case 331:
                if (DatabaseManagerInstance.allZenithGauntlets.Count == 0)
                    return false;
                return DatabaseManagerInstance.allZenithGauntlets.Any(gauntlet =>
                {
                    if (TimeSpan.TryParse(gauntlet.TotalTimeElapsed, out var timeElapsed))
                    {
                        return timeElapsed < TimeSpan.FromHours(4);
                    }
                    return false; // Handle invalid TotalTimeElapsed values
                });
            case 332:
                if (DatabaseManagerInstance.allSolsticeGauntlets.Count >= 1)
                    return true;
                else
                    return false;
            case 333:
                if (DatabaseManagerInstance.allSolsticeGauntlets.Count >= 10)
                    return true;
                else
                    return false;
            case 334:
                if (DatabaseManagerInstance.allSolsticeGauntlets.Count >= 25)
                    return true;
                else
                    return false;
            case 335:
                if (DatabaseManagerInstance.allSolsticeGauntlets.Count == 0)
                    return false;
                return DatabaseManagerInstance.allSolsticeGauntlets.Any(gauntlet =>
                {
                    if (TimeSpan.TryParse(gauntlet.TotalTimeElapsed, out var timeElapsed))
                    {
                        return timeElapsed < TimeSpan.FromHours(1);
                    }
                    return false; // Handle invalid TotalTimeElapsed values
                });
            case 336:
                if (DatabaseManagerInstance.allMusouGauntlets.Count >= 1)
                    return true;
                else
                    return false;
            case 337:
                if (DatabaseManagerInstance.allMusouGauntlets.Count >= 10)
                    return true;
                else
                    return false;
            case 338:
                if (DatabaseManagerInstance.allMusouGauntlets.Count >= 25)
                    return true;
                else
                    return false;
            case 339:
                if (DatabaseManagerInstance.allMusouGauntlets.Count == 0)
                    return false;
                return DatabaseManagerInstance.allMusouGauntlets.Any(gauntlet =>
                {
                    if (TimeSpan.TryParse(gauntlet.TotalTimeElapsed, out var timeElapsed))
                    {
                        return timeElapsed < TimeSpan.FromMinutes(100);
                    }
                    return false; // Handle invalid TotalTimeElapsed values
                });
            case 340:// TODO discord rich presence
                return s.EnableRichPresence;
            case 341:
                if (dataLoader.model.GetOverlayMode().Contains("Zen"))
                    return true;
                else
                    return false;
            case 342:
                if (dataLoader.model.GetOverlayMode().Contains("Freestyle"))
                    return true;
                else
                    return false;
            case 343:
                if (DatabaseManagerInstance.allPlayerGear.Count(playerGear => playerGear.GuildFoodID != 0) >= 50)
                    return true;
                else
                    return false;
            case 344:
                if (DatabaseManagerInstance.allPlayerGear.Count(playerGear => playerGear.DivaSkillID != 0) >= 50)
                    return true;
                else
                    return false;
            case 345:// TODO gallery
                return false;
            case 346:
                if (dataLoader.model.CalculateTotalLargeMonstersHunted() >= 1000)
                    return true;
                else
                    return false;
            case 347:
                if (DatabaseManagerInstance.GetTotalQuestTimeElapsed() >= Numbers.FRAMES_1_HOUR * 100)
                    return true;
                else
                    return false;
            case 348: // TODO idk if i should check by name
                EZlion.Mapper.WeaponBlademaster.IDName.TryGetValue(dataLoader.model.BlademasterWeaponID(), out var blademasterWeaponName);
                EZlion.Mapper.WeaponGunner.IDName.TryGetValue(dataLoader.model.GunnerWeaponID(), out var gunnerWeaponName);

                if (dataLoader.model.GRWeaponLv() == 100 && (
                    blademasterWeaponName != null && (blademasterWeaponName.Contains("\"Shine\"") || blademasterWeaponName.Contains("\"Clear\"") || blademasterWeaponName.Contains("\"Flash\"") || blademasterWeaponName.Contains("\"Glory\""))
                     || gunnerWeaponName != null && (gunnerWeaponName.Contains("\"Shine\"") || gunnerWeaponName.Contains("\"Clear\"") || gunnerWeaponName.Contains("\"Flash\"") || gunnerWeaponName.Contains("\"Glory\""))
                    )
                )
                    return true;
                else
                    return false;
            case 349:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_MOSSWINE_REVENGE);
            case 350:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_JUNGLE_PUZZLE);
            case 351:
                if (DatabaseManagerInstance.allPlayerGear.Count(playerGear => playerGear.PoogieItemID != 0) >= 100)
                    return true;
                else
                    return false;
            case 352:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_NUCLEAR_GYPCEROS);
            case 353:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_MOSSWINE_DUEL);
            case 354:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_MOSSWINE_LAST_STAND);
            case 355:
                return DatabaseManagerInstance.allQuests.Any(quest => quest.QuestID == Numbers.QUEST_ID_HALLOWEEN_SPEEDSTER);
            case 356:// TODO 1000 bingo points in 1 go
                return false;
            case 357:
                if (DatabaseManagerInstance.allBingo.Count >= 1)
                    return true;
                else
                    return false;
            case 358:
                if (DatabaseManagerInstance.allBingo.Count >= 10)
                    return true;
                else
                    return false;
            case 359:
                if (DatabaseManagerInstance.allBingo.Count >= 25)
                    return true;
                else
                    return false;
            case 360:
                if (DatabaseManagerInstance.allBingo.Count >= 50)
                    return true;
                else
                    return false;
            case 361:// TODO gacha stuff
            case 362:
            case 363:
            case 364:
            case 365:
            case 366:
            case 367:
            case 368:
            case 369:
            case 370:
            case 371:
            case 372:
            case 373:
            case 374:
            case 375:
            case 376:
            case 377:
            case 378:
            case 379:
            case 380:
            case 381:
            case 382:
            case 383:
            case 384:
            case 385:
            case 386:
            case 387:
            case 388:
            case 389:
            case 390:
            case 391:
            case 392:
            case 393:
            case 394:
            case 395:
            case 396:
            case 397:
            case 398:
            case 399:
            case 400:
                return false;
        }
    }

    private void UpdatePlayerAchievements(List<int> achievementsID)
    {
        // Update the player achievements table in the database with the newly obtained achievements
        // Use the provided database update logic or similar approach

        foreach (var achievementID in achievementsID)
        {
            obtainedAchievements.Add(achievementID);
            // Store the achievement in the SQLite PlayerAchievements table
            DatabaseManagerInstance.StoreAchievement(achievementID);
        }
    }

}
