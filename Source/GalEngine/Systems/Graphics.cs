using System;
using System.Numerics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Systems
{
    static class Graphics
    {
        private static Dictionary<string, SharpDX.Direct2D1.Brush> colorBrush = 
            new Dictionary<string, SharpDX.Direct2D1.Brush>();

        private static Dictionary<string, SharpDX.DirectWrite.TextFormat> fontFace =
            new Dictionary<string, SharpDX.DirectWrite.TextFormat>();

        internal static SharpDX.Direct2D1.Factory1 factory;
        internal static SharpDX.Direct2D1.Device device2D;
        internal static SharpDX.Direct2D1.DeviceContext deviceContext2D;

        internal static SharpDX.DirectWrite.Factory writeFactory;
        internal static SharpDX.WIC.ImagingFactory imagingFactory;

        internal static SharpDX.Direct3D11.Device device3D;
        internal static SharpDX.Direct3D11.DeviceContext deviceContext3D;

        internal static void SetColorBrush(string ColorName, Color Color)
        {
            colorBrush[ColorName] = new SharpDX.Direct2D1.SolidColorBrush(deviceContext2D,
                new SharpDX.Mathematics.Interop.RawColor4(Color.Red, Color.Green, Color.Blue, Color.Alpha));
        }

        internal static void SetFontFace(string FontName, Font Font)
        {
            fontFace[FontName] = new SharpDX.DirectWrite.TextFormat(writeFactory, Font.FontName,
                (SharpDX.DirectWrite.FontWeight)Font.FontWeight,
                (SharpDX.DirectWrite.FontStyle)Font.FontStyle, Font.FontSize);

            fontFace[FontName].SetTextAlignment(SharpDX.DirectWrite.TextAlignment.Center);
            fontFace[FontName].SetParagraphAlignment(SharpDX.DirectWrite.ParagraphAlignment.Center);
        }

        internal static void CancelColorBrush(string ColorName)
        {
            colorBrush[ColorName].Dispose();
            colorBrush.Remove(ColorName);
        }

        internal static void CancelFontFace(string FontName)
        {
            fontFace[FontName].Dispose();
            fontFace.Remove(FontName);
        }

        internal static SharpDX.Direct2D1.Brush GetColorBrush(string ColorName)
        {
            return colorBrush[ColorName];
        }

        internal static SharpDX.DirectWrite.TextFormat GetFontFace(string FontName)
        {
            return fontFace[FontName];
        }

        private static void CreateInterface()
        {
            factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);
            
            device3D = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                 SharpDX.Direct3D11.DeviceCreationFlags.Debug | SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);

            deviceContext3D = device3D.ImmediateContext;

            writeFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);
            imagingFactory = new SharpDX.WIC.ImagingFactory();

            using (var dxgiDevice = device3D.QueryInterface<SharpDX.DXGI.Device>())
            {
                device2D = new SharpDX.Direct2D1.Device(dxgiDevice);
                deviceContext2D = new SharpDX.Direct2D1.DeviceContext(device2D, SharpDX.Direct2D1.DeviceContextOptions.EnableMultithreadedOptimizations);
            }

        }

        static Graphics()
        {
            CreateInterface();
            
            SetColorBrush(GameDefault.Color, new Color());
            SetFontFace(GameDefault.Font, new Font());
        }

        public static void BeginDraw(Bitmap Target)
        {
            deviceContext2D.Target = Target.resource as SharpDX.Direct2D1.Bitmap1;
            deviceContext2D.BeginDraw();
        }

        public static void EndDraw()
        {
            deviceContext2D.EndDraw();
            deviceContext2D.Target = null;
        }

        public static void Clear(Color ClearColor)
        {
            deviceContext2D.Clear(new SharpDX.Mathematics.Interop.RawColor4(ClearColor.Red, ClearColor.Green,
                ClearColor.Blue, ClearColor.Alpha));
        }

        public static void SetTransform(Matrix3x2 Transform)
        {
            deviceContext2D.Transform = new SharpDX.Mathematics.Interop.RawMatrix3x2(
                Transform.M11, Transform.M12, Transform.M21, Transform.M22, Transform.M31, Transform.M32);
        }

        public static void CreateBitmap(int Width, int Height, out object Resource)
        {
            Resource = new SharpDX.Direct2D1.Bitmap1(deviceContext2D, new SharpDX.Size2(Width, Height),
                new SharpDX.Direct2D1.BitmapProperties1()
                {
                    BitmapOptions = SharpDX.Direct2D1.BitmapOptions.Target,
                    PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied)
                });
        }

        public static void CreateBitmap(Stream BitmapStream, out object Resource, out Size Size)
        {
            using (var bitmapDecoder = new SharpDX.WIC.BitmapDecoder(imagingFactory, BitmapStream,
                 SharpDX.WIC.DecodeOptions.CacheOnLoad))
            {
                using (var bitmapFrame = bitmapDecoder.GetFrame(0))
                {
                    using (var bitmapConverter = new SharpDX.WIC.FormatConverter(imagingFactory))
                    {
                        bitmapConverter.Initialize(bitmapFrame, SharpDX.WIC.PixelFormat.Format32bppPBGRA,
                             SharpDX.WIC.BitmapDitherType.None, null, 0, SharpDX.WIC.BitmapPaletteType.MedianCut);

                        Resource = SharpDX.Direct2D1.Bitmap1.FromWicBitmap(deviceContext2D, bitmapConverter);
                    }
                }
            }

            Size = new Size(
                (int)(Resource as SharpDX.Direct2D1.Bitmap1).Size.Width,
                (int)(Resource as SharpDX.Direct2D1.Bitmap1).Size.Height);

        }

        public static void DrawLine(PositionF Start, PositionF End, string ColorName, float LineWidth = 1.0f)
        {
            deviceContext2D.DrawLine(
                new SharpDX.Mathematics.Interop.RawVector2(Start.X, Start.Y),
                new SharpDX.Mathematics.Interop.RawVector2(End.X, End.Y),
                GetColorBrush(ColorName), LineWidth);
        }

        public static void DrawRectangle(RectangleF Rectangle, string ColorName, float LineWidth = 1.0f)
        {
            deviceContext2D.DrawRectangle(
                new SharpDX.Mathematics.Interop.RawRectangleF(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom),
                GetColorBrush(ColorName), LineWidth);
        }

        public static void FillRectangle(RectangleF Rectangle, string ColorName)
        {
            deviceContext2D.FillRectangle(
                new SharpDX.Mathematics.Interop.RawRectangleF(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom),
                GetColorBrush(ColorName));
        }

        public static void DrawBitmap(Bitmap Bitmap, RectangleF Rectangle)
        {
            deviceContext2D.DrawBitmap(Bitmap.resource as SharpDX.Direct2D1.Bitmap1,
                new SharpDX.Mathematics.Interop.RawRectangleF(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom),
                1.0f, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }

        public static void DrawText(string Text, RectangleF Rectangle, string FontName, string ColorName)
        {
            deviceContext2D.DrawText(Text, GetFontFace(FontName),
                new SharpDX.Mathematics.Interop.RawRectangleF(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom),
                GetColorBrush(ColorName), SharpDX.Direct2D1.DrawTextOptions.Clip);
        }

        public static void DestoryBitmap(ref object resource)
        {
            if (resource == null) return;

            (resource as SharpDX.Direct2D1.Bitmap1).Dispose();
            resource = null;
        }
    }
}
