using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class AssetSystem : BehaviorSystem
    {
        private AssetGraph mAssetGraph;

        public AssetSystem() : base("AssetSystem")
        {
            RequireComponents.AddRequireComponentType<PackageComponent>();

            mAssetGraph = new AssetGraph();
        }

        public void AddAssetDescription(Package package, AssetDescription description)
        {
            package.AddAssetDescription(description);

            mAssetGraph.AddNode(new GraphNode<AssetDescription>(description));
        }

        public void RemoveAssetDescription(AssetDescription description)
        {
            description.Package.RemoveAssetDescription(description);
            
            mAssetGraph.RemoveNode(description);
        }

        public void AddAssetDependency(AssetDescription description, AssetDescription dependentDescription)
        {
            mAssetGraph.AddEdge(description, dependentDescription);
        }

        public void AddAssetDependencies(AssetDescription description, List<AssetDescription> dependentDescriptions)
        {
            foreach (var dependentDescription in dependentDescriptions)
            {
                mAssetGraph.AddEdge(description, dependentDescription);
            }
        }

        public void RemoveAssetDependency(AssetDescription description, AssetDescription dependentDescription)
        {
            mAssetGraph.RemoveEdge(description, dependentDescription);
        }

        public void RemoveAssetDependencies(AssetDescription description, List<AssetDescription> dependentDescriptions)
        {
            foreach (var dependentDescription in dependentDescriptions)
            {
                mAssetGraph.RemoveEdge(description, dependentDescription);
            }
        }

        public Asset CreateAsset(AssetDescription description)
        {
            return mAssetGraph.CreateAsset(description);
        }

        public Asset CreateIndependentAsset(AssetDescription description, SegmentRange<int> range)
        {
            return mAssetGraph.CreateIndependentAsset(description, range);
        }

        public Asset DestoryAsset(Asset asset)
        {
            return mAssetGraph.DestoryAsset(asset);
        }

        public Asset DestoryAssetIndependent(Asset asset)
        {
            return mAssetGraph.DestoryIndependentAsset(asset);
        }
    }
}
