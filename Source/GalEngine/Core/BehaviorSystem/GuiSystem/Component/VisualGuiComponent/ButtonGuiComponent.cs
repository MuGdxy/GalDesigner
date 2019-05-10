using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{

    namespace Internal
    {
        public static class DefaultButtonProperty
        {
            public static Font Font { get; set; }
            public static string Text { get; set; }
            public static Color<float> Background { get; set; }
            public static Color<float> Frontground { get; set; }
            public static Color<float> HoverBackground { get; set; }
            public static Color<float> HoverFrontground { get; set; }

            static DefaultButtonProperty()
            {
                //design requirement
                Font = Font.Default;
                Text = "Button";
                Background = new Color<float>(0.7f, 0.7f, 0.7f, 1.0f);
                Frontground = new Color<float>(0, 0, 0, 1);
                HoverBackground = new Color<float>(0.8f, 0.8f, 0.8f, 1.0f);
                HoverFrontground = new Color<float>(0, 0, 0, 1);
            }
        }
    }

    public class ButtonGuiComponent : VisualGuiComponent
    {
        internal Text mTextAsset;

        public Font Font { get; set; }
        public string Text { get; set; }

        public Color<float> Background { get; set; }
        public Color<float> Frontground { get; set; }

        internal void SetPropertyToAsset()
        {
            //get shape
            var shape = Shape as RectangleShape;
            var maxSize = new Size<int>((int)shape.Size.Width, (int)shape.Size.Height);

            //do not need to change the text asset, because we do not change any propertry
            if (mTextAsset != null && mTextAsset.Content == Text && mTextAsset.Font == Font &&
                mTextAsset.MaxSize == maxSize) return;

            //dispose old asset
            Utility.Dispose(ref mTextAsset);

            //create new asset
            mTextAsset = new Text(Text, Font, maxSize);
        }

        public ButtonGuiComponent() : this(new RectangleShape(),
            Internal.DefaultButtonProperty.Text,
            Internal.DefaultButtonProperty.Font,
            Internal.DefaultButtonProperty.Background,
            Internal.DefaultButtonProperty.Frontground)
        {
            
        }

        public ButtonGuiComponent(
            RectangleShape shape, 
            string text, 
            Font font,
            Color<float> background, 
            Color<float> frontground) : base(shape)
        {
            Font = font;
            Text = text;
            Background = background;
            Frontground = frontground;
        }
    }
}
