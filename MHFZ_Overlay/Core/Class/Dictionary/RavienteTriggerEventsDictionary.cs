﻿// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System.Collections.Generic;

namespace Dictionary
{
    /// <summary>
    /// The Raviente Trigger Events list
    /// </summary>
    public static class RavienteTriggerEvents
    {
        public static IReadOnlyDictionary<int, string> RavienteTriggerEventIDs { get; } = new Dictionary<int, string>
        {
            {0, "Slay 1"},
            {1, "Sedation 1"},
            {2, "Destruction 1"},
            {3, "Slay 2"},
            {4, "Sedation 2"},
            {5, "Sedation 3"},
            {6, "Slay 4"},
            {7, "Slay 5"},
            {8, "Sedation 5"},
            {9, "Slay 6"},
            {10, "Slay 7"},
            {11, "Sedation 7"},
            {12, "Sedation 8"},
            {13, "Slay 9"}
        };
    };
}