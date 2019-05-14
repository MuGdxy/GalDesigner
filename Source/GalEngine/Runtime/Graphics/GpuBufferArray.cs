using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GpuBufferArray : GpuResource
    {
        public int ElementSize { get; }
        public int ElementCount { get; }

        public GpuBufferArray(
            int elementSize,
            int elementCount,
            GpuDevice device,
            GpuResourceInfo resourceInfo) : base(device, elementCount * elementSize, resourceInfo)
        {
            ElementSize = elementSize;
            ElementCount = elementCount;

            mResource = new SharpDX.Direct3D11.Buffer(GpuDevice.Device,
                new SharpDX.Direct3D11.BufferDescription()
                {
                    BindFlags = GpuConvert.ToBindUsage(ResourceInfo.BindUsage),
                    CpuAccessFlags = GpuConvert.ToCpuAccessFlag(ResourceInfo.CpuAccessFlag),
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.BufferStructured,
                    SizeInBytes = SizeInBytes,
                    StructureByteStride = ElementSize,
                    Usage = GpuConvert.ToHeapType(ResourceInfo.HeapType)
                });
        }

        public override void Update<T>(params T[] data)
        {
            Utility.Assert(System.Runtime.InteropServices.Marshal.SizeOf<T>() == ElementSize);
            Utility.Assert(data.Length == ElementCount);

            GpuDevice.ImmediateContext.UpdateSubresource(data, Resource);
        }

        public override void Update(params byte[] data)
        {
            Utility.Assert(data.Length == SizeInBytes);

            GpuDevice.ImmediateContext.UpdateSubresource(data, Resource);
        }
    }
}
