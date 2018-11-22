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
        private GameObject parent = null;
        private Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        public string Name { get; private set; }

        public List<GameObject> Children { get; private set; }

        public GameObject Parent
        {
            get => parent; set
            {
                if (parent != null) parent.RemoveChild(this);

                parent = value;
                parent?.AddChild(this);
            }
        }

        public GameObject()
        {
            Children = new List<GameObject>();
        }

        public GameObject(string name)
        {
            Name = name;

            Children = new List<GameObject>();
        }

        public virtual void Update(float deltaTime)
        {
            foreach (var child in Children)
            {
                child.Update(deltaTime);
            }
        }

        public void AddComponent<TBaseComponent>(TBaseComponent commponent) where TBaseComponent : Component
        {
            components[commponent.BaseComponentType] = commponent;
        }

        public void AddComponent<TBaseComponent>() where TBaseComponent : Component, new()
        {
            components[typeof(TBaseComponent)] = new TBaseComponent();
        }

        public void RemoveComponent<TBaseComponent>() where TBaseComponent : Component
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
            parent?.AddChild(this);
        }

        public void AddChild(GameObject child)
        {
            child.Parent?.RemoveChild(child);
            child.parent = this;

            Children.Add(child);
        }

        public void RemoveChild(GameObject child)
        {
            if (Children.Contains(child) is false) return;

            Children.Remove(child);

            child.parent = null;
        }
    }
}
