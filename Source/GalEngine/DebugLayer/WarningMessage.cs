using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class WarningMessage
    {
        private string warningText;

        private DateTime occurTime;

        public WarningMessage(string text, DateTime dateTime)
        {
            warningText = text;
            occurTime = dateTime;
        }

        public string WarningText => warningText;
        public DateTime OccurTime => occurTime;
    }
}
