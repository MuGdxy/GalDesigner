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

        protected override void ActiveResource(ref object resource)
        {
            if (resource is null)
            {
                resource = new CanvasTextFormat(fontFace, fontSize, fontWeight)
                {
                    TextAlignment = TextAlignment.Center,
                    ParagraphAlignment = ParagraphAlignment.Center
                };
            }
        }

        protected override void DiposeResource(ref object resource)
        {
            Utilities.Dipose(ref resource);
        }

        public string Fontface => fontFace;
        public float Size => fontSize;
        public int Weight => fontWeight;
    }
}
