using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    class BuildListAnalyser : ResourceAnalyser
    {
        private enum BlockType
        {
            resList,
            gsConfig
        }

        private Dictionary<string, ResourceAnalyser> resList;
        private Dictionary<string, ResourceAnalyser> configList;

        private void ProcessBlock(string blockContents, BlockType blockType)
        {

        }

        internal BuildListAnalyser(string Tag, string FilePath) : base(Tag, FilePath)
        {
            resList = new Dictionary<string, ResourceAnalyser>();
            configList = new Dictionary<string, ResourceAnalyser>();
        }

        protected override void ProcessReadFile(ref string contents)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessWriteFile(out string contents)
        {
            throw new NotImplementedException();
        }
    }
}
