# Structured Logging

## Overview

Our software uses structured logging to provide a clear and concise record of events and operations that occur during runtime. To facilitate structured logging, we use [NLog](https://github.com/NLog/NLog), a free logging platform for .NET with rich log routing and management capabilities.

### Logging with NLog

We have implemented a labeling system to categorize log messages into three main categories:

- DATABASE OPERATION: Log messages related to database operations.
- FILE OPERATION: Log messages related to I/O operations.
- PROGRAM OPERATION: Log messages related to any other operations.

All logs are saved to a file called `logs.log`, which is located in the same directory as the program. In the event of a crash, crash logs will be created with information about the crash, including the date and time, in the file name.

### Goals

The main goals of our structured logging implementation are to:

- Provide clear and concise logs that facilitate debugging and issue resolution.
- Categorize logs into relevant categories to help identify the source of issues.
- Ensure logs are easily accessible and stored in a standardized format.
