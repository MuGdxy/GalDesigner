using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace Presenter
{
    public class ConstantBuffer<T> : Buffer where T : struct
    {
        public ConstantBuffer(int dataCount = 1)
        {
            resource = new SharpDX.Direct3D11.Buffer(Engine.ID3D11Device,
                size = dataCount * SharpDX.Utilities.SizeOf<T>(), SharpDX.Direct3D11.ResourceUsage.Default,
                SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
                SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            count = dataCount;
        }

        public ConstantBuffer(T data)
        {
            resource = new SharpDX.Direct3D11.Buffer(Engine.ID3D11Device,
                size = Marshal.SizeOf<T>(), SharpDX.Direct3D11.ResourceUsage.Default,
                SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
                SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(ref data);

            count = 1;
        }

        public ConstantBuffer(T[] data)
        {
            resource = new SharpDX.Direct3D11.Buffer(Engine.ID3D11Device,
                size = Marshal.SizeOf<T>() * data.Length, SharpDX.Direct3D11.ResourceUsage.Default,
                SharpDX.Direct3D11.BindFlags.ConstantBuffer, SharpDX.Direct3D11.CpuAccessFlags.None,
                SharpDX.Direct3D11.ResourceOptionFlags.None, 0);

            Update(data);

            count = data.Length;
        }

    }




    
}
