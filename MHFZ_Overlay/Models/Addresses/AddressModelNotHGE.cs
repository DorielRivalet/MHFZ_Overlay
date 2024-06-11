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
public sealed class AddressModelNotHGE : AddressModel
{
    public AddressModelNotHGE(Mem m)
        : base(m)
    {
        // empty
    }

    public string Monster1BP1 => this.M.Read2Byte("mhfo.dll+60A3E58,348").ToString(CultureInfo.InvariantCulture);

    public string Monster1BP2 => this.M.Read2Byte("mhfo.dll+60A3E58,350").ToString(CultureInfo.InvariantCulture);

    public string Monster1BP3 => this.M.Read2Byte("mhfo.dll+60A3E58,358").ToString(CultureInfo.InvariantCulture);

    public string Monster1BP4 => this.M.Read2Byte("mhfo.dll+60A3E58,360").ToString(CultureInfo.InvariantCulture);

    public string Monster1BP5 => this.M.Read2Byte("mhfo.dll+60A3E58,368").ToString(CultureInfo.InvariantCulture);

    public string Monster1BP6 => this.M.Read2Byte("mhfo.dll+60A3E58,370").ToString(CultureInfo.InvariantCulture);

    public string Monster1BP7 => this.M.Read2Byte("mhfo.dll+60A3E58,378").ToString(CultureInfo.InvariantCulture);

    public string Monster1BP8 => this.M.Read2Byte("mhfo.dll+60A3E58,380").ToString(CultureInfo.InvariantCulture);

    public string Monster1BP9 => this.M.Read2Byte("mhfo.dll+60A3E58,388").ToString(CultureInfo.InvariantCulture);

    public string Monster1BP10 => this.M.Read2Byte("mhfo.dll+60A3E58,390").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP1 => this.M.Read2Byte("mhfo.dll+60A3E58,1238").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP2 => this.M.Read2Byte("mhfo.dll+60A3E58,1240").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP3 => this.M.Read2Byte("mhfo.dll+60A3E58,1248").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP4 => this.M.Read2Byte("mhfo.dll+60A3E58,1250").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP5 => this.M.Read2Byte("mhfo.dll+60A3E58,1258").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP6 => this.M.Read2Byte("mhfo.dll+60A3E58,1260").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP7 => this.M.Read2Byte("mhfo.dll+60A3E58,1268").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP8 => this.M.Read2Byte("mhfo.dll+60A3E58,1270").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP9 => this.M.Read2Byte("mhfo.dll+60A3E58,1278").ToString(CultureInfo.InvariantCulture);

    public string Monster2BP10 => this.M.Read2Byte("mhfo.dll+60A3E58,1280").ToString(CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public override int HitCountInt() => this.M.Read2Byte("mhfo.dll+60792E6");

    // public override int TimeDefInt() => this.M.ReadInt("mhfo.dll+1B97780");

    /// <inheritdoc/>
    public override int TimeDefInt() => this.M.ReadInt("mhfo.dll+28C2C70");

    /// <inheritdoc/>
    public override int TimeInt() => this.M.ReadInt("mhfo.dll+5BC6540");

    // alternative timeint for dure? mhfo.dll+5BC7600

    /// <inheritdoc/>
    public override int WeaponRaw() => this.M.Read2Byte("mhfo.dll+503433A");

    // This is equipment slot number that goes from 0-255 repeatedly
    // "mhfo.dll+60FFCC6
    // "mhfo.dll+B7FF45
    // "mhfo.dll+182D3B93
    // "mhfo.dll+13E1FF45

    /// <inheritdoc/>
    public override int WeaponType() => this.M.ReadByte("mhfo.dll+5033B93");

    /// <inheritdoc/>
    public override bool IsNotRoad() => this.M.ReadByte("mhfo.dll+509C8B0") == 0;

    /// <inheritdoc/>
    public override int LargeMonster1ID() => this.GetNotRoad() ? this.M.ReadByte("mhfo.dll+1B97794") : this.LargeMonster1Road();

    /// <inheritdoc/>
    public override int LargeMonster2ID() => this.GetNotRoad() ? this.M.ReadByte("mhfo.dll+1B9779C") : this.LargeMonster2Road();

    /// <inheritdoc/>
    public override int LargeMonster3ID() => this.M.ReadByte("mhfo.dll+1B977A4");

    /// <inheritdoc/>
    public override int LargeMonster4ID() => this.M.ReadByte("mhfo.dll+1B977AC");

    public int LargeMonster1Road() => this.M.ReadByte("mhfo.dll+509C8B8");

    public int LargeMonster2Road() => this.M.ReadByte("mhfo.dll+509C8D8");

    /// <inheritdoc/>
    public override int Monster1Part1() => this.M.Read2Byte("mhfo.dll+60A3E58,348");

    /// <inheritdoc/>
    public override int Monster1Part2() => this.M.Read2Byte("mhfo.dll+60A3E58,350");

    /// <inheritdoc/>
    public override int Monster1Part3() => this.M.Read2Byte("mhfo.dll+60A3E58,358");

    /// <inheritdoc/>
    public override int Monster1Part4() => this.M.Read2Byte("mhfo.dll+60A3E58,360");

    /// <inheritdoc/>
    public override int Monster1Part5() => this.M.Read2Byte("mhfo.dll+60A3E58,368");

    /// <inheritdoc/>
    public override int Monster1Part6() => this.M.Read2Byte("mhfo.dll+60A3E58,370");

    /// <inheritdoc/>
    public override int Monster1Part7() => this.M.Read2Byte("mhfo.dll+60A3E58,378");

    /// <inheritdoc/>
    public override int Monster1Part8() => this.M.Read2Byte("mhfo.dll+60A3E58,380");

    /// <inheritdoc/>
    public override int Monster1Part9() => this.M.Read2Byte("mhfo.dll+60A3E58,388");

    /// <inheritdoc/>
    public override int Monster1Part10() => this.M.Read2Byte("mhfo.dll+60A3E58,390");

    /// <inheritdoc/>
    public override int Monster2Part1() => this.M.Read2Byte("mhfo.dll+60A3E58,1238");

    /// <inheritdoc/>
    public override int Monster2Part2() => this.M.Read2Byte("mhfo.dll+60A3E58,1240");

    /// <inheritdoc/>
    public override int Monster2Part3() => this.M.Read2Byte("mhfo.dll+60A3E58,1248");

    /// <inheritdoc/>
    public override int Monster2Part4() => this.M.Read2Byte("mhfo.dll+60A3E58,1250");

    /// <inheritdoc/>
    public override int Monster2Part5() => this.M.Read2Byte("mhfo.dll+60A3E58,1258");

    /// <inheritdoc/>
    public override int Monster2Part6() => this.M.Read2Byte("mhfo.dll+60A3E58,1260");

    /// <inheritdoc/>
    public override int Monster2Part7() => this.M.Read2Byte("mhfo.dll+60A3E58,1268");

    /// <inheritdoc/>
    public override int Monster2Part8() => this.M.Read2Byte("mhfo.dll+60A3E58,1270");

    /// <inheritdoc/>
    public override int Monster2Part9() => this.M.Read2Byte("mhfo.dll+60A3E58,1278");

    /// <inheritdoc/>
    public override int Monster2Part10() => this.M.Read2Byte("mhfo.dll+60A3E58,1280");

    /// <inheritdoc/>
    public override int Monster1HPInt() => this.M.Read2Byte("0043C600");

    /// <inheritdoc/>
    public override int Monster2HPInt() => this.M.Read2Byte("0043C604");

    /// <inheritdoc/>
    public override int Monster3HPInt() => this.M.Read2Byte("0043C608");

    /// <inheritdoc/>
    public override int Monster4HPInt() => this.M.Read2Byte("0043C60C");

    /// <inheritdoc/>
    public override string Monster1AtkMult() => this.M.ReadFloat("mhfo.dll+60A3E58,898").ToString(CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public override decimal Monster1DefMult() => (decimal)this.M.ReadFloat("mhfo.dll+60A3E58,89C", string.Empty, false);

    /// <inheritdoc/>
    public override int Monster1Poison() => this.M.Read2Byte("mhfo.dll+60A3E58,88A");

    /// <inheritdoc/>
    public override int Monster1PoisonNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,888");

    /// <inheritdoc/>
    public override int Monster1Sleep() => this.M.Read2Byte("mhfo.dll+60A3E58,86C");

    /// <inheritdoc/>
    public override int Monster1SleepNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,86A");

    /// <inheritdoc/>
    public override int Monster1Para() => this.M.Read2Byte("mhfo.dll+60A3E58,886");

    /// <inheritdoc/>
    public override int Monster1ParaNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,880");

    /// <inheritdoc/>
    public override int Monster1Blast() => this.M.Read2Byte("mhfo.dll+60A3E58,D4A");

    /// <inheritdoc/>
    public override int Monster1BlastNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,D48");

    /// <inheritdoc/>
    public override int Monster1Stun() => this.M.Read2Byte("mhfo.dll+60A3E58,872");

    /// <inheritdoc/>
    public override int Monster1StunNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,A74");

    /// <inheritdoc/>
    public override string Monster1Size() => this.M.Read2Byte("mhfo.dll+28C2BD4").ToString(CultureInfo.InvariantCulture) + "%";

    /// <inheritdoc/>
    public override string Monster2AtkMult() => this.M.ReadFloat("mhfo.dll+60A3E58,1788").ToString(CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public override decimal Monster2DefMult() => (decimal)this.M.ReadFloat("mhfo.dll+60A3E58,178C", string.Empty, false);

    /// <inheritdoc/>
    public override int Monster2Poison() => this.M.Read2Byte("mhfo.dll+60A3E58,177A");

    /// <inheritdoc/>
    public override int Monster2PoisonNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,1778");

    /// <inheritdoc/>
    public override int Monster2Sleep() => this.M.Read2Byte("mhfo.dll+60A3E58,175C");

    /// <inheritdoc/>
    public override int Monster2SleepNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,175A");

    /// <inheritdoc/>
    public override int Monster2Para() => this.M.Read2Byte("mhfo.dll+60A3E58,1776");

    /// <inheritdoc/>
    public override int Monster2ParaNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,1770");

    /// <inheritdoc/>
    public override int Monster2Blast() => this.M.Read2Byte("mhfo.dll+60A3E58,1C3A");

    /// <inheritdoc/>
    public override int Monster2BlastNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,1C38");

    /// <inheritdoc/>
    public override int Monster2Stun() => this.M.Read2Byte("mhfo.dll+60A3E58,1762");

    /// <inheritdoc/>
    public override int Monster2StunNeed() => this.M.Read2Byte("mhfo.dll+60A3E58,1964");

    /// <inheritdoc/>
    public override string Monster2Size() => this.M.Read2Byte("mhfo.dll+2AFA784").ToString(CultureInfo.InvariantCulture) + "%";

    /// <inheritdoc/>
    public override int DamageDealt() => this.M.Read2Byte("mhfo.dll+5CA3430");

    /// <inheritdoc/>
    public override int RoadSelectedMonster() => this.M.ReadByte("mhfo.dll+001B48F4,4");

    // new addresses

    /// <inheritdoc/>
    public override int AreaID() => this.M.Read2Byte("mhfo.dll+5034388");

    /// <inheritdoc/>
    public override int RavienteAreaID() => this.M.Read2Byte("mhfo.dll+6124B6E");

    /// <inheritdoc/>
    public override int GRankNumber() => this.M.Read2Byte("mhfo.dll+613DD30");

    /// <inheritdoc/>
    public override int GSR() => this.M.Read2Byte("mhfo.dll+50349A2");

    /// <inheritdoc/>
    public override int RoadFloor() => this.M.Read2Byte("mhfo.dll+5C47600");

    /// <inheritdoc/>
    public override int WeaponStyle() => this.M.ReadByte("mhfo.dll+50348D2");

    /// <inheritdoc/>
    public override int QuestID() => this.M.Read2Byte("mhfo.dll+5FB4A4C");

    /// <inheritdoc/>
    public override int UrukiPachinkoFish() => this.M.ReadByte("mhfo.dll+61EC176");

    /// <inheritdoc/>
    public override int UrukiPachinkoMushroom() => this.M.ReadByte("mhfo.dll+61EC178");

    /// <inheritdoc/>
    public override int UrukiPachinkoSeed() => this.M.ReadByte("mhfo.dll+61EC17A");

    /// <inheritdoc/>
    public override int UrukiPachinkoMeat() => this.M.ReadByte("mhfo.dll+61EC174");

    /// <inheritdoc/>
    public override int UrukiPachinkoChain() => this.M.ReadByte("mhfo.dll+61EC160");

    /// <inheritdoc/>
    public override int UrukiPachinkoScore() => this.M.ReadInt("mhfo.dll+61EC16C");

    /// <inheritdoc/>
    public override int UrukiPachinkoBonusScore() => this.M.ReadInt("mhfo.dll+61EC170");

    /// <inheritdoc/>
    public override int NyanrendoScore() => this.M.ReadInt("mhfo.dll+61EC160");

    /// <inheritdoc/>
    public override int DokkanBattleCatsScore() => this.M.ReadInt("mhfo.dll+61EC158");

    /// <inheritdoc/>
    public override int DokkanBattleCatsScale() => this.M.ReadByte("mhfo.dll+61EC2EC");

    /// <inheritdoc/>
    public override int DokkanBattleCatsShell() => this.M.ReadByte("mhfo.dll+61EC2EE");

    /// <inheritdoc/>
    public override int DokkanBattleCatsCamp() => this.M.ReadByte("mhfo.dll+61EC2EA");

    /// <inheritdoc/>
    public override int GuukuScoopSmall() => this.M.ReadByte("mhfo.dll+61EC190");

    /// <inheritdoc/>
    public override int GuukuScoopMedium() => this.M.ReadByte("mhfo.dll+61EC194");

    /// <inheritdoc/>
    public override int GuukuScoopLarge() => this.M.ReadByte("mhfo.dll+61EC198");

    /// <inheritdoc/>
    public override int GuukuScoopGolden() => this.M.ReadByte("mhfo.dll+61EC19C");

    /// <inheritdoc/>
    public override int GuukuScoopScore() => this.M.ReadInt("mhfo.dll+61EC184");

    /// <inheritdoc/>
    public override int PanicHoneyScore() => this.M.ReadByte("mhfo.dll+61EC168");

    /// <inheritdoc/>
    public override int Sharpness() => this.M.Read2Byte("mhfo.dll+50346B6");

    /// <inheritdoc/>
    public override int CaravanPoints() => this.M.ReadInt("mhfo.dll+6101894");

    /// <inheritdoc/>
    public override int MezeportaFestivalPoints() => this.M.ReadInt("mhfo.dll+617FA4C");

    /// <inheritdoc/>
    public override int DivaBond() => this.M.Read2Byte("mhfo.dll+61033A8");

    /// <inheritdoc/>
    public override int DivaItemsGiven() => this.M.Read2Byte("mhfo.dll+61033AA");

    /// <inheritdoc/>
    public override int GCP() => this.M.ReadInt("mhfo.dll+58CFA18");

    /// <inheritdoc/>
    public override int RoadPoints() => this.M.ReadInt("mhfo.dll+61043F8");

    /// <inheritdoc/>
    public override int ArmorColor() => this.M.ReadByte("mhfo.dll+6100476");

    /// <inheritdoc/>
    public override int RaviGg() => this.M.ReadInt("mhfo.dll+6104188");

    /// <inheritdoc/>
    public override int Ravig() => this.M.ReadInt("mhfo.dll+61003A0");

    /// <inheritdoc/>
    public override int GZenny() => this.M.ReadInt("mhfo.dll+6100514");

    ///// <inheritdoc/>
    //public override int GuildFoodSkill() => this.M.Read2Byte("mhfo.dll+5BC70D8");

    /// <inheritdoc/>
    public override int GuildFoodSkill() => this.M.Read2Byte("mhfo.dll+5A951DE");

    /// <inheritdoc/>
    public override int GalleryEvaluationScore() => this.M.ReadInt("mhfo.dll+6103250");

    /// <inheritdoc/>
    public override int PoogiePoints() => this.M.ReadByte("mhfo.dll+6100350");

    /// <inheritdoc/>
    public override int PoogieItemUseID() => this.M.Read2Byte("mhfo.dll+61540F8");

    /// <inheritdoc/>
    public override int PoogieCostume() => this.M.ReadByte("mhfo.dll+1A88392");

    // zero-indexed

    /// <inheritdoc/>
    public override int CaravenGemLevel() => this.M.ReadByte("mhfo.dll+610037D");

    /// <inheritdoc/>
    public override int RoadMaxStagesMultiplayer() => this.M.Read2Byte("mhfo.dll+5C47688");

    /// <inheritdoc/>
    public override int RoadTotalStagesMultiplayer() => this.M.Read2Byte("mhfo.dll+5C47668");

    /// <inheritdoc/>
    public override int RoadTotalStagesSolo() => this.M.Read2Byte("mhfo.dll+5C4766C");

    /// <inheritdoc/>
    public override int RoadMaxStagesSolo() => this.M.Read2Byte("mhfo.dll+5C47690");

    /// <inheritdoc/>
    public override int RoadFatalisSlain() => this.M.Read2Byte("mhfo.dll+5C47670");

    /// <inheritdoc/>
    public override int RoadFatalisEncounters() => this.M.Read2Byte("mhfo.dll+5C47698");

    /// <inheritdoc/>
    public override int FirstDistrictDuremudiraEncounters() => this.M.Read2Byte("mhfo.dll+6104414");

    /// <inheritdoc/>
    public override int FirstDistrictDuremudiraSlays() => this.M.Read2Byte("mhfo.dll+6104FCC");

    /// <inheritdoc/>
    public override int SecondDistrictDuremudiraEncounters() => this.M.Read2Byte("mhfo.dll+6104418");

    /// <inheritdoc/>
    public override int SecondDistrictDuremudiraSlays() => this.M.Read2Byte("mhfo.dll+5C47678");

    /// <inheritdoc/>
    public override int DeliveryQuestPoints() => this.M.Read2Byte("mhfo.dll+6100A72");

    // red is 0

    /// <inheritdoc/>
    public override int SharpnessLevel() => this.M.ReadByte("mhfo.dll+50346BF");

    /// <inheritdoc/>
    public override int PartnerLevel() => this.M.Read2Byte("mhfo.dll+574127E");

    // as hex, consult addresses README.md

    /// <inheritdoc/>
    public override int ObjectiveType() => this.M.ReadInt("mhfo.dll+28C2C80");

    /// <inheritdoc/>
    public override int DivaSkillUsesLeft() => this.M.ReadByte("mhfo.dll+610436A");

    /// <inheritdoc/>
    public override int HalkFullness() => this.M.ReadByte("mhfo.dll+6101983");

    /// <inheritdoc/>
    public override int HalkLevel() => this.M.ReadByte("mhfo.dll+6101984");

    /// <inheritdoc/>
    public override int HalkIntimacy() => this.M.ReadByte("mhfo.dll+6101985");

    /// <inheritdoc/>
    public override int HalkHealth() => this.M.ReadByte("mhfo.dll+6101986");

    /// <inheritdoc/>
    public override int HalkAttack() => this.M.ReadByte("mhfo.dll+6101987");

    /// <inheritdoc/>
    public override int HalkDefense() => this.M.ReadByte("mhfo.dll+6101988");

    /// <inheritdoc/>
    public override int HalkIntellect() => this.M.ReadByte("mhfo.dll+6101989");

    /// <inheritdoc/>
    public override int HalkSkill1() => this.M.ReadByte("mhfo.dll+610198A");

    /// <inheritdoc/>
    public override int HalkSkill2() => this.M.ReadByte("mhfo.dll+610198B");

    /// <inheritdoc/>
    public override int HalkSkill3() => this.M.ReadByte("mhfo.dll+610198C");

    /// <inheritdoc/>
    public override int HalkElementNone() => this.M.ReadByte("mhfo.dll+610198E");

    /// <inheritdoc/>
    public override int HalkFire() => this.M.ReadByte("mhfo.dll+610198F");

    /// <inheritdoc/>
    public override int HalkThunder() => this.M.ReadByte("mhfo.dll+6101990");

    /// <inheritdoc/>
    public override int HalkWater() => this.M.ReadByte("mhfo.dll+6101991");

    /// <inheritdoc/>
    public override int HalkIce() => this.M.ReadByte("mhfo.dll+6101992");

    /// <inheritdoc/>
    public override int HalkDragon() => this.M.ReadByte("mhfo.dll+6101993");

    /// <inheritdoc/>
    public override int HalkSleep() => this.M.ReadByte("mhfo.dll+6101994");

    /// <inheritdoc/>
    public override int HalkParalysis() => this.M.ReadByte("mhfo.dll+6101995");

    /// <inheritdoc/>
    public override int HalkPoison() => this.M.ReadByte("mhfo.dll+6101996");

    /// <inheritdoc/>
    public override int RankBand() => this.M.ReadByte("mhfo.dll+28C2BD8");

    /// <inheritdoc/>
    public override int PartnyaRankPoints() => this.M.ReadInt("mhfo.dll+5919554");

    /// <inheritdoc/>
    public override int Objective1ID() => this.M.Read2Byte("mhfo.dll+28C2C84");

    /// <inheritdoc/>
    public override int Objective1Quantity() => this.M.Read2Byte("mhfo.dll+28C2C86");

    /// <inheritdoc/>
    public override int Objective1CurrentQuantityMonster() => this.M.Read2Byte("mhfo.dll+60792E6");

    /// <inheritdoc/>
    public override int Objective1CurrentQuantityItem() => this.M.Read2Byte("mhfo.dll+5034732");

    /// <inheritdoc/>
    public override int RavienteTriggeredEvent() => this.M.Read2Byte("mhfo.dll+61005C6");

    /// <inheritdoc/>
    public override int GreatSlayingPoints() => this.M.ReadInt("mhfo.dll+5B45FF8");

    /// <inheritdoc/>
    public override int GreatSlayingPointsSaved() => this.M.ReadInt("mhfo.dll+61005C4");

    /// <inheritdoc/>
    public override int AlternativeMonster1HPInt() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1AtkMult() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1DefMult() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Size() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Poison() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1PoisonNeed() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Sleep() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1SleepNeed() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Para() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1ParaNeed() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Blast() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1BlastNeed() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Stun() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1StunNeed() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part1() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part2() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part3() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part4() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part5() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part6() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part7() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part8() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part9() => 1;

    /// <inheritdoc/>
    public override int AlternativeMonster1Part10() => 1;

    /// <inheritdoc/>
    public override int DivaSkill() => this.M.ReadByte("mhfo.dll+5A95364");

    /// <inheritdoc/>
    public override int StarGrades() => this.M.ReadByte("mhfo.dll+5B3D086");

    /// <inheritdoc/>
    public override int CaravanSkill1() => this.M.ReadByte("mhfo.dll+5034888");

    /// <inheritdoc/>
    public override int CaravanSkill2() => this.M.ReadByte("mhfo.dll+503488A");

    /// <inheritdoc/>
    public override int CaravanSkill3() => this.M.ReadByte("mhfo.dll+503488C");

    /// <inheritdoc/>
    public override int CurrentFaints() => this.M.ReadByte("mhfo.dll+503479B");

    /// <inheritdoc/>
    public override int MaxFaints() => this.M.ReadByte("mhfo.dll+1AA899C");

    /// <inheritdoc/>
    public override int AlternativeMaxFaints() => this.M.ReadByte("mhfo.dll+28C2C64");

    /// <inheritdoc/>
    public override int CaravanScore() => this.M.ReadInt("mhfo.dll+6154FC4");

    /// <inheritdoc/>
    public override int AlternativeQuestMonster1ID() => this.M.ReadByte("mhfo.dll+28C2C84");

    // unsure

    /// <inheritdoc/>
    public override int AlternativeQuestMonster2ID() => this.M.ReadByte("mhfo.dll+28C2C8C");

    /// <inheritdoc/>
    public override int BlademasterWeaponID() => this.M.Read2Byte("mhfo.dll+5033F92");

    /// <inheritdoc/>
    public override int GunnerWeaponID() => this.M.Read2Byte("mhfo.dll+5033F92");

    // the deco addresses for the weapon includes the tower sigils

    /// <inheritdoc/>
    public override int WeaponDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F96");

    /// <inheritdoc/>
    public override int WeaponDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F98");

    /// <inheritdoc/>
    public override int WeaponDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F9A");

    /// <inheritdoc/>
    public override int ArmorHeadID() => this.M.Read2Byte("mhfo.dll+5033F52");

    /// <inheritdoc/>
    public override int ArmorHeadDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F56");

    /// <inheritdoc/>
    public override int ArmorHeadDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F58");

    /// <inheritdoc/>
    public override int ArmorHeadDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F5A");

    /// <inheritdoc/>
    public override int ArmorChestID() => this.M.Read2Byte("mhfo.dll+5033F62");

    /// <inheritdoc/>
    public override int ArmorChestDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F66");

    /// <inheritdoc/>
    public override int ArmorChestDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F68");

    /// <inheritdoc/>
    public override int ArmorChestDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F6A");

    /// <inheritdoc/>
    public override int ArmorArmsID() => this.M.Read2Byte("mhfo.dll+5033F72");

    /// <inheritdoc/>
    public override int ArmorArmsDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F76");

    /// <inheritdoc/>
    public override int ArmorArmsDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F78");

    /// <inheritdoc/>
    public override int ArmorArmsDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F7A");

    /// <inheritdoc/>
    public override int ArmorWaistID() => this.M.Read2Byte("mhfo.dll+5033F82");

    /// <inheritdoc/>
    public override int ArmorWaistDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F86");

    /// <inheritdoc/>
    public override int ArmorWaistDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F88");

    /// <inheritdoc/>
    public override int ArmorWaistDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F8A");

    /// <inheritdoc/>
    public override int ArmorLegsID() => this.M.Read2Byte("mhfo.dll+5033F32");

    /// <inheritdoc/>
    public override int ArmorLegsDeco1ID() => this.M.Read2Byte("mhfo.dll+5033F36");

    /// <inheritdoc/>
    public override int ArmorLegsDeco2ID() => this.M.Read2Byte("mhfo.dll+5033F38");

    /// <inheritdoc/>
    public override int ArmorLegsDeco3ID() => this.M.Read2Byte("mhfo.dll+5033F3A");

    /// <inheritdoc/>
    public override int Cuff1ID() => this.M.Read2Byte("mhfo.dll+50348C2");

    /// <inheritdoc/>
    public override int Cuff2ID() => this.M.Read2Byte("mhfo.dll+50348C4");

    // updates when checking guild card

    /// <inheritdoc/>
    public override int TotalDefense() => this.M.Read2Byte("mhfo.dll+5034338");

    /// <inheritdoc/>
    public override int PouchItem1ID() => this.M.Read2Byte("mhfo.dll+50345A8");

    /// <inheritdoc/>
    public override int PouchItem1Qty() => this.M.Read2Byte("mhfo.dll+50345AA");

    /// <inheritdoc/>
    public override int PouchItem2ID() => this.M.Read2Byte("mhfo.dll+50345B0");

    /// <inheritdoc/>
    public override int PouchItem2Qty() => this.M.Read2Byte("mhfo.dll+50345B2");

    /// <inheritdoc/>
    public override int PouchItem3ID() => this.M.Read2Byte("mhfo.dll+50345B8");

    /// <inheritdoc/>
    public override int PouchItem3Qty() => this.M.Read2Byte("mhfo.dll+50345BA");

    /// <inheritdoc/>
    public override int PouchItem4ID() => this.M.Read2Byte("mhfo.dll+50345C0");

    /// <inheritdoc/>
    public override int PouchItem4Qty() => this.M.Read2Byte("mhfo.dll+50345C2");

    /// <inheritdoc/>
    public override int PouchItem5ID() => this.M.Read2Byte("mhfo.dll+50345C8");

    /// <inheritdoc/>
    public override int PouchItem5Qty() => this.M.Read2Byte("mhfo.dll+50345CA");

    /// <inheritdoc/>
    public override int PouchItem6ID() => this.M.Read2Byte("mhfo.dll+50345D0");

    /// <inheritdoc/>
    public override int PouchItem6Qty() => this.M.Read2Byte("mhfo.dll+50345D2");

    /// <inheritdoc/>
    public override int PouchItem7ID() => this.M.Read2Byte("mhfo.dll+50345D8");

    /// <inheritdoc/>
    public override int PouchItem7Qty() => this.M.Read2Byte("mhfo.dll+610445A");

    /// <inheritdoc/>
    public override int PouchItem8ID() => this.M.Read2Byte("mhfo.dll+50345E0");

    /// <inheritdoc/>
    public override int PouchItem8Qty() => this.M.Read2Byte("mhfo.dll+50345E2");

    /// <inheritdoc/>
    public override int PouchItem9ID() => this.M.Read2Byte("mhfo.dll+50345E8");

    /// <inheritdoc/>
    public override int PouchItem9Qty() => this.M.Read2Byte("mhfo.dll+610446A");

    /// <inheritdoc/>
    public override int PouchItem10ID() => this.M.Read2Byte("mhfo.dll+50345F0");

    /// <inheritdoc/>
    public override int PouchItem10Qty() => this.M.Read2Byte("mhfo.dll+50345F2");

    /// <inheritdoc/>
    public override int PouchItem11ID() => this.M.Read2Byte("mhfo.dll+50345F8");

    /// <inheritdoc/>
    public override int PouchItem11Qty() => this.M.Read2Byte("mhfo.dll+50345FA");

    /// <inheritdoc/>
    public override int PouchItem12ID() => this.M.Read2Byte("mhfo.dll+5034600");

    /// <inheritdoc/>
    public override int PouchItem12Qty() => this.M.Read2Byte("mhfo.dll+5034602");

    /// <inheritdoc/>
    public override int PouchItem13ID() => this.M.Read2Byte("mhfo.dll+5034608");

    /// <inheritdoc/>
    public override int PouchItem13Qty() => this.M.Read2Byte("mhfo.dll+503460A");

    /// <inheritdoc/>
    public override int PouchItem14ID() => this.M.Read2Byte("mhfo.dll+5034610");

    /// <inheritdoc/>
    public override int PouchItem14Qty() => this.M.Read2Byte("mhfo.dll+5034612");

    /// <inheritdoc/>
    public override int PouchItem15ID() => this.M.Read2Byte("mhfo.dll+5034618");

    /// <inheritdoc/>
    public override int PouchItem15Qty() => this.M.Read2Byte("mhfo.dll+503461A");

    /// <inheritdoc/>
    public override int PouchItem16ID() => this.M.Read2Byte("mhfo.dll+5034620");

    /// <inheritdoc/>
    public override int PouchItem16Qty() => this.M.Read2Byte("mhfo.dll+5034622");

    /// <inheritdoc/>
    public override int PouchItem17ID() => this.M.Read2Byte("mhfo.dll+5034628");

    /// <inheritdoc/>
    public override int PouchItem17Qty() => this.M.Read2Byte("mhfo.dll+503462A");

    /// <inheritdoc/>
    public override int PouchItem18ID() => this.M.Read2Byte("mhfo.dll+5034630");

    /// <inheritdoc/>
    public override int PouchItem18Qty() => this.M.Read2Byte("mhfo.dll+5034632");

    /// <inheritdoc/>
    public override int PouchItem19ID() => this.M.Read2Byte("mhfo.dll+5034638");

    /// <inheritdoc/>
    public override int PouchItem19Qty() => this.M.Read2Byte("mhfo.dll+61044BA");

    /// <inheritdoc/>
    public override int PouchItem20ID() => this.M.Read2Byte("mhfo.dll+5034640");

    /// <inheritdoc/>
    public override int PouchItem20Qty() => this.M.Read2Byte("mhfo.dll+5034642");

    /// <inheritdoc/>
    public override int AmmoPouchItem1ID() => this.M.Read2Byte("mhfo.dll+5034648");

    /// <inheritdoc/>
    public override int AmmoPouchItem1Qty() => this.M.Read2Byte("mhfo.dll+503464A");

    /// <inheritdoc/>
    public override int AmmoPouchItem2ID() => this.M.Read2Byte("mhfo.dll+5034650");

    /// <inheritdoc/>
    public override int AmmoPouchItem2Qty() => this.M.Read2Byte("mhfo.dll+5034652");

    /// <inheritdoc/>
    public override int AmmoPouchItem3ID() => this.M.Read2Byte("mhfo.dll+5034658");

    /// <inheritdoc/>
    public override int AmmoPouchItem3Qty() => this.M.Read2Byte("mhfo.dll+503465A");

    /// <inheritdoc/>
    public override int AmmoPouchItem4ID() => this.M.Read2Byte("mhfo.dll+5034660");

    /// <inheritdoc/>
    public override int AmmoPouchItem4Qty() => this.M.Read2Byte("mhfo.dll+5034662");

    /// <inheritdoc/>
    public override int AmmoPouchItem5ID() => this.M.Read2Byte("mhfo.dll+5034668");

    /// <inheritdoc/>
    public override int AmmoPouchItem5Qty() => this.M.Read2Byte("mhfo.dll+503466A");

    /// <inheritdoc/>
    public override int AmmoPouchItem6ID() => this.M.Read2Byte("mhfo.dll+5034670");

    /// <inheritdoc/>
    public override int AmmoPouchItem6Qty() => this.M.Read2Byte("mhfo.dll+5034672");

    /// <inheritdoc/>
    public override int AmmoPouchItem7ID() => this.M.Read2Byte("mhfo.dll+5034678");

    /// <inheritdoc/>
    public override int AmmoPouchItem7Qty() => this.M.Read2Byte("mhfo.dll+503467A");

    /// <inheritdoc/>
    public override int AmmoPouchItem8ID() => this.M.Read2Byte("mhfo.dll+5034680");

    /// <inheritdoc/>
    public override int AmmoPouchItem8Qty() => this.M.Read2Byte("mhfo.dll+5034682");

    /// <inheritdoc/>
    public override int AmmoPouchItem9ID() => this.M.Read2Byte("mhfo.dll+5034688");

    /// <inheritdoc/>
    public override int AmmoPouchItem9Qty() => this.M.Read2Byte("mhfo.dll+503468A");

    /// <inheritdoc/>
    public override int AmmoPouchItem10ID() => this.M.Read2Byte("mhfo.dll+5034690");

    /// <inheritdoc/>
    public override int AmmoPouchItem10Qty() => this.M.Read2Byte("mhfo.dll+5034692");

    // TODO: cat pouch
    // slots

    /// <inheritdoc/>
    public override int ArmorSkill1() => this.M.Read2Byte("mhfo.dll+503475C");

    /// <inheritdoc/>
    public override int ArmorSkill2() => this.M.Read2Byte("mhfo.dll+503475E");

    /// <inheritdoc/>
    public override int ArmorSkill3() => this.M.Read2Byte("mhfo.dll+5034760");

    /// <inheritdoc/>
    public override int ArmorSkill4() => this.M.Read2Byte("mhfo.dll+5034762");

    /// <inheritdoc/>
    public override int ArmorSkill5() => this.M.Read2Byte("mhfo.dll+5034764");

    /// <inheritdoc/>
    public override int ArmorSkill6() => this.M.Read2Byte("mhfo.dll+5034766");

    /// <inheritdoc/>
    public override int ArmorSkill7() => this.M.Read2Byte("mhfo.dll+5034768");

    /// <inheritdoc/>
    public override int ArmorSkill8() => this.M.Read2Byte("mhfo.dll+503476A");

    /// <inheritdoc/>
    public override int ArmorSkill9() => this.M.Read2Byte("mhfo.dll+503476C");

    /// <inheritdoc/>
    public override int ArmorSkill10() => this.M.Read2Byte("mhfo.dll+503476E");

    /// <inheritdoc/>
    public override int ArmorSkill11() => this.M.Read2Byte("mhfo.dll+5034770");

    /// <inheritdoc/>
    public override int ArmorSkill12() => this.M.Read2Byte("mhfo.dll+5034772");

    /// <inheritdoc/>
    public override int ArmorSkill13() => this.M.Read2Byte("mhfo.dll+5034774");

    /// <inheritdoc/>
    public override int ArmorSkill14() => this.M.Read2Byte("mhfo.dll+5034776");

    /// <inheritdoc/>
    public override int ArmorSkill15() => this.M.Read2Byte("mhfo.dll+5034778");

    /// <inheritdoc/>
    public override int ArmorSkill16() => this.M.Read2Byte("mhfo.dll+503477A");

    /// <inheritdoc/>
    public override int ArmorSkill17() => this.M.Read2Byte("mhfo.dll+503477C");

    /// <inheritdoc/>
    public override int ArmorSkill18() => this.M.Read2Byte("mhfo.dll+503477E");

    /// <inheritdoc/>
    public override int ArmorSkill19() => this.M.Read2Byte("mhfo.dll+5034780");

    /// <inheritdoc/>
    public override int BloatedWeaponAttack() => this.M.Read2Byte("mhfo.dll+5BC68C8");

    /// <inheritdoc/>
    public override int ZenithSkill1() => this.M.ReadByte("mhfo.dll+51C16D8");

    /// <inheritdoc/>
    public override int ZenithSkill2() => this.M.ReadByte("mhfo.dll+51C16DA");

    /// <inheritdoc/>
    public override int ZenithSkill3() => this.M.ReadByte("mhfo.dll+51C16DC");

    /// <inheritdoc/>
    public override int ZenithSkill4() => this.M.ReadByte("mhfo.dll+51C16DE");

    /// <inheritdoc/>
    public override int ZenithSkill5() => this.M.ReadByte("mhfo.dll+51C16E0");

    /// <inheritdoc/>
    public override int ZenithSkill6() => this.M.ReadByte("mhfo.dll+51C16E2");

    /// <inheritdoc/>
    public override int ZenithSkill7() => this.M.ReadByte("mhfo.dll+51C16E4");

    /// <inheritdoc/>
    public override int AutomaticSkillWeapon() => this.M.Read2Byte("mhfo.dll+5034792");

    /// <inheritdoc/>
    public override int AutomaticSkillHead() => this.M.Read2Byte("mhfo.dll+503478A");

    /// <inheritdoc/>
    public override int AutomaticSkillChest() => this.M.Read2Byte("mhfo.dll+503478C");

    /// <inheritdoc/>
    public override int AutomaticSkillArms() => this.M.Read2Byte("mhfo.dll+503478E");

    /// <inheritdoc/>
    public override int AutomaticSkillWaist() => this.M.Read2Byte("mhfo.dll+5034790");

    /// <inheritdoc/>
    public override int AutomaticSkillLegs() => this.M.Read2Byte("mhfo.dll+5034786");

    /// <inheritdoc/>
    public override int StyleRank1() => this.M.ReadByte("mhfo.dll+50348D3");

    /// <inheritdoc/>
    public override int StyleRank2() => this.M.ReadByte("mhfo.dll+503499F");

    /// <inheritdoc/>
    public override int GRWeaponLv() => this.M.Read2Byte("mhfo.dll+5033F94");

    /// <inheritdoc/>
    public override int GRWeaponLvBowguns() => this.M.Read2Byte("mhfo.dll+5033F95");

    /// <inheritdoc/>
    public override int Sigil1Name1() => this.M.ReadByte("mhfo.dll+5BF91E4");

    /// <inheritdoc/>
    public override int Sigil1Value1() => this.M.ReadByte("mhfo.dll+5BF91EA");

    /// <inheritdoc/>
    public override int Sigil1Name2() => this.M.ReadByte("mhfo.dll+5BF91E6");

    /// <inheritdoc/>
    public override int Sigil1Value2() => this.M.ReadByte("mhfo.dll+5BF91EC");

    /// <inheritdoc/>
    public override int Sigil1Name3() => this.M.ReadByte("mhfo.dll+5BF91E8");

    /// <inheritdoc/>
    public override int Sigil1Value3() => this.M.ReadByte("mhfo.dll+5BF91EE");

    /// <inheritdoc/>
    public override int Sigil2Name1() => this.M.ReadByte("mhfo.dll+5BF91F0");

    /// <inheritdoc/>
    public override int Sigil2Value1() => this.M.ReadByte("mhfo.dll+5BF91F6");

    /// <inheritdoc/>
    public override int Sigil2Name2() => this.M.ReadByte("mhfo.dll+5BF91F2");

    /// <inheritdoc/>
    public override int Sigil2Value2() => this.M.ReadByte("mhfo.dll+5BF91F8");

    /// <inheritdoc/>
    public override int Sigil2Name3() => this.M.ReadByte("mhfo.dll+5BF91F4");

    /// <inheritdoc/>
    public override int Sigil2Value3() => this.M.ReadByte("mhfo.dll+5BF91FA");

    /// <inheritdoc/>
    public override int Sigil3Name1() => this.M.ReadByte("mhfo.dll+5BF9604");

    /// <inheritdoc/>
    public override int Sigil3Value1() => this.M.ReadByte("mhfo.dll+5BF960A");

    /// <inheritdoc/>
    public override int Sigil3Name2() => this.M.ReadByte("mhfo.dll+5BF9606");

    /// <inheritdoc/>
    public override int Sigil3Value2() => this.M.ReadByte("mhfo.dll+5BF960C");

    /// <inheritdoc/>
    public override int Sigil3Name3() => this.M.ReadByte("mhfo.dll+5BF9608");

    /// <inheritdoc/>
    public override int Sigil3Value3() => this.M.ReadByte("mhfo.dll+5BF960E");

    /// <inheritdoc/>
    public override int FelyneHunted() => this.M.Read2Byte("mhfo.dll+6103A1E");

    /// <inheritdoc/>
    public override int MelynxHunted() => this.M.Read2Byte("mhfo.dll+6103A3A");

    /// <inheritdoc/>
    public override int ShakalakaHunted() => this.M.Read2Byte("mhfo.dll+6103A7E") + 0;

    /// <inheritdoc/>
    public override int VespoidHunted() => this.M.Read2Byte("mhfo.dll+6103A32");

    /// <inheritdoc/>
    public override int HornetaurHunted() => this.M.Read2Byte("mhfo.dll+6103A3C");

    /// <inheritdoc/>
    public override int GreatThunderbugHunted() => this.M.Read2Byte("mhfo.dll+6103A7C");

    /// <inheritdoc/>
    public override int KelbiHunted() => this.M.Read2Byte("mhfo.dll+6103A12");

    /// <inheritdoc/>
    public override int MosswineHunted() => this.M.Read2Byte("mhfo.dll+6103A14");

    /// <inheritdoc/>
    public override int AntekaHunted() => this.M.Read2Byte("mhfo.dll+6103A96");

    /// <inheritdoc/>
    public override int PopoHunted() => this.M.Read2Byte("mhfo.dll+6103A98");

    /// <inheritdoc/>
    public override int AptonothHunted() => this.M.Read2Byte("mhfo.dll+6103A24");

    /// <inheritdoc/>
    public override int ApcerosHunted() => this.M.Read2Byte("mhfo.dll+6103A3E");

    /// <inheritdoc/>
    public override int BurukkuHunted() => this.M.Read2Byte("mhfo.dll+6103ACE");

    /// <inheritdoc/>
    public override int ErupeHunted() => this.M.Read2Byte("mhfo.dll+6103AD0");

    /// <inheritdoc/>
    public override int VelocipreyHunted() => this.M.Read2Byte("mhfo.dll+6103A2C");

    /// <inheritdoc/>
    public override int VelocidromeHunted() => this.M.Read2Byte("mhfo.dll+6103A42");

    /// <inheritdoc/>
    public override int GenpreyHunted() => this.M.Read2Byte("mhfo.dll+6103A26");

    /// <inheritdoc/>
    public override int GendromeHunted() => this.M.Read2Byte("mhfo.dll+6103A44");

    /// <inheritdoc/>
    public override int IopreyHunted() => this.M.Read2Byte("mhfo.dll+6103A48");

    /// <inheritdoc/>
    public override int IodromeHunted() => this.M.Read2Byte("mhfo.dll+6103A4A");

    /// <inheritdoc/>
    public override int GiapreyHunted() => this.M.Read2Byte("mhfo.dll+6103A52");

    /// <inheritdoc/>
    public override int YianKutKuHunted() => this.M.Read2Byte("mhfo.dll+6103A18");

    /// <inheritdoc/>
    public override int BlueYianKutKuHunted() => this.M.Read2Byte("mhfo.dll+6103A58");

    /// <inheritdoc/>
    public override int YianGarugaHunted() => this.M.Read2Byte("mhfo.dll+6103A5C");

    /// <inheritdoc/>
    public override int GypcerosHunted() => this.M.Read2Byte("mhfo.dll+6103A34");

    /// <inheritdoc/>
    public override int PurpleGypcerosHunted() => this.M.Read2Byte("mhfo.dll+6103A5A");

    /// <inheritdoc/>
    public override int HypnocHunted() => this.M.Read2Byte("mhfo.dll+6103AA0");

    /// <inheritdoc/>
    public override int BrightHypnocHunted() => this.M.Read2Byte("mhfo.dll+6103AA8");

    /// <inheritdoc/>
    public override int SilverHypnocHunted() => this.M.Read2Byte("mhfo.dll+6103AB0");

    /// <inheritdoc/>
    public override int FarunokkuHunted() => this.M.Read2Byte("mhfo.dll+6103AF0");

    /// <inheritdoc/>
    public override int ForokururuHunted() => this.M.Read2Byte("mhfo.dll+6103B06");

    /// <inheritdoc/>
    public override int ToridclessHunted() => this.M.Read2Byte("mhfo.dll+6103B26");

    /// <inheritdoc/>
    public override int RemobraHunted() => this.M.Read2Byte("mhfo.dll+6103A8A");

    /// <inheritdoc/>
    public override int RathianHunted() => this.M.Read2Byte("mhfo.dll+6103A0E");

    /// <inheritdoc/>
    public override int PinkRathianHunted() => this.M.Read2Byte("mhfo.dll+6103A56");

    /// <inheritdoc/>
    public override int GoldRathianHunted() => this.M.Read2Byte("mhfo.dll+6103A60");

    /// <inheritdoc/>
    public override int RathalosHunted() => this.M.Read2Byte("mhfo.dll+6103A22");

    /// <inheritdoc/>
    public override int AzureRathalosHunted() => this.M.Read2Byte("mhfo.dll+6103A6E");

    /// <inheritdoc/>
    public override int SilverRathalosHunted() => this.M.Read2Byte("mhfo.dll+6103A5E");

    /// <inheritdoc/>
    public override int KhezuHunted() => this.M.Read2Byte("mhfo.dll+6103A2A");

    /// <inheritdoc/>
    public override int RedKhezuHunted() => this.M.Read2Byte("mhfo.dll+6103A66");

    /// <inheritdoc/>
    public override int BasariosHunted() => this.M.Read2Byte("mhfo.dll+6103A38");

    /// <inheritdoc/>
    public override int GraviosHunted() => this.M.Read2Byte("mhfo.dll+6103A2E");

    /// <inheritdoc/>
    public override int BlackGraviosHunted() => this.M.Read2Byte("mhfo.dll+6103A6A");

    /// <inheritdoc/>
    public override int MonoblosHunted() => this.M.Read2Byte("mhfo.dll+6103A40");

    /// <inheritdoc/>
    public override int WhiteMonoblosHunted() => this.M.Read2Byte("mhfo.dll+6103A64");

    /// <inheritdoc/>
    public override int DiablosHunted() => this.M.Read2Byte("mhfo.dll+6103A28");

    /// <inheritdoc/>
    public override int BlackDiablosHunted() => this.M.Read2Byte("mhfo.dll+6103A62");

    /// <inheritdoc/>
    public override int TigrexHunted() => this.M.Read2Byte("mhfo.dll+6103AA4");

    /// <inheritdoc/>
    public override int EspinasHunted() => this.M.Read2Byte("mhfo.dll+6103AAC");

    /// <inheritdoc/>
    public override int OrangeEspinasHunted() => this.M.Read2Byte("mhfo.dll+6103AAE");

    /// <inheritdoc/>
    public override int WhiteEspinasHunted() => this.M.Read2Byte("mhfo.dll+6103AC0");

    /// <inheritdoc/>
    public override int AkantorHunted() => this.M.Read2Byte("mhfo.dll+6103AA6");

    /// <inheritdoc/>
    public override int BerukyurosuHunted() => this.M.Read2Byte("mhfo.dll+6103AB6");

    /// <inheritdoc/>
    public override int DoragyurosuHunted() => this.M.Read2Byte("mhfo.dll+6103ACA");

    /// <inheritdoc/>
    public override int PariapuriaHunted() => this.M.Read2Byte("mhfo.dll+6103ABE");

    /// <inheritdoc/>
    public override int DyuragauaHunted() => this.M.Read2Byte("mhfo.dll+6103AC8");

    /// <inheritdoc/>
    public override int GurenzeburuHunted() => this.M.Read2Byte("mhfo.dll+6103ACC");

    /// <inheritdoc/>
    public override int OdibatorasuHunted() => this.M.Read2Byte("mhfo.dll+6103AE0");

    /// <inheritdoc/>
    public override int HyujikikiHunted() => this.M.Read2Byte("mhfo.dll+6103AE8");

    /// <inheritdoc/>
    public override int AnorupatisuHunted() => this.M.Read2Byte("mhfo.dll+6103AE6");

    /// <inheritdoc/>
    public override int ZerureusuHunted() => this.M.Read2Byte("mhfo.dll+6103B00") + 0;

    /// <inheritdoc/>
    public override int MeraginasuHunted() => this.M.Read2Byte("mhfo.dll+6103B08");

    /// <inheritdoc/>
    public override int DiorexHunted() => this.M.Read2Byte("mhfo.dll+6103B0A");

    /// <inheritdoc/>
    public override int PoborubarumuHunted() => this.M.Read2Byte("mhfo.dll+6103B12");

    /// <inheritdoc/>
    public override int VarusaburosuHunted() => this.M.Read2Byte("mhfo.dll+6103B10");

    /// <inheritdoc/>
    public override int GureadomosuHunted() => this.M.Read2Byte("mhfo.dll+6103B22");

    /// <inheritdoc/>
    public override int BariothHunted() => this.M.Read2Byte("mhfo.dll+6103B3A");

    // musous are separate???

    /// <inheritdoc/>
    public override int NargacugaHunted() => this.M.Read2Byte("mhfo.dll+6103B4A") + 0;

    /// <inheritdoc/>
    public override int ZenaserisuHunted() => this.M.Read2Byte("mhfo.dll+6103B4E");

    /// <inheritdoc/>
    public override int SeregiosHunted() => this.M.Read2Byte("mhfo.dll+6103B5E");

    /// <inheritdoc/>
    public override int BogabadorumuHunted() => this.M.Read2Byte("mhfo.dll+6103B60");

    /// <inheritdoc/>
    public override int CephalosHunted() => this.M.Read2Byte("mhfo.dll+6103A50");

    /// <inheritdoc/>
    public override int CephadromeHunted() => this.M.Read2Byte("mhfo.dll+6103A1C");

    /// <inheritdoc/>
    public override int PlesiothHunted() => this.M.Read2Byte("mhfo.dll+6103A36");

    /// <inheritdoc/>
    public override int GreenPlesiothHunted() => this.M.Read2Byte("mhfo.dll+6103A68");

    /// <inheritdoc/>
    public override int VolganosHunted() => this.M.Read2Byte("mhfo.dll+6103AA2");

    /// <inheritdoc/>
    public override int RedVolganosHunted() => this.M.Read2Byte("mhfo.dll+6103AAA");

    /// <inheritdoc/>
    public override int HermitaurHunted() => this.M.Read2Byte("mhfo.dll+6103A90");

    /// <inheritdoc/>
    public override int DaimyoHermitaurHunted() => this.M.Read2Byte("mhfo.dll+6103A6C");

    /// <inheritdoc/>
    public override int CeanataurHunted() => this.M.Read2Byte("mhfo.dll+6103A9E");

    /// <inheritdoc/>
    public override int ShogunCeanataurHunted() => this.M.Read2Byte("mhfo.dll+6103A92");

    /// <inheritdoc/>
    public override int ShenGaorenHunted() => this.M.Read2Byte("mhfo.dll+6103A7A");

    /// <inheritdoc/>
    public override int AkuraVashimuHunted() => this.M.Read2Byte("mhfo.dll+6103AB2");

    /// <inheritdoc/>
    public override int AkuraJebiaHunted() => this.M.Read2Byte("mhfo.dll+6103AB4");

    /// <inheritdoc/>
    public override int TaikunZamuzaHunted() => this.M.Read2Byte("mhfo.dll+6103ADA");

    /// <inheritdoc/>
    public override int KusubamiHunted() => this.M.Read2Byte("mhfo.dll+6103B2A");

    /// <inheritdoc/>
    public override int BullfangoHunted() => this.M.Read2Byte("mhfo.dll+6103A16");

    /// <inheritdoc/>
    public override int BulldromeHunted() => this.M.Read2Byte("mhfo.dll+6103A94");

    /// <inheritdoc/>
    public override int CongaHunted() => this.M.Read2Byte("mhfo.dll+6103A88");

    /// <inheritdoc/>
    public override int CongalalaHunted() => this.M.Read2Byte("mhfo.dll+6103A74");

    /// <inheritdoc/>
    public override int BlangoHunted() => this.M.Read2Byte("mhfo.dll+6103A86");

    /// <inheritdoc/>
    public override int BlangongaHunted() => this.M.Read2Byte("mhfo.dll+6103A72");

    /// <inheritdoc/>
    public override int GogomoaHunted() => this.M.Read2Byte("mhfo.dll+6103AD6");

    /// <inheritdoc/>
    public override int RajangHunted() => this.M.Read2Byte("mhfo.dll+6103A76");

    /// <inheritdoc/>
    public override int KamuOrugaronHunted() => this.M.Read2Byte("mhfo.dll+6103AC2");

    /// <inheritdoc/>
    public override int NonoOrugaronHunted() => this.M.Read2Byte("mhfo.dll+6103AC4");

    /// <inheritdoc/>
    public override int MidogaronHunted() => this.M.Read2Byte("mhfo.dll+6103AEA");

    /// <inheritdoc/>
    public override int GougarfHunted() => this.M.Read2Byte("mhfo.dll+6103B02");

    /// <inheritdoc/>
    public override int VoljangHunted() => this.M.Read2Byte("mhfo.dll+6103B48");

    /// <inheritdoc/>
    public override int KirinHunted() => this.M.Read2Byte("mhfo.dll+6103A4E");

    /// <inheritdoc/>
    public override int KushalaDaoraHunted() => this.M.Read2Byte("mhfo.dll+6103A78");

    /// <inheritdoc/>
    public override int RustedKushalaDaoraHunted() => this.M.Read2Byte("mhfo.dll+6103A84");

    /// <inheritdoc/>
    public override int ChameleosHunted() => this.M.Read2Byte("mhfo.dll+6103A82");

    /// <inheritdoc/>
    public override int LunastraHunted() => this.M.Read2Byte("mhfo.dll+6103A8C");

    /// <inheritdoc/>
    public override int TeostraHunted() => this.M.Read2Byte("mhfo.dll+6103A8E");

    /// <inheritdoc/>
    public override int LaoShanLungHunted() => this.M.Read2Byte("mhfo.dll+6103A1A");

    /// <inheritdoc/>
    public override int AshenLaoShanLungHunted() => this.M.Read2Byte("mhfo.dll+6103A70");

    // untested

    /// <inheritdoc/>
    public override int YamaTsukamiHunted() => this.M.Read2Byte("mhfo.dll+6103A80");

    /// <inheritdoc/>
    public override int RukodioraHunted() => this.M.Read2Byte("mhfo.dll+6103AD2");

    /// <inheritdoc/>
    public override int RebidioraHunted() => this.M.Read2Byte("mhfo.dll+6103AE4");

    /// <inheritdoc/>
    public override int FatalisHunted() => this.M.Read2Byte("mhfo.dll+6103A10");

    /// <inheritdoc/>
    public override int ShantienHunted() => this.M.Read2Byte("mhfo.dll+6103AF4");

    /// <inheritdoc/>
    public override int DisufiroaHunted() => this.M.Read2Byte("mhfo.dll+6103AE2");

    /// <inheritdoc/>
    public override int GarubaDaoraHunted() => this.M.Read2Byte("mhfo.dll+6103B0C");

    /// <inheritdoc/>
    public override int InagamiHunted() => this.M.Read2Byte("mhfo.dll+6103B0E");

    /// <inheritdoc/>
    public override int HarudomeruguHunted() => this.M.Read2Byte("mhfo.dll+6103B24");

    /// <inheritdoc/>
    public override int YamaKuraiHunted() => this.M.Read2Byte("mhfo.dll+6103B2C");

    /// <inheritdoc/>
    public override int ToaTesukatoraHunted() => this.M.Read2Byte("mhfo.dll+6103B38");

    /// <inheritdoc/>
    public override int GuanzorumuHunted() => this.M.Read2Byte("mhfo.dll+6103B40");

    /// <inheritdoc/>
    public override int KeoaruboruHunted() => this.M.Read2Byte("mhfo.dll+6103B4C");

    /// <inheritdoc/>
    public override int ShagaruMagalaHunted() => this.M.Read2Byte("mhfo.dll+6103B54");

    /// <inheritdoc/>
    public override int ElzelionHunted() => this.M.Read2Byte("mhfo.dll+6103B58");

    /// <inheritdoc/>
    public override int AmatsuHunted() => this.M.Read2Byte("mhfo.dll+6103B56");

    /// <inheritdoc/>
    public override int AbioruguHunted() => this.M.Read2Byte("mhfo.dll+6103ADC");

    /// <inheritdoc/>
    public override int GiaoruguHunted() => this.M.Read2Byte("mhfo.dll+6103AEC");

    /// <inheritdoc/>
    public override int GasurabazuraHunted() => this.M.Read2Byte("mhfo.dll+6103B28");

    /// <inheritdoc/>
    public override int DeviljhoHunted() => this.M.Read2Byte("mhfo.dll+6103B32");

    /// <inheritdoc/>
    public override int BrachydiosHunted() => this.M.Read2Byte("mhfo.dll+6103B34");

    /// <inheritdoc/>
    public override int UragaanHunted() => this.M.Read2Byte("mhfo.dll+6103B3C");

    /// <inheritdoc/>
    public override int KuarusepusuHunted() => this.M.Read2Byte("mhfo.dll+6103ADE");

    /// <inheritdoc/>
    public override int PokaraHunted() => this.M.Read2Byte("mhfo.dll+6103AF6");

    /// <inheritdoc/>
    public override int PokaradonHunted() => this.M.Read2Byte("mhfo.dll+6103AF2");

    /// <inheritdoc/>
    public override int BaruragaruHunted() => this.M.Read2Byte("mhfo.dll+6103AFE");

    /// <inheritdoc/>
    public override int ZinogreHunted() => this.M.Read2Byte("mhfo.dll+6103B30");

    /// <inheritdoc/>
    public override int StygianZinogreHunted() => this.M.Read2Byte("mhfo.dll+6103B3E");

    /// <inheritdoc/>
    public override int GoreMagalaHunted() => this.M.Read2Byte("mhfo.dll+6103B50");

    /// <inheritdoc/>
    public override int BombardierBogabadorumuHunted() => this.M.Read2Byte("mhfo.dll+6103B64");

    /// <inheritdoc/>
    public override int SparklingZerureusuHunted() => this.M.Read2Byte("mhfo.dll+6103B68");

    /// <inheritdoc/>
    public override int StarvingDeviljhoHunted() => this.M.Read2Byte("mhfo.dll+6103B42");

    /// <inheritdoc/>
    public override int CrimsonFatalisHunted() => this.M.Read2Byte("mhfo.dll+6103A54");

    /// <inheritdoc/>
    public override int WhiteFatalisHunted() => this.M.Read2Byte("mhfo.dll+6103A9A");

    /// <inheritdoc/>
    public override int CactusHunted() => this.M.Read2Byte("mhfo.dll+6103AB8");

    /// <inheritdoc/>
    public override int ArrogantDuremudiraHunted() => this.M.Read2Byte("mhfo.dll+6103B5A");

    // untested

    /// <inheritdoc/>
    public override int KingShakalakaHunted() => this.M.Read2Byte("mhfo.dll+6103B6C");

    /// <inheritdoc/>
    public override int MiRuHunted() => this.M.Read2Byte("mhfo.dll+6103AEE");

    /// <inheritdoc/>
    public override int UnknownHunted() => this.M.Read2Byte("mhfo.dll+6103AD4");

    /// <inheritdoc/>
    public override int GoruganosuHunted() => this.M.Read2Byte("mhfo.dll+6103AFA");

    /// <inheritdoc/>
    public override int AruganosuHunted() => this.M.Read2Byte("mhfo.dll+6103AFC");

    /// <inheritdoc/>
    public override int PSO2RappyHunted() => this.M.Read2Byte("mhfo.dll+6103B6A");

    /// <inheritdoc/>
    public override int RocksHunted() => this.M.Read2Byte("mhfo.dll+6103A46");

    /// <inheritdoc/>
    public override int UrukiHunted() => this.M.Read2Byte("mhfo.dll+6103B04");

    /// <inheritdoc/>
    public override int GorgeObjectsHunted() => this.M.Read2Byte("mhfo.dll+6103ABA");

    /// <inheritdoc/>
    public override int BlinkingNargacugaHunted() => this.M.Read2Byte("mhfo.dll+6103B52");

    /// <inheritdoc/>
    public override int QuestState() => this.M.ReadByte("mhfo.dll+61180F2");

    /// <inheritdoc/>
    public override int RoadDureSkill1Name() => this.M.ReadByte("mhfo.dll+610403C");

    /// <inheritdoc/>
    public override int RoadDureSkill1Level() => this.M.ReadByte("mhfo.dll+610403E");

    /// <inheritdoc/>
    public override int RoadDureSkill2Name() => this.M.ReadByte("mhfo.dll+6104040");

    /// <inheritdoc/>
    public override int RoadDureSkill2Level() => this.M.ReadByte("mhfo.dll+6104042");

    /// <inheritdoc/>
    public override int RoadDureSkill3Name() => this.M.ReadByte("mhfo.dll+6104044");

    /// <inheritdoc/>
    public override int RoadDureSkill3Level() => this.M.ReadByte("mhfo.dll+6104046");

    /// <inheritdoc/>
    public override int RoadDureSkill4Name() => this.M.ReadByte("mhfo.dll+6104048");

    /// <inheritdoc/>
    public override int RoadDureSkill4Level() => this.M.ReadByte("mhfo.dll+610404A");

    /// <inheritdoc/>
    public override int RoadDureSkill5Name() => this.M.ReadByte("mhfo.dll+610404C");

    /// <inheritdoc/>
    public override int RoadDureSkill5Level() => this.M.ReadByte("mhfo.dll+610404E");

    /// <inheritdoc/>
    public override int RoadDureSkill6Name() => this.M.ReadByte("mhfo.dll+6104050");

    /// <inheritdoc/>
    public override int RoadDureSkill6Level() => this.M.ReadByte("mhfo.dll+6104052");

    /// <inheritdoc/>
    public override int RoadDureSkill7Name() => this.M.ReadByte("mhfo.dll+6104054");

    /// <inheritdoc/>
    public override int RoadDureSkill7Level() => this.M.ReadByte("mhfo.dll+6104056");

    /// <inheritdoc/>
    public override int RoadDureSkill8Name() => this.M.ReadByte("mhfo.dll+6104058");

    /// <inheritdoc/>
    public override int RoadDureSkill8Level() => this.M.ReadByte("mhfo.dll+610405A");

    /// <inheritdoc/>
    public override int RoadDureSkill9Name() => this.M.ReadByte("mhfo.dll+610405C");

    /// <inheritdoc/>
    public override int RoadDureSkill9Level() => this.M.ReadByte("mhfo.dll+610405E");

    /// <inheritdoc/>
    public override int RoadDureSkill10Name() => this.M.ReadByte("mhfo.dll+6104060");

    /// <inheritdoc/>
    public override int RoadDureSkill10Level() => this.M.ReadByte("mhfo.dll+6104062");

    /// <inheritdoc/>
    public override int RoadDureSkill11Name() => this.M.ReadByte("mhfo.dll+6104064");

    /// <inheritdoc/>
    public override int RoadDureSkill11Level() => this.M.ReadByte("mhfo.dll+6104066");

    /// <inheritdoc/>
    public override int RoadDureSkill12Name() => this.M.ReadByte("mhfo.dll+6104068");

    /// <inheritdoc/>
    public override int RoadDureSkill12Level() => this.M.ReadByte("mhfo.dll+610406A");

    /// <inheritdoc/>
    public override int RoadDureSkill13Name() => this.M.ReadByte("mhfo.dll+610406C");

    /// <inheritdoc/>
    public override int RoadDureSkill13Level() => this.M.ReadByte("mhfo.dll+610406E");

    /// <inheritdoc/>
    public override int RoadDureSkill14Name() => this.M.ReadByte("mhfo.dll+6104070");

    /// <inheritdoc/>
    public override int RoadDureSkill14Level() => this.M.ReadByte("mhfo.dll+6104072");

    /// <inheritdoc/>
    public override int RoadDureSkill15Name() => this.M.ReadByte("mhfo.dll+6104074");

    /// <inheritdoc/>
    public override int RoadDureSkill15Level() => this.M.ReadByte("mhfo.dll+6104076");

    /// <inheritdoc/>
    public override int RoadDureSkill16Name() => this.M.ReadByte("mhfo.dll+6104078");

    /// <inheritdoc/>
    public override int RoadDureSkill16Level() => this.M.ReadByte("mhfo.dll+610407A");

    /// <inheritdoc/>
    public override int PartySize() => this.M.ReadByte("mhfo.dll+57967C8");

    /// <inheritdoc/>
    public override int PartySizeMax() => this.M.ReadByte("mhfo.dll+61B6088");

    /// <inheritdoc/>
    public override uint GSRP() => (uint)this.M.ReadInt("mhfo.dll+61041C8");

    /// <inheritdoc/>
    public override uint GRP() => (uint)this.M.ReadInt("mhfo.dll+5BC82F8");

    /// <inheritdoc/>
    public override int HunterHP() => this.M.Read2Byte("mhfo.dll+5BC6548");

    /// <inheritdoc/>
    public override int HunterStamina() => this.M.Read2Byte("mhfo.dll+503438C");

    /// <inheritdoc/>
    public override int QuestItemsUsed() => this.M.Read2Byte("mhfo.dll+57EAE24");

    /// <inheritdoc/>
    public override int AreaHitsTakenBlocked() => this.M.Read2Byte("mhfo.dll+5034078");

    // TODO Untested

    /// <inheritdoc/>
    public override int PartnyaBagItem1ID() => this.M.Read2Byte("mhfo.dll+5745788");

    /// <inheritdoc/>
    public override int PartnyaBagItem1Qty() => this.M.Read2Byte("mhfo.dll+574578A");

    /// <inheritdoc/>
    public override int PartnyaBagItem2ID() => this.M.Read2Byte("mhfo.dll+574578C");

    /// <inheritdoc/>
    public override int PartnyaBagItem2Qty() => this.M.Read2Byte("mhfo.dll+574578E");

    /// <inheritdoc/>
    public override int PartnyaBagItem3ID() => this.M.Read2Byte("mhfo.dll+5745790");

    /// <inheritdoc/>
    public override int PartnyaBagItem3Qty() => this.M.Read2Byte("mhfo.dll+5745792");

    /// <inheritdoc/>
    public override int PartnyaBagItem4ID() => this.M.Read2Byte("mhfo.dll+5745794");

    /// <inheritdoc/>
    public override int PartnyaBagItem4Qty() => this.M.Read2Byte("mhfo.dll+5745796");

    /// <inheritdoc/>
    public override int PartnyaBagItem5ID() => this.M.Read2Byte("mhfo.dll+5745798");

    /// <inheritdoc/>
    public override int PartnyaBagItem5Qty() => this.M.Read2Byte("mhfo.dll+574579A");

    /// <inheritdoc/>
    public override int PartnyaBagItem6ID() => this.M.Read2Byte("mhfo.dll+574579C");

    /// <inheritdoc/>
    public override int PartnyaBagItem6Qty() => this.M.Read2Byte("mhfo.dll+574579E");

    /// <inheritdoc/>
    public override int PartnyaBagItem7ID() => this.M.Read2Byte("mhfo.dll+57457A0");

    /// <inheritdoc/>
    public override int PartnyaBagItem7Qty() => this.M.Read2Byte("mhfo.dll+57457A2");

    /// <inheritdoc/>
    public override int PartnyaBagItem8ID() => this.M.Read2Byte("mhfo.dll+57457A4");

    /// <inheritdoc/>
    public override int PartnyaBagItem8Qty() => this.M.Read2Byte("mhfo.dll+57457A6");

    /// <inheritdoc/>
    public override int PartnyaBagItem9ID() => this.M.Read2Byte("mhfo.dll+57457A8");

    /// <inheritdoc/>
    public override int PartnyaBagItem9Qty() => this.M.Read2Byte("mhfo.dll+57457AA");

    /// <inheritdoc/>
    public override int PartnyaBagItem10ID() => this.M.Read2Byte("mhfo.dll+57457AC");

    /// <inheritdoc/>
    public override int PartnyaBagItem10Qty() => this.M.Read2Byte("mhfo.dll+57457AE");

    /// <inheritdoc/>
    public override int QuestToggleMonsterMode() => this.M.ReadByte("mhfo.dll+5B05B8E");

    /// <inheritdoc/>
    public override int Rights() => this.M.Read2Byte("mhfo.dll+5D98294");

    /// <inheritdoc/>
    public override decimal PlayerPositionX() => (decimal)this.M.ReadFloat("mhfo.dll+5CACB50", string.Empty, false);

    /// <inheritdoc/>
    public override decimal PlayerPositionY() => (decimal)this.M.ReadFloat("mhfo.dll+5CACB54", string.Empty, false);

    /// <inheritdoc/>
    public override decimal PlayerPositionZ() => (decimal)this.M.ReadFloat("mhfo.dll+5CACB58", string.Empty, false);

    /// <inheritdoc/>
    public override decimal PlayerPositionInQuestX() => (decimal)this.M.ReadFloat("mhfo.dll+20BB540", string.Empty, false);

    /// <inheritdoc/>
    public override decimal PlayerPositionInQuestY() => (decimal)this.M.ReadFloat("mhfo.dll+20BB544", string.Empty, false);

    /// <inheritdoc/>
    public override decimal PlayerPositionInQuestZ() => (decimal)this.M.ReadFloat("mhfo.dll+20BB548", string.Empty, false);

    /// <inheritdoc/>
    public override int ActiveFeature1() => this.M.Read2Byte("mhfo.dll+1BD2F50");

    /// <inheritdoc/>
    public override int ActiveFeature2() => this.M.Read2Byte("mhfo.dll+1BD2F58");

    /// <inheritdoc/>
    public override int ActiveFeature3() => this.M.Read2Byte("mhfo.dll+57E26E8");

    /// <inheritdoc/>
    public override int ServerHeartbeatLandMain() => this.M.ReadInt("mhfo.dll+5E83A00");

    /// <inheritdoc/>
    public override int ServerHeartbeatLandAlternative() => this.M.ReadInt("mhfo.dll+5D983C0");

    /// <inheritdoc/>
    public override int LandSlot() => this.M.ReadInt("mhfo.dll+61C11A0");

    /// <inheritdoc/>
    public override int GuildFoodStart() => this.M.ReadInt("mhfo.dll+5BC70E0");

    /// <inheritdoc/>
    public override int DivaSongStart() => this.M.ReadInt("mhfo.dll+61033B0");

    /// <inheritdoc/>
    //public override int DivaPrayerGemStart() => this.M.ReadInt("mhfo.dll+5BE91C8");

    /// <inheritdoc/>
    public override int GuildPoogie1Skill() => this.M.ReadByte("mhfo.dll+5B33FB3");

    /// <inheritdoc/>
    public override int GuildPoogie2Skill() => this.M.ReadByte("mhfo.dll+5B33FC3");

    /// <inheritdoc/>
    public override int GuildPoogie3Skill() => this.M.ReadByte("mhfo.dll+5B33FD3");

    /// <inheritdoc/>
    public override int DivaPrayerGemRedSkill() => this.M.Read2Byte("mhfo.dll+5A95354");

    /// <inheritdoc/>
    public override int DivaPrayerGemRedLevel() => this.M.Read2Byte("mhfo.dll+5A95356");

    /// <inheritdoc/>
    public override int DivaPrayerGemYellowSkill() => this.M.Read2Byte("mhfo.dll+5A95358");

    /// <inheritdoc/>
    public override int DivaPrayerGemYellowLevel() => this.M.Read2Byte("mhfo.dll+5A9535A");

    /// <inheritdoc/>
    public override int DivaPrayerGemGreenSkill() => this.M.Read2Byte("mhfo.dll+5A9535C");

    /// <inheritdoc/>
    public override int DivaPrayerGemGreenLevel() => this.M.Read2Byte("mhfo.dll+5A9535E");

    /// <inheritdoc/>
    public override int DivaPrayerGemBlueSkill() => this.M.Read2Byte("mhfo.dll+5A95360");

    /// <inheritdoc/>
    public override int DivaPrayerGemBlueLevel() => this.M.Read2Byte("mhfo.dll+5A95362");

    /// <inheritdoc/>
    public override bool HalkOn() => this.M.ReadByte("mhfo.dll+5BC6603") > 0 ? true : false;

    /// <inheritdoc/>
    public override bool HalkPotEffectOn() => this.M.ReadByte("mhfo.dll+5034964") > 0 ? true : false;

    /// <inheritdoc/>
    public override int DivaSongFromGuildStart() => this.M.ReadInt("mhfo.dll+6104384");

    /// <inheritdoc/>
    public override int QuestVariant1() => this.M.ReadByte("mhfo.dll+28C2CE7");

    /// <inheritdoc/>
    public override int QuestVariant2() => this.M.ReadByte("mhfo.dll+28C2CE8");

    /// <inheritdoc/>
    public override int QuestVariant3() => this.M.ReadByte("mhfo.dll+28C2CE9");

    /// <inheritdoc/>
    public override int QuestVariant4() => this.M.ReadByte("mhfo.dll+28C2CEA");

    /// <inheritdoc/>
    public override int DualSwordsSharpens() => this.M.ReadByte("mhfo.dll+50346B8");


}
