using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The discord servers list
    ///</summary>
    public static class DiscordServersList
    {
        public static IReadOnlyDictionary<long, string> DiscordServerID { get; } = new Dictionary<long, string>
        {
            {0, "Local"},
            {932246672392740917, "Monster Hunter Frontier: Renewal" },
            {937230168223789066, "Rain Frontier Server" },
            {759022449743495170, "Monster Hunter [Ancient_Warriors]" },
            {973963573619486740, "MezeLounge" },
            {288170871908990976, "Hunsterverse"},
            {967058504403808356, "Arca" }
        };
    };
}