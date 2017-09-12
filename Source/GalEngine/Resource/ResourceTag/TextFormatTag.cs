using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    class TextFormatTag : ResourceTag
    {
        private string fontFace;
        private float fontSize;
        private int fontWeight;

        public TextFormatTag(string Tag, string Fontface, float Size, int Weight) : base(Tag)
        {
            fontFace = Fontface;
            fontSize = Size;
            fontWeight = Weight;
        }

        protected override void ActiveResource(ref object resource)
        {
            if (resource is null)
            {
                resource = new CanvasTextFormat(fontFace, fontSize, fontWeight);
            }
        }

        protected override void DiposeResource(ref object resource)
        {
            if (resource is null) return;

            (resource as CanvasTextFormat).Dispose();
            resource = null;
        }

        public string Fontface => fontFace;
        public float Size => fontSize;
        public int Weight => fontWeight;
    }
}
