using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The weapon list
    ///</summary>
    public static class WeaponList
    {
        public static IReadOnlyDictionary<int, string> WeaponID { get; } = new Dictionary<int, string>
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
}