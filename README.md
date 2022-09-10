# MHFZ_Overlay

## About

This project is directly inspired from the overlay created by https://github.com/suzaku01

## Requirements

- [.NET Desktop Runtime 6.0 x64](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.7-windows-x64-installer)
- [.NET Desktop Runtime 6.0 x86](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.8-windows-x86-installer)

## Installation

1. [Download the latest version from the Releases](https://github.com/Imulion/MHFZ_Overlay/releases/latest)
2. Make sure Windows or your antivirus did not delete the `MHFZOverlay.dll` file (because it reads the games memory windows might detect it as a trojan so you might have to get it out of quarantine)
3. Run `MHFZ_Overlay.exe`

In the top left corner are buttons for configuration (`Shift+F1`), restart overlay (`Shift+F5`) and exit (`Shift+F6`) respectively.

## Features

- Monster Effective HP Bars
- Sharpness Numbers
- Timer
- Hit Count
- Monster Status Ailments
- Damage Numbers
- Discord Rich Presence

## Bugs

- Monster Infos are sometimes outside of the screen (if they don't show at all even if you open the config menu, this is probably your issue)
- Road detection doesn't work all the time (use road override if road in general works for you)

## Features not yet implemented

- Choose which player to load data from
- Auto detect which player is playing
- Selecting monsters for body parts and monster status
- Body parts information
- Fix max HP for Road
- Better detection for if the player is in Road so the Road Override is not necessary anymore
- Add shortcut for saving
- Allow lock-on to be used to select monsters
- Overlay font options
- Damage graph
- Weapon stats import/export
- Quest stats import/export
- Monster stats import/export
- Halk level address
