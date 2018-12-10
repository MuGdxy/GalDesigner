using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public enum LogLevel
    {
        Information,
        Warning,
        Error
    }

    public static class LogLevelConverter
    {
        private static Dictionary<LogLevel, string> mStringMap;

        static LogLevelConverter()
        {
            mStringMap = new Dictionary<LogLevel, string>();

            mStringMap.Add(LogLevel.Information, "Information");
            mStringMap.Add(LogLevel.Warning, "Warning");
            mStringMap.Add(LogLevel.Error, "Error");
        }

        public static string ToString(LogLevel level)
        {
            return mStringMap[level];
        }
    }
}
