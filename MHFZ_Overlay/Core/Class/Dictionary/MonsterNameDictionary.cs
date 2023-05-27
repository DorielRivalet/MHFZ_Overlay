﻿// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System.Collections.Generic;

namespace Dictionary
{
    ///<summary>
    ///The monster name list
    ///</summary>
    public static class MonsterNameDictionary
    {
        public static IReadOnlyDictionary<int, string> MonsterNameID { get; } = new Dictionary<int, string>
        {
            {0, "None"},//
            {1, "Rathian"},//
            {2, "Fatalis"},//
            {3, "Kelbi"},//
            {4, "Mosswine"},//
            {5, "Bullfango"},//
            {6, "Yian Kut-Ku"},//
            {7, "Lao-Shan Lung"},//
            {8, "Cephadrome"},//
            {9, "Felyne"},//
            {10, "Veggie Elder"},//
            {11, "Rathalos"},//
            {12, "Aptonoth"},//
            {13, "Genprey"},//
            {14, "Diablos"},//
            {15, "Khezu"},//
            {16, "Velociprey"},//
            {17, "Gravios"},//
            {18, "Felyne"},//
            {19, "Vespoid"},//
            {20, "Gypceros"},//
            {21, "Plesioth"},//
            {22, "Basarios"},//
            {23, "Melynx"},//
            {24, "Hornetaur"},//
            {25, "Apceros"},//
            {26, "Monoblos"},//
            {27, "Velocidrome"},//
            {28, "Gendrome"},//
            {29, "Rocks"},//
            {30, "Ioprey"},//
            {31, "Iodrome"},//
            {32, "Poogie"},//
            {33, "Kirin"},//
            {34, "Cephalos"},//
            {35, "Giaprey"},//
            {36, "Crimson Fatalis"},//
            {37, "Pink Rathian"},//
            {38, "Blue Yian Kut-Ku"},//
            {39, "Purple Gypceros"},//
            {40, "Yian Garuga"},//
            {41, "Silver Rathalos"},//
            {42, "Gold Rathian"},//
            {43, "Black Diablos"},//
            {44, "White Monoblos"},//
            {45, "Red Khezu"},//
            {46, "Green Plesioth"},//
            {47, "Black Gravios"},//
            {48, "Daimyo Hermitaur"},//
            {49, "Azure Rathalos"},//
            {50, "Ashen Lao-Shan Lung"},//
            {51, "Blangonga"},//
            {52, "Congalala"},//
            {53, "Rajang"},//
            {54, "Kushala Daora"},//
            {55, "Shen Gaoren"},//
            {56, "Great Thunderbug"},//
            {57, "Shakalaka"},//
            {58, "Yama Tsukami"},//
            {59, "Chameleos"},//
            {60, "Rusted Kushala Daora"},//
            {61, "Blango"},//
            {62, "Conga"},//
            {63, "Remobra"},//
            {64, "Lunastra"},//
            
            {66, "Hermitaur"},//
            {67, "Shogun Ceanataur"},//
            {68, "Bulldrome"},//
            {69, "Anteka"},//
            {70, "Popo"},//
            {71, "White Fatalis"},//
            {72, "Yama Tsukami"},//
            {73, "Ceanataur"},//
            {74, "Hypnocatrice"},//
            {75, "Lavasioth"},//
            {76, "Tigrex"},//
            {77, "Akantor"},//
            {78, "Bright Hypnoc"},//
            {79, "Red Lavasioth"},//
            {80, "Espinas"},//
            {81, "Orange Espinas"},//
            {82, "Silver Hypnoc"},//
            {83, "Akura Vashimu"},//
            {84, "Akura Jebia"},//
            {85, "Berukyurosu"},//
            {86, "Cactus"},//
            {87, "Gorge Object"},//
            {88, "Gorge Rock"},//
            
            {90, "White Espinas"},//
            {91, "Kamu Orugaron"},//
            {92, "Nono Orugaron"},//
            {93, "Raviente"},//
            {94, "Dyuragaua"},//
            
            {96, "Gurenzeburu"},//
            {97, "Burukku"},//
            {98, "Erupe"},//
            {99, "Rukodiora"},//
            {100, "UNKNOWN"},//
            {101, "Gogomoa"},//
            {102, "Kokomoa"},//
            {103, "Taikun Zamuza"},//
            {104, "Abiorugu"},//
            {105, "Kuarusepusu"},//
            
            {107, "Disufiroa"},//
            {108, "Rebidiora"},//
            {109, "Anorupatisu"},//
            {110, "Hyujikiki"},//
            {111, "Midogaron"},//
            {112, "Giaorugu"},//
            
            {114, "Farunokku"},//
            {115, "Pokaradon"},//
            {116, "Shantien"},//
            {117, "Pokara"},//
            {118, "Dummy"},//
            {119, "Goruganosu"},//
            {120, "Aruganosu"},//
            {121, "Baruragaru"},//
            {122, "Zerureusu"},//
            {123, "Gougarf"},//
            {124, "Uruki"},//
            {125, "Forokururu"},//
            {126, "Meraginasu"},//
            {127, "Diorex"},//
            {128, "Garuba Daora"},//
            {129, "Inagami"},//
            {130, "Varusaburosu"},//
            {131, "Poborubarumu"},//
            {132, "Duremudira"},//
            {133, "UNK"},//
            {134, "Felyne"},//
            {135, "Blue NPC"},//
            {136, "UNK"},//
            {137, "Cactus"},//
            {138, "Veggie Elders"},//
            {139, "Gureadomosu"},//
            {140, "Harudomerugu"},//
            {141, "Toridcless"},//
            {142, "Gasurabazura"},//
            {143, "Kusubami"},//
            {144, "Yama Kurai"},//
            {145, "3rd Phase Duremudira"},//
            
            {147, "Deviljho"},//
            {148, "Brachydios"},//
            {149, "Berserk Raviente"},//
            {150, "Toa Tesukatora"},//
            {151, "Barioth"},//
            {152, "Uragaan"},//
            {153, "Stygian Zinogre"},//

            {156, "UNK"},//
            {157, "Egyurasu"},//
            {158, "Voljang"},//
            {159, "Nargacuga"},//
            {160, "Keoaruboru"},//
            {161, "Zenaserisu"},//
            {162, "Gore Magala"},//
            {163, "Blinking Nargacuga"},//
            {164, "Shagaru Magala"},//
            {165, "Amatsu"},//

            {167, "Arrogant Duremudira"},//
            {168, "Rocks"},//
            {169, "Seregios"},//
            {170, "Bogabadorumu"},//
            {171, "Unknown Blue Barrel"},//
            {172, "Blitzkrieg Bogabadorumu"},//
            {173, "Costumed Uruki"},//
            {174, "Sparkling Zerureusu"},//
            {175, "PSO2 Rappy"},//
            {176, "King Shakalaka"}//
        };
    }
}