using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder
{
    public abstract partial class GenericWindow
    {
        
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (value is true) Show();
                else Hide();
            }
        }

        public bool IsMinimized => isMinimized;

        public bool IsMaximized => IsMaximized;

        public bool IsFocus => isFocus;

        public bool IsEnable
        {
            get => isEnable;
            set => Enable(value);
        }

        public bool IsVailed => isVailed;

        public float Opacity
        {
            get => opacity;
            set => SetOpacity(value);
        }

        public string Title
        {
            get => tag;
            set => SetTitle(value);
        }
        public int PositionX
        {
            get => positionx;
            set => MoveTo(value, positiony);
        }

        public int PositionY
        {
            get => positiony;
            set => MoveTo(positionx, value);
        }

        public int Width => width;

        public int Height => height;

        public IntPtr Handle => handle;

        public event UpdateHandler Update;
        public event MouseMoveHandler MouseMove;
        public event MouseClickHandler MouseClick;
        public event MouseWheelHandler MouseWheel;
        public event KeyEventHandler KeyEvent;
        public event SizeChangeEventHandler SizeChange;
        public event DestroyedHandler Destroyed;

        public GenericWindow(string Title, int Width, int Height)
        {
            processEvent += Process;

            tag = Title;
            width = Width;
            height = Height;

            APILibrary.Win32.Rect realRect = new APILibrary.Win32.Rect()
            {
                left = 0,
                top = 0,
                right = width,
                bottom = height
            };

            appinfo = new APILibrary.Win32.AppInfo()
            {
                style = Application.AppInfo.style,
                cbClsExtra = Application.AppInfo.cbClsExtra,
                cbWndExtra = Application.AppInfo.cbWndExtra,
                hbrBackground = Application.AppInfo.hbrBackground,
                hCursor = Application.AppInfo.hCursor,
                hIcon = Application.AppInfo.hIcon,
                hInstance = Application.AppInfo.hInstance,
                lpszMenuName = Application.AppInfo.lpszMenuName,
                lpfnWndProc = processEvent,
                lpszClassName = tag
            };

            APILibrary.Win32.Internal.RegisterAppinfo(ref appinfo);

            APILibrary.Win32.Internal.AdjustWindowRect(ref realRect, (uint)windowstyle, false);

            handle = APILibrary.Win32.Internal.CreateWindowEx((uint)exstyle, tag, tag,
                (uint)windowstyle, 0x80000000, 0x80000000, realRect.right - realRect.left,
                realRect.bottom - realRect.top, IntPtr.Zero, IntPtr.Zero,
                Application.AppInfo.hInstance, IntPtr.Zero);

            APILibrary.Win32.Internal.SetLayeredWindowAttributes(handle, 0
               , 255, (uint)APILibrary.Win32.UpdateLayeredWindowsFlags.ULW_ALPHA);

            APILibrary.Win32.Internal.GetWindowRect(handle, ref realRect);

            positionx = realRect.left;
            positiony = realRect.top;
        }
        public void Destory()
        {
            APILibrary.Win32.Internal.DestroyWindow(handle);
        }

        public void Enable(bool enable)
        {
            isEnable = enable;
        }

        public void Hide()
        {
            if (isVisible is false) return;

            isVisible = false;
            isMaximized = false;
            isMinimized = false;

            APILibrary.Win32.Internal.ShowWindow(handle,
                (int)APILibrary.Win32.ShowWindowStyles.SW_HIDE);
        }

        public void Maximize()
        {
            if (isMaximized is true) return;

            isVisible = true;
            isMaximized = true;
            isMinimized = false;

            APILibrary.Win32.Internal.ShowWindow(handle,
                (int)APILibrary.Win32.ShowWindowStyles.SW_MAXIMIZE);
        }

        public void Minimize()
        {
            if (isMinimized is true) return;

            isVisible = true;
            isMaximized = false;
            isMinimized = true;

            APILibrary.Win32.Internal.ShowWindow(handle,
                (int)APILibrary.Win32.ShowWindowStyles.SW_MINIMIZE);
        }

        public void MoveTo(int x, int y)
        {
            positionx = x;
            positiony = y;
            APILibrary.Win32.Internal.MoveWindow(handle, positionx, positiony, width, height,
                false);
        }

        public virtual void OnKeyEvent(object sender, KeyEventArgs e) { }
        public virtual void OnMouseClick(object sender, MouseClickEventArgs e) { }
        public virtual void OnMouseMove(object sender, MouseMoveEventArgs e) { }
        public virtual void OnMouseWheel(object sender, MouseWheelEventArgs e) { }
        public virtual void OnSizeChange(object sender, SizeChangeEventArgs e) { }
        public virtual void OnUpdate(object sender) { }
        public virtual void OnDestroyed(object sender) { }

        public void SetCapture()
        {
            APILibrary.Win32.Internal.SetCapture(handle);
        }

        public void SetFocus()
        {
            APILibrary.Win32.Internal.SetFocus(handle);

            isFocus = true;
        }

        public void SetOpacity(float Opacity)
        {
            opacity = Opacity;

            APILibrary.Win32.Internal.SetLayeredWindowAttributes(handle, 0
              , (byte)(255 * opacity), (uint)APILibrary.Win32.UpdateLayeredWindowsFlags.ULW_ALPHA);
        }

        public void SetTitle(string Title)
        {
            tag = Title;

            APILibrary.Win32.Internal.SetWindowText(handle, tag);
        }

        public void Show()
        {
            if (isVisible is true) return;

            isVisible = true;

            APILibrary.Win32.Internal.ShowWindow(handle, (int)
                APILibrary.Win32.ShowWindowStyles.SW_SHOW);

        }
    }
}
