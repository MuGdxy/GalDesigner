using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class CanvasBrush : CanvasResource
    {
        private SharpDX.Direct2D1.Brush brush;

        public CanvasBrush(float red, float green, float blue, float alpha = 1)
        {
            brush = new SharpDX.Direct2D1.SolidColorBrush(Canvas.ID2D1DeviceContext,
                new SharpDX.Mathematics.Interop.RawColor4(red, green, blue, alpha));
        }

        internal SharpDX.Direct2D1.Brush ID2D1Brush => brush;

        ~CanvasBrush() => SharpDX.Utilities.Dispose(ref brush);
    }
}
