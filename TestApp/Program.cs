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

            private VisualObject player1 = new VisualObject("Player1", 200, 200);
            private VisualObject player2 = new VisualObject("Player2", 200, 200);
            private VisualObject player3 = new VisualObject("Player3", 200, 200);
            private VisualObject textBox = new VisualObject("TextBox", 1920, 200);

            private VisualObject player1SayingBox = new VisualObject("Player1SayingBox", 300, 200);
            private VisualObject player1SayingText = new VisualObject("Player1SayingText", 250, 130);

            private VisualObject player2SayingBox = new VisualObject("Player2SayingBox", 300, 200);
            private VisualObject player2SayingText = new VisualObject("Player2SayingText", 250, 130);

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
                if (e.IsDown is true)
                {
                    player1.SetMemberValue(SystemProperty.BackGroundImage, "Player1");
                    player2.SetMemberValue(SystemProperty.BackGroundImage, "Player2");
                    player3.SetMemberValue(SystemProperty.BackGroundImage, "Player3");

                    textBox.SetMemberValue(SystemProperty.TextFormat, "FirstFont");

                    player1SayingBox.SetMemberValue(SystemProperty.BackGroundImage, "LeftSayingBox");
                    player1SayingText.SetMemberValue(SystemProperty.TextFormat, "TextFont");

                    player2SayingBox.SetMemberValue(SystemProperty.BackGroundImage, "LeftSayingBox");
                    player2SayingText.SetMemberValue(SystemProperty.TextFormat, "TextFont");

                    player1SayingBox.Opacity = 0;
                    player1SayingText.PositionX = 25;
                    player1SayingText.PositionY = 10;

                    player2SayingBox.Opacity = 0;
                    player2SayingText.PositionX = 25;
                    player2SayingText.PositionY = 10;

                    AddVisualObject(player1.Name);
                    AddVisualObject(player2.Name);
                    AddVisualObject(player3.Name);
                    AddVisualObject(textBox.Name);
                    AddVisualObject(player1SayingBox.Name);
                    AddVisualObject(player2SayingBox.Name);

                    player1SayingBox.AddChildren(player1SayingText.Name);
                    player2SayingBox.AddChildren(player2SayingText.Name);

                    player1.PositionY = 440;
                    player1.PositionX = 1920;
                    player1SayingBox.PositionY = 440 - 200;

                    player2.PositionX = 1920;
                    player2.PositionY = 440;
                    player2SayingBox.PositionX = 1920;
                    player2SayingBox.PositionY = 440 - 200;

                    player3.PositionX = 1920;
                    player3.PositionY = 440;

                    textBox.PositionY = 1080 - 200;

                    AnimatorForPlayer1 animatorForPlayer1 = new AnimatorForPlayer1("Player1");
                    AnimatorForPlayer2 animatorForPlayer2 = new AnimatorForPlayer2("Player2");
                    AnimatorForPlayer3 animatorForPlayer3 = new AnimatorForPlayer3("Player3");
                    AnimatorForTextBox animatorForTextBox = new AnimatorForTextBox("TextBox");

                    animatorForPlayer1.Run(StartTime);
                    animatorForPlayer2.Run(StartTime);
                    animatorForPlayer3.Run(StartTime);
                    animatorForTextBox.Run(StartTime);

                    isPlaying = true;
                }

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
