using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    class ScriptFile
    {
        private System.IO.Stream stream;

        private string filePath;

        private string code;

        public ScriptFile(string FilePath, System.IO.Stream Stream)
        {
            filePath = FilePath;

            stream = Stream;

            var buffer = new byte[stream.Length];

            stream.Read(buffer, 0, buffer.Length);

            code = Encoding.UTF8.GetString(buffer);
        }

        public string FilePath => filePath;

        public string Code => code;

        public System.IO.Stream Stream => stream;
    }
}
