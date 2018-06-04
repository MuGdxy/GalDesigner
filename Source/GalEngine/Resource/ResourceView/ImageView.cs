using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    class ImageView : ResourceView
    {
        private string filePath;

        private float left = 0;
        private float top = 0;
        private float right = 0;
        private float bottom = 0;

        public ImageView(string name, string FilePath) : base(name)
        {
            filePath = FilePath;
        }

        public string FilePath => filePath;

        public CanvasImage Source => Resource as CanvasImage;

        public float Left { get => left; set => left = value; }
        public float Top { get => top; set => top = value; }
        public float Right { get => right; set => right = value; }
        public float Bottom { get => bottom; set => bottom = value; }

        public float Size => (right - left) * (bottom - top);
    }
}
