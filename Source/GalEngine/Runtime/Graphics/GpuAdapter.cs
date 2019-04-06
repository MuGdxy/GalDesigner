using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GpuAdapter : IDisposable
    {
        private SharpDX.DXGI.Adapter mAdapter;

        internal SharpDX.DXGI.Adapter Adapter => mAdapter;

        public string Description { get; }

        private GpuAdapter(string description, SharpDX.DXGI.Adapter adapter)
        {
            Description = description;

            mAdapter = adapter;
        }

        ~GpuAdapter() => Dispose();

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mAdapter);
        }

        public static List<GpuAdapter> EnumerateGraphicsAdapter()
        {
            List<GpuAdapter> graphicsAdapters = new List<GpuAdapter>();

            LogEmitter.Apply(LogLevel.Information, "[Start Enumerate GraphicsAdapter]");

            using (var factory = new SharpDX.DXGI.Factory1())
            {
                foreach (var adapter in factory.Adapters)
                {
                    graphicsAdapters.Add(new GpuAdapter(adapter.Description.Description, adapter));

                    LogEmitter.Apply(LogLevel.Information, "[Enumerate GraphicsAdapter] [{0}]", adapter.Description.Description);
                }
            }

            LogEmitter.Apply(LogLevel.Information, "[End Enumerate GraphicsAdapter]");

            return graphicsAdapters;
        }
    }
}
