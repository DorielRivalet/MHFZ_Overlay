﻿using System.Collections.Generic;

namespace Dictionary
{
    public static class WeaponCanUseReflect
    {
        public static IReadOnlyDictionary<int, bool> WeaponTypeID { get; } = new Dictionary<int, bool>
        {
            {0, true},
            {1, false},
            {2, false},
            {3, true},
            {4, true},
            {5, false},
            {6, false},
            {7, true},
            {8, false},
            {9, true},
            {10, false},
            {11, true},
            {12, true},
            {13, true}
        };
    };
}