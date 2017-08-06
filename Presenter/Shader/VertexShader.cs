using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class VertexShader : Shader
    {
        private SharpDX.Direct3D11.VertexShader shader;

        private void CreateVertexShader(SharpDX.D3DCompiler.ShaderBytecode byteCode,
            string entrypoint, bool isCompiled = false)
        {
            bytecode = byteCode;

            if (isCompiled is true)
            {
                shader = new SharpDX.Direct3D11.VertexShader(Engine.ID3D11Device, bytecode);
                return;
            }

#if DEBUG
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "vs_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.Debug | SharpDX.D3DCompiler.ShaderFlags.SkipOptimization);
#else
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "vs_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.OptimizationLevel2);
#endif
            if (result.HasErrors is true || result.Message != null) throw new Exception(result.Message);

            shader = new SharpDX.Direct3D11.VertexShader(Engine.ID3D11Device, bytecode = result.Bytecode);
        }

        public VertexShader(byte[] shaderCode, string entrypoint, bool isCompiled = false)
        {
            CreateVertexShader(new SharpDX.D3DCompiler.ShaderBytecode(shaderCode), entrypoint, isCompiled);
        }

        public VertexShader(string shaderfile, string entrypoint, bool isCompiled = false)
        {
            System.IO.FileStream file = new System.IO.FileStream(shaderfile, System.IO.FileMode.Open);
            
            CreateVertexShader(new SharpDX.D3DCompiler.ShaderBytecode(file), entrypoint, isCompiled);

            file.Close();
        }

        internal SharpDX.Direct3D11.VertexShader ID3D11VertexShader => shader;

        ~VertexShader() => SharpDX.Utilities.Dispose(ref shader);
    }

   
}
