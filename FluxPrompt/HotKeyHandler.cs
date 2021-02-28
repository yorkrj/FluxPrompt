using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FluxPrompt
{
    /// <summary>
    /// HotKeyHandler allows for the registration and handling of global hot keys even when the application has been minimized to the notification tray.
    /// </summary>
    class HotKeyHandler : NativeWindow
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr windowHandle, int id);

        public const int WM_HOTKEY = 0x0312;

        public event EventHandler<HotKeyPressedEventArgs> HotKeyPressed;

        public HotKeyHandler()
        {
            this.CreateHandle(new CreateParams());

            // TODO: Make this default hotkey configurable.
            Register(0,
                new HotKeyModifer[] { HotKeyModifer.Alt, HotKeyModifer.NoRepeat },
                Keys.Space.GetHashCode());
        }

        public bool Register(int id, HotKeyModifer[] modiferKeys, int virtualKeyCode)
        {
            int fsModifiers = 0;

            foreach (HotKeyModifer modiferKey in modiferKeys)
            {
                fsModifiers |= (int)modiferKey;
            }

            return RegisterHotKey(Handle, id, fsModifiers, virtualKeyCode);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int hotkeyID = m.WParam.ToInt32();

                HotKeyPressed?.Invoke(this, new HotKeyPressedEventArgs(hotkeyID));
            }
            base.WndProc(ref m);
        }

        public bool Unregister(int id)
        {
            return UnregisterHotKey(Handle, id);
        }

        /// <summary>
        /// Clean up registered hot keys. Object will be unusable after this call.
        /// </summary>
        public void Close()
        {
            UnregisterHotKey(Handle, 0); // TODO keep track of hot keys registered and unregister all here. For now we just have the one default.
            DestroyHandle();
        }
    }

    public enum HotKeyModifer
    {
        Alt = 0x0001,
        Control = 0x0002,
        Shift = 0x0004,
        Win = 0x0008,
        NoRepeat = 0x4000
    }

    public class HotKeyPressedEventArgs: EventArgs
    {
        private readonly int hotKeyIndex;

        public int HotKeyIndex 
        {
            get { return hotKeyIndex; } 
        }

        public HotKeyPressedEventArgs(int hotKeyIndex)
        {
            this.hotKeyIndex = hotKeyIndex;
        }
    }
}