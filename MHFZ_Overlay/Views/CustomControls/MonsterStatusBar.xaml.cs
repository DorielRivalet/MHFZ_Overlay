// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Views.CustomControls;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/// <summary>
/// Interaction logic for MonsterStatusBar.xaml.
/// </summary>
public partial class MonsterStatusBar : UserControl, INotifyPropertyChanged
{
    public MonsterStatusBar()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description
    {
        get => (string)this.GetValue(DescriptionProperty);
        set => this.SetValue(DescriptionProperty, value);
    }

    public string BarType
    {
        get => (string)this.GetValue(BarTypeProperty);
        set => this.SetValue(BarTypeProperty, value);
    }

    /// <summary>
    /// Gets or sets the current number.
    /// </summary>
    /// <value>
    /// The current number.
    /// </value>
    public int NumCurr
    {
        get => (int)this.GetValue(NumCurrProperty);
        set => this.SetValue(NumCurrProperty, value);
    }

    /// <summary>
    /// Gets or sets the number maximum.
    /// </summary>
    /// <value>
    /// The number maximum.
    /// </value>
    public int NumMax
    {
        get => (int)this.GetValue(NumMaxProperty);
        set => this.SetValue(NumMaxProperty, value);
    }

    /// <summary>
    /// Gets or sets the color of the bar.
    /// </summary>
    /// <value>
    /// The color of the bar.
    /// </value>
    public Brush BarColor
    {
        get => (Brush)this.GetValue(BarColorProperty);
        set => this.SetValue(BarColorProperty, value);
    }

    public Brush StrokeColor
    {
        get => (Brush)this.GetValue(StrokeColorProperty);
        set => this.SetValue(StrokeColorProperty, value);
    }

    public Brush BorderColor
    {
        get => (Brush)this.GetValue(BorderColorProperty);
        set => this.SetValue(BorderColorProperty, value);
    }

    public string IconSource
    {
        get => (string)this.GetValue(IconSourceProperty);
        set => this.SetValue(IconSourceProperty, value);
    }

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(MonsterStatusBar), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty NumCurrProperty =
        DependencyProperty.Register("NumCurr", typeof(int), typeof(MonsterStatusBar), new PropertyMetadata(0));

    public static readonly DependencyProperty NumMaxProperty =
        DependencyProperty.Register("NumMax", typeof(int), typeof(MonsterStatusBar), new PropertyMetadata(0));

    public static readonly DependencyProperty BarColorProperty =
        DependencyProperty.Register("BarColor", typeof(Brush), typeof(MonsterStatusBar), new PropertyMetadata(null));

    public static readonly DependencyProperty BorderColorProperty =
        DependencyProperty.Register("BorderColor", typeof(Brush), typeof(MonsterStatusBar), new PropertyMetadata(null));

    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register("IconSource", typeof(string), typeof(MonsterStatusBar), new PropertyMetadata(@"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/poison.png"));

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register("StrokeColor", typeof(Brush), typeof(MonsterStatusBar), new PropertyMetadata(null));

    public static readonly DependencyProperty BarTypeProperty =
       DependencyProperty.Register("BarType", typeof(string), typeof(MonsterStatusBar), new PropertyMetadata(string.Empty));

    /// <summary>
    /// The current hp percent.
    /// </summary>
    private string currentHPPercent = string.Empty;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets the current hp percent number.
    /// </summary>
    /// <value>
    /// The current hp percent number.
    /// </value>
    public string CurrentHPPercentNumber
    {
        get
        {
            if (this.NumMax < this.NumCurr)
            {
                return "0";
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, " ({0:0}%)", (float)this.NumCurr / this.NumMax * 100.0);
            }
        }
    }

    /// <summary>
    /// Shows the current hp percentage?.
    /// </summary>
    /// <returns></returns>
    public static bool ShowCurrentHPPercentage()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        if (s.EnableCurrentHPPercentage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Reloads the data.
    /// </summary>
    public void ReloadData() => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

    /// <summary>
    /// Gets the value text.
    /// </summary>
    /// <value>
    /// The value text.
    /// </value>
    public string ValueText
    {
        get
        {
            if (this.NumMax == 0)
            {
                return this.NumCurr.ToString(CultureInfo.InvariantCulture);
            }

            if (this.NumMax < this.NumCurr && this.Description != "Poison" && this.Description != "Sleep" && this.Description != "Para." && this.Description != "Blast" && this.Description != "Stun")
            {
                this.NumMax = 0;
                this.NumMax += this.NumCurr;
                if (ShowCurrentHPPercentage())
                {
                    this.currentHPPercent = this.CurrentHPPercentNumber;
                }
                else
                {
                    this.currentHPPercent = string.Empty;
                }

                return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", this.NumCurr, this.NumMax) + this.currentHPPercent;
            }

            if (this.NumCurr == 0 && this.Description != "Poison" && this.Description != "Sleep" && this.Description != "Para." && this.Description != "Blast" && this.Description != "Stun")
            {
                this.NumMax = 1;
            }

            if (ShowCurrentHPPercentage())
            {
                this.currentHPPercent = this.CurrentHPPercentNumber;
            }
            else
            {
                this.currentHPPercent = string.Empty;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", this.NumCurr, this.NumMax) + this.currentHPPercent;
        }
    }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value
    {
        get
        {
            if (this.NumMax == 0 || this.NumCurr == 0)
            {
                return 0f;
            }

            return this.NumCurr / (float)this.NumMax * 100f;
        }

        set => throw new InvalidOperationException();
    }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Desc
    {
        get => this.Description;
        set => this.Description = value;
    }

    public string Icon
    {
        get => this.IconSource;
        set => this.IconSource = value;
    }

    public static string IconShown
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.ProgressBarIconsShown)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
        }
    }

    public static string DescriptionShown
    {
        get
        {
            var s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.ProgressBarIconsShown)
            {
                return "Hidden";
            }
            else
            {
                return "Visible";
            }
        }
    }
}
