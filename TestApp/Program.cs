using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using GalEngine;
using GalEngine.Extension;

namespace TestApp
{

    class Program
    {
        static void Create()
        {
            GameScene.Main = new GameScene("RootScene", new Size(1920, 1080));
            GameScene.Main.SetBehaviorSystem(new ImageRenderSystem(GameScene.Main));

            GameResource.SetBitmap("Test", new System.IO.FileStream("Bitmap.png", System.IO.FileMode.Open));

            GameObject imageObject = new GameObject("ImageObject");

            imageObject.SetComponent(new Transform());
            imageObject.SetComponent(new ImageShape(GameScene.Main.Resolution, "Test"));

            GameScene.Main.SetGameObject(imageObject);
        }

        static void Main(string[] args)
        {
            Create();

            Application.Create("GalEngine", new Size(1920, 1080), null);
            Application.RunLoop();
        }
    }
}
