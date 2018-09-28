using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class RequireComponents
    {
        private List<Type> requireComponentType = new List<Type>();

        public void SetRequireComponentType<BaseComponent>() where BaseComponent : Component
        {
            if (requireComponentType.Contains(typeof(BaseComponent)) is true) return;

            requireComponentType.Add(typeof(BaseComponent));
        }

        public void CancelRequireComponentType<BaseComponent>() where BaseComponent : Component
        {
            if (requireComponentType.Contains(typeof(BaseComponent)) is false) return;

            requireComponentType.Remove(typeof(BaseComponent));
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
