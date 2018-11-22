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
        protected LogFormat logFormat;

        public GameObject Target { get; }
        public List<Log> Logs { get; }

        public LogComponent(GameObject target)
        {
            BaseComponentType = typeof(LogComponent);

            Target = target;

            Logs = new List<Log>();

            logFormat = new BaseLogFormat(this);
        }

        public void Log(string logText, params object[] context)
        {
            Logs.Add(new Log(logFormat.GenerateLog(logText, context).Elements));
        }
    }
}
