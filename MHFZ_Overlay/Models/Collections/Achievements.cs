// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Structures;
using Octokit;

/// <summary>
/// Achievements dictionary. TODO.
/// </summary>
public static class Achievements
{
    /// <summary>
    /// Gets the achievement list.
    /// </summary>
    /// <value>
    /// The achievement list.
    /// </value>
    public static ReadOnlyDictionary<int, Achievement> IDAchievement { get; } = new (new Dictionary<int, Achievement>
    {
        {
            0, new Achievement()
            {
                CompletionDate = DateTime.UnixEpoch,
                Title = "Akura Vashimu Slain",
                Description = string.Empty,
                Rank = AchievementRank.Bronze,
                Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
                Objective = "Complete 1 Zenith★4 Akura Vashimu quest.",
                IsSecret = false,
                Hint = string.Empty,
            }
        },
        {
            1, new Achievement()
            {
                CompletionDate = DateTime.UnixEpoch,
                Title = "Akura Vashimu Slayer",
                Description = string.Empty,
                Rank = AchievementRank.Bronze,
                Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
                Objective = "Complete 10 Zenith★4 Akura Vashimu quests.",
                IsSecret = false,
                Hint = string.Empty,
            }
        },
        {
            2, new Achievement()
            {
                CompletionDate = DateTime.UnixEpoch,
                Title = "Akura Vashimu Annihilator",
                Description = string.Empty,
                Rank = AchievementRank.Silver,
                Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
                Objective = "Complete 25 Zenith★4 Akura Vashimu quests.",
                IsSecret = false,
                Hint = string.Empty,
            }
        },
        {
            3, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Akura Vashimu Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Akura Vashimu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            4, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Akura Vashimu's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Akura Vashimu quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            5, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Anorupatisu Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Anorupatisu quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            6, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Anorupatisu Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Anorupatisu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            7, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Anorupatisu Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Anorupatisu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            8, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Anorupatisu Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Anorupatisu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            9, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Anorupatisu's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Anorupatisu quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            10, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blangonga Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Blangonga quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            11, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blangonga Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Blangonga quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            12, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blangonga Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Blangonga quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            13, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blangonga Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Blangonga quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            14, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blangonga's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Blangonga quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            15, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Daimyo Hermitaur Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Daimyo Hermitaur quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            16, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Daimyo Hermitaur Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Daimyo Hermitaur quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            17, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Daimyo Hermitaur Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Daimyo Hermitaur quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            18, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Daimyo Hermitaur Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Daimyo Hermitaur quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            19, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Daimyo Hermitaur's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Daimyo Hermitaur quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            20, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Doragyurosu Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Doragyurosu quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            21, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Doragyurosu Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Doragyurosu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            22, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Doragyurosu Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Doragyurosu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            23, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Doragyurosu Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Doragyurosu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            24, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Doragyurosu's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Doragyurosu quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            25, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Espinas Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Espinas quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            26, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Espinas Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Espinas quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            27, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Espinas Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Espinas quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            28, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Espinas Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Espinas quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            29, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Espinas' Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Espinas quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            30, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gasurabazura Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Gasurabazura quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            31, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gasurabazura Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Gasurabazura quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            32, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gasurabazura Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Gasurabazura quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            33, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gasurabazura Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Gasurabazura quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            34, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gasurabazura's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Gasurabazura quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            35, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Giaorugu Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Giaorugu quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            36, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Giaorugu Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Giaorugu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            37, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Giaorugu Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Giaorugu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            38, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Giaorugu Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Giaorugu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            39, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Giaorugu's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Giaorugu quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            40, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hypnocatrice Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Hypnocatrice quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            41, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hypnocatrice Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Hypnocatrice quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            42, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hypnocatrice Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Hypnocatrice quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            43, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hypnocatrice Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Hypnocatrice quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            44, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hypnocatrice's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Hypnocatrice quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            45, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hyujikiki Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Hyujikiki quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            46, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hyujikiki Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Hyujikiki quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            47, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hyujikiki Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Hyujikiki quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            48, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hyujikiki Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Hyujikiki quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            49, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hyujikiki's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Hyujikiki quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            50, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Inagami Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Inagami quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            51, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Inagami Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Inagami quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            52, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Inagami Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Inagami quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            53, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Inagami Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Inagami quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            54, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Inagami's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Inagami quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            55, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Khezu Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Khezu quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            56, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Khezu Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Khezu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            57, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Khezu Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Khezu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            58, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Khezu Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Khezu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            59, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Khezu's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Khezu quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            60, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Midogaron Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Midogaron quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            61, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Midogaron Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Midogaron quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            62, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Midogaron Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Midogaron quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            63, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Midogaron Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Midogaron quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            64, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Midogaron's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Midogaron quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            65, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Plesioth Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Plesioth quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            66, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Plesioth Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Plesioth quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            67, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Plesioth Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Plesioth quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            68, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Plesioth Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Plesioth quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            69, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Plesioth's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Plesioth quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            70, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rathalos Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Rathalos quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            71, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rathalos Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Rathalos quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            72, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rathalos Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Rathalos quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            73, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rathalos Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Rathalos quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            74, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rathalos' Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Rathalos quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            75, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rukodiora Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Rukodiora quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            76, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rukodiora Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Rukodiora quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            77, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rukodiora Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Rukodiora quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            78, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rukodiora Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Rukodiora quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            79, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rukodiora's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Rukodiora quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            80, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Tigrex Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Tigrex quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            81, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Tigrex Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Tigrex quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            82, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Tigrex Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Tigrex quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            83, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Tigrex Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Tigrex quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            84, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Tigrex's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Tigrex quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            85, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Toridcless Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Toridcless quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            86, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Toridcless Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Toridcless quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            87, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Toridcless Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Toridcless quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            88, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Toridcless Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Toridcless quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            89, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Toridcless' Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Toridcless quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            90, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Baruragaru Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Baruragaru quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            91, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Baruragaru Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Baruragaru quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            92, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Baruragaru Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Baruragaru quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            93, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Baruragaru Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Baruragaru quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            94, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Baruragaru's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Baruragaru quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            95, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bogabadorumu Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Bogabadorumu quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            96, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bogabadorumu Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Bogabadorumu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            97, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bogabadorumu Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Bogabadorumu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            98, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bogabadorumu Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Bogabadorumu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            99, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bogabadorumu's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Bogabadorumu quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            100, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gravios Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Gravios quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            101, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gravios Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Gravios quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            102, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gravios Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Gravios quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            103, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gravios Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Gravios quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            104, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gravios' Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Gravios quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            105, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Harudomerugu Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Harudomerugu quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            106, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Harudomerugu Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Harudomerugu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            107, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Harudomerugu Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Harudomerugu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            108, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Harudomerugu Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Harudomerugu quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            109, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Harudomerugu's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Harudomerugu quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            110, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Taikun Zamuza Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Taikun Zamuza quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            111, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Taikun Zamuza Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Zenith★4 Taikun Zamuza quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            112, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Taikun Zamuza Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Zenith★4 Taikun Zamuza quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            113, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Taikun Zamuza Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Zenith★4 Taikun Zamuza quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            114, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Taikun Zamuza's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Zenith★4 Taikun Zamuza quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            115, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fatalis Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Lv9999 Fatalis quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            116, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fatalis Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Lv9999 Fatalis quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            117, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fatalis Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Lv9999 Fatalis quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            118, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fatalis Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Lv9999 Fatalis quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            119, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fatalis' Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Lv9999 Fatalis quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            120, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Crimson Fatalis Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Lv9999 Crimson Fatalis quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            121, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Crimson Fatalis Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Lv9999 Crimson Fatalis quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            122, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Crimson Fatalis Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Lv9999 Crimson Fatalis quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            123, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Crimson Fatalis Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Lv9999 Crimson Fatalis quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            124, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Crimson Fatalis' Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Lv9999 Crimson Fatalis quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            125, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shantien Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Lv9999 Shantien quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            126, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shantien Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Lv9999 Shantien quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            127, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shantien Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Lv9999 Shantien quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            128, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shantien Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Lv9999 Shantien quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            129, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shantien's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Lv9999 Shantien quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            130, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Disufiroa Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Lv9999 Disufiroa quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            131, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Disufiroa Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Lv9999 Disufiroa quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            132, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Disufiroa Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Lv9999 Disufiroa quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            133, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Disufiroa Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Lv9999 Disufiroa quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            134, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Disufiroa's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Lv9999 Disufiroa quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            135, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "UNKNOWN Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Upper Shiten Unknown quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            136, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "UNKNOWN Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Upper Shiten Unknown quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            137, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "UNKNOWN Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Upper Shiten Unknown quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            138, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "UNKNOWN Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Upper Shiten Unknown quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            139, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "UNKNOWN's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 1 Upper Shiten Unknown quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            140, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "At World's End",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Upper Shiten Disufiroa quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            141, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Apocalyptic Red Moon",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Upper Shiten Disufiroa quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            142, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Disufiroa's Last Stand",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Upper Shiten Disufiroa quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            143, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blood Moon Emperor",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Upper Shiten Disufiroa quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            144, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Eclipse Conqueror",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 1 Upper Shiten Disufiroa quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            145, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Pariapuria Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Thirsty Pariapuria quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            146, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Pariapuria Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Thirsty Pariapuria quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            147, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Pariapuria Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Thirsty Pariapuria quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            148, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Pariapuria Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Thirsty Pariapuria quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            149, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Pariapuria's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Thirsty Pariapuria quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            150, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Guanzorumu Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Ruling Guanzorumu True Slay quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            151, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Guanzorumu Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Ruling Guanzorumu True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            152, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Guanzorumu Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Ruling Guanzorumu True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            153, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Guanzorumu Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Ruling Guanzorumu True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            154, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Guanzorumu's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Ruling Guanzorumu True Slay quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            155, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Mi Ru Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Shifting Mi Ru quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            156, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Mi Ru Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Shifting Mi Ru quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            157, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Mi Ru Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Shifting Mi Ru quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            158, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Mi Ru Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Shifting Mi Ru quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            159, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Mi Ru's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Shifting Mi Ru quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            160, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Nargacuga Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Blinking Nargacuga True Slay quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            161, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Nargacuga Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Blinking Nargacuga True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            162, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Nargacuga Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Blinking Nargacuga True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            163, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Nargacuga Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Blinking Nargacuga True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            164, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Nargacuga's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 1 Blinking Nargacuga True Slay quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            165, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zinogre Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Howling Zinogre True Slay quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            166, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zinogre Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Howling Zinogre True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            167, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zinogre Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Howling Zinogre True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            168, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zinogre Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Howling Zinogre True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            169, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zinogre's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 1 Howling Zinogre True Slay quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            170, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Deviljho Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Starving Deviljho True Slay quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            171, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Deviljho Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Starving Deviljho True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            172, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Deviljho Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Starving Deviljho True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            173, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Deviljho Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Starving Deviljho True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            174, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Deviljho's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 1 Starving Deviljho True Slay quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            175, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zerureusu Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Sparkling Zerureusu True Slay quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            176, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zerureusu Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Sparkling Zerureusu True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            177, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zerureusu Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Sparkling Zerureusu True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            178, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zerureusu Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Sparkling Zerureusu True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            179, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zerureusu's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 1 Sparkling Zerureusu True Slay quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            180, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Duremudira Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Arrogant Duremudira True Slay quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            181, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Duremudira Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Arrogant Duremudira True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            182, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Duremudira Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Arrogant Duremudira True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            183, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Duremudira Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Arrogant Duremudira True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            184, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Duremudira's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 1 Arrogant Duremudira True Slay quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            185, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bombardier",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Bombardier Bogabadorumu True Slay quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            186, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blitzkrieg",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Bombardier Bogabadorumu True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            187, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Boggy",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Bombardier Bogabadorumu True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            188, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Boggers",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Bombardier Bogabadorumu True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            189, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Explosion!",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 1 Bombardier Bogabadorumu True Slay quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            190, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Elzelion Slain",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 1 Burning Freezing Elzelion True Slay quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            191, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Elzelion Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete 10 Burning Freezing Elzelion True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            192, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Elzelion Annihilator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 25 Burning Freezing Elzelion True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            193, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Elzelion Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete 50 Burning Freezing Elzelion True Slay quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            194, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Elzelion's Nightmare",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete 1 Burning Freezing Elzelion True Slay quest solo (Speedrun/Zen).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            195, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Seriously Thirsty",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete a Thirsty Pariapuria quest with a Serious Drink Affinity in your inventory.",
            IsSecret = true,
            Hint = "I wish I had something to drink...seriously!",
            }
        },
        {
            196, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Back to the Past",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_yellow.jpg",
            Objective = "Complete a Ruling Guanzorumu True Slay quest with a non-extreme style.",
            IsSecret = true,
            Hint = "Do I really need to run to beat this monster?",
            }
        },
        {
            197, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shifty",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete a Shifting Mi Ru quest while having pressed Shift at the start.",
            IsSecret = true,
            Hint = "What if the hunter is also shifting?",
            }
        },
        {
            198, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Ultra Instinct",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_blue2.jpg",
            Objective = "Complete a Blinking Nargacuga True Slay quest without getting hit (including blocks)",
            IsSecret = true,
            Hint = "Do you have the instinct to dodge everything?",
            }
        },
        {
            199, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "A Lonely and Starving Wolf",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_blue2.jpg",
            Objective = "Complete a Howling Zinogre True Slay quest solo with 40 max stamina.",
            IsSecret = true,
            Hint = "I'm starving and lonely.",
            }
        },
        {
            200, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Majestic Lord of Ice and Fire",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_blue2.jpg",
            Objective = "Complete a Burning Freezing Elzelion True Slay quest with Blazing Majesty and Ice Age",
            IsSecret = true,
            Hint = "Fire and flame? No, ice and blaze!",
            }
        },
        {
            201, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bombardment",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete a Bombardier Bogabadorumu True Slay quest with a Large Barrel Bomb in your inventory.",
            IsSecret = true,
            Hint = "We are going to need bigger bombs for this guy...",
            }
        },
        {// TODO test if this works
            202, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Oblivion Negated",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete an Arrogant Duremudira True Slay quest with Soul Revival equipped.",
            IsSecret = true,
            Hint = "This thunder brings oblivion! Unless...",
            }
        },
        {
            203, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Lovely Vegetables",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Complete a certain quest on February 14th.",
            IsSecret = true,
            Hint = "Veggie Elder! How I missed you...",
            }
        },
        {
            204, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Ultimate Gunlance Duel",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Defeat Producer Gogomoa with a Gunlance",
            IsSecret = true,
            Hint = "If he uses a Gunlance, so do I!",
            }
        },
        {
            205, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Emperor of Fire",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Complete a Four Heavenly King Deviljho quest.",
            IsSecret = true,
            Hint = "Slay an emperor of fire.",
            }
        },
        {
            206, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Dancing Gopher",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Complete a Hatsune Miku quest.",
            IsSecret = true,
            Hint = "Erupe thinks we should dance!",
            }
        },
        {
            207, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Singing Rappy",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Complete a PSO2 quest.",
            IsSecret = true,
            Hint = "Rappy thinks we should sing!",
            }
        },
        {
            208, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gears of Destiny",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_white.jpg",
            Objective = "Complete a Megaman quest.",
            IsSecret = true,
            Hint = "Obtain the Gears of Destiny.",
            }
        },
        {
            209, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Higanjima",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Complete a Higanjima quest.",
            IsSecret = true,
            Hint = "Higanjima.",
            }
        },
        {
            210, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Plesioth Transformation",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Defeat the huge Plesioth.",
            IsSecret = true,
            Hint = "Don't let the Plesioth grow.",
            }
        },
        {
            211, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Oh Noes! My Sunglasses!",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Defeat Sunglasses Kut-Ku.",
            IsSecret = true,
            Hint = "I need a new pair of sunglasses...",
            }
        },
        {
            212, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Quiz Time!",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_white.jpg",
            Objective = "Complete 1 MHF-Q quest.",
            IsSecret = true,
            Hint = "You need an extreme amount of knowledge in order to complete this quest.",
            }
        },
        {
            213, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Smell No Evil",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_white2.jpg",
            Objective = "Complete a certain Congalala quest.",
            IsSecret = true,
            Hint = "See no evil, hear no evil, speak no evil.",
            }
        },
        {
            214, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zenny Galore",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Get the maximum amount of GZenny",
            IsSecret = true,
            Hint = "I should probably hunt a Zenith Hypnoc.",
            }
        },
        {
            215, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Diva's Friend",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/diva.jpg",
            Objective = "Get the maximum amount of Diva Bond",
            IsSecret = true,
            Hint = "Do you like fluffy cakes?",
            }
        },
        {
            216, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "#1",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_stamp.jpg",
            Objective = "Obtain S Rank in Nyanrendo, Dokkan Battle Cats, Guuku Scoop and Panic Honey.",
            IsSecret = true,
            Hint = "Do you like minigames?",
            }
        },
        {
            217, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Caravaneer",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/points.jpg",
            Objective = "Obtain the maximum Caravan points.",
            IsSecret = true,
            Hint = "Carry me Caravan take me away",
            }
        },
        {
            218, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Road Champion",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/tower.jpg",
            Objective = "Get to Multiplayer Road Floor 50.",
            IsSecret = true,
            Hint = "A long road ahead...",
            }
        },
        {
            219, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rengoku",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/tower.jpg",
            Objective = "Get to Multiplayer Road Floor 100.",
            IsSecret = true,
            Hint = "End of the road?",
            }
        },
        {
            220, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Expert Companion",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/hunters.jpg",
            Objective = "Obtain a Max Level partner.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            221, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Solid Determination",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/ticket_silver.jpg",
            Objective = "Attempt a quest 1000 times.",
            IsSecret = true,
            Hint = "You need a strong determination for doing this quest this many times.",
            }
        },
        {
            222, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Compensation",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/ticket_gold.jpg",
            Objective = "Attempt a personal best 100 times.",
            IsSecret = true,
            Hint = "You haven't gotten a new record yet, hopefully this cheers you up.",
            }
        },
        {
            223, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Chilling Monster Count",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/duremudira.jpg",
            Objective = "Defeat 2nd District Duremudira 25 times.",
            IsSecret = true,
            Hint = "You killed that many monsters?!",
            }
        },
        {
            224, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Emperor's Final Roar",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/tower.jpg",
            Objective = "Defeat Road Fatalis 100 times.",
            IsSecret = true,
            Hint = "How many times will this monster roar?!",
            }
        },
        {
            225, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #1",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Click a Fumo in the configuration window.",
            IsSecret = true,
            Hint = "Fumo.",
            }
        },
        {
            226, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Monkey Bomb",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/monster_red2.jpg",
            Objective = "Defeat the Twinhead Rajangs.",
            IsSecret = true,
            Hint = "You will need to go even further beyond in order to beat this quest.",
            }
        },
        {
            227, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Akura Vashimu's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Akura Vashimu quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            228, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Anorupatisu's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Anorupatisu quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            229, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blangonga's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Blangonga quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            230, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Daimyo Hermitaur's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Daimyo Hermitaur quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            231, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Doragyurosu's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Doragyurosu quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            232, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Espinas' Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Espinas quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            233, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gasurabazura's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Gasurabazura quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            234, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Giaorugu's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Giaorugu quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            235, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hypnocatrice's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Hypnocatrice quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            236, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hyujikiki's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Hyujikiki quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            237, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Inagami's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Inagami quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            238, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Khezu's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Khezu quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            239, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Midogaron's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Midogaron quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            240, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Plesioth's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Plesioth quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            241, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rathalos' Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Rathalos quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            242, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rukodiora's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Rukodiora quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            243, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Tigrex's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Tigrex quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            244, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Toridcless' Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Toridcless quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            245, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Baruragaru's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Baruragaru quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            246, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bogabadorumu's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Bogabadorumu quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            247, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gravios' Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Gravios quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            248, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Harudomerugu's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Harudomerugu quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            249, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Taikun Zamuza's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Zenith★4 Taikun Zamuza quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            250, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fatalis' Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Lv9999 Fatalis quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            251, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Crimson Fatalis' Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Lv9999 Crimson Fatalis quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            252, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shantien's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Lv9999 Shantien quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            253, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Disufiroa's Nemesis",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_silver.png",
            Objective = "Complete 1 Lv9999 Disufiroa quest under 10 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            254, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Akura Vashimu's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Akura Vashimu quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            255, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Anorupatisu's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Anorupatisu quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            256, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blangonga's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Blangonga quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            257, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Daimyo Hermitaur's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Daimyo Hermitaur quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            258, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Doragyurosu's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Doragyurosu quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            259, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Espinas' Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Espinas quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            260, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gasurabazura's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Gasurabazura quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            261, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Giaorugu's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Giaorugu quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            262, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hypnocatrice's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Hypnocatrice quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            263, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hyujikiki's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Hyujikiki quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            264, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Inagami's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Inagami quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            265, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Khezu's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Khezu quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            266, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Midogaron's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Midogaron quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            267, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Plesioth's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Plesioth quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            268, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rathalos' Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Rathalos quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            269, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rukodiora's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Rukodiora quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            270, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Tigrex's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Tigrex quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            271, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Toridcless's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Toridcless quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            272, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Baruragaru's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Baruragaru quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            273, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bogabadorumu's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Bogabadorumu quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            274, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gravios's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Gravios quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            275, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Harudomerugu's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Harudomerugu quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            276, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Taikun Zamuza's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Zenith★4 Taikun Zamuza quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            277, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fatalis' Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Lv9999 Fatalis quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            278, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Crimson Fatalis' Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Lv9999 Crimson Fatalis quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            279, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shantien's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Lv9999 Shantien quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            280, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Disufiroa's Archenemy",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png",
            Objective = "Complete 1 Lv9999 Disufiroa quest under 8 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            281, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Akura Vashimu's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Akura Vashimu quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            282, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Anorupatisu's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Anorupatisu quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            283, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blangonga's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Blangonga quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            284, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Daimyo Hermitaur's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Daimyo Hermitaur quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            285, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Doragyurosu's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Doragyurosu quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            286, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Espinas' Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Espinas quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            287, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gasurabazura's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Gasurabazura quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            288, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Giaorugu's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Giaorugu quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            289, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hypnocatrice's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Hypnocatrice quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            290, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Hyujikiki's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Hyujikiki quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            291, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Inagami's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Inagami quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            292, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Khezu's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Khezu quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            293, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Midogaron's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Midogaron quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            294, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Plesioth's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Plesioth quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            295, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rathalos' Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Rathalos quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            296, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rukodiora's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Rukodiora quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            297, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Tigrex's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Tigrex quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            298, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Toridcless' Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Toridcless quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            299, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Baruragaru' Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Baruragaru quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            300, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bogabadorumu's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Bogabadorumu quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            301, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gravios' Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Gravios quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            302, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Harudomerugu's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Harudomerugu quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            303, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Taikun Zamuza's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Zenith★4 Taikun Zamuza quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            304, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fatalis' Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Lv9999 Fatalis quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            305, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Crimson Fatalis' Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Lv9999 Crimson Fatalis quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            306, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shantien's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Lv9999 Shantien quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            307, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Disufiroa's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Lv9999 Disufiroa quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            308, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Pariapuria's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Thirsty Pariapuria quest under 3 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            309, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Mi Ru's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Shifting Mi Ru quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            310, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Guanzorumu's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Ruling Guanzorumu True Slay quest under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            311, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Nargacuga's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Blinking Nargacuga True Slay quest under 7 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            312, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zinogre's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Howling Zinogre True Slay quest under 7 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            313, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zerureusu's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Sparkling Zerureusu True Slay quest under 9 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            314, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Deviljho's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Starving Deviljho True Slay quest under 9 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            315, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Duremudira's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Arrogant Duremudira True Slay quest under 9 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            316, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Come on Big Guy!",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Bombardier Bogabadorumu True Slay quest under 9 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            317, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Elzelion's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Burning Freezing Elzelion True Slay quest under 9 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            318, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "UNKNOWN's Bane",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Upper Shiten Unknown quest under 9 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            319, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bloodthirsty Moon",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/clock.jpg",
            Objective = "Complete 1 Upper Shiten Disufiroa quest under 9 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            320, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Well, that was easy!",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Complete Bingo (Easy).",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            321, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Ramping Up The Difficulty",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Complete Bingo (Medium).",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            322, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bingo!",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Complete Bingo (Hard).",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            323, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "B I N G O",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Complete Bingo (Extreme).",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            324, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shiny!",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/book_guild.png",
            Objective = "Obtain a Gacha Card.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            325, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Getting the hang of it",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/book_guild.png",
            Objective = "Obtain 100 Gacha Cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            326, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Collector",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/book_guild.png",
            Objective = "Obtain 1000 Gacha Cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            327, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Overlord",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/book_guild.png",
            Objective = "Obtain all Gacha Cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            328, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "This is just the beginning",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/hunter.jpg",
            Objective = "Complete 1 Zenith Gauntlet.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            329, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Pinnacle of Hunting",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_silver.png",
            Objective = "Complete 10 Zenith Gauntlets.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            330, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Zenith Exterminator",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_gold.png",
            Objective = "Complete 25 Zenith Gauntlets.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            331, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Speedster",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_platinum.png",
            Objective = "Complete a Zenith Gauntlet under 4 hours.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            332, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Conqueror",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/hunter.jpg",
            Objective = "Complete 1 Solstice Gauntlet.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            333, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Supreme Conqueror",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_silver.png",
            Objective = "Complete 10 Solstice Gauntlets.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            334, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Solstice Conqueror",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_gold.png",
            Objective = "Complete 25 Solstice Gauntlets.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            335, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Marathon Runner",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_platinum.png",
            Objective = "Complete a Solstice Gauntlet under an hour.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            336, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Unstoppable",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/hunter.jpg",
            Objective = "Complete 1 Musou Gauntlet.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            337, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Peerless",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_silver.png",
            Objective = "Complete 10 Musou Gauntlets.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            338, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Musou",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_gold.png",
            Objective = "Complete 25 Musou Gauntlets.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            339, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Speedrunner",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_platinum.png",
            Objective = "Complete a Musou Gauntlet under 100 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            340, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "We Gamin' Bois",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/transcend.jpg",
            Objective = "Enable Discord Rich Presence",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            341, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "No Distractions",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/transcend.jpg",
            Objective = "Enable Zen mode",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            342, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Speed",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/transcend.jpg",
            Objective = "Enable one of the Speedrun modes",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            343, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Yummy",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/plate.png",
            Objective = "Complete 50 quests with Guild Food",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            344, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "More Skills!",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/diva.jpg",
            Objective = "Complete 50 quests with a Diva Skill",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            345, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Best Gallery",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/gem.jpg",
            Objective = "Earn 100,000 or more evaluation points in the gallery competition in My Gallery.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            346, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Monster Hunter",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/book_monster.png",
            Objective = "Hunt 1000 Large Monsters.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            347, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Dedicated",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/book_weapon.png",
            Objective = "Have a total hunt time of 100 hours.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            348, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "20% More Damage for 99% More Effort",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/berserk_raviente.jpg",
            Objective = "Complete a quest with a Z100 weapon.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            349, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Mosswine's Revenge",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_white.jpg",
            Objective = "Find a very peculiar mosswine.",
            IsSecret = true,
            Hint = "I have no gear and I'm hunting a White Fatalis?!",
            }
        },
        {
            350, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Jungle Puzzle",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_white.jpg",
            Objective = "Complete the Jungle Puzzle quest.",
            IsSecret = true,
            Hint = "I found some rocks in this jungle, I wonder what they are for...",
            }
        },
        {
            351, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Poogie's Best Friend",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/poogie.jpg",
            Objective = "Complete 100 quests having used a Poogie item",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            352, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gypceros' Judgment",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_white.jpg",
            Objective = "Defeat Nuclear Gypceros",
            IsSecret = true,
            Hint = "Can you survive the nuclear explosion?",
            }
        },
        {
            353, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Mosswine's Rage",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_white.jpg",
            Objective = "Defeat Mosswine in an arena",
            IsSecret = true,
            Hint = "This mosswine wants a duel.",
            }
        },
        {
            354, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Mosswine's Last Stand",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_black.jpg",
            Objective = "Defeat the mosswine on the top of tower",
            IsSecret = true,
            Hint = "You escaped from the White Fatalis, but can you reach the top back in time?",
            }
        },
        {
            355, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Winter General",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Complete the Halloween Speedster quest",
            IsSecret = true,
            Hint = "Can you place the guild flags on this snowy mountain fast enough?",
            }
        },
        {
            356, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Perfect Bingo",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Complete a bingo run without fainting and with 4 lines being crossed at once.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            357, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bingo Beginner",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Complete a bingo card.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            358, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bingo Enthusiast",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Complete 10 bingo cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            359, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bingo Expert",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Complete 25 bingo cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            360, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Bingo Maniac",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Complete 50 bingo cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            361, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Beginner",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Generate 1 gacha pull.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            362, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Enthusiast",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Generate 10 gacha pulls.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            363, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Expert",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Generate 100 gacha pulls.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            364, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Maniac",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Generate 1000 gacha pulls.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            365, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha God",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Generate 10,000 gacha pulls.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            366, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "B O N U S",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Generate a gacha pull using gacha bonus coins.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            367, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shimmering Coins",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Generate a gacha pull using gacha prismatic coins.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            368, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The First Generation",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all gacha cards from MH1, MH1G and MHF1.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            369, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Second Generation",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all gacha cards from MH2, MHF2 and MHFU.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            370, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Third Generation",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all gacha cards from MH3, MHP3 and MH3U.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            371, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Fourth Generation",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all gacha cards from MH4, MH4U, MHG and MHGU.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            372, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Fifth Generation",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all gacha cards from MHW, MHWI, MHR and MHRS.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            373, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Spinoffs",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all gacha cards from MHXR, MHST and MHST2.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            374, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Online",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all gacha cards from Monster Hunter Online.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            375, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "In Search of a New Frontier",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all gacha cards from Monster Hunter Frontier",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            376, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "PRI",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain a ★1 gacha card.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            377, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "DUO",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 2 ★2 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            378, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "TRI",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 3 ★3 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            379, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "TET",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 4 ★4 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            380, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "PEN",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 5 ★5 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            381, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "HEX",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 6 ★6 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            382, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "HEP",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 7 ★7 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            383, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "OCT",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 8 ★8 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            384, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "NON",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 9 ★9 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            385, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "DEC",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 10 ★10 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            386, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "UND",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 11 ★11 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            387, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "DOD",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain 12 ★12 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            388, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "One Star to Rule Them All",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★1 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            389, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Two Stars Make a Supernova",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★2 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            390, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Alpha Centauri",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★3 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            391, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Cosmic Tesseract",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★4 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            392, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "A 5-Star Collection",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★5 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            393, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Hexagram",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★6 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            394, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "777",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★7 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            395, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Octagram",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★8 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            396, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Enneagram",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★9 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            397, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Ten",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★10 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            398, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Up To Eleven",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★11 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            399, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "12",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain all ★12 gacha cards.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            400, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gacha Bonus",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Complete a gacha bonus quest.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            401, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Solid Determination Up",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_platinum.png",
            Objective = "Complete a Zenith Gauntlet without fainting.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            402, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Gauntlet Conqueror",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_platinum.png",
            Objective = "Complete a Solstice Gauntlet without fainting.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            403, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Ultimate Gauntlet",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/medal_platinum.png",
            Objective = "Complete a Musou Gauntlet without fainting.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            404, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Embodiment of Scarlet Devil",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/Element_Crimson_Demon.png",
            Objective = "Defeat Lv9999 Crimson Fatalis solo without using Terrain Res.",
            IsSecret = true,
            Hint =
@"Eternity cutting through the hot wind,
Immortal flares swaying, wrapped in flames in a dimension that
transcends the boundary between life and death,
the journey of rebirth repeats itself

Into the Lava
Burned by scorching flames
that beautiful bird regains its brilliance

Dive Again It's the soul that endured pain many times,
It's the soul that can grasp eternity,
Just now, reduced to ashes",
            }
        },
        {
            405, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Crushing Palms",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/extreme_poison.png",
            Objective = "Defeat Zenith★4 Gasurabazura solo without Anti-Venom.",
            IsSecret = true,
            Hint = "No cures allowed! But your halk is allowed to help.",
            }
        },
        {
            406, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Twisted Fate",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Obtain a Special Gacha Card with all chance boosters active.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            407, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "UNKNOWN Was Her?",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Defeat Upper Shiten Unknown solo without any items.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            408, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Challenge Accepted",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/stamp.jpg",
            Objective = "Accept a rare gacha challenge.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            409, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "No Cheats Allowed!",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Enter the Konami Code on the start of a quest.",
            IsSecret = true,
            Hint = "Sorry, but that cheat code won't give you 30 more tries on this quest.",
            }
        },
        {
            410, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blinky",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Complete 1 Blinking Nargacuga True Slay quest with a certain Switch Axe F.",
            IsSecret = true,
            Hint = @"You found a book and a weapon.

The book reads:
""For those who are new to this environment, perfection is not something you achieve but something all must eventually fall into the ranks to.

It is a forced compulsion to be better, and that is what's enjoyable about entering this community.

In time, I will steal everything there is to need in order to trivialise content as people have done.

Well, the alternative is to believe you're a failure who will never survive in this game which I am not pessimistic about.

So yuh, it will be worthwhile because inevitably you will be forced to be stronger, even if you have to beat up your body just so that it cooperates.

Well, no merits are earned yet to justify a personality. So in the meantime, being humble and full of self-doubt is correct. At least until you proved your place in this game as, not being a failure.

The only question is how long it'll take to be competent, hmmm.""

You turn the pages...

""I feel like garbage right now, because I haven't done anything. But instead of looking up at those people and being awe-inspired about how they're better than me, I seek to take that for myself. Because that's how you survive in this community, it is a compulsory progression, otherwise you can screw off and die somewhere.

I have the utmost gratitude for people who are strong, but it's also the source of dissatisfaction that I will settle for nothing less than to take that for myself.

Otherwise, crash and burn if I'm not good enough.
I'm sure everyone will do great in the end o wo

As long as they don't quit.

That's why we came here, right? We wouldn't find this place if we err, sucked at MH.""

Having read the inspiring notes, you decide to equip the Black Savage Charaxt and embark on a quest.

During the travel to your destination, the top of the Great Forest, you notice that the weapon seems to have been made before Zeniths were known.",
            }
        },
        {
            411, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "A Very Stylish Hunter",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/hunter.jpg",
            Objective = "Complete a quest with Stylish Assault Up and Stylish Up.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            412, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Rarest Guild Card",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Hunt 100 Ashen Lao-Shan Lung.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            413, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Apparently Immortal Monsters",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Hunt 1 Ashen Lao-Shan Lung, HR3 Yama Tsukami and HR3 Shen Gaoren.",
            IsSecret = true,
            Hint = "These monsters only die when they want to.",
            }
        },
        {
            414, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Obtaining All the Buffs",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Complete 1 Arrogant Duremudira True Slay quest with Secret Technique.",
            IsSecret = true,
            Hint = "Who said it's only useful for the knife attack?",
            }
        },
        {
            415, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Bingo Gauntlet",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_max.png",
            Objective = "Start a bingo run with all gauntlet boosts active.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            416, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Master of Bingo",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Buy all Bingo challenge upgrades.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            417, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Sky's the Limit",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Buy all Sky Corridor challenge upgrades.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            418, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "No more upgrades!",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/zenny.jpg",
            Objective = "Buy all Gacha challenge upgrades.",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            419, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #2", // Patchouli
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Click a fumo in Bingo challenge.",
            IsSecret = true,
            Hint =
@"[] [] [] [] [S]
[] [] [] [O] []
[] [] [M] [] []
[] [U] [] [] []
[F] [] [] [] []",
            Unused = true,
            }
        },
        {
            420, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #3", // Flande
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Click a fumo in Gacha challenge. Or not, it's probably a scam.",
            IsSecret = true,
            Hint = "Fumo is very expensive.",
            Unused = true,
            }
        },
        {
            421, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #4", // Remilia
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Click a fumo in Sky Corridor challenge. Looks like it underestimated your power.",
            IsSecret = true,
            Hint = "Fumo says it's over, it has the high ground.",
            Unused = true,
            }
        },
        {
            422, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #5", // Yuyuko
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Click a fumo in Frontier Compendium website and follow the hints.",
            IsSecret = true,
            Hint = "Get a hint from a fumo, not here!",
            Unused = true,
            }
        },
        {
            423, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "New Game+",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Fully transcend once in a Bingo challenge.",
            IsSecret = true,
            Hint = "There's more to bingo than meets the eye.",
            Unused = true,
            }
        },
        {
            424, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Infinity Gauntlet",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Use the ancient gauntlet to get an infinite amount of points in Sky Corridor. Or at least, try to.",
            IsSecret = true,
            Hint = "There's not one, but multiple gauntlets.",
            Unused = true,
            }
        },
        {
            425, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "No More Heroes in Ruins",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Complete Tome I of the Book of Secrets (Challenge).",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            426, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Dubito, Ergo Cogito, Ergo Sum",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Complete Tome II of the Book of Secrets (Challenge).",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            427, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Friend or Foe",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Complete Tome III of the Book of Secrets (Challenge).",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            428, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Shattered Dimensions",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Complete Tome IV of the Book of Secrets (Challenge).",
            IsSecret = false,
            Hint = string.Empty,
            Unused = true,
            }
        },
        {
            429, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #6", // Tenshi
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Click a fumo in a chest.",
            IsSecret = true,
            Hint = "These chests can hold many hidden treasures.",
            Unused = true,
            }
        },
        {
            430, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #7", // Inaba
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Click a fumo in a box.",
            IsSecret = true,
            Hint = "These boxes can have more than just ammo.",
            Unused = true,
            }
        },
        {
            431, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #8", // Suika
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Click a fumo near a monster.",
            IsSecret = true,
            Hint = "This fumo reminds me of a certain fanged beast.",
            Unused = true,
            }
        },
        {
            432, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #9", // Youmu
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_red.jpg",
            Objective = "Click a fumo inside a wooden chest.",
            IsSecret = true,
            Hint = "Why was it hiding inside a wooden chest?",
            Unused = true,
            }
        },
        {
            433, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #10", // Cirno
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_white.jpg",
            Objective = "Click a fumo in the starry sky.",
            IsSecret = true,
            Hint = "You saw a comet and asked for a wish.",
            Unused = true,
            }
        },
        {
            434, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #11", // Sakuya
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_white.jpg",
            Objective = "Click a fumo near a knife.",
            IsSecret = true,
            Hint = "I wonder why we use transcend burst with the carving knife.",
            Unused = true,
            }
        },
        {
            435, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #12", // Suwako
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_white.jpg",
            Objective = "Click a fumo near a frog.",
            IsSecret = true,
            Hint = "This frog was hiding a secret!",
            Unused = true,
            }
        },
        {
            436, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #13", // Koishi
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_black.jpg",
            Objective = "Click a fumo near a monster's eye.",
            IsSecret = true,
            Hint = "We might find something if we go where these eyes are pointing.",
            Unused = true,
            }
        },
        {
            437, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #14", // Reimu
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_black.jpg",
            Objective = "Click a fumo in Frontier Compendium website and follow the hints.",
            IsSecret = true,
            Hint = "Get a hint from a fumo, not here!",
            Unused = true,
            }
        },
        {
            438, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Fumo #15", // Marisa
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Click a fumo in Sky Corridor Headquarters Entrance.",
            IsSecret = true,
            Hint = "Get a hint from a fumo, not here!",
            Unused = true,
            }
        },
        {
            439, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Back to the Land of Illusions",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Help Marisa go back to her universe.",
            IsSecret = true,
            Hint = "Hopefully we don't have any more dimensional shenanigans.",
            Unused = true,
            }
        },
        {
            440, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Unlimited Power",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_blue.jpg",
            Objective = "Have the power to beat 100 Unlimited monsters.",
            IsSecret = true,
            Hint = "POWER!",
            }
        },
        {
            441, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Muse",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/diva_fountain.png",
            Objective = "Use Diva Song buff in 100 quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            442, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blessed Hunter",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/diva_prayer_gems.png",
            Objective = "Use Diva prayer gems in 777 quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            443, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Oink oink!",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/my_tore.png",
            Objective = "Use guild poogie skill in 100 quests.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            444, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Halk's Friend",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/my_support.png",
            Objective = "Level up your halk to LV3.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            445, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Active Hunter",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Great_Sword_Icon_White.png",
            Objective = "Use the active features of all weapon types.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            446, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "That's a Lotta Damage!",
            Description = string.Empty,
            Rank = AchievementRank.Silver,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/attack_up.png",
            Objective = "Reach the maximum true raw in a quest.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            447, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The Long and Winding Road",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/rengoku.png",
            Objective = "Reach a total of 10,000 road floors completed.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            448, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Over the Hills and Far Away",
            Description = string.Empty,
            Rank = AchievementRank.Gold,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/flame_ul.png",
            Objective = "Complete 1 UL Azure Rathalos quest solo (Speedrun/Zen) in Forest and Hills under 5 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            449, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "PLUS ULTRA",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/hunter.jpg",
            Objective = "Attempt a quest with all run buffs active at once.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            450, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Skill Issue 💀",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/skull.png",
            Objective = "Cart a total of 100 times.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            451, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Start Me Up",
            Description = string.Empty,
            Rank = AchievementRank.Bronze,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gamepad_start.png",
            Objective = "Start the overlay 1,000 times.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            452, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The King of Hell",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Burning Freezing Elzelion solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            453, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Guardian of Tartarus",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Arrogant Duremudira solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            454, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Twin Demons",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Golden Deviljho solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            455, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Cerberus",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Howling Zinogre solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            456, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Rush Up",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Bombardier Bogabadorumu solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            457, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Wyvern Specter",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Blinking Nargacuga solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            458, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Draconian",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Ruling Guanzorumu solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            459, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Metamorphosis",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Shifting Mi Ru solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            460, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Olé!",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Thirsty Pariapuria solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            461, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Sparking!",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Sparkling Zerureusu solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            462, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Sparking! Meteor",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Sparkling Zerureusu solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode) under 6 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            463, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Sparking! Zero",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Sparkling Zerureusu solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode), without prayer gems and under 6 minutes.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            464, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "High Voltage",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Hunting_Horn_Icon_White.png",
            Objective = "Defeat Howling Zinogre solo with Hunting Horn without getting hit (Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            465, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Absolute Carnage",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/knife.png",
            Objective = "Hunt a total of 10,000 large monsters.",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            466, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "The King in Black",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Lv9999 Conquest Fatalis solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            467, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Dragon Slayer",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Lv9999 Conquest Crimson Fatalis solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            468, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Blazing Majesty",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Lv9999 Conquest Shantien solo with Dual Swords without dropping the combo more than 3 times (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            469, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Incognito",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Upper Shiten Unknown solo with Dual Swords without dropping the combo (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
        {
            470, new Achievement()
            {
            CompletionDate = DateTime.UnixEpoch,
            Title = "Exsanguination",
            Description = string.Empty,
            Rank = AchievementRank.Platinum,
            Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/weapon/Dual_Blades_Icon_White.png",
            Objective = "Defeat Upper Shiten Disufiroa solo with Dual Swords without dropping the combo more than 3 times (maximum sharpening buff, Speedrun mode).",
            IsSecret = false,
            Hint = string.Empty,
            }
        },
    });
}
