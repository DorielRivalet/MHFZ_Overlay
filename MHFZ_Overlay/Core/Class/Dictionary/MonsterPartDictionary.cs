using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The monster image list
    ///<br>TODO: missing labels</br>
    ///</summary>
    public static class MonsterPartDictionary
    {
        public static IReadOnlyDictionary<List<int>, List<string>> MonsterPartID { get; } = new Dictionary<List<int>, List<string>>
        {
            {new List<int>{0,10,18,23,29,32,57,63,86,87,88,118,124,133,134,135,136,137,138,156,168,171,173,175,176,19,24,56},
                new List<string>{"None","None","None","None","None","None","None","None","None","None"}},
             // Rath skeleton
            {new List<int>{1,11,14,15,26,37,41,42,43,44,45,49,80,81,90,96,100,109,122,126,130,139,174},
                new List<string>{"Body","L. Wing","R. Wing","L. Leg","R. Leg","Neck","Head","Tail Tip","Tail","None"}},
             // Flying Wyvern Skeleton 2
            {new List<int>{17,22,47},
                new List<string>{"Body","L. Wing","R. Wing","L. Leg","R. Leg","Head","Belly","Tail Tip","Tail","None"}},
            //Rex Wyvern Skeleton 1
            {new List<int>{76,77,110,127,151,159,163},
                new List<string>{"Head","Body","Back","Tail","L. Wing","L. Leg","R. Wing","R. Leg","Tail Tip","None"}},
            //Rex Wyvern Skeleton 2
            {new List<int>{89,169,141},
                new List<string>{"Head","Body","L. Wing","R. Wing","L. Leg","R. Leg","Tail","Back","Tail Tip","None"}},
            //Rex Wyvern Skeleton 3
            {new List<int>{113,94},
                new List<string>{"Head","L. Wing","R. Wing","Hindlegs","Body","Belly","Tail","Tail Tip","Back","None"}},
            //Big Rex Wyvern Skeleton 1
            {new List<int>{131},
                new List<string>{"Head","Body","Hindlegs","Tail Tip","R. Foreleg","L. Foreleg","R.Forewing","L.Forewing","Back","None"}},
            //Odibatorasu
            {new List<int>{106},
                new List<string>{"L. Foreleg","R. Foreleg","L. Hindleg","R. Hindleg","Back","Belly","Body","Head","Back","None"}},
            //Keoaruboru
            {new List<int>{160},
                new List<string>{"Head","Body","Back","L. Hindleg","R. Hindleg","L. Foreleg","R. Foreleg","Tail","Tail Tip","None"}},
            //Elder Skeleton (Teostra, Kushala, Chameleos)
            {new List<int>{54,59,60,64,65,99,107,108,128,140,150},
                new List<string>{"Head","Body","Belly","Forelegs","Hindlegs","Wings","Tail","Back","Tail Tip","None"}},
            //Guanzorumu
            {new List<int>{154},
                new List<string>{"Head","Wings","L. Foreleg","R. Foreleg","L. Hindleg","R. Hindleg","Tail","Body","Tail Tip","None"}},
            //Duremudira
            {new List<int>{132,145,167},
                new List<string>{"Head","Wings","Tail","Hindlegs","Belly","Forelegs","Body","Back","Tail Tip","None"}},
            //Fatalis
            {new List<int>{2,36,71},
                new List<string>{"Chest","Belly","L. Leg","R. Leg","Neck","Face","Tail/Back","Wings","Tail Tip","None"}},
            //Magala
            {new List<int>{162,164},
                new List<string>{"Head","Horns","Body","Wings","Forelegs","L. Hindleg","R. Hindleg","Tail","Tail Tip","None"}},
            //Hervibore Skeleton
            {new List<int>{3,4,5,12,25,68,69,70,97,98},
                new List<string>{"Head","None", "None", "None", "None", "None", "None", "None", "None", "None"}},
            //Kirin
            {new List<int>{33},
                new List<string>{"Body","None", "None", "None", "None", "None", "None", "None", "None", "None"}},
            //Bird Wyvern Skeleton
            {new List<int>{6,20,38,39,40,74,78,82,114},
                new List<string>{"Body","L. Wing","R. Wing","L. Leg","R. Leg","Neck","Head","Tail","Tail Tip","None"}},
            //Bird Wyvern Skeleton 2
            {new List<int>{13,16,27,28,30,31,35},
                new List<string>{"Head","None", "None", "None", "None", "None", "None", "None", "None", "None"}},
            //Fanged Beast Monkey Skeleton
            {new List<int>{51,53,61,62},
                new List<string>{"Head","R. Limbs","L. Limbs","Body","Tail", "None", "None", "None", "Chest", "None"}},
            //Gogomoa, Kokomoa
            {new List<int>{101,102},
                new List<string>{"Head","Body","L. Foreleg","R. Foreleg","L. Hindleg","R. Hindleg","Tail", "None", "None","None"}},
            //Gougarf
            {new List<int>{123},
                new List<string>{"Head","R. Hindleg","L. Hindleg","Body","Tail","L. Foreleg","R. Foreleg","Back","Tail Tip","None"}},
            //Congalala
            {new List<int>{52},
                new List<string>{"Head","Body","Hindlegs","Forelegs","Tail","None", "None", "None", "None", "None"}},
            //Voljang
            {new List<int>{158},
                new List<string>{"Head","R. Foreleg","L. Foreleg","Body","R. Hindleg","L. Hindleg","Tail","Back","Tail Tip","None"}},
            //Brute Wyvern Skeleton 1
            {new List<int>{104,112},
                new List<string>{"Head","R. Leg","L. Leg","Arms","?","Torso","?","Tail","None","None"}},
            // Brute Wyvern Skeleton 2
            {new List<int>{147,155},
                new List<string>{"Head","Chest","Neck","Arms","R. Leg","L. Leg","Tail","None","Tail Tip","None"}},
            // Brute Wyvern Skeleton 3
            {new List<int>{142},
                new List<string>{"Head","Belly","L. Arm","R. Arm","L. Leg","R. Leg","Back","Tail","Tail Tip","None"}},
            // Brute Wyvern Skeleton 4
            {new List<int>{148},
                new List<string>{"Head","Belly","L. Arm","R. Arm","L. Leg","R. Leg","Tail","Back","Tail Tip","None"}},
            // Brute Wyvern Skeleton 5
            {new List<int>{152},
                new List<string>{"Head","Body","Belly","Arms","R. Leg","L. Leg","Tail","Back","Tail Tip","None"}},
            // Piscine Wyvern Skeleton
            {new List<int>{8,21,34,46,75,79,119,120},
                new List<string>{"Body","L. Fin","R. Fin","L. Leg","R. Leg","Neck","Head","Tail","Tail Tip","None"}},
            // Leviathan Skeleton
            {new List<int>{105,115,117,121},
                new List<string>{"Head","Body","R. Foreleg","L. Foreleg","Tail","R. Hindleg","L. Hindleg","Back","Tail Tip","None"}},
            // Amatsu
            {new List<int>{121},
                new List<string>{"Head","Body","Neck","L. Wing","R. Wing","Hindlegs","Tail","Back","Tail Tip","None"}},
            // Shantien
            {new List<int>{116},
                new List<string>{"Head","Body","L. Foreleg","R. Foreleg","Hindlegs","Tail","?","?","?","None"}},
            // Fanged Wyvern Skeleton
            {new List<int>{146,153,166},
                new List<string>{"Head","Body","Back","L. Foreleg","R. Foreleg","L. Hindleg","R. Hindleg","Tail","Tail Tip","None"}},
            // Inagami
            {new List<int>{51,53,61,62},
                new List<string>{"Head","Body","R. Foreleg","L. Foreleg","R. Hindleg","L. Hindleg","Tail","?","?","None"}},
            // Fanged Beast Wolf Skeleton
            {new List<int>{91,92,111},
                new List<string>{"Head","Back","Body","Forelegs","Hindlegs","Tail","None", "None", "None", "None"}},
            // Carapaceon Skeleton
            {new List<int>{48,66,67,73},
                new List<string>{"Head","Body","Skull","L. Legs","R. Legs","L. Claw","R. Claw","Arms","Feeler","None"}},
            // Scorpion Skeleton
            {new List<int>{51,53,61,62},
                new List<string>{"Head","R. Claw","L. Claw","Legs","Body","Tail","Crystal","?","?","None"}},
            // Flying Wyvern Skeleton 2
            {new List<int>{85,95,161},
                new List<string>{"Head","Body","L. Wing","R. Wing","Legs","R.Wingclaw","L.Wingclaw","Tail","Sub Tails","None"}},
            // Forokururu
            {new List<int>{125},
                new List<string>{"Head","Body","L. Wing","R. Wing","L. Leg","R. Leg","Belly","Tail","Throat","None"}},
            // Elder Skeleton 2
            {new List<int>{7,50},
                new List<string>{"Head","None","Forelegs","Tail/Leg","?","Back","Chest","Body","None","None"}},
            // Elder Skeleton 3
            {new List<int>{58,72,144},
                new List<string>{"?","?","?", "?", "?", "?", "?", "?", "?", "?"}},
            // Shen Gaoren
            {new List<int>{55},
                new List<string>{"Head","Body","Skull","L. Foreleg","L. Hindleg","R. Foreleg","R. Hindleg","Claws","None","None"}},
            // Taikun Zamuza
            {new List<int>{103},
                new List<string>{"Body","L. Claw", "R. Claw", "L. Legs","None", "R. Legs", "None","Tail", "None", "None"}},
            // Raviente
            {new List<int>{93},
                new List<string>{"?","?","Body","?","Shell","Tail", "None", "None", "None", "None"}},
            // Berserk Raviente
            {new List<int>{149},
                new List<string>{"Horn","Head","Neck","Shell","Body","Tail","None","Crystal","Crystal","None"}},
            // Bogabadorumu
            {new List<int>{170,172},
                new List<string>{"Head","Body","Belly","L. Arm","R. Arm","L. Leg","R. Leg","Tail","Tail Tip","None"}},
        };
    }
}