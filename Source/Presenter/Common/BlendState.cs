using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class BlendState
    {
        private SharpDX.Direct3D11.BlendStateDescription blendState;
        private SharpDX.Direct3D11.RenderTargetBlendDescription rtvBlend;

        public BlendState(bool isEnable = false)
        {
            rtvBlend = new SharpDX.Direct3D11.RenderTargetBlendDescription()
            {
                IsBlendEnabled = isEnable,
                SourceBlend = SharpDX.Direct3D11.BlendOption.One,
                DestinationBlend = SharpDX.Direct3D11.BlendOption.Zero,
                BlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                SourceAlphaBlend = SharpDX.Direct3D11.BlendOption.One,
                DestinationAlphaBlend = SharpDX.Direct3D11.BlendOption.Zero,
                AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add,
                RenderTargetWriteMask = SharpDX.Direct3D11.ColorWriteMaskFlags.All
            };

            blendState = SharpDX.Direct3D11.BlendStateDescription.Default();

            blendState.RenderTarget[0] = rtvBlend;
        }

        public BlendOperation AlphaBlendOperation
        {
            get => (BlendOperation)rtvBlend.AlphaBlendOperation;
            set => rtvBlend.AlphaBlendOperation = (SharpDX.Direct3D11.BlendOperation)value;
        }

        public BlendOperation BlendOperation
        {
            get => (BlendOperation)rtvBlend.BlendOperation;
            set => rtvBlend.BlendOperation = (SharpDX.Direct3D11.BlendOperation)value;
        }

        public BlendOption DestinationAlphaBlend
        {
            get => (BlendOption)rtvBlend.DestinationAlphaBlend;
            set => rtvBlend.DestinationAlphaBlend = (SharpDX.Direct3D11.BlendOption)value;
        }

        public BlendOption DestinationBlend
        {
            get => (BlendOption)rtvBlend.DestinationBlend;
            set => rtvBlend.DestinationBlend = (SharpDX.Direct3D11.BlendOption)value;
        }

        public bool IsBlendEnabled
        {
            get => rtvBlend.IsBlendEnabled;
            set => rtvBlend.IsBlendEnabled = value;
        }

        public ColorMask RenderTargetWriteMask
        {
            get => (ColorMask)rtvBlend.RenderTargetWriteMask;
            set => rtvBlend.RenderTargetWriteMask = (SharpDX.Direct3D11.ColorWriteMaskFlags)value;
        }

        public BlendOption SourceAlphaBlend
        {
            get => (BlendOption)rtvBlend.SourceAlphaBlend;
            set => rtvBlend.SourceAlphaBlend = (SharpDX.Direct3D11.BlendOption)value;
        }

        public BlendOption SourceBlend
        {
            get => (BlendOption)rtvBlend.SourceBlend;
            set => rtvBlend.SourceBlend = (SharpDX.Direct3D11.BlendOption)value;
        }

        internal SharpDX.Direct3D11.BlendStateDescription ID3D11BlendStateDescription
        {
            get
            {
                blendState.RenderTarget[0] = rtvBlend;
                return blendState;
            }
        }
    }
}
