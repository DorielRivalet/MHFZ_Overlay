# MHFZ_Overlay

## About

[This project is directly inspired from the overlay created by suzaku01](https://github.com/suzaku01/mhf_displayer)

[The theme and color palette used for the application is Catppuccin Mocha](https://github.com/catppuccin/catppuccin)

[The icons used in this project are part of the Material Design icons](https://fonts.google.com/icons)

## Requirements

- [.NET Desktop Runtime 6.0 x64](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.7-windows-x64-installer)
- [.NET Desktop Runtime 6.0 x86](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.8-windows-x86-installer)

## Installation

1. [Get the requirements](#requirements)
2. [Download the latest version from the Releases](https://github.com/Imulion/MHFZ_Overlay/releases/latest)
3. Make sure Windows or your antivirus did not delete the `MHFZOverlay.dll` file (because it reads the games memory windows might detect it as a trojan so you might have to get it out of quarantine)
4. Run `MHFZ_Overlay.exe`
5. [Bonk monsters!](https://c.tenor.com/60Tr3Zeg6RkAAAAd/fumo-bonk.gif)

## Hotkeys

- `Shift+F1` Open Configuration
- `Shift+F5` Restart Overlay
- `Shift+F6` Exit

If the overlay doesn't seem to load values properly, restart it. If that didn't fix the issue, [please send information here](https://github.com/Imulion/MHFZ_Overlay/issues).

## Features

- Monster Effective HP Bars
- Sharpness Numbers
- Timer
- Hit Count
- Monster Status Ailments
- Monster Body Parts
- Damage Numbers
- Discord Rich Presence

## Bugs

- Monster Infos are sometimes outside of the screen (if they don't show at all even if you open the config menu, this is probably your issue)
- Road detection doesn't work all the time (use road override if road in general works for you)
- With Monster EHP enabled, if you cart, the max EHP turns into the current EHP

## Features not yet implemented

- Choose which player to load data from
- Auto detect which player is playing
- Selecting monsters for body parts and monster status
- Fix max HP for Road
- Better detection for if the player is in Road so the Road Override is not necessary anymore
- Allow lock-on to be used to select monsters
- Overlay font options
- Add shortcut for saving
- Damage graph
- Weapon stats import/export
- Quest stats import/export
- Monster stats import/export
- Halk level address
- Damage numbers Label border
- Automatically set default positions according to screen resolution
- Global damage number labels
- Player attack multicolor relative to highest attack obtained per quest