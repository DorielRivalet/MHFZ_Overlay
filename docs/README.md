# Deployment process

1. Commits to branch release
2. npm run release
3. Checkout to branch main
4. Merge release into main
5. Switch from Build to Release, Publish in Visual Studio
6. Squirrel.exe pack --packId "MHFZ_Overlay" --packVersion "x.x.x" --packDirectory "PATH\MHFZ_Overlay\MHFZ_Overlay\bin\Release\net6.0-windows\publish" --framework net6.0-x86 
7. Add to GitHub Releases
