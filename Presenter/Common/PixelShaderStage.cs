using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class PixelShaderStage
    {
        PixelShader Shader => GraphicsPipeline.State.PixelShader;

        public void Reset()
        {

        }
    }

    class StaticPixelShaderStage : PixelShaderStage { }

    public static partial class GraphicsPipeline
    {
        private static PixelShaderStage pixelShaderStage = new StaticPixelShaderStage();

        public static PixelShaderStage PixelShaderStage => pixelShaderStage;
    }

}
