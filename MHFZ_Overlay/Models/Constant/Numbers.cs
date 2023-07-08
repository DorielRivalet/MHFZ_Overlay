// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Constant;

public static class Numbers
{
    /// <summary>
    /// The frames per second in the game.
    /// </summary>
    public const int FRAMES_PER_SECOND = 30;
    public const int FRAMES_1_MINUTE = FRAMES_PER_SECOND * 60;
    public const int FRAMES_1_HOUR = FRAMES_1_MINUTE * 60;

    public const int REQUIRED_COMPLETIONS_MONSTER_SLAYER = 10;
    public const int REQUIRED_COMPLETIONS_MONSTER_ANNIHILATOR = 25;
    public const int REQUIRED_COMPLETIONS_MONSTER_EXTERMINATOR = 50;

    /* Quest IDs

    Zeniths
    Akura Vashimu 23539
    Anorupatisu 23721
    Blangonga 23519
    Daimyo Hermitaur 23479
    Doragyurosu 23662
    Espinas 23483
    Gasurabazura 23671
    Giaorugu 23613
    Hypnocatrice 23471
    Hyujikiki 23609
    Inagami 23647
    Khezu 23475
    Midogaron 23617
    Plesioth 23625
    Rathalos 23523
    Rukodiora 23621
    Tigrex 23543
    Toridcless 23658
    Baruragaru 23716
    Bogabadorumu 23708
    Gravios 23712
    Harudomerugu 55932
    Taikun Zamuza 55926

    Conquest
    Fatalis 23596
    Crimson Fatalis 23601
    Shantien 23588
    Disufiroa 23592

    Upper Shiten
    Unknown 23605
    Disufiroa 23603

    Musou
    Pariapuria 55532
    Mi Ru 55531
    Guanzorumu 55529
    Nargacuga 55534 55922
    Zinogre 55919 55535
    Elzelion 55936 55714 
    Duremudira 23649
    Deviljho 55917 55530
    Bogabadorumu 55949
    Zerureusu 56106 55951

    Misc
    Veggie Elder 53189
    Producer Gogomoa 50748 53032
    Four Heavenly King 40219 40220 40237 40238
    Hatsune Miku 40230
    PSO2 40239
    Megaman 40240
    Higanjima 40217
    MHF-Q 53209
    Huge Plesioth 53028
    Sunglasses Kut-Ku 53139
    Congalala 50365
    Jungle Rocks 53208
    Road 23527
    2nd District Duremudira 21746
    Twinhead Rajangs 55937 (TODO there is one for arena too)
    Mosswine Revenge 50143
    Mosswine Duel 62793
    Nuclear Gypceros 63390
    Halloween Speedster 53325
    Mosswine's Last Stand 53323
    VR 53232

    TODO replace the numbers in source code as necessary
     */
    public const int QUEST_ID_Z4_AKURA_VASHIMU = 23539;
    public const int QUEST_ID_Z4_ANORUPATISU = 23721;
    public const int QUEST_ID_Z4_BLANGONGA = 23519;
    public const int QUEST_ID_Z4_DAIMYO_HERMITAUR = 23479;
    public const int QUEST_ID_Z4_DORAGYUROSU = 23662;
    public const int QUEST_ID_Z4_ESPINAS = 23483;
    public const int QUEST_ID_Z4_GASURABAZURA = 23671;
    public const int QUEST_ID_Z4_GIAORUGU = 23613;
    public const int QUEST_ID_Z4_HYPNOCATRICE = 23471;
    public const int QUEST_ID_Z4_HYUJIKIKI = 23609;
    public const int QUEST_ID_Z4_INAGAMI = 23647;
    public const int QUEST_ID_Z4_KHEZU = 23475;
    public const int QUEST_ID_Z4_MIDOGARON = 23617;
    public const int QUEST_ID_Z4_PLESIOTH = 23625;
    public const int QUEST_ID_Z4_RATHALOS = 23523;
    public const int QUEST_ID_Z4_RUKODIORA = 23621;
    public const int QUEST_ID_Z4_TIGREX = 23543;
    public const int QUEST_ID_Z4_TORIDCLESS = 23658;
    public const int QUEST_ID_Z4_BARURAGARU = 23716;
    public const int QUEST_ID_Z4_BOGABADORUMU = 23708;
    public const int QUEST_ID_Z4_GRAVIOS = 23712;
    public const int QUEST_ID_Z4_HARUDOMERUGU = 55932;
    public const int QUEST_ID_Z4_TAIKUN_ZAMUZA = 55926;
    public const int QUEST_ID_LV9999_FATALIS = 23596;
    public const int QUEST_ID_LV9999_CRIMSON_FATALIS = 23601;
    public const int QUEST_ID_LV9999_SHANTIEN = 23588;
    public const int QUEST_ID_LV9999_DISUFIROA = 23592;
    public const int QUEST_ID_UPPER_SHITEN_UNKNOWN = 23605;
    public const int QUEST_ID_UPPER_SHITEN_DISUFIROA = 23603;
    public const int QUEST_ID_THIRSTY_PARIAPURIA = 55532;
    public const int QUEST_ID_RULING_GUANZORUMU = 55529;
    public const int QUEST_ID_SHIFTING_MI_RU = 55531;
    public const int QUEST_ID_BLINKING_NARGACUGA_FOREST = 55534;
    public const int QUEST_ID_BLINKING_NARGACUGA_HISTORIC = 55922;
    public const int QUEST_ID_HOWLING_ZINOGRE_FOREST = 55535;
    public const int QUEST_ID_HOWLING_ZINOGRE_HISTORIC = 55919;
    public const int QUEST_ID_SPARKLING_ZERUREUSU = 55951;
    public const int QUEST_ID_SPARKLING_ZERUREUSU_EVENT = 56106;
    public const int QUEST_ID_ARROGANT_DUREMUDIRA = 23649;
    public const int QUEST_ID_STARVING_DEVILJHO_ARENA = 55530;
    public const int QUEST_ID_STARVING_DEVILJHO_HISTORIC = 55917;
    public const int QUEST_ID_BLITZKRIEG_BOGABADORUMU = 55949;
    public const int QUEST_ID_BURNING_FREEZING_ELZELION_TOWER = 55714;
    public const int QUEST_ID_BURNING_FREEZING_ELZELION_HISTORIC = 55936;
    public const int QUEST_ID_VEGGIE_ELDER_LOVE = 53189;
    public const int QUEST_ID_PRODUCER_GOGOMOA_LR = 50748;
    public const int QUEST_ID_PRODUCER_GOGOMOA_HR = 53032;
    public const int QUEST_ID_FOUR_HEAVENLY_KING_MALE_1 = 40219;
    public const int QUEST_ID_FOUR_HEAVENLY_KING_MALE_2 = 40220;
    public const int QUEST_ID_FOUR_HEAVENLY_KING_FEMALE_1 = 40237;
    public const int QUEST_ID_FOUR_HEAVENLY_KING_FEMALE_2 = 40238;
    public const int QUEST_ID_HATSUNE_MIKU = 40230;
    public const int QUEST_ID_PSO2 = 40239;
    public const int QUEST_ID_MEGAMAN = 40240;
    public const int QUEST_ID_HIGANJIMA = 40217;
    public const int QUEST_ID_MHFQ = 53209;
    public const int QUEST_ID_HUGE_PLESIOTH = 53028;
    public const int QUEST_ID_SUNGLASSES_KUTKU = 53139;
    public const int QUEST_ID_CONGALALA_CURE = 50365;
    public const int QUEST_ID_JUNGLE_PUZZLE = 53208;
    public const int QUEST_ID_MULTIPLAYER_ROAD = 23527;
    public const int QUEST_ID_SECOND_DISTRICT_DUREMUDIRA = 21746;
    public const int QUEST_ID_TWINHEAD_RAJANGS_HISTORIC = 55937;
    public const int QUEST_ID_NUCLEAR_GYPCEROS = 63390;
    public const int QUEST_ID_MOSSWINE_REVENGE = 50143;
    public const int QUEST_ID_MOSSWINE_DUEL = 62793;
    public const int QUEST_ID_MOSSWINE_LAST_STAND = 53323;
    public const int QUEST_ID_HALLOWEEN_SPEEDSTER = 53325;
    public const int QUEST_ID_VR = 53232;

}
