using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public delegate void GuiComponentEventSolver(GuiControl control, GuiComponentEvent eventArg);

    public class GuiComponentEventPart
    {
        public event GuiComponentEventSolver Solver;

        protected virtual void OnSolveEvent(GuiControl control, GuiComponentEvent eventArg) { }

        internal virtual void Invoke(GuiControl control, GuiComponentEvent eventArg)
        {
            //first, we invoke virtual function
            OnSolveEvent(control, eventArg);
            //second, we invoke solver
            Solver?.Invoke(control, eventArg);
        }

        public GuiComponentEventPart()
        {

        }

        public GuiComponentEventPart(GuiComponentEventSolver solver)
        {
            Solver += solver;
        }
    }

    public class GuiComponentFocusEventPart : GuiComponentEventPart
    {
        private bool mFocus;

        public bool Focus => mFocus;

        internal override void Invoke(GuiControl control, GuiComponentEvent eventArg)
        {
            mFocus = (eventArg as GuiComponentFocusEvent).Focus;

            base.Invoke(control, eventArg);
        }

        public GuiComponentFocusEventPart()
        {

        }

        public GuiComponentFocusEventPart(GuiComponentEventSolver solver) : base(solver)
        {

        }
    }

    public class GuiComponentHoverEventPart : GuiComponentEventPart
    {
        private bool mHover;

        public bool Hover => mHover;

        internal override void Invoke(GuiControl control, GuiComponentEvent eventArg)
        {
            mHover = (eventArg as GuiComponentHoverEvent).Hover;

            base.Invoke(control, eventArg);
        }

        public GuiComponentHoverEventPart()
        {

        }

        public GuiComponentHoverEventPart(GuiComponentEventSolver solver) : base(solver)
        {
        }
    }

    public class GuiComponentDragEventPart : GuiComponentEventPart
    {
        private bool mDrag;

        public bool Drag => mDrag;

        internal override void Invoke(GuiControl control, GuiComponentEvent eventArg)
        {
            mDrag = (eventArg as GuiComponentDragEvent).Drag;

            base.Invoke(control, eventArg);
        }

        public GuiComponentDragEventPart()
        {

        }

        public GuiComponentDragEventPart(GuiComponentEventSolver solver) : base(solver)
        {

        }
    }
}
