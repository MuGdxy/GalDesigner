using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class EventListener
    {
        protected internal Queue<BaseEvent> mEventQueue;

        public int EventCount { get => mEventQueue.Count; }

        public EventListener()
        {
            mEventQueue = new Queue<BaseEvent>();
        }

        public BaseEvent GetEvent(bool remove = false)
        {
            if (remove == true) return mEventQueue.Dequeue();
            return mEventQueue.Peek();
        }
    }
}
