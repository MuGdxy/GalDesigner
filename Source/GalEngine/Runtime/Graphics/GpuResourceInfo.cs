using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum GpuCpuAccessFlag
    {
        None = 0,
        Read = 1,
        Write = 2
    }

    public enum GpuBindUsage
    {
        None = 0,
        VertexBufferr = 1,
        IndexBuffer = 2,
        ConstantBuffer = 4,
        ShaderResource = 8,
        StreamOutput = 16,
        RenderTarget = 32,
        DepthStencil = 64,
        UnorderedAccess = 128,
        Decoder = 512,
        VideoEncoder = 1024
    }

   public enum GpuHeapType
    {
        Default = 0,
        Immutable = 1,
        Dynamic = 2,
        Staging = 3
    }

    public class GpuResourceInfo
    {
        public GpuCpuAccessFlag CpuAccessFlag { get; }
        public GpuBindUsage BindUsage { get; }
        public GpuHeapType HeapType { get; }

        public GpuResourceInfo(
            GpuBindUsage bindUsage,
            GpuCpuAccessFlag cpuAccessFlag = GpuCpuAccessFlag.None,
            GpuHeapType heapType = GpuHeapType.Default)
        {
            BindUsage = bindUsage;
            CpuAccessFlag = cpuAccessFlag;
            HeapType = heapType;
        }

        public static GpuResourceInfo ConstantBuffer(
            GpuCpuAccessFlag cpuAccessFlag = GpuCpuAccessFlag.None,
            GpuHeapType heapType = GpuHeapType.Default) 
            => new GpuResourceInfo(GpuBindUsage.ConstantBuffer, cpuAccessFlag, heapType);

        public static GpuResourceInfo VertexBuffer(
            GpuCpuAccessFlag cpuAccessFlag = GpuCpuAccessFlag.None,
            GpuHeapType heapType = GpuHeapType.Default)
            => new GpuResourceInfo(GpuBindUsage.VertexBufferr, cpuAccessFlag, heapType);

        public static GpuResourceInfo IndexBuffer(
            GpuCpuAccessFlag cpuAccessFlag = GpuCpuAccessFlag.None,
            GpuHeapType heapType = GpuHeapType.Default)
            => new GpuResourceInfo(GpuBindUsage.IndexBuffer, cpuAccessFlag, heapType);

        public static GpuResourceInfo ShaderResource(
            GpuCpuAccessFlag cpuAccessFlag = GpuCpuAccessFlag.None,
            GpuHeapType heapType = GpuHeapType.Default)
            => new GpuResourceInfo(GpuBindUsage.ShaderResource, cpuAccessFlag, heapType);
    }
}
