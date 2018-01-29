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

        static void Main(string[] args)
        {
#if false

#else
            GalEngine.GalEngine.Initialize();

            DebugLayer.Watch("Width");
            DebugLayer.Watch("Height");
            DebugLayer.Watch("FullScreen");
            DebugLayer.Watch("AppName");

            DebugCommand.CommandAnalyser += DebugCommand_CommandAnalyser;
            DebugCommand.CommandAnalyser += DebugCommand_CommandAnalyser1;

            GenericPage genericPage = new GenericPage("MainPage");
            VisualObject visualObject1 = new VisualObject("Object1", 100, 100)
            {
                PositionX = 0,
                PositionY = 0,
                BorderSize = 1,
                Opacity = 1,
                Text = "Hello"
            };
            VisualObject visualObject2 = new VisualObject("Object2", 100, 100)
            {
                PositionX = 0,
                PositionY = 0,
                BorderSize = 1,
                Opacity = 0.5f,
                Text = ""
            };

            visualObject1.SetMemberValue("TextBrush", "White");
            visualObject2.SetMemberValue("BackGroundBrush", "Red");

            visualObject1.AddChildren(visualObject2.Tag);
            genericPage.AddVisualObject(visualObject1.Tag);

            GalEngine.GalEngine.TurnToPage("MainPage");
            
            GalEngine.GalEngine.Run();
#endif
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
