using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class GraphicsPipelineState
    {
        private VertexShader vertexShader;
        private PixelShader pixelShader;

        private InputLayout inputLayout;
        private ResourceLayout resourceLayout;

        private DepthStencilState depthStencilState;
        private BlendState blendState;
        private RasterizerState rasterizerState;

        private SharpDX.Direct3D11.InputLayout graphicsInputLayout;
        private SharpDX.Direct3D11.DepthStencilState graphicsDepthStencilState;
        private SharpDX.Direct3D11.BlendState graphicsBlendState;
        private SharpDX.Direct3D11.RasterizerState graphicsRasterizerState;


        public GraphicsPipelineState(VertexShader vertexshader,
            PixelShader pixelshader, InputLayout inputlayout,
            ResourceLayout resourcelayout, RasterizerState rasterizerstate,
            DepthStencilState depthstencilstate, BlendState blendstate)
        {
            vertexShader = vertexshader;
            pixelShader = pixelshader;

            inputLayout = inputlayout;

            resourceLayout = resourcelayout is null ? new ResourceLayout() : resourcelayout;

            rasterizerState = rasterizerstate is null ? new RasterizerState() : rasterizerstate;
            depthStencilState = depthstencilstate is null ? new DepthStencilState(false, false) : depthstencilstate;
            blendState = blendstate is null ? new BlendState(false) : blendstate;

            graphicsInputLayout = new SharpDX.Direct3D11.InputLayout(Engine.ID3D11Device,
                vertexShader.ByteCode, inputLayout.Elements);

            graphicsDepthStencilState = new SharpDX.Direct3D11.DepthStencilState(Engine.ID3D11Device,
                depthStencilState.ID3D11DepthStencilStateDescription);

            graphicsBlendState = new SharpDX.Direct3D11.BlendState(Engine.ID3D11Device,
                blendState.ID3D11BlendStateDescription);

            graphicsRasterizerState = new SharpDX.Direct3D11.RasterizerState(Engine.ID3D11Device,
                rasterizerState.ID3D11RasterizerStateDescription);
        }
        
        public VertexShader VertexShader => vertexShader;

        public PixelShader PixelShader => pixelShader;

        public InputLayout InputLayout => inputLayout;

        public ResourceLayout ResourceLayout => resourceLayout;

        public DepthStencilState DepthStencilState => depthStencilState;

        public BlendState BlendState => blendState;

        internal SharpDX.Direct3D11.InputLayout ID3D11InputLayout => graphicsInputLayout;
        internal SharpDX.Direct3D11.DepthStencilState ID3D11DepthStencilState => graphicsDepthStencilState;
        internal SharpDX.Direct3D11.BlendState ID3D11BlendState => graphicsBlendState;
        internal SharpDX.Direct3D11.RasterizerState ID3D11RasterizerState => graphicsRasterizerState;
    }

    public static partial class GraphicsPipeline
    {
        private static GraphicsPipelineState graphicsPipelineState;


        public static GraphicsPipelineState State
        {
            get => graphicsPipelineState;
        }

        public static void Reset(GraphicsPipelineState GraphicsPipelineState)
        {
            graphicsPipelineState = GraphicsPipelineState;

            Engine.ImmediateContext.VertexShader.Set(graphicsPipelineState.VertexShader.ID3D11VertexShader);
            Engine.ImmediateContext.PixelShader.Set(graphicsPipelineState.PixelShader.ID3D11PixelShader);

            Engine.ImmediateContext.InputAssembler.InputLayout = graphicsPipelineState.ID3D11InputLayout;
            Engine.ImmediateContext.OutputMerger.DepthStencilState = graphicsPipelineState.ID3D11DepthStencilState;
            Engine.ImmediateContext.OutputMerger.BlendState = graphicsPipelineState.ID3D11BlendState;
            Engine.ImmediateContext.Rasterizer.State = graphicsPipelineState.ID3D11RasterizerState;
        }
    }
}
