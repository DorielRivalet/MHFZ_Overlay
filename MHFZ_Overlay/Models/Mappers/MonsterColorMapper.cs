// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Mappers;

using System.Collections.Generic;

///<summary>
///The monster color list
///</summary>
public static class MonsterColorMapper
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

    public static IReadOnlyDictionary<int, string> MonsterColorID { get; } = new Dictionary<int, string>
    {
        { 0, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 1, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 2, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Base"] },
        { 3, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Surface0"] },
        { 4, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 5, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mauve"] },
        { 6, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Pink"] },
        { 7, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 8, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 9, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 10, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 11, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 12, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Overlay0"] },
        { 13, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 14, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 15, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Surface0"] },
        { 16, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 17, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mantle"] },
        { 18, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 19, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 20, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mauve"] },
        { 21, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Teal"] },
        { 22, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Surface1"] },
        { 23, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mantle"] },
        { 24, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 25, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 26, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 27, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 28, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 29, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 30, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 31, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 32, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 33, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 34, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 35, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sky"] },
        { 36, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 37, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Pink"] },
        { 38, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 39, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mauve"] },
        { 40, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Lavender"] },
        { 41, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Subtext1"] },
        { 42, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Yellow"] },
        { 43, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mantle"] },
        { 44, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 45, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 46, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 47, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Crust"] },
        { 48, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Peach"] },
        { 49, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sky"] },
        { 50, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 51, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 52, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Pink"] },
        { 53, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Crust"] },
        { 54, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Subtext1"] },
        { 55, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Overlay2"] },
        { 56, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 57, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 58, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 59, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Lavender"] },
        { 60, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 61, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 62, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Pink"] },
        { 63, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Overlay1"] },
        { 64, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 65, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] }, // teo
        { 66, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 67, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 68, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 69, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 70, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 71, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 72, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 73, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 74, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Peach"] },
        { 75, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 76, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 77, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 78, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Pink"] },
        { 79, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 80, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 81, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Peach"] },
        { 82, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 83, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Lavender"] },
        { 84, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mauve"] },
        { 85, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Yellow"] },
        { 86, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 87, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 88, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 89, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] }, // paria
        { 90, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 91, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Base"] },
        { 92, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 93, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Peach"] },
        { 94, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 95, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mauve"] }, // dora
        { 96, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 97, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 98, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 99, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Peach"] },
        { 100, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Crust"] },
        { 101, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 102, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 103, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Peach"] },
        { 104, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 105, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 106, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] }, // odi
        { 107, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Subtext1"] },
        { 108, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Teal"] },
        { 109, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sapphire"] },
        { 110, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 111, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 112, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sapphire"] },
        { 113, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Crust"] }, // mi ru
        { 114, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Teal"] },
        { 115, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Subtext1"] },
        { 116, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sapphire"] },
        { 117, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sapphire"] },
        { 118, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 119, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Yellow"] },
        { 120, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Subtext1"] },
        { 121, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Subtext1"] },
        { 122, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sky"] },
        { 123, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Lavender"] },
        { 124, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 125, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 126, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Yellow"] },
        { 127, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sapphire"] },
        { 128, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Yellow"] },
        { 129, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 130, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 131, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 132, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 133, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 134, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 135, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 136, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 137, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 138, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 139, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Green"] },
        { 140, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 141, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sapphire"] },
        { 142, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 143, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 144, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 145, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Lavender"] },
        { 146, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sky"] }, // zin
        { 147, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 148, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 149, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Crust"] },
        { 150, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Sapphire"] },
        { 151, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 152, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Yellow"] },
        { 153, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },
        { 154, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Peach"] }, // guanzo
        { 155, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] }, // starving
        { 156, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 157, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 158, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Peach"] },
        { 159, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Base"] },
        { 160, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Subtext1"] },
        { 161, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 162, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mantle"] },
        { 163, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 164, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Subtext1"] },
        { 165, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Text"] },
        { 166, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Mauve"] }, // elzelion
        { 167, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Lavender"] },
        { 168, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 169, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 170, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 171, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 172, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Blue"] },
        { 173, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 174, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Red"] },
        { 175, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
        { 176, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Rosewater"] },
    };
}
