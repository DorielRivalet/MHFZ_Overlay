// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Views.CustomControls;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for MainConfigurationActions.xaml. Drag and Drop, Restore and Save.
/// https://stackoverflow.com/questions/36128148/pass-click-event-of-child-control-to-the-parent-control.
/// </summary>
public partial class MainConfigurationActions : UserControl
{
    public MainConfigurationActions() => this.InitializeComponent();

    public event RoutedEventHandler? ConfigureButtonClicked;

    public event RoutedEventHandler? DefaultButtonClicked;

    public event RoutedEventHandler? SaveButtonClicked;

    protected virtual void OnConfigureButtonClicked(RoutedEventArgs e) => this.ConfigureButtonClicked?.Invoke(this, e);

    protected virtual void OnDefaultButtonClicked(RoutedEventArgs e) => this.DefaultButtonClicked?.Invoke(this, e);

    protected virtual void OnSaveButtonClicked(RoutedEventArgs e) => this.SaveButtonClicked?.Invoke(this, e);

    private void ConfigureButton_Click(object sender, RoutedEventArgs e) => this.OnConfigureButtonClicked(e);

    private void DefaultButton_Click(object sender, RoutedEventArgs e) => this.OnDefaultButtonClicked(e);

    private void SaveButton_Click(object sender, RoutedEventArgs e) => this.OnSaveButtonClicked(e);
}
