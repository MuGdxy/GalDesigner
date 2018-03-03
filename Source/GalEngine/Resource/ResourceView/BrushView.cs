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

        protected override void ActiveResource(ref object resource)
        {
            if (resource is null)
            {
                resource = new CanvasBrush(brushColor.X, brushColor.Y, brushColor.Z,
                    brushColor.W);
            }
        }

        protected override void DiposeResource(ref object resource)
        {
            Utilities.Dipose(ref resource);
        }

        public Vector4 Color => brushColor;
    }
}
