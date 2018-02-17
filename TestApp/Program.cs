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
            public PageSample(string Tag) : base(Tag)
            {

            }
        }


        static void Main(string[] args)
        {
#if false

#else
            GalEngine.GalEngine.Initialize();

            GenericPage genericPage = new PageSample("MainPage");

            VisualObject visualObject1 = new VisualObject("Object1", 100, 100)
            {
                PositionX = 0,
                PositionY = 0,
                BorderSize = 1,
                Opacity = 1,
                Text = "Hello"
            };

            visualObject1.MouseClick += VisualObject1_MouseClick;

            VisualObject visualObject2 = new VisualObject("Object2", 100, 100)
            {
                PositionX = 0,
                PositionY = 0,
                BorderSize = 1,
                Opacity = 0.5f,
                Text = ""
            };

            visualObject1.SetMemberValue("TextBrush", "White");
            visualObject1.SetMemberValue("BackGroundBursh", "Blue");
            visualObject2.SetMemberValue("BackGroundBrush", "Red");
            
            genericPage.AddVisualObject(visualObject1.Tag);
            //genericPage.AddVisualObject(visualObject2.Tag);

            DebugLayer.RegisterWarning(0, "2333");
            DebugLayer.ReportWarning(0, null);

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
