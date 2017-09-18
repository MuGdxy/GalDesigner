using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class ResourceAnalyser
    {
        private string filePath;
        private string tag;

        protected abstract void ProcessReadFile(ref string[] contents);
        protected abstract void ProcessWriteFile(out string[] contents);

        internal ResourceAnalyser(string Tag, string FilePath)
        {
            tag = Tag;
            filePath = FilePath;

            GlobalAnalyser.SetValue(this);
        }

        public void LoadAnalyser()
        {
#if DEBUG
            DebugLayer.Assert(System.IO.File.Exists(filePath) is false,
                 ErrorType.FileIsNotExist, filePath);
#endif
            string[] contents = System.IO.File.ReadAllLines(filePath);

            ProcessReadFile(ref contents);
        }

        public void SaveAnalyser()
        {
            ProcessWriteFile(out string[] contents);
            
            System.IO.File.WriteAllLines(filePath, contents);
        }

        public string Tag => tag;
        public string FilePath => filePath;
    }

}
