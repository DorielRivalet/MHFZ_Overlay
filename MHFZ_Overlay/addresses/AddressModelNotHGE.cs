// Copyright 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using Memory;
using MHFZ_Overlay;


// Most Addresses from https://github.com/suzaku01/ 
namespace MHFZ_Overlay.Addresses
{

    /// <summary>
    /// Inherits from AddressModel and provides the memory address of the hit count value (etc.) when the game is running in non-High Grade Edition (HGE) mode.
    /// </summary>
    public class AddressModelNotHGE : AddressModel
    {
        public AddressModelNotHGE(Mem m) : base(m)
        {

        }

        public override int HitCountInt() => M.Read2Byte("mhfo.dll+60792E6");
        //public override int TimeDefInt() => M.ReadInt("mhfo.dll+1B97780");
        public override int TimeDefInt() => M.ReadInt("mhfo.dll+28C2C70");
        public override int TimeInt() => M.ReadInt("mhfo.dll+5BC6540");
        // alternative timeint for dure? mhfo.dll+5BC7600
        public override int WeaponRaw() => M.Read2Byte("mhfo.dll+503433A");
        //This is equipment slot number that goes from 0-255 repeatedly
        //"mhfo.dll+60FFCC6

        //"mhfo.dll+B7FF45
        //"mhfo.dll+182D3B93
        //"mhfo.dll+13E1FF45
        public override int WeaponType() => M.ReadByte("mhfo.dll+5033B93");
        public override bool IsNotRoad()
        {
            return M.ReadByte("mhfo.dll+509C8B0") == 0;
        }
        public override int LargeMonster1ID()
        {
            return GetNotRoad() ? M.ReadByte("mhfo.dll+1B97794") : LargeMonster1Road();
        }
        public override int LargeMonster2ID()
        {
            return GetNotRoad() ? M.ReadByte("mhfo.dll+1B9779C") : LargeMonster2Road();
        }
        public override int LargeMonster3ID() => M.ReadByte("mhfo.dll+1B977A4");
        public override int LargeMonster4ID() => M.ReadByte("mhfo.dll+1B977AC");
        public int LargeMonster1Road()
        {
            return M.ReadByte("mhfo.dll+509C8B8");
        }
        public int LargeMonster2Road()
        {
            return M.ReadByte("mhfo.dll+509C8D8");
        }
        public string Monster1BP1 { get => M.Read2Byte("mhfo.dll+60A3E58,348").ToString(); }
        public override int Monster1Part1() => M.Read2Byte("mhfo.dll+60A3E58,348");
        public string Monster1BP2 { get => M.Read2Byte("mhfo.dll+60A3E58,350").ToString(); }
        public override int Monster1Part2() => M.Read2Byte("mhfo.dll+60A3E58,350");
        public string Monster1BP3 { get => M.Read2Byte("mhfo.dll+60A3E58,358").ToString(); }
        public override int Monster1Part3() => M.Read2Byte("mhfo.dll+60A3E58,358");
        public string Monster1BP4 { get => M.Read2Byte("mhfo.dll+60A3E58,360").ToString(); }
        public override int Monster1Part4() => M.Read2Byte("mhfo.dll+60A3E58,360");
        public string Monster1BP5 { get => M.Read2Byte("mhfo.dll+60A3E58,368").ToString(); }
        public override int Monster1Part5() => M.Read2Byte("mhfo.dll+60A3E58,368");
        public string Monster1BP6 { get => M.Read2Byte("mhfo.dll+60A3E58,370").ToString(); }
        public override int Monster1Part6() => M.Read2Byte("mhfo.dll+60A3E58,370");
        public string Monster1BP7 { get => M.Read2Byte("mhfo.dll+60A3E58,378").ToString(); }
        public override int Monster1Part7() => M.Read2Byte("mhfo.dll+60A3E58,378");
        public string Monster1BP8 { get => M.Read2Byte("mhfo.dll+60A3E58,380").ToString(); }
        public override int Monster1Part8() => M.Read2Byte("mhfo.dll+60A3E58,380");
        public string Monster1BP9 { get => M.Read2Byte("mhfo.dll+60A3E58,388").ToString(); }
        public override int Monster1Part9() => M.Read2Byte("mhfo.dll+60A3E58,388");
        public string Monster1BP10 { get => M.Read2Byte("mhfo.dll+60A3E58,390").ToString(); }
        public override int Monster1Part10() => M.Read2Byte("mhfo.dll+60A3E58,390");
        public string Monster2BP1 { get => M.Read2Byte("mhfo.dll+60A3E58,1238").ToString(); }
        public override int Monster2Part1() => M.Read2Byte("mhfo.dll+60A3E58,1238");
        public string Monster2BP2 { get => M.Read2Byte("mhfo.dll+60A3E58,1240").ToString(); }
        public override int Monster2Part2() => M.Read2Byte("mhfo.dll+60A3E58,1240");
        public string Monster2BP3 { get => M.Read2Byte("mhfo.dll+60A3E58,1248").ToString(); }
        public override int Monster2Part3() => M.Read2Byte("mhfo.dll+60A3E58,1248");
        public string Monster2BP4 { get => M.Read2Byte("mhfo.dll+60A3E58,1250").ToString(); }
        public override int Monster2Part4() => M.Read2Byte("mhfo.dll+60A3E58,1250");
        public string Monster2BP5 { get => M.Read2Byte("mhfo.dll+60A3E58,1258").ToString(); }
        public override int Monster2Part5() => M.Read2Byte("mhfo.dll+60A3E58,1258");
        public string Monster2BP6 { get => M.Read2Byte("mhfo.dll+60A3E58,1260").ToString(); }
        public override int Monster2Part6() => M.Read2Byte("mhfo.dll+60A3E58,1260");
        public string Monster2BP7 { get => M.Read2Byte("mhfo.dll+60A3E58,1268").ToString(); }
        public override int Monster2Part7() => M.Read2Byte("mhfo.dll+60A3E58,1268");
        public string Monster2BP8 { get => M.Read2Byte("mhfo.dll+60A3E58,1270").ToString(); }
        public override int Monster2Part8() => M.Read2Byte("mhfo.dll+60A3E58,1270");
        public string Monster2BP9 { get => M.Read2Byte("mhfo.dll+60A3E58,1278").ToString(); }
        public override int Monster2Part9() => M.Read2Byte("mhfo.dll+60A3E58,1278");
        public string Monster2BP10 { get => M.Read2Byte("mhfo.dll+60A3E58,1280").ToString(); }
        public override int Monster2Part10() => M.Read2Byte("mhfo.dll+60A3E58,1280");
        public override int Monster1HPInt() => M.Read2Byte("0043C600");
        public override int Monster2HPInt() => M.Read2Byte("0043C604");
        public override int Monster3HPInt() => M.Read2Byte("0043C608");
        public override int Monster4HPInt() => M.Read2Byte("0043C60C");
        public override string Monster1AtkMult() => M.ReadFloat("mhfo.dll+60A3E58,898").ToString();
        public override decimal Monster1DefMult() => (decimal)M.ReadFloat("mhfo.dll+60A3E58,89C", "", false);
        public override int Monster1Poison() => M.Read2Byte("mhfo.dll+60A3E58,88A");
        public override int Monster1PoisonNeed() => M.Read2Byte("mhfo.dll+60A3E58,888");
        public override int Monster1Sleep() => M.Read2Byte("mhfo.dll+60A3E58,86C");
        public override int Monster1SleepNeed() => M.Read2Byte("mhfo.dll+60A3E58,86A");
        public override int Monster1Para() => M.Read2Byte("mhfo.dll+60A3E58,886");
        public override int Monster1ParaNeed() => M.Read2Byte("mhfo.dll+60A3E58,880");
        public override int Monster1Blast() => M.Read2Byte("mhfo.dll+60A3E58,D4A");
        public override int Monster1BlastNeed() => M.Read2Byte("mhfo.dll+60A3E58,D48");
        public override int Monster1Stun() => M.Read2Byte("mhfo.dll+60A3E58,872");
        public override int Monster1StunNeed() => M.Read2Byte("mhfo.dll+60A3E58,A74");
        public override string Monster1Size() => M.Read2Byte("mhfo.dll+2AFA784").ToString() + "%";
        public override string Monster2AtkMult() => M.ReadFloat("mhfo.dll+60A3E58,1788").ToString();
        public override decimal Monster2DefMult() => (decimal)M.ReadFloat("mhfo.dll+60A3E58,178C", "", false);
        public override int Monster2Poison() => M.Read2Byte("mhfo.dll+60A3E58,177A");
        public override int Monster2PoisonNeed() => M.Read2Byte("mhfo.dll+60A3E58,1778");
        public override int Monster2Sleep() => M.Read2Byte("mhfo.dll+60A3E58,175C");
        public override int Monster2SleepNeed() => M.Read2Byte("mhfo.dll+60A3E58,175A");
        public override int Monster2Para() => M.Read2Byte("mhfo.dll+60A3E58,1776");
        public override int Monster2ParaNeed() => M.Read2Byte("mhfo.dll+60A3E58,1770");
        public override int Monster2Blast() => M.Read2Byte("mhfo.dll+60A3E58,1C3A");
        public override int Monster2BlastNeed() => M.Read2Byte("mhfo.dll+60A3E58,1C38");
        public override int Monster2Stun() => M.Read2Byte("mhfo.dll+60A3E58,1762");
        public override int Monster2StunNeed() => M.Read2Byte("mhfo.dll+60A3E58,1964");
        public override string Monster2Size() => M.Read2Byte("mhfo.dll+2AFA784").ToString() + "%";
        public override int DamageDealt() => M.Read2Byte("mhfo.dll+5CA3430");
        public override int RoadSelectedMonster() => M.ReadByte("mhfo.dll+001B48F4,4");

        //new addresses
        public override int AreaID() => M.Read2Byte("mhfo.dll+5034388");
        public override int RavienteAreaID() => M.Read2Byte("mhfo.dll+6124B6E");
        public override int GRankNumber() => M.Read2Byte("mhfo.dll+613DD30");
        public override int GSR() => M.Read2Byte("mhfo.dll+50349A2");
        public override int RoadFloor() => M.Read2Byte("mhfo.dll+5C47600");
        public override int WeaponStyle() => M.ReadByte("mhfo.dll+50348D2");
        public override int QuestID() => M.Read2Byte("mhfo.dll+5FB4A4C");
        public override int UrukiPachinkoFish() => M.ReadByte("mhfo.dll+61EC176");
        public override int UrukiPachinkoMushroom() => M.ReadByte("mhfo.dll+61EC178");
        public override int UrukiPachinkoSeed() => M.ReadByte("mhfo.dll+61EC17A");
        public override int UrukiPachinkoMeat() => M.ReadByte("mhfo.dll+61EC174");
        public override int UrukiPachinkoChain() => M.ReadByte("mhfo.dll+61EC160");
        public override int UrukiPachinkoScore() => M.ReadInt("mhfo.dll+61EC16C");
        public override int UrukiPachinkoBonusScore() => M.ReadInt("mhfo.dll+61EC170");
        public override int NyanrendoScore() => M.ReadInt("mhfo.dll+61EC160");
        public override int DokkanBattleCatsScore() => M.ReadInt("mhfo.dll+61EC158");
        public override int DokkanBattleCatsScale() => M.ReadByte("mhfo.dll+61EC2EC");
        public override int DokkanBattleCatsShell() => M.ReadByte("mhfo.dll+61EC2EE");
        public override int DokkanBattleCatsCamp() => M.ReadByte("mhfo.dll+61EC2EA");
        public override int GuukuScoopSmall() => M.ReadByte("mhfo.dll+61EC190");
        public override int GuukuScoopMedium() => M.ReadByte("mhfo.dll+61EC194");
        public override int GuukuScoopLarge() => M.ReadByte("mhfo.dll+61EC198");
        public override int GuukuScoopGolden() => M.ReadByte("mhfo.dll+61EC19C");
        public override int GuukuScoopScore() => M.ReadInt("mhfo.dll+61EC184");
        public override int PanicHoneyScore() => M.ReadByte("mhfo.dll+61EC168");
        public override int Sharpness() => M.Read2Byte("mhfo.dll+50346B6");
        public override int CaravanPoints() => M.ReadInt("mhfo.dll+6101894");
        public override int MezeportaFestivalPoints() => M.ReadInt("mhfo.dll+617FA4C");
        public override int DivaBond() => M.Read2Byte("mhfo.dll+61033A8");
        public override int DivaItemsGiven() => M.Read2Byte("mhfo.dll+61033AA");
        public override int GCP() => M.ReadInt("mhfo.dll+58CFA18");
        public override int RoadPoints() => M.ReadInt("mhfo.dll+61043F8");
        public override int ArmorColor() => M.ReadByte("mhfo.dll+6100476");
        public override int RaviGg() => M.ReadInt("mhfo.dll+6104188");
        public override int Ravig() => M.ReadInt("mhfo.dll+61003A0");
        public override int GZenny() => M.ReadInt("mhfo.dll+6100514");
        public override int GuildFoodSkill() => M.Read2Byte("mhfo.dll+5BC70D8");
        public override int GalleryEvaluationScore() => M.ReadInt("mhfo.dll+6103250");
        public override int PoogiePoints() => M.ReadByte("mhfo.dll+6100350");
        public override int PoogieItemUseID() => M.Read2Byte("mhfo.dll+61540F8");
        public override int PoogieCostume() => M.ReadByte("mhfo.dll+1A88392");


        //zero-indexed
        public override int CaravenGemLevel() => M.ReadByte("mhfo.dll+610037D");


        public override int RoadMaxStagesMultiplayer() => M.Read2Byte("mhfo.dll+5C47688");
        public override int RoadTotalStagesMultiplayer() => M.Read2Byte("mhfo.dll+5C47668");
        public override int RoadTotalStagesSolo() => M.Read2Byte("mhfo.dll+5C4766C");
        public override int RoadMaxStagesSolo() => M.Read2Byte("mhfo.dll+5C47690");
        public override int RoadFatalisSlain() => M.Read2Byte("mhfo.dll+5C47670");
        public override int RoadFatalisEncounters() => M.Read2Byte("mhfo.dll+5C47698");
        public override int FirstDistrictDuremudiraEncounters() => M.Read2Byte("mhfo.dll+6104414");
        public override int FirstDistrictDuremudiraSlays() => M.Read2Byte("mhfo.dll+6104FCC");
        public override int SecondDistrictDuremudiraEncounters() => M.Read2Byte("mhfo.dll+6104418");
        public override int SecondDistrictDuremudiraSlays() => M.Read2Byte("mhfo.dll+5C47678");
        public override int DeliveryQuestPoints() => M.Read2Byte("mhfo.dll+6100A72");

        //red is 0
        public override int SharpnessLevel() => M.ReadByte("mhfo.dll+50346BF");
        public override int PartnerLevel() => M.Read2Byte("mhfo.dll+574127E");

        //as hex, consult addresses README.md
        public override int ObjectiveType() => M.ReadInt("mhfo.dll+28C2C80");
        public override int DivaSkillUsesLeft() => M.ReadByte("mhfo.dll+610436A");
        public override int HalkFullness() => M.ReadByte("mhfo.dll+6101983");
        public override int RankBand() => M.ReadByte("mhfo.dll+28C2BD8");

        public override int PartnyaRankPoints() => M.ReadInt("mhfo.dll+5919554");

        public override int Objective1ID() => M.Read2Byte("mhfo.dll+28C2C84");

        public override int Objective1Quantity() => M.Read2Byte("mhfo.dll+28C2C86");

        public override int Objective1CurrentQuantityMonster() => M.Read2Byte("mhfo.dll+60792E6");
        public override int Objective1CurrentQuantityItem() => M.Read2Byte("mhfo.dll+5034732");

        public override int RavienteTriggeredEvent() => M.Read2Byte("mhfo.dll+61005C6");
        public override int GreatSlayingPoints() => M.ReadInt("mhfo.dll+5B45FF8");
        public override int GreatSlayingPointsSaved() => M.ReadInt("mhfo.dll+61005C4");

        public override int AlternativeMonster1HPInt() => 1;
        public override int AlternativeMonster1AtkMult() => 1;
        public override int AlternativeMonster1DefMult() => 1;
        public override int AlternativeMonster1Size() => 1;

        public override int AlternativeMonster1Poison() => 1;
        public override int AlternativeMonster1PoisonNeed() => 1;
        public override int AlternativeMonster1Sleep() => 1;
        public override int AlternativeMonster1SleepNeed() => 1;

        public override int AlternativeMonster1Para() => 1;
        public override int AlternativeMonster1ParaNeed() => 1;
        public override int AlternativeMonster1Blast() => 1;
        public override int AlternativeMonster1BlastNeed() => 1;

        public override int AlternativeMonster1Stun() => 1;
        public override int AlternativeMonster1StunNeed() => 1;

        public override int AlternativeMonster1Part1() => 1;
        public override int AlternativeMonster1Part2() => 1;
        public override int AlternativeMonster1Part3() => 1;
        public override int AlternativeMonster1Part4() => 1;
        public override int AlternativeMonster1Part5() => 1;
        public override int AlternativeMonster1Part6() => 1;
        public override int AlternativeMonster1Part7() => 1;
        public override int AlternativeMonster1Part8() => 1;
        public override int AlternativeMonster1Part9() => 1;
        public override int AlternativeMonster1Part10() => 1;

        public override int DivaSkill() => M.ReadByte("mhfo.dll+6104368");
        public override int StarGrades() => M.ReadByte("mhfo.dll+5B3D086");

        public override int CaravanSkill1() => M.ReadByte("mhfo.dll+5034888");
        public override int CaravanSkill2() => M.ReadByte("mhfo.dll+503488A");
        public override int CaravanSkill3() => M.ReadByte("mhfo.dll+503488C");

        public override int CurrentFaints() => M.ReadByte("mhfo.dll+503479B");
        public override int MaxFaints() => M.ReadByte("mhfo.dll+1AA899C");
        public override int AlternativeMaxFaints() => M.ReadByte("mhfo.dll+28C2C64");

        public override int CaravanScore() => M.ReadInt("mhfo.dll+6154FC4");

        public override int CaravanMonster1ID() => M.ReadByte("mhfo.dll+28C2C84");
        //unsure
        public override int CaravanMonster2ID() => M.ReadByte("mhfo.dll+28C2C8C");


        public override int BlademasterWeaponID() => M.Read2Byte("mhfo.dll+5033F92");

        public override int GunnerWeaponID() => M.Read2Byte("mhfo.dll+5033F92");
        //the deco addresses for the weapon includes the tower sigils
        public override int WeaponDeco1ID() => M.Read2Byte("mhfo.dll+5033F96");
        public override int WeaponDeco2ID() => M.Read2Byte("mhfo.dll+5033F98");
        public override int WeaponDeco3ID() => M.Read2Byte("mhfo.dll+5033F9A");
        public override int ArmorHeadID() => M.Read2Byte("mhfo.dll+5033F52");
        public override int ArmorHeadDeco1ID() => M.Read2Byte("mhfo.dll+5033F56");
        public override int ArmorHeadDeco2ID() => M.Read2Byte("mhfo.dll+5033F58");
        public override int ArmorHeadDeco3ID() => M.Read2Byte("mhfo.dll+5033F5A");
        public override int ArmorChestID() => M.Read2Byte("mhfo.dll+5033F62");
        public override int ArmorChestDeco1ID() => M.Read2Byte("mhfo.dll+5033F66");
        public override int ArmorChestDeco2ID() => M.Read2Byte("mhfo.dll+5033F68");
        public override int ArmorChestDeco3ID() => M.Read2Byte("mhfo.dll+5033F6A");
        public override int ArmorArmsID() => M.Read2Byte("mhfo.dll+5033F72");
        public override int ArmorArmsDeco1ID() => M.Read2Byte("mhfo.dll+5033F76");
        public override int ArmorArmsDeco2ID() => M.Read2Byte("mhfo.dll+5033F78");
        public override int ArmorArmsDeco3ID() => M.Read2Byte("mhfo.dll+5033F7A");
        public override int ArmorWaistID() => M.Read2Byte("mhfo.dll+5033F82");
        public override int ArmorWaistDeco1ID() => M.Read2Byte("mhfo.dll+5033F86");
        public override int ArmorWaistDeco2ID() => M.Read2Byte("mhfo.dll+5033F88");
        public override int ArmorWaistDeco3ID() => M.Read2Byte("mhfo.dll+5033F8A");
        public override int ArmorLegsID() => M.Read2Byte("mhfo.dll+5033F32");
        public override int ArmorLegsDeco1ID() => M.Read2Byte("mhfo.dll+5033F36");
        public override int ArmorLegsDeco2ID() => M.Read2Byte("mhfo.dll+5033F38");
        public override int ArmorLegsDeco3ID() => M.Read2Byte("mhfo.dll+5033F3A");
        public override int Cuff1ID() => M.Read2Byte("mhfo.dll+50348C2");
        public override int Cuff2ID() => M.Read2Byte("mhfo.dll+50348C4");
        //updates when checking guild card
        public override int TotalDefense() => M.Read2Byte("mhfo.dll+5034338");
        public override int PouchItem1ID() => M.Read2Byte("mhfo.dll+50345A8");
        public override int PouchItem1Qty() => M.Read2Byte("mhfo.dll+50345AA");
        public override int PouchItem2ID() => M.Read2Byte("mhfo.dll+50345B0");
        public override int PouchItem2Qty() => M.Read2Byte("mhfo.dll+50345B2");
        public override int PouchItem3ID() => M.Read2Byte("mhfo.dll+50345B8");
        public override int PouchItem3Qty() => M.Read2Byte("mhfo.dll+50345BA");
        public override int PouchItem4ID() => M.Read2Byte("mhfo.dll+50345C0");
        public override int PouchItem4Qty() => M.Read2Byte("mhfo.dll+50345C2");
        public override int PouchItem5ID() => M.Read2Byte("mhfo.dll+50345C8");
        public override int PouchItem5Qty() => M.Read2Byte("mhfo.dll+50345CA");
        public override int PouchItem6ID() => M.Read2Byte("mhfo.dll+50345D0");
        public override int PouchItem6Qty() => M.Read2Byte("mhfo.dll+50345D2");
        public override int PouchItem7ID() => M.Read2Byte("mhfo.dll+50345D8");
        public override int PouchItem7Qty() => M.Read2Byte("mhfo.dll+610445A");
        public override int PouchItem8ID() => M.Read2Byte("mhfo.dll+50345E0");
        public override int PouchItem8Qty() => M.Read2Byte("mhfo.dll+50345E2");
        public override int PouchItem9ID() => M.Read2Byte("mhfo.dll+50345E8");
        public override int PouchItem9Qty() => M.Read2Byte("mhfo.dll+610446A");
        public override int PouchItem10ID() => M.Read2Byte("mhfo.dll+50345F0");
        public override int PouchItem10Qty() => M.Read2Byte("mhfo.dll+50345F2");
        public override int PouchItem11ID() => M.Read2Byte("mhfo.dll+50345F8");
        public override int PouchItem11Qty() => M.Read2Byte("mhfo.dll+50345FA");
        public override int PouchItem12ID() => M.Read2Byte("mhfo.dll+5034600");
        public override int PouchItem12Qty() => M.Read2Byte("mhfo.dll+5034602");
        public override int PouchItem13ID() => M.Read2Byte("mhfo.dll+5034608");
        public override int PouchItem13Qty() => M.Read2Byte("mhfo.dll+503460A");
        public override int PouchItem14ID() => M.Read2Byte("mhfo.dll+5034610");
        public override int PouchItem14Qty() => M.Read2Byte("mhfo.dll+5034612");
        public override int PouchItem15ID() => M.Read2Byte("mhfo.dll+5034618");
        public override int PouchItem15Qty() => M.Read2Byte("mhfo.dll+503461A");
        public override int PouchItem16ID() => M.Read2Byte("mhfo.dll+5034620");
        public override int PouchItem16Qty() => M.Read2Byte("mhfo.dll+5034622");
        public override int PouchItem17ID() => M.Read2Byte("mhfo.dll+5034628");
        public override int PouchItem17Qty() => M.Read2Byte("mhfo.dll+503462A");
        public override int PouchItem18ID() => M.Read2Byte("mhfo.dll+5034630");
        public override int PouchItem18Qty() => M.Read2Byte("mhfo.dll+5034632");
        public override int PouchItem19ID() => M.Read2Byte("mhfo.dll+5034638");
        public override int PouchItem19Qty() => M.Read2Byte("mhfo.dll+61044BA");
        public override int PouchItem20ID() => M.Read2Byte("mhfo.dll+5034640");
        public override int PouchItem20Qty() => M.Read2Byte("mhfo.dll+5034642");
        public override int AmmoPouchItem1ID() => M.Read2Byte("mhfo.dll+5034648");
        public override int AmmoPouchItem1Qty() => M.Read2Byte("mhfo.dll+503464A");
        public override int AmmoPouchItem2ID() => M.Read2Byte("mhfo.dll+5034650");
        public override int AmmoPouchItem2Qty() => M.Read2Byte("mhfo.dll+5034652");
        public override int AmmoPouchItem3ID() => M.Read2Byte("mhfo.dll+5034658");
        public override int AmmoPouchItem3Qty() => M.Read2Byte("mhfo.dll+503465A");
        public override int AmmoPouchItem4ID() => M.Read2Byte("mhfo.dll+5034660");
        public override int AmmoPouchItem4Qty() => M.Read2Byte("mhfo.dll+5034662");
        public override int AmmoPouchItem5ID() => M.Read2Byte("mhfo.dll+5034668");
        public override int AmmoPouchItem5Qty() => M.Read2Byte("mhfo.dll+503466A");
        public override int AmmoPouchItem6ID() => M.Read2Byte("mhfo.dll+5034670");
        public override int AmmoPouchItem6Qty() => M.Read2Byte("mhfo.dll+5034672");
        public override int AmmoPouchItem7ID() => M.Read2Byte("mhfo.dll+5034678");
        public override int AmmoPouchItem7Qty() => M.Read2Byte("mhfo.dll+503467A");
        public override int AmmoPouchItem8ID() => M.Read2Byte("mhfo.dll+5034680");
        public override int AmmoPouchItem8Qty() => M.Read2Byte("mhfo.dll+5034682");
        public override int AmmoPouchItem9ID() => M.Read2Byte("mhfo.dll+5034688");
        public override int AmmoPouchItem9Qty() => M.Read2Byte("mhfo.dll+503468A");
        public override int AmmoPouchItem10ID() => M.Read2Byte("mhfo.dll+5034690");
        public override int AmmoPouchItem10Qty() => M.Read2Byte("mhfo.dll+5034692");
        //TODO: cat pouch

        //slots
        public override int ArmorSkill1() => M.Read2Byte("mhfo.dll+503475C");
        public override int ArmorSkill2() => M.Read2Byte("mhfo.dll+503475E");
        public override int ArmorSkill3() => M.Read2Byte("mhfo.dll+5034760");
        public override int ArmorSkill4() => M.Read2Byte("mhfo.dll+5034762");
        public override int ArmorSkill5() => M.Read2Byte("mhfo.dll+5034764");
        public override int ArmorSkill6() => M.Read2Byte("mhfo.dll+5034766");
        public override int ArmorSkill7() => M.Read2Byte("mhfo.dll+5034768");
        public override int ArmorSkill8() => M.Read2Byte("mhfo.dll+503476A");
        public override int ArmorSkill9() => M.Read2Byte("mhfo.dll+503476C");
        public override int ArmorSkill10() => M.Read2Byte("mhfo.dll+503476E");
        public override int ArmorSkill11() => M.Read2Byte("mhfo.dll+5034770");
        public override int ArmorSkill12() => M.Read2Byte("mhfo.dll+5034772");
        public override int ArmorSkill13() => M.Read2Byte("mhfo.dll+5034774");
        public override int ArmorSkill14() => M.Read2Byte("mhfo.dll+5034776");
        public override int ArmorSkill15() => M.Read2Byte("mhfo.dll+5034778");
        public override int ArmorSkill16() => M.Read2Byte("mhfo.dll+503477A");
        public override int ArmorSkill17() => M.Read2Byte("mhfo.dll+503477C");
        public override int ArmorSkill18() => M.Read2Byte("mhfo.dll+503477E");
        public override int ArmorSkill19() => M.Read2Byte("mhfo.dll+5034780");

        public override int BloatedWeaponAttack() => M.Read2Byte("mhfo.dll+5BC68C8");

        public override int ZenithSkill1() => M.ReadByte("mhfo.dll+51C16D8");
        public override int ZenithSkill2() => M.ReadByte("mhfo.dll+51C16DA");
        public override int ZenithSkill3() => M.ReadByte("mhfo.dll+51C16DC");
        public override int ZenithSkill4() => M.ReadByte("mhfo.dll+51C16DE");
        public override int ZenithSkill5() => M.ReadByte("mhfo.dll+51C16E0");
        public override int ZenithSkill6() => M.ReadByte("mhfo.dll+51C16E2");
        public override int ZenithSkill7() => M.ReadByte("mhfo.dll+51C16E4");

        public override int AutomaticSkillWeapon() => M.Read2Byte("mhfo.dll+5034792");
        public override int AutomaticSkillHead() => M.Read2Byte("mhfo.dll+503478A");
        public override int AutomaticSkillChest() => M.Read2Byte("mhfo.dll+503478C");
        public override int AutomaticSkillArms() => M.Read2Byte("mhfo.dll+503478E");
        public override int AutomaticSkillWaist() => M.Read2Byte("mhfo.dll+5034790");
        public override int AutomaticSkillLegs() => M.Read2Byte("mhfo.dll+5034786");

        public override int StyleRank1() => M.ReadByte("mhfo.dll+50348D3");
        public override int StyleRank2() => M.ReadByte("mhfo.dll+503499F");

        public override int GRWeaponLv() => M.Read2Byte("mhfo.dll+5033F94");
        public override int GRWeaponLvBowguns() => M.Read2Byte("mhfo.dll+5033F95");

        public override int Sigil1Name1() => M.ReadByte("mhfo.dll+5BF91E4");
        public override int Sigil1Value1() => M.ReadByte("mhfo.dll+5BF91EA");
        public override int Sigil1Name2() => M.ReadByte("mhfo.dll+5BF91E6");
        public override int Sigil1Value2() => M.ReadByte("mhfo.dll+5BF91EC");
        public override int Sigil1Name3() => M.ReadByte("mhfo.dll+5BF91E8");
        public override int Sigil1Value3() => M.ReadByte("mhfo.dll+5BF91EE");
        public override int Sigil2Name1() => M.ReadByte("mhfo.dll+5BF91F0");
        public override int Sigil2Value1() => M.ReadByte("mhfo.dll+5BF91F6");
        public override int Sigil2Name2() => M.ReadByte("mhfo.dll+5BF91F2");
        public override int Sigil2Value2() => M.ReadByte("mhfo.dll+5BF91F8");
        public override int Sigil2Name3() => M.ReadByte("mhfo.dll+5BF91F4");
        public override int Sigil2Value3() => M.ReadByte("mhfo.dll+5BF91FA");
        public override int Sigil3Name1() => M.ReadByte("mhfo.dll+5BF9604");
        public override int Sigil3Value1() => M.ReadByte("mhfo.dll+5BF960A");
        public override int Sigil3Name2() => M.ReadByte("mhfo.dll+5BF9606");
        public override int Sigil3Value2() => M.ReadByte("mhfo.dll+5BF960C");
        public override int Sigil3Name3() => M.ReadByte("mhfo.dll+5BF9608");
        public override int Sigil3Value3() => M.ReadByte("mhfo.dll+5BF960E");


        public override int FelyneHunted() => M.Read2Byte("mhfo.dll+6103A1E");
        public override int MelynxHunted() => M.Read2Byte("mhfo.dll+6103A3A");
        public override int ShakalakaHunted() => M.Read2Byte("mhfo.dll+6103A7E") + 0;
        public override int VespoidHunted() => M.Read2Byte("mhfo.dll+6103A32");
        public override int HornetaurHunted() => M.Read2Byte("mhfo.dll+6103A3C");
        public override int GreatThunderbugHunted() => M.Read2Byte("mhfo.dll+6103A7C");
        public override int KelbiHunted() => M.Read2Byte("mhfo.dll+6103A12");
        public override int MosswineHunted() => M.Read2Byte("mhfo.dll+6103A14");
        public override int AntekaHunted() => M.Read2Byte("mhfo.dll+6103A96");
        public override int PopoHunted() => M.Read2Byte("mhfo.dll+6103A98");
        public override int AptonothHunted() => M.Read2Byte("mhfo.dll+6103A24");
        public override int ApcerosHunted() => M.Read2Byte("mhfo.dll+6103A3E");
        public override int BurukkuHunted() => M.Read2Byte("mhfo.dll+6103ACE");
        public override int ErupeHunted() => M.Read2Byte("mhfo.dll+6103AD0");
        public override int VelocipreyHunted() => M.Read2Byte("mhfo.dll+6103A2C");
        public override int VelocidromeHunted() => M.Read2Byte("mhfo.dll+6103A42");
        public override int GenpreyHunted() => M.Read2Byte("mhfo.dll+6103A26");
        public override int GendromeHunted() => M.Read2Byte("mhfo.dll+6103A44");
        public override int IopreyHunted() => M.Read2Byte("mhfo.dll+6103A48");
        public override int IodromeHunted() => M.Read2Byte("mhfo.dll+6103A4A");
        public override int GiapreyHunted() => M.Read2Byte("mhfo.dll+6103A52");
        public override int YianKutKuHunted() => M.Read2Byte("mhfo.dll+6103A18");
        public override int BlueYianKutKuHunted() => M.Read2Byte("mhfo.dll+6103A58");
        public override int YianGarugaHunted() => M.Read2Byte("mhfo.dll+6103A5C");
        public override int GypcerosHunted() => M.Read2Byte("mhfo.dll+6103A34");
        public override int PurpleGypcerosHunted() => M.Read2Byte("mhfo.dll+6103A5A");
        public override int HypnocHunted() => M.Read2Byte("mhfo.dll+6103AA0");
        public override int BrightHypnocHunted() => M.Read2Byte("mhfo.dll+6103AA8");
        public override int SilverHypnocHunted() => M.Read2Byte("mhfo.dll+6103AB0");
        public override int FarunokkuHunted() => M.Read2Byte("mhfo.dll+6103AF0");
        public override int ForokururuHunted() => M.Read2Byte("mhfo.dll+6103B06");
        public override int ToridclessHunted() => M.Read2Byte("mhfo.dll+6103B26");
        public override int RemobraHunted() => M.Read2Byte("mhfo.dll+6103A8A");
        public override int RathianHunted() => M.Read2Byte("mhfo.dll+6103A0E");
        public override int PinkRathianHunted() => M.Read2Byte("mhfo.dll+6103A56");
        public override int GoldRathianHunted() => M.Read2Byte("mhfo.dll+6103A60");
        public override int RathalosHunted() => M.Read2Byte("mhfo.dll+6103A22");
        public override int AzureRathalosHunted() => M.Read2Byte("mhfo.dll+6103A6E");
        public override int SilverRathalosHunted() => M.Read2Byte("mhfo.dll+6103A5E");
        public override int KhezuHunted() => M.Read2Byte("mhfo.dll+6103A2A");
        public override int RedKhezuHunted() => M.Read2Byte("mhfo.dll+6103A66");
        public override int BasariosHunted() => M.Read2Byte("mhfo.dll+6103A38");
        public override int GraviosHunted() => M.Read2Byte("mhfo.dll+6103A2E");
        public override int BlackGraviosHunted() => M.Read2Byte("mhfo.dll+6103A6A");
        public override int MonoblosHunted() => M.Read2Byte("mhfo.dll+6103A40");
        public override int WhiteMonoblosHunted() => M.Read2Byte("mhfo.dll+6103A64");
        public override int DiablosHunted() => M.Read2Byte("mhfo.dll+6103A28");
        public override int BlackDiablosHunted() => M.Read2Byte("mhfo.dll+6103A62");
        public override int TigrexHunted() => M.Read2Byte("mhfo.dll+6103AA4");
        public override int EspinasHunted() => M.Read2Byte("mhfo.dll+6103AAC");
        public override int OrangeEspinasHunted() => M.Read2Byte("mhfo.dll+6103AAE");
        public override int WhiteEspinasHunted() => M.Read2Byte("mhfo.dll+6103AC0");
        public override int AkantorHunted() => M.Read2Byte("mhfo.dll+6103AA6");
        public override int BerukyurosuHunted() => M.Read2Byte("mhfo.dll+6103AB6");
        public override int DoragyurosuHunted() => M.Read2Byte("mhfo.dll+6103ACA");
        public override int PariapuriaHunted() => M.Read2Byte("mhfo.dll+6103ABE");
        public override int DyuragauaHunted() => M.Read2Byte("mhfo.dll+6103AC8");
        public override int GurenzeburuHunted() => M.Read2Byte("mhfo.dll+6103ACC");
        public override int OdibatorasuHunted() => M.Read2Byte("mhfo.dll+6103AE0");
        public override int HyujikikiHunted() => M.Read2Byte("mhfo.dll+6103AE8");
        public override int AnorupatisuHunted() => M.Read2Byte("mhfo.dll+6103AE6");
        public override int ZerureusuHunted() => M.Read2Byte("mhfo.dll+6103B00") + 0;
        public override int MeraginasuHunted() => M.Read2Byte("mhfo.dll+6103B08");
        public override int DiorexHunted() => M.Read2Byte("mhfo.dll+6103B0A");
        public override int PoborubarumuHunted() => M.Read2Byte("mhfo.dll+6103B12");
        public override int VarusaburosuHunted() => M.Read2Byte("mhfo.dll+6103B10");
        public override int GureadomosuHunted() => M.Read2Byte("mhfo.dll+6103B22");
        public override int BariothHunted() => M.Read2Byte("mhfo.dll+6103B3A");
        //musous are separate???
        public override int NargacugaHunted() => M.Read2Byte("mhfo.dll+6103B4A") + 0;
        public override int ZenaserisuHunted() => M.Read2Byte("mhfo.dll+6103B4E");
        public override int SeregiosHunted() => M.Read2Byte("mhfo.dll+6103B5E");
        public override int BogabadorumuHunted() => M.Read2Byte("mhfo.dll+6103B60");
        public override int CephalosHunted() => M.Read2Byte("mhfo.dll+6103A50");
        public override int CephadromeHunted() => M.Read2Byte("mhfo.dll+6103A1C");
        public override int PlesiothHunted() => M.Read2Byte("mhfo.dll+6103A36");
        public override int GreenPlesiothHunted() => M.Read2Byte("mhfo.dll+6103A68");
        public override int VolganosHunted() => M.Read2Byte("mhfo.dll+6103AA2");
        public override int RedVolganosHunted() => M.Read2Byte("mhfo.dll+6103AAA");
        public override int HermitaurHunted() => M.Read2Byte("mhfo.dll+6103A90");
        public override int DaimyoHermitaurHunted() => M.Read2Byte("mhfo.dll+6103A6C");
        public override int CeanataurHunted() => M.Read2Byte("mhfo.dll+6103A9E");
        public override int ShogunCeanataurHunted() => M.Read2Byte("mhfo.dll+6103A92");
        public override int ShenGaorenHunted() => M.Read2Byte("mhfo.dll+6103A7A");
        public override int AkuraVashimuHunted() => M.Read2Byte("mhfo.dll+6103AB2");
        public override int AkuraJebiaHunted() => M.Read2Byte("mhfo.dll+6103AB4");
        public override int TaikunZamuzaHunted() => M.Read2Byte("mhfo.dll+6103ADA");
        public override int KusubamiHunted() => M.Read2Byte("mhfo.dll+6103B2A");
        public override int BullfangoHunted() => M.Read2Byte("mhfo.dll+6103A16");
        public override int BulldromeHunted() => M.Read2Byte("mhfo.dll+6103A94");
        public override int CongaHunted() => M.Read2Byte("mhfo.dll+6103A88");
        public override int CongalalaHunted() => M.Read2Byte("mhfo.dll+6103A74");
        public override int BlangoHunted() => M.Read2Byte("mhfo.dll+6103A86");
        public override int BlangongaHunted() => M.Read2Byte("mhfo.dll+6103A72");
        public override int GogomoaHunted() => M.Read2Byte("mhfo.dll+6103AD6");
        public override int RajangHunted() => M.Read2Byte("mhfo.dll+6103A76");
        public override int KamuOrugaronHunted() => M.Read2Byte("mhfo.dll+6103AC2");
        public override int NonoOrugaronHunted() => M.Read2Byte("mhfo.dll+6103AC4");
        public override int MidogaronHunted() => M.Read2Byte("mhfo.dll+6103AEA");
        public override int GougarfHunted() => M.Read2Byte("mhfo.dll+6103B02");
        public override int VoljangHunted() => M.Read2Byte("mhfo.dll+6103B48");
        public override int KirinHunted() => M.Read2Byte("mhfo.dll+6103A4E");
        public override int KushalaDaoraHunted() => M.Read2Byte("mhfo.dll+6103A78");
        public override int RustedKushalaDaoraHunted() => M.Read2Byte("mhfo.dll+6103A84");
        public override int ChameleosHunted() => M.Read2Byte("mhfo.dll+6103A82");
        public override int LunastraHunted() => M.Read2Byte("mhfo.dll+6103A8C");
        public override int TeostraHunted() => M.Read2Byte("mhfo.dll+6103A8E");
        public override int LaoShanLungHunted() => M.Read2Byte("mhfo.dll+6103A1A");
        public override int AshenLaoShanLungHunted() => M.Read2Byte("mhfo.dll+6103A70");//untested
        public override int YamaTsukamiHunted() => M.Read2Byte("mhfo.dll+6103A80");
        public override int RukodioraHunted() => M.Read2Byte("mhfo.dll+6103AD2");
        public override int RebidioraHunted() => M.Read2Byte("mhfo.dll+6103AE4");
        public override int FatalisHunted() => M.Read2Byte("mhfo.dll+6103A10");
        public override int ShantienHunted() => M.Read2Byte("mhfo.dll+6103AF4");
        public override int DisufiroaHunted() => M.Read2Byte("mhfo.dll+6103AE2");
        public override int GarubaDaoraHunted() => M.Read2Byte("mhfo.dll+6103B0C");
        public override int InagamiHunted() => M.Read2Byte("mhfo.dll+6103B0E");
        public override int HarudomeruguHunted() => M.Read2Byte("mhfo.dll+6103B24");
        public override int YamaKuraiHunted() => M.Read2Byte("mhfo.dll+6103B2C");
        public override int ToaTesukatoraHunted() => M.Read2Byte("mhfo.dll+6103B38");
        public override int GuanzorumuHunted() => M.Read2Byte("mhfo.dll+6103B40");
        public override int KeoaruboruHunted() => M.Read2Byte("mhfo.dll+6103B4C");
        public override int ShagaruMagalaHunted() => M.Read2Byte("mhfo.dll+6103B54");
        public override int ElzelionHunted() => M.Read2Byte("mhfo.dll+6103B58");
        public override int AmatsuHunted() => M.Read2Byte("mhfo.dll+6103B56");
        public override int AbioruguHunted() => M.Read2Byte("mhfo.dll+6103ADC");
        public override int GiaoruguHunted() => M.Read2Byte("mhfo.dll+6103AEC");
        public override int GasurabazuraHunted() => M.Read2Byte("mhfo.dll+6103B28");
        public override int DeviljhoHunted() => M.Read2Byte("mhfo.dll+6103B32");
        public override int BrachydiosHunted() => M.Read2Byte("mhfo.dll+6103B34");
        public override int UragaanHunted() => M.Read2Byte("mhfo.dll+6103B3C");
        public override int KuarusepusuHunted() => M.Read2Byte("mhfo.dll+6103ADE");
        public override int PokaraHunted() => M.Read2Byte("mhfo.dll+6103AF6");
        public override int PokaradonHunted() => M.Read2Byte("mhfo.dll+6103AF2");
        public override int BaruragaruHunted() => M.Read2Byte("mhfo.dll+6103AFE");
        public override int ZinogreHunted() => M.Read2Byte("mhfo.dll+6103B30");
        public override int StygianZinogreHunted() => M.Read2Byte("mhfo.dll+6103B3E");
        public override int GoreMagalaHunted() => M.Read2Byte("mhfo.dll+6103B50");
        public override int BlitzkriegBogabadorumuHunted() => M.Read2Byte("mhfo.dll+6103B64");
        public override int SparklingZerureusuHunted() => M.Read2Byte("mhfo.dll+6103B68");
        public override int StarvingDeviljhoHunted() => M.Read2Byte("mhfo.dll+6103B42");
        public override int CrimsonFatalisHunted() => M.Read2Byte("mhfo.dll+6103A54");
        public override int WhiteFatalisHunted() => M.Read2Byte("mhfo.dll+6103A9A");
        public override int CactusHunted() => M.Read2Byte("mhfo.dll+6103AB8");
        public override int ArrogantDuremudiraHunted() => M.Read2Byte("mhfo.dll+6103B5A");//untested
        public override int KingShakalakaHunted() => M.Read2Byte("mhfo.dll+6103B6C");
        public override int MiRuHunted() => M.Read2Byte("mhfo.dll+6103AEE");
        public override int UnknownHunted() => M.Read2Byte("mhfo.dll+6103AD4");
        public override int GoruganosuHunted() => M.Read2Byte("mhfo.dll+6103AFA");
        public override int AruganosuHunted() => M.Read2Byte("mhfo.dll+6103AFC");
        public override int PSO2RappyHunted() => M.Read2Byte("mhfo.dll+6103B6A");
        public override int RocksHunted() => M.Read2Byte("mhfo.dll+6103A46");
        public override int UrukiHunted() => M.Read2Byte("mhfo.dll+6103B04");
        public override int GorgeObjectsHunted() => M.Read2Byte("mhfo.dll+6103ABA");
        public override int BlinkingNargacugaHunted() => M.Read2Byte("mhfo.dll+6103B52");


        public override int QuestState() => M.ReadByte("mhfo.dll+61180F2");


        public override int RoadDureSkill1Name() => M.ReadByte("mhfo.dll+610403C");
        public override int RoadDureSkill1Level() => M.ReadByte("mhfo.dll+610403E");
        public override int RoadDureSkill2Name() => M.ReadByte("mhfo.dll+6104040");
        public override int RoadDureSkill2Level() => M.ReadByte("mhfo.dll+6104042");
        public override int RoadDureSkill3Name() => M.ReadByte("mhfo.dll+6104044");
        public override int RoadDureSkill3Level() => M.ReadByte("mhfo.dll+6104046");
        public override int RoadDureSkill4Name() => M.ReadByte("mhfo.dll+6104048");
        public override int RoadDureSkill4Level() => M.ReadByte("mhfo.dll+610404A");
        public override int RoadDureSkill5Name() => M.ReadByte("mhfo.dll+610404C");
        public override int RoadDureSkill5Level() => M.ReadByte("mhfo.dll+610404E");
        public override int RoadDureSkill6Name() => M.ReadByte("mhfo.dll+6104050");
        public override int RoadDureSkill6Level() => M.ReadByte("mhfo.dll+6104052");
        public override int RoadDureSkill7Name() => M.ReadByte("mhfo.dll+6104054");
        public override int RoadDureSkill7Level() => M.ReadByte("mhfo.dll+6104056");
        public override int RoadDureSkill8Name() => M.ReadByte("mhfo.dll+6104058");
        public override int RoadDureSkill8Level() => M.ReadByte("mhfo.dll+610405A");
        public override int RoadDureSkill9Name() => M.ReadByte("mhfo.dll+610405C");
        public override int RoadDureSkill9Level() => M.ReadByte("mhfo.dll+610405E");
        public override int RoadDureSkill10Name() => M.ReadByte("mhfo.dll+6104060");
        public override int RoadDureSkill10Level() => M.ReadByte("mhfo.dll+6104062");
        public override int RoadDureSkill11Name() => M.ReadByte("mhfo.dll+6104064");
        public override int RoadDureSkill11Level() => M.ReadByte("mhfo.dll+6104066");
        public override int RoadDureSkill12Name() => M.ReadByte("mhfo.dll+6104068");
        public override int RoadDureSkill12Level() => M.ReadByte("mhfo.dll+610406A");
        public override int RoadDureSkill13Name() => M.ReadByte("mhfo.dll+610406C");
        public override int RoadDureSkill13Level() => M.ReadByte("mhfo.dll+610406E");
        public override int RoadDureSkill14Name() => M.ReadByte("mhfo.dll+6104070");
        public override int RoadDureSkill14Level() => M.ReadByte("mhfo.dll+6104072");
        public override int RoadDureSkill15Name() => M.ReadByte("mhfo.dll+6104074");
        public override int RoadDureSkill15Level() => M.ReadByte("mhfo.dll+6104076");
        public override int RoadDureSkill16Name() => M.ReadByte("mhfo.dll+6104078");
        public override int RoadDureSkill16Level() => M.ReadByte("mhfo.dll+610407A");

        public override int PartySize() => M.ReadByte("mhfo.dll+57967C8");
        public override int PartySizeMax() => M.ReadByte("mhfo.dll+61B6088");

        public override uint GSRP() => (uint)M.ReadInt("mhfo.dll+61041C8");
        public override uint GRP() => (uint)M.ReadInt("mhfo.dll+5BC82F8");

        public override int HunterHP() => M.Read2Byte("mhfo.dll+5BC6548");
        public override int HunterStamina() => M.Read2Byte("mhfo.dll+503438C");
        public override int QuestItemsUsed() => M.Read2Byte("mhfo.dll+57EAE24");
        public override int AreaHitsTakenBlocked() => M.Read2Byte("mhfo.dll+5034078");

        // TODO Untested
        public override int PartnyaBagItem1ID() => M.Read2Byte("mhfo.dll+5745788");
        public override int PartnyaBagItem1Qty() => M.Read2Byte("mhfo.dll+574578A");

        public override int PartnyaBagItem2ID() => M.Read2Byte("mhfo.dll+574578C");

        public override int PartnyaBagItem2Qty() => M.Read2Byte("mhfo.dll+574578E");

        public override int PartnyaBagItem3ID() => M.Read2Byte("mhfo.dll+5745790");

        public override int PartnyaBagItem3Qty() => M.Read2Byte("mhfo.dll+5745792");

        public override int PartnyaBagItem4ID() => M.Read2Byte("mhfo.dll+5745794");

        public override int PartnyaBagItem4Qty() => M.Read2Byte("mhfo.dll+5745796");

        public override int PartnyaBagItem5ID() => M.Read2Byte("mhfo.dll+5745798");

        public override int PartnyaBagItem5Qty() => M.Read2Byte("mhfo.dll+574579A");

        public override int PartnyaBagItem6ID() => M.Read2Byte("mhfo.dll+574579C");

        public override int PartnyaBagItem6Qty() => M.Read2Byte("mhfo.dll+574579E");

        public override int PartnyaBagItem7ID() => M.Read2Byte("mhfo.dll+57457A0");

        public override int PartnyaBagItem7Qty() => M.Read2Byte("mhfo.dll+57457A2");

        public override int PartnyaBagItem8ID() => M.Read2Byte("mhfo.dll+57457A4");

        public override int PartnyaBagItem8Qty() => M.Read2Byte("mhfo.dll+57457A6");

        public override int PartnyaBagItem9ID() => M.Read2Byte("mhfo.dll+57457A8");

        public override int PartnyaBagItem9Qty() => M.Read2Byte("mhfo.dll+57457AA");

        public override int PartnyaBagItem10ID() => M.Read2Byte("mhfo.dll+57457AC");

        public override int PartnyaBagItem10Qty() => M.Read2Byte("mhfo.dll+57457AE");

    }
}
