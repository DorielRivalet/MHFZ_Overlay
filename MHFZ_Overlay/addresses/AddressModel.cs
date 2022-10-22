using Dictionary;
using Memory;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace MHFZ_Overlay.addresses
{
    public abstract class AddressModel : INotifyPropertyChanged
    {
        #region properties

        public readonly Mem M;

        public int SavedMonster1MaxHP = 0;
        public int SavedMonster2MaxHP = 0;
        private int SavedMonster3MaxHP = 0;
        private int SavedMonster4MaxHP = 0;


        private int SavedMonster1ID = 0;
        private int SavedMonster2ID = 0;

        public AddressModel(Mem m) => M = m;

        public int SelectedMonster = 0;

        //public string SavedGender = "";
        //public string SavedWeaponClass = "";
        //public string SavedTextFormat = "";
        public string SavedGearStats = "";

        #endregion

        #region init bool

        public bool ShowMonsterInfos { get; set; } = true;

        public bool ShowMonsterAtkMult { get; set; } = true;

        public bool ShowMonsterDefrate { get; set; } = true;

        public bool ShowMonsterSize { get; set; } = true;
        public bool ShowMonsterPoison { get; set; } = true;
        public bool ShowMonsterSleep { get; set; } = true;
        public bool ShowMonsterPara { get; set; } = true;
        public bool ShowMonsterBlast { get; set; } = true;
        public bool ShowMonsterStun { get; set; } = true;


        public bool ShowPlayerInfos { get; set; } = true;

        public bool ShowTimerInfo { get; set; } = true;
        public bool ShowHitCountInfo { get; set; } = true;
        public bool ShowPlayerAtkInfo { get; set; } = true;
       


        public bool ShowMonsterHPBars { get; set; } = true;

        public bool ShowMonster1HPBar { get; set; } = true;
        public bool ShowMonster2HPBar { get; set; } = true;
        public bool ShowMonster3HPBar { get; set; } = true;
        public bool ShowMonster4HPBar { get; set; } = true;

        public bool ShowSharpness { get; set; } = true;

        public bool ShowMonsterPartHP { get; set; } = true;

        public bool ShowMonster1Icon { get; set; } = true;

        public bool ShowMap { get; set; } = true;


        #endregion

        #region abstract vars
        abstract public bool IsNotRoad();

        abstract public int HitCountInt();
        abstract public int DamageDealt();





        //New addresses
        abstract public int AreaID();
        abstract public int GRankNumber();
        abstract public int GSR();
        abstract public int RoadFloor();
        abstract public int WeaponStyle();
        abstract public int QuestID();
        abstract public int UrukiPachinkoFish();
        abstract public int UrukiPachinkoMushroom();
        abstract public int UrukiPachinkoSeed();
        abstract public int UrukiPachinkoMeat() ;
        abstract public int UrukiPachinkoChain() ;
        abstract public int UrukiPachinkoScore();
        abstract public int NyanrendoScore() ;
        abstract public int DokkanBattleCatsScore() ;
        abstract public int DokkanBattleCatsScale();
        abstract public int DokkanBattleCatsShell() ;
        abstract public int DokkanBattleCatsCamp();
        abstract public int GuukuScoopSmall() ;
        abstract public int GuukuScoopMedium() ;
        abstract public int GuukuScoopLarge() ;
        abstract public int GuukuScoopGolden() ;
        abstract public int GuukuScoopScore() ;
        abstract public int PanicHoneyScore() ;
        abstract public int Sharpness();
        abstract public int CaravanPoints() ;
        abstract public int MezeportaFestivalPoints();
        abstract public int DivaBond() ;
        abstract public int DivaItemsGiven() ;
        abstract public int GCP();
        abstract public int RoadPoints() ;
        abstract public int ArmorColor() ;
        abstract public int RaviGg() ;
        abstract public int Ravig() ;
        abstract public int GZenny();
        abstract public int GuildFoodSkill() ;
        abstract public int GalleryEvaluationScore();
        abstract public int PoogiePoints() ;
        abstract public int PoogieItemUseID() ;
        abstract public int PoogieCostume();
        //zero-indexed
        abstract public int CaravenGemLevel() ;
        abstract public int RoadMaxStagesMultiplayer();
        abstract public int RoadTotalStagesMultiplayer() ;
        abstract public int RoadTotalStagesSolo();
        abstract public int RoadMaxStagesSolo() ;
        abstract public int RoadFatalisSlain()  ;
        abstract public int RoadFatalisEncounters() ;
        abstract public int FirstDistrictDuremudiraEncounters();
        abstract public int FirstDistrictDuremudiraSlays();
        abstract public int SecondDistrictDuremudiraEncounters() ;
        abstract public int SecondDistrictDuremudiraSlays() ;
        abstract public int DeliveryQuestPoints(); //doesn't seem to work


        //red is 0
        abstract public int SharpnessLevel();


        abstract public int PartnerLevel();
        abstract public int ObjectiveType();
        abstract public int DivaSkillUsesLeft();
        abstract public int HalkFullness();
        abstract public int RankBand();

        //abstract public int PartnyaRank();
        abstract public int PartnyaRankPoints();
        //parts
        abstract public int Monster1Part1();
        abstract public int Monster1Part2();
        abstract public int Monster1Part3();
        abstract public int Monster1Part4();
        abstract public int Monster1Part5();
        abstract public int Monster1Part6();
        abstract public int Monster1Part7();
        abstract public int Monster1Part8();
        abstract public int Monster1Part9();
        abstract public int Monster1Part10();
        abstract public int Monster2Part1();
        abstract public int Monster2Part2();
        abstract public int Monster2Part3();
        abstract public int Monster2Part4();
        abstract public int Monster2Part5();
        abstract public int Monster2Part6();
        abstract public int Monster2Part7();
        abstract public int Monster2Part8();
        abstract public int Monster2Part9();
        abstract public int Monster2Part10();



        abstract public int TimeInt();
        abstract public int TimeDefInt();

        abstract public int WeaponRaw();
        abstract public int WeaponType();

        abstract public int LargeMonster1ID();
        abstract public int LargeMonster2ID();
        abstract public int LargeMonster3ID();
        abstract public int LargeMonster4ID();

        abstract public int Monster1HPInt();
        abstract public int Monster2HPInt();
        abstract public int Monster3HPInt();
        abstract public int Monster4HPInt();

        abstract public string Monster1AtkMult();
        abstract public string Monster2AtkMult();

        abstract public string Monster1DefMult();
        abstract public string Monster2DefMult();

        abstract public int Monster1Poison();
        abstract public int Monster1PoisonNeed();
        abstract public int Monster1Sleep();
        abstract public int Monster1SleepNeed();
        abstract public int Monster1Para();
        abstract public int Monster1ParaNeed();
        abstract public int Monster1Blast();
        abstract public int Monster1BlastNeed();
        abstract public int Monster1Stun();
        abstract public int Monster1StunNeed();
        abstract public string Monster1Size();

        abstract public int Monster2Poison();
        abstract public int Monster2PoisonNeed();
        abstract public int Monster2Sleep();
        abstract public int Monster2SleepNeed();
        abstract public int Monster2Para();
        abstract public int Monster2ParaNeed();
        abstract public int Monster2Blast();
        abstract public int Monster2BlastNeed();
        abstract public int Monster2Stun();
        abstract public int Monster2StunNeed();
        abstract public string Monster2Size();
        abstract public int Objective1ID();
        abstract public int Objective1Quantity();
        abstract public int Objective1CurrentQuantityMonster();
        abstract public int Objective1CurrentQuantityItem();

        //ravi
        abstract public int RavienteTriggeredEvent();
        abstract public int GreatSlayingPoints();
        abstract public int GreatSlayingPointsSaved();
        //normal and violent. berserk support
        abstract public int RavienteAreaID();


        abstract public int RoadSelectedMonster();


        //Yamas and Beru
        abstract public int AlternativeMonster1HPInt();
        abstract public int AlternativeMonster1AtkMult();
        abstract public int AlternativeMonster1DefMult();
        abstract public int AlternativeMonster1Size();

        abstract public int AlternativeMonster1Poison();
        abstract public int AlternativeMonster1PoisonNeed();
        abstract public int AlternativeMonster1Sleep();
        abstract public int AlternativeMonster1SleepNeed();

        abstract public int AlternativeMonster1Para();
        abstract public int AlternativeMonster1ParaNeed();
        abstract public int AlternativeMonster1Blast();
        abstract public int AlternativeMonster1BlastNeed();

        abstract public int AlternativeMonster1Stun();
        abstract public int AlternativeMonster1StunNeed();

        abstract public int AlternativeMonster1Part1();
        abstract public int AlternativeMonster1Part2();
        abstract public int AlternativeMonster1Part3();
        abstract public int AlternativeMonster1Part4();
        abstract public int AlternativeMonster1Part5();
        abstract public int AlternativeMonster1Part6();
        abstract public int AlternativeMonster1Part7();
        abstract public int AlternativeMonster1Part8();
        abstract public int AlternativeMonster1Part9();
        abstract public int AlternativeMonster1Part10();

        abstract public int DivaSkill();
        abstract public int StarGrades();

        abstract public int CaravanSkill1();
        abstract public int CaravanSkill2();
        abstract public int CaravanSkill3();

        abstract public int CurrentFaints();
        //road and normal
        abstract public int MaxFaints();
        //shitens, conquests, pioneer, daily, caravan, interception
        abstract public int AlternativeMaxFaints();

        abstract public int CaravanScore();

        abstract public int CaravanMonster1ID();
        //unsure
        abstract public int CaravanMonster2ID();

        abstract public int MeleeWeaponID();
        abstract public int RangedWeaponID();
        //TODO: Sigils
        abstract public int WeaponDeco1ID();
        abstract public int WeaponDeco2ID();
        abstract public int WeaponDeco3ID();
        abstract public int ArmorHeadID();
        abstract public int ArmorHeadDeco1ID();
        abstract public int ArmorHeadDeco2ID();
        abstract public int ArmorHeadDeco3ID();
        abstract public int ArmorChestID();
        abstract public int ArmorChestDeco1ID();
        abstract public int ArmorChestDeco2ID();
        abstract public int ArmorChestDeco3ID();
        abstract public int ArmorArmsID();
        abstract public int ArmorArmsDeco1ID();
        abstract public int ArmorArmsDeco2ID();
        abstract public int ArmorArmsDeco3ID();
        abstract public int ArmorWaistID();
        abstract public int ArmorWaistDeco1ID();
        abstract public int ArmorWaistDeco2ID();
        abstract public int ArmorWaistDeco3ID();
        abstract public int ArmorLegsID();
        abstract public int ArmorLegsDeco1ID();
        abstract public int ArmorLegsDeco2ID();
        abstract public int ArmorLegsDeco3ID();
        abstract public int Cuff1ID();
        abstract public int Cuff2ID();
        abstract public int TotalDefense();
        abstract public int PouchItem1ID();
        abstract public int PouchItem1Qty();
        abstract public int PouchItem2ID();
        abstract public int PouchItem2Qty();
        abstract public int PouchItem3ID();
        abstract public int PouchItem3Qty();
        abstract public int PouchItem4ID();
        abstract public int PouchItem4Qty();
        abstract public int PouchItem5ID();
        abstract public int PouchItem5Qty();
        abstract public int PouchItem6ID();
        abstract public int PouchItem6Qty();
        abstract public int PouchItem7ID();
        abstract public int PouchItem7Qty();
        abstract public int PouchItem8ID();
        abstract public int PouchItem8Qty();
        abstract public int PouchItem9ID();
        abstract public int PouchItem9Qty();
        abstract public int PouchItem10ID();
        abstract public int PouchItem10Qty();
        abstract public int PouchItem11ID();
        abstract public int PouchItem11Qty();
        abstract public int PouchItem12ID();
        abstract public int PouchItem12Qty();
        abstract public int PouchItem13ID();
        abstract public int PouchItem13Qty();
        abstract public int PouchItem14ID();
        abstract public int PouchItem14Qty();
        abstract public int PouchItem15ID();
        abstract public int PouchItem15Qty();
        abstract public int PouchItem16ID();
        abstract public int PouchItem16Qty();
        abstract public int PouchItem17ID();
        abstract public int PouchItem17Qty();
        abstract public int PouchItem18ID();
        abstract public int PouchItem18Qty();
        abstract public int PouchItem19ID();
        abstract public int PouchItem19Qty();
        abstract public int PouchItem20ID();
        abstract public int PouchItem20Qty();
        abstract public int AmmoPouchItem1ID();
        abstract public int AmmoPouchItem1Qty();
        abstract public int AmmoPouchItem2ID();
        abstract public int AmmoPouchItem2Qty();
        abstract public int AmmoPouchItem3ID();
        abstract public int AmmoPouchItem3Qty();
        abstract public int AmmoPouchItem4ID();
        abstract public int AmmoPouchItem4Qty();
        abstract public int AmmoPouchItem5ID();
        abstract public int AmmoPouchItem5Qty();
        abstract public int AmmoPouchItem6ID();
        abstract public int AmmoPouchItem6Qty();
        abstract public int AmmoPouchItem7ID();
        abstract public int AmmoPouchItem7Qty();
        abstract public int AmmoPouchItem8ID();
        abstract public int AmmoPouchItem8Qty();
        abstract public int AmmoPouchItem9ID();
        abstract public int AmmoPouchItem9Qty();
        abstract public int AmmoPouchItem10ID();
        abstract public int AmmoPouchItem10Qty();

        abstract public int ArmorSkill1();
        abstract public int ArmorSkill2();
        abstract public int ArmorSkill3();
        abstract public int ArmorSkill4();
        abstract public int ArmorSkill5();
        abstract public int ArmorSkill6();
        abstract public int ArmorSkill7();
        abstract public int ArmorSkill8();
        abstract public int ArmorSkill9();
        abstract public int ArmorSkill10();
        abstract public int ArmorSkill11();
        abstract public int ArmorSkill12();
        abstract public int ArmorSkill13();
        abstract public int ArmorSkill14();
        abstract public int ArmorSkill15();
        abstract public int ArmorSkill16();
        abstract public int ArmorSkill17();
        abstract public int ArmorSkill18();
        abstract public int ArmorSkill19();

        abstract public int BloatedWeaponAttack();

        abstract public int ZenithSkill1();
        abstract public int ZenithSkill2();
        abstract public int ZenithSkill3();
        abstract public int ZenithSkill4();
        abstract public int ZenithSkill5();
        abstract public int ZenithSkill6();
        abstract public int ZenithSkill7();

        abstract public int AutomaticSkillWeapon();
        abstract public int AutomaticSkillHead();
        abstract public int AutomaticSkillChest();
        abstract public int AutomaticSkillArms();
        abstract public int AutomaticSkillWaist();
        abstract public int AutomaticSkillLegs();

        abstract public int StyleRank1();
        abstract public int StyleRank2();

        abstract public int GRWeaponLv();

        abstract public int Sigil1Name1();
        abstract public int Sigil1Value1();
        abstract public int Sigil1Name2();
        abstract public int Sigil1Value2();
        abstract public int Sigil1Name3();
        abstract public int Sigil1Value3();
        abstract public int Sigil2Name1();
        abstract public int Sigil2Value1();
        abstract public int Sigil2Name2();
        abstract public int Sigil2Value2();
        abstract public int Sigil2Name3();
        abstract public int Sigil2Value3();
        abstract public int Sigil3Name1();
        abstract public int Sigil3Value1();
        abstract public int Sigil3Name2();
        abstract public int Sigil3Value2();
        abstract public int Sigil3Name3();
        abstract public int Sigil3Value3();


        abstract public int FelyneHunted();
        abstract public int MelynxHunted();
        abstract public int ShakalakaHunted();
        abstract public int VespoidHunted();
        abstract public int HornetaurHunted();
        abstract public int GreatThunderbugHunted();
        abstract public int KelbiHunted();
        abstract public int MosswineHunted();
        abstract public int AntekaHunted();
        abstract public int PopoHunted();
        abstract public int AptonothHunted();
        abstract public int ApcerosHunted();
        abstract public int BurukkuHunted();
        abstract public int ErupeHunted();
        abstract public int VelocipreyHunted();
        abstract public int VelocidromeHunted();
        abstract public int GenpreyHunted();
        abstract public int GendromeHunted();
        abstract public int IopreyHunted();
        abstract public int IodromeHunted();
        abstract public int GiapreyHunted();
        abstract public int YianKutKuHunted();
        abstract public int BlueYianKutKuHunted();
        abstract public int YianGarugaHunted();
        abstract public int GypcerosHunted();
        abstract public int PurpleGypcerosHunted();
        abstract public int HypnocHunted();
        abstract public int BrightHypnocHunted();
        abstract public int SilverHypnocHunted();
        abstract public int FarunokkuHunted();
        abstract public int ForokururuHunted();
        abstract public int ToridclessHunted();
        abstract public int RemobraHunted();
        abstract public int RathianHunted();
        abstract public int PinkRathianHunted();
        abstract public int GoldRathianHunted();
        abstract public int RathalosHunted();
        abstract public int AzureRathalosHunted();
        abstract public int SilverRathalosHunted();
        abstract public int KhezuHunted();
        abstract public int RedKhezuHunted();
        abstract public int BasariosHunted();
        abstract public int GraviosHunted();
        abstract public int BlackGraviosHunted();
        abstract public int MonoblosHunted();
        abstract public int WhiteMonoblosHunted();
        abstract public int DiablosHunted();
        abstract public int BlackDiablosHunted();
        abstract public int TigrexHunted();
        abstract public int EspinasHunted();
        abstract public int OrangeEspinasHunted();
        abstract public int WhiteEspinasHunted();
        abstract public int AkantorHunted();
        abstract public int BerukyurosuHunted();
        abstract public int DoragyurosuHunted();
        abstract public int PariapuriaHunted();
        abstract public int DyuragauaHunted();
        abstract public int GurenzeburuHunted();
        abstract public int OdibatorasuHunted();
        abstract public int HyujikikiHunted();
        abstract public int AnorupatisuHunted();
        abstract public int ZerureusuHunted();
        abstract public int MeraginasuHunted();
        abstract public int DiorexHunted();
        abstract public int PoborubarumuHunted();
        abstract public int VarusaburosuHunted();
        abstract public int GureadomosuHunted();
        abstract public int BariothHunted();
        abstract public int NargacugaHunted();
        abstract public int ZenaserisuHunted();
        abstract public int SeregiosHunted();
        abstract public int BogabadorumuHunted();
        abstract public int CephalosHunted();
        abstract public int CephadromeHunted();
        abstract public int PlesiothHunted();
        abstract public int GreenPlesiothHunted();
        abstract public int VolganosHunted();
        abstract public int RedVolganosHunted();
        abstract public int HermitaurHunted();
        abstract public int DaimyoHermitaurHunted();
        abstract public int CeanataurHunted();
        abstract public int ShogunCeanataurHunted();
        abstract public int ShenGaorenHunted();
        abstract public int AkuraVashimuHunted();
        abstract public int AkuraJebiaHunted();
        abstract public int TaikunZamuzaHunted();
        abstract public int KusubamiHunted();
        abstract public int BullfangoHunted();
        abstract public int BulldromeHunted();
        abstract public int CongaHunted();
        abstract public int CongalalaHunted();
        abstract public int BlangoHunted();
        abstract public int BlangongaHunted();
        abstract public int GogomoaHunted();
        abstract public int RajangHunted();
        abstract public int KamuOrugaronHunted();
        abstract public int NonoOrugaronHunted();
        abstract public int MidogaronHunted();
        abstract public int GougarfHunted();
        abstract public int VoljangHunted();
        abstract public int KirinHunted();
        abstract public int KushalaDaoraHunted();
        abstract public int RustedKushalaDaoraHunted();
        abstract public int ChameleosHunted();
        abstract public int LunastraHunted();
        abstract public int TeostraHunted();
        abstract public int LaoShanLungHunted();
        abstract public int AshenLaoShanLungHunted();
        abstract public int YamaTsukamiHunted();
        abstract public int RukodioraHunted();
        abstract public int RebidioraHunted();
        abstract public int FatalisHunted();
        abstract public int ShantienHunted();
        abstract public int DisufiroaHunted();
        abstract public int GarubaDaoraHunted();
        abstract public int InagamiHunted();
        abstract public int HarudomeruguHunted();
        abstract public int YamaKuraiHunted();
        abstract public int ToaTesukatoraHunted();
        abstract public int GuanzorumuHunted();
        abstract public int KeoaruboruHunted();
        abstract public int ShagaruMagalaHunted();
        abstract public int ElzelionHunted();
        abstract public int AmatsuHunted();
        abstract public int AbioruguHunted();
        abstract public int GiaoruguHunted();
        abstract public int GasurabazuraHunted();
        abstract public int DeviljhoHunted();
        abstract public int BrachydiosHunted();
        abstract public int UragaanHunted();
        abstract public int KuarusepusuHunted();
        abstract public int PokaraHunted();
        abstract public int PokaradonHunted();
        abstract public int BaruragaruHunted();
        abstract public int ZinogreHunted();
        abstract public int StygianZinogreHunted();
        abstract public int GoreMagalaHunted();

        abstract public int BlitzkriegBogabadorumuHunted();
        abstract public int SparklingZerureusuHunted();
        abstract public int StarvingDeviljhoHunted();

        abstract public int CrimsonFatalisHunted();
        abstract public int WhiteFatalisHunted();
        abstract public int CactusHunted();
        abstract public int ArrogantDuremudiraHunted();//untested
        //abstract public int KingShakalakaHunted() => 1;
        abstract public int MiRuHunted();
        abstract public int UnknownHunted();
        abstract public int GoruganosuHunted();
        abstract public int AruganosuHunted();
        abstract public int PSO2RappyHunted();
        abstract public int RocksHunted();
        abstract public int UrukiHunted();
        abstract public int GorgeObjectsHunted();
        abstract public int BlinkingNargacugaHunted();
        abstract public int KingShakalakaHunted();
        abstract public int QuestState();


        abstract public int RoadDureSkill1Name();
        abstract public int RoadDureSkill1Level();
        abstract public int RoadDureSkill2Name();
        abstract public int RoadDureSkill2Level();
        abstract public int RoadDureSkill3Name();
        abstract public int RoadDureSkill3Level();
        abstract public int RoadDureSkill4Name();
        abstract public int RoadDureSkill4Level();
        abstract public int RoadDureSkill5Name();
        abstract public int RoadDureSkill5Level();
        abstract public int RoadDureSkill6Name();
        abstract public int RoadDureSkill6Level();
        abstract public int RoadDureSkill7Name();
        abstract public int RoadDureSkill7Level();
        abstract public int RoadDureSkill8Name();
        abstract public int RoadDureSkill8Level();
        abstract public int RoadDureSkill9Name();
        abstract public int RoadDureSkill9Level();
        abstract public int RoadDureSkill10Name();
        abstract public int RoadDureSkill10Level();
        abstract public int RoadDureSkill11Name();
        abstract public int RoadDureSkill11Level();
        abstract public int RoadDureSkill12Name();
        abstract public int RoadDureSkill12Level();
        abstract public int RoadDureSkill13Name();
        abstract public int RoadDureSkill13Level();
        abstract public int RoadDureSkill14Name();
        abstract public int RoadDureSkill14Level();
        abstract public int RoadDureSkill15Name();
        abstract public int RoadDureSkill15Level();
        abstract public int RoadDureSkill16Name();
        abstract public int RoadDureSkill16Level();

        abstract public int PartySize();
        abstract public int PartySizeMax();


        #endregion

        public bool HasMonster1 => CaravanOverride() ? ShowHPBar(CaravanMonster1ID(), Monster1HPInt()) : ShowHPBar(LargeMonster1ID(), Monster1HPInt());
        public bool HasMonster2 => CaravanOverride() ? ((CaravanMonster2ID() > 0 && Monster2HPInt() != 0 && GetNotRoad()) || Configuring) : ((LargeMonster2ID() > 0 && Monster2HPInt() != 0 && GetNotRoad()) || Configuring); // road check since the 2nd choice is used as the monster #1
        public bool HasMonster3 => ShowHPBar(LargeMonster3ID(), Monster3HPInt());
        public bool HasMonster4 => ShowHPBar(LargeMonster4ID(), Monster4HPInt());

        public int HitCount => HitCountInt();

        public bool _configuring = false;

        /// <summary>
        /// Shows the monster ehp.
        /// </summary>
        /// <returns></returns>
        public bool ShowMonsterEHP()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableEHPNumbers == true)
                return true;
            else
                return false;
        }

        public bool ShowCaravanScore()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableCaravanScore == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AddressModel"/> is configuring.
        /// </summary>
        /// <value>
        ///   <c>true</c> if configuring; otherwise, <c>false</c>.
        /// </value>
        public bool Configuring { get { return _configuring; } set { _configuring = value; ReloadData(); } }

        public bool IsAlwaysShowingMonsterInfo()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.AlwaysShowMonsterInfo == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Shows the hp bar?
        /// </summary>
        /// <param name="monsterId">The monster identifier.</param>
        /// <param name="monsterHp">The monster hp.</param>
        /// <returns></returns>
        public bool ShowHPBar(int monsterId, int monsterHp)
        {
            return (monsterId > 0 && monsterHp != 0) || Configuring || IsAlwaysShowingMonsterInfo();
        }

        //
        public bool? roadOverride()
        {
            //should work
            if (QuestID() != 23527 && QuestID() != 23628)
                return true;
            else if (QuestID() == 23527 || QuestID() == 23628)
                return false;
            return null;
        }

        public bool CaravanOverride()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableCaravanOverride == true)
                return true;
            else
                return false;
        }

        //
        public bool GetNotRoad()
        {
            bool? b = roadOverride();
            if (b != null)
                return b.Value;
            return IsNotRoad();
        }

        #region monster parts

        public string Monster1Part1Name = "None";
        public string Monster1Part2Name = "None";
        public string Monster1Part3Name = "None";
        public string Monster1Part4Name = "None";
        public string Monster1Part5Name = "None";
        public string Monster1Part6Name = "None";
        public string Monster1Part7Name = "None";
        public string Monster1Part8Name = "None";
        public string Monster1Part9Name = "None";
        public string Monster1Part10Name = "None";
        public string Monster2Part1Name = "None";
        public string Monster2Part2Name = "None";
        public string Monster2Part3Name = "None";
        public string Monster2Part4Name = "None";
        public string Monster2Part5Name = "None";
        public string Monster2Part6Name = "None";
        public string Monster2Part7Name = "None";
        public string Monster2Part8Name = "None";
        public string Monster2Part9Name = "None";
        public string Monster2Part10Name = "None";

        //assumption: it follows ferias' monster part order top to bottom, presumably (e.g. head is at the top, so part 0 is head, and so on)
        // grouping by skeleton too

        ///<summary>
        ///Monster parts labels
        ///<para>int number: The part number from 1 to 10</para>
        ///<para>int monsterID: the monsterID</para>
        ///</summary>
        public string GetPartName(int number,int monsterID)
        {
            //keep in mind this has the null
            if (roadOverride() == false)
                monsterID = RoadSelectedMonster() == 0 ? LargeMonster1ID() : LargeMonster2ID();
            else if (CaravanOverride())
                monsterID = CaravanMonster1ID();
            
            if (getDureName() != "None")
            {
                //switch(getDureName)
                //{
                //    case "1st District Duremudira":
                //    case "2nd District Duremudira":
                //    case "3rd District Duremudira":
                //    case "4th District Duremudira":
                //    case "Arrogant Duremudira":
                //        return 
                //}
                monsterID = 132;
            }

            if (getRaviName() != "None")
            {
                switch (getRaviName())
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
                }
            }


            switch (monsterID)
            {
                case 0: //None
                case 10: //Veggie Elder
                case 18://Felyne?
                case 23://Melynx
                case 29: //Rocks
                case 32: //Pugis
                case 57://Shakalaka
                case 63://Remobra
                case 86: //Cactus
                case 87: //Gorge Objects
                case 88: //Gorge Rocks
                case 118: //Dummy
                case 124://Uruki
                case 133: //UNK
                case 134://Felyne
                case 135: //Blue NPC
                case 136: //UNK
                case 137: //Cactus
                case 138: //Veggie Elders
                case 156: //UNK
                case 157: //Egyurasu
                case 168: //Rocks
                case 171: //Unknown Blue Barrel
                case 173://Costumed Uruki
                case 175://PSO2 Rappy
                case 176://King Shakalaka
                case 19://Vespoid
                case 24://Hornetaur
                case 56: //Great Thunderbug
                    Monster1Part1Name = "None";
                    Monster1Part2Name = "None";
                    Monster1Part3Name = "None";
                    Monster1Part4Name = "None";
                    Monster1Part5Name = "None";
                    Monster1Part6Name = "None";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                // Rath skeleton
                case 1: //Rathian
                case 11: //Rathalos
                case 14: //Diablos
                case 15://Khezu
                case 26: //Monoblos 
                case 37: //Pink Rathian
                case 41: //Silver Rathalos
                case 42: //Gold Rathian
                case 43: //Black Diablos
                case 44: //White Monoblos
                case 45://Red Khezu
                case 49: //Azure Rathalos
                case 80: //Espinas
                case 81: //Orange Espinas
                case 90://White Espinas
                case 96://Gurenzeburu
                case 100://UNKNOWN
                case 109: //Anorupatisu
                case 122: //Zerureusu
                case 126: //Meraginasu
                case 130: //Varusaburosu
                case 139://Gureadomosu
                case 174: //Sparkling Zerureusu
                    Monster1Part1Name = "Body";
                    Monster1Part2Name = "L. Wing";
                    Monster1Part3Name = "R. Wing";
                    Monster1Part4Name = "L. Leg";
                    Monster1Part5Name = "R. Leg";
                    Monster1Part6Name = "Neck";
                    Monster1Part7Name = "Head";
                    Monster1Part8Name = "Tail Tip";
                    Monster1Part9Name = "Tail";
                    Monster1Part10Name = "None";
                    break;

                // Flying Wyvern Skeleton 2
                case 17: //Gravios
                case 22: //Basarios
                case 47://Black Gravios
                    Monster1Part1Name = "Body";
                    Monster1Part2Name = "L. Wing";
                    Monster1Part3Name = "R. Wing";
                    Monster1Part4Name = "L. Leg";
                    Monster1Part5Name = "R. Leg";
                    Monster1Part6Name = "Head";
                    Monster1Part7Name = "Belly";
                    Monster1Part8Name = "Tail Tip";
                    Monster1Part9Name = "Tail";
                    Monster1Part10Name = "None";
                    break;

                //Rex Wyvern Skeleton 1
                case 76://Tigrex
                case 77://Akantor
                case 110://Hyujikiki
                case 127://Diorekkusu
                case 151://Barioth
                case 159://Nargacuga
                case 163://Blinking Nargacuga
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Back";
                    Monster1Part4Name = "Tail";
                    Monster1Part5Name = "L. Wing";
                    Monster1Part6Name = "L. Leg";
                    Monster1Part7Name = "R. Wing";
                    Monster1Part8Name = "R. Leg"; //swap to back?
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Rex Wyvern Skeleton 2
                case 89://Pariapuria
                case 169://Seregios
                case 141: //Toridcless
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "L. Wing";
                    Monster1Part4Name = "R. Wing";
                    Monster1Part5Name = "L. Leg";
                    Monster1Part6Name = "R. Leg";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "Back"; 
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Rex Wyvern Skeleton 3
                case 113://Mi Ru
                case 94://Dyuragaua
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "L. Wing";
                    Monster1Part3Name = "R. Wing";
                    Monster1Part4Name = "Hindlegs";
                    Monster1Part5Name = "Body";
                    Monster1Part6Name = "Belly";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "Tail Tip"; //swap to back?
                    Monster1Part9Name = "Back";
                    Monster1Part10Name = "None";
                    break;


                //Big Rex Wyvern Skeleton 1
                case 131://Poborubarumu
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Hindlegs";
                    Monster1Part4Name = "Tail Tip";
                    Monster1Part5Name = "R. Foreleg";
                    Monster1Part6Name = "L. Foreleg";
                    Monster1Part7Name = "R.Forewing";
                    Monster1Part8Name = "L.Forewing"; 
                    Monster1Part9Name = "Back";//unsure
                    Monster1Part10Name = "None";
                    break;

                case 106://Odibatorasu
                    Monster1Part1Name = "L. Foreleg";
                    Monster1Part2Name = "R. Foreleg";
                    Monster1Part3Name = "L. Hindleg";
                    Monster1Part4Name = "R. Hindleg";
                    Monster1Part5Name = "Back";
                    Monster1Part6Name = "Belly";
                    Monster1Part7Name = "Body";
                    Monster1Part8Name = "Head";
                    Monster1Part9Name = "Back";
                    Monster1Part10Name = "None";
                    break;

                case 160://Keoaruboru (sometimes doesn't load properly)
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Back";
                    Monster1Part4Name = "L. Hindleg";
                    Monster1Part5Name = "R. Hindleg";
                    Monster1Part6Name = "L. Foreleg";
                    Monster1Part7Name = "R. Foreleg";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Elder Skeleton (Teostra, Kushala, Chameleos)
                case 54: //Kushala Daora
                case 59: //Chameleos
                case 60://Rusted Kushala Daora
                case 64: //Lunastra
                case 65://Teostra
                case 99://Rukodiora
                case 107://Disufiroa
                case 108://Rebidiora
                case 128://Garuba Daora
                case 140://Harudomerugu
                case 150://Toa Tesukatora
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Belly"; //or back?
                    Monster1Part4Name = "Forelegs";
                    Monster1Part5Name = "Hindlegs";
                    Monster1Part6Name = "Wings";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "Back";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                case 154://Guanzorumu
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Wings";
                    Monster1Part3Name = "L. Foreleg"; 
                    Monster1Part4Name = "R. Foreleg";
                    Monster1Part5Name = "L. Hindleg";
                    Monster1Part6Name = "R. Hindleg";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "Body";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Duremudira (doesn't load properly)
                case 132://Duremudira
                case 145://3rd Phase Duremudira (used on gathering quest)
                case 167://Arrogant Duremudira
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Wings";
                    Monster1Part3Name = "Tail";
                    Monster1Part4Name = "Hindlegs";
                    Monster1Part5Name = "Belly";
                    Monster1Part6Name = "Forelegs";
                    Monster1Part7Name = "Body";
                    Monster1Part8Name = "Back";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Fatalis
                case 2: //Fatalis
                case 36: //Crimson Fatalis
                case 71: //White Fatalis
                    Monster1Part1Name = "Chest";
                    Monster1Part2Name = "Belly";
                    Monster1Part3Name = "L. Leg";
                    Monster1Part4Name = "R. Leg";
                    Monster1Part5Name = "Neck";
                    Monster1Part6Name = "Face";
                    Monster1Part7Name = "Tail/Back";
                    Monster1Part8Name = "Wings";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;


                //Magala
                case 162: //Gore Magala
                case 164: //Shagaru Magala
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Horn";
                    Monster1Part3Name = "Body";
                    Monster1Part4Name = "Wings";
                    Monster1Part5Name = "Forelegs";
                    Monster1Part6Name = "L. Hindleg";
                    Monster1Part7Name = "R. Hindleg";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Hervibore Skeleton
                case 3: //Kelbi
                case 4://Mosswine
                case 5://Bullfango
                case 12://Aptonoth
                case 25://Apceros
                case 68://Bulldrome
                case 69://Anteka
                case 70://Popo
                case 97://Burukku
                case 98://Erupe
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "None";
                    Monster1Part3Name = "None";
                    Monster1Part4Name = "None";
                    Monster1Part5Name = "None";
                    Monster1Part6Name = "None";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                case 33://Kirin
                    Monster1Part1Name = "Body";
                    Monster1Part2Name = "None";
                    Monster1Part3Name = "None";
                    Monster1Part4Name = "None";
                    Monster1Part5Name = "None";
                    Monster1Part6Name = "None";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                //Bird Wyvern Skeleton
                case 6://Yian Kut-Ku
                case 20://Gypceros
                case 38://Blue Yian Kut-Ku
                case 39: //Purple Gypceros
                case 40: //Yian Garuga
                case 74://Hypnocatrice
                case 78://Bright Hypnoc
                case 82://Silver Hypnoc
                case 114://Farunokku
                    Monster1Part1Name = "Body";
                    Monster1Part2Name = "L. Wing";
                    Monster1Part3Name = "R. Wing";
                    Monster1Part4Name = "L. Leg";
                    Monster1Part5Name = "R. Leg";
                    Monster1Part6Name = "Neck";
                    Monster1Part7Name = "Head";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Bird Wyvern Skeleton 2
                case 13://Genprey
                case 16://Velociprey
                case 27://Velocidrome
                case 28://Gendrome
                case 30://Ioprey
                case 31://Iodrome
                case 35://Giaprey / Giadrome
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "None";
                    Monster1Part3Name = "None";
                    Monster1Part4Name = "None";
                    Monster1Part5Name = "None";
                    Monster1Part6Name = "None";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                //Fanged Beast Monkey Skeleton
                case 51://Blangonga
                case 53://Rajang
                case 61://Blango
                case 62://Conga
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "R. Side";
                    Monster1Part3Name = "L. Side";
                    Monster1Part4Name = "Body";
                    Monster1Part5Name = "Tail";
                    Monster1Part6Name = "None";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "Chest";
                    Monster1Part10Name = "None";
                    break;

                case 101://Gogomoa
                case 102://Kokomoa
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "L. Foreleg";
                    Monster1Part4Name = "R. Foreleg";
                    Monster1Part5Name = "L. Hindleg";
                    Monster1Part6Name = "R. Hindleg";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                case 123://Gougarf (Lolo)
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "R. Hindleg";
                    Monster1Part3Name = "L. Hindleg";
                    Monster1Part4Name = "Body";
                    Monster1Part5Name = "Tail";
                    Monster1Part6Name = "L. Foreleg";
                    Monster1Part7Name = "R. Foreleg";
                    Monster1Part8Name = "Back";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                case 52://Congalala
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Hindlegs";
                    Monster1Part4Name = "Forelegs";
                    Monster1Part5Name = "Tail";
                    Monster1Part6Name = "None";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                case 158://Voljang
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "R. Foreleg";
                    Monster1Part3Name = "L. Foreleg";
                    Monster1Part4Name = "Body";
                    Monster1Part5Name = "R. Hindleg";
                    Monster1Part6Name = "L. Hindleg";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "Back";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Brute Wyvern Skeleton
                case 104://Abiorugu
                case 112://Giaorugu
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "R. Leg";
                    Monster1Part3Name = "L. Leg";
                    Monster1Part4Name = "Arms";
                    Monster1Part5Name = "?"; //TODO
                    Monster1Part6Name = "Torso";
                    Monster1Part7Name = "?"; //TODO
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                case 147://Deviljho
                case 155://Starving Deviljho
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Chest";
                    Monster1Part3Name = "Neck";
                    Monster1Part4Name = "Arms";
                    Monster1Part5Name = "R. Leg";
                    Monster1Part6Name = "L. Leg";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                case 142://Gasurabazura
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Belly"; //swap to back?
                    Monster1Part3Name = "L. Arm";
                    Monster1Part4Name = "R. Arm";
                    Monster1Part5Name = "L. Leg";
                    Monster1Part6Name = "R. Leg";
                    Monster1Part7Name = "Back";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                case 148://Brachydios
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Belly";
                    Monster1Part3Name = "L. Arm";
                    Monster1Part4Name = "R. Arm";
                    Monster1Part5Name = "L. Leg";
                    Monster1Part6Name = "R. Leg";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "Back";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                case 152://Uragaan
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Belly";
                    Monster1Part4Name = "Arms";
                    Monster1Part5Name = "R. Leg";
                    Monster1Part6Name = "L. Leg";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "Back";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Piscine Wyvern Skeleton
                case 8://Cephadrome
                case 21://Plesioth
                case 34://Cephalos
                case 46://Green Plesioth
                case 75://Lavasioth
                case 79://Red Lavasioth
                case 119://Goruganosu
                case 120://Aruganosu
                    Monster1Part1Name = "Body";
                    Monster1Part2Name = "L. Fin";
                    Monster1Part3Name = "R. Fin";
                    Monster1Part4Name = "L. Leg";
                    Monster1Part5Name = "R. Leg";
                    Monster1Part6Name = "Neck";
                    Monster1Part7Name = "Head";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                //Leviathan Skeleton
                case 105://Kuarusepusu
                case 115://Pokaradon
                case 117://Poka
                case 121://Baruragaru
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "R. Foreleg";
                    Monster1Part4Name = "L. Foreleg";
                    Monster1Part5Name = "Tail";
                    Monster1Part6Name = "R. Hindleg";
                    Monster1Part7Name = "L. Hindleg";
                    Monster1Part8Name = "Back";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                case 165://Amatsu
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Neck";
                    Monster1Part4Name = "L. Wing";
                    Monster1Part5Name = "R. Wing";
                    Monster1Part6Name = "Hindlegs";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "Back";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                case 116://Shantien
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "L. Foreleg";
                    Monster1Part4Name = "R. Foreleg";
                    Monster1Part5Name = "Hindlegs";
                    Monster1Part6Name = "Tail";
                    Monster1Part7Name = "?";
                    Monster1Part8Name = "?";
                    Monster1Part9Name = "?";
                    Monster1Part10Name = "None";
                    break;

                //Fanged Wyvern Skeleton
                case 146://Zinogre
                case 153://Stygian Zinogre
                case 166://Elzelion
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Back";
                    Monster1Part4Name = "L. Foreleg";
                    Monster1Part5Name = "R. Foreleg";
                    Monster1Part6Name = "L. Hindleg";
                    Monster1Part7Name = "R. Hindleg";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                case 129://Inagami
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "R. Foreleg";
                    Monster1Part4Name = "L. Foreleg";
                    Monster1Part5Name = "R. Hindleg";
                    Monster1Part6Name = "L. Hindleg";
                    Monster1Part7Name = "Tail";
                    Monster1Part8Name = "?";
                    Monster1Part9Name = "?";
                    Monster1Part10Name = "None";
                    break;

                //Fanged Beast Wolf Skeleton
                case 91://Kamu Orugaron
                case 92://Nono Orugaron
                case 111://Midogaron
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Back";
                    Monster1Part3Name = "Body";
                    Monster1Part4Name = "Forelegs";
                    Monster1Part5Name = "Hindlegs";
                    Monster1Part6Name = "Tail";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                //Carapaceon Skeleton
                case 48://Daimyo Hermitaur
                case 66://Hermitaur
                case 67://Shogun Ceanataur
                case 73://Ceanataur
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Skull";
                    Monster1Part4Name = "L. Legs";
                    Monster1Part5Name = "R. Legs";
                    Monster1Part6Name = "L. Claw";
                    Monster1Part7Name = "R. Claw";
                    Monster1Part8Name = "Arms";
                    Monster1Part9Name = "Feeler";
                    Monster1Part10Name = "None";
                    break;

                //Scorpion Skeleton
                case 83://Akura Vashimu
                case 84://Akura Jebia
                case 143://Kusubami
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "R. Claw";
                    Monster1Part3Name = "L. Claw";
                    Monster1Part4Name = "Legs";
                    Monster1Part5Name = "Body";
                    Monster1Part6Name = "Tail";
                    Monster1Part7Name = "Tail Cut";
                    Monster1Part8Name = "?";
                    Monster1Part9Name = "?";
                    Monster1Part10Name = "None";
                    break;

                //Flying Wyvern Skeleton 2
                case 85://Berukyurosu (TODO doesn't load properly)
                case 95://Doragyurosu
                case 161://Zenaserisu
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "L. Wing";
                    Monster1Part4Name = "R. Wing";
                    Monster1Part5Name = "Legs";
                    Monster1Part6Name = "R. Wing End";
                    Monster1Part7Name = "L. Wind End";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "Aux Tails";
                    Monster1Part10Name = "None";
                    break;

                case 125://Forokururu
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "L. Wing";
                    Monster1Part4Name = "R. Wing";
                    Monster1Part5Name = "L. Leg";
                    Monster1Part6Name = "R. Leg";
                    Monster1Part7Name = "Belly";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "Throat";
                    Monster1Part10Name = "None";
                    break;

                //Elder Skeleton 2
                case 7://Lao-Shan Lung
                case 50://Ashen Lao-Shan Lung
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "None";
                    Monster1Part3Name = "Forelegs";
                    Monster1Part4Name = "Tail/Leg";
                    Monster1Part5Name = "?";
                    Monster1Part6Name = "Back";
                    Monster1Part7Name = "Chest";
                    Monster1Part8Name = "Body";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                //Elder Skeleton 3 (TODO doesn't load properly)
                case 58://Yama Tsukami
                case 72://Yama Tsukami
                case 144://Yama Kurai
                    Monster1Part1Name = "None";
                    Monster1Part2Name = "None";
                    Monster1Part3Name = "None";
                    Monster1Part4Name = "None";
                    Monster1Part5Name = "None";
                    Monster1Part6Name = "None";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                //Shen Gaoren
                case 55:
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Skull";
                    Monster1Part4Name = "L. Foreleg";
                    Monster1Part5Name = "L. Hindleg";
                    Monster1Part6Name = "R. Foreleg";
                    Monster1Part7Name = "R. Hindleg";
                    Monster1Part8Name = "Claws";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                //Taikun Zamuza
                case 103:
                    Monster1Part1Name = "Body";
                    Monster1Part2Name = "L. Claw";
                    Monster1Part3Name = "R. Claw";
                    Monster1Part4Name = "L. Legs";
                    Monster1Part5Name = "None";
                    Monster1Part6Name = "R. Legs";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                //Raviente
                case 93:
                    Monster1Part1Name = "?";
                    Monster1Part2Name = "?";
                    Monster1Part3Name = "Body";
                    Monster1Part4Name = "?";
                    Monster1Part5Name = "Shell";
                    Monster1Part6Name = "Tail";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "None";
                    Monster1Part9Name = "None";
                    Monster1Part10Name = "None";
                    break;

                //Berserk Raviente (doesn't load properly)
                case 149:
                    Monster1Part1Name = "Horn";
                    Monster1Part2Name = "Head";
                    Monster1Part3Name = "Neck";
                    Monster1Part4Name = "Shell";
                    Monster1Part5Name = "Body";
                    Monster1Part6Name = "Tail";
                    Monster1Part7Name = "None";
                    Monster1Part8Name = "Crystal";
                    Monster1Part9Name = "Crystal";
                    Monster1Part10Name = "None";
                    break;

                //Boggers
                case 170:
                case 172: //Blitzkrieg
                    Monster1Part1Name = "Head";
                    Monster1Part2Name = "Body";
                    Monster1Part3Name = "Belly";
                    Monster1Part4Name = "L. Arm";
                    Monster1Part5Name = "R. Arm";
                    Monster1Part6Name = "L. Leg";
                    Monster1Part7Name = "R. Leg";
                    Monster1Part8Name = "Tail";
                    Monster1Part9Name = "Tail Tip";
                    Monster1Part10Name = "None";
                    break;

                case > 176:
                    Monster1Part1Name = "Error";
                    Monster1Part2Name = "Error";
                    Monster1Part3Name = "Error";
                    Monster1Part4Name = "Error";
                    Monster1Part5Name = "Error";
                    Monster1Part6Name = "Error";
                    Monster1Part7Name = "Error";
                    Monster1Part8Name = "Error";
                    Monster1Part9Name = "Error";
                    Monster1Part10Name = "Error";
                    break;
            }

            string partName = number switch
            {
                1 => Monster1Part1Name,
                2 => Monster1Part2Name,
                3 => Monster1Part3Name,
                4 => Monster1Part4Name,
                5 => Monster1Part5Name,
                6 => Monster1Part6Name,
                7 => Monster1Part7Name,
                8 => Monster1Part8Name,
                9 => Monster1Part9Name,
                10 => Monster1Part10Name,
                _ => "None",
            };
            return string.Format("{0}: ",partName);
        }

        public string Monster1Part1Number
        {
            get
            {
                int currentPartHP = Monster1Part1();
                //if (currentPartHP > 0)
                //{
                    return GetPartName(1, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster1Part2Number
        {
            get
            {
                int currentPartHP = Monster1Part2();
                //if (currentPartHP > 0)
                //{
                return GetPartName(2, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster1Part3Number
        {
            get
            {
                int currentPartHP = Monster1Part3();
                //if (currentPartHP > 0)
                //{
                return GetPartName(3, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster1Part4Number
        {
            get
            {
                int currentPartHP = Monster1Part4();
                //if (currentPartHP > 0)
                //{
                return GetPartName(4, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster1Part5Number
        {
            get
            {
                int currentPartHP = Monster1Part5();
                //if (currentPartHP > 0)
                //{
                return GetPartName(5, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster1Part6Number
        {
            get
            {
                int currentPartHP = Monster1Part6();
                //if (currentPartHP > 0)
                //{
                return GetPartName(6, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster1Part7Number
        {
            get
            {
                int currentPartHP = Monster1Part7();
                //if (currentPartHP > 0)
                //{
                return GetPartName(7, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster1Part8Number
        {
            get
            {
                int currentPartHP = Monster1Part8();
                //if (currentPartHP > 0)
                //{
                return GetPartName(8, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster1Part9Number
        {
            get
            {
                int currentPartHP = Monster1Part9();
                //if (currentPartHP > 0)
                //{
                return GetPartName(9, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster1Part10Number
        {
            get
            {
                int currentPartHP = Monster1Part10();
                //if (currentPartHP > 0)
                //{
                return GetPartName(10, LargeMonster1ID()) + currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part1Number
        {
            get
            {
                int currentPartHP = Monster2Part1();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part2Number
        {
            get
            {
                int currentPartHP = Monster2Part2();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part3Number
        {
            get
            {
                int currentPartHP = Monster2Part3();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part4Number
        {
            get
            {
                int currentPartHP = Monster2Part4();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part5Number
        {
            get
            {
                int currentPartHP = Monster2Part5();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part6Number
        {
            get
            {
                int currentPartHP = Monster2Part6();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part7Number
        {
            get
            {
                int currentPartHP = Monster2Part7();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part8Number
        {
            get
            {
                int currentPartHP = Monster2Part8();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part9Number
        {
            get
            {
                int currentPartHP = Monster2Part9();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        public string Monster2Part10Number
        {
            get
            {
                int currentPartHP = Monster2Part10();
                //if (currentPartHP > 0)
                //{
                return currentPartHP.ToString();
                //}
                //return "0";
            }
        }

        #endregion

        /// <summary>
        /// Shows the sharpness percentage.
        /// </summary>
        /// <returns></returns>
        public bool ShowSharpnessPercentage()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableSharpnessPercentage == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Shows the time left percentage.
        /// </summary>
        /// <returns></returns>
        public bool ShowTimeLeftPercentage()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableTimeLeftPercentage == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the timer mode.
        /// </summary>
        /// <returns></returns>
        public string GetTimerMode()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.TimerMode == "Time Left")
                return "Time Left";
            else if (s.TimerMode == "Time Elapsed")
                return "Time Elapsed";
            else return "Time Left";
        }

        public string GetRoadTimerResetMode()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.DiscordRoadTimerReset == "Never")
                return "Never";
            else if (s.DiscordRoadTimerReset == "Always")
                return "Always";
            else return "Never";
        }

        public int MaxSharpness = 0;

        public string TimeLeftPercent = "";

        public string TimeLeftPercentNumber
        {
            get
            {
                if (TimeDefInt() < TimeInt())
                {
                    return "0";
                }
                else
                {
                    return string.Format(" ({0:0}%)", (float)TimeInt() / TimeDefInt() * 100.0);
                }
            }
        }

        public string SharpnessPercentNumber
        {
            get
            {
                if (ShowSharpnessPercentage())
                {
                    if (MaxSharpness < Sharpness())
                    {
                        MaxSharpness = Sharpness();
                        return string.Format(" ({0:0}%)", (float)Sharpness() / MaxSharpness * 100.0);
                    }
                    else if (Sharpness() <= 0)
                    {
                        return " (0%)";
                    } else // MaxSharpness > CurrentSharpness && CurrentSharpness > 0
                    {
                        return string.Format(" ({0:0}%)", (float)Sharpness() / MaxSharpness * 100.0);
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        ///<summary>
        ///MM:SS.cs
        ///</summary>
        public string Time
        {
            get
            {
                int time;

                if (GetTimerMode() == "Time Elapsed")
                {
                    time = TimeDefInt() - TimeInt();
                }
                else if (GetTimerMode() == "Time Left")
                {
                    time = TimeInt();
                } else // default to Time Left mode
                {
                    time = TimeInt();
                }

                double seconds = time / 30;
                double minutes = seconds / 60;
                double centiseconds = seconds / 100;

                if (time > 0)
                {

                    if (ShowTimeLeftPercentage())
                    {
                        TimeLeftPercent = TimeLeftPercentNumber;
                    }
                    else
                    {
                        TimeLeftPercent = "";
                    }

                    if ((time / 30) / 60 < 10) 
                    {
                        if ((time / 30) % 60 < 10)
                        {
                            return string.Format("{0:00}:{1:00}.{2}",(time / 30) / 60, time / 30 % 60, (int)Math.Round((float)((time % 30 * 100)) / 3)) + TimeLeftPercent;
                        } else
                        {
                            return string.Format("{0:00}:{1}.{2}", (time / 30) / 60, (time / 30) % 60, (int)Math.Round((float)(((time % 30) * 100)) / 3)) + TimeLeftPercent;
                        }
                    } else
                    {
                        if ((time / 30) % 60 < 10) 
                        {
                            return string.Format("{0}:{1:00}.{2}", (time / 30) / 60, (time / 30) % 60, (int)Math.Round((float)(((time % 30) * 100)) / 3)) + TimeLeftPercent;
                        }
                        else
                        {
                            return string.Format("{0}:{1}.{2}", (time / 30) / 60, (time / 30) % 60, (int)Math.Round((float)(((time % 30) * 100)) / 3)) + TimeLeftPercent;
                        }
                    }
                } else
                {
                    return string.Format("{0:00}:{1:00}.{2}", (time / 30) / 60, (time / 30) % 60, (int)Math.Round((float)(((time % 30) * 100)) / 3)) + TimeLeftPercent;
                }
        //        if frame > 0 {
        //            if err == nil
        //            {
        //                  sendServerChatMessage(s, fmt.Sprintf("Quest Name : %s.", name))

        //                  sendServerChatMessage(s, fmt.Sprintf("Target Monster : %s", monster))
        //                  if frame / 30 / 60 < 10
        //                  {
        //                      if frame / 30 % 60 < 10
        //                      {
        //                          sendServerChatMessage(s, fmt.Sprintf("Quest Time : 0%d:0%d.%03d (%d frames)\n", frame / 30 / 60, frame / 30 % 60, int(math.Round(float64(frame % 30 * 100) / 3)), frame))
        //                      }
        //                      else
        //                      {
        //                          sendServerChatMessage(s, fmt.Sprintf("Quest Time : 0%d:%d.%03d (%d frames)\n", frame / 30 / 60, frame / 30 % 60, int(math.Round(float64(frame % 30 * 100) / 3)), frame))
        //                      }
        //                  }
        //                  else
        //                  {
        //                      if frame / 30 % 60 < 10
        //                      {
        //                          sendServerChatMessage(s, fmt.Sprintf("Quest Time : %d:0%d.%03d (%d frames)\n", frame / 30 / 60, frame / 30 % 60, int(math.Round(float64(frame % 30 * 100) / 3)), frame))
        //                      }
        //                      else
        //                      {
        //                          sendServerChatMessage(s, fmt.Sprintf("Quest Time : %d:%d.%03d (%d frames)\n", frame / 30 / 60, frame / 30 % 60, int(math.Round(float64(frame % 30 * 100) / 3)), frame))
        //                      }
        //                  }
        //              }
                    //return string.Format("{0}:{}.{}0:00.##}", minutes, seconds % 60);
               }
            }

        //per quest
        public int HighestAtk = 0;

        /// <summary>
        /// Shows the color of the highest atk.
        /// </summary>
        /// <returns></returns>
        public bool ShowHighestAtkColor()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableHighestAtkColor == true)
                return true;
            else
                return false;
        }

        public string isHighestAtk
        {
            get
            {
                if (WeaponRaw() == HighestAtk && ShowHighestAtkColor())
                    return "#f38ba8";
                else
                    return "#f5e0dc";
            }
        }

        ///<summary>
        ///<para>Player true raw</para>
        ///<br>Attack addition is added as either Attack A or Attack B and is before or after Hunting Horn buffs respectively.</br>
        ///<br>Values that are known to reside in Attack B are Rush, Stylish Assault, Flash Conversion, Obscurity, Incitement, Rush, Vigorous Up and Partnyaa Attack Buffs.</br>
        ///<para>Final True  = ((Weapon True + Attack A) * HH Buff + Attack B) * Multipliers + Additional</para>
        ///</summary>
        public string ATK
        {
            get
            {
                int weaponRaw = WeaponRaw();
                int weaponType = WeaponType();

                if (QuestID() == 0) //should work fine
                {
                    HighestAtk = 0;
                }

                if (weaponRaw > HighestAtk)
                {
                    HighestAtk = weaponRaw;
                }

                return weaponRaw.ToString();// ((int)(GetMultFromWeaponType(weaponType) * weaponRaw)).ToString();
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
                //see palettes.md
                int currentSharpnessLevel = SharpnessLevel();
                return currentSharpnessLevel switch
                {
                    0 => "#c50f3a",//Red
                    1 => "#e85218",//Orange
                    2 => "#f3c832",//Yellow
                    3 => "#5ed300",//Green
                    4 => "#3068ee",//Blue
                    5 => "#f0f0f0",//White
                    6 => "#de7aff",//Purple
                    7 => "#86f4f4",//Cyan
                    _ => "#ffffff",//
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
                int currentSharpness = Sharpness();
                if (currentSharpness > 0)
                {
                    return currentSharpness.ToString()+SharpnessPercentNumber;
                }
                return "0"+SharpnessPercentNumber;
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
                int weaponRaw = WeaponRaw();
                int weaponType = WeaponType();
                //return ((int)(GetMultFromWeaponType(weaponType) * weaponRaw)).ToString();
                return GetMultFromWeaponType(weaponType);
            }
        }

        /// <summary>
        /// Gets the name of the current weapon.
        /// </summary>
        /// <value>
        /// The name of the current weapon.
        /// </value>
        public string CurrentWeaponName
        { 
            get
            {
                int weaponType = WeaponType();
                return GetWeaponNameFromType(weaponType);
                //return WeaponType().ToString();
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public string Size
        {
            get
            {
                return SelectedMonster switch
                {
                    0 => Monster1Size(),
                    1 => Monster2Size(),
                    _ => Monster1Size(),
                };
            }
        }

        /// <summary>
        /// Gets the atk mult.
        /// </summary>
        /// <value>
        /// The atk mult.
        /// </value>
        public string AtkMult
        {
            get
            {
                return SelectedMonster switch
                {
                    0 => Monster1AtkMult(),
                    1 => Monster2AtkMult(),
                    _ => Monster1AtkMult(),
                };
            }
        }

        public bool isMonsterFocused = false;

        /// <summary>
        /// Displays the monster ehp.
        /// </summary>
        /// <param name="defrate">The defrate.</param>
        /// <param name="monsterhp">The monsterhp.</param>
        /// <param name="monsterdefrate">The monsterdefrate.</param>
        /// <returns></returns>
        public int DisplayMonsterEHP(float defrate, int monsterhp, string monsterdefrate)
        {
            if (defrate > 0)
            {
                //    if (isMonsterFocused == false && debounce == true)
                //    {
                //        isMonsterFocused = true;
                //        return (int)(monsterhp / float.Parse(monsterdefrate, CultureInfo.InvariantCulture.NumberFormat));
                //    } else if (isMonsterFocused == true && debounce == false)
                //    {
                //        return (int)(monsterhp / float.Parse(monsterdefrate, CultureInfo.InvariantCulture.NumberFormat));
                //    }
                //}
                
                return (int)(monsterhp / float.Parse(monsterdefrate, CultureInfo.InvariantCulture.NumberFormat));
            }
            return 0;
        }

        /// <summary>
        /// Reloads the maximum ehp.
        /// </summary>
        public void ReloadMaxEHP()
        {
            if (SavedMonster1MaxHP < Monster1HPInt())
                SavedMonster1MaxHP = (int)(Monster1HPInt() / float.Parse(Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat));
            if (SavedMonster2MaxHP < Monster2HPInt())
                SavedMonster2MaxHP = (int)(Monster2HPInt() / float.Parse(Monster2DefMult(), CultureInfo.InvariantCulture.NumberFormat));
            if (SavedMonster3MaxHP < Monster3HPInt())
                SavedMonster3MaxHP = (int)(Monster3HPInt() / float.Parse("1", CultureInfo.InvariantCulture.NumberFormat));
            if (SavedMonster4MaxHP < Monster4HPInt())
                SavedMonster4MaxHP = (int)(Monster4HPInt() / float.Parse("1", CultureInfo.InvariantCulture.NumberFormat));
        }

        /// <summary>
        /// Gets the defrate multiplier.
        /// </summary>
        /// <value>
        /// The defrate multiplier.
        /// </value>
        public string DefMult
        {
            get
            {
                
                switch (SelectedMonster)
                {
                    case 0:
                        //showMonsterEHP(ShowMonsterEHP, float.Parse(Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat), Monster1HPInt(), Monster1DefMult(),true);
                        //SavedMonster1MaxHP = (int)(Monster1HPInt() / float.Parse(Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat));
                        return Monster1DefMult();
                    case 1:
                        //showMonsterEHP(ShowMonsterEHP, float.Parse(Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat), Monster2HPInt(), Monster2DefMult(),false);
                        return Monster2DefMult();
                    default:
                        //showMonsterEHP(ShowMonsterEHP, float.Parse(Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat), Monster1HPInt(), Monster1DefMult(),false);
                        return Monster1DefMult();
                }
            }
        }

        #region monster status

        /// <summary>
        /// Gets the current poison.
        /// </summary>
        /// <value>
        /// The poison current.
        /// </value>
        public int PoisonCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1Poison(),
                    1 => Monster2Poison(),
                    _ => Monster1Poison(),
                };
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
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1PoisonNeed(),
                    1 => Monster2PoisonNeed(),
                    _ => Monster1PoisonNeed(),
                };
            }
        }

        /// <summary>
        /// Gets the current sleep.
        /// </summary>
        /// <value>
        /// The sleep current.
        /// </value>
        public int SleepCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1Sleep(),
                    1 => Monster2Sleep(),
                    _ => Monster1Sleep(),
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
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1SleepNeed(),
                    1 => Monster2SleepNeed(),
                    _ => Monster1SleepNeed(),
                };
            }
        }

        /// <summary>
        /// Gets the current paralysis.
        /// </summary>
        /// <value>
        /// The para current.
        /// </value>
        public int ParaCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1Para(),
                    1 => Monster2Para(),
                    _ => Monster1Para(),
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
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1ParaNeed(),
                    1 => Monster2ParaNeed(),
                    _ => Monster1ParaNeed(),
                };
            }
        }

        /// <summary>
        /// Gets the current blast.
        /// </summary>
        /// <value>
        /// The blast current.
        /// </value>
        public int BlastCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1Blast(),
                    1 => Monster2Blast(),
                    _ => Monster1Blast(),
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
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1BlastNeed(),
                    1 => Monster2BlastNeed(),
                    _ => Monster1BlastNeed(),
                };
            }
        }

        /// <summary>
        /// Gets the current stun.
        /// </summary>
        /// <value>
        /// The stun current.
        /// </value>
        public int StunCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1Stun(),
                    1 => Monster2Stun(),
                    _ => Monster1Stun(),
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
                if (Configuring)
                    return 100;
                return SelectedMonster switch
                {
                    0 => Monster1StunNeed(),
                    1 => Monster2StunNeed(),
                    _ => Monster1StunNeed(),
                };
            }
        }

        #endregion

        /// <summary>
        /// Gets the name of Duremudira.
        /// </summary>
        /// <returns></returns>
        public string getDureName()
        {
            if (QuestID() == 21731 || QuestID() == 21749)
                return "1st District Duremudira";
            else if (QuestID() == 21746 || QuestID() == 21750)
                return "2nd District Duremudira";
            else if (QuestID() == 21747 || QuestID() == 21734)
                return "3rd District Duremudira";
            else if (QuestID() == 21748)
                return "4th District Duremudira";
            else if (QuestID() == 23648 || QuestID() == 23649)
                return "Arrogant Duremudira";
            else 
                return "None";
        }

        //quest ids
        //ravi 62105 TODO: same ids in all phases?
        //violent 62101 
        //berserk
        //berserk practice
        // support 1 55803 
        //extreme
        /// <summary>
        /// Gets the name of the ravi.
        /// </summary>
        /// <returns></returns>
        public string getRaviName()
        {
            //quest ids:
            //mp road: 23527
            //solo road: 23628
            //1st district dure: 21731
            //2nd district dure: 21746
            //1st district dure sky corridor: 21749
            //2nd district dure sky corridor: 21750
            //arrogant dure repel: 23648
            //arrogant dure slay: 23649
            //urgent tower: 21751
            //4th district dure: 21748
            //3rd district dure: 21747
            //3rd district dure 2: 21734
            //UNUSED sky corridor: 21730
            //sky corridor prologue: 21729
            //raviente 62105
            //raviente carve 62108
            ///violent raviente 62101
            ///violent carve 62104
            //berserk slay practice 55796
            //berserk support practice 1 55802
            //berserk support practice 2 55803
            //berserk support practice 3 55804
            //berserk support practice 4 55805
            //berserk support practice 5 55806
            //berserk practice carve 55807
            //berserk slay  54751
            //berserk support 1 54756
            //berserk support 2 54757
            //berserk support 3 54758
            //berserk support 4 54759
            //berserk support 5 54760
            //berserk carve 54761
            //extreme slay (musou table 54) 55596 
            //extreme support 1 55602
            //extreme support 2 55603
            //extreme support 3 55604
            //extreme support 4 55605
            //extreme support 5 55606
            //extreme carve 55607
            if (QuestID() == 62105 || QuestID() == 62108)
                return "Raviente";
            else if (QuestID() == 62101 || QuestID() == 62104)
                return "Violent Raviente";
            else if (QuestID() == 55796 || QuestID() == 55802 || QuestID() == 55803 || QuestID() == 55804 || QuestID() == 55805 || QuestID() == 55806 || QuestID() == 55807)
                return "Berserk Raviente Practice";
            else if (QuestID() == 54751 || QuestID() == 54756 || QuestID() == 54757 || QuestID() == 54758 || QuestID() == 54759 || QuestID() == 54760 || QuestID() == 54761)
                return "Berserk Raviente";
            else if (QuestID() == 55596 || QuestID() == 55602 || QuestID() == 55603 || QuestID() == 55604 || QuestID() == 55605 || QuestID() == 55606 || QuestID() == 55607)
                return "Extreme Raviente";
            else
                return "None";
        }

        public string Monster1Name => getDureName() != "None" ? getDureName() : getRaviName() != "None" ? getRaviName() : getMonsterName(GetNotRoad() || RoadSelectedMonster() == 0 ? LargeMonster1ID() : LargeMonster2ID()); //monster 1 is used for the first display and road uses 2nd choice to store 2nd monster
        public string Monster2Name => CaravanOverride() ? getMonsterName(CaravanMonster2ID(),false) : getMonsterName(LargeMonster2ID(),false);
        public string Monster3Name => getMonsterName(LargeMonster3ID(),false);
        public string Monster4Name => getMonsterName(LargeMonster4ID(),false);

        /// <summary>
        /// Gets the name of the rank.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetRankName(int id)
        {
            switch (id)
            {
                case 0:
                    return "";
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    return "Low Rank ";
                case 11:
                    return "Low/High Rank ";
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    return "High Rank ";
                case 26:
                case 31:
                case 42:
                    return "HR5 ";
                case 32:
                case 46://supremacies
                        //if (GetRealMonsterName(DataLoader.model.CurrentMonster1Icon).Contains("Supremacy"))
                        //{
                    return "Supremacy ";
                //} else
                //{
                //   return "";
                //}
                case 53://: conquest levels via quest id
                        //shantien
                        //lv1 23585
                        //lv200 23586
                        //lv1000 23587
                        //lv9999 23588
                        //disufiroa
                        //lv1 23589
                        //lv200 23590
                        //lv1000 23591
                        //lv9999 23592
                        //fatalis
                        //lv1 23593
                        //lv200 23594
                        //lv1000 23595
                        //lv9999 23596
                        //crimson fatalis
                        //lv1 23597
                        //lv200 23598
                        //lv1000 23599
                        //lv9999 23601
                        //upper shiten unknown 23605
                        //lower shiten unknown 23604
                        //upper shiten disufiroa 23603
                        //lower shiten disu 23602
                        //thirsty 55532
                        //shifting 55920
                        //starving 55916
                        //golden 55917
                    switch (QuestID())
                    {
                        default:
                            return "G Rank ";
                        case 23585:
                        case 23589:
                        case 23593:
                        case 23597:
                            return "Lv1 ";
                        case 23586:
                        case 23590:
                        case 23594:
                        case 23598:
                            return "Lv200 ";
                        case 23587:
                        case 23591:
                        case 23595:
                        case 23599:
                            return "Lv1000 ";
                        case 23588:
                        case 23592:
                        case 23596:
                        case 23601:
                            return "Lv9999 ";
                    }

                case 54:
                    switch (QuestID())
                    {
                        default:
                            return "";
                        case 23604:
                        case 23602:
                            return "Lower Shiten ";
                    }
                //return ""; //20m lower shiten/musou repel/musou lesser slay
                case 55:
                    switch (QuestID())
                    {
                        default:
                            return "";
                        case 23603:
                            return "Upper Shiten ";
                    }
                //10m upper shiten/musou true slay


                case 56://twinhead rajang / voljang and rajang
                case 57://twinhead mi ru / white and brown espi / unknown and zeru / rajang and dorag
                    return "Twinhead ";
                case 64:
                    return "Zenith★1 ";
                case 65:
                    return "Zenith★2 ";
                case 66:
                    return "Zenith★3 ";
                case 67:
                    return "Zenith★4 ";
                case 70://unknown
                    return "Upper Shiten ";
                case 71:
                case 72:
                case 73:
                    return "Interception ";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Gets the real name of the monster.
        /// </summary>
        /// <value>
        /// The real name of the monster.
        /// </value>
        public string RealMonsterName
        {
            get {
                //string RealName = CurrentMonster1Icon.Replace("https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/main/img/monster/", "");
                //RealName = RealName.Replace(".gif", "");
                //RealName = RealName.Replace(".png", "");
                //RealName = RealName.Replace("zenith_", "Zenith ");
                //RealName = RealName.Replace("_", " ");

                ////https://stackoverflow.com/questions/4315564/capitalizing-words-in-a-string-using-c-sharp
                //RealName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(RealName);

                int id;

                if (roadOverride() == false)
                    id = RoadSelectedMonster() == 0 ? LargeMonster1ID() : LargeMonster2ID();
                else if (CaravanOverride())
                    id = CaravanMonster1ID();
                else
                    id = LargeMonster1ID();

                //dure
                if (QuestID() == 21731 || QuestID() == 21746 || QuestID() == 21749 || QuestID() == 21750)
                    return "Duremudira";
                else if (QuestID() == 23648 || QuestID() == 23649)
                    return "Arrogant Duremudira";

                switch (id)
                {
                    case 0: //none
                        return "None";
                    case 1:
                        return "Rathian";
                    case 2:
                        if (RankBand() == 53)
                            return "Fatalis";
                        else
                            return "Fatalis";
                    case 3:
                        return "Kelbi";
                    case 4:
                        return "Mosswine";
                    case 5:
                        return "Bullfango";
                    case 6:
                        return "Yian Kut-Ku";
                    case 7:
                        return "Lao-Shan Lung";
                    case 8:
                        return "Cephadrome";
                    case 9:
                        return "Felyne";
                    case 10: //veggie elder
                        return "Veggie Elder";
                    case 11:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Rathalos";
                        else
                            return "Rathalos";
                    case 12:
                        return "Aptonoth";
                    case 13:
                        return "Genprey";
                    case 14:
                        return "Diablos";
                    case 15:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Khezu";
                        else
                            return "Khezu";
                    case 16:
                        return "Velociprey";
                    case 17:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Gravios";
                        else
                            return "Gravios";
                    case 18:
                        return "Felyne";
                    case 19:
                        return "Vespoid";
                    case 20:
                        return "Gypceros";
                    case 21:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Plesioth";
                        else
                            return "Plesioth";
                    case 22:
                        return "Basarios";
                    case 23:
                        return "Melynx";
                    case 24:
                        return "Hornetaur";
                    case 25:
                        return "Apceros";
                    case 26:
                        return "Monoblos";
                    case 27:
                        return "Velocidrome";
                    case 28:
                        return "Gendrome";
                    case 29://rocks
                        return "Rocks";
                    case 30:
                        return "Ioprey";
                    case 31:
                        return "Iodrome";
                    case 32://pugis
                        return "Poogie";
                    case 33:
                        return "Kirin";
                    case 34:
                        return "Cephalos";
                    case 35:
                        return "Giaprey";
                    case 36:
                        if (RankBand() == 53)
                            return "Crimson Fatalis";
                        else
                            return "Crimson Fatalis";
                    case 37:
                        return "Pink Rathian";
                    case 38:
                        return "Blue Yian Kut-Ku";
                    case 39:
                        return "Purple Gypceros";
                    case 40:
                        return "Yian Garuga";
                    case 41:
                        return "Silver Rathalos";
                    case 42:
                        return "Gold Rathian";
                    case 43:
                        return "Black Diablos";
                    case 44:
                        return "White Monoblos";
                    case 45:
                        return "Red Khezu";
                    case 46:
                        return "Green Plesioth";
                    case 47:
                        return "Black Gravios";
                    case 48:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Daimyo Hermitaur";
                        else
                            return "Daimyo Hermitaur";
                    case 49:
                        return "Azure Rathalos";
                    case 50:
                        return "Ashen Lao-Shan Lung";
                    case 51:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Blangonga";
                        else
                            return "Blangonga";
                    case 52:
                        return "Congalala";
                    case 53:
                        if (RankBand() == 56 || RankBand() == 57)
                            return "Rajang";
                        else
                            return "Rajang";
                    case 54:
                        return "Kushala Daora";
                    case 55:
                        return "Shen Gaoren";
                    case 56:
                        return "Great Thunderbug";
                    case 57:
                        return "Shakalaka";
                    case 58:
                        return "Yama Tsukami";
                    case 59:
                        return "Chameleos";
                    case 60:
                        return "Rusted Kushala Daora";
                    case 61:
                        return "Blango";
                    case 62:
                        return "Conga";
                    case 63:
                        return "Remobra";
                    case 64:
                        return "Lunastra";
                    case 65:
                        if (RankBand() == 32)
                            return "Teostra";
                        else
                            return "Teostra";
                    case 66:
                        return "Hermitaur";
                    case 67:
                        return "Shogun Ceanataur";
                    case 68:
                        return "Bulldrome";
                    case 69:
                        return "Anteka";
                    case 70:
                        return "Popo";
                    case 71:
                        if (RankBand() == 53)
                            return "White Fatalis";
                        else
                            return "White Fatalis";
                    case 72:
                        return "Yama Tsukami";
                    case 73:
                        return "Ceanataur";
                    case 74:
                        return "Hypnoc";
                    case 75:
                        return "Lavasioth";
                    case 76:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Tigrex";
                        else
                            return "Tigrex";
                    case 77:
                        return "Akantor";
                    case 78:
                        return "Bright Hypnoc";
                    case 79:
                        return "Red Lavasioth";
                    case 80:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Espinas";
                        else
                            return "Espinas";
                    case 81:
                        return "Orange Espinas";
                    case 82:
                        return "Silver Hypnoc";
                    case 83:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Akura Vashimu";
                        else
                            return "Akura Vashimu";
                    case 84:
                        return "Akura Jebia";
                    case 85:
                        return "Berukyurosu";
                    case 86://cactus
                        return "Cactus";
                    case 87://gorge objects
                        return "Gorge Object";
                    case 88://gorge rocks
                        return "Gorge Rock";
                    case 89:
                        if (RankBand() == 54)
                            return "Thirsty Pariapuria";
                        else if (RankBand() == 32)//supremacy
                            return "Pariapuria";
                        else
                            return "Pariapuria";
                    case 90:
                        return "White Espinas";
                    case 91:
                        return "Kamu Orugaron";
                    case 92:
                        return "Nono Orugaron";
                    case 93:
                        return "Raviente";
                    case 94:
                        return "Dyuragaua";
                    case 95:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Doragyurosu";
                        else if (RankBand() == 32)
                            return "Doragyurosu";
                        else
                            return "Doragyurosu";
                    case 96:
                        return "Gurenzeburu";
                    case 97:
                        return "Burukku";
                    case 98:
                        return "Erupe";
                    case 99:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Rukodiora";
                        else
                            return "Rukodiora";
                    case 100:
                        if (RankBand() == 70 || RankBand() == 54)
                            return "Unknown";
                        else
                            return "Unknown";
                    case 101:
                        return "Gogomoa";
                    case 102://kokomoa
                        return "Kokomoa";
                    case 103:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Taikun Zamuza";
                        else
                            return "Taikun Zamuza";
                    case 104:
                        return "Abiorugu";
                    case 105:
                        return "Kuarusepusu";
                    case 106:
                        if (RankBand() == 32)
                            return "Odibatorasu";
                        else
                            return "Odibatorasu";
                    case 107:
                        if (RankBand() == 54 || RankBand() == 55)
                            return "Disufiroa";
                        else
                            return "Disufiroa";
                    case 108:
                        return "Rebidiora";
                    case 109:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Anorupatisu";
                        else
                            return "Anorupatisu";
                    case 110:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Zenith Hyujikiki";
                        else
                            return "Hyujikiki";
                    case 111:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Midogaron";
                        else
                            return "Midogaron";
                    case 112:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Giaorugu";
                        else
                            return "Giaorugu";
                    case 113:
                        if (RankBand() == 55)
                            return "Shifting Mi Ru";
                        else
                            return "Mi Ru";
                    case 114:
                        return "Farunokku";
                    case 115:
                        return "Pokaradon";
                    case 116:
                        if (RankBand() == 53)
                            return "Shantien";
                        else
                            return "Shantien";
                    case 117:
                        return "Pokara";
                    case 118://dummy
                        return "Dummy";
                    case 119:
                        return "Goruganosu";
                    case 120:
                        return "Aruganosu";
                    case 121:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Baruragaru";
                        else
                            return "Baruragaru";
                    case 122:
                        return "Zerureusu";
                    case 123:
                        return "Gougarf";
                    case 124:
                        return "Uruki";
                    case 125:
                        return "Forokururu";
                    case 126:
                        return "Meraginasu";
                    case 127:
                        return "Diorex";
                    case 128:
                        return "Garuba Daora";
                    case 129:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Inagami";
                        else
                            return "Inagami";
                    case 130:
                        return "Varusaburosu";
                    case 131:
                        return "Poborubarumu";
                    case 132:
                        return "Duremudira";
                    case 133://UNK
                        return "UNK";
                    case 134:
                        return "Felyne";
                    case 135://blue npc
                        return "Blue NPC";
                    case 136://UNK
                        return "UNK";
                    case 137://cactus
                        return "Cactus";
                    case 138://veggie elders
                        return "Veggie Elder";
                    case 139:
                        return "Gureadomosu";
                    case 140:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Harudomerugu";
                        else
                            return "Harudomerugu";
                    case 141:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Toridcless";
                        else
                            return "Toridcless";
                    case 142:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return "Gasurabazura";
                        else
                            return "Gasurabazura";
                    case 143:
                        return "Kusubami";
                    case 144:
                        return "Yama Kurai";
                    case 145://3rd phase duremudira
                        return "Duremudira";
                    case 146:
                        if (RankBand() >= 54 && RankBand() <= 55)
                            return "Howling Zinogre";
                        else
                            return "Zinogre";
                    case 147:
                        return "Deviljho";
                    case 148:
                        return "Brachydios";
                    case 149:
                        return "Berserk Raviente";
                    case 150:
                        return "Toa Tesukatora";
                    case 151:
                        return "Barioth";
                    case 152:
                        return "Uragaan";
                    case 153:
                        return "Stygian Zinogre";
                    case 154:
                        if (RankBand() >= 54 && RankBand() <= 55)
                            return "Ruling Guanzorumu";
                        else
                            return "Guanzorumu";
                    case 155:
                        if (RankBand() == 55)
                            return "Golden Deviljho";
                        else
                            return "Starving Deviljho";
                    case 156://UNK
                        return "UNK";
                    case 157://egyurasu
                        return "Egyurasu";
                    case 158:
                        return "Voljang";
                    case 159:
                        return "Nargacuga";
                    case 160:
                        return "Keoaruboru";
                    case 161:
                        return "Zenaserisu";
                    case 162:
                        return "Gore Magala";
                    case 163:
                        return "Blinking Nargacuga";
                    case 164:
                        return "Shagaru Magala";
                    case 165:
                        return "Amatsu";
                    case 166:
                        if (RankBand() >= 54 && RankBand() <= 55)
                            return "Burning Freezing Elzelion";
                        else
                            return "Elzelion";
                    case 167:
                        return "Arrogant Duremudira";
                    case 168://rocks
                        return "Rock";
                    case 169:
                        return "Seregios";
                    case 170:
                        return "Bogabadorumu";
                    case 171://unknown blue barrel
                        return "Blue Barrel";
                    case 172:
                        return "Blitzkrieg Bogabadorumu";
                    case 173://costumed uruki
                        return "Uruki";
                    case 174:
                        return "Sparkling Zerureusu";
                    case 175://PSO2 Rappy
                        return "PSO2 Rappy";
                    case 176:
                        return "King Shakalaka";
                    default:
                        return "Loading...";
                }
            }
            //quest ids:
            //mp road: 23527
            //solo road: 23628
            //1st district dure: 21731
            //2nd district dure: 21746
            //1st district dure sky corridor: 21749
            //2nd district dure sky corridor: 21750
            //arrogant dure repel: 23648
            //arrogant dure slay: 23649
            //urgent tower: 21751
            //4th district dure: 21748
            //3rd district dure: 21747
            //3rd district dure 2: 21734
            //UNUSED sky corridor: 21730
            //sky corridor prologue: 21729

        }

        /// <summary>
        /// Gets the name of the monster.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string getMonsterName(int id, bool isFirstMonster = true)
        {
            if (Configuring)
                return "Blitzkrieg Bogabadorumu";
            if (id == 0)
                return "";
            Dictionary.List.MonsterID.TryGetValue(id, out string? monstername);

            if (monstername != null && monstername != RealMonsterName && isFirstMonster)
                return string.Format("{0}{1}",GetRankName(RankBand()), RealMonsterName);
            else
                return string.Format("{0}{1}", GetRankName(RankBand()), monstername); ;
        }

        #region monster hp

        //DisplayMonsterEHP(float.Parse(Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat), Monster1HPInt(), Monster1DefMult()).ToString()
        public string Monster1HP => Configuring ? "0" : ShowMonsterEHP() ? DisplayMonsterEHP(float.Parse(Monster1DefMult(), CultureInfo.InvariantCulture.NumberFormat), Monster1HPInt(), Monster1DefMult()).ToString() : Monster1HPInt().ToString();


        public string Monster1MaxHP
        {
            get
            {
                if (Configuring)
                    return "1";
                if (TimeDefInt() == TimeInt())
                    SavedMonster1MaxHP = Monster1HPInt();
                if (LargeMonster1ID() > 0 && SavedMonster1ID == 0)
                {
                    SavedMonster1MaxHP = Monster1HPInt();
                    SavedMonster1ID = LargeMonster1ID();

                }
                if (SavedMonster1ID > 0)
         
                SavedMonster1ID = LargeMonster1ID();

                if (GetNotRoad() || RoadSelectedMonster() == 0)
                {
                    return SavedMonster1MaxHP.ToString();
                }
                else
                {
                    return Monster2MaxHP;
                }
            }
        }
        public string Monster2HP => Configuring ? "0" : ShowMonsterEHP() ? DisplayMonsterEHP(float.Parse(Monster2DefMult(), CultureInfo.InvariantCulture.NumberFormat), Monster2HPInt(), Monster2DefMult()).ToString() : Monster2HPInt().ToString();

        public string Monster2MaxHP
        {
            get
            {
                if (Configuring)
                    return "1";
                if (TimeDefInt() == TimeInt())
                    SavedMonster2MaxHP = Monster2HPInt();
                if (RoadSelectedMonster() > 0 && SavedMonster2ID == 0)
                {
                    SavedMonster2MaxHP = Monster2HPInt();
                    SavedMonster2ID = RoadSelectedMonster();
                }
                if (SavedMonster2ID > 0)
                    SavedMonster2ID = RoadSelectedMonster();

                return SavedMonster2MaxHP.ToString();
            }
        }
        public string Monster3HP => Configuring ? "0" : ShowMonsterEHP() ? DisplayMonsterEHP(float.Parse("1", CultureInfo.InvariantCulture.NumberFormat), Monster3HPInt(), "1").ToString() : Monster3HPInt().ToString();


        public string Monster3MaxHP
        {
            get
            {
                if (Configuring)
                    return "1";
                if (TimeDefInt() == TimeInt())
                    SavedMonster3MaxHP = Monster3HPInt();
                return SavedMonster3MaxHP.ToString();
            }
        }
        public string Monster4HP => Configuring ? "0" : ShowMonsterEHP() ? DisplayMonsterEHP(float.Parse("1", CultureInfo.InvariantCulture.NumberFormat), Monster4HPInt(), "1").ToString() : Monster4HPInt().ToString();

        public string Monster4MaxHP
        {
            get
            {
                if (Configuring)
                    return "1";
                if (TimeDefInt() == TimeInt())
                    SavedMonster4MaxHP = Monster4HPInt();
                return SavedMonster4MaxHP.ToString();
            }
        }

        #endregion

        /* 
        Multipliers 
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
        public float GetMultFromWeaponType(int weaponType)
        {
            return weaponType switch
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
        }

        /// <summary>
        /// Gets the name of the weapon type.
        /// </summary>
        /// <param name="weaponType">Type of the weapon.</param>
        /// <returns></returns>
        public string GetWeaponNameFromType(int weaponType)
        {
            return weaponType switch
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
                _ => "",
            };
        }

        #region monster icon

        

        /// <summary>
        /// Gets the current monster1 icon.
        /// </summary>
        /// <value>
        /// The current monster1 icon.
        /// </value>
        public string CurrentMonster1Icon
        {
            get
            {//TODO: rework this
                string baseAddress = "https://raw.githubusercontent.com/DorielRivalet/MHFZ_Overlay/release/img/monster/";
                string extension1 = ".png";
                string extension2 = ".gif"; //zeniths and rainbow color
                int id;

                if (roadOverride() == false)
                    id = RoadSelectedMonster() == 0 ? LargeMonster1ID() : LargeMonster2ID();
                else if (CaravanOverride())
                    id = CaravanMonster1ID();
                else
                    id = LargeMonster1ID();

                //dure
                if (QuestID() == 21731 || QuestID() == 21746 || QuestID() == 21749 || QuestID() == 21750)
                    return baseAddress + "duremudira" + extension1;
                else if (QuestID() == 23648 || QuestID() == 23649)
                    return baseAddress + "arrogant_duremudira" + extension1;

                switch (id)
                {
                    case 0: //none
                        return baseAddress + "random" + extension1;
                    case 1:
                        return baseAddress + "rathian" + extension1;
                    case 2:
                        if (RankBand() == 53)
                            return baseAddress + "conquest_fatalis" + extension1;
                        else
                            return baseAddress + "fatalis" + extension1;
                    case 3:
                        return baseAddress + "kelbi" + extension1;
                    case 4:
                        return baseAddress + "mosswine" + extension1;
                    case 5:
                        return baseAddress + "bullfango" + extension1;
                    case 6:
                        return baseAddress + "yian_kut-ku" + extension1;
                    case 7:
                        return baseAddress + "lao-shan_lung" + extension1;
                    case 8:
                        return baseAddress + "cephadrome" + extension1;
                    case 9:
                        return baseAddress + "felyne" + extension1;
                    case 10: //veggie elder
                        return baseAddress + "random" + extension1; 
                    case 11:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_rathalos" + extension2;
                        else
                            return baseAddress + "rathalos" + extension1;
                    case 12:
                        return baseAddress + "aptonoth" + extension1;
                    case 13:
                        return baseAddress + "genprey" + extension1;
                    case 14:
                        return baseAddress + "diablos" + extension1;
                    case 15:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_khezu" + extension2;
                        else
                            return baseAddress + "khezu" + extension1;
                    case 16:
                        return baseAddress + "velociprey" + extension1;
                    case 17:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_gravios" + extension2;
                        else
                            return baseAddress + "gravios" + extension1;
                    case 18:
                        return baseAddress + "felyne" + extension1;
                    case 19:
                        return baseAddress + "vespoid" + extension1;
                    case 20:
                        return baseAddress + "gypceros" + extension1;
                    case 21:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_plesioth" + extension2;
                        else
                            return baseAddress + "plesioth" + extension1;
                    case 22:
                        return baseAddress + "basarios" + extension1;
                    case 23:
                        return baseAddress + "melynx" + extension1;
                    case 24:
                        return baseAddress + "hornetaur" + extension1;
                    case 25:
                        return baseAddress + "apceros" + extension1;
                    case 26:
                        return baseAddress + "monoblos" + extension1;
                    case 27:
                        return baseAddress + "velocidrome" + extension1;
                    case 28:
                        return baseAddress + "gendrome" + extension1;
                    case 29://rocks
                        return baseAddress + "random" + extension1;
                    case 30:
                        return baseAddress + "ioprey" + extension1;
                    case 31:
                        return baseAddress + "iodrome" + extension1;
                    case 32://pugis
                        return baseAddress + "random" + extension1;
                    case 33:
                        return baseAddress + "kirin" + extension1;
                    case 34:
                        return baseAddress + "cephalos" + extension1;
                    case 35:
                        return baseAddress + "giaprey" + extension1;
                    case 36:
                        if (RankBand() == 53)
                            return baseAddress + "conquest_crimson_fatalis" + extension1;
                        else
                            return baseAddress + "crimson_fatalis" + extension1;
                    case 37:
                        return baseAddress + "pink_rathian" + extension1;
                    case 38:
                        return baseAddress + "blue_yian_kut-ku" + extension1;
                    case 39:
                        return baseAddress + "purple_gypceros" + extension1;
                    case 40:
                        return baseAddress + "yian_garuga" + extension1;
                    case 41:
                        return baseAddress + "silver_rathalos" + extension1;
                    case 42:
                        return baseAddress + "gold_rathian" + extension1;
                    case 43:
                        return baseAddress + "black_diablos" + extension1;
                    case 44:
                        return baseAddress + "white_monoblos" + extension1;
                    case 45:
                        return baseAddress + "red_khezu" + extension1;
                    case 46:
                        return baseAddress + "green_plesioth" + extension1;
                    case 47:
                        return baseAddress + "black_gravios" + extension1;
                    case 48:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_daimyo_hermitaur" + extension2;
                        else
                            return baseAddress + "daimyo_hermitaur" + extension1;
                    case 49:
                        return baseAddress + "azure_rathalos" + extension1;
                    case 50:
                        return baseAddress + "ashen_lao-shan_lung" + extension1;
                    case 51:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_blangonga" + extension2;
                        else
                            return baseAddress + "blangonga" + extension1;
                    case 52:
                        return baseAddress + "congalala" + extension1;
                    case 53:
                        if (RankBand() == 56 || RankBand() == 57)
                            return baseAddress + "twinhead_rajang" + extension1;
                        else
                            return baseAddress + "rajang" + extension1;
                    case 54:
                        return baseAddress + "kushala_daora" + extension1;
                    case 55:
                        return baseAddress + "shen_gaoren" + extension1;
                    case 56:
                        return baseAddress + "great_thunderbug" + extension1;
                    case 57:
                        return baseAddress + "shakalaka" + extension1;
                    case 58:
                        return baseAddress + "yama_tsukami" + extension1;
                    case 59:
                        return baseAddress + "chameleos" + extension1;
                    case 60:
                        return baseAddress + "rusted_kushala_daora" + extension1;
                    case 61:
                        return baseAddress + "blango" + extension1;
                    case 62:
                        return baseAddress + "conga" + extension1;
                    case 63:
                        return baseAddress + "remobra" + extension1;
                    case 64:
                        return baseAddress + "lunastra" + extension1;
                    case 65:
                        if (RankBand() == 32)
                            return baseAddress + "supremacy_teostra" + extension1;
                        else
                            return baseAddress + "teostra" + extension1;
                    case 66:
                        return baseAddress + "hermitaur" + extension1;
                    case 67:
                        return baseAddress + "shogun_ceanataur" + extension1;
                    case 68:
                        return baseAddress + "bulldrome" + extension1;
                    case 69:
                        return baseAddress + "anteka" + extension1;
                    case 70:
                        return baseAddress + "popo" + extension1;
                    case 71:
                        if (RankBand() == 53)
                            return baseAddress + "road_white_fatalis" + extension1;
                        else
                            return baseAddress + "white_fatalis" + extension1;
                    case 72:
                        return baseAddress + "yama_tsukami" + extension1;
                    case 73:
                        return baseAddress + "ceanataur" + extension1;
                    case 74:
                        return baseAddress + "hypnoc" + extension1;
                    case 75:
                        return baseAddress + "lavasioth" + extension1;
                    case 76:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_tigrex" + extension2;
                        else
                            return baseAddress + "tigrex" + extension1;
                    case 77:
                        return baseAddress + "akantor" + extension1;
                    case 78:
                        return baseAddress + "bright_hypnoc" + extension1;
                    case 79:
                        return baseAddress + "red_lavasioth" + extension1;
                    case 80:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_espinas" + extension2;
                        else
                            return baseAddress + "espinas" + extension1;
                    case 81:
                        return baseAddress + "orange_espinas" + extension1;
                    case 82:
                        return baseAddress + "silver_hypnoc" + extension1;
                    case 83:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_akura_vashimu" + extension2;
                        else
                            return baseAddress + "akura_vashimu" + extension1;
                    case 84:
                        return baseAddress + "akura_jebia" + extension1;
                    case 85:
                        return baseAddress + "berukyurosu" + extension1;
                    case 86://cactus
                        return baseAddress + "random" + extension1;
                    case 87://gorge objects
                        return baseAddress + "random" + extension1;
                    case 88://gorge rocks
                        return baseAddress + "random" + extension1;
                    case 89:
                        if (RankBand() == 32 || RankBand() == 54)
                            return baseAddress + "thirsty_pariapuria" + extension1;
                        else
                            return baseAddress + "pariapuria" + extension1;
                    case 90:
                        return baseAddress + "white_espinas" + extension1;
                    case 91:
                        return baseAddress + "kamu_orugaron" + extension1;
                    case 92:
                        return baseAddress + "nono_orugaron" + extension1;
                    case 93:
                        return baseAddress + "raviente" + extension1;
                    case 94:
                        return baseAddress + "dyuragaua" + extension1;
                    case 95:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_doragyurosu" + extension2;
                        else if (RankBand() == 32)
                            return baseAddress + "supremacy_doragyurosu" + extension1;
                        else
                            return baseAddress + "doragyurosu" + extension1;
                    case 96:
                        return baseAddress + "gurenzeburu" + extension1;
                    case 97:
                        return baseAddress + "burukku" + extension1;
                    case 98:
                        return baseAddress + "erupe" + extension1;
                    case 99:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_rukodiora" + extension2;
                        else
                            return baseAddress + "rukodiora" + extension1;
                    case 100:
                        if (RankBand() == 70 || RankBand() == 54)
                            return baseAddress + "shiten_unknown" + extension1;
                        else
                            return baseAddress + "unknown" + extension1;
                    case 101:
                        return baseAddress + "gogomoa" + extension1;
                    case 102://kokomoa
                        return baseAddress + "gogomoa" + extension1;
                    case 103:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_taikun_zamuza" + extension2;
                        else
                            return baseAddress + "taikun_zamuza" + extension1;
                    case 104:
                        return baseAddress + "abiorugu" + extension1;
                    case 105:
                        return baseAddress + "kuarusepusu" + extension1;
                    case 106:
                        if (RankBand() == 32)
                            return baseAddress + "supremacy_odibatorasu" + extension1;
                        else
                            return baseAddress + "odibatorasu" + extension1;
                    case 107:
                        if (RankBand() == 54 || RankBand() == 55)
                            return baseAddress + "shiten_disufiroa" + extension1;
                        else
                            return baseAddress + "disufiroa" + extension1;
                    case 108:
                        return baseAddress + "rebidiora" + extension1;
                    case 109:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_anorupatisu" + extension2;
                        else
                            return baseAddress + "anorupatisu" + extension1;
                    case 110:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_hyujikiki" + extension2;
                        else
                            return baseAddress + "hyujikiki" + extension1;
                    case 111:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_midogaron" + extension2;
                        else
                            return baseAddress + "midogaron" + extension1;
                    case 112:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_giaorugu" + extension2;
                        else
                            return baseAddress + "giaorugu" + extension1;
                    case 113:
                        if (RankBand() == 55)
                            return baseAddress + "shifting_mi_ru" + extension1;
                        else
                            return baseAddress + "mi_ru" + extension1;
                    case 114:
                        return baseAddress + "farunokku" + extension1;
                    case 115:
                        return baseAddress + "pokaradon" + extension1;
                    case 116:
                        if (RankBand() == 53)
                            return baseAddress + "conquest_shantien" + extension1;
                        else
                            return baseAddress + "shantien" + extension1;
                    case 117:
                        return baseAddress + "pokara" + extension1;
                    case 118://dummy
                        return baseAddress + "random" + extension1;
                    case 119:
                        return baseAddress + "goruganosu" + extension1;
                    case 120:
                        return baseAddress + "aruganosu" + extension1;
                    case 121:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_baruragaru" + extension2;
                        else
                            return baseAddress + "baruragaru" + extension1;
                    case 122:
                        return baseAddress + "zerureusu" + extension1;
                    case 123:
                        return baseAddress + "gougarf" + extension1;
                    case 124:
                        return baseAddress + "uruki" + extension1;
                    case 125:
                        return baseAddress + "forokururu" + extension1;
                    case 126:
                        return baseAddress + "meraginasu" + extension1;
                    case 127:
                        return baseAddress + "diorex" + extension1;
                    case 128:
                        return baseAddress + "garuba_daora" + extension1;
                    case 129:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_inagami" + extension2;
                        else
                            return baseAddress + "inagami" + extension1;
                    case 130:
                        return baseAddress + "varusaburosu" + extension1;
                    case 131:
                        return baseAddress + "poborubarumu" + extension1;
                    case 132:
                        return baseAddress + "duremudira" + extension1;
                    case 133://UNK
                        return baseAddress + "random" + extension1;
                    case 134:
                        return baseAddress + "felyne" + extension1;
                    case 135://blue npc
                        return baseAddress + "random" + extension1;
                    case 136://UNK
                        return baseAddress + "random" + extension1;
                    case 137://cactus
                        return baseAddress + "random" + extension1;
                    case 138://veggie elders
                        return baseAddress + "random" + extension1;
                    case 139:
                        return baseAddress + "gureadomosu" + extension1;
                    case 140:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_harudomerugu" + extension2;
                        else
                            return baseAddress + "harudomerugu" + extension1;
                    case 141:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_toridcless" + extension2;
                        else
                            return baseAddress + "toridcless" + extension1;
                    case 142:
                        if (RankBand() >= 64 && RankBand() <= 67)
                            return baseAddress + "zenith_gasurabazura" + extension2;
                        else
                            return baseAddress + "gasurabazura" + extension1;
                    case 143:
                        return baseAddress + "kusubami" + extension1;
                    case 144:
                        return baseAddress + "yama_kurai" + extension1;
                    case 145://3rd phase duremudira
                        return baseAddress + "duremudira" + extension1;
                    case 146:
                        if (RankBand() >= 54 && RankBand() <= 55)
                            return baseAddress + "howling_zinogre" + extension1;
                        else
                            return baseAddress + "zinogre" + extension1;
                    case 147:
                        return baseAddress + "deviljho" + extension1;
                    case 148:
                        return baseAddress + "brachydios" + extension1;
                    case 149:
                        return baseAddress + "berserk_raviente" + extension1;
                    case 150:
                        return baseAddress + "toa_tesukatora" + extension1;
                    case 151:
                        return baseAddress + "barioth" + extension1;
                    case 152:
                        return baseAddress + "uragaan" + extension1;
                    case 153:
                        return baseAddress + "stygian_zinogre" + extension1;
                    case 154:
                        if (RankBand() >= 54 && RankBand() <= 55)
                            return baseAddress + "ruling_guanzorumu" + extension1;
                        else
                            return baseAddress + "guanzorumu" + extension1;
                    case 155:
                        if (RankBand() == 55)
                            return baseAddress + "golden_deviljho" + extension1;
                        else
                            return baseAddress + "starving_deviljho" + extension1;
                    case 156://UNK
                        return baseAddress + "random" + extension1;
                    case 157://egyurasu
                        return baseAddress + "random" + extension1;
                    case 158:
                        return baseAddress + "voljang" + extension1;
                    case 159:
                        return baseAddress + "nargacuga" + extension1;
                    case 160:
                        return baseAddress + "keoaruboru" + extension1;
                    case 161:
                        return baseAddress + "zenaserisu" + extension1;
                    case 162:
                        return baseAddress + "gore_magala" + extension1;
                    case 163:
                        return baseAddress + "blinking_nargacuga" + extension1;
                    case 164:
                        return baseAddress + "shagaru_magala" + extension1;
                    case 165:
                        return baseAddress + "amatsu" + extension1;
                    case 166:
                        if (RankBand() >= 54 && RankBand() <= 55)
                            return baseAddress + "burning_freezing_elzelion" + extension1;
                        else
                            return baseAddress + "elzelion" + extension1;
                    case 167:
                        return baseAddress + "arrogant_duremudira" + extension1;
                    case 168://rocks
                        return baseAddress + "random" + extension1;
                    case 169:
                        return baseAddress + "seregios" + extension1;
                    case 170:
                        return baseAddress + "zenith_bogabadorumu" + extension2;
                    case 171://unknown blue barrel
                        return baseAddress + "random" + extension1;
                    case 172:
                        return baseAddress + "blitzkrieg_bogabadorumu" + extension1;
                    case 173://costumed uruki
                        return baseAddress + "uruki" + extension1;
                    case 174:
                        return baseAddress + "sparkling_zerureusu" + extension1;
                    case 175://PSO2 Rappy
                        return baseAddress + "random" + extension1;
                    case 176:
                        return baseAddress + "king_shakalaka" + extension1;
                    default:
                        return baseAddress + "random" + extension1;
                }
            }
        }

        public string CurrentMap
        {
            get
            {
                int id = AreaID();

                switch (id)
                {
                    case 0://Loading
                        return "";
                    case 1://Jungle areas
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 18:
                    case 19:
                    case 22:
                    case 23:
                    case 26:
                    case 110:
                    case 111:
                    case 112:
                    case 113:
                    case 114:
                    case 115:
                    case 116:
                    case 117:
                    case 118:
                    case 119:
                    case 120:
                    case 212:
                    case 213:
                        return "https://i.imgur.com/Ht8JRBX.png";
                    case 6: //Snowy mountain areas
                    case 15:
                    case 92:
                    case 93:
                    case 94:
                    case 95:
                    case 96:
                    case 97:
                    case 98:
                    case 99:
                    case 100:
                    case 101:
                    case 102:
                    case 103:
                    case 104:
                    case 105:
                    case 106:
                    case 107:
                    case 108:
                    case 109:
                    case 218:
                    case 219:
                        return "https://i.imgur.com/UtMk4EM.png";
                    case 7: //Desert areas
                    case 24:
                    case 45:
                    case 47:
                    case 48:
                    case 49:
                    case 50:
                    case 51:
                    case 52:
                    case 53:
                    case 54:
                    case 55:
                    case 56:
                    case 140:
                    case 141:
                    case 142:
                    case 143:
                    case 144:
                    case 145:
                    case 146:
                    case 147:
                    case 148:
                    case 149:
                    case 150:
                    case 214:
                    case 215:
                        return "https://i.imgur.com/zO8HYi7.png";
                    case 8://Volcano areas
                    case 27:
                    case 58:
                    case 59:
                    case 60:
                    case 61:
                    case 62:
                    case 63:
                    case 64:
                    case 65:
                    case 74:
                    case 161:
                    case 162:
                    case 163:
                    case 164:
                    case 165:
                    case 166:
                    case 167:
                    case 168:
                    case 169:
                    case 216:
                    case 217:
                    case 220:
                    case 221:
                    case 222:
                    case 223:
                        return "https://i.imgur.com/c59Z6Hw.png";
                    case 9://Swamp areas
                    case 16:
                    case 29:
                    case 44:
                    case 67:
                    case 68:
                    case 69:
                    case 70:
                    case 71:
                    case 72:
                    case 73:
                    case 75:
                    case 151:
                    case 152:
                    case 153:
                    case 154:
                    case 155:
                    case 156:
                    case 157:
                    case 158:
                    case 159:
                    case 160:
                        return "https://i.imgur.com/KJ3QEo2.png";
                    case 21://Forest and Hills areas
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                    case 40:
                    case 41:
                    case 42:
                    case 43:
                    case 184:
                    case 185:
                    case 186:
                    case 187:
                    case 188:
                    case 189:
                    case 190:
                    case 191:
                    case 192:
                    case 193:
                    case 194:
                    case 195:
                    case 196:
                        return "https://i.imgur.com/aVE87MJ.png";
                    case 224://Great Forest areas
                    case 225:
                    case 226:
                    case 227:
                    case 228:
                    case 229:
                    case 230:
                    case 231:
                    case 232:
                    case 233:
                    case 234:
                    case 235:
                    case 236:
                    case 237:
                    case 238:
                    case 239:
                    case 240:
                    case 241:
                        return "https://i.imgur.com/mcAeLy6.png";
                    case 247://Highlands areas
                    case 248:
                    case 249:
                    case 250:
                    case 251:
                    case 252:
                    case 253:
                    case 254:
                    case 255:
                    case 302:
                    case 303:
                    case 304:
                    case 305:
                    case 306:
                    case 307:
                    case 308:
                        return "https://i.imgur.com/EY4FiOa.png";
                    case 322://Tidal Island areas
                    case 323:
                    case 324:
                    case 325:
                    case 326:
                    case 327:
                    case 328:
                    case 329:
                    case 330:
                    case 331:
                    case 332:
                    case 333:
                    case 334:
                    case 335:
                    case 336:
                    case 337:
                    case 338:
                    case 339:
                        return "https://i.imgur.com/HgQHMoS.png";
                    case 345://Polar Sea areas
                    case 346:
                    case 347:
                    case 348:
                    case 349:
                    case 350:
                    case 351:
                    case 352:
                    case 353:
                    case 354:
                    case 355:
                    case 356:
                    case 357:
                    case 358:
                        return "https://i.imgur.com/PWsrh4z.png";
                    case 361://Flower Field areas
                    case 362:
                    case 363:
                    case 364:
                    case 365:
                    case 366:
                    case 367:
                    case 368:
                    case 369:
                    case 370:
                    case 371:
                    case 372:
                        return "https://i.imgur.com/1DMbrp8.png";
                    case 390://TODO test
                    case 391://Tower / Tenrou (Sky Corridor) areas
                    case 392:
                    case 393:
                    case 394:
                    case 399://dure doorway
                    case 414://dure door
                    case 415:
                    case 416:
                        return "";
                    case 400://White Lake areas
                    case 401:
                    case 402:
                    case 403:
                    case 404:
                    case 405:
                    case 406:
                    case 407:
                    case 408:
                    case 409:
                    case 410:
                    case 411:
                    case 412:
                    case 413:
                        return "https://i.imgur.com/l230hKf.png";
                    case 423://Painted Falls areas
                    case 424:
                    case 425:
                    case 426:
                    case 427:
                    case 428:
                    case 429:
                    case 430:
                    case 431:
                    case 432:
                    case 433:
                    case 434:
                    case 435:
                    case 436:
                        return "https://i.imgur.com/cpPaDUz.png";
                    case 458:
                    case 459://Hunter's Road Base Camp
                        return "https://i.imgur.com/EUqIP5W.png";
                    case 288://Gorge areas
                    case 289:
                    case 290:
                    case 291:
                    case 292:
                    case 293:
                    case 294:
                    case 295:
                    case 296:
                    case 297:
                    case 298:
                    case 299:
                    case 300:
                    case 301:
                        return "https://i.imgur.com/9t3vpkV.png";




                    case 200://Mezeporta
                    case 397://Mezeporta dupe non-HD
                        return "https://i.imgur.com/60XYso8.png"; //cattleya
                    case 173://My Houses
                    case 175:
                        return "";
                    case 201://Hairdresser
                        return "";
                    case 202: //Guild Halls
                    case 203:
                    case 204:
                        return "";
                    case 205://Pugi Farm
                        return "";
                    case 210://Private Bar
                    case 211://Rasta Bar
                        return "";
                    case 256://Caravan Areas
                    case 260:
                    case 261:
                    case 262:
                    case 263:
                        return "";
                    case 257://Blacksmith
                        return "";
                    case 264://Gallery
                        return "";
                    case 265://Guuku Farm
                        return "";
                    case 283://Halk Area
                        return "";
                    case 286://PvP Room
                        return "";
                    case 340: //SR Rooms
                    case 341:
                        return "";
                    case 379://Diva Halls
                    case 445:
                        return "";
                    //case road?
                    case 462: //MezFes areas
                    case 463:
                    case 464:
                    case 465:
                    case 466:
                    case 467:
                    case 468:
                    case 469:
                        return "";
                    default:
                        return "";//cattleya
                }
            }
        }



        #endregion

        #region gear stats


        /// <summary>
        /// Gets the weapon class
        /// </summary>
        public string GetWeaponClass()
        {
            
            if (CurrentWeaponName == "Light Bowgun" || CurrentWeaponName == "Heavy Bowgun" || CurrentWeaponName == "Bow")
                return "Gunner";
            else
                return "Blademaster";
        }

        /// <summary>
        /// Gets the text format
        /// </summary>
        public string GetTextFormat()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.TextFormatExport != null)
                return s.TextFormatExport;
            else
                return "None";
        }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string GetGender()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.GenderExport != null)
                return s.GenderExport;
            else
                return "Male";
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
                string className = GetWeaponClass();
                string lv = GetGRWeaponLevel(GRWeaponLv());

                if (GetTextFormat() == "Markdown")
                {
                    if (lv == " Lv. 50" || lv == " Lv. 100")
                        lv = string.Format("**{0}**",lv);
                }

                var style = WeaponStyle() switch
                {
                    0 => "Earth Style",
                    1 => "Heaven Style",
                    2 => "Storm Style",
                    3 => "Extreme Style",
                    _ => "Earth Style"
                };

                if (className == "Blademaster")
                {
                    Dictionary.MeleeWeapons.MeleeWeaponIDs.TryGetValue(MeleeWeaponID(), out string? wepname);
                    //string address = Convert.ToString(MeleeWeaponID(), 16).ToUpper();
                    string address = MeleeWeaponID().ToString("X4").ToUpper();  // gives you hex 4 digit "007B"

                    return string.Format("{0}{1} ({2}) | {3} | {4} | {5} | {6}", wepname, lv, address, style, GetDecoName(WeaponDeco1ID(),1), GetDecoName(WeaponDeco2ID(),2), GetDecoName(WeaponDeco3ID(),3));

                }
                else if (className == "Gunner")
                {
                    Dictionary.RangedWeapons.RangedWeaponIDs.TryGetValue(RangedWeaponID(), out string? wepname);
                    //string address = Convert.ToString(MeleeWeaponID(), 16).ToUpper();
                    string address = RangedWeaponID().ToString("X4").ToUpper();  // gives you hex 4 digit "007B"
                    return string.Format("{0}{1} ({2}) | {3} | {4} | {5} | {6}", wepname, lv, address, style, GetDecoName(WeaponDeco1ID(),1), GetDecoName(WeaponDeco2ID(),2), GetDecoName(WeaponDeco3ID(),3));
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
                int Item1 = AmmoPouchItem1ID();
                int Item2 = AmmoPouchItem2ID();
                int Item3 = AmmoPouchItem3ID();
                int Item4 = AmmoPouchItem4ID();
                int Item5 = AmmoPouchItem5ID();
                int Item6 = AmmoPouchItem6ID();
                int Item7 = AmmoPouchItem7ID();
                int Item8 = AmmoPouchItem8ID();
                int Item9 = AmmoPouchItem9ID();
                int Item10 = AmmoPouchItem10ID();

                Dictionary.Items.ItemIDs.TryGetValue(Item1, out string? ItemName1);
                Dictionary.Items.ItemIDs.TryGetValue(Item2, out string? ItemName2);
                Dictionary.Items.ItemIDs.TryGetValue(Item3, out string? ItemName3);
                Dictionary.Items.ItemIDs.TryGetValue(Item4, out string? ItemName4);
                Dictionary.Items.ItemIDs.TryGetValue(Item5, out string? ItemName5);
                Dictionary.Items.ItemIDs.TryGetValue(Item6, out string? ItemName6);
                Dictionary.Items.ItemIDs.TryGetValue(Item7, out string? ItemName7);
                Dictionary.Items.ItemIDs.TryGetValue(Item8, out string? ItemName8);
                Dictionary.Items.ItemIDs.TryGetValue(Item9, out string? ItemName9);
                Dictionary.Items.ItemIDs.TryGetValue(Item10, out string? ItemName10);

                //TODO: refactor. also the values have to be skipped if item slot is empty
                if (ItemName1 == null || ItemName1 == "None" || ItemName1 == "" || AmmoPouchItem1Qty() == 0)
                    ItemName1 = "Empty, ";
                else if (ItemName2 == null || ItemName2 == "None" || ItemName2 == "" || AmmoPouchItem2Qty() == 0)
                    ItemName1 += ", ";
                else
                    ItemName1 += ", ";

                if (ItemName2 == null || ItemName2 == "None" || ItemName2 == "" || AmmoPouchItem2Qty() == 0)
                    ItemName2 = "Empty, ";
                else if (ItemName3 == null || ItemName3 == "None" || ItemName3 == "" || AmmoPouchItem3Qty() == 0)
                    ItemName2 += ", ";
                else
                    ItemName2 += ", ";

                if (ItemName3 == null || ItemName3 == "None" || ItemName3 == "" || AmmoPouchItem3Qty() == 0)
                    ItemName3 = "Empty, ";
                else if (ItemName4 == null || ItemName4 == "None" || ItemName4 == "" || AmmoPouchItem4Qty() == 0)
                    ItemName3 += ", ";
                else
                    ItemName3 += ", ";

                if (ItemName4 == null || ItemName4 == "None" || ItemName4 == "" || AmmoPouchItem4Qty() == 0)
                    ItemName4 = "Empty, ";
                else if (ItemName5 == null || ItemName5 == "None" || ItemName5 == "" || AmmoPouchItem5Qty() == 0)
                    ItemName4 += ", ";
                else
                    ItemName4 += ", ";

                if (ItemName5 == null || ItemName5 == "None" || ItemName5 == "" || AmmoPouchItem5Qty() == 0)
                    ItemName5 = "Empty, \n";
                else if (ItemName6 == null || ItemName6 == "None" || ItemName6 == "" || AmmoPouchItem6Qty() == 0)
                    ItemName5 += "";
                else
                    ItemName5 += "\n";

                if (ItemName6 == null || ItemName6 == "None" || ItemName6 == "" || AmmoPouchItem6Qty() == 0)
                    ItemName6 = "Empty, ";
                else if (ItemName7 == null || ItemName7 == "None" || ItemName7 == "" || AmmoPouchItem7Qty() == 0)
                    ItemName6 += ", ";
                else
                    ItemName6 += ", ";

                if (ItemName7 == null || ItemName7 == "None" || ItemName7 == "" || AmmoPouchItem7Qty() == 0)
                    ItemName7 = "Empty, ";
                else if (ItemName8 == null || ItemName8 == "None" || ItemName8 == "" || AmmoPouchItem8Qty() == 0)
                    ItemName7 += ", ";
                else
                    ItemName7 += ", ";

                if (ItemName8 == null || ItemName8 == "None" || ItemName8 == "" || AmmoPouchItem8Qty() == 0)
                    ItemName8 = "Empty, ";
                else if (ItemName9 == null || ItemName9 == "None" || ItemName9 == "" || AmmoPouchItem9Qty() == 0)
                    ItemName8 += ", ";
                else
                    ItemName8 += ", ";

                if (ItemName9 == null || ItemName9 == "None" || ItemName9 == "" || AmmoPouchItem9Qty() == 0)
                    ItemName9 = "Empty, ";
                else if (ItemName10 == null || ItemName10 == "None" || ItemName10 == "" || AmmoPouchItem10Qty() == 0)
                    ItemName9 += ", ";
                else
                    ItemName9 += ", ";

                if (ItemName10 == null || ItemName10 == "None" || ItemName10 == "" || AmmoPouchItem10Qty() == 0)
                    ItemName10 = "Empty";
                //else if (ItemName6 == null || ItemName6 == "None")
                //    ItemName5 = ItemName5 + "";
                else
                    ItemName10 += "";

                return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", ItemName1, ItemName2, ItemName3, ItemName4, ItemName5, ItemName6, ItemName7, ItemName8, ItemName9, ItemName10);
            }
        }

        public string GetItemPouch
        {
            get
            {
                int Item1 = PouchItem1ID();
                int Item2 = PouchItem2ID();
                int Item3 = PouchItem3ID();
                int Item4 = PouchItem4ID();
                int Item5 = PouchItem5ID();
                int Item6 = PouchItem6ID();
                int Item7 = PouchItem7ID();
                int Item8 = PouchItem8ID();
                int Item9 = PouchItem9ID();
                int Item10 = PouchItem10ID();
                int Item11 = PouchItem11ID();
                int Item12 = PouchItem12ID();
                int Item13 = PouchItem13ID();
                int Item14 = PouchItem14ID();
                int Item15 = PouchItem15ID();
                int Item16 = PouchItem16ID();
                int Item17 = PouchItem17ID();
                int Item18 = PouchItem18ID();
                int Item19 = PouchItem19ID();
                int Item20 = PouchItem20ID();

                Dictionary.Items.ItemIDs.TryGetValue(Item1, out string? ItemName1);
                Dictionary.Items.ItemIDs.TryGetValue(Item2, out string? ItemName2);
                Dictionary.Items.ItemIDs.TryGetValue(Item3, out string? ItemName3);
                Dictionary.Items.ItemIDs.TryGetValue(Item4, out string? ItemName4);
                Dictionary.Items.ItemIDs.TryGetValue(Item5, out string? ItemName5);
                Dictionary.Items.ItemIDs.TryGetValue(Item6, out string? ItemName6);
                Dictionary.Items.ItemIDs.TryGetValue(Item7, out string? ItemName7);
                Dictionary.Items.ItemIDs.TryGetValue(Item8, out string? ItemName8);
                Dictionary.Items.ItemIDs.TryGetValue(Item9, out string? ItemName9);
                Dictionary.Items.ItemIDs.TryGetValue(Item10, out string? ItemName10);
                Dictionary.Items.ItemIDs.TryGetValue(Item11, out string? ItemName11);
                Dictionary.Items.ItemIDs.TryGetValue(Item12, out string? ItemName12);
                Dictionary.Items.ItemIDs.TryGetValue(Item13, out string? ItemName13);
                Dictionary.Items.ItemIDs.TryGetValue(Item14, out string? ItemName14);
                Dictionary.Items.ItemIDs.TryGetValue(Item15, out string? ItemName15);
                Dictionary.Items.ItemIDs.TryGetValue(Item16, out string? ItemName16);
                Dictionary.Items.ItemIDs.TryGetValue(Item17, out string? ItemName17);
                Dictionary.Items.ItemIDs.TryGetValue(Item18, out string? ItemName18);
                Dictionary.Items.ItemIDs.TryGetValue(Item19, out string? ItemName19);
                Dictionary.Items.ItemIDs.TryGetValue(Item20, out string? ItemName20);

                //todo: refactor pls
                if (GetTextFormat() == "Markdown")
                {
                    if (IsMetaItem(Item1) && (ItemName1 != null || ItemName1 != "None" || ItemName1 != ""))
                        ItemName1 = string.Format("**{0}**", ItemName1);

                    if (IsMetaItem(Item2) && (ItemName2 != null || ItemName2 != "None" || ItemName2 != ""))
                        ItemName2 = string.Format("**{0}**", ItemName2);

                    if (IsMetaItem(Item3) && (ItemName3 != null || ItemName3 != "None" || ItemName3 != ""))
                        ItemName3 = string.Format("**{0}**", ItemName3);

                    if (IsMetaItem(Item4) && (ItemName4 != null || ItemName4 != "None" || ItemName4 != ""))
                        ItemName4 = string.Format("**{0}**", ItemName4);

                    if (IsMetaItem(Item5) && (ItemName5 != null || ItemName5 != "None" || ItemName5 != ""))
                        ItemName5 = string.Format("**{0}**", ItemName5);

                    if (IsMetaItem(Item6) && (ItemName6 != null || ItemName6 != "None" || ItemName6 != ""))
                        ItemName6 = string.Format("**{0}**", ItemName6);

                    if (IsMetaItem(Item7) && (ItemName7 != null || ItemName7 != "None" || ItemName7 != ""))
                        ItemName7 = string.Format("**{0}**", ItemName7);

                    if (IsMetaItem(Item8) && (ItemName8 != null || ItemName8 != "None" || ItemName8 != ""))
                        ItemName8 = string.Format("**{0}**", ItemName8);

                    if (IsMetaItem(Item9) && (ItemName9 != null || ItemName9 != "None" || ItemName9 != ""))
                        ItemName9 = string.Format("**{0}**", ItemName9);

                    if (IsMetaItem(Item10) && (ItemName10 != null || ItemName10 != "None" || ItemName10 != ""))
                        ItemName10 = string.Format("**{0}**", ItemName10);

                    if (IsMetaItem(Item11) && (ItemName11 != null || ItemName11 != "None" || ItemName11 != ""))
                        ItemName11 = string.Format("**{0}**", ItemName11);

                    if (IsMetaItem(Item12) && (ItemName12 != null || ItemName12 != "None" || ItemName12 != ""))
                        ItemName12 = string.Format("**{0}**", ItemName12);

                    if (IsMetaItem(Item13) && (ItemName13 != null || ItemName13 != "None" || ItemName13 != ""))
                        ItemName13 = string.Format("**{0}**", ItemName13);

                    if (IsMetaItem(Item14) && (ItemName14 != null || ItemName14 != "None" || ItemName14 != ""))
                        ItemName14 = string.Format("**{0}**", ItemName14);

                    if (IsMetaItem(Item15) && (ItemName15 != null || ItemName15 != "None" || ItemName15 != ""))
                        ItemName15 = string.Format("**{0}**", ItemName15);

                    if (IsMetaItem(Item16) && (ItemName16 != null || ItemName16 != "None" || ItemName16 != ""))
                        ItemName16 = string.Format("**{0}**", ItemName16);

                    if (IsMetaItem(Item17) && (ItemName17 != null || ItemName17 != "None" || ItemName17 != ""))
                        ItemName17 = string.Format("**{0}**", ItemName17);

                    if (IsMetaItem(Item18) && (ItemName18 != null || ItemName18 != "None" || ItemName18 != ""))
                        ItemName18 = string.Format("**{0}**", ItemName18);

                    if (IsMetaItem(Item19) && (ItemName19 != null || ItemName19 != "None" || ItemName19 != ""))
                        ItemName19 = string.Format("**{0}**", ItemName19);

                    if (IsMetaItem(Item20) && (ItemName20 != null || ItemName20 != "None" || ItemName20 != ""))
                        ItemName20 = string.Format("**{0}**", ItemName20);
                }

                //TODO: refactor. also the values have to be skipped if item slot is empty
                if (ItemName1 == null || ItemName1 == "None" || ItemName1 == "" || PouchItem1Qty() == 0)
                    ItemName1 = "Empty, ";
                else if (ItemName2 == null || ItemName2 == "None" || ItemName2 == "" || PouchItem2Qty() == 0)
                    ItemName1 += ", ";
                else
                    ItemName1 += ", ";

                if (ItemName2 == null || ItemName2 == "None" || ItemName2 == "" || PouchItem2Qty() == 0)
                    ItemName2 = "Empty, ";
                else if (ItemName3 == null || ItemName3 == "None" || ItemName3 == "" || PouchItem3Qty() == 0)
                    ItemName2 += ", ";
                else
                    ItemName2 += ", ";

                if (ItemName3 == null || ItemName3 == "None" || ItemName3 == "" || PouchItem3Qty() == 0)
                    ItemName3 = "Empty, ";
                else if (ItemName4 == null || ItemName4 == "None" || ItemName4 == "" || PouchItem4Qty() == 0)
                    ItemName3 += ", ";
                else
                    ItemName3 += ", ";

                if (ItemName4 == null || ItemName4 == "None" || ItemName4 == "" || PouchItem4Qty() == 0)
                    ItemName4 = "Empty, ";
                else if (ItemName5 == null || ItemName5 == "None" || ItemName5 == "" || PouchItem5Qty() == 0)
                    ItemName4 += ", ";
                else
                    ItemName4 += ", ";

                if (ItemName5 == null || ItemName5 == "None" || ItemName5 == "" || PouchItem5Qty() == 0)
                    ItemName5 = "Empty, \n";
                else if (ItemName6 == null || ItemName6 == "None" || ItemName6 == "" || PouchItem6Qty() == 0)
                    ItemName5 += "";
                else
                    ItemName5 += "\n";

                if (ItemName6 == null || ItemName6 == "None" || ItemName6 == "" || PouchItem6Qty() == 0)
                    ItemName6 = "Empty, ";
                else if (ItemName7 == null || ItemName7 == "None" || ItemName7 == "" || PouchItem7Qty() == 0)
                    ItemName6 += ", ";
                else
                    ItemName6 += ", ";

                if (ItemName7 == null || ItemName7 == "None" || ItemName7 == "" || PouchItem7Qty() == 0)
                    ItemName7 = "Empty, ";
                else if (ItemName8 == null || ItemName8 == "None" || ItemName8 == "" || PouchItem8Qty() == 0)
                    ItemName7 += ", ";
                else
                    ItemName7 += ", ";

                if (ItemName8 == null || ItemName8 == "None" || ItemName8 == "" || PouchItem8Qty() == 0)
                    ItemName8 = "Empty, ";
                else if (ItemName9 == null || ItemName9 == "None" || ItemName9 == "" || PouchItem9Qty() == 0)
                    ItemName8 += ", ";
                else
                    ItemName8 += ", ";

                if (ItemName9 == null || ItemName9 == "None" || ItemName9 == "" || PouchItem9Qty() == 0)
                    ItemName9 = "Empty, ";
                else if (ItemName10 == null || ItemName10 == "None" || ItemName10 == "" || PouchItem10Qty() == 0)
                    ItemName9 += ", ";
                else
                    ItemName9 += ", ";

                if (ItemName10 == null || ItemName10 == "None" || ItemName10 == "" || PouchItem10Qty() == 0)
                    ItemName10 = "Empty, \n";
                else if (ItemName11 == null || ItemName11 == "None" || ItemName11 == "" || PouchItem11Qty() == 0)
                    ItemName10 += "";
                else
                    ItemName10 += "\n";

                if (ItemName11 == null || ItemName11 == "None" || ItemName11 == "" || PouchItem11Qty() == 0)
                    ItemName11 = "Empty, ";
                else if (ItemName12 == null || ItemName12 == "None" || ItemName12 == "" || PouchItem12Qty() == 0)
                    ItemName11 += ", ";
                else
                    ItemName11 += ", ";

                if (ItemName12 == null || ItemName12 == "None" || ItemName12 == "" || PouchItem12Qty() == 0)
                    ItemName12 = "Empty, ";
                else if (ItemName13 == null || ItemName13 == "None" || ItemName13 == "" || PouchItem13Qty() == 0)
                    ItemName12 += ", ";
                else
                    ItemName12 += ", ";

                if (ItemName13 == null || ItemName13 == "None" || ItemName13 == "" || PouchItem13Qty() == 0)
                    ItemName13 = "Empty, ";
                else if (ItemName14 == null || ItemName14 == "None" || ItemName14 == "" || PouchItem14Qty() == 0)
                    ItemName13 += ", ";
                else
                    ItemName13 += ", ";

                if (ItemName14 == null || ItemName14 == "None" || ItemName14 == "" || PouchItem14Qty() == 0)
                    ItemName14 = "Empty, ";
                else if (ItemName15 == null || ItemName15 == "None" || ItemName15 == "" || PouchItem15Qty() == 0)
                    ItemName14 += ", ";
                else
                    ItemName14 += ", ";

                if (ItemName15 == null || ItemName15 == "None" || ItemName15 == "" || PouchItem15Qty() == 0)
                    ItemName15 = "Empty, \n";
                else if (ItemName16 == null || ItemName16 == "None" || ItemName16 == "" || PouchItem16Qty() == 0)
                    ItemName15 += "";
                else
                    ItemName15 += "\n";

                if (ItemName16 == null || ItemName16 == "None" || ItemName16 == "" || PouchItem16Qty() == 0)
                    ItemName16 = "Empty, ";
                else if (ItemName17 == null || ItemName17 == "None" || ItemName17 == "" || PouchItem17Qty() == 0)
                    ItemName16 += ", ";
                else
                    ItemName16 += ", ";

                if (ItemName17 == null || ItemName17 == "None" || ItemName17 == "" || PouchItem16Qty() == 0)
                    ItemName17 = "Empty, ";
                else if (ItemName18 == null || ItemName18 == "None" || ItemName18 == "" || PouchItem18Qty() == 0)
                    ItemName17 += ", ";
                else
                    ItemName17 += ", ";

                if (ItemName18 == null || ItemName18 == "None" || ItemName18 == "" || PouchItem17Qty() == 0)
                    ItemName18 = "Empty, ";
                else if (ItemName19 == null || ItemName19 == "None" || ItemName19 == "" || PouchItem19Qty() == 0)
                    ItemName18 += "";
                else
                    ItemName18 += ", ";

                if (ItemName19 == null || ItemName19 == "None" || ItemName19 == "" || PouchItem18Qty() == 0)
                    ItemName19 = "Empty, ";
                else if (ItemName20 == null || ItemName20 == "None" || ItemName20 == "" || PouchItem20Qty() == 0)
                    ItemName19 += ", ";
                else
                    ItemName19 += ", ";

                if (ItemName20 == null || ItemName20 == "None" || ItemName20 == "" || PouchItem20Qty() == 0)
                    ItemName20 = "Empty";
                //else if (ItemName6 == null || ItemName6 == "None")
                //    ItemName5 = ItemName5 + "";
                else
                    ItemName20 += "";

                return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}", ItemName1, ItemName2, ItemName3, ItemName4, ItemName5, ItemName6, ItemName7, ItemName8, ItemName9, ItemName10, ItemName11, ItemName12, ItemName13, ItemName14, ItemName15, ItemName16, ItemName17, ItemName18, ItemName19, ItemName20);
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
                Dictionary.ArmorHeads.ArmorHeadIDs.TryGetValue(ArmorHeadID(), out string? piecename);

                if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename) )
                    piecename = string.Format("**{0}**", piecename);

                //string address = Convert.ToString(ArmorHeadID(), 16).ToUpper();
                string address = ArmorHeadID().ToString("X4").ToUpper();
                return string.Format("{0} ({1}) | {2} | {3} | {4}", piecename, address, GetDecoName(ArmorHeadDeco1ID()), GetDecoName(ArmorHeadDeco2ID()), GetDecoName(ArmorHeadDeco3ID()));
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
                Dictionary.ArmorChests.ArmorChestIDs.TryGetValue(ArmorChestID(), out string? piecename);

                if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
                    piecename = string.Format("**{0}**", piecename);

                //string address = Convert.ToString(ArmorChestID(), 16).ToUpper();
                string address = ArmorChestID().ToString("X4").ToUpper();
                return string.Format("{0} ({1}) | {2} | {3} | {4}", piecename, address, GetDecoName(ArmorChestDeco1ID()), GetDecoName(ArmorChestDeco2ID()), GetDecoName(ArmorChestDeco3ID()));
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
                Dictionary.ArmorArms.ArmorArmIDs.TryGetValue(ArmorArmsID(), out string? piecename);

                if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
                    piecename = string.Format("**{0}**", piecename);

                //string address = Convert.ToString(ArmorArmsID(), 16).ToUpper();
                string address = ArmorArmsID().ToString("X4").ToUpper();
                return string.Format("{0} ({1}) | {2} | {3} | {4}", piecename, address, GetDecoName(ArmorArmsDeco1ID()), GetDecoName(ArmorArmsDeco2ID()), GetDecoName(ArmorArmsDeco3ID()));
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
                Dictionary.ArmorWaists.ArmorWaistIDs.TryGetValue(ArmorWaistID(), out string? piecename);

                if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
                    piecename = string.Format("**{0}**", piecename);

                //string address = Convert.ToString(ArmorWaistID(), 16).ToUpper();
                string address = ArmorWaistID().ToString("X4").ToUpper();
                return string.Format("{0} ({1}) | {2} | {3} | {4}", piecename, address, GetDecoName(ArmorWaistDeco1ID()), GetDecoName(ArmorWaistDeco2ID()), GetDecoName(ArmorWaistDeco3ID()));
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
                Dictionary.ArmorLegs.ArmorLegIDs.TryGetValue(ArmorLegsID(), out string? piecename);

                if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
                    piecename = string.Format("**{0}**", piecename);

                //string address = Convert.ToString(ArmorLegsID(), 16).ToUpper();
                string address = ArmorLegsID().ToString("X4").ToUpper();
                return string.Format("{0} ({1}) | {2} | {3} | {4}", piecename, address, GetDecoName(ArmorLegsDeco1ID()), GetDecoName(ArmorLegsDeco2ID()), GetDecoName(ArmorLegsDeco3ID()));
            }
        }

        /// <summary>
        /// Gets the decos.
        /// </summary>
        /// <value>
        /// The decos.
        /// </value>
        public string GetDecoName(int id, int slot = 0)
        {
            Dictionary.Items.ItemIDs.TryGetValue(id, out string? DecoName);

            //todo: refactor pls
            if (GetTextFormat() == "Markdown")
            {
                if (IsMetaItem(id) && (DecoName != null || DecoName != "None" || DecoName != ""))
                    DecoName = string.Format("**{0}**", DecoName);
            }

            if (DecoName == null || DecoName == "None" || DecoName == "")
                DecoName = "Empty";
            //else if (ItemName6 == null || ItemName6 == "None")
            //    ItemName5 = ItemName5 + "";
            else
                DecoName += "";

            if (DecoName == "Empty" && slot != 0)
            {
                return GetSigilName(slot);
            }

            //Dictionary.ArmorLegs.ArmorLegIDs.TryGetValue(ArmorLegsID(), out string? piecename);

            //if (GetTextFormat() == "Markdown" && piecename != null && IsMetaGear(piecename))
            //    piecename = string.Format("**{0}**", piecename);

            //string address = Convert.ToString(ArmorLegsID(), 16).ToUpper();
            string address = id.ToString("X4").ToUpper();
            return string.Format("{0} ({1})", DecoName, address);
        }   

        /// <summary>
        /// Gets the sigils.
        /// </summary>
        /// <value>
        /// The sigils.
        /// </value>
        public string GetSigilName(int slot)
        {
            Dictionary.SigilSkillList.SigilSkillID.TryGetValue(Sigil1Name1(), out string? Sigil1Type1);
            Dictionary.SigilSkillList.SigilSkillID.TryGetValue(Sigil1Name2(), out string? Sigil1Type2);
            Dictionary.SigilSkillList.SigilSkillID.TryGetValue(Sigil1Name3(), out string? Sigil1Type3);
            Dictionary.SigilSkillList.SigilSkillID.TryGetValue(Sigil2Name1(), out string? Sigil2Type1);
            Dictionary.SigilSkillList.SigilSkillID.TryGetValue(Sigil2Name2(), out string? Sigil2Type2);
            Dictionary.SigilSkillList.SigilSkillID.TryGetValue(Sigil2Name3(), out string? Sigil2Type3);
            Dictionary.SigilSkillList.SigilSkillID.TryGetValue(Sigil3Name1(), out string? Sigil3Type1);
            Dictionary.SigilSkillList.SigilSkillID.TryGetValue(Sigil3Name2(), out string? Sigil3Type2);
            Dictionary.SigilSkillList.SigilSkillID.TryGetValue(Sigil3Name3(), out string? Sigil3Type3);

            string value1;
            string value2;
            string value3;
            string type1;
            string type2;
            string type3;

            string Sigil1Type1Value = Sigil1Value1().ToString();
            string Sigil1Type2Value = Sigil1Value2().ToString();
            string Sigil1Type3Value = Sigil1Value3().ToString();
            string Sigil2Type1Value = Sigil2Value1().ToString();
            string Sigil2Type2Value = Sigil2Value2().ToString();
            string Sigil2Type3Value = Sigil2Value3().ToString();
            string Sigil3Type1Value = Sigil3Value1().ToString();
            string Sigil3Type2Value = Sigil3Value2().ToString();
            string Sigil3Type3Value = Sigil3Value3().ToString();

            switch (slot)
            {
                default:

                    return "Empty";

                case 1:

                    if (Sigil1Type1Value == "0" || Sigil1Name1() == 0)
                        return "Empty";
                    else
                    {
                        type1 = Sigil1Type1 + ": ";

                        if (int.Parse(Sigil1Type1Value) > 127)
                            value1 = (int.Parse(Sigil1Type1Value) - 256).ToString() + ", ";
                        else
                            value1 = "+" + Sigil1Type1Value + ", ";

                    }

                    if (Sigil1Type2Value == "0" || Sigil1Name2() == 0)
                    {
                        value2 = "";
                        type2 = "Empty, ";
                    }
                    else
                    {

                        if (int.Parse(Sigil1Type2Value) > 127)
                            value2 = (int.Parse(Sigil1Type2Value) - 256).ToString() + ", ";
                        else
                            value2 = "+" + Sigil1Type2Value + ", ";

                        type2 = Sigil1Type2 + ": ";
                    }

                    if (Sigil1Type3Value == "0" || Sigil1Name3() == 0)
                    {
                        value3 = "";
                        type3 = "Empty";
                    }
                    else
                    {

                        if (int.Parse(Sigil1Type3Value) > 127)
                            value3 = (int.Parse(Sigil1Type3Value) - 256).ToString() + ", ";
                        else
                            value3 = "+" + Sigil1Type3Value + ", ";

                        type3 = Sigil1Type3 + ": ";
                    }
                    
                    break;

                case 2:

                    if (Sigil2Type1Value == "0" || Sigil2Name1() == 0)
                        return "Empty";
                    else
                    {
                        type1 = Sigil2Type1 + ": ";

                        if (int.Parse(Sigil2Type1Value) > 127)
                            value1 = (int.Parse(Sigil2Type1Value) - 256).ToString() + ", ";
                        else
                            value1 = "+" + Sigil2Type1Value + ", ";

                    }

                    if (Sigil2Type2Value == "0" || Sigil2Name2() == 0)
                    {
                        value2 = "";
                        type2 = "None, ";
                    }
                    else
                    {

                        if (int.Parse(Sigil2Type2Value) > 127)
                            value2 = (int.Parse(Sigil2Type2Value) - 256).ToString() + ", ";
                        else
                            value2 = "+" + Sigil2Type2Value + ", ";

                        type2 = Sigil2Type2 + ": ";
                    }

                    if (Sigil2Type3Value == "0" || Sigil2Name3() == 0)
                    {
                        value3 = "";
                        type3 = "None";
                    }
                    else
                    {

                        if (int.Parse(Sigil2Type3Value) > 127)
                            value3 = (int.Parse(Sigil2Type3Value) - 256).ToString() + ", ";
                        else
                            value3 = "+" + Sigil2Type3Value + ", ";

                        type3 = Sigil2Type3 + ": ";
                    }

                    break;

                case 3:

                    if (Sigil3Type1Value == "0" || Sigil3Name1() == 0)
                        return "Empty";
                    else
                    {
                        type1 = Sigil3Type1 + ": ";

                        if (int.Parse(Sigil3Type1Value) > 127)
                            value1 = (int.Parse(Sigil3Type1Value) - 256).ToString() + ", ";
                        else
                            value1 = "+" + Sigil3Type1Value + ", ";

                    }

                    if (Sigil3Type2Value == "0" || Sigil3Name2() == 0)
                    {
                        value2 = "";
                        type2 = "None, ";
                    }
                    else
                    {

                        if (int.Parse(Sigil3Type2Value) > 127)
                            value2 = (int.Parse(Sigil3Type2Value) - 256).ToString() + ", ";
                        else
                            value2 = "+" + Sigil3Type2Value + ", ";

                        type2 = Sigil3Type2 + ": ";
                    }

                    if (Sigil3Type3Value == "0" || Sigil3Name3() == 0)
                    {
                        value3 = "";
                        type3 = "None";
                    }
                    else
                    {

                        if (int.Parse(Sigil3Type3Value) > 127)
                            value3 = (int.Parse(Sigil3Type3Value) - 256).ToString() + ", ";
                        else
                            value3 = "+" + Sigil3Type3Value + ", ";

                        type3 = Sigil3Type3 + ": ";
                    }

                    break;
            }

            return string.Format("{0}{1}{2}{3}{4}{5}", type1, value1, type2, value2, type3, value3);
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
                Dictionary.Items.ItemIDs.TryGetValue(Cuff1ID(), out string? cuffname);
                //string address = Convert.ToString(Cuff1ID(), 16).ToUpper();
                string address = Cuff1ID().ToString("X4").ToUpper();
                return string.Format("{0} ({1})", cuffname, address);
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
                Dictionary.Items.ItemIDs.TryGetValue(Cuff2ID(), out string? cuffname);
                //string address = Convert.ToString(Cuff2ID(), 16).ToUpper();
                string address = Cuff2ID().ToString("X4").ToUpper();
                return string.Format("{0} ({1})", cuffname, address);
            }
        }

        public string GetCuffs
        {
            get
            {
                string cuff1 = GetCuff1Name;
                string cuff2 = GetCuff2Name;

                if (IsMetaItem(Cuff1ID()))
                    cuff1 = string.Format("**{0}**",cuff1);

                if (IsMetaItem(Cuff2ID()))
                    cuff2 = string.Format("**{0}**", cuff2);

                return string.Format("{0} | {1}", cuff1, cuff2);
            }
        }

        /// <summary>
        /// Determines whether [is maximum caravan skill] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is maximum caravan skill] [the specified identifier]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMaxCaravanSkill(int id)
        {
            
            switch (id)
            {
                default:
                    return false;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 12:
                case 15:
                case 16:
                case 21:
                case 24:
                case 27:
                case 39:
                case 42:
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                case 83:
                case 86:
                case 89:
                case 92:
                case 96:
                case 100:
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                    return true;
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
                int id1 = CaravanSkill1();
                int id2 = CaravanSkill2();
                int id3 = CaravanSkill3();

                Dictionary.CaravanSkillList.CaravanSkillID.TryGetValue(id1, out string? caravanSkillName1);
                Dictionary.CaravanSkillList.CaravanSkillID.TryGetValue(id2, out string? caravanSkillName2);
                Dictionary.CaravanSkillList.CaravanSkillID.TryGetValue(id3, out string? caravanSkillName3);

                if (GetTextFormat() == "Markdown")
                {
                    if (IsMaxCaravanSkill(id1))
                        caravanSkillName1 = String.Format("**{0}**", caravanSkillName1);

                    if (IsMaxCaravanSkill(id2))
                        caravanSkillName2 = String.Format("**{0}**", caravanSkillName2);

                    if (IsMaxCaravanSkill(id3))
                        caravanSkillName3 = String.Format("**{0}**", caravanSkillName3);
                }   

                if (caravanSkillName1 == "" || caravanSkillName1 == "None")
                    return "None";
                else if (caravanSkillName2 == "" || caravanSkillName2 == "None")
                    return caravanSkillName1 + "";
                else if (caravanSkillName3 == "" || caravanSkillName3 == "None")
                    return caravanSkillName1 + ", " + caravanSkillName2;
                else
                    return caravanSkillName1 + ", " + caravanSkillName2 + ", " + caravanSkillName3;
            }
        }

        /// <summary>
        /// Determines whether [is maximum zenith skill].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is maximum zenith skill] [the specified identifier]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMaxZenithSkill(int id)
        {
            switch (id)
            {
                default:
                    return false;
                case 7:
                case 9:
                case 11:
                case 12:
                case 14:
                case 15:
                case 19:
                case 23:
                case 25:
                case 27:
                case 29:
                case 31:
                case 32:
                case 34:
                case 35:
                case 37:
                case 39:
                case 41:
                case 43:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                    return true;
            }
        }

        /// <summary>
        /// Determines whether [is maximum skill level].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is maximum skill level] [the specified identifier]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMaxSkillLevel(int id)
        {
            switch (id)
            {
                default: 
                    return false;
                case 2:
                case 5:
                case 8:
                case 11:
                case 13:
                case 16:
                case 19:
                //case 23:
                case 28:
                case 33:
                case 38:
                case 41:
                case 44:
                case 45:
                case 48:
                case 51:
                case 53:
                case 54:
                case 55:
                case 56:
                case 58:
                case 61:
                case 64:
                case 67:
                case 70:
                case 71:
                case 72:
                case 74:
                case 78:
                //case 81:
                //case 84:
                case 89:
                case 94:
                case 97:
                case 100:
                case 106:
                case 112:
                case 118:
                case 124:
                case 130:
                case 135:
                case 140:
                case 147:
                case 150:
                case 153:
                case 155:
                case 159:
                case 162:
                case 164:
                case 165:
                case 169:
                case 172:
                case 173:
                case 178:
                case 180:
                case 182:
                case 184:
                case 186:
                case 189:
                case 190:
                case 192:
                case 194:
                case 199:
                case 201:
                case 203://Gunnery?
                case 212:
                case 218:
                //case 219:
                case 222:
                case 223:
                case 228:
                case 232:
                case 235:
                case 238:
                case 240:
                case 242:
                case 244:
                case 246:
                case 248:
                case 250:
                case 252:
                case 256:
                case 259:
                case 262:
                case 265:
                case 268:
                case 271:
                case 274:
                case 277:
                case 280:
                case 283:
                case 285:
                case 287:
                case 288:
                case 289:
                case 290:
                case 292:
                case 293:
                case 295:
                case 296:
                case 297:
                case 299:
                case 300:
                case 301:
                case 302:
                case 305:
                case 309:
                case 313:
                case 317:
                case 321:
                case 325:
                case 329:
                case 333:
                case 337:
                case 341:
                case 345:
                case 349:
                case 350:
                case 352:
                case 353:
                case 354:
                case 356:
                case 359:
                case 360:
                case 362:
                case 365:
                case 366:
                case 368:
                case 384:
                case 388:
                case 390:
                case 392:
                case 393:
                case 394:
                case 395:
                case 396:
                case 397:
                case 398:
                case 401:
                case 404:
                case 407:
                case 414:
                case 417:
                case 420:
                case 423:
                case 425:
                case 426:
                case 431:
                case 432:
                case 437:
                case 438:
                case 443:
                case 446:
                case 449:
                case 452:
                case 453:
                case 456:
                case 457:
                case 458://Red Soul
                case 461:
                case 463:
                case 464:
                case 465:
                case 466:
                case 471:
                case 473:
                case 474:
                case 475:
                case 476:
                case 477:
                case 480:
                case 481:
                case 482:
                case 485:
                case 486:
                case 488:
                case 489:
                case 491:
                case 494:
                case 495:
                case 497:
                case 498:
                case 499:
                case 501:
                case 502:
                case 503:
                case 504:
                case 505:
                case 506:
                case 512:
                case 513:
                case 514:
                case 515:
                case 516:
                case 517:
                //case 518://dupe?
                case 519:
                case 520:
                case 521:
                case 522:
                case 523:
                case 524:
                case 525:
                case 526:
                case 528:
                case 529:
                case 530:
                    return true;
            }
        }

        /// <summary>
        /// Determines whether [is maximum road dure skill level] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="level">The level.</param>
        /// <returns>
        ///   <c>true</c> if [is maximum road dure skill level] [the specified identifier]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMaxRoadDureSkillLevel(int id, string level)
        {
            if (level == "0")
                return false;

            switch (id)
            {
                default:
                    return false;
                case 1:
                case 2:
                case 19:
                case 20:
                    if (level == "5")
                        return true;
                    else return false;
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 34:
                case 29:
                case 31:
                case 14:
                case 15:
                case 18:
                case 22:
                    if (level == "3")
                        return true;
                    else return false;
                case 5:
                case 28:
                case 33:
                case 32:
                    if (level == "2")
                        return true;
                    else return false;
                case 35:
                case 30:
                    if (level == "1")
                        return true;
                    else return false;
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
                Dictionary.ZenithSkillList.ZenithSkillID.TryGetValue(ZenithSkill1(), out string? SkillName1);
                Dictionary.ZenithSkillList.ZenithSkillID.TryGetValue(ZenithSkill2(), out string? SkillName2);
                Dictionary.ZenithSkillList.ZenithSkillID.TryGetValue(ZenithSkill3(), out string? SkillName3);
                Dictionary.ZenithSkillList.ZenithSkillID.TryGetValue(ZenithSkill4(), out string? SkillName4);
                Dictionary.ZenithSkillList.ZenithSkillID.TryGetValue(ZenithSkill5(), out string? SkillName5);
                Dictionary.ZenithSkillList.ZenithSkillID.TryGetValue(ZenithSkill6(), out string? SkillName6);
                Dictionary.ZenithSkillList.ZenithSkillID.TryGetValue(ZenithSkill7(), out string? SkillName7);
                
                //todo: refactor pls
                if (GetTextFormat() == "Markdown")
                {
                    if (IsMaxZenithSkill(ZenithSkill1()) && (SkillName1 != null || SkillName1 != "None" || SkillName1 != ""))
                        SkillName1 = string.Format("**{0}**", SkillName1);

                    if (IsMaxZenithSkill(ZenithSkill2()) && (SkillName2 != null || SkillName2 != "None" || SkillName2 != ""))
                        SkillName2 = string.Format("**{0}**", SkillName2);

                    if (IsMaxZenithSkill(ZenithSkill3()) && (SkillName3 != null || SkillName3 != "None" || SkillName3 != ""))
                        SkillName3 = string.Format("**{0}**", SkillName3);

                    if (IsMaxZenithSkill(ZenithSkill4()) && (SkillName4 != null || SkillName4 != "None" || SkillName4 != ""))
                        SkillName4 = string.Format("**{0}**", SkillName4);

                    if (IsMaxZenithSkill(ZenithSkill5()) && (SkillName5 != null || SkillName5 != "None" || SkillName5 != ""))
                        SkillName5 = string.Format("**{0}**", SkillName5);

                    if (IsMaxZenithSkill(ZenithSkill6()) && (SkillName6 != null || SkillName6 != "None" || SkillName6 != ""))
                        SkillName6 = string.Format("**{0}**", SkillName6);

                    if (IsMaxZenithSkill(ZenithSkill7()) && (SkillName7 != null || SkillName7 != "None" || SkillName7 != ""))
                        SkillName7 = string.Format("**{0}**", SkillName7);
                }

                //TODO: refactor
                if (SkillName1 == null || SkillName1 == "None" || SkillName1 == "")
                    SkillName1 = "";
                else if (SkillName2 == null || SkillName2 == "None" || SkillName2 == "")
                    SkillName1 += "";
                else
                    SkillName1 += ", ";

                if (SkillName2 == null || SkillName2 == "None" || SkillName2 == "")
                    SkillName2 = "";
                else if (SkillName3 == null || SkillName3 == "None" || SkillName3 == "")
                    SkillName2 += "";
                else
                    SkillName2 += ", ";

                if (SkillName3 == null || SkillName3 == "None" || SkillName3 == "")
                    SkillName3 = "";
                else if (SkillName4 == null || SkillName4 == "None" || SkillName4 == "")
                    SkillName3 += "";
                else
                    SkillName3 += ", ";

                if (SkillName4 == null || SkillName4 == "None" || SkillName4 == "")
                    SkillName4 = "";
                else if (SkillName5 == null || SkillName5 == "None" || SkillName5 == "")
                    SkillName4 += "";
                else
                    SkillName4 += ", ";

                if (SkillName5 == null || SkillName5 == "None" || SkillName5 == "")
                    SkillName5 = "";
                else if (SkillName6 == null || SkillName6 == "None" || SkillName6 == "")
                    SkillName5 += "";
                else
                    SkillName5 += "\n";

                if (SkillName6 == null || SkillName6 == "None" || SkillName6 == "")
                    SkillName6 = "";
                else if (SkillName7 == null || SkillName7 == "None" || SkillName7 == "")
                    SkillName6 += "";
                else
                    SkillName6 += ", ";

                if (SkillName7 == null || SkillName7 == "None" || SkillName7 == "")
                    SkillName7 = "";
                //else if (SkillName6 == null || SkillName6 == "None")
                //    SkillName5 = SkillName5 + "";
                else
                    SkillName7 += "";

                if (SkillName1 == "")
                    SkillName1 = "None";

                return string.Format("{0}{1}{2}{3}{4}{5}{6}", SkillName1, SkillName2, SkillName3, SkillName4, SkillName5, SkillName6, SkillName7);
            }
        }

        /// <summary>
        /// Gets the gou boost mode.
        /// </summary>
        /// <returns></returns>
        public bool GetGouBoostMode()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.GouBoostExport == true)
                return true;
            else
                return false;
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
                int Skill1;
                int Skill2;
                int Skill3;
                int Skill4;
                int Skill5;
                int Skill6;
                int Skill7;
                int Skill8;
                int Skill9;
                int Skill10;
                int Skill11;
                int Skill12;
                int Skill13;
                int Skill14;
                int Skill15;
                int Skill16;
                int Skill17;
                int Skill18;
                int Skill19;

                if (GetGouBoostMode())
                {
                    Skill1 = GetGouBoostSkill(ArmorSkill1());
                    Skill2 = GetGouBoostSkill(ArmorSkill2());
                    Skill3 = GetGouBoostSkill(ArmorSkill3());
                    Skill4 = GetGouBoostSkill(ArmorSkill4());
                    Skill5 = GetGouBoostSkill(ArmorSkill5());
                    Skill6 = GetGouBoostSkill(ArmorSkill6());
                    Skill7 = GetGouBoostSkill(ArmorSkill7());
                    Skill8 = GetGouBoostSkill(ArmorSkill8());
                    Skill9 = GetGouBoostSkill(ArmorSkill9());
                    Skill10 = GetGouBoostSkill(ArmorSkill10());
                    Skill11 = GetGouBoostSkill(ArmorSkill11());
                    Skill12 = GetGouBoostSkill(ArmorSkill12());
                    Skill13 = GetGouBoostSkill(ArmorSkill13());
                    Skill14 = GetGouBoostSkill(ArmorSkill14());
                    Skill15 = GetGouBoostSkill(ArmorSkill15());
                    Skill16 = GetGouBoostSkill(ArmorSkill16());
                    Skill17 = GetGouBoostSkill(ArmorSkill17());
                    Skill18 = GetGouBoostSkill(ArmorSkill18());
                    Skill19 = GetGouBoostSkill(ArmorSkill19());
                }
                else
                {
                    Skill1 = ArmorSkill1();
                    Skill2 = ArmorSkill2();
                    Skill3 = ArmorSkill3();
                    Skill4 = ArmorSkill4();
                    Skill5 = ArmorSkill5();
                    Skill6 = ArmorSkill6();
                    Skill7 = ArmorSkill7();
                    Skill8 = ArmorSkill8();
                    Skill9 = ArmorSkill9();
                    Skill10 = ArmorSkill10();
                    Skill11 = ArmorSkill11();
                    Skill12 = ArmorSkill12();
                    Skill13 = ArmorSkill13();
                    Skill14 = ArmorSkill14();
                    Skill15 = ArmorSkill15();
                    Skill16 = ArmorSkill16();
                    Skill17 = ArmorSkill17();
                    Skill18 = ArmorSkill18();
                    Skill19 = ArmorSkill19();
                }

                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill1, out string? SkillName1);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill2, out string? SkillName2);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill3, out string? SkillName3);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill4, out string? SkillName4);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill5, out string? SkillName5);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill6, out string? SkillName6);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill7, out string? SkillName7);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill8, out string? SkillName8);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill9, out string? SkillName9);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill10, out string? SkillName10);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill11, out string? SkillName11);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill12, out string? SkillName12);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill13, out string? SkillName13);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill14, out string? SkillName14);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill15, out string? SkillName15);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill16, out string? SkillName16);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill17, out string? SkillName17);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill18, out string? SkillName18);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(Skill19, out string? SkillName19);

                //todo: refactor pls
                if (GetTextFormat() == "Markdown")
                {
                    if (IsMaxSkillLevel(Skill1) && (SkillName1 != null || SkillName1 != "None" || SkillName1 != ""))
                        SkillName1 = string.Format("**{0}**", SkillName1);

                    if (IsMaxSkillLevel(Skill2) && (SkillName2 != null || SkillName2 != "None" || SkillName2 != ""))
                        SkillName2 = string.Format("**{0}**", SkillName2);

                    if (IsMaxSkillLevel(Skill3) && (SkillName3 != null || SkillName3 != "None" || SkillName3 != ""))
                        SkillName3 = string.Format("**{0}**", SkillName3);

                    if (IsMaxSkillLevel(Skill4) && (SkillName4 != null || SkillName4 != "None" || SkillName4 != ""))
                        SkillName4 = string.Format("**{0}**", SkillName4);

                    if (IsMaxSkillLevel(Skill5) && (SkillName5 != null || SkillName5 != "None" || SkillName5 != ""))
                        SkillName5 = string.Format("**{0}**", SkillName5);

                    if (IsMaxSkillLevel(Skill6) && (SkillName6 != null || SkillName6 != "None" || SkillName6 != ""))
                        SkillName6 = string.Format("**{0}**", SkillName6);

                    if (IsMaxSkillLevel(Skill7) && (SkillName7 != null || SkillName7 != "None" || SkillName7 != ""))
                        SkillName7 = string.Format("**{0}**", SkillName7);

                    if (IsMaxSkillLevel(Skill8) && (SkillName8 != null || SkillName8 != "None" || SkillName8 != ""))
                        SkillName8 = string.Format("**{0}**", SkillName8);

                    if (IsMaxSkillLevel(Skill9) && (SkillName9 != null || SkillName9 != "None" || SkillName9 != ""))
                        SkillName9 = string.Format("**{0}**", SkillName9);

                    if (IsMaxSkillLevel(Skill10) && (SkillName10 != null || SkillName10 != "None" || SkillName10 != ""))
                        SkillName10 = string.Format("**{0}**", SkillName10);

                    if (IsMaxSkillLevel(Skill11) && (SkillName11 != null || SkillName11 != "None" || SkillName11 != ""))
                        SkillName11 = string.Format("**{0}**", SkillName11);

                    if (IsMaxSkillLevel(Skill12) && (SkillName12 != null || SkillName12 != "None" || SkillName12 != ""))
                        SkillName12 = string.Format("**{0}**", SkillName12);

                    if (IsMaxSkillLevel(Skill13) && (SkillName13 != null || SkillName13 != "None" || SkillName13 != ""))
                        SkillName13 = string.Format("**{0}**", SkillName13);

                    if (IsMaxSkillLevel(Skill14) && (SkillName14 != null || SkillName14 != "None" || SkillName14 != ""))
                        SkillName14 = string.Format("**{0}**", SkillName14);

                    if (IsMaxSkillLevel(Skill15) && (SkillName15 != null || SkillName15 != "None" || SkillName15 != ""))
                        SkillName15 = string.Format("**{0}**", SkillName15);

                    if (IsMaxSkillLevel(Skill16) && (SkillName16 != null || SkillName16 != "None" || SkillName16 != ""))
                        SkillName16 = string.Format("**{0}**", SkillName16);

                    if (IsMaxSkillLevel(Skill17) && (SkillName17 != null || SkillName17 != "None" || SkillName17 != ""))
                        SkillName17 = string.Format("**{0}**", SkillName17);

                    if (IsMaxSkillLevel(Skill18) && (SkillName18 != null || SkillName18 != "None" || SkillName18 != ""))
                        SkillName18 = string.Format("**{0}**", SkillName18);

                    if (IsMaxSkillLevel(Skill19) && (SkillName19 != null || SkillName19 != "None" || SkillName19 != ""))
                        SkillName19 = string.Format("**{0}**", SkillName19);
                }

                //TODO: refactor
                if (SkillName1 == null || SkillName1 == "None" || SkillName1 == "")
                    SkillName1 = "";
                else if (SkillName2 == null || SkillName2 == "None" || SkillName2 == "")
                    SkillName1 += "";
                else
                    SkillName1 += ", ";

                if (SkillName2 == null || SkillName2 == "None" || SkillName2 == "")
                    SkillName2 = "";
                else if (SkillName3 == null || SkillName3 == "None" || SkillName3 == "")
                    SkillName2 += "";
                else
                    SkillName2 += ", ";

                if (SkillName3 == null || SkillName3 == "None" || SkillName3 == "")
                    SkillName3 = "";
                else if (SkillName4 == null || SkillName4 == "None" || SkillName4 == "")
                    SkillName3 += "";
                else
                    SkillName3 += ", ";

                if (SkillName4 == null || SkillName4 == "None" || SkillName4 == "")
                    SkillName4 = "";
                else if (SkillName5 == null || SkillName5 == "None" || SkillName5 == "")
                    SkillName4 += "";
                else
                    SkillName4 += ", ";

                if (SkillName5 == null || SkillName5 == "None" || SkillName5 == "")
                    SkillName5 = "";
                else if (SkillName6 == null || SkillName6 == "None" || SkillName6 == "")
                    SkillName5 += "";
                else
                    SkillName5 += "\n";

                if (SkillName6 == null || SkillName6 == "None" || SkillName6 == "")
                    SkillName6 = "";
                else if (SkillName7 == null || SkillName7 == "None" || SkillName7 == "")
                    SkillName6 += "";
                else
                    SkillName6 += ", ";

                if (SkillName7 == null || SkillName7 == "None" || SkillName7 == "")
                    SkillName7 = "";
                else if (SkillName8 == null || SkillName8 == "None" || SkillName8 == "")
                    SkillName7 += "";
                else
                    SkillName7 += ", ";

                if (SkillName8 == null || SkillName8 == "None" || SkillName8 == "")
                    SkillName8 = "";
                else if (SkillName9 == null || SkillName9 == "None" || SkillName9 == "")
                    SkillName8 += "";
                else
                    SkillName8 += ", ";

                if (SkillName9 == null || SkillName9 == "None" || SkillName9 == "")
                    SkillName9 = "";
                else if (SkillName10 == null || SkillName10 == "None" || SkillName10 == "")
                    SkillName9 += "";
                else
                    SkillName9 += ", ";

                if (SkillName10 == null || SkillName10 == "None" || SkillName10 == "")
                    SkillName10 = "";
                else if (SkillName11 == null || SkillName11 == "None" || SkillName11 == "")
                    SkillName10 += "";
                else
                    SkillName10 += "\n";

                if (SkillName11 == null || SkillName11 == "None" || SkillName11 == "")
                    SkillName11 = "";
                else if (SkillName12 == null || SkillName12 == "None" || SkillName12 == "")
                    SkillName11 += "";
                else
                    SkillName11 += ", ";

                if (SkillName12 == null || SkillName12 == "None" || SkillName12 == "")
                    SkillName12 = "";
                else if (SkillName13 == null || SkillName13 == "None" || SkillName13 == "")
                    SkillName12 += "";
                else
                    SkillName12 += ", ";

                if (SkillName13 == null || SkillName13 == "None" || SkillName13 == "")
                    SkillName13 = "";
                else if (SkillName14 == null || SkillName14 == "None" || SkillName14 == "")
                    SkillName13 += "";
                else
                    SkillName13 += ", ";

                if (SkillName14 == null || SkillName14 == "None" || SkillName14 == "")
                    SkillName14 = "";
                else if (SkillName15 == null || SkillName15 == "None" || SkillName15 == "")
                    SkillName14 += "";
                else
                    SkillName14 += ", ";

                if (SkillName15 == null || SkillName15 == "None" || SkillName15 == "")
                    SkillName15 = "";
                else if (SkillName16 == null || SkillName16 == "None" || SkillName16 == "")
                    SkillName15 += "";
                else
                    SkillName15 += "\n";

                if (SkillName16 == null || SkillName16 == "None" || SkillName16 == "")
                    SkillName16 = "";
                else if (SkillName17 == null || SkillName17 == "None" || SkillName17 == "")
                    SkillName16 += "";
                else
                    SkillName16 += ", ";

                if (SkillName17 == null || SkillName17 == "None" || SkillName17 == "")
                    SkillName17 = "";
                else if (SkillName18 == null || SkillName18 == "None" || SkillName18 == "")
                    SkillName17 += "";
                else
                    SkillName17 += ", ";

                if (SkillName18 == null || SkillName18 == "None" || SkillName18 == "")
                    SkillName18 = "";
                else if (SkillName19 == null || SkillName19 == "None" || SkillName19 == "")
                    SkillName18 += "";
                else
                    SkillName18 += ", ";

                if (SkillName19 == null || SkillName19 == "None" || SkillName19 == "")
                    SkillName19 = "";
                //else if (SkillName6 == null || SkillName6 == "None")
                //    SkillName5 = SkillName5 + "";
                else
                    SkillName19 += "";

                return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}", SkillName1, SkillName2, SkillName3, SkillName4, SkillName5, SkillName6, SkillName7, SkillName8, SkillName9, SkillName10, SkillName11, SkillName12, SkillName13, SkillName14, SkillName15, SkillName16, SkillName17, SkillName18, SkillName19);
            }
        }

        /// <summary>
        /// Gets the diva skill name from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetDivaSkillNameFromID(int id)
        {
            Dictionary.DivaSkillList.DivaSkillID.TryGetValue(id, out string? divaskillaname);
            return divaskillaname + "";
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetItemName(int id)
        {
            string itemValue1;
            bool isItemExists1 = Dictionary.Items.ItemIDs.TryGetValue(id, out itemValue1);  //returns true
            //Console.WriteLine(itemValue1); //Print "First"
            //Dictionary.Items.ItemIDs.TryGetValue(1, out itemname);
            return itemValue1 + "";
        }

        /// <summary>
        /// Gets the armor skill.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetArmorSkill(int id)
        {
            Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(id, out string? skillname);
            if (skillname == "" || skillname == null)
                return "None";
            else
                return skillname + "";
        }

        public int GetGouBoostSkill(int id)
        {
            return id switch
            {
                1 => 2,
                //case 3:
                //    return 1;
                4 => 5,
                //case 6:
                //    return 4;
                7 => 8,
                //case 9:
                //    return 7;
                10 => 11,
                //12 => 10,
                21 => 22,
                22 => 23,
                23 => 291,
                //24 => 21,
                //25 => 24,
                //26 => 25,
                27 => 28,
                //29 => 27,
                //30 => 29,
                31 => 293,
                //32 => 31,
                35 => 36,
                36 => 37,
                37 => 284,
                38 => 398,
                //39 => 38,
                40 => 41,
                //42 => 40,
                //43 => 42,
                46 => 47,
                47 => 48,
                //49 => 46,
                52 => 53,
                53 => 482,
                73 => 74,
                //75 => 73,
                //76 => 75,
                79 => 80,
                80 => 81,
                81 => 286,
                82 => 83,
                83 => 84,
                84 => 294,
                //85 => 82,
                //86 => 85,
                //87 => 86,
                88 => 89,
                //90 => 88,
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
                303 => 302,//todo: test
                351 => 352,
                357 => 356,
                361 => 362,
                369 => 370,
                370 => 491,
                477 => 476,

                _ => id,
            };
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
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(AutomaticSkillWeapon(), out string? SkillName1);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(AutomaticSkillHead(), out string? SkillName2);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(AutomaticSkillChest(), out string? SkillName3);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(AutomaticSkillArms(), out string? SkillName4);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(AutomaticSkillWaist(), out string? SkillName5);
                Dictionary.ArmorSkillList.ArmorSkillID.TryGetValue(AutomaticSkillLegs(), out string? SkillName6);

                //TODO: refactor
                if (SkillName1 == null || SkillName1 == "None" || SkillName1 == "")
                    SkillName1 = "";
                else if (SkillName2 == null || SkillName2 == "None" || SkillName2 == "")
                    SkillName1 += "";
                else
                    SkillName1 += ", ";

                if (SkillName2 == null || SkillName2 == "None" || SkillName2 == "")
                    SkillName2 = "";
                else if (SkillName3 == null || SkillName3 == "None" || SkillName3 == "")
                    SkillName2 += "";
                else
                    SkillName2 += ", ";

                if (SkillName3 == null || SkillName3 == "None" || SkillName3 == "")
                    SkillName3 = "";
                else if (SkillName4 == null || SkillName4 == "None" || SkillName4 == "")
                    SkillName3 += "";
                else
                    SkillName3 += ", ";

                if (SkillName4 == null || SkillName4 == "None" || SkillName4 == "")
                    SkillName4 = "";
                else if (SkillName5 == null || SkillName5 == "None" || SkillName5 == "")
                    SkillName4 += "";
                else
                    SkillName4 += ", ";

                if (SkillName5 == null || SkillName5 == "None" || SkillName5 == "")
                    SkillName5 = "";
                else if (SkillName6 == null || SkillName6 == "None" || SkillName6 == "")
                    SkillName5 += "";
                else
                    SkillName5 += "\n";

                if (SkillName6 == null || SkillName6 == "None" || SkillName6 == "")
                    SkillName6 = "";
                //else if (SkillName7 == null || SkillName7 == "None" || SkillName7 == "")
                //    SkillName6 += "";
                else
                    SkillName6 += "";

                if (SkillName1 == "")
                    SkillName1 = "None";

                return string.Format("{0}{1}{2}{3}{4}{5}", SkillName1, SkillName2, SkillName3, SkillName4, SkillName5, SkillName6);
            }
        }

        /// <summary>
        /// Gets the total GSR weapon unlocks.
        /// </summary>
        /// <returns></returns>
        public string GetTotalGSRWeaponUnlocks()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.GSRUnlocksExport != null)
                return s.GSRUnlocksExport;
            else
                return "11";
        }

        /// <summary>
        /// Is the gsr x11+ R999.
        /// </summary>
        /// <returns></returns>
        public bool Is11GSR999()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.Enable11GSR999)
                return true;
            else
                return false;
        }

        public string MarkdownSavedGearStats = "";

        /// <summary>
        /// Determines whether [is fixed GSR skill value] [the specified skill name].
        /// </summary>
        /// <param name="skillName">Name of the skill.</param>
        /// <returns>
        ///   <c>true</c> if [is fixed GSR skill value] [the specified skill name]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFixedGSRSkillValue(string skillName)
        {
            return skillName switch
            {
                //case "Defense+130":
                "Passive Master" or "Soul Revival" or "Secret Tech" or "Max Sharpen" or "Sharpening Up" or "Affinity+26" or "Affinity+24" or "Affinity+22" or "Affinity+20" or "Nothing" => true,
                _ => false,
            };
        }

        /// <summary>
        /// Determines whether [is maximum GSR skill value] [the specified skill name].
        /// </summary>
        /// <param name="skillName">Name of the skill.</param>
        /// <returns>
        ///   <c>true</c> if [is maximum GSR skill value] [the specified skill name]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMaxGSRSkillValue(string skillName)
        {
            return skillName switch
            {
                //case "Defense+130":
                "Passive Master" or "Soul Revival" or "Secret Tech" or "Max Sharpen" or "Affinity+26" or "Conquest Def+330" or "Conquest Atk+115" or "Def+180" or "Fire Res+35" or "Water Res+35" or "Thunder Res+35" or "Ice Res+35" or "Dragon Res+35" or "All Res+20" => true,
                _ => false,
            };
        }

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
                int Skill1 = StyleRank1();
                int Skill2 = StyleRank2();

                Dictionary.StyleRankSkillList.StyleRankSkillID.TryGetValue(Skill1, out string? SkillName1);
                Dictionary.StyleRankSkillList.StyleRankSkillID.TryGetValue(Skill2, out string? SkillName2);

                SkillName1 += "";
                SkillName2 += "";

                if (SkillName1 == "")
                    SkillName1 = "Nothing";

                if (SkillName2 == "")
                    SkillName2 = "Nothing";

                if (!(IsFixedGSRSkillValue(SkillName1)))
                    SkillName1 = GetGSRBonus(SkillName1);

                if (!(IsFixedGSRSkillValue(SkillName2)))
                    SkillName2 = GetGSRBonus(SkillName2);

                //todo: refactor pls
                if (GetTextFormat() == "Markdown")
                {
                    if (IsMaxGSRSkillValue(SkillName1) && (SkillName1 != null || SkillName1 != "Nothing" || SkillName1 != ""))
                        SkillName1 = string.Format("**{0}**", SkillName1);

                    if (IsMaxGSRSkillValue(SkillName2) && (SkillName2 != null || SkillName2 != "Nothing" || SkillName2 != ""))
                        SkillName2 = string.Format("**{0}**", SkillName2);
                }

                if (SkillName1 == null || SkillName1 == "Nothing" || SkillName1 == "")
                    SkillName1 = "";
                else if (SkillName2 == null || SkillName2 == "Nothing" || SkillName2 == "")
                    SkillName1 += "";
                else
                    SkillName1 += ", ";

                if (SkillName2 == null || SkillName2 == "Nothing" || SkillName2 == "")
                    SkillName2 = "";
                //else if (SkillName6 == null || SkillName6 == "None")
                //    SkillName5 = SkillName5 + "";
                else
                    SkillName2 += "";

                return string.Format("{0}{1}", SkillName1, SkillName2);
            }
        }

        /// <summary>
        /// Gets the GSR bonus. Values from Ferias guy
        /// </summary>
        /// <param name="skillName">Name of the skill.</param>
        /// <returns></returns>
        public string GetGSRBonus(string skillName)
        {
            //question: does maxing all gsr give 1 more point of all res?
            //also does it increase before grank slowly for the other res?

            //defense here (needs testing)
            if (IsFixedGSRSkillValue(skillName))
                return skillName;

            int Def = 0;
            int FireRes = 0;
            int WaterRes = 0;
            int IceRes = 0;
            int ThunderRes = 0;
            int DragonRes = 0;
            int AllRes = 0;
            int ConquestAtk = 0;
            int ConquestDef = 0;

            //skillName = skillName;
            int skillNameNumber = int.Parse(skillName.Substring(skillName.IndexOf("+")));
            //int skillNameNumber = int.Parse(skillNameNumberString);
            string skillNameType = skillName.Substring(0, skillName.IndexOf("+"));

            if (GSR() >= 10)
                Def += 1;

            if (GSR() >= 20)
            {
                FireRes += 2;
            }

            if (GSR() >= 30)
                ConquestDef += 10;

            if (GSR() >= 40)
            {
                WaterRes += 2;
            }

            if (GSR() >= 50)
                ConquestAtk += 2;

            if (GSR() >= 60)
                Def += 1;

            if (GSR() >= 70)
                ConquestDef += 10;

            if (GSR() >= 80)
            {
                ThunderRes += 2;
            }

            if (GSR() >= 90)
                Def += 1;

            if (GSR() >= 100)
                ConquestAtk += 2;

            if (GSR() >= 110)
                Def += 1;

            if (GSR() >= 120)
            {
                IceRes += 2;
            }

            if (GSR() >= 130)
                ConquestDef += 10;

            if (GSR() >= 140)
            {
                DragonRes += 2;
            }

            if (GSR() >= 150)
                ConquestAtk += 2;

            if (GSR() >= 160)
                Def += 1;

            if (GSR() >= 170)
                ConquestDef += 10;

            if (GSR() >= 180)
                AllRes += 1;

            if (GSR() >= 190)
                Def += 1;

            if (GSR() >= 200)
                ConquestAtk += 2;

            if (GSR() >= 210)
                Def += 1;

            if (GSR() >= 220)
            {
                FireRes += 2;
            }

            if (GSR() >= 230)
                ConquestDef += 10;

            if (GSR() >= 240)
            {
                WaterRes += 2;
            }

            if (GSR() >= 250)
                ConquestAtk += 2;

            if (GSR() >= 260)
                Def += 1;

            if (GSR() >= 270)
                ConquestDef += 10;

            if (GSR() >= 280)
            {
                ThunderRes += 2;
            }

            if (GSR() >= 290)
                Def += 1;

            if (GSR() >= 300)
                ConquestAtk += 2;

            if (GSR() >= 310)
                Def += 1;

            if (GSR() >= 320)
            {
                IceRes += 2;
            }

            if (GSR() >= 330)
                ConquestDef += 10;

            if (GSR() >= 340)
            {
                DragonRes += 2;
            }

            if (GSR() >= 350)
                ConquestAtk += 2;

            if (GSR() >= 360)
                Def += 2;

            if (GSR() >= 370)
                ConquestDef += 10;

            if (GSR() >= 380)
                AllRes += 1;

            if (GSR() >= 390)
                Def += 2;

            if (GSR() >= 400)
                ConquestAtk += 2;

            if (GSR() >= 410)
                Def += 2;

            if (GSR() >= 420)
            {
                FireRes += 2;
            }

            if (GSR() >= 430)
                ConquestDef += 10;

            if (GSR() >= 440)
            {
                WaterRes += 2;
            }

            if (GSR() >= 450)
                ConquestAtk += 2;

            if (GSR() >= 460)
                Def += 2;

            if (GSR() >= 470)
                ConquestDef += 10;

            if (GSR() >= 480)
            {
                ThunderRes += 2;
            }

            if (GSR() >= 490)
                Def += 2;

            if (GSR() >= 500)
                ConquestAtk += 2;

            if (GSR() >= 510)
                Def += 2;

            if (GSR() >= 520)
            {
                IceRes += 2;
            }

            if (GSR() >= 530)
                ConquestDef += 10;

            if (GSR() >= 540)
            {
                DragonRes += 2;
            }

            if (GSR() >= 550)
                ConquestAtk += 2;

            if (GSR() >= 560)
                Def += 2;

            if (GSR() >= 570)
                ConquestDef += 10;

            if (GSR() >= 580)
                AllRes += 1;

            if (GSR() >= 590)
                Def += 2;

            if (GSR() >= 600)
                ConquestAtk += 2;

            if (GSR() >= 610)
                Def += 2;

            if (GSR() >= 620)
            {
                FireRes += 2;
            }

            if (GSR() >= 630)
                ConquestDef += 10;

            if (GSR() >= 640)
            {
                WaterRes += 2;
            }

            if (GSR() >= 650)
                ConquestAtk += 2;

            if (GSR() >= 660)
                Def += 2;

            if (GSR() >= 670)
                ConquestDef += 10;

            if (GSR() >= 680)
            {
                ThunderRes += 2;
            }

            if (GSR() >= 690)
                Def += 2;

            if (GSR() >= 700)
                ConquestAtk += 2;

            if (GSR() >= 710)
                Def += 2;

            if (GSR() >= 720)
            {
                IceRes += 2;
            }

            if (GSR() >= 730)
                ConquestDef += 10;

            if (GSR() >= 740)
            {
                DragonRes += 2;
            }

            if (GSR() >= 750)
                ConquestAtk += 2;

            if (GSR() >= 760)
                Def += 2;

            if (GSR() >= 770)
                ConquestDef += 10;

            if (GSR() >= 780)
                AllRes += 1;

            if (GSR() >= 790)
                Def += 2;

            if (GSR() >= 800)
                ConquestAtk += 4;

            if (GSR() >= 810)
                Def += 2;

            if (GSR() >= 820)
            {
                FireRes += 2;
            }

            if (GSR() >= 830)
                ConquestDef += 10;//15?

            if (GSR() >= 840)
            {
                WaterRes += 2;
            }

            if (GSR() >= 850)
                ConquestAtk += 4;

            if (GSR() >= 860)
                Def += 2;

            if (GSR() >= 870)
                ConquestDef += 10;//15?

            if (GSR() >= 880)
            {
                ThunderRes += 2;
            }

            if (GSR() >= 890)
                Def += 2;

            if (GSR() >= 900)
                ConquestAtk += 4;

            if (GSR() >= 910)
                Def += 2;

            if (GSR() >= 920)
            {
                IceRes += 2;
            }

            if (GSR() >= 930)
                ConquestDef += 10;//15?

            if (GSR() >= 940)
            {
                DragonRes += 2;
            }

            if (GSR() >= 950)
                ConquestAtk += 4;

            if (GSR() >= 960)
                Def += 2;

            if (GSR() >= 970)
                ConquestDef += 10;//15?

            if (GSR() >= 980)
                AllRes += 1;

            if (GSR() >= 960)
                Def += 2;

            if (GSR() >= 999)
                ConquestAtk += 4;
                //AllRes += 1;/
                //ConquestDef = 300;

            var GSRSkillAddition = GetTotalGSRWeaponUnlocks() switch
            {
                "11" => 0,
                "12" => 2,
                "13" => 4,
                "14" => 5,
                _ => 0,
            };

            var GSRSkillMultiplier = GetTotalGSRWeaponUnlocks() switch
            {
                "11" => 0,
                "12" => 1,
                "13" => 2,
                "14" => 3,
                _ => 0,
            };

            if (Is11GSR999() && (skillNameType == "Conquest Atk" || skillNameType == "Conquest Def"))
            {
                ConquestAtk = 100 + (5 * GSRSkillMultiplier);
                ConquestDef = 300 + (10 * GSRSkillMultiplier);
            }

            //already tested
            return skillNameType switch
            {
                "Nothing" => "Nothing",
                "Def" => string.Format("{0}+{1}", skillNameType, skillNameNumber + Def),//goes to 80?
                "Conquest Atk" => string.Format("{0}+{1}", skillNameType, skillNameNumber + ConquestAtk),
                "Conquest Def" => string.Format("{0}+{1}", skillNameType, skillNameNumber + ConquestDef),
                "Fire Res" => string.Format("{0}+{1}", skillNameType, skillNameNumber + FireRes),
                "Water Res" => string.Format("{0}+{1}", skillNameType, skillNameNumber + WaterRes),
                "Thunder Res" => string.Format("{0}+{1}", skillNameType, skillNameNumber + ThunderRes),
                "Ice Res" => string.Format("{0}+{1}", skillNameType, skillNameNumber + IceRes),
                "Dragon Res" => string.Format("{0}+{1}", skillNameType, skillNameNumber + DragonRes),
                "All Res" => string.Format("{0}+{1}", skillNameType, skillNameNumber + AllRes),
                //"Affinity" => string.Format("{0}{1}",skillNameType,skillNameNumber + (2 * GSRSkillMultiplier)),
                _ => "None",
            };
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public string GetMetadata
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");

                if (!(s.EnableMetadataExport))
                    return "";

                string guildName;
                string hunterName;
                string DateAndTime = DateTime.Now.ToString();

                if (s.GuildName.Length >= 1)
                    guildName = s.GuildName;
                else
                    guildName = "Guild Name";
                    
                if (s.HunterName.Length >= 1)
                    hunterName = s.HunterName;
                else
                    hunterName = "Hunter Name";

                return string.Format("\n{0} | {1} | {2}",hunterName,guildName,DateAndTime);
            }
        }

        /// <summary>
        /// Gets the gear description.
        /// </summary>
        /// <value>
        /// The gear description.
        /// </value>
        public string GetGearDescription
        {
            get
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");

                if (s.GearDescriptionExport != null || s.GearDescriptionExport != "")
                    return string.Format("{0}\n", s.GearDescriptionExport);
                else 
                    return "";
            }
        }

        /// <summary>
        /// Determines whether item [is meta item].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is meta item]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMetaItem(int id)
        {
            switch (id)
            {
                default: return false;
                case 14642://hunter taloncharm
                case 1681://maiden items
                case 1682:
                case 1683:
                case 1684:
                case 1481:
                case 1482:
                case 1483:
                case 4750://perfect whetstone
                case 8030://shiri
                case 8026://encourage
                case 8037://impatience charm
                case 8031://elixir of peace
                case 8040://gathering charm min
                case 8042://gathering charm max
                case 8047://health charm
                case 8052://stamina charm
                case 1548://starving wolf potion
                case 4943://serious drink affinity
                case 9719://all element drug
                case 9715://quick mega juice
                case 9714://quick max potion
                case 4358://potion g2
                case 78://farcaster
                case 4952://halk pot
                case 5771://halk call
                case 1537://nearcaster
                case 1888://power sword crystal
                case 1553://LS spirit drink
                case 14803://adrenaline charm
                case 14804://starving wolf charm
                case 91://small barrel bombs
                case 92:
                case 1782://random ball
                case 300://poogie crackers
                case 9717://attack medicine
                case 34://lifepowder
                case 4723://gook amulet
                case 5277://gook bomb
                case 5279://gook cracker
                case 5711://gook whistle
                case 5712://gook fireworks
                case 5714://gook pickaxe
                case 5715://gook fishing net
                case 5716://gook bugnet
                case 5717://gook gloves

                //decos
                //all ravi
                case 12724:
                case 12725:
                case 12726:
                case 12727:
                case 12728:
                case 12729:
                case 12730:
                case 12731:
                case 12732:
                case 12733:
                case 12734:
                case 12735:
                case 12736:
                case 12737:
                case 12738:
                case 12739:
                case 12740:
                case 12741:
                case 12742:
                case 12743:
                case 12744:
                case 12745:
                case 12746:
                case 12747:
                case 12748:
                case 12749:
                case 12750:
                case 12751:
                case 12752:
                case 12753:
                case 15070:
                case 15071:
                case 15072:
                case 15073:
                case 15074:
                case 15075:
                case 15076:
                case 15077:
                case 15078:
                case 15079:

                //all true hiden
                case 13640:
                case 13641:
                case 13642:
                case 13643:
                case 13644:
                case 13645:
                case 13646:
                case 13647:
                case 13648:
                case 13649:
                case 13650:
                case 13651:
                case 13652:
                case 13653:
                case 13654:
                case 13655:
                case 13656:
                case 13657:
                case 13658:
                case 13659:
                case 13660:
                case 13661:
                case 13662:
                case 13663:
                case 13664:
                case 13665:
                case 13666:
                case 13667:
                case 13668:
                case 13669:
                case 13670:
                case 13671:
                case 13672:
                case 13673:
                case 13674:
                case 13675:
                case 13676:
                case 13677:
                case 13678:
                case 13679:
                case 13680:
                case 13681:
                case 13682:
                case 13683:
                case 13684:
                case 13685:
                case 13686:
                case 13687:
                case 13688:
                case 13689:
                case 13690:
                case 13691:
                case 15546:
                case 15547:
                case 15548:
                case 15549:
                    return true;
            }
        }

        public bool IsMetaGear(string piece)
        {
            if (piece.Contains("ZP") || piece.Contains("PZ") || piece.Contains("SnS・") || piece.Contains("DS・") || piece.Contains("GS・") || piece.Contains("LS・") || piece.Contains("Hammer・") || piece.Contains("HH・") || piece.Contains("Lance・") || piece.Contains("GL・") || piece.Contains("Swaxe・") || piece.Contains("Tonfa・") || piece.Contains("Magspike・") || piece.Contains("LBG・") || piece.Contains("HBG・") || piece.Contains("Bow・"))
                return true;
            else
                return false;
        }

        public string GetGRWeaponLevel(int level)
        {
            if (level == 0)
                return "";
            //else if (level % 50 == 0)
            //    return "Lv. " + level;
            else return " Lv. " + level;

        }

        public string GetRoadDureSkills
        {
            get
            {
                int Skill1 = RoadDureSkill1Name();
                int Skill2 = RoadDureSkill2Name();
                int Skill3 = RoadDureSkill3Name();
                int Skill4 = RoadDureSkill4Name();
                int Skill5 = RoadDureSkill5Name();
                int Skill6 = RoadDureSkill6Name();
                int Skill7 = RoadDureSkill7Name();
                int Skill8 = RoadDureSkill8Name();
                int Skill9 = RoadDureSkill9Name();
                int Skill10 = RoadDureSkill10Name();
                int Skill11 = RoadDureSkill11Name();
                int Skill12 = RoadDureSkill12Name();
                int Skill13 = RoadDureSkill13Name();
                int Skill14 = RoadDureSkill14Name();
                int Skill15 = RoadDureSkill15Name();
                int Skill16 = RoadDureSkill16Name();

                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill1, out string? SkillName1);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill2, out string? SkillName2);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill3, out string? SkillName3);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill4, out string? SkillName4);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill5, out string? SkillName5);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill6, out string? SkillName6);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill7, out string? SkillName7);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill8, out string? SkillName8);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill9, out string? SkillName9);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill10, out string? SkillName10);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill11, out string? SkillName11);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill12, out string? SkillName12);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill13, out string? SkillName13);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill14, out string? SkillName14);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill15, out string? SkillName15);
                Dictionary.RoadDureSkills.RoadDureSkillIDs.TryGetValue(Skill16, out string? SkillName16);

                string SkillLevel1 = RoadDureSkill1Level().ToString();
                string SkillLevel2 = RoadDureSkill2Level().ToString();
                string SkillLevel3 = RoadDureSkill3Level().ToString();
                string SkillLevel4 = RoadDureSkill4Level().ToString();
                string SkillLevel5 = RoadDureSkill5Level().ToString();
                string SkillLevel6 = RoadDureSkill6Level().ToString();
                string SkillLevel7 = RoadDureSkill7Level().ToString();
                string SkillLevel8 = RoadDureSkill8Level().ToString();
                string SkillLevel9 = RoadDureSkill9Level().ToString();
                string SkillLevel10 = RoadDureSkill10Level().ToString();
                string SkillLevel11 = RoadDureSkill11Level().ToString();
                string SkillLevel12 = RoadDureSkill12Level().ToString();
                string SkillLevel13 = RoadDureSkill13Level().ToString();
                string SkillLevel14 = RoadDureSkill14Level().ToString();
                string SkillLevel15 = RoadDureSkill15Level().ToString();
                string SkillLevel16 = RoadDureSkill16Level().ToString();

                //todo: refactor pls
                if (GetTextFormat() == "Markdown")
                {
                    if (IsMaxRoadDureSkillLevel(Skill1, SkillLevel1) && (SkillName1 != null || SkillName1 != "None" || SkillName1 != ""))
                        SkillName1 = string.Format("**{0}** ", SkillName1);

                    if (IsMaxRoadDureSkillLevel(Skill2, SkillLevel2) && (SkillName2 != null || SkillName2 != "None" || SkillName2 != ""))
                        SkillName2 = string.Format("**{0}** ", SkillName2);

                    if (IsMaxRoadDureSkillLevel(Skill3, SkillLevel3) && (SkillName3 != null || SkillName3 != "None" || SkillName3 != ""))
                        SkillName3 = string.Format("**{0}** ", SkillName3);

                    if (IsMaxRoadDureSkillLevel(Skill4, SkillLevel4) && (SkillName4 != null || SkillName4 != "None" || SkillName4 != ""))
                        SkillName4 = string.Format("**{0}** ", SkillName4);

                    if (IsMaxRoadDureSkillLevel(Skill5, SkillLevel5) && (SkillName5 != null || SkillName5 != "None" || SkillName5 != ""))
                        SkillName5 = string.Format("**{0}** ", SkillName5);

                    if (IsMaxRoadDureSkillLevel(Skill6, SkillLevel6) && (SkillName6 != null || SkillName6 != "None" || SkillName6 != ""))
                        SkillName6 = string.Format("**{0}** ", SkillName6);

                    if (IsMaxRoadDureSkillLevel(Skill7, SkillLevel7) && (SkillName7 != null || SkillName7 != "None" || SkillName7 != ""))
                        SkillName7 = string.Format("**{0}** ", SkillName7);

                    if (IsMaxRoadDureSkillLevel(Skill8, SkillLevel8) && (SkillName8 != null || SkillName8 != "None" || SkillName8 != ""))
                        SkillName8 = string.Format("**{0}** ", SkillName8);

                    if (IsMaxRoadDureSkillLevel(Skill9, SkillLevel9) && (SkillName9 != null || SkillName9 != "None" || SkillName9 != ""))
                        SkillName9 = string.Format("**{0}** ", SkillName9);

                    if (IsMaxRoadDureSkillLevel(Skill10, SkillLevel10) && (SkillName10 != null || SkillName10 != "None" || SkillName10 != ""))
                        SkillName10 = string.Format("**{0}** ", SkillName10);

                    if (IsMaxRoadDureSkillLevel(Skill11, SkillLevel11) && (SkillName11 != null || SkillName11 != "None" || SkillName11 != ""))
                        SkillName11 = string.Format("**{0}** ", SkillName11);

                    if (IsMaxRoadDureSkillLevel(Skill12, SkillLevel12) && (SkillName12 != null || SkillName12 != "None" || SkillName12 != ""))
                        SkillName12 = string.Format("**{0}** ", SkillName12);

                    if (IsMaxRoadDureSkillLevel(Skill13, SkillLevel13) && (SkillName13 != null || SkillName13 != "None" || SkillName13 != ""))
                        SkillName13 = string.Format("**{0}** ", SkillName13);

                    if (IsMaxRoadDureSkillLevel(Skill14, SkillLevel14) && (SkillName14 != null || SkillName14 != "None" || SkillName14 != ""))
                        SkillName14 = string.Format("**{0}** ", SkillName14);

                    if (IsMaxRoadDureSkillLevel(Skill15, SkillLevel15) && (SkillName15 != null || SkillName15 != "None" || SkillName15 != ""))
                        SkillName15 = string.Format("**{0}** ", SkillName15);

                    if (IsMaxRoadDureSkillLevel(Skill16, SkillLevel16) && (SkillName16 != null || SkillName16 != "None" || SkillName16 != ""))
                        SkillName16 = string.Format("**{0}** ", SkillName16);
                }

                if (SkillLevel1 == "0")
                {
                    Skill1 = 0;
                    SkillLevel1 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill1, SkillLevel1))
                        SkillLevel1 = string.Format("**LV{0}**", SkillLevel1);
                    else
                        SkillLevel1 = string.Format(" LV{0}", SkillLevel1);
                }

                if (SkillLevel2 == "0")
                {
                    Skill2 = 0;
                    SkillLevel2 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill2, SkillLevel2))
                        SkillLevel2 = string.Format("**LV{0}**", SkillLevel2);
                    else
                        SkillLevel2 = string.Format(" LV{0}", SkillLevel2);
                }

                if (SkillLevel3 == "0")
                {
                    Skill3 = 0;
                    SkillLevel3 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill3, SkillLevel3))
                        SkillLevel3 = string.Format("**LV{0}**", SkillLevel3);
                    else
                        SkillLevel3 = string.Format(" LV{0}", SkillLevel3);
                }

                if (SkillLevel4 == "0")
                {
                    Skill4 = 0;
                    SkillLevel4 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill4, SkillLevel4))
                        SkillLevel4 = string.Format("**LV{0}**", SkillLevel4);
                    else
                        SkillLevel4 = string.Format(" LV{0}", SkillLevel4);

                }

                if (SkillLevel5 == "0")
                {
                    Skill5 = 0;
                    SkillLevel5 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill5, SkillLevel5))
                        SkillLevel5 = string.Format("**LV{0}**", SkillLevel5);
                    else
                        SkillLevel5 = string.Format(" LV{0}", SkillLevel5);
                }


                if (SkillLevel6 == "0")
                {
                    Skill6 = 0;
                    SkillLevel6 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill6, SkillLevel6))
                        SkillLevel6 = string.Format("**LV{0}**", SkillLevel6);
                    else
                        SkillLevel6 = string.Format(" LV{0}", SkillLevel6);
                }

                if (SkillLevel7 == "0")
                {
                    Skill7 = 0;
                    SkillLevel7 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill7, SkillLevel7))
                        SkillLevel7 = string.Format("**LV{0}**", SkillLevel7);
                    else
                        SkillLevel7 = string.Format(" LV{0}", SkillLevel7);
                }

                if (SkillLevel8 == "0")
                {
                    Skill8 = 0;
                    SkillLevel8 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill8, SkillLevel8))
                        SkillLevel8 = string.Format("**LV{0}**", SkillLevel8);
                    else
                        SkillLevel8 = string.Format(" LV{0}", SkillLevel8);
                }

                if (SkillLevel9 == "0")
                {
                    Skill9 = 0;
                    SkillLevel9 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill9, SkillLevel9))
                        SkillLevel9 = string.Format("**LV{0}**", SkillLevel9);
                    else
                        SkillLevel9 = string.Format(" LV{0}", SkillLevel9);
                }

                if (SkillLevel10 == "0")
                {
                    Skill10 = 0;
                    SkillLevel10 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill10, SkillLevel10))
                        SkillLevel10 = string.Format("**LV{0}**", SkillLevel10);
                    else
                        SkillLevel10 = string.Format(" LV{0}", SkillLevel10);
                }

                if (SkillLevel11 == "0")
                {
                    Skill11 = 0;
                    SkillLevel11 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill11, SkillLevel11))
                        SkillLevel11 = string.Format("**LV{0}**", SkillLevel11);
                    else
                        SkillLevel11 = string.Format(" LV{0}", SkillLevel11);
                }

                if (SkillLevel12 == "0")
                {
                    Skill12 = 0;
                    SkillLevel12 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill12, SkillLevel12))
                        SkillLevel12 = string.Format("**LV{0}**", SkillLevel12);
                    else
                        SkillLevel12 = string.Format(" LV{0}", SkillLevel12);
                }

                if (SkillLevel13 == "0")
                {
                    Skill13 = 0;
                    SkillLevel13 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill13, SkillLevel13))
                        SkillLevel13 = string.Format("**LV{0}**", SkillLevel13);
                    else
                        SkillLevel13 = string.Format(" LV{0}", SkillLevel13);
                }

                if (SkillLevel14 == "0")
                {
                    Skill14 = 0;
                    SkillLevel14 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill14, SkillLevel14))
                        SkillLevel14 = string.Format("**LV{0}**", SkillLevel14);
                    else
                        SkillLevel14 = string.Format(" LV{0}", SkillLevel14);
                }

                if (SkillLevel15 == "0")
                {
                    Skill15 = 0;
                    SkillLevel15 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill15, SkillLevel15))
                        SkillLevel15 = string.Format("**LV{0}**", SkillLevel15);
                    else
                        SkillLevel15 = string.Format(" LV{0}", SkillLevel15);
                }

                if (SkillLevel16 == "0")
                {
                    Skill16 = 0;
                    SkillLevel16 = "";
                }
                else
                {
                    if (GetTextFormat() == "Markdown" && IsMaxRoadDureSkillLevel(Skill16, SkillLevel16))
                        SkillLevel16 = string.Format("**LV{0}**", SkillLevel16);
                    else
                        SkillLevel16 = string.Format(" LV{0}", SkillLevel16);
                }


                //TODO: refactor
                if (SkillName1 == null || SkillName1 == "None" || SkillName1 == "")
                    SkillName1 = "";
                else if (SkillName2 == null || SkillName2 == "None" || SkillName2 == "")
                    SkillName1 += "";
                else
                    SkillLevel1 += ", ";

                if (SkillName2 == null || SkillName2 == "None" || SkillName2 == "")
                    SkillName2 = "";
                else if (SkillName3 == null || SkillName3 == "None" || SkillName3 == "")
                    SkillName2 += "";
                else
                    SkillLevel2 += ", ";

                if (SkillName3 == null || SkillName3 == "None" || SkillName3 == "")
                    SkillName3 = "";
                else if (SkillName4 == null || SkillName4 == "None" || SkillName4 == "")
                    SkillName3 += "";
                else
                    SkillLevel3 += ", ";

                if (SkillName4 == null || SkillName4 == "None" || SkillName4 == "")
                    SkillName4 = "";
                else if (SkillName5 == null || SkillName5 == "None" || SkillName5 == "")
                    SkillName4 += "";
                else
                    SkillLevel4 += ", ";

                if (SkillName5 == null || SkillName5 == "None" || SkillName5 == "")
                    SkillName5 = "";
                else if (SkillName6 == null || SkillName6 == "None" || SkillName6 == "")
                    SkillName5 += "";
                else
                    SkillLevel5 += "\n";

                if (SkillName6 == null || SkillName6 == "None" || SkillName6 == "")
                    SkillName6 = "";
                else if (SkillName7 == null || SkillName7 == "None" || SkillName7 == "")
                    SkillName6 += "";
                else
                    SkillLevel6 += ", ";

                if (SkillName7 == null || SkillName7 == "None" || SkillName7 == "")
                    SkillName7 = "";
                else if (SkillName8 == null || SkillName8 == "None" || SkillName8 == "")
                    SkillName7 += "";
                else
                    SkillLevel7 += ", ";

                if (SkillName8 == null || SkillName8 == "None" || SkillName8 == "")
                    SkillName8 = "";
                else if (SkillName9 == null || SkillName9 == "None" || SkillName9 == "")
                    SkillName8 += "";
                else
                    SkillLevel8 += ", ";

                if (SkillName9 == null || SkillName9 == "None" || SkillName9 == "")
                    SkillName9 = "";
                else if (SkillName10 == null || SkillName10 == "None" || SkillName10 == "")
                    SkillName9 += "";
                else
                    SkillLevel9 += ", ";

                if (SkillName10 == null || SkillName10 == "None" || SkillName10 == "")
                    SkillName10 = "";
                else if (SkillName11 == null || SkillName11 == "None" || SkillName11 == "")
                    SkillName10 += "";
                else
                    SkillLevel10 += "\n";

                if (SkillName11 == null || SkillName11 == "None" || SkillName11 == "")
                    SkillName11 = "";
                else if (SkillName12 == null || SkillName12 == "None" || SkillName12 == "")
                    SkillName11 += "";
                else
                    SkillLevel11 += ", ";

                if (SkillName12 == null || SkillName12 == "None" || SkillName12 == "")
                    SkillName12 = "";
                else if (SkillName13 == null || SkillName13 == "None" || SkillName13 == "")
                    SkillName12 += "";
                else
                    SkillLevel12 += ", ";

                if (SkillName13 == null || SkillName13 == "None" || SkillName13 == "")
                    SkillName13 = "";
                else if (SkillName14 == null || SkillName14 == "None" || SkillName14 == "")
                    SkillName13 += "";
                else
                    SkillLevel13 += ", ";

                if (SkillName14 == null || SkillName14 == "None" || SkillName14 == "")
                    SkillName14 = "";
                else if (SkillName15 == null || SkillName15 == "None" || SkillName15 == "")
                    SkillName14 += "";
                else
                    SkillLevel14 += ", ";

                if (SkillName15 == null || SkillName15 == "None" || SkillName15 == "")
                    SkillName15 = "";
                else if (SkillName16 == null || SkillName16 == "None" || SkillName16 == "")
                    SkillName15 += "";
                else
                    SkillLevel15 += "\n";

                if (SkillName16 == null || SkillName16 == "None" || SkillName16 == "")
                    SkillName16 = "";
                //else if (SkillName6 == null || SkillName6 == "None")
                //    SkillName5 = SkillName5 + "";
                else
                    SkillLevel16 += "";

                return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}", SkillName1, SkillLevel1, SkillName2, SkillLevel2, SkillName3, SkillLevel3, SkillName4, SkillLevel4, SkillName5, SkillLevel5, SkillName6, SkillLevel6, SkillName7, SkillLevel7, SkillName8, SkillLevel8, SkillName9, SkillLevel9, SkillName10, SkillLevel10, SkillName11, SkillLevel11, SkillName12, SkillLevel12, SkillName13, SkillLevel13, SkillName14, SkillLevel14, SkillName15, SkillLevel15, SkillName16, SkillLevel16);
            }
        }

        /// <summary>
        /// Generates the gear stats
        /// </summary>
        public string GenerateGearStats()
        {
            //save gear to variable
            string showGouBoost = "";

            if (GetGouBoostMode())
                showGouBoost = " (After Gou/Muscle Boost)";
            //todo: sr skill
            //zp in bold for markdown
            //fruits and speedrunner items also in bold
            SavedGearStats = string.Format("【MHF-Z】Overlay {0} {1}({2}){3}\n\n{4}{5}: {6}\nHead: {7}\nChest: {8}\nArms: {9}\nWaist: {10}\nLegs: {11}\nCuffs: {12}\n\nWeapon Attack: {13} | Total Defense: {14}\n\nZenith Skills:\n{15}\n\nAutomatic Skills:\n{16}\n\nActive Skills{17}:\n{18}\n\nCaravan Skills:\n{19}\n\nDiva Skill:\n{20}\n\nGuild Food:\n{21}\n\nStyle Rank:\n{22}\n\nItems:\n{23}\n\nAmmo:\n{24}\n\nPoogie Item:\n{25}\n\nRoad/Duremudira Skills:\n{26}\n", MainWindow.CurrentProgramVersion, GetWeaponClass(), GetGender(), GetMetadata, GetGearDescription, CurrentWeaponName, GetRealWeaponName, GetArmorHeadName, GetArmorChestName, GetArmorArmName, GetArmorWaistName, GetArmorLegName, GetCuffs, BloatedWeaponAttack().ToString(), TotalDefense().ToString(), GetZenithSkills, GetAutomaticSkills, showGouBoost, GetArmorSkills, GetCaravanSkills, GetDivaSkillNameFromID(DivaSkill()), GetArmorSkill(GuildFoodSkill()), GetGSRSkills, GetItemPouch, GetAmmoPouch, GetItemName(PoogieItemUseID()), GetRoadDureSkills);
            MarkdownSavedGearStats = string.Format("__【MHF-Z】Overlay {0}__ *{1}({2})*{3}\n\n{4}**{5}**: {6}\n**Head:** {7}\n**Chest:** {8}\n**Arms:** {9}\n**Waist:** {10}\n**Legs:** {11}\n**Cuffs:** {12}\n\n**Weapon Attack:** {13} | **Total Defense:** {14}\n\n**Zenith Skills:**\n{15}\n\n**Automatic Skills:**\n{16}\n\n**Active Skills{17}:**\n{18}\n\n**Caravan Skills:**\n{19}\n\n**Diva Skill:**\n{20}\n\n**Guild Food:**\n{21}\n\n**Style Rank:**\n{22}\n\n**Items:**\n{23}\n\n**Ammo:**\n{24}\n\n**Poogie Item:**\n{25}\n\n**Road/Duremudira Skills:**\n{26}\n", MainWindow.CurrentProgramVersion, GetWeaponClass(), GetGender(), GetMetadata, GetGearDescription, CurrentWeaponName, GetRealWeaponName, GetArmorHeadName, GetArmorChestName, GetArmorArmName, GetArmorWaistName, GetArmorLegName, GetCuffs, BloatedWeaponAttack().ToString(), TotalDefense().ToString(), GetZenithSkills, GetAutomaticSkills, showGouBoost, GetArmorSkills, GetCaravanSkills, GetDivaSkillNameFromID(DivaSkill()), GetArmorSkill(GuildFoodSkill()), GetGSRSkills, GetItemPouch, GetAmmoPouch, GetItemName(PoogieItemUseID()),GetRoadDureSkills);
            return string.Format("【MHF-Z】Overlay {0} {1}({2}){3}\n\n{4}{5}: {6}\nHead: {7}\nChest: {8}\nArms: {9}\nWaist: {10}\nLegs: {11}\nCuffs: {12}\n\nWeapon Attack: {13} | Total Defense: {14}\n\nZenith Skills:\n{15}\n\nAutomatic Skills:\n{16}\n\nActive Skills{17}:\n{18}\n\nCaravan Skills:\n{19}\n\nDiva Skill:\n{20}\n\nGuild Food:\n{21}\n\nStyle Rank:\n{22}\n\nItems:\n{23}\n\nAmmo:\n{24}\n\nPoogie Item:\n{25}\n\nRoad/Duremudira Skills:\n{26}\n", MainWindow.CurrentProgramVersion, GetWeaponClass(), GetGender(), GetMetadata, GetGearDescription, CurrentWeaponName, GetRealWeaponName, GetArmorHeadName, GetArmorChestName, GetArmorArmName, GetArmorWaistName, GetArmorLegName, GetCuffs, BloatedWeaponAttack().ToString(), TotalDefense().ToString(), GetZenithSkills, GetAutomaticSkills, showGouBoost, GetArmorSkills, GetCaravanSkills, GetDivaSkillNameFromID(DivaSkill()), GetArmorSkill(GuildFoodSkill()), GetGSRSkills, GetItemPouch, GetAmmoPouch, GetItemName(PoogieItemUseID()), GetRoadDureSkills);
        }

        /// <summary>
        /// Gets the gear stats.
        /// </summary>
        /// <value>
        /// The gear stats.
        /// </value>
        public string GetGearStats
        {
            get
            {
                return GenerateGearStats();
            }
        }

        /// <summary>
        /// https://xl3lackout.github.io/MHFZ-Ferias-English-Project/
        /// </summary>
        public string GetGameArmorSkillsPriority
        {
            get
            {
                return string.Format("Skill Priority:\n" +
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
                    );
            }
        }

        public string GetGameArmorSkillsHealthAndStamina
        {
            get
            {
                return "Health and Stamina\n\nHealth\t○\tHealth+50\t40\tMaximum Health +50\n\nHealth+40\t30\tMaximum Health +40\n\nHealth+30\t20\tMaximum Health +30\n\nHealth+20\t15\tMaximum Health +20\n\nHealth+10\t10\tMaximum Health +10\n\nHealth-10\t-10\tMaximum Health -10\n\nHealth-20\t-15\tMaximum Health -20\n\nHealth-30\t-20\tMaximum Health -30\n\nRecovery Speed\t○\tRecovery Speed+2\t20\t4x Health Recovery Speed\n\nRecovery Speed+1\t10\t3x Health Recovery Speed\n\nRecovery Speed-1\t-10\t3x Slower Health Recovery Speed\n\nRecovery Speed-2\t-20\t4x Slower Health Recovery Speed\n\nHunger\t○\tHunger Negated\t15\tStamina bar length does not decrease in length over time.\n\nHunger Halved\t10\tStamina bar length decreases at 0.5x the speed over time.\n\nHunger Up (Sm)\t-10\tStamina bar length decreases at 1.5x the speed over time\n\nHunger Up (Lg)\t-15\tStamina bar length decreases at 2.0x the speed over time\n\nRecovery\t×\tRecovery Items Up\t10\tRecovery item effect boosted 1.25x\n\nRecovery Items Down\t-10\tRecovery item effectlowered to 0.75x\n\nVampirism\t×\tVampirism+2\t20\tWhen attacking a monster, there is an 80% that your Health will recover.\n\nVampirism+1\t10\tWhen attacking a monster, there is a 60% that your Health will recover.\n\nHerbal Science\t×\tMedical Sage\t10\tInstant recovery of any Red HP when using any healing item.\n\nAdditional effects if multiple party members have the skill：\n\n2Players: recovery items apply to the entire party.\n\n3Players: recovery items apply to the entire party, +20extra HP Recovery\n\n4Players: recovery items apply to the entire party, +50extra HP Recovery\n\n※※Stacks with Recovery skill, only pure recovery items are party wide (e.g.Max Potions yes,Ancient Potions no.)\n\nStamina Recovery\t○\tStamina Recovery Up【Large】\t20\tDoubles stamina recovery speed over time\n\nStamina Rec (Small)\t10\tStamina recovery speed increases by 1.5 times over time.\n\nStamina Rec Down\t-10\tHalves stamina recovery speed over time\n\nStamina\t○\t\n\nStamina values usually decrease by 15 units\n\nPeerless\t20\tStamina decrease rate is halved (decrease is reduced to 8 units)\n\nIn addition, the reduction of stamina when evading or guarding is reduced to 50%.\n\nMarathon Runner\t10\tStamina decrease rate is halved (decrease is reduced to 8 units)\n\nIn addition, the reduction of stamina when evading or guarding is reduced to 75%.\n\nShort Sprinter\t-10\tStamina depletion speed is increased by 1.2 times (up to 18 units)\n\nEating\t×\tSpeed Eating\t10\tIncreases the speed of eating consumables such as meat and healing potions.\n\nSlow Eating\t-10\tReduces the speed of eating consumables such as meat and healing potions.\n\nGluttony\t×\tScavenger\t15\tUsing consumables restores 25 maximum stamina\n\nGourmand\t10\tStamina recovery when eating meat is increased by 25";
            }
        }

        public string GetGameArmorSkills
        {
            get 
            {
                Settings s = (Settings)Application.Current.TryFindResource("Settings");

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
                    _ => GetGameArmorSkillsPriority,
                };
            }
        }

        public string GetGameArmorSkillsAttackSkills
        {
            get
            {
                return "Attack Skills\n\nStrong Attack\t×\tStrong Attack+6\t50\tIncreases attack power by 200. Does not overlap with skills that increase attack power\n\nStrong Attack+5\t40\tIncreases attack power by 150. Does not overlap with skills that increase attack power\n\nStrong Attack+4\t30\tIncreases attack power by 80. Does not overlap with skills that increase attack power\n\nStrong Attack+3\t20\tIncreases attack power by 50. Does not overlap with skills that increase attack power\n\nStrong Attack+2\t15\tIncreases attack power by 35. Does not overlap with skills that increase attack power\n\nStrong Attack+1\t10\tIncreases attack power by 20. Does not overlap with skills that increase attack power\n\nAttack\t○\tAttack Up(Absolute)\t80\tAttack power increases by 50\n\nAttack Up (Very Large)\t50\tAttack power increases by 30\n\nAttack Up (Large)\t25\tAttack power increases by 20\n\nAttack Up (Medium)\t15\tAttack power increases by 10\n\nAttack Up (Small)\t10\tAttack power increases by 5\n\nLone Wolf\t×\tLone Wolf\t10\tIncreases attack power by 100 when there are no Hunters or Rastas in the same area.\n\nHalks are allowed\n\nAdrenaline\t×\t\n\nNormally, at 40 HP, attack power remains unchanged, but defense increases by 60.\n\nAdrenaline+2\t15\n\n∥\n\n20\tAttack power increases by 1.5 times (1.3 times for Bowgun) when HP reaches 40.\n\nDefense increased to 90\n\nAdrenaline+1\t10\n\n21\tWhen HP reaches 40, defense increased to 90\n\nWorry\t-15\n\n30\tWhen HP reaches 40, attack power is reduced by 0.7 times. Defense increase is reduced to 42\n\nIssen\t×\tIssen+3\t30\tIncreases crit rate by 20% and changes critical multiplier to 1.50 (from 1.25)\n\nIssen+2\t20\tIncreases crit rate by 10% and changes critical multiplier to 1.40 (from 1.25)\n\nIssen+1\t10\tIncreases crit rate by 5% and changes critical multiplier to 1.35 (from 1.25)\n\nExpert\t○\tCritical Eye+5\t50\tAffinity +50%\n\nCritical Eye+4\t35\tAffinity +40%\n\nCritical Eye+3\t20\tAffinity +30%\n\nCritical Eye+2\t15\tAffinity +20%\n\nCritical Eye+1\t10\tAffinity +10%\n\nSkilled\t×\tSkilled\t15\tGrants the effects of Speed Eater, Movement Speed+2, True Guts and Weapon Handling.\n\nTrained\t×\tTrained+2\t15\tGrants Focus+2 and Kickboxing King. Stops hits interacting with players and AI.\n\nTrained+1\t10\tGrants Focus+1 and Martial Arts. Stops hits interacting with players and AI.\n\nAbnormality\t×\tAbnormality\t15\tGrants Status Attack Up, Status Pursuit and Drug Knowledge.\n\nCrit Conversion\t×\tCrit Conversion\t10\tAffinity increases by 30%\n\nWhen Affinity Rate is 101% or higher, attack power increases by √(Excess Affinity x 7)\n\nExploit Weakness\t×\tExploit Weakness\t20\tWhen attacking a part with a value of 35 or higher, +5 to the value of the target part. Attribute damage remains the same\n\nReduce Weakness\t-10\tWhen attacking a part with a value of 35 or higher, -5 to the value of the target part. Attribute damage remains the same\n\nStylish Assault\t×\tStylish Assault\t15\tEvading through attacks increases attack by +100 true raw for a time period that varies based on weapon class.\n\nSnS,DS,Tonfa: 14 Seconds、Heavy Bowgun:24 Seconds\n\nAll Others: 19 Seconds\n\nDissolver\t×\tElemental Exploit\t15\tIncreases monsters' hitzones elemental effectiveness values by a value that varies based on weapon class when they are 20 or higher.\n\nSnS,GS, LS,Hammer,Hunting Horn, Lance,Switch Axe F,Magnet Spike: +15\n\nDual Swords,Gunlance, LightBowgun:+10\n\nTonfas,Heavy Bowgun,Bow:+5\n\nElemental Diffusion\t-10\tDecreases monsters' hitzones elemental effectiveness values -5 when they are 20 or higher.\n\nStatus Assault\t×\tStatus Pursuit\t10\tInflicts additional damage with status attacks on monsters suffering status ailments. *However, there is no effect on Raviente.\n\nDrug Knowledge\t×\tDrug Knowledge\t10\tThe attack power of weapons with status attributes is increased by 1/4 of the abnormal status value, and status damage is dealt on every hit.\n\n（※status value on weapon is set to 0.38x with 25% of the true base status）\n\nStatus Attack\t×\tStatus Attack Up\t10\tPoison,Paralysis and Sleep multiplied by 1.125x\n\nBomb Boost\t×\tBomber\t10\tBomb Strength 1.5x、Bombs Combination Rate 100%, Blast Element Damage increased.\n\nGunnery\t○\tArtillery God\t35\tBallista: Damage x1.3\n\nGunlance: Shelling Raw Attribute x1.3,Wyvern Fire Raw Attribute x1.4\n\nBowguns: Crag/Clust S Raw Attribute x1.5,HBG Fire Beam damage x1.3\n\nTonfa: Ryuuki Detonation damage x1.6\n\nArtillery Expert\t20\tBallista: Damage x1.2\n\nGunlance: Shelling Raw Attribute x1.2,Wyvern Fire Raw Attribute x1.3\n\nBowguns: Crag/Clust S Raw Attribute x1.5,HBG Fire Beam damage x1.2\n\nTonfa: Ryuuki Detonation damage x1.4\n\nGunnery\t10\tBallista: Damage x1.1\n\nGunlance: Shelling Raw Attribute x1.1,Wyvern Fire Raw Attribute x1.2\n\nBowguns: Crag/Clust S Raw Attribute x1.5,HBG Fire Beam damage x1.1\n\nTonfa: Ryuuki Detonation damage x1.25\n\nFire Attack\t○\tFire Attack (Large)\t20\tFire Attack 1.2x\n\nFire Attack (Small)\t10\tFire Attack 1.1x\n\nWater Attack\t○\tWater Attack (Large)\t20\tWater Attack 1.2x\n\nWater Attack (Small)\t10\tWater Attack 1.1x\n\nThunder Attack\t○\tThunder Attack (Large)\t20\tThunder Attack 1.2x\n\nThunder Attack (Small)\t10\tThunder Attack 1.1x\n\nIce Attack\t○\tIce Attack (Large)\t20\tIce Attack 1.2x\n\nIce Attack (Small)\t10\tIce Attack 1.1x\n\nDragon Attack\t○\tDragon Attack (Large)\t20\tDragon Attack 1.2x\n\nDragon Attack (Small)\t10\tDragon Attack 1.1x\n\nElemental Attack\t×\tElemental Attack Up\t10\tAll Elements 1.1x\n\nElemental Attack Down\t-10\tAll Elements 0.9x\n\nMartial Arts\t○\tKickboxing King\t20\tKick deals 15damage and becomes a roundhouse kick while\n\nstanding or a drop kick while moving.Tonfa kick attacks deal higherdamage as well as adding more air time to all aerial attacks.\n\nMartial Arts\t10\tKick deals 15 damage.Tonfa kick attacks deal slightly higher Damage\n\nSurvivor\t×\tTenacity\t20\tEach time the number of remaining revivals in the quest decreases,\n\n1st time: Attack power +15, Defense +100\n\n2nd time: Attack power +20, Defense +150\n\n※Does not work in Great Slaying and Caravan Quests\n\nFortify\t10\tEach time the number of remaining revivals in the quest decreases,\n\nEach time the number of remaining revivals in the quest decreases,\n\n1st time: Attack power +5, Defense +50\n\n2nd time: Attack power +10, Defense +100\n\n※Does not work in Great Slaying and Caravan Quests\n\nRage\t×\tBuchigiri\t20\tTrue Guts and Adrenaline+2\n\nWrath Awoken\t15\tTrue Guts and Adrenaline+1\n\nFasting\t×\tStarving Wolf+2\t20\tIncreases affinity by 50%, grants evade+2, and critical hit modifier to 1.35x when hungry\n\n(Active at 25 Stamina *minimum bar*)\n\nStarving Wolf+1\t10\tIncreases affinity by 50%, grants evade+1\n\nPriority is given to higher skills (Active at 25 Stamina *minimum bar*)\n\nFocus\t○\tFocus+2\t20\t0.8x Charge attack duration for GS,Hammer,Long Sword,HBG,Lance,Tonfa and Switch Axe F where appropriate.\n\n1.2x fill rate for gauges (LS Spirit, SwAxe Sword and Tonfa)\n\nHBG heat beam meter filled an extra 2 units per shot (no effect for bows)\n\nFocus+1\t10\t0.9x Charge attack duration for GS,Hammer,Long Sword,HBG,Lance,Tonfa and Switch Axe F where appropriate.\n\n1.1x fill rate for gauges (LS Spirit, SwAxe Sword and Tonfa)\n\nHBG heat beam meter filled an extra 1 unit per shot (no effect for bows)\n\nDistraction\t-10\t1.2x Charge attack duration for GS,Hammer,Long Sword,HBG,Lance,Tonfa and Switch Axe F where appropriate.\n\n0.8x fill rate for gauges (LS Spirit, SwAxe Sword and Tonfa)\n\nHBG heat beam meter loses 1 unit per shot (no effect for bows)\n\nCharge Attack Up\t×\tCharge Attack Up+2\t20\tGreatsword, long sword, hammer, lance, and tonfa damage and attribute values are further increased when charging attacks.\n\nCan be used in combination with 「Focus」\n\nCharge Attack Up+1\t10\tGreatsword, long sword, hammer, lance, and tonfa damage and attribute values are increased when charging attacks.\n\nCan be used in combination with 「Focus」\n\nWeapon Handling\t×\tWeapon Handling\t10\tWeapon draw/sheath speed increased by 20% 「Does not stack with Master Seal or G Finesse weapon effects」\n\nShiriagari\t×\tShiriagari\t15\tAttack power (multiplier) increases according to the elapsed time of the quest\n\n1 min +20, 3 min +50, 5 min +80, 10 min +130, 15 min +180, 20 min +200\n\nAdaptation\t×\tAdaptation+2\t20\tWhen HP is over 100, other attack systems are added based on hitbox values using the multipliers 0.81x for melee and 0.72x for range\n\nType of damage changes to the type with the best result. (ex. Hammers can cut tails if hitbox values for cutting are higher on tail)\n\nAdaptation+1\t10\tWhen HP is over 100, other attack systems are added based on hitbox values using the multipliers 0.72x for melee and 0.64x for range\n\nType of damage changes to the type with the best result. (ex. Hammers can cut tails if hitbox values for cutting are higher on tail)\n\nCombat Supremacy\t×\tCombat Supremacy\t10\tAttack increases by 1.2 times while weapon is unsheathed. However, stamina will always decrease while the effect is active.\n\n※Even if the stamina runs out, the effect will continue as long as the sword is not sheathed.\n\n※Effect is nullified if Rasta is activated\n\nVigorous\t×\tVigorous\t10\tWhen HP is 100 or more, attack power increases by 1.15x\n\nLavish Attack\t×\tConsumption Slayer\t10\tAttack+100 at the cost of additional sharpness loss per hit with Blademaster weapons\n\nadditional 0.2x multiplier on coatings at the cost of double consumption per shot. (no effect on bowguns)\n\nObscurity\t×\tObscurity\t10\tYour attack will increase by a set value every time you block an attack, up to 10x. Values differ per weapon.\n\nSnS,Lance,Gunlance,Tonfa: 1-5 Blocked Attacks +40, 6-10 Blocked Attacks +20,Max Buff +300.\n\nSwitch Axe F,GS,Magnet Spike: 1-5 Blocked Attacks +30, 6-10 Blocked Attacks +15,Max Buff +225\n\nLong Sword: 1-5 Blocked Attacks +20, 6-10 Blocked Attacks +10,Max Buff +150\n\nRush\t×\tRush\t10\tSuccessful attacks and guard actions gradually increase an invisible meter that has two stages.\n\nStage one: Purple flash and +50 attack, Stage two: repeate flash and +130 attack. running with weapons now also costs 0 stamina.\n\nThe effect is completely reset when you use any items or sheathe your weapon.\n\nCeaseless\t×\tCeaseless\t10\tIncreases affinity and critical multiplier as you land attacks\n\nDissapears if you stop attacking for a number of seconds.\n\nReflect and Stylish Up count towards hit totals but Fencing+2 does not.\n\nPoint Breakthrough\t×\tPoint Breakthrough\t10\tIncreases the Raw Weakness Value of a part that has been hit repeatedly by +5.\n\nThe effect has a time limit and is only applied to a single part at a time.\n\nFurious\t×\tFurious\t10\tIncrease attack, affinity, elemental and status across 3 stages as you perform attacks, evasions or guards.\n\nThunder Clad\t×\tThunder Clad\t10\tGauge is accumulated by evading, attacking, and moving.\n\nWhen the gauge reaches its maximum, \"Status Negate\", \"Movement speed up +2\", \"Weapon handling\", and \"Evasion distance up\" will be activated\n\nHowever, if a skill of the same system is activated, the skill with the highest effect takes precedence.\n\nAlso, movement speed increases for a certain period of time when the sword is drawn, and damage to the monster parts always increases.\n\nAttribute damage is unchanged\n\n※Part damage increase stacks with「Weakness Exploit」 and 「Solid Determination」\n\nDetermination\t×\tSolid Determination\t10\tAttack power rises and Affinity increases by 100%.\n\nCrit damage is also increased、「Adrenaline+2」 and 「Sharpness+1」 are also activated、\n\nDamage to the attacked parts and attribute damage is always increased.\n\nAlso, damage increases when certain bullets and arrows hit at a critical distance.\n\nDoes not stack with、「Attack」「Strong Attack」「Issen」「Exploit Weakness」「Elemental Attack」「Precision」「Critical Shot」「Expert」、\n\nIf skills of the same type are activated, the skill with the highest effect takes precedence.\n\n「Absolute Defense」「True Guts」「Great Guts」「Guts」 are disabled\n\nas well as「Guts Tickets」「Mega Guts Ticket」 and 「Soul Revival」";
            }
        }

        public string GetGameArmorSkillsDefensiveSkills
        {
            get
            {
                return "Defensive Skills\n\nVitality\t×\tVitality+3\t30\tDefense+90,Damage Recovery Speed+2,Recovery Items Up,\n\n100% Combination success-rate for Potions,Mega Potions and Max Potions.\n\nVitality+2\t15\tDefense+45,Damage Recovery Speed+1,Recovery Items Up,\n\n100% Combination success-rate for Potions,Mega Potions and Max Potions.\n\nVitality+1\t10\tDefense+15,Damage Recovery Speed+1,Recovery Items Up,\n\n100% Combination success-rate for Potions,Mega Potions and Max Potions.\n\nVitality-1\t-10\tHealth-10,Damage Recovery Speed-1,Recovery Items Weakened.\n\nReflect\t×\tReflect+3\t20\tBlocking an attack causes a special motion to trigger from the point of guarding with an impact based MotionValue of 48.\n\nThe hit uses your current attack and sharpness values.\n\nReflect+2\t15\tBlocking an attack causes a special motion to trigger from the point of guarding with an impact based MotionValue of 36.\n\nThe hit uses your current attack and sharpness values.\n\nReflect+1\t10\tBlocking an attack causes a special motion to trigger from the point of guarding with an impact based MotionValue of 24.\n\nThe hit uses your current attack and sharpness values.\n\nDefense\t○\tDefense+120\t45\tIncreases defense by 120\n\nDefense+90\t35\tIncreases defense by 90\n\nDefense+60\t25\tIncreases defense by 60\n\nDefense+30\t15\tIncreases defense by 30\n\nDefense+20\t10\tIncreases defense by 20\n\nDefense-20\t-10\tDecrease defense by 20\n\nDefense-30\t-15\tDecrease defense by 30\n\nDefense-40\t-20\tDecrease defense by 40\n\nGuard\t○\t\n\nNo damage if not significantly attacked.\n\nWeapon type\tPower Value1～15\tPower Value16～39\tPower Value40+\n\nSnS\tSlight KB\tLarge KB\tLarge KB\n\nGS\tStand Still\tSlight KB\tLarge KB\n\nLance\tStand Still\tStand Still\tLarge KB\n\nGuard+2\t20\tMaximum blocking damage reduction. Further decreases knock-back and stamina consumption for blocking.\n\nAllows for blocking of some previously unblockable attacks\n\nIncreases Lance's shield aura hit limit to 6.\n\nGuard+1\t10\tIncreases blocking damage reduction.Decreases knock-back and stamina consumption forblocking.\n\nIncreases Lance's shield aura hit limit to 4.\n\nGuard-1\t-10\tIncreases damage taken, knockback and stamina consumption while blocking.\n\nGuard-2\t-15\tGreatly Increases damage taken, knockback and stamina consumption while blocking.\n\nAuto-Guard\t×\tAuto-Guard\t10\tAutomatically blocks any incoming attacks which can be blocked while a weapon is unsheathed.\n\nZZ Changes: No longer activates Reflect,Obscurity or Rush. Does not fill lance guard gauge.\n\nFortification\t×\tFortification+2\t15\tHybrid skill containing Guard+2, Peerless and Weapon Handling.\n\nFortification+1\t10\tHybrid skill containing Guard+1, Marathon Runner and Weapon Handling.";
            }
        }

        public string GetGameArmorSkillsBlademasterSkills
        {
            get
            {
                return "Blademaster Skills\n\nSharpening\t○\tSharpening Artisan\t20\tSharpen at 4x original speed, gain 30 seconds of infinite sharpness and Sharpness+1 after sharpening\n\nSpeed Sharpening\t10\tSharpen with a single stroke of a whetstone.\n\nSlothful Sharpening\t-10\tSharpening duration is doubled.\n\nSharpness\t○\tRazor Sharp+2\t20\tSharpness loss is halved where applicable\n\n50% chance of any sharpness loss being completely negated.\n\nRazor Sharp+1\t10\tSharpness loss is halved where applicable\n\nBlunt Edge\t-10\tSharpness loss is doubled.\n\nStylish\t×\tStylish\t15\tRecover some sharpness when evading through attacks.Amount recovered varies per weapon type.\n\nDual Swords: 12Units.\n\nGunlance: 10 Units.\n\nSword and Shield: 8 Units.\n\nHunting Horn / Switch Axe F / Great Sword / Lance: 5 Units.\n\nLong Sword: 5 Units (Blink: 7 Units).\n\nHammer: 3 Units.\n\nTonfa: 4 Units\n\nTonfa Jump: 3 Units\n\nTonfa EX,Emergency: 10 Units\n\nTonfa DashKick: 6 Units\n\nMagnet Spike: 10 Units (Magnetism Evade: 9)\n\nEdgemaster\t×\tHoned Blade+3\t20\tAttack Up (Absolute) and Sharpness+1\n\nHaving a stronger Attack Up or Strong Attack Up skill activated replaces the Attack component of this skill.\n\nHoned Blade+2\t15\tAttack Up (Very Large) and Sharpness+1\n\nHaving a stronger Attack Up or Strong Attack Up skill activated replaces the Attack component of this skill.\n\nHoned Blade+1\t10\tAttack Up (Large) and Sharpness+1\n\nHaving a stronger Attack Up or Strong Attack Up skill activated replaces the Attack component of this skill.\n\nArtisan\t×\tSharpness+1\t10\tMaximum weapon sharpness increased by +1 level if possible.\n\nBomb Sword\t○\tBomb Sword+3\t20\tWhen the bomb sword crystal is loaded, the effect of the bomb sword [strong] is activated. * Consumes 3 times sharpness\n\nBomb Sword+2\t15\tWhen the bomb sword crystal is loaded, the effect of the bomb sword [medium] is activated. * Consumes 3 times sharpness\n\nBomb Sword+1\t10\tWhen the bomb sword crystal is loaded, the effect of the bomb sword [weak] is activated. * Consumes 3 times sharpness\n\nPoison Sword\t○\tPoison Sword+3\t20\tWhen the poison sword crystal is loaded, the effect of poison sword [strong] is activated. *Consumes double sharpness\n\nPoison Sword+2\t15\tWhen the poison sword crystal is loaded, the effect of poison sword [medium] is activated. *Consumes double sharpness\n\nPoison Sword+1\t10\tWhen the poison sword crystal is loaded, the effect of poison sword [weak] is activated. *Consumes double sharpness\n\nPara Sword\t○\tPara Sword+3\t20\tParalysis sword [strong] effect is activated when the paralysis sword crystal is loaded. *Consumes double sharpness\n\nPara Sword+2\t15\tParalysis sword [medium] effect is activated when the paralysis sword crystal is loaded. *Consumes double sharpness\n\nPara Sword+1\t10\tParalysis sword [weak] effect is activated when the paralysis sword crystal is loaded. *Consumes double sharpness\n\nSleep Sword\t○\tSleep Sword+3\t20\tWhen the sleep sword crystal is loaded, the effect of sleep sword [strong] is activated. *Consumes double sharpness\n\nSleep Sword+2\t15\tWhen the sleep sword crystal is loaded, the effect of sleep sword [medium] is activated. *Consumes double sharpness\n\nSleep Sword+1\t10\tWhen the sleep sword crystal is loaded, the effect of sleep sword [weak] is activated. *Consumes double sharpness\n\nFire Sword\t○\tFire Sword+3\t20\tWhen the flame sword crystal is loaded, the effect of the flame sword [strong] is activated. *Consumes double sharpness\n\nFire Sword+2\t15\tWhen the flame sword crystal is loaded, the effect of the flame sword [medium] is activated. *Consumes double sharpness\n\nFire Sword+1\t10\tWhen the flame sword crystal is loaded, the effect of the flame sword [weak] is activated. *Consumes double sharpness\n\nWater Sword\t○\tWater Sword+3\t20\tWhen a Water Sword Crystal is loaded, the effect of Water Sword [strong] is activated. *Consumes double sharpness\n\nWater Sword+2\t15\tWhen a Water Sword Crystal is loaded, the effect of Water Sword [medium] is activated. *Consumes double sharpness\n\nWater Sword+1\t10\tWhen a Water Sword Crystal is loaded, the effect of Water Sword [weak] is activated. *Consumes double sharpness\n\nThunder Sword\t○\tThunder Sword+3\t20\tWhen a Thunder Sword Crystal is loaded, the effect of Thunder Sword [strong] is activated. *Consumes double sharpness\n\nThunder Sword+2\t15\tWhen a Thunder Sword Crystal is loaded, the effect of Thunder Sword [medium] is activated. *Consumes double sharpness\n\nThunder Sword+1\t10\tWhen a Thunder Sword Crystal is loaded, the effect of Thunder Sword [weak] is activated. *Consumes double sharpness\n\nIce Sword\t○\tIce Sword+3\t20\tWhen an ice sword crystal is loaded, the effect of the ice sword [strong] is activated. *Consumes double sharpness\n\nIce Sword+2\t15\tWhen an ice sword crystal is loaded, the effect of the ice sword [medium] is activated. *Consumes double sharpness\n\nIce Sword+1\t10\tWhen an ice sword crystal is loaded, the effect of the ice sword [weak] is activated. *Consumes double sharpness\n\nDragon Sword\t○\tDragon Sword+3\t20\the effect of Dragon Sword [strong] is activated when a Dragon Sword Crystal is loaded. *Consumes double sharpness\n\nDragon Sword+2\t15\the effect of Dragon Sword [medium] is activated when a Dragon Sword Crystal is loaded. *Consumes double sharpness\n\nDragon Sword+1\t10\the effect of Dragon Sword [weak] is activated when a Dragon Sword Crystal is loaded. *Consumes double sharpness\n\nSnS Tech\t×\tSnS Tech【Master】\tBoth Hiden\tAdds sharpness level +1 to SnS Tech [Sword Saint]\n\nSword Saint\t30\tSuper High-Grade Earplugs\n\nAttack x1.3 when wielding a One-handed Sword.\n\nAll Elemental Sword Stone Skills+3,All Status Sword Stone Skills+2\n\nBomb Sword+2, Faster Movement with SnS unsheathed and Fencing.\n\nSnS Tech (Kaiden)\t20\tFencing and Attack 1.1x while wielding a Sword and Shield.\n\nSnS Tech (Expert)\t10\tFencing while wielding a Sword and Shield.\n\nSnS Tech (Novice)\t-10\tAttack 0.8x while wielding a Sword and Shield.\n\nDS Tech\t×\tDS Tech【Master】\tBoth Hiden\tSharpness level +1 is added to DS Tech [Dual Dragon]\n\nDual Dragon\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding Dual Blades\n\nFencing,Recover 3 units of Stamina per hit while attacking in either Demon Mode\n\nFaster activation of Demon Modes.\n\nDS Tech (Kaiden)\t20\tFencing and Attack 1.1x while wielding Dual Swords.\n\nDS Tech (Expert)\t10\tFencing while wielding Dual Swords.\n\nDS Tech (Novice)\t-10\tAttack 0.8x while wielding Dual Swords.\n\nGS Tech\t×\tGS Tech【Master】\tBoth Hiden\tSharpness level +1 is added to GS Tech [Sword King]\n\nSword King\t30\tSuper HG Earplugs,Attack 1.2x when wielding a Great Sword\n\nFencing+1.Guard slash counterblock recovers half of the sharpness that would be lost\n\nfaster charging (stacks with Focus),charge remains at level 4 longer before dropping to level 2 charge(Storm Style)\n\nPerfectly timed blocks cause no knockback, can be evaded out of\n\ndrain no stamina and cause you to recover half of the sharpness that would be lost.\n\nGS Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Great Sword.\n\nGS Tech (Expert)\t10\tFencing while wielding Great Swords.\n\nGS Tech (Novice)\t-10\tAttack 0.8x while wielding Great Swords.\n\nLS Tech\t×\tLS Tech【Master】\tBoth Hiden\tSharpness level +1 is added to LS Tech [Katana God]\n\nKatana God\t30\tSuper High-Grade Earplugs,Attack x1.2when wielding a Long Sword\n\nFencing, Spirit Bar Consumption Halved, Sharp Sword+2 while Spirit Bar is full\n\nand Attack x1.25(originally x1.15)while the Spirit Bar is glowing (after being filled completely).\n\nLS Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Long Sword.\n\nLS Tech (Expert)\t10\tFencing when wielding a Long Sword.\n\nLS Tech (Novice)\t-10\tAttack x0.8 when wielding a Long Sword.\n\nHammer Tech\t×\tHammer Tech【Master B.Beast】\tBoth Hiden\t+1 sharpness level is added to Hammer Tech [Blunt Beast]\n\nBlunt Beast\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Hammer\n\nFencing and Attack x1.3 when releasing a perfectly timed charge attack for that entire combo\n\n(includes infinite).\n\nHammer Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Hammer.\n\nHammer Tech (Expert)\t10\tFencing when wielding a Hammer.\n\nHammer Tech (Novice)\t-10\tAttack x0.8 when wielding a Hammer.\n\nHH Tech\t×\tHH Tech【Master】\tBoth Hiden\tSharpness level +1 is added to HH Tech [Flamboyant Emporer]\n\nFlamboyant Emperor\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Hunting Horn\n\nFencing,Performance mode Note Color decision is made 34% faster.\n\nHH Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Hunting Horn.\n\nHH Tech (Expert)\t10\tFencing when wielding a Hunting Horn.\n\nHH Tech (Novice)\t-10\tAttack x0.8 when wielding a Hunting Horn.\n\nLance Tech\t×\tLance Tech【Master】\tBoth Hiden\tSharpness level +1 is added to Lance Tech [Heavenly Spear]\n\nHeavenly Spear\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Lance,Fencing\n\n0 Damage when blocking against all attacks,Perform 4 hops instead of 3\n\nMotion value +10 for final (3rd or 4th) Normal,Diagonal and Sky-Stabs.\n\nLance Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Lance.\n\nLance Tech (Expert)\t10\tFencing when wielding a Lance.\n\nLance Tech (Novice)\t-10\tAttack x0.8 when wielding a Lance.\n\nGL Tech\t×\tGL Tech【Master】\tBoth Hiden\tSharpness level +1 is added to GL Tech [Cannon Emperor]\n\nCannon Emperor\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Gunlance, Fencing\n\nWyvern Fire and Heat Blade Cooldown-time is halved,Heat Blade Activation Time reduced to 3 seconds\n\nNormal Shells +2 Long Shells +1and Wide Shells +1, stackable with Load Up\n\nGL Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Gunlance.\n\nGL Tech (Expert)\t10\tFencing when wielding a Gunlance.\n\nGL Tech (Novice)\t-10\tAttack x0.8 when wielding a Gunlance.\n\nTonfa Tech\t×\tTonfa Tech【Master】\tBoth Hiden\tSharpness level +1 is added to Tonfa Tech [Piercing Phoenix]\n\nPiercing Phoenix\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding Tonfa,Fencing\n\nan additional 6th Combo / EX Meter segment is added.\n\nTonfa Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding Tonfa.\n\nTonfa Tech (Expert)\t10\tFencing when wielding Tonfa.\n\nTonfa Tech (Novice)\t-10\tAttack x0.8 when wielding Tonfa.\n\nSwitch Axe Tech\t×\tSwitch Axe Tech【Master】\tBoth Hiden\tSharpness level +1 is added to Switch Axe Tech [Edge Marshal]\n\nEdge Marshal\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Switch Axe and Fencing.\n\nSuccessfully using certain actions successfully increases attack by 1.05x for a short time (Morphing in Earth Style, Sword Attacks in Heaven Style and Guarding in Storm Style.)\n\nStamina consumption for infinite swing combo is halved, attacking in axe mode increases phial gauge\n\nattacks utilising the phial consume less meter.\n\nSwitch Axe Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Switch Axe.\n\nSwitch Axe Tech (Expert)\t10\tFencing when wielding a Switch Axe.\n\nSwitch Axe Tech (Novice)\t-10\tAttack x0.8 when wielding a Switch Axe.\n\nMagnet Spike Tech\t×\tMagnet Spike Tech【Master】\tBoth Hiden\tSharpness level +1 is added to Magnet Spike Tech [Magnetic Star]\n\nMagnetic Star\t30\tSuper High-Grade Earplugs,Attack x1.2 when wielding a Magnet Spike, Fencing\n\nSuccessful evasive motions boost attack (1.03x) and gauge recovery (1.5x)for 30seconds.\n\nMovement speed up (1.2x). Magnetic target marker doesn't disappear.\n\nMagnet Spike Tech (Kaiden)\t20\tAttack x1.1 and Fencing when wielding a Magnet Spike.\n\nMagnet Spike Tech (Expert)\t10\tFencing when wielding a Magnet Spike.\n\nMagnet Spike Tech (Novice)\t-10\tAttack x0.8 when wielding a Magnet Spike.\n\nFencing\t○\tFencing+2\t20\tGrants ESP (no bouncing) as well as a 2nd hit for each attack.The 2nd hit has has 20% the value of the first\n\nthis includes Raw, Element, Status and Sword Crystals.\n\nEven if you hit twice Sharpness and Sword Crystals will only be decreased as if you had hit once.\n\nFencing+1\t10\tGrants ESP (no bouncing) when attacking.\n\nSword God\t×\tSword God+3\t20\tSharpness+1, Razor Sharp+2, Fencing+2 and Sharpening Artisan in one skill.\n\nSword God+2\t10\tSharpness+1,Razor Sharp+2, Fencing+2in one skill.\n\nSword God+1\t10\tSharpness+1,Razor Sharp+1, Fencing+1in one skill.";
            }
        }

        public string GetGameArmorSkillsGunnerSkills
        {
            get
            {
                return "Gunner Skills\n\nSteady Hand\t×\tSteady Hand+2\t20\tNormal/Rapid Up,Pierce/Pierce Up,Pellet/Spread Up and +5 to weakness value with critical distance.\n\nDoes not stack with the individual up skills, determination, precision, sniper or critical shot.\n\nSteady Hand+1\t10\tNormal/Rapid Up,Pierce/Pierce Up and Pellet/Spread Up. Does not stack with the individual skill versions.\n\nGentle Shot\t×\tGentle Shot+3\t30\tLoad Up & Recoil Reduction+3.\n\nGentle Shot+2\t15\tLoad Up & Recoil Reduction+2.\n\nGentle Shot+1\t10\tLoad Up & Recoil Reduction+1.\n\nNormal Shot Up\t×\tNormal/Rapid Up\t10\tNormal S and Rapid Bow Arrows do x1.1damage.\n\nPierce Shot Up\t×\tPierce/Pierce Up\t10\tPierce Shots and Pierce Bow Arrows do x1.1damage.\n\nPellet Shot Up\t×\tPellet/Spread Up\t10\tPellet Shots and Scatter Bow Arrows do x1.3damage.\n\nNormal Shot Add\t×\tNormal S All\t15\tGrants the ability to use all Normal S ammo.\n\nNormal S Lv1\t10\tLv1 Normal bullets can be used\n\nPierce Shot Add\t×\tPierce S All\t20\tGrants the ability to use all Pierce S ammo.\n\nPierce S Lv1&2 Add\t15\tPierce S Lv1&2 bullets can be used\n\nPierce S Lv1 Add\t10\tPierce S Lv1 bullets can be used\n\nPellet Shot Add\t×\tPierce S All\t20\tGrants the ability to use all Pellet S ammo.\n\nPellet S LV1&2 Add\t15\tPellet S Lv1&2 bullets can be used\n\nPellet S LV1 Add\t10\tPellet S Lv1 bullets can be used\n\nCrag Shot Add\t×\tCrag S All\t20\tGrants the ability to use all Crag S ammo.\n\nCrag S Lv1&Lv2 Add\t15\tCrag S Lv1&2 bullets can be used\n\nCrag S Lv1 Add\t10\tCrag S Lv1 bullets can be used\n\nCluster Shot Add\t×\tCluster S All\t20\tGrants the ability to use all Cluster S ammo.\n\nCluster S Lv1&Lv2 Add\t15\tCluster S Lv1&2 bullets can be used\n\nCluster S Lv1 Add\t10\tCluster S Lv1 bullets can be used\n\nLoading\t×\tLoadingＵＰ\t10\t+1 capacity for Bowgun and Gunlance ammo.Extra charge level for Bows.\n\nReload\t○\tReload Speed+3\t20\tBowguns: Reload Speed increases by 3 levels.\n\nBows: Coatings are automatically loaded when they are selected.\n\nReload Speed+2\t15\tBowguns: Reload Speed increases by 2 levels.\n\nBows: Coating loading time is 75% of default time.\n\nReload Speed+1\t10\tBowguns: Reload Speed increases by 1 level.\n\nBows: Coating loading time is 85% of default time.\n\nReload Speed-1\t-10\tBowguns: Reload Speed decreases by 1 level.\n\nBows: Coating loading time is 110% of default time.\n\nRecoil\t○\tRecoil Reduction+3\t30\tBowguns: Recoil is reduced by 6 levels.\n\nGunlance: Can evade after Shelling and Recoil from Wyvern Fire is reduced by 30 frames.\n\nRecoil Reduction+2\t15\tBowguns: Recoil is reduced by 4 levels.\n\nGunlance: Can evade after Shelling and Recoil from Wyvern Fire is reduced by 30 frames.\n\nRecoil Reduction+1\t10\tBowguns: Recoil is reduced by 2 levels.\n\nGunlance: Can evade after Shelling.\n\nCritical Shot\t×\tCritical Shot+3\t20\tAttack Up (Absolute)(True Raw+50) and Sniper (Less shot deviation, +5 to a body part's weakness when hit by a Normal/Pierce/Crag Shot or Bow Attacks at a properly spaced critical distance).\n\nDoes not stack with other Attack skills or Sniper (e.g.Attack Up portion would be overwritten by Strong Attack+4).\n\nStacks with Thunderclad,Acid Shots,HHSonicBomb Debuff and Point Breakthrough with the resulting value being used for Exploit Weakness.\n\nCritical Shot+2\t15\tAttack Up (Very Large)(True Raw+30) and Sniper (Less shot deviation, +5 to a body part's weakness when hit by a Normal/Pierce/Crag Shot or Bow Attacks at a properly spaced critical distance).\n\nDoes not stack with other Attack skills or Sniper (e.g.Attack Up portion would be overwritten by Strong Attack+4).\n\nStacks with Thunderclad,Acid Shots,HHSonicBomb Debuff and Point Breakthrough with the resulting value being used for Exploit Weakness.\n\nCritical Shot+1\t10\tAttack Up (Large)(True Raw+20) and Sniper (Less shot deviation, +5 to a body part's weakness when hit by a Normal/Pierce/Crag Shot or Bow Attacks at a properly spaced critical distance).\n\nDoes not stack with other Attack skills or Sniper (e.g.Attack Up portion would be overwritten by Strong Attack+4).\n\nStacks with Thunderclad,Acid Shots,HHSonicBomb Debuff and Point Breakthrough with the resulting value being used for Exploit Weakness.\n\nRapid Fire\t×\tRapid Fire\t15\tRapid Fire volleys fire one extra round, when loading bullets that can be rapid fired you will load all bullets.(Doesn't apply to Ultra Rapid Fire).\n\nAuto-Reload\t×\tAuto-Reload\t10\tBowguns: Can shoot without having to reload but receive high recoil for all shots.\n\nBows: Charging time is reduced to 85% of the default speed.\n\nAmmo Combiner\t×\tMaximum Bullets\t10\tWhen combining bullets or coatings you will always get the maximum possible back.\n\nBullet Saver\t×\tSaving Expert\t20\tWhen you fire a shot there is a 11/32(34.3%) chance that it will notconsume a shot or coating\n\nProcesses all bullets individually on compression shots (HBG only, any number from zero to all loaded shots can be saved.)\n\nbut does not work with Ultra Rapid Fire (LBG) or Heat Beams (HBG).\n\nSaving Master\t10\tWhen you fire a shot there is a 7/32(21.8%) chance that it will not consume a shot or coating\n\nAlso works with compression (HBG). Does not work with Ultra Rapid Fire (LBG) and Heat Beam (HBG).\n\nPrecision\t○\tPrecision+2\t20\tShot deviation (bullet drift) is decreased. and adds +5 to a bodypart's weakness (i.e. 35>40)\n\nwhen shooting with Normal/Pierce/Crag Shot or Bow Attacks within Critical Distance.\n\nDeviation Down\t10\tShot deviation (bullet drift) is decreased.\n\nDeviation Up\t-10\tShot deviation (bullet drift) is increased.\n\nPoison Coating Add\t×\tPoison Coating Add\t10\tGrants the ability to use Poison Coatings\n\nPara Coating Add\t×\tPara Coating Add\t10\tGrants the ability to use Para Coatings\n\nSleep Coating Add\t×\tSleep Coating Add\t10\tGrants the ability to use Sleep Coatings\n\nMounting\t×\tMounting+3\t30\tLoad Up & Reload Speed+3.\n\nMounting+2\t20\tLoad Up & Reload Speed+2.\n\nMounting+1\t10\tLoad Up & Reload Speed+1.\n\nSniper\t×\tSniper\t10\tAuto-Reload and +5 to weakness value within critical distance. *Should only be used with Bows.*\n\nSpacing\t×\tSpacing\t10\tAlters critical distance for Normal,Pierce,Pellet and Blunt shots to be lower and further increases damage at critical distance.\n\nIf you consistently attack within this critical distance you gain Movement Speed+2 and Evade Distance Up.\n\nLBG and HBG get increased perfect shot(3 Segments) and compression windows (2 segments).Bowgets lowered charge times that stack with Auto-Reload.\n\nDoes not buff attacks without Critical Distance, is added to the buff for the first half of HBG's Critical Distance.\n\nBuff builds an internal meter that fills in much the same way as Thunder Clad and total hits needed will vary with Attack, Affinity, Shot Type, etc. but meter build up is not reset on being launched etc.\n\nHBG Tech\t×\tHBG Tech【Master】\tBoth Hiden\tHBG Tech [Gun Sage]'s attack power increases by 1.4x\n\nGun Sage\t30\tSuper HG Earplugs, Attack x1.3 when wielding a Heavy Bowgun, Power (value) of Element/Status Ammo x1.2,Evade Distance Up\n\nMelee Attacks and Crag/Clust Shots all do 15 KO Damage,Heat Beam does x1.2damage and Perfectly-Timed Compression will result in more Attack Power for that 1 salvo.\n\nHBG Tech (Kaiden)\t20\tAttack x1.1 and Evade Extender when wielding a Heavy Bowgun.\n\nHBG Tech (Expert)\t10\tEvade Extender when wielding a Heavy Bowgun.\n\nHBG Tech (Novice)\t-10\tAttack power x0.8 when wielding a Heavy Bowgun.\n\nLBG Tech\t×\tLBG Tech【Master】\tBoth Hiden\tIncreases attack power of LBG Tech [Gun Prodigy] by 1.4x\n\nGun Prodigy\t30\tSuper HG Earplugs,Attack 1.3 when wielding a Light Bowgun, Probability of Ammo bouncing off Monsters is reduced\n\ncan consume items while the weapon is unsheathed,Perfect Shot added to Just Shot meter that deals additional damage.\n\nLBG Tech (Kaiden)\t20\tAttack x1.1 and the probability of Ammo bouncing off Monsters is reduced when wielding a Light Bowgun.\n\nLBG Tech (Expert)\t10\tThe probability of Ammo bouncing off Monsters is reduced when wielding a Light Bowgun.\n\nLBG Tech (Novice)\t-10\tAttack 0.8x while wielding a Light Bowgun.\n\nBow Tech\t×\tBow Tech【Master】\tBoth Hiden\tBow Tech[Bow Demon]'s attack power increases by 1.4x\n\nBow Demon\t30\tSuper HG Earplugs,Attack x1.3 when wielding a Bow, Arrows cannot be deflected, PowerCoating Modifier increases to x1.6 for normal Bows and to x1.7for Gou (and upgrades)\n\nEvolution (Raviente) and G-Rank Bows and the Arc-Shot can be executed at Charge Lv2.\n\nBow Tech (Kaiden)\t20\tAttack x1.1 and arrows are no longer deflected when wielding a Bow.\n\nBow Tech (Expert)\t10\tArrows are no longer deflected when wielding a Bow.\n\nBow Tech (Novice)\t-10\tAttack 0.8x while wielding a Bow.";
            }
        }

        public string GetGameArmorSkillsResistanceSkills
        {
            get
            {
                return "Resistance Skills\n\nAll Res Up\t×\tAll Res+20\t20\tAll resistance values increase by 20\n\nAll Res Up+10\t15\tAll resistance values increase by 10\n\nAll Res Up+5\t10\tAll resistance values increase by 5\n\nAll Res-5\t-10\tAll resistance values decrease by 5\n\nAll Res-10\t-15\tAll resistance values decrease by 10\n\nAll Res-20\t-20\tAll resistance values decrease by 20\n\nFire Res\t○\tFire Res+30\t20\tFire res values increase by 30\n\nFire Res+20\t15\tFire res values increase by 20\n\nFire Res+10\t10\tFire res values increase by 10\n\nFire Res-10\t-10\tFire res values decrease by 10\n\nFire Res-20\t-15\tFire res values decrease by 20\n\nFire Res-30\t-20\tFire res values decrease by 30\n\nWater Res\t○\tWater Res+30\t20\tWater res values increase by 30\n\nWater Res+20\t15\tWater res values increase by 20\n\nWater Res+10\t10\tWater res values increase by 10\n\nWater Res-10\t-10\tWater res values decrease by 10\n\nWater Res-20\t-15\tWater res values decrease by 20\n\nWater Res-30\t-20\tWater res values decrease by 30\n\nIce Res\t○\tIce Res+30\t20\tIce res values increase by 30\n\nIce Res+20\t15\tIce res values increase by 20\n\nIce Res+10\t10\tIce res values increase by 10\n\nIce Res-10\t-10\tIce res values decrease by 10\n\nIce Res-20\t-15\tIce res values decrease by 20\n\nIce Res-30\t-20\tIce res values decrease by 30\n\nThunder Res\t○\tThunder Res+30\t20\tThunder res values increase by 30\n\nThunder Res+20\t15\tThunder res values increase by 20\n\nThunder Res+10\t10\tThunder res values increase by 10\n\nThunder Res-10\t-10\tThunder res values decrease by 10\n\nThunder Res-20\t-15\tThunder res values decrease by 20\n\nThunder Res-30\t-20\tThunder res values decrease by 30\n\nDragon Res\t○\tDragon Res+30\t20\tDragon res values increase by 30\n\nDragon Res+20\t15\tDragon res values increase by 20\n\nDragon Res+10\t10\tDragon res values increase by 10\n\nDragon Res-10\t-10\tDragon res values decrease by 10\n\nDragon Res-20\t-15\tDragon res values decrease by 20\n\nDragon Res-30\t-20\tDragon res values decrease by 30";
            }
        }

        public string GetGameArmorSkillsAbnormalStatusSkills
        {
            get
            {
                return "Abnormal Status Skills\n\nPoison\t○\tNegate Poison\t20\tImmunity to Poison.\n\nPoison Halved\t10\tPoison duration is halved.\n\nDouble Poison\t-10\tPoison duration is doubled.\n\nPara\t○\tNegate Para\t20\tImmunity to Paralysis\n\nPara Halved\t10\tParalysis duration is halved.\n\nPara Doubled\t-10\tParalysis duration is doubled.\n\nSleep\t○\tNegate Sleep\t20\tImmunity to Sleep.\n\nSleep Halved\t10\tSleep duration is halved.\n\nSleep Doubled\t-10\tSleep duration is doubled.\n\nStatus Res\t○\tS. Immunity (Myriad)\t30\tGrants immunity to Poison, Paralysis, Sleep, Stench, Snowman, Chat Disabled, Defense Down, Drunk, Magnetism, Crystallization and Blast.\n\nStatus Immunity\t20\tGrants immunity to Poison,Paralysis and Sleep.\n\nStatus Halved\t10\tHalves Poison,Paralysis and Sleep duration.\n\nStatus Doubled\t-10\tDoubles Poison,Paralysis and Sleep duration.\n\nStun\t○\tNegate Stun\t20\tImmunity to Stun.\n\nStun Halved\t10\tStun duration is halved.\n\nStun Doubled\t-10\tStun duration is doubled.\n\nDeoderant\t×\tDeoderant\t10\tPrevents Stench\n\nSnowball Res\t×\tSnowball Res\t10\tGrants immunity to Snowman.\n\nVocal Chords\t○\tVocal Chord Immunity\t15\tGrants immunity to Chat Disable and Fatigue. Exclusive to Chameleos.\n\nVocal Chord Halved\t10\tHalves chat Disable and Fatigue duration.\n\nDef Lock\t×\tDef Lock\t10\tGrants immunity to Defense Down\n\nSobriety\t×\tHeavy Drinker\t10\tGrants immunity to Drunk status\n\nDrunkard\t-10\tDoubles Drunk duration\n\nBlast Resistance\t×\tBlast Resistance\t10\tGrants immunity to Blast blight.\n\nMagnetic Res\t×\tMagnetic Res\t10\tGrants immunity to Magnetism\n\nMagnet Vulnerability\t-10\tDoubles Magnetism duration\n\nCrystal Res\t×\tCrystal Res\t10\tGrants immunity to Crystallization\n\nCrystal Vulnerability\t-10\tIt takes longer to recover from crystal state.\n\nAlso, crystals are more likely to explode\n\nFreeze Res\t×\tFreeze Resistance\t10\tGrants immunity to Freezing (Toa Tesukatora.)";
            }
        }

        public string GetGameArmorSkillsOtherProtectionSkills
        {
            get
            {
                return "Other Protection Skills\n\nThree Worlds\t×\tUnaffected+3\t20\tSuper High-Grade Earplugs, Violent Wind Breaker and Quake Resistance+2.\n\nUnaffected+2\t15\tHigh-Grade Earplugs,Dragon Wind Breaker and Quake Resistance+1.\n\nUnaffected+1\t10\tEarplugs,Wind Resistance (Large) and Quake Resistance+1.\n\nHearing Res\t○\tSuper HG Earplugs\t25\tProtects against all Monster roars\n\nHigh-Grade Earplugs\t15\tProtects against second tier monster roars\n\nEarplugs\t10\tProtects against first tier monster roars\n\nLowers the length of the flinching duration for second tier roars that aren't blocked.\n\nWind Pressure\t○\tViolent Wind Breaker\t30\tProtects against Violent Wind\n\nDragon Wind Breaker\t20\tProtects against Dragon Wind\n\nWind Res (Large)\t15\tProtects against Large Wind\n\nWind Res (Small)\t10\tProtects against Small Wind\n\nQuake Res\t○\tQuake Res+2\t25\tProtects against large quakes\n\nQuake Res+1\t15\tProtects against small quakes\n\nEvasion\t○\t\n\nInvincible time is usually 6/30 seconds\n\nEvasion+2\t20\tEvasion invulnerability is increased to 12/30 seconds\n\nEvasion+1\t10\tEvasion invulnerability is increased to 10/30 seconds\n\nEvade Distance\t×\tEvade Distance Up\t20\tExtends movement distance for evasion and steps.\n\nEvasion Boost\t×\tEvasion Boost\t15\tGrants the effects of Evasion+2 and Evade Distance Up.\n\nHeat Res\t○\t\n\nNormally, the health gauge will decrease every 2 seconds in the volcano and every 3 seconds in the desert.\n\nSummer Person\t25\tHeat Cancel and Damage Recovery Speed+1 in Hot areas.\n\nHeat Cancel\t15\tGrants immunity to heat(Health loss in Hot areas).\n\nHeat Halved\t10\tThe speed at which Health is lost in Hot areas is halved.\n\nHeat Surge (Small)\t-10\tHealth loss is increased to 1 unit per 1.3 seconds in the Volcano and 1 unit per 2 seconds in the Desert\n\nHeat Surge (Large)\t-20\tHealth loss is increased to 1 unit per 1 second in the Volcano and 1 unit per 1.5 seconds in the Desert\n\nCold Res\t○\t\n\nStamina usually decreases every 6 minutes\n\nWinter General\t25\tCold Cancel, Frost bite Immunity and Marathon Runner effect while in cold areas.\n\nCold Cancel\t15\tGrants immunity to cold (faster stamina loss)\n\nCold Halved\t10\tThe speed at which stamina is lost(hunger)in Cold areas is halved.\n\nCold Surge (Small)\t-10\tThe speed at which stamina is lost in Cold areas is increased x1.5\n\nCold Surge (Large)\t-20\tThe speed at which stamina is lost in Cold areas is increased x2\n\nLight Tread\t×\tLight Tread\t10\tImmunity to monster's Pitfall Traps.\n\nAnti-Theft\t×\tAnti-Theft\t10\tPrevents items being stolen.\n\nTerrain\t○\t\n\nNormally you take a certain amount of damage every 8/30 seconds\n\nHazard Res (Large)\t15\tHealth Loss from Terrain Damage (Lava) and Heat Auras is reduced to 1/3speed.\n\nHazard Res (Small)\t10\tHealth Loss from Terrain Damage (Lava) and Heat Auras is reduced to 2/3speed.\n\nHazard Prone (Small)\t-10\tHealth Loss from Terrain Damage (Lava) and Heat Auras is increased to x1.5 speed.\n\nHazard Prone (Large)\t-15\tHealth Loss from Terrain Damage (Lava) and Heat Auras is increased to x2 speed.\n\nProtection\t○\tGoddess' Embrace\t20\t1/4 chance to receive 0 damage from any attack.\n\nDivine Protection\t10\t1/8 chance to receive 0 damage from any attack.\n\nDemonic Protection\t-10\t1/16 chance to die instantly from any attack.\n\nDeath God's Embrace\t-20\t1/8 chance to die instantly from any attack.\n\nPassive\t×\tPassive\t10\tInvincibility time after getting up from an attack is lengthened 3x.\n\nBreakout\t×\tBreakout\t10\tPanic run speed is increased when below 20% health.\n\nGuts\t○\tTrue Guts\t30\tWhen Health and Stamina are 50 or higher, an otherwise lethal attack will be survived with 1 Health Point left.\n\nGreat Guts\t20\tWhen Health is 70 or higher and Stamina is 50 or higher, an otherwise lethal attack will be survived with 1 Health Point left.\n\nGuts\t10\tWhen Health is 90 or higher and Stamina is 50 or higher, an otherwise lethal attack will be survived with 1 Health Point left.\n\nAbsolute Defense\t×\tAbsolute Defense\t20\tAdds a shield aura that completely negates hits, including all damage and status effects that the hit would cause.\n\nShield vanishes for a set duration after each hit blocked, with the duration before recharging taking longer with each hit taken.\n\nReduces physical damage to around 0.8x while the shield is down";
            }
        }

        public string GetGameArmorSkillsItemComboSkills
        {
            get
            {
                return "Item/Combo Skills\n\nCombo Expert\t×\tCombo Expert+3\t20\tCombo Rate +30% and the effects of Bullet Combination Expert and Health Recovery Items Improved.\n\nCombo Expert+2\t15\tCombo Rate +20% and the effect of Bullet Combination Expert\n\nCombo Expert+1\t10\tCombo Rate +10% and the effect of Bullet Combination Expert\n\nCombo Expert-1\t-10\tCombo Rate -5%\n\nWide-Area\t○\t\n\nEffective when using herbs, healing potions, antidotes, power seeds, armor seeds, flinch-free fruit\n\nWide-Area+3\t30\tMega Potions, Blight Cure Fruits, Zenith Espinas Antitoxin and Crimson Raviente Blood affect allies in the same area\n\nas well as the items covered in Wide-Area+2.\n\nWide-Area+2\t20\tHerbs, Potions, Antidotes, Cool Drinks, Hot Drinks, Armor Seed and Power Seed affects allies in the same area.\n\nWide-Area+1\t10\tHerbs, Potions, Antidotes, Cool Drinks, Hot Drinks, Armor Seed and Power Seed affects allies in the same area\n\nwith 50% effectiveness.\n\nWide-Area-1\t-10\tThe Player cannot be healed through Wide-Range Recovery\n\nEverlasting\t×\tItem Duration Up\t10\tItem effect duration is increased to x1.5\n\nItem Duration DOWN\t-10\tItem effect duration is decreased x0.67.\n\nWhim\t○\t\n\nProbability of item breaking\n\nOld Pickaxe：1/3　Iron Pickaxe：1/10　Mega Pickaxe：1/15　Pickaxe G：1/17\n\nOld Bugnet　：1/3　Bugnet　：1/10　Mega Bugnet　：1/15　Bugnet G：1/17\n\nHealth Flute：1/12　Antidote Flute：1/12　Demon Flute：1/ 8　Armour Flute：1/8　Flute：1/5\n\nShot Flute：1/16　Assault Flute：1/16　Tail Flute：1/12　Deadly Flute：1/8\n\nSpecial Flute：1/16　Rage Flute：1/16　Wrath Flute：1/ 8\n\nDivine Whim\t15\tDecreases chance of breaking Pickaxes,Bug Nets,Horns, and Boomerangs by 50%.\n\nSpirit's Whim\t10\tDecreases chance of breaking Pickaxes,Bug Nets,Horns, and Boomerangs by 25%.\n\nSpectre's Whim\t-10\tIncreases chance of breaking Pickaxes,Bug Nets,Horns, and Boomerangs by 25%.\n\nDevil's Whim\t-15\tIncreases chance of breaking Pickaxes,Bug Nets,Horns, and Boomerangs by 50%.\n\nHunter\t×\tHunter Valhalla\t20\tAlways shows the map and any large monsters location.Easier to fish and BBQ meat.\n\nHunter Life\t10\tAlways shows the map.Easier to fish and BBQ meat.\n\nStrong Arm\t○\tStrong Arm+2\t20\tThrown items do x1.3damage.\n\nStrong Arm+1\t10\tThrown items do x1.1damage.\n\nThrowing\t×\tThrowing Distance UP\t10\tThrowing Distance of Throwing Items is increased and the probability of losing Boomerangs is decreased to 1/8.\n\nCooking\t○\tBBQ Master\t15\tFish and Meat remain Brown (ready)for much longer.\n\nBBQ Expert\t10\tFish and Meat remain Brown (ready)longer.\n\nFalse BBQ Expert\t-10\tFish and Meat remain Brown (ready)for half the time of the usual.\n\nFishing\t×\tFishing Expert\t10\tFish always bite on their first attempt.\n\nCombining\t○\tCombining+30%\t20\tCombo Rate +30%\n\nCombination+15%\t15\tCombo Rate +15%\n\nCombination+10%\t10\tCombo Rate +10%\n\nCombination-5%\t-10\tCombo Rate -5%\n\nAlchemy\t×\tAlchemy\t10\tGrants Alchemy Combinations (alternative item combinations).\n\nEncourage\t×\tEncourage+2\t20\tHorn Maestro.Party wide Evasion+2 and Stun Negate.\n\nEncourage+1\t10\tHorn Maestro.Party wide Evasion+1 and Stun Halved.\n\nFlute Expert\t×\tFlute Expert\t10\t50% less chance of horns breaking. All horn duration 1.5x (including Hunting Horn songs).\n\nSpeed Setup\t○\tTrap Master\t20\tTrap Setup Speed is lowered to x0.66. 100% combine rate for traps, Mocha Pots and Trap-related items.\n\nTrap Expert\t10\tTrap Setup Speed is lowered to x0.8. 100% combine rate for traps, Mocha Pots and Trap-related items.\n\nIron Arm\t×\tIron Arm+2\t20\tThrowing Distance Up, Strong Arm+2,Throwing Knives+2. Increases blocking duration on Great Sword Guard Slash (Heaven and Storm Styles)\n\nLance Shield Rush (Storm and Extreme Styles), and Tonfa's Standard 1, Standard 3, Standard 4,Aerial 1-3, Continuous Thrust 2 and Dash Tonfa Rotatation.\n\nIron Arm+1\t10\tThrowing Distance Up, Strong Arm+1,Throwing Knives+1. Increases blocking frames duration on Great Sword Guard Slash (Heaven and Storm Styles)\n\nLance Shield Rush (Storm and Extreme Styles), and Tonfa's Standard 1, Standard 3, Standard 4,Aerial 1-3, Continuous Thrust 2 and Dash Tonfa Rotatation.\n\nKnife Throwing\t○\tThrowing Knives+2\t20\tThrow 5K nives at once instead of 1. Only consumes 1.\n\nThrowing Knives+1\t10\tThrow 3 Knives at once instead of 1. Only consumes 1.";
            }
        }

        public string GetGameArmorSkillsMapDetectionSkills
        {
            get
            {
                return "Map/Detection Skills\n\nMap\t×\tMap\t10\tAlways display the map.\n\nPsychic\t○\tAutotracker\t15\tMonster locations and detailed icon are always shown on the map.\n\nDetect\t10\tMarked monsters are shown as a detailed icon rather than a round dot on the map.\n\nStealth\t×\tSneak\t10\tMonsters are less likely to target you.\n\nTaunt\t-10\tMonsters are more likely to target you.\n\nIncitement\t×\tIncitement\t10\tAttacking a monsterwill force its attention towards you rather than your fellowHunters.Damage received from the monster will also be reduced during this time.The yellow eye icon indicating you were spotted will turn Red when the skill is in effect.\n\nGoing outside of the monster's range for too long will undo the effect prematurely,the skill has a 30 second cooldown before it can be activated again.\n\nWhile being targetted you gain +40 True Raw.";
            }
        }

        public string GetGameArmorSkillsGatheringTransportSkills
        {
            get
            {
                return "Gathering/Transport Skills\n\nBackpacking\t×\tPro Transporter\t10\tIncreases speed when carrying heavy items such as eggs, powderstone, soothstone, etc.\n\nAlso be able to fall slightly higher altitudes without dropping the item.\n\nGathering Speed\t×\tHigh Speed Gathering\t10\tIncreases gathering speed.\n\nGathering\t○\t\n\nNormally, the probability of gathering after the second time is 27/32\n\nGathering+2\t15\tIncreases the chance of collecting and excavating from the second time onwards to 31/32.\n\nGathering+1\t10\tIncreases the chance of collecting and excavating from the second time onwards to 29/32.\n\nGathering-1\t-10\tIncreases the chance of collecting and excavating from the second time onwards to 23/32.\n\nGathering-2\t-15\tIncreases the chance of collecting and excavating from the second time onwards to 19/32.\n\nCarving\t×\tCarving Expert\t15\tNumberof carves increased by 1\n\nMindfulness\t○\tImperturbable\t15\tWhile using an item or gathering, the Hunter can't be interrupted by any attack.\n\nFully Prepared\t10\tWhile using an item or gathering, the Hunter can't be interrupted by other hunters or small monsters.\n\nNegligence\t-10\tDamage taken from monsters is increased when taking a hit while using an item or gathering.";
            }
        }

        public string GetGameArmorSkillsRewardSkills
        {
            get
            {
                return "Reward Skills\n\nFate\t○\t\n\nUsually, the probability of increasing the number of slots is 22/32\n\nGreat Luck\t20\tGreatly increases chance for standard quest rewards. 29/32.\n\nGood Luck\t10\tIncreases chance for standard quest rewards 26/32.\n\nBad Luck\t-10\tDecreases chance for standard quest rewards 16/32.\n\nHorrible Luck\t-20\tGreatly Decreases chance for standard quest rewards. 8/32.\n\nMonster\t×\tCome on Big Guy!\t10\tIncreases the chances of larger boss monster spawns.?\n\nPressure\t○\tPressure【Large】\t20\tMonster Bounty randomly increases by 30, 50, 75, 100 or 150%.\n\nPressure (Small)\t10\tMonster Bounty increases by 30%.";
            }
        }

        public string GetGameArmorSkillsOtherSkills
        {
            get
            {
                return "Other Skills\n\nBreeder\t×\tBreeder\t10\tPoogie item drops at the Pugi Farm become more common. Halks also drop items more often.\n\nBond\t×\tBond\t10\tWhen another Hunter of the opposite gender is present in a quest:\n\nMale Player: Attack +5.\n\nFemale Player: Defense +40.\n\nInspiration\t×\tInspiration\t10\tVarious effects are triggered at random when starting a quest.\n\nSkill\tMessage\tChance\n\nAll Res＋５\t各耐性に強くなった！\t30%\n\nHeat & Cold Res\t気候の変化に強くなった！\t16%\n\nNegate Stun\t気絶しなくなった！\t16%\n\nTaunt\tﾓﾝｽﾀｰに狙われやすくなった…\t15%\n\nSpeed Eating\t早食いになった！\t12%\n\nAll Res-20\t各耐性に弱くなった…\t　7%\n\nStatus Immunity\t状態異常に強くなった！\t　4%\n\nRelief\t×\tRelief\t10\tRastas, Fostas, Partners and Halks recover 50% faster after disappearing.\n\nLegendary Rastas do not leave and thus are not affected.\n\nCapture Proficiency\t○\tCapture Guru\t20\tMonsters will blink on the map when they can be captured.\n\nCapture Proficiency\t10\tChance of a captured Monster becoming a pet increases by 30%.\n\nCaring\t×\tCaring+3\t45\tAll Quests: Human Players cannot be interrupted by your attacks nor can they interrupt you.\n\nAll Quests: NPCs cannot be interrupted by Human Players and will roll when hit instead.\n\nCaring+2\t25\tNon-G Rank Quests: Human Players cannot be interrupted by your attacks nor can they interrupt you.\n\nAll Quests: NPCs cannot be interrupted by Human Players and will roll when hitinstead.\n\nCaring+1\t10\tAll Quests: NPCs cannot be interrupted by Human Players and will roll when hit instead.\n\nMovement Speed\t×\tMovement Speed ＵＰ+2\t20\tMovement speed with weapon sheathed is increased slightly further.\n\nMovement Speed ＵＰ+1\t10\tMovement speed with weapon sheathed is increased slightly.\n\nReinforcement\t×\tRed Soul\t10\tOwn Attack +15. Hitting another player will increase their Attack +30.\n\n※Hitting a player who is under the effect of Blue Soul\n\n※When hit by a player who has the Blue Soul skill you will be able to KO monsters with any weapon when hitting their head and your own Attack rises by +30. All effects last 2 minutes.\n\n※Attack is a final addition that is always the stated value and completely unaffected by other skills and multipliers.\n\nBlue Soul\t-10\tOwn Defense +50. Hitting another player will increase their Defense +100.\n\n※Hitting a player who is under the effect of Blue Soul while he/she is under any status ailment in the game will dispel the effect.\n\n※When hit by a player who has the Red Soul skill active will increase own Defense by +100 and grant Goddess' Embrace effect.\n\nAll effects last 2minutes.\n\nAssistance\t×\tAssistance\t10\tThe player's arm will glow red and grants +20 Attack, +50 Defense, Damage Recovery Speed+2, Status Immunity and Peerless to nearby Hunters.\n\nEffective radius is 3 rolls or 2 with Evade Distance Up. Affected players have their arms glow yellow.\n\n※The player with the skill does get +20 Attack and +50 Defense but does not benefit from Peerless, Status Immunity or Damage Recovery Speed.\n\n※The skills overwrite lower tier versions if any affected hunters have them. For example a hunter with Status Halved would gain Status Immunity within radius while a hunter with Status\n\nImmunity (Myriad) would retain their version of the skill.\n\n※Attack is a final addition thatis always the stated value and completely unaffected by other skills and multipliers.\n\nGrace\t×\tGrace+3\t30\tWhen there are only 3 or fewer active skills: Attack Up (Absolute), Issen+3, High-Grade Earplugs, WindRes (Large),Quake Res+1, Evasion+1,Guard+1,Goddess' Embrace and Weapon Handling will be active.\n\nGrace+2\t20\tWhen there are only 2 or fewer active skills: Attack Up (Absolute), Issen+3, High-Grade Earplugs, WindRes (Large),Quake Res+1, Evasion+1,Guard+1,Goddess' Embrace and Weapon Handling will be active.\n\nGrace+1\t10\tWhen there is only 1 active skill: Attack Up (Absolute), Issen+3, High-Grade Earplugs, WindRes (Large),Quake Res+1, Evasion+1,Guard+1,Goddess' Embrace and Weapon Handling will be active.\n\nCompensation\t×\tCompensation\t10\tDeath God's Embrace, Sharpness+1, Attack Up (Absolute), Evasion+2 and Critical Eye +4.\n\nDark Pulse\t×\tDark Finale\t20\tUpon health dropping to 0, you will rise again, gaining full Health and Stamina bars and Heavy Buffs but will also start constantly losing health at a steady rate.\n\nHealth loss cannot be halted by any methods and you will cart after exactly one minute has passed. Although the health degeneration cannot be stopped by any standard method, completing a quest will disable health loss meaning any final carts will not occur.\n\nIn your risen form you are buffed with the following skills: Adrenaline+2, Starving Wolf+2, Defense+120, Sharpening Artisan, Super HG Earplugs, Violent Wind Breaker, TremorRes +2, Heat Cancel and Cold Cancel.\n\nAlongside these skills you will gain a Super Armour effect that grants immunity to all forms of knock back.\n\nBlazing Grace\t×\tBlazing Majesty+2\t15\tAdrenaline+2, Red Soul, Bombardier, Fire Res+30, Artillery God, Summer Person, Terrain Damage Decreased (Large)\n\nFire Attack Up (Large), Flame Sword+3 and Bomb Sword+3 combined into 1 skill.\n\nBlazing Majesty+1\t10\tAdrenaline+1, Red Soul, Bombardier, Fire Res+20, Artillery Expert, Heat Cancel, Terrain Damage Decreased (Small)\n\nFire Attack Up (Small), Flame Sword+2 and Bomb Sword+2 combined into 1 skill.\n\nDrawing Arts\t×\tDrawing Arts+2\t20\tWhile weapon unsheathed: Peerless, Evasion+2, Weapon Handling\n\nWhile weapon is sheathed: Damage Recovery Speed+2, Quick Stamina Recovery (Large)\n\nDrawing Arts+1\t10\tWhile weapon unsheathed: Marathon Runner, Evasion+1, Weapon Handling\n\nWhile weapon is sheathed: Damage Recovery Speed+1, Quick Stamina Recovery (Small)\n\nIce Age\t×\tIce Age\t10\tUpon attacking a monster the hunter is surrounded by an icy aura. This aura deals damage to all monsters in its area and grants a number of different skills.This aura has three stages and will progress with more hits.\n\nThe aura also grants Stamina Recovery Up and Sharp Sword to all hunters affected by the aura and the one with the skill also gets Winter General.\n\nDamage is dealt once every second fixed rather than over time.";
            }
        }

        public string GetGameCaravanInfo
        {
            get
            {
                return string.Format("If your caravan gem is below level 8 you will have less pages. The skills will however be in the same order.\n\n"+
                    "Page 1\n"+
                    "Cafeteria Regular\n"+
                    "Chance to not consume food when preparing buffs for a quest.\n\n"+
                    "Negotiation\n"+
                    "1/8th chance to get a 10%/15%/25% discount on buying things.\n\n"
                    +"My Tore Celebrity\n"+
                    "Garden managers affection goes up 1.5x/2x/3x usual values.\n\n"
                    +"Gallery Celebrity\n"+
                    "5000/7000/10000 extra Gallery Points on evaluations.\n\n"
                    +"Garden Celebrity\n"+
                    "1.2x/1.3x/1.5x items received from garden tools.\n\n"+
                    "Recovery Items Up\n"+
                    "Herb, Potion, Mega Potion and Lifepowder effect 1.1x. 100% Bitterbug and Antidote Herb effectiveness.\n\n"+
                    "Blunt Striker\n"+
                    "Bowgun Melee damage up (3.0x).\n\n"+
                    "Courage\n"+
                    "No inching upon being spotted by monsters.\n\n"+
                    "Combination Technique [Small]\n"+
                    "10% additional combination success rate.\n\n"+
                    "Riser [Small]\n"+
                    "1.5x iframes during the rising animation after taking a hit.\n\n"+
                    "Page 2\n"+
                    "Perfect Defense [Small]\n"+
                    "Blocking within 3 frames of an attack hitting you will cause no stamina or sharpness decrease and prevent knockback and allow you to immediately evade after the block.\n\n"+
                    "Lander\n"+
                    "No recovery time after falling, no egg loss on falling.\n\n"+
                    "Vine Superhero\n"+
                    "No stamina is consumed while climbing.\n\n"+
                    "Vine Master\n"+
                    "Getting hurt while climbing will not cause you to fall\n\n"+
                    "Art of Dancing\n"+
                    "Using the 'Dance' action will give +10 attack for one minute. Uses the same buff slot as Power Seeds etc.\n\n"+
                    "Combination Celebrity\n"+
                    "Combining items has a chance to produce double the usual results quantity wise.\n\n"+
                    "Combination Technique [Medium]\n"+
                    "15% additional combination success rate.\n\n"+
                    "Riser [Medium]\n"+
                    "2.0x iframes during the rising animation after taking a hit.\n\n"+
                    "Perfect Defense [Medium]\n"+
                    "Blocking within 4 frames of an attack hitting you will cause no stamina or sharpness loss and prevent knockback and allow you to immediately evade after the block.\n\n"+
                    "Elite Flame\n"+
                    "Increases the Friendly Fire (heat up) meter over time instead of by friendly fire when on caravan quests.\n\n"+
                    "Page 3\n"+
                    "Mine Expert\n"+
                    "Pickaxes are less likely to break after use.\n\n"+
                    "Insect Expert\n"+
                    "Bug nets are less likely to break after use.\n\n"+
                    "(Recommended) KO Technique\n+" +
                    "Increases stun damage dealt by 1.1x. Stacks with Sigil.\n\n"+
                    "Combination Technique [Large]\n"+
                    "20% additional combination success rate.\n\n"+
                    "Riser [Large]\n"+
                    "2.0x iframes during the rising animation after taking a hit.\n\n"+
                    "Secret Healing Technique [Small]\n"+
                    "1/12th chance of not consuming healing items when used. (Up to 5 times a quest)\n\n"+
                    "(Recommended) Perfect Defense [Large]\n"+
                    "Blocking within 4 frames of an attack hitting you will cause no stamina or sharpness loss and prevent knockback and allow you to immediately evade after the block. Perfectly timed blocks also cause a Reflect effect which deals 72 motion (no critical hits, no elemental).\n\n"+
                    "Unstable Defender [Small]\n"+
                    "90% reduction of damage and 20% chance of no damage while blocking\n\n"+
                    "Rousing Attacker [Small]\n"+
                    "Attacking a monster while you have 50 or lower health a 40% chance to cause you to regain 10 HP. Cannot trigger more than once every 10 seconds. Can trigger up to 10 times in a quest.\n\n"+
                    "Revenge![Small]\n"+
                    "After getting up from a hit there's a chance (1 x Health Loss % chance) to gain 25 attack, 50 defense and no minor knockback for 20 seconds. Counted as a Power Pill for terms of buff effects and does not overlap\n\n"+
                    "Page 4\n"+
                    "Hot Master\n"+
                    "Grants the effects of Heat Cancel\n\n"+
                    "Cold Master\n"+
                    "Grants the effects of Cold Cancel\n\n"+
                    "Prepared Stance\n"+
                    "If you perform the gesture 應戰準備 <act20> for around 30 seconds the attack ceiling on your currently equipped weapon type increases for a fixed duration.\n\n"+
                    "Shield Angel\n"+
                    "Decreases the amount of damage taken when on support quests on Berserk Raviente.\n\n"+
                    "Spear Angel\n"+
                    "Increases the amount of damage dealt by Ballistae when playing support on Berserk Raviente.\n\n"+
                    "Secret Healing Technique [Medium]\n"+
                    "1/11th chance of not consuming healing items when used. (Up to 5 times a quest)\n\n"+
                    "Unstable Defender [Medium]\n"+
                    "90% reduction of damage and 25% chance of no damage while blocking\n\n"+
                    "Rousing Attacker [Medium]\n"+
                    "Attacking a monster while you have 50 or lower health a 40% chance to cause you to regain 10 HP. Cannot trigger more than once every 10 seconds. Can trigger up to 15 times in a quest.\n\n"+
                    "Revenge! [Medium]\n"+
                    "After getting up from a hit there's a chance (1.5 x Health Loss % chance) to gain 25 attack, 50 defense and no minor knockback for 20 seconds. Counted as a Power Pill for terms of buff effects and does not overlap\n\n"+
                    "Weapon Art [Small]\n"+
                    "Increases the True Raw of your equipped weapon by 1.01x of its base True Raw on all weapon types.\n\n"+
                    "Page 5\n"+
                    "Bonus Art\n"+
                    "Food Effect is not lost after fainting.\n\n"+
                    "Secret Healing Technique [Large]\n"+
                    "1/10th chance of not consuming healing items when used. (Up to 5 times a quest)\n\n"+
                    "Last Minute Ace [Small]\n"+
                    "In the last 5 minutes of a quest you get 80% affinity but take 1.3x damage\n\n"+
                    "Unstable Defender [Large]\n"+
                    "90% reduction of damage and 50% chance of no damage while blocking\n\n"+
                    "Rousing Attacker [Large]\n"+
                    "Attacking a monster while you have 50 or lower health a 40% chance to cause you to regain 10 HP. Cannot trigger more than once every 10 seconds. Can trigger up to 20 times in a quest.\n\n"+
                    "Revenge! [Large]\n"+
                    "After getting up from a hit there's a chance (2 x Health Loss % chance) to gain 25 attack, 50 defense and no minor knockback for 20 seconds. Counted as a Power Pill for terms of buff effects and does not overlap\n\n"+
                    "Last Minute Ace [Medium]\n"+
                    "In the last 5 minutes of a quest you get 90% affinity but take 1.3x damage\n\n"+
                    "Weapon Art [Medium]\n"+
                    "Increases the True Raw of your equipped weapon by 1.025x of its base True Raw on all weapon types.\n\n"+
                    "Wild Awakening\n"+
                    "Combination of both Hot and Cold Master skills.\n\n"+
                    "Instant Guard Stance\n"+
                    "Combination of Weapon Art [Med] and Perfect Defense [Med]\n\n"+
                    "Page 6\n"+
                    "(Recommended) Shooting Rampage\n"+
                    "Increases the True Raw of your equipped ranged weapon by 1.1x of its base True Raw. If using a bowguns your accuracy immediately after shooting is lowered by 1.5x.\n\n"+
                    "Natural Recovery [Small]\n"+
                    "Using the 'Sleep' gesture will cause your red health to refill up to 5 times.\n\n"+
                    "Master Carver [Small]\n"+
                    "While carving if you roll the top item in the carve table and it is below 51% you have a 1/10th chance of rerolling with that item removed from the carving pool up to a maximum of 10 times in a quest.\n\n"+
                    "Last Minute Ace [Large]\n"+
                    "In the last 5 minutes of a quest you get 100% affinity but take 1.3x damage\n\n"+
                    "(Recommended) Weapon Art [Large]\n"+
                    "Increases the True Raw of your equipped weapon by 1.05x of its base True Raw on all weapon types.\n\n"+
                    "Decisive Hunter\n"+
                    "Combination of Weapon Art [Med] and KO Technique\n\n"+
                    "Natural Recovery [Medium]\n"+
                    "Using the 'Sleep' gesture will cause your red health to refill up to 10 times.\n\n"+
                    "Master Carver [Medium]\n"+
                    "While carving if you roll the top item in the carve table and it is below 51% you have a 1/9th chance of rerolling with that item removed from the carving pool up to a maximum of 15 times in a quest.\n\n"+
                    "Goddess of Luck [Small]\n"+
                    "1/10th chance to take no damage on up to 5 hits in a quest (Stacks with Divine Protection, Diva Buff and Girly Charms).\n\n"+
                    "Self-Defense\n"+
                    "Combination of Weapon Art [Med] and Unstable Defender [Med]\n\n"+
                    "Page 7\n"+
                    "Natural Recovery [Large]\n"+
                    "Using the 'Sleep' gesture will cause your red health to refill up to 15 times.\n\n"+
                    "(Recommended) Master Carver [Large]\n"+
                    "While carving if you roll the top item in the carve table and it is below 51% you have a 1/8th chance of rerolling with that item removed from the carving pool up to a maximum of 20 times in a quest.\n\n"+
                    "Goddess of Luck [Medium]\n"+
                    "1/9th chance to take no damage on up to 10 hits in a quest (Stacks with Divine Protection, Diva Buff and Girly Charms).\n\n"+
                    "Goddess of Luck [Large]\n"+
                    "1/8th chance to take no damage on up to 20 hits in a quest (Stacks with Divine Protection, Diva Buff and Girly Charms).");
            }
        }

        public string GetGameRoadDureInfo
        {
            get
            {
                return string.Format("The Maximum Cost is 130.\n\n"+
                    "Attack\n"+
                    "Lv1: Small increase to attack.(+10)\n"+
                    "Lv2: Increases attack.(+20)\n"+
                    "Lv3: Medium increase to attack.(+30)\n"+
                    "Lv4: Large increase to attack.(+50)\n"+
                    "Lv5: Very large increase to attack.(+70)\n\n"+
                    "Defense\n"+
                    "Lv1: Defense +30\n"+
                    "Lv2: Defense +50\n"+
                    "Lv3: Defense +80\n"+
                    "Lv4: Defense +110\n"+
                    "Lv5: Defense +150\n\n"+
                    "Health Recovery\n"+
                    "Lv1: Red Health recovery speed is increased.\n"+
                    "Lv2: Red health recovery speed is further increased.\n\n"+
                    "Fire Res\n"+
                    "Lv1: Fire Res +10\n"+
                    "Lv2: Fire Res +15\n"+
                    "Lv3: Fire Res +25\n\n"+
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
                    "All Res\n"+
                    "Lv1: All Res +5\n"+
                    "Lv2: All Res +10\n"+
                    "Lv3: All Res +15\n\n"+
                    "Technical Skills\n"+
                    "Starting Gm Up\n"+
                    "Lv1: Increases starting Gm.(+1000Gm)\n"+
                    "Lv2: Increases starting Gm significantly.(+3000Gm)\n\n"+
                    "Hunting Road Points Up\n"+
                    "Lv1: Increases Road Point earning slightly (+10%).\n"+
                    "Lv2: Increases Road Point earning (+20%).\n"+
                    "Lv3: Increases Road Point earning greatly (+40%).\n\n"+
                    "Bonus Stages Up\n"+
                    "Lv1: Increases the likelihood of getting Bonus Stages slightly. Stacks with other players on road.\n"+
                    "Lv2: Increases the likelihood of getting Bonus Stages. Stacks with other players on road.\n\n" +
                    "Resurrection Knowledge\n"+
                    "Lv1: Increases the number of times you can faint on the road before failing (+1 Cart).\n\n"+
                    "Advancement Knowledge\n"+
                    "Lv1: Attack increases every 5 stages on the Road. Floor 6 will give +20 Attack while every 5th floor after will grant +10 Attack stopping after floor 26. Maximum buff of 60 Attack for all floors above 26.\n"+
                    "Lv2: Attack increases every 5 stages on the Road. Floor 6 will give +40 Attack while every 5th floor after will grant +10 Attack stopping after floor 26. Maximum buff of 80 Attack for all floors above 26.\n"+
                    "Lv3: Attack increases every 5 stages on the Road. Floor 6 will give +60 Attack while every 5th floor after will grant +10 Attack stopping after floor 26. Maximum buff of 100 Attack for all floors above 26.\n\n"+
                    "Last Stand\n"+
                    "Lv1: Increases affinity by +30% and Attack by +80 but causes you to have a single faint on Road regardless of other skills.\n"+
                    "Lv2: Increases affinity by +50% and Attack by +120 but causes you to have a single faint on road regardless of other skills.\n\n"+
                    "Duremudira Skills\n"+
                    "Care\n"+
                    "Lv1: Slightly increases speed of revivals and the amount of health left after being revived.\n"+
                    "Lv2: Increases speed of revivals and the amount of health left after being revived.\n"+
                    "Lv3: Greatly increases speed of revivals and the amount of health left after being revived.\n\n"+
                    "Pharmacist\n"+
                    "Lv1: Increases the number of revival items you can carry by 1.\n"+
                    "Lv2: Increases the number of revival items you can carry by 2.\n"+
                    "Lv3: Increases the number of revival items you can carry by 4.\n\n"+
                    "Virus Protection\n"+
                    "Lv1: Increases resistance to Deadly Poison slightly.\n"+
                    "Lv2: Increases resistance to Deadly Poison.\n"+
                    "Lv3: Increases resistance to Deadly Poison greatly.\n\n"+
                    "Frost Protection\n"+
                    "Lv1: Increases resistance to Powerful Frost slightly.\n"+
                    "Lv2: Increases resistance to Powerful Frost.\n"+
                    "Lv3: Increases resistance to Powerful Frost greatly.\n\n"+
                    "Gatekeeper Offensive\n"+
                    "Lv1: Increases attack when facing Duremudira by +50 true raw.\n"+
                    "Lv2: Increases attack when facing Duremudira by +75 true raw.\n"+
                    "Lv3: Increases attack when facing Duremudira by +100 true raw.\n"+
                    "Lv4: Increases attack when facing Duremudira by +150 true raw.\n"+
                    "Lv5: Increases attack when facing Duremudira by +200 true raw.\n\n"+
                    "Gatekeeper Defensive\n"+
                    "Lv1: Increases defense when facing Duremudira.\n"+
                    "Lv2: Increases defense when facing Duremudira.\n"+
                    "Lv3: Increases defense when facing Duremudira.\n"+
                    "Lv4: Increases defense when facing Duremudira.\n"+
                    "Lv5: Increases defense when facing Duremudira.");
            }
        }

        public string GetGameZenithSkills
        {
            get
            {
                return string.Format("Skill Slots Up\n"+
                    "Available Skill Slots go up by 1/2/3/4/5/6/7. Stacks with the +1 or +2 skill slots from having 3 or 5 standard G Rank pieces (including Z, ZF, ZY, ZX and ZP)\n\n"+
                    "Flash Conversion\n"+
                    "Flash Conversion Up+1: Increases attack based on your weapon's natural affinity (5 x √Base Affinity, always rounded down before addition). Example: 50% base affinity = 5 x √50 = 7.07 x 10 = 35. This only uses the true base affinity of your weapon. Sigils, Skills, SR Skills and the +5-10% from having above blue sharpness do not count towards the increase. (The sharpness bonus is always displayed\n"+
                    "Flash Conversion Up+2: Increases attack based on your weapon's natural affinity (10 x √Base Affinity, always rounded down before addition). Example: 50% base affinity = 10 x √50 = 7.07 x 10 = 70. This only uses the true base affinity of your weapon. Sigils, Skills, SR Skills and the +5-10% from having above blue sharpness do not count towards the increase. (The sharpness bonus is always displayed\n\n"+
                    "Stylish Assault\n"+
                    "(Recommended) Stylish Assault Up+1: Boosts Attack by +20 per evade up to a maximum of +220. The duration of the buff resets every evasion. Your attack being overbuffed is indicated by the aura becoming yellow. Synergizes well with Stylish Up.\n"+
                    "Stylish Assault Up+2: Boosts Attack by +40 per evade up to a maximum of +220. The duration of the buff resets every evasion. Your attack being overbuffed is indicated by the aura becoming yellow. Synergizes well with Stylish Up.\n\n"+
                    "Dissolver Up\n" +
                    "Lowers the Elemental Hitbox requirements for Dissolver. (15 Weakness down from 20). Unnecessary with Solid Determination.\n\n"+
                    "Thunder Clad\n"+
                    "Thunder Clad Up+1: Increases the active duration of Thunder Clad to 80 seconds (+20)\n"+
                    "Thunder Clad Up+2: Increases the active duration of Thunder Clad to 120 seconds (+60)\n\n"+
                    "Ice Age Up\n"+
                    "Increases the maximum duration that the Ice Age aura can stay at a stage by +3 seconds. Only affects top tier stage (i.e. Stage 3 is 9>12 seconds but if it hits stage 2 that is 10 seconds for decay, not 13).\n\n"+
                    "Hearing Protection\n"+
                    "Hearing Protection Up+1: Increases Hearing Protection by one tier, allows blocking of Ultra Roars with Super High-Grade Earplugs.\n"+
                    "Hearing Protection Up+2: Increases Hearing Protection by two tiers, allows blocking of Ultra Roars with High-Grade Earplugs.\n" +
                    "Hearing Protection Up+3: Allows blocking of Ultra Roars with Earplugs.\n\n"+
                    "Wind Res Up\n"+
                    "Wind Res Up+1: Increases Wind Res by 1 tier, allows blocking of Ultra Wind Pressure with Violent Wind Breaker\n"+
                    "Wind Res Up+2: Increases Wind Res by 2 tiers, allows blocking of Ultra Wind Pressure with Dragon Wind Breaker\n" +
                    "Wind Res Up+3: Increases Wind Res by 3 tiers, allows blocking of Ultra Wind Pressure with Wind Res (Large)\n" +
                    "Wind Res Up+4: Allows blocking of Ultra Wind Pressure with Wind Res (Small)\n\n"+
                    "Quake Res\n"+
                    "Quake Res Up+1: Increases Quake Res by 1 tier, allows blocking of Ultra Tremor with Quake Res+1\n"+
                    "Quake Res Up+2: Allows blocking of Ultra Tremor with Quake Res+1.\n\n"+
                    "Poison Res\n"+
                    "Poison Res Up+1: Upgrades Poison Res by 1 tier, enables Super Poison Damage Halved with Poison Immunity. There is no immunity for Super Poison. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n"+
                    "Poison Res Up+2: Super Poison Damage Halved. There is no immunity for Super Poison. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n\n"+
                    "Paralysis Res\n" +
                    "Paralysis Res Up+1: Upgrades Paralysis Res by 1 tier, enables Super Paralysis Halved with Paralysis Immunity. There is no immunity for Super Paralysis. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n"+
                    "Paralysis Res Up+2: Super Paralysis Halved. There is no immunity for Super Paralysis. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n\n"+
                    "Sleep Res\n" +
                    "Sleep Res Up+1: Upgrades Sleep Res by 1 tier, enables Super Sleep Halved with Sleep Immunity. There is no immunity for Super Sleep. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n"+
                    "Sleep Res Up+2: Super Sleep Halved. There is no immunity for Super Sleep. Increases level of the hybrid skill Status Immunity but does not increase the skill granted by Assistance or Thunder Clad.\n\n"+
                    "Vampirism\n"+
                    "Vampirism Up: Increases level of Vampirism by one. Increases leech rate to 100% if you already have Vampirism+2. Leeching with Vampirism+3 regains the same amount of health as Vampirism+2, the buff is purely to the leech chance and is not much of a buff compared to that of increasing Vampirism+1 to +2.\n\n"+
                    "Drug Knowledge\n"+
                    "Drug Knowledge Up: Status weapons always inflict 42% of their original status value (Up from 38%). Does not increase the amount of raw gained from the original status value beyond 1/4th. Normal status infliction rate is 33% chance of 100% status points.\n\n"+
                    "Assistance\n"+
                    "Assistance Up: Party members within the area of effect of Assistance get Status Immunity (Myriad). Immunity skill still only affects other hunters, does not affect the person with the skill active.\n\n"+
                    "Bullet Saver\n"+
                    "Bullet Saver Up+1: Upgrades Bullet Saver by 1 tier. If you have Bullet Saving Master, further increases the rate at which you save bullets or coatings to around 46.4%.\n"+
                    "Bullet Saver Up+2: Further increases the rate at which you save bullets or coatings to around 46.4%.\n\n" +
                    "Guard\n"+
                    "(Recommended for Lance/Gunlance) Guard Up+1: Upgrades Guard by one level. If you already have Guard+2, decreases the amount of knockback, stamina loss and health loss while guarding and increases the size of the guard window. Lances and Gunlances only: Only applies when exceeding Guard+2, Guarding affects 360 degrees around you, rear hits still cause knockdown. Heavy and Ranged Guards can block attacks that were previously unblockable. Lance's Guard Meter fills faster (Extreme Style). Gunlances have lowered Wyvern Fire cooldowns. Synergizes well with Reflect Up, Obscurity Up and Rush Up.\n"+
                    "Guard Up+2: Decreases the amount of knockback, stamina loss and health loss while guarding and increases the size of the guard window. Lances and Gunlances only: Only applies when exceeding Guard+2, Guarding affects 360 degrees around you, rear hits still cause knockdown. Heavy and Ranged Guards can block attacks that were previously unblockable. Lance's Guard Meter fills faster (3 Hits > 2 Hits per phial). Gunlances have lowered Wyvern Fire cooldowns (10s faster, 5s with Hiden). Synergizes well with Reflect Up, Obscurity Up and Rush Up.\n\n" +
                    "Adaptation\n"+
                    "Adaptation Up+1: Upgrades Adaptation by one level. If you already have Adaptation+2, increases the % of the adapted hitbox's value used to 90% for Cutting or Impact and 81% for Ranged Weapons.\n"+
                    "Adaptation Up+2: Increases the % of the adapted hitbox's value used to 90% for Cutting or Impact and 81% for Ranged Weapons.\n\n" +
                    "Encourage\n"+
                    "Encourage Up+1: Upgrades Encourage+1 to Encourage+2. If you already have Encourage+2, adds party wide Marathon Runner and Stamina Recovery Up Large.\n"+
                    "Encourage Up+2: Adds party wide Marathon Runner and Stamina Recovery Up Large.\n\n" +
                    "Reflect\n"+
                    "(Recommended) Reflect Up+1: Upgrades Reflect by 1 tier. Going beyond Reflect+3 increases the motion value of the Reflect+3 motion (48 > 68). Also buffs perfect guard reflects while active (72 > 92). The hit uses your current attack and sharpness values and inflicts impact based damage. Reflect motions cannot deal critical hits, inflict status damage or deal elemental damage. Consumes 1 sharpness on reflection hitting a monster. Does not benefit from Fencing+2 or utilize sword crystals if loaded. This skill can trigger on every part of any attacks which hit multiple times without any cooldown period. Does not buff the Perfect Guard reflect without actually having Reflect+3 active alongside Reflect Up. Synergizes well with Guard Up, Rush Up and Obscurity Up.\n"+
                    "Reflect Up+2: Upgrades Reflect by 2 tiers. Going beyond Reflect+3 increases the motion value of the Reflect+3 motion (48 > 68). Also buffs perfect guard reflects while active (72 > 92). The hit uses your current attack and sharpness values and inflicts impact based damage. Reflect motions cannot deal critical hits, inflict status damage or deal elemental damage. Consumes 1 sharpness on reflection hitting a monster. Does not benefit from Fencing+2 or utilize sword crystals if loaded. This skill can trigger on every part of any attacks which hit multiple times without any cooldown period. Does not buff the Perfect Guard reflect without actually having Reflect+3 active alongside Reflect Up. Synergizes well with Guard Up, Rush Up and Obscurity Up.\n"+
                    "Reflect Up+3: Increases the motion value of the Reflect+3 motion (48 > 68). Also buffs perfect guard reflects while active (72 > 92). The hit uses your current attack and sharpness values and inflicts impact based damage. Reflect motions cannot deal critical hits, inflict status damage or deal elemental damage. Consumes 1 sharpness on reflection hitting a monster. Does not benefit from Fencing+2 or utilize sword crystals if loaded. This skill can trigger on every part of any attacks which hit multiple times without any cooldown period. Does not buff the Perfect Guard reflect without actually having Reflect+3 active alongside Reflect Up. Synergizes well with Guard Up, Rush Up and Obscurity Up.\n\n" +
                    "Stylish\n"+
                    "(Recommended for evasion playstyle) Stylish Up: Successful evades cause your weapon to use no sharpness for a fixed number of attacks as well as regaining the usual sharpness from Stylish and releasing an Area of Effect attack with a motion value of 30. Amount of hits that do not consume sharpness varies based on the weapon type in use.\nDS: 5 hits.\nSnS, Tonfa: 4 Hits.\nLS, Lance, Swaxe F, Magnet Spike: 3 Hits.\nGS, Hammer, HH, GL: 2 Hits.\nUsing Consumption Slayer reduces this to 1 hit for everything except SnS and DS which have 2 hits. Stylish Up AoE Motion cannot deal critical hits, inflict status damage or deal elemental damage. The motion does not benefit from Fencing+2 nor does it utilize sword crystals if they are loaded. Synergizes well with Stylish Assault Up.\n"+
                    "Vigorous\n"+
                    "Vigorous Up: Multiplies attack by 1.15x when your HP is over 100. Adds +100 attack for Blademasters or +50 attack for Gunners if your HP bar is also completely filled.\n\n"+
                    "Obscurity\n"+
                    "(Recommended for parry/block playstyle) Obscurity Up: Reduces total attack steps to 6 for maximum buff and allows for sharpness recovery at maximum attack buff. Perfect guards go up three attack steps (i.e. 2 perfect guards is maximum attack buff) and recover additional sharpness while maxed. Recovering sharpness with Gunlance while in heat blade mode reduces the sharpness loss on deactivation (20 guard or 5 perfect guards negates all 100 sharpness loss).\nAttack Increase:\nSnS, Lance, GL, Tonfa: 70 / 140 / 210 / 240 / 270 / 300\nGS, Swaxe F, Magnet Spike: 50 / 100 / 150 / 175 / 200 / 225\nLS: 30 / 60 / 90 / 110 / 130 / 150\nSharpness Recovery:\nSnS: 4 / 12\nLance: 2 / 10\nGunlance: 5 / 20\nGS: 5 / 15\nSwaxe F: 4 / 10\nTonfa: 5 / 13\nLS: 5 / 11\nMagnet Spike: 5 / 10\n\n"+
                    "Soul\n"+
                    "(Recommended for HH) Soul Up: Soul can be applied by using items, attacks will not stagger other players.\nRed Soul: +100 Attack on both user and players struck. Zero stamina consumption for running with weapons unsheathed.\nBlue Soul: +200 Defense on both user and players struck. Health Recovery effects, removes most abnormal status effects (Zenith Blights cannot be removed.)\nThis Skill works with all sources of Soul meaning you can stack Blue Soul with Red Soul from Blazing Grace and buff both. Attack is a final addition that is always the stated value and completely unaffected by other skills and multipliers.\n\n"+
                    "Rush\n"+
                    "(Recommended) Rush Up: Adds a third stage to Rush that is activated from the 2nd stage by attacking or guarding. Strictly time limited and increases attack further and grants infinite stamina during its duration. Grants +70 true raw (Total +200) and has a duration of 30 seconds. Indicated by an intensified version of the current aura with additional lightning effects. Infinite stamina works with Combat Supremacy and Starving Wolf but does not refill, meaning you would need to activate it before the bar empties to have stamina for evading, etc. Synergizes well with Guard Up, Reflect Up and Obscurity Up.\n\n"+
                    "Ceaseless\n"+
                    "(Recommended) Ceaseless Up: Adds a third stage to Ceaseless with higher Affinity and additional Critical Multiplier. All stages require less hits to be reached and the skill goes down one level on timing out instead of losing all levels (e.g. stage 2 downgrades to stage 1 instead of no stages). Third stage is indicated by a red ceaseless aura instead of the standard white.\nFirst Phase: +35% Affinity, +0.1x Critical Multiplier\nSecond Phase: +50% Affinity, +0.15x Critical Multiplier\nThird Phase: +60% Affinity, +0.20x Critical Multiplier.\nEach weapon requires a different number of hits to progress in stages.\nSnS: 10s, 15/35/54 hits\nDS: 11s, 12/29/45 hits\nGS: 15s, 7/16/26 hits\nLS: 12, 11/29/47 hits\nHammer: 15s 6/21/36 hits\nHH: 14s, 8/24/40 hits\nLance, GL, Swaxe F: 12s, 10/27/44 hits\nTonfa: 11s, 11/27/43 hits\nMagnet Spike: 12s, 8/23/38 hits\nLBG: 11s, 27/74/120 hits\nHBG: 13s, 21/57/93 hits\nBow: 12s, 12/36/60 hits\nReflect and Stylish Up count towards these hit totals but Fencing+2's extra hit does not. Both affinity and multiplier stack with similar buffs (e.g. Issen+3 and second phase Ceaseless gives +70% Affinity and a multiplier of 1.65x)"
                    );
            }
        }

        public string GetGameStyleRankInfo
        {
            get
            {
                return string.Format("You can equip your first skill by going into a weapon's Book of Secrets menu and select one of the Special Effect options followed by one of the skills above. After you hit GSR100 you will be able to equip two skills and up until that point you will be able to equip 1.\n\n" +
                    "At HR5 you gain the basic Defense Skill, at HR6 you gain all the various Elemental Res skills and the first version of Sharpening Up and at HR7 you get access to Affinity Up and max Sharpening Up. All of the Res and Defense skills progress naturally as you rank up in G Style Rank with some having the requirement of GSR999 in the weapon or multiple weapons to be unlocked or maxed out.\n\n" +
                    "Every GSR999\n" +
                    "Passive Master\n" +
                    "Causes any monster attacks that would normally leave you completely knocked down to be partially recovered from (i.e. you land on your feet instead of lying down and slowly getting up.)\n\n" +
                    "Secret Technique\n" +
                    "An ability that can be used once a day after 12:00 that deals massive damage after a long wind up animation. Bound to the Kick button or key. Increases attack after use for the rest of that quest's duration.\n\n" +
                    "11x GSR999\n" +
                    "Soul Revival\n" +
                    "An ability that can be triggered once per quest that revives you after hitting 0 HP once and fills your health bar. Disabled with Determination. Unlocked on GSR999 weapons after you have 11 GSR999 total.\n\n" +
                    "### Special Effects (G Style Rank)\r\n\r\n|GSR| Effect|\r\n|--|------|\r\n|0|Def+100, Ele Res+20, All Res+10, Affinity+20|\r\n|10|Def+1|\r\n|20|Fire Res+2|\r\n|30|Conquest Def+10|\r\n|40|Water Res+2|\r\n|50|Conquest Atk+2|\r\n|60|Def+1|\r\n|70|Conquest Def+10|\r\n|80|Thunder Res+2|\r\n|90|Def+1|\r\n|100|Conquest Atk+2, 2 special effects can be set|\r\n|110|Def+1|\r\n|120|Ice Res+2|\r\n|130|Conquest Def+10|\r\n|140|Dragon Res+2|\r\n|150|Conquest Atk+2|\r\n|160|Def+1|\r\n|170|Conquest Def+10|\r\n|180|All Res+1|\r\n|190|Def+1|\r\n|200|Conquest Atk+2|\r\n|210|Def+1|\r\n|220|Fire Res+2|\r\n|230|Conquest Def+10|\r\n|240|Water Res+2|\r\n|250|Conquest Atk+2|\r\n|260|Def+1|\r\n|270|Conquest Def+10|\r\n|280|Thunder Res+2|\r\n|290|Def+1|\r\n|300|Conquest Atk+2|\r\n|310|Def+1|\r\n|320|Ice Res+2|\r\n|330|Conquest Def+10|\r\n|340|Dragon Res+2|\r\n|350|Conquest Atk+2|\r\n|360|Def+2|\r\n|370|Conquest Def+10|\r\n|380|All Res+1|\r\n|390|Def+2|\r\n|400|Conquest Atk+2|\r\n|410|Def+2|\r\n|420|Fire Res+2|\r\n|430|Conquest Def+10|\r\n|440|Water Res+2|\r\n|450|Conquest Atk+2|\r\n|460|Def+2|\r\n|470|Conquest Def+10|\r\n|480|Thunder Res+2|\r\n|490|Def+2|\r\n|500|Conquest Atk+2|\r\n|510|Def+2|\r\n|520|Ice Res+2|\r\n|530|Conquest Def+10|\r\n|540|Dragon Res+2|\r\n|550|Conquest Atk+2|\r\n|560|Def+2|\r\n|570|Conquest Def+10|\r\n|580|All Res+1|\r\n|590|Def+2|\r\n|600|Conquest Atk+2|\r\n|610|Def+2|\r\n|620|Fire Res+2|\r\n|630|Conquest Def+10|\r\n|640|Water Res+2|\r\n|650|Conquest Atk+2|\r\n|660|Def+2|\r\n|670|Conquest Def+10|\r\n|680|Thunder Res+2|\r\n|690|Def+2|\r\n|700|Conquest Atk+2|\r\n|710|Def+2|\r\n|720|Ice Res+2|\r\n|730|Conquest Def+10|\r\n|740|Ice Res+2|\r\n|750|Conquest Atk+2|\r\n|760|Def+2|\r\n|770|Conquest Def+10|\r\n|780|All Res+1|\r\n|790|Def+2|\r\n|800|Conquest Atk+4|\r\n|810|Def+2|\r\n|820|Fire Res+2|\r\n|830|Conquest Def+10|\r\n|840|Water Res+2|\r\n|850|Conquest Atk+4|\r\n|860|Def+2|\r\n|870|Conquest Def+10|\r\n|880|Thunder Res+2|\r\n|890|Def+2|\r\n|900|Conquest Atk+4|\r\n|910|Def+2|\r\n|920|Ice Res+2|\r\n|930|Conquest Defense+10|\r\n|940|Dragon Res+2|\r\n|950|Conquest Atk+4|\r\n|960|Def+2|\r\n|970|Conquest Def+10|\r\n|980|All Res+1|\r\n|990|Def+2|\r\n|999|Passive Master, Conquest Atk+4|\r\n|x11 GSR999|Soul Revival, Conquest Atk Base 100, Conquest Def Base 300, G Rank Weapon unlock bonuses for conquest skills (+10 Def, +5 Atk each)|\r\n\r\n### GSR Weapon Unlock Bonus\r\n\r\n|Unlocks| Bonus|\r\n|--|------|\r\n|11|None|\r\n|12|Affinity+2, Ele Res+2, All Res+2, Def+10, Conquest Atk Base 100+5, Conquest Def Base 300+10|\r\n|13|Affinity+2, Ele Res+2, All Res+2, Def+10, Conquest Atk Base 100+5, Conquest Def Base 300+10|\r\n|14|Affinity+2, Ele Res+1, All Res+1, Def+10, Conquest Atk Base 100+5, Conquest Def Base 300+10|");
            }
        }

        public string GetGameDivaInfo
        {
            get
            {//todo: missing gems?
                return string.Format("Prayer Gems\nRinging Prayer Gem\n"+
                    "Adds new items to the GCP store based on level.\n\n"+
                    "Elegance Prayer Gem\n"+
                    "Adds passive HP recovery to all quests.\n\n"+
                    "Heavy Thunder Prayer Gem\n"+
                    "Elemental damage increases based on level.\n\n"+
                    "Windstorm Prayer Gem\n"+
                    "Sharpness does not decrease with blademaster weapons. Works for 5, 10 or 20 quests depending on level during the prayer active window.\n\n"+
                    "Cutting Edge Prayer Gem\n"+
                    "Increases the amount of raw damage dealt by a cutting weapon by adjusting hitboxes to be weaker against the damage type.\n\n"+
                    "Status Length Prayer\n"+
                    "Increases the duration of status effects on monsters.\n\n"+
                    "Rising Bullet Prayer Gem\n"+
                    "Increases the amount of raw damage dealt by a ranged weapon by adjusting hitboxes to be weaker against the damage type.\n\n"+
                    "Severing Power Prayer Gem\n"+
                    "Tails can be cut with any damage type.\n\n"+
                    "Powerful Strikes Prayer Gem\n"+
                    "Increases affinity of all weapons based on the level of the song.\n\n"+
                    "Protection Prayer Gem\n"+
                    "Gives Divine Protection, Goddess' Embrace or Soul Revival based on level.\n\n"+
                    "Mobilization Prayer Gem\n"+
                    "Attack will go up based on the number of human hunters in a quest.\n\n"+
                    "Unshakable Prayer Gem\n"+
                    "Monsters cannot flee if in the same area as a hunter.\n\n"+
                    "Blunt Prayer Gem\n"+
                    "Increases the amount of raw damage dealt by an impact weapon by adjusting hitboxes to be weaker against the damage type.\n\n"+
                    "Diva Questline\n"+
                    "Unless rank is specified the monsters are any rank. Your partner needs to be PR81 to progress through these quests. Do the special quests for PRP and give it the HRP tickets you get from progressing by choosing the final option followed by the first option on your partner in the smith or your house. If you find yourself unable to progress you probably need to talk to one of the NPCs who gave you the task again (cats etc.). Be sure to also look for monster names in the text if you can't progress after killing one, you might be on a lower step than you thought. Complete Chapter 3 to unlock Diva Song.\n\n"+
                    "Chapter 1\n"+
                    "Part 1: Deliver 1 Thin Jack Mackerel (薄竹筴魚). Deliver 1 Lazurite Jewel (琉璃原珠)\n\n"+
                    "Part 2: Hunt 1 White Monoblos. Return to the Diva Hall\n\n"+
                    "Part 3: Talk to the Guild Mistress. Hunt 1 Yama Tsukami. Talk to the Legendary Rasta Edward (Lance User)\n\n"+
                    "Part 4: Talk to the Guild Mistress. Hunt 1 Chameleos. Talk to the Legendary Rasta Edward\n\n"+
                    "Part 5: Talk to the Guild Mistress. Hunt 1 Yama Tsukami. Return to the Diva Hall. Claim the items you need to deliver from the Hunter Challenge, you don't need to farm a million Kelbi.\n\n"+
                    "Rewards: Diva Armour Materials. Items to deliver in Chapter 2 (Hunter Challenge Reward)\n\n"+
                    "Chapter 2\n"+
                    "Part 1: Deliver 30 Kelbi Horns (精靈鹿的角). Deliver 20 Chaos Shrooms (混沌茸). Deliver 5 Kirin Azure Horns (麒麟的蒼角). Items delivered above are returned to you\n\n"+
                    "Part 2: Hunt 3 Cephadromes. Deliver 10 Dragon Seeds (屠龍果實). Hunt 2 Lao Shan Lungs. Return to the Diva Hall. Talk to the Legendary Rastas Edward and Frau (DS user)\n\n"+
                    "Part 3: Talk to the Legendary Rasta Frau. Return to the Diva Hall. Hunt 1 Baruragaru. Return to the Diva Hall. Talk to the Legendary Rasta Frau. Return to the Diva Hall\n\n"+
                    "Rewards: Diva HC Armour Materials"+
                    "Chapter 3\n"+
                    "Part 1: Hunt 1 Teostra. Return to the Diva Hall. Talk to the Legendary Rasta Frau\n\n"+
                    "Part 2: Go to the Blacksmith. Return to the Diva Hall. Hunt 3 Rukodioras\n\n"+
                    "Part 3: Hunt 1 Anorupatisu\n\n"+
                    "Part 4: Hunt 1 Rebidiora\n\n"+
                    "Rewards: Diva G Rank Weapon Materials. Diva Weapon Gem (1st Series). 5 Diva Song Gems (Hunter Challenge Reward). 5 Warm Honey Tea (Give the Diva as a gift then Hunter Challenge Reward). Completion of this Chapter unlocks the Diva Song Buffs. Cram her full of warm honey tea and fluffy cakes to max its effects. Your Discord RPC shows the current Bond when you are in the Diva Hall, the maximum is 999.\n\n"+
                    "Chapter 4\n"+
                    "Part 1: Hunt 1 Berukyurosu. Hunt 1 Doragyurosu\n\n"+
                    "Part 2: Deliver 1 Saint Ore (純聖礦石). Hunt 1 Hyujikiki. Hunt 1 Giaorugu\n\n"+
                    "Part 3: Speak to the Town Square Cats three times. Hunt 2 Gougarfs\n\n"+
                    "Part 4: Talk to NPC in Blacksmith. Solo Hunt 1 Gurenzeburu\n\n"+
                    "Part 5: Talk to Guild Mistress. Hunt 1 Pokaradon. Hunt 1 Midogaron. Talk to NPC next to Guild Hall entrance\n\n"+
                    "Rewards: Diva HC Armour Materials\n\n"+
                    "Chapter 5\n"+
                    "Part 1: Hunt 1 Farunokku\n\n"+
                    "Part 2: Hunt 2 Baruragaru (Return to Fountain between the two)\n\n"+
                    "Part 3: Hunt 1 Rebidiora\n\n"+
                    "Part 4: Hunt 1 Zerureusu\n\n"+
                    "Rewards: Diva Weapon Gem (1st Series)\n\n"+
                    "Chapter 6\n"+
                    "Part 1: Hunt 1 Akantor\n\n"+
                    "Part 2: Hire a partner if you don't have one and then talk to them in your house. Return to the Diva Hall. Talk to partner in house, return to Diva Hall.\n\n"+
                    "Part 3: ※Partner must be at least PR31 to proceed. Hunt 1 G Rank Yian Kut-ku with partner present.\n\n"+
                    "Part 4: ※Partner must be at least PR51 to proceed. Hunt 1 Pokaradon with partner present.\n\n"+
                    "Part 5: ※Partner must be at least PR81 to proceed. Hunt 1 Midogaron with partner present. (Talk to partner in house and return to Diva Hall before leaving on quest)\n\n"+
                    "Rewards: Diva Armour Materials\n\n" +
                    "Chapter 7\n"+
                    "Part 1: Talk to Blacksmith and return to Diva Hall. Hunt 1 Rebidiora\n\n"+
                    "Part 2: Hunt 2 G Rank HC Gurenzeburu (Return to Fountain between the two)\n\n"+
                    "Part 3: Hunt 1 Taikun Zamuza\n\n"+
                    "Part 4: Hunt 1 Meraginasu\n\n"+
                    "Rewards: Diva Weapon Gem (1st Series)\n\n" +
                    "Chapter 8\n" +
                    "Part 1: Speak to Blacksmith and return to Diva Hall\n\n"+
                    "Part 2: Deliver 3 Grease Stone (白鳥石) and 1 Atarka Ore (亞達爾純礦石). You can mine the ores in the G Rank Flower Field or simply buy them for 235 GCP total. Hunt 1 Forokururu\n\n"+
                    "Part 3: You need to craft the Prototype Tonfas at this point. Kill 3 Aptonoth in the preset quest\n\n"+
                    "Part 4: Hunt 1 Yian Kut-Ku (Does not need to be with Tonfas)\n\n"+
                    "Rewards: Ores spent in part 2 (Hunter Challenge Reward). Used to be ability to craft Tonfas.\n\n" +
                    "Chapter 9\n" +
                    "Part 1: Deliver 1 Teostra Miracle Wing (Supremacy Teo)\n\n"+
                    "Part 2: Hunt 2 G Rank Velocidrome\n\n"+
                    "Part 3: Hunt 1 Meraginasu\n\n"+
                    "Part 4: Speak to Gin (Hammer Rasta)\n\n"+
                    "Rewards: Diva G Rank Armour Materials\n\n" +
                    "Chapter 10\n"+
                    "Part 1: Talk to Guild Master. Hunt 1 Monoblos\n\n"+
                    "Part 2: Hunt 1 Gou Lunastra\n\n"+
                    "Part 3: Speak to the Guild Mistress\n\n"+
                    "Part 4: Hunt 1 Anorupatisu (Preset Quest). ※ Everyone must use Tonfas for this mission . (Restricted equipment disables AI outside of Legendaries)\n\n"+
                    "Rewards: Diva Weapon Materials (1st Series) (2 Gems with Hunter Challenge Reward)\n\n" +
                    "Chapter 11\n\n" +
                    "Part 1: Talk to Guild Mistress, Return to Diva Hall\n\n"+
                    "Part 2: Capture 1 Forokururu\n\n"+
                    "Part 3: Speak to Leila (Tonfa Legendary). Solo Hunt 1 Diorex. Speak to Leila. Return to the Diva Hall\n\n"+
                    "Part 4: Hunt 1 Burst Species Meraginasu\n\n"+
                    "Rewards: Diva Armour Materials\n\n" +
                    "Chapter 12\n" +
                    "Part 1: Hunt 1 G Rank Gold Rathian, talk to cats and return to Diva Hall\n\n"+
                    "Part 2: Speak to Leila and return to the Diva Hall\n\n"+
                    "Part 3: Hunt 1 Inagami\n\n"+
                    "Part 4: Hunt 1 G Rank Inagami (Preset quest with set equipment, AI outside of Legendaries is disabled)\n\n"+
                    "Rewards: Diva Weapon Gem (2nd Series)\n\n" +
                    "Chapter 13\n" +
                    "Part 1: Hunt 1 Giaorugu\n\n"+
                    "Part 2: Hunt 1 G Rank Gravios\n\n"+
                    "Part 3: Speak to Leila and return to the Diva Hall. Speak to the Blacksmith\n\n"+
                    "Part 4: Hunt 1 G Rank Forokururu. Hunt 1 G Rank HC Rajang\n\n"+
                    "Rewards: Diva HC Armour Materials\n\n" +
                    "Chapter 14\n" +
                    "Part 1: Hunt 1 Red Lavasioth (Training Quest on Black Quest NPC)\n\n"+
                    "Part 2: Speak to Flora (SnS Legendary) and return to Diva Hall. Hunt 1 Hyujikiki\n\n"+
                    "Part 3: Hunt 1 Inagami\n\n"+
                    "Part 4: Deliver 3 Herbal Medicine G (中藥G). (Can be bought in Guild Hall for Guild Tix)\n\n"+
                    "Rewards: Diva Weapon Gem (2nd Series)\n\n" +
                    "Chapter 15\n" +
                    "Part 1: Talk to Guild Master. Return to fountain. Hunt 1 G Rank White Espinas.\n\n"+
                    "Part 2: Hunt 1 G Rank Baruragaru\n\n"+
                    "Part 3: Hunt 1 G Rank Akura Jebia\n\n"+
                    "Part 4: Hunt 1 Burst (G Rank) Garuba Daora\n\n"+
                    "Rewards: Diva Armour Materials. Diva Weapon Gem (2nd Series)"
                    );
            }
        }

        public string GetGameGuildFood
        {
            get
            {
                return string.Format("The order of the skills are G.Failure/Failure/Success/G.Success\n\n" +
                    "Easiest way to cook is selecting Secret Seasoning from Chef's Wisdom before cooking. What it does is raise the success level by one. \n\n" +
                    "Page 1\n" +
                    "1: Explosive Rice [Snow Powder, Whole Vanilla, Wabisabi Wasabi, Deep Sea Chub] (Hunger Increased [Lg] / Health+30 / Rage+1 / Rage+2)\n\n"+
                    "2: Pioneer's Meal [Goemon Frog, Rainbow Mint, Onion Sticks, Mimic Vines] (Hunger Increased [Lg] / Health +30 / Three Worlds Protection +2 / Three Worlds Protection +3)\n\n" +
                    "2: Pioneer's Meal [Dangerous Melon, Demon Pepper, Onion Sticks, Mimic Vines] (Hunger Increased [Lg], Wind Pressure [Sm], Wind Pressure [Lg], Wind Pressure [Dragon])\n\n" + 
                    "3: Lucky Pancake [Star Pineapple, Dangerous Melon, Whole Vanilla, Mimic Vines] (Hunger Increased [Lg] / Health +10 / Good Luck / Great Luck)\n\n" +
                    "4: Ultimate Sashimi [Deep Sea Chub, Blk and Wht Dragonfly, Ice Salmon, Whole Vanilla] (Hunger Increased [Lg], Earplugs, High Grade Earplugs, Super Ear Plugs)\n\n" +
                    "5: Brother's BBQ [Monochrome Mushroom, Deep Sea Chub, Ice Salmon, Mimic Vines] (Hunger Increased [Lg], Caring +1, Caring +2, Caring +3)\n\n"+
                    "6: Hot Claw Feast [Lava King Crab, Whole Vanilla, Dangerous Melon, Mimic Vines] (Hunger Increased [Lg], Health +20, Adrenaline +1, Adrenaline +2)\n\n"+
                    "7: Medicinal Spirit [Ancient Algae, Dangerous Melon, Deep Sea Chub, Demon Pepper] (Hunger Increased [Lg], Wind Pressure [Sm], Wind Pressure [Lg], Wind Pressure [Dragon])\n\n"+
                    "7: Medicinal Spirit [Rainbow Mint, Dangerous Melon, Deep Sea Chub, Demon Pepper] (Hunger Increased [Lg], Wind Pressure [Sm], Wind Pressure [Dragon], Violent Wind Breaker)\n\n"+
                    "Page 2\n"+
                    "8: World Fried Rice [Acid Pepper, Mimic Vines, Onion Sticks, Blk and Wht Dragonfly] (Hunger Increased [Lg], Health +20, Runner +1, Runner +2)\n\n"+
                    "9: Goddess Dessert [Snow Kiwi, Dangerous Melon, Whole Vanilla, Magma Mango] (Hunger Increased [Lg], Health +20, Divine Protection, Goddess' Embrace)\n\n"+
                    "10: Grilled Shellfish [Large Blly Shlfish, Ice Salmon, Whole Vanilla, Mimic Vines] (Hunger Increased [Lg], Health +20, Hunger Halved, Hunger Negated)\n\n"+
                    "(Recommended) 11: Holy Seafood Banquet [Deep Sea Chub, Ice Salmon, Whole Vanilla, Mimic Vines] (Hunger Increased [Lg], Wide-Area +1, Wide-Area +2, Wide-Area +3)\n\n" +
                    "(Recommended) 12: Energy Noodles [Mimic Vines, Onion Sticks, Blk and Wht Dragonfly, Whole Vanilla] (Hunger Increased [Lg], Health +10, Terrain [Sm], Terrain [Lg])\n\n" +
                    "13: Hunter's Whim [Ice Salmon, Dangerous Melon, Mimic Vines, Deep Sea Chub] (Hunger Increased [Lg], Health +10, Whim, Divine Whim)\n\n"+
                    "14: Fantasy Dumplings [Dangerous Melon, Magma Mango, Whole Vanilla, Mimic Vines] (Hunger Increased [Lg], Health +10, Paralysis Halved, Paralysis Negated)\n\n"+
                    "Page 3\n"+
                    "15: Crimson BBQ [Demon Pepper, Dangerous Melon, Magma Mango, Mimic Vines] (Hunger Increased [Lg], Health +10, Sleep Halved, Sleep Negated)\n\n"+
                    "16: Sweet Wrap [Magma Mango, Star Pineapple, Dangerous Melon, Mimic Vines] (Hunger Increased [Lg], Health +10, Poison Halved, Poison Negated)\n\n"+
                    "17: Dawn Toast [Whole Vanilla, Magma Mango, Dangerous Melon, Mimic Vines] (Hunger Increased [Lg], Health +10, Stun Halved, Stun Negated)\n\n"+
                    "(Recommended) 18: Rainbow Soup [Onion Sticks, Snow Powder, Blk and Wht Dragonfly, Bright Grain] (Hunger Increased [Lg], All Res UP +5, All Res UP +10, All Res UP +20)\n\n" +
                    "18: Rainbow Soup [Onion Sticks, Magma Mango, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Fire Res +10, Fire Res +20, Fire Res +30)\n\n"+
                    "18: Rainbow Soup [Onion Sticks, Dangerous Melon, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Water Res +10, Water Res +20, Water Res +30)\n\n"+
                    "18: Rainbow Soup [Onion Sticks, Ice Salmon, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Ice Res +10, Ice Res +20, Ice Res +30)\n\n"+
                    "18: Rainbow Soup [Onion Sticks, Demon Pepper, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Thunder Res +10, Thunder Res +20, Thunder Res +30)\n\n"+
                    "18: Rainbow Soup [Onion Sticks, Mimic Vines, Whole Vanilla, Deep Sea Chub] (Hunger Increased [Lg], Dragon Res +10, Dragon Res +20, Dragon Res +30)\n\n"+
                    "(Recommended) 19: Vigor Salad [Taiko Olive, Miracle Herb, Ancient Algae, Whole Vanilla] (Hunger Increased [Lg], Health +30, Wide-Area +3, Herbal Medicine)\n\n"+
                    "(Recommended) 20: Hearty Pie [Gutsy Meat, Onion Sticks, Bright Grain, Snow Powder] (Hunger Increased [Lg], Health +30, Encourage +1, Encourage +2)\n\n" +
                    "21: Unity Buns [Taiko Olive, Bright Grain, Blk and Wht Dragonfly, Mimic Vines] (Hunger Increased [Lg], Health +30, Bond, Assistance)\n\n"+
                    "Page 4\n"+
                    "(Recommended) 22: Blast Steak [Gutsy Meat, Magma Mango, Snow Kiwi, Star Pineapple] (Blue Soul, Blue Soul, Incitement, Red Soul)\n\n" +
                    "0: Guild's Yaminabe [Any, Any, Any, Any] (Hunger Increased [Lg], Random, Random, Random)");
            }
        }

        //void CopyToClipBoard(string data)
        //{
        //    Clipboard.SetText(data);
        //}

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Reloads the data.
        /// </summary>
        public void ReloadData()
        {
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
