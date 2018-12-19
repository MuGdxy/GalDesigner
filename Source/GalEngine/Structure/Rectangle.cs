using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Rectangle<T>
    {
        public T Left { get; }
        public T Top { get; }
        public T Right { get; }
        public T Bottom { get; }

        public Rectangle(T left = default(T), T top = default(T), T right = default(T), T bottom = default(T))
        {
            Left = left;
            Top = top;
            Right = right;
            bottom = Bottom;
        }
    }
}
