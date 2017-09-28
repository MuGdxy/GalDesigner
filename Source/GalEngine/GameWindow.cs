using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Builder;
using Presenter;

namespace GalEngine
{

    partial class GameWindow : GenericWindow
    {
        private Present presentSurface;
        private TextureFace renderSurface;

        private int mousePosX;
        private int mousePosY;

        private void ComputeGameRect(out Rect result)
        {
            if (presentSurface.IsFullScreen is true)
            {
                float Tx = renderSurface.Width;
                float Ty = renderSurface.Height;

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
                }
                else
                {
                    //for h
                    scaleWidth = Width;
                    scaleHeight = Height * ((Ty * x) / (Tx * y));

                    offY = (Height - scaleHeight) / 2f;
                }

                result = new Rect(offX, offY, offX + scaleWidth, offY + scaleHeight);
            }else
            {
                result = new Rect(0, 0, Width, Height);
            }
        }

        private void CheckConfigValue()
        {
            bool isSizeChange = false;
            bool isAppNameChange = false;

            if (GlobalConfig.Width != Width || GlobalConfig.Height != Height)
                isSizeChange = true;

            if (GlobalConfig.AppName != Title)
                isAppNameChange = true;

            //We need to make this state be same as Present 
            GlobalConfig.IsFullScreen = IsFullScreen;

            if (isSizeChange is true)
                SetWindowSize(GlobalConfig.Width, GlobalConfig.Height);

            if (isAppNameChange is true)
                SetTitle(GlobalConfig.AppName);
        }

        private void OnRender()
        {
            renderSurface.ResetBuffer(1, 1, 1, 1);

            Canvas.BeginDraw(renderSurface);

            Canvas.EndDraw();

            Canvas.BeginDraw(presentSurface);

            //Present
            if (presentSurface.IsFullScreen is false)
                Canvas.DrawImage(0, 0, Width, Height, renderSurface);
            else
            {
                ComputeGameRect(out Rect gameRect);

                Canvas.DrawImage(gameRect.Left, gameRect.Top, gameRect.Right, gameRect.Bottom, renderSurface);
            }

            Canvas.EndDraw();

        }

        private void ComputeMousePos(Rect gameRect, int x, int y, ref int resultX, ref int resultY)
        {
            float width = gameRect.Right - gameRect.Left;
            float height = gameRect.Bottom - gameRect.Top;

            resultX = (int)(renderSurface.Width * (x - gameRect.Left) / width);
            resultY = (int)(renderSurface.Height * (y - gameRect.Top) / height);
        }

        public GameWindow(string Title, int Width, int Height) : base(Title, Width, Height)
        {
            presentSurface = new Present(Handle, Width, Height);
            renderSurface = new TextureFace(Width, Height);

            IsVisible = true;
        }

        public override void OnSizeChange(object sender, SizeChangeEventArgs e)
        {
            //On Create Window
            if (presentSurface is null) return;

            //Resize Buffer
            presentSurface.ResizeBuffer(e.NextWidth, e.NextHeight);

            base.OnSizeChange(sender, e);
        }

        public override void OnMouseClick(object sender, MouseClickEventArgs e)
        {
            ComputeGameRect(out Rect gameRect);

            if (gameRect.IsContained(e.X, e.Y) is false) { mousePosX = -1; mousePosY = -1; return; }

            ComputeMousePos(gameRect, e.X, e.Y, ref mousePosX, ref mousePosY);

            base.OnMouseClick(sender, e);
        }

        public override void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            ComputeGameRect(out Rect gameRect);

            if (gameRect.IsContained(e.X, e.Y) is false) { mousePosX = -1; mousePosY = -1; return; }

            ComputeMousePos(gameRect, e.X, e.Y, ref mousePosX, ref mousePosY);
            
            base.OnMouseMove(sender, e);
        }

        public override void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ComputeGameRect(out Rect gameRect);

            if (gameRect.IsContained(e.X, e.Y) is false) { mousePosX = -1; mousePosY = -1; return; }

            ComputeMousePos(gameRect, e.X, e.Y, ref mousePosX, ref mousePosY);

            base.OnMouseWheel(sender, e);
        }

        public override void OnUpdate(object sender)
        {
            //Check State
            CheckConfigValue();

            //Render And Present
            OnRender();

            presentSurface.SwapBuffer();

            base.OnUpdate(sender);
        }

        public bool IsFullScreen
        {
            set => presentSurface.IsFullScreen = value;
            get => presentSurface.IsFullScreen;
        }

    }
}
