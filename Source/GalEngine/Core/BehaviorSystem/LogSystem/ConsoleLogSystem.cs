using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    using Internal;

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

            logPrint = new ConsoleLogPrint();
        }

        protected internal override void Excute(GameObject gameObject)
        {
            var component = gameObject.GetComponent<LogComponent>();

            foreach (var log in LogComponent.Logs)
                logPrint.ApplyLog(log);

            LogComponent.Logs.Clear();
        }
    }
}
