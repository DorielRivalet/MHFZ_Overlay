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
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class MonsterHPBar : UserControl, INotifyPropertyChanged
{
    public MonsterHPBar()
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
        get { return (string)this.GetValue(DescriptionProperty); }
        set { this.SetValue(DescriptionProperty, value); }
    }

    public string BarType
    {
        get { return (string)this.GetValue(BarTypeProperty); }
        set { this.SetValue(BarTypeProperty, value); }
    }

    /// <summary>
    /// Gets or sets the current number.
    /// </summary>
    /// <value>
    /// The current number.
    /// </value>
    public int NumCurr
    {
        get { return (int)this.GetValue(NumCurrProperty); }
        set { this.SetValue(NumCurrProperty, value); }
    }

    /// <summary>
    /// Gets or sets the number maximum.
    /// </summary>
    /// <value>
    /// The number maximum.
    /// </value>
    public int NumMax
    {
        get { return (int)this.GetValue(NumMaxProperty); }
        set { this.SetValue(NumMaxProperty, value); }
    }

    /// <summary>
    /// Gets or sets the color of the bar.
    /// </summary>
    /// <value>
    /// The color of the bar.
    /// </value>
    public Brush BarColor
    {
        get { return (Brush)this.GetValue(BarColorProperty); }
        set { this.SetValue(BarColorProperty, value); }
    }

    public Brush StrokeColor
    {
        get { return (Brush)this.GetValue(StrokeColorProperty); }
        set { this.SetValue(StrokeColorProperty, value); }
    }

    public Brush BorderColor
    {
        get { return (Brush)this.GetValue(BorderColorProperty); }
        set { this.SetValue(BorderColorProperty, value); }
    }

    public string IconSource
    {
        get { return (string)this.GetValue(IconSourceProperty); }
        set { this.SetValue(IconSourceProperty, value); }
    }

    public string HPMode
    {
        get { return (string)this.GetValue(HPModeProperty); }
        set { this.SetValue(HPModeProperty, value); }
    }

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(MonsterHPBar), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty NumCurrProperty =
        DependencyProperty.Register("NumCurr", typeof(int), typeof(MonsterHPBar), new PropertyMetadata(0));

    public static readonly DependencyProperty NumMaxProperty =
        DependencyProperty.Register("NumMax", typeof(int), typeof(MonsterHPBar), new PropertyMetadata(0));

    public static readonly DependencyProperty BarColorProperty =
        DependencyProperty.Register("BarColor", typeof(Brush), typeof(MonsterHPBar), new PropertyMetadata(null));

    public static readonly DependencyProperty BorderColorProperty =
    DependencyProperty.Register("BorderColor", typeof(Brush), typeof(MonsterHPBar), new PropertyMetadata(null));

    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register("IconSource", typeof(string), typeof(MonsterHPBar), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register("StrokeColor", typeof(Brush), typeof(MonsterHPBar), new PropertyMetadata(null));

    public static readonly DependencyProperty BarTypeProperty =
       DependencyProperty.Register("BarType", typeof(string), typeof(MonsterHPBar), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty HPModeProperty =
        DependencyProperty.Register("HPMode", typeof(string), typeof(MonsterHPBar), new PropertyMetadata(string.Empty));

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Reloads the data.
    /// </summary>
    public void ReloadData()
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }

    /// <summary>
    /// Shows the current hp percentage?
    /// </summary>
    /// <returns></returns>
    public static bool ShowCurrentHPPercentage()
    {
        Settings s = (Settings)Application.Current.TryFindResource("Settings");
        return s.EnableCurrentHPPercentage;
    }

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
    /// The current hp percent
    /// </summary>
    private string CurrentHPPercent = string.Empty;

    /// <summary>
    /// Gets the descriptor horizontal alignment.
    /// </summary>
    /// <value>
    /// The descriptor horizontal alignment.
    /// </value>
    public string DescriptorHorizontalAlignment
    {
        get
        {
            if (this.Desc == "Poison" || this.Desc == "Sleep" || this.Desc == "Para." || this.Desc == "Blast" || this.Desc == "Stun")
            {
                return "Left";
            }
            else
            {
                return "Right";
            }
        }
    }

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
                    this.CurrentHPPercent = this.CurrentHPPercentNumber;
                }
                else
                {
                    this.CurrentHPPercent = string.Empty;
                }

                return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", this.NumCurr, this.NumMax) + this.CurrentHPPercent;
            }

            if (this.NumCurr == 0 && this.Description != "Poison" && this.Description != "Sleep" && this.Description != "Para." && this.Description != "Blast" && this.Description != "Stun")
            {
                this.NumMax = 1;
            }

            if (ShowCurrentHPPercentage())
            {
                this.CurrentHPPercent = this.CurrentHPPercentNumber;
            }
            else
            {
                this.CurrentHPPercent = string.Empty;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", this.NumCurr, this.NumMax) + this.CurrentHPPercent;
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

            return ((float)this.NumCurr / (float)this.NumMax) * 100f;
        }

        set
        {
            throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Desc
    {
        get { return this.Description; }
        set { this.Description = value; }
    }

    public string Icon
    {
        get { return this.IconSource; }
        set { this.IconSource = value; }
    }

    public string HPModeText
    {
        get { return this.HPMode; }
        set { this.HPMode = value; }
    }

    public string IconShown
    {
        get
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.MonsterBarIconShown)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
        }
    }

    public string DescriptionShown
    {
        get
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.MonsterBarNameShown)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
        }
    }

    public string HPModeShown
    {
        get
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.MonsterBarHPModeShown)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
        }
    }

    public string NumbersShown
    {
        get
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.MonsterBarNumbersShown)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
        }
    }

    public string BarShown
    {
        get
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");

            if (s.MonsterBarShown)
            {
                return "Visible";
            }
            else
            {
                return "Hidden";
            }
        }
    }
}
