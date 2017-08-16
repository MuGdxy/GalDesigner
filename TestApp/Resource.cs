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
        public static SourceVoice voice1 = new SourceVoice(@"D:\Resource\Test1.wav");
        
        public static void Destory()
        {
            voice1.Dispose();
        }
    }
}
