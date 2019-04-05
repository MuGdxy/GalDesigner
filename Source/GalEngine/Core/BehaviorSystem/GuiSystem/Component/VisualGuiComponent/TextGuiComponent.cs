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
            //get the size we want to render
            var size = new Size<int>((int)(Shape as RectangleShape).Size.Width, (int)(Shape as RectangleShape).Size.Height);

            //do not need to change the text asset, because we do not change any propertry
            if (mTextAsset != null && mTextAsset.TextContent == Text && mTextAsset.Font == Font && 
                mTextAsset.MaxSize == size) return;

            //dispose old asset
            Utility.Dispose(ref mTextAsset);

            //create new asset
            mTextAsset = new Text(Text, Font, size);
        }

        public TextGuiComponent()
        {

        }

        public TextGuiComponent(RectangleShape shape, string text, Font font, Color<float> color) : base(shape)
        {
            Font = font;
            Text = text;
            Color = color;
        }

        public void SetProperty(string text, Font font)
        {
            Text = text;
            Font = font;

            //update asset
            SetPropertyToAsset();
        }
    }
}
