# Monster Hunter Frontier Z Overlay

[![GitHub all releases](https://img.shields.io/github/downloads/DorielRivalet/mhfz-overlay/total?style=for-the-badge)](#installation)

[![Preview](https://res.cloudinary.com/marcomontalbano/image/upload/v1664306592/video_to_markdown/images/youtube--_6hHiRHTt_U-c05b58ac6eb4c4700831b2b3070cd403.jpg)](https://youtu.be/_6hHiRHTt_U "Preview")

[![Build Status](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2FDorielRivalet%2Fmhfz-overlay%2Fbadge%3Fref%3Dmain&style=flat)](https://actions-badge.atrox.dev/DorielRivalet/mhfz-overlay/goto?ref=main)
![CircleCI](https://img.shields.io/circleci/build/github/DorielRivalet/mhfz-overlay?label=CircleCI&style=flat)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/DorielRivalet/mhfz-overlay?style=flat)
![GitHub repo size](https://img.shields.io/github/repo-size/DorielRivalet/mhfz-overlay?style=flat)

<div align = center>

---

**[<kbd> <br> :rocket: Install <br> </kbd>](#installation)** 
**[<kbd> <br> 📘 Hotkeys<br> </kbd>](#hotkeys)** 
**[<kbd> <br> 🕹 Features <br> </kbd>](#features)** 

---
</div>

## About

This project aims to provide a simple, customizable overlay for Monster Hunter Frontier Z on Windows, with the added bonus of Discord Rich Presence integration. The overlay allows players to keep track of their in-game stats and progress, as well as providing a convenient way to access various tools and resources.

The overlay is highly configurable, with a wide range of options available to suit the needs of individual players and speedrunners alike. It is also constantly being updated and improved, so be sure to check back for the latest features and fixes.

We hope you find this overlay useful and enjoyable, and we welcome any feedback or suggestions you may have. Happy hunting!

[This project is directly inspired from the overlay created by *suzaku01*](https://github.com/suzaku01/mhf_displayer)

[The theme and color palette used for the application is *Catppuccin Mocha*](https://github.com/catppuccin/catppuccin)

[The design and icons used in this project are part of the *Material Design Icons* and related components](https://fonts.google.com/icons)

The fonts used is the in-game one, *MS Gothic*, and Source Code Pro for monospaced. This project also uses Font Awesome's fonts.

## Requirements

- [.NET Desktop Runtime 6.0 x64](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.7-windows-x64-installer)
- [.NET Desktop Runtime 6.0 x86](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.8-windows-x86-installer)

## Installation

1. [Get **ALL** of the requirements](#requirements)
2. [Download the latest version from the *Releases*](https://github.com/DorielRivalet/mhfz-overlay/releases/latest/download/Releases.7z)
3. Make sure Windows or your antivirus did not delete the files (because it reads the games memory, Windows might detect it as a trojan, so you might have to get it out of quarantine)
4. Unzip it anywhere. Run `mhfz-overlaySetup.exe` **as Administrator**
5. [Bonk monsters!](https://c.tenor.com/60Tr3Zeg6RkAAAAd/fumo-bonk.gif)
6. [Be sure to leave some feedback here!](https://forms.gle/hrAVWMcYS5HEo1v7A)

[View CHANGELOG.md](https://github.com/DorielRivalet/mhfz-overlay/blob/main/CHANGELOG.md)

[View Release Statistics](https://somsubhra.github.io/github-release-stats/?username=DorielRivalet&repository=mhfz-overlay&page=1&per_page=30)

## Hotkeys

- `Shift+F1` Open Configuration
- `Shift+F5` Restart Overlay
- `Shift+F6` Exit

**It's recommended to start the overlay when you are done loading into Mezeporta.**

If the overlay doesn't seem to load values properly, restart it. If that didn't fix the issue, [please send information here](https://github.com/DorielRivalet/mhfz-overlay/issues).

Additionally, if information from the overlay is wrong or inaccurate (e.g. monster parts labels), feel free to send an issue.

If the monster HP shown is less than what its actual values should be, restart both the game and the overlay. If the HP shows 0/1 then change area for it to load. If issue still occurs, disable Effective HP, otherwise send a bug report if there isn't already one.

## Features

- [x] Monster Effective HP Bars (*e.g.* Burning Freezing Elzelion's 1,000,000 HP!)

![Monster HP Bars](./demo/hp1.png)
![Monster HP Bars](./demo/hp2.png)

- [x] Sharpness Numbers (colorized by current sharpness tier!)

![Sharpness Numbers](./demo/sharpness1.png)
![Sharpness Numbers](./demo/sharpness2.png)
![Sharpness Numbers](./demo/sharpness3.png)

- [x] Quest Timer (Two modes: elapsed time and time left. Down to the centiseconds in accuracy!)
- [x] Hit Count (counts *Reflect*, *Stylish Up*, Heatblade, *Fencing+2* and more!)

![Player Stats](./demo/playerstat1.png)

- [x] Player True Raw (current highest value shown in red!)

![Player Attack](./demo/playeratk1.png)

- [x] Monster Stats (attack multiplier, defense rate and size!)

![Monster Stats](./demo/monsterstat1.png)

- [x] Monster Status Ailments (Poison, Sleep, Paralysis, Blast, Stun!)

![Monster Ailments](./demo/ailments1.png)

- [x] Monster Body Parts (up to 10 parts!)

![Monster Body Parts](./demo/monsterparts1.png)

- [x] Damage Numbers (dynamic colors and size!)

![Damage Numbers](./demo/damagenumbers1.png)

- [x] [Discord Rich Presence](#how-to-enable-discord-rich-presence) (custom monster icons, colored weapons, quest tier, current area, [speedrun mode, zen mode](#how-to-enable-speedrun--zen-modes), and more!)

![Discord Rich Presence](./demo/discord5.png)
![Discord Rich Presence](./demo/discord10.png)

![Discord Rich Presence](./demo/discord9.png)

![Discord Rich Presence](./demo/discord9.gif)

## Configuration Preview

These images don't show everything, find out what's missing by pressing `Shift+F1`!

### Hunter Sets (Text)

![Config1](./demo/config1.png)

### Hunted Logs

![Config2](./demo/config2.png)

### Monster Speedruns, Hitzones and Wiki

![Config3](./demo/config3.png)

### Ferias

![Config4](./demo/config4.png)

### Guild Card

![Config5](./demo/config5.png)

### Hunter Sets (Image)

![Config6](./demo/config6.png)

### Damage Calculator

![Config6](./demo/config7.png)

## Bugs

- Monster Infos are sometimes outside of the screen (if they don't show at all even if you open the config menu, this is probably your issue)
- With Monster EHP enabled, if you cart, the max EHP turns into the current EHP, along with some other info max values
- Sometimes when exiting Drag and Drop the monster HP information disappears
- Spawning in the same area as the monster doesn't load the information properly. Fix: re-enter area
- Duremudira/Road/Raviente HP not showing. Fix: enable *Always Show Monster Info*, load another quest showing the HP bars (not just the numbers), then retry.
- Monster size values not shown correctly
- Monster HP values are less than the actual values when not loading properly
- Damage numbers over 1000 not working
- Yamas and Berukyurosu information not working

Fully reinstalling the game or .NET dependencies may fix some bugs.

Starting the overlay may take a bit of time because of the dictionaries (*i.e.* **all** in-game gear and items) and other features.

Press `Alt+Enter` twice if your screen resolution got lowered.

If the HP shows 0/1 then change area for it to load.

[Check more bugs here](https://github.com/DorielRivalet/mhfz-overlay/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

## Features not yet implemented

- Choose which player to load data from
- Auto detect which player is playing
- Selecting monsters for body parts and monster status
- Fix max HP for Road
- Allow lock-on to be used to select monsters
- Overlay font options
- Add shortcut for saving
- Damage graph
- Weapon stats import/export
- Quest stats import/export
- Monster stats import/export
- Automatically set default positions according to screen resolution
- Global damage number labels
- Attach user interface to game window option
- Sound effects
- Sharpness graph
- Language options
- PvP addresses
- Handle multiple objectives information
- Zenith information in Road
- Settings import/export
- Raviente Support Part Info
- Detect UL/HC
- Guild Pugi address
- Performance improvements
- Armor Set Website links
- Sky Corridor
- Drag and Drop multiple selection
- Sharpness tables
- Gear rarity colors in hunter info stats

[Check more possible future features here](https://github.com/DorielRivalet/mhfz-overlay/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

## How to Enable Discord Rich Presence

- In Discord, My Account -> Activity Privacy -> Check "Display current activity as a status message"

![Discord](./demo/discord1.png)

- [Discord Developer Portal](https://discord.com/developers/applications) -> New Application -> Name it "MONSTER HUNTER FRONTIER Z"

![Discord](./demo/discord2.png)

- In Developer Portal, General Information -> Copy Application ID

![Discord](./demo/discord3.png)

- In Overlay Settings, Paste into Overlay Settings Discord Rich Presence Application/Client ID (The ID also shows up in OAuth2 section as Client ID)

![Discord](./demo/discord4.png)

![Discord Rich Presence](./demo/discord6.gif)

## How to Enable Speedrun & Zen Modes

- Speedrunner Mode: Disable **everything** (Monster Icon, Quest Timer and Discord Rich Presence optional)

![Discord Rich Presence](./demo/discord7.png)

- Zen Mode: Disable **everything** (Monster Icon and Discord Rich Presence optional)

![Discord Rich Presence](./demo/discord8.png)

~~Congrats, now you won't be accused of cheating~~

## How to Manually Update with Update.exe

`update.exe --update https://www.github.com/DorielRivalet/mhfz-overlay/releases/download/ENTER VERSION NUMBER (E.G. v0.6.4)`

## How to Uninstall

1. Go to Apps & Features
2. Search Monster Hunter Frontier Z Overlay
3. Click Uninstall. You can also delete the Desktop shortcut and Start Menu icon after uninstalling.

## Changelog

[CHANGELOG](https://github.com/DorielRivalet/mhfz-overlay/blob/main/CHANGELOG.md)

## License

[MIT](https://github.com/DorielRivalet/mhfz-overlay/blob/main/LICENSE.md)

## Code Analysis

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-black.svg)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)

[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=coverage)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=bugs)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=DorielRivalet_MHFZ_Overlay&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=DorielRivalet_MHFZ_Overlay)

## Feedback

[Google Forms](https://forms.gle/hrAVWMcYS5HEo1v7A)
