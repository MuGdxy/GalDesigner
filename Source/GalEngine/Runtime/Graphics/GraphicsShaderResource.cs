using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsShaderResource
    {
        internal SharpDX.Direct3D11.ShaderResourceView ShaderResource { get; }

        public GraphicsShaderResource(GraphicsDevice device,GraphicsTexture texture)
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

            ShaderResource = new SharpDX.Direct3D11.ShaderResourceView(device.Device, texture.Resource, shaderResourceDesc);
        }
    }
}
