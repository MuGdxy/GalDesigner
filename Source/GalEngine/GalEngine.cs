using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;
using Builder;

namespace GalEngine
{
    public static class GalEngine
    {
        public static void Run()
        {
            Application.Add(new GameWindow("GalEngine", 1920, 1080));

            Application.RunLoop();

            Engine.Stop();
        }

        
    }
}
