using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GlobalConfig
    {
        internal const string WidthName = "Width";
        internal const string HeightName = "Height";
        internal const string ApplicationName = "AppName";
        internal const string FullScreenName = "FullScreen";

        static GlobalConfig()
        { 
        }

        public static void SetValue(string Name, object value)
        {
            GlobalValue.SetValue(Name, value);
        }

        public static object GetValue(string Name) => GlobalValue.GetValue(Name);

        public static T GetValue<T>(string Name) => GlobalValue.GetValue<T>(Name);

        public static int Width
        {
            set => GlobalValue.SetValue(WidthName, value);
            get => GlobalValue.GetValue<int>(WidthName);
        }

        public static int Height
        {
            set => GlobalValue.SetValue(HeightName, value);
            get => GlobalValue.GetValue<int>(HeightName);
        }

        public static string AppName
        {
            set => GlobalValue.SetValue(ApplicationName, value);
            get => GlobalValue.GetValue<string>(ApplicationName);
        }

        public static bool IsFullScreen
        {
            set => GlobalValue.SetValue(FullScreenName, value);
            get => GlobalValue.GetValue<bool>(FullScreenName);
        }
    }
}
