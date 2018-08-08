using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Position ComputePosition(Position Position, RectangleF ViewPort, Size Resolution)
        {
            Position result = new Position();

            result.X = (int)((Position.X - ViewPort.Left) / ViewPort.Width * Resolution.Width);
            result.Y = (int)((Position.Y - ViewPort.Top) / ViewPort.Height * Resolution.Height);

            return result;
        }

        public static void Dispose<T>(ref T Object) where T : class, IDisposable
        {
            if (Object == null) return;

            Object.Dispose();
            Object = null;
        }
    }
}
