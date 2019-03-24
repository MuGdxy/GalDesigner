using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum CpuAccessFlag
    {
        None = 0,
        Read = 1,
        Write = 2
    }

    public enum BindUsage
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

   public enum HeapType
    {
        Default = 0,
        Immutable = 1,
        Dynamic = 2,
        Staging = 3
    }

    public class GpuResourceInfo
    {
        public CpuAccessFlag CpuAccessFlag { get; }
        public BindUsage BindUsage { get; }
        public HeapType HeapType { get; }

        public GpuResourceInfo(
            BindUsage bindUsage,
            CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None,
            HeapType heapType = HeapType.Default)
        {
            BindUsage = bindUsage;
            CpuAccessFlag = cpuAccessFlag;
            HeapType = heapType;
        }

        public static GpuResourceInfo ConstantBuffer(
            CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None,
            HeapType heapType = HeapType.Default) 
            => new GpuResourceInfo(BindUsage.ConstantBuffer, cpuAccessFlag, heapType);

        public static GpuResourceInfo VertexBuffer(
            CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None,
            HeapType heapType = HeapType.Default)
            => new GpuResourceInfo(BindUsage.VertexBufferr, cpuAccessFlag, heapType);

        public static GpuResourceInfo IndexBuffer(
            CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None,
            HeapType heapType = HeapType.Default)
            => new GpuResourceInfo(BindUsage.IndexBuffer, cpuAccessFlag, heapType);

        public static GpuResourceInfo ShaderResource(
            CpuAccessFlag cpuAccessFlag = CpuAccessFlag.None,
            HeapType heapType = HeapType.Default)
            => new GpuResourceInfo(BindUsage.ShaderResource, cpuAccessFlag, heapType);
    }
}
