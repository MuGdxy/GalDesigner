using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    static class VisualLayer
    {
        private const float borderX = 0.01f;
        private const float borderY = 0.01f;

        private const float spaceSizeX = 0.02f;
        private const float spaceSizeY = 0.03f;

        private const float leftRectWidth = 0.2f;

        private static TextureFace debugSurface;

        private static CanvasBrush rectBrush = new CanvasBrush(0, 0, 0, 1);

        private static int width;
        private static int height;

        private static bool isLock = false;
        private static bool isEnable = false;

        private static float opacity = 0.7f;

        internal static void OnResolutionChange(int newWidth, int newHeight)
        {
            Utilities.Dipose(ref debugSurface);

            debugSurface = new TextureFace(width = newWidth, height = newHeight);
        }

        private const float informationRectWidth = leftRectWidth;
        private const float informationRectHeight = 0.35f;

        private static void OnRenderInformationRect()
        {
            float startPosX = borderX * width;
            float startPosY = borderY * height;

            Canvas.DrawRectangle(startPosX, startPosY,
                startPosX + width * informationRectWidth,
                startPosY + height * informationRectHeight, rectBrush);
        }

        private const float warningRectWidth = leftRectWidth;
        private const float warningRectHeight = 0.6f;

        private static void OnRenderWarningRect()
        {
            float startPosX = borderX * width;
            float startPosY = height * (borderY + informationRectHeight + spaceSizeY);

            Canvas.DrawRectangle(startPosX, startPosY,
                startPosX + width * warningRectWidth,
                startPosY + height * warningRectHeight, rectBrush);

        }

        private const float watchRectWidth = 0.7f;
        private const float watchRectHeight = informationRectHeight +
            spaceSizeY + warningRectHeight;

        private static void OnRenderWatchRect()
        {
            float startPosX = width * (borderX + leftRectWidth + spaceSizeX);
            float startPosY = height * borderY;

            Canvas.DrawRectangle(startPosX, startPosY,
                startPosX + width * watchRectWidth,
                startPosY + height * watchRectHeight, rectBrush);
        }

        internal static void OnRender()
        {
            if (isEnable is false) return;

            //Render BackGround
            debugSurface.ResetBuffer(192 / 255f, 192 / 255f, 192 / 255f, 1);

            //Render Object 
            Canvas.BeginDraw(debugSurface);

            OnRenderInformationRect();

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
