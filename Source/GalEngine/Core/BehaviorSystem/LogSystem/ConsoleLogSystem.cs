using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogPrinter;

namespace GalEngine
{
    /// <summary>
    /// console log system
    /// print or solve the logs in log component
    /// </summary>
    public class ConsoleLogSystem : BehaviorSystem
    {
        protected LogPrint logPrint;

        public ConsoleLogSystem() : base("ConsoleLogSystem")
        {
            RequireComponents.AddRequireComponentType<LogComponent>();

            logPrint = new LogPrint();
        }

        public override void Excute(GameObject gameObject)
        {
            var component = gameObject.GetComponent<LogComponent>();

            foreach (var log in component.Logs)
                logPrint.ApplyLog(log);

            component.Logs.Clear();
        }
    }
}
