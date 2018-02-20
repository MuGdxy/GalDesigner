using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using GalEngine;

namespace TestApp
{
    class Program
    {
        public static string AppName => "TestApp";

        private class PageSample : GenericPage
        {
            private VisualObject ImageObject = new VisualObject("ImageObject", 1280, 720);

            public PageSample(string Tag) : base(Tag)
            {
                ImageObject.SetMemberValue(SystemProperty.BackGroundImage, "BackGround");

                AddVisualObject(ImageObject.Tag);
            }
        }


        static void Main(string[] args)
        {
#if false

#else
            GalEngine.GalEngine.Initialize();

            GenericPage genericPage = new PageSample("MainPage");

            GalEngine.GalEngine.TurnToPage("MainPage");
            
            GalEngine.GalEngine.Run();
#endif
        }

        private static void VisualObject1_MouseClick(object sender, Builder.MouseClickEventArgs e)
        {
            if (e.IsDown is true)
            {
                (sender as VisualObject).IsPresented ^= true;
            }
        }

        private static bool DebugCommand_CommandAnalyser1(string[] commandParameters)
        {
            return false;
        }

        private static bool DebugCommand_CommandAnalyser(string[] commandParameters)
        {
            return false;
        }
    }
}
