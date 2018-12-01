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

        public Asset Load(string path, string name)
        {
            if (mAssets[name].Reference == 0) mAssets[name].Load(System.IO.File.ReadAllBytes(path + name));

            return mAssets[name].IncreaseReference();
        }

        public Asset LoadRange(string path, string name, int start, int size)
        {
            throw new NotImplementedException();
        }

        public void UnLoad(string name)
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
    }
}
