using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    abstract class ResourceView
    {
        private int count;
        private string name;
        private object resource;

        protected abstract void ActiveResource(ref object resource);
        protected abstract void DiposeResource(ref object resource);
        
        public ResourceView(string Name)
        {
            name = Name;
            resource = null;

            GlobalResource.SetValue(this);
        }

        public object Use()
        {
            if (count is 0) ActiveResource(ref resource);

            count++;
            return resource;
        }

        public void UnUse()
        {
            count--;

            if (count is 0) DiposeResource(ref resource);
            resource = null;
        }

        public string Name => name;
        public int Count => count;
    }
}
