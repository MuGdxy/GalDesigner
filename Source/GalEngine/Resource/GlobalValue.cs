using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GlobalValue
    {
        private static Dictionary<string, object> valueList;

        static GlobalValue()
        {
            valueList = new Dictionary<string, object>();

            //default value
            SetValue(GlobalConfig.WidthName, 800);
            SetValue(GlobalConfig.HeightName, 600);
            SetValue(GlobalConfig.ApplicationName, "GalEngine");
            SetValue(GlobalConfig.FullScreenName, false);
        }

        public static void SetValue(string Tag, object value)
        {
            //make value same.
            if (Tag == GlobalConfig.FullScreenName && GalEngine.GameWindow != null)
            {
                GalEngine.GameWindow.IsFullScreen = (bool)value;
            }

            valueList[Tag] = value;
        }

        public static object GetValue(string Tag)
        {
            return GetValue<object>(Tag);
        }

        public static T GetValue<T>(string Tag)
        {
            DebugLayer.Assert(valueList.ContainsKey(Tag) is false, ErrorType.InvaildTag, "GlovalValue");

            return (T)valueList[Tag];
        }

        public static bool Contains(string Tag)
        {
            return valueList.ContainsKey(Tag);
        }

        internal static Dictionary<string, object> ValueList => valueList;
    }
}
