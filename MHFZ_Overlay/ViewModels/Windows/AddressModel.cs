// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.ViewModels.Windows;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Security.RightsManagement;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using EZlion.Mapper;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Memory;
using MHFZ_Overlay;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.Services.Converter;
using NLog;
using RESTCountries.NET.Models;
using RESTCountries.NET.Services;
using SkiaSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Application = System.Windows.Application;

/// <summary>
/// Abstract class that defines the base properties and methods that are shared between AddressModelNotHGE and AddressModelHGE classes. It has a Mem object, which is used to read and write data from the game's memory. It also has properties that represent the selected monster, the hit count value, and the monster's health points.
/// </summary>
/// <seealso cref="INotifyPropertyChanged" />
public abstract class AddressModel : INotifyPropertyChanged
{
    public readonly Mem M;

    private static readonly NLog.Logger LoggerInstance = NLog.LogManager.GetCurrentClassLogger();

    private static readonly DatabaseService DatabaseManagerInstance = DatabaseService.GetInstance();

    public ObservableCollection<ObservablePoint> AttackBuffCollection { get; set; } = new();

    private readonly Mem m = new();

    private int savedMonster1MaxHP;

    private int savedMonster2MaxHP;

    private int savedMonster3MaxHP;

    private int savedMonster4MaxHP;

    private int savedMonster1ID;

    private int savedMonster2ID;

    private bool configuring;

    private int maxSharpness;

    public AddressModel(Mem m)
    {
        LoggerInstance.Info(CultureInfo.InvariantCulture, $"AddressModel initialized");
        LoggerInstance.Trace(new StackTrace().ToString());
        this.M = m;
    }

    public static string FullCurrentProgramVersion => string.Format(CultureInfo.InvariantCulture, "Monster Hunter Frontier Z Overlay {0}", Program.CurrentProgramVersion);

    public DateTime DateTimeUtcNow { get; set; } = DateTime.UtcNow;

    public int SelectedMonster { get; set; }

    public bool ShowMonsterAtkMult { get; set; }

    public bool ShowMonsterDefrate { get; set; }

    public bool ShowMonsterSize { get; set; }

    public bool ShowMonsterPoison { get; set; }

    public bool ShowMonsterSleep { get; set; }

    public bool ShowMonsterPara { get; set; }

    public bool ShowMonsterBlast { get; set; }

    public bool ShowMonsterStun { get; set; }

    public bool ShowPersonalBestInfo { get; set; }

    public bool ShowTimerInfo { get; set; }

    public bool ShowHitCountInfo { get; set; }

    public bool ShowPlayerAtkInfo { get; set; }

    public bool ShowPlayerHitsTakenBlockedInfo { get; set; }

    public bool ShowQuestID { get; set; }

    public bool ShowQuestAttemptsInfo { get; set; }

    public bool ShowPersonalBestTimePercentInfo { get; set; }

    public bool ShowPersonalBestAttemptsInfo { get; set; }

    public bool ShowDualSwordsSharpens { get; set; }

    public bool ShowMonster1HPBar { get; set; }

    public bool ShowMonster2HPBar { get; set; }

    public bool ShowMonster3HPBar { get; set; }

    public bool ShowMonster4HPBar { get; set; }

    public bool ShowSharpness { get; set; }

    public bool ShowSessionTimeInfo { get; set; }

    public bool ShowPlayerPositionInfo { get; set; }

    public bool ShowDivaSongTimer { get; set; }

    public bool ShowGuildFoodTimer { get; set; }

    public bool ShowMonsterPartHP { get; set; }

    public bool ShowKBMLayout { get; set; }

    public bool ShowAPM { get; set; }

    public bool ShowGamepadLayout { get; set; }

    public bool ShowMonster1Icon { get; set; }

    public bool ShowFrameCounter { get; set; }

    public bool ShowMap { get; set; }

    public bool ShowDamagePerSecond { get; set; }

    public bool ShowOverlayModeWatermark { get; set; }

    public bool ShowSaveIcon { get; set; } = true;

    public bool ShowLocationTextInfo { get; set; } = true;

    public bool ShowQuestNameInfo { get; set; } = true;

    public bool ShowPlayerAttackGraph { get; set; } = true;

    public bool ShowPlayerDPSGraph { get; set; } = true;

    public bool ShowPlayerAPMGraph { get; set; } = true;

    public bool ShowPlayerHitsPerSecondGraph { get; set; } = true;

    public bool HasMonster1
    {
        get
        {
            // quest ids:
            // mp road: 23527
            // solo road: 23628
            // 1st district dure: 21731
            // 2nd district dure: 21746
            // 1st district dure sky corridor: 21749
            // 2nd district dure sky corridor: 21750
            // arrogant dure repel: 23648
            // arrogant dure slay: 23649
            // urgent tower: 21751
            // 4th district dure: 21748
            // 3rd district dure: 21747
            // 3rd district dure 2: 21734
            // UNUSED sky corridor: 21730
            // sky corridor prologue: 21729
            if (this.QuestID() is 23527 or
                23628 or
                21731 or
                21746 or
                21749 or
                21750 or
                23648 or
                23649 or
                21751 or
                21748 or
                21747 or
                21734)
            {
                return true;
            }

            if (this.AlternativeQuestOverride())
            {
                return this.ShowHPBar(this.AlternativeQuestMonster1ID(), this.Monster1HPInt());
            }
            else
            {
                return this.ShowHPBar(this.LargeMonster1ID(), this.Monster1HPInt());
            }
        }
    }

    public abstract bool IsNotRoad();

    public abstract int HitCountInt();

    public abstract int DamageDealt();

    // New addresses
    public abstract int AreaID();

    public abstract int GRankNumber();

    public abstract int GSR();

    public abstract int RoadFloor();

    public abstract int WeaponStyle();

    public abstract int QuestID();

    public abstract int UrukiPachinkoFish();

    public abstract int UrukiPachinkoMushroom();

    public abstract int UrukiPachinkoSeed();

    public abstract int UrukiPachinkoMeat();

    public abstract int UrukiPachinkoChain();

    public abstract int UrukiPachinkoScore();

    public abstract int UrukiPachinkoBonusScore();

    public abstract int NyanrendoScore();

    public abstract int DokkanBattleCatsScore();

    public abstract int DokkanBattleCatsScale();

    public abstract int DokkanBattleCatsShell();

    public abstract int DokkanBattleCatsCamp();

    public abstract int GuukuScoopSmall();

    public abstract int GuukuScoopMedium();

    public abstract int GuukuScoopLarge();

    public abstract int GuukuScoopGolden();

    public abstract int GuukuScoopScore();

    public abstract int PanicHoneyScore();

    public abstract int Sharpness();

    public abstract int CaravanPoints();

    public abstract int MezeportaFestivalPoints();

    public abstract int DivaBond();

    public abstract int DivaItemsGiven();

    public abstract int GCP();

    public abstract int RoadPoints();

    public abstract int ArmorColor();

    public abstract int RaviGg();

    public abstract int Ravig();

    public abstract int GZenny();

    public abstract int GuildFoodSkill();

    public abstract int GalleryEvaluationScore();

    public abstract int PoogiePoints();

    public abstract int PoogieItemUseID();

    public abstract int PoogieCostume();

    // zero-indexed
    public abstract int CaravenGemLevel();

    public abstract int RoadMaxStagesMultiplayer();

    public abstract int RoadTotalStagesMultiplayer();

    public abstract int RoadTotalStagesSolo();

    public abstract int RoadMaxStagesSolo();

    public abstract int RoadFatalisSlain();

    public abstract int RoadFatalisEncounters();

    public abstract int FirstDistrictDuremudiraEncounters();

    public abstract int FirstDistrictDuremudiraSlays();

    public abstract int SecondDistrictDuremudiraEncounters();

    public abstract int SecondDistrictDuremudiraSlays();

    public abstract int DeliveryQuestPoints(); // doesn't seem to work

    // red is 0
    public abstract int SharpnessLevel();

    public abstract int PartnerLevel();

    public abstract int ObjectiveType();

    public abstract int DivaSkillUsesLeft();

    public abstract int HalkFullness();

    public abstract int HalkLevel();

    public abstract int HalkIntimacy();

    public abstract int HalkHealth();

    public abstract int HalkAttack();

    public abstract int HalkDefense();

    public abstract int HalkIntellect();

    public abstract int HalkSkill1();

    public abstract int HalkSkill2();

    public abstract int HalkSkill3();

    public abstract int HalkElementNone();

    public abstract int HalkFire();

    public abstract int HalkThunder();

    public abstract int HalkWater();

    public abstract int HalkIce();

    public abstract int HalkDragon();

    public abstract int HalkSleep();

    public abstract int HalkParalysis();

    public abstract int HalkPoison();

    public abstract int RankBand();

    public abstract int PartnyaRankPoints();

    // parts
    public abstract int Monster1Part1();

    public abstract int Monster1Part2();

    public abstract int Monster1Part3();

    public abstract int Monster1Part4();

    public abstract int Monster1Part5();

    public abstract int Monster1Part6();

    public abstract int Monster1Part7();

    public abstract int Monster1Part8();

    public abstract int Monster1Part9();

    public abstract int Monster1Part10();

    public abstract int Monster2Part1();

    public abstract int Monster2Part2();

    public abstract int Monster2Part3();

    public abstract int Monster2Part4();

    public abstract int Monster2Part5();

    public abstract int Monster2Part6();

    public abstract int Monster2Part7();

    public abstract int Monster2Part8();

    public abstract int Monster2Part9();

    public abstract int Monster2Part10();

    public abstract int TimeInt();

    public abstract int TimeDefInt();

    public abstract int WeaponRaw();

    public abstract int WeaponType();

    public abstract int LargeMonster1ID();

    public abstract int LargeMonster2ID();

    public abstract int LargeMonster3ID();

    public abstract int LargeMonster4ID();

    public abstract int Monster1HPInt();

    public abstract int Monster2HPInt();

    public abstract int Monster3HPInt();

    public abstract int Monster4HPInt();

    public abstract string Monster1AtkMult();

    public abstract string Monster2AtkMult();

    public abstract decimal Monster1DefMult();

    public abstract decimal Monster2DefMult();

    public abstract int Monster1Poison();

    public abstract int Monster1PoisonNeed();

    public abstract int Monster1Sleep();

    public abstract int Monster1SleepNeed();

    public abstract int Monster1Para();

    public abstract int Monster1ParaNeed();

    public abstract int Monster1Blast();

    public abstract int Monster1BlastNeed();

    public abstract int Monster1Stun();

    public abstract int Monster1StunNeed();

    public abstract string Monster1Size();

    public abstract int Monster2Poison();

    public abstract int Monster2PoisonNeed();

    public abstract int Monster2Sleep();

    public abstract int Monster2SleepNeed();

    public abstract int Monster2Para();

    public abstract int Monster2ParaNeed();

    public abstract int Monster2Blast();

    public abstract int Monster2BlastNeed();

    public abstract int Monster2Stun();

    public abstract int Monster2StunNeed();

    public abstract string Monster2Size();

    public abstract int Objective1ID();

    public abstract int Objective1Quantity();

    public abstract int Objective1CurrentQuantityMonster();

    public abstract int Objective1CurrentQuantityItem();

    // ravi
    public abstract int RavienteTriggeredEvent();

    public abstract int GreatSlayingPoints();

    public abstract int GreatSlayingPointsSaved();

    // normal and violent. berserk support
    public abstract int RavienteAreaID();

    public abstract int RoadSelectedMonster();

    // TODO Yamas and Beru
    public abstract int AlternativeMonster1HPInt();

    public abstract int AlternativeMonster1AtkMult();

    public abstract int AlternativeMonster1DefMult();

    public abstract int AlternativeMonster1Size();

    public abstract int AlternativeMonster1Poison();

    public abstract int AlternativeMonster1PoisonNeed();

    public abstract int AlternativeMonster1Sleep();

    public abstract int AlternativeMonster1SleepNeed();

    public abstract int AlternativeMonster1Para();

    public abstract int AlternativeMonster1ParaNeed();

    public abstract int AlternativeMonster1Blast();

    public abstract int AlternativeMonster1BlastNeed();

    public abstract int AlternativeMonster1Stun();

    public abstract int AlternativeMonster1StunNeed();

    public abstract int AlternativeMonster1Part1();

    public abstract int AlternativeMonster1Part2();

    public abstract int AlternativeMonster1Part3();

    public abstract int AlternativeMonster1Part4();

    public abstract int AlternativeMonster1Part5();

    public abstract int AlternativeMonster1Part6();

    public abstract int AlternativeMonster1Part7();

    public abstract int AlternativeMonster1Part8();

    public abstract int AlternativeMonster1Part9();

    public abstract int AlternativeMonster1Part10();

    public abstract int DivaSkill();

    public abstract int StarGrades();

    public abstract int CaravanSkill1();

    public abstract int CaravanSkill2();

    public abstract int CaravanSkill3();

    public abstract int CurrentFaints();

    // road and normal
    public abstract int MaxFaints();

    // shitens, conquests, pioneer, daily, caravan, interception
    public abstract int AlternativeMaxFaints();

    public abstract int CaravanScore();

    public abstract int AlternativeQuestMonster1ID();

    // TODO unsure
    public abstract int AlternativeQuestMonster2ID();

    public abstract int BlademasterWeaponID();

    // same as melee afaik
    public abstract int GunnerWeaponID();

    public abstract int WeaponDeco1ID();

    public abstract int WeaponDeco2ID();

    public abstract int WeaponDeco3ID();

    public abstract int ArmorHeadID();

    public abstract int ArmorHeadDeco1ID();

    public abstract int ArmorHeadDeco2ID();

    public abstract int ArmorHeadDeco3ID();

    public abstract int ArmorChestID();

    public abstract int ArmorChestDeco1ID();

    public abstract int ArmorChestDeco2ID();

    public abstract int ArmorChestDeco3ID();

    public abstract int ArmorArmsID();

    public abstract int ArmorArmsDeco1ID();

    public abstract int ArmorArmsDeco2ID();

    public abstract int ArmorArmsDeco3ID();

    public abstract int ArmorWaistID();

    public abstract int ArmorWaistDeco1ID();

    public abstract int ArmorWaistDeco2ID();

    public abstract int ArmorWaistDeco3ID();

    public abstract int ArmorLegsID();

    public abstract int ArmorLegsDeco1ID();

    public abstract int ArmorLegsDeco2ID();

    public abstract int ArmorLegsDeco3ID();

    public abstract int Cuff1ID();

    public abstract int Cuff2ID();

    public abstract int TotalDefense();

    public abstract int PouchItem1ID();

    public abstract int PouchItem1Qty();

    public abstract int PouchItem2ID();

    public abstract int PouchItem2Qty();

    public abstract int PouchItem3ID();

    public abstract int PouchItem3Qty();

    public abstract int PouchItem4ID();

    public abstract int PouchItem4Qty();

    public abstract int PouchItem5ID();

    public abstract int PouchItem5Qty();

    public abstract int PouchItem6ID();

    public abstract int PouchItem6Qty();

    public abstract int PouchItem7ID();

    public abstract int PouchItem7Qty();

    public abstract int PouchItem8ID();

    public abstract int PouchItem8Qty();

    public abstract int PouchItem9ID();

    public abstract int PouchItem9Qty();

    public abstract int PouchItem10ID();

    public abstract int PouchItem10Qty();

    public abstract int PouchItem11ID();

    public abstract int PouchItem11Qty();

    public abstract int PouchItem12ID();

    public abstract int PouchItem12Qty();

    public abstract int PouchItem13ID();

    public abstract int PouchItem13Qty();

    public abstract int PouchItem14ID();

    public abstract int PouchItem14Qty();

    public abstract int PouchItem15ID();

    public abstract int PouchItem15Qty();

    public abstract int PouchItem16ID();

    public abstract int PouchItem16Qty();

    public abstract int PouchItem17ID();

    public abstract int PouchItem17Qty();

    public abstract int PouchItem18ID();

    public abstract int PouchItem18Qty();

    public abstract int PouchItem19ID();

    public abstract int PouchItem19Qty();

    public abstract int PouchItem20ID();

    public abstract int PouchItem20Qty();

    public abstract int AmmoPouchItem1ID();

    public abstract int AmmoPouchItem1Qty();

    public abstract int AmmoPouchItem2ID();

    public abstract int AmmoPouchItem2Qty();

    public abstract int AmmoPouchItem3ID();

    public abstract int AmmoPouchItem3Qty();

    public abstract int AmmoPouchItem4ID();

    public abstract int AmmoPouchItem4Qty();

    public abstract int AmmoPouchItem5ID();

    public abstract int AmmoPouchItem5Qty();

    public abstract int AmmoPouchItem6ID();

    public abstract int AmmoPouchItem6Qty();

    public abstract int AmmoPouchItem7ID();

    public abstract int AmmoPouchItem7Qty();

    public abstract int AmmoPouchItem8ID();

    public abstract int AmmoPouchItem8Qty();

    public abstract int AmmoPouchItem9ID();

    public abstract int AmmoPouchItem9Qty();

    public abstract int AmmoPouchItem10ID();

    public abstract int AmmoPouchItem10Qty();

    public abstract int ArmorSkill1();

    public abstract int ArmorSkill2();

    public abstract int ArmorSkill3();

    public abstract int ArmorSkill4();

    public abstract int ArmorSkill5();

    public abstract int ArmorSkill6();

    public abstract int ArmorSkill7();

    public abstract int ArmorSkill8();

    public abstract int ArmorSkill9();

    public abstract int ArmorSkill10();

    public abstract int ArmorSkill11();

    public abstract int ArmorSkill12();

    public abstract int ArmorSkill13();

    public abstract int ArmorSkill14();

    public abstract int ArmorSkill15();

    public abstract int ArmorSkill16();

    public abstract int ArmorSkill17();

    public abstract int ArmorSkill18();

    public abstract int ArmorSkill19();

    public abstract int BloatedWeaponAttack();

    public abstract int ZenithSkill1();

    public abstract int ZenithSkill2();

    public abstract int ZenithSkill3();

    public abstract int ZenithSkill4();

    public abstract int ZenithSkill5();

    public abstract int ZenithSkill6();

    public abstract int ZenithSkill7();

    public abstract int AutomaticSkillWeapon();

    public abstract int AutomaticSkillHead();

    public abstract int AutomaticSkillChest();

    public abstract int AutomaticSkillArms();

    public abstract int AutomaticSkillWaist();

    public abstract int AutomaticSkillLegs();

    public abstract int StyleRank1();

    public abstract int StyleRank2();

    public abstract int GRWeaponLv();

    public abstract int GRWeaponLvBowguns();

    public abstract int Sigil1Name1();

    public abstract int Sigil1Value1();

    public abstract int Sigil1Name2();

    public abstract int Sigil1Value2();

    public abstract int Sigil1Name3();

    public abstract int Sigil1Value3();

    public abstract int Sigil2Name1();

    public abstract int Sigil2Value1();

    public abstract int Sigil2Name2();

    public abstract int Sigil2Value2();

    public abstract int Sigil2Name3();

    public abstract int Sigil2Value3();

    public abstract int Sigil3Name1();

    public abstract int Sigil3Value1();

    public abstract int Sigil3Name2();

    public abstract int Sigil3Value2();

    public abstract int Sigil3Name3();

    public abstract int Sigil3Value3();

    public abstract int FelyneHunted();

    public abstract int MelynxHunted();

    public abstract int ShakalakaHunted();

    public abstract int VespoidHunted();

    public abstract int HornetaurHunted();

    public abstract int GreatThunderbugHunted();

    public abstract int KelbiHunted();

    public abstract int MosswineHunted();

    public abstract int AntekaHunted();

    public abstract int PopoHunted();

    public abstract int AptonothHunted();

    public abstract int ApcerosHunted();

    public abstract int BurukkuHunted();

    public abstract int ErupeHunted();

    public abstract int VelocipreyHunted();

    public abstract int VelocidromeHunted();

    public abstract int GenpreyHunted();

    public abstract int GendromeHunted();

    public abstract int IopreyHunted();

    public abstract int IodromeHunted();

    public abstract int GiapreyHunted();

    public abstract int YianKutKuHunted();

    public abstract int BlueYianKutKuHunted();

    public abstract int YianGarugaHunted();

    public abstract int GypcerosHunted();

    public abstract int PurpleGypcerosHunted();

    public abstract int HypnocHunted();

    public abstract int BrightHypnocHunted();

    public abstract int SilverHypnocHunted();

    public abstract int FarunokkuHunted();

    public abstract int ForokururuHunted();

    public abstract int ToridclessHunted();

    public abstract int RemobraHunted();

    public abstract int RathianHunted();

    public abstract int PinkRathianHunted();

    public abstract int GoldRathianHunted();

    public abstract int RathalosHunted();

    public abstract int AzureRathalosHunted();

    public abstract int SilverRathalosHunted();

    public abstract int KhezuHunted();

    public abstract int RedKhezuHunted();

    public abstract int BasariosHunted();

    public abstract int GraviosHunted();

    public abstract int BlackGraviosHunted();

    public abstract int MonoblosHunted();

    public abstract int WhiteMonoblosHunted();

    public abstract int DiablosHunted();

    public abstract int BlackDiablosHunted();

    public abstract int TigrexHunted();

    public abstract int EspinasHunted();

    public abstract int OrangeEspinasHunted();

    public abstract int WhiteEspinasHunted();

    public abstract int AkantorHunted();

    public abstract int BerukyurosuHunted();

    public abstract int DoragyurosuHunted();

    public abstract int PariapuriaHunted();

    public abstract int DyuragauaHunted();

    public abstract int GurenzeburuHunted();

    public abstract int OdibatorasuHunted();

    public abstract int HyujikikiHunted();

    public abstract int AnorupatisuHunted();

    public abstract int ZerureusuHunted();

    public abstract int MeraginasuHunted();

    public abstract int DiorexHunted();

    public abstract int PoborubarumuHunted();

    public abstract int VarusaburosuHunted();

    public abstract int GureadomosuHunted();

    public abstract int BariothHunted();

    public abstract int NargacugaHunted();

    public abstract int ZenaserisuHunted();

    public abstract int SeregiosHunted();

    public abstract int BogabadorumuHunted();

    public abstract int CephalosHunted();

    public abstract int CephadromeHunted();

    public abstract int PlesiothHunted();

    public abstract int GreenPlesiothHunted();

    public abstract int VolganosHunted();

    public abstract int RedVolganosHunted();

    public abstract int HermitaurHunted();

    public abstract int DaimyoHermitaurHunted();

    public abstract int CeanataurHunted();

    public abstract int ShogunCeanataurHunted();

    public abstract int ShenGaorenHunted();

    public abstract int AkuraVashimuHunted();

    public abstract int AkuraJebiaHunted();

    public abstract int TaikunZamuzaHunted();

    public abstract int KusubamiHunted();

    public abstract int BullfangoHunted();

    public abstract int BulldromeHunted();

    public abstract int CongaHunted();

    public abstract int CongalalaHunted();

    public abstract int BlangoHunted();

    public abstract int BlangongaHunted();

    public abstract int GogomoaHunted();

    public abstract int RajangHunted();

    public abstract int KamuOrugaronHunted();

    public abstract int NonoOrugaronHunted();

    public abstract int MidogaronHunted();

    public abstract int GougarfHunted();

    public abstract int VoljangHunted();

    public abstract int KirinHunted();

    public abstract int KushalaDaoraHunted();

    public abstract int RustedKushalaDaoraHunted();

    public abstract int ChameleosHunted();

    public abstract int LunastraHunted();

    public abstract int TeostraHunted();

    public abstract int LaoShanLungHunted();

    public abstract int AshenLaoShanLungHunted();

    public abstract int YamaTsukamiHunted();

    public abstract int RukodioraHunted();

    public abstract int RebidioraHunted();

    public abstract int FatalisHunted();

    public abstract int ShantienHunted();

    public abstract int DisufiroaHunted();

    public abstract int GarubaDaoraHunted();

    public abstract int InagamiHunted();

    public abstract int HarudomeruguHunted();

    public abstract int YamaKuraiHunted();

    public abstract int ToaTesukatoraHunted();

    public abstract int GuanzorumuHunted();

    public abstract int KeoaruboruHunted();

    public abstract int ShagaruMagalaHunted();

    public abstract int ElzelionHunted();

    public abstract int AmatsuHunted();

    public abstract int AbioruguHunted();

    public abstract int GiaoruguHunted();

    public abstract int GasurabazuraHunted();

    public abstract int DeviljhoHunted();

    public abstract int BrachydiosHunted();

    public abstract int UragaanHunted();

    public abstract int KuarusepusuHunted();

    public abstract int PokaraHunted();

    public abstract int PokaradonHunted();

    public abstract int BaruragaruHunted();

    public abstract int ZinogreHunted();

    public abstract int StygianZinogreHunted();

    public abstract int GoreMagalaHunted();

    public abstract int BombardierBogabadorumuHunted();

    public abstract int SparklingZerureusuHunted();

    public abstract int StarvingDeviljhoHunted();

    public abstract int CrimsonFatalisHunted();

    public abstract int WhiteFatalisHunted();

    public abstract int CactusHunted();

    public abstract int ArrogantDuremudiraHunted(); // untested

    public abstract int MiRuHunted();

    public abstract int UnknownHunted();

    public abstract int GoruganosuHunted();

    public abstract int AruganosuHunted();

    public abstract int PSO2RappyHunted();

    public abstract int RocksHunted();

    public abstract int UrukiHunted();

    public abstract int GorgeObjectsHunted();

    public abstract int BlinkingNargacugaHunted();

    public abstract int KingShakalakaHunted();

    public abstract int QuestState();

    public abstract int RoadDureSkill1Name();

    public abstract int RoadDureSkill1Level();

    public abstract int RoadDureSkill2Name();

    public abstract int RoadDureSkill2Level();

    public abstract int RoadDureSkill3Name();

    public abstract int RoadDureSkill3Level();

    public abstract int RoadDureSkill4Name();

    public abstract int RoadDureSkill4Level();

    public abstract int RoadDureSkill5Name();

    public abstract int RoadDureSkill5Level();

    public abstract int RoadDureSkill6Name();

    public abstract int RoadDureSkill6Level();

    public abstract int RoadDureSkill7Name();

    public abstract int RoadDureSkill7Level();

    public abstract int RoadDureSkill8Name();

    public abstract int RoadDureSkill8Level();

    public abstract int RoadDureSkill9Name();

    public abstract int RoadDureSkill9Level();

    public abstract int RoadDureSkill10Name();

    public abstract int RoadDureSkill10Level();

    public abstract int RoadDureSkill11Name();

    public abstract int RoadDureSkill11Level();

    public abstract int RoadDureSkill12Name();

    public abstract int RoadDureSkill12Level();

    public abstract int RoadDureSkill13Name();

    public abstract int RoadDureSkill13Level();

    public abstract int RoadDureSkill14Name();

    public abstract int RoadDureSkill14Level();

    public abstract int RoadDureSkill15Name();

    public abstract int RoadDureSkill15Level();

    public abstract int RoadDureSkill16Name();

    public abstract int RoadDureSkill16Level();

    public abstract int PartySize();

    public abstract int PartySizeMax();

    public abstract uint GSRP();

    public abstract uint GRP();

    public abstract int HunterHP();

    public abstract int HunterStamina();

    public abstract int QuestItemsUsed();

    public abstract int AreaHitsTakenBlocked();

    public abstract int PartnyaBagItem1ID();

    public abstract int PartnyaBagItem1Qty();

    public abstract int PartnyaBagItem2ID();

    public abstract int PartnyaBagItem2Qty();

    public abstract int PartnyaBagItem3ID();

    public abstract int PartnyaBagItem3Qty();

    public abstract int PartnyaBagItem4ID();

    public abstract int PartnyaBagItem4Qty();

    public abstract int PartnyaBagItem5ID();

    public abstract int PartnyaBagItem5Qty();

    public abstract int PartnyaBagItem6ID();

    public abstract int PartnyaBagItem6Qty();

    public abstract int PartnyaBagItem7ID();

    public abstract int PartnyaBagItem7Qty();

    public abstract int PartnyaBagItem8ID();

    public abstract int PartnyaBagItem8Qty();

    public abstract int PartnyaBagItem9ID();

    public abstract int PartnyaBagItem9Qty();

    public abstract int PartnyaBagItem10ID();

    public abstract int PartnyaBagItem10Qty();

    /// <summary>
    /// Normal/HC/UL. Set at quest counter option selection.
    /// </summary>
    /// <returns></returns>
    public abstract int QuestToggleMonsterMode();

    /// <summary>
    /// Course rights. 14 is hl+ex
    /// </summary>
    /// <returns></returns>
    public abstract int Rights();

    public abstract decimal PlayerPositionX();

    public abstract decimal PlayerPositionY();

    public abstract decimal PlayerPositionZ();

    public abstract decimal PlayerPositionInQuestX();

    public abstract decimal PlayerPositionInQuestY();

    public abstract decimal PlayerPositionInQuestZ();

    public abstract int ActiveFeature1();

    /// <summary>
    /// Alternative.
    /// </summary>
    /// <returns></returns>
    public abstract int ActiveFeature2();

    /// <summary>
    /// Alternative.
    /// </summary>
    /// <returns></returns>
    public abstract int ActiveFeature3();

    public abstract int ServerHeartbeatLandAlternative();

    public abstract int ServerHeartbeatLandMain();

    public abstract int LandSlot();

    public abstract int GuildFoodStart();

    public abstract int DivaSongStart();

    public abstract int GuildPoogie1Skill();

    public abstract int GuildPoogie2Skill();

    public abstract int GuildPoogie3Skill();

    public abstract int DivaPrayerGemRedSkill();

    public abstract int DivaPrayerGemRedLevel();

    public abstract int DivaPrayerGemYellowSkill();

    public abstract int DivaPrayerGemYellowLevel();

    public abstract int DivaPrayerGemGreenSkill();

    public abstract int DivaPrayerGemGreenLevel();

    public abstract int DivaPrayerGemBlueSkill();

    public abstract int DivaPrayerGemBlueLevel();

    public abstract bool HalkOn();

    public abstract bool HalkPotEffectOn();

    public abstract int DivaSongFromGuildStart();

    public abstract int QuestVariant1();

    public abstract int QuestVariant2();

    public abstract int QuestVariant3();

    /// <summary>
    /// unused?
    /// </summary>
    /// <returns></returns>
    public abstract int QuestVariant4();

    public abstract int DualSwordsSharpens();

    /// <summary>
    /// Updates every 11 seconds
    /// </summary>
    /// <returns></returns>
    public int ServerHeartbeat => ServerHeartbeatLandMain() > ServerHeartbeatLandAlternative() ? ServerHeartbeatLandMain() : ServerHeartbeatLandAlternative();

    /// <TODO>
    /// [] Not Done
    /// [X] Done
    /// [O] WIP
    /// [] bento, 
    /// [] sharpness table, 
    /// [] pvp, 
    /// [] zenith in road, gear rarity colors.
    /// [] Database would store bento, sharpness table, pvp. Should i use separate table?
    /// </TODO>

    public bool HasMonster2
    {
        get
        {
            // road
            // TODO replace all instances of magic numbers
            if (this.QuestID() is 23527 or 23628)
            {
                return true;
            }

            if (this.AlternativeQuestOverride())
            {
                return this.ShowHPBar(this.AlternativeQuestMonster2ID(), this.Monster2HPInt()) && this.GetNotRoad();
            }
            else
            {
                // road check since the 2nd choice is used as the monster #1
                return this.ShowHPBar(this.LargeMonster2ID(), this.Monster2HPInt()) && this.GetNotRoad();
            }
        }
    }

    public bool HasMonster3 => this.ShowHPBar(this.LargeMonster3ID(), this.Monster3HPInt());

    public bool HasMonster4 => this.ShowHPBar(this.LargeMonster4ID(), this.Monster4HPInt());

    public static string SimplifiedCurrentProgramVersion => string.Format(CultureInfo.InvariantCulture, "MHF-Z Overlay {0}", Program.CurrentProgramVersion);

    public string HitCount
    {
        get
        {
            var hitsPerSecond = string.Empty;

            if (ShowHitsPerSecond())
            {
                hitsPerSecond = string.Format(CultureInfo.InvariantCulture, " ({0:0.##}/s)", this.HitsPerSecond);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", this.HitCountInt(), hitsPerSecond);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="AddressModel"/> is configuring.
    /// </summary>
    /// <value>
    ///   <c>true</c> if configuring; otherwise, <c>false</c>.
    /// </value>
    public bool Configuring
    {
        get => this.configuring;
        set
        {
            this.configuring = value;
            this.ReloadData();
        }
    }

    public static bool ShowOverlayStatText
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.OverlayStatIconShown)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public static bool ShowHitsPerSecond()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.HitsPerSecondShown;
    }

    public static string GetFullCurrentProgramVersion() => string.Format(CultureInfo.InvariantCulture, "Monster Hunter Frontier Z Overlay {0}", Program.CurrentProgramVersion);

    public static bool ShowTotalHitsTakenBlockedPerSecond()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.TotalHitsTakenBlockedPerSecondShown;
    }

    /// <summary>
    /// Shows the monster ehp.
    /// </summary>
    /// <returns></returns>
    public static bool ShowMonsterEHP()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableEHPNumbers;
    }

    public static bool IsAlwaysShowingMonsterInfo()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.AlwaysShowMonsterInfo;
    }

    /// <summary>
    /// Shows the hp bar?.
    /// </summary>
    /// <param name="monsterId">The monster identifier.</param>
    /// <param name="monsterHp">The monster hp.</param>
    /// <returns></returns>
    public bool ShowHPBar(int monsterId, int monsterHp) => (monsterId > 0 && monsterHp != 0) || this.Configuring || IsAlwaysShowingMonsterInfo();

    public static bool ShowOverlayStatIcon
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            return s.OverlayStatIconShown;
        }
    }

    public static string QuestTimeMode => GetTimerMode() switch
    {
        "Time Left" => "Time Left",
        "Time Elapsed" => "Elapsed",
        _ => "Quest Time",
    };

    public bool? RoadOverride()
    {
        // should work
        if (this.QuestID() is not 23527 and not 23628)
        {
            return true;
        }
        else if (this.QuestID() is 23527 or 23628)
        {
            return false;
        }

        return null;
    }

    /// <summary>
    /// Gets the name of the quest for discord.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetDiscordQuestNameFromID(int id)
    {
        if (id > 63421 && DiscordService.ShowDiscordQuestNames())
        {
            return $"Custom Quest {id}";
        }

        if (!DiscordService.ShowDiscordQuestNames())
        {
            return string.Empty;
        }

        EZlion.Mapper.Quest.IDName.TryGetValue(id, out var questValue1);  // returns true

        return questValue1 + string.Empty;
    }

    public static string GetQuestName(int questID)
    {
        EZlion.Mapper.Quest.IDName.TryGetValue(questID, out var questValue1);  // returns true

        return questValue1 + string.Empty;
    }

    // TODO need to create another variable for discordManager. Ideally discordManager state only affects it.
    public int PreviousHubAreaID { get; set; }

    public bool ClosedGame { get; set; }

    public bool IsInLauncherBool { get; set; }

    public string Monster1Part1Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part1();
            return this.GetPartName(1, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    // TODO convert to bool and remove isInLauncherBool?
    public string IsInLauncher()
    {
        // TODO: test
        if (this.QuestID() != 0 ||
        (this.AreaID() != 0 && Location.IDName.ContainsKey(this.AreaID())))
        {
            return "No";
        }

        var pidToSearch = this.m.GetProcIdFromName("mhf");

        // Init a condition indicating that you want to search by process id.
        var condition = new PropertyCondition(
            AutomationElementIdentifiers.ProcessIdProperty,
            pidToSearch);

        AutomationElement? element = null;

        // Find the automation element matching the criteria
        // TODO: does this even fix anything?
        try
        {
            element = AutomationElement.RootElement.FindFirst(
TreeScope.Children, condition);
        }
        catch (Exception ex)
        {
            LoggerInstance.Warn(ex, "Could not find AutomationElement");
        }

        if (element == null || pidToSearch == 0)
        {
            return "NULL"; // TODO this looks ugly
        }

        // get the classname
        var className = element.Current.ClassName;

        if (className == "MHFLAUNCH")
        {
            return "Yes";
        }
        else
        {
            return "No";
        }
    }

    public string GetOverlayModeForStorage()
    {
        return this.GetOverlayMode() switch
        {
            OverlayMode.Unknown => "Unknown",
            OverlayMode.Standard => "Standard",
            OverlayMode.Configuring => "Configuring",
            OverlayMode.ClosedGame => "Closed Game",
            OverlayMode.Launcher => "Launcher",
            OverlayMode.NoGame => "No Game",
            OverlayMode.MainMenu => "Main Menu",
            OverlayMode.WorldSelect => "World Select",
            OverlayMode.Speedrun => "Speedrun",
            OverlayMode.Zen => "Zen",
            _ => "Not Found",
        };
    }

    /// <summary>
    /// This is only used for display
    /// </summary>
    /// <returns></returns>
    public string GetFinalOverlayModeForDisplay()
    {
        if ((OverlayModeDictionary.Count == 2 && OverlayModeDictionary.Last().Value == "Standard") ||
                            (OverlayModeDictionary.Count == 1 && OverlayModeDictionary.First().Value == "Standard") ||
                            OverlayModeDictionary.Count > 2 || OverlayModeDictionary.Count == 0)
        {
            return "Standard+";
        }
        else
        {
            // TODO: test
            if (OverlayModeDictionary.Count == 2 && OverlayModeDictionary.First().Value == "Standard")
            {
                if (OverlayModeDictionary.Last().Value == "Speedrun")
                {
                    return "Speedrun+" + $" ({GetRunBuffsTag(GetRunBuffs(), (QuestVariant2)QuestVariant2(), (QuestVariant3)QuestVariant3())})";
                }

                return OverlayModeDictionary.Last().Value + "+";
            }
            else
            {
                if (OverlayModeDictionary.First().Value == "Speedrun")
                {
                    return "Speedrun+" + $" ({GetRunBuffsTag(GetRunBuffs(), (QuestVariant2)QuestVariant2(), (QuestVariant3)QuestVariant3())})";
                }

                return OverlayModeDictionary.First().Value + "+";
            }
        }
    }

    public string GetOverlayModeForRPC()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        if (s.ShowDiscordRPCOverlayMode)
        {
            if (s.DiscordOverlayMode == "Final" || (s.DiscordOverlayMode == "Automatic" && s.OverlayWatermarkMode == "Final"))
            {
                return $"{GetFinalOverlayModeForDisplay()} | ";
            }
            else
            {
                return this.GetOverlayMode() switch
                {
                    OverlayMode.Standard => "Standard | ",
                    OverlayMode.Configuring => "Configuring | ",
                    OverlayMode.ClosedGame => "Closed Game | ",
                    OverlayMode.Launcher => "Launcher | ",
                    OverlayMode.NoGame => "Game not found | ",
                    OverlayMode.MainMenu => "Main menu | ",
                    OverlayMode.WorldSelect => "World Select | ",
                    OverlayMode.Speedrun => "Speedrun | ",
                    OverlayMode.Zen => "Zen | ",
                    _ => string.Empty,

                };
            }
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the overlay mode.
    /// </summary>
    /// <returns></returns>
    public OverlayMode GetOverlayMode()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        var playerAtk = 0;
        var success = int.TryParse(this.ATK, NumberStyles.Number, CultureInfo.InvariantCulture, out var playerTrueRaw);
        if (!success)
        {
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Could not parse player true raw as integer: {0}", this.ATK);
        }
        else
        {
            playerAtk = playerTrueRaw;
        }

        if (this.Configuring)
        {
            return OverlayMode.Configuring;
        }
        else if (this.ClosedGame)
        {
            return OverlayMode.ClosedGame;
        }
        else if (this.IsInLauncherBool || this.IsInLauncher() == "Yes") // works?
        {
            return OverlayMode.Launcher;
        }
        else if (this.IsInLauncher() == "NULL")
        {
            return OverlayMode.NoGame;
        }
        else if (this.QuestID() == 0 && this.AreaID() == 0 && this.BlademasterWeaponID() == 0 && this.GunnerWeaponID() == 0)
        {
            return OverlayMode.MainMenu;
        }
        else if (this.QuestID() == 0 && this.AreaID() == 200 && this.BlademasterWeaponID() == 0 && this.GunnerWeaponID() == 0)
        {
            return OverlayMode.WorldSelect;
        } // TODO do i need to check for road and dure?
        else if (
            !(
                (this.QuestID() != 0
                && this.TimeDefInt() > this.TimeInt()
                && playerAtk > 0)
                || this.IsRoad() || this.IsDure())
            || s.EnableDamageNumbers
            || s.EnableSharpness
            || s.PartThresholdShown
            || s.HitCountShown
            || s.PlayerAtkShown
            || s.MonsterAtkMultShown
            || s.MonsterDefrateShown
            || s.MonsterSizeShown
            || s.MonsterPoisonShown
            || s.MonsterParaShown
            || s.MonsterSleepShown
            || s.MonsterBlastShown
            || s.MonsterStunShown
            || s.DamagePerSecondShown
            || s.TotalHitsTakenBlockedShown
            || s.PlayerAPMGraphShown
            || s.PlayerAttackGraphShown
            || s.PlayerDPSGraphShown
            || s.PlayerHitsPerSecondGraphShown
            || s.EnableQuestPaceColor
            || s.Monster1HealthBarShown
            || s.Monster2HealthBarShown
            || s.Monster3HealthBarShown
            || s.Monster4HealthBarShown
            || s.EnableMap
            || s.PersonalBestTimePercentShown
            || s.EnablePersonalBestPaceColor
            || s.PlayerPositionShown
            || s.DualSwordsSharpensShown) // TODO monster 1 overview? and update README
        {
            return OverlayMode.Standard;
        }
        else if (s.TimerInfoShown && s.EnableInputLogging && s.EnableQuestLogging && s.OverlayModeWatermarkShown)
        {
            return OverlayMode.Speedrun;
        }
        else
        {
            return OverlayMode.Zen;
        }
    }

    /// <summary>
    /// TODO needs real testing
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public int GetActiveFeature()
    {
        var activeFeatures = new[] { this.ActiveFeature1(), this.ActiveFeature2(), this.ActiveFeature3() };

        foreach (var activeFeature in activeFeatures)
        {
            if (IsValidBitfield((uint)activeFeature, (uint)ActiveFeature.All))
            {
                return activeFeature;
            }
        }

        LoggerInstance.Warn("Active feature not found: {0} {1} {2}", activeFeatures[0], activeFeatures[1], activeFeatures[2]);
        return 0;
    }

    /// <summary>
    /// This only works for bitfield argument values 255 and below, if you don't intend to extract bytes. Consider using HasFlag directly if so.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bitfield"></param>
    /// <param name="flag"></param>
    /// <param name="all"></param>
    /// <param name="extractByte"></param>
    /// <param name="bytePosition"></param>
    /// <returns></returns>
    public bool IsBitfieldContainingFlag<T>(uint bitfield, T flag, uint all, bool extractByte = false, int bytePosition = 0) where T : Enum
    {
        byte value = extractByte ? (byte)(((uint)bitfield >> (bytePosition * 8)) & 0xFF) : (byte)bitfield;

        // Validate
        if (!IsValidBitfield(value, all))
        {
            return false;
        }

        // Convert
        T convertedValue = (T)Enum.ToObject(typeof(T), value);

        var isFlagSet = convertedValue.HasFlag(flag);
        return isFlagSet;
    }

    public bool IsValidBitfield(uint value, uint all)
    {
        return (value & all) == value;
    }

    public bool AlternativeQuestOverride()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableCaravanOverride switch
        {
            "Enabled" => true,
            "Disabled" => false,
            "Automatic" => this.IsAlternativeQuestName() || this.IsDure(),
            _ => false,
        };
    }

    private bool QuestNameContainsAlternativeTitle() => GetQuestName(this.QuestID()).Contains("Daily Limited Quest≫") ||
            GetQuestName(this.QuestID()).Contains("Daily Quest≫") ||
            GetQuestName(this.QuestID()).Contains("Guild Quest≫") ||
            GetQuestName(this.QuestID()).Contains("Interception Base≫") ||
            GetQuestName(this.QuestID()).Contains("Interception Quest≫") ||
            GetQuestName(this.QuestID()).Contains("Interception Urgent Quest≫") ||
            GetQuestName(this.QuestID()).Contains("Great Slaying Quest≫") ||
            GetQuestName(this.QuestID()).Contains("G Rank Great Slaying≫") ||
            GetQuestName(this.QuestID()).Contains("New Weapon Type Acquisition≫");

    private bool QuestNameContainsAlternativeTitle(int questID) => GetQuestName(questID).Contains("Daily Limited Quest≫") ||
        GetQuestName(questID).Contains("Daily Quest≫") ||
        GetQuestName(questID).Contains("Guild Quest≫") ||
        GetQuestName(questID).Contains("Interception Base≫") ||
        GetQuestName(questID).Contains("Interception Quest≫") ||
        GetQuestName(questID).Contains("Interception Urgent Quest≫") ||
        GetQuestName(questID).Contains("Great Slaying Quest≫") ||
        GetQuestName(questID).Contains("G Rank Great Slaying≫") ||
        GetQuestName(questID).Contains("New Weapon Type Acquisition≫");

    private bool PreviousHubAreaIDIsAlternative() => this.PreviousHubAreaID switch
    {
        // Private Bar
        210 or 260 or 282 or 202 or 203 or 204 => true,
        _ => false,
    };

    public bool IsAlternativeQuestName()
    {
        if (this.QuestNameContainsAlternativeTitle() || this.PreviousHubAreaIDIsAlternative())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAlternativeQuestName(int questID)
    {
        if (this.QuestNameContainsAlternativeTitle(questID))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetNotRoad()
    {
        var b = this.RoadOverride();
        if (b != null)
        {
            return b.Value;
        }

        return this.IsNotRoad();
    }

    public string GetGamePatchInfo(GamePatchFile file, string hash)
    {
        var result = "Unknown";

        switch (file)
        {
            case GamePatchFile.dat:
                QuestsGamePatches.datHashInfo.TryGetValue(hash, out var datInfo);
                if (datInfo != null)
                {
                    //var a = datInfo.Keys.FirstOrDefault();
                    //var b = datInfo.Keys.FirstOrDefault().ToString();

                    return $"{datInfo.Keys.FirstOrDefault().ToString()}-{datInfo.Values.FirstOrDefault().ToString()}";
                }
                break;
            case GamePatchFile.emd:
                QuestsGamePatches.emdHashInfo.TryGetValue(hash, out var emdInfo);
                if (emdInfo != null)
                {
                    return $"{emdInfo.Keys.FirstOrDefault().ToString()}-{emdInfo.Values.FirstOrDefault().ToString()}";
                }
                break;
            case GamePatchFile.dll:
                QuestsGamePatches.mhfodllHashInfo.TryGetValue(hash, out var dllInfo);
                if (dllInfo != null)
                {
                    return $"{dllInfo.Keys.FirstOrDefault().ToString()}-{dllInfo.Values.FirstOrDefault().ToString()}";
                }
                break;
            case GamePatchFile.hddll:
                QuestsGamePatches.mhfohddllHashInfo.TryGetValue(hash, out var hddllInfo);
                if (hddllInfo != null)
                {
                    return $"{hddllInfo.Keys.FirstOrDefault().ToString()}-{hddllInfo.Values.FirstOrDefault().ToString()}";
                }
                break;
        }

        return result;
    }

    // assumption: it follows ferias' monster part order top to bottom, presumably (e.g. head is at the top, so part 0 is head, and so on)
    // grouping by skeleton too

    /// <summary>
    /// Monster parts labels.
    /// <para>int number: The part number from 1 to 10.</para>
    /// <para>int monsterID: the monsterID.</para>
    /// </summary>
    /// <returns></returns>
    public string GetPartName(int number, int monsterID)
    {
        // keep in mind this has the null
        if (this.RoadOverride() == false)
        {
            monsterID = this.RoadSelectedMonster() == 0 ? this.LargeMonster1ID() : this.LargeMonster2ID();
        }
        else if (this.AlternativeQuestOverride())
        {
            monsterID = this.AlternativeQuestMonster1ID();
        }

        if (this.GetDureName() != "None")
        {
            monsterID = 132;
        }

        if (this.GetRaviName() != "None")
        {
            switch (this.GetRaviName())
            {
                case "Raviente":
                case "Violent Raviente":
                    monsterID = 93;
                    break;
                case "Berserk Raviente Practice":
                case "Berserk Raviente":
                case "Extreme Raviente":
                    monsterID = 149;
                    break;
                default:
                    break;
            }
        }

        if (monsterID > 176)
        {
            return "Error: ";
        }
        else
        {
            if (number <= 0 && number >= 11)
            {
                return "None: ";
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}: ", FindPartName(number, monsterID));
            }
        }
    }

    /// <summary>
    /// https://stackoverflow.com/a/35775810/18859245.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private static string FindPartName(int number, int monsterID)
    {
        var partMonsterGroup = new List<int> { 0 };

        foreach (var kvp in MonsterPart.IDName)
        {
            var monsterIDs = kvp.Key;

            if (monsterIDs.Contains(monsterID))
            {
                partMonsterGroup = kvp.Key;
                break;
            }
        }

        return DeterminePartName(partMonsterGroup, number - 1);
    }

    private static string DeterminePartName(List<int> key, int slot)
    {
        var keyFound = MonsterPart.IDName.ContainsKey(key);

        if (!keyFound)
        {
            return "None";
        }
        else
        {
            if (slot > MonsterPart.IDName[key].Count)
            {
                return "None";
            }
            else
            {
                return MonsterPart.IDName[key][slot];
            }
        }
    }

    public string Monster1Part2Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part2();
            return this.GetPartName(2, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster1Part3Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part3();
            return this.GetPartName(3, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster1Part4Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part4();
            return this.GetPartName(4, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster1Part5Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part5();
            return this.GetPartName(5, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster1Part6Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part6();
            return this.GetPartName(6, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster1Part7Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part7();
            return this.GetPartName(7, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster1Part8Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part8();
            return this.GetPartName(8, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster1Part9Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part9();
            return this.GetPartName(9, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster1Part10Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster1Part10();
            return this.GetPartName(10, this.LargeMonster1ID()) + currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part1Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part1();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part2Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part2();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part3Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part3();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part4Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part4();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part5Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part5();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part6Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part6();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part7Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part7();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part8Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part8();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part9Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part9();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster2Part10Number
    {
        get
        {
            if (this.QuestID() == 0)
            {
                return Messages.MonsterPartNotLoaded;
            }

            var currentPartHP = this.Monster2Part10();
            return currentPartHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Shows the sharpness percentage.
    /// </summary>
    /// <returns></returns>
    public static bool ShowSharpnessPercentage()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableSharpnessPercentage;
    }

    /// <summary>
    /// Shows the time left percentage.
    /// </summary>
    /// <returns></returns>
    public static bool ShowTimeLeftPercentage()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableTimeLeftPercentage;
    }

    /// <summary>
    /// Gets the timer mode.
    /// </summary>
    /// <returns></returns>
    public static string GetTimerMode()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        if (s.TimerMode == "Time Left")
        {
            return "Time Left";
        }
        else if (s.TimerMode == "Time Elapsed")
        {
            return "Time Elapsed";
        }
        else
        {
            return "Time Left";
        }
    }

    private string timeLeftPercent = string.Empty;

    private decimal previousMonsterDefrate = decimal.Zero;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <value>
    /// The metadata.
    /// </value>
    public static string GetMetadata
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (!s.EnableMetadataExport)
            {
                return string.Empty;
            }

            string guildName;
            string hunterName;

            var dateAndTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

            if (s.GuildName.Length >= 1)
            {
                guildName = s.GuildName;
            }
            else
            {
                guildName = "Guild Name";
            }

            if (s.HunterName.Length >= 1)
            {
                hunterName = s.HunterName;
            }
            else
            {
                hunterName = "Hunter Name";
            }

            return string.Format(CultureInfo.InvariantCulture, "\n{0} | {1} | {2}", hunterName, guildName, dateAndTime);
        }
    }

    public string SharpnessPercentNumber
    {
        get
        {
            if (ShowSharpnessPercentage())
            {
                if (this.maxSharpness < this.Sharpness())
                {
                    this.maxSharpness = this.Sharpness();
                    return string.Format(CultureInfo.InvariantCulture, " ({0:0}%)", (float)this.Sharpness() / this.maxSharpness * 100.0);
                }
                else if (this.Sharpness() <= 0)
                {
                    return " (0%)";
                }
                else // MaxSharpness > CurrentSharpness && CurrentSharpness > 0
                {
                    return string.Format(CultureInfo.InvariantCulture, " ({0:0}%)", (float)this.Sharpness() / this.maxSharpness * 100.0);
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }

    private StringBuilder sbForTimer = new StringBuilder();

    /// <summary>
    /// Gets quest time in the format of mm:ss.fff. This should only be used for display purposes.
    /// </summary>
    public string Time
    {
        get
        {
            // check for 1st and 2nd district dure
            // TODO: find timedefint address for dures
            var isDure = QuestID() == 21731 || QuestID() == 21746;
            decimal timeDefInt = isDure ? Numbers.DuremudiraTimeLimitFrames : TimeDefInt();

            var timerMode = GetTimerMode() == "Time Elapsed" ? TimerMode.Elapsed : TimerMode.TimeLeft;
            decimal time = TimeService.GetTimeValue(timerMode, timeDefInt, (decimal)TimeInt());
            decimal framesPerSecond = Numbers.FramesPerSecond;
            decimal totalSeconds = time / framesPerSecond;
            decimal minutes = Math.Floor(totalSeconds / 60);
            decimal seconds = Math.Floor(totalSeconds % 60);
            decimal milliseconds = Math.Round((time % framesPerSecond) * (1000M / framesPerSecond));

            this.timeLeftPercent = ShowTimeLeftPercentage() ? TimeService.GetTimeLeftPercent(timeDefInt, TimeInt(), isDure) : string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
            sb.Append(this.timeLeftPercent);

            // MessageBox.Show(TimeService.TestTimerMethods(216_000 * 10)); // 2 hours at 30 fps

            return sb.ToString();
        }
    }

    // per quest
    public int HighestAtk { get; set; }

    public double HighestDPS { get; set; }

    public string IsHighestAtk
    {
        get
        {
            if (this.WeaponRaw() == this.HighestAtk && ShowHighestAtkColor())
            {
                this.PlayerAttackIcon = "../../Assets/Icons/png/attack_up_red.png";
                return "#f38ba8";
            }
            else
            {
                this.PlayerAttackIcon = "../../Assets/Icons/png/attack_up.png";
                return "#f5e0dc";
            }
        }
    }

    /// <summary>
    /// Shows the color of the highest atk.
    /// </summary>
    /// <returns></returns>
    public static bool ShowHighestAtkColor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableHighestAtkColor;
    }

    public static bool ShowHighestDPSColor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableHighestDPSColor;
    }

    public static bool ShowAverageHitsPerSecondColor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableAverageHitsPerSecondColor;
    }

    public static bool ShowAverageAPMColor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableAverageActionsPerMinuteColor;
    }

    public static bool ShowHitsTakenBlockedColor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableTotalHitsTakenBlockedColor;
    }

    public static bool ShowQuestPaceColor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableQuestPaceColor;
    }

    public static bool ShowPersonalBestPaceColor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnablePersonalBestPaceColor;
    }

    public static bool ShowHighestMonsterAttackMultiplierColor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableHighestMonsterAttackMultiplierColor;
    }

    public static bool ShowLowestMonsterDefrateColor()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableLowestMonsterDefrateColor;
    }

    public string IsHighestDPS
    {
        get
        {
            if (this.DPS + 10 >= this.HighestDPS && ShowHighestDPSColor())
            {
                this.DPSIcon = "../../Assets/Icons/png/burst_red.png";
                return "#f38ba8";
            }
            else
            {
                this.DPSIcon = "../../Assets/Icons/png/burst.png";
                return "#f5e0dc";
            }
        }
    }

    public string IsAverageHitsPerSecond
    {
        get
        {
            if (this.HitsPerSecond >= GetAverageHitsPerSecond(this.WeaponType()) && ShowAverageHitsPerSecondColor())
            {
                this.HitCountIcon = "../../Assets/Icons/png/red_soul.png";
                return "#f38ba8";
            }
            else
            {
                this.HitCountIcon = "../../Assets/Icons/png/blue_soul.png";
                return "#f5e0dc";
            }
        }
    }

    public string IsAverageAPM
    {
        get
        {
            // get the average hits of a weapon per second, multiply by 60 to get the minutes, multiply by 2 to account for other actions
            if (this.APM >= GetAverageHitsPerSecond(this.WeaponType()) * 60 * 2 && ShowAverageAPMColor())
            {
                this.APMIcon = "../../Assets/Icons/png/flame_hc.png";
                return "#f38ba8";
            }
            else
            {
                this.APMIcon = "../../Assets/Icons/png/flame_ul.png";
                return "#f5e0dc";
            }
        }
    }

    public string IsPlayerHit
    {
        get
        {
            var weaponFound = WeaponCanUseReflect.WeaponTypeID.ContainsKey(this.WeaponType());
            if (!weaponFound)
            {
                return "#f5e0dc";
            }

            var hasReflect = false;

            if (WeaponCanUseReflect.WeaponTypeID[this.WeaponType()])
            {
                hasReflect = true;
            }

            // for speedrunners to get hit by 1 small bomb at least. and 2 perfect guards to get max obscurity.
            if ((!hasReflect && this.TotalHitsTakenBlocked <= 1) || (hasReflect && this.TotalHitsTakenBlocked <= 3 && ShowHitsTakenBlockedColor()))
            {
                this.PlayerHitsTakenBlockedIcon = "../../Assets/Icons/png/defense_up.png";
                return "#f38ba8";
            }
            else
            {
                this.PlayerHitsTakenBlockedIcon = "../../Assets/Icons/png/defense_down.png";
                return "#f5e0dc";
            }
        }
    }

    public bool DivaSongEnding
    {
        get
        {
            var divaSongStart = Math.Max(DivaSongStart(), DivaSongFromGuildStart());

            if (divaSongStart <= 0)
            {
                return true;
            }

            var expiry = divaSongStart + (60 * 90);
            double secondsLeft = expiry - ServerHeartbeat;

            return secondsLeft <= 60 * 10;
        }
    }

    public bool DivaSongEnded
    {
        get
        {
            var divaSongStart = Math.Max(DivaSongStart(), DivaSongFromGuildStart());

            if (divaSongStart <= 0)
            {
                return true;
            }

            var expiry = divaSongStart + (60 * 90);
            double secondsLeft = expiry - ServerHeartbeat;

            return secondsLeft <= 0;
        }
    }

    /// <summary>
    /// Whether the buff is still active even if it expired inside quest but after quest start.
    /// </summary>
    public bool DivaSongActive { get; set; }

    /// <summary>
    /// Whether the buff is still active even if it expired inside quest but after quest start.
    /// </summary>
    public bool GuildFoodActive { get; set; }

    public bool GuildFoodEnding
    {
        get
        {
            if (GuildFoodStart() <= 0)
            {
                return true;
            }

            var expiry = GuildFoodStart() + (60 * 90);
            double secondsLeft = expiry - ServerHeartbeat;

            return secondsLeft <= 60 * 10;
        }
    }

    public bool GuildFoodEnded
    {
        get
        {
            if (GuildFoodStart() <= 0)
            {
                return true;
            }

            var expiry = GuildFoodStart() + (60 * 90);
            double secondsLeft = expiry - ServerHeartbeat;

            return secondsLeft <= 0;
        }
    }

    public string DivaSongFill => DivaSongEnding ? CatppuccinMochaColors.NameHex["Red"] : CatppuccinMochaColors.NameHex["Rosewater"];

    public string GuildFoodFill => GuildFoodEnding ? CatppuccinMochaColors.NameHex["Red"] : CatppuccinMochaColors.NameHex["Rosewater"];

    public double DivaSongOpacity => DivaSongEnding ? 1 : .5;

    public double GuildFoodOpacity => GuildFoodEnding ? 1 : .5;

    public string IsOnPace
    {
        get
        {
            if (!int.TryParse(this.Monster1MaxHP, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedMonster1MaxHP))
            {
                // Handle the case when Monster1MaxHP cannot be parsed to an int
                // For example, you can return an error message, set a default value or throw an exception
                LoggerInstance.Warn(CultureInfo.InvariantCulture, "Could not parse Monster1MaxHP to get pace: {0}", this.Monster1MaxHP);
                return "#f5e0dc";
            }

            if (this.TimeDefInt() == 0 || parsedMonster1MaxHP <= 1)
            {
                this.QuestTimeIcon = "../../Assets/Icons/png/sand_clock.png";
                return "#f5e0dc";
            }

            if (this.TimeDefInt() < this.TimeInt())
            {
                this.QuestTimeIcon = "../../Assets/Icons/png/sand_clock.png";
                return "#f5e0dc";
            }

            var timePercent = (float)this.TimeInt() / this.TimeDefInt() * 100.0;
            var monster1HPPercent = (float)this.Monster1HPInt() / parsedMonster1MaxHP * 100.0;

            if (timePercent >= monster1HPPercent && ShowQuestPaceColor())
            {
                if (GetTimerMode() == "Time Left")
                {
                    this.QuestTimeIcon = "../../Assets/Icons/png/sand_clock2_red.png";
                }
                else
                {
                    this.QuestTimeIcon = "../../Assets/Icons/png/clock_red.png";
                }

                return "#f38ba8";
            }
            else
            {
                if (GetTimerMode() == "Time Left")
                {
                    this.QuestTimeIcon = "../../Assets/Icons/png/sand_clock2.png";
                }
                else
                {
                    this.QuestTimeIcon = "../../Assets/Icons/png/clock.png";
                }

                return "#f5e0dc";
            }
        }
    }

    public bool isShitenQuest(int questID)
    {
        return questID switch
        {
            Numbers.QuestIDUpperShitenDisufiroa => true,
            Numbers.QuestIDLowerShitenDisufiroa => true,
            Numbers.QuestIDUpperShitenUnknown => true,
            Numbers.QuestIDLowerShitenUnknown => true,
            _ => false
        };
    }

    public bool IsHalkPotEquipped()
    {
        return (PouchItem1ID() == 4952 || PouchItem2ID() == 4952 || PouchItem3ID() == 4952 || PouchItem4ID() == 4952 || PouchItem5ID() == 4952 || PouchItem6ID() == 4952 || PouchItem7ID() == 4952 || PouchItem8ID() == 4952 || PouchItem9ID() == 4952 || PouchItem10ID() == 4952 || PouchItem11ID() == 4952 || PouchItem12ID() == 4952 || PouchItem13ID() == 4952 || PouchItem14ID() == 4952 || PouchItem15ID() == 4952 || PouchItem16ID() == 4952 || PouchItem17ID() == 4952 || PouchItem18ID() == 4952 || PouchItem19ID() == 4952 || PouchItem20ID() == 4952 || PartnyaBagItem1ID() == 4952 || PartnyaBagItem2ID() == 4952 || PartnyaBagItem3ID() == 4952 || PartnyaBagItem4ID() == 4952 || PartnyaBagItem5ID() == 4952 || PartnyaBagItem6ID() == 4952 || PartnyaBagItem7ID() == 4952 || PartnyaBagItem8ID() == 4952 || PartnyaBagItem9ID() == 4952 || PartnyaBagItem10ID() == 4952);
    }

    public string GetRunBuffsTag(RunBuff runBuff, QuestVariant2 questVariant2, QuestVariant3 questVariant3)
    {
        return runBuff switch
        {
            RunBuff.None => "None",
            RunBuff.LeaderboardTimeAttack => "TA",
            RunBuff.LeaderboardFreestyleDivaSkill => "FDS",
            RunBuff.LeaderboardFreestyleDivaPrayerGem => "FDP",
            RunBuff.LeaderboardFreestyleSecretTechnique => "FST",
            RunBuff.LeaderboardFreestyleCourseAttackBoost => "FCA",
            _ => CalculateRunBuffsTag(runBuff, questVariant2, questVariant3),
        };
    }

    /// <summary>
    /// TODO test
    /// </summary>
    /// <param name="runBuffs"></param>
    /// <returns></returns>
    public string CalculateRunBuffsTag(RunBuff runBuffs, QuestVariant2 questVariant2, QuestVariant3 questVariant3)
    {
        var value = (uint)runBuffs;

        if (runBuffs.HasFlag(RunBuff.CourseAttackBoost))
        {
            return "FCA";
        }

        if (runBuffs.HasFlag(RunBuff.SecretTechnique) && !runBuffs.HasFlag(RunBuff.HalkPotEffect))
        {
            return "FST";
        }

        if (runBuffs.HasFlag(RunBuff.DivaPrayerGem) && !runBuffs.HasFlag(RunBuff.HalkPotEffect))
        {
            return "FDP";
        }

        if (runBuffs.HasFlag(RunBuff.DivaSkill) && !runBuffs.HasFlag(RunBuff.HalkPotEffect))
        {
            return "FDS";
        }

        if (!runBuffs.HasFlag(RunBuff.HalkPotEffect) && !runBuffs.HasFlag(RunBuff.ActiveFeature))
        {
            return "TA";
        }

        // elz 3m
        if (runBuffs.HasFlag(RunBuff.LeaderboardTimeAttack) && runBuffs.HasFlag(RunBuff.ActiveFeature) && (questVariant3.HasFlag(Models.Structures.QuestVariant3.NoGPSkills)))
        {
            return "TA";
        }

        // dures and w/e
        if (runBuffs.HasFlag(RunBuff.PoogieItem) && runBuffs.HasFlag(RunBuff.DivaSong) && runBuffs.HasFlag(RunBuff.Bento) && runBuffs.HasFlag(RunBuff.GuildPoogie) && runBuffs.HasFlag(RunBuff.ActiveFeature) && runBuffs.HasFlag(RunBuff.GuildFood) && (questVariant2.HasFlag(Models.Structures.QuestVariant2.Road) || questVariant3.HasFlag(Models.Structures.QuestVariant3.NoGPSkills)))
        {
            return "TA";
        }

        return value.ToString();
    }

    public QuestsQuestVariant GetQuestVariants(long questID)
    {
        QuestsQuestVariant questVariants = new();

        if (QuestVariants.QuestIDVariant.ContainsKey(questID))
        {
            questVariants.QuestVariant1 = QuestVariants.QuestIDVariant[questID].QuestVariant1;
            questVariants.QuestVariant2 = QuestVariants.QuestIDVariant[questID].QuestVariant2;
            questVariants.QuestVariant3 = QuestVariants.QuestIDVariant[questID].QuestVariant3;
            questVariants.QuestVariant4 = QuestVariants.QuestIDVariant[questID].QuestVariant4;
        }

        return questVariants;
    }

    /// <summary>
    /// Decrements the run buffs input if the quest variants disallow it.
    /// </summary>
    /// <param name="runBuffs"></param>
    /// <param name="questVariants"></param>
    /// <returns></returns>
    public RunBuff GetRunBuffs(RunBuff runBuffs, QuestsQuestVariant questVariants)
    {
        var questVariant2 = (QuestVariant2?)questVariants.QuestVariant2 ?? Models.Structures.QuestVariant2.None;
        var questVariant3 = (QuestVariant3?)questVariants.QuestVariant3 ?? Models.Structures.QuestVariant3.None;

        if (runBuffs.HasFlag(RunBuff.Halk) && (questVariant2.HasFlag(Models.Structures.QuestVariant2.DisableHalkPoogieCuff) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Road)))
        {
            runBuffs -= RunBuff.Halk;
        }

        //if (PoogieItemUseID() > 0)
        //{
        //    runBuffs |= RunBuff.PoogieItem;
        //}

        //if (DivaSongActive)
        //{
        //    runBuffs |= RunBuff.DivaSong;
        //}

        if (runBuffs.HasFlag(RunBuff.HalkPotEffect) && (questVariant2.HasFlag(Models.Structures.QuestVariant2.DisableHalkPotionCourseAttack) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Level9999) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Road)))
        {
            runBuffs -= RunBuff.HalkPotEffect;
        }

        // TODO bento
        //if (true == true)
        //{
        //    runBuffs |= RunBuff.Bento;
        //}

        //if (GuildPoogie1Skill() > 0 || GuildPoogie2Skill() > 0 || GuildPoogie3Skill() > 0)
        //{
        //    runBuffs |= RunBuff.GuildPoogie;
        //}

        if (runBuffs.HasFlag(RunBuff.ActiveFeature) && questVariant2.HasFlag(Models.Structures.QuestVariant2.DisableActiveFeature))
        {
            runBuffs -= RunBuff.ActiveFeature;
        }

        //if (GuildFoodSkill() > 0)
        //{
        //    runBuffs |= RunBuff.GuildFood;
        //}

        if (runBuffs.HasFlag(RunBuff.DivaSkill) && (questVariant2.HasFlag(Models.Structures.QuestVariant2.Road) || questVariant3.HasFlag(Models.Structures.QuestVariant3.NoGPSkills)))
        {
            runBuffs -= RunBuff.DivaSkill;
        }

        if (runBuffs.HasFlag(RunBuff.SecretTechnique) && questVariant2.HasFlag(Models.Structures.QuestVariant2.Level9999))
        {
            runBuffs -= RunBuff.SecretTechnique;
        }

        if (runBuffs.HasFlag(RunBuff.DivaPrayerGem) && (questVariant2.HasFlag(Models.Structures.QuestVariant2.Road) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Level9999)))
        {
            runBuffs -= RunBuff.DivaPrayerGem;
        }

        if (runBuffs.HasFlag(RunBuff.CourseAttackBoost) && (questVariant2.HasFlag(Models.Structures.QuestVariant2.DisableHalkPotionCourseAttack) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Level9999)))
        {
            runBuffs -= RunBuff.CourseAttackBoost;
        }

        return runBuffs;
    }

    /// <summary>
    /// Gets the run buffs. The runBuffs parameter should be given by doing GetBaseRunBuffs.
    /// </summary>
    /// <param name="overlayMode"></param>
    /// <returns></returns>
    public RunBuff GetRunBuffs(long questID, string overlayMode, RunBuff runBuffs)
    {
        if (overlayMode != string.Empty && questID > 0)
        {
            QuestsQuestVariant questVariants = GetQuestVariants(questID);

            switch (overlayMode)
            {
                case Messages.OverlayModeFreestyleNoSecretTech:
                    return GetRunBuffs(RunBuff.FreestyleNoSecretTech, questVariants);
                case Messages.OverlayModeFreestyleWithSecretTech:
                    return GetRunBuffs(RunBuff.FreestyleWithSecretTech, questVariants);
                case Messages.OverlayModeTimeAttack:
                    return GetRunBuffs(RunBuff.TimeAttack, questVariants);
                case Messages.OverlayModeSpeedrun:
                    return GetRunBuffs(runBuffs, questVariants);
                default:
                    // todo update
                    // we do not know the quest variants in 0.34, so we set as none.
                    // if we do not take into account quest variants, calculating the run buffs
                    // may be wrong because, for example, if we detect that halk was on we increase by 1 but
                    // in quests where halk is disabled the value in db is still positive, although in-game
                    // halk is off. Also setting this to TA tags would not take into account that the runs may have used HP bars etc.
                    return RunBuff.None;
            }
        }
        else
        {
            LoggerInstance.Error($"Wrong argument values for GetRunBuffs. QuestID {questID} OverlayMode {overlayMode}");
            return RunBuff.None;
        }
    }

    public RunBuff GetRunBuffs()
    {
        var runBuffs = RunBuff.None;
        var questVariant2 = (QuestVariant2)QuestVariant2();
        var questVariant3 = (QuestVariant3)QuestVariant3();

        if (HalkOn() && !(questVariant2.HasFlag(Models.Structures.QuestVariant2.DisableHalkPoogieCuff) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Road)))
        {
            runBuffs |= RunBuff.Halk;
        }

        if (PoogieItemUseID() > 0)
        {
            runBuffs |= RunBuff.PoogieItem;
        }

        if (DivaSongActive)
        {
            runBuffs |= RunBuff.DivaSong;
        }

        if ((HalkPotEffectOn() || IsHalkPotEquipped()) && !(questVariant2.HasFlag(Models.Structures.QuestVariant2.DisableHalkPotionCourseAttack) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Level9999) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Road)))
        {
            runBuffs |= RunBuff.HalkPotEffect;
        }

        // TODO bento
        if (true == true)
        {
            runBuffs |= RunBuff.Bento;
        }

        if (GuildPoogie1Skill() > 0 || GuildPoogie2Skill() > 0 || GuildPoogie3Skill() > 0)
        {
            runBuffs |= RunBuff.GuildPoogie;
        }

        if (IsActiveFeatureOn(GetActiveFeature(), WeaponType()) && !questVariant2.HasFlag(Models.Structures.QuestVariant2.DisableActiveFeature))
        {
            runBuffs |= RunBuff.ActiveFeature;
        }

        if (GuildFoodSkill() > 0)
        {
            runBuffs |= RunBuff.GuildFood;
        }

        if ((DivaSkill() > 0 && DivaSkillUsesLeft() > 0) && !(questVariant2.HasFlag(Models.Structures.QuestVariant2.Road) || questVariant3.HasFlag(Models.Structures.QuestVariant3.NoGPSkills)))
        {
            runBuffs |= RunBuff.DivaSkill;
        }

        if ((StyleRank1() == 15 || StyleRank2() == 15) && !questVariant2.HasFlag(Models.Structures.QuestVariant2.Level9999))
        {
            runBuffs |= RunBuff.SecretTechnique;
        }

        if ((DivaPrayerGemRedSkill() != 0 || DivaPrayerGemYellowSkill() != 0 || DivaPrayerGemGreenSkill() != 0 || DivaPrayerGemBlueSkill() != 0) && !(questVariant2.HasFlag(Models.Structures.QuestVariant2.Road) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Level9999)))
        {
            runBuffs |= RunBuff.DivaPrayerGem;
        }

        if (GetAdditionalCourses(Rights()).Contains("Support") && !(questVariant2.HasFlag(Models.Structures.QuestVariant2.DisableHalkPotionCourseAttack) || questVariant2.HasFlag(Models.Structures.QuestVariant2.Level9999)))
        {
            runBuffs |= RunBuff.CourseAttackBoost;
        }

        return runBuffs;
    }

    public static double GetAverageHitsPerSecond(int weaponTypeID)
    {
        var weaponFound = WeaponCanUseReflect.WeaponTypeID.ContainsKey(weaponTypeID);
        if (!weaponFound)
        {
            return 0;
        }

        var averageFound = WeaponAverageHitsPerSecond.WeaponAverageHitsPerSecondID.ContainsKey(weaponTypeID);
        if (!averageFound)
        {
            return 0;
        }

        var averageMultiplier = 0.5;

        if (!WeaponCanUseReflect.WeaponTypeID[weaponTypeID])
        {
            averageMultiplier = 0.4;
        }

        return WeaponAverageHitsPerSecond.WeaponAverageHitsPerSecondID[weaponTypeID] * averageMultiplier;
    }

    public string IsOnBestPace
    {
        get
        {
            if (!int.TryParse(this.Monster1MaxHP, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedMonster1MaxHP))
            {
                // Handle the case when Monster1MaxHP cannot be parsed to an int
                // For example, you can return an error message, set a default value or throw an exception
                LoggerInstance.Warn(CultureInfo.InvariantCulture, "Could not parse Monster1MaxHP to get best pace: {0}", this.Monster1MaxHP);
                return "#f5e0dc";
            }

            if (this.TimeDefInt() == 0 || parsedMonster1MaxHP <= 1)
            {
                this.PersonalBestIcon = "../../Assets/Icons/png/quest_clock.png";
                return "#f5e0dc";
            }

            if (this.TimeDefInt() < this.TimeInt())
            {
                this.PersonalBestIcon = "../../Assets/Icons/png/quest_clock.png";
                return "#f5e0dc";
            }

            if (!int.TryParse(this.PersonalBestTimePercent.Replace("%", string.Empty), NumberStyles.Integer, CultureInfo.InvariantCulture, out var timePercent))
            {
                // Handle the case when PersonalBestTimePercent cannot be parsed to an int
                // For example, you can return an error message, set a default value or throw an exception
                LoggerInstance.Warn(CultureInfo.InvariantCulture, "Could not parse Monster1MaxHP to get best pace: {0}", this.Monster1MaxHP);
                return "#f5e0dc";
            }

            var monster1HPPercent = (float)this.Monster1HPInt() / parsedMonster1MaxHP * 100.0;

            if (timePercent >= monster1HPPercent && ShowPersonalBestPaceColor())
            {
                this.PersonalBestIcon = "../../Assets/Icons/png/quest_clock_red.png";
                return "#f38ba8";
            }
            else
            {
                this.PersonalBestIcon = "../../Assets/Icons/png/quest_clock.png";
                return "#f5e0dc";
            }
        }
    }

    public string PersonalBestLoaded { get; set; } = Messages.TimerNotLoaded;

    public string PersonalBestTimePercent
    {
        get
        {
            if (this.PersonalBestLoaded != Messages.TimerNotLoaded &&
                ShowPersonalBestPaceColor() &&
                !this.PersonalBestLoaded.Contains("-"))
            {
                // TODO does this work for times over 59m?
                var personalBestInFrames = (int)((int)Numbers.FramesPerSecond * TimeSpan.ParseExact(this.PersonalBestLoaded, "mm':'ss'.'fff", CultureInfo.InvariantCulture).TotalSeconds);
                var personalBestTimeFramesElapsed = 0;
                var timeDefInt = this.QuestID() == Numbers.QuestIDFirstDistrictDuremudira || this.QuestID() == Numbers.QuestIDSecondDistrictDuremudira ? Numbers.DuremudiraTimeLimitFrames : this.TimeDefInt();
                if (GetTimerMode() == "Time Left")
                {
                    personalBestTimeFramesElapsed = (int)(timeDefInt - personalBestInFrames);
                }
                else
                {
                    personalBestTimeFramesElapsed = personalBestInFrames;
                }

                var elapsedPersonalBestTimePercent = this.CalculatePersonalBestInFramesPercent(personalBestTimeFramesElapsed, (int)timeDefInt);

                return string.Format(CultureInfo.InvariantCulture, "{0:0}%", elapsedPersonalBestTimePercent);
            }
            else
            {
                return "0%";
            }
        }
    }

    public string IsHighestMonsterAttackMultiplier
    {
        get
        {
            if (this.AtkMult == this.HighestAttackMult.ToString(CultureInfo.InvariantCulture) && ShowHighestMonsterAttackMultiplierColor())
            {
                this.MonsterAttackMultiplierIcon = "../../Assets/Icons/png/dure_attack_red.png";
                return "#f38ba8";
            }
            else
            {
                this.MonsterAttackMultiplierIcon = "../../Assets/Icons/png/dure_attack.png";
                return "#f5e0dc";
            }
        }
    }

    private double HighestAttackMult { get; set; }

    public double CalculatePersonalBestInFramesPercent(double personalBestInFramesElapsed, int timeDefInt)
    {
        if (personalBestInFramesElapsed <= 0)
        {
            return 0;
        }
        else
        {
            return 100 - ((timeDefInt - this.TimeInt()) / personalBestInFramesElapsed * 100.0);
        }
    }

    private decimal LowestMonsterDefrate { get; set; } = 1_000;

    public string IsLowestMonsterDefrate
    {
        get
        {
            if (this.DefMult == this.LowestMonsterDefrate.ToString(CultureInfo.InvariantCulture) && ShowLowestMonsterDefrateColor())
            {
                this.MonsterDefrateIcon = "../../Assets/Icons/png/dure_defense_red.png";
                return "#f38ba8";
            }
            else
            {
                this.MonsterDefrateIcon = "../../Assets/Icons/png/dure_defense.png";
                return "#f5e0dc";
            }
        }
    }

    public string MonsterAttackMultiplierIcon { get; set; } = "../../Assets/Icons/png/dure_attack.png";

    public string MonsterDefrateIcon { get; set; } = "../../Assets/Icons/png/dure_defense.png";

    public string DPSIcon { get; set; } = "../../Assets/Icons/png/burst.png";

    public string QuestTimeIcon { get; set; } = "../../Assets/Icons/png/clock.png";

    public string PlayerAttackIcon { get; set; } = "../../Assets/Icons/png/attack_up.png";

    public string PlayerHitsTakenBlockedIcon { get; set; } = "../../Assets/Icons/png/defense_up.png";

    public string HitCountIcon { get; set; } = "../../Assets/Icons/png/blue_soul.png";

    public string APMIcon { get; set; } = "../../Assets/Icons/png/flame_ul.png";

    public string PersonalBestIcon { get; set; } = "../../Assets/Icons/png/quest_clock.png";

    public string DivaSongIcon { get; set; } = "../../Assets/Icons/png/diva_fountain.png";

    public string DualSwordsSharpensIcon { get; set; } = "../../Assets/Icons/png/whetstone.png";

    public string GuildFoodIcon { get; set; } = "../../Assets/Icons/png/guild_hall.png";

    /// <summary>
    /// <para>Gets player true raw.</para>
    /// <br>Attack addition is added as either Attack A or Attack B and is before or after Hunting Horn buffs respectively.</br>
    /// <br>Values that are known to reside in Attack B are Rush, Stylish Assault, Flash Conversion, Obscurity, Incitement, Rush, Vigorous Up and Partnyaa Attack Buffs.</br>
    /// <para>Final True  = ((Weapon True + Attack A) * HH Buff + Attack B) * Multipliers + Additional.</para>
    /// </summary>
    public string ATK
    {
        get
        {
            var weaponRaw = this.WeaponRaw();

            if (this.QuestID() == 0) // should work fine
            {
                this.HighestAtk = 0;
                this.HighestDPS = 0;
                this.HighestAttackMult = 0; // i get stackoverflow otherwise
                this.LowestMonsterDefrate = 1_000; // should be enough
            }

            if (weaponRaw > this.HighestAtk)
            {
                this.HighestAtk = weaponRaw;
            }

            if (this.DPS > this.HighestDPS)
            {
                this.HighestDPS = this.DPS;
            }

            if (double.TryParse(this.AtkMult, NumberStyles.Any, CultureInfo.InvariantCulture, out var atkMultResult))
            {
                if (atkMultResult > this.HighestAttackMult)
                {
                    this.HighestAttackMult = atkMultResult;
                }
            }
            else
            {
                LoggerInstance.Warn(CultureInfo.InvariantCulture, "Could not parse monster attack multiplier to double: {0}", this.AtkMult);
            }

            if (decimal.TryParse(this.DefMult, NumberStyles.Any, CultureInfo.InvariantCulture, out var defMultResult))
            {
                if (defMultResult < this.LowestMonsterDefrate && defMultResult != 0)
                {
                    this.LowestMonsterDefrate = defMultResult;
                }
            }
            else
            {
                LoggerInstance.Warn(CultureInfo.InvariantCulture, "Could not parse monster defense multiplier to decimal: {0}", this.DefMult);
            }

            return weaponRaw.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string DualSwordsSharpensColor
    {
        get
        {
            var count = this.DualSwordsSharpens();
            if (count >= 4)
            {
                this.DualSwordsSharpensIcon = "../../Assets/Icons/png/whetstone_red.png";
            }
            else
            {
                this.DualSwordsSharpensIcon = "../../Assets/Icons/png/whetstone.png";
            }

            return count switch
            {
                4 => "#cba6f7", // Mauve
                3 => "#f38ba8", // Red
                2 => "#fab387", // Peach    
                1 => "#f9e2af", // Yellow
                0 => "#f5e0dc", // Rosewater
                _ => "#cdd6f4",
            };
        }
    }

    /// <summary>
    /// Gets the color of the sharpness.
    /// </summary>
    /// <value>
    /// The color of the sharpness.
    /// </value>
    public string SharpnessColor
    {
        get
        {
            // see palettes.md
            var currentSharpnessLevel = this.SharpnessLevel();
            return currentSharpnessLevel switch
            {
                0 => "#c50f3a", // Red
                1 => "#e85218", // Orange
                2 => "#f3c832", // Yellow
                3 => "#5ed300", // Green
                4 => "#3068ee", // Blue
                5 => "#f0f0f0", // White
                6 => "#de7aff", // Purple
                7 => "#86f4f4", // Cyan
                _ => "#ffffff",
            };
        }
    }

    /// <summary>
    /// Gets the sharpness number.
    /// </summary>
    /// <value>
    /// The sharpness number.
    /// </value>
    public string SharpnessNumber
    {
        get
        {
            var currentSharpness = this.Sharpness();
            if (currentSharpness > 0)
            {
                return currentSharpness.ToString(CultureInfo.InvariantCulture) + this.SharpnessPercentNumber;
            }

            return "0" + this.SharpnessPercentNumber;
        }
    }

    public string DualSwordsSharpensCount
    {
        get
        {
            var count = this.DualSwordsSharpens();
            return count.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Gets the current weapon multiplier.
    /// </summary>
    /// <value>
    /// The current weapon multiplier.
    /// </value>
    public float CurrentWeaponMultiplier
    {
        get
        {
            var weaponType = this.WeaponType();
            return GetMultFromWeaponType(weaponType);
        }
    }

    /// <summary>
    /// Gets the name of the current weapon. This refers to the weapon type name.
    /// </summary>
    /// <value>
    /// The name of the current weapon.
    /// </value>
    public string CurrentWeaponName
    {
        get
        {
            var weaponType = this.WeaponType();
            return GetWeaponNameFromType(weaponType);
        }
    }

    /// <summary>
    /// Gets the size.
    /// </summary>
    /// <value>
    /// The size.
    /// </value>
    public string Size => this.SelectedMonster switch
    {
        0 => this.Monster1Size(),
        1 => this.Monster2Size(),
        _ => this.Monster1Size(),
    };

    /// <summary>
    /// Gets the monster atk mult.
    /// </summary>
    /// <value>
    /// The atk mult.
    /// </value>
    public string AtkMult => this.SelectedMonster switch
    {
        0 => this.Monster1AtkMult(),
        1 => this.Monster2AtkMult(),
        _ => this.Monster1AtkMult(),
    };

    /// <summary>
    /// Gets the defrate multiplier.
    /// </summary>
    /// <value>
    /// The defrate multiplier.
    /// </value>
    public string DefMult => this.SelectedMonster switch
    {
        0 => this.Monster1DefMult().ToString(CultureInfo.InvariantCulture),
        1 => this.Monster2DefMult().ToString(CultureInfo.InvariantCulture),
        _ => this.Monster1DefMult().ToString(CultureInfo.InvariantCulture),
    };

    /// <summary>
    /// Gets the current poison.
    /// </summary>
    public int PoisonCurrent
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1Poison(),
                1 => this.Monster2Poison(),
                _ => this.Monster1Poison(),
            };
        }
    }

    /// <summary>
    /// Gets the size.
    /// </summary>
    /// <value>
    /// The size.
    /// </value>
    /// <returns></returns>
    public double Monster1SizeMultForDictionary()
    {
        var success = double.TryParse(this.Monster1Size().Replace("%", string.Empty), NumberStyles.Number, CultureInfo.InvariantCulture, out var monster1SizePercent);
        var success2 = double.TryParse(this.Monster2Size().Replace("%", string.Empty), NumberStyles.Number, CultureInfo.InvariantCulture, out var monster2SizePercent);

        if (!(success && success2))
        {
            LoggerInstance.Warn("Could not parse monster sizes as double: {0} {1}", this.Monster1Size().Replace("%", string.Empty), this.Monster2Size().Replace("%", string.Empty));
            return 0.0;
        }

        return this.SelectedMonster switch
        {
            0 => monster1SizePercent,
            1 => monster2SizePercent,
            _ => monster1SizePercent,
        };
    }

    /// <summary>
    /// Gets the atk mult.
    /// </summary>
    /// <value>
    /// The atk mult.
    /// </value>
    /// <returns></returns>
    public double Monster1AttackMultForDictionary()
    {
        var success = double.TryParse(this.Monster1AtkMult(), NumberStyles.Number, CultureInfo.InvariantCulture, out var monster1AtkMultiplier);
        var success2 = double.TryParse(this.Monster2AtkMult(), NumberStyles.Number, CultureInfo.InvariantCulture, out var monster2AtkMultiplier);

        if (!(success && success2))
        {
            LoggerInstance.Warn("Could not parse monster attack multipliers as double: {0} {1}", this.Monster1AtkMult(), this.Monster2AtkMult());
            return 0.0;
        }

        return this.SelectedMonster switch
        {
            0 => monster1AtkMultiplier,
            1 => monster2AtkMultiplier,
            _ => monster1AtkMultiplier,
        };
    }

    /// <summary>
    /// Gets the defrate multiplier.
    /// </summary>
    /// <value>
    /// The defrate multiplier.
    /// </value>
    /// <returns></returns>
    public double Monster1DefMultForDictionary()
    {
        var success = double.TryParse(this.Monster1DefMult().ToString(CultureInfo.InvariantCulture), NumberStyles.Number, CultureInfo.InvariantCulture, out var monster1Defrate);
        var success2 = double.TryParse(this.Monster2DefMult().ToString(CultureInfo.InvariantCulture), NumberStyles.Number, CultureInfo.InvariantCulture, out var monster2Defrate);

        if (!(success && success2))
        {
            LoggerInstance.Warn("Could not parse monster defense multipliers as double: {0} {1}", this.Monster1DefMult().ToString(CultureInfo.InvariantCulture), this.Monster2DefMult().ToString(CultureInfo.InvariantCulture));
            return 0.0;
        }

        return this.SelectedMonster switch
        {
            0 => monster1Defrate,
            1 => monster2Defrate,
            _ => monster1Defrate,
        };
    }

    /// <summary>
    /// Displays the monster ehp.
    /// </summary>
    /// <param name="defrate">The defrate.</param>
    /// <param name="monsterhp">The monsterhp.</param>
    /// <param name="monsterdefrate">The monsterdefrate.</param>
    /// <returns></returns>
    public int DisplayMonsterEHP(int monsterSlot, decimal monsterDefrate, int monsterHP)
    {
        if (this.QuestID() == 0)
        {
            this.previousMonsterDefrate = decimal.Zero;
            return 0;
        }

        var s = (Settings)Application.Current.TryFindResource("Settings");
        if (s.EnableMonsterEHPDisplayCorrector)
        {
            if (s.MonsterEHPDisplayCorrectorDefrateMinimumThreshold >= s.MonsterEHPDisplayCorrectorDefrateMaximumThreshold)
            {
                switch (monsterSlot)
                {
                    case 1:
                        this.Monster1HPModeText = "THP";
                        break;
                    case 2:
                        this.Monster2HPModeText = "THP";
                        break;
                    case 3:
                        this.Monster3HPModeText = "THP";
                        break;
                    case 4:
                        this.Monster4HPModeText = "THP";
                        break;
                    default:
                        break;
                }

                return monsterHP;
            }

            if (monsterDefrate > s.MonsterEHPDisplayCorrectorDefrateMinimumThreshold && monsterDefrate < s.MonsterEHPDisplayCorrectorDefrateMaximumThreshold)
            {
                this.previousMonsterDefrate = monsterDefrate;
                var result = Convert.ToDecimal(monsterHP / this.previousMonsterDefrate);
                if (result is <= int.MaxValue and >= int.MinValue)
                {
                    switch (monsterSlot)
                    {
                        case 1:
                            this.Monster1HPModeText = "EHP";
                            break;
                        case 2:
                            this.Monster2HPModeText = "EHP";
                            break;
                        case 3:
                            this.Monster3HPModeText = "THP";
                            break;
                        case 4:
                            this.Monster4HPModeText = "THP";
                            break;
                        default:
                            break;
                    }

                    return Convert.ToInt32(result);
                }
                else
                {
                    switch (monsterSlot)
                    {
                        case 1:
                            this.Monster1HPModeText = "THP";
                            break;
                        case 2:
                            this.Monster2HPModeText = "THP";
                            break;
                        case 3:
                            this.Monster3HPModeText = "THP";
                            break;
                        case 4:
                            this.Monster4HPModeText = "THP";
                            break;
                        default:
                            break;
                    }

                    return monsterHP;
                }
            }
            else
            {
                if (this.previousMonsterDefrate > 0)
                {
                    var result = Convert.ToDecimal(monsterHP / this.previousMonsterDefrate);

                    if (result is <= int.MaxValue and >= int.MinValue)
                    {
                        switch (monsterSlot)
                        {
                            case 1:
                                this.Monster1HPModeText = "EHP";
                                break;
                            case 2:
                                this.Monster2HPModeText = "EHP";
                                break;
                            case 3:
                                this.Monster3HPModeText = "THP";
                                break;
                            case 4:
                                this.Monster4HPModeText = "THP";
                                break;
                            default:
                                break;
                        }

                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        switch (monsterSlot)
                        {
                            case 1:
                                this.Monster1HPModeText = "THP";
                                break;
                            case 2:
                                this.Monster2HPModeText = "THP";
                                break;
                            case 3:
                                this.Monster3HPModeText = "THP";
                                break;
                            case 4:
                                this.Monster4HPModeText = "THP";
                                break;
                            default:
                                break;
                        }

                        return monsterHP;
                    }
                }
                else
                {
                    switch (monsterSlot)
                    {
                        case 1:
                            this.Monster1HPModeText = "THP";
                            break;
                        case 2:
                            this.Monster2HPModeText = "THP";
                            break;
                        case 3:
                            this.Monster3HPModeText = "THP";
                            break;
                        case 4:
                            this.Monster4HPModeText = "THP";
                            break;
                        default:
                            break;
                    }

                    return monsterHP;
                }
            }
        }
        else
        {
            if (monsterDefrate > 0)
            {
                var result = Convert.ToDecimal(monsterHP / monsterDefrate);

                if (result is <= int.MaxValue and >= int.MinValue)
                {
                    switch (monsterSlot)
                    {
                        case 1:
                            this.Monster1HPModeText = "EHP";
                            break;
                        case 2:
                            this.Monster2HPModeText = "EHP";
                            break;
                        case 3:
                            this.Monster3HPModeText = "THP";
                            break;
                        case 4:
                            this.Monster4HPModeText = "THP";
                            break;
                        default:
                            break;
                    }

                    return Convert.ToInt32(result);
                }
                else
                {
                    return 0;

                    // Handle the case where the result is too large or too small for an int
                    // Return an appropriate value or throw an exception if necessary
                }
            }
        }

        return 0;
    }

    /// <summary>
    /// Reloads the maximum ehp.
    /// </summary>
    public void ReloadMaxEHP()
    {
        if (this.savedMonster1MaxHP < this.Monster1HPInt())
        {
            this.savedMonster1MaxHP = (int)(this.Monster1HPInt() / this.Monster1DefMult());
        }

        if (this.savedMonster2MaxHP < this.Monster2HPInt())
        {
            this.savedMonster2MaxHP = (int)(this.Monster2HPInt() / this.Monster2DefMult());
        }

        if (this.savedMonster3MaxHP < this.Monster3HPInt())
        {
            this.savedMonster3MaxHP = (int)(this.Monster3HPInt() / float.Parse("1", CultureInfo.InvariantCulture.NumberFormat));
        }

        if (this.savedMonster4MaxHP < this.Monster4HPInt())
        {
            this.savedMonster4MaxHP = (int)(this.Monster4HPInt() / float.Parse("1", CultureInfo.InvariantCulture.NumberFormat));
        }
    }

    /// <summary>
    /// Gets the poison maximum.
    /// </summary>
    /// <value>
    /// The poison maximum.
    /// </value>
    public int PoisonMax
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1PoisonNeed(),
                1 => this.Monster2PoisonNeed(),
                _ => this.Monster1PoisonNeed(),
            };
        }
    }

    /// <summary>
    /// Gets the current sleep.
    /// </summary>
    public int SleepCurrent
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1Sleep(),
                1 => this.Monster2Sleep(),
                _ => this.Monster1Sleep(),
            };
        }
    }

    /// <summary>
    /// Gets the sleep maximum.
    /// </summary>
    /// <value>
    /// The sleep maximum.
    /// </value>
    public int SleepMax
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1SleepNeed(),
                1 => this.Monster2SleepNeed(),
                _ => this.Monster1SleepNeed(),
            };
        }
    }

    /// <summary>
    /// Gets the current paralysis.
    /// </summary>
    public int ParaCurrent
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1Para(),
                1 => this.Monster2Para(),
                _ => this.Monster1Para(),
            };
        }
    }

    /// <summary>
    /// Gets the para maximum.
    /// </summary>
    /// <value>
    /// The para maximum.
    /// </value>
    public int ParaMax
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1ParaNeed(),
                1 => this.Monster2ParaNeed(),
                _ => this.Monster1ParaNeed(),
            };
        }
    }

    /// <summary>
    /// Gets the current blast.
    /// </summary>
    public int BlastCurrent
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1Blast(),
                1 => this.Monster2Blast(),
                _ => this.Monster1Blast(),
            };
        }
    }

    /// <summary>
    /// Gets the blast maximum.
    /// </summary>
    /// <value>
    /// The blast maximum.
    /// </value>
    public int BlastMax
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1BlastNeed(),
                1 => this.Monster2BlastNeed(),
                _ => this.Monster1BlastNeed(),
            };
        }
    }

    /// <summary>
    /// Gets the current stun.
    /// </summary>
    public int StunCurrent
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1Stun(),
                1 => this.Monster2Stun(),
                _ => this.Monster1Stun(),
            };
        }
    }

    /// <summary>
    /// Gets the stun maximum.
    /// </summary>
    /// <value>
    /// The stun maximum.
    /// </value>
    public int StunMax
    {
        get
        {
            if (this.Configuring)
            {
                return 100;
            }

            return this.SelectedMonster switch
            {
                0 => this.Monster1StunNeed(),
                1 => this.Monster2StunNeed(),
                _ => this.Monster1StunNeed(),
            };
        }
    }

    public string Monster1Name => this.GetDureName() != "None" ? this.GetDureName() : this.GetRaviName() != "None" ? this.GetRaviName() : this.GetMonsterName(this.GetNotRoad() || this.RoadSelectedMonster() == 0 ? this.LargeMonster1ID() : this.LargeMonster2ID()); // monster 1 is used for the first display and road uses 2nd choice to store 2nd monster

    /// <summary>
    /// Gets the current poison.
    /// </summary>
    /// <returns></returns>
    public int Monster1PoisonForDictionary() => this.SelectedMonster switch
    {
        0 => this.Monster1Poison(),
        1 => this.Monster2Poison(),
        _ => this.Monster1Poison(),
    };

    /// <summary>
    /// Gets the current sleep.
    /// </summary>
    /// <returns></returns>
    public int Monster1SleepForDictionary() => this.SelectedMonster switch
    {
        0 => this.Monster1Sleep(),
        1 => this.Monster2Sleep(),
        _ => this.Monster1Sleep(),
    };

    /// <summary>
    /// Gets the current paralysis.
    /// </summary>
    /// <returns></returns>
    public int Monster1ParalysisForDictionary() => this.SelectedMonster switch
    {
        0 => this.Monster1Para(),
        1 => this.Monster2Para(),
        _ => this.Monster1Para(),
    };

    /// <summary>
    /// Gets the current blast.
    /// </summary>
    /// <returns></returns>
    public int Monster1BlastForDictionary() => this.SelectedMonster switch
    {
        0 => this.Monster1Blast(),
        1 => this.Monster2Blast(),
        _ => this.Monster1Blast(),
    };

    /// <summary>
    /// Gets the current stun.
    /// </summary>
    /// <returns></returns>
    public int Monster1StunForDictionary() => this.SelectedMonster switch
    {
        0 => this.Monster1Stun(),
        1 => this.Monster2Stun(),
        _ => this.Monster1Stun(),
    };

    /// <summary>
    /// Gets the name of Duremudira.
    /// </summary>
    /// <returns></returns>
    public string GetDureName()
    {
        if (this.QuestID() is 21731 or 21749)
        {
            return "1st District Duremudira";
        }
        else if (this.QuestID() is 21746 or 21750)
        {
            return "2nd District Duremudira";
        }
        else if (this.QuestID() is 21747 or 21734)
        {
            return "3rd District Duremudira";
        }
        else if (this.QuestID() == 21748)
        {
            return "4th District Duremudira";
        }
        else if (this.QuestID() is 23648 or 23649)
        {
            return "Arrogant Duremudira";
        }
        else
        {
            return "None";
        }
    }

    public string GetDureName(int questID)
    {
        if (questID is 21731 or 21749)
        {
            return "1st District Duremudira";
        }
        else if (questID is 21746 or 21750)
        {
            return "2nd District Duremudira";
        }
        else if (questID is 21747 or 21734)
        {
            return "3rd District Duremudira";
        }
        else if (questID == 21748)
        {
            return "4th District Duremudira";
        }
        else if (questID is 23648 or 23649)
        {
            return "Arrogant Duremudira";
        }
        else
        {
            return "None";
        }
    }

    // quest ids
    // ravi 62105 TODO: same ids in all phases?
    // violent 62101
    // berserk
    // berserk practice
    // support 1 55803
    // extreme

    /// <summary>
    /// Gets the name of the ravi.
    /// </summary>
    /// <returns></returns>
    public string GetRaviName()
    {
        // quest ids:
        // mp road: 23527
        // solo road: 23628
        // 1st district dure: 21731
        // 2nd district dure: 21746
        // 1st district dure sky corridor: 21749
        // 2nd district dure sky corridor: 21750
        // arrogant dure repel: 23648
        // arrogant dure slay: 23649
        // urgent tower: 21751
        // 4th district dure: 21748
        // 3rd district dure: 21747
        // 3rd district dure 2: 21734
        // UNUSED sky corridor: 21730
        // sky corridor prologue: 21729
        // raviente 62105
        // raviente carve 62108
        // violent raviente 62101
        // violent carve 62104
        // berserk slay practice 55796
        // berserk support practice 1 55802
        // berserk support practice 2 55803
        // berserk support practice 3 55804
        // berserk support practice 4 55805
        // berserk support practice 5 55806
        // berserk practice carve 55807
        // berserk slay  54751
        // berserk support 1 54756
        // berserk support 2 54757
        // berserk support 3 54758
        // berserk support 4 54759
        // berserk support 5 54760
        // berserk carve 54761
        // extreme slay (musou table 54) 55596
        // extreme support 1 55602
        // extreme support 2 55603
        // extreme support 3 55604
        // extreme support 4 55605
        // extreme support 5 55606
        // extreme carve 55607
        if (this.QuestID() is 62105 or 62108)
        {
            return "Raviente";
        }
        else if (this.QuestID() is 62101 or 62104)
        {
            return "Violent Raviente";
        }
        else if (this.QuestID() is 55796 or 55802 or 55803 or 55804 or 55805 or 55806 or 55807)
        {
            return "Berserk Raviente Practice";
        }
        else if (this.QuestID() is 54751 or 54756 or 54757 or 54758 or 54759 or 54760 or 54761)
        {
            return "Berserk Raviente";
        }
        else if (this.QuestID() is 55596 or 55602 or 55603 or 55604 or 55605 or 55606 or 55607)
        {
            return "Extreme Raviente";
        }
        else
        {
            return "None";
        }
    }

    public string Monster2Name => this.AlternativeQuestOverride() ? this.GetMonsterName(this.AlternativeQuestMonster2ID(), false) : this.GetMonsterName(this.LargeMonster2ID(), false);

    public string Monster3Name => this.GetMonsterName(this.LargeMonster3ID(), false);

    public string Monster4Name => this.GetMonsterName(this.LargeMonster4ID(), false);

    /// <summary>
    /// Gets the real name of the monster.
    /// </summary>
    /// <value>
    /// The real name of the monster.
    /// </value>
    public string RealMonsterName
    {
        get
        {
            // https://stackoverflow.com/questions/4315564/capitalizing-words-in-a-string-using-c-sharp
            int id;

            if (this.RoadOverride() == false)
            {
                id = this.RoadSelectedMonster() == 0 ? this.LargeMonster1ID() : this.LargeMonster2ID();
            }
            else if (this.AlternativeQuestOverride())
            {
                id = this.AlternativeQuestMonster1ID();
            }
            else
            {
                id = this.LargeMonster1ID();
            }

            // dure
            if (this.QuestID() is 21731 or 21746 or 21749 or 21750)
            {
                return "Duremudira";
            }
            else if (this.QuestID() is 23648 or 23649)
            {
                return "Arrogant Duremudira";
            }

            return this.DetermineMonsterName(id);
        }

        // quest ids:
        // mp road: 23527
        // solo road: 23628
        // 1st district dure: 21731
        // 2nd district dure: 21746
        // 1st district dure sky corridor: 21749
        // 2nd district dure sky corridor: 21750
        // arrogant dure repel: 23648
        // arrogant dure slay: 23649
        // urgent tower: 21751
        // 4th district dure: 21748
        // 3rd district dure: 21747
        // 3rd district dure 2: 21734
        // UNUSED sky corridor: 21730
        // sky corridor prologue: 21729
    }

    public string Monster1HP
    {
        get
        {
            if (this.Configuring)
            {
                return "0";
            }
            else if (ShowMonsterEHP())
            {
                var monsterDefMultiplier = this.Monster1DefMult();
                var monsterHP = this.Monster1HPInt();
                return this.DisplayMonsterEHP(1, monsterDefMultiplier, monsterHP).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                var monsterHP = this.Monster1HPInt();
                this.Monster1HPModeText = "THP";
                return monsterHP.ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    /// <summary>
    /// Gets the name of the rank.TODO: In database the spaces are there.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public string GetRankName(int id) => id switch
    {
        0 => string.Empty,
        1 or 2 or 3 or 4 or 5 or 6 or 7 or 8 or 9 or 10 => "Low Rank ",
        11 => "Low/High Rank ",
        12 or 13 or 14 or 15 or 16 or 17 or 18 or 19 or 20 => "High Rank ",
        26 or 31 or 42 => "HR5 ",
        32 or 46 => "Supremacy ",

        // : conquest levels via quest id
        53 => this.QuestID() switch
        {
            23585 or 23589 or 23593 or 23597 => "Lv1 ",
            23586 or 23590 or 23594 or 23598 => "Lv200 ",
            23587 or 23591 or 23595 or 23599 => "Lv1000 ",
            23588 or 23592 or 23596 or 23601 => "Lv9999 ",
            _ => "G Rank ",
        }, // shantien

        // lv1 23585
        // lv200 23586
        // lv1000 23587
        // lv9999 23588
        // disufiroa
        // lv1 23589
        // lv200 23590
        // lv1000 23591
        // lv9999 23592
        // fatalis
        // lv1 23593
        // lv200 23594
        // lv1000 23595
        // lv9999 23596
        // crimson fatalis
        // lv1 23597
        // lv200 23598
        // lv1000 23599
        // lv9999 23601
        // upper shiten unknown 23605
        // lower shiten unknown 23604
        // upper shiten disufiroa 23603
        // lower shiten disu 23602
        // thirsty 55532
        // shifting 55920
        // starving 55916
        // golden 55917
        54 => this.QuestID() switch
        {
            23604 or 23602 => "Lower Shiten ",
            _ => string.Empty,
        },
        55 => this.QuestID() switch
        {
            23603 => "Upper Shiten ",
            _ => string.Empty,
        },

        // 10m upper shiten/musou true slay
        // twinhead rajang / voljang and rajang
        56 or 57 => "Twinhead ",
        64 => "Zenith★1 ",
        65 => "Zenith★2 ",
        66 => "Zenith★3 ",
        67 => "Zenith★4 ",

        // unknown
        70 => "Upper Shiten ",
        71 or 72 or 73 => "Interception ",
        _ => string.Empty,
    };

    public string DetermineMonsterName(int id)
    {
        var keyFound = MonsterNames.MonsterNameID.TryGetValue(id, out var link);
        link ??= "Loading...";

        if (keyFound)
        {
            return link;
        }
        else
        {
            return this.GetAlternateMonsterName(id);
        }
    }

    public string GetAlternateMonsterName(int id)
    {
        switch (id)
        {
            case 65:
                return "Teostra";
            case 89:
                if (this.RankBand() == 54)
                {
                    return "Thirsty Pariapuria";
                }
                else
                {
                    return "Pariapuria";
                }

            case 95:
                return "Doragyurosu";
            case 106:
                return "Odibatorasu";
            case 113:
                if (this.RankBand() == 55)
                {
                    return "Shifting Mi Ru";
                }
                else
                {
                    return "Mi Ru";
                }

            case 146:
                if (this.RankBand() is >= 54 and <= 55)
                {
                    return "Howling Zinogre";
                }
                else
                {
                    return "Zinogre";
                }

            case 154:
                if (this.RankBand() is >= 54 and <= 55)
                {
                    return "Ruling Guanzorumu";
                }
                else
                {
                    return "Guanzorumu";
                }

            case 155:
                if (this.RankBand() == 55)
                {
                    return "Golden Deviljho";
                }
                else
                {
                    return "Starving Deviljho";
                }

            case 166:
                if (this.RankBand() is >= 54 and <= 55)
                {
                    return "Burning Freezing Elzelion";
                }
                else
                {
                    return "Elzelion";
                }

            default:
                return "Loading...";
        }
    }

    /// <summary>
    /// Gets the name of the monster.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public string GetMonsterName(int id, bool isFirstMonster = true)
    {
        if (this.Configuring)
        {
            return "Bombardier Bogabadorumu";
        }

        if (id == 0)
        {
            return string.Empty;
        }

        Monsters.MonsterID.TryGetValue(id, out var monstername);

        if (monstername != null && monstername != this.RealMonsterName && isFirstMonster)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", this.GetRankName(this.RankBand()), this.RealMonsterName);
        }
        else
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", this.GetRankName(this.RankBand()), monstername);
        }
    }

    public string Monster1MaxHP
    {
        get
        {
            if (this.Configuring)
            {
                return "1";
            }

            if (this.TimeDefInt() == this.TimeInt())
            {
                this.savedMonster1MaxHP = this.Monster1HPInt();
            }

            if (this.LargeMonster1ID() > 0 && this.savedMonster1ID == 0)
            {
                this.savedMonster1MaxHP = this.Monster1HPInt();
                this.savedMonster1ID = this.LargeMonster1ID();
            }

            if (this.savedMonster1ID > 0)
            {
                this.savedMonster1ID = this.LargeMonster1ID();
            }

            if (this.GetNotRoad() || this.RoadSelectedMonster() == 0)
            {
                return this.savedMonster1MaxHP.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return this.Monster2MaxHP;
            }
        }
    }

    public string Monster2HP
    {
        get
        {
            if (this.Configuring)
            {
                return "0";
            }
            else
            {
                if (ShowMonsterEHP())
                {
                    return this.DisplayMonsterEHP(2, this.Monster2DefMult(), this.Monster2HPInt()).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    this.Monster2HPModeText = "THP";
                    return this.Monster2HPInt().ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }

    public string Monster2MaxHP
    {
        get
        {
            if (this.Configuring)
            {
                return "1";
            }

            if (this.TimeDefInt() == this.TimeInt())
            {
                this.savedMonster2MaxHP = this.Monster2HPInt();
            }

            if (this.RoadSelectedMonster() > 0 && this.savedMonster2ID == 0)
            {
                this.savedMonster2MaxHP = this.Monster2HPInt();
                this.savedMonster2ID = this.RoadSelectedMonster();
            }

            if (this.savedMonster2ID > 0)
            {
                this.savedMonster2ID = this.RoadSelectedMonster();
            }

            return this.savedMonster2MaxHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster3HP => this.Configuring ?
        "0" :
        ShowMonsterEHP() ?
            this.DisplayMonsterEHP(3, 1, this.Monster3HPInt()).ToString(CultureInfo.InvariantCulture) :
            this.Monster3HPInt().ToString(CultureInfo.InvariantCulture);

    public string Monster3MaxHP
    {
        get
        {
            if (this.Configuring)
            {
                return "1";
            }

            if (this.TimeDefInt() == this.TimeInt())
            {
                this.savedMonster3MaxHP = this.Monster3HPInt();
            }

            return this.savedMonster3MaxHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string Monster4HP => this.Configuring ?
        "0" :
        ShowMonsterEHP() ?
            this.DisplayMonsterEHP(4, 1, this.Monster4HPInt()).ToString(CultureInfo.InvariantCulture) :
            this.Monster4HPInt().ToString(CultureInfo.InvariantCulture);

    public string Monster4MaxHP
    {
        get
        {
            if (this.Configuring)
            {
                return "1";
            }

            if (this.TimeDefInt() == this.TimeInt())
            {
                this.savedMonster4MaxHP = this.Monster4HPInt();
            }

            return this.savedMonster4MaxHP.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Gets the current monster1 icon.
    /// </summary>
    /// <value>
    /// The current monster1 icon.
    /// </value>
    public string CurrentMonster1Icon
    {
        get
        {
            int id;

            if (this.RoadOverride() == false)
            {
                id = this.RoadSelectedMonster() == 0 ? this.LargeMonster1ID() : this.LargeMonster2ID();
            }
            else if (this.AlternativeQuestOverride())
            {
                id = this.AlternativeQuestMonster1ID();
            }
            else
            {
                id = this.LargeMonster1ID();
            }

            // dure
            if (this.QuestID() is 21731 or 21746 or 21749 or 21750)
            {
                id = 132;
            }
            else if (this.QuestID() is 23648 or 23649)
            {
                id = 167;
            }

            return this.DetermineMonsterImage(id);
        }
    }

    /*  Multipliers
        Sword and Shield 單手劍 片手剣 1.4x
        Dual Swords 雙劍 双剣 1.4x
        Great Sword 大劍 大剣 4.8x
        Long Sword 太刀 太刀 4.8x
        Hammer 大錘 ハンマー 5.2x
        Hunting Horn 狩獵笛 狩猟笛 5.2x
        Lance 長槍 ランス 2.3x
        Gunlance 銃槍 ガンランス 2.3x
        Tonfa 穿龍棍 穿龍棍 1.8x
        Switch Axe F 斬擊斧Ｆ スラッシュアックスF 5.4x
        Magnet Spike 磁斬鎚 マグネットスパイク 5.4x
        Heavy Bowgun 重銃 ヘビィボウガン 1.2x
        Light Bowgun 輕弩 ライトボウガン 1.2x
        Bow 弓 弓 1.2x
        IDs
        0    Great Sword
        1    Heavy Bowgun
        2    Hammer
        3    Lance
        4    Sword and Shield
        5    Light Bowgun
        6    Dual Swords
        7    Long Sword
        8    Hunting Horn
        9    Gunlance
        10    Bow
        11    Tonfa
        12    Switch Axe F
        13    Magnet Spike
        14    Group
    */

    /// <summary>
    /// Gets the multiplier from weapon type.
    /// </summary>
    /// <param name="weaponType">Type of the weapon.</param>
    /// <returns></returns>
    public static float GetMultFromWeaponType(int weaponType) => weaponType switch
    {
        0 or 7 => 4.8f,
        4 or 6 => 1.4f,
        2 or 8 => 5.2f,
        12 or 13 => 5.4f,
        3 or 9 => 2.3f,
        1 or 5 or 10 => 1.2f,
        11 => 1.8f,
        _ => 1f,
    };

    // TODO: EZlion

    /// <summary>
    /// Gets the name of the weapon type.
    /// </summary>
    /// <param name="weaponType">Type of the weapon.</param>
    /// <returns></returns>
    public static string GetWeaponNameFromType(int weaponType) => weaponType switch
    {
        0 => "Great Sword",
        1 => "Heavy Bowgun",
        2 => "Hammer",
        3 => "Lance",
        4 => "Sword and Shield",
        5 => "Light Bowgun",
        6 => "Dual Swords",
        7 => "Long Sword",
        8 => "Hunting Horn",
        9 => "Gunlance",
        10 => "Bow",
        11 => "Tonfa",
        12 => "Switch Axe F",
        13 => "Magnet Spike",
        14 => "Group",
        _ => string.Empty,
    };

    public string CurrentMonster1Image
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            var monsterIcon = this.CurrentMonster1Icon.Replace("https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/");

            if (s.EnableMonsterRenders)
            {
                var renderFound = MonsterRenders.MonsterRender.ContainsKey(monsterIcon);
                if (renderFound)
                {
                    return MonsterRenders.MonsterRender[monsterIcon];
                }
                else
                {
                    return Messages.UnknownMonsterIcon;
                }
            }
            else
            {
                return monsterIcon;
            }
        }
    }

    public string CurrentMonster2Image
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            var monsterIcon = this.CurrentMonster2Icon.Replace("https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/");

            if (s.EnableMonsterRenders)
            {
                var renderFound = MonsterRenders.MonsterRender.ContainsKey(monsterIcon);
                if (renderFound)
                {
                    return MonsterRenders.MonsterRender[monsterIcon];
                }
                else
                {
                    return Messages.UnknownMonsterIcon;
                }
            }
            else
            {
                return monsterIcon;
            }
        }
    }

    public string CurrentMonster3Image
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            var monsterIcon = this.CurrentMonster3Icon.Replace("https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/");

            if (s.EnableMonsterRenders)
            {
                var renderFound = MonsterRenders.MonsterRender.ContainsKey(monsterIcon);
                if (renderFound)
                {
                    return MonsterRenders.MonsterRender[monsterIcon];
                }
                else
                {
                    return Messages.UnknownMonsterIcon;
                }
            }
            else
            {
                return monsterIcon;
            }
        }
    }

    public string CurrentMonster4Image
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            var monsterIcon = this.CurrentMonster4Icon.Replace("https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/", @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/");

            if (s.EnableMonsterRenders)
            {
                var renderFound = MonsterRenders.MonsterRender.ContainsKey(monsterIcon);
                if (renderFound)
                {
                    return MonsterRenders.MonsterRender[monsterIcon];
                }
                else
                {
                    return Messages.UnknownMonsterIcon;
                }
            }
            else
            {
                return monsterIcon;
            }
        }
    }

    public string CurrentMonster2Icon
    {
        get
        {
            int id;

            if (this.RoadOverride() == false)
            {
                id = this.RoadSelectedMonster() == 0 ? this.LargeMonster2ID() : this.LargeMonster1ID();
            }
            else if (this.AlternativeQuestOverride())
            {
                id = this.AlternativeQuestMonster2ID();
            }
            else
            {
                id = this.LargeMonster2ID();
            }

            // dure
            if (this.QuestID() is 21731 or 21746 or 21749 or 21750)
            {
                id = 132;
            }
            else if (this.QuestID() is 23648 or 23649)
            {
                id = 167;
            }

            return this.DetermineMonsterImage(id);
        }
    }

    public string CurrentMonster3Icon
    {
        get
        {
            var id = this.LargeMonster3ID();

            // dure
            if (this.QuestID() is 21731 or 21746 or 21749 or 21750)
            {
                id = 132;
            }
            else if (this.QuestID() is 23648 or 23649)
            {
                id = 167;
            }

            return this.DetermineMonsterImage(id);
        }
    }

    public string CurrentMonster4Icon
    {
        get
        {
            var id = this.LargeMonster4ID();

            // dure
            if (this.QuestID() is 21731 or 21746 or 21749 or 21750)
            {
                id = 132;
            }
            else if (this.QuestID() is 23648 or 23649)
            {
                id = 167;
            }

            return this.DetermineMonsterImage(id);
        }
    }

    public string Monster1HPBarColor => this.DetermineMonsterHPBarColor(1);

    public string DetermineMonsterImage(int id, bool forDiscord = false)
    {
        var keyFound = false;

        if (forDiscord)
        {
            keyFound = MonsterImagesDiscord.MonsterImageID.TryGetValue(id, out var link);
            link ??= Messages.UnknownMonsterIcon;

            if (keyFound)
            {
                return link;
            }
            else
            {
                return this.GetAlternateMonsterImage(id, forDiscord);
            }
        }
        else
        {
            keyFound = MonsterImages.MonsterImageID.TryGetValue(id, out var link);
            link ??= Messages.UnknownMonsterIcon;

            if (keyFound)
            {
                return link;
            }
            else
            {
                return this.GetAlternateMonsterImage(id);
            }
        }
    }

    public string DetermineMonsterHPBarColor(int n)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        var barColorSetting = "#1e1e2e";
        var monsterID = 0;

        switch (n)
        {
            case 1:
                barColorSetting = s.Monster1BarColor;
                monsterID = this.LargeMonster1ID();
                break;
            case 2:
                barColorSetting = s.Monster2BarColor;
                monsterID = this.LargeMonster2ID();
                break;
            case 3:
                barColorSetting = s.Monster3BarColor;
                monsterID = this.LargeMonster3ID();
                break;
            case 4:
                barColorSetting = s.Monster4BarColor;
                monsterID = this.LargeMonster4ID();
                break;
            default:
                break;
        }

        var keyFound = MonsterColors.MonsterColorID.TryGetValue(monsterID, out var color);
        color ??= barColorSetting;

        if (keyFound && s.EnableMonsterHPBarsAutomaticColor)
        {
            return color;
        }
        else
        {
            return barColorSetting;
        }
    }

    public string DetermineMonsterStrokeColor(int n)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        var barColor = this.DetermineMonsterHPBarColor(n);
        var strokeColorSetting = string.Empty;
        var color = string.Empty;

        color = barColor switch
        {
            // Surface0
            "#313244" or "#45475a" or "#585b70" or "#1e1e2e" or "#181825" or "#11111b" => "#f5e0dc", // Rosewater
            _ => "#1e1e2e", // Base
        };
        switch (n)
        {
            case 1:
                strokeColorSetting = s.Monster1BarStrokeColor;
                break;
            case 2:
                strokeColorSetting = s.Monster2BarStrokeColor;
                break;
            case 3:
                strokeColorSetting = s.Monster3BarStrokeColor;
                break;
            case 4:
                strokeColorSetting = s.Monster4BarStrokeColor;
                break;
            default:
                break;
        }

        if (s.EnableMonsterHPBarsAutomaticColor)
        {
            return color;
        }
        else
        {
            return strokeColorSetting;
        }
    }

    public string DetermineMonsterBorderColor(int n)
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        var barColor = this.DetermineMonsterHPBarColor(n);
        var colorSetting = string.Empty;
        var color = string.Empty;

        color = barColor switch
        {
            // Surface0
            "#313244" or "#45475a" or "#585b70" or "#1e1e2e" or "#181825" or "#11111b" => "#f5e0dc", // Rosewater
            _ => "#11111b", // Crust
        };
        switch (n)
        {
            case 1:
                colorSetting = s.Monster1BarBorderColor;
                break;
            case 2:
                colorSetting = s.Monster2BarBorderColor;
                break;
            case 3:
                colorSetting = s.Monster3BarBorderColor;
                break;
            case 4:
                colorSetting = s.Monster4BarBorderColor;
                break;
            default:
                break;
        }

        if (s.EnableMonsterHPBarsAutomaticColor)
        {
            return color;
        }
        else
        {
            return colorSetting;
        }
    }

    public string DetermineQuestToggleMonsterModeSelected(int mode)
    {
        return mode switch
        {
            (int)QuestToggleMonsterModeOption.Normal => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
            (int)QuestToggleMonsterModeOption.Hardcore => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/flame_hc.png",
            (int)QuestToggleMonsterModeOption.Unlimited => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/flame_ul.png",
            _ => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
        };
    }

    public string Monster2HPBarColor => this.DetermineMonsterHPBarColor(2);

    public string Monster3HPBarColor => this.DetermineMonsterHPBarColor(3);

    public string Monster4HPBarColor => this.DetermineMonsterHPBarColor(4);

    public string Monster1StrokeColor => this.DetermineMonsterStrokeColor(1);

    public string Monster2StrokeColor => this.DetermineMonsterStrokeColor(2);

    public string Monster3StrokeColor => this.DetermineMonsterStrokeColor(3);

    public string Monster4StrokeColor => this.DetermineMonsterStrokeColor(4);

    public string Monster1BarBorderColor => this.DetermineMonsterBorderColor(1);

    public string Monster2BarBorderColor => this.DetermineMonsterBorderColor(2);

    public string Monster3BarBorderColor => this.DetermineMonsterBorderColor(3);

    public string Monster4BarBorderColor => this.DetermineMonsterBorderColor(4);

    public string Monster1HPModeText { get; set; } = string.Empty;

    public string Monster2HPModeText { get; set; } = string.Empty;

    // TODO defrate addresses
    public string Monster3HPModeText { get; set; } = "THP";

    public string Monster4HPModeText { get; set; } = "THP";

    public string QuestToggleModeSelected => this.DetermineQuestToggleMonsterModeSelected(QuestToggleMonsterMode());

    public string QuestToggleModeSelectedShown
    {
        get
        {
            var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

            if (s.QuestToggleMonsterModeShown && (this.QuestToggleMonsterMode() is (int)QuestToggleMonsterModeOption.Hardcore or (int)QuestToggleMonsterModeOption.Unlimited))
            {
                return "Visible";
            }
            else {
                return "Collapsed";
            }
        }
    }

    public string CurrentMap
    {
        get
        {
            var id = this.AreaID();
            return GetGatheringMapFromID(id);
        }
    }

    /// <summary>
    /// Gets the name of the weapon.
    /// </summary>
    /// <value>
    /// The name of the weapon.
    /// </value>
    public string GetRealWeaponName
    {
        get
        {
            var className = this.GetWeaponClass();
            var lv = GetWeaponNameFromType(this.WeaponType()).Contains("Bowgun") ? GetGRWeaponLevel(this.GRWeaponLvBowguns()) : GetGRWeaponLevel(this.GRWeaponLv());

            if (GetTextFormat() == "Markdown" && (lv == " Lv. 50" || lv == " Lv. 100"))
            {
                lv = string.Format(CultureInfo.InvariantCulture, "**{0}**", lv);
            }

            var style = this.WeaponStyle() switch
            {
                0 => "Earth Style",
                1 => "Heaven Style",
                2 => "Storm Style",
                3 => "Extreme Style",
                _ => "Earth Style"
            };

            if (className == "Blademaster")
            {
                WeaponBlademaster.IDName.TryGetValue(this.BlademasterWeaponID(), out var wepname);
                var address = this.BlademasterWeaponID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();  // gives you hex 4 digit "007B"

                return string.Format(CultureInfo.InvariantCulture, "{0}{1} ({2}) | {3}\n{4} | {5} | {6}", wepname, lv, address, style, this.GetDecoName(this.WeaponDeco1ID(), EquipmentSlot.One), this.GetDecoName(this.WeaponDeco2ID(), EquipmentSlot.Two), this.GetDecoName(this.WeaponDeco3ID(), EquipmentSlot.Three));
            }
            else if (className == "Gunner")
            {
                WeaponGunner.IDName.TryGetValue(this.GunnerWeaponID(), out var wepname);
                var address = this.GunnerWeaponID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();  // gives you hex 4 digit "007B"
                return string.Format(CultureInfo.InvariantCulture, "{0}{1} ({2}) | {3}\n{4} | {5} | {6}", wepname, lv, address, style, this.GetDecoName(this.WeaponDeco1ID(), EquipmentSlot.One), this.GetDecoName(this.WeaponDeco2ID(), EquipmentSlot.Two), this.GetDecoName(this.WeaponDeco3ID(), EquipmentSlot.Three));
            }
            else
            {
                return "None";
            }
        }
    }

    public string GetAmmoPouch
    {
        get
        {
            var item1 = this.AmmoPouchItem1ID();
            var item2 = this.AmmoPouchItem2ID();
            var item3 = this.AmmoPouchItem3ID();
            var item4 = this.AmmoPouchItem4ID();
            var item5 = this.AmmoPouchItem5ID();
            var item6 = this.AmmoPouchItem6ID();
            var item7 = this.AmmoPouchItem7ID();
            var item8 = this.AmmoPouchItem8ID();
            var item9 = this.AmmoPouchItem9ID();
            var item10 = this.AmmoPouchItem10ID();

            Item.IDName.TryGetValue(item1, out var itemName1);
            Item.IDName.TryGetValue(item2, out var itemName2);
            Item.IDName.TryGetValue(item3, out var itemName3);
            Item.IDName.TryGetValue(item4, out var itemName4);
            Item.IDName.TryGetValue(item5, out var itemName5);
            Item.IDName.TryGetValue(item6, out var itemName6);
            Item.IDName.TryGetValue(item7, out var itemName7);
            Item.IDName.TryGetValue(item8, out var itemName8);
            Item.IDName.TryGetValue(item9, out var itemName9);
            Item.IDName.TryGetValue(item10, out var itemName10);

            // . also the values have to be skipped if item slot is empty
            if (itemName1 == null || itemName1 == "None" || itemName1 == string.Empty || this.AmmoPouchItem1Qty() == 0)
            {
                itemName1 = "Empty, ";
            }
            else if (itemName2 == null || itemName2 == "None" || itemName2 == string.Empty || this.AmmoPouchItem2Qty() == 0)
            {
                itemName1 += ", ";
            }
            else
            {
                itemName1 += ", ";
            }

            if (itemName2 == null || itemName2 == "None" || itemName2 == string.Empty || this.AmmoPouchItem2Qty() == 0)
            {
                itemName2 = "Empty, ";
            }
            else if (itemName3 == null || itemName3 == "None" || itemName3 == string.Empty || this.AmmoPouchItem3Qty() == 0)
            {
                itemName2 += ", ";
            }
            else
            {
                itemName2 += ", ";
            }

            if (itemName3 == null || itemName3 == "None" || itemName3 == string.Empty || this.AmmoPouchItem3Qty() == 0)
            {
                itemName3 = "Empty, ";
            }
            else if (itemName4 == null || itemName4 == "None" || itemName4 == string.Empty || this.AmmoPouchItem4Qty() == 0)
            {
                itemName3 += ", ";
            }
            else
            {
                itemName3 += ", ";
            }

            if (itemName4 == null || itemName4 == "None" || itemName4 == string.Empty || this.AmmoPouchItem4Qty() == 0)
            {
                itemName4 = "Empty, ";
            }
            else if (itemName5 == null || itemName5 == "None" || itemName5 == string.Empty || this.AmmoPouchItem5Qty() == 0)
            {
                itemName4 += ", ";
            }
            else
            {
                itemName4 += ", ";
            }

            if (itemName5 == null || itemName5 == "None" || itemName5 == string.Empty || this.AmmoPouchItem5Qty() == 0)
            {
                itemName5 = "Empty, \n";
            }
            else if (itemName6 == null || itemName6 == "None" || itemName6 == string.Empty || this.AmmoPouchItem6Qty() == 0)
            {
                itemName5 += string.Empty;
            }
            else
            {
                itemName5 += "\n";
            }

            if (itemName6 == null || itemName6 == "None" || itemName6 == string.Empty || this.AmmoPouchItem6Qty() == 0)
            {
                itemName6 = "Empty, ";
            }
            else if (itemName7 == null || itemName7 == "None" || itemName7 == string.Empty || this.AmmoPouchItem7Qty() == 0)
            {
                itemName6 += ", ";
            }
            else
            {
                itemName6 += ", ";
            }

            if (itemName7 == null || itemName7 == "None" || itemName7 == string.Empty || this.AmmoPouchItem7Qty() == 0)
            {
                itemName7 = "Empty, ";
            }
            else if (itemName8 == null || itemName8 == "None" || itemName8 == string.Empty || this.AmmoPouchItem8Qty() == 0)
            {
                itemName7 += ", ";
            }
            else
            {
                itemName7 += ", ";
            }

            if (itemName8 == null || itemName8 == "None" || itemName8 == string.Empty || this.AmmoPouchItem8Qty() == 0)
            {
                itemName8 = "Empty, ";
            }
            else if (itemName9 == null || itemName9 == "None" || itemName9 == string.Empty || this.AmmoPouchItem9Qty() == 0)
            {
                itemName8 += ", ";
            }
            else
            {
                itemName8 += ", ";
            }

            if (itemName9 == null || itemName9 == "None" || itemName9 == string.Empty || this.AmmoPouchItem9Qty() == 0)
            {
                itemName9 = "Empty, ";
            }
            else if (itemName10 == null || itemName10 == "None" || itemName10 == string.Empty || this.AmmoPouchItem10Qty() == 0)
            {
                itemName9 += ", ";
            }
            else
            {
                itemName9 += ", ";
            }

            if (itemName10 == null || itemName10 == "None" || itemName10 == string.Empty || this.AmmoPouchItem10Qty() == 0)
            {
                itemName10 = "Empty";
            }
            else
            {
                itemName10 += string.Empty;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", itemName1, itemName2, itemName3, itemName4, itemName5, itemName6, itemName7, itemName8, itemName9, itemName10);
        }
    }

    /// <summary>
    /// Gets the weapon class.
    /// </summary>
    /// <returns></returns>
    public string GetWeaponClass(int? weaponClass = null)
    {
        if (weaponClass == null)
        {
            if (this.CurrentWeaponName is "Light Bowgun" or "Heavy Bowgun" or "Bow")
            {
                return "Gunner";
            }
            else
            {
                return "Blademaster";
            }
        }
        else
        {
            if (weaponClass is 1 or 5 or 10)
            {
                return "Gunner";
            }
            else
            {
                return "Blademaster";
            }
        }
    }

    private static string GetGatheringMapFromID(int id) // TODO: are highlands, tidal island or painted falls icons correct?
    {
        if (id >= 470 && id < 0)
        {
            return Messages.EmptyImage;
        }
        else
        {
            return FindGatheringMap(id);
        }
    }

    private static string FindGatheringMap(int id)
    {
        var areaGroup = new List<int> { 0 };

        foreach (var kvp in GatheringMaps.GatheringMapID)
        {
            var areaIDs = kvp.Key;

            if (areaIDs.Contains(id))
            {
                areaGroup = kvp.Key;
                break;
            }
        }

        return DetermineGatheringMap(areaGroup);
    }

    private static string DetermineGatheringMap(List<int> key)
    {
        var gatheringMap = GatheringMaps.GatheringMapID.ContainsKey(key);
        if (!gatheringMap)
        {
            return Messages.EmptyImage;
        }
        else
        {
            return GatheringMaps.GatheringMapID[key];
        }
    }

    /// <summary>
    /// Gets the text format.
    /// </summary>
    /// <returns></returns>
    public static string GetTextFormat()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.TextFormatExport ?? "None";
    }

    /// <summary>
    /// Gets the gender.
    /// </summary>
    /// <value>
    /// The gender.
    /// </value>
    /// <returns></returns>
    public static string GetGender()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.GenderExport ?? "Male";
    }

    public static string GetRealWeaponNameForRunID(string className, long styleID, long weaponID, string weaponSlot1, string weaponSlot2, string weaponSlot3)
    {
        var style = styleID switch
        {
            0 => "Earth Style",
            1 => "Heaven Style",
            2 => "Storm Style",
            3 => "Extreme Style",
            _ => "Earth Style"
        };

        if (className == "Blademaster")
        {
            WeaponBlademaster.IDName.TryGetValue((int)weaponID, out var wepname);
            var address = weaponID.ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();  // gives you hex 4 digit "007B"

            return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2}\n{3} | {4} | {5}", wepname, address, style, weaponSlot1, weaponSlot2, weaponSlot3);
        }
        else if (className == "Gunner")
        {
            WeaponGunner.IDName.TryGetValue((int)weaponID, out var wepname);
            var address = weaponID.ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();  // gives you hex 4 digit "007B"
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2}\n{3} | {4} | {5}", wepname, address, style, weaponSlot1, weaponSlot2, weaponSlot3);
        }
        else
        {
            return "None";
        }
    }

    public string GetItemPouch
    {
        get
        {
            var itemIDs = new int[20]
            {
        this.PouchItem1ID(),
        this.PouchItem2ID(),
        this.PouchItem3ID(),
        this.PouchItem4ID(),
        this.PouchItem5ID(),
        this.PouchItem6ID(),
        this.PouchItem7ID(),
        this.PouchItem8ID(),
        this.PouchItem9ID(),
        this.PouchItem10ID(),
        this.PouchItem11ID(),
        this.PouchItem12ID(),
        this.PouchItem13ID(),
        this.PouchItem14ID(),
        this.PouchItem15ID(),
        this.PouchItem16ID(),
        this.PouchItem17ID(),
        this.PouchItem18ID(),
        this.PouchItem19ID(),
        this.PouchItem20ID(),
            };

            var itemNames = new string[20];
            for (var i = 0; i < 20; i++)
            {
                if (Item.IDName.TryGetValue(itemIDs[i], out var itemName))
                {
                    if (GetTextFormat() == "Markdown" && IsMetaItem(itemIDs[i]) && itemName != null && itemName != "None" && itemName != string.Empty)
                    {
                        itemName = string.Format(CultureInfo.InvariantCulture, "**{0}**", itemName);
                    }
                }
                else
                {
                    itemName = "None";
                }

                itemNames[i] = itemName ?? "None";
            }

            var itemPouch = string.Empty;
            for (var i = 0; i < 20; i++)
            {
                itemPouch += itemNames[i];
                if ((i + 1) % 5 == 0)
                {
                    itemPouch += "\n";
                }
                else if (i != 19)
                {
                    itemPouch += ", ";
                }
            }

            return itemPouch;
        }
    }

    /// <summary>
    /// Gets the name of the head piece.
    /// </summary>
    /// <value>
    /// The name of the head piece.
    /// </value>
    public string GetArmorHeadName
    {
        get
        {
            ArmorHead.IDName.TryGetValue(this.ArmorHeadID(), out var piecename);

            if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
            {
                piecename = string.Format(CultureInfo.InvariantCulture, "**{0}**", piecename);
            }

            var address = this.ArmorHeadID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(this.ArmorHeadDeco1ID()), this.GetDecoName(this.ArmorHeadDeco2ID()), this.GetDecoName(this.ArmorHeadDeco3ID()));
        }
    }

    /// <summary>
    /// Gets the name of the chest piece.
    /// </summary>
    /// <value>
    /// The name of the chest piece.
    /// </value>
    public string GetArmorChestName
    {
        get
        {
            ArmorChest.IDName.TryGetValue(this.ArmorChestID(), out var piecename);

            if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
            {
                piecename = string.Format(CultureInfo.InvariantCulture, "**{0}**", piecename);
            }

            var address = this.ArmorChestID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(this.ArmorChestDeco1ID()), this.GetDecoName(this.ArmorChestDeco2ID()), this.GetDecoName(this.ArmorChestDeco3ID()));
        }
    }

    /// <summary>
    /// Gets the name of the arms piece.
    /// </summary>
    /// <value>
    /// The name of the arms piece.
    /// </value>
    public string GetArmorArmName
    {
        get
        {
            ArmorArms.IDName.TryGetValue(this.ArmorArmsID(), out var piecename);

            if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
            {
                piecename = string.Format(CultureInfo.InvariantCulture, "**{0}**", piecename);
            }

            var address = this.ArmorArmsID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(this.ArmorArmsDeco1ID()), this.GetDecoName(this.ArmorArmsDeco2ID()), this.GetDecoName(this.ArmorArmsDeco3ID()));
        }
    }

    /// <summary>
    /// Gets the name of the waist piece.
    /// </summary>
    /// <value>
    /// The name of the waist piece.
    /// </value>
    public string GetArmorWaistName
    {
        get
        {
            ArmorWaist.IDName.TryGetValue(this.ArmorWaistID(), out var piecename);

            if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
            {
                piecename = string.Format(CultureInfo.InvariantCulture, "**{0}**", piecename);
            }

            var address = this.ArmorWaistID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(this.ArmorWaistDeco1ID()), this.GetDecoName(this.ArmorWaistDeco2ID()), this.GetDecoName(this.ArmorWaistDeco3ID()));
        }
    }

    /// <summary>
    /// Gets the name of the head piece.
    /// </summary>
    /// <value>
    /// The name of the head piece.
    /// </value>
    public string GetArmorLegName
    {
        get
        {
            ArmorLegs.IDName.TryGetValue(this.ArmorLegsID(), out var piecename);

            if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
            {
                piecename = string.Format(CultureInfo.InvariantCulture, "**{0}**", piecename);
            }

            var address = this.ArmorLegsID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(this.ArmorLegsDeco1ID()), this.GetDecoName(this.ArmorLegsDeco2ID()), this.GetDecoName(this.ArmorLegsDeco3ID()));
        }
    }

    /// <summary>
    /// Gets the name of the first cuff.
    /// </summary>
    /// <value>
    /// The name of the first cuff.
    /// </value>
    public string GetCuff1Name
    {
        get
        {
            Item.IDName.TryGetValue(this.Cuff1ID(), out var cuffname);
            var address = this.Cuff1ID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", cuffname, address);
        }
    }

    /// <summary>
    /// Gets the name of the second cuff.
    /// </summary>
    /// <value>
    /// The name of the second cuff.
    /// </value>
    public string GetCuff2Name
    {
        get
        {
            Item.IDName.TryGetValue(this.Cuff2ID(), out var cuffname);
            var address = this.Cuff2ID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", cuffname, address);
        }
    }

    public static string GetItemsForRunID(int[] items)
    {
        var sb = new StringBuilder();
        var counter = 0;
        for (var i = 0; i < items.Length; i++)
        {
            var id = items[i];
            if (Item.IDName.TryGetValue(id, out var value))
            {
                if (value != "None" && value != string.Empty)
                {
                    sb.Append(value);
                    counter++;
                    if (counter % 5 == 0)
                    {
                        sb.Append('\n');
                    }
                    else if (i != items.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
            }
        }

        if (sb.Length == 0)
        {
            return "None";
        }

        return sb.ToString();
    }

    // TODO: Levels
    public static string GetRoadDureSkillsForRunID(int[] skills)
    {
        var name = string.Empty;
        for (var i = 0; i < skills.Length; i++)
        {
            var id = skills[i];
            if (SkillRoadTower.IDName.TryGetValue(id, out var value) && value != "None" && value != string.Empty)
            {
                name += value;
                if (i != skills.Length - 1)
                {
                    name += ", ";
                }

                if (i % 5 == 4)
                {
                    name += "\n";
                }
            }
        }

        return string.IsNullOrEmpty(name) ? "None" : name;
    }

    public string GetArmorHeadNameForRunID(int armorHeadID, int headSlot1ID, int headslot2ID, int headSlot3ID)
    {
        ArmorHead.IDName.TryGetValue(armorHeadID, out var piecename);

        var address = armorHeadID.ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
        return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(headSlot1ID), this.GetDecoName(headslot2ID), this.GetDecoName(headSlot3ID));
    }

    public string GetArmorChestNameForRunID(int armorID, int slot1ID, int slot2ID, int slot3ID)
    {
        ArmorChest.IDName.TryGetValue(armorID, out var piecename);

        var address = armorID.ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
        return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(slot1ID), this.GetDecoName(slot2ID), this.GetDecoName(slot3ID));
    }

    public string GetArmorArmNameForRunID(int armorID, int slot1ID, int slot2ID, int slot3ID)
    {
        ArmorArms.IDName.TryGetValue(armorID, out var piecename);

        var address = armorID.ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
        return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(slot1ID), this.GetDecoName(slot2ID), this.GetDecoName(slot3ID));
    }

    public string GetArmorWaistNameForRunID(int armorID, int slot1ID, int slot2ID, int slot3ID)
    {
        ArmorWaist.IDName.TryGetValue(armorID, out var piecename);

        var address = armorID.ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
        return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(slot1ID), this.GetDecoName(slot2ID), this.GetDecoName(slot3ID));
    }

    public string GetArmorLegNameForRunID(int armorID, int slot1ID, int slot2ID, int slot3ID)
    {
        ArmorLegs.IDName.TryGetValue(armorID, out var piecename);

        var address = armorID.ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
        return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) | {2} | {3} | {4}", piecename, address, this.GetDecoName(slot1ID), this.GetDecoName(slot2ID), this.GetDecoName(slot3ID));
    }

    /// <summary>
    /// Gets the decos.
    /// </summary>
    /// <value>
    /// The decos.
    /// </value>
    /// <returns></returns>
    public string GetDecoName(int id, EquipmentSlot slot = EquipmentSlot.None, bool isForImage = false)
    {
        var decoName = string.Empty;

        var keyFound = Item.IDName.TryGetValue(id, out decoName);

        if (GetTextFormat() == "Markdown" && IsMetaItem(id) && decoName != null && decoName == "None" && decoName != string.Empty && keyFound)
        {
            decoName = string.Format(CultureInfo.InvariantCulture, "**{0}**", decoName);
        }

        if (decoName == null || decoName == "None" || decoName == string.Empty)
        {
            decoName = "Empty";
        }
        else
        {
            decoName += string.Empty;
        }

        if (decoName == "Empty" && slot != EquipmentSlot.None)
        {
            return this.GetSigilName(slot);
        }

        string address;
        if (!isForImage)
        {
            address = " (" + id.ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant() + ")";
        }
        else
        {
            address = string.Empty;
        }

        return string.Format(CultureInfo.InvariantCulture, "{0}{1}", decoName, address);
    }

    /// <summary>
    /// Gets the sigils.
    /// </summary>
    /// <value>
    /// The sigils.
    /// </value>
    /// <returns></returns>
    public string GetSigilName(EquipmentSlot slot)
    {
        if (slot == EquipmentSlot.None)
        {
            return "Empty";
        }

        var decoSlot1Occupied = WeaponDeco1ID() > 0;
        var decoSlot2Occupied = WeaponDeco2ID() > 0;
        var decoSlot3Occupied = WeaponDeco3ID() > 0;

        if ((decoSlot1Occupied && decoSlot2Occupied && decoSlot3Occupied)
            || (decoSlot1Occupied && slot == EquipmentSlot.One)
            || (decoSlot2Occupied && slot == EquipmentSlot.Two)
            || (decoSlot3Occupied && slot == EquipmentSlot.Three))
        {
            return "Empty";
        }

        var sigilSkillList = SkillSigil.IDName;
        var sigilNames = new int[] { this.Sigil1Name1(), this.Sigil1Name2(), this.Sigil1Name3(), this.Sigil2Name1(), this.Sigil2Name2(), this.Sigil2Name3(), this.Sigil3Name1(), this.Sigil3Name2(), this.Sigil3Name3() };
        var sigilValues = new int[] { this.Sigil1Value1(), this.Sigil1Value2(), this.Sigil1Value3(), this.Sigil2Value1(), this.Sigil2Value2(), this.Sigil2Value3(), this.Sigil3Value1(), this.Sigil3Value2(), this.Sigil3Value3() };
        var sigilTypes = new string[9];
        for (var i = 0; i < 9; i++)
        {
            sigilSkillList.TryGetValue(sigilNames[i], out var type);
            sigilTypes[i] = type ?? string.Empty;
        }

        var index = 0;
        var slotNumber = 1;

        switch (slot)
        {
            case EquipmentSlot.Two:
                slotNumber = 2;
                break;
            case EquipmentSlot.Three:
                slotNumber = 3;
                break;
        }

        if ((decoSlot1Occupied && decoSlot2Occupied)
            || (decoSlot1Occupied && !decoSlot2Occupied && !decoSlot3Occupied && slot == EquipmentSlot.Two))
        {
            index = 0;
        }
        else if (decoSlot1Occupied && !decoSlot2Occupied && !decoSlot3Occupied && slot == EquipmentSlot.Three){
            index = 3;
        }
        else if (!decoSlot1Occupied && !decoSlot2Occupied && !decoSlot3Occupied)
        {
            index = (slotNumber - 1) * 3;
        }

        if (sigilValues[index] == 0 || sigilNames[index] == 0)
        {
            return "Empty";
        }

        var type1 = sigilTypes[index] + ": ";
        var value1 = (sigilValues[index] > 127 ? sigilValues[index] - 256 : sigilValues[index]).ToString(CultureInfo.InvariantCulture);
        var type2 = "Empty, ";
        var value2 = string.Empty;
        var type3 = "Empty";
        var value3 = string.Empty;

        if (sigilValues[index + 1] != 0 && sigilNames[index + 1] != 0)
        {
            type2 = sigilTypes[index + 1] + ": ";
            value2 = (sigilValues[index + 1] > 127 ? sigilValues[index + 1] - 256 : sigilValues[index + 1]).ToString(CultureInfo.InvariantCulture);
        }

        if (sigilValues[index + 2] != 0 && sigilNames[index + 2] != 0)
        {
            type3 = sigilTypes[index + 2] + ": ";
            value3 = (sigilValues[index + 2] > 127 ? sigilValues[index + 2] - 256 : sigilValues[index + 2]).ToString(CultureInfo.InvariantCulture);
        }

        return type1 + (value1 != string.Empty && !value1.Contains('-') ? "+" : string.Empty) + value1 + ", " + type2 + (value2 != string.Empty && !value2.Contains('-') ? "+" : string.Empty) + value2 + ", " + type3 + (value3 != string.Empty && !value3.Contains('-') ? "+" : string.Empty) + value3;
    }

    public static string GetCuffName(int cuffID)
    {
        Item.IDName.TryGetValue(cuffID, out var cuffname);
        var address = cuffID.ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
        return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", cuffname, address);
    }

    public string GetCuffs
    {
        get
        {
            var cuff1 = this.GetCuff1Name;
            var cuff2 = this.GetCuff2Name;

            if (IsMetaItem(this.Cuff1ID()))
            {
                cuff1 = string.Format(CultureInfo.InvariantCulture, "**{0}**", cuff1);
            }

            if (IsMetaItem(this.Cuff2ID()))
            {
                cuff2 = string.Format(CultureInfo.InvariantCulture, "**{0}**", cuff2);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0} | {1}", cuff1, cuff2);
        }
    }

    /// <summary>
    /// Gets the caravan skills.
    /// </summary>
    /// <returns></returns>
    public string GetCaravanSkills
    {
        get
        {
            var id1 = this.CaravanSkill1();
            var id2 = this.CaravanSkill2();
            var id3 = this.CaravanSkill3();

            SkillCaravan.IDName.TryGetValue(id1, out var caravanSkillName1);
            SkillCaravan.IDName.TryGetValue(id2, out var caravanSkillName2);
            SkillCaravan.IDName.TryGetValue(id3, out var caravanSkillName3);

            if (GetTextFormat() == "Markdown")
            {
                if (IsMaxCaravanSkill(id1))
                {
                    caravanSkillName1 = string.Format(CultureInfo.InvariantCulture, "**{0}**", caravanSkillName1);
                }

                if (IsMaxCaravanSkill(id2))
                {
                    caravanSkillName2 = string.Format(CultureInfo.InvariantCulture, "**{0}**", caravanSkillName2);
                }

                if (IsMaxCaravanSkill(id3))
                {
                    caravanSkillName3 = string.Format(CultureInfo.InvariantCulture, "**{0}**", caravanSkillName3);
                }
            }

            if (caravanSkillName1 == string.Empty || caravanSkillName1 == "None")
            {
                return "None";
            }
            else if (caravanSkillName2 == string.Empty || caravanSkillName2 == "None")
            {
                return caravanSkillName1 + string.Empty;
            }
            else if (caravanSkillName3 == string.Empty || caravanSkillName3 == "None")
            {
                return caravanSkillName1 + ", " + caravanSkillName2;
            }
            else
            {
                return caravanSkillName1 + ", " + caravanSkillName2 + ", " + caravanSkillName3;
            }
        }
    }

    /// <summary>
    /// Gets the zenith skills.
    /// </summary>
    /// <value>
    /// The zenith skills.
    /// </value>
    public string GetZenithSkills
    {
        get
        {
            var skills = new string[7];
            for (var i = 0; i < 7; i++)
            {
                var skillNum = i + 1;
                var skillID = (int)this.GetType().GetMethod($"ZenithSkill{skillNum}").Invoke(this, null);
                if (SkillZenith.IDName.TryGetValue(skillID, out var skillName))
                {
                    if (IsMaxZenithSkill(skillID) && skillName != "None" && !string.IsNullOrEmpty(skillName))
                    {
                        skillName = "**" + skillName + "**";
                    }

                    skills[i] = skillName;
                }
            }

            var result = string.Join(", ", skills.Where(s => s != "None" && s != string.Empty).Take(5));
            for (var i = 5; i < 7; i++)
            {
                if (skills[i] != "None" && skills[i] != string.Empty)
                {
                    result += "\n" + skills[i];
                    if (i < 6 && skills[i + 1] != "None" && skills[i + 1] != string.Empty)
                    {
                        result += ", ";
                    }
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Gets the armor skills.
    /// </summary>
    /// <value>
    /// The armor skills.
    /// </value>
    public string GetArmorSkills
    {
        get
        {
            var skills = new int[19];
            for (var i = 0; i < 19; i++)
            {
                skills[i] = GetGouBoostMode()
                    ? GetGouBoostSkill((int)this.GetType().GetMethod("ArmorSkill" + (i + 1)).Invoke(this, null))
                    : (int)this.GetType().GetMethod("ArmorSkill" + (i + 1)).Invoke(this, null);
            }

            var skillNames = new string[19];
            for (var i = 0; i < 19; i++)
            {
                SkillArmor.IDName.TryGetValue(skills[i], out var skillName);
                skillNames[i] = skillName ?? string.Empty;
            }

            var result = string.Empty;
            for (var i = 0; i < 19; i++)
            {
                if (GetTextFormat() == "Markdown" && IsMaxSkillLevel(skills[i]) && skillNames[i] != string.Empty && skillNames[i] != "None")
                {
                    skillNames[i] = string.Format(CultureInfo.InvariantCulture, "**{0}**", skillNames[i]);
                }

                result += skillNames[i] + (i == 18 ? string.Empty : i % 5 == 4 ? "\n" : ", ");
            }

            return result;
        }
    }

    /// <summary>
    /// Gets the automatic skills.
    /// </summary>
    /// <value>
    /// The automatic skills.
    /// </value>
    public string GetAutomaticSkills
    {
        get
        {
            SkillArmor.IDName.TryGetValue(this.AutomaticSkillWeapon(), out var skillName1);
            SkillArmor.IDName.TryGetValue(this.AutomaticSkillHead(), out var skillName2);
            SkillArmor.IDName.TryGetValue(this.AutomaticSkillChest(), out var skillName3);
            SkillArmor.IDName.TryGetValue(this.AutomaticSkillArms(), out var skillName4);
            SkillArmor.IDName.TryGetValue(this.AutomaticSkillWaist(), out var skillName5);
            SkillArmor.IDName.TryGetValue(this.AutomaticSkillLegs(), out var skillName6);

            if (skillName1 == null || skillName1 == "None" || skillName1 == string.Empty)
            {
                skillName1 = string.Empty;
            }
            else if (skillName2 == null || skillName2 == "None" || skillName2 == string.Empty)
            {
                skillName1 += string.Empty;
            }
            else
            {
                skillName1 += ", ";
            }

            if (skillName2 == null || skillName2 == "None" || skillName2 == string.Empty)
            {
                skillName2 = string.Empty;
            }
            else if (skillName3 == null || skillName3 == "None" || skillName3 == string.Empty)
            {
                skillName2 += string.Empty;
            }
            else
            {
                skillName2 += ", ";
            }

            if (skillName3 == null || skillName3 == "None" || skillName3 == string.Empty)
            {
                skillName3 = string.Empty;
            }
            else if (skillName4 == null || skillName4 == "None" || skillName4 == string.Empty)
            {
                skillName3 += string.Empty;
            }
            else
            {
                skillName3 += ", ";
            }

            if (skillName4 == null || skillName4 == "None" || skillName4 == string.Empty)
            {
                skillName4 = string.Empty;
            }
            else if (skillName5 == null || skillName5 == "None" || skillName5 == string.Empty)
            {
                skillName4 += string.Empty;
            }
            else
            {
                skillName4 += ", ";
            }

            if (skillName5 == null || skillName5 == "None" || skillName5 == string.Empty)
            {
                skillName5 = string.Empty;
            }
            else if (skillName6 == null || skillName6 == "None" || skillName6 == string.Empty)
            {
                skillName5 += string.Empty;
            }
            else
            {
                skillName5 += "\n";
            }

            if (skillName6 == null || skillName6 == "None" || skillName6 == string.Empty)
            {
                skillName6 = string.Empty;
            }
            else
            {
                skillName6 += string.Empty;
            }

            if (skillName1 == string.Empty)
            {
                skillName1 = "None";
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}", skillName1, skillName2, skillName3, skillName4, skillName5, skillName6);
        }
    }

    public string MarkdownSavedGearStats { get; set; } = string.Empty;

    /// <summary>
    /// Determines whether [is maximum caravan skill] [the specified identifier].
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>
    ///   <c>true</c> if [is maximum caravan skill] [the specified identifier]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMaxCaravanSkill(int id) => id switch
    {
        1 or 2 or 3 or 4 or 5 or 6 or 7 or 8 or 9 or 12 or 15 or 16 or 21 or 24 or 27 or 39 or 42 or 76 or 77 or 78 or 79 or 80 or 83 or 86 or 89 or 92 or 96 or 100 or 101 or 102 or 103 or 104 or 105 or 106 or 107 or 108 => true,
        _ => false,
    };

    /// <summary>
    /// Determines whether [is maximum zenith skill].
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>
    ///   <c>true</c> if [is maximum zenith skill] [the specified identifier]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMaxZenithSkill(int id) => id switch
    {
        7 or 9 or 11 or 12 or 14 or 15 or 19 or 23 or 25 or 27 or 29 or 31 or 32 or 34 or 35 or 37 or 39 or 41 or 43 or 46 or 47 or 48 or 49 or 50 or 51 or 52 => true,
        _ => false,
    };

    public string GetCuffsForRunID(long cuff1ID, long cuff2ID)
    {
        var cuff1 = GetCuffName((int)cuff1ID);
        var cuff2 = GetCuffName((int)cuff2ID);

        return string.Format(CultureInfo.InvariantCulture, "{0} | {1}", cuff1, cuff2);
    }

    public static string GetCaravanSkillsForRunID(int skill1, int skill2, int skill3)
    {
        var caravanSkillName = string.Empty;
        var skills = new int[] { skill1, skill2, skill3 };
        for (var i = 0; i < skills.Length; i++)
        {
            var skillId = skills[i];
            if (SkillCaravan.IDName.TryGetValue(skillId, out var skillName)
                && skillName != "None" && skillName != string.Empty)
            {
                caravanSkillName += skillName;
                if (i != skills.Length - 1)
                {
                    caravanSkillName += ", ";
                }

                if (i % 5 == 4)
                {
                    caravanSkillName += "\n";
                }
            }
        }

        return string.IsNullOrEmpty(caravanSkillName) ? "None" : caravanSkillName;
    }

    /// <summary>
    /// Determines whether [is maximum skill level].
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>
    ///   <c>true</c> if [is maximum skill level] [the specified identifier]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMaxSkillLevel(int id) => id switch
    {
        2 or 5 or 8 or 11 or 13 or 16 or 19 or 28 or 33 or 38 or 41 or 44 or 45 or 48 or 51 or 53 or 54 or 55 or 56 or 58 or 61 or 64 or 67 or 70 or 71 or 72 or 74 or 78 or 89 or 94 or 97 or 100 or 106 or 112 or 118 or 124 or 130 or 135 or 140 or 147 or 150 or 153 or 155 or 159 or 162 or 164 or 165 or 169 or 172 or 173 or 178 or 180 or 182 or 184 or 186 or 189 or 190 or 192 or 194 or 199 or 201 or 203 or 212 or 218 or 222 or 223 or 228 or 232 or 235 or 238 or 240 or 242 or 244 or 246 or 248 or 250 or 252 or 256 or 259 or 262 or 265 or 268 or 271 or 274 or 277 or 280 or 283 or 285 or 287 or 288 or 289 or 290 or 292 or 293 or 295 or 296 or 297 or 299 or 300 or 301 or 302 or 305 or 309 or 313 or 317 or 321 or 325 or 329 or 333 or 337 or 341 or 345 or 349 or 350 or 352 or 353 or 354 or 356 or 359 or 360 or 362 or 365 or 366 or 368 or 384 or 388 or 390 or 392 or 393 or 394 or 395 or 396 or 397 or 398 or 401 or 404 or 407 or 414 or 417 or 420 or 423 or 425 or 426 or 431 or 432 or 437 or 438 or 443 or 446 or 449 or 452 or 453 or 456 or 457 or 458 or 461 or 463 or 464 or 465 or 466 or 471 or 473 or 474 or 475 or 476 or 477 or 480 or 481 or 482 or 485 or 486 or 488 or 489 or 491 or 494 or 495 or 497 or 498 or 499 or 501 or 502 or 503 or 504 or 505 or 506 or 512 or 513 or 514 or 515 or 516 or 517 or 519 or 520 or 521 or 522 or 523 or 524 or 525 or 526 or 528 or 529 or 530 => true,
        _ => false,
    };

    /// <summary>
    /// Determines whether [is maximum road dure skill level] [the specified identifier].
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="level">The level.</param>
    /// <returns>
    ///   <c>true</c> if [is maximum road dure skill level] [the specified identifier]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMaxRoadDureSkillLevel(int id, string level)
    {
        if (level == "0")
        {
            return false;
        }

        return id switch
        {
            1 or 2 or 19 or 20 => level == "5",
            23 or 24 or 25 or 26 or 27 or 34 or 29 or 31 or 14 or 15 or 18 or 22 => level == "3",
            5 or 28 or 33 or 32 => level == "2",
            35 or 30 => level == "1",
            _ => false,
        };
    }

    /// <summary>
    /// Gets the gou boost mode.
    /// </summary>
    /// <returns></returns>
    public static bool GetGouBoostMode()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.GouBoostExport;
    }

    /// <summary>
    /// Gets the goushou boost skill.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int GetGouBoostSkill(int id) => id switch
    {
        1 => 2,
        4 => 5,
        7 => 8,
        10 => 11,
        21 => 22,
        22 => 23,
        23 => 291,
        27 => 28,
        31 => 293,
        35 => 36,
        36 => 37,
        37 => 284,
        38 => 398,
        40 => 41,
        46 => 47,
        47 => 48,
        52 => 53,
        53 => 482,
        73 => 74,
        79 => 80,
        80 => 81,
        81 => 286,
        82 => 83,
        83 => 84,
        84 => 294,
        88 => 89,
        92 => 93,
        93 => 288,
        95 => 96,
        96 => 384,
        104 => 105,
        105 => 106,
        110 => 111,
        111 => 112,
        116 => 117,
        117 => 118,
        122 => 123,
        123 => 124,
        128 => 129,
        129 => 130,
        134 => 135,
        139 => 140,
        144 => 145,
        145 => 146,
        146 => 349,
        149 => 150,
        154 => 155,
        158 => 159,
        163 => 164,
        167 => 168,
        168 => 169,
        177 => 178,
        184 => 289,
        187 => 290,
        193 => 194,
        197 => 350,
        198 => 199,
        200 => 201,
        203 => 298,
        219 => 359,
        220 => 360,
        230 => 231,
        231 => 232,
        234 => 300,
        235 => 301,
        241 => 242,
        243 => 244,
        245 => 246,
        247 => 248,
        249 => 250,
        254 => 255,
        255 => 256,
        257 => 258,
        258 => 259,
        260 => 261,
        261 => 262,
        263 => 264,
        264 => 265,
        266 => 267,
        267 => 268,
        269 => 270,
        270 => 271,
        272 => 273,
        273 => 274,
        275 => 276,
        276 => 277,
        278 => 279,
        279 => 280,
        281 => 282,
        282 => 283,
        298 => 299,
        303 => 302, // TODO: test
        351 => 352,
        357 => 356,
        361 => 362,
        369 => 370,
        370 => 491,
        477 => 476,
        _ => id,
    };

    /// <summary>
    /// Gets the total GSR weapon unlocks.
    /// </summary>
    /// <returns></returns>
    public static string GetTotalGSRWeaponUnlocks()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.GSRUnlocksExport ?? "11";
    }

    public static string GetZenithSkillsForRunID(int skill1, int skill2, int skill3, int skill4, int skill5, int skill6, int skill7)
    {
        var zenithSkillName = string.Empty;
        var skills = new int[] { skill1, skill2, skill3, skill4, skill5, skill6, skill7 };
        for (var i = 0; i < skills.Length; i++)
        {
            var skillId = skills[i];
            if (SkillZenith.IDName.TryGetValue(skillId, out var skillName))
            {
                if (skillName != "None" && skillName != string.Empty)
                {
                    zenithSkillName += skillName;
                    if (i != skills.Length - 1)
                    {
                        zenithSkillName += ", ";
                    }

                    if (i % 5 == 4)
                    {
                        zenithSkillName += "\n";
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(zenithSkillName))
        {
            return "None";
        }

        return zenithSkillName;
    }

    public static string GetArmorSkillsForRunID(int skill1, int skill2, int skill3, int skill4, int skill5, int skill6, int skill7, int skill8, int skill9, int skill10, int skill11, int skill12, int skill13, int skill14, int skill15, int skill16, int skill17, int skill18, int skill19)
    {
        var armorSkillName = string.Empty;
        var skills = new int[] { skill1, skill2, skill3, skill4, skill5, skill6, skill7, skill8, skill9, skill10, skill11, skill12, skill13, skill14, skill15, skill16, skill17, skill18, skill19 };
        for (var i = 0; i < skills.Length; i++)
        {
            var skillId = skills[i];
            if (SkillArmor.IDName.TryGetValue(skillId, out var skillName))
            {
                if (skillName != "None" && skillName != string.Empty)
                {
                    armorSkillName += skillName;
                    if (i != skills.Length - 1)
                    {
                        armorSkillName += ", ";
                    }

                    if (i % 5 == 4)
                    {
                        armorSkillName += "\n";
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(armorSkillName))
        {
            return "None";
        }

        return armorSkillName;
    }

    /// <summary>
    /// Gets the diva skill name from identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetDivaSkillNameFromID(int id)
    {
        SkillDiva.IDName.TryGetValue(id, out var divaskillaname);
        return divaskillaname + string.Empty;
    }

    /// <summary>
    /// Gets the name of the item.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetItemName(int id)
    {
        Item.IDName.TryGetValue(id, out var itemValue1);  // returns true

        return itemValue1 + string.Empty;
    }

    /// <summary>
    /// Gets the armor skill.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetArmorSkill(int id)
    {
        SkillArmor.IDName.TryGetValue(id, out var skillname);
        if (skillname == string.Empty || skillname == null)
        {
            return "None";
        }
        else
        {
            return skillname + string.Empty;
        }
    }

    /// <summary>
    /// Gets the armor skill.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetArmorSkillWithNull(int id)
    {
        SkillArmor.IDName.TryGetValue(id, out var skillname);
        if (skillname == string.Empty)
        {
            return "None";
        }
        else
        {
            return skillname + string.Empty;
        }
    }

    public static string GetAutomaticSkillsForRunID(int skill1, int skill2, int skill3, int skill4, int skill5, int skill6)
    {
        var automaticSkillName = string.Empty;
        var skills = new int[] { skill1, skill2, skill3, skill4, skill5, skill6 };
        for (var i = 0; i < skills.Length; i++)
        {
            var skillId = skills[i];
            if (SkillArmor.IDName.TryGetValue(skillId, out var skillName))
            {
                if (skillName != "None" && skillName != string.Empty)
                {
                    automaticSkillName += skillName;
                    if (i != skills.Length - 1)
                    {
                        automaticSkillName += ", ";
                    }

                    if (i % 5 == 4)
                    {
                        automaticSkillName += "\n";
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(automaticSkillName))
        {
            return "None";
        }

        return automaticSkillName;
    }

    /// <summary>
    /// Is the gsr x11+ R999.
    /// </summary>
    /// <returns></returns>
    public static bool Is11GSR999()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.Enable11GSR999;
    }

    public string SavedGearStats { get; set; } = string.Empty;

    /// <summary>
    /// Gets the GSR skills.
    /// </summary>
    /// <value>
    /// The GSR skills.
    /// </value>
    public string GetGSRSkills
    {
        get
        {
            var skill1 = this.StyleRank1();
            var skill2 = this.StyleRank2();

            SkillStyleRank.IDName.TryGetValue(skill1, out var skillName1);
            SkillStyleRank.IDName.TryGetValue(skill2, out var skillName2);

            skillName1 += string.Empty;
            skillName2 += string.Empty;

            if (skillName1 == string.Empty)
            {
                skillName1 = "Nothing";
            }

            if (skillName2 == string.Empty)
            {
                skillName2 = "Nothing";
            }

            if (!IsFixedGSRSkillValue(skillName1))
            {
                skillName1 = this.GetGSRBonus(skillName1);
            }

            if (!IsFixedGSRSkillValue(skillName2))
            {
                skillName2 = this.GetGSRBonus(skillName2);
            }

            // pls
            if (GetTextFormat() == "Markdown")
            {
                if (IsMaxGSRSkillValue(skillName1) && (skillName1 != null || skillName1 != "Nothing" || skillName1 != string.Empty))
                {
                    skillName1 = string.Format(CultureInfo.InvariantCulture, "**{0}**", skillName1);
                }

                if (IsMaxGSRSkillValue(skillName2) && (skillName2 != null || skillName2 != "Nothing" || skillName2 != string.Empty))
                {
                    skillName2 = string.Format(CultureInfo.InvariantCulture, "**{0}**", skillName2);
                }
            }

            if (skillName1 == null || skillName1 == "Nothing" || skillName1 == string.Empty)
            {
                skillName1 = string.Empty;
            }
            else if (skillName2 == null || skillName2 == "Nothing" || skillName2 == string.Empty)
            {
                skillName1 += string.Empty;
            }
            else
            {
                skillName1 += ", ";
            }

            if (skillName2 == null || skillName2 == "Nothing" || skillName2 == string.Empty)
            {
                skillName2 = string.Empty;
            }
            else
            {
                skillName2 += string.Empty;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", skillName1, skillName2);
        }
    }

    /// <summary>
    /// Determines whether [is fixed GSR skill value] [the specified skill name].
    /// </summary>
    /// <param name="skillName">Name of the skill.</param>
    /// <returns>
    ///   <c>true</c> if [is fixed GSR skill value] [the specified skill name]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsFixedGSRSkillValue(string skillName) => skillName switch
    {
        "Passive Master" or "Soul Revival" or "Secret Tech" or "Max Sharpen" or "Sharpening Up" or "Affinity+26" or "Affinity+24" or "Affinity+22" or "Affinity+20" or "Nothing" => true,
        _ => false,
    };

    /// <summary>
    /// Determines whether [is maximum GSR skill value] [the specified skill name].
    /// </summary>
    /// <param name="skillName">Name of the skill.</param>
    /// <returns>
    ///   <c>true</c> if [is maximum GSR skill value] [the specified skill name]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMaxGSRSkillValue(string skillName) => skillName switch
    {
        "Passive Master" or "Soul Revival" or "Secret Tech" or "Max Sharpen" or "Affinity+26" or "Conquest Def+330" or "Conquest Atk+115" or "Def+180" or "Fire Res+35" or "Water Res+35" or "Thunder Res+35" or "Ice Res+35" or "Dragon Res+35" or "All Res+20" => true,
        _ => false,
    };

    public static string GetGSRSkillsForRunID(int skill1, int skill2)
    {
        var styleRankSkillName = string.Empty;
        var skills = new int[] { skill1, skill2 };
        for (var i = 0; i < skills.Length; i++)
        {
            var skillId = skills[i];
            if (SkillStyleRank.IDName.TryGetValue(skillId, out var skillName))
            {
                if (skillName != "None" && skillName != string.Empty)
                {
                    styleRankSkillName += skillName;
                    if (i != skills.Length - 1)
                    {
                        styleRankSkillName += ", ";
                    }

                    if (i % 5 == 4)
                    {
                        styleRankSkillName += "\n";
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(styleRankSkillName))
        {
            return "None";
        }

        return styleRankSkillName;
    }

    /// <summary>
    /// Gets the GSR bonus. Values from Ferias guy.
    /// </summary>
    /// <param name="skillName">Name of the skill.</param>
    /// <returns></returns>
    public string GetGSRBonus(string skillName)
    {
        // question: does maxing all gsr give 1 more point of all res?
        // also does it increase before grank slowly for the other res?

        // defense here (needs testing)
        if (IsFixedGSRSkillValue(skillName))
        {
            return skillName;
        }

        var def = 0;
        var fireRes = 0;
        var waterRes = 0;
        var iceRes = 0;
        var thunderRes = 0;
        var dragonRes = 0;
        var allRes = 0;
        var conquestAtk = 0;
        var conquestDef = 0;

        var skillNameNumber = int.Parse(skillName[skillName.IndexOf("+", StringComparison.InvariantCulture) ..], CultureInfo.InvariantCulture);
        var skillNameType = skillName[..skillName.IndexOf("+", StringComparison.InvariantCulture)];

        if (this.GSR() >= 10)
        {
            def += 1;
        }

        if (this.GSR() >= 20)
        {
            fireRes += 2;
        }

        if (this.GSR() >= 30)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 40)
        {
            waterRes += 2;
        }

        if (this.GSR() >= 50)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 60)
        {
            def += 1;
        }

        if (this.GSR() >= 70)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 80)
        {
            thunderRes += 2;
        }

        if (this.GSR() >= 90)
        {
            def += 1;
        }

        if (this.GSR() >= 100)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 110)
        {
            def += 1;
        }

        if (this.GSR() >= 120)
        {
            iceRes += 2;
        }

        if (this.GSR() >= 130)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 140)
        {
            dragonRes += 2;
        }

        if (this.GSR() >= 150)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 160)
        {
            def += 1;
        }

        if (this.GSR() >= 170)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 180)
        {
            allRes += 1;
        }

        if (this.GSR() >= 190)
        {
            def += 1;
        }

        if (this.GSR() >= 200)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 210)
        {
            def += 1;
        }

        if (this.GSR() >= 220)
        {
            fireRes += 2;
        }

        if (this.GSR() >= 230)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 240)
        {
            waterRes += 2;
        }

        if (this.GSR() >= 250)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 260)
        {
            def += 1;
        }

        if (this.GSR() >= 270)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 280)
        {
            thunderRes += 2;
        }

        if (this.GSR() >= 290)
        {
            def += 1;
        }

        if (this.GSR() >= 300)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 310)
        {
            def += 1;
        }

        if (this.GSR() >= 320)
        {
            iceRes += 2;
        }

        if (this.GSR() >= 330)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 340)
        {
            dragonRes += 2;
        }

        if (this.GSR() >= 350)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 360)
        {
            def += 2;
        }

        if (this.GSR() >= 370)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 380)
        {
            allRes += 1;
        }

        if (this.GSR() >= 390)
        {
            def += 2;
        }

        if (this.GSR() >= 400)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 410)
        {
            def += 2;
        }

        if (this.GSR() >= 420)
        {
            fireRes += 2;
        }

        if (this.GSR() >= 430)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 440)
        {
            waterRes += 2;
        }

        if (this.GSR() >= 450)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 460)
        {
            def += 2;
        }

        if (this.GSR() >= 470)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 480)
        {
            thunderRes += 2;
        }

        if (this.GSR() >= 490)
        {
            def += 2;
        }

        if (this.GSR() >= 500)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 510)
        {
            def += 2;
        }

        if (this.GSR() >= 520)
        {
            iceRes += 2;
        }

        if (this.GSR() >= 530)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 540)
        {
            dragonRes += 2;
        }

        if (this.GSR() >= 550)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 560)
        {
            def += 2;
        }

        if (this.GSR() >= 570)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 580)
        {
            allRes += 1;
        }

        if (this.GSR() >= 590)
        {
            def += 2;
        }

        if (this.GSR() >= 600)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 610)
        {
            def += 2;
        }

        if (this.GSR() >= 620)
        {
            fireRes += 2;
        }

        if (this.GSR() >= 630)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 640)
        {
            waterRes += 2;
        }

        if (this.GSR() >= 650)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 660)
        {
            def += 2;
        }

        if (this.GSR() >= 670)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 680)
        {
            thunderRes += 2;
        }

        if (this.GSR() >= 690)
        {
            def += 2;
        }

        if (this.GSR() >= 700)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 710)
        {
            def += 2;
        }

        if (this.GSR() >= 720)
        {
            iceRes += 2;
        }

        if (this.GSR() >= 730)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 740)
        {
            dragonRes += 2;
        }

        if (this.GSR() >= 750)
        {
            conquestAtk += 2;
        }

        if (this.GSR() >= 760)
        {
            def += 2;
        }

        if (this.GSR() >= 770)
        {
            conquestDef += 10;
        }

        if (this.GSR() >= 780)
        {
            allRes += 1;
        }

        if (this.GSR() >= 790)
        {
            def += 2;
        }

        if (this.GSR() >= 800)
        {
            conquestAtk += 4;
        }

        if (this.GSR() >= 810)
        {
            def += 2;
        }

        if (this.GSR() >= 820)
        {
            fireRes += 2;
        }

        if (this.GSR() >= 830)
        {
            conquestDef += 10; // 15?
        }

        if (this.GSR() >= 840)
        {
            waterRes += 2;
        }

        if (this.GSR() >= 850)
        {
            conquestAtk += 4;
        }

        if (this.GSR() >= 860)
        {
            def += 2;
        }

        if (this.GSR() >= 870)
        {
            conquestDef += 10; // 15?
        }

        if (this.GSR() >= 880)
        {
            thunderRes += 2;
        }

        if (this.GSR() >= 890)
        {
            def += 2;
        }

        if (this.GSR() >= 900)
        {
            conquestAtk += 4;
        }

        if (this.GSR() >= 910)
        {
            def += 2;
        }

        if (this.GSR() >= 920)
        {
            iceRes += 2;
        }

        if (this.GSR() >= 930)
        {
            conquestDef += 10; // 15?
        }

        if (this.GSR() >= 940)
        {
            dragonRes += 2;
        }

        if (this.GSR() >= 950)
        {
            conquestAtk += 4;
        }

        if (this.GSR() >= 960)
        {
            def += 2;
        }

        if (this.GSR() >= 970)
        {
            conquestDef += 10; // 15?
        }

        if (this.GSR() >= 980)
        {
            allRes += 1;
        }

        if (this.GSR() >= 960)
        {
            def += 2;
        }

        if (this.GSR() >= 999)
        {
            conquestAtk += 4;
        }

        var gSRSkillMultiplier = GetTotalGSRWeaponUnlocks() switch
        {
            "11" => 0,
            "12" => 1,
            "13" => 2,
            "14" => 3,
            _ => 0,
        };

        if (Is11GSR999() && (skillNameType == "Conquest Atk" || skillNameType == "Conquest Def"))
        {
            conquestAtk = 100 + (5 * gSRSkillMultiplier);
            conquestDef = 300 + (10 * gSRSkillMultiplier);
        }

        // already tested
        return skillNameType switch
        {
            "Nothing" => "Nothing",
            "Def" => string.Format(CultureInfo.InvariantCulture, "{0}+{1}", skillNameType, skillNameNumber + def), // goes to 80?
            "Conquest Atk" => string.Format(CultureInfo.InvariantCulture, "{0}+{1}", skillNameType, skillNameNumber + conquestAtk),
            "Conquest Def" => string.Format(CultureInfo.InvariantCulture, "{0}+{1}", skillNameType, skillNameNumber + conquestDef),
            "Fire Res" => string.Format(CultureInfo.InvariantCulture, "{0}+{1}", skillNameType, skillNameNumber + fireRes),
            "Water Res" => string.Format(CultureInfo.InvariantCulture, "{0}+{1}", skillNameType, skillNameNumber + waterRes),
            "Thunder Res" => string.Format(CultureInfo.InvariantCulture, "{0}+{1}", skillNameType, skillNameNumber + thunderRes),
            "Ice Res" => string.Format(CultureInfo.InvariantCulture, "{0}+{1}", skillNameType, skillNameNumber + iceRes),
            "Dragon Res" => string.Format(CultureInfo.InvariantCulture, "{0}+{1}", skillNameType, skillNameNumber + dragonRes),
            "All Res" => string.Format(CultureInfo.InvariantCulture, "{0}+{1}", skillNameType, skillNameNumber + allRes),
            _ => "None",
        };
    }

    public static string GetHunterName
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            return s.HunterName;
        }
    }

    public static string GetGuildName
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            return s.GuildName;
        }
    }

    public static string GetGenderName => GetGender();

    /// <summary>
    /// Gets the gear description.
    /// </summary>
    /// <value>
    /// The gear description.
    /// </value>
    public static string GetGearDescription
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.GearDescriptionExport != null || s.GearDescriptionExport != string.Empty)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}\n", s.GearDescriptionExport);
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public string GetGZenny => this.GZenny().ToString(CultureInfo.InvariantCulture) + " Gz";

    public string GetCaravanLevel => "Lv. " + (this.CaravenGemLevel() + 1).ToString(CultureInfo.InvariantCulture);

    public string GetGSRLevel => this.GSR().ToString(CultureInfo.InvariantCulture);

    public string GetGSRP => this.GSRP().ToString(CultureInfo.InvariantCulture);

    public string GetGRLevel => this.GRankNumber().ToString(CultureInfo.InvariantCulture);

    public string GetGRP => this.GRP().ToString(CultureInfo.InvariantCulture);

    public string GetSR
    {
        get
        {
            var style = this.WeaponStyle() switch
            {
                0 => "Earth Style",
                1 => "Heaven Style",
                2 => "Storm Style",
                3 => "Extreme Style",
                _ => "Earth Style"
            };

            return string.Format(CultureInfo.InvariantCulture, "{0}", style);
        }
    }

    public string GetRP => this.GSR().ToString(CultureInfo.InvariantCulture);

    public string GetBloatAtk => this.BloatedWeaponAttack().ToString(CultureInfo.InvariantCulture);

    public string GetTotalDef => this.TotalDefense().ToString(CultureInfo.InvariantCulture);

    public string GetGCP => this.GCP().ToString(CultureInfo.InvariantCulture);

    public string GetGSRLevelString => this.GSR().ToString(CultureInfo.InvariantCulture);

    public string GetWeaponClassName => this.GetWeaponClass();

    /// <summary>
    /// Gets https://dorielrivalet.github.io/mhfz-ferias-english-project/.
    /// </summary>
    public static string GetGameArmorSkillsPriority => "Skill Priority:\n" +
                "1: SnS Tech\n" +
                "2: DS Tech\n" +
                "3: GS Tech\n" +
                "4: LS Tech\n" +
                "5: Hammer Tech\n" +
                "6: HH Tech\n" +
                "7: Lance Tech\n" +
                "8: GL Tech\n" +
                "9: Switch Axe F Tech\n" +
                "10: Tonfa Tech\n" +
                "11: Magnet Spike Tech\n" +
                "12: HBG Tech\n" +
                "13: LBG Tech\n" +
                "14: Bow Tech\n" +
                "15: Determination\n" +
                "16: Strong attack\n" +
                "17: Issen\n" +
                "18: Furious\n" +
                "19: Exploit Weakness\n" +
                "20: Dissolver\n" +
                "21: Thunder Clad\n" +
                "22: Rush\n" +
                "23: Grace\n" +
                "24: Sword God\n" +
                "25: Edgemaster\n" +
                "26: Critical Shot\n" +
                "27: Point Breakthrough\n" +
                "28: Compensation\n" +
                "29: Rapid Fire\n" +
                "30: Three Worlds\n" +
                "31: Reflect\n" +
                "32: Drawing Arts\n" +
                "33: Encourage\n" +
                "34: Steady Hand\n" +
                "35: Mounting\n" +
                "36: Gentle Shot\n" +
                "37: Spacing\n" +
                "38: Combo Expert\n" +
                "39: Shiriagari\n" +
                "40: Lone Wolf\n" +
                "41: Light Tread\n" +
                "42: Vitality\n" +
                "43: Skilled\n" +
                "44: Trained\n" +
                "45: Rage\n" +
                "46: Iron Arm\n" +
                "47: Breeder\n" +
                "48: Survivor\n" +
                "49: Relief\n" +
                "50: Hunter\n" +
                "51: Sobriety\n" +
                "52: Blast Resistance\n" +
                "53: Crystal Res\n" +
                "54: Magnetic Res\n" +
                "55: Freeze Res\n" +
                "56: Evade Distance\n" +
                "57: Charge Attack Up\n" +
                "58: Bullet Saver\n" +
                "59: Movement Speed\n" +
                "60: Reinforcement\n" +
                "61: Vampirism\n" +
                "62: Adaptation\n" +
                "63: Ice Age\n" +
                "64: Vigorous\n" +
                "65: Dark Pulse\n" +
                "66: Herbal Science\n" +
                "67: Incitement\n" +
                "68: Blazing Grace\n" +
                "69: Abnormality\n" +
                "70: Drug Knowledge\n" +
                "71: Status Assault\n" +
                "72: Stylish Assault\n" +
                "73: Stylish\n" +
                "74: Absolute Defense\n" +
                "75: Assistance\n" +
                "76: Combat Supremacy\n" +
                "77: Mindfulness\n" +
                "78: 相討ち\n" +
                "79: Weapon Handling\n" +
                "80: Elemental Attack\n" +
                "81: Stamina Recovery\n" +
                "82: Guts\n" +
                "83: Speed Setup\n" +
                "84: Status Res\n" +
                "85: Fencing\n" +
                "86: Knife Throwing\n" +
                "87: Caring\n" +
                "88: Def Lock\n" +
                "89: Para\n" +
                "90: Sleep\n" +
                "91: Stun\n" +
                "92: Poison\n" +
                "93: Deoderant\n" +
                "94: Snowball Res\n" +
                "95: Stealth\n" +
                "96: Health\n" +
                "97: Recovery Speed\n" +
                "98: Lavish Attack\n" +
                "99: Sharpness\n" +
                "100: Artisan\n" +
                "101: Expert\n" +
                "102: Crit Conversion\n" +
                "103: Ceaseless\n" +
                "104: Sharpening\n" +
                "105: Obscurity\n" +
                "106: Fortification\n" +
                "107: Guard\n" +
                "108: Auto-Guard\n" +
                "109: Throwing\n" +
                "110: Reload\n" +
                "111: Sniper\n" +
                "112: Auto-Reload\n" +
                "113: Recoil\n" +
                "114: Normal Shot Up\n" +
                "115: Pierce Shot Up\n" +
                "116: Pellet Shot Up\n" +
                "117: Normal Shot Add\n" +
                "118: Pierce Shot Add\n" +
                "119: Pellet Shot Add\n" +
                "120: Crag Shot Add\n" +
                "121: Cluster Shot Add\n" +
                "122: Status Attack\n" +
                "123: Bomb Boost\n" +
                "124: Hunger\n" +
                "125: Gluttony\n" +
                "126: Attack\n" +
                "127: Defense\n" +
                "128: Protection\n" +
                "129: Hearing Protection\n" +
                "130: Anti-Theft\n" +
                "131: Wide-Area\n" +
                "132: Backpacking\n" +
                "133: All Res Up\n" +
                "134: Fire Res\n" +
                "135: Water Res\n" +
                "136: Ice Res\n" +
                "137: Thunder Res\n" +
                "138: Dragon Res\n" +
                "139: Heat Res\n" +
                "140: Cold Res\n" +
                "141: Wind Pressure\n" +
                "142: Map\n" +
                "143: Gathering\n" +
                "144: Gathering Speed\n" +
                "145: Whim\n" +
                "146: Fate\n" +
                "147: Fishing\n" +
                "148: Psychic\n" +
                "149: Recovery\n" +
                "150: Combining\n" +
                "151: Ammo Combiner\n" +
                "152: Alchemy\n" +
                "153: Evasion Boost\n" +
                "154: Evasion\n" +
                "155: Adrenaline\n" +
                "156: Everlasting\n" +
                "157: Stamina\n" +
                "158: Loading\n" +
                "159: Precision\n" +
                "160: Monster\n" +
                "161: Eating\n" +
                "162: Carving\n" +
                "163: Terrain\n" +
                "164: Quake Res\n" +
                "165: Vocal Chords\n" +
                "166: Cooking\n" +
                "167: Gunnery\n" +
                "168: Flute Expert\n" +
                "169: Breakout\n" +
                "170: Martial Arts\n" +
                "171: Strong Arm\n" +
                "172: Inspiration\n" +
                "173: Passive\n" +
                "174: Bond\n" +
                "175: Pressure\n" +
                "176: Capture Proficiency\n" +
                "177: Poison Coating Add\n" +
                "178: Para Coating Add\n" +
                "179: Sleep Coating Add\n" +
                "180: Fire Attack\n" +
                "181: Water Attack\n" +
                "182: Thunder Attack\n" +
                "183: Ice Attack\n" +
                "184: Dragon Attack\n" +
                "185: Fasting\n" +
                "186: Bomb Sword\n" +
                "187: 強撃剣\n" +
                "188: Poison Sword\n" +
                "189: Para Sword\n" +
                "190: Sleep Sword\n" +
                "191: Fire Sword\n" +
                "192: Water Sword\n" +
                "193: Thunder Sword\n" +
                "194: Ice Sword\n" +
                "195: Dragon Sword\n" +
                "196: Focus\n\n" +
                "※Up to 10 skills (12 with G Rank Armour, 19 with Zenith Gear, 20 with Hiden Cuff, 21 with Guild Food, 22 with Fruits, 23 with Diva Skill) can be active at once, the priority of skills is shown above."
                ;

    /// <summary>
    /// gets the road/duremudira skills.
    /// </summary>
    public string GetRoadDureSkills
    {
        get
        {
            var skill1 = this.RoadDureSkill1Name();
            var skill2 = this.RoadDureSkill2Name();
            var skill3 = this.RoadDureSkill3Name();
            var skill4 = this.RoadDureSkill4Name();
            var skill5 = this.RoadDureSkill5Name();
            var skill6 = this.RoadDureSkill6Name();
            var skill7 = this.RoadDureSkill7Name();
            var skill8 = this.RoadDureSkill8Name();
            var skill9 = this.RoadDureSkill9Name();
            var skill10 = this.RoadDureSkill10Name();
            var skill11 = this.RoadDureSkill11Name();
            var skill12 = this.RoadDureSkill12Name();
            var skill13 = this.RoadDureSkill13Name();
            var skill14 = this.RoadDureSkill14Name();
            var skill15 = this.RoadDureSkill15Name();
            var skill16 = this.RoadDureSkill16Name();

            SkillRoadTower.IDName.TryGetValue(skill1, out var skillName1);
            SkillRoadTower.IDName.TryGetValue(skill2, out var skillName2);
            SkillRoadTower.IDName.TryGetValue(skill3, out var skillName3);
            SkillRoadTower.IDName.TryGetValue(skill4, out var skillName4);
            SkillRoadTower.IDName.TryGetValue(skill5, out var skillName5);
            SkillRoadTower.IDName.TryGetValue(skill6, out var skillName6);
            SkillRoadTower.IDName.TryGetValue(skill7, out var skillName7);
            SkillRoadTower.IDName.TryGetValue(skill8, out var skillName8);
            SkillRoadTower.IDName.TryGetValue(skill9, out var skillName9);
            SkillRoadTower.IDName.TryGetValue(skill10, out var skillName10);
            SkillRoadTower.IDName.TryGetValue(skill11, out var skillName11);
            SkillRoadTower.IDName.TryGetValue(skill12, out var skillName12);
            SkillRoadTower.IDName.TryGetValue(skill13, out var skillName13);
            SkillRoadTower.IDName.TryGetValue(skill14, out var skillName14);
            SkillRoadTower.IDName.TryGetValue(skill15, out var skillName15);
            SkillRoadTower.IDName.TryGetValue(skill16, out var skillName16);

            var skillLevel1 = this.RoadDureSkill1Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel2 = this.RoadDureSkill2Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel3 = this.RoadDureSkill3Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel4 = this.RoadDureSkill4Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel5 = this.RoadDureSkill5Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel6 = this.RoadDureSkill6Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel7 = this.RoadDureSkill7Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel8 = this.RoadDureSkill8Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel9 = this.RoadDureSkill9Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel10 = this.RoadDureSkill10Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel11 = this.RoadDureSkill11Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel12 = this.RoadDureSkill12Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel13 = this.RoadDureSkill13Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel14 = this.RoadDureSkill14Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel15 = this.RoadDureSkill15Level().ToString(CultureInfo.InvariantCulture);
            var skillLevel16 = this.RoadDureSkill16Level().ToString(CultureInfo.InvariantCulture);

            // pls
            if (GetTextFormat() == "Markdown")
            {
                if (IsMaxRoadDureSkillLevel(skill1, skillLevel1) && (skillName1 != null || skillName1 != "None" || skillName1 != string.Empty))
                {
                    skillName1 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName1);
                }

                if (IsMaxRoadDureSkillLevel(skill2, skillLevel2) && (skillName2 != null || skillName2 != "None" || skillName2 != string.Empty))
                {
                    skillName2 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName2);
                }

                if (IsMaxRoadDureSkillLevel(skill3, skillLevel3) && (skillName3 != null || skillName3 != "None" || skillName3 != string.Empty))
                {
                    skillName3 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName3);
                }

                if (IsMaxRoadDureSkillLevel(skill4, skillLevel4) && (skillName4 != null || skillName4 != "None" || skillName4 != string.Empty))
                {
                    skillName4 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName4);
                }

                if (IsMaxRoadDureSkillLevel(skill5, skillLevel5) && (skillName5 != null || skillName5 != "None" || skillName5 != string.Empty))
                {
                    skillName5 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName5);
                }

                if (IsMaxRoadDureSkillLevel(skill6, skillLevel6) && (skillName6 != null || skillName6 != "None" || skillName6 != string.Empty))
                {
                    skillName6 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName6);
                }

                if (IsMaxRoadDureSkillLevel(skill7, skillLevel7) && (skillName7 != null || skillName7 != "None" || skillName7 != string.Empty))
                {
                    skillName7 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName7);
                }

                if (IsMaxRoadDureSkillLevel(skill8, skillLevel8) && (skillName8 != null || skillName8 != "None" || skillName8 != string.Empty))
                {
                    skillName8 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName8);
                }

                if (IsMaxRoadDureSkillLevel(skill9, skillLevel9) && (skillName9 != null || skillName9 != "None" || skillName9 != string.Empty))
                {
                    skillName9 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName9);
                }

                if (IsMaxRoadDureSkillLevel(skill10, skillLevel10) && (skillName10 != null || skillName10 != "None" || skillName10 != string.Empty))
                {
                    skillName10 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName10);
                }

                if (IsMaxRoadDureSkillLevel(skill11, skillLevel11) && (skillName11 != null || skillName11 != "None" || skillName11 != string.Empty))
                {
                    skillName11 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName11);
                }

                if (IsMaxRoadDureSkillLevel(skill12, skillLevel12) && (skillName12 != null || skillName12 != "None" || skillName12 != string.Empty))
                {
                    skillName12 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName12);
                }

                if (IsMaxRoadDureSkillLevel(skill13, skillLevel13) && (skillName13 != null || skillName13 != "None" || skillName13 != string.Empty))
                {
                    skillName13 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName13);
                }

                if (IsMaxRoadDureSkillLevel(skill14, skillLevel14) && (skillName14 != null || skillName14 != "None" || skillName14 != string.Empty))
                {
                    skillName14 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName14);
                }

                if (IsMaxRoadDureSkillLevel(skill15, skillLevel15) && (skillName15 != null || skillName15 != "None" || skillName15 != string.Empty))
                {
                    skillName15 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName15);
                }

                if (IsMaxRoadDureSkillLevel(skill16, skillLevel16) && (skillName16 != null || skillName16 != "None" || skillName16 != string.Empty))
                {
                    skillName16 = string.Format(CultureInfo.InvariantCulture, "**{0}** ", skillName16);
                }
            }

            if (skillLevel1 == "0")
            {
                skillLevel1 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill1, skillLevel1))
                {
                    skillLevel1 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel1);
                }
                else
                {
                    skillLevel1 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel1);
                }
            }

            if (skillLevel2 == "0")
            {
                skillLevel2 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill2, skillLevel2))
                {
                    skillLevel2 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel2);
                }
                else
                {
                    skillLevel2 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel2);
                }
            }

            if (skillLevel3 == "0")
            {
                skillLevel3 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill3, skillLevel3))
                {
                    skillLevel3 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel3);
                }
                else
                {
                    skillLevel3 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel3);
                }
            }

            if (skillLevel4 == "0")
            {
                skillLevel4 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill4, skillLevel4))
                {
                    skillLevel4 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel4);
                }
                else
                {
                    skillLevel4 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel4);
                }
            }

            if (skillLevel5 == "0")
            {
                skillLevel5 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill5, skillLevel5))
                {
                    skillLevel5 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel5);
                }
                else
                {
                    skillLevel5 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel5);
                }
            }

            if (skillLevel6 == "0")
            {
                skillLevel6 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill6, skillLevel6))
                {
                    skillLevel6 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel6);
                }
                else
                {
                    skillLevel6 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel6);
                }
            }

            if (skillLevel7 == "0")
            {
                skillLevel7 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill7, skillLevel7))
                {
                    skillLevel7 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel7);
                }
                else
                {
                    skillLevel7 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel7);
                }
            }

            if (skillLevel8 == "0")
            {
                skillLevel8 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill8, skillLevel8))
                {
                    skillLevel8 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel8);
                }
                else
                {
                    skillLevel8 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel8);
                }
            }

            if (skillLevel9 == "0")
            {
                skillLevel9 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill9, skillLevel9))
                {
                    skillLevel9 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel9);
                }
                else
                {
                    skillLevel9 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel9);
                }
            }

            if (skillLevel10 == "0")
            {
                skillLevel10 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill10, skillLevel10))
                {
                    skillLevel10 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel10);
                }
                else
                {
                    skillLevel10 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel10);
                }
            }

            if (skillLevel11 == "0")
            {
                skillLevel11 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill11, skillLevel11))
                {
                    skillLevel11 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel11);
                }
                else
                {
                    skillLevel11 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel11);
                }
            }

            if (skillLevel12 == "0")
            {
                skillLevel12 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill12, skillLevel12))
                {
                    skillLevel12 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel12);
                }
                else
                {
                    skillLevel12 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel12);
                }
            }

            if (skillLevel13 == "0")
            {
                skillLevel13 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill13, skillLevel13))
                {
                    skillLevel13 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel13);
                }
                else
                {
                    skillLevel13 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel13);
                }
            }

            if (skillLevel14 == "0")
            {
                skillLevel14 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill14, skillLevel14))
                {
                    skillLevel14 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel14);
                }
                else
                {
                    skillLevel14 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel14);
                }
            }

            if (skillLevel15 == "0")
            {
                skillLevel15 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill15, skillLevel15))
                {
                    skillLevel15 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel15);
                }
                else
                {
                    skillLevel15 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel15);
                }
            }

            if (skillLevel16 == "0")
            {
                skillLevel16 = string.Empty;
            }
            else
            {
                if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(skill16, skillLevel16))
                {
                    skillLevel16 = string.Format(CultureInfo.InvariantCulture, "**LV{0}**", skillLevel16);
                }
                else
                {
                    skillLevel16 = string.Format(CultureInfo.InvariantCulture, " LV{0}", skillLevel16);
                }
            }

            if (skillName1 == null || skillName1 == "None" || skillName1 == string.Empty)
            {
                skillName1 = string.Empty;
            }
            else if (skillName2 == null || skillName2 == "None" || skillName2 == string.Empty)
            {
                skillName1 += string.Empty;
            }
            else
            {
                skillLevel1 += ", ";
            }

            if (skillName2 == null || skillName2 == "None" || skillName2 == string.Empty)
            {
                skillName2 = string.Empty;
            }
            else if (skillName3 == null || skillName3 == "None" || skillName3 == string.Empty)
            {
                skillName2 += string.Empty;
            }
            else
            {
                skillLevel2 += ", ";
            }

            if (skillName3 == null || skillName3 == "None" || skillName3 == string.Empty)
            {
                skillName3 = string.Empty;
            }
            else if (skillName4 == null || skillName4 == "None" || skillName4 == string.Empty)
            {
                skillName3 += string.Empty;
            }
            else
            {
                skillLevel3 += ", ";
            }

            if (skillName4 == null || skillName4 == "None" || skillName4 == string.Empty)
            {
                skillName4 = string.Empty;
            }
            else if (skillName5 == null || skillName5 == "None" || skillName5 == string.Empty)
            {
                skillName4 += string.Empty;
            }
            else
            {
                skillLevel4 += ", ";
            }

            if (skillName5 == null || skillName5 == "None" || skillName5 == string.Empty)
            {
                skillName5 = string.Empty;
            }
            else if (skillName6 == null || skillName6 == "None" || skillName6 == string.Empty)
            {
                skillName5 += string.Empty;
            }
            else
            {
                skillLevel5 += "\n";
            }

            if (skillName6 == null || skillName6 == "None" || skillName6 == string.Empty)
            {
                skillName6 = string.Empty;
            }
            else if (skillName7 == null || skillName7 == "None" || skillName7 == string.Empty)
            {
                skillName6 += string.Empty;
            }
            else
            {
                skillLevel6 += ", ";
            }

            if (skillName7 == null || skillName7 == "None" || skillName7 == string.Empty)
            {
                skillName7 = string.Empty;
            }
            else if (skillName8 == null || skillName8 == "None" || skillName8 == string.Empty)
            {
                skillName7 += string.Empty;
            }
            else
            {
                skillLevel7 += ", ";
            }

            if (skillName8 == null || skillName8 == "None" || skillName8 == string.Empty)
            {
                skillName8 = string.Empty;
            }
            else if (skillName9 == null || skillName9 == "None" || skillName9 == string.Empty)
            {
                skillName8 += string.Empty;
            }
            else
            {
                skillLevel8 += ", ";
            }

            if (skillName9 == null || skillName9 == "None" || skillName9 == string.Empty)
            {
                skillName9 = string.Empty;
            }
            else if (skillName10 == null || skillName10 == "None" || skillName10 == string.Empty)
            {
                skillName9 += string.Empty;
            }
            else
            {
                skillLevel9 += ", ";
            }

            if (skillName10 == null || skillName10 == "None" || skillName10 == string.Empty)
            {
                skillName10 = string.Empty;
            }
            else if (skillName11 == null || skillName11 == "None" || skillName11 == string.Empty)
            {
                skillName10 += string.Empty;
            }
            else
            {
                skillLevel10 += "\n";
            }

            if (skillName11 == null || skillName11 == "None" || skillName11 == string.Empty)
            {
                skillName11 = string.Empty;
            }
            else if (skillName12 == null || skillName12 == "None" || skillName12 == string.Empty)
            {
                skillName11 += string.Empty;
            }
            else
            {
                skillLevel11 += ", ";
            }

            if (skillName12 == null || skillName12 == "None" || skillName12 == string.Empty)
            {
                skillName12 = string.Empty;
            }
            else if (skillName13 == null || skillName13 == "None" || skillName13 == string.Empty)
            {
                skillName12 += string.Empty;
            }
            else
            {
                skillLevel12 += ", ";
            }

            if (skillName13 == null || skillName13 == "None" || skillName13 == string.Empty)
            {
                skillName13 = string.Empty;
            }
            else if (skillName14 == null || skillName14 == "None" || skillName14 == string.Empty)
            {
                skillName13 += string.Empty;
            }
            else
            {
                skillLevel13 += ", ";
            }

            if (skillName14 == null || skillName14 == "None" || skillName14 == string.Empty)
            {
                skillName14 = string.Empty;
            }
            else if (skillName15 == null || skillName15 == "None" || skillName15 == string.Empty)
            {
                skillName14 += string.Empty;
            }
            else
            {
                skillLevel14 += ", ";
            }

            if (skillName15 == null || skillName15 == "None" || skillName15 == string.Empty)
            {
                skillName15 = string.Empty;
            }
            else if (skillName16 == null || skillName16 == "None" || skillName16 == string.Empty)
            {
                skillName15 += string.Empty;
            }
            else
            {
                skillLevel15 += "\n";
            }

            if (skillName16 == null || skillName16 == "None" || skillName16 == string.Empty)
            {
                skillName16 = string.Empty;
            }
            else
            {
                skillLevel16 += string.Empty;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}", skillName1, skillLevel1, skillName2, skillLevel2, skillName3, skillLevel3, skillName4, skillLevel4, skillName5, skillLevel5, skillName6, skillLevel6, skillName7, skillLevel7, skillName8, skillLevel8, skillName9, skillLevel9, skillName10, skillLevel10, skillName11, skillLevel11, skillName12, skillLevel12, skillName13, skillLevel13, skillName14, skillLevel14, skillName15, skillLevel15, skillName16, skillLevel16);
        }
    }

    /// <summary>
    /// Gets the gear stats.
    /// </summary>
    /// <value>
    /// The gear stats.
    /// </value>
    public string GetGearStats => this.GenerateGearStats();

    /// <summary>
    /// Determines whether item [is meta item].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is meta item]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMetaItem(int id) => id switch
    {
        // hunter taloncharm
        14642 or 1681 or 1682 or 1683 or 1684 or 1481 or 1482 or 1483 or 4750 or 8030 or 8026 or 8037 or 8031 or 8040 or 8042 or 8047 or 8052 or 1548 or 4943 or 9719 or 9715 or 9714 or 4358 or 78 or 4952 or 5771 or 1537 or 1888 or 1553 or 14803 or 14804 or 91 or 92 or 1782 or 300 or 9717 or 34 or 4723 or 5277 or 5279 or 5711 or 5712 or 5714 or 5715 or 5716 or 5717 or 12724 or 12725 or 12726 or 12727 or 12728 or 12729 or 12730 or 12731 or 12732 or 12733 or 12734 or 12735 or 12736 or 12737 or 12738 or 12739 or 12740 or 12741 or 12742 or 12743 or 12744 or 12745 or 12746 or 12747 or 12748 or 12749 or 12750 or 12751 or 12752 or 12753 or 15070 or 15071 or 15072 or 15073 or 15074 or 15075 or 15076 or 15077 or 15078 or 15079 or 13640 or 13641 or 13642 or 13643 or 13644 or 13645 or 13646 or 13647 or 13648 or 13649 or 13650 or 13651 or 13652 or 13653 or 13654 or 13655 or 13656 or 13657 or 13658 or 13659 or 13660 or 13661 or 13662 or 13663 or 13664 or 13665 or 13666 or 13667 or 13668 or 13669 or 13670 or 13671 or 13672 or 13673 or 13674 or 13675 or 13676 or 13677 or 13678 or 13679 or 13680 or 13681 or 13682 or 13683 or 13684 or 13685 or 13686 or 13687 or 13688 or 13689 or 13690 or 13691 or 15546 or 15547 or 15548 or 15549 => true,
        _ => false,
    };

    /// <summary>
    /// Checks whether the gear is part of the meta.
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
    public static bool IsMetaGear(string piece)
    {
        if (piece.Contains("ZP") || piece.Contains("PZ") || piece.Contains("SnS・") || piece.Contains("DS・") || piece.Contains("GS・") || piece.Contains("LS・") || piece.Contains("Hammer・") || piece.Contains("HH・") || piece.Contains("Lance・") || piece.Contains("GL・") || piece.Contains("Swaxe・") || piece.Contains("Tonfa・") || piece.Contains("Magspike・") || piece.Contains("LBG・") || piece.Contains("HBG・") || piece.Contains("Bow・"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// gets the g rank weapon level.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static string GetGRWeaponLevel(int level)
    {
        if (level == 0)
        {
            return string.Empty;
        }
        else
        {
            return " Lv. " + level;
        }
    }

    /// <summary>
    /// Generates the gear stats.
    /// </summary>
    /// <returns></returns>
    public string GenerateGearStats(long? runID = null)
    {
        // TODO: update
        if (runID == null)
        {
            // save gear to variable
            var showGouBoost = string.Empty;

            if (GetGouBoostMode())
            {
                showGouBoost = " (After Gou/Muscle Boost)";
            }

            var s = (Settings)Application.Current.TryFindResource("Settings");

            var gameFolderPathStatus = s.GameFolderPath == @"C:\Program Files (x86)\CAPCOM\Monster Hunter Frontier Online" ? "Standard" : "Custom";
            var mhfdatHash = DatabaseService.CalculateFileHash(s.GameFolderPath, @"\dat\mhfdat.bin");
            var mhfemdHash = DatabaseService.CalculateFileHash(s.GameFolderPath, @"\dat\mhfemd.bin");
            var mhfodllHash = DatabaseService.CalculateFileHash(s.GameFolderPath, @"\mhfo.dll");
            var mhfohddllHash = DatabaseService.CalculateFileHash(s.GameFolderPath, @"\mhfo-hd.dll");

            // zp in bold for markdown
            // fruits and speedrunner items also in bold
            var stats = string.Format(
                CultureInfo.InvariantCulture,
                @$"【MHF-Z】Overlay {Program.CurrentProgramVersion} {this.GetWeaponClass()}({GetGender()}){GetMetadata}

Set Name: {GetGearDescription}
{this.CurrentWeaponName}: {this.GetRealWeaponName}
Head: {this.GetArmorHeadName}
Chest: {this.GetArmorChestName}
Arms: {this.GetArmorArmName}
Waist: {this.GetArmorWaistName}
Legs: {this.GetArmorLegName}
Cuffs: {this.GetCuffs}

Weapon Attack: {this.BloatedWeaponAttack().ToString(CultureInfo.InvariantCulture)} | Total Defense: {this.TotalDefense().ToString(CultureInfo.InvariantCulture)}

Zenith Skills:
{this.GetZenithSkills}

Automatic Skills:
{this.GetAutomaticSkills}

Active Skills{showGouBoost}:
{this.GetArmorSkills}

Caravan Skills:
{this.GetCaravanSkills}

Diva:
{GetDivaSkillNameFromID(this.DivaSkill())}
Song {(DivaSongActive ? "ON" : "OFF")}

Diva Prayer Gems:
{GetDivaPrayerGems()}

Guild:
{GetArmorSkill(this.GuildFoodSkill())}
{GetGuildPoogieEffect()}

Style Rank:
{this.GetGSRSkills}

Items:
{this.GetItemPouch}

Ammo:
{this.GetAmmoPouch}

Poogie Item:
{GetItemName(this.PoogieItemUseID())}

Road/Duremudira Skills:
{this.GetRoadDureSkills}

Active Feature {(IsActiveFeatureOn(GetActiveFeature(), WeaponType()) ? "ON" : "OFF")}

Courses:
Main: {GetMainCourses()}
Additional: {GetAdditionalCourses()}

Halk:
{(HalkOn() ? "Active" : "Inactive")}
Halk Pot {(IsHalkPotEquipped() || HalkPotEffectOn() ? "ON" : "OFF")}
LV{HalkLevel()}
Element Type {GetHalkElement()}
Status Type {GetHalkStatus()}
Intimacy {HalkIntimacy()}
Health {HalkHealth()}
Attack {HalkAttack()}
Defense {HalkDefense()}
Intellect {HalkIntellect()}
{(HalkSkill1() == 0 ? "None" : EZlion.Mapper.SkillHalk.IDName[HalkSkill1()])} | {(HalkSkill2() == 0 ? "None" : EZlion.Mapper.SkillHalk.IDName[HalkSkill2()])} | {(HalkSkill3() == 0 ? "None" : EZlion.Mapper.SkillHalk.IDName[HalkSkill3()])}

Overlay Hash: {DatabaseManagerInstance.GetOverlayHash()}

Game Patch Information:
dat: {GetGamePatchInfo(GamePatchFile.dat, mhfdatHash)}
emd: {GetGamePatchInfo(GamePatchFile.emd, mhfemdHash)}
dll: {GetGamePatchInfo(GamePatchFile.dll, mhfodllHash)}
hddll: {GetGamePatchInfo(GamePatchFile.hddll, mhfohddllHash)}

Weapon Specific:
Dual Swords dropped combos: {this.DualSwordsSharpensDictionary.Count((e) => e.Value == 0)}
");
            this.SavedGearStats = stats;
            var formattedStats = string.Format(
                CultureInfo.InvariantCulture,
                @$"__【MHF-Z】Overlay {Program.CurrentProgramVersion}__ *{this.GetWeaponClass()}({GetGender()})*{GetMetadata}

Set Name: {GetGearDescription}
**{this.CurrentWeaponName}:** {this.GetRealWeaponName}
**Head:** {this.GetArmorHeadName}
**Chest:** {this.GetArmorChestName}
**Arms:** {this.GetArmorArmName}
**Waist:** {this.GetArmorWaistName}
**Legs:** {this.GetArmorLegName}
**Cuffs:** {this.GetCuffs}

**Weapon Attack:** {this.BloatedWeaponAttack().ToString(CultureInfo.InvariantCulture)} | **Total Defense:** {this.TotalDefense().ToString(CultureInfo.InvariantCulture)}

**Zenith Skills:**
{this.GetZenithSkills}

**Automatic Skills:**
{this.GetAutomaticSkills}

**Active Skills{showGouBoost}:**
{this.GetArmorSkills}

**Caravan Skills:**
{this.GetCaravanSkills}

**Diva:**
{GetDivaSkillNameFromID(this.DivaSkill())}
Song {(DivaSongActive ? "ON" : "OFF")}

**Diva Prayer Gems:**
{GetDivaPrayerGems()}

**Guild:**
{GetArmorSkill(this.GuildFoodSkill())}
{GetGuildPoogieEffect()}

**Style Rank:**
{this.GetGSRSkills}

**Items:**
{this.GetItemPouch}

**Ammo:**
{this.GetAmmoPouch}

**Poogie Item:**
{GetItemName(this.PoogieItemUseID())}

**Road/Duremudira Skills:**
{this.GetRoadDureSkills}

**Active Feature {(IsActiveFeatureOn(GetActiveFeature(), WeaponType()) ? "ON" : "OFF")}**

**Courses:**
Main: {GetMainCourses()}
Additional: {GetAdditionalCourses()}

**Halk:**
{(HalkOn() ? "Active" : "Inactive")}
Halk Pot {(IsHalkPotEquipped() || HalkPotEffectOn() ? "ON" : "OFF")}
LV{HalkLevel()}
Element Type {GetHalkElement()}
Status Type {GetHalkStatus()}
Intimacy {HalkIntimacy()}
Health {HalkHealth()}
Attack {HalkAttack()}
Defense {HalkDefense()}
Intellect {HalkIntellect()}
{(HalkSkill1() == 0 ? "None" : EZlion.Mapper.SkillHalk.IDName[HalkSkill1()])} | {(HalkSkill2() == 0 ? "None" : EZlion.Mapper.SkillHalk.IDName[HalkSkill2()])} | {(HalkSkill3() == 0 ? "None" : EZlion.Mapper.SkillHalk.IDName[HalkSkill3()])}

**Overlay Hash:** {DatabaseManagerInstance.GetOverlayHash()}

**Game Patch Information:**
dat: {GetGamePatchInfo(GamePatchFile.dat, mhfdatHash)}
emd: {GetGamePatchInfo(GamePatchFile.emd, mhfemdHash)}
dll: {GetGamePatchInfo(GamePatchFile.dll, mhfodllHash)}
hddll: {GetGamePatchInfo(GamePatchFile.hddll, mhfohddllHash)}

Weapon Specific:
Dual Swords dropped combos: {this.DualSwordsSharpensDictionary.Count((e) => e.Value == 0)}
");
            this.MarkdownSavedGearStats = formattedStats;
            return stats;
        }
        else
        {
            var activeSkills = DatabaseManagerInstance.GetActiveSkills((long)runID);

            if (string.IsNullOrEmpty(activeSkills.CreatedBy))
            {
                return "Run Not Found.\n\nReload the section.";
            }

            var ammoPouch = DatabaseManagerInstance.GetAmmoPouch((long)runID);
            var automaticSkills = DatabaseManagerInstance.GetAutomaticSkills((long)runID);
            var caravanSkills = DatabaseManagerInstance.GetCaravanSkills((long)runID);
            var playerGear = DatabaseManagerInstance.GetPlayerGear((long)runID);
            var playerInventory = DatabaseManagerInstance.GetPlayerInventory((long)runID);
            var roadDureSkills = DatabaseManagerInstance.GetRoadDureSkills((long)runID);
            var styleRankSkills = DatabaseManagerInstance.GetStyleRankSkills((long)runID);
            var zenithSkills = DatabaseManagerInstance.GetZenithSkills((long)runID);
            var quest = DatabaseManagerInstance.GetQuest((long)runID);
            var diva = DatabaseManagerInstance.GetDiva((long)runID);
            var activeFeature = DatabaseManagerInstance.GetActiveFeature((long)runID);
            var courses = DatabaseManagerInstance.GetCourses((long)runID);
            var guildPoogie = DatabaseManagerInstance.GetGuildPoogie((long)runID);
            var weaponBuffs = DatabaseManagerInstance.GetQuestsWeaponBuffs((long)runID);
            var halk = DatabaseManagerInstance.GetHalk((long)runID);
            var toggleMode = DatabaseManagerInstance.GetQuestToggleMode((long)runID);
            var overlayHash = DatabaseManagerInstance.GetOverlayHash((long)runID);

            var createdBy = playerGear.CreatedBy;
            var weaponClass = this.GetWeaponClass((int?)playerGear.WeaponClassID);
            var gender = GetGender();
            var metadata = GetMetadata;
            var gearName = playerGear.GearName;
            var weaponName = GetWeaponNameFromType((int)playerGear.WeaponTypeID);
            var weaponID = (long)(
                playerGear.BlademasterWeaponID == null ?
                    playerGear.GunnerWeaponID == null ?
                        0
                        : playerGear.GunnerWeaponID
                    : playerGear.BlademasterWeaponID);
            var realweaponName = GetRealWeaponNameForRunID(this.GetWeaponClass((int)playerGear.WeaponClassID), playerGear.StyleID, weaponID, playerGear.WeaponSlot1, playerGear.WeaponSlot2, playerGear.WeaponSlot3);
            var head = this.GetArmorHeadNameForRunID((int)playerGear.HeadID, (int)playerGear.HeadSlot1ID, (int)playerGear.HeadSlot2ID, (int)playerGear.HeadSlot3ID);
            var chest = this.GetArmorChestNameForRunID((int)playerGear.ChestID, (int)playerGear.ChestSlot1ID, (int)playerGear.ChestSlot2ID, (int)playerGear.ChestSlot3ID);
            var arms = this.GetArmorArmNameForRunID((int)playerGear.ArmsID, (int)playerGear.ArmsSlot1ID, (int)playerGear.ArmsSlot2ID, (int)playerGear.ArmsSlot3ID);
            var waist = this.GetArmorWaistNameForRunID((int)playerGear.WaistID, (int)playerGear.WaistSlot1ID, (int)playerGear.WaistSlot2ID, (int)playerGear.WaistSlot3ID);
            var legs = this.GetArmorLegNameForRunID((int)playerGear.LegsID, (int)playerGear.LegsSlot1ID, (int)playerGear.LegsSlot2ID, (int)playerGear.LegsSlot3ID);
            var cuffs = this.GetCuffsForRunID(playerGear.Cuff1ID, playerGear.Cuff2ID);
            var date = playerGear.CreatedAt;
            var hash = playerGear.PlayerGearHash;
            var zenithSkillsList = GetZenithSkillsForRunID((int)zenithSkills.ZenithSkill1ID, (int)zenithSkills.ZenithSkill2ID, (int)zenithSkills.ZenithSkill3ID, (int)zenithSkills.ZenithSkill4ID, (int)zenithSkills.ZenithSkill5ID, (int)zenithSkills.ZenithSkill6ID, (int)zenithSkills.ZenithSkill7ID);
            var automaticSkillsList = GetAutomaticSkillsForRunID((int)automaticSkills.AutomaticSkill1ID, (int)automaticSkills.AutomaticSkill2ID, (int)automaticSkills.AutomaticSkill3ID, (int)automaticSkills.AutomaticSkill4ID, (int)automaticSkills.AutomaticSkill5ID, (int)automaticSkills.AutomaticSkill6ID);
            var gouBoost = string.Empty;
            var armorSkills = GetArmorSkillsForRunID((int)activeSkills.ActiveSkill1ID, (int)activeSkills.ActiveSkill2ID, (int)activeSkills.ActiveSkill3ID, (int)activeSkills.ActiveSkill4ID, (int)activeSkills.ActiveSkill5ID, (int)activeSkills.ActiveSkill6ID, (int)activeSkills.ActiveSkill7ID, (int)activeSkills.ActiveSkill8ID, (int)activeSkills.ActiveSkill9ID, (int)activeSkills.ActiveSkill10ID, (int)activeSkills.ActiveSkill11ID, (int)activeSkills.ActiveSkill12ID, (int)activeSkills.ActiveSkill13ID, (int)activeSkills.ActiveSkill14ID, (int)activeSkills.ActiveSkill15ID, (int)activeSkills.ActiveSkill16ID, (int)activeSkills.ActiveSkill17ID, (int)activeSkills.ActiveSkill18ID, (int)activeSkills.ActiveSkill19ID);
            var caravanSkillsList = GetCaravanSkillsForRunID((int)caravanSkills.CaravanSkill1ID, (int)caravanSkills.CaravanSkill2ID, (int)caravanSkills.CaravanSkill3ID);
            var divaSkill = GetDivaSkillNameFromID((int)playerGear.DivaSkillID);
            var guildFood = GetArmorSkill((int)playerGear.GuildFoodID) == "None" ? "No Food" : GetArmorSkill((int)playerGear.GuildFoodID);
            var styleRankSkillsList = GetGSRSkillsForRunID((int)styleRankSkills.StyleRankSkill1ID, (int)styleRankSkills.StyleRankSkill2ID);
            var inventory = GetItemsForRunID(new int[] { (int)playerInventory.Item1ID, (int)playerInventory.Item2ID, (int)playerInventory.Item3ID, (int)playerInventory.Item4ID, (int)playerInventory.Item5ID, (int)playerInventory.Item6ID, (int)playerInventory.Item7ID, (int)playerInventory.Item8ID, (int)playerInventory.Item9ID, (int)playerInventory.Item10ID, (int)playerInventory.Item11ID, (int)playerInventory.Item12ID, (int)playerInventory.Item13ID, (int)playerInventory.Item14ID, (int)playerInventory.Item15ID, (int)playerInventory.Item16ID, (int)playerInventory.Item17ID, (int)playerInventory.Item18ID, (int)playerInventory.Item19ID, (int)playerInventory.Item20ID });
            var ammo = GetItemsForRunID(new int[] { (int)ammoPouch.Item1ID, (int)ammoPouch.Item2ID, (int)ammoPouch.Item3ID, (int)ammoPouch.Item4ID, (int)ammoPouch.Item5ID, (int)ammoPouch.Item6ID, (int)ammoPouch.Item7ID, (int)ammoPouch.Item8ID, (int)ammoPouch.Item9ID, (int)ammoPouch.Item10ID });
            var poogieItem = GetItemName((int)playerGear.PoogieItemID);
            var roadDureSkillsList = GetRoadDureSkillsForRunID(new int[] { (int)roadDureSkills.RoadDureSkill1ID, (int)roadDureSkills.RoadDureSkill2ID, (int)roadDureSkills.RoadDureSkill3ID, (int)roadDureSkills.RoadDureSkill4ID, (int)roadDureSkills.RoadDureSkill5ID, (int)roadDureSkills.RoadDureSkill6ID, (int)roadDureSkills.RoadDureSkill7ID, (int)roadDureSkills.RoadDureSkill8ID, (int)roadDureSkills.RoadDureSkill9ID, (int)roadDureSkills.RoadDureSkill10ID, (int)roadDureSkills.RoadDureSkill11ID, (int)roadDureSkills.RoadDureSkill12ID, (int)roadDureSkills.RoadDureSkill13ID, (int)roadDureSkills.RoadDureSkill14ID, (int)roadDureSkills.RoadDureSkill15ID, (int)roadDureSkills.RoadDureSkill16ID });
            var questName = EZlion.Mapper.Quest.IDName[(int)quest.QuestID];
            var questObjectiveType = EZlion.Mapper.ObjectiveType.IDName[(int)quest.ObjectiveTypeID];
            var questObjectiveQuantity = quest.ObjectiveQuantity;
            var questObjectiveName = quest.ObjectiveName;
            var questCategory = quest.ActualOverlayMode;
            var partySize = quest.PartySize;

            var toggleModeName = toggleMode.QuestToggleMode == null ? "Normal" : EZlion.Mapper.QuestToggleMode.IDName[(int)toggleMode.QuestToggleMode];
            var activeFeatureState = activeFeature.ActiveFeature == null ? false : IsActiveFeatureOn((long)activeFeature.ActiveFeature, playerGear.WeaponTypeID);
            var halkSkill1 = halk.HalkSkill1 == null ? "None" : EZlion.Mapper.SkillHalk.IDName[(int)halk.HalkSkill1];
            var halkSkill2 = halk.HalkSkill2 == null ? "None" : EZlion.Mapper.SkillHalk.IDName[(int)halk.HalkSkill2];
            var halkSkill3 = halk.HalkSkill3 == null ? "None" : EZlion.Mapper.SkillHalk.IDName[(int)halk.HalkSkill3];

            var courseInfo = $"Main: {GetMainCourses(courses.Rights)}\nAdditional: {GetAdditionalCourses(courses.Rights)}";
            var patchInfo = DatabaseManagerInstance.GetQuestsGamePatch((long)runID);
            var questRestrictions = DatabaseManagerInstance.GetQuestRestrictions((long)runID);

            // TODO: fix
            // TODO partnyaBagItems
            // var partnyaBagItems = GetItemsForRunID(new int[] { (int)partnyaBag.Item1ID, (int)partnyaBag.Item2ID, (int)partnyaBag.Item3ID, (int)partnyaBag.Item4ID, (int)partnyaBag.Item5ID, (int)partnyaBag.Item6ID, (int)partnyaBag.Item7ID, (int)partnyaBag.Item8ID, (int)partnyaBag.Item9ID, (int)partnyaBag.Item10ID });
            return string.Format(
                CultureInfo.InvariantCulture,
                $@"{createdBy} {weaponClass}({gender}){metadata}

Set Name: {gearName}
{weaponName}{(questRestrictions.Sigil ? " (disabled sigils)" : string.Empty)}: {realweaponName}
Head: {head}
Chest: {chest}
Arms: {arms}
Waist: {waist}
Legs: {legs}
Cuffs: {cuffs}{(questRestrictions.PoogieCuff ? " (disabled cuffs)" : string.Empty)}

Run Date: {date} | Run Hash: {hash}

Zenith Skills:
{zenithSkillsList}

Automatic Skills:
{automaticSkillsList}

Active Skills{(questRestrictions.GPSkill ? " (disabled GP Skills)" : string.Empty)}{gouBoost}:
{armorSkills}

Caravan Skills:
{caravanSkillsList}

Diva:
{(divaSkill == "None" ? "No Skill" : divaSkill)}{(questRestrictions.DivaSkill ? " (disabled skill)" : string.Empty)}
Song {(diva.DivaSongBuffOn > 0 ? "ON" : "OFF")}

Diva Prayer Gems{(questRestrictions.DivaPrayerGem ? " (disabled gems)" : string.Empty)}:
{GetDivaPrayerGems(diva)}

Guild:
{guildFood}
{GetGuildPoogieEffect(guildPoogie)}

Style Rank{(questRestrictions.SecretTechnique ? " (disabled secret technique)" : string.Empty)}{(questRestrictions.SoulRevival ? " (disabled soul revival)" : string.Empty)}{(questRestrictions.TwinHiden ? " (disabled twin hiden)" : string.Empty)}:
{styleRankSkillsList}

Items:
{inventory}

Ammo:
{ammo}

Poogie Item:
{poogieItem}

Road/Duremudira Skills:
{roadDureSkillsList}

Quest{(questRestrictions.QuestBonusReward ? " (disabled bonus reward)" : string.Empty)}: {questName}
{questObjectiveType} {questObjectiveQuantity} {questObjectiveName}
Category: {questCategory}
Party Size: {partySize}
Mode: {toggleModeName}
Active Feature {(activeFeatureState == true ? "ON" : "OFF")}{(questRestrictions.ActiveFeature ? " (disabled active feature)" : string.Empty)}

Courses{(questRestrictions.CourseAttack ? " (disabled course attack)" : string.Empty)}:
{courseInfo}

Halk:
{(halk.HalkOn > 0 ? "Active" : "Inactive")}{(questRestrictions.Halk ? " (disabled halk)" : string.Empty)}
Halk Pot {(halk.HalkPotEffectOn > 0 ? "ON" : "OFF")}{(questRestrictions.Sigil ? " (disabled halk pot)" : string.Empty)}
LV{halk.HalkLevel}
Element Type {GetHalkElement(halk)}
Status Type {GetHalkStatus(halk)}
Intimacy {halk.HalkIntimacy}
Health {halk.HalkHealth}
Attack {halk.HalkAttack}
Defense {halk.HalkDefense}
Intellect {halk.HalkIntellect}
{halkSkill1} | {halkSkill2} | {halkSkill3}

Overlay Hash: {overlayHash.OverlayHash}

Game Patch Information:
dat: {patchInfo.mhfdatInfo}
emd: {patchInfo.mhfemdInfo}
dll: {patchInfo.mhfodllInfo}
hddll: {patchInfo.mhfohddllInfo}

Weapon Specific:
Dual Swords dropped combos: {weaponBuffs.DualSwordsSharpensDictionary.Count((e) => e.Value == 0)}
");
        }
    }

    public string GetMainCourses(long? rights)
    {
        if (rights == null || rights <= 0 || rights > 0xFFFF)
        {
            return "None";
        }

        // Map the first and second bytes to course rights names
        var courseNames = MapCourseRightsToNames((long)rights, 0, typeof(CourseRightsSecondByte));
        // Join the names with commas
        return string.Join(", ", courseNames).Trim();
    }

    public string GetAdditionalCourses(long? rights)
    {
        if (rights == null || rights <= 0 || rights > 0xFFFF)
        {
            return "None";
        }

        // Map the first and second bytes to course rights names
        var courseNames = MapCourseRightsToNames((long)rights, 1, typeof(CourseRightsFirstByte));
        // Join the names with commas
        return string.Join(", ", courseNames).Trim();
    }

    public string GetMainCourses()
    {
        if (Rights() <= 0 || Rights() > 0xFFFF)
        {
            return "None";
        }

        // Map the first and second bytes to course rights names
        var courseNames = MapCourseRightsToNames((long)Rights(), 0, typeof(CourseRightsSecondByte));
        // Join the names with commas
        return string.Join(", ", courseNames).Trim();
    }

    public string GetAdditionalCourses()
    {
        if (Rights() <= 0 || Rights() > 0xFFFF)
        {
            return "None";
        }

        // Map the first and second bytes to course rights names
        var courseNames = MapCourseRightsToNames((long)Rights(), 1, typeof(CourseRightsFirstByte));
        // Join the names with commas
        return string.Join(", ", courseNames).Trim();
    }

    private IEnumerable<string> MapCourseRightsToNames(long rights, int bytePosition, Type enumType)
    {
        if ((rights < 0x0100 && bytePosition == 1) || rights == 0)
        {
            yield return "None";
            yield break;
        }

        foreach (var name in Enum.GetNames(enumType))
        {
            if (name == "None")
                continue;

            if (enumType == typeof(CourseRightsFirstByte))
            {
                if (IsBitfieldContainingFlag((uint)rights, (CourseRightsFirstByte)Enum.Parse(enumType, name), (uint)CourseRightsFirstByte.All, true, bytePosition))
                {
                    yield return name;
                }
            }
            else if (enumType == typeof(CourseRightsSecondByte))
            {
                if (IsBitfieldContainingFlag((uint)rights, (CourseRightsSecondByte)Enum.Parse(enumType, name), (uint)CourseRightsSecondByte.All, true, bytePosition))
                {
                    yield return name;
                }
            }
        }
    }

    public bool IsActiveFeatureOn(long activeFeature, long weaponTypeID)
    {
        if (activeFeature <= 0 || activeFeature > (long)ActiveFeature.All)
        {
            return false;
        }

        var weaponFlag = (ActiveFeature)Math.Pow(2, weaponTypeID);
        var activeFeatureFlags = (ActiveFeature)activeFeature;

        return activeFeatureFlags.HasFlag(weaponFlag);
    }

    public string GetHalkElement()
    {
        var element = "None";

        if (HalkFire() >= 100)
        {
            return "Fire";
        }
        if (HalkWater() >= 100)
        {
            return "Water";
        }
        if (HalkThunder() >= 100)
        {
            return "Thunder";
        }
        if (HalkIce() >= 100)
        {
            return "Ice";
        }
        if (HalkDragon() >= 100)
        {
            return "Dragon";
        }

        return element;
    }

    public string GetHalkStatus()
    {
        var status = "None";

        if (HalkPoison() >= 100)
        {
            return "Poison";
        }
        if (HalkSleep() >= 100)
        {
            return "Sleep";
        }
        if (HalkParalysis() >= 100)
        {
            return "Paralysis";
        }

        return status;
    }

    public string GetHalkElement(QuestsHalk halk)
    {
        var element = "None";

        if (halk.HalkFire >= 100)
        {
            return "Fire";
        }
        if (halk.HalkWater >= 100)
        {
            return "Water";
        }
        if (halk.HalkThunder >= 100)
        {
            return "Thunder";
        }
        if (halk.HalkIce >= 100)
        {
            return "Ice";
        }
        if (halk.HalkDragon >= 100)
        {
            return "Dragon";
        }

        return element;
    }

    public string GetHalkStatus(QuestsHalk halk)
    {
        var status = "None";

        if (halk.HalkPoison >= 100)
        {
            return "Poison";
        }
        if (halk.HalkSleep >= 100)
        {
            return "Sleep";
        }
        if (halk.HalkParalysis >= 100)
        {
            return "Paralysis";
        }

        return status;
    }

    public string GetGuildPoogieEffect(QuestsGuildPoogie poogie)
    {
        var effect = "No Poogie";

        if (poogie.GuildPoogie1Skill >= 1)
        {
            return EZlion.Mapper.SkillGuildPoogie.IDName[(int)poogie.GuildPoogie1Skill];
        }

        if (poogie.GuildPoogie2Skill >= 1)
        {
            return EZlion.Mapper.SkillGuildPoogie.IDName[(int)poogie.GuildPoogie2Skill];
        }

        if (poogie.GuildPoogie3Skill >= 1)
        {
            return EZlion.Mapper.SkillGuildPoogie.IDName[(int)poogie.GuildPoogie3Skill];
        }

        return effect;
    }

    public string GetGuildPoogieEffect()
    {
        var effect = "No Poogie";

        if (GuildPoogie1Skill() >= 1)
        {
            return EZlion.Mapper.SkillGuildPoogie.IDName[GuildPoogie1Skill()];
        }

        if (GuildPoogie2Skill() >= 1)
        {
            return EZlion.Mapper.SkillGuildPoogie.IDName[GuildPoogie2Skill()];
        }

        if (GuildPoogie3Skill() >= 1)
        {
            return EZlion.Mapper.SkillGuildPoogie.IDName[GuildPoogie3Skill()];
        }

        return effect;
    }

    public string GetGuildPoogieEffect(List<int> poogie)
    {
        var effect = "No Poogie";

        if (poogie[0] >= 1)
        {
            return EZlion.Mapper.SkillGuildPoogie.IDName[(int)poogie[0]];
        }

        if (poogie[1] >= 1)
        {
            return EZlion.Mapper.SkillGuildPoogie.IDName[(int)poogie[1]];
        }

        if (poogie[2] >= 1)
        {
            return EZlion.Mapper.SkillGuildPoogie.IDName[(int)poogie[2]];
        }

        return effect;
    }

    public string GetDivaPrayerGems(QuestsDiva diva)
    {
        var gems = string.Empty;
        var gemTypes = new long?[] { diva.DivaPrayerGemRedSkill, diva.DivaPrayerGemYellowSkill, diva.DivaPrayerGemGreenSkill, diva.DivaPrayerGemBlueSkill };
        var gemLevels = new long?[] { diva.DivaPrayerGemRedLevel, diva.DivaPrayerGemYellowLevel, diva.DivaPrayerGemGreenLevel, diva.DivaPrayerGemBlueLevel };

        for (var i = 0; i < gemTypes.Length; i++)
        {
            var skillId = gemTypes[i];

            if (skillId == null)
            {
                continue;
            }

            if (SkillDivaPrayerGem.IDName.TryGetValue((int)skillId, out var skillName)
                && skillName != "None" && skillName != string.Empty)
            {
                var gemColor = string.Empty;

                switch (i){
                    case 0:
                        gemColor = "Red";
                        break;
                    case 1:
                        gemColor = "Yellow";
                        break;
                    case 2:
                        gemColor = "Green";
                        break;
                    case 3:
                        gemColor = "Blue";
                        break;
                }

                gems += $"{gemColor} 💎 {skillName} LV{gemLevels[i]}";
                if (i != gemTypes.Length - 1)
                {
                    gems += "\n";
                }
            }
        }

        return string.IsNullOrEmpty(gems) ? "None" : gems;
    }

    public string GetDivaPrayerGems()
    {
        var gems = string.Empty;
        var gemTypes = new long?[] { DivaPrayerGemRedSkill(), DivaPrayerGemYellowSkill(), DivaPrayerGemGreenSkill(), DivaPrayerGemBlueSkill() };
        var gemLevels = new long?[] { DivaPrayerGemRedLevel(), DivaPrayerGemYellowLevel(), DivaPrayerGemGreenLevel(), DivaPrayerGemBlueLevel() };

        for (var i = 0; i < gemTypes.Length; i++)
        {
            var skillId = gemTypes[i];

            if (skillId == null)
            {
                continue;
            }

            if (SkillDivaPrayerGem.IDName.TryGetValue((int)skillId, out var skillName)
                && skillName != "None" && skillName != string.Empty)
            {
                var gemColor = string.Empty;

                switch (i)
                {
                    case 0:
                        gemColor = "Red";
                        break;
                    case 1:
                        gemColor = "Yellow";
                        break;
                    case 2:
                        gemColor = "Green";
                        break;
                    case 3:
                        gemColor = "Blue";
                        break;
                }

                gems += $"{gemColor} 💎 {skillName} LV{gemLevels[i]}";
                if (i != gemTypes.Length - 1)
                {
                    gems += "\n";
                }
            }
        }

        return string.IsNullOrEmpty(gems) ? "None" : gems;
    }

    /// <summary>
    /// red yellow green blue. type level.
    /// </summary>
    /// <param name="diva"></param>
    /// <returns></returns>
    public string GetDivaPrayerGems(List<int> diva)
    {
        var gems = string.Empty;
        var gemTypes = new int[] { diva[0], diva[2], diva[4], diva[6] };
        var gemLevels = new int[] { diva[1], diva[3], diva[4], diva[5] };

        for (var i = 0; i < gemTypes.Length; i++)
        {
            var skillId = gemTypes[i];

            if (SkillDivaPrayerGem.IDName.TryGetValue((int)skillId, out var skillName)
                && skillName != "None" && skillName != string.Empty)
            {
                var gemColor = string.Empty;

                switch (i)
                {
                    case 0:
                        gemColor = "Red";
                        break;
                    case 1:
                        gemColor = "Yellow";
                        break;
                    case 2:
                        gemColor = "Green";
                        break;
                    case 3:
                        gemColor = "Blue";
                        break;
                }

                gems += $"{gemColor} 💎 {skillName} LV{gemLevels[i]}";
                if (i != gemTypes.Length - 1)
                {
                    gems += "\n";
                }
            }
        }

        return string.IsNullOrEmpty(gems) ? "None" : gems;
    }

    public int CalculateTotalLargeMonstersHunted() => this.RoadFatalisSlain() +
            this.FirstDistrictDuremudiraSlays() +
            this.SecondDistrictDuremudiraSlays() +
            this.RathianHunted() +
            this.FatalisHunted() +
            this.YianKutKuHunted() +
            this.LaoShanLungHunted() +
            this.CephadromeHunted() +
            this.RathalosHunted() +
            this.DiablosHunted() +
            this.KhezuHunted() +
            this.GraviosHunted() +
            this.GypcerosHunted() +
            this.PlesiothHunted() +
            this.BasariosHunted() +
            this.MonoblosHunted() +
            this.VelocidromeHunted() +
            this.GendromeHunted() +
            this.IodromeHunted() +
            this.KirinHunted() +
            this.CrimsonFatalisHunted() +
            this.PinkRathianHunted() +
            this.BlueYianKutKuHunted() +
            this.PurpleGypcerosHunted() +
            this.YianGarugaHunted() +
            this.SilverRathalosHunted() +
            this.GoldRathianHunted() +
            this.BlackDiablosHunted() +
            this.WhiteMonoblosHunted() +
            this.RedKhezuHunted() +
            this.GreenPlesiothHunted() +
            this.BlackGraviosHunted() +
            this.DaimyoHermitaurHunted() +
            this.AzureRathalosHunted() +
            this.AshenLaoShanLungHunted() +
            this.BlangongaHunted() +
            this.CongalalaHunted() +
            this.RajangHunted() +
            this.KushalaDaoraHunted() +
            this.ShenGaorenHunted() +
            this.YamaTsukamiHunted() +
            this.ChameleosHunted() +
            this.RustedKushalaDaoraHunted() +
            this.LunastraHunted() +
            this.TeostraHunted() +
            this.ShogunCeanataurHunted() +
            this.BulldromeHunted() +
            this.WhiteFatalisHunted() +
            this.HypnocHunted() +
            this.VolganosHunted() +
            this.TigrexHunted() +
            this.AkantorHunted() +
            this.BrightHypnocHunted() +
            this.RedVolganosHunted() +
            this.EspinasHunted() +
            this.OrangeEspinasHunted() +
            this.SilverHypnocHunted() +
            this.AkuraVashimuHunted() +
            this.AkuraJebiaHunted() +
            this.BerukyurosuHunted() +
            this.PariapuriaHunted() +
            this.WhiteEspinasHunted() +
            this.KamuOrugaronHunted() +
            this.NonoOrugaronHunted() +
            this.DyuragauaHunted() +
            this.DoragyurosuHunted() +
            this.GurenzeburuHunted() +
            this.RukodioraHunted() +
            this.UnknownHunted() +
            this.GogomoaHunted() +
            this.TaikunZamuzaHunted() +
            this.AbioruguHunted() +
            this.KuarusepusuHunted() +
            this.OdibatorasuHunted() +
            this.DisufiroaHunted() +
            this.RebidioraHunted() +
            this.AnorupatisuHunted() +
            this.HyujikikiHunted() +
            this.MidogaronHunted() +
            this.GiaoruguHunted() +
            this.MiRuHunted() +
            this.FarunokkuHunted() +
            this.PokaradonHunted() +
            this.ShantienHunted() +
            this.GoruganosuHunted() +
            this.AruganosuHunted() +
            this.BaruragaruHunted() +
            this.ZerureusuHunted() +
            this.GougarfHunted() +
            this.ForokururuHunted() +
            this.MeraginasuHunted() +
            this.DiorexHunted() +
            this.GarubaDaoraHunted() +
            this.InagamiHunted() +
            this.VarusaburosuHunted() +
            this.PoborubarumuHunted() +
            this.GureadomosuHunted() +
            this.HarudomeruguHunted() +
            this.ToridclessHunted() +
            this.GasurabazuraHunted() +
            this.YamaKuraiHunted() +
            this.ZinogreHunted() +
            this.DeviljhoHunted() +
            this.BrachydiosHunted() +
            this.ToaTesukatoraHunted() +
            this.BariothHunted() +
            this.UragaanHunted() +
            this.StygianZinogreHunted() +
            this.GuanzorumuHunted() +
            this.StarvingDeviljhoHunted() +
            this.VoljangHunted() +
            this.NargacugaHunted() +
            this.KeoaruboruHunted() +
            this.ZenaserisuHunted() +
            this.GoreMagalaHunted() +
            this.BlinkingNargacugaHunted() +
            this.ShagaruMagalaHunted() +
            this.AmatsuHunted() +
            this.ElzelionHunted() +
            this.ArrogantDuremudiraHunted() +
            this.SeregiosHunted() +
            this.BogabadorumuHunted() +
            this.BombardierBogabadorumuHunted() +
            this.SparklingZerureusuHunted() +
            this.KingShakalakaHunted();

    public int CalculateTotalSmallMonstersHunted() => this.KelbiHunted() +
        this.MosswineHunted() +
        this.BullfangoHunted() +
        this.FelyneHunted() +
        this.AptonothHunted() +
        this.GenpreyHunted() +
        this.VelocipreyHunted() +
        this.VespoidHunted() +
        this.MelynxHunted() +
        this.HornetaurHunted() +
        this.ApcerosHunted() +
        this.RocksHunted() +
        this.IopreyHunted() +
        this.CephalosHunted() +
        this.GiapreyHunted() +
        this.GreatThunderbugHunted() +
        this.ShakalakaHunted() +
        this.BlangoHunted() +
        this.CongaHunted() +
        this.RemobraHunted() +
        this.HermitaurHunted() +
        this.AntekaHunted() +
        this.PopoHunted() +
        this.CeanataurHunted() +
        this.CactusHunted() +
        this.GorgeObjectsHunted() +
        this.BurukkuHunted() +
        this.ErupeHunted() +
        this.PokaraHunted() +
        this.UrukiHunted() +
        this.KusubamiHunted() +
        this.PSO2RappyHunted();

    /// <summary>
    /// Generates the compendium.
    /// </summary>
    /// <returns></returns>
    public string GenerateCompendium()
    {
        var createdBy = GetFullCurrentProgramVersion();
        var createdAt = DateTime.UtcNow;

        if (createdBy == null)
        {
            return "Program Version Not Found.\n\nReload the section.";
        }

        var questCompendium = DatabaseManagerInstance.GetQuestCompendium();
        var gearCompendium = DatabaseManagerInstance.GetGearCompendium();
        var performanceCompendium = DatabaseManagerInstance.GetPerformanceCompendium();
        var mezeportaFestivalCompendium = DatabaseManagerInstance.GetMezFesCompendium();
        var miscellaneousCompendium = DatabaseManagerInstance.GetMiscellaneousCompendium();
        var monsterCompendium = DatabaseManagerInstance.GetMonsterCompendium();

        var mostCompletedQuest = questCompendium.MostCompletedQuestRuns;
        var mostCompletedQuestAttempts = questCompendium.MostCompletedQuestRunsAttempted;
        var mostCompletedQuestID = questCompendium.MostCompletedQuestRunsQuestID;

        var mostAttemptedQuest = questCompendium.MostAttemptedQuestRuns;
        var mostAttemptedQuestCompletions = questCompendium.MostAttemptedQuestRunsCompleted;
        var mostAttemptedQuestID = questCompendium.MostAttemptedQuestRunsQuestID;

        var totalQuestsCompleted = questCompendium.TotalQuestsCompleted;
        var totalQuestsAttempted = questCompendium.TotalQuestsAttempted;

        var questCompletionTimeElapsedAverage = questCompendium.QuestCompletionTimeElapsedAverage;
        var questCompletionTimeElapsedMedian = questCompendium.QuestCompletionTimeElapsedMedian;

        var totalTimeElapsedDuringQuest = questCompendium.TotalTimeElapsedQuests;

        var mostCompletedQuestWithCarts = questCompendium.MostCompletedQuestWithCarts;
        var mostCompletedQuestWithCartsQuestID = questCompendium.MostCompletedQuestWithCartsQuestID;

        var totalCartsInQuest = questCompendium.TotalCartsInQuest;
        var totalCartsInQuestAverage = questCompendium.TotalCartsInQuestAverage;
        var totalCartsInQuestMedian = questCompendium.TotalCartsInQuestMedian;

        var questPartySizeAverage = questCompendium.QuestPartySizeAverage;
        var questPartySizeMedian = questCompendium.QuestPartySizeMedian;
        var questPartySizeMode = questCompendium.QuestPartySizeMode;

        var percentOfSoloQuests = questCompendium.PercentOfSoloQuests;
        var percentOfGuildFood = questCompendium.PercentOfGuildFood;
        var percentOfDivaSkill = questCompendium.PercentOfDivaSkill;
        var percentOfSkillFruit = questCompendium.PercentOfSkillFruit;
        var mostCommonDivaSkill = questCompendium.MostCommonDivaSkill;
        var mostCommonGuildFood = questCompendium.MostCommonGuildFood;

        var mostUsedWeaponType = gearCompendium.MostUsedWeaponType;
        var totalUniqueArmorPieces = gearCompendium.TotalUniqueArmorPiecesUsed;
        var totalUniqueWeapons = gearCompendium.TotalUniqueWeaponsUsed;
        var totalUniqueDecorations = gearCompendium.TotalUniqueDecorationsUsed;

        var mostCommonDecorationID = gearCompendium.MostCommonDecorationID;
        var leastUsedArmorSkill = gearCompendium.LeastUsedArmorSkill;

        var highestTrueRaw = performanceCompendium.HighestTrueRaw;
        var trueRawAverage = performanceCompendium.TrueRawAverage;
        var trueRawMedian = performanceCompendium.TrueRawMedian;
        var highestTrueRawRunID = performanceCompendium.HighestTrueRawRunID;

        var highestSingleHitDamage = performanceCompendium.HighestSingleHitDamage;
        var singleHitDamageAverage = performanceCompendium.SingleHitDamageAverage;
        var singleHitDamageMedian = performanceCompendium.SingleHitDamageMedian;
        var highestSingleHitDamageRunID = performanceCompendium.HighestSingleHitDamageRunID;

        var highestHitCount = performanceCompendium.HighestHitCount;
        var hitCountAverage = performanceCompendium.HitCountAverage;
        var hitCountMedian = performanceCompendium.HitCountMedian;
        var highestHitCountRunID = performanceCompendium.HighestHitCountRunID;

        var highestHitsTakenBlocked = performanceCompendium.HighestHitsTakenBlocked;
        var hitsTakenBlockedAverage = performanceCompendium.HitsTakenBlockedAverage;
        var hitsTakenBlockedMedian = performanceCompendium.HitsTakenBlockedMedian;
        var highestHitsTakenBlockedRunID = performanceCompendium.HighestHitsTakenBlockedRunID;

        var highestDPS = performanceCompendium.HighestDPS;
        var dPSAverage = performanceCompendium.DPSAverage;
        var dPSMedian = performanceCompendium.DPSMedian;
        var highestDPSRunID = performanceCompendium.HighestDPSRunID;

        var highestHitsPerSecond = performanceCompendium.HighestHitsPerSecond;
        var hitsPerSecondAverage = performanceCompendium.HitsPerSecondAverage;
        var hitsPerSecondMedian = performanceCompendium.HitsPerSecondMedian;
        var highestHitsPerSecondRunID = performanceCompendium.HighestHitsPerSecondRunID;

        var highestHitsTakenBlockedPerSecond = performanceCompendium.HighestHitsTakenBlockedPerSecond;
        var hitsTakenBlockedPerSecondAverage = performanceCompendium.HitsTakenBlockedPerSecondAverage;
        var hitsTakenBlockedPerSecondMedian = performanceCompendium.HitsTakenBlockedPerSecondMedian;
        var highestHitsTakenBlockedPerSecondRunID = performanceCompendium.HighestHitsTakenBlockedPerSecondRunID;

        var highestActionsPerMinute = performanceCompendium.HighestActionsPerMinute;
        var actionsPerMinuteAverage = performanceCompendium.ActionsPerMinuteAverage;
        var actionsPerMinuteMedian = performanceCompendium.ActionsPerMinuteMedian;
        var highestActionsPerMinuteRunID = performanceCompendium.HighestActionsPerMinuteRunID;

        var totalHitsCount = performanceCompendium.TotalHitsCount;
        var totalHitsTakenBlocked = performanceCompendium.TotalHitsTakenBlocked;
        var totalActions = performanceCompendium.TotalActions;

        var healthAverage = performanceCompendium.HealthAverage;
        var healthMedian = performanceCompendium.HealthMedian;
        var healthMode = performanceCompendium.HealthMode;

        var staminaAverage = performanceCompendium.StaminaAverage;
        var staminaMedian = performanceCompendium.StaminaMedian;
        var staminaMode = performanceCompendium.StaminaMode;

        var minigamesPlayed = mezeportaFestivalCompendium.MinigamesPlayed;

        var urukiPachinkoTimesPlayed = mezeportaFestivalCompendium.UrukiPachinkoTimesPlayed;
        var urukiPachinkoHighscore = mezeportaFestivalCompendium.UrukiPachinkoHighscore;
        var urukiPachinkoScoreAverage = mezeportaFestivalCompendium.UrukiPachinkoAverageScore;
        var urukiPachinkoScoreMedian = mezeportaFestivalCompendium.UrukiPachinkoMedianScore;

        var guukuScoopTimesPlayed = mezeportaFestivalCompendium.GuukuScoopTimesPlayed;
        var guukuScoopHighscore = mezeportaFestivalCompendium.GuukuScoopHighscore;
        var guukuScoopScoreAverage = mezeportaFestivalCompendium.GuukuScoopAverageScore;
        var guukuScoopScoreMedian = mezeportaFestivalCompendium.GuukuScoopMedianScore;

        var nyanrendoTimesPlayed = mezeportaFestivalCompendium.NyanrendoTimesPlayed;
        var nyanrendoHighscore = mezeportaFestivalCompendium.NyanrendoHighscore;
        var nyanrendoScoreAverage = mezeportaFestivalCompendium.NyanrendoAverageScore;
        var nyanrendoScoreMedian = mezeportaFestivalCompendium.NyanrendoMedianScore;

        var panicHoneyTimesPlayed = mezeportaFestivalCompendium.PanicHoneyTimesPlayed;
        var panicHoneyHighscore = mezeportaFestivalCompendium.PanicHoneyHighscore;
        var panicHoneyScoreAverage = mezeportaFestivalCompendium.PanicHoneyAverageScore;
        var panicHoneyScoreMedian = mezeportaFestivalCompendium.PanicHoneyMedianScore;

        var dokkanBattleCatsTimesPlayed = mezeportaFestivalCompendium.DokkanBattleCatsTimesPlayed;
        var dokkanBattleCatsHighscore = mezeportaFestivalCompendium.DokkanBattleCatsHighscore;
        var dokkanBattleCatsScoreAverage = mezeportaFestivalCompendium.DokkanBattleCatsAverageScore;
        var dokkanBattleCatsScoreMedian = mezeportaFestivalCompendium.DokkanBattleCatsMedianScore;

        var monster1AttackMultiplierHighest = monsterCompendium.HighestMonsterAttackMultiplier;
        var monster1AttackMultiplierHighestRunID = monsterCompendium.HighestMonsterAttackMultiplierRunID;

        var monster1AttackMultiplierLowest = monsterCompendium.LowestMonsterAttackMultiplier;
        var monster1AttackMultiplierLowestRunID = monsterCompendium.LowestMonsterAttackMultiplierRunID;

        var monster1DefenseRateHighest = monsterCompendium.HighestMonsterDefenseRate;
        var monster1DefenseRateHighestRunID = monsterCompendium.HighestMonsterDefenseRateRunID;

        var monster1DefenseRateLowest = monsterCompendium.LowestMonsterDefenseRate;
        var monster1DefenseRateLowestRunID = monsterCompendium.LowestMonsterDefenseRateRunID;

        var monster1SizeMultiplierHighest = monsterCompendium.HighestMonsterSizeMultiplier;
        var monster1SizeMultiplierHighestRunID = monsterCompendium.HighestMonsterSizeMultiplierRunID;

        var monster1SizeMultiplierLowest = monsterCompendium.LowestMonsterSizeMultiplier;
        var monster1SizeMultiplierLowestRunID = monsterCompendium.LowestMonsterSizeMultiplierRunID;

        var totalLargeMonstersHunted = this.CalculateTotalLargeMonstersHunted();
        var totalSmallMonstersHunted = this.CalculateTotalSmallMonstersHunted();

        var totalOverlaySessions = miscellaneousCompendium.TotalOverlaySessions;
        var sessionDurationHighest = miscellaneousCompendium.HighestSessionDuration;
        var sessionDurationLowest = miscellaneousCompendium.LowestSessionDuration;
        var sessionDurationAverage = miscellaneousCompendium.AverageSessionDuration;
        var sessionDurationMedian = miscellaneousCompendium.MedianSessionDuration;

        SkillArmor.IDName.TryGetValue((int)mostCommonGuildFood, out var mostCommonGuildFoodName);
        SkillDiva.IDName.TryGetValue((int)mostCommonDivaSkill, out var mostCommonDivaSkillName);
        EZlion.Mapper.WeaponType.IDName.TryGetValue((int)mostUsedWeaponType, out var mostUsedWeaponTypeName);
        Item.IDName.TryGetValue((int)mostCommonDecorationID, out var mostCommonDecorationName);
        SkillArmor.IDName.TryGetValue((int)leastUsedArmorSkill, out var leastUsedArmorSkillName);
        SkillGuildPoogie.IDName.TryGetValue((int)questCompendium.MostCommonGuildPoogie, out var mostCommonGuildPoogie);

        return string.Format(
            CultureInfo.InvariantCulture,
            $@"{createdAt} (UTC)
{createdBy}

Quest
Most Completed Quest: {mostCompletedQuest} (Attempted {mostCompletedQuestAttempts}) [Quest ID {mostCompletedQuestID}]
Most Attempted Quest: {mostAttemptedQuest} (Completed {mostAttemptedQuestCompletions}) [Quest ID {mostAttemptedQuestID}]
Total Quests Completed/Attempted: {totalQuestsCompleted}/{totalQuestsAttempted}
Quest Completion Time Elapsed (Average/Median): {TimeService.GetMinutesSecondsMillisecondsFromFrames((long)questCompletionTimeElapsedAverage)} / {TimeService.GetMinutesSecondsMillisecondsFromFrames((long)questCompletionTimeElapsedMedian)}
Total Time Elapsed during Quest: {TimeService.GetMinutesSecondsMillisecondsFromFrames(totalTimeElapsedDuringQuest)}
Most Completed Quest with Carts: {mostCompletedQuestWithCarts} [Quest ID {mostCompletedQuestWithCartsQuestID}]
Total Carts in Quest (Average/Median): {totalCartsInQuest} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", totalCartsInQuestAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", totalCartsInQuestMedian)})
Quest Party Size (Average/Median/Mode): {string.Format(CultureInfo.InvariantCulture, "{0:0.##}", questPartySizeAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", questPartySizeMedian)}/{questPartySizeMode}
Percent of Solo Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", percentOfSoloQuests)}
Percent of Guild Poogie in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", questCompendium.PercentOfGuildPoogie)}
Percent of Guild Food in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", percentOfGuildFood)}
Percent of Diva Song in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", questCompendium.PercentOfDivaSong)}
Percent of Diva Skill in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", percentOfDivaSkill)}
Percent of Diva Prayer Gem in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", questCompendium.PercentOfDivaPrayerGem)}
Percent of Skill Fruit in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", percentOfSkillFruit)}
Percent of Halk in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", questCompendium.PercentOfHalkOn)}
Percent of Halk Pot Effect in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", questCompendium.PercentOfHalkPotEffectOn)}
Percent of Active Feature in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", questCompendium.PercentOfActiveFeature)}
Percent of Course Attack Boost in Quests: {string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", questCompendium.PercentOfCourseAttackBoost)}
Most Common Guild Poogie in Quests: {mostCommonGuildPoogie}
Most Common Guild Food in Quests: {mostCommonGuildFoodName}
Most Common Diva Skill in Quests: {mostCommonDivaSkillName}

Gear
Most Used Weapon Type: {mostUsedWeaponTypeName}
Total Unique Armor Pieces/Weapons/Decorations used: {totalUniqueArmorPieces}/{totalUniqueWeapons}/{totalUniqueDecorations}
Most Common Decoration: {mostCommonDecorationName} [ID {mostCommonDecorationID.ToString("X", CultureInfo.InvariantCulture)}]
Least Used Armor Skill: {leastUsedArmorSkillName}

Hunter Performance
Highest True Raw (Average/Median): {highestTrueRaw} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", trueRawAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", trueRawMedian)}) [Run ID {highestTrueRawRunID}]
Highest Single Hit Damage (Average/Median): {highestSingleHitDamage} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", singleHitDamageAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", singleHitDamageMedian)}) [Run ID {highestSingleHitDamageRunID}]
Highest Hit Count (Average/Median): {highestHitCount} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", hitCountAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", hitCountMedian)}) [Run ID {highestHitCountRunID}]
Highest Hits Taken/Blocked (Average/Median): {highestHitsTakenBlocked} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", hitsTakenBlockedAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", hitsTakenBlockedMedian)}) [Run ID {highestHitsTakenBlockedRunID}]
Highest DPS (Average/Median): {string.Format(CultureInfo.InvariantCulture, "{0:0.##}", highestDPS)} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", dPSAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", dPSMedian)}) [Run ID {highestDPSRunID}]
Highest Hits per Second (Average/Median): {string.Format(CultureInfo.InvariantCulture, "{0:0.##}", highestHitsPerSecond)} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", hitsPerSecondAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", hitsPerSecondMedian)}) [Run ID {highestHitsPerSecondRunID}]
Highest Hits Taken/Blocked per Second (Average/Median): {string.Format(CultureInfo.InvariantCulture, "{0:0.##}", highestHitsTakenBlockedPerSecond)} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", hitsTakenBlockedPerSecondAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", hitsTakenBlockedPerSecondMedian)}) [Run ID {highestHitsTakenBlockedPerSecondRunID}]
Highest Actions per Minute (Average/Median): {string.Format(CultureInfo.InvariantCulture, "{0:0.##}", highestActionsPerMinute)} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", actionsPerMinuteAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", actionsPerMinuteMedian)}) [Run ID {highestActionsPerMinuteRunID}]
Total Hits Count: {totalHitsCount}
Total Hits Taken/Blocked: {totalHitsTakenBlocked}
Total Actions: {totalActions}
Health (Average/Median/Mode): {string.Format(CultureInfo.InvariantCulture, "{0:0.##}", healthAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", healthMedian)}/{healthMode}
Stamina (Average/Median/Mode): {string.Format(CultureInfo.InvariantCulture, "{0:0.##}", staminaAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", staminaMedian)}/{staminaMode}

Mezeporta Festival (MezFes)
Minigames Played: {minigamesPlayed}
Uruki Pachinko Times Played: {urukiPachinkoTimesPlayed}
Uruki Pachinko High-score (Average/Median): {urukiPachinkoHighscore} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", urukiPachinkoScoreAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", urukiPachinkoScoreMedian)})
Guuku Scoop Times Played: {guukuScoopTimesPlayed}
Guuku Scoop High-score (Average/Median): {guukuScoopHighscore} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", guukuScoopScoreAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", guukuScoopScoreMedian)})
Nyanrendo Times Played: {nyanrendoTimesPlayed}
Nyanrendo High-score (Average/Median): {nyanrendoHighscore} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", nyanrendoScoreAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", nyanrendoScoreMedian)})
Panic Honey Times Played: {panicHoneyTimesPlayed}
Panic Honey High-score (Average/Median): {panicHoneyHighscore} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", panicHoneyScoreAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", panicHoneyScoreMedian)})
Dokkan Battle Cats Times Played: {dokkanBattleCatsTimesPlayed}
Dokkan Battle Cats High-score (Average/Median): {dokkanBattleCatsHighscore} ({string.Format(CultureInfo.InvariantCulture, "{0:0.##}", dokkanBattleCatsScoreAverage)}/{string.Format(CultureInfo.InvariantCulture, "{0:0.##}", dokkanBattleCatsScoreMedian)})

Monster
Highest Monster Attack Multiplier: {monster1AttackMultiplierHighest} [Run ID {monster1AttackMultiplierHighestRunID}]
Lowest Monster Attack Multiplier: {monster1AttackMultiplierLowest} [Run ID {monster1AttackMultiplierLowestRunID}]
Highest Monster Defense Rate: {monster1DefenseRateHighest} [Run ID {monster1DefenseRateHighestRunID}]
Lowest Monster Defense Rate: {monster1DefenseRateLowest} [Run ID {monster1DefenseRateLowestRunID}]
Highest Monster Size Multiplier: {monster1SizeMultiplierHighest} [Run ID {monster1SizeMultiplierHighestRunID}]
Lowest Monster Size Multiplier: {monster1SizeMultiplierLowest} [Run ID {monster1SizeMultiplierLowestRunID}]
Total Large Monsters Hunted: {totalLargeMonstersHunted}
Total Small Monsters Hunted: {totalSmallMonstersHunted}

Miscellaneous
Total Overlay Sessions: {totalOverlaySessions}
Session Duration (Highest/Lowest/Average/Median): {TimeSpan.FromSeconds(sessionDurationHighest).ToString(TimeFormats.HoursMinutesSecondsMilliseconds, CultureInfo.InvariantCulture)} / {TimeSpan.FromSeconds(sessionDurationLowest).ToString(TimeFormats.HoursMinutesSecondsMilliseconds, CultureInfo.InvariantCulture)} / {TimeSpan.FromSeconds(sessionDurationAverage).ToString(TimeFormats.HoursMinutesSecondsMilliseconds, CultureInfo.InvariantCulture)} / {TimeSpan.FromSeconds(sessionDurationMedian).ToString(TimeFormats.HoursMinutesSecondsMilliseconds, CultureInfo.InvariantCulture)}
");
    }

    public static string GetGameArmorSkillsHealthAndStamina => "Health and Stamina\n\nHealth\t○\tHealth+50\t40\tMaximum Health +50\n\nHealth+40\t30\tMaximum Health +40\n\nHealth+30\t20\tMaximum Health +30\n\nHealth+20\t15\tMaximum Health +20\n\nHealth+10\t10\tMaximum Health +10\n\nHealth-10\t-10\tMaximum Health -10\n\nHealth-20\t-15\tMaximum Health -20\n\nHealth-30\t-20\tMaximum Health -30\n\nRecovery Speed\t○\tRecovery Speed+2\t20\t4x Health Recovery Speed\n\nRecovery Speed+1\t10\t3x Health Recovery Speed\n\nRecovery Speed-1\t-10\t3x Slower Health Recovery Speed\n\nRecovery Speed-2\t-20\t4x Slower Health Recovery Speed\n\nHunger\t○\tHunger Negated\t15\tStamina bar length does not decrease in length over time.\n\nHunger Halved\t10\tStamina bar length decreases at 0.5x the speed over time.\n\nHunger Up (Sm)\t-10\tStamina bar length decreases at 1.5x the speed over time\n\nHunger Up (Lg)\t-15\tStamina bar length decreases at 2.0x the speed over time\n\nRecovery\t×\tRecovery Items Up\t10\tRecovery item effect boosted 1.25x\n\nRecovery Items Down\t-10\tRecovery item effectlowered to 0.75x\n\nVampirism\t×\tVampirism+2\t20\tWhen attacking a monster, there is an 80% that your Health will recover.\n\nVampirism+1\t10\tWhen attacking a monster, there is a 60% that your Health will recover.\n\nHerbal Science\t×\tMedical Sage\t10\tInstant recovery of any Red HP when using any healing item.\n\nAdditional effects if multiple party members have the skill：\n\n2Players: recovery items apply to the entire party.\n\n3Players: recovery items apply to the entire party, +20extra HP Recovery\n\n4Players: recovery items apply to the entire party, +50extra HP Recovery\n\n※※Stacks with Recovery skill, only pure recovery items are party wide (e.g.Max Potions yes,Ancient Potions no.)\n\nStamina Recovery\t○\tStamina Recovery Up【Large】\t20\tDoubles stamina recovery speed over time\n\nStamina Rec (Small)\t10\tStamina recovery speed increases by 1.5 times over time.\n\nStamina Rec Down\t-10\tHalves stamina recovery speed over time\n\nStamina\t○\t\n\nStamina values usually decrease by 15 units\n\nPeerless\t20\tStamina decrease rate is halved (decrease is reduced to 8 units)\n\nIn addition, the reduction of stamina when evading or guarding is reduced to 50%.\n\nMarathon Runner\t10\tStamina decrease rate is halved (decrease is reduced to 8 units)\n\nIn addition, the reduction of stamina when evading or guarding is reduced to 75%.\n\nShort Sprinter\t-10\tStamina depletion speed is increased by 1.2 times (up to 18 units)\n\nEating\t×\tSpeed Eating\t10\tIncreases the speed of eating consumables such as meat and healing potions.\n\nSlow Eating\t-10\tReduces the speed of eating consumables such as meat and healing potions.\n\nGluttony\t×\tScavenger\t15\tUsing consumables restores 25 maximum stamina\n\nGourmand\t10\tStamina recovery when eating meat is increased by 25";

    public static string GetGameArmorColors => "Armour and Icon Colours\r\nIn Frontier armour colours are unlocked by doing a number of different tasks. These colours are also the colour that is displayed behind your weapon icon in the town.\r\nColours other than Green, Red and White require that you have reached at least HR3 to be used.\r\nThe menu to actually customise the colour used is unlocked by simply creating or otherwise obtaining an armour piece that lets you set its colour. The absolute easiest way to do this is to buy the T-Shirt that the NPC wearing glasses in the Blacksmith sells.\r\n\n" +
                "Colours other than Green, Red and White require that you have reached at least HR3 to be used." +
                "Colour Requirements\r\n◆ Green\r\nCompletion of a large number of Delivery Quests\r\nEach Delivery quest adds +3 points, 100 points required.\r\n◆ Red\r\nCompletion of a large number of Slaying Quests\r\nSlaying a main objective adds +2 points, Sub-quests +1, 100 points required\r\n◆ White\r\nCapturing a large number of Monsters\r\nEach capture adds +3 points, 100 points required\r\nIf your total points for Green, Red or White pass 100, the total points of all three goes down by -30.\r\n◆ Blue Have 50 Friends at once on your friendslist\r\n◆ Dark Blue Have 20 or more Rare-7 weapons or higher of each type (excluding tonfas)\r\n◆ Light Green 35x any armour pieces that are needed for Well Dressed title\r\n◆ Purple Achieve a single PVP victory\r\n◆ Orange Complete a large number of Hunter Guide Challenges\r\n◆ Yellow 15 or more Large Gold Crown Monsters\r\n◆ Grey Eat 100 times in your house\r\n◆ Pink Contribute 5000 Souls during the festival\r\n◆ Rainbow Unlock all other colours\r\n\n" +
                "Brief Unlocking Guides\r\n White,Red and Green\r\nGreen is easily earned while working on your Caravan Gem, delivering 3 iron ore grants you the same colours as killing a monster but takes only a matter of seconds. These deliveries all count towards the green colour so in the space of an hour or so you can easily finish up the colour\n\n" +
                "Red\n" +
                "Red you will almost certainly unlock naturally just by playing, it is likely to be the first colour you unlock.\n\n" +
                "White can be pretty hard to get at this point in frontier due to newer content consisting solely of monsters you cannot actually capture. The best way to go about getting these consistent is to buy 2x 捕獲珠Ｇ from the road store,these giveCapture Guru which will cause monsters to blink on the mini-map when they are able to be captured. You can also get Auto-Tracker from using 3x 洞察珠Ｇ but this is much less practical and you can simply use paintballs and psychoserums instead.\n\n" +
                "Blue\n" +
                "As the friend list is only on your end and does not produce a mutual prompt you can simply add 50 random other\r\nplayers and then delete them afterwards (Do actually delete them, or else you will spam them with messages whenever you sign in).\n\n" +
                "Dark Blue\n" +
                "For Dark Blue you have to craft 20 Rare-7 weapons of each type to unlock the Weapon Maniac titles\n" +
                "Duplicate weapons do not count, G Rank weapons do not count as being overRare-7 and Tonfas and Switch Axe titles are not considered for this\n" +
                "Pure raw SP Weapons upgrade to Rare-7 and bowguns can outright be made at Rare-7 or higher unlike most other weapons. Make whichever type you have the most materials for\r\n\n" +
                "Light Green\n" +
                "Light Green can be unlocked by making 35 of theChest Piece: 廚師服飾\n" +
                "This only needs Raw Meat and items that can be bought at the Pugi Farm store:\n" +
                "熱烤肉 x105 Buyable (Pugi Farm Store Lv3)\r\n辣椒 x210 Buy-able Herb\r\n生肉 x210 Raw Meat (Carves)\r\n調味生肉 x105Combine生肉 + 辣椒 (Raw Meat + Buyable Herb)\n\n" +
                "Purple\n" +
                "You will need to have a pet monster,to get one you need to go talk to the Blue NPC in town and choose:\n" +
                "獵人道場 > 選擇演習 > 鬥技演習\r\nThis will bring up a list ofArena quests with preset gear, you will need to do the get a pet. The quests are hunting 2Velocidromes, hunting 1 Gypceros and hunting 1 Rathian.\n" +
                "After this you can capture certain monsters as pets by simply capturing them while hosting the quest.When you get one as a pet you will be presented with a fairly obvious 'do you want to keep this monster' screen and will have to choose one of 3 slots for the monster. Dromes have incredibly high capture rates but many monsters are fairly easy to capture.\n" +
                "When you and one other have a pet simply choose 鬥技大會 on the same NPC to set up a fight. After you win one match you will get the Purple Colour.\n\n" +
                "Yellow\n" +
                "Hunt things until you get it naturally. There's so many monsters that simply trying out their normal and HC versions makes it pretty likely to unlock.\n\n" +
                "Grey / Black\n" +
                "Eat in your house a lot. There is no way to speed it up.\n\n" +
                "Pink\n" +
                "Participate in the Hunter Festival. 5000 Souls required.";

    public static string GetGameArmorSkillsAttackSkills => "Attack Skills\n\nStrong Attack\t×\tStrong Attack+6\t50\tIncreases attack power by 200. Does not overlap with skills that increase attack power\n\nStrong Attack+5\t40\tIncreases attack power by 150. Does not overlap with skills that increase attack power\n\nStrong Attack+4\t30\tIncreases attack power by 80. Does not overlap with skills that increase attack power\n\nStrong Attack+3\t20\tIncreases attack power by 50. Does not overlap with skills that increase attack power\n\nStrong Attack+2\t15\tIncreases attack power by 35. Does not overlap with skills that increase attack power\n\nStrong Attack+1\t10\tIncreases attack power by 20. Does not overlap with skills that increase attack power\n\nAttack\t○\tAttack Up(Absolute)\t80\tAttack power increases by 50\n\nAttack Up (Very Large)\t50\tAttack power increases by 30\n\nAttack Up (Large)\t25\tAttack power increases by 20\n\nAttack Up (Medium)\t15\tAttack power increases by 10\n\nAttack Up (Small)\t10\tAttack power increases by 5\n\nLone Wolf\t×\tLone Wolf\t10\tIncreases attack power by 100 when there are no Hunters or Rastas in the same area.\n\nHalks are allowed\n\nAdrenaline\t×\t\n\nNormally, at 40 HP, attack power remains unchanged, but defense increases by 60.\n\nAdrenaline+2\t15\n\n∥\n\n20\tAttack power increases by 1.5 times (1.3 times for Bowgun) when HP reaches 40.\n\nDefense increased to 90\n\nAdrenaline+1\t10\n\n21\tWhen HP reaches 40, defense increased to 90\n\nWorry\t-15\n\n30\tWhen HP reaches 40, attack power is reduced by 0.7 times. Defense increase is reduced to 42\n\nIssen\t×\tIssen+3\t30\tIncreases crit rate by 20% and changes critical multiplier to 1.50 (from 1.25)\n\nIssen+2\t20\tIncreases crit rate by 10% and changes critical multiplier to 1.40 (from 1.25)\n\nIssen+1\t10\tIncreases crit rate by 5% and changes critical multiplier to 1.35 (from 1.25)\n\nExpert\t○\tCritical Eye+5\t50\tAffinity +50%\n\nCritical Eye+4\t35\tAffinity +40%\n\nCritical Eye+3\t20\tAffinity +30%\n\nCritical Eye+2\t15\tAffinity +20%\n\nCritical Eye+1\t10\tAffinity +10%\n\nSkilled\t×\tSkilled\t15\tGrants the effects of Speed Eater, Movement Speed+2, True Guts and Weapon Handling.\n\nTrained\t×\tTrained+2\t15\tGrants Focus+2 and Kickboxing King. Stops hits interacting with players and AI.\n\nTrained+1\t10\tGrants Focus+1 and Martial Arts. Stops hits interacting with players and AI.\n\nAbnormality\t×\tAbnormality\t15\tGrants Status Attack Up, Status Pursuit and Drug Knowledge.\n\nCrit Conversion\t×\tCrit Conversion\t10\tAffinity increases by 30%\n\nWhen Affinity Rate is 101% or higher, attack power increases by √(Excess Affinity x 7)\n\nExploit Weakness\t×\tExploit Weakness\t20\tWhen attacking a part with a value of 35 or higher, +5 to the value of the target part. Attribute damage remains the same\n\nReduce Weakness\t-10\tWhen attacking a part with a value of 35 or higher, -5 to the value of the target part. Attribute damage remains the same\n\nStylish Assault\t×\tStylish Assault\t15\tEvading through attacks increases attack by +100 true raw for a time period that varies based on weapon class.\n\nSnS,DS,Tonfa: 14 Seconds、Heavy Bowgun:24 Seconds\n\nAll Others: 19 Seconds\n\nDissolver\t×\tElemental Exploit\t15\tIncreases monsters' hitzones elemental effectiveness values by a value that varies based on weapon class when they are 20 or higher.\n\nSnS,GS, LS,Hammer,Hunting Horn, Lance,Switch Axe F,Magnet Spike: +15\n\nDual Swords,Gunlance, LightBowgun:+10\n\nTonfas,Heavy Bowgun,Bow:+5\n\nElemental Diffusion\t-10\tDecreases monsters' hitzones elemental effectiveness values -5 when they are 20 or higher.\n\nStatus Assault\t×\tStatus Pursuit\t10\tInflicts additional damage with status attacks on monsters suffering status ailments. *However, there is no effect on Raviente.\n\nDrug Knowledge\t×\tDrug Knowledge\t10\tThe attack power of weapons with status attributes is increased by 1/4 of the abnormal status value, and status damage is dealt on every hit.\n\n（※status value on weapon is set to 0.38x with 25% of the true base status）\n\nStatus Attack\t×\tStatus Attack Up\t10\tPoison,Paralysis and Sleep multiplied by 1.125x\n\nBomb Boost\t×\tBomber\t10\tBomb Strength 1.5x、Bombs Combination Rate 100%, Blast Element Damage increased.\n\nGunnery\t○\tArtillery God\t35\tBallista: Damage x1.3\n\nGunlance: Shelling Raw Attribute x1.3,Wyvern Fire Raw Attribute x1.4\n\nBowguns: Crag/Clust S Raw Attribute x1.5,HBG Fire Beam damage x1.3\n\nTonfa: Ryuuki Detonation damage x1.6\n\nArtillery Expert\t20\tBallista: Damage x1.2\n\nGunlance: Shelling Raw Attribute x1.2,Wyvern Fire Raw Attribute x1.3\n\nBowguns: Crag/Clust S Raw Attribute x1.5,HBG Fire Beam damage x1.2\n\nTonfa: Ryuuki Detonation damage x1.4\n\nGunnery\t10\tBallista: Damage x1.1\n\nGunlance: Shelling Raw Attribute x1.1,Wyvern Fire Raw Attribute x1.2\n\nBowguns: Crag/Clust S Raw Attribute x1.5,HBG Fire Beam damage x1.1\n\nTonfa: Ryuuki Detonation damage x1.25\n\nFire Attack\t○\tFire Attack (Large)\t20\tFire Attack 1.2x\n\nFire Attack (Small)\t10\tFire Attack 1.1x\n\nWater Attack\t○\tWater Attack (Large)\t20\tWater Attack 1.2x\n\nWater Attack (Small)\t10\tWater Attack 1.1x\n\nThunder Attack\t○\tThunder Attack (Large)\t20\tThunder Attack 1.2x\n\nThunder Attack (Small)\t10\tThunder Attack 1.1x\n\nIce Attack\t○\tIce Attack (Large)\t20\tIce Attack 1.2x\n\nIce Attack (Small)\t10\tIce Attack 1.1x\n\nDragon Attack\t○\tDragon Attack (Large)\t20\tDragon Attack 1.2x\n\nDragon Attack (Small)\t10\tDragon Attack 1.1x\n\nElemental Attack\t×\tElemental Attack Up\t10\tAll Elements 1.1x\n\nElemental Attack Down\t-10\tAll Elements 0.9x\n\nMartial Arts\t○\tKickboxing King\t20\tKick deals 15damage and becomes a roundhouse kick while\n\nstanding or a drop kick while moving.Tonfa kick attacks deal higherdamage as well as adding more air time to all aerial attacks.\n\nMartial Arts\t10\tKick deals 15 damage.Tonfa kick attacks deal slightly higher Damage\n\nSurvivor\t×\tTenacity\t20\tEach time the number of remaining revivals in the quest decreases,\n\n1st time: Attack power +15, Defense +100\n\n2nd time: Attack power +20, Defense +150\n\n※Does not work in Great Slaying and Caravan Quests\n\nFortify\t10\tEach time the number of remaining revivals in the quest decreases,\n\nEach time the number of remaining revivals in the quest decreases,\n\n1st time: Attack power +5, Defense +50\n\n2nd time: Attack power +10, Defense +100\n\n※Does not work in Great Slaying and Caravan Quests\n\nRage\t×\tBuchigiri\t20\tTrue Guts and Adrenaline+2\n\nWrath Awoken\t15\tTrue Guts and Adrenaline+1\n\nFasting\t×\tStarving Wolf+2\t20\tIncreases affinity by 50%, grants evade+2, and critical hit modifier to 1.35x when hungry\n\n(Active at 25 Stamina *minimum bar*)\n\nStarving Wolf+1\t10\tIncreases affinity by 50%, grants evade+1\n\nPriority is given to higher skills (Active at 25 Stamina *minimum bar*)\n\nFocus\t○\tFocus+2\t20\t0.8x Charge attack duration for GS,Hammer,Long Sword,HBG,Lance,Tonfa and Switch Axe F where appropriate.\n\n1.2x fill rate for gauges (LS Spirit, SwAxe Sword and Tonfa)\n\nHBG heat beam meter filled an extra 2 units per shot (no effect for bows)\n\nFocus+1\t10\t0.9x Charge attack duration for GS,Hammer,Long Sword,HBG,Lance,Tonfa and Switch Axe F where appropriate.\n\n1.1x fill rate for gauges (LS Spirit, SwAxe Sword and Tonfa)\n\nHBG heat beam meter filled an extra 1 unit per shot (no effect for bows)\n\nDistraction\t-10\t1.2x Charge attack duration for GS,Hammer,Long Sword,HBG,Lance,Tonfa and Switch Axe F where appropriate.\n\n0.8x fill rate for gauges (LS Spirit, SwAxe Sword and Tonfa)\n\nHBG heat beam meter loses 1 unit per shot (no effect for bows)\n\nCharge Attack Up\t×\tCharge Attack Up+2\t20\tGreatsword, long sword, hammer, lance, and tonfa damage and attribute values are further increased when charging attacks.\n\nCan be used in combination with 「Focus」\n\nCharge Attack Up+1\t10\tGreatsword, long sword, hammer, lance, and tonfa damage and attribute values are increased when charging attacks.\n\nCan be used in combination with 「Focus」\n\nWeapon Handling\t×\tWeapon Handling\t10\tWeapon draw/sheath speed increased by 20% 「Does not stack with Master Seal or G Finesse weapon effects」\n\nShiriagari\t×\tShiriagari\t15\tAttack power (multiplier) increases according to the elapsed time of the quest\n\n1 min +20, 3 min +50, 5 min +80, 10 min +130, 15 min +180, 20 min +200\n\nAdaptation\t×\tAdaptation+2\t20\tWhen HP is over 100, other attack systems are added based on hitbox values using the multipliers 0.81x for melee and 0.72x for range\n\nType of damage changes to the type with the best result. (ex. Hammers can cut tails if hitbox values for cutting are higher on tail)\n\nAdaptation+1\t10\tWhen HP is over 100, other attack systems are added based on hitbox values using the multipliers 0.72x for melee and 0.64x for range\n\nType of damage changes to the type with the best result. (ex. Hammers can cut tails if hitbox values for cutting are higher on tail)\n\nCombat Supremacy\t×\tCombat Supremacy\t10\tAttack increases by 1.2 times while weapon is unsheathed. However, stamina will always decrease while the effect is active.\n\n※Even if the stamina runs out, the effect will continue as long as the sword is not sheathed.\n\n※Effect is nullified if Rasta is activated\n\nVigorous\t×\tVigorous\t10\tWhen HP is 100 or more, attack power increases by 1.15x\n\nLavish Attack\t×\tConsumption Slayer\t10\tAttack+100 at the cost of additional sharpness loss per hit with Blademaster weapons\n\nadditional 0.2x multiplier on coatings at the cost of double consumption per shot. (no effect on bowguns)\n\nObscurity\t×\tObscurity\t10\tYour attack will increase by a set value every time you block an attack, up to 10x. Values differ per weapon.\n\nSnS,Lance,Gunlance,Tonfa: 1-5 Blocked Attacks +40, 6-10 Blocked Attacks +20,Max Buff +300.\n\nSwitch Axe F,GS,Magnet Spike: 1-5 Blocked Attacks +30, 6-10 Blocked Attacks +15,Max Buff +225\n\nLong Sword: 1-5 Blocked Attacks +20, 6-10 Blocked Attacks +10,Max Buff +150\n\nRush\t×\tRush\t10\tSuccessful attacks and guard actions gradually increase an invisible meter that has two stages.\n\nStage one: Purple flash and +50 attack, Stage two: repeate flash and +130 attack. running with weapons now also costs 0 stamina.\n\nThe effect is completely reset when you use any items or sheathe your weapon.\n\nCeaseless\t×\tCeaseless\t10\tIncreases affinity and critical multiplier as you land attacks\n\nDissapears if you stop attacking for a number of seconds.\n\nReflect and Stylish Up count towards hit totals but Fencing+2 does not.\n\nPoint Breakthrough\t×\tPoint Breakthrough\t10\tIncreases the Raw Weakness Value of a part that has been hit repeatedly by +5.\n\nThe effect has a time limit and is only applied to a single part at a time.\n\nFurious\t×\tFurious\t10\tIncrease attack, affinity, elemental and status across 3 stages as you perform attacks, evasions or guards.\n\nThunder Clad\t×\tThunder Clad\t10\tGauge is accumulated by evading, attacking, and moving.\n\nWhen the gauge reaches its maximum, \"Status Negate\", \"Movement speed up +2\", \"Weapon handling\", and \"Evasion distance up\" will be activated\n\nHowever, if a skill of the same system is activated, the skill with the highest effect takes precedence.\n\nAlso, movement speed increases for a certain period of time when the sword is drawn, and damage to the monster parts always increases.\n\nAttribute damage is unchanged\n\n※Part damage increase stacks with「Weakness Exploit」 and 「Solid Determination」\n\nDetermination\t×\tSolid Determination\t10\tAttack power rises and Affinity increases by 100%.\n\nCrit damage is also increased、「Adrenaline+2」 and 「Sharpness+1」 are also activated、\n\nDamage to the attacked parts and attribute damage is always increased.\n\nAlso, damage increases when certain bullets and arrows hit at a critical distance.\n\nDoes not stack with、「Attack」「Strong Attack」「Issen」「Exploit Weakness」「Elemental Attack」「Precision」「Critical Shot」「Expert」、\n\nIf skills of the same type are activated, the skill with the highest effect takes precedence.\n\n「Absolute Defense」「True Guts」「Great Guts」「Guts」 are disabled\n\nas well as「Guts Tickets」「Mega Guts Ticket」 and 「Soul Revival」";

    public static string GetGameArmorSkillsDefensiveSkills => "Defensive Skills\n\nVitality\t×\tVitality+3\t30\tDefense+90,Damage Recovery Speed+2,Recovery Items Up,\n\n100% Combination success-rate for Potions,Mega Potions and Max Potions.\n\nVitality+2\t15\tDefense+45,Damage Recovery Speed+1,Recovery Items Up,\n\n100% Combination success-rate for Potions,Mega Potions and Max Potions.\n\nVitality+1\t10\tDefense+15,Damage Recovery Speed+1,Recovery Items Up,\n\n100% Combination success-rate for Potions,Mega Potions and Max Potions.\n\nVitality-1\t-10\tHealth-10,Damage Recovery Speed-1,Recovery Items Weakened.\n\nReflect\t×\tReflect+3\t20\tBlocking an attack causes a special motion to trigger from the point of guarding with an impact based MotionValue of 48.\n\nThe hit uses your current attack and sharpness values.\n\nReflect+2\t15\tBlocking an attack causes a special motion to trigger from the point of guarding with an impact based MotionValue of 36.\n\nThe hit uses your current attack and sharpness values.\n\nReflect+1\t10\tBlocking an attack causes a special motion to trigger from the point of guarding with an impact based MotionValue of 24.\n\nThe hit uses your current attack and sharpness values.\n\nDefense\t○\tDefense+120\t45\tIncreases defense by 120\n\nDefense+90\t35\tIncreases defense by 90\n\nDefense+60\t25\tIncreases defense by 60\n\nDefense+30\t15\tIncreases defense by 30\n\nDefense+20\t10\tIncreases defense by 20\n\nDefense-20\t-10\tDecrease defense by 20\n\nDefense-30\t-15\tDecrease defense by 30\n\nDefense-40\t-20\tDecrease defense by 40\n\nGuard\t○\t\n\nNo damage if not significantly attacked.\n\nWeapon type\tPower Value1～15\tPower Value16～39\tPower Value40+\n\nSnS\tSlight KB\tLarge KB\tLarge KB\n\nGS\tStand Still\tSlight KB\tLarge KB\n\nLance\tStand Still\tStand Still\tLarge KB\n\nGuard+2\t20\tMaximum blocking damage reduction. Further decreases knock-back and stamina consumption for blocking.\n\nAllows for blocking of some previously unblockable attacks\n\nIncreases Lance's shield aura hit limit to 6.\n\nGuard+1\t10\tIncreases blocking damage reduction.Decreases knock-back and stamina consumption forblocking.\n\nIncreases Lance's shield aura hit limit to 4.\n\nGuard-1\t-10\tIncreases damage taken, knockback and stamina consumption while blocking.\n\nGuard-2\t-15\tGreatly Increases damage taken, knockback and stamina consumption while blocking.\n\nAuto-Guard\t×\tAuto-Guard\t10\tAutomatically blocks any incoming attacks which can be blocked while a weapon is unsheathed.\n\nZZ Changes: No longer activates Reflect,Obscurity or Rush. Does not fill lance guard gauge.\n\nFortification\t×\tFortification+2\t15\tHybrid skill containing Guard+2, Peerless and Weapon Handling.\n\nFortification+1\t10\tHybrid skill containing Guard+1, Marathon Runner and Weapon Handling.";

    public static string GetGameArmorSkillsBlademasterSkills => "Blademaster Skills\n\nSharpening\t○\tSharpening Artisan\t20\tSharpen at 4x original speed, gain 30 seconds of infinite sharpness and Sharpness+1 after sharpening\n\nSpeed Sharpening\t10\tSharpen with a single stroke of a whetstone.\n\nSlothful Sharpening\t-10\tSharpening duration is doubled.\n\nSharpness\t○\tRazor Sharp+2\t20\tSharpness loss is halved where applicable\n\n50% chance of any sharpness loss being completely negated.\n\nRazor Sharp+1\t10\tSharpness loss is halved where applicable\n\nBlunt Edge\t-10\tSharpness loss is doubled.\n\nStylish\t×\tStylish\t15\tRecover some sharpness when evading through attacks.Amount recovered varies per weapon type.\n\nDual Swords: 12Units.\n\nGunlance: 10 Units.\n\nSword and Shield: 8 Units.\n\nHunting Horn / Switch Axe F / Great Sword / Lance: 5 Units.\n\nLong Sword: 5 Units (Blink: 7 Units).\n\nHammer: 3 Units.\n\nTonfa: 4 Units\n\nTonfa Jump: 3 Units\n\nTonfa EX,Emergency: 10 Units\n\nTonfa DashKick: 6 Units\n\nMagnet Spike: 10 Units (Magnetism Evade: 9)\n\nEdgemaster\t×\tHoned Blade+3\t20\tAttack Up (Absolute) and Sharpness+1\n\nHaving a stronger Attack Up or Strong Attack Up skill activated replaces the Attack component of this skill.\n\nHoned Blade+2\t15\tAttack Up (Very Large) and Sharpness+1\n\nHaving a stronger Attack Up or Strong Attack Up skill activated replaces the Attack component of this skill.\n\nHoned Blade+1\t10\tAttack Up (Large) and Sharpness+1\n\nHaving a stronger Attack Up or Strong Attack Up skill activated replaces the Attack component of this skill.\n\nArtisan\t×\tSharpness+1\t10\tMaximum weapon sharpness increased by +1 level if possible.\n\nBomb Sword\t○\tBomb Sword+3\t20\tWhen the bomb sword crystal is loaded, the effect of the bomb sword [strong] is activated. * Consumes 3 times sharpness\n\nBomb Sword+2\t15\tWhen the bomb sword crystal is loaded, the effect of the bomb sword [medium] is activated. * Consumes 3 times sharpness\n\nBomb Sword+1\t10\tWhen the bomb sword crystal is loaded, the effect of the bomb sword [weak] is activated. * Consumes 3 times sharpness\n\nPoison Sword\t○\tPoison Sword+3\t20\tWhen the poison sword crystal is loaded, the effect of poison sword [strong] is activated. *Consumes double sharpness\n\nPoison Sword+2\t15\tWhen the poison sword crystal is loaded, the effect of poison sword [medium] is activated. *Consumes double sharpness\n\nPoison Sword+1\t10\tWhen the poison sword crystal is loaded, the effect of poison sword [weak] is activated. *Consumes double sharpness\n\nPara Sword\t○\tPara Sword+3\t20\tParalysis sword [strong] effect is activated when the paralysis sword crystal is loaded. *Consumes double sharpness\n\nPara Sword+2\t15\tParalysis sword [medium] effect is activated when the paralysis sword crystal is loaded. *Consumes double sharpness\n\nPara Sword+1\t10\tParalysis sword [weak] effect is activated when the paralysis sword crystal is loaded. *Consumes double sharpness\n\nSleep Sword\t○\tSleep Sword+3\t20\tWhen the sleep sword crystal is loaded, the effect of sleep sword [strong] is activated. *Consumes double sharpness\n\nSleep Sword+2\t15\tWhen the sleep sword crystal is loaded, the effect of sleep sword [medium] is activated. *Consumes double sharpness\n\nSleep Sword+1\t10\tWhen the sleep sword crystal is loaded, the effect of sleep sword [weak] is activated. *Consumes double sharpness\n\nFire Sword\t○\tFire Sword+3\t20\tWhen the flame sword crystal is loaded, the effect of the flame sword [strong] is activated. *Consumes double sharpness\n\nFire Sword+2\t15\tWhen the flame sword crystal is loaded, the effect of the flame sword [medium] is activated. *Consumes double sharpness\n\nFire Sword+1\t10\tWhen the flame sword crystal is loaded, the effect of the flame sword [weak] is activated. *Consumes double sharpness\n\nWater Sword\t○\tWater Sword+3\t20\tWhen a Water Sword Crystal is loaded, the effect of Water Sword [strong] is activated. *Consumes double sharpness\n\nWater Sword+2\t15\tWhen a Water Sword Crystal is loaded, the effect of Water Sword [medium] is activated. *Consumes double sharpness\n\nWater Sword+1\t10\tWhen a Water Sword Crystal is loaded, the effect of Water Sword [weak] is activated. *Consumes double sharpness\n\nThunder Sword\t○\tThunder Sword+3\t20\tWhen a Thunder Sword Crystal is loaded, the effect of Thunder Sword [strong] is activated. *Consumes double sharpness\n\nThunder Sword+2\t15\tWhen a Thunder Sword Crystal is loaded, the effect of Thunder Sword [medium] is activated. *Consumes double sharpness\n\nThunder Sword+1\t10\tWhen a Thunder Sword Crystal is loaded, the effect of Thunder Sword [weak] is activated. *Consumes double sharpness\n\nIce Sword\t○\tIce Sword+3\t20\tWhen an ice sword crystal is loaded, the effect of the ice sword [strong] is activated. *Consumes double sharpness\n\nIce Sword+2\t15\tWhen an ice sword crystal is loaded, the effect of the ice sword [medium] is activated. *Consumes double sharpness\n\nIce Sword+1\t10\tWhen an ice sword crystal is loaded, the effect of the ice sword [weak] is activated. *Consumes double sharpness\n\nDragon Sword\t○\tDragon Sword+3\t20\the effect of Dragon Sword [strong] is activated when a Dragon Sword Crystal is loaded. *Consumes double sharpness\n\nDragon Sword+2\t15\the effect of Dragon Sword [medium] is activated when a Dragon Sword Crystal is loaded. *Consumes double sharpness\n\nDragon Sword+1\t10\the effect of Dragon Sword [weak] is activated when a Dragon Sword Crystal is loaded. *Consumes double sharpness\n\nSnS Tech\t×\tSnS Tech【Master】\tBoth Hiden\tAdds sharpness level +1 to SnS Tech [Sword Saint]\n\nSword Saint\t30\tSuper High-Grade Earplugs\n\nAttack x1.3 when wielding a One-handed Sword.\n\nAll Elemental Sword Stone Skills+3,All Status Sword Stone Skills+2\n\nBomb Sword+2, Faster Movement with SnS unsheathed and Fencing.\n\nSnS Tech (Kaiden)\t20\tFencing and Attack 1.1x while wielding a Sword and Shield.\n\nSnS Tech (Expert)\t10\tFencing while wielding a Sword and Shield.\n\nSnS Tech (Novice)\t-10\tAttack 0.8x while wielding a Sword and Shield.\n\nDS Tech\t×\tDS Tech【Master】\tBoth Hiden\tSharpness level +1 is added to DS Tech [Dual Dragon]\n\nDual Dragon\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding Dual Swords\n\nFencing,Recover 3 units of Stamina per hit while attacking in either Demon Mode\n\nFaster activation of Demon Modes.\n\nDS Tech (Kaiden)\t20\tFencing and Attack 1.1x while wielding Dual Swords.\n\nDS Tech (Expert)\t10\tFencing while wielding Dual Swords.\n\nDS Tech (Novice)\t-10\tAttack 0.8x while wielding Dual Swords.\n\nGS Tech\t×\tGS Tech【Master】\tBoth Hiden\tSharpness level +1 is added to GS Tech [Sword King]\n\nSword King\t30\tSuper HG Earplugs,Attack 1.2x when wielding a Great Sword\n\nFencing+1.Guard slash counterblock recovers half of the sharpness that would be lost\n\nfaster charging (stacks with Focus),charge remains at level 4 longer before dropping to level 2 charge(Storm Style)\n\nPerfectly timed blocks cause no knockback, can be evaded out of\n\ndrain no stamina and cause you to recover half of the sharpness that would be lost.\n\nGS Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Great Sword.\n\nGS Tech (Expert)\t10\tFencing while wielding Great Swords.\n\nGS Tech (Novice)\t-10\tAttack 0.8x while wielding Great Swords.\n\nLS Tech\t×\tLS Tech【Master】\tBoth Hiden\tSharpness level +1 is added to LS Tech [Katana God]\n\nKatana God\t30\tSuper High-Grade Earplugs,Attack x1.2when wielding a Long Sword\n\nFencing, Spirit Bar Consumption Halved, Sharp Sword+2 while Spirit Bar is full\n\nand Attack x1.25(originally x1.15)while the Spirit Bar is glowing (after being filled completely).\n\nLS Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Long Sword.\n\nLS Tech (Expert)\t10\tFencing when wielding a Long Sword.\n\nLS Tech (Novice)\t-10\tAttack x0.8 when wielding a Long Sword.\n\nHammer Tech\t×\tHammer Tech【Master B.Beast】\tBoth Hiden\t+1 sharpness level is added to Hammer Tech [Blunt Beast]\n\nBlunt Beast\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Hammer\n\nFencing and Attack x1.3 when releasing a perfectly timed charge attack for that entire combo\n\n(includes infinite).\n\nHammer Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Hammer.\n\nHammer Tech (Expert)\t10\tFencing when wielding a Hammer.\n\nHammer Tech (Novice)\t-10\tAttack x0.8 when wielding a Hammer.\n\nHH Tech\t×\tHH Tech【Master】\tBoth Hiden\tSharpness level +1 is added to HH Tech [Flamboyant Emporer]\n\nFlamboyant Emperor\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Hunting Horn\n\nFencing,Performance mode Note Color decision is made 34% faster.\n\nHH Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Hunting Horn.\n\nHH Tech (Expert)\t10\tFencing when wielding a Hunting Horn.\n\nHH Tech (Novice)\t-10\tAttack x0.8 when wielding a Hunting Horn.\n\nLance Tech\t×\tLance Tech【Master】\tBoth Hiden\tSharpness level +1 is added to Lance Tech [Heavenly Spear]\n\nHeavenly Spear\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Lance,Fencing\n\n0 Damage when blocking against all attacks,Perform 4 hops instead of 3\n\nMotion value +10 for final (3rd or 4th) Normal,Diagonal and Sky-Stabs.\n\nLance Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Lance.\n\nLance Tech (Expert)\t10\tFencing when wielding a Lance.\n\nLance Tech (Novice)\t-10\tAttack x0.8 when wielding a Lance.\n\nGL Tech\t×\tGL Tech【Master】\tBoth Hiden\tSharpness level +1 is added to GL Tech [Cannon Emperor]\n\nCannon Emperor\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Gunlance, Fencing\n\nWyvern Fire and Heat Blade Cooldown-time is halved,Heat Blade Activation Time reduced to 3 seconds\n\nNormal Shells +2 Long Shells +1and Wide Shells +1, stackable with Load Up\n\nGL Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Gunlance.\n\nGL Tech (Expert)\t10\tFencing when wielding a Gunlance.\n\nGL Tech (Novice)\t-10\tAttack x0.8 when wielding a Gunlance.\n\nTonfa Tech\t×\tTonfa Tech【Master】\tBoth Hiden\tSharpness level +1 is added to Tonfa Tech [Piercing Phoenix]\n\nPiercing Phoenix\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding Tonfa,Fencing\n\nan additional 6th Combo / EX Meter segment is added.\n\nTonfa Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding Tonfa.\n\nTonfa Tech (Expert)\t10\tFencing when wielding Tonfa.\n\nTonfa Tech (Novice)\t-10\tAttack x0.8 when wielding Tonfa.\n\nSwitch Axe Tech\t×\tSwitch Axe Tech【Master】\tBoth Hiden\tSharpness level +1 is added to Switch Axe Tech [Edge Marshal]\n\nEdge Marshal\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Switch Axe and Fencing.\n\nSuccessfully using certain actions successfully increases attack by 1.05x for a short time (Morphing in Earth Style, Sword Attacks in Heaven Style and Guarding in Storm Style.)\n\nStamina consumption for infinite swing combo is halved, attacking in axe mode increases phial gauge\n\nattacks utilising the phial consume less meter.\n\nSwitch Axe Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Switch Axe.\n\nSwitch Axe Tech (Expert)\t10\tFencing when wielding a Switch Axe.\n\nSwitch Axe Tech (Novice)\t-10\tAttack x0.8 when wielding a Switch Axe.\n\nMagnet Spike Tech\t×\tMagnet Spike Tech【Master】\tBoth Hiden\tSharpness level +1 is added to Magnet Spike Tech [Magnetic Star]\n\nMagnetic Star\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Magnet Spike, Fencing\n\nSuccessful evasive motions boost attack (1.03x) and gauge recovery (1.5x)for 30seconds.\n\nMovement speed up (1.2x). Magnetic target marker doesn't disappear.\n\nMagnet Spike Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Magnet Spike.\n\nMagnet Spike Tech (Expert)\t10\tFencing when wielding a Magnet Spike.\n\nMagnet Spike Tech (Novice)\t-10\tAttack x0.8 when wielding a Magnet Spike.\n\nFencing\t○\tFencing+2\t20\tGrants ESP (no bouncing) as well as a 2nd hit for each attack.The 2nd hit has has 20% the value of the first\n\nthis includes Raw, Element, Status and Sword Crystals.\n\nEven if you hit twice Sharpness and Sword Crystals will only be decreased as if you had hit once.\n\nFencing+1\t10\tGrants ESP (no bouncing) when attacking.\n\nSword God\t×\tSword God+3\t20\tSharpness+1, Razor Sharp+2, Fencing+2 and Sharpening Artisan in one skill.\n\nSword God+2\t10\tSharpness+1,Razor Sharp+2, Fencing+2in one skill.\n\nSword God+1\t10\tSharpness+1,Razor Sharp+1, Fencing+1in one skill.";

    public static string GetGameArmorSkillsGunnerSkills => "Gunner Skills\n\nSteady Hand\t×\tSteady Hand+2\t20\tNormal/Rapid Up,Pierce/Pierce Up,Pellet/Spread Up and +5 to weakness value with critical distance.\n\nDoes not stack with the individual up skills, determination, precision, sniper or critical shot.\n\nSteady Hand+1\t10\tNormal/Rapid Up,Pierce/Pierce Up and Pellet/Spread Up. Does not stack with the individual skill versions.\n\nGentle Shot\t×\tGentle Shot+3\t30\tLoad Up & Recoil Reduction+3.\n\nGentle Shot+2\t15\tLoad Up & Recoil Reduction+2.\n\nGentle Shot+1\t10\tLoad Up & Recoil Reduction+1.\n\nNormal Shot Up\t×\tNormal/Rapid Up\t10\tNormal S and Rapid Bow Arrows do x1.1damage.\n\nPierce Shot Up\t×\tPierce/Pierce Up\t10\tPierce Shots and Pierce Bow Arrows do x1.1damage.\n\nPellet Shot Up\t×\tPellet/Spread Up\t10\tPellet Shots and Scatter Bow Arrows do x1.3damage.\n\nNormal Shot Add\t×\tNormal S All\t15\tGrants the ability to use all Normal S ammo.\n\nNormal S Lv1\t10\tLv1 Normal bullets can be used\n\nPierce Shot Add\t×\tPierce S All\t20\tGrants the ability to use all Pierce S ammo.\n\nPierce S Lv1&2 Add\t15\tPierce S Lv1&2 bullets can be used\n\nPierce S Lv1 Add\t10\tPierce S Lv1 bullets can be used\n\nPellet Shot Add\t×\tPierce S All\t20\tGrants the ability to use all Pellet S ammo.\n\nPellet S LV1&2 Add\t15\tPellet S Lv1&2 bullets can be used\n\nPellet S LV1 Add\t10\tPellet S Lv1 bullets can be used\n\nCrag Shot Add\t×\tCrag S All\t20\tGrants the ability to use all Crag S ammo.\n\nCrag S Lv1&Lv2 Add\t15\tCrag S Lv1&2 bullets can be used\n\nCrag S Lv1 Add\t10\tCrag S Lv1 bullets can be used\n\nCluster Shot Add\t×\tCluster S All\t20\tGrants the ability to use all Cluster S ammo.\n\nCluster S Lv1&Lv2 Add\t15\tCluster S Lv1&2 bullets can be used\n\nCluster S Lv1 Add\t10\tCluster S Lv1 bullets can be used\n\nLoading\t×\tLoadingＵＰ\t10\t+1 capacity for Bowgun and Gunlance ammo.Extra charge level for Bows.\n\nReload\t○\tReload Speed+3\t20\tBowguns: Reload Speed increases by 3 levels.\n\nBows: Coatings are automatically loaded when they are selected.\n\nReload Speed+2\t15\tBowguns: Reload Speed increases by 2 levels.\n\nBows: Coating loading time is 75% of default time.\n\nReload Speed+1\t10\tBowguns: Reload Speed increases by 1 level.\n\nBows: Coating loading time is 85% of default time.\n\nReload Speed-1\t-10\tBowguns: Reload Speed decreases by 1 level.\n\nBows: Coating loading time is 110% of default time.\n\nRecoil\t○\tRecoil Reduction+3\t30\tBowguns: Recoil is reduced by 6 levels.\n\nGunlance: Can evade after Shelling and Recoil from Wyvern Fire is reduced by 30 frames.\n\nRecoil Reduction+2\t15\tBowguns: Recoil is reduced by 4 levels.\n\nGunlance: Can evade after Shelling and Recoil from Wyvern Fire is reduced by 30 frames.\n\nRecoil Reduction+1\t10\tBowguns: Recoil is reduced by 2 levels.\n\nGunlance: Can evade after Shelling.\n\nCritical Shot\t×\tCritical Shot+3\t20\tAttack Up (Absolute)(True Raw+50) and Sniper (Less shot deviation, +5 to a body part's weakness when hit by a Normal/Pierce/Crag Shot or Bow Attacks at a properly spaced critical distance).\n\nDoes not stack with other Attack skills or Sniper (e.g.Attack Up portion would be overwritten by Strong Attack+4).\n\nStacks with Thunderclad,Acid Shots,HHSonicBomb Debuff and Point Breakthrough with the resulting value being used for Exploit Weakness.\n\nCritical Shot+2\t15\tAttack Up (Very Large)(True Raw+30) and Sniper (Less shot deviation, +5 to a body part's weakness when hit by a Normal/Pierce/Crag Shot or Bow Attacks at a properly spaced critical distance).\n\nDoes not stack with other Attack skills or Sniper (e.g.Attack Up portion would be overwritten by Strong Attack+4).\n\nStacks with Thunderclad,Acid Shots,HHSonicBomb Debuff and Point Breakthrough with the resulting value being used for Exploit Weakness.\n\nCritical Shot+1\t10\tAttack Up (Large)(True Raw+20) and Sniper (Less shot deviation, +5 to a body part's weakness when hit by a Normal/Pierce/Crag Shot or Bow Attacks at a properly spaced critical distance).\n\nDoes not stack with other Attack skills or Sniper (e.g.Attack Up portion would be overwritten by Strong Attack+4).\n\nStacks with Thunderclad,Acid Shots,HHSonicBomb Debuff and Point Breakthrough with the resulting value being used for Exploit Weakness.\n\nRapid Fire\t×\tRapid Fire\t15\tRapid Fire volleys fire one extra round, when loading bullets that can be rapid fired you will load all bullets.(Doesn't apply to Ultra Rapid Fire).\n\nAuto-Reload\t×\tAuto-Reload\t10\tBowguns: Can shoot without having to reload but receive high recoil for all shots.\n\nBows: Charging time is reduced to 85% of the default speed.\n\nAmmo Combiner\t×\tMaximum Bullets\t10\tWhen combining bullets or coatings you will always get the maximum possible back.\n\nBullet Saver\t×\tSaving Expert\t20\tWhen you fire a shot there is a 11/32(34.3%) chance that it will notconsume a shot or coating\n\nProcesses all bullets individually on compression shots (HBG only, any number from zero to all loaded shots can be saved.)\n\nbut does not work with Ultra Rapid Fire (LBG) or Heat Beams (HBG).\n\nSaving Master\t10\tWhen you fire a shot there is a 7/32(21.8%) chance that it will not consume a shot or coating\n\nAlso works with compression (HBG). Does not work with Ultra Rapid Fire (LBG) and Heat Beam (HBG).\n\nPrecision\t○\tPrecision+2\t20\tShot deviation (bullet drift) is decreased. and adds +5 to a bodypart's weakness (i.e. 35>40)\n\nwhen shooting with Normal/Pierce/Crag Shot or Bow Attacks within Critical Distance.\n\nDeviation Down\t10\tShot deviation (bullet drift) is decreased.\n\nDeviation Up\t-10\tShot deviation (bullet drift) is increased.\n\nPoison Coating Add\t×\tPoison Coating Add\t10\tGrants the ability to use Poison Coatings\n\nPara Coating Add\t×\tPara Coating Add\t10\tGrants the ability to use Para Coatings\n\nSleep Coating Add\t×\tSleep Coating Add\t10\tGrants the ability to use Sleep Coatings\n\nMounting\t×\tMounting+3\t30\tLoad Up & Reload Speed+3.\n\nMounting+2\t20\tLoad Up & Reload Speed+2.\n\nMounting+1\t10\tLoad Up & Reload Speed+1.\n\nSniper\t×\tSniper\t10\tAuto-Reload and +5 to weakness value within critical distance. *Should only be used with Bows.*\n\nSpacing\t×\tSpacing\t10\tAlters critical distance for Normal,Pierce,Pellet and Blunt shots to be lower and further increases damage at critical distance.\n\nIf you consistently attack within this critical distance you gain Movement Speed+2 and Evade Distance Up.\n\nLBG and HBG get increased perfect shot(3 Segments) and compression windows (2 segments).Bowgets lowered charge times that stack with Auto-Reload.\n\nDoes not buff attacks without Critical Distance, is added to the buff for the first half of HBG's Critical Distance.\n\nBuff builds an public meter that fills in much the same way as Thunder Clad and total hits needed will vary with Attack, Affinity, Shot Type, etc. but meter build up is not reset on being launched etc.\n\nHBG Tech\t×\tHBG Tech【Master】\tBoth Hiden\tHBG Tech [Gun Sage]'s attack power increases by 1.4x\n\nGun Sage\t30\tSuper HG Earplugs, Attack x1.3 when wielding a Heavy Bowgun, Power (value) of Element/Status Ammo x1.2,Evade Distance Up\n\nMelee Attacks and Crag/Clust Shots all do 15 KO Damage,Heat Beam does x1.2damage and Perfectly-Timed Compression will result in more Attack Power for that 1 salvo.\n\nHBG Tech (Kaiden)\t20\tAttack x1.1 and Evade Extender when wielding a Heavy Bowgun.\n\nHBG Tech (Expert)\t10\tEvade Extender when wielding a Heavy Bowgun.\n\nHBG Tech (Novice)\t-10\tAttack power x0.8 when wielding a Heavy Bowgun.\n\nLBG Tech\t×\tLBG Tech【Master】\tBoth Hiden\tIncreases attack power of LBG Tech [Gun Prodigy] by 1.4x\n\nGun Prodigy\t30\tSuper HG Earplugs,Attack 1.3 when wielding a Light Bowgun, Probability of Ammo bouncing off Monsters is reduced\n\ncan consume items while the weapon is unsheathed,Perfect Shot added to Just Shot meter that deals additional damage.\n\nLBG Tech (Kaiden)\t20\tAttack x1.1 and the probability of Ammo bouncing off Monsters is reduced when wielding a Light Bowgun.\n\nLBG Tech (Expert)\t10\tThe probability of Ammo bouncing off Monsters is reduced when wielding a Light Bowgun.\n\nLBG Tech (Novice)\t-10\tAttack 0.8x while wielding a Light Bowgun.\n\nBow Tech\t×\tBow Tech【Master】\tBoth Hiden\tBow Tech[Bow Demon]'s attack power increases by 1.4x\n\nBow Demon\t30\tSuper HG Earplugs,Attack x1.3 when wielding a Bow, Arrows cannot be deflected, PowerCoating Modifier increases to x1.6 for normal Bows and to x1.7for Gou (and upgrades)\n\nEvolution (Raviente) and G-Rank Bows and the Arc-Shot can be executed at Charge Lv2.\n\nBow Tech (Kaiden)\t20\tAttack x1.1 and arrows are no longer deflected when wielding a Bow.\n\nBow Tech (Expert)\t10\tArrows are no longer deflected when wielding a Bow.\n\nBow Tech (Novice)\t-10\tAttack 0.8x while wielding a Bow.";

    public static string GetGameArmorSkillsResistanceSkills => "Resistance Skills\n\nAll Res Up\t×\tAll Res+20\t20\tAll resistance values increase by 20\n\nAll Res Up+10\t15\tAll resistance values increase by 10\n\nAll Res Up+5\t10\tAll resistance values increase by 5\n\nAll Res-5\t-10\tAll resistance values decrease by 5\n\nAll Res-10\t-15\tAll resistance values decrease by 10\n\nAll Res-20\t-20\tAll resistance values decrease by 20\n\nFire Res\t○\tFire Res+30\t20\tFire res values increase by 30\n\nFire Res+20\t15\tFire res values increase by 20\n\nFire Res+10\t10\tFire res values increase by 10\n\nFire Res-10\t-10\tFire res values decrease by 10\n\nFire Res-20\t-15\tFire res values decrease by 20\n\nFire Res-30\t-20\tFire res values decrease by 30\n\nWater Res\t○\tWater Res+30\t20\tWater res values increase by 30\n\nWater Res+20\t15\tWater res values increase by 20\n\nWater Res+10\t10\tWater res values increase by 10\n\nWater Res-10\t-10\tWater res values decrease by 10\n\nWater Res-20\t-15\tWater res values decrease by 20\n\nWater Res-30\t-20\tWater res values decrease by 30\n\nIce Res\t○\tIce Res+30\t20\tIce res values increase by 30\n\nIce Res+20\t15\tIce res values increase by 20\n\nIce Res+10\t10\tIce res values increase by 10\n\nIce Res-10\t-10\tIce res values decrease by 10\n\nIce Res-20\t-15\tIce res values decrease by 20\n\nIce Res-30\t-20\tIce res values decrease by 30\n\nThunder Res\t○\tThunder Res+30\t20\tThunder res values increase by 30\n\nThunder Res+20\t15\tThunder res values increase by 20\n\nThunder Res+10\t10\tThunder res values increase by 10\n\nThunder Res-10\t-10\tThunder res values decrease by 10\n\nThunder Res-20\t-15\tThunder res values decrease by 20\n\nThunder Res-30\t-20\tThunder res values decrease by 30\n\nDragon Res\t○\tDragon Res+30\t20\tDragon res values increase by 30\n\nDragon Res+20\t15\tDragon res values increase by 20\n\nDragon Res+10\t10\tDragon res values increase by 10\n\nDragon Res-10\t-10\tDragon res values decrease by 10\n\nDragon Res-20\t-15\tDragon res values decrease by 20\n\nDragon Res-30\t-20\tDragon res values decrease by 30";

    public static string GetGameArmorSkillsAbnormalStatusSkills => "Abnormal Status Skills\n\nPoison\t○\tNegate Poison\t20\tImmunity to Poison.\n\nPoison Halved\t10\tPoison duration is halved.\n\nDouble Poison\t-10\tPoison duration is doubled.\n\nPara\t○\tNegate Para\t20\tImmunity to Paralysis\n\nPara Halved\t10\tParalysis duration is halved.\n\nPara Doubled\t-10\tParalysis duration is doubled.\n\nSleep\t○\tNegate Sleep\t20\tImmunity to Sleep.\n\nSleep Halved\t10\tSleep duration is halved.\n\nSleep Doubled\t-10\tSleep duration is doubled.\n\nStatus Res\t○\tS. Immunity (Myriad)\t30\tGrants immunity to Poison, Paralysis, Sleep, Stench, Snowman, Chat Disabled, Defense Down, Drunk, Magnetism, Crystallization and Blast.\n\nStatus Immunity\t20\tGrants immunity to Poison,Paralysis and Sleep.\n\nStatus Halved\t10\tHalves Poison,Paralysis and Sleep duration.\n\nStatus Doubled\t-10\tDoubles Poison,Paralysis and Sleep duration.\n\nStun\t○\tNegate Stun\t20\tImmunity to Stun.\n\nStun Halved\t10\tStun duration is halved.\n\nStun Doubled\t-10\tStun duration is doubled.\n\nDeoderant\t×\tDeoderant\t10\tPrevents Stench\n\nSnowball Res\t×\tSnowball Res\t10\tGrants immunity to Snowman.\n\nVocal Chords\t○\tVocal Chord Immunity\t15\tGrants immunity to Chat Disable and Fatigue. Exclusive to Chameleos.\n\nVocal Chord Halved\t10\tHalves chat Disable and Fatigue duration.\n\nDef Lock\t×\tDef Lock\t10\tGrants immunity to Defense Down\n\nSobriety\t×\tHeavy Drinker\t10\tGrants immunity to Drunk status\n\nDrunkard\t-10\tDoubles Drunk duration\n\nBlast Resistance\t×\tBlast Resistance\t10\tGrants immunity to Blast blight.\n\nMagnetic Res\t×\tMagnetic Res\t10\tGrants immunity to Magnetism\n\nMagnet Vulnerability\t-10\tDoubles Magnetism duration\n\nCrystal Res\t×\tCrystal Res\t10\tGrants immunity to Crystallization\n\nCrystal Vulnerability\t-10\tIt takes longer to recover from crystal state.\n\nAlso, crystals are more likely to explode\n\nFreeze Res\t×\tFreeze Resistance\t10\tGrants immunity to Freezing (Toa Tesukatora.)";

    public static string GetGameArmorSkillsOtherProtectionSkills => "Other Protection Skills\n\nThree Worlds\t×\tUnaffected+3\t20\tSuper High-Grade Earplugs, Violent Wind Breaker and Quake Resistance+2.\n\nUnaffected+2\t15\tHigh-Grade Earplugs,Dragon Wind Breaker and Quake Resistance+1.\n\nUnaffected+1\t10\tEarplugs,Wind Resistance (Large) and Quake Resistance+1.\n\nHearing Res\t○\tSuper HG Earplugs\t25\tProtects against all Monster roars\n\nHigh-Grade Earplugs\t15\tProtects against second tier monster roars\n\nEarplugs\t10\tProtects against first tier monster roars\n\nLowers the length of the flinching duration for second tier roars that aren't blocked.\n\nWind Pressure\t○\tViolent Wind Breaker\t30\tProtects against Violent Wind\n\nDragon Wind Breaker\t20\tProtects against Dragon Wind\n\nWind Res (Large)\t15\tProtects against Large Wind\n\nWind Res (Small)\t10\tProtects against Small Wind\n\nQuake Res\t○\tQuake Res+2\t25\tProtects against large quakes\n\nQuake Res+1\t15\tProtects against small quakes\n\nEvasion\t○\t\n\nInvincible time is usually 6/30 seconds\n\nEvasion+2\t20\tEvasion invulnerability is increased to 12/30 seconds\n\nEvasion+1\t10\tEvasion invulnerability is increased to 10/30 seconds\n\nEvade Distance\t×\tEvade Distance Up\t20\tExtends movement distance for evasion and steps.\n\nEvasion Boost\t×\tEvasion Boost\t15\tGrants the effects of Evasion+2 and Evade Distance Up.\n\nHeat Res\t○\t\n\nNormally, the health gauge will decrease every 2 seconds in the volcano and every 3 seconds in the desert.\n\nSummer Person\t25\tHeat Cancel and Damage Recovery Speed+1 in Hot areas.\n\nHeat Cancel\t15\tGrants immunity to heat(Health loss in Hot areas).\n\nHeat Halved\t10\tThe speed at which Health is lost in Hot areas is halved.\n\nHeat Surge (Small)\t-10\tHealth loss is increased to 1 unit per 1.3 seconds in the Volcano and 1 unit per 2 seconds in the Desert\n\nHeat Surge (Large)\t-20\tHealth loss is increased to 1 unit per 1 second in the Volcano and 1 unit per 1.5 seconds in the Desert\n\nCold Res\t○\t\n\nStamina usually decreases every 6 minutes\n\nWinter General\t25\tCold Cancel, Frost bite Immunity and Marathon Runner effect while in cold areas.\n\nCold Cancel\t15\tGrants immunity to cold (faster stamina loss)\n\nCold Halved\t10\tThe speed at which stamina is lost(hunger)in Cold areas is halved.\n\nCold Surge (Small)\t-10\tThe speed at which stamina is lost in Cold areas is increased x1.5\n\nCold Surge (Large)\t-20\tThe speed at which stamina is lost in Cold areas is increased x2\n\nLight Tread\t×\tLight Tread\t10\tImmunity to monster's Pitfall Traps.\n\nAnti-Theft\t×\tAnti-Theft\t10\tPrevents items being stolen.\n\nTerrain\t○\t\n\nNormally you take a certain amount of damage every 8/30 seconds\n\nHazard Res (Large)\t15\tHealth Loss from Terrain Damage (Lava) and Heat Auras is reduced to 1/3speed.\n\nHazard Res (Small)\t10\tHealth Loss from Terrain Damage (Lava) and Heat Auras is reduced to 2/3speed.\n\nHazard Prone (Small)\t-10\tHealth Loss from Terrain Damage (Lava) and Heat Auras is increased to x1.5 speed.\n\nHazard Prone (Large)\t-15\tHealth Loss from Terrain Damage (Lava) and Heat Auras is increased to x2 speed.\n\nProtection\t○\tGoddess' Embrace\t20\t1/4 chance to receive 0 damage from any attack.\n\nDivine Protection\t10\t1/8 chance to receive 0 damage from any attack.\n\nDemonic Protection\t-10\t1/16 chance to die instantly from any attack.\n\nDeath God's Embrace\t-20\t1/8 chance to die instantly from any attack.\n\nPassive\t×\tPassive\t10\tInvincibility time after getting up from an attack is lengthened 3x.\n\nBreakout\t×\tBreakout\t10\tPanic run speed is increased when below 20% health.\n\nGuts\t○\tTrue Guts\t30\tWhen Health and Stamina are 50 or higher, an otherwise lethal attack will be survived with 1 Health Point left.\n\nGreat Guts\t20\tWhen Health is 70 or higher and Stamina is 50 or higher, an otherwise lethal attack will be survived with 1 Health Point left.\n\nGuts\t10\tWhen Health is 90 or higher and Stamina is 50 or higher, an otherwise lethal attack will be survived with 1 Health Point left.\n\nAbsolute Defense\t×\tAbsolute Defense\t20\tAdds a shield aura that completely negates hits, including all damage and status effects that the hit would cause.\n\nShield vanishes for a set duration after each hit blocked, with the duration before recharging taking longer with each hit taken.\n\nReduces physical damage to around 0.8x while the shield is down";

    public static string GetGameArmorSkillsItemComboSkills => "Item/Combo Skills\n\nCombo Expert\t×\tCombo Expert+3\t20\tCombo Rate +30% and the effects of Bullet Combination Expert and Health Recovery Items Improved.\n\nCombo Expert+2\t15\tCombo Rate +20% and the effect of Bullet Combination Expert\n\nCombo Expert+1\t10\tCombo Rate +10% and the effect of Bullet Combination Expert\n\nCombo Expert-1\t-10\tCombo Rate -5%\n\nWide-Area\t○\t\n\nEffective when using herbs, healing potions, antidotes, power seeds, armor seeds, flinch-free fruit\n\nWide-Area+3\t30\tMega Potions, Blight Cure Fruits, Zenith Espinas Antitoxin and Crimson Raviente Blood affect allies in the same area\n\nas well as the items covered in Wide-Area+2.\n\nWide-Area+2\t20\tHerbs, Potions, Antidotes, Cool Drinks, Hot Drinks, Armor Seed and Power Seed affects allies in the same area.\n\nWide-Area+1\t10\tHerbs, Potions, Antidotes, Cool Drinks, Hot Drinks, Armor Seed and Power Seed affects allies in the same area\n\nwith 50% effectiveness.\n\nWide-Area-1\t-10\tThe Player cannot be healed through Wide-Range Recovery\n\nEverlasting\t×\tItem Duration Up\t10\tItem effect duration is increased to x1.5\n\nItem Duration DOWN\t-10\tItem effect duration is decreased x0.67.\n\nWhim\t○\t\n\nProbability of item breaking\n\nOld Pickaxe：1/3\u3000Iron Pickaxe：1/10\u3000Mega Pickaxe：1/15\u3000Pickaxe G：1/17\n\nOld Bugnet\u3000：1/3\u3000Bugnet\u3000：1/10\u3000Mega Bugnet\u3000：1/15\u3000Bugnet G：1/17\n\nHealth Flute：1/12\u3000Antidote Flute：1/12\u3000Demon Flute：1/ 8\u3000Armour Flute：1/8\u3000Flute：1/5\n\nShot Flute：1/16\u3000Assault Flute：1/16\u3000Tail Flute：1/12\u3000Deadly Flute：1/8\n\nSpecial Flute：1/16\u3000Rage Flute：1/16\u3000Wrath Flute：1/ 8\n\nDivine Whim\t15\tDecreases chance of breaking Pickaxes,Bug Nets,Horns, and Boomerangs by 50%.\n\nSpirit's Whim\t10\tDecreases chance of breaking Pickaxes,Bug Nets,Horns, and Boomerangs by 25%.\n\nSpectre's Whim\t-10\tIncreases chance of breaking Pickaxes,Bug Nets,Horns, and Boomerangs by 25%.\n\nDevil's Whim\t-15\tIncreases chance of breaking Pickaxes,Bug Nets,Horns, and Boomerangs by 50%.\n\nHunter\t×\tHunter Valhalla\t20\tAlways shows the map and any large monsters location.Easier to fish and BBQ meat.\n\nHunter Life\t10\tAlways shows the map.Easier to fish and BBQ meat.\n\nStrong Arm\t○\tStrong Arm+2\t20\tThrown items do x1.3damage.\n\nStrong Arm+1\t10\tThrown items do x1.1damage.\n\nThrowing\t×\tThrowing Distance UP\t10\tThrowing Distance of Throwing Items is increased and the probability of losing Boomerangs is decreased to 1/8.\n\nCooking\t○\tBBQ Master\t15\tFish and Meat remain Brown (ready)for much longer.\n\nBBQ Expert\t10\tFish and Meat remain Brown (ready)longer.\n\nFalse BBQ Expert\t-10\tFish and Meat remain Brown (ready)for half the time of the usual.\n\nFishing\t×\tFishing Expert\t10\tFish always bite on their first attempt.\n\nCombining\t○\tCombining+30%\t20\tCombo Rate +30%\n\nCombination+15%\t15\tCombo Rate +15%\n\nCombination+10%\t10\tCombo Rate +10%\n\nCombination-5%\t-10\tCombo Rate -5%\n\nAlchemy\t×\tAlchemy\t10\tGrants Alchemy Combinations (alternative item combinations).\n\nEncourage\t×\tEncourage+2\t20\tHorn Maestro.Party wide Evasion+2 and Stun Negate.\n\nEncourage+1\t10\tHorn Maestro.Party wide Evasion+1 and Stun Halved.\n\nFlute Expert\t×\tFlute Expert\t10\t50% less chance of horns breaking. All horn duration 1.5x (including Hunting Horn songs).\n\nSpeed Setup\t○\tTrap Master\t20\tTrap Setup Speed is lowered to x0.66. 100% combine rate for traps, Mocha Pots and Trap-related items.\n\nTrap Expert\t10\tTrap Setup Speed is lowered to x0.8. 100% combine rate for traps, Mocha Pots and Trap-related items.\n\nIron Arm\t×\tIron Arm+2\t20\tThrowing Distance Up, Strong Arm+2,Throwing Knives+2. Increases blocking duration on Great Sword Guard Slash (Heaven and Storm Styles)\n\nLance Shield Rush (Storm and Extreme Styles), and Tonfa's Standard 1, Standard 3, Standard 4,Aerial 1-3, Continuous Thrust 2 and Dash Tonfa Rotatation.\n\nIron Arm+1\t10\tThrowing Distance Up, Strong Arm+1,Throwing Knives+1. Increases blocking frames duration on Great Sword Guard Slash (Heaven and Storm Styles)\n\nLance Shield Rush (Storm and Extreme Styles), and Tonfa's Standard 1, Standard 3, Standard 4,Aerial 1-3, Continuous Thrust 2 and Dash Tonfa Rotatation.\n\nKnife Throwing\t○\tThrowing Knives+2\t20\tThrow 5K nives at once instead of 1. Only consumes 1.\n\nThrowing Knives+1\t10\tThrow 3 Knives at once instead of 1. Only consumes 1.";

    public static string GetGameArmorSkillsMapDetectionSkills => "Map/Detection Skills\n\nMap\t×\tMap\t10\tAlways display the map.\n\nPsychic\t○\tAutotracker\t15\tMonster locations and detailed icon are always shown on the map.\n\nDetect\t10\tMarked monsters are shown as a detailed icon rather than a round dot on the map.\n\nStealth\t×\tSneak\t10\tMonsters are less likely to target you.\n\nTaunt\t-10\tMonsters are more likely to target you.\n\nIncitement\t×\tIncitement\t10\tAttacking a monsterwill force its attention towards you rather than your fellowHunters.Damage received from the monster will also be reduced during this time.The yellow eye icon indicating you were spotted will turn Red when the skill is in effect.\n\nGoing outside of the monster's range for too long will undo the effect prematurely,the skill has a 30 second cooldown before it can be activated again.\n\nWhile being targetted you gain +40 True Raw.";

    public static string GetGameArmorSkillsGatheringTransportSkills => "Gathering/Transport Skills\n\nBackpacking\t×\tPro Transporter\t10\tIncreases speed when carrying heavy items such as eggs, powderstone, soothstone, etc.\n\nAlso be able to fall slightly higher altitudes without dropping the item.\n\nGathering Speed\t×\tHigh Speed Gathering\t10\tIncreases gathering speed.\n\nGathering\t○\t\n\nNormally, the probability of gathering after the second time is 27/32\n\nGathering+2\t15\tIncreases the chance of collecting and excavating from the second time onwards to 31/32.\n\nGathering+1\t10\tIncreases the chance of collecting and excavating from the second time onwards to 29/32.\n\nGathering-1\t-10\tIncreases the chance of collecting and excavating from the second time onwards to 23/32.\n\nGathering-2\t-15\tIncreases the chance of collecting and excavating from the second time onwards to 19/32.\n\nCarving\t×\tCarving Expert\t15\tNumberof carves increased by 1\n\nMindfulness\t○\tImperturbable\t15\tWhile using an item or gathering, the Hunter can't be interrupted by any attack.\n\nFully Prepared\t10\tWhile using an item or gathering, the Hunter can't be interrupted by other hunters or small monsters.\n\nNegligence\t-10\tDamage taken from monsters is increased when taking a hit while using an item or gathering.";

    public static string GetGameArmorSkillsRewardSkills => "Reward Skills\n\nFate\t○\t\n\nUsually, the probability of increasing the number of slots is 22/32\n\nGreat Luck\t20\tGreatly increases chance for standard quest rewards. 29/32.\n\nGood Luck\t10\tIncreases chance for standard quest rewards 26/32.\n\nBad Luck\t-10\tDecreases chance for standard quest rewards 16/32.\n\nHorrible Luck\t-20\tGreatly Decreases chance for standard quest rewards. 8/32.\n\nMonster\t×\tCome on Big Guy!\t10\tIncreases the chances of larger boss monster spawns.?\n\nPressure\t○\tPressure【Large】\t20\tMonster Bounty randomly increases by 30, 50, 75, 100 or 150%.\n\nPressure (Small)\t10\tMonster Bounty increases by 30%.";

    public static string GetGameArmorSkillsOtherSkills => "Other Skills\n\nBreeder\t×\tBreeder\t10\tPoogie item drops at the Pugi Farm become more common. Halks also drop items more often.\n\nBond\t×\tBond\t10\tWhen another Hunter of the opposite gender is present in a quest:\n\nMale Player: Attack +5.\n\nFemale Player: Defense +40.\n\nInspiration\t×\tInspiration\t10\tVarious effects are triggered at random when starting a quest.\n\nSkill\tMessage\tChance\n\nAll Res＋５\t各耐性に強くなった！\t30%\n\nHeat & Cold Res\t気候の変化に強くなった！\t16%\n\nNegate Stun\t気絶しなくなった！\t16%\n\nTaunt\tﾓﾝｽﾀｰに狙われやすくなった…\t15%\n\nSpeed Eating\t早食いになった！\t12%\n\nAll Res-20\t各耐性に弱くなった…\t\u30007%\n\nStatus Immunity\t状態異常に強くなった！\t\u30004%\n\nRelief\t×\tRelief\t10\tRastas, Fostas, Partners and Halks recover 50% faster after disappearing.\n\nLegendary Rastas do not leave and thus are not affected.\n\nCapture Proficiency\t○\tCapture Guru\t20\tMonsters will blink on the map when they can be captured.\n\nCapture Proficiency\t10\tChance of a captured Monster becoming a pet increases by 30%.\n\nCaring\t×\tCaring+3\t45\tAll Quests: Human Players cannot be interrupted by your attacks nor can they interrupt you.\n\nAll Quests: NPCs cannot be interrupted by Human Players and will roll when hit instead.\n\nCaring+2\t25\tNon-G Rank Quests: Human Players cannot be interrupted by your attacks nor can they interrupt you.\n\nAll Quests: NPCs cannot be interrupted by Human Players and will roll when hitinstead.\n\nCaring+1\t10\tAll Quests: NPCs cannot be interrupted by Human Players and will roll when hit instead.\n\nMovement Speed\t×\tMovement Speed ＵＰ+2\t20\tMovement speed with weapon sheathed is increased slightly further.\n\nMovement Speed ＵＰ+1\t10\tMovement speed with weapon sheathed is increased slightly.\n\nReinforcement\t×\tRed Soul\t10\tOwn Attack +15. Hitting another player will increase their Attack +30.\n\n※Hitting a player who is under the effect of Blue Soul\n\n※When hit by a player who has the Blue Soul skill you will be able to KO monsters with any weapon when hitting their head and your own Attack rises by +30. All effects last 2 minutes.\n\n※Attack is a final addition that is always the stated value and completely unaffected by other skills and multipliers.\n\nBlue Soul\t-10\tOwn Defense +50. Hitting another player will increase their Defense +100.\n\n※Hitting a player who is under the effect of Blue Soul while he/she is under any status ailment in the game will dispel the effect.\n\n※When hit by a player who has the Red Soul skill active will increase own Defense by +100 and grant Goddess' Embrace effect.\n\nAll effects last 2minutes.\n\nAssistance\t×\tAssistance\t10\tThe player's arm will glow red and grants +20 Attack, +50 Defense, Damage Recovery Speed+2, Status Immunity and Peerless to nearby Hunters.\n\nEffective radius is 3 rolls or 2 with Evade Distance Up. Affected players have their arms glow yellow.\n\n※The player with the skill does get +20 Attack and +50 Defense but does not benefit from Peerless, Status Immunity or Damage Recovery Speed.\n\n※The skills overwrite lower tier versions if any affected hunters have them. For example a hunter with Status Halved would gain Status Immunity within radius while a hunter with Status\n\nImmunity (Myriad) would retain their version of the skill.\n\n※Attack is a final addition thatis always the stated value and completely unaffected by other skills and multipliers.\n\nGrace\t×\tGrace+3\t30\tWhen there are only 3 or fewer active skills: Attack Up (Absolute), Issen+3, High-Grade Earplugs, WindRes (Large),Quake Res+1, Evasion+1,Guard+1,Goddess' Embrace and Weapon Handling will be active.\n\nGrace+2\t20\tWhen there are only 2 or fewer active skills: Attack Up (Absolute), Issen+3, High-Grade Earplugs, WindRes (Large),Quake Res+1, Evasion+1,Guard+1,Goddess' Embrace and Weapon Handling will be active.\n\nGrace+1\t10\tWhen there is only 1 active skill: Attack Up (Absolute), Issen+3, High-Grade Earplugs, WindRes (Large),Quake Res+1, Evasion+1,Guard+1,Goddess' Embrace and Weapon Handling will be active.\n\nCompensation\t×\tCompensation\t10\tDeath God's Embrace, Sharpness+1, Attack Up (Absolute), Evasion+2 and Critical Eye +4.\n\nDark Pulse\t×\tDark Finale\t20\tUpon health dropping to 0, you will rise again, gaining full Health and Stamina bars and Heavy Buffs but will also start constantly losing health at a steady rate.\n\nHealth loss cannot be halted by any methods and you will cart after exactly one minute has passed. Although the health degeneration cannot be stopped by any standard method, completing a quest will disable health loss meaning any final carts will not occur.\n\nIn your risen form you are buffed with the following skills: Adrenaline+2, Starving Wolf+2, Defense+120, Sharpening Artisan, Super HG Earplugs, Violent Wind Breaker, TremorRes +2, Heat Cancel and Cold Cancel.\n\nAlongside these skills you will gain a Super Armour effect that grants immunity to all forms of knock back.\n\nBlazing Grace\t×\tBlazing Majesty+2\t15\tAdrenaline+2, Red Soul, Bombardier, Fire Res+30, Artillery God, Summer Person, Terrain Damage Decreased (Large)\n\nFire Attack Up (Large), Flame Sword+3 and Bomb Sword+3 combined into 1 skill.\n\nBlazing Majesty+1\t10\tAdrenaline+1, Red Soul, Bombardier, Fire Res+20, Artillery Expert, Heat Cancel, Terrain Damage Decreased (Small)\n\nFire Attack Up (Small), Flame Sword+2 and Bomb Sword+2 combined into 1 skill.\n\nDrawing Arts\t×\tDrawing Arts+2\t20\tWhile weapon unsheathed: Peerless, Evasion+2, Weapon Handling\n\nWhile weapon is sheathed: Damage Recovery Speed+2, Quick Stamina Recovery (Large)\n\nDrawing Arts+1\t10\tWhile weapon unsheathed: Marathon Runner, Evasion+1, Weapon Handling\n\nWhile weapon is sheathed: Damage Recovery Speed+1, Quick Stamina Recovery (Small)\n\nIce Age\t×\tIce Age\t10\tUpon attacking a monster the hunter is surrounded by an icy aura. This aura deals damage to all monsters in its area and grants a number of different skills.This aura has three stages and will progress with more hits.\n\nThe aura also grants Stamina Recovery Up and Sharp Sword to all hunters affected by the aura and the one with the skill also gets Winter General.\n\nDamage is dealt once every second fixed rather than over time.";

    public static string GetGameSigilsInfo => "Sigils are similar to decorations but exclusively used in G Rank weaponry. They are crafted at theCat Smith who creates random Gou weapons.Sigil Slots are triangular slots that either replace decoration slots in standard G Rank weaponry or are part of hybrid slots that can take either decorations or a sigil.Weapons can have up to three sigils. Sigils with variable values will generally always stack but ones that add or enhance abilities and motions generally have a fixed effect regardless of the number slotted.\n\n" +
                "Although most weapons have dedicated sigils, many are far from optimal. You should carefully consider the frequency with which you will actually be using the attack. For an excellent example,the Dual Swords sigils for the Rush Slash and Frontip Slash have incredibly low viability for Extreme Style because of Extreme Demon Mode not having access to them and because of optimal play involving almost exclusive use of that mode. Similarly,the GS Guard Slash is outright unavailable for Extreme Style and Upswings will mostly be charged - which does not benefit from the sigil - and so the sigils to buff those moves are mostly useless, especially compared to simply buffing raw. Realistically, if you want the best results you should really just be aiming almost entirely to get Attack or Elemental on sigils and one or more of your weapon's specific sigils. For Gunlance you should always aim to have Lv9 shelling if you are utilizing Shelling and Wyvern Fires.\n\n" +
                "Optimal sigil build is 15/15/15 Attack/Element Adv Shiten, 15/15/15 Attack/Element UL Sigil, 15/12(Duration)/20(Cooldown) Attack/Element Zenith Sigil (for multiplayer: Z Area of Effect)\n\n" +
                "Health and Stamina\n" +
                "Sleeping: Recover gear by using the gesture 「睡覺」 (does not stack)\n\n" +
                "Offensive\n" +
                "Attack Adjustment: -10 / +15. Adjusts the true raw of the weapon by the stated amount.\n\n" +
                "Attribute Adjustment: -10 / +15. Adjusts the true elemental of a weapon by the stated amount for +10 Displayed element per point.\n\n" +
                "Affinity: -10 / +15. Adjusts the affinity percentage of a weapon by the amount stated.\n\n" +
                "Flying Wyvern Slayer: -10 / +15. Adjusts the true raw value stated while in an area containing the specific defined species.\n\n" +
                "Bird Wyvern Slayer: -10 / +15. Adjusts the true raw value stated while in an area containing the specific defined species.\n\n" +
                "Carapaceon Slayer: -10 / +15. Adjusts the true raw value stated while in an area containing the specific defined species.\n\n" +
                "Piscine Slayer: -10 / +15. Adjusts the true raw value stated while in an area containing the specific defined species.\n\n" +
                "Fanged Beast Slayer: -10 / +15. Adjusts the true raw value stated while in an area containing the specific defined species.\n\n" +
                "Brute Wyvern Slayer: -10 / +15. Adjusts the true raw value stated while in an area containing the specific defined species.\n\n" +
                "Leviathan Slayer: -10 / +15. Adjusts the true raw value stated while in an area containing the specific defined species.\n\n" +
                "Elder Dragon Slayer: -10 / +15. Adjusts the true raw value stated while in an area containing the specific defined species.\n\n" +
                "Minion Slayer: -10 / +15. Adjusts the true raw value stated while in an area containing small minion monsters.\n\n" +
                "Hot: -10 / +15. Adjusts true raw by the value stated while in a hot area that would require a Cold Pot(Desert,Volcano).\n\n" +
                "Cold: -10 / +15. Adjusts true raw by the value stated while in a cold area that would require a Hot Pot(Snowy Mountain, Swamp Caves).\n\n" +
                "Morning Slayer: -10 / +15. Adjusts true raw by the value stated while it is morning Taiwan Time or Japan Time\n\n" +
                "Night Slayer: -10 / +15. Adjusts true raw by the value stated while it is night Taiwan Time or Japan Time\n\n" +
                "Breeding Season Slayer: -10 / +15. Adjusts true raw by the value stated when in quests taking place in the Breeding Season (Green).\n\n" +
                "Warm Season Slayer: -10 / +15. Adjusts true raw by the value stated when in quests taking place in the Warm Season (Orange).\n\n" +
                "Cold Season Slayer: -10 / +15. Adjusts true raw by the value stated when in quests taking place in the Cold Season (Blue).\n\n" +
                "Monday Slayer: -10 / +15. Adjusts true raw by the value stated when it is Monday Taiwanese or Japanese Time\n\n" +
                "Tuesday Slayer: -10 / +15. Adjusts true raw by the value stated when it is Tuesday Taiwanese or Japanese Time\n\n" +
                "Wednesday Slayer: -10 / +15. Adjusts true raw by the value stated when it is Wednesday Taiwanese or Japanese Time\n\n" +
                "Thursday Slayer: -10 / +15. Adjusts true raw by the value stated when it is Thursday Taiwanese or Japanese Time\n\n" +
                "Friday Slayer: -10 / +15. Adjusts true raw by the value stated when it is Friday Taiwanese or Japanese Time\n\n" +
                "Saturday Slayer: -10 / +15. Adjusts true raw by the value stated when it is Saturday Taiwanese or Japanese Time\n\n" +
                "Sunday Slayer: -10 / +15. Adjusts true raw by the value stated when it is Sunday Taiwanese or Japanese Time\n\n" +
                "Unity: Adjusts true raw by +5 if all hunters in quest are nearby. Does not stack.\n\n" +
                "Strengthen Bow Raw: Adds additional raw damage based on attack strength. Formula: Attack x 0.015 x Charge Modifier x Hitbox.\nLv1: 0.04x\nLv2: 1.0x\nLv3: 1.5x\nLv4: 1.85x\nSnipe Lv3: 1.0x\nSnipe Lv4: 1.1x\nRising: 2.0x\nIs not affected by affinity, coatings or critical distance. Does not stack, does not work with Bows with Elemental\n\n" +
                "Strengthen SnS Raw: Adds additional raw damaged based on attack strength. Formula is: Attack x0.025 x Sharp Multiplier x Hitbox\nBlue: 1.0625x\nWhite: 1.125x\nPurple: 1.15x\nCyan: 1.20x\nIgnored motion values, Additional 0.2x the extra damage is added by Fencing+2. Does not stack, does not work with Sns with Elemental or Status.\n\n" +
                "Unlimited Mode: Only one Up roll can be active at any time. E.g. +14 DS Up and a +12 DS Up sigils would only be +14, not +26\n\n" +
                "Weapon Class Up: +1 / +15. Increases attack, affinity, and elemental values of appropriate weapon class.\n\n" +
                "Defensive\n\n" +
                "Defense: -10 / +20. Alters defense by the stated value.\n\n" +
                "Helper: Hunter's that are not G Rank gain +50defense in quests with a user of this sigil (does not stack).\n\n" +
                "Status\n" +
                "Status Attack: Increases Status value on weapons by 1.1x. Does not stack with more sigils. Does stack with Guild Poogie, Status Attack Up and Status Phials.\n\n" +
                "Stun Value: Increases KO inflicted by 1.1x. Only one sigil applies, does not stack with multiple sigils. Stacks with Caravan Skill (1.1x) and Active Feature (1.5x) for a maximum of 1.815x\n\n" +
                "Items\n" +
                "Omnivore: Chance to heal by using any consumable. Does not stack\n\n" +
                "Ballista Saver: Chance to not consume Ballista ammo by firing. Does not stack.\n\n" +
                "Map\n" +
                "Balloon's Friend: Allows unlimited waving at the Balloon to locate monster. Does not stack, does not force balloon spawns.\n\n" +
                "Decoy: Use the Clap gesture to attract a monster's attention to yourself. Does not stack.\n\n" +
                "Gathering\n" +
                "Dowsing: +1 / +15. Apparently increases the rate of Rare and G Rank items from a gathering spot's item pool. Unclear how it works mechanically, possibly similar to Caravan carving skill which rerolls if you get a carve above 51% and removes that item from the carving pool\n\n" +
                "Carving Division: Always carve Raw Meat in 2 stacks. Does not stack\n\n" +
                "Rewards\n" +
                "Hunter Soul: Increases Colour PP by +1 to the appropriate types on all eligible quests. Does not stack.\n\n" +
                "Soul Collection: Increases Soul from quests during festival by +1. Does not stack\n\n" +
                "Money Expert: +1 / +10. Increases money received from quest by 1.10 x Defined (Value/100) (e.g. +6 is 1.16x original Zeny or GZeny)\n\n" +
                "Experience Expert: +1 / +10. Increases Ranking Points received from quest by 1.01x Defined Value. Additive 1.10x, 1.20x and 1.30x on three separate sigils would be 1.50x, not 1.71x. Does not affect GSRP, only GRP.\n\n" +
                "Miscellaneous\n" +
                "Manager's Friend: +1 / +10. Increased chance of Farm Waifu appearing during quests.\n\n" +
                "Gook's Friend: +1 / +10. Increases chance of Rare Gooks on Gook Quests.\n\n" +
                "Halk's Friend: Increased chance of Halk dropping an egg at quest completion. Does not stack.\n\n" +
                "Cat Breeder: Partnyaa mood is more likely to increase.\n\n" +
                "Daddy's Influence: Perform「耍賴/ SpoiledBrat」action to get a 10% discount in stores.\n\n" +
                "Sonic Bomb Range: Increases the range of sonic bomb effect.\n\n" +
                "Weapon Movement Speed: Movement Speed will increase after unsheathing a weapon. Speed increase is between 1.13x to 1.20x. Does not stack or overlap with most similar armour skills. Overlaps with Hammer Charge Speed, SnS Hiden, Hunting Horn Movement Speed Song, Dual Swords Demon Mode and possibly MS Hiden speed buffs.\n\n" +
                "Rarity 1-12: Changes the rarity of a G Rank Weapon to be the defined value at the cost of lowering Raw, Elemental and Status values.\n\n" +
                "Zenith Sigil\n" +
                "Duration: +1 / +12. Dictates the effect of the Zenith Sigil, duration is 15 seconds plus the value of the sigil.\n\n" +
                "Cooldown: +1 / +20. Dictates the recharge duration of the sigil, cooldown is 120 seconds minus the value of the sigil.\n\n" +
                "[Zenith] Fire Res: +4 / +20. Increases the associated elemental resistance and removes any active blights of that element.\n\n" +
                "[Zenith] Water Res: +1 / +10. Increases the associated elemental resistance and removes any active blights of that element.\n\n" +
                "[Zenith] Thunder Res: +1 / +10. Increases the associated elemental resistance and removes any active blights of that element.\n\n" +
                "[Zenith] Ice Res: +1 / +10. Increases the associated elemental resistance and removes any active blights of that element.\n\n" +
                "[Zenith] Dragon Res: +1 / +10. Increases the associated elemental resistance and removes any active blights of that element.\n\n" +
                "[Zenith] Healing: +1 / +5. Recovers health over time while active. Can survive Rukodioras' nuke, Zinogres' nuke and Guanzorumus' nuke.\n\n" +
                "[Zenith] Heroics: Causes a single hit to deal 0damage.Reactions to the hit such as launching still occur.\n\n" +
                "[Zenith] Attack: +1 / +15. Increases raw values for its duration by 30+20x Value (e.g. for +8 it would be 20 * 8 = 160 + 30 = 190 True Raw.\n\n" +
                "[Zenith] Elemental: +1 / +15. Multiplies elemental values for its duration by 1.3 + Value * 0.1 (e.g. for +8 it would be 0.1 * 8 = 0.8 + 1.3 = 2.1x Elemental)\n\n" +
                "[Zenith] Movement Speed: +1 / +5. Increases movement speed for its duration.\n\n" +
                "Area of Effect Sigil\n" +
                "[Ranged] Attack: +1 / +15. Increases Attack Value while in the radius of the dome. Dome is coloured Red. Value for each roll is 25 + (Value * 5) (e.g. for +8 it would be 8 * 5 = 45 + 25 = 70 Attack Value)\n\n" +
                "[Ranged] Elemental: +1 / +15. Increases Elemental Value while in the radius of the dome. Dome is coloured Pink. Value for each roll is 50 + (Value * 50) (e.g. for +8 it would be 8 * 50 = 400 + 50 = 450 Elemental Value)\n\n" +
                "[Ranged] Affinity: +1 / +15. Increases Affinity Value while in the radius of the dome. Dome is coloured Blue. Value for each roll is 20+(Value *2)(e.g. for +8 it would be 8 * 2 = 16 + 20 = +36% Affinity\n\n" +
                "[Ranged] Status: Increases Status values by 1.50x while in the radius of the dome. Dome is coloured Purple. Does not stack.\n\n" +
                "[Ranged] Stun: Increases Stun values while in the radius of the dome. Dome is coloured Yellow.\n\n" +
                "[Ranged] Healing: Recovers Health while in the radius of the dome. Dome is coloured Green.\n\n" +
                "[Ranged] All Res: +1 / +15. Increases All Resistances while in the radius of the dome. Dome is coloured Black. Resistance is increased by roll x 2.\n\n"
                ;

    
    public static string GetGameCaravanAbout => "Pallone Caravan\r\nThe Pallone Caravan is a Hub Area that you can enter from Mezeporta at HR17\r\nIf you talk to The Chief when you enter, you'll recieve a Caravan Gem\r\nPallone Carnival\r\n・Great Reduction Festival：Slightly increases the chance of obtaining quest rewards, and also increases the chance of obtaining items in emergency missions.\r\n・Great Bead Festival：Party points (PP) will increase 1.5 times more than normal\r\n・Great Recommendation：Reputation points will be increased by 1.5 times than usual.\r\n・Great Reward Festival：Greatly increases the chances of getting quest rewards\r\n\r\nFacilities\r\n・Hospitality Lounge\u3000※You can enter with the reputation of 「Hard Working Hunter」\r\n\u3000Lounge Bento(1000CP)can be purchased.\r\n\r\n・Guest House\u3000※You can enter with the reputation of 「Familiar Hunter」\r\n\u3000Inside is the Specialty Workshop, and Specialty Item Shop.\r\n・Guest House Floor 2\u3000※You can enter with the reputation of 「Tale Telling Hunter」\r\nCarnival Medicine：200000CP\r\n\u3000\u3000Generate a Great ***** listed above randomly. It is effective when you are the owner of the quest (applies to all participants). 2 hours\r\n\r\nReputation\r\n\u3000Caravan Quests gain recognition, instead of HRP\r\n\r\nName\tPoints\r\nTemporary Hunter\t0\r\nBarely a Hunter\t5940\r\nHard Working Hunter\t37088\r\nFamiliar Hunter\t121688\r\nTale Telling Hunter\u3000\t275648\r\nPositive Hunter\t689648\r\nMemorable Hunter\t1153328\r\nTrustworthy Hunter\t1799792\r\nAppreciated Hunter\t2492432\r\nWidely Trusted Hunter\t3185072\r\nReliable Hunter\t4180752\r\nSuccessful Hunter\t5176432\r\nExclusive Hunter\t6257456\r\nAspiring Legend\t8028848\r\nLegendary Hunter\t9926768\n\n" +
                "Hunter Jewel\r\nColor\r\n・There are eight colors of hunter beads: pink, brown, yellow, green, white, blue, light blue, and rainbow. Rainbow is a special color.\r\n・The color will change during regular maintenance (every Wednesday).\r\n\u3000\u3000※Apart from this, it may change to \"rainbow color\" at 12:00 noon\r\n・If it is rainbow, it will change to another color at 12:00 the next day.\r\n\r\n・In order to accumulate colors, clear route 101 and beyond.\r\n・Only the number of colors of the people who participated in the quest will be added, but rainbow will change to all colors +1\r\n\u3000※Since the effects of rainbow colors do not overlap, all colors remain +1 no matter how many rainbow colors are present.\r\n\r\n・Activation is required when the hunter jewel is strengthened beyond the second level.\r\n・Once the Activation period has passed, you will no longer be able to use the Hunter Jewel skill.(outdated?)\r\n\r\nSkill\r\n・Can activate up to 3 skills with a total cost of up to 10";

    public static string GetGameCaravanQuests => "Caravan Quest\r\nCharacteristics\r\n・All materials obtained during the quest are converted into CP.\r\n※Items obtained by poogie will be kept as material.\r\n・There is an emergency mission during the quest, and if you complete it, you will get 500 CP or the following items.\r\n\u3000Common：Return Ball,Vitality Agent G,Sword Crystals,Monster Mats\r\n\u3000Great Reduction Festival：Gunlance Coolant,Guts Tkt,Adrenaline Potion,Starving Wolf Potion,L.Sword Spirit Drug\r\n\u3000Other than Great Reduction Festival：PoisonDefenceTkt,Sleep Defence Tkt,Para Defence Tkt,Emergency Bag\r\n・There is no disadvantage in clearing via a sub-target.\r\n・The quests are the same as those of the Normal Hunting.\r\n・The quests you can post are limited by the * rank you've cleared.Participation is open to everyone regardless of *rank。\r\n・If you return halfway through, you will receive the number of recognition, CP, etc. that you have earned so far.\r\n・Only ★0 will not get the hunter recognition.\r\nTension gauge (verification required)\r\n・By filling up the Tension Gauge, your attack and defense power will increase. Accumulate the gauge to the maximum and get 1.5 times atk.\r\n・During a strike, attacking an ally raises the tension gauge of the attacked side.\r\n・The amount of increase in the gauge is determined according to the Route difference. 1 hit increases attack and defense power by 1.01 times (up to half of the gauge).\r\n・Also, tension may increase depending on the route difference cleared at the start of the quest (up to half of the gauge)\r\n・Tension UP at the start of the quest + Tension UP during the quest will maximize the gauge.";

    public static string GetGameCaravanPioneer => "Pioneering\r\n・You can develop a map by talking to the balloon cat in Parone Caravan.\r\n・By developing it, you will be able to acquire special items.\r\n\u3000Also, by posting a development quest, other hunters will be able to participate in the quest.\r\n\r\nPioneering Procedure\r\n１．Talk to the balloon cat and select a map.\r\n\u3000\u3000(When switching to another map, all developments up to that point will be reset.)\r\n２．Select an area and place a base (gathering, mining, insects, artifacts, defense). Two can be placed in one area.\r\n\u3000\u3000(Materials and CP required for installation are listed separately)\r\nSelecting Migration instead of Restart will keep your levels while migrating to a new map.\r\n３．The base is completed by completing a specified number of quests.\r\n\u3000\u3000(You can fail the quest. Abandoning is not allowed.)\r\n４．By hiring a hired cat, you can accelerate the development speed.\r\n５．After a certain period of time, bases begin to deteriorate. It can be updated just by talking to the balloon cat, and the maintenance fee is free. You can't go below Lv1.\r\n\r\nBase development speed\r\nLV1： Complete 20 quests\r\nLV2： Complete 50 quests\r\nLV3： Complete 100 quests\r\n\r\nEmployed cat effect\r\n\u3000\u3000\u3000\u30000～6000CP： No Change\r\n\u30007000～29000CP： 24 hours, 1.2 times adjustment to the number of required quests\r\n30000～59000CP： 24 hours, 1.5 times adjustment to the number of required quests\r\n60000CP～\u3000\u3000\u3000\u3000： 24 hours, 2.0 times adjustment to the number of required quests\r\n\r\nDevelopment progress\r\nLevel up according to the base LV of each area\r\n11～20： LV1 One flag at the base camp\r\n21～30： LV2 Two flags at the base camp\r\n31～40： LV3 Three flags at the base camp\r\n41～50： LV4 Four flags at the base camp\r\n51～60： LV5 Five flags at the base camp\r\n\r\nPioneer Quest\r\n・Quests can be changed in the public quest settings to change the targets to be defeated and the level of difficulty.\r\n・The specifications of the quest are the same as the caravan, and the color of the beads is added to the color of the participant.\r\n・Collecting from the base is not possible without going on a pioneering quest.\r\n・If a pioneering quest is left open to the public, other hunters can join the quest.\r\n\u3000\u3000(public settings will be canceled when you log out)\r\n・When the pioneer quest is accepted, when play time (regardless of success or failure) reaches a certain amount, you can receive a public reward from the balloon cat.\r\n\u3000\u3000(Before receiving the materials, you can set the quest for the materials you want and you will receive the materials for that quest.)\r\n・Pioneer quests are randomly selected from the hunter's quests according to the set HR and target monster conditions.\r\n・One worker cat is placed for each type of base in each area, if there is a monster, the cat will perform a support attack with rock throwing and throwing bombs.\r\n・For defensive bases, ballistas are launched from balloons. The number of ballistas fired increases as the base level increases.\r\n・Difficulty\r\n\u3000Weak: Rewards are halved (one piece per frame will be 0), physical strength and attack power are 60%\r\n\u3000Normal: normal rewards\r\n\u3000Strong: 1.5x reward (fractions are rounded down), 1.5x HP and ATK\r\n\u3000Strongest: 2x rewards, 2x health and attack power";

    public static string GetGameCaravanSkills => "If your caravan gem is below level 8 you will have less pages. The skills will however be in the same order.\n\n" +
                "Page 1\n" +
                "Cafeteria Regular\n" +
                "Chance to not consume food when preparing buffs for a quest.\n\n" +
                "Negotiation\n" +
                "1/8th chance to get a 10%/15%/25% discount on buying things.\n\n"
                + "My Tore Celebrity\n" +
                "Garden managers affection goes up 1.5x/2x/3x usual values.\n\n"
                + "Gallery Celebrity\n" +
                "5000/7000/10000 extra Gallery Points on evaluations.\n\n"
                + "Garden Celebrity\n" +
                "1.2x/1.3x/1.5x items received from garden tools.\n\n" +
                "Recovery Items Up\n" +
                "Herb, Potion, Mega Potion and Lifepowder effect 1.1x. 100% Bitterbug and Antidote Herb effectiveness.\n\n" +
                "Blunt Striker\n" +
                "Bowgun Melee damage up (3.0x).\n\n" +
                "Courage\n" +
                "No inching upon being spotted by monsters.\n\n" +
                "Combination Technique [Small]\n" +
                "10% additional combination success rate.\n\n" +
                "Riser [Small]\n" +
                "1.5x iframes during the rising animation after taking a hit.\n\n" +
                "Page 2\n" +
                "Perfect Defense [Small]\n" +
                "Blocking within 3 frames of an attack hitting you will cause no stamina or sharpness decrease and prevent knockback and allow you to immediately evade after the block.\n\n" +
                "Lander\n" +
                "No recovery time after falling, no egg loss on falling.\n\n" +
                "Vine Superhero\n" +
                "No stamina is consumed while climbing.\n\n" +
                "Vine Master\n" +
                "Getting hurt while climbing will not cause you to fall\n\n" +
                "Art of Dancing\n" +
                "Using the 'Dance' action will give +10 attack for one minute. Uses the same buff slot as Power Seeds etc.\n\n" +
                "Combination Celebrity\n" +
                "Combining items has a chance to produce double the usual results quantity wise.\n\n" +
                "Combination Technique [Medium]\n" +
                "15% additional combination success rate.\n\n" +
                "Riser [Medium]\n" +
                "2.0x iframes during the rising animation after taking a hit.\n\n" +
                "Perfect Defense [Medium]\n" +
                "Blocking within 4 frames of an attack hitting you will cause no stamina or sharpness loss and prevent knockback and allow you to immediately evade after the block.\n\n" +
                "Elite Flame\n" +
                "Increases the Friendly Fire (heat up) meter over time instead of by friendly fire when on caravan quests.\n\n" +
                "Page 3\n" +
                "Mine Expert\n" +
                "Pickaxes are less likely to break after use.\n\n" +
                "Insect Expert\n" +
                "Bug nets are less likely to break after use.\n\n" +
                "(Recommended) KO Technique\n+" +
                "Increases stun damage dealt by 1.1x. Stacks with Sigil.\n\n" +
                "Combination Technique [Large]\n" +
                "20% additional combination success rate.\n\n" +
                "Riser [Large]\n" +
                "2.0x iframes during the rising animation after taking a hit.\n\n" +
                "Secret Healing Technique [Small]\n" +
                "1/12th chance of not consuming healing items when used. (Up to 5 times a quest)\n\n" +
                "(Recommended) Perfect Defense [Large]\n" +
                "Blocking within 4 frames of an attack hitting you will cause no stamina or sharpness loss and prevent knockback and allow you to immediately evade after the block. Perfectly timed blocks also cause a Reflect effect which deals 72 motion (no critical hits, no elemental).\n\n" +
                "Unstable Defender [Small]\n" +
                "90% reduction of damage and 20% chance of no damage while blocking\n\n" +
                "Rousing Attacker [Small]\n" +
                "Attacking a monster while you have 50 or lower health a 40% chance to cause you to regain 10 HP. Cannot trigger more than once every 10 seconds. Can trigger up to 10 times in a quest.\n\n" +
                "Revenge![Small]\n" +
                "After getting up from a hit there's a chance (1 x Health Loss % chance) to gain 25 attack, 50 defense and no minor knockback for 20 seconds. Counted as a Power Pill for terms of buff effects and does not overlap\n\n" +
                "Page 4\n" +
                "Hot Master\n" +
                "Grants the effects of Heat Cancel\n\n" +
                "Cold Master\n" +
                "Grants the effects of Cold Cancel\n\n" +
                "Prepared Stance\n" +
                "If you perform the gesture 應戰準備 <act20> for around 30 seconds the attack ceiling on your currently equipped weapon type increases for a fixed duration.\n\n" +
                "Shield Angel\n" +
                "Decreases the amount of damage taken when on support quests on Berserk Raviente.\n\n" +
                "Spear Angel\n" +
                "Increases the amount of damage dealt by Ballistae when playing support on Berserk Raviente.\n\n" +
                "Secret Healing Technique [Medium]\n" +
                "1/11th chance of not consuming healing items when used. (Up to 5 times a quest)\n\n" +
                "Unstable Defender [Medium]\n" +
                "90% reduction of damage and 25% chance of no damage while blocking\n\n" +
                "Rousing Attacker [Medium]\n" +
                "Attacking a monster while you have 50 or lower health a 40% chance to cause you to regain 10 HP. Cannot trigger more than once every 10 seconds. Can trigger up to 15 times in a quest.\n\n" +
                "Revenge! [Medium]\n" +
                "After getting up from a hit there's a chance (1.5 x Health Loss % chance) to gain 25 attack, 50 defense and no minor knockback for 20 seconds. Counted as a Power Pill for terms of buff effects and does not overlap\n\n" +
                "Weapon Art [Small]\n" +
                "Increases the True Raw of your equipped weapon by 1.01x of its base True Raw on all weapon types.\n\n" +
                "Page 5\n" +
                "Bonus Art\n" +
                "Food Effect is not lost after fainting.\n\n" +
                "Secret Healing Technique [Large]\n" +
                "1/10th chance of not consuming healing items when used. (Up to 5 times a quest)\n\n" +
                "Last Minute Ace [Small]\n" +
                "In the last 5 minutes of a quest you get 80% affinity but take 1.3x damage\n\n" +
                "Unstable Defender [Large]\n" +
                "90% reduction of damage and 50% chance of no damage while blocking\n\n" +
                "Rousing Attacker [Large]\n" +
                "Attacking a monster while you have 50 or lower health a 40% chance to cause you to regain 10 HP. Cannot trigger more than once every 10 seconds. Can trigger up to 20 times in a quest.\n\n" +
                "Revenge! [Large]\n" +
                "After getting up from a hit there's a chance (2 x Health Loss % chance) to gain 25 attack, 50 defense and no minor knockback for 20 seconds. Counted as a Power Pill for terms of buff effects and does not overlap\n\n" +
                "Last Minute Ace [Medium]\n" +
                "In the last 5 minutes of a quest you get 90% affinity but take 1.3x damage\n\n" +
                "Weapon Art [Medium]\n" +
                "Increases the True Raw of your equipped weapon by 1.025x of its base True Raw on all weapon types.\n\n" +
                "Wild Awakening\n" +
                "Combination of both Hot and Cold Master skills.\n\n" +
                "Instant Guard Stance\n" +
                "Combination of Weapon Art [Med] and Perfect Defense [Med]\n\n" +
                "Page 6\n" +
                "(Recommended) Shooting Rampage\n" +
                "Increases the True Raw of your equipped ranged weapon by 1.1x of its base True Raw. If using a bowguns your accuracy immediately after shooting is lowered by 1.5x.\n\n" +
                "Natural Recovery [Small]\n" +
                "Using the 'Sleep' gesture will cause your red health to refill up to 5 times.\n\n" +
                "Master Carver [Small]\n" +
                "While carving if you roll the top item in the carve table and it is below 51% you have a 1/10th chance of rerolling with that item removed from the carving pool up to a maximum of 10 times in a quest.\n\n" +
                "Last Minute Ace [Large]\n" +
                "In the last 5 minutes of a quest you get 100% affinity but take 1.3x damage\n\n" +
                "(Recommended) Weapon Art [Large]\n" +
                "Increases the True Raw of your equipped weapon by 1.05x of its base True Raw on all weapon types.\n\n" +
                "Decisive Hunter\n" +
                "Combination of Weapon Art [Med] and KO Technique\n\n" +
                "Natural Recovery [Medium]\n" +
                "Using the 'Sleep' gesture will cause your red health to refill up to 10 times.\n\n" +
                "Master Carver [Medium]\n" +
                "While carving if you roll the top item in the carve table and it is below 51% you have a 1/9th chance of rerolling with that item removed from the carving pool up to a maximum of 15 times in a quest.\n\n" +
                "Goddess of Luck [Small]\n" +
                "1/10th chance to take no damage on up to 5 hits in a quest (Stacks with Divine Protection, Diva Buff and Girly Charms).\n\n" +
                "Self-Defense\n" +
                "Combination of Weapon Art [Med] and Unstable Defender [Med]\n\n" +
                "Page 7\n" +
                "Natural Recovery [Large]\n" +
                "Using the 'Sleep' gesture will cause your red health to refill up to 15 times.\n\n" +
                "(Recommended) Master Carver [Large]\n" +
                "While carving if you roll the top item in the carve table and it is below 51% you have a 1/8th chance of rerolling with that item removed from the carving pool up to a maximum of 20 times in a quest.\n\n" +
                "Goddess of Luck [Medium]\n" +
                "1/9th chance to take no damage on up to 10 hits in a quest (Stacks with Divine Protection, Diva Buff and Girly Charms).\n\n" +
                "Goddess of Luck [Large]\n" +
                "1/8th chance to take no damage on up to 20 hits in a quest (Stacks with Divine Protection, Diva Buff and Girly Charms).";

    public static string GetGameGuildAbout => "Create/Join a Guild\r\n・Guilds can be created at HR5 or higher\r\n・You cannot create a guild with the same name as another.\r\n・Creation of a guild and application for membership are carried out by the guild receptionist in Mezeporta.\r\n\r\nMaintaining a Guild\r\n・There are 4 or more characters of Hunter Rank (HR) 3 or higher who have played MHF within the past month in the guild.\r\n・Inactive guilds will be disbanded, checked at the time of maintanence each Wednesday.\r\n・If the conditions are not met at that point, the guild association will send mail to the guild leader.\r\n1st: Attention mail\r\n2nd: Warning mail\r\n3rd: Forced disband mail\r\n・The third time, the group is forcibly disbanded and a disbandment mail is sent to all guild members.\r\n\r\nChanging Guild Leaders\r\nIf the Guild Leader does not log in for 2 consecutive weeks, the second in command will become the new Guild Leader.\r\nThe third player in the guild list will then be made the new second in command.\r\n\r\nRestrictions on disbanding or leaving a guild\r\nGuild leader (disbanded): Cannot create or apply to join a guild for 5 days (120 hours) after disbandment.\r\nGuild member (kicked): Cannot create a guild or apply for membership for 3 days (72 hours) after being kicked out.\r\nGuild members (leave): cannot create a guild or apply for membership for 10 days (240 hours) after leaving a guild.\r\n\r\nAlliance\r\nAlliances can be joined by up to three guilds.\r\nOnce an alliance is formed, the use of alliance chat is enabled.\r\nRegistration for the Hunters' Festival will be on an alliance basis and all guilds will be on the same team.\r\n\r\nGuild Rank\r\nBonuses for increasing hunting party ranks will be available after each scheduled maintenance.\r\nHunting points (RP) will accumulate 1P after logging in for a total of 30 minutes. (15 minutes/1P at authorized NetCafes)\r\nOther RPs can be obtained through official hunting competitions.\r\n\r\nRank\tRP\tCapacity\tFeatures\r\n0\t0\t30\tJump from guild chat / guild list\r\n1\t24\t30\tAlliances, Motto, Guild icon\r\n2\t48\t30\tTent hall, Item box, Rasta, Change Room\r\n3\t96\t40\tGuild Shop, +10 members\r\n4\t144\t40\tGuild Quests, Meals\r\n5\t192\t40\tGuild Pugi +1, Store discount, Pet pugi\r\n6\t240\t40\tPugi shop, Change pugi, Feed pugi\r\n7\t288\t50\tSmall hall, Item box +1pg, +10 members, +10 stamps\r\n8\t360\t50\tAdd 4 types of clothes for Guild Poogie\r\n9\t432\t50\tAdd 2 types of clothes for Guild Poogie\r\n10\t504\t60\tLarge hall, Item box +1pg\r\n+10 members, Bonus menu, +10 stamps\r\n11\t600\t60\tAdd 2nd Guild Poogie\r\n12\t696\t60\tAdd 3rd Guild Poogie\r\n13\t792\t60\tLimited time quests, Tournament quests\r\n14\t888\t60\tG-rank quests\r\n15\t984\t60\tGuild Cooking\r\n16\t1080\t60\tAdventure Cat\r\n17\t1200\t60\tBonus menu slots, Hunting road quests\r\nWeekly Bonus\r\nDepending on the number of logins in the previous week, the following effects will be triggered on partnya\r\nThe guild leader can also spend RP to add more logins.\r\n\r\nGuild Rank\tEffect Name\tEffect\r\n10\tPartnya attack power UP\tAffects the attack power of partnyas during a quest.\r\n10\tPartnya Defense UP\tAffects the amount of damage done to partnyas during a quest.\r\n10\tPartnya Health UP\tAffects the frequency of partnyas withdrawaling during a quest.\r\n17\tPartnya Courage UP\tAffects the frequency of use of partnyas using strong attacks during a quest.\r\n17\tPartnya Reflexes UP\tAffects how often your partnya guards during quests.";

    public static string GetGameGuildPoogies => "Poogie\r\nGuild store discount (from guild rank 5)\r\nThe discount is activated by successfully completing the \"pat on the head\" and \"it seems pretty happy\" responses a specified number of times.\r\n\r\n5 times successful\tGuild shop 5% discount\r\n10 times successful\tGuild shop 10% discount\r\n15 times successful\tGuild shop 15% discount\r\nThe effect disappears in any of [12 hours passed / logout / petting for a long time and getting angry] occur.\r\nPoogie Skill (Unlocked at Guild Rank 6)\r\nGiving food and \"seems to love it!\" will activate a skill according to Poogie's clothes with a probability. Only one type of skill can be activated.\r\nOnly the guild leader can purchase and change clothes.\n\n" +
                "Rank 6\n" +
                "Red & White (Poogie Thrift): Flute, Pickaxe and Bugnet 1/4 less likely to break (stacks with Whim)\n" +
                "Naked Emperor (Poogie Discount): Furniture store purchase price (Material/Zenny) 10% discount. Needs Wht Durable Fabricx45, Appropriate Partsx15\n" +
                "(Recommended) Soporific White (Poogie Taijutsu): Halves stamina consumption when evading and guarding. Needs Wht Durable Fabricx75, Flexible Medicinex22, Appropriate Partsx30\n" +
                "(Recommended) Black Green Clash (Poogie Status Attack): 1.125x attribute value for status attacks (Can be used with status skills). Needs Striped Fabricx45, Green Fabricx30\n\n" +
                "Rank 8\n" +
                "Silent Suit (Poogie Reward Technique): 1/8 chance that the next reward won't be given as a quest reward. In addition, the maximum reward frame will also be increased. Needs Wht Durable Fabricx30, Black Fabricx50, Appropriate Partsx20\n" +
                "Bewitching Pink (Poogie Defense): When attacked, 1/4 chance to reduce damage taken by 30% (Can be used with the Protection skill). Needs Wht Durable Fabricx20, Peach Fabricx45\n" +
                "Nostalgic Stripe (Poogie Escape Technique): Halves stamina when running away. Needs Striped Fabricx45, Blue Fabrix25\n" +
                "Soothing Sky (Poogie Transportation): Halves stamina consumption while running while transporting (Can be used with Marathon Runner skill). Needs Wht Durable Fabricx40, Blue Fabricx35, Appropriate Partsx10\n\n" +
                "Rank 9\n" +
                "Gentle Green (Poogie Trap Mastery): 100% success in combining pitfall and shock traps. Needs Green Fabricx35, Blue Fabricx20, Appropriate Partsx20\n" +
                "Restless Brown (Poogie Patience): When attacked, 1/6 chance of gaining super armor (no knockback). Needs Brown Fabricx30, Black Fabricx10, Appropriate Partsx10\n\n" +
                "Clothing material is RARE2 and can be put in the guild box.\r\n\r\nClothing Material\tProduction Materials\r\nWht Tough Fabric\tAnteka Peltx3、W.Velociprey Hidex2、Blangonga Peltx2\r\nAppropriate Parts\tSunstonex2、Sticky Caterpillarx1、Sm Monster Bonex1、Machalite Orex3\r\nStriped Fabric\tHigh Qual Peltx3、Striped Skinx2、Blk Rajang Peltx1、Carpenterbugx2\r\nGreen Fabric\tBuruku Hidex2、Rathian Scalex3、Gendrome Hidex3、Festi Tktx1\r\nFlexible Medicine\tBomb Arrowanax3、Firecell Stonex1、Fire Herbx5、Monster Fluidx2\r\nBlk Fabric\tRemobra Skin+x2、High Qual Peltx3、B.Gravios Shellx1\r\nPeach Fabric\tIoprey Hide+x1、Teostra Shlx1、W.Velociprey Hidex3\r\nBlue Fabric\tVelociprey Scalex3、B.Kut-Ku Scalex2、Rubbery Hidex1\r\nBrown Fabric\tGenprey Hidex4、Diablos Shlx1、Gluehopperx2";

    public static string GetGameRoadDureInfo => "The Maximum Cost is 130.\n\n" +
                "Attack\n" +
                "Lv1: Small increase to attack.(+10)\n" +
                "Lv2: Increases attack.(+20)\n" +
                "Lv3: Medium increase to attack.(+30)\n" +
                "Lv4: Large increase to attack.(+50)\n" +
                "Lv5: Very large increase to attack.(+70)\n\n" +
                "Defense\n" +
                "Lv1: Defense +30\n" +
                "Lv2: Defense +50\n" +
                "Lv3: Defense +80\n" +
                "Lv4: Defense +110\n" +
                "Lv5: Defense +150\n\n" +
                "Health Recovery\n" +
                "Lv1: Red Health recovery speed is increased.\n" +
                "Lv2: Red health recovery speed is further increased.\n\n" +
                "Fire Res\n" +
                "Lv1: Fire Res +10\n" +
                "Lv2: Fire Res +15\n" +
                "Lv3: Fire Res +25\n\n" +
                "Water Res\n" +
                "Lv1: Water Res +10\n" +
                "Lv2: Water Res +15\n" +
                "Lv3: Water Res +25\n\n" +
                "Thunder Res\n" +
                "Lv1: Thunder Res +10\n" +
                "Lv2: Thunder Res +15\n" +
                "Lv3: Thunder Res +25\n\n" +
                "Ice Res\n" +
                "Lv1: Ice Res +10\n" +
                "Lv2: Ice Res +15\n" +
                "Lv3: Ice Res +25\n\n" +
                "Dragon Res\n" +
                "Lv1: Dragon Res +10\n" +
                "Lv2: Dragon Res +15\n" +
                "Lv3: Dragon Res +25\n\n" +
                "All Res\n" +
                "Lv1: All Res +5\n" +
                "Lv2: All Res +10\n" +
                "Lv3: All Res +15\n\n" +
                "Technical Skills\n" +
                "Starting Gm Up\n" +
                "Lv1: Increases starting Gm.(+1000Gm)\n" +
                "Lv2: Increases starting Gm significantly.(+3000Gm)\n\n" +
                "Hunting Road Points Up\n" +
                "Lv1: Increases Road Point earning slightly (+10%).\n" +
                "Lv2: Increases Road Point earning (+20%).\n" +
                "Lv3: Increases Road Point earning greatly (+40%).\n\n" +
                "Bonus Stages Up\n" +
                "Lv1: Increases the likelihood of getting Bonus Stages slightly. Stacks with other players on road.\n" +
                "Lv2: Increases the likelihood of getting Bonus Stages. Stacks with other players on road.\n\n" +
                "Resurrection Knowledge\n" +
                "Lv1: Increases the number of times you can faint on the road before failing (+1 Cart).\n\n" +
                "Advancement Knowledge\n" +
                "Lv1: Attack increases every 5 stages on the Road. Floor 6 will give +20 Attack while every 5th floor after will grant +10 Attack stopping after floor 26. Maximum buff of 60 Attack for all floors above 26.\n" +
                "Lv2: Attack increases every 5 stages on the Road. Floor 6 will give +40 Attack while every 5th floor after will grant +10 Attack stopping after floor 26. Maximum buff of 80 Attack for all floors above 26.\n" +
                "Lv3: Attack increases every 5 stages on the Road. Floor 6 will give +60 Attack while every 5th floor after will grant +10 Attack stopping after floor 26. Maximum buff of 100 Attack for all floors above 26.\n\n" +
                "Last Stand\n" +
                "Lv1: Increases affinity by +30% and Attack by +80 but causes you to have a single faint on Road regardless of other skills.\n" +
                "Lv2: Increases affinity by +50% and Attack by +120 but causes you to have a single faint on road regardless of other skills.\n\n" +
                "Duremudira Skills\n" +
                "Care\n" +
                "Lv1: Slightly increases speed of revivals and the amount of health left after being revived.\n" +
                "Lv2: Increases speed of revivals and the amount of health left after being revived.\n" +
                "Lv3: Greatly increases speed of revivals and the amount of health left after being revived.\n\n" +
                "Pharmacist\n" +
                "Lv1: Increases the number of revival items you can carry by 1.\n" +
                "Lv2: Increases the number of revival items you can carry by 2.\n" +
                "Lv3: Increases the number of revival items you can carry by 4.\n\n" +
                "Virus Protection\n" +
                "Lv1: Increases resistance to Deadly Poison slightly.\n" +
                "Lv2: Increases resistance to Deadly Poison.\n" +
                "Lv3: Increases resistance to Deadly Poison greatly.\n\n" +
                "Frost Protection\n" +
                "Lv1: Increases resistance to Powerful Frost slightly.\n" +
                "Lv2: Increases resistance to Powerful Frost.\n" +
                "Lv3: Increases resistance to Powerful Frost greatly.\n\n" +
                "Gatekeeper Offensive\n" +
                "Lv1: Increases attack when facing Duremudira by +50 true raw.\n" +
                "Lv2: Increases attack when facing Duremudira by +75 true raw.\n" +
                "Lv3: Increases attack when facing Duremudira by +100 true raw.\n" +
                "Lv4: Increases attack when facing Duremudira by +150 true raw.\n" +
                "Lv5: Increases attack when facing Duremudira by +200 true raw.\n\n" +
                "Gatekeeper Defensive\n" +
                "Lv1: Increases defense when facing Duremudira.\n" +
                "Lv2: Increases defense when facing Duremudira.\n" +
                "Lv3: Increases defense when facing Duremudira.\n" +
                "Lv4: Increases defense when facing Duremudira.\n" +
                "Lv5: Increases defense when facing Duremudira.\n\n" +
                "Tower Weapons\n" +
                "The best one is Blue Tower. You generally want the upgrades to be 1 into sharpness, max poison, 4 into element, rest into raw, with a Skill Slots Up sigil.\n\n" +
                "Duremudira\n" +
                "In terms of HP, Second District Duremudira starts at 150,000 Effective HP. Deal 51k to reach 2nd phase (10k True HP), then deal 83,333 (20k True HP) to reach 3rd phase, which has 100k hp left, for a total hunt of 284,333 EHP.";

    public static string GetGameZenithSkills => "Skill Slots Up\n" +
                "Available Skill Slots go up by 1/2/3/4/5/6/7. Stacks with the +1 or +2 skill slots from having 3 or 5 standard G Rank pieces (including Z, ZF, ZY, ZX and ZP)\n\n" +
                "Flash Conversion\n" +
                "Flash Conversion Up+1: Increases attack based on your weapon's natural affinity (5 x √Base Affinity, always rounded down before addition). Example: 50% base affinity = 5 x √50 = 7.07 x 10 = 35. This only uses the true base affinity of your weapon. Sigils, Skills, SR Skills and the +5-10% from having above blue sharpness do not count towards the increase. (The sharpness bonus is always displayed\n" +
                "Flash Conversion Up+2: Increases attack based on your weapon's natural affinity (10 x √Base Affinity, always rounded down before addition). Example: 50% base affinity = 10 x √50 = 7.07 x 10 = 70. This only uses the true base affinity of your weapon. Sigils, Skills, SR Skills and the +5-10% from having above blue sharpness do not count towards the increase. (The sharpness bonus is always displayed\n\n" +
                "Stylish Assault\n" +
                "(Recommended) Stylish Assault Up+1: Boosts Attack by +20 per evade up to a maximum of +220. The duration of the buff resets every evasion. Your attack being overbuffed is indicated by the aura becoming yellow. Synergizes well with Stylish Up.\n" +
                "Stylish Assault Up+2: Boosts Attack by +40 per evade up to a maximum of +220. The duration of the buff resets every evasion. Your attack being overbuffed is indicated by the aura becoming yellow. Synergizes well with Stylish Up.\n\n" +
                "Dissolver Up\n" +
                "Lowers the Elemental Hitbox requirements for Dissolver. (15 Weakness down from 20). Unnecessary with Solid Determination.\n\n" +
                "Thunder Clad\n" +
                "Thunder Clad Up+1: Increases the active duration of Thunder Clad to 80 seconds (+20)\n" +
                "Thunder Clad Up+2: Increases the active duration of Thunder Clad to 120 seconds (+60)\n\n" +
                "Ice Age Up\n" +
                "Increases the maximum duration that the Ice Age aura can stay at a stage by +3 seconds. Only affects top tier stage (i.e. Stage 3 is 9>12 seconds but if it hits stage 2 that is 10 seconds for decay, not 13).\n\n" +
                "Hearing Protection\n" +
                "Hearing Protection Up+1: Increases Hearing Protection by one tier, allows blocking of Ultra Roars with Super High-Grade Earplugs.\n" +
                "Hearing Protection Up+2: Increases Hearing Protection by two tiers, allows blocking of Ultra Roars with High-Grade Earplugs.\n" +
                "Hearing Protection Up+3: Allows blocking of Ultra Roars with Earplugs.\n\n" +
                "Wind Res Up\n" +
                "Wind Res Up+1: Increases Wind Res by 1 tier, allows blocking of Ultra Wind Pressure with Violent Wind Breaker\n" +
                "Wind Res Up+2: Increases Wind Res by 2 tiers, allows blocking of Ultra Wind Pressure with Dragon Wind Breaker\n" +
                "Wind Res Up+3: Increases Wind Res by 3 tiers, allows blocking of Ultra Wind Pressure with Wind Res (Large)\n" +
                "Wind Res Up+4: Allows blocking of Ultra Wind Pressure with Wind Res (Small)\n\n" +
                "Quake Res\n" +
                "Quake Res Up+1: Increases Quake Res by 1 tier, allows blocking of Ultra Tremor with Quake Res+1\n" +
                "Quake Res Up+2: Allows blocking of Ultra Tremor with Quake Res+1.\n\n" +
                "Poison Res\n" +
                "Poison Res Up+1: Upgrades Poison Res by 1 tier, enables Super Poison Damage Halved with Poison Immunity. There is no immunity for Super Poison. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n" +
                "Poison Res Up+2: Super Poison Damage Halved. There is no immunity for Super Poison. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n\n" +
                "Paralysis Res\n" +
                "Paralysis Res Up+1: Upgrades Paralysis Res by 1 tier, enables Super Paralysis Halved with Paralysis Immunity. There is no immunity for Super Paralysis. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n" +
                "Paralysis Res Up+2: Super Paralysis Halved. There is no immunity for Super Paralysis. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n\n" +
                "Sleep Res\n" +
                "Sleep Res Up+1: Upgrades Sleep Res by 1 tier, enables Super Sleep Halved with Sleep Immunity. There is no immunity for Super Sleep. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n" +
                "Sleep Res Up+2: Super Sleep Halved. There is no immunity for Super Sleep. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n\n" +
                "Vampirism\n" +
                "Vampirism Up: Increases level of Vampirism by one. Increases leech rate to 100% if you already have Vampirism+2. Leeching with Vampirism+3 regains the same amount of health as Vampirism+2, the buff is purely to the leech chance and is not much of a buff compared to that of increasing Vampirism+1 to +2.\n\n" +
                "Drug Knowledge\n" +
                "Drug Knowledge Up: Status weapons always inflict 42% of their original status value (Up from 38%). Does not increase the amount of raw gained from the original status value beyond 1/4th. Normal status infliction rate is 33% chance of 100% status points.\n\n" +
                "Assistance\n" +
                "Assistance Up: Party members within the area of effect of Assistance get Status Immunity (Myriad). Immunity skill still only affects other hunters, does not affect the person with the skill active.\n\n" +
                "Bullet Saver\n" +
                "Bullet Saver Up+1: Upgrades Bullet Saver by 1 tier. If you have Bullet Saving Master, further increases the rate at which you save bullets or coatings to around 46.4%.\n" +
                "Bullet Saver Up+2: Further increases the rate at which you save bullets or coatings to around 46.4%.\n\n" +
                "Guard\n" +
                "(Recommended for Lance/Gunlance) Guard Up+1: Upgrades Guard by one level. If you already have Guard+2, decreases the amount of knockback, stamina loss and health loss while guarding and increases the size of the guard window. Lances and Gunlances only: Only applies when exceeding Guard+2, Guarding affects 360 degrees around you, rear hits still cause knockdown. Heavy and Ranged Guards can block attacks that were previously unblockable. Lance's Guard Meter fills faster (Extreme Style). Gunlances have lowered Wyvern Fire cooldowns. Synergizes well with Reflect Up, Obscurity Up and Rush Up.\n" +
                "Guard Up+2: Decreases the amount of knockback, stamina loss and health loss while guarding and increases the size of the guard window. Lances and Gunlances only: Only applies when exceeding Guard+2, Guarding affects 360 degrees around you, rear hits still cause knockdown. Heavy and Ranged Guards can block attacks that were previously unblockable. Lance's Guard Meter fills faster (3 Hits > 2 Hits per phial). Gunlances have lowered Wyvern Fire cooldowns (10s faster, 5s with Hiden). Synergizes well with Reflect Up, Obscurity Up and Rush Up.\n\n" +
                "Adaptation\n" +
                "Adaptation Up+1: Upgrades Adaptation by one level. If you already have Adaptation+2, increases the % of the adapted hitbox's value used to 90% for Cutting or Impact and 81% for Ranged Weapons.\n" +
                "Adaptation Up+2: Increases the % of the adapted hitbox's value used to 90% for Cutting or Impact and 81% for Ranged Weapons.\n\n" +
                "Encourage\n" +
                "Encourage Up+1: Upgrades Encourage+1 to Encourage+2. If you already have Encourage+2, adds party wide Marathon Runner and Stamina Recovery Up Large.\n" +
                "Encourage Up+2: Adds party wide Marathon Runner and Stamina Recovery Up Large.\n\n" +
                "Reflect\n" +
                "(Recommended) Reflect Up+1: Upgrades Reflect by 1 tier. Going beyond Reflect+3 increases the motion value of the Reflect+3 motion (48 > 68). Also buffs perfect guard reflects while active (72 > 92). The hit uses your current attack and sharpness values and inflicts impact based damage. Reflect motions cannot deal critical hits, inflict status damage or deal elemental damage. Consumes 1 sharpness on reflection hitting a monster. Does not benefit from Fencing+2 or utilize sword crystals if loaded. This skill can trigger on every part of any attacks which hit multiple times without any cooldown period. Does not buff the Perfect Guard reflect without actually having Reflect+3 active alongside Reflect Up. Synergizes well with Guard Up, Rush Up and Obscurity Up.\n" +
                "Reflect Up+2: Upgrades Reflect by 2 tiers. Going beyond Reflect+3 increases the motion value of the Reflect+3 motion (48 > 68). Also buffs perfect guard reflects while active (72 > 92). The hit uses your current attack and sharpness values and inflicts impact based damage. Reflect motions cannot deal critical hits, inflict status damage or deal elemental damage. Consumes 1 sharpness on reflection hitting a monster. Does not benefit from Fencing+2 or utilize sword crystals if loaded. This skill can trigger on every part of any attacks which hit multiple times without any cooldown period. Does not buff the Perfect Guard reflect without actually having Reflect+3 active alongside Reflect Up. Synergizes well with Guard Up, Rush Up and Obscurity Up.\n" +
                "Reflect Up+3: Increases the motion value of the Reflect+3 motion (48 > 68). Also buffs perfect guard reflects while active (72 > 92). The hit uses your current attack and sharpness values and inflicts impact based damage. Reflect motions cannot deal critical hits, inflict status damage or deal elemental damage. Consumes 1 sharpness on reflection hitting a monster. Does not benefit from Fencing+2 or utilize sword crystals if loaded. This skill can trigger on every part of any attacks which hit multiple times without any cooldown period. Does not buff the Perfect Guard reflect without actually having Reflect+3 active alongside Reflect Up. Synergizes well with Guard Up, Rush Up and Obscurity Up.\n\n" +
                "Stylish\n" +
                "(Recommended for evasion playstyle) Stylish Up: Successful evades cause your weapon to use no sharpness for a fixed number of attacks as well as regaining the usual sharpness from Stylish and releasing an Area of Effect attack with a motion value of 30. Amount of hits that do not consume sharpness varies based on the weapon type in use.\nDS: 5 hits.\nSnS, Tonfa: 4 Hits.\nLS, Lance, Swaxe F, Magnet Spike: 3 Hits.\nGS, Hammer, HH, GL: 2 Hits.\nUsing Consumption Slayer reduces this to 1 hit for everything except SnS and DS which have 2 hits. Stylish Up AoE Motion cannot deal critical hits, inflict status damage or deal elemental damage. The motion does not benefit from Fencing+2 nor does it utilize sword crystals if they are loaded. Synergizes well with Stylish Assault Up.\n" +
                "Vigorous\n" +
                "Vigorous Up: Multiplies attack by 1.15x when your HP is over 100. Adds +100 attack for Blademasters or +50 attack for Gunners if your HP bar is also completely filled.\n\n" +
                "Obscurity\n" +
                "(Recommended for parry/block playstyle) Obscurity Up: Reduces total attack steps to 6 for maximum buff and allows for sharpness recovery at maximum attack buff. Perfect guards go up three attack steps (i.e. 2 perfect guards is maximum attack buff) and recover additional sharpness while maxed. Recovering sharpness with Gunlance while in heat blade mode reduces the sharpness loss on deactivation (20 guard or 5 perfect guards negates all 100 sharpness loss).\nAttack Increase:\nSnS, Lance, GL, Tonfa: 70 / 140 / 210 / 240 / 270 / 300\nGS, Swaxe F, Magnet Spike: 50 / 100 / 150 / 175 / 200 / 225\nLS: 30 / 60 / 90 / 110 / 130 / 150\nSharpness Recovery:\nSnS: 4 / 12\nLance: 2 / 10\nGunlance: 5 / 20\nGS: 5 / 15\nSwaxe F: 4 / 10\nTonfa: 5 / 13\nLS: 5 / 11\nMagnet Spike: 5 / 10\n\n" +
                "Soul\n" +
                "(Recommended for HH) Soul Up: Soul can be applied by using items, attacks will not stagger other players.\nRed Soul: +100 Attack on both user and players struck. Zero stamina consumption for running with weapons unsheathed.\nBlue Soul: +200 Defense on both user and players struck. Health Recovery effects, removes most abnormal status effects (Zenith Blights cannot be removed.)\nThis Skill works with all sources of Soul meaning you can stack Blue Soul with Red Soul from Blazing Grace and buff both. Attack is a final addition that is always the stated value and completely unaffected by other skills and multipliers.\n\n" +
                "Rush\n" +
                "(Recommended) Rush Up: Adds a third stage to Rush that is activated from the 2nd stage by attacking or guarding. Strictly time limited and increases attack further and grants infinite stamina during its duration. Grants +70 true raw (Total +200) and has a duration of 30 seconds. Indicated by an intensified version of the current aura with additional lightning effects. Infinite stamina works with Combat Supremacy and Starving Wolf but does not refill, meaning you would need to activate it before the bar empties to have stamina for evading, etc. Synergizes well with Guard Up, Reflect Up and Obscurity Up.\n\n" +
                "Ceaseless\n" +
                "(Recommended) Ceaseless Up: Adds a third stage to Ceaseless with higher Affinity and additional Critical Multiplier. All stages require less hits to be reached and the skill goes down one level on timing out instead of losing all levels (e.g. stage 2 downgrades to stage 1 instead of no stages). Third stage is indicated by a red ceaseless aura instead of the standard white.\nFirst Phase: +35% Affinity, +0.1x Critical Multiplier\nSecond Phase: +50% Affinity, +0.15x Critical Multiplier\nThird Phase: +60% Affinity, +0.20x Critical Multiplier.\nEach weapon requires a different number of hits to progress in stages.\nSnS: 10s, 15/35/54 hits\nDS: 11s, 12/29/45 hits\nGS: 15s, 7/16/26 hits\nLS: 12, 11/29/47 hits\nHammer: 15s 6/21/36 hits\nHH: 14s, 8/24/40 hits\nLance, GL, Swaxe F: 12s, 10/27/44 hits\nTonfa: 11s, 11/27/43 hits\nMagnet Spike: 12s, 8/23/38 hits\nLBG: 11s, 27/74/120 hits\nHBG: 13s, 21/57/93 hits\nBow: 12s, 12/36/60 hits\nReflect and Stylish Up count towards these hit totals but Fencing+2's extra hit does not. Both affinity and multiplier stack with similar buffs (e.g. Issen+3 and second phase Ceaseless gives +70% Affinity and a multiplier of 1.65x)"
                ;

    public static string GetGameStyleRankInfo => "You can equip your first skill by going into a weapon's Book of Secrets menu and select one of the Special Effect options followed by one of the skills above. After you hit GSR100 you will be able to equip two skills and up until that point you will be able to equip 1.\n\n" +
                "At HR5 you gain the basic Defense Skill, at HR6 you gain all the various Elemental Res skills and the first version of Sharpening Up and at HR7 you get access to Affinity Up and max Sharpening Up. All of the Res and Defense skills progress naturally as you rank up in G Style Rank with some having the requirement of GSR999 in the weapon or multiple weapons to be unlocked or maxed out.\n\n" +
                "Every GSR999\n" +
                "Passive Master\n" +
                "Causes any monster attacks that would normally leave you completely knocked down to be partially recovered from (i.e. you land on your feet instead of lying down and slowly getting up.)\n\n" +
                "Secret Technique\n" +
                "An ability that can be used once a day after 12:00 that deals massive damage after a long wind up animation. Bound to the Kick button or key. Increases attack after use for the rest of that quest's duration.\n\n" +
                "11x GSR999\n" +
                "Soul Revival\n" +
                "An ability that can be triggered once per quest that revives you after hitting 0 HP once and fills your health bar. Disabled with Determination. Unlocked on GSR999 weapons after you have 11 GSR999 total.\n\n" +
                "### Special Effects (G Style Rank)\r\n\r\n|GSR| Effect|\r\n|--|------|\r\n|0|Def+100, Ele Res+20, All Res+10, Affinity+20|\r\n|10|Def+1|\r\n|20|Fire Res+2|\r\n|30|Conquest Def+10|\r\n|40|Water Res+2|\r\n|50|Conquest Atk+2|\r\n|60|Def+1|\r\n|70|Conquest Def+10|\r\n|80|Thunder Res+2|\r\n|90|Def+1|\r\n|100|Conquest Atk+2, 2 special effects can be set|\r\n|110|Def+1|\r\n|120|Ice Res+2|\r\n|130|Conquest Def+10|\r\n|140|Dragon Res+2|\r\n|150|Conquest Atk+2|\r\n|160|Def+1|\r\n|170|Conquest Def+10|\r\n|180|All Res+1|\r\n|190|Def+1|\r\n|200|Conquest Atk+2|\r\n|210|Def+1|\r\n|220|Fire Res+2|\r\n|230|Conquest Def+10|\r\n|240|Water Res+2|\r\n|250|Conquest Atk+2|\r\n|260|Def+1|\r\n|270|Conquest Def+10|\r\n|280|Thunder Res+2|\r\n|290|Def+1|\r\n|300|Conquest Atk+2|\r\n|310|Def+1|\r\n|320|Ice Res+2|\r\n|330|Conquest Def+10|\r\n|340|Dragon Res+2|\r\n|350|Conquest Atk+2|\r\n|360|Def+2|\r\n|370|Conquest Def+10|\r\n|380|All Res+1|\r\n|390|Def+2|\r\n|400|Conquest Atk+2|\r\n|410|Def+2|\r\n|420|Fire Res+2|\r\n|430|Conquest Def+10|\r\n|440|Water Res+2|\r\n|450|Conquest Atk+2|\r\n|460|Def+2|\r\n|470|Conquest Def+10|\r\n|480|Thunder Res+2|\r\n|490|Def+2|\r\n|500|Conquest Atk+2|\r\n|510|Def+2|\r\n|520|Ice Res+2|\r\n|530|Conquest Def+10|\r\n|540|Dragon Res+2|\r\n|550|Conquest Atk+2|\r\n|560|Def+2|\r\n|570|Conquest Def+10|\r\n|580|All Res+1|\r\n|590|Def+2|\r\n|600|Conquest Atk+2|\r\n|610|Def+2|\r\n|620|Fire Res+2|\r\n|630|Conquest Def+10|\r\n|640|Water Res+2|\r\n|650|Conquest Atk+2|\r\n|660|Def+2|\r\n|670|Conquest Def+10|\r\n|680|Thunder Res+2|\r\n|690|Def+2|\r\n|700|Conquest Atk+2|\r\n|710|Def+2|\r\n|720|Ice Res+2|\r\n|730|Conquest Def+10|\r\n|740|Ice Res+2|\r\n|750|Conquest Atk+2|\r\n|760|Def+2|\r\n|770|Conquest Def+10|\r\n|780|All Res+1|\r\n|790|Def+2|\r\n|800|Conquest Atk+4|\r\n|810|Def+2|\r\n|820|Fire Res+2|\r\n|830|Conquest Def+10|\r\n|840|Water Res+2|\r\n|850|Conquest Atk+4|\r\n|860|Def+2|\r\n|870|Conquest Def+10|\r\n|880|Thunder Res+2|\r\n|890|Def+2|\r\n|900|Conquest Atk+4|\r\n|910|Def+2|\r\n|920|Ice Res+2|\r\n|930|Conquest Defense+10|\r\n|940|Dragon Res+2|\r\n|950|Conquest Atk+4|\r\n|960|Def+2|\r\n|970|Conquest Def+10|\r\n|980|All Res+1|\r\n|990|Def+2|\r\n|999|Passive Master, Conquest Atk+4|\r\n|x11 GSR999|Soul Revival, Conquest Atk Base 100, Conquest Def Base 300, G Rank Weapon unlock bonuses for conquest skills (+10 Def, +5 Atk each)|\r\n\r\n### GSR Weapon Unlock Bonus\r\n\r\n|Unlocks| Bonus|\r\n|--|------|\r\n|11|None|\r\n|12|Affinity+2, Ele Res+2, All Res+2, Def+10, Conquest Atk Base 100+5, Conquest Def Base 300+10|\r\n|13|Affinity+2, Ele Res+2, All Res+2, Def+10, Conquest Atk Base 100+5, Conquest Def Base 300+10|\r\n|14|Affinity+2, Ele Res+1, All Res+1, Def+10, Conquest Atk Base 100+5, Conquest Def Base 300+10|";

    public static string GetGameDivaInfo => "Prayer Gems\nRinging Prayer Gem\n" +
                "Adds new items to the GCP store based on level.\n\n" +
                "Elegance Prayer Gem\n" +
                "Adds passive HP recovery to all quests.\n\n" +
                "Heavy Thunder Prayer Gem\n" +
                "Elemental damage increases based on level.\n\n" +
                "Windstorm Prayer Gem\n" +
                "Sharpness does not decrease with blademaster weapons. Works for 5, 10 or 20 quests depending on level during the prayer active window.\n\n" +
                "Cutting Edge Prayer Gem\n" +
                "Increases the amount of raw damage dealt by a cutting weapon by adjusting hitboxes to be weaker against the damage type.\n\n" +
                "Status Length Prayer\n" +
                "Increases the duration of status effects on monsters.\n\n" +
                "Rising Bullet Prayer Gem\n" +
                "Increases the amount of raw damage dealt by a ranged weapon by adjusting hitboxes to be weaker against the damage type.\n\n" +
                "Severing Power Prayer Gem\n" +
                "Tails can be cut with any damage type.\n\n" +
                "Powerful Strikes Prayer Gem\n" +
                "Increases affinity of all weapons based on the level of the song.\n\n" +
                "Protection Prayer Gem\n" +
                "Gives Divine Protection, Goddess' Embrace or Soul Revival based on level.\n\n" +
                "Mobilization Prayer Gem\n" +
                "Attack will go up based on the number of human hunters in a quest.\n\n" +
                "Unshakable Prayer Gem\n" +
                "Monsters cannot flee if in the same area as a hunter.\n\n" +
                "Blunt Prayer Gem\n" +
                "Increases the amount of raw damage dealt by an impact weapon by adjusting hitboxes to be weaker against the damage type.\n\n" +
                "Diva Questline\n" +
                "Unless rank is specified the monsters are any rank. Your partner needs to be PR81 to progress through these quests. Do the special quests for PRP and give it the HRP tickets you get from progressing by choosing the final option followed by the first option on your partner in the smith or your house. If you find yourself unable to progress you probably need to talk to one of the NPCs who gave you the task again (cats etc.). Be sure to also look for monster names in the text if you can't progress after killing one, you might be on a lower step than you thought. Complete Chapter 3 to unlock Diva Song.\n\n" +
                "Chapter 1\n" +
                "Part 1: Deliver 1 Thin Jack Mackerel (薄竹筴魚). Deliver 1 Lazurite Jewel (琉璃原珠)\n\n" +
                "Part 2: Hunt 1 White Monoblos. Return to the Diva Hall\n\n" +
                "Part 3: Talk to the Guild Mistress. Hunt 1 Yama Tsukami. Talk to the Legendary Rasta Edward (Lance User)\n\n" +
                "Part 4: Talk to the Guild Mistress. Hunt 1 Chameleos. Talk to the Legendary Rasta Edward\n\n" +
                "Part 5: Talk to the Guild Mistress. Hunt 1 Yama Tsukami. Return to the Diva Hall. Claim the items you need to deliver from the Hunter Challenge, you don't need to farm a million Kelbi.\n\n" +
                "Rewards: Diva Armour Materials. Items to deliver in Chapter 2 (Hunter Challenge Reward)\n\n" +
                "Chapter 2\n" +
                "Part 1: Deliver 30 Kelbi Horns (精靈鹿的角). Deliver 20 Chaos Shrooms (混沌茸). Deliver 5 Kirin Azure Horns (麒麟的蒼角). Items delivered above are returned to you\n\n" +
                "Part 2: Hunt 3 Cephadromes. Deliver 10 Dragon Seeds (屠龍果實). Hunt 2 Lao Shan Lungs. Return to the Diva Hall. Talk to the Legendary Rastas Edward and Frau (DS user)\n\n" +
                "Part 3: Talk to the Legendary Rasta Frau. Return to the Diva Hall. Hunt 1 Baruragaru. Return to the Diva Hall. Talk to the Legendary Rasta Frau. Return to the Diva Hall\n\n" +
                "Rewards: Diva HC Armour Materials" +
                "Chapter 3\n" +
                "Part 1: Hunt 1 Teostra. Return to the Diva Hall. Talk to the Legendary Rasta Frau\n\n" +
                "Part 2: Go to the Blacksmith. Return to the Diva Hall. Hunt 3 Rukodioras\n\n" +
                "Part 3: Hunt 1 Anorupatisu\n\n" +
                "Part 4: Hunt 1 Rebidiora\n\n" +
                "Rewards: Diva G Rank Weapon Materials. Diva Weapon Gem (1st Series). 5 Diva Song Gems (Hunter Challenge Reward). 5 Warm Honey Tea (Give the Diva as a gift then Hunter Challenge Reward). Completion of this Chapter unlocks the Diva Song Buffs. Cram her full of warm honey tea and fluffy cakes to max its effects. Your Discord RPC shows the current Bond when you are in the Diva Hall, the maximum is 999.\n\n" +
                "Chapter 4\n" +
                "Part 1: Hunt 1 Berukyurosu. Hunt 1 Doragyurosu\n\n" +
                "Part 2: Deliver 1 Saint Ore (純聖礦石). Hunt 1 Hyujikiki. Hunt 1 Giaorugu\n\n" +
                "Part 3: Speak to the Town Square Cats three times. Hunt 2 Gougarfs\n\n" +
                "Part 4: Talk to NPC in Blacksmith. Solo Hunt 1 Gurenzeburu\n\n" +
                "Part 5: Talk to Guild Mistress. Hunt 1 Pokaradon. Hunt 1 Midogaron. Talk to NPC next to Guild Hall entrance\n\n" +
                "Rewards: Diva HC Armour Materials\n\n" +
                "Chapter 5\n" +
                "Part 1: Hunt 1 Farunokku\n\n" +
                "Part 2: Hunt 2 Baruragaru (Return to Fountain between the two)\n\n" +
                "Part 3: Hunt 1 Rebidiora\n\n" +
                "Part 4: Hunt 1 Zerureusu\n\n" +
                "Rewards: Diva Weapon Gem (1st Series)\n\n" +
                "Chapter 6\n" +
                "Part 1: Hunt 1 Akantor\n\n" +
                "Part 2: Hire a partner if you don't have one and then talk to them in your house. Return to the Diva Hall. Talk to partner in house, return to Diva Hall.\n\n" +
                "Part 3: ※Partner must be at least PR31 to proceed. Hunt 1 G Rank Yian Kut-ku with partner present.\n\n" +
                "Part 4: ※Partner must be at least PR51 to proceed. Hunt 1 Pokaradon with partner present.\n\n" +
                "Part 5: ※Partner must be at least PR81 to proceed. Hunt 1 Midogaron with partner present. (Talk to partner in house and return to Diva Hall before leaving on quest)\n\n" +
                "Rewards: Diva Armour Materials\n\n" +
                "Chapter 7\n" +
                "Part 1: Talk to Blacksmith and return to Diva Hall. Hunt 1 Rebidiora\n\n" +
                "Part 2: Hunt 2 G Rank HC Gurenzeburu (Return to Fountain between the two)\n\n" +
                "Part 3: Hunt 1 Taikun Zamuza\n\n" +
                "Part 4: Hunt 1 Meraginasu\n\n" +
                "Rewards: Diva Weapon Gem (1st Series)\n\n" +
                "Chapter 8\n" +
                "Part 1: Speak to Blacksmith and return to Diva Hall\n\n" +
                "Part 2: Deliver 3 Grease Stone (白鳥石) and 1 Atarka Ore (亞達爾純礦石). You can mine the ores in the G Rank Flower Field or simply buy them for 235 GCP total. Hunt 1 Forokururu\n\n" +
                "Part 3: You need to craft the Prototype Tonfas at this point. Kill 3 Aptonoth in the preset quest\n\n" +
                "Part 4: Hunt 1 Yian Kut-Ku (Does not need to be with Tonfas)\n\n" +
                "Rewards: Ores spent in part 2 (Hunter Challenge Reward). Used to be ability to craft Tonfas.\n\n" +
                "Chapter 9\n" +
                "Part 1: Deliver 1 Teostra Miracle Wing (Supremacy Teo)\n\n" +
                "Part 2: Hunt 2 G Rank Velocidrome\n\n" +
                "Part 3: Hunt 1 Meraginasu\n\n" +
                "Part 4: Speak to Gin (Hammer Rasta)\n\n" +
                "Rewards: Diva G Rank Armour Materials\n\n" +
                "Chapter 10\n" +
                "Part 1: Talk to Guild Master. Hunt 1 Monoblos\n\n" +
                "Part 2: Hunt 1 Gou Lunastra\n\n" +
                "Part 3: Speak to the Guild Mistress\n\n" +
                "Part 4: Hunt 1 Anorupatisu (Preset Quest). ※ Everyone must use Tonfas for this mission . (Restricted equipment disables AI outside of Legendaries)\n\n" +
                "Rewards: Diva Weapon Materials (1st Series) (2 Gems with Hunter Challenge Reward)\n\n" +
                "Chapter 11\n\n" +
                "Part 1: Talk to Guild Mistress, Return to Diva Hall\n\n" +
                "Part 2: Capture 1 Forokururu\n\n" +
                "Part 3: Speak to Leila (Tonfa Legendary). Solo Hunt 1 Diorex. Speak to Leila. Return to the Diva Hall\n\n" +
                "Part 4: Hunt 1 Burst Species Meraginasu\n\n" +
                "Rewards: Diva Armour Materials\n\n" +
                "Chapter 12\n" +
                "Part 1: Hunt 1 G Rank Gold Rathian, talk to cats and return to Diva Hall\n\n" +
                "Part 2: Speak to Leila and return to the Diva Hall\n\n" +
                "Part 3: Hunt 1 Inagami\n\n" +
                "Part 4: Hunt 1 G Rank Inagami (Preset quest with set equipment, AI outside of Legendaries is disabled)\n\n" +
                "Rewards: Diva Weapon Gem (2nd Series)\n\n" +
                "Chapter 13\n" +
                "Part 1: Hunt 1 Giaorugu\n\n" +
                "Part 2: Hunt 1 G Rank Gravios\n\n" +
                "Part 3: Speak to Leila and return to the Diva Hall. Speak to the Blacksmith\n\n" +
                "Part 4: Hunt 1 G Rank Forokururu. Hunt 1 G Rank HC Rajang\n\n" +
                "Rewards: Diva HC Armour Materials\n\n" +
                "Chapter 14\n" +
                "Part 1: Hunt 1 Red Lavasioth (Training Quest on Black Quest NPC)\n\n" +
                "Part 2: Speak to Flora (SnS Legendary) and return to Diva Hall. Hunt 1 Hyujikiki\n\n" +
                "Part 3: Hunt 1 Inagami\n\n" +
                "Part 4: Deliver 3 Herbal Medicine G (中藥G). (Can be bought in Guild Hall for Guild Tix)\n\n" +
                "Rewards: Diva Weapon Gem (2nd Series)\n\n" +
                "Chapter 15\n" +
                "Part 1: Talk to Guild Master. Return to fountain. Hunt 1 G Rank White Espinas.\n\n" +
                "Part 2: Hunt 1 G Rank Baruragaru\n\n" +
                "Part 3: Hunt 1 G Rank Akura Jebia\n\n" +
                "Part 4: Hunt 1 Burst (G Rank) Garuba Daora\n\n" +
                "Rewards: Diva Armour Materials. Diva Weapon Gem (2nd Series)"
                ;

    public static string GetGameGuildFood => "Guild Cooking\r\nGuild cooking is available at guild rank 15, it is a mini-game performed by up to four people.\r\nCooking has the following menu items, which can activate effects separate from armor skills.\r\n■The skill effect gained by cooking lasts for 90 minutes. However, if a new skill is obtained by cooking, the effect is overwritten.\r\n■Up to 6 dishes can be \"left over\". The leftover dishes will be stored for 1 hour.\r\nHow to cook\r\n[Select the menu]\r\n\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000※Note, however, that the selections here are only a guide for selecting ingredients.\r\n[Selecting] yellow ingredients are \"base ingredients\" and pink ingredients are \"Auxiliary\". These allow you to create dishes via the menu.\r\n\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000※If you select any other option, it will be a Guild's Yaminabe.\r\n[How to cook] By repeatedly pressing the confirm button, the cursor on the gauge will move to the right.\r\n\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000By hovering the cursor over the green \"Success Area\" or blue \"Great Success Area,\" a stamp will accumulate directly below the food meter.\r\nWhen the color of the stamp turns green, the dish is a success, and when it turns blue, it is a great success.\r\n\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000※The more people that participate, the higher the chance the dish will be a great success.\r\nChef Cat's Wisdom\r\nSecret of success\t10 tickets\tGreen area expanded.\r\nNo failure allowed\t10 tickets\tRed areas removed\r\nUltimate Success\t10 tickets\tBlue area gets doubled.\r\nCooking Technique\t10 tickets\tCursors return speed is slower\r\nSecret seasoning\t20 tickets\tResult will be one rank higher.\r\nMystery seasoning\t5 tickets\tResult is randomized\n\nThe order of the skills are G.Failure/Failure/Success/G.Success\n\n" +
                "Easiest way to cook is selecting Secret Seasoning from Chef's Wisdom before cooking. What it does is raise the success level by one. \n\n" +
                "Page 1\n" +
                "1: Explosive Rice [Snow Powder, Whole Vanilla, Wabisabi Wasabi, Deep Sea Chub] (Hunger Increased [Lg] / Health+30 / Rage+1 / Rage+2)\n\n" +
                "2: Pioneer's Meal [Goemon Frog, Rainbow Mint, Onion Sticks, Mimic Vines] (Hunger Increased [Lg] / Health +30 / Three Worlds Protection +2 / Three Worlds Protection +3)\n\n" +
                "2: Pioneer's Meal [Dangerous Melon, Demon Pepper, Onion Sticks, Mimic Vines] (Hunger Increased [Lg], Wind Pressure [Sm], Wind Pressure [Lg], Wind Pressure [Dragon])\n\n" +
                "3: Lucky Pancake [Star Pineapple, Dangerous Melon, Whole Vanilla, Mimic Vines] (Hunger Increased [Lg] / Health +10 / Good Luck / Great Luck)\n\n" +
                "4: Ultimate Sashimi [Deep Sea Chub, Blk and Wht Dragonfly, Ice Salmon, Whole Vanilla] (Hunger Increased [Lg], Earplugs, High Grade Earplugs, Super Ear Plugs)\n\n" +
                "5: Brother's BBQ [Monochrome Mushroom, Deep Sea Chub, Ice Salmon, Mimic Vines] (Hunger Increased [Lg], Caring +1, Caring +2, Caring +3)\n\n" +
                "6: Hot Claw Feast [Lava King Crab, Whole Vanilla, Dangerous Melon, Mimic Vines] (Hunger Increased [Lg], Health +20, Adrenaline +1, Adrenaline +2)\n\n" +
                "7: Medicinal Spirit [Ancient Algae, Dangerous Melon, Deep Sea Chub, Demon Pepper] (Hunger Increased [Lg], Wind Pressure [Sm], Wind Pressure [Lg], Wind Pressure [Dragon])\n\n" +
                "7: Medicinal Spirit [Rainbow Mint, Dangerous Melon, Deep Sea Chub, Demon Pepper] (Hunger Increased [Lg], Wind Pressure [Sm], Wind Pressure [Dragon], Violent Wind Breaker)\n\n" +
                "Page 2\n" +
                "8: World Fried Rice [Acid Pepper, Mimic Vines, Onion Sticks, Blk and Wht Dragonfly] (Hunger Increased [Lg], Health +20, Runner +1, Runner +2)\n\n" +
                "9: Goddess Dessert [Snow Kiwi, Dangerous Melon, Whole Vanilla, Magma Mango] (Hunger Increased [Lg], Health +20, Divine Protection, Goddess' Embrace)\n\n" +
                "10: Grilled Shellfish [Large Blly Shlfish, Ice Salmon, Whole Vanilla, Mimic Vines] (Hunger Increased [Lg], Health +20, Hunger Halved, Hunger Negated)\n\n" +
                "(Recommended) 11: Holy Seafood Banquet [Deep Sea Chub, Ice Salmon, Whole Vanilla, Mimic Vines] (Hunger Increased [Lg], Wide-Area +1, Wide-Area +2, Wide-Area +3)\n\n" +
                "(Recommended) 12: Energy Noodles [Mimic Vines, Onion Sticks, Blk and Wht Dragonfly, Whole Vanilla] (Hunger Increased [Lg], Health +10, Terrain [Sm], Terrain [Lg])\n\n" +
                "13: Hunter's Whim [Ice Salmon, Dangerous Melon, Mimic Vines, Deep Sea Chub] (Hunger Increased [Lg], Health +10, Whim, Divine Whim)\n\n" +
                "14: Fantasy Dumplings [Dangerous Melon, Magma Mango, Whole Vanilla, Mimic Vines] (Hunger Increased [Lg], Health +10, Paralysis Halved, Paralysis Negated)\n\n" +
                "Page 3\n" +
                "15: Crimson BBQ [Demon Pepper, Dangerous Melon, Magma Mango, Mimic Vines] (Hunger Increased [Lg], Health +10, Sleep Halved, Sleep Negated)\n\n" +
                "16: Sweet Wrap [Magma Mango, Star Pineapple, Dangerous Melon, Mimic Vines] (Hunger Increased [Lg], Health +10, Poison Halved, Poison Negated)\n\n" +
                "17: Dawn Toast [Whole Vanilla, Magma Mango, Dangerous Melon, Mimic Vines] (Hunger Increased [Lg], Health +10, Stun Halved, Stun Negated)\n\n" +
                "(Recommended) 18: Rainbow Soup [Onion Sticks, Snow Powder, Blk and Wht Dragonfly, Bright Grain] (Hunger Increased [Lg], All Res UP +5, All Res UP +10, All Res UP +20)\n\n" +
                "18: Rainbow Soup [Onion Sticks, Magma Mango, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Fire Res +10, Fire Res +20, Fire Res +30)\n\n" +
                "18: Rainbow Soup [Onion Sticks, Dangerous Melon, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Water Res +10, Water Res +20, Water Res +30)\n\n" +
                "18: Rainbow Soup [Onion Sticks, Ice Salmon, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Ice Res +10, Ice Res +20, Ice Res +30)\n\n" +
                "18: Rainbow Soup [Onion Sticks, Demon Pepper, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Thunder Res +10, Thunder Res +20, Thunder Res +30)\n\n" +
                "18: Rainbow Soup [Onion Sticks, Mimic Vines, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Dragon Res +10, Dragon Res +20, Dragon Res +30)\n\n" +
                "(Recommended) 19: Vigor Salad [Taiko Olive, Miracle Herb, Ancient Algae, Whole Vanilla] (Hunger Increased [Lg], Health +30, Wide-Area +3, Herbal Medicine)\n\n" +
                "(Recommended) 20: Hearty Pie [Gutsy Meat, Onion Sticks, Bright Grain, Snow Powder] (Hunger Increased [Lg], Health +30, Encourage +1, Encourage +2)\n\n" +
                "21: Unity Buns [Taiko Olive, Bright Grain, Blk and Wht Dragonfly, Mimic Vines] (Hunger Increased [Lg], Health +30, Bond, Assistance)\n\n" +
                "Page 4\n" +
                "(Recommended) 22: Blast Steak [Gutsy Meat, Magma Mango, Snow Kiwi, Star Pineapple] (Blue Soul, Blue Soul, Incitement, Red Soul)\n\n" +
                "0: Guild's Yaminabe [Any, Any, Any, Any] (Hunger Increased [Lg], Random, Random, Random)";

    public static string GetGameSnSInfo => "This weapon is for certain far more easier to operate than Dual Swords, it however also has notoriously long hitlag effects. It is also arguably the highest benefactor of Determination's effects.\n" +
                "It has some of the best paralysis weapons in the game, playing a large part in multiplayer road runs.\n" +
                "Hiden Skill Effects\nSnS obtains a larger 1.30x raw modifier rather than the usual 1.20x Blademaster weapons usually get. It can also use all elemental and status sword crystals, and has slightly faster movement speed.\nNotes\nThe slide attack has a few i-frames (~9 with E+2) and can do a 180° turning slash at the end.\r\nLong length is generally a flat upgrade with its reach. Very Long is usable if you're just going to spam roundslashes but may want some spacing management.\nSnS has a very lenient hit requirement to ready the Transcend burst attack.\nYou can technically manipulate a transcend burst to any element you want by using the sword crystal compatibility from the hiden skill.\r\nShield attacks do impact damage and can stun, but they use white sharpness values at best.\r\nThe vacuum slash has no hitlag or sharpness cost on the extra hit it adds to the roundslash. It isn't as a big deal today, but can be nice to have.\r\nUsing items unsheathed resets Rush.\r\nOf note, some time attack players have had success with dropping Fencing +2 for the sake of reducing hitlag.\r\nSnS's blocking isn't good, but the Perfect Defense caravan skill can technically be used with Obscurity Up to amass 300 raw in two blocks cleanly if you have good timing, keeping it until you take knockback.\r\nGalatine Z is the best overall SnS in the game when poison is not necessary.\n" +
                "Galatine is also a good weapon for Road. Outside of Road, you can use Blue Tower.\n\n" +
                "SnS Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nSlide Attack\tMotion value change (21 > 31)\t\t\t\r\nShield Attacks\tMotion value changes, but doesn't influence Extreme style attacks\t\t\t\r\nInfinite Slash\tMotion value change (25 > 28)\t\t\t\n\n" +
                "Skill Recommendations\t\r\nSkill\tNotes / Reasoning\r\nCore\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot on JP if a hiden cuff is used.\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback.\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\r\nEvasion +2\tGreatly increases survivability in Frontier, also triggers evasion-reliant skills much more easily. Obtain it through Drawing Arts +2, Evasion Boost, Encourage, or wearing a piece of Nargacuga armor.\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\r\nRecommended\t\r\nDrawing Arts +2\tA compound skill that is perhaps the best Evasion skill for SnS with its stamina benefits, constant evasion can surprisingly drain it quickly.\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. Use the Z skill to boost raw gains further.\r\nStylish (Up)\tSharpness restoration on avoiding attacks.\r\nRush (Up)\tGood source of raw if you stay unsheathed and do not use any items. Cannot get the Z skill on TW.\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW.\r\nAbnormality\tStellar compound skill when paired with poison. Also makes paralysis incredibly consistent for SnS.\r\nOther Skills\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\r\nAdrenaline\tPopular in Frontier due to the availability of -60 HP food, Evasion +2, and overall more dangerous but predictable monsters. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\r\nLavish Attack\tProvides +100 true raw at the expense of sharpness. Is often paired with SnS's roundslash attack and Stylish.\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\r\nIce Age (Up)\tGrants a DOT AoE around your character, pairs great with poison and grants other benefits. Useless Z skill.\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill.\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\r\nObscurity (Up)\tViable with a combination of Obscurity Up and the Perfect Guard caravan skill. However, this needs good timing and the boost is lost on knockback, but if executed well you can get +300 true raw on two perfect guards.\r\nElement Attack Up\tCan be potent, but the means of obtaining this skill are rare (the most reliable are hiden decorations and premium gear).\r\nEncourage (Up)\tThe user gets Horn Maestro and also grants party-wide Evasion +2 and KO immunity. The Z skill confers party-wide stamina buffs. You could run it on Road SnS but it will dig into your set potential significantly.\r\nNot Recommended\t\r\nCombat Supremacy\tToo difficult for SnS to keep up.\r\nDissolver (Up)\tVery little actual payoff. Obsolete with Determination.\r\nGuard skills\tReflect, Fortification +2/Guard +2, etc. You are not a lance.";

    public static string GetGameDSInfo => "One of the best weapons in the game, but with a higher skill ceiling due to its unique sharpen motion. Compatibility with Vampirism has made it drastically more accessible to pick up, however.\r\nDuals have some of the fastest clear times against musous, competing with Tonfa.\r\nHiden Skill Effects\r\nAttacks in any Demon mode restore 3 stamina per hit. Compatible with Combat Supremacy.\r\nNotes\r\nLike SnS, long length is pretty much an upgrade and often a common find on modern weapons.\r\nToday, there is no reason to use any demonization that isn't Extreme Demon Mode. However, you cannot receive any healing other than from Vampirism in this state.\r\nEach sharpen in a combo adds 1.05x raw, stackable four times for up to a 1.20x raw bonus. The attacking evades in True/Extreme Demon modes continue this combo state.\n" +
                "You can't receive any healing when demonized, Vampirism leeching is the only exception. This is made much more apparent on road.\n" +
                "Dainsleif Z is the best DS in the game.\n" +
                "Sharpen 4 times to gain maximum buff. Don't drop the combo.\n\n" +
                "DS Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nFront Flip Slash\tMotion value change (11-15 > 16-22)\t\t\t\r\nRush Slash\tMotion value change (11-5 > 16-7) (14-6 > 21-9 demonized)\t\t\t\r\nSharpening Technique\tThe first sharpen within a combo counts as two sharpens for the multiplier\t\t\t\r\nMisc\t\t\t\t\r\nDS Length Up\tLength raised by one step at the cost of -25 raw, can stack with no extra raw loss\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nDrawing Arts +2\tBest Evasion +2 skill for DS due to the stamina benefits. Evasion Boost is sometimes used in time attack for consistency.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tLeech back HP even when demonized. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. Use the Z skill to boost raw gains further.\t\t\t\t\t\t\t\t\r\nStylish (Up)\tSharpness restoration on avoiding attacks.\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nAbnormality\tStellar compound skill when paired with poison.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\t\t\t\t\t\t\t\t\r\nAdrenaline\tEspecially popular on DS. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nLavish Attack\tProvides +100 true raw at the expense of sharpness. It can work well with Stylish Up on harder fights that require constant evasion.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\t\t\t\t\t\t\t\t\r\nIce Age (Up)\tGrants a DOT AoE around your character, pairs great with poison and grants other benefits. Useless Z skill.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\r\nElement Attack Up\tCan be potent, but the means of obtaining this skill are rare (the most reliable is premium gear).\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tDifficult to maintain as easily with Extreme Demon Mode.\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tIt can technically be paired effectively with Rush Up on JP because DS Hiden restores stamina on every hit.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nDissolver (Up)\tVery little actual payoff. Obsolete with Determination.\t\t\t\t\t\t\t\t";

    public static string GetGameGSInfo => "A weapon balanced around premium gear and Raviente, with standard content only getting harder for it. Still, it can hit really hard, but it's a matter of when it's allowed to do so.\r\nIronically the hardest weapon to play properly on Raviente, but a good GS player makes a world of difference.\r\nHiden Skill Effects\r\nFaster charging that stacks with Focus, and a wider window before the charged upswing attack overcharges down to Lv2. Timed parries can recover some sharpness, being a net gain with Sharp Sword +2.\r\nOther effects for non-Extreme styles have been truncated.\r\nNotes\r\nYou are a friendly fire liability.\r\nHold R1 before unsheathing to do a regular overhead attack, as you'll otherwise do a special wide slash (although it does combo into any charged slash). You can also do upswing charges and parries (Storm, Extreme) from a sheathed state.\r\nVery Long length is the most popular due to the most-used attacks being vertical slices and overall more leniency with keeping a distance. Long is plenty usable, though.\r\nCombat Supremacy is the Frontier equivalent of Critical Draw on GS, pairing very well with hit & run strategies with the stamina drain and 1.20x raw increase. Starving Wolf +2 w/ Rush is an alternative for unsheathed play.\r\nGS gets the best mileage out of Raviente power crystals. They do not work on the shining attack however, but at least they don't get used up.\r\nThe unsheathed running hop attack has a few i-frames at the start, but it's rarely used.\r\nWell-timed parries with GS Hiden and Obscurity Up greatly manage sharpness when using Lavish Attack.\r\nLarge amounts of hitstop is a valid way of avoiding some attacks.\n" +
                "If you have 30 power crystals with a Raviente GS and if the hunt isn't over by the time they're all used up, you are doing something wrong.\n" +
                "Z100 element is technically the best option, but needs good sigils + 1x GS Length Up for best results\n\n" +
                "GS Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nUpswing\tMotion value change (56 > 72), does not benefit charged upswings\t\t\t\r\nRotation Slash\tMotion value change (56 > 72)\t\t\t\r\nGuard Slash\tHeaven/Storm guard slash can block an extra attack, stacks up to two times\t\t\t\r\nMisc\t\t\t\t\r\nGS Length Up\tLength raised by one step at the cost of -25 raw, can stack with no extra raw loss\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nDrawing Arts +2\tThe stamina boosts make this source of Evasion an excellent pairing with Combat Supremacy playstyle and pairs\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nFocus/Trained +2\tFaster charging for the weapon's strongest attack. Can be acquired with Gore Magala armor, but more reliably obtained with premium gear. Trained only exists on JP.\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tBenefits from the GS hit & run playstyle.\t\t\t\t\t\t\t\t\r\nCharge Attack UP +2\tConfers large damage boosts towards the weapon's hardest hitting attacks. Also includes shining sword attack.\t\t\t\t\t\t\t\t\r\nObscurity (Up)\tCan get up to +225 true raw by parrying. The Z skill boost reduces the amount of steps required and enables sharpness recovery at max buff.\t\t\t\t\t\t\t\t\r\nLavish Attack\tProvides +100 true raw at the expense of sharpness. The higher sharpness cost can be mitigated with GS Hiden and Obscurity Up.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\t\t\t\t\t\t\t\t\r\nPerfect Guard\tTimed blocks proc a 72 MV Reflect attack and can immediately be rolled out of.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nGuard/Fortification +2\tAllows parrying to block more kinds of attacks. You can get Guard +2 with Uragaan armor. Guard Up has little discernible effect on parry weapons.\t\t\t\t\t\t\t\t\r\nCaring +3 / Trained +2\tDisables friendly fire through either direction, notably because the upswing charges are very disruptive. Can be obtained as a guild food skill. Trained only exists on JP.\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tWhile harder to implement, you can definitely invoke it in the GS playstyle.\t\t\t\t\t\t\t\t\r\nRush (Up)\tNot as good on GS with sheathing involved. Still, on parries it's a free +50 true raw. You could use this with Starving Wolf for an alternative to Combat Supremacy.\t\t\t\t\t\t\t\t\r\nStarving Wolf +2\tGrants +0.10x to the crit multiplier, +50% affinity and Evasion +2 if you are at 25 Stamina and do not have infinite stamina buffs.\t\t\t\t\t\t\t\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\t\t\t\t\t\t\t\t\r\nAdrenaline\tPopular in Frontier due to the availability of -60 HP food, Evasion +2, and overall more dangerous but predictable monsters. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\r\nStylish (Up)\tYou're a GS.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tNot a good pairing, GS has issues with sustaining the buff even with shining.\t\t\t\t\t\t\t\t\r\nReflect +3\tGenerally not worth how hard it is to get alongside everything else. Try the Perfect Guard caravan skill instead.\t\t\t\t\t\t\t\t\r\nAbsolute Defense\tTakes priority over guarding.\t\t\t\t\t\t\t\t";

    public static string GetGameLSInfo => "Building off the 2nd gen iteration, Frontier's evolutionary path combines numerous anime tropes into one effective, but chuuni package. Strangely, it actually takes effort to play effectively in Frontier.\t\t\t\t\t\t\t\t\t\r\nLS is comparable to Swaxe in power, it arguably has a higher skill ceiling to use effectively but does not rely on parrying to sustain its moveset.\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\r\nSpirit meter consumption is halved, and the glowing meter attack buff is increased from 1.15x to 1.25x.\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\r\nThere is no spirit level mechanic like MH3 and newer. Fill up the bar completely to get a temporary raw bonus.\t\t\t\t\t\t\t\t\t\r\nLength is largely up to taste. Keep in mind LS has multiple kinds of slashes that go in all kinds of directions.\t\t\t\t\t\t\t\t\t\r\nLearning to use the Evasion Slash properly is good practice, and it can also be used to reset combos. The i-frames start at the beginning but don't last too long.\t\t\t\t\t\t\t\t\t\r\nPiercing stabs from parries are powerful, but require good positioning to hit with them.\t\t\t\t\t\t\t\t\t\r\nYou can do an upswing from rolling. Parry from sheathe has a different button combination (Triangle + Circle) than other weapons due to unsheahe > spirit slash.\t\t\t\t\t\t\t\t\t\r\nThe orange/white bar below the spirit meter builds up a stronger multi-hit attack that can be done halfway filled or when maxed out. It otherwise depletes when you sheathe or are forced into doing so (ie tremors).\t\t\t\t\t\t\t\t\t\n" +
                "LS receives the smallest raw boost from Obscurity, but it's still sizeable.\t\t\t\t\t\t\t\t\t\r\nIf you really want a Fade Slash back, just play Switch Axe instead.\t\t\t\t\t\t\t\t\t\n\n" +
                "Go Blue Tenrou for Poison, otherwise Raviente. Murakumo Z / Kusanagi-No-Tsurugi (NetCafe Premium) is also good.\n\n" +
                "LS Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nPiercing Stab\t\"Uncharged (24-12x2 > 32-16x2), Charged (24-12x5-30 > 32-16x5-40) MV changes\r\nDoes not benefit piercing stabs via parrying\"\t\t\t\r\nUpswing\tMotion value change (28 > 42)\t\t\t\r\nRetreating Sword\tEarth Fade Slash obtains i-frames\t\t\t\r\nBlink\tExtends teleport distance\t\t\t\r\nMisc\t\t\t\t\r\nLS Draw Strength\tUnsheathe attacks boosted by x1.50\t\t\t\r\nLS Length Up\tLength raised by one step at the cost of -25 raw, can stack with no extra raw loss\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nEvasion +2\tGreatly increases survivability in Frontier, also triggers evasion-reliant skills much more easily. Obtain it through Drawing Arts +2, Evasion Boost, Encourage, or wearing a piece of Nargacuga armor.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. Use the Z skill to boost raw gains further.\t\t\t\t\t\t\t\t\r\nStylish (Up)\tSharpness restoration on avoiding attacks.\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nObscurity (Up)\tCan get up to +150 true raw by parrying. The Z skill boost provides a different means of restoring sharpness.\t\t\t\t\t\t\t\t\r\nAbnormality\tGood compound skill when paired with poison.\t\t\t\t\t\t\t\t\r\nPerfect Guard\tTimed blocks do not take chip damage, proc a 72 MV Reflect attack, and can immediately be rolled out of.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nGuard/Fortification +2\tAllows parrying to block more kinds of attacks. You can get Guard +2 with Uragaan armor. Guard Up has little discernible effect on parry weapons.\t\t\t\t\t\t\t\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\t\t\t\t\t\t\t\t\r\nAdrenaline\tPopular in Frontier due to the availability of -60 HP food, Evasion +2, and overall more dangerous but predictable monsters. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\t\t\t\t\t\t\t\t\r\nIce Age (Up)\tGrants a DOT AoE around your character, pairs great with poison and grants other benefits. Useless Z skill.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\r\nStarving Wolf +2\tGrants +0.10x to the crit multiplier, +50% affinity and Evasion +2 if you are at 25 Stamina and do not have infinite stamina buffs.\t\t\t\t\t\t\t\t\r\nFocus/Trained +2\tFaster charging and meter gain, but generally not that useful after obtaining hiden. Can be acquired with Gore Magala armor, but more reliably obtained with premium gear. Trained only exists on JP.\t\t\t\t\t\t\t\t\r\nCharge Attack UP +2\tCan be used in charged thrust sets but is otherwise niche.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nAbsolute Defense\tTakes priority over guarding.\t\t\t\t\t\t\t\t";

    public static string GetGameHAInfo => "Being much more focused on charged attacks in this game, Hammer sadly shares similar premium and Raviente problems as GS. It still has the best stun, but is also upstaged by Magspike in damage.\t\t\t\t\t\t\t\t\t\r\nHammer had been largely phased out by MS in the Ravi meta, but the reality is that it's much more consistent.\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\r\nWell-timed charges add an extra 1.30x raw bonus; the difference can be briefly seen on the guild card. This bonus does apply to every hit on the Lv3 infinite swing until the combo is over (the baseball swing finisher counts as part of the combo).\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\r\nGolfswings are effectively obsolete, you're best charging an attack. Focus +2 is highly recommended.\t\t\t\t\t\t\t\t\t\r\nLength on Hammer is largely up to taste. Lv4-Lv5 charge attacks are pretty much all vertical so the moved hitbox isn't often a problem and can outright reach some hitzones on different monsters, but the Lv3 infinite is less cumbersome with shorter lengths.\t\t\t\t\t\t\t\t\t\r\nThree Worlds Protection is very useful to create openings. Try using Zenith cuffs for specific resists, depending on the hunt.\t\t\t\t\t\t\t\t\t\r\nUnlike mainline, every source of additional stun/KO damage you can grab is recommended (Caravan Skill + Stun Sigil) as they stack and contribute a lot of stun damage.\t\t\t\t\t\t\t\t\t\r\nBecause of the multiplied raw from timed charges with hiden, you should get an appropriate of overhead with attack ceiling from My Missions.\t\t\t\t\t\t\t\t\t\r\nYou can technically dodge some very low profile attacks with the leap from the Lv5 charge.\t\t\t\t\t\t\t\t\t\n\n" +
                "Blue Tower for Poison, Z100 otherwise.\n\n" +
                "HA Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nBaseball Swing\tMotion value change (100 > 130)\t\t\t\r\nUnsheathe Upswing\tMotion value change (32 > 48)\t\t\t\r\nCharging Movement\tNon-running charges have higher base movement speed, stacks three times\t\t\t\r\nMisc\t\t\t\t\r\nStun Value\tNot Hammer-specific, but important\t\t\t\r\nHA Length Up\tLength raised by one step at the cost of -25 raw, can stack with no extra raw loss\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nFocus/Trained +2\tIncreases the charging rate for the weapon's most heavily used attacks. Can be acquired with Gore Magala armor, but more reliably obtained with premium gear. Trained only exists on JP.\t\t\t\t\t\t\t\t\r\nEvasion +2\tGreatly increases survivability in Frontier, also triggers evasion-reliant skills much more easily. Obtain it through Drawing Arts +2, Evasion Boost, Encourage, or wearing a piece of Nargacuga armor.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nUnaffected +3\tThis catch-all hearing/tremor/wind resist skill provides Hammer the extra opportunities it needs to deal damage with. Zenith resists are recommended.\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. Use the Z skill to boost raw gains further.\t\t\t\t\t\t\t\t\r\nStylish (Up)\tSharpness restoration on avoiding attacks. You may not feel the need to carry the Z skill.\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nCharge Attack UP +2\tPowers up the weapon's strongest attacks.\t\t\t\t\t\t\t\t\r\nKO Technique\tBoosts stun damage. Stack it with the sigil effect.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nAdrenaline\tPopular in Frontier due to the availability of -60 HP food, Evasion +2, and overall more dangerous but predictable monsters. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tMainly only used in Raviente hunts as the stamina drain is mostly disruptive to Extreme Hammer play.\t\t\t\t\t\t\t\t";

    public static string GetGameHHInfo => "The buffs are extremely powerful, but the actual weapon isn't. It does however have the benefit of being a free ticket to Raviente trains, due to the buffs being a large centerpiece for teams and also contributing KO damage.\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\r\nNotes come out 34% faster.\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\r\nThe weapon uses a tweaked version of recital mode from Monster Hunter 2. The biggest difference is that notes are retained until you encore, play a debuff melody, or sheathe.\t\t\t\t\t\t\t\t\t\r\nAlways take up Encourage +2 for Horn Maestro and party-wide Evasion +2 . Encourage Up grants party-wide stamina benefits if you have it.\t\t\t\t\t\t\t\t\t\r\nEvery HH in the game has Attack Up Large, by virtue of Storm style's pink notes.\t\t\t\t\t\t\t\t\t\r\nChaining attacks into recitals plays the note faster than usual. If you were to start a melody with Purple, Hilt Stab > Recital is the fastest way to get your song going.\t\t\t\t\t\t\t\t\t\r\nHH has the lowest hit requirement of all weapons to ready a Transcend burst attack.\t\t\t\t\t\t\t\t\t\r\nHH is a good but also somewhat harsh learning tool in regards to fighting Raviente as part of a combat team, especially in regards to animation lock.\t\t\t\t\t\t\t\t\t\r\nIf you are using Cyan + Red notes, do not attempt to cross streams and go from AuL to DuL without encoring the former first if there are Adrenaline users. You will end up playing a Health Increase melody in the process, which will deactivate it.\t\t\t\t\t\t\t\t\t\n" +
                "Going the heal-only route with items is not a great idea because you will already be trying to juggle buffs and damage on a relatively slow weapon. It's not worth it.\t\t\t\t\t\t\t\t\t\r\nThe sonic bomb and debuff song motions can reliably apply the effects of the Red & Blue Soul skills onto other hunters.\t\t\t\t\t\t\t\t\t\r\nThe offerings of Soul Up are pretty reasonable, so it's not uncommon for HH users to take up both Red Soul (via Blazing Majesty) and Blue Soul for extra buffs.\t\t\t\t\t\t\t\t\t\n\n" +
                "Blue Tower for Poison, Z100 otherwise.\n\n" +
                "HH Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nMusical Attacks\tMotion value changes to all recital mode motions, all made roughly ~50% stronger\t\t\t\r\nBeatdown\tMotion value change on the hilt stab (16 > 27)\t\t\t\r\nNote Change (1, 2, 3)\tCan change the respective note's slot to the color listed on the sigil\t\t\t\r\nMisc\t\t\t\t\r\nStun Value\tNot HH-specific, but important\t\t\t\r\nSonic Bomb Range\tAlso extends the reach of the HH's sonic bomb effects\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nEncourage +2\tThe user gets Horn Maestro and also grants party-wide Evasion +2 and KO immunity. The Z skill confers party-wide stamina buffs. Effectively mandatory on HH.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. Use the Z skill to boost raw gains further.\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nRed/Blue Soul Up\tAttack teammates or use sonics to provide them bonuses; Soul Up modernizes these skills to be useful. Blue Soul can be often found on Soul Up pieces and you can get Red Soul via Blazing Majesty +1/2.\t\t\t\t\t\t\t\t\r\nKO Technique\tBoosts stun damage. Stack it with the sigil effect.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nUnaffected +3\tThis catch-all hearing/tremor/wind resist skill can provide less interruptions when playing music.\t\t\t\t\t\t\t\t\r\nIce Age (Up)\tThe skill has low hit requirements for HH and it can be a nice source of extra damage.\t\t\t\t\t\t\t\t\r\nAdrenaline\tPopular in Frontier due to the availability of -60 HP food, Evasion +2, and overall more dangerous but predictable monsters. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill, including the aforementioned Red Soul.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\r\nStylish (Up)\tHH doesn't attack as frequently, so the use case for this isn't exactly high priority.\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tMainly only used in Raviente hunts as the stamina drain is mostly disruptive, especially with being unable to roll out of recital mode.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nAssistance (Up)\tEffectively superseded by Red/Blue Soul Up and is on fewer pieces You also have to be in range of other teammates for the buffs to apply.\t\t\t\t\t\t\t\t\r\nWide-Range +3\tIt's going to be a pain juggling damage dealing, buffs, debuffs, and items. Though HH in Frontier is more support oriented than mainline and that this can still be useful, never go full-in support.\t\t\t\t\t\t\t\t";

    public static string GetGameLAInfo => "THE WALL. And the last guy to return from a trip on road, when you stop tripping them that is.\t\t\t\t\t\t\t\t\t\r\nIts best use is on hunting road for extra survival, or as part of cheese methods to beat certain fights.\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\r\nTake no chip damage from blocking any attack. Can chain up to four consecutive hops instead of three. +10 MV bonus to the final Normal/Upward/Skyward thrust in a combo.\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\r\nLong is the most common length on Zenith Lances for a reason. Very Long may have uses, but you may find it to be very unwieldly (and ridiculous).\t\t\t\t\t\t\t\t\t\r\nLance retains its cutting/impact hitzone adjustment modifier; if 0.72x of impact's hitzone is still superior to cutting; Lance uses impact's value as cutting damage. (Carapaceons are a good example of this)\t\t\t\t\t\t\t\t\t\r\nWith Lance Hiden, Guard/Fortification +2, and Guard Up, only a handful of attacks in the game cannot be blocked safely. This list mostly extends to things like most modern nukes, DOTs, Barioth applying snowman, etc.\t\t\t\t\t\t\t\t\t\r\nZenith-level hazards will always cause pushback if you're not using the heavy guard, guard advance, or running shield bash.\t\t\t\t\t\t\t\t\t\r\nThe shield bash while running takes zero pushback from guarding with it and has an innate Guard +2. The only equivalent to this from neutral is Storm's guard advance, which is much more rigid.\t\t\t\t\t\t\t\t\t\r\nThe ranged guard works by blocking the hit yourself for it to block for other players.\t\t\t\t\t\t\t\t\t\r\nBackhop thrusts return into guarding faster, you can also do them from neutral.\t\t\t\t\t\t\t\t\t\r\nActual time attack play mostly invokes an evasive playstyle, although reflects from the Perfect Guard caravan skill see usage and shield bashes still build Obscurity + shield phial charges.\t\t\t\t\t\t\t\t\t\n" +
                "Guard lancing can be a good tool to learn parrying with other weapons.\t\t\t\t\t\t\t\t\t\n" +
                "The Damage buff lasts 3 minutes, while the stamina buff lasts a few seconds minimum (maximum depends on stamina usage). You can only heal your teammates, they don't receive more attack and stamina.\n" +
                "With Guard+2 and Guard Up, you can strong guard Stygian Zinogre's nuke, Guanzorumu's nuke, HC Akantor's nuke, Zenith Hyujikiki's Super (Area-Guard), Sparkling Zerureusu's Landing, Conquest Fatalis's nuke, Rajang's Nuke, Zenith Rukodiora's Impale, Harudos' Nukes, Shagaru Magala's Nuke, Elzelion's Phase Transition, Uragaan's Super (need good angle), Taikuns' Super Slam, Zenith Gravios' Super (block backwards), Duremudira's Charge Attack, 3rd Phase Duremudira's Spin into Ice attack, Arrogant Duremudira's Aerial Spin.\n\n" +
                "Blue Tenrou for Poison, Z100 Affinity otherwise.\n\n" +
                "LA Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nShield Attack\tMotion value change (3-25 > 4-37)\t\t\t\r\nCharge Finisher\tMotion value change (50 > 75)\t\t\t\r\nTech Change\t\t\t\t\r\nDrill Charge\tEarth-only attack change\t\t\t\r\nMisc\t\t\t\t\r\nDraw Strength\tUnsheathe attacks boosted by x1.50\t\t\t\r\nLance Length Up\tLength raised by one step at the cost of -25 raw, can stack with no extra raw loss\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback. Requires perfect guards and evasion to advance reliably.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nEvasion +2\tLance does lend itself well to an evasion playstyle. It is reasonable to drop any source of this skill if you require space for a guard-focused set.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\tNote that Lance can run evasion and guard playstyles. It is common to mix elements of the two somewhat for buffs but it is best to focus on one or the other due to constraints.\t\t\t\t\t\t\t\t\r\nObscurity (Up)\tCan get up to +300 true raw by guarding. The Z skill boost provides a decent means of restoring sharpness on guard sets especially. Should be taken on either playstyle.\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. You could skip this if you're playing a guard-oriented set.\t\t\t\t\t\t\t\t\r\nStylish (Up)\tSharpness restoration on avoiding attacks. Recommended for evasion-focused play\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nAbnormality\tGood compound skill when paired with poison or paralysis.\t\t\t\t\t\t\t\t\r\nFortification +2\tGrants Guard +2 and stamina buffs. Guard Up in Frontier is a Z skill, granting Lance 360° blocking; it only trips the player if they are hit from behind.\t\t\t\t\t\t\t\t\r\nReflect +3 (Up)\tWith the Reflect Up Z skill, every blocked monster attack sets off a 68 MV hit. It benefits from raw and deals impact damage, but it cannot crit or use element/status.\t\t\t\t\t\t\t\t\r\nPerfect Guard\tTimed blocks do not take chip damage, proc a 72 MV Reflect attack, and can immediately be dodged out of. If you are using Reflect 3 + Up, reflect damage from perfect guards are boosted to 92 MV.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nUnaffected\tCan create some more attacking opportunities and quality of life, but Lance has other reliable means to deal with hazards and this competes for precious skill points and slots.\t\t\t\t\t\t\t\t\r\nAdrenaline\tLance Hiden negates chip damage entirely from blocking monster attacks, so it may be safer to run outside of unblockable damage sources. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\t\t\t\t\t\t\t\t\r\nIce Age (Up)\tGrants a DOT AoE around your character, pairs great with poison and grants other benefits. Useless Z skill.\t\t\t\t\t\t\t\t\r\nCharge Attack UP +2\tCan be used with charged slash combos and evasion slashes, but is otherwise niche.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nFocus/Trained +2\tIt only boosts the charged stab.\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tVery harsh on Lance.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tJust block everything.\t\t\t\t\t\t\t\t\r\nAbsolute Defense\tTakes priority over guarding.\t\t\t\t\t\t\t\t";

    public static string GetGameGLInfo => "With shelling not being that useful today, GL's main shtick is being a more aggressive Lance, utilizing the Heat Blade feature and blast dashes for forward mobility.\t\t\t\t\t\t\t\t\t\r\nVery comparable to the regular Lance in survivability, it's mostly up to taste.\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\r\nBoth Wyvern Fire and Heat Blade cooldowns are halved, and the latter starts up faster. Shelling capacity is increased, the amount depends on the weapon's shelling type.\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\r\nHeat Blade grants numerous benefits; a second hitbox on every physical attack, extended reach, and there is no sharpness loss until it wears off for -100 units. On activation, it also restores a bit of sharpness.\t\t\t\t\t\t\t\t\t\r\nIf the weapon has status, Heat Blade doesn't inflict extra. But if it doesn't have an element, it will use the higher raw heat blade motion value.\t\t\t\t\t\t\t\t\t\r\nAlthough GL can get similar guarding benefits as Lance, it cannot negate chip damage which will make blocking certain attacks potentially lethal, especially on Adrenaline.\t\t\t\t\t\t\t\t\t\r\nHit requirements for multiple skills and transcend are easier to meet with Heat Blade activated.\t\t\t\t\t\t\t\t\t\n" +
                "Recoil/Gentle Shot enables GL to evade after shelling.\t\t\t\t\t\t\t\t\t\r\nConsumption Slayer can be freely abused during Heat Blade's uptime, and can be offset with Obscurity Up's sharpness recovery.\t\t\t\t\t\t\t\t\t\n\n" +
                "Blue Tenrou for Poison, Z100 Affinity otherwise.\n\n" +
                "GL Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nRush Thrust\tMotion value change (38 > 57)\t\t\t\r\nHorizontal Slash\tMotion value change (34 > 51), does not affect Extreme dash followup\t\t\t\r\nMisc\t\t\t\t\r\nShell Change\tShelling type changes to the one displayed on the sigil\t\t\t\r\nShelling Level Up\tUpgrade shelling by one level, can stack up to the maximum of Lv9\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback. Requires perfect guards and evasion to advance reliably.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nEvasion +2\tIt is reasonable to drop any source of this skill if you require space for a guard-focused set, but this will come at the expense of few i-frames on the blast dash.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nObscurity (Up)\tCan get up to +300 true raw by guarding. The Z skill boost provides a decent means of restoring sharpness on guard sets especially, which GL can use to mitigate the sharpness drop from Heat Blade expiring in advance.\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tBuilds up incredibly fast on GL. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nLavish Attack\tProvides +100 true raw and the sharpness penalty is negated if Heat Blade is activated. However, it may be harder to keep up sharpness on road.\t\t\t\t\t\t\t\t\r\nAbnormality\tGood compound skill when paired with poison or paralysis.\t\t\t\t\t\t\t\t\r\nFortification +2\tGrants Guard +2 and stamina buffs. Guard Up in Frontier is a Z skill, granting GL 360° blocking; it only trips the player if they are hit from behind.\t\t\t\t\t\t\t\t\r\nReflect +3 (Up)\tWith the Reflect Up Z skill, every blocked monster attack sets off a 68 MV hit. It benefits from raw and deals impact damage, but it cannot crit or use element/status.\t\t\t\t\t\t\t\t\r\nPerfect Guard\tTimed blocks do not take chip damage, proc a 72 MV Reflect attack, and can immediately be dodged out of. If you are using Reflect 3 + Up, reflect damage from perfect guards are boosted to 92 MV.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tMuch easier to utilize with Heat Blade.\t\t\t\t\t\t\t\t\r\nUnaffected\tCan create some more attacking opportunities and quality of life, but GL has other reliable means to deal with hazards and this competes for precious skill points and slots.\t\t\t\t\t\t\t\t\r\nAdrenaline\tGood user of this skill, but chip damage from guard always poses a risk. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tGood source of raw, but might be difficult to find room for.\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. The most notable for GL is Artillery God, boosting shelling and WF damage but their usage is almost absent at this point.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nIce Age (Up)\tGrants a DOT AoE around your character, pairs great with poison and grants other benefits. Useless Z skill.\t\t\t\t\t\t\t\t\r\nCharge Attack UP +2\tCan be used with charged slash combos and evasion slashes, but is otherwise niche.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nRecoil / Gentle Shot\tCan evade after shelling, Gentle Shot also bolsters shell capacity by one. Unfortunately shelling isn't really worth it.\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tVery harsh on GL.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tJust block everything.\t\t\t\t\t\t\t\t\r\nAbsolute Defense\tTakes priority over guarding.\t\t\t\t\t\t\t\t";

    public static string GetGameTOInfo => "The Tonfa is a weapon that attacks primarily with airdashes. It could also fight on the ground.\t\t\t\t\t\t\t\t\t\r\nEffectively the strongest weapon in the game, relative to effort taken.\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\r\nThe combo and EX meters are both extended to have a sixth segment each. The former technically augments Tonfa's combo multiplier with an additional 1.10x raw, up to a maximum of 1.60x.\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\r\nDue to how the combo meter inflates raw, Tonfa requires a much higher My Mission attack ceiling than most weapons!\t\t\t\t\t\t\t\t\t\r\nLong mode is the default and does more KO damage. Short mode was reworked into being centered around meter gain, but it has a ton of hitlag and isn't really worth using outside of the dash attack for meter.\t\t\t\t\t\t\t\t\t\r\nGuardpoints do exist on some attacks, but it's not that practical to invoke them. Iron Arm is also fairly tough to get on modern sets.\t\t\t\t\t\t\t\t\t\r\nThe Ryuuki finisher tops up meters of all nearby Tonfa players. It can also be triggered by Tonfa-wielding hunter NPCs. These bursts also grant a fair few other benefits, but most are pretty lightweight in function at this point.\t\t\t\t\t\t\t\t\t\r\nTo keep your combo meter, just hit or evade things. Generally, the timer runs slower if the monster is going through a lengthy attack when you can't or shouldn't be hitting them.\t\t\t\t\t\t\t\t\t\n" +
                "EX Evasion doesn't cost stamina, only meter.\t\t\t\t\t\t\t\t\t\r\nEX Pursuit (charged jab + meter) benefits from Gunnery/Artillery (obtainable via Blazing Majesty), Charge Attack Up, and Focus, but is quite expensive compared to airdash spam.\t\t\t\t\t\t\t\t\t\r\nLike SnS, two well-timed blocks with the Perfect Defense caravan skill alongside Obscurity Up can amass 300 raw until you get knocked back.\t\t\t\t\t\t\t\t\t\n\n" +
                "Blue Tower for Poison, Z100 Affinity otherwise.\n\n" +
                "TO Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nEX Meter\tSlightly increases EX meter gain\t\t\t\r\nCombo Timer\tThe combo timer decreases more slowly before meter is lost\t\t\t\r\nMisc\t\t\t\t\r\nStun Value\tNot Tonfa-specific, but still relevant as an impact weapon\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nEvasion +2\tGreatly increases survivability in Frontier, also triggers evasion-reliant skills much more easily. Obtain it through Drawing Arts +2, Evasion Boost, Encourage, or wearing a piece of Nargacuga armor.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. Use the Z skill to boost raw gains further.\t\t\t\t\t\t\t\t\r\nStylish (Up)\tSharpness restoration on avoiding attacks.\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nAbnormality\tStellar compound skill when paired with poison.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tPairs well with EX Evasion spam on the ground as they do not cost stamina. On JP it is more commonly paired with Rush Up and used in the air if possible.\t\t\t\t\t\t\t\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\t\t\t\t\t\t\t\t\r\nAdrenaline\tPopular in Frontier due to the availability of -60 HP food, Evasion +2, and overall more dangerous but predictable monsters. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. For Tonfa, this can boost the damage from EX Pursuits significantly due to the Artillery God portion.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\t\t\t\t\t\t\t\t\r\nIce Age (Up)\tGrants a DOT AoE around your character, pairs great with poison and grants other benefits. Useless Z skill.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\r\nObscurity (Up)\tViable with a combination of Obscurity Up and the Perfect Guard caravan skill. However, this needs good timing and the boost is lost on knockback, but if executed well you can get +300 true raw on two perfect guards.\t\t\t\t\t\t\t\t\r\nStarving Wolf +2\tGrants +0.10x to the crit multiplier, +50% affinity and Evasion +2 if you are at 25 Stamina and do not have infinite stamina buffs. Pairs well with Combat Supremacy.\t\t\t\t\t\t\t\t\r\nFocus/Trained +2\tFocus can be used with Storm EX Pursuit attacks, otherwise effectively a luxury. Trained only exists on JP, and also includes Martial Arts +2 which can help sustain airtime.\t\t\t\t\t\t\t\t\r\nCharge Attack UP +2\tIncreases the damage of the EX Pursuit and the charged overhead.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nLavish Attack\tYour sharpness will get murdered.\t\t\t\t\t\t\t\t\r\nIron Arm +2\tTechnically boosts Tonfa's guardpoints by 7 frames, but the skill is very rare.\t\t\t\t\t\t\t\t\r\nAbsolute Defense\tTakes priority over guarding.\t\t\t\t\t\t\t\t";

    public static string GetGameSAFInfo => "In a strange alternate reality, Swaxe gained proper defensive options and helpful things like getting meter from axe hits.\t\t\t\t\t\t\t\t\t\r\nThis is a very consistent weapon if you can reliably parry attacks to sustain phial energy.\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\r\nWild Swings have halved stamina drain, axe attacks gain more meter, and attacks using the phial use less meter. Additionally, successful parries (in Extreme style) reward a temporary 1.05x bonus to raw, indicated by a glowing arm effect.\t\t\t\t\t\t\t\t\t\r\nThe means for receiving the 1.05x boost differs between styles.\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\r\nConsider using Focus +2 if you lack Switch Axe Hiden.\t\t\t\t\t\t\t\t\t\r\nA power phial doesn't increase raw per se, it boosts the motion values of phial attacks by roughly 10% instead. It's still considered the best kind, but is found on fewer Swaxes.\t\t\t\t\t\t\t\t\t\r\nElemental phials are currently the most common phial type on Zenith weapons, and in some cases do still provide a massive boost.\t\t\t\t\t\t\t\t\t\r\nA source of Guard +2 is much more important on Swaxe than its comtemporaries LS and MS, because the weapon relies on parries to sustain meter and not just Obscurity stacks.\t\t\t\t\t\t\t\t\t\r\nWith hiden, it takes two parries to fill the meter and activate light sword mode. If the Swaxe active feature is one, it only takes one.\t\t\t\t\t\t\t\t\t\r\nThe elemental discharge from Earth can still be done in Extreme, just tap the other button after the initial thrust.\t\t\t\t\t\t\t\t\t\n" +
                "Swaxe's healing from the guard sigil is useful to get around attacks that cause chip damage on parrying, especially on Adrenaline levels of HP.\t\t\t\t\t\t\t\t\t\r\nAs Swaxe's unsheathed running uses the phial instead of stamina, you may find it easier to use skills like Starving Wolf or Combat Supremacy.\t\t\t\t\t\t\t\t\t\r\nThe second stage of Rush negates all meter drain while running.\t\t\t\t\t\t\t\t\t\r\nSome monsters may start fights doing two hits that can be parried in succession to start you in lightsword right away. If you have a Prayer Swaxe with the buff active, you only need to parry one hit.\t\t\t\t\t\t\t\t\t\n\n" +
                "Blue Tenrou for Poison (Can change phial type, best overall is Power), Z100 Raw otherwise. Francisca Z is also nice, but held back by the stun phial.\n\n" +
                "SAF Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nStunning Blast\tAdds stun values on elemental discharge and absorption release\t\t\t\r\nGuard\tParry hitbox slightly increased(?) and successful parries heal 15 HP\t\t\t\r\nCharge Movement Spd\tCan move faster while charging\t\t\t" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback. Builds up very fast on Swaxe.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nEvasion +2\tGreatly increases survivability in Frontier, also triggers evasion-reliant skills much more easily. Obtain it through Drawing Arts +2, Evasion Boost, Encourage, or wearing a piece of Nargacuga armor.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. Use the Z skill to boost raw gains further.\t\t\t\t\t\t\t\t\r\nStylish (Up)\tSharpness restoration on avoiding attacks.\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW.\t\t\t\t\t\t\t\t\r\nObscurity (Up)\tCan get up to +225 true raw by parrying. The Z skill boost provides a different means of restoring sharpness.\t\t\t\t\t\t\t\t\r\nGuard/Fortification +2\tAllows parries to block more kinds of attacks, vital for meter in certain hunts (ie Z Hypnoc) or allows for more opportunities in general. Can get Guard +2 with Uragaan armor. Guard Up doesn't do much for SAF at all.\t\t\t\t\t\t\t\t\r\nAbnormality\tGood compound skill when paired with poison.\t\t\t\t\t\t\t\t\r\nPerfect Guard\tTimed blocks do not take chip damage, proc a 72 MV Reflect attack, and can immediately be rolled out of.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nFocus/Trained +2\tVery useful before hiden but fairly redundant afterwards outside of charge attack-based sets. Can be acquired with Gore Magala armor, more reliably obtained with premium gear. Trained only exists on JP.\t\t\t\t\t\t\t\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\t\t\t\t\t\t\t\t\r\nAdrenaline\tPopular in Frontier due to the availability of -60 HP food, Evasion +2, and overall more dangerous but predictable monsters. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\t\t\t\t\t\t\t\t\r\nIce Age (Up)\tGrants a DOT AoE around your character, pairs great with poison and grants other benefits. Useless Z skill.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tLegitimate playstyle with Swaxe. However, the evasion slashes and meter-based unsheathed running become the only reliable means of dodging attacks.\t\t\t\t\t\t\t\t\r\nStarving Wolf +2\tGrants +0.10x to the crit multiplier, +50% affinity and Evasion +2 if you are at 25 Stamina and do not have infinite stamina buffs.\t\t\t\t\t\t\t\t\r\nCharge Attack UP +2\tCan be used with charged slash combos and evasion slashes, but is otherwise niche.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nAbsolute Defense\tTakes priority over guarding.\t\t\t\t\t\t\t\t";

    public static string GetGameMSInfo => "Magspike is to Frontier what Charge Blade was to MH4U. Although it hasn't put Hammer out of a job completely, you can take this thing on any content you want.\t\t\t\t\t\t\t\t\t\r\nThe weapon sees heavy usage on Raviente trains.\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\r\nThe marker does not disappear unless you faint, and base movement speed is increased by 1.2x. Successfully using i-frames on evade motions grants a small raw boost (1.03x) and better meter recovery (1.5x).\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\r\nFollow these instructions for starting and going through the questline to unlock Magnet Spikes, and how to use them.\t\t\t\t\t\t\t\t\t\r\nThe pin attack applies the damage to where you have placed a marker on a monster, you can use this to break certain parts faster.\t\t\t\t\t\t\t\t\t\r\nPinning reliability depends on who the current area host is. If you are not the host, there is a chance that it can fail. NPCs are tied to their host player in this context.\t\t\t\t\t\t\t\t\t\r\nPay attention to whether you are running unsheathed or not, since it can limit your attack options.\t\t\t\t\t\t\t\t\t\r\nFalling attacks via magnetic attraction have both generous motion values and i-frames.\t\t\t\t\t\t\t\t\t\r\nThe basic cutting attacks all have a very large window between each of them, which can be roll cancelled or delay attacks.\t\t\t\t\t\t\t\t\t\r\nCutting's guard motion is effectively a guardpoint attack, it can chain into the Finishing Slash, a magnetic dodge if you need more intangibility, or another guard attack.\t\t\t\t\t\t\t\t\t\r\nImpact's guard has forced fixed knockback and the rebounding attack is similar to but doesn't chain into any meter attacks like Magnetic Assault does, although it can be roll cancelled and still has i-frames.\t\t\t\t\t\t\t\t\t\n" +
                "There is officially a second control bindings menu for the weapon, but only the approach cancel is binded by default for PSP controls in that menu.\t\t\t\t\t\t\t\t\t\r\nMagspike lacks an elemental multiplier for Charge Attack Up. The skill will only boost the impact charged strike, although by a large degree.\t\t\t\t\t\t\t\t\t\r\nIt can be practical to skip out on a source of Evasion +2 completely, due to the copious amount of innate i-frames.\t\t\t\t\t\t\t\t\t\n\n" +
                "Z100 Status is best in slot.\n\n" +
                "MS Sigil Effects\t\t\t\t\r\n\tThere are no Magnet Spike-specific sigil effects outside of UL.\t\t\t\r\nMisc\t\t\t\t\r\nStun Value\tNot Magspike-specific, but still relevant in impact mode\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon. Good raw multiplier, super earplugs, and a myriad of benefits. Does not take up a skill slot if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40) or Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback. Builds up very fast on MS.\t\t\t\t\t\t\t\t\r\nSword God +2\tCombines Sharpness +1, Fencing +2, and Razor Sharp +2 into one excellent compound skill.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts), going higher only increases leech rate.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. Use the Z skill to boost raw gains further.\t\t\t\t\t\t\t\t\r\nStylish (Up)\tSharpness restoration on avoiding attacks.\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Builds up quickly in cutting mode.\t\t\t\t\t\t\t\t\r\nObscurity (Up)\tCan get up to +225 true raw by parrying. The Z skill boost provides a different means of restoring sharpness.\t\t\t\t\t\t\t\t\r\nAbnormality\tStellar compound skill when paired with poison.\t\t\t\t\t\t\t\t\r\nPerfect Guard\tTimed blocks do not take chip damage and proc a 72 MV Reflect attack.\t\t\t\t\t\t\t\t\r\nEvasion +2\tMS's meter attacks already have a generous amount of i-frames, but this can be a nice luxury otherwise.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nGuard/Fortification +2\tAllows parrying to block more kinds of attacks. You can get Guard +2 with Uragaan armor. Guard Up has little discernible effect on parry weapons.\t\t\t\t\t\t\t\t\r\nFocus/Trained +2\tVery useful before hiden for meter, but effectively a luxury afterwards.\t\t\t\t\t\t\t\t\r\nCharge Attack UP +2\tBolsters the MV of Impact mode's charged swing attack dramatically. Does not grant it extra elemental damage.\t\t\t\t\t\t\t\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\t\t\t\t\t\t\t\t\r\nAdrenaline\tPopular in Frontier due to the availability of -60 HP food, Evasion +2, and overall more dangerous but predictable monsters. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tProvides a 1.15x boost to raw if your HP is 100 or greater. Vigorous Up provides +100 true raw at maximum HP. Its usefulness varies a lot and may outright be skipped on JP depending on your other skills.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 30> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill.\t\t\t\t\t\t\t\t\r\nIce Age (Up)\tGrants a DOT AoE around your character, pairs great with poison and grants other benefits. Useless Z skill.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tUsable, but stamina has to be managed.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nAbsolute Defense\tTakes priority over guarding.\t\t\t\t\t\t\t\t";

    public static string GetGameLBGInfo => "In MHF, rapid fire and element take a backseat as raw and the 'Just Shot' take precedence. The easier bowgun to get into and also have results with.\t\t\t\t\t\t\t\t\t\t\r\nIronically, no Raviente combat team is complete without a Raviente LBG, thanks to their exclusive Acid S. The role however does require you to know what you are doing.\t\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\t\r\nA 'Perfect Shot' window is added to the Just Shot meter which deals more damage. Ammo has a chance of not bouncing off monsters, and items can be used unsheathed like SnS. \t\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\t\r\nAs a Gunner weapon, LBG can meet the requirements of various skills extremely quickly.\t\t\t\t\t\t\t\t\t\t\r\nLong barrels don't provide a raw bonus. Instead it is a shot speed increase, which may screw up timing with pierce ammo hits.\t\t\t\t\t\t\t\t\t\t\r\nAs Rapid Fire doesn't really have a place in modern Frontier, newer guns don't come with it. You can still force it with sigils, though.\t\t\t\t\t\t\t\t\t\t\r\nPierce S is the workhorse raw ammo type used.\t\t\t\t\t\t\t\t\t\t\r\nIt may be easier to stick with first-person camera options if you are having trouble tracking ammo when using Just Shots, with the >clip display moved under the crosshairs.\t\t\t\t\t\t\t\t\t\t\n" +
                "Like SnS, using items unsheathed with LBG hiden resets Rush.\t\t\t\t\t\t\t\t\t\t\r\nSpacing increases the size of LBG Hiden's Perfect Shot window when the buff activates.\t\t\t\t\t\t\t\t\t\t\r\nConsider building sets with and without Combat Supremacy. Thunder Clad provides good mobility, but some monsters may call for evasion.\t\t\t\t\t\t\t\t\t\t\r\nAcid S applies a hitzone debuff, which is why they are extensively against Raviente (because the monster is largely three hitboxes)\t\t\t\t\t\t\t\t\t\t\r\nCombined with Caring +3, LBG is a common pick on Raviente support teams to clean up trash mobs fast with Pellet S.\t\t\t\t\t\t\t\t\t\t\n\n" +
                "Z100 Raw Acid is BIS.\n\n" +
                "LBG Sigil Effects\t\t\t\t\r\nRapid Fire\t\t\t\t\r\nRapid Fire Add\tCan fire X shot type as rapid fire\t\t\t\r\nSuper RF Add\tCan fire X shot type as super rapid fire\t\t\t\r\nRapid Fire Immunity\tDisables rapid fire of X shot type\t\t\t\r\nTech Change\t\t\t\t\r\nJ Gauge\tJust Shot's sweetspot window increased\t\t\t\r\nMisc\t\t\t\t\r\nBlunt Bullets\tPellet shots use impact damage\t\t\t\r\nExplosion\tCluster shots may detonate into large explosions\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\t\r\nZenith Armor Piece\tNot a formal skill, but wearing at least one Z/ZF/ZY/ZX or ZP piece grant an innate critical distance bonus. This is effectively compulsory and provides a large damage boost.\t\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon but massively powerful for Gunners especially. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable and Critical Shot/Precision +2 redundant.\t\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback. Relies on evasion to build stacks.\t\t\t\t\t\t\t\t\t\r\nSteady Hand\tCombines Normal/Pierce/Pellet Up in one skill. In JP, a second rank provides the hitzone buff of Critical Shot/Precision +2 but it is redundant with Determination.\t\t\t\t\t\t\t\t\t\r\nCritical Shot / Precis +2\tGrants +5 to hitzones if the monster is in critical distance. Obsolete with Determination.\t\t\t\t\t\t\t\t\t\r\nLoading\tYou will want either Mounting or Gentle Shot for the +1 to capacity. Either one will suffice on a Raviente LBG.\t\t\t\t\t\t\t\t\t\r\nSpacing\tLowers critical distance but attacks within it build up an invisible meter to further boost damage and grant mobility skills. LBG's Perfect Shot window increases.\t\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity. Builds up incredibly quickly on guns.\t\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts),  this builds up very quickly with guns.\t\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tIncredibly safe skill for LBG to take in many matchups, effectively being a free 1.20x raw boost.\t\t\t\t\t\t\t\t\t\r\nEvasion +2\tMight be wasted on Combat Supremacy sets outside of JP's Rush Up if you can obtain the stamina buff before the bar runs dry. Obtain it through Drawing Arts +2 or wearing a piece of Nargacuga armor.\t\t\t\t\t\t\t\t\t\r\nBullet Saver (Up)\tHave a chance to not consume ammo on firing, up to 46% with the Z skill.\t\t\t\t\t\t\t\t\t\r\nReload / Mounting\tUsed to 'fix' reload speed. 'Fast' is needed to reload Pierce 2 quickly and 'Very Fast' for Pierce 3. Mounting +2 and +3 require less points on JP.\t\t\t\t\t\t\t\t\t\r\nRecoil / Gentle Shot\tUsed to 'fix' recoil levels. 'Small' is needed to fire Pierce 3 effectively. The +3 versions of both skills require less points on JP.\t\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well. Try using it on sets without Combat Supremacy.\t\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed (and do not use any items with LBG hiden). Cannot get the Z skill on TW, but it can halt stamina drain from Combat Supremacy on JP. Builds up quickly on guns.\t\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW. Builds up quickly on guns.\t\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\t\t\t\t\t\t\t\t\t\r\nAdrenaline\tSomewhat easier to run this on guns than on Blademaster weapons. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill, especially with Pierce.\t\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. These vary greatly but it can be an alright quality of life skill.\t\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad and other Gunner skills. Obsolete with Determination.\t\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\t\r\nAmmo Combiner\tYou could also take multiple Books of Combos and Caravan skills instead. Can be obtained with Uragaan armor.\t\t\t\t\t\t\t\t\t\r\nShiriagari\tThis skill can squeeze some extra raw out but may be contested by other skills especially. Often employed with skill fruits in time attack.\t\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\t\r\nRapid Fire\tLittle practical use as Rapid Fire capabilities are effectively absent in the current meta.\t\t\t\t\t\t\t\t\t\r\nAuto-Reload / Sniper\tUnlike MHFU, this effectively makes your gun unusable.\t\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tGunners are significantly more fragile, of which you might as well run Adrenaline if you have run a skill dependent on HP. The benefit from the Z skill is also halved.\t\t\t\t\t\t\t\t\t";

    public static string GetGameHBGInfo => "Frontier's HBG sets itself apart from mainline with the ability to use compressed and charged shots, on top of laser beams. An acquired taste.\t\t\t\t\t\t\t\t\t\t\r\nRaviente HBGs are a huge investment, but also have an incredible amount of attack that enables them to keep up in trains as a potential GS replacement.\t\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\t\r\nInnate Evade Extender. The power of Element and Status shots are increased by 1.2x, while Melee, Crag, Cluster inflict (more) KO. Compression shots can be perfectly-timed for extra damage, and Heat Beams deal 1.2x more damage.\t\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\t\r\nAs a Gunner weapon, HBG can meet the requirements of various skills extremely quickly.\t\t\t\t\t\t\t\t\t\t\r\nOne use of compressed shots is on faster targets. Mitigating recoil on these requires more work than usual.\t\t\t\t\t\t\t\t\t\t\r\nExtreme style's heat gauge doesn't require any item management. Blast dodges use its meter and have a ton of i-frames, and can go into a run or roll after landing.\t\t\t\t\t\t\t\t\t\t\r\nCharging shots can be used to save ammo, works best with compressed Normal 3 and Element.\t\t\t\t\t\t\t\t\t\t\n" +
                "Spacing increases the size of HBG Hiden's 'perfect' window for compression shots when the buff activates.\t\t\t\t\t\t\t\t\t\t\r\nBullet Saver factors in every shot used in a single compression shot salvo. All saved ammo is still retained in the >clip and can be fired off.\t\t\t\t\t\t\t\t\t\t\r\nRaviente HBG bomb shots are kinda shitty, nobody uses them for those (just the retarded statline will do).\t\t\t\t\t\t\t\t\t\t\r\nThe shield addon doesn't benefit from Obscurity.\t\t\t\t\t\t\t\t\t\t\n\n" +
                "Z100 Affinity is the highest raw weapon in all of Monster Hunter\n" +
                "Has seemingly not so good performance on Musous.\n\n" +
                "HBG Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nHeat Cannon\tAdds the heat beam functionality, co-exists with Extreme's blue beam\t\t\t\r\nMisc\t\t\t\t\r\nBlunt Bullets\tPellet shots use impact damage\t\t\t\r\nExplosion\tCluster shots may detonate into large explosions\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\t\r\nZenith Armor Piece\tWearing at least one Z/ZF/ZY/ZX or ZP piece grant an innate critical distance bonus. This is effectively compulsory and provides a large damage boost.\t\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon but massively powerful for Gunners especially. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, Elemental Exploiter, and any source of Adrenaline +2 in one skill. Makes Guts unusable and Critical Shot/Precision +2 redundant.\t\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback.\t\t\t\t\t\t\t\t\t\r\nSteady Hand\tCombines Normal/Pierce/Pellet Up in one skill. In JP, a second rank provides the hitzone buff of Critical Shot/Precision +2 but it is redundant with Determination.\t\t\t\t\t\t\t\t\t\r\nCritical Shot / Precis +2\tGrants +5 to hitzones if the monster is in critical distance. Obsolete with Determination.\t\t\t\t\t\t\t\t\t\r\nLoading\tYou will want either Mounting or Gentle Shot for the +1 to capacity. A Raviente HBG will take the latter.\t\t\t\t\t\t\t\t\t\r\nSpacing\tLowers critical distance but attacks within it build up an invisible meter to further boost damage and grant mobility skills. HBG's perfect compression window increases.\t\t\t\t\t\t\t\t\t\r\nEvasion +2\tGreatly increases survivability in Frontier, also triggers evasion-reliant skills much more easily. Obtain it through Drawing Arts +2, Evasion Boost, or wearing a piece of Nargacuga armor.\t\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity. Builds up incredibly quickly on guns.\t\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts),  this builds up very quickly with guns.\t\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\t\r\nBullet Saver (Up)\tHave a chance to not consume ammo on firing, up to 46% with the Z skill. It rolls for every round in a compression shot.\t\t\t\t\t\t\t\t\t\r\nReload / Mounting\tUsed to 'fix' reload speed. 'Fast' is needed to reload Pierce 2 quickly and 'Very Fast' for Pierce 3. Mounting +2 and +3 require less points on JP.\t\t\t\t\t\t\t\t\t\r\nRecoil / Gentle Shot\tUsed to 'fix' recoil levels. High investment is required for less to no recoil on compressed shots. The +3 versions of both skills require less points on JP.\t\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW, but it can halt stamina drain from Combat Supremacy on JP. Builds up quickly on guns.\t\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW. Builds up quickly on guns.\t\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\t\r\nFocus/Trained +2\tCharged shots can be useful against fast monsters and other matchups, but this skill might be difficult to fit on. Trained only exists on JP.\t\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well.\t\t\t\t\t\t\t\t\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\t\t\t\t\t\t\t\t\t\r\nAdrenaline\tSomewhat easier to run this on guns than on Blademaster weapons. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill, especially with Pierce.\t\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. For HBG the Artillery God portion can boost heat beam damage.\t\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get with a high point requirement as well. Pairs very well with Thunder Clad and other Gunner skills. Obsolete with Determination.\t\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\t\r\nAmmo Combiner\tYou could also take multiple Books of Combos and Caravan skills instead. Can be obtained with Uragaan armor.\t\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tDifficult to use, especially as you won't be able to roll out of blast dodges.\t\t\t\t\t\t\t\t\t\r\nShiriagari\tThis skill can squeeze some extra raw out but may be contested by other skills especially. Often employed with skill fruits in time attack.\t\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\t\r\nAuto-Reload / Sniper\tUnlike MHFU, this effectively makes your gun unusable.\t\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tGunners are significantly more fragile, of which you might as well run Adrenaline if you have run a skill dependent on HP. The benefit from the Z skill is also halved.\t\t\t\t\t\t\t\t\t\r\nObscurity (Up)\tIt doesn't work with the shield addon.\t\t\t\t\t\t\t\t\t";

    public static string GetGameBowInfo => "The Bow in Frontier continues to do what it has been doing since the 2nd generation and shoots things with charged shots. Deceptively powerful.\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\r\nHiden Skill Effects\t\t\t\t\t\t\t\t\t\r\nPower Coatings are boosted to x1.7 with Gou/G-Rank/Evolution Bows. Arc shots can be fired at Lv2 charge and arrows cannot bounce.\t\t\t\t\t\t\t\t\t\r\nNotes\t\t\t\t\t\t\t\t\t\r\nAs a Gunner weapon, Bow can meet the requirements of various skills extremely quickly.\t\t\t\t\t\t\t\t\t\r\nBow has numerous aiming modes in the options, but ultimately it is left with 2nd generation-style controls. With that said, while charging you can press Square (or X) to dodge.\t\t\t\t\t\t\t\t\t\r\nWhile aiming, you can see the critical distance ranges of your shot.\t\t\t\t\t\t\t\t\t\r\nHolding down L1 while releasing a shot does a coatingless shot, not consuming nor granting the benefits of the loaded coating. This technique is based off a glitch in MH2.\t\t\t\t\t\t\t\t\t\r\nRapid shot types are recommended. Spread is powerful, but awkward. Pierce works well as a Lv3 charge.\t\t\t\t\t\t\t\t\t\r\nCrouching shots are awkward to use, but with the piercing shot sigil they can inflict status well.\t\t\t\t\t\t\t\t\t\n" +
                "As Sniper's critical distance bonus is wrapped into both Steady Hand +2 and Determination, just using Auto-Reload is fine.\t\t\t\t\t\t\t\t\t\n\n" +
                "Z100 Pierce-Rapid is best overall, can use special impact coatings.\n\n" +
                "Bow Sigil Effects\t\t\t\t\r\nTech Boost\t\t\t\t\r\nRising Dragon Bow\tMotion values boosted on Shoryuken attack (Heaven style and newer)\t\t\t\r\nTech Change\t\t\t\t\r\nPiercing Shot\tFully charged crouching shots (Storm/Extreme) turn into piercing shots\t\t\t\r\nMisc\t\t\t\t\r\nArc Shot Change\tChanges arc shot behavior to the type displayed on the sigil\t\t\t\n\n" +
                "Skill Recommendations\t\t\t\t\t\t\t\t\t\r\nSkill\tNotes / Reasoning\t\t\t\t\t\t\t\t\r\nCore\t\t\t\t\t\t\t\t\t\r\nZenith Armor Piece\tWearing at least one Z/ZF/ZY/ZX or ZP piece grant an innate critical distance bonus. This is effectively compulsory and provides a large damage boost.\t\t\t\t\t\t\t\t\r\n(Weapon) Hiden\tSecond best skill for any weapon but massively powerful for Gunners especially. Does not take up a skill slot on JP if a hiden cuff is used.\t\t\t\t\t\t\t\t\r\nDetermination\tBest in slot skill in all of Monster Hunter. Supersedes Issen, Critical Eye, Exploit Weakness, and any source of Adrenaline +2 in one skill. Makes Guts unusable and Critical Shot/Precision +2 redundant.\t\t\t\t\t\t\t\t\r\nStrong Attack\tUp to +150 true raw with Strong Attack +5 (40). JP has access to Strong Attack +6 (50).\t\t\t\t\t\t\t\t\r\nIssen +1/2/3\tCritical hit multiplier. Difficult to obtain higher levels of it on Zenith equipment outside of JP, but still very potent if you lack Determination.\t\t\t\t\t\t\t\t\r\nFurious\tVery powerful skill that grants successive raw, element, status, and affinity bonuses. Stacks are lost on getting hit, not just knockback.\t\t\t\t\t\t\t\t\r\nSteady Hand\tCombines Normal/Pierce/Pellet Up in one skill. In JP, a second rank provides the hitzone buff of Critical Shot/Precision +2 but it is redundant with Determination.\t\t\t\t\t\t\t\t\r\nCritical Shot / Sniper\tGrants +5 to hitzones if the monster is in critical distance. Sniper provides this bonus and Auto-Reload. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nMounting\tYou will want the skill for an extra charging level.\t\t\t\t\t\t\t\t\r\nSpacing\tLowers critical distance but attacks within it build up an invisible meter to further boost damage and grant mobility skills. Bow's charge times are made shorter.\t\t\t\t\t\t\t\t\r\nDrawing Arts +2\tGreat to have, not only it provides Evasion +2 but it also serves as a stamina skill for Bow.\t\t\t\t\t\t\t\t\r\nThunder Clad\tFill up a meter for a bonus against hitzones, additional movement speed, and basic status immunity. Builds up quickly on Bow.\t\t\t\t\t\t\t\t\r\nVampirism (Up)\tDramatically increases sustainability with health leeching and also adds some attack. You do not need more than Vampirism +1 (10pts),  this builds up very quickly on Bow.\t\t\t\t\t\t\t\t\r\nFlash Conversion (Up)\tGet +30% affinity, and all excess affinity is converted into extra raw. May be dropped by some players. Bad Z skill outside of Affinity-inclined Raviente weapons.\t\t\t\t\t\t\t\t\r\nSkill Slots Up\tZ skill that boosts the amount of maximum skill slots you can have by one (you have 12 by default with a full set of G-Rank armor). You can get up to +7 for a maximum of 19 skill slots, but this competes with other Z skills.\t\t\t\t\t\t\t\t\r\nRecommended\t\t\t\t\t\t\t\t\t\r\nAuto-Reload / Sniper\tBow does not use Focus in Frontier but rather shortens charge times with this skill instead. Sniper also includes the benefits of Critical Shot, but so does Determination. Auto-Reload is easier to gen into sets.\t\t\t\t\t\t\t\t\r\nLavish Attack\tAdds a multiplier bonus to attack when using any type of coating at the cost of two coatings used up per shot.\t\t\t\t\t\t\t\t\r\nStylish Assault (Up)\tFree +100 true raw on successful dodging and for effectively playing well.\t\t\t\t\t\t\t\t\r\nRush (Up)\tGood source of raw if you stay unsheathed. Cannot get the Z skill on TW, but it can halt stamina drain from Combat Supremacy on JP. Builds up quickly on Bow.\t\t\t\t\t\t\t\t\r\nCeaseless (Up)\tRepeat Offender on crystal meth with up to +50% affinity and even a higher critical hit multiplier. Cannot get the Z skill on TW. Builds up quickly on Bow.\t\t\t\t\t\t\t\t\r\nOther Skills\t\t\t\t\t\t\t\t\t\r\nUnaffected\tCatch-all hearing/tremor/wind resist skill. Can create some more attacking opportunities and quality of life, but min-maxing may involve avoiding this skill. Can be paired with Z skill equivalents for Zenith-grade resists.\t\t\t\t\t\t\t\t\r\nAdrenaline\tSomewhat easier to run on Bow than on Blademaster weapons. Get it through guild food, Buchigire, Blazing Majesty +2, or Determination.\t\t\t\t\t\t\t\t\r\nPoint Breakthrough\tSuccessive attacks on a single hitzone provide a bonus. Not always a consistent skill, especially with Pierce.\t\t\t\t\t\t\t\t\r\nBlazing Majesty\tCompound skill with a myraid of benefits. For HBG the Artillery God portion can boost heat beam damage.\t\t\t\t\t\t\t\t\r\nExploit Weakness\tBehaves like MH3U/MH4U Weakness Exploit but at 35> hitzones. Powerful raw skill, but hard to get. Pairs very well with Thunder Clad and other Gunner skills. Obsolete with Determination.\t\t\t\t\t\t\t\t\r\nExpert / Critical Eye\tAlternate source of pre-Determination affinity, but affinity itself is not difficult to get with other sources.\t\t\t\t\t\t\t\t\r\nStatus Immunity \tNiche or outright unnecessary for anything at a given point. Shagaru Magala armor is an easy means of obtaining this, but it comes with bad resistances and low defense in its stead.\t\t\t\t\t\t\t\t\r\nAmmo Combiner\tYou could also take multiple Books of Combos and Caravan skills instead. Can be obtained with Uragaan armor.\t\t\t\t\t\t\t\t\r\nCombat Supremacy\tCan only work on JP with Rush Up. Stamina is vital for the weapon to work at all.\t\t\t\t\t\t\t\t\r\nBullet Saver (Up)\tHave up to a 46% chance with the Z skill to not use up coatings. It does not ignore coating usage entirely for Lavish Attack,\t\t\t\t\t\t\t\t\r\nShiriagari\tThis skill can squeeze some extra raw out but may be contested by other skills especially. Often employed with skill fruits in time attack.\t\t\t\t\t\t\t\t\r\nNot Recommended\t\t\t\t\t\t\t\t\t\r\nVigorous (Up)\tGunners are significantly more fragile, of which you might as well run Adrenaline if you have run a skill dependent on HP. The benefit from the Z skill is also halved.\t\t\t\t\t\t\t\t\r\nEncourage +2\tHard to reliably obtain for Gunners.\t\t\t\t\t\t\t\t\r\nFocus / Trained +2\tIt doesn't do anything here.\t\t\t\t\t\t\t\t";

    public static string GetGameWeaponUnlocks => @"Q: How do I unlock Tonfas/SA-F/Magnet spike?

A: Tonfas and SA-F are unlocked as soon as you enter G rank, all you have to do is make one of either weapon, equip it and then speak to the Guildmaster to unlock the styles for it.

Magnet spike has its own quest line to unlock:
1. Talk to the smithy
2. Talk to Graham (The magnet spike Rasta in the Rasta bar)
3. Talk to the apprentice cat at the entrance of Mezeporta
4. Go back and talk to Graham
5. Talk to the G rank quest receptionist and there should be a quest to hunt G Rank Gougarfs
6. Talk to Graham again then the combiner (by the store)
7. Hunt the Rebidiora at the top of the quest list
8. Talk to Graham, the Smithy, Graham again, then the Road quest receptionist
9. Post the GHC Rukodiora and kill it as hard as you can.
10. Talk to Graham and then the Smithy

After all that you’ve unlocked magnet spike! You should get a material to make the “Protospike α”";

    public static string GetGameMeleeStats => "Attacking Monsters\u3000【Melee Weapons】\r\n●\u3000Weapon Multiplier\r\nWeapons\tMultiplier\r\nGS/LS\t4.8\r\nSnS/DS\t1.4\r\nHammer/Hunting Horn\t5.2\r\nLance/Gunlance\t2.3\r\nTonfa\t1.8\r\nSwitch Axe\t5.4\r\nMagnet Spike\t1.2\r\nBowgun/Bow\t1.2\r\nThe weapon multiplier is a number that represents the inherent strength of the weapon and is the basis for calculating the damage to the monster from the attack.\r\nThe attack power shown in the status is this weapon multiplier multiplied by a certain factor.\r\nThe multipliers for each weapon are shown in the table to the left.\r\n※The weapon multiplier can be obtained by dividing the attack power shown in the status by the multiplier shown on the left\r\n\r\nAny increase in attack power due to food effects, item use, etc. will be added to this weapon multiplier by a fixed value.\r\nTherefore, in terms of status, the amount of increase in attack power depends on the type of weapon.\r\n\r\nEx：Bloodred Drgn Sword （Weapon Multiplier：170）\u3000170×4.8＝816 Attack Power\u3000816÷4.8＝170\r\n\r\n(In some cases, the statuses may not be accurately divisible back due to rounding differences when displaying statuses.\r\nEx：Ophion BladeLV5（Weapon Multiplier：154）\u3000154×4.8＝739.2＝739 Attack Power\u3000739÷4.8＝153.96\r\n\r\n●\u3000Power of Motion Values\r\nThe same weapon can have different power depending on how it is used to attack.\r\nThe weapon multiplier multiplied by the power of each action as a percentage is the base power.\r\nThe power of each action is described on a separate page. (Untranslated section of damage Calc)\r\nEx：Bloodred Drgn Sword In the case of a vertical slash (power: 28), the basic power is 170 x 28% = 47.6 = 47\r\n\r\nAlso, depending on the type of weapon, the power will be corrected.\r\nEx：Blackbelt Sword JumpSlash（Power：18)\u3000150 x 18% x correction 125% = 33.75 = 33 is the base power\r\n\r\n●\u3000Affinity Rate\r\nAffinity rate is the probability of a critical hit when attacking.\r\nOn a critical hit, if the affinity rate is greater than 0%, the base power is multiplied by 1.25; for negative values, the base power is multiplied by 0.75.\r\n\r\nIf the hit rate is a positive value greater than 0%, the hit rate will fluctuate depending on the sharpness level.（後述：切れ味倍率）\r\nAffinity rate on the status page is displayed as fixed, but it actually depends on the level of sharpness during a quest.\r\nIn addition, a crit will not be triggered when the sharpness level is 2 (yellow) or lower.\r\n※Skill: When the plus value of the affinity rate is obtained by the expert skill, it will also be affected by the slashing ability level. (However, this does not apply when the positive value is obtained by starving wolf or SP compensation.\r\n\r\nEx：Bloodred Drgn Sword If a critical hit is made with a step-in slash\r\n\u3000\u3000 170×28%×1.25＝59.5＝59 is the base power\r\n\r\n●\u3000Additional damage by elements\r\nWhen attacking with weapons with normal elements (fire/water/lightning/dragon/ice), element damage is added in addition to normal damage.\r\nComposite elements include multiple elements in one weapon. ○ Element strengthening skills are applied to each.\n\n" +
                "The element value shown in the status is the actual element value multiplied by 10, which is 1/10 of the actual element value when calculating the damage.\r\n\r\nWhen a strengthening skill such as \"Fire Attack\" is activated, the element value will be increased.\n\n" +
                "●\u3000Abnormal Status Attacks\r\nWhen attacking with a weapon with an abnormal status attribute (poison, paralysis, sleep), there is a 1/3 chance of an attack with the abnormal attribute added.\r\nThe attribute values shown in the status are multiplied by 10 times the actual attribute values, and are calculated as 1/10 of the actual value when accumulating.\r\nIf the Status Attack Up skill is activated, the attribute value is multiplied by 1.125.\r\nIn the case of dual attributes (normal element + abnormal status), damage from the normal attribute is added even if the abnormal attribute is activated.\r\nIf the accumulation exceeds the resistance value, the monster will fall into the status condition.\r\nWhen the condition is removed after a certain period of time, the monster becomes more resistant to that attribute, thus increasing its resistance value.\r\nThe accumulated value decreases over time.\r\n\r\nWhen a monster is poisoned, the amount of poison accumulated returns to zero, but it will accumulate again if a poison attack is performed while the monster is poisoned.\r\nThe amount of accumulation stops at the current resistance value -1, but the amount of accumulation does not decrease during the poisoned state.\r\nThis accumulated amount remains unchanged even after the poison state is removed, so the monster can be continuously poisoned by performing a poison attack.\r\n\r\nIn the case of paralysis, no accumulation occurs even if a paralyzing attack is performed during paralysis. After the paralyzed state is over, the accumulation will start from 0 again.\r\n\r\nImpact attacks cause the monster to accumulate a stun value, and when the resistance level is exceeded, the monster is stunned for a certain period of time.\n\n" +
                "●\u3000Sharpness Modifier\r\nMotion\tSharpness: yellow or lower\r\nCorrection\tSharpness: Green or higher\r\nCorrection\r\nBeginning of Swing\t0.3x\t1.0x\r\nNormal Hit\t1.0x\t1.0x\r\nEnd of Swing\t0.5x\t1.0x\r\nThe sharpness modifier value is the one that modifies the power depending on which motion of the attack action hits.\r\nIf the sharpness level is 2 (yellow) or less, as shown in the left table, the power will decrease at the beginning and end of the swing of the weapon.\r\nIf the sharpness level is 3 (green) or higher, any motion will be 1.0x.\r\n（GS／LS Normal middle hit will be 1.05 instead of 1.0）\n\n" +
                "●\u3000Hit Motion\r\nWhen you attack a monster, it will perform one of the following motions: \"bounced,\" \"normal,\" \"slow motion,\" or \"hit stop\"\r\nThis motion is judged based on the hitzone values.\r\n(A hit is not repelled because the damage dealt is low, it is repelled because it cut a hard part of the monster)\r\n\r\nFormula：Part Value×Bounced Multiplier×Sharpness Modifier x Weapon Correction\r\n\r\nGauge\tBounce\tNormal\tSlow Motion\tHit Stop\r\nBlue or Lower\t23 or less\t24+\t45+\t100+\r\nWhite or Higher\t16 or less\t17+\t31+\t70+\r\nSlow Motion, Number of frames generated at hit stop （30 frames = 1 second)\n\n" +
                "Events that occur while a hit stop is in progress.\r\n・Natural recovery of HP, suspension of reduction due to terrain, poison, etc.\r\n・Natural recovery of stamina, stop depletion by motion\r\n・Stopped hunting horn song duration\r\n・Stopped reduction of the effect time of items with timed effects\r\n・No Damage\n\n" +
                "No. of Frames\tSlow Motion\tHit Stop\r\nGS\t6\t10\r\nLS\t2\t3\r\nLS：\r\nKai blade Slash / Penetration 1 hit\t1\t2\r\nSnS\t2\t4\r\nDS\t1\t3\r\nDS（Demon Dance)\t2+1x8+1\t3+1x8+3\r\nDS（True Demon Dance)\t1x3,1*3\t1x3,1*3\r\nHammer\t4\t7\r\nHammer：rotate 1hit\t1\t2\r\nHH\t4\t8\r\nLance\t2\t5\r\nGunlance\t2\t5\r\nBow(Arrow Cut)\t2\t5\r\nBow(Shoriyuken)\tFixed 9+11+1x3+2\n\n" +
                "●\u3000Increased Damage Dealt\n" +
                "Condition\tMultiplier\r\nDuring Paralysis\t1.1x\r\nImpersonating a Rock (Basarios)\t1.1x\r\nPitfalls (Only between falling\r\nand starting to struggle)\t1.1x\r\nSleep (first attack only)\t3.0x\n\n" +
                "If you attack a monster in a certain state, the damage dealt will increase as shown above.\r\n(Only non-element damage increases, element damage does not change)\r\nWhen caught in a shock trap, it is not paralyzed but shocked, so it does not increase.\r\nIf you put it in a pitfall in an undiscovered state, the effect time will be extended by about 1.7 times. In addition, when angry, it will be about 0.7 times. (Priority is given to undiscovered cases)\r\n(Rajang always stays at 6 seconds even if he is asleep when you lay down the trap)\r\nIf the monster is weak and sleeping, it is the same as when put to sleep. But, the monster's HP is gradually recovering.\r\n(except for Espinas' status at the beginning of the quest)\n\n" +
                "●\u3000Damage reduction due to part values and overall defense rate\r\nMonsters have a set part value, which reduces damage.\r\nNormal damage and element damage are each reduced by the part value.\r\nIn addition, monsters have an overall defense rate that is separate from their part values, which reduces the total damage value.\r\nFurthermore, there are monsters whose overall defense rate is corrected when they become enraged.\r\n\r\nEx：A vertical slash to the head of Rathian (total defense rate: 0.9) with a green sharpness Bloodred Drgn Sword\r\n\u3000\u3000 170×28%×1.125×70%＝37.485＝37\r\n\u3000\u3000 48 Dragon×35%＝16.8＝16\r\n\u3000\u3000 （37＋16）×90%＝47.7＝47\r\n\u3000\u3000 47 total damage\r\n\r\n●\u3000Increased player attack power\r\nThere are many ways to increase a player's offensive power, but they can be broadly divided into three groups.\r\n\u3000\u3000１：Activation of skills that increase attack power, possession of items that increase attack power (Powercharm, Powertalon)\r\n\u3000\u3000２：Up due to food effects, use of demon drug and mega demon drug\r\n\u3000\u3000３：Use of power seeds, power pills, and demon flute\r\n\r\nGroup 1 will not lose potency even if you run out during a quest.\r\nGroup 2 is effective all the time during the quest, but loses its effectiveness when you faint.\r\nGroup 3 loses potency after a certain amount of time.\r\n\r\nGroups 2 and 3 do not overlap in their respective effects within the group. In the event of overlap, the one with the greater effect takes precedence regardless of the order.";

    public static string GetGameBowgunStats => "Attacking Monsters\u3000【Bowgun】\r\n●\u3000Weapon Multiplier\n" +
                "Weapons\tMultiplier\r\nGS/LS\t4.8\r\nSnS/DS\t1.4\r\nHamer/Hunting Horn\t5.2\r\nLance/Gunlance\t2.3\r\nBowgun/Bow\t1.2\n\n" +
                "The weapon multiplier is a number that represents the inherent strength of the weapon and is the basis for calculating the damage to the monster from attacks.\r\nThe attack power shown in the status is this weapon multiplier multiplied by a certain factor.\r\nThe multipliers for each weapon are shown in the table to the left.\r\n※The weapon multiplier can be obtained by dividing the attack power shown in the status by the multiplier shown on the left\r\n\r\nAny increase in attack power due to food effects, item use, etc. will be added to this weapon multiplier by a fixed value.\r\nThus, in terms of status, the amount of increase in attack power depends on the type of weapon.\n\n" +
                "In the case of bowguns, the power varies depending on the bullet.\r\nThe base power is the weapon multiplier multiplied by the power of the bullet as a percentage.\r\nCrag Shots explode after impact, while Clust Shots scatter their small bombs.\r\nThe explosion that occurs after landing is fixed damage and is not reduced by part values, but the fire attribute part is reduced by part values.\r\nIf the recovery shot hits a monster, it will not recover. (Damage 1 will be incurred.)\r\n\r\nThe power of heavy bowguns with compressed reloads, etc., depends on the number of bullets loaded.\r\n\u3000※The impact damage is non-elemental and fixed, but the explosion damage after the impact of the Crag Shot changes depending on the number of bullets.\r\n\r\nEx：When shooting LV2 normal bullets with a grenade launcher\u300090×12%＝10.8＝10 is the base power\n\n" +
                "●\u3000Bullet Reload Speed\n" +
                "Reload Speed Value\tMotion\r\n4 or less\tVery Fast （62）\r\n５ ～ ７\tNormal （78）\r\n8 or more\tVery Slow （103\n\n" +
                "The speed at which bullets are reloaded into the bow gun is divided into three stages as shown in the table on the left, depending on the result of the following formula.\r\n\r\n\u3000\u3000\u3000Reload Speed Value ＝ Reload value of bullets － (Reload modifier for bowgun + modifier from loading speed skill)\r\n\r\nThe value in parentheses in the Motion column is the number of frames required to reload. (30 frames = 1 second)\n\n" +
                "Bowgun reload modifier\r\nReload\tModified Value\r\nVery Slow\t0\r\nSlow\t1\r\nNormal\t2\r\nFast\t3\r\nVery Fast\t4\n\n" +
                "Modified value by loading speed skill\r\nSkill\tModified Value\r\nReload Speed-1\t-1\r\nNo Skill\t0\r\nReload Speed+1\t1\r\nReload Speed+2\t2\r\nReload Speed+3\t3\n\n" +
                "●\u3000recoil when fired\r\nRecoil Value\tMotion\r\n０ ～ ８\tSmaller （26）\r\n９ ～ １０\tMedium （56）\r\n１１ ～ １４\tVery Large （72）\n\n" +
                "The recoil generated when a bullet is fired is divided into three stages as shown in the table on the left, depending on the result of the following formula.\r\n\r\n\u3000\u3000\u3000Recoil Value ＝ Recoil value of the bullet - (bowgun recoil modifier + recoil reduction skill modifier)\r\n\r\nThe value in parentheses in the Motion column is the number of frames required for the recoil to subside. (30 frames = 1 second)\r\n\r\nNote that when the rapid fire skill is activated, the two modifiers are set to 0, and the recoil value of the bullet becomes the firing recoil value as it is.\r\nTherefore, most bullets will recoil more than without the skill.\n\n" +
                "Bowgun recoil modifier\r\nRecoil\tModified Value\r\nVery Large\t0\r\nLarge\t1\r\nMedium\t2\r\nSmall\t3\n\n" +
                "Modified by recoil reduction skill\r\nSkill\tModified Value\r\nNo Skill\t0\r\nRecoil Reduction+1\t2\r\nRecoil Reduction+2\t4\n\n" +
                "●\u3000Basic power modification by distance\r\nThe basic power of the Norm S, Pierc S, and Crag S varies with the distance after firing.\r\n\r\n\u3000\u3000Normal bullets change 1.5x ⇒ 1.0x ⇒ 0.8x ⇒ 0.5x after firing\r\n\u3000Pierce bullets and Clust bullets change after firing: 1.0x ⇒ 1.5x ⇒ 1.0x ⇒ 0.8x ⇒ 0.5x\r\n\u3000*1.7x instead of 1.5x for heavy bowguns\r\n\r\nThe graph below shows changes in power over time. Depending on the bow gun and processing level, etc.\r\nThere is a difference in bullet velocity, so the graph below is corrected for bullet velocity.\r\nThe faster the bullet speed, the more the graph is compressed horizontally, and the slower it is, the more it expands.\r\nBy the way, many heavy bowguns shoot faster than light bowguns.\r\n\r\nDue to the correction, when viewed in terms of time, the faster the bullet speed, the shorter the time period during which high power is exerted,\r\nFrom a distance perspective, the distance at which high power is exerted is almost the same regardless of the bowgun or processing level.\r\nTherefore, as long as you know the distance, you can use any bowgun to demonstrate high power at the same distance.\r\n\r\nExcept for the three bullets listed above, the basic power does not change with distance.\r\nThe power of a pellet S does not change with distance, but the number of hits does.\r\nThe various attribute bullets also do not change their attribute values with distance.\n\n" +
                "●\u3000Basic power/attribute value correction by skill\r\nSkill\tEffect\r\nNormal S UP\tNormal bullets are 1.1 times more powerful\r\nPierce S UP\tPierce bullets are 1.1 times more powerful\r\nClust S UP\tClust bullets are 1.3 times more powerful\r\nSniper\tAuto-Reload and +5 to weakness value within critical distance\r\n○Elemental Attack Up\tIncreases the power of each elemental bullet\r\nStatus Attack Up\tThe attribute value of the status bullet is 1.125x\n\n" +
                "When the skills shown in the left table are activated, the power and attribute values of the bullets are increased.\n\n" +
                "●\u3000Affinity Rate\r\nAffinity rate is the probability of a critical hit when attacking.\r\nOn a critical hit, if the affinity rate is greater than 0%, the base power is multiplied by 1.25; if it is a negative value, the base power is multiplied by 0.75.\r\n\r\n●\u3000Hit Effects\r\nThe bowgun's hit effect varies depending on the change in power after firing.\r\nThe effect becomes smaller in the order of 1.5x ⇒ 1.0x ⇒ 0.8x ⇒ 0.5x.\r\n\u3000\u3000※1.7x instead of 1.5x for Heavy Bowgun\r\n(Since it is not affected by the damage dealt, even parts that receive little damage may have a large effect.)\r\n\r\n●\u3000Increased damage dealt\r\nCondition\tMultiplier\r\nParalized\t1.1x\r\nImpersonating a Rock\u3000（Basarios）\t1.1x\r\nPitfall\u3000（Only from the time they fall\r\nto the time they start struggling)\t1.1x\r\nSleep\u3000（First Shot Only）\t3.0x\n\n" +
                "If you attack a monster in a certain state, the damage dealt will increase as shown on the left table.\r\n(Only non-element damage increases, element damage does not change)\r\nWhen caught in a shock trap, it is not paralyzed but shocked, so it does not increase.\r\nIf you put it in a pitfall in an undiscovered state, the effect time will be extended by about 1.7 times. In addition, when angry, it will be about 0.7 times. (Priority is given to undiscovered cases)\r\n(Rajang always stays at 6 seconds even if he is asleep when you lay down the trap)\r\nIf the monster is weak and sleeping, it is the same as when put to sleep. But, the monster's HP is gradually recovering. (except for Espinas' status at the beginning of the quest)\n\n" +
                "●\u3000Damage reduction due to part values and overall defense rate\r\nMonsters have a set part value, which reduces damage.\r\nNormal damage and element damage are each reduced by the part value.\r\nIn addition, monsters have an overall defense rate that is separate from their part values, which reduces the total damage value.\r\nFurthermore, there are monsters whose overall defense rate is corrected when they become enraged.\r\n\r\nEx：When a normal bullet LV2 hits the head of Rathian (overall defense rate: 0.9) at a critical distance with a grenade launcher\r\n\u3000\u3000 90×6%×1.5×50%＝4.05＝4\r\n\u3000\u3000 4×90%＝3.6＝3 Damage\r\n\r\n●\u3000Increased player attack power\r\nThere are many ways to increase a player's offensive power, but they can be broadly divided into three groups.\r\n\u3000\u3000１：Activation of skills that increase attack power, possession of items that increase attack power (Powercharm, Powertalon)\r\n\u3000\u3000２：Up due to food effects, use of demon drug and mega demon drug\r\n\u3000\u3000３：Use of power seeds, power pills, and demon flute\r\n\r\nGroup 1 will not lose potency even if you run out during a quest.\r\nGroup 2 is effective all the time during the quest, but loses its effectiveness when you faint.\r\nGroup 3 loses potency after a certain amount of time.\r\n\r\nGroups 2 and 3 do not overlap in their respective effects within the group. In the event of overlap, the one with the greater effect takes precedence regardless of the order.";

    public static string GetGameBowStats => "●\u3000Arrow Power\n" +
                "There are three types of bow arrows: rapid, spread, and pierce, each ranging from lv. 1 to lv. 4.\r\n\u3000\u3000Rapid: 1 to 4 arrows are shot simultaneously at a point. The higher the firing level, the more arrows are shot.\r\n\u3000\u3000Spread: Shoots multiple arrows in a radial pattern. The higher the spread level, the more arrows are released.\r\n\u3000\u3000Pierce: Shoots arrows that pierce. The higher the pierce level, the greater the number of hits.\r\nThere are four types of bow arcs: Wide, Narrow, Bomb, and Slicing.\r\n\u3000\u3000Wide: Arrows have higher attack power than usual\r\n\u3000\u3000Narrow: higher than usual accumulation value of abnormal status\r\n\u3000\u3000Bomb: high stun value and damage ignores part defense\r\n\u3000\u3000Slicing: calculated by bullet quality, but attacks with the Slicing effect\r\nThe type of bow determines which type/level can be used.\r\nThe arrow power for each type is shown in the table on the left.\r\nThe base power is the weapon multiplier multiplied by the power of the arrow as a percentage.\r\n\r\nAttaching a power coating to an arrow increases its power by 1.5 times.\r\n(1.6 times for gou weapons, 1.7 times for gou weapons when Bow Demon is activated,\r\n\u3000(1.7x for 2 or more parts of Tenran Armor and a HC weapon in the Gou/HC quests)\r\nWhen an explosive coating is attached to an arrow, the stun value accumulates, calculated by the strike quality\n\n" +
                "●\u3000Charge Multiplier\n" +
                "Even with arrows of the same type/level, the base power will change depending on the charging stage.\r\nBelow shows the basic power multiplier for each charge stage.\r\n\u3000(Charge stage 4 can only be used when the Loading UP skill is activated.)\n\n" +
                "Charge Stage\tBasic Power Multiplier\tElement Multiplier\tStatus Multiplier\r\n1\t0.4 x\t0.5 x\t0.5 x\r\n2\t1.0 x\t0.75 x\t1.0 x\r\n3\t1.5 x\t1.0 x\t1.0 x\r\n4\t1.8 x\t1.125 x\t1.0 x\r\nAura 4\t1.0 x\t1.0 x\t1.0 x\r\nAura 5\t1.125 x\t1.1 x\t1.1 x\n\n" +
                "●\u3000Basic power/element value correction by skill\n" +
                "The power and attribute values of arrows are increased when the skills shown below are activated.\n\n" +
                "Skill\tEffect\r\nRapid UP\tPower of rapid arrows 1.1x\r\nPierce UP\tPower of pierce arrows 1.1x\r\nSpread UP\tPower of spread arrows 1.3x\r\n○Elemental Attack Up\tPower up for each element\r\nStatus Attack Up\tIf the attribute value of each coating is 1.125x\n\n" +
                "●\u3000Affinity Rate\r\nAffinity rate is the probability of a critical hit when attacking.\r\nOn a critical hit, if the affinity rate is greater than 0%, the base power is multiplied by 1.25; for negative values, the base power is multiplied by 0.75.\r\n\r\n●\u3000Additional damage by elements\r\nAttacking with a weapon with normal elements (Fire/Water/Thunder/Dragon/Ice) adds element damage in addition to normal damage.\r\nCompound elements include multiple elements in one weapon. ○ element strengthening skills are applied to each.\n\n" +
                "●\u3000Loading a Coating\n" +
                "If you attach a status attribute (poison, paralysis, sleep) coating to an arrow, you can shoot an arrow with the status attribute added.\r\nWhen the status attribute coating is attached, the normal element attached to the bow is nullified.\r\nAttribute values are shown in the table on the left. If you shoot multiple arrows, an attribute value will be added to each one.\r\nUnlike melee weapons, it is always triggered.\r\n\r\nThe attribute value of the status coating is multiplied by 1.125 when the condition attack enhancement skill is activated.\r\nIn addition, the attribute value is halved only at the accumulation stage 1. In the case of Poison Coatings, it will be 1.5 times higher than charge level 3.\r\n\r\nIn the case of explosive coatings, the damage is unattributed and deals a fixed value of damage.\r\n\r\nIn the case of an impact coating, the stun value is halved for charge 1.\r\nDepending on the distance, the stun value varies from 0.7x, 1.0x, and 0.8x.\n\n" +
                "●\u3000Bow Melee Attacks\r\nDescribed Elsewhere\r\n\r\n●\u3000Hit Effects\r\nBow hit effects vary depending on the change in power after firing.\r\nThe effect becomes smaller in the order of 1.5x ⇒ 1.0x ⇒ 0.8x ⇒ 0.5x.\r\n(Since it is not affected by the damage dealt, even parts that receive little damage may have a large effect.)\n\n" +
                "●\u3000Increased damage dealt\n" +
                "If you attack a monster in a certain state, the damage dealt will increase as shown on the left table.\r\n(Only non-element damage increases, element damage does not change)\r\nWhen caught in a shock trap, it is not paralyzed but shocked, so it does not increase.\r\nIf you put it in a pitfall in an undiscovered state, the effect time will be extended by about 1.7 times. In addition, when angry, it will be about 0.7 times. (Priority is given to undiscovered cases)\r\n(Rajang always stays at 6 seconds even if he is asleep when you lay down the trap)\r\nIf the monster is weak and sleeping, it is the same as when put to sleep. But, the monster's HP is gradually recovering. (except for Espinas' status at the beginning of the quest)\n\n" +
                "Condition\tMultiplier\r\nParalized\t1.1x\r\nImpersonating a Rock\u3000（Basarios）\t1.1x\r\nPitfall\u3000（Only from the time they fall\r\nto the time they start struggling)\t1.1x\r\nSleep\u3000（First Shot Only）\t3.0x\n\n" +
                "●\u3000Damage reduction due to part values and overall defense rate\r\nMonsters have a set part value, which reduces damage.\r\nNormal damage and element damage are each reduced by the part value.\r\nIn addition, monsters have an overall defense rate that is separate from their part values, which reduces the total damage value.\r\nFurthermore, there are monsters whose overall defense rate is corrected when they become enraged.\r\n\r\n●\u3000Increased player attack power\r\nThere are many ways to increase a player's offensive power, but they can be broadly divided into three groups.\r\n\u3000\u3000１：Activation of skills that increase attack power, possession of items that increase attack power (Powercharm, Powertalon)\r\n\u3000\u3000２：Up due to food effects, use of demon drug and mega demon drug\r\n\u3000\u3000３：Use of power seeds, power pills, and demon flute\r\n\r\nGroup 1 will not lose potency even if you run out during a quest.\r\nGroup 2 is effective all the time during the quest, but loses its effectiveness when you faint.\r\nGroup 3 loses potency after a certain amount of time.\r\n\r\nGroups 2 and 3 do not overlap in their respective effects within the group. In the event of overlap, the one with the greater effect takes precedence regardless of the order.";

    public static string GetGameSwordCrystalSkills => "The Sword Crystal skill doubles the consumption of the sharpness gauge; one is consumed per hit.\r\nStatus ailments hits will always trigger.\n\n" +
                "Bomb Sword consumes 3 times the sharpness gauge, while other sword crystal skills consume twice as much. 1 consumed per hit.\r\nThe base power used in calculating the damage of bomb sword is calculated by Motion Value x Bomb sword lvl. (The weapon's attack power is irrelevant.)\r\nElement sword crystal are fixed in terms of status.";

    public static string GetGameWeaponTypesInfo => "Weapons\r\nSP Weapons\r\nFor HR5 and G-rank quests, weapon multiplier +10 and +20% affinity.\r\n\r\nHC Weapons\r\nFor hardcore quests, melee weapons will have their sharpness gauge increased by 1 level. Bowguns/bows will have a +40% affinity boost.\r\n\r\nMaster Mark\r\nThe weapon can be Unsheathed and Sheathed 20% faster. Skill: Equivalent to Weapon Handling\r\n\r\nEvolution Weapons\r\nEvolution Weapons（Melee）\r\nPower Sword Crystals can be set. While equipped, the weapon multiplier is 1.2x, and the consumption of the sharpness gauge is 4x.\r\n\r\nEvolution Weapons（Bow）\r\nCan be equipped with an impact coating. The attack becomes a stun attribute and the damage calculation is calculated based on the hit quality. Melee accumulates 2 stun and 4 for arrows.\r\n\r\nEvolution Weapons（Light Bowgun）\r\nAcid S and Dragon Acid S can be loaded. Each bullet has a +10 part quality effect on the part where it hits, which lasts for 30 seconds. Boss monsters only\r\n\r\nGou Weapons\r\nGou Weapons（Melee）\r\nIf your HP and stamina are at maximum, your attacks will not bounce.\r\n\r\nGou Weapons（Bow）\r\nCan equip explosive coatings. While using the explosive coatings, the original attribute will be overwritten and will not be activated.\r\nAll explosove coating damage is fixed damage, and only the overall defense rate is affected.\r\nWeapons are quicker to deploy and retract.\r\nThe effect of the power coating is changed to 1.6 times.\r\n\r\nGou Weapons（Light Bowgun）\r\n・rapid-fire\r\nIf the user presses the \"fire\" key during rapid-fire, the next rapid-fire will be started in succession. (Each press of the fire key consumes ammunition.)\r\nIn that case, the number of rounds fired in rapid fire increases by one round for each additional input. (In the case of normal bullets, 5 rounds -> 6 rounds -> 7 rounds -> ...)\r\nThe recoil after firing changes with the number of times the weapon is fired rapidly, and naturally, the more times the weapon is fired, the more the recoil. (The recoil can be reduced to some extent with recoil reduction).\r\n\u3000For example, with recoil reduction +2, up to 4 shots will have small recoil, 5 or 6 shots will have medium recoil, and the 7th and subsequent shots will have large recoil.\r\n\u3000\u3000\u3000\u3000\u3000\u3000With recoil reduction +1, up to four shots will have medium recoil, and the fifth and later shots will have Large recoil.\r\n\r\nGou Weapons（Heavy Bowgun）\r\n・Heat Beam\r\nIt can be used by attaching the \"Heat Beam\" to a Gou type heavy bowgun.\r\nEach time a bullet is fired, an \"public temperature gauge\" accumulates, and when it reaches its maximum, a \"heat beam\" can be fired.\r\n\u3000：public Temperature Gauge\r\n\u3000・Base 100/minimum 40/maximum 200. Increases by 10 each time you hit with a heat bomb. Or +10 with Warm Oil, -10 with Cooling Oil.\r\n\u3000・The gauge fills up every time you shoot a bullet. After firing the bullet, reduction starts in 3 seconds, and the gauge decreases by 1 per second.\r\n\u3000・After charging is complete, the gauge will flash. Reduction starts 5 seconds after charging is complete.\r\n\u3000・If you reload the heat bomb, it will stop reducing. (Reloading itself is possible even if not accumulated)\r\n\u3000・1 Hit Fixed 22 + 10 fire damage. 6 Hits per 10 gauge (damage is affected by overall defense ratio).\r\n\u3000\u3000\u3000(Gunnery: 24 + fire 10, Artillery Expert: 26 + fire 10, Artillery God: 28 + fire 10 / HBG Tech [Gun Sage] is multiplied by 1.2 for both fixed and fire)\r\n\u3000\u3000\u3000(For initial gauge: 100 ÷ 10 × 6 × 22 = 1320 + fire 600)\r\n\u3000\u3000\u3000※2 Hit limit for small monsters?\n\n" +
                "Tenran / Supremacy (G-rank Supremacy) Weapon\r\nIf you equip 2 or more pieces of Tenran armor and go to a quest for a gou rank/supremacy rank (G-rank),\r\nMelee weapons will have their sharpness gauge increased by 1 level. Bow guns/bows will have their multiplier at critical distance increased by +0.3.\r\nIn addition, for bows, the effect of the power coating changes to 1.7";

    public static string GetGameArmorTypesInfo => "Armor Types\r\nSP Armor\r\nIt does not have unique skills, but it is possible to equip SP decorations. You can also equip accessories with slot 1.\r\nDefense power is basically the same for all, but SP armor that uses Gou material has high defense power.\r\nFor quests with HR 100 or higher, if you equip even one part, your defense will be +100.\r\nHiden Armor\r\nEach weapon has an armor that can be created with SR 300 or more, and even if you have the materials, you cannot produce it if you do not have enough SR.\r\nIf you have both White Hiden and Red Hiden at LV7, you can activate Twin Hiden.\r\nG-class hiden armor\r\nSame as Hiden Armor\r\nGou Armor\r\nWhen equipped, the activated skill will be increased by one rank under certain conditions. (Only those with higher level skills)\r\n\r\nConditions for changing the rank of the activated skill\r\nNo. of Parts Equipped\tHP\tStamina\r\n1 Part\t100%\tMax value 26+\r\n2 Parts\t90%+\tMax value 26+\r\n3 Parts\t83%+\tMax value 26+\r\n4 Parts\t76%+\tMax value 26+\r\n5 Parts\t70%+\tMax value 26+\r\nTenran Armor\r\nIn addition to the effects of gou armor, the following effects are also available in the gou/HC/g-rank quests.\r\n・In the case of Gou/Tenran/HC weapons, the weapon multiplier increases with the number of parts equipped.\r\n\u3000In addition, the rarity limit when equipping a secret book is lowered by one per equipped part.\r\n・Equipping 2 or more pieces will change the performance of Tenran/HC Weapons.\r\nEffect when equipping Gou/Tenran/HC weapons\r\nNo. of Parts Equipped\tWeapon multiplier\tRarity limit for SR\r\n1 Part\t15\t-1\r\n2 Parts\t30\t-2\r\n3 Parts\t45\t-3\r\n4 Parts\t60\t-4\r\n5 Parts\t80\t-5\r\nSupremacy Armor\r\nIn addition to the effects of the Gou/Tenran armor, the Gou/HC/G-rank quests will also include the following\r\n・In the case of the gou/tenran/HC type weapons, the attribute and condition value will be increased by 2% per part.\r\nIn addition, the skill rank up conditions will be relaxed.\r\n\r\nCondition for changing rank of activated skill\r\nThe strength limit will be relaxed by 33% for one part of the supremacy armor and by 6% for one part of the tenran armor.\r\nEx：\r\nIn the case of 2 parts of supremacy + 3 parts of tenran: 100 - 33x2 - 6x3 = 16%+ of HP\r\nG Supremacy Armor\r\nSupremacy armor effects will be activated when you are equipped with a Gou/Tenran/HC/G rank supremacy weapon.\r\nBurst Armor\r\nThe effect of Supremacy Armor will be activated when you are equipped with Gou/Tenran/HC/G-Rank Supremacy/Burst Weapons.\r\nThe skill rank-up condition has been removed, and the skill will always be upgraded.\r\nOrigin Armor\r\nThe effects of Burst armor will be activated when you are equipped with Gou/Tenran/HC/G-rank supremacy/Burst/Origin Weapons.\r\n※However, bowgun/bows are applicable to all weapon types.\r\n・Equipping 1 or more pieces will change the performance of Tenran/HC/G-rank supremacy/Burst/Origin Weapon.\r\n・Weapon multiplier increases by 20 for 1 part, 110 for 5 parts.\r\n・Increases element value and abnormal status value by 3% per part.\r\nAlways has skill rank up.\r\nG Rank Armor\r\nIf you strengthen your GF to LV7, you can refine it into decos.\r\nEach part has a defense correction of (own GR - armor GR) x 20\r\n\u3000※In GR quests, the damage is calculated by subtracting (quest * level - 1) x 150 from the defense, regardless of the armor type.\r\nSkill slots +1 for 3 or more parts; skill slots +2 for 5 parts.\r\n・Weapon multiplier +30 when equipping 3 or more parts in G level quests.\r\nＨＣ Armor\r\nWhen equipped in HC Quest, it will restore your HP to maximum under certain conditions.\r\nHowever, HP must be at least 50%.\r\n\r\nConditions\r\nNo. of Parts Equipped\tMax Stamina\tRecovery\r\n1 Part\t150\t8 seconds for 1 HP\r\n2 Parts\t125+\t4 seconds for 1 HP\r\n3 Parts\t100+\t2 seconds for 1 HP\r\n4 Parts\t75+\t1.5 seconds for 1 HP\r\n5 Parts\t50+\t1 seconds for 1 HP\r\nSafeguard Armor\r\nWhen equipped in HC Quest, it will restore your HP to maximum under certain conditions.\r\n・In HC/Supremacy/G-rank quests, defense power is +20 for each part.\r\n・Damage is reduced in HC/Supremacy/G-rank quests after SR100. (Priority given to Halk Pot)\r\nConditions\r\nNo. of Parts Equipped\tDamage Reduction\r\n1 Part\t10%\r\n2 Parts\t17%\r\n3 Parts\t24%\r\n4 Parts\t27%\r\n5 Parts\t30%\r\nG Safeguard Armor\r\nWhen equipped in HC quests, your HP recovers to the maximum under certain conditions.\r\nEach part has a defense correction of (own GR - armor GR) x 20\r\n\u3000※In the GR quest, the damage is calculated by subtracting (quest * level - 1) x 150 from the defense, regardless of the armor type.\r\nSkill slots +1 for 3 or more parts; skill slots +2 for 5 parts.\r\n・Weapon multiplier +30 when equipping 3 or more parts in G level quests.";

    public static string GetGameItemInfo => "Item\tEffect\r\nSonic Bomb\tProduce a Sound(Impactful)\r\nGook Cracker\tProduce a Sound(Non Impactful)\r\nGook Cracker\r\n(Amulet)\r\nProduce a Sound(Impactful)\r\n??? Doll\tKickable\r\n??? Doll\r\n(Rarely when using Gook Amulet)\r\nBig Kick\r\nGook Fireworks\t3 Shots in the air\r\nGook Fireworks\r\n(Amulet)\r\n5 Shots in the air\r\nRaw Meat\tCan be eaten\r\nOnly some monsters\r\nFlash Bomb\tFlash condition, increased hate\r\nFlute\r\n(HC quest not allowed)\r\nIncreased Hate\r\nGook Whistle\tReduce Hate\r\nGook Whistle\r\n(Amulet)\r\nReduce Hate(Large)\r\nDung Bomb\tMove a Monster\r\n(Only while unseen)\r\nSmoke Bomb\r\n(HC quest not allowed)\r\nHard to spot\r\nSupressing Ball\tRemoves Hate\r\nWeapon\tPower\tNotes\r\nBallista\t100\t2 strikes per 1 hit\r\nCannon\t250\t\r\nDragonator(Fortress）\t255×4 Applied\t10 Min Cooldown\r\nDragonator(Town）\t255×2 Applied\t10 Min Cooldown\r\nDragonator(Schrade）\t255×4x\t10 Min Cooldown\r\nCastle Gate(Schrade）\t255\tOnly Once\r\nBombs are 1.5 times more powerful with the skill \"Bomber\"\r\nThe bomber skill applies to the \"installer\" and the detonation can be done by anyone.\r\nIn addition, a stun value of 2 is added to barrel bombs other than attribute/status abnormal type barrel bombs.\r\n\r\nThe ballista and cannon vary in power with the skill \"Gunnery\".\r\n\r\nBoomerangs have a 1/8 chance of being lost.\r\nSkill \"Throwing Distance UP\" avoids boomerange loss of 1/8\r\n\r\nStrong Arm +1 increases damage of thrown items by 1.1x\r\nStrong Arm +2 further increases thrown item damage by 1.3x\r\n\r\nThrowing Knife +1 allows you to throw 3 knives at once with 1 knife\r\nThrowing Knife +2 allows you to throw 5 knives at once with 1 knife";

    public static string GetGameSharpnessInfo => "Decreased Sharpness／Recovery\r\nAction\tAmt\r\nNormal Attack\t-1\r\nBounced\t-2\r\nGS Guard\t-1～-10\r\nSonic Wave (HH)\t-10\r\nShelling(Normal)\t-3\r\nShelling(Long)\t-4\r\nShelling(Spread)\t-4\r\nWyvern Fire\t-20\r\nSuper Wyvern Fire\t-24\r\nKnife Mackerel\t150\r\nLg Knife Mackarel\t200\r\nMini-Whetstone\r\n(Supplies)\r\n100\r\nWhetstone\t150\r\nG Whetstone\t200\r\nHi-Speed Whetstone\t75\r\nFast USe\r\nHi-Qual Whetstone\r\n(Caravan Only)\r\n400\r\nPerfect Whetstone\t400\r\nFast USe\r\nExpert Whetstone\t150\r\nAffinity +10 for 1 minute\r\nSkill Points：Razor Sharp\" and \"Blunt Edge\" affect sharpness by these amounts\r\n\r\nRazor Sharp+2\tSharpness loss is halved where applicable, 50% chance of any sharpness loss being completely negated.\r\nRazor Sharp+1\tDecrease by half (rounded down, minimum 1)\r\nBlunt Edge\tSharpness loss is doubled.\r\nSkill Points：\"Slothful Sharpening\" activated by the sharpening skill affects the sharpening speed\r\n\r\nSpeed Sharpening\tSharpen with a single stroke of a whetstone.\r\nSlothful Sharpening\tSharpening duration is doubled.";

    public static string GetGameMonsterSizeInfo => "Monster Size\r\nThe monster size is determined by the basic size and variable limit width set for each quest.\r\nBasic size % × fluctuation value % (within fluctuation limit range)\r\n\r\nThe basic size is set for each quest, and is expressed in % inside.\r\nThis is multiplied by the randomly determined variation value % (varying up and down around 100), and the resulting % multiplied by the size of the monster at 100% determines the size.\r\n\r\nThe variation table is first selected from one of the following seven types.\r\n\r\n\r\nFluctuation Range\tChance\r\nTable 1\t75％～79％\t1/32\r\nTable 2\t80％～84％\t1/32\r\nTable 3\t85％～94％\t2/32\r\nTable 4\t95％～105％\t19/32\r\nTable 5\t106％～115％\t6/32\r\nTable 6\t116％～120％\t2/32\r\nTable 7\t121％～125％\t1/32\r\nOnce a variation table is selected, the variation values within that table are randomly determined.\r\nHowever, a variation limit range is set in the quest data, which is modified to fall within the following range\r\n(One of the following five types. Which limit width depends on the quest)\r\n\r\nPattern\tWidth Limit\r\n0\t100％ Fixed\r\n1\t100％～105％\r\n2\t95％～110％\r\n3\t87％～118％\r\n4\t80％～125％\r\nIt may be +10% or -10% depending on some conditions. Those conditions are unknown. (Continuous hunting, good time of day? Come on big guy!)";

    public static string GetGameRewardInfo => "About quest rewards\r\n●\u3000How to view the reward frame\n" +
                "・The top three rows (red boxes) are the basic reward slots you will receive when you achieve the main target.\r\n・The four left frames in the middle row (green frames) are the reward frames you will receive for achieving Sub-Target A.\r\n・The four right frames in the middle row (blue frames) are the reward frames you will receive when you achieve Sub-Target B.\r\n・The bottom two rows (white frame) are the reward frames received for achieving capture and site destruction.\r\n・The bottom three right frames (yellow frames) are, from left to right, NetCafe rewards, premium SR book, and regular SR book frames.\r\n\r\n※Attention\r\nIf more than one of the same monster the target, the reward for capturing and destroying parts of the monster will only be for one monster.\r\nIn addition, the following monsters will be added to the basic reward frame depending on their status\r\n・Additional rewards for making Pariapuria vomit\r\n・Additional reward for tripping White Espinas while in flame circle.\r\n・Additional reward for hunting HC Red Rajang\r\n・HC tickets, souls of each weapon\r\n\r\n２．Legendary Poogie Reward Frame\n\n" +
                "・The three slots on the first tier (red slots) are the boss monster's stripped reward slots.\r\n・The second row of four frames (white frames) are the collection reward frames for that location and season.\r\n\r\n※Attention\r\nThe maximum number of stripped reward slots is 3, even if you hunt multiple boss monsters.\n\n" +
                "※Attention\r\n・When Guild Poogie's \"Poogie Reward Technique\" is activated, the basic reward frame may exceed the upper limit and all 3 rows (24 frames) may become reward frames.\r\n・When a PT (2-4 people) of the same hunting party goes on a quest with different street name titles set for each other, the \"Street Name Title Skill (equivalent to Luck)\" will be activated with a certain probability. (Priority is given to the \"Great Luck\" skill.)\r\n・If you have a Super Lucky Charm, a Large Lucky Charm, or have purchased the Assist Course, it has the same effect as the Great Luck Skill.\r\n・Some quest rewards have fixed slots, in which case only the specified amount of reward slots will appear, regardless of luck skill, etc.\n\n" +
                "●\u3000Obtaining Reward Materials\r\nOnce the number of frames has been determined, reward materials will be picked via RNG from a list according to the probability, and will be placed in order from the first frame.\r\nQuest rewards may have a fixed item, and always only the first slot will have a fixed item, not a RNG.\r\nFinally, each reward is sorted by item code.\r\nAdditional rewards for hunting HC Red Rajang, HC tickets and souls for each weapon will be added afterwards.\r\n\r\nIf you have the following charms, the number of materials obtained will be increased.\r\n・Super Lucky Charm：There is a 50% chance that the materials obtained will be doubled.\r\n・Large Lucky Charm：There is a 15% chance that the materials obtained will be doubled.\r\n\r\nThe \"Super Lucky Charm\" and the \"Large Lucky Charm\" can be used together. When they are used together, the \"Super Lucky Charm\" will activate the \"Super Luck\" skill, and then the \"Large Lucky Charm\" will activate the \"Great Luck\" skill.\r\n\r\n※Attention\r\n・In the case of practice quests, when \"no items\" is selected, the drawing ends there and no further drawings will be made.\r\n・Some quests have a fixed quota of rewards, in which case you only get the specified number of rewards, regardless of your charms, skills, etc.\n\n" +
                "Daily Special\r\nThe following benefits are available once a day (updated at 12:00) in the \"Daily Special\" section of the General Shop.\r\nThe maximum N-point limit is 60,000 P. Others are overwritten each time they are received.\r\n\r\nN points\tTrial course: 1 point\r\nHunter Life Course: +3 points\r\nExtra course: +1 point\r\nPoint Bonus Rights\t3x\r\nGet Halk Pot\t5x\r\nDaily Quest Rights\t1x";

    public static string GetGamePartnerInfo => "Partner\r\nYou can register at the \"Rasta Tavern\" which can be entered from the entry zone in Mezeporta Square.\r\nAccept the quest and bring back materials.\r\nRequirements\r\n\u30001.HR2+\r\n\u30002.Purchased House Expansion 1\r\nRegistering a Partner\r\nTalk to a Legendary Rasta and select \"Partner\" to start partner registration.\r\nThe weapon type of the Legend Rasta you spoke to at that time becomes the partner's initial weapon type.\r\n\u3000(Other weapon types can be acquired by talking to the corresponding Legend Rasta for every PR10.)\r\nOnce you have selected your gender and personality, your partners appearance will be selected in your House.\r\nYou can change your partners appearance at the beauty salon in the plaza, just like a hunter.\r\n\r\nNPC accompany priority\r\nLegendary＞Partner＞Temporary Rasta＞Normal Rasta＞Fosta\r\n\r\nPartner Equipment\r\nThe hunter can \"gift\" equipment to the partner so that they can equip it.\r\nMy Set registration, decos, and Sigils on equipment must be removed before they can be gifted.\r\n\u3000(The gifted equipment will disappear from the hunter's Equipment box and will be stored in the partner's exclusive equipment box.)\r\nEven after the gift is given, equipment can be upgraded by selecting the partner at the blacksmith.\r\n\r\nArmor\r\n・Defense is the same for each type regardless of LV\r\n・No skills or special effects\r\nWeapons\r\n・Weapon power is the same for each type\r\n・Element value is 0, status attacks such as para or sleep are transferred\r\n\u3000(The abnormal status value is halved for tonfa)\r\n・No special effects\n\n" +
                "Partner Growth\r\nBy having them accompany you on quests, your PR (partner rank) and \"weapon proficiency\" will increase.\r\nPR leveling is the same as HR. Obtained PRP calculation is as follows\r\nNormal Quest\r\n・HRP obtained×0.4=PRP\r\n・HRP obtained×0.2=Weapon proficiency\r\nG-rank quest\r\n・GRP obtained×1.2=PRP\r\n・GRP obtained×0.7=Weapon proficiency\r\nWhen the proficiency of one weapon is at 999, the weapon multiplier gains a +5 bonus, which is also applied to other weapons.";

    public static string GetGameRastaInfo => "Trusted Hunter(Rasta)\r\nCan be registered at the \"Rasta Tavern\", which can be entered from the entry and free zones after HR2.\r\n\u30001.Talk to the \"Rasta Receptionist\" and receive an order for \"Hunting an Yian Kut-Ku while logged in.\"\r\n\u30002.Hunt one Yian Kut-Ku. Any quest is fine.\r\n\u30003.Head to the \"Rasta Tavern\" again and talk to the \"Rasta Receptionist\".\r\n\u30004.Receive \"Hunter Knife\" and you will complete \"rasta registration\". Weapon will appear in the empty space in the BOX!\r\n\r\nTrial Rasta\r\nFree trial rasta. You can use it as many times as you want, but you need to make a contract for each quest.\r\nNote that guild contribution points are not accumulated by having them accompany you on quests.It's just a sample.\r\n\r\nTemporary Rastas\r\nFrom the status screen of other hunters, you can make a temporary contract by selecting \"Show sub menu\" > \"Confirm rasta\" > \"Temporary rasta\".\r\nThe contract will continue until you log out.\r\n\r\nLegendary Rastas\r\nA contract can be signed by purchasing assist course. Contracts are made by talking to each legendary rasta at the \"Rasta Tavern.\"\r\nLegendary rastas can be replaced at any time.\r\n\r\nLegendary rasta can participate in all quests except hunter dojo (excluding solo quests, of course).\r\nAs a bonus, great luck is always in effect.\r\nA Legendary Poogie accompanies you and brings you items during quests (better than regular Poogie)\r\nIn addition, one reward screen has been added after the quest ends, and you can randomly obtain materials for gathering, mining, and fishing.\r\nOn that screen, when you hunt a large monster, you can get 3 additional frames of carving materials.\r\n(In the case of HC quests, HC materials are also included.)\r\nContract Rasta\r\nThere are two types of contracts: “accompany contract” and “relief contract”.\r\nHunter B\t←(I want you to come on a quest)←\tHunter A\t\tAccompanying contract (one person only)\r\n→(I'm going on a quest.)→\t\tRelief contract (can contract with up to 3 people)\r\nThe \"accompaniment contract\" made by Hunter A becomes a \"relief contract\" from the perspective of Hunter B.\r\nThe contract should have both characters facing each other. (Additional account characters as the main, is not allowed.)\r\n・Contract Procedure\r\n\u30001.The person making the \"relief contract\" sits in the \"relief contract\" chair.(a chair with a backrest).\r\n\u30002.The person making the \"accompanying contract\" sits in the \"accompanying contract seat.\" (round chair)\r\n\u30003.If both parties select \"Yes\" on the contract screen, the contract will be completed.\r\n\u30004.The party who took the \"Accompanying Contract\" needs to set \"Rasta Accompaniment\" in the \"Rasta Reception\" section.\r\n※If you do not set \"rasta accompany\", Rasta will not come during the quest and you will not accumulate guild contribution points, so be careful not to forget to set it※\n\n" +
                "Rasta Accompaniment\r\nIf the \"Rasta Accompaniment\" option is set in \"Rasta Reception\", a rasta will accompany you during the quest.\r\n※If you do not set \"rasta accompany\", Rasta will not come during the quest and you will not accumulate guild contribution points, so be careful not to forget to set it※\r\nAcoompany Priority\r\n1：Legendary Rasta\r\n2：Temporary Rasta\r\n3：Rasta (including Instructor Rasta and Secret Instructor)\r\n4：Fosta\r\n\r\n◎Notes\r\n・If the maximum number of participants in the quest is reached, Rastas will not join you.\r\n\u3000If quest is set to four and three hunters join, one of someone's rastas will be chosen at random.\r\n\u3000If quest is set to two and two hunters join, Rastas will not join.\r\n\u3000Rasta will not appear with Single-person, item restrictions, weapon and armor restrictions, combat maneuvers, solo only, etc.\r\n・Compensation for quests is also shared with rasta\r\n\u3000Example: If you clear Counterattack Solo (with 2 or more participants) with a Rasta, the reward will be halved. (The rasta portion will be lost.)\r\n\u3000※The reward money that disappeared does not go to Rasta, it just disappears.\r\n・If you don't want to lose rewards, just reduce the number of quest participants. (No issues with GRP, amount does not change)\r\n\r\nGuild Contribution Points\r\n・1 point will be added to you and your Rasta companion hunter for every minute you participate in a quest.\r\n・If you set \"rasta to accompany\", you can get points even if the rasta does not actually participate in the quest.\r\n・Points will not be awarded if:\r\n\u3000\"Rasta accompany\" is not set\r\n\u3000Retire or fail the quest\r\n\r\nObtaining materials from a rasta\r\n・When your rasta accompanies another hunter on a quest, he/she brings back materials from the monster he/she hunted.\r\n・Materials brought back by the Rasta can be obtained from Rasta's receptionist at the Rasta Tavern, \"Receive materials\".\r\n・You can receive it twice a day (once from 0:00 to 11:59, once from 12:00 to 23:59). Stores up to 5\r\n\r\nCancelling\r\nCancellation is done by talking to the \"Rasta Reception\".\r\n※Cancellation becomes possible after one week from the signing of the contract.\r\n\r\nForced Cancellation\r\nWhen 5 maintenances have passed since the last quest accompaniment\r\n※Note that if you go on a quest with the hunter you contracted from, you are not considered to have \"accompanied\" his rasta.\r\n\u3000But since you can go on a quest where the other hunter is not there, forced cancellation does not happen often.\n\n" +
                "Rasta Enhancement\r\n・The same settings as yours are used for appearance. (Cannot be changed)\r\n・Weapons and armor can be equipped with items that you own.\r\n・It is possible for you and Rasta to be equipped with the same weapons and armor.\r\n※However, items equipped by Rasta cannot be upgraded or sold.\r\n\r\nUnlocking Weapon Types\r\n・Unlock Procedure\r\n\u30001.Select \"Release rasta function\" → \"Unlock weapon type\" → \"Hunting order required for release\"\r\n\u30002.Select the target monster from the \"Hunting order list\" and accept the order. (only one at a time)\r\n\u30003.Hunt the target monster, regardless of whether it is a high or low rank monster.\r\n\u30004.Select a weapon from \"Release rasta function\" → \"Unlock weapon type\" → \"Select weapon type to release\"\r\n\u3000\u3000※The number of target monsters cleared is the number of weapon types that can be opened. Any of them can be opened in any order.\r\n\r\nTarget monster\r\n・Berukyurosu\r\n・White Hypnoc\r\n・Kamu Orugaron\r\n・Nono Orugaron\r\n・Orange Espinas\r\n・Akura Vashimu\r\n・Pariapuria\r\n・Rajang\r\n・Volganos\r\n・Rathian\r\nWeapon LV and Armor LV release\r\n・Release Procedure\r\n\u30001.Select \"Release Rasta Functions\" and then \"Release Armor LV.\"\r\n\u3000\u3000Guild loyalty UP: By releasing it, you can draw out attack power and attribute attack power\r\n\u3000\u3000Guild reliability UP: By releasing it, you can draw out defense power and the number of skill activations\r\n\u30002.Pay guild contribution points according to open LV\r\n\r\nOpen Lv\tGCP\r\n1\tUnlocked\r\n2\t200GP\r\n3\t300GP\r\n4\t500GP\r\n5\t500GP\r\n6\t700GP\r\n7\t700GP\r\n8\t800GP\r\n9\t900GP\r\n10\t1000GP\r\n11\t1100GP\r\n12\t1200GP\r\n13\t1200GP\r\n14\t1200GP\r\n15\t1200GP\r\n16\t1200GP\r\n17\t1200GP\r\n18\t1200GP\r\n19\t1200GP\r\n20\t1200GP\n\n" +
                "Behavior During Quests\r\n・If they take more than a certain amount of damage, they will temporarily leave the quest and return to the quest after a certain amount of time has passed.\r\n\u3000\u3000※If they leave, it will not be included in the death count.\r\n・Rastas HP is affected by Bentos\r\n・Action priority (just a guess)\r\n\u3000１．Received a status ailment attack: Use an item or remove it quickly\r\n\u3000\u3000※Snowball and crystals are removed quickly. Poison and stench use items.\r\n\u3000\u3000※Only while under stench, the rastas may use deodorant balls on hunters (low priority).\r\n\u3000\u3000※May cancel hunter's earplugs and stunned status\r\n\u3000２．Boss monster in the area: Attack the boss monster\r\n\u3000\u3000※Throw paintball first (will not throw if rasta has psychic skills)\r\n\u3000\u3000※They cannot defeat boss monsters （will be stuck at about 10 HP)\r\n\u3000\u3000※Prioritize attacking medium-sized monsters.\r\n\u3000\u3000※don't attack monsters while they are sleeping\r\n\u3000３．Boss monster attacks: Guard if it is possible to guard (performance is lower than auto guard)\r\n\u3000４．Miscellaneous small monsters nearby: Attack the miscellaneous small monsters.\r\n\u3000５．There is a monster corpse nearby: Carve it\r\n\u3000６．Damage was taken. Attacked. Stamina decreased: use potions, whetstone, and cooked meat.\r\n\u3000７．Hunter moves: Moves by automatically tracking the hunter\r\n\u3000\u3000※When the hunter moves to another area, they use a nearcaster and move together. (Area movement is top priority)\r\n\u3000\u3000※Drink a hot drink / cool drink when moving to the area (if it is required)\r\n\u3000\u3000※if the hunter moves to a place where it can't be followed, it will warp to the hunter's location with a nearcaster.\r\n\r\n・If you continuously attack Rasta, it will perform the action \"Shake Head\"\r\n・Come closer to the hunter with the action \"call over\"\r\n・If a rasta blasts or uses a move that would normally launch a hunter, it will not launch the hunter.\r\n・The bow is shot at charge Lv 3. It may also be shot with a coating. The coating will be refilled in a certain amount of time? replenished in a certain amount of time?\r\n※Legendary Rastas\r\n・Legendary Rasta's attacks do not hit hunters. Hunter attacks do not hit Legendary Rastas\r\n・Will chat with you during a quest and let you know when you can capture a monster.\r\n・Prioritize attacking boss monsters\r\n・Adrenaline +2, Starving Wolf +2 is always in effect\r\n・Boss monsters can be killed.\r\nRasta peculiarities\r\n・Sharpness gauge does not decrease. But they still sharpen.\r\n・will eat meat even if it's not hungry\r\n・Running does not decrease stamina.\r\n・Health may not be affected by bentos?\r\n・Defenses and damage calculations are different from hunters? (Hard to say)\n\n" +
                "Item Lending\r\n・You can borrow items used by Rasta at the Rasta Tavern.\r\n\u3000\u3000※No need to pay it back, no time limit. Simply set it up.\r\n・Guild contribution points are not required for each quest\r\n・There is a limit to the number of these available per quest, but they are replenished each quest.\n\n" +
                "For Legendary Rastas、Dust Set、Flute Set、Each item in the throwing knife set can be set individually.\r\nThe difference between legendary and normal rasta is that the demon flute and armour flute can be reused with legendaries.\n\n" +
                string.Empty;

    public static string GetGameHalkInfo => "Halk\r\nThey will go on quests with you and help you defeat monsters.[HR1～]\r\nUnlike Rasta, it is not included in the number of participants, and 4 halks will participate in a 4-person PT.\r\nBy raising them, you can increase their strength, attack power, etc., and also enable them to attack with elements.\r\nWhen completing hunting quests, halk feathers, halk calls, and Skill Books may be dropped.\r\n\r\nMy Halk\r\nYou can call the halk when you approach the rock in My Support.\r\nWhen the Halk is on the rock, you can feed it.\r\nBy talking to the instructor, you can set up Skill Books and change the halk's name.\r\n\r\nStatus\r\nThe initial value is randomly distributed so that the total of health, attack power, defense, and intellect is 10.\r\n\"Intimacy\"、\"Health\"、\"Attack\"、\"Defense\"、\"Intellect\" Halk will level up when all five values reach the right end of the gauge.\r\nLV1:Max 255\u3000LV2:Max 510\u3000LV3:Max 765\r\nAs they level up, not only do their abilities change but also the halk's appearance changes.\r\n\r\nLV\tAs level increases、Each status in increased\r\nAttack Element\tAttack element when attacking.[None、Fire、Water、Thunder、Dragon、Ice]\r\nStatus Attribute\tStatus Attribute when attacking.[None、Para、Poison、Sleep]\r\nFullness\tState of fullness; cannot be fed when 100.\r\nIntimacy\tAffects the probability of dropping items during the quest.\r\nHealth\tAffects the damage that can be sustained.\r\nAttack\tAffects damage done.\r\nDefense\tAffects the damage received.\r\nIntellect\tAffects the accuracy of Halk's actions.\r\nThe higher the number, the less wasteful actions taken.\r\n\r\nAttack Element\tWhen one of the element reaches 100, the element is determined. The other elements will be 0.\r\nIf you want to change an element, raise another element to 100 and it will change.\r\nThe appearance changes depending on the element.\r\nStatus Attribute\tWhen one of the attributes reaches 100, the attribute is determined. The other attributes will be 0.\r\nIf you want to change an attribute, raise another attribute to 100 and it will change.\r\nTraining\r\nThe increase value is determined by the clear time of a quest\r\n\r\nTrain Health\tIncreases Health\r\nTrain Attack\tIncreases Attack\r\nTrain Defense\tIncreases Defense\r\nTrain Intellect\tIncreases Intellect\r\nNot Training\tIncreases Intimacy\n\n" +
                "Skill Book\r\nA book that may be dropped by the halk at the end of a quest. (Hunting, slaying, capturing)\r\nYou can equip up to 2 by talking to the instructor in My Support.\r\nAt GR150, Halk becomes G-rank and can equip up to 3 skill books.\r\n\r\nBook\tQuest\tEffect Description\r\nLarge Monster Priority\tAll\tThey will give priority to attacking large monsters regardless of the distance from their hunters.\r\nSmall Monster Priority\tAll\tThey will give priority to attacking small monsters regardless of the distance from their hunters.\r\nNear Hunter Priority\tAll\tThey will give priority to attacking near partner hunters.\r\nAlways Aggressive\tAll\tHalk will always attack regardless of whether the partner hunter is sheathed.\r\nItem Defender\tAll\tWhen a partner hunter uses an item, it will attack nearby monsters.\r\nRising Intent\tAll\tIf partner hunter dies twice, Halk's attack power increases.\r\nCarving Defender\tAll\tAttacks monsters around partner hunters while they are carving.\r\nUnescapable\tAll\tAttacks the target that Halk himself has targeted without changing for a while.\r\nHealth Conscious\tAll\tWhen Holk's own HP decreases, he performs his own recovery action.\r\nHelper [Health]\tAll\tWhen the partner hunter is in a low health state, perform a health recovery action.\r\nApplies to non partnered hunters as well\r\nHelper [Antidote]\tAll\tIf the partner hunter becomes poisoned, perform a poison recovery action.\r\nApplies to non partnered hunters as well\r\nHelper [Deoderant]\tAll\tIf the partner hunter becomes stinky, perform deodorant action.\r\nApplies to non partnered hunters as well\r\nGuuku Luck\tAll\tRare gooks are more likely to be encountered during gook quests.\r\nFast Riser\tAll\tShortens the time it takes for Halk to return after he leaves the battlefield.\r\nBomber\tAll\tHalk will set barrel bombs (non-attribute bombs).\r\nCounts as a player setting the bomb\r\nUnflinching\tAll\tWhen Halk receives damage, it becomes less likely that he will be sent flying.\r\nMonster Spotter\t～HR30\tIt sometimes gives the effect of a psychoserum to the hunter.\r\nMapper\t～HR30\tEven without a map, the hunter can see the entire Mini-Map\r\nBreath Ball Attack\tHR31～\tHalk shoots a spherical breath attack.\r\nClaw Attack Focus\tHR31～\tHalk will perform more claw attacks (Status attacks).\r\nMore Pugi Appearances\tHR51～\tShorter intervals between Poogie appearances during quests.\r\nFlying Wyvern Offense\tHR100～\tHalk's attack power will increase slightly against the listed monster type.\r\nFanged Beast Offense\tHR100～\tHalk's attack power will increase slightly against the listed monster type.\r\nPiscine Offense\tHR100～\tHalk's attack power will increase slightly against the listed monster type.\r\nCarapaceon Offense\tHR100～\tHalk's attack power will increase slightly against the listed monster type.\r\nBird Wyvern Offense\tHR100～\tHalk's attack power will increase slightly against the listed monster type.\r\nElder Dragon Offense\tHR100～\tHalk's attack power will increase slightly against the listed monster type.\r\nElemental Beam Attack\tHC\tHalk will shoot a linear breath attack.\r\nDemon Drug Dust\tHC\tGives partner hunters the same effect as demon drug.\r\nApplies to non partnered hunters as well\r\nArmour Drug Dust\tHC\tGives partner hunters the same effect as armorskin.\r\nApplies to non partnered hunters as well\r\nAffinity Added\tHC\tHalk's attacks will sometimes be critical.\r\nImprove Status Attack\tHC\tHalk's status value from claw attacks is increased.\r\nImprove Evasion\tHC\tWhen Halk is attacked, he may evade it.\r\nImprove Attack\tHC\tIncreases Holk's attack element value.\r\nSignal to Halt Attacking\tAll\tHalk will stop attacks if you use the signal function.\r\nImprove Fire Element\tHR100～\tIncreases Halk's fire element attack value. * No effect at all if not the element of your halk\r\nImprove Ice Element\tHR100～\tIncreases Halk's ice element attack value. * No effect at all if not the element of your halk\r\nImprove Water Element\tHR100～\tIncreases Halk's water element attack value. * No effect at all if not the element of your halk\r\nImprove Thunder Element\tHR100～\tIncreases Halk's thunder element attack value. * No effect at all if not the element of your halk\r\nImprove Dragon Element\tHR100～\tIncreases Halk's dragon element attack value. * No effect at all if not the element of your halk\r\nSmall Bomber\tHR100～\tHalk will now set small barrel bombs.\r\nSupporter\tHC\tHalk attempts to break hunter out of any statuses such as Snowman or Crystallisation\r\nElemental Bite\tHC\tHalk will perform a powerful bite attack with elemental values.\r\nBrute Wyvern Offense\tHR100～\tHalk's attack power will increase slightly against the listed monster type.\r\nBreath Ball Support\tHR100～\tHalk attacks with an emphasis on breath ball attacks. *There is no effect unless the breath ball attack is active.\r\nAttack Focus\tHC\tHalk's attack power increases, but his defense decreases.\r\nDefense Focus\tHC\tHalk's defense increases, his attack power decreases.\r\nJungle Attack\tHR31～\tIf the quest destination is the Jungle, Halk's attack power will increase slightly.\r\nDesert Attack\tHR31～\tIf the quest destination is the Desert, Halk's attack power will increase slightly.\r\nSwamp Attack\tHR31～\tIf the quest destination is the Swamp, Halk's attack power will increase slightly.\r\nSnowy Mountains Attack\tHR31～\tIf the quest destination is the Snowy Mountains, Halk's attack power will increase slightly.\r\nVolcano Attack\tHR31～\tIf the quest destination is the Volcano, Halk's attack power will increase slightly.\r\nTower Attack\tHR31～\tIf the quest destination is the Tower, Halk's attack power will increase slightly.\r\nForest and Hills Attack\tHR31～\tIf the quest destination is the Forest and Hills, Halk's attack power will increase slightly.\r\nGreat Forest Attack\tHR31～\tIf the quest destination is the Great Forest, Halk's attack power will increase slightly.\r\nArena Attack\tHR31～\tIf the quest destination is the Arena, Halk's attack power will increase slightly.\r\nGorge Attack\tHR31～\tIf the quest destination is the Gorge, Halk's attack power will increase slightly.\r\nHighlands Attack\tHR31～\tIf the quest destination is the Highlands, Halk's attack power will increase slightly.\r\nTidal Island Attack\tHR31～\tIf the quest destination is Tidal Island, Halk's attack power will increase slightly.\r\nWind Pressure\tHC\tHalk will not react to monster wind pressure attacks.\r\nEarplugs\tHC\tHalk will not react to monster roars.\r\nLone Attack\tHC\tIf there is only one quest member, Halk's attack power and defense will increase.\r\nParty Attack\tHC\tIf there are 4 quest members, Halk's attack power and defense will increase.\r\nGathering Affection\tHC\tHalk will no longer attack but will drop items instead.\r\nDanger Affection\tHC\tYou are more likely to spawn in the area of a Large Monster※Supported quests only。\r\nSecret Area Affection\tHC\tYou are more likely to spawn in the secret area if a map has one.※Supported quests only。\r\nLeviathan Offense\tHR100～\tHalk's attack power will increase slightly against the listed monster type.\r\nRapid Breath Attack\tHC\tHalk will now perform an attack in which he spits out a series of spherical breaths.\r\nEmpower Attack and Def\tHC\tHalk will spend time charging up, increasing attack and defense.\r\nCharging Attack\tHC\tHalk will rarely do a damaging high speed attack.\r\nSmash Attack\tHC\tHalk gains the ability to do a spiralling rush attack.\r\nPolar Sea Attack\tGR\tIf the quest destination is the Polar Sea, Halk's attack power will increase slightly.\r\nSummer Assault\tGR\tHalk's Attack and Defense increase while in Hot Areas.\r\nWinter Assault\tGR\tHalk's Attack and Defense increase while in Cold Areas.\r\nContain Monster\tGR\tLarge monsters are less likely to move areas.\r\nSwift Battle\tGR\tHalk's attack power increases, but decreases as quest time passes.\r\nLong Battle\tGR\tAs the quest time goes on, halk's attack power increases.\r\nPainted Falls Attack\tGR\tIf the quest destination is the Painted Falls, Halk's attack power will increase slightly.";

    public static string GetGamePartnyaaInfo => "Partnya\r\nThey will accompany you on quests and help you defeat monsters. [HR1~]\r\nUnlike Halks, Partnya's are treated as a party member.\r\nBy raising them, you can increase their Health, attack power, etc.\r\nYou will be able to hire more partnyas when the previous one reaches rank 300.\r\n\r\nStatus\r\nThe initial and maximum values of their abilities are determined by the partnya's nature. They also increase by training.\r\nIncreasing the PNR (partnya rank) increases the value of the abilities.\r\nNormal quest: HRP earned x 0.25\r\nG rank quest: GRP earned x 1.25\r\n\r\nHealth\tThis is the Health of the Partnya\r\nAtk Power\tPartnya's attack power.\r\nDef Power\tPartnya's Defense\r\nBravery\tThe higher the number, the more powerful the attack is likely to be than usual.\r\nReflexes\tAffects the accuracy of guarding. The higher the number, the more accurate the guard will be.\r\nLuck\tIf the number is high, the partnya may bring back materials after completing a quest.\r\nNature\tEach nature type has a different method of status growth and different lines of dialogue.\r\nBehavior\tThis is the pattern of behavior during the quest. Each behavior has a different partnya movement.\r\nAttack Type\tThis is the method of attack used by partnyas in a quest.\r\nHappiness\tAffects movement accuracy. The higher the number, the less useless actions you will take.\r\nIf you use a Felvine, it will be maximized.\r\nStatus\tShows what the Partnya is currently doing.\n\n" +
                "Nature\tHealth\tAtk\tDef\tBravery\tReflex\tLuck\r\nPatient\t200\t80\t190\t5\t35\t5\r\nNaughty\t250\t85\t190\t5\t5\t5\r\nHot Blooded\t200\t100\t130\t35\t5\t5\r\nCareful\t200\t100\t130\t5\t35\t5\r\nObedient\t200\t95\t160\t12\t12\t15\r\nSerious\t200\t95\t205\t10\t10\t5\r\nNaughty\t200\t100\t130\t5\t5\t35\r\nCozy\t200\t80\t130\t5\t15\t45\r\nMax Value\r\nPatient\t350\t100\t420\t50\t200\t35\r\nNaughty\t450\t165\t485\t62\t62\t60\r\nHot Blooded\t300\t180\t255\t200\t50\t35\r\nCareful\t250\t165\t255\t90\t200\t35\r\nObedient\t350\t140\t345\t87\t87\t95\r\nSerious\t350\t165\t405\t87\t87\t50\r\nNaughty\t250\t170\t285\t72\t37\t200\r\nCozy\t250\t100\t375\t37\t137\t175\r\n\n\n" +
                "Special Training\r\nYou can do this up to 50 times. After the selection is made, quests must be completed 5 times or failed.\r\nCan be reset using 9228・初心の巻物\r\n\r\nSleep\tHealth＋２\tFE25・スヤスヤまくら\r\nLift\tAtk Power＋１\tFA25・グイグイダンベル\r\nAbs\tDef Power＋３\tFF25・ビヨーンベルト\r\nBox\tBravery＋１\tFB25・ポスポスグローブ\r\nSwing\tReflexes＋１\tFC25・ブンブンステッキ\r\nZen\tLuck＋１\tFD25・クンクンアロマ\r\nColor Characteristic\r\nColor is determined by the partnyas behavior and weapons, it affects the treasures found and recommended use slot for guild trasure cats.\r\nColor can be changed using 7227・得意行動の書\n\n" +
                "Partnya Item Pouch\r\nExtra pouch available only when accompanied by a partnya.\r\nAt first, it is 1 slot, but it increases for every PR100, and the max is 5 slots at PR400.\r\nWith assist course active, +5 slots will be added (max. 10 slots).\r\n\r\nQuest Accompaniment\r\nThe following bonuses will occur if your partnya accompanies you on a quest. Numbers change depending on luck.\r\nHunters' Festival: Bonus of up to +15 souls\r\nGreat Voyage Festival: Up to 1.8x CP and score increase *outdated*\r\n\r\nPartnya dispatch [soul collection].\r\nOnce dispatched, you can receive souls from the partner sign at 12:00 noon(JP) once a day. (Need to deposit souls at NPC after collecting)\n\n" +
                "Material\tSouls\r\nGreen Valor Stone\t200 Souls\r\nEvent Assist Tkt\t100 Souls\r\nConfession Stone\t80 Souls\r\nShining Mystery\t70 Souls\r\nHammerhead Tuna\t50 Souls\r\nGuild Tkt\t50 Souls\r\nSpeartuna\t30 Souls\r\nLg Knife Mackarel\t20 Souls\r\nWell Done Fish\t10 Souls\r\nGlutton Tuna\t10 Souls\r\nHoney\t5 Souls\r\nKnife Mackerel\t5 Souls\r\nFelvine\t5 Souls\r\nSushifish\t5 Souls\r\nPartnya Dispatch [Guild]\r\nIf the guilds rank is 2 or higher, you can send it to the treasure cat in the guild house.\r\nWhen dispatching, match the characteristic color, the higher the partnya rank, the better the rewards.\n\n" +
                "Dispatch Partnya [My Tore]\r\nWhen dispatched to My Tore, they will go to each store to help.\r\n・My Tore Combo：ネコ珠の素・青(Cat Pearl・Blue) Added\r\n・My Tore General：ネコ珠の素・赤(Cat Pearl・Red) Added\r\n・My Tore Adventurer：ネコ珠の素・緑,黄,紫,白,黒,茶,灰,橙,金/銀/銅の肉球手形(Cat Pearl・Green, Yellow, Purple, White, Black, Brown, Gray, Silver, Gold/Silver/Brnz Pawprint Token) Added";

    public static string GetGamePoogieInfo => "My Tore\r\nFarm Manager\r\nYou will receive a manager at HR51, which manager you get is decided by your responses to their questions.（My Tore Mistress）\r\n※The choices in order from left to right will get you which manager. Y = Yes, N = No.\r\nAnswers→Manager\r\n\u3000YYY\u3000\u3000→Middle\r\n\u3000YYN\u3000\u3000→Youngest\r\n\u3000YNY\u3000\u3000→Eldest\r\n\u3000YNN\u3000\u3000→Youngest\r\n\u3000NYY\u3000\u3000→Random\r\n\u3000NYN\u3000\u3000→Random\r\n\u3000NNY\u3000\u3000→Random\r\n\u3000NNN\u3000\u3000→Eldest\r\n\r\nManagerial change event\r\nAfter repeatedly talking to the sisters you want to change to when all three sisters are in My Tore, you are asked if you want to switch when you go to leave leave.\r\nPoogie Points\r\nYou can recieve poogie points by talking to your manager, alternating between 1 and 2 points everyday. You can hold up to 99.\r\nEven if you forget to claim the points, you may still be able to recover them by talking to her. (Up to 14 points)\r\nAfter HR200, you will recieve 10 points for each 100 levels in HR.\r\n\r\nGive Clothing\r\n\r\nItem\tPoints\r\nCool Blue Costume\t30P\r\nLight Orange Costume\t30P\r\nHealthy Cyan Costume\t30P\r\nGraceful White Costume\t30P\r\nAirou Square\r\nThe amount of sales is determined by the cattiness of the Felynes who come to the square.\n\n" +
                "Poogie Ranch\r\n\r\nExpansion\tPoint Cost\r\nObstacle Course\t30P\r\nClimbing Facility\t30P\r\nRunning Facility\t30P\r\nStudy Facility\t30P\r\nDeparture Facility\t30P\r\nPoogie Extension\r\n\r\nItem\tPoint Cost\r\nAdd a Poogie\t30P(Up to 3)\r\nRank up to G rank\t60P\n\n" +
                "※Clothes obtained with poogie points are only displayed 3 at a time, another being revealed each time you make a purchase.\r\nClothes in yellow are premium outfits and can be used for premium skill cuffs.\n\n" +
                "### Poogie Costumes\r\n\r\n| ID | Poogie Costume |\r\n|----|------|\r\n| 0  |First Costume|\r\n| 1  |Kirin Costume  |\r\n| 2  | Smart Costume   |\r\n| 3  | Cheerful Costume   |\r\n| 4  | Wild Costume  |\r\n| 5  |Hypnoc Costume   |\r\n| 6  |  Volganos Costume  |\r\n| 7  | Espinas Costume  |\r\n| 8  | Comrade Costume |\r\n| 9  | Sporty Costume  |\r\n|10  | Lively Costume   |\r\n|11  | Akura Costume |\r\n|12  | Azul Costume   |\r\n|13  |Cool Costume     |\r\n|14  |  Fine Costume   |\r\n|15  | Beru Costume    |\r\n|16  |  Gospel Costume    |\r\n|17  | Winning Costume    |\r\n|18  | Miner Costume |\r\n|19  | Paria Costume  |\r\n|20  | Magisa Costume     |\r\n|21  | Mischievous Costume |\r\n|22  | Gatherer Costume |\r\n|23  |Kamu Costume |\r\n|24  |  Nono Costume  |\r\n|25  | Tasty Costume   |\r\n|26  | Fishing Costume    |\r\n|27  | Legendary Costume  |\r\n|28  |UNK  |\r\n|29  |  Rainbow Costume  |\r\n|30  | Hope Costume   |\r\n|31  | Pokara Costume   |\n\n" +
                "Manager\r\nGifts, Etc.\r\nWhen an item is requested, favor-ability will not increase if it is taken from storage. (In this case, don't give her the item)\r\n「Flower Extract」will be given the next day after the maximum number of seeds have been planted, after which the caretaker will have a conversation with you, saying \"Here is the flower extract.\"\r\nWhen giving an item as a gift, the second item is the key. The first doesn't affect anything.\r\nThe manager responds according to our choices.\n\n" +
                "Special Requests\r\nRequests other than the initial offerings will increase by receiving gifts from the Maitre manager.\r\n\r\nName\tReward\tState\tTarget\r\nBoiled Silver Egg\t3\tInitial\tRathian\r\nHot and Cold Drink\t3\tInitial\tBlangonga\r\nHerbal Medicine G\t2\tInitial\tBasarios\r\nBoiled Golden Egg\t2\t\r\nTigrex\r\nDisposable Earplug\t3\t\r\nLavasioth\r\nHGE Earplugs\t3\t\r\nRajang\r\nFast Whetstone\t3\t\r\nPlesioth\r\nG Whetstone\t5\t\r\nRathalos\r\nRock-steady Fruit\t2\t\r\nYian Garuga\n\n" +
                "My Tore Poogi\r\nBody color/size changes when fed.\r\n\r\nmeals\tEffect\r\nStubborn Shrinking Stew(頑固な縮小シチュー)\tSmaller body size. (up to 0.75 times normal)\r\nMerry Expansion Bowl(陽気な拡張ボーロ)\tIncreased body length. (up to 1.25 times normal)\r\nAll Else\tChanges the body color\r\nOn rare occasions, managers may become ill. Note that when sick, the conversation event to receive missed points does not occur until the manager is healed.\r\n\r\nManager\tMats to cure\r\nEldest\tSleep Herbx3、Ice Crystalx5\r\nMiddle\tSleepyfishx4、Cold Meat Gx3\r\nYoungest\tNutrientsx2、Cool Drinkx1\r\nIf you maximize your favorability, you can get the title of \"I love Poogie\" with \"street name\" titles.\r\n\r\nAbility\r\n・LV：Increases the number of appearances during each quest. Makes it easier to bring items\r\n\u3000(Intelligence + Exercise power + Perseverance) / 5 = Poogie LV (Rounded Down)\r\n・Intelligence: Brings better items. bring items for the situation\r\n・Exercise power: Poogi appears at the right time, such as when your health is low.\r\n・Perseverance: Appears at shorter intervals\r\n\r\nTreasure\r\nCan bring back items you get from mining, catching, gathering, fishing and carving at a (5%?) chance.\r\nThe higher the LV, the easier it is to bring back.\n\n" +
                "Items to be carried(If Intelligence is high, the number to bring may also increase?)\r\n\r\nItem\tNo.\tIntelligence\r\nHerb\t1\tlow\r\nFirst-Aid Med\t1-2\tlow\r\nMega Potion\t1-2\tlow\r\nEZ Max Potion\t1-2\tlow\r\nAncient Potion\t1\tMed\r\nAntidote\t1-2\tlow\r\nHerbal Medicine\t1-2\tlow\r\nRation\t1-2\tlow\r\nGourmet Steak\t1-2\tlow\r\nPower Pill\t1-2\tlow\r\nDemondrug\t1-2\tlow\r\nPower Juice\t1-2\tlow\r\nArmourskin\t1-2\tlow\r\nArmour Pill\t1-2\tlow\r\nThrowing Knife\t1-4\tlow\r\nEZ Barrel Bomb L\t1-2\tlow\r\nSplyLgBarrelBombG2\t1\tHigh\r\nHot Drink\t1-2\tlow\r\nCool Drink\t1-2\tlow\r\nPsychoserum\t1-2\tlow\r\nPaintball\t1-2\tlow\r\nMega Pickaxe\t1-2\tlow\r\nMega Bugnet\t1-2\tlow\r\nFarcaster\t1\tlow\r\nMini-Whetstone\t1-2\tlow\r\nWhetstone\t1-2\tMed\r\nHi-Speed Whetstone\t1\tHigh\r\nNormal S LV2\t10-20\tlow\r\nPierce S LV1\t10-20\tlow\r\nPellet S Lv1\t10-20\tlow\r\nPoison Coating\t5\tlow\r\nPara Coating\t5\tlow\r\nG-rank Poogie\r\nA G-class skill cuff can be equipped at GR 50.\r\nItems held for use can be given at GR 350.\r\nCarried items are: Potions, Mega Potions, Antidotes, Energy Drinks, Power Juice and Max Potions.\r\n（Poogie uses these directly from your item box.）\n\n" +
                "My Tore Felynes\r\nLV\r\nRestrictions on use of stores depending on cat LV (currently at My Tore Adventurer's adventure destination).\r\n(Affection + Cattiness)/5+1=Cat LV (round off fractions)\r\n\r\nAttributes(MAX255)\r\nCats have Affection (affects the items brought back in the adventure) and cattiness (sales of the store).\r\nThe two Attributes increase differently depending on the type of shop.\r\n\r\nShop\tHow it affects lvls\r\nMy Tore Combine\tIncreased cattiness (rarely decreases affection)\r\nMy Tore General Store\tIncreased cattiness (rarely decreases affection)\r\nMy Tore Bar\tRaises an average amount\r\nMy Tore Clothing\tCattiness increases slightly(rarely decreases affection)\r\nMy Tore Adventurer\tIncreases affection (rarely decreases cattiness)\r\nThe type of cat depends on the number of shops, and the number of cats that come to the square at one time depends on the level of the bar.\n\n" +
                "My Tore General Store\r\nItem Name\tCost\tNotes\r\nHealth Drink Mix\t800z\tLV1\r\nHerb\t20z\tLV2\r\nPotion\t66z\tLV2\r\nNutrients\t760z\tLV1\r\nAntidote\t60z\tLV1\r\nImmunizer\t923z\tLV3\r\nDemondrug\t668z\tLV3\r\nMega Demondrug\t2831ｚ\tLV3SR\r\nArmourskin\t578z\tLV3\r\nMega Armourskin\t2696ｚ\tLV3SR\r\nCool Drink\t300z\tLV1\r\nHot Drink\t250z\tLV1\r\nHot & Cold Drink\t450z\tLV2\r\nPsychoserum\t300z\tLV1\r\nHerbal Medicine\t250z\tLV3SR\r\nTranquilizer\t150z\tLV1\r\nPoisoned Meat\t188z\tLV1\r\nTainted Meat\t300z\tLV1\r\nDrugged Meat\t315z\tLV1\r\nHot Meat\t250z\tLV3\r\nHot Meat G\t300z\tLV3SR\r\nCold Meat\t300z\tLV3\r\nCold Meat G\t380z\tLV3SR\r\nAntidote Meat\t200z\tLV3\r\nAntidote Meat G\t280z\tLV3SR\r\nBoomerang\t150z\tLV1\r\nPaintball\t100z\tLV3\r\nSmoke Bomb\t437z\tLV3\r\nPoison Smoke Bomb\t600z\tLV3\r\nFarcaster\t300z\tLV1\r\nDeodorant\t80z\tLV3\r\nTrap Tool\t200z\tLV1\r\nSmall Barrel\t80z\tLV1\r\nLarge Barrel\t210z\tLV1\r\nSm Barrel Bomb\t156z\tLV1\r\nLg Barrel Bomb\t518z\tLV1\r\nHuskberry\t2z\tLV1\r\nNormal S LV2\t2z\tLV1\r\nNormal S LV3\t4z\tLV1\r\nPierce S LV1\t10z\tLV1\r\nPierce S LV2\t23z\tLV1\r\nPierce S LV3\t36z\tLV1\r\nPellet S Lv1\t7z\tLV1\r\nPellet S Lv2\t11z\tLV1\r\nPellet S Lv3\t15z\tLV1\r\nCrag S Lv1\t29z\tLV1\r\nCrag S Lv2\t46z\tLV1\r\nCrag S Lv3\t49z\tLV3SR\r\nClust S Lv1\t27z\tLV1\r\nClust S Lv2\t44z\tLV1\r\nClust S Lv3\t69z\tLV1\r\nFlaming S\t20z\tLV1\r\nWater S\t20z\tLV1\r\nThunder S\t20z\tLV1\r\nFreeze S\t20z\tLV1\r\nRecov S Lv1\t6z\tLV1\r\nRecov S Lv2\t15z\tLV1\r\nPoison S Lv1\t12z\tLV1\r\nPoison S Lv2\t17z\tLV3SR\r\nPara S Lv1\t18z\tLV1\r\nPara S Lv2\t38z\tLV3SR\r\nSleep S Lv1\t12z\tLV1\r\nSleep S Lv2\t25z\tLV3SR\r\nPaint S\t14z\tLV3\r\nEmpty Bottle\t3z\tLV1\r\nPower Coating\t16z\tLV1\r\nPoison Coating\t12z\tLV1\r\nPara Coating\t20z\tLV1\r\nSleep Coating\t12z\tLV1\r\nCricket\t10z\tLV1\r\nWorm\t20z\tLV1\r\nIron Pickaxe\t160z\tLV1\r\nMega Pickaxe\t240z\tLV2\r\nBugnet\t80z\tLV1\r\nMega Bugnet\t120z\tLV2\r\nWhetstone\t80z\tLV1\r\nBinoculars\t50z\tLV3\r\nFlute\t480z\tLV1\r\nBlue Mushroom\t24z\tLV2\r\nNitroshroom\t60z\tLV2\r\nParashroom\t150z\tLV2\r\nToadstool\t75z\tLV2\r\nAntidote Herb\t20z\tLV1\r\nFire Herb\t44z\tLV1\r\nIvy\t75z\tLV1\r\nSap Plant\t24z\tLV1\r\nFelvine\t14z\tLV1\r\nHot Pepper\t44z\tLV1\r\nPower Seed\t280z\tLV2\r\nArmour Seed\t220z\tLV2\r\nDragon Seed\t780z\tLV3\r\nNeedleberry\t22z\tLV1\r\nBomberry\t120z\tLV1\r\nPaintberry\t60z\tLV2\r\nMocha Pot\t300z\tLV1\r\nVelociprey Fang\t82z\tLV2\r\nIoprey Fang\t50z\tLV2\r\nGenprey Fang\t80z\tLV2\r\nGreen Onion\t50z\tLV3\r\nMixed Beans\t150z\tLV3\r\nStubborn Bread\t350z\tLV3\r\nBeans\t880z\tLV3\r\nFish Notes\t30z\tLV3\r\nRice Weevil\t30z\tLV3\r\nPower Lard\t400z\tLV3\r\nLongevity Jam\t980z\tLV3\r\nDry Margarine\t100z\tLV3\r\nMaengwoo Butter\t400z\tLV3\r\nRoyal Cheese\t880z\tLV3\r\nHopi Alcohol\t300z\tLV3\r\nBrass Wine\t600z\tLV3\r\nRed Oil\t100z\tLV3\r\nRare Onion\t250z\tLV3GR600\r\nGodbug\t420z\tLV3GR600\r\nGolden Alcohol\t1280z\tLV3GR600\r\nGold Extract\t1620z\tLV3GR600\r\nPredator Honey\t1940z\tLV3GR600\r\nCE27・ネコ珠の素・赤\t1500z\tパートニャー派遣\r\nEA29・ネコ珠の素・桜\t1500z\tパートニャー派遣\r\nF92D・ネコ珠の素・梅\t1500z\tパートニャー派遣\n\n" +
                "My Tore Combo List\r\nItems for guild special requests can only be created if they are given as a gift from the manager.\n\n" +
                "My Tore Adventurer\r\n\u3000※As Affection rises, the number of frames and the number of takeaways increase (maximum + 1 more takeaway)Needs Verified\n\n" +
                "My Tore Clothing Store\r\nPersonality and Clothing\r\nActivated in the order in which the conditions are met, starting with skill 1\r\n\r\nPersonality\tActivated Skill\r\nShallow and Wide\tActivate up to 4 skills [small] on clothing.\r\nShallow and Narrow\tActivate up to 3 skills [small] on clothing.\r\nDeep and Narrow\tActivate up to two skills [Small] or [Large] on the clothing.\r\nDeep and Wide\tActivate up to 3 skills [Small] or [Large] on the clothing.\r\nOther\tActivate only one skill [Small] or [Large] on the clothing.\r\nSkill\r\n\r\nSkill Name\tEffect\r\n～～王(King)\tA certain probability of bringing back more of the relevant materials.\r\n～～嫌(Reluctant)\tLess chance to bring back the relevant material.\r\n～～爆発(撃沈) Explosion (sinking)\tWhen customers, Felynes, are looking for ~~, there is a probability that it will be added (subtracted) to sales\r\n受け上手(下手) Good (bad) at receiving\tFaster (slower) responses to instructions given to the shopkeeper\r\n怠け上手 Lazy\tMay stop in the middle of the instructions given to the store keeper.\r\n戻り上手(下手) Good (bad) at returning\tAdventurer cat's return time may be shortened (extended)\n\n" +
                "My Tore Bar\r\nThe number of cats coming to My Tore increases.\r\nYou can hear information such as skills related to cat, open-air baths, and personalities of cats in small talk.\r\n\r\nA \"hospitality\" section is added at Lv2.\r\nIf \"Hospitality Settings\" is ON, Other Players will receive an item 10 times a day when they select \"Hospitality\" at the bar.\r\nThe items you will receive are those on the guild special request list of the bar owner.\r\n\r\nItems you can get\r\n・Boiled Golden Egg\r\n・Boiled Silver Egg\r\n・Hot & Cold Drink\r\n・Herbal Medicine G\r\n・Disposable Earplugs\r\n・HGE Earplugs\r\n・Hi-Speed Whetstone\r\n・G Whetstone\r\n・Anti-Flinch Fruit\r\n\r\nAt Lv3, you can get \"Regulars' Gifts\" 3 times a day when you select \"Talk\" at your own bar.\r\n(※You have to talk to them several times. Once you get one again, you need to go on a quest.)\r\n・Perk Token\r\n・Poogie Garden Tkt\r\n・Best Recipe\r\n・Essential Drink\r\n\r\nYou can ask a hunter to introduce you to a Felyne that he or she has hired, and you can hire him or her for your own My Tore\r\nHowever, cat LV, etc. are returned to their initial values.\r\nYou can't introduce a cat that was introduced to you and hired for your bar.";

    public static string GetGameDamageInfo => "Calculation formula: Basic weapon multiplier = attack power ÷ (Great Sword/Long Sword: 4.8, SnS/dual sword: 1.4, hammer/hunting horn: 5.2, lance/gunlance: 2.3, bowgun/bow: 1.2)\r\nWeapon Magnification = [[[ ([Basic Weapon Magnification x Weapon Technique] + Powercharm + Powertalon + Meal/Demon Medicine + Demon Flute/Strength Class + Attack Power Up Skill + SR Correction + Sigil + SP Correction)\r\n\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000×Melody Effect]×Real Power]×Secret] + Bonds + Tenran/Supremacy/G-class Armor Correction\r\n\u3000\u3000\u3000\u3000Attribute value = [(weapon attribute value + sigil) x ○ attribute enhancement x attribute attack x supremacy armor correction] x serious drink\r\n\u3000\u3000\u3000\u3000 Abnormal status value = Abnormal weapon status value x Abnormal status enhancement x Supreme armor correction\r\nAffinity rate = (weapon criticality + criticality UP skill + SR correction + sigil)  *if positive up to this point + sharpness bonus\r\n+ SP Correction + HC Gun Correction + Fatal Fury + Serious + Critical Whetstone\r\n①Slash/Strike\r\nPhysical Damage = Weapon Magnification x Motion Value x Sharpness Magnification x Cutting Method x Weapon Modification x Flesh Quality x Critical Hit Modification\r\nAttribute damage = attribute value x sharpness multiplier x meat quality x attribute melody\r\n② Shots\r\n\u3000Physical Damage = Weapon Magnification x Ammo x Distance Modification x Power UP Skill x Weapon Modification x Flesh Quality x Critical Hit Modification\r\nAttribute damage = attribute value x flesh quality x attribute melody\r\n\u3000\u3000\u3000\u3000\u3000\u3000The power of the bow will also be corrected by charging\r\nTotal\r\nFinal damage = (physical damage + elemental damage) x total defense rate x anger multiplier x HC correction x HC anger correction\r\n\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000※A minimum of 1 physical damage is included\r\nNotation Gray: Bounced, Green: small hitstop, Blue: large hitstop";

    public static string GetGuildCardBackground
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            var optionChosen = s.GuildCardBackground.ToLowerInvariant();
            optionChosen = optionChosen.Replace(" ", "_");

            return "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/guild_card/" + optionChosen + ".png";
        }
    }

    public string GetGameWeaponInformation
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            return s.WeaponTypeInfo switch
            {
                "Sword and Shield" => GetGameSnSInfo,
                "Dual Swords" => GetGameDSInfo,
                "Great Sword" => GetGameGSInfo,
                "Long Sword" => GetGameLSInfo,
                "Hammer" => GetGameHAInfo,
                "Hunting Horn" => GetGameHHInfo,
                "Lance" => GetGameLAInfo,
                "Gunlance" => GetGameGLInfo,
                "Tonfa" => GetGameTOInfo,
                "Switch Axe F" => GetGameSAFInfo,
                "Magnet Spike" => GetGameMSInfo,
                "Light Bowgun" => GetGameLBGInfo,
                "Heavy Bowgun" => GetGameHBGInfo,
                "Bow" => GetGameBowInfo,
                "Melee Stats" => GetGameMeleeStats,
                "Bowgun Stats" => GetGameBowgunStats,
                "Bow Stats" => GetGameBowStats,
                "Sword Crystals" => GetGameSwordCrystalSkills,
                "Types" => GetGameWeaponTypesInfo,
                "Sharpness" => GetGameSharpnessInfo,
                "Damage Formula" => GetGameDamageInfo,
                "Unlocks" => GetGameWeaponUnlocks,
                _ => GetGameSnSInfo,
            };
        }
    }

    public string GetWeaponIcon => GetWeaponNameFromType(this.WeaponType()) switch
    {
        "Sword and Shield" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/sns.png",
        "Dual Swords" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/ds.png",
        "Great Sword" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/gs.png",
        "Long Sword" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/ls.png",
        "Hammer" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/hammer.png",
        "Hunting Horn" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/hh.png",
        "Lance" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/lance.png",
        "Gunlance" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/gl.png",
        "Tonfa" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/tonfa.png",
        "Switch Axe F" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/saf.png",
        "Magnet Spike" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/ms.png",
        "Heavy Bowgun" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/hbg.png",
        "Light Bowgun" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/lbg.png",
        "Bow" => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/bow.png",
        _ => "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/weapon/sns.png",
    };

    public string GetGameArmorSkills
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            return s.ArmorSkillsInfo switch
            {
                "Priority" => GetGameArmorSkillsPriority,
                "Health and Stamina" => GetGameArmorSkillsHealthAndStamina,
                "Attack Skills" => GetGameArmorSkillsAttackSkills,
                "Defensive Skills" => GetGameArmorSkillsDefensiveSkills,
                "Blademaster Skills" => GetGameArmorSkillsBlademasterSkills,
                "Gunner Skills" => GetGameArmorSkillsGunnerSkills,
                "Resistance Skills" => GetGameArmorSkillsResistanceSkills,
                "Abnormal Status Skills" => GetGameArmorSkillsAbnormalStatusSkills,
                "Other Protection Skills" => GetGameArmorSkillsOtherProtectionSkills,
                "Item/Combo Skills" => GetGameArmorSkillsItemComboSkills,
                "Map/Detection Skills" => GetGameArmorSkillsMapDetectionSkills,
                "Gathering/Transport Skills" => GetGameArmorSkillsGatheringTransportSkills,
                "Reward Skills" => GetGameArmorSkillsRewardSkills,
                "Other Skills" => GetGameArmorSkillsOtherSkills,
                "Colors" => GetGameArmorColors,
                "Types" => GetGameArmorTypesInfo,
                _ => GetGameArmorSkillsPriority,
            };
        }
    }

    public string GetGameCompanionInfo
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            return s.CompanionOptionInfo switch
            {
                "Partner" => GetGamePartnerInfo,
                "Rasta" => GetGameRastaInfo,
                "Poogie" => GetGamePoogieInfo,
                "Halk" => GetGameHalkInfo,
                "Partnyaa" => GetGamePartnyaaInfo,
                _ => GetGamePartnerInfo,
            };
        }
    }

    public string GetGameCaravanInfo
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            return s.CaravanOptionInfo switch
            {
                "About" => GetGameCaravanAbout,
                "Skills" => GetGameCaravanSkills,
                "Quests" => GetGameCaravanQuests,
                "Pioneer" => GetGameCaravanPioneer,
                _ => GetGameCaravanAbout,
            };
        }
    }

    public string GetGameGuildInfo
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            return s.GuildOptionInfo switch
            {
                "About" => GetGameGuildAbout,
                "Guild Food" => GetGameGuildFood,
                "Guild Poogies" => GetGameGuildPoogies,
                _ => GetGameGuildAbout,
            };
        }
    }

    public string GetMetadataForImage
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            var metadata = string.Empty;

            if (!s.EnableMetadataExport)
            {
                metadata = string.Empty;
            }
            else
            {
                string guildName;
                string hunterName;
                var dateAndTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

                if (s.GuildName.Length >= 1)
                {
                    guildName = s.GuildName;
                }
                else
                {
                    guildName = "Guild Name";
                }

                if (s.HunterName.Length >= 1)
                {
                    hunterName = s.HunterName;
                }
                else
                {
                    hunterName = "Hunter Name";
                }

                metadata = " | " + hunterName + " | " + guildName + " | " + dateAndTime;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}({1}){2}", this.GetWeaponClass(), GetGender(), metadata);
        }
    }

    public string GetWeaponForGuildCard
    {
        get
        {
            var className = this.GetWeaponClass();
            var lv = GetWeaponNameFromType(this.WeaponType()).Contains("Bowgun") ? GetGRWeaponLevel(this.GRWeaponLvBowguns()) : GetGRWeaponLevel(this.GRWeaponLv());

            if (className == "Blademaster")
            {
                WeaponBlademaster.IDName.TryGetValue(this.BlademasterWeaponID(), out var wepname);
                var address = this.BlademasterWeaponID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();  // gives you hex 4 digit "007B"

                return string.Format(CultureInfo.InvariantCulture, "{0}{1} ({2})", wepname, lv, address);
            }
            else if (className == "Gunner")
            {
                WeaponGunner.IDName.TryGetValue(this.GunnerWeaponID(), out var wepname);
                var address = this.GunnerWeaponID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();  // gives you hex 4 digit "007B"
                return string.Format(CultureInfo.InvariantCulture, "{0}{1} ({2})", wepname, lv, address);
            }
            else
            {
                return "None";
            }
        }
    }

    public string GetWeaponDecos => string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", this.GetDecoName(this.WeaponDeco1ID(), EquipmentSlot.One, true), this.GetDecoName(this.WeaponDeco2ID(), EquipmentSlot.Two, true), this.GetDecoName(this.WeaponDeco3ID(), EquipmentSlot.Three, true));

    public string GetArmorHeadNameForGuildCard
    {
        get
        {
            ArmorHead.IDName.TryGetValue(this.ArmorHeadID(), out var piecename);
            var address = this.ArmorHeadID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", piecename, address);
        }
    }

    public string GetArmorChestNameForGuildCard
    {
        get
        {
            ArmorChest.IDName.TryGetValue(this.ArmorChestID(), out var piecename);
            var address = this.ArmorChestID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", piecename, address);
        }
    }

    public string GetArmorArmNameForGuildCard
    {
        get
        {
            ArmorArms.IDName.TryGetValue(this.ArmorArmsID(), out var piecename);
            var address = this.ArmorArmsID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", piecename, address);
        }
    }

    public string GetArmorWaistNameForGuildCard
    {
        get
        {
            ArmorWaist.IDName.TryGetValue(this.ArmorWaistID(), out var piecename);
            var address = this.ArmorWaistID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", piecename, address);
        }
    }

    public string GetArmorLegNameForGuildCard
    {
        get
        {
            ArmorLegs.IDName.TryGetValue(this.ArmorLegsID(), out var piecename);
            var address = this.ArmorLegsID().ToString("X4", CultureInfo.InvariantCulture).ToUpperInvariant();
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", piecename, address);
        }
    }

    public string GetArmorHeadDecos => string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", GetItemName(this.ArmorHeadDeco1ID()), GetItemName(this.ArmorHeadDeco2ID()), GetItemName(this.ArmorHeadDeco3ID()));

    public string GetArmorChestDecos => string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", GetItemName(this.ArmorChestDeco1ID()), GetItemName(this.ArmorChestDeco2ID()), GetItemName(this.ArmorChestDeco3ID()));

    public string GetArmorArmDecos => string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", GetItemName(this.ArmorArmsDeco1ID()), GetItemName(this.ArmorArmsDeco2ID()), GetItemName(this.ArmorArmsDeco3ID()));

    public string GetArmorWaistDecos => string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", GetItemName(this.ArmorWaistDeco1ID()), GetItemName(this.ArmorWaistDeco2ID()), GetItemName(this.ArmorWaistDeco3ID()));

    public string GetArmorLegDecos => string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", GetItemName(this.ArmorLegsDeco1ID()), GetItemName(this.ArmorLegsDeco2ID()), GetItemName(this.ArmorLegsDeco3ID()));

    public string GetWeaponForImage
    {
        get
        {
            var className = this.GetWeaponClass();
            var lv = GetWeaponNameFromType(this.WeaponType()).Contains("Bowgun") ? GetGRWeaponLevel(this.GRWeaponLvBowguns()) : GetGRWeaponLevel(this.GRWeaponLv());

            if (className == "Blademaster")
            {
                WeaponBlademaster.IDName.TryGetValue(this.BlademasterWeaponID(), out var wepname);
                return string.Format(CultureInfo.InvariantCulture, "{0}{1}", wepname, lv);
            }
            else if (className == "Gunner")
            {
                WeaponGunner.IDName.TryGetValue(this.GunnerWeaponID(), out var wepname);
                return string.Format(CultureInfo.InvariantCulture, "{0}{1}", wepname, lv);
            }
            else
            {
                return "None";
            }
        }
    }

    public string GetWeaponDecosForImage
    {
        get
        {
            var className = this.GetWeaponClass();

            if (className is "Blademaster" or "Gunner")
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} | {1} | {2}", this.GetDecoName(this.WeaponDeco1ID(), EquipmentSlot.One), this.GetDecoName(this.WeaponDeco2ID(), EquipmentSlot.Two), this.GetDecoName(this.WeaponDeco3ID(), EquipmentSlot.Three));
            }
            else
            {
                return "None";
            }
        }
    }

    public string GetArmorHeadNameForImage
    {
        get
        {
            ArmorHead.IDName.TryGetValue(this.ArmorHeadID(), out var piecename);
            return string.Format(CultureInfo.InvariantCulture, "{0} | {1} | {2} | {3}", piecename, this.GetDecoName(this.ArmorHeadDeco1ID(), 0, true), this.GetDecoName(this.ArmorHeadDeco2ID(), 0, true), this.GetDecoName(this.ArmorHeadDeco3ID(), 0, true));
        }
    }

    public string GetArmorChestNameForImage
    {
        get
        {
            ArmorChest.IDName.TryGetValue(this.ArmorChestID(), out var piecename);
            return string.Format(CultureInfo.InvariantCulture, "{0} | {1} | {2} | {3}", piecename, this.GetDecoName(this.ArmorChestDeco1ID(), 0, true), this.GetDecoName(this.ArmorChestDeco2ID(), 0, true), this.GetDecoName(this.ArmorChestDeco3ID(), 0, true));
        }
    }

    public string GetArmorArmNameForImage
    {
        get
        {
            ArmorArms.IDName.TryGetValue(this.ArmorArmsID(), out var piecename);
            return string.Format(CultureInfo.InvariantCulture, "{0} | {1} | {2} | {3}", piecename, this.GetDecoName(this.ArmorArmsDeco1ID(), 0, true), this.GetDecoName(this.ArmorArmsDeco2ID(), 0, true), this.GetDecoName(this.ArmorArmsDeco3ID(), 0, true));
        }
    }

    public string GetArmorWaistNameForImage
    {
        get
        {
            ArmorWaist.IDName.TryGetValue(this.ArmorWaistID(), out var piecename);
            return string.Format(CultureInfo.InvariantCulture, "{0} | {1} | {2} | {3}", piecename, this.GetDecoName(this.ArmorWaistDeco1ID(), 0, true), this.GetDecoName(this.ArmorWaistDeco2ID(), 0, true), this.GetDecoName(this.ArmorWaistDeco3ID(), 0, true));
        }
    }

    public string GetArmorLegNameForImage
    {
        get
        {
            ArmorLegs.IDName.TryGetValue(this.ArmorLegsID(), out var piecename);
            return string.Format(CultureInfo.InvariantCulture, "{0} | {1} | {2} | {3}", piecename, this.GetDecoName(this.ArmorLegsDeco1ID(), 0, true), this.GetDecoName(this.ArmorLegsDeco2ID(), 0, true), this.GetDecoName(this.ArmorLegsDeco3ID(), 0, true));
        }
    }

    public string GetAtkDefForImage => string.Format(CultureInfo.InvariantCulture, "Bloat Attack: {0} | Total Defense: {1}", this.BloatedWeaponAttack().ToString(CultureInfo.InvariantCulture), this.TotalDefense().ToString(CultureInfo.InvariantCulture));

    public string GetStyleRankForImage
    {
        get
        {
            var style = this.WeaponStyle() switch
            {
                0 => "Earth Style",
                1 => "Heaven Style",
                2 => "Storm Style",
                3 => "Extreme Style",
                _ => "Earth Style"
            };

            return string.Format(CultureInfo.InvariantCulture, "{0} | {1}", style, this.GetGSRSkills);
        }
    }

    public string GetZenithSkillsForImage
    {
        get
        {
            var zenithSkills = this.GetZenithSkills;
            zenithSkills = zenithSkills.Replace("\n", ", ");
            return zenithSkills;
        }
    }

    public string GetAutomaticSkillsForImage
    {
        get
        {
            var automaticSkills = this.GetAutomaticSkills;
            automaticSkills = automaticSkills.Replace("\n", ", ");
            return automaticSkills;
        }
    }

    public string GetActiveSkillsForImage
    {
        get
        {
            var activeSkills = this.GetArmorSkills;
            activeSkills = activeSkills.Replace("\n", ", ");
            return activeSkills;
        }
    }

    public string GetDivaSkillForImage
    {
        get
        {
            SkillDiva.IDName.TryGetValue(this.DivaSkill(), out var divaskillaname);
            return divaskillaname + string.Empty;
        }
    }

    // GetArmorSkill(GuildFoodSkill()), GetGSRSkills, GetItemPouch, GetAmmoPouch, GetItemName(PoogieItemUseID()), GetRoadDureSkills
    public string GetGuildFoodForImage => GetArmorSkill(this.GuildFoodSkill());

    public string GetPoogieItemForImage => GetItemName(this.PoogieItemUseID());

    public string GetAlternateMonsterImage(int id, bool forDiscord = false)
    {
        var pathContext = forDiscord ? @"https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/monster/" : @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/monster/";

        switch (id)
        {
            default:
                return pathContext + "random.png";
            case 2:
                if (this.RankBand() == 53)
                {
                    return pathContext + "conquest_fatalis.png";
                }
                else
                {
                    return pathContext + "fatalis.png";
                }

            case 11:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_rathalos.gif";
                }
                else
                {
                    return pathContext + "rathalos.png";
                }

            case 15:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_khezu.gif";
                }
                else
                {
                    return pathContext + "khezu.png";
                }

            case 17:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_gravios.gif";
                }
                else
                {
                    return pathContext + "gravios.png";
                }

            case 21:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_plesioth.gif";
                }
                else
                {
                    return pathContext + "plesioth.png";
                }

            case 36:
                if (this.RankBand() == 53)
                {
                    return pathContext + "conquest_crimson_fatalis.png";
                }
                else
                {
                    return pathContext + "crimson_fatalis.png";
                }

            case 48:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_daimyo_hermitaur.gif";
                }
                else
                {
                    return pathContext + "daimyo_hermitaur.png";
                }

            case 51:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_blangonga.gif";
                }
                else
                {
                    return pathContext + "blangonga.png";
                }

            case 53:
                if (this.RankBand() is 56 or 57)
                {
                    return pathContext + "twinhead_rajang.png";
                }
                else
                {
                    return pathContext + "rajang.png";
                }

            case 65:
                if (this.RankBand() == 32)
                {
                    return pathContext + "supremacy_teostra.png";
                }
                else
                {
                    return pathContext + "teostra.png";
                }

            case 71:
                if (this.RankBand() == 53)
                {
                    return pathContext + "road_white_fatalis.png";
                }
                else
                {
                    return pathContext + "white_fatalis.png";
                }

            case 74:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_hypnoc.gif";
                }
                else
                {
                    return pathContext + "hypnoc.png";
                }

            case 76:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_tigrex.gif";
                }
                else
                {
                    return pathContext + "tigrex.png";
                }

            case 80:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_espinas.gif";
                }
                else
                {
                    return pathContext + "espinas.png";
                }

            case 83:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_akura_vashimu.gif";
                }
                else
                {
                    return pathContext + "akura_vashimu.png";
                }

            case 89:
                if (this.RankBand() == 54)
                {
                    return pathContext + "thirsty_pariapuria.png";
                }
                else if (this.RankBand() == 32)
                {
                    return pathContext + "thirsty_pariapuria.png";
                }
                else
                {
                    return pathContext + "pariapuria.png";
                }

            case 95:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_doragyurosu.gif";
                }
                else if (this.RankBand() == 32)
                {
                    return pathContext + "supremacy_doragyurosu.png";
                }
                else
                {
                    return pathContext + "doragyurosu.png";
                }

            case 99:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_rukodiora.gif";
                }
                else
                {
                    return pathContext + "rukodiora.png";
                }

            case 100:
                if (this.RankBand() is 70 or 54)
                {
                    return pathContext + "shiten_unknown.png";
                }
                else if (this.RankBand() == 46)
                {
                    return pathContext + "unknown.png";
                }
                else
                {
                    return pathContext + "unknown.png";
                }

            case 103:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_taikun_zamuza.gif";
                }
                else
                {
                    return pathContext + "taikun_zamuza.png";
                }

            case 106:
                if (this.RankBand() == 32)
                {
                    return pathContext + "odibatorasu.png";
                }
                else
                {
                    return pathContext + "odibatorasu.png";
                }

            case 107:
                if (this.RankBand() is 54 or 55)
                {
                    return pathContext + "shiten_disufiroa.png";
                }
                else
                {
                    return pathContext + "disufiroa.png";
                }

            case 109:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_anorupatisu.gif";
                }
                else
                {
                    return pathContext + "anorupatisu.png";
                }

            case 110:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_hyujikiki.gif";
                }
                else
                {
                    return pathContext + "hyujikiki.png";
                }

            case 111:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_midogaron.gif";
                }
                else
                {
                    return pathContext + "midogaron.png";
                }

            case 112:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_giaorugu.gif";
                }
                else
                {
                    return pathContext + "giaorugu.png";
                }

            case 113:
                if (this.RankBand() == 55)
                {
                    return pathContext + "shifting_mi_ru.png";
                }
                else
                {
                    return pathContext + "mi_ru.png";
                }

            case 116:
                if (this.RankBand() == 53)
                {
                    return pathContext + "conquest_shantien.png";
                }
                else
                {
                    return pathContext + "shantien.png";
                }

            case 121:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_baruragaru.gif";
                }
                else
                {
                    return pathContext + "baruragaru.png";
                }

            case 129:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_inagami.gif";
                }
                else
                {
                    return pathContext + "inagami.png";
                }

            case 140:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_harudomerugu.gif";
                }
                else
                {
                    return pathContext + "harudomerugu.png";
                }

            case 141:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_toridcless.gif";
                }
                else
                {
                    return pathContext + "toridcless.png";
                }

            case 142:
                if (this.RankBand() is >= 64 and <= 67)
                {
                    return pathContext + "zenith_gasurabazura.gif";
                }
                else
                {
                    return pathContext + "gasurabazura.png";
                }

            case 146:
                if (this.RankBand() is >= 54 and <= 55)
                {
                    return pathContext + "howling_zinogre.png";
                }
                else
                {
                    return pathContext + "zinogre.png";
                }

            case 154:
                if (this.RankBand() is >= 54 and <= 55)
                {
                    return pathContext + "ruling_guanzorumu.png";
                }
                else
                {
                    return pathContext + "guanzorumu.png";
                }

            case 155:
                if (this.RankBand() == 55)
                {
                    return pathContext + "golden_deviljho.png";
                }
                else
                {
                    return pathContext + "starving_deviljho.png";
                }

            case 166:
                if (this.RankBand() is >= 54 and <= 55)
                {
                    return pathContext + "burning_freezing_elzelion.png";
                }
                else
                {
                    return pathContext + "elzelion.png";
                }
        }
    }

    public static string GetCurrentFeriasVersion
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");
            return s.FeriasVersionLink;
        }
    }

    public ObservableCollection<ObservablePoint> DamagePerSecondCollection { get; set; } = new ();

    public ObservableCollection<ObservablePoint> ActionsPerMinuteCollection { get; set; } = new ();

    public ObservableCollection<ObservablePoint> HitsPerSecondCollection { get; set; } = new ();

    public ObservableCollection<long> WeaponUsageEarthStyle { get; set; } = new ();

    public ObservableCollection<long> WeaponUsageHeavenStyle { get; set; } = new ();

    public ObservableCollection<long> WeaponUsageStormStyle { get; set; } = new ();

    public ObservableCollection<long> WeaponUsageExtremeStyle { get; set; } = new ();

    public int PreviousMezFesArea { get; set; } = -1;

    public object AttackBuffSync { get; } = new ();

    public object DamagePerSecondSync { get; } = new ();

    public object ActionsPerMinuteSync { get; } = new ();

    public object HitsPerSecondSync { get; } = new ();

    public object WeaponUsageSync { get; set; } = new ();

    public List<ISeries> AttackBuffSeries { get; set; } = new ();

    public List<ISeries> DamagePerSecondSeries { get; set; } = new ();

    public List<ISeries> ActionsPerMinuteSeries { get; set; } = new ();

    public List<ISeries> HitsPerSecondSeries { get; set; } = new ();

    public List<ISeries> WeaponUsageSeries { get; set; } = new ();

    // since the x axis for all of my graphs is the time elapsed in seconds in a quest, i only need 1 definition
    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            // Now the Y axis we will display labels as currency
            // LiveCharts provides some common formatters
            // in this case we are using the currency formatter.
            TextSize = 12,
            Labeler = (value) => TimeService.GetMinutesSecondsFromSeconds(value),
            NamePaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),
            LabelsPaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),

            // you could also build your own currency formatter
            // for example:
            // Labeler = (value) => value.ToString("C")

            // But the one that LiveCharts provides creates shorter labels when
            // the amount is in millions or trillions
        },
    };

    public Axis[] AttackBuffYAxes { get; set; } =
    {
        new Axis
        {
            NameTextSize = 12,
            TextSize = 12,
            NamePadding = new LiveChartsCore.Drawing.Padding(0),
            NamePaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),
            LabelsPaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),
            Name = "ATK",
        },
    };

    public Axis[] HitsPerSecondYAxes { get; set; } =
{
        new Axis
        {
            NameTextSize = 12,
            TextSize = 12,
            NamePadding = new LiveChartsCore.Drawing.Padding(0),
            NamePaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),
            LabelsPaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),
            Name = "HPS",
        },
};

    public Axis[] DamagePerSecondYAxes { get; set; } =
{
        new Axis
        {
            NameTextSize = 12,
            TextSize = 12,
            NamePadding = new LiveChartsCore.Drawing.Padding(0),
            NamePaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),
            LabelsPaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),
            Name = "DPS",
        },
};

    public Axis[] ActionsPerMinuteYAxes { get; set; } =
{
        new Axis
        {
            NameTextSize = 12,
            TextSize = 12,
            NamePadding = new LiveChartsCore.Drawing.Padding(0),
            NamePaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),
            LabelsPaint = new SolidColorPaint(new SKColor(StaticHexColorToDecimal("#a6adc8"))),
            Name = "APM",
        },
};

    /// <summary>
    /// Gets the objective name from identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetObjectiveNameFromID(int id, bool isLargeImageText = false)
    {
        if (DiscordService.ShowDiscordQuestNames() && !isLargeImageText)
        {
            return string.Empty;
        }

        // TODO dictionary
        return id switch
        {
            0 => "Nothing ",
            1 => "Hunt ",
            257 => "Capture ",
            513 => "Slay ",
            32772 => "Repel ",
            98308 => "Slay or Repel ",
            262144 => "Slay All ",
            131072 => "Slay Total ",
            2 => "Deliver ",
            16388 => "Break Part ",
            4098 => "Deliver Flag ",
            16 => "Esoteric Action ",
            _ => "Nothing ",
        };
    }

    /// <summary>
    /// Gets the objective1 quantity.
    /// </summary>
    /// <returns></returns>
    public string GetObjective1Quantity(bool isLargeImageText = false)
    {
        if (DiscordService.ShowDiscordQuestNames() && !isLargeImageText)
        {
            return string.Empty;
        }

        if (this.Objective1Quantity() <= 1)
        {
            return string.Empty;
        }

        // hunt / capture / slay
        else if (this.ObjectiveType() is 0x1 or 0x101 or 0x201)
        {
            return this.Objective1Quantity().ToString(CultureInfo.InvariantCulture) + " ";
        }
        else if (this.ObjectiveType() is 0x8004 or 0x18004)
        {
            return string.Format(CultureInfo.InvariantCulture, "({0} True HP) ", this.Objective1Quantity() * 100);
        }
        else
        {
            return this.Objective1Quantity().ToString(CultureInfo.InvariantCulture) + " ";
        }
    }

    /// <summary>
    /// Gets the objective1 current quantity.
    /// </summary>
    /// <returns></returns>
    public string GetObjective1CurrentQuantity(bool isLargeImageText = false)
    {
        if (DiscordService.ShowDiscordQuestNames() && !isLargeImageText)
        {
            return string.Empty;
        }

        if (this.ObjectiveType() is 0x0 or 0x02 or 0x1002)
        {
            if (this.Objective1Quantity() <= 1)
            {
                return string.Empty;
            }
            else
            {
                return this.Objective1CurrentQuantityItem().ToString(CultureInfo.InvariantCulture) + "/";
            }
        }
        else
        {
            if (this.Objective1Quantity() <= 1)
            {
                return string.Empty;
            }
            else
            {
                // increases when u hit a dead large monster
                return this.Objective1CurrentQuantityMonster().ToString(CultureInfo.InvariantCulture) + "/";
            }
        }
    }

    /// <summary>
    /// Gets the rank name from identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public string GetRankNameFromID(int id, bool isLargeImageText = false)
    {
        if (DiscordService.ShowDiscordQuestNames() && !isLargeImageText)
        {
            return string.Empty;
        }

        return this.GetRankName(id);
    }

    public string GetDuremudiraIcon(int questID)
    {
        return questID switch
        {
            Numbers.QuestIDArrogantDuremudira => MonsterImages.MonsterImageID[167],
            Numbers.QuestIDArrogantDuremudiraRepel => MonsterImages.MonsterImageID[167],
            Numbers.QuestIDFirstDistrictDuremudira => MonsterImages.MonsterImageID[132],
            Numbers.QuestIDSecondDistrictDuremudira => MonsterImages.MonsterImageID[132],
            _ => MonsterImages.MonsterImageID[132],
        };
    }

    /// <summary>
    /// Gets the monster icon.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public string GetMonsterIcon(int id, bool forDiscord = false)
    {
        // quest ids:
        // mp road: 23527
        // solo road: 23628
        // 1st district dure: 21731
        // 2nd district dure: 21746
        // 1st district dure sky corridor: 21749
        // 2nd district dure sky corridor: 21750
        // arrogant dure repel: 23648
        // arrogant dure slay: 23649
        // urgent tower: 21751
        // 4th district dure: 21748
        // 3rd district dure: 21747
        // 3rd district dure 2: 21734
        // UNUSED sky corridor: 21730
        // sky corridor prologue: 21729
        if (this.RoadOverride() == false)
        {
            id = this.RoadSelectedMonster() == 0 ? this.LargeMonster1ID() : this.LargeMonster2ID();
        }
        else if (this.AlternativeQuestOverride())
        {
            id = this.AlternativeQuestMonster1ID();
        }

        // Duremudira Arena
        if (this.AreaID() == 398 && (this.QuestID() == 21731 || this.QuestID() == 21746 || this.QuestID() == 21749 || this.QuestID() == 21750))
        {
            id = 132;
        }

        // duremudira
        else if (this.AreaID() == 398 && (this.QuestID() == 23648 || this.QuestID() == 23649))
        {
            id = 167;
        }

        // arrogant duremudira
        return this.DetermineMonsterImage(id, forDiscord);
    }

    /// <summary>
    /// Gets the star grade.
    /// </summary>
    /// <param name="isLargeImageText">if set to <c>true</c> [is large image text].</param>
    /// <returns></returns>
    public string GetStarGrade(bool isLargeImageText = false)
    {
        if ((DiscordService.ShowDiscordQuestNames() && !isLargeImageText) || this.AlternativeQuestOverride())
        {
            return string.Empty;
        }

        if (this.IsToggeableDifficulty())
        {
            return string.Format(CultureInfo.InvariantCulture, "★{0} ", this.StarGrades().ToString(CultureInfo.InvariantCulture));
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Determines whether this instance is road.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is road; otherwise, <c>false</c>.
    /// </returns>
    public bool IsRoad()
    {
        if (this.RoadOverride() is not null and false)
        {
            return true;
        }
        else if (this.RoadOverride() is not null and true)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public int PreviousMezFesScore { get; set; }

    public bool QuestCleared { get; set; } // quest state 1

    public bool LoadedItemsAtQuestStart { get; set; }

    /// <summary>
    /// Determines whether this instance is dure quest.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is dure; otherwise, <c>false</c>.
    /// </returns>
    public bool IsDure()
    {
        if (this.GetDureName() != "None")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsDure(int questID)
    {
        if (this.GetDureName(questID) != "None")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether [is toggeable difficulty].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is toggeable difficulty]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsToggeableDifficulty()
    {
        if (!this.IsDure() && !this.IsRavi() && !this.IsRoad() && this.QuestID() != 0)
        {
            return this.RankBand() switch
            {
                0 or 1 or 2 or 3 or 4 or 5 or 6 or 7 or 8 or 9 or 10 or 11 or 12 or 13 or 14 or 15 or 16 or 17 or 18 or 19 or 20 or 26 or 31 or 42 or 53 => true,
                _ => false,
            };
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether this instance is ravi.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is ravi; otherwise, <c>false</c>.
    /// </returns>
    public bool IsRavi()
    {
        if (this.GetRaviName() != "None")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the real name of the monster.
    /// </summary>
    /// <param name="iconName">Name of the icon.</param>
    /// <returns></returns>
    public string GetRealMonsterName(bool isLargeImageText = false)
    {
        if (DiscordService.ShowDiscordQuestNames() && !isLargeImageText)
        {
            return string.Empty;
        }

        // quest ids:
        // mp road: 23527
        // solo road: 23628
        // 1st district dure: 21731
        // 2nd district dure: 21746
        // 1st district dure sky corridor: 21749
        // 2nd district dure sky corridor: 21750
        // arrogant dure repel: 23648
        // arrogant dure slay: 23649
        // urgent tower: 21751
        // 4th district dure: 21748
        // 3rd district dure: 21747
        // 3rd district dure 2: 21734
        // UNUSED sky corridor: 21730
        // sky corridor prologue: 21729
        // https://stackoverflow.com/questions/4315564/capitalizing-words-in-a-string-using-c-sharp
        int id;

        if (this.RoadOverride() == false)
        {
            id = this.RoadSelectedMonster() == 0 ? this.LargeMonster1ID() : this.LargeMonster2ID();
        }
        else if (this.AlternativeQuestOverride())
        {
            id = this.AlternativeQuestMonster1ID();
        }
        else
        {
            id = this.LargeMonster1ID();
        }

        // dure
        if (this.QuestID() is 21731 or 21746 or 21749 or 21750)
        {
            return "Duremudira";
        }
        else if (this.QuestID() is 23648 or 23649)
        {
            return "Arrogant Duremudira";
        }

        // ravi
        if (this.GetRaviName() != "None")
        {
            return this.GetRaviName() switch
            {
                "Raviente" => "Raviente",
                "Violent Raviente" => "Violent Raviente",
                "Berserk Raviente Practice" => "Berserk Raviente (Practice)",
                "Berserk Raviente" => "Berserk Raviente",
                "Extreme Raviente" => "Extreme Raviente",
                _ => "Raviente",
            };
        }

        return this.DetermineMonsterName(id);
    }

    /// <summary>
    /// Gets the name of the objective1.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetObjective1Name(int id, bool isLargeImageText = false)
    {
        if (DiscordService.ShowDiscordQuestNames() && !isLargeImageText)
        {
            return string.Empty;
        }

        Item.IDName.TryGetValue(id, out var objValue1);  // returns true
        return objValue1 + string.Empty;
    }

    /// <summary>
    /// Gets the name of the area.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public string GetAreaName(int id)
    {
        if (id == 0)
        {
            return "Loading...";
        }

        Location.IDName.TryGetValue(id, out var areaname);
        Location.IDName.TryGetValue(this.RavienteAreaID(), out var raviareaname);

        switch (this.GetRaviName())
        {
            default: // or None
                return areaname + string.Empty;
            case "Raviente":
            case "Violent Raviente":
                return raviareaname + string.Empty;
            case "Berserk Raviente Practice":
            case "Berserk Raviente":
            case "Extreme Raviente":
                if (this.QuestID() is not 55796 or not 55807 or not 54751 or not 54761 or not 55596 or not 55607)
                {
                    return areaname + string.Empty;
                }
                else
                {
                    return raviareaname + string.Empty;
                }
        }
    }

    public int PouchItem1IDAtQuestStart { get; set; }

    public int PouchItem2IDAtQuestStart { get; set; }

    public int PouchItem3IDAtQuestStart { get; set; }

    public int PouchItem4IDAtQuestStart { get; set; }

    public int PouchItem5IDAtQuestStart { get; set; }

    public int PouchItem6IDAtQuestStart { get; set; }

    public int PouchItem7IDAtQuestStart { get; set; }

    public int PouchItem8IDAtQuestStart { get; set; }

    public int PouchItem9IDAtQuestStart { get; set; }

    public int PouchItem10IDAtQuestStart { get; set; }

    public int PouchItem11IDAtQuestStart { get; set; }

    public int PouchItem12IDAtQuestStart { get; set; }

    public int PouchItem13IDAtQuestStart { get; set; }

    public int PouchItem14IDAtQuestStart { get; set; }

    public int PouchItem15IDAtQuestStart { get; set; }

    public int PouchItem16IDAtQuestStart { get; set; }

    public int PouchItem17IDAtQuestStart { get; set; }

    public int PouchItem18IDAtQuestStart { get; set; }

    public int PouchItem19IDAtQuestStart { get; set; }

    public int PouchItem20IDAtQuestStart { get; set; }

    public int PouchItem1QuantityAtQuestStart { get; set; }

    public int PouchItem2QuantityAtQuestStart { get; set; }

    public int PouchItem3QuantityAtQuestStart { get; set; }

    public int PouchItem4QuantityAtQuestStart { get; set; }

    public int PouchItem5QuantityAtQuestStart { get; set; }

    public int PouchItem6QuantityAtQuestStart { get; set; }

    public int PouchItem7QuantityAtQuestStart { get; set; }

    public int PouchItem8QuantityAtQuestStart { get; set; }

    public int PouchItem9QuantityAtQuestStart { get; set; }

    public int PouchItem10QuantityAtQuestStart { get; set; }

    public int PouchItem11QuantityAtQuestStart { get; set; }

    public int PouchItem12QuantityAtQuestStart { get; set; }

    public int PouchItem13QuantityAtQuestStart { get; set; }

    public int PouchItem14QuantityAtQuestStart { get; set; }

    public int PouchItem15QuantityAtQuestStart { get; set; }

    public int PouchItem16QuantityAtQuestStart { get; set; }

    public int PouchItem17QuantityAtQuestStart { get; set; }

    public int PouchItem18QuantityAtQuestStart { get; set; }

    public int PouchItem19QuantityAtQuestStart { get; set; }

    public int PouchItem20QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem1IDAtQuestStart { get; set; }

    public int AmmoPouchItem2IDAtQuestStart { get; set; }

    public int AmmoPouchItem3IDAtQuestStart { get; set; }

    public int AmmoPouchItem4IDAtQuestStart { get; set; }

    public int AmmoPouchItem5IDAtQuestStart { get; set; }

    public int AmmoPouchItem6IDAtQuestStart { get; set; }

    public int AmmoPouchItem7IDAtQuestStart { get; set; }

    public int AmmoPouchItem8IDAtQuestStart { get; set; }

    public int AmmoPouchItem9IDAtQuestStart { get; set; }

    public int AmmoPouchItem10IDAtQuestStart { get; set; }

    public int AmmoPouchItem1QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem2QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem3QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem4QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem5QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem6QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem7QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem8QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem9QuantityAtQuestStart { get; set; }

    public int AmmoPouchItem10QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem1IDAtQuestStart { get; set; }

    public int PartnyaBagItem2IDAtQuestStart { get; set; }

    public int PartnyaBagItem3IDAtQuestStart { get; set; }

    public int PartnyaBagItem4IDAtQuestStart { get; set; }

    public int PartnyaBagItem5IDAtQuestStart { get; set; }

    public int PartnyaBagItem6IDAtQuestStart { get; set; }

    public int PartnyaBagItem7IDAtQuestStart { get; set; }

    public int PartnyaBagItem8IDAtQuestStart { get; set; }

    public int PartnyaBagItem9IDAtQuestStart { get; set; }

    public int PartnyaBagItem10IDAtQuestStart { get; set; }

    public int PartnyaBagItem1QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem2QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem3QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem4QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem5QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem6QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem7QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem8QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem9QuantityAtQuestStart { get; set; }

    public int PartnyaBagItem10QuantityAtQuestStart { get; set; }

    public TimeSpan TotalTimeSpent { get; set; }

    public bool QuestRewardsGiven { get; set; } // quest state 129

    // all dictionaries get a new entry every 1 second. freezes on quest state 1, resets on quest id = 0.
    // use modulo
    // int for timeint() which is current quest time, second int for current attack buff
    public Dictionary<int, int> AttackBuffDictionary { get; set; } = new ();

    public static bool ValidateGameFolder()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        var findFiles = new List<string>
        {
            s.GameFolderPath + @"\dat\mhfdat.bin",
            s.GameFolderPath + @"\dat\mhfinf.bin",
            s.GameFolderPath + @"\dat\mhfemd.bin",
            s.GameFolderPath + @"\dat\mhfsqd.bin",
            s.GameFolderPath + @"\mhfo.dll",
            s.GameFolderPath + @"\mhfo-hd.dll",
            s.GameFolderPath + @"\mhf.exe",
        };

        // check if the folder is named dat and contains dat, emd, and sqd.
        if (s.GameFolderPath == string.Empty || s.GameFolderPath == null)
        {
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Game folder path not found");
            MessageBox.Show("Game folder path not found. If you do not want to log quests into the database or see this message, disable the Quest Logging option in Quest Logs section, and click the save button.", Messages.WarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
            s.EnableQuestLogging = false;
            s.Save();
            return false;
        }

        if (s.DatabaseFilePath == string.Empty || s.DatabaseFilePath == null)
        {
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Database file path not found");
            MessageBox.Show("Database file path not found. If you do not want to log quests into the database or see this message, disable the Quest Logging option in Quest Logs section, and click the save button.", Messages.WarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
            s.EnableQuestLogging = false;
            s.Save();
            return false;
        }

        // Get a list of file names in the game folder
        var gameFolderFiles = Directory.EnumerateFiles(s.GameFolderPath);

        // Get the intersection of the findFiles list and the gameFolderFiles list
        var intersection = findFiles.Intersect(gameFolderFiles);

        // If the intersection is not empty, it means that all the files in the findFiles list are present in the game folder
        if (intersection.Any())
        {
            // All required files are present
            return true;
        }
        else
        {
            // Some required files are missing
            MessageBox.Show(
                "Some required files are missing from the game folder. Please make sure that the game folder contains the following files: "
            + string.Join(", ", findFiles) + "\n" +
            "gameFolderFiles: " + string.Join(", ", gameFolderFiles),
                Messages.WarningTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Missing game files");
            s.EnableQuestLogging = false;
            s.Save();
            return false;
        }
    }

    // the deserealized are used for displays
    public Dictionary<int, int>? AttackBuffDictionaryDeserealized { get; set; }

    // same for this but second is current hit count
    public Dictionary<int, int> HitCountDictionary { get; set; } = new ();

    public Dictionary<int, int>? HitCountDictionaryDeserealized { get; set; }

    // same but the second int is the damage dealt when hitting monster.
    public Dictionary<int, int> DamageDealtDictionary { get; set; } = new ();

    public Dictionary<int, int>? DamageDealtDictionaryDeserealized { get; set; }

    // new entry every second during quest (use this for chart?)
    public Dictionary<int, double> DamagePerSecondDictionary { get; set; } = new ();

    // then for DPS i can calculate from the above dictionary and only update the
    // DPS value every time you register a new hit (new entry in damageDealtDictionary), which means its accurate but
    // not according to the real-time, but then again, the time that passes in-game
    // is not real-time.
    public double DPS { get; set; }

    public double HitsPerSecond { get; set; }

    public double APM { get; set; }

    public double TotalHitsTakenBlocked
    {
        get
        {
            // Check if the damageDealtDictionary is empty
            if (!this.HitsTakenBlockedDictionary.Any())
            {
                // If the dictionary is empty, return 0 as the DPS value
                return 0;
            }

            // Sum up the values in the maxValues dictionary to get the total amount
            var totalAmount = this.HitsTakenBlockedDictionary.Count;
            return totalAmount;
        }
    }

    public string TotalHitsTakenBlockedStats
    {
        get
        {
            var totalHitsTakenBlockedPerSecond = string.Empty;

            if (ShowTotalHitsTakenBlockedPerSecond())
            {
                totalHitsTakenBlockedPerSecond = string.Format(CultureInfo.InvariantCulture, " ({0:0.##}/s)", this.TotalHitsTakenBlockedPerSecond);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", this.TotalHitsTakenBlocked, totalHitsTakenBlockedPerSecond);
        }
    }

    public double TotalHitsTakenBlockedPerSecond { get; set; }

    public double CalculateDPS()
    {
        // Check if the damageDealtDictionary is empty
        if (!this.DamageDealtDictionary.Any())
        {
            // If the dictionary is empty, return 0 as the DPS value
            return 0;
        }

        double damageDealt = this.DamageDealtDictionary.Values.Sum();
        double timeElapsedIn30FPS = this.TimeDefInt() - this.TimeInt();

        // Calculate and return the DPS
        return damageDealt / (timeElapsedIn30FPS / (double)Numbers.FramesPerSecond);
    }

    // TODO: gamepad
    public double CalculateAPM()
    {
        double totalInputs = this.GamepadInputDictionary.Count + this.KeystrokesDictionary.Count + this.MouseInputDictionary.Count;
        double timeElapsedIn30FPS = this.TimeDefInt() - this.TimeInt();

        return totalInputs / (timeElapsedIn30FPS / 1800);
    }

    public double CalculateTotalHitsTakenBlockedPerSecond()
    {
        double timeElapsedIn30FPS = this.TimeDefInt() - this.TimeInt();

        // Calculate and return the DPS
        return this.TotalHitsTakenBlocked / (timeElapsedIn30FPS / (double)Numbers.FramesPerSecond);
    }

    public double CalculateHitsPerSecond()
    {
        if (!this.HitCountDictionary.Any())
        {
            return 0;
        }
        // TODO is this correct?
        return this.HitCountInt() / ((double)(this.TimeDefInt() - this.TimeInt()) / (double)Numbers.FramesPerSecond);
    }

    public Dictionary<int, double>? DamagePerSecondDictionaryDeserealized { get; set; }

    public Dictionary<int, int> AreaChangesDictionary { get; set; } = new ();

    public Dictionary<int, int>? AreaChangesDictionaryDeserealized { get; set; }

    public Dictionary<int, int> CartsDictionary { get; set; } = new ();

    public Dictionary<int, int>? CartsDictionaryDeserealized { get; set; }

    // time <monsterid, monsterhp>
    public Dictionary<int, Dictionary<int, int>> Monster1HPDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, int>> Monster2HPDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, int>> Monster3HPDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, int>> Monster4HPDictionary { get; set; } = new ();

    // time, itemid, itemquantity
    // can calculate itemuse by counting rows
    // this is a dicitonary where the first int is the quest time,
    // the second int is the item id and the third int is the item quantity of that id.
    // meaning that this is a dictionary of quest time and a list of item ids and quantities respectively.
    public Dictionary<int, List<Dictionary<int, int>>> PlayerInventoryDictionary { get; set; } = new ();

    public Dictionary<int, List<Dictionary<int, int>>> PlayerAmmoPouchDictionary { get; set; } = new ();

    public Dictionary<int, List<Dictionary<int, int>>> PartnyaBagDictionary { get; set; } = new ();

    // time, areaid, hitstakenblocked
    // can calculate total hits by area by checking areaid, or in total by all sum.
    public Dictionary<int, Dictionary<int, int>> HitsTakenBlockedDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, int>>? Monster1HPDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, int>>? Monster2HPDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, int>>? Monster3HPDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, int>>? Monster4HPDictionaryDeserealized { get; set; }

    public Dictionary<int, List<Dictionary<int, int>>>? PlayerInventoryDictionaryDeserealized { get; set; }

    public Dictionary<int, List<Dictionary<int, int>>>? PlayerAmmoPouchDictionaryDeserealized { get; set; }

    public Dictionary<int, List<Dictionary<int, int>>>? PartnyaBagDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, int>>? HitsTakenBlockedDictionaryDeserealized { get; set; } = new ();

    public Dictionary<int, int> PlayerHPDictionary { get; set; } = new ();

    public Dictionary<int, int>? PlayerHPDictionaryDeserealized { get; set; } = new ();

    public Dictionary<int, int> PlayerStaminaDictionary { get; set; } = new ();

    public Dictionary<int, int>? PlayerStaminaDictionaryDeserealized { get; set; } = new ();

    public Dictionary<int, double> HitsPerSecondDictionary { get; set; } = new ();

    public Dictionary<int, double>? HitsPerSecondDictionaryDeserealized { get; set; } = new ();

    public Dictionary<int, double> HitsTakenBlockedPerSecondDictionary { get; set; } = new ();

    public Dictionary<int, double>? HitsTakenBlockedPerSecondDictionaryDeserealized { get; set; } = new ();

    public Dictionary<int, string> KeystrokesDictionary { get; set; } = new ();

    public Dictionary<int, string>? KeystrokesDictionaryDeserealized { get; set; } = new ();

    public Dictionary<int, string> GamepadInputDictionary { get; set; } = new ();

    public Dictionary<int, string>? GamepadInputDictionaryDeserealized { get; set; } = new ();

    public Dictionary<int, string> MouseInputDictionary { get; set; } = new ();

    public Dictionary<int, string>? MouseInputDictionaryDeserealized { get; set; } = new ();

    public Dictionary<int, double> ActionsPerMinuteDictionary { get; set; } = new ();

    public Dictionary<int, double>? ActionsPerMinuteDictionaryDeserealized { get; set; } = new ();

    public Dictionary<int, string> OverlayModeDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, double>> Monster1AttackMultiplierDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, double>> Monster1DefenseRateDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, double>> Monster1SizeMultiplierDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, int>> Monster1PoisonThresholdDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, int>> Monster1SleepThresholdDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, int>> Monster1ParalysisThresholdDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, int>> Monster1BlastThresholdDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, int>> Monster1StunThresholdDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, List<int>>> Monster1PartThresholdDictionary { get; set; } = new ();

    public Dictionary<int, Dictionary<int, List<int>>> Monster2PartThresholdDictionary { get; set; } = new ();

    public int PreviousRoadFloor { get; set; }

    private readonly DateTime programStart = DateTime.UtcNow;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    public Dictionary<int, Dictionary<int, double>>? Monster1AttackMultiplierDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, double>>? Monster1DefenseRateDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, double>>? Monster1SizeMultiplierDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, int>>? Monster1PoisonThresholdDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, int>>? Monster1SleepThresholdDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, int>>? Monster1ParalysisThresholdDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, int>>? Monster1BlastThresholdDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, int>>? Monster1StunThresholdDictionaryDeserealized { get; set; }

    public Dictionary<int, Dictionary<int, List<int>>>? Monster1PartThresholdDictionaryDeserialized { get; set; }

    public Dictionary<int, Dictionary<int, List<int>>>? Monster2PartThresholdDictionaryDeserialized { get; set; }

    public int PreviousTimeInt { get; set; }

    public int PreviousAttackBuffInt { get; set; }

    public int PreviousHitCountInt { get; set; }

    public double PreviousDPS { get; set; }

    public int PreviousAreaID { get; set; }

    public int PreviousGlobalAreaID { get; set; }

    public int PreviousQuestID { get; set; }

    public int PreviousCartsInt { get; set; }

    public int PreviousMonster1HP { get; set; }

    public int PreviousMonster2HP { get; set; }

    public int PreviousMonster3HP { get; set; }

    public int PreviousMonster4HP { get; set; }

    public int PreviousTotalInventoryItems { get; set; }

    public int PreviousTotalAmmo { get; set; }

    public int PreviousTotalPartnyaItems { get; set; }

    public int PreviousHitsTakenBlocked { get; set; }

    public double PreviousTotalHitsTakenBlockedPerSecond { get; set; }

    public int PreviousPlayerHP { get; set; }

    public int PreviousPlayerStamina { get; set; }

    public double PreviousHitsPerSecond { get; set; }

    public double PreviousActionsPerMinute { get; set; }

    public OverlayMode PreviousOverlayMode { get; set; } = OverlayMode.Unknown;

    public double PreviousMonster1AttackMultiplier { get; set; }

    public double PreviousMonster1DefenseRate { get; set; }

    public double PreviousMonster1SizeMultiplier { get; set; }

    public int PreviousMonster1PoisonThreshold { get; set; }

    public int PreviousMonster1SleepThreshold { get; set; }

    public int PreviousMonster1ParalysisThreshold { get; set; }

    public int PreviousMonster1BlastThreshold { get; set; }

    public int PreviousMonster1StunThreshold { get; set; }

    public int PreviousMonster1Part1Threshold { get; set; }

    public int PreviousMonster1Part2Threshold { get; set; }

    public int PreviousMonster1Part3Threshold { get; set; }

    public int PreviousMonster1Part4Threshold { get; set; }

    public int PreviousMonster1Part5Threshold { get; set; }

    public int PreviousMonster1Part6Threshold { get; set; }

    public int PreviousMonster1Part7Threshold { get; set; }

    public int PreviousMonster1Part8Threshold { get; set; }

    public int PreviousMonster1Part9Threshold { get; set; }

    public int PreviousMonster1Part10Threshold { get; set; }

    public int PreviousMonster2Part1Threshold { get; set; }

    public int PreviousMonster2Part2Threshold { get; set; }

    public int PreviousMonster2Part3Threshold { get; set; }

    public int PreviousMonster2Part4Threshold { get; set; }

    public int PreviousMonster2Part5Threshold { get; set; }

    public int PreviousMonster2Part6Threshold { get; set; }

    public int PreviousMonster2Part7Threshold { get; set; }

    public int PreviousMonster2Part8Threshold { get; set; }

    public int PreviousMonster2Part9Threshold { get; set; }

    public int PreviousMonster2Part10Threshold { get; set; }

    public Dictionary<int, int> DualSwordsSharpensDictionary { get; set; } = new();

    public int PreviousDualSwordsSharpens { get; set; }

    public Dictionary<int,int> PartySizeDictionary { get; set; } = new();

    public int PreviousPartySize { get; set; }

    // Get all countries
    public static IEnumerable<Country> Countries => RestCountriesService.GetAllCountries();

    public List<Dictionary<int, int>> InsertInventoryDictionaryIntoList(string inventoryType)
    {
        var list = new List<Dictionary<int, int>>();

        var x = 20;
        switch (inventoryType)
        {
            case "Pouch":
            case "Ammo":
                x = 20;
                break;
            case "Partnya Bag":
                x = 10;
                break;
            default:
                break;
        }

        for (var i = 1; i <= x; i++)
        {
            var itemID = 0;
            var itemQty = 0;
            Dictionary<int, int> itemIDQuantityDictionary = new ();

            if (inventoryType == "Pouch")
            {
                switch (i)
                {
                    case 1:
                        itemID = this.PouchItem1ID();
                        itemQty = this.PouchItem1Qty();
                        break;
                    case 2:
                        itemID = this.PouchItem2ID();
                        itemQty = this.PouchItem2Qty();
                        break;
                    case 3:
                        itemID = this.PouchItem3ID();
                        itemQty = this.PouchItem3Qty();
                        break;
                    case 4:
                        itemID = this.PouchItem4ID();
                        itemQty = this.PouchItem4Qty();
                        break;
                    case 5:
                        itemID = this.PouchItem5ID();
                        itemQty = this.PouchItem5Qty();
                        break;
                    case 6:
                        itemID = this.PouchItem6ID();
                        itemQty = this.PouchItem6Qty();
                        break;
                    case 7:
                        itemID = this.PouchItem7ID();
                        itemQty = this.PouchItem7Qty();
                        break;
                    case 8:
                        itemID = this.PouchItem8ID();
                        itemQty = this.PouchItem8Qty();
                        break;
                    case 9:
                        itemID = this.PouchItem9ID();
                        itemQty = this.PouchItem9Qty();
                        break;
                    case 10:
                        itemID = this.PouchItem10ID();
                        itemQty = this.PouchItem10Qty();
                        break;
                    case 11:
                        itemID = this.PouchItem11ID();
                        itemQty = this.PouchItem11Qty();
                        break;
                    case 12:
                        itemID = this.PouchItem12ID();
                        itemQty = this.PouchItem12Qty();
                        break;
                    case 13:
                        itemID = this.PouchItem13ID();
                        itemQty = this.PouchItem13Qty();
                        break;
                    case 14:
                        itemID = this.PouchItem14ID();
                        itemQty = this.PouchItem14Qty();
                        break;
                    case 15:
                        itemID = this.PouchItem15ID();
                        itemQty = this.PouchItem15Qty();
                        break;
                    case 16:
                        itemID = this.PouchItem16ID();
                        itemQty = this.PouchItem16Qty();
                        break;
                    case 17:
                        itemID = this.PouchItem17ID();
                        itemQty = this.PouchItem17Qty();
                        break;
                    case 18:
                        itemID = this.PouchItem18ID();
                        itemQty = this.PouchItem18Qty();
                        break;
                    case 19:
                        itemID = this.PouchItem19ID();
                        itemQty = this.PouchItem19Qty();
                        break;
                    case 20:
                        itemID = this.PouchItem20ID();
                        itemQty = this.PouchItem20Qty();
                        break;
                    default:
                        break;
                }
            }
            else if (inventoryType == "Ammo")
            {
                switch (i)
                {
                    case 1:
                        itemID = this.AmmoPouchItem1ID();
                        itemQty = this.AmmoPouchItem1Qty();
                        break;
                    case 2:
                        itemID = this.AmmoPouchItem2ID();
                        itemQty = this.AmmoPouchItem2Qty();
                        break;
                    case 3:
                        itemID = this.AmmoPouchItem3ID();
                        itemQty = this.AmmoPouchItem3Qty();
                        break;
                    case 4:
                        itemID = this.AmmoPouchItem4ID();
                        itemQty = this.AmmoPouchItem4Qty();
                        break;
                    case 5:
                        itemID = this.AmmoPouchItem5ID();
                        itemQty = this.AmmoPouchItem5Qty();
                        break;
                    case 6:
                        itemID = this.AmmoPouchItem6ID();
                        itemQty = this.AmmoPouchItem6Qty();
                        break;
                    case 7:
                        itemID = this.AmmoPouchItem7ID();
                        itemQty = this.AmmoPouchItem7Qty();
                        break;
                    case 8:
                        itemID = this.AmmoPouchItem8ID();
                        itemQty = this.AmmoPouchItem8Qty();
                        break;
                    case 9:
                        itemID = this.AmmoPouchItem9ID();
                        itemQty = this.AmmoPouchItem9Qty();
                        break;
                    case 10:
                        itemID = this.AmmoPouchItem10ID();
                        itemQty = this.AmmoPouchItem10Qty();
                        break;
                    default:
                        break;
                }
            }
            else if (inventoryType == "Partnya Bag")
            {
                switch (i)
                {
                    case 1:
                        itemID = this.PartnyaBagItem1ID();
                        itemQty = this.PartnyaBagItem1Qty();
                        break;
                    case 2:
                        itemID = this.PartnyaBagItem2ID();
                        itemQty = this.PartnyaBagItem2Qty();
                        break;
                    case 3:
                        itemID = this.PartnyaBagItem3ID();
                        itemQty = this.PartnyaBagItem3Qty();
                        break;
                    case 4:
                        itemID = this.PartnyaBagItem4ID();
                        itemQty = this.PartnyaBagItem4Qty();
                        break;
                    case 5:
                        itemID = this.PartnyaBagItem5ID();
                        itemQty = this.PartnyaBagItem5Qty();
                        break;
                    case 6:
                        itemID = this.PartnyaBagItem6ID();
                        itemQty = this.PartnyaBagItem6Qty();
                        break;
                    case 7:
                        itemID = this.PartnyaBagItem7ID();
                        itemQty = this.PartnyaBagItem7Qty();
                        break;
                    case 8:
                        itemID = this.PartnyaBagItem8ID();
                        itemQty = this.PartnyaBagItem8Qty();
                        break;
                    case 9:
                        itemID = this.PartnyaBagItem9ID();
                        itemQty = this.PartnyaBagItem9Qty();
                        break;
                    case 10:
                        itemID = this.PartnyaBagItem10ID();
                        itemQty = this.PartnyaBagItem10Qty();
                        break;
                    default:
                        break;
                }
            }

            itemIDQuantityDictionary.Add(itemID, itemQty);
            list.Add(itemIDQuantityDictionary);
        }

        return list;
    }

    public static int CalculateTotalDictionaryValuesInList(List<Dictionary<int, int>> list)
    {
        if (list.Count > 0)
        {
            return list.SelectMany(x => x.Values).Sum();
        }
        else
        {
            return 0;
        }
    }

    public double GetCurrentQuestElapsedTimeInSeconds()
    {
        if (this.TimeDefInt() <= 0)
        {
            return 0;
        }

        return (double)(this.TimeDefInt() - this.TimeInt()) / (double)Numbers.FramesPerSecond;
    }

    /// <summary>
    /// Inserts the quest information into dictionaries. If you want to insert a new dictionary, do the following:
    /// 1. Create a dictionary as a property of this class.
    /// 2. Follow the code structure of the current dictionary insertions,
    /// but for this particular dictionary structure if modifications are needed.
    /// 3. Go to DatabaseManager and modify the Quests table for taking into account this new property.
    /// 4. Copy-paste the query into a new function for updating the database schema (PerformUpdateToVersion_x_y_z), for new_Quests table.
    /// See also the 12 steps from SQLite linked for helping update and port data from an old table schema to a new table schema.
    /// 5. Modify InsertQuestData Quests table insertion section taking into account the new property and table schema.
    /// 6. Don't forget to add the clear() in the cleanup functions for the dictionaries.
    /// 7. Complete a quest to see if everything works. Also test the update process.
    /// This does not take into account tracking for achievements. See the already made stats to check how they are tracked.
    /// </summary>
    public void InsertQuestInfoIntoDictionaries()
    {
        // TODO: the above update process should be simplified. refactoring might be needed
        // in many places, not just this function.
        var timeInt = this.TimeInt();
        var monster1ID = IsAlternativeQuestName() || IsDure() ? this.AlternativeQuestMonster1ID() : this.LargeMonster1ID();
        var monster2ID = IsAlternativeQuestName() || IsDure() ? this.AlternativeQuestMonster2ID() : this.LargeMonster2ID();

        if (this.IsRoad() && this.AreaID() == 459) // Hunter's Road Base Camp
        {
            if (this.RoadFloor() + 1 > this.PreviousRoadFloor)
            {
                this.PreviousRoadFloor = this.RoadFloor() + 1;
                this.ClearQuestInfoDictionaries();
                this.ClearGraphCollections();
                this.ResetQuestInfoVariables();
            }
        }

        if (this.PreviousAttackBuffInt != this.WeaponRaw() && !this.AttackBuffDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousAttackBuffInt = this.WeaponRaw();
                this.AttackBuffDictionary.Add(this.TimeInt(), this.WeaponRaw());

                // TODO the very last value gets put which is 0 when going back to mezeporta
                lock (this.AttackBuffSync)
                {
                    // Any changes including adding, clearing, etc must be synced.
                    this.AttackBuffCollection.Add(new ObservablePoint(this.GetCurrentQuestElapsedTimeInSeconds(), this.WeaponRaw()));
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into attackBuffDictionary");
            }
        }

        if (this.PreviousHitCountInt != this.HitCountInt() && !this.HitCountDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousHitCountInt = this.HitCountInt();
                this.HitCountDictionary.Add(this.TimeInt(), this.HitCountInt());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into hitCountDictionary");
            }
        }

        if (this.PreviousDPS != this.DPS && !this.DamagePerSecondDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousDPS = this.DPS;
                this.DamagePerSecondDictionary.Add(this.TimeInt(), this.DPS);

                lock (this.DamagePerSecondSync)
                {
                    // Any changes including adding, clearing, etc must be synced.
                    this.DamagePerSecondCollection.Add(new ObservablePoint(this.GetCurrentQuestElapsedTimeInSeconds(), this.DPS));
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into damagePerSecondDictionary");
            }
        }

        if (this.PreviousCartsInt != this.CurrentFaints() && !this.CartsDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousCartsInt = this.CurrentFaints();
                this.CartsDictionary.Add(this.TimeInt(), this.CurrentFaints());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into cartsDictionary");
            }
        }

        if (this.PreviousAreaID != this.AreaID() && !this.AreaChangesDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousAreaID = this.AreaID();
                this.AreaChangesDictionary.Add(this.TimeInt(), this.AreaID());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into areaChangesDictionary");
            }
        }

        if (this.PreviousMonster1HP != this.Monster1HPInt() && !this.Monster1HPDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1HP = this.Monster1HPInt();
                Dictionary<int, int> monster1HPDictionaryMonsterInfo = new ()
                {
                    { monster1ID, this.Monster1HPInt() },
                };
                this.Monster1HPDictionary.Add(this.TimeInt(), monster1HPDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1HPDictionary");
            }
        }

        if (this.PreviousMonster2HP != this.Monster2HPInt() && !this.Monster2HPDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster2HP = this.Monster2HPInt();
                Dictionary<int, int> monster2HPDictionaryMonsterInfo = new ()
                {
                    {monster2ID, this.Monster2HPInt() },
                };
                this.Monster2HPDictionary.Add(this.TimeInt(), monster2HPDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster2HPDictionary");
            }
        }

        if (this.PreviousMonster3HP != this.Monster3HPInt() && !this.Monster3HPDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster3HP = this.Monster3HPInt();
                Dictionary<int, int> monster3HPDictionaryMonsterInfo = new ()
                {
                    { this.LargeMonster3ID(), this.Monster3HPInt() },
                };
                this.Monster3HPDictionary.Add(this.TimeInt(), monster3HPDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster3HPDictionary");
            }
        }

        if (this.PreviousMonster4HP != this.Monster4HPInt() && !this.Monster4HPDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster4HP = this.Monster4HPInt();
                Dictionary<int, int> monster4HPDictionaryMonsterInfo = new ()
                {
                    { this.LargeMonster4ID(), this.Monster4HPInt() },
                };
                this.Monster4HPDictionary.Add(this.TimeInt(), monster4HPDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster4HPDictionary");
            }
        }

        // inventory
        var currentInventoryList = this.InsertInventoryDictionaryIntoList("Pouch");
        var currentInventorySum = CalculateTotalDictionaryValuesInList(currentInventoryList);
        var lastInventorySum = 0;

        if (this.PlayerInventoryDictionary.Values.Any())
        {
            var lastInsertedDictionary = this.PlayerInventoryDictionary.Values.Last();

            // Sum the values in the list
            lastInventorySum = lastInsertedDictionary.SelectMany(x => x.Values).Sum();
        }

        if (currentInventorySum != lastInventorySum)
        {
            try
            {
                this.PlayerInventoryDictionary.Add(this.TimeInt(), currentInventoryList);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into playerInventoryDictionary");
            }
        }
        else if (this.LoadedItemsAtQuestStart && !this.PlayerInventoryDictionary.Values.Any())
        {
            List<Dictionary<int, int>> itemIDsQuantityList = new ();
            for (var i = 1; i <= 20; i++)
            {
                var itemID = 0;
                var itemQty = 0;
                Dictionary<int, int> itemIDQuantityDictionary = new ();
                switch (i)
                {
                    case 1:
                        itemID = this.PouchItem1IDAtQuestStart;
                        itemQty = this.PouchItem1QuantityAtQuestStart;
                        break;
                    case 2:
                        itemID = this.PouchItem2IDAtQuestStart;
                        itemQty = this.PouchItem2QuantityAtQuestStart;
                        break;
                    case 3:
                        itemID = this.PouchItem3IDAtQuestStart;
                        itemQty = this.PouchItem3QuantityAtQuestStart;
                        break;
                    case 4:
                        itemID = this.PouchItem4IDAtQuestStart;
                        itemQty = this.PouchItem4QuantityAtQuestStart;
                        break;
                    case 5:
                        itemID = this.PouchItem5IDAtQuestStart;
                        itemQty = this.PouchItem5QuantityAtQuestStart;
                        break;
                    case 6:
                        itemID = this.PouchItem6IDAtQuestStart;
                        itemQty = this.PouchItem6QuantityAtQuestStart;
                        break;
                    case 7:
                        itemID = this.PouchItem7IDAtQuestStart;
                        itemQty = this.PouchItem7QuantityAtQuestStart;
                        break;
                    case 8:
                        itemID = this.PouchItem8IDAtQuestStart;
                        itemQty = this.PouchItem8QuantityAtQuestStart;
                        break;
                    case 9:
                        itemID = this.PouchItem9IDAtQuestStart;
                        itemQty = this.PouchItem9QuantityAtQuestStart;
                        break;
                    case 10:
                        itemID = this.PouchItem10IDAtQuestStart;
                        itemQty = this.PouchItem10QuantityAtQuestStart;
                        break;
                    case 11:
                        itemID = this.PouchItem11IDAtQuestStart;
                        itemQty = this.PouchItem11QuantityAtQuestStart;
                        break;
                    case 12:
                        itemID = this.PouchItem12IDAtQuestStart;
                        itemQty = this.PouchItem12QuantityAtQuestStart;
                        break;
                    case 13:
                        itemID = this.PouchItem13IDAtQuestStart;
                        itemQty = this.PouchItem13QuantityAtQuestStart;
                        break;
                    case 14:
                        itemID = this.PouchItem14IDAtQuestStart;
                        itemQty = this.PouchItem14QuantityAtQuestStart;
                        break;
                    case 15:
                        itemID = this.PouchItem15IDAtQuestStart;
                        itemQty = this.PouchItem15QuantityAtQuestStart;
                        break;
                    case 16:
                        itemID = this.PouchItem16IDAtQuestStart;
                        itemQty = this.PouchItem16QuantityAtQuestStart;
                        break;
                    case 17:
                        itemID = this.PouchItem17IDAtQuestStart;
                        itemQty = this.PouchItem17QuantityAtQuestStart;
                        break;
                    case 18:
                        itemID = this.PouchItem18IDAtQuestStart;
                        itemQty = this.PouchItem18QuantityAtQuestStart;
                        break;
                    case 19:
                        itemID = this.PouchItem19IDAtQuestStart;
                        itemQty = this.PouchItem19QuantityAtQuestStart;
                        break;
                    case 20:
                        itemID = this.PouchItem20IDAtQuestStart;
                        itemQty = this.PouchItem20QuantityAtQuestStart;
                        break;
                    default:
                        break;
                }

                itemIDQuantityDictionary.Add(itemID, itemQty);
                itemIDsQuantityList.Add(itemIDQuantityDictionary);
            }

            try
            {
                this.PlayerInventoryDictionary.Add(this.TimeInt(), itemIDsQuantityList);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into playerInventoryDictionary");
            }
        }

        // ammo
        var currentAmmoList = this.InsertInventoryDictionaryIntoList("Ammo");
        var currentAmmoSum = CalculateTotalDictionaryValuesInList(currentAmmoList);
        var lastAmmoSum = 0;

        if (this.PlayerAmmoPouchDictionary.Values.Any())
        {
            var lastInsertedDictionary = this.PlayerAmmoPouchDictionary.Values.Last();

            // Sum the values in the list
            lastAmmoSum = lastInsertedDictionary.SelectMany(x => x.Values).Sum();
        }

        if (currentAmmoSum != lastAmmoSum)
        {
            try
            {
                this.PlayerAmmoPouchDictionary.Add(this.TimeInt(), currentAmmoList);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into playerAmmoPouchDictionary");
            }
        }
        else if (this.LoadedItemsAtQuestStart && !this.PlayerAmmoPouchDictionary.Values.Any())
        {
            List<Dictionary<int, int>> itemIDsQuantityList = new ();
            for (var i = 1; i <= 20; i++)
            {
                var itemID = 0;
                var itemQty = 0;
                Dictionary<int, int> itemIDQuantityDictionary = new ();
                switch (i)
                {
                    case 1:
                        itemID = this.AmmoPouchItem1IDAtQuestStart;
                        itemQty = this.AmmoPouchItem1QuantityAtQuestStart;
                        break;
                    case 2:
                        itemID = this.AmmoPouchItem2IDAtQuestStart;
                        itemQty = this.AmmoPouchItem2QuantityAtQuestStart;
                        break;
                    case 3:
                        itemID = this.AmmoPouchItem3IDAtQuestStart;
                        itemQty = this.AmmoPouchItem3QuantityAtQuestStart;
                        break;
                    case 4:
                        itemID = this.AmmoPouchItem4IDAtQuestStart;
                        itemQty = this.AmmoPouchItem4QuantityAtQuestStart;
                        break;
                    case 5:
                        itemID = this.AmmoPouchItem5IDAtQuestStart;
                        itemQty = this.AmmoPouchItem5QuantityAtQuestStart;
                        break;
                    case 6:
                        itemID = this.AmmoPouchItem6IDAtQuestStart;
                        itemQty = this.AmmoPouchItem6QuantityAtQuestStart;
                        break;
                    case 7:
                        itemID = this.AmmoPouchItem7IDAtQuestStart;
                        itemQty = this.AmmoPouchItem7QuantityAtQuestStart;
                        break;
                    case 8:
                        itemID = this.AmmoPouchItem8IDAtQuestStart;
                        itemQty = this.AmmoPouchItem8QuantityAtQuestStart;
                        break;
                    case 9:
                        itemID = this.AmmoPouchItem9IDAtQuestStart;
                        itemQty = this.AmmoPouchItem9QuantityAtQuestStart;
                        break;
                    case 10:
                        itemID = this.AmmoPouchItem10IDAtQuestStart;
                        itemQty = this.AmmoPouchItem10QuantityAtQuestStart;
                        break;
                    default:
                        break;
                }

                itemIDQuantityDictionary.Add(itemID, itemQty);
                itemIDsQuantityList.Add(itemIDQuantityDictionary);
            }

            try
            {
                this.PlayerAmmoPouchDictionary.Add(this.TimeInt(), itemIDsQuantityList);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into playerAmmoPouchDictionary");
            }
        }

        // partnya bag
        var currentPartnyaBagList = this.InsertInventoryDictionaryIntoList("Partnya Bag");
        var currentPartnyaBagSum = CalculateTotalDictionaryValuesInList(currentPartnyaBagList);
        var lastPartnyaBagSum = 0;

        if (this.PartnyaBagDictionary.Values.Any())
        {
            var lastInsertedDictionary = this.PartnyaBagDictionary.Values.Last();

            // Sum the values in the list
            lastPartnyaBagSum = lastInsertedDictionary.SelectMany(x => x.Values).Sum();
        }

        if (currentPartnyaBagSum != lastPartnyaBagSum)
        {
            try
            {
                this.PartnyaBagDictionary.Add(this.TimeInt(), currentPartnyaBagList);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into partnyaBagDictionary");
            }
        }
        else if (this.LoadedItemsAtQuestStart && !this.PartnyaBagDictionary.Values.Any())
        {
            List<Dictionary<int, int>> itemIDsQuantityList = new ();
            for (var i = 1; i <= 20; i++)
            {
                var itemID = 0;
                var itemQty = 0;
                Dictionary<int, int> itemIDQuantityDictionary = new ();
                switch (i)
                {
                    case 1:
                        itemID = this.PartnyaBagItem1IDAtQuestStart;
                        itemQty = this.PartnyaBagItem1QuantityAtQuestStart;
                        break;
                    case 2:
                        itemID = this.PartnyaBagItem2IDAtQuestStart;
                        itemQty = this.PartnyaBagItem2QuantityAtQuestStart;
                        break;
                    case 3:
                        itemID = this.PartnyaBagItem3IDAtQuestStart;
                        itemQty = this.PartnyaBagItem3QuantityAtQuestStart;
                        break;
                    case 4:
                        itemID = this.PartnyaBagItem4IDAtQuestStart;
                        itemQty = this.PartnyaBagItem4QuantityAtQuestStart;
                        break;
                    case 5:
                        itemID = this.PartnyaBagItem5IDAtQuestStart;
                        itemQty = this.PartnyaBagItem5QuantityAtQuestStart;
                        break;
                    case 6:
                        itemID = this.PartnyaBagItem6IDAtQuestStart;
                        itemQty = this.PartnyaBagItem6QuantityAtQuestStart;
                        break;
                    case 7:
                        itemID = this.PartnyaBagItem7IDAtQuestStart;
                        itemQty = this.PartnyaBagItem7QuantityAtQuestStart;
                        break;
                    case 8:
                        itemID = this.PartnyaBagItem8IDAtQuestStart;
                        itemQty = this.PartnyaBagItem8QuantityAtQuestStart;
                        break;
                    case 9:
                        itemID = this.PartnyaBagItem9IDAtQuestStart;
                        itemQty = this.PartnyaBagItem9QuantityAtQuestStart;
                        break;
                    case 10:
                        itemID = this.PartnyaBagItem10IDAtQuestStart;
                        itemQty = this.PartnyaBagItem10QuantityAtQuestStart;
                        break;
                    default:
                        break;
                }

                itemIDQuantityDictionary.Add(itemID, itemQty);
                itemIDsQuantityList.Add(itemIDQuantityDictionary);
            }

            try
            {
                this.PartnyaBagDictionary.Add(this.TimeInt(), itemIDsQuantityList);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into partnyaBagDictionary");
            }
        }

        if (this.PreviousHitsTakenBlocked != this.AreaHitsTakenBlocked() && this.AreaHitsTakenBlocked() != 0 && !this.HitsTakenBlockedDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousHitsTakenBlocked = this.AreaHitsTakenBlocked();
                Dictionary<int, int> hitsAreaPairs = new ()
                {
                    { this.AreaID(), this.AreaHitsTakenBlocked() },
                };
                this.HitsTakenBlockedDictionary.Add(this.TimeInt(), hitsAreaPairs);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into hitsTakenBlockedDictionary");
            }
        }

        if (this.PreviousPlayerHP != this.HunterHP() && this.PlayerHPDictionary.TryGetValue(this.TimeInt(), out var hp) == false)
        {
            try
            {
                this.PreviousPlayerHP = this.HunterHP();
                this.PlayerHPDictionary.Add(this.TimeInt(), this.HunterHP());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into playerHPDictionary");
            }
        }

        if (this.PreviousPlayerStamina != this.HunterStamina() && !this.PlayerStaminaDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousPlayerStamina = this.HunterStamina();
                this.PlayerStaminaDictionary.Add(this.TimeInt(), this.HunterStamina());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into playerStaminaDictionary");
            }
        }

        if (this.PreviousHitsPerSecond != this.HitsPerSecond && !this.HitsPerSecondDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousHitsPerSecond = this.HitsPerSecond;
                this.HitsPerSecondDictionary.Add(this.TimeInt(), this.HitsPerSecond);

                lock (this.HitsPerSecondSync)
                {
                    // Any changes including adding, clearing, etc must be synced.
                    this.HitsPerSecondCollection.Add(new ObservablePoint(this.GetCurrentQuestElapsedTimeInSeconds(), this.HitsPerSecond));
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into hitsPerSecondDictionary");
            }
        }

        if (this.PreviousTotalHitsTakenBlockedPerSecond != this.TotalHitsTakenBlockedPerSecond && !this.HitsTakenBlockedPerSecondDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousTotalHitsTakenBlockedPerSecond = this.TotalHitsTakenBlockedPerSecond;
                this.HitsTakenBlockedPerSecondDictionary.Add(this.TimeInt(), this.TotalHitsTakenBlockedPerSecond);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into hitsTakenBlockedPerSecondDictionary");
            }
        }

        if (this.PreviousActionsPerMinute != this.APM && !this.ActionsPerMinuteDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousActionsPerMinute = this.APM;
                this.ActionsPerMinuteDictionary.Add(this.TimeInt(), this.APM);

                lock (this.ActionsPerMinuteSync)
                {
                    // Any changes including adding, clearing, etc must be synced.
                    this.ActionsPerMinuteCollection.Add(new ObservablePoint(this.GetCurrentQuestElapsedTimeInSeconds(), this.APM));
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into actionsPerMinuteDictionary");
            }
        }

        if (this.PreviousOverlayMode != this.GetOverlayMode() && !this.OverlayModeDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousOverlayMode = this.GetOverlayMode();
                this.OverlayModeDictionary.Add(this.TimeInt(), GetOverlayModeForStorage());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into overlayModeDictionary");
            }
        }

        if (this.PreviousMonster1AttackMultiplier != this.Monster1AttackMultForDictionary() && !this.Monster1AttackMultiplierDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1AttackMultiplier = this.Monster1AttackMultForDictionary();
                Dictionary<int, double> monster1AttackMultiplierDictionaryMonsterInfo = new ()
                {
                    { monster1ID, this.Monster1AttackMultForDictionary() },
                };
                this.Monster1AttackMultiplierDictionary.Add(this.TimeInt(), monster1AttackMultiplierDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1AttackMultiplierDictionary");
            }
        }

        if (this.PreviousMonster1DefenseRate != this.Monster1DefMultForDictionary() && !this.Monster1DefenseRateDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1DefenseRate = this.Monster1DefMultForDictionary();
                Dictionary<int, double> monster1DefenseRateDictionaryMonsterInfo = new ()
                {
                    { monster1ID, this.Monster1DefMultForDictionary() },
                };
                this.Monster1DefenseRateDictionary.Add(this.TimeInt(), monster1DefenseRateDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1DefenseRateDictionary");
            }
        }

        if (this.PreviousMonster1SizeMultiplier != this.Monster1SizeMultForDictionary() && !this.Monster1SizeMultiplierDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1SizeMultiplier = this.Monster1SizeMultForDictionary();
                Dictionary<int, double> monster1SizeMultiplierDictionaryMonsterInfo = new ()
                {
                    { monster1ID, this.Monster1SizeMultForDictionary() },
                };
                this.Monster1SizeMultiplierDictionary.Add(this.TimeInt(), monster1SizeMultiplierDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1SizeMultiplierDictionary");
            }
        }

        if (this.PreviousMonster1PoisonThreshold != this.Monster1PoisonForDictionary() && !this.Monster1PoisonThresholdDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1PoisonThreshold = this.Monster1PoisonForDictionary();
                Dictionary<int, int> monster1PoisonThresholdDictionaryMonsterInfo = new ()
                {
                    { monster1ID, this.Monster1PoisonForDictionary() },
                };
                this.Monster1PoisonThresholdDictionary.Add(this.TimeInt(), monster1PoisonThresholdDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1PoisonThresholdDictionary");
            }
        }

        if (this.PreviousMonster1SleepThreshold != this.Monster1SleepForDictionary() && !this.Monster1SleepThresholdDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1SleepThreshold = this.Monster1SleepForDictionary();
                Dictionary<int, int> monster1SleepThresholdDictionaryMonsterInfo = new ()
                {
                    { monster1ID, this.Monster1SleepForDictionary() },
                };
                this.Monster1SleepThresholdDictionary.Add(this.TimeInt(), monster1SleepThresholdDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1SleepThresholdDictionary");
            }
        }

        if (this.PreviousMonster1ParalysisThreshold != this.Monster1ParalysisForDictionary() && !this.Monster1ParalysisThresholdDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1ParalysisThreshold = this.Monster1ParalysisForDictionary();
                Dictionary<int, int> monster1ParalysisThresholdDictionaryMonsterInfo = new ()
                {
                    { monster1ID, this.Monster1ParalysisForDictionary() },
                };
                this.Monster1ParalysisThresholdDictionary.Add(this.TimeInt(), monster1ParalysisThresholdDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1ParalysisThresholdDictionary");
            }
        }

        if (this.PreviousMonster1BlastThreshold != this.Monster1BlastForDictionary() && !this.Monster1BlastThresholdDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1BlastThreshold = this.Monster1BlastForDictionary();
                Dictionary<int, int> monster1BlastThresholdDictionaryMonsterInfo = new ()
                {
                    { monster1ID, this.Monster1BlastForDictionary() },
                };
                this.Monster1BlastThresholdDictionary.Add(this.TimeInt(), monster1BlastThresholdDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1BlastThresholdDictionary");
            }
        }

        if (this.PreviousMonster1StunThreshold != this.Monster1StunForDictionary() && !this.Monster1StunThresholdDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1StunThreshold = this.Monster1StunForDictionary();
                Dictionary<int, int> monster1StunThresholdDictionaryMonsterInfo = new ()
                {
                    { monster1ID, this.Monster1StunForDictionary() },
                };
                this.Monster1StunThresholdDictionary.Add(this.TimeInt(), monster1StunThresholdDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1StunThresholdDictionary");
            }
        }

        // TODO: may want to optimize performance/readability
        if ((this.PreviousMonster1Part1Threshold != this.Monster1Part1() ||
            this.PreviousMonster1Part2Threshold != this.Monster1Part2() ||
            this.PreviousMonster1Part3Threshold != this.Monster1Part3() ||
            this.PreviousMonster1Part4Threshold != this.Monster1Part4() ||
            this.PreviousMonster1Part5Threshold != this.Monster1Part5() ||
            this.PreviousMonster1Part6Threshold != this.Monster1Part6() ||
            this.PreviousMonster1Part7Threshold != this.Monster1Part7() ||
            this.PreviousMonster1Part8Threshold != this.Monster1Part8() ||
            this.PreviousMonster1Part9Threshold != this.Monster1Part9() ||
            this.PreviousMonster1Part10Threshold != this.Monster1Part10())
            && !this.Monster1PartThresholdDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster1Part1Threshold = this.Monster1Part1();
                this.PreviousMonster1Part2Threshold = this.Monster1Part2();
                this.PreviousMonster1Part3Threshold = this.Monster1Part3();
                this.PreviousMonster1Part4Threshold = this.Monster1Part4();
                this.PreviousMonster1Part5Threshold = this.Monster1Part5();
                this.PreviousMonster1Part6Threshold = this.Monster1Part6();
                this.PreviousMonster1Part7Threshold = this.Monster1Part7();
                this.PreviousMonster1Part8Threshold = this.Monster1Part8();
                this.PreviousMonster1Part9Threshold = this.Monster1Part9();
                this.PreviousMonster1Part10Threshold = this.Monster1Part10();
                Dictionary<int, List<int>> monster1PartThresholdDictionaryMonsterInfo = new ();
                var partsList = new List<int>()
                {
                    this.Monster1Part1(),
                    this.Monster1Part2(),
                    this.Monster1Part3(),
                    this.Monster1Part4(),
                    this.Monster1Part5(),
                    this.Monster1Part6(),
                    this.Monster1Part7(),
                    this.Monster1Part8(),
                    this.Monster1Part9(),
                    this.Monster1Part10(),
                };
                monster1PartThresholdDictionaryMonsterInfo.Add(this.LargeMonster1ID(), partsList);
                this.Monster1PartThresholdDictionary.Add(this.TimeInt(), monster1PartThresholdDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster1PartThresholdDictionary");
            }
        }

        if ((this.PreviousMonster2Part1Threshold != this.Monster2Part1() ||
            this.PreviousMonster2Part2Threshold != this.Monster2Part2() ||
            this.PreviousMonster2Part3Threshold != this.Monster2Part3() ||
            this.PreviousMonster2Part4Threshold != this.Monster2Part4() ||
            this.PreviousMonster2Part5Threshold != this.Monster2Part5() ||
            this.PreviousMonster2Part6Threshold != this.Monster2Part6() ||
            this.PreviousMonster2Part7Threshold != this.Monster2Part7() ||
            this.PreviousMonster2Part8Threshold != this.Monster2Part8() ||
            this.PreviousMonster2Part9Threshold != this.Monster2Part9() ||
            this.PreviousMonster2Part10Threshold != this.Monster2Part10())
            && !this.Monster2PartThresholdDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousMonster2Part1Threshold = this.Monster2Part1();
                this.PreviousMonster2Part2Threshold = this.Monster2Part2();
                this.PreviousMonster2Part3Threshold = this.Monster2Part3();
                this.PreviousMonster2Part4Threshold = this.Monster2Part4();
                this.PreviousMonster2Part5Threshold = this.Monster2Part5();
                this.PreviousMonster2Part6Threshold = this.Monster2Part6();
                this.PreviousMonster2Part7Threshold = this.Monster2Part7();
                this.PreviousMonster2Part8Threshold = this.Monster2Part8();
                this.PreviousMonster2Part9Threshold = this.Monster2Part9();
                this.PreviousMonster2Part10Threshold = this.Monster2Part10();
                Dictionary<int, List<int>> monster2PartThresholdDictionaryMonsterInfo = new ();
                var partsList = new List<int>()
                {
                    this.Monster2Part1(),
                    this.Monster2Part2(),
                    this.Monster2Part3(),
                    this.Monster2Part4(),
                    this.Monster2Part5(),
                    this.Monster2Part6(),
                    this.Monster2Part7(),
                    this.Monster2Part8(),
                    this.Monster2Part9(),
                    this.Monster2Part10(),
                };
                monster2PartThresholdDictionaryMonsterInfo.Add(this.LargeMonster2ID(), partsList);
                this.Monster2PartThresholdDictionary.Add(this.TimeInt(), monster2PartThresholdDictionaryMonsterInfo);
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into monster2PartThresholdDictionary");
            }
        }

        if (this.PreviousDualSwordsSharpens != this.DualSwordsSharpens() && !this.DualSwordsSharpensDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousDualSwordsSharpens = this.DualSwordsSharpens();
                this.DualSwordsSharpensDictionary.Add(this.TimeInt(), this.DualSwordsSharpens());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into DualSwordsSharpensDictionary");
            }
        }

        if (this.PreviousPartySize != this.PartySize() && !this.PartySizeDictionary.ContainsKey(this.TimeInt()))
        {
            try
            {
                this.PreviousPartySize = this.PartySize();
                this.PartySizeDictionary.Add(this.TimeInt(), this.PartySize());
            }
            catch (Exception ex)
            {
                LoggerInstance.Warn(ex, "Could not insert into PartySizeDictionary");
            }
        }
    }

    public void ResetQuestInfoVariables()
    {
        this.PouchItem1IDAtQuestStart = 0;
        this.PouchItem2IDAtQuestStart = 0;
        this.PouchItem3IDAtQuestStart = 0;
        this.PouchItem4IDAtQuestStart = 0;
        this.PouchItem5IDAtQuestStart = 0;
        this.PouchItem6IDAtQuestStart = 0;
        this.PouchItem7IDAtQuestStart = 0;
        this.PouchItem8IDAtQuestStart = 0;
        this.PouchItem9IDAtQuestStart = 0;
        this.PouchItem10IDAtQuestStart = 0;
        this.PouchItem11IDAtQuestStart = 0;
        this.PouchItem12IDAtQuestStart = 0;
        this.PouchItem13IDAtQuestStart = 0;
        this.PouchItem14IDAtQuestStart = 0;
        this.PouchItem15IDAtQuestStart = 0;
        this.PouchItem16IDAtQuestStart = 0;
        this.PouchItem17IDAtQuestStart = 0;
        this.PouchItem18IDAtQuestStart = 0;
        this.PouchItem19IDAtQuestStart = 0;
        this.PouchItem20IDAtQuestStart = 0;
        this.PouchItem1QuantityAtQuestStart = 0;
        this.PouchItem2QuantityAtQuestStart = 0;
        this.PouchItem3QuantityAtQuestStart = 0;
        this.PouchItem4QuantityAtQuestStart = 0;
        this.PouchItem5QuantityAtQuestStart = 0;
        this.PouchItem6QuantityAtQuestStart = 0;
        this.PouchItem7QuantityAtQuestStart = 0;
        this.PouchItem8QuantityAtQuestStart = 0;
        this.PouchItem9QuantityAtQuestStart = 0;
        this.PouchItem10QuantityAtQuestStart = 0;
        this.PouchItem11QuantityAtQuestStart = 0;
        this.PouchItem12QuantityAtQuestStart = 0;
        this.PouchItem13QuantityAtQuestStart = 0;
        this.PouchItem14QuantityAtQuestStart = 0;
        this.PouchItem15QuantityAtQuestStart = 0;
        this.PouchItem16QuantityAtQuestStart = 0;
        this.PouchItem17QuantityAtQuestStart = 0;
        this.PouchItem18QuantityAtQuestStart = 0;
        this.PouchItem19QuantityAtQuestStart = 0;
        this.PouchItem20QuantityAtQuestStart = 0;

        this.AmmoPouchItem1IDAtQuestStart = 0;
        this.AmmoPouchItem2IDAtQuestStart = 0;
        this.AmmoPouchItem3IDAtQuestStart = 0;
        this.AmmoPouchItem4IDAtQuestStart = 0;
        this.AmmoPouchItem5IDAtQuestStart = 0;
        this.AmmoPouchItem6IDAtQuestStart = 0;
        this.AmmoPouchItem7IDAtQuestStart = 0;
        this.AmmoPouchItem8IDAtQuestStart = 0;
        this.AmmoPouchItem9IDAtQuestStart = 0;
        this.AmmoPouchItem10IDAtQuestStart = 0;
        this.AmmoPouchItem1QuantityAtQuestStart = 0;
        this.AmmoPouchItem2QuantityAtQuestStart = 0;
        this.AmmoPouchItem3QuantityAtQuestStart = 0;
        this.AmmoPouchItem4QuantityAtQuestStart = 0;
        this.AmmoPouchItem5QuantityAtQuestStart = 0;
        this.AmmoPouchItem6QuantityAtQuestStart = 0;
        this.AmmoPouchItem7QuantityAtQuestStart = 0;
        this.AmmoPouchItem8QuantityAtQuestStart = 0;
        this.AmmoPouchItem9QuantityAtQuestStart = 0;
        this.AmmoPouchItem10QuantityAtQuestStart = 0;

        this.PartnyaBagItem1IDAtQuestStart = 0;
        this.PartnyaBagItem2IDAtQuestStart = 0;
        this.PartnyaBagItem3IDAtQuestStart = 0;
        this.PartnyaBagItem4IDAtQuestStart = 0;
        this.PartnyaBagItem5IDAtQuestStart = 0;
        this.PartnyaBagItem6IDAtQuestStart = 0;
        this.PartnyaBagItem7IDAtQuestStart = 0;
        this.PartnyaBagItem8IDAtQuestStart = 0;
        this.PartnyaBagItem9IDAtQuestStart = 0;
        this.PartnyaBagItem10IDAtQuestStart = 0;
        this.PartnyaBagItem1QuantityAtQuestStart = 0;
        this.PartnyaBagItem2QuantityAtQuestStart = 0;
        this.PartnyaBagItem3QuantityAtQuestStart = 0;
        this.PartnyaBagItem4QuantityAtQuestStart = 0;
        this.PartnyaBagItem5QuantityAtQuestStart = 0;
        this.PartnyaBagItem6QuantityAtQuestStart = 0;
        this.PartnyaBagItem7QuantityAtQuestStart = 0;
        this.PartnyaBagItem8QuantityAtQuestStart = 0;
        this.PartnyaBagItem9QuantityAtQuestStart = 0;
        this.PartnyaBagItem10QuantityAtQuestStart = 0;

        this.PreviousAttackBuffInt = 0;
        this.PreviousDPS = 0;
        this.PreviousHitCountInt = 0;
        this.PreviousAreaID = 0;
        this.PreviousCartsInt = 0;
        this.PreviousMonster1HP = 0;
        this.PreviousMonster2HP = 0;
        this.PreviousMonster3HP = 0;
        this.PreviousMonster4HP = 0;
        this.PreviousTotalInventoryItems = 0;
        this.PreviousTotalAmmo = 0;
        this.PreviousTotalPartnyaItems = 0;
        this.PreviousHitsTakenBlocked = 0;
        this.PreviousTotalHitsTakenBlockedPerSecond = 0;
        this.PreviousPlayerHP = 0;
        this.PreviousPlayerStamina = 0;
        this.PreviousHitsPerSecond = 0;
        this.PreviousActionsPerMinute = 0;
        this.PreviousOverlayMode = OverlayMode.Unknown;
        this.PreviousRoadFloor = 0;

        this.PreviousMonster1AttackMultiplier = 0;
        this.PreviousMonster1DefenseRate = 0;
        this.PreviousMonster1SizeMultiplier = 0;
        this.PreviousMonster1PoisonThreshold = 0;
        this.PreviousMonster1SleepThreshold = 0;
        this.PreviousMonster1ParalysisThreshold = 0;
        this.PreviousMonster1BlastThreshold = 0;
        this.PreviousMonster1StunThreshold = 0;
        this.PreviousMonster1Part1Threshold = 0;
        this.PreviousMonster1Part2Threshold = 0;
        this.PreviousMonster1Part3Threshold = 0;
        this.PreviousMonster1Part4Threshold = 0;
        this.PreviousMonster1Part5Threshold = 0;
        this.PreviousMonster1Part6Threshold = 0;
        this.PreviousMonster1Part7Threshold = 0;
        this.PreviousMonster1Part8Threshold = 0;
        this.PreviousMonster1Part9Threshold = 0;
        this.PreviousMonster1Part10Threshold = 0;
        this.PreviousMonster2Part1Threshold = 0;
        this.PreviousMonster2Part2Threshold = 0;
        this.PreviousMonster2Part3Threshold = 0;
        this.PreviousMonster2Part4Threshold = 0;
        this.PreviousMonster2Part5Threshold = 0;
        this.PreviousMonster2Part6Threshold = 0;
        this.PreviousMonster2Part7Threshold = 0;
        this.PreviousMonster2Part8Threshold = 0;
        this.PreviousMonster2Part9Threshold = 0;
        this.PreviousMonster2Part10Threshold = 0;
        this.PreviousDualSwordsSharpens = 0;
        this.PreviousPartySize = 0;
    }

    public void ClearQuestInfoDictionaries()
    {
        this.AttackBuffDictionary.Clear();
        this.HitCountDictionary.Clear();
        this.DamageDealtDictionary.Clear();
        this.DamagePerSecondDictionary.Clear();
        this.AreaChangesDictionary.Clear();
        this.CartsDictionary.Clear();
        this.Monster1HPDictionary.Clear();
        this.Monster2HPDictionary.Clear();
        this.Monster3HPDictionary.Clear();
        this.Monster4HPDictionary.Clear();
        this.PlayerInventoryDictionary.Clear();
        this.PlayerAmmoPouchDictionary.Clear();
        this.PartnyaBagDictionary.Clear();
        this.HitsTakenBlockedDictionary.Clear();
        this.PlayerHPDictionary.Clear();
        this.PlayerStaminaDictionary.Clear();
        this.HitsPerSecondDictionary.Clear();
        this.HitsTakenBlockedPerSecondDictionary.Clear();
        this.KeystrokesDictionary.Clear();
        this.MouseInputDictionary.Clear();
        this.GamepadInputDictionary.Clear();
        this.ActionsPerMinuteDictionary.Clear();
        this.OverlayModeDictionary.Clear();

        this.Monster1AttackMultiplierDictionary.Clear();
        this.Monster1DefenseRateDictionary.Clear();
        this.Monster1SizeMultiplierDictionary.Clear();
        this.Monster1PoisonThresholdDictionary.Clear();
        this.Monster1SleepThresholdDictionary.Clear();
        this.Monster1ParalysisThresholdDictionary.Clear();
        this.Monster1BlastThresholdDictionary.Clear();
        this.Monster1StunThresholdDictionary.Clear();
        this.Monster1PartThresholdDictionary.Clear();
        this.Monster2PartThresholdDictionary.Clear();

        this.DualSwordsSharpensDictionary.Clear();
        this.PartySizeDictionary.Clear();
    }

    public void ClearGraphCollections()
    {
        lock (this.AttackBuffSync)
        {
            // Any changes including adding, clearing, etc must be synced.
            this.AttackBuffCollection.Clear();
        }

        lock (this.ActionsPerMinuteSync)
        {
            this.ActionsPerMinuteCollection.Clear();
        }

        lock (this.DamagePerSecondSync)
        {
            this.DamagePerSecondCollection.Clear();
        }

        lock (this.HitsPerSecondSync)
        {
            this.HitsPerSecondCollection.Clear();
        }
    }

    public string OverlayModeWatermarkText
    {
        get
        {
            if (ShowOverlayModeFinalMode())
            {
                return GetFinalOverlayModeForDisplay();
            }

            if (GetOverlayModeForStorage() == "Speedrun")
            {
                return $"Speedrun ({GetRunBuffsTag(GetRunBuffs(), (QuestVariant2)QuestVariant2(), (QuestVariant3)QuestVariant3())})";
            }

            return GetOverlayModeForStorage();
        }
    }

    public string QuestIDBind => this.QuestID().ToString(CultureInfo.InvariantCulture);

    public ObservableCollection<RecentRuns> RecentRuns { get; set; } = new ();

    // TODO: the plural/singular is inconsistent
    public List<FastestRun> FastestRuns { get; set; } = new ();

    public List<RecentRuns> CalendarRuns { get; set; } = new ();

    public List<Achievement> PlayerAchievements { get; set; } = new ();

    public List<Achievement> ObtainablePlayerAchievements { get; set; } = new ();

    public ReadOnlyDictionary<int, Challenge> PlayerChallenges { get; set; }

    public ObservableCollection<QuestLogsOption> QuestLogsSearchOption { get; set; } = new ObservableCollection<QuestLogsOption>()
    {
        new QuestLogsOption { Name = "Compendium", IsSelected = false },
        new QuestLogsOption { Name = "Calendar", IsSelected = false },
        new QuestLogsOption { Name = "Personal Best", IsSelected = false },
        new QuestLogsOption { Name = "Gear", IsSelected = false },
        new QuestLogsOption { Name = "Top 20", IsSelected = false },
        new QuestLogsOption { Name = "Weapon Stats", IsSelected = false },
        new QuestLogsOption { Name = "Most Recent", IsSelected = false },
        new QuestLogsOption { Name = "YouTube", IsSelected = false },
        new QuestLogsOption { Name = "Stats (Graphs)", IsSelected = false },
        new QuestLogsOption { Name = "Stats (Text)", IsSelected = false },
        new QuestLogsOption { Name = "Quest Pace", IsSelected = false },
    };

    public ObservableCollection<QuestLogsOption> RunBuffsSearchOption { get; set; } = new ObservableCollection<QuestLogsOption>()
    {
        new QuestLogsOption { Name = "Halk", IsSelected = false },
        new QuestLogsOption { Name = "Poogie Item", IsSelected = false },
        new QuestLogsOption { Name = "Diva Song", IsSelected = false },
        new QuestLogsOption { Name = "Halk Pot Effect", IsSelected = false },
        new QuestLogsOption { Name = "Bento", IsSelected = false },
        new QuestLogsOption { Name = "Guild Poogie", IsSelected = false },
        new QuestLogsOption { Name = "Active Feature", IsSelected = false },
        new QuestLogsOption { Name = "Guild Food", IsSelected = false },
        new QuestLogsOption { Name = "Diva Skill", IsSelected = false },
        new QuestLogsOption { Name = "Secret Technique", IsSelected = false },
        new QuestLogsOption { Name = "Diva Prayer Gem", IsSelected = false },
        new QuestLogsOption { Name = "Course Attack Boost", IsSelected = false },
    };

    public QuestLogsOption SelectedOption { get; set; } = new QuestLogsOption { Name = "Default", IsSelected = true };

    public static string StaticReplaceFirstFF(string hexColor)
    {
        var index = hexColor.IndexOf("ff", StringComparison.InvariantCulture);
        if (index != -1)
        {
            hexColor = hexColor.Remove(index, 2).Insert(index, "00");
        }

        return hexColor;
    }

    public static uint StaticHexColorToDecimal(string hexColor, bool? toTransparent = false)
    {
        if (hexColor.StartsWith("#", StringComparison.InvariantCulture))
        {
            hexColor = hexColor[1..];
        }

        if (hexColor.Length == 6)
        {
            hexColor = hexColor.Insert(0, "ff");
        }
        else
        {
            hexColor = hexColor.Remove(0, 2).Insert(0, "ff");
        }

        if (toTransparent is not null and true)
        {
            hexColor = StaticReplaceFirstFF(hexColor);
        }

        return uint.Parse(hexColor, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }

    public static string ReplaceAlphaChannel(string hexColor, string alphaChannel)
    {
        var index = hexColor.IndexOf("ff", StringComparison.InvariantCulture);
        if (index != -1)
        {
            hexColor = hexColor.Remove(index, 2).Insert(index, alphaChannel);
        }

        return hexColor;
    }

    public uint HexColorToDecimal(string hexColor, string? alphaChannel = "ff")
    {
        if (hexColor.StartsWith("#", StringComparison.InvariantCulture))
        {
            hexColor = hexColor[1..];
        }

        if (hexColor.Length == 6)
        {
            hexColor = hexColor.Insert(0, "ff");
        }
        else
        {
            hexColor = hexColor.Remove(0, 2).Insert(0, "ff");
        }

        if (alphaChannel is not null and not "ff")
        {
            hexColor = ReplaceAlphaChannel(hexColor, alphaChannel);
        }

        return uint.Parse(hexColor, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }

    public TimeSpan CurrentSessionTime
    {
        get
        {
            var programEnd = DateTime.UtcNow;
            var duration = programEnd - this.programStart;
            return duration;
        }
    }

    public string GuildFoodTimeLeft
    {
        get
        {
            if (GuildFoodStart() <= 0)
            {
                return "0m";
            }

            var expiry  = GuildFoodStart() + (60 * 90);
            double secondsLeft = expiry - ServerHeartbeat;

            if (secondsLeft <= 0)
            {
                return "0m";
            }

            return $"{Math.Truncate(secondsLeft/60.0)}m";
        }
    }

    public string DivaSongTimeLeft
    {
        get
        {
            var divaSongStart = Math.Max(DivaSongStart(), DivaSongFromGuildStart());

            if (divaSongStart <= 0)
            {
                return "0m";
            }

            var expiry = divaSongStart + (60 * 90);
            double secondsLeft = expiry - ServerHeartbeat;

            if (secondsLeft <= 0)
            {
                return "0m";
            }

            return $"{Math.Truncate(secondsLeft / 60.0)}m";
        }
    }

    public string CurrentPlayerPosition
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.PlayerPositionMode == "Automatic")
            {
                if (this.QuestID() != 0)
                {
                    return $"{decimal.Truncate(PlayerPositionInQuestX())}, {decimal.Truncate(PlayerPositionInQuestY())}, {decimal.Truncate(PlayerPositionInQuestZ())}";
                }
                else
                {
                    return $"{decimal.Truncate(PlayerPositionX())}, {decimal.Truncate(PlayerPositionY())}, {decimal.Truncate(PlayerPositionZ())}";
                }

            } else if (s.PlayerPositionMode == "Lobby")
            {
                return $"{decimal.Truncate(PlayerPositionX())}, {decimal.Truncate(PlayerPositionY())}, {decimal.Truncate(PlayerPositionZ())}";

            } else if (s.PlayerPositionMode == "Quest")
            {
                return $"{decimal.Truncate(PlayerPositionInQuestX())}, {decimal.Truncate(PlayerPositionInQuestY())}, {decimal.Truncate(PlayerPositionInQuestZ())}";

            }

            return "?, ?, ?";
        }
    } 

    public int CurrentMonster1MaxHP { get; set; }

    /// <summary>
    /// Shows the current hp percentage.
    /// </summary>
    /// <returns></returns>
    public static bool ShowCurrentHPPercentage()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableCurrentHPPercentage;
    }

    public static bool ShowOverlayModeFinalMode()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        return s.OverlayWatermarkMode == "Final";
    }

    public static string FindAreaIcon(int id, bool forDiscord = false)
    {
        var areaGroup = new List<int> { 0 };

        foreach (var kvp in AreaIcons.AreaIconID)
        {
            var areaIDs = kvp.Key;

            if (areaIDs.Contains(id))
            {
                areaGroup = kvp.Key;
                break;
            }
        }

        return DetermineAreaIcon(areaGroup, forDiscord);
    }

    /// <summary>
    /// Gets the monster1 ehp percent.
    /// </summary>
    /// <returns></returns>
    public string GetMonster1EHPPercent()
    {
        if (!int.TryParse(this.Monster1HP, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedMonster1HP))
        {
            // Handle the case when Monster1HP cannot be parsed to an int
            // For example, you can return an error message or set some default value
            LoggerInstance.Warn(CultureInfo.InvariantCulture, "Could not parse monster 1 HP to get monster 1 EHP Percent: {0}", this.Monster1HP);
            return " (0%)";
        }

        if (this.CurrentMonster1MaxHP < parsedMonster1HP)
        {
            this.CurrentMonster1MaxHP = parsedMonster1HP;
        }

        if (this.CurrentMonster1MaxHP == 0 || this.GetMonster1EHP() == 0) // should be OK
        {
            this.CurrentMonster1MaxHP = 1;
        }

        if (!ShowCurrentHPPercentage())
        {
            return string.Empty;
        }

        return string.Format(CultureInfo.InvariantCulture, " ({0:0}%)", (float)parsedMonster1HP / this.CurrentMonster1MaxHP * 100.0);
    }

    /// <summary>
    /// Gets the monster1 ehp.
    /// </summary>
    /// <returns></returns>
    public int GetMonster1EHP() => this.DisplayMonsterEHP(1, this.Monster1DefMult(), this.Monster1HPInt());

    /// <summary>
    /// Gets the monster1 maximum ehp.
    /// </summary>
    /// <returns></returns>
    public int GetMonster1MaxEHP() => this.CurrentMonster1MaxHP;

    /// <summary>
    /// Gets the max faints.
    /// TODO optimize
    /// </summary>
    /// <returns></returns>
    public string GetMaxFaints()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");

        switch (s.MaxFaintsOverride)
        {
            default:
                return this.MaxFaints().ToString(CultureInfo.InvariantCulture);
            case "Normal Quests":
                return this.MaxFaints().ToString(CultureInfo.InvariantCulture);
            case "Shiten/Conquest/Pioneer/Daily/Caravan/Interception Quests":
                return this.AlternativeMaxFaints().ToString(CultureInfo.InvariantCulture);
            case "Automatic":
                if (this.RoadOverride() is not null and false) //TODO test
                {
                    return this.MaxFaints().ToString(CultureInfo.InvariantCulture);
                }

                if
                (

                        (this.AlternativeQuestOverride() && !(
                            this.QuestID() == 23603 ||
                            this.RankBand() == 70 ||
                            this.QuestID() == 23602 ||
                            this.QuestID() == 23604 ||
                            this.QuestID() == 23588 ||
                            this.QuestID() == 23592 ||
                            this.QuestID() == 23596 ||
                            this.QuestID() == 23601 ||
                            this.QuestID() == 23599 ||
                            this.QuestID() == 23595 ||
                            this.QuestID() == 23591 ||
                            this.QuestID() == 23587 ||
                            this.QuestID() == 23598 ||
                            this.QuestID() == 23594 ||
                            this.QuestID() == 23590 ||
                            this.QuestID() == 23586 ||
                            this.QuestID() == 23597 ||
                            this.QuestID() == 23593 ||
                            this.QuestID() == 23589 ||
                            this.QuestID() == 23585))

                    ||

                    this.QuestID() == 23603 ||
                    this.RankBand() == 70 ||
                    this.QuestID() == 23602 ||
                    this.QuestID() == 23604 ||
                    this.QuestID() == 23588 ||
                    this.QuestID() == 23592 ||
                    this.QuestID() == 23596 ||
                    this.QuestID() == 23601 ||
                    this.QuestID() == 23599 ||
                    this.QuestID() == 23595 ||
                    this.QuestID() == 23591 ||
                    this.QuestID() == 23587 ||
                    this.QuestID() == 23598 ||
                    this.QuestID() == 23594 ||
                    this.QuestID() == 23590 ||
                    this.QuestID() == 23586 ||
                    this.QuestID() == 23597 ||
                    this.QuestID() == 23593 ||
                    this.QuestID() == 23589 ||
                    this.QuestID() == 23585)
                {
                    return this.AlternativeMaxFaints().ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    return this.MaxFaints().ToString(CultureInfo.InvariantCulture);
                }
        }
    }

    /// <summary>
    /// Gets the color of the armor.
    /// </summary>
    /// <returns></returns>
    public string GetArmorColor()
    {
        EZlion.Mapper.ArmorColor.IDName.TryGetValue(this.ArmorColor(), out var colorname);
        return colorname + string.Empty;
    }

    /// <summary>
    /// Gets the weapon style from identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetWeaponStyleFromID(int id) => id switch
    {
        0 => "Earth",
        1 => "Heaven",
        2 => "Storm",
        3 => "Extreme",
        _ => "None",
    };

    /// <summary>
    /// Gets the area icon from identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetAreaIconFromID(int id, bool forDiscord = false) // TODO: are highlands, tidal island or painted falls icons correct?
    {
        if (id >= 470 && id < 0)
        {
            if (forDiscord)
            {
                return "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/icon/cattleya.png";
            }

            return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/cattleya.png";
        }
        else
        {
            return FindAreaIcon(id, forDiscord);
        }
    }

    public static string DetermineAreaIcon(List<int> key, bool forDiscord = false)
    {
        var areaIcon = AreaIcons.AreaIconID.ContainsKey(key);
        if (!areaIcon)
        {
            if (forDiscord)
            {
                return "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/icon/cattleya.png";
            }

            return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/cattleya.png";
        }
        else
        {
            var areaIconValue = AreaIcons.AreaIconID[key];
            if (forDiscord)
            {
                areaIconValue = areaIconValue.ToString().Replace(@"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/", "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/icon/");
            }

            return areaIconValue;
        }
    }

    /// <summary>
    /// Gets the poogie clothes.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static string GetPoogieClothes(int id)
    {
        _ = EZlion.Mapper.PoogieCostume.IDName.TryGetValue(id, out var clothesValue1);  // returns true
        return clothesValue1 + string.Empty;
    }

    /// <summary>
    /// Gets the caravan skills.
    /// </summary>
    /// <returns></returns>
    public static string GetCaravanSkillsWithoutMarkdown(DataLoader dataLoader)
    {
        var id1 = dataLoader.Model.CaravanSkill1();
        var id2 = dataLoader.Model.CaravanSkill2();
        var id3 = dataLoader.Model.CaravanSkill3();

        SkillCaravan.IDName.TryGetValue(id1, out var caravanSkillName1);
        SkillCaravan.IDName.TryGetValue(id2, out var caravanSkillName2);
        SkillCaravan.IDName.TryGetValue(id3, out var caravanSkillName3);

        if (caravanSkillName1 == string.Empty || caravanSkillName1 == "None")
        {
            return "None";
        }
        else if (caravanSkillName2 == string.Empty || caravanSkillName2 == "None")
        {
            return caravanSkillName1 + string.Empty;
        }
        else if (caravanSkillName3 == string.Empty || caravanSkillName3 == "None")
        {
            return caravanSkillName1 + ", " + caravanSkillName2;
        }
        else
        {
            return caravanSkillName1 + ", " + caravanSkillName2 + ", " + caravanSkillName3;
        }
    }

    /// <summary>
    /// Reloads the data.
    /// </summary>
    public void ReloadData(string? propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
