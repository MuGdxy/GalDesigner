using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Position<T>
    {
        public T X { get; }
        public T Y { get; }

        public Position(T x = default(T),
            T y = default(T))
        {
            X = x;
            Y = y;
        }
    }
}
