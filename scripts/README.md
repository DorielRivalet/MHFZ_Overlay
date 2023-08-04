# Scripts

## Input

The git information is retrieved in the following ways, in the root of the repository:

- Windows

```bash
set LC_ALL=C.UTF-8
git log --numstat > ./scripts/input/git.txt
```

- Unix

```bash
export LANG=en_US.UTF-8
git log --numstat > ./scripts/input/git.txt
```

We then run the following command:

```bash
luajit rename_emails.lua
```

## Dependencies

### Ruby

- Gruff
- RMagick

### Python

- Pandas
- Matplotlib

## Usage

```bash
python ./commits_per_hour.py; python ./commits_per_weekday.py
ruby ./commits_over_time.rb; ruby ./commits_type_count.rb
```

## GitHub Actions

TODO
