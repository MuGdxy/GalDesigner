using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class Component
    {
        public Type BaseComponentType { get; protected set; }

        public bool IsBaseComponentType => GetType() == BaseComponentType;

        public Component()
        {
            BaseComponentType = typeof(Component);
        }
    }
}
