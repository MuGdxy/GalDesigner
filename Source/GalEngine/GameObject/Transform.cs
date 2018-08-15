using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Transform
    {
        private PositionF position = new PositionF(0.0f, 0.0f);
        private SizeF scale = new SizeF(1.0f, 1.0f);
        private Vector2 forward = new Vector2(0.0f, 1.0f);
        private float angle = 0.0f;

        private Matrix3x2 matrix = Matrix3x2.Identity;

        public PositionF Position { get => position; set => position = value; }
        public SizeF Scale { get => scale; set => scale = value; }

        public Vector2 Forward
        {
            get => forward; set
            {
                forward = Vector2.Normalize(value);

                angle = (float)Math.Atan2(-forward.X, forward.Y);
            }
        }

        public float Angle
        {
            get => angle; set
            {
                angle = value;

                forward = Vector2.Transform(new Vector2(0.0f, 1.0f), Matrix3x2.CreateRotation(angle));
                forward = Vector2.Normalize(forward);
            }
        }

        public Matrix3x2 Matrix => matrix;

        internal void Update(SizeF Size)
        {
            PositionF center = new PositionF(Position.X + Size.Width * 0.5f, Position.Y + Size.Height * 0.5f);

            matrix = CreateMatrixFromTransform(this, center);
        }

        public static Matrix3x2 CreateMatrixFromTransform(Transform Transform, PositionF Center)
        {
            Matrix3x2 matrix = Matrix3x2.Identity;

            matrix = matrix * Matrix3x2.CreateTranslation(Transform.Position.Vector);
            matrix = matrix * Matrix3x2.CreateScale(Transform.Scale.Vector, Center.Vector);
            matrix = matrix * Matrix3x2.CreateRotation(Transform.Angle, Center.Vector);

            return matrix;
        }
    }
}
