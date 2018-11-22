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
        private GameObject sendObject;
        
        protected override string MapMethod(KeySetting setting)
        {
            return "Log Sender: " + sendObject.Name;
        }

        public ObjectKeySetting(GameObject sendObject)
        {
            this.sendObject = sendObject;
        }
    }
}
