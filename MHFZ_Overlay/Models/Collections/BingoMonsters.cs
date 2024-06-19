// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;   

/// <summary>
/// The bingo monster difficulty list.
/// </summary>
public static class BingoMonsters
{
    public static ReadOnlyDictionary<Difficulty, List<BingoMonster>> DifficultyBingoMonster { get; } = new (new Dictionary<Difficulty, List<BingoMonster>>
    {
        // Extreme difficulty is same as hard difficulty but the bingo board is twice as big.
        // Custom quests and event quests for monsters that aren't event-only are not counted.
        {
            Difficulty.Easy, new List<BingoMonster>
            {
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/elzelion.png",
                    Name = "Elzelion",
                    QuestIDs = new List<int> { 23626, },
                    BaseScore = 10,
                    Type = FrontierMonsterType.ElderDragon,
                },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zinogre.png",
                        Name = "Zinogre",
                        QuestIDs = new List<int> {  23499,  },
                        BaseScore = 8,
                        Type = FrontierMonsterType.FangedWyvern,
                    }, // zin
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/seregios.png",
                        Name = "Seregios",
                        QuestIDs = new List<int> {  23667, },
                        BaseScore = 9,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // seregios
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/nargacuga.png",
                        Name = "Nargacuga",
                        QuestIDs = new List<int> {  23494, },
                        BaseScore = 5,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // narga
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/uragaan.png",
                        Name = "Uragaan",
                        QuestIDs = new List<int> {  23495, },
                        BaseScore = 6,
                        Type = FrontierMonsterType.BruteWyvern,
                    }, // uragaan
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shagaru_magala.png",
                        Name = "Shagaru Magala",
                        QuestIDs = new List<int> {  23528, },
                        BaseScore = 9,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // shagaru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gore_magala.png",
                        Name = "Gore Magala",
                        QuestIDs = new List<int> {  23513,  },
                        BaseScore = 5,
                        Type = FrontierMonsterType.Other,
                    }, // gore
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/amatsu.png",
                        Name = "Amatsu",
                        QuestIDs = new List<int> {  23643,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // amatsu
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/mi_ru.png",
                        Name = "Mi Ru",
                        QuestIDs = new List<int> {  54244,  },
                        BaseScore = 5,
                        Type = FrontierMonsterType.Other,
                    }, // mi ru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/barioth.png",
                        Name = "Barioth",
                        QuestIDs = new List<int> {  23496,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // barioth
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/brachydios.png",
                        Name = "Brachydios",
                        QuestIDs = new List<int> {  23497,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.BruteWyvern,
                    }, // brachy
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/deviljho.png",
                        Name = "Deviljho",
                        QuestIDs = new List<int> {  23498,  },
                        BaseScore = 5,
                        Type = FrontierMonsterType.BruteWyvern,
                    }, // jho
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/diorex.png",
                        Name = "Diorex",
                        QuestIDs = new List<int> {  23490,  },
                        BaseScore = 1,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // diorex
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/garuba_daora.png",
                        Name = "Garuba Daora",
                        QuestIDs = new List<int> {  23489,  },
                        BaseScore = 8,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // garuba
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/varusaburosu.png",
                        Name = "Varusaburosu",
                        QuestIDs = new List<int> {  23488,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // varusa
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gureadomosu.png",
                        Name = "Gureadomosu",
                        QuestIDs = new List<int> {  23487,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // gurea
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/toa_tesukatora.png",
                        Name = "Toa Tesukatora",
                        QuestIDs = new List<int> {  23485,  },
                        BaseScore = 9,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // toa
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/yama_kurai.png",
                        Name = "Yama Kurai",
                        QuestIDs = new List<int> {  23486,  },
                        BaseScore = 10,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // kurai
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zerureusu.png",
                        Name = "Zerureusu",
                        QuestIDs = new List<int> {  23492,  },
                        BaseScore = 4,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // zeru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/meraginasu.png",
                        Name = "Meraginasu",
                        QuestIDs = new List<int> {  23491,  },
                        BaseScore = 5,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // mera
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/voljang.png",
                        Name = "Voljang",
                        QuestIDs = new List<int> {  23484,  },
                        BaseScore = 8,
                        Type = FrontierMonsterType.FangedBeast,
                    }, // voljang
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/stygian_zinogre.png",
                        Name = "Stygian Zinogre",
                        QuestIDs = new List<int> {  23493,  },
                        BaseScore = 9,
                        Type = FrontierMonsterType.FangedWyvern,
                    }, // stygian
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/thirsty_pariapuria.png",
                        Name = "Thirsty Pariapuria",
                        QuestIDs = new List<int> { Numbers.QuestIDThirstyPariapuria,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/ruling_guanzorumu.png",
                        Name = "Ruling Guanzorumu",
                        QuestIDs = new List<int> { Numbers.QuestIDRulingGuanzorumu,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/keoaruboru.png",
                        Name = "Keoaruboru",
                        QuestIDs = new List<int> {  58043,  },
                        BaseScore = 3,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // keo
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/guanzorumu.png",
                        Name = "Guanzorumu",
                        QuestIDs = new List<int> {  23424,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // guanzo
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_shantien.png",
                        Name = "Lv1000 Shantien",
                        QuestIDs = new List<int> {  23587,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // lv1000 shantien
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/disufiroa.png",
                        Name = "Lv1000 Disufiroa",
                        QuestIDs = new List<int> {  23591,  },
                        BaseScore = 13,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // lv1000 disu
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_fatalis.png",
                        Name = "Lv1000 Fatalis",
                        QuestIDs = new List<int> {  23595,  },
                        BaseScore = 11,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // lv1000 fatalis
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_crimson_fatalis.png",
                        Name = "Lv1000 Crimson Fatalis",
                        QuestIDs = new List<int> {  23599,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // lv1000 crimson
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shiten_disufiroa.png",
                        Name = "Lower Shiten Disufiroa",
                        QuestIDs = new List<int> {  23602,  },
                        BaseScore = 15,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // lower shiten disu
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shiten_unknown.png",
                        Name = "Lower Shiten Unknown",
                        QuestIDs = new List<int> {  23604,  },
                        BaseScore = 10,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // lower shiten unknown
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_akura_vashimu.gif",
                        Name = "Zenith★1 Akura Vashimu",
                        QuestIDs = new List<int> {  23536,  },
                        BaseScore = 7,
                        Type = FrontierMonsterType.Carapaceon,
                    }, // z1+2 akura
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_akura_vashimu.gif",
                        Name = "Zenith★2 Akura Vashimu",
                        QuestIDs = new List<int> {  23537,  },
                        BaseScore = 14,
                        Type = FrontierMonsterType.Carapaceon,
                    }, 
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_anorupatisu.gif",
                        Name = "Zenith★1 Anorupatisu",
                        QuestIDs = new List<int> {  23718,  },
                        BaseScore = 9,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // z1+2 anoru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_anorupatisu.gif",
                        Name = "Zenith★2 Anorupatisu",
                        QuestIDs = new List<int> {  23719,  },
                        BaseScore = 18,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_inagami.gif",
                        Name = "Zenith★1 Inagami",
                        QuestIDs = new List<int> {  23644,  },
                        BaseScore = 10,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // z1 inagami
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_inagami.gif",
                        Name = "Zenith★2 Inagami",
                        QuestIDs = new List<int> {  23645,  },
                        BaseScore = 20,
                        Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_espinas.gif",
                        Name = "Zenith★1 Espinas",
                        QuestIDs = new List<int> {  23480,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // z1 espi
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_espinas.gif",
                        Name = "Zenith★2 Espinas",
                        QuestIDs = new List<int> {  23481,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gasurabazura.gif",
                        Name = "Zenith★1 Gasurabazura",
                        QuestIDs = new List<int> {  23668,  },
                        BaseScore = 9,
                        Type = FrontierMonsterType.BruteWyvern,
                    }, // z1 gasura
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gasurabazura.gif",
                        Name = "Zenith★2 Gasurabazura",
                        QuestIDs = new List<int> {  23669,  },
                        BaseScore = 18,
                        Type = FrontierMonsterType.BruteWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_plesioth.gif",
                        Name = "Zenith★1 Plesioth",
                        QuestIDs = new List<int> {  23622,  },
                        BaseScore = 8,
                        Type = FrontierMonsterType.PiscineWyvern,
                    }, // z1 plesioth
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_plesioth.gif",
                        Name = "Zenith★2 Plesioth",
                        QuestIDs = new List<int> {  23623,  },
                        BaseScore = 16,
                        Type = FrontierMonsterType.PiscineWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_giaorugu.gif",
                        Name = "Zenith★1 Giaorugu",
                        QuestIDs = new List<int> {  23610,  },
                        BaseScore = 7,
                        Type = FrontierMonsterType.BruteWyvern,
                    }, // z1 giao
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_giaorugu.gif",
                        Name = "Zenith★2 Giaorugu",
                        QuestIDs = new List<int> {  23611,  },
                        BaseScore = 14,
                        Type = FrontierMonsterType.BruteWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gravios.gif",
                        Name = "Zenith★1 Gravios",
                        QuestIDs = new List<int> {  23709,  },
                        BaseScore = 9,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // z1 gravios
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gravios.gif",
                        Name = "Zenith★2 Gravios",
                        QuestIDs = new List<int> {  23710,  },
                        BaseScore = 18,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_daimyo_hermitaur.gif",
                        Name = "Zenith★1 Daimyo Hermitaur",
                        QuestIDs = new List<int> {  23476,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.Carapaceon,
                    }, // z1 daimyo
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_daimyo_hermitaur.gif",
                        Name = "Zenith★2 Daimyo Hermitaur",
                        QuestIDs = new List<int> {  23477,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.Carapaceon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_tigrex.gif",
                        Name = "Zenith★1 Tigrex",
                        QuestIDs = new List<int> {  23540,  },
                        BaseScore = 5,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // z1 tigrex
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_tigrex.gif",
                        Name = "Zenith★2 Tigrex",
                        QuestIDs = new List<int> {  23541,  },
                        BaseScore = 10,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_blangonga.gif",
                        Name = "Zenith★1 Blangonga",
                        QuestIDs = new List<int> {  23516,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.FangedBeast,
                    }, // z1 blango
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_blangonga.gif",
                        Name = "Zenith★2 Blangonga",
                        QuestIDs = new List<int> {  23517,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.FangedBeast,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_doragyurosu.gif",
                        Name = "Zenith★1 Doragyurosu",
                        QuestIDs = new List<int> {  23659,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // z1 dora
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_doragyurosu.gif",
                        Name = "Zenith★2 Doragyurosu",
                        QuestIDs = new List<int> {  23660,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_toridcless.gif",
                        Name = "Zenith★1 Toridcless",
                        QuestIDs = new List<int> {  23655,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.BirdWyvern,
                    }, // z1 torid
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_toricless.gif",
                        Name = "Zenith★2 Toricless",
                        QuestIDs = new List<int> {  23656,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_baruragaru.gif",
                        Name = "Zenith★1 Baruragaru",
                        QuestIDs = new List<int> {  23713,  },
                        BaseScore = 9,
                        Type = FrontierMonsterType.Leviathan,
                    }, // z1 baru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_baruragaru.gif",
                        Name = "Zenith★2 Baruragaru",
                        QuestIDs = new List<int> {  23714,  },
                        BaseScore = 18,
                        Type = FrontierMonsterType.Leviathan,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hypnoc.gif",
                        Name = "Zenith★1 Hypnocatrice",
                        QuestIDs = new List<int> {  23468,  },
                        BaseScore = 7,
                        Type = FrontierMonsterType.BirdWyvern,
                    }, // z1 hypnoc
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hypnoc.gif",
                        Name = "Zenith★2 Hypnocatrice",
                        QuestIDs = new List<int> {  23469,  },
                        BaseScore = 14,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hyujikiki.gif",
                        Name = "Zenith★1 Hyujikiki",
                        QuestIDs = new List<int> {  23606,  },
                        BaseScore = 8,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // z1 hyuji
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hyujikiki.gif",
                        Name = "Zenith★2 Hyujikiki",
                        QuestIDs = new List<int> {  23607,  },
                        BaseScore = 16,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_khezu.gif",
                        Name = "Zenith★1 Khezu",
                        QuestIDs = new List<int> {  23472,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // z1 khezu
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_khezu.gif",
                        Name = "Zenith★2 Khezu",
                        QuestIDs = new List<int> {  23473,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_bogabadorumu.gif",
                        Name = "Zenith★1 Bogabadorumu",
                        QuestIDs = new List<int> {  23705,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // z1 boggy
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_bogabadorumu.gif",
                        Name = "Zenith★2 Bogabadorumu",
                        QuestIDs = new List<int> {  23706,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_midogaron.gif",
                        Name = "Zenith★1 Midogaron",
                        QuestIDs = new List<int> {  23614,  },
                        BaseScore = 7,
                        Type = FrontierMonsterType.FangedBeast,
                    }, // z1 mido
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_midogaron.gif",
                        Name = "Zenith★2 Midogaron",
                        QuestIDs = new List<int> {  23615,  },
                        BaseScore = 14,
                        Type = FrontierMonsterType.FangedBeast,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rathalos.gif",
                        Name = "Zenith★1 Rathalos",
                        QuestIDs = new List<int> {  23520,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.FlyingWyvern,
                    }, // z1 rath
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rathalos.gif",
                        Name = "Zenith★2 Rathalos",
                        QuestIDs = new List<int> {  23521,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rukodiora.gif",
                        Name = "Zenith★1 Rukodiora",
                        QuestIDs = new List<int> {  23618,  },
                        BaseScore = 9,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // z1 ruko
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rukodiora.gif",
                        Name = "Zenith★2 Rukodiora",
                        QuestIDs = new List<int> {  23619,  },
                        BaseScore = 18,
                        Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_taikun_zamuza.gif",
                        Name = "Zenith★1 Taikun Zamuza",
                        QuestIDs = new List<int> {  55923,  },
                        BaseScore = 9,
                        Type = FrontierMonsterType.Carapaceon,
                    }, // z1 taikun
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_taikun_zamuza.gif",
                        Name = "Zenith★2 Taikun Zamuza",
                        QuestIDs = new List<int> {  55924,  },
                        BaseScore = 18,
                        Type = FrontierMonsterType.Carapaceon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_harudomerugu.gif",
                        Name = "Zenith★1 Harudomerugu",
                        QuestIDs = new List<int> {  55929,  },
                        BaseScore = 6,
                        Type = FrontierMonsterType.ElderDragon,
                    }, // z1 harudo
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_harudomerugu.gif",
                        Name = "Zenith★2 Harudomerugu",
                        QuestIDs = new List<int> {  55930,  },
                        BaseScore = 12,
                        Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/duremudira.png",
                        Name = "1st District Duremudira",
                        QuestIDs = new List<int> {  Numbers.QuestIDFirstDistrictDuremudira,  },
                        BaseScore = 15,
                        Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/yian_kut-ku.png",
                        Name = "UL Yian Kut-Ku",
                        BaseScore = 16,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 6,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/lavasioth.png",
                        Name = "UL Lavasioth",
                        BaseScore = 18,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 75,
                        Type = FrontierMonsterType.PiscineWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gypceros.png",
                        Name = "UL Gypceros",
                        BaseScore = 18,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 20,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gougarf.png",
                        Name = "UL Gougarf",
                        BaseScore = 20,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 123,
                        Type = FrontierMonsterType.FangedBeast,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gogomoa.png",
                        Name = "UL Gogomoa",
                        BaseScore = 18,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 101,
                        Type = FrontierMonsterType.FangedBeast,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shogun_ceanataur.png",
                        Name = "UL Shogun Ceanataur",
                        BaseScore = 18,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 67,
                        Type = FrontierMonsterType.Carapaceon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/dyuragaua.png",
                        Name = "UL Dyuragaua",
                        BaseScore = 14,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 94,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/cephadrome.png",
                        Name = "UL Cephadrome",
                        BaseScore = 14,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 8,
                        Type = FrontierMonsterType.PiscineWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/bulldrome.png",
                        Name = "UL Bulldrome",
                        BaseScore = 12,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 68,
                        Type = FrontierMonsterType.FangedBeast,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/basarios.png",
                        Name = "UL Basarios",
                        BaseScore = 18,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 22,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/congalala.png",
                        Name = "UL Congalala",
                        BaseScore = 16,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 52,
                        Type = FrontierMonsterType.FangedBeast,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/pokaradon.png",
                        Name = "UL Pokaradon",
                        BaseScore = 16,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 115,
                        Type = FrontierMonsterType.Leviathan,
                    },
            }
        },
        {
            Difficulty.Medium, new List<BingoMonster>
            {
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_akura_vashimu.gif",
                    Name = "Zenith★3 Akura Vashimu",
                    QuestIDs = new List<int> {  23538,  },
                    BaseScore = 28,
                    Type = FrontierMonsterType.Carapaceon,
                    }, // z3 akura
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_anorupatisu.gif",
                    Name = "Zenith★3 Anorupatisu",
                    QuestIDs = new List<int> {  23720,  },
                    BaseScore = 36,
                    Type = FrontierMonsterType.FlyingWyvern,
                    }, // z3 anoru
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_inagami.gif",
                    Name = "Zenith★3 Inagami",
                    QuestIDs = new List<int> {  23646,  },
                    BaseScore = 40,
                    Type = FrontierMonsterType.ElderDragon,
                    }, // z3 inagami
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_espinas.gif",
                    Name = "Zenith★3 Espinas",
                    QuestIDs = new List<int> {  23482,  },
                    BaseScore = 24,
                    Type = FrontierMonsterType.FlyingWyvern,
                    }, // z3 espi
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gasurabazura.gif",
                    Name = "Zenith★3 Gasurabazura",
                    QuestIDs = new List<int> {  23670,  },
                    BaseScore = 36,
                    Type = FrontierMonsterType.BruteWyvern,
                    }, // z3 gasura
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_plesioth.gif",
                    Name = "Zenith★3 Plesioth",
                    QuestIDs = new List<int> {  23624,  },
                    BaseScore = 32,
                    Type = FrontierMonsterType.PiscineWyvern,
                    }, // z3 plesioth
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_giaorugu.gif",
                    Name = "Zenith★3 Giaorugu",
                    QuestIDs = new List<int> {  23612,  },
                    BaseScore = 28,
                    Type = FrontierMonsterType.BruteWyvern,
                    }, // z3 giao
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gravios.gif",
                     Name = "Zenith★3 Gravios",
                     QuestIDs = new List<int> {  23711,  },
                     BaseScore = 36,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z3 gravios
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_daimyo_hermitaur.gif",
                     Name = "Zenith★3 Daimyo Hermitaur",
                     QuestIDs = new List<int> {  23478,  },
                     BaseScore = 24,
                     Type = FrontierMonsterType.Carapaceon,
                    }, // z3 daimyo
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_tigrex.gif",
                     Name = "Zenith★3 Tigrex",
                     QuestIDs = new List<int> {  23542,  },
                     BaseScore = 20,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z3 tigrex
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_blangonga.gif",
                     Name = "Zenith★3 Blangonga",
                     QuestIDs = new List<int> {  23518,  },
                     BaseScore = 24,
                     Type = FrontierMonsterType.FangedBeast,
                    }, // z3 blango
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_doragyurosu.gif",
                     Name = "Zenith★3 Doragyurosu",
                     QuestIDs = new List<int> {  23661,  },
                     BaseScore = 24,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z3 dora
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_toridcless.gif",
                     Name = "Zenith★3 Toridcless",
                     QuestIDs = new List<int> {  23657,  },
                     BaseScore = 24,
                     Type = FrontierMonsterType.BirdWyvern,
                    }, // z3 torid
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_baruragaru.gif",
                     Name = "Zenith★3 Baruragaru",
                     QuestIDs = new List<int> {  23715,  },
                     BaseScore = 36,
                     Type = FrontierMonsterType.Leviathan,
                    }, // z3 baru
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hypnoc.gif",
                     Name = "Zenith★3 Hypnocatrice",
                     QuestIDs = new List<int> {  23470,  },
                     BaseScore = 28,
                     Type = FrontierMonsterType.BirdWyvern,
                    }, // z3 hypnoc
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hyujikiki.gif",
                     Name = "Zenith★3 Hyujikiki",
                     QuestIDs = new List<int> {  23608,  },
                     BaseScore = 32,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z3 hyuji
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_khezu.gif",
                     Name = "Zenith★3 Khezu",
                     QuestIDs = new List<int> {  23474,  },
                     BaseScore = 24,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z3 khezu
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_bogabadorumu.gif",
                     Name = "Zenith★3 Bogabadorumu",
                     QuestIDs = new List<int> {  23707,  },
                     BaseScore = 24,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z3 boggy
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_midogaron.gif",
                     Name = "Zenith★3 Midogaron",
                     QuestIDs = new List<int> {  23616,  },
                     BaseScore = 28,
                     Type = FrontierMonsterType.FangedBeast,
                    }, // z3 mido
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rathalos.gif",
                     Name = "Zenith★3 Rathalos",
                     QuestIDs = new List<int> {  23522,  },
                     BaseScore = 24,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z3 rath
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rukodiora.gif",
                     Name = "Zenith★3 Rukodiora",
                     QuestIDs = new List<int> {  23620,  },
                     BaseScore = 36,
                     Type = FrontierMonsterType.ElderDragon,
                    }, // z3 ruko
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_taikun_zamuza.gif",
                     Name = "Zenith★3 Taikun Zamuza",
                     QuestIDs = new List<int> {  55925,  },
                     BaseScore = 36,
                     Type = FrontierMonsterType.Carapaceon,
                    }, // z3 taikun
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_harudomerugu.gif",
                     Name = "Zenith★3 Harudomerugu",
                     QuestIDs = new List<int> {  55931,  },
                     BaseScore = 24,
                     Type = FrontierMonsterType.ElderDragon,
                    }, // z3 harudo
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/starving_deviljho.png",
                     Name = "Starving Deviljho",
                     QuestIDs = new List<int> {  55916,  },
                     BaseScore = 30,
                     Type = FrontierMonsterType.BruteWyvern,
                    }, // starving jho
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shifting_mi_ru.png",
                    Name = "Shifting Mi Ru",
                    QuestIDs = new List<int> {  Numbers.QuestIDShiftingMiRu, Numbers.QuestIDShiftingMiRuHistoric, },
                    BaseScore = 30,
                    Type = FrontierMonsterType.Other,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/twinhead_rajang.png",
                    Name = "Twinhead Rajangs",
                    QuestIDs = new List<int> {  Numbers.QuestIDTwinheadRajangsHistoric,  },
                    BaseScore = 20,
                    Type = FrontierMonsterType.FangedBeast,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_fatalis.png",
                    Name = "Lv9999 Fatalis",
                    QuestIDs = new List<int> {  Numbers.QuestIDLV9999Fatalis,  },
                    BaseScore = 35,
                    Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_crimson_fatalis.png",
                    Name = "Lv9999 Crimson Fatalis",
                    QuestIDs = new List<int> {  Numbers.QuestIDLV9999CrimsonFatalis,  },
                    BaseScore = 40,
                    Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/disufiroa.png",
                    Name = "Lv9999 Disufiroa",
                    QuestIDs = new List<int> {  Numbers.QuestIDLV9999Disufiroa,  },
                    BaseScore = 40,
                    Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_shantien.png",
                    Name = "Lv9999 Shantien",
                    QuestIDs = new List<int> {  Numbers.QuestIDLV9999Shantien,  },
                    BaseScore = 40,
                    Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/duremudira.png",
                    Name = "2nd Distric Duremudira",
                    QuestIDs = new List<int> {  Numbers.QuestIDSecondDistrictDuremudira,  },
                    BaseScore = 40,
                    Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/azure_rathalos.png",
                        Name = "UL Azure Rathalos",
                        BaseScore = 30,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 49,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/pink_rathian.png",
                        Name = "UL Pink Rathian",
                        BaseScore = 28,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 37,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/bright_hypnoc.png",
                        Name = "UL Bright Hypnoc",
                        BaseScore = 26,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 78,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/purple_gypceros.png",
                        Name = "UL Purple Gypceros",
                        BaseScore = 26,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 39,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/green_plesioth.png",
                        Name = "UL Green Plesioth",
                        BaseScore = 30,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 46,
                        Type = FrontierMonsterType.PiscineWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/blue_yian_kut-ku.png",
                        Name = "UL Blue Yian Kut-Ku",
                        BaseScore = 24,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 38,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/akura_jebia.png",
                        Name = "UL Akura Jebia",
                        BaseScore = 30,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 84,
                        Type = FrontierMonsterType.Carapaceon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/orange_espinas.png",
                        Name = "UL Orange Espinas",
                        BaseScore = 40,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 81,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
            }
        },
        {
            Difficulty.Hard, new List<BingoMonster>
            {
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_akura_vashimu.gif",
                    Name = "Zenith★4 Akura Vashimu",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4AkuraVashimu,  },
                    BaseScore = 56,
                    Type = FrontierMonsterType.Carapaceon,
                    }, // z4 akura
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_anorupatisu.gif",
                    Name = "Zenith★4 Anorupatisu",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Anorupatisu,  },
                    BaseScore = 72,
                    Type = FrontierMonsterType.FlyingWyvern,
                    }, // z4 anoru
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_inagami.gif",
                    Name = "Zenith★4 Inagami",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Inagami,  },
                    BaseScore = 80,
                    Type = FrontierMonsterType.ElderDragon,
                    }, // z4 inagami
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_espinas.gif",
                    Name = "Zenith★4 Espinas",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Espinas,  },
                    BaseScore = 48,
                    Type = FrontierMonsterType.FlyingWyvern,
                    }, // z4 espi
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gasurabazura.gif",
                    Name = "Zenith★4 Gasurabazura",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Gasurabazura,  },
                    BaseScore = 80,
                    Type = FrontierMonsterType.BruteWyvern,
                    }, // z4 gasura
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_plesioth.gif",
                    Name = "Zenith★4 Plesioth",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Plesioth,  },
                    BaseScore = 64,
                    Type = FrontierMonsterType.PiscineWyvern,
                    }, // z4 plesioth
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_giaorugu.gif",
                    Name = "Zenith★4 Giaorugu",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Giaorugu,  },
                    BaseScore = 56,
                    Type = FrontierMonsterType.BruteWyvern,
                    }, // z4 giao
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gravios.gif",
                     Name = "Zenith★4 Gravios",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Gravios,  },
                     BaseScore = 72,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z4 gravios
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_daimyo_hermitaur.gif",
                     Name = "Zenith★4 Daimyo Hermitaur",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4DaimyoHermitaur,  },
                     BaseScore = 48,
                     Type = FrontierMonsterType.Carapaceon,
                    }, // z4 daimyo
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_tigrex.gif",
                     Name = "Zenith★4 Tigrex",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Tigrex,  },
                     BaseScore = 40,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z4 tigrex
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_blangonga.gif",
                     Name = "Zenith★4 Blangonga",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Blangonga,  },
                     BaseScore = 48,
                     Type = FrontierMonsterType.FangedBeast,
                    }, // z4 blango
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_doragyurosu.gif",
                     Name = "Zenith★4 Doragyurosu",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Doragyurosu,  },
                     BaseScore = 48,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z4 dora
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_toridcless.gif",
                     Name = "Zenith★4 Toridcless",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Toridcless,  },
                     BaseScore = 48,
                     Type = FrontierMonsterType.BirdWyvern,
                    }, // z4 torid
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_baruragaru.gif",
                     Name = "Zenith★4 Baruragaru",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Baruragaru,  },
                     BaseScore = 72,
                     Type = FrontierMonsterType.Leviathan,
                    }, // z4 baru
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hypnoc.gif",
                     Name = "Zenith★4 Hypnocatrice",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Hypnocatrice,  },
                     BaseScore = 56,
                     Type = FrontierMonsterType.BirdWyvern,
                    }, // z4 hypnoc
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hyujikiki.gif",
                     Name = "Zenith★4 Hyujikiki",
                     QuestIDs = new List<int> { Numbers.QuestIDZ4Hyujikiki,  },
                     BaseScore = 64,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z4 hyuji
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_khezu.gif",
                     Name = "Zenith★4 Khezu",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Khezu,  },
                     BaseScore = 48,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z4 khezu
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_bogabadorumu.gif",
                     Name = "Zenith★4 Bogabadorumu",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Bogabadorumu,  },
                     BaseScore = 48,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z4 boggy
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_midogaron.gif",
                     Name = "Zenith★4 Midogaron",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Midogaron,  },
                     BaseScore = 56,
                     Type = FrontierMonsterType.FangedBeast,
                    }, // z4 mido
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rathalos.gif",
                     Name = "Zenith★4 Rathalos",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Rathalos,  },
                     BaseScore = 48,
                     Type = FrontierMonsterType.FlyingWyvern,
                    }, // z4 rath
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rukodiora.gif",
                     Name = "Zenith★4 Rukodiora",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Rukodiora,  },
                     BaseScore = 72,
                     Type = FrontierMonsterType.ElderDragon,
                    }, // z4 ruko
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_taikun_zamuza.gif",
                     Name = "Zenith★4 Taikun Zamuza",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4TaikunZamuza,  },
                     BaseScore = 72,
                     Type = FrontierMonsterType.Carapaceon,
                    }, // z4 taikun
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_harudomerugu.gif",
                     Name = "Zenith★4 Harudomerugu",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Harudomerugu,  },
                     BaseScore = 48,
                     Type = FrontierMonsterType.ElderDragon,
                    }, // z4 harudo
                new BingoMonster
                {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/howling_zinogre.png",
                    Name = "Howling Zinogre",
                    QuestIDs = new List<int> { Numbers.QuestIDHowlingZinogreForest, Numbers.QuestIDHowlingZinogreHistoric,  },
                    BaseScore = 90,
                    Type = FrontierMonsterType.FangedWyvern,
                },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/blinking_nargacuga.png",
                    Name = "Blinking Nargacuga",
                    QuestIDs = new List<int> {  Numbers.QuestIDBlinkingNargacugaForest, Numbers.QuestIDBlinkingNargacugaHistoric,  },
                    BaseScore = 90,
                    Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/arrogant_duremudira.png",
                    Name = "Arrogant Duremudira",
                    QuestIDs = new List<int> { Numbers.QuestIDArrogantDuremudira,  },
                    BaseScore = 120,
                    Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/sparkling_zerureusu.png",
                    Name = "Sparkling Zerureusu",
                    QuestIDs = new List<int> { Numbers.QuestIDSparklingZerureusu,  },
                    BaseScore = 110,
                    Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/burning_freezing_elzelion.png",
                    Name = "Burning Freezing Elzelion",
                    QuestIDs = new List<int> { Numbers.QuestIDBurningFreezingElzelionTower, Numbers.QuestIDBurningFreezingElzelionHistoric,  },
                    BaseScore = 150,
                    Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/bombardier_bogabadorumu.png",
                    Name = "Bombardier Bogabadorumu",
                    QuestIDs = new List<int> {  Numbers.QuestIDBombardierBogabadorumu,  },
                    BaseScore = 110,
                    Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/golden_deviljho.png",
                    Name = "Golden Deviljho",
                    QuestIDs = new List<int> { Numbers.QuestIDStarvingDeviljhoArena, Numbers.QuestIDStarvingDeviljhoHistoric,  },
                    BaseScore = 100,
                    Type = FrontierMonsterType.BruteWyvern,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shiten_disufiroa.png",
                    Name = "Upper Shiten Disufiroa",
                    QuestIDs = new List<int> {  Numbers.QuestIDUpperShitenDisufiroa,  },
                    BaseScore = 120,
                    Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shiten_unknown.png",
                    Name = "Upper Shiten Unknown",
                    QuestIDs = new List<int> {  Numbers.QuestIDUpperShitenUnknown,  },
                    BaseScore = 90,
                    Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/rebidiora.png",
                        Name = "UL Rebidiora",
                        BaseScore = 60,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 108,
                        Type = FrontierMonsterType.ElderDragon,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/akantor.png",
                        Name = "UL Akantor",
                        BaseScore = 80,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 77,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/silver_rathalos.png",
                        Name = "UL Silver Rathalos",
                        BaseScore = 70,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 41,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gold_rathian.png",
                        Name = "UL Gold Rathian",
                        BaseScore = 68,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 42,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/poborubarumu.png",
                        Name = "UL Poborubarumu",
                        BaseScore = 80,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 131,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/berukyurosu.png",
                        Name = "UL Berukyurosu",
                        BaseScore = 70,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 85,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/red_khezu.png",
                        Name = "UL Red Khezu",
                        BaseScore = 60,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 45,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/forokururu.png",
                        Name = "UL Forokururu",
                        BaseScore = 80,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 125,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/farunokku.png",
                        Name = "UL Farunokku",
                        BaseScore = 70,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 114,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/silver_hypnoc.png",
                        Name = "UL Silver Hypnoc",
                        BaseScore = 80,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 82,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/black_diablos.png",
                        Name = "UL Black Diablos",
                        BaseScore = 90,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 43,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenaserisu.png",
                        Name = "UL Zenaserisu",
                        BaseScore = 100,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 161,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gurenzeburu.png",
                        Name = "UL Gurenzeburu",
                        BaseScore = 90,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 96,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/black_gravios.png",
                        Name = "UL Black Gravios",
                        BaseScore = 90,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 47,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/kuarusepusu.png",
                        Name = "UL Kuarusepusu",
                        BaseScore = 100,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 105,
                        Type = FrontierMonsterType.Leviathan,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/white_espinas.png",
                        Name = "UL White Espinas",
                        BaseScore = 100,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 90,
                        Type = FrontierMonsterType.FlyingWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/red_lavasioth.png",
                        Name = "UL Red Lavasioth",
                        BaseScore = 100,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 79,
                        Type = FrontierMonsterType.PiscineWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/yian_garuga.png",
                        Name = "UL Yian Garuga",
                        BaseScore = 100,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 40,
                        Type = FrontierMonsterType.BirdWyvern,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/abiorugu.png",
                        Name = "UL Abiorugu",
                        BaseScore = 90,
                        IsUnlimited = true,
                        UnlimitedMonsterID = 104,
                        Type = FrontierMonsterType.BruteWyvern,
                    },
            }
        },
    });
}
