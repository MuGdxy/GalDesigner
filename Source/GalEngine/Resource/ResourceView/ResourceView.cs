using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    abstract class ResourceView : IDisposable
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
            if (resource is null) ActiveResource(ref resource);
            
            count++;

            return resource;
        }

        public void UnUse()
        {
            count--;
        }

        public void Dispose()
        {
            DiposeResource(ref resource);
        }

        public string Name => name;
        public int Count => count;
    }
}
