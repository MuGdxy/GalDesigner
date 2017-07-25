using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expresser
{
    public static class Engine
    {
        private static SharpDX.Direct2D1.Factory1 d2d1Factory;
        private static SharpDX.Direct2D1.Device d2d1Device;
        private static SharpDX.Direct2D1.DeviceContext d2d1Context;

        private static SharpDX.WIC.ImagingFactory imagingFactory;

        private static SharpDX.DirectWrite.Factory1 writeFactory;

        private static SharpDX.Direct3D11.Device d3d11Device;
        private static SharpDX.Direct3D11.DeviceContext d3d11Context;

        static Engine()
        {
#if DEBUG
            d3d11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware, 
                SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport | SharpDX.Direct3D11.DeviceCreationFlags.Debug);
#else
            d3d11Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware,
                SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);
#endif

            d3d11Context = d3d11Device.ImmediateContext;

            writeFactory = new SharpDX.DirectWrite.Factory1(SharpDX.DirectWrite.FactoryType.Shared);

            imagingFactory = new SharpDX.WIC.ImagingFactory();

            d2d1Factory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);
        }

    }
}
