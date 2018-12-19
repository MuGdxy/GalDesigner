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
            GraphicsLogProvider.Log("[Start Compile Pixel Shader] [object]", LogLevel.Information);

            var result = SharpDX.D3DCompiler.ShaderBytecode.Compile(byteCode, entryPoint, "ps_5_0",
                    ShaderFlags, SharpDX.D3DCompiler.EffectFlags.None);

            GraphicsLogProvider.Assert(result.HasErrors == false, "[Compile Pixel Shader Failed] [Message = {0}] [object]", LogLevel.Error, result.Message);

            return result;
        }
    }
}
