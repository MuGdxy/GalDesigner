using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogPrinter;

namespace GalEngine
{
    /// <summary>
    /// write log to console
    /// </summary>
    public class ConsoleLogPrint : LogPrint
    {
        public override void ApplyLog(LogPrinter.Log log)
        {
            base.ApplyLog(log);
        }
    }
}
