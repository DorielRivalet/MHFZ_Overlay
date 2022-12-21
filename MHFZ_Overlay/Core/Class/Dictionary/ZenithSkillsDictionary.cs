using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The zenith skills list
    ///</summary>
    public static class ZenithSkillList
    {
        public static IReadOnlyDictionary<int, string> ZenithSkillID { get; } = new Dictionary<int, string>
        {
            {0, "None"},
            {1, "Skill Slots Up+1"},
            {2, "Skill Slots Up+2"},
            {3, "Skill Slots Up+3"},
            {4, "Skill Slots Up+4"},
            {5, "Skill Slots Up+5"},
            {6, "Skill Slots Up+6"},
            {7, "Skill Slots Up+7"},
            {8, "Flash Conversion Up+1"},
            {9, "Flash Conversion Up+2"},
            {10, "Stylish Assault Up+1"},
            {11, "Stylish Assault Up+2"},
            {12, "Dissolver Up"},
            {13, "Thunder Clad Up+1"},
            {14, "Thunder Clad Up+2"},
            {15, "Ice Age Up"},
            {16, "" },
            {17, "Hearing Protection Up+1"},
            {18, "Hearing Protection Up+2"},
            {19, "Hearing Protection Up+3"},
            {20, "Wind Res Up+1"},
            {21, "Wind Res Up+2"},
            {22, "Wind Res Up+3"},
            {23, "Wind Res Up+4"},
            {24, "Quake Res Up+1"},
            {25, "Quake Res Up+2"},
            {26, "Poison Res Up+1"},
            {27, "Poison Res Up+2"},
            {28, "Para Res Up+1"},
            {29, "Para Res Up+2"},
            {30, "Sleep Res Up+1"},
            {31, "Sleep Res Up+2"},
            {32, "Vampirism Up"},
            {33, "" },
            {34, "Drug Knowledge Up"},
            {35, "Assistance Up"},
            {36, "Bullet Saver Up+1"},
            {37, "Bullet Saver Up+2"},
            {38, "Guard Up+1"},
            {39, "Guard Up+2"},
            {40, "Adaptation Up+1"},
            {41, "Adaptation Up+2"},
            {42, "Encourage Up+1"},
            {43, "Encourage Up+2"},
            {44, "Reflect Up+1"},
            {45, "Reflect Up+2"},
            {46, "Reflect Up+3"},
            {47, "Stylish Up"},
            {48, "Vigorous Up"},
            {49, "Obscurity Up"},
            {50, "Soul Up"},
            {51, "Ceaseless Up"},
            {52, "Rush Up"},
        };
    };
}