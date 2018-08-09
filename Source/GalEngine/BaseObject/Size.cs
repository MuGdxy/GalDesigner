using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Size
    {
        private int width;
        private int height;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public Size(int Width = 0, int Height = 0)
        {
            width = Width;
            height = Height;
        }

        public static implicit operator Size(SizeF Size)
        {
            return new Size((int)Size.Width, (int)Size.Height);
        }
    }

    public class SizeF
    {
        private Vector2 vector = new Vector2();   
        
        public float Width { get => vector.X; set => vector.X = value; }
        public float Height { get => vector.Y; set => vector.Y = value; }
        public Vector2 Vector { get => vector; set => vector = value; }

        public SizeF(float Width = 0.0f, float Height = 0.0f)
        {
            vector.X = Width;
            vector.Y = Height;
        }

        public static implicit operator SizeF(Size Size)
        {
            return new SizeF(Size.Width, Size.Height);
        }
    }

}
