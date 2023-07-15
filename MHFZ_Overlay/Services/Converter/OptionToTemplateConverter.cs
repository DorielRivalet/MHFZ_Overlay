// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System.Windows;
using System.Windows.Controls;
using MHFZ_Overlay.Models;

public sealed class OptionToTemplateConverter : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is not QuestLogsOption option)
        {
            return null;
        }

        // this only needs run id
        if (option.Name == "Gear")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("GearTemplate");
        }

        // this only needs quest id
        else if (option.Name == "Top 20")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("Top20Template");
        }

        // this only needs quest id or nothing
        else if (option.Name == "Weapon Stats")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("WeaponStatsTemplate");
        }

        // this only needs quest id or nothing
        else if (option.Name == "Most Recent")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("MostRecentTemplate");
        }

        // this only needs run id
        else if (option.Name == "YouTube")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("YouTubeTemplate");
        }

        // quest id, run id, or nothing. quest id: general stats for that quest. run id: stats for that run. nothing: general stats for all of ur runs. or monster/gear info in general.
        else if (option.Name == "Stats (Graphs)")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("StatsGraphsTemplate");
        }

        // run id
        else if (option.Name == "Stats (Text)")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("StatsTextTemplate");
        }

        // quest id
        else if (option.Name == "Personal Best")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("PersonalBestTemplate");
        }
        else if (option.Name == "Compendium")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("CompendiumTemplate");
        }
        else if (option.Name == "Calendar")
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("CalendarTemplate");
        }
        else
        {
            return (DataTemplate)((FrameworkElement)container).FindResource("DefaultTemplate");
        }
    }
}
