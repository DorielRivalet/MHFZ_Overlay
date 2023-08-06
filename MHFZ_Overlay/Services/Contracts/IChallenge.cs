// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Contracts;

public interface IChallenge
{
    /// <summary>
    /// Checks the requirements to unlock the challenge.
    /// </summary>
    /// <param name="bronzeCount"></param>
    /// <param name="silverCount"></param>
    /// <param name="goldCount"></param>
    /// <param name="platinumCount"></param>
    /// <param name="secretID"></param>
    /// <returns>false if the requirements are not met</returns>
    bool CheckAchievementRequirements(int bronzeCount, int silverCount, int goldCount, int platinumCount, int secretID);

    /// <summary>
    /// Unlocks the challenge.
    /// </summary>
    /// <param name="checkRequirements"></param>
    /// <returns>false if the challenge could not be unlocked</returns>
    bool Unlock(bool checkRequirements);

    /// <summary>
    /// Starts the challenge. If there is already a challenge in progress, that challenge is canceled.
    /// </summary>
    /// <returns>false if the challenge could not be started</returns>
    bool Start();

    /// <summary>
    /// Cancels the challenge.
    /// </summary>
    void Cancel();
}
