using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Presenter
{
    public class VertexBuffer<T> : Buffer where T : struct
    { 
        public VertexBuffer(T[] vertices)
        {
            resource = new SharpDX.Direct3D11.Buffer(Engine.ID3D11Device,
               size = Marshal.SizeOf<T>() * vertices.Length, SharpDX.Direct3D11.ResourceUsage.Default,
               SharpDX.Direct3D11.BindFlags.VertexBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
               SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(vertices);

            count = vertices.Length;
        }

       
    }

}
