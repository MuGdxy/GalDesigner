using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    using Internal;

    /// <summary>
    /// log
    /// </summary>
    public class Log : Internal.Log
    {
        public string Text { get; }
        public LogLevel Level { get; }

        public Log(List<LogElement> elements, LogLevel level) : base(elements)
        {
            Level = level;
            Text = "[" + LogLevelConverter.ToString(level) + "] ";

            foreach (var logElement in elements)
            {
                Text = Text + logElement.Text;
            }
        }
    }
}
