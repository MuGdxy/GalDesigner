using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class InputAssemblerStage
    {
        private Buffer vertexBuffer;
        private Buffer indexBuffer;

        private PrimitiveType primitiveType = PrimitiveType.TriangleList;

        public void Reset()
        {
            vertexBuffer = null;
            indexBuffer = null;

            PrimitiveType = PrimitiveType.TriangleList;
        }

        public Buffer VertexBuffer
        {
            get => vertexBuffer;
            set
            {
                vertexBuffer = value;

                Engine.ImmediateContext.InputAssembler.SetVertexBuffers(0,
                    new SharpDX.Direct3D11.VertexBufferBinding(vertexBuffer.ID3D11Buffer, vertexBuffer.Size / vertexBuffer.Count, 0));
            }
        }

        public Buffer IndexBuffer
        {
            get => indexBuffer;
            set
            {
                indexBuffer = value;

                Engine.ImmediateContext.InputAssembler.SetIndexBuffer(indexBuffer.ID3D11Buffer,
                     SharpDX.DXGI.Format.R32_UInt, 0);
            }
        }

        public PrimitiveType PrimitiveType
        {
            get => primitiveType;
            set
            {
                primitiveType = value;

                Engine.ImmediateContext.InputAssembler.PrimitiveTopology = (SharpDX.Direct3D.PrimitiveTopology)primitiveType;
            }
        }

        public InputLayout InputLayout
        {
            get => GraphicsPipeline.State.InputLayout;
        }

    }

    class StaticInputAssemblerStage : InputAssemblerStage { }

    public static partial class GraphicsPipeline
    {
        private static InputAssemblerStage inputAssemblerStage = new StaticInputAssemblerStage();

        public static InputAssemblerStage InputAssemblerStage => inputAssemblerStage;

    }

}
