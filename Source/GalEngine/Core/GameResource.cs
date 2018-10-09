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

        public static void SetColor(string colorName, Color color)
        {
            if (colors.ContainsKey(colorName) is true)
            {
                Systems.Graphics.CancelColorBrush(colorName);
            }
            
            colors[colorName] = color;
            Systems.Graphics.SetColorBrush(colorName, color);
        }

        public static void SetFont(string fontName, Font font)
        {
            if (fonts.ContainsKey(fontName) is true)
            {
                Systems.Graphics.CancelFontFace(fontName);
            }

            fonts[fontName] = font;
            Systems.Graphics.SetFontFace(fontName, font);
        }

        public static void SetBitmap(string bitmapName, System.IO.Stream bitmapStream)
        {
            if (bitmaps.ContainsKey(bitmapName) is true)
            {
                bitmaps[bitmapName].Dispose();
                bitmaps.Remove(bitmapName);
            }

            bitmaps[bitmapName] = new Bitmap(bitmapStream);
        }

        public static void SetBitmap(string bitmapName, Bitmap bitmap)
        {
            if (bitmaps.ContainsKey(bitmapName) is true)
            {
                bitmaps[bitmapName].Dispose();
                bitmaps.Remove(bitmapName);
            }

            bitmaps[bitmapName] = bitmap;
        }

        public static void CancelColor(string colorName)
        {
            if (colorName == null || colors.ContainsKey(colorName) is false) return;

            colors.Remove(colorName);
            Systems.Graphics.CancelColorBrush(colorName);
        }

        public static void CancelFont(string fontName)
        {
            if (fontName == null || fonts.ContainsKey(fontName) is false) return;

            fonts.Remove(fontName);
            Systems.Graphics.CancelFontFace(fontName);
        }

        public static void CancelBitmap(string bitmapName)
        {
            if (bitmapName == null || bitmaps.ContainsKey(bitmapName) is false) return;

            bitmaps[bitmapName].Dispose();
            bitmaps.Remove(bitmapName);
        }

        public static Color GetColor(string colorName)
        {
            if (colorName == null || colors.ContainsKey(colorName) is false) return null;

            return colors[colorName];
        }

        public static Font GetFont(string fontName)
        {
            if (fontName == null || fonts.ContainsKey(fontName) is false) return null;

            return fonts[fontName];
        }

        public static Bitmap GetBitmap(string bitmapName)
        {
            if (bitmapName == null || bitmaps.ContainsKey(bitmapName) is false) return null;

            return bitmaps[bitmapName];
        }

        public static bool IsColorExist(string colorName)
        {
            if (colorName == null) return false;

            return colors.ContainsKey(colorName);
        }

        public static bool IsFontExist(string fontName)
        {
            if (fontName == null) return false;

            return fonts.ContainsKey(fontName);
        }

        public static bool IsBitmapExist(string bitmapName)
        {
            if (bitmapName == null) return false;

            return bitmaps.ContainsKey(bitmapName);
        }
    }
}
