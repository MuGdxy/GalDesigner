using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GuiProperty
    {
        public static float InputTextPadding { get; set; }
        public static float InputTextCursorWidth { get; set; }

        static GuiProperty()
        {
            InputTextPadding = 1.0f;
        }
    }
}
