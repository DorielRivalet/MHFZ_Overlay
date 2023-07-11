// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
namespace MHFZ_Overlay.Services.Converter;

using static MHFZ_Overlay.Services.Manager.OverlaySettingsManager;

public static class ConfigurationPresetConverter
{
    public static ConfigurationPreset Convert(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return ConfigurationPreset.None;
        }

        switch (input.ToLowerInvariant())
        {
            case "speedrun":
                return ConfigurationPreset.Speedrun;
            case "zen":
                return ConfigurationPreset.Zen;
            case "hp only":
                return ConfigurationPreset.HPOnly;
            case "all":
                return ConfigurationPreset.All;
            default:
                return ConfigurationPreset.None;
        }
    }
}
