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
            throw new NotImplementedException();
        }
    }

    public class GameObject
    {
        private string name;
      
        private GameObject parent = null;
        private List<GameObject> children = new List<GameObject>();
        private Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        public string Name { get => name; set => name = value; }

        public List<GameObject> Children { get => children; }
       
        public GameObject Parent
        {
            get => parent; set
            {
                if (parent != null) parent.CancelChild(this);

                parent = value;
                parent?.SetChild(this);
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

        public virtual void Update(float deltaTime)
        {
            foreach (var child in children)
            {
                child.Update(deltaTime);
            }
        }

        public void SetComponent<T>(T commponent) where T : Component
        {
            components[commponent.BaseComponentType] = commponent;
        }

        public void SetComponent<BaseComponent>() where BaseComponent : Component, new()
        {
            components[typeof(BaseComponent)] = new BaseComponent();
        }

        public void CancelComponent<BaseComponent>() where BaseComponent : Component
        {
            components.Remove(typeof(BaseComponent));
        }

        public BaseComponent GetComponent<BaseComponent>() where BaseComponent : Component
        {
            return components[typeof(BaseComponent)] as BaseComponent;
        }

        public bool IsComponentExist<BaseComponent>() where BaseComponent : Component
        {
            return components.ContainsKey(typeof(BaseComponent));
        }

        public bool IsComponentExist(Type type)
        {
            return components.ContainsKey(type);
        }

        public void SetChild(GameObject child)
        {
            if (child.Parent != null) child.Parent.CancelChild(child);

            children.Add(child);
            child.parent = this;
        }

        public void CancelChild(GameObject child)
        {
            if (children.Contains(child) is false) return;

            child.parent = null;
            children.Remove(child);
        }
    }
}
