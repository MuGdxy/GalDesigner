using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Size
    {
        private int width;
        private int height;

        public Size(int Width = 0, int Height = 0)
        {
            width = Width;
            height = Height;
        }

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
    }
}
