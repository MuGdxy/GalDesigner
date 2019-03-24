using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class FrameVisualGuiComponent : VisualGuiComponent
    {
        public Size<float> Size { get; set; }
        public Color<float> Color { get; set; }
        public float Padding { get; set; }

        public FrameVisualGuiComponent(Size<float> size, Color<float> color, float padding = 2.0f)
        {
            //frame visual gui component, it presents the rectangle 
            Size = size;
            Color = color;
            Padding = padding;
        }
    }
}
