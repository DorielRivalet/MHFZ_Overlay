﻿using Memory;
using System;
using System.Windows.Media.Animation;

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
        //public override int WeaponType() => M.ReadByte("mhfo-hd.dll+ED3A466");
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




        //TODO
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
        //public override int RoadFatalisEncounters() => M.Read2Byte("mhfo-hd.dll+ECD71C4");
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

        //public override int PartnyaRank() => M.Read2Byte("mhfo-hd.dll+E8DF010");
        public override int PartnyaRankPoints() => M.ReadInt("mhfo-hd.dll+E551114");

        public override int Objective1ID() => M.Read2Byte("mhfo-hd.dll+2AFA834");
        public override int Objective1Quantity() => M.Read2Byte("mhfo-hd.dll+2AFA836");
        public override int Objective1CurrentQuantityMonster() => M.Read2Byte("mhfo-hd.dll+ECB2A38");
        public override int Objective1CurrentQuantityItem() => M.Read2Byte("mhfo-hd.dll+DC6C2F2");

        public override int RavienteTriggeredEvent() => 1;
        public override int RavienteAreaID() => 1;
        public override int GreatSlayingPoints() => 1;
        public override int GreatSlayingPointsSaved() => 1;

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
        public override int ArmorArmsID() => 1;
        public override int ArmorArmsDeco1ID() => 1;
        public override int ArmorArmsDeco2ID() => 1;
        public override int ArmorArmsDeco3ID() => 1;
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
        public override int TotalDefense() => 1;
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
        public override int AmmoPouchItem1Qty() => 1;
        public override int AmmoPouchItem2ID() => 1;
        public override int AmmoPouchItem2Qty() => 1;
        public override int AmmoPouchItem3ID() => 1;
        public override int AmmoPouchItem3Qty() => 1;
        public override int AmmoPouchItem4ID() => 1;
        public override int AmmoPouchItem4Qty() => 1;
        public override int AmmoPouchItem5ID() => 1;
        public override int AmmoPouchItem5Qty() => 1;
        public override int AmmoPouchItem6ID() => 1;
        public override int AmmoPouchItem6Qty() => 1;
        public override int AmmoPouchItem7ID() => 1;
        public override int AmmoPouchItem7Qty() => 1;
        public override int AmmoPouchItem8ID() => 1;
        public override int AmmoPouchItem8Qty() => 1;
        public override int AmmoPouchItem9ID() => 1;
        public override int AmmoPouchItem9Qty() => 1;
        public override int AmmoPouchItem10ID() => 1;
        public override int AmmoPouchItem10Qty() => 1;

        //slots
        public override int ArmorSkill1() => 1;
        public override int ArmorSkill2() => 1;
        public override int ArmorSkill3() => 1;
        public override int ArmorSkill4() => 1;
        public override int ArmorSkill5() => 1;
        public override int ArmorSkill6() => 1;
        public override int ArmorSkill7() => 1;
        public override int ArmorSkill8() => 1;
        public override int ArmorSkill9() => 1;
        public override int ArmorSkill10() => 1;
        public override int ArmorSkill11() => 1;
        public override int ArmorSkill12() => 1;
        public override int ArmorSkill13() => 1;
        public override int ArmorSkill14() => 1;
        public override int ArmorSkill15() => 1;
        public override int ArmorSkill16() => 1;
        public override int ArmorSkill17() => 1;
        public override int ArmorSkill18() => 1;
        public override int ArmorSkill19() => 1;

        public override int BloatedWeaponAttack() => 1;

        public override int ZenithSkill1() => 1;
        public override int ZenithSkill2() => 1;
        public override int ZenithSkill3() => 1;
        public override int ZenithSkill4() => 1;
        public override int ZenithSkill5() => 1;
        public override int ZenithSkill6() => 1;
        public override int ZenithSkill7() => 1;

        public override int AutomaticSkillWeapon() => 1;
        public override int AutomaticSkillHead() => 1;
        public override int AutomaticSkillChest() => 1;
        public override int AutomaticSkillArms() => 1;
        public override int AutomaticSkillWaist() => 1;
        public override int AutomaticSkillLegs() => 1;

        public override int StyleRank1() => M.ReadByte("mhfo-hd.dll+DC6C493");
        public override int StyleRank2() => M.ReadByte("mhfo-hd.dll+DC6C55F");

        public override int GRWeaponLv() => 1;

        public override int Sigil1Name1() => 1;
        public override int Sigil1Value1() => 1;
        public override int Sigil1Name2() => 1;
        public override int Sigil1Value2() => 1;
        public override int Sigil1Name3() => 1;
        public override int Sigil1Value3() => 1;
        public override int Sigil2Name1() => 1;
        public override int Sigil2Value1() => 1;
        public override int Sigil2Name2() => 1;
        public override int Sigil2Value2() => 1;
        public override int Sigil2Name3() => 1;
        public override int Sigil2Value3() => 1;
        public override int Sigil3Name1() => 1;
        public override int Sigil3Value1() => 1;
        public override int Sigil3Name2() => 1;
        public override int Sigil3Value2() => 1;
        public override int Sigil3Name3() => 1;
        public override int Sigil3Value3() => 1;

        public override int FelyneHunted() => 1;
        public override int MelynxHunted() => 1;
        public override int ShakalakaHunted() => 1;
        public override int VespoidHunted() => 1;
        public override int HornetaurHunted() => 1;
        public override int GreatThunderbugHunted() => 1;
        public override int KelbiHunted() => 1;
        public override int MosswineHunted() => 1;
        public override int AntekaHunted() => 1;
        public override int PopoHunted() => 1;
        public override int AptonothHunted() => 1;
        public override int ApcerosHunted() => 1;
        public override int BurukkuHunted() => 1;
        public override int ErupeHunted() => 1;
        public override int VelocipreyHunted() => 1;
        public override int VelocidromeHunted() => 1;
        public override int GenpreyHunted() => 1;
        public override int GendromeHunted() => 1;
        public override int IopreyHunted() => 1;
        public override int IodromeHunted() => 1;
        public override int GiapreyHunted() => 1;
        public override int YianKutKuHunted() => 1;
        public override int BlueYianKutKuHunted() => 1;
        public override int YianGarugaHunted() => 1;
        public override int GypcerosHunted() => 1;
        public override int PurpleGypcerosHunted() => 1;
        public override int HypnocHunted() => 1;
        public override int BrightHypnocHunted() => 1;
        public override int SilverHypnocHunted() => 1;
        public override int FarunokkuHunted() => 1;
        public override int ForokururuHunted() => 1;
        public override int ToridclessHunted() => 1;
        public override int RemobraHunted() => 1;
        public override int RathianHunted() => 1;
        public override int PinkRathianHunted() => 1;
        public override int GoldRathianHunted() => 1;
        public override int RathalosHunted() => 1;
        public override int AzureRathalosHunted() => 1;
        public override int SilverRathalosHunted() => 1;
        public override int KhezuHunted() => 1;
        public override int RedKhezuHunted() => 1;
        public override int BasariosHunted() => 1;
        public override int GraviosHunted() => 1;
        public override int BlackGraviosHunted() => 1;
        public override int MonoblosHunted() => 1;
        public override int WhiteMonoblosHunted() => 1;
        public override int DiablosHunted() => 1;
        public override int BlackDiablosHunted() => 1;
        public override int TigrexHunted() => 1;
        public override int EspinasHunted() => 1;
        public override int OrangeEspinasHunted() => 1;
        public override int WhiteEspinasHunted() => 1;
        public override int AkantorHunted() => 1;
        public override int BerukyurosuHunted() => 1;
        public override int DoragyurosuHunted() => 1;
        public override int PariapuriaHunted() => 1;
        public override int DyuragauaHunted() => 1;
        public override int GurenzeburuHunted() => 1;
        public override int OdibatorasuHunted() => 1;
        public override int HyujikikiHunted() => 1;
        public override int AnorupatisuHunted() => 1;
        public override int ZerureusuHunted() => 1;
        public override int MeraginasuHunted() => 1;
        public override int DiorexHunted() => 1;
        public override int PoborubarumuHunted() => 1;
        public override int VarusaburosuHunted() => 1;
        public override int GureadomosuHunted() => 1;
        public override int BariothHunted() => 1;
        public override int NargacugaHunted() => 1;
        public override int ZenaserisuHunted() => 1;
        public override int SeregiosHunted() => 1;
        public override int BogabadorumuHunted() => 1;
        public override int CephalosHunted() => 1;
        public override int CephadromeHunted() => 1;
        public override int PlesiothHunted() => 1;
        public override int GreenPlesiothHunted() => 1;
        public override int VolganosHunted() => 1;
        public override int RedVolganosHunted() => 1;
        public override int HermitaurHunted() => 1;
        public override int DaimyoHermitaurHunted() => 1;
        public override int CeanataurHunted() => 1;
        public override int ShogunCeanataurHunted() => 1;
        public override int ShenGaorenHunted() => 1;
        public override int AkuraVashimuHunted() => 1;
        public override int AkuraJebiaHunted() => 1;
        public override int TaikunZamuzaHunted() => 1;
        public override int KusubamiHunted() => 1;
        public override int BullfangoHunted() => 1;
        public override int BulldromeHunted() => 1;
        public override int CongaHunted() => 1;
        public override int CongalalaHunted() => 1;
        public override int BlangoHunted() => 1;
        public override int BlangongaHunted() => 1;
        public override int GogomoaHunted() => 1;
        public override int RajangHunted() => 1;
        public override int KamuOrugaronHunted() => 1;
        public override int NonoOrugaronHunted() => 1;
        public override int MidogaronHunted() => 1;
        public override int GougarfHunted() => 1;
        public override int VoljangHunted() => 1;
        public override int KirinHunted() => 1;
        public override int KushalaDaoraHunted() => 1;
        public override int RustedKushalaDaoraHunted() => 1;
        public override int ChameleosHunted() => 1;
        public override int LunastraHunted() => 1;
        public override int TeostraHunted() => 1;
        public override int LaoShanLungHunted() => 1;
        public override int AshenLaoShanLungHunted() => 1;
        public override int YamaTsukamiHunted() => 1;
        public override int RukodioraHunted() => 1;
        public override int RebidioraHunted() => 1;
        public override int FatalisHunted() => 1;
        public override int ShantienHunted() => 1;
        public override int DisufiroaHunted() => 1;
        public override int GarubaDaoraHunted() => 1;
        public override int InagamiHunted() => 1;
        public override int HarudomeruguHunted() => 1;
        public override int YamaKuraiHunted() => 1;
        public override int ToaTesukatoraHunted() => 1;
        public override int GuanzorumuHunted() => 1;
        public override int KeoaruboruHunted() => 1;
        public override int ShagaruMagalaHunted() => 1;
        public override int ElzelionHunted() => 1;
        public override int AmatsuHunted() => 1;
        public override int AbioruguHunted() => 1;
        public override int GiaoruguHunted() => 1;
        public override int GasurabazuraHunted() => 1;
        public override int DeviljhoHunted() => 1;
        public override int BrachydiosHunted() => 1;
        public override int UragaanHunted() => 1;
        public override int KuarusepusuHunted() => 1;
        public override int PokaraHunted() => 1;
        public override int PokaradonHunted() => 1;
        public override int BaruragaruHunted() => 1;
        public override int ZinogreHunted() => 1;
        public override int StygianZinogreHunted() => 1;
        public override int GoreMagalaHunted() => 1;

        public override int BlitzkriegBogabadorumuHunted() => 1;
        public override int SparklingZerureusuHunted() => 1;
        public override int StarvingDeviljhoHunted() => 1;

        public override int CrimsonFatalisHunted() => 1;
        public override int WhiteFatalisHunted() => 1;
        public override int CactusHunted() => 1;
        public override int ArrogantDuremudiraHunted() => 1;//untested
        public override int KingShakalakaHunted() => 1;
        public override int MiRuHunted() => 1;
        public override int UnknownHunted() => 1;
        public override int GoruganosuHunted() => 1;
        public override int AruganosuHunted() => 1;
        public override int PSO2RappyHunted() => 1;
        public override int RocksHunted() => 1;
        public override int UrukiHunted() => 1;
        public override int GorgeObjectsHunted() => 1;
        public override int BlinkingNargacugaHunted() => 1;
        public override int QuestState() => 0;

        public override int RoadDureSkill1Name() => 1;
        public override int RoadDureSkill1Level() => 1;
        public override int RoadDureSkill2Name() => 1;
        public override int RoadDureSkill2Level() => 1;
        public override int RoadDureSkill3Name() => 1;
        public override int RoadDureSkill3Level() => 1;
        public override int RoadDureSkill4Name() => 1;
        public override int RoadDureSkill4Level() => 1;
        public override int RoadDureSkill5Name() => 1;
        public override int RoadDureSkill5Level() => 1;
        public override int RoadDureSkill6Name() => 1;
        public override int RoadDureSkill6Level() => 1;
        public override int RoadDureSkill7Name() => 1;
        public override int RoadDureSkill7Level() => 1;
        public override int RoadDureSkill8Name() => 1;
        public override int RoadDureSkill8Level() => 1;
        public override int RoadDureSkill9Name() => 1;
        public override int RoadDureSkill9Level() => 1;
        public override int RoadDureSkill10Name() => 1;
        public override int RoadDureSkill10Level() => 1;
        public override int RoadDureSkill11Name() => 1;
        public override int RoadDureSkill11Level() => 1;
        public override int RoadDureSkill12Name() => 1;
        public override int RoadDureSkill12Level() => 1;
        public override int RoadDureSkill13Name() => 1;
        public override int RoadDureSkill13Level() => 1;
        public override int RoadDureSkill14Name() => 1;
        public override int RoadDureSkill14Level() => 1;
        public override int RoadDureSkill15Name() => 1;
        public override int RoadDureSkill15Level() => 1;
        public override int RoadDureSkill16Name() => 1;
        public override int RoadDureSkill16Level() => 1;

        public override int PartySize() => 1;
        public override int PartySizeMax() => 1;

        public override uint GSRP() => 1;
        public override uint GRP() => 1;

    }
}
