using Memory;

namespace MHFZ_Overlay.addresses
{

    /// Most Addresses from https://github.com/suzaku01/
    public class AddressModelNotHGE : AddressModel
    {
        public AddressModelNotHGE(Mem m) : base(m)
        {

        }

        
        //public override int HitCountInt() => M.Read2Byte("mhfo.dll+5CA3430");
        public override int HitCountInt() => M.Read2Byte("mhfo.dll+60792E6");
        public override int TimeDefInt() => M.ReadInt("mhfo.dll+1B97780");
        public override int TimeInt() => M.ReadInt("mhfo.dll+5BC6540");
        public override int WeaponRaw() => M.Read2Byte("mhfo.dll+503433A");
        //This is equipment slot number that goes from 0-255 repeatedly
        //public override int WeaponType() => M.ReadByte("mhfo.dll+60FFCC6");

        //public override int WeaponType() => M.ReadByte("mhfo.dll+B7FF45");
        //public override int WeaponType() => M.ReadByte("mhfo.dll+182D3B93");
        //public override int WeaponType() => M.ReadByte("mhfo.dll+13E1FF45");
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
        public override string Monster1DefMult() => M.ReadFloat("mhfo.dll+60A3E58,89C").ToString();
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
        public override string Monster2DefMult() => M.ReadFloat("mhfo.dll+60A3E58,178C").ToString();
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
        public override int UrukiPachinkoScore() => M.Read2Byte("mhfo.dll+61EC16C");
        public override int NyanrendoScore() => M.Read2Byte("mhfo.dll+61EC160");
        public override int DokkanBattleCatsScore() => M.Read2Byte("mhfo.dll+61EC158");
        public override int DokkanBattleCatsScale() => M.ReadByte("mhfo.dll+61EC2EC");
        public override int DokkanBattleCatsShell() => M.ReadByte("mhfo.dll+61EC2EE");
        public override int DokkanBattleCatsCamp() => M.ReadByte("mhfo.dll+61EC2EA");
        public override int GuukuScoopSmall() => M.ReadByte("mhfo.dll+61EC190");
        public override int GuukuScoopMedium() => M.ReadByte("mhfo.dll+61EC194");
        public override int GuukuScoopLarge() => M.ReadByte("mhfo.dll+61EC198");
        public override int GuukuScoopGolden() => M.ReadByte("mhfo.dll+61EC19C");
        public override int GuukuScoopScore() => M.Read2Byte("mhfo.dll+61EC184");
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

        //public override int PartnyaRank() => M.Read2Byte("mhfo.dll+5CA5520");
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


        public override int MeleeWeaponID() => M.Read2Byte("mhfo.dll+5033F92");
        
        public override int RangedWeaponID() => M.Read2Byte("mhfo.dll+5033F92");
            //TODO: Sigils
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

    }
}
