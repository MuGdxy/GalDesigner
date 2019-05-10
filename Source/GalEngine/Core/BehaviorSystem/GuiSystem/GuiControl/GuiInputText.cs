using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiInputText : GuiControl
    {
        public LegalContainer<char> LegalInput { get; set; }

        public GuiInputText() : this(null, 0)
        {
        }

        public GuiInputText(string name, float width) : this(name, width,
            DefaultInputTextProperty.Background,
            DefaultInputTextProperty.Frontground,
            DefaultInputTextProperty.Font)
        {

        }

        public GuiInputText(
            float width,
            Color<float> background,
            Color<float> frontground,
            Font font) : this(null, width, background, frontground, font)
        {

        }

        public GuiInputText(
            string name,
            float width,
            Color<float> background,
            Color<float> frontground,
            Font font)
        {
            AddComponent(new LogicGuiComponent());
            AddComponent(new InputTextGuiComponent(width, background, frontground, font));

            LegalInput = GuiProperty.TextLegalInput;

            var logicComponent = GetComponent<LogicGuiComponent>();
            
            logicComponent.EventParts.Add(GuiComponentSupportEvent.Focus, new GuiComponentFocusEventPart());
            logicComponent.EventParts.Add(GuiComponentSupportEvent.Input, new GuiComponentEventPart(
                (control, eventArg) =>
                {
                    var inputArg = eventArg as GuiComponentInputEvent;



                }));
        }
    }
}
