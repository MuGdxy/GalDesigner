using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.BaseStruct
{
    struct Rect
    {
        public float left;
        public float top;
        public float right;
        public float bottom;

        public Rect(float Left, float Top, float Right, float Bottom)
        {
            left = Left; top = Top; right = Right; bottom = Bottom;
        }

        public bool IsContained(float x, float y)
            => (x >= left && x <= right && y >= top && y <= bottom);
    }
}
