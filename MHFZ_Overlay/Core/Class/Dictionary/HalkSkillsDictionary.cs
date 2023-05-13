using Discord;
using DiscordRPC;
using Gma.System.MouseKeyHook;
using MaterialDesignThemes.Wpf;
using Octokit;
using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.DirectoryServices.ActiveDirectory;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows;
using Xunit.Abstractions;
using Xunit;

namespace Dictionary
{
    ///<summary>
    ///The halk skill list
    ///</summary>
    public static class HalkSkills
    {
        public static IReadOnlyDictionary<int, string> HalkSkillID { get; } = new Dictionary<int, string>
        {
            {0, "None"},
            {1, "Large Monster Priority"},
            {2, "Small Monster Priority"},
            {3, "Near Hunter Priority"},
            {4, "Always Aggressive"},
            {5, "Item Defender"},
            {6, "Rising Intent"},
            {7, "Carving Defender"},
            {8, "Unescapable"},
            {9, "Health Conscious"},
            {10, "Helper [Health]"},
            {11, "Helper [Antidote]"},
            {12, "Helper [Deoderant]"},
            {13, "Gook Luck"},
            {13, "Fast Riser"},
            {14, "Bomber"},
            {15, "Unflinching"},
            {16, "Monster Spotter"},
            {17, "Mapper"},
            {18, "Breath Ball Attack"},
            {19, "Claw Attack Focus"},
            {20, "More Pugi Appearances"},
            {21, "Flying Wyvern Offense"},
            {22, "Fanged Beast Offense"},
            {23, "Piscine Offense"},
            {24, "Carapaceon Offense"},
            {25, "Bird Wyvern Offense"},
            {26, "Elder Dragon Offense"},
            {27, "Elemental Beam Attack"},
            {28, "Demon Drug Dust"},
            {29, "Armour Drug Dust"},
            {30, "Affinity Added"},
            {31, "Improve Status Attack"},
            {32, "Improve Evasion"},
            {33, "Improve Attack"},
            {34, "Signal to Halt Attacks"},
            {35, "Improve Fire Element"},
            {36, "Improve Ice Element"},
            {37, "Improve Water Element"},
            {38, "Improve Thunder Element"},
            {39, "Improve Dragon Element"},
            {40, "Small Bomber"},
            {41, "Supporter"},
            {42, "Elemental Bite"},
            {43, "Brute Wyvern Offense"},
            {44, "Breath Ball Support"},
            {45, "Attack Focus"},
            {46, "Defence Focus"},
            {47, "Jungle Attack"},
            {48, "Desert Attack"},
            {49, "Swamp Attack"},
            {50, "Snowy Mountains Attack"},
            {51, "Volcano Attack"},
            {52, "Tower Attack"},
            {53, "Forest and Hills Attack"},
            {54, "Great Forest Attack"},
            {55, "Arena Attack"},
            {56, "Gorge Attack"},
            {57, "Highlands Attack"},
            {58, "Tidal Island Attack"},
            {59, "Wind Pressure"},
            {60, "Earplugs"},
            {61, "Lone Attack"},
            {62, "Party Attack"},
            {63, "Gathering Affection"},
            {64, "Danger Affection"},
            {65, "Secret Area Affection"},
            {66, "Leviathan Offense"},
            {67, "Rapid Breath Attack"},
            {68, "Empower Attack and Def"},
            {69, "Charging Attack"},
            {70, "Smash Attack"},
            {71, "Polar Sea Attack"},
            {72, "Summer Assault"},
            {73, "Winter Assault"},
            {74, "Contain Monster"},
            {75, "Swift Battle"},
            {76, "Long Battle"},
            {77, "Fanged Wyvern Offense"},
            {78, "Flower Field Attack"},
            {79, "White Lake Attack"},
            {80, "Painted Falls Attack"},
        };
    };
}