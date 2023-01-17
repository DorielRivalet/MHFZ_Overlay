﻿using Microsoft.VisualBasic;
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
            {0, new List<string>{DateTime.UnixEpoch.Date.ToString(), "None","NoGuild","0","Unknown","Japan"}},
            // Local Player
            {1, new List<string>{DateTime.Now.Date.ToString(), "HunterName","GuildName","0","Unknown","Japan"}},
        };
    };
}