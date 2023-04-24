namespace MHFZ_Overlay.UI.Class
{
    //TODO: ORM
    public class GearCompendium
    {
        public int MostUsedWeaponType { get; set; }
        public int TotalUniqueArmorPiecesUsed { get; set; }
        public int TotalUniqueWeaponsUsed { get; set; }
        public int TotalUniqueDecorationsUsed { get; set; }
        public int MostCommonDecoration { get; set; }
        public int MostCommonDecorationID { get; set; }
        public int LeastUsedArmorSkill { get; set; }
    }
}
