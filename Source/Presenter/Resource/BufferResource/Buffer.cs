using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Buffer : Resource
    {
        protected int count;

        public int Count => count;

        public override void Update<T>(ref T data)
        {
            Engine.ImmediateContext.UpdateSubresource(ref data, resource);
        }

        public override void Update<T>(T[] data)
        {
            Engine.ImmediateContext.UpdateSubresource(data, resource);
        }

        public override void Update(IntPtr data)
        {
            Engine.ImmediateContext.UpdateSubresource(resource, 0, null,
                data, size, size);
        }

        internal SharpDX.Direct3D11.Buffer ID3D11Buffer => resource as SharpDX.Direct3D11.Buffer;
    }

    public static partial class GraphicsPipeline
    {
        public static void PutObject(int vertexCount, int startLocation = 0)
        {
            Engine.ImmediateContext.DrawInstanced(vertexCount, 1, startLocation, 0);
        }

        public static void PutObjectIndexed(int indexCount, int startLocation = 0,
            int baseVertexLocation = 0)
        {
            Engine.ImmediateContext.DrawIndexedInstanced(indexCount, 1, startLocation, baseVertexLocation, 0);
        }
    }


}
