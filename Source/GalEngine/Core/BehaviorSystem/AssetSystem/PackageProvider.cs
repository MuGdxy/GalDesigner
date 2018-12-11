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

        internal AssetReference LoadAsset(string name, List<AssetReference> dependentAssets)
        {
            mLogComponent.Log(StringProperty.Log + "[Load Asset] [Name = {0}].", LogLevel.Information, name);

            return mPackageComponent.LoadAsset(FullPath, name, dependentAssets);
        }

        internal AssetReference LoadAssetIndependent(string name, SegmentRange<int> range, List<AssetReference> dependentAssets)
        {
            mLogComponent.Log(StringProperty.Log + "[Load Asset Independent] [Name = {0}].", LogLevel.Information, name);

            return mPackageComponent.LoadAssetIndependent(FullPath, name, range, dependentAssets);
        }

        internal void UnLoadAsset(ref AssetReference assetReference)
        {
            mLogComponent.Log(StringProperty.Log + "[UnLoad Asset] [Name = {0}].", LogLevel.Information, assetReference.Source.Name);

            mPackageComponent.UnLoadAsset(ref assetReference);
        }

        internal void AddAsset(Asset asset)
        {
            mLogComponent.Log(StringProperty.Log + "[Add Asset] [Type = {0}] [Name = {1}].", LogLevel.Information, asset.GetType().Name, asset.Name);

            if (asset.Package != null)
            {
                mLogComponent.Log(StringProperty.Log + "[Add Asset and Change the Asset's Package] [Type = {0}] [Name = {1}].",
                    LogLevel.Warning, asset.GetType().Name, asset.Name);

                asset.Package.RemoveAsset(asset);
            }

            asset.Package = this;

            mPackageComponent.AddAsset(asset);
        }

        internal void RemoveAsset(Asset asset)
        {
            mLogComponent.Log(StringProperty.Log + "[Remove Asset] [Type = {0}] [Name = {1}].", LogLevel.Information, asset.GetType().Name, asset.Name);

            asset.Package = null;

            mPackageComponent.RemoveAsset(asset);
        }

        public PackageProvider(string name, string path) : base(name)
        {
            AddComponent(mPackageComponent = new PackageComponent());
            AddComponent(mLogComponent = new LogComponent(name));

            Path = path;

            mLogComponent.Log(StringProperty.Log + "[Initialize PackageProvider Finish].", LogLevel.Information);
        }

        public Asset GetAsset(string name)
        {
            return mPackageComponent.GetAsset(name);
        }

        public string GetFullPath()
        {
            if (Parent.GetType() != typeof(PackageProvider)) return Path;

            return (Parent as PackageProvider)?.GetFullPath() + "/" + Path;
        }
    }
}
