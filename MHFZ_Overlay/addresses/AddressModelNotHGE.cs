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

        public override int CaravanScore() => M.ReadInt("mhfo.dll+6154FC4");

        public override int CaravanMonster1ID() => M.ReadByte("mhfo.dll+28C2C84");
        //unsure
        public override int CaravanMonster2ID() => M.ReadByte("mhfo.dll+28C2C8C");


        public override int MeleeWeaponID() => 1;
        public override int RangedWeaponID() => 1;
            //TODO: Sigils
        public override int WeaponDeco1ID() => 1;
        public override int WeaponDeco2ID() => 1;
        public override int WeaponDeco3ID() => 1;
        public override int ArmorHeadID() => 1;
        public override int ArmorHeadDeco1ID() => 1;
        public override int ArmorHeadDeco2ID() => 1;
        public override int ArmorHeadDeco3ID() => 1;
        public override int ArmorChestID() => 1;
        public override int ArmorChestDeco1ID() => 1;
        public override int ArmorChestDeco2ID() => 1;
        public override int ArmorChestDeco3ID() => 1;
        public override int ArmorWaistID() => 1;
        public override int ArmorWaistDeco1ID() => 1;
        public override int ArmorWaistDeco2ID() => 1;
        public override int ArmorWaistDeco3ID() => 1;
        public override int ArmorLegsID() => 1;
        public override int ArmorLegsDeco1ID() => 1;
        public override int ArmorLegsDeco2ID() => 1;
        public override int ArmorLegsDeco3ID() => 1;
        public override int Cuff1ID() => 1;
        public override int Cuff2ID() => 1;
        public override int TotalDefense() => M.Read2Byte("mhfo.dll+5034338");
        public override int PouchItem1ID() => 1;
        public override int PouchItem1Qty() => 1;
        public override int PouchItem2ID() => 1;
        public override int PouchItem2Qty() => 1;
        public override int PouchItem3ID() => 1;
        public override int PouchItem3Qty() => 1;
        public override int PouchItem4ID() => 1;
        public override int PouchItem4Qty() => 1;
        public override int PouchItem5ID() => 1;
        public override int PouchItem5Qty() => 1;
        public override int PouchItem6ID() => 1;
        public override int PouchItem6Qty() => 1;
        public override int PouchItem7ID() => 1;
        public override int PouchItem7Qty() => 1;
        public override int PouchItem8ID() => 1;
        public override int PouchItem8Qty() => 1;
        public override int PouchItem9ID() => 1;
        public override int PouchItem9Qty() => 1;
        public override int PouchItem10ID() => 1;
        public override int PouchItem10Qty() => 1;
        public override int PouchItem11ID() => 1;
        public override int PouchItem11Qty() => 1;
        public override int PouchItem12ID() => 1;
        public override int PouchItem12Qty() => 1;
        public override int PouchItem13ID() => 1;
        public override int PouchItem13Qty() => 1;
        public override int PouchItem14ID() => 1;
        public override int PouchItem14Qty() => 1;
        public override int PouchItem15ID() => 1;
        public override int PouchItem15Qty() => 1;
        public override int PouchItem16ID() => 1;
        public override int PouchItem16Qty() => 1;
        public override int PouchItem17ID() => 1;
        public override int PouchItem17Qty() => 1;
        public override int PouchItem18ID() => 1;
        public override int PouchItem18Qty() => 1;
        public override int PouchItem19ID() => 1;
        public override int PouchItem19Qty() => 1;
        public override int PouchItem20ID() => 1;
        public override int PouchItem20Qty() => 1;
        public override int AmmoPouchItem1ID() => 1;
        public override int AmmoPouchItem2ID() => 1;
        public override int AmmoPouchItem3ID() => 1;
        public override int AmmoPouchItem4ID() => 1;
        public override int AmmoPouchItem5ID() => 1;
        public override int AmmoPouchItem6ID() => 1;
        public override int AmmoPouchItem7ID() => 1;
        public override int AmmoPouchItem8ID() => 1;
        public override int AmmoPouchItem9ID() => 1;
        public override int AmmoPouchItem10ID() => 1;
        //TODO: cat pouch

    }
}
