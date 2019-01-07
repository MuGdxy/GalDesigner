using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsShaderResource : IDisposable
    {
        private SharpDX.Direct3D11.ShaderResourceView mShaderResource;

        internal SharpDX.Direct3D11.ShaderResourceView ShaderResource { get => mShaderResource; }

        public GraphicsShaderResource(GraphicsDevice device, GraphicsTexture texture)
        {
            var shaderResourceDesc = new SharpDX.Direct3D11.ShaderResourceViewDescription()
            {
                Format = GraphicsConvert.ToPixelFormat(texture.PixelFormat),
                Texture2D = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture2DResource()
                {
                    MipLevels = 1,
                    MostDetailedMip = 0
                },
                Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2D
            };

            mShaderResource = new SharpDX.Direct3D11.ShaderResourceView(device.Device, texture.Resource, shaderResourceDesc);
        }

        ~GraphicsShaderResource() => Dispose();

        public void Dispose()
        {
            SharpDX.Utilities.Dispose(ref mShaderResource);
        }
    }
}
