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
        private GameObject mParent = null;
        private Dictionary<Type, Component> mComponents = new Dictionary<Type, Component>();

        public string Name { get; private set; }

        public List<GameObject> Children { get; private set; }

        public GameObject Parent
        {
            get => mParent; set
            {
                if (mParent != null) mParent.RemoveChild(this);

                mParent = value;
                mParent?.AddChild(this);
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
            mComponents[commponent.BaseComponentType] = commponent;
        }

        public void AddComponent<TBaseComponent>() where TBaseComponent : Component, new()
        {
            mComponents[typeof(TBaseComponent)] = new TBaseComponent();
        }

        public void RemoveComponent<TBaseComponent>() where TBaseComponent : Component
        {
            mComponents.Remove(typeof(TBaseComponent));
        }

        public TBaseComponent GetComponent<TBaseComponent>() where TBaseComponent : Component
        {
            return mComponents[typeof(TBaseComponent)] as TBaseComponent;
        }

        public bool IsComponentExist<TBaseComponent>() where TBaseComponent : Component
        {
            return mComponents.ContainsKey(typeof(TBaseComponent));
        }

        public bool IsComponentExist(Type type)
        {
            return mComponents.ContainsKey(type);
        }

        public void SetParent(GameObject parent)
        {
            parent?.AddChild(this);
        }

        public void AddChild(GameObject child)
        {
            child.Parent?.RemoveChild(child);
            child.mParent = this;

            Children.Add(child);
        }

        public void RemoveChild(GameObject child)
        {
            if (Children.Contains(child) is false) return;

            Children.Remove(child);

            child.mParent = null;
        }

        public GameObject GetChild(string name)
        {
            foreach (var child in Children)
                if (child.Name == name) return child;

            return null;
        }

        public bool IsChildExist(string name)
        {
            foreach (var child in Children)
                if (child.Name == name) return true;

            return false;
        }

        public bool IsChildExist(GameObject gameObject)
        {
            foreach (var child in Children)
                if (child == gameObject) return true;

            return false;
        }

    }
}
