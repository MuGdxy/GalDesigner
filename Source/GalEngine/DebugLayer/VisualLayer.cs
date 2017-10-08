using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    static partial class VisualLayer
    {
        private class VisualPad
        {
            private float width;
            private float height;

            private float startPosX;
            private float startPosY;

            public void OnRender()
            {
                float realStartPosX = startPosX * VisualLayer.width;
                float realStartPosY = startPosY * VisualLayer.height;

                float realWidth = width * VisualLayer.width;
                float realHeight = height * VisualLayer.height;

                Canvas.DrawRectangle(realStartPosX, realStartPosY,
                    realStartPosX + realWidth,
                    realStartPosY + realHeight, rectBrush,2);

            }

            public void SetArea(float Width, float Height)
            {
                width = Width;
                height = Height;
            }

            public void SetAreaKeeper(float baseValue, float targetAspectRatio, bool isWidth = true)
            {
                if (isWidth is true)
                {
                    width = baseValue;
                    height = width / targetAspectRatio * aspectRatio;
                }else

                {
                    height = baseValue;
                    width = height * targetAspectRatio / aspectRatio;
                }
            }

            public void SetPosition(float posX, float posY)
            {
                startPosX = posX;
                startPosY = posY;
            }

            public float Width => height;

            public float Height => height;

            public float StartPosX => startPosX;

            public float StartPosY => startPosY;
        }
    }

    static partial class VisualLayer
    {
        private const float borderX = 0.01f;
        private const float borderY = 0.01f;
        
        private static TextureFace debugSurface;

        private static CanvasBrush rectBrush = new CanvasBrush(0, 0, 0, 1);

        private static VisualPad infomationPad = new VisualPad();
        private static VisualPad warningPad = new VisualPad();
        private static VisualPad watchPad = new VisualPad();

        private static int width;
        private static int height;

        private static float aspectRatio;

        private static bool isEnable = false;

        private static float opacity = 0.5f;

        private static void UpdatePads(int newWidth, int newHeight)
        {
            if (newWidth > newHeight)
            {
                //const
                const float leftAspectRatio = 16f / 9f;

                //Width > Height

                //Update Information Pad
                infomationPad.SetAreaKeeper(0.2f, leftAspectRatio, false);
                infomationPad.SetPosition(borderX, borderY);

                //Update Warning Pad 
                warningPad.SetAreaKeeper(0.7f, leftAspectRatio, false);
                warningPad.SetPosition(borderX, borderY + infomationPad.Height + borderY);
                
            }
            else
            {

            }
        }

        static VisualLayer()
        {

        }

        internal static void OnResolutionChange(int newWidth, int newHeight)
        {
            aspectRatio = newWidth / (float)newHeight;

            Utilities.Dipose(ref debugSurface);

            debugSurface = new TextureFace(width = newWidth, height = newHeight);

            UpdatePads(width, height);
        }

        private static void OnRenderInformationPad()
        {
            infomationPad.OnRender();
        }

        private static void OnRenderWarningRect()
        {
            warningPad.OnRender();
        }

        private static void OnRenderWatchRect()
        {

        }

        internal static void OnRender()
        {
            if (isEnable is false) return;

            //Render BackGround
            debugSurface.ResetBuffer(192 / 255f, 192 / 255f, 192 / 255f, 1);

            //Render Object 
            Canvas.BeginDraw(debugSurface);

            OnRenderInformationPad();

            OnRenderWarningRect();

            OnRenderWatchRect();

            Canvas.EndDraw();


            //Render DebugSurface
            Canvas.BeginDraw(GalEngine.RenderSurface);

            Canvas.DrawImage(0, 0, width, height, debugSurface, opacity);

            Canvas.EndDraw();
        }

        public static void Enable()
        {
            isEnable = true;
        }

        public static void UnEnable()
        {
            isEnable = false;
        }

        public static bool IsEnable
        {
            set => isEnable = value;
            get => isEnable;
        }

        internal static TextureFace DebugSurface => debugSurface;
    }
}
