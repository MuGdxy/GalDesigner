using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum GpuFillMode : uint
    {
        Wireframe = 2,
        Solid = 3
    }

    public enum GpuCullMode : uint
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

        public GpuFillMode FillMode { get; }
        public GpuCullMode CullMode { get; }

        public GpuRasterizerState(GpuDevice device)
        {
            GpuDevice = device;

            mRasterizerState = new SharpDX.Direct3D11.RasterizerState(GpuDevice.Device,
                mDescription = SharpDX.Direct3D11.RasterizerStateDescription.Default());

            FillMode = (GpuFillMode)mDescription.FillMode;
            CullMode = (GpuCullMode)mDescription.CullMode;
        }

        public GpuRasterizerState(GpuDevice device, GpuFillMode fillMode, GpuCullMode cullMode)
        {
            GpuDevice = device;

            FillMode = fillMode;
            CullMode = cullMode;

            mDescription = SharpDX.Direct3D11.RasterizerStateDescription.Default();

            mDescription.FillMode = (SharpDX.Direct3D11.FillMode)FillMode;
            mDescription.CullMode = (SharpDX.Direct3D11.CullMode)CullMode;

            mRasterizerState = new SharpDX.Direct3D11.RasterizerState(GpuDevice.Device, mDescription);
        }

        ~GpuRasterizerState() => Dispose();

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mRasterizerState);
        }
    }
}
