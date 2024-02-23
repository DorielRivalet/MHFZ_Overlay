// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// The area icon list.
/// </summary>
public static class AreaIcons
{
    public static ReadOnlyDictionary<List<int>, string> AreaIconID { get; } = new (new Dictionary<List<int>, string>
    {
        // Loading
        { new List<int> { 0 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/entrance.png" },

        // Jungle areas
        { new List<int> { 1, 2, 3, 4, 5, 18, 19, 22, 23, 26, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 212, 213 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/jungle.png" },

        // Snowy mountain areas
        { new List<int> { 6, 15, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 218, 219 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/snowy_mountains.png" },

        // Desert areas
        { new List<int> { 7, 24, 45, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 214, 215 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/desert.png" },

        // Volcano areas
        { new List<int> { 8, 27, 58, 59, 60, 61, 62, 63, 64, 65, 74, 161, 162, 163, 164, 165, 166, 167, 169, 216, 217, 220, 221, 222, 223 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/volcano.png" },

        // Swamp areas
        { new List<int> { 9, 16, 29, 44, 67, 68, 69, 70, 71, 72, 73, 75, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/swamp.png" },

        // Forest and Hills areas
        { new List<int> { 21, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/forest_and_hills.png" },

        // Great Forest areas
        { new List<int> { 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/great_forest.png" },

        // Highlands areas
        { new List<int> { 247, 248, 249, 250, 251, 252, 253, 254, 255, 302, 303, 304, 305, 306, 307, 308 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/highlands.png" },

        // Tidal Island areas
        { new List<int> { 322, 323, 324, 325, 326, 327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/tidal_island.png" },

        // Polar Sea areas
        { new List<int> { 345, 346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/polar_sea.png" },

        // Flower Field areas
        { new List<int> { 361, 362, 363, 364, 365, 366, 367, 368, 369, 370, 371, 372 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/flower_fields.png" },

        // Sky Corridor / Tower
        { new List<int> { 390, 391, 392, 393, 394, 415, 416 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/tower.png" },

        // Duremudira Areas
        { new List<int> { 399, 414 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/dure.gif" },

        // White Lake
        { new List<int> { 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/white_lake.png" },

        // Painted Falls areas
        { new List<int> { 423, 424, 425, 426, 427, 428, 429, 430, 431, 432, 433, 434, 435, 436 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/painted_falls.png" },

        // road
        { new List<int> { 459 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/rengoku.png" },

        // Gorge areas
        { new List<int> { 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/gorge.png" },

        // Mezeporta
        { new List<int> { 200, 397 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/cattleya.png" },

        // my houses
        { new List<int> { 173, 174, 175 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/my_house.png" },

        // hairdresser
        { new List<int> { 201 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/tent.png" },

        // guild halls
        { new List<int> { 202, 203, 204 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/guild_hall.png" },

        // my tore / poogie farm
        { new List<int> { 205 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/my_tore.png" },

        // bars
        { new List<int> { 210, 211 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/rasta_bar.png" },

        // caravan / pallone
        { new List<int> { 256, 260, 261, 262, 263 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/pallone_caravan.png" },

        // blacksmith
        { new List<int> { 257 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/blacksmith.png" },

        // gallery
        { new List<int> { 264 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/my_gallery.png" },

        // guuku farm/garden
        { new List<int> { 265 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/my_garden.png" },

        // halk area
        { new List<int> { 283 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/my_support.png" },

        // PvP room
        { new List<int> { 286 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/pvp.png" },

        // sr rooms
        { new List<int> { 340, 341 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/my_missions.png" },

        // diva halls/fountain
        { new List<int> { 379, 445 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/diva_fountain.png" },

        // MezFes areas
        { new List<int> { 462, 463, 464, 465, 466, 467, 468, 469 }, @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/area/mezfes.png" },
    });
}
