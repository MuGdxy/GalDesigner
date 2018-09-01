using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    class GameObjectDepthComparer : IComparer<GameObject>
    {
        public int Compare(GameObject x, GameObject y)
        {
            float xDepth = x.IsCommponentExist<Transform>() is true ? x.GetCommponent<Transform>().Depth : GameDefault.Depth;
            float yDepth = y.IsCommponentExist<Transform>() is true ? y.GetCommponent<Transform>().Depth : GameDefault.Depth;

            return xDepth.CompareTo(yDepth);
        }
    }

    public class GameObject
    {
        private readonly string name;
      
        private bool isHover = false;

        private bool isEnableRead = false;
        private bool isEnableVisual = true;

        private GameObject parent = null;
        private Dictionary<string, GameObject> children = new Dictionary<string, GameObject>();
        private Dictionary<Type, Commponent> commponents = new Dictionary<Type, Commponent>();

        public string Name { get => name; }
       
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

        internal static void RenderGameObject(GameObject GameObject, Matrix3x2 BaseTransform, Camera Camera)
        {
            if (Camera is null) return;
            if (GameObject.IsEnableVisual is false) return;
            if (GameObject.IsCommponentExist<Sharp>() is false) return;

            var transform = (GameObject.IsCommponentExist<Transform>() is true ?
                GameObject.GetCommponent<Transform>().Matrix : Matrix3x2.Identity) * BaseTransform;
            var sharp = GameObject.GetCommponent<Sharp>();

            if (Utility.IsIntersect(Camera, sharp.Size, transform) is true)
            {
                Vector2 internalScale = new Vector2(GameScene.Resolution.Width / Camera.Size.Width,
                    GameScene.Resolution.Height / Camera.Size.Height);
                Vector2 scaleCenter = new Vector2(Camera.Area.Left, Camera.Area.Top);

                Systems.Graphics.SetTransform(transform * Matrix3x2.CreateScale(internalScale, scaleCenter)
                    * Matrix3x2.CreateTranslation(-scaleCenter));

                foreach (var item in GameObject.commponents)
                {
                    item.Value.OnRender(GameObject);
                }
            } 

            //need change
            List<GameObject> sortedChildren = GameObject.children.Values.ToList();

            sortedChildren.Sort(new GameObjectDepthComparer());

            foreach (var item in sortedChildren)
            {
                RenderGameObject(item, transform, Camera);
            }
        }

        internal static void ProcessMouseMove(GameObject GameObject, MouseMoveEvent EventArg, Matrix3x2 BaseTransform)
        {
            if (GameObject.IsCommponentExist<Sharp>() is false) return;

            var transform = GameObject.IsCommponentExist<Transform>() is true ? GameObject.GetCommponent<Transform>() : new Transform();
            var sharp = GameObject.GetCommponent<Sharp>();

            transform.Update(sharp.Size);

            BaseTransform = transform.Matrix * BaseTransform;

            if (Utility.IsContain(EventArg.MousePosition, sharp.Size, BaseTransform) is true)
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
            if (GameObject.IsCommponentExist<Sharp>() is false) return;

            var transform = GameObject.IsCommponentExist<Transform>() is true ? GameObject.GetCommponent<Transform>() : new Transform();
            var sharp = GameObject.GetCommponent<Sharp>();

            transform.Update(sharp.Size);

            BaseTransform = transform.Matrix * BaseTransform;

            if (Utility.IsContain(EventArg.MousePosition, sharp.Size, BaseTransform) is true)
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
            if (GameObject.IsCommponentExist<Sharp>() is false) return;

            var transform = GameObject.IsCommponentExist<Transform>() is true ? GameObject.GetCommponent<Transform>() : new Transform();
            var sharp = GameObject.GetCommponent<Sharp>();

            transform.Update(sharp.Size);

            BaseTransform = transform.Matrix * BaseTransform;

            if (Utility.IsContain(EventArg.MousePosition, sharp.Size, BaseTransform) is true)
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
            GameObject.OnUpdate(GameObject);
            GameObject.Update?.Invoke(GameObject);

            if (GameObject.IsCommponentExist<Transform>() is true && GameObject.IsCommponentExist<Sharp>() is true)
                GameObject.GetCommponent<Transform>().Update(GameObject.GetCommponent<Sharp>().Size);
            
            foreach (var item in GameObject.children)
            {
                ProcessUpdate(item.Value);
            }
        }

        public GameObject()
        {
            name = GameDefault.GameObjectName + GetHashCode().ToString();
        }

        public GameObject(string Name)
        {
            name = Name;
        }

        public void SetCommponent<T>(T commponent) where T : Commponent
        {
            commponents[commponent.GetType()] = commponent;
        }

        public void SetCommponent<T>() where T : Commponent, new()
        {
            commponents[typeof(T)] = new T();
        }

        public void CancelCommponent<T>() where T : Commponent
        {
            commponents.Remove(typeof(T));
        }

        public T GetCommponent<T>() where T : Commponent
        {
            return commponents[typeof(T)] as T;
        }

        public bool IsCommponentExist<T>() where T : Commponent
        {
            return commponents.ContainsKey(typeof(T));
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
