using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogPrinter;

namespace GalEngine
{
    /// <summary>
    /// log component.
    /// we can record log by this component.
    /// see base log format to know the format.
    /// </summary>
    public class LogComponent : Component
    {
        protected LogFormat mLogFormat;
        
        public List<Log> Logs { get; }

        public LogComponent(string sendObject)
        {
            BaseComponentType = typeof(LogComponent);

            Logs = new List<Log>();

            mLogFormat = new BaseLogFormat(sendObject);
        }

        public void Log(string logText, params object[] context)
        {
            Logs.Add(new Log(mLogFormat.GenerateLog(logText, context).Elements));
        }
    }
}
