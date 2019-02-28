using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsTexture : GraphicsResource
    {
        private int mRowPitch;

        public PixelFormat PixelFormat { get; }
        public int Width { get; }
        public int Height { get; }

        public GraphicsTexture(GraphicsDevice device, int width, int height,
            PixelFormat pixelFormat, GraphicsResourceBindType bindType) :
            base(device, width * height * GraphicsConvert.SizeOfInBytes(pixelFormat), bindType)
        {
            Width = width; Height = height;
            PixelFormat = pixelFormat;

            mRowPitch = width * GraphicsConvert.SizeOfInBytes(PixelFormat);

            mResource = new SharpDX.Direct3D11.Texture2D(device.Device, new SharpDX.Direct3D11.Texture2DDescription()
            {
                ArraySize = 1,
                BindFlags = GraphicsConvert.ToBindFlags(bindType),
                CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                Format = GraphicsConvert.ToPixelFormat(PixelFormat),
                Width = Width,
                Height = Height,
                MipLevels = 1,
                OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Usage = SharpDX.Direct3D11.ResourceUsage.Default
            });
        }

        public override void Update<T>(params T[] data)
        {
            Device.ImmediateContext.UpdateSubresource(data, Resource, 0, mRowPitch);
        }

        public override void Update(params byte[] data)
        {
            Device.ImmediateContext.UpdateSubresource(data, Resource, 0, mRowPitch);
        }
    }
}
