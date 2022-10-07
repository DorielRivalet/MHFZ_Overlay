using System.Collections.Concurrent;

namespace Dictionary
{
    /// <summary>
    /// Items dictionary
    /// </summary>
    public class ArmorWaists
    {
        public static ConcurrentDictionary<int, string> ArmorWaistIDs = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Initiates this instance.
        /// </summary>
        public static void Initiate()
        { }
    }
}