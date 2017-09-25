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
            Unknown,
            resList,
            gsConfig
        }

        private Dictionary<string, ResourceAnalyser> resList;
        private Dictionary<string, ResourceAnalyser> configList;

        private static void ProcessSentence(string value, int Line, string FileTag)
        {

        }

        private static void ProcessBlock(string blockContents, BlockType blockType, int Line, string FileTag)
        {

        }

        private static BlockType GetBlockType(string typeName, int Line, string FileTag)
        {
            switch (typeName)
            {
                case "resList":
                    return BlockType.resList;
                case "gsConfig":
                    return BlockType.gsConfig;
                default:
                    DebugLayer.ReportError(ErrorType.InvaildFileType, Line, FileTag);
                    return BlockType.Unknown;
            }
        }

        internal BuildListAnalyser(string Tag, string FilePath) : base(Tag, FilePath)
        {
            resList = new Dictionary<string, ResourceAnalyser>();
            configList = new Dictionary<string, ResourceAnalyser>();
        }

        protected override void ProcessReadFile(ref string contents)
        {
            int line = 0;
            int blockStartLine = 0;
           
            bool InBlock = false;

            string currentBlock = "";
            string typeName = "";

            foreach (var item in contents)
            {
                if (InBlock is true)
                    currentBlock += item;

                switch (item)
                {
                    case '\n':
                        line++;
                        break;

                    case '{':
#if DEBUG
                        DebugLayer.Assert(InBlock is true, ErrorType.InvalidResourceFormat, line, FilePath);
#endif
                        InBlock = true;
                        blockStartLine = line;
                        break;

                    case '}':
#if DEBUG
                        DebugLayer.Assert(InBlock is false, ErrorType.InvalidResourceFormat, line, FilePath);
#endif

                        BlockType blockType = GetBlockType(typeName, blockStartLine, Tag);

                        ProcessBlock(currentBlock, blockType, blockStartLine, Tag);

                        typeName = "";
                        currentBlock = "";
                        blockStartLine = 0;
                        InBlock = false;
                        break;
                    default:
                        if (Utilities.IsAlpha(item) is true && InBlock is false)
                            typeName += item;
                        break;
                }
            }

        }

        protected override void ProcessWriteFile(out string contents)
        {
            throw new NotImplementedException();
        }
    }
}
