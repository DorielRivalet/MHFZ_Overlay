# Frequently Asked Questions

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

## How to Enable Speedrun Categories & Zen Mode

- Speedrun Mode Categories: SOLO ONLY. Enable the required settings in the Quest Logs section, disable **everything** else, including Quest Pace Color (Monster Icon, Quest Timer + Percentage, KBM Layout, Personal Best and Discord Rich Presence optional)

**Important**: It is recommended to make a backup of the `MHFZ_Overlay.sqlite` file periodically. The file is located inside the database folder, which is inside your game folder. Don't lose your speedrun records!

![Discord Rich Presence](./demo/discord8.png)

- Zen Mode: Disable **everything** (Monster Icon and Discord Rich Presence optional)

![Discord Rich Presence](./demo/discord11.png)

~~Congrats, now you won't be accused of cheating~~

## How to Record Videos with the Overlay

1. [Download OBS](https://obsproject.com/)
2. Go to the Sources section, click Add Source, select Window Capture, select Create new, click OK.
3. Select the Window dropdown, choose mhf.exe.
4. Make sure Window Match Priority is set to "Match title, otherwise find window of same type"
5. Click Add Source again, select Window Capture, select Create new, click OK.
6. Select the Window dropdown, choose MHFZ_Overlay.exe (if the option is not shown, load the overlay first and retry)
7. For Capture Method, choose Windows 10
8. Make sure Window Match Priority is set to "Match title, otherwise find window of same executable"
9. Happy Hunting!

## How to Enable Quest Logging

1. Go to Mezeporta, load the Overlay.
2. Press `Shift+F1`, go to Quest Log tab.
3. Go to Settings tab inside the Quest Log tab, toggle Quest Logging.
4. The overlay should detect the folder location of the game and make a folder named `database` inside the game folder, creating the `MHFZ_Overlay.sqlite` in that database folder if there isn't one.
5. If you already had a database before, you can rename it to `MHFZ_Overlay.sqlite` and place it in that database folder. It should keep your past quest information.
6. Be careful if you re-install the game, make a backup of the database folder first.
