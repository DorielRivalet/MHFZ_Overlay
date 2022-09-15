using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MHFZ_Overlay.addresses;

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
        public int Row1Width
        {
            get { return (int)GetValue(Row1WidthProperty); }
            set { SetValue(Row1WidthProperty, value); }
        }
        public int Row2Width
        {
            get { return (int)GetValue(Row2WidthProperty); }
            set { SetValue(Row2WidthProperty, value); }
        }
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public int NumCurr
        {
            get { return (int)GetValue(NumCurrProperty); }
            set { SetValue(NumCurrProperty, value); }
        }
        public int NumMax
        {
            get { return (int)GetValue(NumMaxProperty); }
            set { SetValue(NumMaxProperty, value); }
        }
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


        public void ReloadData()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        #endregion

        #region UsedBindings
        public string Width1 { get => Row1Width.ToString() + "*"; }
        public string Width2 { get => Row2Width.ToString() + "*"; }
        public bool ShowCurrentHPPercentage()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            if (s.EnableCurrentHPPercentage == true)
                return true;
            else
                return false;
        }

        public string CurrentHPPercentNumber
        { get
            {
                if (NumMax < NumCurr)
                {
                    return "0";
                } else
                {
                    return string.Format(" ({0:0}%)",(float)NumCurr / NumMax * 100.0);
                }
            }
        }

        public string CurrentHPPercent = "";

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
                    } else
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

            }
        }

        public string Desc
        {
            get => Description; set => Description = value;
        }
        #endregion
    }
}
