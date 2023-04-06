# Deployment process

1. `git checkout release`
2. `git pull release`
3. Make changes
4. Bump version in *CurrentProgramVersion* variable
5. `git add` to branch *release*
6. `git commit` to branch *release*
7. `npm run release`
8. `git checkout` to branch *main*
9. [Merge *release* into *main*](#merging-via-command-line)
10. Switch from *Build* to *Release*, Publish in Visual Studio
11. Update the packVersion with this command, using cmd in the `C:\Users\Name\.nuget\packages\clowd.squirrel\x.x.xx\tools` folder
    - `Squirrel.exe pack --packId "MHFZ_Overlay" --packDirectory "PATH\MHFZ_Overlay\MHFZ_Overlay\bin\Release\net6.0-windows\publish" --framework net6.0-x86 --packTitle="Monster Hunter Frontier Z Overlay" --packAuthors="DorielRivalet" --splashImage="PATH\MHFZ_Overlay\splash.png" --icon="PATH\MHFZ_Overlay\img\icon\mhfzoverlayicon256.ico" --appIcon="PATH\MHFZ_Overlay\img\icon\mhfzoverlayicon256.ico" --packVersion "0.13.0"`
12. Add to GitHub Releases
13. Verify Github Actions

## Merging via command line

If you do not want to use the merge button or an automatic merge cannot be performed, you can perform a manual merge on the command line. However, the following steps are not applicable if the base branch is protected.

1. Clone the repository or update your local repository with the latest changes.
   - `git@github.com:DorielRivalet/mhfz-overlay.git`
   - `git pull origin main`
2. Switch to the base branch of the pull request.
   - `git checkout main`
3. Merge the head branch into the base branch.
   - `git merge release`
4. Push the changes.
   - `git push -u origin main`
