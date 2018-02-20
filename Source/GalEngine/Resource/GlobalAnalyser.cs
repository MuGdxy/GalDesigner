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
            => analyserList[resourceAnalyser.Tag] = resourceAnalyser;
        
        public static ResourceAnalyser GetValue(string Tag)
        {
            return GetValue<ResourceAnalyser>(Tag);
        }

        public static T GetValue<T>(string Tag) where T : ResourceAnalyser
        {
            DebugLayer.Assert(analyserList.ContainsKey(Tag) is false, ErrorType.InvaildTag, "GlobalAnalyser");

            return analyserList[Tag] as T;
        }
    }
}
