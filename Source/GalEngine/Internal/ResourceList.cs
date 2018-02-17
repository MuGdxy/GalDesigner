using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Presenter;

namespace GalEngine.Internal
{
    internal static class ResourceList
    {
        private static CanvasBrush blackBrush = new CanvasBrush(0, 0, 0, 1);
        private static CanvasBrush blueBrush = new CanvasBrush(0, 0, 1, 1);
        private static CanvasBrush redBrush = new CanvasBrush(1, 0, 0, 1);
        private static CanvasBrush greenBrush = new CanvasBrush(0, 1, 0, 1);
        private static CanvasBrush whiteBrush = new CanvasBrush(1, 1, 1, 1);

        private static CanvasTextFormat defaultTextFormat = new CanvasTextFormat("Consolas", 10);

        public static CanvasBrush BlackBrush => blackBrush;
        public static CanvasBrush BlueBrush => blueBrush;
        public static CanvasBrush RedBrush => redBrush;
        public static CanvasBrush GreenBrush => greenBrush;
        public static CanvasBrush WhiteBrush => whiteBrush;

        public static CanvasTextFormat DefaultTextFormat => defaultTextFormat;

        public static CanvasBrush DefaultBackGroundBrush => whiteBrush;
        
        public static void ReSizeResource(int newWidth, int newHeight)
        {
            defaultTextFormat.Reset("Consolas", newHeight * 0.027f);
            defaultTextFormat.ParagraphAlignment = ParagraphAlignment.Center;
        }
    }
}
