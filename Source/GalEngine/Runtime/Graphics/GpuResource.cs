using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public abstract class GpuResource : IDisposable
    {
        protected SharpDX.Direct3D11.Resource mResource;

        protected GpuDevice GpuDevice { get; }

        internal protected SharpDX.Direct3D11.Resource Resource { get => mResource; }

        public int SizeInBytes { get; }
        public GpuResourceInfo ResourceInfo { get; }

        public GpuResource(GpuDevice device, int size, GpuResourceInfo resourceInfo)
        {
            SizeInBytes = size;
            ResourceInfo = resourceInfo;
            GpuDevice = device;

            mResource = null;
        }

        ~GpuResource() => Dispose();

        public abstract void Update<T>(params T[] data) where T : struct;
        public abstract void Update(params byte[] data);

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mResource);
        }
    }
}
