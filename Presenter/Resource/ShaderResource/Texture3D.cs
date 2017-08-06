using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Texture3D : ShaderResource
    {
        private int tWidth;
        private int tHeight;
        private int tDepth;
        private int mipLevels;

        private int rowPitch;
        private int depthPitch;

        public Texture3D(int width, int height, int depth, ResourceFormat format, int miplevels = 1)
        {
            tWidth = width;
            tHeight = height;
            tDepth = depth;
            pixelFormat = format;
            mipLevels = miplevels;

            rowPitch = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;
            depthPitch = rowPitch * tHeight;

            resource = new SharpDX.Direct3D11.Texture3D(Engine.ID3D11Device,
                new SharpDX.Direct3D11.Texture3DDescription()
                {
                    BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Width = tWidth,
                    Height = tHeight,
                    Depth = tDepth,
                    Format = (SharpDX.DXGI.Format)pixelFormat,
                    MipLevels = mipLevels,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default
                });

            resourceview = new SharpDX.Direct3D11.ShaderResourceView(Engine.ID3D11Device,
                resource);

            size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth * tHeight * tDepth;
        }

        public override void Update<T>(ref T data)
        {
            Engine.ImmediateContext.UpdateSubresource(ref data, resource, 0, rowPitch, depthPitch);
        }

        public override void Update<T>(T[] data)
        {
            Engine.ImmediateContext.UpdateSubresource(data, resource, 0, rowPitch, depthPitch);
        }

        public override void Update(IntPtr data)
        {
            Engine.ImmediateContext.UpdateSubresource(resource, 0, null, data, rowPitch, depthPitch);
        }

        public int Width => tWidth;

        public int Height => tHeight;

        public int Depth => tDepth;

        public int MipLevels => mipLevels;
    }
}
