using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    using Debug = System.Diagnostics.Debug;

    public class AssetSystem : BehaviorSystem
    {
        private AssetGraph mAssetGraph;

        public AssetSystem() : base("AssetSystem")
        {
            RequireComponents.AddRequireComponentType<PackageComponent>();

            mAssetGraph = new AssetGraph();
        }

        public void AddAsset(PackageProvider package, Asset asset)
        {
            package.AddAsset(asset);

            mAssetGraph.AddNode(new GraphNode<Asset>(asset));
        }

        public void RemoveAsset(Asset asset)
        {
            asset.Package.RemoveAsset(asset);
            
            mAssetGraph.RemoveNode(asset);
        }

        public void AddAssetDependency(Asset asset, Asset dependentAsset)
        {
            mAssetGraph.AddEdge(asset, dependentAsset);
        }

        public void AddAssetDependencies(Asset asset, List<Asset> dependentAssets)
        {
            foreach (var dependentAsset in dependentAssets)
            {
                mAssetGraph.AddEdge(asset, dependentAsset);
            }
        }

        public void RemoveAssetDependency(Asset asset, Asset dependentAsset)
        {
            mAssetGraph.RemoveEdge(asset, dependentAsset);
        }

        public void RemoveAssetDependencies(Asset asset, List<Asset> dependentAssets)
        {
            foreach (var dependentAsset in dependentAssets)
            {
                mAssetGraph.RemoveEdge(asset, dependentAsset);
            }
        }

        public AssetReference LoadAsset(Asset asset)
        {
            return mAssetGraph.LoadAsset(asset);
        }

        public AssetReference LoadAssetIndependent(Asset asset, SegmentRange<int> range)
        {
            return mAssetGraph.LoadAssetIndependent(asset, range);
        }

        public void UnLoadAsset(ref AssetReference assetReference)
        {
            Debug.Assert(assetReference.IsReference is true);

            mAssetGraph.UnLoadAsset(ref assetReference);
        }

        public void UnLoadAssetIndependent(ref AssetReference assetReference)
        {
            Debug.Assert(assetReference.IsReference is false);

            mAssetGraph.UnLoadAssetIndependent(ref assetReference);
        }

        public override void Excute(GameObject gameObject)
        {
            
        }
    }
}
