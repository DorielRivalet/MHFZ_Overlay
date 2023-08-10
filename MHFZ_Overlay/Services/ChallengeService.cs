// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System.Diagnostics;
using System.Globalization;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Services.Contracts;

public sealed class ChallengeService : IChallenge
{
    public static ChallengeService GetInstance()
    {
        if (instance == null)
        {
            Logger.Debug("Singleton not found, creating instance.");
            instance = new ChallengeService();
        }

        Logger.Debug("Singleton found, returning instance.");
        Logger.Trace(new StackTrace().ToString());
        return instance;
    }

    /// <inheritdoc/>
    public bool CheckRequirements(Challenge challenge)
    {
        return false;
    }

    /// <inheritdoc/>
    public bool Unlock(Challenge challenge)
    {
        return false;
    }

    /// <inheritdoc/>
    public bool Start(Challenge challenge)
    {
        return false;
    }

    /// <inheritdoc/>
    public void Cancel(Challenge challenge)
    {

    }

    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    private static ChallengeService? instance;

    private ChallengeService() => Logger.Info(CultureInfo.InvariantCulture, $"Service initialized");
}
