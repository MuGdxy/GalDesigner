using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    class AssetGraph : Graph<AssetDescription>
    {
        private List<Asset> SearchDependentAssets(GraphNode<AssetDescription> node)
        {
            List<Asset> dependentAssets = new List<Asset>();

            //get dependent assets
            foreach (var next in node.Next) dependentAssets.Add(CreateAsset(next.Data));

            return dependentAssets;
        }

        public Asset CreateAsset(AssetDescription description)
        {
            //if the reference is not 0, we can set dependent assets to null
            //because we do not need to load and create asset instance and we can save the performance
            //if "IsKeepDependentAssets" is true, we do not destory the dependent assets and keep them
            //else we will destory the dependent assets
            var dependentAssets = description.Reference == 0 ? SearchDependentAssets(mNodes[description]) : null;

            var result = description.Package.CreateAsset(description.Name, dependentAssets);

            //when we need keep the dependent assets or the reference is not 0
            //we do not need destory the dependent assets
            //becase we need to keep them until destory this asset or we had done it.
            if (description.IsKeepDependentAssets == true || dependentAssets == null) return result;

            foreach (var dependentAsset in dependentAssets) DestoryAsset(dependentAsset); return result;
        }

        public Asset CreateIndependentAsset(AssetDescription description, SegmentRange<int> range)
        {
            //for independent asset, we wiil search the dependent assets
            //and we will destory the dependent assets after we create independent asset
            var dependentAssets = SearchDependentAssets(mNodes[description]);

            var result = description.Package.CreateIndependentAsset(description.Name, range, dependentAssets);

            foreach (var dependentAsset in dependentAssets) DestoryAsset(dependentAsset); return result;
        }

        public Asset DestoryAsset(Asset asset)
        {
            //destory the asset and destory the dependent assets
            //when the "IsKeepDependentAssets" is true and reference is 0
            asset = asset.AssetDescription.Package.DestoryAsset(asset);

            //the condition of destory dependent assets is not true
            if (asset.AssetDescription.IsKeepDependentAssets == false ||
                asset.AssetDescription.Reference != 0) return asset;

            foreach (var dependentAsset in asset.AssetDescription.DependentAssets) DestoryAsset(dependentAsset);

            return asset;
        }

        public Asset DestoryIndependentAsset(Asset asset)
        {
            //because we had destoryed the dependent assets after we create the asset
            //we do not need to destory the dependent assets
            return asset.AssetDescription.DestoryIndependentAsset(asset);
        }
    }
}
