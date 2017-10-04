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
        private static TextureFace debugSurface;

        private static bool isLock = false;
        private static bool isEnable = false;

        internal static void OnResolutionChange()
        {

        }

        internal static void OnRender()
        {

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
