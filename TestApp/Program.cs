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
        static void Main(string[] args)
        {
            GameScene.Create("GalEngine", new Size(1920, 1080), "");
            GameScene.RunLoop();
        }
    }
}
