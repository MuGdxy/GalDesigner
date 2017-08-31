using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Builder;
using Presenter;

namespace TestApp
{
    public class TestWindow : GenericWindow
    {
        private Present present;
        private TextureFace textureFace;

        private CanvasBrush redBrush = new CanvasBrush(1, 0, 0);
        private CanvasBrush whiteBrush = new CanvasBrush(1, 1, 1);
        private CanvasTextFormat textFormat = new CanvasTextFormat("Consolas", 20);

        private int posX;
        private int posY;

        public TestWindow(string Title, int Width, int Height) : base(Title, Width, Height)
        {
            Show();
            
            present = new Present(Handle, Width, Height);

            textureFace = new TextureFace(Width, Height);
        }

        public override void OnSizeChange(object sender, SizeChangeEventArgs e)
        {
            base.OnSizeChange(sender, e);
        }

        public override void OnMouseClick(object sender, MouseClickEventArgs e)
        {
            base.OnMouseClick(sender, e);
        }

        public override void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            posX = e.X;
            posY = e.Y;
            base.OnMouseMove(sender, e);
        }

        public override void OnDestroyed(object sender)
        {
            base.OnDestroyed(sender);
        }

        public override void OnUpdate(object sender)
        {
            if (present.IsFullScreen is false)
            {
                Canvas.BeginDraw(present);

                Canvas.FillRectangle(0, 0, present.Width, present.Height, whiteBrush);

                Canvas.DrawText(Width + " " + Height, 0, 0, 100, 100, textFormat, redBrush);

                Canvas.EndDraw();
            }
            else
            {
                Canvas.BeginDraw(textureFace);

                Canvas.FillRectangle(0, 0, present.Width, present.Height, whiteBrush);

                Canvas.DrawText(Width + " " + Height, 0, 0, 100, 100, textFormat, redBrush);

                Canvas.EndDraw();

                Canvas.BeginDraw(present);

                float scaleWidth = Width;
                float scaleHeight = (Height * 1080f / 1280f);

                float offX = 0;
                float offY = (Height - scaleHeight) / 2f;

                Canvas.DrawImage(offX, offY, offX + scaleWidth, offY + scaleHeight,
                    textureFace);

                Canvas.EndDraw();
            }
            present.SwapBuffer();

            base.OnUpdate(sender);
        }

    }
}
