import re
import datetime
import pandas as pd
import matplotlib.pyplot as plt
from pathlib import Path

author_name = "Doriel Rivalet"  # replace with your name for display
author_email = "100863878+DorielRivalet@users.noreply.github.com"  # replace with your email for identification
input_file = './input/git_anonymized.txt'
output_file = './output/commits_per_day_of_week.png'

data = []  # this will hold our commit times
commit_regex = r"Date:\s+(.+)"  # regex to match the datetime line
author_regex = r"Author:\s+(.+)"  # regex to match the author line
current_author = None

# get the current working directory
current_working_directory = Path.cwd()

# print output to the console
print(current_working_directory)

print("Calculating commits per day of the week... ")

with open(input_file, 'r', encoding='utf-8') as f:
    for line in f:
        author_match = re.search(author_regex, line)
        if author_match:
            current_author = author_match.group(1)
        date_match = re.search(commit_regex, line)
        if date_match and author_email in current_author:
            date_string = date_match.group(1)
            date = datetime.datetime.strptime(date_string, '%a %b %d %H:%M:%S %Y %z')
            data.append(date)

print("Data: ", data)

# Check if the data list is empty
if len(data) == 0:
    print(f"No data for author {author_name} ({author_email}).")
    exit()

df = pd.DataFrame(data, columns=['datetime'])
df['datetime'] = pd.to_datetime(df['datetime'], utc=True)

# Count the number of commits per day of the week
commits_per_day_of_week = df['datetime'].dt.dayofweek.value_counts().sort_index()

# Use a dark background
plt.style.use('dark_background')

# Set figure size
fig_width = 10  # width in inches
fig_height = 5.625  # height in inches (to maintain the 16:9 aspect ratio)
plt.figure(figsize=(fig_width, fig_height))

ax = plt.gca()
ax.bar(commits_per_day_of_week.index, commits_per_day_of_week.values, color='#f38ba8')
plt.xlabel('Day of the Week', color='#cdd6f4')
plt.ylabel('Number of Commits', color='#cdd6f4')
plt.title(f'Number of Commits per Day of the Week by {author_name}', color='#cdd6f4')
plt.xticks(range(7), ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'], color='#cdd6f4')
plt.yticks(color='#cdd6f4')  # Set y-ticks color

# Add horizontal lines at each y-tick
for y_marker in ax.get_yticks():
    ax.axhline(y=y_marker, color='#cdd6f4', linestyle='--', alpha=0.5)

# Calculate dpi for 1080p resolution
dpi_width = 1920 / fig_width
dpi_height = 1080 / fig_height
dpi = max(dpi_width, dpi_height)

# Set dpi to control the size of the output image
# Save the figure as a PNG image
plt.savefig(output_file, dpi=dpi)
plt.show()

print('Created commits per day of the week image succesfully')
