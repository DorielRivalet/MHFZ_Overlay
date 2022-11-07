# Deployment process

1. Bump version in *CurrentProgramVersion* variable
2. Commits to branch *release*
3. `npm run release`
4. Checkout to branch *main*
5. Merge *release* into *main*
6. Switch from *Build* to *Release*, Publish in Visual Studio
7. `Squirrel.exe pack --packId "MHFZ_Overlay" --packDirectory "PATH\MHFZ_Overlay\MHFZ_Overlay\bin\Release\net6.0-windows\publish" --framework net6.0-x86 --packTitle="Monster Hunter Frontier Z Overlay" --packAuthors="DorielRivalet" --releaseNotes="PATH\MHFZ_Overlay\CHANGELOG.md" --splashImage="PATH\MHFZ_Overlay\img\icon\cattleya.png" --icon="PATH\MHFZ_Overlay\img\icon\mhfzoverlayicon256.ico" --appIcon="PATH\MHFZ_Overlay\img\icon\mhfzoverlayicon256.ico" --packVersion "0.13.0"`
8. Add to GitHub Releases
