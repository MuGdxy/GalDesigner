using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiButton : GuiControl
    {
        public Color<float> Background { get; set; }
        public Color<float> Frontground { get; set; }
        public Color<float> HoverBackground { get; set; }
        public Color<float> HoverFrontground { get; set; }

        public GuiButton() : this(new Size<float>(),
            Internal.DefaultButtonProperty.Text,
            Internal.DefaultButtonProperty.Font,
            Internal.DefaultButtonProperty.Background,
            Internal.DefaultButtonProperty.Frontground)
        {
        }

        public GuiButton(string name, Size<float> size) : this(name, size,
            Internal.DefaultButtonProperty.Text,
            Internal.DefaultButtonProperty.Font,
            Internal.DefaultButtonProperty.Background,
            Internal.DefaultButtonProperty.Frontground)
        {
        }

        public GuiButton(Size<float> size,
            string text,
            Font font,
            Color<float> background,
            Color<float> frontground) : this(null, size, text, font, background, frontground)
        {

        }

        public GuiButton(string name, 
            Size<float> size,
            string text, 
            Font font, 
            Color<float> background,
            Color<float> frontground) : base(name)
        {
            Background = background;
            Frontground = frontground;

            AddComponent(new LogicGuiComponent());
            AddComponent(new ButtonGuiComponent(new RectangleShape(size), text, font, background, frontground));

            HoverBackground = Internal.DefaultButtonProperty.HoverBackground;
            HoverFrontground = Internal.DefaultButtonProperty.HoverFrontground;

            GetComponent<LogicGuiComponent>().EventParts.Add(GuiComponentSupportEvent.Hover,
                new GuiComponentHoverEventPart((control, eventArg) =>
                {
                    var buttonCompoent = GetComponent<VisualGuiComponent>() as ButtonGuiComponent;
                    var hover = (eventArg as GuiComponentHoverEvent).Hover;

                    buttonCompoent.Background = hover ? HoverBackground : Background;
                    buttonCompoent.Frontground = hover ? HoverFrontground : Frontground;
                }));

            GetComponent<LogicGuiComponent>().EventParts.Add(GuiComponentSupportEvent.MouseClick,
                new GuiComponentEventPart());
        }
    }
}
