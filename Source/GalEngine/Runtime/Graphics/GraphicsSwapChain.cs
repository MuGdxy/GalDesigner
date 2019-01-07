using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsSwapChain : IDisposable
    {
        internal SharpDX.DXGI.SwapChain mSwapChain;

        public Size<int> Size { get; }
        public PixelFormat PixelFormat { get; }
        public GraphicsRenderTarget RenderTarget { get; }

        public GraphicsSwapChain(GraphicsDevice device, IntPtr handle, Size<int> size, PixelFormat pixelFormat)
        {
            //size property
            Size = size;
            PixelFormat = pixelFormat;

            //get factory
            using (var factory = device.Adapter.Adapter.GetParent<SharpDX.DXGI.Factory>())
            {
                //set swapchain desc
                var swapChainDesc = new SharpDX.DXGI.SwapChainDescription()
                {
                    BufferCount = 1,
                    Flags = SharpDX.DXGI.SwapChainFlags.None,
                    IsWindowed = true,
                    ModeDescription = new SharpDX.DXGI.ModeDescription()
                    {
                        Format = GraphicsConvert.ToPixelFormat(PixelFormat),
                        Height = Size.Height,
                        Width = Size.Width,
                        RefreshRate = new SharpDX.DXGI.Rational(60, 1),
                        Scaling = SharpDX.DXGI.DisplayModeScaling.Unspecified,
                        ScanlineOrdering = SharpDX.DXGI.DisplayModeScanlineOrder.Unspecified
                    },
                    OutputHandle = handle,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
                    Usage = SharpDX.DXGI.Usage.RenderTargetOutput
                };

                mSwapChain = new SharpDX.DXGI.SwapChain(factory, device.Device, swapChainDesc);

                //report error, if create swapchain failed
                LogEmitter.Assert(mSwapChain != null, LogLevel.Error, 
                    "[Create SwapChain Failed] [Width = {0}] [Height = {1}] [Format = {2}]", Size.Width, Size.Height, PixelFormat);

                RenderTarget = new GraphicsRenderTarget(device, this);
            }
        }

        ~GraphicsSwapChain() => Dispose();
        
        public void Present(bool sync)
        {
            mSwapChain.Present(sync ? 1 : 0, SharpDX.DXGI.PresentFlags.None);
        }

        public void Dispose()
        {
            SharpDX.Utilities.Dispose(ref mSwapChain);
        }
    }
}
