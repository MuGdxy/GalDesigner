using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Builder;

namespace TestApp
{
    class Program
    {
        public static string AppName => "TestApp";

        static void Main(string[] args)
        {
            Resource.Create();

            Application.Add(new TestWindow(AppName, 1800, 1200));

            Application.RunLoop();

            Resource.Destory();

            Presenter.Engine.Stop();
        }
    }
}
