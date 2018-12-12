using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime
{
    static class RuntimeLogProvider
    {
        private static LogProvider mLogProvider;

        public static void Initialize()
        {
            GameSystems.SystemScene.Root.GetChild(StringProperty.LogRoot)
                .AddChild(mLogProvider = new LogProvider(StringProperty.LogRuntimeNode));

            mLogProvider.Log("[Initialize Runtime Log Finish] [object]");
        }

        public static void Log(string logText, LogLevel logLevel = LogLevel.Information, params object[] context)
        {
            mLogProvider.Log(logText, logLevel, context);
        }
    }
}
