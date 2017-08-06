using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class Resource
    {
        protected int size;

        protected SharpDX.Direct3D11.Resource resource;

        public abstract void Update<T>(ref T data) where T : struct;

        public abstract void Update<T>(T[] data) where T : struct;

        public abstract void Update(IntPtr data);

        public int Size => size;

        internal SharpDX.Direct3D11.Resource ID3D11Resource => resource;

        ~Resource() => SharpDX.Utilities.Dispose(ref resource);
    }
}
