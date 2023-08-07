// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Contracts;

internal interface IAudio
{
    /// <summary>
    /// Plays the audio file with volume of mainVolume * audioVolume. Maximum volume value in either parameter or the result has to be 1 (100%) at maximum.
    /// </summary>
    /// <param name="mainVolume"></param>
    /// <param name="audioVolume"></param>
    void Play(float mainVolume, float audioVolume);
}
