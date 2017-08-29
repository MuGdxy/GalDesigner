using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public class TextureFace : Texture2D
    {
        private SharpDX.Direct3D11.Texture2D depthStencil;

        private SharpDX.Direct3D11.RenderTargetView renderTargetView;
        private SharpDX.Direct3D11.DepthStencilView depthStencilView;

        private SharpDX.Direct2D1.Bitmap1 canvasTarget;

        private bool enableDepth;

        private void CreateResourceView()
        {
            renderTargetView = new SharpDX.Direct3D11.RenderTargetView(Engine.ID3D11Device, ID3D11Resource,
                new SharpDX.Direct3D11.RenderTargetViewDescription()
                {
                    Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2D,
                    Format = Present.RenderTargetFormat,
                    Texture2D = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DResource()
                    {
                        MipSlice = 0
                    }
                });

            if (enableDepth is true)
            {
                depthStencilView = new SharpDX.Direct3D11.DepthStencilView(Engine.ID3D11Device, depthStencil,
                    new SharpDX.Direct3D11.DepthStencilViewDescription()
                    {
                        Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D,
                        Format = Present.DepthStencilFormat,
                        Flags = SharpDX.Direct3D11.DepthStencilViewFlags.None,
                        Texture2D = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture2DResource()
                        {
                            MipSlice = 0
                        }
                    });
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
                    Height = Height,
                    MipLevels = 1,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                    Width = Width
                });
        }

        private void CreateCanvasTarget()
        {
            using (var surface = ID3D11Resource.QueryInterface<SharpDX.DXGI.Surface>())
            {
                canvasTarget = new SharpDX.Direct2D1.Bitmap1(Canvas.ID2D1DeviceContext, surface);
            }
        }

        internal void ResetViewPort()
        {
            Engine.ImmediateContext.Rasterizer.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF()
            {
                Height = Height,
                Width = Width,
                MaxDepth = 1.0f,
                MinDepth = 0.0f,
                X = 0f,
                Y = 0f
            });
        }

        internal void ResetResourceView()
        {
            if (enableDepth is true)
            {
                Engine.ImmediateContext.OutputMerger.SetTargets(depthStencilView, renderTargetView);

                Engine.ImmediateContext.ClearRenderTargetView(renderTargetView,
                    new SharpDX.Mathematics.Interop.RawColor4(1, 1, 1, 1));

                Engine.ImmediateContext.ClearDepthStencilView(depthStencilView,
                     SharpDX.Direct3D11.DepthStencilClearFlags.Depth | SharpDX.Direct3D11.DepthStencilClearFlags.Stencil, 1f, 0);
            }
            else
            {
                Engine.ImmediateContext.OutputMerger.SetTargets(renderTargetView);

                Engine.ImmediateContext.ClearRenderTargetView(renderTargetView,
                    new SharpDX.Mathematics.Interop.RawColor4(1, 1, 1, 1));
            }
        }

        internal void ClearState()
        {

        }

        private TextureFace(Texture2D texture, bool enableDepthBuffer = false) : base(texture)
        {
            enableDepth = enableDepthBuffer;

            if (enableDepth is true)
            {
                CreateResource(ref depthStencil, SharpDX.Direct3D11.BindFlags.DepthStencil,
                    Present.DepthStencilFormat);
            }

            CreateResourceView();
            CreateCanvasTarget();
        }
        
        public TextureFace(int width, int height, bool enableDepthBuffer = false) 
            : base(width, height, (ResourceFormat)Present.RenderTargetFormat, 1)
        {
            enableDepth = enableDepthBuffer;

            if (enableDepth is true)
            {
                CreateResource(ref depthStencil, SharpDX.Direct3D11.BindFlags.DepthStencil,
                    Present.DepthStencilFormat);
            }

            CreateResourceView();
            CreateCanvasTarget();
        }

        public void ResetBuffer()
        {
            ResetResourceView();
        }

        public static TextureFace FromTexture2D(Texture2D texture, bool enableDepthBuffer = false)
        {
            return new TextureFace(texture, enableDepthBuffer);
        }

        internal SharpDX.Direct2D1.Bitmap1 CanvasTarget => canvasTarget;

        public bool EnableDepthTest => enableDepth;

        public override void Dispose()
        {
            SharpDX.Utilities.Dispose(ref depthStencil);
            SharpDX.Utilities.Dispose(ref canvasTarget);

            SharpDX.Utilities.Dispose(ref renderTargetView);
            SharpDX.Utilities.Dispose(ref depthStencilView);
            base.Dispose();
        }

        ~TextureFace()
        {
            SharpDX.Utilities.Dispose(ref depthStencil);
            SharpDX.Utilities.Dispose(ref canvasTarget);

            SharpDX.Utilities.Dispose(ref renderTargetView);
            SharpDX.Utilities.Dispose(ref depthStencilView);
        }
    }

    public static partial class GraphicsPipeline
    {
        private static TextureFace textureFace;
    }
}
