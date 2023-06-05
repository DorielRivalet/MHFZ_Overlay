# Releasifying and Deployment Process

## Table of Contents

- [Releasifying and Deployment Process](#releasifying-and-deployment-process)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
    - [Packaging, Releasifying, Deployment and Distribution Steps](#packaging-releasifying-deployment-and-distribution-steps)
    - [Merging via command line](#merging-via-command-line)

## Overview

The following steps outline the process for releasing and deploying the software using clowd.squirrel, after making changes and testing in-game.

### Packaging, Releasifying, Deployment and Distribution Steps

1. Run `git fetch` to check for the latest remote changes. Then switch to the release branch: Run `git switch release` to switch to the release branch.
2. Pull the latest changes: Run `git pull origin release` to pull the latest changes from the remote release branch.
3. Make changes: Make the necessary changes to the software, and thoroughly test them in-game. Ideally, add unit tests to cover the changes and fix any new bugs with new commits.
4. Bump version: Update the Version in csproj, following SemVer's specifications.
5. Commit changes: Run `git add .` to stage the changes, then run `git commit -m "Your commit message here"` to commit the changes, following commitlint's specifications.
6. Update CHANGELOG.md: Run `npm run release` to automatically update the CHANGELOG.md file with the changes made. When prompted for the tag name, follow Semver's specifications and prefix with a `v`. Answer `Yes` to all except the last question ("Create a Release on GitHub?").
7. Merge release branch into main: **Recommended to send a Pull Request to the main branch**. Otherwise, run `git checkout main` to switch to the main branch and do `git pull origin main` to pull the latest remote changes, then run `git merge release` to merge the release branch into the main branch, and finally `git push origin main`.
8. Verify GitHub Actions: Verify that the GitHub Actions workflow has been triggered and completed successfully. If something is wrong, go back to step 1 and fix any issues without modifying the git history (no rebases and no squashes).
9. Publish in Visual Studio: Open the solution in Visual Studio then switch from Build configuration to Release configuration. Publish the software's project using Visual Studio.
10. Update packVersion with Clowd.Squirrel: Open the command prompt in the `C:\Users\Name\.nuget\packages\clowd.squirrel\x.x.xx\tools` folder, then run the following command, replacing the appropriate paths and version number (replace the packVersion flag with the actual version number):
    - `Squirrel.exe pack --packId "MHFZ_Overlay" --packDirectory "ABSOLUTE_PATH\MHFZ_Overlay\MHFZ_Overlay\bin\Release\net6.0-windows\win-x86\publish" --framework net6.0-x86 --packTitle="Monster Hunter Frontier Z Overlay" --packAuthors="DorielRivalet" --splashImage="ABSOLUTE_PATH\MHFZ_Overlay\splash.png" --icon="ABSOLUTE_PATH\MHFZ_Overlay\img\icon\mhfzoverlayicon256.ico" --appIcon="ABSOLUTE_PATH\MHFZ_Overlay\img\icon\mhfzoverlayicon256.ico" --packVersion "0.13.0"`
11. Add to GitHub Releases: Create a new release on GitHub with the appropriate version number, release notes, and documentation. Lastly, attach all of the files in the generated Releases folder separately.

By following these steps, the software can be releasified and deployed using clowd.squirrel, ensuring that the latest changes are packaged and released to users efficiently and reliably.

### Merging via command line

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
