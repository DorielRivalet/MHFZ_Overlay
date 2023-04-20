using Memory;

namespace MHFZ_Overlay.addresses
{

    /// Most Addresses from https://github.com/suzaku01/
    internal class AddressModelHGE : AddressModel
    {
        public AddressModelHGE(Mem m) : base(m)
        {

        }

        public override int HitCountInt() => M.Read2Byte("mhfo-hd.dll+ECB2DC6");
        public override int TimeDefInt() => M.ReadInt("mhfo-hd.dll+2AFA820");
        public override int TimeInt() => M.ReadInt("mhfo-hd.dll+E7FE170");
        public override int WeaponRaw() => M.Read2Byte("mhfo-hd.dll+DC6BEFA");

        //This is equipment slot number that goes from 0-255 repeatedly
        //"mhfo-hd.dll+ED3A466
        public override int WeaponType() => M.ReadByte("mhfo-hd.dll+DC6B753");
        public override bool IsNotRoad()
        {
            return M.ReadByte("mhfo-hd.dll+DCD4490") == 0;
        }
        public override int LargeMonster1ID()
        {
            return GetNotRoad() ? M.ReadByte("mhfo-hd.dll+1BEF3D9") : LargeMonster1Road();
        }
        public override int LargeMonster2ID()
        {
            return GetNotRoad() ? M.ReadByte("mhfo-hd.dll+1BEF3DA") : LargeMonster2Road();
        }
        public override int LargeMonster3ID() => M.ReadByte("mhfo-hd.dll+1BEF3DB");
        public override int LargeMonster4ID() => M.ReadByte("mhfo-hd.dll+1BEF3DC");
        public int LargeMonster1Road()
        {
            return M.ReadByte("mhfo-hd.dll+DCD4478");
        }
        public int LargeMonster2Road()
        {
            return M.ReadByte("mhfo-hd.dll+DCD4498");
        }
        public string Monster1BP1() => M.Read2Byte("mhfo-hd.dll+0E37DD38,348").ToString();
        public override int Monster1Part1() => M.Read2Byte("mhfo-hd.dll+0E37DD38,348");
        public string Monster1BP2() => M.Read2Byte("mhfo-hd.dll+0E37DD38,350").ToString();
        public override int Monster1Part2() => M.Read2Byte("mhfo-hd.dll+0E37DD38,350");

        public string Monster1BP3() => M.Read2Byte("mhfo-hd.dll+0E37DD38,358").ToString();
        public override int Monster1Part3() => M.Read2Byte("mhfo-hd.dll+0E37DD38,358");

        public string Monster1BP4() => M.Read2Byte("mhfo-hd.dll+0E37DD38,360").ToString();
        public override int Monster1Part4() => M.Read2Byte("mhfo-hd.dll+0E37DD38,360");

        public string Monster1BP5() => M.Read2Byte("mhfo-hd.dll+0E37DD38,368").ToString();
        public override int Monster1Part5() => M.Read2Byte("mhfo-hd.dll+0E37DD38,368");

        public string Monster1BP6() => M.Read2Byte("mhfo-hd.dll+0E37DD38,370").ToString();
        public override int Monster1Part6() => M.Read2Byte("mhfo-hd.dll+0E37DD38,370");

        public string Monster1BP7() => M.Read2Byte("mhfo-hd.dll+0E37DD38,378").ToString();
        public override int Monster1Part7() => M.Read2Byte("mhfo-hd.dll+0E37DD38,378");

        public string Monster1BP8() => M.Read2Byte("mhfo-hd.dll+0E37DD38,380").ToString();
        public override int Monster1Part8() => M.Read2Byte("mhfo-hd.dll+0E37DD38,380");

        public string Monster1BP9() => M.Read2Byte("mhfo-hd.dll+0E37DD38,388").ToString();
        public override int Monster1Part9() => M.Read2Byte("mhfo-hd.dll+0E37DD38,388");

        public string Monster1BP10() => M.Read2Byte("mhfo-hd.dll+0E37DD38,390").ToString();
        public override int Monster1Part10() => M.Read2Byte("mhfo-hd.dll+0E37DD38,390");

        public string Monster2BP1() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1238").ToString();
        public override int Monster2Part1() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1238");

        public string Monster2BP2() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1240").ToString();
        public override int Monster2Part2() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1240");

        public string Monster2BP3() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1248").ToString();
        public override int Monster2Part3() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1248");

        public string Monster2BP4() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1250").ToString();
        public override int Monster2Part4() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1250");

        public string Monster2BP5() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1258").ToString();
        public override int Monster2Part5() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1258");

        public string Monster2BP6() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1260").ToString();
        public override int Monster2Part6() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1260");

        public string Monster2BP7() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1268").ToString();
        public override int Monster2Part7() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1268");

        public string Monster2BP8() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1270").ToString();
        public override int Monster2Part8() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1270");

        public string Monster2BP9() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1278").ToString();
        public override int Monster2Part9() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1278");

        public string Monster2BP10() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1280").ToString();
        public override int Monster2Part10() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1280");

        public string Monster1RoadBP1() => M.Read2Byte("mhfo-hd.dll+E37DF18,348").ToString();
        public string Monster1RoadBP2() => M.Read2Byte("mhfo-hd.dll+E37DF18,350").ToString();
        public string Monster1RoadBP3() => M.Read2Byte("mhfo-hd.dll+E37DF18,358").ToString();
        public string Monster1RoadBP4() => M.Read2Byte("mhfo-hd.dll+E37DF18,360").ToString();
        public string Monster1RoadBP5() => M.Read2Byte("mhfo-hd.dll+E37DF18,368").ToString();
        public string Monster1RoadBP6() => M.Read2Byte("mhfo-hd.dll+E37DF18,370").ToString();
        public string Monster1RoadBP7() => M.Read2Byte("mhfo-hd.dll+E37DF18,378").ToString();
        public string Monster1RoadBP8() => M.Read2Byte("mhfo-hd.dll+E37DF18,380").ToString();
        public string Monster1RoadBP9() => M.Read2Byte("mhfo-hd.dll+E37DF18,388").ToString();
        public string Monster1RoadBP10() => M.Read2Byte("mhfo-hd.dll+E37DF18,390").ToString();
        public string Monster2RoadBP1() => M.Read2Byte("mhfo-hd.dll+E37DF18,1238").ToString();
        public string Monster2RoadBP2() => M.Read2Byte("mhfo-hd.dll+E37DF18,1240").ToString();
        public string Monster2RoadBP3() => M.Read2Byte("mhfo-hd.dll+E37DF18,1248").ToString();
        public string Monster2RoadBP4() => M.Read2Byte("mhfo-hd.dll+E37DF18,1250").ToString();
        public string Monster2RoadBP5() => M.Read2Byte("mhfo-hd.dll+E37DF18,1258").ToString();
        public string Monster2RoadBP6() => M.Read2Byte("mhfo-hd.dll+E37DF18,1260").ToString();
        public string Monster2RoadBP7() => M.Read2Byte("mhfo-hd.dll+E37DF18,1268").ToString();
        public string Monster2RoadBP8() => M.Read2Byte("mhfo-hd.dll+E37DF18,1270").ToString();
        public string Monster2RoadBP9() => M.Read2Byte("mhfo-hd.dll+E37DF18,1278").ToString();
        public string Monster2RoadBP10() => M.Read2Byte("mhfo-hd.dll+E37DF18,1280").ToString();



        public override int Monster1HPInt() => M.Read2Byte("0043C600");
        public override int Monster2HPInt() => M.Read2Byte("0043C604");
        public override int Monster3HPInt() => M.Read2Byte("0043C608");
        public override int Monster4HPInt() => M.Read2Byte("0043C60C");
        public override string Monster1AtkMult() { return GetNotRoad() ? M.ReadFloat("mhfo-hd.dll+0E37DD38,898").ToString() : Monster1RoadAtkMult(); }
        public override decimal Monster1DefMult() { return GetNotRoad() ? (decimal)M.ReadFloat("mhfo-hd.dll+0E37DD38,89C", "", false) : Monster1RoadDefMult(); }
        public override int Monster1Poison() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,88A") : Monster1RoadPoison(); }
        public override int Monster1PoisonNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,888") : Monster1RoadPoisonNeed(); }
        public override int Monster1Sleep() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,86C") : Monster1RoadSleep(); }
        public override int Monster1SleepNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,86A") : Monster1RoadSleepNeed(); }
        public override int Monster1Para() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,886") : Monster1RoadPara(); }
        public override int Monster1ParaNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,880") : Monster1RoadParaNeed(); }
        public override int Monster1Blast() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,D4A") : Monster1RoadBlast(); }
        public override int Monster1BlastNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,D48") : Monster1RoadBlastNeed(); }
        public override int Monster1Stun() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,872") : Monster1RoadStun(); }
        public override int Monster1StunNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,A74") : Monster1RoadStunNeed(); }
        public override string Monster1Size() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+2AFA784").ToString() + "%" : Monster1RoadSize(); }
        public override string Monster2AtkMult() { return GetNotRoad() ? M.ReadFloat("mhfo-hd.dll+0E37DD38,1788").ToString() : Monster2RoadAtkMult(); }
        public override decimal Monster2DefMult() { return GetNotRoad() ? (decimal)M.ReadFloat("mhfo-hd.dll+0E37DD38,178C", "", false) : Monster2RoadDefMult(); }
        public override int Monster2Poison() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,177A") : Monster2RoadPoison(); }
        public override int Monster2PoisonNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,1778") : Monster2RoadPoisonNeed(); }
        public override int Monster2Sleep() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,175C") : Monster2RoadSleep(); }
        public override int Monster2SleepNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,175A") : Monster2RoadSleepNeed(); }
        public override int Monster2Para() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,1776") : Monster2RoadPara(); }
        public override int Monster2ParaNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,1770") : Monster2RoadParaNeed(); }
        public override int Monster2Blast() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,1C3A") : Monster2RoadBlast(); }
        public override int Monster2BlastNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,1C38") : Monster2RoadBlastNeed(); }
        public override int Monster2Stun() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,1762") : Monster2RoadStun(); }
        public override int Monster2StunNeed() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+0E37DD38,1964") : Monster2RoadStunNeed(); }
        public override string Monster2Size() { return GetNotRoad() ? M.Read2Byte("mhfo-hd.dll+2AFA784").ToString() + "%" : Monster2RoadSize(); }
        public string Monster1RoadAtkMult() => M.ReadFloat("mhfo-hd.dll+E37DF18,898").ToString();
        public decimal Monster1RoadDefMult() => (decimal)M.ReadFloat("mhfo-hd.dll+E37DF18,89C", "", false);
        public int Monster1RoadPoison() => M.Read2Byte("mhfo-hd.dll+E37DF18,88A");
        public int Monster1RoadPoisonNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,888");
        public int Monster1RoadSleep() => M.Read2Byte("mhfo-hd.dll+E37DF18,86C");
        public int Monster1RoadSleepNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,86A");
        public int Monster1RoadPara() => M.Read2Byte("mhfo-hd.dll+E37DF18,886");
        public int Monster1RoadParaNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,880");
        public int Monster1RoadBlast() => M.Read2Byte("mhfo-hd.dll+E37DF18,D4A");
        public int Monster1RoadBlastNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,D48");
        public int Monster1RoadStun() => M.Read2Byte("mhfo-hd.dll+E37DF18,872");
        public int Monster1RoadStunNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,A74");
        public string Monster1RoadSize() => M.Read2Byte("mhfo-hd.dll+2AFA784").ToString() + "%";
        public string Monster2RoadAtkMult() => M.ReadFloat("mhfo-hd.dll+E37DF18,1788").ToString();
        public decimal Monster2RoadDefMult() => (decimal)M.ReadFloat("mhfo-hd.dll+E37DF18,178C", "", false);
        public int Monster2RoadPoison() => M.Read2Byte("mhfo-hd.dll+E37DF18,177A");
        public int Monster2RoadPoisonNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,1778");
        public int Monster2RoadSleep() => M.Read2Byte("mhfo-hd.dll+E37DF18,175C");
        public int Monster2RoadSleepNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,175A");
        public int Monster2RoadPara() => M.Read2Byte("mhfo-hd.dll+E37DF18,1776");
        public int Monster2RoadParaNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,1770");
        public int Monster2RoadBlast() => M.Read2Byte("mhfo-hd.dll+E37DF18,1C3A");
        public int Monster2RoadBlastNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,1C38");
        public int Monster2RoadStun() => M.Read2Byte("mhfo-hd.dll+E37DF18,1762");
        public int Monster2RoadStunNeed() => M.Read2Byte("mhfo-hd.dll+E37DF18,1964");
        public string Monster2RoadSize() => M.Read2Byte("mhfo-hd.dll+2AFA784").ToString() + "%";
        public override int DamageDealt() => M.Read2Byte("mhfo-hd.dll+E8DCF18");
        public override int RoadSelectedMonster() => M.ReadByte("mhfo-hd.dll+E87FB04");



        //new addresses
        public override int AreaID() => M.Read2Byte("mhfo-hd.dll+DC6BF48");
        public override int GRankNumber() => M.Read2Byte("mhfo-hd.dll+ED784D0");
        public override int GSR() => M.Read2Byte("mhfo-hd.dll+DC6C562");
        public override int RoadFloor() => M.Read2Byte("mhfo-hd.dll+E87FAF0");
        public override int WeaponStyle() => M.ReadByte("mhfo-hd.dll+DC6C492");
        public override int QuestID() => M.Read2Byte("mhfo-hd.dll+EBEE53C");
        public override int UrukiPachinkoFish() => M.ReadByte("mhfo-hd.dll+EE26916");
        public override int UrukiPachinkoMushroom() => M.ReadByte("mhfo-hd.dll+EE26918");
        public override int UrukiPachinkoSeed() => M.ReadByte("mhfo-hd.dll+EE2691A");
        public override int UrukiPachinkoMeat() => M.ReadByte("mhfo-hd.dll+EE26914");
        public override int UrukiPachinkoChain() => M.ReadByte("mhfo-hd.dll+EE26900");
        public override int UrukiPachinkoScore() => M.Read2Byte("mhfo-hd.dll+EE2690C");
        public override int UrukiPachinkoBonusScore() => M.Read2Byte("mhfo-hd.dl+EE26910");
        public override int NyanrendoScore() => M.Read2Byte("mhfo-hd.dll+EE26900");
        public override int DokkanBattleCatsScore() => M.Read2Byte("mhfo-hd.dll+EE268F8");
        public override int DokkanBattleCatsScale() => M.ReadByte("mhfo-hd.dll+EE26A8C");
        public override int DokkanBattleCatsShell() => M.ReadByte("mhfo-hd.dll+EE268F4");
        public override int DokkanBattleCatsCamp() => M.ReadByte("mhfo-hd.dll+EE26A8A");
        public override int GuukuScoopSmall() => M.ReadByte("mhfo-hd.dll+EE26930");
        public override int GuukuScoopMedium() => M.ReadByte("mhfo-hd.dll+EE26934");
        public override int GuukuScoopLarge() => M.ReadByte("mhfo-hd.dll+EE26938");
        public override int GuukuScoopGolden() => M.ReadByte("mhfo-hd.dll+EE2693C");
        public override int GuukuScoopScore() => M.Read2Byte("mhfo-hd.dll+EE26924");
        public override int PanicHoneyScore() => M.ReadByte("mhfo-hd.dll+EE26908");
        // TODO: Volpkun Together addresses
        public override int Sharpness() => M.Read2Byte("mhfo-hd.dll+DC6C276");
        public override int CaravanPoints() => M.ReadInt("mhfo-hd.dll+ED3C034");
        public override int MezeportaFestivalPoints() => M.ReadInt("mhfo-hd.dll+EDBA1EC");
        public override int DivaBond() => M.Read2Byte("mhfo-hd.dll+ED3DB48");
        public override int DivaItemsGiven() => M.Read2Byte("mhfo-hd.dll+ED3DB4A");
        public override int GCP() => M.ReadInt("mhfo-hd.dll+E5075D8");
        public override int RoadPoints() => M.ReadInt("mhfo-hd.dll+ED3EB98");
        public override int ArmorColor() => M.ReadByte("mhfo-hd.dll+EDE66A8");
        public override int RaviGg() => M.ReadInt("mhfo-hd.dll+ED3E928");
        public override int Ravig() => M.ReadInt("mhfo-hd.dll+ED3AB40");
        public override int GZenny() => M.ReadInt("mhfo-hd.dll+ED3ACB4");
        public override int GuildFoodSkill() => M.Read2Byte("mhfo-hd.dll+E7FED00");
        public override int GalleryEvaluationScore() => M.ReadInt("mhfo-hd.dll+ED3D9F0");
        public override int PoogiePoints() => M.ReadByte("mhfo-hd.dll+ED3AAF0");
        public override int PoogieItemUseID() => M.Read2Byte("mhfo-hd.dll+ED8E898");
        public override int PoogieCostume() => M.ReadByte("mhfo-hd.dll+1A77AF2");

        //zero-indexed
        public override int CaravenGemLevel() => M.ReadByte("mhfo-hd.dll+1C747F6");


        public override int RoadMaxStagesMultiplayer() => M.Read2Byte("mhfo-hd.dll+E87FB78");
        public override int RoadTotalStagesMultiplayer() => M.Read2Byte("mhfo-hd.dll+E87FB58");
        public override int RoadTotalStagesSolo() => M.Read2Byte("mhfo-hd.dll+E87FB5C");
        public override int RoadMaxStagesSolo() => M.Read2Byte("mhfo-hd.dll+E87FB80");
        public override int RoadFatalisSlain() => M.Read2Byte("mhfo-hd.dll+E87FB60");
        public override int RoadFatalisEncounters() => M.Read2Byte("mhfo-hd.dll+ED3EBBC");
        public override int FirstDistrictDuremudiraEncounters() => M.Read2Byte("mhfo-hd.dll+ED3EBB4");
        public override int FirstDistrictDuremudiraSlays() => M.Read2Byte("mhfo-hd.dll+E87FB64");
        public override int SecondDistrictDuremudiraEncounters() => M.Read2Byte("mhfo-hd.dll+ED3EBB8");
        public override int SecondDistrictDuremudiraSlays() => M.Read2Byte("mhfo-hd.dll+E87FB68");
        public override int DeliveryQuestPoints() => M.Read2Byte("mhfo-hd.dll+ED3B212");

        //red is 0
        public override int SharpnessLevel() => M.ReadByte("mhfo-hd.dll+DC6C27F");
        public override int PartnerLevel() => M.Read2Byte("mhfo-hd.dll+E378E3E");

        // as hex
        public override int ObjectiveType() => M.ReadInt("mhfo-hd.dll+2AFA830");


        public override int DivaSkillUsesLeft() => M.ReadByte("mhfo-hd.dll+ED3EB0A");
        public override int HalkFullness() => M.ReadByte("mhfo-hd.dll+ED3C123");
        public override int RankBand() => M.ReadByte("mhfo-hd.dll+2AFA788");
        public override int PartnyaRankPoints() => M.ReadInt("mhfo-hd.dll+E551114");

        public override int Objective1ID() => M.Read2Byte("mhfo-hd.dll+2AFA834");
        public override int Objective1Quantity() => M.Read2Byte("mhfo-hd.dll+2AFA836");
        public override int Objective1CurrentQuantityMonster() => M.Read2Byte("mhfo-hd.dll+ECB2A38");
        public override int Objective1CurrentQuantityItem() => M.Read2Byte("mhfo-hd.dll+DC6C2F2");

        public override int RavienteTriggeredEvent() => M.ReadByte("mhfo-hd.dll+ED3AD66");
        public override int RavienteAreaID() => M.Read2Byte("mhfo-hd.dll+ED5F30E");//untested
        public override int GreatSlayingPoints() => M.ReadInt("mhfo-hd.dll+ED3AD64");
        public override int GreatSlayingPointsSaved() => M.ReadInt("mhfo-hd.dll+E77DC20");

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

        public override int DivaSkill() => M.ReadByte("mhfo-hd.dll+ED3EB08");

        public override int StarGrades() => M.ReadByte("mhfo-hd.dll+E774CAE");

        public override int CurrentFaints() => M.ReadByte("mhfo-hd.dll+DC6C35B");
        public override int MaxFaints() => M.ReadByte("mhfo-hd.dll+C4C8FE8");
        public override int AlternativeMaxFaints() => M.ReadByte("mhfo-hd.dll+2AFA814");

        public override int CaravanSkill1() => M.ReadByte("mhfo-hd.dll+DC6C448");
        public override int CaravanSkill2() => M.ReadByte("mhfo-hd.dll+DC6C44A");
        public override int CaravanSkill3() => M.ReadByte("mhfo-hd.dll+DC6C44C");
        public override int CaravanScore() => M.ReadInt("mhfo-hd.dll+ED8F764");

        public override int CaravanMonster1ID() => M.ReadByte("mhfo-hd.dll+2AFA834");
        //unsure
        public override int CaravanMonster2ID() => M.ReadByte("mhfo-hd.dll+1C41D12");


        public override int BlademasterWeaponID() => M.Read2Byte("mhfo-hd.dll+DC6BB52");
        public override int GunnerWeaponID() => M.Read2Byte("mhfo-hd.dll+DC6BB52");
        public override int WeaponDeco1ID() => M.Read2Byte("mhfo-hd.dll+DC6BB56");
        public override int WeaponDeco2ID() => M.Read2Byte("mhfo-hd.dll+DC6BB58");
        public override int WeaponDeco3ID() => M.Read2Byte("mhfo-hd.dll+DC6BB5A");
        public override int ArmorHeadID() => M.Read2Byte("mhfo-hd.dll+DC6BB12");
        public override int ArmorHeadDeco1ID() => M.Read2Byte("mhfo-hd.dll+DC6BB16");
        public override int ArmorHeadDeco2ID() => M.Read2Byte("mhfo-hd.dll+DC6BB18");
        public override int ArmorHeadDeco3ID() => M.Read2Byte("mhfo-hd.dll+DC6BB1A");
        public override int ArmorChestID() => M.Read2Byte("mhfo-hd.dll+DC6BB22");
        public override int ArmorChestDeco1ID() => M.Read2Byte("mhfo-hd.dll+DC6BB26");
        public override int ArmorChestDeco2ID() => M.Read2Byte("mhfo-hd.dll+DC6BB28");
        public override int ArmorChestDeco3ID() => M.Read2Byte("mhfo-hd.dll+DC6BB2A");
        public override int ArmorArmsID() => M.Read2Byte("mhfo-hd.dll+DC6BB32");
        public override int ArmorArmsDeco1ID() => M.Read2Byte("mhfo-hd.dll+DC6BB36");
        public override int ArmorArmsDeco2ID() => M.Read2Byte("mhfo-hd.dll+DC6BB38");
        public override int ArmorArmsDeco3ID() => M.Read2Byte("mhfo-hd.dll+DC6BB3A");
        public override int ArmorWaistID() => M.Read2Byte("mhfo-hd.dll+DC6BB42");
        public override int ArmorWaistDeco1ID() => M.Read2Byte("mhfo-hd.dll+DC6BB46");
        public override int ArmorWaistDeco2ID() => M.Read2Byte("mhfo-hd.dll+DC6BB48");
        public override int ArmorWaistDeco3ID() => M.Read2Byte("mhfo-hd.dll+DC6BB4A");
        public override int ArmorLegsID() => M.Read2Byte("mhfo-hd.dll+DC6BAF2");
        public override int ArmorLegsDeco1ID() => M.Read2Byte("mhfo-hd.dll+DC6BAF6");
        public override int ArmorLegsDeco2ID() => M.Read2Byte("mhfo-hd.dll+DC6BAF8");
        public override int ArmorLegsDeco3ID() => M.Read2Byte("mhfo-hd.dll+DC6BAFA");
        public override int Cuff1ID() => M.Read2Byte("mhfo-hd.dll+DC6C482");
        public override int Cuff2ID() => M.Read2Byte("mhfo-hd.dll+DC6C484");
        public override int TotalDefense() => M.Read2Byte("mhfo-hd.dll+DC6BEF8");
        public override int PouchItem1ID() => M.Read2Byte("mhfo-hd.dll+DC6C168");
        public override int PouchItem1Qty() => M.Read2Byte("mhfo-hd.dll+DC6C16A");
        public override int PouchItem2ID() => M.Read2Byte("mhfo-hd.dll+DC6C170");
        public override int PouchItem2Qty() => M.Read2Byte("mhfo-hd.dll+DC6C172");
        public override int PouchItem3ID() => M.Read2Byte("mhfo-hd.dll+DC6C178");
        public override int PouchItem3Qty() => M.Read2Byte("mhfo-hd.dll+DC6C17A");
        public override int PouchItem4ID() => M.Read2Byte("mhfo-hd.dll+DC6C180");
        public override int PouchItem4Qty() => M.Read2Byte("mhfo-hd.dll+DC6C182");
        public override int PouchItem5ID() => M.Read2Byte("mhfo-hd.dll+DC6C188");
        public override int PouchItem5Qty() => M.Read2Byte("mhfo-hd.dll+DC6C18A");
        public override int PouchItem6ID() => M.Read2Byte("mhfo-hd.dll+DC6C190");
        public override int PouchItem6Qty() => M.Read2Byte("mhfo-hd.dll+DC6C192");
        public override int PouchItem7ID() => M.Read2Byte("mhfo-hd.dll+DC6C198");
        public override int PouchItem7Qty() => M.Read2Byte("mhfo-hd.dll+DC6C19A");
        public override int PouchItem8ID() => M.Read2Byte("mhfo-hd.dll+DC6C1A0");
        public override int PouchItem8Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1A2");
        public override int PouchItem9ID() => M.Read2Byte("mhfo-hd.dll+DC6C1A8");
        public override int PouchItem9Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1AA");
        public override int PouchItem10ID() => M.Read2Byte("mhfo-hd.dll+DC6C1B0");
        public override int PouchItem10Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1B2");
        public override int PouchItem11ID() => M.Read2Byte("mhfo-hd.dll+DC6C1B8");
        public override int PouchItem11Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1BA");
        public override int PouchItem12ID() => M.Read2Byte("mhfo-hd.dll+DC6C1C0");
        public override int PouchItem12Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1C2");
        public override int PouchItem13ID() => M.Read2Byte("mhfo-hd.dll+DC6C1C8");
        public override int PouchItem13Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1CA");
        public override int PouchItem14ID() => M.Read2Byte("mhfo-hd.dll+DC6C1D0");
        public override int PouchItem14Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1D2");
        public override int PouchItem15ID() => M.Read2Byte("mhfo-hd.dll+DC6C1D8");
        public override int PouchItem15Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1DA");
        public override int PouchItem16ID() => M.Read2Byte("mhfo-hd.dll+DC6C1E0");
        public override int PouchItem16Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1E2");
        public override int PouchItem17ID() => M.Read2Byte("mhfo-hd.dll+DC6C1E8");
        public override int PouchItem17Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1EA");
        public override int PouchItem18ID() => M.Read2Byte("mhfo-hd.dll+DC6C1F0");
        public override int PouchItem18Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1F2");
        public override int PouchItem19ID() => M.Read2Byte("mhfo-hd.dll+DC6C1F8");
        public override int PouchItem19Qty() => M.Read2Byte("mhfo-hd.dll+DC6C1FA");
        public override int PouchItem20ID() => M.Read2Byte("mhfo-hd.dll+DC6C200");
        public override int PouchItem20Qty() => M.Read2Byte("mhfo-hd.dll+DC6C202");
        public override int AmmoPouchItem1ID() => M.Read2Byte("mhfo-hd.dll+DC6C208");
        public override int AmmoPouchItem1Qty() => M.Read2Byte("mhfo-hd.dll+DC6C20A");
        public override int AmmoPouchItem2ID() => M.Read2Byte("mhfo-hd.dll+DC6C210");
        public override int AmmoPouchItem2Qty() => M.Read2Byte("mhfo-hd.dll+DC6C212");
        public override int AmmoPouchItem3ID() => M.Read2Byte("mhfo-hd.dll+DC6C218");
        public override int AmmoPouchItem3Qty() => M.Read2Byte("mhfo-hd.dll+DC6C21A");
        public override int AmmoPouchItem4ID() => M.Read2Byte("mhfo-hd.dll+DC6C220");
        public override int AmmoPouchItem4Qty() => M.Read2Byte("mhfo-hd.dll+DC6C222");
        public override int AmmoPouchItem5ID() => M.Read2Byte("mhfo-hd.dll+DC6C228");
        public override int AmmoPouchItem5Qty() => M.Read2Byte("mhfo-hd.dll+DC6C22A");
        public override int AmmoPouchItem6ID() => M.Read2Byte("mhfo-hd.dll+DC6C230");
        public override int AmmoPouchItem6Qty() => M.Read2Byte("mhfo-hd.dll+DC6C232");
        public override int AmmoPouchItem7ID() => M.Read2Byte("mhfo-hd.dll+DC6C238");
        public override int AmmoPouchItem7Qty() => M.Read2Byte("mhfo-hd.dll+DC6C23A");
        public override int AmmoPouchItem8ID() => M.Read2Byte("mhfo-hd.dll+DC6C240");
        public override int AmmoPouchItem8Qty() => M.Read2Byte("mhfo-hd.dll+DC6C242");
        public override int AmmoPouchItem9ID() => M.Read2Byte("mhfo-hd.dll+DC6C248");
        public override int AmmoPouchItem9Qty() => M.Read2Byte("mhfo-hd.dll+DC6C24A");
        public override int AmmoPouchItem10ID() => M.Read2Byte("mhfo-hd.dll+DC6C250");
        public override int AmmoPouchItem10Qty() => M.Read2Byte("mhfo-hd.dll+DC6C252");

        //slots
        public override int ArmorSkill1() => M.Read2Byte("mhfo-hd.dll+DC6C31C");
        public override int ArmorSkill2() => M.Read2Byte("mhfo-hd.dll+DC6C31E");
        public override int ArmorSkill3() => M.Read2Byte("mhfo-hd.dll+DC6C320");
        public override int ArmorSkill4() => M.Read2Byte("mhfo-hd.dll+DC6C322");
        public override int ArmorSkill5() => M.Read2Byte("mhfo-hd.dll+DC6C324");
        public override int ArmorSkill6() => M.Read2Byte("mhfo-hd.dll+DC6C326");
        public override int ArmorSkill7() => M.Read2Byte("mhfo-hd.dll+DC6C328");
        public override int ArmorSkill8() => M.Read2Byte("mhfo-hd.dll+DC6C32A");
        public override int ArmorSkill9() => M.Read2Byte("mhfo-hd.dll+DC6C32C");
        public override int ArmorSkill10() => M.Read2Byte("mhfo-hd.dll+DC6C32E");
        public override int ArmorSkill11() => M.Read2Byte("mhfo-hd.dll+DC6C330");
        public override int ArmorSkill12() => M.Read2Byte("mhfo-hd.dll+DC6C332");
        public override int ArmorSkill13() => M.Read2Byte("mhfo-hd.dll+DC6C334");
        public override int ArmorSkill14() => M.Read2Byte("mhfo-hd.dll+DC6C336");
        public override int ArmorSkill15() => M.Read2Byte("mhfo-hd.dll+DC6C338");
        public override int ArmorSkill16() => M.Read2Byte("mhfo-hd.dll+DC6C33A");
        public override int ArmorSkill17() => M.Read2Byte("mhfo-hd.dll+DC6C33C");
        public override int ArmorSkill18() => M.Read2Byte("mhfo-hd.dll+DC6C33E");
        public override int ArmorSkill19() => M.Read2Byte("mhfo-hd.dll+DC6C340");

        public override int BloatedWeaponAttack() => M.Read2Byte("mhfo-hd.dll+E7FE4F0");

        public override int ZenithSkill1() => M.Read2Byte("mhfo-hd.dll+DCD1DC8");
        public override int ZenithSkill2() => M.Read2Byte("mhfo-hd.dll+DCD1DCA");
        public override int ZenithSkill3() => M.Read2Byte("mhfo-hd.dll+DCD1DCC");
        public override int ZenithSkill4() => M.Read2Byte("mhfo-hd.dll+DCD1DCE");
        public override int ZenithSkill5() => M.Read2Byte("mhfo-hd.dll+DCD1DD0");
        public override int ZenithSkill6() => M.Read2Byte("mhfo-hd.dll+DCD1DD2");
        public override int ZenithSkill7() => M.Read2Byte("mhfo-hd.dll+DCD1DD4");

        public override int AutomaticSkillWeapon() => M.Read2Byte("mhfo-hd.dll+DC6C352");
        public override int AutomaticSkillHead() => M.Read2Byte("mhfo-hd.dll+DC6C34A");
        public override int AutomaticSkillChest() => M.Read2Byte("mhfo-hd.dll+DC6C34C");
        public override int AutomaticSkillArms() => M.Read2Byte("mhfo-hd.dll+DC6C34E");
        public override int AutomaticSkillWaist() => M.Read2Byte("mhfo-hd.dll+DC6C350");
        public override int AutomaticSkillLegs() => M.Read2Byte("mhfo-hd.dll+DC6C346");

        public override int StyleRank1() => M.ReadByte("mhfo-hd.dll+DC6C493");
        public override int StyleRank2() => M.ReadByte("mhfo-hd.dll+DC6C55F");

        public override int GRWeaponLv() => M.ReadByte("mhfo-hd.dll+DC6BB54");
        public override int GRWeaponLvBowguns() => M.ReadByte("mhfo-hd.dll+DC6BB55");

        public override int Sigil1Name1() => M.Read2Byte("mhfo-hd.dll+E830E14");
        public override int Sigil1Value1() => M.Read2Byte("mhfo-hd.dll+E830E1A");
        public override int Sigil1Name2() => M.Read2Byte("mhfo-hd.dll+E830E16");
        public override int Sigil1Value2() => M.Read2Byte("mhfo-hd.dll+E830E1C");
        public override int Sigil1Name3() => M.Read2Byte("mhfo-hd.dll+E830E18");
        public override int Sigil1Value3() => M.Read2Byte("mhfo-hd.dll+E830E1E");
        public override int Sigil2Name1() => M.Read2Byte("mhfo-hd.dll+E830E20");
        public override int Sigil2Value1() => M.Read2Byte("mhfo-hd.dll+E830E26");
        public override int Sigil2Name2() => M.Read2Byte("mhfo-hd.dll+E830E22");
        public override int Sigil2Value2() => M.Read2Byte("mhfo-hd.dll+E830E28");
        public override int Sigil2Name3() => M.Read2Byte("mhfo-hd.dll+E830E24");
        public override int Sigil2Value3() => M.Read2Byte("mhfo-hd.dll+E830E2A");
        public override int Sigil3Name1() => M.Read2Byte("mhfo-hd.dll+E831234");
        public override int Sigil3Value1() => M.Read2Byte("mhfo-hd.dll+E83123A");
        public override int Sigil3Name2() => M.Read2Byte("mhfo-hd.dll+E831236");
        public override int Sigil3Value2() => M.Read2Byte("mhfo-hd.dll+E83123C");
        public override int Sigil3Name3() => M.Read2Byte("mhfo-hd.dll+E831238");
        public override int Sigil3Value3() => M.Read2Byte("mhfo-hd.dll+E83123E");

        public override int RathianHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1AE");
        public override int FatalisHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1B0");
        public override int KelbiHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1B2");
        public override int MosswineHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1B4");
        public override int BullfangoHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1B6");

        public override int YianKutKuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1B8");
        public override int LaoShanLungHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1BA");
        public override int CephadromeHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1BC");
        public override int FelyneHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1BE");
        public override int RathalosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1C2");
        public override int AptonothHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1C4");
        public override int GenpreyHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1C6");
        public override int DiablosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1C8");
        public override int KhezuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1CA");
        public override int VelocipreyHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1CC");
        public override int GraviosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1CE");
        public override int VespoidHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1D2");
        public override int GypcerosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1D4");
        public override int PlesiothHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1D6");
        public override int BasariosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1D8");
        public override int MelynxHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1DA");
        public override int HornetaurHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1DC");
        public override int ApcerosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1DE");
        public override int MonoblosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1E0");
        public override int VelocidromeHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1E2");
        public override int GendromeHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1E4");
        public override int RocksHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1E6");
        public override int IopreyHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1E8");
        public override int IodromeHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1EA");
        public override int KirinHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1EE");
        public override int CephalosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1F0");

        public override int GiapreyHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1F2");
        public override int CrimsonFatalisHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1F4");
        public override int PinkRathianHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1F6");
        public override int BlueYianKutKuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1F8");
        public override int PurpleGypcerosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1FA");
        public override int YianGarugaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1FC");
        public override int SilverRathalosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E1FE");
        public override int GoldRathianHunted() => M.Read2Byte("mhfo-hd.dll+ED3E200");
        public override int BlackDiablosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E202");
        public override int WhiteMonoblosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E204");
        public override int RedKhezuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E206");
        public override int GreenPlesiothHunted() => M.Read2Byte("mhfo-hd.dll+ED3E208");
        public override int BlackGraviosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E20A");
        public override int DaimyoHermitaurHunted() => M.Read2Byte("mhfo-hd.dll+ED3E20C");
        public override int AzureRathalosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E20E");
        public override int AshenLaoShanLungHunted() => M.Read2Byte("mhfo-hd.dll+ED3E210");
        public override int BlangongaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E212");
        public override int CongalalaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E214");
        public override int RajangHunted() => M.Read2Byte("mhfo-hd.dll+ED3E216");

        public override int KushalaDaoraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E218");
        public override int ShenGaorenHunted() => M.Read2Byte("mhfo-hd.dll+ED3E21A");
        public override int GreatThunderbugHunted() => M.Read2Byte("mhfo-hd.dll+ED3E21C");

        public override int ShakalakaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E21E");
        public override int YamaTsukamiHunted() => M.Read2Byte("mhfo-hd.dll+ED3E220");
        public override int ChameleosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E222");
        public override int RustedKushalaDaoraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E224");
        public override int BlangoHunted() => M.Read2Byte("mhfo-hd.dll+ED3E226");
        public override int CongaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E228");
        public override int RemobraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E22A");
        public override int LunastraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E22C");
        public override int TeostraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E22E");
        public override int HermitaurHunted() => M.Read2Byte("mhfo-hd.dll+ED3E230");
        public override int ShogunCeanataurHunted() => M.Read2Byte("mhfo-hd.dll+ED3E232");
        public override int BulldromeHunted() => M.Read2Byte("mhfo-hd.dll+ED3E234");
        public override int AntekaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E236");
        public override int PopoHunted() => M.Read2Byte("mhfo-hd.dll+ED3E238");
        public override int WhiteFatalisHunted() => M.Read2Byte("mhfo-hd.dll+ED3E23A");

        public override int CeanataurHunted() => M.Read2Byte("mhfo-hd.dll+ED3E23E");
        public override int HypnocHunted() => M.Read2Byte("mhfo-hd.dll+ED3E240");
        public override int VolganosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E242");
        public override int TigrexHunted() => M.Read2Byte("mhfo-hd.dll+ED3E244");
        public override int AkantorHunted() => M.Read2Byte("mhfo-hd.dll+ED3E246");
        public override int BrightHypnocHunted() => M.Read2Byte("mhfo-hd.dll+ED3E248");
        public override int RedVolganosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E24A");
        public override int EspinasHunted() => M.Read2Byte("mhfo-hd.dll+ED3E24C");
        public override int OrangeEspinasHunted() => M.Read2Byte("mhfo-hd.dll+ED3E24E");
        public override int SilverHypnocHunted() => M.Read2Byte("mhfo-hd.dll+ED3E250");
        public override int AkuraVashimuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E252");
        public override int AkuraJebiaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E254");

        public override int BerukyurosuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E256");
        public override int CactusHunted() => M.Read2Byte("mhfo-hd.dll+ED3E258");
        public override int GorgeObjectsHunted() => M.Read2Byte("mhfo-hd.dll+ED3E25A");
        public override int PariapuriaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E25E");
        public override int WhiteEspinasHunted() => M.Read2Byte("mhfo-hd.dll+ED3E260");
        public override int KamuOrugaronHunted() => M.Read2Byte("mhfo-hd.dll+ED3E262");
        public override int NonoOrugaronHunted() => M.Read2Byte("mhfo-hd.dll+ED3E264");
        public override int DyuragauaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E268");
        public override int DoragyurosuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E26A");
        public override int GurenzeburuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E26C");
        public override int BurukkuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E26E");
        public override int ErupeHunted() => M.Read2Byte("mhfo-hd.dll+ED3E270");
        public override int RukodioraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E272");
        public override int UnknownHunted() => M.Read2Byte("mhfo-hd.dll+ED3E274");
        public override int GogomoaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E276");
        public override int TaikunZamuzaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E27A");
        public override int AbioruguHunted() => M.Read2Byte("mhfo-hd.dll+ED3E27C");
        public override int KuarusepusuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E27E");
        public override int OdibatorasuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E280");
        public override int DisufiroaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E282");
        public override int RebidioraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E284");
        public override int AnorupatisuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E286");
        public override int HyujikikiHunted() => M.Read2Byte("mhfo-hd.dll+ED3E288");
        public override int MidogaronHunted() => M.Read2Byte("mhfo-hd.dll+ED3E28A");
        public override int GiaoruguHunted() => M.Read2Byte("mhfo-hd.dll+ED3E28C");
        public override int MiRuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E28E");
        public override int FarunokkuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E290");
        public override int PokaradonHunted() => M.Read2Byte("mhfo-hd.dll+ED3E292");
        public override int ShantienHunted() => M.Read2Byte("mhfo-hd.dll+ED3E294");
        public override int PokaraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E296");
        public override int GoruganosuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E29A");
        public override int AruganosuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E29C");
        public override int BaruragaruHunted() => M.Read2Byte("mhfo-hd.dll+ED3E29E");
        public override int ZerureusuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2A0");
        public override int GougarfHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2A2");
        public override int UrukiHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2A4");
        public override int ForokururuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2A6");
        public override int MeraginasuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2A8");
        public override int DiorexHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2AA");
        public override int GarubaDaoraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2AC");
        public override int InagamiHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2AE");
        public override int VarusaburosuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2B0");
        public override int PoborubarumuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2B2");

        public override int GureadomosuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2C2");
        public override int HarudomeruguHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2C4");
        public override int ToridclessHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2C6");
        public override int GasurabazuraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2C8");
        public override int KusubamiHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2CA");
        public override int YamaKuraiHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2CC");
        public override int ZinogreHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2D0");
        public override int DeviljhoHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2D2");
        public override int BrachydiosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2D4");
        public override int ToaTesukatoraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2D8");
        public override int BariothHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2DA");
        public override int UragaanHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2DC");
        public override int StygianZinogreHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2DE");
        public override int GuanzorumuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2E0");
        public override int StarvingDeviljhoHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2E2");
        public override int VoljangHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2E8");
        public override int NargacugaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2EA");
        public override int KeoaruboruHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2EC");
        public override int ZenaserisuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2EE");
        public override int GoreMagalaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2F0");
        public override int BlinkingNargacugaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2F2");
        public override int ShagaruMagalaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2F4");
        public override int AmatsuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2F6");
        public override int ElzelionHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2F8");
        public override int ArrogantDuremudiraHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2FA");
        public override int SeregiosHunted() => M.Read2Byte("mhfo-hd.dll+ED3E2FE");
        public override int BogabadorumuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E300");
        public override int BlitzkriegBogabadorumuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E304");
        public override int SparklingZerureusuHunted() => M.Read2Byte("mhfo-hd.dll+ED3E308");
        public override int PSO2RappyHunted() => M.Read2Byte("mhfo-hd.dll+ED3E30A");
        public override int KingShakalakaHunted() => M.Read2Byte("mhfo-hd.dll+ED3E30C");//




        public override int QuestState() => M.ReadByte("mhfo-hd.dll+ED52892");

        public override int RoadDureSkill1Name() => M.ReadByte("mhfo-hd.dll+ED3E7DC");
        public override int RoadDureSkill1Level() => M.ReadByte("mhfo-hd.dll+ED3E7DE");
        public override int RoadDureSkill2Name() => M.ReadByte("mhfo-hd.dll+ED3E7E0");
        public override int RoadDureSkill2Level() => M.ReadByte("mhfo-hd.dll+ED3E7E2");
        public override int RoadDureSkill3Name() => M.ReadByte("mhfo-hd.dll+ED3E7E4");
        public override int RoadDureSkill3Level() => M.ReadByte("mhfo-hd.dll+ED3E7E6");
        public override int RoadDureSkill4Name() => M.ReadByte("mhfo-hd.dll+ED3E7E8");
        public override int RoadDureSkill4Level() => M.ReadByte("mhfo-hd.dll+ED3E7EA");
        public override int RoadDureSkill5Name() => M.ReadByte("mhfo-hd.dll+ED3E7EC");
        public override int RoadDureSkill5Level() => M.ReadByte("mhfo-hd.dll+ED3E7EE");
        public override int RoadDureSkill6Name() => M.ReadByte("mhfo-hd.dll+ED3E7F0");
        public override int RoadDureSkill6Level() => M.ReadByte("mhfo-hd.dll+ED3E7F2");
        public override int RoadDureSkill7Name() => M.ReadByte("mhfo-hd.dll+ED3E7F4");
        public override int RoadDureSkill7Level() => M.ReadByte("mhfo-hd.dll+ED3E7F6");
        public override int RoadDureSkill8Name() => M.ReadByte("mhfo-hd.dll+ED3E7F8");
        public override int RoadDureSkill8Level() => M.ReadByte("mhfo-hd.dll+ED3E7FA");
        public override int RoadDureSkill9Name() => M.ReadByte("mhfo-hd.dll+ED3E7FC");
        public override int RoadDureSkill9Level() => M.ReadByte("mhfo-hd.dll+ED3E7FE");
        public override int RoadDureSkill10Name() => M.ReadByte("mhfo-hd.dll+ED3E800");
        public override int RoadDureSkill10Level() => M.ReadByte("mhfo-hd.dll+ED3E802");
        public override int RoadDureSkill11Name() => M.ReadByte("mhfo-hd.dll+ED3E804");
        public override int RoadDureSkill11Level() => M.ReadByte("mhfo-hd.dll+ED3E806");
        public override int RoadDureSkill12Name() => M.ReadByte("mhfo-hd.dll+ED3E808");
        public override int RoadDureSkill12Level() => M.ReadByte("mhfo-hd.dll+ED3E80A");
        public override int RoadDureSkill13Name() => M.ReadByte("mhfo-hd.dll+ED3E80C");
        public override int RoadDureSkill13Level() => M.ReadByte("mhfo-hd.dll+ED3E80E");
        public override int RoadDureSkill14Name() => M.ReadByte("mhfo-hd.dll+ED3E810");
        public override int RoadDureSkill14Level() => M.ReadByte("mhfo-hd.dll+ED3E812");
        public override int RoadDureSkill15Name() => M.ReadByte("mhfo-hd.dll+ED3E814");
        public override int RoadDureSkill15Level() => M.ReadByte("mhfo-hd.dll+ED3E816");
        public override int RoadDureSkill16Name() => M.ReadByte("mhfo-hd.dll+ED3E818");
        public override int RoadDureSkill16Level() => M.ReadByte("mhfo-hd.dll+ED3E81A");

        public override int PartySize() => M.ReadByte("mhfo-hd.dll+E3CE388");
        public override int PartySizeMax() => M.ReadByte("mhfo-hd.dll+EDF0828");

        public override uint GSRP() => 1;
        public override uint GRP() => 1;

        public override int HunterHP() => M.Read2Byte("mhfo-hd.dll+E7FE178");
        public override int HunterStamina() => M.Read2Byte("mhfo-hd.dll+DC6BF4C");

        //farcaster doesnt count as used
        public override int QuestItemsUsed() => M.Read2Byte("mhfo-hd.dll+E4229E4");
        public override int AreaHitsTakenBlocked() => M.Read2Byte("mhfo-hd.dll+DC6BC38");

        public override int PartnyaBagItem1ID() => M.Read2Byte("mhfo-hd.dll+E37D348");
        public override int PartnyaBagItem1Qty() => M.Read2Byte("mhfo-hd.dll+E37D34A");

        public override int PartnyaBagItem2ID() => M.Read2Byte("mhfo-hd.dll+E37D34C");

        public override int PartnyaBagItem2Qty() => M.Read2Byte("mhfo-hd.dll+E37D34E");

        public override int PartnyaBagItem3ID() => M.Read2Byte("mhfo-hd.dll+E37D350");

        public override int PartnyaBagItem3Qty() => M.Read2Byte("mhfo-hd.dll+E37D352");

        public override int PartnyaBagItem4ID() => M.Read2Byte("mhfo-hd.dll+E37D354");

        public override int PartnyaBagItem4Qty() => M.Read2Byte("mhfo-hd.dll+E37D356");

        public override int PartnyaBagItem5ID() => M.Read2Byte("mhfo-hd.dll+E37D358");

        public override int PartnyaBagItem5Qty() => M.Read2Byte("mhfo-hd.dll+E37D35A");

        public override int PartnyaBagItem6ID() => M.Read2Byte("mhfo-hd.dll+E37D35C");

        public override int PartnyaBagItem6Qty() => M.Read2Byte("mhfo-hd.dll+E37D35E");

        public override int PartnyaBagItem7ID() => M.Read2Byte("mhfo-hd.dll+E37D360");

        public override int PartnyaBagItem7Qty() => M.Read2Byte("mhfo-hd.dll+E37D362");

        public override int PartnyaBagItem8ID() => M.Read2Byte("mhfo-hd.dll+E37D364");

        public override int PartnyaBagItem8Qty() => M.Read2Byte("mhfo-hd.dll+E37D366");

        public override int PartnyaBagItem9ID() => M.Read2Byte("mhfo-hd.dll+E37D368");

        public override int PartnyaBagItem9Qty() => M.Read2Byte("mhfo-hd.dll+E37D36A");

        public override int PartnyaBagItem10ID() => M.Read2Byte("mhfo-hd.dll+E37D36C");

        public override int PartnyaBagItem10Qty() => M.Read2Byte("mhfo-hd.dll+E37D36E");


    }
}
