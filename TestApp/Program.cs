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
            Builder.Application.Add(new TestWindow(AppName, 1920, 1080));

            Builder.Application.RunLoop();

            Presenter.Engine.Stop();
#else
            GalEngine.GalEngine.Run();
#endif

        }
    }
}
