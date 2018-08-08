using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Position
    {
        private int x;
        private int y;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public Position(int X = 0, int Y = 0)
        {
            x = X;
            y = Y;
        }
    }

    public class PositionF
    {
        private Vector2 position = Vector2.Zero;

        public float X { get => position.X; set => position.X = value; }
        public float Y { get => position.Y; set => position.Y = value; }
        public Vector2 Vector { get => position; set => position = value; }

        public PositionF(float X = 0.0f, float Y = 0.0f)
        {
            position = new Vector2(X, Y);
        }

        public PositionF(Vector2 Position)
        {
            position = Position;
        }
    }
}
