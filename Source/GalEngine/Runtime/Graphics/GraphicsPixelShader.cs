using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsPixelShader : GraphicsShader
    {
        internal SharpDX.Direct3D11.PixelShader PixelShader { get; }

        public GraphicsPixelShader(GraphicsDevice device, byte[] byteCode) : base(device, byteCode)
        {
            PixelShader = new SharpDX.Direct3D11.PixelShader(Device.Device, ByteCode);
        }

        public static byte[] Compile(byte[] byteCode, string entryPoint = "main")
        {
            LogEmitter.Apply(LogLevel.Information, "[Start Compile Pixel Shader]", LogLevel.Information);

            var result = SharpDX.D3DCompiler.ShaderBytecode.Compile(byteCode, entryPoint, "ps_5_0",
                    ShaderFlags, SharpDX.D3DCompiler.EffectFlags.None);

            LogEmitter.Assert(result.HasErrors == false,LogLevel.Error, "[Compile Pixel Shader Failed] [Message = {0}]", result.Message);

            return result;
        }
    }
}
