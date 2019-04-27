using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class RectangleGuiComponent : VisualGuiComponent
    {
        public Color<float> Color { get; set; }
        public GuiRenderMode RenderMode { get; set; }
        public float Padding { get; set; }

        public RectangleGuiComponent() : this(new RectangleShape(), new Color<float>(0, 0, 0, 1), GuiRenderMode.Solid)
        {

        }

        public RectangleGuiComponent(RectangleShape shape, Color<float> color, GuiRenderMode renderMode, float padding = 2.0f)
            : base(shape)
        {
            Color = color;
            RenderMode = renderMode;
            Padding = 2.0f;
        }
    }
}
