using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public static class Engine
    {
        private static SharpDX.Direct2D1.Factory1 d2d1Factory;

        private static SharpDX.DirectWrite.Factory writeFactory;

        private static SharpDX.WIC.ImagingFactory imagingFactory;

        private static SharpDX.XAudio2.XAudio2 xAudio;
        private static SharpDX.XAudio2.MasteringVoice masteringVoice;

        private static SharpDX.Direct3D11.Device device;
        private static SharpDX.Direct3D11.DeviceContext immediateContext;

        static Engine()
        {
#if DEBUG
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                 SharpDX.Direct3D11.DeviceCreationFlags.Debug | SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);
          
            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);
#else
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport); 

            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);
#endif
            immediateContext = ID3D11Device.ImmediateContext;

            writeFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);
          
            ImagingFactory = new SharpDX.WIC.ImagingFactory();

            CreateAudio();
        }

        /*public static void Start()
        {
#if DEBUG
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                 SharpDX.Direct3D11.DeviceCreationFlags.Debug | SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);

            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);
#else
            ID3D11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport); 

            ID2D1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);
#endif
            immediateContext = ID3D11Device.ImmediateContext;

            writeFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);

            ImagingFactory = new SharpDX.WIC.ImagingFactory();
        }*/

        public static void Stop()
        {
            SharpDX.Utilities.Dispose(ref device);
            SharpDX.Utilities.Dispose(ref d2d1Factory);
            SharpDX.Utilities.Dispose(ref immediateContext);
            SharpDX.Utilities.Dispose(ref writeFactory);
            SharpDX.Utilities.Dispose(ref imagingFactory);

            DestoryAudio();
        }

        internal static SharpDX.Direct2D1.Factory1 ID2D1Factory
        {
            private set => d2d1Factory = value;
            get => d2d1Factory;
        }

        internal static SharpDX.DirectWrite.Factory WriteFactory => writeFactory;

        internal static SharpDX.XAudio2.XAudio2 XAudio => xAudio;

        internal static SharpDX.XAudio2.MasteringVoice MasteringVoice => masteringVoice;

        internal static SharpDX.WIC.ImagingFactory ImagingFactory
        {
            private set => imagingFactory = value;
            get => imagingFactory;
        }

        internal static SharpDX.Direct3D11.Device ID3D11Device
        {
            private set => device = value;
            get => device;
        }

        internal static SharpDX.Direct3D11.DeviceContext ImmediateContext
            => immediateContext;

        internal static void CreateAudio()
        { 
            xAudio = new SharpDX.XAudio2.XAudio2();
            masteringVoice = new SharpDX.XAudio2.MasteringVoice(xAudio);
        }

        internal static void DestoryAudio()
        {
            masteringVoice.DestroyVoice();
            masteringVoice.Dispose();
            xAudio.Dispose();
        }

        public static float DpiX => d2d1Factory.DesktopDpi.Width;
        public static float DpiY => d2d1Factory.DesktopDpi.Height;

        public static float AppScale => (DpiX + DpiY) / 192;

        

    }
}
