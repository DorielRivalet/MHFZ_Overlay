# Contributing

[![License](https://img.shields.io/github/license/dorielrivalet/mhfz-overlay)](https://github.com/dorielrivalet/mhfz-overlay)

- [Contributing](#contributing)
  - [Ways to contribute](#ways-to-contribute)
  - [Contributing workflow](#contributing-workflow)
  - [Commit convention](#commit-convention)
  - [Git branches](#git-branches)
  - [Get started with the overlay locally](#get-started-with-the-overlay-locally)
  - [Did you find a bug?](#did-you-find-a-bug)
  - [Do you want to request a feature?](#do-you-want-to-request-a-feature)
  - [Did you write a patch that fixes a bug?](#did-you-write-a-patch-that-fixes-a-bug)
  - [Do you intend to add a new feature yourself or change an existing one?](#do-you-intend-to-add-a-new-feature-yourself-or-change-an-existing-one)
  - [Do you want to contribute to the documentation?](#do-you-want-to-contribute-to-the-documentation)
  - [Closing notes](#closing-notes)

## Ways to contribute

- **Improve documentation:** fix incomplete or missing docs, bad wording, examples or explanations.
- **Give feedback:** we are constantly working on making the overlay better, please share how you use the overlay, what features are missing and what is done good.
- **Share the overlay:** share link to the overlay with anyone who could be interested.
- **Contribute to codebase:** propose new feature via GitHub Issues or find an [existing one](https://github.com/dorielrivalet/mhfz-overlay/labels/help%20wanted) that you are interested in and work on it.
- **Give us a code review:** help us identify problems with [source code](https://github.com/dorielrivalet/mhfz-overlay) or make the overlay more performant.

## Contributing workflow

Please see [the documentation](./docs/deployment.md#repository-branch-structure) to understand the git workflow.

In short:

- Decide what you want to contribute.
- If you want to implement a new feature, consider a draft pull request before fully jumping into code.
- After finalizing your work, please follow our commit conventions.
- Submit a PR if everything is fine.
- Get a code review and fix all issues noticed by a maintainer.
- PR is merged, and we're done!

## Commit convention

It is important to write clear commit messages to keep the git history clean.

This repo uses [commitlint](https://github.com/conventional-changelog/commitlint) to make commits easier.

## Git branches

- **main** - This branch reflects what is being published on the main github page.
- **release** - This is used for working on upcoming releases. After a release is tested, the changes are merged into main.
- **backup** - This is used as a backup branch, in case main needs to be updated significantly.

Feature branches can be merged into the release branch.

## Get started with the overlay locally

- Install [editorconfig](https://editorconfig.org/) extension for your editor.
- Fork the [repository](https://github.com/dorielrivalet/mhfz-overlay), clone or download your fork.

## Did you find a bug?

- **Ensure the bug was not already reported** by searching on GitHub under [Issues](https://github.com/dorielrivalet/mhfz-overlay/issues).

- If you're unable to find an open issue addressing the problem, [open a new one](https://github.com/DorielRivalet/mhfz-overlay/issues/new?assignees=DorielRivalet&labels=bug&template=BUG-REPORT.yml&title=%5BBUG%5D+-+%3Ctitle%3E). Be sure to include a **title and clear description**, as much relevant information as possible, and a **reproducible test case** demonstrating the expected behavior that is not occurring.

- For reporting security vulnerabilities, please go [here](https://github.com/DorielRivalet/mhfz-overlay/security/advisories/new)

Alternatively, send an issue [here](https://github.com/DorielRivalet/mhfz-overlay/issues/new) detailing your inquiry about the program.

## Do you want to request a feature?

If you would like to send a feature request, please go [here](https://github.com/DorielRivalet/mhfz-overlay/issues/new?assignees=&labels=question%2Cenhancement&template=FEATURE-REQUEST.yml&title=%5BREQUEST%5D+-+%3Ctitle%3E)

## Did you write a patch that fixes a bug?

- Open a new GitHub pull request with the patch.

- Ensure the PR description clearly describes the problem and solution. Include the relevant issue number if applicable.

- Before submitting, please read the [technical documentation](hhttps://github.com/DorielRivalet/mhfz-overlay/tree/main/docs) to know more about coding conventions and benchmarks.

## Do you intend to add a new feature yourself or change an existing one?

- Suggest your change in a draft pull request and start writing code.

- Do not open an issue on GitHub until you have collected positive feedback about the change. GitHub issues are primarily intended for feature requests by end-users, bug reports and fixes.

## Do you want to contribute to the documentation?

This repository has two main sets of documentation: the files in the `docs` folder, which help you learn about the program, and the C# docblocks, which serves as a reference.

You can help improve the technical documentation or the C# docblocks by making them more coherent, consistent, or readable, adding missing information, correcting factual errors, fixing typos, or bringing them up to date with the latest changes.

To do so, make changes to the documentation source files (located here on GitHub) or C# comments in source code. Then open a pull request to apply your changes to the main branch.

When working with documentation, please take into account proper English grammatical structure and punctuation. Strive for Simple English whenever possible.

## Closing notes

This overlay is a volunteer effort. We encourage you to share it with fellow hunters!

Thanks! :heart: :heart: :heart:

@DorielRivalet
