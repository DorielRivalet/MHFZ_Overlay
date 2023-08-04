# Scripts

- [Scripts](#scripts)
  - [Input](#input)
    - [Windows](#windows)
    - [Unix](#unix)
    - [Email Renaming](#email-renaming)
  - [Dependencies](#dependencies)
    - [Ruby](#ruby)
    - [Python](#python)
  - [Usage](#usage)
  - [GitHub Actions](#github-actions)

## Input

The git information is retrieved in the following ways, in the root of the repository:

### Windows

- Open the command prompt.
- Set the LC_ALL environment variable to C.UTF-8 by running the command:

```bash
set LC_ALL=C.UTF-8
```

- Run the following command to generate the git information file:

```bash
git log --numstat > ./scripts/input/git.txt
```

### Unix

- Open the terminal.
- Set the LANG environment variable to en_US.UTF-8 by running the command:

```bash
export LANG=en_US.UTF-8
```

- Run the following command to generate the git information file:

```bash
git log --numstat > ./scripts/input/git.txt
```

### Email Renaming

After generating the git information file, you need to anonymize the emails. Run the following command:

```bash
luajit rename_emails.lua
```

## Dependencies

Make sure you have the following dependencies installed:

### Ruby

- Gruff
- RMagick

You can install these dependencies by running the following command:

```bash
gem install gruff rmagick
```

### Python

- Pandas
- Matplotlib

You can install these dependencies by running the following command:

```bash
pip install pandas matplotlib
```

## Usage

To generate the git statistics, run the following commands:

```bash
python ./commits_per_hour.py; python ./commits_per_weekday.py
ruby ./commits_over_time.rb; ruby ./commits_type_count.rb
```

These commands will generate the necessary images and statistics based on the git information.

## GitHub Actions

The [workflow](../.github/workflows/automate-git-stats.yml) automatically does the steps above every 3 months, it installs the necessary dependencies by checking the contents of the [dependencies folder](./dependencies/).
