using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.GameResource
{
    public class Text : IDisposable
    {
        private readonly Font mFont;
        private readonly string mTextContent;

        private Texture2D mTextTexture;

        public Font Font => mFont;

        public Texture2D Texture => Texture;

        public string TextContent => mTextContent;

        public Size<int> Size => mTextTexture.Size;

        private static Size<int> RequirementSize(string textContent, Font font, Size<int> maxSize)
        {
            //see more in freetype
            var sizeMetrices = font.FontFace.Size.Metrics;
            var baseLineDistance = sizeMetrices.Height.Value >> 6;

            //pen position, in other words, the position of current character
            var penPositionX = 0;
            var penPositionY = baseLineDistance;

            var requirementWidth = 0;
            var requirementHeight = 0;

            //internal function: update requirement size(width and height)
            void internalfunctionUpdateSize(int width, int height)
            { 
                //when we has gone to next line in this time, the "penPositionX" maybe less than requirement width
                //but at the last line, "penPositionX" maybe more than requirement width
                //so we need to use "max" function to update requirement width
                requirementWidth = Math.Max(requirementWidth, width);
                requirementHeight = Math.Max(requirementHeight, height);
            }
            
            //internal function: go to next line, when the width of "textContent" more than max width
            //internal function: we need to go to next line to layout the text we want to display
            void internalfunctionNextLine()
            {
                penPositionX = 0;
                penPositionY = penPositionY + baseLineDistance;
            }

            foreach (var character in textContent)
            {
                //if "textContent" contains '\n', we need to go next line
                if (character == '\n')
                {
                    internalfunctionNextLine();
                    internalfunctionUpdateSize(penPositionX, penPositionY);

                    continue;
                }

                var codeMetrics = font.GetCharacterCodeMetrics(character);

                //see more in "internalfunctionNextLine"
                if (penPositionX + codeMetrics.Advance >= maxSize.Width) internalfunctionNextLine();

                //accept an new chacacter, we need to update the size
                internalfunctionUpdateSize(penPositionX + codeMetrics.Texture.Size.Width + codeMetrics.HoriBearingX, penPositionY);

                //go to next character in the same line
                penPositionX = penPositionX + codeMetrics.Advance;
            }

            //limit the requirement size
            internalfunctionUpdateSize(maxSize.Width, maxSize.Height);

            return new Size<int>(requirementWidth, requirementHeight);
        }

        private static void CopyTextToTexture(string textContent, Font font, Texture2D texture)
        {
            //see more in freetype
            var sizeMetrics = font.FontFace.Size.Metrics;
            var maxCharacterHeight = sizeMetrics.Ascender - sizeMetrics.Descender;
            var baseLineDistance = sizeMetrics.Height.Value >> 6;
            var baseLine = ((sizeMetrics.Height - maxCharacterHeight) / 2 + sizeMetrics.Ascender).Value >> 6;

            var penPositionX = 0;
            var penPositionY = baseLine;

            //internal function: go to next line, when the width of "textContent" more than max width
            //internal function: we need to go to next line to layout the text we want to display
            void internalfunctionNextLine()
            {
                penPositionX = 0;
                penPositionY = penPositionY + baseLineDistance;
            }

            foreach (var character in textContent)
            {
                //if "textContent" contains '\n', we need to go next line
                if (character == '\n')
                {
                    internalfunctionNextLine();

                    continue;
                }

                var codeMetrics = font.GetCharacterCodeMetrics(character);

                //see more in "internalfunctionNextLine"
                if (penPositionX + codeMetrics.HoriBearingX + codeMetrics.Texture.Size.Width > texture.Size.Width)
                    internalfunctionNextLine();

                //it is possible that the character texture may out of the text texture
                //we need to clip the out part(only copy the in part)
                var sourceTextureWidth = codeMetrics.Texture.Size.Width;
                var sourceTextureHeight = Math.Min(
                    codeMetrics.Texture.Size.Height,
                    codeMetrics.HoriBearingY + texture.Size.Height - penPositionY);

                //no part of character texture are in the text texture
                //we can finish our work
                if (sourceTextureHeight < 0) return;

                //copy character texture to text texture 
                //the text texture is the texture we will display
                texture.CopyFromTexture2D(new Position<int>(
                    penPositionX + codeMetrics.HoriBearingX,
                    penPositionY - codeMetrics.HoriBearingY),
                    codeMetrics.Texture,
                    new Rectangle<int>(0, 0, sourceTextureWidth, sourceTextureHeight));

                //next character
                penPositionX = penPositionX + codeMetrics.Advance;
            }
        }

        public Text(string textContent, Font font, Size<int> maxSize)
        {
            mFont = font;
            mTextContent = textContent;

            maxSize = new Size<int>(
                maxSize.Width == 0 ? int.MaxValue : maxSize.Width,
                maxSize.Height == 0 ? int.MaxValue : maxSize.Height);

            mTextTexture = new Texture2D(RequirementSize(mTextContent, mFont, maxSize), PixelFormat.Alpha8bit);

            CopyTextToTexture(mTextContent, mFont, mTextTexture);
        }

        public Text(string textContent, Font font) : this(textContent, font, new Size<int>(0, 0))
        {

        }

        public void Dispose()
        {
            Utility.Dispose(ref mTextTexture);
        }
    }
}
