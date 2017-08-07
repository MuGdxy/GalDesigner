using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class CanvasImage : CanvasResource
    {
        private SharpDX.Direct2D1.Bitmap1 bitmap;

        private int iWidth;
        private int iHeight;

        public CanvasImage(int width, int height)
        {
            bitmap = new SharpDX.Direct2D1.Bitmap1(Canvas.ID2D1DeviceContext,
                new SharpDX.Size2(iWidth = width, iHeight = height));
        }

        public int Width => iWidth;
        public int Height => iHeight;

        internal SharpDX.Direct2D1.Bitmap ID2D1Bitmap => bitmap;

        ~CanvasImage() => SharpDX.Utilities.Dispose(ref bitmap); 
    }
}
