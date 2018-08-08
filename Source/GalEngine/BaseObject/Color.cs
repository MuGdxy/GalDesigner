using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Color
    {
        private Vector4 color = Vector4.Zero;

        public float Red { get => color.X; set => color.X = value; }
        public float Green { get => color.Y; set => color.Y = value; }
        public float Blue { get => color.Z; set => color.Z = value; }
        public float Alpha { get => color.W; set => color.W = value; }
        public Vector4 Vector { get => color; set => color = value; }

        public Color(float Red = 0.0f, float Green = 0.0f, float Blue = 0.0f, float Alpha = 1.0f)
        {
            color = new Vector4(Red, Green, Blue, Alpha);
        }

        public Color(Vector4 Vector)
        {
            color = Vector;
        }
    }
}
