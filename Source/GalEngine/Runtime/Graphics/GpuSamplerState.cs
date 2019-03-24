using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum TextureAddressMode : uint
    {
        Wrap = 1,
        Mirror = 2,
        Clamp = 3,
        Border = 4,
        MirrorOnce = 5
    }

    public enum TextureFilter : uint
    {
        MinMagMipPoint = 0,
        MinMagPointMipLinear = 1,
        MinPointMagLinearMipPoint = 4,
        MinPointMagMipLinear = 5,
        MinLinearMagMipPoint = 16,
        MinLinearMagPointMipLinear = 17,
        MinMagLinearMipPoint = 20,
        MinMagMipLinear = 21,
        Anisotropic = 85,
        ComparisonMinMagMipPoint = 128,
        ComparisonMinMagPointMipLinear = 129,
        ComparisonMinPointMagLinearMipPoint = 132,
        ComparisonMinPointMagMipLinear = 133,
        ComparisonMinLinearMagMipPoint = 144,
        ComparisonMinLinearMagPointMipLinear = 145,
        ComparisonMinMagLinearMipPoint = 148,
        ComparisonMinMagMipLinear = 149,
        ComparisonAnisotropic = 213,
        MinimumMinMagMipPoint = 256,
        MinimumMinMagPointMipLinear = 257,
        MinimumMinPointMagLinearMipPoint = 260,
        MinimumMinPointMagMipLinear = 261,
        MinimumMinLinearMagMipPoint = 272,
        MinimumMinLinearMagPointMipLinear = 273,
        MinimumMinMagLinearMipPoint = 276,
        MinimumMinMagMipLinear = 277,
        MinimumAnisotropic = 341,
        MaximumMinMagMipPoint = 384,
        MaximumMinMagPointMipLinear = 385,
        MaximumMinPointMagLinearMipPoint = 388,
        MaximumMinPointMagMipLinear = 389,
        MaximumMinLinearMagMipPoint = 400,
        MaximumMinLinearMagPointMipLinear = 401,
        MaximumMinMagLinearMipPoint = 404,
        MaximumMinMagMipLinear = 405,
        MaximumAnisotropic = 469
    }

    public class GpuSamplerState : IDisposable
    {
        private SharpDX.Direct3D11.SamplerState mSamplerState;

        protected GpuDevice GpuDevice { get; }

        internal SharpDX.Direct3D11.SamplerState SamplerState => mSamplerState;

        public TextureAddressMode AddressU { get; }
        public TextureAddressMode AddressV { get; }
        public TextureAddressMode AddressW { get; }

        public TextureFilter Filter { get; }

        public GpuSamplerState(GpuDevice device, 
            TextureAddressMode addressU,
            TextureAddressMode addressV, 
            TextureAddressMode addressW, 
            TextureFilter filter = TextureFilter.MinMagMipLinear)
        {
            GpuDevice = device;

            //set address mode 
            AddressU = addressU;
            AddressV = addressV;
            AddressW = addressW;

            Filter = filter;

            mSamplerState = new SharpDX.Direct3D11.SamplerState(GpuDevice.Device, new SharpDX.Direct3D11.SamplerStateDescription()
            {
                AddressU = GpuConvert.ToTextureAddressMode(AddressU),
                AddressV = GpuConvert.ToTextureAddressMode(AddressV),
                AddressW = GpuConvert.ToTextureAddressMode(AddressW),
                Filter = GpuConvert.ToTextureFilter(Filter),
                ComparisonFunction = SharpDX.Direct3D11.Comparison.Never,
                MinimumLod = float.MinValue,
                MaximumLod = float.MaxValue,
                BorderColor = new SharpDX.Mathematics.Interop.RawColor4(1, 1, 1, 1),
                MaximumAnisotropy = 4,
                MipLodBias = 0.0f
            });
        }

        public GpuSamplerState(GpuDevice device,
            TextureAddressMode addressUVW,
            TextureFilter filter) : this(device, addressUVW, addressUVW, addressUVW, filter)
        {

        }

        ~GpuSamplerState() => Dispose();

        public void Dispose()
        {
            SharpDX.Utilities.Dispose(ref mSamplerState);
        }
    }
}
