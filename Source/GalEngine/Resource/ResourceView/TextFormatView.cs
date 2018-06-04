using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    class TextFormatView : ResourceView
    {
        private string fontFace;
        private float fontSize;
        private int fontWeight;

        public TextFormatView(string name, string Fontface, float Size, int Weight) : base(name)
        {
            fontFace = Fontface;
            fontSize = Size;
            fontWeight = Weight;
        }

        public string Fontface => fontFace;
        public float Size => fontSize;
        public int Weight => fontWeight;

        public CanvasTextFormat Source => Resource as CanvasTextFormat;
    }
}
