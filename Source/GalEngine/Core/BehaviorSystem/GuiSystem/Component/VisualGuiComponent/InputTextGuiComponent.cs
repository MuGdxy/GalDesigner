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
        public static Color<float> Background { get; set; }
        public static Color<float> Frontground { get; set; }

        static DefaultInputTextProperty()
        {
            Font = Font.Default;
            Background = new Color<float>(0, 0.4f, 0.7f, 1);
            Frontground = new Color<float>(0, 0, 0, 1);
        }
    }

    public class InputTextGuiComponent : VisualGuiComponent
    {
        private static int NullLocation = 0;

        internal RowText mInputText;

        public Font Font { get; set; }
        public Color<float> Background { get; set; }
        public Color<float> Frontground { get; set; }

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
            CursorLocation = Utility.Clamp(CursorLocation, NullLocation, Content.Length);

            mInputText = new RowText(Content, Font);
        }

        public InputTextGuiComponent() : this(0,
            DefaultInputTextProperty.Background,
            DefaultInputTextProperty.Frontground,
            DefaultInputTextProperty.Font)
        {

        }

        public InputTextGuiComponent(
            float width, 
            Color<float> background, 
            Color<float> frontground, 
            Font font) :
            base(new RectangleShape(new Size<float>(width, font.Size + GuiProperty.InputTextPadding * 2.0f)))
        {
            Font = font;
            Background = background;
            Frontground = frontground;

            Content = "";
            CursorLocation = NullLocation;
        }
    }
}
