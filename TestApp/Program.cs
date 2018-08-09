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
        public class TestObject : GameObject
        {
            protected override void OnMouseEnter(object sender)
            {
                Console.WriteLine("MouseEnter: " + Name);

                base.OnMouseEnter(sender);
            }

            protected override void OnMouseLeave(object sender)
            {
                Console.WriteLine("MouseLeave: " + Name);

                base.OnMouseLeave(sender);
            }

            protected override void OnBoardClick(object sender, BoardClickEvent eventArg)
            {
                Console.WriteLine(eventArg.KeyCode.ToString());

                base.OnBoardClick(sender, eventArg);
            }

            public TestObject(Size Size) : base(Size)
            {
            }
        }


        static void Main(string[] args)
        {
            GameObject gameObject = new TestObject(new Size(50, 50));
            GameObject gameObject2 = new TestObject(new Size(200, 200));

            gameObject.Border = 1.0f;
            gameObject.Transform.Position = new PositionF(0, 50);
            
            gameObject2.Border = 1.0f;
            gameObject2.Transform.Position = new PositionF(100, 100);
            gameObject2.Transform.Forward = new Vector2(-1, -1);

            gameObject2.SetChild(gameObject);
            
            GameScene.SetGameObject(gameObject2);
            GameScene.Create("GalEngine", new Size(1920, 1080), "");
            GameScene.RunLoop();
        }
    }
}
