using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public partial class PixelShader : Shader
    {
        private SharpDX.Direct3D11.PixelShader shader;

        private void CreatePixelShader(SharpDX.D3DCompiler.ShaderBytecode byteCode,
            string entrypoint,bool isCompiled = false)
        {
            bytecode = byteCode;

            if (isCompiled is true)
            {
                shader = new SharpDX.Direct3D11.PixelShader(Engine.ID3D11Device, bytecode);
                return;
            }

#if DEBUG
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "ps_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.Debug | SharpDX.D3DCompiler.ShaderFlags.SkipOptimization);
#else
            SharpDX.D3DCompiler.CompilationResult result = SharpDX.D3DCompiler.ShaderBytecode.Compile(bytecode, entrypoint, "ps_5_0",
                 SharpDX.D3DCompiler.ShaderFlags.OptimizationLevel2);
#endif
            if (result.Message != null || result.Message != null) throw new Exception(result.Message);

            shader = new SharpDX.Direct3D11.PixelShader(Engine.ID3D11Device, bytecode = result.Bytecode);
        }

        public PixelShader(byte[] shaderCode, string entrypoint, bool isCompiled = false)
        {
            CreatePixelShader(new SharpDX.D3DCompiler.ShaderBytecode(shaderCode), entrypoint, isCompiled);
        }

        public PixelShader(string shaderfile, string entrypoint, bool isCompiled = false)
        {
            System.IO.FileStream file = new System.IO.FileStream(shaderfile, System.IO.FileMode.Open);

            CreatePixelShader(new SharpDX.D3DCompiler.ShaderBytecode(file), entrypoint, isCompiled);

            file.Close();
        }

        internal SharpDX.Direct3D11.PixelShader ID3D11PixelShader => shader;

        ~PixelShader() => SharpDX.Utilities.Dispose(ref shader);
    }

}
