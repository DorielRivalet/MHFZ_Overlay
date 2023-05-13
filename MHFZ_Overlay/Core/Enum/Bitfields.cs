using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

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

namespace MHFZ_Overlay.Core.Enum
{
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
        Random_Gou = 16,
        [Description("Raviente weapons with a different element")]
        Gacha_Evolution = 32,
        G_Rank = 64,
        UNK = 128,
        Raviente = Supremacy | UNK // bitwise operators are pog
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
        Master_Mark = 1,
        Heavenly_Storm = 2,
        Supremacy = 4,
        G_Rank = 8,
        G_Supremacy = 16,
        Burst = 32,
        G_Finesse = 64,
        Tower = 128,
        Hardcore = 256,
        Exotic = 512,
        G_Evolution = 1024,
        [Description("From Diva event")]
        Prayer = 2048,
        Zenith = 4096,
        UNK1 = 8192,
        UNK2 = 16384,
        UNK3 = 32768,
        Zenith_Finesse = G_Finesse | Zenith // TODO untested
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
        Flame_Shot = 32768,
        Water_Shot = 65536,
        Thunder_Shot = 131072,
        Freeze_Shot = 262144,
        Dragon_Shot = 524288,
        Recovery_Shot1 = 1048576,
        Recovery_Shot2 = 2097152,
        Poison_Shot1 = 4194304,
        Poison_Shot2 = 8388608,
        Paralysis_Shot1 = 16777216,
        Paralysis_Shot2 = 33554432,
        Sleep_Shot1 = 67108864,
        Sleep_Shot2 = 134217728,
        Tranq_Shot = 268435456,
        Paint_Shot = 536870912,
        Demon_Shot = 1073741824,
        Armor_Shot = 2147483648
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
        NULL3 = 32768
    }

    /// <summary>
    /// Quest states
    /// </summary>
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuestState : uint
    {
        [DefaultValue(None)]
        None = 0,
        Achieved_Main_Objective = 1,
        UNK1 = 2,
        UNK2 = 4,
        UNK3 = 8,
        UNK4 = 16,
        UNK5 = 32,
        UNK6 = 64,
        UNK7 = 128,
        Quest_Clear = Achieved_Main_Objective | UNK7
    }
}
