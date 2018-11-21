using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using GalEngine;
using GalEngine.Extension;
using LogPrinter;

namespace TestApp
{
    class TimeSetting : KeySetting
    {
        protected override string MapMethod(KeySetting setting)
        {
            return DateTime.Now.ToString();
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
        }
    }
}
