// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System.Diagnostics;
using System.Globalization;
using MHFZ_Overlay;

public sealed class OverlaySettingsService
{
    private OverlaySettingsService()
    {
        logger.Info($"Service initialized");
    }

    public enum ConfigurationPreset
    {
        None,
        Speedrun,
        Zen,
        HPOnly,
        All,
    }

    public static OverlaySettingsService GetInstance()
    {
        if (instance == null)
        {
            logger.Debug("Singleton not found, creating instance.");
            instance = new OverlaySettingsService();
        }

        logger.Debug("Singleton found, returning instance.");
        logger.Trace(new StackTrace().ToString());
        return instance;
    }

    /// <summary>
    /// Sets the configuration preset. Does not save the settings.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="preset">The preset.</param>
    public void SetConfigurationPreset(Settings s, ConfigurationPreset preset)
    {
        switch (preset)
        {
            default:
                logger.Warn(CultureInfo.InvariantCulture, "Could not find preset name for settings");
                return;
            case ConfigurationPreset.None:
                return;
            case ConfigurationPreset.Speedrun:
                s.EnableDamageNumbers = false;
                s.EnableSharpness = false;
                s.PartThresholdShown = false;
                s.HitCountShown = false;
                s.PlayerAtkShown = false;
                s.MonsterAtkMultShown = false;
                s.MonsterDefrateShown = false;
                s.MonsterSizeShown = false;
                s.MonsterPoisonShown = false;
                s.MonsterParaShown = false;
                s.MonsterSleepShown = false;
                s.MonsterBlastShown = false;
                s.MonsterStunShown = false;
                s.DamagePerSecondShown = false;
                s.TotalHitsTakenBlockedShown = false;
                s.PlayerAPMGraphShown = false;
                s.PlayerAttackGraphShown = false;
                s.PlayerDPSGraphShown = false;
                s.PlayerHitsPerSecondGraphShown = false;
                s.EnableQuestPaceColor = false;
                s.Monster1HealthBarShown = false;
                s.Monster2HealthBarShown = false;
                s.Monster3HealthBarShown = false;
                s.Monster4HealthBarShown = false;
                s.EnableMap = false;
                s.PersonalBestTimePercentShown = false;
                s.EnablePersonalBestPaceColor = false;

                s.TimerInfoShown = true;
                s.EnableInputLogging = true;
                s.EnableQuestLogging = true;
                s.OverlayModeWatermarkShown = true;

                break;

            case ConfigurationPreset.Zen:
                s.EnableDamageNumbers = false;
                s.EnableSharpness = false;
                s.PartThresholdShown = false;
                s.HitCountShown = false;
                s.PlayerAtkShown = false;
                s.MonsterAtkMultShown = false;
                s.MonsterDefrateShown = false;
                s.MonsterSizeShown = false;
                s.MonsterPoisonShown = false;
                s.MonsterParaShown = false;
                s.MonsterSleepShown = false;
                s.MonsterBlastShown = false;
                s.MonsterStunShown = false;
                s.DamagePerSecondShown = false;
                s.TotalHitsTakenBlockedShown = false;
                s.PlayerAPMGraphShown = false;
                s.PlayerAttackGraphShown = false;
                s.PlayerDPSGraphShown = false;
                s.PlayerHitsPerSecondGraphShown = false;
                s.EnableQuestPaceColor = false;
                s.Monster1HealthBarShown = false;
                s.Monster2HealthBarShown = false;
                s.Monster3HealthBarShown = false;
                s.Monster4HealthBarShown = false;
                s.TimerInfoShown = false;
                s.EnableInputLogging = false;
                s.EnableMap = false;

                s.OverlayModeWatermarkShown = false;

                s.Monster1IconShown = true;
                break;

            case ConfigurationPreset.HPOnly:
                s.EnableDamageNumbers = false;
                s.EnableSharpness = false;
                s.PartThresholdShown = false;
                s.HitCountShown = false;
                s.PlayerAtkShown = false;
                s.MonsterAtkMultShown = false;
                s.MonsterDefrateShown = false;
                s.MonsterSizeShown = false;
                s.MonsterPoisonShown = false;
                s.MonsterParaShown = false;
                s.MonsterSleepShown = false;
                s.MonsterBlastShown = false;
                s.MonsterStunShown = false;
                s.DamagePerSecondShown = false;
                s.TotalHitsTakenBlockedShown = false;
                s.PlayerAPMGraphShown = false;
                s.PlayerAttackGraphShown = false;
                s.PlayerDPSGraphShown = false;
                s.PlayerHitsPerSecondGraphShown = false;
                s.TimerInfoShown = false;
                s.EnableMap = false;
                s.ActionsPerMinuteShown = false;
                s.PersonalBestShown = false;

                s.OverlayModeWatermarkShown = false;

                s.Monster1IconShown = false;

                s.Monster1HealthBarShown = true;
                s.Monster2HealthBarShown = true;
                s.Monster3HealthBarShown = true;
                s.Monster4HealthBarShown = true;
                break;

            case ConfigurationPreset.All:
                s.EnableDamageNumbers = true;
                s.EnableSharpness = true;
                s.PartThresholdShown = true;
                s.HitCountShown = true;
                s.PlayerAtkShown = true;
                s.MonsterAtkMultShown = true;
                s.MonsterDefrateShown = true;
                s.MonsterSizeShown = true;
                s.MonsterPoisonShown = true;
                s.MonsterParaShown = true;
                s.MonsterSleepShown = true;
                s.MonsterBlastShown = true;
                s.MonsterStunShown = true;
                s.DamagePerSecondShown = true;
                s.TotalHitsTakenBlockedShown = true;
                s.PlayerAPMGraphShown = true;
                s.PlayerAttackGraphShown = true;
                s.PlayerDPSGraphShown = true;
                s.PlayerHitsPerSecondGraphShown = true;
                s.EnableQuestPaceColor = true;
                s.Monster1HealthBarShown = true;
                s.Monster2HealthBarShown = true;
                s.Monster3HealthBarShown = true;
                s.Monster4HealthBarShown = true;
                s.TimerInfoShown = true;
                s.EnableInputLogging = true;
                s.EnableMap = true;

                s.OverlayModeWatermarkShown = true;

                s.Monster1IconShown = true;

                s.GamepadShown = true;
                s.KBMLayoutShown = true;
                s.LocationTextShown = true;
                s.QuestAttemptsShown = true;
                s.QuestIDShown = true;
                s.QuestNameShown = true;
                s.PersonalBestAttemptsShown = true;
                s.EnableQuestCompletionsCounter = true;
                s.EnableQuestLogging = true;
                s.EnableSharpnessPercentage = true;
                s.EnableTimeLeftPercentage = true;
                s.PersonalBestTimePercentShown = true;
                s.EnableCurrentHPPercentage = true;
                s.SessionTimeShown = true;
                s.EnableAverageActionsPerMinuteColor = true;
                s.EnableAverageHitsPerSecondColor = true;
                s.EnableDamageNumbersMulticolor = true;
                s.EnableHighestAtkColor = true;
                s.EnableDamageNumbersFlash = true;
                s.EnableDamageNumbersSize = true;
                s.EnableHighestMonsterAttackMultiplierColor = true;
                s.EnableLowestMonsterDefrateColor = true;
                s.EnableMonsterHPBarsAutomaticColor = true;
                s.EnableHighestDPSColor = true;
                s.EnablePersonalBestPaceColor = true;
                s.EnableQuestPaceColor = true;
                s.EnableTotalHitsTakenBlockedColor = true;
                break;
        }
    }

    private static OverlaySettingsService? instance;

    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
}
