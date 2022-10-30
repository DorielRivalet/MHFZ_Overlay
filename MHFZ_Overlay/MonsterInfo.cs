using System.Collections.Generic;

namespace MHFZ_Overlay
{
    public class MonsterInfo
    {

        public string Name { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
        public string Weakness { get; set; }
        public string Ailments { get; set; }
        public string Elements { get; set; }
        public Dictionary<string, string> Hitzones { get; set; }
        public string InfoLink { get; set; }

        public MonsterInfo(int id, string name, string title, string weakness, string ailments, string elements, string infolink, Dictionary<string, string> hitzones)
        {
            ID = id;
            Name = name;
            Title = title;
            Weakness = weakness;
            Ailments = ailments;
            Elements = elements;
            Hitzones = hitzones;
            InfoLink = infolink;
        }
    }
}
