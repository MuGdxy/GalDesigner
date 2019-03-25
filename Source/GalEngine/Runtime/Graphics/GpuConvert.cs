using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    using BindFlag = SharpDX.Direct3D11.BindFlags;

    static class GpuConvert
    {
        private static GpuBindUsage[] mBindUsagePool = new GpuBindUsage[]
        {
            GpuBindUsage.VertexBufferr, GpuBindUsage.IndexBuffer,
            GpuBindUsage.ConstantBuffer, GpuBindUsage.ShaderResource,
            GpuBindUsage.StreamOutput, GpuBindUsage.RenderTarget,
            GpuBindUsage.DepthStencil, GpuBindUsage.UnorderedAccess,
            GpuBindUsage.Decoder, GpuBindUsage.VideoEncoder
        };

        private static BindFlag[] mBindFlagPool = new BindFlag[]
        {
             BindFlag.VertexBuffer, BindFlag.IndexBuffer,
             BindFlag.ConstantBuffer, BindFlag.ShaderResource,
             BindFlag.StreamOutput, BindFlag.RenderTarget,
             BindFlag.DepthStencil, BindFlag.UnorderedAccess,
             BindFlag.Decoder, BindFlag.VideoEncoder
        };

        public static bool HasBindUsage(GpuBindUsage usage, GpuBindUsage requirement)
        {
            if (((uint)usage & (uint)requirement) != 0) return true;
            return false;
        }

        public static bool HasCpuAccessFlag(GpuCpuAccessFlag flag, GpuCpuAccessFlag requirement)
        {
            if (((uint)flag & (uint)requirement) != 0) return true;
            return false;
        }

        public static SharpDX.Direct3D.PrimitiveTopology ToPrimitiveType(GpuPrimitiveType primitiveType)
        {
            return (SharpDX.Direct3D.PrimitiveTopology)primitiveType;
        }

        public static BindFlag ToBindUsage(GpuBindUsage bindUsage)
        {
            uint result = 0;

            for (int i = 0; i < mBindUsagePool.Length; i++)
            {
                if (HasBindUsage(bindUsage, mBindUsagePool[i]) == true)
                    result = result | (uint)mBindFlagPool[i];
            }

            return (BindFlag)result;
        }

        public static SharpDX.Direct3D11.ResourceUsage ToHeapType(GpuHeapType heapType)
        {
            switch (heapType)
            {
                case GpuHeapType.Default:
                    return SharpDX.Direct3D11.ResourceUsage.Default;
                case GpuHeapType.Immutable:
                    return SharpDX.Direct3D11.ResourceUsage.Immutable;
                case GpuHeapType.Dynamic:
                    return SharpDX.Direct3D11.ResourceUsage.Dynamic;
                case GpuHeapType.Staging:
                    return SharpDX.Direct3D11.ResourceUsage.Staging;
                default:
                    throw new Exception("The heap type is not support");
            }
        }

        public static SharpDX.Direct3D11.CpuAccessFlags ToCpuAccessFlag(GpuCpuAccessFlag cpuAccessFlag)
        {
            uint result = 0;

            if (HasCpuAccessFlag(cpuAccessFlag, GpuCpuAccessFlag.Read)) result = result | (uint)SharpDX.Direct3D11.CpuAccessFlags.Read;
            if (HasCpuAccessFlag(cpuAccessFlag, GpuCpuAccessFlag.Write)) result = result | (uint)SharpDX.Direct3D11.CpuAccessFlags.Write;

            return (SharpDX.Direct3D11.CpuAccessFlags)result;
        }

        public static SharpDX.DXGI.Format ToPixelFormat(GpuPixelFormat pixelFormat)
        {
            return (SharpDX.DXGI.Format)pixelFormat;
        }

        public static SharpDX.Direct3D11.FillMode ToFillMode(GpuFillMode fillMode)
        {
            return (SharpDX.Direct3D11.FillMode)fillMode;
        }

        public static SharpDX.Direct3D11.CullMode ToCullMode(GpuCullMode cullMode)
        {
            return (SharpDX.Direct3D11.CullMode)cullMode;
        }

        public static SharpDX.Direct3D11.BlendOperation ToBlendOperation(GpuBlendOperation blendOperation)
        {
            return (SharpDX.Direct3D11.BlendOperation)blendOperation;
        }

        public static SharpDX.Direct3D11.BlendOption ToBlendOption(GpuBlendOption blendOption)
        {
            return (SharpDX.Direct3D11.BlendOption)blendOption;
        }

        public static SharpDX.Direct3D11.TextureAddressMode ToTextureAddressMode(GpuTextureAddressMode addressMode)
        {
            return (SharpDX.Direct3D11.TextureAddressMode)addressMode;
        }

        public static SharpDX.Direct3D11.Filter ToTextureFilter(GpuTextureFilter textureFilter)
        {
            return (SharpDX.Direct3D11.Filter)textureFilter;
        }

        public static int SizeOfInBytes(GpuPixelFormat pixelFormat)
        {
            return SharpDX.DXGI.FormatHelper.SizeOfInBytes(ToPixelFormat(pixelFormat));
        }
    }
}
