using System.Collections.Concurrent;

namespace Dictionary
{
    /// <summary>
    /// Items dictionary
    /// </summary>
    public class ArmorHeads
    {
        public static ConcurrentDictionary<int, string> ArmorHeadIDs = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Initiates this instance.
        /// </summary>
        public static void Initiate()
        { }
    }
}