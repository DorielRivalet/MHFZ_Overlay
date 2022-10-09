using Memory;
using System;
using System.ComponentModel;
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




        abstract public int RoadSelectedMonster();

        abstract public int RavienteTriggeredEvent();

        //normal and violent. berserk support
        abstract public int RavienteAreaID();

        abstract public int GreatSlayingPoints();
        abstract public int GreatSlayingPointsSaved();


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

        /// <summary>
        /// Shows the hp bar?
        /// </summary>
        /// <param name="monsterId">The monster identifier.</param>
        /// <param name="monsterHp">The monster hp.</param>
        /// <returns></returns>
        public bool ShowHPBar(int monsterId, int monsterHp)
        {
            return (monsterId > 0 && monsterHp != 0) || Configuring;
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

        public string Monster1Name => getDureName() != "None" ? getDureName() : getMonsterName(GetNotRoad() || RoadSelectedMonster() == 0 ? LargeMonster1ID() : LargeMonster2ID()); //monster 1 is used for the first display and road uses 2nd choice to store 2nd monster
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
                        if (RankBand() == 32 || RankBand() == 54)
                            return "Thirsty Pariapuria";
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

                if (className == "Blademaster")
                {
                    Dictionary.MeleeWeapons.MeleeWeaponIDs.TryGetValue(MeleeWeaponID(), out string? wepname);
                    //string address = Convert.ToString(MeleeWeaponID(), 16).ToUpper();
                    string address = MeleeWeaponID().ToString("X4").ToUpper();  // gives you hex 4 digit "007B"
                    return string.Format("{0} ({1})", wepname,address);
                }
                else if (className == "Gunner")
                {
                    Dictionary.RangedWeapons.RangedWeaponIDs.TryGetValue(RangedWeaponID(), out string? wepname);
                    //string address = Convert.ToString(MeleeWeaponID(), 16).ToUpper();
                    string address = RangedWeaponID().ToString("X4").ToUpper();  // gives you hex 4 digit "007B"
                    return string.Format("{0} ({1})", wepname, address);
                }
                else
                {
                    return "None";
                }
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
                //string address = Convert.ToString(ArmorHeadID(), 16).ToUpper();
                string address = ArmorHeadID().ToString("X4").ToUpper();
                return string.Format("{0} ({1})", piecename, address);
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
                //string address = Convert.ToString(ArmorChestID(), 16).ToUpper();
                string address = ArmorChestID().ToString("X4").ToUpper();
                return string.Format("{0} ({1})", piecename, address);
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
                //string address = Convert.ToString(ArmorArmsID(), 16).ToUpper();
                string address = ArmorArmsID().ToString("X4").ToUpper();
                return string.Format("{0} ({1})", piecename, address);
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
                //string address = Convert.ToString(ArmorWaistID(), 16).ToUpper();
                string address = ArmorWaistID().ToString("X4").ToUpper();
                return string.Format("{0} ({1})", piecename, address);
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
                //string address = Convert.ToString(ArmorLegsID(), 16).ToUpper();
                string address = ArmorLegsID().ToString("X4").ToUpper();
                return string.Format("{0} ({1})", piecename, address);
            }
        }

        /// <summary>
        /// Gets the decos.
        /// </summary>
        /// <value>
        /// The decos.
        /// </value>
        public string GetDecos
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the sigils.
        /// </summary>
        /// <value>
        /// The sigils.
        /// </value>
        public string GetSigils
        {
            get
            {
                return "";
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

        /// <summary>
        /// Generates the gear stats
        /// </summary>
        //public string GenerateGearStats()
        //{
        //    SavedGearStats = string.Format("【MHF-Z】Overlay {0} {1}({2}){3}Text", MainWindow.CurrentProgramVersion, GetWeaponClass(), GetGender(), GetRealWeaponName());
        //    return SavedGearStats;
        //}

        //public string GetGearStats
        //{
        //    get
        //    {
        //        return GenerateGearStats();
        //    }
        //}

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
                else if (SkillName3 == null || SkillName3 == "None")
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
                else if (SkillName3 == null || SkillName3 == "None")
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
                else if (SkillName3 == null || SkillName3 == "None")
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

        public string MarkdownSavedGearStats = "";

        /// <summary>
        /// Generates the gear stats
        /// </summary>
        public string GenerateGearStats()
        {
            //save gear to variable
            string showGouBoost = "";

            if (GetGouBoostMode())
                showGouBoost = " (After Gou/Muscle Boost)";

            SavedGearStats = string.Format("【MHF-Z】Overlay {0} {1}({2})\n\n{3}: {4}\nHead: {5}\nChest: {6}\nArms: {7}\nWaist: {8}\nLegs: {9}\nCuffs: {10}\n\nWeapon Attack: {11} | Total Defense: {12}\n\nZenith Skills:\n{13}\n\nAutomatic Skills:\n{14}\n\nActive Skills{15}:\n{16}\n\nCaravan Skills:\n{17}\n\nDiva Skill:\n{18}\n\nGuild Food:\n{19}\n\nItems:\n{20}\n\nAmmo:\n{21}\n\nPoogie Item:\n{22}\n", MainWindow.CurrentProgramVersion, GetWeaponClass(), GetGender(), CurrentWeaponName, GetRealWeaponName, "head", "chest", "arm", "waist", "leg", "cuff", BloatedWeaponAttack().ToString(), TotalDefense().ToString(), GetZenithSkills, GetAutomaticSkills, showGouBoost, GetArmorSkills, GetCaravanSkills, GetDivaSkillNameFromID(DivaSkill()), GetArmorSkill(GuildFoodSkill()), "items", "ammo", GetItemName(PoogieItemUseID()));
            MarkdownSavedGearStats = string.Format("__【MHF-Z】Overlay {0}__ *{1}({2})*\n\n**{3}**: {4}\n**Head:** {5}\n**Chest:** {6}\n**Arms:** {7}\n**Waist:** {8}\n**Legs:** {9}\n**Cuffs:** {10}\n\n**Weapon Attack:** {11} | **Total Defense:** {12}\n\n**Zenith Skills:**\n{13}\n\n**Automatic Skills:**\n{14}\n\n**Active Skills{15}:**\n{16}\n\n**Caravan Skills:**\n{17}\n\n**Diva Skill:**\n{18}\n\n**Guild Food:**\n{19}\n\n**Items:**\n{20}\n\n**Ammo:**\n{21}\n\n**Poogie Item:**\n{22}\n", MainWindow.CurrentProgramVersion, GetWeaponClass(), GetGender(), CurrentWeaponName, GetRealWeaponName, "head", "chest", "arm", "waist", "leg", "cuff", BloatedWeaponAttack().ToString(), TotalDefense().ToString(), GetZenithSkills, GetAutomaticSkills, showGouBoost, GetArmorSkills, GetCaravanSkills, GetDivaSkillNameFromID(DivaSkill()), GetArmorSkill(GuildFoodSkill()), "items", "ammo", GetItemName(PoogieItemUseID()));
            return string.Format("【MHF-Z】Overlay {0} {1}({2})\n\n{3}: {4}\nHead: {5}\nChest: {6}\nArms: {7}\nWaist: {8}\nLegs: {9}\nCuffs: {10}\n\nWeapon Attack: {11} | Total Defense: {12}\n\nZenith Skills:\n{13}\n\nAutomatic Skills:\n{14}\n\nActive Skills{15}:\n{16}\n\nCaravan Skills:\n{17}\n\nDiva Skill:\n{18}\n\nGuild Food:\n{19}\n\nItems:\n{20}\n\nAmmo:\n{21}\n\nPoogie Item:\n{22}\n", MainWindow.CurrentProgramVersion, GetWeaponClass(), GetGender(), CurrentWeaponName, GetRealWeaponName,"head", "chest", "arm", "waist", "leg", "cuff", BloatedWeaponAttack().ToString(), TotalDefense().ToString(), GetZenithSkills, GetAutomaticSkills, showGouBoost, GetArmorSkills, GetCaravanSkills, GetDivaSkillNameFromID(DivaSkill()), GetArmorSkill(GuildFoodSkill()), "items", "ammo", GetItemName(PoogieItemUseID()));
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
