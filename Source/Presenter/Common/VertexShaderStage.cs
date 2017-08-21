using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class VertexShaderStage
    {
        public VertexShader Shader => GraphicsPipeline.State.VertexShader;

        public void Reset()
        {

        }
    }

    class StaticVertexShaderStage : VertexShaderStage { }

    public static partial class GraphicsPipeline
    {
        private static VertexShaderStage vertexShaderStage = new StaticVertexShaderStage();

        public static VertexShaderStage VertexShaderStage => vertexShaderStage;
    }


}
