using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Builder;

namespace GalEngine
{
    public abstract class GenericPage
    {
        private string tag;

        internal void ProcessKeyEvent(object sender, KeyEventArgs e)
        {
            OnKeyEvent(sender, e);

            KeyEvent?.Invoke(sender, e);
        }

        internal void ProcessMouseClick(object sender, MouseClickEventArgs e)
        {
            OnMouseClick(sender, e);

            MouseClick?.Invoke(sender, e);
        }

        internal void ProcessMouseMove(object sender, MouseMoveEventArgs e)
        {
            OnMouseMove(sender, e);

            MouseMove?.Invoke(sender, e);
        }

        internal void ProcessMouseWheel(object sender, MouseWheelEventArgs e)
        {
            OnMouseWheel(sender, e);

            MouseWheel?.Invoke(sender, e);
        }

        internal void ProcessSizeChange(object sender, SizeChangeEventArgs e)
        {
            OnSizeChange(sender, e);

            SizeChange?.Invoke(sender, e);
        }

        internal void ProcessUpdate(object sender)
        {
            OnUpdate(sender);

            Update?.Invoke(sender);
        }

        public GenericPage(string Tag)
        {
            tag = Tag;

            PageList.AddPage(tag, this);
        }

        public abstract void OnKeyEvent(object sender, KeyEventArgs e);
        public abstract void OnMouseClick(object sender, MouseClickEventArgs e);
        public abstract void OnMouseMove(object sender, MouseMoveEventArgs e);
        public abstract void OnMouseWheel(object sender, MouseWheelEventArgs e);
        public abstract void OnSizeChange(object sender, SizeChangeEventArgs e);
        public abstract void OnUpdate(object sender);

        public event UpdateHandler Update;
        public event MouseMoveHandler MouseMove;
        public event MouseClickHandler MouseClick;
        public event MouseWheelHandler MouseWheel;
        public event KeyEventHandler KeyEvent;
        public event SizeChangeEventHandler SizeChange;

        public string Tag => tag;
    }
}
