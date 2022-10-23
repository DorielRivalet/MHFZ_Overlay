namespace MHFZ_Overlay
{
    public class Monster
    {

        public string Name { get; set; }
        public int ID { get; set; }
        public int Hunted { get; set; }

        public bool IsLarge { get; set; }
        public string MonsterImage { get; set; }

        public Monster(int id, string name, string image, int hunted, bool islarge = false)
        {
            ID = id;
            Name = name;
            MonsterImage = image;
            Hunted = hunted;
            IsLarge = islarge;
        }
    }
}
