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
        private static BindUsage[] mBindUsagePool = new BindUsage[]
        {
            BindUsage.VertexBufferr, BindUsage.IndexBuffer,
            BindUsage.ConstantBuffer, BindUsage.ShaderResource,
            BindUsage.StreamOutput, BindUsage.RenderTarget,
            BindUsage.DepthStencil, BindUsage.UnorderedAccess,
            BindUsage.Decoder, BindUsage.VideoEncoder
        };

        private static BindFlag[] mBindFlagPool = new BindFlag[]
        {
             BindFlag.VertexBuffer, BindFlag.IndexBuffer,
             BindFlag.ConstantBuffer, BindFlag.ShaderResource,
             BindFlag.StreamOutput, BindFlag.RenderTarget,
             BindFlag.DepthStencil, BindFlag.UnorderedAccess,
             BindFlag.Decoder, BindFlag.VideoEncoder
        };

        public static bool HasBindUsage(BindUsage usage, BindUsage requirement)
        {
            if (((uint)usage & (uint)requirement) != 0) return true;
            return false;
        }

        public static bool HasCpuAccessFlag(CpuAccessFlag flag, CpuAccessFlag requirement)
        {
            if (((uint)flag & (uint)requirement) != 0) return true;
            return false;
        }

        public static SharpDX.Direct3D.PrimitiveTopology ToPrimitiveType(PrimitiveType primitiveType)
        {
            return (SharpDX.Direct3D.PrimitiveTopology)primitiveType;
        }

        public static BindFlag ToBindUsage(BindUsage bindUsage)
        {
            uint result = 0;

            for (int i = 0; i < mBindUsagePool.Length; i++)
            {
                if (HasBindUsage(bindUsage, mBindUsagePool[i]) == true)
                    result = result | (uint)mBindFlagPool[i];
            }

            return (BindFlag)result;
        }

        public static SharpDX.Direct3D11.ResourceUsage ToHeapType(HeapType heapType)
        {
            switch (heapType)
            {
                case HeapType.Default:
                    return SharpDX.Direct3D11.ResourceUsage.Default;
                case HeapType.Immutable:
                    return SharpDX.Direct3D11.ResourceUsage.Immutable;
                case HeapType.Dynamic:
                    return SharpDX.Direct3D11.ResourceUsage.Dynamic;
                case HeapType.Staging:
                    return SharpDX.Direct3D11.ResourceUsage.Staging;
                default:
                    throw new Exception("The heap type is not support");
            }
        }

        public static SharpDX.Direct3D11.CpuAccessFlags ToCpuAccessFlag(CpuAccessFlag cpuAccessFlag)
        {
            uint result = 0;

            if (HasCpuAccessFlag(cpuAccessFlag, CpuAccessFlag.Read)) result = result | (uint)SharpDX.Direct3D11.CpuAccessFlags.Read;
            if (HasCpuAccessFlag(cpuAccessFlag, CpuAccessFlag.Write)) result = result | (uint)SharpDX.Direct3D11.CpuAccessFlags.Write;

            return (SharpDX.Direct3D11.CpuAccessFlags)result;
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

        public static SharpDX.Direct3D11.TextureAddressMode ToTextureAddressMode(TextureAddressMode addressMode)
        {
            return (SharpDX.Direct3D11.TextureAddressMode)addressMode;
        }

        public static SharpDX.Direct3D11.Filter ToTextureFilter(TextureFilter textureFilter)
        {
            return (SharpDX.Direct3D11.Filter)textureFilter;
        }

        public static int SizeOfInBytes(PixelFormat pixelFormat)
        {
            return SharpDX.DXGI.FormatHelper.SizeOfInBytes(ToPixelFormat(pixelFormat));
        }
    }
}
