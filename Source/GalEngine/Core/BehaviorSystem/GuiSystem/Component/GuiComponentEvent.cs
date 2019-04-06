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
        public GuiControl DragControl;
        public GuiControl FocusControl;
    }

    public class GuiComponentEvent : BaseEvent
    {
        public string Name { get; }

        public GuiComponentEvent(DateTime time, string name) : base(time)
        {
            Name = name;
        }
    }

    public class GuiComponentMoveEvent : GuiComponentEvent
    {
        public Position<int> Position { get; }

        public GuiComponentMoveEvent(DateTime time, string name, Position<int> position) : base(time, name)
        {
            Position = position;
        }

        public GuiComponentMoveEvent(DateTime time, Position<int> position) : 
            this(time, GuiComponentStatusProperty.Move, position)
        {

        }
    }

    public class GuiComponentClickEvent : GuiComponentMoveEvent
    {
        public MouseButton Button { get; }
        public bool IsDown { get; }

        public GuiComponentClickEvent(DateTime time, string name, Position<int> position, MouseButton button, bool isDown) : base(time, name, position)
        {
            Button = button;
            IsDown = isDown;
        }

        public GuiComponentClickEvent(DateTime time, Position<int> position, MouseButton button, bool isDown) :
            this(time, GuiComponentStatusProperty.Click, position, button, isDown)
        {

        }
    }

    public class GuiComponentWheelEvent : GuiComponentMoveEvent
    {
        public int Offset { get; }

        public GuiComponentWheelEvent(DateTime time, string name, Position<int> position, int offset) : base(time, name, position)
        {
            Offset = offset;
        }

        public GuiComponentWheelEvent(DateTime time, Position<int> position, int offset) 
            : this(time, GuiComponentStatusProperty.Wheel, position, offset)
        {

        }
    }

    public class GuiComponentInputEvent : GuiComponentEvent
    {
        public KeyCode Input { get; }

        public GuiComponentInputEvent(DateTime time, string name, KeyCode input) : base(time, name)
        {
            Input = input;
        }

        public GuiComponentInputEvent(DateTime time, KeyCode input) : 
            this(time, GuiComponentStatusProperty.Input, input)
        {

        }
    }

    public class GuiComponentFocusEvent : GuiComponentEvent
    {
        public bool Focus { get; }

        public GuiComponentFocusEvent(DateTime time, string name, bool focus) : base(time, name)
        {
            Focus = focus;
        }

        public GuiComponentFocusEvent(DateTime time, bool focus)
            : this(time, GuiComponentStatusProperty.Focus, focus)
        {

        }
    }

    public class GuiComponentHoverEvent : GuiComponentEvent
    {
        public bool Hover { get; }
        public bool Enter => Hover;
        public bool Leave => !Enter;

        public GuiComponentHoverEvent(DateTime time, string name, bool hover) : base(time, name)
        {
            Hover = hover;
        }

        public GuiComponentHoverEvent(DateTime time, bool hover)
            : this(time, GuiComponentStatusProperty.Hover, hover)
        {

        }
    }

    public static class GuiComponentStatusProperty
    {
        public static string Drag => "Drag";
        public static string Show => "Show";
        public static string Hide => "Hide";
        public static string Move => "Move";
        public static string Click => "Click";
        public static string Wheel => "Wheel";
        public static string Hover => "Hover";
        public static string Input => "Input";
        public static string Focus => "Focus";

        public static string[] Event => new string[]
        {
            Drag, Show, Hide, Move, Click, Wheel, Hover, Input, Focus
        };

        public static string[] Component => new string[]
        {
            Drag, Show, Hide, Hover, Focus
        };
    }
    
    public class GuiComponentStatus : PropertyContainer<bool>
    {
        public GuiComponentStatus(string[] statusNames)
        {
            foreach (var statusName in statusNames)
            {
                SetProperty(statusName, false);
            }
        }
    }

    public delegate void GuiComponentEventSolver(GuiControl control, GuiComponentEvent eventArg);
}
