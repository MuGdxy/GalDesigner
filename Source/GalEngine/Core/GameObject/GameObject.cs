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

        public List<GameObject> Children => children;

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

        public GameObject(string name)
        {
            this.name = name;
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

        public void SetComponent<TBaseComponent>() where TBaseComponent : Component, new()
        {
            components[typeof(TBaseComponent)] = new TBaseComponent();
        }

        public void CancelComponent<TBaseComponent>() where TBaseComponent : Component
        {
            components.Remove(typeof(TBaseComponent));
        }

        public TBaseComponent GetComponent<TBaseComponent>() where TBaseComponent : Component
        {
            return components[typeof(TBaseComponent)] as TBaseComponent;
        }

        public bool IsComponentExist<TBaseComponent>() where TBaseComponent : Component
        {
            return components.ContainsKey(typeof(TBaseComponent));
        }

        public bool IsComponentExist(Type type)
        {
            return components.ContainsKey(type);
        }

        public void SetParent(GameObject parent)
        {
            parent?.SetChild(this);
        }

        public void SetChild(GameObject child)
        {
            child.Parent?.CancelChild(child);

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
