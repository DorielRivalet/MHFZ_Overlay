// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Views;

using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Window;

/// <summary>
/// Interaction logic for SettingsForm.xaml
/// </summary>
public partial class SettingsForm : FluentWindow
{
    public SettingsForm()
    {
        InitializeComponent();
    }

    public bool IsDefaultSettingsSelected { get; private set; }

    public bool IsMonsterHpOnlySelected { get; private set; }

    public bool IsSpeedrunSelected { get; private set; }

    public bool IsEverythingSelected { get; private set; }

    public string MonsterHPModeSelected { get; private set; } = string.Empty;

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {

        // Get the selected radio button and handle the user's selection
        if (DefaultSettingsRadioButton.IsChecked == true)
        {

            // Handle default settings selection
            IsDefaultSettingsSelected = true;
        }
        else if (MonsterHPRadioButton.IsChecked == true)
        {

            // Handle monster HP only selection
            IsMonsterHpOnlySelected = true;

            // Get the selected ComboBox option
            var selectedComboBoxItem = MonsterHPComboBox.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem != null)
            {
                var selectedOption = selectedComboBoxItem.Content.ToString();

                // Handle the selected ComboBox option
                MonsterHPModeSelected = selectedOption;
            }
        }
        else if (SpeedrunModeRadioButton.IsChecked == true)
        {

            // Handle speedrun mode selection
            IsSpeedrunSelected = true;
        }
        else if (EnableAllFeaturesRadioButton.IsChecked == true)
        {

            // Handle enabling all features selection
            IsEverythingSelected = true;
        }
        else
        {
            return;
        }

        // Close the settings form
        this.DialogResult = true;
        Close();
    }
}
