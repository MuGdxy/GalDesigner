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

        public string Path { get; }
        public string FullPath => GetFullPath();

        public PackageProvider(string name, string path) : base(name)
        {
            AddComponent(mPackageComponent = new PackageComponent());
            AddComponent(mLogComponent = new LogComponent(name));

            Path = path;

            mLogComponent.Log(StringGroup.Log + "[Initialize PackageProvider Finish].");
        }

        public Asset Load(string name)
        {
            mLogComponent.Log(StringGroup.Log + "[Load Asset] [Name = {0}].", name);

            return mPackageComponent.Load(FullPath, name);
        }

        public Asset LoadRange(string name, int start, int size)
        {
            mLogComponent.Log(StringGroup.Log + "[Load Asset Range] [Name = {0}].", name);

            return mPackageComponent.LoadRange(FullPath, name, start, size);
        }

        public void UnLoad(string name)
        {
            mLogComponent.Log(StringGroup.Log + "[UnLoad Asset] [Name = {0}].", name);

            mPackageComponent.UnLoad(name);
        }

        public void AddAsset(Asset asset)
        {
            mLogComponent.Log(StringGroup.Log + "[Add Asset] [Type = {0}] [Name = {1}].", asset.GetType(), asset.Name);

            mPackageComponent.AddAsset(asset);
        }

        public void RemoveAsset(Asset asset)
        {
            mLogComponent.Log(StringGroup.Log + "[Remove Asset] [Type = {0}] [Name = {1}].", asset.GetType(), asset.Name);

            mPackageComponent.RemoveAsset(asset);
        }

        public string GetFullPath()
        {
            if (GetType() != typeof(PackageProvider)) return "";

            return (Parent as PackageProvider)?.GetFullPath() + "/" + Path;
        }
    }
}
