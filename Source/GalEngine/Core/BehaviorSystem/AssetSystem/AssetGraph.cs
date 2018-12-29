using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    using Debug = System.Diagnostics.Debug;
    
    class AssetGraph : Graph<Asset>
    {
        private AssetReference LoadAsset(GraphNode<Asset> node)
        {
            //exist, so we only add reference
            if (node.Data.Reference != 0)
                return node.Data.Package.LoadAsset(node.Data.Name, null);

            var dependentAssets = new List<AssetReference>();

            //get dependent assets
            foreach (var need in node.Next)
            {
                dependentAssets.Add(LoadAsset(need));
            }

            //load resource
            var result = node.Data.Package.LoadAsset(node.Data.Name, dependentAssets);

            //dispose the dependent assets
            //we only keep the asset we need
            foreach (var assetReference in dependentAssets)
            {
                var template = assetReference;

                UnLoadAsset(ref template);
            }

            return result;
        }

        public AssetReference LoadAsset(Asset asset)
        {
            return LoadAsset(mNodes[asset]);
        }

        public AssetReference LoadAssetIndependent(Asset asset, SegmentRange<int> range)
        {
            var node = mNodes[asset];
            var dependentAssets = new List<AssetReference>();

            //get dependent assets
            foreach (var need in node.Next)
            {
                dependentAssets.Add(LoadAsset(need));
            }

            //load resource
            var result = node.Data.Package.LoadAssetIndependent(node.Data.Name, range, dependentAssets);

            foreach (var assetReference in dependentAssets)
            {
                var template = assetReference;

                UnLoadAsset(ref template);
            }

            return result;
        }

        public void UnLoadAsset(ref AssetReference assetReference)
        {
            assetReference.Source.Package.UnLoadAsset(ref assetReference);
        }

        public void UnLoadAssetIndependent(ref AssetReference assetReference)
        {
            assetReference.Source.UnLoadIndependentReference(ref assetReference);
        }
    }
}
