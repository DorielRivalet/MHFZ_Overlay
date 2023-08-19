// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;
using NLog;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

/// <summary>
/// A service for doing time and date manipulation. Consult the benchmarks project for the performance.
/// </summary>
public static class TimeService
{
    private static readonly Logger LoggerInstance = LogManager.GetCurrentClassLogger();

    public static double GetFramesFromTimeSpan(TimeSpan time)
    {
        return TimeSpan.FromSeconds(time.TotalSeconds * (double)Numbers.FramesPerSecond).TotalSeconds * (double)Numbers.FramesPerSecond;
    }

    public static TimeSpan GetTimeSpanFromFrames(decimal frames)
    {
        return TimeSpan.FromSeconds((double)frames / (double)Numbers.FramesPerSecond);
    }

    public static string GetTimeLeftPercent(decimal timeDefInt, decimal timeInt, bool isDure)
    {
        if (timeDefInt < timeInt || timeDefInt <= 0)
        {
            return " (?)";
        }
        else
        {
            return string.Format(CultureInfo.InvariantCulture, " ({0:0}%)", timeInt / timeDefInt * 100.0M);
        }
    }

    public static decimal GetTimeValue(TimerMode mode, decimal timeDefInt, decimal timeInt)
    {
        decimal time;

        if (mode == TimerMode.Elapsed)
        {
            time = timeDefInt - timeInt;
        }
        else // default to Time Left mode
        {
            time = timeInt;
        }

        return time;
    }

    /// <summary>
    /// Test the timer methods for equality up until the specified max time in frames.
    /// </summary>
    /// <param name="timeDefInt"></param>
    /// <returns>The string where the inequality happened.</returns>
    public static string TestTimerMethods(decimal timeDefInt)
    {
        decimal timeInt = timeDefInt;
        var maxTime = TimeSpan.FromSeconds((double)(timeDefInt / Numbers.FramesPerSecond));
        string timer1Result = string.Empty;
        string timer2Result = string.Empty;
        string timer3Result = string.Empty;

        for (decimal i = timeInt; i >= 0M; i--)
        {
            timer1Result = StringBuilderTimer(timeInt, TimerFormat.MinutesSecondsMilliseconds, true, timeDefInt, true, GetTimeLeftPercent(timeDefInt, timeInt, true), TimerMode.Elapsed);
            timer2Result = TimeSpanTimer(timeInt, timeDefInt, true, GetTimeLeftPercent(timeDefInt, timeInt, true), TimerMode.Elapsed);
            timer3Result = SimpleTimer(timeInt, timeDefInt, true, GetTimeLeftPercent(timeDefInt, timeInt, true), TimerMode.Elapsed);

            if (timer1Result != timer2Result || timer3Result != timer1Result || timer3Result != timer2Result)
            {
                return @$"timeDefInt: {timeDefInt} ({maxTime}) | timeInt: {timeInt}
StringBuilder: {timer1Result}
TimeSpan: {timer2Result}
Simple: {timer3Result}";
            }

            timeInt--;
        }

        return @$"No inequalities found.

timeDefInt: {timeDefInt} ({maxTime}) | timeInt: {timeInt}
StringBuilder: {timer1Result}
TimeSpan: {timer2Result}
Simple: {timer3Result}";
    }

    public static string SimpleTimer(decimal timeInt, decimal timeDefInt = 0, bool timeLeftPercentShown = false, string timeLeftPercentNumber = "", TimerMode timerMode = TimerMode.Elapsed)
    {
        // TODO wrong conditionals for timeint >= timedefint?
        decimal time = timerMode == TimerMode.Elapsed && timeInt <= timeDefInt ? time = timeDefInt - timeInt : time = timeInt;
        decimal milliseconds = time / 30 * 1000;
        decimal totalMinutes = Math.Floor(milliseconds / 60000);
        decimal minutes = totalMinutes >= 60 ? totalMinutes : Math.Floor(milliseconds / 60000);
        decimal seconds = Math.Floor((milliseconds - (minutes * 60000)) / 1000);
        decimal remainingMilliseconds = milliseconds - (minutes * 60000) - (seconds * 1000);
        var timeLeftPercent = timeLeftPercentShown ? timeLeftPercentNumber : string.Empty;

        return $"{minutes:00}:{seconds:00}.{remainingMilliseconds:000}" + timeLeftPercent;
    }

    public static string StringBuilderTimer(decimal timeInt, TimerFormat timerFormat, bool isFrames = true, decimal timeDefInt = 0, bool timeLeftPercentShown = false, string timeLeftPercentNumber = "", TimerMode timerMode = TimerMode.Elapsed)
    {
        decimal time = timerMode == TimerMode.Elapsed && timeInt <= timeDefInt ? time = timeDefInt - timeInt : time = timeInt;
        decimal framesPerSecond = isFrames ? Numbers.FramesPerSecond : 1;
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

    public static string TimeSpanTimer(decimal timeInt, decimal timeDefInt = 0, bool timeLeftPercentShown = false, string timeLeftPercentNumber = "", TimerMode timerMode = TimerMode.Elapsed)
    {
        decimal time = timerMode == TimerMode.Elapsed && timeInt <= timeDefInt ? time = timeDefInt - timeInt : time = timeInt;
        decimal timeInSeconds = time / Numbers.FramesPerSecond;
        TimeSpan timeInSecondsSpan = TimeSpan.FromSeconds((double)timeInSeconds);
        int roundedMilliseconds = (int)(Math.Round(timeInSecondsSpan.TotalMilliseconds) % 1000);
        var totalMinutes = Math.Floor(timeInSecondsSpan.TotalSeconds / 60);
        var minutes = totalMinutes >= 60 ? totalMinutes : timeInSecondsSpan.Minutes;
        var timeLeftPercent = timeLeftPercentShown ? timeLeftPercentNumber : string.Empty;

        // Format the TimeSpan object as a string
        return $"{minutes:00}:{timeInSecondsSpan.Seconds:00}.{roundedMilliseconds:000}" + timeLeftPercent;
    }

    /// <summary>
    /// Gets the elapsed time in the desired format.
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    public static string GetMinutesSecondsFromSeconds(double seconds) => StringBuilderTimer((long)seconds, TimerFormat.MinutesSeconds, false);

    /// <summary>
    /// Gets the elapsed time in the desired format.
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    public static string GetMinutesSecondsFromFrames(double frames) => StringBuilderTimer((long)frames, TimerFormat.MinutesSeconds);

    /// <summary>
    /// Gets the elapsed time in the desired format.
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    public static string GetMinutesSecondsMillisecondsFromFrames(double frames) => StringBuilderTimer((long)frames, TimerFormat.MinutesSecondsMilliseconds);

    /// <summary>
    /// Gets the elapsed time in the desired format.
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    public static string GetMinutesSecondsMillisecondsFromFrames(long frames) => StringBuilderTimer(frames, TimerFormat.MinutesSecondsMilliseconds);
}
