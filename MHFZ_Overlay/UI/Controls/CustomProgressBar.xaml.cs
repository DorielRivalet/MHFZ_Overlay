using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MHFZ_Overlay.controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CustomProgressBar : UserControl, INotifyPropertyChanged
    {
        public CustomProgressBar()
        {
            InitializeComponent();
            DataContext = this;
        }
        #region UserInput

        /// <summary>
        /// Gets or sets the width of the row1.
        /// </summary>
        /// <value>
        /// The width of the row1.
        /// </value>
        public int Row1Width
        {
            get { return (int)GetValue(Row1WidthProperty); }
            set { SetValue(Row1WidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the row2.
        /// </summary>
        /// <value>
        /// The width of the row2.
        /// </value>
        public int Row2Width
        {
            get { return (int)GetValue(Row2WidthProperty); }
            set { SetValue(Row2WidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current number.
        /// </summary>
        /// <value>
        /// The current number.
        /// </value>
        public int NumCurr
        {
            get { return (int)GetValue(NumCurrProperty); }
            set { SetValue(NumCurrProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number maximum.
        /// </summary>
        /// <value>
        /// The number maximum.
        /// </value>
        public int NumMax
        {
            get { return (int)GetValue(NumMaxProperty); }
            set { SetValue(NumMaxProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the bar.
        /// </summary>
        /// <value>
        /// The color of the bar.
        /// </value>
        public Brush BarColor
        {
            get { return (Brush)GetValue(BarColorProperty); }
            set { SetValue(BarColorProperty, value); }
        }
        #endregion

        #region BindingRegisters
        public static readonly DependencyProperty Row1WidthProperty =
            DependencyProperty.Register("Row1Width", typeof(int), typeof(CustomProgressBar), new PropertyMetadata(1));
        public static readonly DependencyProperty Row2WidthProperty =
            DependencyProperty.Register("Row2Width", typeof(int), typeof(CustomProgressBar), new PropertyMetadata(1));
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(CustomProgressBar), new PropertyMetadata(""));
        public static readonly DependencyProperty NumCurrProperty =
            DependencyProperty.Register("NumCurr", typeof(int), typeof(CustomProgressBar), new PropertyMetadata(0));
        public static readonly DependencyProperty NumMaxProperty =
            DependencyProperty.Register("NumMax", typeof(int), typeof(CustomProgressBar), new PropertyMetadata(0));
        public static readonly DependencyProperty BarColorProperty =
            DependencyProperty.Register("BarColor", typeof(Brush), typeof(CustomProgressBar), new PropertyMetadata(null));

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Reloads the data.
        /// </summary>
        public void ReloadData()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        #endregion

        #region UsedBindings
        public string Width1 { get => Row1Width.ToString() + "*"; }
        public string Width2 { get => Row2Width.ToString() + "*"; }

        /// <summary>
        /// Shows the current hp percentage?
        /// </summary>
        /// <returns></returns>
        public static bool ShowCurrentHPPercentage()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableCurrentHPPercentage)
                return true;
            else
                return false;
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
                if (NumMax < NumCurr)
                {
                    return "0";
                }
                else
                {
                    return string.Format(" ({0:0}%)", (float)NumCurr / NumMax * 100.0);
                }
            }
        }

        /// <summary>
        /// The current hp percent
        /// </summary>
        private string CurrentHPPercent = "";

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
                if (Desc == "Poison" || Desc == "Sleep" || Desc == "Para." || Desc == "Blast" || Desc == "Stun")
                    return "Left";
                else
                    return "Right";
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
                if (NumMax == 0)
                    return NumCurr.ToString();

                if (NumMax < NumCurr && Description != "Poison" && Description != "Sleep" && Description != "Para." && Description != "Blast" && Description != "Stun")
                {
                    NumMax = 0;
                    NumMax += NumCurr;
                    if (ShowCurrentHPPercentage())
                    {
                        CurrentHPPercent = CurrentHPPercentNumber;
                    }
                    else
                    {
                        CurrentHPPercent = "";
                    }
                    return string.Format("{0}/{1}", NumCurr, NumMax) + CurrentHPPercent;
                }

                if (NumCurr == 0 && Description != "Poison" && Description != "Sleep" && Description != "Para." && Description != "Blast" && Description != "Stun")
                {
                    NumMax = 1;
                }
                if (ShowCurrentHPPercentage())
                {
                    CurrentHPPercent = CurrentHPPercentNumber;
                }
                else
                {
                    CurrentHPPercent = "";
                }
                return string.Format("{0}/{1}", NumCurr, NumMax) + CurrentHPPercent;
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
                if (NumMax == 0 || NumCurr == 0)
                    return 0f;
                return ((float)NumCurr / (float)NumMax) * 100f;
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
            get { return Description; }
            set { Description = value; }
        }
        #endregion
    }
}
