# Monster Hunter Frontier Z Overlay

[![GitHub all releases](https://img.shields.io/github/downloads/DorielRivalet/mhfz-overlay/total?style=for-the-badge)](#installation)

[![Monster Hunter Frontier Z Overlay v0.21.0 Preview](./demo/youtubepreview1.jpg)](https://www.youtube.com/watch?v=A9ffbRICqZY "Monster Hunter Frontier Z Overlay v0.21.0 Preview")

<div align = center>

---

**[<kbd> <br> 🚀 Install <br> </kbd>](#installation)**
**[<kbd> <br> 📘 Hotkeys<br> </kbd>](#hotkeys)**
**[<kbd> <br> 🕹 Features <br> </kbd>](#features)**

---

</div>

- [Monster Hunter Frontier Z Overlay](#monster-hunter-frontier-z-overlay)
  - [About](#about)
  - [System Requirements](#system-requirements)
    - [Minimum](#minimum)
    - [Recommended](#recommended)
  - [Installation](#installation)
    - [DISCLAIMER](#disclaimer)
    - [Misconceptions](#misconceptions)
  - [Hotkeys](#hotkeys)
    - [Quick Troubleshooting](#quick-troubleshooting)
  - [Features](#features)
  - [Configuration Preview](#configuration-preview)
    - [Hunter Sets (Text)](#hunter-sets-text)
    - [Hunted Logs](#hunted-logs)
    - [Monster Speedruns, Hitzones and Wiki](#monster-speedruns-hitzones-and-wiki)
    - [Ferias](#ferias)
    - [Guild Card](#guild-card)
    - [Hunter Sets (Image)](#hunter-sets-image)
    - [Achievements](#achievements)
    - [Context Menus](#context-menus)
    - [Damage Calculator](#damage-calculator)
    - [Monster Info](#monster-info)
    - [Tower Weapon Simulator](#tower-weapon-simulator)
    - [Ice Age Damage](#ice-age-damage)
    - [Critical Distance Graphs](#critical-distance-graphs)
    - [Elements](#elements)
  - [Features not yet implemented](#features-not-yet-implemented)
  - [Bugs](#bugs)
  - [Frequently Asked Questions](#frequently-asked-questions)
  - [How to Uninstall](#how-to-uninstall)
  - [Release Notes](#release-notes)
  - [Changelog](#changelog)
  - [Website](#website)
  - [Documentation](#documentation)
  - [Contributing](#contributing)
  - [Project Development](#project-development)
  - [Code Analysis](#code-analysis)
  - [Repository Overview](#repository-overview)
  - [Feedback](#feedback)
  - [License](#license)
  - [Acknowledgements](#acknowledgements)

## About

This project aims to provide a simple, customizable overlay for Monster Hunter Frontier Z on Windows, with the added bonus of Discord Rich Presence integration. The overlay allows players to keep track of their in-game stats and progress, as well as providing a convenient way to access various tools and resources.

The overlay is highly configurable, with a wide range of options available to suit the needs of individual players and speedrunners alike. It is also constantly being updated and improved, so be sure to check back for the latest features and fixes.

We hope you find this overlay useful and enjoyable, and we welcome any feedback or suggestions you may have. Happy hunting!

## System Requirements

### Minimum

- OS: Windows 10 (64-bit)
- Processor: Intel® Core™ i3-4130 or Core™ i5-3470
- Memory: 6 GB RAM
- Storage: 500 MB available space
- Additional Notes: 1080p/30fps when refresh rate is set to 1. System requirements subject to change during software development.

### Recommended

- OS: Windows 10 (64-bit)
- Processor: Intel® Core™ i5-4460 or better
- Memory: 6 GB RAM
- Storage: 500 MB available space
- Additional Notes: 1080p/30fps when refresh rate is set to 30. System requirements subject to change during software development.

Please note that these system requirements are estimates and may vary based on your specific hardware configuration. If you encounter any performance issues or have different hardware specifications, please provide feedback or send an issue for assistance. We appreciate your input in helping us improve the overlay's compatibility with a wider range of hardware setups.

If you record and/or stream with the overlay, the recommended processor is Intel® Core™ i7 or better.

## Installation

1. [Download the latest version from the _Releases_](https://github.com/DorielRivalet/mhfz-overlay/releases/latest/download/MHFZ_Overlay-win-Setup.exe).
2. Make sure _Windows_ or your antivirus did not delete the file (because it reads the game's memory, _Windows_ might detect it as a trojan, so you might have to get it out of quarantine).
3. Run `MHFZ_Overlay-win-Setup.exe` **as Administrator**.
4. [Bonk monsters!](https://c.tenor.com/60Tr3Zeg6RkAAAAd/fumo-bonk.gif)
5. [Be sure to leave some feedback here!](https://forms.gle/hrAVWMcYS5HEo1v7A)

> [!NOTE]<br>
> It's recommended to start the overlay when you are done loading into Mezeporta.

[View CHANGELOG.md](https://github.com/DorielRivalet/mhfz-overlay/blob/main/CHANGELOG.md).
[View Release Notes](https://wycademy.vercel.app/overlay/release-notes).
[View Release Statistics](https://somsubhra.github.io/github-release-stats/?username=DorielRivalet&repository=mhfz-overlay&page=1&per_page=30).

### DISCLAIMER

If you obtained the mhfz-overlay software from any source other than GitHub releases, please be aware that there are no guarantees that the program has the same code as the version in this repository, nor that it was created by the same individual(s), or that it represents the latest version.

We strongly recommend that you verify the authenticity and integrity of the software by comparing the SHA-256 checksum we provide against the checksum of the software you have obtained. The SHA-256 checksum can be found in the GitHub release notes and can be used to ensure that the software you are using matches the code in this repository. We assume no responsibility for any issues or problems that may arise from the use of software obtained from other sources.

[How to check the downloaded file hashes](https://github.com/DorielRivalet/mhfz-overlay/blob/main/FAQ.md#how-to-check-the-downloaded-file-hashes).

### Misconceptions

1. [Is the overlay malware?](./FAQ.md#is-the-overlay-malware)
2. [Does X overlay feature slow down my computer?](./FAQ.md#does-x-feature-of-the-overlay-slow-down-my-computer)
3. [Does the overlay affect the game?](./FAQ.md#does-the-overlay-affect-the-game)
4. [Does this monster really have 30,000 HP?](./FAQ.md#what-does-effective-hp-and-true-hp-mean)
5. [Does the database slow down my computer?](./FAQ.md#what-is-the-database-used-for)

## Hotkeys

- `Shift+F1` Open Configuration.
- `Shift+F5` Restart Overlay.
- `Shift+F6` Exit.

As an alternative to hotkeys, you can use the system tray options by right-clicking the icon. You can change the hotkeys by opening the Hotkeys window, found in the system tray menu.

![System Tray options](./demo/systemtray.png)

![Hotkeys options](./demo/hotkeys.png)

### Quick Troubleshooting

- Use the Configuration Preset option for quickly setting up your configuration.

![Preset](./demo/preset.png)

- If you have slow performance, lower the refresh rate from 30 to 1.

- If the monster HP shown is less than what its actual values should be, restart both the game and the overlay. If the HP shows 0/1 then change area for it to load. If the issue still occurs, change the Effective HP Corrector's minimum and maximum thresholds or disable Effective HP.

- If you are having issues with the damage numbers when hitting the monster, change Damage Numbers Mode from Automatic to True Damage.

- If you want the overlay to use the least memory possible, you can decide to not open the configuration window. If you want to change settings, then open the configuration window, edit settings, click save and restart the overlay.

- Press `Alt+Enter` if your screen resolution got lowered.

- If you have screen issues when starting the overlay, first press `Alt+Enter` in-game, load the overlay, then press `Alt+Enter` in-game again. Also make sure that the UAC prompts do not cause issues in your computer, and that you have the correct operating system permissions.

- If you are having performance issues, lower the overlay resolution and refresh rate. Also try setting Rendering Mode to Software or Hardware.

- If you are playing on multiplayer, keep in mind that is not fully supported.

- Fully reinstalling the game or .NET dependencies may fix some bugs.

- If the overlay doesn't seem to load values properly, restart it. If that didn't fix the issue, [please send information here](https://github.com/DorielRivalet/mhfz-overlay/issues).

- Additionally, if information from the overlay is wrong or inaccurate (_e.g._ monster parts labels), feel free to send an issue.

## Features

- [x] Monster Effective HP Bars (_e.g._ Burning Freezing Elzelion's 1,000,000 HP!).

You can also see the monster icons or renders, and there is an option for automatic bar colors depending on the monster. You can adjust the font size, color, family and weight; in addition to that you can hide/show any sub-component. Includes Hardcore and Unlimited icons.

![Monster HP Bars 1](./demo/hp1.png)
![Monster HP Bars 2](./demo/hp2.png)

- [x] Sharpness Numbers (colorized by current sharpness tier!).

![Sharpness Numbers 1](./demo/sharpness1.png)
![Sharpness Numbers 2](./demo/sharpness2.png)
![Sharpness Numbers 3](./demo/sharpness3.png)

- [x] Quest Timer (Two modes: elapsed time and time left. Down to the milliseconds in accuracy!).
- [x] Hit Count (counts _Reflect_, _Stylish Up_, Heatblade, _Fencing+2_ and more!).

![Player Stats](./demo/playerstat1.png)

Includes icons!

![Player Stats Icons](./demo/playerstat2.png)

- [x] Player Input (KBM + Gamepad).

![Player Input](./demo/kbm.gif)

![Gamepad Input](./demo/gamepad.png)

- [x] Player Stats Graphs (Actions per Minute, Damage Per Second, Hits per Second and True Raw!).

![Player APM](./demo/apm.gif)
![Player Graphs](./demo/graphs.png)

- [x] Player True Raw (currently highest value shown in red!).

![Player Attack](./demo/playeratk1.png)

- [x] Monster Stats + Icons (attack multiplier, defense rate and size!).

![Monster Stats](./demo/monsterstat1.png)
![Monster Stats Icons](./demo/monsterstat2.png)

- [x] Monster Status Ailments + Icons (Poison, Sleep, Paralysis, Blast, Stun!).

![Monster Ailments](./demo/ailments1.png)
![Monster Ailments Icons](./demo/ailments2.png)

- [x] Monster Body Parts (up to 10 parts!).

![Monster Body Parts](./demo/monsterparts1.png)

- [x] Damage Numbers (dynamic colors and size!).

![Damage Numbers](./demo/hits.gif)

- [x] [Discord Rich Presence](./FAQ.md/#how-to-enable-discord-rich-presence) (custom monster icons, colored weapons, quest tier, current area, [speedrun mode, zen mode](./FAQ.md/#how-to-enable-speedrun-categories--zen-mode), and more!).

![Buffs with Timers](./demo/buffs.png)

- [x] Diva Song and Guild Food timers. Keep track of your buffs!

![Discord Rich Presence](./demo/discord5.png)
![Discord Rich Presence](./demo/discord10.png)

![Discord Rich Presence](./demo/discord9.png)

![Discord Rich Presence](./demo/discord9.gif)

![Discord Rich Presence](./demo/discord7.png)

- [x] Run Category Watermarks and Personal Best Times for your speedrun videos!

![Watermarks](./demo/speedrun.png)

- [x] Quest Runs Database (check weapon usage, set YouTube URLs, view past statistics, etc.!)

![Weapon Usage](./demo/databaseweaponusage.png)

![Compendium of Personal Statistics](./demo/compendium.png)

![Calendar](./demo/calendar.png)

![Top 20](./demo/databasetop20.png)

![Gear](./demo/databasegear.png)

![Graphs](./demo/databasegraphs.png)

![Most Recent](./demo/databasemostrecent.png)

![YouTube](./demo/databaseyoutube.png)

![Quest Pace](./demo/questpace.png)

![Inventories](./demo/databaseinventories.png)

- [x] Also includes personal best times by date and by attempts! The total frames elapsed are shown in the graph.

![Personal Best by Attempts](./demo/personalbest1.png)

**Important**: It is recommended to make a backup of the `MHFZ_Overlay.sqlite` file periodically. The file is located inside the database folder, which is inside your game folder.

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

### Achievements

![achievements](./demo/achievements.png)

### Context Menus

Some sections have context menus where you can save the section contents to a file or copy to clipboard as either an image or text. In order to view the context menus you have to right click on the section contents. To see if a section has these context menus, hover over the section contents and a tooltip will show, saying "Right-click for more options".

![contextmenu1](./demo/contextmenu1.png)
![contextmenu2](./demo/contextmenu2.png)

![contextmenu3](./demo/contextmenu3.png)
![contextmenu4](./demo/contextmenu4.png)
![contextmenu5](./demo/contextmenu5.png)

### Damage Calculator

![Config7](./demo/config7.png)

The damage calculator comes from [Wycademy](https://wycademy.vercel.app/), a compendium of resources for Monster Hunter Frontier Z made by DorielRivalet. Check it out! Here are some examples of the website:

### Monster Info

![Wycademy1](./demo/wycademy1.png)

### Tower Weapon Simulator

![Wycademy2](./demo/wycademy2.png)

### Ice Age Damage

![Wycademy3](./demo/wycademy3.png)

### Critical Distance Graphs

![Wycademy4](./demo/wycademy4.png)

### Elements

Includes light theme!

![Wycademy5](./demo/wycademy5.png)

## Features not yet implemented

- Choose which player to load data from.
- Auto detect which player is playing.
- Selecting monsters for body parts and monster status.
- Allow lock-on to be used to select monsters.
- Add shortcut for saving.
- Automatically set default positions according to screen resolution.
- Global damage number labels.
- Attach user interface to game window option.
- Sharpness graph.
- Language options.
- PvP addresses.
- Handle multiple objectives information.
- Zenith information in Road.
- Raviente Support Part Info.
- Armor Set Website links.
- Sky Corridor.
- Drag and Drop multiple selection.
- Sharpness tables.
- Gear rarity colors in hunter info stats.

[Check more possible future features here](https://github.com/DorielRivalet/mhfz-overlay/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement).

## Bugs

- Monster stats are sometimes outside of the screen (if they don't show at all even if you open the config menu, this is probably your issue).
- With Monster EHP enabled, if you cart, the max EHP turns into the current EHP, along with some other stats max values.
- Sometimes when exiting Drag and Drop the monster HP information disappears.
- Spawning in the same area as the monster doesn't load the information properly. Fix: re-enter area.
- Duremudira/Road/Raviente HP not showing. Fix: enable _Always Show Monster Info_, load another quest showing the HP bars (not just the numbers), then retry.
- Monster size values not shown correctly.
- Monster HP values are less than the actual values when not loading properly.
- Damage numbers over 1000 not working.
- Yamas and Berukyurosu information not working.

[Check more bugs here](https://github.com/DorielRivalet/mhfz-overlay/issues?q=is%3Aissue+is%3Aopen+label%3Abug).

## Frequently Asked Questions

[FAQ](https://github.com/DorielRivalet/mhfz-overlay/blob/main/FAQ.md).

## How to Uninstall

1. Go to Apps & Features.
2. Search Monster Hunter Frontier Z Overlay.
3. Click Uninstall. You can also delete the Desktop shortcut and Start Menu icon after uninstalling.

## Release Notes

[See our developer's notes about the latest changes](https://wycademy.vercel.app/overlay/release-notes).

## Changelog

[Learn about the latest improvements in greater detail](https://github.com/DorielRivalet/mhfz-overlay/blob/main/CHANGELOG.md).

## Website

The overlay has it's own page in [Wycademy](https://wycademy.vercel.app/overlay).

![Wycademy Overlay](./demo/wycademy6.png)

## Documentation

This repository includes two main documentation files:

- **`FAQ.md`**: This file is meant for end-users and provides information about how to use and troubleshoot the software. If you're new to the software, we recommend starting here.

- **`docs/README.md`**: This file is meant for developers and technical users who are contributing to the project or working on the codebase. It contains information about the software's intricacies, as well as instructions for deploying the project.

We recommend consulting the appropriate documentation file based on your needs. If you have any questions or issues, feel free to [contact us](https://github.com/DorielRivalet/mhfz-overlay/issues/new/choose) for support.

## Contributing

[Learn about how to contribute](https://github.com/DorielRivalet/mhfz-overlay/blob/main/CONTRIBUTING.md).

## Project Development

[GitHub Projects](https://github.com/DorielRivalet/mhfz-overlay/projects?query=is%3Aopen).

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

[![CodeFactor](https://www.codefactor.io/repository/github/dorielrivalet/mhfz-overlay/badge/main)](https://www.codefactor.io/repository/github/dorielrivalet/mhfz-overlay/overview/main)

## Repository Overview

[![Build Status](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2FDorielRivalet%2Fmhfz-overlay%2Fbadge%3Fref%3Dmain&style=flat)](https://actions-badge.atrox.dev/DorielRivalet/mhfz-overlay/goto?ref=main)
![CircleCI](https://img.shields.io/circleci/build/github/DorielRivalet/mhfz-overlay?label=CircleCI&style=flat)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/DorielRivalet/mhfz-overlay?style=flat)
![GitHub repo size](https://img.shields.io/github/repo-size/DorielRivalet/mhfz-overlay?style=flat)
[![wakatime](https://wakatime.com/badge/user/958e2c62-03f7-4c2a-82df-75c8df9ee232/project/db4298ba-fbc4-4fc2-aa24-67938f65ee8a.svg)](https://wakatime.com/badge/user/958e2c62-03f7-4c2a-82df-75c8df9ee232/project/db4298ba-fbc4-4fc2-aa24-67938f65ee8a)

[![Automate Git Stats](https://github.com/DorielRivalet/mhfz-overlay/actions/workflows/automate-git-stats.yml/badge.svg?event=schedule)](https://github.com/DorielRivalet/mhfz-overlay/actions/workflows/automate-git-stats.yml)

![Repository Commits Stats 1](./scripts/output/commit_types.png)

![Repository Commits Stats 2](./scripts/output/commits_over_time.png)

![Repository Commits Stats 3](./scripts/output/commits_per_day_of_week.png)

![Repository Commits Stats 4](./scripts/output/commits_per_hour.png)

[Generated thanks to the following scripts](./scripts)

![Ruby](https://img.shields.io/badge/Ruby-1e1e2e?style=for-the-badge&labelColor=23f5e0dc&logoColor=CC342D&logo=ruby)
![Python](https://img.shields.io/badge/Python-1e1e2e?style=for-the-badge&labelColor=23f5e0dc&logoColor=3776AB&logo=python)
![Lua](https://img.shields.io/badge/Lua-1e1e2e?style=for-the-badge&labelColor=23f5e0dc&logoColor=2C2D72&logo=lua)

## Feedback

[Google Forms](https://forms.gle/hrAVWMcYS5HEo1v7A).

## License

[MIT](https://github.com/DorielRivalet/mhfz-overlay/blob/main/LICENSE.md).

## Acknowledgements

- This project is directly inspired from the overlay created by [_suzaku01_](https://github.com/suzaku01/mhf_displayer).
- The theme and color palette used for the application is [_Catppuccin Mocha_](https://github.com/catppuccin/catppuccin).
- The design and icons used in this project are part of [_Segoe Fluent Icons_](https://learn.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font) and [WPF UI](https://github.com/lepoco/wpfui).
- The fonts used is the in-game one, _MS Gothic_. For monospaced, the application uses Source Code Pro and MesloLGM NF. This project also uses Font Awesome's fonts. The Monster Hunter font made by XMitsarugiX comes from [here](https://www.deviantart.com/xmitsarugix/art/Monster-Hunter-Font-Type-1-and-2-380816151).
- Thanks to [Kairi](https://www.youtube.com/@kairi_mhfz) and [Sera](https://www.youtube.com/@Sera9145) for extensive beta testing and early support.
- The combo element icons are made by [Narwhaler](https://fanonmonsterhunter.fandom.com/wiki/User:Narwhaler).
- Additional icons can be found [here](https://fanonmonsterhunter.fandom.com/wiki/Category:Icon).

<a href="https://github.com/suzaku01"><img style="border-radius: 50% !important;" alt="Avatar" src="https://avatars.githubusercontent.com/u/89909040?v=4" width="64px" height="auto" ></a>
<a href="https://github.com/Imulion"><img style="border-radius: 50% !important;" alt="Avatar" src="https://avatars.githubusercontent.com/u/27354834?v=4" width="64px" height="auto" ></a>
<a href="https://github.com/Catppuccin"><img style="border-radius: 50% !important;" alt="Avatar" src="https://raw.githubusercontent.com/catppuccin/catppuccin/main/assets/logos/exports/1544x1544_circle.png" width="64px" height="auto" ></a>
<a href="https://github.com/lepoco/wpfui"><img style="background-color:#eff1f5" alt="Avatar" src="https://avatars.githubusercontent.com/u/87412094?s=200&v=4" width="64px" height="auto" ></a>
<a href="https://www.youtube.com/@kairi_mhfz"><img style="border-radius: 50% !important;" alt="Avatar" src="https://yt3.googleusercontent.com/ytc/AOPolaQWKx3GIOgWWRu9YCHBI7lrkFjT2NxLP45IlnSM=s176-c-k-c0x00ffffff-no-rj" width="64px" height="auto" ></a>
<a href="https://www.youtube.com/@Sera9145"><img style="border-radius: 50% !important;" alt="Avatar" src="https://yt3.googleusercontent.com/2PmblC16_LZtoziuCn9ZMbivRpkLXi60t44bhp3WSl3KB_ShbDvvek-hRfZgfbf2HDOYetDP=s176-c-k-c0x00ffffff-no-rj" width="64px" height="auto" ></a>
