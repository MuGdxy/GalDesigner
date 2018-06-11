using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GlobalConfig
    {
        static GlobalConfig()
        { 
        }

        public static void SetValue(string name, object value)
        {
            GlobalValue.SetValue(name, value);
        }

        public static object GetValue(string name) => GlobalValue.GetValue(name);

        public static T GetValue<T>(string name) => GlobalValue.GetValue<T>(name);

        public static int Width
        {
            set => GlobalValue.SetValue(SystemProperty.Width, value);
            get => GlobalValue.GetValue<int>(SystemProperty.Width);
        }

        public static int Height
        {
            set => GlobalValue.SetValue(SystemProperty.Height, value);
            get => GlobalValue.GetValue<int>(SystemProperty.Height);
        }

        public static string AppName
        {
            set => GlobalValue.SetValue(SystemProperty.AppName, value);
            get => GlobalValue.GetValue<string>(SystemProperty.AppName);
        }

        public static bool IsFullScreen
        {
            set => GlobalValue.SetValue(SystemProperty.FullScreeen, value);
            get => GlobalValue.GetValue<bool>(SystemProperty.FullScreeen);
        }

        public static bool IsExit
        {
            set => GlobalValue.SetValue(SystemProperty.IsExit, value);
            get => GlobalValue.GetValue<bool>(SystemProperty.IsExit);
        }
    }
}
