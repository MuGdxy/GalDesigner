using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class BaseEvent
    {

    }

    public class MouseMoveEvent : BaseEvent
    {
        public Position MousePosition;
    }

    public enum MouseButton
    {
        Left, Middle, Right
    }

    public class MouseClickEvent : MouseMoveEvent
    {
        public bool IsDown;
        public MouseButton Button;
    }

    public class MouseWheelEvent : MouseMoveEvent
    {
        public int Offset;
    }

    public class BoardClickEvent : BaseEvent
    {
        public KeyCode KeyCode;
        public bool IsDown;
    }

    public class SizeChangeEvent : BaseEvent
    {
        public int Width;
        public int Height;
    }

    public delegate void MouseMoveHandler(object sender, MouseMoveEvent eventArg);
    public delegate void MouseClickHandler(object sender, MouseClickEvent eventArg);
    public delegate void MouseWheelHandler(object sender, MouseWheelEvent eventArg);
    public delegate void BoardClickHandler(object sender, BoardClickEvent eventArg);
    public delegate void UpdateHandler(object sender);
    public delegate void MouseEnterEventHandler(object sender);
    public delegate void MouseLeaveEventHandler(object sender);
}
