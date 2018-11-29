using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    /// <summary>
    /// a package 
    /// we can load or unload resource
    /// </summary>
    public class PackageProvider : GameObject
    {
        private PackageComponent mPackageComponent;
        private LogComponent mLogComponent;

        public string FullPath => GetFullPath();

        public PackageProvider(string name) : base(name)
        {
            AddComponent(mPackageComponent = new PackageComponent());
            AddComponent(mLogComponent = new LogComponent(name));

            mLogComponent.Log(StringGroup.Log + "[Initialize Package Provider Finish].");
        }

        public PackageBytesResource Load(string name)
        {
            return mPackageComponent.Load(FullPath, name);
        }

        public PackageBytesResource LoadRange(string name, int start, int size)
        {
            return mPackageComponent.LoadRange(FullPath, name, start, size);
        }

        public void UnLoad(string name)
        {
            mPackageComponent.UnLoad(name);
        }

        public PackageProvider FindPackage(string path)
        {
            var package = this;
            var pathArray = path.Split('/');

            foreach (var name in pathArray)
            {
                package = package.GetChild(name) as PackageProvider;

                if (package == null) return null;
            }

            return package;
        }

        public void AddResource(PackageBytesResource packageBytesResource)
        {
            mPackageComponent.AddResource(packageBytesResource);
        }

        public void RemoveResource(PackageBytesResource packageBytesResource)
        {
            mPackageComponent.RemoveResource(packageBytesResource);
        }

        public string GetFullPath()
        {
            if (GetType() != typeof(PackageProvider)) return "";

            return (Parent as PackageProvider)?.GetFullPath() + "/" + Name;
        }
    }
}
