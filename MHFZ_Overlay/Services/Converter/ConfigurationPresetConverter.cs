// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using MHFZ_Overlay.Models.Structures;
using static MHFZ_Overlay.Services.OverlaySettingsService;

public static class ConfigurationPresetConverter
{
    public static ConfigurationPreset Convert(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return ConfigurationPreset.None;
        }

        return input.ToLowerInvariant() switch
        {
            "speedrun" => ConfigurationPreset.Speedrun,
            "zen" => ConfigurationPreset.Zen,
            "hp only" => ConfigurationPreset.HPOnly,
            "all" => ConfigurationPreset.All,
            _ => ConfigurationPreset.None,
        };
    }
}
