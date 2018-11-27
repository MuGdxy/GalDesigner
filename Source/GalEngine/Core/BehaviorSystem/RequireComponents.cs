using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class RequireComponents
    {
        private readonly List<Type> mRequireComponentType = new List<Type>();

        public void AddRequireComponentType<TBaseComponent>() where TBaseComponent : Component
        {
            if (mRequireComponentType.Contains(typeof(TBaseComponent)) is true) return;

            mRequireComponentType.Add(typeof(TBaseComponent));
        }

        public void RemoveRequireComponentType<TBaseComponent>() where TBaseComponent : Component
        {
            if (mRequireComponentType.Contains(typeof(TBaseComponent)) is false) return;

            mRequireComponentType.Remove(typeof(TBaseComponent));
        }

        public bool IsPass(GameObject gameObject)
        {
            foreach (var type in mRequireComponentType)
            {
                if (gameObject.IsComponentExist(type) is false) return false;
            }

            return true;
        }
    }
}
