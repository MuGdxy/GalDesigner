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

        private string fileName;

        public ScriptFile(string FileName,System.IO.Stream Stream)
        {
            fileName = FileName;

            stream = Stream;
        }

        public string FileName => fileName;

        public System.IO.Stream Stream => stream;
    }
}
