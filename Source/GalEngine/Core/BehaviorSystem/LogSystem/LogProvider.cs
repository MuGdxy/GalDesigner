using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class LogProvider : GameObject
    {
        private LogComponent mLogComponent;

        public bool IsActive { get; set; }

        public LogProvider(string name) : base(name)
        {
            AddComponent(mLogComponent = new LogComponent(name));
            
            IsActive = true;
        }

        public void Log(string logText, LogLevel logLevel = LogLevel.Information, params object[] context)
        {
            if (IsActive is false) return;

            mLogComponent.Log(logText, logLevel, context);
        }

        public void EnableLogLevel(LogLevel level)
        {
            mLogComponent.EnableLogLevel(level);
        }

        public void DisableLogLevel(LogLevel level)
        {
            mLogComponent.DisableLogLevel(level);
        }
    }
}
