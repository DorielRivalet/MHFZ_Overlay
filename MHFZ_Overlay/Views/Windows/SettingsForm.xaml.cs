// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Views.Windows;

using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

/// <summary>
/// Interaction logic for SettingsForm.xaml.
/// </summary>
public partial class SettingsForm : FluentWindow
{
    public SettingsForm() => this.InitializeComponent();

    public bool IsDefaultSettingsSelected { get; private set; }

    public bool IsMonsterHpOnlySelected { get; private set; }

    public bool IsSpeedrunSelected { get; private set; }

    public bool IsEverythingSelected { get; private set; }

    public string MonsterHPModeSelected { get; private set; } = string.Empty;

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        // Get the selected radio button and handle the user's selection
        if (this.DefaultSettingsRadioButton.IsChecked == true)
        {
            // Handle default settings selection
            this.IsDefaultSettingsSelected = true;
        }
        else if (this.MonsterHPRadioButton.IsChecked == true)
        {
            // Handle monster HP only selection
            this.IsMonsterHpOnlySelected = true;

            // Get the selected ComboBox option
            if (this.MonsterHPComboBox.SelectedItem is ComboBoxItem selectedComboBoxItem)
            {
                var selectedOption = selectedComboBoxItem.Content.ToString();

                // Handle the selected ComboBox option
                this.MonsterHPModeSelected = selectedOption;
            }
        }
        else if (this.SpeedrunModeRadioButton.IsChecked == true)
        {
            // Handle speedrun mode selection
            this.IsSpeedrunSelected = true;
        }
        else if (this.EnableAllFeaturesRadioButton.IsChecked == true)
        {
            // Handle enabling all features selection
            this.IsEverythingSelected = true;
        }
        else
        {
            return;
        }

        // Close the settings form
        this.DialogResult = true;
        this.Close();
    }
}
