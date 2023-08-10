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
    public static ReadOnlyDictionary<Difficulty, List<List<int>>> BingoMonsterDifficulty { get; } = new (new Dictionary<Difficulty, List<List<int>>>
    {
        // Extreme difficulty is same as hard difficulty but the bingo board is twice as big.
        // Custom quests and event quests for monsters that aren't event-only are not counted.
        {
            Difficulty.Easy, new List<List<int>>
            {
                new List<int> { 23626, }, // Elzelion
                new List<int> { 23514, 23515, }, // zena day 23514 night 23515
                new List<int> { 23499, }, // zin
                new List<int> { 23667, }, // seregios
                new List<int> { 23494, }, // narga
                new List<int> { 23495, }, // uragaan
                new List<int> { 23528, }, // shagaru
                new List<int> { 23513, }, // gore
                new List<int> { 23643, }, // amatsu
                new List<int> { 54244, }, // mi ru
                new List<int> { 23496, }, // barioth
                new List<int> { 23497, }, // brachy
                new List<int> { 23498, }, // jho
                new List<int> { 23490, }, // diorex
                new List<int> { 23489, }, // garuba
                new List<int> { 23488, }, // varusa
                new List<int> { 23487, }, // gurea
                new List<int> { 23485, }, // toa
                new List<int> { 23486, }, // kurai
                new List<int> { 23492, }, // zeru
                new List<int> { 23491, }, // mera
                new List<int> { 23484, }, // voljang
                new List<int> { 23493, }, // stygian
                new List<int> { Numbers.QuestIDThirstyPariapuria, },
                new List<int> { Numbers.QuestIDRulingGuanzorumu, },
                new List<int> { 58043, }, // keo
                new List<int> { 23424, }, // guanzo
                new List<int> { 23431, 23432, }, // ruko
                new List<int> { 23265, 23266, }, // rebi
                new List<int> { 23587, }, // lv1000 shantien
                new List<int> { 23591, }, // lv1000 disu
                new List<int> { 23595, }, // lv1000 fatalis
                new List<int> { 23599, }, // lv1000 crimson
                new List<int> { 23602, }, // lower shiten disu
                new List<int> { 23604, }, // lower shiten unknown
                new List<int> { 23536, }, // z1+2 akura
                new List<int> { 23537, }, 
                new List<int> { 23718, }, // z1+2 anoru
                new List<int> { 23719, },
                new List<int> { 23644, }, // z1 inagami
                new List<int> { 23645, },
                new List<int> { 23480, }, // z1 espi
                new List<int> { 23481, },
                new List<int> { 23668, }, // z1 gasura
                new List<int> { 23669, },
                new List<int> { 23622, }, // z1 plesioth
                new List<int> { 23623, },
                new List<int> { 23610, }, // z1 giao
                new List<int> { 23611, },
                new List<int> { 23709, }, // z1 gravios
                new List<int> { 23710, },
                new List<int> { 23476, }, // z1 daimyo
                new List<int> { 23477, },
                new List<int> { 23540, }, // z1 tigrex
                new List<int> { 23541, },
                new List<int> { 23516, }, // z1 blango
                new List<int> { 23517, },
                new List<int> { 23659, }, // z1 dora
                new List<int> { 23660, },
                new List<int> { 23655, }, // z1 torid
                new List<int> { 23656, },
                new List<int> { 23713, }, // z1 baru
                new List<int> { 23714, },
                new List<int> { 23468, }, // z1 hypnoc
                new List<int> { 23469, },
                new List<int> { 23606, }, // z1 hyuji
                new List<int> { 23607, },
                new List<int> { 23472, }, // z1 khezu
                new List<int> { 23473, },
                new List<int> { 23705, }, // z1 boggy
                new List<int> { 23706, },
                new List<int> { 23614, }, // z1 mido
                new List<int> { 23615, },
                new List<int> { 23520, }, // z1 rath
                new List<int> { 23521, },
                new List<int> { 23618, }, // z1 ruko
                new List<int> { 23619, },
                new List<int> { 55923, }, // z1 taikun
                new List<int> { 55924, },
                new List<int> { 55929, }, // z1 harudo
                new List<int> { 55930, },    
            }
        },
        {
            Difficulty.Medium, new List<List<int>>
            {
                new List<int> { 23538, }, // z3 akura
                new List<int> { 23720, }, // z3 anoru
                new List<int> { 23646, }, // z3 inagami
                new List<int> { 23482, }, // z3 espi
                new List<int> { 23670, }, // z3 gasura
                new List<int> { 23624, }, // z3 plesioth
                new List<int> { 23612, }, // z3 giao
                new List<int> { 23711, }, // z3 gravios
                new List<int> { 23478, }, // z3 daimyo
                new List<int> { 23542, }, // z3 tigrex
                new List<int> { 23518, }, // z3 blango
                new List<int> { 23661, }, // z3 dora
                new List<int> { 23657, }, // z3 torid
                new List<int> { 23715, }, // z3 baru
                new List<int> { 23470, }, // z3 hypnoc
                new List<int> { 23608, }, // z3 hyuji
                new List<int> { 23474, }, // z3 khezu
                new List<int> { 23707, }, // z3 boggy
                new List<int> { 23616, }, // z3 mido
                new List<int> { 23522, }, // z3 rath
                new List<int> { 23620, }, // z3 ruko
                new List<int> { 55925, }, // z3 taikun
                new List<int> { 55931, }, // z3 harudo
                new List<int> { 55916, }, // starving jho
                new List<int> { Numbers.QuestIDShiftingMiRu, },
                new List<int> { Numbers.QuestIDTwinheadRajangsHistoric, },
                new List<int> { Numbers.QuestIDLV9999Fatalis, },
                new List<int> { Numbers.QuestIDLV9999CrimsonFatalis, },
                new List<int> { Numbers.QuestIDLV9999Disufiroa, },
                new List<int> { Numbers.QuestIDLV9999Shantien, },
                new List<int> { Numbers.QuestIDSecondDistrictDuremudira, },
            }
        },
        {
            Difficulty.Hard, new List<List<int>>
            {
                new List<int> { 23539, }, // z4 akura
                new List<int> { 23721, }, // z4 anoru
                new List<int> { 23647, }, // z4 inagami
                new List<int> { 23483, }, // z4 espi
                new List<int> { 23671, }, // z4 gasura
                new List<int> { 23625, }, // z4 plesioth
                new List<int> { 23613, }, // z4 giao
                new List<int> { 23712, }, // z4 gravios
                new List<int> { 23479, }, // z4 daimyo
                new List<int> { 23543, }, // z4 tigrex
                new List<int> { 23519, }, // z4 blango
                new List<int> { 23662, }, // z4 dora
                new List<int> { 23658, }, // z4 torid
                new List<int> { 23716, }, // z4 baru
                new List<int> { 23471, }, // z4 hypnoc
                new List<int> { 23609, }, // z4 hyuji
                new List<int> { 23475, }, // z4 khezu
                new List<int> { 23708, }, // z4 boggy
                new List<int> { 23617, }, // z4 mido
                new List<int> { 23523, }, // z4 rath
                new List<int> { 23621, }, // z4 ruko
                new List<int> { 55926, }, // z4 taikun
                new List<int> { 55932, }, // z4 harudo
                new List<int> { Numbers.QuestIDHowlingZinogreForest, Numbers.QuestIDHowlingZinogreHistoric },
                new List<int> { Numbers.QuestIDBlinkingNargacugaForest, Numbers.QuestIDBlinkingNargacugaHistoric, },
                new List<int> { Numbers.QuestIDArrogantDuremudira, },
                new List<int> { Numbers.QuestIDSparklingZerureusu, },
                new List<int> { Numbers.QuestIDBurningFreezingElzelionTower, Numbers.QuestIDBurningFreezingElzelionHistoric, },
                new List<int> { Numbers.QuestIDBlitzkriegBogabadorumu, },
                new List<int> { Numbers.QuestIDStarvingDeviljhoArena, Numbers.QuestIDStarvingDeviljhoHistoric },
                new List<int> { Numbers.QuestIDUpperShitenDisufiroa, },
                new List<int> { Numbers.QuestIDUpperShitenUnknown, },
            }
        },
    });
}
