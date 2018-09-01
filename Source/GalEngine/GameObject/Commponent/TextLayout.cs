using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class TextMetrics
    {
        private float width = 0;
        private float height = 0;
        private int lineCount = 0;

        public float Width => width;
        public float Height => height;
        public int LineCount => lineCount;

        public TextMetrics(float Width = 0, float Height = 0, int LineCount = 0)
        {
            width = Width;
            height = Height;
            lineCount = LineCount;
        }
    }

    public class TextLayout : Commponent
    {
        private string text = GameDefault.TextLayoutText;
        private string font = GameDefault.Font;
        private string color = GameDefault.Color;
        private float opacity = 1.0f;

        public string Text { get => text; set => text = value; }
        public string Font { get => font; set => font = value; }
        public string Color { get => color; set => color = value; }
        public float Opacity { get => opacity; set => opacity = value; }

        protected internal override void OnRender(GameObject gameObject)
        {
            if (gameObject.IsCommponentExist<Sharp>() is false) return;

            if (Text != GameDefault.TextLayoutText && Text != null)
            {
                var sharp = gameObject.GetCommponent<Sharp>();

                var rectangle = new RectangleF(0, 0, sharp.Size.Width, sharp.Size.Height);

                var textColor = GameResource.IsColorExist(Color) is true ? Color : GameDefault.Color;
                var textFont = GameResource.IsFontExist(Font) is true ? Font : GameDefault.Font;

                Systems.Graphics.DrawText(Text, rectangle, textFont, textColor, Opacity);
            }
        }

        public TextLayout() : this("")
        {

        }

        public TextLayout(string Text = "")
        {
            text = Text;
        }

        public TextLayout(string Text, string Font, string Color)
        {
            text = Text;
            font = Font;
            color = Color;
        }

        public void Add(string Text)
        {
            text += Text;
        }

        public void Remove(int Position, int Count = 1)
        {
            if (Text.Length == 0) return;

            int minRange = Math.Max(Position, 0);
            int maxRange = Math.Min(Position + Count - 1, Text.Length - 1);

            Text = Text.Remove(minRange, maxRange - minRange + 1);
        }

        public static TextMetrics ComputeTextMetrics(TextLayout TextLayout, SizeF MaxSize)
        {
            var TextFont = GameResource.IsFontExist(TextLayout.Font) ? TextLayout.Font : GameDefault.Font;

            Systems.Graphics.CreateTextLayout(TextLayout.Text, TextFont, MaxSize, out object Resource);
            Systems.Graphics.CreateTextMetrics(Resource, out TextMetrics TextMetrics);
            Systems.Graphics.DestoryTextLayout(ref Resource);

            return TextMetrics;
        }
    }
}
