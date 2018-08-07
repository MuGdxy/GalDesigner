using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Bitmap : IDisposable
    {
        internal object resource = null;

        private Size size;

        public Size Size => size;

        public Bitmap(int Width, int Height)
        {
            System.Graphics.CreateBitmap(ref resource, Width, Height);

            size = new Size(Width, Height);
        }

        public Bitmap(Size Size) : this(Size.Width, Size.Height)
        {

        }

        public void Dispose()
        {
            System.Graphics.DestoryBitmap(ref resource);
        }
    }
}
