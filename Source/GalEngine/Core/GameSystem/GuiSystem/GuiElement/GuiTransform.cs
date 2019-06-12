using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiTransform
    {
        public Point2f Position { get; set; }

        public Matrix4x4 GetMatrix()
        {
            return Matrix4x4.CreateTranslation(Position.X, Position.Y, 0);
        }
    }
}
