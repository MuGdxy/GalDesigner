using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder
{
    /// <summary>
    /// Mouse Button
    /// </summary>
    public enum MouseButton
    {
        Null,
        LeftButton = 1,
        MiddleButton = 2,
        RightButton = 3
    }

    /// <summary>
    /// Mouse Move Event
    /// </summary>
    public class MouseMoveEventArgs : EventArgs
    {
        internal int x = 0;
        internal int y = 0;

        /// <summary>
        /// Mouse Position X
        /// </summary>
        public int X => x;
        
        /// <summary>
        /// Mouse Position Y
        /// </summary>
        public int Y => y;
    }

    /// <summary>
    /// Mouse Click Event
    /// </summary>
    public class MouseClickEventArgs : MouseMoveEventArgs
    {
        internal bool isdown = false;
        internal MouseButton which = MouseButton.Null;

        public bool IsDown { get => isdown; }

        /// <summary>
        /// Which Button Click
        /// </summary>
        public MouseButton Which { get => which; }
    }

    /// <summary>
    /// Mouse Wheel Event
    /// </summary>
    public class MouseWheelEventArgs : MouseMoveEventArgs
    {
        internal int offset = 0;

        /// <summary>
        /// Whell Offset
        /// </summary>
        public int Offset => offset;
    }

    public class KeyEventArgs : EventArgs
    {
        internal bool isdown = false;
        internal KeyCode keycode = 0;

        public bool IsDown { get => isdown; }

        /// <summary>
        /// Which Keycode 
        /// </summary>
        public KeyCode KeyCode { get => keycode; }
    }

    public class SizeChangeEventArgs : EventArgs
    {
        internal int old_width = 0;
        internal int old_height = 0;

        internal int new_width = 0;
        internal int new_height = 0;

        /// <summary>
        /// Old Width
        /// </summary>
        public int LastWidth => old_width;
        /// <summary>
        /// Old Height
        /// </summary>
        public int LastHeight => old_height;
        
        /// <summary>
        /// New Width
        /// </summary>
        public int NextWidth => new_width;
        /// <summary>
        /// New Height
        /// </summary>
        public int NextHeight => new_height;
    }

    public delegate void UpdateHandler(object sender);
    public delegate void MouseMoveHandler(object sender, MouseMoveEventArgs e);
    public delegate void MouseClickHandler(object sender, MouseClickEventArgs e);
    public delegate void MouseWheelHandler(object sender, MouseWheelEventArgs e);
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);
    public delegate void SizeChangeEventHandler(object sender, SizeChangeEventArgs e);
    public delegate void DestroyedHandler(object sender);
}
