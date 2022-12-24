using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The weapon class list
    ///</summary>
    public static class WeaponClass
    {
        public static IReadOnlyDictionary<int, string> WeaponClassID { get; } = new Dictionary<int, string>
        {
            {0, "Blademaster"},
            {1, "Gunner"},
            {2, "Blademaster"},
            {3, "Blademaster"},
            {4, "Blademaster"},
            {5, "Gunner"},
            {6, "Blademaster"},
            {7, "Blademaster"},
            {8, "Blademaster"},
            {9, "Blademaster"},
            {10, "Gunner"},
            {11, "Blademaster"},
            {12, "Blademaster"},
            {13, "Blademaster"}
        };
    };
}