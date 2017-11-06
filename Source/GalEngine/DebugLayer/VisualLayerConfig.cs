using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    static class VisualLayerConfig
    {
        private const string titleFormatFont = "Consolas";
        private const int titleFormatWeight = 500;

        private const float borderSize = 0.001f;

        private const float titleHeight = 0.05f;
        private const float titleOffsetX = 0.01f;

        private const float offsetSpeed = 0.02f;

        public static CanvasBrush BorderBrush = new CanvasBrush(0, 0, 0, 1);
        public static CanvasBrush TextBrush = new CanvasBrush(0, 0, 0, 1);
        public static CanvasBrush BackGroundBrush = new CanvasBrush(1, 1, 1, 1);
        public static CanvasBrush TitleBrush = new CanvasBrush(135 / 255f, 113 / 255f, 207 / 255f, 1);

        public static CanvasTextFormat TextFormat = new CanvasTextFormat("Consolas", 10);

        public static CanvasTextFormat TitleFormat = new CanvasTextFormat(TitleFormatFont, 10, TitleFormatWeight);

        public static void OnResizeResource(int newWidth, int newHeight)
        {
            TextFormat.Reset("Consolas", newHeight * 0.027f);

            TitleFormat.Reset(titleFormatFont, newHeight * 0.03f, titleFormatWeight);
            
            TextFormat.ParagraphAlignment = ParagraphAlignment.Center;

            TitleFormat.ParagraphAlignment = ParagraphAlignment.Center;
        } 

        public static string TitleFormatFont => titleFormatFont;

        public static int TitleFormatWeight => titleFormatWeight;

        public static float BorderSize => borderSize * VisualLayer.Width;

        public static float TitleHeight => titleHeight * VisualLayer.Height;

        public static float TitleOffsetX => titleOffsetX * VisualLayer.Width;

        public static float OffsetSpeed => offsetSpeed * VisualLayer.Height;
    }
}
