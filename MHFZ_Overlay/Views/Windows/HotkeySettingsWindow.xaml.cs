namespace MHFZ_Overlay.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MHFZ_Overlay.Services.Hotkey;
using Wpf.Ui.Controls;

// HotkeySettingsWindow.xaml.cs
public partial class HotkeySettingsWindow : FluentWindow
{
    private readonly HotkeyManager _hotkeyManager;
    private readonly Dictionary<System.Windows.Controls.TextBox, string> _originalHotkeys = new();

    public HotkeySettingsWindow(HotkeyManager hotkeyManager)
    {
        this.InitializeComponent();
        _hotkeyManager = hotkeyManager;
        LoadCurrentHotkeys();
    }

    private void LoadCurrentHotkeys()
    {
        var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

        this.OpenSettingsHotkey.Text = s.OpenSettingsHotkey;
        this.RestartProgramHotkey.Text = s.RestartProgramHotkey;
        this.CloseProgramHotkey.Text = s.CloseProgramHotkey;

        // Store original values for cancellation
        _originalHotkeys[this.OpenSettingsHotkey] = s.OpenSettingsHotkey;
        _originalHotkeys[this.RestartProgramHotkey] = s.RestartProgramHotkey;
        _originalHotkeys[this.CloseProgramHotkey] = s.CloseProgramHotkey;
    }

    private void Hotkey_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        var textBox = (System.Windows.Controls.TextBox)sender;

        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key == Key.Escape)
        {
            textBox.Text = _originalHotkeys[textBox];
            return;
        }

        var modifiers = new List<string>();
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            modifiers.Add("Ctrl");
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            modifiers.Add("Shift");
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            modifiers.Add("Alt");
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Windows))
            modifiers.Add("Win");

        if (key != Key.LeftCtrl && key != Key.RightCtrl &&
            key != Key.LeftAlt && key != Key.RightAlt &&
            key != Key.LeftShift && key != Key.RightShift &&
            key != Key.LWin && key != Key.RWin)
        {
            if (modifiers.Count == 0)
            {
                System.Windows.MessageBox.Show("Hotkey must include at least one modifier (Ctrl, Alt, Shift, or Win)",
                              "Invalid Hotkey", System.Windows.MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var hotkeyString = string.Join(" + ", modifiers.Concat(new[] { key.ToString() }));
            textBox.Text = hotkeyString;
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        _hotkeyManager.UpdateHotkey("OpenSettings", this.OpenSettingsHotkey.Text);
        _hotkeyManager.UpdateHotkey("RestartProgram", this.RestartProgramHotkey.Text);
        _hotkeyManager.UpdateHotkey("CloseProgram", this.CloseProgramHotkey.Text);
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
