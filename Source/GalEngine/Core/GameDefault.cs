using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GameDefault
    {
        public static string Color => "Default";
        public static string Font => "Default";
        public static string Bitmap => "Default";

        public static string FontName => "Consolas";
        public static FontWeight FontWeight => FontWeight.Normal;
        public static FontStyle FontStyle => FontStyle.Normal;
        public static float FontSize => 17;

        public static Vector2 Forward => new Vector2(0.0f, 1.0f);

        public static string GameObjectName => "GameObject";

        public static Transform Transform => new Transform();
        public static SizeF SizeF => new SizeF();
        public static Border Border => new Border();
        public static TextLayout TextLayout => new TextLayout();
        public static BackGround BackGround => new BackGround();
        public static float Opacity => 1.0f;
        public static int Depth => 0;

        public static float BorderWidth => 0.0f;

        public static string TextLayoutText => "";
    }
}
