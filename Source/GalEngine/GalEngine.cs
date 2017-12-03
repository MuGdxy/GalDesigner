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
        private static TextureFace templateSurface;

        private static GameWindow gameWindow;

        internal static void SetResolution(int Width, int Height)
        {
            Utilities.Dipose(ref renderSurface);
            Utilities.Dipose(ref templateSurface);

            renderSurface = new TextureFace(Width, Height);
            templateSurface = new TextureFace(Width, Height);

            //Make sure the Window size
            if (presentSurface.IsFullScreen is false)
                gameWindow.SetWindowSize(Width, Height);
            else
            {
                presentSurface.IsFullScreen = false;
                gameWindow.SetWindowSize(Width, Height);
                presentSurface.IsFullScreen = true;
            }

            if (VisualLayer.IsEnable is true)
                //When we change the resolution, we need to update something.
                VisualLayer.OnResolutionChange(Width, Height);

        }

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

        public static void TurnToPage(string pageTag)
        {
            gameWindow.CurrentPage = PageList.Element[pageTag];
        }

        internal static GameWindow GameWindow => gameWindow;

        internal static Present PresentSurface
        {
            set => presentSurface = value;
            get => presentSurface;
        }

        internal static TextureFace RenderSurface => renderSurface;

        internal static TextureFace TemplateSurface => templateSurface;
    }
}
