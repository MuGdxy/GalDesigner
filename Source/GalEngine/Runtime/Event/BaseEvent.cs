using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class BaseEvent
    {
        public DateTime Time { get; }

        public BaseEvent(DateTime time)
        {
            Time = time;
        }
    }
}
