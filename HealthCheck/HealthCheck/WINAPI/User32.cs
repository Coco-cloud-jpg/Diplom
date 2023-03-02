using System.Runtime.InteropServices;
using System.Text;

namespace HealthCheck.WINAPI
{
    internal class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        //public const int SW_HIDE = 0;
        //public const int SW_SHOW = 5;
        //public const int SW_SHOWNORMAL = 1;
        //public const int SW_SHOWMAXIMIZED = 3;
        //public const int SW_RESTORE = 9;

        //[DllImport("user32.dll")]
        //public static extern bool SetForegroundWindow(IntPtr hWnd);
        //[DllImport("user32.dll")]
        //public static extern bool AllowSetForegroundWindow(uint dwProcessId);
        //[DllImport("user32.dll")]
        //public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, StringBuilder lParam);

    }
}
