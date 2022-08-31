# MHFZ_Overlay

This project is directly inspired from the overlay created by https://github.com/suzaku01


## Usage

1. Download the latest version from the Releases
2. Install the required .NET runtime **BE SURE TO INSTALL x86 VERSION EVEN IF YOUR WINDOWS IS x64 (this is required because MHFZ runs on 32-Bit)** 
3. Make sure windows did not delete the MHFZOverlay.dll file (because it reads the games memory windows might detect it as a trojan so you might have to get it out of quarantine)
4. Execute the MHFZ_Overlay.exe

In the top left corner are buttons for configuration, restart overlay and exit respectively. You can hide them by pressing `Shift+Insert`.

## Bugs

- Monster Infos are sometimes outside of the screen (if they don't show at all even if you open the config menu, this is probably your issue)
- Sometimes you can be in drag and drop mode without having the settings open
- Road detection doesn't work all the time (use road override if road in general works for you)

## TODO

- Choose which player to load data from
- Auto detect which player is playing
- Selecting monsters for body parts and monster status
- Body parts infos
- Fix max HP for Road
- Better detection for if the player is in Road so the Road Override is not necessary anymore
- Discord Rich Presence
- Add shortcut for saving
- Allow lock-on to be used to select monsters
- Overlay font option
- Damage graph