using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;
using Builder;

namespace GalEngine
{
    public static class GalEngine
    {
        private static Present presentSurface;
        private static TextureFace renderSurface;

        private static GameWindow gameWindow;

        public static void Run()
        {
            BuildListAnalyser.LoadAllBuildList();

            Application.Add(gameWindow = new GameWindow(GlobalConfig.AppName, GlobalConfig.Width, GlobalConfig.Height)
            {
                IsFullScreen = GlobalConfig.IsFullScreen
            });

            SetResolution(gameWindow.Width, gameWindow.Height);


            Application.RunLoop();

            BuildListAnalyser.SaveAllBuildList();

            Engine.Stop();
        }

        internal static void SetResolution(int Width, int Height)
        {
            Utilities.Dipose(ref renderSurface);

            renderSurface = new TextureFace(Width, Height);

            //Make sure the Window size
            if (presentSurface.IsFullScreen is false)
                gameWindow.SetWindowSize(Width, Height);
            else
            {
                presentSurface.IsFullScreen = false;
                gameWindow.SetWindowSize(Width, Height);
                presentSurface.IsFullScreen = true;
            }
        }

        internal static GameWindow GameWindow => gameWindow;

        internal static Present PresentSurface
        {
            set => presentSurface = value;
            get => presentSurface;
        }
        internal static TextureFace RenderSurface
        {
            get => renderSurface;
        }
    }
}
