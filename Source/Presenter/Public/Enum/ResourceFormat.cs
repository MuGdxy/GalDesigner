using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public enum ResourceFormat
    {
        R32G32B32A32_Float = SharpDX.DXGI.Format.R32G32B32A32_Float,
        R16G16B16A16_Float = SharpDX.DXGI.Format.R16G16B16A16_Float,
        R16G16B16A16 = SharpDX.DXGI.Format.R16G16B16A16_UNorm,
        R8G8B8A8 = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
        B8G8R8A8 = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
        B8G8R8X8 = SharpDX.DXGI.Format.B8G8R8X8_UNorm,
        R32_Float = SharpDX.DXGI.Format.R32_Float,
        R16_Float = SharpDX.DXGI.Format.R16_Float,
        R16 = SharpDX.DXGI.Format.R16_UNorm,
        R8 = SharpDX.DXGI.Format.R8_UNorm,
        A8 = SharpDX.DXGI.Format.A8_UNorm,
        Unknown = SharpDX.DXGI.Format.Unknown
    }

    public static class ResourceFormatCounter
    {
        public static int CountFormatSize(ResourceFormat format)
        {
            switch (format)
            {
                case ResourceFormat.R32G32B32A32_Float:
                    return 16;
                case ResourceFormat.R16G16B16A16_Float:
                    return 8;
                case ResourceFormat.R16G16B16A16:
                    return 8;
                case ResourceFormat.R8G8B8A8:
                    return 4;
                case ResourceFormat.B8G8R8A8:
                    return 4;
                case ResourceFormat.B8G8R8X8:
                    return 4;
                case ResourceFormat.R32_Float:
                    return 4;
                case ResourceFormat.R16_Float:
                    return 2;
                case ResourceFormat.R16:
                    return 2;
                case ResourceFormat.R8:
                    return 1;
                case ResourceFormat.A8:
                    return 1;
                case ResourceFormat.Unknown:
                    return 0;
            }
            throw new NotImplementedException("Invalid format");
        }
    }

}
