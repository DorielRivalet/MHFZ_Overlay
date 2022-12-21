using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The rank bands list
    ///</summary>
    public static class RanksBandsList
    {
        public static IReadOnlyDictionary<int, string> RankBandsID { get; } = new Dictionary<int, string>
        {
            {1, "Lower"},
            {2, "Lower"},
            {3, "Lower"},
            {4, "Lower"},
            {5, "Lower"},
            {6, "Lower"},
            {7, "Lower"},
            {8, "Lower"},
            {9, "Lower"},
            {10, "Lower"},
            {11, "Lower/Higher"},
            {12, "Higher"},
            {13, "Higher"},
            {14, "Higher"},
            {15, "Higher"},
            {16, "Higher"},
            {17, "Higher"},
            {18, "Higher"},
            {19, "Higher"},
            {20, "Higher"},
            {26, "HR5"},
            {31, "HR5"},
            {32, "Supremacy 1"}, //needs testing
            {42, "HR5"},
            {46, "Supremacy 2" }, //needs testing. unknown repel+slay uses this
            {53, "G Rank"},//conquest
            {54, "Musou 1"},//lower shiten unknown/disu. thirsty/starving.
            {55, "Musou 2"},//upper shiten disu. shifting/golden.
            {56, "Twinhead 1" },//twinhead rajang / voljang and rajang
            {57, "Twinhead 2" },//twinhead mi ru / white and brown espi / unknown and zeru / rajang and dorag
            {64, "Zenith★1"},
            {65, "Zenith★2"},
            {66, "Zenith★3"},
            {67, "Zenith★4"},
            {70,"Upper Shiten" },//upper shiten Unknown
            {71, "Interception"},
            {72, "Interception"},
            {73, "Interception"}
        };
    }
}