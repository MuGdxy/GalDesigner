using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class ConfigAnalyser : ResourceAnalyser
    {
        enum ValueType
        {
            Int,
            Bool,
            Float,
            String
        }

        internal ConfigAnalyser(string Tag, string FilePath) : base(Tag, FilePath)
        {
            
        }

        protected override void ProcessReadFile(ref string[] contents)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessWriteFile(out string[] contents)
        {
            throw new NotImplementedException();
        }
    }
}
