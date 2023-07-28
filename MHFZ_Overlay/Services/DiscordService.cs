// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Diagnostics;
using System.Globalization;
using DiscordRPC;
using EZlion.Mapper;
using MHFZ_Overlay;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Constant;

/// <summary>
/// Handles the Discord Rich Presence. Should not operate if the user is not enabling it.
/// </summary>
public sealed class DiscordService
{
    public static DiscordService GetInstance()
    {
        if (instance == null)
        {
            LoggerInstance.Debug("Singleton not found, creating instance.");
            instance = new DiscordService();
        }

        LoggerInstance.Debug("Singleton found, returning instance.");
        LoggerInstance.Trace(new StackTrace().ToString());
        return instance;
    }

    /// <summary>
    /// Initializes the discord RPC.
    /// </summary>
    public static void InitializeDiscordRPC()
    {
        if (isDiscordRPCRunning)
        {
            return;
        }

        if (ShowDiscordRPC && GetDiscordClientID != string.Empty)
        {
            SetupDiscordRPC();

            // Set Presence
            presenceTemplate.Timestamps = Timestamps.Now;

            if (GetHunterName != string.Empty && GetGuildName != string.Empty && GetServerName != string.Empty)
            {
                presenceTemplate.Assets = new Assets()
                {
                    LargeImageKey = "cattleya",
                    LargeImageText = "Please Wait",
                    SmallImageKey = "https://i.imgur.com/9OkLYAz.png",
                    SmallImageText = GetHunterName + " | " + GetGuildName + " | " + GetServerName,
                };
            }

            // should work fine
            presenceTemplate.Buttons = new Button[] { };
            presenceTemplate.Buttons = new Button[]
            {
                new Button()
                {
                    Label = "【MHF-Z】Overlay "+ App.CurrentProgramVersion,
                    Url = "https://github.com/DorielRivalet/mhfz-overlay",
                },
                new Button()
                {
                    Label = "Discord RPC C# Dev Site",
                    Url = "https://lachee.dev/",
                },
            };

            if (GetDiscordServerInvite != string.Empty)
            {
                presenceTemplate.Buttons = new Button[]
                {
                    // nothing
                };
                presenceTemplate.Buttons = new Button[]
                {
                    new Button()
                    {
                        Label = "【MHF-Z】Overlay "+ App.CurrentProgramVersion,
                        Url = "https://github.com/DorielRivalet/mhfz-overlay",
                    },
                    new Button()
                    {
                        Label = "Join Discord Server",
                        Url = string.Format(CultureInfo.InvariantCulture, "https://discord.com/invite/{0}",GetDiscordServerInvite),
                    }
                };
            }

            if (discordRPCClient == null)
            {
                return;
            }

            discordRPCClient.SetPresence(presenceTemplate);
            isDiscordRPCRunning = true;
            LoggerInstance.Info(CultureInfo.InvariantCulture, "Discord RPC is now running");
        }
    }

    /// <summary>
    /// Cleanups/disposes the Discord RPC instance.
    /// </summary>
    public static void DiscordRPCCleanup()
    {
        if (discordRPCClient != null) //&& ShowDiscordRPC)
        {
            discordRPCClient.Dispose();
            LoggerInstance.Info(CultureInfo.InvariantCulture, "Disposed Discord RPC");
        }
    }

    /// <summary>
    /// Gets a value indicating whether [show discord quest names].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [show discord quest names]; otherwise, <c>false</c>.
    /// </value>
    public static bool ShowDiscordQuestNames()
    {
        var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
        return s.DiscordQuestNameShown;
    }

    /// <summary>
    /// Updates the discord RPC.
    /// </summary>
    public void UpdateDiscordRPC(DataLoader dataLoader)
    {
        if (!isDiscordRPCRunning)
        {
            return;
        }

        // TODO also need to handle the other fields lengths
        if (string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}", this.GetPartySize(dataLoader), GetQuestState(dataLoader), this.GetCaravanScore(dataLoader), dataLoader.model.GetOverlayModeForRPC(), dataLoader.model.GetAreaName(dataLoader.model.AreaID()), GetGameMode(dataLoader.isHighGradeEdition)).Length >= 95)
        {
            presenceTemplate.Details = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", GetQuestState(dataLoader), dataLoader.model.GetOverlayModeForRPC(), dataLoader.model.GetAreaName(dataLoader.model.AreaID()));
        }
        else
        {
            presenceTemplate.Details = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}", this.GetPartySize(dataLoader), GetQuestState(dataLoader), this.GetCaravanScore(dataLoader), dataLoader.model.GetOverlayModeForRPC(), dataLoader.model.GetAreaName(dataLoader.model.AreaID()), GetGameMode(dataLoader.isHighGradeEdition));
        }

        var stateString = string.Empty;
        var largeImageTextString = string.Empty;
        var smallImageTextString = string.Empty;

        // Info
        if (dataLoader.model.QuestID() != 0 && dataLoader.model.TimeDefInt() != dataLoader.model.TimeInt() && int.Parse(dataLoader.model.ATK, CultureInfo.InvariantCulture) > 0 || (dataLoader.model.QuestID() == 21731 || dataLoader.model.QuestID() == 21746 || dataLoader.model.QuestID() == 21749 || dataLoader.model.QuestID() == 21750 || dataLoader.model.QuestID() == 23648 || dataLoader.model.QuestID() == 23649 || dataLoader.model.QuestID() == 21748 || dataLoader.model.QuestID() == 21747 || dataLoader.model.QuestID() == 21734) && int.Parse(dataLoader.model.ATK, CultureInfo.InvariantCulture) > 0)
        {
            switch (dataLoader.model.QuestID())
            {
                case 23527: // Hunter's Road Multiplayer
                    stateString = string.Format(CultureInfo.InvariantCulture, "Multiplayer Floor: {0} ({1}/{2} Max/Total) | RP: {3} | White Fatalis: {4}/{5} (Slain/Encounters)", dataLoader.model.RoadFloor() + 1, dataLoader.model.RoadMaxStagesMultiplayer(), dataLoader.model.RoadTotalStagesMultiplayer(), dataLoader.model.RoadPoints(), dataLoader.model.RoadFatalisSlain(), dataLoader.model.RoadFatalisEncounters());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 23628: // solo road
                    stateString = string.Format(CultureInfo.InvariantCulture, "Solo Floor: {0} ({1}/{2} Max/Total) | RP: {3} | White Fatalis: {4}/{5} (Slain/Encounters)", dataLoader.model.RoadFloor() + 1, dataLoader.model.RoadMaxStagesSolo(), dataLoader.model.RoadTotalStagesSolo(), dataLoader.model.RoadPoints(), dataLoader.model.RoadFatalisSlain(), dataLoader.model.RoadFatalisEncounters());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 21731: // 1st district dure
                case 21749: // sky corridor version
                    stateString = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5} | Slain: {6} | Encounters: {7}", dataLoader.model.GetQuestNameFromID(dataLoader.model.QuestID()), dataLoader.model.GetObjectiveNameFromID(dataLoader.model.ObjectiveType()), string.Empty, dataLoader.model.GetObjective1Quantity(), dataLoader.model.GetRankNameFromID(dataLoader.model.RankBand()), dataLoader.model.GetRealMonsterName(), dataLoader.model.FirstDistrictDuremudiraSlays(), dataLoader.model.FirstDistrictDuremudiraEncounters());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 21746: // 2nd district dure
                case 21750: // sky corridor version
                    stateString = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5} | Slain: {6} | Encounters: {7}", dataLoader.model.GetQuestNameFromID(dataLoader.model.QuestID()), dataLoader.model.GetObjectiveNameFromID(dataLoader.model.ObjectiveType()), string.Empty, dataLoader.model.GetObjective1Quantity(), dataLoader.model.GetRankNameFromID(dataLoader.model.RankBand()), dataLoader.model.GetRealMonsterName(), dataLoader.model.SecondDistrictDuremudiraSlays(), dataLoader.model.SecondDistrictDuremudiraEncounters());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 62105: // raviente quests
                case 62108:
                case 62101:
                case 62104:
                case 55796:
                case 55802:
                case 55803:
                case 55804:
                case 55805:
                case 55806:
                case 55807:
                case 54751:
                case 54756:
                case 54757:
                case 54758:
                case 54759:
                case 54760:
                case 54761:
                case 55596: // extreme
                case 55602:
                case 55603:
                case 55604:
                case 55605:
                case 55606:
                case 55607:
                    stateString = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}{6} | True Raw: {7} (Max {8}) | Hits: {9}", dataLoader.model.GetQuestNameFromID(dataLoader.model.QuestID()), dataLoader.model.GetObjectiveNameFromID(dataLoader.model.ObjectiveType()), string.Empty, dataLoader.model.GetObjective1Quantity(), dataLoader.model.GetRankNameFromID(dataLoader.model.RankBand()), dataLoader.model.GetStarGrade(), dataLoader.model.GetRealMonsterName(), dataLoader.model.ATK, dataLoader.model.HighestAtk, dataLoader.model.HitCountInt());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");

                    break;
                default:
                    if ((dataLoader.model.ObjectiveType() == 0x0 || dataLoader.model.ObjectiveType() == 0x02 || dataLoader.model.ObjectiveType() == 0x1002 || dataLoader.model.ObjectiveType() == 0x10) && dataLoader.model.QuestID() != 23527 && dataLoader.model.QuestID() != 23628 && dataLoader.model.QuestID() != 21731 && dataLoader.model.QuestID() != 21749 && dataLoader.model.QuestID() != 21746 && dataLoader.model.QuestID() != 21750)
                    {
                        stateString = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}{6} | True Raw: {7} (Max {8}) | Hits: {9}", dataLoader.model.GetQuestNameFromID(dataLoader.model.QuestID()), dataLoader.model.GetObjectiveNameFromID(dataLoader.model.ObjectiveType()), dataLoader.model.GetObjective1CurrentQuantity(), dataLoader.model.GetObjective1Quantity(), dataLoader.model.GetRankNameFromID(dataLoader.model.RankBand()), dataLoader.model.GetStarGrade(), dataLoader.model.GetObjective1Name(dataLoader.model.Objective1ID()), dataLoader.model.ATK, dataLoader.model.HighestAtk, dataLoader.model.HitCountInt());
                        presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    }
                    else
                    {
                        stateString = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}{6} | True Raw: {7} (Max {8}) | Hits: {9}", dataLoader.model.GetQuestNameFromID(dataLoader.model.QuestID()), dataLoader.model.GetObjectiveNameFromID(dataLoader.model.ObjectiveType()), string.Empty, dataLoader.model.GetObjective1Quantity(), dataLoader.model.GetRankNameFromID(dataLoader.model.RankBand()), dataLoader.model.GetStarGrade(), dataLoader.model.GetRealMonsterName(), dataLoader.model.ATK, dataLoader.model.HighestAtk, dataLoader.model.HitCountInt());
                        presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    }

                    break;
            }

            // Gathering/etc
            if ((dataLoader.model.ObjectiveType() == 0x0 || dataLoader.model.ObjectiveType() == 0x02 || dataLoader.model.ObjectiveType() == 0x1002) && dataLoader.model.QuestID() != 23527 && dataLoader.model.QuestID() != 23628 && dataLoader.model.QuestID() != 21731 && dataLoader.model.QuestID() != 21749 && dataLoader.model.QuestID() != 21746 && dataLoader.model.QuestID() != 21750)
            {
                largeImageTextString = string.Format(CultureInfo.InvariantCulture, "{0}{1}", this.GetQuestInformation(dataLoader), dataLoader.model.GetAreaName(dataLoader.model.AreaID()));
                presenceTemplate.Assets.LargeImageKey = dataLoader.model.GetAreaIconFromID(dataLoader.model.AreaID());
                presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MaxDiscordRPCStringLength ? largeImageTextString : largeImageTextString.Substring(0, MaxDiscordRPCStringLength - 3) + "...";
            }

            // Tenrou Sky Corridor areas
            else if (dataLoader.model.AreaID() == 391 || dataLoader.model.AreaID() == 392 || dataLoader.model.AreaID() == 394 || dataLoader.model.AreaID() == 415 || dataLoader.model.AreaID() == 416)
            {
                largeImageTextString = string.Format(CultureInfo.InvariantCulture, "{0}{1}", this.GetQuestInformation(dataLoader), dataLoader.model.GetAreaName(dataLoader.model.AreaID()));
                presenceTemplate.Assets.LargeImageKey = dataLoader.model.GetAreaIconFromID(dataLoader.model.AreaID());
                presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MaxDiscordRPCStringLength ? largeImageTextString : largeImageTextString.Substring(0, MaxDiscordRPCStringLength - 3) + "...";
            }

            // Duremudira Doors
            else if (dataLoader.model.AreaID() == 399 || dataLoader.model.AreaID() == 414)
            {
                presenceTemplate.Assets.LargeImageKey = dataLoader.model.GetAreaIconFromID(dataLoader.model.AreaID());
                largeImageTextString = string.Format(CultureInfo.InvariantCulture, "{0}{1}", this.GetQuestInformation(dataLoader), dataLoader.model.GetAreaName(dataLoader.model.AreaID()));
                presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MaxDiscordRPCStringLength ? largeImageTextString : largeImageTextString.Substring(0, MaxDiscordRPCStringLength - 3) + "...";
            }

            // Duremudira Arena
            else if (dataLoader.model.AreaID() == 398)
            {
                presenceTemplate.Assets.LargeImageKey = dataLoader.model.GetMonsterIcon(dataLoader.model.LargeMonster1ID(), true);
                largeImageTextString = string.Format(CultureInfo.InvariantCulture, "{0}{1}/{2}{3}", this.GetQuestInformation(dataLoader), dataLoader.model.GetMonster1EHP(), dataLoader.model.GetMonster1MaxEHP(), dataLoader.model.GetMonster1EHPPercent());
                presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MaxDiscordRPCStringLength ? largeImageTextString : largeImageTextString.Substring(0, MaxDiscordRPCStringLength - 3) + "...";
            }

            // Hunter's Road Base Camp
            else if (dataLoader.model.AreaID() == 459)
            {
                presenceTemplate.Assets.LargeImageKey = dataLoader.model.GetAreaIconFromID(dataLoader.model.AreaID());
                largeImageTextString = string.Format(CultureInfo.InvariantCulture, "{0}{1} | Faints: {2}/{3}", this.GetQuestInformation(dataLoader), dataLoader.model.GetAreaName(dataLoader.model.AreaID()), dataLoader.model.CurrentFaints(), dataLoader.model.GetMaxFaints());
                presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MaxDiscordRPCStringLength ? largeImageTextString : largeImageTextString.Substring(0, MaxDiscordRPCStringLength - 3) + "...";
            }

            // Raviente
            else if (dataLoader.model.AreaID() == 309 || dataLoader.model.AreaID() >= 311 && dataLoader.model.AreaID() <= 321 || dataLoader.model.AreaID() >= 417 && dataLoader.model.AreaID() <= 422 || dataLoader.model.AreaID() == 437 || dataLoader.model.AreaID() >= 440 && dataLoader.model.AreaID() <= 444)
            {
                presenceTemplate.Assets.LargeImageKey = dataLoader.model.GetMonsterIcon(dataLoader.model.LargeMonster1ID(), true);
                largeImageTextString = string.Format(CultureInfo.InvariantCulture, "{0}{1}/{2}{3} | Faints: {4}/{5} | Points: {6} | {7}", this.GetQuestInformation(dataLoader), dataLoader.model.GetMonster1EHP(), dataLoader.model.GetMonster1MaxEHP(), dataLoader.model.GetMonster1EHPPercent(), dataLoader.model.CurrentFaints(), dataLoader.model.GetMaxFaints(), dataLoader.model.GreatSlayingPoints(), GetRavienteEvent(dataLoader.model.RavienteTriggeredEvent(), dataLoader));
                presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MaxDiscordRPCStringLength ? largeImageTextString : largeImageTextString.Substring(0, MaxDiscordRPCStringLength - 3) + "...";
            }
            else
            {
                presenceTemplate.Assets.LargeImageKey = dataLoader.model.GetMonsterIcon(dataLoader.model.LargeMonster1ID(), true);
                largeImageTextString = string.Format(CultureInfo.InvariantCulture, "{0}{1}/{2}{3} | Faints: {4}/{5}", this.GetQuestInformation(dataLoader), dataLoader.model.GetMonster1EHP(), dataLoader.model.GetMonster1MaxEHP(), dataLoader.model.GetMonster1EHPPercent(), dataLoader.model.CurrentFaints(), dataLoader.model.GetMaxFaints());
                presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MaxDiscordRPCStringLength ? largeImageTextString : largeImageTextString.Substring(0, MaxDiscordRPCStringLength - 3) + "...";
            }
        }
        else if (dataLoader.model.QuestID() == 0)
        {
            switch (dataLoader.model.AreaID())
            {
                case 0: // Loading
                    presenceTemplate.State = "Loading...";
                    break;
                case 87: // Kokoto Village
                case 131: // Dundorma areas
                case 132:
                case 133:
                case 134:
                case 135:
                case 136:
                case 200: // Mezeporta
                case 201: // Hairdresser
                case 206: // Old Town Areas
                case 207:
                case 210: // Private Bar
                case 211: // Rasta Bar
                case 244: // Code Claiming Room
                case 282: // Cities Map
                case 340: // SR Rooms
                case 341:
                case 397: // Mezeporta Dupe(non-HD)
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | GCP: {1} | Guild Food: {2} | Diva Skill: {3} ({4} Left) | Poogie Item: {5}", dataLoader.model.GRankNumber(), dataLoader.model.GCP(), dataLoader.model.GetArmorSkillWithNull(dataLoader.model.GuildFoodSkill()), dataLoader.model.GetDivaSkillNameFromID(dataLoader.model.DivaSkill()), dataLoader.model.DivaSkillUsesLeft(), dataLoader.model.GetItemName(dataLoader.model.PoogieItemUseID()));
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 173: // My House (original)
                case 175: // My House (MAX)
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | Partner Lv: {1} | Armor Color: {2} | GCP: {3}", dataLoader.model.GRankNumber(), dataLoader.model.PartnerLevel(), dataLoader.model.GetArmorColor(), dataLoader.model.GCP());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 202: // Guild Halls
                case 203:
                case 204:
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", dataLoader.model.GRankNumber(), dataLoader.model.GCP(), dataLoader.model.GetArmorSkill(dataLoader.model.GuildFoodSkill()), dataLoader.model.GetItemName(dataLoader.model.PoogieItemUseID()));
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 205: // Pugi Farm
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | Poogie Points: {1} | Poogie Clothes: {2} | Poogie Item: {3}", dataLoader.model.GRankNumber(), dataLoader.model.PoogiePoints(), dataLoader.model.GetPoogieClothes(dataLoader.model.PoogieCostume()), dataLoader.model.GetItemName(dataLoader.model.PoogieItemUseID()));
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 256: // Caravan Areas
                case 260:
                case 261:
                case 262:
                case 263:
                    stateString = string.Format(CultureInfo.InvariantCulture, "CP: {0} | Gg: {1} | g: {2} | Gem Lv: {3} | Great Slaying Points: {4}", dataLoader.model.CaravanPoints(), dataLoader.model.RaviGg(), dataLoader.model.Ravig(), dataLoader.model.CaravenGemLevel() + 1, dataLoader.model.GreatSlayingPointsSaved());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 257: // Blacksmith
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | GCP: {1} | Guild Food: {2} | GZenny: {3}", dataLoader.model.GRankNumber(), dataLoader.model.GCP(), dataLoader.model.GetArmorSkill(dataLoader.model.GuildFoodSkill()), dataLoader.model.GZenny());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 264: // Gallery
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | GCP: {1} | Guild Food: {2} | Score: {3}", dataLoader.model.GRankNumber(), dataLoader.model.GCP(), dataLoader.model.GetArmorSkill(dataLoader.model.GuildFoodSkill()), dataLoader.model.GalleryEvaluationScore());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 265: // Guuku Farm
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", dataLoader.model.GRankNumber(), dataLoader.model.GCP(), dataLoader.model.GetArmorSkill(dataLoader.model.GuildFoodSkill()), dataLoader.model.GetItemName(dataLoader.model.PoogieItemUseID()));
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 283: // Halk Area TODO partnya lv
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | GCP: {1} | PNRP: {2} | Halk Fullness: {3}", dataLoader.model.GRankNumber(), dataLoader.model.GCP(), dataLoader.model.PartnyaRankPoints(), dataLoader.model.HalkFullness());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 286: // PvP Room
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", dataLoader.model.GRankNumber(), dataLoader.model.GCP(), dataLoader.model.GetArmorSkill(dataLoader.model.GuildFoodSkill()), dataLoader.model.GetItemName(dataLoader.model.PoogieItemUseID()));
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 379: // Diva Hall
                case 445: // Guild Hall (Diva Event)
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | Diva Skill: {1} ({2} Left) | Diva Bond: {3} | Items Given: {4}", dataLoader.model.GRankNumber(), dataLoader.model.GetDivaSkillNameFromID(dataLoader.model.DivaSkill()), dataLoader.model.DivaSkillUsesLeft(), dataLoader.model.DivaBond(), dataLoader.model.DivaItemsGiven());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 462: // MezFez Entrance
                case 463: // Volpkun Together
                case 465: // MezFez Minigame
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | MezFes Points: {1} | Guild Food: {2} | Poogie Item: {3}", dataLoader.model.GRankNumber(), dataLoader.model.MezeportaFestivalPoints(), dataLoader.model.GetArmorSkill(dataLoader.model.GuildFoodSkill()), dataLoader.model.GetItemName(dataLoader.model.PoogieItemUseID()));
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 464: // Uruki Pachinko
                    stateString = string.Format(CultureInfo.InvariantCulture, "Score: {0} | Chain: {1} | Fish: {2} | Mushroom: {3} | Seed: {4} | Meat: {5}", dataLoader.model.UrukiPachinkoScore() + dataLoader.model.UrukiPachinkoBonusScore(), dataLoader.model.UrukiPachinkoChain(), dataLoader.model.UrukiPachinkoFish(), dataLoader.model.UrukiPachinkoMushroom(), dataLoader.model.UrukiPachinkoSeed(), dataLoader.model.UrukiPachinkoMeat());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 466: // Guuku Scoop
                    stateString = string.Format(CultureInfo.InvariantCulture, "Score: {0} | Small Guuku: {1} | Medium Guuku: {2} | Large Guuku: {3} | Golden Guuku: {4}", dataLoader.model.GuukuScoopScore(), dataLoader.model.GuukuScoopSmall(), dataLoader.model.GuukuScoopMedium(), dataLoader.model.GuukuScoopLarge(), dataLoader.model.GuukuScoopGolden());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 467: // Nyanrendo
                    stateString = string.Format(CultureInfo.InvariantCulture, "Score: {0}", dataLoader.model.NyanrendoScore());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 468: // Panic Honey
                    stateString = string.Format(CultureInfo.InvariantCulture, "Honey: {0}", dataLoader.model.PanicHoneyScore());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                case 469: // Dokkan Battle Cats
                    stateString = string.Format(CultureInfo.InvariantCulture, "Score: {0} | Scale: {1} | Shell: {2} | Camp: {3}", dataLoader.model.DokkanBattleCatsScore(), dataLoader.model.DokkanBattleCatsScale(), dataLoader.model.DokkanBattleCatsShell(), dataLoader.model.DokkanBattleCatsCamp());
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
                default: // same as Mezeporta
                    stateString = string.Format(CultureInfo.InvariantCulture, "GR: {0} | GCP: {1} | Guild Food: {2} | Poogie Item: {3}", dataLoader.model.GRankNumber(), dataLoader.model.GCP(), dataLoader.model.GetArmorSkill(dataLoader.model.GuildFoodSkill()), dataLoader.model.GetItemName(dataLoader.model.PoogieItemUseID()));
                    presenceTemplate.State = stateString.Length <= MaxDiscordRPCStringLength ? stateString : string.Concat(stateString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
                    break;
            }

            presenceTemplate.Assets.LargeImageKey = dataLoader.model.GetAreaIconFromID(dataLoader.model.AreaID());
            largeImageTextString = dataLoader.model.GetAreaName(dataLoader.model.AreaID());
            presenceTemplate.Assets.LargeImageText = largeImageTextString.Length <= MaxDiscordRPCStringLength ? largeImageTextString : string.Concat(largeImageTextString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
        }

        // Timer
        if (dataLoader.model.QuestID() != 0 && !this.inQuest && dataLoader.model.TimeDefInt() > dataLoader.model.TimeInt() && int.Parse(dataLoader.model.ATK, CultureInfo.InvariantCulture) > 0 || dataLoader.model.IsRoad() || dataLoader.model.IsDure())
        {
            this.inQuest = true;

            if (!(dataLoader.model.IsRoad() || dataLoader.model.IsDure()))
            {
                presenceTemplate.Timestamps = GetDiscordTimerMode() switch
                {
                    "Time Left" => Timestamps.FromTimeSpan(dataLoader.model.TimeDefInt() / double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)),
                    "Time Elapsed" => Timestamps.Now,

                    // dure doorway too
                    _ => Timestamps.FromTimeSpan(dataLoader.model.TimeDefInt() / double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)),
                };
            }

            if (dataLoader.model.IsRoad())
            {
                switch (this.GetRoadTimerResetMode())
                {
                    case "Always":
                        if (dataLoader.model.AreaID() == 458) // Hunter's Road Area 1
                        {
                            break;
                        }
                        else if (dataLoader.model.AreaID() == 459) // Hunter's Road Base Camp
                        {
                            if (dataLoader.model.RoadFloor() + 1 > dataLoader.model.previousRoadFloor)
                            {
                                // reset values
                                this.inQuest = false;
                                presenceTemplate.Timestamps = GetDiscordTimerMode() switch
                                {
                                    "Time Left" => Timestamps.FromTimeSpan(dataLoader.model.TimeInt() / double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)),
                                    "Time Elapsed" => Timestamps.Now,
                                    _ => Timestamps.FromTimeSpan(dataLoader.model.TimeInt() / double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)),
                                };
                            }

                            break;
                        }
                        else
                        {
                            break;
                        }

                    case "Never":
                        if (dataLoader.model.AreaID() == 458) // Hunter's Road Area 1
                        {
                            break;
                        }
                        else if (dataLoader.model.AreaID() == 459) // Hunter's Road Base Camp
                        {
                            if (dataLoader.model.RoadFloor() + 1 > dataLoader.model.previousRoadFloor)
                            {
                                // reset values
                                this.inQuest = false;

                                if (!startedRoadElapsedTime)
                                {
                                    startedRoadElapsedTime = true;
                                    presenceTemplate.Timestamps = Timestamps.Now;
                                }
                            }

                            break;
                        }
                        else
                        {
                            break;
                        }

                    default:
                        if (dataLoader.model.AreaID() == 458) // Hunter's Road Area 1
                        {
                            break;
                        }
                        else if (dataLoader.model.AreaID() == 459) // Hunter's Road Base Camp
                        {
                            if (dataLoader.model.RoadFloor() + 1 > dataLoader.model.previousRoadFloor)
                            {
                                // reset values
                                this.inQuest = false;

                                if (!startedRoadElapsedTime)
                                {
                                    startedRoadElapsedTime = true;
                                    presenceTemplate.Timestamps = Timestamps.Now;
                                }
                            }

                            break;
                        }
                        else
                        {
                            break;
                        }
                }
            }

            if (dataLoader.model.IsDure())
            {
                switch (dataLoader.model.AreaID())
                {
                    case 398: // Duremudira Arena

                        if (!inDuremudiraArena)
                        {
                            inDuremudiraArena = true;

                            if (dataLoader.model.QuestID() == 23649) // Arrogant Dure Slay
                            {
                                presenceTemplate.Timestamps = GetDiscordTimerMode() switch
                                {
                                    "Time Left" => Timestamps.FromTimeSpan(600),
                                    "Time Elapsed" => Timestamps.Now,
                                    _ => Timestamps.FromTimeSpan(600),
                                };
                            }
                            else
                            {
                                presenceTemplate.Timestamps = GetDiscordTimerMode() switch
                                {
                                    "Time Left" => Timestamps.FromTimeSpan(1200),
                                    "Time Elapsed" => Timestamps.Now,
                                    _ => Timestamps.FromTimeSpan(1200),
                                };
                            }
                        }

                        break;
                    default:
                        if (!inDuremudiraDoorway)
                        {
                            inDuremudiraDoorway = true;
                            presenceTemplate.Timestamps = Timestamps.Now;
                        }

                        break;
                }
            }
        }

        // going back to Mezeporta or w/e
        else if (dataLoader.model.QuestState() != 1 && dataLoader.model.QuestID() == 0 && this.inQuest && int.Parse(dataLoader.model.ATK, CultureInfo.InvariantCulture) == 0)
        {
            // reset values
            this.inQuest = false;
            startedRoadElapsedTime = false;
            inDuremudiraArena = false;
            inDuremudiraDoorway = false;

            presenceTemplate.Timestamps = Timestamps.Now;
        }

        // SmallInfo
        presenceTemplate.Assets.SmallImageKey = this.GetWeaponIconFromID(dataLoader.model.WeaponType(), dataLoader);

        if (GetHunterName != string.Empty && GetGuildName != string.Empty && GetServerName != string.Empty)
        {
            smallImageTextString = string.Format(CultureInfo.InvariantCulture, "{0} | {1} | {2} | GSR: {3} | {4} Style | Caravan Skills: {5}", GetHunterName, GetGuildName, GetServerName, dataLoader.model.GSR(), dataLoader.model.GetWeaponStyleFromID(dataLoader.model.WeaponStyle()), dataLoader.model.GetCaravanSkillsWithoutMarkdown(dataLoader));
            presenceTemplate.Assets.SmallImageText = smallImageTextString.Length <= MaxDiscordRPCStringLength ? smallImageTextString : string.Concat(smallImageTextString.AsSpan(0, MaxDiscordRPCStringLength - 3), "...");
        }

        if (discordRPCClient != null)
        {
            discordRPCClient.SetPresence(presenceTemplate);
        }
    }

    private static DiscordService? instance;

    private static readonly NLog.Logger LoggerInstance = NLog.LogManager.GetCurrentClassLogger();

    private static bool startedRoadElapsedTime;

    private static bool inDuremudiraArena;

    private static bool inDuremudiraDoorway;

    /// <summary>
    /// Is the main loop currently running?
    /// </summary>
    private static bool isDiscordRPCRunning;

    private DiscordService()
    {
        LoggerInstance.Info(CultureInfo.InvariantCulture, $"Service initialized");
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <value>
    /// The client.
    /// </value>
    private static DiscordRpcClient? discordRPCClient;

    // Called when your application first starts.
    // For example, just before your main loop, on OnEnable for unity.
    private static void SetupDiscordRPC()
    {
        /*
        Create a Discord client
        NOTE: 	If you are using Unity3D, you must use the full constructor and define
                 the pipe connection.
        */
        discordRPCClient = new DiscordRpcClient(GetDiscordClientID);

        // Set the LoggerInstance

        // Subscribe to events

        // Connect to the RPC
        discordRPCClient.Initialize();

        // Set the rich presence
        // Call this as many times as you want and anywhere in your code.
        LoggerInstance.Info(CultureInfo.InvariantCulture, "Set up Discord RPC");
    }

    /// <summary>
    /// Gets a value indicating whether [show discord RPC].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [show discord RPC]; otherwise, <c>false</c>.
    /// </value>
    private static bool ShowDiscordRPC
    {
        get
        {
            var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            return s.EnableRichPresence;
        }
    }

    /// <summary>
    /// Gets the discord client identifier.
    /// </summary>
    /// <value>
    /// The discord client identifier.
    /// </value>
    private static string GetDiscordClientID
    {
        get
        {
            var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            if (s.DiscordClientID.Length == 19)
            {
                return s.DiscordClientID;
            }
            else
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Gets the discord server invite.
    /// </summary>
    /// <value>
    /// The discord server invite.
    /// </value>
    private static string GetDiscordServerInvite
    {
        get
        {
            var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            if (s.DiscordServerInvite.Length >= 8 && s.DiscordServerInvite.Length <= 64)
            {
                return s.DiscordServerInvite;
            }
            else
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Gets the name of the hunter.
    /// </summary>
    /// <value>
    /// The name of the hunter.
    /// </value>
    private static string GetHunterName
    {
        get
        {
            var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            if (s.HunterName.Length >= 1 && s.HunterName.Length <= 64)
            {
                return s.HunterName;
            }
            else
            {
                return "Hunter Name";
            }
        }
    }

    /// <summary>
    /// Gets the name of the guild.
    /// </summary>
    /// <value>
    /// The name of the guild.
    /// </value>
    private static string GetGuildName
    {
        get
        {
            var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            if (s.GuildName.Length >= 1 && s.GuildName.Length <= 64)
            {
                return s.GuildName;
            }
            else
            {
                return "Guild Name";
            }
        }
    }

    private static string GetServerName
    {
        get
        {
            var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
            var serverNameFound = DiscordServers.DiscordServerID.TryGetValue(s.DiscordServerID, out var value);
            if (serverNameFound && value != null)
            {
                return value;
            }
            else
            {
                return "Unknown Server";
            }
        }
    }

    /// <summary>
    /// The current presence to send to discord.
    /// </summary>
    private static RichPresence presenceTemplate = new RichPresence()
    {
        Details = "【MHF-Z】Overlay " + App.CurrentProgramVersion,
        State = "Loading...",

        // check img folder
        Assets = new Assets()
        {
            LargeImageKey = "cattleya",
            LargeImageText = "Please Wait",
            SmallImageKey = "https://i.imgur.com/9OkLYAz.png",
            SmallImageText = "Hunter Name | Guild Name",
        },
        Buttons = new Button[]
            {
                new Button() { Label = "【MHF-Z】Overlay "+ App.CurrentProgramVersion, Url = "https://github.com/DorielRivalet/mhfz-overlay" },
                new Button() { Label = "Discord RPC C# Dev Site", Url = "https://lachee.dev/" },
            },
    };
      
    /// <summary>
    /// In the arena?
    /// </summary>
    /// <returns></returns>
    private static bool InArena(DataLoader dataLoader)
    {
        // dure and road
        if (dataLoader.model.AreaID() == 398 || dataLoader.model.AreaID() == 458)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the raviente event.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    private static string GetRavienteEvent(int id, DataLoader dataLoader)
    {
        RavienteTriggerEvents.RavienteTriggerEventIDs.TryGetValue(id, out var eventValue1);
        ViolentRavienteTriggerEvents.ViolentRavienteTriggerEventIDs.TryGetValue(id, out var eventValue2);
        BerserkRavienteTriggerEvents.BerserkRavienteTriggerEventIDs.TryGetValue(id, out var eventValue3);
        BerserkRavientePracticeTriggerEvents.BerserkRavientePracticeTriggerEventIDs.TryGetValue(id, out var eventValue4);

        switch (dataLoader.model.getRaviName())
        {
            default:
                return string.Empty;
            case "Raviente":
                return eventValue1 + string.Empty;
            case "Violent Raviente":
                return eventValue2 + string.Empty;
            case "Berserk Raviente Practice":
                return eventValue4 + string.Empty;
            case "Berserk Raviente":
                return eventValue3 + string.Empty;
            case "Extreme Raviente":
                return eventValue3 + string.Empty;
        }
    }

    /// <summary>
    /// Get quest state
    /// </summary>
    /// <returns></returns>
    private static string GetQuestState(DataLoader dataLoader)
    {
        if (dataLoader.model.isInLauncherBool) //works?
        {
            return string.Empty;
        }

        switch (dataLoader.model.QuestState())
        {
            default:
                return string.Empty;
            case 0:
                return string.Empty;
            case 1:
                return string.Format(CultureInfo.InvariantCulture, "Achieved Main Objective | {0} | ", dataLoader.model.Time);
            case 129:
                return string.Format(CultureInfo.InvariantCulture, "Quest Clear! | {0} | ", dataLoader.model.Time);
        }
    }

    private static bool ShowCaravanScore()
    {
        var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
        return s.EnableCaravanScore;
    }

    /// <summary>
    /// Gets the game mode.
    /// </summary>
    /// <param name="isHighGradeEdition">if set to <c>true</c> [is high grade edition].</param>
    /// <returns></returns>
    private static string GetGameMode(bool isHighGradeEdition)
    {
        if (isHighGradeEdition)
        {
            return " [High-Grade Edition]";
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the discord timer mode.
    /// </summary>
    /// <returns></returns>
    private static string GetDiscordTimerMode()
    {
        var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
        if (s.DiscordTimerMode == "Time Left")
        {
            return "Time Left";
        }
        else if (s.DiscordTimerMode == "Time Elapsed")
        {
            return "Time Elapsed";
        }
        else
        {
            return "Time Left";
        }
    }

    /// <summary>
    /// Gets the weapon name from identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    private static string GetWeaponNameFromID(int id)
    {
        WeaponType.IDName.TryGetValue(id, out var weaponname);
        return weaponname + string.Empty;
    }

    /// <summary>
    /// Gets the weapon icon from identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    private string GetWeaponIconFromID(int id, DataLoader dataLoader)
    {
        var weaponName = GetWeaponNameFromID(id);
        var colorName = dataLoader.model.GetArmorColor();

        var weaponIconName = string.Empty;

        switch (weaponName)
        {
            case "Sword and Shield":
                weaponIconName += "sns";
                break;
            case "Dual Swords":
                weaponIconName += "ds";
                break;
            case "Great Sword":
                weaponIconName += "gs";
                break;
            case "Long Sword":
                weaponIconName += "ls";
                break;
            case "Hammer":
                weaponIconName += "hammer";
                break;
            case "Hunting Horn":
                weaponIconName += "hh";
                break;
            case "Lance":
                weaponIconName += "lance";
                break;
            case "Gunlance":
                weaponIconName += "gl";
                break;
            case "Switch Axe F":
                weaponIconName += "saf";
                break;
            case "Tonfa":
                weaponIconName += "tonfa";
                break;
            case "Magnet Spike":
                weaponIconName += "ms";
                break;
            case "Light Bowgun":
                weaponIconName += "lbg";
                break;
            case "Heavy Bowgun":
                weaponIconName += "hbg";
                break;
            case "Bow":
                weaponIconName += "bow";
                break;
            default:
                weaponIconName = "https://i.imgur.com/9OkLYAz.png";//transcend
                break;
        }

        if (weaponIconName != "https://i.imgur.com/9OkLYAz.png" && !colorName.Contains("White"))
        {
            weaponIconName += "_";
        }

        if (colorName.Contains("Green"))
        {
            weaponIconName += "green";
        }
        else if (colorName.Contains("Red"))
        {
            weaponIconName += "red";
        }
        else if (colorName.Contains("Pink"))
        {
            weaponIconName += "pink";
        }
        else if (colorName.Contains("Blue"))
        {
            weaponIconName += "blue";
        }
        else if (colorName.Contains("Navy"))
        {
            weaponIconName += "navy";
        }
        else if (colorName.Contains("Cyan"))
        {
            weaponIconName += "cyan";
        }
        else if (colorName.Contains("Purple"))
        {
            weaponIconName += "purple";
        }
        else if (colorName.Contains("Orange"))
        {
            weaponIconName += "orange";
        }
        else if (colorName.Contains("Yellow"))
        {
            weaponIconName += "yellow";
        }
        else if (colorName.Contains("Grey"))
        {
            weaponIconName += "grey";
        }
        else if (colorName.Contains("Rainbow"))
        {
            weaponIconName += "rainbow";
        }

        return weaponIconName switch
        {
            "sns" => "https://i.imgur.com/hVCDfnA.png",
            "sns_green" => "https://i.imgur.com/U61zPJa.png",
            "sns_red" => "https://i.imgur.com/ZGCd1lH.png",
            "sns_pink" => "https://i.imgur.com/O4qyBfI.png",
            "sns_blue" => "https://i.imgur.com/dQvflcw.png",
            "sns_navy" => "https://i.imgur.com/vdeSnZh.png",
            "sns_cyan" => "https://i.imgur.com/37gfP8P.png",
            "sns_purple" => "https://i.imgur.com/x7dFt4G.png",
            "sns_orange" => "https://i.imgur.com/bz22IiF.png",
            "sns_yellow" => "https://i.imgur.com/wKItSbP.png",
            "sns_grey" => "https://i.imgur.com/U25Xxfj.png",
            "sns_rainbow" => "https://i.imgur.com/3a6OI1V.gif",
            "ds" => "https://i.imgur.com/JIFNgz9.png",
            "ds_green" => "https://i.imgur.com/MEWrHcC.png",
            "ds_red" => "https://i.imgur.com/dzIoOF2.png",
            "ds_pink" => "https://i.imgur.com/OfUVJy6.png",
            "ds_blue" => "https://i.imgur.com/fUvIuCl.png",
            "ds_navy" => "https://i.imgur.com/oPz7WAA.png",
            "ds_cyan" => "https://i.imgur.com/Lkf6v4A.png",
            "ds_purple" => "https://i.imgur.com/b5Ly09E.png",
            "ds_orange" => "https://i.imgur.com/LdHWvui.png",
            "ds_yellow" => "https://i.imgur.com/2A8UXdT.png",
            "ds_grey" => "https://i.imgur.com/snw6dPs.png",
            "ds_rainbow" => "https://i.imgur.com/eWTRTJl.gif",
            "gs" => "https://i.imgur.com/vLxcWM8.png",
            "gs_green" => "https://i.imgur.com/9puI44e.png",
            "gs_red" => "https://i.imgur.com/Xhs5yJj.png",
            "gs_pink" => "https://i.imgur.com/DXI9FHs.png",
            "gs_blue" => "https://i.imgur.com/GxWofdH.png",
            "gs_navy" => "https://i.imgur.com/ZM1Isqt.png",
            "gs_cyan" => "https://i.imgur.com/tO2TrkB.png",
            "gs_purple" => "https://i.imgur.com/ijgJ69Y.png",
            "gs_orange" => "https://i.imgur.com/CWHEhAi.png",
            "gs_yellow" => "https://i.imgur.com/ZpGOD3z.png",
            "gs_grey" => "https://i.imgur.com/82HhSFD.png",
            "gs_rainbow" => "https://i.imgur.com/WnRuqll.gif",
            "ls" => "https://i.imgur.com/qdA0x3k.png",
            "ls_green" => "https://i.imgur.com/9LvQVQ7.png",
            "ls_red" => "https://i.imgur.com/oc0ExLi.png",
            "ls_pink" => "https://i.imgur.com/jjBGbGu.png",
            "ls_blue" => "https://i.imgur.com/AZ606vb.png",
            "ls_navy" => "https://i.imgur.com/M6mmOpO.png",
            "ls_cyan" => "https://i.imgur.com/qHFbpoJ.png",
            "ls_purple" => "https://i.imgur.com/ICgTu6S.png",
            "ls_orange" => "https://i.imgur.com/XPYDego.png",
            "ls_yellow" => "https://i.imgur.com/H4vJFd1.png",
            "ls_grey" => "https://i.imgur.com/1v7T5Hm.png",
            "ls_rainbow" => "https://i.imgur.com/BUYVOih.gif",
            "hammer" => "https://i.imgur.com/hnY1HC0.png",
            "hammer_green" => "https://i.imgur.com/iOGBcmQ.png",
            "hammer_red" => "https://i.imgur.com/Z5QGsTO.png",
            "hammer_pink" => "https://i.imgur.com/WHkXOoC.png",
            "hammer_blue" => "https://i.imgur.com/fb7bxlw.png",
            "hammer_navy" => "https://i.imgur.com/oaLfSIP.png",
            "hammer_cyan" => "https://i.imgur.com/N2N0Uib.png",
            "hammer_purple" => "https://i.imgur.com/CqNUgtg.png",
            "hammer_orange" => "https://i.imgur.com/PzYNYZh.png",
            "hammer_yellow" => "https://i.imgur.com/Ujpj7WL.png",
            "hammer_grey" => "https://i.imgur.com/R0xCYk5.png",
            "hammer_rainbow" => "https://i.imgur.com/GIAbKkO.gif",
            "hh" => "https://i.imgur.com/EmjAq37.png",
            "hh_green" => "https://i.imgur.com/LWCOXI4.png",
            "hh_red" => "https://i.imgur.com/lwtBV09.png",
            "hh_pink" => "https://i.imgur.com/tZBuDi2.png",
            "hh_blue" => "https://i.imgur.com/7qncIzQ.png",
            "hh_navy" => "https://i.imgur.com/yaFS4N0.png",
            "hh_cyan" => "https://i.imgur.com/GvHKg1u.png",
            "hh_purple" => "https://i.imgur.com/33FpZMA.png",
            "hh_orange" => "https://i.imgur.com/5ZHbR8K.png",
            "hh_yellow" => "https://i.imgur.com/2YdtoVI.png",
            "hh_grey" => "https://i.imgur.com/pyPzmJI.png",
            "hh_rainbow" => "https://i.imgur.com/VuRLWWG.gif",
            "lance" => "https://i.imgur.com/M8fmT4f.png",
            "lance_green" => "https://i.imgur.com/zSyyIZY.png",
            "lance_red" => "https://i.imgur.com/ZFeN3aA.png",
            "lance_pink" => "https://i.imgur.com/X1EncHA.png",
            "lance_blue" => "https://i.imgur.com/qMM2gqG.png",
            "lance_navy" => "https://i.imgur.com/F7vp82x.png",
            "lance_cyan" => "https://i.imgur.com/9q1rqfF.png",
            "lance_purple" => "https://i.imgur.com/qF9JMEE.png",
            "lance_orange" => "https://i.imgur.com/s1Agqri.png",
            "lance_yellow" => "https://i.imgur.com/EcOCe50.png",
            "lance_grey" => "https://i.imgur.com/jKPcLtN.png",
            "lance_rainbow" => "https://i.imgur.com/BXgEuDy.gif",
            "gl" => "https://i.imgur.com/9wq3LQe.png",
            "gl_green" => "https://i.imgur.com/bQdGiiB.png",
            "gl_red" => "https://i.imgur.com/QorzUm5.png",
            "gl_pink" => "https://i.imgur.com/OqkXeZy.png",
            "gl_blue" => "https://i.imgur.com/lsFZSnT.png",
            "gl_navy" => "https://i.imgur.com/2fkkHVd.png",
            "gl_cyan" => "https://i.imgur.com/MzqTO9c.png",
            "gl_purple" => "https://i.imgur.com/QqDN0jm.png",
            "gl_orange" => "https://i.imgur.com/GowSPSA.png",
            "gl_yellow" => "https://i.imgur.com/az9QfWH.png",
            "gl_grey" => "https://i.imgur.com/Q5lK9Nw.png",
            "gl_rainbow" => "https://i.imgur.com/47VsWHj.gif",
            "saf" => "https://i.imgur.com/fVbaN34.png",
            "saf_green" => "https://i.imgur.com/V3x8aaf.png",
            "saf_red" => "https://i.imgur.com/3l8TO9T.png",
            "saf_pink" => "https://i.imgur.com/DTXXEb9.png",
            "saf_blue" => "https://i.imgur.com/Dgr9oQg.png",
            "saf_navy" => "https://i.imgur.com/Tv40lQg.png",
            "saf_cyan" => "https://i.imgur.com/uKxiYhr.png",
            "saf_purple" => "https://i.imgur.com/x3RC716.png",
            "saf_orange" => "https://i.imgur.com/GU2eOdb.png",
            "saf_yellow" => "https://i.imgur.com/f0jrcYq.png",
            "saf_grey" => "https://i.imgur.com/jIRe9fA.png",
            "saf_rainbow" => "https://i.imgur.com/icBF5lS.gif",
            "tonfa" => "https://i.imgur.com/8YpLQ5G.png",
            "tonfa_green" => "https://i.imgur.com/0VflTRd.png",
            "tonfa_red" => "https://i.imgur.com/f5mIJgU.png",
            "tonfa_pink" => "https://i.imgur.com/M6ANARX.png",
            "tonfa_blue" => "https://i.imgur.com/BrCnJbs.png",
            "tonfa_navy" => "https://i.imgur.com/b2lbCN1.png",
            "tonfa_cyan" => "https://i.imgur.com/7bm8xyW.png",
            "tonfa_purple" => "https://i.imgur.com/BOcCFhU.png",
            "tonfa_orange" => "https://i.imgur.com/vi8qGs5.png",
            "tonfa_yellow" => "https://i.imgur.com/qDR1aJZ.png",
            "tonfa_grey" => "https://i.imgur.com/GxFrQm6.png",
            "tonfa_rainbow" => "https://i.imgur.com/2StcKCZ.gif",
            "ms" => "https://i.imgur.com/s3OaNkP.png",
            "ms_green" => "https://i.imgur.com/7c8pPow.png",
            "ms_red" => "https://i.imgur.com/zA4wMON.png",
            "ms_pink" => "https://i.imgur.com/dOc22Dm.png",
            "ms_blue" => "https://i.imgur.com/rz4anE4.png",
            "ms_navy" => "https://i.imgur.com/dvghN1a.png",
            "ms_cyan" => "https://i.imgur.com/gCWBOWm.png",
            "ms_purple" => "https://i.imgur.com/UI3KO1c.png",
            "ms_orange" => "https://i.imgur.com/9Bg0QzE.png",
            "ms_yellow" => "https://i.imgur.com/rAKEtTa.png",
            "ms_grey" => "https://i.imgur.com/dNkcRIR.png",
            "ms_rainbow" => "https://i.imgur.com/TyZFrvK.gif",
            "lbg" => "https://i.imgur.com/txp2GsM.png",
            "lbg_green" => "https://i.imgur.com/CMf9U6x.png",
            "lbg_red" => "https://i.imgur.com/aKLv0na.png",
            "lbg_pink" => "https://i.imgur.com/theTGWy.png",
            "lbg_blue" => "https://i.imgur.com/IgXj7vl.png",
            "lbg_navy" => "https://i.imgur.com/N8vleIL.png",
            "lbg_cyan" => "https://i.imgur.com/4iF0Kex.png",
            "lbg_purple" => "https://i.imgur.com/V36MwUh.png",
            "lbg_orange" => "https://i.imgur.com/FZ3sAAr.png",
            "lbg_yellow" => "https://i.imgur.com/l0Fga4q.png",
            "lbg_grey" => "https://i.imgur.com/WE1oZuG.png",
            "lbg_rainbow" => "https://i.imgur.com/Q0Firpd.gif",
            "hbg" => "https://i.imgur.com/8WD2bI7.png",
            "hbg_green" => "https://i.imgur.com/j6qe1uh.png",
            "hbg_red" => "https://i.imgur.com/hd8cwCa.png",
            "hbg_pink" => "https://i.imgur.com/PDfABOO.png",
            "hbg_blue" => "https://i.imgur.com/qURblCM.png",
            "hbg_navy" => "https://i.imgur.com/FxInecI.png",
            "hbg_cyan" => "https://i.imgur.com/UclnhBS.png",
            "hbg_purple" => "https://i.imgur.com/IHifTBB.png",
            "hbg_orange" => "https://i.imgur.com/7JRHNzp.png",
            "hbg_yellow" => "https://i.imgur.com/rihlgaB.png",
            "hbg_grey" => "https://i.imgur.com/mKpJc0p.png",
            "hbg_rainbow" => "https://i.imgur.com/TgPORx6.gif",
            "bow" => "https://i.imgur.com/haCsXQr.png",
            "bow_green" => "https://i.imgur.com/vykrGg9.png",
            "bow_red" => "https://i.imgur.com/01nEtNy.png",
            "bow_pink" => "https://i.imgur.com/DLIYT8G.png",
            "bow_blue" => "https://i.imgur.com/THX3O3X.png",
            "bow_navy" => "https://i.imgur.com/DGHifcq.png",
            "bow_cyan" => "https://i.imgur.com/sXnzQrG.png",
            "bow_purple" => "https://i.imgur.com/D6NYg8r.png",
            "bow_orange" => "https://i.imgur.com/fy47m6l.png",
            "bow_yellow" => "https://i.imgur.com/ExGTxvl.png",
            "bow_grey" => "https://i.imgur.com/Y5vOofE.png",
            "bow_rainbow" => "https://i.imgur.com/rsEycVk.gif",
            _ => "https://i.imgur.com/9OkLYAz.png", // transcend
        };
    }

    private bool inQuest;

    // quest ids:
    // mp road: 23527
    // solo road: 23628
    // 1st district dure: 21731
    // 2nd district dure: 21746
    // 1st district dure sky corridor: 21749
    // 2nd district dure sky corridor: 21750
    // arrogant dure repel: 23648
    // arrogant dure slay: 23649
    // urgent tower: 21751
    // 4th district dure: 21748
    // 3rd district dure: 21747
    // 3rd district dure 2: 21734
    // UNUSED sky corridor: 21730
    // sky corridor prologue: 21729
    // raviente 62105
    // raviente carve 62108
    ///violent raviente 62101
    ///violent carve 62104
    // berserk slay practice 55796
    // berserk support practice 1 55802
    // berserk support practice 2 55803
    // berserk support practice 3 55804
    // berserk support practice 4 55805
    // berserk support practice 5 55806
    // berserk practice carve 55807
    // berserk slay  54751
    // berserk support 1 54756
    // berserk support 2 54757
    // berserk support 3 54758
    // berserk support 4 54759
    // berserk support 5 54760
    // berserk carve 54761
    // extreme slay (musou table 54) 55596
    // extreme support 1 55602
    // extreme support 2 55603
    // extreme support 3 55604
    // extreme support 4 55605
    // extreme support 5 55606
    // extreme carve 55607
    private const int MaxDiscordRPCStringLength = 127; // or any other maximum length specified by Discord

    /// <summary>
    /// Gets the road timer reset mode.
    /// </summary>
    /// <returns></returns>
    private string GetRoadTimerResetMode()
    {
        var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");
        if (s.DiscordRoadTimerReset == "Never")
        {
            return "Never";
        }
        else if (s.DiscordRoadTimerReset == "Always")
        {
            return "Always";
        }
        else
        {
            return "Never";
        }
    }

    /// <summary>
    /// Gets the quest information.
    /// </summary>
    /// <returns></returns>
    private string GetQuestInformation(DataLoader dataLoader)
    {
        if (ShowDiscordQuestNames())
        {
            switch (dataLoader.model.QuestID())
            {
                case 23648: // arrogant repel
                    return "Repel Arrogant Duremudira | ";
                case 23649: // arrogant slay
                    return "Slay Arrogant Duremudira | ";
                case 23527: // Hunter's Road Multiplayer
                    return string.Empty;
                case 23628: // solo road
                    return string.Empty;
                case 21731: // 1st district dure
                case 21749: // sky corridor version
                    return "Slay 1st District Duremudira | ";
                case 21746: // 2nd district dure
                case 21750: // sky corridor version
                    return "Slay 2nd District Duremudira | ";
                default:
                    if ((dataLoader.model.ObjectiveType() == 0x0 || dataLoader.model.ObjectiveType() == 0x02 || dataLoader.model.ObjectiveType() == 0x1002 || dataLoader.model.ObjectiveType() == 0x10) && dataLoader.model.QuestID() != 23527 && dataLoader.model.QuestID() != 23628 && dataLoader.model.QuestID() != 21731 && dataLoader.model.QuestID() != 21749 && dataLoader.model.QuestID() != 21746 && dataLoader.model.QuestID() != 21750)
                    {
                        return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5} | ", dataLoader.model.GetObjectiveNameFromID(dataLoader.model.ObjectiveType(), true), dataLoader.model.GetObjective1CurrentQuantity(true), dataLoader.model.GetObjective1Quantity(true), dataLoader.model.GetRankNameFromID(dataLoader.model.RankBand(), true), dataLoader.model.GetStarGrade(true), dataLoader.model.GetObjective1Name(dataLoader.model.Objective1ID(), true));
                    }
                    else
                    {
                        return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5} | ", dataLoader.model.GetObjectiveNameFromID(dataLoader.model.ObjectiveType(), true), string.Empty, dataLoader.model.GetObjective1Quantity(true), dataLoader.model.GetRankNameFromID(dataLoader.model.RankBand(), true), dataLoader.model.GetStarGrade(true), dataLoader.model.GetRealMonsterName(true));
                    }
            }
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the size of the party.
    /// </summary>
    /// <returns></returns>
    private string GetPartySize(DataLoader dataLoader)
    {
        if (dataLoader.model.QuestID() == 0 || dataLoader.model.PartySize() == 0 || dataLoader.model.isInLauncher() == "NULL" || dataLoader.model.isInLauncher() == "Yes")
        {
            return string.Empty;
        }
        else
        {
            return string.Format(CultureInfo.InvariantCulture, "Party: {0}/{1} | ", dataLoader.model.PartySize(), this.GetPartySizeMax(dataLoader));
        }
    }

    private int GetPartySizeMax(DataLoader dataLoader)
    {
        if (dataLoader.model.PartySize() >= dataLoader.model.PartySizeMax())
        {
            return dataLoader.model.PartySize();
        }
        else
        {
            return dataLoader.model.PartySizeMax();
        }
    }

    /// <summary>
    /// Gets the caravan score.
    /// </summary>
    /// <returns></returns>
    private string GetCaravanScore(DataLoader dataLoader)
    {
        if (ShowCaravanScore())
        {
            return string.Format(CultureInfo.InvariantCulture, "Caravan Score: {0} | ", dataLoader.model.CaravanScore());
        }
        else
        {
            return string.Empty;
        }
    }
}
