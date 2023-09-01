// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZOverlayBenchmark.Comparisons;

using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public enum TimerFormat
{
    MinutesSeconds,
    MinutesSecondsMilliseconds,
    HoursMinutesSeconds,
}

public enum TimerMode
{
    TimeLeft,
    Elapsed,
}

// TODO optimize
[RPlotExporter]
[MedianColumn, MinColumn, MaxColumn]
[UnicodeConsoleLogger]
[MemoryDiagnoser]
[ExceptionDiagnoser]
public class TimerComparison
{
    public const decimal FramesPerSecond = 30;

    public string TimeLeftPercentNumber = " (100%)";

    [Params(120 * 60 * 30)]
    public int TimeDefInt;

    [Benchmark]
    public string BenchmarkSimpleTimer()
    {
        string s = string.Empty;
        for (int i = TimeDefInt; i > 0; i--)
        {
            s = SimpleTimer(i, TimerFormat.MinutesSecondsMilliseconds, true, TimeDefInt, true, TimeLeftPercentNumber, TimerMode.Elapsed);
        };

        return s;
    }

    [Benchmark]
    public string BenchmarkTimeSpanTimer()
    {
        string s = string.Empty;
        for (int i = TimeDefInt; i > 0; i--)
        {
            s = TimeSpanTimer(i, TimerFormat.MinutesSecondsMilliseconds, true, TimeDefInt, true, TimeLeftPercentNumber, TimerMode.Elapsed);
        };

        return s;
    }

    [Benchmark]
    public string BenchmarkStringBuilderTimer()
    {
        string s = string.Empty;
        for (int i = TimeDefInt; i > 0; i--)
        {
            s = StringBuilderTimer(i, TimerFormat.MinutesSecondsMilliseconds, true, TimeDefInt, true, TimeLeftPercentNumber, TimerMode.Elapsed);
        };

        return s;
    }

    public string SimpleTimer(decimal timeInt, TimerFormat timerFormat, bool isFrames = true, decimal timeDefInt = 0, bool timeLeftPercentShown = false, string timeLeftPercentNumber = "", TimerMode timerMode = TimerMode.Elapsed)
    {
        // TODO wrong conditionals for timeint >= timedefint?
        decimal time = timerMode == TimerMode.Elapsed && timeInt <= timeDefInt ? time = timeDefInt - timeInt : time = timeInt;
        decimal framesPerSecond = isFrames ? FramesPerSecond : 1;
        decimal milliseconds = time / framesPerSecond * 1000;
        decimal totalMinutes = Math.Floor(milliseconds / 60000);
        decimal minutes = totalMinutes >= 60 ? totalMinutes : Math.Floor(milliseconds / 60000);
        decimal seconds = Math.Floor((milliseconds - (minutes * 60000)) / 1000);
        decimal remainingMilliseconds = milliseconds - (minutes * 60000) - (seconds * 1000);
        var timeLeftPercent = timeLeftPercentShown ? timeLeftPercentNumber : string.Empty;

        return timerFormat switch
        {
            TimerFormat.MinutesSeconds => $"{minutes:00}:{seconds:00}" + timeLeftPercent,
            TimerFormat.MinutesSecondsMilliseconds => $"{minutes:00}:{seconds:00}.{remainingMilliseconds:000}" + timeLeftPercent,
            _ => $"{minutes:00}:{seconds:00}.{remainingMilliseconds:000}" + timeLeftPercent,
        };
    }

    public string StringBuilderTimer(decimal timeInt, TimerFormat timerFormat, bool isFrames = true, decimal timeDefInt = 0, bool timeLeftPercentShown = false, string timeLeftPercentNumber = "", TimerMode timerMode = TimerMode.Elapsed)
    {
        decimal time = timerMode == TimerMode.Elapsed && timeInt <= timeDefInt ? time = timeDefInt - timeInt : time = timeInt;
        decimal framesPerSecond = isFrames ? FramesPerSecond : 1;
        decimal totalSeconds = time / framesPerSecond;
        decimal totalMinutes = Math.Floor(totalSeconds / 60);
        decimal minutes = totalMinutes >= 60 ? totalMinutes : Math.Floor(totalSeconds / 60);
        decimal seconds = Math.Floor(totalSeconds % 60);
        decimal milliseconds = Math.Round((time % framesPerSecond) * (1000M / framesPerSecond));
        var timeLeftPercent = timeLeftPercentShown ? timeLeftPercentNumber : string.Empty;

        StringBuilder sb = new StringBuilder();
        switch (timerFormat)
        {
            default:
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
                break;
            case TimerFormat.MinutesSeconds:
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}", minutes, seconds);
                break;
            case TimerFormat.MinutesSecondsMilliseconds:
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
                break;
        }

        sb.Append(timeLeftPercent);
        return sb.ToString();
    }

    public string TimeSpanTimer(decimal timeInt, TimerFormat timerFormat, bool isFrames = true, decimal timeDefInt = 0, bool timeLeftPercentShown = false, string timeLeftPercentNumber = "", TimerMode timerMode = TimerMode.Elapsed)
    {
        decimal time = timerMode == TimerMode.Elapsed && timeInt <= timeDefInt ? time = timeDefInt - timeInt : time = timeInt;
        decimal framesPerSecond = isFrames ? FramesPerSecond : 1;
        decimal timeInSeconds = time / framesPerSecond;
        TimeSpan timeInSecondsSpan = TimeSpan.FromSeconds((double)timeInSeconds);
        int roundedMilliseconds = (int)(Math.Round(timeInSecondsSpan.TotalMilliseconds) % 1000);
        var totalMinutes = Math.Floor(timeInSecondsSpan.TotalSeconds / 60);
        var minutes = totalMinutes >= 60 ? totalMinutes : timeInSecondsSpan.Minutes;
        var timeLeftPercent = timeLeftPercentShown ? timeLeftPercentNumber : string.Empty;

        // Format the TimeSpan object as a string
        return timerFormat switch
        {
            TimerFormat.MinutesSeconds => $"{minutes:00}:{timeInSecondsSpan.Seconds:00}" + timeLeftPercent,
            TimerFormat.MinutesSecondsMilliseconds => $"{minutes:00}:{timeInSecondsSpan.Seconds:00}.{roundedMilliseconds:000}" + timeLeftPercent,
            _ => $"{minutes:00}:{timeInSecondsSpan.Seconds:00}.{roundedMilliseconds:000}" + timeLeftPercent,
        };
    }
}
