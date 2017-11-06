using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Presenter;

namespace GalEngine
{
    using LayerConfig = VisualLayerConfig;

    //relative VisualLayer
    public static class DebugCommand
    {
        private const float borderX = 0.01f; //relative DebugCommand 
        
        private static float width;
        private static float height;

        private static float startPosX;
        private static float startPosY;

        private static float realWidth;
        private static float realHeight;

        private static float realStartPosX;
        private static float realStartPosY;

        private static int cursorPosition = 0;
        private static float cursorRealPosition = 0;

        private static CanvasBrush cursorBrush = new CanvasBrush(0, 0, 0, 1);

        private static CanvasText inputCommand;
        private static Rect inputCommandRect = new Rect(); //relative DebugCommand

        private static float inputCommandOffset = 0;

        static DebugCommand()
        {
            inputCommand = new CanvasText("1233", float.MaxValue, CommandInputPadHeight, LayerConfig.TextFormat);
        }

        internal static void OnRender()
        {
            Matrix3x2 transform = Matrix3x2.CreateTranslation(new Vector2(realStartPosX,
                realStartPosY));

            //Transform 
            Canvas.Transform = transform;

            //Render Console Border
            Canvas.DrawRectangle(0, 0, realWidth, realHeight, LayerConfig.BorderBrush, LayerConfig.BorderSize);

            float inputStartPosition = realHeight - CommandInputPadHeight;

            //Render Command Input Rect
            Canvas.DrawRectangle(0, inputStartPosition, realWidth,
                realHeight, LayerConfig.BorderBrush, LayerConfig.BorderSize);

            Canvas.PushLayer(inputCommandRect.Left - LayerConfig.BorderSize,
                inputCommandRect.Top, inputCommandRect.Right + LayerConfig.BorderSize, inputCommandRect.Bottom);

            Canvas.DrawText(inputCommandRect.Left + inputCommandOffset, inputStartPosition,
                inputCommand, LayerConfig.TextBrush);

            float offset = (CommandInputPadHeight - LayerConfig.TextFormat.Size) / 2.3f;

            Canvas.DrawLine(inputCommandRect.Left + cursorRealPosition, inputStartPosition + offset,
                inputCommandRect.Left + cursorRealPosition, realHeight - offset, cursorBrush, LayerConfig.BorderSize);
            
            Canvas.PopLayer();

            Canvas.Transform = Matrix3x2.Identity;
        }

        internal static void SetArea(float Width, float Height)
        {
            width = Width;
            height = Height;

            realWidth = VisualLayer.Width * width;
            realHeight = VisualLayer.Height * height;

            realStartPosX = VisualLayer.Width * startPosX;
            realStartPosY = VisualLayer.Height * startPosY;

            inputCommandRect = new Rect(borderX * realWidth, realHeight - CommandInputPadHeight,
                realWidth * (1 - borderX), realHeight);
        }

        internal static void SetPosition(float newStartPosX, float newStartPosY)
        {
            startPosX = newStartPosX;
            startPosY = newStartPosY;

            realStartPosX = VisualLayer.Width * startPosX;
            realStartPosY = VisualLayer.Height * startPosY;
        }

        private static float CommandInputPadHeight => LayerConfig.TextFormat.Size * 2.5f;
    }
}
