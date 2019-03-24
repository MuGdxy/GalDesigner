using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    class PresentRender
    {
        private readonly GpuDevice mDevice;

        private readonly GpuBlendState mBlendState;
        private readonly GpuPixelShader mPixelShader;
        private readonly GpuVertexShader mVertexShader;
        private readonly GpuInputLayout mInputLayout;

        private readonly GpuBuffer mVertexBuffer;
        private readonly GpuBuffer mIndexBuffer;

        private readonly GpuBuffer mRenderConfigBuffer;

        private readonly int mRenderConfigBufferSlot;

        public PresentRender(GpuDevice device)
        {
            mDevice = device;

            mBlendState = new GpuBlendState(mDevice, new RenderTargetBlendDescription()
            {
                AlphaBlendOperation = BlendOperation.Add,
                BlendOperation = BlendOperation.Add,
                DestinationAlphaBlend = BlendOption.InverseSourceAlpha,
                DestinationBlend = BlendOption.InverseSourceAlpha,
                SourceAlphaBlend = BlendOption.SourceAlpha,
                SourceBlend = BlendOption.SourceAlpha,
                IsBlendEnable = true
            });

            
        }
    }
}
