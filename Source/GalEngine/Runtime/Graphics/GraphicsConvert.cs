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
    }
}
