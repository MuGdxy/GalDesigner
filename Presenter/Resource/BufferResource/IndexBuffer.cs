using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Presenter
{
    public class IndexBuffer<T> : Buffer where T : struct
    {
        public IndexBuffer(T[] indices)
        {
            resource = new SharpDX.Direct3D11.Buffer(Engine.ID3D11Device,
               size = Marshal.SizeOf<T>() * indices.Length, SharpDX.Direct3D11.ResourceUsage.Default,
               SharpDX.Direct3D11.BindFlags.IndexBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
               SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(indices);

            count = indices.Length;
        }
    }
}
