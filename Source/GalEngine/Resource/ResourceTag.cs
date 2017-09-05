using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    class ResourceTag
    {
        private int count;
        private string tag;
        private object resource;

        protected virtual void ActiveResource(ref object resource)
        {
            resource = null;
        }

        protected virtual void DiposeResource(ref object resource)
        {
            resource = null;
        }

        public ResourceTag(string Tag)
        {
            tag = Tag;
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

        public string Tag => tag;
    }
}
