﻿using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The rank bands list
    ///</summary>
    public class RanksBandsList
    {
        public static Dictionary<int, string> RankBandsID = new Dictionary<int, string>()
        {
            {1, "Lower"},
            {2, "Lower"},
            {3, "Lower"},
            {4, "Lower"},
            {5, "Lower"},
            {6, "Lower"},
            {7, "Lower"},
            {8, "Lower"},
            {9, "Lower"},
            {10, "Lower"},
            {11, "Lower/Higher"},
            {12, "Higher"},
            {13, "Higher"},
            {14, "Higher"},
            {15, "Higher"},
            {16, "Higher"},
            {17, "Higher"},
            {18, "Higher"},
            {19, "Higher"},
            {20, "Higher"},
            {26, "HR5"},
            {31, "HR5"},
            {32, "Supremacy 1"}, //needs testing
            {42, "HR5"},
            {46, "Supremacy 2" }, //needs testing. unknown repel+slay uses this
            {53, "G Rank"},
            {54, "Musou 1"},//lower shiten unknown/disu. thirsty/starving.
            {55, "Musou 2"},//upper shiten disu. shifting/golden.
            {56, "Twinhead 1" },//twinhead rajang / voljang and rajang
            {57, "Twinhead 2" },//twinhead mi ru / white and brown espi / unknown and zeru / rajang and dorag
            {64, "Z1"},
            {65, "Z2"},
            {66, "Z3"},
            {67, "Z4"},
            {70,"Upper Shiten" },//upper shiten Unknown
            {71, "Interception"},
            {72, "Interception"},
            {73, "Interception"}
        };
    }

    ///<summary>
    ///The monster ID list
    ///</summary>
    public class List
    {
        public static Dictionary<int, string> MonsterID = new Dictionary<int, string>()
        {
            {0, "None"},//
            {1, "Rathian"},//
            {2, "Fatalis"},//
            {3, "Kelbi"},//
            {4, "Mosswine"},//
            {5, "Bullfango"},//
            {6, "Yian Kut-Ku"},//
            {7, "Lao-Shan Lung"},//
            {8, "Cephadrome"},//
            {9, "Felyne"},//
            {10, "Veggie Elder"},//
            {11, "Rathalos"},//
            {12, "Aptonoth"},//
            {13, "Genprey"},//
            {14, "Diablos"},//
            {15, "Khezu"},//
            {16, "Velociprey"},//
            {17, "Gravios"},//
            {18, "Felyne?"},//
            {19, "Vespoid"},//
            {20, "Gypceros"},//
            {21, "Plesioth"},//
            {22, "Basarios"},//
            {23, "Melynx"},//
            {24, "Hornetaur"},//
            {25, "Apceros"},//
            {26, "Monoblos"},//
            {27, "Velocidrome"},//
            {28, "Gendrome"},//
            {29, "Rocks"},//
            {30, "Ioprey"},//
            {31, "Iodrome"},//
            {32, "Pugis"},//
            {33, "Kirin"},//
            {34, "Cephalos"},//
            {35, "Giaprey / Giadrome"},//
            {36, "Crimson Fatalis"},//
            {37, "Pink Rathian"},//
            {38, "Blue Yian Kut-Ku"},//
            {39, "Purple Gypceros"},//
            {40, "Yian Garuga"},//
            {41, "Silver Rathalos"},//
            {42, "Gold Rathian"},//
            {43, "Black Diablos"},//
            {44, "White Monoblos"},//
            {45, "Red Khezu"},//
            {46, "Green Plesioth"},//
            {47, "Black Gravios"},//
            {48, "Daimyo Hermitaur"},//
            {49, "Azure Rathalos"},//
            {50, "Ashen Lao-Shan Lung"},//
            {51, "Blangonga"},//
            {52, "Congalala"},//
            {53, "Rajang"},//
            {54, "Kushala Daora"},//
            {55, "Shen Gaoren"},//
            {56, "Great Thunderbug"},//
            {57, "Shakalaka"},//
            {58, "Yama Tsukami"},//
            {59, "Chameleos"},//
            {60, "Rusted Kushala Daora"},//
            {61, "Blango"},//
            {62, "Conga"},//
            {63, "Remobra"},//
            {64, "Lunastra"},//
            {65, "Teostra"},//
            {66, "Hermitaur"},//
            {67, "Shogun Ceanataur"},//
            {68, "Bulldrome"},//
            {69, "Anteka"},//
            {70, "Popo"},//
            {71, "White Fatalis"},//
            {72, "Yama Tsukami"},//
            {73, "Ceanataur"},//
            {74, "Hypnocatrice"},//
            {75, "Lavasioth"},//
            {76, "Tigrex"},//
            {77, "Akantor"},//
            {78, "Bright Hypnoc"},//
            {79, "Red Lavasioth"},//
            {80, "Espinas"},//
            {81, "Orange Espinas"},//
            {82, "Silver Hypnoc"},//
            {83, "Akura Vashimu"},//
            {84, "Akura Jebia"},//
            {85, "Berukyurosu"},//
            {86, "Cactus"},//
            {87, "Gorge Objects"},//
            {88, "Gorge Rocks"},//
            {89, "Pariapuria"},//
            {90, "White Espinas"},//
            {91, "Kamu Orugaron"},//
            {92, "Nono Orugaron"},//
            {93, "Raviente"},//
            {94, "Dyuragaua"},//
            {95, "Doragyurosu"},//
            {96, "Gurenzeburu"},//
            {97, "Burukku"},//
            {98, "Erupe"},//
            {99, "Rukodiora"},//
            {100, "UNKNOWN"},//
            {101, "Gogomoa"},//
            {102, "Kokomoa"},//
            {103, "Taikun Zamuza"},//
            {104, "Abiorugu"},//
            {105, "Kuarusepusu"},//
            {106, "Odibatorasu"},//
            {107, "Disufiroa"},//
            {108, "Rebidiora"},//
            {109, "Anorupatisu"},//
            {110, "Hyujikiki"},//
            {111, "Midogaron"},//
            {112, "Giaorugu"},//
            {113, "Mi Ru"},//
            {114, "Farunokku"},//
            {115, "Pokaradon"},//
            {116, "Shantien"},//
            {117, "Pokara"},//
            {118, "Dummy"},//
            {119, "Goruganosu"},//
            {120, "Aruganosu"},//
            {121, "Baruragaru"},//
            {122, "Zerureusu"},//
            {123, "Gougarf"},//
            {124, "Uruki"},//
            {125, "Forokururu"},//
            {126, "Meraginasu"},//
            {127, "Diorex"},//
            {128, "Garuba Daora"},//
            {129, "Inagami"},//
            {130, "Varusaburosu"},//
            {131, "Poborubarumu"},//
            {132, "Duremudira"},//
            {133, "UNK"},//
            {134, "Felyne"},//
            {135, "Blue NPC"},//
            {136, "UNK"},//
            {137, "Cactus"},//
            {138, "Veggie Elders"},//
            {139, "Gureadomosu"},//
            {140, "Harudomerugu"},//
            {141, "Toridcless"},//
            {142, "Gasurabazura"},//
            {143, "Kusubami"},//
            {144, "Yama Kurai"},//
            {145, "3rd Phase Duremudira"},//
            {146, "Zinogre"},//
            {147, "Deviljho"},//
            {148, "Brachydios"},//
            {149, "Berserk Raviente"},//
            {150, "Toa Tesukatora"},//
            {151, "Barioth"},//
            {152, "Uragaan"},//
            {153, "Stygian Zinogre"},//
            {154, "Guanzorumu"},//
            {155, "Starving Deviljho"},//
            {156, "UNK"},//
            {157, "Egyurasu"},//
            {158, "Voljang"},//
            {159, "Nargacuga"},//
            {160, "Keoaruboru"},//
            {161, "Zenaserisu"},//
            {162, "Gore Magala"},//
            {163, "Blinking Nargacuga"},//
            {164, "Shagaru Magala"},//
            {165, "Amatsu"},//
            {166, "Elzelion"},//
            {167, "Arrogant Duremudira"},//
            {168, "Rocks"},//
            {169, "Seregios"},//
            {170, "Bogabadorumu"},//
            {171, "Unknown Blue Barrel"},//
            {172, "Blitzkrieg Bogabadorumu"},//
            {173, "Costumed Uruki"},//
            {174, "Sparkling Zerureusu"},//
            {175, "PSO2 Rappy"},//
            {176, "King Shakalaka"}//
        };
    };

    ///<summary>
    ///The weapon list
    ///</summary>
    public class WeaponList
    {
        public static Dictionary<int, string> WeaponID = new Dictionary<int, string>()
        {
            {0, "Great Sword"},
            {1, "Heavy Bowgun"},
            {2, "Hammer"},
            {3, "Lance"},
            {4, "Sword and Shield"},
            {5, "Light Bowgun"},
            {6, "Dual Swords"},
            {7, "Long Sword"},
            {8, "Hunting Horn"},
            {9, "Gunlance"},
            {10, "Bow"},
            {11, "Tonfa"},
            {12, "Switch Axe F"},
            {13, "Magnet Spike"},
            {14, "Group"}
        };
    };

    ///<summary>
    ///The sharpness list
    ///</summary>
    public class SharpnessList
    {

        public static Dictionary<int, string> SharpnessID = new Dictionary<int, string>()
        {
            {0, "Red"},
            {1, "Orange"},
            {2, "Yellow"},
            {3, "Green"},
            {4, "Blue"},
            {5, "White"},
            {6, "Purple"},
            {7, "Cyan"}
        };
    };

    ///<summary>
    ///The poogie costumes list
    ///</summary>
    public class PoogieCostumeList
    {

        public static Dictionary<int, string> PoogieCostumeID = new Dictionary<int, string>()
        {
            {0, "First Costume"},
            {1, "Kirin Costume"},
            {2, "Smart Costume"},
            {3, "Cheerful Costume"},
            {4, "Wild Costume"},
            {5, "Hypnoc Costume"},
            {6, "Volganos Costume"},
            {7, "Espinas Costume"},
            {8, "Comrade Costume"},
            {9, "Sporty Costume"},
            {10, "Lively Costume"},
            {11, "Akura Costume"},
            {12, "Azul Costume"},
            {13, "Cool Costume"},
            {14, "Fine Costume"},
            {15, "Beru Costume"},
            {16, "Gospel Costume"},
            {17, "Winning Costume"},
            {18, "Miner Costume"},
            {19, "Paria Costume"},
            {20, "Magisa Costume"},
            {21, "Mischievous Costume"},
            {22, "Gatherer Costume"},
            {23, "Kamu Costume"},
            {24, "Nono Costume"},
            {25, "Tasty Costume "},
            {26, "Fishing Costume"},
            {27, "Legendary Costume"},
            {28, "UNK"},
            {29, "Rainbow Costume"},
            {30, "Hope Costume"},
            {31, "Pokara Costume"}
        };

    };

    ///<summary>
    ///The armor skills list
    ///</summary>
    public class ArmorSkillList
    {
        public static Dictionary<int, string> ArmorSkillID = new Dictionary<int, string>()
        {
            {0, "None"},
            {1, "Paralysis Halved"},
            {2, "Negate Paralysis"},
            {3, "Paralysis Doubled"},
            {4, "Sleep Halved"},
            {5, "Negate Sleep"},
            {6, "Sleep Doubled"},
            {7, "Stun Halved"},
            {8, "Negate Stun"},
            {9, "Stun Doubled"},
            {10, "Poison Halved"},
            {11, "Negate Poison"},
            {12, "Double Poison"},
            {13, "Deoderant"},
            {14, "Deoderant2"},
            {15, ""},
            {16, "Snowball Res"},
            {17, "Snowball Res2"},
            {18, ""},
            {19, "Sneak"},
            {20, "Taunt"},
            {21, "Health+10"},
            {22, "Health+20"},
            {23, "Health+30"},
            {24, "Health-10"},
            {25, "Health-20"},
            {26, "Health-30"},
            {27, "Recovery Speed+1"},
            {28, "Recovery Speed+2"},
            {29, "Recovery Speed-1"},
            {30, "Recovery Speed-2"},
            {31, "Razor Sharp+1"},
            {32, "Blunt Edge"},
            {33, "Sharpness+1"},
            {34, ""},
            {35, "Critical Eye+1"},
            {36, "Critical Eye+2"},
            {37, "Critical Eye+3"},
            {38, "Speed Sharpening"},
            {39, "Slothful Sharpening"},
            {40, "Guard+1"},
            {41, "Guard+2"},
            {42, "Guard-1"},
            {43, "Guard-2"},
            {44, "Auto Guard"},
            {45, "Throwing Distance UP"},
            {46, "Reload Speed+1"},
            {47, "Reload Speed+2"},
            {48, "Reload Speed+3"},
            {49, "Reload Speed-1"},
            {50, ""},
            {51, "Auto Reload"},
            {52, "Recoil Reduction+1"},
            {53, "Recoil Reduction+2"},
            {54, "Normal/Rapid UP"},
            {55, "Pierce/Pierce UP"},
            {56, "Pellet/Spread UP"},
            {57, "Normal S Lv1 Add"},
            {58, "Normal S All Add"},
            {59, "Pierce S Lv1 Add"},
            {60, "Pierce S Lv1&2 Add"},
            {61, "Pierce S All Add"},
            {62, "Pellet S LV1 Add"},
            {63, "Pierce S Lv1&2 Add"},
            {64, "Pellet Shot All Add"},
            {65, "Crag S Lv1 Add"},
            {66, "Crag S Lv1&Lv2 Add"},
            {67, "Crag S All Add"},
            {68, "Cluster S Lv1 Add"},
            {69, "Cluster S Lv1&Lv2 Add"},
            {70, "Cluster S All Add"},
            {71, "Status Attack UP"},
            {72, "Bomber"},
            {73, "Hunger Halved"},
            {74, "Hunger Negated"},
            {75, "Hunger UP Sm"},
            {76, "Hunger UP Lg"},
            {77, "Gourmand"},
            {78, "Scavenger"},
            {79, "Attack UP Small"},
            {80, "Attack UP Medium"},
            {81, "Attack UP Large"},
            {82, "Defense+20"},
            {83, "Defense+30"},
            {84, "Defense+60"},
            {85, "Defense-20"},
            {86, "Defense-30"},
            {87, "Defense-40"},
            {88, "Divine Protection"},
            {89, "Goddess Embrace"},
            {90, "Demonic Protection"},
            {91, "Death Gods Embrace"},
            {92, "Earplugs"},
            {93, "High Grade Earplugs"},
            {94, "Anti Theft"},
            {95, "Wide Area+1"},
            {96, "Wide Area+2"},
            {97, "Pro Transporter"},
            {98, "All Res UP+5"},
            {99, "All Res UP+10"},
            {100, "All Res UP+20"},
            {101, "All Res-5"},
            {102, "All Res-10"},
            {103, "All Res-20"},
            {104, "Fire Res+10"},
            {105, "Fire Res+20"},
            {106, "Fire Res+30"},
            {107, "Fire Res-10"},
            {108, "Fire Res-20"},
            {109, "Fire Res-30"},
            {110, "Water Res+10"},
            {111, "Water Res+20"},
            {112, "Water Res+30"},
            {113, "Water Res-10"},
            {114, "Water Res-20"},
            {115, "Water Res-30"},
            {116, "Ice Res+10"},
            {117, "Ice Res+20"},
            {118, "Ice Res+30"},
            {119, "Ice Res-10"},
            {120, "Ice Res-20"},
            {121, "Ice Res-30"},
            {122, "Thunder Res+10"},
            {123, "Thunder Res+20"},
            {124, "Thunder Res+30"},
            {125, "Thunder Res-10"},
            {126, "Thunder Res-20"},
            {127, "Thunder Res-30"},
            {128, "Dragon Res+10"},
            {129, "Dragon Res+20"},
            {130, "Dragon Res+30"},
            {131, "Dragon Res-10"},
            {132, "Dragon Res-20"},
            {133, "Dragon Res-30"},
            {134, "Heat Halved"},
            {135, "Heat Cancel"},
            {136, "Heat Cancel2"},
            {137, "Heat Surge Small"},
            {138, "Heat Surge Large"},
            {139, "Cold Halved"},
            {140, "Cold Cancel"},
            {141, "Cold Cancel2"},
            {142, "Cold Surge Small"},
            {143, "Cold Surge Large"},
            {144, "Wind Res Small"},
            {145, "Wind Res Large"},
            {146, "Dragon Wind Breaker"},
            {147, "Map"},
            {148, ""},
            {149, "Gathering+1"},
            {150, "Gathering+2"},
            {151, "Gathering-1"},
            {152, "Gathering-2"},
            {153, "High Speed Gathering"},
            {154, "Spirits Whim"},
            {155, "Divine Whim"},
            {156, "Spectres Whim"},
            {157, "Devils Whim"},
            {158, "Good Luck"},
            {159, "Great Luck"},
            {160, "Bad Luck"},
            {161, "Horrible Luck"},
            {162, "Fishing Expert"},
            {163, "Detect"},
            {164, "Auto-tracker"},
            {165, "Recovery Items +"},
            {166, "Recovery Items -"},
            {167, "Combination+10%"},
            {168, "Combination+15%"},
            {169, "Combination+30%"},
            {170, "Combination-5%"},
            {171, "Combination-15%"},
            {172, "Maximum Bullets"},
            {173, "Alchemy"},
            {174, ""},
            {175, ""},
            {176, ""},
            {177, "Evasion+1"},
            {178, "Evasion+2"},
            {179, "Adrenaline+1"},
            {180, "Adrenaline+2"},
            {181, "Worry"},
            {182, "Item Duration UP"},
            {183, "Item Duration DOWN"},
            {184, "Marathon Runner"},
            {185, "Short Sprinter"},
            {186, "Load UP"},
            {187, "Deviation DOWN"},
            {188, "Deviation UP"},
            {189, "Come on Big Guy"},
            {190, "Speed Eating"},
            {191, "Slow Eating"},
            {192, "Carving Expert"},
            {193, "Hazard Res Small"},
            {194, "Hazard Res Large"},
            {195, "Hazard Prone Small"},
            {196, "Hazard Prone Large"},
            {197, "Quake Res+1"},
            {198, "Vocal Chord Halved"},
            {199, "Vocal Chord Immunity"},
            {200, "BBQ Expert"},
            {201, "BBQ Master"},
            {202, "False BBQ Expert"},
            {203, "Gunnery"},
            {204, ""},
            {205, ""},
            {206, ""},
            {207, ""},
            {208, ""},
            {209, ""},
            {210, ""},
            {211, ""},
            {212, "Flute Expert"},
            {213, ""},
            {214, ""},
            {215, ""},
            {216, ""},
            {217, ""},
            {218, "Breakout"},
            {219, "Martial Arts"},
            {220, "Strong Arm+1"},
            {221, ""},
            {222, "Inspiration"},
            {223, "Passive"},
            {224, ""},
            {225, ""},
            {226, ""},
            {227, ""},
            {228, "Bond"},
            {229, ""},
            {230, "Guts"},
            {231, "Great Guts"},
            {232, "True Guts"},
            {233, ""},
            {234, "Pressure Small"},
            {235, "Capture Proficiency"},
            {236, ""},
            {237, ""},
            {238, "Poison Coatings Add"},
            {239, "Poison Coating Add"},
            {240, "Sleep Coatings Add"},
            {241, "Fire Attack Small"},
            {242, "Fire Attack Large"},
            {243, "Water Attack Small"},
            {244, "Water Attack Large"},
            {245, "Thunder Attack Small"},
            {246, "Thunder Attack Large"},
            {247, "Ice Attack Small"},
            {248, "Ice Attack Large"},
            {249, "Dragon Attack Small"},
            {250, "Dragon Attack Large"},
            {251, "Starving Wolf+1"},
            {252, "Starving Wolf+2"},
            {253, ""},
            {254, "Bomb Sword+1"},
            {255, "Bomb Sword+2"},
            {256, "Bomb Sword+3"},
            {257, "Assault Sword+1"},
            {258, "Assault Sword+2"},
            {259, "Assault Sword+3"},
            {260, "Poison Sword+1"},
            {261, "Poison Sword+2"},
            {262, "Poison Sword+3"},
            {263, "Paralysis Sword+1"},
            {264, "Paralysis Sword+2"},
            {265, "Paralysis Sword+3"},
            {266, "Sleep Sword+1"},
            {267, "Sleep Sword+2"},
            {268, "Sleep Sword+3"},
            {269, "Fire Sword+1"},
            {270, "Fire Sword+2"},
            {271, "Fire Sword+3"},
            {272, "Water Sword+1"},
            {273, "Water Sword+2"},
            {274, "Water Sword+3"},
            {275, "Ice Sword+1"},
            {276, "Ice Sword+2"},
            {277, "Ice Sword+3"},
            {278, "Thunder Sword+1"},
            {279, "Thunder Sword+2"},
            {280, "Thunder Sword+3"},
            {281, "Dragon Sword+1"},
            {282, "Dragon Sword+2"},
            {283, "Dragon Sword+3"},
            {284, "Critical Eye+4"},
            {285, "Critical Eye+5"},
            {286, "Attack UP Very Large"},
            {287, "Attack UP Absolute"},
            {288, "Super Earplugs"},
            {289, "Peerless"},
            {290, "Precision+2"},
            {291, "Health+40"},
            {292, "Health+50"},
            {293, "Razor Sharp+2"},
            {294, "Defense+90"},
            {295, "Defense+120"},
            {296, "Summer Person"},
            {297, "Winter General"},
            {298, "Artillery Expert"},
            {299, "Artillery God"},
            {300, "Pressure Large"},
            {301, "Capture Guru"},
            {302, "Focus+2"},
            {303, "Focus+1"},
            {304, "Distraction"},
            {305, "SnS Tech Sword Saint"},
            {306, "SnS Tech Kaiden"},
            {307, "SnS Tech Expert"},
            {308, "SnS Tech Novice"},
            {309, "DS Tech Dual Dragon"},
            {310, "DS Tech Kaiden"},
            {311, "DS Tech Expert"},
            {312, "DS Tech Novice"},
            {313, "GS Tech Sword King"},
            {314, "GS Tech Kaiden"},
            {315, "GS Tech Expert"},
            {316, "GS Tech Novice"},
            {317, "LS Tech Katana God"},
            {318, "LS Tech Kaiden"},
            {319, "LS Tech Expert"},
            {320, "LS Tech Novice"},
            {321, "Hammer Tech Blunt Beast"},
            {322, "Hammer Tech Kaiden"},
            {323, "Hammer Tech Expert"},
            {324, "Hammer Tech Novice"},
            {325, "HH Tech Flamboyant Emperor"},
            {326, "HH Tech Kaiden"},
            {327, "HH Tech Expert"},
            {328, "HH Tech Novice"},
            {329, "Lance Tech Heavenly Spear"},
            {330, "Lance Tech Kaiden"},
            {331, "Lance Tech Expert"},
            {332, "Lance Tech Novice"},
            {333, "GL Tech Cannon Ruler"},
            {334, "GL Tech Kaiden"},
            {335, "GL Tech Expert"},
            {336, "GL Tech Novice"},
            {337, "HBG Tech Gun Sage"},
            {338, "HBG Tech Kaiden"},
            {339, "HBG Tech Expert"},
            {340, "HBG Tech Novice"},
            {341, "LBG Tech Gun Prodigy"},
            {342, "LBG Tech Kaiden"},
            {343, "LBG Tech Expert"},
            {344, "LBG Tech Novice"},
            {345, "Bow Tech Bow Demon"},
            {346, "Bow Tech Kaiden"},
            {347, "Bow Tech Expert"},
            {348, "Bow Tech Novice"},
            {349, "Violent Wind Breaker"},
            {350, "Quake Res+2"},
            {351, "Trap Expert"},
            {352, "Trap Master"},
            {353, "Weapon Handling"},
            {354, "Elemental Attack UP"},
            {355, "Elemental Attack DOWN"},
            {356, "Stamina Rec Large"},
            {357, "Stamina Rec Small"},
            {358, "Stamina Rec DOWN"},
            {359, "Kickboxing King"},
            {360, "Strong Arm+2"},
            {361, "Throwing Knives+1"},
            {362, "Throwing Knives+2"},
            {363, "Caring+1"},
            {364, "Caring+2"},
            {365, "Caring+3"},
            {366, "Def Lock"},
            {367, "Fencing+1"},
            {368, "Fencing+2"},
            {369, "Status Halved"},
            {370, "Status Immunity"},
            {371, "Status Doubled"},
            {372, "SnS Tech Sword Saint2"},
            {373, "DS Tech Dual Dragon2"},
            {374, "GS Tech Sword King2"},
            {375, "LS Tech Katana God2"},
            {376, "Hammer Tech Blunt Beast2"},
            {377, "HH Tech Flamboyant Emperor2"},
            {378, "Lance Tech Heavenly Spear2"},
            {379, "GL Tech Cannon Emperor2"},
            {380, "HBG Tech Gun Sage2"},
            {381, "LBG Tech Gun Prodigy2"},
            {382, "Bow Tech Bow Demon2"},
            {383, "Passive+1"},
            {384, "Wide Area+3"},
            {385, "Wide Area-1"},
            {386, "Heavy Drinker"},
            {387, "Drunkard"},
            {388, "Crystal Res"},
            {389, "Crystal Vulnerability"},
            {390, "Magnetic Res"},
            {391, "Magnet Vulnerability"},
            {392, "Light Tread"},
            {393, "Relief"},
            {394, "Shiriagari"},
            {395, "Lone Wolf"},
            {396, "Compensation"},
            {397, "Rapid Fire"},
            {398, "Sharpening Artisan"},
            {399, "Unaffected+1"},
            {400, "Unaffected+2"},
            {401, "Unaffected+3"},
            {402, "Reflect+1"},
            {403, "Reflect+2"},
            {404, "Reflect+3"},
            {405, "Honed Blade+1"},
            {406, "Honed Blade+2"},
            {407, "Honed Blade+3"},
            {408, "Strong Attack+1"},
            {409, "Strong Attack+2"},
            {410, "Strong Attack+3"},
            {411, "Strong Attack+4"},
            {412, "Strong Attack+5"},
            {413, "Encourage+1"},
            {414, "Encourage+2"},
            {415, "Grace+1"},
            {416, "Grace+2"},
            {417, "Grace+3"},
            {418, "Vitality+1"},
            {419, "Vitality+2"},
            {420, "Vitality+3"},
            {421, "Vitality-1"},
            {422, "Wrath Awoken"},
            {423, "Buchigire"},
            {424, "Iron Arm+1"},
            {425, "Iron Arm+2"},
            {426, "Breeder"},
            {427, "unk1"},
            {428, "unk2"},
            {429, "Issen+1"},
            {430, "Issen+2"},
            {431, "Issen+3"},
            {432, "Fortify"},
            {433, "Tenacity"},
            {434, "Steady Hand+1"},
            {435, "Mounting+1"},
            {436, "Mounting+2"},
            {437, "Mounting+3"},
            {438, "Exploit Weakness"},
            {439, "Exploit Weakness2"},
            {440, "Reduce Weakness"},
            {441, "Combo Expert+1"},
            {442, "Combo Expert+2"},
            {443, "Combo Expert+3"},
            {444, "Combo Expert-1"},
            {445, "Hunter Life"},
            {446, "Hunter Valhalla"},
            {447, "Critical Shot+1"},
            {448, "Critical Shot+2"},
            {449, "Critical Shot+3"},
            {450, "Evasion+3"},
            {451, "Movement Speed UP+1"},
            {452, "Movement Speed UP+2"},
            {453, "Saving Master"},
            {454, "Saving Expert"},
            {455, "Charge Attack UP+1"},
            {456, "Charge Attack UP+2"},
            {457, "Evade Distance UP"},
            {458, "Red Soul"},
            {459, "Blue Soul"},
            {460, "Vampirism+1"},
            {461, "Vampirism+2"},
            {462, "Adaptation+1"},
            {463, "Adaptation+2"},
            {464, "Dark Finale"},
            {465, "Medical Sage"},
            {466, "Tonfa Tech Piercing Phoenix"},
            {467, "Tonfa Tech Kaiden"},
            {468, "Tonfa Tech Expert"},
            {469, "Tonfa Tech Novice"},
            {470, "Tonfa Tech Large Dragon Club"},
            {471, "Incitement"},
            {472, "Blazing Majesty+1"},
            {473, "Blazing Majesty+2"},
            {474, "Drug Knowledge"},
            {475, "Absolute Defense"},
            {476, "Imperturbable"},
            {477, "Fully Prepared"},
            {478, "Negligence"},
            {479, "Extreme Collection"},
            {480, "Stylish"},
            {481, "Assistance"},
            {482, "Recoil Reduction+3"},
            {483, "Gentle Shot+1"},
            {484, "Gentle Shot+2"},
            {485, "Gentle Shot+3"},
            {486, "Elemental Exploit"},
            {487, "Elemental Diffusion"},
            {488, "Combat Supremacy"},
            {489, "Vigorous"},
            {490, "Vigorous+2"},
            {491, "Status Immunity Myriad"},
            {492, "Sword God+1"},
            {493, "Sword God+2"},
            {494, "Thunder Clad"},
            {495, "Status Pursuit"},
            {496, "Drawing Arts+1"},
            {497, "Drawing Arts+2"},
            {498, "Blast Resistance"},
            {499, "Crit Conversion"},
            {500, "Crit Conversion2"},
            {501, "Solid Determination"},
            {502, "Stylish Assault"},
            {503, "Freeze Res"},
            {504, "Ice Age"},
            {505, "Consumption Slayer"},
            {506, "Swaxe Tech Edge Marshal"},
            {507, "Swaxe Tech Kaiden"},
            {508, "Swaxe Tech Expert"},
            {509, "Swaxe Tech Novice"},
            {510, "Swaxe Tech Great Sword General"},
            {511, "Fortification+1"},
            {512, "Fortification+2"},
            {513, "Sniper"},
            {514, "Obscurity"},
            {515, "Evasion Boost"},
            {516, "Rush"},
            {517, "Encourage +2"},
            {518, "Reflect +3"},
            {519, "Skilled"},
            {520, "Ceaseless"},
            {521, "Point Breakthrough"},
            {522, "Abnormality"},
            {523, "Spacing"},
            {524, "Strong Attack+6"},
            {525, "Sword God+3"},
            {526, "Steady Hand+2"},
            {527, "Trained+1"},
            {528, "Trained+2"},
            {529, "Furious"},
            {530, "Magspike Tech Magnetic Star"},
            {531, "Magspike Tech Kaiden"},
            {532, "Magspike Tech Expert"},
            {533, "Magspike Tech Novice"},
            {534, "Magspike Tech Large Magnetic Star"}
        };
    };

    ///<summary>
    ///The objective type list
    ///</summary>
    public class ObjectiveTypeList
    {

        public static Dictionary<int, string> ObjectiveTypeID = new Dictionary<int, string>()
        {
            {0x0,"Nothing" },
            {0x1,"Hunt" },
            {0x101,"Capture" },
            {0x201,"Slay" },
            {0x8004,"Damage" },
            {0x18004,"Slay or Damage" },
            {0x40000,"Slay All" },
            {0x20000,"Slay Total" },
            {0x02,"Deliver" },
            {0x4004,"Break Part" },
            {0x1002,"Deliver Flag" },
            {0x10,"Esoteric Action" } //what is this for?
        };
    };

    ///<summary>
    ///The map areas list
    ///</summary>
    public class MapAreaList
    {

        public static Dictionary<int, string> MapAreaID = new Dictionary<int, string>()
        {
            {0,"Loading" },
            {1,"Jungle Night Base Camp" },
            {2,"Jungle Night Area 1" },
            {3,"Jungle Night Area 2" },
            {4,"Jungle Night Area 3" },
            {5,"Jungle Night Area 4" },
            {6,"Snowy Mountains Day Secret Area" },
            {7,"Desert Day Secret Area" },
            {8,"Volcano Day Secret Area" },
            {9,"Swamp Day Secret Area" },
            {10,"Siege Fortress Day" },
            {11,"Siege Fortress Day" },
            {12,"Siege Fortress Day" },
            {13,"Arena with Moat Day Area 1" },
            {14,"Siege Fortress Day" },
            {15,"Snowy Mountains Night Secret Area" },
            {16,"Swamp Night Base Camp" },
            {17,"Arena with Moat Night Area 1" },
            {18,"Jungle Night Area 5" },
            {19,"Jungle Night Area 9" },
            {20,"Castle Schrade Base Camp/Area 1" },
            {21,"Forest and Hills Day Base Camp" },
            {22,"Jungle Night Area 8" },
            {23,"Jungle Night Area 7" },
            {24,"Desert Night Secret Area" },
            {25,"Castle Schrade Area 2" },
            {26,"Jungle Night Area 6" },
            {27,"Volcano Night Secret Area" },
            {28,"Siege Fortress Day" },
            {29,"Swamp Night Secret Area" },
            {30,"Siege Fortress Day" },
            {31,"Siege Fortress Day" },
            {32,"Forest and Hills Day Area 6" },
            {33,"Forest and Hills Day Area 3" },
            {34,"Forest and Hills Day Area 10" },
            {35,"Forest and Hills Day Area 8" },
            {36,"Forest and Hills Day Area 9" },
            {37,"Forest and Hills Day Area 4" },
            {38,"Forest and Hills Day Area 2" },
            {39,"Forest and Hills Day Area 1" },
            {40,"Forest and Hills Day Area 5" },
            {41,"Forest and Hills Day Area 7" },
            {42,"Forest and Hills Day Area 11" },
            {43,"Forest and Hills Day Area 12" },
            {44,"Duplicate of Swamp Night Area 1" },
            {45,"Desert Base Camp Night" },
            {46,"Arena with Moat Night Base Camp" },
            {47,"Desert Night Area 2" },
            {48,"Desert Night Area 7" },
            {49,"Desert Night Area 3" },
            {50,"Desert Night Area 6" },
            {51,"Desert Night Area 5" },
            {52,"Desert Night Area 1" },
            {53,"Desert Night Area 4" },
            {54,"Desert Night Area 8" },
            {55,"Desert Night Area 9" },
            {56,"Desert Night Area 10" },
            {57,"Arena with Pillar Night Area 1" },
            {58,"Volcano Night Base Camp" },
            {59,"Volcano Night Area 4" },
            {60,"Volcano Night Area 5" },
            {61,"Volcano Night Area 1" },
            {62,"Volcano Night Area 2" },
            {63,"Volcano Night Area 3" },
            {64,"Volcano Night Area 7" },
            {65,"Volcano Night Area 6" },
            {66,"Arena with Pillar (Cat Arena) Night Area 1" },
            {67,"Swamp Night Area 2" },
            {68,"Swamp Night Area 3" },
            {69,"Swamp Night Area 9" },
            {70,"Swamp Night Area 8" },
            {71,"Swamp Night Area 6" },
            {72,"Swamp Night Area 5" },
            {73,"Swamp Night Area 7" },
            {74,"Volcano Night Secret Area" },
            {75,"Swamp Night Area 4" },
            {76,"" },
            {77,"" },
            {78,"" },
            {79,"" },
            {80,"" },
            {81,"" },
            {82,"" },
            {83,"Tower 3 Area 8" },
            {84,"Tower 3 Area 9" },
            {85,"Absent" },
            {86,"Absent" },
            {87,"Kokoto Village" },
            {88,"Crimson Battleground" },
            {89,"Arena with Pillar (Cat Arena) Day Base Camp" },
            {90,"Arena with Ledge Day" },
            {91,"Arena with Pillar (Cat Arena) Day Area 1" },
            {92,"Snowy Mountains Day Area 4" },
            {93,"Snowy Mountains Day Area 5" },
            {94,"Snowy Mountains Day Area 3" },
            {95,"Snowy Mountains Day Area 2" },
            {96,"Snowy Mountains Day Area 7" },
            {97,"Snowy Mountains Day Area 8" },
            {98,"Snowy Mountains Day Area Base Camp" },
            {99,"Snowy Mountains Day Area 1" },
            {100,"Snowy Mountains Day Area 6" },
            {101,"Snowy Mountains Night Area 4" },
            {102,"Snowy Mountains Night Area 5" },
            {103,"Snowy Mountains Night Area 3" },
            {104,"Snowy Mountains Night Area 2" },
            {105,"Snowy Mountains Night Area 7" },
            {106,"Snowy Mountains Night Area 8" },
            {107,"Snowy Mountains Night Area Base Camp" },
            {108,"Snowy Mountains Night Area 1" },
            {109,"Snowy Mountains Night Area 6" },
            {110,"Jungle Day Base Camp" },
            {111,"Jungle Day Area 10" },
            {112,"Jungle Day Area 2" },
            {113,"Jungle Day Area 3" },
            {114,"Jungle Day Area 4" },
            {115,"Jungle Day Area 5" },
            {116,"Jungle Day Area 9" },
            {117,"Jungle Day Area 8" },
            {118,"Jungle Day Area 7" },
            {119,"Jungle Day Area 6" },
            {120,"Jungle Day Area 10" },
            {121,"Tower 3 Base Camp" },
            {122,"Tower 3 Area 1" },
            {123,"Tower 3 Area 2" },
            {124,"Tower 3 Area 10" },
            {125,"Tower 3 Area 6" },
            {126,"Tower 1 Area 8" },
            {127,"Tower 3 Area 7" },
            {128,"Tower 3 Area 5" },
            {129,"Tower 3 Area 4" },
            {130,"Tower 3 Area 3" },
            {131,"Dundorma Town Square" },
            {132,"Dundorma Main Area" },
            {133,"Dundorma Smithy" },
            {134,"Dundorma Public Tavern" },
            {135,"Dundorma Arena" },
            {136,"Dundorma Grand Priest" },
            {137,"Broken" },
            {138,"Tower 2" },
            {139,"Tower 1 Area 9 (Nest Hole)" },
            {140,"Desert Day Base Camp" },
            {141,"Desert Day Area 2" },
            {142,"Desert Day Area 7" },
            {143,"Desert Day Area 3" },
            {144,"Desert Day Area 6" },
            {145,"Desert Day Area 5" },
            {146,"Desert Day Area 1" },
            {147,"Desert Day Area 4" },
            {148,"Desert Day Area 8" },
            {149,"Desert Day Area 9" },
            {150,"Desert Day Area 10" },
            {151,"Swamp Day Base Camp" },
            {152,"Swamp Day Area 1" },
            {153,"Swamp Day Area 2" },
            {154,"Swamp Day Area 3" },
            {155,"Swamp Day Area 9" },
            {156,"Swamp Day Area 8" },
            {157,"Swamp Day Area 7" },
            {158,"Swamp Day Area 5" },
            {159,"Swamp Day Area 7" },
            {160,"Swamp Day Area 4" },
            {161,"Volcano Day Base Camp" },
            {162,"Volcano Day Area 4" },
            {163,"Volcano Day Area 5" },
            {164,"Volcano Day Area 1" },
            {165,"Volcano Day Area 2" },
            {166,"Volcano Day Area 3" },
            {167,"Volcano Day Area 7" },
            {168,"Volcano Day Area 6" },
            {169,"Volcano Day Area 8" },
            {170,"Broken" },
            {171,"Broken" },
            {172,"Broken" },
            {173,"My House (original)" },
            {174,"Broken" },
            {175,"My House (MAX)" },//also house
            {176,"Dundorma Fort 03" },
            {177,"Dundorma Fort 02" },
            {178,"Dundorma Fort 01" },
            {179,"Dundorma Fort Base Camp" },
            {180,"Dundorma Fort 03" },
            {181,"Dundorma Fort 02" },
            {182,"Dundorma Fort 01" },
            {183,"Dundorma Fort Base Camp" },
            {184,"Forest and Hills Night Base Camp" },
            {185,"Forest and Hills Night Area 6" },
            {186,"Forest and Hills Night Area 3" },
            {187,"Forest and Hills Night Area 10" },
            {188,"Forest and Hills Night Area 8" },
            {189,"Forest and Hills Night Area 9" },
            {190,"Forest and Hills Night Area 4" },
            {191,"Forest and Hills Night Area 2" },
            {192,"Forest and Hills Night Area 1" },
            {193,"Forest and Hills Night Area 5" },
            {194,"Forest and Hills Night Area 7" },
            {195,"Forest and Hills Night Area 11" },
            {196,"Forest and Hills Night Area 12" },
            {197,"Akantor Battleground" },
            {198,"Absent" },
            {199,"Absent" },
            {200,"Mezeporta" },
            {201,"Hairdresser" },
            {202,"Guild Hall Lv1" },
            {203,"Guild Hall Lv2" },
            {204,"Guild Hall LvMAX" },
            {205,"Pugi Farm" },
            {206,"Old Town Area" },
            {207,"Old Town Area" },
            {208,"Absent" },
            {209,"Absent" },
            {210,"Private Bar" },
            {211,"Rasta Bar" },
            {212,"Jungle Pioneer Secret 01" },
            {213,"Jungle Pioneer Secret 02" },
            {214,"Desert Pioneer Secret 01" },
            {215,"Desert Pioneer Secret 02" },
            {216,"Volcano Pioneer Secret 01" },
            {217,"Volcano Pioneer Secret 02" },
            {218,"Snowy Mountain Pioneer Secret 01" },
            {219,"Snowy Mountain Pioneer Secret 02" },
            {220,"Volcano Day Area 9" },
            {221,"Volcano Night Area 9" },
            {222,"Volcano Day Area 10" },
            {223,"Volcano Night Area 10" },
            {224,"Great Forest Day Base Camp" },
            {225,"Great Forest Night Base Camp" },
            {226,"Great Forest Day Area 1" },
            {227,"Great Forest Night Area 1" },
            {228,"Great Forest Day Area 2" },
            {229,"Great Forest Night Area 2" },
            {230,"Great Forest Day Area 3" },
            {231,"Great Forest Night Area 3" },
            {232,"Great Forest Day Area 4" },
            {233,"Great Forest Night Area 4" },
            {234,"Great Forest Day Area 5" },
            {235,"Great Forest Night Area 5" },
            {236,"Great Forest Day Area 6" },
            {237,"Great Forest Night Area 6" },
            {238,"Great Forest Day Area 7" },
            {239,"Great Forest Night Area 7" },
            {240,"Great Forest Day Area 8" },
            {241,"Great Forest Night Area 8" },
            {242,"Unused Version of the Top of G.Forest (Day)" },
            {243,"Unused Version of the Top of G.Forest (Night)" },
            {244,"Code Claiming Room" },
            {245,"Top of Great Forest Day Base Camp" },
            {246,"Top of Great Forest Day Area 1" },
            {247,"Highlands Day Base Camp" },
            {248,"Highlands Night Base Camp" },
            {249,"Highlands Day Area 1" },
            {250,"Highlands Night Area 1" },
            {251,"Highlands Day Area 2" },
            {252,"Highlands Night Area 2" },
            {253,"Highlands Day Area 3" },
            {254,"Highlands Night Area 3" },
            {255,"Highlands Day Area 4" },
            {256,"Caravan Changing Area (Night/Day)" },
            {257,"Blacksmith" },
            {258,"Caravan Blimp Base Camp" },
            {259,"Caravan Blimp Base Camp" },
            {260,"Caravan Area (Ravi \"Great Reward Clause\")" },
            {261,"Caravan Changing Area" },
            {262,"Caravan Guest House" },
            {263,"Caravan Guest House Second Floor" },
            {264,"Gallery" },
            {265,"Guuku Farm" },
            {266,"Absent" },
            {267,"Absent" },
            {268,"Absent" },
            {269,"Absent" },
            {270,"Absent" },
            {271,"Absent" },
            {272,"Interception Base, Fortress Day Base Camp" },
            {273,"Fortress Night Base Camp" },
            {274,"Interception Base, Fortress Day Area 1" },
            {275,"Fortress Night Area 1" },
            {276,"Fortress Day Area 2" },
            {277,"Fortress Night Area 2" },
            {278,"Interception Base, Fortress Day Area 3" },
            {279,"Fortress Night Area 3" },
            {280,"Arena with Moat Day Base Camp" },
            {281,"Arena with Moat Night Base Camp" },
            {282,"Cities Map" },
            {283,"Halk Area" },
            {284,"Absent" },
            {285,"Absent" },
            {286,"PvP Room" },
            {287,"Absent" },
            {288,"Gorge Day Base Camp" },
            {289,"Gorge Night Base Camp" },
            {290,"Gorge Day Base Camp" },
            {291,"Gorge Night Area 1" },
            {292,"Gorge Day Area 1" },
            {293,"Gorge Night Area 2" },
            {294,"Gorge Day Area 2" },
            {295,"Gorge Night Area 3" },
            {296,"Gorge Day Area 3" },
            {297,"Gorge Night Area 4" },
            {298,"Gorge Day Area 4" },
            {299,"Gorge Night Area 5" },
            {300,"Gorge Day Area 5" },
            {301,"Gorge Night Area 6" },
            {302,"Highlands Night Area 4" },
            {303,"Highlands Day Area 5" },
            {304,"Highlands Night Area 5" },
            {305,"Highlands Day Area 6" },
            {306,"Highlands Night Area 6" },
            {307,"Highlands Day Area 7" },
            {308,"Highlands Night Area 7" },
            {309,"Solitude Island 9 (Area 3)" },
            {310,"Campaign Tent" }, //in quest?
            {311,"Raviente Balloon (Day)" },
            {312,"Raviente Balloon (Sunset)" },
            {313,"Raviente Balloon (Night)"},
            {314,"Solitude Island 1 (Area 1)" },
            {315,"Solitude Island 2 (Area 1)" },
            {316,"Solitude Island 3 (Area 1)" },
            {317,"Solitude Island 4 (Area 2)" },
            {318,"Solitude Island 5 (Area 2)" },
            {319,"Solitude Island 6 (Area 2)" },
            {320,"Solitude Island 7 (Area 3)" },
            {321,"Solitude Island 8 (Area 3)" },
            {322,"Tidal Island Day Base Camp" },
            {323,"Tidal Island Day Area 1" },
            {324,"Tidal Island Day Area 2" },
            {325,"Tidal Island Day Area 3" },
            {326,"Tidal Island Day" },
            {327,"Tidal Island Day" },
            {328,"Tidal Island Day" },
            {329,"Tidal Island Day" },
            {330,"Tidal Island Base Camp (Night)" },
            {331,"Tidal Island Area 1 (Night)" },
            {332,"Tidal Island Area 2 (Night)" },
            {333,"Tidal Island Area 3 (Night)" },
            {334,"Tidal Island Day Area 4" },
            {335,"Tidal Island Day Area 5" },
            {336,"Tidal Island Day" },
            {337,"Tidal Island (Orange Area 5)" },
            {338,"Tidal Island Area 4 (Night)" },
            {339,"Tidal Island Area 5 (Night)" },
            {340,"SR Room" },
            {341,"SR Room" },
            {342,"Large Airship Base Camp Day" },
            {343,"Large Airship Deck Day" },
            {344,"Large Airship Crashed" },
            {345,"Polar Sea Base Camp Day" },
            {346,"Polar Sea Night" },
            {347,"Polar Sea Day Area 1" },
            {348,"Polar Sea Night Area 1" },
            {349,"Polar Sea Day Area 2" },
            {350,"Polar Sea Night Area 2" },
            {351,"Polar Sea Day Area 3" },
            {352,"Polar Sea Night Area 3" },
            {353,"Polar Sea Day Area 4" },
            {354,"Polar Sea Night Area 4" },
            {355,"Polar Sea Day Area 5" },
            {356,"Polar Sea Night Area 5" },
            {357,"Polar Sea Day Area 6" },
            {358,"Polar Sea Night Area 6" },
            {359,"World's End Base Camp" },
            {360,"World's End Area 1" },
            {361,"Flower Field Day Base Camp" },
            {362,"Flower Field Night Base Camp" },
            {363,"Flower Field Day Area 1" },
            {364,"Flower Field Night Area 1" },
            {365,"Flower Field Day Area 2" },
            {366,"Flower Field Night Area 2" },
            {367,"Flower Field Day Area 3" },
            {368,"Flower Field Night Area 3" },
            {369,"Flower Field Day Area 4" },
            {370,"Flower Field Night Area 4" },
            {371,"Flower Field Day Area 5" },
            {372,"Flower Field Night Area 5" },
            {373,"Top of Tower" },
            {374,"Deep Crater Area 1" },
            {375,"Bamboo Entrance Day" },
            {376,"Bamboo Entrance Night" },
            {377,"Bamboo Forest Day Area 1" },
            {378,"Bamboo Forest Night Area 1" },
            {379,"Diva Hall" },
            {380,"Battlefield Entrance, Deep Crater Base Camp" },
            {381,"Absent" },
            {382,"Absent" },
            {383,"Absent" },
            {384,"Absent" },
            {385,"Absent" },
            {386,"Absent" },
            {387,"Absent" },
            {388,"Absent" },
            {389,"Absent" },
            {390,"Tower 01" },
            {391,"Tower Airship" },
            {392,"Tower Entrance" },
            {393,"Tower 02" },
            {394,"Urgent Tower" },
            {395,"Absent" },
            {396,"Battlefield (Varusa)" },
            {397,"Mezeporta Dupe (nonHD)" },
            {398,"Duremudira Arena" },
            {399,"Duremudira Doorway" },
            {400,"White Lake Day Base Camp" },
            {401,"White Lake Night Base Camp" },
            {402,"White Lake Day Area 1" },
            {403,"White Lake Night Area 1" },
            {404,"White Lake Day Area 2" },
            {405,"White Lake Night Area 2" },
            {406,"White Lake Day Area 3" },
            {407,"White Lake Night Area 3" },
            {408,"White Lake Day Area 4" },
            {409,"White Lake Night Area 4" },
            {410,"White Lake Day Area 5" },
            {411,"White Lake Night Area 5" },
            {412,"White Lake Day Area 6" },
            {413,"White Lake Night Area 6" },
            {414,"Duremudira Door" },
            {415,"Urgent Tower" },
            {416,"4th District Tower" },
            {417,"Berserk Raviente Base Camp" },
            {418,"Berserk Raviente Combat Phase 1" },
            {419,"Berserk Raviente Combat Phase 2" },
            {420,"Berserk Raviente Combat Phase 3" },
            {421,"Berserk Raviente Combat Phase 4" },
            {422,"Berserk Raviente Combat Phase 5" },
            {423,"Painted Falls Day Base Camp" },
            {424,"Painted Falls Night Base Camp" },
            {425,"Painted Falls Day Area 1" },
            {426,"Painted Falls Night Area 1" },
            {427,"Painted Falls Day Area 2" },
            {428,"Painted Falls Night Area 2" },
            {429,"Painted Falls Day Area 3" },
            {430,"Painted Falls Night Area 3" },
            {431,"Painted Falls Day Area 4" },
            {432,"Painted Falls Night Area 4" },
            {433,"Painted Falls Day Area 5" },
            {434,"Painted Falls Night Area 5" },
            {435,"Painted Falls Day Area 6" },
            {436,"Painted Falls Night Area 6" },
            {437,"Berserk Raviente Base Camp (Support)" },//TODO: test
            {438,"Guanzorumu Arena Base Camp" },
            {439,"Guanzorumu Arena Area 1" },
            {440,"Berserk Raviente Support Phase 1" },
            {441,"Berserk Raviente Support Phase 2" },
            {442,"Berserk Raviente Support Phase 3" },
            {443,"Berserk Raviente Support Phase 4" },
            {444,"Berserk Raviente Support Phase 5" },
            {445,"Diva Hall" },
            {446,"Amatsu Arena 1" },
            {447,"Amatsu Arena 2" },
            {448,"Shagaru Arena 1" },
            {449,"Shagaru Arena 2" },
            {450,"Interception/Fortress Base Camp" },
            {451,"Interception/Fortress Base Camp" },
            {452,"Interception/Fortress Area 1" },
            {453,"Interception/Fortress Area 1" },
            {454,"Interception/Fortress Area 2" },
            {455,"Interception/Fortress Area 2" },
            {456,"Interception/Fortress Area 3 (Main Area)" },
            {457,"Interception/Fortress Area 3 (Main Area)" },
            {458,"Hunter's Road Area 1" },
            {459,"Hunter's Road Base Camp" },
            {460,"Historic Base Camp" },
            {461,"Historic Area 1" },
            {462,"MezFes Entrance" },
            {463,"Volpkun Together" },
            {464,"Uruki Pachinko" },
            {465,"MezFes Minigame" },
            {466,"Guuku Scoop" },
            {467,"Nyanrendo" },
            {468,"Panic Honey" },
            {469,"Dokkan Battle Cats" }
        };
    };

    ///<summary>
    ///The armor colors list
    ///</summary>
    public class ArmorColorList
    {

        public static Dictionary<int, string> ArmorColorID = new Dictionary<int, string>()
        {
            {0, "Material Green 0"},
            {1, "Powerful Red 0"},
            {2, "Seized White 0"},
            {3, "Festival Pink 0"},
            {4, "Friendly Blue 0"},
            {5, "Rare Navy 0"},
            {6, "Armored Cyan 0"},
            {7, "Combat Purple 0"},
            {8, "Guided Orange 0"},
            {9, "Gigantic Yellow 0"},
            {10, "Dining Grey 0"},
            {11, "Changing Rainbow"},
            {12, "Material Green 1"},
            {13, "Material Green 2"},
            {14, "Material Green 3"},
            {15, "Material Green 4"},
            {16, ""},
            {17, ""},
            {18, ""},
            {19, ""},
            {20, ""},
            {21, "Powerful Red 1"},
            {22, "Powerful Red 2"},
            {23, "Powerful Red 3"},
            {24, "Powerful Red 4"},
            {25, ""},
            {26, ""},
            {27, ""},
            {28, ""},
            {29, ""},
            {30, ""},
            {31, ""},
            {32, ""},
            {33, ""},
            {34, ""},
            {35, ""},
            {36, ""},
            {37, ""},
            {38, ""},
            {39, "Festival Pink 1"},
            {40, "Festival Pink 2"},
            {41, "Festival Pink 3"},
            {42, "Festival Pink 4"},
            {43, ""},
            {44, ""},
            {45, ""},
            {46, ""},
            {47, ""},
            {48, "Friendly Blue 1"},
            {49, "Friendly Blue 2"},
            {50, "Friendly Blue 3"},
            {51, "Friendly Blue 4"},
            {52, ""},
            {53, ""},
            {54, ""},
            {55, ""},
            {56, ""},
            {57, "Rare Navy 1"},
            {58, "Rare Navy 2"},
            {59, "Rare Navy 3"},
            {60, "Rare Navy 4"},
            {61, ""},
            {62, ""},
            {63, ""},
            {64, ""},
            {65, ""},
            {66, "Armored Cyan 1"},
            {67, "Armored Cyan 2"},
            {68, "Armored Cyan 3"},
            {69, "Armored Cyan 4"},
            {70, ""},
            {71, ""},
            {72, ""},
            {73, ""},
            {74, ""},
            {75, "Combat Purple 1"},
            {76, "Combat Purple 2"},
            {77, "Combat Purple 3"},
            {78, "Combat Purple 4"},
            {79, ""},
            {80, ""},
            {81, ""},
            {82, ""},
            {83, ""},
            {84, "Guided Orange 1"},
            {85, "Guided Orange 2"},
            {86, "Guided Orange 3"},
            {87, "Guided Orange 4"},
            {88, ""},
            {89, ""},
            {90, ""},
            {91, ""},
            {92, ""},
            {93, "Gigantic Yellow 1"},
            {94, "Gigantic Yellow 2"},
            {95, "Gigantic Yellow 3"},
            {96, "Gigantic Yellow 4"},
            {97, ""},
            {98, ""},
            {99, ""},
            {100, ""},
            {101, ""},
            {102, "Dining Grey 1"},
            {103, "Dining Grey 2"},
            {104, "Dining Grey 3"},
            {105, "Dining Grey 4"}
        };
    };

    ///<summary>
    ///The diva skill list
    ///</summary>
    public class DivaSkillList
    {

        public static Dictionary<int, string> DivaSkillID = new Dictionary<int, string>()
        {
            {0, "None"},
            {1, "High Speed Gathering"},
            {2, "Weapon Handling"},
            {3, "Focus+2"},
            {4, "Hunter Valhalla"},
            {5, "Status Immunity"},
            {6, "S. Immunity (Myriad)"},
            {7, "Starving Wolf+2"},
            {8, "Imperturbable"},
            {9, "Lone Wolf"},
            {10, "Vampirism+2"},
            {11, "Evade Distance Up"},
            {12, "Combination Expert+3"},
            {13, "Stylish"},
            {14, "Good Luck"},
            {15, "Great Luck"},
            {16, "Carving Expert"},
            {17, ""},
            {18, ""},
            {19, ""},
            {20, "Relief"},
            {21, "Recovery Items UP"},
            {22, "Hunger Negated"},
            {23, "Sharpening Artisan"},
            {24, "Recovery Speed +2"},
            {25, "Guard+2"},
            {26, "Speed Eating"},
            {27, "Encourage+2"},
            {28, "Stamina Rec Up (Lg)"},
            {29, "Razor Sharp +2"},
            {30, "Evasion+2"},
            {31, "True Guts"},
            {32, "Magnetic Res"},
            {33, "Crystal Res"},
            {34, "Honed Blade+3"}
        };
    }
}