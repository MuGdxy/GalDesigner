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
            SetValue(SystemProperty.Width, 800);
            SetValue(SystemProperty.Height, 600);
            SetValue(SystemProperty.AppName, "GalEngine");
            SetValue(SystemProperty.FullScreeen, false);
            SetValue(SystemProperty.IsExit, false);
        }

        public static void SetValue(string name, object value)
        {
            //make value same.
            if (name == SystemProperty.FullScreeen && GalEngine.GameWindow != null)
            {
                GalEngine.GameWindow.IsFullScreen = (bool)value;
            }

            valueList[name] = value;
        }

        public static object GetValue(string name)
        {
            return GetValue<object>(name);
        }

        public static T GetValue<T>(string name)
        {
            DebugLayer.Assert(valueList.ContainsKey(name) is false, ErrorType.InvaildName, "GlovalValue");

            return (T)valueList[name];
        }

        public static bool Contains(string name)
        {
            return valueList.ContainsKey(name);
        }

        internal static Dictionary<string, object> ValueList => valueList;
    }
}
