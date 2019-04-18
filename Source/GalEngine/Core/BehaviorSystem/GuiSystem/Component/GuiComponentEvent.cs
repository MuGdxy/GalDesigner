using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    struct GuiComponentEventProperty
    {
        public Position<int> MousePosition;
        public GuiControl FocusControl;
    }

    public class GuiComponentEvent : BaseEvent
    {
        public GuiComponentEvent(DateTime time) : base(time)
        {
        }
    }

    public class GuiComponentMouseMoveEvent : GuiComponentEvent
    {
        public Position<int> Position { get; }

        public GuiComponentMouseMoveEvent(DateTime time, Position<int> position) : base(time)
        {
            Position = position;
        }
    }

    public class GuiComponentMouseClickEvent : GuiComponentMouseMoveEvent
    {
        public MouseButton Button { get; }
        public bool IsDown { get; }

        public GuiComponentMouseClickEvent(DateTime time, Position<int> position, MouseButton button, bool isDown) : base(time, position)
        {
            Button = button;
            IsDown = isDown;
        }
    }

    public class GuiComponentMouseWheelEvent : GuiComponentMouseMoveEvent
    {
        public int Offset { get; }

        public GuiComponentMouseWheelEvent(DateTime time, Position<int> position, int offset) : base(time, position)
        {
            Offset = offset;
        }

    }

    public class GuiComponentInputEvent : GuiComponentEvent
    {
        public KeyCode Input { get; }

        public GuiComponentInputEvent(DateTime time, KeyCode input) : base(time)
        {
            Input = input;
        }

    }

    public class GuiComponentFocusEvent : GuiComponentEvent
    {
        public bool Focus { get; }

        public GuiComponentFocusEvent(DateTime time, bool focus) : base(time)
        {
            Focus = focus;
        }
    }

    public class GuiComponentHoverEvent : GuiComponentEvent
    {
        public bool Hover { get; }
        public bool Enter => Hover;
        public bool Leave => !Enter;

        public GuiComponentHoverEvent(DateTime time, bool hover) : base(time)
        {
            Hover = hover;
        }
    }

    /**
     * @brief Rule of Event
     * for input event: we only trigger it on the control who get focus
     * for mouse event: we only trigger it when the control contain the mouse cursor
     * for mouse event: except the hover event, we trigger it when mouse enter and leave the control
     */
    public static class GuiComponentSupportEvent
    {
        public static string MouseMove => "MouseMove";
        public static string MouseClick => "MouseClick";
        public static string MouseWheel => "MouseWheel";
        public static string Hover => "Hover";
        public static string Input => "Input";
        public static string Focus => "Focus";
    }
}
