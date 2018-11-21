using System.Collections.Generic;
using System.Diagnostics;

namespace LogPrinter
{
    /// <summary>
    /// Log format: Log: [Key0] [Key1] [Key2] ... {0} ... {1}...
    /// We replace [key] to value with key setting
    /// we replace {number} to value with parameter 
    /// </summary>
    public class LogFormat
    {
        private readonly Dictionary<string, KeySetting> mMapKeySetting = null;

        private static string SetParamsToString(string text, params object[] context)
        {
            string result = text;

            int count = 0;

            while (text.Contains("{" + count + "}") is true)
            {
                result = result.Replace("{" + count + "}", context[count].ToString());

                count++;
            }

            return result;
        }

        public LogFormat()
        {
            mMapKeySetting = new Dictionary<string, KeySetting>();
        }

        public Log GenerateLog(string logText, params object[] context)
        {
            logText = SetParamsToString(logText, context);

            var log = new Log();

            var key = "";
            
            foreach (var item in logText)
            {
                switch (item)
                {
                    case '[':
                        log.Elements.Add(new LogElement(key, null));

                        key = "";
                        break;
                    
                    case ']':
                        if (mMapKeySetting.ContainsKey(key) is false)
                            log.Elements.Add(new LogElement('[' + key + ']', null));
                        else
                            log.Elements.Add(new LogElement(
                                mMapKeySetting[key].GetValue(), mMapKeySetting[key]));

                        key = "";
                        break;
                    
                    default:
                        key = key + item;
                        break;
                }
            }

            log.Elements.Add(new LogElement(key, null));

            return log;
        }
        
        public void AddKeySetting(string key, KeySetting setting)
        {
            mMapKeySetting.Add(key, setting);
        }

        public void RemoveKeySetting(string key)
        {
            mMapKeySetting.Remove(key);
        }
    }
}