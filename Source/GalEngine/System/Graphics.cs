using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.System
{
    static class Graphics
    {
        internal static SharpDX.Direct2D1.Factory1 factory;
        internal static SharpDX.Direct2D1.Device device2D;
        internal static SharpDX.Direct2D1.DeviceContext deviceContext2D;

        internal static SharpDX.DirectWrite.Factory writeFactory;
        internal static SharpDX.WIC.ImagingFactory imagingFactory;

        internal static SharpDX.Direct3D11.Device device3D;
        internal static SharpDX.Direct3D11.DeviceContext deviceContext3D;

        private static Bitmap target = null;

        public static Bitmap Target { get => target; set => target = value; }

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

            deviceContext2D.BeginDraw();
        }

        static Graphics()
        {
            CreateInterface();
        }

        public static void CreateBitmap(ref object Resource, int Width, int Height)
        {
            Resource = new SharpDX.Direct2D1.Bitmap1(deviceContext2D, new SharpDX.Size2(Width, Height),
                new SharpDX.Direct2D1.BitmapProperties1()
                {
                    BitmapOptions = SharpDX.Direct2D1.BitmapOptions.Target,
                    PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied)
                });
        }

        public static void DestoryBitmap(ref object resource)
        {
            if (resource == null) return;

            (resource as SharpDX.Direct2D1.Bitmap1).Dispose();
            resource = null;
        }
    }
}
