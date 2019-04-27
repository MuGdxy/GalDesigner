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

        internal protected virtual void Update(float deltaTime)
        {
            foreach (var child in Children)
            {
                child.Update(deltaTime);
            }
        }

        public GameObject() : this(null)
        {
        }

        public GameObject(string name)
        {
            Name = name;

            if (Name == null) Name = "GameObject" + GetHashCode();

            Children = new List<GameObject>();
        }

        public void AddComponent<TComponent>(TComponent component) where TComponent : Component
        {
            //we record the base component type and the component type for component
            //record the base component type for system execution
            mComponents[component.BaseComponentType] = component;

            //record the true component type for user
            if (!component.IsBaseComponentType) mComponents[component.GetType()] = component;
        }

        public void RemoveComponent<TComponent>() where TComponent : Component
        {
            //get component we want to remove
            var component = mComponents[typeof(TComponent)];

            //remove base component type
            mComponents.Remove(component.BaseComponentType);

            //remove true component type if the component type is not base component type
            if (!component.IsBaseComponentType) mComponents.Remove(component.GetType());
        }

        public TComponent GetComponent<TComponent>() where TComponent : Component
        {
            return mComponents[typeof(TComponent)] as TComponent;
        }

        public bool IsComponentExist<TComponent>() where TComponent : Component
        {
            return mComponents.ContainsKey(typeof(TComponent));
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
