using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            GameSystems.Initialize(new GameStartInfo
            {
                Window = new WindowInfo()
                {
                    Name = "TestUnit",
                    Size = new Size(1920, 1080)
                }
            });

            GameSystems.RunLoop();
        }
    }
}
