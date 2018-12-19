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
            foreach (var element in log.Elements)
            {
                if (element.Setting == null) Console.Write(element.Text);
                if (element.Setting is ColorKeySetting)
                {
                    Console.ForegroundColor = (element.Setting as ColorKeySetting).Color;
                    Console.Write(element.Text);
                    Console.ResetColor();
                }
            }
            Console.WriteLine();
        }
    }
}
