using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class Component
    {
        protected Type baseComponentType = typeof(Component);

        public Type BaseComponentType { get => baseComponentType; }
    }
}
