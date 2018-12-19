using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.Internal;

namespace GalEngine
{
    class TimeKeySetting : ColorKeySetting
    {
        protected override string MapMethod(KeySetting setting)
        {
            return DateTime.Now.ToString();
        }

        public TimeKeySetting()
        {
            Color = ConsoleColor.Green;
        }
    }
}
