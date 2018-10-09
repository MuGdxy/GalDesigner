using System;
using System.IO;
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

        public Bitmap(int Width = 0, int Height = 0)
        {
            size = new Size(Width, Height);

            if (Width * Height == 0) return;

            Systems.Graphics.CreateBitmap(Width, Height, out resource);
        }

        public Bitmap(Size Size) : this(Size.Width, Size.Height)
        {

        }

        public Bitmap(Stream BitmapStream)
        {
            Systems.Graphics.CreateBitmap(BitmapStream, out resource, out size);
        }

        public void CopyFromMemroy(byte[] bytes)
        {
            Systems.Graphics.CopyBitampFromMemory(bytes, ref resource);
        }

        public void Dispose()
        {
            Systems.Graphics.DestoryBitmap(ref resource);
        }
    }
}
