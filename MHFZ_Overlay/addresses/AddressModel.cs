using Memory;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using System.Security.RightsManagement;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace MHFZ_Overlay.addresses
{
    public abstract class AddressModel : INotifyPropertyChanged
    {
        public readonly Mem M;

        public int SavedMonster1MaxHP = 0;
        public int SavedMonster2MaxHP = 0;
        private int SavedMonster3MaxHP = 0;
        private int SavedMonster4MaxHP = 0;


        private int SavedMonster1ID = 0;
        private int SavedMonster2ID = 0;

        public AddressModel(Mem m) => M = m;

        public int SelectedMonster = 0;

        public bool ShowMonsterInfos { get; set; } = true;

        public bool ShowMonsterHPBars { get; set; } = true;

        public bool ShowMonsterPartHP { get; set; } = true;

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
        abstract public int DeliveryQuestPoints();


        //red is 0
        abstract public int SharpnessLevel();


        abstract public int PartnerLevel();
        abstract public int ObjectiveType();
        abstract public int DivaSkillUsesLeft();
        abstract public int HalkFullness();
        abstract public int RankBand();



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

        abstract public int RoadSelectedMonster();
        public bool HasMonster1 => ShowHPBar(LargeMonster1ID(), Monster1HPInt());
        public bool HasMonster2 => ((LargeMonster2ID() > 0 && Monster2HPInt() != 0 && GetNotRoad()) || Configuring); // road check since the 2nd choice is used as the monster #1
        public bool HasMonster3 => ShowHPBar(LargeMonster3ID(), Monster3HPInt());
        public bool HasMonster4 => ShowHPBar(LargeMonster4ID(), Monster4HPInt());

        public int HitCount => HitCountInt();

        public bool _configuring = false;

        public bool ShowMonsterEHP()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableEHPNumbers == true)
                return true;
            else
                return false;
        }

        public bool Configuring { get { return _configuring; } set { _configuring = value; ReloadData(); } }

        public bool ShowHPBar(int monsterId, int monsterHp)
        {
            return (monsterId > 0 && monsterHp != 0) || Configuring;
        }


        public bool? roadOverride()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.IsRoadOverride == 1)
                return true;
            else if (s.IsRoadOverride == 2)
                return false;
            return null;
        }

        public bool GetNotRoad()
        {
            bool? b = roadOverride();
            if (b != null)
                return b.Value;
            return IsNotRoad();
        }

        //assumption: it follows ferias' monster part order top to bottom, presumably (e.g. head is at the top, so part 0 is head, and so on)
        public string GetPartName(int number,int monsterID = 0)
        {
            string partName = "None";

            switch (number)
            {
                case 0:
                    partName = "Head";
                    break;
                case 1:
                    partName = "";
                    break;
                case 2:
                    partName = "";
                    break;
                case 3:
                    partName = "";
                    break;
                case 4:
                    partName = "";
                    break;
                case 5:
                    partName = "";
                    break;
                case 6:
                    partName = "";
                    break;
                case 7:
                    partName = "";
                    break;
                case 8:
                    partName = "";
                    break;
                case 9:
                    partName = "";
                    break;
                case 10:
                    partName = "";
                    break;
                default:
                    partName = "None";
                    break;
            }
                

            //switch (monsterID)
            //{
            //    case 0:
            //    case 1:
            //        return "None";

            //}

            return string.Format("{0}: ",partName);
        }



        public string Monster1Part1Number
        {
            get
            {
                int currentPartHP = Monster1Part1();
                //if (currentPartHP > 0)
                //{
                    return GetPartName(0) + currentPartHP.ToString();
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
                return currentPartHP.ToString();
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
                return currentPartHP.ToString();
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
                return currentPartHP.ToString();
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
                return currentPartHP.ToString();
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
                return currentPartHP.ToString();
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
                return currentPartHP.ToString();
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
                return currentPartHP.ToString();
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
                return currentPartHP.ToString();
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
                return currentPartHP.ToString();
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



        public bool ShowTimeLeftPercentage()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableTimeLeftPercentage == true)
                return true;
            else
                return false;
        }

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

        public string TimeLeftPercent = "";

        public string Time
        {
            get
            {
                int time = TimeInt();
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
        public string ATK
        {
            get
            {
                int weaponRaw = WeaponRaw();
                int weaponType = WeaponType();
                return weaponRaw.ToString();// ((int)(GetMultFromWeaponType(weaponType) * weaponRaw)).ToString();
            }
        }

        public string SharpnessColor
        {
            get
            {
                //see palettes.md
                int currentSharpnessLevel = SharpnessLevel();
                switch (currentSharpnessLevel)
                {
                    case 0:
                        return "#c50f3a";
                    case 1:
                        return "#e85218";
                    case 2:
                        return "#f3c832";
                    case 3:
                        return "#5ed300";
                    case 4:
                        return "#3068ee";
                    case 5:
                        return "#f0f0f0";
                    case 6:
                        return "#de7aff";
                    case 7:
                        return "#86f4f4";
                    default:
                        return "#ffffff";
                }
            }
        }

        public string SharpnessNumber
        {
            get
            {
                int currentSharpness = Sharpness();
                if (currentSharpness > 0)
                {
                    return currentSharpness.ToString();
                }
                return "0";
            }
        }

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

        public string CurrentWeaponName
        { 
            get
            {
                int weaponType = WeaponType();
                return GetWeaponNameFromType(weaponType);
                //return WeaponType().ToString();
            }
        }

        public string Size
        {
            get
            {
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1Size();
                    case 1:
                        return Monster2Size();
                    default:
                        return Monster1Size();
                }
            }
        }


        public string AtkMult
        {
            get
            {
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1AtkMult();
                    case 1:
                        return Monster2AtkMult();
                    default:
                        return Monster1AtkMult();
                }
            }
        }

        public bool isMonsterFocused = false;

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

        public int PoisonCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1Poison();
                    case 1:
                        return Monster2Poison();
                    default:
                        return Monster1Poison();
                }
            }
        }

        public int PoisonMax
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1PoisonNeed();
                    case 1:
                        return Monster2PoisonNeed();
                    default:
                        return Monster1PoisonNeed();
                }
            }
        }

        public int SleepCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1Sleep();
                    case 1:
                        return Monster2Sleep();
                    default:
                        return Monster1Sleep();
                }
            }
        }

        public int SleepMax
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1SleepNeed();
                    case 1:
                        return Monster2SleepNeed();
                    default:
                        return Monster1SleepNeed();
                }
            }
        }
        public int ParaCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1Para();
                    case 1:
                        return Monster2Para();
                    default:
                        return Monster1Para();
                }
            }
        }

        public int ParaMax
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1ParaNeed();
                    case 1:
                        return Monster2ParaNeed();
                    default:
                        return Monster1ParaNeed();
                }
            }
        }



        public int BlastCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1Blast();
                    case 1:
                        return Monster2Blast();
                    default:
                        return Monster1Blast();
                }
            }
        }

        public int BlastMax
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1BlastNeed();
                    case 1:
                        return Monster2BlastNeed();
                    default:
                        return Monster1BlastNeed();
                }
            }
        }
        public int StunCurrent
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1Stun();
                    case 1:
                        return Monster2Stun();
                    default:
                        return Monster1Stun();
                }
            }
        }

        public int StunMax
        {
            get
            {
                if (Configuring)
                    return 100;
                switch (SelectedMonster)
                {
                    case 0:
                        return Monster1StunNeed();
                    case 1:
                        return Monster2StunNeed();
                    default:
                        return Monster1StunNeed();
                }
            }
        }

        public string Monster1Name => getMonsterName(GetNotRoad() || RoadSelectedMonster() == 0 ? LargeMonster1ID() : LargeMonster2ID()); //monster 1 is used for the first display and road uses 2nd choice to store 2nd monster
        public string Monster2Name => getMonsterName(LargeMonster2ID());
        public string Monster3Name => getMonsterName(LargeMonster3ID());
        public string Monster4Name => getMonsterName(LargeMonster4ID());

        public string getMonsterName(int id)
        {
            if (Configuring)
                return "Blitzkrieg Bogabadorumu";
            if (id == 0)
                return "";
            Dictionary.List.MonsterID.TryGetValue(id, out string? monstername);
            return monstername + "";
        }
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
        public float GetMultFromWeaponType(int weaponType)
        {
            switch (weaponType)
            {
                case 0:
                case 7:
                    return 4.8f;
                case 4:
                case 6:
                    return 1.4f;
                case 2:
                case 8:
                    return 5.2f;
                case 12:
                case 13:
                    return 5.4f;
                case 3:
                case 9:
                    return 2.3f;
                case 1:
                case 5:
                case 10:
                    return 1.2f;
                case 11:
                    return 1.8f;
                default:
                    return 1f;
            }
        }

        public string GetWeaponNameFromType(int weaponType)
        {
            switch (weaponType)
            {
                case 0:
                    return "Great Sword";
                case 1:
                    return "Heavy Bowgun";
                case 2:
                    return "Hammer";
                case 3:
                    return "Lance";
                case 4:
                    return "Sword and Shield";
                case 5:
                    return "Light Bowgun";
                case 6:
                    return "Dual Swords";
                case 7:
                    return "Long Sword";
                case 8:
                    return "Hunting Horn";
                case 9:
                    return "Gunlance";
                case 10:
                    return "Bow";
                case 11:
                    return "Tonfa";
                case 12:
                    return "Switch Axe F";
                case 13:
                    return "Magnet Spike";
                case 14:
                    return "Group";
                default: 
                    return "";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void ReloadData()
        {
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
