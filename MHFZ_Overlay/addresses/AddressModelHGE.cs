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
        public string Monster1BP2() => M.Read2Byte("mhfo-hd.dll+0E37DD38,350").ToString();
        public string Monster1BP3() => M.Read2Byte("mhfo-hd.dll+0E37DD38,358").ToString();
        public string Monster1BP4() => M.Read2Byte("mhfo-hd.dll+0E37DD38,360").ToString();
        public string Monster1BP5() => M.Read2Byte("mhfo-hd.dll+0E37DD38,368").ToString();
        public string Monster1BP6() => M.Read2Byte("mhfo-hd.dll+0E37DD38,370").ToString();
        public string Monster1BP7() => M.Read2Byte("mhfo-hd.dll+0E37DD38,378").ToString();
        public string Monster1BP8() => M.Read2Byte("mhfo-hd.dll+0E37DD38,380").ToString();
        public string Monster1BP9() => M.Read2Byte("mhfo-hd.dll+0E37DD38,388").ToString();
        public string Monster1BP10() => M.Read2Byte("mhfo-hd.dll+0E37DD38,390").ToString();
        public string Monster2BP1() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1238").ToString();
        public string Monster2BP2() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1240").ToString();
        public string Monster2BP3() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1248").ToString();
        public string Monster2BP4() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1250").ToString();
        public string Monster2BP5() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1258").ToString();
        public string Monster2BP6() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1260").ToString();
        public string Monster2BP7() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1268").ToString();
        public string Monster2BP8() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1270").ToString();
        public string Monster2BP9() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1278").ToString();
        public string Monster2BP10() => M.Read2Byte("mhfo-hd.dll+0E37DD38,1280").ToString();
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
        public override string Monster1DefMult() { return GetNotRoad() ? M.ReadFloat("mhfo-hd.dll+0E37DD38,89C").ToString() : Monster1RoadDefMult(); }
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
        public override string Monster2DefMult() { return GetNotRoad() ? M.ReadFloat("mhfo-hd.dll+0E37DD38,178C").ToString() : Monster2RoadDefMult(); }
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
        public string Monster1RoadDefMult() => M.ReadFloat("mhfo-hd.dll+E37DF18,89C").ToString();
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
        public string Monster2RoadDefMult() => M.ReadFloat("mhfo-hd.dll+E37DF18,178C").ToString();
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
    }
}
