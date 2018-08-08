using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Systems
{
    using Message = APILibrary.Win32.Message;

    static class Windows
    {
        private static readonly APILibrary.Win32.Internal.WndProc processFunction = ProcessMessage;

        private static readonly uint style = (uint)(APILibrary.Win32.WindowStyles.WS_OVERLAPPEDWINDOW 
            ^ APILibrary.Win32.WindowStyles.WS_SIZEBOX ^ APILibrary.Win32.WindowStyles.WS_MAXIMIZEBOX);

        private static IntPtr handle = IntPtr.Zero;

        private static SharpDX.DXGI.SwapChain swapChain = null;
        private static SharpDX.Direct2D1.Bitmap1 renderTarget = null;

        private static IntPtr ProcessMessage(IntPtr Hwnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            var type = (APILibrary.Win32.WinMsg)message;

            switch (type)
            {
                case APILibrary.Win32.WinMsg.WM_MOUSEMOVE:
                    Application.OnMouseMove(null, new MouseMoveEvent()
                    {
                       MousePosition = new Position(
                           Message.LowWord(lParam), 
                           Message.HighWord(lParam))
                    }); break;

                case APILibrary.Win32.WinMsg.WM_LBUTTONDOWN:
                    Application.OnMouseClick(null, new MouseClickEvent()
                    {
                        MousePosition = new Position(
                           Message.LowWord(lParam),
                           Message.HighWord(lParam)),
                        Button = MouseButton.Left,
                        IsDown = true
                    }); break;

                case APILibrary.Win32.WinMsg.WM_LBUTTONUP:
                    Application.OnMouseClick(null, new MouseClickEvent()
                    {
                        MousePosition = new Position(
                           Message.LowWord(lParam),
                           Message.HighWord(lParam)),
                        Button = MouseButton.Left,
                        IsDown = false
                    }); break;

                case APILibrary.Win32.WinMsg.WM_MBUTTONDOWN:
                    Application.OnMouseClick(null, new MouseClickEvent()
                    {
                        MousePosition = new Position(
                           Message.LowWord(lParam),
                           Message.HighWord(lParam)),
                        Button = MouseButton.Middle,
                        IsDown = true
                    }); break;

                case APILibrary.Win32.WinMsg.WM_MBUTTONUP:
                    Application.OnMouseClick(null, new MouseClickEvent()
                    {
                        MousePosition = new Position(
                           Message.LowWord(lParam),
                           Message.HighWord(lParam)),
                        Button = MouseButton.Middle,
                        IsDown = false
                    }); break;

                case APILibrary.Win32.WinMsg.WM_RBUTTONDOWN:
                    Application.OnMouseClick(null, new MouseClickEvent()
                    {
                        MousePosition = new Position(
                           Message.LowWord(lParam),
                           Message.HighWord(lParam)),
                        Button = MouseButton.Right,
                        IsDown = true
                    }); break;

                case APILibrary.Win32.WinMsg.WM_RBUTTONUP:
                    Application.OnMouseClick(null, new MouseClickEvent()
                    {
                        MousePosition = new Position(
                           Message.LowWord(lParam),
                           Message.HighWord(lParam)),
                        Button = MouseButton.Right,
                        IsDown = false
                    }); break;

                case APILibrary.Win32.WinMsg.WM_MOUSEWHEEL:
                    Application.OnMouseWheel(null, new MouseWheelEvent()
                    {
                        MousePosition = new Position(
                           Message.LowWord(lParam),
                           Message.HighWord(lParam)),
                        Offset = (short)Message.HighWord(wParam)
                    }); break;

                case APILibrary.Win32.WinMsg.WM_KEYDOWN:
                    Application.OnBoardClick(null, new BoardClickEvent()
                    {
                        KeyCode = (KeyCode)wParam,
                        IsDown = true
                    }); break;

                case APILibrary.Win32.WinMsg.WM_KEYUP:
                    Application.OnBoardClick(null, new BoardClickEvent()
                    {
                        KeyCode = (KeyCode)wParam,
                        IsDown = false
                    }); break;

                case APILibrary.Win32.WinMsg.WM_SIZE:
                    Application.Size = new Size(Message.LowWord(lParam), Message.HighWord(lParam));
                    break;

                case APILibrary.Win32.WinMsg.WM_DESTROY:
                    APILibrary.Win32.Internal.PostQuitMessage(0);
                    break;

                default:
                    break;
            }

            return APILibrary.Win32.Internal.DefWindowProc(Hwnd, message, wParam, lParam);
        }

        public static void CreateWindow(string Name, Size Size, string Icon)
        {
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
                right = Size.Width,
                bottom = Size.Height
            };

            APILibrary.Win32.Internal.AdjustWindowRect(ref rect, style, false);

            handle = APILibrary.Win32.Internal.CreateWindowEx(0, Name, Name, style,
                0x80000000, 0x80000000, rect.right - rect.left, rect.bottom - rect.top, IntPtr.Zero, IntPtr.Zero,
                hInstance, IntPtr.Zero);

            var desc = new SharpDX.DXGI.SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new SharpDX.DXGI.ModeDescription()
                {
                    Width = Size.Width,
                    Height = Size.Height,
                    Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                    RefreshRate = new SharpDX.DXGI.Rational(60, 1),
                    Scaling = SharpDX.DXGI.DisplayModeScaling.Unspecified,
                    ScanlineOrdering = SharpDX.DXGI.DisplayModeScanlineOrder.Unspecified
                },
                Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                Flags = SharpDX.DXGI.SwapChainFlags.None,
                OutputHandle = handle,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
                IsWindowed = true
            };

            using (var dxgiDevice = Graphics.device3D.QueryInterface<SharpDX.DXGI.Device>())
            {
                using (var dxgiAdapter = dxgiDevice.GetParent<SharpDX.DXGI.Adapter>())
                {
                    using (var dxgiFactory = dxgiAdapter.GetParent<SharpDX.DXGI.Factory>())
                    {
                        swapChain = new SharpDX.DXGI.SwapChain(dxgiFactory, Graphics.device3D, desc);
                    }
                }
            }

            using (var backBuffer = swapChain.GetBackBuffer<SharpDX.DXGI.Surface>(0))
            {
                renderTarget = new SharpDX.Direct2D1.Bitmap1(Graphics.deviceContext2D, backBuffer);
            }

            APILibrary.Win32.Internal.ShowWindow(handle, (int)APILibrary.Win32.ShowWindowStyles.SW_SHOW);
        }

        public static void SetWindowText(string Name)
        {
            APILibrary.Win32.Internal.SetWindowText(handle, Name);
        }

        public static void SetWindowSize(Size size)
        {
            var rect = new APILibrary.Win32.Rect()
            {
                left = 0,
                top = 0,
                right = size.Width,
                bottom = size.Height
            };

            APILibrary.Win32.Internal.AdjustWindowRect(ref rect, style, false);

            APILibrary.Win32.Internal.SetWindowPos(handle, IntPtr.Zero, 0, 0, rect.right - rect.left, rect.bottom - rect.top,
                (uint)(APILibrary.Win32.SetWindowPosFlags.SWP_NOZORDER ^ APILibrary.Win32.SetWindowPosFlags.SWP_NOMOVE));

            SharpDX.Utilities.Dispose(ref renderTarget);
            
            swapChain.ResizeBuffers(0, size.Width, size.Height, SharpDX.DXGI.Format.Unknown, SharpDX.DXGI.SwapChainFlags.None);
            
            using (var backBuffer = swapChain.GetBackBuffer<SharpDX.DXGI.Surface>(0))
            {
                renderTarget = new SharpDX.Direct2D1.Bitmap1(Graphics.deviceContext2D, backBuffer);
            }
        }

        public static void PresentBitmap(bool isLock = true)
        {
            Graphics.deviceContext2D.Target = renderTarget;
            Graphics.deviceContext2D.BeginDraw();
            Graphics.deviceContext2D.Clear(new SharpDX.Mathematics.Interop.RawColor4(0, 0, 0, 0));

            var viewPort = Utility.ComputeViewPort(Application.Size, GameScene.renderTarget.Size);
            
            Graphics.deviceContext2D.DrawBitmap(GameScene.renderTarget.resource as SharpDX.Direct2D1.Bitmap1,
                new SharpDX.Mathematics.Interop.RawRectangleF(viewPort.Left, viewPort.Top, viewPort.Right, viewPort.Bottom),
                 1.0f, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);

            Graphics.deviceContext2D.EndDraw();
            Graphics.deviceContext2D.Target = null;
            
            swapChain.Present(isLock ? 1 : 0, SharpDX.DXGI.PresentFlags.None);
        }

        public static bool UpdateWindow()
        {
            var message = new APILibrary.Win32.Message();
            var result = false;

            message.hwnd = handle;

            while (APILibrary.Win32.Internal.PeekMessage(out message, IntPtr.Zero, 0, 0,
                (int)APILibrary.Win32.PeekMessageFlags.PM_REMOVE))
            {
                APILibrary.Win32.Internal.TranslateMessage(ref message);
                APILibrary.Win32.Internal.DispatchMessage(ref message);

                if (message.type == (uint)APILibrary.Win32.WinMsg.WM_QUIT)
                    result = true;
            }

            return result;
        }
    }
}
