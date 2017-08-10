using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace Presenter
{
    public static partial class Canvas
    {
        private static SharpDX.Direct2D1.Device device;
        private static SharpDX.Direct2D1.DeviceContext context;

        private static Matrix3x2 transformMatrix;

        static Canvas()
        {
            using (var dxgiDevice = Engine.ID3D11Device.QueryInterface<SharpDX.DXGI.Device>())
            {
                device = new SharpDX.Direct2D1.Device(dxgiDevice);
                context = new SharpDX.Direct2D1.DeviceContext(device, SharpDX.Direct2D1.DeviceContextOptions.None);
            }
        }

        public static void BeginDraw(Present present)
        {
            context.Target = present.CanvasTarget;

            context.BeginDraw();
        }

        public static void BeginDraw(TextureFace textureFace)
        {
            context.Target = textureFace.CanvasTarget;

            context.BeginDraw();
        }

        public static void EndDraw()
        {
            context.Target = null;

            context.EndDraw();
        }

        public static Matrix3x2 Transform
        {
            set
            {
                transformMatrix = value;
                context.Transform = new SharpDX.Mathematics.Interop.RawMatrix3x2()
                {
                    M11 = transformMatrix.M11,
                    M12 = transformMatrix.M12,
                    M21 = transformMatrix.M21,
                    M22 = transformMatrix.M22,
                    M31 = transformMatrix.M31,
                    M32 = transformMatrix.M32
                };
            }

            get => transformMatrix;
        }

        internal static SharpDX.Direct2D1.Device ID2D1Device => device;
        internal static SharpDX.Direct2D1.DeviceContext ID2D1DeviceContext => context;
    }
}
