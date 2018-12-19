using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.Internal;

namespace GalEngine
{
    abstract class ColorKeySetting : KeySetting
    {
        public ConsoleColor Color { get; protected set; }
    }
}
