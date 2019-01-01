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

    public class GraphicsRasterizerState
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
    }
}
