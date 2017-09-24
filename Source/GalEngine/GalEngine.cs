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

#if false
            ResListAnalyser resListAnalyser = new ResListAnalyser("1", @"ResListT.resList");
            resListAnalyser.LoadAnalyser();
            resListAnalyser.SaveAnalyser();
#endif

#if true
            ConfigAnalyser configAnalyser = new ConfigAnalyser("1", @"ConfigT.gsConfig");
            configAnalyser.LoadAnalyser();
            configAnalyser.SaveAnalyser();
#endif

            Application.Add(new GameWindow(GlobalConfig.AppName, GlobalConfig.Width, GlobalConfig.Height));

            Application.RunLoop();
            
            Engine.Stop();
        }

        
    }
}
