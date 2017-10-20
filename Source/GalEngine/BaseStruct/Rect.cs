using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public struct Rect
    {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        public Rect(float left, float top, float right, float bottom)
        {
            Left = left; Top = top; Right = right; Bottom = bottom;
        }

        public bool Contains(float x, float y)
            => (x >= Left && x <= Right && y >= Top && y <= Bottom);
    }
}
