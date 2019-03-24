using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum BlendOperation : uint
    {
        Add = 1,
        Subtract = 2,
        ReverseSubtract = 3,
        Minimum = 4,
        Maximum = 5
    }

    public enum BlendOption : uint
    {
        Zero = 1,
        One = 2,
        SourceColor = 3,
        InverseSourceColor = 4,
        SourceAlpha = 5,
        InverseSourceAlpha = 6,
        DestinationAlpha = 7,
        InverseDestinationAlpha = 8,
        DestinationColor = 9,
        InverseDestinationColor = 10,
        SourceAlphaSaturate = 11,
        BlendFactor = 14,
        InverseBlendFactor = 15,
        SecondarySourceColor = 16,
        InverseSecondarySourceColor = 17,
        SecondarySourceAlpha = 18,
        InverseSecondarySourceAlpha = 19
    }

    public class RenderTargetBlendDescription
    {
        public bool IsBlendEnable { get; set; }
        public BlendOption SourceBlend { get; set; }
        public BlendOption DestinationBlend { get; set; }
        public BlendOperation BlendOperation { get; set; }
        public BlendOption SourceAlphaBlend { get; set; }
        public BlendOption DestinationAlphaBlend { get; set; }
        public BlendOperation AlphaBlendOperation { get; set; }

        public RenderTargetBlendDescription()
        {
            IsBlendEnable = false;
            SourceBlend = BlendOption.One;
            DestinationBlend = BlendOption.Zero;
            BlendOperation = BlendOperation.Add;
            SourceAlphaBlend = BlendOption.One;
            DestinationAlphaBlend = BlendOption.Zero;
            AlphaBlendOperation = BlendOperation.Add;
        }
    }


    public class GpuBlendState : IDisposable
    {
        private SharpDX.Direct3D11.BlendState mBlendState;
        private SharpDX.Direct3D11.BlendStateDescription mDescription;

        protected GpuDevice GpuDevice { get; }

        internal SharpDX.Direct3D11.BlendState BlendState { get => mBlendState; }

        public GpuBlendState(GpuDevice device)
        {
            GpuDevice = device;

            mBlendState = new SharpDX.Direct3D11.BlendState(GpuDevice.Device,
                mDescription = SharpDX.Direct3D11.BlendStateDescription.Default());
        }

        public GpuBlendState(GpuDevice device, RenderTargetBlendDescription description)
        {
            GpuDevice = device;

            mDescription = SharpDX.Direct3D11.BlendStateDescription.Default();

            mDescription.RenderTarget[0] = new SharpDX.Direct3D11.RenderTargetBlendDescription()
            {
                IsBlendEnabled = description.IsBlendEnable,
                SourceBlend = GpuConvert.ToBlendOption(description.SourceBlend),
                DestinationBlend = GpuConvert.ToBlendOption(description.DestinationBlend),
                BlendOperation = GpuConvert.ToBlendOperation(description.BlendOperation),
                SourceAlphaBlend = GpuConvert.ToBlendOption(description.SourceAlphaBlend),
                DestinationAlphaBlend = GpuConvert.ToBlendOption(description.DestinationAlphaBlend),
                AlphaBlendOperation = GpuConvert.ToBlendOperation(description.AlphaBlendOperation),
                RenderTargetWriteMask = SharpDX.Direct3D11.ColorWriteMaskFlags.All
            };

            mBlendState = new SharpDX.Direct3D11.BlendState(GpuDevice.Device, mDescription);
        }

        ~GpuBlendState() => Dispose();

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mBlendState);
        }
    }
}
