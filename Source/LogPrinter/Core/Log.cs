using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace LogPrinter
{
    public class LogElement
    {
        public string Text { get; private set; }
        
        public KeySetting Setting { get; private set; }

        public LogElement(string text, KeySetting setting)
        {
            Text = text;
            Setting = setting;
        }
    }
    
    public class Log
    {
        public List<LogElement> Elements { get; private set; }

        public Log()
        {
            Elements = new List<LogElement>();
        }
    }
}