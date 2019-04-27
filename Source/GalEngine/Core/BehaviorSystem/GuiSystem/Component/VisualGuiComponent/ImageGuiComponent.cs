using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class ImageGuiComponent : VisualGuiComponent
    {
        public Image Image { get; set; }
        public float Opacity { get; set; }

        public ImageGuiComponent() : this(new RectangleShape(), null)
        {

        }

        public ImageGuiComponent(Image image) : 
            this(new RectangleShape(new Size<float>(image.Size.Width, image. Size.Height)), image)
        {

        }

        public ImageGuiComponent(RectangleShape shape, Image image, float opacity = 1.0f) : base(shape)
        {
            BaseComponentType = typeof(ImageGuiComponent);

            Image = image;
            Opacity = opacity;
        }
    }
}
