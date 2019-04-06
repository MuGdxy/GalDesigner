using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GpuPixelShader : GpuShader, IDisposable
    {
        private SharpDX.Direct3D11.PixelShader mPixelShader;

        internal SharpDX.Direct3D11.PixelShader PixelShader { get => mPixelShader; }

        public GpuPixelShader(GpuDevice device, byte[] byteCode) : base(device, byteCode)
        {
            mPixelShader = new SharpDX.Direct3D11.PixelShader(GpuDevice.Device, ByteCode);
        }

        ~GpuPixelShader() => Dispose();

        public static byte[] Compile(byte[] byteCode, string entryPoint = "main")
        {
            LogEmitter.Apply(LogLevel.Information, "[Start Compile Pixel Shader]", LogLevel.Information);

            var result = SharpDX.D3DCompiler.ShaderBytecode.Compile(byteCode, entryPoint, "ps_5_0",
                    ShaderFlags, SharpDX.D3DCompiler.EffectFlags.None);

            LogEmitter.Assert(result.HasErrors == false,LogLevel.Error, "[Compile Pixel Shader Failed] [Message = {0}]", result.Message);

            return result;
        }

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mPixelShader);
        }
    }
}
