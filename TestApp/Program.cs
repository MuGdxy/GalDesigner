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

#else
            DebugLayer.Watch("Width");
            DebugLayer.Watch("Height");
            DebugLayer.Watch("FullScreen");
            DebugLayer.Watch("AppName");
            
            GalEngine.GalEngine.Run();
#endif
        }
    }
}
