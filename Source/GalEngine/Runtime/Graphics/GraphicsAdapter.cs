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

            LogEmitter.Apply(LogLevel.Information, "[Start Enumerate GraphicsAdapter]");

            using (var factory = new SharpDX.DXGI.Factory1())
            {
                foreach (var adapter in factory.Adapters)
                {
                    graphicsAdapters.Add(new GraphicsAdapter(adapter.Description.Description, adapter));

                    LogEmitter.Apply(LogLevel.Information, "[Enumerate GraphicsAdapter] [{0}]", LogLevel.Information, adapter.Description.Description);
                }
            }

            LogEmitter.Apply(LogLevel.Information, "[End Enumerate GraphicsAdapter]");

            return graphicsAdapters;
        }
    }
}
