using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    /// <summary>
    /// package component
    /// manager the resource in the package
    /// </summary>
    public class PackageComponent : Component
    {
        private Dictionary<string, PackageBytesResource> mCachedResource;
        
        public string Name { get; private set; }

        public PackageComponent(string name)
        {
            Name = name;
            
            mCachedResource = new Dictionary<string, PackageBytesResource>();
        }

        public PackageBytesResource Load(string name)
        {
            if (mCachedResource.ContainsKey(name) is true) return mCachedResource[name].IncreaseReference();

            mCachedResource[name] =  new PackageBytesResource(name, System.IO.File.ReadAllBytes(name));

            return mCachedResource[name].IncreaseReference();
        }

        public PackageBytesResource LoadRange(string name, int start, int size)
        {
            var resource = new PackageBytesResource(name, size);

            Array.Copy(System.IO.File.ReadAllBytes(name), 0, resource.Bytes, start, size);

            return resource;
        }

        public void UnLoad(string name)
        {
            mCachedResource[name].DecreaseReference();

            if (mCachedResource[name].Reference == 0) mCachedResource.Remove(name);
        }
    }
}
