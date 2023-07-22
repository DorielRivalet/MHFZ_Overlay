// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using MHFZ_Overlay;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services.Contracts;
using Newtonsoft.Json;
using NLog;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;

public sealed class AchievementService : IAchievementService
{
    public static AchievementRank ConvertToAchievementRank(long rankValue)
    {
        switch (rankValue)
        {
            case 1:
                return AchievementRank.Bronze;
            case 2:
                return AchievementRank.Silver;
            case 3:
                return AchievementRank.Gold;
            case 4:
                return AchievementRank.Platinum;
            default:
                return AchievementRank.None;
        }
    }

    public static List<Achievement> FilterAchievementsToCompletedOnly(List<Achievement> achievements)
    {
        List<Achievement> filteredAchievements = achievements.Where(a => a.CompletionDate != DateTime.UnixEpoch).ToList();
        return filteredAchievements;
    }

    public static async Task ShowMany(Snackbar snackbar, List<int> achievementsID)
    {
        const int maxAchievementsToShow = 5;
        var remainingAchievements = achievementsID.Count - maxAchievementsToShow;

        foreach (var achievementID in achievementsID.Take(maxAchievementsToShow))
        {
            if (Achievements.IDAchievement.TryGetValue(achievementID, out Achievement? achievement) && achievement != null)
            {
                await achievement.Show(snackbar);
                await Task.Delay(TimeSpan.FromSeconds(2)); // Delay between each achievement
            }
        }

        if (remainingAchievements > 0)
        {
            await ShowAchievementsTabInfo(snackbar, remainingAchievements);
        }
    }

    public static async Task ShowAchievementsTabInfo(Snackbar snackbar, int remainingAchievements)
    {
        var brushConverter = new BrushConverter();
        var brushColor = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Crust"]);
        snackbar.Title = "Too many achievements!";
        snackbar.Message = $"To see the rest of the achievements unlocked ({remainingAchievements} left), see the Achievements tab in the Quests Logs section.";
        snackbar.Icon = new SymbolIcon()
        {
            Symbol = SymbolRegular.Info28,
        };
        snackbar.Icon.Foreground = brushColor ?? Brushes.Black;
        snackbar.Appearance = ControlAppearance.Info;
        await snackbar.ShowAsync();
        await Task.Delay(TimeSpan.FromSeconds(1)); // Delay for a certain duration
        snackbar.Hide(); // Hide the snackbar
    }

    public static AchievementService GetInstance()
    {
        if (instance == null)
        {
            LoggerInstance.Debug("Singleton not found, creating instance.");
            instance = new AchievementService();
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
            this.obtainedAchievements.Add(achievementID);
        }
    }

    public async Task CheckForAchievementsAsync(Snackbar snackbar, DataLoader dataLoader, DatabaseService databaseManagerInstance, Settings s)
    {
        var newAchievements = this.GetNewlyObtainedAchievements(dataLoader, databaseManagerInstance, s);

        if (newAchievements.Count > 0)
        {
            this.UpdatePlayerAchievements(newAchievements);
            await ShowMany(snackbar, newAchievements);
            LoggerInstance.Info(CultureInfo.InvariantCulture, "Awarded achievements: {0}", JsonConvert.SerializeObject(newAchievements));
        }
        else
        {
            LoggerInstance.Info(CultureInfo.InvariantCulture, "No new achievements found");
        }
    }

    public async Task RewardAchievement(int achievementID, Snackbar snackbar)
    {
        Achievements.IDAchievement.TryGetValue(achievementID, out var achievement);
        if (achievement == null)
        {
            return;
        }

        if (!this.obtainedAchievements.Contains(achievementID))
        {
            this.obtainedAchievements.Add(achievementID);

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

    private static readonly DatabaseService DatabaseManagerInstance = DatabaseService.GetInstance();
    private static readonly NLog.Logger LoggerInstance = NLog.LogManager.GetCurrentClassLogger();
    private static AchievementService? instance;
    private HashSet<int> obtainedAchievements = new ();

    private AchievementService()
    {
        LoggerInstance.Info($"Service initialized");
    }

    private List<int> GetNewlyObtainedAchievements(DataLoader dataLoader, DatabaseService databaseManagerInstance, Settings s)
    {
        var newAchievements = new List<int>();

        foreach (var kvp in Achievements.IDAchievement)
        {
            var achievementID = kvp.Key;
            var achievement = kvp.Value;

            // Check the specific conditions for obtaining the achievement
            if (!this.obtainedAchievements.Contains(achievementID) && this.CheckConditionsForAchievement(achievementID, dataLoader, databaseManagerInstance, s))
            {
                newAchievements.Add(achievementID);
            }
        }

        return newAchievements;
    }

    private bool CheckConditionsForAchievement(int achievementID, DataLoader dataLoader, DatabaseService databaseManagerInstance, Settings s)
    {
        // Implement your logic here to check the conditions for obtaining the achievement
        // Return true if the conditions are met, false otherwise
        // You can access the properties of the achievement object to perform the checks
        switch (achievementID)
        {
            default:
            {
                LoggerInstance.Error("Achievement ID {0} not found", achievementID);
                return false;
            }

            case 0:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4AkuraVashimu);
            case 1:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4AkuraVashimu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 2:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4AkuraVashimu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 3:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4AkuraVashimu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 4:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4AkuraVashimu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 5:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Anorupatisu);
            case 6:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Anorupatisu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 7:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Anorupatisu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 8:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Anorupatisu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 9:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Anorupatisu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 10:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Blangonga);
            case 11:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Blangonga) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 12:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Blangonga) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 13:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Blangonga) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 14:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Blangonga && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 15:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4DaimyoHermitaur);
            case 16:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4DaimyoHermitaur) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 17:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4DaimyoHermitaur) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 18:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4DaimyoHermitaur) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 19:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4DaimyoHermitaur && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 20:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Doragyurosu);
            case 21:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Doragyurosu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 22:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Doragyurosu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 23:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Doragyurosu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 24:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Doragyurosu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 25:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Espinas);
            case 26:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Espinas) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 27:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Espinas) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 28:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Espinas) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 29:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Espinas && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 30:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gasurabazura);
            case 31:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Gasurabazura) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 32:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Gasurabazura) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 33:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Gasurabazura) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 34:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gasurabazura && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 35:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Giaorugu);
            case 36:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Giaorugu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 37:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Giaorugu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 38:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Giaorugu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 39:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Giaorugu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 40:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hypnocatrice);
            case 41:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Hypnocatrice) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 42:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Hypnocatrice) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 43:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Hypnocatrice) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 44:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hypnocatrice && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 45:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hyujikiki);
            case 46:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Hyujikiki) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 47:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Hyujikiki) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 48:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Hyujikiki) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 49:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hyujikiki && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 50:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Inagami);
            case 51:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Inagami) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 52:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Inagami) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 53:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Inagami) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 54:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Inagami && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 55:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Khezu);
            case 56:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Khezu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 57:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Khezu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 58:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Khezu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 59:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Khezu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 60:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Midogaron);
            case 61:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Midogaron) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 62:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Midogaron) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 63:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Midogaron) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 64:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Midogaron && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 65:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDHugePlesioth);
            case 66:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDHugePlesioth) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 67:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDHugePlesioth) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 68:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDHugePlesioth) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 69:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDHugePlesioth && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 70:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rathalos);
            case 71:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Rathalos) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 72:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Rathalos) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 73:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Rathalos) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 74:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rathalos && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 75:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rukodiora);
            case 76:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Rukodiora) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 77:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Rukodiora) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 78:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Rukodiora) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 79:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rukodiora && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 80:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Tigrex);
            case 81:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Tigrex) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 82:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Tigrex) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 83:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Tigrex) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 84:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Tigrex && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 85:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Toridcless);
            case 86:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Toridcless) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 87:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Toridcless) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 88:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Toridcless) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 89:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Toridcless && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 90:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Baruragaru);
            case 91:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Baruragaru) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 92:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Baruragaru) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 93:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Baruragaru) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 94:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Baruragaru && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 95:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Bogabadorumu);
            case 96:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Bogabadorumu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 97:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Bogabadorumu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 98:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Bogabadorumu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 99:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Bogabadorumu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 100:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gravios);
            case 101:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Gravios) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 102:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Gravios) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 103:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Gravios) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 104:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gravios && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 105:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Harudomerugu);
            case 106:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Harudomerugu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 107:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Harudomerugu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 108:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4Harudomerugu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 109:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Harudomerugu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 110:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4TaikunZamuza);
            case 111:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4TaikunZamuza) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 112:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4TaikunZamuza) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 113:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDZ4TaikunZamuza) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 114:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4TaikunZamuza && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 115:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Fatalis);
            case 116:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999Fatalis) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 117:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999Fatalis) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 118:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999Fatalis) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 119:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Fatalis && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 120:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999CrimsonFatalis);
            case 121:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999CrimsonFatalis) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 122:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999CrimsonFatalis) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 123:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999CrimsonFatalis) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 124:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999CrimsonFatalis && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 125:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Shantien);
            case 126:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999Shantien) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 127:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999Shantien) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 128:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999Shantien) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 129:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Shantien && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 130:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Disufiroa);
            case 131:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999Disufiroa) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 132:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999Disufiroa) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 133:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDLV9999Disufiroa) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 134:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Disufiroa && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 135:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDUpperShitenUnknown);
            case 136:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDUpperShitenUnknown) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 137:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDUpperShitenUnknown) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 138:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDUpperShitenUnknown) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 139:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDUpperShitenUnknown && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 140:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDUpperShitenDisufiroa);
            case 141:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDUpperShitenDisufiroa) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 142:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDUpperShitenDisufiroa) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 143:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDUpperShitenDisufiroa) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 144:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDUpperShitenDisufiroa && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 145:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDThirstyPariapuria);
            case 146:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDThirstyPariapuria) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 147:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDThirstyPariapuria) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 148:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDThirstyPariapuria) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 149:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDThirstyPariapuria && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 150:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDRulingGuanzorumu);
            case 151:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDRulingGuanzorumu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 152:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDRulingGuanzorumu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 153:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDRulingGuanzorumu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 154:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDRulingGuanzorumu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 155:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDShiftingMiRu);
            case 156:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDShiftingMiRu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 157:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDShiftingMiRu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 158:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDShiftingMiRu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 159:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDShiftingMiRu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 160:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDBlinkingNargacugaForest || quest.QuestID == Numbers.QuestIDBlinkingNargacugaHistoric);
            case 161:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDBlinkingNargacugaForest || quest.QuestID == Numbers.QuestIDBlinkingNargacugaHistoric) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 162:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDBlinkingNargacugaForest || quest.QuestID == Numbers.QuestIDBlinkingNargacugaHistoric) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 163:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDBlinkingNargacugaForest || quest.QuestID == Numbers.QuestIDBlinkingNargacugaHistoric) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 164:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDBlinkingNargacugaForest || quest.QuestID == Numbers.QuestIDBlinkingNargacugaHistoric) && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 165:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDHowlingZinogreForest || quest.QuestID == Numbers.QuestIDHowlingZinogreHistoric);
            case 166:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDHowlingZinogreForest || quest.QuestID == Numbers.QuestIDHowlingZinogreHistoric) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 167:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDHowlingZinogreForest || quest.QuestID == Numbers.QuestIDHowlingZinogreHistoric) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 168:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDHowlingZinogreForest || quest.QuestID == Numbers.QuestIDHowlingZinogreHistoric) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 169:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDHowlingZinogreForest || quest.QuestID == Numbers.QuestIDHowlingZinogreHistoric) && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 170:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDStarvingDeviljhoArena || quest.QuestID == Numbers.QuestIDStarvingDeviljhoHistoric);
            case 171:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDStarvingDeviljhoArena || quest.QuestID == Numbers.QuestIDStarvingDeviljhoHistoric) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 172:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDStarvingDeviljhoArena || quest.QuestID == Numbers.QuestIDStarvingDeviljhoHistoric) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 173:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDStarvingDeviljhoArena || quest.QuestID == Numbers.QuestIDStarvingDeviljhoHistoric) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 174:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDStarvingDeviljhoArena || quest.QuestID == Numbers.QuestIDStarvingDeviljhoHistoric) && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 175:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDSparklingZerureusu || quest.QuestID == Numbers.QuestIDSparklingZerureusuEvent);
            case 176:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDSparklingZerureusu || quest.QuestID == Numbers.QuestIDSparklingZerureusuEvent) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 177:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDSparklingZerureusu || quest.QuestID == Numbers.QuestIDSparklingZerureusuEvent) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 178:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDSparklingZerureusu || quest.QuestID == Numbers.QuestIDSparklingZerureusuEvent) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 179:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDSparklingZerureusu || quest.QuestID == Numbers.QuestIDSparklingZerureusuEvent) && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 180:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDArrogantDuremudira);
            case 181:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDArrogantDuremudira) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 182:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDArrogantDuremudira) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 183:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDArrogantDuremudira) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 184:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDArrogantDuremudira && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 185:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDBlitzkriegBogabadorumu);
            case 186:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDBlitzkriegBogabadorumu) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 187:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDBlitzkriegBogabadorumu) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 188:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDBlitzkriegBogabadorumu) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 189:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDBlitzkriegBogabadorumu && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 190:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDBurningFreezingElzelionTower || quest.QuestID == Numbers.QuestIDBurningFreezingElzelionHistoric);
            case 191:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDBurningFreezingElzelionTower || quest.QuestID == Numbers.QuestIDBurningFreezingElzelionHistoric) >= Numbers.RequiredCompletionsMonsterSlayer)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 192:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDBurningFreezingElzelionTower || quest.QuestID == Numbers.QuestIDBurningFreezingElzelionHistoric) >= Numbers.RequiredCompletionsMonsterAnnihilator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 193:
                if (databaseManagerInstance.AllQuests.Count(quest => quest.QuestID == Numbers.QuestIDBurningFreezingElzelionTower || quest.QuestID == Numbers.QuestIDBurningFreezingElzelionHistoric) >= Numbers.RequiredCompletionsMonsterExterminator)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 194:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDBurningFreezingElzelionTower || quest.QuestID == Numbers.QuestIDBurningFreezingElzelionHistoric) && quest.PartySize == 1 && quest.ActualOverlayMode != null && (quest.ActualOverlayMode == "Zen" || quest.ActualOverlayMode == "Time Attack" || quest.ActualOverlayMode.Contains("Freestyle")));
            case 195:
                // Join quests and player inventories based on RunID
                var completedQuests = from quest in databaseManagerInstance.AllQuests
                                      join playerInventory in databaseManagerInstance.AllPlayerInventories on quest.RunID equals playerInventory.RunID
                                      where quest.QuestID == Numbers.QuestIDThirstyPariapuria &&
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
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 196:
                completedQuests = from quest in databaseManagerInstance.AllQuests
                                  join playerGear in databaseManagerInstance.AllPlayerGear on quest.RunID equals playerGear.RunID
                                  where quest.QuestID == Numbers.QuestIDRulingGuanzorumu && playerGear.StyleID != 3
                                  select quest;
                if (completedQuests != null && completedQuests.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 197:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDShiftingMiRu && quest.KeyStrokesDictionary != null && JsonConvert.DeserializeObject<Dictionary<int, string>>(quest.KeyStrokesDictionary).Values.First() == "LShiftKey");
            case 198:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDBlinkingNargacugaForest || quest.QuestID == Numbers.QuestIDBlinkingNargacugaHistoric) && quest.HitsTakenBlockedDictionary != null && JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(quest.HitsTakenBlockedDictionary).Count == 0);
            case 199:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDHowlingZinogreForest || quest.QuestID == Numbers.QuestIDHowlingZinogreHistoric) && quest.PartySize == 1 && quest.PlayerStaminaDictionary != null && JsonConvert.DeserializeObject<Dictionary<int, int>>(quest.PlayerStaminaDictionary).Values.First() <= 75);
            case 200:
                completedQuests = from quest in databaseManagerInstance.AllQuests
                                  join activeSkills in databaseManagerInstance.AllActiveSkills on quest.RunID equals activeSkills.RunID
                                  where (quest.QuestID == Numbers.QuestIDBurningFreezingElzelionHistoric || quest.QuestID == Numbers.QuestIDBurningFreezingElzelionTower) &&
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
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 201:
                completedQuests = from quest in databaseManagerInstance.AllQuests
                                  join playerInventory in databaseManagerInstance.AllPlayerInventories on quest.RunID equals playerInventory.RunID
                                  where quest.QuestID == Numbers.QuestIDBlitzkriegBogabadorumu &&
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
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 202:
                completedQuests = from quest in databaseManagerInstance.AllQuests
                                  join styleRankSkills in databaseManagerInstance.AllStyleRankSkills on quest.RunID equals styleRankSkills.RunID
                                  where quest.QuestID == Numbers.QuestIDArrogantDuremudira &&
                                        (styleRankSkills.StyleRankSkill1ID == 14 || styleRankSkills.StyleRankSkill2ID == 14)
                                  select quest;
                if (completedQuests != null && completedQuests.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 203:
                // Set the target date to February 14th of the current year
                var targetDate = new DateTime(DateTime.UtcNow.Year, 2, 14);

                if (databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDVeggieElderLove && quest.CreatedAt?.Date == targetDate.Date))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 204:
                completedQuests = from quest in databaseManagerInstance.AllQuests
                                  join playerGear in databaseManagerInstance.AllPlayerGear on quest.RunID equals playerGear.RunID
                                  where (quest.QuestID == Numbers.QuestIDProducerGogomoaHR || quest.QuestID == Numbers.QuestIDProducerGogomoaLR) &&
                                  playerGear.WeaponTypeID == 9
                                  select quest;
                if (completedQuests != null && completedQuests.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 205:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDFourHeavenlyKingMale1 || quest.QuestID == Numbers.QuestIDFourHeavenlyKingMale2 || quest.QuestID == Numbers.QuestIDFourHeavenlyKingFemale1 || quest.QuestID == Numbers.QuestIDFourHeavenlyKingFemale2);
            case 206:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDHatsuneMiku);
            case 207:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDPSO2);
            case 208:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDMegaman);
            case 209:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDHiganjima);
            case 210:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDHugePlesioth);
            case 211:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDSunglassesKutKu);
            case 212:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDMHFQ);
            case 213:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDCongalalaCure);
            case 214:
                if (dataLoader.model.GZenny() >= 9_999_999)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 215: // TODO test
                if (dataLoader.model.DivaBond() >= 999)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 216:// TODO Obtain S Rank in all single-player MezFes minigames
            {
                return false;
            }

            case 217:
                if (dataLoader.model.CaravanPoints() >= 9_999_999)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 218:
                if (dataLoader.model.RoadMaxStagesMultiplayer() >= 50)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 219:
                if (dataLoader.model.RoadMaxStagesMultiplayer() >= 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 220:
                if (dataLoader.model.PartnerLevel() >= 999)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 221:
                return databaseManagerInstance.AllQuestAttempts.Any(questAttempts => questAttempts.Attempts >= 1_000);
            case 222:
                return databaseManagerInstance.AllPersonalBestAttempts.Any(pbAttempts => pbAttempts.Attempts >= 100);
            case 223:
                if (dataLoader.model.SecondDistrictDuremudiraSlays() >= 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 224:
                if (dataLoader.model.RoadFatalisSlain() >= 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 225:// fumo
            {
                return false;
            }

            case 226:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDTwinheadRajangsHistoric);
            case 227:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4AkuraVashimu && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 228:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Anorupatisu && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 229:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Blangonga && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 230:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4DaimyoHermitaur && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 231:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Doragyurosu && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 232:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Espinas && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 233:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gasurabazura && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 234:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Giaorugu && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 235:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hypnocatrice && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 236:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hyujikiki && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 237:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Inagami && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 238:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Khezu && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 239:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Midogaron && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 240:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Plesioth && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 241:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rathalos && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 242:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rukodiora && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 243:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Tigrex && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 244:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Toridcless && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 245:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Baruragaru && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 246:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Bogabadorumu && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 247:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gravios && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 248:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Harudomerugu && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 249:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4TaikunZamuza && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 250:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Fatalis && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 251:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999CrimsonFatalis && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 252:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Shantien && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 253:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Disufiroa && quest.FinalTimeValue < Numbers.Frames1Minute * 10);
            case 254:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4AkuraVashimu && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 255:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Anorupatisu && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 256:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Blangonga && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 257:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4DaimyoHermitaur && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 258:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Doragyurosu && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 259:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Espinas && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 260:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gasurabazura && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 261:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Giaorugu && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 262:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hypnocatrice && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 263:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hyujikiki && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 264:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Inagami && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 265:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Khezu && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 266:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Midogaron && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 267:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Plesioth && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 268:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rathalos && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 269:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rukodiora && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 270:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Tigrex && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 271:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Toridcless && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 272:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Baruragaru && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 273:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Bogabadorumu && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 274:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gravios && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 275:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Harudomerugu && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 276:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4TaikunZamuza && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 277:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Fatalis && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 278:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999CrimsonFatalis && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 279:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Shantien && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 280:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Disufiroa && quest.FinalTimeValue < Numbers.Frames1Minute * 8);
            case 281:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4AkuraVashimu && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 282:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Anorupatisu && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 283:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Blangonga && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 284:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4DaimyoHermitaur && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 285:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Doragyurosu && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 286:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Espinas && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 287:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gasurabazura && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 288:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Giaorugu && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 289:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hypnocatrice && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 290:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Hyujikiki && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 291:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Inagami && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 292:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Khezu && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 293:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Midogaron && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 294:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Plesioth && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 295:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rathalos && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 296:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Rukodiora && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 297:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Tigrex && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 298:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Toridcless && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 299:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Baruragaru && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 300:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Bogabadorumu && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 301:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Gravios && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 302:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4Harudomerugu && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 303:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDZ4TaikunZamuza && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 304:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Fatalis && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 305:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999CrimsonFatalis && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 306:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Shantien && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 307:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDLV9999Disufiroa && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 308:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDThirstyPariapuria && quest.FinalTimeValue < Numbers.Frames1Minute * 3);
            case 309:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDShiftingMiRu && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 310:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDRulingGuanzorumu && quest.FinalTimeValue < Numbers.Frames1Minute * 5);
            case 311:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDBlinkingNargacugaForest || quest.QuestID == Numbers.QuestIDBlinkingNargacugaHistoric) && quest.FinalTimeValue < Numbers.Frames1Minute * 7);
            case 312:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDHowlingZinogreForest || quest.QuestID == Numbers.QuestIDHowlingZinogreHistoric) && quest.FinalTimeValue < Numbers.Frames1Minute * 7);
            case 313:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDSparklingZerureusu || quest.QuestID == Numbers.QuestIDSparklingZerureusuEvent) && quest.FinalTimeValue < Numbers.Frames1Minute * 9);
            case 314:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDStarvingDeviljhoArena || quest.QuestID == Numbers.QuestIDStarvingDeviljhoHistoric) && quest.FinalTimeValue < Numbers.Frames1Minute * 9);
            case 315:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDArrogantDuremudira && quest.FinalTimeValue < Numbers.Frames1Minute * 9);
            case 316:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDBlitzkriegBogabadorumu && quest.FinalTimeValue < Numbers.Frames1Minute * 9);
            case 317:
                return databaseManagerInstance.AllQuests.Any(quest => (quest.QuestID == Numbers.QuestIDBurningFreezingElzelionHistoric || quest.QuestID == Numbers.QuestIDBurningFreezingElzelionTower) && quest.FinalTimeValue < Numbers.Frames1Minute * 9);
            case 318:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDUpperShitenUnknown && quest.FinalTimeValue < Numbers.Frames1Minute * 9);
            case 319:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDUpperShitenDisufiroa && quest.FinalTimeValue < Numbers.Frames1Minute * 9);
            case 320:
                return databaseManagerInstance.AllBingo.Any(bingo => bingo.Difficulty == Difficulty.Easy);
            case 321:
                return databaseManagerInstance.AllBingo.Any(bingo => bingo.Difficulty == Difficulty.Medium);
            case 322:
                return databaseManagerInstance.AllBingo.Any(bingo => bingo.Difficulty == Difficulty.Hard);
            case 323:
                return databaseManagerInstance.AllBingo.Any(bingo => bingo.Difficulty == Difficulty.Extreme);
            case 324:
                if (databaseManagerInstance.AllGachaCards.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 325:
                if (databaseManagerInstance.AllGachaCards.Count >= 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 326:
                if (databaseManagerInstance.AllGachaCards.Count >= 1000)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 327: // TODO obtain all gacha cards
            {
                return false;
            }

            case 328:
                if (databaseManagerInstance.AllZenithGauntlets.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 329:
                if (databaseManagerInstance.AllZenithGauntlets.Count >= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 330:
                if (databaseManagerInstance.AllZenithGauntlets.Count >= 25)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 331:
                if (databaseManagerInstance.AllZenithGauntlets.Count == 0)
                {
                    return false;
                }

                return databaseManagerInstance.AllZenithGauntlets.Any(gauntlet =>
                {
                    if (TimeSpan.TryParse(gauntlet.TotalTimeElapsed, out var timeElapsed))
                    {
                        return timeElapsed < TimeSpan.FromHours(4);
                    }

                    return false; // Handle invalid TotalTimeElapsed values
                });
            case 332:
                if (databaseManagerInstance.AllSolsticeGauntlets.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 333:
                if (databaseManagerInstance.AllSolsticeGauntlets.Count >= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 334:
                if (databaseManagerInstance.AllSolsticeGauntlets.Count >= 25)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 335:
                if (databaseManagerInstance.AllSolsticeGauntlets.Count == 0)
                {
                    return false;
                }

                return databaseManagerInstance.AllSolsticeGauntlets.Any(gauntlet =>
                {
                    if (TimeSpan.TryParse(gauntlet.TotalTimeElapsed, out var timeElapsed))
                    {
                        return timeElapsed < TimeSpan.FromHours(1);
                    }

                    return false; // Handle invalid TotalTimeElapsed values
                });
            case 336:
                if (databaseManagerInstance.AllMusouGauntlets.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 337:
                if (databaseManagerInstance.AllMusouGauntlets.Count >= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 338:
                if (databaseManagerInstance.AllMusouGauntlets.Count >= 25)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 339:
                if (databaseManagerInstance.AllMusouGauntlets.Count == 0)
                {
                    return false;
                }

                return databaseManagerInstance.AllMusouGauntlets.Any(gauntlet =>
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
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 342:
                if (dataLoader.model.GetOverlayMode().Contains("Freestyle"))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 343:
                if (databaseManagerInstance.AllPlayerGear.Count(playerGear => playerGear.GuildFoodID != 0) >= 50)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 344:
                if (databaseManagerInstance.AllPlayerGear.Count(playerGear => playerGear.DivaSkillID != 0) >= 50)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 345:// TODO gallery
            {
                return false;
            }

            case 346:
                if (dataLoader.model.CalculateTotalLargeMonstersHunted() >= 1000)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 347:
                if (databaseManagerInstance.GetTotalQuestTimeElapsed() >= Numbers.Frames1Hour * 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 348: // TODO idk if i should check by name
                EZlion.Mapper.WeaponBlademaster.IDName.TryGetValue(dataLoader.model.BlademasterWeaponID(), out var blademasterWeaponName);
                EZlion.Mapper.WeaponGunner.IDName.TryGetValue(dataLoader.model.GunnerWeaponID(), out var gunnerWeaponName);

                if (dataLoader.model.GRWeaponLv() == 100 && (

                        blademasterWeaponName != null && (blademasterWeaponName.Contains("\"Shine\"") || blademasterWeaponName.Contains("\"Clear\"") || blademasterWeaponName.Contains("\"Flash\"") || blademasterWeaponName.Contains("\"Glory\""))
                     ||

                        gunnerWeaponName != null && (gunnerWeaponName.Contains("\"Shine\"") || gunnerWeaponName.Contains("\"Clear\"") || gunnerWeaponName.Contains("\"Flash\"") || gunnerWeaponName.Contains("\"Glory\""))

                ))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 349:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDMosswineRevenge);
            case 350:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDJunglePuzzle);
            case 351:
                if (databaseManagerInstance.AllPlayerGear.Count(playerGear => playerGear.PoogieItemID != 0) >= 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 352:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDNuclearGypceros);
            case 353:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDMosswineDuel);
            case 354:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDMosswineLastStand);
            case 355:
                return databaseManagerInstance.AllQuests.Any(quest => quest.QuestID == Numbers.QuestIDHalloweenSpeedster);
            case 356:// TODO 1000 bingo points in 1 go
            {
                return false;
            }

            case 357:
                if (databaseManagerInstance.AllBingo.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 358:
                if (databaseManagerInstance.AllBingo.Count >= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 359:
                if (databaseManagerInstance.AllBingo.Count >= 25)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case 360:
                if (databaseManagerInstance.AllBingo.Count >= 50)
                {
                    return true;
                }
                else
                {
                    return false;
                }

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
            case 401:
            case 402:
            case 403:
            {
                return false;
            }
        }
    }

    private void UpdatePlayerAchievements(List<int> achievementsID)
    {
        // Update the player achievements table in the database with the newly obtained achievements
        // Use the provided database update logic or similar approach
        foreach (var achievementID in achievementsID)
        {
            this.obtainedAchievements.Add(achievementID);

            // Store the achievement in the SQLite PlayerAchievements table
            DatabaseManagerInstance.StoreAchievement(achievementID);
        }
    }
}
