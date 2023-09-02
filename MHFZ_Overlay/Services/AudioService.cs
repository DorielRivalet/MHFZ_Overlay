// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Diagnostics;
using System.Windows.Media;
using MHFZ_Overlay.Services.Contracts;
using NLog;
using SharpCompress.Common;

public sealed class AudioService : IAudio
{
    public static AudioService GetInstance()
    {
        if (instance == null)
        {
            LoggerInstance.Debug("Singleton not found, creating instance.");
            instance = new AudioService();
        }

        LoggerInstance.Debug("Singleton found, returning instance.");
        LoggerInstance.Trace(new StackTrace().ToString());
        return instance;
    }

    /// <inheritdoc/>
    public bool Play(string fileName, MediaPlayer? mediaPlayer, float mainVolume, float audioVolume = 1)
    {
        try
        {
            if (mediaPlayer == null)
            {
                LoggerInstance.Warn("Could not find media player");
                return false;
            }

            mediaPlayer.Open(new Uri(fileName));
            if (mainVolume > 1)
            {
                mainVolume = 1;
            }

            if (audioVolume > 1)
            {
                audioVolume = 1;
            }

            if (mainVolume < 0)
            {
                mainVolume = 0;
            }

            if (audioVolume < 0)
            {
                audioVolume = 0;
            }

            mediaPlayer.Volume = mainVolume * audioVolume;
            mediaPlayer.Play();
            return true;
        }
        catch(Exception ex)
        {
            LoggerInstance.Error(ex);
            return false;
        }
    }

    private AudioService() => LoggerInstance.Info($"Service initialized");
    private static readonly Logger LoggerInstance = LogManager.GetCurrentClassLogger();
    private static AudioService? instance;
}
