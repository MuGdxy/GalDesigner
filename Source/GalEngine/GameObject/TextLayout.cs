using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class TextLayout
    {
        private string text = GameDefault.TextLayoutText;
        private string font = GameDefault.Font;
        private string color = GameDefault.Color;

        public string Text { get => text; set => text = value; }
        public string Font { get => font; set => font = value; }
        public string Color { get => color; set => color = value; }
        
        public TextLayout(string Text = "")
        {
            text = Text;
            font = GameDefault.Font;
            color = GameDefault.Color;
        }

        public TextLayout(string Text, string Font, string Color)
        {
            text = Text;
            font = Font;
            color = Color;
        }
    }
}
