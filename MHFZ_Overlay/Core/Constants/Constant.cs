using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHFZ_Overlay.Core.Constants;

public static class TimeFormats
{
    public const string HOURS_MINUTES_SECONDS_MILLISECONDS = @"hh\:mm\:ss\.fff";
    public const string MINUTES_SECONDS_MILLISECONDS = @"mm\:ss\.fff";
}

public static class Messages
{
    public const string RUN_NOT_FOUND = "Run Not Found";
    public const string QUEST_NOT_FOUND = "Quest Not Found";
    public const string INVALID_QUEST = "Invalid Quest";
    public const string NOT_A_NUMBER = "N/A";
    public const string TIMER_NOT_LOADED = "--:--.---";
    public const string FATAL_TITLE = "MHF-Z Overlay Fatal Error";
    public const string ERROR_TITLE = "MHF-Z Overlay Error";
    public const string WARNING_TITLE = "MHF-Z Overlay Warning";
    public const string INFO_TITLE = "MHF-Z Overlay Info";
    public const string DEBUG_TITLE = "MHF-Z Overlay Debug";
}
