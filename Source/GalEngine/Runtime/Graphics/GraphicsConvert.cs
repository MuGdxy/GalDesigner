using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    static class GraphicsConvert
    {
        public static SharpDX.Direct3D.PrimitiveTopology ToPrimitiveTopology(PrimitiveType primitiveType)
        {
            return (SharpDX.Direct3D.PrimitiveTopology)primitiveType;
        }

        public static SharpDX.Direct3D11.BindFlags ToBindFlags(GraphicsResourceBindType graphicsResourceBindType)
        {
            return (SharpDX.Direct3D11.BindFlags)graphicsResourceBindType;
        }

        public static SharpDX.DXGI.Format ToPixelFormat(PixelFormat pixelFormat)
        {
            return (SharpDX.DXGI.Format)pixelFormat;
        }

        public static SharpDX.Direct3D11.FillMode ToFillMode(FillMode fillMode)
        {
            return (SharpDX.Direct3D11.FillMode)fillMode;
        }

        public static SharpDX.Direct3D11.CullMode ToCullMode(CullMode cullMode)
        {
            return (SharpDX.Direct3D11.CullMode)cullMode;
        }

        public static SharpDX.Direct3D11.BlendOperation ToBlendOperation(BlendOperation blendOperation)
        {
            return (SharpDX.Direct3D11.BlendOperation)blendOperation;
        }

        public static SharpDX.Direct3D11.BlendOption ToBlendOption(BlendOption blendOption)
        {
            return (SharpDX.Direct3D11.BlendOption)blendOption;
        }

        public static int SizeOfInBytes(PixelFormat pixelFormat)
        {
            return SharpDX.DXGI.FormatHelper.SizeOfInBytes(ToPixelFormat(pixelFormat));
        }
    }
}
