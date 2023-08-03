// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

// https://stackoverflow.com/questions/8096852/brush-to-brush-animation
public sealed class BrushAnimation : AnimationTimeline
{
    public static readonly DependencyProperty FromProperty =
        DependencyProperty.Register("From", typeof(Brush), typeof(BrushAnimation));

    /// <inheritdoc/>
    public override Type TargetPropertyType => typeof(Brush);

    // we must define From and To, AnimationTimeline does not have this properties
    public Brush From
    {
        get => (Brush)this.GetValue(FromProperty);

        set => this.SetValue(FromProperty, value);
    }

    /// <inheritdoc/>
    public override object GetCurrentValue(
        object defaultOriginValue,
        object defaultDestinationValue,
        AnimationClock animationClock) => this.GetCurrentValue(
            defaultOriginValue as Brush,
            defaultDestinationValue as Brush,
            animationClock);

    public object GetCurrentValue(
        Brush? defaultOriginValue,
        Brush? defaultDestinationValue,
        AnimationClock animationClock)
    {
        if (!animationClock.CurrentProgress.HasValue)
        {
            return Brushes.Transparent;
        }

        // use the standard values if From and To are not set
        // (it is the value of the given property)
        defaultOriginValue = this.From ?? defaultOriginValue;
        defaultDestinationValue = this.To ?? defaultDestinationValue;

        if (animationClock.CurrentProgress.Value == 0 && defaultOriginValue != null)
        {
            return defaultOriginValue;
        }

        if (animationClock.CurrentProgress.Value == 1 && defaultDestinationValue != null)
        {
            return defaultDestinationValue;
        }

        return new VisualBrush(new Border()
        {
            Width = 1,
            Height = 1,
            Background = defaultOriginValue,
            Child = new Border()
            {
                Background = defaultDestinationValue,
                Opacity = animationClock.CurrentProgress.Value,
            },
        });
    }

    /// <inheritdoc/>
    protected override Freezable CreateInstanceCore() => new BrushAnimation();

    public Brush To
    {
        get => (Brush)this.GetValue(ToProperty);

        set => this.SetValue(ToProperty, value);
    }

    public static readonly DependencyProperty ToProperty =
        DependencyProperty.Register("To", typeof(Brush), typeof(BrushAnimation));
}
