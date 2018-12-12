using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace GalEngine.Internal
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
        public List<LogElement> Elements { get; }

        public Log(List<LogElement> elements)
        {
            Elements = elements;
        }
    }
}