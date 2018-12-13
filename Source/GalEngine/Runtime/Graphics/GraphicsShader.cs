using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
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
