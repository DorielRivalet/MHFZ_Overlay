// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Contracts;

using MHFZ_Overlay.Models;

public interface IChallenge
{
    /// <summary>
    /// Checks the requirements to unlock the challenge.
    /// </summary>
    /// <returns>false if the requirements are not met</returns>
    bool CheckRequirements(Challenge challenge);

    /// <summary>
    /// Unlocks the challenge. If using a data structure to avoid querying databases, that data structure should also be updated after storing the unlock date with this method.
    /// </summary>
    /// <returns>false if the challenge could not be unlocked</returns>
    bool Unlock(Challenge challenge);

    /// <summary>
    /// Starts the challenge. If there is already a challenge in progress, that challenge is canceled.
    /// </summary>
    /// <returns>false if the challenge could not be started</returns>
    bool Start(Challenge challenge);

    /// <summary>
    /// Cancels the challenge.
    /// </summary>
    /// <param name="challenge"></param>
    /// <returns>The challenge that was cancelled</returns>
    Challenge Cancel(Challenge challenge);
}
