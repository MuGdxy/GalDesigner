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

            DebugCommand.CommandAnalyser += DebugCommand_CommandAnalyser;
            DebugCommand.CommandAnalyser += DebugCommand_CommandAnalyser1;
            
            GalEngine.GalEngine.Run();
#endif
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
