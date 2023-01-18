using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace MHFZ_Overlay
{
    public class OptionTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var selectedOption = item as string;
            if (selectedOption != null)
            {
                FrameworkElement element = container as FrameworkElement;
                if (element != null)
                {
                    return element.FindResource(selectedOption) as DataTemplate;
                }
            }
            return null;
        }
    }

}
