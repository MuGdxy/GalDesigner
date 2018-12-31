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
        private Dictionary<string, AssetDescription> mAssetDescriptions;
        
        public PackageComponent()
        {
            BaseComponentType = typeof(PackageComponent);

            mAssetDescriptions = new Dictionary<string, AssetDescription>();
        }

        public Asset CreateAsset(string path, string name, List<Asset> dependentAssets)
        {
            //create asset with reference count
            byte[] bytes = mAssetDescriptions[name].Reference == 0 ? System.IO.File.ReadAllBytes(path + "/" + name) : null;

            return mAssetDescriptions[name].IncreaseAssetReference(bytes, dependentAssets);
        }

        public Asset CreateIndependentAsset(string path, string name, SegmentRange<int> range, List<Asset> dependentAssets)
        {
            //create independent asset(without reference count)
            //we do not have any reference count for independent asset
            System.IO.FileStream file = new System.IO.FileStream(path + "/" + name, System.IO.FileMode.Open);

            byte[] bytes = new byte[range.End - range.Start + 1];

            file.Read(bytes, range.Start, bytes.Length);

            return mAssetDescriptions[name].CreateIndependentAsset(bytes, dependentAssets);
        }

        public Asset DestoryAsset(Asset asset)
        {
            return mAssetDescriptions[asset.AssetDescription.Name].DecreaseAssetReference(asset);
        }

        public Asset DestoryIndependentAsset(Asset asset)
        {
            return mAssetDescriptions[asset.AssetDescription.Name].DestoryIndependentAsset(asset);
        }

        public void AddAssetDescription(AssetDescription description)
        {
            mAssetDescriptions.Add(description.Name, description);
        }

        public void RemoveAssetDescription(AssetDescription description)
        {
            RuntimeException.Assert(description.Reference == 0);

            mAssetDescriptions.Remove(description.Name);
        }
        
        public AssetDescription GetAssetDescription(string name)
        {
            return mAssetDescriptions[name];
        }
    }
}
