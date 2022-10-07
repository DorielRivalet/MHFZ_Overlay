using System.Collections.Concurrent;

namespace Dictionary
{
    /// <summary>
    /// Items dictionary
    /// </summary>
    public class ArmorArms
    {
        public static ConcurrentDictionary<int, string> ArmorArmIDs = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Initiates this instance.
        /// </summary>
        public static void Initiate()
        { }
    }
}