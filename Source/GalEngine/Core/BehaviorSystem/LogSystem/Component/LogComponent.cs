using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    using Internal;
    /// <summary>
    /// log component.
    /// we can record log by this component.
    /// see base log format to know the format.
    /// </summary>
    public class LogComponent : Component
    {
        private HashSet<LogLevel> mDisableLogLevel;

        protected LogFormat mLogFormat;
        
        public List<Log> Logs { get; }

        public LogComponent(string sendObject)
        {
            BaseComponentType = typeof(LogComponent);

            Logs = new List<Log>();

            mLogFormat = new BaseLogFormat(sendObject);

            mDisableLogLevel = new HashSet<LogLevel>();
        }

        public void Log(string logText, LogLevel level = LogLevel.Information, params object[] context)
        {
            if (mDisableLogLevel.Contains(level) is true) return;

            Logs.Add(new Log(mLogFormat.GenerateLog(logText, context).Elements, level));
        }

        public void EnableLogLevel(LogLevel level)
        {
            mDisableLogLevel.Remove(level);
        }

        public void DisableLogLevel(LogLevel level)
        {
            mDisableLogLevel.Add(level);
        }
    }
}
