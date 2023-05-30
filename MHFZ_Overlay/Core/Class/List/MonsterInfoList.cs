// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MHFZ_Overlay;
using System.Collections.Generic;
using MHFZ_Overlay.Core.Class;

namespace Dictionary
{
    ///<summary>
    ///The monster info list
    ///<br>TODO: missing labels</br>
    ///</summary>
    public static class MonsterInfoList
    {
        private static string RickRoll = "";

        public static IReadOnlyList<MonsterInfo> MonsterInfoIDs { get; } = new List<MonsterInfo>
        {
            new MonsterInfo("[Musou] Arrogant Duremudira",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dolem_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","https://www.youtube.com/embed/jqaUPZ8DVfw" },
                    {"Dual Swords","https://www.youtube.com/embed/qa6LfcOr_S0" },
                    {"Great Sword","https://www.youtube.com/embed/MQit-3HZLhM" },
                    {"Long Sword","https://www.youtube.com/embed/8letGMGpjbU" },
                    {"Hammer","https://www.youtube.com/embed/Z2OwUAWROio" },
                    {"Hunting Horn","" },
                    {"Lance","https://www.youtube.com/embed/laa4x-V_qrQ" },
                    {"Gunlance","https://www.youtube.com/embed/68WK1F69fMo" },
                    {"Tonfa","https://www.youtube.com/embed/ry1faWMTdtQ" },
                    {"Switch Axe F","https://www.youtube.com/embed/HV8qzOGYEoM" },
                    {"Magnet Spike","https://www.youtube.com/embed/0Av3vuNs1pA" },
                    {"Light Bowgun","https://www.youtube.com/embed/aWsO7pdp8OU" },
                    {"Heavy Bowgun","" },
                    {"Bow","https://www.youtube.com/embed/9UVFdkZT6SQ" },
                },
                "https://monsterhunter.fandom.com/wiki/Arrogant_Duremudira"
                ),

            new MonsterInfo("[Musou] Blinking Nargacuga",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/nalgaK_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","https://www.youtube.com/embed/baWvEuLLwxk" },
                    {"Dual Swords","https://www.youtube.com/embed/dlBMmEwCO6k" },
                    {"Great Sword","https://www.youtube.com/embed/MA46kDZpDEs" },
                    {"Long Sword","https://www.youtube.com/embed/qHCVjd0Ov7E" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","https://www.youtube.com/embed/TwWPTcuwK4o" },
                    {"Gunlance","https://www.youtube.com/embed/IQRRyzkUSew" },
                    {"Tonfa","https://www.youtube.com/embed/Q1bnA9LnWlU" },
                    {"Switch Axe F","https://www.youtube.com/embed/n6SdO7Cpugg" },
                    {"Magnet Spike","https://www.youtube.com/embed/n6SdO7Cpugg" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","https://www.youtube.com/embed/pXPh2uqvFD8" },
                },
                "https://monsterhunter.fandom.com/wiki/Blinking_Nargacuga"
                ),

            new MonsterInfo("[Musou] Blitzkrieg Bogabadorumu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/bogaK_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","https://youtube.com/embed/ak1o6N06V6U" },
                    {"Dual Swords","https://youtube.com/embed/XAIvnGBVIp8" },
                    {"Great Sword","https://youtube.com/embed/huULgUrupPM" },
                    {"Long Sword","https://youtube.com/embed/X49Z85BCVIk" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","https://youtube.com/embed/kL1Zkhoa9eg" },
                    {"Gunlance","https://youtube.com/embed/Lc3fS_010Ws" },
                    {"Tonfa","https://youtube.com/embed/SyVWewqI1nw" },
                    {"Switch Axe F","https://youtube.com/embed/LXaYqpRonrk" },
                    {"Magnet Spike","https://www.youtube.com/embed/60UlLymak2k" },
                    {"Light Bowgun","https://www.youtube.com/embed/_D5UqIjNm4Q" },
                    {"Heavy Bowgun","" },
                    {"Bow","https://www.youtube.com/embed/3aHSjPo90Vc" },
                },
                "https://monsterhunter.fandom.com/wiki/Bombardier_Bogabadorumu"
                ),

            new MonsterInfo("[Musou] Burning Freezing Elzelion",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/elze_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","https://www.youtube.com/embed/RAdft8GHKvU" },
                    {"Dual Swords","https://www.youtube.com/embed/X6F2uywrZiE" },
                    {"Great Sword","https://www.youtube.com/embed/lN-4yfgd9y0" },
                    {"Long Sword","https://www.youtube.com/embed/5EC60bXCUZ8" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","https://www.youtube.com/embed/vXkz-JgDWJQ" },
                    {"Gunlance","https://www.youtube.com/embed/KxNao8p8eYw" },
                    {"Tonfa","https://www.youtube.com/embed/4TmtciKAJyg" },
                    {"Switch Axe F","https://www.youtube.com/embed/H5e7TYB1B9A" },
                    {"Magnet Spike","https://www.youtube.com/embed/n27VlF1r894" },
                    {"Light Bowgun","https://www.youtube.com/embed/lWZaWexqzxE" },
                    {"Heavy Bowgun","https://www.youtube.com/embed/yGLLnEDVZoY" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Burning_Freezing_Eruzerion"
                ),


                new MonsterInfo("[Musou] Howling Zinogre",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zin_ng.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","https://www.youtube.com/embed/wc8lCUg0pko" },
                    {"Dual Swords","https://www.youtube.com/embed/QOK1Kg5fzHk" },
                    {"Great Sword","https://www.youtube.com/embed/sjBwjTN1VVg" },
                    {"Long Sword","https://www.youtube.com/embed/C5eJ4wk9fYk" },
                    {"Hammer","https://www.youtube.com/embed/R1rpbYzrCnQ" },
                    {"Hunting Horn","https://www.youtube.com/embed/8GRK_r4al_Y" },
                    {"Lance","https://www.youtube.com/embed/fii-JAZACQM" },
                    {"Gunlance","https://www.youtube.com/embed/hawNX97Xp28" },
                    {"Tonfa","https://www.youtube.com/embed/I8BbYtfJZdI" },
                    {"Switch Axe F","https://www.youtube.com/embed/Dg9xQkCzqmo" },
                    {"Magnet Spike","https://www.youtube.com/embed/bUpRnWAlpUw" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","https://www.youtube.com/embed/GqMh_KUJx4E" },
                },
                "https://monsterhunter.fandom.com/wiki/Howling_Zinogre"
                ),

            new MonsterInfo("[Musou] Ruling Guanzorumu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/guan_ng.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","https://www.youtube.com/embed/UzLiZrO9CDE" },
                    {"Dual Swords","https://www.youtube.com/embed/vMfgFOYoO3Q" },
                    {"Great Sword","https://www.youtube.com/embed/3OUgl9HnTUQ" },
                    {"Long Sword","https://www.youtube.com/embed/8OhtxYRlTIg" },
                    {"Hammer","" },
                    {"Hunting Horn","https://www.youtube.com/embed/DPX1MMBNojc" },
                    {"Lance","" },
                    {"Gunlance","https://www.youtube.com/embed/m4tZGBrdZWo" },
                    {"Tonfa","https://www.youtube.com/embed/r7kPtv2v_m0" },
                    {"Switch Axe F","https://www.youtube.com/embed/YNxAe0emonY" },
                    {"Magnet Spike","https://www.youtube.com/embed/ZrGuUhbS06M" },
                    {"Light Bowgun","https://www.youtube.com/embed/YjPCAW2VVcE" },
                    {"Heavy Bowgun","https://www.youtube.com/embed/S5UYDotONl0" },
                    {"Bow","https://www.youtube.com/embed/DXjkOqzyll8" },
                },
                "https://monsterhunter.fandom.com/wiki/Ruler_Guanzorumu"
                ),

            new MonsterInfo("[Musou] Shifting Mi Ru",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mi-ru_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","https://www.youtube.com/embed/V8YpNhcqJdE" },
                    {"Great Sword","https://www.youtube.com/embed/7FwFQgJdumc" },
                    {"Long Sword","https://www.youtube.com/embed/DpXkiVAvxYs" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","https://www.youtube.com/embed/4KbAyc9PElo" },
                    {"Switch Axe F","https://www.youtube.com/embed/FyfYV8uuR9Q" },
                    {"Magnet Spike","https://www.youtube.com/embed/ZP-IYyGhzu0" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","https://www.youtube.com/embed/6UPpgA1V2n8" },
                },
                "https://monsterhunter.fandom.com/wiki/Mysterious_Mi_Ru"
                ),

            new MonsterInfo("[Musou] Sparkling Zerureusu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zeruK_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","https://www.youtube.com/embed/kTIjBZtjZvk" },
                    {"Great Sword","https://www.youtube.com/embed/J49O7mh3Zyo" },
                    {"Long Sword","https://www.youtube.com/embed/fTdHHEkZ6ho" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","https://www.youtube.com/embed/xRsJPgvGeLo" },
                    {"Gunlance","https://www.youtube.com/embed/mMmweo6r5_Y" },
                    {"Tonfa","https://www.youtube.com/embed/Tog0m5RnMzg" },
                    {"Switch Axe F","https://www.youtube.com/embed/QJSEa9tle4U" },
                    {"Magnet Spike","https://www.youtube.com/embed/xCSTBXjhE_4" },
                    {"Light Bowgun","https://www.youtube.com/embed/Amg_AoLuabw" },
                    {"Heavy Bowgun","" },
                    {"Bow","https://www.youtube.com/embed/w3tjb5NNHQA" },
                },
                "https://monsterhunter.fandom.com/wiki/Sparkling_Zerureusu"
                ),

            new MonsterInfo("[Musou] Starving Deviljho",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/joeK_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","https://www.youtube.com/embed/RMiSqvbCaPA" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","https://www.youtube.com/embed/9jOg_zTG0X8" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Starving_Deviljho"
                ),

            new MonsterInfo("[Musou] Thirsty Pariapuria",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/paria_ng.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","https://www.youtube.com/embed/_B6E8ijgFis" },
                    {"Great Sword","https://www.youtube.com/embed/ySVMaA0LdiM" },
                    {"Long Sword","https://www.youtube.com/embed/TGyS7w7dbuc" },
                    {"Hammer","https://www.youtube.com/embed/onsSrWl1kfE" },
                    {"Hunting Horn","https://www.youtube.com/embed/M_j4qp9efYs" },
                    {"Lance","https://www.youtube.com/embed/TkriXHXyMWw" },
                    {"Gunlance","" },
                    {"Tonfa","https://www.youtube.com/embed/wVdgUosZvZc" },
                    {"Switch Axe F","https://www.youtube.com/embed/55Wfapp4eDI" },
                    {"Magnet Spike","https://www.youtube.com/embed/myyS3hL7XT0" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","https://www.youtube.com/embed/5kZF5LIWT3A" },
                    {"Bow","https://www.youtube.com/embed/Y3YY1v4afJA" },
                },
                "https://monsterhunter.fandom.com/wiki/Thirsty_Pariapuria"
                ),


            new MonsterInfo("Zenith Akura Vashimu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/aqura_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Akura_Vashimu"
                ),
            new MonsterInfo("Zenith Anorupatisu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/anolu_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Anorupatisu"
                ),
            new MonsterInfo("Zenith Baruragaru",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/bal_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Baruragaru"
                ),
            new MonsterInfo("Zenith Blangonga",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dodobura_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Blangonga"
                ),
            new MonsterInfo("Zenith Daimyo Hermitaur",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/daimyo_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Daimyo_Hermitaur"
                ),
            new MonsterInfo("Zenith Doragyurosu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/doragyu_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Doragyurosu"
                ),

            new MonsterInfo("Zenith Espinas",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/esp_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Espinas"
                ),
            new MonsterInfo("Zenith Gasurabazura",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gasra_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Gasurabazura"
                ),
            new MonsterInfo("Zenith Giaorugu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/giao_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Giaorugu"
                ),
            new MonsterInfo("Zenith Gravios",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gura_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Gravios"
                ),

            new MonsterInfo("Zenith Harudomerugu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hald_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Harudomerugu"
                ),
            new MonsterInfo("Zenith Hypnoc",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hipu_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Hypnocatrice"
                ),
            new MonsterInfo("Zenith Hyujikiki",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hyuji_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Hyujikiki"
                ),
            new MonsterInfo("Zenith Inagami",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ina_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Inagami"
                ),
            new MonsterInfo("Zenith Khezu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/furu_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Khezu"
                ),

            new MonsterInfo("Zenith Midogaron",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mido_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Midogaron"
                ),

            new MonsterInfo("Zenith Plesioth",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gano_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Plesioth"
                ),
            new MonsterInfo("Zenith Rathalos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reusu_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","https://www.youtube.com/embed/pGqYN1ks5AQ" },
                    {"Dual Swords","" },
                    {"Great Sword","https://www.youtube.com/embed/LQra_886zSA" },
                    {"Long Sword","https://www.youtube.com/embed/2rKIOjUx1wU" },
                    {"Hammer","https://www.youtube.com/embed/Dmmr6rrRkXg" },
                    {"Hunting Horn","https://www.youtube.com/embed/cKe1_xqLGm8" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","https://www.youtube.com/embed/Xt96sulv-Wc" },
                    {"Switch Axe F","https://www.youtube.com/embed/mI2WKvwVKXU" },
                    {"Magnet Spike","https://www.youtube.com/embed/gQT9DiO7BJ4" },
                    {"Light Bowgun","https://www.youtube.com/embed/R9kK5AmjcHk" },
                    {"Heavy Bowgun","" },
                    {"Bow","https://www.youtube.com/embed/bsQ-skmpe4Q" },
                },
                "https://monsterhunter.fandom.com/wiki/Rathalos"
                ),
            new MonsterInfo("Zenith Rukodiora",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ruco_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Rukodiora"
                ),
            new MonsterInfo("Zenith Taikun Zamuza",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/taikun_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Taikun_Zamuza"
                ),
            new MonsterInfo("Zenith Tigrex",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/tiga_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Tigrex"
                ),
            new MonsterInfo("Zenith Toridcless",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/torid_ni.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","" },
                    {"Dual Swords","" },
                    {"Great Sword","" },
                    {"Long Sword","" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","" },
                    {"Gunlance","" },
                    {"Tonfa","" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","" },
                    {"Light Bowgun","" },
                    {"Heavy Bowgun","" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenith_Toridcless"
                ),


                        new MonsterInfo("1st District Duremudira",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dolem1_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Duremudira"
                ),            new MonsterInfo("2nd District Duremudira",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dolem2_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Duremudira"
                ),            new MonsterInfo("Abiorugu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/abio_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Abiorugu"
                ),            new MonsterInfo("Akantor",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/akamu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Akantor"
                ),            new MonsterInfo("Akura Jebia",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/aquraj_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Akura_Jebia"
                ),            new MonsterInfo("Akura Vashimu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/aqura_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Akura_Vashimu"
                ),            new MonsterInfo("Amatsu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/amatsu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Amatsu"
                ),            new MonsterInfo("Anorupatisu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/anolu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Anorupatisu"
                ),            new MonsterInfo("Anteka",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gau_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Anteka"
                ),            new MonsterInfo("Apceros",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/apuke_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Apceros"
                ),            new MonsterInfo("Aptonoth",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/apono_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Aptonoth"
                ),             new MonsterInfo("Aruganosu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/volgin_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Aruganosu"
                ),            new MonsterInfo("Ashen Lao-Shan Lung",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/raoao_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Ashen_Lao-Shan_Lung"
                ),            new MonsterInfo("Azure Rathalos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reusuao_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Azure_Rathalos"
                ),            new MonsterInfo("Barioth",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/berio_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Barioth"
                ),            new MonsterInfo("Baruragaru",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/bal_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Baruragaru"
                ),
                new MonsterInfo("Basarios",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/basa_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Basarios"
                ),new MonsterInfo("Berserk Raviente",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/lavieG_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Berserk_Raviente"
                ),new MonsterInfo("Berukyurosu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/beru_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Berukyurosu"
                ),new MonsterInfo("Black Diablos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/deakuro_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Black_Diablos"
                ),new MonsterInfo("Black Gravios",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gurakuro_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Black_Gravios"
                ),new MonsterInfo("Blango",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/bura_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Blango"
                ),new MonsterInfo("Blangonga",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dodobura_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Blangonga"
                ),new MonsterInfo("Blue Yian Kut-Ku",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kukkuao_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Blue_Yian_Kut-Ku"
                ),new MonsterInfo("Bogabadorumu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/boga_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Bogabadorumu"
                ),new MonsterInfo("Brachydios",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/buraki_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Brachydios"
                ),new MonsterInfo("Bright Hypnoc",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hipuao_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Breeding_Season_Hypnocatrice"
                ),new MonsterInfo("Bulldrome",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosburu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Bulldrome"
                ),new MonsterInfo("Bullfango",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/buru_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Bullfango"
                ),new MonsterInfo("Burukku",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/brook_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Burukku"
                ),new MonsterInfo("Ceanataur",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/yao_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Ceanataur"
                ),new MonsterInfo("Cephadrome",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosgare_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Cephadrome"
                ),new MonsterInfo("Cephalos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gare_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Cephalos"
                ),new MonsterInfo("Chameleos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/oo_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Chameleos"
                ),new MonsterInfo("Conga",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/konga_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Conga"
                ),new MonsterInfo("Congalala",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/babakonga_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Congalala"
                ),new MonsterInfo("Crimson Fatalis",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/miraval_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Crimson_Fatalis"
                ),new MonsterInfo("Daimyo Hermitaur",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/daimyo_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Daimyo_Hermitaur"
                ),new MonsterInfo("Deviljho",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/joe_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Deviljho"
                ),new MonsterInfo("Diablos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dea_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Diablos"
                ),new MonsterInfo("Diorex",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dio_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Diorekkusu"
                ),new MonsterInfo("Disufiroa",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/disf_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","https://www.youtube.com/embed/leNWg4HoAbQ" },
                    {"Dual Swords","https://www.youtube.com/embed/obcm9-ebei8" },
                    {"Great Sword","https://www.youtube.com/embed/obcm9-ebei8" },
                    {"Long Sword","https://www.youtube.com/embed/3dJR6YqTZcM" },
                    {"Hammer","" },
                    {"Hunting Horn","" },
                    {"Lance","https://www.youtube.com/embed/TTj9T6Tskcg" },
                    {"Gunlance","https://www.youtube.com/embed/wgR1Bf-kApo" },
                    {"Tonfa","https://www.youtube.com/embed/1sjynMO0CJk" },
                    {"Switch Axe F","" },
                    {"Magnet Spike","https://www.youtube.com/embed/FYS_yi7EQmA" },
                    {"Light Bowgun","https://www.youtube.com/embed/TPR4nYlWFgY" },
                    {"Heavy Bowgun","https://www.youtube.com/embed/MHf2R504_xc" },
                    {"Bow","" },
                },
                "https://monsterhunter.fandom.com/wiki/Disufiroa"
                ),new MonsterInfo("Doragyurosu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/doragyu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Doragyurosu"
                ),new MonsterInfo("Dyuragaua",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dura_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Dyuragaua"
                ),new MonsterInfo("Egyurasu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/egura_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Egyurasu"
                ),new MonsterInfo("Elzelion",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/elze_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Eruzerion"
                ),new MonsterInfo("Erupe",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/erupe_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Erupe"
                ),new MonsterInfo("Espinas",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/esp_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Espinas"
                ),new MonsterInfo("Farunokku",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/faru_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Farunokku"
                ),new MonsterInfo("Fatalis",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mira_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Fatalis"
                ),new MonsterInfo("Felyne",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/airu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Felyne"
                ),new MonsterInfo("Forokururu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/folo_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Forokururu"
                ),new MonsterInfo("Garuba Daora",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/galba_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Garuba_Daora"
                ),new MonsterInfo("Gasurabazura",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gasra_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gasurabazura"
                ),new MonsterInfo("Gendrome",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosgene_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gendrome"
                ),new MonsterInfo("Genprey",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gene_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Genprey"
                ),new MonsterInfo("Giaorugu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/giao_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Giaorugu"
                ),new MonsterInfo("Giaprey",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/giano_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Giaprey"
                ),new MonsterInfo("Gogomoa",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gogo_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gogomoa"
                ),new MonsterInfo("Gold Rathian",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reiakin_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gold_Rathian"
                ),new MonsterInfo("Gore Magala",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/goa_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gore_Magala"
                ),new MonsterInfo("Goruganosu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/volkin_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Goruganosu"
                ),new MonsterInfo("Gougarf, Ray",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/goglf_r_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Ray_Gougarf"
                ),new MonsterInfo("Gougarf, Lolo",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/goglf_l_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Lolo_Gougarf"
                ),new MonsterInfo("Gravios",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gura_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gravios"
                ),new MonsterInfo("Great Thunderbug",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/raikou_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Great_Thunderbug"
                ),new MonsterInfo("Green Plesioth",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ganomidori_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Green_Plesioth"
                ),new MonsterInfo("Guanzorumu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/guan_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Guanzorumu"
                ),new MonsterInfo("Gureadomosu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/glare_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gureadomosu"
                ),new MonsterInfo("Gurenzeburu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/guren_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gurenzeburu"
                ),new MonsterInfo("Gypceros",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/geryo_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gypceros"
                ),new MonsterInfo("Harudomerugu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hald_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Harudomerugu"
                ),new MonsterInfo("Hermitaur",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gami_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Hermitaur"
                ),new MonsterInfo("Hornetaur",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kanta_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Hornetaur"
                ),new MonsterInfo("Hypnoc",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hipu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Hypnocatrice"
                ),new MonsterInfo("Hyujikiki",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hyuji_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Hyujikiki"
                ),new MonsterInfo("Inagami",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ina_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Inagami"
                ),new MonsterInfo("Iodrome",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosios_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Iodrome"
                ),new MonsterInfo("Ioprey",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ios_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Ioprey"
                ),new MonsterInfo("Kamu Orugaron",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/olgk_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Kamu_Orugaron"
                ),new MonsterInfo("Kelbi",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kerubi_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Kelbi"
                ),new MonsterInfo("Keoaruboru",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/keoa_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Keoaruboru"
                ),new MonsterInfo("Khezu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/furu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Khezu"
                ),new MonsterInfo("King Shakalaka",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/cyacyaKing_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/King_Shakalaka"
                ),new MonsterInfo("Kirin",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kirin_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Kirin"
                ),new MonsterInfo("Kokomoa",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/koko_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Gogomoa"
                ),new MonsterInfo("Kuarusepusu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/qual_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Kuarusepusu"
                ),new MonsterInfo("Kushala Daora",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kusha_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Kushala_Daora"
                ),new MonsterInfo("Kusubami",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kusb_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Kusubami"
                ),new MonsterInfo("Lao-Shan Lung",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/rao_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Lao-Shan_Lung"
                ),new MonsterInfo("Lavasioth",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/vol_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Lavasioth"
                ),new MonsterInfo("Lunastra",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/nana_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Lunastra"
                ),new MonsterInfo("Melynx",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/merura_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Melynx"
                ),new MonsterInfo("Meraginasu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mera_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Meraginasu"
                ),new MonsterInfo("Mi Ru",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mi-ru_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Mi_Ru"
                ),new MonsterInfo("Midogaron",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mido_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Midogaron"
                ),new MonsterInfo("Monoblos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mono_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Monoblos"
                ),new MonsterInfo("Mosswine",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mos_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Mosswine"
                ),new MonsterInfo("Nargacuga",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/nalga_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Nargacuga"
                ),new MonsterInfo("Nono Orugaron",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/olgn_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Nono_Orugaron"
                ),new MonsterInfo("Odibatorasu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/odiva_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Odibatorasu"
                ),new MonsterInfo("Orange Espinas",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/espcya_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Flaming_Espinas"
                ),new MonsterInfo("Pariapuria",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/paria_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Pariapuria"
                ),new MonsterInfo("Pink Rathian",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reiasa_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Pink_Rathian"
                ),new MonsterInfo("Plesioth",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gano_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Plesioth"
                ),new MonsterInfo("Poborubarumu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/pobol_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Poborubarumu"
                ),new MonsterInfo("Pokara",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/pokara_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Pokara"
                ),new MonsterInfo("Pokaradon",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/pokaradon_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Pokaradon"
                ),new MonsterInfo("Popo",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/popo_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Popo"
                ),new MonsterInfo("PSO2 Rappy",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/lappy_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                RickRoll
                ),new MonsterInfo("Purple Gypceros",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/geryoao_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Purple_Gypceros"
                ),new MonsterInfo("Rajang",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ra_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Rajang"
                ),new MonsterInfo("Rathalos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reusu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Rathalos"
                ),new MonsterInfo("Rathian",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reia_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Rathian"
                ),new MonsterInfo("Raviente",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/lavie_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Raviente"
                ),new MonsterInfo("Rebidiora",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/levy_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Rebidiora"
                ),new MonsterInfo("Red Khezu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/furuaka_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Red_Khezu"
                ),new MonsterInfo("Red Lavasioth",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/volaka_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Red_Volganos"
                ),new MonsterInfo("Remobra",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gabu_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Remobra"
                ),new MonsterInfo("Rocks",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/iwa_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                RickRoll
                ),new MonsterInfo("Rukodiora",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ruco_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Rukodiora"
                ),new MonsterInfo("Rusted Kushala Daora",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kushasabi_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Rusted_Kushala_Daora"
                ),new MonsterInfo("Seregios",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/sell_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Seregios"
                ),new MonsterInfo("Shagaru Magala",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/shagal_nh.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Shagaru_Magala"
                ),new MonsterInfo("Shakalaka",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/cyacya_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Shakalaka"
                ),new MonsterInfo("Shantien",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/shan_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Shantien"
                ),new MonsterInfo("Shen Gaoren",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/shen_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Rathian"
                ),new MonsterInfo("Shogun Ceanataur",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/syougun_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Shogun_Ceanataur"
                ),new MonsterInfo("Silver Hypnoc",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hipusiro_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Silver_Hypnocatrice"
                ),new MonsterInfo("Silver Rathalos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reusugin_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Silver_Rathalos"
                ),new MonsterInfo("Stygian Zinogre",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zingoku_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Stygian_Zinogre"
                ),new MonsterInfo("Taikun Zamuza",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/taikun_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Taikun_Zamuza"
                ),new MonsterInfo("Teostra",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/teo_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Teostra"
                ),new MonsterInfo("Tigrex",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/tiga_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Tigrex"
                ),new MonsterInfo("Toa Tesukatora",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/toa_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Toa_Tesukatora"
                ),new MonsterInfo("Toridcless",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/torid_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Toridcless"
                ),new MonsterInfo("UNKNOWN",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ra-ro_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"Sword and Shield","https://www.youtube.com/embed/REF3OBNu4Wo" },
                    {"Dual Swords","https://www.youtube.com/embed/fAremzgKfos" },
                    {"Great Sword","https://www.youtube.com/embed/xX7KH0r68f4" },
                    {"Long Sword","https://www.youtube.com/embed/6G8865fllSo" },
                    {"Hammer","https://www.youtube.com/embed/A6pqZgrA-9o" },
                    {"Hunting Horn","https://www.youtube.com/embed/-qMOOTOOnrw" },
                    {"Lance","https://www.youtube.com/embed/m66unQSZzIc" },
                    {"Gunlance","https://www.youtube.com/embed/yCz_sigKiGs" },
                    {"Tonfa","https://www.youtube.com/embed/sj_jMp0T3Pc" },
                    {"Switch Axe F","https://www.youtube.com/embed/hZvxtDsqSf4" },
                    {"Magnet Spike","https://www.youtube.com/embed/FLmxy-xCoqM" },
                    {"Light Bowgun","https://www.youtube.com/embed/xTvTlPkIp3w" },
                    {"Heavy Bowgun","" },
                    {"Bow","https://www.youtube.com/embed/gp48Gk-y_JY" },
                },
                "https://monsterhunter.fandom.com/wiki/Unknown_(Black_Flying_Wyvern)"
                ),new MonsterInfo("Uragaan",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ura_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Uragaan"
                ),new MonsterInfo("Uruki",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ulky_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Uruki"
                ),new MonsterInfo("Varusaburosu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/valsa_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Varusaburosu"
                ),new MonsterInfo("Velocidrome",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosran_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Velocidrome"
                ),new MonsterInfo("Velociprey",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ran_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Velociprey"
                ),new MonsterInfo("Vespoid",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/rango_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Vespoid"
                ),new MonsterInfo("Voljang",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/Vau_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Voljang"
                ),new MonsterInfo("White Espinas",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/espsiro_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Espinas_Rare_Species"
                ),new MonsterInfo("White Fatalis",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/miraru_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/White_Fatalis"
                ),new MonsterInfo("White Monoblos",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/monosiro_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/White_Monoblos"
                ),new MonsterInfo("Yama Kurai",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/yamac_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Yama_Kurai"
                ),new MonsterInfo("Yama Tsukami",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/yama_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Yama_Tsukami"
                ),new MonsterInfo("Yian Garuga",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/garuru_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Yian_Garuga"
                ),new MonsterInfo("Yian Kut-Ku",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kukku_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Yian_Kut-Ku"
                ),new MonsterInfo("Zenaserisu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zena_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zenaserisu"
                ),new MonsterInfo("Zerureusu",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zeru_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zerureusu"
                ),new MonsterInfo("Zinogre",
                "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zin_n.htm",
                new System.Collections.Generic.Dictionary<string, string>()
                {
                    {"","" },
                },
                "https://monsterhunter.fandom.com/wiki/Zinogre"
                )
            };
    };
}