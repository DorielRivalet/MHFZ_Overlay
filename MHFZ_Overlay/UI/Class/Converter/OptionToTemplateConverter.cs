using System.Windows;
using System.Windows.Controls;

namespace MHFZ_Overlay
{
    public class OptionToTemplateConverter : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var option = item as Option;
            if (option == null)
                return null;

            //this only needs run id
            if (option.Name == "Gear")//gear
                return (DataTemplate)((FrameworkElement)container).FindResource("GearTemplate");
            //this only needs quest id
            else if (option.Name == "Top 20")
                return (DataTemplate)((FrameworkElement)container).FindResource("Top20Template");
            //this only needs quest id or nothing
            else if (option.Name == "Weapon Usage")
                return (DataTemplate)((FrameworkElement)container).FindResource("WeaponUsageTemplate");
            //this only needs quest id or nothing
            else if (option.Name == "Most Recent")
                return (DataTemplate)((FrameworkElement)container).FindResource("MostRecentTemplate");
            //this only needs run id
            else if (option.Name == "YouTube")
                return (DataTemplate)((FrameworkElement)container).FindResource("YouTubeTemplate");
            //quest id, run id, or nothing. quest id: general stats for that quest. run id: stats for that run. nothing: general stats for all of ur runs. or monster/gear info in general.
            else if (option.Name == "Graphs")
                return (DataTemplate)((FrameworkElement)container).FindResource("GraphsTemplate");
            //run id
            else if (option.Name == "Inventories")
                return (DataTemplate)((FrameworkElement)container).FindResource("InventoriesTemplate");
            else
                return (DataTemplate)((FrameworkElement)container).FindResource("DefaultTemplate");
        }
    }
}
