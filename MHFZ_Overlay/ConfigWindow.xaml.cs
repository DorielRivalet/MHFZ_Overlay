using CsvHelper;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Window = System.Windows.Window;

namespace MHFZ_Overlay
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        /// <summary>
        /// Gets or sets the main window.
        /// </summary>
        /// <value>
        /// The main window.
        /// </value>
        private MainWindow MainWindow { get; set; }

        private static string randomMonsterImage = "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png";

        public static Uri MonsterInfoLink
        {
            get { return new Uri(randomMonsterImage, UriKind.RelativeOrAbsolute); }
        }

        public static readonly string RickRoll = "https://www.youtube.com/embed/dQw4w9WgXcQ";

        public static string getFeriasLink()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            return s.FeriasVersionLink;
        }

        private MonsterInfo[] monsterInfos = new MonsterInfo[]
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

        public static Uri MonsterImage
        {
            get { return new Uri(randomMonsterImage, UriKind.RelativeOrAbsolute); }
        }

        private MonsterLog[] Monsters = new MonsterLog[]
        {
          new MonsterLog(0, "None","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/none.png",0),
          new MonsterLog(1, "Rathian","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rathian.png",0,true),
          new MonsterLog(2, "Fatalis","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/fatalis.png",0,true),
          new MonsterLog(3, "Kelbi","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kelbi.png",0),
          new MonsterLog(4, "Mosswine","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/mosswine.png",0),
          new MonsterLog(5, "Bullfango","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/bullfango.png",0),
          new MonsterLog(6, "Yian Kut-Ku","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yian_kut-ku.png",0,true),
          new MonsterLog(7, "Lao-Shan Lung","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/lao-shan_lung.png",0,true),
          new MonsterLog(8, "Cephadrome","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/cephadrome.png",0,true),
          new MonsterLog(9, "Felyne","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/felyne.png",0),
          new MonsterLog(10, "Veggie Elder","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(11, "Rathalos","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rathalos.png",0,true),
          new MonsterLog(12, "Aptonoth","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/aptonoth.png",0),
          new MonsterLog(13, "Genprey","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/genprey.png",0),
          new MonsterLog(14, "Diablos","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/diablos.png",0,true),
          new MonsterLog(15, "Khezu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/khezu.png",0,true),
          new MonsterLog(16, "Velociprey","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/velociprey.png",0),
          new MonsterLog(17, "Gravios","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gravios.png",0,true),
          new MonsterLog(18, "Felyne?","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/felyne.png",0),
          new MonsterLog(19, "Vespoid","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/vespoid.png",0),
          new MonsterLog(20, "Gypceros","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gypceros.png",0,true),
          new MonsterLog(21, "Plesioth","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/plesioth.png",0,true),
          new MonsterLog(22, "Basarios","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/basarios.png",0,true),
          new MonsterLog(23, "Melynx","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/melynx.png",0),
          new MonsterLog(24, "Hornetaur","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/hornetaur.png",0),
          new MonsterLog(25, "Apceros","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/apceros.png",0),
          new MonsterLog(26, "Monoblos","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/monoblos.png",0,true),
          new MonsterLog(27, "Velocidrome","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/velocidrome.png",0,true),
          new MonsterLog(28, "Gendrome","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gendrome.png",0,true),
          new MonsterLog(29, "Rocks","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(30, "Ioprey","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/ioprey.png",0),
          new MonsterLog(31, "Iodrome","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/iodrome.png",0,true),
          new MonsterLog(32, "Pugis","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(33, "Kirin","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kirin.png",0,true),
          new MonsterLog(34, "Cephalos","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/cephalos.png",0),
          new MonsterLog(35, "Giaprey / Giadrome","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/giaprey.png",0),
          new MonsterLog(36, "Crimson Fatalis","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/crimson_fatalis.png",0,true),
          new MonsterLog(37, "Pink Rathian","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pink_rathian.png",0,true),
          new MonsterLog(38, "Blue Yian Kut-Ku","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blue_yian_kut-ku.png",0,true),
          new MonsterLog(39, "Purple Gypceros","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/purple_gypceros.png",0,true),
          new MonsterLog(40, "Yian Garuga","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yian_garuga.png",0,true),
          new MonsterLog(41, "Silver Rathalos","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/silver_rathalos.png",0,true),
          new MonsterLog(42, "Gold Rathian","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gold_rathian.png",0,true),
          new MonsterLog(43, "Black Diablos","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/black_diablos.png",0,true),
          new MonsterLog(44, "White Monoblos","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/white_monoblos.png",0,true),
          new MonsterLog(45, "Red Khezu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/red_khezu.png",0,true),
          new MonsterLog(46, "Green Plesioth","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/green_plesioth.png",0,true),
          new MonsterLog(47, "Black Gravios","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/black_gravios.png",0,true),
          new MonsterLog(48, "Daimyo Hermitaur","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/daimyo_hermitaur.png",0,true),
          new MonsterLog(49, "Azure Rathalos","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/azure_rathalos.png",0,true),
          new MonsterLog(50, "Ashen Lao-Shan Lung","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/ashen_lao-shan_lung.png",0,true),
          new MonsterLog(51, "Blangonga","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blangonga.png",0,true),
          new MonsterLog(52, "Congalala","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/congalala.png",0,true),
          new MonsterLog(53, "Rajang","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rajang.png",0,true),
          new MonsterLog(54, "Kushala Daora","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kushala_daora.png",0,true),
          new MonsterLog(55, "Shen Gaoren","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shen_gaoren.png",0,true),
          new MonsterLog(56, "Great Thunderbug","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/great_thunderbug.png",0),
          new MonsterLog(57, "Shakalaka","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shakalaka.png",0),
          new MonsterLog(58, "Yama Tsukami","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yama_tsukami.png",0,true),
          new MonsterLog(59, "Chameleos","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/chameleos.png",0,true),
          new MonsterLog(60, "Rusted Kushala Daora","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rusted_kushala_daora.png",0,true),
          new MonsterLog(61, "Blango","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blango.png",0),
          new MonsterLog(62, "Conga","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/conga.png",0),
          new MonsterLog(63, "Remobra","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/remobra.png",0),
          new MonsterLog(64, "Lunastra","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/lunastra.png",0,true),
          new MonsterLog(65, "Teostra","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/teostra.png",0,true),
          new MonsterLog(66, "Hermitaur","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/hermitaur.png",0),
          new MonsterLog(67, "Shogun Ceanataur","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shogun_ceanataur.png",0,true),
          new MonsterLog(68, "Bulldrome","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/bulldrome.png",0,true),
          new MonsterLog(69, "Anteka","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/anteka.png",0),
          new MonsterLog(70, "Popo","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/popo.png",0),
          new MonsterLog(71, "White Fatalis","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/white_fatalis.png",0,true),
          new MonsterLog(72, "Yama Tsukami","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yama_tsukami.png",0,true),
          new MonsterLog(73, "Ceanataur","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/ceanataur.png",0),
          new MonsterLog(74, "Hypnocatrice","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/hypnocatrice.png",0,true),
          new MonsterLog(75, "Lavasioth","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/lavasioth.png",0,true),
          new MonsterLog(76, "Tigrex","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/tigrex.png",0,true),
          new MonsterLog(77, "Akantor","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/akantor.png",0,true),
          new MonsterLog(78, "Bright Hypnoc","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/bright_hypnoc.png",0,true),
          new MonsterLog(79, "Red Lavasioth","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/red_lavasioth.png",0,true),
          new MonsterLog(80, "Espinas","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/espinas.png",0,true),
          new MonsterLog(81, "Orange Espinas","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/orange_espinas.png",0,true),
          new MonsterLog(82, "Silver Hypnoc","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/silver_hypnoc.png",0,true),
          new MonsterLog(83, "Akura Vashimu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/akura_vashimu.png",0,true),
          new MonsterLog(84, "Akura Jebia","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/akura_jebia.png",0,true),
          new MonsterLog(85, "Berukyurosu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/berukyurosu.png",0,true),
          new MonsterLog(86, "Cactus","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/cactus.png",0),
          new MonsterLog(87, "Gorge Objects","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(88, "Gorge Rocks","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(89, "Pariapuria","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pariapuria.png",0,true),
          new MonsterLog(90, "White Espinas","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/white_espinas.png",0,true),
          new MonsterLog(91, "Kamu Orugaron","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kamu_orugaron.png",0,true),
          new MonsterLog(92, "Nono Orugaron","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/nono_orugaron.png",0,true),
          new MonsterLog(93, "Raviente","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/raviente.png",0,true),
          new MonsterLog(94, "Dyuragaua","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/dyuragaua.png",0,true),
          new MonsterLog(95, "Doragyurosu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/doragyurosu.png",0,true),
          new MonsterLog(96, "Gurenzeburu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gurenzeburu.png",0,true),
          new MonsterLog(97, "Burukku","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/burukku.png",0),
          new MonsterLog(98, "Erupe","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/erupe.png",0),
          new MonsterLog(99, "Rukodiora","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rukodiora.png",0,true),
          new MonsterLog(100, "UNKNOWN","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/unknown.png",0,true),
          new MonsterLog(101, "Gogomoa","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gogomoa.png",0,true),
          new MonsterLog(102, "Kokomoa","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gogomoa.png",0),
          new MonsterLog(103, "Taikun Zamuza","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/taikun_zamuza.png",0,true),
          new MonsterLog(104, "Abiorugu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/abiorugu.png",0,true),
          new MonsterLog(105, "Kuarusepusu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kuarusepusu.png",0,true),
          new MonsterLog(106, "Odibatorasu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/odibatorasu.png",0,true),
          new MonsterLog(107, "Disufiroa","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/disufiroa.png",0,true),
          new MonsterLog(108, "Rebidiora","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rebidiora.png",0,true),
          new MonsterLog(109, "Anorupatisu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/anorupatisu.png",0,true),
          new MonsterLog(110, "Hyujikiki","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/hyujikiki.png",0,true),
          new MonsterLog(111, "Midogaron","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/midogaron.png",0,true),
          new MonsterLog(112, "Giaorugu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/giaorugu.png",0,true),
          new MonsterLog(113, "Mi Ru","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/mi_ru.png",0,true),
          new MonsterLog(114, "Farunokku","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/farunokku.png",0,true),
          new MonsterLog(115, "Pokaradon","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pokaradon.png",0,true),
          new MonsterLog(116, "Shantien","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shantien.png",0,true),
          new MonsterLog(117, "Pokara","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pokara.png",0),
          new MonsterLog(118, "Dummy","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(119, "Goruganosu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/goruganosu.png",0,true),
          new MonsterLog(120, "Aruganosu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/aruganosu.png",0,true),
          new MonsterLog(121, "Baruragaru","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/baruragaru.png",0,true),
          new MonsterLog(122, "Zerureusu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/zerureusu.png",0,true),
          new MonsterLog(123, "Gougarf","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gougarf.png",0,true),
          new MonsterLog(124, "Uruki","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/uruki.png",0),
          new MonsterLog(125, "Forokururu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/forokururu.png",0,true),
          new MonsterLog(126, "Meraginasu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/meraginasu.png",0,true),
          new MonsterLog(127, "Diorex","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/diorex.png",0,true),
          new MonsterLog(128, "Garuba Daora","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/garuba_daora.png",0,true),
          new MonsterLog(129, "Inagami","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/inagami.png",0,true),
          new MonsterLog(130, "Varusaburosu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/varusaburosu.png",0,true),
          new MonsterLog(131, "Poborubarumu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/poborubarumu.png",0,true),
          new MonsterLog(132, "1st District Duremudira","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/duremudira.png",0,true),
          new MonsterLog(133, "UNK","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(134, "Felyne","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/felyne.png",0),
          new MonsterLog(135, "Blue NPC","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(136, "UNK","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(137, "Cactus","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/cactus.png",0),
          new MonsterLog(138, "Veggie Elders","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(139, "Gureadomosu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gureadomosu.png",0,true),
          new MonsterLog(140, "Harudomerugu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/harudomerugu.png",0,true),
          new MonsterLog(141, "Toridcless","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/toridcless.png",0,true),
          new MonsterLog(142, "Gasurabazura","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gasurabazura.png",0,true),
          new MonsterLog(143, "Kusubami","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kusubami.png",0),
          new MonsterLog(144, "Yama Kurai","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yama_kurai.png",0,true),
          new MonsterLog(145, "2nd District Duremudira","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/duremudira.png",0,true),
          new MonsterLog(146, "Zinogre","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/zinogre.png",0,true),
          new MonsterLog(147, "Deviljho","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/deviljho.png",0,true),
          new MonsterLog(148, "Brachydios","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/brachydios.png",0,true),
          new MonsterLog(149, "Berserk Raviente","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/berserk_raviente.png",0,true),
          new MonsterLog(150, "Toa Tesukatora","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/toa_tesukatora.png",0,true),
          new MonsterLog(151, "Barioth","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/barioth.png",0,true),
          new MonsterLog(152, "Uragaan","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/uragaan.png",0,true),
          new MonsterLog(153, "Stygian Zinogre","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/stygian_zinogre.png",0,true),
          new MonsterLog(154, "Guanzorumu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/guanzorumu.png",0,true),
          new MonsterLog(155, "Starving Deviljho","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/starving_deviljho.png",0,true),
          new MonsterLog(156, "UNK","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(157, "Egyurasu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(158, "Voljang","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/voljang.png",0,true),
          new MonsterLog(159, "Nargacuga","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/nargacuga.png",0,true),
          new MonsterLog(160, "Keoaruboru","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/keoaruboru.png",0,true),
          new MonsterLog(161, "Zenaserisu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/zenaserisu.png",0,true),
          new MonsterLog(162, "Gore Magala","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gore_magala.png",0,true),
          new MonsterLog(163, "Blinking Nargacuga","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blinking_nargacuga.png",0,true),
          new MonsterLog(164, "Shagaru Magala","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shagaru_magala.png",0,true),
          new MonsterLog(165, "Amatsu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/amatsu.png",0,true),
          new MonsterLog(166, "Elzelion","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/elzelion.png",0,true),
          new MonsterLog(167, "Arrogant Duremudira","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/arrogant_duremudira.png",0,true),
          new MonsterLog(168, "Rocks","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(169, "Seregios","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/seregios.png",0,true),
          new MonsterLog(170, "Bogabadorumu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/bogabadorumu.png",0,true),
          new MonsterLog(171, "Unknown Blue Barrel","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png",0),
          new MonsterLog(172, "Blitzkrieg Bogabadorumu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blitzkrieg_bogabadorumu.png",0,true),
          new MonsterLog(173, "Costumed Uruki","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/uruki.png",0),
          new MonsterLog(174, "Sparkling Zerureusu","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/sparkling_zerureusu.png",0,true),
          new MonsterLog(175, "PSO2 Rappy","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pso2_rappy.png",0),
          new MonsterLog(176, "King Shakalaka","https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/king_shakalaka.png",0,true)
        };

        public static string ReplaceMonsterInfoFeriasVersion(string link)
        {
            string ReplaceSettingsLink = getFeriasLink();

            // Check if no need to replace because its the same version already
            if (link.Contains(ReplaceSettingsLink))
                return link;

            string separator = "mons/";
            string info = link.Split(separator)[1];

            return string.Format("{0}{1}{2}", ReplaceSettingsLink, separator, info);
        }

        public int GetHuntedCount(int id)
        {
            var dl = MainWindow.DataLoader;

            switch (id)
            {
                default:
                    return 0;
                case 0:
                    return 0;
                case 1:
                    return dl.model.RathianHunted();
                case 2:
                    return dl.model.FatalisHunted();
                case 3:
                    return dl.model.KelbiHunted();
                case 4:
                    return dl.model.MosswineHunted();
                case 5:
                    return dl.model.BullfangoHunted();
                case 6:
                    return dl.model.YianKutKuHunted();
                case 7:
                    return dl.model.LaoShanLungHunted();
                case 8:
                    return dl.model.CephadromeHunted();
                case 9: return dl.model.FelyneHunted();
                case 10: return 0;
                case 11: return dl.model.RathalosHunted();
                case 12: return dl.model.AptonothHunted();
                case 13: return dl.model.GenpreyHunted();
                case 14: return dl.model.DiablosHunted();
                case 15: return dl.model.KhezuHunted();
                case 16: return dl.model.VelocipreyHunted();
                case 17: return dl.model.GraviosHunted();
                case 18: return 0;
                case 19: return dl.model.VespoidHunted();
                case 20: return dl.model.GypcerosHunted();
                case 21: return dl.model.PlesiothHunted();
                case 22: return dl.model.BasariosHunted();
                case 23: return dl.model.MelynxHunted();
                case 24: return dl.model.HornetaurHunted();
                case 25: return dl.model.ApcerosHunted();
                case 26: return dl.model.MonoblosHunted();
                case 27: return dl.model.VelocidromeHunted();
                case 28: return dl.model.GendromeHunted();
                case 29: return dl.model.RocksHunted();
                case 30: return dl.model.IopreyHunted();
                case 31: return dl.model.IodromeHunted();
                case 32: return 0;
                case 33: return dl.model.KirinHunted();
                case 34: return dl.model.CephalosHunted();
                case 35: return dl.model.GiapreyHunted();
                case 36: return dl.model.CrimsonFatalisHunted();
                case 37: return dl.model.PinkRathianHunted();
                case 38: return dl.model.BlueYianKutKuHunted();
                case 39: return dl.model.PurpleGypcerosHunted();
                case 40: return dl.model.YianGarugaHunted();
                case 41: return dl.model.SilverRathalosHunted();
                case 42: return dl.model.GoldRathianHunted();
                case 43: return dl.model.BlackDiablosHunted();
                case 44: return dl.model.WhiteMonoblosHunted();
                case 45: return dl.model.RedKhezuHunted();
                case 46: return dl.model.GreenPlesiothHunted();
                case 47: return dl.model.BlackGraviosHunted();
                case 48: return dl.model.DaimyoHermitaurHunted();
                case 49: return dl.model.AzureRathalosHunted();
                case 50: return dl.model.AshenLaoShanLungHunted();
                case 51: return dl.model.BlangongaHunted();
                case 52: return dl.model.CongalalaHunted();
                case 53: return dl.model.RajangHunted();
                case 54: return dl.model.KushalaDaoraHunted();
                case 55: return dl.model.ShenGaorenHunted();
                case 56: return dl.model.GreatThunderbugHunted();
                case 57: return dl.model.ShakalakaHunted();
                case 58: return dl.model.YamaTsukamiHunted();
                case 59: return dl.model.ChameleosHunted();
                case 60: return dl.model.RustedKushalaDaoraHunted();
                case 61: return dl.model.BlangoHunted();
                case 62: return dl.model.CongaHunted();
                case 63: return dl.model.RemobraHunted();
                case 64: return dl.model.LunastraHunted();
                case 65: return dl.model.TeostraHunted();
                case 66: return dl.model.HermitaurHunted();
                case 67: return dl.model.ShogunCeanataurHunted();
                case 68: return dl.model.BulldromeHunted();
                case 69: return dl.model.AntekaHunted();
                case 70: return dl.model.PopoHunted();
                case 71: return dl.model.WhiteFatalisHunted();
                case 72: return dl.model.YamaTsukamiHunted();
                case 73: return dl.model.CeanataurHunted();
                case 74: return dl.model.HypnocHunted();
                case 75: return dl.model.VolganosHunted();
                case 76: return dl.model.TigrexHunted();
                case 77: return dl.model.AkantorHunted();
                case 78: return dl.model.BrightHypnocHunted();
                case 79: return dl.model.RedVolganosHunted();
                case 80: return dl.model.EspinasHunted();
                case 81: return dl.model.OrangeEspinasHunted();
                case 82: return dl.model.SilverHypnocHunted();
                case 83: return dl.model.AkuraVashimuHunted();
                case 84: return dl.model.AkuraJebiaHunted();
                case 85: return dl.model.BerukyurosuHunted();
                case 86: return dl.model.CactusHunted();
                case 87: return dl.model.GorgeObjectsHunted();
                case 88: return 0;
                case 89: return dl.model.PariapuriaHunted();
                case 90: return dl.model.WhiteEspinasHunted();
                case 91: return dl.model.KamuOrugaronHunted();
                case 92: return dl.model.NonoOrugaronHunted();
                case 93: return 0;
                case 94: return dl.model.DyuragauaHunted();
                case 95: return dl.model.DoragyurosuHunted();
                case 96: return dl.model.GurenzeburuHunted();
                case 97: return dl.model.BurukkuHunted();
                case 98: return dl.model.ErupeHunted();
                case 99: return dl.model.RukodioraHunted();
                case 100: return dl.model.UnknownHunted();
                case 101: return dl.model.GogomoaHunted();
                case 102: return 0;
                case 103: return dl.model.TaikunZamuzaHunted();
                case 104: return dl.model.AbioruguHunted();
                case 105: return dl.model.KuarusepusuHunted();
                case 106: return dl.model.OdibatorasuHunted();
                case 107: return dl.model.DisufiroaHunted();
                case 108: return dl.model.RebidioraHunted();
                case 109: return dl.model.AnorupatisuHunted();
                case 110: return dl.model.HyujikikiHunted();
                case 111: return dl.model.MidogaronHunted();
                case 112: return dl.model.GiaoruguHunted();
                case 113: return dl.model.MiRuHunted();
                case 114: return dl.model.FarunokkuHunted();
                case 115: return dl.model.PokaradonHunted();
                case 116: return dl.model.ShantienHunted();
                case 117: return dl.model.PokaraHunted();
                case 118: return 0;
                case 119: return dl.model.GoruganosuHunted();
                case 120: return dl.model.AruganosuHunted();
                case 121: return dl.model.BaruragaruHunted();
                case 122: return dl.model.ZerureusuHunted();
                case 123: return dl.model.GougarfHunted();
                case 124: return dl.model.UrukiHunted();
                case 125: return dl.model.ForokururuHunted();
                case 126: return dl.model.MeraginasuHunted();
                case 127: return dl.model.DiorexHunted();
                case 128: return dl.model.GarubaDaoraHunted();
                case 129: return dl.model.InagamiHunted();
                case 130: return dl.model.VarusaburosuHunted();
                case 131: return dl.model.PoborubarumuHunted();
                case 132: return dl.model.FirstDistrictDuremudiraSlays();
                case 133: return 0;
                case 134: return 0;
                case 135: return 0;
                case 136: return 0;
                case 137: return dl.model.CactusHunted();
                case 138: return 0;
                case 139: return dl.model.GureadomosuHunted();
                case 140: return dl.model.HarudomeruguHunted();
                case 141: return dl.model.ToridclessHunted();
                case 142: return dl.model.GasurabazuraHunted();
                case 143: return dl.model.KusubamiHunted();
                case 144: return dl.model.YamaKuraiHunted();
                case 145: return dl.model.SecondDistrictDuremudiraSlays();
                case 146: return dl.model.ZinogreHunted();
                case 147: return dl.model.DeviljhoHunted();
                case 148: return dl.model.BrachydiosHunted();
                case 149: return 0;
                case 150: return dl.model.ToaTesukatoraHunted();
                case 151: return dl.model.BariothHunted();
                case 152: return dl.model.UragaanHunted();
                case 153: return dl.model.StygianZinogreHunted();
                case 154: return dl.model.GuanzorumuHunted();
                case 155: return dl.model.StarvingDeviljhoHunted();
                case 156: return 0;
                case 157: return 0;
                case 158: return dl.model.VoljangHunted();
                case 159: return dl.model.NargacugaHunted();
                case 160: return dl.model.KeoaruboruHunted();
                case 161: return dl.model.ZenaserisuHunted();
                case 162: return dl.model.GoreMagalaHunted();
                case 163: return dl.model.BlinkingNargacugaHunted();
                case 164: return dl.model.ShagaruMagalaHunted();
                case 165: return dl.model.AmatsuHunted();
                case 166: return dl.model.ElzelionHunted();
                case 167: return dl.model.ArrogantDuremudiraHunted();
                case 168: return 0;
                case 169: return dl.model.SeregiosHunted();
                case 170: return dl.model.BogabadorumuHunted();
                case 171: return 0;
                case 172: return dl.model.BlitzkriegBogabadorumuHunted();
                case 173: return 0;
                case 174: return dl.model.SparklingZerureusuHunted();
                case 175: return dl.model.PSO2RappyHunted();
                case 176: return dl.model.KingShakalakaHunted();
            }
        }

        public void SortList()
        {
            var SortProperty = SortBy.SelectedItem.ToString();
            var SortDirection = SortDir.SelectedItem.ToString() == "Ascending" ? ListSortDirection.Ascending : ListSortDirection.Descending;
            MyList.Items.SortDescriptions[0] = new SortDescription(SortProperty, SortDirection);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigWindow"/> class.
        /// </summary>
        /// <param name="mainWindow">The main window.</param>
        public ConfigWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            Topmost = true;
            MainWindow = mainWindow;

            string background1 = @"pack://application:,,,/MHFZ_Overlay;component/UI/Background/1.png";
            string background2 = @"pack://application:,,,/MHFZ_Overlay;component/UI/Background/2.png";
            string background3 = @"pack://application:,,,/MHFZ_Overlay;component/UI/Background/3.png";
            string background4 = @"pack://application:,,,/MHFZ_Overlay;component/UI/Background/4.png";
            string background5 = @"pack://application:,,,/MHFZ_Overlay;component/UI/Background/5.png";
            string background6 = @"pack://application:,,,/MHFZ_Overlay;component/UI/Background/6.png";
            string background7 = @"pack://application:,,,/MHFZ_Overlay;component/UI/Background/7.png";

            //https://stackoverflow.com/questions/30839173/change-background-image-in-wpf-using-c-sharp
            GeneralContent.Background = new ImageBrush(new BitmapImage(new Uri(background1)));
            PlayerContent.Background = new ImageBrush(new BitmapImage(new Uri(background2)));
            MonsterHPContent.Background = new ImageBrush(new BitmapImage(new Uri(background3)));
            MonsterStatusContent.Background = new ImageBrush(new BitmapImage(new Uri(background4)));
            DiscordRPCContent.Background = new ImageBrush(new BitmapImage(new Uri(background5)));
            CreditsContent.Background = new ImageBrush(new BitmapImage(new Uri(background6)));
            MonsterInfoContent.Background = new ImageBrush(new BitmapImage(new Uri(background7)));

            //TODO: test this
            DataContext = MainWindow.DataLoader.model;

            for (int i = 0; i < Monsters.Length; i++)
            {
                Monsters[i].Hunted = GetHuntedCount(Monsters[i].ID);
            }

            MyList.ItemsSource = Monsters;
            SortBy.ItemsSource = new string[] { "ID", "Name", "Hunted" };
            SortDir.ItemsSource = Enum.GetNames<ListSortDirection>();

            SortBy.SelectionChanged += SelectionChanged;
            SortDir.SelectionChanged += SelectionChanged;

            MyList.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            FilterBox.ItemsSource = new string[] { "All", "Large Monster", "Small Monster" };

            MyList.Items.Filter = MonsterFilterAll;

            //// See: https://stackoverflow.com/questions/22285866/why-relaycommand
            //// Or use MVVM Light to obtain RelayCommand.

            List<string> MonsterNameList = new List<string>();

            for (int i = 0; i < monsterInfos.Length; i++)
            {
                MonsterNameList.Add(monsterInfos[i].Name);
            }

            MonsterNameListBox.ItemsSource = MonsterNameList;

            _ = GetRepoStats();

            replaceAllMonsterInfoFeriasLinks();
        }

        private void replaceAllMonsterInfoFeriasLinks()
        {
            for (int i = 0; i < monsterInfos.Length; i++)
            {
                monsterInfos[i].FeriasLink = ReplaceMonsterInfoFeriasVersion(monsterInfos[i].FeriasLink);
            }
        }

        private bool MonsterFilterAll(object obj)
        {
            var FilterObj = obj as MonsterLog;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return FilterObj.IsLarge || !FilterObj.IsLarge;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private bool MonsterFilterLarge(object obj)
        {
            var FilterObj = obj as MonsterLog;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return FilterObj.IsLarge;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private bool MonsterFilterSmall(object obj)
        {
            var FilterObj = obj as MonsterLog;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return !FilterObj.IsLarge;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }


        public Predicate<object> GetFilter()
        {
            switch (FilterBox.SelectedItem as string)
            {
                default:
                    return MonsterFilterAll;
                case "Large Monster":
                    return MonsterFilterLarge;
                case "Small Monster":
                    return MonsterFilterSmall;
            }
        }

        /// <summary>
        /// Handles the PreviewTextInput event of the RoadOverrideTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        private void RoadOverrideTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "0" && e.Text != "1" && e.Text != "2")
            {
                e.Handled = true;
            }

        }

        /// <summary>
        /// Saves the key press.
        /// </summary>
        public void SaveKey_Press()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Save();
            DisposeAllWebViews();
            Close();
        }

        private void DisposeAllWebViews()
        {
            webViewFerias.Dispose();
            webViewDamageCalculator.Dispose();
            webViewMonsterInfo.Dispose();
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Save();
            DisposeAllWebViews();
            Close();
        }

        /// <summary>
        /// Cancels the key press.
        /// </summary>
        public void CancelKey_Press()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Reload();
            DisposeAllWebViews();
            Close();
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            DisposeAllWebViews();
            s.Reload();
            Close();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.Closing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Reload();
            MainWindow.DataLoader.model.Configuring = false;
        }

        /// <summary>
        /// Defaults the key press.
        /// </summary>
        public void DefaultKey_Press()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            DisposeAllWebViews();
            s.Reset();
        }

        /// <summary>
        /// Handles the Click event of the DefaultButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            DisposeAllWebViews();
            s.Reset();
        }

        /// <summary>
        /// Handles the Click event of the ConfigureButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ConfigureButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.EnableDragAndDrop();
        }

        /// <summary>
        /// Validates the number.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        private void ValidateNumber(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
            {
                if (!char.IsNumber(ch))
                {
                    e.Handled = true;
                    return;
                }
            }
            if (e.Text.Length > 1 && e.Text[0] == '0')
                e.Handled = true;
        }

        //https://stackoverflow.com/questions/1051989/regex-for-alphanumeric-but-at-least-one-letter
        //^(?=.*[a-zA-Z].*)([a-zA-Z0-9]{6,12})$
        //([a-zA-Z0-9_\s]+)
        //[^a-zA-Z_0-9]

        /// <summary>
        /// Validates the name.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        private void ValidateName(object sender, TextCompositionEventArgs e)
        {
            // Create a Regex

            // Get all matches  
            //https://stackoverflow.com/questions/1046740/how-can-i-validate-a-string-to-only-allow-alphanumeric-characters-in-it
            if (!(e.Text.All(char.IsLetterOrDigit)))
            {
                //just letters and digits.
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the RequestNavigate event of the lnkImg control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Navigation.RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void lnkImg_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        /// <summary>
        /// Shows the text format mode
        /// </summary>
        /// <returns></returns>
        public static string GetTextFormatMode()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.TextFormatExport != null)
                return s.TextFormatExport;
            else
                return "None";
        }

        /// <summary>
        /// Handles the Click event of the btnSaveFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            string textToSave = GearStats.Text;


            if (GetTextFormatMode() == "Code Block")
                textToSave = string.Format("```text\n{0}\n```", textToSave);
            else if (GetTextFormatMode() == "Markdown")
                textToSave = MainWindow.DataLoader.model.MarkdownSavedGearStats;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Markdown file (*.md)|*.md|Text file (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory + @"USERDATA\HunterSets\";
            string dateTime = DateTime.Now.ToString();
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            saveFileDialog.FileName = "Set-" + dateTime;
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, textToSave);
            }

        }

        /// <summary>
        /// Copy to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCopyFile_Click(object sender, RoutedEventArgs e)
        {
            string textToSave = GearStats.Text;

            if (GetTextFormatMode() == "Code Block")
                textToSave = string.Format("```text\n{0}\n```", textToSave);
            else if (GetTextFormatMode() == "Markdown")
                textToSave = MainWindow.DataLoader.model.MarkdownSavedGearStats;
            else if (GetTextFormatMode() == "Image")
            {
                GearTextGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
                CopyUIElementToClipboard(GearTextGrid);
                GearTextGrid.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x1E, 0x1E, 0x2E));
                return;
            }

            //https://stackoverflow.com/questions/3546016/how-to-copy-data-to-clipboard-in-c-sharp
            Clipboard.SetText(textToSave);
        }

        private void BtnImageFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            string dateTime = DateTime.Now.ToString();
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            savefile.FileName = "HunterSet-" + dateTime + ".png";
            savefile.Filter = "PNG files (*.png)|*.png";
            savefile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory + @"USERDATA\HunterSets\";

            if (savefile.ShowDialog() == true)
            {
                GearImageGrid.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x1E, 0x1E, 0x2E));
                CreateBitmapFromVisual(GearImageGrid, savefile.FileName);
                CopyUIElementToClipboard(GearImageGrid);
                GearImageGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
            }
        }

        /// <summary>
        /// Copies a UI element to the clipboard as an image.
        /// </summary>
        /// <param name="element">The element to copy.</param>
        public static void CopyUIElementToClipboard(FrameworkElement element)
        {
            double width = element.ActualWidth;
            double height = element.ActualHeight;
            if (width <= 0 || height <= 0)
            {
                System.Windows.MessageBox.Show("Please load the gear stats by visiting the text tab in the configuration window", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(element);
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
            }
            bmpCopied.Render(dv);
            Clipboard.SetImage(bmpCopied);
        }

        public static void CreateBitmapFromVisual(Visual target, string fileName)
        {
            if (target == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);

            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                System.Windows.MessageBox.Show("Please load the gear stats by visiting the text tab in the configuration window", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            RenderTargetBitmap renderTarget = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext context = visual.RenderOpen())
            {
                VisualBrush visualBrush = new VisualBrush(target);
                visualBrush.Stretch = Stretch.None;
                context.DrawRectangle(visualBrush, null, new Rect(new Point(), bounds.Size));

            }

            renderTarget.Render(visual);
            PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
            using (Stream stm = File.Create(fileName))
            {
                bitmapEncoder.Save(stm);
            }
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyList.Items.Filter = GetFilter();
        }

        // on generate csv button click
        protected void BtnLogFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            string dateTime = DateTime.Now.ToString();
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            savefile.FileName = "HuntedLog-" + dateTime + ".csv";
            savefile.Filter = "CSV files (*.csv)|*.csv";
            savefile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory + @"USERDATA\HuntedLogs\";

            //https://stackoverflow.com/questions/11776781/savefiledialog-make-problems-with-streamwriter-in-c-sharp
            if (savefile.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(savefile.FileName))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(Monsters);
                }
            }
        }


        private void Config_Closed(object sender, EventArgs e)
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            DisposeAllWebViews();
            s.Reload();
            Close();
            DeletexNames_OnClosed();
        }

        private void BtnGuildCardFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            string dateTime = DateTime.Now.ToString();
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            savefile.FileName = "GuildCard-" + dateTime + ".png";
            savefile.Filter = "PNG files (*.png)|*.png";
            savefile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory + @"USERDATA\HunterSets\";

            if (savefile.ShowDialog() == true)
            {
                CreateBitmapFromVisual(GuildCardGrid, savefile.FileName);
                CopyUIElementToClipboard(GuildCardGrid);
            }
        }

        private void ChangeMonsterInfo()
        {
            if (webViewMonsterInfo == null)
                return;

            Dictionary<string, string> MonsterFeriasOptionDictionary = new Dictionary<string, string>();
            Dictionary<string, string> MonsterWikiOptionDictionary = new Dictionary<string, string>();
            Dictionary<string, string> MonsterVideoLinkOptionDictionary = new Dictionary<string, string>();

            for (int i = 0; i < monsterInfos.Length; i++)
            {
                foreach (var videolinkPerRankBand in monsterInfos[i].WeaponMatchups)
                {
                    MonsterVideoLinkOptionDictionary.Add(videolinkPerRankBand.Key + " " + monsterInfos[i].Name, videolinkPerRankBand.Value);
                }

                MonsterWikiOptionDictionary.Add(monsterInfos[i].Name, monsterInfos[i].WikiLink);
                MonsterFeriasOptionDictionary.Add(monsterInfos[i].Name, monsterInfos[i].FeriasLink);
            }

            string selectedName = MonsterNameListBox.SelectedItem.ToString() + "";
            selectedName = selectedName.Replace("System.Windows.Controls.ComboBoxItem: ", "");

            string selectedMatchup = WeaponMatchupListBox.SelectedItem.ToString() + " " + MonsterNameListBox.SelectedItem.ToString();
            selectedMatchup = selectedMatchup.Replace("System.Windows.Controls.ComboBoxItem: ", "");

            if (!MonsterFeriasOptionDictionary.TryGetValue(selectedName, out string? val1) || !MonsterWikiOptionDictionary.TryGetValue(selectedName, out string? val2))
                return;

            if (webViewMonsterInfo.CoreWebView2 == null)
                return;

            switch (MonsterInfoViewOptionComboBox.SelectedIndex)
            {
                default:
                    return;
                case 0://ferias
                    //https://stackoverflow.com/questions/1265812/howto-define-the-auto-width-of-the-wpf-gridview-column-in-code
                    DockPanelMonsterInfo.Width = Double.NaN;//Auto
                    DockPanelMonsterInfo.Height = Double.NaN;//Auto
                    webViewMonsterInfo.CoreWebView2.Navigate(MonsterFeriasOptionDictionary[MonsterNameListBox.SelectedItem.ToString() + ""]);
                    return;
                case 1://wiki
                    DockPanelMonsterInfo.Width = Double.NaN;//Auto
                    DockPanelMonsterInfo.Height = Double.NaN;//Auto
                    webViewMonsterInfo.CoreWebView2.Navigate(MonsterWikiOptionDictionary[MonsterNameListBox.SelectedItem.ToString() + ""]);
                    return;
                case 2://youtube
                    if (MonsterVideoLinkOptionDictionary.TryGetValue(selectedMatchup, out string? videoval) && MonsterVideoLinkOptionDictionary[selectedMatchup] != "")
                    {
                        DockPanelMonsterInfo.Width = 854;
                        DockPanelMonsterInfo.Height = 480;
                        webViewMonsterInfo.CoreWebView2.Navigate(MonsterVideoLinkOptionDictionary[selectedMatchup]);
                    }
                    else
                    {
                        System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Video not found. Go to issues page?", "【MHF-Z】Overlay Information Missing", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning, MessageBoxResult.No);
                        if (messageBoxResult.ToString() == "Yes")
                        {
                            string issueLink = "https://github.com/DorielRivalet/mhfz-overlay/issues/26";
                            var sInfo = new System.Diagnostics.ProcessStartInfo(issueLink)
                            {
                                UseShellExecute = true,
                            };
                            System.Diagnostics.Process.Start(sInfo);
                        }
                    }
                    return;
            }
        }

        private void MonsterNameListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeMonsterInfo();
        }

        private void MonsterInfoSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeMonsterInfo();
        }

        private void WeaponMatchupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeMonsterInfo();
        }

        private void MonsterViewInfoOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeMonsterInfo();
        }

        #region repository stats

        GitHubClient client = new GitHubClient(new ProductHeaderValue("MHFZ_Overlay"));

        private async Task GetRepoStats()
        {
            var info = client.GetLastApiInfo();

            if (info != null)
                OctokitInfo.Text = String.Format("Server Time Difference: {0}, Max Requests/hr: {1}, Requests remaining: {2}, Current Rate Limit Window Reset: {3}", info.ServerTimeDifference, info.RateLimit.Limit, info.RateLimit.Remaining, info.RateLimit.Reset);

            var issuesForOctokit = await client.Issue.GetAllForRepository("DorielRivalet", "MHFZ_Overlay");

            IssuesTextBlock.Text = issuesForOctokit.Count.ToString() + " Issue(s)";

            var watchers = await client.Activity.Watching.GetAllWatchers("DorielRivalet", "MHFZ_Overlay");
            WatchersTextBlock.Text = watchers.Count.ToString() + " Watcher(s)";

            info = client.GetLastApiInfo();

            if (info != null)
                OctokitInfo.Text = String.Format("Server Time Difference: {0}, Max Requests/hr: {1}, Requests remaining: {2}, Current Rate Limit Window Reset: {3}", info.ServerTimeDifference, info.RateLimit.Limit, info.RateLimit.Remaining, info.RateLimit.Reset);

            info = client.GetLastApiInfo();

            if (info != null)
                OctokitInfo.Text = String.Format("Server Time Difference: {0}, Max Requests/hr: {1}, Requests remaining: {2}, Reset Time: {3}", info.ServerTimeDifference, info.RateLimit.Limit, info.RateLimit.Remaining, info.RateLimit.Reset);

        }

        #endregion

        private void Fumo_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (myFumo == null)
                return;
            myFumo.Position = new TimeSpan(0, 0, 1);
            myFumo.Play();
        }

        private void Krill_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (myKrill == null)
                return;
            myKrill.Position = new TimeSpan(0, 0, 1);
            myKrill.Play();
        }

        private void Stars_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (myAnime == null) return;
            myAnime.Position = new TimeSpan(0, 0, 1);
            myAnime.Play();
        }

        private void Watcher_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (myWatcher == null) return;
            myWatcher.Position = new TimeSpan(0, 0, 1);
            myWatcher.Play();
        }

        /// <summary>
        /// might need to work on? is the memory reduction worth it?
        /// </summary>
        private void DeletexNames_OnClosed()
        {
            if (myWatcher != null)
            {
                this.UnregisterName("myWatcher");
                myWatcher = null;
            }
        }

        private void ExportUserSettings_Click(object sender, RoutedEventArgs e)
        {
            // Show a Save File Dialog to let the user choose the location for the JSON file
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.FileName = "user_settings"; // Default file name
            saveFileDialog.DefaultExt = ".json"; // Default file extension
            saveFileDialog.Filter = "JSON files (.json)|*.json"; // Filter files by extension

            // Show the Save File Dialog
            Nullable<bool> result = saveFileDialog.ShowDialog();

            // If the user clicked the Save button and selected a file
            if (result == true)
            {
                // Get the user settings from the Settings class
                Settings s = (Settings)Application.Current.TryFindResource("Settings");

                // Create a dictionary to store the user settings
                Dictionary<string, string> settings = new Dictionary<string, string>();

                // Loop through the user settings properties and add them to the dictionary
                foreach (System.Configuration.SettingsProperty setting in s.Properties)
                {
                    string settingName = setting.Name;
                    string settingValue = s[settingName].ToString();
                    settings.Add(settingName, settingValue);
                }

                // Serialize the dictionary to a JSON string
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

                // Save the JSON string to the selected file
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }
    };

    /* LoadConfig on startup
     * Load Config on window open to have extra copy
     * On Save -> Window close -> tell program to use new copy instead of current -> Save Config File
     * On Cancel -> Window Close -> Discard copy of config
     * On Config Change Still show changes immediately and show windows which are set to show -> Ignore logic that hides windows during this time and force  them on if they are enabled
     * 
     */
}
