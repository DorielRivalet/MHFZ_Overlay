# Deployment process

1. Bump version in CurrentProgramVersion variable
2. Commits to branch release
3. npm run release
4. Checkout to branch main
5. Merge release into main
6. Switch from Build to Release, Publish in Visual Studio
7. Squirrel.exe pack --packId "MHFZ_Overlay" --packVersion "x.x.x" --packDirectory "PATH\MHFZ_Overlay\MHFZ_Overlay\bin\Release\net6.0-windows\publish" --framework net6.0-x86 
8. Add to GitHub Releases
