using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class UpdateEvent : BaseEvent
    {
        float DeltaTime { get; }

        public UpdateEvent(DateTime time, float deltaTime) : base(time)
        {
            DeltaTime = deltaTime;
        }
    }

    public class RenderEvent : BaseEvent
    {
        float DeltaTime { get; }

        public RenderEvent(DateTime time, float deltaTime) : base(time)
        {
            DeltaTime = deltaTime;
        }
    }

    public delegate void UpdateEventHandler(object sender, UpdateEvent eventArg);
    public delegate void RenderEventHandler(object sender, RenderEvent eventArg);
}
