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
        private Dictionary<string, PackageBytesResource> mResources;

        public PackageComponent()
        {
            mResources = new Dictionary<string, PackageBytesResource>();
        }

        public PackageBytesResource Load(string path, string name)
        {
            if (mResources[name].Reference == 0)
                mResources[name].Bytes = System.IO.File.ReadAllBytes(path + name);
            return mResources[name].IncreaseReference();
        }

        public PackageBytesResource LoadRange(string path, string name, int start, int size)
        {
            var resource = new PackageBytesResource(name, size);

            Array.Copy(System.IO.File.ReadAllBytes(path + name), 0, resource.Bytes, start, size);

            return resource;
        }

        public void UnLoad(string name)
        {
            mResources[name].DecreaseReference();

            if (mResources[name].Reference == 0) mResources[name].Bytes = null;
        }

        public void AddResource(PackageBytesResource packageBytesResource)
        {
            mResources.Add(packageBytesResource.Name, packageBytesResource);
        }

        public void RemoveResource(PackageBytesResource packageBytesResource)
        {
            mResources.Remove(packageBytesResource.Name);
        }
    }
}
