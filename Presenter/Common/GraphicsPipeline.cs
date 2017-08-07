using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public static partial class GraphicsPipeline
    {
        private static bool isOpened = false;

        static GraphicsPipeline()
        { 

        }

        public static void Open(GraphicsPipelineState GraphicsPipelineState, Present target)
        {
#if DEBUG
            if (IsOpened is true) throw new NotImplementedException("GraphicsPipeline has opened");
#endif
            isOpened = true;

            present = target;

            Reset(GraphicsPipelineState);

            present.ResetViewPort();
        }

        public static void Open(GraphicsPipelineState GraphicsPipelineState, TextureFace target)
        {
#if DEBUG
            if (IsOpened is true) throw new NotImplementedException("GraphicsPipeline has opened");
#endif

            isOpened = true;

            textureFace = target;

            Reset(GraphicsPipelineState);

            textureFace.ResetViewPort();
        }

        public static void Close()
        {
            present?.ClearState();
            textureFace?.ClearState();

            InputAssemblerStage.Reset();
            VertexShaderStage.Reset();
            PixelShaderStage.Reset();
            OutputMergerStage.Reset();

            present = null;
            textureFace = null;
            graphicsPipelineState = null;

            isOpened = false;
        }

        public static bool IsOpened => isOpened;
    }
}
