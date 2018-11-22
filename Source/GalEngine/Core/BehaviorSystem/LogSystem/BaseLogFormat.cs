using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogPrinter;

namespace GalEngine
{
    /// <summary>
    /// base log format
    /// support [time] -> current time
    /// support [object] -> who send the log(game object)
    /// </summary>
    public class BaseLogFormat : LogFormat
    {
        public BaseLogFormat(LogComponent logComponent)
        {
            //[time] -> current time
            AddKeySetting("time", new TimeKeySetting());

            //[object] -> who send the log(game object)
            AddKeySetting("object", new ObjectKeySetting(logComponent.Target));
        }
    }
}
