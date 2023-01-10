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
            // TODO: External players
            // The creation dates are from youtube channel creation date or known date they joined frontier.
            // the gender are the characters most recent and prominent gender
            //Doriel
            {2, new List<string>{"2019-09-05","https://yt3.ggpht.com/RbjaWxFESqX18_NYoWnRATISLZH0U94oG00jp-sTQ9iSciKazxgRRC8ecg3RXg4yI31kfXySQw=s176-c-k-c0x00ffffff-no-rj-mo","Doriel","Unknown","Dancing Crab","Male","Argentina"}},
            //TOMOTAKA
            {3, new List<string>{"2017-04-03","https://yt3.googleusercontent.com/ytc/AMLnZu8z2zQVY19h79QdJlxhX4c2_XCVYqvlcAso8ztv=s176-c-k-c0x00ffffff-no-rj-mo","TOMOTAKA","Unknown","CAPCOM ONLINE GAMES Japan","Male","Japan"}},
            // Hygogg
            {4, new List<string>{"2006-12-31","https://yt3.googleusercontent.com/ytc/AMLnZu9NROoqVSW4_lbdj2eJl1TSpF7u5_u-E8nmeE0G=s176-c-k-c0x00ffffff-no-rj-mo","Hygogg","Unknown", "CAPCOM ONLINE GAMES Japan","Male","Japan"}},
            // Forest
            {5, new List<string>{"2010-08-10","https://yt3.googleusercontent.com/ytc/AMLnZu9Eb2x9H48BeN5ivpOXHfDAsMU5ed00HNc7spjT-A=s176-c-k-c0x00ffffff-no-rj-mo","Forest","Unknown","CAPCOM ONLINE GAMES Japan","Female","Japan"}},
            // ZABON
            {6, new List<string>{"2017-01-17","https://yt3.googleusercontent.com/ytc/AMLnZu_vKMjsfEFdag54mw86yMGFQDF_KU8IKiwscR4o=s176-c-k-c0x00ffffff-no-rj-mo","ZABON","Unknown","CAPCOM ONLINE GAMES Japan","Female","Japan"}},
            // Jun N
            {7, new List<string>{"2013-08-30","https://yt3.googleusercontent.com/ytc/AMLnZu_lgju1RLfdoHwZaBpCVXlqbaI9s5ZDVpxWB8zUog=s176-c-k-c0x00ffffff-no-rj-mo","Jun","Unknown","CAPCOM ONLINE GAMES Japan","Female","Japan"}},
            // Kairi
            {8, new List<string>{"2019-12-27","https://yt3.googleusercontent.com/ytc/AMLnZu_qx9rv53gJmvds8rE2BCbYQ3AUPbz0PBQEdMEJ=s176-c-k-c0x00ffffff-no-rj-mo","Kairi","Unknown","Local","Female","World"}},
            // Nerscylia
            {10, new List<string>{"2022-08-29","https://yt3.ggpht.com/2LsZGiRbTZ6P64X4_H7B6rVhBvvONEGspYqKOD-RnEWg0ARN2s56CgY9MyHkwGqxMAWS_R64ZKE=s176-c-k-c0x00ffffff-no-rj-mo","Nerscylia","Unknown","Unknown","Male","World"}},
            
            // missing sns, gs, hammer, hh, lbg, hbg, bow
        };
    };
}