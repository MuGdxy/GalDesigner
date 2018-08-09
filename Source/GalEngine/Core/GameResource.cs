using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class GameResource
    {
        internal static Dictionary<string, Color> colors = new Dictionary<string, Color>();
        internal static Dictionary<string, Font> fonts = new Dictionary<string, Font>();

        public static void SetColor(string ColorName, Color Color)
        {
            if (colors.ContainsKey(ColorName) is true)
            {
                Systems.Graphics.CancelColorBrush(ColorName);
            }
            
            colors[ColorName] = Color;
            Systems.Graphics.SetColorBrush(ColorName, Color);
        }

        public static void SetFont(string FontName, Font Font)
        {
            if (fonts.ContainsKey(FontName) is true)
            {
                Systems.Graphics.CancelFontFace(FontName);
            }

            fonts[FontName] = Font;
            Systems.Graphics.SetFontFace(FontName, Font);
        }

        public static void CancelColor(string ColorName)
        {
            if (colors.ContainsKey(ColorName) is false) return;

            colors.Remove(ColorName);
            Systems.Graphics.CancelColorBrush(ColorName);
        }

        public static void CancelFont(string FontName)
        {
            if (fonts.ContainsKey(FontName) is false) return;

            fonts.Remove(FontName);
            Systems.Graphics.CancelFontFace(FontName);
        }

        public static Color GetColor(string ColorName)
        {
            if (colors.ContainsKey(ColorName) is false) return new Color();

            return colors[ColorName];
        }

        public static Font GetFont(string FontName)
        {
            if (fonts.ContainsKey(FontName) is false) return new Font();

            return fonts[FontName];
        }

        public static bool IsColorExist(string ColorName)
        {
            if (ColorName == null) return false;

            return colors.ContainsKey(ColorName);
        }

        public static bool IsFontExist(string FontName)
        {
            if (FontName == null) return false;

            return fonts.ContainsKey(FontName);
        }
    }
}
