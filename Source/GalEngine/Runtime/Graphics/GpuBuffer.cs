using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    using Debug = System.Diagnostics.Debug;

    public class GpuBuffer : GpuResource
    {
        public int ElementSize { get; }

        public GpuBuffer(
            int bufferSize,
            int elementSize,
            GpuDevice device, 
            GpuResourceInfo resourceInfo) : base(device, bufferSize, resourceInfo)
        {
            ElementSize = elementSize;

            mResource = new SharpDX.Direct3D11.Buffer(GpuDevice.Device,
                new SharpDX.Direct3D11.BufferDescription()
                {
                    BindFlags = GpuConvert.ToBindUsage(ResourceInfo.BindUsage),
                    CpuAccessFlags = GpuConvert.ToCpuAccessFlag(ResourceInfo.CpuAccessFlag),
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SizeInBytes = SizeInBytes,
                    StructureByteStride = ElementSize,
                    Usage = GpuConvert.ToHeapType(ResourceInfo.HeapType)
                });
            
            Debug.Assert(bufferSize % elementSize == 0);
        }

        public override void Update<T>(params T[] data)
        {
            Debug.Assert(System.Runtime.InteropServices.Marshal.SizeOf<T>() == ElementSize);
            Debug.Assert(data.Length == SizeInBytes / ElementSize);
            
            GpuDevice.ImmediateContext.UpdateSubresource(data, Resource);
        }

        public override void Update(params byte[] data)
        {
            Debug.Assert(data.Length == SizeInBytes);

            GpuDevice.ImmediateContext.UpdateSubresource(data, Resource);
        }
    }
}
