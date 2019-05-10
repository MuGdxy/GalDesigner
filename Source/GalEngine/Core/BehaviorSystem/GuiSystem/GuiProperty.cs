using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GuiProperty
    {
        /// <summary>
        /// distance from first text character to edge of input box
        /// </summary>
        public static float InputTextPadding { get; set; }

        /// <summary>
        /// width of input cursor
        /// </summary>
        public static float InputTextCursorWidth { get; set; }

        public static LegalContainer<char> TextLegalInput { get; set; }
        
        static GuiProperty()
        {
            InputTextPadding = 5.0f;
            InputTextCursorWidth = 1.0f;

            TextLegalInput = new LegalContainer<char>();
        }
    }
}
