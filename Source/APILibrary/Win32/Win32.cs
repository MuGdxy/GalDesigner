using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace APILibrary.Win32
{
    public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

    public static class Internal
    {
        const string User32 = "user32.dll";
        const string Gdi32 = "gdi32.dll";
        const string kernel32 = "kernel32.dll";
        const string Imm32 = "Imm32.dll";

        public delegate IntPtr WndProc(IntPtr Hwnd, uint message,
           IntPtr wParam, IntPtr lParam);

        [DllImport(Imm32)]
        public static extern bool ImmDisableIME(int idThread);

        [DllImport(User32)]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport(kernel32)]
        public static extern int GetLastError();

        [DllImport(kernel32)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(User32, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DefWindowProc(IntPtr Hwnd, uint message,
            IntPtr wParam, IntPtr lParam);

        [DllImport(User32)]
        public static extern IntPtr LoadCursor(IntPtr hlnstance, uint IpCursorName);

        [DllImport(User32, CharSet = CharSet.Auto, EntryPoint = "LoadImageW")]
        public static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType,
            int cxDesired, int cyDesired, uint fuLoad);

        [DllImport(User32, CharSet = CharSet.Auto, EntryPoint = "CreateWindowExW")]
        public static extern IntPtr CreateWindowEx(uint DdwExStyle, string lpClassName, string lpWindowName,
            uint dwStyle, uint x, uint y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu,
            IntPtr hlnstance, IntPtr lpParam);

        [DllImport(User32)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport(User32)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport(User32)]
        public static extern int ShowCursor(bool bShow);

        [DllImport(User32)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y,
            int nWidth, int nHeight, bool bRepaint);

        [DllImport(User32)]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport(User32)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport(User32, CharSet = CharSet.Auto, EntryPoint = "SetWindowTextW")]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport(User32)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
            int Y, int cx, int cy, uint uFlags);

        [DllImport(User32)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport(User32)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport(User32)]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport(User32)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewlong);

        [DllImport(User32)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport(User32)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);

        [DllImport(User32)]
        public static extern int GetCurrentThreadId();

        [DllImport(User32)]
        public static extern IntPtr GetFocus();

        [DllImport(User32)]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey,
            byte bAlpha, uint dwFlags);

        [DllImport(User32, EntryPoint = "RegisterClass")]
        public static extern ushort RegisterAppinfo(ref AppInfo appinfo);

        [DllImport(User32, EntryPoint = "UnregisterClass", CharSet = CharSet.Auto)]
        public static extern bool UnRegisterAppinfo(string appinfo_name, IntPtr hInstance);

        [DllImport(User32)]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr IParam);

        [DllImport(User32)]
        public static extern bool PeekMessage(out Message message, IntPtr hwnd,
          int wMSGfilterMin, int wMsgFilterMax, int wRemoveMsg);

        [DllImport(User32)]
        public static extern bool TranslateMessage(ref Message message);

        [DllImport(User32)]
        public static extern bool DispatchMessage(ref Message message);

        [DllImport(User32)]
        public static extern void PostQuitMessage(int exitCode);

        [DllImport(User32)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport(User32)]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport(User32)]
        public static extern bool ReleaseCapture();

        [DllImport(User32)]
        public static extern short GetKeyState(int keyCode);

        [DllImport(User32)]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport(User32)]
        public static extern bool GetClipCursor(ref Rect lpRect);

        [DllImport(User32)]
        public static extern bool GetClientRect(IntPtr hWnd, ref Rect lpRect);

        [DllImport(User32)]
        public static extern bool ClipCursor(ref Rect lpRect);

        [DllImport(User32)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport(User32)]
        public static extern bool AdjustWindowRect(ref Rect lpRect, uint dwStyle, bool bMENU);

        [DllImport(Gdi32)]
        public static extern IntPtr GetStockObject(int fnObject);
    }
}
