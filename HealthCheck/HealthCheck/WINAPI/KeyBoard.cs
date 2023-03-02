using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HealthCheck.WINAPI
{
    /// <summary>
    /// A class that manages a global low level keyboard hook
    /// </summary>
    /// Винести в бібліотеку
    internal class Keyboard : IDisposable
    {
        #region Api

        private struct keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;

        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookCallback callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);
        [DllImport("user32.dll")]
        public static extern int ToUnicode(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpChar, int uFlags);

        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] pbKeyState);
        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        #endregion

        #region Fields & Properties

        /// <summary>
        /// defines the callback type for the hook
        /// </summary>
        private delegate int keyboardHookCallback(int code, int wParam, ref keyboardHookStruct lParam);

        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        private IntPtr hHook = IntPtr.Zero;

        /// <summary>
        /// The callback method reference
        /// </summary>
        private keyboardHookCallback khp;

        /// <summary>
        /// Key press supression
        /// </summary>
        private bool supressKeyPress = false;

        /// <summary>
        /// Gets or sets whether or not to hook all keys.
        /// </summary>
        public bool HookAllKeys { get; set; }

        /// <summary>
        /// is disposed or not
        /// </summary>
        private bool disposed;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when one of the hooked keys is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;
        /// <summary>
        /// Occurs when one of the hooked keys is released
        /// </summary>
        public event KeyEventHandler KeyUp;

        #endregion

        #region Constructor

        public Keyboard(bool supressKeyPress)
        {
            this.disposed = false;
            this.supressKeyPress = supressKeyPress;
            this.HookAllKeys = true;
            khp = new keyboardHookCallback(OnHookCallback);
        }

        ~Keyboard()
        {
            Dispose();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add hooked key
        /// </summary>
        public void AddHookedKey(Keys key)
        {
            ThrowIfDisposed();
        }
        public static char GetAsciiCharacter(int uVirtKey)
        {
            byte[] lpKeyState = new byte[256];
            GetKeyboardState(lpKeyState);
            byte[] lpChar = new byte[2];
            if (ToUnicode(uVirtKey, 0, lpKeyState, lpChar, 0) == 1)
                return Convert.ToChar((lpChar[0]));
            else
                return new char();
        }
        /// <summary>
        /// Remove hooked key
        /// </summary>
        public void RemoveHookedKey(Keys key)
        {
            ThrowIfDisposed();
        }

        /// <summary>
        /// Hook
        /// </summary>
        public void Hook()
        {
            ThrowIfDisposed();

            IntPtr hInstance = LoadLibrary("User32");
            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, khp, /*IntPtr.Zero*/ hInstance, 0);
        }

        /// <summary>
        /// Unhook
        /// </summary>
        public void Unhook()
        {
            ThrowIfDisposed();

            UnhookWindowsHookEx(hHook);
        }

        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        private int OnHookCallback(int code, int wParam, ref keyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;

                if (HookAllKeys)
                {
                    KeyEventArgs kea = new KeyEventArgs(key);

                    if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null))
                    {
                        OnKeyDown(kea);
                    }

                    /*if (kea.Handled && supressKeyPress)
                    {
                        return 1;
                    }*/
                }
            }

            int ret = CallNextHookEx(hHook, code, wParam, ref lParam);

            int error = Marshal.GetLastWin32Error();

            return ret;
        }

        /// <summary>
        /// Raises the KeyDown event.
        /// </summary>
        private void OnKeyDown(KeyEventArgs e)
        {
            if (KeyDown != null)
                KeyDown(this, e);
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.Unhook();
                this.khp = null;
                this.hHook = IntPtr.Zero;
                this.disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        #endregion
    }
}
