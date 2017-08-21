using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace APILibrary.Win32
{

    [StructLayout(LayoutKind.Sequential)]
    public partial struct Message
    {
        public IntPtr hwnd;
        public uint type;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public int x;
        public int y;

        public Message(IntPtr hWnd)
        {
            hwnd = hWnd;
            type = 0;
            wParam = IntPtr.Zero;
            lParam = IntPtr.Zero;
            time = 0;
            x = 0;
            y = 0;
        }

        public static int LowWord(IntPtr number)
            => (int)number & 0xffff;

        public static int HighWord(IntPtr number)
            => ((int)number >> 16) & 0xffff;
    }

}
