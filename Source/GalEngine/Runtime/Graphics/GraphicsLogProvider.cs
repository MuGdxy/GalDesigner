using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    static class GraphicsLogProvider
    {
        private static LogProvider mLogProvider;

        public static void Initialize()
        {
            GameSystems.SystemScene.Root.GetChild(StringProperty.LogRoot)
                .GetChild(StringProperty.LogRuntimeNode)
                .AddChild(mLogProvider = new LogProvider(StringProperty.LogGraphicsNode));

            mLogProvider.Log("[Initialize Graphics Log Finish] [object]");
        }

        public static void Log(string logText, LogLevel logLevel = LogLevel.Information, params object[] context)
        {
            mLogProvider.Log(logText, logLevel, context);
        }
    }
}
