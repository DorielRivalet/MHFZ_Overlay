// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

///<summary>
///The monster color list
///</summary>
public static class MonsterColorCollection
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
        { 0, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 1, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 2, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Base"] },
        { 3, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Surface0"] },
        { 4, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 5, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mauve"] },
        { 6, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Pink"] },
        { 7, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 8, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 9, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 10, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 11, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 12, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Overlay0"] },
        { 13, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 14, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 15, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Surface0"] },
        { 16, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 17, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mantle"] },
        { 18, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 19, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 20, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mauve"] },
        { 21, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Teal"] },
        { 22, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Surface1"] },
        { 23, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mantle"] },
        { 24, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 25, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 26, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 27, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 28, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 29, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 30, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 31, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 32, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 33, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 34, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 35, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sky"] },
        { 36, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 37, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Pink"] },
        { 38, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 39, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mauve"] },
        { 40, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Lavender"] },
        { 41, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Subtext1"] },
        { 42, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Yellow"] },
        { 43, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mantle"] },
        { 44, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 45, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 46, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 47, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Crust"] },
        { 48, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Peach"] },
        { 49, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sky"] },
        { 50, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 51, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 52, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Pink"] },
        { 53, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Crust"] },
        { 54, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Subtext1"] },
        { 55, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Overlay2"] },
        { 56, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 57, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 58, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 59, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Lavender"] },
        { 60, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 61, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 62, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Pink"] },
        { 63, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Overlay1"] },
        { 64, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 65, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] }, // teo
        { 66, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 67, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 68, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 69, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 70, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 71, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 72, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 73, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 74, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Peach"] },
        { 75, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 76, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 77, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 78, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Pink"] },
        { 79, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 80, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 81, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Peach"] },
        { 82, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 83, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Lavender"] },
        { 84, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mauve"] },
        { 85, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Yellow"] },
        { 86, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 87, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 88, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 89, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] }, // paria
        { 90, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 91, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Base"] },
        { 92, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 93, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Peach"] },
        { 94, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 95, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mauve"] }, // dora
        { 96, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 97, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 98, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 99, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Peach"] },
        { 100, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Crust"] },
        { 101, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 102, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 103, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Peach"] },
        { 104, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 105, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 106, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] }, // odi
        { 107, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Subtext1"] },
        { 108, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Teal"] },
        { 109, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sapphire"] },
        { 110, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 111, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 112, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sapphire"] },
        { 113, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Crust"] }, // mi ru
        { 114, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Teal"] },
        { 115, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Subtext1"] },
        { 116, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sapphire"] },
        { 117, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sapphire"] },
        { 118, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 119, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Yellow"] },
        { 120, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Subtext1"] },
        { 121, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Subtext1"] },
        { 122, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sky"] },
        { 123, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Lavender"] },
        { 124, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 125, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 126, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Yellow"] },
        { 127, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sapphire"] },
        { 128, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Yellow"] },
        { 129, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 130, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 131, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 132, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 133, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 134, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 135, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 136, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 137, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 138, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 139, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Green"] },
        { 140, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 141, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sapphire"] },
        { 142, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 143, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 144, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 145, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Lavender"] },
        { 146, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sky"] }, // zin
        { 147, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 148, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 149, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Crust"] },
        { 150, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Sapphire"] },
        { 151, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 152, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Yellow"] },
        { 153, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] },
        { 154, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Peach"] }, // guanzo
        { 155, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Maroon"] }, // starving
        { 156, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 157, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 158, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Peach"] },
        { 159, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Base"] },
        { 160, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Subtext1"] },
        { 161, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 162, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mantle"] },
        { 163, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 164, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Subtext1"] },
        { 165, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Text"] },
        { 166, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Mauve"] }, // elzelion
        { 167, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Lavender"] },
        { 168, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 169, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 170, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 171, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 172, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Blue"] },
        { 173, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 174, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Red"] },
        { 175, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
        { 176, CatppuccinMochaColorsCollection.CatppuccinMochaColors["Rosewater"] },
    });
}
