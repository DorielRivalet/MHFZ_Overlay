// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Contracts;

using System.Windows.Media;

/// <summary>
/// https://stackoverflow.com/questions/50521363/soundplayer-adjustable-volume
/// </summary>
public interface IAudio
{
    /// <summary>
    /// Plays the audio file with volume of mainVolume * audioVolume. Maximum volume value in either parameter or the result has to be 1 (100%) at maximum.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="mediaplayer"></param>
    /// <param name="mainVolume"></param>
    /// <param name="audioVolume"></param>
    /// <returns>false if the audio could not be played</returns>
    bool Play(string fileName, MediaPlayer? mediaplayer, float mainVolume, float audioVolume = 1);
}
