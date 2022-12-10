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
10. `Squirrel.exe pack --packId "mhfz-overlay" --packDirectory "PATH\mhfz-overlay\mhfz-overlay\bin\Release\net6.0-windows\publish" --framework net6.0-x86 --packTitle="Monster Hunter Frontier Z Overlay" --packAuthors="DorielRivalet" --splashImage="PATH\mhfz-overlay\splash.png" --icon="PATH\mhfz-overlay\img\icon\mhfzoverlayicon256.ico" --appIcon="PATH\mhfz-overlay\img\icon\mhfzoverlayicon256.ico" --packVersion "0.13.0"`
11. Add to GitHub Releases
12. Verify Github Actions
