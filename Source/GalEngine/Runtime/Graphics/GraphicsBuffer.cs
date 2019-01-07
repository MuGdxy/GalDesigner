using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    using Debug = System.Diagnostics.Debug;

    public class GraphicsBuffer : GraphicsResource
    {
        public int ElementSize { get; }

        public GraphicsBuffer(GraphicsDevice device, int size, int elementSize, GraphicsResourceBindType bindType) : base(device, size, bindType)
        {
            ElementSize = elementSize;

            mResource = new SharpDX.Direct3D11.Buffer(Device.Device,
                new SharpDX.Direct3D11.BufferDescription()
                {
                    BindFlags = GraphicsConvert.ToBindFlags(BindType),
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SizeInBytes = Size,
                    StructureByteStride = ElementSize,
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default
                });

            Debug.Assert(size % elementSize == 0);
        }

        public override void Update<T>(T[] data)
        {
            Debug.Assert(System.Runtime.InteropServices.Marshal.SizeOf<T>() == ElementSize);
            Debug.Assert(data.Length == Size / ElementSize);
            
            Device.ImmediateContext.UpdateSubresource(data, Resource);
        }

        public override void Update(byte[] data)
        {
            Debug.Assert(data.Length == Size);

            Device.ImmediateContext.UpdateSubresource(data, Resource);
        }
    }
}
