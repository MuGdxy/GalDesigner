using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsVertexShader : GraphicsShader
    {
        internal SharpDX.Direct3D11.VertexShader VertexShader { get; }

        public GraphicsVertexShader(GraphicsDevice device, byte[] byteCode) : base(device, byteCode)
        {
            VertexShader = new SharpDX.Direct3D11.VertexShader(Device.Device, ByteCode);
        }

        public static byte[] Compile(byte[] byteCode, string entryPoint = "main")
        {
            LogEmitter.Apply(LogLevel.Information, "[Start Compile Vertex Shader]");

            var result = SharpDX.D3DCompiler.ShaderBytecode.Compile(byteCode, entryPoint, "vs_5_0",
                 ShaderFlags, SharpDX.D3DCompiler.EffectFlags.None);

            LogEmitter.Assert(result.HasErrors == false, LogLevel.Error, "[Compile Vertex Shader Failed] [Message = {0}]", result.Message);

            return result;
        }
    }
}
