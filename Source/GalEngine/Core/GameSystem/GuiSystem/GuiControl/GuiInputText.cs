using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiInputTextStyle : GuiStyle
    {
        public Colorf Background { get; set; }
        public Colorf Cursor { get; set; }
        public Colorf Text { get; set; }
        public Colorf Padding { get; set; }

        public GuiInputTextStyle()
        {
            Background = new Colorf(0, 0, 0, 0);
            Cursor = new Colorf(0, 0, 0, 1);
            Text = new Colorf(0, 0, 0, 1);
            Padding = new Colorf(0, 0, 0, 1);
        }
    }

    public class GuiInputText : GuiRectangleElement
    {
        private static float Duration = 1.0f;
        private static float HalfDuration = 0.5f;

        private RowText mTextBuffer;

        private float mPassTime;

        public string Content { get; set; }
        public FontClass FontClass { get; set; }
        public int FontSize { get; private set; }

        public int Cursor { get; set; }

        public GuiInputTextStyle Style { get; set; }

        protected internal override void Update(float delta)
        {
            mPassTime = (mPassTime + delta) % Duration;

            void OnSizeChange()
            {
                //ensure the size of input text
                Size = new Size(Size.Width, Utility.Max(Size.Height, 4));

                FontSize = Size.Height - 4;
            }

            void OnFontChange()
            {
                //when font changed
                var font = FontClass.Fonts(FontSize);

                //do not need to change the text buffer, because we do not change any propertry
                if (mTextBuffer != null && mTextBuffer.Content == Content && mTextBuffer.Font == font) return;

                Utility.Dispose(ref mTextBuffer);

                mTextBuffer = new RowText(Content, font);
            }

            OnSizeChange();
            OnFontChange();

            //ensure the cursor position is legal, from [0, content.length]
            Cursor = Utility.Clamp(Cursor, 0, Content.Length);
        }

        protected internal override void Draw(GuiRender render)
        {
            var padding = 2.0f;
            var cursorHeight = FontSize * 0.9f;
            var textPosition = new Point2f(padding * 2, Utility.Center(Size.Height, mTextBuffer.Size.Height));
            var cursorPosition = new Point2f(textPosition.X + (
                Cursor == 0 ? 0 : mTextBuffer.GetCharacterPostLocation(Cursor - 1)), Utility.Center(Size.Height, cursorHeight));
            var area = new Rectanglef(0, 0, Size.Width, Size.Height);

            render.FillRectangle(area, Style.Background);
            render.DrawRectangle(area, Style.Padding, padding);
            render.DrawText(textPosition, mTextBuffer, Style.Text);

            if (mPassTime < HalfDuration) return;

            render.DrawLine(cursorPosition, cursorPosition + new Vector2f(0, cursorHeight), Style.Cursor, padding);
        }

        protected internal override void Input(InputAction action)
        {
            if (action.Type == InputType.Axis) return;

            var buttonAction = action as ButtonInputAction;

            //if the input button is keycode and keycode is down
            if (InputProperty.InvertKeyCodeMapped.ContainsKey(buttonAction.Name) && buttonAction.Status)
            {
                //get keycode
                var keyCode = InputProperty.InvertKeyCodeMapped[buttonAction.Name];

                //move the cursor with left or right button
                void OnCursorMove()
                {
                    if (keyCode == KeyCode.Left) Cursor--;
                    if (keyCode == KeyCode.Right) Cursor++;

                    Cursor = Utility.Clamp(Cursor, 0, Content.Length);
                }

                void OnBackSpace()
                {
                    if (keyCode == KeyCode.Back && Cursor != 0)
                        Content = Content.Remove(--Cursor, 1);
                }

                void OnInsert()
                {
                    //get the insert value
                    var value = KeyCodeConvertTo(keyCode);

                    //null means we do not need to insert
                    if (value == null) return;

                    Content = Content.Insert(Cursor, value);
                    Cursor = Cursor + value.Length;
                }

                //solve the input action
                OnCursorMove();
                OnBackSpace();
                OnInsert();
            }
        }

        protected virtual string KeyCodeConvertTo(KeyCode keyCode)
        {

        }
        
        public GuiInputText(string content, Size size) :
            this(content, GuiFactory.FontClass, size)
        {

        }

        public GuiInputText(string content, FontClass fontClass, Size size)
        {
            Content = content;
            FontClass = fontClass;
            Size = size;

            Cursor = 0;
            Style = new GuiInputTextStyle();

            Dragable = false;

            mTextBuffer = null;
        }
    }
}
