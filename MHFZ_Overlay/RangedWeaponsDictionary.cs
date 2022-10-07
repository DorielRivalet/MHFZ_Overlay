using System.Collections.Concurrent;

namespace Dictionary
{
    /// <summary>
    /// Quests dictionary
    /// </summary>
    public class RangedWeapons
    {
        public static ConcurrentDictionary<int, string> RangedWeaponIDs = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Initiates this instance.
        /// </summary>
        public static void Initiate()
        { }
    }
}