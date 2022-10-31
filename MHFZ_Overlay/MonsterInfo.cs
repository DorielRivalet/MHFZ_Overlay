using System.Collections.Generic;

namespace MHFZ_Overlay
{
    public class MonsterInfo
    {

        public string Name { get; set; }
        public string Title { get; set; }
        public string Weakness { get; set; }
        public string Ailments { get; set; }
        public string Elements { get; set; }
        public Dictionary<string, string> Hitzones { get; set; }
        public Dictionary<string, string> InfoLinks { get; set; }
        public Dictionary<string, string> WeaponMatchups { get; set; }

        public string Description { get; set; }

        public MonsterInfo(string name, string title, string weakness, string ailments, string elements, Dictionary<string, string> infolinks, Dictionary<string, string> hitzones, Dictionary<string, string> matchups, string description)
        {
            Name = name;
            Title = title;
            Weakness = weakness;
            Ailments = ailments;
            Elements = elements;
            Hitzones = hitzones;
            InfoLinks = infolinks;
            WeaponMatchups = matchups;
            Description = description;
            
        }
    }
}
