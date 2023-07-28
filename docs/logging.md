# Structured Logging

## Table of Contents

- [Structured Logging](#structured-logging)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
    - [Logging with NLog](#logging-with-nlog)
    - [Goals](#goals)

## Overview

Our software uses structured logging to provide a clear and concise record of events and operations that occur during runtime. To facilitate structured logging, we use [NLog](https://github.com/NLog/NLog), a free logging platform for .NET with rich log routing and management capabilities.

### Logging with NLog

All logs are saved to a file called `logs.log`, which is inside the logs folder, located in the same directory as the program. In the event of a crash, crash logs will be created with information about the crash, including the date and time, in the file name. Should the application exit because of such errors, the log will be labeled as FATAL.

### Goals

The main goals of our structured logging implementation are to:

- Provide clear and concise logs that facilitate debugging and issue resolution.
- Categorize logs into relevant categories to help identify the source of issues.
- Ensure logs are easily accessible and stored in a standardized format.
