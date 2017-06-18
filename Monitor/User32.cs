using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Monitor
{
    class User32
    {

        /* Event values https://msdn.microsoft.com/ja-jp/library/windows/desktop/dd318066(v=vs.85).aspx */

        /// <summary>
        /// The foreground window has changed. The system sends this event even if the foreground window 
        /// has changed to another window in the same thread. Server applications never send this event.
        /// </summary>
        private const uint EVENT_SYSTEM_FOREGROUND = 0x0003;

        /// <summary>
        /// A window has received mouse capture. This event is sent by the system, never by servers.
        /// </summary>
        private const uint EVENT_SYSTEM_CAPTURESTART = 0x0008;
        private const uint WINEVENT_OUTOFCONTEXT = 1;

        /// <summary>
        /// Callback signature for when the active window changes
        /// </summary>
        public delegate void WinEventDelegate();

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32")]
        static extern int GetWindowThreadProcessId(int hwnd, ref int lpdwProcessId);

        public static Process GetForegroundProcess()
        {
            int pid = 0;
            int handle = (int)User32.GetForegroundWindow();
            User32.GetWindowThreadProcessId(handle, ref pid);
            return Process.GetProcessById(pid);
        }

        public static void SetClickHook(WinEventDelegate callback)
        {
            User32.SetWinEventHook(EVENT_SYSTEM_CAPTURESTART, EVENT_SYSTEM_CAPTURESTART, IntPtr.Zero, callback, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        public static void SetForegroundHook(WinEventDelegate callback)
        {
            User32.SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, callback, 0, 0, WINEVENT_OUTOFCONTEXT);
        }


    }
}
