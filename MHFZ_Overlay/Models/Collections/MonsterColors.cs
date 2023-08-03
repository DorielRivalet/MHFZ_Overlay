// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// The monster color list.
/// </summary>
public static class MonsterColors
{
    // Labels  Hex  RGB  HSL
    // Rosewater  #f5e0dc  rgb(245, 224, 220)  hsl(10, 56%, 91%)
    // Flamingo  #f2cdcd  rgb(242, 205, 205)  hsl(0, 59%, 88%)
    // Pink  #f5c2e7  rgb(245, 194, 231)  hsl(316, 72%, 86%)
    // Mauve  #cba6f7  rgb(203, 166, 247)  hsl(267, 84%, 81%)
    // Red  #f38ba8  rgb(243, 139, 168)  hsl(343, 81%, 75%)
    // Maroon  #eba0ac  rgb(235, 160, 172)  hsl(350, 65%, 77%)
    // Peach  #fab387  rgb(250, 179, 135)  hsl(23, 92%, 75%)
    // Yellow  #f9e2af  rgb(249, 226, 175)  hsl(41, 86%, 83%)
    // Green  #a6e3a1  rgb(166, 227, 161)  hsl(115, 54%, 76%)
    // Teal  #94e2d5  rgb(148, 226, 213)  hsl(170, 57%, 73%)
    // Sky  #89dceb  rgb(137, 220, 235)  hsl(189, 71%, 73%)
    // Sapphire  #74c7ec  rgb(116, 199, 236)  hsl(199, 76%, 69%)
    // Blue  #89b4fa  rgb(137, 180, 250)  hsl(217, 92%, 76%)
    // Lavender  #b4befe  rgb(180, 190, 254)  hsl(232, 97%, 85%)
    // Text  #cdd6f4  rgb(205, 214, 244)  hsl(226, 64%, 88%)
    // Subtext1  #bac2de  rgb(186, 194, 222)  hsl(227, 35%, 80%)
    // Subtext0  #a6adc8  rgb(166, 173, 200)  hsl(228, 24%, 72%)
    // Overlay2  #9399b2  rgb(147, 153, 178)  hsl(228, 17%, 64%)
    // Overlay1  #7f849c  rgb(127, 132, 156)  hsl(230, 13%, 55%)
    // Overlay0  #6c7086  rgb(108, 112, 134)  hsl(231, 11%, 47%)
    // Surface2  #585b70  rgb(88, 91, 112)  hsl(233, 12%, 39%)
    // Surface1  #45475a  rgb(69, 71, 90)  hsl(234, 13%, 31%)
    // Surface0  #313244  rgb(49, 50, 68)  hsl(237, 16%, 23%)
    // Base  #1e1e2e  rgb(30, 30, 46)  hsl(240, 21%, 15%)
    // Mantle  #181825  rgb(24, 24, 37)  hsl(240, 21%, 12%)
    // Crust  #11111b  rgb(17, 17, 27)  hsl(240, 23%, 9%)
    public static ReadOnlyDictionary<int, string> MonsterColorID { get; } = new (new Dictionary<int, string>
    {
        { 0, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 1, CatppuccinMochaColors.NameHex["Green"] },
        { 2, CatppuccinMochaColors.NameHex["Base"] },
        { 3, CatppuccinMochaColors.NameHex["Surface0"] },
        { 4, CatppuccinMochaColors.NameHex["Green"] },
        { 5, CatppuccinMochaColors.NameHex["Mauve"] },
        { 6, CatppuccinMochaColors.NameHex["Pink"] },
        { 7, CatppuccinMochaColors.NameHex["Maroon"] },
        { 8, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 9, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 10, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 11, CatppuccinMochaColors.NameHex["Red"] },
        { 12, CatppuccinMochaColors.NameHex["Overlay0"] },
        { 13, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 14, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 15, CatppuccinMochaColors.NameHex["Surface0"] },
        { 16, CatppuccinMochaColors.NameHex["Blue"] },
        { 17, CatppuccinMochaColors.NameHex["Mantle"] },
        { 18, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 19, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 20, CatppuccinMochaColors.NameHex["Mauve"] },
        { 21, CatppuccinMochaColors.NameHex["Teal"] },
        { 22, CatppuccinMochaColors.NameHex["Surface1"] },
        { 23, CatppuccinMochaColors.NameHex["Mantle"] },
        { 24, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 25, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 26, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 27, CatppuccinMochaColors.NameHex["Blue"] },
        { 28, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 29, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 30, CatppuccinMochaColors.NameHex["Red"] },
        { 31, CatppuccinMochaColors.NameHex["Red"] },
        { 32, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 33, CatppuccinMochaColors.NameHex["Text"] },
        { 34, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 35, CatppuccinMochaColors.NameHex["Sky"] },
        { 36, CatppuccinMochaColors.NameHex["Maroon"] },
        { 37, CatppuccinMochaColors.NameHex["Pink"] },
        { 38, CatppuccinMochaColors.NameHex["Blue"] },
        { 39, CatppuccinMochaColors.NameHex["Mauve"] },
        { 40, CatppuccinMochaColors.NameHex["Lavender"] },
        { 41, CatppuccinMochaColors.NameHex["Subtext1"] },
        { 42, CatppuccinMochaColors.NameHex["Yellow"] },
        { 43, CatppuccinMochaColors.NameHex["Mantle"] },
        { 44, CatppuccinMochaColors.NameHex["Text"] },
        { 45, CatppuccinMochaColors.NameHex["Red"] },
        { 46, CatppuccinMochaColors.NameHex["Green"] },
        { 47, CatppuccinMochaColors.NameHex["Crust"] },
        { 48, CatppuccinMochaColors.NameHex["Peach"] },
        { 49, CatppuccinMochaColors.NameHex["Sky"] },
        { 50, CatppuccinMochaColors.NameHex["Blue"] },
        { 51, CatppuccinMochaColors.NameHex["Text"] },
        { 52, CatppuccinMochaColors.NameHex["Pink"] },
        { 53, CatppuccinMochaColors.NameHex["Crust"] },
        { 54, CatppuccinMochaColors.NameHex["Subtext1"] },
        { 55, CatppuccinMochaColors.NameHex["Overlay2"] },
        { 56, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 57, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 58, CatppuccinMochaColors.NameHex["Green"] },
        { 59, CatppuccinMochaColors.NameHex["Lavender"] },
        { 60, CatppuccinMochaColors.NameHex["Maroon"] },
        { 61, CatppuccinMochaColors.NameHex["Text"] },
        { 62, CatppuccinMochaColors.NameHex["Pink"] },
        { 63, CatppuccinMochaColors.NameHex["Overlay1"] },
        { 64, CatppuccinMochaColors.NameHex["Blue"] },
        { 65, CatppuccinMochaColors.NameHex["Red"] }, // teo
        { 66, CatppuccinMochaColors.NameHex["Red"] },
        { 67, CatppuccinMochaColors.NameHex["Blue"] },
        { 68, CatppuccinMochaColors.NameHex["Maroon"] },
        { 69, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 70, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 71, CatppuccinMochaColors.NameHex["Text"] },
        { 72, CatppuccinMochaColors.NameHex["Green"] },
        { 73, CatppuccinMochaColors.NameHex["Blue"] },
        { 74, CatppuccinMochaColors.NameHex["Peach"] },
        { 75, CatppuccinMochaColors.NameHex["Maroon"] },
        { 76, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 77, CatppuccinMochaColors.NameHex["Maroon"] },
        { 78, CatppuccinMochaColors.NameHex["Pink"] },
        { 79, CatppuccinMochaColors.NameHex["Red"] },
        { 80, CatppuccinMochaColors.NameHex["Green"] },
        { 81, CatppuccinMochaColors.NameHex["Peach"] },
        { 82, CatppuccinMochaColors.NameHex["Text"] },
        { 83, CatppuccinMochaColors.NameHex["Lavender"] },
        { 84, CatppuccinMochaColors.NameHex["Mauve"] },
        { 85, CatppuccinMochaColors.NameHex["Yellow"] },
        { 86, CatppuccinMochaColors.NameHex["Green"] },
        { 87, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 88, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 89, CatppuccinMochaColors.NameHex["Maroon"] }, // paria
        { 90, CatppuccinMochaColors.NameHex["Text"] },
        { 91, CatppuccinMochaColors.NameHex["Base"] },
        { 92, CatppuccinMochaColors.NameHex["Text"] },
        { 93, CatppuccinMochaColors.NameHex["Peach"] },
        { 94, CatppuccinMochaColors.NameHex["Green"] },
        { 95, CatppuccinMochaColors.NameHex["Mauve"] }, // dora
        { 96, CatppuccinMochaColors.NameHex["Blue"] },
        { 97, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 98, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 99, CatppuccinMochaColors.NameHex["Peach"] },
        { 100, CatppuccinMochaColors.NameHex["Crust"] },
        { 101, CatppuccinMochaColors.NameHex["Red"] },
        { 102, CatppuccinMochaColors.NameHex["Red"] },
        { 103, CatppuccinMochaColors.NameHex["Peach"] },
        { 104, CatppuccinMochaColors.NameHex["Green"] },
        { 105, CatppuccinMochaColors.NameHex["Blue"] },
        { 106, CatppuccinMochaColors.NameHex["Red"] }, // odi
        { 107, CatppuccinMochaColors.NameHex["Subtext1"] },
        { 108, CatppuccinMochaColors.NameHex["Teal"] },
        { 109, CatppuccinMochaColors.NameHex["Sapphire"] },
        { 110, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 111, CatppuccinMochaColors.NameHex["Maroon"] },
        { 112, CatppuccinMochaColors.NameHex["Sapphire"] },
        { 113, CatppuccinMochaColors.NameHex["Crust"] }, // mi ru
        { 114, CatppuccinMochaColors.NameHex["Teal"] },
        { 115, CatppuccinMochaColors.NameHex["Subtext1"] },
        { 116, CatppuccinMochaColors.NameHex["Sapphire"] },
        { 117, CatppuccinMochaColors.NameHex["Sapphire"] },
        { 118, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 119, CatppuccinMochaColors.NameHex["Yellow"] },
        { 120, CatppuccinMochaColors.NameHex["Subtext1"] },
        { 121, CatppuccinMochaColors.NameHex["Subtext1"] },
        { 122, CatppuccinMochaColors.NameHex["Sky"] },
        { 123, CatppuccinMochaColors.NameHex["Lavender"] },
        { 124, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 125, CatppuccinMochaColors.NameHex["Green"] },
        { 126, CatppuccinMochaColors.NameHex["Yellow"] },
        { 127, CatppuccinMochaColors.NameHex["Sapphire"] },
        { 128, CatppuccinMochaColors.NameHex["Yellow"] },
        { 129, CatppuccinMochaColors.NameHex["Green"] },
        { 130, CatppuccinMochaColors.NameHex["Red"] },
        { 131, CatppuccinMochaColors.NameHex["Blue"] },
        { 132, CatppuccinMochaColors.NameHex["Blue"] },
        { 133, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 134, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 135, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 136, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 137, CatppuccinMochaColors.NameHex["Green"] },
        { 138, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 139, CatppuccinMochaColors.NameHex["Green"] },
        { 140, CatppuccinMochaColors.NameHex["Text"] },
        { 141, CatppuccinMochaColors.NameHex["Sapphire"] },
        { 142, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 143, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 144, CatppuccinMochaColors.NameHex["Maroon"] },
        { 145, CatppuccinMochaColors.NameHex["Lavender"] },
        { 146, CatppuccinMochaColors.NameHex["Sky"] }, // zin
        { 147, CatppuccinMochaColors.NameHex["Maroon"] },
        { 148, CatppuccinMochaColors.NameHex["Blue"] },
        { 149, CatppuccinMochaColors.NameHex["Crust"] },
        { 150, CatppuccinMochaColors.NameHex["Sapphire"] },
        { 151, CatppuccinMochaColors.NameHex["Text"] },
        { 152, CatppuccinMochaColors.NameHex["Yellow"] },
        { 153, CatppuccinMochaColors.NameHex["Maroon"] },
        { 154, CatppuccinMochaColors.NameHex["Peach"] }, // guanzo
        { 155, CatppuccinMochaColors.NameHex["Maroon"] }, // starving
        { 156, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 157, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 158, CatppuccinMochaColors.NameHex["Peach"] },
        { 159, CatppuccinMochaColors.NameHex["Base"] },
        { 160, CatppuccinMochaColors.NameHex["Subtext1"] },
        { 161, CatppuccinMochaColors.NameHex["Blue"] },
        { 162, CatppuccinMochaColors.NameHex["Mantle"] },
        { 163, CatppuccinMochaColors.NameHex["Text"] },
        { 164, CatppuccinMochaColors.NameHex["Subtext1"] },
        { 165, CatppuccinMochaColors.NameHex["Text"] },
        { 166, CatppuccinMochaColors.NameHex["Mauve"] }, // elzelion
        { 167, CatppuccinMochaColors.NameHex["Lavender"] },
        { 168, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 169, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 170, CatppuccinMochaColors.NameHex["Blue"] },
        { 171, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 172, CatppuccinMochaColors.NameHex["Blue"] },
        { 173, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 174, CatppuccinMochaColors.NameHex["Red"] },
        { 175, CatppuccinMochaColors.NameHex["Rosewater"] },
        { 176, CatppuccinMochaColors.NameHex["Rosewater"] },
    });
}
