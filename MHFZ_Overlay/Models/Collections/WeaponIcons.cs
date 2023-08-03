// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// The weapon icons list.
/// </summary>
public static class WeaponIcons
{
    public static ReadOnlyDictionary<int, string> WeaponIconID { get; } = new (new Dictionary<int, string>
    {
        { 0, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/gs.png" },
        { 1, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/hbg.png" },
        { 2, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/hammer.png" },
        { 3, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/lance.png" },
        { 4, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/sns.png" },
        { 5, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/lbg.png" },
        { 6, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/ds.png" },
        { 7, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/ls.png" },
        { 8, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/hh.png" },
        { 9, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/gl.png" },
        { 10, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/bow.png" },
        { 11, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/tonfa.png" },
        { 12, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/saf.png" },
        { 13, "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/ms.png" },
    });
}
