using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Dictionary
{
    ///<summary>
    ///The players list
    ///</summary>
    public static class PlayersList
    {
        public static IReadOnlyDictionary<int, List<string>> PlayerIDs { get; } = new Dictionary<int, List<string>>
        {
            // No Player
            {0, new List<string>{DateTime.UnixEpoch.Date.ToString(), "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/icon/transcend.png", "None","NoGuild","NoServer","Unknown","World"}},
            // Local Player
            {1, new List<string>{DateTime.Now.Date.ToString(), "https://raw.githubusercontent.com/DorielRivalet/mhfz-overlay/main/img/icon/transcend.png", "HunterName","GuildName","ServerName","Unknown","World"}},
        };
    };
}