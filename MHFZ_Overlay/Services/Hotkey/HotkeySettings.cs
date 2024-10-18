namespace MHFZ_Overlay.Services.Hotkey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// HotkeySettings.cs
public class HotkeySettings
{
    public string OpenSettings { get; set; } = "Shift + F1";
    public string RestartProgram { get; set; } = "Shift + F5";
    public string CloseProgram { get; set; } = "Shift + F6";
}

public class HotkeyManager : IDisposable
{
    private readonly Dictionary<string, Action> _hotkeyActions = new();
    private HotkeySettings _settings;
    private readonly GlobalHotKey _globalHotKey;

    public HotkeyManager()
    {
        _globalHotKey = new GlobalHotKey();
        LoadSettings();
    }


    public void LoadSettings()
    {
        var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

        // Load from user settings or create default
        _settings = new HotkeySettings
        {
            OpenSettings = s.OpenSettingsHotkey ?? "Shift + F1",
            RestartProgram = s.RestartProgramHotkey ?? "Shift + F5",
            CloseProgram = s.CloseProgramHotkey ?? "Shift + F6"
        };
    }

    public void SaveSettings()
    {
        var s = (Settings)System.Windows.Application.Current.TryFindResource("Settings");

        s.OpenSettingsHotkey = _settings.OpenSettings;
        s.RestartProgramHotkey = _settings.RestartProgram;
        s.CloseProgramHotkey = _settings.CloseProgram;
        s.Save();
    }

    public void RegisterHotkeys(Action openSettings, Action restart, Action close)
    {
        // Store the actions
        _hotkeyActions["OpenSettings"] = openSettings;
        _hotkeyActions["RestartProgram"] = restart;
        _hotkeyActions["CloseProgram"] = close;

        // Register hotkeys using existing GlobalHotKey class
        GlobalHotKey.RegisterHotKey(_settings.OpenSettings, openSettings);
        GlobalHotKey.RegisterHotKey(_settings.RestartProgram, restart);
        GlobalHotKey.RegisterHotKey(_settings.CloseProgram, close);
    }

    public void UpdateHotkey(string action, string newHotkey)
    {
        // Update the settings
        switch (action)
        {
            case "OpenSettings":
                _settings.OpenSettings = newHotkey;
                break;
            case "RestartProgram":
                _settings.RestartProgram = newHotkey;
                break;
            case "CloseProgram":
                _settings.CloseProgram = newHotkey;
                break;
        }

        SaveSettings();

        // Re-register the hotkey if we have an action for it
        if (_hotkeyActions.TryGetValue(action, out var hotkeyAction))
        {
            GlobalHotKey.RegisterHotKey(newHotkey, hotkeyAction);
        }
    }

    public void Dispose()
    {
        _globalHotKey?.Dispose();
    }
}
