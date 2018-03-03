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

        public ImageView(string name, string FilePath) : base(name)
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
