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
                },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenaserisu.png",
                        Name = "Zenaserisu",
                        QuestIDs = new List<int> {  23514, 23515, },
                        BaseScore = 9,
                    }, // zena day 23514 night 23515
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zinogre.png",
                        Name = "Zinogre",
                        QuestIDs = new List<int> {  23499,  },
                        BaseScore = 8,
                    }, // zin
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/seregios.png",
                        Name = "Seregios",
                        QuestIDs = new List<int> {  23667, },
                        BaseScore = 9,
                    }, // seregios
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/nargacuga.png",
                        Name = "Nargacuga",
                        QuestIDs = new List<int> {  23494, },
                        BaseScore = 5,
                    }, // narga
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/uragaan.png",
                        Name = "Uragaan",
                        QuestIDs = new List<int> {  23495, },
                        BaseScore = 6,
                    }, // uragaan
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shagaru_magala.png",
                        Name = "Shagaru Magala",
                        QuestIDs = new List<int> {  23528, },
                        BaseScore = 9,
                    }, // shagaru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gore_magala.png",
                        Name = "Gore Magala",
                        QuestIDs = new List<int> {  23513,  },
                        BaseScore = 5,
                    }, // gore
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/amatsu.png",
                        Name = "Amatsu",
                        QuestIDs = new List<int> {  23643,  },
                        BaseScore = 6,
                    }, // amatsu
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/mi_ru.png",
                        Name = "Mi Ru",
                        QuestIDs = new List<int> {  54244,  },
                        BaseScore = 5,
                    }, // mi ru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/barioth.png",
                        Name = "Barioth",
                        QuestIDs = new List<int> {  23496,  },
                        BaseScore = 6,
                    }, // barioth
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/brachydios.png",
                        Name = "Brachydios",
                        QuestIDs = new List<int> {  23497,  },
                        BaseScore = 6,
                    }, // brachy
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/deviljho.png",
                        Name = "Deviljho",
                        QuestIDs = new List<int> {  23498,  },
                        BaseScore = 5,
                    }, // jho
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/diorex.png",
                        Name = "Diorex",
                        QuestIDs = new List<int> {  23490,  },
                        BaseScore = 1,
                    }, // diorex
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/garuba_daora.png",
                        Name = "Garuba Daora",
                        QuestIDs = new List<int> {  23489,  },
                        BaseScore = 8,
                    }, // garuba
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/varusaburosu.png",
                        Name = "Varusaburosu",
                        QuestIDs = new List<int> {  23488,  },
                        BaseScore = 6,
                    }, // varusa
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gureadomosu.png",
                        Name = "Gureadomosu",
                        QuestIDs = new List<int> {  23487,  },
                        BaseScore = 6,
                    }, // gurea
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/toa_tesukatora.png",
                        Name = "Toa Tesukatora",
                        QuestIDs = new List<int> {  23485,  },
                        BaseScore = 9,
                    }, // toa
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/yama_kurai.png",
                        Name = "Yama Kurai",
                        QuestIDs = new List<int> {  23486,  },
                        BaseScore = 10,
                    }, // kurai
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zerureusu.png",
                        Name = "Zerureusu",
                        QuestIDs = new List<int> {  23492,  },
                        BaseScore = 4,
                    }, // zeru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/meraginasu.png",
                        Name = "Meraginasu",
                        QuestIDs = new List<int> {  23491,  },
                        BaseScore = 5,
                    }, // mera
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/voljang.png",
                        Name = "Voljang",
                        QuestIDs = new List<int> {  23484,  },
                        BaseScore = 8,
                    }, // voljang
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/stygian_zinogre.png",
                        Name = "Stygian Zinogre",
                        QuestIDs = new List<int> {  23493,  },
                        BaseScore = 9,
                    }, // stygian
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/thirsty_pariapuria.png",
                        Name = "Thirsty Pariapuria",
                        QuestIDs = new List<int> { Numbers.QuestIDThirstyPariapuria,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/ruling_guanzorumu.png",
                        Name = "Ruling Guanzorumu",
                        QuestIDs = new List<int> { Numbers.QuestIDRulingGuanzorumu,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/keoaruboru.png",
                        Name = "Keoaruboru",
                        QuestIDs = new List<int> {  58043,  },
                        BaseScore = 3,
                    }, // keo
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/guanzorumu.png",
                        Name = "Guanzorumu",
                        QuestIDs = new List<int> {  23424,  },
                        BaseScore = 6,
                    }, // guanzo
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/rukodiora.png",
                        Name = "Rukodiora",
                        QuestIDs = new List<int> {  23431, 23432,  },
                        BaseScore = 3,
                    }, // ruko
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/rebidiora.png",
                        Name = "Rebidiora",
                        QuestIDs = new List<int> {  23265, 23266,  },
                        BaseScore = 2,
                    }, // rebi
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_shantien.png",
                        Name = "Lv1000 Shantien",
                        QuestIDs = new List<int> {  23587,  },
                        BaseScore = 12,
                    }, // lv1000 shantien
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/disufiroa.png",
                        Name = "Lv1000 Disufiroa",
                        QuestIDs = new List<int> {  23591,  },
                        BaseScore = 13,
                    }, // lv1000 disu
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_fatalis.png",
                        Name = "Lv1000 Fatalis",
                        QuestIDs = new List<int> {  23595,  },
                        BaseScore = 11,
                    }, // lv1000 fatalis
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_crimson_fatalis.png",
                        Name = "Lv1000 Crimson Fatalis",
                        QuestIDs = new List<int> {  23599,  },
                        BaseScore = 12,
                    }, // lv1000 crimson
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shiten_disufiroa.png",
                        Name = "Lower Shiten Disufiroa",
                        QuestIDs = new List<int> {  23602,  },
                        BaseScore = 15,
                    }, // lower shiten disu
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shiten_unknown.png",
                        Name = "Lower Shiten Unknown",
                        QuestIDs = new List<int> {  23604,  },
                        BaseScore = 10,
                    }, // lower shiten unknown
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_akura_vashimu.gif",
                        Name = "Zenith★1 Akura Vashimu",
                        QuestIDs = new List<int> {  23536,  },
                        BaseScore = 7,
                    }, // z1+2 akura
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_akura_vashimu.gif",
                        Name = "Zenith★2 Akura Vashimu",
                        QuestIDs = new List<int> {  23537,  },
                        BaseScore = 14,
                    }, 
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_anorupatisu.gif",
                        Name = "Zenith★1 Anorupatisu",
                        QuestIDs = new List<int> {  23718,  },
                        BaseScore = 9,
                    }, // z1+2 anoru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_anorupatisu.gif",
                        Name = "Zenith★2 Anorupatisu",
                        QuestIDs = new List<int> {  23719,  },
                        BaseScore = 18,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_inagami.gif",
                        Name = "Zenith★1 Inagami",
                        QuestIDs = new List<int> {  23644,  },
                        BaseScore = 10,
                    }, // z1 inagami
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_inagami.gif",
                        Name = "Zenith★2 Inagami",
                        QuestIDs = new List<int> {  23645,  },
                        BaseScore = 20,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_espinas.gif",
                        Name = "Zenith★1 Espinas",
                        QuestIDs = new List<int> {  23480,  },
                        BaseScore = 6,
                    }, // z1 espi
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_espinas.gif",
                        Name = "Zenith★2 Espinas",
                        QuestIDs = new List<int> {  23481,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gasurabazura.gif",
                        Name = "Zenith★1 Gasurabazura",
                        QuestIDs = new List<int> {  23668,  },
                        BaseScore = 9,
                    }, // z1 gasura
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gasurabazura.gif",
                        Name = "Zenith★2 Gasurabazura",
                        QuestIDs = new List<int> {  23669,  },
                        BaseScore = 18,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_plesioth.gif",
                        Name = "Zenith★1 Plesioth",
                        QuestIDs = new List<int> {  23622,  },
                        BaseScore = 8,
                    }, // z1 plesioth
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_plesioth.gif",
                        Name = "Zenith★2 Plesioth",
                        QuestIDs = new List<int> {  23623,  },
                        BaseScore = 16,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_giaorugu.gif",
                        Name = "Zenith★1 Giaorugu",
                        QuestIDs = new List<int> {  23610,  },
                        BaseScore = 7,
                    }, // z1 giao
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_giaorugu.gif",
                        Name = "Zenith★2 Giaorugu",
                        QuestIDs = new List<int> {  23611,  },
                        BaseScore = 14,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gravios.gif",
                        Name = "Zenith★1 Gravios",
                        QuestIDs = new List<int> {  23709,  },
                        BaseScore = 9,
                    }, // z1 gravios
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gravios.gif",
                        Name = "Zenith★2 Gravios",
                        QuestIDs = new List<int> {  23710,  },
                        BaseScore = 18,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_daimyo_hermitaur.gif",
                        Name = "Zenith★1 Daimyo Hermitaur",
                        QuestIDs = new List<int> {  23476,  },
                        BaseScore = 6,
                    }, // z1 daimyo
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_daimyo_hermitaur.gif",
                        Name = "Zenith★2 Daimyo Hermitaur",
                        QuestIDs = new List<int> {  23477,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_tigrex.gif",
                        Name = "Zenith★1 Tigrex",
                        QuestIDs = new List<int> {  23540,  },
                        BaseScore = 5,
                    }, // z1 tigrex
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_tigrex.gif",
                        Name = "Zenith★2 Tigrex",
                        QuestIDs = new List<int> {  23541,  },
                        BaseScore = 10,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_blangonga.gif",
                        Name = "Zenith★1 Blangonga",
                        QuestIDs = new List<int> {  23516,  },
                        BaseScore = 6,
                    }, // z1 blango
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_blangonga.gif",
                        Name = "Zenith★2 Blangonga",
                        QuestIDs = new List<int> {  23517,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_doragyurosu.gif",
                        Name = "Zenith★1 Doragyurosu",
                        QuestIDs = new List<int> {  23659,  },
                        BaseScore = 6,
                    }, // z1 dora
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_doragyurosu.gif",
                        Name = "Zenith★2 Doragyurosu",
                        QuestIDs = new List<int> {  23660,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_toridcless.gif",
                        Name = "Zenith★1 Toridcless",
                        QuestIDs = new List<int> {  23655,  },
                        BaseScore = 6,
                    }, // z1 torid
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_toricless.gif",
                        Name = "Zenith★2 Toricless",
                        QuestIDs = new List<int> {  23656,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_baruragaru.gif",
                        Name = "Zenith★1 Baruragaru",
                        QuestIDs = new List<int> {  23713,  },
                        BaseScore = 9,
                    }, // z1 baru
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_baruragaru.gif",
                        Name = "Zenith★2 Baruragaru",
                        QuestIDs = new List<int> {  23714,  },
                        BaseScore = 18,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hypnoc.gif",
                        Name = "Zenith★1 Hypnocatrice",
                        QuestIDs = new List<int> {  23468,  },
                        BaseScore = 7,
                    }, // z1 hypnoc
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hypnoc.gif",
                        Name = "Zenith★2 Hypnocatrice",
                        QuestIDs = new List<int> {  23469,  },
                        BaseScore = 14,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hyujikiki.gif",
                        Name = "Zenith★1 Hyujikiki",
                        QuestIDs = new List<int> {  23606,  },
                        BaseScore = 8,
                    }, // z1 hyuji
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hyujikiki.gif",
                        Name = "Zenith★2 Hyujikiki",
                        QuestIDs = new List<int> {  23607,  },
                        BaseScore = 16,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_khezu.gif",
                        Name = "Zenith★1 Khezu",
                        QuestIDs = new List<int> {  23472,  },
                        BaseScore = 6,
                    }, // z1 khezu
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_khezu.gif",
                        Name = "Zenith★2 Khezu",
                        QuestIDs = new List<int> {  23473,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_bogabadorumu.gif",
                        Name = "Zenith★1 Bogabadorumu",
                        QuestIDs = new List<int> {  23705,  },
                        BaseScore = 6,
                    }, // z1 boggy
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_bogabadorumu.gif",
                        Name = "Zenith★2 Bogabadorumu",
                        QuestIDs = new List<int> {  23706,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_midogaron.gif",
                        Name = "Zenith★1 Midogaron",
                        QuestIDs = new List<int> {  23614,  },
                        BaseScore = 7,
                    }, // z1 mido
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_midogaron.gif",
                        Name = "Zenith★2 Midogaron",
                        QuestIDs = new List<int> {  23615,  },
                        BaseScore = 14,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rathalos.gif",
                        Name = "Zenith★1 Rathalos",
                        QuestIDs = new List<int> {  23520,  },
                        BaseScore = 6,
                    }, // z1 rath
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rathalos.gif",
                        Name = "Zenith★2 Rathalos",
                        QuestIDs = new List<int> {  23521,  },
                        BaseScore = 12,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rukodiora.gif",
                        Name = "Zenith★1 Rukodiora",
                        QuestIDs = new List<int> {  23618,  },
                        BaseScore = 9,
                    }, // z1 ruko
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rukodiora.gif",
                        Name = "Zenith★2 Rukodiora",
                        QuestIDs = new List<int> {  23619,  },
                        BaseScore = 18,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_taikun_zamuza.gif",
                        Name = "Zenith★1 Taikun Zamuza",
                        QuestIDs = new List<int> {  55923,  },
                        BaseScore = 9,
                    }, // z1 taikun
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_taikun_zamuza.gif",
                        Name = "Zenith★2 Taikun Zamuza",
                        QuestIDs = new List<int> {  55924,  },
                        BaseScore = 18,
                    },
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_harudomerugu.gif",
                        Name = "Zenith★1 Harudomerugu",
                        QuestIDs = new List<int> {  55929,  },
                        BaseScore = 6,
                    }, // z1 harudo
                new BingoMonster {
                        Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_harudomerugu.gif",
                        Name = "Zenith★2 Harudomerugu",
                        QuestIDs = new List<int> {  55930,  },
                        BaseScore = 12,
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
                    }, // z3 akura
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_anorupatisu.gif",
                    Name = "Zenith★3 Anorupatisu",
                    QuestIDs = new List<int> {  23720,  },
                    BaseScore = 36,
                    }, // z3 anoru
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_inagami.gif",
                    Name = "Zenith★3 Inagami",
                    QuestIDs = new List<int> {  23646,  },
                    BaseScore = 40,
                    }, // z3 inagami
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_espinas.gif",
                    Name = "Zenith★3 Espinas",
                    QuestIDs = new List<int> {  23482,  },
                    BaseScore = 24,
                    }, // z3 espi
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gasurabazura.gif",
                    Name = "Zenith★3 Gasurabazura",
                    QuestIDs = new List<int> {  23670,  },
                    BaseScore = 36,
                    }, // z3 gasura
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_plesioth.gif",
                    Name = "Zenith★3 Plesioth",
                    QuestIDs = new List<int> {  23624,  },
                    BaseScore = 32,
                    }, // z3 plesioth
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_giaorugu.gif",
                    Name = "Zenith★3 Giaorugu",
                    QuestIDs = new List<int> {  23612,  },
                    BaseScore = 28,
                    }, // z3 giao
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gravios.gif",
                     Name = "Zenith★3 Gravios",
                     QuestIDs = new List<int> {  23711,  },
                     BaseScore = 36,
                    }, // z3 gravios
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_daimyo_hermitaur.gif",
                     Name = "Zenith★3 Daimyo Hermitaur",
                     QuestIDs = new List<int> {  23478,  },
                     BaseScore = 24,
                    }, // z3 daimyo
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_tigrex.gif",
                     Name = "Zenith★3 Tigrex",
                     QuestIDs = new List<int> {  23542,  },
                     BaseScore = 20,
                    }, // z3 tigrex
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_blangonga.gif",
                     Name = "Zenith★3 Blangonga",
                     QuestIDs = new List<int> {  23518,  },
                     BaseScore = 24,
                    }, // z3 blango
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_doragyurosu.gif",
                     Name = "Zenith★3 Doragyurosu",
                     QuestIDs = new List<int> {  23661,  },
                     BaseScore = 24,
                    }, // z3 dora
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_toridcless.gif",
                     Name = "Zenith★3 Toridcless",
                     QuestIDs = new List<int> {  23657,  },
                     BaseScore = 24,
                    }, // z3 torid
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_baruragaru.gif",
                     Name = "Zenith★3 Baruragaru",
                     QuestIDs = new List<int> {  23715,  },
                     BaseScore = 36,
                    }, // z3 baru
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hypnoc.gif",
                     Name = "Zenith★3 Hypnocatrice",
                     QuestIDs = new List<int> {  23470,  },
                     BaseScore = 28,
                    }, // z3 hypnoc
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hyujikiki.gif",
                     Name = "Zenith★3 Hyujikiki",
                     QuestIDs = new List<int> {  23608,  },
                     BaseScore = 32,
                    }, // z3 hyuji
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_khezu.gif",
                     Name = "Zenith★3 Khezu",
                     QuestIDs = new List<int> {  23474,  },
                     BaseScore = 24,
                    }, // z3 khezu
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_bogabadorumu.gif",
                     Name = "Zenith★3 Bogabadorumu",
                     QuestIDs = new List<int> {  23707,  },
                     BaseScore = 24,
                    }, // z3 boggy
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_midogaron.gif",
                     Name = "Zenith★3 Midogaron",
                     QuestIDs = new List<int> {  23616,  },
                     BaseScore = 28,
                    }, // z3 mido
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rathalos.gif",
                     Name = "Zenith★3 Rathalos",
                     QuestIDs = new List<int> {  23522,  },
                     BaseScore = 24,
                    }, // z3 rath
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rukodiora.gif",
                     Name = "Zenith★3 Rukodiora",
                     QuestIDs = new List<int> {  23620,  },
                     BaseScore = 36,
                    }, // z3 ruko
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_taikun_zamuza.gif",
                     Name = "Zenith★3 Taikun Zamuza",
                     QuestIDs = new List<int> {  55925,  },
                     BaseScore = 36,
                    }, // z3 taikun
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_harudomerugu.gif",
                     Name = "Zenith★3 Harudomerugu",
                     QuestIDs = new List<int> {  55931,  },
                     BaseScore = 24,
                    }, // z3 harudo
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/starving_deviljho.png",
                     Name = "Starving Deviljho",
                     QuestIDs = new List<int> {  55916,  },
                     BaseScore = 30,
                    }, // starving jho
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shifting_mi_ru.png",
                    Name = "Shifting Mi Ru",
                    QuestIDs = new List<int> {  Numbers.QuestIDShiftingMiRu,  },
                    BaseScore = 30,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/twinhead_rajang.png",
                    Name = "Twinhead Rajangs",
                    QuestIDs = new List<int> {  Numbers.QuestIDTwinheadRajangsHistoric,  },
                    BaseScore = 20,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_fatalis.png",
                    Name = "Lv9999 Fatalis",
                    QuestIDs = new List<int> {  Numbers.QuestIDLV9999Fatalis,  },
                    BaseScore = 35,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_crimson_fatalis.png",
                    Name = "Lv9999 Crimson Fatalis",
                    QuestIDs = new List<int> {  Numbers.QuestIDLV9999CrimsonFatalis,  },
                    BaseScore = 40,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/disufiroa.png",
                    Name = "Lv9999 Disufiroa",
                    QuestIDs = new List<int> {  Numbers.QuestIDLV9999Disufiroa,  },
                    BaseScore = 40,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conquest_shantien.png",
                    Name = "Lv9999 Shantien",
                    QuestIDs = new List<int> {  Numbers.QuestIDLV9999Shantien,  },
                    BaseScore = 40,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/duremudira.png",
                    Name = "2nd Distric Duremudira",
                    QuestIDs = new List<int> {  Numbers.QuestIDSecondDistrictDuremudira,  },
                    BaseScore = 40,
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
                    }, // z4 akura
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_anorupatisu.gif",
                    Name = "Zenith★4 Anorupatisu",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Anorupatisu,  },
                    BaseScore = 72,
                    }, // z4 anoru
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_inagami.gif",
                    Name = "Zenith★4 Inagami",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Inagami,  },
                    BaseScore = 80,
                    }, // z4 inagami
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_espinas.gif",
                    Name = "Zenith★4 Espinas",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Espinas,  },
                    BaseScore = 48,
                    }, // z4 espi
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gasurabazura.gif",
                    Name = "Zenith★4 Gasurabazura",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Gasurabazura,  },
                    BaseScore = 80,
                    }, // z4 gasura
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_plesioth.gif",
                    Name = "Zenith★4 Plesioth",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Plesioth,  },
                    BaseScore = 64,
                    }, // z4 plesioth
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_giaorugu.gif",
                    Name = "Zenith★4 Giaorugu",
                    QuestIDs = new List<int> {  Numbers.QuestIDZ4Giaorugu,  },
                    BaseScore = 56,
                    }, // z4 giao
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_gravios.gif",
                     Name = "Zenith★4 Gravios",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Gravios,  },
                     BaseScore = 72,
                    }, // z4 gravios
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_daimyo_hermitaur.gif",
                     Name = "Zenith★4 Daimyo Hermitaur",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4DaimyoHermitaur,  },
                     BaseScore = 48,
                    }, // z4 daimyo
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_tigrex.gif",
                     Name = "Zenith★4 Tigrex",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Tigrex,  },
                     BaseScore = 40,
                    }, // z4 tigrex
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_blangonga.gif",
                     Name = "Zenith★4 Blangonga",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Blangonga,  },
                     BaseScore = 48,
                    }, // z4 blango
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_doragyurosu.gif",
                     Name = "Zenith★4 Doragyurosu",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Doragyurosu,  },
                     BaseScore = 48,
                    }, // z4 dora
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_toridcless.gif",
                     Name = "Zenith★4 Toridcless",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Toridcless,  },
                     BaseScore = 48,
                    }, // z4 torid
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_baruragaru.gif",
                     Name = "Zenith★4 Baruragaru",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Baruragaru,  },
                     BaseScore = 72,
                    }, // z4 baru
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hypnoc.gif",
                     Name = "Zenith★4 Hypnocatrice",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Hypnocatrice,  },
                     BaseScore = 56,
                    }, // z4 hypnoc
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_hyujikiki.gif",
                     Name = "Zenith★4 Hyujikiki",
                     QuestIDs = new List<int> { Numbers.QuestIDZ4Hyujikiki,  },
                     BaseScore = 64,
                    }, // z4 hyuji
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_khezu.gif",
                     Name = "Zenith★4 Khezu",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Khezu,  },
                     BaseScore = 48,
                    }, // z4 khezu
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_bogabadorumu.gif",
                     Name = "Zenith★4 Bogabadorumu",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Bogabadorumu,  },
                     BaseScore = 48,
                    }, // z4 boggy
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_midogaron.gif",
                     Name = "Zenith★4 Midogaron",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Midogaron,  },
                     BaseScore = 56,
                    }, // z4 mido
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rathalos.gif",
                     Name = "Zenith★4 Rathalos",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Rathalos,  },
                     BaseScore = 48,
                    }, // z4 rath
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_rukodiora.gif",
                     Name = "Zenith★4 Rukodiora",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Rukodiora,  },
                     BaseScore = 72,
                    }, // z4 ruko
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_taikun_zamuza.gif",
                     Name = "Zenith★4 Taikun Zamuza",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4TaikunZamuza,  },
                     BaseScore = 72,
                    }, // z4 taikun
                new BingoMonster {
                     Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_harudomerugu.gif",
                     Name = "Zenith★4 Harudomerugu",
                     QuestIDs = new List<int> {  Numbers.QuestIDZ4Harudomerugu,  },
                     BaseScore = 48,
                    }, // z4 harudo
                new BingoMonster
                {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/howling_zinogre.png",
                    Name = "Howling Zinogre",
                    QuestIDs = new List<int> { Numbers.QuestIDHowlingZinogreForest, Numbers.QuestIDHowlingZinogreHistoric,  },
                    BaseScore = 90,
                },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/blinking_nargacuga.png",
                    Name = "Blinking Nargacuga",
                    QuestIDs = new List<int> {  Numbers.QuestIDBlinkingNargacugaForest, Numbers.QuestIDBlinkingNargacugaHistoric,  },
                    BaseScore = 90,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/arrogant_duremudira.png",
                    Name = "Arrogant Duremudira",
                    QuestIDs = new List<int> { Numbers.QuestIDArrogantDuremudira,  },
                    BaseScore = 120,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/sparkling_zerureusu.png",
                    Name = "Sparkling Zerureusu",
                    QuestIDs = new List<int> { Numbers.QuestIDSparklingZerureusu,  },
                    BaseScore = 110,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/burning_freezing_elzelion.png",
                    Name = "Burning Freezing Elzelion",
                    QuestIDs = new List<int> { Numbers.QuestIDBurningFreezingElzelionTower, Numbers.QuestIDBurningFreezingElzelionHistoric,  },
                    BaseScore = 150,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/blitzkrieg_bogabadorumu.png",
                    Name = "Blitzkrieg Bogabadorumu",
                    QuestIDs = new List<int> {  Numbers.QuestIDBlitzkriegBogabadorumu,  },
                    BaseScore = 110,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/golden_deviljho.png",
                    Name = "Golden Deviljho",
                    QuestIDs = new List<int> { Numbers.QuestIDStarvingDeviljhoArena, Numbers.QuestIDStarvingDeviljhoHistoric,  },
                    BaseScore = 100,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shiten_disufiroa.png",
                    Name = "Upper Shiten Disufiroa",
                    QuestIDs = new List<int> {  Numbers.QuestIDUpperShitenDisufiroa,  },
                    BaseScore = 120,
                    },
                new BingoMonster {
                    Image = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shiten_unknown.png",
                    Name = "Upper Shiten Unknown",
                    QuestIDs = new List<int> {  Numbers.QuestIDUpperShitenUnknown,  },
                    BaseScore = 90,
                    },
            }
        },
    });
}
