// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

// Most Addresses from https:// github.com/suzaku01/
namespace MHFZ_Overlay.Models.Addresses;

using System.Globalization;
using Memory;
using MHFZ_Overlay.ViewModels.Windows;

/// <summary>
/// Inherits from AddressModel and provides the memory address of the hit count value (etc.) when the game is running in non-High Grade Edition (HGE) mode.
/// </summary>
public class AddressModelNotHGE : AddressModel
{
    public AddressModelNotHGE(Mem m) : base(m)
    {
        // empty
    }

    public override int HitCountInt() => this.M.Read2Byte("mhfo.dll+60792E6");

    // public override int TimeDefInt() => this.M.ReadInt("mhfo.dll+1B97780");
    public override int TimeDefInt() => this.M.ReadInt("mhfo.dll+28C2C70");

    public override int TimeInt() => this.M.ReadInt("mhfo.dll+5BC6540");

    // alternative timeint for dure? mhfo.dll+5BC7600
    public override int WeaponRaw() => this.M.Read2Byte("mhfo.dll+503433A");

    // This is equipment slot number that goes from 0-255 repeatedly
    // "mhfo.dll+60FFCC6
    // "mhfo.dll+B7FF45
    // "mhfo.dll+182D3B93
    // "mhfo.dll+13E1FF45
    public override int WeaponType() => this.M.ReadByte("mhfo.dll+5033B93");

    public override bool IsNotRoad() => this.M.ReadByte("mhfo.dll+509C8B0") == 0;

    public override int LargeMonster1ID() => this.GetNotRoad() ? this.M.ReadByte("mhfo.dll+1B97794") : this.LargeMonster1Road();

    public override int LargeMonster2ID() => this.GetNotRoad() ? this.M.ReadByte("mhfo.dll+1B9779C") : this.LargeMonster2Road();

    public override int LargeMonster3ID() => this.M.ReadByte("mhfo.dll+1B977A4");

    public override int LargeMonster4ID() => this.M.ReadByte("mhfo.dll+1B977AC");

    public int LargeMonster1Road() => this.M.ReadByte("mhfo.dll+509C8B8");

    public int LargeMonster2Road() => this.M.ReadByte("mhfo.dll+509C8D8");

    public string Monster1BP1 { get => this.M.Read2Byte("mhfo.dll+60A3E58,348").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part1() => this.M.Read2Byte("mhfo.dll+60A3E58,348");

    public string Monster1BP2 { get => this.M.Read2Byte("mhfo.dll+60A3E58,350").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part2() => this.M.Read2Byte("mhfo.dll+60A3E58,350");

    public string Monster1BP3 { get => this.M.Read2Byte("mhfo.dll+60A3E58,358").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part3() => this.M.Read2Byte("mhfo.dll+60A3E58,358");

    public string Monster1BP4 { get => this.M.Read2Byte("mhfo.dll+60A3E58,360").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part4() => this.M.Read2Byte("mhfo.dll+60A3E58,360");

    public string Monster1BP5 { get => this.M.Read2Byte("mhfo.dll+60A3E58,368").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part5() => this.M.Read2Byte("mhfo.dll+60A3E58,368");

    public string Monster1BP6 { get => this.M.Read2Byte("mhfo.dll+60A3E58,370").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part6() => this.M.Read2Byte("mhfo.dll+60A3E58,370");

    public string Monster1BP7 { get => this.M.Read2Byte("mhfo.dll+60A3E58,378").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part7() => this.M.Read2Byte("mhfo.dll+60A3E58,378");

    public string Monster1BP8 { get => this.M.Read2Byte("mhfo.dll+60A3E58,380").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part8() => this.M.Read2Byte("mhfo.dll+60A3E58,380");

    public string Monster1BP9 { get => this.M.Read2Byte("mhfo.dll+60A3E58,388").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part9() => this.M.Read2Byte("mhfo.dll+60A3E58,388");

    public string Monster1BP10 { get => this.M.Read2Byte("mhfo.dll+60A3E58,390").ToString(CultureInfo.InvariantCulture); }

    public override int Monster1Part10() => this.M.Read2Byte("mhfo.dll+60A3E58,390");

    public string Monster2BP1 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1238").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part1() => this.M.Read2Byte("mhfo.dll+60A3E58,1238");

    public string Monster2BP2 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1240").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part2() => this.M.Read2Byte("mhfo.dll+60A3E58,1240");

    public string Monster2BP3 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1248").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part3() => this.M.Read2Byte("mhfo.dll+60A3E58,1248");

    public string Monster2BP4 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1250").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part4() => this.M.Read2Byte("mhfo.dll+60A3E58,1250");

    public string Monster2BP5 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1258").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part5() => this.M.Read2Byte("mhfo.dll+60A3E58,1258");

    public string Monster2BP6 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1260").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part6() => this.M.Read2Byte("mhfo.dll+60A3E58,1260");

    public string Monster2BP7 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1268").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part7() => this.M.Read2Byte("mhfo.dll+60A3E58,1268");

    public string Monster2BP8 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1270").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part8() => this.M.Read2Byte("mhfo.dll+60A3E58,1270");

    public string Monster2BP9 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1278").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part9() => this.M.Read2Byte("mhfo.dll+60A3E58,1278");

    public string Monster2BP10 { get => this.M.Read2Byte("mhfo.dll+60A3E58,1280").ToString(CultureInfo.InvariantCulture); }

    public override int Monster2Part10() => this.M.Read2Byte("mhfo.dll+60A3E58,1280");

    public override int Monster1HPInt() => this.M.Read2Byte("0043C600");

    public override int Monster2HPInt() => this.M.Read2Byte("0043C604");

    public override int Monster3HPInt() => this.M.Read2Byte("0043C608");

    public override int Monster4HPInt() => this.M.Read2Byte("0043C60C");

    public override string Monster1AtkMult() => this.M.ReadFloat("mhfo.dll+60A3E58,898").ToString(CultureInfo.InvariantCulture);

    public override decimal Monster1DefMult() => (decimal)this.M.ReadFloat("mhfo.dll+60A3E58,89C", "", false);

    public override int Monster1Poison() => this.M.Read2Byte("mhfo.dll+60A3E58,88A");

    public override int Monster1PoisonNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,888");

    public override int Monster1Sleep() => this.M.Read2Byte("mhfo.dll+60A3E58,86C");

    public override int Monster1SleepNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,86A");

    public override int Monster1Para() => this.M.Read2Byte("mhfo.dll+60A3E58,886");

    public override int Monster1ParaNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,880");

    public override int Monster1Blast() => this.M.Read2Byte("mhfo.dll+60A3E58,D4A");

    public override int Monster1BlastNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,D48");

    public override int Monster1Stun() => this.M.Read2Byte("mhfo.dll+60A3E58,872");

    public override int Monster1StunNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,A74");

    public override string Monster1Size() => this.M.Read2Byte("mhfo.dll+2AFA784").ToString(CultureInfo.InvariantCulture) + "%";

    public override string Monster2AtkMult() => this.M.ReadFloat("mhfo.dll+60A3E58,1788").ToString(CultureInfo.InvariantCulture);

    public override decimal Monster2DefMult() => (decimal)this.M.ReadFloat("mhfo.dll+60A3E58,178C", "", false);

    public override int Monster2Poison() => this.M.Read2Byte("mhfo.dll+60A3E58,177A");

    public override int Monster2PoisonNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,1778");

    public override int Monster2Sleep() => this.M.Read2Byte("mhfo.dll+60A3E58,175C");

    public override int Monster2SleepNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,175A");

    public override int Monster2Para() => this.M.Read2Byte("mhfo.dll+60A3E58,1776");

    public override int Monster2ParaNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,1770");

    public override int Monster2Blast() => this.M.Read2Byte("mhfo.dll+60A3E58,1C3A");

    public override int Monster2BlastNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,1C38");

    public override int Monster2Stun() => this.M.Read2Byte("mhfo.dll+60A3E58,1762");

    public override int Monster2StunNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,1964");

    public override string Monster2Size() => this.M.Read2Byte("mhfo.dll+2AFA784").ToString(CultureInfo.InvariantCulture) + "%";

    public override int DamageDealt() => this.M.Read2Byte("mhfo.dll+5CA3430");

    public override int RoadSelectedMonster() => this.M.ReadByte("mhfo.dll+001B48F4,4");

    // new addresses
    public override int AreaID() => this.M.Read2Byte("mhfo.dll+5034388");

    public override int RavienteAreaID() => this.M.Read2Byte("mhfo.dll+6124B6E");

    public override int GRankNumber() => this.M.Read2Byte("mhfo.dll+613DD30");

    public override int GSR() => this.M.Read2Byte("mhfo.dll+50349A2");

    public override int RoadFloor() => this.M.Read2Byte("mhfo.dll+5C47600");

    public override int WeaponStyle() => this.M.ReadByte("mhfo.dll+50348D2");

    public override int QuestID() => this.M.Read2Byte("mhfo.dll+5FB4A4C");

    public override int UrukiPachinkoFish() => this.M.ReadByte("mhfo.dll+61EC176");

    public override int UrukiPachinkoMushroom() => this.M.ReadByte("mhfo.dll+61EC178");

    public override int UrukiPachinkoSeed() => this.M.ReadByte("mhfo.dll+61EC17A");

    public override int UrukiPachinkoMeat() => this.M.ReadByte("mhfo.dll+61EC174");

    public override int UrukiPachinkoChain() => this.M.ReadByte("mhfo.dll+61EC160");

    public override int UrukiPachinkoScore() => this.M.ReadInt("mhfo.dll+61EC16C");

    public override int UrukiPachinkoBonusScore() => this.M.ReadInt("mhfo.dll+61EC170");

    public override int NyanrendoScore() => this.M.ReadInt("mhfo.dll+61EC160");

    public override int DokkanBattleCatsScore() => this.M.ReadInt("mhfo.dll+61EC158");

    public override int DokkanBattleCatsScale() => this.M.ReadByte("mhfo.dll+61EC2EC");

    public override int DokkanBattleCatsShell() => this.M.ReadByte("mhfo.dll+61EC2EE");

    public override int DokkanBattleCatsCamp() => this.M.ReadByte("mhfo.dll+61EC2EA");

    public override int GuukuScoopSmall() => this.M.ReadByte("mhfo.dll+61EC190");

    public override int GuukuScoopMedium() => this.M.ReadByte("mhfo.dll+61EC194");

    public override int GuukuScoopLarge() => this.M.ReadByte("mhfo.dll+61EC198");

    public override int GuukuScoopGolden() => this.M.ReadByte("mhfo.dll+61EC19C");

    public override int GuukuScoopScore() => this.M.ReadInt("mhfo.dll+61EC184");

    public override int PanicHoneyScore() => this.M.ReadByte("mhfo.dll+61EC168");

    public override int Sharpness() => this.M.Read2Byte("mhfo.dll+50346B6");

    public override int CaravanPoints() => this.M.ReadInt("mhfo.dll+6101894");

    public override int MezeportaFestivalPoints() => this.M.ReadInt("mhfo.dll+617FA4C");

    public override int DivaBond() => this.M.Read2Byte("mhfo.dll+61033A8");

    public override int DivaItemsGiven() => this.M.Read2Byte("mhfo.dll+61033AA");

    public override int GCP() => this.M.ReadInt("mhfo.dll+58CFA18");

    public override int RoadPoints() => this.M.ReadInt("mhfo.dll+61043F8");

    public override int ArmorColor() => this.M.ReadByte("mhfo.dll+6100476");

    public override int RaviGg() => this.M.ReadInt("mhfo.dll+6104188");

    public override int Ravig() => this.M.ReadInt("mhfo.dll+61003A0");

    public override int GZenny() => this.M.ReadInt("mhfo.dll+6100514");

    public override int GuildFoodSkill() => this.M.Read2Byte("mhfo.dll+5BC70D8");

    public override int GalleryEvaluationScore() => this.M.ReadInt("mhfo.dll+6103250");

    public override int PoogiePoints() => this.M.ReadByte("mhfo.dll+6100350");

    public override int PoogieItemUseID() => this.M.Read2Byte("mhfo.dll+61540F8");

    public override int PoogieCostume() => this.M.ReadByte("mhfo.dll+1A88392");

    // zero-indexed
    public override int CaravenGemLevel() => this.M.ReadByte("mhfo.dll+610037D");

    public override int RoadMaxStagesMultiplayer() => this.M.Read2Byte("mhfo.dll+5C47688");

    public override int RoadTotalStagesMultiplayer() => this.M.Read2Byte("mhfo.dll+5C47668");

    public override int RoadTotalStagesSolo() => this.M.Read2Byte("mhfo.dll+5C4766C");

    public override int RoadMaxStagesSolo() => this.M.Read2Byte("mhfo.dll+5C47690");

    public override int RoadFatalisSlain() => this.M.Read2Byte("mhfo.dll+5C47670");

    public override int RoadFatalisEncounters() => this.M.Read2Byte("mhfo.dll+5C47698");

    public override int FirstDistrictDuremudiraEncounters() => this.M.Read2Byte("mhfo.dll+6104414");

    public override int FirstDistrictDuremudiraSlays() => this.M.Read2Byte("mhfo.dll+6104FCC");

    public override int SecondDistrictDuremudiraEncounters() => this.M.Read2Byte("mhfo.dll+6104418");

    public override int SecondDistrictDuremudiraSlays() => this.M.Read2Byte("mhfo.dll+5C47678");

    public override int DeliveryQuestPoints() => this.M.Read2Byte("mhfo.dll+6100A72");

    // red is 0
    public override int SharpnessLevel() => this.M.ReadByte("mhfo.dll+50346BF");

    public override int PartnerLevel() => this.M.Read2Byte("mhfo.dll+574127E");

    // as hex, consult addresses README.md
    public override int ObjectiveType() => this.M.ReadInt("mhfo.dll+28C2C80");

    public override int DivaSkillUsesLeft() => this.M.ReadByte("mhfo.dll+610436A");

    public override int HalkFullness() => this.M.ReadByte("mhfo.dll+6101983");

    public override int RankBand() => this.M.ReadByte("mhfo.dll+28C2BD8");

    public override int PartnyaRankPoints() => this.M.ReadInt("mhfo.dll+5919554");

    public override int Objective1ID() => this.M.Read2Byte("mhfo.dll+28C2C84");

    public override int Objective1Quantity() => this.M.Read2Byte("mhfo.dll+28C2C86");

    public override int Objective1CurrentQuantityMonster() => this.M.Read2Byte("mhfo.dll+60792E6");

    public override int Objective1CurrentQuantityItem() => this.M.Read2Byte("mhfo.dll+5034732");

    public override int RavienteTriggeredEvent() => this.M.Read2Byte("mhfo.dll+61005C6");

    public override int GreatSlayingPoints() => this.M.ReadInt("mhfo.dll+5B45FF8");

    public override int GreatSlayingPointsSaved() => this.M.ReadInt("mhfo.dll+61005C4");

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

    public override int DivaSkill() => this.M.ReadByte("mhfo.dll+6104368");

    public override int StarGrades() => this.M.ReadByte("mhfo.dll+5B3D086");

    public override int CaravanSkill1() => this.M.ReadByte("mhfo.dll+5034888");

    public override int CaravanSkill2() => this.M.ReadByte("mhfo.dll+503488A");

    public override int CaravanSkill3() => this.M.ReadByte("mhfo.dll+503488C");

    public override int CurrentFaints() => this.M.ReadByte("mhfo.dll+503479B");

    public override int MaxFaints() => this.M.ReadByte("mhfo.dll+1AA899C");

    public override int AlternativeMaxFaints() => this.M.ReadByte("mhfo.dll+28C2C64");

    public override int CaravanScore() => this.M.ReadInt("mhfo.dll+6154FC4");

    public override int CaravanMonster1ID() => this.M.ReadByte("mhfo.dll+28C2C84");

    // unsure
    public override int CaravanMonster2ID() => this.M.ReadByte("mhfo.dll+28C2C8C");

    public override int BlademasterWeaponID() => this.M.Read2Byte("mhfo.dll+5033F92");

    public override int GunnerWeaponID() => this.M.Read2Byte("mhfo.dll+5033F92");

    // the deco addresses for the weapon includes the tower sigils
    public override int WeaponDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F96");

    public override int WeaponDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F98");

    public override int WeaponDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F9A");

    public override int ArmorHeadID() => this.M.Read2Byte("mhfo.dll+5033F52");

    public override int ArmorHeadDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F56");

    public override int ArmorHeadDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F58");

    public override int ArmorHeadDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F5A");

    public override int ArmorChestID() => this.M.Read2Byte("mhfo.dll+5033F62");

    public override int ArmorChestDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F66");

    public override int ArmorChestDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F68");

    public override int ArmorChestDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F6A");

    public override int ArmorArmsID() => this.M.Read2Byte("mhfo.dll+5033F72");

    public override int ArmorArmsDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F76");

    public override int ArmorArmsDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F78");

    public override int ArmorArmsDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F7A");

    public override int ArmorWaistID() => this.M.Read2Byte("mhfo.dll+5033F82");

    public override int ArmorWaistDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F86");

    public override int ArmorWaistDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F88");

    public override int ArmorWaistDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F8A");

    public override int ArmorLegsID() => this.M.Read2Byte("mhfo.dll+5033F32");

    public override int ArmorLegsDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F36");

    public override int ArmorLegsDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F38");

    public override int ArmorLegsDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F3A");

    public override int Cuff1ID() => this.M.Read2Byte("mhfo.dll+50348C2");

    public override int Cuff2ID() => this.M.Read2Byte("mhfo.dll+50348C4");

    // updates when checking guild card
    public override int TotalDefense() => this.M.Read2Byte("mhfo.dll+5034338");

    public override int PouchItem1ID() => this.M.Read2Byte("mhfo.dll+50345A8");

    public override int PouchItem1Qty() => this.M.Read2Byte("mhfo.dll+50345AA");

    public override int PouchItem2ID() => this.M.Read2Byte("mhfo.dll+50345B0");

    public override int PouchItem2Qty() => this.M.Read2Byte("mhfo.dll+50345B2");

    public override int PouchItem3ID() => this.M.Read2Byte("mhfo.dll+50345B8");

    public override int PouchItem3Qty() => this.M.Read2Byte("mhfo.dll+50345BA");

    public override int PouchItem4ID() => this.M.Read2Byte("mhfo.dll+50345C0");

    public override int PouchItem4Qty() => this.M.Read2Byte("mhfo.dll+50345C2");

    public override int PouchItem5ID() => this.M.Read2Byte("mhfo.dll+50345C8");

    public override int PouchItem5Qty() => this.M.Read2Byte("mhfo.dll+50345CA");

    public override int PouchItem6ID() => this.M.Read2Byte("mhfo.dll+50345D0");

    public override int PouchItem6Qty() => this.M.Read2Byte("mhfo.dll+50345D2");

    public override int PouchItem7ID() => this.M.Read2Byte("mhfo.dll+50345D8");

    public override int PouchItem7Qty() => this.M.Read2Byte("mhfo.dll+610445A");

    public override int PouchItem8ID() => this.M.Read2Byte("mhfo.dll+50345E0");

    public override int PouchItem8Qty() => this.M.Read2Byte("mhfo.dll+50345E2");

    public override int PouchItem9ID() => this.M.Read2Byte("mhfo.dll+50345E8");

    public override int PouchItem9Qty() => this.M.Read2Byte("mhfo.dll+610446A");

    public override int PouchItem10ID() => this.M.Read2Byte("mhfo.dll+50345F0");

    public override int PouchItem10Qty() => this.M.Read2Byte("mhfo.dll+50345F2");

    public override int PouchItem11ID() => this.M.Read2Byte("mhfo.dll+50345F8");

    public override int PouchItem11Qty() => this.M.Read2Byte("mhfo.dll+50345FA");

    public override int PouchItem12ID() => this.M.Read2Byte("mhfo.dll+5034600");

    public override int PouchItem12Qty() => this.M.Read2Byte("mhfo.dll+5034602");

    public override int PouchItem13ID() => this.M.Read2Byte("mhfo.dll+5034608");

    public override int PouchItem13Qty() => this.M.Read2Byte("mhfo.dll+503460A");

    public override int PouchItem14ID() => this.M.Read2Byte("mhfo.dll+5034610");

    public override int PouchItem14Qty() => this.M.Read2Byte("mhfo.dll+5034612");

    public override int PouchItem15ID() => this.M.Read2Byte("mhfo.dll+5034618");

    public override int PouchItem15Qty() => this.M.Read2Byte("mhfo.dll+503461A");

    public override int PouchItem16ID() => this.M.Read2Byte("mhfo.dll+5034620");

    public override int PouchItem16Qty() => this.M.Read2Byte("mhfo.dll+5034622");

    public override int PouchItem17ID() => this.M.Read2Byte("mhfo.dll+5034628");

    public override int PouchItem17Qty() => this.M.Read2Byte("mhfo.dll+503462A");

    public override int PouchItem18ID() => this.M.Read2Byte("mhfo.dll+5034630");

    public override int PouchItem18Qty() => this.M.Read2Byte("mhfo.dll+5034632");

    public override int PouchItem19ID() => this.M.Read2Byte("mhfo.dll+5034638");

    public override int PouchItem19Qty() => this.M.Read2Byte("mhfo.dll+61044BA");

    public override int PouchItem20ID() => this.M.Read2Byte("mhfo.dll+5034640");

    public override int PouchItem20Qty() => this.M.Read2Byte("mhfo.dll+5034642");

    public override int AmmoPouchItem1ID() => this.M.Read2Byte("mhfo.dll+5034648");

    public override int AmmoPouchItem1Qty() => this.M.Read2Byte("mhfo.dll+503464A");

    public override int AmmoPouchItem2ID() => this.M.Read2Byte("mhfo.dll+5034650");

    public override int AmmoPouchItem2Qty() => this.M.Read2Byte("mhfo.dll+5034652");

    public override int AmmoPouchItem3ID() => this.M.Read2Byte("mhfo.dll+5034658");

    public override int AmmoPouchItem3Qty() => this.M.Read2Byte("mhfo.dll+503465A");

    public override int AmmoPouchItem4ID() => this.M.Read2Byte("mhfo.dll+5034660");

    public override int AmmoPouchItem4Qty() => this.M.Read2Byte("mhfo.dll+5034662");

    public override int AmmoPouchItem5ID() => this.M.Read2Byte("mhfo.dll+5034668");

    public override int AmmoPouchItem5Qty() => this.M.Read2Byte("mhfo.dll+503466A");

    public override int AmmoPouchItem6ID() => this.M.Read2Byte("mhfo.dll+5034670");

    public override int AmmoPouchItem6Qty() => this.M.Read2Byte("mhfo.dll+5034672");

    public override int AmmoPouchItem7ID() => this.M.Read2Byte("mhfo.dll+5034678");

    public override int AmmoPouchItem7Qty() => this.M.Read2Byte("mhfo.dll+503467A");

    public override int AmmoPouchItem8ID() => this.M.Read2Byte("mhfo.dll+5034680");

    public override int AmmoPouchItem8Qty() => this.M.Read2Byte("mhfo.dll+5034682");

    public override int AmmoPouchItem9ID() => this.M.Read2Byte("mhfo.dll+5034688");

    public override int AmmoPouchItem9Qty() => this.M.Read2Byte("mhfo.dll+503468A");

    public override int AmmoPouchItem10ID() => this.M.Read2Byte("mhfo.dll+5034690");

    public override int AmmoPouchItem10Qty() => this.M.Read2Byte("mhfo.dll+5034692");

    // TODO: cat pouch
    // slots
    public override int ArmorSkill1() => this.M.Read2Byte("mhfo.dll+503475C");

    public override int ArmorSkill2() => this.M.Read2Byte("mhfo.dll+503475E");

    public override int ArmorSkill3() => this.M.Read2Byte("mhfo.dll+5034760");

    public override int ArmorSkill4() => this.M.Read2Byte("mhfo.dll+5034762");

    public override int ArmorSkill5() => this.M.Read2Byte("mhfo.dll+5034764");

    public override int ArmorSkill6() => this.M.Read2Byte("mhfo.dll+5034766");

    public override int ArmorSkill7() => this.M.Read2Byte("mhfo.dll+5034768");

    public override int ArmorSkill8() => this.M.Read2Byte("mhfo.dll+503476A");

    public override int ArmorSkill9() => this.M.Read2Byte("mhfo.dll+503476C");

    public override int ArmorSkill10() => this.M.Read2Byte("mhfo.dll+503476E");

    public override int ArmorSkill11() => this.M.Read2Byte("mhfo.dll+5034770");

    public override int ArmorSkill12() => this.M.Read2Byte("mhfo.dll+5034772");

    public override int ArmorSkill13() => this.M.Read2Byte("mhfo.dll+5034774");

    public override int ArmorSkill14() => this.M.Read2Byte("mhfo.dll+5034776");

    public override int ArmorSkill15() => this.M.Read2Byte("mhfo.dll+5034778");

    public override int ArmorSkill16() => this.M.Read2Byte("mhfo.dll+503477A");

    public override int ArmorSkill17() => this.M.Read2Byte("mhfo.dll+503477C");

    public override int ArmorSkill18() => this.M.Read2Byte("mhfo.dll+503477E");

    public override int ArmorSkill19() => this.M.Read2Byte("mhfo.dll+5034780");

    public override int BloatedWeaponAttack() => this.M.Read2Byte("mhfo.dll+5BC68C8");

    public override int ZenithSkill1() => this.M.ReadByte("mhfo.dll+51C16D8");

    public override int ZenithSkill2() => this.M.ReadByte("mhfo.dll+51C16DA");

    public override int ZenithSkill3() => this.M.ReadByte("mhfo.dll+51C16DC");

    public override int ZenithSkill4() => this.M.ReadByte("mhfo.dll+51C16DE");

    public override int ZenithSkill5() => this.M.ReadByte("mhfo.dll+51C16E0");

    public override int ZenithSkill6() => this.M.ReadByte("mhfo.dll+51C16E2");

    public override int ZenithSkill7() => this.M.ReadByte("mhfo.dll+51C16E4");

    public override int AutomaticSkillWeapon() => this.M.Read2Byte("mhfo.dll+5034792");

    public override int AutomaticSkillHead() => this.M.Read2Byte("mhfo.dll+503478A");

    public override int AutomaticSkillChest() => this.M.Read2Byte("mhfo.dll+503478C");

    public override int AutomaticSkillArms() => this.M.Read2Byte("mhfo.dll+503478E");

    public override int AutomaticSkillWaist() => this.M.Read2Byte("mhfo.dll+5034790");

    public override int AutomaticSkillLegs() => this.M.Read2Byte("mhfo.dll+5034786");

    public override int StyleRank1() => this.M.ReadByte("mhfo.dll+50348D3");

    public override int StyleRank2() => this.M.ReadByte("mhfo.dll+503499F");

    public override int GRWeaponLv() => this.M.Read2Byte("mhfo.dll+5033F94");

    public override int GRWeaponLvBowguns() => this.M.Read2Byte("mhfo.dll+5033F95");

    public override int Sigil1Name1() => this.M.ReadByte("mhfo.dll+5BF91E4");

    public override int Sigil1Value1() => this.M.ReadByte("mhfo.dll+5BF91EA");

    public override int Sigil1Name2() => this.M.ReadByte("mhfo.dll+5BF91E6");

    public override int Sigil1Value2() => this.M.ReadByte("mhfo.dll+5BF91EC");

    public override int Sigil1Name3() => this.M.ReadByte("mhfo.dll+5BF91E8");

    public override int Sigil1Value3() => this.M.ReadByte("mhfo.dll+5BF91EE");

    public override int Sigil2Name1() => this.M.ReadByte("mhfo.dll+5BF91F0");

    public override int Sigil2Value1() => this.M.ReadByte("mhfo.dll+5BF91F6");

    public override int Sigil2Name2() => this.M.ReadByte("mhfo.dll+5BF91F2");

    public override int Sigil2Value2() => this.M.ReadByte("mhfo.dll+5BF91F8");

    public override int Sigil2Name3() => this.M.ReadByte("mhfo.dll+5BF91F4");

    public override int Sigil2Value3() => this.M.ReadByte("mhfo.dll+5BF91FA");

    public override int Sigil3Name1() => this.M.ReadByte("mhfo.dll+5BF9604");

    public override int Sigil3Value1() => this.M.ReadByte("mhfo.dll+5BF960A");

    public override int Sigil3Name2() => this.M.ReadByte("mhfo.dll+5BF9606");

    public override int Sigil3Value2() => this.M.ReadByte("mhfo.dll+5BF960C");

    public override int Sigil3Name3() => this.M.ReadByte("mhfo.dll+5BF9608");

    public override int Sigil3Value3() => this.M.ReadByte("mhfo.dll+5BF960E");

    public override int FelyneHunted() => this.M.Read2Byte("mhfo.dll+6103A1E");

    public override int MelynxHunted() => this.M.Read2Byte("mhfo.dll+6103A3A");

    public override int ShakalakaHunted() => this.M.Read2Byte("mhfo.dll+6103A7E") + 0;

    public override int VespoidHunted() => this.M.Read2Byte("mhfo.dll+6103A32");

    public override int HornetaurHunted() => this.M.Read2Byte("mhfo.dll+6103A3C");

    public override int GreatThunderbugHunted() => this.M.Read2Byte("mhfo.dll+6103A7C");

    public override int KelbiHunted() => this.M.Read2Byte("mhfo.dll+6103A12");

    public override int MosswineHunted() => this.M.Read2Byte("mhfo.dll+6103A14");

    public override int AntekaHunted() => this.M.Read2Byte("mhfo.dll+6103A96");

    public override int PopoHunted() => this.M.Read2Byte("mhfo.dll+6103A98");

    public override int AptonothHunted() => this.M.Read2Byte("mhfo.dll+6103A24");

    public override int ApcerosHunted() => this.M.Read2Byte("mhfo.dll+6103A3E");

    public override int BurukkuHunted() => this.M.Read2Byte("mhfo.dll+6103ACE");

    public override int ErupeHunted() => this.M.Read2Byte("mhfo.dll+6103AD0");

    public override int VelocipreyHunted() => this.M.Read2Byte("mhfo.dll+6103A2C");

    public override int VelocidromeHunted() => this.M.Read2Byte("mhfo.dll+6103A42");

    public override int GenpreyHunted() => this.M.Read2Byte("mhfo.dll+6103A26");

    public override int GendromeHunted() => this.M.Read2Byte("mhfo.dll+6103A44");

    public override int IopreyHunted() => this.M.Read2Byte("mhfo.dll+6103A48");

    public override int IodromeHunted() => this.M.Read2Byte("mhfo.dll+6103A4A");

    public override int GiapreyHunted() => this.M.Read2Byte("mhfo.dll+6103A52");

    public override int YianKutKuHunted() => this.M.Read2Byte("mhfo.dll+6103A18");

    public override int BlueYianKutKuHunted() => this.M.Read2Byte("mhfo.dll+6103A58");

    public override int YianGarugaHunted() => this.M.Read2Byte("mhfo.dll+6103A5C");

    public override int GypcerosHunted() => this.M.Read2Byte("mhfo.dll+6103A34");

    public override int PurpleGypcerosHunted() => this.M.Read2Byte("mhfo.dll+6103A5A");

    public override int HypnocHunted() => this.M.Read2Byte("mhfo.dll+6103AA0");

    public override int BrightHypnocHunted() => this.M.Read2Byte("mhfo.dll+6103AA8");

    public override int SilverHypnocHunted() => this.M.Read2Byte("mhfo.dll+6103AB0");

    public override int FarunokkuHunted() => this.M.Read2Byte("mhfo.dll+6103AF0");

    public override int ForokururuHunted() => this.M.Read2Byte("mhfo.dll+6103B06");

    public override int ToridclessHunted() => this.M.Read2Byte("mhfo.dll+6103B26");

    public override int RemobraHunted() => this.M.Read2Byte("mhfo.dll+6103A8A");

    public override int RathianHunted() => this.M.Read2Byte("mhfo.dll+6103A0E");

    public override int PinkRathianHunted() => this.M.Read2Byte("mhfo.dll+6103A56");

    public override int GoldRathianHunted() => this.M.Read2Byte("mhfo.dll+6103A60");

    public override int RathalosHunted() => this.M.Read2Byte("mhfo.dll+6103A22");

    public override int AzureRathalosHunted() => this.M.Read2Byte("mhfo.dll+6103A6E");

    public override int SilverRathalosHunted() => this.M.Read2Byte("mhfo.dll+6103A5E");

    public override int KhezuHunted() => this.M.Read2Byte("mhfo.dll+6103A2A");

    public override int RedKhezuHunted() => this.M.Read2Byte("mhfo.dll+6103A66");

    public override int BasariosHunted() => this.M.Read2Byte("mhfo.dll+6103A38");

    public override int GraviosHunted() => this.M.Read2Byte("mhfo.dll+6103A2E");

    public override int BlackGraviosHunted() => this.M.Read2Byte("mhfo.dll+6103A6A");

    public override int MonoblosHunted() => this.M.Read2Byte("mhfo.dll+6103A40");

    public override int WhiteMonoblosHunted() => this.M.Read2Byte("mhfo.dll+6103A64");

    public override int DiablosHunted() => this.M.Read2Byte("mhfo.dll+6103A28");

    public override int BlackDiablosHunted() => this.M.Read2Byte("mhfo.dll+6103A62");

    public override int TigrexHunted() => this.M.Read2Byte("mhfo.dll+6103AA4");

    public override int EspinasHunted() => this.M.Read2Byte("mhfo.dll+6103AAC");

    public override int OrangeEspinasHunted() => this.M.Read2Byte("mhfo.dll+6103AAE");

    public override int WhiteEspinasHunted() => this.M.Read2Byte("mhfo.dll+6103AC0");

    public override int AkantorHunted() => this.M.Read2Byte("mhfo.dll+6103AA6");

    public override int BerukyurosuHunted() => this.M.Read2Byte("mhfo.dll+6103AB6");

    public override int DoragyurosuHunted() => this.M.Read2Byte("mhfo.dll+6103ACA");

    public override int PariapuriaHunted() => this.M.Read2Byte("mhfo.dll+6103ABE");

    public override int DyuragauaHunted() => this.M.Read2Byte("mhfo.dll+6103AC8");

    public override int GurenzeburuHunted() => this.M.Read2Byte("mhfo.dll+6103ACC");

    public override int OdibatorasuHunted() => this.M.Read2Byte("mhfo.dll+6103AE0");

    public override int HyujikikiHunted() => this.M.Read2Byte("mhfo.dll+6103AE8");

    public override int AnorupatisuHunted() => this.M.Read2Byte("mhfo.dll+6103AE6");

    public override int ZerureusuHunted() => this.M.Read2Byte("mhfo.dll+6103B00") + 0;

    public override int MeraginasuHunted() => this.M.Read2Byte("mhfo.dll+6103B08");

    public override int DiorexHunted() => this.M.Read2Byte("mhfo.dll+6103B0A");

    public override int PoborubarumuHunted() => this.M.Read2Byte("mhfo.dll+6103B12");

    public override int VarusaburosuHunted() => this.M.Read2Byte("mhfo.dll+6103B10");

    public override int GureadomosuHunted() => this.M.Read2Byte("mhfo.dll+6103B22");

    public override int BariothHunted() => this.M.Read2Byte("mhfo.dll+6103B3A");

    // musous are separate???
    public override int NargacugaHunted() => this.M.Read2Byte("mhfo.dll+6103B4A") + 0;

    public override int ZenaserisuHunted() => this.M.Read2Byte("mhfo.dll+6103B4E");

    public override int SeregiosHunted() => this.M.Read2Byte("mhfo.dll+6103B5E");

    public override int BogabadorumuHunted() => this.M.Read2Byte("mhfo.dll+6103B60");

    public override int CephalosHunted() => this.M.Read2Byte("mhfo.dll+6103A50");

    public override int CephadromeHunted() => this.M.Read2Byte("mhfo.dll+6103A1C");

    public override int PlesiothHunted() => this.M.Read2Byte("mhfo.dll+6103A36");

    public override int GreenPlesiothHunted() => this.M.Read2Byte("mhfo.dll+6103A68");

    public override int VolganosHunted() => this.M.Read2Byte("mhfo.dll+6103AA2");

    public override int RedVolganosHunted() => this.M.Read2Byte("mhfo.dll+6103AAA");

    public override int HermitaurHunted() => this.M.Read2Byte("mhfo.dll+6103A90");

    public override int DaimyoHermitaurHunted() => this.M.Read2Byte("mhfo.dll+6103A6C");

    public override int CeanataurHunted() => this.M.Read2Byte("mhfo.dll+6103A9E");

    public override int ShogunCeanataurHunted() => this.M.Read2Byte("mhfo.dll+6103A92");

    public override int ShenGaorenHunted() => this.M.Read2Byte("mhfo.dll+6103A7A");

    public override int AkuraVashimuHunted() => this.M.Read2Byte("mhfo.dll+6103AB2");

    public override int AkuraJebiaHunted() => this.M.Read2Byte("mhfo.dll+6103AB4");

    public override int TaikunZamuzaHunted() => this.M.Read2Byte("mhfo.dll+6103ADA");

    public override int KusubamiHunted() => this.M.Read2Byte("mhfo.dll+6103B2A");

    public override int BullfangoHunted() => this.M.Read2Byte("mhfo.dll+6103A16");

    public override int BulldromeHunted() => this.M.Read2Byte("mhfo.dll+6103A94");

    public override int CongaHunted() => this.M.Read2Byte("mhfo.dll+6103A88");

    public override int CongalalaHunted() => this.M.Read2Byte("mhfo.dll+6103A74");

    public override int BlangoHunted() => this.M.Read2Byte("mhfo.dll+6103A86");

    public override int BlangongaHunted() => this.M.Read2Byte("mhfo.dll+6103A72");

    public override int GogomoaHunted() => this.M.Read2Byte("mhfo.dll+6103AD6");

    public override int RajangHunted() => this.M.Read2Byte("mhfo.dll+6103A76");

    public override int KamuOrugaronHunted() => this.M.Read2Byte("mhfo.dll+6103AC2");

    public override int NonoOrugaronHunted() => this.M.Read2Byte("mhfo.dll+6103AC4");

    public override int MidogaronHunted() => this.M.Read2Byte("mhfo.dll+6103AEA");

    public override int GougarfHunted() => this.M.Read2Byte("mhfo.dll+6103B02");

    public override int VoljangHunted() => this.M.Read2Byte("mhfo.dll+6103B48");

    public override int KirinHunted() => this.M.Read2Byte("mhfo.dll+6103A4E");

    public override int KushalaDaoraHunted() => this.M.Read2Byte("mhfo.dll+6103A78");

    public override int RustedKushalaDaoraHunted() => this.M.Read2Byte("mhfo.dll+6103A84");

    public override int ChameleosHunted() => this.M.Read2Byte("mhfo.dll+6103A82");

    public override int LunastraHunted() => this.M.Read2Byte("mhfo.dll+6103A8C");

    public override int TeostraHunted() => this.M.Read2Byte("mhfo.dll+6103A8E");

    public override int LaoShanLungHunted() => this.M.Read2Byte("mhfo.dll+6103A1A");

    public override int AshenLaoShanLungHunted() => this.M.Read2Byte("mhfo.dll+6103A70");

    // untested
    public override int YamaTsukamiHunted() => this.M.Read2Byte("mhfo.dll+6103A80");

    public override int RukodioraHunted() => this.M.Read2Byte("mhfo.dll+6103AD2");

    public override int RebidioraHunted() => this.M.Read2Byte("mhfo.dll+6103AE4");

    public override int FatalisHunted() => this.M.Read2Byte("mhfo.dll+6103A10");

    public override int ShantienHunted() => this.M.Read2Byte("mhfo.dll+6103AF4");

    public override int DisufiroaHunted() => this.M.Read2Byte("mhfo.dll+6103AE2");

    public override int GarubaDaoraHunted() => this.M.Read2Byte("mhfo.dll+6103B0C");

    public override int InagamiHunted() => this.M.Read2Byte("mhfo.dll+6103B0E");

    public override int HarudomeruguHunted() => this.M.Read2Byte("mhfo.dll+6103B24");

    public override int YamaKuraiHunted() => this.M.Read2Byte("mhfo.dll+6103B2C");

    public override int ToaTesukatoraHunted() => this.M.Read2Byte("mhfo.dll+6103B38");

    public override int GuanzorumuHunted() => this.M.Read2Byte("mhfo.dll+6103B40");

    public override int KeoaruboruHunted() => this.M.Read2Byte("mhfo.dll+6103B4C");

    public override int ShagaruMagalaHunted() => this.M.Read2Byte("mhfo.dll+6103B54");

    public override int ElzelionHunted() => this.M.Read2Byte("mhfo.dll+6103B58");

    public override int AmatsuHunted() => this.M.Read2Byte("mhfo.dll+6103B56");

    public override int AbioruguHunted() => this.M.Read2Byte("mhfo.dll+6103ADC");

    public override int GiaoruguHunted() => this.M.Read2Byte("mhfo.dll+6103AEC");

    public override int GasurabazuraHunted() => this.M.Read2Byte("mhfo.dll+6103B28");

    public override int DeviljhoHunted() => this.M.Read2Byte("mhfo.dll+6103B32");

    public override int BrachydiosHunted() => this.M.Read2Byte("mhfo.dll+6103B34");

    public override int UragaanHunted() => this.M.Read2Byte("mhfo.dll+6103B3C");

    public override int KuarusepusuHunted() => this.M.Read2Byte("mhfo.dll+6103ADE");

    public override int PokaraHunted() => this.M.Read2Byte("mhfo.dll+6103AF6");

    public override int PokaradonHunted() => this.M.Read2Byte("mhfo.dll+6103AF2");

    public override int BaruragaruHunted() => this.M.Read2Byte("mhfo.dll+6103AFE");

    public override int ZinogreHunted() => this.M.Read2Byte("mhfo.dll+6103B30");

    public override int StygianZinogreHunted() => this.M.Read2Byte("mhfo.dll+6103B3E");

    public override int GoreMagalaHunted() => this.M.Read2Byte("mhfo.dll+6103B50");

    public override int BlitzkriegBogabadorumuHunted() => this.M.Read2Byte("mhfo.dll+6103B64");

    public override int SparklingZerureusuHunted() => this.M.Read2Byte("mhfo.dll+6103B68");

    public override int StarvingDeviljhoHunted() => this.M.Read2Byte("mhfo.dll+6103B42");

    public override int CrimsonFatalisHunted() => this.M.Read2Byte("mhfo.dll+6103A54");

    public override int WhiteFatalisHunted() => this.M.Read2Byte("mhfo.dll+6103A9A");

    public override int CactusHunted() => this.M.Read2Byte("mhfo.dll+6103AB8");

    public override int ArrogantDuremudiraHunted() => this.M.Read2Byte("mhfo.dll+6103B5A");

    // untested
    public override int KingShakalakaHunted() => this.M.Read2Byte("mhfo.dll+6103B6C");

    public override int MiRuHunted() => this.M.Read2Byte("mhfo.dll+6103AEE");

    public override int UnknownHunted() => this.M.Read2Byte("mhfo.dll+6103AD4");

    public override int GoruganosuHunted() => this.M.Read2Byte("mhfo.dll+6103AFA");

    public override int AruganosuHunted() => this.M.Read2Byte("mhfo.dll+6103AFC");

    public override int PSO2RappyHunted() => this.M.Read2Byte("mhfo.dll+6103B6A");

    public override int RocksHunted() => this.M.Read2Byte("mhfo.dll+6103A46");

    public override int UrukiHunted() => this.M.Read2Byte("mhfo.dll+6103B04");

    public override int GorgeObjectsHunted() => this.M.Read2Byte("mhfo.dll+6103ABA");

    public override int BlinkingNargacugaHunted() => this.M.Read2Byte("mhfo.dll+6103B52");

    public override int QuestState() => this.M.ReadByte("mhfo.dll+61180F2");

    public override int RoadDureSkill1Name() => this.M.ReadByte("mhfo.dll+610403C");

    public override int RoadDureSkill1Level() => this.M.ReadByte("mhfo.dll+610403E");

    public override int RoadDureSkill2Name() => this.M.ReadByte("mhfo.dll+6104040");

    public override int RoadDureSkill2Level() => this.M.ReadByte("mhfo.dll+6104042");

    public override int RoadDureSkill3Name() => this.M.ReadByte("mhfo.dll+6104044");

    public override int RoadDureSkill3Level() => this.M.ReadByte("mhfo.dll+6104046");

    public override int RoadDureSkill4Name() => this.M.ReadByte("mhfo.dll+6104048");

    public override int RoadDureSkill4Level() => this.M.ReadByte("mhfo.dll+610404A");

    public override int RoadDureSkill5Name() => this.M.ReadByte("mhfo.dll+610404C");

    public override int RoadDureSkill5Level() => this.M.ReadByte("mhfo.dll+610404E");

    public override int RoadDureSkill6Name() => this.M.ReadByte("mhfo.dll+6104050");

    public override int RoadDureSkill6Level() => this.M.ReadByte("mhfo.dll+6104052");

    public override int RoadDureSkill7Name() => this.M.ReadByte("mhfo.dll+6104054");

    public override int RoadDureSkill7Level() => this.M.ReadByte("mhfo.dll+6104056");

    public override int RoadDureSkill8Name() => this.M.ReadByte("mhfo.dll+6104058");

    public override int RoadDureSkill8Level() => this.M.ReadByte("mhfo.dll+610405A");

    public override int RoadDureSkill9Name() => this.M.ReadByte("mhfo.dll+610405C");

    public override int RoadDureSkill9Level() => this.M.ReadByte("mhfo.dll+610405E");

    public override int RoadDureSkill10Name() => this.M.ReadByte("mhfo.dll+6104060");

    public override int RoadDureSkill10Level() => this.M.ReadByte("mhfo.dll+6104062");

    public override int RoadDureSkill11Name() => this.M.ReadByte("mhfo.dll+6104064");

    public override int RoadDureSkill11Level() => this.M.ReadByte("mhfo.dll+6104066");

    public override int RoadDureSkill12Name() => this.M.ReadByte("mhfo.dll+6104068");

    public override int RoadDureSkill12Level() => this.M.ReadByte("mhfo.dll+610406A");

    public override int RoadDureSkill13Name() => this.M.ReadByte("mhfo.dll+610406C");

    public override int RoadDureSkill13Level() => this.M.ReadByte("mhfo.dll+610406E");

    public override int RoadDureSkill14Name() => this.M.ReadByte("mhfo.dll+6104070");

    public override int RoadDureSkill14Level() => this.M.ReadByte("mhfo.dll+6104072");

    public override int RoadDureSkill15Name() => this.M.ReadByte("mhfo.dll+6104074");

    public override int RoadDureSkill15Level() => this.M.ReadByte("mhfo.dll+6104076");

    public override int RoadDureSkill16Name() => this.M.ReadByte("mhfo.dll+6104078");

    public override int RoadDureSkill16Level() => this.M.ReadByte("mhfo.dll+610407A");

    public override int PartySize() => this.M.ReadByte("mhfo.dll+57967C8");

    public override int PartySizeMax() => this.M.ReadByte("mhfo.dll+61B6088");

    public override uint GSRP() => (uint)this.M.ReadInt("mhfo.dll+61041C8");

    public override uint GRP() => (uint)this.M.ReadInt("mhfo.dll+5BC82F8");

    public override int HunterHP() => this.M.Read2Byte("mhfo.dll+5BC6548");

    public override int HunterStamina() => this.M.Read2Byte("mhfo.dll+503438C");

    public override int QuestItemsUsed() => this.M.Read2Byte("mhfo.dll+57EAE24");

    public override int AreaHitsTakenBlocked() => this.M.Read2Byte("mhfo.dll+5034078");

    // TODO Untested
    public override int PartnyaBagItem1ID() => this.M.Read2Byte("mhfo.dll+5745788");

    public override int PartnyaBagItem1Qty() => this.M.Read2Byte("mhfo.dll+574578A");

    public override int PartnyaBagItem2ID() => this.M.Read2Byte("mhfo.dll+574578C");

    public override int PartnyaBagItem2Qty() => this.M.Read2Byte("mhfo.dll+574578E");

    public override int PartnyaBagItem3ID() => this.M.Read2Byte("mhfo.dll+5745790");

    public override int PartnyaBagItem3Qty() => this.M.Read2Byte("mhfo.dll+5745792");

    public override int PartnyaBagItem4ID() => this.M.Read2Byte("mhfo.dll+5745794");

    public override int PartnyaBagItem4Qty() => this.M.Read2Byte("mhfo.dll+5745796");

    public override int PartnyaBagItem5ID() => this.M.Read2Byte("mhfo.dll+5745798");

    public override int PartnyaBagItem5Qty() => this.M.Read2Byte("mhfo.dll+574579A");

    public override int PartnyaBagItem6ID() => this.M.Read2Byte("mhfo.dll+574579C");

    public override int PartnyaBagItem6Qty() => this.M.Read2Byte("mhfo.dll+574579E");

    public override int PartnyaBagItem7ID() => this.M.Read2Byte("mhfo.dll+57457A0");

    public override int PartnyaBagItem7Qty() => this.M.Read2Byte("mhfo.dll+57457A2");

    public override int PartnyaBagItem8ID() => this.M.Read2Byte("mhfo.dll+57457A4");

    public override int PartnyaBagItem8Qty() => this.M.Read2Byte("mhfo.dll+57457A6");

    public override int PartnyaBagItem9ID() => this.M.Read2Byte("mhfo.dll+57457A8");

    public override int PartnyaBagItem9Qty() => this.M.Read2Byte("mhfo.dll+57457AA");

    public override int PartnyaBagItem10ID() => this.M.Read2Byte("mhfo.dll+57457AC");

    public override int PartnyaBagItem10Qty() => this.M.Read2Byte("mhfo.dll+57457AE");
}
