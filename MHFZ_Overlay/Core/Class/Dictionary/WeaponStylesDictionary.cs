using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The weapon styles list
    ///</summary>
    public static class WeaponStyles
    {
        public static IReadOnlyDictionary<int, string> WeaponStyleID { get; } = new Dictionary<int, string>
        {
            {0, "Earth Style"},
            {1, "Heaven Style"},
            {2, "Storm Style"},
            {3, "Extreme Style"}
        };
    };
}