using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogPrinter;

namespace GalEngine
{
    class ObjectKeySetting : KeySetting
    {
        private string mSendObject;
        
        protected override string MapMethod(KeySetting setting)
        {
            return "Log Sender = " + mSendObject;
        }

        public ObjectKeySetting(string sendObject)
        {
            mSendObject = sendObject;
        }
    }
}
