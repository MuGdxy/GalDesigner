using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GuiProperty
    {
        public static string InputMoveX => "InputMoveX";
        public static string InputMoveY => "InputMoveY";
        public static string InputWheel => "InputWheel";

        public static string InputClick => "InputClick";

        public static string InputText => "InputText";

        public static bool IsGuiInput(string name)
        {
            if (name == InputMoveX || 
                name == InputMoveY || 
                name == InputWheel ||
                name == InputClick || 
                name == InputText) return true;

            return false;
        }
    }
}
