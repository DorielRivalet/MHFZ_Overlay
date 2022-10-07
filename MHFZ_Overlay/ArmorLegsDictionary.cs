using System.Collections.Concurrent;

namespace Dictionary
{
    /// <summary>
    /// Items dictionary
    /// </summary>
    public class ArmorLegs
    {
        public static ConcurrentDictionary<int, string> ArmorLegIDs = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Initiates this instance.
        /// </summary>
        public static void Initiate()
        { }
    }
}