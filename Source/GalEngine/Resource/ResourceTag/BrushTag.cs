using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Presenter;

namespace GalEngine
{
    class BrushTag : ResourceTag
    {
        private Vector4 brushColor;
        
        public BrushTag(string Tag, Vector4 color) : base(Tag)
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
            if (resource is null) return;

            (resource as CanvasBrush).Dispose();
            resource = null;
        }

        public Vector4 Color => brushColor;
    }
}
