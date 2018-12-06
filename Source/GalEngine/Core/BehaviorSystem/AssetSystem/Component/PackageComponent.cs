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

        public Asset LoadAsset(string path, string name, List<Asset> dependentAssets)
        {
            if (mAssets[name].Reference == 0) mAssets[name].Load(System.IO.File.ReadAllBytes(path + name), dependentAssets);

            return mAssets[name].IncreaseReference();
        }

        public Asset LoadAssetRange(string path, string name, int start, int size, List<Asset> dependentAssets)
        {
            throw new NotImplementedException();
        }

        public void UnLoadAsset(string name)
        {
            mAssets[name].DecreaseReference();
            
            if (mAssets[name].Reference == 0) mAssets[name].UnLoad();
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
