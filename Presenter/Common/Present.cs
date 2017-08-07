using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public class Present
    {
        private SharpDX.Direct3D11.RenderTargetView renderTargetView;
        private SharpDX.Direct3D11.DepthStencilView depthStencilView;

        private SharpDX.Direct3D11.Texture2D renderTarget;
        private SharpDX.Direct3D11.Texture2D depthStencil;

        private int width;
        private int height;

        private SharpDX.DXGI.SwapChain swapChain;

        private IntPtr handle;

        private void CreateResource(ref SharpDX.Direct3D11.Texture2D resource,
            SharpDX.Direct3D11.BindFlags bindFlags, SharpDX.DXGI.Format format)
        {
            SharpDX.Utilities.Dispose(ref resource);

            resource = new SharpDX.Direct3D11.Texture2D(Engine.ID3D11Device,
                new SharpDX.Direct3D11.Texture2DDescription()
                {
                    ArraySize = 1,
                    BindFlags = bindFlags,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = format,
                    Height = height,
                    MipLevels = 1,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    Width = width
                });
        }

        private void CreateResourceView()
        {
            renderTargetView = new SharpDX.Direct3D11.RenderTargetView(Engine.ID3D11Device, renderTarget,
                new SharpDX.Direct3D11.RenderTargetViewDescription()
                {
                    Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2D,
                    Format = RenderTargetFormat,
                    Texture2D = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DResource()
                    {
                        MipSlice = 0
                    }
                });

            depthStencilView = new SharpDX.Direct3D11.DepthStencilView(Engine.ID3D11Device, depthStencil,
                new SharpDX.Direct3D11.DepthStencilViewDescription()
                {
                    Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D,
                    Format = DepthStencilFormat,
                    Flags = SharpDX.Direct3D11.DepthStencilViewFlags.None,
                    Texture2D = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture2DResource()
                    {
                        MipSlice = 0
                    }
                });
        }

        internal void ResetViewPort()
        {
            Engine.ImmediateContext.Rasterizer.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Height = height,
                Width = width,
                MaxDepth = 1.0f,
                MinDepth = 0.0f,
                X = 0f,
                Y = 0f
            });

            Engine.ImmediateContext.Rasterizer.SetScissorRectangle(0, 0, width, height);
        }

        internal void ResetResourceView()
        {
            Engine.ImmediateContext.OutputMerger.SetTargets(depthStencilView, renderTargetView);

            Engine.ImmediateContext.ClearRenderTargetView(renderTargetView,
                new SharpDX.Mathematics.Interop.RawColor4(1, 1, 1, 1));

            Engine.ImmediateContext.ClearDepthStencilView(depthStencilView,
                 SharpDX.Direct3D11.DepthStencilClearFlags.Depth | SharpDX.Direct3D11.DepthStencilClearFlags.Stencil, 1f, 0);
        }

        internal void ClearState()
        {

        }

        public Present(IntPtr Handle, bool Windowed = true)
        {
            handle = Handle;

            APILibrary.Win32.Rect realRect = new APILibrary.Win32.Rect();

            APILibrary.Win32.Internal.GetClientRect(handle, ref realRect);

            SharpDX.DXGI.Device dxgidevice = Engine.ID3D11Device.QueryInterface<SharpDX.DXGI.Device>();
            SharpDX.DXGI.Adapter dxgiadapte = dxgidevice.GetParent<SharpDX.DXGI.Adapter>();
            SharpDX.DXGI.Factory dxgifactory = dxgiadapte.GetParent<SharpDX.DXGI.Factory>();

            swapChain = new SharpDX.DXGI.SwapChain(dxgifactory, Engine.ID3D11Device,
                new SharpDX.DXGI.SwapChainDescription()
                {
                    ModeDescription = new SharpDX.DXGI.ModeDescription()
                    {
                        Width = width = realRect.right - realRect.left,
                        Height = height = realRect.bottom - realRect.top,
                        RefreshRate = new SharpDX.DXGI.Rational(60, 1),
                        Scaling = SharpDX.DXGI.DisplayModeScaling.Unspecified,
                        ScanlineOrdering = SharpDX.DXGI.DisplayModeScanlineOrder.Unspecified,
                        Format = RenderTargetFormat,
                    },
                    BufferCount = 1,
                    Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                    Flags = SharpDX.DXGI.SwapChainFlags.AllowModeSwitch,
                    OutputHandle = handle,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
                    IsWindowed = Windowed
                });

            renderTarget = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0);

            CreateResource(ref depthStencil, SharpDX.Direct3D11.BindFlags.DepthStencil, DepthStencilFormat);

            CreateResourceView();

            SharpDX.Utilities.Dispose(ref dxgidevice);
            SharpDX.Utilities.Dispose(ref dxgiadapte);
            SharpDX.Utilities.Dispose(ref dxgifactory);
        }

        public void SwapBuffer()
        {
            swapChain.Present(0, SharpDX.DXGI.PresentFlags.None);

            ResetResourceView();
        }

        internal SharpDX.Direct3D11.Texture2D RenderTarget => renderTarget;
        internal SharpDX.Direct3D11.Texture2D DepthStencil => depthStencil;

        internal static SharpDX.DXGI.Format DepthStencilFormat => SharpDX.DXGI.Format.D24_UNorm_S8_UInt;
        internal static SharpDX.DXGI.Format RenderTargetFormat => SharpDX.DXGI.Format.R8G8B8A8_UNorm;

        public int Width => width;
        public int Height => height;

        public IntPtr Handle => handle;

        ~Present()
        {
            SharpDX.Utilities.Dispose(ref renderTarget);
            SharpDX.Utilities.Dispose(ref depthStencil);
            SharpDX.Utilities.Dispose(ref renderTargetView);
            SharpDX.Utilities.Dispose(ref depthStencilView);
            SharpDX.Utilities.Dispose(ref swapChain);
        }
    }


    public static partial class GraphicsPipeline
    {
        private static Present present;
    }
}
