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
        
        public string Path { get; }
        public string FullPath => GetFullPath();

        internal AssetReference LoadAsset(string name, List<AssetReference> dependentAssets)
        {
            LogEmitter.Apply(LogLevel.Information, "[Load Asset] [Name = {0}] from [{1}]", name, Name);

            return mPackageComponent.LoadAsset(FullPath, name, dependentAssets);
        }

        internal AssetReference LoadAssetIndependent(string name, SegmentRange<int> range, List<AssetReference> dependentAssets)
        {
            LogEmitter.Apply(LogLevel.Information, "[Load Asset Independent] [Name = {0}] from [{1}]", name, Name);

            return mPackageComponent.LoadAssetIndependent(FullPath, name, range, dependentAssets);
        }

        internal void UnLoadAsset(ref AssetReference assetReference)
        {
            LogEmitter.Apply(LogLevel.Information, "[UnLoad Asset] [Name = {0}] from [{1}]", assetReference.Source.Name, Name);

            mPackageComponent.UnLoadAsset(ref assetReference);
        }

        internal void AddAsset(Asset asset)
        {
            LogEmitter.Apply(LogLevel.Information, "[Add Asset] [Type = {0}] [Name = {1}] from [{2}]", asset.GetType().Name, asset.Name, Name);

            //if the asset has been in the other package
            //we change the package asset in
            if (asset.Package != null)
            {
                LogEmitter.Apply(LogLevel.Warning, "[Add Asset and Change the Asset's Package] [Type = {0}] [Name = {1}] from [{2}]",
                    asset.GetType().Name, asset.Name, Name);

                asset.Package.RemoveAsset(asset);
            }

            asset.Package = this;

            mPackageComponent.AddAsset(asset);
        }

        internal void RemoveAsset(Asset asset)
        {
            LogEmitter.Apply(LogLevel.Information, "[Remove Asset] [Type = {0}] [Name = {1}] from [{2}]", asset.GetType().Name, asset.Name, Name);

            asset.Package = null;

            mPackageComponent.RemoveAsset(asset);
        }

        public PackageProvider(string name, string path) : base(name)
        {
            AddComponent(mPackageComponent = new PackageComponent());
            
            Path = path;
            
            LogEmitter.Apply(LogLevel.Information, "[Initialize PackageProvider Finish] from [{0}]", Name);
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
