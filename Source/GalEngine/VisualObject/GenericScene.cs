using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Builder;

namespace GalEngine
{
    public class GenericScene
    {
        private string name;

        private List<VisualObject> visualObjects = new List<VisualObject>();

        internal void ProcessKeyEvent(object sender, KeyEventArgs e)
        {
            OnKeyEvent(sender, e);

            KeyEvent?.Invoke(sender, e);

            foreach (var item in visualObjects)
            {
                item.PrivateOnKeyEvent(item, e);
            }
        }

        internal void ProcessMouseClick(object sender, MouseClickEventArgs e)
        {
            OnMouseClick(sender, e);

            MouseClick?.Invoke(sender, e);

            foreach (var item in visualObjects)
            {
                item.PrivateOnMouseClick(item, e);
            }
        }

        internal void ProcessMouseMove(object sender, MouseMoveEventArgs e)
        {
            OnMouseMove(sender, e);

            MouseMove?.Invoke(sender, e);

            foreach (var item in visualObjects)
            {
                item.PrivateOnMouseMove(item, e);
            }
        }

        internal void ProcessMouseWheel(object sender, MouseWheelEventArgs e)
        {
            OnMouseWheel(sender, e);

            MouseWheel?.Invoke(sender, e);

            foreach (var item in visualObjects)
            {
                item.PrivateOnMouseWheel(item, e);
            }
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

            foreach (var item in visualObjects)
            {
                item.PrivateOnUpdate(item);
            }
        }

        internal void ProcessRender(object sender)
        {
            foreach (var item in visualObjects)
            {
                item.OnRender();
            }
        }

        public GenericScene(string Name)
        {
            name = Name;

            SceneList.AddScene(this);
        }

        public void AddVisualObject(string name)
        {
            visualObjects.Add(VisualObjectList.GetVisualObject(name));
        }

        public void RemoveVisualObject(string name)
        {
            visualObjects.Remove(VisualObjectList.GetVisualObject(name));
        }

        public virtual void OnKeyEvent(object sender, KeyEventArgs e) { }
        public virtual void OnMouseClick(object sender, MouseClickEventArgs e) { }
        public virtual void OnMouseMove(object sender, MouseMoveEventArgs e) { }
        public virtual void OnMouseWheel(object sender, MouseWheelEventArgs e) { }
        public virtual void OnSizeChange(object sender, SizeChangeEventArgs e) { }
        public virtual void OnUpdate(object sender) { }

        public event UpdateHandler Update;
        public event MouseMoveHandler MouseMove;
        public event MouseClickHandler MouseClick;
        public event MouseWheelHandler MouseWheel;
        public event KeyEventHandler KeyEvent;
        public event SizeChangeEventHandler SizeChange;

        public string Name => name;
    }
}
