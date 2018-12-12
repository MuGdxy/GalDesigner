using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsResource
    {
        private SharpDX.Direct3D11.Resource mResource;

        internal protected SharpDX.Direct3D11.Resource Resource { get => mResource; protected set => mResource = value; }

        public int Size { get; }

        public GraphicsResource(int size)
        {
            Size = size;
            Resource = null;
        }

        ~GraphicsResource()
        {
            SharpDX.Utilities.Dispose(ref mResource);
        }
    }
}
