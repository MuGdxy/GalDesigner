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

    public class GuiComponentDragEvent : GuiComponentEvent
    {
        public bool Begin { get; }
        public bool End => !Begin;

        public GuiComponentDragEvent(DateTime time, string name, bool begin) : base(time, name)
        {
            Begin = begin;
        }

        public GuiComponentDragEvent(DateTime time, bool begin)
            : this(time, GuiComponentStatusProperty.Drag, begin)
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

    public class GuiComponentShowEvent : GuiComponentEvent
    {
        public bool Show { get; }
        public bool Hide => !Show;

        public GuiComponentShowEvent(DateTime time, string name, bool show) : base(time, name)
        {
            Show = show;
        }

        public GuiComponentShowEvent(DateTime time, bool show)
            : this(time, GuiComponentStatusProperty.Show, show)
        {

        }
    }

    /**
     * @brief Rule of Event
     * we only trigger the event whose status is true
     * for drag and focus event: there are most one control can get focus or get drag status
     * for drag and focus event: so the focus or drag control is the biggest dfs order control(may use z order)
     * for input event: we only trigger it on the control who enable it and get focus
     * for mouse event: we only trigger it when the control contain the mouse cursor
     * for mouse event: except the hover event, we trigger it when mouse enter and leave the control
     * for show event: it effect other events, such as mouse and drag event. it only can be run when the show status is true
     * for show event: and the show status of component is not effected by the show event status
     */
    public static class GuiComponentStatusProperty
    {
        public static string Drag => "Drag";
        public static string Move => "Move";
        public static string Show => "Show";
        public static string Click => "Click";
        public static string Wheel => "Wheel";
        public static string Hover => "Hover";
        public static string Input => "Input";
        public static string Focus => "Focus";

        public static string[] Event => new string[]
        {
            Drag, Show, Move, Click, Wheel, Hover, Input, Focus
        };

        public static string[] Component => new string[]
        {
            Drag, Show, Hover, Focus
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
