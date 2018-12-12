using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    using Internal;

    /// <summary>
    /// write log to console
    /// </summary>
    public class ConsoleLogPrint : LogPrint
    {
        public override void ApplyLog(Internal.Log log)
        {
            Console.WriteLine((log as Log).Text);
        }
    }
}
