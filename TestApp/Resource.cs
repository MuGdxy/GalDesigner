using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace TestApp
{
    public static class Resource
    {
        public static SourceVoice voice1 = new SourceVoice(@"C:\Users\LinkC\Documents\Visual Studio 2017\Projects\GalDesigner\TestApp\Test1.wav");
        public static SourceVoice voice2 = new SourceVoice(@"C:\Users\LinkC\Documents\Visual Studio 2017\Projects\GalDesigner\TestApp\Test2.wav");

        public static void Destory()
        {
            voice1.Dispose();
            voice2.Dispose();
        }
    }
}
