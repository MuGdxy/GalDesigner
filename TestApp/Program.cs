using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using GalEngine;
using Builder;

namespace TestApp
{
    class Program
    {
        public static string AppName => "TestApp";

        public static float Speed => 1;

        public static float StartTime => 0;

        private class PageSample : Scene
        {
            private Timer timer = new Timer(StartTime);

            private VisualObject backGround = new VisualObject("BackGround", 1920, 1080);
            private VisualObject frame = new VisualObject("Frame", 1920, 450);
            private VisualObject window = new VisualObject("window", 1725, 234);

            public override void OnUpdate(object sender)
            {
                base.OnUpdate(sender);
            }

            public override void OnMouseClick(object sender, MouseClickEventArgs e)
            {
                base.OnMouseClick(sender, e);
            }

            public PageSample(string Tag) : base(Tag)
            {
                backGround.SetMemberValue(SystemProperty.BackGroundImage, "BackGround");
                frame.SetMemberValue(SystemProperty.BackGroundImage, "Frame");
                window.SetMemberValue(SystemProperty.BackGroundImage, "Window");

                window.PositionX = (1920 - window.Width) / 2;
                window.PositionY = 1080 - frame.Height - 20 + 180;
                window.PositionZ = 2;

                window.Opacity = 0.5f;

                frame.PositionY = 1080 - frame.Height - 20;
                frame.PositionZ = 3;

                AddVisualObject(window.Name);
                AddVisualObject(frame.Name);
                AddVisualObject(backGround.Name);
            }
        }


        static void Main(string[] args)
        {
#if false

#else
            GalEngine.GalEngine.Initialize();

            Scene genericPage = new PageSample("MainPage");

            GalEngine.GalEngine.TurnToScene("MainPage");
            
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
