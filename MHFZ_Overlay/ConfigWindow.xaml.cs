using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Input;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace MHFZ_Overlay
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {

        private MainWindow MainWindow { get; set; }

        public ConfigWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            Topmost = true;
            MainWindow = mainWindow;
            //GlobalHotKey.RegisterHotKey("Alt+Shift+a", () => SaveKey_Press());
            //GlobalHotKey.RegisterHotKey("Alt+Shift+b", () => CancelKey_Press());
            //GlobalHotKey.RegisterHotKey("Alt+Shift+c", () => DefaultKey_Press());
        }

        private void RoadOverrideTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "0" && e.Text != "1" && e.Text != "2")
            {
                e.Handled = true;
                return;
            }

        }

        private void RoadOverrideTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (RoadOverrideTextBox.Text.Length > 1)
            {
                RoadOverrideTextBox.Text = RoadOverrideTextBox.Text.Remove(0, 1);
                RoadOverrideTextBox.CaretIndex = 1;
            }
        }

        public void SaveKey_Press()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Save();
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Save();
            Close();
        }

        public void CancelKey_Press()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Reload();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Reload();
            Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Reload();
            MainWindow.DataLoader.model.Configuring = false;
        }

        public void DefaultKey_Press()
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Reset();
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            Settings s = (Settings)Application.Current.TryFindResource("Settings");
            s.Reset();
        }

        private void ConfigureButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.EnableDragAndDrop();
        }

        private void ValidateNumber(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
            {
                if (!char.IsNumber(ch))
                {
                    e.Handled = true;
                    return;
                }
            }
            if (e.Text.Length > 1 && e.Text[0] == '0')
                e.Handled = true;
        }

        //https://stackoverflow.com/questions/1051989/regex-for-alphanumeric-but-at-least-one-letter
        //^(?=.*[a-zA-Z].*)([a-zA-Z0-9]{6,12})$
        //([a-zA-Z0-9_\s]+)
        //[^a-zA-Z_0-9]

        //private string ValidateNamePattern = @"[^a-zA-Z_0-9]";

        private void ValidateName(object sender, TextCompositionEventArgs e)
        {
            // Create a Regex  
            //Regex rg = new Regex(ValidateNamePattern);

            // Get all matches  
            //MatchCollection matchedText = rg.Matches(e.Text);
            //https://stackoverflow.com/questions/1046740/how-can-i-validate-a-string-to-only-allow-alphanumeric-characters-in-it
            if (!(e.Text.All(char.IsLetterOrDigit)))
            {
                //just letters and digits.
                e.Handled = true;
            }

            //if (matchedText.Count == 0 && e.Text.Length >= 12)
             //   e.Handled = true;
        }

        //private void ValidateDiscordInvite(object sender, TextCompositionEventArgs e)
        //{
        //    if (!(e.Text.Substring(0,27) == "https://discord.com/invite/") )
        //        e.Handled = true;
        //}

        private void lnkImg_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    };


    /* LoadConfig on startup
     * Load Config on window open to have extra copy
     * On Save -> Window close -> tell programm to use new copy instead of current -> Save Config File
     * On Cancel -> Window Close -> Discard copy of config
     * On Config Change Still show changes immediatly and show windows which are set to show -> Ignore logic that hides windows during this time and force  them on if they are enabled
     * 
     */
}
