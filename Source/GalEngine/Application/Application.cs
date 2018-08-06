using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class Application
    {
        private static StaticObjectProperties objectProperties = new StaticObjectProperties();

        private static bool isWindowCreated = false;

        private static IntPtr handle = IntPtr.Zero;

        private static int mousePositionX = 0;
        private static int mousePositionY = 0;

        private static bool isFocused = false;

        private static readonly uint style = (uint)(APILibrary.Win32.WindowStyles.WS_OVERLAPPEDWINDOW ^ APILibrary.Win32.WindowStyles.WS_SIZEBOX ^
                APILibrary.Win32.WindowStyles.WS_MAXIMIZEBOX);

        private static readonly APILibrary.Win32.Internal.WndProc processFunction = ProcessMessage;

        private static void ChangeNameProperty()
        {
            APILibrary.Win32.Internal.SetWindowText(handle, Name);
        }

        private static void ChangeSizeProperty()
        {
            var rect = new APILibrary.Win32.Rect()
            {
                left = 0,
                top = 0,
                right = Width,
                bottom = Height
            };

            APILibrary.Win32.Internal.AdjustWindowRect(ref rect, style, false);

            APILibrary.Win32.Internal.SetWindowPos(handle, IntPtr.Zero, 0, 0, rect.right - rect.left, rect.bottom - rect.top,
                (uint)(APILibrary.Win32.SetWindowPosFlags.SWP_NOZORDER ^ APILibrary.Win32.SetWindowPosFlags.SWP_NOMOVE));
        }

        private static void ChangeVisableProperty()
        {
            if (Visable is true) APILibrary.Win32.Internal.ShowWindow(handle, (int)APILibrary.Win32.ShowWindowStyles.SW_SHOW);
            if (Visable is false) APILibrary.Win32.Internal.ShowWindow(handle, (int)APILibrary.Win32.ShowWindowStyles.SW_HIDE);
        }

        private static void MakeProperties()
        {
            objectProperties.AddProperty(Property.Name, "GalEngine");
            objectProperties.AddProperty(Property.Width, 0);
            objectProperties.AddProperty(Property.Height, 0);
            objectProperties.AddProperty(Property.Icon, "");
            objectProperties.AddProperty(Property.Visable, false);
        }

        private static IntPtr ProcessMessage(IntPtr Hwnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            var type = (APILibrary.Win32.WinMsg)message;

            switch (type)
            {
                case APILibrary.Win32.WinMsg.WM_MOUSEMOVE:
                    OnMouseMove(null, new MouseMoveEvent()
                    {
                        X = APILibrary.Win32.Message.LowWord(lParam),
                        Y = APILibrary.Win32.Message.HighWord(lParam)
                    }); break;

                case APILibrary.Win32.WinMsg.WM_LBUTTONDOWN:
                    OnMouseClick(null, new MouseClickEvent()
                    {
                        X = APILibrary.Win32.Message.LowWord(lParam),
                        Y = APILibrary.Win32.Message.HighWord(lParam),
                        Button = MouseButton.Left,
                        IsDown = true
                    }); break;

                case APILibrary.Win32.WinMsg.WM_LBUTTONUP:
                    OnMouseClick(null, new MouseClickEvent()
                    {
                        X = APILibrary.Win32.Message.LowWord(lParam),
                        Y = APILibrary.Win32.Message.HighWord(lParam),
                        Button = MouseButton.Left,
                        IsDown = false
                    }); break;

                case APILibrary.Win32.WinMsg.WM_MBUTTONDOWN:
                    OnMouseClick(null, new MouseClickEvent()
                    {
                        X = APILibrary.Win32.Message.LowWord(lParam),
                        Y = APILibrary.Win32.Message.HighWord(lParam),
                        Button = MouseButton.Middle,
                        IsDown = true
                    }); break;

                case APILibrary.Win32.WinMsg.WM_MBUTTONUP:
                    OnMouseClick(null, new MouseClickEvent()
                    {
                        X = APILibrary.Win32.Message.LowWord(lParam),
                        Y = APILibrary.Win32.Message.HighWord(lParam),
                        Button = MouseButton.Middle,
                        IsDown = false
                    }); break;

                case APILibrary.Win32.WinMsg.WM_RBUTTONDOWN:
                    OnMouseClick(null, new MouseClickEvent()
                    {
                        X = APILibrary.Win32.Message.LowWord(lParam),
                        Y = APILibrary.Win32.Message.HighWord(lParam),
                        Button = MouseButton.Right,
                        IsDown = true
                    }); break;

                case APILibrary.Win32.WinMsg.WM_RBUTTONUP:
                    OnMouseClick(null, new MouseClickEvent()
                    {
                        X = APILibrary.Win32.Message.LowWord(lParam),
                        Y = APILibrary.Win32.Message.HighWord(lParam),
                        Button = MouseButton.Right,
                        IsDown = false
                    }); break;

                case APILibrary.Win32.WinMsg.WM_MOUSEWHEEL:
                    OnMouseWheel(null, new MouseWheelEvent()
                    {
                        Offset = (short)APILibrary.Win32.Message.HighWord(wParam),
                        X = APILibrary.Win32.Message.GetXFromLparam(lParam),
                        Y = APILibrary.Win32.Message.GetYFromLparam(lParam)
                    }); break;

                case APILibrary.Win32.WinMsg.WM_KEYDOWN:
                    OnBoardClick(null, new BoardClickEvent()
                    {
                        KeyCode = (KeyCode)wParam,
                        IsDown = true
                    }); break;

                case APILibrary.Win32.WinMsg.WM_KEYUP:
                    OnBoardClick(null, new BoardClickEvent()
                    {
                        KeyCode = (KeyCode)wParam,
                        IsDown = false
                    }); break;

                case APILibrary.Win32.WinMsg.WM_SETFOCUS:
                    isFocused = true;
                    break;

                case APILibrary.Win32.WinMsg.WM_KILLFOCUS:
                    isFocused = false;
                    break;

                case APILibrary.Win32.WinMsg.WM_DESTROY:
                    APILibrary.Win32.Internal.PostQuitMessage(0);
                    break;

                default:
                    break;
            }

            return APILibrary.Win32.Internal.DefWindowProc(Hwnd, message, wParam, lParam);
        }

        private static void OnChangeProperty()
        {
            if (objectProperties.IsChanged(Property.Name) is true) ChangeNameProperty();
            if (objectProperties.IsChanged(Property.Width) is true || objectProperties.IsChanged(Property.Height) is true) ChangeSizeProperty();
            if (objectProperties.IsChanged(Property.Visable) is true) ChangeVisableProperty();


            objectProperties.FinishUpdate();
        }

        private static void OnMouseMove(object sender, MouseMoveEvent eventArg)
        {
            mousePositionX = eventArg.X;
            mousePositionY = eventArg.Y;

            MouseMove?.Invoke(sender, eventArg);
        }

        private static void OnMouseClick(object sender, MouseClickEvent eventArg)
        {
            eventArg.X = mousePositionX;
            eventArg.Y = mousePositionY;

            MouseClick?.Invoke(sender, eventArg);
        }

        private static void OnMouseWheel(object sender, MouseWheelEvent eventArg)
        {
            eventArg.X = mousePositionX;
            eventArg.Y = mousePositionY;

            MouseWheel?.Invoke(sender, eventArg);
        }

        private static void OnBoardClick(object sender, BoardClickEvent eventArg)
        {
            BoardClick?.Invoke(sender, eventArg);
        }

        private static void OnUpdate(object sender)
        {
            OnChangeProperty();

            Update?.Invoke(sender);
        }

        static Application()
        {
            MakeProperties();

            GC.KeepAlive(processFunction);
        }

        public static event MouseMoveHandler MouseMove;
        public static event MouseClickHandler MouseClick;
        public static event MouseWheelHandler MouseWheel;
        public static event BoardClickHandler BoardClick;
        public static event UpdateHandler Update;

        public static string Name
        {
            set => objectProperties.SetProperty(Property.Name, value);
            get => objectProperties.GetProperty<string>(Property.Name);
        }

        public static string Icon
        {
            private set => objectProperties.SetProperty(Property.Icon, value);
            get => objectProperties.GetProperty<string>(Property.Icon);
        }

        public static int Width
        {
            set => objectProperties.SetProperty(Property.Width, value);
            get => objectProperties.GetProperty<int>(Property.Width);
        }

        public static int Height
        {
            set => objectProperties.SetProperty(Property.Height, value);
            get => objectProperties.GetProperty<int>(Property.Height);
        }

        public static bool Visable
        {
            set => objectProperties.SetProperty(Property.Visable, value);
            get => objectProperties.GetProperty<bool>(Property.Visable);
        }

        public static bool IsFocused => isFocused;

        public static void MakeWindow(string name, int width, int height, string iconFilePath = "")
        {
            if (isWindowCreated is true)
            {
                DebugLayer.ReportWarning(Warning.TheWindowIsCreated);

                return;
            }

            Name = name;
            Width = width;
            Height = height;
            Icon = iconFilePath;

            var hInstance = APILibrary.Win32.Internal.GetModuleHandle(null);

            var appInfo = new APILibrary.Win32.AppInfo
            {
                style = (uint)APILibrary.Win32.AppInfoStyle.CS_DBLCLKS,
                lpfnWndProc = processFunction,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = hInstance,
                hIcon = APILibrary.Win32.Internal.LoadImage(IntPtr.Zero, Icon, 1, 0, 0, 0x00000010),
                hCursor = APILibrary.Win32.Internal.LoadCursor(IntPtr.Zero, (uint)APILibrary.Win32.CursorType.IDC_ARROW),
                hbrBackground = APILibrary.Win32.Internal.GetStockObject(0),
                lpszMenuName = null,
                lpszClassName = Name
            };

            APILibrary.Win32.Internal.RegisterAppinfo(ref appInfo);

            var rect = new APILibrary.Win32.Rect()
            {
                left = 0,
                top = 0,
                right = Width,
                bottom = Height
            };

            APILibrary.Win32.Internal.AdjustWindowRect(ref rect, style, false);

            handle = APILibrary.Win32.Internal.CreateWindowEx(0, Name, Name, style,
                0x80000000, 0x80000000, rect.right - rect.left, rect.bottom - rect.top, IntPtr.Zero, IntPtr.Zero,
                hInstance, IntPtr.Zero);

            isWindowCreated = true;
        }

        public static void RunLoop()
        {
            while (isWindowCreated is true)
            {
                var message = new APILibrary.Win32.Message();
                message.hwnd = handle;

                while (APILibrary.Win32.Internal.PeekMessage(out message, IntPtr.Zero, 0, 0,
                    (int)APILibrary.Win32.PeekMessageFlags.PM_REMOVE))
                {
                    APILibrary.Win32.Internal.TranslateMessage(ref message);
                    APILibrary.Win32.Internal.DispatchMessage(ref message);

                    if (message.type == (uint)APILibrary.Win32.WinMsg.WM_QUIT)
                        isWindowCreated = false;
                }


                OnUpdate(null);
            }
        }

    }
}
