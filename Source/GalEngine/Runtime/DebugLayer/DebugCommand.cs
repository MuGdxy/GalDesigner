using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class DebugCommandProperty
    {
        public static string Name => "DebugCommand";
        public static string LineNmae => "DebugCommandLine";

        public static string CommandNotice => "Command>";

        public static string BackGround => "DebugCommandBackGround";

        public static string Font => "DebugCommandFont";

        public static float ScrollSpeed => 0.5f;
        public static float Opacity => 0.7f;
        
        internal static void Update(SizeF DebugCommandSize)
        {
            GameResource.SetFont(Font, new Font("Consolas", DebugCommandSize.Height * 0.03f));
        }

        static DebugCommandProperty()
        {
            GameResource.SetColor(BackGround, new Color(0.5f, 0.5f, 0.5f));
        }
    }
}
