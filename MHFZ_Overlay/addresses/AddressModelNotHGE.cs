using Memory;

namespace MHFZ_Overlay.addresses
{

    /// Most Addresses from https://github.com/suzaku01/
    public class AddressModelNotHGE : AddressModel
    {
        public AddressModelNotHGE(Mem m) : base(m)
        {

        }

        public override int HitCountInt() => M.Read2Byte("mhfo.dll+5CA3430");
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
        public string Monster1BP2 { get => M.Read2Byte("mhfo.dll+60A3E58,350").ToString(); }
        public string Monster1BP3 { get => M.Read2Byte("mhfo.dll+60A3E58,358").ToString(); }
        public string Monster1BP4 { get => M.Read2Byte("mhfo.dll+60A3E58,360").ToString(); }
        public string Monster1BP5 { get => M.Read2Byte("mhfo.dll+60A3E58,368").ToString(); }
        public string Monster1BP6 { get => M.Read2Byte("mhfo.dll+60A3E58,370").ToString(); }
        public string Monster1BP7 { get => M.Read2Byte("mhfo.dll+60A3E58,378").ToString(); }
        public string Monster1BP8 { get => M.Read2Byte("mhfo.dll+60A3E58,380").ToString(); }
        public string Monster1BP9 { get => M.Read2Byte("mhfo.dll+60A3E58,388").ToString(); }
        public string Monster1BP10 { get => M.Read2Byte("mhfo.dll+60A3E58,390").ToString(); }
        public string Monster2BP1 { get => M.Read2Byte("mhfo.dll+60A3E58,1238").ToString(); }
        public string Monster2BP2 { get => M.Read2Byte("mhfo.dll+60A3E58,1240").ToString(); }
        public string Monster2BP3 { get => M.Read2Byte("mhfo.dll+60A3E58,1248").ToString(); }
        public string Monster2BP4 { get => M.Read2Byte("mhfo.dll+60A3E58,1250").ToString(); }
        public string Monster2BP5 { get => M.Read2Byte("mhfo.dll+60A3E58,1258").ToString(); }
        public string Monster2BP6 { get => M.Read2Byte("mhfo.dll+60A3E58,1260").ToString(); }
        public string Monster2BP7 { get => M.Read2Byte("mhfo.dll+60A3E58,1268").ToString(); }
        public string Monster2BP8 { get => M.Read2Byte("mhfo.dll+60A3E58,1270").ToString(); }
        public string Monster2BP9 { get => M.Read2Byte("mhfo.dll+60A3E58,1278").ToString(); }
        public string Monster2BP10 { get => M.Read2Byte("mhfo.dll+60A3E58,1280").ToString(); }
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
    }
}
