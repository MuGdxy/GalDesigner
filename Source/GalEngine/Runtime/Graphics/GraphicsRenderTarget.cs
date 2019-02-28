using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsRenderTarget : IDisposable
    {
        private SharpDX.Direct3D11.RenderTargetView mRenderTarget;

        internal SharpDX.Direct3D11.RenderTargetView RenderTarget { get => mRenderTarget; }

        public Size<int> Size { get; private set; }
        
        public GraphicsRenderTarget(GraphicsDevice device, GraphicsSwapChain swapChain)
        {
            //get back buffer and set render target desc
            var backTexture = swapChain.mSwapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0);
            var renderTargetDesc = new SharpDX.Direct3D11.RenderTargetViewDescription()
            {
                 Format = GraphicsConvert.ToPixelFormat(swapChain.PixelFormat),
                 Texture2D = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DResource()
                 {
                     MipSlice = 0
                 },
                 Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2D
            };

            mRenderTarget = new SharpDX.Direct3D11.RenderTargetView(device.Device, backTexture, renderTargetDesc);

            //set size
            Size = swapChain.Size;
        }

        public GraphicsRenderTarget(GraphicsDevice device, GraphicsTexture texture)
        {
            //set render target desc
            var renderTargetDesc = new SharpDX.Direct3D11.RenderTargetViewDescription()
            {
                Format = GraphicsConvert.ToPixelFormat(texture.PixelFormat),
                Texture2D = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DResource()
                {
                    MipSlice = 0,
                },
                Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2D
            };

            mRenderTarget = new SharpDX.Direct3D11.RenderTargetView(device.Device, texture.Resource, renderTargetDesc);

            //set size
            Size = new Size<int>(texture.Width, texture.Height);
        }

        ~GraphicsRenderTarget() => Dispose();

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mRenderTarget);
        }
    }
}
