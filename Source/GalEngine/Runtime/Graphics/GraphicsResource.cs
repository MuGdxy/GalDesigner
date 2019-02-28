using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum GraphicsResourceBindType
    {
        None = 0,
        VertexBufferr = 1,
        IndexBuffer = 2,
        ConstantBuffer = 4,
        ShaderResource = 8,
        StreamOutput = 16,
        RenderTarget = 32,
        DepthStencil = 64,
        UnorderedAccess = 128,
        Decoder = 512,
        VideoEncoder = 1024
    }

    public abstract class GraphicsResource : IDisposable
    {
        protected SharpDX.Direct3D11.Resource mResource;

        protected GraphicsDevice Device { get; }

        internal protected SharpDX.Direct3D11.Resource Resource { get => mResource; }

        public int Size { get; }
        public GraphicsResourceBindType BindType { get; }

        public GraphicsResource(GraphicsDevice device, int size, GraphicsResourceBindType bindType)
        {
            Size = size;
            BindType = bindType;
            Device = device;

            mResource = null;
        }

        ~GraphicsResource() => Dispose();

        public abstract void Update<T>(params T[] data) where T : struct;
        public abstract void Update(params byte[] data);

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mResource);
        }
    }
}
