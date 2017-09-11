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

        protected abstract void ProcessReadFile(ref string[] file);
        protected abstract void ProcessWriteFile(out string[] file);

        internal ResourceAnalyser(string Tag, string FilePath)
        {
            tag = Tag;
            filePath = FilePath;

            LoadAnalyser();
        }

        public void LoadAnalyser()
        {
            string[] contents = System.IO.File.ReadAllLines(filePath);

            DebugLayer.Assert(Decoder.GetInvocationList().Length > 1, WarningType.MoreThanOneDecoderDelegate,
                Decoder.GetInvocationList().Length);

            Decoder?.Invoke(ref contents);

            ProcessReadFile(ref contents);
        }

        public void SaveAnalyser()
        {
            ProcessWriteFile(out string[] contents);

            DebugLayer.Assert(Encoder.GetInvocationList().Length > 1, WarningType.MoreThanOneEncoderDelegate,
                Encoder.GetInvocationList().Length);

            System.IO.File.WriteAllLines(filePath, contents);
        }

        public event DecoderHandler Decoder;
        public event EncoderHandler Encoder;

        public string Tag => tag;
        public string FilePath => filePath;
    }

    public delegate void DecoderHandler(ref string[] file);
    public delegate void EncoderHandler(ref string[] file);

}
