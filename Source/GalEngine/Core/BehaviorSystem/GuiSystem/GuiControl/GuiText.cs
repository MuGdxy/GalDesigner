using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.GameResource;

namespace GalEngine
{
    public class GuiText : GuiControl
    {
        public GuiText() : this("text", Font.Default, new Color<float>(0, 0, 0, 1))
        {

        }

        public GuiText(string name) : this(name, "text", Font.Default, new Color<float>(0, 0, 0, 1))
        {

        }

        public GuiText(string text, Font font, Color<float> color) : base()
        {
            AddComponent(new TextGuiComponent(text, font, color));
        }

        public GuiText(string name, string text, Font font, Color<float> color) : base(name)
        {
            AddComponent(new TextGuiComponent(text, font, color));
        }
    }
}
