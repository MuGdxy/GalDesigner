using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Package : GameObject
    {
        private PackageComponent mPackageComponent;
        
        public string Path { get; }
        public string FullPath => GetFullPath();

        internal Asset CreateAsset(string name, List<Asset> dependentAssets)
        {
            return mPackageComponent.CreateAsset(FullPath, name, dependentAssets);
        }

        internal Asset CreateIndependentAsset(string name, SegmentRange<int> range, List<Asset> dependentAssets)
        {
            return mPackageComponent.CreateIndependentAsset(FullPath, name, range, dependentAssets);
        }

        internal Asset DestoryAsset(Asset asset)
        {
            return mPackageComponent.DestoryAsset(asset);
        }

        internal Asset DestoryIndependentAsset(Asset asset)
        {
            return mPackageComponent.DestoryIndependentAsset(asset);
        }

        internal void AddAssetDescription(AssetDescription description)
        {
            LogEmitter.Apply(LogLevel.Information, "[Add Asset Description] [Type = {0}] [Name = {1}] from [{2}]", description.Type, description.Name, Name);

            //if the asset has been in the other package
            //we change the package asset in
            if (description.Package != null)
            {
                LogEmitter.Apply(LogLevel.Warning, "[Add Asset Description and Change the Package] [Type = {0}] [Name = {1}] from [{2}]",
                    description.Type, description.Name, Name);

                description.Package.RemoveAssetDescription(description);
            }

            description.Package = this;

            mPackageComponent.AddAssetDescription(description);
        }

        internal void RemoveAssetDescription(AssetDescription description)
        {
            LogEmitter.Apply(LogLevel.Information, "[Remove Asset Description] [Type = {0}] [Name = {1}] from [{2}]", description.Type, description.Name, Name);

            description.Package = null;

            mPackageComponent.RemoveAssetDescription(description);
        }

        public Package(string name, string path) : base(name)
        {
            AddComponent(mPackageComponent = new PackageComponent());
            
            Path = path;
            
            LogEmitter.Apply(LogLevel.Information, "[Initialize PackageProvider Finish] from [{0}]", Name);
        }

        public AssetDescription GetAssetDescription(string name)
        {
            return mPackageComponent.GetAssetDescription(name);
        }

        public string GetFullPath()
        {
            if (Parent.GetType() != typeof(Package)) return Path;

            return (Parent as Package)?.GetFullPath() + "/" + Path;
        }
    }
}
