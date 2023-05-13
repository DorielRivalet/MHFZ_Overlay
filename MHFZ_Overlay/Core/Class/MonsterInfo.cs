using System.Collections.Generic;

namespace MHFZ_Overlay
{
    /// <summary>
    /// Unaffected by player stats
    /// </summary>
    public class MonsterInfo
    {

        public string Name { get; set; }
        public Dictionary<string, string> WeaponMatchups { get; set; }
        public string WikiLink { get; set; }
        public string FeriasLink { get; set; }

        public MonsterInfo(string name, string feriaslink, Dictionary<string, string> matchups, string wikilink)
        {
            Name = name;
            FeriasLink = feriaslink;
            WeaponMatchups = matchups;
            WikiLink = wikilink;
        }
    }
}
