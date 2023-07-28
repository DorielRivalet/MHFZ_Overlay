// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

// Most Addresses from https://github.com/suzaku01/
namespace MHFZ_Overlay.Models.Addresses;

using System.Globalization;
using Memory;
using MHFZ_Overlay.ViewModels.Windows;

/// <summary>
/// Inherits from AddressModel and provides the memory address of the hit count value (etc.) when the game is running in HGE mode.
/// </summary>
public sealed class AddressModelHGE : AddressModel
{
    public AddressModelHGE(Mem m) : base(m)
    {
        // empty
    }

    public override int HitCountInt() => this.M.Read2Byte("mhfo-hd.dll+ECB2DC6");

    public override int TimeDefInt() => this.M.ReadInt("mhfo-hd.dll+2AFA820");

    public override int TimeInt() => this.M.ReadInt("mhfo-hd.dll+E7FE170");

    public override int WeaponRaw() => this.M.Read2Byte("mhfo-hd.dll+DC6BEFA");

    // This is equipment slot number that goes from 0-255 repeatedly
    // "mhfo-hd.dll+ED3A466
    public override int WeaponType() => this.M.ReadByte("mhfo-hd.dll+DC6B753");

    public override bool IsNotRoad() => this.M.ReadByte("mhfo-hd.dll+DCD4490") == 0;

    public override int LargeMonster1ID() => this.GetNotRoad() ? this.M.ReadByte("mhfo-hd.dll+1BEF3D9") : this.LargeMonster1Road();

    public override int LargeMonster2ID() => this.GetNotRoad() ? this.M.ReadByte("mhfo-hd.dll+1BEF3DA") : this.LargeMonster2Road();

    public override int LargeMonster3ID() => this.M.ReadByte("mhfo-hd.dll+1BEF3DB");

    public override int LargeMonster4ID() => this.M.ReadByte("mhfo-hd.dll+1BEF3DC");

    public int LargeMonster1Road() => this.M.ReadByte("mhfo-hd.dll+DCD4478");

    public int LargeMonster2Road() => this.M.ReadByte("mhfo-hd.dll+DCD4498");

    // TODO monster parts max values
    public string Monster1BP1() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,348").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part1() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,348");

    public string Monster1BP2() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,350").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part2() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,350");

    public string Monster1BP3() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,358").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part3() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,358");

    public string Monster1BP4() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,360").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part4() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,360");

    public string Monster1BP5() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,368").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part5() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,368");

    public string Monster1BP6() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,370").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part6() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,370");

    public string Monster1BP7() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,378").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part7() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,378");

    public string Monster1BP8() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,380").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part8() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,380");

    public string Monster1BP9() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,388").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part9() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,388");

    public string Monster1BP10() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,390").ToString(CultureInfo.InvariantCulture);

    public override int Monster1Part10() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,390");

    public string Monster2BP1() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1238").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part1() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1238");

    public string Monster2BP2() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1240").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part2() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1240");

    public string Monster2BP3() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1248").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part3() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1248");

    public string Monster2BP4() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1250").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part4() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1250");

    public string Monster2BP5() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1258").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part5() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1258");

    public string Monster2BP6() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1260").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part6() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1260");

    public string Monster2BP7() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1268").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part7() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1268");

    public string Monster2BP8() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1270").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part8() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1270");

    public string Monster2BP9() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1278").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part9() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1278");

    public string Monster2BP10() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1280").ToString(CultureInfo.InvariantCulture);

    public override int Monster2Part10() => this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1280");

    public string Monster1RoadBP1() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,348").ToString(CultureInfo.InvariantCulture);

    public string Monster1RoadBP2() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,350").ToString(CultureInfo.InvariantCulture);

    public string Monster1RoadBP3() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,358").ToString(CultureInfo.InvariantCulture);

    public string Monster1RoadBP4() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,360").ToString(CultureInfo.InvariantCulture);

    public string Monster1RoadBP5() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,368").ToString(CultureInfo.InvariantCulture);

    public string Monster1RoadBP6() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,370").ToString(CultureInfo.InvariantCulture);

    public string Monster1RoadBP7() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,378").ToString(CultureInfo.InvariantCulture);

    public string Monster1RoadBP8() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,380").ToString(CultureInfo.InvariantCulture);

    public string Monster1RoadBP9() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,388").ToString(CultureInfo.InvariantCulture);

    public string Monster1RoadBP10() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,390").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP1() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1238").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP2() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1240").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP3() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1248").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP4() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1250").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP5() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1258").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP6() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1260").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP7() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1268").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP8() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1270").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP9() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1278").ToString(CultureInfo.InvariantCulture);

    public string Monster2RoadBP10() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1280").ToString(CultureInfo.InvariantCulture);

    public override int Monster1HPInt() => this.M.Read2Byte("0043C600");

    public override int Monster2HPInt() => this.M.Read2Byte("0043C604");

    public override int Monster3HPInt() => this.M.Read2Byte("0043C608");

    public override int Monster4HPInt() => this.M.Read2Byte("0043C60C");

    public override string Monster1AtkMult() => this.GetNotRoad() ? this.M.ReadFloat("mhfo-hd.dll+0E37DD38,898").ToString(CultureInfo.InvariantCulture) : this.Monster1RoadAtkMult();

    public override decimal Monster1DefMult() => this.GetNotRoad() ? (decimal)this.M.ReadFloat("mhfo-hd.dll+0E37DD38,89C", string.Empty, false) : this.Monster1RoadDefMult();

    public override int Monster1Poison() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,88A") : this.Monster1RoadPoison();

    public override int Monster1PoisonNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,888") : this.Monster1RoadPoisonNeed();

    public override int Monster1Sleep() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,86C") : this.Monster1RoadSleep();

    public override int Monster1SleepNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,86A") : this.Monster1RoadSleepNeed();

    public override int Monster1Para() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,886") : this.Monster1RoadPara();

    public override int Monster1ParaNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,880") : this.Monster1RoadParaNeed();

    public override int Monster1Blast() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,D4A") : this.Monster1RoadBlast();

    public override int Monster1BlastNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,D48") : this.Monster1RoadBlastNeed();

    public override int Monster1Stun() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,872") : this.Monster1RoadStun();

    public override int Monster1StunNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,A74") : this.Monster1RoadStunNeed();

    public override string Monster1Size() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+2AFA784").ToString(CultureInfo.InvariantCulture) + "%" : this.Monster1RoadSize();

    public override string Monster2AtkMult() => this.GetNotRoad() ? this.M.ReadFloat("mhfo-hd.dll+0E37DD38,1788").ToString(CultureInfo.InvariantCulture) : this.Monster2RoadAtkMult();

    public override decimal Monster2DefMult() => this.GetNotRoad() ? (decimal)this.M.ReadFloat("mhfo-hd.dll+0E37DD38,178C", string.Empty, false) : this.Monster2RoadDefMult();

    public override int Monster2Poison() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,177A") : this.Monster2RoadPoison();

    public override int Monster2PoisonNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1778") : this.Monster2RoadPoisonNeed();

    public override int Monster2Sleep() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,175C") : this.Monster2RoadSleep();

    public override int Monster2SleepNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,175A") : this.Monster2RoadSleepNeed();

    public override int Monster2Para() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1776") : this.Monster2RoadPara();

    public override int Monster2ParaNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1770") : this.Monster2RoadParaNeed();

    public override int Monster2Blast() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1C3A") : this.Monster2RoadBlast();

    public override int Monster2BlastNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1C38") : this.Monster2RoadBlastNeed();

    public override int Monster2Stun() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1762") : this.Monster2RoadStun();

    public override int Monster2StunNeed() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+0E37DD38,1964") : this.Monster2RoadStunNeed();

    public override string Monster2Size() => this.GetNotRoad() ? this.M.Read2Byte("mhfo-hd.dll+2AFA784").ToString(CultureInfo.InvariantCulture) + "%" : this.Monster2RoadSize();

    public string Monster1RoadAtkMult() => this.M.ReadFloat("mhfo-hd.dll+E37DF18,898").ToString(CultureInfo.InvariantCulture);

    public decimal Monster1RoadDefMult() => (decimal)this.M.ReadFloat("mhfo-hd.dll+E37DF18,89C", string.Empty, false);

    public int Monster1RoadPoison() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,88A");

    public int Monster1RoadPoisonNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,888");

    public int Monster1RoadSleep() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,86C");

    public int Monster1RoadSleepNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,86A");

    public int Monster1RoadPara() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,886");

    public int Monster1RoadParaNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,880");

    public int Monster1RoadBlast() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,D4A");

    public int Monster1RoadBlastNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,D48");

    public int Monster1RoadStun() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,872");

    public int Monster1RoadStunNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,A74");

    public string Monster1RoadSize() => this.M.Read2Byte("mhfo-hd.dll+2AFA784").ToString(CultureInfo.InvariantCulture) + "%";

    public string Monster2RoadAtkMult() => this.M.ReadFloat("mhfo-hd.dll+E37DF18,1788").ToString(CultureInfo.InvariantCulture);

    public decimal Monster2RoadDefMult() => (decimal)this.M.ReadFloat("mhfo-hd.dll+E37DF18,178C", string.Empty, false);

    public int Monster2RoadPoison() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,177A");

    public int Monster2RoadPoisonNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1778");

    public int Monster2RoadSleep() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,175C");

    public int Monster2RoadSleepNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,175A");

    public int Monster2RoadPara() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1776");

    public int Monster2RoadParaNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1770");

    public int Monster2RoadBlast() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1C3A");

    public int Monster2RoadBlastNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1C38");

    public int Monster2RoadStun() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1762");

    public int Monster2RoadStunNeed() => this.M.Read2Byte("mhfo-hd.dll+E37DF18,1964");

    public string Monster2RoadSize() => this.M.Read2Byte("mhfo-hd.dll+2AFA784").ToString(CultureInfo.InvariantCulture) + "%";

    public override int DamageDealt() => this.M.Read2Byte("mhfo-hd.dll+E8DCF18");

    public override int RoadSelectedMonster() => this.M.ReadByte("mhfo-hd.dll+E87FB04");

    // new addresses
    public override int AreaID() => this.M.Read2Byte("mhfo-hd.dll+DC6BF48");

    public override int GRankNumber() => this.M.Read2Byte("mhfo-hd.dll+ED784D0");

    public override int GSR() => this.M.Read2Byte("mhfo-hd.dll+DC6C562");

    public override int RoadFloor() => this.M.Read2Byte("mhfo-hd.dll+E87FAF0");

    public override int WeaponStyle() => this.M.ReadByte("mhfo-hd.dll+DC6C492");

    public override int QuestID() => this.M.Read2Byte("mhfo-hd.dll+EBEE53C");

    public override int UrukiPachinkoFish() => this.M.ReadByte("mhfo-hd.dll+EE26916");

    public override int UrukiPachinkoMushroom() => this.M.ReadByte("mhfo-hd.dll+EE26918");

    public override int UrukiPachinkoSeed() => this.M.ReadByte("mhfo-hd.dll+EE2691A");

    public override int UrukiPachinkoMeat() => this.M.ReadByte("mhfo-hd.dll+EE26914");

    public override int UrukiPachinkoChain() => this.M.ReadByte("mhfo-hd.dll+EE26900");

    public override int UrukiPachinkoScore() => this.M.ReadInt("mhfo-hd.dll+EE2690C");

    public override int UrukiPachinkoBonusScore() => this.M.ReadInt("mhfo-hd.dll+EE26910");

    public override int NyanrendoScore() => this.M.ReadInt("mhfo-hd.dll+EE26900");

    public override int DokkanBattleCatsScore() => this.M.ReadInt("mhfo-hd.dll+EE268F8");

    public override int DokkanBattleCatsScale() => this.M.ReadByte("mhfo-hd.dll+EE26A8C");

    public override int DokkanBattleCatsShell() => this.M.ReadByte("mhfo-hd.dll+EE268F4");

    public override int DokkanBattleCatsCamp() => this.M.ReadByte("mhfo-hd.dll+EE26A8A");

    public override int GuukuScoopSmall() => this.M.ReadByte("mhfo-hd.dll+EE26930");

    public override int GuukuScoopMedium() => this.M.ReadByte("mhfo-hd.dll+EE26934");

    public override int GuukuScoopLarge() => this.M.ReadByte("mhfo-hd.dll+EE26938");

    public override int GuukuScoopGolden() => this.M.ReadByte("mhfo-hd.dll+EE2693C");

    public override int GuukuScoopScore() => this.M.ReadInt("mhfo-hd.dll+EE26924");

    public override int PanicHoneyScore() => this.M.ReadByte("mhfo-hd.dll+EE26908");

    // TODO: Volpkun Together addresses
    public override int Sharpness() => this.M.Read2Byte("mhfo-hd.dll+DC6C276");

    public override int CaravanPoints() => this.M.ReadInt("mhfo-hd.dll+ED3C034");

    public override int MezeportaFestivalPoints() => this.M.ReadInt("mhfo-hd.dll+EDBA1EC");

    public override int DivaBond() => this.M.Read2Byte("mhfo-hd.dll+ED3DB48");

    public override int DivaItemsGiven() => this.M.Read2Byte("mhfo-hd.dll+ED3DB4A");

    public override int GCP() => this.M.ReadInt("mhfo-hd.dll+E5075D8");

    public override int RoadPoints() => this.M.ReadInt("mhfo-hd.dll+ED3EB98");

    public override int ArmorColor() => this.M.ReadByte("mhfo-hd.dll+EDE66A8");

    public override int RaviGg() => this.M.ReadInt("mhfo-hd.dll+ED3E928");

    public override int Ravig() => this.M.ReadInt("mhfo-hd.dll+ED3AB40");

    public override int GZenny() => this.M.ReadInt("mhfo-hd.dll+ED3ACB4");

    public override int GuildFoodSkill() => this.M.Read2Byte("mhfo-hd.dll+E7FED00");

    public override int GalleryEvaluationScore() => this.M.ReadInt("mhfo-hd.dll+ED3D9F0");

    public override int PoogiePoints() => this.M.ReadByte("mhfo-hd.dll+ED3AAF0");

    public override int PoogieItemUseID() => this.M.Read2Byte("mhfo-hd.dll+ED8E898");

    public override int PoogieCostume() => this.M.ReadByte("mhfo-hd.dll+1A77AF2");

    // zero-indexed
    public override int CaravenGemLevel() => this.M.ReadByte("mhfo-hd.dll+1C747F6");

    public override int RoadMaxStagesMultiplayer() => this.M.Read2Byte("mhfo-hd.dll+E87FB78");

    public override int RoadTotalStagesMultiplayer() => this.M.Read2Byte("mhfo-hd.dll+E87FB58");

    public override int RoadTotalStagesSolo() => this.M.Read2Byte("mhfo-hd.dll+E87FB5C");

    public override int RoadMaxStagesSolo() => this.M.Read2Byte("mhfo-hd.dll+E87FB80");

    public override int RoadFatalisSlain() => this.M.Read2Byte("mhfo-hd.dll+E87FB60");

    public override int RoadFatalisEncounters() => this.M.Read2Byte("mhfo-hd.dll+ED3EBBC");

    public override int FirstDistrictDuremudiraEncounters() => this.M.Read2Byte("mhfo-hd.dll+ED3EBB4");

    public override int FirstDistrictDuremudiraSlays() => this.M.Read2Byte("mhfo-hd.dll+E87FB64");

    public override int SecondDistrictDuremudiraEncounters() => this.M.Read2Byte("mhfo-hd.dll+ED3EBB8");

    public override int SecondDistrictDuremudiraSlays() => this.M.Read2Byte("mhfo-hd.dll+E87FB68");

    public override int DeliveryQuestPoints() => this.M.Read2Byte("mhfo-hd.dll+ED3B212");

    // red is 0
    public override int SharpnessLevel() => this.M.ReadByte("mhfo-hd.dll+DC6C27F");

    public override int PartnerLevel() => this.M.Read2Byte("mhfo-hd.dll+E378E3E");

    // as hex
    public override int ObjectiveType() => this.M.ReadInt("mhfo-hd.dll+2AFA830");

    public override int DivaSkillUsesLeft() => this.M.ReadByte("mhfo-hd.dll+ED3EB0A");

    public override int HalkFullness() => this.M.ReadByte("mhfo-hd.dll+ED3C123");

    public override int RankBand() => this.M.ReadByte("mhfo-hd.dll+2AFA788");

    public override int PartnyaRankPoints() => this.M.ReadInt("mhfo-hd.dll+E551114");

    public override int Objective1ID() => this.M.Read2Byte("mhfo-hd.dll+2AFA834");

    public override int Objective1Quantity() => this.M.Read2Byte("mhfo-hd.dll+2AFA836");

    public override int Objective1CurrentQuantityMonster() => this.M.Read2Byte("mhfo-hd.dll+ECB2A38");

    public override int Objective1CurrentQuantityItem() => this.M.Read2Byte("mhfo-hd.dll+DC6C2F2");

    public override int RavienteTriggeredEvent() => this.M.ReadByte("mhfo-hd.dll+ED3AD66");

    public override int RavienteAreaID() => this.M.Read2Byte("mhfo-hd.dll+ED5F30E");

    // untested
    public override int GreatSlayingPoints() => this.M.ReadInt("mhfo-hd.dll+ED3AD64");

    public override int GreatSlayingPointsSaved() => this.M.ReadInt("mhfo-hd.dll+E77DC20");

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

    public override int DivaSkill() => this.M.ReadByte("mhfo-hd.dll+ED3EB08");

    public override int StarGrades() => this.M.ReadByte("mhfo-hd.dll+E774CAE");

    public override int CurrentFaints() => this.M.ReadByte("mhfo-hd.dll+DC6C35B");

    // public override int MaxFaints() => this.M.ReadByte("mhfo-hd.dll+C4C8FE8");
    public override int MaxFaints() => this.M.ReadByte("mhfo-hd.dll+2B20C0C");

    public override int AlternativeMaxFaints() => this.M.ReadByte("mhfo-hd.dll+2AFA814");

    public override int CaravanSkill1() => this.M.ReadByte("mhfo-hd.dll+DC6C448");

    public override int CaravanSkill2() => this.M.ReadByte("mhfo-hd.dll+DC6C44A");

    public override int CaravanSkill3() => this.M.ReadByte("mhfo-hd.dll+DC6C44C");

    public override int CaravanScore() => this.M.ReadInt("mhfo-hd.dll+ED8F764");

    public override int CaravanMonster1ID() => this.M.ReadByte("mhfo-hd.dll+2AFA834");

    // unsure
    public override int CaravanMonster2ID() => this.M.ReadByte("mhfo-hd.dll+1C41D12");

    public override int BlademasterWeaponID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB52");

    public override int GunnerWeaponID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB52");

    public override int WeaponDeco1ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB56");

    public override int WeaponDeco2ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB58");

    public override int WeaponDeco3ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB5A");

    public override int ArmorHeadID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB12");

    public override int ArmorHeadDeco1ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB16");

    public override int ArmorHeadDeco2ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB18");

    public override int ArmorHeadDeco3ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB1A");

    public override int ArmorChestID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB22");

    public override int ArmorChestDeco1ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB26");

    public override int ArmorChestDeco2ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB28");

    public override int ArmorChestDeco3ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB2A");

    public override int ArmorArmsID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB32");

    public override int ArmorArmsDeco1ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB36");

    public override int ArmorArmsDeco2ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB38");

    public override int ArmorArmsDeco3ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB3A");

    public override int ArmorWaistID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB42");

    public override int ArmorWaistDeco1ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB46");

    public override int ArmorWaistDeco2ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB48");

    public override int ArmorWaistDeco3ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BB4A");

    public override int ArmorLegsID() => this.M.Read2Byte("mhfo-hd.dll+DC6BAF2");

    public override int ArmorLegsDeco1ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BAF6");

    public override int ArmorLegsDeco2ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BAF8");

    public override int ArmorLegsDeco3ID() => this.M.Read2Byte("mhfo-hd.dll+DC6BAFA");

    public override int Cuff1ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C482");

    public override int Cuff2ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C484");

    public override int TotalDefense() => this.M.Read2Byte("mhfo-hd.dll+DC6BEF8");

    public override int PouchItem1ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C168");

    public override int PouchItem1Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C16A");

    public override int PouchItem2ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C170");

    public override int PouchItem2Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C172");

    public override int PouchItem3ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C178");

    public override int PouchItem3Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C17A");

    public override int PouchItem4ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C180");

    public override int PouchItem4Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C182");

    public override int PouchItem5ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C188");

    public override int PouchItem5Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C18A");

    public override int PouchItem6ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C190");

    public override int PouchItem6Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C192");

    public override int PouchItem7ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C198");

    public override int PouchItem7Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C19A");

    public override int PouchItem8ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1A0");

    public override int PouchItem8Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1A2");

    public override int PouchItem9ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1A8");

    public override int PouchItem9Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1AA");

    public override int PouchItem10ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1B0");

    public override int PouchItem10Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1B2");

    public override int PouchItem11ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1B8");

    public override int PouchItem11Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1BA");

    public override int PouchItem12ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1C0");

    public override int PouchItem12Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1C2");

    public override int PouchItem13ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1C8");

    public override int PouchItem13Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1CA");

    public override int PouchItem14ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1D0");

    public override int PouchItem14Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1D2");

    public override int PouchItem15ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1D8");

    public override int PouchItem15Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1DA");

    public override int PouchItem16ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1E0");

    public override int PouchItem16Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1E2");

    public override int PouchItem17ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1E8");

    public override int PouchItem17Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1EA");

    public override int PouchItem18ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1F0");

    public override int PouchItem18Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1F2");

    public override int PouchItem19ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C1F8");

    public override int PouchItem19Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C1FA");

    public override int PouchItem20ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C200");

    public override int PouchItem20Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C202");

    public override int AmmoPouchItem1ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C208");

    public override int AmmoPouchItem1Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C20A");

    public override int AmmoPouchItem2ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C210");

    public override int AmmoPouchItem2Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C212");

    public override int AmmoPouchItem3ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C218");

    public override int AmmoPouchItem3Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C21A");

    public override int AmmoPouchItem4ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C220");

    public override int AmmoPouchItem4Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C222");

    public override int AmmoPouchItem5ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C228");

    public override int AmmoPouchItem5Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C22A");

    public override int AmmoPouchItem6ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C230");

    public override int AmmoPouchItem6Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C232");

    public override int AmmoPouchItem7ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C238");

    public override int AmmoPouchItem7Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C23A");

    public override int AmmoPouchItem8ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C240");

    public override int AmmoPouchItem8Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C242");

    public override int AmmoPouchItem9ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C248");

    public override int AmmoPouchItem9Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C24A");

    public override int AmmoPouchItem10ID() => this.M.Read2Byte("mhfo-hd.dll+DC6C250");

    public override int AmmoPouchItem10Qty() => this.M.Read2Byte("mhfo-hd.dll+DC6C252");

    // slots
    public override int ArmorSkill1() => this.M.Read2Byte("mhfo-hd.dll+DC6C31C");

    public override int ArmorSkill2() => this.M.Read2Byte("mhfo-hd.dll+DC6C31E");

    public override int ArmorSkill3() => this.M.Read2Byte("mhfo-hd.dll+DC6C320");

    public override int ArmorSkill4() => this.M.Read2Byte("mhfo-hd.dll+DC6C322");

    public override int ArmorSkill5() => this.M.Read2Byte("mhfo-hd.dll+DC6C324");

    public override int ArmorSkill6() => this.M.Read2Byte("mhfo-hd.dll+DC6C326");

    public override int ArmorSkill7() => this.M.Read2Byte("mhfo-hd.dll+DC6C328");

    public override int ArmorSkill8() => this.M.Read2Byte("mhfo-hd.dll+DC6C32A");

    public override int ArmorSkill9() => this.M.Read2Byte("mhfo-hd.dll+DC6C32C");

    public override int ArmorSkill10() => this.M.Read2Byte("mhfo-hd.dll+DC6C32E");

    public override int ArmorSkill11() => this.M.Read2Byte("mhfo-hd.dll+DC6C330");

    public override int ArmorSkill12() => this.M.Read2Byte("mhfo-hd.dll+DC6C332");

    public override int ArmorSkill13() => this.M.Read2Byte("mhfo-hd.dll+DC6C334");

    public override int ArmorSkill14() => this.M.Read2Byte("mhfo-hd.dll+DC6C336");

    public override int ArmorSkill15() => this.M.Read2Byte("mhfo-hd.dll+DC6C338");

    public override int ArmorSkill16() => this.M.Read2Byte("mhfo-hd.dll+DC6C33A");

    public override int ArmorSkill17() => this.M.Read2Byte("mhfo-hd.dll+DC6C33C");

    public override int ArmorSkill18() => this.M.Read2Byte("mhfo-hd.dll+DC6C33E");

    public override int ArmorSkill19() => this.M.Read2Byte("mhfo-hd.dll+DC6C340");

    public override int BloatedWeaponAttack() => this.M.Read2Byte("mhfo-hd.dll+E7FE4F0");

    public override int ZenithSkill1() => this.M.Read2Byte("mhfo-hd.dll+DCD1DC8");

    public override int ZenithSkill2() => this.M.Read2Byte("mhfo-hd.dll+DCD1DCA");

    public override int ZenithSkill3() => this.M.Read2Byte("mhfo-hd.dll+DCD1DCC");

    public override int ZenithSkill4() => this.M.Read2Byte("mhfo-hd.dll+DCD1DCE");

    public override int ZenithSkill5() => this.M.Read2Byte("mhfo-hd.dll+DCD1DD0");

    public override int ZenithSkill6() => this.M.Read2Byte("mhfo-hd.dll+DCD1DD2");

    public override int ZenithSkill7() => this.M.Read2Byte("mhfo-hd.dll+DCD1DD4");

    public override int AutomaticSkillWeapon() => this.M.Read2Byte("mhfo-hd.dll+DC6C352");

    public override int AutomaticSkillHead() => this.M.Read2Byte("mhfo-hd.dll+DC6C34A");

    public override int AutomaticSkillChest() => this.M.Read2Byte("mhfo-hd.dll+DC6C34C");

    public override int AutomaticSkillArms() => this.M.Read2Byte("mhfo-hd.dll+DC6C34E");

    public override int AutomaticSkillWaist() => this.M.Read2Byte("mhfo-hd.dll+DC6C350");

    public override int AutomaticSkillLegs() => this.M.Read2Byte("mhfo-hd.dll+DC6C346");

    public override int StyleRank1() => this.M.ReadByte("mhfo-hd.dll+DC6C493");

    public override int StyleRank2() => this.M.ReadByte("mhfo-hd.dll+DC6C55F");

    public override int GRWeaponLv() => this.M.ReadByte("mhfo-hd.dll+DC6BB54");

    public override int GRWeaponLvBowguns() => this.M.ReadByte("mhfo-hd.dll+DC6BB55");

    public override int Sigil1Name1() => this.M.Read2Byte("mhfo-hd.dll+E830E14");

    public override int Sigil1Value1() => this.M.Read2Byte("mhfo-hd.dll+E830E1A");

    public override int Sigil1Name2() => this.M.Read2Byte("mhfo-hd.dll+E830E16");

    public override int Sigil1Value2() => this.M.Read2Byte("mhfo-hd.dll+E830E1C");

    public override int Sigil1Name3() => this.M.Read2Byte("mhfo-hd.dll+E830E18");

    public override int Sigil1Value3() => this.M.Read2Byte("mhfo-hd.dll+E830E1E");

    public override int Sigil2Name1() => this.M.Read2Byte("mhfo-hd.dll+E830E20");

    public override int Sigil2Value1() => this.M.Read2Byte("mhfo-hd.dll+E830E26");

    public override int Sigil2Name2() => this.M.Read2Byte("mhfo-hd.dll+E830E22");

    public override int Sigil2Value2() => this.M.Read2Byte("mhfo-hd.dll+E830E28");

    public override int Sigil2Name3() => this.M.Read2Byte("mhfo-hd.dll+E830E24");

    public override int Sigil2Value3() => this.M.Read2Byte("mhfo-hd.dll+E830E2A");

    public override int Sigil3Name1() => this.M.Read2Byte("mhfo-hd.dll+E831234");

    public override int Sigil3Value1() => this.M.Read2Byte("mhfo-hd.dll+E83123A");

    public override int Sigil3Name2() => this.M.Read2Byte("mhfo-hd.dll+E831236");

    public override int Sigil3Value2() => this.M.Read2Byte("mhfo-hd.dll+E83123C");

    public override int Sigil3Name3() => this.M.Read2Byte("mhfo-hd.dll+E831238");

    public override int Sigil3Value3() => this.M.Read2Byte("mhfo-hd.dll+E83123E");

    public override int RathianHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1AE");

    public override int FatalisHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1B0");

    public override int KelbiHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1B2");

    public override int MosswineHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1B4");

    public override int BullfangoHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1B6");

    public override int YianKutKuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1B8");

    public override int LaoShanLungHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1BA");

    public override int CephadromeHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1BC");

    public override int FelyneHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1BE");

    public override int RathalosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1C2");

    public override int AptonothHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1C4");

    public override int GenpreyHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1C6");

    public override int DiablosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1C8");

    public override int KhezuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1CA");

    public override int VelocipreyHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1CC");

    public override int GraviosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1CE");

    public override int VespoidHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1D2");

    public override int GypcerosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1D4");

    public override int PlesiothHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1D6");

    public override int BasariosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1D8");

    public override int MelynxHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1DA");

    public override int HornetaurHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1DC");

    public override int ApcerosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1DE");

    public override int MonoblosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1E0");

    public override int VelocidromeHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1E2");

    public override int GendromeHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1E4");

    public override int RocksHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1E6");

    public override int IopreyHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1E8");

    public override int IodromeHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1EA");

    public override int KirinHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1EE");

    public override int CephalosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1F0");

    public override int GiapreyHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1F2");

    public override int CrimsonFatalisHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1F4");

    public override int PinkRathianHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1F6");

    public override int BlueYianKutKuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1F8");

    public override int PurpleGypcerosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1FA");

    public override int YianGarugaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1FC");

    public override int SilverRathalosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E1FE");

    public override int GoldRathianHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E200");

    public override int BlackDiablosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E202");

    public override int WhiteMonoblosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E204");

    public override int RedKhezuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E206");

    public override int GreenPlesiothHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E208");

    public override int BlackGraviosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E20A");

    public override int DaimyoHermitaurHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E20C");

    public override int AzureRathalosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E20E");

    public override int AshenLaoShanLungHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E210");

    public override int BlangongaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E212");

    public override int CongalalaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E214");

    public override int RajangHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E216");

    public override int KushalaDaoraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E218");

    public override int ShenGaorenHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E21A");

    public override int GreatThunderbugHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E21C");

    public override int ShakalakaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E21E");

    public override int YamaTsukamiHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E220");

    public override int ChameleosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E222");

    public override int RustedKushalaDaoraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E224");

    public override int BlangoHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E226");

    public override int CongaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E228");

    public override int RemobraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E22A");

    public override int LunastraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E22C");

    public override int TeostraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E22E");

    public override int HermitaurHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E230");

    public override int ShogunCeanataurHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E232");

    public override int BulldromeHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E234");

    public override int AntekaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E236");

    public override int PopoHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E238");

    public override int WhiteFatalisHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E23A");

    public override int CeanataurHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E23E");

    public override int HypnocHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E240");

    public override int VolganosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E242");

    public override int TigrexHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E244");

    public override int AkantorHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E246");

    public override int BrightHypnocHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E248");

    public override int RedVolganosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E24A");

    public override int EspinasHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E24C");

    public override int OrangeEspinasHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E24E");

    public override int SilverHypnocHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E250");

    public override int AkuraVashimuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E252");

    public override int AkuraJebiaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E254");

    public override int BerukyurosuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E256");

    public override int CactusHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E258");

    public override int GorgeObjectsHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E25A");

    public override int PariapuriaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E25E");

    public override int WhiteEspinasHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E260");

    public override int KamuOrugaronHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E262");

    public override int NonoOrugaronHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E264");

    public override int DyuragauaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E268");

    public override int DoragyurosuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E26A");

    public override int GurenzeburuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E26C");

    public override int BurukkuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E26E");

    public override int ErupeHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E270");

    public override int RukodioraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E272");

    public override int UnknownHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E274");

    public override int GogomoaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E276");

    public override int TaikunZamuzaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E27A");

    public override int AbioruguHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E27C");

    public override int KuarusepusuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E27E");

    public override int OdibatorasuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E280");

    public override int DisufiroaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E282");

    public override int RebidioraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E284");

    public override int AnorupatisuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E286");

    public override int HyujikikiHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E288");

    public override int MidogaronHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E28A");

    public override int GiaoruguHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E28C");

    public override int MiRuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E28E");

    public override int FarunokkuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E290");

    public override int PokaradonHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E292");

    public override int ShantienHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E294");

    public override int PokaraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E296");

    public override int GoruganosuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E29A");

    public override int AruganosuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E29C");

    public override int BaruragaruHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E29E");

    public override int ZerureusuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2A0");

    public override int GougarfHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2A2");

    public override int UrukiHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2A4");

    public override int ForokururuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2A6");

    public override int MeraginasuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2A8");

    public override int DiorexHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2AA");

    public override int GarubaDaoraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2AC");

    public override int InagamiHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2AE");

    public override int VarusaburosuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2B0");

    public override int PoborubarumuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2B2");

    public override int GureadomosuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2C2");

    public override int HarudomeruguHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2C4");

    public override int ToridclessHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2C6");

    public override int GasurabazuraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2C8");

    public override int KusubamiHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2CA");

    public override int YamaKuraiHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2CC");

    public override int ZinogreHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2D0");

    public override int DeviljhoHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2D2");

    public override int BrachydiosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2D4");

    public override int ToaTesukatoraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2D8");

    public override int BariothHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2DA");

    public override int UragaanHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2DC");

    public override int StygianZinogreHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2DE");

    public override int GuanzorumuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2E0");

    public override int StarvingDeviljhoHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2E2");

    public override int VoljangHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2E8");

    public override int NargacugaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2EA");

    public override int KeoaruboruHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2EC");

    public override int ZenaserisuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2EE");

    public override int GoreMagalaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2F0");

    public override int BlinkingNargacugaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2F2");

    public override int ShagaruMagalaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2F4");

    public override int AmatsuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2F6");

    public override int ElzelionHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2F8");

    public override int ArrogantDuremudiraHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2FA");

    public override int SeregiosHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E2FE");

    public override int BogabadorumuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E300");

    public override int BlitzkriegBogabadorumuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E304");

    public override int SparklingZerureusuHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E308");

    public override int PSO2RappyHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E30A");

    public override int KingShakalakaHunted() => this.M.Read2Byte("mhfo-hd.dll+ED3E30C");

    public override int QuestState() => this.M.ReadByte("mhfo-hd.dll+ED52892");

    public override int RoadDureSkill1Name() => this.M.ReadByte("mhfo-hd.dll+ED3E7DC");

    public override int RoadDureSkill1Level() => this.M.ReadByte("mhfo-hd.dll+ED3E7DE");

    public override int RoadDureSkill2Name() => this.M.ReadByte("mhfo-hd.dll+ED3E7E0");

    public override int RoadDureSkill2Level() => this.M.ReadByte("mhfo-hd.dll+ED3E7E2");

    public override int RoadDureSkill3Name() => this.M.ReadByte("mhfo-hd.dll+ED3E7E4");

    public override int RoadDureSkill3Level() => this.M.ReadByte("mhfo-hd.dll+ED3E7E6");

    public override int RoadDureSkill4Name() => this.M.ReadByte("mhfo-hd.dll+ED3E7E8");

    public override int RoadDureSkill4Level() => this.M.ReadByte("mhfo-hd.dll+ED3E7EA");

    public override int RoadDureSkill5Name() => this.M.ReadByte("mhfo-hd.dll+ED3E7EC");

    public override int RoadDureSkill5Level() => this.M.ReadByte("mhfo-hd.dll+ED3E7EE");

    public override int RoadDureSkill6Name() => this.M.ReadByte("mhfo-hd.dll+ED3E7F0");

    public override int RoadDureSkill6Level() => this.M.ReadByte("mhfo-hd.dll+ED3E7F2");

    public override int RoadDureSkill7Name() => this.M.ReadByte("mhfo-hd.dll+ED3E7F4");

    public override int RoadDureSkill7Level() => this.M.ReadByte("mhfo-hd.dll+ED3E7F6");

    public override int RoadDureSkill8Name() => this.M.ReadByte("mhfo-hd.dll+ED3E7F8");

    public override int RoadDureSkill8Level() => this.M.ReadByte("mhfo-hd.dll+ED3E7FA");

    public override int RoadDureSkill9Name() => this.M.ReadByte("mhfo-hd.dll+ED3E7FC");

    public override int RoadDureSkill9Level() => this.M.ReadByte("mhfo-hd.dll+ED3E7FE");

    public override int RoadDureSkill10Name() => this.M.ReadByte("mhfo-hd.dll+ED3E800");

    public override int RoadDureSkill10Level() => this.M.ReadByte("mhfo-hd.dll+ED3E802");

    public override int RoadDureSkill11Name() => this.M.ReadByte("mhfo-hd.dll+ED3E804");

    public override int RoadDureSkill11Level() => this.M.ReadByte("mhfo-hd.dll+ED3E806");

    public override int RoadDureSkill12Name() => this.M.ReadByte("mhfo-hd.dll+ED3E808");

    public override int RoadDureSkill12Level() => this.M.ReadByte("mhfo-hd.dll+ED3E80A");

    public override int RoadDureSkill13Name() => this.M.ReadByte("mhfo-hd.dll+ED3E80C");

    public override int RoadDureSkill13Level() => this.M.ReadByte("mhfo-hd.dll+ED3E80E");

    public override int RoadDureSkill14Name() => this.M.ReadByte("mhfo-hd.dll+ED3E810");

    public override int RoadDureSkill14Level() => this.M.ReadByte("mhfo-hd.dll+ED3E812");

    public override int RoadDureSkill15Name() => this.M.ReadByte("mhfo-hd.dll+ED3E814");

    public override int RoadDureSkill15Level() => this.M.ReadByte("mhfo-hd.dll+ED3E816");

    public override int RoadDureSkill16Name() => this.M.ReadByte("mhfo-hd.dll+ED3E818");

    public override int RoadDureSkill16Level() => this.M.ReadByte("mhfo-hd.dll+ED3E81A");

    public override int PartySize() => this.M.ReadByte("mhfo-hd.dll+E3CE388");

    public override int PartySizeMax() => this.M.ReadByte("mhfo-hd.dll+EDF0828");

    public override uint GSRP() => 1;

    public override uint GRP() => 1;

    public override int HunterHP() => this.M.Read2Byte("mhfo-hd.dll+E7FE178");

    public override int HunterStamina() => this.M.Read2Byte("mhfo-hd.dll+DC6BF4C");

    // farcaster doesnt count as used
    public override int QuestItemsUsed() => this.M.Read2Byte("mhfo-hd.dll+E4229E4");

    public override int AreaHitsTakenBlocked() => this.M.Read2Byte("mhfo-hd.dll+DC6BC38");

    public override int PartnyaBagItem1ID() => this.M.Read2Byte("mhfo-hd.dll+E37D348");

    public override int PartnyaBagItem1Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D34A");

    public override int PartnyaBagItem2ID() => this.M.Read2Byte("mhfo-hd.dll+E37D34C");

    public override int PartnyaBagItem2Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D34E");

    public override int PartnyaBagItem3ID() => this.M.Read2Byte("mhfo-hd.dll+E37D350");

    public override int PartnyaBagItem3Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D352");

    public override int PartnyaBagItem4ID() => this.M.Read2Byte("mhfo-hd.dll+E37D354");

    public override int PartnyaBagItem4Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D356");

    public override int PartnyaBagItem5ID() => this.M.Read2Byte("mhfo-hd.dll+E37D358");

    public override int PartnyaBagItem5Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D35A");

    public override int PartnyaBagItem6ID() => this.M.Read2Byte("mhfo-hd.dll+E37D35C");

    public override int PartnyaBagItem6Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D35E");

    public override int PartnyaBagItem7ID() => this.M.Read2Byte("mhfo-hd.dll+E37D360");

    public override int PartnyaBagItem7Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D362");

    public override int PartnyaBagItem8ID() => this.M.Read2Byte("mhfo-hd.dll+E37D364");

    public override int PartnyaBagItem8Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D366");

    public override int PartnyaBagItem9ID() => this.M.Read2Byte("mhfo-hd.dll+E37D368");

    public override int PartnyaBagItem9Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D36A");

    public override int PartnyaBagItem10ID() => this.M.Read2Byte("mhfo-hd.dll+E37D36C");

    public override int PartnyaBagItem10Qty() => this.M.Read2Byte("mhfo-hd.dll+E37D36E");
}
