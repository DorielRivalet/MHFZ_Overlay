namespace MHFZ_Overlay
{
    public class Monster
    {

        public string Name { get; set; }
        public int ID { get; set; }
        public int Hunted { get; set; }
        public string MonsterImage { get; set; }

        public Monster(int id, string name, string image, int hunted)
        {
            ID = id;
            Name = name;
            MonsterImage = image;
            Hunted = hunted;
        }
    }
}
