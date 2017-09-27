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
#endif

#if false
            ConfigAnalyser configAnalyser = new ConfigAnalyser("1", @"ConfigT.gsConfig");
#endif

#if true
#endif
            BuildListAnalyser.LoadAllBuildList();
            
            Application.Add(new GameWindow(GlobalConfig.AppName, GlobalConfig.Width, GlobalConfig.Height));

            Application.RunLoop();

            BuildListAnalyser.SaveAllBuildList();

            Engine.Stop();
        }

        
    }
}
