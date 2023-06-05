# Continuous Integration and Continuous Deployment (CI/CD) Process

## Table of Contents

- [Continuous Integration and Continuous Deployment (CI/CD) Process](#continuous-integration-and-continuous-deployment-cicd-process)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
    - [CI/CD Steps](#cicd-steps)
    - [Tools Used](#tools-used)

## Overview

Our development process follows the best practices of Continuous Integration and Continuous Deployment (CI/CD), which involve automating the building, testing, and deployment of software changes. This allows us to ensure the quality and reliability of our software, and deliver updates to our users quickly and efficiently.

### CI/CD Steps

- **Code Commit**: Developers commit their code changes to the version control system (*Git*) using appropriate commit messages that follow the commitlint style guidelines. *CircleCI* together with commitlint will automatically detect the commit style when pushing the commit, with *CircleCI* used alongside *GitHub Actions* detecting when pushing to the remote branch; commitlint will automatically alert in the command-line interface while `mhfz-overlay/.circleci/config.yml` will alert in GitHub.
- **Build and Code Integrity/Quality**: *GitHub Actions* is used to automatically build the software whenever changes are pushed to the repository. Code integrity and quality checks are performed using *SonarCloud*, which analyzes the code for code smells, security vulnerabilities, code coverage, and other issues.
- **Automated Testing**: *XUnit* is used for writing and executing unit tests to catch any regressions or issues. The automated tests are executed as part of the testing workflow to ensure the software's functionality and reliability.
- **Deployment to Test Environment**: A release branch is used for the test environment, where the software is deployed for testing in a staging environment. The software's project is published using *Visual Studio*, then the published software is packaged using clow.squirrel to create an installer/package for easy installation in the test environment.
- **Manual Testing**: The software is thoroughly tested in the test environment, including real-world testing in an external video game to ensure its usability and performance.
- **Deployment to Production Environment**: Once the software passes all the automated and manual testing stages, it is merged into the main branch (prod) and deployed to the production environment. *GitHub Releases* and the `npm run release` command are used to create versioned releases with appropriate release notes, changelog and documentation.
- **Continuous Monitoring and Feedback**: The software is continuously monitored in production using logging and performance monitoring tools, such as *NLog*. User feedback and bug reports are collected and used for continuous improvement of the software, using tools such as *GitHub Issues* and *Google Forms*. For dependency tracking and security issues, *Dependabot* alerts and *GitHub Insights* are used.

### Tools Used

- **Version Control**: *Git* with commitlint and *CircleCI* for enforcing commit message style.
- **Build and Automation**: *GitHub Actions* for building and testing the software automatically.
- **Code Integrity/Quality**: *SonarCloud* for code analysis, code coverage and quality checks. *Dependabot* alerts for vulnerability issues mainly relating to package-lock.json.
- **Packaging**: *clow.squirrel* for creating installers/packages.
- **Testing**: *XUnit* for writing and executing unit tests.
- **Release Management**: *GitHub Releases* for creating versioned releases with release notes and documentation.
- **Development Environment**: C# with *Visual Studio* and .NET 6.0 for developing the software.
- **Publishing**: *Visual Studio* for creating executables or installers.
- **External Testing**: Real-world testing in an external video game (*Monster Hunter Frontier Z*) to ensure usability and performance.

By following this CI/CD process and using the mentioned tools, we aim to ensure the quality, reliability, and timely delivery of our software updates to our users.
