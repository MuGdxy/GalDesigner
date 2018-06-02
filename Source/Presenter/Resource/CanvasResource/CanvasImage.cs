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
            Reset(width, height);
        }

        public CanvasImage(string fileName)
        {
            SharpDX.WIC.BitmapDecoder decoder = new SharpDX.WIC.BitmapDecoder(Engine.ImagingFactory,
                fileName, SharpDX.IO.NativeFileAccess.Read, SharpDX.WIC.DecodeOptions.CacheOnLoad);

            SharpDX.WIC.BitmapFrameDecode frame = decoder.GetFrame(0);

            SharpDX.WIC.FormatConverter converter = new SharpDX.WIC.FormatConverter(Engine.ImagingFactory);

            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPBGRA,
                SharpDX.WIC.BitmapDitherType.None, null, 0, SharpDX.WIC.BitmapPaletteType.MedianCut);

            bitmap = SharpDX.Direct2D1.Bitmap1.FromWicBitmap(Canvas.ID2D1DeviceContext, converter);

            iWidth = (int)bitmap.Size.Width;
            iHeight = (int)bitmap.Size.Height;

            SharpDX.Utilities.Dispose(ref converter);
            SharpDX.Utilities.Dispose(ref frame);
            SharpDX.Utilities.Dispose(ref decoder);
        }

        public void Reset(int width, int height)
        {
            SharpDX.Utilities.Dispose(ref bitmap);

            bitmap = new SharpDX.Direct2D1.Bitmap1(Canvas.ID2D1DeviceContext,
                new SharpDX.Size2(iWidth = width, iHeight = height));
        }

        public CanvasImage CopyTo(int left, int top, int right, int bottom)
        {
            var result = new CanvasImage(right - left, bottom - top);

            result.bitmap.CopyFromBitmap(bitmap, new SharpDX.Mathematics.Interop.RawPoint(0, 0),
                new SharpDX.Mathematics.Interop.RawRectangle(left, top, right, bottom));

            return result;
        }

        public override void Dispose()
        {
            SharpDX.Utilities.Dispose(ref bitmap);
            base.Dispose();
        }

        public static CanvasImage FromFile(string fileName) => new CanvasImage(fileName);

        public int Width => iWidth;
        public int Height => iHeight;

        internal SharpDX.Direct2D1.Bitmap ID2D1Bitmap => bitmap;

        ~CanvasImage() => SharpDX.Utilities.Dispose(ref bitmap);
    }

    public static partial class Canvas
    {
        public static void DrawImage(float left, float top, float right, float bottom,
            CanvasImage image, float opacity = 1.0f)
        {
            ID2D1DeviceContext.DrawBitmap(image.ID2D1Bitmap, new SharpDX.Mathematics.Interop.RawRectangleF(
                left, top, right, bottom), opacity, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public static void DrawImage(float left, float top, float right, float bottom,
            CanvasImage image, float sourceLeft, float sourceTop,
            float sourceRight, float sourceBottom, float opacity = 1.0f)
        {
            ID2D1DeviceContext.DrawBitmap(image.ID2D1Bitmap, new SharpDX.Mathematics.Interop.RawRectangleF(
               left, top, right, bottom), opacity, SharpDX.Direct2D1.BitmapInterpolationMode.Linear,
               new SharpDX.Mathematics.Interop.RawRectangleF(sourceLeft, sourceTop, sourceRight, sourceBottom));
        }

        public static void DrawImage(float left, float top, float right, float bottom,
            TextureFace textureFace, float opacity = 1.0f)
        {
            ID2D1DeviceContext.DrawBitmap(textureFace.CanvasTarget, new SharpDX.Mathematics.Interop.RawRectangleF(
                left, top, right, bottom), opacity, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public static void DrawImage(float left, float top, float right, float bottom,
            TextureFace textureFace, float sourceLeft, float sourceTop,
            float sourceRight, float sourceBottom, float opacity = 1.0f)
        {
            ID2D1DeviceContext.DrawBitmap(textureFace.CanvasTarget, new SharpDX.Mathematics.Interop.RawRectangleF(
               left, top, right, bottom), opacity, SharpDX.Direct2D1.BitmapInterpolationMode.Linear,
               new SharpDX.Mathematics.Interop.RawRectangleF(sourceLeft, sourceTop, sourceRight, sourceBottom));
        }

    }
}
