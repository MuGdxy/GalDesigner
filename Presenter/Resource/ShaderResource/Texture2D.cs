using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class Texture2D : ShaderResource
    {
        private int tWidth;
        private int tHeight;
        private int mipLevels;

        private int rowPitch;

        public Texture2D(int width, int height, ResourceFormat format, int miplevels = 1)
        {
            tWidth = width;
            tHeight = height;
            pixelFormat = format;
            mipLevels = miplevels;

            rowPitch = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth;

            resource = new SharpDX.Direct3D11.Texture2D(Engine.ID3D11Device,
                new SharpDX.Direct3D11.Texture2DDescription()
                {
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource | SharpDX.Direct3D11.BindFlags.RenderTarget,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = (SharpDX.DXGI.Format)pixelFormat,
                    Height = tHeight,
                    Width = tWidth,
                    MipLevels = mipLevels,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    Usage = SharpDX.Direct3D11.ResourceUsage.Default
                });

            resourceview = new SharpDX.Direct3D11.ShaderResourceView(Engine.ID3D11Device,
                resource);

            size = ResourceFormatCounter.CountFormatSize(pixelFormat) * tWidth * tHeight;
        }

        public override void Update<T>(ref T data)
        {
            Engine.ImmediateContext.UpdateSubresource(ref data, resource, 0, rowPitch);
        }

        public override void Update<T>(T[] data)
        {
            Engine.ImmediateContext.UpdateSubresource(data, resource, 0, rowPitch);
        }

        public override void Update(IntPtr data)
        {
            Engine.ImmediateContext.UpdateSubresource(resource, 0, null, data, rowPitch, size);
        }

        public static Texture2D FromFile(string filename, int miplevels = 1)
        {
            using (var decoder = new SharpDX.WIC.BitmapDecoder(Engine.ImagingFactory,
            filename, SharpDX.IO.NativeFileAccess.Read, SharpDX.WIC.DecodeOptions.CacheOnLoad))
            {

                using (var converter = new SharpDX.WIC.FormatConverter(Engine.ImagingFactory))
                {

                    using (var frame = decoder.GetFrame(0))
                    {

                        converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppRGBA,
                             SharpDX.WIC.BitmapDitherType.None, null, 0, SharpDX.WIC.BitmapPaletteType.MedianCut);

                        int width = converter.Size.Width;
                        int height = converter.Size.Height;

                        int bpp = SharpDX.WIC.PixelFormat.GetBitsPerPixel(converter.PixelFormat);

                        int rowPitch = (width * bpp + 7) / 8;
                        int imagesize = rowPitch * height;

                        byte[] pixels = new byte[imagesize];

                        converter.CopyPixels(pixels, rowPitch);

                        Texture2D texture = new Texture2D(width, height, (ResourceFormat)WICHelper.Translate[converter.PixelFormat],
                            miplevels);
                    
                        texture.Update(pixels);

                        return texture;
                    }
                }
            }
        }

        public int Width => tWidth;

        public int Height => tHeight;

        public int MipLevels => mipLevels;
    }
}
