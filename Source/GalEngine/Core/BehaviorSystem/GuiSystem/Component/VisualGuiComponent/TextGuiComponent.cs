using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.GameResource;

namespace GalEngine
{
    public class TextGuiComponent : VisualGuiComponent
    {
        internal Text mTextAsset;

        public Font Font { get; set; }
        public string Text { get; set; }
        public Color<float> Color { get; set; }

        internal void SetPropertyToAsset()
        {
            //do not need to change the text asset, because we do not change any propertry
            if (mTextAsset != null && mTextAsset.TextContent == Text && mTextAsset.Font == Font) return;

            //dispose old asset
            Utility.Dispose(ref mTextAsset);

            //create new asset
            mTextAsset = new Text(Text, Font, new Size<int>(0, 0));

            //change the shape
            (Shape as RectangleShape).Size = new Size<float>(mTextAsset.Size.Width, mTextAsset.Size.Height);
        }

        public TextGuiComponent() : this("Text", Font.Default, new Color<float>(0, 0, 0, 1))
        {

        }

        public TextGuiComponent(string text, Font font, Color<float> color) : base(new RectangleShape())
        {
            Font = font;
            Text = text;
            Color = color;
        }
    }
}
