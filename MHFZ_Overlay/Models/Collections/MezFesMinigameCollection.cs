// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;

///<summary>
///The Mezeporta Festival mini-games list
///</summary>
public static class MezFesMinigameCollection
{
    public static IReadOnlyDictionary<int, string> ID { get; } = new Dictionary<int, string>
    {
        { 463, "Volpkun Together" }, // TODO
        { 464, "Uruki Pachinko" },
        { 465, "MezFes Minigame" }, // TODO
        { 466, "Guuku Scoop" },
        { 467, "Nyanrendo" },
        { 468, "Panic Honey" },
        { 469, "Dokkan Battle Cats" },
    };
}
