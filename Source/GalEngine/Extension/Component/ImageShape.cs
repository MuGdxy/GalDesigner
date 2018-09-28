using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Extension
{
    public class ImageShape : Shape
    {
        private Size size;
        private string imageName;

        public ImageShape(Size size, string imageName)
        {
            this.size = size;
            this.imageName = imageName;

            this.baseComponentType = typeof(ImageShape);
        }

        public Size Size { get => size; set => size = value; }
        public string ImageName { get => imageName; set => imageName = value; }
    }
}
