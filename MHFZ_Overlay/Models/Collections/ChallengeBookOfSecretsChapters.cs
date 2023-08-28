// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MHFZ_Overlay.Models.Structures;

public static class ChallengeBookOfSecretsChapters
{
    public static ReadOnlyDictionary<int, ChallengeBookOfSecretsChapter> IDChapters { get; } = InitializeChapters();

    public static ReadOnlyDictionary<int, ChallengeBookOfSecretsChapter> InitializeChapters()
    {
        var idChapters = new Dictionary<int, ChallengeBookOfSecretsChapter>
        {
            {
                0, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Bingo I",
                    PagesRequired = 0,
                    Description = "Shop: Unlocks Base Score Increase and Base Score Multiplier",
                }
            },
            {
                1, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Felynes",
                    PagesRequired = 0,
                    Description = "Investigation: Research more about Felynes.",
                }
            },
            {
                2, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Weapon Mastery",
                    PagesRequired = 2,
                    Description = "Shop: Unlocks Weapon Type Multiplier.",
                }
            },
            {
                3, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Veggie Elders",
                    PagesRequired = 3,
                    Description = "Investigation: Research more about Veggie Elders.",
                }
            },
            {
                4, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Bingo II",
                    PagesRequired = 10,
                    Description = "Shop: Unlocks Carts Score.",
                }
            },
            {
                5, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Bingo III",
                    PagesRequired = 20,
                    Description = "Shop: Unlocks Bonus Score.",
                }
            },
            {
                6, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Bingo IV",
                    PagesRequired = 25,
                    Description = "Shop: Unlocks Middle Square Multiplier.",
                }
            },
            {
                7, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Bingo V",
                    PagesRequired = 30,
                    Description = "Shop: Unlocks Extra Carts.",
                }
            },
            {
                8, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Bingo VI",
                    PagesRequired = 40,
                    Description = "Shop: Unlocks Starting Cost Reduction.",
                }
            },
            {
                9, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Extreme Style I",
                    PagesRequired = 0,
                    Description = "Shop: Unlocks Middle Square Reroll Chance (Extreme Difficulty).",
                }
            },
            {
                10, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Extreme Style II",
                    PagesRequired = 50,
                    Description = "Shop: Unlocks Burning Freezing Elzelion Rerolls (Extreme Difficulty).",
                }
            },
            {
                11, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Extreme Style III",
                    PagesRequired = 100,
                    Description = "Shop: Unlocks Burning Freezing Elzelion Reroll Chance (Extreme Difficulty).",
                }
            },
            {
                12, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Achievements I",
                    PagesRequired = 60,
                    Description = "Shop: Unlocks Achievements Multiplier.",
                }
            },
            {
                13, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Achievements II",
                    PagesRequired = 70,
                    Description = "Shop: Unlocks Secret Achievements Multiplier.",
                }
            },
            {
                14, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Bingo VII",
                    PagesRequired = 80,
                    Description = "Shop: Unlocks Bingo Completions Multiplier.",
                }
            },
            {
                15, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Monsters I",
                    PagesRequired = 90,
                    Description = "Shop: Unlocks Zenith Square Multiplier.",
                }
            },
            {
                16, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Monsters II",
                    PagesRequired = 100,
                    Description = "Shop: Unlocks Conquest/Shiten Square Multiplier.",
                }
            },
            {
                17, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Monsters III",
                    PagesRequired = 120,
                    Description = "Shop: Unlocks Musou Square Multiplier.",
                }
            },
            {
                18, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Bingo VIII",
                    PagesRequired = 140,
                    Description = "Shop: Unlocks Horizontal Line Bingo Completion Multiplier, Vertical Line Bingo Completion Multiplier and Diagonal Line Bingo Completion Multiplier.",
                }
            },
            {
                19, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Time I",
                    PagesRequired = 0,
                    Description = "Shop: Unlocks Bingo Run Time Completion Multiplier.",
                }
            },
            {
                20, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Scavengers I",
                    PagesRequired = 0,
                    Description = "Shop: Unlocks an upgrade for increasing the chance of finding missing Book of Secrets pages.",
                }
            },
            {
                21, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Scavengers II",
                    PagesRequired = 100,
                    Description = "Shop: Unlocks an upgrade for increasing the chance of finding an ancient dragon part's scraps.",
                }
            },
            {
                22, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Actuaries I",
                    PagesRequired = 0,
                    Description = "Shop: Unlocks an upgrade for gaining more bingo points with compound interest.",
                }
            },
            {
                23, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Actuaries II",
                    PagesRequired = 100,
                    Description = "Shop: Unlocks an upgrade for reducing the cost of upgrades.",
                }
            },
            {
                24, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Transcendence I",
                    PagesRequired = 100,
                    Description = "Investigation: Reseach about an ancient technique.",
                }
            },
            {
                25, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Transcendence II",
                    PagesRequired = 120,
                    Description = "Shop: Unlocks an upgrade for filling the transcend meter faster.",
                }
            },
            {
                26, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Transcendence III",
                    PagesRequired = 140,
                    Description = "Shop: Unlock an upgrade for reducing the cost of a true transcend.",
                }
            },
            {
                27, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Transcendence IV",
                    PagesRequired = 160,
                    Description = "Shop: Unlocks an upgrade for decreasing the rate at which the transcend meter drains.",
                }
            },
            {
                28, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of Time II",
                    PagesRequired = 180,
                    Description = "Shop: Unlocks an upgrade that increases the grace period for obtaining the maximum time score.",
                }
            },
            {
                29, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of the Ancient Civilization I",
                    PagesRequired = 200,
                    Description = "Investigation: Research more about the Ancient Civilization.", // new tab
                }
            },
            {
                30, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of the Ancient Civilization II",
                    PagesRequired = 250,
                    Description = "Investigation: Research more about ancient engineering.", // scraps
                }
            },
            {
                31, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of the Ancient Civilization III",
                    PagesRequired = 300,
                    Description = "Investigation: Research more about an ancient technique.", // true transcend
                }
            },
            {
                32, new ChallengeBookOfSecretsChapter
                {
                    Name = "Knowledge of the Ancient Civilization IV",
                    PagesRequired = 500,
                    Description = "Investigation: Research more about ancient documents.", // blueprint at all 6 parts req.
                }
            },
        };

        idChapters = AddBingoChapters(idChapters);
        idChapters = AddBingoUpgrades(idChapters);
        idChapters = AddBingoRequiredParts(idChapters);

        //chapters["chapter2"].Prerequisites.Add(chapters["chapter1"]);

        return new ReadOnlyDictionary<int, ChallengeBookOfSecretsChapter>(idChapters);
    }

    private static Dictionary<int, ChallengeBookOfSecretsChapter> AddBingoRequiredParts(Dictionary<int, ChallengeBookOfSecretsChapter> idChapters)
    {
        idChapters[9].ChallengeAncientDragonPartsRequired.AddRange(new List<ChallengeAncientDragonPart> { ChallengeAncientDragonParts.TierParts[0][1], });
        idChapters[19].ChallengeAncientDragonPartsRequired.AddRange(new List<ChallengeAncientDragonPart> { ChallengeAncientDragonParts.TierParts[0][4], });
        idChapters[20].ChallengeAncientDragonPartsRequired.AddRange(new List<ChallengeAncientDragonPart> { ChallengeAncientDragonParts.TierParts[0][0], });
        idChapters[22].ChallengeAncientDragonPartsRequired.AddRange(new List<ChallengeAncientDragonPart> { ChallengeAncientDragonParts.TierParts[0][3], });
        idChapters[25].ChallengeAncientDragonPartsRequired.AddRange(new List<ChallengeAncientDragonPart> { ChallengeAncientDragonParts.TierParts[0][4], });
        idChapters[32].ChallengeAncientDragonPartsRequired.AddRange(new List<ChallengeAncientDragonPart> { ChallengeAncientDragonParts.TierParts[0][0], ChallengeAncientDragonParts.TierParts[0][1], ChallengeAncientDragonParts.TierParts[0][2], ChallengeAncientDragonParts.TierParts[0][3], ChallengeAncientDragonParts.TierParts[0][4], ChallengeAncientDragonParts.TierParts[0][5], });

        return idChapters;
    }

        private static Dictionary<int, ChallengeBookOfSecretsChapter> AddBingoChapters(Dictionary<int, ChallengeBookOfSecretsChapter> idChapters)
    {
        idChapters[0].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[2], idChapters[4] });
        idChapters[1].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[3], });

        idChapters[3].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[29], });

        idChapters[4].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[5], });
        idChapters[5].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[6], });
        idChapters[6].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[7] });
        idChapters[7].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[8] });

        idChapters[9].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[10] });
        idChapters[10].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[11] });

        idChapters[7].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[12] });
        idChapters[12].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[13] });

        idChapters[7].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[15] });
        idChapters[15].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[16] });
        idChapters[16].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[17] });

        idChapters[8].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[14] });
        idChapters[14].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[18] });

        idChapters[19].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[28] });

        idChapters[20].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[21] });

        idChapters[22].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[23] });

        idChapters[29].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[24], idChapters[30], });

        idChapters[24].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[25], });
        idChapters[25].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[26], });
        idChapters[26].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[27], });

        idChapters[30].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[31], });
        idChapters[31].UnlockedChapters.AddRange(new List<ChallengeBookOfSecretsChapter> { idChapters[32], });

        return idChapters;
    }

    private static Dictionary<int, ChallengeBookOfSecretsChapter> AddBingoUpgrades(Dictionary<int, ChallengeBookOfSecretsChapter> idChapters)
    {
        idChapters[0].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[0], BingoUpgrades.IDBingoUpgrade[1] });
        idChapters[2].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[2], });
        idChapters[4].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[3], });
        idChapters[5].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[4], });
        idChapters[6].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[5], });
        idChapters[7].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[6], });
        idChapters[8].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[7], });
        idChapters[9].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[8], });
        idChapters[10].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[9], });
        idChapters[11].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[10], });
        idChapters[12].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[11], });
        idChapters[13].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[12], });
        idChapters[14].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[13], });
        idChapters[15].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[14], });
        idChapters[16].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[15], });
        idChapters[17].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[16], });
        idChapters[18].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[17], BingoUpgrades.IDBingoUpgrade[18], BingoUpgrades.IDBingoUpgrade[19], });
        idChapters[19].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[20], });
        idChapters[20].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[21], });
        idChapters[21].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[22], });
        idChapters[22].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[23], });
        idChapters[23].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[24], });

        idChapters[25].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[25], });
        idChapters[26].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[26], });
        idChapters[27].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[27], });
        idChapters[28].UnlockedUpgrades.AddRange(new List<BingoUpgrade> { BingoUpgrades.IDBingoUpgrade[28], });

        return idChapters;
    }
}
