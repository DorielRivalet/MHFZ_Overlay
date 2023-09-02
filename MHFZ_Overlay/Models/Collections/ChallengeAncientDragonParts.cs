// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;
using Wpf.Ui.Hardware;

/// <summary>
/// The challenge ancient dragon parts list.
/// </summary>
public static class ChallengeAncientDragonParts
{
    public static ReadOnlyDictionary<int, List<ChallengeAncientDragonPart>> TierParts { get; } = InitializeParts();

    public static ReadOnlyDictionary<int, List<ChallengeAncientDragonPart>> InitializeParts()
    {
        var tierParts = new Dictionary<int, List<ChallengeAncientDragonPart>>
        {
            {
                0, new List<ChallengeAncientDragonPart>
                {
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "The head of the dragon. Presents skull fractures.",
                        ActivePartDescription = "Imposing head, reminiscent of a true dragon's. With three distinct horns atop it, the central horn stands tall and vibrant in yellow, flanked by two shorter, white horns that curve downwards. A visor-like structure conceals any hint of visible eyes, but a fierce maw is fully exposed, exuding a sense of power.",
                        Name = "Cranium",
                        InactivePartImageLink = @"Assets/Icons/Head_Icon_White.xaml",
                        ActivePartImageLink = @"Assets/Icons/AncientDragonCranium.xaml",
                        IsSource = true,
                        Effect = "Unlock the statistics tab.",
                        SynergyEffect = @"Unlock all features of the statistics tab.
Unlock an upgrade for increasing the chance of finding missing Book of Secrets pages.
Unlock an upgrade for increasing the chance of finding an ancient dragon part's scraps.",
                        SourceEffect = "None.",
                        GemsRequiredForScrap = new Dictionary<FrontierMonsterType, int>
                        {
                            { FrontierMonsterType.Other, 1 },
                            { FrontierMonsterType.ElderDragon, 100 },
                            { FrontierMonsterType.Carapaceon, 1 },
                            { FrontierMonsterType.FlyingWyvern, 1 },
                            { FrontierMonsterType.BruteWyvern, 1 },
                            { FrontierMonsterType.PiscineWyvern, 1 },
                            { FrontierMonsterType.BirdWyvern, 1 },
                            { FrontierMonsterType.FangedBeast, 1 },
                            { FrontierMonsterType.Leviathan, 1 },
                            { FrontierMonsterType.FangedWyvern, 1 },
                        },
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "The battle-worn armor of the dragon.",
                        ActivePartDescription = "Encased in grey, steel armor, it forms a protective shell around the core of its being. The armor is particularly concentrated on the chest, back, and limbs, providing a robust defense against adversaries. White marbling patterns adorn parts of the thorax, adding an intricate touch to the fierce creature's appearance.",
                        Name = "Thorax",
                        InactivePartImageLink = @"Assets/Icons/Sturdy_Shell_Icon_White.xaml",
                        ActivePartImageLink = @"Assets/Icons/AncientDragonThorax.xaml",
                        Effect = "Unlock Extreme difficulty and related upgrades.",
                        SynergyEffect = "Gain an extra cart in Easy/Medium/Hard difficulty and two extra carts in Extreme difficulty.",
                        SourceEffect = "Carts from the first 3 squares are not penalized, both in points or in the carts counter going down.",
                        GemsRequiredForScrap = new Dictionary<FrontierMonsterType, int>
                        {
                            { FrontierMonsterType.Other, 1 },
                            { FrontierMonsterType.ElderDragon, 10 },
                            { FrontierMonsterType.Carapaceon, 100 },
                            { FrontierMonsterType.FlyingWyvern, 5 },
                            { FrontierMonsterType.BruteWyvern, 10 },
                            { FrontierMonsterType.PiscineWyvern, 5 },
                            { FrontierMonsterType.BirdWyvern, 10 },
                            { FrontierMonsterType.FangedBeast, 5 },
                            { FrontierMonsterType.Leviathan, 10 },
                            { FrontierMonsterType.FangedWyvern, 10 },
                        },
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "Wings with holes presenting all throughout.",
                        ActivePartDescription = "Massive, membranous wings sprout from the sides, granting it the gift of flight. These wings bear testament to the creature's draconic heritage, allowing it to soar through the skies with an awe-inspiring presence. Their size and structure mirror those of legendary dragons, giving it both a fearsome aspect and unmatched aerial capabilities.",
                        Name = "Wings",
                        InactivePartImageLink = @"Assets/Icons/Wing_Icon_White.xaml",
                        ActivePartImageLink = @"Assets/Icons/AncientDragonWings.xaml",
                        Effect = "Unlock the ability to randomly rearrange cells at a cost.",
                        SynergyEffect = "Unlock the ability to rearrange the weapon bonuses up to 3 cells of your choice per run once, at a cost.",
                        SourceEffect = "Unlock the ability to rearrange up to 10 cells of your choice per run once, without any cost.",
                        GemsRequiredForScrap = new Dictionary<FrontierMonsterType, int>
                        {
                            { FrontierMonsterType.Other, 1 },
                            { FrontierMonsterType.ElderDragon, 25 },
                            { FrontierMonsterType.Carapaceon, 1 },
                            { FrontierMonsterType.FlyingWyvern, 100 },
                            { FrontierMonsterType.BruteWyvern, 1 },
                            { FrontierMonsterType.PiscineWyvern, 1 },
                            { FrontierMonsterType.BirdWyvern, 10 },
                            { FrontierMonsterType.FangedBeast, 1 },
                            { FrontierMonsterType.Leviathan, 1 },
                            { FrontierMonsterType.FangedWyvern, 1 },
                        },
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "Arms with many scars, look like they came from some black dragon.",
                        ActivePartDescription = "A testament to both strength and grace. Muscular and sinewy, these limbs are clad in the same formidable steel armor that encases its body. From shoulder to claw-tipped digits, the forelimbs exude power and purpose. Despite their bulk, they carry an undeniable agility, capable of precise movements and devastating strikes.",
                        Name = "Forelimbs",
                        InactivePartImageLink = @"Assets/Icons/Flipper_Icon_White.xaml",
                        ActivePartImageLink = @"Assets/Icons/AncientDragonForelimbs.xaml",
                        Effect = "Unlock an upgrade for gaining more bingo points with compound interest.",
                        SynergyEffect = "Unlock an upgrade for reducing the cost of upgrades.",
                        SourceEffect = "Unlock the ability to purchase an upgrade for free once.",
                        GemsRequiredForScrap = new Dictionary<FrontierMonsterType, int>
                        {
                            { FrontierMonsterType.Other, 1 },
                            { FrontierMonsterType.ElderDragon, 25 },
                            { FrontierMonsterType.Carapaceon, 10 },
                            { FrontierMonsterType.FlyingWyvern, 1 },
                            { FrontierMonsterType.BruteWyvern, 25 },
                            { FrontierMonsterType.PiscineWyvern, 1 },
                            { FrontierMonsterType.BirdWyvern, 10 },
                            { FrontierMonsterType.FangedBeast, 25 },
                            { FrontierMonsterType.Leviathan, 10 },
                            { FrontierMonsterType.FangedWyvern, 25 },
                        },
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "Legs so badly damaged that you can see the bone of the dragon in some places.",
                        ActivePartDescription = "Providing a stable and mighty foundation, these limbs, clad in the same steel armor as the rest of its body, are capable of propelling the creature forward with incredible force. They allow for agile movement and powerful strikes, making them a crucial asset in combat.",
                        Name = "Hindlegs",
                        InactivePartImageLink = @"Assets/Icons/Leg_Carve_Icon_White.xaml",
                        ActivePartImageLink = @"Assets/Icons/AncientDragonHindlegs.xaml",
                        Effect = "Unlock an upgrade for filling the transcend meter faster.",
                        SynergyEffect = "Unlock an upgrade for reducing the cost of a true transcend.",
                        SourceEffect = @"Unlock an upgrade for decreasing the rate at which the transcend meter drains.
Unlock an upgrade that increases the grace period for obtaining the maximum time score.
Unlock an upgrade for increasing the time score multiplier.",
                        GemsRequiredForScrap = new Dictionary<FrontierMonsterType, int>
                        {
                            { FrontierMonsterType.Other, 1 },
                            { FrontierMonsterType.ElderDragon, 1 },
                            { FrontierMonsterType.Carapaceon, 1 },
                            { FrontierMonsterType.FlyingWyvern, 1 },
                            { FrontierMonsterType.BruteWyvern, 50 },
                            { FrontierMonsterType.PiscineWyvern, 50 },
                            { FrontierMonsterType.BirdWyvern, 1 },
                            { FrontierMonsterType.FangedBeast, 10 },
                            { FrontierMonsterType.Leviathan, 10 },
                            { FrontierMonsterType.FangedWyvern, 10 },
                        },
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A tail that was cut in the middle.",
                        ActivePartDescription = "A sturdy and stout tail extends from its rear, completing its fearsome form. This tail, like the rest of its body, is encased in armor, reinforcing its defensive capabilities. The tail's design is not just for appearance; it contributes to the creature's balance and agility, ensuring that it maintains control even during its most dynamic maneuvers.",
                        Name = "Tail",
                        InactivePartImageLink = @"Assets/Icons/Tail_Icon_White.xaml",
                        ActivePartImageLink = @"Assets/Icons/AncientDragonTail.xaml",
                        Effect = "Unlock the ability to keep a certain amount of your current bingo points after doing a true transcend. A maximum of 20,000 Bingo Points can be transferred if you have 100,000 Bingo Points or more.",
                        SynergyEffect = "Unlock the ability to keep one upgrade in its first level after doing a true transcend, at a cost.",
                        SourceEffect = "Unlock the ability to exchange gauntlet gems for bingo points, and vice versa.",
                        GemsRequiredForScrap = new Dictionary<FrontierMonsterType, int>
                        {
                            { FrontierMonsterType.Other, 1 },
                            { FrontierMonsterType.ElderDragon, 10 },
                            { FrontierMonsterType.Carapaceon, 1 },
                            { FrontierMonsterType.FlyingWyvern, 20 },
                            { FrontierMonsterType.BruteWyvern, 1 },
                            { FrontierMonsterType.PiscineWyvern, 50 },
                            { FrontierMonsterType.BirdWyvern, 10 },
                            { FrontierMonsterType.FangedBeast, 1 },
                            { FrontierMonsterType.Leviathan, 50 },
                            { FrontierMonsterType.FangedWyvern, 1 },
                        },
                    },
                }
            },
            {
                1, new List<ChallengeAncientDragonPart>
                {
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A black colored goo which seems to have been something else eons ago.",
                        ActivePartDescription = "A technological marvel installed by the Ancient Civilization. This advanced component enables the creature to process and manage its vast elemental powers seamlessly. The Cognisphere acts as a bridge between the creature's consciousness and its immense capabilities, allowing for swift transitions between different elements and tactics.",
                        Name = "Cognisphere",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                        IsSource = true,
                        Effect = "Unlock the statistics tab.",
                        SynergyEffect = @"Unlock all features of the statistics tab.
Unlock an upgrade for increasing the chance of finding missing Book of Secrets pages.
Unlock an upgrade for increasing the chance of finding an ancient dragon part's scraps.",
                        SourceEffect = "None.",
                        GemsRequiredForScrap = new Dictionary<FrontierMonsterType, int>
                        {
                            { FrontierMonsterType.Other, 0 },
                            { FrontierMonsterType.ElderDragon, 0 },
                            { FrontierMonsterType.Carapaceon, 0 },
                            { FrontierMonsterType.FlyingWyvern, 0 },
                            { FrontierMonsterType.BruteWyvern, 0 },
                            { FrontierMonsterType.PiscineWyvern, 0 },
                            { FrontierMonsterType.BirdWyvern, 0 },
                            { FrontierMonsterType.FangedBeast, 0 },
                            { FrontierMonsterType.Leviathan, 0 },
                            { FrontierMonsterType.FangedWyvern, 0 },
                        },
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A heart that beats no more.",
                        ActivePartDescription = "The heartbeat of the creature, a pulsating core that sustains its existence. Crafted by the Ancient Civilization, this intricate component ensures that the amalgamation of Elder Dragon parts remains stable. It grants the creature the ability to harness its elemental powers without the risk of losing control, making it a linchpin of the creature's formidable abilities.",
                        Name = "Vital Nexus",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A visible component hidden in the depths of the being.",
                        ActivePartDescription = "Within the depths of the creature's being resides the Invisible Core, a hidden gem of technology that regulates its elemental energies. Imperceptible from the outside, this core works tirelessly to harmonize the conflicting powers within, preventing catastrophic imbalances and maintaining the creature's dominance in battle.",
                        Name = "Invisible Core",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "Claws that lack luster and sharpness.",
                        ActivePartDescription = "At the terminus of the creature's forelimbs are the fearsome True Talonclaws. These lethal appendages are a marvel of design, infused with the very essence of draconic power. Each claw is a masterpiece of organic and synthetic fusion, resembling long, wickedly sharp blades. Coated in a metallic sheen that glints in the light, they hint at their formidable potential. Upon closer inspection, intricate patterns trace along the surface, reflecting the elemental energies they can harness. When unleashed, the True Talonclaws become conduits of destruction. With a swing, they can conjure torrents of fire, surges of electricity, icy shards, or waves of water. Each strike is a symphony of destruction, a manifestation of the dragon's mastery over the elements.",
                        Name = "True Talonclaws",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "Rusted armor, to the point of decay.",
                        ActivePartDescription = "The Daora Plating adorns the creature's body, forming an unyielding shield against all manner of attacks. Named after the dragons that contributed to its creation, this steel armor offers unparalleled protection, allowing the creature to withstand assaults that would shatter lesser opponents.",
                        Name = "Daora Plating",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A component that seems to have lost its fuel.",
                        ActivePartDescription = "The embodiment of the creature's mastery over fire. It manifests as a blazing aura that envelops the dragon, scorching foes that dare to come close. This fierce display of pyrokinetic might strikes fear into the hearts of its adversaries, leaving them vulnerable to the creature's relentless onslaught.",
                        Name = "Hellfire Aegis",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                }
            },
            {
                2, new List<ChallengeAncientDragonPart>
                {
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A component that looks like fragile glass.",
                        ActivePartDescription = "At the core of its being, the Ethereal Matrix exists as a nexus of power. It interconnects the creature's various elemental abilities, allowing for seamless transitions between different forms of devastation. This matrix symbolizes the culmination of the Ancient Civilization's technological prowess, enabling the creature to wield the might of dragons with unparalleled precision and potency.",
                        Name = "Ethereal Matrix",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                        IsSource = true,
                        Effect = "Unlock the statistics tab.",
                        SynergyEffect = @"Unlock all features of the statistics tab.
Unlock an upgrade for increasing the chance of finding missing Book of Secrets pages.
Unlock an upgrade for increasing the chance of finding an ancient dragon part's scraps.",
                        SourceEffect = "None.",
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "You sense that there should be something here, but don't know what.",
                        ActivePartDescription = "The Empyrean Aura radiates from afar, signifying its dominion over various elemental forces. This ethereal glow shifts and shimmers, reflecting the creature's adaptability as it seamlessly transitions between fire, water, thunder, ice, and draconic energy. The Empyrean Aura is both a symbol of its power and a harbinger of imminent destruction. Rumors circulate that this aura also commands the monsters of the Sky Corridor.",
                        Name = "Empyrean Aura",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A tattered cloak.",
                        ActivePartDescription = "Wrapped around the creature's form, the Conquerors' Cloak is a manifestation of its indomitable will. Woven from the threads of victory and conquest, this intangible shroud bolsters the dragon's resolve, empowering it to face any challenge with unwavering determination. It serves as a reminder that the creature is a force to be reckoned with. Should the creature be challenged, enemies will be struck down by black lightning. It is said that those who dare be near the cloak either faints or suffer a worse fate. It can also empower the Vigorous Armament.",
                        Name = "Conquerors' Cloak",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A component that seems to emit white smoke.",
                        ActivePartDescription = "A crucial aspect of the creature's design. This intricate network of jet-black steel and innovation envelops the creature's organic body, fortifying it against the challenges of battle. With a focus on protection, the armament ensures that the amalgamation of Elder Dragon parts remains intact, allowing the creature to endure and prevail against even the most relentless foes. Synergizes well with the Conquerors' Cloak.",
                        Name = "Vigorous Armament",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A component that when touched feels as if your cognition slows down.",
                        ActivePartDescription = "Enhances the dragon's agility, allowing it to move with unparalleled swiftness. This mystical garment bestows the gift of incredible speed upon the creature, enabling it to close distances swiftly or evade attacks with finesse. Its flowing presence is a testament to the creature's prowess in combat.",
                        Name = "Alacrity Mantle",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                    new ChallengeAncientDragonPart
                    {
                        InactivePartDescription = "A component that smells pretty bad.",
                        ActivePartDescription = "A manifestation of the dragon's mastery over the elements. With each exhalation, it can unleash torrents of elemental energy, shaping them into devastating attacks. This breath is a deadly tool, enabling the creature to unleash destructive forces upon its adversaries, leaving chaos in its wake.",
                        Name = "Spirit Breath",
                        InactivePartImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                    },
                }
            },
        };

        // Set the NextSynergyPart property for each part to establish the circular synergy relationship
        for (int i = 0; i < tierParts.Count; i++)
        {
            for (int j = 0; i < tierParts[i].Count; i++)
            {
                int nextIndex = (j + 1) % tierParts[i].Count; // Use modulus to loop back to 0 at the end
                tierParts[i][j].NextSynergyPart = tierParts[i][nextIndex];
            }
        }

        // Validate that there is exactly one source part per tier
        foreach (var tier in tierParts.Values)
        {
            int sourcePartCount = tier.Count(part => part.IsSource);
            if (sourcePartCount != 1)
            {
                throw new InvalidOperationException($"Tier contains {sourcePartCount} source parts. There should be exactly one source part per tier.");
            }
        }


        return new ReadOnlyDictionary<int, List<ChallengeAncientDragonPart>>(tierParts);
    }
}
