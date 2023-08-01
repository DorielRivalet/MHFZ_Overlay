// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

// https://stackoverflow.com/a/65412682/18859245
namespace MHFZ_Overlay.Services.Hotkey;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;

public sealed class GlobalHotKey : IDisposable
{
    private static readonly InvisibleWindowForMessages Window = new ();

    static GlobalHotKey() => Window.KeyPressed += (s, e) => RegisteredHotKeys.ForEach(x =>
                                      {
                                          if (e.Modifier == x.Modifier && e.Key == x.Key)
                                          {
                                              x.Action();
                                          }
                                      });

    /// <summary>
    /// Registers a global hotkey.
    /// </summary>
    /// <param name="aKeyGesture">e.g. Alt + Shift + Control + Win + S.</param>
    /// <param name="aAction">Action to be called when hotkey is pressed.</param>
    /// <returns>true, if registration succeeded, otherwise false.</returns>
    public static bool RegisterHotKey(string aKeyGestureString, Action aAction)
    {
        var c = new KeyGestureConverter();
        var aKeyGesture = (KeyGesture?)c.ConvertFrom(aKeyGestureString);
        if (aKeyGesture == null)
        {
            return false;
        }

        return RegisterHotKey(aKeyGesture.Modifiers, aKeyGesture.Key, aAction);
    }

    public static bool RegisterHotKey(ModifierKeys aModifier, Key aKey, Action aAction)
    {
        if (aModifier == ModifierKeys.None)
        {
            throw new ArgumentException("Modifier must not be ModifierKeys.None");
        }

        if (aAction is null)
        {
            throw new ArgumentNullException(nameof(aAction));
        }

        var aVirtualKeyCode = (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(aKey);
        currentID++;
        var aRegistered = RegisterHotKey(
            Window.Handle,
            currentID,
            (uint)aModifier | MODNOREPEAT,
            (uint)aVirtualKeyCode);

        if (aRegistered)
        {
            RegisteredHotKeys.Add(new HotKeyWithAction(aModifier, aKey, aAction));
        }

        return aRegistered;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // unregister all the registered hot keys.
        for (var i = currentID; i > 0; i--)
        {
            UnregisterHotKey(Window.Handle, i);
        }

        // dispose the inner native window.
        Window.Dispose();
    }

    private static readonly uint MODNOREPEAT = 0x4000;

    private static int currentID;

    private static readonly List<HotKeyWithAction> RegisteredHotKeys = new ();

    // Registers a hot key with Windows.
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    private class HotKeyWithAction
    {
        public HotKeyWithAction(ModifierKeys modifier, Key key, Action action)
        {
            this.Modifier = modifier;
            this.Key = key;
            this.Action = action;
        }

        public ModifierKeys Modifier { get; }

        public Key Key { get; }

        public Action Action { get; }
    }

    // Unregisters the hot key with Windows.
    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private class InvisibleWindowForMessages : System.Windows.Forms.NativeWindow, IDisposable
    {
        private static readonly int WMHOTKEY = 0x0312;

        public InvisibleWindowForMessages() => this.CreateHandle(new System.Windows.Forms.CreateParams());

        public event EventHandler<HotKeyPressedEventArgs> KeyPressed;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WMHOTKEY)
            {
                var aWPFKey = KeyInterop.KeyFromVirtualKey(((int)m.LParam >> 16) & 0xFFFF);
                var modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);
                this.KeyPressed?.Invoke(this, new HotKeyPressedEventArgs(modifier, aWPFKey));
            }
        }

        public sealed class HotKeyPressedEventArgs : EventArgs
        {
            private readonly Key key;

            public HotKeyPressedEventArgs(ModifierKeys modifier, Key key)
            {
                this.Modifier = modifier;
                this.key = key;
            }

            public ModifierKeys Modifier { get; }

            public Key Key => this.key;
        }

        public void Dispose() => this.DestroyHandle();
    }
}
