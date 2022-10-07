using System.Collections.Concurrent;

namespace Dictionary
{
    /// <summary>
    /// Quests dictionary
    /// </summary>
    public class MeleeWeapons
    {
        public static ConcurrentDictionary<int, string> MeleeWeaponIDs = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Initiates this instance.
        /// </summary>
        public static void Initiate()
        { }
    }
}