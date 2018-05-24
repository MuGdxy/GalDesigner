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

        private class PageSample : GenericScene
        {
            private Timer timer = new Timer(StartTime);

            bool isPlaying = false;

            public override void OnUpdate(object sender)
            {
                if (isPlaying is true)
                {
                    timer.Pass(Time.DeltaSeconds * Speed);
                }
                base.OnUpdate(sender);
            }

            public override void OnMouseClick(object sender, MouseClickEventArgs e)
            {
                base.OnMouseClick(sender, e);
            }

            public PageSample(string Tag) : base(Tag)
            {
            }
        }


        static void Main(string[] args)
        {
#if false

#else
            GalEngine.GalEngine.Initialize();

            GenericScene genericPage = new PageSample("MainPage");

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
