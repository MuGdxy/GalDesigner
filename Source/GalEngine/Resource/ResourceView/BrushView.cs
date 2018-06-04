using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Presenter;

namespace GalEngine
{
    class BrushView : ResourceView
    {
        private Vector4 brushColor;

        public BrushView(string name, Vector4 color) : base(name)
        {
            brushColor = color;
        }

        public Vector4 Color => brushColor;

        public CanvasBrush Source => Resource as CanvasBrush;
    }
}
