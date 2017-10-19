using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{

    //Current Version, we disable this.
    class CanvasLayer : CanvasResource
    {
        private SharpDX.Direct2D1.Layer layer;

        private float left;
        private float top;
        private float right;
        private float bottom;

        public CanvasLayer(float Left, float Top, float Right, float Bottom)
        {
            Reset(Left, Top, Right, Bottom);
        }

        public void Reset(float Left, float Top, float Right, float Bottom)
        {
            SharpDX.Utilities.Dispose(ref layer);

            layer = new SharpDX.Direct2D1.Layer(Canvas.ID2D1DeviceContext,
                new SharpDX.Size2F((right = Right) - (left = Left), (bottom = Bottom) - (top = Top)));
        }

        public float Left => left;
        public float Top => top;
        public float Right => right;
        public float Bottom => bottom;

        public override void Dispose()
        {
            SharpDX.Utilities.Dispose(ref layer);
            base.Dispose();
        }

        ~CanvasLayer() => SharpDX.Utilities.Dispose(ref layer);
    }
}
