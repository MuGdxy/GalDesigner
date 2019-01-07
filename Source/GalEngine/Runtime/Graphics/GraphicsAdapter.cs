using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsAdapter : IDisposable
    {
        private SharpDX.DXGI.Adapter mAdapter;

        internal SharpDX.DXGI.Adapter Adapter { get => mAdapter; }

        public string Description { get; }

        private GraphicsAdapter(string description, SharpDX.DXGI.Adapter adapter)
        {
            Description = description;

            mAdapter = adapter;
        }

        ~GraphicsAdapter() => Dispose();

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mAdapter);
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
