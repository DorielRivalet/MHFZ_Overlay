using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

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
            if (option.Name == "Stats")//gear + graphs
                return (DataTemplate)((FrameworkElement)container).FindResource("StatsTemplate");
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
            else
                return (DataTemplate)((FrameworkElement)container).FindResource("DefaultTemplate");
        }
    }
}
