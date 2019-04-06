using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.GameResource
{
    public class Shape
    {
        public virtual bool Contain(Position<float> position)
        {
            return false;
        }
    }

    public class ShapeDebugProperty
    {
        public float Padding { get; set; }
        public Color<float> Color { get; set; }

        public ShapeDebugProperty(float padding, Color<float> color)
        {
            Padding = padding;
            Color = color;
        }
    }

    public class RectangleShape : Shape
    {
        public Size<float> Size { get; set; }

        public RectangleShape() : this(new Size<float>())
        {

        }

        public RectangleShape(Size<float> area)
        {
            Size = area; 
        }

        public override bool Contain(Position<float> position)
        {
            //local space
            if (position.X < 0 || position.X > Size.Width) return false;
            if (position.Y < 0 || position.Y > Size.Height) return false;

            return true;
        }
    }
}
