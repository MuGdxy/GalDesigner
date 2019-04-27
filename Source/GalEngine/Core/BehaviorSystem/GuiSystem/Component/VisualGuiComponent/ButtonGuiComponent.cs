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
            public static Color<float> BackGround { get; set; }
            public static Color<float> FrontGround { get; set; }
            public static Color<float> HoverBackGround { get; set; }
            public static Color<float> HoverFrontGround { get; set; }

            static DefaultButtonProperty()
            {
                //design requirement
                Font = Font.Default;
                Text = "Button";
                BackGround = new Color<float>(0.7f, 0.7f, 0.7f, 1.0f);
                FrontGround = new Color<float>(0, 0, 0, 1);
                HoverBackGround = new Color<float>(0.8f, 0.8f, 0.8f, 1.0f);
                HoverFrontGround = new Color<float>(0, 0, 0, 1);
            }
        }
    }

    public class ButtonGuiComponent : VisualGuiComponent
    {
        internal Text mTextAsset;

        public Font Font { get; set; }
        public string Text { get; set; }

        public Color<float> BackGround { get; set; }
        public Color<float> FrontGround { get; set; }

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
            Internal.DefaultButtonProperty.BackGround,
            Internal.DefaultButtonProperty.FrontGround)
        {
            
        }

        public ButtonGuiComponent(
            RectangleShape shape, 
            string text, 
            Font font,
            Color<float> backGround, 
            Color<float> frontGround) : base(shape)
        {
            Font = font;
            Text = text;
            BackGround = backGround;
            FrontGround = frontGround;
        }
    }
}
