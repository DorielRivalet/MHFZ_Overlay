// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;

public static class Challenges
{
    public static List<Challenge> ChallengeList { get; private set; } = new List<Challenge>
    {
        {
            new Challenge()
            {
                BannerImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_black.jpg",
                Name = "Bingo",
                AchievementIDRequired = 214, // Zenny Galore
                AchievementNameRequired = Achievements.IDAchievement[214].Title,
                AchievementsBronzeRequired = 10,
                AchievementsSilverRequired = 5,
                AchievementsGoldRequired = 1,
                AchievementsPlatinumRequired = 0,
                ChallengeDataTemplateKey = null,
                Description =
@"CURRENTLY UNAVAILABLE. You are presented with a variable sized grid depending on the bingo difficulty. In each grid is a monster icon which upon hunting marks the square as completed.

Try to complete a fully diagonal, vertical or horizontal line in order to finish the challenge, and strive for the maximum bingo points!",
            }
        },
        {
            new Challenge()
            {
                BannerImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_black.jpg",
                Name = "Gacha",
                AchievementIDRequired = 212, // Quiz Time!
                AchievementNameRequired = Achievements.IDAchievement[212].Title,
                AchievementsBronzeRequired = 20,
                AchievementsSilverRequired = 10,
                AchievementsGoldRequired = 5,
                AchievementsPlatinumRequired = 1,
                ChallengeDataTemplateKey = null,
                Description = "CURRENTLY UNAVAILABLE. This is a minigame where you collect gacha cards and buy many sorts of upgrades with gacha coins (of the overlay) in order to help you get more cards! Can you get every single card?",
            }
        },
        {
            new Challenge()
            {
                BannerImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_black.jpg",
                Name = "Zenith Gauntlet",
                AchievementIDRequired = 405, // Crushing Palms
                AchievementNameRequired = Achievements.IDAchievement[405].Title,
                AchievementsBronzeRequired = 50,
                AchievementsSilverRequired = 20,
                AchievementsGoldRequired = 10,
                AchievementsPlatinumRequired = 5,
                ChallengeDataTemplateKey = null,
                Description = "CURRENTLY UNAVAILABLE. A challenge only for the best of hunters. You have to hunt all 23 Zenith★4 monsters in any order you want, all in one go! Includes a real-time timer.",
            }
        },
        {
            new Challenge()
            {
                BannerImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_black.jpg",
                Name = "Solstice Gauntlet",
                AchievementIDRequired = 404, // The Embodiment of Scarlet Devil
                AchievementNameRequired = Achievements.IDAchievement[404].Title,
                AchievementsBronzeRequired = 60,
                AchievementsSilverRequired = 50,
                AchievementsGoldRequired = 20,
                AchievementsPlatinumRequired = 10,
                ChallengeDataTemplateKey = null,
                Description = "CURRENTLY UNAVAILABLE. A supreme challenge that tests the limits of even the most skilled hunters. This challenge demands the defeat of all Conquest Lv9999 and Upper Shiten monsters in a relentless, back-to-back confrontation. Only those who can conquer this formidable trial in one go shall earn the right to claim victory under the Bloodthirsty Moon.",
            }
        },
        {
            new Challenge()
            {
                BannerImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/unknown_black.jpg",
                Name = "Musou Gauntlet",
                AchievementIDRequired = 195, // Seriously Thirsty
                AchievementNameRequired = Achievements.IDAchievement[195].Title,
                AchievementsBronzeRequired = 70,
                AchievementsSilverRequired = 60,
                AchievementsGoldRequired = 50,
                AchievementsPlatinumRequired = 20,
                ChallengeDataTemplateKey = null,
                Description =
@"CURRENTLY UNAVAILABLE. Embark on a herculean trial that pushes hunters to the brink of their abilities. This challenge commands the defeat of all Musou monsters in an unyielding, consecutive battle.

This merciless gauntlet demands more than mere strength; it calls for tactical finesse and the will to triumph against insurmountable odds. Only those who can surmount this grueling challenge shall earn the title of Unstoppable, showcasing their prowess to the world.",
            }
        },
    };
}
