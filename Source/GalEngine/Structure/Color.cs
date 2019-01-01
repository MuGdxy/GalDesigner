using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Color<T>
    {
        public T Red { get; }
        public T Green { get; }
        public T Blue { get; }
        public T Alpha { get; }

        public Color(
            T red = default(T),
            T green = default(T),
            T blue = default(T),
            T alpha = default(T))
        {
            Red = red; Green = green; Blue = blue; Alpha = alpha;
        }
    }
}
