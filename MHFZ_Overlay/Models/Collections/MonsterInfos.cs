// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;

/// <summary>
/// The monster info list.
/// <br>TODO: missing labels</br>
/// </summary>
public static class MonsterInfos
{
    private static readonly string RickRoll = string.Empty;

    public static IReadOnlyList<MonsterInfo> MonsterInfoIDs { get; } = new List<MonsterInfo>
    {
        new MonsterInfo(
            "[Musou] Arrogant Duremudira",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dolem_n.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", "https://www.youtube.com/embed/jqaUPZ8DVfw" },
                { "Dual Swords", "https://www.youtube.com/embed/qa6LfcOr_S0" },
                { "Great Sword", "https://www.youtube.com/embed/MQit-3HZLhM" },
                { "Long Sword", "https://www.youtube.com/embed/8letGMGpjbU" },
                { "Hammer", "https://www.youtube.com/embed/Z2OwUAWROio" },
                { "Hunting Horn", string.Empty },
                { "Lance", "https://www.youtube.com/embed/laa4x-V_qrQ" },
                { "Gunlance", "https://www.youtube.com/embed/68WK1F69fMo" },
                { "Tonfa", "https://www.youtube.com/embed/ry1faWMTdtQ" },
                { "Switch Axe F", "https://www.youtube.com/embed/HV8qzOGYEoM" },
                { "Magnet Spike", "https://www.youtube.com/embed/0Av3vuNs1pA" },
                { "Light Bowgun", "https://www.youtube.com/embed/aWsO7pdp8OU" },
                { "Heavy Bowgun", string.Empty },
                { "Bow", "https://www.youtube.com/embed/9UVFdkZT6SQ" },
            },
            "https://monsterhunter.fandom.com/wiki/Arrogant_Duremudira",
        "https://wycademy.vercel.app/bestiary/Arrogant_Duremudira".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "[Musou] Blinking Nargacuga",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/nalgaK_n.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", "https://www.youtube.com/embed/baWvEuLLwxk" },
                { "Dual Swords", "https://www.youtube.com/embed/dlBMmEwCO6k" },
                { "Great Sword", "https://www.youtube.com/embed/MA46kDZpDEs" },
                { "Long Sword", "https://www.youtube.com/embed/qHCVjd0Ov7E" },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", "https://www.youtube.com/embed/TwWPTcuwK4o" },
                { "Gunlance", "https://www.youtube.com/embed/IQRRyzkUSew" },
                { "Tonfa", "https://www.youtube.com/embed/Q1bnA9LnWlU" },
                { "Switch Axe F", "https://www.youtube.com/embed/n6SdO7Cpugg" },
                { "Magnet Spike", "https://www.youtube.com/embed/n6SdO7Cpugg" },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", "https://www.youtube.com/embed/pXPh2uqvFD8" },
            },
            "https://monsterhunter.fandom.com/wiki/Blinking_Nargacuga",
        "https://wycademy.vercel.app/bestiary/Blinking_Nargacuga".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "[Musou] Bombardier Bogabadorumu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/bogaK_n.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", "https://youtube.com/embed/ak1o6N06V6U" },
                { "Dual Swords", "https://youtube.com/embed/XAIvnGBVIp8" },
                { "Great Sword", "https://youtube.com/embed/huULgUrupPM" },
                { "Long Sword", "https://youtube.com/embed/X49Z85BCVIk" },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", "https://youtube.com/embed/kL1Zkhoa9eg" },
                { "Gunlance", "https://youtube.com/embed/Lc3fS_010Ws" },
                { "Tonfa", "https://youtube.com/embed/SyVWewqI1nw" },
                { "Switch Axe F", "https://youtube.com/embed/LXaYqpRonrk" },
                { "Magnet Spike", "https://www.youtube.com/embed/60UlLymak2k" },
                { "Light Bowgun", "https://www.youtube.com/embed/_D5UqIjNm4Q" },
                { "Heavy Bowgun", string.Empty },
                { "Bow", "https://www.youtube.com/embed/3aHSjPo90Vc" },
            },
            "https://monsterhunter.fandom.com/wiki/Bombardier_Bogabadorumu",
        "https://wycademy.vercel.app/bestiary/Bombardier_Bogabadorumu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "[Musou] Burning Freezing Elzelion",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/elze_n.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", "https://www.youtube.com/embed/RAdft8GHKvU" },
                { "Dual Swords", "https://www.youtube.com/embed/X6F2uywrZiE" },
                { "Great Sword", "https://www.youtube.com/embed/lN-4yfgd9y0" },
                { "Long Sword", "https://www.youtube.com/embed/5EC60bXCUZ8" },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", "https://www.youtube.com/embed/vXkz-JgDWJQ" },
                { "Gunlance", "https://www.youtube.com/embed/KxNao8p8eYw" },
                { "Tonfa", "https://www.youtube.com/embed/4TmtciKAJyg" },
                { "Switch Axe F", "https://www.youtube.com/embed/H5e7TYB1B9A" },
                { "Magnet Spike", "https://www.youtube.com/embed/n27VlF1r894" },
                { "Light Bowgun", "https://www.youtube.com/embed/lWZaWexqzxE" },
                { "Heavy Bowgun", "https://www.youtube.com/embed/yGLLnEDVZoY" },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Burning_Freezing_Eruzerion",
        "https://wycademy.vercel.app/bestiary/Burning_Freezing_Elzelion".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "[Musou] Howling Zinogre",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zin_ng.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", "https://www.youtube.com/embed/wc8lCUg0pko" },
                { "Dual Swords", "https://www.youtube.com/embed/QOK1Kg5fzHk" },
                { "Great Sword", "https://www.youtube.com/embed/sjBwjTN1VVg" },
                { "Long Sword", "https://www.youtube.com/embed/C5eJ4wk9fYk" },
                { "Hammer", "https://www.youtube.com/embed/R1rpbYzrCnQ" },
                { "Hunting Horn", "https://www.youtube.com/embed/8GRK_r4al_Y" },
                { "Lance", "https://www.youtube.com/embed/fii-JAZACQM" },
                { "Gunlance", "https://www.youtube.com/embed/hawNX97Xp28" },
                { "Tonfa", "https://www.youtube.com/embed/I8BbYtfJZdI" },
                { "Switch Axe F", "https://www.youtube.com/embed/Dg9xQkCzqmo" },
                { "Magnet Spike", "https://www.youtube.com/embed/bUpRnWAlpUw" },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", "https://www.youtube.com/embed/GqMh_KUJx4E" },
            },
            "https://monsterhunter.fandom.com/wiki/Howling_Zinogre",
        "https://wycademy.vercel.app/bestiary/Howling_Zinogre".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "[Musou] Ruling Guanzorumu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/guan_ng.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", "https://www.youtube.com/embed/UzLiZrO9CDE" },
                { "Dual Swords", "https://www.youtube.com/embed/vMfgFOYoO3Q" },
                { "Great Sword", "https://www.youtube.com/embed/3OUgl9HnTUQ" },
                { "Long Sword", "https://www.youtube.com/embed/8OhtxYRlTIg" },
                { "Hammer", string.Empty },
                { "Hunting Horn", "https://www.youtube.com/embed/DPX1MMBNojc" },
                { "Lance", string.Empty },
                { "Gunlance", "https://www.youtube.com/embed/m4tZGBrdZWo" },
                { "Tonfa", "https://www.youtube.com/embed/r7kPtv2v_m0" },
                { "Switch Axe F", "https://www.youtube.com/embed/YNxAe0emonY" },
                { "Magnet Spike", "https://www.youtube.com/embed/ZrGuUhbS06M" },
                { "Light Bowgun", "https://www.youtube.com/embed/YjPCAW2VVcE" },
                { "Heavy Bowgun", "https://www.youtube.com/embed/S5UYDotONl0" },
                { "Bow", "https://www.youtube.com/embed/DXjkOqzyll8" },
            },
            "https://monsterhunter.fandom.com/wiki/Ruler_Guanzorumu",
        "https://wycademy.vercel.app/bestiary/Ruling_Guanzorumu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "[Musou] Shifting Mi Ru",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mi-ru_n.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", "https://www.youtube.com/embed/V8YpNhcqJdE" },
                { "Great Sword", "https://www.youtube.com/embed/7FwFQgJdumc" },
                { "Long Sword", "https://www.youtube.com/embed/DpXkiVAvxYs" },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", "https://www.youtube.com/embed/4KbAyc9PElo" },
                { "Switch Axe F", "https://www.youtube.com/embed/FyfYV8uuR9Q" },
                { "Magnet Spike", "https://www.youtube.com/embed/ZP-IYyGhzu0" },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", "https://www.youtube.com/embed/6UPpgA1V2n8" },
            },
            "https://monsterhunter.fandom.com/wiki/Mysterious_Mi_Ru",
        "https://wycademy.vercel.app/bestiary/Shifting_Mi_Ru".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "[Musou] Sparkling Zerureusu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zeruK_n.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", "https://www.youtube.com/embed/kTIjBZtjZvk" },
                { "Great Sword", "https://www.youtube.com/embed/J49O7mh3Zyo" },
                { "Long Sword", "https://www.youtube.com/embed/fTdHHEkZ6ho" },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", "https://www.youtube.com/embed/xRsJPgvGeLo" },
                { "Gunlance", "https://www.youtube.com/embed/mMmweo6r5_Y" },
                { "Tonfa", "https://www.youtube.com/embed/Tog0m5RnMzg" },
                { "Switch Axe F", "https://www.youtube.com/embed/QJSEa9tle4U" },
                { "Magnet Spike", "https://www.youtube.com/embed/xCSTBXjhE_4" },
                { "Light Bowgun", "https://www.youtube.com/embed/Amg_AoLuabw" },
                { "Heavy Bowgun", string.Empty },
                { "Bow", "https://www.youtube.com/embed/w3tjb5NNHQA" },
            },
            "https://monsterhunter.fandom.com/wiki/Sparkling_Zerureusu",
        "https://wycademy.vercel.app/bestiary/Sparkling_Zerureusu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "[Musou] Starving Deviljho",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/joeK_n.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", "https://www.youtube.com/embed/RMiSqvbCaPA" },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", "https://www.youtube.com/embed/9jOg_zTG0X8" },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Starving_Deviljho",
        "https://wycademy.vercel.app/bestiary/Starving_Deviljho".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "[Musou] Thirsty Pariapuria",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/paria_ng.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", "https://www.youtube.com/embed/_B6E8ijgFis" },
                { "Great Sword", "https://www.youtube.com/embed/ySVMaA0LdiM" },
                { "Long Sword", "https://www.youtube.com/embed/TGyS7w7dbuc" },
                { "Hammer", "https://www.youtube.com/embed/onsSrWl1kfE" },
                { "Hunting Horn", "https://www.youtube.com/embed/M_j4qp9efYs" },
                { "Lance", "https://www.youtube.com/embed/TkriXHXyMWw" },
                { "Gunlance", string.Empty },
                { "Tonfa", "https://www.youtube.com/embed/wVdgUosZvZc" },
                { "Switch Axe F", "https://www.youtube.com/embed/55Wfapp4eDI" },
                { "Magnet Spike", "https://www.youtube.com/embed/myyS3hL7XT0" },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", "https://www.youtube.com/embed/5kZF5LIWT3A" },
                { "Bow", "https://www.youtube.com/embed/Y3YY1v4afJA" },
            },
            "https://monsterhunter.fandom.com/wiki/Thirsty_Pariapuria",
        "https://wycademy.vercel.app/bestiary/Thirsty_Pariapuria".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Akura Vashimu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/aqura_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Akura_Vashimu",
        "https://wycademy.vercel.app/bestiary/Zenith_Akura_Vashimu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Anorupatisu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/anolu_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Anorupatisu",
        "https://wycademy.vercel.app/bestiary/Zenith_Anorupatisu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Baruragaru",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/bal_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Baruragaru",
        "https://wycademy.vercel.app/bestiary/Zenith_Baruragaru".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Blangonga",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dodobura_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Blangonga",
        "https://wycademy.vercel.app/bestiary/Zenith_Blangonga".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Daimyo Hermitaur",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/daimyo_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Daimyo_Hermitaur",
        "https://wycademy.vercel.app/bestiary/Zenith_Daimyo_Hermitaur".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Doragyurosu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/doragyu_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Doragyurosu",
        "https://wycademy.vercel.app/bestiary/Zenith_Doragyurosu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Espinas",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/esp_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Espinas",
        "https://wycademy.vercel.app/bestiary/Zenith_Espinas".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Gasurabazura",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gasra_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Gasurabazura",
        "https://wycademy.vercel.app/bestiary/Zenith_Gasurabazura".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Giaorugu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/giao_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Giaorugu",
        "https://wycademy.vercel.app/bestiary/Zenith_Giaorugu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Gravios",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gura_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Gravios",
        "https://wycademy.vercel.app/bestiary/Zenith_Gravios".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Harudomerugu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hald_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Harudomerugu",
        "https://wycademy.vercel.app/bestiary/Zenith_Harudomerugu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Hypnoc",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hipu_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Hypnocatrice",
        "https://wycademy.vercel.app/bestiary/Zenith_Hypnocatrice".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Hyujikiki",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hyuji_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Hyujikiki",
        "https://wycademy.vercel.app/bestiary/Zenith_Hyujikiki".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Inagami",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ina_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Inagami",
        "https://wycademy.vercel.app/bestiary/Zenith_Inagami".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Khezu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/furu_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Khezu",
        "https://wycademy.vercel.app/bestiary/Zenith_Khezu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Midogaron",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mido_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Midogaron",
        "https://wycademy.vercel.app/bestiary/Zenith_Midogaron".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Plesioth",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gano_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Plesioth",
        "https://wycademy.vercel.app/bestiary/Zenith_Plesioth".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Rathalos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reusu_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", "https://www.youtube.com/embed/pGqYN1ks5AQ" },
                { "Dual Swords", string.Empty },
                { "Great Sword", "https://www.youtube.com/embed/LQra_886zSA" },
                { "Long Sword", "https://www.youtube.com/embed/2rKIOjUx1wU" },
                { "Hammer", "https://www.youtube.com/embed/Dmmr6rrRkXg" },
                { "Hunting Horn", "https://www.youtube.com/embed/cKe1_xqLGm8" },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", "https://www.youtube.com/embed/Xt96sulv-Wc" },
                { "Switch Axe F", "https://www.youtube.com/embed/mI2WKvwVKXU" },
                { "Magnet Spike", "https://www.youtube.com/embed/gQT9DiO7BJ4" },
                { "Light Bowgun", "https://www.youtube.com/embed/R9kK5AmjcHk" },
                { "Heavy Bowgun", string.Empty },
                { "Bow", "https://www.youtube.com/embed/bsQ-skmpe4Q" },
            },
            "https://monsterhunter.fandom.com/wiki/Rathalos",
        "https://wycademy.vercel.app/bestiary/Rathalos".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Rukodiora",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ruco_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Rukodiora",
        "https://wycademy.vercel.app/bestiary/Zenith_Rukodiora".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Taikun Zamuza",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/taikun_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Taikun_Zamuza",
        "https://wycademy.vercel.app/bestiary/Zenith_Taikun_Zamuza".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Tigrex",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/tiga_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Tigrex",
        "https://wycademy.vercel.app/bestiary/Zenith_Tigrex".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Zenith Toridcless",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/torid_ni.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", string.Empty },
                { "Dual Swords", string.Empty },
                { "Great Sword", string.Empty },
                { "Long Sword", string.Empty },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", string.Empty },
                { "Gunlance", string.Empty },
                { "Tonfa", string.Empty },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", string.Empty },
                { "Light Bowgun", string.Empty },
                { "Heavy Bowgun", string.Empty },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenith_Toridcless",
        "https://wycademy.vercel.app/bestiary/Zenith_Toridcless".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "1st District Duremudira",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dolem1_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Duremudira",
        "https://wycademy.vercel.app/bestiary/Duremudira".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "2nd District Duremudira",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dolem2_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Duremudira",
        "https://wycademy.vercel.app/bestiary/Duremudira".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Abiorugu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/abio_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Abiorugu",
        "https://wycademy.vercel.app/bestiary/Abiorugu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Akantor",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/akamu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Akantor",
        "https://wycademy.vercel.app/bestiary/Akantor".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Akura Jebia",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/aquraj_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Akura_Jebia",
        "https://wycademy.vercel.app/bestiary/Akura_Jebia".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Akura Vashimu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/aqura_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Akura_Vashimu",
        "https://wycademy.vercel.app/bestiary/Akura_Vashimu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Amatsu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/amatsu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Amatsu",
        "https://wycademy.vercel.app/bestiary/Amatsu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Anorupatisu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/anolu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Anorupatisu",
        "https://wycademy.vercel.app/bestiary/Anorupatisu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Anteka",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gau_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Anteka",
        "https://wycademy.vercel.app/bestiary/Anteka".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Apceros",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/apuke_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Apceros",
        "https://wycademy.vercel.app/bestiary/Apceros".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Aptonoth",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/apono_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Aptonoth",
        "https://wycademy.vercel.app/bestiary/Aptonoth".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Aruganosu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/volgin_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Aruganosu",
        "https://wycademy.vercel.app/bestiary/Aruganosu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Ashen Lao-Shan Lung",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/raoao_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Ashen_Lao-Shan_Lung",
        "https://wycademy.vercel.app/bestiary/Ashen_Lao-Shan_Lung".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Azure Rathalos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reusuao_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Azure_Rathalos",
        "https://wycademy.vercel.app/bestiary/Azure_Rathalos".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Barioth",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/berio_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Barioth",
        "https://wycademy.vercel.app/bestiary/Barioth".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Baruragaru",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/bal_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Baruragaru",
        "https://wycademy.vercel.app/bestiary/Baruragaru".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Basarios",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/basa_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Basarios",
        "https://wycademy.vercel.app/bestiary/Basarios".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Berserk Raviente",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/lavieG_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Berserk_Raviente",
        "https://wycademy.vercel.app/bestiary/Berserk_Raviente".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Berukyurosu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/beru_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Berukyurosu",
        "https://wycademy.vercel.app/bestiary/Berukyurosu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Black Diablos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/deakuro_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Black_Diablos",
        "https://wycademy.vercel.app/bestiary/Black_Diablos".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Black Gravios",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gurakuro_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Black_Gravios",
        "https://wycademy.vercel.app/bestiary/Black_Gravios".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Blango",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/bura_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Blango",
        "https://wycademy.vercel.app/bestiary/Blango".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Blangonga",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dodobura_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Blangonga",
        "https://wycademy.vercel.app/bestiary/Blangonga".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Blue Yian Kut-Ku",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kukkuao_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Blue_Yian_Kut-Ku",
        "https://wycademy.vercel.app/bestiary/Blue_Yian_Kut-Ku".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Bogabadorumu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/boga_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Bogabadorumu",
        "https://wycademy.vercel.app/bestiary/Bogabadorumu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Brachydios",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/buraki_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Brachydios",
        "https://wycademy.vercel.app/bestiary/Brachydios".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Bright Hypnoc",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hipuao_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Breeding_Season_Hypnocatrice",
        "https://wycademy.vercel.app/bestiary/bright-hypnoc".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Bulldrome",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosburu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Bulldrome",
        "https://wycademy.vercel.app/bestiary/Bulldrome".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Bullfango",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/buru_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Bullfango",
        "https://wycademy.vercel.app/bestiary/Bullfango".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Burukku",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/brook_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Burukku",
        "https://wycademy.vercel.app/bestiary/Burukku".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Ceanataur",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/yao_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Ceanataur",
        "https://wycademy.vercel.app/bestiary/Ceanataur".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Cephadrome",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosgare_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Cephadrome",
        "https://wycademy.vercel.app/bestiary/Cephadrome".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Cephalos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gare_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Cephalos",
        "https://wycademy.vercel.app/bestiary/Cephalos".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Chameleos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/oo_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Chameleos",
        "https://wycademy.vercel.app/bestiary/Chameleos".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Conga",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/konga_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Conga",
        "https://wycademy.vercel.app/bestiary/Conga".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Congalala",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/babakonga_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Congalala",
        "https://wycademy.vercel.app/bestiary/Congalala".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Crimson Fatalis",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/miraval_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Crimson_Fatalis",
        "https://wycademy.vercel.app/bestiary/Crimson_Fatalis".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Daimyo Hermitaur",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/daimyo_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Daimyo_Hermitaur",
        "https://wycademy.vercel.app/bestiary/Daimyo_Hermitaur".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Deviljho",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/joe_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Deviljho",
        "https://wycademy.vercel.app/bestiary/Deviljho".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Diablos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dea_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Diablos",
        "https://wycademy.vercel.app/bestiary/Diablos".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Diorex",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dio_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Diorekkusu",
        "https://wycademy.vercel.app/bestiary/Diorex".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Disufiroa",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/disf_n.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", "https://www.youtube.com/embed/leNWg4HoAbQ" },
                { "Dual Swords", "https://www.youtube.com/embed/obcm9-ebei8" },
                { "Great Sword", "https://www.youtube.com/embed/obcm9-ebei8" },
                { "Long Sword", "https://www.youtube.com/embed/3dJR6YqTZcM" },
                { "Hammer", string.Empty },
                { "Hunting Horn", string.Empty },
                { "Lance", "https://www.youtube.com/embed/TTj9T6Tskcg" },
                { "Gunlance", "https://www.youtube.com/embed/wgR1Bf-kApo" },
                { "Tonfa", "https://www.youtube.com/embed/1sjynMO0CJk" },
                { "Switch Axe F", string.Empty },
                { "Magnet Spike", "https://www.youtube.com/embed/FYS_yi7EQmA" },
                { "Light Bowgun", "https://www.youtube.com/embed/TPR4nYlWFgY" },
                { "Heavy Bowgun", "https://www.youtube.com/embed/MHf2R504_xc" },
                { "Bow", string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Disufiroa",
        "https://wycademy.vercel.app/bestiary/Disufiroa".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Doragyurosu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/doragyu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Doragyurosu",
        "https://wycademy.vercel.app/bestiary/Doragyurosu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Dyuragaua",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dura_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Dyuragaua",
        "https://wycademy.vercel.app/bestiary/Dyuragaua".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Egyurasu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/egura_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Egyurasu",
        "https://wycademy.vercel.app/bestiary".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Elzelion",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/elze_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Eruzerion",
        "https://wycademy.vercel.app/bestiary/Elzelion".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Erupe",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/erupe_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Erupe",
        "https://wycademy.vercel.app/bestiary/Erupe".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Espinas",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/esp_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Espinas",
        "https://wycademy.vercel.app/bestiary/Espinas".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Farunokku",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/faru_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Farunokku",
        "https://wycademy.vercel.app/bestiary/Farunokku".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Fatalis",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mira_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Fatalis",
        "https://wycademy.vercel.app/bestiary/Fatalis".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Felyne",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/airu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Felyne",
        "https://wycademy.vercel.app/bestiary/Felyne".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Forokururu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/folo_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Forokururu",
        "https://wycademy.vercel.app/bestiary/Forokururu".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Garuba Daora",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/galba_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Garuba_Daora",
        "https://wycademy.vercel.app/bestiary/Garuba_Daora".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Gasurabazura",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gasra_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gasurabazura",
        "https://wycademy.vercel.app/bestiary/Gasurabazura".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Gendrome",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosgene_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gendrome",
        "https://wycademy.vercel.app/bestiary/Gendrome".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Genprey",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gene_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Genprey",
        "https://wycademy.vercel.app/bestiary/Genprey".Replace("_","-").ToLowerInvariant()),

        new MonsterInfo(
            "Giaorugu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/giao_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Giaorugu",
        "https://wycademy.vercel.app/bestiary/Giaorugu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Giaprey",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/giano_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Giaprey",
        "https://wycademy.vercel.app/bestiary/Giaprey".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Gogomoa",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gogo_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gogomoa",
        "https://wycademy.vercel.app/bestiary/Gogomoa".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Gold Rathian",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reiakin_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gold_Rathian",
        "https://wycademy.vercel.app/bestiary/Gold_Rathian".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Gore Magala",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/goa_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gore_Magala",
        "https://wycademy.vercel.app/bestiary/Gore_Magala".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Goruganosu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/volkin_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Goruganosu",
        "https://wycademy.vercel.app/bestiary/Goruganosu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Gougarf, Ray",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/goglf_r_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Ray_Gougarf",
        "https://wycademy.vercel.app/bestiary/Gougarf".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Gougarf, Lolo",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/goglf_l_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Lolo_Gougarf",
        "https://wycademy.vercel.app/bestiary/Gougarf".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Gravios",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gura_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gravios",
        "https://wycademy.vercel.app/bestiary/Gravios".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Great Thunderbug",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/raikou_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Great_Thunderbug",
        "https://wycademy.vercel.app/bestiary/Great_Thunderbug".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Green Plesioth",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ganomidori_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Green_Plesioth",
        "https://wycademy.vercel.app/bestiary/Green_Plesioth".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Guanzorumu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/guan_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Guanzorumu",
        "https://wycademy.vercel.app/bestiary/Guanzorumu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Gureadomosu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/glare_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gureadomosu",
        "https://wycademy.vercel.app/bestiary/Gureadomosu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Gurenzeburu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/guren_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gurenzeburu",
        "https://wycademy.vercel.app/bestiary/Gurenzeburu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Gypceros",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/geryo_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gypceros",
        "https://wycademy.vercel.app/bestiary/Gypceros".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Harudomerugu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hald_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Harudomerugu",
        "https://wycademy.vercel.app/bestiary/Harudomerugu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Hermitaur",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gami_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Hermitaur",
        "https://wycademy.vercel.app/bestiary/Hermitaur".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Hornetaur",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kanta_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Hornetaur",
        "https://wycademy.vercel.app/bestiary/Hornetaur".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Hypnoc",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hipu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Hypnocatrice",
        "https://wycademy.vercel.app/bestiary/Hypnocatrice".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Hyujikiki",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hyuji_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Hyujikiki",
        "https://wycademy.vercel.app/bestiary/Hyujikiki".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Inagami",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ina_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Inagami",
        "https://wycademy.vercel.app/bestiary/Inagami".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Iodrome",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosios_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Iodrome",
        "https://wycademy.vercel.app/bestiary/Iodrome".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Ioprey",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ios_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Ioprey",
        "https://wycademy.vercel.app/bestiary/Ioprey".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Kamu Orugaron",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/olgk_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Kamu_Orugaron",
        "https://wycademy.vercel.app/bestiary/Kamu_Orugaron".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Kelbi",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kerubi_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Kelbi",
        "https://wycademy.vercel.app/bestiary/Kelbi".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Keoaruboru",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/keoa_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Keoaruboru",
        "https://wycademy.vercel.app/bestiary/Keoaruboru".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Khezu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/furu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Khezu",
        "https://wycademy.vercel.app/bestiary/Khezu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "King Shakalaka",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/cyacyaKing_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/King_Shakalaka",
        "https://wycademy.vercel.app/bestiary/King_Shakalaka".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Kirin",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kirin_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Kirin",
        "https://wycademy.vercel.app/bestiary/Kirin".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Kokomoa",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/koko_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Gogomoa",
        "https://wycademy.vercel.app/bestiary/Gogomoa".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Kuarusepusu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/qual_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Kuarusepusu",
        "https://wycademy.vercel.app/bestiary/Kuarusepusu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Kushala Daora",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kusha_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Kushala_Daora",
        "https://wycademy.vercel.app/bestiary/Kushala_Daora".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Kusubami",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kusb_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Kusubami",
        "https://wycademy.vercel.app/bestiary/Kusubami".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Lao-Shan Lung",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/rao_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Lao-Shan_Lung",
        "https://wycademy.vercel.app/bestiary/Lao-Shan_Lung".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Lavasioth",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/vol_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Lavasioth",
        "https://wycademy.vercel.app/bestiary/Lavasioth".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Lunastra",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/nana_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Lunastra",
        "https://wycademy.vercel.app/bestiary/Lunastra".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Melynx",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/merura_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Melynx",
        "https://wycademy.vercel.app/bestiary/Melynx".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Meraginasu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mera_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Meraginasu",
        "https://wycademy.vercel.app/bestiary/Meraginasu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Mi Ru",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mi-ru_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Mi_Ru",
        "https://wycademy.vercel.app/bestiary/Mi_Ru".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Midogaron",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mido_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Midogaron",
        "https://wycademy.vercel.app/bestiary/Midogaron".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Monoblos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mono_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Monoblos",
        "https://wycademy.vercel.app/bestiary/Monoblos".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Mosswine",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/mos_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Mosswine",
        "https://wycademy.vercel.app/bestiary/Mosswine".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Nargacuga",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/nalga_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Nargacuga",
        "https://wycademy.vercel.app/bestiary/Nargacuga".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Nono Orugaron",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/olgn_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Nono_Orugaron",
        "https://wycademy.vercel.app/bestiary/Nono_Orugaron".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Odibatorasu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/odiva_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Odibatorasu",
        "https://wycademy.vercel.app/bestiary/Odibatorasu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Orange Espinas",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/espcya_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Flaming_Espinas",
        "https://wycademy.vercel.app/bestiary/Orange_Espinas".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Pariapuria",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/paria_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Pariapuria",
        "https://wycademy.vercel.app/bestiary/Pariapuria".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Pink Rathian",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reiasa_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Pink_Rathian",
        "https://wycademy.vercel.app/bestiary/Pink_Rathian".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Plesioth",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gano_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Plesioth",
        "https://wycademy.vercel.app/bestiary/Plesioth".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Poborubarumu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/pobol_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Poborubarumu",
        "https://wycademy.vercel.app/bestiary/Poborubarumu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Pokara",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/pokara_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Pokara",
        "https://wycademy.vercel.app/bestiary/Pokara".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Pokaradon",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/pokaradon_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Pokaradon",
        "https://wycademy.vercel.app/bestiary/Pokaradon".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Popo",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/popo_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Popo",
        "https://wycademy.vercel.app/bestiary/Popo".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "PSO2 Rappy",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/lappy_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            RickRoll, "https://wycademy.vercel.app/bestiary"),
        new MonsterInfo(
            "Purple Gypceros",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/geryoao_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Purple_Gypceros",
        "https://wycademy.vercel.app/bestiary/Purple_Gypceros".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Rajang",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ra_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Rajang",
        "https://wycademy.vercel.app/bestiary/Rajang".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Rathalos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reusu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Rathalos",
        "https://wycademy.vercel.app/bestiary/Rathalos".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Rathian",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reia_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Rathian",
        "https://wycademy.vercel.app/bestiary/Rathian".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Raviente",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/lavie_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Raviente",
        "https://wycademy.vercel.app/bestiary/Raviente".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Rebidiora",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/levy_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Rebidiora",
        "https://wycademy.vercel.app/bestiary/Rebidiora".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Red Khezu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/furuaka_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Red_Khezu",
        "https://wycademy.vercel.app/bestiary/Red_Khezu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Red Lavasioth",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/volaka_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Red_Volganos",
        "https://wycademy.vercel.app/bestiary/Red_Volganos".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Remobra",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/gabu_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Remobra",
        "https://wycademy.vercel.app/bestiary/Remobra".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Rocks",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/iwa_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            RickRoll,"https://wycademy.vercel.app/bestiary"),
        new MonsterInfo(
            "Rukodiora",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ruco_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Rukodiora",
        "https://wycademy.vercel.app/bestiary/Rukodiora".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Rusted Kushala Daora",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kushasabi_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Rusted_Kushala_Daora",
        "https://wycademy.vercel.app/bestiary/Rusted_Kushala_Daora".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Seregios",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/sell_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Seregios",
        "https://wycademy.vercel.app/bestiary/Seregios".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Shagaru Magala",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/shagal_nh.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Shagaru_Magala",
        "https://wycademy.vercel.app/bestiary/Shagaru_Magala".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Shakalaka",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/cyacya_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Shakalaka",
        "https://wycademy.vercel.app/bestiary/Shakalaka".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Shantien",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/shan_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Shantien",
        "https://wycademy.vercel.app/bestiary/Shantien".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Shen Gaoren",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/shen_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Rathian",
        "https://wycademy.vercel.app/bestiary/Rathian".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Shogun Ceanataur",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/syougun_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Shogun_Ceanataur",
        "https://wycademy.vercel.app/bestiary/Shogun_Ceanataur".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Silver Hypnoc",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/hipusiro_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Silver_Hypnocatrice",
        "https://wycademy.vercel.app/bestiary/Silver_Hypnoc".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Silver Rathalos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/reusugin_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Silver_Rathalos",
        "https://wycademy.vercel.app/bestiary/Silver_Rathalos".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Stygian Zinogre",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zingoku_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Stygian_Zinogre",
        "https://wycademy.vercel.app/bestiary/Stygian_Zinogre".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Taikun Zamuza",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/taikun_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Taikun_Zamuza",
        "https://wycademy.vercel.app/bestiary/Taikun_Zamuza".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Teostra",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/teo_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Teostra",
        "https://wycademy.vercel.app/bestiary/Teostra".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Tigrex",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/tiga_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Tigrex",
        "https://wycademy.vercel.app/bestiary/Tigrex".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Toa Tesukatora",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/toa_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Toa_Tesukatora",
        "https://wycademy.vercel.app/bestiary/Toa_Tesukatora".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Toridcless",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/torid_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Toridcless",
        "https://wycademy.vercel.app/bestiary/Toridcless".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "UNKNOWN",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ra-ro_n.htm",
            new Dictionary<string, string>()
            {
                { "Sword and Shield", "https://www.youtube.com/embed/REF3OBNu4Wo" },
                { "Dual Swords", "https://www.youtube.com/embed/fAremzgKfos" },
                { "Great Sword", "https://www.youtube.com/embed/xX7KH0r68f4" },
                { "Long Sword", "https://www.youtube.com/embed/6G8865fllSo" },
                { "Hammer", "https://www.youtube.com/embed/A6pqZgrA-9o" },
                { "Hunting Horn", "https://www.youtube.com/embed/-qMOOTOOnrw" },
                { "Lance", "https://www.youtube.com/embed/m66unQSZzIc" },
                { "Gunlance", "https://www.youtube.com/embed/yCz_sigKiGs" },
                { "Tonfa", "https://www.youtube.com/embed/sj_jMp0T3Pc" },
                { "Switch Axe F", "https://www.youtube.com/embed/hZvxtDsqSf4" },
                { "Magnet Spike", "https://www.youtube.com/embed/FLmxy-xCoqM" },
                { "Light Bowgun", "https://www.youtube.com/embed/xTvTlPkIp3w" },
                { "Heavy Bowgun", string.Empty },
                { "Bow", "https://www.youtube.com/embed/gp48Gk-y_JY" },
            },
            "https://monsterhunter.fandom.com/wiki/Unknown_(Black_Flying_Wyvern)",
        "https://wycademy.vercel.app/bestiary/Unknown_(Black_Flying_Wyvern)".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Uragaan",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ura_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Uragaan",
        "https://wycademy.vercel.app/bestiary/Uragaan".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Uruki",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ulky_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Uruki",
        "https://wycademy.vercel.app/bestiary/Uruki".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Varusaburosu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/valsa_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Varusaburosu",
        "https://wycademy.vercel.app/bestiary/Varusaburosu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Velocidrome",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/dosran_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Velocidrome",
        "https://wycademy.vercel.app/bestiary/Velocidrome".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Velociprey",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/ran_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Velociprey",
        "https://wycademy.vercel.app/bestiary/Velociprey".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Vespoid",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/rango_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Vespoid",
        "https://wycademy.vercel.app/bestiary/Vespoid".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Voljang",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/Vau_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Voljang",
        "https://wycademy.vercel.app/bestiary/Voljang".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "White Espinas",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/espsiro_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Espinas_Rare_Species",
        "https://wycademy.vercel.app/bestiary/Espinas_Rare_Species".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "White Fatalis",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/miraru_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/White_Fatalis",
        "https://wycademy.vercel.app/bestiary/White_Fatalis".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "White Monoblos",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/monosiro_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/White_Monoblos",
        "https://wycademy.vercel.app/bestiary/White_Monoblos".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Yama Kurai",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/yamac_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Yama_Kurai",
        "https://wycademy.vercel.app/bestiary/Yama_Kurai".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Yama Tsukami",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/yama_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Yama_Tsukami",
        "https://wycademy.vercel.app/bestiary/Yama_Tsukami".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Yian Garuga",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/garuru_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Yian_Garuga",
        "https://wycademy.vercel.app/bestiary/Yian_Garuga".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Yian Kut-Ku",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/kukku_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Yian_Kut-Ku",
        "https://wycademy.vercel.app/bestiary/Yian_Kut-Ku".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Zenaserisu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zena_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zenaserisu",
        "https://wycademy.vercel.app/bestiary/Zenaserisu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Zerureusu",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zeru_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zerureusu",
        "https://wycademy.vercel.app/bestiary/Zerureusu".Replace("_","-").ToLowerInvariant()),
        new MonsterInfo(
            "Zinogre",
            "https://dorielrivalet.github.io/mhfz-ferias-english-project/mons/zin_n.htm",
            new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
            },
            "https://monsterhunter.fandom.com/wiki/Zinogre",
        "https://wycademy.vercel.app/bestiary/Zinogre".Replace("_","-").ToLowerInvariant()),
    };
}
