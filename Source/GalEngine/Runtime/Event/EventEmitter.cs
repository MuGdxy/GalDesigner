using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class EventEmitter
    {
        protected Queue<BaseEvent> mEventQueue;

        public EventEmitter()
        {
            mEventQueue = new Queue<BaseEvent>();
        }

        public void SenderEvent(BaseEvent baseEvent)
        {
            mEventQueue.Enqueue(baseEvent);
        }

        public BaseEvent GetEvent(bool remove = false)
        {
            if (remove == true) return mEventQueue.Dequeue();
            return mEventQueue.Peek();
        }
    }
}
