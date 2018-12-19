using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Size<T>
    {
        public T Width { get; }
        public T Height { get; }

        public Size(T width = default(T), T height = default(T))
        {
            Width = width;
            Height = height;
        }
    }
}
