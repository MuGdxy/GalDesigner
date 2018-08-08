using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Font
    {
        private string fontName = GameDefault.FontName;
        private FontWeight fontWeight = GameDefault.FontWeight;
        private FontStyle fontStyle = GameDefault.FontStyle;
        private float fontSize = GameDefault.FontSize;

        public string FontName { get => fontName; set => fontName = value; }
        public FontWeight FontWeight { get => fontWeight; set => fontWeight = value; }
        public FontStyle FontStyle { get => fontStyle; set => fontStyle = value; }
        public float FontSize { get => fontSize; set => fontSize = value; }

        public Font(string FontName = "Consolas", float FontSize = 17, FontWeight FontWeight = FontWeight.Normal,
            FontStyle FontStyle = FontStyle.Normal)
        {
            fontName = FontName;
            fontSize = FontSize;
            fontWeight = FontWeight;
            fontStyle = FontStyle;
        }
    }
}
