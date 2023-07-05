// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MHFZ_Overlay.Core.Class.OverlaySettings.OverlaySettingsManager;

namespace MHFZ_Overlay.UI.Class.Converter;

public static class ConfigurationPresetConverter
{
    public static ConfigurationPreset Convert(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return ConfigurationPreset.None;
        }

        switch (input.ToLower())
        {
            case "speedrun":
                return ConfigurationPreset.Speedrun;
            case "zen":
                return ConfigurationPreset.Zen;
            case "hp only":
                return ConfigurationPreset.HP_Only;
            case "all":
                return ConfigurationPreset.All;
            default:
                return ConfigurationPreset.None;
        }
    }
}
