﻿using System;
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

        public Bitmap(int Width, int Height)
        {
            Systems.Graphics.CreateBitmap(Width, Height, out resource);

            size = new Size(Width, Height);
        }

        public Bitmap(Size Size) : this(Size.Width, Size.Height)
        {

        }

        public Bitmap(Stream BitmapStream)
        {
            Systems.Graphics.CreateBitmap(BitmapStream, out resource, out size);
        }

        public void Dispose()
        {
            Systems.Graphics.DestoryBitmap(ref resource);
        }
    }
}
