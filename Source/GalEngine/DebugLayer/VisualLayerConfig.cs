using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    using ResourceList = Internal.ResourceList;

    static class VisualLayerConfig
    {
        /// <summary>
        /// The VisualPad Title Fontface.
        /// </summary>
        private const string titleFormatFont = "Consolas";
        
        /// <summary>
        /// The VisualPad Title Weight.
        /// </summary>
        private const int titleFormatWeight = 500;

        /// <summary>
        /// The border size (relative VisualLayer).
        /// </summary>
        private const float borderSize = 0.001f;

        /// <summary>
        /// The VisualPad title height. (relative VisualLayer).
        /// </summary>
        private const float titleHeight = 0.05f;
        
        /// <summary>
        /// The VisualPad title position-X (relative VisualLayer).
        /// </summary>
        private const float titleOffsetX = 0.01f;

        /// <summary>
        /// The MouseScroll offset speed (relative VisualLayer Height).
        /// </summary>
        private const float offsetSpeed = 0.001f;

        /// <summary>
        /// The Border Brush RGBA(0, 0, 0, 1).
        /// </summary>
        public static CanvasBrush BorderBrush = ResourceList.BlackBrush;
        /// <summary>
        /// The Text Brush RGBA(0, 0, 0, 1).
        /// </summary>
        public static CanvasBrush TextBrush = ResourceList.BlackBrush;

        /// <summary>
        /// The VisualLayer BackGround Brush RGBA(1, 1, 1, 1).
        /// </summary>
        public static CanvasBrush BackGroundBrush = ResourceList.WhiteBrush;

        /// <summary>
        /// The Title Brush RGBA(0.529f, 0.443f, 0.811f, 1f).
        /// </summary>
        public static CanvasBrush TitleBrush = new CanvasBrush(0.529f, 0.443f, 0.811f, 1f);

        /// <summary>
        /// The Text Foramt.
        /// </summary>
        public static CanvasTextFormat TextFormat = new CanvasTextFormat("Consolas", 10);

        /// <summary>
        /// The Title Format.
        /// </summary>
        public static CanvasTextFormat TitleFormat = new CanvasTextFormat(TitleFormatFont, 10, TitleFormatWeight);

        /// <summary>
        /// Reset the Resource When SizeChange
        /// </summary>
        /// <param name="newWidth">NewWidth (pixel).</param>
        /// <param name="newHeight">NewHeight (pixel).</param>
        public static void OnResizeResource(int newWidth, int newHeight)
        {
            TextFormat.Reset("Consolas", newHeight * 0.027f);

            TitleFormat.Reset(titleFormatFont, newHeight * 0.03f, titleFormatWeight);
            
            TextFormat.ParagraphAlignment = ParagraphAlignment.Center;

            TitleFormat.ParagraphAlignment = ParagraphAlignment.Center;
        }

        /// <summary>
        /// The VisualPad Title Fontface.
        /// </summary>
        public static string TitleFormatFont => titleFormatFont;

        /// <summary>
        /// The VisualPad Title Weight.
        /// </summary>
        public static int TitleFormatWeight => titleFormatWeight;

        /// <summary>
        /// The Border Size (pixel).
        /// </summary>
        public static float BorderSize => borderSize * VisualLayer.Width;

        /// <summary>
        /// The Title Height (pixel).
        /// </summary>
        public static float TitleHeight => titleHeight * VisualLayer.Height;

        /// <summary>
        /// The VisualPad Title position-X (pixel).
        /// </summary>
        public static float TitleOffsetX => titleOffsetX * VisualLayer.Width;
        
        /// <summary>
        /// The MouseScroll offset speed (pixel).
        /// </summary>
        public static float OffsetSpeed => offsetSpeed * VisualLayer.Height;
    }
}
