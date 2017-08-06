﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public static class Canvas
    {
        private static SharpDX.Direct2D1.Device device;
        private static SharpDX.Direct2D1.DeviceContext context;

        static Canvas()
        {
            using (var dxgiDevice = Engine.ID3D11Device.QueryInterface<SharpDX.DXGI.Device>())
            {
                device = new SharpDX.Direct2D1.Device(dxgiDevice);
                context = new SharpDX.Direct2D1.DeviceContext(device, SharpDX.Direct2D1.DeviceContextOptions.None);
            }
        }

        internal static SharpDX.Direct2D1.Device ID2D1Device => device;
        internal static SharpDX.Direct2D1.DeviceContext ID2D1DeviceContext => context;
    }
}
