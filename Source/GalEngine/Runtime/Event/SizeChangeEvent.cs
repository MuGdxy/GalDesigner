using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class SizeChangeEvent : BaseEvent
    {
        public Size<int> Before { get; }
        public Size<int> After { get; }

        public SizeChangeEvent(DateTime time, Size<int> before, Size<int> after) : base(time)
        {
            Before = before;
            After = after;
        }
    }

    public delegate void SizeChangeEventHandler(object sender, SizeChangeEvent eventArg);
}
