using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.Structure;

namespace GalEngine
{
    using Debug = System.Diagnostics.Debug;
    
    class AssetGraph : Graph<Asset>
    {
        private GraphNode<Asset> LoadAsset(GraphNode<Asset> node)
        {
            if (node.Data.Reference != 0)
            {
                node.Data.Package.LoadAsset(node.Data.Name, null);

                return node;
            }

            List<Asset> dependentAssets = new List<Asset>();

            foreach (var next in node.Next)
            {
                dependentAssets.Add(LoadAsset(next).Data);
            }

            node.Data.Package.LoadAsset(node.Data.Name, dependentAssets);

            return node;
        }

        private void UnLoadAsset(GraphNode<Asset> node)
        {
            Debug.Assert(node.Data.Reference != 0);

            node.Data.Package.UnLoadAsset(node.Data.Name);

            if (node.Data.Reference != 0) return;

            foreach (var next in node.Next)
            {
                UnLoadAsset(next);
            }
        }

        public void LoadAsset(Asset asset)
        {
            LoadAsset(mNodes[asset]);
        }

        public void UnLoadAsset(Asset asset)
        {
            UnLoadAsset(mNodes[asset]);
        }
    }
}
