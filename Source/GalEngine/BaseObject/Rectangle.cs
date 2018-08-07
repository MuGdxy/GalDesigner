using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Rectangle
    {
        private int left;
        private int top;
        private int right;
        private int bottom;

        public int Left { get => left; set => left = value; }
        public int Top { get => top; set => top = value; }
        public int Right { get => right; set => right = value; }
        public int Bottom { get => bottom; set => bottom = value; }

        public Rectangle(int Left = 0, int Top = 0, int Right = 0, int Bottom = 0)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }
    }

    public class RectangleF
    {
        private float left;
        private float top;
        private float right;
        private float bottom;

        public float Left { get => left; set => left = value; }
        public float Top { get => top; set => top = value; }
        public float Right { get => right; set => right = value; }
        public float Bottom { get => bottom; set => bottom = value; }

        public RectangleF(float Left = 0, float Top = 0, float Right = 0, float Bottom = 0)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }
    }
}
