import re
import datetime
import pandas as pd
import matplotlib.pyplot as plt

author_name = "Doriel Rivalet"  # replace with your name for display
author_email = "100863878+DorielRivalet@users.noreply.github.com"  # replace with your email for identification
input_file = './input/git_anonymized.txt'
output_file = './output/commits_per_hour.png'

data = []  # this will hold our commit times
commit_regex = r"Date:\s+(.+)"  # regex to match the datetime line
author_regex = r"Author:\s+(.+)"  # regex to match the author line
current_author = None

print("Calculating commits per hour... ")

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
df['hour'] = df['datetime'].dt.hour  # extract the hour from the datetime
commits_per_hour = df['hour'].value_counts().sort_index()  # count the number of commits per hour

# Use a dark background
plt.style.use('dark_background')

# Set figure size
fig_width = 10  # width in inches
fig_height = 5.625  # height in inches (to maintain the 16:9 aspect ratio)
plt.figure(figsize=(fig_width, fig_height))

ax = plt.gca()
ax.bar(commits_per_hour.index, commits_per_hour.values, color='#f38ba8')
plt.xlabel('Hour of the Day', color='#cdd6f4')
plt.ylabel('Number of Commits', color='#cdd6f4')
plt.title(f'Number of Commits per Hour by {author_name}', color='#cdd6f4')
plt.xticks(range(24), color='#cdd6f4')  # Ensure all hours from 0 to 23 are shown
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