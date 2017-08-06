using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    internal class WICTranslate
    {
        private Dictionary<Guid, SharpDX.DXGI.Format> Translate
           = new Dictionary<Guid, SharpDX.DXGI.Format>();

        public WICTranslate()
        {
            Translate.Add(SharpDX.WIC.PixelFormat.Format128bppRGBAFloat, SharpDX.DXGI.Format.R32G32B32A32_Float);

            Translate.Add(SharpDX.WIC.PixelFormat.Format64bppRGBAHalf, SharpDX.DXGI.Format.R16G16B16A16_Float);
            Translate.Add(SharpDX.WIC.PixelFormat.Format64bppRGBA, SharpDX.DXGI.Format.R16G16B16A16_UNorm);

            Translate.Add(SharpDX.WIC.PixelFormat.Format32bppRGBA, SharpDX.DXGI.Format.R8G8B8A8_UNorm);
            Translate.Add(SharpDX.WIC.PixelFormat.Format32bppBGRA, SharpDX.DXGI.Format.B8G8R8A8_UNorm);
            Translate.Add(SharpDX.WIC.PixelFormat.Format32bppBGR, SharpDX.DXGI.Format.B8G8R8X8_UNorm);

            Translate.Add(SharpDX.WIC.PixelFormat.Format32bppRGBA1010102XR, SharpDX.DXGI.Format.R10G10B10_Xr_Bias_A2_UNorm);
            Translate.Add(SharpDX.WIC.PixelFormat.Format32bppRGBA1010102, SharpDX.DXGI.Format.R10G10B10A2_UNorm);

            Translate.Add(SharpDX.WIC.PixelFormat.Format16bppBGRA5551, SharpDX.DXGI.Format.B5G5R5A1_UNorm);
            Translate.Add(SharpDX.WIC.PixelFormat.Format16bppBGR565, SharpDX.DXGI.Format.B5G6R5_UNorm);

            Translate.Add(SharpDX.WIC.PixelFormat.Format32bppGrayFloat, SharpDX.DXGI.Format.R32_Float);
            Translate.Add(SharpDX.WIC.PixelFormat.Format16bppGrayHalf, SharpDX.DXGI.Format.R16_Float);
            Translate.Add(SharpDX.WIC.PixelFormat.Format16bppGray, SharpDX.DXGI.Format.R16_UNorm);
            Translate.Add(SharpDX.WIC.PixelFormat.Format8bppGray, SharpDX.DXGI.Format.R8_UNorm);

            Translate.Add(SharpDX.WIC.PixelFormat.Format8bppAlpha, SharpDX.DXGI.Format.A8_UNorm);
        }

        public Dictionary<Guid, SharpDX.DXGI.Format> Element => Translate;

        public SharpDX.DXGI.Format this[Guid index]
        {
            get
            {
                if (Translate.ContainsKey(index) is false)
                    return SharpDX.DXGI.Format.Unknown;
                return Translate[index];
            }
        }
    }

    internal class WICConvert
    {

        private Dictionary<Guid, Guid> Convert = new Dictionary<Guid, Guid>();

        public WICConvert()
        {
            Convert.Add(SharpDX.WIC.PixelFormat.FormatBlackWhite, SharpDX.WIC.PixelFormat.Format8bppGray);

            Convert.Add(SharpDX.WIC.PixelFormat.Format1bppIndexed, SharpDX.WIC.PixelFormat.Format32bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format2bppIndexed, SharpDX.WIC.PixelFormat.Format32bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format4bppIndexed, SharpDX.WIC.PixelFormat.Format32bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format8bppIndexed, SharpDX.WIC.PixelFormat.Format32bppRGBA);

            Convert.Add(SharpDX.WIC.PixelFormat.Format2bppGray, SharpDX.WIC.PixelFormat.Format8bppGray);
            Convert.Add(SharpDX.WIC.PixelFormat.Format4bppGray, SharpDX.WIC.PixelFormat.Format8bppGray);

            Convert.Add(SharpDX.WIC.PixelFormat.Format16bppGrayFixedPoint, SharpDX.WIC.PixelFormat.Format16bppGrayHalf);
            Convert.Add(SharpDX.WIC.PixelFormat.Format32bppGrayFixedPoint, SharpDX.WIC.PixelFormat.Format32bppGrayFloat);

            Convert.Add(SharpDX.WIC.PixelFormat.Format16bppBGR555, SharpDX.WIC.PixelFormat.Format16bppBGRA5551);

            Convert.Add(SharpDX.WIC.PixelFormat.Format32bppBGR101010, SharpDX.WIC.PixelFormat.Format32bppRGBA1010102);

            Convert.Add(SharpDX.WIC.PixelFormat.Format24bppBGR, SharpDX.WIC.PixelFormat.Format32bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format24bppRGB, SharpDX.WIC.PixelFormat.Format32bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format32bppPBGRA, SharpDX.WIC.PixelFormat.Format32bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format32bppPRGBA, SharpDX.WIC.PixelFormat.Format32bppRGBA);

            Convert.Add(SharpDX.WIC.PixelFormat.Format48bppRGB, SharpDX.WIC.PixelFormat.Format48bppRGB);
            Convert.Add(SharpDX.WIC.PixelFormat.Format48bppBGR, SharpDX.WIC.PixelFormat.Format64bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format64bppRGBAFixedPoint, SharpDX.WIC.PixelFormat.Format64bppRGBAHalf);
            Convert.Add(SharpDX.WIC.PixelFormat.Format64bppBGRAFixedPoint, SharpDX.WIC.PixelFormat.Format64bppRGBAHalf);
            Convert.Add(SharpDX.WIC.PixelFormat.Format64bppRGBFixedPoint, SharpDX.WIC.PixelFormat.Format64bppRGBAHalf);
            Convert.Add(SharpDX.WIC.PixelFormat.Format64bppRGBHalf, SharpDX.WIC.PixelFormat.Format64bppRGBAHalf);
            Convert.Add(SharpDX.WIC.PixelFormat.Format48bppRGBHalf, SharpDX.WIC.PixelFormat.Format64bppRGBAHalf);

            Convert.Add(SharpDX.WIC.PixelFormat.Format128bppPRGBAFloat, SharpDX.WIC.PixelFormat.Format128bppRGBAFloat);
            Convert.Add(SharpDX.WIC.PixelFormat.Format128bppRGBFloat, SharpDX.WIC.PixelFormat.Format128bppRGBAFloat);
            Convert.Add(SharpDX.WIC.PixelFormat.Format128bppRGBAFixedPoint, SharpDX.WIC.PixelFormat.Format128bppRGBAFloat);
            Convert.Add(SharpDX.WIC.PixelFormat.Format128bppRGBFixedPoint, SharpDX.WIC.PixelFormat.Format128bppRGBAFloat);
            Convert.Add(SharpDX.WIC.PixelFormat.Format32bppRGBE, SharpDX.WIC.PixelFormat.Format128bppRGBAFloat);

            Convert.Add(SharpDX.WIC.PixelFormat.Format32bppCMYK, SharpDX.WIC.PixelFormat.Format32bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format64bppCMYK, SharpDX.WIC.PixelFormat.Format64bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format40bppCMYKAlpha, SharpDX.WIC.PixelFormat.Format32bppRGBA);
            Convert.Add(SharpDX.WIC.PixelFormat.Format80bppCMYKAlpha, SharpDX.WIC.PixelFormat.Format64bppRGBA);
        }

        public Dictionary<Guid, Guid> Element => Convert;

        public Guid this[Guid index]
        {
            get
            {
                if (Convert.ContainsKey(index) is false)
                    throw new ArgumentException("Index Vailed");
                return Convert[index];
            }
        }
    }

}
