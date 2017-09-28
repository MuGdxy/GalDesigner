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
            SetValue(WidthName, 800);
            SetValue(HeightName, 600);
            SetValue(ApplicationName, "GalEngine");
            SetValue(FullScreenName, false);
        }

        public static void SetValue(string Tag, object value)
        {
            GlobalValue.SetValue(Tag, value);
        }

        public static object GetValue(string Tag) => GlobalValue.GetValue(Tag);

        public static T GetValue<T>(string Tag) => GlobalValue.GetValue<T>(Tag);

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
