# Frequently Asked Questions

## How to Enable Discord Rich Presence?

- In Discord, My Account -> Activity Privacy -> Check "Display current activity as a status message"

![Discord](./demo/discord1.png)

- [Discord Developer Portal](https://discord.com/developers/applications) -> New Application -> Name it "MONSTER HUNTER FRONTIER Z"

![Discord](./demo/discord2.png)

- In Developer Portal, General Information -> Copy Application ID

![Discord](./demo/discord3.png)

- In Overlay Settings, Paste into Overlay Settings Discord Rich Presence Application/Client ID (The ID also shows up in OAuth2 section as Client ID)

![Discord](./demo/discord4.png)

![Discord Rich Presence](./demo/discord6.gif)

## How to Enable Speedrun Categories & Zen Mode?

- Speedrun Mode Categories: SOLO ONLY. Enable the required settings in the Quest Logs section, disable **everything** else, including Quest Pace Color (Monster Icon, Quest Timer + Percentage, KBM Layout, Personal Best and Discord Rich Presence optional)

**Important**: It is recommended to make a backup of the `MHFZ_Overlay.sqlite` file periodically. The file is located inside the database folder, which is inside your game folder. Don't lose your speedrun records!

![Discord Rich Presence](./demo/discord8.png)

- Zen Mode: Disable **everything** (Monster Icon and Discord Rich Presence optional)

![Discord Rich Presence](./demo/discord11.png)

~~Congrats, now you won't be accused of cheating~~

## How to Record Videos with the Overlay?

1. [Download OBS](https://obsproject.com/)
2. Go to the Sources section, click Add Source, select Window Capture, select Create new, click OK.
3. Select the Window dropdown, choose mhf.exe.
4. Make sure Window Match Priority is set to "Match title, otherwise find window of same type"
5. Click Add Source again, select Window Capture, select Create new, click OK.
6. Select the Window dropdown, choose MHFZ_Overlay.exe (if the option is not shown, load the overlay first and retry)
7. For Capture Method, choose Windows 10
8. Make sure Window Match Priority is set to "Match title, otherwise find window of same executable"
9. Happy Hunting!

## My game slows down when recording with the Overlay, what should I do?

If you're experiencing a slowdown in your game while recording with OBS and using the overlay, there are a few potential reasons and solutions to consider:

- System resources: Recording gameplay with OBS and running an overlay can be resource-intensive, especially if your computer has limited CPU or GPU resources. Try closing any unnecessary programs, lowering in-game graphics settings, or upgrading your hardware to meet the requirements for recording and overlay usage.

- Overlay settings: The settings of the overlay can also impact performance. Make sure your overlay settings are optimized for performance, such as using lower resolution and disabling any unnecessary features. Try one of the configuration presets, such as Speedrun, Zen or HP only. Lastly, try reducing the maximum resolution values. As a last resort, try lowering the Refresh Rate, at the cost of stats accuracy.

- OBS settings: Review your OBS settings to ensure they are optimized for recording. Adjusting bitrate, resolution, and frame rate settings in OBS can help improve performance while recording with an overlay.

- Test on different hardware: Test your overlay program and OBS on different hardware configurations to determine if the issue persists across different systems, which can help identify if it's a hardware-specific issue.

## How to Enable Quest Logging?

1. Go to Mezeporta, load the Overlay.
2. Press `Shift+F1`, go to Quest Log tab.
3. Go to Settings tab inside the Quest Log tab, toggle Quest Logging.
4. The overlay should detect the folder location of the game and make a folder named `database` inside the game folder, creating the `MHFZ_Overlay.sqlite` in that database folder if there isn't one.
5. If you already had a database before, you can rename it to `MHFZ_Overlay.sqlite` and place it in that database folder. It should keep your past quest information.
6. Be careful if you re-install the game, make a backup of the database folder first.

## What does the version numbers mean?

We use [semantic versioning](https://semver.org/) to number our releases. In semantic versioning, versions are represented as "major.minor.patch" numbers, such as "1.2.3". Here's what each number signifies:

- **Major version**: A change in the major version number indicates a significant change that may require you to upgrade your software. This typically includes breaking changes that are not backwards-compatible.

- **Minor version**: A change in the minor version number indicates new features or enhancements that have been added to the software.

- **Patch version**: A change in the patch version number indicates bug fixes and small improvements.

Major version zero (0.y.z) is for initial development. Anything MAY change at any time. The public API SHOULD NOT be considered stable.

We provide [release notes](https://github.com/DorielRivalet/mhfz-overlay/blob/main/CHANGELOG.md) with each new version of the software, which describe the changes that have been made and how they may affect you. If upgrading to a new version requires any special instructions, we will provide these as well.

If you have any questions about versioning or how to upgrade your software, please don't hesitate to contact us for support.

## Where do I find my logs or crash files?

In the folder where `MHFZ_Overlay.exe` is, there should be a `log` folder with a log file named `logs.log`. This file is useful for debugging the program. You should only send the relevant text in the logs when reporting an issue, not the whole file, so only send the last lines where appropiate. Look out for words such as WARN, ERROR, or FATAL.

## Does this software support other operating systems?

As detailed in the program's *about* section, this software is meant for Windows 10. It might work with other versions of Windows, but it may function with less stability.

## The software messed up my computer or my game, what should I do?

Please read the [license](https://github.com/DorielRivalet/mhfz-overlay/blob/main/LICENSE.md) fully before utilizing the software.

## I would like to send a review or feedback for the overlay, where do I send it?

Sure thing! [Click here](https://forms.gle/hrAVWMcYS5HEo1v7A) and thank you for your feedback.

## How can I prevent the overlay from changing the game's resolution when launching it?

The fix would have to come from Window's side, not the overlay.

## I cannot run the setup executable, what should I do?

Make sure to run as Administrator.

## How to import/export settings from the overlay?

Open the overlay settings, go to General tab, press the Open Settings Folder button. This is where your user settings file is stored. If you wish to replace current settings from another settings file:

1. Close the overlay.
2. Copy/Paste and overwrite, with the current same folder and file name.
3. Start the overlay.

Also keep in mind that when you update the overlay from the program itself, the following happens:

1. Your current settings are automatically and temporarily backed up.
2. Restores the settings from this backup.
3. The backup file is automatically deleted.

This way, there are no lost settings when updating, and you don't have to make the backup manually.

## How to check the downloaded file hashes?

Open PowerShell and enter the command `Get-FileHash (drag and drop your file into the terminal)`. Check if the hashes provided by the developer are the same as the hashes provided by PowerShell. If they are the same, the files shouldn't have been tampered with. If the command worked, you should get SHA256 in the `Algorithm` column, and the hash value in the `Hash` column, along with the Path provided.

## My question isn't answered here, how can I contact the developers?

First, you may want to check the issues that have already been reported [here](https://github.com/DorielRivalet/mhfz-overlay/issues)

If you want to report a bug, please go [here](https://github.com/DorielRivalet/mhfz-overlay/issues/new?assignees=DorielRivalet&labels=bug&template=BUG-REPORT.yml&title=%5BBUG%5D+-+%3Ctitle%3E)

If you would like to send a feature request, please go [here](https://github.com/DorielRivalet/mhfz-overlay/issues/new?assignees=&labels=question%2Cenhancement&template=FEATURE-REQUEST.yml&title=%5BREQUEST%5D+-+%3Ctitle%3E)

For reporting security vulnerabilities, please go [here](https://github.com/DorielRivalet/mhfz-overlay/security/advisories/new)

Alternatively, send an issue [here](https://github.com/DorielRivalet/mhfz-overlay/issues/new) detailing your inquiry about the program.
