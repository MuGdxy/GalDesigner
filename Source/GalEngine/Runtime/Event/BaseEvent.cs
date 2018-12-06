using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public enum EventOperation
    {
        Add,
        Remove
    }

    public class BaseEvent
    {
        public DateTime Time { get; }
        public EventOperation Operation { get; }

        public BaseEvent(DateTime time, EventOperation operation)
        {
            Time = time;

            Operation = operation;
        }
    }
}
