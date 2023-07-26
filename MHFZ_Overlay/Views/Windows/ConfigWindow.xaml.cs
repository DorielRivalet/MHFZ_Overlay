// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Views.Windows;

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using EZlion.Mapper;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WPF;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.Services.Converter;
using MHFZ_Overlay.Views.CustomControls;
using Microsoft.Win32;
using Octokit;
using SkiaSharp;
using Wpf.Ui.Common;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Services;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using ComboBox = System.Windows.Controls.ComboBox;
using DataGrid = Wpf.Ui.Controls.DataGrid;
using ListView = System.Windows.Controls.ListView;
using MenuItem = Wpf.Ui.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxResult = System.Windows.MessageBoxResult;
using TextBlock = System.Windows.Controls.TextBlock;
using TextBox = System.Windows.Controls.TextBox;

/// <summary>
/// Interaction logic for ConfigWindow.xaml
/// </summary>
public partial class ConfigWindow : FluentWindow
{
    /// <summary>
    /// Gets or sets the main window.
    /// </summary>
    /// <value>
    /// The main window.
    /// </value>
    private MainWindow MainWindow { get; }

    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    private static string randomMonsterImage = "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png";

    private static readonly DatabaseService databaseManager = DatabaseService.GetInstance();
    private static readonly AchievementService achievementManager = AchievementService.GetInstance();
    private static readonly OverlaySettingsService overlaySettingsManager = OverlaySettingsService.GetInstance();

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

    public static Uri MonsterImage
    {
        get { return new Uri(randomMonsterImage, UriKind.RelativeOrAbsolute); }
    }

    // TODO put this in a read-only dictionary thing
    private MonsterLog[] Monsters = new MonsterLog[]
    {
      new MonsterLog(0, "None", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/none.png", 0),
      new MonsterLog(1, "Rathian", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rathian.png", 0, true),
      new MonsterLog(2, "Fatalis", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/fatalis.png", 0, true),
      new MonsterLog(3, "Kelbi", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kelbi.png", 0),
      new MonsterLog(4, "Mosswine", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/mosswine.png", 0),
      new MonsterLog(5, "Bullfango", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/bullfango.png", 0),
      new MonsterLog(6, "Yian Kut-Ku", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yian_kut-ku.png", 0, true),
      new MonsterLog(7, "Lao-Shan Lung", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/lao-shan_lung.png", 0, true),
      new MonsterLog(8, "Cephadrome", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/cephadrome.png", 0, true),
      new MonsterLog(9, "Felyne", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/felyne.png", 0),
      new MonsterLog(10, "Veggie Elder", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(11, "Rathalos", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rathalos.png", 0, true),
      new MonsterLog(12, "Aptonoth", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/aptonoth.png", 0),
      new MonsterLog(13, "Genprey", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/genprey.png", 0),
      new MonsterLog(14, "Diablos", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/diablos.png", 0, true),
      new MonsterLog(15, "Khezu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/khezu.png", 0, true),
      new MonsterLog(16, "Velociprey", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/velociprey.png", 0),
      new MonsterLog(17, "Gravios", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gravios.png", 0, true),
      new MonsterLog(18, "Felyne?", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/felyne.png", 0),
      new MonsterLog(19, "Vespoid", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/vespoid.png", 0),
      new MonsterLog(20, "Gypceros", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gypceros.png", 0, true),
      new MonsterLog(21, "Plesioth", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/plesioth.png", 0, true),
      new MonsterLog(22, "Basarios", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/basarios.png", 0, true),
      new MonsterLog(23, "Melynx", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/melynx.png", 0),
      new MonsterLog(24, "Hornetaur", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/hornetaur.png", 0),
      new MonsterLog(25, "Apceros", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/apceros.png", 0),
      new MonsterLog(26, "Monoblos", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/monoblos.png", 0, true),
      new MonsterLog(27, "Velocidrome", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/velocidrome.png", 0, true),
      new MonsterLog(28, "Gendrome", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gendrome.png", 0, true),
      new MonsterLog(29, "Rocks", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(30, "Ioprey", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/ioprey.png", 0),
      new MonsterLog(31, "Iodrome", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/iodrome.png", 0, true),
      new MonsterLog(32, "Pugis", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(33, "Kirin", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kirin.png", 0, true),
      new MonsterLog(34, "Cephalos", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/cephalos.png", 0),
      new MonsterLog(35, "Giaprey / Giadrome", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/giaprey.png", 0),
      new MonsterLog(36, "Crimson Fatalis", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/crimson_fatalis.png", 0, true),
      new MonsterLog(37, "Pink Rathian", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pink_rathian.png", 0, true),
      new MonsterLog(38, "Blue Yian Kut-Ku", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blue_yian_kut-ku.png", 0, true),
      new MonsterLog(39, "Purple Gypceros", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/purple_gypceros.png", 0, true),
      new MonsterLog(40, "Yian Garuga", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yian_garuga.png", 0, true),
      new MonsterLog(41, "Silver Rathalos", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/silver_rathalos.png", 0, true),
      new MonsterLog(42, "Gold Rathian", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gold_rathian.png", 0, true),
      new MonsterLog(43, "Black Diablos", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/black_diablos.png", 0, true),
      new MonsterLog(44, "White Monoblos", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/white_monoblos.png", 0, true),
      new MonsterLog(45, "Red Khezu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/red_khezu.png", 0, true),
      new MonsterLog(46, "Green Plesioth", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/green_plesioth.png", 0, true),
      new MonsterLog(47, "Black Gravios", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/black_gravios.png", 0, true),
      new MonsterLog(48, "Daimyo Hermitaur", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/daimyo_hermitaur.png", 0, true),
      new MonsterLog(49, "Azure Rathalos", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/azure_rathalos.png", 0, true),
      new MonsterLog(50, "Ashen Lao-Shan Lung", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/ashen_lao-shan_lung.png", 0, true),
      new MonsterLog(51, "Blangonga", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blangonga.png", 0, true),
      new MonsterLog(52, "Congalala", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/congalala.png", 0, true),
      new MonsterLog(53, "Rajang", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rajang.png", 0, true),
      new MonsterLog(54, "Kushala Daora", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kushala_daora.png", 0, true),
      new MonsterLog(55, "Shen Gaoren", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shen_gaoren.png", 0, true),
      new MonsterLog(56, "Great Thunderbug", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/great_thunderbug.png", 0),
      new MonsterLog(57, "Shakalaka", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shakalaka.png", 0),
      new MonsterLog(58, "Yama Tsukami", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yama_tsukami.png", 0, true),
      new MonsterLog(59, "Chameleos", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/chameleos.png", 0, true),
      new MonsterLog(60, "Rusted Kushala Daora", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rusted_kushala_daora.png", 0, true),
      new MonsterLog(61, "Blango", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blango.png", 0),
      new MonsterLog(62, "Conga", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/conga.png", 0),
      new MonsterLog(63, "Remobra", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/remobra.png", 0),
      new MonsterLog(64, "Lunastra", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/lunastra.png", 0, true),
      new MonsterLog(65, "Teostra", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/teostra.png", 0, true),
      new MonsterLog(66, "Hermitaur", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/hermitaur.png", 0),
      new MonsterLog(67, "Shogun Ceanataur", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shogun_ceanataur.png", 0, true),
      new MonsterLog(68, "Bulldrome", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/bulldrome.png", 0, true),
      new MonsterLog(69, "Anteka", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/anteka.png", 0),
      new MonsterLog(70, "Popo", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/popo.png", 0),
      new MonsterLog(71, "White Fatalis", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/white_fatalis.png", 0, true),
      new MonsterLog(72, "Yama Tsukami", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yama_tsukami.png", 0, true),
      new MonsterLog(73, "Ceanataur", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/ceanataur.png", 0),
      new MonsterLog(74, "Hypnocatrice", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/hypnoc.png", 0, true),
      new MonsterLog(75, "Lavasioth", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/lavasioth.png", 0, true),
      new MonsterLog(76, "Tigrex", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/tigrex.png", 0, true),
      new MonsterLog(77, "Akantor", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/akantor.png", 0, true),
      new MonsterLog(78, "Bright Hypnoc", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/bright_hypnoc.png", 0, true),
      new MonsterLog(79, "Red Lavasioth", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/red_lavasioth.png", 0, true),
      new MonsterLog(80, "Espinas", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/espinas.png", 0, true),
      new MonsterLog(81, "Orange Espinas", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/orange_espinas.png", 0, true),
      new MonsterLog(82, "Silver Hypnoc", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/silver_hypnoc.png", 0, true),
      new MonsterLog(83, "Akura Vashimu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/akura_vashimu.png", 0, true),
      new MonsterLog(84, "Akura Jebia", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/akura_jebia.png", 0, true),
      new MonsterLog(85, "Berukyurosu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/berukyurosu.png", 0, true),
      new MonsterLog(86, "Cactus", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/cactus.png", 0),
      new MonsterLog(87, "Gorge Objects", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(88, "Gorge Rocks", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(89, "Pariapuria", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pariapuria.png", 0, true),
      new MonsterLog(90, "White Espinas", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/white_espinas.png", 0, true),
      new MonsterLog(91, "Kamu Orugaron", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kamu_orugaron.png", 0, true),
      new MonsterLog(92, "Nono Orugaron", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/nono_orugaron.png", 0, true),
      new MonsterLog(93, "Raviente", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/raviente.png", 0, true),
      new MonsterLog(94, "Dyuragaua", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/dyuragaua.png", 0, true),
      new MonsterLog(95, "Doragyurosu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/doragyurosu.png", 0, true),
      new MonsterLog(96, "Gurenzeburu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gurenzeburu.png", 0, true),
      new MonsterLog(97, "Burukku", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/burukku.png", 0),
      new MonsterLog(98, "Erupe", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/erupe.png", 0),
      new MonsterLog(99, "Rukodiora", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rukodiora.png", 0, true),
      new MonsterLog(100, "UNKNOWN", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/unknown.png", 0, true),
      new MonsterLog(101, "Gogomoa", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gogomoa.png", 0, true),
      new MonsterLog(102, "Kokomoa", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gogomoa.png", 0),
      new MonsterLog(103, "Taikun Zamuza", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/taikun_zamuza.png", 0, true),
      new MonsterLog(104, "Abiorugu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/abiorugu.png", 0, true),
      new MonsterLog(105, "Kuarusepusu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kuarusepusu.png", 0, true),
      new MonsterLog(106, "Odibatorasu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/odibatorasu.png", 0, true),
      new MonsterLog(107, "Disufiroa", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/disufiroa.png", 0, true),
      new MonsterLog(108, "Rebidiora", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/rebidiora.png", 0, true),
      new MonsterLog(109, "Anorupatisu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/anorupatisu.png", 0, true),
      new MonsterLog(110, "Hyujikiki", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/hyujikiki.png", 0, true),
      new MonsterLog(111, "Midogaron", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/midogaron.png", 0, true),
      new MonsterLog(112, "Giaorugu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/giaorugu.png", 0, true),
      new MonsterLog(113, "Mi Ru", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/mi_ru.png", 0, true),
      new MonsterLog(114, "Farunokku", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/farunokku.png", 0, true),
      new MonsterLog(115, "Pokaradon", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pokaradon.png", 0, true),
      new MonsterLog(116, "Shantien", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shantien.png", 0, true),
      new MonsterLog(117, "Pokara", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pokara.png", 0),
      new MonsterLog(118, "Dummy", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(119, "Goruganosu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/goruganosu.png", 0, true),
      new MonsterLog(120, "Aruganosu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/aruganosu.png", 0, true),
      new MonsterLog(121, "Baruragaru", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/baruragaru.png", 0, true),
      new MonsterLog(122, "Zerureusu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/zerureusu.png", 0, true),
      new MonsterLog(123, "Gougarf", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gougarf.png", 0, true),
      new MonsterLog(124, "Uruki", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/uruki.png", 0),
      new MonsterLog(125, "Forokururu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/forokururu.png", 0, true),
      new MonsterLog(126, "Meraginasu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/meraginasu.png", 0, true),
      new MonsterLog(127, "Diorex", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/diorex.png", 0, true),
      new MonsterLog(128, "Garuba Daora", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/garuba_daora.png", 0, true),
      new MonsterLog(129, "Inagami", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/inagami.png", 0, true),
      new MonsterLog(130, "Varusaburosu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/varusaburosu.png", 0, true),
      new MonsterLog(131, "Poborubarumu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/poborubarumu.png", 0, true),
      new MonsterLog(132, "1st District Duremudira", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/duremudira.png", 0, true),
      new MonsterLog(133, "UNK", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(134, "Felyne", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/felyne.png", 0),
      new MonsterLog(135, "Blue NPC", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(136, "UNK", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(137, "Cactus", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/cactus.png", 0),
      new MonsterLog(138, "Veggie Elders", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(139, "Gureadomosu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gureadomosu.png", 0, true),
      new MonsterLog(140, "Harudomerugu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/harudomerugu.png", 0, true),
      new MonsterLog(141, "Toridcless", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/toridcless.png", 0, true),
      new MonsterLog(142, "Gasurabazura", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gasurabazura.png", 0, true),
      new MonsterLog(143, "Kusubami", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/kusubami.png", 0),
      new MonsterLog(144, "Yama Kurai", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/yama_kurai.png", 0, true),
      new MonsterLog(145, "2nd District Duremudira", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/duremudira.png", 0, true),
      new MonsterLog(146, "Zinogre", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/zinogre.png", 0, true),
      new MonsterLog(147, "Deviljho", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/deviljho.png", 0, true),
      new MonsterLog(148, "Brachydios", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/brachydios.png", 0, true),
      new MonsterLog(149, "Berserk Raviente", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/berserk_raviente.png", 0, true),
      new MonsterLog(150, "Toa Tesukatora", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/toa_tesukatora.png", 0, true),
      new MonsterLog(151, "Barioth", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/barioth.png", 0, true),
      new MonsterLog(152, "Uragaan", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/uragaan.png", 0, true),
      new MonsterLog(153, "Stygian Zinogre", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/stygian_zinogre.png", 0, true),
      new MonsterLog(154, "Guanzorumu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/guanzorumu.png", 0, true),
      new MonsterLog(155, "Starving Deviljho", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/starving_deviljho.png", 0, true),
      new MonsterLog(156, "UNK", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(157, "Egyurasu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(158, "Voljang", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/voljang.png", 0, true),
      new MonsterLog(159, "Nargacuga", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/nargacuga.png", 0, true),
      new MonsterLog(160, "Keoaruboru", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/keoaruboru.png", 0, true),
      new MonsterLog(161, "Zenaserisu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/zenaserisu.png", 0, true),
      new MonsterLog(162, "Gore Magala", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/gore_magala.png", 0, true),
      new MonsterLog(163, "Blinking Nargacuga", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blinking_nargacuga.png", 0, true),
      new MonsterLog(164, "Shagaru Magala", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/shagaru_magala.png", 0, true),
      new MonsterLog(165, "Amatsu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/amatsu.png", 0, true),
      new MonsterLog(166, "Elzelion", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/elzelion.png", 0, true),
      new MonsterLog(167, "Arrogant Duremudira", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/arrogant_duremudira.png", 0, true),
      new MonsterLog(168, "Rocks", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(169, "Seregios", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/seregios.png", 0, true),
      new MonsterLog(170, "Bogabadorumu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/zenith_bogabadorumu.gif", 0, true),
      new MonsterLog(171, "Unknown Blue Barrel", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png", 0),
      new MonsterLog(172, "Blitzkrieg Bogabadorumu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/blitzkrieg_bogabadorumu.png", 0, true),
      new MonsterLog(173, "Costumed Uruki", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/uruki.png", 0),
      new MonsterLog(174, "Sparkling Zerureusu", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/sparkling_zerureusu.png", 0, true),
      new MonsterLog(175, "PSO2 Rappy", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/pso2_rappy.png", 0),
      new MonsterLog(176, "King Shakalaka", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/king_shakalaka.png", 0, true),
    };

    public static string ReplaceMonsterInfoFeriasVersion(string link)
    {
        string ReplaceSettingsLink = getFeriasLink();

        // Check if no need to replace because its the same version already
        if (link.Contains(ReplaceSettingsLink))
        {
            return link;
        }

        string separator = "mons/";
        string info = link.Split(separator)[1];

        return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", ReplaceSettingsLink, separator, info);
    }

    public int GetHuntedCount(int id)
    {
        var dl = MainWindow.dataLoader;

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
            case 9:
                return dl.model.FelyneHunted();
            case 10:
                return 0;
            case 11:
                return dl.model.RathalosHunted();
            case 12:
                return dl.model.AptonothHunted();
            case 13:
                return dl.model.GenpreyHunted();
            case 14:
                return dl.model.DiablosHunted();
            case 15:
                return dl.model.KhezuHunted();
            case 16:
                return dl.model.VelocipreyHunted();
            case 17:
                return dl.model.GraviosHunted();
            case 18:
                return 0;
            case 19:
                return dl.model.VespoidHunted();
            case 20:
                return dl.model.GypcerosHunted();
            case 21:
                return dl.model.PlesiothHunted();
            case 22:
                return dl.model.BasariosHunted();
            case 23:
                return dl.model.MelynxHunted();
            case 24:
                return dl.model.HornetaurHunted();
            case 25:
                return dl.model.ApcerosHunted();
            case 26:
                return dl.model.MonoblosHunted();
            case 27:
                return dl.model.VelocidromeHunted();
            case 28:
                return dl.model.GendromeHunted();
            case 29:
                return dl.model.RocksHunted();
            case 30:
                return dl.model.IopreyHunted();
            case 31:
                return dl.model.IodromeHunted();
            case 32:
                return 0;
            case 33:
                return dl.model.KirinHunted();
            case 34:
                return dl.model.CephalosHunted();
            case 35:
                return dl.model.GiapreyHunted();
            case 36:
                return dl.model.CrimsonFatalisHunted();
            case 37:
                return dl.model.PinkRathianHunted();
            case 38:
                return dl.model.BlueYianKutKuHunted();
            case 39:
                return dl.model.PurpleGypcerosHunted();
            case 40:
                return dl.model.YianGarugaHunted();
            case 41:
                return dl.model.SilverRathalosHunted();
            case 42:
                return dl.model.GoldRathianHunted();
            case 43:
                return dl.model.BlackDiablosHunted();
            case 44:
                return dl.model.WhiteMonoblosHunted();
            case 45:
                return dl.model.RedKhezuHunted();
            case 46:
                return dl.model.GreenPlesiothHunted();
            case 47:
                return dl.model.BlackGraviosHunted();
            case 48:
                return dl.model.DaimyoHermitaurHunted();
            case 49:
                return dl.model.AzureRathalosHunted();
            case 50:
                return dl.model.AshenLaoShanLungHunted();
            case 51:
                return dl.model.BlangongaHunted();
            case 52:
                return dl.model.CongalalaHunted();
            case 53:
                return dl.model.RajangHunted();
            case 54:
                return dl.model.KushalaDaoraHunted();
            case 55:
                return dl.model.ShenGaorenHunted();
            case 56:
                return dl.model.GreatThunderbugHunted();
            case 57:
                return dl.model.ShakalakaHunted();
            case 58:
                return dl.model.YamaTsukamiHunted();
            case 59:
                return dl.model.ChameleosHunted();
            case 60:
                return dl.model.RustedKushalaDaoraHunted();
            case 61:
                return dl.model.BlangoHunted();
            case 62:
                return dl.model.CongaHunted();
            case 63:
                return dl.model.RemobraHunted();
            case 64:
                return dl.model.LunastraHunted();
            case 65:
                return dl.model.TeostraHunted();
            case 66:
                return dl.model.HermitaurHunted();
            case 67:
                return dl.model.ShogunCeanataurHunted();
            case 68:
                return dl.model.BulldromeHunted();
            case 69:
                return dl.model.AntekaHunted();
            case 70:
                return dl.model.PopoHunted();
            case 71:
                return dl.model.WhiteFatalisHunted();
            case 72:
                return dl.model.YamaTsukamiHunted();
            case 73:
                return dl.model.CeanataurHunted();
            case 74:
                return dl.model.HypnocHunted();
            case 75:
                return dl.model.VolganosHunted();
            case 76:
                return dl.model.TigrexHunted();
            case 77:
                return dl.model.AkantorHunted();
            case 78:
                return dl.model.BrightHypnocHunted();
            case 79:
                return dl.model.RedVolganosHunted();
            case 80:
                return dl.model.EspinasHunted();
            case 81:
                return dl.model.OrangeEspinasHunted();
            case 82:
                return dl.model.SilverHypnocHunted();
            case 83:
                return dl.model.AkuraVashimuHunted();
            case 84:
                return dl.model.AkuraJebiaHunted();
            case 85:
                return dl.model.BerukyurosuHunted();
            case 86:
                return dl.model.CactusHunted();
            case 87:
                return dl.model.GorgeObjectsHunted();
            case 88:
                return 0;
            case 89:
                return dl.model.PariapuriaHunted();
            case 90:
                return dl.model.WhiteEspinasHunted();
            case 91:
                return dl.model.KamuOrugaronHunted();
            case 92:
                return dl.model.NonoOrugaronHunted();
            case 93:
                return 0;
            case 94:
                return dl.model.DyuragauaHunted();
            case 95:
                return dl.model.DoragyurosuHunted();
            case 96:
                return dl.model.GurenzeburuHunted();
            case 97:
                return dl.model.BurukkuHunted();
            case 98:
                return dl.model.ErupeHunted();
            case 99:
                return dl.model.RukodioraHunted();
            case 100:
                return dl.model.UnknownHunted();
            case 101:
                return dl.model.GogomoaHunted();
            case 102:
                return 0;
            case 103:
                return dl.model.TaikunZamuzaHunted();
            case 104:
                return dl.model.AbioruguHunted();
            case 105:
                return dl.model.KuarusepusuHunted();
            case 106:
                return dl.model.OdibatorasuHunted();
            case 107:
                return dl.model.DisufiroaHunted();
            case 108:
                return dl.model.RebidioraHunted();
            case 109:
                return dl.model.AnorupatisuHunted();
            case 110:
                return dl.model.HyujikikiHunted();
            case 111:
                return dl.model.MidogaronHunted();
            case 112:
                return dl.model.GiaoruguHunted();
            case 113:
                return dl.model.MiRuHunted();
            case 114:
                return dl.model.FarunokkuHunted();
            case 115:
                return dl.model.PokaradonHunted();
            case 116:
                return dl.model.ShantienHunted();
            case 117:
                return dl.model.PokaraHunted();
            case 118:
                return 0;
            case 119:
                return dl.model.GoruganosuHunted();
            case 120:
                return dl.model.AruganosuHunted();
            case 121:
                return dl.model.BaruragaruHunted();
            case 122:
                return dl.model.ZerureusuHunted();
            case 123:
                return dl.model.GougarfHunted();
            case 124:
                return dl.model.UrukiHunted();
            case 125:
                return dl.model.ForokururuHunted();
            case 126:
                return dl.model.MeraginasuHunted();
            case 127:
                return dl.model.DiorexHunted();
            case 128:
                return dl.model.GarubaDaoraHunted();
            case 129:
                return dl.model.InagamiHunted();
            case 130:
                return dl.model.VarusaburosuHunted();
            case 131:
                return dl.model.PoborubarumuHunted();
            case 132:
                return dl.model.FirstDistrictDuremudiraSlays();
            case 133:
                return 0;
            case 134:
                return 0;
            case 135:
                return 0;
            case 136:
                return 0;
            case 137:
                return dl.model.CactusHunted();
            case 138:
                return 0;
            case 139:
                return dl.model.GureadomosuHunted();
            case 140:
                return dl.model.HarudomeruguHunted();
            case 141:
                return dl.model.ToridclessHunted();
            case 142:
                return dl.model.GasurabazuraHunted();
            case 143:
                return dl.model.KusubamiHunted();
            case 144:
                return dl.model.YamaKuraiHunted();
            case 145:
                return dl.model.SecondDistrictDuremudiraSlays();
            case 146:
                return dl.model.ZinogreHunted();
            case 147:
                return dl.model.DeviljhoHunted();
            case 148:
                return dl.model.BrachydiosHunted();
            case 149:
                return 0;
            case 150:
                return dl.model.ToaTesukatoraHunted();
            case 151:
                return dl.model.BariothHunted();
            case 152:
                return dl.model.UragaanHunted();
            case 153:
                return dl.model.StygianZinogreHunted();
            case 154:
                return dl.model.GuanzorumuHunted();
            case 155:
                return dl.model.StarvingDeviljhoHunted();
            case 156:
                return 0;
            case 157:
                return 0;
            case 158:
                return dl.model.VoljangHunted();
            case 159:
                return dl.model.NargacugaHunted();
            case 160:
                return dl.model.KeoaruboruHunted();
            case 161:
                return dl.model.ZenaserisuHunted();
            case 162:
                return dl.model.GoreMagalaHunted();
            case 163:
                return dl.model.BlinkingNargacugaHunted();
            case 164:
                return dl.model.ShagaruMagalaHunted();
            case 165:
                return dl.model.AmatsuHunted();
            case 166:
                return dl.model.ElzelionHunted();
            case 167:
                return dl.model.ArrogantDuremudiraHunted();
            case 168:
                return 0;
            case 169:
                return dl.model.SeregiosHunted();
            case 170:
                return dl.model.BogabadorumuHunted();
            case 171:
                return 0;
            case 172:
                return dl.model.BlitzkriegBogabadorumuHunted();
            case 173:
                return 0;
            case 174:
                return dl.model.SparklingZerureusuHunted();
            case 175:
                return dl.model.PSO2RappyHunted();
            case 176:
                return dl.model.KingShakalakaHunted();
        }
    }

    private IReadOnlyList<MonsterInfo> monsterInfos = MonsterInfos.MonsterInfoIDs;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigWindow"/> class.
    /// </summary>
    /// <param name="mainWindow">The main window.</param>
    public ConfigWindow(MainWindow mainWindow)
    {
        // Create a Stopwatch instance
        Stopwatch stopwatch = new Stopwatch();

        // Start the stopwatch
        stopwatch.Start();

        InitializeComponent();
        logger.Info(CultureInfo.InvariantCulture, $"ConfigWindow initialized");

        Topmost = true;
        MainWindow = mainWindow;

        string background1 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/1.png";
        string background2 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/2.png";
        string background3 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/3.png";
        string background4 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/4.png";
        string background5 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/5.png";
        string background6 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/6.png";
        string background7 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/7.png";
        string background8 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/8.png";
        string background9 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/9.png";

        // https://stackoverflow.com/questions/30839173/change-background-image-in-wpf-using-c-sharp
        GeneralContent.Background = new ImageBrush(new BitmapImage(new Uri(background1)));
        HunterNotesContent.Background = new ImageBrush(new BitmapImage(new Uri(background2)));
        MonsterHPContent.Background = new ImageBrush(new BitmapImage(new Uri(background3)));
        MonsterStatusContent.Background = new ImageBrush(new BitmapImage(new Uri(background4)));
        DiscordRPCContent.Background = new ImageBrush(new BitmapImage(new Uri(background5)));
        CreditsContent.Background = new ImageBrush(new BitmapImage(new Uri(background6)));
        MonsterInfoContent.Background = new ImageBrush(new BitmapImage(new Uri(background7)));
        QuestLogContent.Background = new ImageBrush(new BitmapImage(new Uri(background8)));
        PlayerContent.Background = new ImageBrush(new BitmapImage(new Uri(background9)));

        // TODO: test this
        DataContext = MainWindow.dataLoader.model;

        for (int i = 0; i < Monsters.Length; i++)
        {
            Monsters[i].Hunted = GetHuntedCount(Monsters[i].ID);
        }

        HuntLogDataGrid.ItemsSource = Monsters;
        HuntLogDataGrid.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        FilterBox.ItemsSource = new string[] { "All", "Large Monster", "Small Monster" };
        HuntLogDataGrid.Items.Filter = MonsterFilterAll;

        // See: https://stackoverflow.com/questions/22285866/why-relaycommand
        // Or use MVVM Light to obtain RelayCommand.
        List<string> MonsterNameList = new List<string>();

        for (int i = 0; i < monsterInfos.Count; i++)
        {
            MonsterNameList.Add(monsterInfos[i].Name);
        }

        MonsterNameComboBox.ItemsSource = MonsterNameList;

        _ = GetRepoStats();

        replaceAllMonsterInfoFeriasLinks();

        weaponUsageData = databaseManager.CalculateTotalWeaponUsage(this, MainWindow.dataLoader);

        // In your initialization or setup code
        ISnackbarService snackbarService = new SnackbarService();

        // Replace 'snackbarControl' with your actual snackbar control instance
        snackbarService.SetSnackbarPresenter(ConfigWindowSnackBarPresenter);

        // Stop the stopwatch
        stopwatch.Stop();

        // Get the elapsed time in milliseconds
        double elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        // Print the elapsed time
        logger.Debug($"ConfigWindow ctor Elapsed Time: {elapsedTimeMs} ms");
    }

    private List<WeaponUsage> weaponUsageData = new ();

    private void SetWeaponUsageChart(CartesianChart weaponUsageChart)
    {
        MainWindow.dataLoader.model.weaponUsageSeries.Clear();
        Settings s = (Settings)Application.Current.TryFindResource("Settings");

        var weaponStyles = new[] { "Earth Style", "Heaven Style", "Storm Style", "Extreme Style" };
        var weaponTypes = new[] { "Sword and Shield", "Dual Swords", "Great Sword", "Long Sword",
                          "Hammer", "Hunting Horn", "Lance", "Gunlance", "Tonfa",
                          "Switch Axe F", "Magnet Spike", "Light Bowgun", "Heavy Bowgun",
                          "Bow" };

        foreach (var weaponType in weaponTypes)
        {
            foreach (var weaponStyle in weaponStyles)
            {
                switch (weaponStyle)
                {
                    case "Earth Style":
                        var weaponUsageCount = weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        MainWindow.dataLoader.model.weaponUsageEarthStyle.Add(weaponUsageCount);
                        break;
                    case "Heaven Style":
                        weaponUsageCount = weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        MainWindow.dataLoader.model.weaponUsageHeavenStyle.Add(weaponUsageCount);
                        break;
                    case "Storm Style":
                        weaponUsageCount = weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        MainWindow.dataLoader.model.weaponUsageStormStyle.Add(weaponUsageCount);
                        break;
                    case "Extreme Style":
                        weaponUsageCount = weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        MainWindow.dataLoader.model.weaponUsageExtremeStyle.Add(weaponUsageCount);
                        break;
                }
            }
        }

        MainWindow.dataLoader.model.weaponUsageSeries.Add(new StackedColumnSeries<long>
        {
            Name = "Earth Style",
            Values = MainWindow.dataLoader.model.weaponUsageEarthStyle,
            DataLabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
            DataLabelsSize = 14,
            DataLabelsPosition = DataLabelsPosition.Middle,
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
        });

        MainWindow.dataLoader.model.weaponUsageSeries.Add(new StackedColumnSeries<long>
        {
            Name = "Heaven Style",
            Values = MainWindow.dataLoader.model.weaponUsageHeavenStyle,
            DataLabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
            DataLabelsSize = 14,
            DataLabelsPosition = DataLabelsPosition.Middle,
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
        });

        MainWindow.dataLoader.model.weaponUsageSeries.Add(new StackedColumnSeries<long>
        {
            Name = "Storm Style",
            Values = MainWindow.dataLoader.model.weaponUsageStormStyle,
            DataLabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
            DataLabelsSize = 14,
            DataLabelsPosition = DataLabelsPosition.Middle,
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
        });

        MainWindow.dataLoader.model.weaponUsageSeries.Add(new StackedColumnSeries<long>
        {
            Name = "Extreme Style",
            Values = MainWindow.dataLoader.model.weaponUsageExtremeStyle,
            DataLabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
            DataLabelsSize = 14,
            DataLabelsPosition = DataLabelsPosition.Middle,
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
        });

        weaponUsageChart.Series = MainWindow.dataLoader.model.weaponUsageSeries;
        weaponUsageChart.XAxes = new List<Axis>
        {
                new Axis
                {
                    MinStep=1,
                    Padding=new LiveChartsCore.Drawing.Padding(0,0,0,0),
                    ShowSeparatorLines=true,
                    IsVisible=false,
                    LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
                }
        };
        weaponUsageChart.YAxes = new List<Axis>
        {
                new Axis
                {
                    MinStep=1,
                    LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
                    ShowSeparatorLines=true,
                    TextSize=12,
                }
        };
    }

    private void replaceAllMonsterInfoFeriasLinks()
    {
        for (int i = 0; i < monsterInfos.Count; i++)
        {
            monsterInfos[i].FeriasLink = ReplaceMonsterInfoFeriasVersion(monsterInfos[i].FeriasLink);
        }
    }

    private bool MonsterFilterAll(object obj)
    {
        var FilterObj = obj as MonsterLog;
        if (FilterObj == null)
        {
            return false;
        }

        return FilterObj.IsLarge || !FilterObj.IsLarge;
    }

    private bool MonsterFilterLarge(object obj)
    {
        var FilterObj = obj as MonsterLog;
        if (FilterObj == null)
        {
            return false;
        }

        return FilterObj.IsLarge;
    }

    private bool MonsterFilterSmall(object obj)
    {
        var FilterObj = obj as MonsterLog;
        return FilterObj != null ? !FilterObj.IsLarge : false;
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

    // TODO does this cover everything?
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
        MainWindow.dataLoader.model.Configuring = false;
    }

    /// <summary>
    /// Set default settings
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
        var result = MessageBox.Show("Resetting settings, are you sure?", Messages.InfoTitle, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        if (result == MessageBoxResult.Yes)
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            DisposeAllWebViews();
            s.Reset();
        }
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
        {
            e.Handled = true;
        }
    }

    private void ValidateDecimalNumber(object sender, TextCompositionEventArgs e)
    {
        foreach (char ch in e.Text)
        {
            if (!char.IsDigit(ch) && ch != '.')
            {
                e.Handled = true;
                return;
            }
        }

        var textBox = (TextBox)sender;
        var newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var isValidDecimal = decimal.TryParse(newText, out _);

        if (!isValidDecimal)
        {
            e.Handled = true;
        }
        else if (e.Text == "." && textBox.Text.Contains("."))
        {
            e.Handled = true;
        }
    }

    // https://stackoverflow.com/questions/1051989/regex-for-alphanumeric-but-at-least-one-letter
    // ^(?=.*[a-zA-Z].*)([a-zA-Z0-9]{6,12})$
    // ([a-zA-Z0-9_\s]+)
    // [^a-zA-Z_0-9]

    /// <summary>
    /// Validates the name.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
    private void ValidateName(object sender, TextCompositionEventArgs e)
    {
        // Create a Regex

        // Get all matches  
        // https://stackoverflow.com/questions/1046740/how-can-i-validate-a-string-to-only-allow-alphanumeric-characters-in-it
        if (!(e.Text.All(char.IsLetterOrDigit)))
        {
            // just letters and digits.
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
        return s.TextFormatExport ?? "None";
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
        {
            textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        }
        else if (GetTextFormatMode() == "Markdown")
        {
            textToSave = MainWindow.dataLoader.model.MarkdownSavedGearStats;
        }

        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        FileService.SaveTextFile(snackbar, textToSave, "GearStats");
    }

    /// <summary>
    /// Copy to clipboard. TODO: change function name, its too generic
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnCopyFile_Click(object sender, RoutedEventArgs e)
    {
        string textToSave = GearStats.Text;

        if (GetTextFormatMode() == "Code Block")
        {
            textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        }
        else if (GetTextFormatMode() == "Markdown")
        {
            textToSave = MainWindow.dataLoader.model.MarkdownSavedGearStats;
        }
        else if (GetTextFormatMode() == "Image")
        {
            var previousBackground = GearTextGrid.Background;
            GearTextGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
            var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
            snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
            FileService.CopyUIElementToClipboard(GearTextGrid, snackbar);
            GearTextGrid.Background = previousBackground;
            return;
        }

        // https://stackoverflow.com/questions/3546016/how-to-copy-data-to-clipboard-in-c-sharp
        Clipboard.SetText(textToSave);
        var snackbarSuccess = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbarSuccess.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        snackbarSuccess.Title = Messages.InfoTitle;
        snackbarSuccess.Content = "Copied text to clipboard";
        snackbarSuccess.Appearance = ControlAppearance.Success;
        snackbarSuccess.Icon = new SymbolIcon(SymbolRegular.Clipboard32);
        snackbarSuccess.Timeout = SnackbarTimeOut;
        snackbarSuccess.Show();
    }

    public TimeSpan SnackbarTimeOut { get; set; } = TimeSpan.FromSeconds(5);

    private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        HuntLogDataGrid.Items.Filter = GetFilter();
    }

    private void Config_Closed(object sender, EventArgs e)
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        DisposeAllWebViews();
        s.Reload();
        Close();
        DeletexNames_OnClosed();
    }

    private void ChangeMonsterInfo()
    {
        if (webViewMonsterInfo == null)
        {
            return;
        }

        Dictionary<string, string> MonsterFeriasOptionDictionary = new ();
        Dictionary<string, string> MonsterWikiOptionDictionary = new ();
        Dictionary<string, string> MonsterVideoLinkOptionDictionary = new ();

        for (int i = 0; i < monsterInfos.Count; i++)
        {
            foreach (var videolinkPerRankBand in monsterInfos[i].WeaponMatchups)
            {
                MonsterVideoLinkOptionDictionary.Add(videolinkPerRankBand.Key + " " + monsterInfos[i].Name, videolinkPerRankBand.Value);
            }

            MonsterWikiOptionDictionary.Add(monsterInfos[i].Name, monsterInfos[i].WikiLink);
            MonsterFeriasOptionDictionary.Add(monsterInfos[i].Name, monsterInfos[i].FeriasLink);
        }

        // see this
        // string selectedOverlayMode = ((ComboBoxItem)configWindow.OverlayModeComboBox.SelectedItem).Content.ToString();
        string? selectedName = MonsterNameComboBox.SelectedItem.ToString();
        if (string.IsNullOrEmpty(selectedName))
        {
            selectedName = string.Empty;
        }

        string selectedMatchup = $"{((ComboBoxItem)WeaponMatchupComboBox.SelectedItem).Content} {selectedName}";

        if (!MonsterFeriasOptionDictionary.TryGetValue(selectedName, out string? val1) || !MonsterWikiOptionDictionary.TryGetValue(selectedName, out string? val2))
        {
            return;
        }

        if (webViewMonsterInfo.CoreWebView2 == null)
        {
            return;
        }

        switch (MonsterInfoViewOptionComboBox.SelectedIndex)
        {
            default:
                return;
            case 0:// ferias
                // https://stackoverflow.com/questions/1265812/howto-define-the-auto-width-of-the-wpf-gridview-column-in-code
                DockPanelMonsterInfo.Width = Double.NaN;// Auto
                DockPanelMonsterInfo.Height = Double.NaN;// Auto
                webViewMonsterInfo.CoreWebView2.Navigate(MonsterFeriasOptionDictionary[MonsterNameComboBox.SelectedItem.ToString() + ""]);
                return;
            case 1:// wiki
                DockPanelMonsterInfo.Width = Double.NaN;// Auto
                DockPanelMonsterInfo.Height = Double.NaN;// Auto
                webViewMonsterInfo.CoreWebView2.Navigate(MonsterWikiOptionDictionary[MonsterNameComboBox.SelectedItem.ToString() + ""]);
                return;
            case 2:// youtube
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

    private void MonsterNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ChangeMonsterInfo();
    }

    private void MonsterInfoSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ChangeMonsterInfo();
    }

    private void WeaponMatchupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ChangeMonsterInfo();
    }

    private void MonsterViewInfoOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ChangeMonsterInfo();
    }

    GitHubClient client = new GitHubClient(new ProductHeaderValue("MHFZ_Overlay"));

    // TODO optimize
    private async Task GetRepoStats()
    {
        var info = client.GetLastApiInfo();

        if (info != null)
        {
            OctokitInfo.Text = string.Format(CultureInfo.InvariantCulture, "Server Time Difference: {0}, Max Requests/hr: {1}, Requests remaining: {2}, Current Rate Limit Window Reset: {3}", info.ServerTimeDifference, info.RateLimit.Limit, info.RateLimit.Remaining, info.RateLimit.Reset);
        }

        var issuesForOctokit = await client.Issue.GetAllForRepository("DorielRivalet", "MHFZ_Overlay");

        // TODO
        IssuesTextBlock.Text = (issuesForOctokit.Count - 2).ToString(CultureInfo.InvariantCulture) + " Issue(s)";

        var watchers = await client.Activity.Watching.GetAllWatchers("DorielRivalet", "MHFZ_Overlay");
        WatchersTextBlock.Text = watchers.Count.ToString(CultureInfo.InvariantCulture) + " Watcher(s)";

        info = client.GetLastApiInfo();

        if (info != null)
        {
            OctokitInfo.Text = string.Format(CultureInfo.InvariantCulture, "Server Time Difference: {0}, Max Requests/hr: {1}, Requests remaining: {2}, Current Rate Limit Window Reset: {3}", info.ServerTimeDifference, info.RateLimit.Limit, info.RateLimit.Remaining, info.RateLimit.Reset);
        }

        info = client.GetLastApiInfo();

        if (info != null)
        {
            OctokitInfo.Text = string.Format(CultureInfo.InvariantCulture, "Server Time Difference: {0}, Max Requests/hr: {1}, Requests remaining: {2}, Reset Time: {3}", info.ServerTimeDifference, info.RateLimit.Limit, info.RateLimit.Remaining, info.RateLimit.Reset);
        }
    }

    private void Fumo_MediaEnded(object sender, RoutedEventArgs e)
    {
        if (myFumo == null)
        {
            return;
        }

        myFumo.Position = new TimeSpan(0, 0, 1);
        myFumo.Play();
    }

    private void Krill_MediaEnded(object sender, RoutedEventArgs e)
    {
        if (myKrill == null)
        {
            return;
        }

        myKrill.Position = new TimeSpan(0, 0, 1);
        myKrill.Play();
    }

    private void Stars_MediaEnded(object sender, RoutedEventArgs e)
    {
        if (myAnime == null)
        {
            return;
        }

        myAnime.Position = new TimeSpan(0, 0, 1);
        myAnime.Play();
    }

    private void Watcher_MediaEnded(object sender, RoutedEventArgs e)
    {
        if (myWatcher == null)
        {
            return;
        }

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

    private void OpenSettingsFolder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string settingsFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            var settingsFileDirectoryName = Path.GetDirectoryName(settingsFile);
            if (!Directory.Exists(settingsFileDirectoryName))
            {
                logger.Error(CultureInfo.InvariantCulture, "Could not open settings folder");
                var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
                snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
                snackbar.Title = Messages.ErrorTitle;
                snackbar.Content = "Could not open settings folder";
                snackbar.Appearance = ControlAppearance.Danger;
                snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle24);
                snackbar.Timeout = SnackbarTimeOut;
                snackbar.Show();
                return;
            }

            string settingsFolder = settingsFileDirectoryName;

            // Open file manager at the specified folder
            Process.Start(ApplicationPaths.ExplorerPath, settingsFolder);
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
            snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
            snackbar.Title = Messages.ErrorTitle;
            snackbar.Content = "Could not open settings folder";
            snackbar.Appearance = ControlAppearance.Danger;
            snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle24);
            snackbar.Timeout = SnackbarTimeOut;
            snackbar.Show();
        }
    }

    private void OpenLogsFolder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName == null)
            {
                return;
            }

            var logFilePath = Path.Combine(directoryName, "logs", "logs.log");

            if (!File.Exists(logFilePath))
            {
                logger.Error(CultureInfo.InvariantCulture, "Could not find the log file: {0}", logFilePath);
                System.Windows.MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Could not find the log file: {0}", logFilePath), Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Open the log file using the default application
            try
            {
                var logFilePathDirectory = Path.GetDirectoryName(logFilePath);
                if (logFilePathDirectory == null)
                {
                    return;
                }

                Process.Start(ApplicationPaths.ExplorerPath, logFilePathDirectory);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }
    }

    private void OpenDatabaseFolder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            var directoryName = Path.GetDirectoryName(s.DatabaseFilePath);
            if (directoryName == null)
            {
                return;
            }

            if (!File.Exists(s.DatabaseFilePath))
            {
                logger.Error(CultureInfo.InvariantCulture, "Could not find the database file: {0}", s.DatabaseFilePath);
                System.Windows.MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Could not find the database file: {0}", s.DatabaseFilePath), Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Open the log file using the default application
            try
            {
                Process.Start(ApplicationPaths.ExplorerPath, directoryName);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }
    }

    private void questLoggingToggle_Check(object sender, RoutedEventArgs e)
    {
        if (MainWindow == null)
        {
            return;
        }

        MainWindow.dataLoader.model.ValidateGameFolder();

        databaseManager.CheckIfSchemaChanged(MainWindow.dataLoader);
    }

    private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        s.PlayerNationalityIndex = CountryComboBox.SelectedIndex;
    }

    private void QuestIDButton_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(QuestIDTextBox.Text))
        {
            SetDefaultInfoInQuestIDWeaponSection();
            databaseManager.QuestIDButton_Click(sender, e, this);
        }
    }

    private void SetDefaultInfoInQuestIDWeaponSection()
    {
        SwordAndShieldBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        SwordAndShieldRunIDTextBlock.Text = Messages.RunNotFound;

        GreatSwordBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        GreatSwordRunIDTextBlock.Text = Messages.RunNotFound;

        DualSwordsBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        DualSwordsRunIDTextBlock.Text = Messages.RunNotFound;

        LongSwordBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        LongSwordRunIDTextBlock.Text = Messages.RunNotFound;

        LanceBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        LanceRunIDTextBlock.Text = Messages.RunNotFound;

        GunlanceBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        GunlanceRunIDTextBlock.Text = Messages.RunNotFound;

        HammerBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        HammerRunIDTextBlock.Text = Messages.RunNotFound;

        HuntingHornBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        HuntingHornRunIDTextBlock.Text = Messages.RunNotFound;

        TonfaBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        TonfaRunIDTextBlock.Text = Messages.RunNotFound;

        SwitchAxeFBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        SwitchAxeFRunIDTextBlock.Text = Messages.RunNotFound;

        MagnetSpikeBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        MagnetSpikeRunIDTextBlock.Text = Messages.RunNotFound;

        LightBowgunBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        LightBowgunRunIDTextBlock.Text = Messages.RunNotFound;

        HeavyBowgunBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        HeavyBowgunRunIDTextBlock.Text = Messages.RunNotFound;

        BowBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        BowRunIDTextBlock.Text = Messages.RunNotFound;

        SelectedQuestObjectiveImage.Source = new BitmapImage(new Uri("https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/random.png"));
        SelectedQuestNameTextBlock.Text = Messages.QuestNotFound;
        SelectedQuestObjectiveTextBlock.Text = Messages.InvalidQuest;
        CurrentTimeTextBlock.Text = Messages.NotANumber;
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // nothing
    }

    private void QuestLogsSectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // nothing
    }

    private T? FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
    {
        // Confirm parent and childName are valid. 
        if (parent == null)
        {
            return null;
        }

        T? foundChild = null;

        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            // If the child is not of the request child type child
            T? childType = child as T;
            if (childType == null)
            {
                // recursively drill down the tree
                foundChild = FindChild<T>(child, childName);

                // If the child is found, break so we do not overwrite the found child. 
                if (foundChild != null)
                {
                    break;
                }
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                var frameworkElement = child as FrameworkElement;

                // If the child's name is set for search
                if (frameworkElement != null && frameworkElement.Name == childName)
                {
                    // if the child's name is of the request name
                    foundChild = (T)child;
                    break;
                }
            }
            else
            {
                // child element found.
                foundChild = (T)child;
                break;
            }
        }

        return foundChild;
    }

    private void WeaponUsageGraphComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        if (weaponUsageChart == null)
        {
            return;
        }

        MainWindow.dataLoader.model.weaponUsageEarthStyle.Clear();
        MainWindow.dataLoader.model.weaponUsageHeavenStyle.Clear();
        MainWindow.dataLoader.model.weaponUsageStormStyle.Clear();
        MainWindow.dataLoader.model.weaponUsageExtremeStyle.Clear();

        if (comboBox.SelectedIndex == 0)
        {
            weaponUsageData = databaseManager.CalculateTotalWeaponUsage(this, MainWindow.dataLoader);
        }
        else if (comboBox.SelectedIndex == 1)
        {
            weaponUsageData = databaseManager.CalculateTotalWeaponUsage(this, MainWindow.dataLoader, true);
        }
        else
        {
            return;
        }

        var weaponStyles = new[] { "Earth Style", "Heaven Style", "Storm Style", "Extreme Style" };
        var weaponTypes = new[] { "Sword and Shield", "Dual Swords", "Great Sword", "Long Sword",
                          "Hammer", "Hunting Horn", "Lance", "Gunlance", "Tonfa",
                          "Switch Axe F", "Magnet Spike", "Light Bowgun", "Heavy Bowgun",
                          "Bow", };

        foreach (var weaponType in weaponTypes)
        {
            foreach (var weaponStyle in weaponStyles)
            {
                switch (weaponStyle)
                {
                    case "Earth Style":
                        var weaponUsageCount = weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        MainWindow.dataLoader.model.weaponUsageEarthStyle.Add(weaponUsageCount);
                        break;
                    case "Heaven Style":
                        weaponUsageCount = weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        MainWindow.dataLoader.model.weaponUsageHeavenStyle.Add(weaponUsageCount);
                        break;
                    case "Storm Style":
                        weaponUsageCount = weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        MainWindow.dataLoader.model.weaponUsageStormStyle.Add(weaponUsageCount);
                        break;
                    case "Extreme Style":
                        weaponUsageCount = weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        MainWindow.dataLoader.model.weaponUsageExtremeStyle.Add(weaponUsageCount);
                        break;
                }
            }
        }
    }

    private void weaponUsageChart_Loaded(object sender, RoutedEventArgs e)
    {
        weaponUsageChart = (CartesianChart)sender;

        if (!weaponUsageChart.Series.Any())
        {
            MainWindow.dataLoader.model.weaponUsageEarthStyle.Clear();
            MainWindow.dataLoader.model.weaponUsageHeavenStyle.Clear();
            MainWindow.dataLoader.model.weaponUsageStormStyle.Clear();
            MainWindow.dataLoader.model.weaponUsageExtremeStyle.Clear();

            // TODO: is this needed?
            weaponUsageChart.SyncContext = MainWindow.dataLoader.model.weaponUsageSync;

            SetWeaponUsageChart(weaponUsageChart);
        }
    }

    private CartesianChart? weaponUsageChart;
    private TextBox? youtubeLinkTextBox;
    private ListView? mostRecentRunsListView;
    private DataGrid? mostRecentRunsDataGrid;
    private ListView? top20RunsListView;
    private DataGrid? top20RunsDataGrid;
    private TextBlock? questLogGearStatsTextBlock;
    private TextBlock? compendiumTextBlock;
    private CartesianChart? graphChart;
    private TextBlock? statsTextTextBlock;
    private CartesianChart? personalBestChart;
    private PolarChart? hunterPerformanceChart;
    private StackPanel? compendiumInformationStackPanel;
    private string personalBestSelectedWeapon = string.Empty;
    private string personalBestSelectedType = string.Empty;
    private DataGrid? calendarDataGrid;
    private Grid? personalBestChartGrid;
    private Grid? weaponUsageChartGrid;
    private Grid? statsGraphsGrid;
    private TextBlock? personalBestDescriptionTextBlock;
    private TextBlock? top20RunsDescriptionTextblock;
    private Grid? personalBestMainGrid;
    private Grid? top20MainGrid;
    private Grid? weaponStatsMainGrid;
    private Grid? statsGraphsMainGrid;
    private Grid? statsTextMainGrid;
    private ListView? achievementsListView;
    private Grid? achievementsSelectedInfoGrid;

    private void UpdateYoutubeLink_ButtonClick(object sender, RoutedEventArgs e)
    {
        // Get the quest ID and new YouTube link from the textboxes
        long runID = long.Parse(RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        if (youtubeLinkTextBox == null)
        {
            return;
        }

        string youtubeLink = youtubeLinkTextBox.Text.Trim();
        if (databaseManager.UpdateYoutubeLink(sender, e, runID, youtubeLink))
        {
            var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
            snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
            snackbar.Title = Messages.InfoTitle;
            snackbar.Content = string.Format(CultureInfo.InvariantCulture, "Updated run {0} with link https://youtube.com/watch?v={1}", runID, youtubeLink);
            snackbar.Appearance = ControlAppearance.Success;
            snackbar.Icon = new SymbolIcon(SymbolRegular.Video32);
            snackbar.Timeout = SnackbarTimeOut;
            snackbar.Show();
        }
        else
        {
            var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
            snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
            snackbar.Title = Messages.ErrorTitle;
            snackbar.Content = string.Format(CultureInfo.InvariantCulture, "Could not update run {0} with link https://youtube.com/watch?v={1}. The link may have already been set to the same value, or the run ID and link input are invalid.", runID, youtubeLink);
            snackbar.Appearance = ControlAppearance.Danger;
            snackbar.Icon = new SymbolIcon(SymbolRegular.Video32);
            snackbar.Timeout = SnackbarTimeOut;
            snackbar.Show();
        }
    }

    private void YoutubeIconButton_Click(object sender, RoutedEventArgs e)
    {
        long runID = long.Parse(RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        string youtubeLink = databaseManager.GetYoutubeLinkForRunID(runID);
        if (youtubeLink != "")
        {
            var sInfo = new System.Diagnostics.ProcessStartInfo(youtubeLink)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }
        else
        {
            MessageBox.Show("Run not found", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void YoutubeLinkTextBox_Loaded(object sender, RoutedEventArgs e)
    {
        youtubeLinkTextBox = (TextBox)sender;
    }

    private void Top20Runs_DataGridLoaded(object sender, RoutedEventArgs e)
    {
        top20RunsDataGrid = (DataGrid)sender;
        MainWindow.dataLoader.model.FastestRuns = databaseManager.GetFastestRuns(this);
        top20RunsDataGrid.ItemsSource = MainWindow.dataLoader.model.FastestRuns;
        top20RunsDataGrid.Items.Refresh();
    }

    private string top20RunsSelectedWeapon = string.Empty;

    private string statsGraphsSelectedOption = string.Empty;

    private string statsTextSelectedOption = string.Empty;

    private void weaponListTop20RunsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (top20RunsDataGrid == null)
        {
            return;
        }

        var comboBox = sender as ComboBox;
        if (comboBox == null)
        {
            return;
        }

        var selectedItem = comboBox.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }

        // You can now use the selectedItem variable to get the data or value of the selected option
        string? selectedWeapon = selectedItem.ToString()?.Replace("System.Windows.Controls.ComboBoxItem: ", "");
        if (string.IsNullOrEmpty(selectedWeapon))
        {
            return;
        }

        top20RunsSelectedWeapon = selectedWeapon;
        MainWindow.dataLoader.model.FastestRuns = databaseManager.GetFastestRuns(this, selectedWeapon);
        top20RunsDataGrid.ItemsSource = MainWindow.dataLoader.model.FastestRuns;
        top20RunsDataGrid.Items.Refresh();

        if (top20RunsDescriptionTextblock != null)
        {
            top20RunsDescriptionTextblock.Text = $"Top 20 fastest solo runs of quest ID {QuestIDTextBox.Text} by category {OverlayModeComboBox.Text}";
        }
    }

    private void QuestLogGearStats_Loaded(object sender, RoutedEventArgs e)
    {
        var textBlock = sender as TextBlock;
        if (textBlock == null)
        {
            return;
        }

        long runID = long.Parse(RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        textBlock.Text = MainWindow.dataLoader.model.GenerateGearStats(runID);
        questLogGearStatsTextBlock = textBlock;
    }

    private void QuestLogGearBtnSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (questLogGearStatsTextBlock == null)
        {
            return;
        }

        string textToSave = questLogGearStatsTextBlock.Text;
        textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        var fileName = "Set";
        var beginningFileName = "Run";
        var beginningText = RunIDTextBox.Text.Trim();
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.SaveTextFile(snackbar, textToSave, fileName, beginningFileName, beginningText);
    }

    private void QuestLogGearBtnCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (questLogGearStatsTextBlock == null)
        {
            return;
        }

        var previousBackground = questLogGearStatsTextBlock.Background;
        questLogGearStatsTextBlock.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(questLogGearStatsTextBlock, snackbar);
        questLogGearStatsTextBlock.Background = previousBackground;
    }

    private void Compendium_Loaded(object sender, RoutedEventArgs e)
    {
        var textBlock = sender as TextBlock;
        if (textBlock == null)
        {
            return;
        }

        textBlock.Text = MainWindow.dataLoader.model.GenerateCompendium(MainWindow.dataLoader);
        compendiumTextBlock = textBlock;
    }

    private void CompendiumBtnSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (compendiumTextBlock == null)
        {
            return;
        }

        string textToSave = compendiumTextBlock.Text;
        textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.SaveTextFile(snackbar, textToSave, "Compendium");
    }

    private void CompendiumBtnCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (compendiumInformationStackPanel == null)
        {
            return;
        }

        var previousBackground = compendiumInformationStackPanel.Background;
        compendiumInformationStackPanel.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(compendiumInformationStackPanel, snackbar);
        compendiumInformationStackPanel.Background = previousBackground;
    }

    // TODO: put in file manager class?
    private void CalendarButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var data = MainWindow.dataLoader.model.CalendarRuns;
            if (data == null)
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.Title = "Save Calendar Runs as CSV";
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            string dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            saveFileDialog.FileName = string.Format(CultureInfo.InvariantCulture, "CalendarRuns-{0}", datePickerDate.ToString("yy/MM/dd", CultureInfo.InvariantCulture).Replace("/", "-"));
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                SaveCSVFromListOfRecentRuns(data, filePath);
                logger.Info(CultureInfo.InvariantCulture, "Saved text {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not save text file");
        }
    }

    // TODO to another class
    public void SaveCSVFromListOfRecentRuns(List<RecentRuns> recentRuns, string filePath)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Objective Image,Quest Name,Run ID,Quest ID,Youtube ID,Final Time Display,Date,Actual Overlay Mode,Party Size");

        foreach (var run in recentRuns)
        {
            string objectiveImage = run.ObjectiveImage.Replace(",", "");
            string questName = run.QuestName.Replace(",", "");
            string youtubeID = run.YoutubeID.Replace(",", "");
            string finalTimeDisplay = run.FinalTimeDisplay.Replace(",", "");
            string actualOverlayMode = run.ActualOverlayMode.Replace(",", "");

            string line = $"{objectiveImage},{questName},{run.RunID},{run.QuestID},{youtubeID},{finalTimeDisplay},{run.Date},{actualOverlayMode},{run.PartySize}";
            sb.AppendLine(line);
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    public void SaveCSVFromWeaponUsage(List<WeaponUsage> weaponUsageData, string filePath)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Weapon Type,Style,Run Count");

        foreach (var WeaponUsageMapper in weaponUsageData)
        {
            string weaponType = WeaponUsageMapper.WeaponType.Replace(",", "");
            string style = WeaponUsageMapper.Style.Replace(",", "");
            string runCount = WeaponUsageMapper.RunCount.ToString(CultureInfo.InvariantCulture).Replace(",", "");

            string line = $"{weaponType},{style},{runCount}";
            sb.AppendLine(line);
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    public void SaveCSVFromListOfRecentRuns(ObservableCollection<RecentRuns> recentRuns, string filePath)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Objective Image,Quest Name,Run ID,Quest ID,Youtube ID,Final Time Display,Date,Actual Overlay Mode,Party Size");

        foreach (var run in recentRuns)
        {
            string objectiveImage = run.ObjectiveImage.Replace(",", "");
            string questName = run.QuestName.Replace(",", "");
            string youtubeID = run.YoutubeID.Replace(",", "");
            string finalTimeDisplay = run.FinalTimeDisplay.Replace(",", "");
            string actualOverlayMode = run.ActualOverlayMode.Replace(",", "");

            string line = $"{objectiveImage},{questName},{run.RunID},{run.QuestID},{youtubeID},{finalTimeDisplay},{run.Date},{actualOverlayMode},{run.PartySize}";
            sb.AppendLine(line);
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    public void SaveCSVFromListOfFastestRuns(List<FastestRun> fastestRuns, string filePath)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Objective Image,Quest Name,Run ID,Quest ID,Youtube ID,Final Time Display,Date");

        foreach (var run in fastestRuns)
        {
            string objectiveImage = run.ObjectiveImage.Replace(",", "");
            string questName = run.QuestName.Replace(",", "");
            string youtubeID = run.YoutubeID.Replace(",", "");
            string finalTimeDisplay = run.FinalTimeDisplay.Replace(",", "");

            string line = $"{objectiveImage},{questName},{run.RunID},{run.QuestID},{youtubeID},{finalTimeDisplay},{run.Date}";
            sb.AppendLine(line);
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    private void CalendarButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (calendarDataGrid == null)
        {
            return;
        }

        var previousBackground = calendarDataGrid.Background;
        calendarDataGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(calendarDataGrid, snackbar);
        calendarDataGrid.Background = previousBackground;
    }

    private void PersonalBestButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (personalBestChart == null || personalBestChartGrid == null || personalBestMainGrid == null)
        {
            return;
        }

        var fileName = $"PersonalBest-Quest_{QuestIDTextBox.Text}-{OverlayModeComboBox.Text}-{personalBestSelectedType}-{personalBestSelectedWeapon}".Trim().Replace(" ", "_");
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.SaveElementAsImageFile(personalBestMainGrid, fileName, snackbar, false);
    }

    private void PersonalBestButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (personalBestChartGrid == null || personalBestMainGrid == null)
        {
            return;
        }

        var previousBackground = personalBestMainGrid.Background;
        personalBestMainGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(personalBestMainGrid, snackbar);
        personalBestMainGrid.Background = previousBackground;
    }

    private void Top20ButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var data = MainWindow.dataLoader.model.FastestRuns;
            if (data == null)
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.Title = "Save Fastest Runs as CSV";
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            string dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            saveFileDialog.FileName = string.Format(CultureInfo.InvariantCulture, "FastestRuns-Quest_{0}-{1}-{2}-{3}", QuestIDTextBox.Text, OverlayModeComboBox.Text, top20RunsSelectedWeapon, DateTime.UtcNow.ToString("yy/MM/dd", CultureInfo.InvariantCulture).Replace("/", "-"));
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                SaveCSVFromListOfFastestRuns(data, filePath);
                logger.Info(CultureInfo.InvariantCulture, "Saved text {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not save text file");
        }
    }

    private void Top20ButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (top20RunsDataGrid == null || top20MainGrid == null)
        {
            return;
        }

        var previousBackground = top20MainGrid.Background;
        top20MainGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(top20MainGrid, snackbar);
        top20MainGrid.Background = previousBackground;
    }

    private void WeaponStatsButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (weaponUsageChartGrid == null || weaponUsageChart == null || weaponUsageData == null || weaponStatsMainGrid == null)
        {
            return;
        }

        try
        {
            var data = MainWindow.dataLoader.model.CalendarRuns;
            if (data == null)
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.Title = "Save Weapon Stats as CSV";
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            string dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            saveFileDialog.FileName = string.Format(CultureInfo.InvariantCulture, "WeaponUsage-{0}", dateTime);
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                SaveCSVFromWeaponUsage(weaponUsageData, filePath);
                logger.Info(CultureInfo.InvariantCulture, "Saved text {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not save text file");
        }
    }

    private void WeaponStatsButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (weaponUsageChartGrid == null || weaponStatsMainGrid == null)
        {
            return;
        }

        var previousBackground = weaponStatsMainGrid.Background;
        weaponStatsMainGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(weaponStatsMainGrid, snackbar);
        weaponStatsMainGrid.Background = previousBackground;
    }

    private void MostRecentButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var data = MainWindow.dataLoader.model.RecentRuns;
            if (data == null)
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.Title = "Save Recent Runs as CSV";
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            string dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            saveFileDialog.FileName = string.Format(CultureInfo.InvariantCulture, "RecentRuns-{0}", dateTime);
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                SaveCSVFromListOfRecentRuns(data, filePath);
                logger.Info(CultureInfo.InvariantCulture, "Saved text {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Could not save text file");
        }
    }

    private void MostRecentButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (mostRecentRunsDataGrid == null)
        {
            return;
        }

        var previousBackground = mostRecentRunsDataGrid.Background;
        mostRecentRunsDataGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(mostRecentRunsDataGrid, snackbar);
        mostRecentRunsDataGrid.Background = previousBackground;
    }

    private void StatsGraphsButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (statsGraphsGrid == null || statsGraphsMainGrid == null)
        {
            return;
        }

        var fileName = $"StatsGraphs-{statsGraphsSelectedOption}";
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.SaveElementAsImageFile(statsGraphsMainGrid, fileName, snackbar, false);
    }

    private void StatsGraphsButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (statsGraphsGrid == null || statsGraphsMainGrid == null)
        {
            return;
        }

        var previousBackground = statsGraphsMainGrid.Background;
        statsGraphsMainGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(statsGraphsMainGrid, snackbar);
        statsGraphsMainGrid.Background = previousBackground;
    }

    private void StatsTextButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (statsTextTextBlock == null || statsTextMainGrid == null)
        {
            return;
        }

        string textToSave = statsTextTextBlock.Text;
        textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.SaveTextFile(snackbar, textToSave, $"StatsText-Run_{RunIDTextBox.Text}-{statsTextSelectedOption}");
    }

    private void StatsTextButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (statsTextTextBlock == null || statsTextMainGrid == null)
        {
            return;
        }

        var previousBackground = statsTextTextBlock.Background;
        statsTextTextBlock.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(statsTextTextBlock, snackbar);
        statsTextTextBlock.Background = previousBackground;
    }

    private void PersonalBestsOverviewButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (DiscordEmbedWeaponPersonalBest == null || QuestIDTextBox == null)
        {
            return;
        }

        var fileName = $"PersonalBestsOverview-Quest_{QuestIDTextBox.Text}-{DateTime.UtcNow.ToString("yy/MM/dd", CultureInfo.InvariantCulture).Replace("/", "-")}";
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.SaveElementAsImageFile(DiscordEmbedWeaponPersonalBest, fileName, snackbar, false);
    }

    private void PersonalBestsOverviewButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (DiscordEmbedWeaponPersonalBest == null)
        {
            return;
        }

        var previousBackground = DiscordEmbedWeaponPersonalBest.Background;
        DiscordEmbedWeaponPersonalBest.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        FileService.CopyUIElementToClipboard(DiscordEmbedWeaponPersonalBest, snackbar);
        DiscordEmbedWeaponPersonalBest.Background = previousBackground;
    }

    private ISeries[]? Series { get; set; }

    private Axis[]? XAxes { get; set; }

    private Axis[]? YAxes { get; set; }

    private ISeries[]? PersonalBestSeries { get; set; }

    private Axis[]? PersonalBestXAxes { get; set; }

    private Axis[]? PersonalBestYAxes { get; set; }

    private void SetColumnSeriesForDictionaryIntInt(Dictionary<int, int> data)
    {
        Series = new ISeries[data.Count];
        int i = 0;
        foreach (var entry in data)
        {
            Series[i] = new ColumnSeries<double>
            {
                Name = entry.Key.ToString(CultureInfo.InvariantCulture),
                Values = new double[] { entry.Value },
            };

            i++;
        }

        XAxes = new Axis[]
        {
            new Axis
            {
                Labels = data.Keys.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray(),
                LabelsRotation = 0,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };
    }

    private void SetColumnSeriesForDictionaryStringInt(Dictionary<string, int> data)
    {
        Series = new ISeries[data.Count];
        int i = 0;
        foreach (var entry in data)
        {
            Series[i] = new ColumnSeries<double>
            {
                Name = entry.Key.ToString(),
                Values = new double[] { entry.Value }
            };

            i++;
        }

        XAxes = new Axis[]
        {
            new Axis
            {
                Labels = data.Keys.Select(x => x.ToString()).ToArray(),
                LabelsRotation = 0,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };
    }

    private void SetColumnSeriesForDictionaryDateInt(Dictionary<DateTime, int> data)
    {
        Series = new ISeries[data.Count];
        int i = 0;
        foreach (var entry in data)
        {
            Series[i] = new ColumnSeries<double>
            {
                Name = entry.Key.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Values = new double[] { entry.Value }
            };

            i++;
        }

        XAxes = new Axis[]
        {
    new Axis
    {
        Labels = data.Keys.Select(x => x.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).ToArray(),
        LabelsRotation = 0,
        SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
        TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
        NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
        LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
    }
        };
    }

    private Dictionary<int, int> GetElapsedTime(Dictionary<int, int> timeAttackDict)
    {
        Dictionary<int, int> elapsedTimeDict = new ();
        if (timeAttackDict == null || !timeAttackDict.Any())
        {
            return elapsedTimeDict;
        }

        int initialTime = timeAttackDict.First().Key;
        foreach (var entry in timeAttackDict)
        {
            elapsedTimeDict[initialTime - entry.Key] = entry.Value;
        }

        return elapsedTimeDict;
    }

    private Dictionary<int, double> GetElapsedTimeForDictionaryIntDouble(Dictionary<int, double> timeAttackDict)
    {
        Dictionary<int, double> elapsedTimeDict = new ();
        if (timeAttackDict == null || !timeAttackDict.Any())
        {
            return elapsedTimeDict;
        }

        int initialTime = timeAttackDict.First().Key;
        foreach (var entry in timeAttackDict)
        {
            elapsedTimeDict[initialTime - entry.Key] = entry.Value;
        }

        return elapsedTimeDict;
    }

    private void SetLineSeriesForDictionaryIntInt(Dictionary<int, int> data)
    {
        if (graphChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<ObservablePoint> collection = new ();

        Dictionary<int, int> newData = GetElapsedTime(data);

        foreach (var entry in newData)
        {
            collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        XAxes = new Axis[]
        {
            new Axis
            {
                TextSize=12,
                Labeler = (value) => MainWindow.dataLoader.model.GetTimeElapsed(value),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize= 12,
                TextSize=12,
                NamePadding= new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        graphChart.Series = series;
        graphChart.XAxes = XAxes;
        graphChart.YAxes = YAxes;
    }

    private void SetLineSeriesForDictionaryIntDouble(Dictionary<int, double> data)
    {
        if (graphChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<ObservablePoint> collection = new ();

        Dictionary<int, double> newData = GetElapsedTimeForDictionaryIntDouble(data);

        foreach (var entry in newData)
        {
            collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        XAxes = new Axis[]
        {
            new Axis
            {
                TextSize=12,
                Labeler = (value) => MainWindow.dataLoader.model.GetTimeElapsed(value),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        graphChart.Series = series;
        graphChart.XAxes = XAxes;
        graphChart.YAxes = YAxes;
    }

    private void SetStepLineSeriesForPersonalBestByAttempts(Dictionary<long, long> data)
    {
        if (personalBestChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<ObservablePoint> collection = new ();

        foreach (var entry in data)
        {
            collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new StepLineSeries<ObservablePoint>
        {
            Values = collection,
            TooltipLabelFormatter = (chartPoint) =>
            $"Attempt {chartPoint.SecondaryValue}: {MainWindow.dataLoader.model.GetMinutesSecondsMillisecondsFromFrames((long)chartPoint.PrimaryValue)}",
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
        });

        PersonalBestXAxes = new Axis[]
        {
            new Axis
            {
                MinStep = 1,
                TextSize = 12,
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        PersonalBestYAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                NameTextSize = 12,
                MinStep = 1,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        personalBestChart.Series = series;
        personalBestChart.XAxes = PersonalBestXAxes;
        personalBestChart.YAxes = PersonalBestYAxes;
    }

    private void SetStepLineSeriesForPersonalBestByDate(Dictionary<DateTime, long> data)
    {
        if (personalBestChart == null)
        {
            return;
        }

        List<ISeries> series = new ();

        ObservableCollection<DateTimePoint> collection = new ();

        DateTime? prevDate = null;
        long? prevTime = null;

        foreach (var entry in data.OrderBy(e => e.Key))
        {
            var date = entry.Key;
            var time = entry.Value;

            // Fill in missing dates with the last known personal best time
            if (prevDate != null && prevTime != null && date > prevDate.Value.AddDays(1))
            {
                collection.Add(new DateTimePoint(prevDate.Value.AddDays(1), prevTime.Value));
            }

            collection.Add(new DateTimePoint(date, time));

            prevDate = date;
            prevTime = time;
        }

        series.Add(new StepLineSeries<DateTimePoint>
        {
            Values = collection,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            TooltipLabelFormatter = (chartPoint) =>
            $"{new DateTime((long)chartPoint.SecondaryValue):yy-MM-dd}: {MainWindow.dataLoader.model.GetMinutesSecondsMillisecondsFromFrames((long)chartPoint.PrimaryValue)}",
        });

        PersonalBestXAxes = new Axis[]
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("yy-MM-dd", CultureInfo.InvariantCulture),
                LabelsRotation = 80,
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),

                // when using a date time type, let the library know your unit 
                UnitWidth = TimeSpan.FromDays(1).Ticks, 

                // if the difference between our points is in hours then we would do:
                // UnitWidth = TimeSpan.FromHours(1).Ticks,

                // since all the months and years have a different number of days
                // we can use the average, it would not cause any visible error in the user interface
                // Months: TimeSpan.FromDays(30.4375).Ticks
                // Years: TimeSpan.FromDays(365.25).Ticks

                // The MinStep property forces the separator to be greater than 1 day.
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };

        PersonalBestYAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                MinStep = 1,
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        personalBestChart.Series = series;
        personalBestChart.XAxes = PersonalBestXAxes;
        personalBestChart.YAxes = PersonalBestYAxes;
    }

    private void SetHitsTakenBlocked(Dictionary<int, Dictionary<int, int>> data)
    {
        if (graphChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<ObservablePoint> collection = new ();

        Dictionary<int, int> hitsTakenBlocked = CalculateHitsTakenBlocked(data);

        Dictionary<int, int> newData = GetElapsedTime(hitsTakenBlocked);

        foreach (var entry in newData)
        {
            collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
        });

        XAxes = new Axis[]
        {
            new Axis
            {
                TextSize=12,
                Labeler = (value) => MainWindow.dataLoader.model.GetTimeElapsed(value),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize= 12,
                TextSize=12,
                NamePadding= new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        graphChart.Series = series;
        graphChart.XAxes = XAxes;
        graphChart.YAxes = YAxes;
    }

    public void SetPlayerHealthStamina(Dictionary<int, int> hp, Dictionary<int, int> stamina)
    {
        if (graphChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<ObservablePoint> healthCollection = new ();
        ObservableCollection<ObservablePoint> staminaCollection = new ();

        Dictionary<int, int> newHP = GetElapsedTime(hp);
        Dictionary<int, int> newStamina = GetElapsedTime(stamina);

        foreach (var entry in newHP)
        {
            healthCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        foreach (var entry in newStamina)
        {
            staminaCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = healthCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) =>
            $"Health: {((long)chartPoint.PrimaryValue)}",
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#ffa6e3a1"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#ffa6e3a1", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#ffa6e3a1", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = staminaCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) =>
            $"Stamina: {((long)chartPoint.PrimaryValue)}",
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff9e2af"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff9e2af", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff9e2af", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        XAxes = new Axis[]
        {
            new Axis
            {
                TextSize=12,
                Labeler = (value) => MainWindow.dataLoader.model.GetTimeElapsed(value),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize= 12,
                TextSize=12,
                NamePadding= new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        graphChart.Series = series;
        graphChart.XAxes = XAxes;
        graphChart.YAxes = YAxes;
    }

    public void SetMonsterAttackMultiplier(Dictionary<int, double> attack)
    {
        if (graphChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<ObservablePoint> attackCollection = new ();

        Dictionary<int, double> newAttack = GetElapsedTimeForDictionaryIntDouble(attack);

        foreach (var entry in newAttack)
        {
            attackCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new StepLineSeries<ObservablePoint>
        {
            Values = attackCollection,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#f38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#f38ba8", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#f38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        XAxes = new Axis[]
        {
            new Axis
            {
                TextSize=12,
                Labeler = (value) => MainWindow.dataLoader.model.GetTimeElapsed(value),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize= 12,
                TextSize=12,
                NamePadding= new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        graphChart.Series = series;
        graphChart.XAxes = XAxes;
        graphChart.YAxes = YAxes;
    }

    public void SetMonsterDefenseRate(Dictionary<int, double> defense)
    {
        if (graphChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<ObservablePoint> defenseCollection = new ();

        Dictionary<int, double> newDefense = GetElapsedTimeForDictionaryIntDouble(defense);

        foreach (var entry in newDefense)
        {
            defenseCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new StepLineSeries<ObservablePoint>
        {
            Values = defenseCollection,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#74c7ec"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#74c7ec", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#74c7ec", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        XAxes = new Axis[]
        {
            new Axis
            {
                TextSize=12,
                Labeler = (value) => MainWindow.dataLoader.model.GetTimeElapsed(value),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize= 12,
                TextSize=12,
                NamePadding= new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        graphChart.Series = series;
        graphChart.XAxes = XAxes;
        graphChart.YAxes = YAxes;
    }

    private void SetMonsterStatusAilmentsThresholds(Dictionary<int, int> poison, Dictionary<int, int> sleep, Dictionary<int, int> para, Dictionary<int, int> blast, Dictionary<int, int> stun)
    {
        if (graphChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<ObservablePoint> poisonCollection = new ();
        ObservableCollection<ObservablePoint> sleepCollection = new ();
        ObservableCollection<ObservablePoint> paraCollection = new ();
        ObservableCollection<ObservablePoint> blastCollection = new ();
        ObservableCollection<ObservablePoint> stunCollection = new ();

        Dictionary<int, int> newPoison = GetElapsedTime(poison);
        Dictionary<int, int> newSleep = GetElapsedTime(sleep);
        Dictionary<int, int> newPara = GetElapsedTime(para);
        Dictionary<int, int> newBlast = GetElapsedTime(blast);
        Dictionary<int, int> newStun = GetElapsedTime(stun);

        foreach (var entry in newPoison)
        {
            poisonCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        foreach (var entry in newSleep)
        {
            sleepCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        foreach (var entry in newPara)
        {
            paraCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        foreach (var entry in newBlast)
        {
            blastCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        foreach (var entry in newStun)
        {
            stunCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = poisonCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) =>
            $"Poison: {((long)chartPoint.PrimaryValue)}",
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#cba6f7"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#cba6f7", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#cba6f7", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = sleepCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) => $"Sleep: {((long)chartPoint.PrimaryValue)}",
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#74c7ec"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#74c7ec", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#74c7ec", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = paraCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) => $"Paralysis: {((long)chartPoint.PrimaryValue)}",
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#f9e2af"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#f9e2af", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#f9e2af", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = blastCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) => $"Blast: {((long)chartPoint.PrimaryValue)}",
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6e3a1"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6e3a1", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6e3a1", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = stunCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) => $"Stun: {((long)chartPoint.PrimaryValue)}",
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#7f849c"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#7f849c", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#7f849c", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        XAxes = new Axis[]
        {
            new Axis
            {
                TextSize=12,
                Labeler = (value) => MainWindow.dataLoader.model.GetTimeElapsed(value),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize= 12,
                TextSize=12,
                NamePadding= new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        graphChart.Series = series;
        graphChart.XAxes = XAxes;
        graphChart.YAxes = YAxes;
    }

    private void CreateQuestDurationStackedChart(Dictionary<int, int> questDurations)
    {
        var series = new List<StackedColumnSeries<int>>();

        foreach (var questDuration in questDurations)
        {
            series.Add(new StackedColumnSeries<int>
            {
                Values = new List<int> { questDuration.Value },
                Name = questDuration.Key.ToString(CultureInfo.InvariantCulture),
                DataLabelsPosition = DataLabelsPosition.Middle,
                DataLabelsSize = 6,
                TooltipLabelFormatter = value => questDuration.Key.ToString(CultureInfo.InvariantCulture) + " " + TimeSpan.FromSeconds(value.PrimaryValue / Double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture),
                DataLabelsFormatter = value => TimeSpan.FromSeconds(value.PrimaryValue / Double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture)
            });
        }

        Series = series.ToArray();
        YAxes = new Axis[]
        {
            new Axis
            {
                Labeler = (value) => TimeSpan.FromSeconds(value / Double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture),
                LabelsRotation = 0,
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
            }
        };
    }

    // TODO: gamepad test
    private Dictionary<string, int> GetMostCommonInputs(long runID)
    {
        var keystrokesDictionary = databaseManager.GetKeystrokesDictionary(runID);
        var mouseInputDictionary = databaseManager.GetMouseInputDictionary(runID);
        var gamepadInputDictionary = databaseManager.GetGamepadInputDictionary(runID);
        var combinedDictionary = keystrokesDictionary
            .Union(mouseInputDictionary)
            .Union(gamepadInputDictionary)
            .GroupBy(kvp => kvp.Value)
            .ToDictionary(g => g.Key, g => g.Count());

        return combinedDictionary
            .OrderByDescending(kvp => kvp.Value)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private Dictionary<int, int> CalculateHitsTakenBlocked(Dictionary<int, Dictionary<int, int>> hitsTakenBlocked)
    {
        Dictionary<int, int> dictionary = new ();

        int i = 1;
        foreach (var entry in hitsTakenBlocked)
        {
            int time = int.Parse(entry.Key.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            int count = i;
            dictionary.Add(time, count);
            i++;
        }

        return dictionary;
    }

    private Dictionary<int, int> CalculateMonsterHP(Dictionary<int, Dictionary<int, int>> monsterHP)
    {
        Dictionary<int, int> dictionary = new ();

        int i = 1;
        foreach (var entry in monsterHP)
        {
            int time = int.Parse(entry.Key.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

            // get the value of the inner dictionary
            int hp = entry.Value.Values.First();
            dictionary.Add(time, hp);
            i++;
        }

        return dictionary;
    }

    private Dictionary<int, double> CalculateMonsterMultiplier(Dictionary<int, Dictionary<int, double>> monsterDictionary)
    {
        Dictionary<int, double> dictionary = new ();

        int i = 1;
        foreach (var entry in monsterDictionary)
        {
            int time = int.Parse(entry.Key.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

            // get the value of the inner dictionary
            double mult = entry.Value.Values.First();
            dictionary.Add(time, mult);
            i++;
        }

        return dictionary;
    }

    private Dictionary<int, int> CalculateMonsterStatusAilmentThresholds(Dictionary<int, Dictionary<int, int>> monsterDictionary)
    {
        Dictionary<int, int> dictionary = new ();

        int i = 1;
        foreach (var entry in monsterDictionary)
        {
            int time = int.Parse(entry.Key.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

            // get the value of the inner dictionary
            int threshold = entry.Value.Values.First();
            dictionary.Add(time, threshold);
            i++;
        }

        return dictionary;
    }

    private void SetMonsterHP(Dictionary<int, int> monster1, Dictionary<int, int> monster2, Dictionary<int, int> monster3, Dictionary<int, int> monster4)
    {
        if (graphChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<ObservablePoint> monster1Collection = new ();
        ObservableCollection<ObservablePoint> monster2Collection = new ();
        ObservableCollection<ObservablePoint> monster3Collection = new ();
        ObservableCollection<ObservablePoint> monster4Collection = new ();

        Dictionary<int, int> newMonster1 = GetElapsedTime(monster1);
        Dictionary<int, int> newMonster2 = GetElapsedTime(monster2);
        Dictionary<int, int> newMonster3 = GetElapsedTime(monster3);
        Dictionary<int, int> newMonster4 = GetElapsedTime(monster4);

        foreach (var entry in newMonster1)
        {
            monster1Collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        foreach (var entry in newMonster2)
        {
            monster2Collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        foreach (var entry in newMonster3)
        {
            monster3Collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        foreach (var entry in newMonster4)
        {
            monster4Collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = monster1Collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1))
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = monster2Collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff9e2af"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff9e2af", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff9e2af", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = monster3Collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#ff94e2d5"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#ff94e2d5", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#ff94e2d5", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = monster4Collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#ffcba6f7"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#ffcba6f7", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#ffcba6f7", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        XAxes = new Axis[]
        {
            new Axis
            {
                TextSize=12,
                Labeler = (value) => MainWindow.dataLoader.model.GetTimeElapsed(value),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize= 12,
                TextSize=12,
                NamePadding= new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        graphChart.Series = series;
        graphChart.XAxes = XAxes;
        graphChart.YAxes = YAxes;
    }

    private string GetHunterPerformanceValueType(double value)
    {
        switch (value)
        {
            case 0:
                return "True Raw";
            case 1:
                return "DPH";
            case 2:
                return "DPS";
            case 3:
                return "Hits/s";
            case 4:
                return "Blocked Hits/s";
            case 5:
                return "APM";
            default:
                return "None";
        }
    }

    private void SetPolarLineSeriesForHunterPerformance(PerformanceCompendium performanceCompendium)
    {
        if (hunterPerformanceChart == null)
        {
            return;
        }

        List<ISeries> series = new ();
        ObservableCollection<double> performanceCollection = new ();

        performanceCollection.Add(performanceCompendium.TrueRawMedian / performanceCompendium.HighestTrueRaw);
        performanceCollection.Add(performanceCompendium.SingleHitDamageMedian / performanceCompendium.HighestSingleHitDamage);
        performanceCollection.Add(performanceCompendium.DPSMedian / performanceCompendium.HighestDPS);
        performanceCollection.Add(performanceCompendium.HitsPerSecondMedian / performanceCompendium.HighestHitsPerSecond);
        performanceCollection.Add(performanceCompendium.HitsTakenBlockedPerSecondMedian / performanceCompendium.HighestHitsTakenBlockedPerSecond);
        performanceCollection.Add(performanceCompendium.ActionsPerMinuteMedian / performanceCompendium.HighestActionsPerMinute);

        series.Add(new PolarLineSeries<double>
        {
            Values = performanceCollection,
            LineSmoothness = 0,
            GeometrySize = 0,
            IsClosed = true,
            GeometryFill = null,
            GeometryStroke = null,
            TooltipLabelFormatter = value => string.Format(CultureInfo.InvariantCulture, "{0}: {1:0.##}%", GetHunterPerformanceValueType(value.SecondaryValue), value.PrimaryValue * 100),
            Stroke = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        PolarAxis[] RadiusAxes = new PolarAxis[]
        {
            new PolarAxis
            {
                // formats the value as a number with 2 decimals.
                Labeler = value => string.Format(CultureInfo.InvariantCulture, "{0:0}%", value * 100),
                LabelsBackground = new LiveChartsCore.Drawing.LvcColor(0x1e,0x1e,0x2e,0xa8),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#cdd6f4a8")))
            }
        };

        PolarAxis[] AngleAxes = new PolarAxis[]
        {
            new PolarAxis
            {
                Labels = new[] { "TRaw", "DPH", "DPS", "Hits/s", "Blocks/s", "APM" },
                LabelsBackground = new LiveChartsCore.Drawing.LvcColor(0x1e,0x1e,0x2e,0xa8),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#cdd6f4a8"))),
                MinStep = 1,
                ForceStepToMin = true
            }
        };

        hunterPerformanceChart.AngleAxes = AngleAxes;
        hunterPerformanceChart.RadiusAxes = RadiusAxes;
        hunterPerformanceChart.Series = series;
    }

    private void GraphsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;

        var selectedItem = (ComboBoxItem)comboBox.SelectedItem;

        if (selectedItem == null)
        {
            return;
        }

        string? selectedOption = selectedItem.Content.ToString();

        if (graphChart == null || selectedOption == null || string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        Series = null;
        XAxes = new Axis[]
        {
            new Axis
            {
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };
        YAxes = new Axis[]
        {
            new Axis
            {
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        long runID = long.Parse(RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);

        switch (selectedOption)
        {
            case "(General) Most Quest Completions":
                SetColumnSeriesForDictionaryIntInt(databaseManager.GetMostQuestCompletions());
                break;
            case "(General) Quest Durations":
                CreateQuestDurationStackedChart(databaseManager.GetTotalTimeSpentInQuests());
                break;
            case "(General) Most Common Objective Types":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonObjectiveTypes());
                break;
            case "(General) Most Common Star Grades":
                SetColumnSeriesForDictionaryIntInt(databaseManager.GetMostCommonStarGrades());
                break;
            case "(General) Most Common Rank Bands":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonRankBands());
                break;
            case "(General) Most Common Objective":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonObjectives());
                break;
            case "(General) Quests Completed by Date":
                SetColumnSeriesForDictionaryDateInt(databaseManager.GetQuestsCompletedByDate());
                break;
            case "(General) Most Common Party Size":
                SetColumnSeriesForDictionaryIntInt(databaseManager.GetMostCommonPartySize());
                break;
            case "(General) Most Common Set Name":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonSetNames());
                break;
            case "(General) Most Common Weapon Name":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonWeaponNames());
                break;
            case "(General) Most Common Head Piece":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonHeadPieces());
                break;
            case "(General) Most Common Chest Piece":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonChestPieces());
                break;
            case "(General) Most Common Arms Piece":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonArmsPieces());
                break;
            case "(General) Most Common Waist Piece":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonWaistPieces());
                break;
            case "(General) Most Common Legs Piece":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonLegsPieces());
                break;
            case "(General) Most Common Diva Skill":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonDivaSkill());
                break;
            case "(General) Most Common Guild Food":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonGuildFood());
                break;
            case "(General) Most Common Style Rank Skills":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonStyleRankSkills());
                break;
            case "(General) Most Common Caravan Skills":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonCaravanSkills());
                break;
            case "(General) Most Common Category":
                SetColumnSeriesForDictionaryStringInt(databaseManager.GetMostCommonCategory());
                break;
            case "(Run ID) Attack Buff":
                SetLineSeriesForDictionaryIntInt(databaseManager.GetAttackBuffDictionary(runID));
                return;
            case "(Run ID) Hit Count":
                SetLineSeriesForDictionaryIntInt(databaseManager.GetHitCountDictionary(runID));
                return;
            case "(Run ID) Hits per Second":
                SetLineSeriesForDictionaryIntDouble(databaseManager.GetHitsPerSecondDictionary(runID));
                return;
            case "(Run ID) Damage Dealt":
                SetLineSeriesForDictionaryIntInt(databaseManager.GetDamageDealtDictionary(runID));
                return;
            case "(Run ID) Damage per Second":
                SetLineSeriesForDictionaryIntDouble(databaseManager.GetDamagePerSecondDictionary(runID));
                return;
            case "(Run ID) Carts":
                SetLineSeriesForDictionaryIntInt(databaseManager.GetCartsDictionary(runID));
                return;
            case "(Run ID) Hits Taken/Blocked":
                SetHitsTakenBlocked(databaseManager.GetHitsTakenBlockedDictionary(runID));
                return;
            case "(Run ID) Hits Taken/Blocked per Second":
                SetLineSeriesForDictionaryIntDouble(databaseManager.GetHitsTakenBlockedPerSecondDictionary(runID));
                return;
            case "(Run ID) Player Health and Stamina":
                SetPlayerHealthStamina(databaseManager.GetPlayerHPDictionary(runID), databaseManager.GetPlayerStaminaDictionary(runID));
                return;
            case "(Run ID) Most Common Player Input":
                SetColumnSeriesForDictionaryStringInt(GetMostCommonInputs(runID));
                break;
            case "(Run ID) Actions per Minute":
                SetLineSeriesForDictionaryIntDouble(databaseManager.GetActionsPerMinuteDictionary(runID));
                return;
            case "(Run ID) Monster HP":
                SetMonsterHP(CalculateMonsterHP(databaseManager.GetMonster1HPDictionary(runID)), CalculateMonsterHP(databaseManager.GetMonster2HPDictionary(runID)), CalculateMonsterHP(databaseManager.GetMonster3HPDictionary(runID)), CalculateMonsterHP(databaseManager.GetMonster4HPDictionary(runID)));
                return;
            case "(Run ID) Monster Attack Multiplier":
                SetMonsterAttackMultiplier(CalculateMonsterMultiplier(databaseManager.GetMonster1AttackMultiplierDictionary(runID)));
                return;
            case "(Run ID) Monster Defense Rate":
                SetMonsterDefenseRate(CalculateMonsterMultiplier(databaseManager.GetMonster1DefenseRateDictionary(runID)));
                return;
            case "(Run ID) Monster Status Ailments Thresholds":
                SetMonsterStatusAilmentsThresholds(
                    CalculateMonsterStatusAilmentThresholds(
                        databaseManager.GetMonster1PoisonThresholdDictionary(runID)),
                    CalculateMonsterStatusAilmentThresholds(
                        databaseManager.GetMonster1SleepThresholdDictionary(runID)),
                    CalculateMonsterStatusAilmentThresholds(
                        databaseManager.GetMonster1ParalysisThresholdDictionary(runID)),
                    CalculateMonsterStatusAilmentThresholds(
                        databaseManager.GetMonster1BlastThresholdDictionary(runID)),
                    CalculateMonsterStatusAilmentThresholds(
                        databaseManager.GetMonster1StunThresholdDictionary(runID)
                        )
                    );
                return;
        }

        statsGraphsSelectedOption = selectedOption.Trim().Replace(" ", "_");

        if (Series == null)
        {
            return;
        }

        graphChart.Series = Series;
        graphChart.XAxes = XAxes;
        graphChart.YAxes = YAxes;
    }

    private void GraphsChart_Loaded(object sender, RoutedEventArgs e)
    {
        graphChart = (CartesianChart)sender;
    }

    private void StatsTextTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        statsTextTextBlock = (TextBlock)sender;
    }

    private Dictionary<int, List<Dictionary<int, int>>> GetElapsedTimeForInventories(Dictionary<int, List<Dictionary<int, int>>> dictionary)
    {
        Dictionary<int, List<Dictionary<int, int>>> elapsedTimeDict = new ();
        if (dictionary == null || !dictionary.Any())
        {
            return elapsedTimeDict;
        }

        int initialTime = dictionary.First().Key;
        foreach (var entry in dictionary)
        {
            elapsedTimeDict[initialTime - entry.Key] = entry.Value;
        }

        return elapsedTimeDict;
    }

    private Dictionary<int, int> GetElapsedTimeForDictionaryIntInt(Dictionary<int, int> dictionary)
    {
        Dictionary<int, int> elapsedTimeDict = new ();

        if (dictionary == null || !dictionary.Any())
        {
            return elapsedTimeDict;
        }

        int initialTime = dictionary.First().Key;
        foreach (var entry in dictionary)
        {
            elapsedTimeDict[initialTime - entry.Key] = entry.Value;
        }

        return elapsedTimeDict;
    }

    private string FormatInventory(Dictionary<int, List<Dictionary<int, int>>> inventory)
    {
        inventory = GetElapsedTimeForInventories(inventory);

        StringBuilder sb = new StringBuilder();

        foreach (var entry in inventory)
        {
            int time = entry.Key;
            string timeString = TimeSpan.FromSeconds((double)time / Numbers.FramesPerSecond).ToString(TimeFormats.MinutesSecondsMilliseconds, CultureInfo.InvariantCulture);
            var items = entry.Value;
            int count = 0;
            sb.AppendLine(timeString + " ");
            foreach (var item in items)
            {
                foreach (var itemData in item)
                {
                    if (itemData.Value > 0)
                    {
                        string itemName = GetItemName(itemData.Key);
                        sb.Append(itemName + " x" + itemData.Value + ", ");
                        count++;
                    }

                    if (count == 5)
                    {
                        sb.AppendLine();
                        count = 0;
                    }
                }
            }

            sb.AppendLine();
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private string DisplayAreaChanges(Dictionary<int, int> areas)
    {
        areas = GetElapsedTimeForDictionaryIntInt(areas);

        StringBuilder sb = new StringBuilder();

        foreach (var entry in areas)
        {
            int time = entry.Key;
            string timeString = TimeSpan.FromSeconds((double)time / Numbers.FramesPerSecond).ToString(TimeFormats.MinutesSecondsMilliseconds, CultureInfo.InvariantCulture);
            var area = entry.Value;
            sb.AppendLine(timeString + " ");

            Location.IDName.TryGetValue(area, out string? itemName);
            sb.Append(itemName);
            sb.AppendLine();
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private string GetItemName(int itemID)
    {
        // implement code to get item name based on itemID
        Item.IDName.TryGetValue(itemID, out string? value);
        if (value == null)
        {
            return string.Empty;
        }

        return value;
    }

    private void StatsTextComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;

        var selectedItem = (ComboBoxItem)comboBox.SelectedItem;

        if (selectedItem == null || statsTextTextBlock == null)
        {
            return;
        }

        string? selectedOption = selectedItem.Content.ToString();

        if (statsTextTextBlock == null || selectedOption == null || string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        statsTextTextBlock.Text = string.Empty;

        long runID = long.Parse(RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);

        switch (selectedOption)
        {
            case "Inventory":
                statsTextTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Inventory\n\n{0}", FormatInventory(databaseManager.GetPlayerInventoryDictionary(runID)));
                break;
            case "Ammo":
                statsTextTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Ammo\n\n{0}", FormatInventory(databaseManager.GetAmmoDictionary(runID)));
                break;
            case "Partnya Bag":
                statsTextTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Partnya Bag\n\n{0}", FormatInventory(databaseManager.GetPartnyaBagDictionary(runID)));
                break;
            case "Area Changes":
                statsTextTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Area Changes\n\n{0}", DisplayAreaChanges(databaseManager.GetAreaChangesDictionary(runID)));
                break;
        }

        statsTextSelectedOption = selectedOption.Trim().Replace(" ", "_");
    }

    // TODO: double-check the settings and the conditionals in the other code
    private void settingsPresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");

        ComboBox comboBox = (ComboBox)sender;

        var selectedItem = (ComboBoxItem)comboBox.SelectedItem;

        if (selectedItem == null)
        {
            return;
        }

        string? selectedOption = selectedItem.Content.ToString();

        if (string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        if (s != null)
        {
            overlaySettingsManager.SetConfigurationPreset(s, ConfigurationPresetConverter.Convert(selectedOption));
        }
    }

    private void personalBest_Loaded(object sender, RoutedEventArgs e)
    {
        personalBestChart = (CartesianChart)sender;
    }

    private void PersonalBestTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (personalBestChart == null)
        {
            return;
        }

        var comboBox = sender as ComboBox;
        if (comboBox == null)
        {
            return;
        }

        var selectedItem = comboBox.SelectedItem;

        if (selectedItem == null)
        {
            return;
        }

        string? selectedType = selectedItem.ToString();
        if (string.IsNullOrEmpty(selectedType))
        {
            return;
        }

        personalBestSelectedType = selectedType.Replace("System.Windows.Controls.ComboBoxItem: ", "");
    }

    private void PersonalBestWeaponComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (personalBestChart == null)
        {
            return;
        }

        var comboBox = sender as ComboBox;
        if (comboBox == null)
        {
            return;
        }

        var selectedItem = comboBox.SelectedItem;

        if (selectedItem == null)
        {
            return;
        }

        string? selectedWeapon = selectedItem.ToString();
        if (string.IsNullOrEmpty(selectedWeapon))
        {
            return;
        }

        personalBestSelectedWeapon = selectedWeapon.Replace("System.Windows.Controls.ComboBoxItem: ", "");
    }

    private void PersonalBestRefreshButton_Click(object sender, RoutedEventArgs e)
    {
        if (personalBestChart == null || personalBestSelectedWeapon == "" || personalBestSelectedType == "")
        {
            return;
        }

        PersonalBestSeries = null;
        PersonalBestXAxes = new Axis[]
        {
            new Axis
            {
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };
        PersonalBestYAxes = new Axis[]
        {
            new Axis
            {
                NamePaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(MainWindow.dataLoader.model.HexColorToDecimal("#a6adc8"))),
            }
        };

        long questID = long.Parse(QuestIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        int weaponTypeID = WeaponType.IDName.FirstOrDefault(x => x.Value == personalBestSelectedWeapon).Key;

        switch (personalBestSelectedType)
        {
            case "(Quest ID) Personal Best by Date":
                SetStepLineSeriesForPersonalBestByDate(databaseManager.GetPersonalBestsByDate(questID, weaponTypeID, OverlayModeComboBox.Text));
                break;
            case "(Quest ID) Personal Best by Attempts":
                SetStepLineSeriesForPersonalBestByAttempts(databaseManager.GetPersonalBestsByAttempts(questID, weaponTypeID, OverlayModeComboBox.Text));
                break;
            default:
                personalBestChart.Series = PersonalBestSeries ?? new ISeries[0]; // TODO test
                personalBestChart.XAxes = PersonalBestXAxes;
                personalBestChart.YAxes = PersonalBestYAxes;
                break;
        }

        if (personalBestDescriptionTextBlock != null)
        {
            personalBestDescriptionTextBlock.Text = $"Personal best of solo runs of quest ID {QuestIDTextBox.Text} by category {OverlayModeComboBox.Text}";
        }
    }

    private void HunterPerformancePolarChart_Loaded(object sender, RoutedEventArgs e)
    {
        var chart = sender as PolarChart;
        hunterPerformanceChart = chart;
        SetPolarLineSeriesForHunterPerformance(databaseManager.GetPerformanceCompendium());
    }

    private void CompendiumInformationStackPanel_Loaded(object sender, RoutedEventArgs e)
    {
        var stackPanel = sender as StackPanel;
        compendiumInformationStackPanel = stackPanel;
    }

    private DateTime datePickerDate = DateTime.UtcNow;

    private void CalendarDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if (calendarDataGrid == null || sender == null)
        {
            return;
        }

        DatePicker? datePicker = sender as DatePicker;

        if (datePicker == null)
        {
            return;
        }

        var selectedDate = datePicker.SelectedDate;

        if (selectedDate == null)
        {
            return;
        }

        datePickerDate = (DateTime)selectedDate;
        MainWindow.dataLoader.model.CalendarRuns = databaseManager.GetCalendarRuns(selectedDate);
        calendarDataGrid.ItemsSource = MainWindow.dataLoader.model.CalendarRuns;
        calendarDataGrid.Items.Refresh();
    }

    private void MostRecentRuns_DataGridLoaded(object sender, RoutedEventArgs e)
    {
        mostRecentRunsDataGrid = (DataGrid)sender;
        MainWindow.dataLoader.model.RecentRuns = databaseManager.GetRecentRuns();
        mostRecentRunsDataGrid.ItemsSource = MainWindow.dataLoader.model.RecentRuns;
        mostRecentRunsDataGrid.Items.Refresh();
    }

    private void Calendar_DataGridLoaded(object sender, RoutedEventArgs e)
    {
        calendarDataGrid = (DataGrid)sender;
    }

    // Declare flags to track event subscription
    private bool isConfigureButtonClickedSubscribed;
    private bool isDefaultButtonClickedSubscribed;
    private bool isSaveButtonClickedSubscribed;

    // https://stackoverflow.com/questions/36128148/pass-click-event-of-child-control-to-the-parent-control
    private void MainConfigurationActions_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (MainConfigurationActions)sender;

        // Subscribe to events only if not already subscribed
        if (!isSaveButtonClickedSubscribed)
        {
            obj.SaveButtonClicked += SaveButton_Click;
            isSaveButtonClickedSubscribed = true;
        }

        if (!isConfigureButtonClickedSubscribed)
        {
            obj.ConfigureButtonClicked += ConfigureButton_Click;
            isConfigureButtonClickedSubscribed = true;
        }

        if (!isDefaultButtonClickedSubscribed)
        {
            obj.DefaultButtonClicked += DefaultButton_Click;
            isDefaultButtonClickedSubscribed = true;
        }
    }

    // I do not understand the following function, but I've verified it is required for the above function to work correctly.
    private void MainConfigurationActions_Unloaded(object sender, RoutedEventArgs e)
    {
        var obj = (MainConfigurationActions)sender;

        // Unsubscribe from events and update flags
        if (isSaveButtonClickedSubscribed)
        {
            obj.SaveButtonClicked -= SaveButton_Click;
            isSaveButtonClickedSubscribed = false;
        }

        if (isConfigureButtonClickedSubscribed)
        {
            obj.ConfigureButtonClicked -= ConfigureButton_Click;
            isConfigureButtonClickedSubscribed = false;
        }

        if (isDefaultButtonClickedSubscribed)
        {
            obj.DefaultButtonClicked -= DefaultButton_Click;
            isDefaultButtonClickedSubscribed = false;
        }
    }

    private void personalBestChartGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            personalBestChartGrid = obj;
        }
    }

    private void WeaponUsageChartGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            weaponUsageChartGrid = obj;
        }
    }

    private void StatsGraphsComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var comboBox = (ComboBox)sender;
        if (comboBox == null)
        {
            return;
        }

        var selectedItem = comboBox.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }

        // You can now use the selectedItem variable to get the data or value of the selected option
        string? selectedOption = selectedItem.ToString()?.Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim().Replace(" ", "_");
        if (string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        statsGraphsSelectedOption = selectedOption;
    }

    private void StatsTextComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var comboBox = (ComboBox)sender;
        if (comboBox == null)
        {
            return;
        }

        var selectedItem = comboBox.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }

        // You can now use the selectedItem variable to get the data or value of the selected option
        string? selectedOption = selectedItem.ToString()?.Replace("System.Windows.Controls.ComboBoxItem: ", "").Trim().Replace(" ", "_");
        if (string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        statsTextSelectedOption = selectedOption;
    }

    private void GraphsChartGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            statsGraphsGrid = obj;
        }
    }

    private void PersonalBestDescriptionTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (TextBlock)sender;
        if (obj != null)
        {
            personalBestDescriptionTextBlock = obj;
        }
    }

    private void Top20RunsDescriptionTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (TextBlock)sender;
        if (obj != null)
        {
            top20RunsDescriptionTextblock = obj;
        }
    }

    private void PersonalBestMainGridLoaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            personalBestMainGrid = obj;
        }
    }

    private void Top20MainGridLoaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            top20MainGrid = obj;
        }
    }

    private void WeaponStatsMainGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            weaponStatsMainGrid = obj;
        }
    }

    private void StatsGraphsMainGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            statsGraphsMainGrid = obj;
        }
    }

    private void StatsTextMainGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            statsTextMainGrid = obj;
        }
    }

    private async void FumoImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
        snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");
        await achievementManager.RewardAchievement(225, snackbar);
    }

    private void Achievements3DPreviewGrid_Loaded(object sender, RoutedEventArgs e)
    {
        // Create the rotation animation
        Storyboard storyboard = new Storyboard();
        DoubleAnimation animation = new DoubleAnimation
        {
            From = 0,
            To = 360,
            Duration = TimeSpan.FromSeconds(10), // Adjust the duration to control the rotation speed
            RepeatBehavior = RepeatBehavior.Forever
        };
        Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
        storyboard.Begin();
    }

    private void AchievementsListView_Loaded(object sender, RoutedEventArgs e)
    {
        achievementsListView = (ListView)sender;
        MainWindow.dataLoader.model.PlayerAchievements = databaseManager.GetPlayerAchievements();
        achievementsListView.ItemsSource = MainWindow.dataLoader.model.PlayerAchievements;
        achievementsListView.Items.Refresh();
        UpdateAchievementsProgress();
    }

    private void AchievementsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (achievementsSelectedInfoGrid is null)
        {
            return;
        }

        if (AchievementsListView.SelectedItem is Achievement selectedAchievement)
        {
            // Update the other grid's UI elements based on the selected achievement
            // For example:
            AchievementSelectionInfoImage.Source = new BitmapImage(new Uri(selectedAchievement.Image));
            if (selectedAchievement.CompletionDate == DateTime.UnixEpoch)
            {
                AchievementSelectionInfoTitle.Text = selectedAchievement.Title;
                AchievementSelectionInfoImage.Opacity = .25;
            }
            else
            {
                AchievementSelectionInfoImage.Opacity = 1;
                AchievementSelectionInfoTitle.Text = $"{selectedAchievement.Title} | {selectedAchievement.CompletionDate.ToString("yy/MM/dd HH:mm:ss")}"; ;
            }

            var brushConverter = new BrushConverter();

            if (selectedAchievement.IsSecret && selectedAchievement.CompletionDate == DateTime.UnixEpoch)
            {
                AchievementSelectionInfoObjective.Text = string.Empty;
                AchievementSelectionInfoTitle.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Text"]);
            }
            else
            {
                AchievementSelectionInfoObjective.Text = selectedAchievement.Objective;
                AchievementSelectionInfoTitle.Fill = selectedAchievement.GetBrushColorFromRank();
            }

            AchievementSelectionInfoHint.Text = selectedAchievement.Hint;

            AchievementFrontSide.ImageSource = new BitmapImage(new Uri($"{selectedAchievement.Image}"));
            AchievementBackSide.ImageSource = new BitmapImage(new Uri($"{selectedAchievement.GetTrophyImageLinkFromRank()}"));
            // ...
        }
    }

    private void UpdateAchievementsProgress()
    {
        int totalAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.IsSecret == false);
        int obtainedAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch);
        int obtainedSecretAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.IsSecret == true);
        double progressPercentage = (obtainedAchievements * 100.0 / totalAchievements) + obtainedSecretAchievements;
        AchievementsProgressBar.Value = progressPercentage;

        int totalBronzeAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.IsSecret == false && a.Rank == AchievementRank.Bronze);
        int totalSilverAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.IsSecret == false && a.Rank == AchievementRank.Silver);
        int totalGoldAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.IsSecret == false && a.Rank == AchievementRank.Gold);
        int totalPlatinumAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.IsSecret == false && a.Rank == AchievementRank.Platinum);

        int obtainedBronzeAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Bronze);
        int obtainedSilverAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Silver);
        int obtainedGoldAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Gold);
        int obtainedPlatinumAchievements = MainWindow.dataLoader.model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Platinum);

        TrophyBronzeCountTextBlock.Text = $"{obtainedBronzeAchievements}/{totalBronzeAchievements}";
        TrophySilverCountTextBlock.Text = $"{obtainedSilverAchievements}/{totalSilverAchievements}";
        TrophyGoldCountTextBlock.Text = $"{obtainedGoldAchievements}/{totalGoldAchievements}";
        TrophyPlatinumCountTextBlock.Text = $"{obtainedPlatinumAchievements}/{totalPlatinumAchievements}";

        var brushConverter = new BrushConverter();

        if (obtainedAchievements >= 50 && obtainedAchievements < 100) // unlock Bingo + Gacha
        {
            AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Maroon"]);
            AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Maroon"]);
            AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Maroon"]);
            AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Maroon"]);
        }
        else if (obtainedAchievements >= 100 && obtainedAchievements < 150) // unlock zenith gauntlet
        {
            AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Lavender"]);
            AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Lavender"]);
            AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Lavender"]);
            AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Lavender"]);

        }
        else if (obtainedAchievements >= 150 && obtainedAchievements < 200) // unlock solstice gauntlet
        {
            AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Yellow"]);
            AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Yellow"]);
            AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Yellow"]);
            AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Yellow"]);
        }
        else if (obtainedAchievements >= 200 && obtainedAchievements < 300) // unlock musou gauntlet
        {
            AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
        }
        else if (obtainedAchievements == totalAchievements) // all
        {
            AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);

            // Create and apply the DropShadowEffect to the ProgressBar and TextBlock
            var dropShadowEffect = new DropShadowEffect
            {
                Color = (Color)ColorConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]),
                ShadowDepth = 0,
                BlurRadius = 15,
                Opacity = 1
            };

            AchievementsProgressBar.Effect = dropShadowEffect;
            AchievementsProgressTextBlock.Effect = dropShadowEffect;
            AchievementTotalProgressTextBlock.Effect = dropShadowEffect;
            AchievementTotalProgressPercentTextBlock.Effect = dropShadowEffect;
        }
        else
        {
            AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Text"]);
            AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Text"]);
            AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Text"]);
        }

        AchievementsProgressTextBlock.Text = $"{obtainedAchievements}/{totalAchievements}";
        AchievementTotalProgressPercentTextBlock.Text = $"{progressPercentage:F2}%";
    }

    private void AchievementSelectedInfoGrid_Loaded(object sender, RoutedEventArgs e)
    {
        achievementsSelectedInfoGrid = (Grid)sender;
    }

    private void HunterNotesGridMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem && menuItem.Parent is ContextMenu contextMenu && contextMenu.PlacementTarget is FrameworkElement element)
        {
            var snackbar = new Snackbar(ConfigWindowSnackBarPresenter);
            snackbar.Style = (Style)FindResource("CatppuccinMochaSnackBar");

            // Check the Tag property of the ContextMenu to decide which menu items to handle
            if (contextMenu.Name == "HunterNotesContextMenu")
            {
                if (menuItem.Name == "HunterNotesCopyTextMenuItem")
                {
                    FileService.CopyTextToClipboard(element, snackbar);
                }
                else if (menuItem.Name == "HunterNotesSaveTextMenuItem")
                {
                    FileService.SaveTextFile(snackbar, element, element.Name);
                }
                else if (menuItem.Name == "HunterNotesSaveImageMenuItem")
                {
                    FileService.SaveElementAsImageFile(snackbar, element, element.Name);
                }
                else if (menuItem.Name == "HunterNotesCopyImageMenuItem")
                {
                    FileService.CopyUIElementToClipboard(element, snackbar);
                }
                else
                {
                    logger.Error("Invalid Menu Item option: {0}", menuItem);
                    snackbar.Title = Messages.ErrorTitle;
                    snackbar.Content = $"Invalid Menu Item option: {menuItem}";
                    snackbar.Appearance = ControlAppearance.Danger;
                    snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle20);
                    snackbar.Timeout = SnackbarTimeOut;
                    snackbar.Show();
                }
            }
            else if (contextMenu.Name == "HunterNotesContextMenuImageOnly")
            {
                if (menuItem.Name == "HunterNotesSaveImageMenuItem2")
                {
                    FileService.SaveElementAsImageFile(snackbar, element, element.Name);
                }
                else if (menuItem.Name == "HunterNotesCopyImageMenuItem2")
                {
                    FileService.CopyUIElementToClipboard(element, snackbar);
                }
                else
                {
                    logger.Error("Invalid Menu Item option: {0}", menuItem);
                    snackbar.Title = Messages.ErrorTitle;
                    snackbar.Content = $"Invalid Menu Item option: {menuItem}";
                    snackbar.Appearance = ControlAppearance.Danger;
                    snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle20);
                    snackbar.Timeout = SnackbarTimeOut;
                    snackbar.Show();
                }
            }
            else if (contextMenu.Name == "HunterNotesContextMenuImageCSV")
            {
                if (menuItem.Name == "HunterNotesSaveImageMenuItem3")
                {
                    FileService.SaveElementAsImageFile(snackbar, element, element.Name);
                }
                else if (menuItem.Name == "HunterNotesCopyImageMenuItem3")
                {
                    FileService.CopyUIElementToClipboard(element, snackbar);
                }
                else if (menuItem.Name == "HunterNotesSaveCSVMenuItem")
                {
                    if (element.Name == "HuntedLogGrid")
                    {
                        FileService.SaveRecordsAsCSVFile(Monsters, snackbar, "HuntedLog");
                    }
                    else if (element.Name == "AchievementsGrid")
                    {
                        FileService.SaveRecordsAsCSVFile(
                            AchievementService.FilterAchievementsToCompletedOnly(
                                MainWindow.dataLoader.model.PlayerAchievements
                                ).ToArray(), snackbar, "Achievements");
                    }
                    else
                    {
                        logger.Error("Unhandled csv class records: {0}", element.Name);
                        snackbar.Title = Messages.ErrorTitle;
                        snackbar.Content = "Could not save class records as CSV file: unhandled element";
                        snackbar.Appearance = ControlAppearance.Danger;
                        snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle20);
                        snackbar.Timeout = SnackbarTimeOut;
                        snackbar.Show();
                    }
                }
                else
                {
                    logger.Error("Invalid Menu Item option: {0}", menuItem);
                    snackbar.Title = Messages.ErrorTitle;
                    snackbar.Content = $"Invalid Menu Item option: {menuItem}";
                    snackbar.Appearance = ControlAppearance.Danger;
                    snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle20);
                    snackbar.Timeout = SnackbarTimeOut;
                    snackbar.Show();
                }
            }
            else
            {
                logger.Error("Unhandled Context Menu found: {0}", contextMenu.Name);
            }
        }
    }

    private void AchievementsSearchButton_Click(object sender, RoutedEventArgs e)
    {
        // Check if the text in the AutoSuggestBox is empty
        if (string.IsNullOrWhiteSpace(AchievementsSearchComboBox.Text))
        {
            // If the text is empty, show the original list in the ListView
            AchievementsListView.ItemsSource = MainWindow.dataLoader.model.PlayerAchievements;
        }
        else
        {
            // If the text is not empty, set the ItemsSource to null temporarily to clear the ListView
            AchievementsListView.ItemsSource = null;

            // Then, set the ItemsSource back to the filtered achievements list based on the user's input
            string userInput = AchievementsSearchComboBox.Text;
            List<Achievement> filteredAchievements = MainWindow.dataLoader.model.PlayerAchievements
                .Where(achievement => achievement.Title.Contains(userInput, StringComparison.OrdinalIgnoreCase))
                .ToList();
            AchievementsListView.ItemsSource = filteredAchievements;
        }
    }

    private void ChallengesInfoBar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is InfoBar infobar)
        {
            infobar.IsOpen = false;
        }
    }

    private void ChallengesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var listBox = sender as ListBox;
        if (listBox != null)
        {
            listBox.SelectedItem = null;
        }
    }

    private void ChallengesListBox_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        //foreach (var item in ChallengesListBox.Items)
        //{
        //    var listBoxItem = (ListBoxItem)ChallengesListBox.ItemContainerGenerator.ContainerFromItem(item);
        //    var textBlock = FindVisualChild < TextBlock > (listBoxItem);
        //    if (textBlock != null)
        //    {
        //        textBlock.MaxWidth = ChallengesListBox.ActualWidth;
        //    }
        //}
    }

    private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            var child = VisualTreeHelper.GetChild(obj, i);
            if (child is T typedChild)
            {
                return typedChild;
            }

            var childOfChild = FindVisualChild<T>(child);
            if (childOfChild != null)
            {
                return childOfChild;
            }
        }
        return null;
    }
}

/* LoadConfig on startup
 * Load Config on window open to have extra copy
 * On Save -> Window close -> tell program to use new copy instead of current -> Save Config File
 * On Cancel -> Window Close -> Discard copy of config
 * On Config Change Still show changes immediately and show windows which are set to show -> Ignore logic that hides windows during this time and force  them on if they are enabled
 * 
 */
