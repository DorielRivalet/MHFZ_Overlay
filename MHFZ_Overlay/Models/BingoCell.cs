// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// TODO
/// </summary>
public class BingoCell
{
    /// <summary>
    /// Whether the cell is completed. Changes opacity via converter.
    /// </summary>
    public bool IsComplete { get; internal set; }

    /// <summary>
    /// The bingo monster in the cell.
    /// </summary>
    public BingoMonster? Monster { get; internal set; }

    /// <summary>
    /// TODO The number of carts in the bingo board. Used for decreasing scores in each cell.
    /// </summary>
    public int Carts { get; internal set; }

    /// <summary>
    /// The weapon type bonuses in the bingo board. Used for increasing scores in each cell and for rerolls.
    /// </summary>
    public FrontierWeaponTypes WeaponTypeBonus { get; internal set; }
}
