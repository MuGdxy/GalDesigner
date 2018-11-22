using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogPrinter;

namespace GalEngine
{
    /// <summary>
    /// log
    /// </summary>
    public class Log : LogPrinter.Log
    {
        public string Text { get; }

        public Log(List<LogElement> elements) : base(elements)
        {
            Text = "";

            foreach (var logElement in elements)
            {
                Text = Text + logElement.Text;
            }
        }
    }
}
