using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum FillMode : uint
    {
        Wireframe = 2,
        Solid = 3
    }

    public enum CullMode : uint
    {
        None = 1,
        Front = 2,
        Back = 3
    }

    public class GpuRasterizerState : IDisposable
    {
        private SharpDX.Direct3D11.RasterizerState mRasterizerState;
        private SharpDX.Direct3D11.RasterizerStateDescription mDescription;

        protected GpuDevice GpuDevice { get; }

        internal SharpDX.Direct3D11.RasterizerState RasterizerState { get => mRasterizerState; }

        public FillMode FillMode { get; }
        public CullMode CullMode { get; }

        public GpuRasterizerState(GpuDevice device)
        {
            GpuDevice = device;

            mRasterizerState = new SharpDX.Direct3D11.RasterizerState(GpuDevice.Device,
                mDescription = SharpDX.Direct3D11.RasterizerStateDescription.Default());
        }

        ~GpuRasterizerState() => Dispose();

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mRasterizerState);
        }
    }
}
