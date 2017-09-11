using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    class ImageResourceTag : ResourceTag
    {
        private string filePath;

        public ImageResourceTag(string Tag, string FilePath) : base(Tag)
        {
            filePath = FilePath;
        }

        protected override void ActiveResource(ref object resource)
        {
            if (resource is null)
            {
                resource = new CanvasImage(filePath);
            }
        }

        protected override void DiposeResource(ref object resource)
        {
            if (resource is null) return;

            (resource as CanvasImage).Dispose();
            resource = null;
        }
    }
}
