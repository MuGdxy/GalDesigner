using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class DefaultInputTextProperty
    {
        public static Font Font { get; set; }
        public static Color<float> BackGround { get; set; }
        public static Color<float> FrontGround { get; set; }

        static DefaultInputTextProperty()
        {
            Font = Font.Default;
            BackGround = new Color<float>(0, 0, 0.7f, 1);
            FrontGround = new Color<float>(0, 0, 0, 1);
        }
    }

    public class InputTextGuiComponent : VisualGuiComponent
    {
        private static int NullLocation = 0;

        internal RowText mInputText;

        public Font Font { get; set; }
        public Color<float> BackGround { get; set; }
        public Color<float> FrontGround { get; set; }

        public string Content { get; set; }
        public int CursorLocation { get; set; }

        internal void SetPropertyToAsset()
        {
            //do not need to update the input text
            if (mInputText != null && mInputText.Content == Content && mInputText.Font == Font)
                return;

            //dispose the input text and create new input text
            Utility.Dispose(ref mInputText);
            //limit the cursor position
            Utility.Clamp(CursorLocation, NullLocation, Content.Length);

            mInputText = new RowText(Content, Font);
        }

        public InputTextGuiComponent() : this(
            new RectangleShape(),
            DefaultInputTextProperty.BackGround,
            DefaultInputTextProperty.FrontGround,
            DefaultInputTextProperty.Font)
        {

        }

        public InputTextGuiComponent(
            RectangleShape shape, 
            Color<float> backGround, 
            Color<float> frontGround, 
            Font font) :
            base(shape)
        {
            Font = font;
            BackGround = backGround;
            FrontGround = frontGround;

            Content = "";
            CursorLocation = NullLocation;
        }
    }
}
