# Performance

## Table of Contents

- [Performance](#performance)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Glossary](#glossary)
  - [Metrics and Benchmarks](#metrics-and-benchmarks)
    - [Startup Time](#startup-time)
    - [Responsiveness](#responsiveness)
    - [Memory Usage](#memory-usage)
    - [Rendering Performance](#rendering-performance)
  - [Performance Testing Methodology](#performance-testing-methodology)
  - [Optimization Strategies](#optimization-strategies)
  - [Reporting and Tracking](#reporting-and-tracking)
    - [Tracking](#tracking)
    - [Reporting](#reporting)
  - [Performance Scenarios](#performance-scenarios)
  - [Performance Results](#performance-results)
    - [v0.25.0](#v0250)
  - [Performance Test Process](#performance-test-process)

## Overview

In this document we will examine the performance of the application, its goals, and an outline of our results using various methodologies and tools.

## Glossary

- Startup Time Metric: The time it takes for the application to launch and become fully functional.
- Responsiveness Metric: The time it takes for the application to respond to user input or interactions.
- Memory Usage Metric: The amount of memory the application consumes during runtime.
- Rendering Performance Metric: The frame rate or smoothness of visual elements in the application.

## Metrics and Benchmarks

In the context of performance benchmarking, *metric* refers to the specific measurement or indicator used to assess the performance of a particular aspect of the application. It quantifies a particular aspect of performance, such as time, size, or rate.

*Benchmark* refers to the target value or goal we set for the corresponding metric. It represents the desired performance level or threshold that we aim to achieve. The benchmark should be defined based on what we consider acceptable or optimal performance for the application.

### Startup Time

- Metric: Startup time in seconds.
- Benchmark: Less than 5 seconds.

### Responsiveness

- Metric: Average response time in milliseconds.
- Benchmark: Less than 100 milliseconds.

- Metric: Maximum response time in milliseconds.
- Benchmark: Less than 500 milliseconds.

### Memory Usage

- Metric: Memory usage in megabytes.
- Benchmark: Less than 500 megabytes.

### Rendering Performance

- Metric: Frame rate in frames per second (FPS).
- Benchmark: Consistently maintain 60 FPS.

## Performance Testing Methodology

We specify the tools we will use for performance testing, such as profiling tools or benchmarking frameworks, provide the techniques we will employ to measure and analyze performance, and outline the overall approach we will follow for performance testing.

- Tools: Visual Studio Profiler, System.Diagnostics.Stopwatch, NLog.
- Techniques: Profiling, Benchmarking, Logging.
- Methodology: Follow a systematic approach of designing test scenarios, collecting performance data, and analyzing the results. Generate the necessary source code changes for improving performance and compare results.

## Optimization Strategies

We outline specific strategies or approaches we plan to employ to optimize the performance of the application. These strategies can include code-level improvements, architectural changes, or other optimization techniques.

- Strategy 1: Database Query Optimization
- Description: Analyze and optimize database queries to improve data retrieval performance.

- Strategy 2: Caching Mechanism
- Description: Implement a caching mechanism to reduce the frequency of expensive computations and data fetch operations.

- Strategy 3: Compiled Bindings Optimization
- Description: Implement a compiled binding mechanism for a more efficient binding.

## Reporting and Tracking

### Tracking

To track and measure performance improvements over time, we can consider the following approaches:

- Implement performance monitoring tools and frameworks that provide insights into key performance metrics. These tools can automatically collect data on metrics like response times, resource utilization, and throughput.

- Set up a performance testing environment where you can run tests periodically to gather performance data.

- Use application performance management (APM) tools that offer real-time monitoring and analytics to track performance metrics and identify bottlenecks or areas for improvement.

- Employ logging and instrumentation within your application code to capture relevant performance-related data, such as execution times, database query durations, and network latency.

### Reporting

To report performance results, we can consider the following approaches:

- Develop automated reports that summarize performance metrics and trends. These reports can be generated periodically and shared to provide visibility into the performance of the application over time.

- Use visualization tools or dashboards to present performance data in a visually appealing and easily understandable format.

- Document performance findings, optimizations, and their impact on the application's performance in a central repository or knowledge base.

## Performance Scenarios

We describe a specific scenario or task for performance evaluation. These scenarios should represent real-world usage patterns or critical functionalities of the application.

- Scenario 1: Application Startup
  - Description: Measure the performance of the user starting the overlay, including memory usage, CPU usage and responsiveness.

- Scenario 2: Open Configuration Window.
  - Description: Measure the performance of the user opening the configuration window, including memory usage and responsiveness.

- Scenario 3: Data Processing.
  - Description: Evaluate the performance of processing large datasets, such as importing and analyzing data from the SQLite database file.

## Performance Results

### v0.25.0

**Test Scenario 1: Application Startup.**

- Metric 1: Average response time
  - Application: 5.56 seconds
  - MainWindow: 2.76 seconds
  - App ctor Elapsed Time: 314.691 ms
  - DataLoader ctor Elapsed Time: 2626.2459 ms
  - MainWindow ctor Elapsed Time: 3582.5787 ms

- Metric 2: CPU Utilization

|Name|Total CPU \[unit, %\]|Self CPU \[unit, %\]|
|-|-|-|
|\| + mhfz\_overlay|3561 \(38.17%\)|17 \(0.18%\)|
|\|\| - MHFZ\_Overlay.App.Main\(\)|3561 \(38.17%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.MainWindow.ctor\(\)|1828 \(19.59%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.DataLoader.ctor\(\)|1353 \(14.50%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.Core.Class.DataAccessLayer.DatabaseManager.SetupLocalDatabase\(MHFZ\_Overlay.DataLoader\)|1196 \(12.82%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.Core.Class.DataAccessLayer.DatabaseManager.CreateDatabaseTables\(System.Data.SQLite.SQLiteConnection, MHFZ\_Overlay.DataLoader\)|990 \(10.61%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.Core.Class.DataAccessLayer.DatabaseManager.InsertDictionaryDataIntoTable\(System.Collections.Generic.IReadOnlyDictionary\<int, string\>, string, string, string, System.Data.SQLite.SQLiteConnection\)|355 \(3.81%\)|2 \(0.02%\)|
|\|\| - MHFZ\_Overlay.UI.Class.OutlinedTextBlock.OnRender\(System.Windows.Media.DrawingContext\)|310 \(3.32%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.UI.Class.OutlinedTextBlock.EnsureGeometry\(\)|309 \(3.31%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.App.InitializeComponent\(\)|289 \(3.10%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.MainWindow.Timer\_Tick\(object, System.EventArgs\)|276 \(2.96%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.MainWindow.InitializeComponent\(\)|234 \(2.51%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.Addresses.AddressModel.ReloadData\(string\)|169 \(1.81%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.Addresses.AddressModelHGE.QuestID\(\)|155 \(1.66%\)|1 \(0.01%\)|
|\|\| - MHFZ\_Overlay.App.OnStartup\(System.Windows.StartupEventArgs\)|155 \(1.66%\)|0 \(0.00%\)|
|\|\| - MHFZ\_Overlay.App.cctor\(\)|106 \(1.14%\)|0 \(0.00%\)|

**Test Scenario 2: Open Configuration Window.**

- Metric 1: Average response time (first time, second time)
  1. ConfigWindow ctor Elapsed Time: 849.7549 ms
  2. ConfigWindow ctor Elapsed Time: 180.4346 ms

- Metric 3: Memory usage
  - 180 MB (before)
  - 450 MB (after)
  - 410 MB (closed)
  
**Test Scenario 3: Data Processing.**

- Metric 1: Average response time
  1. SetupLocalDatabase Elapsed Time: 1282.2002 ms

- Metric 2: CPU utilization

|Name|Total CPU \[unit, %\]|Self CPU \[unit, %\]|
|-|-|-|
|\| + system.data.sqlite|420 \(4.50%\)|420 \(4.50%\)|
|\|\| - System.Data.SQLite.SQLiteCommand.ExecuteNonQuery\(System.Data.CommandBehavior\)|352 \(3.77%\)|352 \(3.77%\)|
|\|\| - System.Data.SQLite.SQLiteConnection.Open\(\)|19 \(0.20%\)|19 \(0.20%\)|
|\|\| - System.Data.SQLite.SQLiteConnection.ctor\(string\)|15 \(0.16%\)|15 \(0.16%\)|
|\|\| - System.Data.SQLite.SQLiteCommand.ExecuteScalar\(System.Data.CommandBehavior\)|9 \(0.10%\)|9 \(0.10%\)|
|\|\| - System.Data.SQLite.SQLiteConnection.BeginTransaction\(\)|9 \(0.10%\)|9 \(0.10%\)|
|\|\| - System.Data.SQLite.SQLiteConnection.Dispose\(\)|5 \(0.05%\)|5 \(0.05%\)|
|\|\| - System.Data.SQLite.SQLiteTransaction.Commit\(\)|3 \(0.03%\)|3 \(0.03%\)|
|\|\| - System.Data.SQLite.SQLiteCommand.ExecuteNonQuery\(\)|2 \(0.02%\)|2 \(0.02%\)|
|\|\| - System.Data.SQLite.SQLiteCommand.ExecuteScalar\(\)|2 \(0.02%\)|2 \(0.02%\)|
|\|\| - System.Data.SQLite.SQLiteDataReader.NextResult\(\)|2 \(0.02%\)|2 \(0.02%\)|
|\|\| - System.Data.SQLite.SQLiteCommand.Dispose\(bool\)|1 \(0.01%\)|1 \(0.01%\)|
|\|\| - System.Data.SQLite.SQLiteCommand.ExecuteReader\(System.Data.CommandBehavior\)|1 \(0.01%\)|1 \(0.01%\)|

## Performance Test Process

- Define Performance Metrics: Determine the specific performance metrics you want to measure, such as response time, throughput, memory usage, or CPU utilization. Select metrics that are relevant to your application and align with your performance goals.

- Prepare Test Scenarios: Create a set of test scenarios that represent typical usage patterns or critical functionalities of your application. These scenarios should be designed to stress different aspects of your application's performance.

- Conduct Performance Tests: Execute the test scenarios on each version of your application that you want to compare. Measure and record the performance metrics for each test run. You can use benchmarking tools, profiling tools, or custom scripts to automate the test execution and data collection process.

- Compare Performance Results: Analyze the collected performance data for each version of your application. Compare the performance metrics to identify any performance improvements or regressions between versions. Look for significant differences and trends in the data.

- Document the Findings: Create a performance report or documentation that summarizes the performance results for each version tested. Include details such as the version number, test scenarios used, performance metrics measured, and any relevant observations or insights.

- Interpret the Results: Analyze the performance findings and interpret their implications. Identify areas where performance has improved, potential bottlenecks or regressions, and areas for further optimization. Use the results to guide your decision-making and prioritize performance optimization efforts.
