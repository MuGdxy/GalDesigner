using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum ShaderType : uint
    {
        None = 0,
        VertexShader = 1,
        PixelShader = 2,
        VertexShaderAndPixelShader = 3
    }

    public abstract class GraphicsShader
    {
#if DEBUG
        protected static SharpDX.D3DCompiler.ShaderFlags ShaderFlags => SharpDX.D3DCompiler.ShaderFlags.Debug;
#else
         protected static SharpDX.D3DCompiler.ShaderFlags ShaderFlags => SharpDX.D3DCompiler.ShaderFlags.OptimizationLevel2;
#endif
        protected GraphicsDevice Device { get; }
        
        public byte[] ByteCode { get; private set; }

        public GraphicsShader(GraphicsDevice device, byte[] byteCode)
        {
            ByteCode = byteCode;
            Device = device;
        }
    }
}
