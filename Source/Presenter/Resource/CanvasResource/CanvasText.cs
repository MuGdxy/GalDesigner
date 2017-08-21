using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class CanvasText : CanvasResource
    {
        private SharpDX.DirectWrite.TextLayout textLayout;

        private float tWidth;
        private float tHeight;

        private string tText;

        private CanvasTextFormat textFormat;

        public CanvasText(string text, float width, float height,
            CanvasTextFormat format)
        {
            textLayout = new SharpDX.DirectWrite.TextLayout(Engine.WriteFactory,
                tText = text, (textFormat = format).TextFormat, tWidth = width, tHeight = height);
        }

        public override void Dispose()
        {
            SharpDX.Utilities.Dispose(ref textLayout);
            base.Dispose();
        }

        internal SharpDX.DirectWrite.TextLayout TextLayout => textLayout;

        public CanvasTextFormat TextFormat => textFormat;

        ~CanvasText() => SharpDX.Utilities.Dispose(ref textLayout);

    }

    public static partial class Canvas
    {
        public static void DrawText(float posX, float posY, CanvasText text,
            CanvasBrush brush)
        {
            ID2D1DeviceContext.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2(posX, posY),
                text.TextLayout, brush.ID2D1Brush);
        }

        public static void DrawText(string text, float left, float top, float right, float bottom,
            CanvasTextFormat format, CanvasBrush brush)
        {
            ID2D1DeviceContext.DrawText(text, format.TextFormat,
                new SharpDX.Mathematics.Interop.RawRectangleF(left, top, right, bottom),
                brush.ID2D1Brush);
        }
    }
}
