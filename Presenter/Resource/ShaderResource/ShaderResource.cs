using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public abstract class ShaderResource : Resource
    {
        protected SharpDX.Direct3D11.ShaderResourceView resourceview;

        protected ResourceFormat pixelFormat;

        internal SharpDX.Direct3D11.ShaderResourceView ID3D11ShaderResourceView => resourceview;

        public ResourceFormat PixelFormat => pixelFormat;

        ~ShaderResource() => SharpDX.Utilities.Dispose(ref resourceview);
    }


   
    
}
