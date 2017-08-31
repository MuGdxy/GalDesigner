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
        private static bool EnableDepth = false;

        private SharpDX.Direct3D11.RenderTargetView renderTargetView;
        private SharpDX.Direct3D11.DepthStencilView depthStencilView;

        private SharpDX.Direct3D11.Texture2D renderTarget;
        private SharpDX.Direct3D11.Texture2D depthStencil;

        private SharpDX.Direct2D1.Bitmap1 canvasTarget;

        private int width;
        private int height;

        private SharpDX.DXGI.SwapChain swapChain;

        private IntPtr handle;

        private void CreateCanvasTarget()
        {
            using (var surface = renderTarget.QueryInterface<SharpDX.DXGI.Surface>())
            {
                canvasTarget = new SharpDX.Direct2D1.Bitmap1(Canvas.ID2D1DeviceContext, surface);
            }
        }

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

            if (EnableDepth is false) return;

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
        }

        internal void ResetResourceView()
        {
            if (EnableDepth is true)
                Engine.ImmediateContext.OutputMerger.SetTargets(depthStencilView, renderTargetView);
            else Engine.ImmediateContext.OutputMerger.SetTargets(renderTargetView);

            Engine.ImmediateContext.ClearRenderTargetView(renderTargetView,
                new SharpDX.Mathematics.Interop.RawColor4(0, 0, 0, 0));

            if (EnableDepth is false) return;

            Engine.ImmediateContext.ClearDepthStencilView(depthStencilView,
                 SharpDX.Direct3D11.DepthStencilClearFlags.Depth | SharpDX.Direct3D11.DepthStencilClearFlags.Stencil, 1f, 0);
        }

        internal void ClearState()
        {

        }

        public Present(IntPtr Handle, int bufferWidth, int bufferHeight)
        {
            handle = Handle;

            SharpDX.DXGI.Device dxgidevice = Engine.ID3D11Device.QueryInterface<SharpDX.DXGI.Device>();
            SharpDX.DXGI.Adapter dxgiadapte = dxgidevice.GetParent<SharpDX.DXGI.Adapter>();
            SharpDX.DXGI.Factory dxgifactory = dxgiadapte.GetParent<SharpDX.DXGI.Factory>();

            swapChain = new SharpDX.DXGI.SwapChain(dxgifactory, Engine.ID3D11Device,
                new SharpDX.DXGI.SwapChainDescription()
                {
                    ModeDescription = new SharpDX.DXGI.ModeDescription()
                    {
                        Width = width = bufferWidth,
                        Height = height = bufferHeight,
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
                    IsWindowed = true
                });

            renderTarget = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0);

            if (EnableDepth is true)
                CreateResource(ref depthStencil, SharpDX.Direct3D11.BindFlags.DepthStencil, DepthStencilFormat);

            CreateResourceView();

            CreateCanvasTarget();

            SharpDX.Utilities.Dispose(ref dxgidevice);
            SharpDX.Utilities.Dispose(ref dxgiadapte);
            SharpDX.Utilities.Dispose(ref dxgifactory);
        }

        public void ResizeBuffer(int newWidth,int newHeight)
        {
            SharpDX.Utilities.Dispose(ref renderTarget);
            SharpDX.Utilities.Dispose(ref depthStencil);
            SharpDX.Utilities.Dispose(ref renderTargetView);
            SharpDX.Utilities.Dispose(ref depthStencilView);
            SharpDX.Utilities.Dispose(ref canvasTarget);

            swapChain.ResizeBuffers(1, newWidth, newHeight, RenderTargetFormat, SharpDX.DXGI.SwapChainFlags.AllowModeSwitch);

            renderTarget = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0);

            if (EnableDepth is true)
                CreateResource(ref depthStencil, SharpDX.Direct3D11.BindFlags.DepthStencil, DepthStencilFormat);

            CreateResourceView();
            CreateCanvasTarget();
        }

        public void SwapBuffer()
        {
            swapChain.Present(1, SharpDX.DXGI.PresentFlags.None);

            ResetResourceView();
        }

        public void CopyFromTexture(Texture2D texture)
        {
            Engine.ImmediateContext.CopyResource(texture.ID3D11Resource,
                renderTarget);
        }

        internal SharpDX.Direct3D11.Texture2D RenderTarget => renderTarget;
        internal SharpDX.Direct3D11.Texture2D DepthStencil => depthStencil;

        internal SharpDX.Direct3D11.RenderTargetView RenderTargetView => renderTargetView;
        internal SharpDX.Direct3D11.DepthStencilView DepthStencilView => depthStencilView;

        internal SharpDX.Direct2D1.Bitmap1 CanvasTarget => canvasTarget;

        internal static SharpDX.DXGI.Format DepthStencilFormat => SharpDX.DXGI.Format.D24_UNorm_S8_UInt;
        internal static SharpDX.DXGI.Format RenderTargetFormat => SharpDX.DXGI.Format.R8G8B8A8_UNorm;

        public int Width => width;
        public int Height => height;

        public IntPtr Handle => handle;

        public bool IsFullScreen
        {
            set => swapChain.IsFullScreen = value;
            get => swapChain.IsFullScreen;
        }

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
