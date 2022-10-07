using System.Collections.Concurrent;

namespace Dictionary
{
    /// <summary>
    /// Items dictionary
    /// </summary>
    public class ArmorChests
    {
        public static ConcurrentDictionary<int, string> ArmorChestIDs = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Initiates this instance.
        /// </summary>
        public static void Initiate()
        { }
    }
}