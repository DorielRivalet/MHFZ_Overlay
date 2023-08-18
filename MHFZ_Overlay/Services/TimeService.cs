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
using NLog;

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

    /// <summary>
    /// Test the timer methods for equality up until the specified max time in frames.
    /// </summary>
    /// <param name="timeDefInt"></param>
    /// <returns>The string where the inequality happened.</returns>
    public static string TestTimerMethods(decimal timeDefInt)
    {
        decimal timeInt = timeDefInt;
        var maxTime = timeDefInt.ToString(TimeFormats.HoursMinutesSecondsMilliseconds, CultureInfo.InvariantCulture);
        for (decimal i = timeDefInt; i > 0M; i--)
        {
            var timer1Result = StringBuilderTimer(timeInt, timeDefInt);
            var timer2Result = TimeSpanTimer(timeInt, timeDefInt);
            var fastestResult = FastestTimer(timeDefInt - timeInt);

            if (timer1Result != timer2Result || fastestResult != timer1Result || fastestResult != timer2Result)
            {
                return $"timeDefInt: {timeDefInt} ({maxTime}) timeInt: {timeInt} | StringBuilder: {timer1Result} | TimeSpan: {timer2Result}";
            }

            timeInt--;
        }

        return $"No inequalities found.";
    }

    public static string FastestTimer(decimal time)
    {
        decimal frameValue = time; // Example frame value

        decimal milliseconds = frameValue / 30 * 1000;
        decimal minutes = Math.Floor(milliseconds / 60000);
        decimal seconds = Math.Floor((milliseconds - (minutes * 60000)) / 1000);
        decimal remainingMilliseconds = milliseconds - (minutes * 60000) - (seconds * 1000);

        return $"{minutes:00}:{seconds:00}.{remainingMilliseconds:000}";
    }

    public static string StringBuilderTimer(decimal timeInt, decimal timeDefInt, bool timeLeftPercentShown = false, string timeLeftPercentNumber = "")
    {
        if (timeInt <= 0M)
        {
            return "00:00.000";
        }

        decimal time = timeDefInt - timeInt;
        decimal framesPerSecond = Numbers.FramesPerSecond;
        decimal totalSeconds = time / framesPerSecond;
        decimal minutes = Math.Floor(totalSeconds / 60);
        decimal seconds = Math.Floor(totalSeconds % 60);
        decimal milliseconds = Math.Round((time % framesPerSecond) * (1000M / framesPerSecond));

        var timeLeftPercent = timeLeftPercentShown ? timeLeftPercentNumber : string.Empty;

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        sb.Append(timeLeftPercent);

        return sb.ToString();
    }

    public static string TimeSpanTimer(decimal timeInt, decimal timeDefInt)
    {
        if (timeInt <= 0M)
        {
            return "00:00.000";
        }

        decimal totalQuestDurationSeconds = timeDefInt / Numbers.FramesPerSecond; // Total duration of the quest in seconds
        decimal timeRemainingInSeconds = timeInt / Numbers.FramesPerSecond; // Time left in the quest in seconds

        // Calculate the elapsed time by subtracting the time left from the total duration
        decimal elapsedTimeSeconds = totalQuestDurationSeconds - timeRemainingInSeconds;

        // Create a TimeSpan object directly from elapsed time in seconds
        TimeSpan elapsedTime = TimeSpan.FromSeconds((double)elapsedTimeSeconds);

        // Format the TimeSpan object as a string
        string formattedElapsedTime = elapsedTime.ToString(TimeFormats.MinutesSecondsMilliseconds, CultureInfo.InvariantCulture);

        return formattedElapsedTime;
    }
}
