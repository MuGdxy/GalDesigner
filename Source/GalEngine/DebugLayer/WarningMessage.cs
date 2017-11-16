using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    /// <summary>
    /// Warning Message
    /// </summary>
    public class WarningMessage
    {
        /// <summary>
        /// Warning text.
        /// </summary>
        private string warningText;

        /// <summary>
        /// The warning occur time.
        /// </summary>
        private DateTime occurTime;

        /// <summary>
        /// Create a warning message.
        /// </summary>
        /// <param name="text">Warning text.</param>
        /// <param name="dateTime">Occur time.</param>
        public WarningMessage(string text, DateTime dateTime)
        {
            warningText = text;
            occurTime = dateTime;
        }

        /// <summary>
        /// Warning text.
        /// </summary>
        public string WarningText => warningText;

        /// <summary>
        /// Occur time.
        /// </summary>
        public DateTime OccurTime => occurTime;
    }
}
