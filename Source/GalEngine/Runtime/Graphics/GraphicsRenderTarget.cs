using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsRenderTarget
    {
        internal SharpDX.Direct3D11.RenderTargetView RenderTarget { get; }

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

            RenderTarget = new SharpDX.Direct3D11.RenderTargetView(device.Device, backTexture, renderTargetDesc);
        } 
    }
}
