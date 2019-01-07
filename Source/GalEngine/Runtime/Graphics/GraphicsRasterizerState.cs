using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum FillMode
    {
        Wireframe = 2,
        Solid = 3
    }

    public enum CullMode
    {
        None = 1,
        Front = 2,
        Back = 3
    }

    public class GraphicsRasterizerState : IDisposable
    {
        private GraphicsDevice mDevice;

        private SharpDX.Direct3D11.RasterizerState mRasterizerState;
        private SharpDX.Direct3D11.RasterizerStateDescription mDescription;

        internal SharpDX.Direct3D11.RasterizerState RasterizerState { get => mRasterizerState; }

        public FillMode FillMode { get; private set; }
        public CullMode CullMode { get; private set; }

        public GraphicsRasterizerState(GraphicsDevice device)
        {
            mDevice = device;

            mRasterizerState = new SharpDX.Direct3D11.RasterizerState(mDevice.Device,
                mDescription = SharpDX.Direct3D11.RasterizerStateDescription.Default());
        }

        ~GraphicsRasterizerState() => Dispose();

        public void SetFillMode(FillMode fillMode)
        {
            //note: each change we need to create rasterizer state again
            mDescription.FillMode = GraphicsConvert.ToFillMode(FillMode = fillMode);

            SharpDX.Utilities.Dispose(ref mRasterizerState);

            mRasterizerState = new SharpDX.Direct3D11.RasterizerState(mDevice.Device, mDescription);
        }

        public void SetCullMode(CullMode cullMode)
        {
            //note: each change we need to create rasterizer state again
            mDescription.CullMode = GraphicsConvert.ToCullMode(CullMode = cullMode);

            SharpDX.Utilities.Dispose(ref mRasterizerState);

            mRasterizerState = new SharpDX.Direct3D11.RasterizerState(mDevice.Device, mDescription);
        }

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mRasterizerState);
        }
    }
}
