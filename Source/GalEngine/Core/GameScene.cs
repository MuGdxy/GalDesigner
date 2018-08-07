using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GameScene
    {
        private static ValueManager<Size> resolution = new ValueManager<Size>(ChangeResolutionEvent);

        private static Bitmap renderBuffer = null;

        public static Size Resolution { set => resolution.Value = value; get => resolution.Value; }

        private static void ChangeResolutionEvent(Size oldSize, Size newSize)
        {
            Utility.Dispose(ref renderBuffer);

            renderBuffer = new Bitmap(newSize);

            System.Graphics.Target = renderBuffer;
        }

        internal static void OnUpdate()
        {
            resolution.Update();
        }

        public static void Create(string GameName, Size Resolution, string GameIcon)
        {
            resolution.Value = Resolution;

            Application.Create(GameName, Resolution, GameIcon);
        }

        public static void RunLoop()
        {
            Application.RunLoop();
        }
    }
}
