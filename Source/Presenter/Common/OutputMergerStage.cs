using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Presenter
{
    public abstract class OutputMergerStage
    {
        public void Reset()
        {
            
        }

        public DepthStencilState DepthStencilState => GraphicsPipeline.State.DepthStencilState;

        public BlendState BlendState => GraphicsPipeline.State.BlendState;

        public Vector4 BlendFactor
        {
            set => Engine.ImmediateContext.OutputMerger.BlendFactor = new SharpDX.Mathematics.Interop.RawColor4(
                value.X, value.Y, value.Z, value.W);
        }
    }

    class StaticOutputMergerStage: OutputMergerStage { }

    public static partial class GraphicsPipeline
    {
        private static OutputMergerStage outputMergerStage = new StaticOutputMergerStage();

        public static OutputMergerStage OutputMergerStage => outputMergerStage;
    }
}
