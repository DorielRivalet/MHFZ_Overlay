local lfs = require("lfs")

print('Renaming emails...')
local current_dir = lfs.currentdir()
print(current_dir)

local input_file = './input/git.txt'
local output_file = './input/git_anonymized.txt'
local keep_email = '100863878+DorielRivalet@users.noreply.github.com'
local renamed_email = 'unknown@thisisaplaceholderemail.com'
local email_regex = '<(.-)>'

-- Function to replace email addresses with <unknown@unknown.com>
local function replace_email(match)
    -- Check if the matched email is the one to keep
    if match == keep_email then
        return match
    else
        return renamed_email
    end
end

-- Read the content of the input file
local content
do
    local file = io.open(input_file, 'r')
    content = file:read('*all')
    file:close()
end

-- Count the total number of email addresses to be replaced
local total_emails = select(2, content:gsub(email_regex, ''))

-- Initialize progress variables
local progress = 0
local progress_bar_length = 50

-- Perform email anonymization using pattern matching and substitution
local anonymized_content = content:gsub(email_regex, function(match)
    progress = progress + 1
    local percent = progress / total_emails
    local num_bar_chars = math.floor(percent * progress_bar_length)
    local progress_bar = ('='):rep(num_bar_chars) .. (' '):rep(progress_bar_length - num_bar_chars)
    io.write('\r[' .. progress_bar .. '] ' .. string.format('%.2f%%', percent * 100))
    io.flush()
    return '<' .. replace_email(match) .. '>'
end)

-- Write the anonymized content to the output file
do
    local file = io.open(output_file, 'w')
    file:write(anonymized_content)
    file:close()
end

print('\nAnonymized git.txt created: ' .. output_file)
