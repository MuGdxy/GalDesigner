using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GlobalConfig
    {
        private const string WidthName = "Width";
        private const string HeightName = "Height";
        private const string ApplicationName = "AppName";
        private const string FullScreenName = "FullScreen";

        private static Dictionary<string, object> valueList;

        static GlobalConfig()
        {
            valueList = new Dictionary<string, object>();

            SetValue(WidthName, 800);
            SetValue(HeightName, 600);
            SetValue(ApplicationName, "GalEngine");
            SetValue(FullScreenName, false);
        }

        public static void SetValue(string Tag, object value)
        {

#if DEBUG
            DebugLayer.Assert(Utilities.IsBaseType(value) is false, ErrorType.InvalidValueType);
#endif

            valueList[Tag] = value;
        }

        public static object GetValue(string Tag) => valueList[Tag];

        public static T GetValue<T>(string Tag)
        {
            return (T)valueList[Tag];
        }

        internal static Dictionary<string, object> ValueList => valueList;

        public static int Width
        {
            set => valueList[WidthName] = value;
            get => (int)valueList[WidthName];
        }

        public static int Height
        {
            set => valueList[HeightName] = value;
            get => (int)valueList[HeightName];
        }

        public static string AppName
        {
            set => valueList[ApplicationName] = value;
            get => valueList[ApplicationName] as string;
        }

        public static bool IsFullScreen
        {
            set => valueList[FullScreenName] = value;
            get => (bool)valueList[FullScreenName];
        }
    }
}
