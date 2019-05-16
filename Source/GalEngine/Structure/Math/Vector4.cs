using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Vector4<T>
    {
        public T X { get; }
        public T Y { get; }
        public T Z { get; }
        public T W { get; }

        public Vector4(
            T x = default(T), 
            T y = default(T), 
            T z = default(T), 
            T w = default(T))
        {
            X = x; Y = y; Z = z; W = w;
        }
    }
}
