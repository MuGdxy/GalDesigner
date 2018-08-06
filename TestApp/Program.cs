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
            Application.MakeWindow("GalEngine", 1920, 1080);
            Application.Visable = true;
            Application.RunLoop();
        }
    }
}
