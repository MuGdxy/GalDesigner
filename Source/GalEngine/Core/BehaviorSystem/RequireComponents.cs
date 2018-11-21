using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class RequireComponents
    {
        private readonly List<Type> requireComponentType = new List<Type>();

        public void SetRequireComponentType<TBaseComponent>() where TBaseComponent : Component
        {
            if (requireComponentType.Contains(typeof(TBaseComponent)) is true) return;

            requireComponentType.Add(typeof(TBaseComponent));
        }

        public void CancelRequireComponentType<TBaseComponent>() where TBaseComponent : Component
        {
            if (requireComponentType.Contains(typeof(TBaseComponent)) is false) return;

            requireComponentType.Remove(typeof(TBaseComponent));
        }

        public bool IsPass(GameObject gameObject)
        {
            foreach (var type in requireComponentType)
            {
                if (gameObject.IsComponentExist(type) is false) return false;
            }

            return true;
        }
    }
}
