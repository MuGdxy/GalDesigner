using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GameObject
    {
        private readonly string name;
        private Transform transform = new Transform();
        private SizeF size = new SizeF(0.0f, 0.0f);
        private float border = 0.0f;

        private string borderColor = GameDefault.Color;

        private bool isHover = false;

        private bool isEnableRead = false;
        private bool isEnableVisual = true;

        private GameObject parent = null;
        private Dictionary<string, GameObject> children = new Dictionary<string, GameObject>();

        public string Name { get => name; }
        public Transform Transform { get => transform; set => transform = value; }
        public SizeF Size { get => size; set => size = value; }
        public float Border { get => border; set => border = value; }
        public string BorderColor { get => borderColor; set => borderColor = value; }

        public bool IsHover => isHover;

        public bool IsEnableRead { get => isEnableRead; set => isEnableRead = value; }
        public bool IsEnableVisual { get => isEnableVisual; set => isEnableVisual = value; }

        public GameObject Parent
        {
            get => parent; set
            {
                if (parent != null) parent.CancelChild(this);

                parent = value;
                parent?.SetChild(this);
            }
        }

        public event MouseMoveHandler MouseMove;
        public event MouseClickHandler MouseClick;
        public event MouseWheelHandler MouseWheel;
        public event BoardClickHandler BoardClick;
        public event MouseEnterEventHandler MouseEnter;
        public event MouseLeaveEventHandler MouseLeave;
        public event UpdateHandler Update;

        protected virtual void OnMouseMove(object sender, MouseMoveEvent eventArg) { }
        protected virtual void OnMouseClick(object sender, MouseClickEvent eventArg) { }
        protected virtual void OnMouseWheel(object sender, MouseWheelEvent eventArg) { }
        protected virtual void OnBoardClick(object sender, BoardClickEvent eventArg) { }
        protected virtual void OnMouseEnter(object sender) { }
        protected virtual void OnMouseLeave(object sender) { }
        protected virtual void OnUpdate(object sender) { }

        internal static void RenderGameObject(GameObject GameObject, Matrix3x2 BaseTransform)
        {
            if (GameObject.IsEnableVisual is false) return;

            float halfWidth = GameObject.Size.Width * 0.5f;
            float halfHeight = GameObject.Size.Height * 0.5f;

            var transform = GameObject.Transform.Matrix * BaseTransform;
            var rectangle = new RectangleF(- halfWidth, -halfHeight, halfWidth, halfHeight);
            var borderColor = GameResource.IsColorExist(GameObject.BorderColor) is true ? GameObject.BorderColor : GameDefault.Color;

            Systems.Graphics.SetTransform(transform);

            Systems.Graphics.DrawRectangle(rectangle, borderColor, GameObject.Border);

            foreach (var item in GameObject.children)
            {
                RenderGameObject(item.Value, transform);
            }
        }

        internal static void ProcessMouseMove(GameObject GameObject, MouseMoveEvent EventArg, Matrix3x2 BaseTransform)
        {
            GameObject.Transform.Update();

            BaseTransform = GameObject.Transform.Matrix * BaseTransform;

            if (Utility.IsContain(EventArg.MousePosition, GameObject.Size, BaseTransform) is true)
            {
                GameObject.OnMouseMove(GameObject, EventArg);
                GameObject.MouseMove?.Invoke(GameObject, EventArg);

                if (GameObject.isHover is false) ProcessMouseEnter(GameObject);
            }
            else if (GameObject.isHover is true) ProcessMouseLeave(GameObject);

            foreach (var item in GameObject.children)
            {
                ProcessMouseMove(item.Value, EventArg, BaseTransform);
            }
        }

        internal static void ProcessMouseClick(GameObject GameObject, MouseClickEvent EventArg, Matrix3x2 BaseTransform)
        {
            GameObject.Transform.Update();

            BaseTransform = GameObject.Transform.Matrix * BaseTransform;

            if (Utility.IsContain(EventArg.MousePosition, GameObject.Size, BaseTransform) is true)
            {
                GameObject.OnMouseClick(GameObject, EventArg);
                GameObject.MouseClick?.Invoke(GameObject, EventArg);
            }

            foreach (var item in GameObject.children)
            {
                ProcessMouseClick(item.Value, EventArg, BaseTransform);
            }
        }

        internal static void ProcessMouseWheel(GameObject GameObject, MouseWheelEvent EventArg, Matrix3x2 BaseTransform)
        {
            GameObject.Transform.Update();

            BaseTransform = GameObject.Transform.Matrix * BaseTransform;

            if (Utility.IsContain(EventArg.MousePosition, GameObject.Size, BaseTransform) is true)
            {
                GameObject.OnMouseWheel(GameObject, EventArg);
                GameObject.MouseWheel?.Invoke(GameObject, EventArg);
            }

            foreach (var item in GameObject.children)
            {
                ProcessMouseWheel(item.Value, EventArg, BaseTransform);
            }
        }

        internal static void ProcessBoardClick(GameObject GameObject, BoardClickEvent EventArg)
        {
            if (GameObject.IsEnableRead is true)
            {
                GameObject.OnBoardClick(GameObject, EventArg);
                GameObject.BoardClick?.Invoke(GameObject, EventArg);
            }

            foreach (var item in GameObject.children)
            {
                ProcessBoardClick(item.Value, EventArg);
            }
        }

        internal static void ProcessMouseEnter(GameObject GameObject)
        {
            GameObject.isHover = true;

            GameObject.OnMouseEnter(GameObject);
            GameObject.MouseEnter?.Invoke(GameObject);
        }

        internal static void ProcessMouseLeave(GameObject GameObject)
        {
            GameObject.isHover = false;

            GameObject.OnMouseLeave(GameObject);
            GameObject.MouseLeave?.Invoke(GameObject);
        }

        internal static void ProcessUpdate(GameObject GameObject)
        {
            GameObject.Transform.Update();

            GameObject.OnUpdate(GameObject);
            GameObject.Update?.Invoke(GameObject);

            foreach (var item in GameObject.children)
            {
                ProcessUpdate(item.Value);
            }
        }

        public GameObject(Size Size)
        {
            name = GameDefault.GameObjectName + GetHashCode().ToString();
            size = Size;
        }

        public GameObject(string Name, Size Size)
        {
            name = Name;
            size = Size;
        }

        public void SetChild(GameObject Child)
        {
            if (Child.Parent != null) Child.Parent.CancelChild(Child);

            children[Child.Name] = Child;
            Child.parent = this;
        }

        public void CancelChild(GameObject Child)
        {
            if (children.ContainsKey(Child.Name) is false) return;

            Child.parent = null;
            children.Remove(Child.Name);
        }

        public void CancelChild(string Name)
        {
            if (children.ContainsKey(Name) is false) return;

            children[Name].parent = null;
            children.Remove(Name);
        }

        public GameObject GetChild(string Name)
        {
            if (children.ContainsKey(Name) is false) return null;

            return children[Name];
        }
    }
}
