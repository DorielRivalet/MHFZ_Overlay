﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Input;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics.Metrics;
using System.Threading;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using Application = System.Windows.Application;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Clipboard = System.Windows.Clipboard;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;
using ListViewItem = System.Windows.Controls.ListViewItem;
using ListView = System.Windows.Controls.ListView;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Reflection;
using Binding = System.Windows.Data.Binding;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Window = System.Windows.Window;
using CsvHelper;
using MaterialDesignThemes.Wpf.Converters;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Microsoft.Web.WebView2.Core;


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

        public Uri MonsterImage
        {
            get { return new Uri("https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png", UriKind.RelativeOrAbsolute); }
        }

        public Monster[] Monsters = new Monster[]
        {
          new Monster(0, "None","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/none.png",0),
          new Monster(1, "Rathian","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/rathian.png",0,true),
          new Monster(2, "Fatalis","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/fatalis.png",0,true),
          new Monster(3, "Kelbi","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/kelbi.png",0),
          new Monster(4, "Mosswine","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/mosswine.png",0),
          new Monster(5, "Bullfango","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/bullfango.png",0),
          new Monster(6, "Yian Kut-Ku","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/yian_kut-ku.png",0,true),
          new Monster(7, "Lao-Shan Lung","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/lao-shan_lung.png",0,true),
          new Monster(8, "Cephadrome","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/cephadrome.png",0,true),
          new Monster(9, "Felyne","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/felyne.png",0),
          new Monster(10, "Veggie Elder","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(11, "Rathalos","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/rathalos.png",0,true),
          new Monster(12, "Aptonoth","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/aptonoth.png",0),
          new Monster(13, "Genprey","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/genprey.png",0),
          new Monster(14, "Diablos","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/diablos.png",0,true),
          new Monster(15, "Khezu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/khezu.png",0,true),
          new Monster(16, "Velociprey","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/velociprey.png",0),
          new Monster(17, "Gravios","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gravios.png",0,true),
          new Monster(18, "Felyne?","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/felyne.png",0),
          new Monster(19, "Vespoid","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/vespoid.png",0),
          new Monster(20, "Gypceros","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gypceros.png",0,true),
          new Monster(21, "Plesioth","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/plesioth.png",0,true),
          new Monster(22, "Basarios","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/basarios.png",0,true),
          new Monster(23, "Melynx","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/melynx.png",0),
          new Monster(24, "Hornetaur","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/hornetaur.png",0),
          new Monster(25, "Apceros","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/apceros.png",0),
          new Monster(26, "Monoblos","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/monoblos.png",0,true),
          new Monster(27, "Velocidrome","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/velocidrome.png",0,true),
          new Monster(28, "Gendrome","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gendrome.png",0,true),
          new Monster(29, "Rocks","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(30, "Ioprey","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/ioprey.png",0),
          new Monster(31, "Iodrome","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/iodrome.png",0,true),
          new Monster(32, "Pugis","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(33, "Kirin","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/kirin.png",0,true),
          new Monster(34, "Cephalos","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/cephalos.png",0),
          new Monster(35, "Giaprey / Giadrome","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/giaprey.png",0),
          new Monster(36, "Crimson Fatalis","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/crimson_fatalis.png",0,true),
          new Monster(37, "Pink Rathian","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/pink_rathian.png",0,true),
          new Monster(38, "Blue Yian Kut-Ku","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/blue_yian_kut-ku.png",0,true),
          new Monster(39, "Purple Gypceros","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/purple_gypceros.png",0,true),
          new Monster(40, "Yian Garuga","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/yian_garuga.png",0,true),
          new Monster(41, "Silver Rathalos","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/silver_rathalos.png",0,true),
          new Monster(42, "Gold Rathian","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gold_rathian.png",0,true),
          new Monster(43, "Black Diablos","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/black_diablos.png",0,true),
          new Monster(44, "White Monoblos","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/white_monoblos.png",0,true),
          new Monster(45, "Red Khezu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/red_khezu.png",0,true),
          new Monster(46, "Green Plesioth","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/green_plesioth.png",0,true),
          new Monster(47, "Black Gravios","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/black_gravios.png",0,true),
          new Monster(48, "Daimyo Hermitaur","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/daimyo_hermitaur.png",0,true),
          new Monster(49, "Azure Rathalos","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/azure_rathalos.png",0,true),
          new Monster(50, "Ashen Lao-Shan Lung","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/ashen_lao-shan_lung.png",0,true),
          new Monster(51, "Blangonga","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/blangonga.png",0,true),
          new Monster(52, "Congalala","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/congalala.png",0,true),
          new Monster(53, "Rajang","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/rajang.png",0,true),
          new Monster(54, "Kushala Daora","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/kushala_daora.png",0,true),
          new Monster(55, "Shen Gaoren","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/shen_gaoren.png",0,true),
          new Monster(56, "Great Thunderbug","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/great_thunderbug.png",0),
          new Monster(57, "Shakalaka","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/shakalaka.png",0),
          new Monster(58, "Yama Tsukami","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/yama_tsukami.png",0,true),
          new Monster(59, "Chameleos","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/chameleos.png",0,true),
          new Monster(60, "Rusted Kushala Daora","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/rusted_kushala_daora.png",0,true),
          new Monster(61, "Blango","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/blango.png",0),
          new Monster(62, "Conga","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/conga.png",0),
          new Monster(63, "Remobra","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/remobra.png",0),
          new Monster(64, "Lunastra","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/lunastra.png",0,true),
          new Monster(65, "Teostra","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/teostra.png",0,true),
          new Monster(66, "Hermitaur","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/hermitaur.png",0),
          new Monster(67, "Shogun Ceanataur","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/shogun_ceanataur.png",0,true),
          new Monster(68, "Bulldrome","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/bulldrome.png",0,true),
          new Monster(69, "Anteka","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/anteka.png",0),
          new Monster(70, "Popo","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/popo.png",0),
          new Monster(71, "White Fatalis","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/white_fatalis.png",0,true),
          new Monster(72, "Yama Tsukami","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/yama_tsukami.png",0,true),
          new Monster(73, "Ceanataur","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/ceanataur.png",0),
          new Monster(74, "Hypnocatrice","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/hypnocatrice.png",0,true),
          new Monster(75, "Lavasioth","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/lavasioth.png",0,true),
          new Monster(76, "Tigrex","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/tigrex.png",0,true),
          new Monster(77, "Akantor","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/akantor.png",0,true),
          new Monster(78, "Bright Hypnoc","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/bright_hypnoc.png",0,true),
          new Monster(79, "Red Lavasioth","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/red_lavasioth.png",0,true),
          new Monster(80, "Espinas","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/espinas.png",0,true),
          new Monster(81, "Orange Espinas","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/orange_espinas.png",0,true),
          new Monster(82, "Silver Hypnoc","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/silver_hypnoc.png",0,true),
          new Monster(83, "Akura Vashimu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/akura_vashimu.png",0,true),
          new Monster(84, "Akura Jebia","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/akura_jebia.png",0,true),
          new Monster(85, "Berukyurosu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/berukyurosu.png",0,true),
          new Monster(86, "Cactus","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/cactus.png",0),
          new Monster(87, "Gorge Objects","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(88, "Gorge Rocks","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(89, "Pariapuria","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/pariapuria.png",0,true),
          new Monster(90, "White Espinas","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/white_espinas.png",0,true),
          new Monster(91, "Kamu Orugaron","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/kamu_orugaron.png",0,true),
          new Monster(92, "Nono Orugaron","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/nono_orugaron.png",0,true),
          new Monster(93, "Raviente","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/raviente.png",0,true),
          new Monster(94, "Dyuragaua","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/dyuragaua.png",0,true),
          new Monster(95, "Doragyurosu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/doragyurosu.png",0,true),
          new Monster(96, "Gurenzeburu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gurenzeburu.png",0,true),
          new Monster(97, "Burukku","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/burukku.png",0),
          new Monster(98, "Erupe","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/erupe.png",0),
          new Monster(99, "Rukodiora","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/rukodiora.png",0,true),
          new Monster(100, "UNKNOWN","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/unknown.png",0,true),
          new Monster(101, "Gogomoa","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gogomoa.png",0,true),
          new Monster(102, "Kokomoa","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gogomoa.png",0),
          new Monster(103, "Taikun Zamuza","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/taikun_zamuza.png",0,true),
          new Monster(104, "Abiorugu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/abiorugu.png",0,true),
          new Monster(105, "Kuarusepusu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/kuarusepusu.png",0,true),
          new Monster(106, "Odibatorasu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/odibatorasu.png",0,true),
          new Monster(107, "Disufiroa","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/disufiroa.png",0,true),
          new Monster(108, "Rebidiora","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/rebidiora.png",0,true),
          new Monster(109, "Anorupatisu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/anorupatisu.png",0,true),
          new Monster(110, "Hyujikiki","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/hyujikiki.png",0,true),
          new Monster(111, "Midogaron","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/midogaron.png",0,true),
          new Monster(112, "Giaorugu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/giaorugu.png",0,true),
          new Monster(113, "Mi Ru","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/mi_ru.png",0,true),
          new Monster(114, "Farunokku","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/farunokku.png",0,true),
          new Monster(115, "Pokaradon","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/pokaradon.png",0,true),
          new Monster(116, "Shantien","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/shantien.png",0,true),
          new Monster(117, "Pokara","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/pokara.png",0),
          new Monster(118, "Dummy","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(119, "Goruganosu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/goruganosu.png",0,true),
          new Monster(120, "Aruganosu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/aruganosu.png",0,true),
          new Monster(121, "Baruragaru","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/baruragaru.png",0,true),
          new Monster(122, "Zerureusu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/zerureusu.png",0,true),
          new Monster(123, "Gougarf","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gougarf.png",0,true),
          new Monster(124, "Uruki","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/uruki.png",0),
          new Monster(125, "Forokururu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/forokururu.png",0,true),
          new Monster(126, "Meraginasu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/meraginasu.png",0,true),
          new Monster(127, "Diorex","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/diorex.png",0,true),
          new Monster(128, "Garuba Daora","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/garuba_daora.png",0,true),
          new Monster(129, "Inagami","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/inagami.png",0,true),
          new Monster(130, "Varusaburosu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/varusaburosu.png",0,true),
          new Monster(131, "Poborubarumu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/poborubarumu.png",0,true),
          new Monster(132, "1st District Duremudira","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/duremudira.png",0,true),
          new Monster(133, "UNK","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(134, "Felyne","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/felyne.png",0),
          new Monster(135, "Blue NPC","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(136, "UNK","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(137, "Cactus","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/cactus.png",0),
          new Monster(138, "Veggie Elders","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(139, "Gureadomosu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gureadomosu.png",0,true),
          new Monster(140, "Harudomerugu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/harudomerugu.png",0,true),
          new Monster(141, "Toridcless","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/toridcless.png",0,true),
          new Monster(142, "Gasurabazura","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gasurabazura.png",0,true),
          new Monster(143, "Kusubami","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/kusubami.png",0),
          new Monster(144, "Yama Kurai","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/yama_kurai.png",0,true),
          new Monster(145, "2nd District Duremudira","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/duremudira.png",0,true),
          new Monster(146, "Zinogre","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/zinogre.png",0,true),
          new Monster(147, "Deviljho","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/deviljho.png",0,true),
          new Monster(148, "Brachydios","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/brachydios.png",0,true),
          new Monster(149, "Berserk Raviente","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/berserk_raviente.png",0,true),
          new Monster(150, "Toa Tesukatora","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/toa_tesukatora.png",0,true),
          new Monster(151, "Barioth","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/barioth.png",0,true),
          new Monster(152, "Uragaan","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/uragaan.png",0,true),
          new Monster(153, "Stygian Zinogre","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/stygian_zinogre.png",0,true),
          new Monster(154, "Guanzorumu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/guanzorumu.png",0,true),
          new Monster(155, "Starving Deviljho","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/starving_deviljho.png",0,true),
          new Monster(156, "UNK","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(157, "Egyurasu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(158, "Voljang","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/voljang.png",0,true),
          new Monster(159, "Nargacuga","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/nargacuga.png",0,true),
          new Monster(160, "Keoaruboru","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/keoaruboru.png",0,true),
          new Monster(161, "Zenaserisu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/zenaserisu.png",0,true),
          new Monster(162, "Gore Magala","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/gore_magala.png",0,true),
          new Monster(163, "Blinking Nargacuga","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/blinking_nargacuga.png",0,true),
          new Monster(164, "Shagaru Magala","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/shagaru_magala.png",0,true),
          new Monster(165, "Amatsu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/amatsu.png",0,true),
          new Monster(166, "Elzelion","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/elzelion.png",0,true),
          new Monster(167, "Arrogant Duremudira","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/arrogant_duremudira.png",0,true),
          new Monster(168, "Rocks","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(169, "Seregios","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/seregios.png",0,true),
          new Monster(170, "Bogabadorumu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/bogabadorumu.png",0,true),
          new Monster(171, "Unknown Blue Barrel","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/random.png",0),
          new Monster(172, "Blitzkrieg Bogabadorumu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/blitzkrieg_bogabadorumu.png",0,true),
          new Monster(173, "Costumed Uruki","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/uruki.png",0),
          new Monster(174, "Sparkling Zerureusu","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/sparkling_zerureusu.png",0,true),
          new Monster(175, "PSO2 Rappy","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/pso2_rappy.png",0),
          new Monster(176, "King Shakalaka","https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/king_shakalaka.png",0,true)
        };

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

            //https://stackoverflow.com/questions/30839173/change-background-image-in-wpf-using-c-sharp
            GeneralContent.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/MHFZ_Overlay;component/Background/1.png")));
            PlayerContent.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/MHFZ_Overlay;component/Background/2.png")));
            MonsterHPContent.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/MHFZ_Overlay;component/Background/3.png")));
            MonsterStatusContent.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/MHFZ_Overlay;component/Background/4.png")));
            DiscordRPCContent.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/MHFZ_Overlay;component/Background/5.png")));
            CreditsContent.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/MHFZ_Overlay;component/Background/6.png")));
            MonsterInfoContent.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/MHFZ_Overlay;component/Background/7.png")));
            //QuestInfoContent.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/MHFZ_Overlay;component/Background/8.png")));

            //GlobalHotKey.RegisterHotKey("Alt+Shift+a", () => SaveKey_Press());
            //GlobalHotKey.RegisterHotKey("Alt+Shift+b", () => CancelKey_Press());
            //GlobalHotKey.RegisterHotKey("Alt+Shift+c", () => DefaultKey_Press());

            //todo: test this
            DataContext = MainWindow.DataLoader.model;
            //this.DataContext = this;
            //MyTitle = FullCurrentProgramVersion();

            for (int i = 0; i < Monsters.Length; i++)
            {
                Monsters[i].Hunted = GetHuntedCount(Monsters[i].ID);
                //Monsters[i].Image = "icons/armor_skills.png";
            }

            MyList.ItemsSource = Monsters;
            SortBy.ItemsSource = new string[] { "ID", "Name", "Hunted" };
            SortDir.ItemsSource = Enum.GetNames<ListSortDirection>();

            SortBy.SelectionChanged += SelectionChanged;
            SortDir.SelectionChanged += SelectionChanged;

            MyList.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            //Monsters[1].Hunted = GetHuntedCount(Monsters[1].ID);


            //var view = CollectionViewSource.GetDefaultView(MyList.Items);
            //view.Filter = i => ((MyType)i).IsDeleted != 1;
            //MyListView.DataSource = view;

            //FilterBox.ItemsSource = typeof(Monster).GetProperties().Select((o) => o.Name);
            FilterBox.ItemsSource = new string[] { "All", "Large Monster", "Small Monster" };

            MyList.Items.Filter = MonsterFilterAll;

            //// See: https://stackoverflow.com/questions/22285866/why-relaycommand
            //// Or use MVVM Light to obtain RelayCommand.
            //this.ScreenShotCommand = new RelayCommand<FrameworkElement>(this.OnScreenShotCommandAsync);
            //webViewFerias.Style = 


        }

        private bool MonsterFilterAll(object obj)
        {
            var FilterObj = obj as Monster;
            return FilterObj.IsLarge == true || FilterObj.IsLarge == false;
        }

        private bool MonsterFilterLarge(object obj)
        {
            var FilterObj = obj as Monster;
            return FilterObj.IsLarge == true;
        }

        private bool MonsterFilterSmall(object obj)
        {
            var FilterObj = obj as Monster;
            return FilterObj.IsLarge == false;
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

        //public bool IsLargeMonster(int id)
        //{
        //    switch (id)
        //    {
        //        default: return false;
        //            case 0: return true;
        //            case 1: return true;
        //        case 2: return true;
        //        case 3: return true;
        //        case 4: return true;
        //        case 5: return true;
        //        case 6: return true;
        //        case 7: return true;
        //        case 8: return true;
        //    }
        //}

        //public bool MonsterFilter(object item)
        //{
        //    var view = CollectionViewSource.GetDefaultView(MyList.Items);
        //    //var monsters = (Monster)item;
        //    Settings s = (Settings)Application.Current.TryFindResource("Settings");

        //    if (s.HuntedMonsterFilter == "All")
        //        view.Filter = i => ((Monster)i).IsLarge == true || ((Monster)i).IsLarge == false;
        //    else if (s.HuntedMonsterFilter == "Large Monster")
        //        view.Filter = i => ((Monster)i).IsLarge == true;
        //    else if (s.HuntedMonsterFilter == "Small Monster")
        //        view.Filter = i => ((Monster)i).IsLarge == false;

        //    MyList.View = (ViewBase)view;

        //    return (s.)
        //}

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
                return;
            }

        }

        //private void RoadOverrideTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        //{
        //    if (RoadOverrideTextBox.Text.Length > 1)
        //    {
        //        RoadOverrideTextBox.Text = RoadOverrideTextBox.Text.Remove(0, 1);
        //        RoadOverrideTextBox.CaretIndex = 1;
        //    }
        //}

        /// <summary>
        /// Saves the key press.
        /// </summary>
        public void SaveKey_Press()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Save();
            webViewFerias.Dispose();
            webView.Dispose();
            Close();
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
            webViewFerias.Dispose();
            webView.Dispose();
            Close();
        }

        /// <summary>
        /// Cancels the key press.
        /// </summary>
        public void CancelKey_Press()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Reload();
            webViewFerias.Dispose();
            webView.Dispose();
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
            webViewFerias.Dispose();
            webView.Dispose();
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
            webViewFerias.Dispose();
            webView.Dispose();
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
            webViewFerias.Dispose();
            webView.Dispose();
            s.Reset();
        }

        /// <summary>
        /// Handles the Click event of the ConfigureButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ConfigureButton_Click(object sender, RoutedEventArgs e)
        {
            //webViewFerias.Dispose();
            //webView.Dispose();
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

        //private string ValidateNamePattern = @"[^a-zA-Z_0-9]";

        /// <summary>
        /// Validates the name.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        private void ValidateName(object sender, TextCompositionEventArgs e)
        {
            // Create a Regex  
            //Regex rg = new Regex(ValidateNamePattern);

            // Get all matches  
            //MatchCollection matchedText = rg.Matches(e.Text);
            //https://stackoverflow.com/questions/1046740/how-can-i-validate-a-string-to-only-allow-alphanumeric-characters-in-it
            if (!(e.Text.All(char.IsLetterOrDigit)))
            {
                //just letters and digits.
                e.Handled = true;
            }

            //if (matchedText.Count == 0 && e.Text.Length >= 12)
             //   e.Handled = true;
        }

        //private void ValidateDiscordInvite(object sender, TextCompositionEventArgs e)
        //{
        //    if (!(e.Text.Substring(0,27) == "https://discord.com/invite/") )
        //        e.Handled = true;
        //}

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
        public string GetTextFormatMode()
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
            //else if (GetTextFormatMode() == "Image")
            //{
            //    CopyUIElementToClipboard(GearTextGrid);
            //    return;
            //}

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Markdown file (*.md)|*.md|Text file (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory+@"USERDATA\HunterSets\";
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
            //System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Overwrite current file?", "Gear Stats", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Asterisk, MessageBoxResult.No); if (messageBoxResult.ToString() == "No") { return; }

            SaveFileDialog savefile = new SaveFileDialog();
            string dateTime = DateTime.Now.ToString();
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            savefile.FileName = "HuntedLog-" + dateTime + ".png";
            savefile.Filter = "PNG files (*.png)|*.png";
            savefile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory + @"USERDATA\HunterSets\";

            if (savefile.ShowDialog() == true)
            {
                GearImageGrid.Background = new SolidColorBrush(Color.FromArgb(0x00,0x1E,0x1E,0x2E));
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
            //System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Overwrite current file?", "Gear Stats", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Asterisk, MessageBoxResult.No); if (messageBoxResult.ToString() == "No") { return; }

            SaveFileDialog savefile = new SaveFileDialog();
            string dateTime = DateTime.Now.ToString();
            dateTime = dateTime.Replace("/", "-");
            dateTime = dateTime.Replace(" ", "_");
            dateTime = dateTime.Replace(":", "-");
            savefile.FileName = "HuntedLog-"+dateTime+".csv";
            savefile.Filter = "CSV files (*.csv)|*.csv";
            savefile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory + @"USERDATA\HuntedLogs\";

            //https://stackoverflow.com/questions/11776781/savefiledialog-make-problems-with-streamwriter-in-c-sharp
            if (savefile.ShowDialog() == true)
            {
                //using (StreamWriter sw = new StreamWriter(savefile.FileName,
                //          false, System.Text.Encoding.Unicode))
                //{
                //    sw.WriteLine("Test line");
                //    sw.WriteLine("Test line2");
                //    sw.WriteLine("Test line3");
                //}
                using (var writer = new StreamWriter(savefile.FileName))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(Monsters);
                }
            }
        }


        private void Config_Closed(object sender, EventArgs e)
        {
            webViewFerias.Dispose();
            webView.Dispose();
        }

        private void Config_Closing(object sender, EventArgs e)
        {
            return;
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
