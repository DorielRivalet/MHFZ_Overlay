// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Template;

using System.Windows;
using System.Windows.Controls;

public sealed class OptionTemplateSelector : DataTemplateSelector
{
    /// <inheritdoc/>
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is string selectedOption)
        {
            if (container is FrameworkElement element)
            {
                return element.FindResource(selectedOption) as DataTemplate;
            }
        }

        return null;
    }
}
