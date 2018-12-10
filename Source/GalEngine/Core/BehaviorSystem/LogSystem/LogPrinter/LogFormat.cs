using System.Collections.Generic;
using System.Diagnostics;

namespace GalEngine.Internal
{
    /// <summary>
    /// Log format: Log: [Key0] [Key1] [Key2] ... {0} ... {1}...
    /// We replace [key] to value with key setting
    /// we replace {number} to value with parameter 
    /// </summary>
    public class LogFormat
    {
        private readonly Dictionary<string, KeySetting> mapKeySetting = null;

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
            mapKeySetting = new Dictionary<string, KeySetting>();
        }

        public virtual Log GenerateLog(string logText, params object[] context)
        {
            logText = SetParamsToString(logText, context);

            var elements = new List<LogElement>();

            var key = "";
            
            foreach (var item in logText)
            {
                switch (item)
                {
                    case '[':
                        elements.Add(new LogElement(key, null));

                        key = "";
                        break;
                    
                    case ']':
                        if (mapKeySetting.ContainsKey(key) is false)
                            elements.Add(new LogElement('[' + key + ']', null));
                        else
                            elements.Add(new LogElement(
                                mapKeySetting[key].GetValue(), mapKeySetting[key]));

                        key = "";
                        break;
                    
                    default:
                        key = key + item;
                        break;
                }
            }

            elements.Add(new LogElement(key, null));

            return new Log(elements);
        }
        
        public void AddKeySetting(string key, KeySetting setting)
        {
            mapKeySetting.Add(key, setting);
        }

        public void RemoveKeySetting(string key)
        {
            mapKeySetting.Remove(key);
        }
    }
}