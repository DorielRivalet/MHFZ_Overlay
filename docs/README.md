# Deployment process

1. `git checkout`
2. `git pull`
3. Make changes
4. Bump version in *CurrentProgramVersion* variable
5. Commits to branch *release*
6. `npm run release`
7. Checkout to branch *main*
8. Merge *release* into *main*
9. Switch from *Build* to *Release*, Publish in Visual Studio
10. `Squirrel.exe pack --packId "MHFZ_Overlay" --packDirectory "PATH\MHFZ_Overlay\MHFZ_Overlay\bin\Release\net6.0-windows\publish" --framework net6.0-x86 --packTitle="Monster Hunter Frontier Z Overlay" --packAuthors="DorielRivalet" --splashImage="PATH\MHFZ_Overlay\splash.png" --icon="PATH\MHFZ_Overlay\img\icon\mhfzoverlayicon256.ico" --appIcon="PATH\MHFZ_Overlay\img\icon\mhfzoverlayicon256.ico" --packVersion "0.13.0"`
11. Add to GitHub Releases
12. Verify Github Actions
