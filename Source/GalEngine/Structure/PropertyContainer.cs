using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class PropertyContainer<T>
    {
        private Dictionary<string, T> mProperties;

        public PropertyContainer()
        {
            mProperties = new Dictionary<string, T>();
        }

        public void SetProperty(string name, T property)
        {
            mProperties[name] = property;
        }

        public T GetProperty(string name)
        {
            if (mProperties.ContainsKey(name) == false) return default(T);
            
            return mProperties[name];
        }
    }
}
