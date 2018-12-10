using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace GalEngine.Internal
{
    public class LogPrint
    {
        public virtual void ApplyLog(Log log)
        {
            foreach (var element in log.Elements)
            {
                Console.Write(element.Text);
            }
            
            Console.WriteLine();
        }

        public LogPrint()
        {
        }
    }
}