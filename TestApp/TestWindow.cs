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
        private CanvasTextFormat textFormat = new CanvasTextFormat("Consolas", 12);

       
        public TestWindow(string Title, int Width, int Height) : base(Title, Width, Height)
        {
            present = new Present(Handle);

            textureFace = new TextureFace(Width, Height);

            Show();
        }

        public override void OnUpdate(object sender)
        {
            Canvas.BeginDraw(present);

            Canvas.EndDraw();
            
            present.SwapBuffer();

            base.OnUpdate(sender);
        }

    }
}
