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

        public static CanvasBrush BlackBrush => blackBrush;
        public static CanvasBrush BlueBrush => blueBrush;
        public static CanvasBrush RedBrush => redBrush;
        public static CanvasBrush GreenBrush => greenBrush;
    }
}
