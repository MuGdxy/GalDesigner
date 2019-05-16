using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Vector2<T>
    {
        public T X { get; }
        public T Y { get; }
        
        public Vector2(
            T x = default(T),
            T y = default(T))
        {
            X = x; Y = y;
        }
    }
}
