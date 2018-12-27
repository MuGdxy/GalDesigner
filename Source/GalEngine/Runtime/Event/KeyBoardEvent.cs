using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class KeyBoardEvent : BaseEvent
    {
        public bool IsDown { get; }
        public KeyCode KeyCode { get; }

        public KeyBoardEvent(DateTime time, KeyCode keyCode, bool isDown) : base(time)
        {
            KeyCode = keyCode;
            IsDown = isDown;
        }
    }

    public delegate void KeyBoardEventHandler(object sender, KeyBoardEvent eventArg);
}