using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.GameResource;

namespace GalEngine
{
    public class GuiButton : GuiControl
    {
        public Color<float> BackGround { get; set; }
        public Color<float> FrontGround { get; set; }
        public Color<float> HoverBackGround { get; set; }
        public Color<float> HoverFrontGround { get; set; }

        public GuiButton() : this(new Size<float>(),
            Internal.DefaultButtonProperty.Text,
            Internal.DefaultButtonProperty.Font,
            Internal.DefaultButtonProperty.BackGround,
            Internal.DefaultButtonProperty.FrontGround)
        {
        }

        public GuiButton(string name) : this(name, new Size<float>(),
            Internal.DefaultButtonProperty.Text,
            Internal.DefaultButtonProperty.Font,
            Internal.DefaultButtonProperty.BackGround,
            Internal.DefaultButtonProperty.FrontGround)
        {
        }

        public GuiButton(Size<float> size,
            string text,
            Font font,
            Color<float> backGround,
            Color<float> frontGround) : this(null, size, text, font, backGround, frontGround)
        {

        }

        public GuiButton(string name, 
            Size<float> size,
            string text, 
            Font font, 
            Color<float> backGround,
            Color<float> frontGround) : base(name)
        {
            BackGround = backGround;
            FrontGround = frontGround;

            AddComponent(new LogicGuiComponent());
            AddComponent(new ButtonGuiComponent(new RectangleShape(size), text, font, backGround, frontGround));

            HoverBackGround = Internal.DefaultButtonProperty.HoverBackGround;
            HoverFrontGround = Internal.DefaultButtonProperty.HoverFrontGround;

            GetComponent<LogicGuiComponent>().EventParts.Add(GuiComponentSupportEvent.Hover,
                new GuiComponentHoverEventPart((control, eventArg) =>
                {
                    var buttonCompoent = GetComponent<VisualGuiComponent>() as ButtonGuiComponent;
                    var hover = (eventArg as GuiComponentHoverEvent).Hover;

                    buttonCompoent.BackGround = hover ? HoverBackGround : BackGround;
                    buttonCompoent.FrontGround = hover ? HoverFrontGround : FrontGround;
                }));

            GetComponent<LogicGuiComponent>().EventParts.Add(GuiComponentSupportEvent.MouseClick,
                new GuiComponentEventPart());
        }
    }
}
