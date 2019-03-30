using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
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
    }

    public class GuiComponentClickEvent : GuiComponentMoveEvent
    {
        public MouseButton Button { get; }

        public GuiComponentClickEvent(DateTime time, string name, Position<int> position, MouseButton button) : base(time, name, position)
        {
            Button = button;
        }
    }

    public class GuiComponentWheelEvent : GuiComponentMoveEvent
    {
        public int Offset { get; }

        public GuiComponentWheelEvent(DateTime time, string name, Position<int> position, int offset) : base(time, name, position)
        {
            Offset = offset;
        }
    }

    public class GuiComponentInputEvent : GuiComponentEvent
    {
        public char Input { get; }

        public GuiComponentInputEvent(DateTime time, string name, char input) : base(time, name)
        {
            Input = input;
        }
    }

    public class GuiComponentEventName
    {
        public string Drag => "Drag";
        public string Show => "Show";
        public string Hide => "Hide";
        public string Move => "Move";
        public string Click => "Click";
        public string Wheel => "Wheel";
        public string Hover => "Hover";
        public string Enter => "Enter";
        public string Leave => "Leave";
        public string Input => "Input";
        public string SetFocus => "SetFocus";
        public string LostFocus => "LostFocus";

        public string[] Array => new string[]
        {
            Drag, Show, Hide, Move, Click, Wheel, Hover, Enter , Leave, Input, SetFocus, LostFocus
        };

        internal GuiComponentEventName() { }
    }

    public static partial class StringProperty
    {
        public static GuiComponentEventName GuiComponentEvent { get; }
    }

    public delegate void GuiComponentEventSolver(GuiControl control, GuiComponentEvent eventArg);
}
