using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder
{
    public static partial class Application
    {
        private static APILibrary.Win32.AppInfo appinfo;
        private static APILibrary.Win32.AppInfoStyle appstyle = APILibrary.Win32.AppInfoStyle.CS_DBLCLKS;

        private static string appIcon = null;

        private static List<GenericWindow> Windows = new List<GenericWindow>();

        private static List<GenericWindow> AddList = new List<GenericWindow>();
        private static List<GenericWindow> RemoveList = new List<GenericWindow>();

        private static int updateCount = 0;
        private static int sleepTime = 0;

        private static void PumpMessage()
        {
            while (APILibrary.Win32.Internal.PeekMessage(out APILibrary.Win32.Message message, IntPtr.Zero, 0, 0,
                (int)APILibrary.Win32.PeekMessageFlags.PM_REMOVE) is true)
            {
                APILibrary.Win32.Internal.TranslateMessage(ref message);
                APILibrary.Win32.Internal.DispatchMessage(ref message);
            }
        }

        private static bool IsNoWindow()
        {
            return (Windows.Count == 0 && AddList.Count == 0);
        }

        private static bool IsRunLoopEnd()
        {
            return IsNoWindow();
        }

        static Application()
        {
            appinfo = new APILibrary.Win32.AppInfo()
            {
                style = (uint)appstyle,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = APILibrary.Win32.Internal.GetModuleHandle(null),
                hbrBackground = APILibrary.Win32.Internal.GetStockObject(0),
                hIcon = APILibrary.Win32.Internal.LoadImage(IntPtr.Zero, appIcon, 1, 0, 0, 0x00000010),
                hCursor = APILibrary.Win32.Internal.LoadCursor(IntPtr.Zero, (uint)APILibrary.Win32.CursorType.IDC_ARROW),
                lpszMenuName = null
            };
        }

        public static void Add(GenericWindow window)
        {
            AddList.Add(window);
        }

        public static void Remove(GenericWindow window)
        {
            RemoveList.Add(window);
        }

        public static void RunLoop()
        {
            float delta = (updateCount != 0) ? 1f / updateCount : 0;

            float passtime = delta;
            DateTime last_time = DateTime.Now;

            while (IsRunLoopEnd() is false)
            {
                delta = (updateCount != 0) ? 1f / updateCount : 0;
                System.Threading.Thread.Sleep(sleepTime);

                DateTime current_time = DateTime.Now;
                passtime += (float)(current_time - last_time).TotalSeconds;
                last_time = current_time;

                if (passtime < delta) continue;

                passtime -= delta;

                PumpMessage();

                foreach (var item in Windows)
                {
                    if (item.IsEnable is false) continue;
                    if (item.IsVailed is false) { Remove(item); continue; }

                    item.PrivateOnUpdate(item);
                }

                foreach (var item in RemoveList) Windows.Remove(item);
                foreach (var item in AddList) Windows.Add(item);

                RemoveList.Clear();
                AddList.Clear();
            }
        }

        public static bool IsKeyDown(KeyCode keycode)
        {
            return APILibrary.Win32.Internal.GetKeyState((int)keycode) < 0;
        }

        public static string AppIcon
        {
            get => appIcon;
            set { appIcon = value; appinfo.hIcon = APILibrary.Win32.Internal.LoadImage(IntPtr.Zero, appIcon, 1, 0, 0, 0x00000010); }
        }

        public static int UpdateCount { set => updateCount = value; get => updateCount; }

        public static int SleepTime { set => sleepTime = value; get => sleepTime; }

        internal static APILibrary.Win32.AppInfo AppInfo => appinfo;
    }
}
