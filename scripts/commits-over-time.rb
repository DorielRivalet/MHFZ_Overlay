require 'date'
require 'gruff'

author_name = 'Doriel Rivalet' # used for display only
author_email = '100863878+DorielRivalet@users.noreply.github.com' # used for identification
input_file = './input/git.txt'
output_file = './output/commits_over_time.png'

data = []
email_regex = /<(.*)>/ 
commit_regex = /Date:\s+(.+)/
author_regex = /Author:\s+(.+)/
current_author = nil

File.open(input_file, 'r').each_line do |line|
  author_match = line.match(author_regex)
  if author_match
    current_author = author_match[1]
    email_match = current_author.match(email_regex)
    current_author = email_match[1] if email_match
  else
    date_match = line.match(commit_regex)
    if date_match && current_author == author_email
      datetime = DateTime.parse(date_match[1])
      date = datetime.to_date.to_time.to_i # Convert date to integer for x coordinate
      hour = datetime.hour
      data << [date, hour]
    end
  end
end

puts "Data: #{data}"

# Check if the data array is empty
if data.empty?
  puts "No data for author #{author_name} (#{author_email})."
  exit
end

g = Gruff::Scatter.new(3840)  # the width of the image in pixels
g.title = "Commits over time by #{author_name}"
g.x_axis_label = "Day"
g.y_axis_label = "Hour"
g.y_axis_increment = 1
g.minimum_value = 0
g.maximum_value = 23
g.circle_radius = 2  # You can adjust the size of the dots here
g.hide_legend = true
g.label_margin = 0

# Set the label format from an integer into the date format YY-MM-DD
g.x_axis_label_format = lambda do |value|
  Time.at(value).strftime('%y-%m-%d')
end

# Split data into two arrays: dates and hours
dates, hours = data.transpose

# Define a custom theme with red dots
g.theme = {
  colors: ['#f38ba8'],
  marker_color: '#cdd6f4',
  font_color: '#cdd6f4',
  background_colors: '#1e1e2e'
}

g.data('Commits', dates, hours)
g.write(output_file)
