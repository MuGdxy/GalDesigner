using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GpuVertexShader : GpuShader, IDisposable
    {
        private SharpDX.Direct3D11.VertexShader mVertexShader;

        internal SharpDX.Direct3D11.VertexShader VertexShader => mVertexShader;

        public GpuVertexShader(GpuDevice device, byte[] byteCode) : base(device, byteCode)
        {
            mVertexShader = new SharpDX.Direct3D11.VertexShader(GpuDevice.Device, ByteCode);
        }

        ~GpuVertexShader() => Dispose();

        public static byte[] Compile(byte[] byteCode, string entryPoint = "main")
        {
            LogEmitter.Apply(LogLevel.Information, "[Start Compile Vertex Shader]");

            var result = SharpDX.D3DCompiler.ShaderBytecode.Compile(byteCode, entryPoint, "vs_5_0",
                 ShaderFlags);

            LogEmitter.Assert(result.HasErrors == false, LogLevel.Error, "[Compile Vertex Shader Failed] [Message = {0}]", result.Message);

            return result;
        }

        public void Dispose()
        {
            SharpDX.Utilities.Dispose(ref mVertexShader);
        }
    }
}
