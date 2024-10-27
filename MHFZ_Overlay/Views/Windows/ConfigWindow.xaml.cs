// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Views.Windows;

using System;
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
using CommunityToolkit.Mvvm.Input;
using EZlion.Mapper;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
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
using Newtonsoft.Json;
using Octokit;

using SkiaSharp;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using ComboBox = System.Windows.Controls.ComboBox;
using DataGrid = Wpf.Ui.Controls.DataGrid;
using ListBox = System.Windows.Controls.ListBox;
using ListView = System.Windows.Controls.ListView;
using MenuItem = Wpf.Ui.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxResult = System.Windows.MessageBoxResult;
using TextBlock = System.Windows.Controls.TextBlock;
using TextBox = System.Windows.Controls.TextBox;

/// <summary>
/// Interaction logic for ConfigWindow.xaml.
/// </summary>
public partial class ConfigWindow : FluentWindow
{
    public static readonly string RickRoll = "https://www.youtube.com/embed/dQw4w9WgXcQ";

    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Gets the main window.
    /// </summary>
    /// <value>
    /// The main window.
    /// </value>
    private MainWindow MainWindow { get; }

    private static readonly string RandomMonsterImage = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png";

    // TODO: change manager to service
    private static readonly DatabaseService DatabaseManager = DatabaseService.GetInstance();
    private static readonly AchievementService AchievementManager = AchievementService.GetInstance();
    private static readonly OverlaySettingsService OverlaySettingsManager = OverlaySettingsService.GetInstance();
    private static readonly AudioService AudioServiceInstance = AudioService.GetInstance();
    private static readonly ChallengeService ChallengeServiceInstance = ChallengeService.GetInstance();


    // TODO put this in a read-only dictionary thing
    private readonly MonsterLog[] monsters = new MonsterLog[]
    {
      new MonsterLog(0, "None", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/none.png", 0),
      new MonsterLog(1, "Rathian", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/rathian.png", 0, true),
      new MonsterLog(2, "Fatalis", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/fatalis.png", 0, true),
      new MonsterLog(3, "Kelbi", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/kelbi.png", 0),
      new MonsterLog(4, "Mosswine", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/mosswine.png", 0),
      new MonsterLog(5, "Bullfango", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/bullfango.png", 0),
      new MonsterLog(6, "Yian Kut-Ku", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/yian_kut-ku.png", 0, true),
      new MonsterLog(7, "Lao-Shan Lung", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/lao-shan_lung.png", 0, true),
      new MonsterLog(8, "Cephadrome", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/cephadrome.png", 0, true),
      new MonsterLog(9, "Felyne", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/felyne.png", 0),
      new MonsterLog(10, "Veggie Elder", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(11, "Rathalos", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/rathalos.png", 0, true),
      new MonsterLog(12, "Aptonoth", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/aptonoth.png", 0),
      new MonsterLog(13, "Genprey", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/genprey.png", 0),
      new MonsterLog(14, "Diablos", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/diablos.png", 0, true),
      new MonsterLog(15, "Khezu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/khezu.png", 0, true),
      new MonsterLog(16, "Velociprey", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/velociprey.png", 0),
      new MonsterLog(17, "Gravios", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gravios.png", 0, true),
      new MonsterLog(18, "Felyne?", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/felyne.png", 0),
      new MonsterLog(19, "Vespoid", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/vespoid.png", 0),
      new MonsterLog(20, "Gypceros", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gypceros.png", 0, true),
      new MonsterLog(21, "Plesioth", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/plesioth.png", 0, true),
      new MonsterLog(22, "Basarios", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/basarios.png", 0, true),
      new MonsterLog(23, "Melynx", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/melynx.png", 0),
      new MonsterLog(24, "Hornetaur", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/hornetaur.png", 0),
      new MonsterLog(25, "Apceros", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/apceros.png", 0),
      new MonsterLog(26, "Monoblos", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/monoblos.png", 0, true),
      new MonsterLog(27, "Velocidrome", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/velocidrome.png", 0, true),
      new MonsterLog(28, "Gendrome", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gendrome.png", 0, true),
      new MonsterLog(29, "Rocks", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(30, "Ioprey", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/ioprey.png", 0),
      new MonsterLog(31, "Iodrome", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/iodrome.png", 0, true),
      new MonsterLog(32, "Poogies", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(33, "Kirin", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/kirin.png", 0, true),
      new MonsterLog(34, "Cephalos", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/cephalos.png", 0),
      new MonsterLog(35, "Giaprey / Giadrome", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/giaprey.png", 0),
      new MonsterLog(36, "Crimson Fatalis", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/crimson_fatalis.png", 0, true),
      new MonsterLog(37, "Pink Rathian", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/pink_rathian.png", 0, true),
      new MonsterLog(38, "Blue Yian Kut-Ku", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/blue_yian_kut-ku.png", 0, true),
      new MonsterLog(39, "Purple Gypceros", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/purple_gypceros.png", 0, true),
      new MonsterLog(40, "Yian Garuga", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/yian_garuga.png", 0, true),
      new MonsterLog(41, "Silver Rathalos", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/silver_rathalos.png", 0, true),
      new MonsterLog(42, "Gold Rathian", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gold_rathian.png", 0, true),
      new MonsterLog(43, "Black Diablos", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/black_diablos.png", 0, true),
      new MonsterLog(44, "White Monoblos", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/white_monoblos.png", 0, true),
      new MonsterLog(45, "Red Khezu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/red_khezu.png", 0, true),
      new MonsterLog(46, "Green Plesioth", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/green_plesioth.png", 0, true),
      new MonsterLog(47, "Black Gravios", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/black_gravios.png", 0, true),
      new MonsterLog(48, "Daimyo Hermitaur", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/daimyo_hermitaur.png", 0, true),
      new MonsterLog(49, "Azure Rathalos", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/azure_rathalos.png", 0, true),
      new MonsterLog(50, "Ashen Lao-Shan Lung", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/ashen_lao-shan_lung.png", 0, true),
      new MonsterLog(51, "Blangonga", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/blangonga.png", 0, true),
      new MonsterLog(52, "Congalala", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/congalala.png", 0, true),
      new MonsterLog(53, "Rajang", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/rajang.png", 0, true),
      new MonsterLog(54, "Kushala Daora", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/kushala_daora.png", 0, true),
      new MonsterLog(55, "Shen Gaoren", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shen_gaoren.png", 0, true),
      new MonsterLog(56, "Great Thunderbug", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/great_thunderbug.png", 0),
      new MonsterLog(57, "Shakalaka", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shakalaka.png", 0),
      new MonsterLog(58, "Yama Tsukami", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/yama_tsukami.png", 0, true),
      new MonsterLog(59, "Chameleos", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/chameleos.png", 0, true),
      new MonsterLog(60, "Rusted Kushala Daora", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/rusted_kushala_daora.png", 0, true),
      new MonsterLog(61, "Blango", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/blango.png", 0),
      new MonsterLog(62, "Conga", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/conga.png", 0),
      new MonsterLog(63, "Remobra", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/remobra.png", 0),
      new MonsterLog(64, "Lunastra", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/lunastra.png", 0, true),
      new MonsterLog(65, "Teostra", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/teostra.png", 0, true),
      new MonsterLog(66, "Hermitaur", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/hermitaur.png", 0),
      new MonsterLog(67, "Shogun Ceanataur", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shogun_ceanataur.png", 0, true),
      new MonsterLog(68, "Bulldrome", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/bulldrome.png", 0, true),
      new MonsterLog(69, "Anteka", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/anteka.png", 0),
      new MonsterLog(70, "Popo", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/popo.png", 0),
      new MonsterLog(71, "White Fatalis", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/white_fatalis.png", 0, true),
      new MonsterLog(72, "Yama Tsukami", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/yama_tsukami.png", 0, true),
      new MonsterLog(73, "Ceanataur", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/ceanataur.png", 0),
      new MonsterLog(74, "Hypnocatrice", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/hypnoc.png", 0, true),
      new MonsterLog(75, "Lavasioth", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/lavasioth.png", 0, true),
      new MonsterLog(76, "Tigrex", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/tigrex.png", 0, true),
      new MonsterLog(77, "Akantor", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/akantor.png", 0, true),
      new MonsterLog(78, "Bright Hypnoc", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/bright_hypnoc.png", 0, true),
      new MonsterLog(79, "Red Lavasioth", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/red_lavasioth.png", 0, true),
      new MonsterLog(80, "Espinas", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/espinas.png", 0, true),
      new MonsterLog(81, "Orange Espinas", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/orange_espinas.png", 0, true),
      new MonsterLog(82, "Silver Hypnoc", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/silver_hypnoc.png", 0, true),
      new MonsterLog(83, "Akura Vashimu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/akura_vashimu.png", 0, true),
      new MonsterLog(84, "Akura Jebia", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/akura_jebia.png", 0, true),
      new MonsterLog(85, "Berukyurosu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/berukyurosu.png", 0, true),
      new MonsterLog(86, "Cactus", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/cactus.png", 0),
      new MonsterLog(87, "Gorge Objects", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(88, "Gorge Rocks", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(89, "Pariapuria", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/pariapuria.png", 0, true),
      new MonsterLog(90, "White Espinas", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/white_espinas.png", 0, true),
      new MonsterLog(91, "Kamu Orugaron", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/kamu_orugaron.png", 0, true),
      new MonsterLog(92, "Nono Orugaron", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/nono_orugaron.png", 0, true),
      new MonsterLog(93, "Raviente", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/raviente.png", 0, true),
      new MonsterLog(94, "Dyuragaua", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/dyuragaua.png", 0, true),
      new MonsterLog(95, "Doragyurosu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/doragyurosu.png", 0, true),
      new MonsterLog(96, "Gurenzeburu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gurenzeburu.png", 0, true),
      new MonsterLog(97, "Burukku", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/burukku.png", 0),
      new MonsterLog(98, "Erupe", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/erupe.png", 0),
      new MonsterLog(99, "Rukodiora", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/rukodiora.png", 0, true),
      new MonsterLog(100, "UNKNOWN", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/unknown.png", 0, true),
      new MonsterLog(101, "Gogomoa", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gogomoa.png", 0, true),
      new MonsterLog(102, "Kokomoa", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gogomoa.png", 0),
      new MonsterLog(103, "Taikun Zamuza", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/taikun_zamuza.png", 0, true),
      new MonsterLog(104, "Abiorugu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/abiorugu.png", 0, true),
      new MonsterLog(105, "Kuarusepusu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/kuarusepusu.png", 0, true),
      new MonsterLog(106, "Odibatorasu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/odibatorasu.png", 0, true),
      new MonsterLog(107, "Disufiroa", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/disufiroa.png", 0, true),
      new MonsterLog(108, "Rebidiora", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/rebidiora.png", 0, true),
      new MonsterLog(109, "Anorupatisu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/anorupatisu.png", 0, true),
      new MonsterLog(110, "Hyujikiki", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/hyujikiki.png", 0, true),
      new MonsterLog(111, "Midogaron", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/midogaron.png", 0, true),
      new MonsterLog(112, "Giaorugu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/giaorugu.png", 0, true),
      new MonsterLog(113, "Mi Ru", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/mi_ru.png", 0, true),
      new MonsterLog(114, "Farunokku", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/farunokku.png", 0, true),
      new MonsterLog(115, "Pokaradon", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/pokaradon.png", 0, true),
      new MonsterLog(116, "Shantien", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shantien.png", 0, true),
      new MonsterLog(117, "Pokara", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/pokara.png", 0),
      new MonsterLog(118, "Dummy", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(119, "Goruganosu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/goruganosu.png", 0, true),
      new MonsterLog(120, "Aruganosu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/aruganosu.png", 0, true),
      new MonsterLog(121, "Baruragaru", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/baruragaru.png", 0, true),
      new MonsterLog(122, "Zerureusu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zerureusu.png", 0, true),
      new MonsterLog(123, "Gougarf", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gougarf.png", 0, true),
      new MonsterLog(124, "Uruki", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/uruki.png", 0),
      new MonsterLog(125, "Forokururu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/forokururu.png", 0, true),
      new MonsterLog(126, "Meraginasu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/meraginasu.png", 0, true),
      new MonsterLog(127, "Diorex", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/diorex.png", 0, true),
      new MonsterLog(128, "Garuba Daora", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/garuba_daora.png", 0, true),
      new MonsterLog(129, "Inagami", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/inagami.png", 0, true),
      new MonsterLog(130, "Varusaburosu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/varusaburosu.png", 0, true),
      new MonsterLog(131, "Poborubarumu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/poborubarumu.png", 0, true),
      new MonsterLog(132, "1st District Duremudira", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/duremudira.png", 0, true),
      new MonsterLog(133, "UNK", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(134, "Felyne", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/felyne.png", 0),
      new MonsterLog(135, "Blue NPC", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(136, "UNK", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(137, "Cactus", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/cactus.png", 0),
      new MonsterLog(138, "Veggie Elders", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(139, "Gureadomosu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gureadomosu.png", 0, true),
      new MonsterLog(140, "Harudomerugu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/harudomerugu.png", 0, true),
      new MonsterLog(141, "Toridcless", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/toridcless.png", 0, true),
      new MonsterLog(142, "Gasurabazura", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gasurabazura.png", 0, true),
      new MonsterLog(143, "Kusubami", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/kusubami.png", 0),
      new MonsterLog(144, "Yama Kurai", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/yama_kurai.png", 0, true),
      new MonsterLog(145, "2nd District Duremudira", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/duremudira.png", 0, true),
      new MonsterLog(146, "Zinogre", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zinogre.png", 0, true),
      new MonsterLog(147, "Deviljho", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/deviljho.png", 0, true),
      new MonsterLog(148, "Brachydios", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/brachydios.png", 0, true),
      new MonsterLog(149, "Berserk Raviente", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/berserk_raviente.png", 0, true),
      new MonsterLog(150, "Toa Tesukatora", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/toa_tesukatora.png", 0, true),
      new MonsterLog(151, "Barioth", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/barioth.png", 0, true),
      new MonsterLog(152, "Uragaan", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/uragaan.png", 0, true),
      new MonsterLog(153, "Stygian Zinogre", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/stygian_zinogre.png", 0, true),
      new MonsterLog(154, "Guanzorumu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/guanzorumu.png", 0, true),
      new MonsterLog(155, "Starving Deviljho", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/starving_deviljho.png", 0, true),
      new MonsterLog(156, "UNK", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(157, "Egyurasu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(158, "Voljang", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/voljang.png", 0, true),
      new MonsterLog(159, "Nargacuga", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/nargacuga.png", 0, true),
      new MonsterLog(160, "Keoaruboru", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/keoaruboru.png", 0, true),
      new MonsterLog(161, "Zenaserisu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenaserisu.png", 0, true),
      new MonsterLog(162, "Gore Magala", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/gore_magala.png", 0, true),
      new MonsterLog(163, "Blinking Nargacuga", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/blinking_nargacuga.png", 0, true),
      new MonsterLog(164, "Shagaru Magala", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/shagaru_magala.png", 0, true),
      new MonsterLog(165, "Amatsu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/amatsu.png", 0, true),
      new MonsterLog(166, "Elzelion", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/elzelion.png", 0, true),
      new MonsterLog(167, "Arrogant Duremudira", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/arrogant_duremudira.png", 0, true),
      new MonsterLog(168, "Rocks", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(169, "Seregios", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/seregios.png", 0, true),
      new MonsterLog(170, "Bogabadorumu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/zenith_bogabadorumu.gif", 0, true),
      new MonsterLog(171, "Unknown Blue Barrel", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png", 0),
      new MonsterLog(172, "Bombardier Bogabadorumu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/bombardier_bogabadorumu.png", 0, true),
      new MonsterLog(173, "Costumed Uruki", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/uruki.png", 0),
      new MonsterLog(174, "Sparkling Zerureusu", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/sparkling_zerureusu.png", 0, true),
      new MonsterLog(175, "PSO2 Rappy", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/pso2_rappy.png", 0),
      new MonsterLog(176, "King Shakalaka", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/king_shakalaka.png", 0, true),
    };

    private readonly IReadOnlyList<MonsterInfo> monsterInfos = MonsterInfos.MonsterInfoIDs;

    private readonly GitHubClient client = new(new ProductHeaderValue("MHFZ_Overlay"));

    private List<WeaponUsage> weaponUsageData = new();

    public static Uri MonsterInfoLink => new(RandomMonsterImage, UriKind.RelativeOrAbsolute);

    public static Uri MonsterImage => new(RandomMonsterImage, UriKind.RelativeOrAbsolute);

    public static string GetFeriasLink()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.FeriasVersionLink;
    }

    public static string ReplaceMonsterInfoFeriasVersion(string link)
    {
        var replaceSettingsLink = GetFeriasLink();

        // Check if no need to replace because its the same version already
        if (link.Contains(replaceSettingsLink))
        {
            return link;
        }

        var separator = "mons/";
        var info = link.Split(separator)[1];

        return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", replaceSettingsLink, separator, info);
    }

    public int GetHuntedCount(int id)
    {
        var dl = this.MainWindow.DataLoader;

        return id switch
        {
            0 => 0,
            1 => dl.Model.RathianHunted(),
            2 => dl.Model.FatalisHunted(),
            3 => dl.Model.KelbiHunted(),
            4 => dl.Model.MosswineHunted(),
            5 => dl.Model.BullfangoHunted(),
            6 => dl.Model.YianKutKuHunted(),
            7 => dl.Model.LaoShanLungHunted(),
            8 => dl.Model.CephadromeHunted(),
            9 => dl.Model.FelyneHunted(),
            10 => 0,
            11 => dl.Model.RathalosHunted(),
            12 => dl.Model.AptonothHunted(),
            13 => dl.Model.GenpreyHunted(),
            14 => dl.Model.DiablosHunted(),
            15 => dl.Model.KhezuHunted(),
            16 => dl.Model.VelocipreyHunted(),
            17 => dl.Model.GraviosHunted(),
            18 => 0,
            19 => dl.Model.VespoidHunted(),
            20 => dl.Model.GypcerosHunted(),
            21 => dl.Model.PlesiothHunted(),
            22 => dl.Model.BasariosHunted(),
            23 => dl.Model.MelynxHunted(),
            24 => dl.Model.HornetaurHunted(),
            25 => dl.Model.ApcerosHunted(),
            26 => dl.Model.MonoblosHunted(),
            27 => dl.Model.VelocidromeHunted(),
            28 => dl.Model.GendromeHunted(),
            29 => dl.Model.RocksHunted(),
            30 => dl.Model.IopreyHunted(),
            31 => dl.Model.IodromeHunted(),
            32 => 0,
            33 => dl.Model.KirinHunted(),
            34 => dl.Model.CephalosHunted(),
            35 => dl.Model.GiapreyHunted(),
            36 => dl.Model.CrimsonFatalisHunted(),
            37 => dl.Model.PinkRathianHunted(),
            38 => dl.Model.BlueYianKutKuHunted(),
            39 => dl.Model.PurpleGypcerosHunted(),
            40 => dl.Model.YianGarugaHunted(),
            41 => dl.Model.SilverRathalosHunted(),
            42 => dl.Model.GoldRathianHunted(),
            43 => dl.Model.BlackDiablosHunted(),
            44 => dl.Model.WhiteMonoblosHunted(),
            45 => dl.Model.RedKhezuHunted(),
            46 => dl.Model.GreenPlesiothHunted(),
            47 => dl.Model.BlackGraviosHunted(),
            48 => dl.Model.DaimyoHermitaurHunted(),
            49 => dl.Model.AzureRathalosHunted(),
            50 => dl.Model.AshenLaoShanLungHunted(),
            51 => dl.Model.BlangongaHunted(),
            52 => dl.Model.CongalalaHunted(),
            53 => dl.Model.RajangHunted(),
            54 => dl.Model.KushalaDaoraHunted(),
            55 => dl.Model.ShenGaorenHunted(),
            56 => dl.Model.GreatThunderbugHunted(),
            57 => dl.Model.ShakalakaHunted(),
            58 => dl.Model.YamaTsukamiHunted(),
            59 => dl.Model.ChameleosHunted(),
            60 => dl.Model.RustedKushalaDaoraHunted(),
            61 => dl.Model.BlangoHunted(),
            62 => dl.Model.CongaHunted(),
            63 => dl.Model.RemobraHunted(),
            64 => dl.Model.LunastraHunted(),
            65 => dl.Model.TeostraHunted(),
            66 => dl.Model.HermitaurHunted(),
            67 => dl.Model.ShogunCeanataurHunted(),
            68 => dl.Model.BulldromeHunted(),
            69 => dl.Model.AntekaHunted(),
            70 => dl.Model.PopoHunted(),
            71 => dl.Model.WhiteFatalisHunted(),
            72 => dl.Model.YamaTsukamiHunted(),
            73 => dl.Model.CeanataurHunted(),
            74 => dl.Model.HypnocHunted(),
            75 => dl.Model.VolganosHunted(),
            76 => dl.Model.TigrexHunted(),
            77 => dl.Model.AkantorHunted(),
            78 => dl.Model.BrightHypnocHunted(),
            79 => dl.Model.RedVolganosHunted(),
            80 => dl.Model.EspinasHunted(),
            81 => dl.Model.OrangeEspinasHunted(),
            82 => dl.Model.SilverHypnocHunted(),
            83 => dl.Model.AkuraVashimuHunted(),
            84 => dl.Model.AkuraJebiaHunted(),
            85 => dl.Model.BerukyurosuHunted(),
            86 => dl.Model.CactusHunted(),
            87 => dl.Model.GorgeObjectsHunted(),
            88 => 0,
            89 => dl.Model.PariapuriaHunted(),
            90 => dl.Model.WhiteEspinasHunted(),
            91 => dl.Model.KamuOrugaronHunted(),
            92 => dl.Model.NonoOrugaronHunted(),
            93 => 0,
            94 => dl.Model.DyuragauaHunted(),
            95 => dl.Model.DoragyurosuHunted(),
            96 => dl.Model.GurenzeburuHunted(),
            97 => dl.Model.BurukkuHunted(),
            98 => dl.Model.ErupeHunted(),
            99 => dl.Model.RukodioraHunted(),
            100 => dl.Model.UnknownHunted(),
            101 => dl.Model.GogomoaHunted(),
            102 => 0,
            103 => dl.Model.TaikunZamuzaHunted(),
            104 => dl.Model.AbioruguHunted(),
            105 => dl.Model.KuarusepusuHunted(),
            106 => dl.Model.OdibatorasuHunted(),
            107 => dl.Model.DisufiroaHunted(),
            108 => dl.Model.RebidioraHunted(),
            109 => dl.Model.AnorupatisuHunted(),
            110 => dl.Model.HyujikikiHunted(),
            111 => dl.Model.MidogaronHunted(),
            112 => dl.Model.GiaoruguHunted(),
            113 => dl.Model.MiRuHunted(),
            114 => dl.Model.FarunokkuHunted(),
            115 => dl.Model.PokaradonHunted(),
            116 => dl.Model.ShantienHunted(),
            117 => dl.Model.PokaraHunted(),
            118 => 0,
            119 => dl.Model.GoruganosuHunted(),
            120 => dl.Model.AruganosuHunted(),
            121 => dl.Model.BaruragaruHunted(),
            122 => dl.Model.ZerureusuHunted(),
            123 => dl.Model.GougarfHunted(),
            124 => dl.Model.UrukiHunted(),
            125 => dl.Model.ForokururuHunted(),
            126 => dl.Model.MeraginasuHunted(),
            127 => dl.Model.DiorexHunted(),
            128 => dl.Model.GarubaDaoraHunted(),
            129 => dl.Model.InagamiHunted(),
            130 => dl.Model.VarusaburosuHunted(),
            131 => dl.Model.PoborubarumuHunted(),
            132 => dl.Model.FirstDistrictDuremudiraSlays(),
            133 => 0,
            134 => 0,
            135 => 0,
            136 => 0,
            137 => dl.Model.CactusHunted(),
            138 => 0,
            139 => dl.Model.GureadomosuHunted(),
            140 => dl.Model.HarudomeruguHunted(),
            141 => dl.Model.ToridclessHunted(),
            142 => dl.Model.GasurabazuraHunted(),
            143 => dl.Model.KusubamiHunted(),
            144 => dl.Model.YamaKuraiHunted(),
            145 => dl.Model.SecondDistrictDuremudiraSlays(),
            146 => dl.Model.ZinogreHunted(),
            147 => dl.Model.DeviljhoHunted(),
            148 => dl.Model.BrachydiosHunted(),
            149 => 0,
            150 => dl.Model.ToaTesukatoraHunted(),
            151 => dl.Model.BariothHunted(),
            152 => dl.Model.UragaanHunted(),
            153 => dl.Model.StygianZinogreHunted(),
            154 => dl.Model.GuanzorumuHunted(),
            155 => dl.Model.StarvingDeviljhoHunted(),
            156 => 0,
            157 => 0,
            158 => dl.Model.VoljangHunted(),
            159 => dl.Model.NargacugaHunted(),
            160 => dl.Model.KeoaruboruHunted(),
            161 => dl.Model.ZenaserisuHunted(),
            162 => dl.Model.GoreMagalaHunted(),
            163 => dl.Model.BlinkingNargacugaHunted(),
            164 => dl.Model.ShagaruMagalaHunted(),
            165 => dl.Model.AmatsuHunted(),
            166 => dl.Model.ElzelionHunted(),
            167 => dl.Model.ArrogantDuremudiraHunted(),
            168 => 0,
            169 => dl.Model.SeregiosHunted(),
            170 => dl.Model.BogabadorumuHunted(),
            171 => 0,
            172 => dl.Model.BombardierBogabadorumuHunted(),
            173 => 0,
            174 => dl.Model.SparklingZerureusuHunted(),
            175 => dl.Model.PSO2RappyHunted(),
            176 => dl.Model.KingShakalakaHunted(),
            _ => 0,
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigWindow"/> class.
    /// </summary>
    /// <param name="mainWindow">The main window.</param>
    public ConfigWindow(MainWindow mainWindow)
    {
        // Create a Stopwatch instance
        var stopwatch = new Stopwatch();

        // Start the stopwatch
        stopwatch.Start();

        this.InitializeComponent();
        Logger.Info(CultureInfo.InvariantCulture, $"ConfigWindow initialized");

        this.Topmost = true;
        this.MainWindow = mainWindow;

        var background1 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/1.png";
        var background2 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/2.png";
        var background3 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/3.png";
        var background4 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/4.png";
        var background5 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/5.png";
        var background6 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/6.png";
        var background7 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/7.png";
        var background8 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/8.png";
        var background9 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/9.png";
        var background10 = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Background/10.png";

        // https://stackoverflow.com/questions/30839173/change-background-image-in-wpf-using-c-sharp
        this.GeneralContent.Background = new ImageBrush(new BitmapImage(new Uri(background1)));
        this.HunterNotesContent.Background = new ImageBrush(new BitmapImage(new Uri(background2)));
        this.MonsterHPContent.Background = new ImageBrush(new BitmapImage(new Uri(background3)));
        this.MonsterStatusContent.Background = new ImageBrush(new BitmapImage(new Uri(background4)));
        this.DiscordRPCContent.Background = new ImageBrush(new BitmapImage(new Uri(background5)));
        this.CreditsContent.Background = new ImageBrush(new BitmapImage(new Uri(background6)));
        this.MonsterInfoContent.Background = new ImageBrush(new BitmapImage(new Uri(background7)));
        this.QuestLogContent.Background = new ImageBrush(new BitmapImage(new Uri(background8)));
        this.PlayerContent.Background = new ImageBrush(new BitmapImage(new Uri(background9)));
        this.AudioContent.Background = new ImageBrush(new BitmapImage(new Uri(background10)));

        // TODO: test this
        this.DataContext = this.MainWindow.DataLoader.Model;

        for (var i = 0; i < this.monsters.Length; i++)
        {
            this.monsters[i].Hunted = this.GetHuntedCount(this.monsters[i].ID);
        }

        this.HuntLogDataGrid.ItemsSource = this.monsters;
        this.HuntLogDataGrid.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        this.FilterBox.ItemsSource = new string[] { "All", "Large Monster", "Small Monster" };
        this.HuntLogDataGrid.Items.Filter = MonsterFilterAll;

        // See: https://stackoverflow.com/questions/22285866/why-relaycommand
        // Or use MVVM Light to obtain RelayCommand.
        var monsterNameList = new List<string>();

        for (var i = 0; i < this.monsterInfos.Count; i++)
        {
            monsterNameList.Add(this.monsterInfos[i].Name);
        }

        this.MonsterNameComboBox.ItemsSource = monsterNameList;

        _ = this.GetRepoStats();

        this.ReplaceAllMonsterInfoFeriasLinks();

        this.weaponUsageData = DatabaseManager.CalculateTotalWeaponUsage(this, this.MainWindow.DataLoader);

        // In your initialization or setup code
        ISnackbarService snackbarService = new SnackbarService();

        // Replace 'snackbarControl' with your actual snackbar control instance
        snackbarService.SetSnackbarPresenter(this.ConfigWindowSnackBarPresenter);

        // Stop the stopwatch
        stopwatch.Stop();

        // Get the elapsed time in milliseconds
        var elapsedTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        StartChallengeCommand = new RelayCommand<Challenge?>(StartChallenge);

        // Print the elapsed time
        Logger.Debug($"ConfigWindow ctor Elapsed Time: {elapsedTimeMs} ms");
    }

    public TimeSpan SnackbarTimeOut { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Shows the text format mode.
    /// </summary>
    /// <returns></returns>
    public static string GetTextFormatMode()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.TextFormatExport ?? "None";
    }

    public Predicate<object> GetFilter() => (this.FilterBox.SelectedItem as string) switch
    {
        "Large Monster" => MonsterFilterLarge,
        "Small Monster" => MonsterFilterSmall,
        _ => MonsterFilterAll,
    };

    /// <summary>
    /// Saves the key press.
    /// </summary>
    public void SaveKeyPress()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        s.Save();
        this.DisposeAllWebViews();
        this.Close();
    }

    /// <summary>
    /// Cancels the key press.
    /// </summary>
    public void CancelKeyPress()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        s.Reload();
        this.DisposeAllWebViews();
        this.Close();
    }

    /// <summary>
    /// Set default settings.
    /// </summary>
    public void DefaultKeyPress()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        this.DisposeAllWebViews();
        s.Reset();
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Window.Closing" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        var s = (Settings)Application.Current.TryFindResource("Settings");
        s.Reload();
        this.MainWindow.DataLoader.Model.Configuring = false;
    }

    private void SetWeaponUsageChart(CartesianChart weaponUsageChart)
    {
        this.MainWindow.DataLoader.Model.WeaponUsageSeries.Clear();
        var s = (Settings)Application.Current.TryFindResource("Settings");

        var weaponStyles = new[] { "Earth Style", "Heaven Style", "Storm Style", "Extreme Style" };
        var weaponTypes = new[]
        {
            "Sword and Shield", "Dual Swords", "Great Sword", "Long Sword",
            "Hammer", "Hunting Horn", "Lance", "Gunlance", "Tonfa",
            "Switch Axe F", "Magnet Spike", "Light Bowgun", "Heavy Bowgun",
            "Bow",
        };

        foreach (var weaponType in weaponTypes)
        {
            foreach (var weaponStyle in weaponStyles)
            {
                switch (weaponStyle)
                {
                    case "Earth Style":
                        var weaponUsageCount = this.weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        this.MainWindow.DataLoader.Model.WeaponUsageEarthStyle.Add(weaponUsageCount);
                        break;
                    case "Heaven Style":
                        weaponUsageCount = this.weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        this.MainWindow.DataLoader.Model.WeaponUsageHeavenStyle.Add(weaponUsageCount);
                        break;
                    case "Storm Style":
                        weaponUsageCount = this.weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        this.MainWindow.DataLoader.Model.WeaponUsageStormStyle.Add(weaponUsageCount);
                        break;
                    case "Extreme Style":
                        weaponUsageCount = this.weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        this.MainWindow.DataLoader.Model.WeaponUsageExtremeStyle.Add(weaponUsageCount);
                        break;
                    default:
                        break;
                }
            }
        }

        this.MainWindow.DataLoader.Model.WeaponUsageSeries.Add(new StackedColumnSeries<long>
        {
            Name = "Earth Style",
            Values = this.MainWindow.DataLoader.Model.WeaponUsageEarthStyle,
            DataLabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
            DataLabelsSize = 14,
            DataLabelsPosition = DataLabelsPosition.Middle,
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
        });

        this.MainWindow.DataLoader.Model.WeaponUsageSeries.Add(new StackedColumnSeries<long>
        {
            Name = "Heaven Style",
            Values = this.MainWindow.DataLoader.Model.WeaponUsageHeavenStyle,
            DataLabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
            DataLabelsSize = 14,
            DataLabelsPosition = DataLabelsPosition.Middle,
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
        });

        this.MainWindow.DataLoader.Model.WeaponUsageSeries.Add(new StackedColumnSeries<long>
        {
            Name = "Storm Style",
            Values = this.MainWindow.DataLoader.Model.WeaponUsageStormStyle,
            DataLabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
            DataLabelsSize = 14,
            DataLabelsPosition = DataLabelsPosition.Middle,
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
        });

        this.MainWindow.DataLoader.Model.WeaponUsageSeries.Add(new StackedColumnSeries<long>
        {
            Name = "Extreme Style",
            Values = this.MainWindow.DataLoader.Model.WeaponUsageExtremeStyle,
            DataLabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
            DataLabelsSize = 14,
            DataLabelsPosition = DataLabelsPosition.Middle,
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor, "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(s.PlayerHitsPerSecondGraphColor))) { StrokeThickness = 2 },
        });

        weaponUsageChart.Series = this.MainWindow.DataLoader.Model.WeaponUsageSeries;
        weaponUsageChart.XAxes = new List<Axis>
        {
                new Axis
                {
                    MinStep = 1,
                    Padding = new LiveChartsCore.Drawing.Padding(0, 0, 0, 0),
                    ShowSeparatorLines = true,
                    IsVisible = false,
                    LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
                },
        };
        weaponUsageChart.YAxes = new List<Axis>
        {
                new Axis
                {
                    MinStep = 1,
                    LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal(CatppuccinMochaColors.NameHex["Text"]))),
                    ShowSeparatorLines = true,
                    TextSize = 12,
                },
        };
    }

    private void ReplaceAllMonsterInfoFeriasLinks()
    {
        for (var i = 0; i < this.monsterInfos.Count; i++)
        {
            this.monsterInfos[i].FeriasLink = ReplaceMonsterInfoFeriasVersion(this.monsterInfos[i].FeriasLink);
        }
    }

    private static bool MonsterFilterAll(object obj)
    {
        if (obj is not MonsterLog filterObj)
        {
            return false;
        }

        return filterObj.IsLarge || !filterObj.IsLarge;
    }

    private static bool MonsterFilterLarge(object obj)
    {
        if (obj is not MonsterLog filterObj)
        {
            return false;
        }

        return filterObj.IsLarge;
    }

    private static bool MonsterFilterSmall(object obj) => obj is MonsterLog filterObj && !filterObj.IsLarge;

    /// <summary>
    /// Handles the PreviewTextInput event of the RoadOverrideTextBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
    private void RoadOverrideTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (e.Text is not "0" and not "1" and not "2")
        {
            e.Handled = true;
        }
    }

    // TODO does this cover everything?
    private void DisposeAllWebViews()
    {
        this.webViewFerias.Dispose();
        this.webViewMonsterInfo.Dispose();
        this.webViewWycademy.Dispose();
    }

    /// <summary>
    /// Handles the Click event of the SaveButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        s.Save();
        this.DisposeAllWebViews();
        this.Close();
    }

    /// <summary>
    /// Handles the Click event of the CancelButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        this.DisposeAllWebViews();
        s.Reload();
        this.Close();
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
            var s = (Settings)Application.Current.TryFindResource("Settings");
            this.DisposeAllWebViews();
            s.Reset();
        }
    }

    /// <summary>
    /// Handles the Click event of the ConfigureButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void ConfigureButton_Click(object sender, RoutedEventArgs e) => this.MainWindow.EnableDragAndDrop();

    /// <summary>
    /// Validates the number.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
    private void ValidateNumber(object sender, TextCompositionEventArgs e)
    {
        foreach (var ch in e.Text)
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
        foreach (var ch in e.Text)
        {
            if (!char.IsDigit(ch) && ch != '.')
            {
                e.Handled = true;
                return;
            }
        }

        var textBox = (TextBox)sender;
        var newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
        var isValidDecimal = decimal.TryParse(newText, NumberStyles.Any, CultureInfo.InvariantCulture, out _);

        if (!isValidDecimal)
        {
            e.Handled = true;
        }
        else if (e.Text == "." && textBox.Text.Contains('.'))
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
        if (!e.Text.All(char.IsLetterOrDigit))
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
    private void LnkImg_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        e.Handled = true;
    }

    /// <summary>
    /// Handles the Click event of the btnSaveFile control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
    {
        var textToSave = this.GearStats.Text;

        if (GetTextFormatMode() == "Code Block")
        {
            textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        }
        else if (GetTextFormatMode() == "Markdown")
        {
            textToSave = this.MainWindow.DataLoader.Model.MarkdownSavedGearStats;
        }

        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter);
        FileService.SaveTextFile(snackbar, textToSave, "GearStats");
    }

    /// <summary>
    /// Copy to clipboard. TODO: change function name, its too generic.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnCopyFile_Click(object sender, RoutedEventArgs e)
    {
        var textToSave = this.GearStats.Text;

        if (GetTextFormatMode() == "Code Block")
        {
            textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        }
        else if (GetTextFormatMode() == "Markdown")
        {
            textToSave = this.MainWindow.DataLoader.Model.MarkdownSavedGearStats;
        }
        else if (GetTextFormatMode() == "Image")
        {
            var previousBackground = this.GearTextGrid.Background;
            this.GearTextGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
            var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
            {
                Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
            };
            FileService.CopyUIElementToClipboard(this.GearTextGrid, snackbar);
            this.GearTextGrid.Background = previousBackground;
            return;
        }

        // https://stackoverflow.com/questions/3546016/how-to-copy-data-to-clipboard-in-c-sharp
        Clipboard.SetText(textToSave);
        var snackbarSuccess = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
            Title = Messages.InfoTitle,
            Content = "Copied text to clipboard",
            Appearance = ControlAppearance.Success,
            Icon = new SymbolIcon(SymbolRegular.Clipboard32),
            Timeout = this.SnackbarTimeOut,
        };
        snackbarSuccess.Show();
    }

    private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.HuntLogDataGrid.Items.Filter = this.GetFilter();

    private void Config_Closed(object sender, EventArgs e)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        this.DisposeAllWebViews();
        s.Reload();
        this.Close();
    }

    private void ChangeMonsterInfo()
    {
        if (this.webViewMonsterInfo == null)
        {
            return;
        }

        Dictionary<string, string> monsterFeriasOptionDictionary = new();
        Dictionary<string, string> monsterWikiOptionDictionary = new();
        Dictionary<string, string> monsterVideoLinkOptionDictionary = new();
        Dictionary<string, string> monsterWycademyOptionDictionary = new();

        for (var i = 0; i < this.monsterInfos.Count; i++)
        {
            foreach (var videolinkPerRankBand in this.monsterInfos[i].WeaponMatchups)
            {
                monsterVideoLinkOptionDictionary.Add(videolinkPerRankBand.Key + " " + this.monsterInfos[i].Name, videolinkPerRankBand.Value);
            }

            monsterWikiOptionDictionary.Add(this.monsterInfos[i].Name, this.monsterInfos[i].WikiLink);
            monsterWycademyOptionDictionary.Add(this.monsterInfos[i].Name, this.monsterInfos[i].WycademyLink);
            monsterFeriasOptionDictionary.Add(this.monsterInfos[i].Name, this.monsterInfos[i].FeriasLink);
        }

        // see this
        // string selectedOverlayMode = ((ComboBoxItem)configWindow.OverlayModeComboBox.SelectedItem).Content.ToString();
        string? selectedName = this.MonsterNameComboBox.SelectedItem.ToString();
        if (string.IsNullOrEmpty(selectedName))
        {
            selectedName = string.Empty;
        }

        var selectedMatchup = $"{((ComboBoxItem)this.WeaponMatchupComboBox.SelectedItem).Content} {selectedName}";

        if (!monsterFeriasOptionDictionary.TryGetValue(selectedName, out var val1) || !monsterWikiOptionDictionary.TryGetValue(selectedName, out var val2))
        {
            return;
        }

        if (this.webViewMonsterInfo.CoreWebView2 == null)
        {
            return;
        }

        switch (this.MonsterInfoViewOptionComboBox.SelectedIndex)
        {
            default:
                this.DockPanelMonsterInfo.Width = double.NaN; // Auto
                this.DockPanelMonsterInfo.Height = double.NaN; // Auto
                this.webViewMonsterInfo.CoreWebView2.Navigate(monsterWycademyOptionDictionary[this.MonsterNameComboBox.SelectedItem.ToString() + string.Empty]);
                return;
            case 1: // ferias
                // https://stackoverflow.com/questions/1265812/howto-define-the-auto-width-of-the-wpf-gridview-column-in-code
                this.DockPanelMonsterInfo.Width = double.NaN; // Auto
                this.DockPanelMonsterInfo.Height = double.NaN; // Auto
                this.webViewMonsterInfo.CoreWebView2.Navigate(monsterFeriasOptionDictionary[this.MonsterNameComboBox.SelectedItem.ToString() + string.Empty]);
                return;
            case 2: // wiki
                this.DockPanelMonsterInfo.Width = double.NaN; // Auto
                this.DockPanelMonsterInfo.Height = double.NaN; // Auto
                this.webViewMonsterInfo.CoreWebView2.Navigate(monsterWikiOptionDictionary[this.MonsterNameComboBox.SelectedItem.ToString() + string.Empty]);
                return;
            case 3: // youtube
                if (monsterVideoLinkOptionDictionary.TryGetValue(selectedMatchup, out var videoval) && monsterVideoLinkOptionDictionary[selectedMatchup] != string.Empty)
                {
                    this.DockPanelMonsterInfo.Width = 854;
                    this.DockPanelMonsterInfo.Height = 480;
                    this.webViewMonsterInfo.CoreWebView2.Navigate(monsterVideoLinkOptionDictionary[selectedMatchup]);
                }
                else
                {
                    var messageBoxResult = MessageBox.Show("Video not found. Go to issues page?", "【MHF-Z】Overlay Information Missing", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    if (messageBoxResult.ToString() == "Yes")
                    {
                        var issueLink = "https://github.com/DorielRivalet/mhfz-overlay/issues/26";
                        var sInfo = new ProcessStartInfo(issueLink)
                        {
                            UseShellExecute = true,
                        };
                        Process.Start(sInfo);
                    }
                }

                return;
        }
    }

    private void MonsterNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.ChangeMonsterInfo();

    private void MonsterInfoSource_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.ChangeMonsterInfo();

    private void WeaponMatchupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.ChangeMonsterInfo();

    private void MonsterViewInfoOption_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.ChangeMonsterInfo();

    private readonly ListView? mostRecentRunsListView;

    private CartesianChart? weaponUsageChart;

    // TODO optimize
    private async Task GetRepoStats()
    {
        var info = this.client.GetLastApiInfo();

        if (info != null)
        {
            this.OctokitInfo.Text = string.Format(CultureInfo.InvariantCulture, "Server Time Difference: {0}, Max Requests/hr: {1}, Requests remaining: {2}, Current Rate Limit Window Reset: {3}", info.ServerTimeDifference, info.RateLimit.Limit, info.RateLimit.Remaining, info.RateLimit.Reset);
        }

        var issuesForOctokit = await this.client.Issue.GetAllForRepository("DorielRivalet", "MHFZ_Overlay");

        // TODO
        this.IssuesTextBlock.Text = (issuesForOctokit.Count - 2).ToString(CultureInfo.InvariantCulture) + " Issue(s)";

        var watchers = await this.client.Activity.Watching.GetAllWatchers("DorielRivalet", "MHFZ_Overlay");
        this.WatchersTextBlock.Text = watchers.Count.ToString(CultureInfo.InvariantCulture) + " Watcher(s)";

        info = this.client.GetLastApiInfo();

        if (info != null)
        {
            this.OctokitInfo.Text = string.Format(CultureInfo.InvariantCulture, "Server Time Difference: {0}, Max Requests/hr: {1}, Requests remaining: {2}, Current Rate Limit Window Reset: {3}", info.ServerTimeDifference, info.RateLimit.Limit, info.RateLimit.Remaining, info.RateLimit.Reset);
        }

        info = this.client.GetLastApiInfo();

        if (info != null)
        {
            this.OctokitInfo.Text = string.Format(CultureInfo.InvariantCulture, "Server Time Difference: {0}, Max Requests/hr: {1}, Requests remaining: {2}, Reset Time: {3}", info.ServerTimeDifference, info.RateLimit.Limit, info.RateLimit.Remaining, info.RateLimit.Reset);
        }
    }

    private void OpenOverlayFolder_Click(object sender, RoutedEventArgs e) => FileService.OpenApplicationFolder(this.ConfigWindowSnackBarPresenter, (Style)this.FindResource("CatppuccinMochaSnackBar"), this.SnackbarTimeOut);

    private void GenerateSpeedrunFiles_Click(object sender, RoutedEventArgs e) => FileService.GenerateSpeedrunFiles(this.ConfigWindowSnackBarPresenter, (Style)this.FindResource("CatppuccinMochaSnackBar"), this.SnackbarTimeOut);

    private void OpenSettingsFolder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var settingsFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            var settingsFileDirectoryName = Path.GetDirectoryName(settingsFile);
            if (!Directory.Exists(settingsFileDirectoryName))
            {
                Logger.Error(CultureInfo.InvariantCulture, "Could not open settings folder");
                var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
                {
                    Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
                    Title = Messages.ErrorTitle,
                    Content = "Could not open settings folder",
                    Appearance = ControlAppearance.Danger,
                    Icon = new SymbolIcon(SymbolRegular.ErrorCircle24),
                    Timeout = this.SnackbarTimeOut,
                };
                snackbar.Show();
                return;
            }

            var settingsFolder = settingsFileDirectoryName;

            // Open file manager at the specified folder
            Process.Start(ApplicationPaths.ExplorerPath, settingsFolder);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
            {
                Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
                Title = Messages.ErrorTitle,
                Content = "Could not open settings folder",
                Appearance = ControlAppearance.Danger,
                Icon = new SymbolIcon(SymbolRegular.ErrorCircle24),
                Timeout = this.SnackbarTimeOut,
            };
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
                Logger.Error(CultureInfo.InvariantCulture, "Could not find the log file: {0}", logFilePath);
                MessageBox.Show(string.Format(CultureInfo.InvariantCulture, "Could not find the log file: {0}", logFilePath), Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
                Logger.Error(ex);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }

    private void OpenDatabaseFolder_Click(object sender, RoutedEventArgs e)
    {
        // Open the file explorer at the directory
        try
        {
            Process.Start(ApplicationPaths.ExplorerPath, "\"" + DatabaseManager.GetDatabaseFolderPath() + "\"");
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }

    private void QuestLoggingToggle_Check(object sender, RoutedEventArgs e)
    {
        if (this.MainWindow == null)
        {
            return;
        }

        ViewModels.Windows.AddressModel.ValidateGameFolder();

        DatabaseService.CheckIfSchemaChanged(this.MainWindow.DataLoader);
    }

    private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        s.PlayerNationalityIndex = this.CountryComboBox.SelectedIndex;
    }

    private void QuestIDButton_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(this.QuestIDTextBox.Text))
        {
            this.SetDefaultInfoInQuestIDWeaponSection();
            DatabaseManager.QuestIDButtonClick(sender, e, this, (uint)GetRunBuffs(MainWindow.DataLoader.Model.RunBuffsSearchOption));
        }
    }

    private void SetDefaultInfoInQuestIDWeaponSection()
    {
        this.SwordAndShieldBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.SwordAndShieldRunIDTextBlock.Text = Messages.RunNotFound;

        this.GreatSwordBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.GreatSwordRunIDTextBlock.Text = Messages.RunNotFound;

        this.DualSwordsBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.DualSwordsRunIDTextBlock.Text = Messages.RunNotFound;

        this.LongSwordBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.LongSwordRunIDTextBlock.Text = Messages.RunNotFound;

        this.LanceBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.LanceRunIDTextBlock.Text = Messages.RunNotFound;

        this.GunlanceBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.GunlanceRunIDTextBlock.Text = Messages.RunNotFound;

        this.HammerBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.HammerRunIDTextBlock.Text = Messages.RunNotFound;

        this.HuntingHornBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.HuntingHornRunIDTextBlock.Text = Messages.RunNotFound;

        this.TonfaBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.TonfaRunIDTextBlock.Text = Messages.RunNotFound;

        this.SwitchAxeFBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.SwitchAxeFRunIDTextBlock.Text = Messages.RunNotFound;

        this.MagnetSpikeBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.MagnetSpikeRunIDTextBlock.Text = Messages.RunNotFound;

        this.LightBowgunBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.LightBowgunRunIDTextBlock.Text = Messages.RunNotFound;

        this.HeavyBowgunBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.HeavyBowgunRunIDTextBlock.Text = Messages.RunNotFound;

        this.BowBestTimeTextBlock.Text = Messages.TimerNotLoaded;
        this.BowRunIDTextBlock.Text = Messages.RunNotFound;

        this.SelectedQuestObjectiveImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/random.png"));
        this.SelectedQuestNameTextBlock.Text = Messages.QuestNotFound;
        this.SelectedQuestObjectiveTextBlock.Text = Messages.InvalidQuest;
        this.CurrentTimeTextBlock.Text = Messages.NotANumber;
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // nothing
    }

    private void QuestLogsSectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // nothing
    }

    private T? FindChild<T>(DependencyObject parent, string childName)
        where T : DependencyObject
    {
        // Confirm parent and childName are valid.
        if (parent == null)
        {
            return null;
        }

        T? foundChild = null;

        var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            // If the child is not of the request child type child
            if (child is not T childType)
            {
                // recursively drill down the tree
                foundChild = this.FindChild<T>(child, childName);

                // If the child is found, break so we do not overwrite the found child.
                if (foundChild != null)
                {
                    break;
                }
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                // If the child's name is set for search
                if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
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
        var comboBox = (ComboBox)sender;
        if (this.weaponUsageChart == null)
        {
            return;
        }

        this.MainWindow.DataLoader.Model.WeaponUsageEarthStyle.Clear();
        this.MainWindow.DataLoader.Model.WeaponUsageHeavenStyle.Clear();
        this.MainWindow.DataLoader.Model.WeaponUsageStormStyle.Clear();
        this.MainWindow.DataLoader.Model.WeaponUsageExtremeStyle.Clear();

        if (comboBox.SelectedIndex == 0)
        {
            this.weaponUsageData = DatabaseManager.CalculateTotalWeaponUsage(this, this.MainWindow.DataLoader);
        }
        else if (comboBox.SelectedIndex == 1)
        {
            this.weaponUsageData = DatabaseManager.CalculateTotalWeaponUsage(this, this.MainWindow.DataLoader, true);
        }
        else
        {
            return;
        }

        var weaponStyles = new[] { "Earth Style", "Heaven Style", "Storm Style", "Extreme Style" };
        var weaponTypes = new[]
        {
            "Sword and Shield", "Dual Swords", "Great Sword", "Long Sword",
            "Hammer", "Hunting Horn", "Lance", "Gunlance", "Tonfa",
            "Switch Axe F", "Magnet Spike", "Light Bowgun", "Heavy Bowgun",
            "Bow",
        };

        foreach (var weaponType in weaponTypes)
        {
            foreach (var weaponStyle in weaponStyles)
            {
                switch (weaponStyle)
                {
                    case "Earth Style":
                        var weaponUsageCount = this.weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        this.MainWindow.DataLoader.Model.WeaponUsageEarthStyle.Add(weaponUsageCount);
                        break;
                    case "Heaven Style":
                        weaponUsageCount = this.weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        this.MainWindow.DataLoader.Model.WeaponUsageHeavenStyle.Add(weaponUsageCount);
                        break;
                    case "Storm Style":
                        weaponUsageCount = this.weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        this.MainWindow.DataLoader.Model.WeaponUsageStormStyle.Add(weaponUsageCount);
                        break;
                    case "Extreme Style":
                        weaponUsageCount = this.weaponUsageData.Where(x => x.WeaponType == weaponType && x.Style == weaponStyle)
                                                                      .Sum(x => x.RunCount);
                        this.MainWindow.DataLoader.Model.WeaponUsageExtremeStyle.Add(weaponUsageCount);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Does not count multi-monster quests.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void QuestPaceWeaponComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.questPaceGraph == null)
        {
            return;
        }

        if (sender is not ComboBox comboBox)
        {
            return;
        }

        var selectedItem = comboBox.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }

        // You can now use the selectedItem variable to get the data or value of the selected option
        var selectedWeapon = selectedItem.ToString()?.Replace("System.Windows.Controls.ComboBoxItem: ", string.Empty);
        if (string.IsNullOrEmpty(selectedWeapon))
        {
            return;
        }

        this.questPaceWeaponSelected = selectedWeapon;

        var allQuestsRuns = DatabaseManager.GetQuests(long.Parse(this.QuestIDTextBox.Text.Trim()), this.questPaceWeaponSelected, this.OverlayModeComboBox.Text.Trim(), (uint)GetRunBuffs(this.MainWindow.DataLoader.Model.RunBuffsSearchOption));

        if (allQuestsRuns.Count == 0)
        {
            return;
        }

        List<Models.Quest> soloQuests = allQuestsRuns.Where(q => q.PartySize == 1).ToList();

        if (soloQuests.Count == 0)
        {
            return;
        }

        List<QuestPace> monster1HPList = GetMonster1HPListForQuestPace(soloQuests);

        // Filter the quest runs where the monster ID stayed the same
        var consistentMonsterIdRuns = monster1HPList.Where(questRun =>
            questRun.MonsterHPField.Values
                .Select(m => m.Keys.FirstOrDefault())
                .Distinct()
                .Count() == 1)
            .ToList();

        List<QuestPace> flattenedList = GetFlattenedListForQuestPace(consistentMonsterIdRuns);

        // TODO this is not removing all outliers
        List<QuestPace> removedOutliersList = GetRemovedOutliersListForQuestPace(flattenedList);

        List<QuestPace> elapsedTimeList = new ();
        foreach (var run in removedOutliersList)
        {
            QuestPace pace = run;
            if (pace.MonsterHPFieldFlattened == null)
            {
                continue;
            }
            var elapsedTimeRun = GetElapsedTimeForDictionaryIntInt(pace.MonsterHPFieldFlattened);
            pace.MonsterHPFieldFlattened = elapsedTimeRun;
            elapsedTimeList.Add(pace);
        }

        List<QuestPace> hpPercentagesList = GetHPPercentagesListForQuestPace(elapsedTimeList);

        if (selectedQuestPaceSplitOption == "Every 10% Monster HP Dealt")
        {
            List<QuestPace> questsSplits = GetQuestsSplitsListForQuestPace(hpPercentagesList);

            QuestSplit medianSplitTimes = GetMedianSplitTimes(questsSplits);

            // TODO test
            QuestSplit fastestSplitTimes = GetFastestSplitTimes(questsSplits);

            // Now, fastestSplitTimes contains the fastest split times for each property

            var sumOfMedian = medianSplitTimes.Sum() ?? 0;
            List<long?> finalTimeValues = soloQuests
                .Select(quest => quest.FinalTimeValue)
                .ToList();

            var personalBest = finalTimeValues.Min() ?? 0;

            finalTimeValues.Sort();
            long medianFinalTimeValue = CalculateMedian(finalTimeValues);

            long? sumOfBest = (long?)fastestSplitTimes.Sum() ?? 0;

            MakeQuestPaceGraph(questsSplits, medianSplitTimes);

            if (this.questPaceDescriptionTextBlock != null)
            {
                this.questPaceDescriptionTextBlock.Text = @$"Quest ID {this.QuestIDTextBox.Text} pace by category {this.OverlayModeComboBox.Text} | Personal Best: {TimeService.GetMinutesSecondsMillisecondsFromFrames((double)personalBest)} | Sum of Best: {TimeService.GetMinutesSecondsMillisecondsFromFrames((long)sumOfBest)} | Sum Of Median: {TimeService.GetMinutesSecondsMillisecondsFromFrames((long)sumOfMedian)} | Median: {TimeService.GetMinutesSecondsMillisecondsFromFrames(medianFinalTimeValue)}

Run IDs with best paces for each HP% Dealt:
10%: {GetFastestRunIDPaceForHPPercent(10, questsSplits)}
20%: {GetFastestRunIDPaceForHPPercent(20, questsSplits)}
30%: {GetFastestRunIDPaceForHPPercent(30, questsSplits)}
40%: {GetFastestRunIDPaceForHPPercent(40, questsSplits)}
50%: {GetFastestRunIDPaceForHPPercent(50, questsSplits)}
60%: {GetFastestRunIDPaceForHPPercent(60, questsSplits)}
70%: {GetFastestRunIDPaceForHPPercent(70, questsSplits)}
80%: {GetFastestRunIDPaceForHPPercent(80, questsSplits)}
90%: {GetFastestRunIDPaceForHPPercent(90, questsSplits)}
100%: {GetFastestRunIDPaceForHPPercent(100, questsSplits)}
";
            }

        } else if (selectedQuestPaceSplitOption == "60%/80%/100% Monster HP Dealt")
        {
            List<QuestPace> hpPercentagesQuestObjectiveList = GetHPPercentagesQuestObjectiveListForQuestPace(hpPercentagesList);

            List<QuestPace> questsSplits = GetQuestObjectivesSplitsListForQuestPace(hpPercentagesQuestObjectiveList);

            QuestObjectiveSplit medianSplitTimes = GetMedianSplitObjectiveTimes(questsSplits);

            // TODO test
            QuestObjectiveSplit fastestSplitTimes = GetFastestSplitObjectiveTimes(questsSplits);

            // Now, fastestSplitTimes contains the fastest split times for each property

            var sumOfMedian = medianSplitTimes.Sum() ?? 0;
            List<long?> finalTimeValues = soloQuests
                .Select(quest => quest.FinalTimeValue)
                .ToList();

            var personalBest = finalTimeValues.Min() ?? 0;

            finalTimeValues.Sort();
            long medianFinalTimeValue = CalculateMedian(finalTimeValues);

            long? sumOfBest = (long?)fastestSplitTimes.Sum() ?? 0;

            MakeQuestPaceGraph(questsSplits, medianSplitTimes);

            if (this.questPaceDescriptionTextBlock != null)
            {
                this.questPaceDescriptionTextBlock.Text = @$"Quest ID {this.QuestIDTextBox.Text} pace by category {this.OverlayModeComboBox.Text} | Personal Best: {TimeService.GetMinutesSecondsMillisecondsFromFrames((double)personalBest)} | Sum of Best: {TimeService.GetMinutesSecondsMillisecondsFromFrames((long)sumOfBest)} | Sum Of Median: {TimeService.GetMinutesSecondsMillisecondsFromFrames((long)sumOfMedian)} | Median: {TimeService.GetMinutesSecondsMillisecondsFromFrames(medianFinalTimeValue)}

Run IDs with best paces for each HP% Dealt:
60%: {GetFastestRunIDPaceForHPPercent(60, questsSplits, true)}
80%: {GetFastestRunIDPaceForHPPercent(80, questsSplits, true)}
100%: {GetFastestRunIDPaceForHPPercent(100, questsSplits, true)}
";
            }
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Finds the minimum elapsed frames for a certain HP threshold. The fastest pace for that tier.
    /// </summary>
    /// <param name="percent"></param>
    /// <param name="questsSplits"></param>
    /// <returns></returns>
    private long GetFastestRunIDPaceForHPPercent(int percent, List<QuestPace> questsSplits, bool byObjective = false)
    {
        long runID = 0;
        int? fastestPace = null;

        foreach (var questSplit in questsSplits)
        {
            // Assuming `questSplit.Splits` is a QuestSplit object containing HP percent frame data
            int? currentPace = null;
            if (byObjective)
            {
                switch (percent)
                {
                    case 60:
                        currentPace = questSplit.ObjectiveSplits?.FortyPercentRemainingHPFrames;
                        break;
                    case 80:
                        currentPace = questSplit.ObjectiveSplits?.TwentyPercentRemainingHPFrames;
                        break;
                    case 100:
                        currentPace = questSplit.ObjectiveSplits?.ZeroPercentRemainingHPFrames;
                        break;
                    default:
                        return runID;
                }
            }
            else
            {
                switch (percent)
                {
                    case 10:
                        currentPace = questSplit.Splits?.NinetyPercentRemainingHPFrames;
                        break;
                    case 20:
                        currentPace = questSplit.Splits?.EightyPercentRemainingHPFrames;
                        break;
                    case 30:
                        currentPace = questSplit.Splits?.SeventyPercentRemainingHPFrames;
                        break;
                    case 40:
                        currentPace = questSplit.Splits?.SixtyPercentRemainingHPFrames;
                        break;
                    case 50:
                        currentPace = questSplit.Splits?.FiftyPercentRemainingHPFrames;
                        break;
                    case 60:
                        currentPace = questSplit.Splits?.FortyPercentRemainingHPFrames;
                        break;
                    case 70:
                        currentPace = questSplit.Splits?.ThirtyPercentRemainingHPFrames;
                        break;
                    case 80:
                        currentPace = questSplit.Splits?.TwentyPercentRemainingHPFrames;
                        break;
                    case 90:
                        currentPace = questSplit.Splits?.TenPercentRemainingHPFrames;
                        break;
                    case 100:
                        currentPace = questSplit.Splits?.ZeroPercentRemainingHPFrames;
                        break;
                    default:
                        return runID;
                }
            }
            

            if (currentPace.HasValue && (!fastestPace.HasValue || currentPace < fastestPace))
            {
                fastestPace = currentPace;
                runID = questSplit.RunID;
            }
        }

        return runID;
    }

    private void MakeQuestPaceGraph(List<QuestPace> questsSplits, QuestSplit medianSplitTimes)
    {
        List<QuestPace> runPaces = GetQuestRunPaces(questsSplits, medianSplitTimes);

        if (this.questPaceGraph == null)
        {
            return;
        }

        List<ISeries> series = new();

        //var newHP = GetElapsedTime(hp);

        // get the minimum value of ZeroPercentRemainingHPFrames in runPaces.
        var personalBestTime = runPaces.Min(rp => rp.Splits.ZeroPercentRemainingHPFrames);

        foreach (var run in runPaces)
        {
            ObservableCollection<ObservablePoint> paceCollection = new();
            if (run.Splits == null)
            {
                continue;
            }
            paceCollection.Add(new ObservablePoint(10, run.Splits.NinetyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(20, run.Splits.EightyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(30, run.Splits.SeventyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(40, run.Splits.SixtyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(50, run.Splits.FiftyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(60, run.Splits.FortyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(70, run.Splits.ThirtyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(80, run.Splits.TwentyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(90, run.Splits.TenPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(100, run.Splits.ZeroPercentRemainingHPFrames));

            var stroke = new SolidColorPaint();

            if (personalBestTime == run.Splits.ZeroPercentRemainingHPFrames)
            {
                stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 };
            }
            else
            {
                stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa"))) { StrokeThickness = 2 };
            }

            series.Add(new LineSeries<ObservablePoint>
            {
                Values = paceCollection,
                LineSmoothness = 0,
                GeometrySize = 0,
                
                // TODO tooltip?
                //TooltipLabelFormatter = (chartPoint) =>
                //$"Health: {(long)chartPoint.PrimaryValue}",
                Stroke = stroke,
                Fill = null,
            });
        }

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                NameTextSize = 12,
                Name = "HP% Dealt",
                Labeler = (value) => string.Format("{0}%", value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                Name = "Pace at x% HP",
                NameTextSize = 12,
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.questPaceGraph.Series = series;
        this.questPaceGraph.XAxes = this.XAxes;
        this.questPaceGraph.YAxes = this.YAxes;
        this.questPaceGraph.TooltipPosition = TooltipPosition.Hidden;
    }

    private void MakeQuestPaceGraph(List<QuestPace> questsSplits, QuestObjectiveSplit medianSplitTimes)
    {
        if (this.questPaceGraph == null)
        {
            return;
        }

        List<QuestPace> runPaces = GetQuestRunPaces(questsSplits, medianSplitTimes);

        if (runPaces.Count == 0)
        {
            return;
        }

        List<ISeries> series = new();

        //var newHP = GetElapsedTime(hp);
        // get the minimum value of ZeroPercentRemainingHPFrames in runPaces.
        var personalBestTime = runPaces.Min(rp => rp.ObjectiveSplits.ZeroPercentRemainingHPFrames);

        foreach (var run in runPaces)
        {
            ObservableCollection<ObservablePoint> paceCollection = new();
            if (run.ObjectiveSplits == null)
            {
                continue;
            }
            paceCollection.Add(new ObservablePoint(60, run.ObjectiveSplits.FortyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(80, run.ObjectiveSplits.TwentyPercentRemainingHPFrames));
            paceCollection.Add(new ObservablePoint(100, run.ObjectiveSplits.ZeroPercentRemainingHPFrames));

            var stroke = new SolidColorPaint();

            if (personalBestTime == run.ObjectiveSplits.ZeroPercentRemainingHPFrames)
            {
                stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 };
            }
            else
            {
                stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa"))) { StrokeThickness = 2 };
            }

            series.Add(new LineSeries<ObservablePoint>
            {
                Values = paceCollection,
                LineSmoothness = 0,
                GeometrySize = 0,
                // TODO tooltip?
                //TooltipLabelFormatter = (chartPoint) =>
                //$"Health: {(long)chartPoint.PrimaryValue}",
                Stroke = stroke,
                Fill = null,
            });
        }

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                NameTextSize = 12,
                Name = "HP% Dealt",
                Labeler = (value) => string.Format("{0}%", value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                Name = "Pace at x% HP",
                NameTextSize = 12,
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.questPaceGraph.Series = series;
        this.questPaceGraph.XAxes = this.XAxes;
        this.questPaceGraph.YAxes = this.YAxes;
        this.questPaceGraph.TooltipPosition = TooltipPosition.Hidden;
    }

    private List<QuestPace> GetQuestRunPaces(List<QuestPace> questsSplits, QuestSplit medianSplitTimes)
    {
        List<QuestPace> runPaces = new();

        foreach(var run in questsSplits)
        {
            // this is for calculating pace
            QuestPace questPace = run;
            QuestSplit paceSplits = new();
            var runPace = run.Splits;

            if (runPace == null)
            {
                continue;
            }

            // the pace at x% hp is its value in the current run + all of the rest median values for the other %.
            paceSplits.NinetyPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + medianSplitTimes.EightyPercentRemainingHPFrames + medianSplitTimes.SeventyPercentRemainingHPFrames + medianSplitTimes.SixtyPercentRemainingHPFrames + medianSplitTimes.FiftyPercentRemainingHPFrames + medianSplitTimes.FortyPercentRemainingHPFrames + medianSplitTimes.ThirtyPercentRemainingHPFrames + medianSplitTimes.TwentyPercentRemainingHPFrames + medianSplitTimes.TenPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            // the next pace is the same, but we include the value of the previous % tier.
            paceSplits.EightyPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + runPace.EightyPercentRemainingHPFrames + medianSplitTimes.SeventyPercentRemainingHPFrames + medianSplitTimes.SixtyPercentRemainingHPFrames + medianSplitTimes.FiftyPercentRemainingHPFrames + medianSplitTimes.FortyPercentRemainingHPFrames + medianSplitTimes.ThirtyPercentRemainingHPFrames + medianSplitTimes.TwentyPercentRemainingHPFrames + medianSplitTimes.TenPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            // include the rest
            paceSplits.SeventyPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + runPace.EightyPercentRemainingHPFrames + runPace.SeventyPercentRemainingHPFrames + medianSplitTimes.SixtyPercentRemainingHPFrames + medianSplitTimes.FiftyPercentRemainingHPFrames + medianSplitTimes.FortyPercentRemainingHPFrames + medianSplitTimes.ThirtyPercentRemainingHPFrames + medianSplitTimes.TwentyPercentRemainingHPFrames + medianSplitTimes.TenPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            paceSplits.SixtyPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + runPace.EightyPercentRemainingHPFrames + runPace.SeventyPercentRemainingHPFrames + runPace.SixtyPercentRemainingHPFrames + medianSplitTimes.FiftyPercentRemainingHPFrames + medianSplitTimes.FortyPercentRemainingHPFrames + medianSplitTimes.ThirtyPercentRemainingHPFrames + medianSplitTimes.TwentyPercentRemainingHPFrames + medianSplitTimes.TenPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            paceSplits.FiftyPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + runPace.EightyPercentRemainingHPFrames + runPace.SeventyPercentRemainingHPFrames + runPace.SixtyPercentRemainingHPFrames + runPace.FiftyPercentRemainingHPFrames + medianSplitTimes.FortyPercentRemainingHPFrames + medianSplitTimes.ThirtyPercentRemainingHPFrames + medianSplitTimes.TwentyPercentRemainingHPFrames + medianSplitTimes.TenPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            paceSplits.FortyPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + runPace.EightyPercentRemainingHPFrames + runPace.SeventyPercentRemainingHPFrames + runPace.SixtyPercentRemainingHPFrames + runPace.FiftyPercentRemainingHPFrames + runPace.FortyPercentRemainingHPFrames + medianSplitTimes.ThirtyPercentRemainingHPFrames + medianSplitTimes.TwentyPercentRemainingHPFrames + medianSplitTimes.TenPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            paceSplits.ThirtyPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + runPace.EightyPercentRemainingHPFrames + runPace.SeventyPercentRemainingHPFrames + runPace.SixtyPercentRemainingHPFrames + runPace.FiftyPercentRemainingHPFrames + runPace.FortyPercentRemainingHPFrames + medianSplitTimes.ThirtyPercentRemainingHPFrames + medianSplitTimes.TwentyPercentRemainingHPFrames + medianSplitTimes.TenPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            paceSplits.TwentyPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + runPace.EightyPercentRemainingHPFrames + runPace.SeventyPercentRemainingHPFrames + runPace.SixtyPercentRemainingHPFrames + runPace.FiftyPercentRemainingHPFrames + runPace.FortyPercentRemainingHPFrames + runPace.ThirtyPercentRemainingHPFrames + runPace.TwentyPercentRemainingHPFrames + medianSplitTimes.TenPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            paceSplits.TenPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + runPace.EightyPercentRemainingHPFrames + runPace.SeventyPercentRemainingHPFrames + runPace.SixtyPercentRemainingHPFrames + runPace.FiftyPercentRemainingHPFrames + runPace.FortyPercentRemainingHPFrames + runPace.ThirtyPercentRemainingHPFrames + runPace.TwentyPercentRemainingHPFrames + runPace.TenPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            paceSplits.ZeroPercentRemainingHPFrames = runPace.NinetyPercentRemainingHPFrames + runPace.EightyPercentRemainingHPFrames + runPace.SeventyPercentRemainingHPFrames + runPace.SixtyPercentRemainingHPFrames + runPace.FiftyPercentRemainingHPFrames + runPace.FortyPercentRemainingHPFrames + runPace.ThirtyPercentRemainingHPFrames + runPace.TwentyPercentRemainingHPFrames + runPace.TenPercentRemainingHPFrames + runPace.ZeroPercentRemainingHPFrames;

            // TODO this looks confusing
            questPace.Splits = paceSplits;
            runPaces.Add(questPace);
        }

        return runPaces;
    }

    private List<QuestPace> GetQuestRunPaces(List<QuestPace> questsSplits, QuestObjectiveSplit medianSplitTimes)
    {
        List<QuestPace> runPaces = new();

        foreach (var run in questsSplits)
        {
            QuestPace questPace = run;
            QuestObjectiveSplit paceSplits = new();
            var runPace = run.ObjectiveSplits;

            if (runPace == null)
            {
                continue;
            }

            // the pace at x% hp is its value in the current run + all of the rest median values for the other %.
            paceSplits.FortyPercentRemainingHPFrames = runPace.FortyPercentRemainingHPFrames +  medianSplitTimes.TwentyPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            paceSplits.TwentyPercentRemainingHPFrames = runPace.FortyPercentRemainingHPFrames + runPace.TwentyPercentRemainingHPFrames + medianSplitTimes.ZeroPercentRemainingHPFrames;

            paceSplits.ZeroPercentRemainingHPFrames = runPace.FortyPercentRemainingHPFrames + runPace.TwentyPercentRemainingHPFrames + runPace.ZeroPercentRemainingHPFrames;

            questPace.ObjectiveSplits = paceSplits;
            runPaces.Add(questPace);
        }

        return runPaces;
    }

    string? selectedQuestPaceSplitOption = "Every 10% Monster HP Dealt";

    private void QuestPaceSplitOptionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.questPaceGraph == null)
        {
            return;
        }

        if (sender is not ComboBox comboBox)
        {
            return;
        }

        ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }

        selectedQuestPaceSplitOption = selectedItem.Content.ToString();
    }

    private List<QuestPace> GetMonster1HPListForQuestPace(List<Models.Quest> quests)
    {
        List<QuestPace> monster1HPList = new();
        foreach (var quest in quests)
        {
            QuestPace pace = new();

            if (quest.Monster1HPDictionary != null)
            {

                var monster1HPDictionary = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(quest.Monster1HPDictionary);
                if (monster1HPDictionary != null)
                {
                    pace.MonsterHPField = monster1HPDictionary;
                }

                if (quest.RunID == null)
                {
                    continue;
                }

                pace.RunID = (long)quest.RunID;
                monster1HPList.Add(pace);
            }
        }
        return monster1HPList;
    }

    private List<QuestPace> GetFlattenedListForQuestPace(List<QuestPace> consistentMonsterIdRuns)
    {
        List<QuestPace> flattenedList = new();

        foreach (var questRun in consistentMonsterIdRuns)
        {
            QuestPace pace = questRun;

            // Create a new dictionary for each quest run
            Dictionary<int, int> run = new Dictionary<int, int>();

            if (questRun.MonsterHPField == null)
            {
                continue;
            }

            foreach (var kvp in questRun.MonsterHPField)
            {
                // kvp.Key is the time in frames
                // kvp.Value is the Dictionary<int, int> where the Key is the monster ID and Value is the HP
                // Since the monster ID is constant throughout the quest run, we can take any of the inner dictionary's Value
                run.Add(kvp.Key, kvp.Value.Values.First());
            }

            // Add the flattened run to the list
            pace.MonsterHPFieldFlattened = run;
            flattenedList.Add(pace);
        }

        return flattenedList;
    }

    private List<QuestPace> GetRemovedOutliersListForQuestPace(List<QuestPace> flattenedList)
    {
        List<QuestPace> removedOutliersList = new();

        foreach (var questRun in flattenedList)
        {
            QuestPace pace = questRun;

            if (questRun.MonsterHPFieldFlattened == null)
            {
                continue;
            }

            if (questRun.MonsterHPFieldFlattened.Count == 0)
            {
                continue;
            }

            var filteredRun = new Dictionary<int, int>();
            int? previousHp = null;
            int? lastValidHp = null;
            bool isOutlier = false;

            foreach (var frame in questRun.MonsterHPFieldFlattened.OrderByDescending(kvp => kvp.Key))
            {
                int currentHp = frame.Value;
                int currentTime = frame.Key;

                if (previousHp.HasValue)
                {
                    if (previousHp - currentHp > questRun.MonsterHPFieldFlattened.Values.First() * 0.25)
                    {
                        // This is a potential outlier, skip adding and mark the flag
                        isOutlier = true;
                    }
                    else
                    {
                        if (isOutlier && currentHp > previousHp.Value)
                        {
                            // The previous point was an outlier, don't add it
                            isOutlier = false;
                        }
                        else
                        {
                            // This is not an outlier, add the last valid HP if it was skipped
                            if (lastValidHp.HasValue)
                            {
                                filteredRun[currentTime - 1] = lastValidHp.Value;
                                lastValidHp = null;
                            }
                            filteredRun.Add(currentTime, currentHp);
                        }
                    }
                }
                else if (currentHp == 0)
                {
                    // If the first HP value is 0, discard the whole run
                    filteredRun.Clear();
                    break;
                }
                else
                {
                    // This is the first value, set it as the max HP
                    filteredRun.Add(currentTime, currentHp);
                }

                // If not marked as an outlier, update last valid HP
                if (!isOutlier)
                {
                    previousHp = currentHp;
                }
                else
                {
                    // Keep the last valid HP in case the next one is not an outlier
                    lastValidHp = previousHp;
                    previousHp = null;
                }
            }

            if (filteredRun.Any())
            {
                pace.MonsterHPFieldFlattened = filteredRun;
                removedOutliersList.Add(pace);
            }
        }

        return removedOutliersList;
    }

    private List<QuestPace> GetHPPercentagesListForQuestPace(List<QuestPace> elapsedTimeList)
    {
        List<QuestPace> hpPercentagesList = new ();

        foreach (var run in elapsedTimeList)
        {
            QuestPace pace = run;

            Dictionary<int, (int, int)> hpPercentValues = new()
            {
                {0, (0,0) },
                {1, (0,0) },
                {2, (0,0) },
                {3, (0,0) },
                {4, (0,0) },
                {5, (0,0) },
                {6, (0,0) },
                {7, (0,0) },
                {8, (0,0) },
                {9, (0,0) },
            };

            if (run.MonsterHPFieldFlattened == null)
            {
                continue;
            }

            var maxHP = run.MonsterHPFieldFlattened.Values.First();
            int previousHP = maxHP;

            foreach (var entry in run.MonsterHPFieldFlattened)
            {
                if (previousHP < entry.Value)
                {
                    // previousHP = entry.Value;
                    continue;
                }

                if (previousHP - entry.Value <= 0)
                {
                    continue;
                }

                for (int i = 9; i >= 0; i--)
                {
                    if (entry.Value >= maxHP * (i / 10.0))
                    {
                        hpPercentValues[9 - i] = (entry.Key, entry.Value);
                        break;
                    }
                }
            }

            // add as dictionary<int,int>
            Dictionary<int, int> hpPercentValuesFlattened = new();
            foreach (var entry in hpPercentValues)
            {
                hpPercentValuesFlattened.Add(entry.Value.Item1, entry.Value.Item2);
            }

            pace.MonsterHPFieldFlattened = hpPercentValuesFlattened;
            hpPercentagesList.Add(pace);
        }

        return hpPercentagesList;
    }

    private List<QuestPace> GetQuestsSplitsListForQuestPace(List<QuestPace> hpPercentagesList)
    {
        List<QuestPace> questsSplits = new();

        // hpPercentagesList is a List<Dictionary<int, int>> containing the frames elapsed in total in first int and hp in second int
        foreach (var run in hpPercentagesList)
        {
            QuestPace pace = run;
            // get the frames elapsed in each split. a split is each entry in the dictionary<int,int>
            // with first int as frames elapsed and second int the hp remaining.
            QuestSplit splitTimesFrames = new();
            int index = 0;
            int previousKeyFramesElapsed = 0;
            int framesElapsed = 0;

            if (run.MonsterHPFieldFlattened == null)
            {
                continue;
            }

            foreach (var entry in run.MonsterHPFieldFlattened)
            {
                //int hpPercent = entry.Value;    // Assuming the value represents the HP percentage

                // Assuming the key represents the frames elapsed
                // TODO test
                framesElapsed = entry.Key - previousKeyFramesElapsed;
                previousKeyFramesElapsed = entry.Key;

                // Populate splitTimesFrames fields based on the HP percentage
                PopulateQuestSplitField(splitTimesFrames, index, framesElapsed);

                index++;
            }

            pace.Splits = splitTimesFrames;
            questsSplits.Add(pace);
        }

        return questsSplits;
    }

    private List<QuestPace> GetHPPercentagesQuestObjectiveListForQuestPace(List<QuestPace> hpPercentagesList)
    {
        List<QuestPace> hpPercentagesQuestObjectiveList = new();

        // each dictionary entry has 10 entries.
        // we want to remove all except the 6th, 8th, and 10th entry.
        // into a new list.
        foreach(var run in hpPercentagesList)
        {
            QuestPace pace = run;
            var filteredRun = run.MonsterHPFieldFlattened
                .Where((entry, index) => index == 5 || index == 7 || index == 9) // index is zero-based, so 5, 7, and 9 correspond to the 6th, 8th, and 10th entries
                .ToDictionary(entry => entry.Key, entry => entry.Value);

            pace.MonsterHPFieldFlattened = filteredRun;
            hpPercentagesQuestObjectiveList.Add(pace);
        }

        return hpPercentagesQuestObjectiveList;
    }

    private List<QuestPace> GetQuestObjectivesSplitsListForQuestPace(List<QuestPace> hpPercentagesList)
    {
        List<QuestPace> questsSplits = new();

        // hpPercentagesList is a List<Dictionary<int, int>> containing the frames elapsed in total in first int and hp in second int
        foreach (var run in hpPercentagesList)
        {
            QuestPace pace = run;
            // get the frames elapsed in each split. a split is each entry in the dictionary<int,int>
            // with first int as frames elapsed and second int the hp remaining.
            QuestObjectiveSplit splitTimesFrames = new();
            int index = 0;
            int previousKeyFramesElapsed = 0;
            int framesElapsed = 0;

            if (run.MonsterHPFieldFlattened == null)
            {
                continue;
            }

            foreach (var entry in run.MonsterHPFieldFlattened)
            {
                //int hpPercent = entry.Value;    // Assuming the value represents the HP percentage

                // Assuming the key represents the frames elapsed
                // TODO test
                framesElapsed = entry.Key - previousKeyFramesElapsed;
                previousKeyFramesElapsed = entry.Key;

                // Populate splitTimesFrames fields based on the HP percentage
                PopulateQuestSplitField(splitTimesFrames, index, framesElapsed);

                index++;
            }

            pace.ObjectiveSplits = splitTimesFrames;
            questsSplits.Add(pace);
        }

        return questsSplits;
    }

    // Update the PopulateQuestSplitField method to handle new cases if necessary.


    private QuestSplit GetMedianSplitTimes(List<QuestPace> questsSplits)
    {
        QuestSplit medianSplitTimes = new();
        // get the median of each questsplit field from questsSplits.
        // Calculate median for each property
        foreach (var property in typeof(QuestSplit).GetProperties())
        {
            if (property.PropertyType == typeof(int?))
            {
                // Get all non-null values for the property from questsSplits
                List<int> propertyValues = questsSplits
                    .Select(split => split.Splits) // Access the Splits property
                    .Where(split => property.GetValue(split) != null)
                    .Select(split => (int)property.GetValue(split))
                    .ToList();

                // Sort the values to calculate the median
                propertyValues.Sort();

                // Calculate the median
                int medianValue = CalculateMedian(propertyValues);

                // Set the median value to the property in medianSplitTimes
                property.SetValue(medianSplitTimes, medianValue);
            }
        }

        return medianSplitTimes;
    }

    private QuestObjectiveSplit GetMedianSplitObjectiveTimes(List<QuestPace> questsSplits)
    {
        QuestObjectiveSplit medianSplitTimes = new();
        // get the median of each questsplit field from questsSplits.
        // Calculate median for each property
        foreach (var property in typeof(QuestObjectiveSplit).GetProperties())
        {
            if (property.PropertyType == typeof(int?))
            {
                // Get all non-null values for the property from questsSplits
                List<int> propertyValues = questsSplits
                    .Select(split => split.ObjectiveSplits)
                    .Where(split => property.GetValue(split) != null)
                    .Select(split => (int)property.GetValue(split))
                    .ToList();

                // Sort the values to calculate the median
                propertyValues.Sort();

                // Calculate the median
                int medianValue = CalculateMedian(propertyValues);

                // Set the median value to the property in medianSplitTimes
                property.SetValue(medianSplitTimes, medianValue);
            }
        }

        return medianSplitTimes;
    }

    private QuestSplit GetFastestSplitTimes(List<QuestPace> questsSplits)
    {
        QuestSplit fastestSplitTimes = new();

        // Get the fastest value for each property from questsSplits
        foreach (var property in typeof(QuestSplit).GetProperties())
        {
            if (property.PropertyType == typeof(int?))
            {
                // Get all non-null values for the property from questsSplits
                List<int> propertyValues = questsSplits
                    .Select(split => split.Splits)
                    .Where(split => property.GetValue(split) != null)
                    .Select(split => (int)property.GetValue(split))
                    .ToList();

                if (propertyValues.Any())
                {
                    // Find the fastest (lowest) value
                    int fastestValue = propertyValues.Min();

                    // Set the fastest value to the property in fastestSplitTimes
                    property.SetValue(fastestSplitTimes, fastestValue);
                }
            }
        }

        return fastestSplitTimes;
    }

    private QuestObjectiveSplit GetFastestSplitObjectiveTimes(List<QuestPace> questsSplits)
    {
        QuestObjectiveSplit fastestSplitTimes = new();

        // Get the fastest value for each property from questsSplits
        foreach (var property in typeof(QuestObjectiveSplit).GetProperties())
        {
            if (property.PropertyType == typeof(int?))
            {
                // Get all non-null values for the property from questsSplits
                List<int> propertyValues = questsSplits
                    .Select(split => split.ObjectiveSplits)
                    .Where(split => property.GetValue(split) != null)
                    .Select(split => (int)property.GetValue(split))
                    .ToList();

                if (propertyValues.Any())
                {
                    // Find the fastest (lowest) value
                    int fastestValue = propertyValues.Min();

                    // Set the fastest value to the property in fastestSplitTimes
                    property.SetValue(fastestSplitTimes, fastestValue);
                }
            }
        }

        return fastestSplitTimes;
    }

    private static int CalculateMedian(List<int> values)
    {
        int count = values.Count;
        if (count <= 0)
        {
            return 0;
        }
        else if (count == 1)
        {
            return values.FirstOrDefault();
        }

        int middle = count / 2;

        if (count % 2 == 0)
        {
            // For an even number of values, take the average of the two middle values
            return (values[middle - 1] + values[middle]) / 2;
        }
        else
        {
            // For an odd number of values, return the middle value
            return values[middle];
        }
    }

    private static long CalculateMedian(List<long?> values)
    {
        var count = values.Count;
        if (count <= 0)
        {
            return 0;
        }
        else if (count == 1)
        {
            return (long)values.FirstOrDefault();
        }
        var middle = count / 2;

        if (count % 2 == 0)
        {
            // For an even number of values, take the average of the two middle values
            return (long)((values[middle - 1] + values[middle]) / 2);
        }
        else
        {
            // For an odd number of values, return the middle value
            return (long)values[middle];
        }
    }

    private static void PopulateQuestSplitField(QuestSplit questSplit, int index, int framesElapsed)
    {
        switch (index)
        {
            case 0:
                questSplit.NinetyPercentRemainingHPFrames = framesElapsed;
                break;
            case 1:
                questSplit.EightyPercentRemainingHPFrames = framesElapsed;
                break;
            case 2:
                questSplit.SeventyPercentRemainingHPFrames = framesElapsed;
                break;
            case 3:
                questSplit.SixtyPercentRemainingHPFrames = framesElapsed;
                break;
            case 4:
                questSplit.FiftyPercentRemainingHPFrames = framesElapsed;
                break;
            case 5:
                questSplit.FortyPercentRemainingHPFrames = framesElapsed;
                break;
            case 6:
                questSplit.ThirtyPercentRemainingHPFrames = framesElapsed;
                break;
            case 7:
                questSplit.TwentyPercentRemainingHPFrames = framesElapsed;
                break;
            case 8:
                questSplit.TenPercentRemainingHPFrames = framesElapsed;
                break;
            case 9:
                questSplit.ZeroPercentRemainingHPFrames = framesElapsed;
                break;
                // Handle additional cases if needed
        }
    }

    private static void PopulateQuestSplitField(QuestObjectiveSplit questSplit, int index, int framesElapsed)
    {
        switch (index)
        {
            case 0:
                questSplit.FortyPercentRemainingHPFrames = framesElapsed;
                break;
            case 1:
                questSplit.TwentyPercentRemainingHPFrames = framesElapsed;
                break;
            case 2:
                questSplit.ZeroPercentRemainingHPFrames = framesElapsed;
                break;
                // Handle additional cases if needed
        }
    }

    private void WeaponUsageChart_Loaded(object sender, RoutedEventArgs e)
    {
        this.weaponUsageChart = (CartesianChart)sender;

        if (!this.weaponUsageChart.Series.Any())
        {
            this.MainWindow.DataLoader.Model.WeaponUsageEarthStyle.Clear();
            this.MainWindow.DataLoader.Model.WeaponUsageHeavenStyle.Clear();
            this.MainWindow.DataLoader.Model.WeaponUsageStormStyle.Clear();
            this.MainWindow.DataLoader.Model.WeaponUsageExtremeStyle.Clear();

            // TODO: is this needed?
            this.weaponUsageChart.SyncContext = this.MainWindow.DataLoader.Model.WeaponUsageSync;

            this.SetWeaponUsageChart(this.weaponUsageChart);
        }
    }

    private TextBox? youtubeLinkTextBox;
    private DataGrid? mostRecentRunsDataGrid;
    private DataGrid? top20RunsDataGrid;
    private DataGrid? uneditedYouTubeLinkRunsDataGrid;
    private TextBlock? questLogGearStatsTextBlock;
    private TextBlock? compendiumTextBlock;
    private CartesianChart? graphChart;
    private TextBlock? statsTextTextBlock;
    private CartesianChart? personalBestChart;
    private CartesianChart? questPaceGraph;
    private PolarChart? hunterPerformanceChart;
    private StackPanel? compendiumInformationStackPanel;
    private string personalBestSelectedWeapon = string.Empty;
    private string personalBestSelectedType = string.Empty;
    private DataGrid? calendarDataGrid;
    private Grid? personalBestChartGrid;
    private Grid? weaponUsageChartGrid;
    private Grid? statsGraphsGrid;
    private TextBlock? personalBestDescriptionTextBlock;
    private TextBlock? top20RunsDescriptionTextBlock;
    private TextBlock? questPaceDescriptionTextBlock;
    private Grid? personalBestMainGrid;
    private Grid? top20MainGrid;
    private Grid? questPaceMainGrid;
    private Grid? questPaceGraphGrid;
    private Grid? weaponStatsMainGrid;
    private Grid? statsGraphsMainGrid;
    private Grid? statsTextMainGrid;
    private ListView? achievementsListView;
    private Grid? achievementsSelectedInfoGrid;
    private ListBox? challengesListBox;
    private TextBox? extraRunIDTextBox;
    private TextBlock? runIDComparisonTextBlock;

    private string top20RunsSelectedWeapon = string.Empty;
    private string questPaceWeaponSelected = string.Empty;

    private void UpdateYoutubeLink_ButtonClick(object sender, RoutedEventArgs e)
    {
        // Get the quest ID and new YouTube link from the textboxes
        var runID = long.Parse(this.RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        if (this.youtubeLinkTextBox == null)
        {
            return;
        }

        var youtubeLink = this.youtubeLinkTextBox.Text.Trim();
        if (DatabaseManager.UpdateYoutubeLink(sender, e, runID, youtubeLink))
        {
            var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
            {
                Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
                Title = Messages.InfoTitle,
                Content = string.Format(CultureInfo.InvariantCulture, "Updated run {0} with link https://youtube.com/watch?v={1}", runID, youtubeLink),
                Appearance = ControlAppearance.Success,
                Icon = new SymbolIcon(SymbolRegular.Video32),
                Timeout = this.SnackbarTimeOut,
            };
            snackbar.Show();
        }
        else
        {
            var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
            {
                Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
                Title = Messages.ErrorTitle,
                Content = string.Format(CultureInfo.InvariantCulture, "Could not update run {0} with link https://youtube.com/watch?v={1}. The link may have already been set to the same value, or the run ID and link input are invalid.", runID, youtubeLink),
                Appearance = ControlAppearance.Danger,
                Icon = new SymbolIcon(SymbolRegular.Video32),
                Timeout = this.SnackbarTimeOut,
            };
            snackbar.Show();
        }
    }

    private void YoutubeIconButton_Click(object sender, RoutedEventArgs e)
    {
        var runID = long.Parse(this.RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        var youtubeLink = DatabaseManager.GetYoutubeLinkForRunID(runID);
        if (youtubeLink != string.Empty)
        {
            var sInfo = new ProcessStartInfo(youtubeLink)
            {
                UseShellExecute = true,
            };
            Process.Start(sInfo);
        }
        else
        {
            MessageBox.Show("Run not found", Messages.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void YoutubeLinkTextBox_Loaded(object sender, RoutedEventArgs e) => this.youtubeLinkTextBox = (TextBox)sender;

    private void Top20Runs_DataGridLoaded(object sender, RoutedEventArgs e)
    {
        this.top20RunsDataGrid = (DataGrid)sender;
        this.MainWindow.DataLoader.Model.FastestRuns = DatabaseManager.GetFastestRuns(this, (uint)GetRunBuffs(MainWindow.DataLoader.Model.RunBuffsSearchOption));
        this.top20RunsDataGrid.ItemsSource = this.MainWindow.DataLoader.Model.FastestRuns;
        this.top20RunsDataGrid.Items.Refresh();
    }

    private void UneditedYouTubeLinkRuns_DataGridLoaded(object sender, RoutedEventArgs e)
    {
        this.uneditedYouTubeLinkRunsDataGrid = (DataGrid)sender;
    }

    private void YouTubeFindRuns_Click(object sender, RoutedEventArgs e)
    {
        if (this.uneditedYouTubeLinkRunsDataGrid == null)
        {
            return;
        }

        this.uneditedYouTubeLinkRunsDataGrid.ItemsSource = DatabaseManager.GetUneditedYouTubeLinkRuns();
        this.uneditedYouTubeLinkRunsDataGrid.Items.Refresh();
    }

    private string statsGraphsSelectedOption = string.Empty;

    private string statsTextSelectedOption = string.Empty;

    private DateTime datePickerDate = DateTime.UtcNow;

    // Declare flags to track event subscription
    private bool isConfigureButtonClickedSubscribed;

    private ISeries[]? Series { get; set; }

    // TODO to another class
    public static void SaveCSVFromListOfRecentRuns(List<RecentRuns> recentRuns, string filePath)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Objective Image,Quest Name,Run ID,Quest ID,Youtube ID,Final Time Display,Date,Actual Overlay Mode,Run Buffs,Party Size");

        foreach (var run in recentRuns)
        {
            var objectiveImage = run.ObjectiveImage.Replace(",", string.Empty);
            var questName = run.QuestName.Replace(",", string.Empty);
            var youtubeID = run.YouTubeID.Replace(",", string.Empty);
            var finalTimeDisplay = run.FinalTimeDisplay.Replace(",", string.Empty);
            var actualOverlayMode = run.ActualOverlayMode.Replace(",", string.Empty);
            var runBuffs = run.RunBuffs.ToString().Replace(",", string.Empty);

            var line = $"{objectiveImage},{questName},{run.RunID},{run.QuestID},{youtubeID},{finalTimeDisplay},{run.Date},{actualOverlayMode},{runBuffs},{run.PartySize}";
            sb.AppendLine(line);
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    private void WeaponListTop20RunsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.top20RunsDataGrid == null)
        {
            return;
        }

        if (sender is not ComboBox comboBox)
        {
            return;
        }

        var selectedItem = comboBox.SelectedItem;
        if (selectedItem == null)
        {
            return;
        }

        // You can now use the selectedItem variable to get the data or value of the selected option
        var selectedWeapon = selectedItem.ToString()?.Replace("System.Windows.Controls.ComboBoxItem: ", string.Empty);
        if (string.IsNullOrEmpty(selectedWeapon))
        {
            return;
        }

        this.top20RunsSelectedWeapon = selectedWeapon;
        this.MainWindow.DataLoader.Model.FastestRuns = DatabaseManager.GetFastestRuns(this, (uint)GetRunBuffs(MainWindow.DataLoader.Model.RunBuffsSearchOption), selectedWeapon);
        this.top20RunsDataGrid.ItemsSource = this.MainWindow.DataLoader.Model.FastestRuns;
        this.top20RunsDataGrid.Items.Refresh();

        if (this.top20RunsDescriptionTextBlock != null)
        {
            this.top20RunsDescriptionTextBlock.Text = $"Top 20 fastest solo runs of quest ID {this.QuestIDTextBox.Text} by category {this.OverlayModeComboBox.Text}";
        }
    }

    private void QuestLogGearStats_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBlock textBlock)
        {
            return;
        }

        var runID = long.Parse(this.RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        textBlock.Text = this.MainWindow.DataLoader.Model.GenerateGearStats(runID);
        this.questLogGearStatsTextBlock = textBlock;
    }

    private void QuestLogGearBtnSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.questLogGearStatsTextBlock == null)
        {
            return;
        }

        var textToSave = this.questLogGearStatsTextBlock.Text;
        textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        var fileName = "Set";
        var beginningFileName = "Run";
        var beginningText = this.RunIDTextBox.Text.Trim();
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.SaveTextFile(snackbar, textToSave, fileName, beginningFileName, beginningText);
    }

    private void QuestLogGearBtnCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.questLogGearStatsTextBlock == null)
        {
            return;
        }

        var previousBackground = this.questLogGearStatsTextBlock.Background;
        this.questLogGearStatsTextBlock.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.questLogGearStatsTextBlock, snackbar);
        this.questLogGearStatsTextBlock.Background = previousBackground;
    }

    private void Compendium_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBlock textBlock)
        {
            return;
        }

        textBlock.Text = this.MainWindow.DataLoader.Model.GenerateCompendium();
        this.compendiumTextBlock = textBlock;
    }

    private void CompendiumBtnSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.compendiumTextBlock == null)
        {
            return;
        }

        var textToSave = this.compendiumTextBlock.Text;
        textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.SaveTextFile(snackbar, textToSave, "Compendium");
    }

    private void CompendiumBtnCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.compendiumInformationStackPanel == null)
        {
            return;
        }

        var previousBackground = this.compendiumInformationStackPanel.Background;
        this.compendiumInformationStackPanel.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.compendiumInformationStackPanel, snackbar);
        this.compendiumInformationStackPanel.Background = previousBackground;
    }

    // TODO: put in file manager class?
    private void CalendarButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var data = this.MainWindow.DataLoader.Model.CalendarRuns;
            if (data == null)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save Calendar Runs as CSV",
            };
            var s = (Settings)Application.Current.TryFindResource("Settings");
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            var dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            saveFileDialog.FileName = string.Format(CultureInfo.InvariantCulture, "CalendarRuns-{0}", this.datePickerDate.ToString("yy/MM/dd", CultureInfo.InvariantCulture).Replace("/", "-"));
            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                SaveCSVFromListOfRecentRuns(data, filePath);
                Logger.Info(CultureInfo.InvariantCulture, "Saved text {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Could not save text file");
        }
    }

    public static void SaveCSVFromWeaponUsage(List<WeaponUsage> weaponUsageData, string filePath)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Weapon Type,Style,Run Count");

        foreach (var weaponUsageMapper in weaponUsageData)
        {
            var weaponType = weaponUsageMapper.WeaponType.Replace(",", string.Empty);
            var style = weaponUsageMapper.Style.Replace(",", string.Empty);
            var runCount = weaponUsageMapper.RunCount.ToString(CultureInfo.InvariantCulture).Replace(",", string.Empty);

            var line = $"{weaponType},{style},{runCount}";
            sb.AppendLine(line);
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    public static void SaveCSVFromListOfRecentRuns(ObservableCollection<RecentRuns> recentRuns, string filePath)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Objective Image,Quest Name,Run ID,Quest ID,Youtube ID,Final Time Display,Date,Actual Overlay Mode,Run Buffs,Party Size");

        foreach (var run in recentRuns)
        {
            var objectiveImage = run.ObjectiveImage.Replace(",", string.Empty);
            var questName = run.QuestName.Replace(",", string.Empty);
            var youtubeID = run.YouTubeID.Replace(",", string.Empty);
            var finalTimeDisplay = run.FinalTimeDisplay.Replace(",", string.Empty);
            var actualOverlayMode = run.ActualOverlayMode.Replace(",", string.Empty);
            var runBuffs = run.RunBuffs.ToString().Replace(",", string.Empty);

            var line = $"{objectiveImage},{questName},{run.RunID},{run.QuestID},{youtubeID},{finalTimeDisplay},{run.Date},{actualOverlayMode},{runBuffs},{run.PartySize}";
            sb.AppendLine(line);
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    public static void SaveCSVFromListOfFastestRuns(List<FastestRun> fastestRuns, string filePath)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Objective Image,Quest Name,Run ID,Quest ID,Youtube ID,Final Time Display,Run Buffs,Date");

        foreach (var run in fastestRuns)
        {
            var objectiveImage = run.ObjectiveImage.Replace(",", string.Empty);
            var questName = run.QuestName.Replace(",", string.Empty);
            var youtubeID = run.YouTubeID.Replace(",", string.Empty);
            var finalTimeDisplay = run.FinalTimeDisplay.Replace(",", string.Empty);
            var runBuffs = run.RunBuffs.ToString().Replace(",", string.Empty);

            var line = $"{objectiveImage},{questName},{run.RunID},{run.QuestID},{youtubeID},{finalTimeDisplay},{runBuffs},{run.Date}";
            sb.AppendLine(line);
        }

        File.WriteAllText(filePath, sb.ToString());
    }

    private void CalendarButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.calendarDataGrid == null)
        {
            return;
        }

        var previousBackground = this.calendarDataGrid.Background;
        this.calendarDataGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.calendarDataGrid, snackbar);
        this.calendarDataGrid.Background = previousBackground;
    }

    private void PersonalBestButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.personalBestChart == null || this.personalBestChartGrid == null || this.personalBestMainGrid == null)
        {
            return;
        }

        var fileName = $"PersonalBest-Quest_{this.QuestIDTextBox.Text}-{this.OverlayModeComboBox.Text}-{this.personalBestSelectedType}-{this.personalBestSelectedWeapon}".Trim().Replace(" ", "_");
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.SaveElementAsImageFile(this.personalBestMainGrid, fileName, snackbar, false);
    }

    private void PersonalBestButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.personalBestChartGrid == null || this.personalBestMainGrid == null)
        {
            return;
        }

        var previousBackground = this.personalBestMainGrid.Background;
        this.personalBestMainGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.personalBestMainGrid, snackbar);
        this.personalBestMainGrid.Background = previousBackground;
    }

    private void Top20ButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var data = this.MainWindow.DataLoader.Model.FastestRuns;
            if (data == null)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save Fastest Runs as CSV",
            };
            var s = (Settings)Application.Current.TryFindResource("Settings");
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            var dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            saveFileDialog.FileName = string.Format(CultureInfo.InvariantCulture, "FastestRuns-Quest_{0}-{1}-{2}-{3}", this.QuestIDTextBox.Text, this.OverlayModeComboBox.Text, this.top20RunsSelectedWeapon, DateTime.UtcNow.ToString("yy/MM/dd", CultureInfo.InvariantCulture).Replace("/", "-"));
            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                SaveCSVFromListOfFastestRuns(data, filePath);
                Logger.Info(CultureInfo.InvariantCulture, "Saved text {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Could not save text file");
        }
    }

    private void Top20ButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.top20RunsDataGrid == null || this.top20MainGrid == null)
        {
            return;
        }

        var previousBackground = this.top20MainGrid.Background;
        this.top20MainGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.top20MainGrid, snackbar);
        this.top20MainGrid.Background = previousBackground;
    }

    private void WeaponStatsButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.weaponUsageChartGrid == null || this.weaponUsageChart == null || this.weaponUsageData == null || this.weaponStatsMainGrid == null)
        {
            return;
        }

        try
        {
            var data = this.MainWindow.DataLoader.Model.CalendarRuns;
            if (data == null)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save Weapon Stats as CSV",
            };
            var s = (Settings)Application.Current.TryFindResource("Settings");
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            var dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            saveFileDialog.FileName = string.Format(CultureInfo.InvariantCulture, "WeaponUsage-{0}", dateTime);
            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                SaveCSVFromWeaponUsage(this.weaponUsageData, filePath);
                Logger.Info(CultureInfo.InvariantCulture, "Saved text {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Could not save text file");
        }
    }

    private void WeaponStatsButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.weaponUsageChartGrid == null || this.weaponStatsMainGrid == null)
        {
            return;
        }

        var previousBackground = this.weaponStatsMainGrid.Background;
        this.weaponStatsMainGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.weaponStatsMainGrid, snackbar);
        this.weaponStatsMainGrid.Background = previousBackground;
    }

    private void QuestPaceButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.questPaceMainGrid == null)
        {
            return;
        }

        if (this.questPaceGraphGrid == null)
        {
            return;
        }

        var fileName = $"QuestPace-{this.QuestIDTextBox.Text.Trim()}-{this.questPaceWeaponSelected}-{this.OverlayModeComboBox.Text.Trim()}";

        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.SaveElementAsImageFile(this.questPaceMainGrid, fileName, snackbar, false);
    }

    private void QuestPaceButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.questPaceMainGrid == null)
        {
            return;
        }

        if (this.questPaceGraphGrid == null)
        {
            return;
        }

        var previousBackground = this.questPaceMainGrid.Background;
        this.questPaceMainGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.questPaceMainGrid, snackbar);
        this.questPaceMainGrid.Background = previousBackground;
    }

    private void MostRecentButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var data = this.MainWindow.DataLoader.Model.RecentRuns;
            if (data == null)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save Recent Runs as CSV",
            };
            var s = (Settings)Application.Current.TryFindResource("Settings");
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(s.DatabaseFilePath);
            var dateTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            saveFileDialog.FileName = string.Format(CultureInfo.InvariantCulture, "RecentRuns-{0}", dateTime);
            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                SaveCSVFromListOfRecentRuns(data, filePath);
                Logger.Info(CultureInfo.InvariantCulture, "Saved text {0}", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Could not save text file");
        }
    }

    private void MostRecentButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.mostRecentRunsDataGrid == null)
        {
            return;
        }

        var previousBackground = this.mostRecentRunsDataGrid.Background;
        this.mostRecentRunsDataGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.mostRecentRunsDataGrid, snackbar);
        this.mostRecentRunsDataGrid.Background = previousBackground;
    }

    private void StatsGraphsButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.statsGraphsGrid == null || this.statsGraphsMainGrid == null)
        {
            return;
        }

        var fileName = $"StatsGraphs-{this.statsGraphsSelectedOption}";
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.SaveElementAsImageFile(this.statsGraphsMainGrid, fileName, snackbar, false);
    }

    private void StatsGraphsButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.statsGraphsGrid == null || this.statsGraphsMainGrid == null)
        {
            return;
        }

        var previousBackground = this.statsGraphsMainGrid.Background;
        this.statsGraphsMainGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.statsGraphsMainGrid, snackbar);
        this.statsGraphsMainGrid.Background = previousBackground;
    }

    private void StatsTextButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.statsTextTextBlock == null || this.statsTextMainGrid == null)
        {
            return;
        }

        var textToSave = this.statsTextTextBlock.Text;
        textToSave = string.Format(CultureInfo.InvariantCulture, "```text\n{0}\n```", textToSave);
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.SaveTextFile(snackbar, textToSave, $"StatsText-Run_{this.RunIDTextBox.Text}-{this.statsTextSelectedOption}");
    }

    private void StatsTextButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.statsTextTextBlock == null || this.statsTextMainGrid == null)
        {
            return;
        }

        var previousBackground = this.statsTextTextBlock.Background;
        this.statsTextTextBlock.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.statsTextTextBlock, snackbar);
        this.statsTextTextBlock.Background = previousBackground;
    }

    private void PersonalBestsOverviewButtonSaveFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.DiscordEmbedWeaponPersonalBest == null || this.QuestIDTextBox == null)
        {
            return;
        }

        var fileName = $"PersonalBestsOverview-Quest_{this.QuestIDTextBox.Text}-{DateTime.UtcNow.ToString("yy/MM/dd", CultureInfo.InvariantCulture).Replace("/", "-")}";
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.SaveElementAsImageFile(this.DiscordEmbedWeaponPersonalBest, fileName, snackbar, false);
    }

    private void PersonalBestsOverviewButtonCopyFile_Click(object sender, RoutedEventArgs e)
    {
        if (this.DiscordEmbedWeaponPersonalBest == null)
        {
            return;
        }

        var previousBackground = this.DiscordEmbedWeaponPersonalBest.Background;
        this.DiscordEmbedWeaponPersonalBest.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x2E));
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        FileService.CopyUIElementToClipboard(this.DiscordEmbedWeaponPersonalBest, snackbar);
        this.DiscordEmbedWeaponPersonalBest.Background = previousBackground;
    }

    private Axis[]? XAxes { get; set; }

    private Axis[]? YAxes { get; set; }

    private ISeries[]? PersonalBestSeries { get; set; }

    private Axis[]? PersonalBestXAxes { get; set; }

    private Axis[]? PersonalBestYAxes { get; set; }

    public void SetPlayerHealthStamina(Dictionary<int, int> hp, Dictionary<int, int> stamina)
    {
        if (this.graphChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> healthCollection = new();
        ObservableCollection<ObservablePoint> staminaCollection = new();

        var newHP = GetElapsedTime(hp);
        var newStamina = GetElapsedTime(stamina);

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
            $"Health: {(long)chartPoint.PrimaryValue}",
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ffa6e3a1"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ffa6e3a1", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ffa6e3a1", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = staminaCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) =>
            $"Stamina: {(long)chartPoint.PrimaryValue}",
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff9e2af"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff9e2af", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff9e2af", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.graphChart.Series = series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
    }

    private void SetColumnSeriesForDictionaryIntInt(Dictionary<int, int> data)
    {
        this.Series = new ISeries[data.Count];
        var i = 0;
        foreach (var entry in data)
        {
            this.Series[i] = new ColumnSeries<double>
            {
                Name = entry.Key.ToString(CultureInfo.InvariantCulture),
                Values = new double[] { entry.Value },
            };

            i++;
        }

        this.XAxes = new Axis[]
        {
            new Axis
            {
                Labels = data.Keys.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray(),
                LabelsRotation = 0,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };
    }

    private void SetColumnSeriesForDictionaryStringInt(Dictionary<string, int> data)
    {
        this.Series = new ISeries[data.Count];
        var i = 0;
        foreach (var entry in data)
        {
            this.Series[i] = new ColumnSeries<double>
            {
                Name = entry.Key.ToString(CultureInfo.InvariantCulture),
                Values = new double[] { entry.Value },
            };

            i++;
        }

        this.XAxes = new Axis[]
        {
            new Axis
            {
                Labels = data.Keys.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray(),
                LabelsRotation = 0,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };
    }

    private void SetColumnSeriesForDictionaryDateInt(Dictionary<DateTime, int> data)
    {
        this.Series = new ISeries[data.Count];
        var i = 0;
        foreach (var entry in data)
        {
            this.Series[i] = new ColumnSeries<double>
            {
                Name = entry.Key.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Values = new double[] { entry.Value },
            };

            i++;
        }

        this.XAxes = new Axis[]
        {
    new Axis
    {
        Labels = data.Keys.Select(x => x.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).ToArray(),
        LabelsRotation = 0,
        SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
        TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
        NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
        LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
    },
        };
    }

    private static Dictionary<int, int> GetElapsedTime(Dictionary<int, int> timeAttackDict)
    {
        Dictionary<int, int> elapsedTimeDict = new();
        if (timeAttackDict == null || !timeAttackDict.Any())
        {
            return elapsedTimeDict;
        }

        var initialTime = timeAttackDict.First().Key;
        foreach (var entry in timeAttackDict)
        {
            elapsedTimeDict[initialTime - entry.Key] = entry.Value;
        }

        return elapsedTimeDict;
    }

    private static Dictionary<int, double> GetElapsedTimeForDictionaryIntDouble(Dictionary<int, double> timeAttackDict)
    {
        Dictionary<int, double> elapsedTimeDict = new();
        if (timeAttackDict == null || !timeAttackDict.Any())
        {
            return elapsedTimeDict;
        }

        var initialTime = timeAttackDict.First().Key;
        foreach (var entry in timeAttackDict)
        {
            elapsedTimeDict[initialTime - entry.Key] = entry.Value;
        }

        return elapsedTimeDict;
    }

    private void SetLineSeriesForDictionaryIntInt(Dictionary<int, int> data, Dictionary<int, int>? extraData)
    {
        if (this.graphChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> collection = new();

        var newData = GetElapsedTime(data);

        foreach (var entry in newData)
        {
            collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        if (extraData != null)
        {
            ObservableCollection<ObservablePoint> collection2 = new();

            var newData2 = GetElapsedTime(extraData);

            foreach (var entry in newData2)
            {
                collection2.Add(new ObservablePoint(entry.Key, entry.Value));
            }

            series.Add(new LineSeries<ObservablePoint>
            {
                Values = collection2,
                LineSmoothness = .5,
                GeometrySize = 0,
                Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa"))) { StrokeThickness = 2 },
                Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            });

            if (this.runIDComparisonTextBlock != null && this.extraRunIDTextBox != null)
            {
                var runComparisonPercentage = CalculateBetterLinePercentage(newData, newData2);
                var betterRun = runComparisonPercentage >= 0.0 ? RunIDTextBox.Text : extraRunIDTextBox.Text;
                this.runIDComparisonTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Run {0} is higher by around {1:0.##}%", betterRun, Math.Abs(runComparisonPercentage));
            }
        }

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.graphChart.Series = series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
    }

    private double CalculateBetterLinePercentage(Dictionary<int, int> line1, Dictionary<int, int> line2)
    {
        if (line1.Count == 0 || line2.Count == 0)
            return 0.0;
        // Find the cutoff time where the first series ends.
        int cutoffTime = Math.Min(line1.Keys.Max(), line2.Keys.Max());

        // Calculate the area under each line series up to the cutoff time.
        double area1 = CalculateAreaUnderLine(line1, cutoffTime);
        double area2 = CalculateAreaUnderLine(line2, cutoffTime);

        // Determine which series has the larger area and calculate the percentage difference.
        if (area1 == area2)
        {
            return 0; // Both series are equal.
        }
        else
        {
            double largerArea = Math.Max(area1, area2);
            double smallerArea = Math.Min(area1, area2);
            double percentageDifference = ((largerArea - smallerArea) / smallerArea) * 100;
            return area1 > area2 ? percentageDifference : -percentageDifference;
        }
    }

    private double CalculateBetterLinePercentage(Dictionary<int, double> line1, Dictionary<int, double> line2)
    {
        if (line1.Count == 0 || line2.Count == 0)
            return 0.0;
        // Find the cutoff time where the first series ends.
        int cutoffTime = Math.Min(line1.Keys.Max(), line2.Keys.Max());

        // Calculate the area under each line series up to the cutoff time.
        double area1 = CalculateAreaUnderLine(line1, cutoffTime);
        double area2 = CalculateAreaUnderLine(line2, cutoffTime);

        // Determine which series has the larger area and calculate the percentage difference.
        if (area1 == area2)
        {
            return 0; // Both series are equal.
        }
        else
        {
            double largerArea = Math.Max(area1, area2);
            double smallerArea = Math.Min(area1, area2);
            double percentageDifference = ((largerArea - smallerArea) / smallerArea) * 100;
            return area1 > area2 ? percentageDifference : -percentageDifference;
        }
    }

    private double CalculateAreaUnderLine(Dictionary<int, int> line, int cutoffTime)
    {
        // Assuming the line is sorted by time.
        double area = 0;
        int previousTime = 0;
        int previousScore = 0;

        foreach (var point in line.OrderBy(p => p.Key))
        {
            if (point.Key > cutoffTime)
                break;

            if (previousTime != 0)
            {
                // Calculate the area of the trapezoid.
                double base1 = point.Value;
                double base2 = previousScore;
                double height = point.Key - previousTime;
                area += (base1 + base2) * height / 2;
            }

            previousTime = point.Key;
            previousScore = point.Value;
        }

        return area;
    }

    private double CalculateAreaUnderLine(Dictionary<int, double> line, int cutoffTime)
    {
        // Assuming the line is sorted by time.
        double area = 0;
        int previousTime = 0;
        double previousScore = 0.0;

        foreach (var point in line.OrderBy(p => p.Key))
        {
            if (point.Key > cutoffTime)
                break;

            if (previousTime != 0)
            {
                // Calculate the area of the trapezoid.
                double base1 = point.Value;
                double base2 = previousScore;
                double height = point.Key - previousTime;
                area += (base1 + base2) * height / 2;
            }

            previousTime = point.Key;
            previousScore = point.Value;
        }

        return area;
    }

    private void SetLineSeriesForDictionaryIntDouble(Dictionary<int, double> data, Dictionary<int, double>? extraData)
    {
        if (this.graphChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> collection = new();

        var newData = GetElapsedTimeForDictionaryIntDouble(data);

        foreach (var entry in newData)
        {
            collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        if (extraData != null)
        {
            ObservableCollection<ObservablePoint> collection2 = new();

            var newData2 = GetElapsedTimeForDictionaryIntDouble(extraData);

            foreach (var entry in newData2)
            {
                collection2.Add(new ObservablePoint(entry.Key, entry.Value));
            }

            series.Add(new LineSeries<ObservablePoint>
            {
                Values = collection2,
                LineSmoothness = .5,
                GeometrySize = 0,
                Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa"))) { StrokeThickness = 2 },
                Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            });

            if (this.runIDComparisonTextBlock != null && this.extraRunIDTextBox != null)
            {
                var runComparisonPercentage = CalculateBetterLinePercentage(newData, newData2);
                var betterRun = runComparisonPercentage >= 0.0 ? RunIDTextBox.Text : extraRunIDTextBox.Text;
                this.runIDComparisonTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Run {0} is higher by around {1:0.##}%", betterRun, Math.Abs(runComparisonPercentage));
            }
        }

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.graphChart.Series = series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
    }

    private void SetStepLineSeriesForPersonalBestByAttempts(Dictionary<long, long> data)
    {
        if (this.personalBestChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> collection = new();

        foreach (var entry in data)
        {
            collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new StepLineSeries<ObservablePoint>
        {
            Values = collection,
            TooltipLabelFormatter = (chartPoint) =>
            $"Attempt {chartPoint.SecondaryValue}: {TimeService.GetMinutesSecondsMillisecondsFromFrames((long)chartPoint.PrimaryValue)}",
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.PersonalBestXAxes = new Axis[]
        {
            new Axis
            {
                MinStep = 1,
                TextSize = 12,
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.PersonalBestYAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                NameTextSize = 12,
                MinStep = 1,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.personalBestChart.Series = series;
        this.personalBestChart.XAxes = this.PersonalBestXAxes;
        this.personalBestChart.YAxes = this.PersonalBestYAxes;
    }

    private void SetStepLineSeriesForDictionaryIntInt(Dictionary<int, int> data, Dictionary<int, int>? extraData)
    {
        if (this.graphChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> collection = new();

        var newData = GetElapsedTime(data);

        foreach (var entry in newData)
        {
            collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new StepLineSeries<ObservablePoint>
        {
            Values = collection,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        if (extraData != null)
        {
            ObservableCollection<ObservablePoint> collection2 = new();

            var newData2 = GetElapsedTime(extraData);

            foreach (var entry in newData2)
            {
                collection2.Add(new ObservablePoint(entry.Key, entry.Value));
            }

            series.Add(new StepLineSeries<ObservablePoint>
            {
                Values = collection2,
                GeometrySize = 0,
                Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa"))) { StrokeThickness = 2 },
                Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            });

            if (this.runIDComparisonTextBlock != null && this.extraRunIDTextBox != null)
            {
                var runComparisonPercentage = CalculateBetterLinePercentage(newData, newData2);
                var betterRun = runComparisonPercentage >= 0.0 ? RunIDTextBox.Text : extraRunIDTextBox.Text;
                this.runIDComparisonTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Run {0} is higher by around {1:0.##}%", betterRun, Math.Abs(runComparisonPercentage));
            }
        }

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.graphChart.Series = series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
    }

    private void SetStepLineSeriesForPersonalBestByDate(Dictionary<DateTime, long> data)
    {
        if (this.personalBestChart == null)
        {
            return;
        }

        List<ISeries> series = new();

        ObservableCollection<DateTimePoint> collection = new();

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
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            TooltipLabelFormatter = (chartPoint) =>
            $"{new DateTime((long)chartPoint.SecondaryValue):yy-MM-dd}: {TimeService.GetMinutesSecondsMillisecondsFromFrames((long)chartPoint.PrimaryValue)}",
        });

        this.PersonalBestXAxes = new Axis[]
        {
            new Axis
            {
                Labeler = value => new DateTime((long)value).ToString("yy-MM-dd", CultureInfo.InvariantCulture),
                LabelsRotation = 80,
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),

                // when using a date time type, let the library know your unit
                UnitWidth = TimeSpan.FromDays(1).Ticks,

                // if the difference between our points is in hours then we would do:
                // UnitWidth = TimeSpan.FromHours(1).Ticks,

                // since all the months and years have a different number of days
                // we can use the average, it would not cause any visible error in the user interface
                // Months: TimeSpan.FromDays(30.4375).Ticks
                // Years: TimeSpan.FromDays(365.25).Ticks

                // The MinStep property forces the separator to be greater than 1 day.
                MinStep = TimeSpan.FromDays(1).Ticks,
            },
        };

        this.PersonalBestYAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                MinStep = 1,
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.personalBestChart.Series = series;
        this.personalBestChart.XAxes = this.PersonalBestXAxes;
        this.personalBestChart.YAxes = this.PersonalBestYAxes;
    }

    private void SetHitsTakenBlocked(Dictionary<int, Dictionary<int, int>> data, Dictionary<int, Dictionary<int, int>>? extraData)
    {
        if (this.graphChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> collection = new();

        var hitsTakenBlocked = CalculateHitsTakenBlocked(data);

        var newData = GetElapsedTime(hitsTakenBlocked);

        foreach (var entry in newData)
        {
            collection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        if (extraData != null)
        {
            ObservableCollection<ObservablePoint> collection2 = new();

            var hitsTakenBlocked2 = CalculateHitsTakenBlocked(extraData);

            var newData2 = GetElapsedTime(hitsTakenBlocked2);

            foreach (var entry in newData2)
            {
                collection2.Add(new ObservablePoint(entry.Key, entry.Value));
            }

            series.Add(new LineSeries<ObservablePoint>
            {
                Values = collection2,
                LineSmoothness = .5,
                GeometrySize = 0,
                Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa"))) { StrokeThickness = 2 },
                Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff89b4fa", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
            });

            if (this.runIDComparisonTextBlock != null && this.extraRunIDTextBox != null)
            {
                var runComparisonPercentage = CalculateBetterLinePercentage(newData, newData2);
                var betterRun = runComparisonPercentage >= 0.0 ? RunIDTextBox.Text : extraRunIDTextBox.Text;
                this.runIDComparisonTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Run {0} is higher by around {1:0.##}%", betterRun, Math.Abs(runComparisonPercentage));
            }
        }

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.graphChart.Series = series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
    }

    public void SetMonsterAttackMultiplier(Dictionary<int, double> attack)
    {
        if (this.graphChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> attackCollection = new();

        var newAttack = GetElapsedTimeForDictionaryIntDouble(attack);

        foreach (var entry in newAttack)
        {
            attackCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new StepLineSeries<ObservablePoint>
        {
            Values = attackCollection,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#f38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#f38ba8", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#f38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.graphChart.Series = series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
    }

    public void SetMonsterDefenseRate(Dictionary<int, double> defense)
    {
        if (this.graphChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> defenseCollection = new();

        var newDefense = GetElapsedTimeForDictionaryIntDouble(defense);

        foreach (var entry in newDefense)
        {
            defenseCollection.Add(new ObservablePoint(entry.Key, entry.Value));
        }

        series.Add(new StepLineSeries<ObservablePoint>
        {
            Values = defenseCollection,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#74c7ec"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#74c7ec", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#74c7ec", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.graphChart.Series = series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
    }

    private void SetMonsterStatusAilmentsThresholds(Dictionary<int, int> poison, Dictionary<int, int> sleep, Dictionary<int, int> para, Dictionary<int, int> blast, Dictionary<int, int> stun)
    {
        if (this.graphChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> poisonCollection = new();
        ObservableCollection<ObservablePoint> sleepCollection = new();
        ObservableCollection<ObservablePoint> paraCollection = new();
        ObservableCollection<ObservablePoint> blastCollection = new();
        ObservableCollection<ObservablePoint> stunCollection = new();

        var newPoison = GetElapsedTime(poison);
        var newSleep = GetElapsedTime(sleep);
        var newPara = GetElapsedTime(para);
        var newBlast = GetElapsedTime(blast);
        var newStun = GetElapsedTime(stun);

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
            $"Poison: {(long)chartPoint.PrimaryValue}",
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#cba6f7"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#cba6f7", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#cba6f7", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = sleepCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) => $"Sleep: {(long)chartPoint.PrimaryValue}",
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#74c7ec"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#74c7ec", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#74c7ec", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = paraCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) => $"Paralysis: {(long)chartPoint.PrimaryValue}",
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#f9e2af"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#f9e2af", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#f9e2af", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = blastCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) => $"Blast: {(long)chartPoint.PrimaryValue}",
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6e3a1"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6e3a1", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6e3a1", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = stunCollection,
            LineSmoothness = .5,
            GeometrySize = 0,
            TooltipLabelFormatter = (chartPoint) => $"Stun: {(long)chartPoint.PrimaryValue}",
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#7f849c"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#7f849c", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#7f849c", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.graphChart.Series = series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
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
                TooltipLabelFormatter = value => questDuration.Key.ToString(CultureInfo.InvariantCulture) + " " + TimeSpan.FromSeconds(value.PrimaryValue / double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture),
                DataLabelsFormatter = value => TimeSpan.FromSeconds(value.PrimaryValue / double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture),
            });
        }

        this.Series = series.ToArray();
        this.YAxes = new Axis[]
        {
            new Axis
            {
                Labeler = (value) => TimeSpan.FromSeconds(value / double.Parse(Numbers.FramesPerSecond.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)).ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture),
                LabelsRotation = 0,
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
            },
        };
    }

    // TODO: gamepad test
    private static Dictionary<string, int> GetMostCommonInputs(long runID)
    {
        var keystrokesDictionary = DatabaseManager.GetKeystrokesDictionary(runID);
        var mouseInputDictionary = DatabaseManager.GetMouseInputDictionary(runID);
        var gamepadInputDictionary = DatabaseManager.GetGamepadInputDictionary(runID);
        var combinedDictionary = keystrokesDictionary
            .Union(mouseInputDictionary)
            .Union(gamepadInputDictionary)
            .GroupBy(kvp => kvp.Value)
            .ToDictionary(g => g.Key, g => g.Count());

        return combinedDictionary
            .OrderByDescending(kvp => kvp.Value)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private static Dictionary<int, int> CalculateHitsTakenBlocked(Dictionary<int, Dictionary<int, int>> hitsTakenBlocked)
    {
        Dictionary<int, int> dictionary = new();

        var i = 1;
        foreach (var entry in hitsTakenBlocked)
        {
            var time = int.Parse(entry.Key.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            var count = i;
            dictionary.Add(time, count);
            i++;
        }

        return dictionary;
    }

    private static Dictionary<int, int> CalculateMonsterHP(Dictionary<int, Dictionary<int, int>> monsterHP)
    {
        Dictionary<int, int> dictionary = new();

        var i = 1;
        foreach (var entry in monsterHP)
        {
            var time = int.Parse(entry.Key.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

            // get the value of the inner dictionary
            var hp = entry.Value.Values.First();
            dictionary.Add(time, hp);
            i++;
        }

        return dictionary;
    }

    private static Dictionary<int, double> CalculateMonsterMultiplier(Dictionary<int, Dictionary<int, double>> monsterDictionary)
    {
        Dictionary<int, double> dictionary = new();

        var i = 1;
        foreach (var entry in monsterDictionary)
        {
            var time = int.Parse(entry.Key.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

            // get the value of the inner dictionary
            var mult = entry.Value.Values.First();
            dictionary.Add(time, mult);
            i++;
        }

        return dictionary;
    }

    private static Dictionary<int, int> CalculateMonsterStatusAilmentThresholds(Dictionary<int, Dictionary<int, int>> monsterDictionary)
    {
        Dictionary<int, int> dictionary = new();

        var i = 1;
        foreach (var entry in monsterDictionary)
        {
            var time = int.Parse(entry.Key.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

            // get the value of the inner dictionary
            var threshold = entry.Value.Values.First();
            dictionary.Add(time, threshold);
            i++;
        }

        return dictionary;
    }

    private void SetMonsterHP(Dictionary<int, int> monster1, Dictionary<int, int> monster2, Dictionary<int, int> monster3, Dictionary<int, int> monster4)
    {
        if (this.graphChart == null)
        {
            return;
        }

        List<ISeries> series = new();
        ObservableCollection<ObservablePoint> monster1Collection = new();
        ObservableCollection<ObservablePoint> monster2Collection = new();
        ObservableCollection<ObservablePoint> monster3Collection = new();
        ObservableCollection<ObservablePoint> monster4Collection = new();

        var newMonster1 = GetElapsedTime(monster1);
        var newMonster2 = GetElapsedTime(monster2);
        var newMonster3 = GetElapsedTime(monster3);
        var newMonster4 = GetElapsedTime(monster4);

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
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = monster2Collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff9e2af"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff9e2af", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff9e2af", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = monster3Collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff94e2d5"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff94e2d5", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ff94e2d5", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = monster4Collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ffcba6f7"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ffcba6f7", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#ffcba6f7", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        this.XAxes = new Axis[]
        {
            new Axis
            {
                TextSize = 12,
                Labeler = (value) => TimeService.GetMinutesSecondsFromFrames(value),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.YAxes = new Axis[]
        {
            new Axis
            {
                NameTextSize = 12,
                TextSize = 12,
                NamePadding = new LiveChartsCore.Drawing.Padding(0),
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        this.graphChart.Series = series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
    }

    private static string GetHunterPerformanceValueType(double value) => value switch
    {
        0 => "True Raw",
        1 => "DPH",
        2 => "DPS",
        3 => "Hits/s",
        4 => "Blocked Hits/s",
        5 => "APM",
        _ => "None",
    };

    private void SetPolarLineSeriesForHunterPerformance(PerformanceCompendium performanceCompendium)
    {
        if (this.hunterPerformanceChart == null)
        {
            return;
        }

        List<ISeries> series = new();

        ObservableCollection<double> performanceCollection = new()
        {
            performanceCompendium.HighestTrueRaw != 0 ? performanceCompendium.TrueRawMedian / performanceCompendium.HighestTrueRaw : 0,
            performanceCompendium.HighestSingleHitDamage != 0 ? performanceCompendium.SingleHitDamageMedian / performanceCompendium.HighestSingleHitDamage : 0,
            performanceCompendium.HighestDPS != 0 ? performanceCompendium.DPSMedian / performanceCompendium.HighestDPS : 0,
            performanceCompendium.HighestHitsPerSecond != 0 ? performanceCompendium.HitsPerSecondMedian / performanceCompendium.HighestHitsPerSecond : 0,
            performanceCompendium.HighestHitsTakenBlockedPerSecond != 0 ? performanceCompendium.HitsTakenBlockedPerSecondMedian / performanceCompendium.HighestHitsTakenBlockedPerSecond : 0,
            performanceCompendium.HighestActionsPerMinute != 0 ? performanceCompendium.ActionsPerMinuteMedian / performanceCompendium.HighestActionsPerMinute : 0,
        };

        series.Add(new PolarLineSeries<double>
        {
            Values = performanceCollection,
            LineSmoothness = 0,
            GeometrySize = 0,
            IsClosed = true,
            GeometryFill = null,
            GeometryStroke = null,
            TooltipLabelFormatter = value => string.Format(CultureInfo.InvariantCulture, "{0}: {1:0.##}%", GetHunterPerformanceValueType(value.SecondaryValue), value.PrimaryValue * 100),
            Stroke = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8"))) { StrokeThickness = 2 },
            Fill = new LinearGradientPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "7f")), new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#fff38ba8", "00")), new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        });

        var radiusAxes = new PolarAxis[]
        {
            new PolarAxis
            {
                // formats the value as a number with 2 decimals.
                Labeler = value => string.Format(CultureInfo.InvariantCulture, "{0:0}%", value * 100),
                LabelsBackground = new LiveChartsCore.Drawing.LvcColor(0x1e, 0x1e, 0x2e, 0xa8),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#cdd6f4a8"))),
            },
        };

        var angleAxes = new PolarAxis[]
        {
            new PolarAxis
            {
                Labels = new[] { "TRaw", "DPH", "DPS", "Hits/s", "Blocks/s", "APM" },
                LabelsBackground = new LiveChartsCore.Drawing.LvcColor(0x1e, 0x1e, 0x2e, 0xa8),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#cdd6f4a8"))),
                MinStep = 1,
                ForceStepToMin = true,
            },
        };

        this.hunterPerformanceChart.AngleAxes = angleAxes;
        this.hunterPerformanceChart.RadiusAxes = radiusAxes;
        this.hunterPerformanceChart.Series = series;
    }

    private string? statsGraphsComboBoxOption = string.Empty;

    private void GraphsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox)sender;

        var selectedItem = (ComboBoxItem)comboBox.SelectedItem;

        if (selectedItem == null)
        {
            return;
        }

        var selectedOption = selectedItem.Content.ToString();
        statsGraphsComboBoxOption = selectedOption;

        if (this.graphChart == null || selectedOption == null || string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        UpdateStatsGraphs(selectedOption);
    }

    private void GraphsChart_Loaded(object sender, RoutedEventArgs e) => this.graphChart = (CartesianChart)sender;

    private void StatsTextTextBlock_Loaded(object sender, RoutedEventArgs e) => this.statsTextTextBlock = (TextBlock)sender;

    private static Dictionary<int, List<Dictionary<int, int>>> GetElapsedTimeForInventories(Dictionary<int, List<Dictionary<int, int>>> dictionary)
    {
        Dictionary<int, List<Dictionary<int, int>>> elapsedTimeDict = new();
        if (dictionary == null || !dictionary.Any())
        {
            return elapsedTimeDict;
        }

        var initialTime = dictionary.First().Key;
        foreach (var entry in dictionary)
        {
            elapsedTimeDict[initialTime - entry.Key] = entry.Value;
        }

        return elapsedTimeDict;
    }

    private static Dictionary<int, int> GetElapsedTimeForDictionaryIntInt(Dictionary<int, int> dictionary)
    {
        Dictionary<int, int> elapsedTimeDict = new();

        if (dictionary == null || !dictionary.Any())
        {
            return elapsedTimeDict;
        }

        var initialTime = dictionary.First().Key;
        foreach (var entry in dictionary)
        {
            elapsedTimeDict[initialTime - entry.Key] = entry.Value;
        }

        return elapsedTimeDict;
    }

    private string FormatInventory(Dictionary<int, List<Dictionary<int, int>>> inventory)
    {
        inventory = GetElapsedTimeForInventories(inventory);

        var sb = new StringBuilder();

        foreach (var entry in inventory)
        {
            var time = entry.Key;
            var timeString = TimeSpan.FromSeconds(time / (double)Numbers.FramesPerSecond).ToString(TimeFormats.MinutesSecondsMilliseconds, CultureInfo.InvariantCulture);
            var items = entry.Value;
            var count = 0;
            sb.AppendLine(timeString + " ");
            foreach (var item in items)
            {
                foreach (var itemData in item)
                {
                    if (itemData.Value > 0)
                    {
                        var itemName = GetItemName(itemData.Key);
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

        var sb = new StringBuilder();

        foreach (var entry in areas)
        {
            var time = entry.Key;
            var timeString = TimeSpan.FromSeconds(time / (double)Numbers.FramesPerSecond).ToString(TimeFormats.MinutesSecondsMilliseconds, CultureInfo.InvariantCulture);
            var area = entry.Value;
            sb.AppendLine(timeString + " ");

            Location.IDName.TryGetValue(area, out var itemName);
            sb.Append(itemName);
            sb.AppendLine();
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static string GetItemName(int itemID)
    {
        // implement code to get item name based on itemID
        Item.IDName.TryGetValue(itemID, out var value);
        if (value == null)
        {
            return string.Empty;
        }

        return value;
    }

    private void StatsTextComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox)sender;

        var selectedItem = (ComboBoxItem)comboBox.SelectedItem;

        if (selectedItem == null || this.statsTextTextBlock == null)
        {
            return;
        }

        var selectedOption = selectedItem.Content.ToString();

        if (this.statsTextTextBlock == null || selectedOption == null || string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        this.statsTextTextBlock.Text = string.Empty;

        var runID = long.Parse(this.RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);

        switch (selectedOption)
        {
            case "Inventory":
                this.statsTextTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Inventory\n\n{0}", this.FormatInventory(DatabaseManager.GetPlayerInventoryDictionary(runID)));
                break;
            case "Ammo":
                this.statsTextTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Ammo\n\n{0}", this.FormatInventory(DatabaseManager.GetAmmoDictionary(runID)));
                break;
            case "Partnya Bag":
                this.statsTextTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Partnya Bag\n\n{0}", this.FormatInventory(DatabaseManager.GetPartnyaBagDictionary(runID)));
                break;
            case "Area Changes":
                this.statsTextTextBlock.Text = string.Format(CultureInfo.InvariantCulture, "Area Changes\n\n{0}", this.DisplayAreaChanges(DatabaseManager.GetAreaChangesDictionary(runID)));
                break;
            default:
                break;
        }

        this.statsTextSelectedOption = selectedOption.Trim().Replace(" ", "_");
    }

    // TODO: double-check the settings and the conditionals in the other code
    private void SettingsPresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        var comboBox = (ComboBox)sender;

        var selectedItem = (ComboBoxItem)comboBox.SelectedItem;

        if (selectedItem == null)
        {
            return;
        }

        var selectedOption = selectedItem.Content.ToString();

        if (string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        if (s != null)
        {
            OverlaySettingsService.SetConfigurationPreset(s, ConfigurationPresetConverter.Convert(selectedOption));
        }
    }

    private void PersonalBest_Loaded(object sender, RoutedEventArgs e) => this.personalBestChart = (CartesianChart)sender;

    private void PersonalBestTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.personalBestChart == null)
        {
            return;
        }

        if (sender is not ComboBox comboBox)
        {
            return;
        }

        var selectedItem = comboBox.SelectedItem;

        if (selectedItem == null)
        {
            return;
        }

        var selectedType = selectedItem.ToString();
        if (string.IsNullOrEmpty(selectedType))
        {
            return;
        }

        this.personalBestSelectedType = selectedType.Replace("System.Windows.Controls.ComboBoxItem: ", string.Empty);
    }

    private void PersonalBestWeaponComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.personalBestChart == null)
        {
            return;
        }

        if (sender is not ComboBox comboBox)
        {
            return;
        }

        var selectedItem = comboBox.SelectedItem;

        if (selectedItem == null)
        {
            return;
        }

        var selectedWeapon = selectedItem.ToString();
        if (string.IsNullOrEmpty(selectedWeapon))
        {
            return;
        }

        this.personalBestSelectedWeapon = selectedWeapon.Replace("System.Windows.Controls.ComboBoxItem: ", string.Empty);
    }

    private void StatsGraphsRefreshButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.graphChart == null || statsGraphsComboBoxOption == null || string.IsNullOrEmpty(statsGraphsComboBoxOption))
        {
            return;
        }

        UpdateStatsGraphs(statsGraphsComboBoxOption);
    }

    private void UpdateStatsGraphs(string selectedOption)
    {
        this.Series = null;
        this.XAxes = new Axis[]
        {
            new Axis
            {
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };
        this.YAxes = new Axis[]
        {
            new Axis
            {
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        long runID = 0;
        if (this.RunIDTextBox != null && this.RunIDTextBox.Text != string.Empty)
        {
            runID = long.Parse(this.RunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        }

        long extraRunID = 0;
        if (this.extraRunIDTextBox != null && this.extraRunIDTextBox.Text != string.Empty)
        {
            extraRunID = long.Parse(this.extraRunIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        }

        if (runIDComparisonTextBlock != null)
        {
            runIDComparisonTextBlock.Text = string.Empty;
        }

        switch (selectedOption)
        {
            case "(General) Most Quest Completions":
                this.SetColumnSeriesForDictionaryIntInt(DatabaseManager.GetMostQuestCompletions());
                break;
            case "(General) Quest Durations":
                this.CreateQuestDurationStackedChart(DatabaseManager.GetTotalTimeSpentInQuests());
                break;
            case "(General) Most Common Objective Types":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonObjectiveTypes());
                break;
            case "(General) Most Common Star Grades":
                this.SetColumnSeriesForDictionaryIntInt(DatabaseManager.GetMostCommonStarGrades());
                break;
            case "(General) Most Common Rank Bands":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonRankBands());
                break;
            case "(General) Most Common Objective":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonObjectives());
                break;
            case "(General) Quests Completed by Date":
                this.SetColumnSeriesForDictionaryDateInt(DatabaseManager.GetQuestsCompletedByDate());
                break;
            case "(General) Most Common Party Size":
                this.SetColumnSeriesForDictionaryIntInt(DatabaseManager.GetMostCommonPartySize());
                break;
            case "(General) Most Common Set Name":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonSetNames());
                break;
            case "(General) Most Common Weapon Name":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonWeaponNames());
                break;
            case "(General) Most Common Head Piece":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonHeadPieces());
                break;
            case "(General) Most Common Chest Piece":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonChestPieces());
                break;
            case "(General) Most Common Arms Piece":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonArmsPieces());
                break;
            case "(General) Most Common Waist Piece":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonWaistPieces());
                break;
            case "(General) Most Common Legs Piece":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonLegsPieces());
                break;
            case "(General) Most Common Diva Skill":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonDivaSkill());
                break;
            case "(General) Most Common Guild Food":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonGuildFood());
                break;
            case "(General) Most Common Style Rank Skills":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonStyleRankSkills());
                break;
            case "(General) Most Common Caravan Skills":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonCaravanSkills());
                break;
            case "(General) Most Common Category":
                this.SetColumnSeriesForDictionaryStringInt(DatabaseManager.GetMostCommonCategory());
                break;
            case "(Run ID) Attack Buff":
                this.SetLineSeriesForDictionaryIntInt(DatabaseManager.GetAttackBuffDictionary(runID), DatabaseManager.GetAttackBuffDictionary(extraRunID));
                return;
            case "(Run ID) Hit Count":
                this.SetLineSeriesForDictionaryIntInt(DatabaseManager.GetHitCountDictionary(runID), DatabaseManager.GetHitCountDictionary(extraRunID));
                return;
            case "(Run ID) Hits per Second":
                this.SetLineSeriesForDictionaryIntDouble(DatabaseManager.GetHitsPerSecondDictionary(runID), DatabaseManager.GetHitsPerSecondDictionary(extraRunID));
                return;
            case "(Run ID) Damage Dealt":
                this.SetLineSeriesForDictionaryIntInt(DatabaseManager.GetDamageDealtDictionary(runID), DatabaseManager.GetDamageDealtDictionary(extraRunID));
                return;
            case "(Run ID) Damage per Second":
                this.SetLineSeriesForDictionaryIntDouble(DatabaseManager.GetDamagePerSecondDictionary(runID), DatabaseManager.GetDamagePerSecondDictionary(extraRunID));
                return;
            case "(Run ID) Carts":
                this.SetLineSeriesForDictionaryIntInt(DatabaseManager.GetCartsDictionary(runID), null);
                return;
            case "(Run ID) Hits Taken/Blocked":
                this.SetHitsTakenBlocked(DatabaseManager.GetHitsTakenBlockedDictionary(runID), DatabaseManager.GetHitsTakenBlockedDictionary(extraRunID));
                return;
            case "(Run ID) Hits Taken/Blocked per Second":
                this.SetLineSeriesForDictionaryIntDouble(DatabaseManager.GetHitsTakenBlockedPerSecondDictionary(runID), DatabaseManager.GetHitsTakenBlockedPerSecondDictionary(extraRunID));
                return;
            case "(Run ID) Player Health and Stamina":
                this.SetPlayerHealthStamina(DatabaseManager.GetPlayerHPDictionary(runID), DatabaseManager.GetPlayerStaminaDictionary(runID));
                return;
            case "(Run ID) Most Common Player Input":
                this.SetColumnSeriesForDictionaryStringInt(GetMostCommonInputs(runID));
                break;
            case "(Run ID) Actions per Minute":
                this.SetLineSeriesForDictionaryIntDouble(DatabaseManager.GetActionsPerMinuteDictionary(runID), DatabaseManager.GetActionsPerMinuteDictionary(extraRunID));
                return;
            case "(Run ID) Monster HP":
                this.SetMonsterHP(CalculateMonsterHP(DatabaseManager.GetMonster1HPDictionary(runID)), CalculateMonsterHP(DatabaseManager.GetMonster2HPDictionary(runID)), CalculateMonsterHP(DatabaseManager.GetMonster3HPDictionary(runID)), CalculateMonsterHP(DatabaseManager.GetMonster4HPDictionary(runID)));
                return;
            case "(Run ID) Monster Attack Multiplier":
                this.SetMonsterAttackMultiplier(CalculateMonsterMultiplier(DatabaseManager.GetMonster1AttackMultiplierDictionary(runID)));
                return;
            case "(Run ID) Monster Defense Rate":
                this.SetMonsterDefenseRate(CalculateMonsterMultiplier(DatabaseManager.GetMonster1DefenseRateDictionary(runID)));
                return;
            case "(Run ID) Monster Status Ailments Thresholds":
                this.SetMonsterStatusAilmentsThresholds(
                    CalculateMonsterStatusAilmentThresholds(
                        DatabaseManager.GetMonster1PoisonThresholdDictionary(runID)),
                    CalculateMonsterStatusAilmentThresholds(
                        DatabaseManager.GetMonster1SleepThresholdDictionary(runID)),
                    CalculateMonsterStatusAilmentThresholds(
                        DatabaseManager.GetMonster1ParalysisThresholdDictionary(runID)),
                    CalculateMonsterStatusAilmentThresholds(
                        DatabaseManager.GetMonster1BlastThresholdDictionary(runID)),
                    CalculateMonsterStatusAilmentThresholds(
                        DatabaseManager.GetMonster1StunThresholdDictionary(runID)));
                return;
            case "(Run ID) Dual Swords Sharpens":
                this.SetStepLineSeriesForDictionaryIntInt(DatabaseManager.GetDualSwordsSharpensDictionary(runID), DatabaseManager.GetDualSwordsSharpensDictionary(extraRunID));
                return;
            default:
                this.graphChart.Series = this.Series ?? Array.Empty<ISeries>(); // TODO test
                this.graphChart.XAxes = this.XAxes;
                this.graphChart.YAxes = this.YAxes;
                break;
        }

        this.statsGraphsSelectedOption = selectedOption.Trim().Replace(" ", "_");

        if (this.Series == null)
        {
            return;
        }

        this.graphChart.Series = this.Series;
        this.graphChart.XAxes = this.XAxes;
        this.graphChart.YAxes = this.YAxes;
    }

    private RunBuff GetRunBuffs(ObservableCollection<QuestLogsOption> options)
    {

        var runBuffs = RunBuff.None;

        if (options.Any(o => o.Name == "Halk" && o.IsSelected))
        {
            runBuffs |= RunBuff.Halk;
        }

        if (options.Any(o => o.Name == "Poogie Item" && o.IsSelected))
        {
            runBuffs |= RunBuff.PoogieItem;
        }

        if (options.Any(o => o.Name == "Diva Song" && o.IsSelected))
        {
            runBuffs |= RunBuff.DivaSong;
        }

        if (options.Any(o => o.Name == "Halk Pot Effect" && o.IsSelected))
        {
            runBuffs |= RunBuff.HalkPotEffect;
        }

        if (options.Any(o => o.Name == "Bento" && o.IsSelected))
        {
            runBuffs |= RunBuff.Bento;
        }

        if (options.Any(o => o.Name == "Guild Poogie" && o.IsSelected))
        {
            runBuffs |= RunBuff.GuildPoogie;
        }

        if (options.Any(o => o.Name == "Active Feature" && o.IsSelected))
        {
            runBuffs |= RunBuff.ActiveFeature;
        }

        if (options.Any(o => o.Name == "Guild Food" && o.IsSelected))
        {
            runBuffs |= RunBuff.GuildFood;
        }

        if (options.Any(o => o.Name == "Diva Skill" && o.IsSelected))
        {
            runBuffs |= RunBuff.DivaSkill;
        }

        if (options.Any(o => o.Name == "Secret Technique" && o.IsSelected))
        {
            runBuffs |= RunBuff.SecretTechnique;
        }

        if (options.Any(o => o.Name == "Diva Prayer Gem" && o.IsSelected))
        {
            runBuffs |= RunBuff.DivaPrayerGem;
        }

        if (options.Any(o => o.Name == "Course Attack Boost" && o.IsSelected))
        {
            runBuffs |= RunBuff.CourseAttackBoost;
        }

        return runBuffs;
    }

    private void PersonalBestRefreshButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.personalBestChart == null || this.personalBestSelectedWeapon == string.Empty || this.personalBestSelectedType == string.Empty)
        {
            return;
        }

        this.PersonalBestSeries = null;
        this.PersonalBestXAxes = new Axis[]
        {
            new Axis
            {
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };
        this.PersonalBestYAxes = new Axis[]
        {
            new Axis
            {
                NamePaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
                LabelsPaint = new SolidColorPaint(new SKColor(this.MainWindow.DataLoader.Model.HexColorToDecimal("#a6adc8"))),
            },
        };

        var questID = long.Parse(this.QuestIDTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        var weaponTypeID = WeaponType.IDName.FirstOrDefault(x => x.Value == this.personalBestSelectedWeapon).Key;

        switch (this.personalBestSelectedType)
        {
            case "(Quest ID) Personal Best by Date":
                this.SetStepLineSeriesForPersonalBestByDate(DatabaseManager.GetPersonalBestsByDate(questID, weaponTypeID, this.OverlayModeComboBox.Text, (uint)GetRunBuffs(this.MainWindow.DataLoader.Model.RunBuffsSearchOption)));
                break;
            case "(Quest ID) Personal Best by Attempts":
                this.SetStepLineSeriesForPersonalBestByAttempts(DatabaseManager.GetPersonalBestsByAttempts(questID, weaponTypeID, this.OverlayModeComboBox.Text, (uint)GetRunBuffs(this.MainWindow.DataLoader.Model.RunBuffsSearchOption)));
                break;
            default:
                this.personalBestChart.Series = this.PersonalBestSeries ?? Array.Empty<ISeries>(); // TODO test
                this.personalBestChart.XAxes = this.PersonalBestXAxes;
                this.personalBestChart.YAxes = this.PersonalBestYAxes;
                break;
        }

        if (this.personalBestDescriptionTextBlock != null)
        {
            this.personalBestDescriptionTextBlock.Text = $"Personal best of solo runs of quest ID {this.QuestIDTextBox.Text} by category {this.OverlayModeComboBox.Text}";
        }
    }

    private void HunterPerformancePolarChart_Loaded(object sender, RoutedEventArgs e)
    {
        var chart = sender as PolarChart;
        this.hunterPerformanceChart = chart;
        this.SetPolarLineSeriesForHunterPerformance(DatabaseManager.GetPerformanceCompendium());
    }

    private void CompendiumInformationStackPanel_Loaded(object sender, RoutedEventArgs e)
    {
        var stackPanel = sender as StackPanel;
        this.compendiumInformationStackPanel = stackPanel;
    }

    private void CalendarDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.calendarDataGrid == null || sender == null)
        {
            return;
        }

        if (sender is not DatePicker datePicker)
        {
            return;
        }

        var selectedDate = datePicker.SelectedDate;

        if (selectedDate == null)
        {
            return;
        }

        this.datePickerDate = (DateTime)selectedDate;
        this.MainWindow.DataLoader.Model.CalendarRuns = DatabaseManager.GetCalendarRuns(selectedDate);
        this.calendarDataGrid.ItemsSource = this.MainWindow.DataLoader.Model.CalendarRuns;
        this.calendarDataGrid.Items.Refresh();
    }

    private void MostRecentRuns_DataGridLoaded(object sender, RoutedEventArgs e)
    {
        this.mostRecentRunsDataGrid = (DataGrid)sender;
        this.MainWindow.DataLoader.Model.RecentRuns = DatabaseManager.GetRecentRuns();
        this.mostRecentRunsDataGrid.ItemsSource = this.MainWindow.DataLoader.Model.RecentRuns;
        this.mostRecentRunsDataGrid.Items.Refresh();
    }

    private void Calendar_DataGridLoaded(object sender, RoutedEventArgs e) => this.calendarDataGrid = (DataGrid)sender;

    private bool isDefaultButtonClickedSubscribed;

    private bool isSaveButtonClickedSubscribed;

    // https://stackoverflow.com/questions/36128148/pass-click-event-of-child-control-to-the-parent-control
    private void MainConfigurationActions_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (MainConfigurationActions)sender;

        // Subscribe to events only if not already subscribed
        if (!this.isSaveButtonClickedSubscribed)
        {
            obj.SaveButtonClicked += this.SaveButton_Click;
            this.isSaveButtonClickedSubscribed = true;
        }

        if (!this.isConfigureButtonClickedSubscribed)
        {
            obj.ConfigureButtonClicked += this.ConfigureButton_Click;
            this.isConfigureButtonClickedSubscribed = true;
        }

        if (!this.isDefaultButtonClickedSubscribed)
        {
            obj.DefaultButtonClicked += this.DefaultButton_Click;
            this.isDefaultButtonClickedSubscribed = true;
        }
    }

    // I do not understand the following function, but I've verified it is required for the above function to work correctly.
    private void MainConfigurationActions_Unloaded(object sender, RoutedEventArgs e)
    {
        var obj = (MainConfigurationActions)sender;

        // Unsubscribe from events and update flags
        if (this.isSaveButtonClickedSubscribed)
        {
            obj.SaveButtonClicked -= this.SaveButton_Click;
            this.isSaveButtonClickedSubscribed = false;
        }

        if (this.isConfigureButtonClickedSubscribed)
        {
            obj.ConfigureButtonClicked -= this.ConfigureButton_Click;
            this.isConfigureButtonClickedSubscribed = false;
        }

        if (this.isDefaultButtonClickedSubscribed)
        {
            obj.DefaultButtonClicked -= this.DefaultButton_Click;
            this.isDefaultButtonClickedSubscribed = false;
        }
    }

    private void PersonalBestChartGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.personalBestChartGrid = obj;
        }
    }

    private void WeaponUsageChartGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.weaponUsageChartGrid = obj;
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
        var selectedOption = selectedItem.ToString()?.Replace("System.Windows.Controls.ComboBoxItem: ", string.Empty).Trim().Replace(" ", "_");
        if (string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        this.statsGraphsSelectedOption = selectedOption;
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
        var selectedOption = selectedItem.ToString()?.Replace("System.Windows.Controls.ComboBoxItem: ", string.Empty).Trim().Replace(" ", "_");
        if (string.IsNullOrEmpty(selectedOption))
        {
            return;
        }

        this.statsTextSelectedOption = selectedOption;
    }

    private void GraphsChartGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.statsGraphsGrid = obj;
        }
    }

    private void QuestPaceGraphGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.questPaceGraphGrid = obj;
        }
    }

    private void QuestPaceGraph_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (CartesianChart)sender;
        if (obj != null)
        {
            this.questPaceGraph = obj;
        }
    }

    private void PersonalBestDescriptionTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (TextBlock)sender;
        if (obj != null)
        {
            this.personalBestDescriptionTextBlock = obj;
        }
    }

    private void QuestPaceDescriptionTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (TextBlock)sender;
        if (obj != null)
        {
            this.questPaceDescriptionTextBlock = obj;
        }
    }

    private void Top20RunsDescriptionTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (TextBlock)sender;
        if (obj != null)
        {
            this.top20RunsDescriptionTextBlock = obj;
        }
    }

    private void PersonalBestMainGridLoaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.personalBestMainGrid = obj;
        }
    }

    private void Top20MainGridLoaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.top20MainGrid = obj;
        }
    }

    private void QuestPaceMainGridLoaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.questPaceMainGrid = obj;
        }
    }

    private void WeaponStatsMainGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.weaponStatsMainGrid = obj;
        }
    }

    private void StatsGraphsMainGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.statsGraphsMainGrid = obj;
        }
    }

    private void StatsTextMainGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var obj = (Grid)sender;
        if (obj != null)
        {
            this.statsTextMainGrid = obj;
        }
    }

    private void FumoImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
        {
            Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
        };
        AchievementManager.RewardAchievement(225, snackbar, (Style)this.FindResource("CatppuccinMochaSnackBar"));
    }

    private void Achievements3DPreviewGrid_Loaded(object sender, RoutedEventArgs e)
    {
        // Create the rotation animation
        var storyboard = new Storyboard();
        var animation = new DoubleAnimation
        {
            From = 0,
            To = 360,
            Duration = TimeSpan.FromSeconds(10), // Adjust the duration to control the rotation speed
            RepeatBehavior = RepeatBehavior.Forever,
        };
        this.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
        storyboard.Begin();
    }

    private void AchievementsListView_Loaded(object sender, RoutedEventArgs e)
    {
        this.achievementsListView = (ListView)sender;
        this.MainWindow.DataLoader.Model.PlayerAchievements = DatabaseManager.GetPlayerAchievements();
        this.MainWindow.DataLoader.Model.ObtainablePlayerAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Where((e) => e.Unused is false or null).ToList();

        this.achievementsListView.ItemsSource = this.MainWindow.DataLoader.Model.ObtainablePlayerAchievements;
        this.achievementsListView.Items.Refresh();
        this.UpdateAchievementsProgress();
    }

    private void AchievementsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.achievementsSelectedInfoGrid is null)
        {
            return;
        }

        if (this.AchievementsListView.SelectedItem is Achievement selectedAchievement)
        {
            // Update the other grid's UI elements based on the selected achievement
            // For example:
            this.AchievementSelectionInfoImage.Source = new BitmapImage(new Uri(selectedAchievement.Image));
            if (selectedAchievement.CompletionDate == DateTime.UnixEpoch)
            {
                this.AchievementSelectionInfoTitle.Text = selectedAchievement.Title;
                this.AchievementSelectionInfoImage.Opacity = .25;
            }
            else
            {
                this.AchievementSelectionInfoImage.Opacity = 1;
                this.AchievementSelectionInfoTitle.Text = $"{selectedAchievement.Title} | {selectedAchievement.CompletionDate:yy/MM/dd HH:mm:ss}";
            }

            var brushConverter = new BrushConverter();

            if (selectedAchievement.IsSecret && selectedAchievement.CompletionDate == DateTime.UnixEpoch)
            {
                this.AchievementSelectionInfoObjective.Text = string.Empty;
                this.AchievementSelectionInfoTitle.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Text"]);
            }
            else
            {
                this.AchievementSelectionInfoObjective.Text = selectedAchievement.Objective;
                this.AchievementSelectionInfoTitle.Fill = selectedAchievement.GetBrushColorFromRank();
            }

            this.AchievementSelectionInfoHint.Text = selectedAchievement.Hint;

            this.AchievementFrontSide.ImageSource = new BitmapImage(new Uri($"{selectedAchievement.Image}"));
            this.AchievementBackSide.ImageSource = new BitmapImage(new Uri($"{selectedAchievement.GetTrophyImageLinkFromRank()}"));
        }
    }

    private void UpdateAchievementsProgress()
    {
        var totalAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.IsSecret == false && (a.Unused is false or null));
        var obtainedAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch);
        var obtainedSecretAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.IsSecret);
        var totalSecretAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.IsSecret);
        var progressPercentage = (obtainedAchievements * 100.0 / totalAchievements) == 100 ? (obtainedAchievements * 100.0 / totalAchievements) + (obtainedSecretAchievements / totalSecretAchievements) : (obtainedAchievements * 100.0 / totalAchievements);
        this.AchievementsProgressBar.Value = progressPercentage;

        var totalBronzeAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.IsSecret == false && a.Rank == AchievementRank.Bronze && (a.Unused is false or null));
        var totalSilverAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.IsSecret == false && a.Rank == AchievementRank.Silver && (a.Unused is false or null));
        var totalGoldAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.IsSecret == false && a.Rank == AchievementRank.Gold && (a.Unused is false or null));
        var totalPlatinumAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.IsSecret == false && a.Rank == AchievementRank.Platinum && (a.Unused is false or null));

        var obtainedBronzeAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Bronze);
        var obtainedSilverAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Silver);
        var obtainedGoldAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Gold);
        var obtainedPlatinumAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Platinum);

        this.TrophyBronzeCountTextBlock.Text = $"{obtainedBronzeAchievements}/{totalBronzeAchievements}";
        this.TrophySilverCountTextBlock.Text = $"{obtainedSilverAchievements}/{totalSilverAchievements}";
        this.TrophyGoldCountTextBlock.Text = $"{obtainedGoldAchievements}/{totalGoldAchievements}";
        this.TrophyPlatinumCountTextBlock.Text = $"{obtainedPlatinumAchievements}/{totalPlatinumAchievements}";

        var brushConverter = new BrushConverter();

        if (obtainedAchievements is >= 50 and < 100) // unlock Bingo + Gacha
        {
            this.AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Maroon"]);
            this.AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Maroon"]);
            this.AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Maroon"]);
            this.AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Maroon"]);
        }
        else if (obtainedAchievements is >= 100 and < 150) // unlock zenith gauntlet
        {
            this.AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Lavender"]);
            this.AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Lavender"]);
            this.AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Lavender"]);
            this.AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Lavender"]);
        }
        else if (obtainedAchievements is >= 150 and < 200) // unlock solstice gauntlet
        {
            this.AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Yellow"]);
            this.AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Yellow"]);
            this.AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Yellow"]);
            this.AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Yellow"]);
        }
        else if (obtainedAchievements is >= 200 and < 300) // unlock musou gauntlet
        {
            this.AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            this.AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            this.AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            this.AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
        }
        else if (obtainedAchievements == totalAchievements) // all
        {
            this.AchievementsProgressBar.Foreground = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            this.AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            this.AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);
            this.AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]);

            // Create and apply the DropShadowEffect to the ProgressBar and TextBlock
            var dropShadowEffect = new DropShadowEffect
            {
                Color = (Color)ColorConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Teal"]),
                ShadowDepth = 0,
                BlurRadius = 15,
                Opacity = 1,
            };

            this.AchievementsProgressBar.Effect = dropShadowEffect;
            this.AchievementsProgressTextBlock.Effect = dropShadowEffect;
            this.AchievementTotalProgressTextBlock.Effect = dropShadowEffect;
            this.AchievementTotalProgressPercentTextBlock.Effect = dropShadowEffect;
        }
        else
        {
            this.AchievementsProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Text"]);
            this.AchievementTotalProgressTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Text"]);
            this.AchievementTotalProgressPercentTextBlock.Fill = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Text"]);
        }

        this.AchievementsProgressTextBlock.Text = $"{obtainedAchievements}/{totalAchievements}";
        this.AchievementTotalProgressPercentTextBlock.Text = $"{progressPercentage:F2}%";
        this.AchievementSelectionInfoHint.Text = $"Most achievements requirements are checked when you are at the quest completion rewards screen. There are 5 types of trophies: bronze, silver, gold, platinum and secret. For every secret trophy you find, you gain {1.0 / totalSecretAchievements:F2}% more progress if you have already obtained every non-secret achievement already. You have obtained {obtainedSecretAchievements} out of {totalSecretAchievements} secret trophies ({obtainedSecretAchievements * 100.0 / totalSecretAchievements:F2}%). Custom quests are not counted for progress for almost all achievements.";
    }

    private void AchievementSelectedInfoGrid_Loaded(object sender, RoutedEventArgs e) => this.achievementsSelectedInfoGrid = (Grid)sender;

    private void HunterNotesGridMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem && menuItem.Parent is ContextMenu contextMenu && contextMenu.PlacementTarget is FrameworkElement element)
        {
            var snackbar = new Snackbar(this.ConfigWindowSnackBarPresenter)
            {
                Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
            };

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
                    Logger.Error("Invalid Menu Item option: {0}", menuItem);
                    snackbar.Title = Messages.ErrorTitle;
                    snackbar.Content = $"Invalid Menu Item option: {menuItem}";
                    snackbar.Appearance = ControlAppearance.Danger;
                    snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle20);
                    snackbar.Timeout = this.SnackbarTimeOut;
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
                    Logger.Error("Invalid Menu Item option: {0}", menuItem);
                    snackbar.Title = Messages.ErrorTitle;
                    snackbar.Content = $"Invalid Menu Item option: {menuItem}";
                    snackbar.Appearance = ControlAppearance.Danger;
                    snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle20);
                    snackbar.Timeout = this.SnackbarTimeOut;
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
                        FileService.SaveRecordsAsCSVFile(this.monsters, snackbar, "HuntedLog");
                    }
                    else if (element.Name == "AchievementsGrid")
                    {
                        FileService.SaveRecordsAsCSVFile(
                            AchievementService.FilterAchievementsToCompletedOnly(
                                this.MainWindow.DataLoader.Model.PlayerAchievements).ToArray(),
                            snackbar,
                            "Achievements");
                    }
                    else
                    {
                        Logger.Error("Unhandled csv class records: {0}", element.Name);
                        snackbar.Title = Messages.ErrorTitle;
                        snackbar.Content = "Could not save class records as CSV file: unhandled element";
                        snackbar.Appearance = ControlAppearance.Danger;
                        snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle20);
                        snackbar.Timeout = this.SnackbarTimeOut;
                        snackbar.Show();
                    }
                }
                else
                {
                    Logger.Error("Invalid Menu Item option: {0}", menuItem);
                    snackbar.Title = Messages.ErrorTitle;
                    snackbar.Content = $"Invalid Menu Item option: {menuItem}";
                    snackbar.Appearance = ControlAppearance.Danger;
                    snackbar.Icon = new SymbolIcon(SymbolRegular.ErrorCircle20);
                    snackbar.Timeout = this.SnackbarTimeOut;
                    snackbar.Show();
                }
            }
            else
            {
                Logger.Error("Unhandled Context Menu found: {0}", contextMenu.Name);
            }
        }
    }

    private void AchievementsSearchButton_Click(object sender, RoutedEventArgs e)
    {
        // Check if the text in the AutoSuggestBox is empty
        if (string.IsNullOrWhiteSpace(this.AchievementsSearchComboBox.Text))
        {
            // If the text is empty, show the original list in the ListView
            this.AchievementsListView.ItemsSource = this.MainWindow.DataLoader.Model.PlayerAchievements.Where(achievement => (achievement.Unused is false or null))
                .ToList();
        }
        else
        {
            // If the text is not empty, set the ItemsSource to null temporarily to clear the ListView
            this.AchievementsListView.ItemsSource = null;

            // Then, set the ItemsSource back to the filtered achievements list based on the user's input
            var userInput = this.AchievementsSearchComboBox.Text;
            var filteredAchievements = this.MainWindow.DataLoader.Model.PlayerAchievements
                .Where(achievement => achievement.Title.Contains(userInput, StringComparison.OrdinalIgnoreCase) && (achievement.Unused is false or null))
                .ToList();
            this.AchievementsListView.ItemsSource = filteredAchievements;
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
        if (sender is ListBox listBox)
        {
            listBox.SelectedItem = null;
        }
    }

    private void GameInfoNavigationCommandsBrowseBack(object sender, RoutedEventArgs e) => this.webViewFerias.GoBack();

    private void GameInfoNavigationCommandsBrowseForward(object sender, RoutedEventArgs e) => this.webViewFerias.GoForward();

    private void GameInfoNavigationCommandsRefresh(object sender, RoutedEventArgs e) => this.webViewFerias.Reload();

    private void GameInfoNavigationCommandsBrowseStop(object sender, RoutedEventArgs e) => this.webViewFerias.Stop();

    private void GameInfoNavigationCommandsGoToPage(object sender, RoutedEventArgs e)
    {
        try
        {
            if (this.webViewFerias != null && this.webViewFerias.CoreWebView2 != null)
            {
                this.webViewFerias.CoreWebView2.Navigate(this.GameInfoURL.Text);
            }
        }
        catch
        {
            Logger.Error("Could not navigate in WebView2");
        }
    }

    private void WycademyNavigationCommandsBrowseBack(object sender, RoutedEventArgs e) => this.webViewFerias.GoBack();

    private void WycademyNavigationCommandsBrowseForward(object sender, RoutedEventArgs e) => this.webViewFerias.GoForward();

    private void WycademyNavigationCommandsRefresh(object sender, RoutedEventArgs e) => this.webViewFerias.Reload();

    private void WycademyNavigationCommandsBrowseStop(object sender, RoutedEventArgs e) => this.webViewFerias.Stop();

    private void WycademyNavigationCommandsGoToPage(object sender, RoutedEventArgs e)
    {
        try
        {
            if (this.webViewWycademy != null && this.webViewWycademy.CoreWebView2 != null)
            {
                this.webViewWycademy.CoreWebView2.Navigate(this.WycademyURL.Text);
            }
        }
        catch
        {
            Logger.Error("Could not navigate in WebView2");
        }
    }

    public ICommand StartChallengeCommand { get; private set; }

    private void CheckForChallengeRequirementsForStart(Challenge challenge)
    {
        var brushConverter = new BrushConverter();
        var brushColor = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Crust"]);
        var snackbar = new Snackbar(ConfigWindowSnackBarPresenter) { };

        if (ChallengeServiceInstance.CheckRequirements(challenge))
        {
            Logger.Info(CultureInfo.InvariantCulture, "Meets requirements for challenge unlock, unlocking challenge {0}", challenge.Name);
            var successful = ChallengeServiceInstance.Unlock(challenge);

            if (!successful)
            {
                Logger.Error(CultureInfo.InvariantCulture, "Could not unlock challenge");
                snackbar = new Snackbar(ConfigWindowSnackBarPresenter)
                {
                    Title = "Challenge locked!",
                    Content = $"{challenge.Name} could not be unlocked, it may currently be in development.",
                    Icon = new SymbolIcon()
                    {
                        Symbol = SymbolRegular.LockOpen28,
                        Foreground = brushColor ?? Brushes.Black,
                    },
                    Appearance = ControlAppearance.Info,
                    Timeout = TimeSpan.FromSeconds(10),
                    Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
                };

                ConfigWindowSnackBarPresenter.AddToQue(snackbar);
                return;
            }

            if (this.challengesListBox == null)
            {
                Logger.Error(CultureInfo.InvariantCulture, "Challenges ListBox not found");
                return;
            }

            this.MainWindow.DataLoader.Model.PlayerChallenges = DatabaseManager.GetPlayerChallenges();
            this.challengesListBox.ItemsSource = this.MainWindow.DataLoader.Model.PlayerChallenges.Values.ToList();
            foreach (var item in (List<Challenge>)this.challengesListBox.ItemsSource)
            {
                item.StartChallengeCommand = StartChallengeCommand;
            }

            this.challengesListBox.Items.Refresh();

            var s = (Settings)Application.Current.TryFindResource("Settings");
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\Sounds\challenge_unlock.wav");
            AudioServiceInstance.Play(fileName, MainWindow.MainWindowMediaPlayer, s.VolumeMain, s.VolumeChallengeUnlock);
            snackbar = new Snackbar(ConfigWindowSnackBarPresenter)
            {
                Title = "Challenge unlocked!",
                Content = $"Congratulations on unlocking {challenge.Name}, you can now start it by pressing the Start button inside that challenge section.",
                Icon = new SymbolIcon()
                {
                    Symbol = SymbolRegular.LockOpen28,
                    Foreground = brushColor ?? Brushes.Black,
                },
                Appearance = ControlAppearance.Info,
                Timeout = TimeSpan.FromSeconds(10),
                Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
            };

            ConfigWindowSnackBarPresenter.AddToQue(snackbar);
        }
        else
        {
            snackbar = new Snackbar(ConfigWindowSnackBarPresenter)
            {
                Title = "Challenge locked!",
                Content = "Check the information bar at the top of this section in order to view unlocking instructions.",
                Icon = new SymbolIcon()
                {
                    Symbol = SymbolRegular.LockClosed32,
                    Foreground = brushColor ?? Brushes.Black,
                },
                Appearance = ControlAppearance.Info,
                Timeout = SnackbarTimeOut,
                Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
            };

            ConfigWindowSnackBarPresenter.AddToQue(snackbar);
        }
    }

    private void StartChallenge(Challenge? challenge)
    {
        return; // TODO uncomment when in release for now.

        if (challenge == null || this.challengesListBox == null)
        {
            Logger.Warn(CultureInfo.InvariantCulture, "Challenge not found, canceling start process");
            return;
        }

        // Challenge unlock date is not default, can skip requirements check.
        if (challenge.UnlockDate != DateTime.UnixEpoch)
        {
            var successful = ChallengeServiceInstance.Start(challenge);

            if (!successful)
            {
                Logger.Warn(CultureInfo.InvariantCulture, "Could not start challenge {0}", challenge.Name);

                if (ChallengeServiceInstance.State == ChallengeState.Running)
                {
                    var brushConverter = new BrushConverter();
                    var brushColor = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Crust"]);
                    var snackbar = new Snackbar(ConfigWindowSnackBarPresenter)
                    {
                        Title = "Could not start challenge",
                        Content = "A challenge might already be in progress. If you have a challenge window open, please close it before starting another one.",
                        Icon = new SymbolIcon()
                        {
                            Symbol = SymbolRegular.Warning28,
                            Foreground = brushColor ?? Brushes.Black,
                        },
                        Appearance = ControlAppearance.Caution,
                        Timeout = TimeSpan.FromSeconds(10),
                        Style = (Style)this.FindResource("CatppuccinMochaSnackBar"),
                    };

                    ConfigWindowSnackBarPresenter.AddToQue(snackbar);
                }

                return;
            }

            var s = (Settings)Application.Current.TryFindResource("Settings");
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\Sounds\challenge_start.wav");
            AudioServiceInstance.Play(fileName, MainWindow.MainWindowMediaPlayer, s.VolumeMain, s.VolumeChallengeStart);

            this.Close();

            return;
        }
        else
        {
            CheckForChallengeRequirementsForStart(challenge);
        }
    }

    private void ChallengesListBox_Loaded(object sender, RoutedEventArgs e)
    {
        this.challengesListBox = (ListBox)sender;
        this.MainWindow.DataLoader.Model.PlayerChallenges = DatabaseManager.GetPlayerChallenges();
        this.challengesListBox.ItemsSource = this.MainWindow.DataLoader.Model.PlayerChallenges.Values.ToList();
        foreach (var challenge in (List<Challenge>)this.challengesListBox.ItemsSource)
        {
            challenge.StartChallengeCommand = StartChallengeCommand;
        }

        this.challengesListBox.Items.Refresh();
    }

    private void ExtraRunIDTextBox_Loaded(object sender, RoutedEventArgs e)
    {
        this.extraRunIDTextBox = (TextBox)sender;
    }

    private void RunIDComparisonTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        this.runIDComparisonTextBlock = (TextBlock)sender;
    }
}

/* LoadConfig on startup
 * Load Config on window open to have extra copy
 * On Save -> Window close -> tell program to use new copy instead of current -> Save Config File
 * On Cancel -> Window Close -> Discard copy of config
 * On Config Change Still show changes immediately and show windows which are set to show -> Ignore logic that hides windows during this time and force  them on if they are enabled
 *
 */
