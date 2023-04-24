using System.Text.Json.Serialization;

namespace MHFZ_Overlay.Core.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GachaCardTypes : uint
    {
        Monster,
        Endemic_Life,
        Achievement,
        Item,
        Gear,
        Character,
        Location,
        Special
    }

    public enum RankTypes : uint
    {
        Low_Rank,
        High_Rank,
        G_Rank,
        Zenith_Rank
    }

}
