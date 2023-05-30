// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System.Windows;
using System.Windows.Controls;

namespace MHFZ_Overlay.UI.Class.Converter
{
    public class OptionToTemplateConverter : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var option = item as Option;
            if (option == null)
                return null;

            // this only needs run id
            if (option.Name == "Gear")//gear
                return (DataTemplate)((FrameworkElement)container).FindResource("GearTemplate");
            // this only needs quest id
            else if (option.Name == "Top 20")
                return (DataTemplate)((FrameworkElement)container).FindResource("Top20Template");
            // this only needs quest id or nothing
            else if (option.Name == "Weapon Stats")
                return (DataTemplate)((FrameworkElement)container).FindResource("WeaponStatsTemplate");
            // this only needs quest id or nothing
            else if (option.Name == "Most Recent")
                return (DataTemplate)((FrameworkElement)container).FindResource("MostRecentTemplate");
            // this only needs run id
            else if (option.Name == "YouTube")
                return (DataTemplate)((FrameworkElement)container).FindResource("YouTubeTemplate");
            // quest id, run id, or nothing. quest id: general stats for that quest. run id: stats for that run. nothing: general stats for all of ur runs. or monster/gear info in general.
            else if (option.Name == "Stats (Graphs)")
                return (DataTemplate)((FrameworkElement)container).FindResource("StatsGraphsTemplate");
            // run id
            else if (option.Name == "Stats (Text)")
                return (DataTemplate)((FrameworkElement)container).FindResource("StatsTextTemplate");
            // quest id
            else if (option.Name == "Personal Best")
                return (DataTemplate)((FrameworkElement)container).FindResource("PersonalBestTemplate");
            else if (option.Name == "Compendium")
                return (DataTemplate)((FrameworkElement)container).FindResource("CompendiumTemplate");
            else
                return (DataTemplate)((FrameworkElement)container).FindResource("DefaultTemplate");
        }
    }
}
