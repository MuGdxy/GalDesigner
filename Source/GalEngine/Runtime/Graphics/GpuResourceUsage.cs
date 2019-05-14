using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    using Debug = System.Diagnostics.Debug;

    public class GpuResourceUsage : IDisposable
    {
        private SharpDX.Direct3D11.ShaderResourceView mShaderResource;

        protected GpuDevice GpuDevice { get; }

        internal SharpDX.Direct3D11.ShaderResourceView ShaderResource { get => mShaderResource; }

        public GpuResourceUsage(GpuDevice device, GpuTexture2D texture)
        {
            Debug.Assert(GpuConvert.HasBindUsage(texture.ResourceInfo.BindUsage, GpuBindUsage.ShaderResource) == true);

            GpuDevice = device;

            var shaderResourceDesc = new SharpDX.Direct3D11.ShaderResourceViewDescription()
            {
                Format = GpuConvert.ToPixelFormat(texture.PixelFormat),
                Texture2D = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture2DResource()
                {
                    MipLevels = 1,
                    MostDetailedMip = 0
                },
                Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2D
            };

            mShaderResource = new SharpDX.Direct3D11.ShaderResourceView(GpuDevice.Device, texture.Resource, shaderResourceDesc);
        }

        public GpuResourceUsage(GpuDevice device, GpuBufferArray bufferArray)
        {
            Utility.Assert(GpuConvert.HasBindUsage(bufferArray.ResourceInfo.BindUsage, GpuBindUsage.ShaderResource) == true);

            GpuDevice = device;

            var shaderResourceDesc = new SharpDX.Direct3D11.ShaderResourceViewDescription()
            {
                Format = SharpDX.DXGI.Format.Unknown,
                Buffer = new SharpDX.Direct3D11.ShaderResourceViewDescription.BufferResource()
                {
                    FirstElement = 0,
                    ElementCount = bufferArray.ElementCount
                },
                Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Buffer
            };

            mShaderResource = new SharpDX.Direct3D11.ShaderResourceView(GpuDevice.Device, bufferArray.Resource, shaderResourceDesc);
        }

        ~GpuResourceUsage() => Dispose();

        public void Dispose()
        {
            SharpDX.Utilities.Dispose(ref mShaderResource);
        }
    }
}
