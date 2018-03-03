using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GlobalAnalyser
    {
        private static Dictionary<string, ResourceAnalyser> analyserList;

        static GlobalAnalyser()
        {
            analyserList = new Dictionary<string, ResourceAnalyser>();
        }

        internal static Dictionary<string, ResourceAnalyser> AnalyserList => analyserList;

        public static void SetValue(ResourceAnalyser resourceAnalyser)
            => analyserList[resourceAnalyser.Name] = resourceAnalyser;
        
        public static ResourceAnalyser GetValue(string Name)
        {
            return GetValue<ResourceAnalyser>(Name);
        }

        public static T GetValue<T>(string Name) where T : ResourceAnalyser
        {
            DebugLayer.Assert(analyserList.ContainsKey(Name) is false, ErrorType.InvaildName, "GlobalAnalyser");

            return analyserList[Name] as T;
        }
    }
}
