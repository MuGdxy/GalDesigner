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
        private CanvasTextFormat textFormat = new CanvasTextFormat(@"FLOWERS", 40, 400);

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
            present?.ResizeBuffer(e.NextWidth, e.NextHeight);
            base.OnSizeChange(sender, e);
        }

        public override void OnMouseClick(object sender, MouseClickEventArgs e)
        {
            if (e.IsDown is false) return;

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
            Canvas.BeginDraw(textureFace);

            Canvas.FillRectangle(0, 0, present.Width, present.Height, whiteBrush);
            Canvas.DrawText(posX + " 告白 " + posY, 0, 0, 100, 100, textFormat, redBrush);

            Canvas.EndDraw();

            Canvas.BeginDraw(present);


            if (present.IsFullScreen is false)
                Canvas.DrawImage(0, 0, present.Width, present.Height, textureFace);
            else
            {
                float Tx = textureFace.Width;
                float Ty = textureFace.Height;

                float x = Engine.PhysicsAspectRatioMaxX;
                float y = Engine.PhysicsAspectRatioMaxY;

                float scaleHeight = 0;
                float scaleWidth = 0;

                float offX = 0;
                float offY = 0;

                if (Ty * x > Tx * y)
                {
                    //for w
                    scaleWidth = Width / ((Ty * x) / (Tx * y));
                    scaleHeight = Height;

                    offX = (Width - scaleWidth) / 2f;
                }else
                {
                    //for h
                    scaleWidth = Width;
                    scaleHeight = Height * ((Ty * x) / (Tx * y));

                    offY = (Height - scaleHeight) / 2f;
                }

                Canvas.DrawImage(offX, offY, offX + scaleWidth, offY + scaleHeight, textureFace);
                Canvas.DrawText((x).ToString() + " " + (y).ToString(), 0, 0, 100, 100, textFormat, redBrush);
            }

            Canvas.EndDraw();

            present.SwapBuffer();

            base.OnUpdate(sender);
        }

    }
}
