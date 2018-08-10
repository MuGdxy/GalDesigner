using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static partial class GameResource
    {
        internal static Dictionary<string, Color> colors = new Dictionary<string, Color>();
        internal static Dictionary<string, Font> fonts = new Dictionary<string, Font>();
        internal static Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();

        static GameResource()
        {
            SetColor(GameDefault.Color, new Color());
            SetFont(GameDefault.Font, new Font());

            SetSystemColors();
        }

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

        public static void SetBitmap(string BitmapName, System.IO.Stream BitmapStream)
        {
            if (bitmaps.ContainsKey(BitmapName) is true)
            {
                bitmaps[BitmapName].Dispose();
                bitmaps.Remove(BitmapName);
            }

            bitmaps[BitmapName] = new Bitmap(BitmapStream);
        }

        public static void CancelColor(string ColorName)
        {
            if (ColorName == null || colors.ContainsKey(ColorName) is false) return;

            colors.Remove(ColorName);
            Systems.Graphics.CancelColorBrush(ColorName);
        }

        public static void CancelFont(string FontName)
        {
            if (FontName == null || fonts.ContainsKey(FontName) is false) return;

            fonts.Remove(FontName);
            Systems.Graphics.CancelFontFace(FontName);
        }

        public static void CancelBitmap(string BitmapName)
        {
            if (BitmapName == null || bitmaps.ContainsKey(BitmapName) is false) return;

            bitmaps[BitmapName].Dispose();
            bitmaps.Remove(BitmapName);
        }

        public static Color GetColor(string ColorName)
        {
            if (ColorName == null || colors.ContainsKey(ColorName) is false) return null;

            return colors[ColorName];
        }

        public static Font GetFont(string FontName)
        {
            if (FontName == null || fonts.ContainsKey(FontName) is false) return null;

            return fonts[FontName];
        }

        public static Bitmap GetBitmap(string BitmapName)
        {
            if (BitmapName == null || bitmaps.ContainsKey(BitmapName) is false) return null;

            return bitmaps[BitmapName];
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

        public static bool IsBitmapExist(string BitmapName)
        {
            if (BitmapName == null) return false;

            return bitmaps.ContainsKey(BitmapName);
        }
    }
}
