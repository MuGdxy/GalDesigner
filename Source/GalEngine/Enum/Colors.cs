using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class Colors
    {
        public static string Red => "Red";
        public static string Green => "Green";
        public static string Blue => "Blue";
    }

    public static partial class GameResource
    {
        private static void SetSystemColors()
        {
            SetColor(Colors.Red, new Color(1, 0, 0));
            SetColor(Colors.Green, new Color(0, 1, 0));
            SetColor(Colors.Blue, new Color(0, 0, 1));
        }
    }
}
