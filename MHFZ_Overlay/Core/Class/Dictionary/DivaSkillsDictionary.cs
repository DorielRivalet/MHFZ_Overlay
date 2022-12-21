using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The diva skill list
    ///</summary>
    public static class DivaSkillList
    {
        public static IReadOnlyDictionary<int, string> DivaSkillID { get; } = new Dictionary<int, string>
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
    };
}