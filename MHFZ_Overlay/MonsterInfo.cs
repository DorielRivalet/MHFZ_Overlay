using System.Collections.Generic;

namespace MHFZ_Overlay
{
    public class MonsterInfo
    {

        public string Name { get; set; }
        public Dictionary<string, string> WeaponMatchups { get; set; }
        public string Description { get; set; }
        public string FeriasLink { get; set; }

        public MonsterInfo(string name, string feriaslink, Dictionary<string, string> matchups, string description)
        {
            Name = name;
            FeriasLink = feriaslink;
            WeaponMatchups = matchups;
            Description = description;
        }
    }
}
