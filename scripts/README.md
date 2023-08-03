# Scripts

## Input

The git information is retrieved in the following ways, in the root of the repository:

- Windows

```text
set LC_ALL=C.UTF-8
git log --numstat > ./scripts/input/git.txt
```

- Unix

```text
export LANG=en_US.UTF-8
git log --numstat > ./scripts/input/git.txt
```

## Dependencies

### Ruby

- Gruff
- RMagick

### Python

- Pandas
- Matplotlib
