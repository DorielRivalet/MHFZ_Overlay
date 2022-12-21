using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The SR skills list
    ///</summary>
    public static class StyleRankSkillList
    {
        public static IReadOnlyDictionary<int, string> StyleRankSkillID { get; } = new Dictionary<int, string>
        {
            //the bonuses totals are:
            //defense+50
            //res+10.
            //all res+5
            //conquest def+200 (300 base at x11 gsr999, +10 per g rank weapon unlock)
            //conquest atk+50 (100 base at x11 gsr999, +5 per g rank weapon unlock)
            {0, "Nothing"},
            {1, ""},
            {2, ""},
            {3, ""},
            {4, ""},
            {5, ""},
            {6, ""},
            {7, ""},
            {8, ""},
            {9, ""},
            {10, "Defense+110"},//?
            {11, "Conquest Def+0"},//+200 at gsr999, +300 at x11 gsr999, goes to 330 with the 3 weapon unlocks at g rank
            {12, "Conquest Atk+0"},// see above. but with 100 + 5+ 5+5.
            {13, "Passive Master"},
            {14, "Soul Revival"},
            {15, "Secret Tech"},
            {16, "Sharpening Up"},
            {17, "Max Sharpen"},
            {18, ""},
            {19, ""},
            {20, ""},
            {21, ""},
            {22, ""},
            {23, ""},
            {24, ""},
            {25, ""},
            {26, ""},
            {27, "Def+60"},//start
            {28, ""},
            {29, ""},
            {30, ""},
            {31, "Def+100"},//after nosus. no g rank weps
            {32, "Def+110"},//after swaxe?. 1 g rank wep.
            {33, "Def+120"},//120. then 121,... (from bonus) (+10 from magnet spike). 2 g rank weps.
            {34, "Def+130"},//TODO: needs testing. is at least 140 min. 3 g rank weps. max is 180.
            {35, ""},
            {36, ""},
            {37, ""},
            {38, ""},
            {39, ""},
            {40, ""},
            {41, ""},
            {42, ""},
            {43, ""},
            {44, "Fire Res+15"},//start
            {45, ""},
            {46, ""},
            {47, ""},
            {48, "Fire Res+20"},//after hr6
            {49, "Fire Res+22"},//1 grank wep
            {50, "Fire Res+24"},//2 grank wep
            {51, "Fire Res+25"},//3rd grank wep goes by +1.
            {52, ""},
            {53, ""},
            {54, ""},
            {55, ""},
            {56, ""},
            {57, ""},
            {58, ""},
            {59, ""},
            {60, ""},
            {61, "Water Res+15"},
            {62, ""},
            {63, ""},
            {64, ""},
            {65, "Water Res+20"},//after nosus
            {66, "Water Res+22"},
            {67, "Water Res+24"},
            {68, "Water Res+25"},
            {69, ""},
            {70, ""},
            {71, ""},
            {72, ""},
            {73, ""},
            {74, ""},
            {75, ""},
            {76, ""},
            {77, ""},
            {78, "Thunder Res+15"},//start
            {79, ""},
            {80, ""},
            {81, ""},
            {82, "Thunder Res+20"},
            {83, "Thunder Res+22"},
            {84, "Thunder Res+24"},
            {85, "Thunder Res+25"},
            {86, ""},
            {87, ""},
            {88, ""},
            {89, ""},
            {90, ""},
            {91, ""},
            {92, ""},
            {93, ""},
            {94, ""},
            {95, "Ice Res+15"},
            {96, ""},
            {97, ""},
            {98, ""},
            {99, "Ice Res+20"},
            {100, "Ice Res+22"},
            {101, "Ice Res+24"},
            {102, "Ice Res+25"},
            {103, ""},
            {104, ""},
            {105, ""},
            {106, ""},
            {107, ""},
            {108, ""},
            {109, ""},
            {110, ""},
            {111, ""},
            {112, "Dragon Res+15"},
            {113, ""},
            {114, ""},
            {115, ""},
            {116, "Dragon Res+20"},
            {117, "Dragon Res+22"},
            {118, "Dragon Res+24"},
            {119, "Dragon Res+25"},
            {120, ""},
            {121, ""},
            {122, ""},
            {123, ""},
            {124, "All Res+7"},
            {125, ""},//untested
            {126, ""},//untested
            {127, "All Res+10"},//from 7 to 10. after hr6.
            {128, "All Res+12"},//1st g rank wep
            {129, "All Res+14"},//2nd g rank wep
            {130, "All Res+15"},//3rd g rank wep
            {131, ""},
            {132, ""},
            {133, ""},
            {134, ""},
            {135, ""},
            {136, ""},
            {137, ""},
            {138, ""},
            {139, ""},
            {140, ""},
            {141, ""},
            {142, ""},
            {143, ""},
            {144, ""},
            {145, ""},
            {146, ""},
            {147, ""},
            {148, ""},
            {149, ""},
            {150, ""},
            {151, ""},
            {152, ""},
            {153, ""},
            {154, ""},
            {155, "Affinity+20"},//everything in this dictionary needs testing  
            {156, "Affinity+22"},
            {157, "Affinity+24"},
            {158, "Affinity+26"}
        };
    };
}