using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsAdapter
    {
        internal SharpDX.DXGI.Adapter Adapter { get; }

        public string Description { get; }

        private GraphicsAdapter(string description, SharpDX.DXGI.Adapter adapter)
        {
            Description = description;
            Adapter = adapter;
        }

        public static List<GraphicsAdapter> EnumerateGraphicsAdapter()
        {
            List<GraphicsAdapter> graphicsAdapters = new List<GraphicsAdapter>();

            GraphicsLogProvider.Log("[Start Enumerate GraphicsAdapter] [object]");

            using (var factory = new SharpDX.DXGI.Factory1())
            {
                foreach (var adapter in factory.Adapters)
                {
                    graphicsAdapters.Add(new GraphicsAdapter(adapter.Description.Description, adapter));

                    GraphicsLogProvider.Log("[Enumerate GraphicsAdapter] [{0}] [object]", LogLevel.Information, adapter.Description.Description);
                }
            }

            GraphicsLogProvider.Log("[End Enumerate GraphicsAdapter] [object]");

            return graphicsAdapters;
        }
    }
}
