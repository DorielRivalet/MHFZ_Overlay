// Copyright 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The monster color list
    ///</summary>
    public static class MonsterColorDictionary
    {
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


        public static IReadOnlyDictionary<int, string> MonsterColorID { get; } = new Dictionary<int, string>
        {
            {0, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {1, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {2, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Base"]},//
            {3, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Surface0"]},//
            {4, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {5, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mauve"]},//
            {6, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Pink"]},//
            {7, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {8, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {9, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {10, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {11, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {12, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Overlay0"]},//
            {13, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {14, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {15, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Surface0"]},//
            {16, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {17, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mantle"]},//
            {18, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {19, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {20, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mauve"]},//
            {21, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Teal"]},//
            {22, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Surface1"]},//
            {23, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mantle"]},//
            {24, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {25, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {26, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {27, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {28, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {29, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {30, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {31, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {32, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {33, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {34, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {35, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sky"]},//
            {36, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {37, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Pink"]},//
            {38, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {39, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mauve"]},//
            {40, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Lavender"]},//
            {41, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Subtext1"]},//
            {42, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Yellow"]},//
            {43, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mantle"]},//
            {44, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {45, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {46, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {47, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Crust"]},//
            {48, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Peach"]},//
            {49, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sky"]},//
            {50, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {51, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {52, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Pink"]},//
            {53, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Crust"]},//
            {54, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Subtext1"]},//
            {55, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Overlay2"]},//
            {56, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {57, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {58, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {59, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Lavender"]},//
            {60, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {61, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {62, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Pink"]},//
            {63, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Overlay1"]},//
            {64, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {65, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},// teo
            {66, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {67, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {68, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {69, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {70, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {71, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {72, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {73, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {74, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Peach"]},//
            {75, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {76, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {77, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {78, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Pink"]},//
            {79, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {80, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {81, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Peach"]},//
            {82, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {83, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Lavender"]},//
            {84, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mauve"]},//
            {85, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Yellow"]},//
            {86, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {87, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {88, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {89, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},// paria
            {90, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {91, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Base"]},//
            {92, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {93, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Peach"]},//
            {94, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {95, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mauve"]},// dora
            {96, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {97, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {98, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {99, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Peach"]},//
            {100, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Crust"]},//
            {101, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {102, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {103, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Peach"]},//
            {104, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {105, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {106, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},// odi
            {107, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Subtext1"]},//
            {108, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Teal"]},//
            {109, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sapphire"]},//
            {110, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {111, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {112, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sapphire"]},//
            {113, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Crust"]},// mi ru
            {114, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Teal"]},//
            {115, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Subtext1"]},//
            {116, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sapphire"]},//
            {117, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sapphire"]},//
            {118, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {119, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Yellow"]},//
            {120, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Subtext1"]},//
            {121, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Subtext1"]},//
            {122, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sky"]},//
            {123, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Lavender"]},//
            {124, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {125, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {126, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Yellow"]},//
            {127, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sapphire"]},//
            {128, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Yellow"]},//
            {129, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {130, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {131, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {132, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {133, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {134, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {135, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {136, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {137, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {138, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {139, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Green"]},//
            {140, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {141, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sapphire"]},//
            {142, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {143, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {144, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {145, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Lavender"]},//
            {146, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sky"]},// zin
            {147, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {148, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {149, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Crust"]},//
            {150, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Sapphire"]},//
            {151, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {152, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Yellow"]},//
            {153, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},//
            {154, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Peach"]},// guanzo
            {155, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"]},// starving
            {156, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {157, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {158, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Peach"]},//
            {159, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Base"]},//
            {160, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Subtext1"]},//
            {161, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {162, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mantle"]},//
            {163, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {164, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Subtext1"]},//
            {165, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Text"]},//
            {166, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Mauve"]},// elzelion
            {167, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Lavender"]},//
            {168, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {169, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {170, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {171, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {172, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Blue"]},//
            {173, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {174, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Red"]},//
            {175, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]},//
            {176, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Rosewater"]}//
        };
    }
}
