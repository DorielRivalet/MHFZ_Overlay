using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The sharpness list
    ///</summary>
    public static class SharpnessList
    {
        public static IReadOnlyDictionary<int, string> SharpnessID { get; } = new Dictionary<int, string>
        {
            {0, "Red"},
            {1, "Orange"},
            {2, "Yellow"},
            {3, "Green"},
            {4, "Blue"},
            {5, "White"},
            {6, "Purple"},
            {7, "Cyan"}
        };
    };
}