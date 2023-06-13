﻿// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System.Collections.Generic;

namespace MHFZ_Overlay.Core.Class.Dictionary;

///<summary>
///The color list
///</summary>
///	///	Labels	Hex	RGB	HSL
//Rosewater	#f5e0dc	rgb(245, 224, 220)	hsl(10, 56%, 91%)
//Flamingo	#f2cdcd	rgb(242, 205, 205)	hsl(0, 59%, 88%)
//Pink	#f5c2e7	rgb(245, 194, 231)	hsl(316, 72%, 86%)
//Mauve	#cba6f7	rgb(203, 166, 247)	hsl(267, 84%, 81%)
//Red	#f38ba8	rgb(243, 139, 168)	hsl(343, 81%, 75%)
//Maroon	#eba0ac	rgb(235, 160, 172)	hsl(350, 65%, 77%)
//Peach	#fab387	rgb(250, 179, 135)	hsl(23, 92%, 75%)
//Yellow	#f9e2af	rgb(249, 226, 175)	hsl(41, 86%, 83%)
//Green	#a6e3a1	rgb(166, 227, 161)	hsl(115, 54%, 76%)
//Teal	#94e2d5	rgb(148, 226, 213)	hsl(170, 57%, 73%)
//Sky	#89dceb	rgb(137, 220, 235)	hsl(189, 71%, 73%)
//Sapphire	#74c7ec	rgb(116, 199, 236)	hsl(199, 76%, 69%)
//Blue	#89b4fa	rgb(137, 180, 250)	hsl(217, 92%, 76%)
//Lavender	#b4befe	rgb(180, 190, 254)	hsl(232, 97%, 85%)
//Text	#cdd6f4	rgb(205, 214, 244)	hsl(226, 64%, 88%)
//Subtext1	#bac2de	rgb(186, 194, 222)	hsl(227, 35%, 80%)
//Subtext0	#a6adc8	rgb(166, 173, 200)	hsl(228, 24%, 72%)
//Overlay2	#9399b2	rgb(147, 153, 178)	hsl(228, 17%, 64%)
//Overlay1	#7f849c	rgb(127, 132, 156)	hsl(230, 13%, 55%)
//Overlay0	#6c7086	rgb(108, 112, 134)	hsl(231, 11%, 47%)
//Surface2	#585b70	rgb(88, 91, 112)	hsl(233, 12%, 39%)
//Surface1	#45475a	rgb(69, 71, 90)	hsl(234, 13%, 31%)
//Surface0	#313244	rgb(49, 50, 68)	hsl(237, 16%, 23%)
//Base	#1e1e2e	rgb(30, 30, 46)	hsl(240, 21%, 15%)
//Mantle	#181825	rgb(24, 24, 37)	hsl(240, 21%, 12%)
//Crust	#11111b	rgb(17, 17, 27)	hsl(240, 23%, 9%)
public static class CatppuccinMochaColorsDictionary
{
    public static IReadOnlyDictionary<string, string> CatppuccinMochaColors { get; } = new Dictionary<string, string>
    {
        {"Rosewater","#f5e0dc" },
        { "Flamingo","#f2cdcd"},
        { "Pink","#f5c2e7"},
        { "Mauve","#cba6f7"},
        { "Red","#f38ba8"},
        { "Maroon","#eba0ac"},
        { "Peach","#fab387"},
        { "Yellow","#f9e2af"},
        { "Green","#a6e3a1"},
        { "Teal","#94e2d5"},
        { "Sky","#89dceb"},
        { "Sapphire","#74c7ec"},
        { "Blue","#89b4fa"},
        { "Lavender","#b4befe"},
        { "Text","#cdd6f4"},
        { "Subtext1","#bac2de"},
        { "Subtext0","#a6adc8"},
        { "Overlay2","#9399b2"},
        { "Overlay1","#7f849c"},
        { "Overlay0","#6c7086"},
        { "Surface2","#585b70"},
        { "Surface1","#45475a"},
        { "Surface0","#313244"},
        { "Base","#1e1e2e"},
        { "Mantle","#181825"},
        { "Crust","#11111b"}
    };
}