using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class Utility
    {
        public static RectangleF ComputeViewPort(int Width, int Height, int ResolutionWidth, int ResolutionHeight)
        {
            if (Width * ResolutionHeight == Height * ResolutionWidth) return new RectangleF(0, 0, Width, Height);
            
            float Tx = ResolutionWidth;
            float Ty = ResolutionHeight;

            float x = Width;
            float y = Height;

            float scaleWidth = 0;
            float scaleHeight = 0;

            float offX = 0;
            float offY = 0;

            if (Ty * x > Tx * y)
            {
                //for width
                scaleWidth = Width / ((Ty * x) / (Tx * y));
                scaleHeight = Height;

                offX = (Width - scaleWidth) / 2;
            }
            else
            {
                //for height
                scaleWidth = Width;
                scaleHeight = Height * ((Ty * x) / (Tx * y));

                offY = (Height - scaleHeight) / 2;
            }
            
            return new RectangleF(offX, offY, offX + scaleWidth, offY + scaleHeight);
        }

        public static RectangleF ComputeViewPort(Size Size, Size Resolution)
        {
            return ComputeViewPort(Size.Width, Size.Height, Resolution.Width, Resolution.Height);
        }

        public static Position ComputePosition(PositionF Position, RectangleF ViewPort, Size Resolution)
        {
            Position result = new Position();

            result.X = (int)((Position.X - ViewPort.Left) / ViewPort.Width * Resolution.Width);
            result.Y = (int)((Position.Y - ViewPort.Top) / ViewPort.Height * Resolution.Height);

            return result;
        }

        public static Position ComputePosition(PositionF Position, Camera Camera, Size Resolution)
        {
            PositionF result = new PositionF();

            result.X = Position.X / Resolution.Width * Camera.Size.Width;
            result.Y = Position.Y / Resolution.Height * Camera.Size.Height;

            return result;
        }

        public static bool IsContain(Position Position, Size Size, Matrix3x2 TransformMatrix)
        {
            Matrix3x2.Invert(TransformMatrix, out Matrix3x2 invMatrix);

            var transformPosition = Vector2.Transform(Position.Vector, invMatrix);

            if (transformPosition.X >= 0 && transformPosition.X <= Size.Width &&
                transformPosition.Y >= 0 && transformPosition.Y <= Size.Height)
                return true;

            return false;
        }

        public static void Project(Vector2[] Position, Vector2 Axis, out float minPosition, out float maxPosition)
        {
            minPosition = float.MaxValue;
            maxPosition = float.MinValue;
            
            foreach (var item in Position)
            {
                float dotValue = Vector2.Dot(item, Axis);

                minPosition = Math.Min(minPosition, dotValue);
                maxPosition = Math.Max(maxPosition, dotValue);
            }
        }

        public static bool IsIntersect(Camera Camera, Size Size, Matrix3x2 TransformMatrix)
        {
            Vector2[] axes = new Vector2[4];
            Vector2[] position = new Vector2[4];
            Vector2[] cameraPosition = new Vector2[4];

            position[0] = new Vector2(0, 0);
            position[1] = new Vector2(0, Size.Height);
            position[2] = new Vector2(Size.Width, Size.Height);
            position[3] = new Vector2(Size.Width, 0);

            for (int i = 0; i < 4; i++)
                position[i] = Vector2.Transform(position[i], TransformMatrix);

            cameraPosition[0] = new Vector2(Camera.Area.Left, Camera.Area.Top);
            cameraPosition[1] = new Vector2(Camera.Area.Left, Camera.Area.Bottom);
            cameraPosition[2] = new Vector2(Camera.Area.Right, Camera.Area.Bottom);
            cameraPosition[3] = new Vector2(Camera.Area.Right, Camera.Area.Top);

            axes[0] = Vector2.Normalize(position[0] - position[1]);
            axes[1] = Vector2.Normalize(position[1] - position[2]);
            axes[2] = Vector2.Normalize(cameraPosition[0] - cameraPosition[1]);
            axes[3] = Vector2.Normalize(cameraPosition[1] - cameraPosition[2]);

            foreach (var axis in axes)
            {
                float[] minPosition = new float[2];
                float[] maxPosition = new float[2];

                Project(position, axis, out minPosition[0], out maxPosition[0]);
                Project(cameraPosition, axis, out minPosition[1], out maxPosition[1]);

                if (maxPosition[0] < minPosition[1] ||
                    maxPosition[1] < minPosition[0]) return false;
            }

            return true;
        }

        public static void Dispose<T>(ref T Object) where T : class, IDisposable
        {
            if (Object == null) return;

            Object.Dispose();
            Object = null;
        }
    }
}
