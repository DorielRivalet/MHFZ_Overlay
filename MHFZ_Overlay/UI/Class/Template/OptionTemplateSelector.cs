// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System.Windows;
using System.Windows.Controls;

namespace MHFZ_Overlay.UI.Class.Template;

public class OptionTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
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
