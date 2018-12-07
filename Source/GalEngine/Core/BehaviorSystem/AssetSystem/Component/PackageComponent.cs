using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    using Debug = System.Diagnostics.Debug;

    /// <summary>
    /// package component
    /// manager the resource in the package
    /// </summary>
    public class PackageComponent : Component
    {
        private Dictionary<string, Asset> mAssets;
        
        public PackageComponent()
        {
            BaseComponentType = typeof(PackageComponent);

            mAssets = new Dictionary<string, Asset>();
        }

        public AssetReference LoadAsset(string path, string name, List<AssetReference> dependentAssets)
        {
            Debug.Assert(mAssets[name].Reference >= 0);

            if (mAssets[name].Reference == 0) mAssets[name].Load(System.IO.File.ReadAllBytes(path + name), dependentAssets);

            return mAssets[name].IncreaseReference();
        }

        public AssetReference LoadAssetIndependent(string path, string name, SegmentRange<int> range, List<AssetReference> dependentAssets)
        {
            System.IO.FileStream file = new System.IO.FileStream(path + name, System.IO.FileMode.Open);

            byte[] bytes = new byte[range.End - range.Start + 1];

            file.Read(bytes, range.Start, bytes.Length);

            return mAssets[name].LoadIndependentReference(bytes, dependentAssets);
        }

        public void UnLoadAsset(ref AssetReference assetReference)
        {
            var name = assetReference.Source.Name;

            mAssets[name].DecreaseReference();
            
            if (mAssets[name].Reference == 0) mAssets[name].UnLoad();

            assetReference = null;
        }

        public void AddAsset(Asset asset)
        {
            mAssets.Add(asset.Name, asset);
        }

        public void RemoveAsset(Asset asset)
        {
            Debug.Assert(asset.Reference == 0);

            mAssets.Remove(asset.Name);
        }

        public Asset GetAsset(string name)
        {
            return mAssets[name];
        }
    }
}
