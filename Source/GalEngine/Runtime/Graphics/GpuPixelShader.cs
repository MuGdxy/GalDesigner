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
            LogEmitter.Apply(LogLevel.Information, "[Start Compile Pixel Shader]");

            var result = SharpDX.D3DCompiler.ShaderBytecode.Compile(byteCode, entryPoint, "ps_5_0",
                    ShaderFlags, SharpDX.D3DCompiler.EffectFlags.None);

            if (result.Message != null)
            {
                var messages = result.Message.Split('\n');
                
                foreach (var message in messages)
                {
                    if (message == "") continue;

                    LogEmitter.Apply(LogLevel.Warning, "[Pixel Shader Message = {0}]", message);
                }
            }

            LogEmitter.Apply(LogLevel.Information, "[Finish Compile Pixel Shader]");

            LogEmitter.Assert(result.HasErrors == false, LogLevel.Error, "[Compile Pixel Shader Failed]");

            return result;
        }

        public void Dispose()
        {
            //we can dispose it any times, because we only dispose resource really at the first time
            SharpDX.Utilities.Dispose(ref mPixelShader);
        }
    }
}
