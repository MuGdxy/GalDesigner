using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class LogProvider : GameObject
    {
        private LogComponent logComponent;

        public bool IsActive { get; set; }

        public LogProvider(string name) : base(name)
        {
            AddComponent(new LogComponent(this));

            logComponent = GetComponent<LogComponent>();

            IsActive = true;
        }

        public void Log(string logText, params object[] context)
        {
            if (IsActive is false) return;

            logComponent.Log(logText, context);
        }
    }
}
