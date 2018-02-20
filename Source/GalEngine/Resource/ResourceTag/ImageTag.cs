using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    class ImageTag : ResourceTag
    {
        private string filePath;

        public ImageTag(string Tag, string FilePath) : base(Tag)
        {
            filePath = FilePath;
        }

        protected override void ActiveResource(ref object resource)
        {
            if (resource is null)
            {
                DebugLayer.Assert(System.IO.File.Exists(filePath) is false, ErrorType.FileIsNotExist, filePath);

                resource = new CanvasImage(filePath);
            }
        }

        protected override void DiposeResource(ref object resource)
        {
            Utilities.Dipose(ref resource);
        }

        public string FilePath => filePath;
    }
}
