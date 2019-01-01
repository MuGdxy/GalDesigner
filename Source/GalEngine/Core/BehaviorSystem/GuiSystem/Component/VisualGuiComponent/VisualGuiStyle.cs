using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class VisualGuiFrame
    {
        public float Padding { get; set; }
        public Color<float> Color { get; set; }

        public VisualGuiFrame(float padding = 1.0f)
        {
            //default padding: 1.0f
            //default color: (0.5f, 0.5f, 0.5f, 1.0f)
            Padding = padding;
            Color = new Color<float>(0.5f, 0.5f, 0.5f, 1.0f);
        }

        public VisualGuiFrame(float padding, Color<float> color)
        {
            Padding = padding;
            Color = color;
        }
    }

    public class VisualGuiStyle
    {
        public float Opacity { get; set; }

        public VisualGuiStyle(float opacity = 1.0f)
        {
            Opacity = opacity;
        }
    }
}
