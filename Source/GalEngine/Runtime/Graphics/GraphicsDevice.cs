using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public class GraphicsDevice
    {
        internal SharpDX.Direct3D11.Device Device { get; }
        internal SharpDX.Direct3D11.DeviceContext ImmediateContext { get; }

        public GraphicsDevice(GraphicsAdapter adapter)
        {
#if DEBUG
            var creationFlags = SharpDX.Direct3D11.DeviceCreationFlags.Debug;
#else
            var creationFlags = SharpDX.Direct3D11.DeviceCreationFlags.None;
#endif
            var fetuares = new SharpDX.Direct3D.FeatureLevel[3]
            {
                 SharpDX.Direct3D.FeatureLevel.Level_10_1,
                 SharpDX.Direct3D.FeatureLevel.Level_11_0,
                 SharpDX.Direct3D.FeatureLevel.Level_12_0
            };

            Device = new SharpDX.Direct3D11.Device(adapter.Adapter, creationFlags, fetuares);
            ImmediateContext = Device.ImmediateContext;
        }
    }
}
