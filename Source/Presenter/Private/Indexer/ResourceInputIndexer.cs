using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ResourceInputIndexer
    {
        private void ReportError(int index, ResourceType input,
            ResourceType target)
        {
#if DEBUG
            if (input != target)
                throw new NotImplementedException("Resource InputSlot " + index + " need: "
                    + target + ", but set: " + input);
#endif 
        }

        private void ReportError(int currentCount, int targetCount, ResourceType type)
        {
#if DEBUG
            if (currentCount > targetCount && targetCount != -1)
                throw new NotImplementedException("Resource InputSlot too many resource in heap, Type: "
                    + type);
#endif 
        }

        internal ResourceInputIndexer()
        {

        }

        public object this[int index]
        { 
            set
            {
                ResourceLayout.Element element = GraphicsPipeline.State.ResourceLayout.Elements[index];

                switch (value)
                {
                    case Buffer buffer:
                        ReportError(index, ResourceType.ConstantBufferView, element.Type);

                        Engine.ImmediateContext.VertexShader.SetConstantBuffer(element.Register, buffer.ID3D11Buffer);
                        Engine.ImmediateContext.PixelShader.SetConstantBuffer(element.Register, buffer.ID3D11Buffer);
                        break;
                    case ShaderResource shaderResource:
                        ReportError(index, ResourceType.ShaderResourceView, element.Type);

                        Engine.ImmediateContext.VertexShader.SetShaderResource(element.Register, shaderResource.ID3D11ShaderResourceView);
                        Engine.ImmediateContext.PixelShader.SetShaderResource(element.Register, shaderResource.ID3D11ShaderResourceView);
                        break;
                    case ResourceTable resourceTable:

                        switch (element.Type)
                        {
                            case ResourceType.ConstantBufferTable:
                                SharpDX.Direct3D11.Buffer[] buffers = new SharpDX.Direct3D11.Buffer[element.Count];

                                for (int i = 0; i < element.Count; i++)
                                    buffers[i] = (resourceTable.WhcihHeap.Elements[i + resourceTable.Start] as Buffer).ID3D11Buffer;

                                Engine.ImmediateContext.VertexShader.SetConstantBuffers(element.Register, buffers);
                                Engine.ImmediateContext.PixelShader.SetConstantBuffers(element.Register, buffers);
                                break;
                            case ResourceType.ShaderResourceTable:
                                SharpDX.Direct3D11.ShaderResourceView[] shaderResources = new SharpDX.Direct3D11.ShaderResourceView[element.Count];

                                for (int i = 0; i < element.Count; i++)
                                    shaderResources[i] = (resourceTable.WhcihHeap.Elements[i + resourceTable.Start] as ShaderResource).ID3D11ShaderResourceView;

                                Engine.ImmediateContext.VertexShader.SetShaderResources(element.Register, shaderResources);
                                Engine.ImmediateContext.PixelShader.SetShaderResources(element.Register, shaderResources);
                                break;
                            default:
                                ReportError(index, ResourceType.Unknown, element.Type);
                                break;
                        }

                        break;
                    default:
                        break;
                }
            } 
        }
        
        public void Reset()
        {

        }
    }

    public static partial class GraphicsPipeline
    {
        private static ResourceInputIndexer resouceInput = new ResourceInputIndexer();

        public static ResourceInputIndexer InputSlot => resouceInput;

        public static void SetHeaps(ResourceHeap[] heaps)
        {
            
        }
    }
}
