# frozen_string_literal: true

require 'gruff'

author_name = 'Doriel Rivalet' # used for display only
author_email = '100863878+DorielRivalet@users.noreply.github.com' # used for identification
input_file = './input/git_anonymized.txt'
output_file = './output/commit_types.png'
valid_commit_types = %w[build chore ci docs feat fix perf refactor revert style test]

commit_type_counts = {
  'build' => 0,
  'chore' => 0,
  'ci' => 0,
  'docs' => 0,
  'feat' => 0,
  'fix' => 0,
  'perf' => 0,
  'refactor' => 0,
  'revert' => 0,
  'style' => 0,
  'test' => 0
}
email_regex = /<(.*)>/
commit_regex = /commit\s+(.+)/
author_regex = /Author:\s+(.+)/
commit_message_regex = /^\s+(.+)/
current_author = nil
current_commit = nil

puts 'Calculating commit types count...'

File.open(input_file, 'r').each_line do |line|
  author_match = line.match(author_regex)
  commit_match = line.match(commit_regex)
  if author_match
    current_author = author_match[1]
    email_match = current_author.match(email_regex)
    current_author = email_match[1] if email_match
  elsif commit_match && current_author == author_email
    current_commit = commit_match[1]
  else
    commit_message_match = line.match(commit_message_regex)
    if commit_message_match && current_commit
      commit_type = commit_message_match[1].split(':').first.strip.downcase
      commit_type_counts[commit_type] += 1 if valid_commit_types.include?(commit_type)
      current_commit = nil
    end
  end
end

puts "Data: #{commit_type_counts}"

# Check if the data array is empty
if commit_type_counts.values.all?(&:zero?)
  puts "No data for author #{author_name} (#{author_email})."
  exit
end

# Create the chart object
g = Gruff::Bar.new

g.hide_legend = true
g.title = "Commit types by #{author_name}"
g.y_axis_label = 'Count'
g.y_axis_increment = 50
g.show_labels_for_bar_values = true
g.minimum_value = 0
g.marker_font_size = 16

# Set the commit types as custom labels
commit_labels = valid_commit_types.map { |type| [valid_commit_types.index(type), type] }.to_h
g.labels = commit_labels

# Define a custom theme
g.theme = {
  colors: ['#f38ba8'],
  marker_color: '#cdd6f4',
  font_color: '#cdd6f4',
  background_colors: '#1e1e2e'
}

# Define a lambda function for label formatting to remove the .00
g.label_formatting = lambda do |value|
  value.to_i.to_s
end

# Make a single data series with all the commit counts
g.data('Commits', commit_type_counts.values)

g.write(output_file)
