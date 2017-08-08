using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class CanvasTextFormat : CanvasResource
    {
        private SharpDX.DirectWrite.TextFormat textFormat;

        private float fSize;
        private int fWeight;

        private SharpDX.DirectWrite.TextAlignment textAlignment = SharpDX.DirectWrite.TextAlignment.Leading;
        private SharpDX.DirectWrite.ParagraphAlignment paragraphAlignment = SharpDX.DirectWrite.ParagraphAlignment.Near;

        public CanvasTextFormat(string fontface, float size, int weight = 400)
        {
            textFormat = new SharpDX.DirectWrite.TextFormat(Engine.WriteFactory,
                fontface, (SharpDX.DirectWrite.FontWeight)(fWeight = weight), SharpDX.DirectWrite.FontStyle.Normal,
                fSize = size);

            textFormat.TextAlignment = textAlignment;
            textFormat.ParagraphAlignment = paragraphAlignment;
        }

        public float Size => fSize;
        public int Weight => fWeight;

        public TextAlignment TextAlignment
        {
            get => (TextAlignment)textAlignment;
            set
            {
                textAlignment = (SharpDX.DirectWrite.TextAlignment)value;
                textFormat.TextAlignment = textAlignment;
            }
        }

        public ParagraphAlignment ParagraphAlignment
        {
            get => (ParagraphAlignment)paragraphAlignment;
            set
            {
                paragraphAlignment = (SharpDX.DirectWrite.ParagraphAlignment)value;
                textFormat.ParagraphAlignment = paragraphAlignment;
            }
        }

        internal SharpDX.DirectWrite.TextFormat TextFormat => textFormat;

        ~CanvasTextFormat() => SharpDX.Utilities.Dispose(ref textFormat);
    }
}
