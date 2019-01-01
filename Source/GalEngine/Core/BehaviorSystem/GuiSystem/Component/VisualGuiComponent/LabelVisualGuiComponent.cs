using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class LabelVisualGuiStyle : VisualGuiStyle
    {
        public VisualGuiFrame GuiFrame { get; set; }
        public Color<float> BackGround { get; set; }

        public LabelVisualGuiStyle(float opacity = 1.0f) : base(opacity)
        {
            //use default setting
            GuiFrame = new VisualGuiFrame();
            BackGround = new Color<float>(0.9f, 0.9f, 0.9f, 1.0f);
        }

        public LabelVisualGuiStyle(float opacity, VisualGuiFrame guiFrame, Color<float> backGround) : base(opacity)
        {
            GuiFrame = guiFrame;
            BackGround = backGround;
        }
    }

    public class LabelVisualGuiComponent : VisualGuiComponent
    {
        public string Text { get; set; }
        public Size<float> Size { get; set; }
        public LabelVisualGuiStyle Style { get; set; }

        public LabelVisualGuiComponent(string text, Size<float> size = null)
        { 
            BaseComponentType = typeof(LabelVisualGuiComponent);

            Text = text; Size = size; Style = new LabelVisualGuiStyle();
        }
    }
}