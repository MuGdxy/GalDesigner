using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder
{
    public partial class GenericWindow
    {
        private bool isVisible = false;
        private bool isMinimized = false;
        private bool isMaximized = false;
        private bool isFocus = false;
        private bool isEnable = true;
        private bool isVailed = true;

        private float opacity = 1;
        private string tag = null;
        
        private int width = 0;
        private int height = 0;
        private int positionx = 0;
        private int positiony = 0;

        private APILibrary.Win32.Internal.WndProc processEvent;
        private APILibrary.Win32.AppInfo appinfo;

        private IntPtr handle = IntPtr.Zero;

        internal void PrivateOnUpdate(object sender)
        {
            OnUpdate(sender);

            Update?.Invoke(sender);
        }

        internal void PrivateMouseMove(object sender, MouseMoveEventArgs e)
        {
            OnMouseMove(sender, e);
            
            MouseMove?.Invoke(sender, e);
        }

        internal void PrivateMouseClick(object sender, MouseClickEventArgs e)
        {
            OnMouseClick(sender, e);

            MouseClick?.Invoke(sender, e);
        }

        internal void PrivateMouseWheel(object sender, MouseWheelEventArgs e)
        {
            OnMouseWheel(sender, e);

            MouseWheel?.Invoke(sender, e);
        }

        internal void PrivateKeyEvent(object sender, KeyEventArgs e)
        {
            OnKeyEvent(sender, e);

            KeyEvent?.Invoke(sender, e);
        }

        internal void PrivateSizeChangeEvent(object sender,SizeChangeEventArgs e)
        {
            OnSizeChange(sender, e);

            SizeChange?.Invoke(sender, e);
        }

        internal void PrivateDestory(object sender)
        {
            OnDestroyed(sender);

            Destroyed?.Invoke(sender);
        }

        private IntPtr Process(IntPtr Hwnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            return ProcessMessage(Hwnd, message, wParam, lParam);
        }

        private IntPtr ProcessMessage(IntPtr Hwnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            APILibrary.Win32.WinMsg type = (APILibrary.Win32.WinMsg)message;
            
            switch (type)
            {
                case APILibrary.Win32.WinMsg.WM_MOUSEMOVE:
                    PrivateMouseMove(this, new MouseMoveEventArgs()
                    {
                        x = APILibrary.Win32.Message.LowWord(lParam),
                        y = APILibrary.Win32.Message.HighWord(lParam)
                    });
                    break;
                case APILibrary.Win32.WinMsg.WM_LBUTTONDOWN:
                    PrivateMouseClick(this, new MouseClickEventArgs()
                    {
                        isdown = true,
                        which = MouseButton.LeftButton,
                        x = APILibrary.Win32.Message.LowWord(lParam),
                        y = APILibrary.Win32.Message.HighWord(lParam)
                    });
                    break;
                case APILibrary.Win32.WinMsg.WM_LBUTTONUP:
                    PrivateMouseClick(this, new MouseClickEventArgs()
                    {
                        isdown = false,
                        which = MouseButton.LeftButton,
                        x = APILibrary.Win32.Message.LowWord(lParam),
                        y = APILibrary.Win32.Message.HighWord(lParam)
                    });
                    break;
                case APILibrary.Win32.WinMsg.WM_MBUTTONDOWN:
                    PrivateMouseClick(this, new MouseClickEventArgs()
                    {
                        isdown = true,
                        which = MouseButton.MiddleButton,
                        x = APILibrary.Win32.Message.LowWord(lParam),
                        y = APILibrary.Win32.Message.HighWord(lParam)
                    });
                    break;
                case APILibrary.Win32.WinMsg.WM_MBUTTONUP:
                    PrivateMouseClick(this, new MouseClickEventArgs()
                    {
                        isdown = false,
                        which = MouseButton.MiddleButton,
                        x = APILibrary.Win32.Message.LowWord(lParam),
                        y = APILibrary.Win32.Message.HighWord(lParam)
                    });
                    break;
                case APILibrary.Win32.WinMsg.WM_RBUTTONDOWN:
                    PrivateMouseClick(this, new MouseClickEventArgs()
                    {
                        isdown = true,
                        which = MouseButton.RightButton,
                        x = APILibrary.Win32.Message.LowWord(lParam),
                        y = APILibrary.Win32.Message.HighWord(lParam)
                    });
                    break;
                case APILibrary.Win32.WinMsg.WM_RBUTTONUP:
                    PrivateMouseClick(this, new MouseClickEventArgs()
                    {
                        isdown = false,
                        which = MouseButton.RightButton,
                        x = APILibrary.Win32.Message.LowWord(lParam),
                        y = APILibrary.Win32.Message.HighWord(lParam)
                    });
                    break;
                case APILibrary.Win32.WinMsg.WM_KEYDOWN:
                    PrivateKeyEvent(this, new KeyEventArgs() { isdown = true, keycode = (KeyCode)wParam });

                    if ((KeyCode)wParam is KeyCode.CapsLock)
                        Application.IsCapsLock ^= true;
                    break;
                case APILibrary.Win32.WinMsg.WM_SYSKEYDOWN:
                    break;
                case APILibrary.Win32.WinMsg.WM_KEYUP:
                    PrivateKeyEvent(this, new KeyEventArgs() { isdown = false, keycode = (KeyCode)wParam });
                    break;
                case APILibrary.Win32.WinMsg.WM_DESTROY:
                    PrivateDestory(this);
                    isVailed = false;
                    APILibrary.Win32.Internal.UnRegisterAppinfo(appinfo.lpszClassName, appinfo.hInstance);
                    break;
                case APILibrary.Win32.WinMsg.WM_KILLFOCUS:
                    isFocus = false;
                    break;
                case APILibrary.Win32.WinMsg.WM_SETFOCUS:
                    isFocus = true;
                    break;
                case APILibrary.Win32.WinMsg.WM_MOVE:
                    positionx = APILibrary.Win32.Message.LowWord(lParam);
                    positiony = APILibrary.Win32.Message.HighWord(lParam);
                    break;
                case APILibrary.Win32.WinMsg.WM_MOUSEWHEEL:
                    int window_offset = (short)APILibrary.Win32.Message.HighWord(wParam);

                    PrivateMouseWheel(this, new MouseWheelEventArgs()
                    {
                        offset = window_offset,
                        x = APILibrary.Win32.Message.GetXFromLparam(lParam) - positionx,
                        y = APILibrary.Win32.Message.GetYFromLparam(lParam) - positiony
                    });
                    break;
                case APILibrary.Win32.WinMsg.WM_SIZE:
                    PrivateSizeChangeEvent(this, new SizeChangeEventArgs()
                    {
                        old_width = width,
                        old_height = height,
                        new_width = width = APILibrary.Win32.Message.LowWord(lParam),
                        new_height = height = APILibrary.Win32.Message.HighWord(lParam)
                    });
                    break;
                default:
                    return APILibrary.Win32.Internal.DefWindowProc(Hwnd, message, wParam, lParam);
            }

            return IntPtr.Zero;
        }

    }
}
