// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

/*
[Flags]: Indicates that an enum is intended to be used as a bit field,
where each value represents a single bit. Any enum can be used as flags, it does not require
the attribute. What the Flags attribute does is changing the default ToString behavior.
Without the Flags attribute, any enum whose value doesn't match a specified member exactly would
make ToString return its numeric value. Whereas if you use the Flags attribute,
you get a comma separated list of the flags that are set, which can be desirable.

[Obsolete]: Marks an enum as obsolete and provides a message to indicate why it is obsolete.

[DataContract], [DataMember]: Used for serializing an enum to a specific format,
such as JSON or XML.

[Description]: This attribute can be used to provide a string that describes the enum value.
It can be useful when generating documentation or when displaying the enum values
in a user interface.

[DefaultValue]: This attribute can be used to specify the default value for an enum.
It can be useful when you want to initialize a variable with the default value of the enum.

[XmlEnum]: This attribute can be used to specify the XML name for an enum value.
It can be useful when serializing or deserializing an enum to or from XML.

[JsonConverter]: used to specify a custom converter for JSON serialization and deserialization,
and it works independently of the [Flags] attribute.

Validation:
string printValidity(Status status){
    switch (status)
    {
        case Status.Failed:
        case Status.OK:
        case Status.Waiting:
            return "Valid input";
        default:
            return "Invalid input";
    }
}
*/

namespace MHFZ_Overlay.Models.Structures;

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

/// <summary>
/// Some of these fields have odd interactions.
/// Supremacy and UNK are set on Raviente weapons.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WeaponSpecialProperty1 : uint
{
    [DefaultValue(None)]
    None = 0,
    SP = 1,
    [Description("Goushou")]
    Gou = 2,
    Supremacy = 4,
    [Description("HC")]
    Hardcore = 8,
    RandomGou = 16,
    [Description("Raviente weapons with a different element")]
    GachaEvolution = 32,
    GRank = 64,
    UNK = 128,
    Raviente = Supremacy | UNK, // bitwise operators are pog
}

/// <summary>
/// Unique properties such as Zenith Part Breaker, most are mutually exclusive.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WeaponSpecialProperty2 : uint
{
    [DefaultValue(None)]
    None = 0,
    MasterMark = 1,
    HeavenlyStorm = 2,
    Supremacy = 4,
    GRank = 8,
    GSupremacy = 16,
    Burst = 32,
    GFinesse = 64,
    Tower = 128,
    Hardcore = 256,
    Exotic = 512,
    GEvolution = 1024,
    [Description("From Diva event")]
    Prayer = 2048,
    Zenith = 4096,
    UNK1 = 8192,
    UNK2 = 16384,
    UNK3 = 32768,
    ZenithFinesse = GFinesse | Zenith, // TODO untested
}

/// <summary>
/// Raviente specific shots are defined by a bitfield on the weapon entry itself,
/// they cannot be added to arbitrary bowguns.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BowgunShots : uint
{
    [DefaultValue(None)]
    None = 0,
    Normal1 = 1,
    Normal2 = 2,
    Normal3 = 4,
    Pierce1 = 8,
    Pierce2 = 16,
    Pierce3 = 32,
    Pellet1 = 64,
    Pellet2 = 128,
    Pellet3 = 256,
    Crag1 = 512,
    Crag2 = 1024,
    Crag3 = 2048,
    Cluster1 = 4096,
    Cluster2 = 8192,
    Cluster3 = 16384,
    FlameShot = 32768,
    WaterShot = 65536,
    ThunderShot = 131072,
    FreezeShot = 262144,
    DragonShot = 524288,
    RecoveryShot1 = 1048576,
    RecoveryShot2 = 2097152,
    PoisonShot1 = 4194304,
    PoisonShot2 = 8388608,
    ParalysisShot1 = 16777216,
    ParalysisShot2 = 33554432,
    SleepShot1 = 67108864,
    SleepShot2 = 134217728,
    TranqShot = 268435456,
    PaintShot = 536870912,
    DemonShot = 1073741824,
    ArmorShot = 2147483648,
}

/// <summary>
/// Impact shots are typically limited to Raviente only and Blast to Gou trees.
/// Both can arbitrarily be added to any bows.
/// For Poison/Sleep/Paralysis +1 or +2 select the base coating on top row too.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BowCoatings : uint
{
    [DefaultValue(None)]
    None = 0,
    NULL1 = 1,
    Power = 2,
    Poison = 4,
    Paralysis = 8,
    Sleep = 16,
    Blast = 32,
    Dummy = 64,
    Impact = 128,
    PoisonPLUS1 = 256,
    ParalysisPLUS1 = 512,
    SleepPLUS1 = 1024,
    PoisonPLUS2 = 2048,
    ParalysisPLUS2 = 4096,
    SleepPLUS2 = 8192,
    NULL2 = 16384,
    NULL3 = 32768,
}

/// <summary>
/// Quest states.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestState : uint
{
    [DefaultValue(None)]
    None = 0,
    AchievedMainObjective = 1,
    UNK1 = 2,
    UNK2 = 4,
    UNK3 = 8,
    UNK4 = 16,
    UNK5 = 32,
    UNK6 = 64,
    UNK7 = 128,
    RewardScreen = AchievedMainObjective | UNK7,
}

/// <summary>
/// Quest banned weapons.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestBannedWeapons : uint
{
    [DefaultValue(None)]
    None = 0,
    Tower = 1,
    Evolution = 2,
    Master = 4,
    HC = 8,
    SP = 16,
    RNGou = 32,
    Gou = 64,
    Heaven = 128,
    Supremacy = 256,
    GSupremacy = 512,
    Burst = 1024,
    GRank = 2048,
    GLevel = 4096,
    Origin = 8192,
    Other = 16384,
    Exotic = 32768,
    Prayer = 65536,
    Zenith = 131072,
}

/// <summary>
/// Quest weapon types disabled.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestWeaponTypesDisabled : uint
{
    [DefaultValue(None)]
    None = 0,
    GreatSword = 1,
    HeavyBowgun = 2,
    Hammer = 4,
    Lance = 8,
    SwordAndShield = 16,
    LightBowgun = 32,
    DualSwords = 64,
    LongSword = 128,
    HuntingHorn = 256,
    Gunlance = 512,
    Bow = 1024,
    Tonfa = 2048,
    SwitchAxeF = 4096,
    // MS Flag 64
}

/// <summary>
/// Quest variant 1.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestVariant1 : uint
{
    [DefaultValue(HR)]
    HR = 0,
    Hiden = 1,
    HardcoreFixed = 2,
    HardcoreUnlimitedToggle = 4,
    GRank = 8,
    UNK1 = 16,
    Diva = 32,
    HarcoreNormalToggle = 64,
    UnlimitedFixed = 128,
}

/// <summary>
/// Quest variant 2.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestVariant2 : uint
{
    [DefaultValue(None)]
    None = 0,
    Level1 = 1,
    DisableHalkPotion = 2,
    DisableHalkPoogie = 4,
    Timer = 8,
    DisableActiveFeature = 16,
    FixedDifficulty = 32,
    Level9999 = 64,
    Road = 128,
}

/// <summary>
/// Quest variant 3.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestVariant3 : uint
{
    [DefaultValue(None)]
    None = 0,
    DisableRewardBonus = 1,
    RequireGRank = 2,
    UNK1 = 4,
    UNK2 = 8,
    Zenith = 16,
    DivaDefense = 32,
    UNK3Course = 64,
    DisabledSigil = 128,
}

/// <summary>
/// Quest variant 4.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestVariant4 : uint
{
    [DefaultValue(None)]
    None = 0,
    UNK0 = 1,
    UNK1 = 2,
    UNK2 = 4,
    UNK3 = 8,
    UNK4 = 16,
    UNK5 = 32,
    UNK6 = 64,
    UNK7 = 128,
}

/// <summary>
/// Quest objective type.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestObjectiveType : uint
{
    [DefaultValue (None)]
    None = 0,
    Hunt = 1,
    Deliver = 2,
    UNK1 = 4,
    UNK2 = 8,

    /// <summary>
    /// TODO: what is this for?
    /// </summary>
    EsotericAction = 16,
    UNK3 = 32,
    UNK4 = 64,
    UNK5 = 128,
    UNK6 = 256,
    Capture = Hunt | UNK6,
    UNK7 = 512,
    Slay = Hunt | UNK7,
    UNK8 = 1_024,
    UNK9 = 2_048,
    UNK10 = 4_096,
    DeliverFlag = Deliver | UNK10,
    UNK11 = 8192,
    UNK12 = 16_384,
    PartBreak = UNK1 | UNK12,
    UNK13 = 32_768,
    Damage = UNK1 | UNK13,
    UNK14 = 65_536,
    SlayDamage = UNK14 | UNK13 | UNK1,
    SlayTotal = 131_072,
    SlayAll = 262_144,
}

/// <summary>
/// Quest toggle monster mode option.
/// </summary>
[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestToggleMonsterModeOption : uint
{
    [DefaultValue(Normal)]
    Normal = 0,
    Hardcore = 1,
    UNK1 = 2,
    Unlimited = Hardcore | UNK1,
}
